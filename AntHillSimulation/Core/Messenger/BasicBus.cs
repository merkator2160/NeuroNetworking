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
        public void Subscribe(String busName, Action<String, Object> action)
        {
            if(busName == null)
                throw new ArgumentNullException($"{nameof(busName)} is null");

            if (action == null)
                throw new ArgumentNullException($"{nameof(action)} is null");

            _buses.AddHandler(busName, action);
        }
        public void Subscribe(BusType busType, Action<BusType, Object> action)
        {
            _buses.AddHandler(busType.ToString(), action);
        }
        public void Subscribe<T>(BusType busType, Action<BusType, T> action)
        {
            _buses.AddHandler(busType, action);
        }
        public void Unsubscribe(String busName, Action<String, Object> action)
        {
            if (busName == null)
                throw new ArgumentNullException($"{nameof(busName)} is null");

            if (action == null)
                throw new ArgumentNullException($"{nameof(action)} is null");

            _buses.RemoveHandler(busName, action);
        }
        public void Unsubscribe(BusType busType, Action<BusType, Object> action)
        {
            if (action == null)
                throw new ArgumentNullException($"{nameof(action)} is null");

            _buses.RemoveHandler(busType, action);
        }
        public void Unsubscribe<T>(BusType busType, Action<BusType, T> action)
        {
            if (action == null)
                throw new ArgumentNullException($"{nameof(action)} is null");

            _buses.RemoveHandler(busType, action);
        }
        public void Send(String busName, Object message)
        {
            if (busName == null)
                throw new ArgumentNullException($"{nameof(busName)} is null");

            var specifyedBus = _buses[busName];
            if(specifyedBus != null)
                _buses[busName].DynamicInvoke(busName, message);
        }
        public void Send(BusType busType, Object message)
        {
            var busTypeAsStr = busType.ToString();
            var specifyedBus = _buses[busTypeAsStr];
            if (specifyedBus != null)
                _buses[busType].DynamicInvoke(busTypeAsStr, message);
        }
        public void Send<T>(BusType busType, T message)
        {
            var specifyedBus = _buses[busType];
            if (specifyedBus != null)
                _buses[busType].DynamicInvoke(busType, message);
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