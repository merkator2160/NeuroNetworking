using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Sandbox.Units
{
    public static class SpeechSynthesizerUnit
    {
        public static void Run()
        {
            //Say("Hello world!");

            var sr = new SpeechRecognitionEngine();
            sr.SetInputToDefaultAudioDevice();
            var grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(new Choices("left", "right", "up", "down", "sibo"));
            sr.UnloadAllGrammars();

            sr.LoadGrammar(new Grammar(grammarBuilder));
            sr.SpeechRecognized += OnSpeechRecognized;
            sr.RecognizeAsync(RecognizeMode.Multiple);

            Console.ReadKey();
        }
        private static void Say(String text)
        {
            using (var speechSynthesizer = new SpeechSynthesizer()
            {
                Volume = 100,
                Rate = 0
            })
            {
                speechSynthesizer.Speak(text);
            }
        }


        // EVENTS /////////////////////////////////////////////////////////////////////////////////
        private static void OnSpeechRecognized(Object sender, SpeechRecognizedEventArgs speechRecognizedEventArgs)
        {
            Console.WriteLine("Hypothesized phrase: " + speechRecognizedEventArgs.Result.Text);
        }
    }
}