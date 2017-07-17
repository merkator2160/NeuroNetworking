using System;
using System.Drawing;
using System.Windows.Forms;
using AntHillSimulation.Core.Config;
using AntHillSimulation.Core.Messenger.Enums;
using Assets.Icons;

namespace AntHillSimulation.Core.Messenger
{
    internal class TrayNotificator : IDisposable, INotificator
    {
        private readonly NotifyIcon _trayIcon;
        private readonly ApplicationConfig _config;
        private readonly ICommunicationBus _communicationBus;
        private Boolean _disposed;


        public TrayNotificator(ICommunicationBus communicationBus, ApplicationConfig config)
        {
            _communicationBus = communicationBus;
            _config = config;

            var icon = Icons.ResourceManager.GetObject(config.IconName) as Icon;
            _trayIcon = new NotifyIcon()
            {
                Icon = icon,
                Visible = true,
                BalloonTipTitle = config.Tray.BalloonTitle,
                ContextMenu = ConfigureMenu()
            };
            _trayIcon.DoubleClick += TrayIconOnDoubleClick;
        }
        ~TrayNotificator()
        {
            Dispose(false);
        }


        // INotificator ///////////////////////////////////////////////////////////////////////////
        public void ShowMessage(String message)
        {
            _trayIcon.ShowBalloonTip(_config.Tray.BalloonLifetime, _config.Tray.BalloonTitle, message, ToolTipIcon.None);
        }
        public void ShowError(String message)
        {
            _trayIcon.ShowBalloonTip(_config.Tray.BalloonLifetime, _config.Tray.BalloonTitle, message, ToolTipIcon.Error);
        }
        
        private ContextMenu ConfigureMenu()
        {
            var menu = new ContextMenu();

            var exitMenu = new MenuItem("Exit");
            exitMenu.Click += ExitMenuOnClick;
            menu.MenuItems.Add(exitMenu);

            return menu;
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private void TrayIconOnDoubleClick(object sender, EventArgs eventArgs)
        {
            _communicationBus.Send(BusType.Forms, null);
        }
        private void ExitMenuOnClick(object sender, EventArgs eventArgs)
        {
            Application.Exit();
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
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
            }
        }
    }
}