CREATE PROCEDURE [GpsCoordinates_GetData]
	@TripId AS NVARCHAR(450)
	,@From AS DATETIME2
	,@To AS DATETIME2
AS
BEGIN
	SET NOCOUNT ON;
	
	--DECLARE @From AS DATETIME2 = CAST('2018-03-15 13:00:00' AS DATETIME2)
	--DECLARE @To AS DATETIME2 = CAST('2018-08-20 13:00:00' AS DATETIME2)
	--DECLARE @TripId AS NVARCHAR(450) = '06FC88D0-56CD-42CF-BF9D-43CE40086B9D'

	DECLARE @TableNames AS TABLE ([Name] NVARCHAR(64))
	DECLARE @RowCnt AS INT

	INSERT INTO @TableNames ([Name])
		SELECT TableName
		FROM [GpsCoordinates_History]
		WHERE   (@From < [To] AND @From > [From])
				OR (@From < [From] AND @To > [To])
				OR (@To > [From]  AND @To < [To])
		ORDER BY [From]
	SET @RowCnt = @@ROWCOUNT

	DECLARE @SelectQuery AS NVARCHAR(MAX)
	DECLARE @TableNameFirst AS NVARCHAR(64)
	DECLARE @TableNameLast AS NVARCHAR(64)

	IF (@RowCnt = 1)
	BEGIN
	
		SELECT @TableNameFirst = [Name] FROM @TableNames

		SET @SelectQuery = '
		SELECT [GpsCoordinateId],[Lg],[Lt],[Speed],[Time],[TripId]	
		FROM [' + @TableNameFirst + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + ''' 
			AND [Time] >= CAST(''' + CONVERT(NVARCHAR(19), @From, 120) + ''' AS DATETIME2) 
			AND [Time] <=  CAST(''' + CONVERT(NVARCHAR(19), @To, 120) + ''' AS DATETIME2)
		ORDER BY [Time] ASC
		'
	END

	IF (@RowCnt = 2)
	BEGIN
		SELECT TOP 1 @TableNameFirst = [Name] FROM @TableNames ORDER BY Name
		SELECT TOP 1 @TableNameLast = [Name] FROM @TableNames ORDER BY Name DESC

		SET @SelectQuery = '
		SELECT [GpsCoordinateId],[Lg],[Lt],[Speed],[Time],[TripId]
		FROM [' + @TableNameFirst + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + ''' 
			AND [Time] >= CAST(''' + CONVERT(NVARCHAR(19), @From, 120) + ''' AS DATETIME2) 
		UNION ALL
		SELECT [GpsCoordinateId],[Lg],[Lt],[Speed],[Time],[TripId]
		FROM [' + @TableNameLast + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + '''
			AND [Time] <= CAST(''' + CONVERT(NVARCHAR(19), @To, 120) + ''' AS DATETIME2)
		ORDER BY [Time] ASC
		'
	END

	IF (@RowCnt > 2)
	BEGIN
		SELECT TOP 1 @TableNameFirst = [Name] FROM @TableNames ORDER BY Name
		SELECT TOP 1 @TableNameLast = [Name] FROM @TableNames ORDER BY Name DESC

		DECLARE @TableNames2 AS TABLE ([Name] NVARCHAR(64))
		INSERT INTO @TableNames2 (Name)
			SELECT [Name]
			FROM @TableNames
			WHERE Name != @TableNameFirst
				AND Name != @TableNameLast

		DECLARE @OtherSelects NVARCHAR(MAX)

		-- STUFF multiple rows into one column, without GROUP BY, must be used TOP 1
		SELECT TOP 1 @OtherSelects = 
			STUFF((SELECT ' ' + 'SELECT [GpsCoordinateId],[Lg],[Lt],[Speed],[Time],[TripId] FROM [' + x.[Name] + '] WHERE TripId = ''' + CAST(@TripId AS NVARCHAR(50)) + ''' UNION ALL '
			FROM @TableNames2 x
				FOR XML PATH('')), 1, 1, '')
		FROM @TableNames2 

		SET @SelectQuery = '
		SELECT [GpsCoordinateId],[Lg],[Lt],[Speed],[Time],[TripId]
		FROM [' + @TableNameFirst + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + ''' 
			AND [Time] >= CAST(''' + CONVERT(NVARCHAR(19), @From, 120) + ''' AS DATETIME2) 
		UNION ALL ' 
		+ @OtherSelects + '	
		SELECT [GpsCoordinateId],[Lg],[Lt],[Speed],[Time],[TripId]
		FROM [' + @TableNameLast + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + '''
			AND [Time] <= CAST(''' + CONVERT(NVARCHAR(19), @To, 120) + ''' AS DATETIME2)
		ORDER BY [Time] ASC
		'
	END

	--PRINT (@SelectQuery)
	EXEC(@SelectQuery)
END