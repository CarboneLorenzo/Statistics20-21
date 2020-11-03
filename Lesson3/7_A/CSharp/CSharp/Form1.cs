using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace CSharp
{
    public partial class Form1 : Form
    {
        char delimiter = ',';
        DataTable dt;
        public Form1()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( comboBox1.SelectedIndex == 0)
            {
                delimiter = ',';
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                delimiter = ';';
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                delimiter = '-';
            }
            else
            {
                delimiter = '\t';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();
                file.ShowDialog();
                if (file.FileName.Length > 0)
                {
                    richTextBox1.Text = file.FileName;
                    button2.Enabled = true;
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                dt = new DataTable();

                using (StreamReader reader = new StreamReader(richTextBox1.Text))
                {
                    string line = reader.ReadLine();
                    string[] parsed = line.Split(delimiter);
                    int columns = parsed.Length;

                    if (checkBox1.Checked)
                    {
                        foreach (string x in parsed)
                        {
                            dt.Columns.Add(x);
                            line = reader.ReadLine();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < columns; i++)
                        {
                            dt.Columns.Add("Variabile" + i);
                        }
                    }


                    do
                    {
                        string[] parsedAgain = line.Split(delimiter);
                        if (parsedAgain.Length > columns)
                        {
                            for (int i = 0; i < (parsedAgain.Length - columns); i++)
                            {
                                dt.Columns.Add("Variabile" + (i + columns));

                            }
                            columns = parsedAgain.Length;
                        }
                        dt.Rows.Add(parsedAgain);
                    } while ((line = reader.ReadLine()) != null);

                    dataGridView1.DataSource = dt;
                    richTextBox2.Clear();
                    button2.Enabled = false;
                }
                

            }
            catch(Exception err2)
            {
                MessageBox.Show(err2.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        int selectedColumn;

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCloned = dt.Clone();

                if (radioButton1.Checked)
                {
                    dtCloned.Columns[selectedColumn].DataType = typeof(String);
                }
                else if (radioButton2.Checked)
                {
                    dtCloned.Columns[selectedColumn].DataType = typeof(Int32);
                }
                else if (radioButton3.Checked)
                {
                    dtCloned.Columns[selectedColumn].DataType = typeof(Double);
                }
                else
                {
                    dtCloned.Columns[selectedColumn].DataType = typeof(Boolean);
                }

                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }
                dataGridView1.DataSource = dtCloned;
                dt = dtCloned;
                MessageBox.Show("Done", "Done", MessageBoxButtons.OK);
            }
            catch (Exception err3)
            {
                MessageBox.Show(err3.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button3.Enabled = true;
            selectedColumn = e.ColumnIndex;
            richTextBox2.Text = "Variabile: " + dataGridView1.Columns[selectedColumn].Name + "\n" + "Tipo:" + dataGridView1.Columns[selectedColumn].ValueType;
        }
    }
}
