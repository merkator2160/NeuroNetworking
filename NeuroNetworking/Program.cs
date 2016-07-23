using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.Windows.Forms;

namespace NeuroNetworking
{
    static class Program
    {
        private static ImageViewer _viewerForm;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _viewerForm = new ImageViewer();
            IniteCupture();
        }
        private static void IniteCupture()
        {
            var capture = new Capture();
            Application.Idle += delegate (Object sender, EventArgs e)
            {
                _viewerForm.Image = capture.QueryFrame();
            };
            _viewerForm.ShowDialog();
        }
    }
}
