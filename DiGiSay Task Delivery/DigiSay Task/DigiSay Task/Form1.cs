using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigiSay_Task
{
    public partial class Form1 : Form
    {
        List<Tuple<string, int>> DataSet = new List<Tuple<string, int>>();
        Object thisLock = new Object();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string link = textBox1.Text;
            webBrowser1.Navigate(new Uri(link));
            Initializer initializer = new Initializer(link);
        }

        public void ReadDataSet()
        {
            lock (thisLock)
            {
                string path = System.IO.Directory.GetCurrentDirectory() + "\\DataSet.csv";
                string text = System.IO.File.ReadAllText(path);

                string[] lines = System.IO.File.ReadAllLines(path);

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] lineSplitter = lines[i].Split(',');
                    DataSet.Add(new Tuple<string, int>(lineSplitter[0], int.Parse(lineSplitter[1])));
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReadDataSet();
            LinearRegression regression = new LinearRegression(DataSet);
        }
    }
}
