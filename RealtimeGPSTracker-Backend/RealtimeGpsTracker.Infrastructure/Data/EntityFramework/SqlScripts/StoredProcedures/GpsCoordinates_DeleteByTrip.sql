CREATE PROCEDURE [GpsCoordinates_DeleteByTrip]
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
END