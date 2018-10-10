using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PictureBox_Ext
{
    public class PictureBoxExt : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; }

        public PixelOffsetMode PixelOffsetMode { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.PixelOffsetMode = PixelOffsetMode;
            e.Graphics.InterpolationMode = InterpolationMode;

            base.OnPaint(e);
        }
    }
}
