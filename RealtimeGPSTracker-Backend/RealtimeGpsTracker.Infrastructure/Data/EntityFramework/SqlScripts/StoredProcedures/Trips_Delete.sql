CREATE PROCEDURE [Trips_Delete]
     @Id AS NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

	EXEC [GpsCoordinates_DeleteByTrip] @Id

	DELETE t
	FROM [Trips] AS t
	WHERE t.[TripId] = @Id
END