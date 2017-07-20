using System;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;

namespace Sandbox.Units.MvvmLight
{
    public static class MvvmLightMediator
    {
        public static void Run()
        {
            var container = ConfigureContainer();
            UseMessenger(container);
        }
        private static IUnityContainer ConfigureContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IMessenger, Messenger>(new ContainerControlledLifetimeManager());

            return container;
        }
        private static void UseMessenger(IUnityContainer container)
        {
            var messanger = container.Resolve<IMessenger>();
            var handler = new Handler(messanger);

            messanger.Send("Hello world!");
        }
    }
}