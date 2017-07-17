using System;

namespace AntHillSimulation.Core.Messenger
{
    public interface INotificator
    {
        void ShowMessage(String message);
        void ShowError(String message);
    }
}