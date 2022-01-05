using System;
using System.Globalization;
using Newtonsoft.Json;
using static RaspberryPiDaemon.Deprecated.Protocols.NMEA_Protocol;

namespace RaspberryPiDaemon.Entities
{
    public class GpsCoordinate
    {
        public enum GpsCoordinateType
        {
            NMEA_GNRMC,
            NMEA_GNGGA
        }

        // Cas bude zaznamenany v UTC
        [JsonProperty("dt")]
        public string DateTime { get; set; }

        [JsonProperty("lt")]
        public double Lt { get; set; }

        [JsonProperty("lg")]
        public double Lg { get; set; }

        [JsonProperty("s")]
        public double Speed { get; set; }

        public GpsCoordinate() { }

        public GpsCoordinate(string t, double latitude, double longitude, double s)
        {
            DateTime = t;
            Lt = latitude;
            Lg = longitude;
            Speed = s;
        }

        private static string TryParseDateTime(string timeStr, string dateStr)
        {
            // Spojime jednotlive hodnoty do tvaru "ddMMyy HHmmss"

            string dateTimeStr = dateStr + " " + timeStr.Trim().Split('.')[0];

            // Overime ci su hodnoty v tvare "ddMMyy HHmmss"

            DateTime dateTime;
            if (System.DateTime.TryParseExact(
                    dateTimeStr,
                    "ddMMyy HHmmss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dateTime))
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return null;
            }

        }     

        // Vrati null ak v GNRMC sprave chybali niektore udaje
        public static GpsCoordinate TryParseGNRMC(GNRMC gNRMC)
        {
            // Example: $GNRMC,183848.00,A,4846.45274,N,02147.24757,E,0.033,,010318,,,A*66

            // Skontrolujeme ci su poslane data dostacujuce, predpokladame ze checksum je vporiadku
            if (gNRMC.Status.Equals("A") && gNRMC.PositionFixMode.Equals("A"))
            {
                GpsCoordinate gpsCoord = new GpsCoordinate();

                // Prve pozadovane data su spravny datum a cas
                // Vyparsujeme datum a cas
                if ((gpsCoord.DateTime = TryParseDateTime(gNRMC.Time, gNRMC.Date)) == null)
                {
                    return null;
                }

                // Pozadujeme spravnost suradnic
                if (double.TryParse(gNRMC.Latitude, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double latit))
                {
                    // Upravime format suradnic
                    int latCelling = (int)(latit / 100.0);
                    double latDecimal = (latit - latCelling * 100.0) / 60.0;
                    double latitude = latCelling + latDecimal;

                    if (gNRMC.NS.Equals("N"))
                    {
                        gpsCoord.Lt = latitude;
                    }
                    else if (gNRMC.Equals("S"))
                    {
                        gpsCoord.Lt = -1.0 * latitude;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

                if (double.TryParse(gNRMC.Longitude, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double longit))
                {
                    // Upravime format suradnic
                    int longCelling = (int)(longit / 100.0);
                    double longDecimal = (longit - longCelling * 100.0) / 60.0;
                    double longitude = longCelling + longDecimal;

                    if (gNRMC.NS.Equals("E"))
                    {
                        gpsCoord.Lg = longitude;
                    }
                    else if (gNRMC.Equals("W"))
                    {
                        gpsCoord.Lg = -1.0 * longitude;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

                // Nepozadujeme, aby bol dostupny aj udaj o rychlosti (v uzloch), vratime rychlost -1.0 ak sa ziadna rychlost nenamerala
                if (double.TryParse(gNRMC.Speed, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double speed))
                {
                    gpsCoord.Speed = speed;
                }
                else { gpsCoord.Speed = -1.0; }

                // Vratime validne udaje o polohe
                return gpsCoord;
            }
            else
            {
                return null;
            }
        }

        public static GpsCoordinate TryParseGNRMC(GNGGA gNGGA)
        {
            throw new NotImplementedException();
        }
    }
}
