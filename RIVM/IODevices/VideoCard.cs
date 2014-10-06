using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RIVM.IODevices
{
    public class VideoCard
    {
        private float VIDEO_WIDTH;
        private float VIDEO_HEIGHT;
        private int NUM_COLS = 80;        
        private int NUM_ROWS = 60;

        private Graphics _graphics;
        private Font _font;
        private System.Windows.Forms.Timer _timer;

        private byte[] _videoMemory;

        public VideoCard(Graphics graphics)
        {
            _videoMemory = new byte[NUM_COLS * NUM_ROWS];

            ResizeDisplay(graphics);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 100;
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        public void ResizeDisplay(Graphics newGraphics)
        {
            if (_graphics != null)
            {
                _graphics.Dispose();
            }

            _graphics = newGraphics;

            VIDEO_WIDTH = _graphics.VisibleClipBounds.Width;
            VIDEO_HEIGHT = _graphics.VisibleClipBounds.Height;

            _font = new Font(FontFamily.GenericMonospace, VIDEO_HEIGHT / NUM_ROWS * 0.9f);
        }

        public byte this[int address]
        {
            set
            {
                _videoMemory[address] = value;
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            DrawVideoMemory();
        }

        private void DrawVideoMemory()
        {
            int charWidth = (int)(VIDEO_WIDTH / NUM_COLS);
            int charHeight = (int)(VIDEO_HEIGHT / NUM_ROWS);

            using(var tempBmp = new Bitmap((int)VIDEO_WIDTH, (int)VIDEO_HEIGHT))
            using (var buffer = Graphics.FromImage(tempBmp))
            {
                buffer.FillRectangle(Brushes.Black, 0, 0, VIDEO_WIDTH, VIDEO_HEIGHT);

                for (int row = 0; row < NUM_ROWS; row++)
                {
                    for (int col = 0; col < NUM_COLS; col++)
                    {
                        char c = (char)_videoMemory[col + row * NUM_COLS];

                        buffer.DrawString(c.ToString(), _font, Brushes.White, col * charWidth, row * charHeight);
                    }
                }

                _graphics.DrawImage(tempBmp, 0, 0);
            }
        }
    }
}
