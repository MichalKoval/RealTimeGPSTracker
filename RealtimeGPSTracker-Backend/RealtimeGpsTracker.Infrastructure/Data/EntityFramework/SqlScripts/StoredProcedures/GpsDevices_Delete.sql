CREATE PROCEDURE [GpsDevices_Delete]
     @Id AS NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;
	
	CREATE TABLE #TripsForDelete(
		[TripId] NVARCHAR(450)
	)

	INSERT INTO #TripsForDelete
		SELECT t.[TripId]
		FROM [GpsDevices] AS d
			INNER JOIN [Trips] AS t ON t.GpsDeviceId = d.GpsDeviceId
		WHERE d.[GpsDeviceId] = @Id

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
			
			DELETE FROM #TripsForDelete
			WHERE TripId = @TripId
			
			SET @RowCount = @@ROWCOUNT
		END
		ELSE
		BEGIN
			SET @RowCount = 0
		END
	END

	DELETE d
	FROM [GpsDevices] AS d
	WHERE d.[GpsDeviceId] = @Id
END