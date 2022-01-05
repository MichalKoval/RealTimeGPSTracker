using System;
using System.Net.NetworkInformation;

namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.SqlScripts
{
    public static class StoredProcedure
    {
		private static readonly string aspNetUsers_Delete =
@"CREATE PROCEDURE [AspNetUsers_Delete]
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
END";
		
		private static readonly string createCoordinatesTable =
@"CREATE PROCEDURE [CreateCoordinatesTable]
	-- new name for table
	@TableName nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = @TableName and xtype = 'U')
	BEGIN

		DECLARE @CreateTableSQL nvarchar(1024)

		SET @CreateTableSQL = 'CREATE TABLE [' + @TableName + '](
			[GpsCoordinateId] [nvarchar](450) NOT NULL PRIMARY KEY CLUSTERED,
			[Lg] [nvarchar](64) NOT NULL,
			[Lt] [nvarchar](64) NOT NULL,
			[Speed] [nvarchar](64) NULL,
			[Time] [datetime2](7) NOT NULL,
			[TripId] [nvarchar](450) NOT NULL
		)
		ALTER TABLE [' + @TableName + '] WITH CHECK ADD  CONSTRAINT [FK_' + @TableName + '_Trips_TripId] FOREIGN KEY([TripId]) REFERENCES [Trips] ([TripId])
		ALTER TABLE [' + @TableName + '] CHECK CONSTRAINT [FK_' + @TableName + '_Trips_TripId]'

		EXEC(@CreateTableSQL)
	
	END
END";

        private static readonly string createNewCoordinatesTableWithTimestamp =
@"CREATE PROCEDURE [CreateNewCoordinatesTableWithTimestamp]
	@TableName nvarchar(64) OUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE
		@CurrentYear nvarchar(4),
		@CurrentMonth nvarchar(2),
		@NewTableName nvarchar(64),
		@FirstDayOfPreviousMonth datetime2(7),
		@FirstDayOfCurrentMonth datetime2(7),
		@FirstDayOfNextMonth datetime2(7),
		@LastDayOfPreviousMonth datetime2(7),
		@LastDayOfCurrentMonth datetime2(7),
		@LastDayOfNextMonth datetime2(7)

	SELECT
		@CurrentYear = YEAR(GETUTCDATE()),
		@CurrentMonth = MONTH(GETUTCDATE()),
		@NewTableName = 'GpsCoordinates_' + @CurrentYear + '_' + @CurrentMonth,
		@FirstDayOfPreviousMonth = DATEADD(mm, DATEDIFF(mm, 0, GETUTCDATE()) - 1, 0),
		@FirstDayOfCurrentMonth = DATEADD(mm, DATEDIFF(mm, 0, GETUTCDATE()), 0),
		@FirstDayOfNextMonth = DATEADD(mm, DATEDIFF(mm, 0, GETUTCDATE()) + 1, 0),
		@LastDayOfPreviousMonth = DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, GETUTCDATE()), 0)),
		@LastDayOfCurrentMonth = DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, GETUTCDATE()) + 1, 0)),
		@LastDayOfNextMonth = DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, GETUTCDATE()) + 2, 0))

	
	IF NOT EXISTS(
		SELECT 1
		FROM [GpsCoordinates_History]
		WHERE [TableName] = @NewTableName
	)
	BEGIN
		-- creates new table based on current MONTH and YEAR
		EXEC CreateCoordinatesTable @NewTableName;

		-- adds Timestamp to GpsCoordinates_History about creation of a new table
		INSERT INTO [GpsCoordinates_History] ([GpsCoordinatesHistoryId], [From], [To], [TableName])
		VALUES (CAST(NEWID() AS NVARCHAR(50)), @FirstDayOfCurrentMonth, @LastDayOfCurrentMonth, @NewTableName)
	END

	SET @TableName = @NewTableName
END";

        private static readonly string gpsCoordinates_Delete =
