using System;
using System.Collections.Generic;

namespace RaspberryPiDaemon.Deprecated.Protocols
{
    public static class NMEA_Protocol
    {
        public enum MessageType
        {
            DTM, // Datum Reference
            GBQ, // standard message (if the current Talker ID is GB)
            GBS, // GNSS Satellite Fault Detection
            GGA, // Global positioning system fix data
            GLL, // Latitude and longitude, with time of position fix and status
            GLQ, // Poll a standard message(if the current Talker ID is GL)
            GNQ, // Poll a standard message(if the current Talker ID is GN)
            GNS, // GNSS fix data
            GPQ, // Poll a standard message(if the current Talker ID is GP)
            GRS, // GNSS Range Residuals
            GSA, // GNSS DOP and Active Satellites
            GST, // GNSS Pseudo Range Error Statistics
            GSV, // GNSS Satellites in View
            RMC, // Recommended Minimum data
            TXT, // Text Transmission
            VLW, // Dual ground/water distance
            VTG, // Course over ground and Ground speed
            ZDA, // Time and Date


            // NMEA messages list source: https://www.u-blox.com/sites/default/files/products/documents/u-blox8-M8_ReceiverDescrProtSpec_%28UBX-13003221%29.pdf
        }

        private static Dictionary<MessageType, byte> messageTypeStrs = new Dictionary<MessageType, byte>
        {
            // Pred kazdym bytom reprezentujucim NMEA spravu je potrebne pridat hex byte 0xF0
            { MessageType.DTM, 0x0A },
            { MessageType.GBQ, 0x44 },
            { MessageType.GBS, 0x09 },
            { MessageType.GGA, 0x00 },
            { MessageType.GLL, 0x01 },
            { MessageType.GLQ, 0x43 },
            { MessageType.GNQ, 0x42 },
            { MessageType.GNS, 0x0D },
            { MessageType.GPQ, 0x40 },
            { MessageType.GRS, 0x06 },
            { MessageType.GSA, 0x02 },
            { MessageType.GST, 0x07 },
            { MessageType.GSV, 0x03 },
            { MessageType.RMC, 0x04 },
            { MessageType.TXT, 0x41 },
            { MessageType.VLW, 0x0F },
            { MessageType.VTG, 0x05 },
            { MessageType.ZDA, 0x08 }
        };

        public static byte[] GetHexBytes(this MessageType mgsType)
        {
            messageTypeStrs.TryGetValue(mgsType, out byte msgByte);

            return new byte[2] { 0xF0, msgByte };
        }

        private static string calculateNMEACheckSum(string str)
        {
            int checksum = 0;
            int strLength = str.Length;

            for (int i = 0; i < strLength; i++)
            {
                checksum ^= char.ConvertToUtf32(str, i);
            }

            return checksum.ToString("X2");
        }

        private static string stringOrEmpty(string str)
        {
            return str.Length == 0 ? null : str;
        }


        public static GNGGA TryParse_GNGGA(string nmeaMessage)
        {
            return null;
        }

        public class GNGGA
        {

        }

        public static GNRMC TryParse_GNRMC(string nmeaMessage)
        {
            // Prv skontrolujeme checksum prijatej NMEA spravy
            string[] NMEA_Parts = nmeaMessage.Split('*');

            if (NMEA_Parts.Length == 2)
            {
                string NMEA_Body = NMEA_Parts[0].Substring(1);
                string NMEA_Checksum = NMEA_Parts[1].Split('\r')[0];

                if (calculateNMEACheckSum(NMEA_Body).Equals(NMEA_Checksum))
                {
                    string[] NMEA_Body_Parts = NMEA_Body.Split(',');

                    GNRMC gNRMC = new GNRMC();

                    gNRMC.Time = stringOrEmpty(NMEA_Body_Parts[1]);
                    gNRMC.Status = stringOrEmpty(NMEA_Body_Parts[2]);
                    gNRMC.Latitude = stringOrEmpty(NMEA_Body_Parts[3]);
                    gNRMC.NS = stringOrEmpty(NMEA_Body_Parts[4]);
                    gNRMC.Longitude = stringOrEmpty(NMEA_Body_Parts[5]);
                    gNRMC.EW = stringOrEmpty(NMEA_Body_Parts[6]);
                    gNRMC.Speed = stringOrEmpty(NMEA_Body_Parts[7]);
                    gNRMC.CourseOverGround = stringOrEmpty(NMEA_Body_Parts[8]);
                    gNRMC.Date = stringOrEmpty(NMEA_Body_Parts[9]);
                    gNRMC.MagneticVariation = stringOrEmpty(NMEA_Body_Parts[10]);
                    gNRMC.MagneticVariationEW = stringOrEmpty(NMEA_Body_Parts[11]);
                    gNRMC.PositionFixMode = stringOrEmpty(NMEA_Body_Parts[12]);
                    //gNRMC.NavigationStatus = NMEA_Body_Parts[13];

                    return gNRMC;
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
        }

        public class GNRMC
        {
            // Example: $GNRMC,183848.00,A,4846.45274,N,02147.24757,E,0.033,,010318,,,A*66

            public string Time { get; set; }

            // Ci su nazbierane data validne. Ci existuje fix, nie checksum
            public string Status { get; set; }

            public string Latitude { get; set; }

            // North / South indikator
            public string NS { get; set; }

            public string Longitude { get; set; }

            // East / West indikator
            public string EW { get; set; }

            // Rychlost v uzloch
            public string Speed { get; set; }

            // Uhol naklonenia
            public string CourseOverGround { get; set; }

            public string Date { get; set; }

            // Podporovane od ADR v4.10
            public string MagneticVariation { get; set; }

            // Podporovane od ADR v4.10, East / West indikator
            public string MagneticVariationEW { get; set; }

            // Indikator modu pred position fix:
            // --- N -> No fix;
            // --- A -> 2D / 3D GNSS fix;
            // NMEA v2.3 a vyssia
            public string PositionFixMode { get; set; }

            // Ak:  V -> Nie je poskytnuty navigacny stav
            // NMEA v4.1 a vyssia
            //public String NavigationStatus { get; set; }
        }
    }
}

//OLD------------------------------------------------

//public enum MessageType
//{
//    AAM, // Waypoint Arrival Alarm
//    ALM, // Almanac data
//    APA, // Auto Pilot A sentence
//    APB, // Auto Pilot B sentence
//    BOD, // Bearing Origin to Destination
//    BWC, // Bearing using Great Circle route
//    DTM, // Datum being used.
//    GGA, // Fix information
//    GLL, // Lat/Lon data
//    GRS, // GPS Range Residuals
//    GSA, // Overall Satellite data
//    GST, // GPS Pseudorange Noise Statistics
//    GSV, // Detailed Satellite data
//    MSK, // send control for a beacon receiver
//    MSS, // Beacon receiver status information.
//    RMA, // recommended Loran data
//    RMB, // recommended navigation data for gps
//    RMC, // recommended minimum data for gps
//    RTE, // route message
//    TRF, // Transit Fix Data
//    STN, // Multiple Data ID
//    VBW, // dual Ground / Water Spped
//    VTG, // Vector track an Speed over the Ground
//    WCV, // Waypoint closure velocity (Velocity Made Good)
//    WPL, // Waypoint Location information
//    XTC, // cross track error
//    XTE, // measured cross track error
//    ZTG, // Zulu(UTC) time and time to go(to destination)
//    ZDA  // Date and Time

//    // NMEA messages list source: http://www.gpsinformation.org/dale/nmea.htm#GGA
//}
