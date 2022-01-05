CREATE PROCEDURE [GpsDevices_RefreshStatus]
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
END