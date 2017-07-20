using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace AntHillSimulation.Core
{
    internal class ManualController
    {
        private readonly ApplicationConfig _config;
        private readonly IMessenger _communicationBus;


        public ManualController(ApplicationConfig config, IMessenger communicationBus)
        {
            _config = config;
            _communicationBus = communicationBus;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Initialyze()
        {
            _communicationBus.Register<PlaygroundKeyDownMessage>(this, OnPlaygroundFormKeyDown);
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private void OnPlaygroundFormKeyDown(PlaygroundKeyDownMessage message)
        {

        }
    }
}