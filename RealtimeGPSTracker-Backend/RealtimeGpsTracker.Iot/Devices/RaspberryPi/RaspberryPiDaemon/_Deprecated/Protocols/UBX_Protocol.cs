using System.Collections.Generic;

namespace RaspberryPiDaemon.Deprecated.Protocols
{
    public static class UBX_Protocol
    {
        // Spoji byte polia
        //public static byte[] ConcatByteArrays(params byte[][] arrays)
        //{
        //    return arrays.SelectMany(x => x).ToArray();
        //}

        // Vrati checksum v HEX
        private static byte[] calculate_UBX_CGF_MSG_Checksum(List<byte> bytes)
        {
            byte CK_A = 0;
            byte CK_B = 0;

            int byteArrayLenght = bytes.Count;

            for (int i = 2; i < byteArrayLenght; i++)
            {
                CK_A += bytes[i];
                CK_B += CK_A;
            }

            return new byte[2] { CK_A, CK_B };
        }

        // Metoda generaju UBX_CFG_MSG spravu, konkretne tato UBX_CFG sprava povie GPS modulu, ci chceme prijimat danu NMEA spravu z modulu
        public static byte[] generate_UBX_CGF_MSG(NMEA_Protocol.MessageType msgtype, bool enable)
        {
            // Set NMEA Message Rate(s) using UBX_CGF_MSG
            // Reference: https://www.u-blox.com/sites/default/files/products/documents/u-blox8-M8_ReceiverDescrProtSpec_%28UBX-13003221%29.pdf
            List<byte> UBX_CGF_MSG = new List<byte>();

            // UBX_CGF_MSG header ----------------------------------
            UBX_CGF_MSG.Add(0xB5);
            UBX_CGF_MSG.Add(0x62);

            // UBX_CGF_MSG Class and ID ----------------------------
            UBX_CGF_MSG.Add(0x06); // UBX-CFG-MSG
            UBX_CGF_MSG.Add(0x01); // ID

            // UBX_CGF_MSG Payload lenght --> 8 bytes --------------
            UBX_CGF_MSG.Add(0x08);
            UBX_CGF_MSG.Add(0x00);

            // UBX_CGF_MSG Payload ---------------------------------

            UBX_CGF_MSG.AddRange(msgtype.GetHexBytes());

            /*
             "x,x,x,x,x,x", eg: "0,1,0,0,0,0" --> enable for USART1

             Output message to:
             1, I2C / DDC
             2, USART1
             3, USART2
             4, USB
             5, SPI
             6, reserved

             */

            if (enable)
            {
                UBX_CGF_MSG.AddRange(new byte[6] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 });
            }
            else
            {
                UBX_CGF_MSG.AddRange(new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }

            // UBX_CGF_MSG Checksum -----------------------------------
            UBX_CGF_MSG.AddRange(calculate_UBX_CGF_MSG_Checksum(UBX_CGF_MSG)); //Message Checksum

            return UBX_CGF_MSG.ToArray();
        }
    }
}
