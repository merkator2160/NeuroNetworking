using System;

namespace AntHillSimulation.Core.Messenger
{
    public interface ITrayNotificator
    {
        void ShowMessage(String message);
        void ShowError(String message);
    }
}