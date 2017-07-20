using System;
using GalaSoft.MvvmLight.Messaging;

namespace Sandbox.Units.MvvmLight
{
    public class Handler
    {
        public Handler(IMessenger messenger)
        {
            messenger.Register<String>(this, Console.WriteLine);
        }
    }
}