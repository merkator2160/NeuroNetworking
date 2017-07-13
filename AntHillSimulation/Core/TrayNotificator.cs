using System;
using System.Windows.Forms;
using Assets.Icons;

namespace AntHillSimulation.Core
{
    public class TrayNotificator
    {
        private readonly NotifyIcon _trayIcon;


        public TrayNotificator(String balloonTitle)
        {
            _trayIcon = new NotifyIcon()
            {
                Icon = Icons.InsectsAnt,
                Visible = true,
                BalloonTipTitle = balloonTitle
            };
            _trayIcon.DoubleClick += TrayIconOnDoubleClick;
        }
        

        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Show(String message)
        {
            _trayIcon.BalloonTipText = message;
            _trayIcon.ShowBalloonTip(1000);
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private void TrayIconOnDoubleClick(object sender, EventArgs eventArgs)
        {
            _trayIcon.BalloonTipText = "Test";
            _trayIcon.ShowBalloonTip(1000);
        }
    }
}