using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DigiSay_Task
{
    class WebHelper
    {
        string Link { get; set; }
        //string WebPageContent { get; set; }

        public WebHelper() { }
        public WebHelper(string link)
        {
            Link = link;
        }

        public bool IsValidLink()
        {
            var client = new WebClient();

            try
            {
                client.DownloadString(Link);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
