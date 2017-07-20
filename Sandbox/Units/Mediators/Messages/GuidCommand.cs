using System;
using Mediator.Net.Contracts;

namespace Sandbox.Units.Mediators.Messages
{
    public class GuidCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}