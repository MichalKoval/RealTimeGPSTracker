using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace RaspberryPiDaemon
{
    public class AdvancedSerialPort : SerialPort
    {
        public bool Start()
        {
            try
            {
                
                var sPortNameExists = SerialPort.GetPortNames().Any(name => name == this.PortName);

                if (sPortNameExists)
                {
                    // Pokusime sa otvorit port pre nasledne spracovanie, ak to nevyjde je pouzity inym procesom
                    if (!this.IsOpen)
                    {
                        this.Open();
                        this.DtrEnable = true;
                        this.RtsEnable = true;

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Serial port with name: '" + this.PortName + "' is used by another process.");
                    }
                }
                else
                {
                    Console.WriteLine("Serial port with name: '" + this.PortName + "' doesn't exist. Please set name of the port in appsetting.json file.");
                }

            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Serial port: '" + this.PortName + "' is used by another process.");
            }

            return false;
        }

        public bool Stop()
        {
            try
            {
                if (this.IsOpen)
                {
                    Thread.Sleep(500);
                    this.Close();
                    this.Dispose();
                }

                return true;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Serial port: '" + this.PortName + "' wasn't closed sucessfully.");
            }

            return false;
        }
    }
}
