using System;
using AntHillSimulation.Core.Messenger.Enums;

namespace AntHillSimulation.Core.Messenger
{
    internal interface ICommunicationBus
    {
        void Subscribe<T>(String busName, Action<String, T> action);
        void Unsubscribe<T>(String busName, Action<String, T> action);
        void Send<T>(String busNamee, T message);
    }
}