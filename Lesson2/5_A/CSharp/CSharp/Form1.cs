using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp
{
    public partial class Form1 : Form
    {

        //params
        int min = 40;
        int max = 120;
        int intervalSize = 5;

        //---------------------------------------------------------

        double avg = 0;
        Random R = new Random();
        int count = 0;
        List<Intervallo> list = new List<Intervallo>();

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;
            int random = R.Next(min, max);
            avg = avg + (random - avg) / count;
            addFreq(count, random);
            richTextBox1.AppendText("Weight: " + random + "  avg: " + avg + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("--------------------------------------------------------" + Environment.NewLine);
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            richTextBox1.AppendText("--------------------------------------------------------" + Environment.NewLine);
            for( int y=0; y<list.Count; y++)
            {
                richTextBox1.AppendText("Interval:  " + list[y].lowerBound + "-" + list[y].upperBound + 
                                            "  Count: " + list[y].recurrency + Environment.NewLine);
            }
            
        }

        private void addFreq(int count, int random)
        {
            if (count == 1)
            {
                Intervallo i = new Intervallo();
                i.lowerBound = random;
                i.upperBound = i.lowerBound + intervalSize;
                i.recurrency = 1;
                list.Add(i);
                return;
            }
            else
            {
                bool allocated = false;
                if (random < list[0].lowerBound)
                {
                    Intervallo x = new Intervallo();
                    x.upperBound = list[0].lowerBound;
                    x.lowerBound = x.upperBound - intervalSize;
                    x.recurrency = 0;
                    list.Insert(0, x);
                    addFreq(count, random);
                    return;
                }
                else if (random >= list[list.Count - 1].upperBound)
                {
                    Intervallo x = new Intervallo();
                    x.lowerBound = list[list.Count - 1].upperBound;
                    x.upperBound = x.lowerBound + intervalSize;
                    x.recurrency = 0;
                    list.Add(x);
                    addFreq(count, random);
                    return;
                }
                else
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if ((random >= list[j].lowerBound) && (random < list[j].upperBound))
                        {
                            list[j].recurrency++;
                            allocated = true;
                            return;
                        }
                    }
                }
                if (allocated == false)
                {
                    throw new Exception("errore intervalli");
                }
            }
        }

    }
}
