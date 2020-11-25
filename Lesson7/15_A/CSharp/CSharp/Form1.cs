using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random r = new Random();
        Bitmap b;
        Graphics g;
        Font smallFont = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Pixel);
        int n = 100;
        int max = 20;
        int min = 1;
        int m = 1000;

        private void initializeGraphics()
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        }

        private void representPath()
        {
            Rectangle viewport = new Rectangle(20, 20, (pictureBox1.Width-21), (pictureBox1.Height / 3));
            Rectangle viewport2 = new Rectangle(20, (pictureBox1.Height / 3)+60, (pictureBox1.Width - 21), (pictureBox1.Height / 3));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window = max;
            double maxY_Window = 1;
            double minX_Window = min;
            double minY_Window = 0;
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;

            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);

            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero, y_Device), new Point(x_Device, y_Device));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero, y_zero), new Point(x_Device, y_zero));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device, y_Device), new Point(x_Device, y_zero));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero, y_zero), new Point(x_zero, y_Device));

            int x_Device2 = X_viewPort(maxX_Window, viewport2, minX_Window, RangeX);
            int y_Device2 = Y_viewPort(maxY_Window, viewport2, minY_Window, RangeY);
            int x_zero2 = X_viewPort(minX_Window, viewport2, minX_Window, RangeX);
            int y_zero2 = Y_viewPort(minY_Window, viewport2, minY_Window, RangeY);

            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero2, y_Device2), new Point(x_Device2, y_Device2));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero2, y_zero2), new Point(x_Device2, y_zero2));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device2, y_Device2), new Point(x_Device2, y_zero2));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero2, y_zero2), new Point(x_zero2, y_Device2));

            for (double i=0; i<=10; i++)
            {
                double point = i / 10.0;
                g.DrawString(point.ToString(), smallFont, Brushes.Black, new Point(x_zero-20, Y_viewPort(point, viewport, minY_Window, RangeY)));
                g.DrawString(point.ToString(), smallFont, Brushes.Black, new Point(x_zero2 - 20, Y_viewPort(point, viewport2, minY_Window, RangeY)));
            }

            Dictionary<int, int> freq = new Dictionary<int, int>();
            for (int i=min; i<max; i++)
            {
                freq.Add(i, 0);
                g.DrawLine(new Pen(Brushes.LightGray), new Point(X_viewPort(i, viewport, minX_Window, RangeX), y_zero), new Point(X_viewPort(i, viewport, minX_Window, RangeX), y_Device));
                g.DrawString(i.ToString(), smallFont, Brushes.Black, new Point(X_viewPort(i, viewport, minX_Window, RangeX), y_zero));

                g.DrawLine(new Pen(Brushes.LightGray), new Point(X_viewPort(i, viewport2, minX_Window, RangeX), y_zero2), new Point(X_viewPort(i, viewport2, minX_Window, RangeX), y_Device2));
                g.DrawString(i.ToString(), smallFont, Brushes.Black, new Point(X_viewPort(i, viewport2, minX_Window, RangeX), y_zero2));

            }

            List<int> paths = new List<int>();
            for(int i=0; i<m; i++)
            {
                int media = 0;
                for (int j=1; j<=n; j++)
                {
                    int obs = r.Next(min, max);
                    media = media + (obs - media) / j;

                }
                paths.Add(media);
            }

            List<int> keys = new List<int>(freq.Keys);
            foreach( int path in paths)
            {
                foreach(int k in keys)
                {
                    if(k == path)
                    {
                        freq[k]++;
                    }
                }
            }

            int count = 0;
            Point start = new Point(X_viewPort(minX_Window, viewport, minX_Window, RangeX), Y_viewPort(minY_Window, viewport, minY_Window, RangeY));
            foreach(KeyValuePair<int,int> kv in freq)
            {
                g.DrawLine(new Pen(Brushes.Blue), start, new Point(X_viewPort(kv.Key, viewport, minX_Window, RangeX), start.Y));
                start = new Point(X_viewPort(kv.Key, viewport, minX_Window, RangeX), start.Y);
                if (kv.Value>0)
                {
                    count += kv.Value;
                    g.DrawLine(new Pen(Brushes.Blue), start, new Point(start.X, Y_viewPort((double)count/(double)m, viewport, minY_Window, RangeY)));
                    start = new Point(start.X, Y_viewPort((double)count / (double)m, viewport, minY_Window, RangeY));
                }

                int rectx = X_viewPort(kv.Key, viewport2, minX_Window, RangeX);
                int recty = Y_viewPort(((double)kv.Value / (double)m), viewport2, minY_Window, RangeY);
                Rectangle rect = new Rectangle(rectx, recty, X_viewPort(kv.Key+1, viewport2, minX_Window, RangeX) - rectx, y_zero2 - recty);
                g.FillRectangle(Brushes.Blue, rect);
                g.DrawRectangle(Pens.Red, rect);

            }


            pictureBox1.Image = b;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (max>min)
            {
                initializeGraphics();
                representPath();
            }
            else
            {
                MessageBox.Show("max must be bigger than min!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        int X_viewPort(double x_World, Rectangle viewPort, double minX, double rangeX)
        {
            return (int)(viewPort.Left + ((viewPort.Width * (x_World - minX)) / rangeX));
        }

        int Y_viewPort(double y_World, Rectangle viewPort, double minY, double rangeY)
        {
            return (int)(viewPort.Top + viewPort.Height - ((viewPort.Height * (y_World - minY)) / rangeY));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            n = (int)numericUpDown1.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            min = (int)numericUpDown3.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            max = (int)numericUpDown2.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            m = (int)numericUpDown4.Value;
        }
    }
}
