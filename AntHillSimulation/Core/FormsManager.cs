using System;
using System.Windows.Forms;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using AntHillSimulation.Forms;
using Assets.Icons;

namespace AntHillSimulation.Core
{
    internal class FormsManager
    {
        private readonly ApplicationConfig _config;
        private readonly ICommunicationBus _communicationBus;

        private Form _playgroundForm;


        public FormsManager(ApplicationConfig config, ICommunicationBus communicationBus)
        {
            _config = config;
            _communicationBus = communicationBus;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Run()
        {
            _playgroundForm = new Playground(_config);
            _communicationBus.Subscribe("FormsBus", ShowPlaygroundForm);
        }
        private void ShowPlaygroundForm(String busName, Object data)
        {
            if (!_playgroundForm.Created)
                _playgroundForm.ShowDialog();
        }
    }
}