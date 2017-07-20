using System;
using System.Threading.Tasks;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Sandbox.Units.Mediators.Messages;

namespace Sandbox.Units.Mediators.Handlers
{
    public class RequestHandler : IRequestHandler<GuidRequest, GuidResponse>
    {
        public Task<GuidResponse> Handle(ReceiveContext<GuidRequest> context)
        {
            return Task.Run(() => new GuidResponse()
            {
                ModyfiedId = $"{context.Message.Id} - from response"
            });
        }
    }
}