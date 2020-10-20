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
        //params:
        static int max = 120;
        static int min = 0;



        //---------------------------
        Random R = new Random();
        double avg = 0;
        int count = 0;
        int[] freq = new int[max];

        public Form1()
        {
            InitializeComponent();
            start();
        }

        private void start()
        {
            for(int i=0; i<max; i++)
            {
                freq[i] = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;
            int random = R.Next(min, max);
            avg = avg + (random - avg) / count;
            freq[random]++;
            richTextBox1.AppendText( "age: " + random + "  new average: " + avg + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            richTextBox1.AppendText("---------------------------------------------" + Environment.NewLine);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            richTextBox1.AppendText("---------------------------------------------" + Environment.NewLine);
            for (int i = 0; i < max; i++)
            {
                richTextBox1.AppendText("Age: " + i + "  count: " + freq[i] + Environment.NewLine);
            }
        }
    }
}
