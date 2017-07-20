using System;
using Mediator.Net.Contracts;

namespace Sandbox.Units.Mediators.Messages
{
    public class GuidResponse : IResponse
    {
        public String ModyfiedId { get; set; }
    }
}