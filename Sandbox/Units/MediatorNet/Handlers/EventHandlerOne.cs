using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Sandbox.Units.MediatorNet.Messages;
using System;
using System.Threading.Tasks;

namespace Sandbox.Units.MediatorNet.Handlers
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