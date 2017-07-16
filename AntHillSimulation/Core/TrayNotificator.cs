using AntHillSimulation.Models;
using Mediator.Net;
using System;
using System.Windows.Forms;

namespace AntHillSimulation.Core
{
    public class TrayNotificator : IDisposable
    {
        private readonly NotifyIcon _trayIcon;
        private readonly IMediator _mediator;
        private readonly TrayNotificatorConfig _config;
        private Boolean _disposed;


        public TrayNotificator(IMediator mediator, TrayNotificatorConfig config)
        {
            _mediator = mediator;
            _config = config;
            _trayIcon = new NotifyIcon()
            {
                Icon = config.TrayIcon,
                Visible = true,
                BalloonTipTitle = config.BalloonTitle
            };
            _trayIcon.DoubleClick += TrayIconOnDoubleClick;
        }
        ~TrayNotificator()
        {
            Dispose(false);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void ShowMessage(String message)
        {
            _trayIcon.ShowBalloonTip(_config.BaloonLifetime, _config.BalloonTitle, message, ToolTipIcon.None);
        }
        public void ShowError(String message)
        {
            _trayIcon.ShowBalloonTip(_config.BaloonLifetime, _config.BalloonTitle, message, ToolTipIcon.Error);
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private void TrayIconOnDoubleClick(object sender, EventArgs eventArgs)
        {
            _trayIcon.ShowBalloonTip(_config.BaloonLifetime, _config.BalloonTitle, "Test", ToolTipIcon.None);
            //await _mediator.SendAsync(new ShowPlaygroundCommand());
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