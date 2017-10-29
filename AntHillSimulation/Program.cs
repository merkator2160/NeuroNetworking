using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AntHillSimulation
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (CheckAnyOtherInstances())
                return;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Application.ThreadException += ApplicationOnThreadException;
            Application.ApplicationExit += OnApplicationExit;
            Application.Run(new SimulatorContext());
        }
        private static Boolean CheckAnyOtherInstances()
        {
            var guid = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();

            Boolean created;
            var mutexObj = new Mutex(true, guid, out created);
            if (!created)
                return true;

            return false;
        }



        // HANDLERS /////////////////////////////////////////////////////////////////////////////////
        private static void OnApplicationExit(object sender, EventArgs eventArgs)
        {

        }
        private static void CurrentDomainOnUnhandledException(Object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {

        }
        private static void ApplicationOnThreadException(Object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
        {

        }
    }
}
