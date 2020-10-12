using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homeworks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.ResetText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "Pong";
        }

        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {
           this.richTextBox1.BackColor = Color.Red;
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        private void richTextBox1_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            this.richTextBox1.BackColor = Color.White;
        }
    }
}
