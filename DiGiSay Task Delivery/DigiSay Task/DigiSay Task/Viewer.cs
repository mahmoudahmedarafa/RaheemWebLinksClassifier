using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigiSay_Task
{
    class Viewer
    {
        string Result;

        public Viewer() { }
        public Viewer(string result)
        {
            Result = result;
        }

        public void view()
        {
            MessageBox.Show(Result);
        }
    }
}
