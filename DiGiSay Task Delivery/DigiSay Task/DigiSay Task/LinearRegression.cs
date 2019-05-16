using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using edu.stanford.nlp.international.arabic.process;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.util;
using java.io;

namespace DigiSay_Task
{
    public struct RegressionVar
    {
        public int X1 { get; set; }
        public int X2 { get; set; }
    }

    public struct Sample
    {
        public RegressionVar X;
        public int Y;       //Sample Label
    }

    class LinearRegression
    {
        List<Tuple<string, int>> DataSet = new List<Tuple<string,int>>();
        string[] Bag = { "rahim", "raheem", "رحيم", "rahem" };
        RegressionVar[] DataSetRegVars;
        int[] DataSetLabels;

        double[] Bias;
        double Intercept;

        double sum_X1, sum_X2, sum_X1_X2, sum_X1_Square, sum_X2_Square;
        double sum_Y, sum_Y_Square;
        double sum_X1_Y, sum_X2_Y;
        double y_Bar, x1_Bar, x2_Bar;   //means
        double y;                       //prediction

        Object thisLock = new Object();

        public LinearRegression() { }

        public bool BagMatch(List <string> tokens)
        {
            for (int i = 0; i < Bag.Length; i++)
            {
                string word = Bag[i];

                if (tokens.Contains(word))
                {
                    return true;
                }
            }
            return false;
        }

        public LinearRegression(List<Tuple<string, int>> dataSet)
        {
            Bias = new double[3];
            DataSet = dataSet;

            DataSetRegVars = new RegressionVar[dataSet.Count];
            DataSetLabels = new int[dataSet.Count];

            StartTraining();
        }

        void CalcXs()
        {
            sum_X1 = 0;
            sum_X2 = 0;
            sum_X1_X2 = 0;
            sum_X1_Square = 0;
            sum_X2_Square = 0;

            for (int i = 0; i < DataSetRegVars.Length; i++)
            {
                sum_X1_Square += DataSetRegVars[i].X1 * DataSetRegVars[i].X1;
                sum_X2_Square += DataSetRegVars[i].X2 * DataSetRegVars[i].X2;

                sum_X1 += DataSetRegVars[i].X1;
                sum_X2 += DataSetRegVars[i].X2;

                sum_X1_X2 += DataSetRegVars[i].X1 * DataSetRegVars[i].X2;
            }
        }

        void CalcYs()
        {
            double sum_Y = 0;
            double sum_Y_Square = 0;

            for (int i = 0; i < DataSetLabels.Length; i++)
            {
                sum_Y += DataSetLabels[i];
                sum_Y_Square += DataSetLabels[i] * DataSetLabels[i];
            }
        }

        void CalcXsYs()
        {
            sum_X1_Y = 0;
            sum_X2_Y = 0;

            for (int i = 0; i < DataSetRegVars.Length; i++)
            {
                sum_X1_Y += DataSetRegVars[i].X1 * DataSetLabels[i];
                sum_X2_Y += DataSetRegVars[i].X2 * DataSetLabels[i];
            }
        }

        void CalcBiases()
        {
            Bias[1] = (sum_X2_Square * sum_X1_Y - sum_X1_X2 * sum_X2_Y) / (sum_X1_Square * sum_X2_Square - sum_X1_X2 * sum_X1_X2);
            Bias[2] = (sum_X1_Square * sum_X2_Y - sum_X1_X2 * sum_X1_Y) / (sum_X1_Square * sum_X2_Square - sum_X1_X2 * sum_X1_X2);
        }

        void CalcIntercept()
        {
            y_Bar = sum_Y / DataSetLabels.Length;
            x1_Bar = sum_X1 / DataSetRegVars.Length;
            x2_Bar = sum_X2 / DataSetLabels.Length;

            Intercept = y_Bar - Bias[1] * x1_Bar - Bias[2] * x2_Bar;
        }

        void StartCalculation()
        {
            CalcXs();
            CalcYs();
            CalcXsYs();
            CalcBiases();
            CalcIntercept();
        }

        public Sample MapLinkToNumbers(string link)
        {
            WebClient webClient = new WebClient();

            string html = webClient.DownloadString(link);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var metaList = doc.DocumentNode.SelectNodes("//meta");

            int metaElements = metaList.Count;


            int matchingContents = 0;
            foreach (var node in metaList)
            {
                var attributeList = node.Attributes;

                foreach (var attr in attributeList)
                {
                    NLPArabicTokenizer tokenizer = new NLPArabicTokenizer(attr.Value);
                    List<string> tokens = tokenizer.TokenizeSentence();

                    if (BagMatch(tokens))
                        metaElements++;
                }
            }

            Sample sample = new Sample();
            sample.X.X1 = metaElements;
            sample.X.X2 = matchingContents;

            return sample;
        }

        void StartTraining()
        {
            foreach (var sample in DataSet)
            {
                string link = sample.Item1;
                int sampleLabel = sample.Item2;
                int sampleNum = 0;

                WebHelper helper = new WebHelper(link);
                if (helper.IsValidLink() == false)
                    continue;

                Sample sampleFeatures = MapLinkToNumbers(link);

                DataSetRegVars[sampleNum].X1 = sampleFeatures.X.X1;
                DataSetRegVars[sampleNum].X2 = sampleFeatures.X.X2;
                DataSetLabels[sampleNum] = sampleLabel;

                StartCalculation();

                sampleNum++;
            }

            lock (thisLock)
            {
                //save intercept and biases to file
                string[] lines = { Intercept.ToString(), Bias[1].ToString(), Bias[2].ToString() };
                // WriteAllLines creates a file, writes a collection of strings to the file,
                // and then closes the file.  You do NOT need to call Flush() or Close().
                System.IO.File.WriteAllLines(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()) + "\\TrainingOutput.csv", lines);
            }
        }

        public string GetClassification(int x1, int x2)
        {
            int RegIntercept;
            int RegBias1;
            int RegBias2;

            lock (thisLock)
            {
                string path = Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()) + "\\TrainingOutput.csv";
                string[] lines = System.IO.File.ReadAllLines(path);

                RegIntercept = int.Parse(lines[0]);
                RegBias1 = int.Parse(lines[1]);
                RegBias2 = int.Parse(lines[2]);

                y = RegIntercept + RegBias1 * x1 + RegBias2 * x2;

                if (y < 0)
                    return "Related";
                else
                    return "UnRelated";
            }
        }
    }
}
