using Microsoft.SPOT;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Netduino.Sandbox.Units
{
    internal static class NetworkUnit
    {
        public static void Run()
        {
            new App().Run();


            try
            {
                var responseStr = MakeWebRequest("http://google.com");
                Debug.Print(responseStr);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }



            Thread.Sleep(10000);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        private static String MakeWebRequest(String url)
        {
            using (var httpWebRequest = WebRequest.Create(url))
            {
                using (var httpResponse = httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}