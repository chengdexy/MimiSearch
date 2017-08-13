using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace MimiSearch
{
    internal class HtmlDownloader
    {
        private WebClient _wc = new WebClient();

        public HtmlDownloader()
        {
        }

        internal string DownloadHtml(string url)
        {
            Encoding enc = Encoding.GetEncoding("utf-8");
            _wc.Encoding = enc;
            return _wc.DownloadString(url);
        }

        internal void DownloadImage(string url)
        {
            // TODO: check for if need '\' between the path and file name
            try
            {
                _wc.DownloadFile(url, AppDomain.CurrentDomain.BaseDirectory + Path.GetFileName(url));
            }
            catch (Exception e)
            {
                Debug.Print(e.Source + ": " + e.Message);
            }
        }
    }
}