using AntHillSimulation.Core.Messenger.Messages;
using MediatR;

namespace AntHillSimulation.Core.Messenger.Handlers
{
    public class MessageHandler : INotificationHandler<FirstMessage>
    {
        public void Handle(FirstMessage notification)
        {
            throw new System.NotImplementedException();
        }
    }
}