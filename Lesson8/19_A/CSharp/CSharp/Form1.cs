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
        int selectedColumn = -1;
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
                    var line = reader.ReadLine();
                    string[] parsed = line.Split(delimiter);
                    int columns = parsed.Length;

                    if (checkBox1.Checked)
                    {
                        foreach (string x in parsed)
                        {
                            dt.Columns.Add(x);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < columns; i++)
                        {
                            dt.Columns.Add("Variabile" + i);
                        }
                        dt.Rows.Add(parsed);
                    }

                    bool error = false;
                    while(!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        parsed = line.Split(delimiter);
                        if (parsed.Length > columns)
                        {
                            for (int i = 0; i < (parsed.Length - columns); i++)
                            {
                                dt.Columns.Add("Variabile" + (i + columns));

                            }
                            columns = parsed.Length;
                            error = true;
                        }
                        dt.Rows.Add(parsed);
                    }

                    
                    DataTable dtCloned = dt.Clone();

                    foreach (DataColumn col in dt.Columns)
                    {
                        Type lastDataType = null;
                        double i = 0;
                        bool flag = true;
                        foreach(DataRow row in dt.Rows)
                        {
                            Type dataType = ParseString(row.ItemArray[dt.Columns.IndexOf(col)].ToString());
                            if(i>0)
                            {
                                if(lastDataType != dataType)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            lastDataType = dataType;
                            i++;
                        }

                        if(flag)
                        {
                            dtCloned.Columns[dt.Columns.IndexOf(col)].DataType = lastDataType;
                        }
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        dtCloned.ImportRow(row);
                    }
                    dataGridView1.DataSource = dtCloned;
                    dt = dtCloned;
                    richTextBox2.Clear();
                    button2.Enabled = false;
                    comboBox2.Items.Clear();
                    comboBox3.Items.Clear();
                    comboBox4.Items.Clear();
                    selectedColumn = -1;
                    button3.Enabled = false;
                    button7.Enabled = false;

                    foreach (DataColumn col in dt.Columns)
                    {
                        if(col.DataType == typeof(double) || col.DataType == typeof(Int32))
                        {
                            if (!(comboBox2.Items.Contains(dtCloned.Columns[dt.Columns.IndexOf(col)].ColumnName))) comboBox2.Items.Add(dtCloned.Columns[dt.Columns.IndexOf(col)].ColumnName);
                            if (!(comboBox3.Items.Contains(dtCloned.Columns[dt.Columns.IndexOf(col)].ColumnName))) comboBox3.Items.Add(dtCloned.Columns[dt.Columns.IndexOf(col)].ColumnName);
                            if (!(comboBox4.Items.Contains(dtCloned.Columns[dt.Columns.IndexOf(col)].ColumnName))) comboBox4.Items.Add(dtCloned.Columns[dt.Columns.IndexOf(col)].ColumnName);
                        }
                    }

                    if(error)
                    {
                        MessageBox.Show("some data fields are missing, some features may do not work properly.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                

            }
            catch(Exception err2)
            {
                MessageBox.Show(err2.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedColumn != -1)
                {

                    DataTable dtCloned = dt.Clone();

                    if (radioButton1.Checked)
                    {
                        dtCloned.Columns[selectedColumn].DataType = typeof(String);
                        if (comboBox2.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox2.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                        if (comboBox3.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox3.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                        if (comboBox4.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox4.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                    }
                    else if (radioButton2.Checked)
                    {
                        dtCloned.Columns[selectedColumn].DataType = typeof(Int32);
                        if (!(comboBox2.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox2.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);
                        if (!(comboBox3.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox3.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);
                        if (!(comboBox4.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox4.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);

                    }
                    else if (radioButton3.Checked)
                    {
                        dtCloned.Columns[selectedColumn].DataType = typeof(double);
                        if (!(comboBox2.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox2.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);
                        if (!(comboBox3.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox3.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);
                        if (!(comboBox4.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName))) comboBox4.Items.Add(dtCloned.Columns[selectedColumn].ColumnName);
                    }
                    else
                    {
                        dtCloned.Columns[selectedColumn].DataType = typeof(Boolean);
                        if (comboBox2.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox2.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                        if (comboBox3.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox3.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                        if (comboBox4.Items.Contains(dtCloned.Columns[selectedColumn].ColumnName)) comboBox4.Items.Remove(dtCloned.Columns[selectedColumn].ColumnName);
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        dtCloned.ImportRow(row);
                    }
                    dataGridView1.DataSource = dtCloned;
                    dt = dtCloned;
                    richTextBox2.Text = "Variabile: " + dataGridView1.Columns[selectedColumn].Name + "\n" + "Tipo:" + dataGridView1.Columns[selectedColumn].ValueType;
                    MessageBox.Show("Done", "Done", MessageBoxButtons.OK);
                    
                }
                else
                {
                    MessageBox.Show("Select a valid single index", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        Bitmap b, b2;
        Graphics g, g2;
        Font smallFont = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Pixel);
        Font mediumFont = new Font("Calibri", 20, FontStyle.Regular, GraphicsUnit.Pixel);
        Font bigFont = new Font("Calibri", 30, FontStyle.Regular, GraphicsUnit.Pixel);
        double IntervalSize_X = 10;
        double IntervalSize_Y = 10;
        List<interval> listOfInterval_X;
        List<interval> listOfInterval_Y;
        int maxValues = 100;
        List<DataPoint> data;
        int maxX = 0;
        int maxY = 0;
        int minX = 0;
        int minY = 0;
        List<double> ListX;
        List<double> ListY;
        List<double> ListXnotord;
        List<double> ListYnotord;
        Dictionary<Tuple<interval, interval>, int> freqs;
        private void initializeGraphics()
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        }

        private void initializeGraphics2()
        {        
            b2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g2 = Graphics.FromImage(b2);
            g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        }
        private void initializeDP()
        {
            data = new List<DataPoint>();

            if (dt.Rows.Count < maxValues)
            {
                maxValues = dt.Rows.Count;
                numericUpDown3.Value = maxValues;
            }
            ListX = new List<double>();
            ListY = new List<double>();
            ListXnotord = new List<double>();
            ListYnotord = new List<double>();
            for (int i = 0; i < maxValues; i++)
            {
                DataRow row = dt.Rows[i];
                DataPoint dp = new DataPoint();
                double X = double.Parse((row.ItemArray[row.Table.Columns.IndexOf(comboBox2.GetItemText(comboBox2.SelectedItem))].ToString()));
                dp.X = X;
                ListX.Add(X);
                ListXnotord.Add(X);
                double Y = double.Parse((row.ItemArray[row.Table.Columns.IndexOf(comboBox3.GetItemText(comboBox3.SelectedItem))].ToString()));
                dp.Y = Y;
                ListY.Add(Y);
                ListYnotord.Add(Y);
                data.Add(dp);
            }
            ListX.Sort();
            ListY.Sort();
            maxX = (int)ListX[ListX.Count-1];
            maxY = (int)ListY[ListY.Count-1];
            minX = (int)ListX[0];
            minY = (int)ListY[0];

            freqs = BivariateDistribution_DiscreteVariable(IntervalSize_X, IntervalSize_Y, minX, minY, data);
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null && comboBox3.SelectedItem != null && comboBox2.GetItemText(comboBox2.SelectedItem) != comboBox3.GetItemText(comboBox3.SelectedItem))
            {

                if (((dt.Columns[comboBox2.GetItemText(comboBox2.SelectedItem)].DataType == typeof(int)) || (dt.Columns[comboBox2.GetItemText(comboBox2.SelectedItem)].DataType == typeof(double)))
                    && ((dt.Columns[comboBox3.GetItemText(comboBox3.SelectedItem)].DataType == typeof(int)) || (dt.Columns[comboBox3.GetItemText(comboBox3.SelectedItem)].DataType == typeof(double))))
                {
                    initializeGraphics();

                    initializeDP();


                    double maxX_Window = maxX;
                    double maxY_Window = maxY;
                    double minX_Window = minX;
                    double minY_Window = minY;
                    double maxX_Window2 = maxX;
                    double maxY_Window2 = 250;
                    double minX_Window2 = minX;
                    double minY_Window2 = 0;
                    double maxX_Window3 = 250;
                    double maxY_Window3 = maxY;
                    double minX_Window3 = 0;
                    double minY_Window3 = minY;
                    double RangeX = maxX_Window - minX_Window;
                    double RangeY = maxY_Window - minY_Window;
                    double RangeX2 = maxX_Window2 - minX_Window2;
                    double RangeY2 = maxY_Window2 - minY_Window2;
                    double RangeX3 = maxX_Window3 - minX_Window3;
                    double RangeY3 = maxY_Window3 - minY_Window3;

                    Rectangle viewport = new Rectangle((pictureBox1.Width / 4) + 20, (pictureBox1.Height / 2) + 20, (pictureBox1.Width / 2) - 40, (pictureBox1.Height / 2) - 40);
                    Rectangle viewport2 = new Rectangle((pictureBox1.Width / 2) + 20, 30, (pictureBox1.Width / 2) - 40, (pictureBox1.Height / 2) - 40);
                    Rectangle viewport3 = new Rectangle(20, 30, (pictureBox1.Width / 2) - 40, (pictureBox1.Height / 2) - 40);
                    g.DrawRectangle(Pens.Black, viewport3);
                    g.DrawRectangle(Pens.Black, viewport2);
                    g.DrawRectangle(Pens.Black, viewport);

                    

                    int X_count = 0;
                    double X_quantile = 0;
                    g.DrawString(comboBox2.SelectedItem.ToString(), mediumFont, Brushes.Red, new Point(X_viewPort(minX_Window2, viewport2, minX_Window2, RangeX2), Y_viewPort(maxY_Window2, viewport2, minY_Window2, RangeY2) - 25));
                    foreach (interval interv in listOfInterval_X)
                    {
                        double lb = interv.lowerBound;
                        double ub = interv.upperBound;


                        int x_start_width = X_viewPort((minX_Window2+(X_count * (RangeX2/ listOfInterval_X.Count))), viewport2, minX_Window2, RangeX2);
                        int x_stop_width = X_viewPort((minX_Window2+((X_count+1) * (RangeX2 / listOfInterval_X.Count))), viewport2, minX_Window2, RangeX2);
                        int y_zero = Y_viewPort(minY_Window2, viewport2, minY_Window2, RangeY2);
                        int y_Device = Y_viewPort(maxY_Window2, viewport2, minY_Window2, RangeY2);

                        double mF = maxY_Window2 * (interv.marginalFrequency / maxValues);
                        if (mF > maxY_Window2) mF = maxY_Window2;
                        int y_Resized = Y_viewPort(mF, viewport2, minY_Window2, RangeY2);

                        Rectangle recX = new Rectangle(x_start_width, y_Resized, x_stop_width - x_start_width, y_zero - y_Resized);
                        if (interv.marginalFrequency > 0)
                        {
                            g.DrawString(interv.marginalFrequency.ToString(), smallFont, Brushes.Red, new Point(x_start_width + 2, y_Resized - 15));

                        }
                        if (X_count==0)
                        {
                            g.DrawString(lb.ToString(), smallFont, Brushes.Red, new Point(x_start_width - 5, Y_viewPort(minY_Window2, viewport2, minY_Window2, RangeY2)));
                        }
                        g.DrawString(ub.ToString(), smallFont, Brushes.Red, new Point(x_stop_width- 5, Y_viewPort(minY_Window2, viewport2, minY_Window2, RangeY2)));
                        g.FillRectangle(Brushes.Blue, recX);
                        g.DrawRectangle(Pens.Red, recX);
                        double X_quantile_plus = X_quantile + interv.marginalFrequency;
                        double quantile_limit = (double)maxValues / 4.0;
                        if ((X_quantile < quantile_limit) && (X_quantile_plus >= quantile_limit))
                        {
                            g.DrawLine(new Pen(Brushes.Black), new Point(x_stop_width, y_zero), new Point(x_stop_width, y_Device));
                            g.DrawString("Q1", smallFont, Brushes.Black, new Point(x_stop_width, y_Device));
                        }
                        else if ((X_quantile < quantile_limit * 2) && (X_quantile_plus >= quantile_limit * 2))
                        {
                            g.DrawLine(new Pen(Brushes.Black), new Point(x_stop_width, y_zero), new Point(x_stop_width, y_Device));
                            g.DrawString("Q2", smallFont, Brushes.Black, new Point(x_stop_width, y_Device));
                        }
                        else if ((X_quantile < quantile_limit * 3) && (X_quantile_plus >= quantile_limit * 3))
                        {
                            g.DrawLine(new Pen(Brushes.Black), new Point(x_stop_width, y_zero), new Point(x_stop_width, y_Device));
                            g.DrawString("Q3", smallFont, Brushes.Black, new Point(x_stop_width, y_Device));
                        }
                        X_quantile = X_quantile_plus;
                        X_count++;                      
                    }

                    int Y_count = 0;
                    double Y_quantile = 0;
                    g.DrawString(comboBox3.SelectedItem.ToString(), mediumFont, Brushes.Red, new Point(X_viewPort(minX_Window3, viewport3, minX_Window3, RangeX3), Y_viewPort(maxY_Window3, viewport3, minY_Window3, RangeY3) - 25));
                    foreach (interval interv in listOfInterval_Y)
                    {
                        double lb = interv.lowerBound;
                        double ub = interv.upperBound;
                        int y_start_height = Y_viewPort((minY_Window3 + (Y_count * (RangeY3 / listOfInterval_Y.Count))), viewport3, minY_Window3, RangeY3);
                        int y_stop_height = Y_viewPort((minY_Window3 + ((Y_count + 1) * (RangeY3 / listOfInterval_Y.Count))), viewport3, minY_Window3, RangeY3);
                        int x_zero = X_viewPort(minX_Window3, viewport3, minX_Window3, RangeX3);
                        int x_Device = X_viewPort(maxX_Window3, viewport3, minX_Window3, RangeX3);

                        double width = maxX_Window3 * (interv.marginalFrequency / maxValues);
                        if (width > maxX_Window3) width = maxX_Window3;
                        int x_Resized = X_viewPort(width, viewport3, minX_Window3, RangeX3);


                        Rectangle recY = new Rectangle(x_zero, y_stop_height, x_Resized - x_zero, y_start_height - y_stop_height);
                        if (interv.marginalFrequency > 0)
                        {
                            g.DrawString(interv.marginalFrequency.ToString(), smallFont, Brushes.Red, new Point(x_Resized + 5, y_stop_height));

                        }
                        if(Y_count == 0)
                        {
                            g.DrawString(lb.ToString(), smallFont, Brushes.Red, new Point(X_viewPort(minX_Window3, viewport3, minX_Window3, RangeX3) - 20, y_start_height - 5));
                        }
                        g.DrawString(ub.ToString(), smallFont, Brushes.Red, new Point(X_viewPort(minX_Window3, viewport3, minX_Window3, RangeX3) - 20, y_stop_height - 5));
                        g.FillRectangle(Brushes.Blue, recY);
                        g.DrawRectangle(Pens.Red, recY);
                        double Y_quantile_plus = Y_quantile + interv.marginalFrequency;
                        double quantile_limit = (double)maxValues / 4.0;
                        if ((Y_quantile < quantile_limit) && (Y_quantile_plus >= quantile_limit))
                        {
                            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero, y_stop_height), new Point(x_Device, y_stop_height));
                            g.DrawString("Q1", smallFont, Brushes.Black, new Point(x_Device, y_stop_height));
                        }
                        else if ((Y_quantile < quantile_limit*2) && (Y_quantile_plus >= quantile_limit*2))
                        {
                            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero, y_stop_height), new Point(x_Device, y_stop_height));
                            g.DrawString("Q2", smallFont, Brushes.Black, new Point(x_Device, y_stop_height));
                        }
                        else if ((Y_quantile < quantile_limit * 3) && (Y_quantile_plus >= quantile_limit * 3))
                        {
                            g.DrawLine(new Pen(Brushes.Black), new Point(x_zero, y_stop_height), new Point(x_Device, y_stop_height));
                            g.DrawString("Q3", smallFont, Brushes.Black, new Point(x_Device, y_stop_height));
                        }
                        Y_quantile = Y_quantile_plus;
                        Y_count++;
                    }

                    int x_zero0 = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
                    int y_zero0 = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);
                    int x_dev0 = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
                    int y_dev0 = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
                    foreach (DataPoint dp in data)
                    {
                        int x_Device = X_viewPort(dp.X, viewport, minX_Window, RangeX);
                        int y_Device = Y_viewPort(dp.Y, viewport, minY_Window, RangeY);
                        
                        if ((dp.X < maxX_Window && dp.X > minX_Window) && (dp.Y < maxY_Window && dp.Y > minY_Window))
                        {
                            g.DrawLine(new Pen(Brushes.Red), new Point(x_zero0 - 5, y_Device), new Point(x_zero0 + 5, y_Device));
                            g.DrawLine(new Pen(Brushes.Red), new Point(x_Device, y_zero0 - 5), new Point(x_Device, y_zero0 + 5));
                            g.FillEllipse(Brushes.Blue, new Rectangle(new Point(x_Device - 3, y_Device - 3), new Size(6, 6)));
                            g.DrawString(dp.X.ToString() + "," + dp.Y.ToString(), smallFont, Brushes.Red, new Point(x_Device, y_Device));
                        }
                        

                    }


                    double q1, q2, q3;
                    q1 = (double)ListX.Count / 4.0;
                    if ((q1-(int)q1)==0)
                    {
                        q1 = (int)q1 - 1;
                    }
                    q2 = 2.0*((double)ListX.Count / 4.0);
                    if ((q2 - (int)q2) == 0)
                    {
                        q2 = (int)q2 - 1;
                    }
                    q3 = 3.0*((double)ListX.Count / 4.0);
                    if ((q3 - (int)q3) == 0)
                    {
                        q3 = (int)q3 - 1;
                    }


                    g.DrawLine(new Pen(Brushes.Black), new Point(X_viewPort(ListX[(int)q1], viewport, minX_Window, RangeX), y_zero0), new Point(X_viewPort(ListX[(int)q1], viewport, minX_Window, RangeX), y_dev0));
                    g.DrawString("Q1="+ListX[(int)q1] , smallFont, Brushes.Black, new Point(X_viewPort(ListX[(int)q1], viewport, minX_Window, RangeX), y_dev0));

                    g.DrawLine(new Pen(Brushes.Black), new Point(X_viewPort(ListX[(int)q2], viewport, minX_Window, RangeX), y_zero0), new Point(X_viewPort(ListX[(int)q2], viewport, minX_Window, RangeX), y_dev0));
                    g.DrawString("Q2=" + ListX[(int)q2], smallFont, Brushes.Black, new Point(X_viewPort(ListX[(int)q2], viewport, minX_Window, RangeX), y_dev0));

                    g.DrawLine(new Pen(Brushes.Black), new Point(X_viewPort(ListX[(int)q3], viewport, minX_Window, RangeX), y_zero0), new Point(X_viewPort(ListX[(int)q3], viewport, minX_Window, RangeX), y_dev0));
                    g.DrawString("Q3=" + ListX[(int)q3], smallFont, Brushes.Black, new Point(X_viewPort(ListX[(int)q3], viewport, minX_Window, RangeX), y_dev0));

                    g.DrawLine(new Pen(Brushes.Black), new Point(x_zero0, Y_viewPort(ListY[(int)q1], viewport, minY_Window, RangeY)), new Point(x_dev0, Y_viewPort(ListY[(int)q1], viewport, minY_Window, RangeY)));
                    g.DrawString("Q1=" + ListY[(int)q1], smallFont, Brushes.Black, new Point(x_dev0, Y_viewPort(ListY[(int)q1], viewport, minY_Window, RangeY)));

                    g.DrawLine(new Pen(Brushes.Black), new Point(x_zero0, Y_viewPort(ListY[(int)q2], viewport, minY_Window, RangeY)), new Point(x_dev0, Y_viewPort(ListY[(int)q2], viewport, minY_Window, RangeY)));
                    g.DrawString("Q2=" + ListY[(int)q2], smallFont, Brushes.Black, new Point(x_dev0, Y_viewPort(ListY[(int)q2], viewport, minY_Window, RangeY)));

                    g.DrawLine(new Pen(Brushes.Black), new Point(x_zero0, Y_viewPort(ListY[(int)q3], viewport, minY_Window, RangeY)), new Point(x_dev0, Y_viewPort(ListY[(int)q3], viewport, minY_Window, RangeY)));
                    g.DrawString("Q3=" + ListY[(int)q3], smallFont, Brushes.Black, new Point(x_dev0, Y_viewPort(ListY[(int)q3], viewport, minY_Window, RangeY)));

                    if(checkBox3.Checked)
                    {
                        double rSquared, yIntercept, slope;
                        LinearRegression(ListXnotord.ToArray(), ListYnotord.ToArray(), out rSquared, out yIntercept, out slope);
                        g.DrawLine(new Pen(Brushes.Black), new Point(X_viewPort(minX_Window, viewport, minX_Window, RangeX), Y_viewPort(yIntercept, viewport, minY_Window, RangeY)), 
                                                           new Point(X_viewPort(maxX_Window, viewport, minX_Window, RangeX), Y_viewPort(yIntercept + (slope * maxX_Window), viewport, minY_Window, RangeY)));

                    }

                    pictureBox1.Image = b;
                }
                else
                {
                    MessageBox.Show("Select an Integer or a Double variable!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Select two distinct Variables to display!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        Type ParseString(string str)
        {

            Int32 intValue;
            double doubleValue;
            bool boolValue;
            DateTime dateValue;

            // Place checks higher in if-else statement to give higher priority to type.

            if (double.TryParse(str, out doubleValue))
                return doubleValue.GetType();
            else if (Int32.TryParse(str, out intValue))
                return intValue.GetType();
            else if (bool.TryParse(str, out boolValue))
                return boolValue.GetType();
            else if (DateTime.TryParse(str, out dateValue))
                return dateValue.GetType();
            else return str.GetType();

        }

        public static void LinearRegression(
            double[] xVals,
            double[] yVals,
            out double rSquared,
            out double yIntercept,
            out double slope)
        {
            if (xVals.Length != yVals.Length)
            {
                throw new Exception("Input values should be with the same length.");
            }

            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviates = 0;

            for (var i = 0; i < xVals.Length; i++)
            {
                var x = xVals[i];
                var y = yVals[i];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            var count = xVals.Length;
            var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

            var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            var dblR = rNumerator / Math.Sqrt(rDenom);

            rSquared = dblR * dblR;
            yIntercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
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
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            if (((dt.Columns[comboBox4.GetItemText(comboBox4.SelectedItem)].DataType == typeof(int)) || (dt.Columns[comboBox4.GetItemText(comboBox4.SelectedItem)].DataType == typeof(double)))
                && comboBox4.SelectedItem != null)
            {
                Dictionary<double, double> list = new Dictionary<double, double>();
                double avg = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    double Z = double.Parse((row.ItemArray[row.Table.Columns.IndexOf(comboBox4.GetItemText(comboBox4.SelectedItem))].ToString()));
                    avg = avg + (Z - avg) / (i+1);
                    if(list.ContainsKey(Z))
                    {
                        list[Z]++;
                    }
                    else
                    {
                        list.Add(Z, 1);
                    }
                }

                richTextBox3.Text = "Variable: " + comboBox4.GetItemText(comboBox4.SelectedItem).ToString() + "\nMean = " + avg + "\n";
                richTextBox3.AppendText("Value\tCounts\tFreq\n");
                foreach (KeyValuePair<double, double> k in list)
                {
                    richTextBox3.AppendText(k.Key.ToString() + "\t" + k.Value.ToString() + "\t" + (k.Value/ dt.Rows.Count).ToString() + "\n");
                }



            }
            else
            {
                MessageBox.Show("Select an Integer or a Double variable! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
   
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            maxValues = (int)numericUpDown3.Value;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null && comboBox3.SelectedItem != null && comboBox2.GetItemText(comboBox2.SelectedItem) != comboBox3.GetItemText(comboBox3.SelectedItem))
            {
                if (((dt.Columns[comboBox2.GetItemText(comboBox2.SelectedItem)].DataType == typeof(int)) || (dt.Columns[comboBox2.GetItemText(comboBox2.SelectedItem)].DataType == typeof(double)))
                   && ((dt.Columns[comboBox3.GetItemText(comboBox3.SelectedItem)].DataType == typeof(int)) || (dt.Columns[comboBox3.GetItemText(comboBox3.SelectedItem)].DataType == typeof(double))))
                {
                    initializeGraphics2();
                    initializeDP();

                    double maxX_Window = maxX;
                    double maxY_Window = maxY;
                    double minX_Window = 0;
                    double minY_Window = 0;
                    double RangeX = maxX_Window - minX_Window;
                    double RangeY = maxY_Window - minY_Window;
                    Rectangle viewport = new Rectangle((pictureBox2.Width / 8), (pictureBox2.Height / 8), (pictureBox2.Width / 2), (pictureBox2.Height / 2));
                    Rectangle viewport2 = new Rectangle((pictureBox2.Width / 8), (pictureBox2.Height / 8)+(pictureBox2.Height /2) , (pictureBox2.Width / 2), (pictureBox2.Height - ((pictureBox2.Height / 8) + (pictureBox2.Height / 2)) - 5 ));
                    Rectangle viewport3 = new Rectangle((pictureBox2.Width / 8)+(pictureBox2.Width /2), (pictureBox2.Height / 8), (pictureBox2.Width - ((pictureBox2.Width / 8) + (pictureBox2.Width / 2)) - 5), (pictureBox2.Height / 2));
                    g2.DrawRectangle(Pens.Black, viewport);
                    

                    int x_Device = X_viewPort(maxX_Window, viewport, minX_Window, RangeX);
                    int y_Device = Y_viewPort(maxY_Window, viewport, minY_Window, RangeY);
                    int x_zero = X_viewPort(minX_Window, viewport, minX_Window, RangeX);
                    int y_zero = Y_viewPort(minY_Window, viewport, minY_Window, RangeY);

                    int x_Device_splitted = X_viewPort(maxX_Window/3 , viewport, minX_Window, RangeX);
                    int y_Device_splitted = Y_viewPort(maxY_Window - (maxY_Window/4) , viewport, minY_Window, RangeY);

                    g2.DrawString(comboBox2.SelectedItem.ToString(), bigFont, Brushes.Black, new Point(x_Device_splitted, y_Device - 50));
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                    g2.DrawString(comboBox3.SelectedItem.ToString(), bigFont, Brushes.Black, new Point(x_zero - 50, y_Device_splitted), drawFormat);
                    
                    
                    for (int i = 0; i < listOfInterval_X.Count+2; i++)
                    {
                        int x_sliding = X_viewPort(i * (maxX_Window / (listOfInterval_X.Count + 2)), viewport, minX_Window, RangeX);
                        g2.DrawLine(new Pen(Brushes.Black), new Point(x_sliding, y_Device), new Point(x_sliding, y_zero));
                    }

                    for (int i = 0; i < listOfInterval_Y.Count+2; i++)
                    {
                        int y_sliding = Y_viewPort(i * (maxY_Window / (listOfInterval_Y.Count + 2)), viewport, minY_Window, RangeY);
                        g2.DrawLine(new Pen(Brushes.Black), new Point(x_Device, y_sliding), new Point(x_zero, y_sliding));
                    }

                    int X = 1;
                    int Y = 1;
                    foreach (interval intervX in listOfInterval_X)
                    {
                        int x_sliding = X_viewPort(((X * (maxX_Window / (listOfInterval_X.Count + 2))) + (maxX_Window / (listOfInterval_X.Count + 2) / 8)), viewport, minX_Window, RangeX);
        
                        foreach (interval intervY in listOfInterval_Y)
                        {
                            int y_sliding = Y_viewPort((maxY_Window - ((Y * (maxY_Window / (listOfInterval_Y.Count + 2))) + (maxY_Window / (listOfInterval_Y.Count + 2) / 4))), viewport, minY_Window, RangeY);
                            if (X == 1)
                            {
                                int x_sliding_reduced = X_viewPort((maxX_Window / (listOfInterval_X.Count + 2) / 8), viewport, minX_Window, RangeX);

                                g2.DrawString(intervY.lowerBound.ToString() + "-" + intervY.upperBound.ToString(), smallFont, Brushes.Red, new Point(x_sliding_reduced, y_sliding));

                            }

                            if (Y == 1)
                            {
                                int y_sliding_reduced = Y_viewPort((maxY_Window - ((maxY_Window / (listOfInterval_Y.Count + 2)) / 4)), viewport, minY_Window, RangeY);
                                g2.DrawString(intervX.lowerBound.ToString() + "-" + intervX.upperBound.ToString(), smallFont, Brushes.Red, new Point(x_sliding, y_sliding_reduced));
                            }
                            Tuple<interval, interval> foundTuple = new Tuple<interval, interval>(intervX, intervY);
                            int freq = 0;
                            if (freqs.ContainsKey(foundTuple))
                            {
                                freq = freqs[foundTuple];
                            }
                            if(radioButton5.Checked)
                            {
                                g2.DrawString(freq.ToString(), smallFont, Brushes.Red, new Point(x_sliding, y_sliding));
                            }else
                            {
                                g2.DrawString(Math.Round((double)freq/(double)maxValues, 3).ToString(), smallFont, Brushes.Red, new Point(x_sliding, y_sliding));
                            }
                                
                            if (Y == listOfInterval_Y.Count)
                            {
                                int y_sliding_marginal = Y_viewPort((maxY_Window - (((Y+1) * (maxY_Window / (listOfInterval_Y.Count + 2))) + (maxY_Window / (listOfInterval_Y.Count + 2) / 4))), viewport, minY_Window, RangeY);
                                if (radioButton5.Checked)
                                {
                                    g2.DrawString(intervX.marginalFrequency.ToString(), smallFont, Brushes.Red, new Point(x_sliding, y_sliding_marginal));
                                }else
                                {
                                    g2.DrawString(Math.Round(intervX.marginalFrequency/maxValues, 3).ToString(), smallFont, Brushes.Red, new Point(x_sliding, y_sliding_marginal));
                                }

                                if(checkBox2.Checked)
                                {
                                    int y_device2 = Y_viewPort(maxY_Window, viewport2, minY_Window, RangeY);
                                    int y_zero2 = Y_viewPort(minY_Window, viewport2, minY_Window, RangeY);
                                    int x_rec_start = X_viewPort((X * (maxX_Window / (listOfInterval_X.Count + 2))), viewport2, minX_Window, RangeX);
                                    int x_rec_stop = X_viewPort(((X+1) * (maxX_Window / (listOfInterval_X.Count + 2))), viewport2, minX_Window, RangeX);
                                    int y_rec_stop = Y_viewPort((maxY_Window*(intervX.marginalFrequency / maxValues)) , viewport2, minY_Window, RangeY);

                                    Rectangle recX = new Rectangle(x_rec_start, y_device2, x_rec_stop - x_rec_start, y_zero2 - y_rec_stop);
                                    g2.FillRectangle(Brushes.Blue, recX);
                                    g2.DrawRectangle(Pens.Red, recX);
                                }
                            }
                            if (X == listOfInterval_X.Count)
                            {
                                int x_sliding_marginal = X_viewPort((((X+1) * (maxX_Window / (listOfInterval_X.Count + 2))) + (maxX_Window / (listOfInterval_X.Count + 2) / 8)), viewport, minX_Window, RangeX);
                                if (radioButton5.Checked)
                                {
                                    g2.DrawString(intervY.marginalFrequency.ToString(), smallFont, Brushes.Red, new Point(x_sliding_marginal, y_sliding));
                                }else
                                {
                                    g2.DrawString(Math.Round(intervY.marginalFrequency/maxValues, 3).ToString(), smallFont, Brushes.Red, new Point(x_sliding_marginal, y_sliding));
                                }

                                if (checkBox2.Checked)
                                { 
                                    int x_zero3 = X_viewPort(minX_Window, viewport3, minX_Window, RangeX);
                                    int x_rec_stop = X_viewPort((maxX_Window * (intervY.marginalFrequency / maxValues)), viewport3, minX_Window, RangeX);
                                    int y_rec_start = Y_viewPort((maxY_Window - ((Y * (maxY_Window / (listOfInterval_Y.Count + 2))) )), viewport3, minY_Window, RangeY);
                                    int y_rec_stop = Y_viewPort((maxY_Window - (((Y+1) * (maxY_Window / (listOfInterval_Y.Count + 2))))), viewport3, minY_Window, RangeY);

                                    Rectangle recY = new Rectangle(x_zero3, y_rec_start, x_rec_stop - x_zero3, y_rec_stop - y_rec_start);
                                    g2.FillRectangle(Brushes.Blue, recY);
                                    g2.DrawRectangle(Pens.Red, recY);
                                }
                            }

                            Y++;
                        }
                        Y = 1;
                        X++;
                    }
                    pictureBox2.Image = b2;
                }
                else
                {
                    MessageBox.Show("Select an Integer or a Double variable from 'chart' page! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Select Variables to display from 'chart' page!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
