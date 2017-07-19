using System;
using System.Drawing;
using System.Windows.Forms;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using AntHillSimulation.Core.Messenger.Enums;
using AntHillSimulation.Forms;
using Assets.Icons;

namespace AntHillSimulation.Core
{
    internal class FormsManager
    {
        private readonly ApplicationConfig _config;
        private readonly ICommunicationBus _communicationBus;

        private readonly PlaygroundForm _playgroundForm;
        private readonly SecondForm _secondForm;


        public FormsManager(ApplicationConfig config, 
            ICommunicationBus communicationBus,
            PlaygroundForm playgroundForm,
            SecondForm secondForm)
        {
            _config = config;
            _communicationBus = communicationBus;
            _playgroundForm = playgroundForm;
            _secondForm = secondForm;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Initialyze()
        {
            _communicationBus.Subscribe<Object>(Buses.TrayIconDoubleClick.ToString(), OnTrayIconDoubleClick);

            _playgroundForm.Show();
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        public void ShowMainForm()
        {
            if (!_playgroundForm.Created)
            {
                _playgroundForm.Show();
            }
        }
        private void OnTrayIconDoubleClick(String busName, Object data)
        {
            if (!_playgroundForm.Created)
                _playgroundForm.ShowDialog();
        }
    }
}