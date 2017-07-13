using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using AntHillSimulation.Forms;
using AntHillSimulation.Models;
using MediatR;

namespace AntHillSimulation.Core
{
    internal class Engine
    {
        private readonly EngineConfig _config;
        private readonly Form _playGroundForm;
        private readonly TrayNotificator _trayNotificator;


        public Engine(EngineConfig config)
        {
            _config = config;
            _trayNotificator = new TrayNotificator(_config.ApplicationName);
            _playGroundForm = new Playground();
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Run()
        {
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
            if(!_playGroundForm.Created)
            {
                _playGroundForm.ShowDialog();
            }
        }

        private void ConfigureMediator()
        {
            var container = new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.AssemblyContainingType<Ping>();                                         // Our assembly with requests & handlers
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>));        // Handlers with no response
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));       // Handlers with a response
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<>));   // Async handlers with no response
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));  // Async Handlers with a response
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                });
                cfg.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                cfg.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                cfg.For<IMediator>().Use<Mediator>();
            });
        }
    }
}