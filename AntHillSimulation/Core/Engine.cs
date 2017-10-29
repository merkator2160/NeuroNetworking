using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading;

namespace AntHillSimulation.Core
{
    internal class Engine : IDisposable
    {
        private readonly ApplicationConfig _config;
        private readonly ITrayNotificator _notificator;
        private readonly IMessenger _messanger;
        private Boolean _disposed;


        public Engine(ApplicationConfig config, ITrayNotificator notificator, IMessenger communicationBus)
        {
            _config = config;
            _notificator = notificator;
            _messanger = communicationBus;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Run()
        {
            ThreadPool.QueueUserWorkItem(DoWork);
        }
        private void DoWork(Object state)
        {
            while (!_disposed)
            {
                try
                {
                    Thread.Sleep(_config.Engine.SimulationSpeed);
                }
                catch (Exception ex)
                {
                    _notificator.ShowError(ex.Message);
                    throw;
                }
            }
        }


        // IDisposable ////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}