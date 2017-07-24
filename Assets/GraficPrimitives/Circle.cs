using Assets.Models;
using System;
using System.Drawing;

namespace Assets.GraficPrimitives
{
    public class Circle : IGraficPrimtive, IMapObject
    {
        // PROPERTIES /////////////////////////////////////////////////////////////////////////////
        public Brush Color { get; set; }


        // IMapObject /////////////////////////////////////////////////////////////////////////////
        public Dimensions Dimensions { get; set; }
        public Position Position { get; set; }
        public Int32 Radius { get; set; }

        public void CalculatePosition()
        {
            throw new NotImplementedException();
        }


        // IGraficPrimtive ////////////////////////////////////////////////////////////////////////
        public void Draw(Bitmap canvas)
        {
            Graphics.FromImage(canvas).FillEllipse(Color, 50, 50, 19, 19);
        }
    }
}