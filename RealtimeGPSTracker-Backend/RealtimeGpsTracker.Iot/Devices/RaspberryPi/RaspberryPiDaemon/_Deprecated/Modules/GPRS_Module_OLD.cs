//#define DEBUG
#define DEBUG_ECHO

using RaspberryPiDaemon.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryPiDaemon.Deprecated.Modules
{
    public class GPRS_Module
    {
        public class ATCommandToSend
        {
            public ATCommand Command { get; set; }
            public Expected_AT_Response ExpectedResponse { get; set; }
            public string Data { get; set; }
            public int RepeatCount { get; set; }

            public ATCommandToSend(ATCommand command, Expected_AT_Response expATResp, int repeatCount)
            {
                Command = command;
                ExpectedResponse = expATResp;
                Data = null;
                RepeatCount = repeatCount;
            }

            public ATCommandToSend(ATCommand command, string data, Expected_AT_Response expATResp, int repeatCount)
            {
                Command = command;
                ExpectedResponse = expATResp;
                Data = data;
                RepeatCount = repeatCount;
            }
        }


        private SerialPort _sPort = null;
        private string _id = null;
        private bool _serverSSLEnabled = false;
        private string _serverAddress = null;
        private string _serverPOSTAddress = null;
        private string _operatorAPN = null;
        private string _operatorAPNUser = null;
        private string _operatorAPNPwd = null;

        public GPRS_Module(
            SerialPort sp,
            string id,
            bool _serverSSLEnabled,
            string serverAddress,
            string serverPOSTAddress,
            string operatorAPN,
            string operatorAPNUser,
            string operatorAPNPwd)
        {
            _sPort = sp;
            _id = id;

            if (id != null && !id.Equals(""))
            {
                _id = id;
            }
            else
            {
                throw new Exception("GPRS_Module: Chyba ID.");
            }

            if (serverAddress != null && !serverAddress.Equals(""))
            {
                _serverAddress = serverAddress;
            }
            else
            {
                throw new Exception("GPRS_Module: Chyba adresa servera.");
            }

            if (serverPOSTAddress == null)
            {
                _serverPOSTAddress = "";
            }
            else
            {
                _serverPOSTAddress = serverPOSTAddress;
            }

            if (operatorAPN != null && !operatorAPN.Equals(""))
            {
                _operatorAPN = operatorAPN;
            }
            else
            {
                throw new Exception("GPRS_Module: Chyba APN operatora.");
            }

            if (operatorAPNUser == null)
            {
                _operatorAPNUser = "";
            }
            else
            {
                _operatorAPNUser = operatorAPNUser;
            }

            if (operatorAPNPwd == null)
            {
                _operatorAPNPwd = "";
            }
            else
            {
                _operatorAPNPwd = operatorAPNPwd;
            }

        }

        private string FindATResponse(string expectedValue, bool mustBeEqual)
        {
            string responseLine;

            try
            {
                while (true)
                {
                    if ((responseLine = _sPort.ReadLine()).Length == 0)
                    {
#if DEBUG_ECHO
                        Console.WriteLine(responseLine);
#endif
                        continue;
                    }
#if DEBUG_ECHO
                    Console.WriteLine(responseLine);
#endif
                    if (mustBeEqual && responseLine.Equals(expectedValue))
                    {
                        return responseLine;
                    }

                    if (!mustBeEqual && responseLine.StartsWith(expectedValue))
                    {
                        return responseLine;
                    }
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("AT odpoved prekrocila casovy limit.");
                return null;
            }
        }

        private string FindATResponse(Regex expectedRegex)
        {
            string responseLine;

            try
            {
                while (true)
                {
                    if ((responseLine = _sPort.ReadLine()).Length == 0)
                    {
#if DEBUG_ECHO
                        Console.WriteLine(responseLine);
#endif
                        continue;
                    }

                    MatchCollection matches = expectedRegex.Matches(responseLine);

                    if (matches.Count > 0)
                    {
                        return matches[0].Value;
                    }
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("AT odpoved prekrocila casovy limit.");
                return null;
            }
        }


        private AT_Response HandleATResponse(Expected_AT_Response expATResp)
        {
            bool satisfied = true;
            List<string> msgs = new List<string>();
            string responseLine;

            foreach (IATResponse iATResponse in expATResp.ExpectedResponses)
            {
                if (iATResponse.GetType() == typeof(AT_Response_String))
                {
                    AT_Response_String aT_Response_String = (AT_Response_String)iATResponse;

                    if ((responseLine = FindATResponse(aT_Response_String.Value, false)) != null)
                    {
                        msgs.Add(responseLine);
                    }
                    else
                    {
                        satisfied = false;
                        msgs.Add("V odpovedi sa nevyskytuje hladany podretazec: '" + aT_Response_String.Value + "'");
                        break;
                    }
                }

                if (iATResponse.GetType() == typeof(AT_Response_Status))
                {
                    AT_Response_Status aT_Response_Status = (AT_Response_Status)iATResponse;


                    if ((responseLine = FindATResponse(aT_Response_Status.Value.ToString(), true)) != null)
                    {
                        msgs.Add(responseLine);
                    }
                    else
                    {
                        satisfied = false;
                        msgs.Add("V odpovedi sa nevyskytuje hladany status: '" + aT_Response_Status.Value + "'");
                        break;
                    }
                }

                if (iATResponse.GetType() == typeof(AT_Response_StringStatus))
                {
                    AT_Response_StringStatus aT_Response_StringStatus = (AT_Response_StringStatus)iATResponse;

                    if ((responseLine = FindATResponse(aT_Response_StringStatus.Value, true)) != null)
                    {
                        msgs.Add(responseLine);
                    }
                    else
                    {
                        satisfied = false;
                        msgs.Add("V odpovedi sa nevyskytuje hladany podretazec a status: '" + aT_Response_StringStatus.Value + "'");
                        break;
                    }
                }

                if (iATResponse.GetType() == typeof(AT_Response_CmdString))
                {
                    AT_Response_CmdString aT_Response_CmdString = (AT_Response_CmdString)iATResponse;

                    if ((responseLine = FindATResponse(aT_Response_CmdString.Value, false)) != null)
                    {
                        msgs.Add(responseLine);
                    }
                    else
                    {
                        satisfied = false;
                        msgs.Add("V odpovedi sa nevyskytuje hladany prikaz a podretazec: '" + aT_Response_CmdString.Value + "'");
                        break;
                    }
                }

                if (iATResponse.GetType() == typeof(AT_Response_Regex))
                {
                    AT_Response_Regex aT_Response_Regex = (AT_Response_Regex)iATResponse;

                    if ((responseLine = FindATResponse(aT_Response_Regex.Value)) != null)
                    {
                        msgs.Add(responseLine);
                    }
                    else
                    {
                        satisfied = false;
                        msgs.Add("V odpovedi sa nevyskytuje hladany regularny vyraz: '" + aT_Response_Regex.Value + "'");
                        break;
                    }
                }
            }


            return new AT_Response(satisfied, msgs);
        }

        // Seriovy port musi byt uz otvoreny pre dalsie spracovanie (samozrejme nesmie byt null)
        public void SendATCommand(ATCommand command, Expected_AT_Response expATResp, out AT_Response atResponse)
        {
            if (_sPort != null)
            {
                if (_sPort.IsOpen)
                {
                    _sPort.Write(command.ToString() + "\r\n");
#if DEBUG
                    Console.WriteLine(command.ToString());
#endif

                    atResponse = HandleATResponse(expATResp);
                }
                else
                {
                    throw new Exception("Seriovy port musi byt otvoreny.");
                }
            }
            else
            {
                throw new Exception("Je potrebne priradit instanciu SerialPortu do 'GPRS_Module.SerialPort'.");
            }
        }

        // Seriovy port musi byt uz otvoreny pre dalsie spracovanie (samozrejme nesmie byt null)
        public void SendATCommand(ATCommand command, string data, Expected_AT_Response expATResp, out AT_Response atResponse)
        {
            if (_sPort != null)
            {
                if (_sPort.IsOpen)
                {
                    _sPort.Write(command.ToString() + "\r\n");
#if DEBUG
                    Console.WriteLine(command.ToString());
#endif

                    Thread.Sleep(300);
                    _sPort.Write(data + "\x1A");

                    Thread.Sleep(300);

                    atResponse = HandleATResponse(expATResp);
                }
                else
                {
                    throw new Exception("Seriovy port musi byt otvoreny.");
                }
            }
            else
            {
                throw new Exception("Je potrebne priradit instanciu SerialPortu do 'GPRS_Module.SerialPort'.");
            }
        }

        public bool SendATCommands(params ATCommandToSend[] aTCommandsToSend)
        {
            List<ATCommandToSend> commandsToSend = aTCommandsToSend.ToList();

            AT_Response aT_Response;

            try
            {
                foreach (ATCommandToSend commandToSend in commandsToSend)
                {
                    // Prikaz zopakujeme len tolko krat kolko je zadane, ak na prvykrat nevysiel
                    bool satisfied = false;
                    for (int i = 0; i < commandToSend.RepeatCount; i++)
                    {
                        if (commandToSend.Data == null)
                        {
                            SendATCommand(commandToSend.Command, commandToSend.ExpectedResponse, out aT_Response);
                        }
                        else
                        {
                            SendATCommand(commandToSend.Command, commandToSend.Data, commandToSend.ExpectedResponse, out aT_Response);
                        }

                        // Vypise sa odpoved na AT prikaz len ak _debugResponses == true
#if DEBUG
                        WriteATResponseMessages(aT_Response);
#endif
                        if (aT_Response.Satisfied)
                        {
                            satisfied = true;
                            break;
                        }
                    }

                    if (!satisfied)
                    {
                        Console.WriteLine("Error: prikaz '{0}'", commandToSend.Command.ToString());
                        return false;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private void WriteATResponseMessages(AT_Response aT_Response)
        {
            foreach (string msg in aT_Response.Messages)
            {
                Console.WriteLine(msg);
            }
        }

        public void Start()
        {
            try
            {
                if (_sPort != null)
                {
                    var sPortNameExists = SerialPort.GetPortNames().Any(name => name == _sPort.PortName);

                    if (sPortNameExists)
                    {
                        // Pokusime sa otvorit port pre nasledne spracovanie, ak to nevyjde je pouzity inym procesom
                        if (!_sPort.IsOpen)
                        {
                            _sPort.NewLine = "\r\n";
                            _sPort.Open();
                            _sPort.DtrEnable = true;
                            _sPort.RtsEnable = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Seriovy port s menom: '" + _sPort.PortName + "' neexistuje. Zmente nastavenia v subore 'GPS_Tracker_Config.json' pre GPRS serial port.");
                    }

                }

            }
            catch (IOException)
            {
                Console.WriteLine("IOException");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Seriovy port: '" + _sPort.PortName + "' je prave pouzivany inym procesom.");
            }
        }

        public void Stop()
        {
            try
            {
                if (_sPort != null && _sPort.IsOpen)
                {
                    Thread.Sleep(400);
                    _sPort.Close();
                    _sPort.Dispose();
                }

            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Seriovy port: '" + _sPort.PortName + "' sa nepodarilo uzavriet.");
            }
        }

        public bool InitializeModem()
        {
            Console.WriteLine("\n### Initializing GPRS Modem... ###\n");

            bool result = false;

            //    SendATCommands(
            //    new ATCommandToSend(
            //        new ATCommand(ATCommandType.EMPTY, ATResponseType.OK),
            //        2
            //    ),
            //    new ATCommandToSend(
            //        new ATCommand(ATCommandType.ATE1, ATResponseType.OK),
            //        2
            //    ),
            //    new ATCommandToSend(
            //        new ATCommand(ATCommandType.CPIN, ATResponseType.OK, true),
            //        new Expected_AT_Response(
            //            new AT_Response_CmdString(ATCommandType.CPIN, "READY"),
            //            new AT_Response_Status(AT_Response_Status_Type.OK)
            //        ),
            //        2
            //    ),
            //    new ATCommandToSend(
            //        new ATCommand(ATCommandType.CREG, true),
            //        new Expected_AT_Response(
            //            new AT_Response_CmdString(ATCommandType.CREG, "0,1"),
            //            new AT_Response_Status(AT_Response_Status_Type.OK)
            //        ),
            //        2
            //    ),
            //    new ATCommandToSend(
            //        new ATCommand(ATCommandType.CGATT, true),
            //        new Expected_AT_Response(
            //            new AT_Response_CmdString(ATCommandType.CGATT, "1"),
            //            new AT_Response_Status(AT_Response_Status_Type.OK)
            //        ),
            //        2
            //    ),
            //    new ATCommandToSend(
            //        new ATCommand(ATCommandType.CSQ),
            //        new Expected_AT_Response(
            //            new AT_Response_CmdString(ATCommandType.CSQ, ""),
            //            new AT_Response_Status(AT_Response_Status_Type.OK)
            //        ),
            //        2
            //    )
            //);

            return result;
        }

        public bool InitializeGPRSConnection()
        {
            AT_Response aT_Response;

            try
            {
                // Nastavime 'Bearer Profile', info: https://researchdesignlab.com/projects/AN_SIM900_FTP_HTTP_AT_COMMANDS_USER_GUIDE_beta_V1.00.pdf

                //SendATCommand(
                //    new ATCommand(ATCommandType.SAPBR, "3", "1", "Contype", "GPRS"),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    out aT_Response);
                //WriteATResponseMessages(aT_Response);
                //if (!aT_Response.Satisfied) { return false; }

                //// Nastavime pristupovy bod pre GPRS
                //SendATCommand(
                //    new ATCommand(ATCommandType.SAPBR, "3", "1", "APN", _operatorAPN),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    out aT_Response);
                //WriteATResponseMessages(aT_Response);
                //if (!aT_Response.Satisfied) { return false; }

                //// Zatvorime GPRS spojenie ak uz bolo pred tym otvorene
                //SendATCommand(
                //    new ATCommand(ATCommandType.SAPBR, "0", "1"),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    out aT_Response);
                //WriteATResponseMessages(aT_Response);

                //// Otvorime GPRS spojenie
                //SendATCommand(
                //    new ATCommand(ATCommandType.SAPBR, "1", "1"),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    out aT_Response);
                //WriteATResponseMessages(aT_Response);
                //if (!aT_Response.Satisfied) { return false; }

                //// Zistime ci nam bola pridelena IP adresa
                //SendATCommand(
                //    new ATCommand(ATCommandType.SAPBR, "2", "1"),
                //    new Expected_AT_Response(
                //        new AT_Response_CmdString(ATCommandType.SAPBR, "1,1,\""),
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    out aT_Response);
                //WriteATResponseMessages(aT_Response);
                //if (!aT_Response.Satisfied) { return false; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public bool CloseGPRSConnection()
        {
            AT_Response aT_Response;

            try
            {
                //// Zatvorime GPRS spojenie, nebudeme zistovar ci bol ERROR alebo OK, ak bol ERROR, GPRS spojenie bolo uz predtym uzavrete
                //SendATCommand(
                //    new ATCommand(ATCommandType.SAPBR, "0", "1"),
                //    new Expected_AT_Response(),
                //    out aT_Response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        // Metoda odosle HTTP Post za pomoci TCP protokolu
        public bool SendHTTPPOST(string data, out string serverResponse)
        {
            serverResponse = null;

            Console.WriteLine("\n### Sending HTTP POST... ###\n");

            bool result = SendATCommands(
                // Zistime ci je k dispozicii GPRS spojenie
                //new ATCommandToSend(
                //    new ATCommand(ATCommandType.CGATT, true),
                //    new Expected_AT_Response(
                //        new AT_Response_CmdString(ATCommandType.CGATT, "1"),
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    2
                //),
                // Pokusime sa vypnut predchadzajuce TCP spojenie, ak sa z nejake dovodu pred tym nevyplo aby slo nastavit APN
                //new ATCommandToSend(
                //    new ATCommand(ATCommandType.CIPSHUT),
                //    new Expected_AT_Response(
                //        new AT_Response_StringStatus("SHUT", AT_Response_Status_Type.OK)
                //    ),
                //    3
                //),
                // Nastavit APN operatora, popripade naviac meno a heslo
                //new ATCommandToSend(
                //    new AT_Command(AT_CommandType.CSTT, _operatorAPN, _operatorAPNUser, _operatorAPNPwd),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    2
                //),
                // Otvorime GPRS spojenie
                //new ATCommandToSend(
                //    new ATCommand(ATCommandType.CIICR),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    2
                //),
                //// Overime ci sme ziskali verejnu IP adresu
                //new ATCommandToSend(
                //    new ATCommand(ATCommandType.CIFSR),
                //    new Expected_AT_Response(
                //        new AT_Response_Regex(
                //            new Regex(@"^\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b$")
                //        )
                //    ),
                //    2
                //),
                // Vypneme aby pri AT+CIPSEND vypisovalo prompt '>' ako spatne echo
                //new ATCommandToSend(
                //    new ATCommand(ATCommandType.CIPSPRT, "0"),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    2
                //),
                //Nastavime kominukaciu cez SSL
                //new ATCommandToSend(
                //    new AT_Command(AT_CommandType.CIPSSL, "1"),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK)
                //    ),
                //    2
                //),
                //Nadviazeme TCP spojenie so serverom
                //new ATCommandToSend(
                //    new ATCommand(ATCommandType.CIPSTART, "TCP", _serverAddress, "80"),
                //    //new AT_Command(AT_CommandType.CIPSTART, "TCP", _serverAddress, "443"),
                //    new Expected_AT_Response(
                //        new AT_Response_Status(AT_Response_Status_Type.OK),
                //        new AT_Response_StringStatus("CONNECT", AT_Response_Status_Type.OK)

                //    ),
                //    3
                //),
                // Odosleme data pomocou TCP spojenia a budeme ocakavat odpoved zo servera, ak sa to nepodari nebudeme znova opakovat prikaz
                //new ATCommandToSend(
                //    new ATCommand(ATCommandType.CIPSEND),
                //    data,
                //    new Expected_AT_Response(
                //        new AT_Response_StringStatus("SEND", AT_Response_Status_Type.OK),
                //        new AT_Response_Status(AT_Response_Status_Type.CLOSED)
                //    ),
                //    1
                //)//,
                 // Zavrieme TCP spojenie zopakujeme viackrat, ak to na prvykrat nevyjde
            /*new ATCommandToSend(
                new AT_Command(AT_CommandType.CIPSHUT),
                new Expected_AT_Response(
                    new AT_Response_StringStatus("SHUT", AT_Response_Status_Type.OK)
                ),
                3
            )*/
            );

            return result;
        }

        //public bool SendHTTPGET()
        //{

        //}

    }
}


//OLD Code ---------------------------------------------------------------------
//
//try
//{
//    // Zistime ci modul reaguje na zakladny prikaz AT
//    SendATCommand(

//        new AT_Command(AT_CommandType.EMPTY),
//		new Expected_AT_Response(

//            new AT_Response_Status(AT_Response_Status_Type.OK)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }

//	int i = -1;
//	while (i< 10)
//	{
//		i++;

//        SendATCommand(

//            new AT_Command(AT_CommandType.ATE0),
//			new Expected_AT_Response(

//                new AT_Response_Status(AT_Response_Status_Type.OK)
//			),
//			out aT_Response
//		);

//        WriteATResponseMessages(aT_Response);
//		if (aT_Response.Satisfied) { break; }
//	}
//	if (!aT_Response.Satisfied) { return false; }



//    SendATCommand(

//        new AT_Command(AT_CommandType.CPIN, true),
//		new Expected_AT_Response(

//            new AT_Response_CmdString(AT_CommandType.CPIN, "READY"),
//			new AT_Response_Status(AT_Response_Status_Type.OK)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }


//    SendATCommand(

//        new AT_Command(AT_CommandType.CREG, true),
//		new Expected_AT_Response(

//            new AT_Response_CmdString(AT_CommandType.CREG, "0,1"),
//			new AT_Response_Status(AT_Response_Status_Type.OK)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }


//    SendATCommand(

//        new AT_Command(AT_CommandType.CGATT, true),
//		new Expected_AT_Response(

//            new AT_Response_CmdString(AT_CommandType.CGATT, "1"),
//			new AT_Response_Status(AT_Response_Status_Type.OK)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }


//    SendATCommand(

//        new AT_Command(AT_CommandType.CSQ),
//		new Expected_AT_Response(

//            new AT_Response_CmdString(AT_CommandType.CSQ, ""),
//			new AT_Response_Status(AT_Response_Status_Type.OK)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }
//}
//catch (Exception e)
//{
//	Console.WriteLine(e.Message);
//	return false;
//}

//return true;






//try
//{
//    // Zistime ci je k dispozicii GPRS spojenie
//    SendATCommand(

//        new AT_Command(AT_CommandType.CGATT, true),
//		new Expected_AT_Response(

//            new AT_Response_CmdString(AT_CommandType.CGATT, "1"),
//			new AT_Response_Status(AT_Response_Status_Type.OK)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }

//	// Pokusime sa vypnut predchadzajuce TCP spojenie a nastavit APN operatora, ak sa z nejake dovodu pred tym nevyplo aby slo nastavit APN
//	int i = -1;
//	while (i< 10)
//	{
//		i++;

//        SendATCommand(

//            new AT_Command(AT_CommandType.CIPSHUT),
//			new Expected_AT_Response(

//                new AT_Response_StringStatus("SHUT", AT_Response_Status_Type.OK)
//			),
//			out aT_Response
//		);

//        WriteATResponseMessages(aT_Response);
//		if (!aT_Response.Satisfied) { continue; }


//        SendATCommand(

//            new AT_Command(AT_CommandType.CSTT, _operatorAPN, _operatorAPNUser, _operatorAPNPwd),
//			new Expected_AT_Response(

//                new AT_Response_Status(AT_Response_Status_Type.OK)
//			),
//			out aT_Response
//		);

//        WriteATResponseMessages(aT_Response);
//		if (aT_Response.Satisfied) { break; }
//	}
//	if (!aT_Response.Satisfied) { return false; }


//    SendATCommand(

//        new AT_Command(AT_CommandType.CIICR),
//		new Expected_AT_Response(

//            new AT_Response_Status(AT_Response_Status_Type.OK)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }


//    SendATCommand(

//        new AT_Command(AT_CommandType.CIFSR),
//		new Expected_AT_Response(

//            new AT_Response_Regex(

//                new Regex(@"^\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b$")
//			)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }

//    // Vypneme aby pri AT+CIPSEND vypisovalo prompt '>'
//    SendATCommand(

//            new AT_Command(AT_CommandType.CIPSPRT, "0"),
//			new Expected_AT_Response(

//                new AT_Response_Status(AT_Response_Status_Type.OK)
//			),
//			out aT_Response
//		);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }

//    //Nadviazeme TCP spojenie so serverom
//    SendATCommand(

//        new AT_Command(AT_CommandType.CIPSTART, "TCP", _serverAddress, "80"),
//		new Expected_AT_Response(

//            new AT_Response_Status(AT_Response_Status_Type.OK),
//			new AT_Response_StringStatus("CONNECT", AT_Response_Status_Type.OK)

//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }


//    SendATCommand(

//        new AT_Command(AT_CommandType.CIPSEND),
//		data,
//		new Expected_AT_Response(

//            new AT_Response_StringStatus("SEND", AT_Response_Status_Type.OK),
//			new AT_Response_Status(AT_Response_Status_Type.CLOSED)
//		),
//		out aT_Response
//	);

//    WriteATResponseMessages(aT_Response);
//	if (!aT_Response.Satisfied) { return false; }
//	serverResponse = aT_Response.Messages[1];

//	i = -1;
//	while (i< 10)
//	{
//		i++;

//        SendATCommand(

//            new AT_Command(AT_CommandType.CIPSHUT),
//			new Expected_AT_Response(

//                new AT_Response_StringStatus("SHUT", AT_Response_Status_Type.OK)
//			),
//			out aT_Response
//		);

//        WriteATResponseMessages(aT_Response);
//		if (aT_Response.Satisfied) { break; }
//	}
//	if (!aT_Response.Satisfied) { return false; }
//}
//catch (Exception e)
//{
//	Console.WriteLine(e.Message);
//	return false;
//}

//return true;

//private AT_Response ParseATResponse(String response)
//{
//    //Kazda odpoved moze byt na viacerych riadkoch, priznaky 'OK' alebo 'ERROR' nemusia byt na poslednej riadke

//    String[] responseLines = response.Split(
//        new[] { "\r\n", "\r", "\n" },
//        StringSplitOptions.None
//    );

//    //string[] arr = { "One", "Two", "Three" };
//    //var target = "One";
//    //var results = Array.FindAll(arr, s => s.Equals(target));

//    return null;
//}
//WriteATResponseMessages(aT_Response);
//if (!aT_Response.Satisfied) { return false; }

//SendATCommand(new AT_Command(AT_CommandType.HTTPPARA, "URL", _serverAddress + _id), out aT_Response);
//if (!aT_Response.Success) { return false; }
//WriteATResponseMessages(aT_Response);

//// Odosleme data o urcitej velkosti, TODO: latenciu vyladits poziadavkom so serveru, zisit maximalnu velkost postu
//SendATCommand(new AT_Command(AT_CommandType.HTTPDATA, data.Length.ToString(), "10000"), data, out aT_Response);
//if (!aT_Response.Success) { return false; }
//WriteATResponseMessages(aT_Response);

//// Spustime POST Session a pockame na HTTPACTION vysledok, napr.: "+HTTPACTION:1,200,0"
//SendATCommand(new AT_Command(AT_CommandType.HTTPACTION, "1"), out aT_Response);
//if (!aT_Response.Success) { return false; }
//WriteATResponseMessages(aT_Response);

//// Ukoncime HTTP spojenie
//SendATCommand(new AT_Command(AT_CommandType.HTTPTERM), out aT_Response);
//if (!aT_Response.Success) { return false; }
//WriteATResponseMessages(aT_Response);
