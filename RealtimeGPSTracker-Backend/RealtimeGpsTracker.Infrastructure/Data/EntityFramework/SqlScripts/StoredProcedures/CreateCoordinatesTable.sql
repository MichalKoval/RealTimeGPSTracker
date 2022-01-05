CREATE PROCEDURE [CreateCoordinatesTable]
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
END