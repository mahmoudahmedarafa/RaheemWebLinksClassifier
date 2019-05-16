using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigiSay_Task
{
    class Classifier
    {
        string Link { get; set; }
        string[] Bag = { "rahim", "raheem", "رحيم", "rahem" };
        WebBrowser TaskWebBrowser;
        List <double> X;
        List <double> Y;

        public Classifier() { }
        public Classifier(string link, WebBrowser webBrowser)
        {
            Link = link.ToLower();
            TaskWebBrowser = webBrowser;
            X = new List<double>();
            Y = new List<double>();
        }

        public bool BagMatch(string pattern)
        {
            for (int i = 0; i < Bag.Length; i++)
            {
                string word = Bag[i];

                if (pattern.Contains(word))
                {
                    return true;
                }
            }
            return false;
        }

        public string IsRelated()
        {
            //check if "Link" works
            WebHelper webHelper = new WebHelper(Link);
            //if it works, check bag of words
            if (webHelper.IsValidLink())
            {
                //if there is some bag word matches with the link, then return true
                if (BagMatch(Link))
                    return "Related";
            }
            //if it doesn't work, then get page content
            else
            {
                //get attributes of "content"



                WebClient webClient = new WebClient();

                string html = webClient.DownloadString(Link);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                var metaElements = doc.DocumentNode.Descendants("meta");
                var attributesInMetaElements = doc.DocumentNode.SelectNodes("//meta[@attribute]").LastOrDefault();








                //HtmlDocument domDocument = TaskWebBrowser.Document;
                //HtmlElementCollection metaElements = doc.GetElementsByTagName("meta"); // get all the meta elements

                int matchingContents = 0;
                int metaElementsLength = 0;

                //foreach (var meta in metaElements)
                //{
                //    metaElementsLength++;

                //    string content = meta.GetAttribute("content");

                //    if (BagMatch(content))
                //        matchingContents++;
                //}

                //foreach (HtmlElement meta in metaElements)
                //{


                //    metaElementsLength++;
                //    string content = meta.GetAttribute("content");

                //    if (BagMatch(content))
                //        matchingContents++;
                //}


                //if(matchingContents >= lambda * metaElementsLength)
                //    return "Related";

                
            }
                //check in each of them for the bag of words
                        //if at least, there is 50% of the "content" match with the bag, then return true

            return "NotRelated";
        }
    }
}
