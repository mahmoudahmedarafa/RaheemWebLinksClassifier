using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiSay_Task
{
    class FileReader
    {
        public FileReader() { }

        public void ReadDataSet()
        {
            string path = Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()) + "DataSet.csv";
            string text = System.IO.File.ReadAllText(path);

            string[] lines = System.IO.File.ReadAllLines(path);
            List<Tuple<string, int>> DataSet = new List<Tuple<string, int>>();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] lineSplitter = lines[i].Split(',');
                DataSet.Add(new Tuple<string, int>(lineSplitter[0], int.Parse(lineSplitter[1])));
            }

            // LinearRegression regression = new LinearRegression(DataSet, );
        }
    }
}
