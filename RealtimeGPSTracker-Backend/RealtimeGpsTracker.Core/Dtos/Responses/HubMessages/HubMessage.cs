namespace RealtimeGpsTracker.Core.Dtos.Responses.HubMessages
{
    public class HubMessage
    {
        public HubMessage() { }

        public HubMessage(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
