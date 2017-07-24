using Mediator.Net.Contracts;
using System;

namespace Sandbox.Units.MediatorNet.Messages
{
    public class GuidRequest : IRequest
    {
        public String Id { get; set; }
    }
}