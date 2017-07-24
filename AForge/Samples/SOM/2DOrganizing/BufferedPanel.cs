using System.Windows.Forms;

namespace _2DOrganizing
{
    /// <summary>
    /// Dummy class to enable  doble buffering in Panel
    /// </summary>
    public class BufferedPanel : Panel
    {
        // Construcor
        public BufferedPanel()
        {
            // Update control style
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
        }
    }
}
