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
                    comboBox2.Items.Clear();
                    comboBox3.Items.Clear();

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
                    if (comboBox2.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox2.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                    if (comboBox3.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox3.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                }
                else if (radioButton2.Checked)
                {
                    dtCloned.Columns[selectedColumn].DataType = typeof(Int32);
                    if (!(comboBox2.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox2.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);
                    if (!(comboBox3.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox3.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);

                }
                else if (radioButton3.Checked)
                {
                    dtCloned.Columns[selectedColumn].DataType = typeof(Double);
                    if (!(comboBox2.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox2.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);
                    if (!(comboBox3.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox3.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);
                }
                else
                {
                    dtCloned.Columns[selectedColumn].DataType = typeof(Boolean);
                    if (comboBox2.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox2.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                    if (comboBox3.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox3.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
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

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        Bitmap b;
        Graphics g;
        Font smallFont = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Pixel);
        Random r = new Random();
        double IntervalSize_X = 10;
        double IntervalSize_Y = 10;
        double maxX_Window = 250;
        double maxY_Window = 250;
        double minX_Window = 0;
        double minY_Window = 0;
        double percentage = 50;
        List<interval> listOfInterval_X;
        List<interval> listOfInterval_Y;

        private void initializeGraphics()
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void createContingencyTable(List<interval> listX, List<interval> listY, Dictionary<Tuple<interval, interval>, int> dict)
        {
            DataTable ct = new DataTable();
            ct.Columns.Add("CT");
            foreach(interval intervX in listX)
            {
                ct.Columns.Add(intervX.lowerBound + " - " + intervX.upperBound);
            }

            
            foreach(interval intervY in listY)
            {
                //ct.Rows.Add(interv.lowerBound + " - " + interv.upperBound);
                List<String> newlist = new List<string>();
                for(int x = 0; x<ct.Columns.Count; x++)
                {
                    newlist.Add("0");
                }
                newlist[0] = (intervY.lowerBound + " - " + intervY.upperBound);

                foreach (KeyValuePair<Tuple<interval, interval>, int> pair in dict)
                {
                    if (pair.Key.Item2.lowerBound == intervY.lowerBound)
                    {
                        int x = (ct.Columns.IndexOf(pair.Key.Item1.lowerBound + " - " + pair.Key.Item1.upperBound));
                        newlist[x] = (pair.Value.ToString());
                    }
                }
                ct.Rows.Add(newlist.ToArray());
            }

            dataGridView2.DataSource = ct;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (((dt.Columns[comboBox2.GetItemText(comboBox2.SelectedItem)].DataType == typeof(int)) || (dt.Columns[comboBox2.GetItemText(comboBox2.SelectedItem)].DataType == typeof(double)))
                && ((dt.Columns[comboBox3.GetItemText(comboBox3.SelectedItem)].DataType == typeof(int)) || (dt.Columns[comboBox3.GetItemText(comboBox3.SelectedItem)].DataType == typeof(double))))
            {
                initializeGraphics();
                List<DataPoint> data = new List<DataPoint>();


                foreach (DataRow row in dt.Rows)
                {
                    DataPoint dp = new DataPoint();
                    double X = double.Parse((row.ItemArray[row.Table.Columns.IndexOf(comboBox2.GetItemText(comboBox2.SelectedItem))].ToString()));
                    dp.X = X;
                    double Y = double.Parse((row.ItemArray[row.Table.Columns.IndexOf(comboBox3.GetItemText(comboBox3.SelectedItem))].ToString()));
                    dp.Y = Y;
                    data.Add(dp);
                }

                Dictionary<Tuple<interval, interval>, int> freqs = BivariateDistribution_DiscreteVariable(IntervalSize_X, IntervalSize_Y, minX_Window, minY_Window, data);
                createContingencyTable(listOfInterval_X, listOfInterval_Y,freqs);
                double RangeX = maxX_Window - minX_Window;
                double RangeY = maxY_Window - minY_Window;

                Rectangle viewport = new Rectangle(400, 100, 320, 270);
                Rectangle viewport2 = new Rectangle(20, 250, 320, 220);
                Rectangle viewport3 = new Rectangle(20, 10, 320, 220);
                g.DrawRectangle(Pens.Black, viewport3);
                g.DrawRectangle(Pens.Black, viewport2);
                g.DrawRectangle(Pens.Black, viewport);

                for (double x = minX_Window; x < maxX_Window; x += IntervalSize_X)
                {
                    g.DrawString(x.ToString(), smallFont, Brushes.Red, new Point(X_viewPort(x, viewport2, minX_Window, RangeX) - 5, Y_viewPort(minY_Window, viewport2, minY_Window, RangeY)));
                }

                for (double y = minY_Window; y < maxY_Window; y += IntervalSize_Y)
                {
                    g.DrawString(y.ToString(), smallFont, Brushes.Red, new Point(X_viewPort(minX_Window, viewport3, minX_Window, RangeX) - 20, Y_viewPort(y, viewport3, minY_Window, RangeY) - 5));
                }

                foreach (interval interv in listOfInterval_X)
                {
                    double lb = interv.lowerBound;
                    double ub = interv.upperBound;
                    if (lb >= minX_Window && ub <= maxX_Window)
                    {

                        int x_start_width = X_viewPort(lb, viewport2, minX_Window, RangeX);
                        int x_stop_width = X_viewPort(ub, viewport2, minX_Window, RangeX);

                        int y_zero = Y_viewPort(minY_Window, viewport2, minY_Window, RangeY);

                        double mF = interv.marginalFrequency * percentage;
                        if (mF > maxY_Window) mF = maxY_Window;
                        int y_Resized = Y_viewPort(mF, viewport2, minY_Window, RangeY);

                        Rectangle recX = new Rectangle(x_start_width, y_Resized, x_stop_width - x_start_width, y_zero - y_Resized);
                        if (interv.marginalFrequency > 0)
                        {
                            g.DrawString(interv.marginalFrequency.ToString(), smallFont, Brushes.Red, new Point(x_start_width + 2, y_Resized - 15));

                        }
                        g.FillRectangle(Brushes.Blue, recX);
                        g.DrawRectangle(Pens.Red, recX);
                    }
                }

                foreach (interval interv in listOfInterval_Y)
                {
                    double lb = interv.lowerBound;
                    double ub = interv.upperBound;
                    if (lb >= minY_Window && ub <= maxY_Window)
                    {
                        int y_start_height = Y_viewPort(lb, viewport3, minY_Window, RangeY);
                        int y_stop_height = Y_viewPort(ub, viewport3, minY_Window, RangeY);

                        int x_zero = X_viewPort(minX_Window, viewport3, minX_Window, RangeX);

                        double width = interv.marginalFrequency * percentage;
                        if (width > maxX_Window) width = maxX_Window;
                        int x_Resized = X_viewPort(width, viewport3, minX_Window, RangeX);


                        Rectangle recY = new Rectangle(x_zero, y_stop_height, x_Resized - x_zero, y_start_height - y_stop_height);
                        if (interv.marginalFrequency > 0)
                        {
                            g.DrawString(interv.marginalFrequency.ToString(), smallFont, Brushes.Red, new Point(x_Resized + 5, y_stop_height));

                        }
                        g.FillRectangle(Brushes.Blue, recY);
                        g.DrawRectangle(Pens.Red, recY);
                    }
                }

                foreach (DataPoint dp in data)
                {
                    int x_Device = X_viewPort(dp.X, viewport, minX_Window, RangeX);
                    int y_Device = Y_viewPort(dp.Y, viewport, minY_Window, RangeY);
                    int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
                    int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);
                    if ((dp.X < maxX_Window && dp.X > minX_Window) && (dp.Y < maxY_Window && dp.Y > minY_Window))
                    {
                        g.DrawLine(new Pen(Brushes.Red), new Point(x_zero - 5, y_Device), new Point(x_zero + 5, y_Device));
                        g.DrawLine(new Pen(Brushes.Red), new Point(x_Device, y_zero - 5), new Point(x_Device, y_zero + 5));
                        g.FillEllipse(Brushes.Blue, new Rectangle(new Point(x_Device - 3, y_Device - 3), new Size(6, 6)));
                        g.DrawString(dp.X.ToString() + "," + dp.Y.ToString(), smallFont, Brushes.Red, new Point(x_Device, y_Device));
                    }

                }


                pictureBox1.Image = b;
            }
            else
            {
                MessageBox.Show("Select an Integer or a Double variable!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private interval findIntervalForValue(double v, double intervalSize, List<interval> listOfIntervals)
        {
            foreach (interval i in listOfIntervals)
            {
                if (v >= i.lowerBound && v < i.upperBound)
                {
                    i.marginalFrequency++;
                    return i;
                }
            }

            if (v < listOfIntervals[0].lowerBound)
            {

                while (true)
                {
                    interval newLeft = new interval();
                    newLeft.upperBound = listOfIntervals[0].lowerBound;
                    newLeft.lowerBound = newLeft.upperBound - intervalSize;
                    newLeft.marginalFrequency = 0;
                    listOfIntervals.Insert(0, newLeft);
                    if (v >= newLeft.lowerBound && v < newLeft.upperBound)
                    {
                        newLeft.marginalFrequency++;
                        return newLeft;
                    }
                }
            }
            else if (v >= listOfIntervals[listOfIntervals.Count() - 1].upperBound)
            {

                while (true)
                {
                    interval newRight = new interval();
                    newRight.lowerBound = listOfIntervals[listOfIntervals.Count() - 1].upperBound;
                    newRight.upperBound = newRight.lowerBound + intervalSize;
                    newRight.marginalFrequency = 0;
                    listOfIntervals.Add(newRight);
                    if (v >= newRight.lowerBound && v < newRight.upperBound)
                    {
                        newRight.marginalFrequency++;
                        return newRight;
                    }
                }
            }
            else
            {
                throw new Exception("not expected occurrence");

            }
        }

        private Dictionary<Tuple<interval, interval>, int> BivariateDistribution_DiscreteVariable(double IntervalSize_X, double IntervalSize_Y, double StartingEndPoint_X,
                                                                                            double StartingEndPoint_Y, List<DataPoint> listOfObservations)
        {
            Dictionary<Tuple<interval, interval>, int> frequencies = new Dictionary<Tuple<interval, interval>, int>();

            interval interval_X_0 = new interval();
            interval_X_0.lowerBound = StartingEndPoint_X;
            interval_X_0.upperBound = StartingEndPoint_X + IntervalSize_X;
            interval_X_0.marginalFrequency = 0;
            listOfInterval_X = new List<interval>();
            listOfInterval_X.Add(interval_X_0);

            interval interval_Y_0 = new interval();
            interval_Y_0.lowerBound = StartingEndPoint_Y;
            interval_Y_0.upperBound = StartingEndPoint_Y + IntervalSize_Y;
            interval_Y_0.marginalFrequency = 0;
            listOfInterval_Y = new List<interval>();
            listOfInterval_Y.Add(interval_Y_0);

            interval intervalFound_X;
            interval intervalFound_Y;

            foreach (DataPoint dp in listOfObservations)
            {
                intervalFound_X = null;
                intervalFound_Y = null;

                intervalFound_X = findIntervalForValue(dp.X, IntervalSize_X, listOfInterval_X);

                intervalFound_Y = findIntervalForValue(dp.Y, IntervalSize_Y, listOfInterval_Y);

                Tuple<interval, interval> foundTuple = new Tuple<interval, interval>(intervalFound_X, intervalFound_Y);
                if (frequencies.ContainsKey(foundTuple))
                {
                    frequencies[foundTuple] += 1;
                }
                else
                {
                    frequencies.Add(foundTuple, 1);
                }
            }
            return frequencies;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            IntervalSize_X = (double)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            IntervalSize_Y = (double)numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            minY_Window = (double)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            minX_Window = (double)numericUpDown4.Value;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            maxY_Window = (double)numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            maxX_Window = (double)numericUpDown6.Value;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            percentage = (double)numericUpDown7.Value;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
