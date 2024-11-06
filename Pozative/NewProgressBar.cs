using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthReportCard
{
    public class NewProgressBar : ProgressBar
    {
        //[Description("Obtiene el Texto que se muestra dentro del Control"), Category("Behavior")]
        public override string Text { get; set; }
        private long _Value;

        public long Value
        {
            get { return _Value; }
            set
            {
                if (value > 100)
                    value = 100;
                _Value = value;
                Invalidate();
            }
        }

        public NewProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            this.Text = Convert.ToString(Convert.ToInt32((100 / 100) * _Value));
                  

            if (this.Text != string.Empty)
            {
                using (Bitmap bitmap = new Bitmap(this.Width, this.Height))
                {
                    using (Brush FontColor = new SolidBrush(this.ForeColor))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            int ShadowOffset = 2;
                            SizeF MS = graphics.MeasureString(this.Text, this.Font);
                            SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, this.ForeColor));

                            //Sombra del Texto:
                            graphics.DrawString(this.Text, this.Font, shadowBrush,
                                Convert.ToInt32(Width / 2 - MS.Width / 2) + ShadowOffset,
                                Convert.ToInt32(Height / 2 - MS.Height / 2) + ShadowOffset
                            );

                            //Texto del Control:
                            graphics.DrawString(this.Text, this.Font, FontColor,
                                Convert.ToInt32(Width / 2 - MS.Width / 2),
                                Convert.ToInt32(Height / 2 - MS.Height / 2));
                        }
                    }
                }
            }



            Rectangle rec = e.ClipRectangle;
            //rec.
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
            rec.Height = rec.Height - 4;
            e.Graphics.FillRectangle(Brushes.Orange, 2, 2, rec.Width, rec.Height);
            this.Text = _Value.ToString();
        }
    }
}
