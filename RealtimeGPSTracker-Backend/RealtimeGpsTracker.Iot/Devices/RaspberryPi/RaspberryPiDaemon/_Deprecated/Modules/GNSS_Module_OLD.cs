using RaspberryPiDaemon.Deprecated.Protocols;
using System;
using System.IO.Ports;

namespace RaspberryPiDaemon.Deprecated.Modules
{
    public class GNSS_Module
    {
        private SerialPort _sPort = null;

        public SerialPort SerialPort
        {
            get { return _sPort; }
            set { _sPort = value; }
        }

        public GNSS_Module()
        {

        }

        public GNSS_Module(SerialPort sp)
        {
            _sPort = sp;
        }

        public void setMessage(SerialPort serialPort, NMEA_Protocol.MessageType msgType, bool enable)
        {
            byte[] messageBytes = UBX_Protocol.generate_UBX_CGF_MSG(msgType, enable);
            string hexMessageBytes = BitConverter.ToString(messageBytes);
            //serialPort.WriteLine("\\x" + hexMessageBytes.Replace("-", "\\x"));
            Console.WriteLine("\\x{0}", hexMessageBytes.Replace("-", "\\x"));
        }


    }
}
