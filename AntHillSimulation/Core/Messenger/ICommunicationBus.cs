using System;
using AntHillSimulation.Core.Messenger.Enums;

namespace AntHillSimulation.Core.Messenger
{
    internal interface ICommunicationBus
    {
        void Subscribe(String busName, Action<String, Object> action);
        void Subscribe(BusType busType, Action<BusType, Object> action);
        void Subscribe<T>(BusType busType, Action<BusType, T> action);
        
        void Unsubscribe(String busName, Action<String, Object> action);
        void Unsubscribe(BusType busType, Action<BusType, Object> action);
        void Unsubscribe<T>(BusType busType, Action<BusType, T> action);
        
        void Send(String busName, Object message);
        void Send(BusType busType, Object message);
        void Send<T>(BusType busType, T message);
    }
}