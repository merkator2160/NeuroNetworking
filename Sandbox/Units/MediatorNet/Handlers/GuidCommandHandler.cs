using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Sandbox.Units.Mediators.Messages;

namespace Sandbox.Units.Mediators.Handlers
{
    public class GuidCommandHandler : ICommandHandler<GuidCommand>
    {
        public Task Handle(ReceiveContext<GuidCommand> context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Command handler: {context.Message.Id}");
            });
        }
    }
}