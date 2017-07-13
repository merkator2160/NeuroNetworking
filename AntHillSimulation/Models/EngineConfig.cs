using System;

namespace AntHillSimulation.Models
{
    internal class EngineConfig
    {
        public const Int32 DefaultSimulationSpeed = 100;
        public const String DefaultApplicationName = "Ant hive simulator";


        public EngineConfig()
        {
            SimulationSpeed = DefaultSimulationSpeed;
            ApplicationName = DefaultApplicationName;
        }


        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public Int32 SimulationSpeed { get; set; }
        public String ApplicationName { get; set; }
    }
}