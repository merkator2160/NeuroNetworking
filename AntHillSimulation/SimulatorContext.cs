using AntHillSimulation.Core;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using AntHillSimulation.Core.Messenger.Messages;
using AntHillSimulation.Forms;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace AntHillSimulation
{
    public class SimulatorContext : ApplicationContext
    {
        private readonly IUnityContainer _container;


        public SimulatorContext()
        {
            _container = ConfigureContainer();

            _container.Resolve<Engine>().Run();

            var trayNotificator = _container.Resolve<ITrayNotificator>();
            var messenger = _container.Resolve<IMessenger>();
            messenger.Register<TrayIconClickMessage>(this, OnTrayIconDoubleClick);
        }

        // HANDLERS ///////////////////////////////////////////////////////////////////////////////
        private void OnTrayIconDoubleClick(TrayIconClickMessage message)
        {
            var playgroundForm = _container.Resolve<PlaygroundForm>();
            playgroundForm.Show();
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private static ApplicationConfig GetConfig()
        {
            var configFilePath = $"{Environment.CurrentDirectory}//ApplicationConfig.json";
            using (var fileStream = new FileStream(configFilePath, FileMode.Open))
            {
                var serializer = new JsonSerializer();
                using (var streamReader = new StreamReader(fileStream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return serializer.Deserialize<ApplicationConfig>(jsonTextReader);
                    }
                }
            }
        }
        private static IUnityContainer ConfigureContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance(GetConfig());

            container.RegisterType<IMessenger, Messenger>(new ContainerControlledLifetimeManager());
            container.RegisterType<Engine>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITrayNotificator, TrayNotificator>(new ContainerControlledLifetimeManager());
            container.RegisterType<PlaygroundForm>();
            container.RegisterType<SecondForm>();

            return container;
        }


        // IDisposable ////////////////////////////////////////////////////////////////////////////
        protected override void Dispose(Boolean disposing)
        {
            base.Dispose(disposing);
            _container.Dispose();
        }
    }
}