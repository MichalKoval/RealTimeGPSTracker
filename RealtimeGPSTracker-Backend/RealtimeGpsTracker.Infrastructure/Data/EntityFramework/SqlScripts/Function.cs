namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.SqlScripts
{
    public static class Function
    {
        private static readonly string splitStringByDelimiter =
@"CREATE FUNCTION [SplitStringByDelimiter] 
( 
	@SourceString NVARCHAR(MAX), 
	@Delimiter CHAR(1) 
)
-- Return table of strings(each in separate row)
RETURNS @OutputTable TABLE(
	[SplitString] NVARCHAR(MAX) 
) 
BEGIN 
	DECLARE 
		@StartIndex INT,
		@EndIndex INT

	SELECT
		@StartIndex = 1,
		-- Finds index of first delimiter in string
		@EndIndex = CHARINDEX(@Delimiter, @SourceString)

	WHILE @StartIndex <= LEN(@SourceString)
	BEGIN 
		IF @EndIndex = 0  
			SET @EndIndex = LEN(@SourceString) + 1
       
		INSERT INTO @OutputTable ([SplitString])  
			VALUES(SUBSTRING(@SourceString, @StartIndex, @EndIndex - @StartIndex))

		-- Moves start index after the delimiter where supouse to be the next substring
		SET @StartIndex = @EndIndex + 1
		-- Finds next delimiter in source substring which left from original one
		SET @EndIndex = CHARINDEX(@Delimiter, @SourceString, @StartIndex)        
	END 
	RETURN 
END";

        public static string SplitStringByDelimiter => splitStringByDelimiter;
    }


}
