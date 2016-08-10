using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace NeuroNetworking
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            Boolean existed;
            var guid = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();
            var globalMutex = new Mutex(true, guid, out existed);
            if (!existed)
            {
                Application.Exit();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IniteCupture();
        }
        private static void IniteCupture()
        {
            using (var viewerForm = new ImageViewer())
            {
                var capture = new Capture(1);
                Application.Idle += (sender, e) =>
                {
                    viewerForm.Image = capture.QueryFrame();
                };
                viewerForm.ShowDialog();
            }
        }
    }
}
