using System.Drawing;

namespace Assets.GraficPrimitives
{
    public class Circle : IGraficPrimtive
    {
        public void Draw(Bitmap canvas)
        {
            Graphics.FromImage(canvas).FillEllipse(Brushes.Crimson, 50, 50, 19, 19);
        }
    }
}