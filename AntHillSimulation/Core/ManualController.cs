using System;
using System.Drawing;
using System.Windows.Forms;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using AntHillSimulation.Core.Messenger.Enums;

namespace AntHillSimulation.Core
{
    internal class ManualController
    {
        private readonly ApplicationConfig _config;
        private readonly ICommunicationBus _communicationBus;


        public ManualController(ApplicationConfig config, ICommunicationBus communicationBus)
        {
            _config = config;
            _communicationBus = communicationBus;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Initialyze()
        {
            _communicationBus.Subscribe<KeyEventArgs>(Buses.PlaygroundFormKeyDown.ToString(), OnPlaygroundFormKeyDown);
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private void OnPlaygroundFormKeyDown(String busName, KeyEventArgs data)
        {

            
        }
    }
}