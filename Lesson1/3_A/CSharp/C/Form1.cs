using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
            richTextBox1.AllowDrop = true;
            this.richTextBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.richTextBox1_DragDrop);
            this.richTextBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.richTextBox1_DragEnter);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "Pong" + Environment.NewLine;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.richTextBox1.ResetText();
        }

        private void richTextBox1_MouseEnter(object sender, EventArgs e)
        {
            this.richTextBox1.BackColor = Color.Red;
        }

        private void richTextBox1_MouseLeave(object sender, EventArgs e)
        {
            this.richTextBox1.BackColor = Color.White;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void richTextBox1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            for (i = 0; i < s.Length; i++)
                richTextBox1.AppendText(s[i] + Environment.NewLine);
        }
    }
}
