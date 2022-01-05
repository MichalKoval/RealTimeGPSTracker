CREATE PROCEDURE [AspNetUsers_Delete]
     @Id AS NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #TripsForDelete(
		[TripId] NVARCHAR(450)
	)

	INSERT INTO #TripsForDelete
		SELECT t.[TripId]
		FROM [AspNetUsers] AS anu
			INNER JOIN [GpsDevices] AS d ON d.UserId = anu.Id
			INNER JOIN [Trips] AS t ON t.GpsDeviceId = d.GpsDeviceId
		WHERE anu.[Id] = @Id

	DECLARE @TripId AS NVARCHAR(450)
	DECLARE @RowCount AS INT = 1
	WHILE(@RowCount > 0)
	BEGIN
		SET @TripId = NULL
				
		SELECT TOP 1 @TripId = [TripId] FROM #TripsForDelete 
		
		IF (@TripId IS NOT NULL)
		BEGIN
			EXEC [GpsCoordinates_DeleteByTrip] @TripId

			DELETE t
			FROM [Trips] AS t
			WHERE t.[TripId] = @TripId
			
			DELETE FROM #TripsForDelete WHERE TripId = @TripId
			SET @RowCount = @@ROWCOUNT
		END
		ELSE
		BEGIN
			SET @RowCount = 0
		END
	END

	DELETE d
	FROM [AspNetUsers] AS anu
		INNER JOIN [GpsDevices] AS d ON d.UserId = anu.Id
	WHERE anu.[Id] = @Id

	DELETE anuc
	FROM [AspNetUsers] AS anu
		INNER JOIN [AspNetUserClaims] AS anuc ON anuc.UserId = anu.Id
	WHERE anu.[Id] = @Id

	DELETE anul
	FROM [AspNetUsers] AS anu
		INNER JOIN [AspNetUserLogins] AS anul ON anul.UserId = anu.Id
	WHERE anu.[Id] = @Id

	DELETE anur
	FROM [AspNetUsers] AS anu
		INNER JOIN [AspNetUserRoles] AS anur ON anur.UserId = anu.Id
	WHERE anu.[Id] = @Id

	DELETE anut
	FROM [AspNetUsers] AS anu
		INNER JOIN [AspNetUserTokens] AS anut ON anut.UserId = anu.Id
	WHERE anu.[Id] = @Id

	DELETE anu
	FROM [AspNetUsers] AS anu
	WHERE anu.[Id] = @Id
END