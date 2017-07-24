using Mediator.Net.Contracts;
using System;

namespace Sandbox.Units.MediatorNet.Messages
{
    public class GuidCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}