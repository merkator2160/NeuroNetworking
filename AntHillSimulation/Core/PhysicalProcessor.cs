using Assets.GraficPrimitives;
using Assets.Models;

namespace AntHillSimulation.Core
{
    public class PhysicalProcessor
    {
        public PhysicalProcessor(Map map)
        {

        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public IGraficPrimtive[] CalculateMap()
        {
            return new IGraficPrimtive[] { new Circle() };
        }
    }
}