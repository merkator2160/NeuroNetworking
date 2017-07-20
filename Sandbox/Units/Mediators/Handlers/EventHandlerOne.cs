using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Sandbox.Units.Mediators.Messages;

namespace Sandbox.Units.Mediators.Handlers
{
    public class EventHandlerOne : IEventHandler<EventMessage>
    {
        public Task Handle(IReceiveContext<EventMessage> context)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Event handler 1: {context.Message.Id}");
            });
        }
    }
}