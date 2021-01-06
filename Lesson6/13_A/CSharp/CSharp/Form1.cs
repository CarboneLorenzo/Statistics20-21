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
        Font smallFont = new Font("Calibri", 20, FontStyle.Regular, GraphicsUnit.Pixel);
        Font otherFont = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Pixel);

        //----------Bernoulli---------------

        double p = 50;
        int m = 1000;
        int n = 500;
        double epsilon = 0.10;
        int splitter = 100;


        private void representPathBern(Bitmap b, Graphics g)
        {
            g.DrawLine(new Pen(Brushes.Black), new Point(1, 1), new Point(1, pictureBox1.Height));
            Rectangle viewport = new Rectangle(1,(pictureBox1.Height/4), (pictureBox1.Width - 10) , ( pictureBox1.Height/2));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window = n+(n/4);
            double maxY_Window = 1;
            double minX_Window = 0;
            double minY_Window = 0;
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;

            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);

            int x_Device_less = X_viewPort(maxX_Window-(n/4), viewport, minX_Window, RangeX);
            int x_splitter = X_viewPort(splitter, viewport, minX_Window, RangeX);
            int y_prob = Y_viewPort((p/100.0), viewport, minY_Window, RangeY);
            int y_probUp = Y_viewPort((p / 100.0) + epsilon, viewport, minY_Window, RangeY);
            int y_probDown = Y_viewPort((p / 100.0) - epsilon, viewport, minY_Window, RangeY);

            g.DrawString("1", smallFont, Brushes.Black, new Point(x_zero, y_Device-20));
            g.DrawString("0", smallFont, Brushes.Black, new Point(x_zero, y_zero));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device_less, y_Device), new Point(x_Device_less, y_zero));
            g.DrawString("N", smallFont, Brushes.Black, new Point(x_Device_less, y_Device-20));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_splitter, y_Device), new Point(x_splitter, y_zero));
            g.DrawString("J", smallFont, Brushes.Black, new Point(x_splitter, y_Device-20));
            g.DrawLine(new Pen(Brushes.Green), new Point(x_zero, y_prob), new Point(x_Device, y_prob));
            g.DrawString("P", smallFont, Brushes.Black, new Point(x_Device-10, y_prob-5));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero, y_probUp), new Point(x_Device, y_probUp));
            g.DrawString("P+E", smallFont, Brushes.Black, new Point(x_Device-30, y_probUp-5));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero, y_probDown), new Point(x_Device, y_probDown));
            g.DrawString("P-E", smallFont, Brushes.Black, new Point(x_Device-30, y_probDown-5));

            List<double> listj = new List<double>();
            List<double> listn = new List<double>();
            for (int i = 1; i <= m; i++)
            {
                int aux = 0;
                double freq = 0;
                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                for (int j = 1; j <= n; j++)
                {
                    Point start = new Point(X_viewPort(j - 1, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY));
                    double x = r.NextDouble();
                    if (x > (p / 100.0))
                    {
                        x = 0;
                    }
                    else
                    {
                        x = 1;
                        aux++;
                    }

                    freq = (double)aux / (double)j;

                    if (j > 1)
                    {
                        g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y));

                        g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y),
                                            new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY)));
                    }
                    else
                    {
                        g.DrawLine(new Pen(brush), new Point(X_viewPort(0, viewport, minX_Window, RangeX), Y_viewPort(x, viewport, minY_Window, RangeY)),
                                            new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(x, viewport, minY_Window, RangeY)));

                        g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(x, viewport, minY_Window, RangeY)),
                                            new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY)));
                    }

                    if (j == splitter)
                    {
                        listj.Add(freq);
                    }

                    if (j == n)
                    {
                        listn.Add(freq);
                    }

                }
            }

            double countj = 0;
            foreach(double x in listj)
            {
                if((x < (p / 100.0) + epsilon) && (x > (p / 100.0) - epsilon))
                {
                    countj++;
                }
            }

            double countn = 0;
            foreach (double x in listn)
            {
                if ((x < (p / 100.0) + epsilon) && (x > (p / 100.0) - epsilon))
                {
                    countn++;
                }
            }

            richTextBox1.Text = "Paths between P+E and P-E at time J: " + countj + ";   frequency at time J: " + countj/(double)m + "\n" +
                                    "Paths between P+E and P-E at time N: " + countn + ";   frequency at time N: "  + countn / (double)m;

            if(checkBox1.Checked)
            {
                Dictionary<double, double> histoj = new Dictionary<double, double>();
                for(double x=0; x<10; x++)
                {
                    histoj.Add(x, 0);
                }

                List<double> keysj = new List<double>(histoj.Keys);
                foreach (double x in listj)
                {
                    foreach(double y in keysj)
                    {
                        double par = y/ 10.0;
                        if(x>=par && x < (par + 0.1))
                        {
                            histoj[y]+=1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histoj)
                {
                    double par = (double)y.Key / 10.0;
                    int y_height = Y_viewPort((par + 0.1), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n/4)*(y.Value/m), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_splitter, y_height, x_width-x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

                Dictionary<double, double> histon = new Dictionary<double, double>();
                for (int x = 0; x < 10; x++)
                {
                    histon.Add(x, 0);
                }

                List<double> keysn = new List<double>(histoj.Keys);
                foreach (double x in listn)
                {
                    foreach (double y in keysn)
                    {
                        double par = y / 10.0;
                        if (x >= par && x < (par + 0.1))
                        {
                            histon[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histon)
                {
                    double par = (double)y.Key / 10.0;
                    int y_height = Y_viewPort((par + 0.1), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort(((n / 4) * (y.Value/m)), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_Device_less, y_height, x_width-x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

            }
            

            pictureBox1.Image = b;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(splitter<n)
            {
                Bitmap b;
                Graphics g;
                b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(b);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                representPathBern(b, g);
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
            p = (double)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            epsilon = (double)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            splitter = (int)numericUpDown5.Value;
        }



        //-------Glivenko-Cantelli--------------

        int n_gc = 100;
        int max = 20;
        int min = 1;
        int m_gc = 1000;

        private void representPathGC(Bitmap b, Graphics g)
        {
            Rectangle viewport = new Rectangle(20, 20, (pictureBox2.Width - 21), (pictureBox2.Height / 3));
            Rectangle viewport2 = new Rectangle(20, (pictureBox2.Height / 3) + 60, (pictureBox2.Width - 21), (pictureBox2.Height / 3));
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

            for (double i = 0; i <= 10; i++)
            {
                double point = i / 10.0;
                g.DrawString(point.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, Y_viewPort(point, viewport, minY_Window, RangeY)));
                g.DrawString(point.ToString(), otherFont, Brushes.Black, new Point(x_zero2 - 20, Y_viewPort(point, viewport2, minY_Window, RangeY)));
            }

            Dictionary<int, int> freq = new Dictionary<int, int>();
            for (int i = min; i < max; i++)
            {
                freq.Add(i, 0);
                g.DrawLine(new Pen(Brushes.LightGray), new Point(X_viewPort(i, viewport, minX_Window, RangeX), y_zero), new Point(X_viewPort(i, viewport, minX_Window, RangeX), y_Device));
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(X_viewPort(i, viewport, minX_Window, RangeX), y_zero));

                g.DrawLine(new Pen(Brushes.LightGray), new Point(X_viewPort(i, viewport2, minX_Window, RangeX), y_zero2), new Point(X_viewPort(i, viewport2, minX_Window, RangeX), y_Device2));
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(X_viewPort(i, viewport2, minX_Window, RangeX), y_zero2));

            }

            List<double> paths = new List<double>();
            for (int i = 0; i < m_gc; i++)
            {
                double media = 0;
                for (int j = 1; j <= n_gc; j++)
                {
                    int obs = r.Next(min, max);
                    media = media + ((double)obs - media) / j;

                }
                paths.Add(media);
            }

            List<int> keys = new List<int>(freq.Keys);
            foreach (int path in paths)
            {
                foreach (int k in keys)
                {
                    if (k == (int)path)
                    {
                        freq[k]++;
                    }
                }
            }

            paths.Sort();
            int count = 0;
            Point start = new Point(X_viewPort(minX_Window, viewport, minX_Window, RangeX), Y_viewPort(minY_Window, viewport, minY_Window, RangeY));
            foreach (double mean in paths)
            {
                g.DrawLine(new Pen(Brushes.Blue), start, new Point(X_viewPort(mean, viewport, minX_Window, RangeX), start.Y));
                start = new Point(X_viewPort(mean, viewport, minX_Window, RangeX), start.Y);
                count++;
                g.DrawLine(new Pen(Brushes.Blue), start, new Point(start.X, Y_viewPort((double)count / (double)m_gc, viewport, minY_Window, RangeY)));
                start = new Point(start.X, Y_viewPort((double)count / (double)m_gc, viewport, minY_Window, RangeY));
            }

            foreach (KeyValuePair<int, int> kv in freq)
            {
                int rectx = X_viewPort(kv.Key, viewport2, minX_Window, RangeX);
                int recty = Y_viewPort(((double)kv.Value / (double)m_gc), viewport2, minY_Window, RangeY);
                Rectangle rect = new Rectangle(rectx, recty, X_viewPort(kv.Key + 1, viewport2, minX_Window, RangeX) - rectx, y_zero2 - recty);
                g.FillRectangle(Brushes.Blue, rect);
                g.DrawRectangle(Pens.Red, rect);
            }


            pictureBox2.Image = b;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (max > min)
            {
                Bitmap b;
                Graphics g;
                b = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                g = Graphics.FromImage(b);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                representPathGC(b, g);
            }
            else
            {
                MessageBox.Show("max must be bigger than min!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            m_gc = (int)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            n_gc = (int)numericUpDown7.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            max = (int)numericUpDown8.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            min = (int)numericUpDown9.Value;
        }

        //-------Rademacher---------------

        int m_r = 1000;
        int splitter_r = 50;
        int n_r = 1000;

        private void representPathR(Bitmap b, Graphics g)
        {
            g.DrawLine(new Pen(Brushes.Black), new Point(1, 1), new Point(1, pictureBox3.Height));
            Rectangle viewport = new Rectangle(3, (pictureBox3.Height / 10), (pictureBox3.Width - 10), (pictureBox3.Height - (pictureBox3.Height/8) - 10));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window = n_r + (n_r / 4);
            double maxY_Window = n_r/4;
            double minX_Window = 0;
            double minY_Window = -(n_r/4);
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;

            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);

            int x_Device_less = X_viewPort(maxX_Window - (n_r / 4), viewport, minX_Window, RangeX);
            int x_splitter = X_viewPort(splitter_r, viewport, minX_Window, RangeX);
           
            g.DrawString((n_r/4).ToString(), smallFont, Brushes.Black, new Point(x_zero, y_Device - 20));
            g.DrawString((-n_r / 4).ToString(), smallFont, Brushes.Black, new Point(x_zero, y_zero));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device_less, y_Device), new Point(x_Device_less, y_zero));
            g.DrawString("N", smallFont, Brushes.Black, new Point(x_Device_less, y_Device - 20));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_splitter, y_Device), new Point(x_splitter, y_zero));
            g.DrawString("J", smallFont, Brushes.Black, new Point(x_splitter, y_Device - 20));

            List<double> listj = new List<double>();
            List<double> listn = new List<double>();
            for (int i = 1; i <= m_r; i++)
            {

                double freq = 0;
                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                for (int j = 1; j <= n_r; j++)
                {
                    Point start = new Point(X_viewPort(j - 1, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY));
                    double x = r.NextDouble();
                    if (x > (50 / 100.0))
                    {
                        x = -1;
                    }
                    else
                    {
                        x = 1;
                    }

                    freq += x;

                    g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y));

                    g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y),
                                               new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY)));


                    if (j == splitter_r)
                    {
                        listj.Add(freq);
                    }

                    if (j == n_r)
                    {
                        listn.Add(freq);
                    }

                }
            }


            if (checkBox2.Checked)
            {
                Dictionary<double, double> histoj = new Dictionary<double, double>();
                for (int x = (-n_r); x < n_r; x += 10)
                {
                    histoj.Add(x, 0);
                }

                List<double> keysj = new List<double>(histoj.Keys);
                foreach (double x in listj)
                {
                    foreach (double y in keysj)
                    {

                        if (x >= y && x < (y + 10))
                        {
                            histoj[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histoj)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 10), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_r / 4) * (y.Value / m_r), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_splitter, y_height, x_width-x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

                Dictionary<double, double> histon = new Dictionary<double, double>();
                for (int x = (-n_r); x < n_r; x += 10)
                {
                    histon.Add(x, 0);
                }

                List<double> keysn = new List<double>(histoj.Keys);
                foreach (double x in listn)
                {
                    foreach (double y in keysn)
                    {

                        if (x >= y && x < (y + 10))
                        {
                            histon[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histon)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 10), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_r / 4) * (y.Value / m_r), viewport, minX_Window, RangeX);
                        


                    Rectangle rect = new Rectangle(x_Device_less, y_height, x_width-x_zero , y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

            }
            

            pictureBox3.Image = b;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (splitter_r < n_r)
            {
                Bitmap b;
                Graphics g;
                b = new Bitmap(pictureBox3.Width, pictureBox3.Height);
                g = Graphics.FromImage(b);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                representPathR(b, g);
            }
            else
            {
                MessageBox.Show("J must be smaller than N!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            m_r = (int)numericUpDown10.Value;
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            splitter_r = (int)numericUpDown11.Value;
        }

        private void numericUpDown12_ValueChanged_1(object sender, EventArgs e)
        {
            n_r = (int)numericUpDown12.Value;
        }

        //------------Poisson-------------

        double lam = 50;
        int m_p = 500;
        int n_p = 400;
        int splitter_p = 100;

        private void representPathPoisson(Bitmap b, Graphics g)
        {
            g.DrawLine(new Pen(Brushes.Black), new Point(1, 1), new Point(1, pictureBox4.Height));
            Rectangle viewport = new Rectangle(1, 20, (pictureBox4.Width - 10), (pictureBox4.Height - 40));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window = n_p + (n_p / 4);
            double maxY_Window = n_p;
            double minX_Window = 0;
            double minY_Window = 0;
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;
            numericUpDown16.Maximum = n_p;

            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);

            int x_Device_less = X_viewPort(maxX_Window - (n_p / 4), viewport, minX_Window, RangeX);
            int x_splitter = X_viewPort(splitter_p, viewport, minX_Window, RangeX);
            int y_prob = Y_viewPort((lam / 100.0), viewport, minY_Window, RangeY);

            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device_less, y_Device), new Point(x_Device_less, y_zero));
            g.DrawString("N", smallFont, Brushes.Black, new Point(x_Device_less, y_Device - 20));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_splitter, y_Device), new Point(x_splitter, y_zero));
            g.DrawString("J", smallFont, Brushes.Black, new Point(x_splitter, y_Device - 20));
            g.DrawLine(new Pen(Brushes.Green), new Point(x_zero, y_prob), new Point(x_Device, y_prob));
            for (int i = 0; i < n_p; i += 10)
            {
                int x_index = X_viewPort(i, viewport, minX_Window, RangeX);
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(x_index, y_zero));

                int y_index = Y_viewPort(i, viewport, minY_Window, RangeY);
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(x_zero, y_index));
            }

            SortedDictionary<double, double> distjumps = new SortedDictionary<double, double>();
            SortedDictionary<double, double> jumpfreq = new SortedDictionary<double, double>();
            for(double i=1; i<=n_p; i++)
            {
                jumpfreq.Add(i, 0);
            }
            List<double> listj = new List<double>();
            List<double> listn = new List<double>();
            double jumps = 0;
            for (int i = 1; i <= m_p; i++)
            {
                double freq = 0;
                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                double last = 0;
                for (int j = 1; j <= n_p; j++)
                {
                    Point start = new Point(X_viewPort(j - 1, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY));
                    double x = r.NextDouble();
                    if (x > (lam * (1.00 / (double)n_p)))
                    {
                        x = 0;
                    }
                    else
                    {
                        x = 1;
                        jumpfreq[j]++;
                        if(last > 0)
                        {
                            double dist = j - last;
                            if (distjumps.ContainsKey(dist))
                            {
                                distjumps[dist]++;
                            }
                            else
                            {
                                distjumps.Add(dist, 1);
                            }
                            
                        }
                        last = j;
                        jumps++;
                    }

                    freq += (double)x;



                    g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y));

                    g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y), 
                                                new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY)));
 

                    if (j == splitter_p)
                    {
                        listj.Add(freq);
                    }

                    if (j == n_p)
                    {
                        listn.Add(freq);
                    }

                }
            }

            
            richTextBox2.Text = "";
            richTextBox2.AppendText("Distribution of consecutive jumps: \n");
            richTextBox2.AppendText("Time elapsed\tfreq \n");
            foreach(KeyValuePair<double,double> k in distjumps)
            {
                richTextBox2.AppendText(k.Key.ToString() + "\t" + (k.Value/(jumps-m_p)).ToString() + "\n");
            }
            richTextBox2.AppendText("------------------------------------\n");
            richTextBox2.AppendText("Distribution of individual jumps: \n");
            richTextBox2.AppendText("Time elapsed\tfreq \n");
            foreach (KeyValuePair<double, double> k in jumpfreq)
            {
                richTextBox2.AppendText(k.Key.ToString() + "\t" + (k.Value / jumps).ToString() + "\n");
            }






            if (checkBox3.Checked)
            {
                Dictionary<double, double> histoj = new Dictionary<double, double>();
                for (int x = 0; x < n_p; x+=5)
                {
                    histoj.Add(x, 0);
                }

                List<double> keysj = new List<double>(histoj.Keys);
                foreach (double x in listj)
                {
                    foreach (double y in keysj)
                    {

                        if (x >= y && x < (y + 5))
                        {
                            histoj[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histoj)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 5), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_p / 4) * (y.Value / m_p), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_splitter, y_height, x_width-x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

                Dictionary<double, double> histon = new Dictionary<double, double>();
                for (int x = 0; x < n_p; x+=5)
                {
                    histon.Add(x, 0);
                }

                List<double> keysn = new List<double>(histoj.Keys);
                foreach (double x in listn)
                {
                    foreach (double y in keysn)
                    {

                        if (x >= y && x < (y + 5))
                        {
                            histon[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histon)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 5), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_p / 4) * (y.Value / m_p), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_Device_less, y_height, x_width-x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

            }
            

            pictureBox4.Image = b;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (splitter_p < n_p)
            {
                Bitmap b;
                Graphics g;
                b = new Bitmap(pictureBox4.Width, pictureBox4.Height);
                g = Graphics.FromImage(b);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                representPathPoisson(b, g);
            }
            else
            {
                MessageBox.Show("J must be smaller than N!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            m_p = (int)numericUpDown13.Value;
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            n_p = (int)numericUpDown15.Value;
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            lam = (int)numericUpDown16.Value;
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            splitter_p = (int)numericUpDown14.Value;
        }

        //-----------------Brownian Motion-------------------------

        double variance = 50;
        int m_b = 1000;
        int n_b = 500;
        int splitter_b = 100;

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

        private void representPathBrownian(Bitmap b, Graphics g)
        {

            Rectangle viewport = new Rectangle(20, 20, (pictureBox5.Width - 10), (pictureBox5.Height - 40));
            g.DrawLine(new Pen(Brushes.Black), new Point(viewport.X, viewport.Y + viewport.Height), new Point(viewport.X, viewport.Y));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window = n_b + (n_b / 4);
            double maxY_Window = n_b;
            double minX_Window = 0;
            double minY_Window = -n_b;
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;

            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);
            int x_real_zero = X_viewPort(0, viewport, minX_Window, RangeX);
            int y_real_zero = Y_viewPort(0, viewport, minY_Window, RangeY);

            int x_Device_less = X_viewPort(maxX_Window - (n_b / 4), viewport, minX_Window, RangeX);
            int x_splitter = X_viewPort(splitter_b, viewport, minX_Window, RangeX);


            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device_less, y_Device), new Point(x_Device_less, y_zero));
            g.DrawString("N", smallFont, Brushes.Black, new Point(x_Device_less, y_Device - 20));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_splitter, y_Device), new Point(x_splitter, y_zero));
            g.DrawString("J", smallFont, Brushes.Black, new Point(x_splitter, y_Device - 20));
            g.DrawString(maxX_Window.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_Device));
            g.DrawString(minX_Window.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_zero - 3));
            g.DrawString("0", otherFont, Brushes.Black, new Point(x_real_zero - 10, y_real_zero - 10));

            for (int i = 0; i < n_b; i += 10)
            {
                int x_index = X_viewPort(i, viewport, minX_Window, RangeX);
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(x_index, y_zero));

            }





            List<double> listj = new List<double>();
            List<double> listn = new List<double>();
            for (int i = 1; i <= m_b; i++)
            {
                double previousStep = 0;
                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                for (int j = 1; j <= n_b; j++)
                {
                    Point start = new Point(X_viewPort(j - 1, viewport, minX_Window, RangeX), Y_viewPort(previousStep, viewport, minY_Window, RangeY));

                    double value = NextStandardGaussianDouble(r);
                    double randomStep = (float)(variance * Math.Sqrt(((double)1 / (double)n_b)) * value++);
                    double step = previousStep + randomStep;
                    previousStep = (float)step;


                    if (j > 1)
                    {
                        g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y));

                        g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y),
                                                    new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY)));
                    }
                    else
                    {
                        g.DrawLine(new Pen(brush), new Point(X_viewPort(0, viewport, minX_Window, RangeX), Y_viewPort(0, viewport, minY_Window, RangeY)),
                                            new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(0, viewport, minY_Window, RangeY)));

                        g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(0, viewport, minY_Window, RangeY)),
                                            new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY)));
                    }

                    if (j == splitter_b)
                    {
                        listj.Add(step);
                    }

                    if (j == n_b)
                    {
                        listn.Add(step);
                    }

                }
            }

            if (checkBox4.Checked)
            {
                Dictionary<double, double> histoj = new Dictionary<double, double>();
                for (int x = (-n_b); x < n_b; x += 10)
                {
                    histoj.Add(x, 0);
                }

                List<double> keysj = new List<double>(histoj.Keys);
                foreach (double x in listj)
                {
                    foreach (double y in keysj)
                    {

                        if (x >= y && x < (y + 10))
                        {
                            histoj[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histoj)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 10), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_b / 4) * (y.Value / m_b), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_splitter, y_height, x_width - x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

                Dictionary<double, double> histon = new Dictionary<double, double>();
                for (int x = (-n_b); x < n_b; x += 10)
                {
                    histon.Add(x, 0);
                }

                List<double> keysn = new List<double>(histoj.Keys);
                foreach (double x in listn)
                {
                    foreach (double y in keysn)
                    {

                        if (x >= y && x < (y + 10))
                        {
                            histon[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histon)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 10), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_b / 4) * (y.Value / m_b), viewport, minX_Window, RangeX);



                    Rectangle rect = new Rectangle(x_Device_less, y_height, x_width - x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

            }
            

            pictureBox5.Image = b;
        }



        private void button5_Click(object sender, EventArgs e)
        {
            if (splitter_b < n_b)
            {
                Bitmap b;
                Graphics g;
                b = new Bitmap(pictureBox5.Width, pictureBox5.Height);
                g = Graphics.FromImage(b);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                representPathBrownian(b, g);
            }
            else
            {
                MessageBox.Show("J must be smaller than N!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDown17_ValueChanged(object sender, EventArgs e)
        {
            m_b = (int)numericUpDown17.Value;
        }

        private void numericUpDown19_ValueChanged(object sender, EventArgs e)
        {
            n_b = (int)numericUpDown19.Value;
        }

        private void numericUpDown20_ValueChanged(object sender, EventArgs e)
        {
            variance = (int)numericUpDown20.Value;
        }

        private void numericUpDown18_ValueChanged(object sender, EventArgs e)
        {
            splitter_b = (int)numericUpDown18.Value;
        }

        //------------GBM-----------------------

        double sigma = 1;
        double mu = 0;
        int m_gbm = 500;
        int n_gbm = 500;
        int splitter_gbm = 100;

        private void representPathGBM(Bitmap b, Graphics g)
        {

            Rectangle viewport = new Rectangle(20, 20, (pictureBox6.Width - 10), (pictureBox6.Height - 40));
            g.DrawLine(new Pen(Brushes.Black), new Point(viewport.X, viewport.Y + viewport.Height), new Point(viewport.X, viewport.Y));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window = n_gbm + (n_gbm / 4);
            double maxY_Window = n_gbm*2;
            double minX_Window = 0;
            double minY_Window = -n_gbm/2;
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;

            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);
            int x_real_zero = X_viewPort(0, viewport, minX_Window, RangeX);
            int y_real_zero = Y_viewPort(0, viewport, minY_Window, RangeY);

            int x_Device_less = X_viewPort(maxX_Window - (n_gbm / 4), viewport, minX_Window, RangeX);
            int x_splitter = X_viewPort(splitter_gbm, viewport, minX_Window, RangeX);


            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device_less, y_Device), new Point(x_Device_less, y_zero));
            g.DrawString("N", smallFont, Brushes.Black, new Point(x_Device_less, y_Device - 20));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_splitter, y_Device), new Point(x_splitter, y_zero));
            g.DrawString("J", smallFont, Brushes.Black, new Point(x_splitter, y_Device - 20));
            g.DrawString(maxY_Window.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_Device));
            g.DrawString(minY_Window.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_zero - 3));
            g.DrawString("0", otherFont, Brushes.Black, new Point(x_real_zero - 10, y_real_zero - 10));

            for (int i = 0; i < n_gbm; i += 10)
            {
                int x_index = X_viewPort(i, viewport, minX_Window, RangeX);
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(x_index, y_zero));

            }


            double t = (1 / (double)n_gbm);


            List<double> listj = new List<double>();
            List<double> listn = new List<double>();
            for (int i = 1; i <= m_gbm; i++)
            {
                double step = 10;
                double value = -0.5 * sigma * sigma * t + sigma * Math.Sqrt(t);

                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                for (int j = 1; j <= n_gbm; j++)
                {
                    Point start = new Point(X_viewPort(j - 1, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY));

                    double stdDoubles = NextStandardGaussianDouble(r);
                    double normalValue = mu * t + sigma * stdDoubles;
                    double newstep = Math.Exp(value * normalValue);
                    step *= newstep;


                    g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y));

                    g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y),
                                                new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY)));

                    if (j == splitter_gbm)
                    {
                        listj.Add(step);
                    }

                    if (j == n_gbm)
                    {
                        listn.Add(step);
                    }

                }
            }

            if (checkBox5.Checked)
            {
                Dictionary<double, double> histoj = new Dictionary<double, double>();
                for (int x = (-n_gbm/2); x < n_gbm*2; x += 10)
                {
                    histoj.Add(x, 0);
                }

                List<double> keysj = new List<double>(histoj.Keys);
                foreach (double x in listj)
                {
                    foreach (double y in keysj)
                    {

                        if (x >= y && x < (y + 10))
                        {
                            histoj[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histoj)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 10), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_gbm / 4) * (y.Value / m_gbm), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_splitter, y_height, x_width - x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

                Dictionary<double, double> histon = new Dictionary<double, double>();
                for (int x = (-n_gbm/2); x < n_gbm*2; x += 10)
                {
                    histon.Add(x, 0);
                }

                List<double> keysn = new List<double>(histoj.Keys);
                foreach (double x in listn)
                {
                    foreach (double y in keysn)
                    {

                        if (x >= y && x < (y + 10))
                        {
                            histon[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histon)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 10), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_gbm / 4) * (y.Value / m_gbm), viewport, minX_Window, RangeX);



                    Rectangle rect = new Rectangle(x_Device_less, y_height, x_width - x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

            }
            

            pictureBox6.Image = b;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (splitter_gbm < n_gbm)
            {
                Bitmap b;
                Graphics g;
                b = new Bitmap(pictureBox6.Width, pictureBox6.Height);
                g = Graphics.FromImage(b);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                representPathGBM(b, g);
            }
            else
            {
                MessageBox.Show("J must be smaller than N!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDown21_ValueChanged(object sender, EventArgs e)
        {
            m_gbm = (int)numericUpDown21.Value;
        }

        private void numericUpDown23_ValueChanged(object sender, EventArgs e)
        {
            n_gbm = (int)numericUpDown23.Value;
        }

        private void numericUpDown24_ValueChanged(object sender, EventArgs e)
        {
            mu = (double)numericUpDown24.Value;
        }

        private void numericUpDown22_ValueChanged(object sender, EventArgs e)
        {
            splitter_gbm = (int)numericUpDown22.Value;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            sigma = 0.75;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            sigma = 1.0;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            sigma = 1.25;
        }

        //----------------Ornstein-Uhlenbeck-----------------

        double sigma_o = 1;
        double equilibrium = 100;
        int m_o = 1000;
        int n_o = 500;
        int splitter_o = 100;
        double reversion = 0.1;
        

        private void representPathOU(Bitmap b, Graphics g)
        {
            
            Rectangle viewport = new Rectangle(20, 20, (pictureBox7.Width - 10), (pictureBox7.Height - 40));
            g.DrawLine(new Pen(Brushes.Black), new Point(viewport.X, viewport.Y + viewport.Height), new Point(viewport.X, viewport.Y));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window =  n_o + (n_o / 4);
            double maxY_Window = 110 ;
            double minX_Window = 0;
            double minY_Window = 90;
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;
            

            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);
            int x_real_zero = X_viewPort(0, viewport, minX_Window, RangeX);
            int y_real_zero = Y_viewPort(0, viewport, minY_Window, RangeY);
            int y_starting = Y_viewPort(100, viewport, minY_Window, RangeY);

            int x_Device_less = X_viewPort(maxX_Window - (n_o / 4), viewport, minX_Window, RangeX);
            int x_splitter = X_viewPort(splitter_o, viewport, minX_Window, RangeX);


            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device_less, y_Device), new Point(x_Device_less, y_zero));
            g.DrawString("N", smallFont, Brushes.Black, new Point(x_Device_less, y_Device - 20));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_splitter, y_Device), new Point(x_splitter, y_zero));
            g.DrawString("J", smallFont, Brushes.Black, new Point(x_splitter, y_Device - 20));
            g.DrawString(maxY_Window.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_Device));
            g.DrawString(minY_Window.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_zero - 3));
            g.DrawString("100", otherFont, Brushes.Black, new Point(x_real_zero - 20, y_starting-3));

            for (int i = 0; i < n_o; i += 10)
            {
                int x_index = X_viewPort(i, viewport, minX_Window, RangeX);
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(x_index, y_zero));

            }


            double t = (1 / (double)n_o);


            List<double> listj = new List<double>();
            List<double> listn = new List<double>();
            for (int i = 1; i <= m_o; i++)
            {
                double step = 100;

                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                for (int j = 1; j <= n_o; j++)
                {
                    Point start = new Point(X_viewPort(j - 1, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY));

                    double stdDoubles = NextStandardGaussianDouble(r);
                    double newstep = (reversion * (equilibrium - step) * t) + (sigma_o * stdDoubles * Math.Sqrt(t));
                    step += newstep;


                    g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y));

                    g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y),
                                               new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY)));

                    if (j == splitter_o)
                    {
                        listj.Add(step);
                    }

                    if (j == n_o)
                    {
                        listn.Add(step);
                    }

                }
            }

            if (checkBox6.Checked)
            {
                Dictionary<double, double> histoj = new Dictionary<double, double>();
                for (double x = minY_Window; x < maxY_Window; x ++)
                {
                    histoj.Add(x, 0);
                }

                List<double> keysj = new List<double>(histoj.Keys);
                foreach (double x in listj)
                {
                    foreach (double y in keysj)
                    {

                        if (x >= y && x < (y + 1))
                        {
                            histoj[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histoj)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 1), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_o / 4) * (y.Value / m_o), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_splitter, y_height, x_width - x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

                Dictionary<double, double> histon = new Dictionary<double, double>();
                for (double x = minY_Window; x < maxY_Window; x ++)
                {
                    histon.Add(x, 0);
                }

                List<double> keysn = new List<double>(histoj.Keys);
                foreach (double x in listn)
                {
                    foreach (double y in keysn)
                    {

                        if (x >= y && x < (y + 1))
                        {
                            histon[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histon)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 1), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_o / 4) * (y.Value / m_o), viewport, minX_Window, RangeX);



                    Rectangle rect = new Rectangle(x_Device_less, y_height, x_width - x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

            }
            

            pictureBox7.Image = b;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (splitter_o < n_o)
            {
                Bitmap b;
                Graphics g;
                b = new Bitmap(pictureBox7.Width, pictureBox7.Height);
                g = Graphics.FromImage(b);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                representPathOU(b, g);
            }
            else
            {
                MessageBox.Show("J must be smaller than N!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDown25_ValueChanged(object sender, EventArgs e)
        {
            m_o = (int)numericUpDown25.Value;
        }

        private void numericUpDown27_ValueChanged(object sender, EventArgs e)
        {
            n_o = (int)numericUpDown27.Value;
        }

        private void numericUpDown28_ValueChanged(object sender, EventArgs e)
        {
            equilibrium = (double)numericUpDown28.Value;
        }

        private void numericUpDown26_ValueChanged(object sender, EventArgs e)
        {
            splitter_o = (int)numericUpDown26.Value;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            sigma_o = 0.75;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            sigma_o = 1.0;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            sigma_o = 1.25;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            reversion = 0.1;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            reversion = 0.5;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            reversion = 2.0;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            reversion = 10;
        }

        //------------CIR------------

        double sigma_cir = 1;
        double equilibrium_cir = 100;
        int m_cir = 1000;
        int n_cir = 500;
        int splitter_cir = 100;
        double reversion_cir = 0.1;

        private void representPathCIR(Bitmap b, Graphics g)
        {

            Rectangle viewport = new Rectangle(20, 20, (pictureBox8.Width - 10), (pictureBox8.Height - 40));
            g.DrawLine(new Pen(Brushes.Black), new Point(viewport.X, viewport.Y + viewport.Height), new Point(viewport.X, viewport.Y));
            //g.DrawRectangle(Pens.Black, viewport);
            double maxX_Window = n_cir + (n_cir / 4);
            double maxY_Window = 180;
            double minX_Window = 0;
            double minY_Window = 20;
            double RangeX = maxX_Window - minX_Window;
            double RangeY = maxY_Window - minY_Window;


            int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
            int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
            int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
            int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);
            int x_real_zero = X_viewPort(0, viewport, minX_Window, RangeX);
            int y_real_zero = Y_viewPort(0, viewport, minY_Window, RangeY);
            int y_starting = Y_viewPort(100, viewport, minY_Window, RangeY);

            int x_Device_less = X_viewPort(maxX_Window - (n_cir / 4), viewport, minX_Window, RangeX);
            int x_splitter = X_viewPort(splitter_cir, viewport, minX_Window, RangeX);


            g.DrawLine(new Pen(Brushes.Black), new Point(x_Device_less, y_Device), new Point(x_Device_less, y_zero));
            g.DrawString("N", smallFont, Brushes.Black, new Point(x_Device_less, y_Device - 20));
            g.DrawLine(new Pen(Brushes.Black), new Point(x_splitter, y_Device), new Point(x_splitter, y_zero));
            g.DrawString("J", smallFont, Brushes.Black, new Point(x_splitter, y_Device - 20));
            g.DrawString(maxY_Window.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_Device));
            g.DrawString(minY_Window.ToString(), otherFont, Brushes.Black, new Point(x_zero - 20, y_zero - 3));
            g.DrawString("0", otherFont, Brushes.Black, new Point(x_real_zero - 10, y_real_zero - 10));
            g.DrawString("100", otherFont, Brushes.Black, new Point(x_real_zero - 20, y_starting - 3));

            for (int i = 0; i < n_cir; i += 10)
            {
                int x_index = X_viewPort(i, viewport, minX_Window, RangeX);
                g.DrawString(i.ToString(), otherFont, Brushes.Black, new Point(x_index, y_zero));

            }


            double t = (1 / (double)n_cir);


            List<double> listj = new List<double>();
            List<double> listn = new List<double>();
            for (int i = 1; i <= m_cir; i++)
            {
                double step = 100;

                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                for (int j = 1; j <= n_cir; j++)
                {
                    Point start = new Point(X_viewPort(j - 1, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY));

                    double stdDoubles = NextStandardGaussianDouble(r);
                    double newstep = (reversion_cir * (equilibrium_cir - step) * t) + (sigma_cir * Math.Sqrt(step) * stdDoubles * Math.Sqrt(t));
                    step += newstep;


                    g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y));

                    g.DrawLine(new Pen(brush), new Point(X_viewPort(j, viewport, minX_Window, RangeX), start.Y),
                                               new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(step, viewport, minY_Window, RangeY)));

                    if (j == splitter_cir)
                    {
                        listj.Add(step);
                    }

                    if (j == n_cir)
                    {
                        listn.Add(step);
                    }

                }
            }

            if (checkBox7.Checked)
            {
                Dictionary<double, double> histoj = new Dictionary<double, double>();
                for (double x = minY_Window; x < maxY_Window; x+=5)
                {
                    histoj.Add(x, 0);
                }

                List<double> keysj = new List<double>(histoj.Keys);
                foreach (double x in listj)
                {
                    foreach (double y in keysj)
                    {

                        if (x >= y && x < (y + 5))
                        {
                            histoj[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histoj)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 5), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_cir / 4) * (y.Value / m_cir), viewport, minX_Window, RangeX);


                    Rectangle rect = new Rectangle(x_splitter, y_height, x_width - x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

                Dictionary<double, double> histon = new Dictionary<double, double>();
                for (double x = minY_Window; x < maxY_Window; x+=5)
                {
                    histon.Add(x, 0);
                }

                List<double> keysn = new List<double>(histoj.Keys);
                foreach (double x in listn)
                {
                    foreach (double y in keysn)
                    {

                        if (x >= y && x < (y + 5))
                        {
                            histon[y] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<double, double> y in histon)
                {
                    double par = (double)y.Key;
                    int y_height = Y_viewPort((par + 5), viewport, minY_Window, RangeY);
                    int y_freq = Y_viewPort(par, viewport, minY_Window, RangeY);
                    int x_width = X_viewPort((n_cir / 4) * (y.Value / m_cir), viewport, minX_Window, RangeX);



                    Rectangle rect = new Rectangle(x_Device_less, y_height, x_width - x_zero, y_freq - y_height);
                    g.FillRectangle(Brushes.Blue, rect);
                    g.DrawRectangle(Pens.Red, rect);
                }

            }
            

            pictureBox8.Image = b;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (splitter_cir < n_cir)
            {
                Bitmap b;
                Graphics g;
                b = new Bitmap(pictureBox8.Width, pictureBox8.Height);
                g = Graphics.FromImage(b);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                representPathCIR(b, g);
            }
            else
            {
                MessageBox.Show("J must be smaller than N!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void numericUpDown29_ValueChanged(object sender, EventArgs e)
        {
            m_cir = (int)numericUpDown29.Value;
        }

        private void numericUpDown31_ValueChanged(object sender, EventArgs e)
        {
            n_cir = (int)numericUpDown31.Value;
        }

        private void numericUpDown32_ValueChanged(object sender, EventArgs e)
        {
            equilibrium_cir = (int)numericUpDown32.Value;
        }

        private void numericUpDown30_ValueChanged(object sender, EventArgs e)
        {
            splitter_cir = (int)numericUpDown30.Value;
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            sigma_cir = 0.75;
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            sigma_cir = 1.0;
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            sigma_cir = 1.25;
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            reversion_cir = 0.1;
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            reversion_cir = 0.5;
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            reversion_cir = 2.0;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            reversion_cir = 10;
        }
    }
}
