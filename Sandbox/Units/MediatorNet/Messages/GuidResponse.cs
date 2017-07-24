using Mediator.Net.Contracts;
using System;

namespace Sandbox.Units.MediatorNet.Messages
{
    public class GuidResponse : IResponse
    {
        public String ModyfiedId { get; set; }
    }
}