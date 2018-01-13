using Microsoft.SPOT;
using Microsoft.SPOT.IO;
using System;
using System.Text;

namespace Netduino.Sandbox.Units
{
    internal static class SdCardUnit
    {
        public static void Run()
        {
            Debug.Print(OutputSdInfo());
        }
        public static String OutputSdInfo()
        {
            var stringBuilder = new StringBuilder();
            var vInfo = new VolumeInfo("SD");

            stringBuilder.AppendLine("Is Formatted: " + vInfo.IsFormatted);
            stringBuilder.AppendLine("Total Free Space: " + vInfo.TotalFreeSpace);
            stringBuilder.AppendLine("Total Size: " + vInfo.TotalSize);
            stringBuilder.AppendLine("File System: " + vInfo.FileSystem);

            return stringBuilder.ToString();
        }
    }
}