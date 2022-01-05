using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPiDaemon
{
    public enum ATCommandType
    {
        ATE0,
        ATE1,
        CIICR,
        CIFSR,
        CIPSEND,
        CIPSHUT,
        CIPSPRT,
        CIPSSL,
        CIPSTART,
        CGATT,
        CPIN,
        CREG,
        CSQ,
        EMPTY,      // Just 'AT' command itself, for testing
        HTTPACTION,
        HTTPDATA,
        HTTPINIT,
        HTTPPARA,
        HTTPREAD,
        HTTPSSL,
        HTTPTERM,
        SAPBR, //APN nastavime pomocou SAPBR (Bearer profile)

        // GNSS Commands
        CGNSPWR, // Sets GNSS Power Control Start/Stop (Cable for GNSS antena need to be connected)
        CGNSINF, // Gives GNSS Navigation Information (GNSS Coordinate) Parsed From NMEA Sentences (GNSS Power control need to be set to Start = 1.)



    }

    public enum ATResponseType
    {
        OK,
        ERROR,
        DOWNLOAD,
        FAIL,
        CLOSED
    }

    public class ATCommand
    {
        private ATCommandType _command;
        private string _commandStr;
        private ATResponseType _expectedResponse;
        private string _expectedResponseStr;
        private string[] _params;
        
        private bool _questionMark = false;

        private bool isNumberOnly(string str)
        {
            return str != "" && str.All(c => c >= '0' && c <= '9');
        }

        private void generateCommandStr()
        {
            if (_command == ATCommandType.ATE0)
            {
                _commandStr = ATCommandType.ATE0.ToString();
                return;
            }

            if (_command == ATCommandType.ATE1)
            {
                _commandStr = ATCommandType.ATE1.ToString();
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("AT");

            if (_command != ATCommandType.EMPTY)
            {
                sb.Append('+');
                sb.Append(_command);

                if (_questionMark)
                {
                    sb.Append('?');
                }

                if (_params != null && _params.Length > 0 && !_questionMark)
                {
                    sb.Append('=');

                    int paramsLength = _params.Length;

                    for (int i = 0; i < paramsLength; i++)
                    {
                        if (isNumberOnly(_params[i]))
                        {
                            sb.Append(_params[i]);
                        }
                        else
                        {
                            sb.Append('"');
                            sb.Append(_params[i]);
                            sb.Append('"');
                        }

                        if (i < paramsLength - 1)
                        {
                            sb.Append(',');
                        }
                    }
                }
            }

            // Very important to include this CR character at the end of each command.
            // Command must be of the format <command><CR> in order to work properly.
            sb.Append('\r');
            sb.Append('\n');

            _commandStr = sb.ToString();
        }

        private void generateExpectedResponseStr()
        {
            // Response must be of the format <CR><LF><response><CR><LF>
            StringBuilder sb = new StringBuilder();
                        
            sb.Append('\r');
            sb.Append('\n');

            switch (_expectedResponse)
            {
                case ATResponseType.OK:
                    sb.Append(ATResponseType.OK.ToString());
                    break;
                case ATResponseType.ERROR:
                    sb.Append(ATResponseType.ERROR.ToString());
                    break;
                case ATResponseType.DOWNLOAD:
                    sb.Append(ATResponseType.DOWNLOAD.ToString());
                    break;
                case ATResponseType.FAIL:
                    sb.Append(ATResponseType.FAIL.ToString());
                    break;
                case ATResponseType.CLOSED:
                    sb.Append(ATResponseType.CLOSED.ToString());
                    break;
                default:
                    sb.Append("");
                    break;
            }

            sb.Append('\r');

            _expectedResponseStr = sb.ToString();
        }

        public ATCommand(ATCommandType command, ATResponseType expectedResponse, params string[] prms)
        {
            _command = command;
            _expectedResponse = expectedResponse;
            _params = prms;
            generateCommandStr();
            generateExpectedResponseStr();
        }

        public ATCommand(ATCommandType command, ATResponseType expectedResponse, bool questionMark = false)
        {
            _command = command;
            _expectedResponse = expectedResponse;
            _questionMark = questionMark;
            generateCommandStr();
            generateExpectedResponseStr();
        }

        public ATCommandType Command
        {
            get { return _command; }
            set
            {
                _command = value;
                generateCommandStr();
                generateExpectedResponseStr();
            }
        }

        public string[] Parameters
        {
            get { return _params; }
            set
            {
                _params = value;
                generateCommandStr();
                generateExpectedResponseStr();
            }
        }

        public string CommandStr
        {
            get { return _commandStr; }
        }
        public string ExpectedResponseStr
        {
            get { return _expectedResponseStr; }
        }
    }
}

