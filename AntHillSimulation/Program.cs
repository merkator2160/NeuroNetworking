using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AntHillSimulation.Core;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using AntHillSimulation.Core.Messenger.Enums;
using AntHillSimulation.Forms;
using Assets.Icons;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace AntHillSimulation
{
    static class Program
    {
        private static IUnityContainer _container;


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (CheckAnyOtherInstances())
            {
                var config = GetConfig();
                ConfigureContainer(config);

                _container.Resolve<Engine>().Run();
                _container.Resolve<FormsManager>().Initialyze();
                _container.Resolve<ICommunicationBus>().Subscribe<Object>(Buses.TrayMenuExitCLick.ToString(), OnTrayExitButtonCLick);

                Application.ApplicationExit += OnApplicationExit;
                Application.Run();
            }
        }
        private static Boolean CheckAnyOtherInstances()
        {
            var guid = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();

            Boolean created;
            var mutexObj = new Mutex(true, guid, out created);
            if(!created)
            {
                MessageBox.Show("Application instance already exist");
            }
            return created;
        }
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
        private static void ConfigureContainer(ApplicationConfig config)
        {
            _container = new UnityContainer();
            _container.RegisterInstance(config);

            _container.RegisterType<ICommunicationBus, BasicBus>(new ContainerControlledLifetimeManager());
            _container.RegisterType<Engine>(new ContainerControlledLifetimeManager());
            _container.RegisterType<INotificator, TrayNotificator>(new ContainerControlledLifetimeManager());

            RegisterForms();
        }
        private static void RegisterForms()
        {
            _container.RegisterType<FormsManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<PlaygroundForm>(new ContainerControlledLifetimeManager());
            _container.RegisterType<SecondForm>(new ContainerControlledLifetimeManager());
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private static void OnTrayExitButtonCLick(String arg1, Object arg2)
        {
            Application.Exit();
        }
        private static void OnApplicationExit(object sender, EventArgs eventArgs)
        {
            _container?.Dispose();
        }
    }
}
