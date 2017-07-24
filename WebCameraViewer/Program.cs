﻿using Emgu.CV;
using Emgu.CV.UI;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WebCameraViewer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            if (CheckOtherInstances())
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var viewerForm = new ImageViewer())
            {
                viewerForm.Text = "Web camera view";
                var capture = new Capture(1);
                Application.Idle += (sender, e) =>
                {
                    viewerForm.Image = capture.QueryFrame();
                };
                viewerForm.ShowDialog();
            }
        }
        private static Boolean CheckOtherInstances()
        {
            Boolean existed;
            var guid = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();
            var globalMutex = new Mutex(true, guid, out existed);
            return existed;
        }
    }
}