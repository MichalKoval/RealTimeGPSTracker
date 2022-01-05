CREATE PROCEDURE [GpsDevice_Exists] 
	@deviceId NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	SELECT *
	FROM [GpsDevices] gd
	WHERE gd.GpsDeviceId = @deviceId
END