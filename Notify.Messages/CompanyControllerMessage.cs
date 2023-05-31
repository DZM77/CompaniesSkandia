using NServiceBus;

namespace Notify.Messages
{
    public class CompanyControllerMessage : IMessage
    {
        public string Message { get; set; }
    }
}