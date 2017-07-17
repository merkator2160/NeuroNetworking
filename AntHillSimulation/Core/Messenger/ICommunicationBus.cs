using System;

namespace AntHillSimulation.Core.Messenger
{
    internal interface ICommunicationBus
    {
        void Subscribe(String busName, Action<String, Object> action);
        void Unsubscribe(String busName, Action<String, Object> action);
        void Send(String busName, Object message);
    }
}