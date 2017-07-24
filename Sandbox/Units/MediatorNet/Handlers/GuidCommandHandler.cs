using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Sandbox.Units.MediatorNet.Messages;
using System;
using System.Threading.Tasks;

namespace Sandbox.Units.MediatorNet.Handlers
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