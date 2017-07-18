using System;
using System.ComponentModel;
using AntHillSimulation.Core.Messenger.Enums;

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


        // ICommunicationBus //////////////////////////////////////////////////////////////////////
        public void Subscribe<T>(String busName, Action<String, T> action)
        {
            _buses.AddHandler(busName, action);
        }
        public void Unsubscribe<T>(String busName, Action<String, T> action)
        {
            if (action == null)
                throw new ArgumentNullException($"{nameof(action)} is null");

            _buses.RemoveHandler(busName, action);
        }
        public void Send<T>(String busName, T message)
        {
            var specifyedBus = _buses[busName];
            if (specifyedBus != null)
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