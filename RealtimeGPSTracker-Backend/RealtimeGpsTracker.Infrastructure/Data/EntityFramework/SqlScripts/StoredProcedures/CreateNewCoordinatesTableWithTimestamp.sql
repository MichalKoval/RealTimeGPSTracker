CREATE PROCEDURE [CreateNewCoordinatesTableWithTimestamp]
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
END