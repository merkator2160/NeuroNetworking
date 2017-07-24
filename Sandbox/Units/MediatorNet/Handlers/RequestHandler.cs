using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Sandbox.Units.MediatorNet.Messages;
using System.Threading.Tasks;

namespace Sandbox.Units.MediatorNet.Handlers
{
    public class RequestHandler : IRequestHandler<GuidRequest, GuidResponse>
    {
        public Task<GuidResponse> Handle(ReceiveContext<GuidRequest> context)
        {
            return Task.Run(() => new GuidResponse
            {
                ModyfiedId = $"{context.Message.Id} - from response"
            });
        }
    }
}