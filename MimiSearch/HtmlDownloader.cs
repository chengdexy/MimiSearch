﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace MimiSearch
{
    internal class HtmlDownloader
    {
        private WebClient _wc;

        public HtmlDownloader()
        {
            _wc = new WebClient();
        }

        internal string DownloadHtml(string url)
        {
            Encoding enc = Encoding.GetEncoding("gb2312");
            _wc.Encoding = enc;
            return _wc.DownloadString(url);
        }

        internal void DownloadImage(string url)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "imgs\\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                _wc.DownloadFile(url, path + Path.GetFileName(url));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}