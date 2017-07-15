using System;
using System.Drawing;

namespace AntHillSimulation.Models
{
    public class TrayNotificatorConfig
    {
        public String BalloonTitle { get; set; }
        public Icon TrayIcon { get; set; }
        public Int32 BaloonLifetime { get; set; }
    }
}