﻿using Netduino.Sandbox.Units.StepEngineDir.Enums;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace Netduino.Sandbox.Units.StepEngineDir
{
    internal static class StepEngineUnit
    {
        public static void Run()
        {
            using (var stepEngine = new StepEngine(Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D3))
            {
                while (true)
                {
                    stepEngine.Move(Direction.Forward, StepEngine.SinglePrecisione, 2);
                    Thread.Sleep(2000);
                    stepEngine.Move(Direction.Backward, StepEngine.SinglePrecisione, 2);
                    Thread.Sleep(2000);
                }
            }
        }
    }
}