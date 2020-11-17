﻿using System;
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
        double p = 50;
        int m = 10;
        int n = 200;
        double epsilon = 0.10;
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
            for (int i = 1; i<=m; i++)
            {
                int aux = 0;
                double freq = 0;
                Color randomColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Brush brush = new SolidBrush(randomColor);
                for (int j = 1; j<=n; j++)
                {
                    Point start = new Point(X_viewPort(j-1, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY));
                    double x = r.NextDouble();
                    if (x > (p/100.0) )
                    {
                        x = 0;
                    }
                    else
                    {
                        x = 1;
                        aux++;
                    }

                    freq = (double) aux / (double) j;

                    if (j>1)
                    {        
                        g.DrawLine(new Pen(brush), start, new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY)));
                    }
                    else
                    {
                        g.DrawLine(new Pen(brush), new Point(X_viewPort(0, viewport, minX_Window, RangeX), Y_viewPort(x, viewport, minY_Window, RangeY)), 
                                            new Point(X_viewPort(j, viewport, minX_Window, RangeX), Y_viewPort(freq, viewport, minY_Window, RangeY)));
                    }

                    if(j==splitter)
                    {
                        listj.Add(freq);
                    }

                    if (j == n)
                    {
                        listn.Add(freq);
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
                    for(int x=0; x<10; x++)
                    {
                        histoj.Add(x, 0);
                    }

                    List<double> keysj = new List<double>(histoj.Keys);
                    foreach (double x in listj)
                    {
                        foreach(double y in keysj)
                        {
                            double par = y/ 10.0;
                            if(x>par && x < (par + 0.1))
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
                        int x_width = X_viewPort(y.Value, viewport, minX_Window, RangeX);


                        Rectangle rect = new Rectangle(x_splitter, y_height, x_width, y_freq - y_height);
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
                            if (x > par && x < (par + 0.1))
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
                        int x_width = X_viewPort(y.Value, viewport, minX_Window, RangeX);


                        Rectangle rect = new Rectangle(x_Device_less, y_height, x_width, y_freq - y_height);
                        g.FillRectangle(Brushes.Blue, rect);
                        g.DrawRectangle(Pens.Red, rect);
                    }

                }
            }

            pictureBox1.Image = b;
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
    }
}