CREATE PROCEDURE [GpsDevices_GetCount]
	@UserId NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT d.[Status] 
			,COUNT(1) as [Count]
	FROM [GpsDevices] d
	WHERE d.UserId = @UserId
	GROUP BY d.[Status]
END