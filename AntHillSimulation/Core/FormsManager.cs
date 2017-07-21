using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger.Messages;
using AntHillSimulation.Forms;
using Assets.Models;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;

namespace AntHillSimulation.Core
{
    internal class FormsManager
    {
        private readonly ApplicationConfig _config;
        private readonly IMessenger _communicationBus;
        private readonly IUnityContainer _container;

        private PlaygroundForm _playgroundForm;
        private SecondForm _secondForm;


        public FormsManager(ApplicationConfig config, IMessenger communicationBus, IUnityContainer container)
        {
            _config = config;
            _communicationBus = communicationBus;
            _container = container;
            _communicationBus.Register<TrayIconClickMessage>(this, OnTrayIconDoubleClick);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void ShowPlaygroundForm(Dimensions canvasDimensions)
        {
            if (_playgroundForm != null && !_playgroundForm.IsDisposed)
                return;

            _playgroundForm = _container.Resolve<PlaygroundForm>();
            _playgroundForm.Show();
        }
        public void ShowSecondForm()
        {
            if (_secondForm != null && !_secondForm.IsDisposed)
                return;

            _secondForm = _container.Resolve<SecondForm>();
            _secondForm.Show();
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private void OnTrayIconDoubleClick(TrayIconClickMessage message)
        {
            ShowPlaygroundForm();
        }
    }
}