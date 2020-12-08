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
        Font smallFont = new Font("Calibri", 20, FontStyle.Regular, GraphicsUnit.Pixel);
        Font otherFont = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Pixel);
        double variance = 50;
        int m = 10;
        int n = 200;
        int splitter = 100;

        private void initializeGraphics()
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        }

        private void representPath()
        {
            
            Rectangle viewport = new Rectangle(20,(pictureBox1.Height/4), (pictureBox1.Width - 10) , ( pictureBox1.Height/2));
            g.DrawLine(new Pen(Brushes.Black), new Point(viewport.X, viewport.Y+viewport.Height), new Point(viewport.X, viewport.Y));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window = n+(n/4);
            double maxY_Window = n;
            double minX_Window = 0;
            double minY_Window = -n;
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;

            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);
            int x_real_zero = X_viewPort(0, viewport, minX_Window, RangeX);
            int y_real_zero = Y_viewPort(0, viewport, minY_Window, RangeY);

            int x_Device_less = X_viewPort(maxX_Window-(n/4), viewport, minX_Window, RangeX);
            int x_splitter = X_viewPort(splitter, viewport, minX_Window, RangeX);
            

            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device_less, y_Device), new Point(x_Device_less, y_zero));
            g.DrawString("N", smallFont, Brushes.Black, new Point(x_Device_less, y_Device-20));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_splitter, y_Device), new Point(x_splitter, y_zero));
            g.DrawString("J", smallFont, Brushes.Black, new Point(x_splitter, y_Device-20));
            g.DrawString(n.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_Device));
            g.DrawString("-" + n.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_zero - 3));
            g.DrawString("0", otherFont, Brushes.Black, new Point(x_real_zero - 10, y_real_zero - 10));

            for (int i=0; i<n; i+=10)
            {
                int x_index = X_viewPort( i, viewport, minX_Window, RangeX);
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(x_index, y_zero));

            }
            




            List<double> listj = new List<double>();
            List<double> listn = new List<double>();
            for (int i = 1; i<=m; i++)
            {
                double previousStep = 0;
                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                for (int j = 1; j<=n; j++)
                {
                    Point start = new Point(X_viewPort(j-1, viewport, minX_Window, RangeX), Y_viewPort(previousStep, viewport, minY_Window, RangeY));

                    double value = NextStandardGaussianDouble(r);
                    double randomStep = (float)(variance * Math.Sqrt(((double)1 / (double)n)) * value++);
                    double step = previousStep + randomStep;
                    previousStep = (float)step;
                    

                    if (j>1)
                    {        
                        g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY)));
                    }
                    else
                    {
                        g.DrawLine(new Pen(brush), new Point(X_viewPort(0, viewport, minX_Window, RangeX), Y_viewPort(0, viewport, minY_Window, RangeY)), 
                                            new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY)));
                    }

                    if(j==splitter)
                    {
                        listj.Add(step);
                    }

                    if (j == n)
                    {
                        listn.Add(step);
                    }

                }

                if(checkBox1.Checked)
                {
                    Dictionary<int, int> histoj = new Dictionary<int, int>();
                    for(int x=-n; x<n; x+=5)
                    {
                        histoj.Add(x, 0);
                    }

                    List<int> keysj = new List<int>(histoj.Keys);
                    foreach (double z in listj)
                    {
                        int x = (int)z;
                        foreach(int y in keysj)
                        {
                           
                            if(x>=y && x < (y + 5))
                            {
                                histoj[y]+=1;
                            }
                        }
                    }

                    foreach (KeyValuePair<int, int> y in histoj)
                    {
                        double par = (double)y.Key;
                        int y_height = Y_viewPort((par + 5), viewport, minY_Window, RangeY);
                        int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                        int x_width = X_viewPort(y.Value, viewport, minX_Window, RangeX);


                        Rectangle rect = new Rectangle(x_splitter, y_height, (int)y.Value * 10, y_freq - y_height);
                        
                        g.FillRectangle(Brushes.Blue, rect);
                        g.DrawRectangle(Pens.Red, rect);
                    }

                    Dictionary<int, int> histon = new Dictionary<int, int>();
                    for (int x = -n; x < n; x+=5)
                    {
                        histon.Add(x, 0);
                    }

                    List<int> keysn = new List<int>(histon.Keys);
                    foreach (double z in listn)
                    {
                        int x = (int)z;
                        foreach (int y in keysn)
                        {
          
                            if (x >= y && x < (y + 5))
                            {
                                histon[y] += 1;
                            }
                        }
                    }

                    foreach (KeyValuePair<int, int> y in histon)
                    {
                        double par = (double)y.Key;
                        int y_height = Y_viewPort((par + 5), viewport, minY_Window, RangeY);
                        int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                        int x_width = X_viewPort(y.Value, viewport, minX_Window, RangeX);


                        Rectangle rect = new Rectangle(x_Device_less, y_height, y.Value * 10, y_freq - y_height);
                        g.FillRectangle(Brushes.Blue, rect);
                        g.DrawRectangle(Pens.Red, rect);
                    }

                }
            }

            pictureBox1.Image = b;
        }

        private double NextStandardGaussianDouble(Random r)
        {
            double u, v, S;
            do
            {
                u = 2.0 * r.NextDouble() - 1.0;
                v = 2.0 * r.NextDouble() - 1.0;
                S = u * u + v * v;
            }
            while (S >= 1.0);

            double fac = Math.Sqrt(-2.0 * Math.Log(S) / S);
            return u * fac;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(splitter<n)
            {
                initializeGraphics();
                representPath();
            }
            else
            {
                MessageBox.Show("J must be smaller than N!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private double func_y(int p, int x)
        {
            return (double) ( p ^ (x* ((1 - p) ^ (1 - x)) ));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            m = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            n = (int)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            variance = (double)numericUpDown3.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            splitter = (int)numericUpDown5.Value;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
