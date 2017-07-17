using System;
using System.ComponentModel;

namespace AntHillSimulation.Core.Messenger
{
    internal class BasicBus : ICommunicationBus, IDisposable
    {
        private readonly EventHandlerList _buses;
        private Boolean _disposed;


        public BasicBus()
        {
            _buses = new EventHandlerList();
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Subscribe(String busName, Action<String, Object> action)
        {
            if(busName == null)
                throw new ArgumentNullException($"{nameof(busName)} is null");

            if (action == null)
                throw new ArgumentNullException($"{nameof(action)} is null");

            _buses.AddHandler(busName, action);
        }
        public void Unsubscribe(String busName, Action<String, Object> action)
        {
            _buses.RemoveHandler(busName, action);
        }
        public void Send(String busName, Object message)
        {
            var specifyedBus = _buses[busName];
            if(specifyedBus != null)
                _buses[busName].DynamicInvoke(busName, message);
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
            _buses?.Dispose();
        }
    }
}