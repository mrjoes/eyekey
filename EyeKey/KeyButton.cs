using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace EyeKey
{
    class KeyButton : Button
    {
        private bool active = false;
        private DateTime startDate;
        private DateTime curDate;
        private bool blinked = false;

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            if (active)
            {
                if (blinked)
                {
                    pevent.Graphics.FillRectangle(Brushes.Green, pevent.ClipRectangle);
                    blinked = false;
                }
                else
                {
                    Brush brush = Brushes.Red;

                    int milli = (int)(curDate - startDate).TotalMilliseconds;
                    if (milli > Consts.KeyDelay)
                    {
                        milli = Consts.KeyDelay;
                    }

                    int width = (int)((pevent.ClipRectangle.Width * (float)milli) / Consts.KeyDelay);

                    Rectangle rect = new Rectangle(pevent.ClipRectangle.Left, pevent.ClipRectangle.Bottom - 10, width, 8);

                    pevent.Graphics.FillRectangle(brush, rect);
                }
            }
        }

        public void Activate()
        {
            active = true;
            startDate = DateTime.Now;
            Invalidate();
        }

        public bool KeyUpdate()
        {
            if (active)
            {
                curDate = DateTime.Now;
                Invalidate();

                return (curDate - startDate).TotalMilliseconds >= Consts.KeyDelay;
            }

            return false;
        }

        public void Blink()
        {
            blinked = true;
            Invalidate();
        }

        public void Deactivate()
        {
            active = false;
            Invalidate();
        }
    }
}
