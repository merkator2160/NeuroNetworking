using ScriptCs.SpeakR.ScriptPack;
using System;

namespace Sandbox.Units
{
    public static class ScriptCsUnit
    {
        public static void Run()
        {
            Say("Hello governer!");
        }
        private static void Say(String text)
        {
            using (var speakr = new SpeakR())
            {
                speakr
                    .Gender("female")
                    .Culture("en-gb")
                    .Rate(1)
                    .SpeakWrite(text);
            }
        }
    }
}