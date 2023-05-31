using Microsoft.Extensions.Logging;
using Notify.Messages;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notify
{
    public class CompanyControllerHandler : IHandleMessages<CompanyControllerMessage>
    {
        static ILog log = LogManager.GetLogger<CompanyControllerHandler>();
        public Task Handle(CompanyControllerMessage message, IMessageHandlerContext context)
        {
            log.Info($"Get message: {message.Message}");
            return Task.CompletedTask;
        }
    }
}
