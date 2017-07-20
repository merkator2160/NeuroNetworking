using System;
using Sandbox.Units;
using Sandbox.Units.Mediators;
using Sandbox.Units.MvvmLight;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            MvvmLightMediator.Run();
            Console.ReadKey();
        }
    }
}