namespace RealtimeGpsTracker.Infrastructure.Data.EntityFramework.SqlScripts
{
    public static class DataType
    {
        private static readonly string gpsCoordinatesType =
@"CREATE TYPE [GpsCoordinatesType] AS TABLE(
	[GpsCoordinateId] [nvarchar](450) NOT NULL,
	[Time] [datetime2](7) NOT NULL,
	[Lg] [nvarchar](64) NULL,
	[Lt] [nvarchar](64) NULL,
	[Speed] [nvarchar](64) NULL,
	[TripId] [nvarchar](450) NULL
)";

		public static string GpsCoordinatesType => gpsCoordinatesType;
    }


}
