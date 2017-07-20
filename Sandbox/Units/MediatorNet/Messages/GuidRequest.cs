using System;
using Mediator.Net.Contracts;

namespace Sandbox.Units.Mediators.Messages
{
    public class GuidRequest : IRequest
    {
        public String Id { get; set; }
    }
}