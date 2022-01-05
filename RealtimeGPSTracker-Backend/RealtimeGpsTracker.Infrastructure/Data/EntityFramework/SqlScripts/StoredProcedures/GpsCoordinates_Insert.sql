CREATE PROCEDURE [GpsCoordinates_Insert]
	-- Concatenated string of coordinates about to be saved to the table
	@Coordinates GpsCoordinatesType READONLY
AS
BEGIN
	--DECLARE
	--	@CurrentYear nvarchar(4),
	--	@CurrentMonth nvarchar(2),
	--	@NewTableName nvarchar(64)

	--SELECT
	--	@CurrentYear = YEAR(GETUTCDATE()),
	--	@CurrentMonth = MONTH(GETUTCDATE()),
	--	@NewTableName = 'GpsCoordinates_' + @CurrentYear + '_' + @CurrentMonth

	---- Overime ci dane dane trip ID je uz v databaze
	--DECLARE @TripId AS NVARCHAR(450)
	--DECLARE @Start AS DATETIME2
	--DECLARE @End AS DATETIME2

	--SELECT TOP 1 @TripId = [TripId] FROM @Coordinates t ORDER BY t.[Time] DESC 
	

	---- Ak trip neexistuje zaregistrujeme novy trip
	--IF NOT EXISTS(SELECT 1 FROM [Trips] WHERE [TripId] = @TripId )
	--BEGIN
	--	SELECT TOP 1 @Start = t.[Time] FROM @Coordinates t ORDER BY t.[Time] ASC 
	--	SELECT TOP 1 @End = t.[Time] FROM @Coordinates t ORDER BY t.[Time] DESC 

	--	INSERT INTO [Trips] (
	--		 [TripId]
	--		,[UserId]
	--		,[Start]
	--		,[End]
	--		,[GpsDeviceId]
	--	)
	--	VALUES (@TripId, NULL, @Start, @End, )
	--END


	--INSERT INTO [GpsCoordinates] ([GpsCoordinateId], [Lg], [Lt], [Speed], [Time], [TripId])
	--	SELECT c.[GpsCoordinateId], c.[Lg], c.[Lt], c.[Speed], c.[Time], c.[TripId]
	--	FROM @Coordinates AS c

	CREATE TABLE #Temporary_GpsCoordinates (
		[GpsCoordinateId] [nvarchar](450) NOT NULL,
		[Time] [datetime2](7) NOT NULL,
		[Lg] [nvarchar](64) NULL,
		[Lt] [nvarchar](64) NULL,
		[Speed] [nvarchar](64) NULL,
		[TripId] [nvarchar](450) NULL
	)

	INSERT INTO #Temporary_GpsCoordinates ([GpsCoordinateId], [Lg], [Lt], [Speed], [Time], [TripId])
		SELECT c.[GpsCoordinateId], c.[Lg], c.[Lt], c.[Speed], c.[Time], c.[TripId]
		FROM @Coordinates AS c

	DECLARE @TripId AS NVARCHAR(450)
	DECLARE @End AS DATETIME2

	SELECT TOP 1 
			@TripId = [TripId]
		,@End = [Time]	   
	FROM #Temporary_GpsCoordinates
	ORDER BY [Time] DESC

	DECLARE @TableName AS NVARCHAR(64)
	EXEC [CreateNewCoordinatesTableWithTimestamp] @TableName OUT

	DECLARE @InsertQuery AS NVARCHAR(MAX)
	SET @InsertQuery = N'
	INSERT INTO [' + @TableName + '] ([GpsCoordinateId], [Lg], [Lt], [Speed], [Time], [TripId])
		SELECT c.[GpsCoordinateId], c.[Lg], c.[Lt], c.[Speed], c.[Time], c.[TripId]
		FROM #Temporary_GpsCoordinates c

	UPDATE [Trips]
	SET  [End] = CAST(''' + CONVERT(NVARCHAR(19), @End, 120) + ''' AS DATETIME2)
		,[InProgress] = 1
	WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + '''
	'
	EXEC(@InsertQuery)
END