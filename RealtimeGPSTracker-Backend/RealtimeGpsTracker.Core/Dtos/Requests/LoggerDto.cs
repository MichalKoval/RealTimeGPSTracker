using System;
using System.Collections.Generic;
using System.Text;

namespace RealtimeGpsTracker.Core.Dtos.Requests
{
    public class LoggerDto
    {
        public string Message { get; set; } = null;
        public string FileName { get; set; } = null;
        public string Level { get; set; } = null;
        public string LineNumber { get; set; } = null;
        public string Timestamp { get; set; } = null;
    }
}
