using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers
{
    public class DeleteRegisterConsumer : IConsumer<DeleteRegisterConsumer>
    {
        public Task Consume(ConsumeContext<DeleteRegisterConsumer> context)
        {
            throw new NotImplementedException();
        }
    }
}
