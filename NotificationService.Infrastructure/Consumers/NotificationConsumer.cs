using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Consumers
{
    public class NotificationConsumer : IConsumer<NotificationConsumer>
    {
        public Task Consume(ConsumeContext<NotificationConsumer> context)
        {
            throw new NotImplementedException();
        }
    }
}
