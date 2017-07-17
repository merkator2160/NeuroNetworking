using System;
using System.Threading;
using System.Windows.Forms;

namespace AntHillSimulation.Core.Messenger.Messages
{
    internal class BaseMessage
    {
        public String BusName { get; set; }
        public Control Control { get; set; }
    }
}