@"CREATE PROCEDURE [GpsCoordinates_Delete]
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
		DELETE FROM [' + @TableNameFirst + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + ''' 
			AND [Time] >= CAST(''' + CONVERT(NVARCHAR(19), @From, 120) + ''' AS DATETIME2) 
			AND [Time] <=  CAST(''' + CONVERT(NVARCHAR(19), @To, 120) + ''' AS DATETIME2)
		'
	END

	IF (@RowCnt = 2)
	BEGIN
		SELECT TOP 1 @TableNameFirst = [Name] FROM @TableNames ORDER BY Name
		SELECT TOP 1 @TableNameLast = [Name] FROM @TableNames ORDER BY Name DESC

		SET @SelectQuery = '
		DELETE FROM [' + @TableNameFirst + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + ''' 
			AND [Time] >= CAST(''' + CONVERT(NVARCHAR(19), @From, 120) + ''' AS DATETIME2) 

		DELETE FROM [' + @TableNameLast + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + '''
			AND [Time] <= CAST(''' + CONVERT(NVARCHAR(19), @To, 120) + ''' AS DATETIME2)
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
			STUFF((SELECT ' ' + 'DELETE FROM [' + x.[Name] + '] WHERE TripId = ''' + CAST(@TripId AS NVARCHAR(50)) + ''' '
			FROM @TableNames2 x
				FOR XML PATH('')), 1, 1, '')
		FROM @TableNames2 

		SET @SelectQuery = '
		DELETE FROM [' + @TableNameFirst + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + ''' 
			AND [Time] >= CAST(''' + CONVERT(NVARCHAR(19), @From, 120) + ''' AS DATETIME2) 
		' 
		+ @OtherSelects + '	
		DELETE FROM [' + @TableNameLast + '] 
		WHERE [TripId] = ''' + CAST(@TripId AS NVARCHAR(50)) + '''
			AND [Time] <= CAST(''' + CONVERT(NVARCHAR(19), @To, 120) + ''' AS DATETIME2)
		'
	END

	--PRINT (@SelectQuery)
	EXEC(@SelectQuery)
END";

		private static readonly string gpsCoordinates_DeleteByTrip =
@"CREATE PROCEDURE [GpsCoordinates_DeleteByTrip]
     @TripId AS NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #TableNames(
		[Name] NVARCHAR(64)
	)
	DECLARE @RowCnt AS INT

	INSERT INTO #TableNames ([Name])
		SELECT TableName
		FROM [GpsCoordinates_History]
	SET @RowCnt = @@ROWCOUNT

	DECLARE @DeleteQuery AS NVARCHAR(MAX)
	
	DECLARE @TableName AS NVARCHAR(64)
	DECLARE @RowCount AS INT = 1
	WHILE(@RowCount > 0)
	BEGIN
		SET @TableName = NULL
				
		SELECT TOP 1 @TableName = [Name] FROM #TableNames
		
		IF (@TableName IS NOT NULL)
		BEGIN
			SET @DeleteQuery = '
			DELETE FROM [' + @TableName + '] 
			WHERE [TripId] = ''' + @TripId + ''''

			EXEC(@DeleteQuery)
						
			DELETE FROM #TableNames
			WHERE [Name] = @TableName
			SET @RowCount = @@ROWCOUNT
		END
		ELSE
		BEGIN
			SET @RowCount = 0
		END
	END	
END";

		private static readonly string gpsCoordinates_GetData =
@"CREATE PROCEDURE [GpsCoordinates_GetData]
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
END";

        private static readonly string gpsCoordinates_Insert =
@"CREATE PROCEDURE [GpsCoordinates_Insert]
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
END";

        private static readonly string gpsDevices_GetCount =
@"CREATE PROCEDURE [GpsDevices_GetCount]
	@UserId NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT d.[Status] 
			,COUNT(1) as [Count]
	FROM [GpsDevices] d
	WHERE d.UserId = @UserId
	GROUP BY d.[Status]
END";

		private static readonly string gpsDevices_Delete =
@"CREATE PROCEDURE [GpsDevices_Delete]
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
END";

		private static readonly string gpsDevices_RefreshStatus =
@"CREATE PROCEDURE [GpsDevices_RefreshStatus]
AS
BEGIN
	SET NOCOUNT ON;

	;WITH [GpsDeviceDiff] AS (
		SELECT t.[GpsDeviceId], DATEDIFF(MINUTE, MAX(t.[End]), GETDATE()) AS Diff --, MAX(t.[End]) AS [MaxEnd]
		FROM [Trips] AS t
		GROUP BY t.[GpsDeviceId]
	)
	UPDATE d
	SET [Status] = CASE WHEN gdd.[GpsDeviceId] IS NULL THEN 0
						WHEN gdd.[GpsDeviceId] IS NOT NULL AND gdd.[Diff] <= 5 THEN 1
						WHEN gdd.[GpsDeviceId] IS NOT NULL AND gdd.[Diff] > 5 AND gdd.[Diff] <= 20 THEN 2
						WHEN gdd.[GpsDeviceId] IS NOT NULL AND gdd.[Diff] > 20 THEN 0
						ELSE d.[Status]
				   END
	FROM [GpsDevices] d
		LEFT JOIN [GpsDeviceDiff] AS gdd ON gdd.GpsDeviceId = d.GpsDeviceId
	WHERE d.[Status] != 3
END";


		private static readonly string gpsDevice_AspNetUserId =
@"CREATE PROCEDURE [GpsDevice_AspNetUserId] 
	@deviceId NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

	-- Ake je Id uzivatela, ktory vlastni zariadenie s danym Id
	SELECT u.Id
	FROM [AspNetUsers] u
		inner join [GpsDevices] gd on gd.UserId = u.Id 
	WHERE gd.GpsDeviceId = @deviceId
END";

        private static readonly string gpsDevice_Exists =
@"CREATE PROCEDURE [GpsDevice_Exists] 
	@deviceId NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT *
	FROM [GpsDevices] gd
	WHERE gd.GpsDeviceId = @deviceId
END";

		private static readonly string trips_Delete =
@"CREATE PROCEDURE [Trips_Delete]
     @Id AS NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

	EXEC [GpsCoordinates_DeleteByTrip] @Id

	DELETE t
	FROM [Trips] AS t
	WHERE t.[TripId] = @Id
END";

		public static string AspNetUsers_Delete => aspNetUsers_Delete;
		public static string CreateCoordinatesTable => createCoordinatesTable;
        public static string CreateNewCoordinatesTableWithTimestamp => createNewCoordinatesTableWithTimestamp;
        public static string GpsCoordinates_Delete => gpsCoordinates_Delete;
		public static string GpsCoordinates_DeleteByTrip => gpsCoordinates_DeleteByTrip;
		public static string GpsCoordinates_GetData => gpsCoordinates_GetData;
        public static string GpsCoordinates_Insert => gpsCoordinates_Insert;
        public static string GpsDevices_GetCount => gpsDevices_GetCount;
		public static string GpsDevices_Delete => gpsDevices_Delete;
		public static string GpsDevices_RefreshStatus => gpsDevices_RefreshStatus;
		public static string GpsDevice_AspNetUserId => gpsDevice_AspNetUserId;
        public static string GpsDevice_Exists => gpsDevice_Exists;
		public static string Trips_Delete => trips_Delete;
	}


}
