using RaspberryPiDaemon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RaspberryPiDaemon
{
    public class AT_Response_String : IATResponse
    {
        public AT_Response_String(string expectedStr)
        {
            Value = expectedStr;
        }

        public string Value { get; set; }
    }

    public class AT_Response_CmdString : IATResponse
    {
        public AT_Response_CmdString(ATCommandType aT_CommandType, string expectedStr)
        {
            Value = "+" + aT_CommandType + ": " + expectedStr;
        }

        public string Value { get; set; }
    }

    public class AT_Response_StringStatus : IATResponse
    {
        public AT_Response_StringStatus(string expectedStr, AT_Response_Status_Type expectedStatusType)
        {
            Value = expectedStr + " " + expectedStatusType;
        }

        public string Value { get; set; }
    }

    public class AT_Response_Regex : IATResponse
    {
        public AT_Response_Regex(Regex regex)
        {
            Value = regex;
        }

        public Regex Value { get; set; }
    }

    public class AT_Response_Server : IATResponse
    {
        public AT_Response_Server() { }
    }


    public class AT_Response_Status : IATResponse
    {
        public AT_Response_Status(AT_Response_Status_Type expectedStatusType)
        {
            Value = expectedStatusType;
        }

        public AT_Response_Status_Type Value { get; set; }
    }

    public class Expected_AT_Response
    {
        public Expected_AT_Response(params IATResponse[] expectedATResponses)
        {
            ExpectedResponses = expectedATResponses.ToList();
        }

        public List<IATResponse> ExpectedResponses { get; set; }
    }

    public class AT_Response
    {
        public AT_Response(bool satisfied, List<string> msg)
        {
            Satisfied = satisfied;
            Messages = msg;
        }

        public bool Satisfied { get; set; }
        public List<string> Messages { get; set; }
    }

    public enum AT_Response_Status_Type
    {
        OK,
        ERROR,
        DOWNLOAD,
        FAIL,
        CLOSED
        // +SAPBR:1,1,"10.89.193.1"
        // +HTTPACTION: 0,200,1000 ... GET
        // +HTTPACTION: 1,200,0    ... POST
        // +HTTPREAD: 1000
        // 
    }
}
