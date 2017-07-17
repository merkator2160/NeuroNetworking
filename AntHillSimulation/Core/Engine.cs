using AntHillSimulation.Forms;
using Assets.Icons;
using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.Practices.Unity;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger;

namespace AntHillSimulation.Core
{
    internal class Engine : IDisposable
    {
        private readonly ApplicationConfig _config;
        private readonly INotificator _notificator;
        private readonly ICommunicationBus _communicationBus;
        private Boolean _disposed;
        private Thread _workerThread;


        public Engine(ApplicationConfig config, INotificator notificator, ICommunicationBus communicationBus)
        {
            _config = config;
            _notificator = notificator;
            _communicationBus = communicationBus;
            
            _workerThread = new Thread(DoWork);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Run()
        {
            _workerThread?.Start();
        }
        private void DoWork()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(_config.Engine.SimulationSpeed);
                }
                catch (ThreadAbortException)
                {

                }
                catch (Exception ex)
                {
                    _notificator.ShowError(ex.Message);
#if DEBUG
                    throw;
#endif
                }
            }
        }
        
        

        // IDisposable ////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (!_disposed)
            {
                ReleaseUnmanagedResources();
                if (disposing)
                    ReleaseManagedResources();

                _disposed = true;
            }
        }
        private void ReleaseUnmanagedResources()
        {
            // We didn't have its yet.
        }
        private void ReleaseManagedResources()
        {
            _workerThread?.Abort();
        }
    }
}