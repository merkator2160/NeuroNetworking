using System;

namespace AntHillSimulation.Core.Config
{
    internal class ApplicationConfig
    {
        public String AppName { get; set; }
        public String IconName { get; set; }


        // LOW LEVEL CONFIGS //////////////////////////////////////////////////////////////////////
        public EngineConfig Engine { get; set; }
        public TrayNotificatorConfig Tray { get; set; }
        public PlaygroundConfig Playground { get; set; }
    }
}