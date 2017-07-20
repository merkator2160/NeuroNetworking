using AntHillSimulation.Core;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using AntHillSimulation.Forms;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AntHillSimulation
{
    static class Program
    {
        private static IUnityContainer _container;
        private static ApplicationConfig _config;


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (CheckAnyOtherInstances())
            {
                _config = GetConfig();
                _container = ConfigureContainer(_config);

                _container.Resolve<Engine>().Run();

                _container.Resolve<FormsManager>().ShowPlaygroundForm();

                Application.ApplicationExit += OnApplicationExit;
                Application.Run();
            }
        }
        private static Boolean CheckAnyOtherInstances()
        {
            var guid = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();

            Boolean created;
            var mutexObj = new Mutex(true, guid, out created);
            if (!created)
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
        private static IUnityContainer ConfigureContainer(ApplicationConfig config)
        {
            var container = new UnityContainer();
            container.RegisterInstance(config);

            container.RegisterType<IMessenger, Messenger>(new ContainerControlledLifetimeManager());
            container.RegisterType<Engine>(new ContainerControlledLifetimeManager());
            container.RegisterType<INotificator, TrayNotificator>(new ContainerControlledLifetimeManager());
            container.RegisterType<FormsManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<PlaygroundForm>();
            container.RegisterType<SecondForm>();

            return container;
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private static void OnApplicationExit(object sender, EventArgs eventArgs)
        {
            _container?.Dispose();
        }
    }
}
