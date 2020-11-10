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

        double y(double x)
        {
            return x * x;
        }

        double riemann_integral(int max, int min, double interval_size)
        {
            double res = 0;
            int numInter = (int) ((max - min) / interval_size);
            for (int i = 0; i < numInter; i++)
            {
                res += y(min + (interval_size * i)) * interval_size;
            }
            return res;
        }

        double lebesgue_integral(int max, int min, double interval_size)
        {
            double res = 0;
            double right = min + interval_size;
            for (double i = min + interval_size; i < max; i+=interval_size)
            {
                double rightY = y(right);
                res += interval_size * rightY;
                right += interval_size;
            }
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("y = x*x , test done with: min=100 and max=500 , expected result: 4.133333333333333 x 10^7" + Environment.NewLine);
            double rres = riemann_integral(500,100,1);
            richTextBox1.AppendText("Riemann integral result: " + rres.ToString() + Environment.NewLine);
            double lres = lebesgue_integral(500, 100, 1);
            richTextBox1.AppendText("Lebesgue integral result: " + lres.ToString() + Environment.NewLine);

        }
    }
}
