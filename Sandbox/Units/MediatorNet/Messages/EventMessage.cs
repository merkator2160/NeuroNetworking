using System;
using Mediator.Net.Contracts;

namespace Sandbox.Units.Mediators.Messages
{
    public class EventMessage : IEvent
    {
        public Guid Id { get; set; }
    }
}