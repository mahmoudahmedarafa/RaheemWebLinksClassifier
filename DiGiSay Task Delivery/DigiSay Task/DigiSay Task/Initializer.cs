using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigiSay_Task
{
    class Initializer
    {
        string StartingLink { get; set; }

        public Initializer() { }

        public Initializer(string link)
        {
            WebHelper helper = new WebHelper(link);
            Viewer viewer;

            if (helper.IsValidLink() == false)
            {
                viewer = new Viewer("Not related");
            }
            else
            {
                StartingLink = link;

                LinearRegression regression = new LinearRegression();

                Sample testSample = regression.MapLinkToNumbers(link);

                string classfication = regression.GetClassification(testSample.X.X1, testSample.X.X2);
                viewer = new Viewer(classfication);
            }

            viewer.view();
        }
    }
}
