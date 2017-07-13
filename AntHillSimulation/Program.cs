using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AntHillSimulation.Core;
using AntHillSimulation.Models;

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
            {
                var engine = new Engine(new EngineConfig());
                engine.Run();
            }
        }
        private static Boolean CheckAnyOtherInstances()
        {
            var guid = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();

            Boolean created;
            var mutexObj = new Mutex(true, guid, out created);
            if(!created)
            {
                MessageBox.Show("Application instance already exist");
            }
            return created;
        }
    }
}
