using Assets.GraficPrimitives;

namespace AntHillSimulation.Core
{
    public class PhysicalProcessor
    {
        public PhysicalProcessor()
        {

        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public IGraficPrimtive[] CalculatePrimitives()
        {
            return new IGraficPrimtive[] { new Circle() };
        }
    }
}