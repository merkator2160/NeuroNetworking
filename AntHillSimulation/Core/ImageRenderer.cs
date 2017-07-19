using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Assets.GraficPrimitives;

namespace AntHillSimulation.Core
{
    public class ImageRenderer
    {
        private readonly PictureBox _display;


        public ImageRenderer(PictureBox display)
        {
            _display = display;
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        public void Render(IReadOnlyCollection<IGraficPrimtive> preparedPrimitives)
        {
            var canvas = new Bitmap(_display.Width, _display.Height);
            foreach (var x in preparedPrimitives)
            {
                x.Draw(canvas);
            }

            _display.Image = canvas;
            _display.Refresh();
        }
    }
}