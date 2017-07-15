using AntHillSimulation.Core.Messenger.Commands;
using AntHillSimulation.Forms;
using AntHillSimulation.Models;
using Assets.Icons;
using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntHillSimulation.Core
{
    internal class Engine : ICommandHandler<ShowPlaygroundCommand>
    {
        private readonly EngineConfig _config;
        private readonly Form _playGroundForm;
        private readonly IUnityContainer _container;
        private readonly TrayNotificator _trayNotificator;
        private readonly IMediator _mediator;


        public Engine(EngineConfig config)
        {
            _config = config;
            _container = ConfigureContainer();
            _mediator = new MediatorBuilder().RegisterHandlers(typeof(Engine).Assembly).Build();
            _trayNotificator = new TrayNotificator(_mediator, new TrayNotificatorConfig()
            {
                TrayIcon = Icons.InsectsAnt,
                BalloonTitle = _config.ApplicationName,
                BaloonLifetime = 500
            });
            _playGroundForm = new Playground(new PlaygroundConfig()
            {
                Title = _config.ApplicationName,
                Icon = Icons.InsectsAnt
            });
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Run()
        {
            while (true)
            {
                try
                {
                    DoWork();
                    Thread.Sleep(_config.SimulationSpeed);
                }
                catch (ThreadAbortException)
                {

                }
                catch (Exception ex)
                {
                    _trayNotificator.ShowError(ex.Message);
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

            return container;
        }
        public Task Handle(ReceiveContext<ShowPlaygroundCommand> context)
        {
            return Task.Run(() =>
            {
                _playGroundForm.ShowDialog();
            });
        }
    }
}