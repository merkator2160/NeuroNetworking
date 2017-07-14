using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AntHillSimulation.Core.Messenger.Handlers;
using AntHillSimulation.Core.Messenger.Messages;
using AntHillSimulation.Forms;
using AntHillSimulation.Models;
using Assets.Icons;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Practices.Unity;

namespace AntHillSimulation.Core
{
    internal class Engine
    {
        private readonly EngineConfig _config;
        private readonly Form _playGroundForm;
        private readonly IUnityContainer _container;
        private readonly TrayNotificator _trayNotificator;


        public Engine(EngineConfig config)
        {
            _config = config;
            _container = ConfigureContainer();
            _trayNotificator = new TrayNotificator(_config.ApplicationName);
            _playGroundForm = new Playground();
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Run()
        {
            UseMediatorAsync();

            while(true)
            {
                try
                {
                    DoWork();
                    Thread.Sleep(_config.SimulationSpeed);
                }
                catch(ThreadAbortException)
                {

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
#if DEBUG
                    throw;
#endif
                }
            }
        }
        private void DoWork()
        {
            //if(!_playGroundForm.Created)
            //{
            //    _playGroundForm.ShowDialog();
            //}
        }
        private IUnityContainer ConfigureContainer()
        {
            var container = new UnityContainer();
            ConfigureMediator(container);
            
            return container;
        }
        private void ConfigureMediator(IUnityContainer container)
        {
            container.RegisterInstance<SingleInstanceFactory>(instanceType => container.Resolve(instanceType));
            container.RegisterInstance<MultiInstanceFactory>(instanceTypeCollection => container.ResolveAll(instanceTypeCollection));
            container.RegisterType<IMediator, Mediator>();

            //Pipeline
            container.RegisterType(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            container.RegisterType(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
            //container.RegisterType(typeof(IPipelineBehavior<,>), typeof(GenericPipelineBehavior<,>));
            //container.RegisterType(typeof(IRequestPreProcessor<>), typeof(GenericRequestPreProcessor<>));
            //container.RegisterType(typeof(IRequestPostProcessor<,>), typeof(GenericRequestPostProcessor<,>));

            var typesForExport = typeof(IMediator).GetTypeInfo().Assembly.ExportedTypes
                .Where(p => p.GetTypeInfo().IsClass)
                .ToArray();
            container.RegisterTypes(typesForExport);
            
            container.RegisterType(typeof(INotificationHandler<FirstMessage>), typeof(MessageHandler));
        }
        private async void UseMediatorAsync()
        {
            var mediator = _container.Resolve<IMediator>();

            await mediator.Publish(new FirstMessage());
        }
    }
}