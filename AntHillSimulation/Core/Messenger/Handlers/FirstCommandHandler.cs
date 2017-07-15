using AntHillSimulation.Core.Messenger.Commands;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using System.Threading.Tasks;

namespace AntHillSimulation.Core.Messenger.Handlers
{
    public class FirstCommandHandler : ICommandHandler<FirstCommand>
    {
        public Task Handle(ReceiveContext<FirstCommand> context)
        {
            throw new System.NotImplementedException();
        }
    }
}