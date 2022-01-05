CREATE PROCEDURE [GpsDevice_AspNetUserId] 
	@deviceId NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

	-- Ake je Id uzivatela, ktory vlastni zariadenie s danym Id
	SELECT u.Id
	FROM [AspNetUsers] u
		inner join [GpsDevices] gd on gd.UserId = u.Id 
	WHERE gd.GpsDeviceId = @deviceId
END