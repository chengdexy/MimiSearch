﻿using System;
using System.Collections.Generic;

namespace MimiSearch
{
    internal class MimiSearcher
    {
        // the root url
        public string SearchRoot { get; set; }
        // url manager
        public UrlManager UrlManager { get; set; }
        // downloader for downloading html code or image files
        public HtmlDownloader HtmlDownloader { get; set; }
        // html code parser
        public HtmlParser HtmlParser { get; set; }
        // outputer
        public HtmlOutputer HtmlOutputer { get; set; }

        public enum UrlType
        {
            Page = 1,
            Item = 2,
            Image = 3
        }

        public MimiSearcher(string url)
        {
            SearchRoot = url;
            UrlManager = new UrlManager();
            HtmlDownloader = new HtmlDownloader();
            HtmlParser = new HtmlParser();
            HtmlOutputer = new HtmlOutputer();

            // add root-url into url-manager
            UrlManager.Add(SearchRoot, UrlType.Page);
        }

        public void Craw()
        {
            // get all page-urls for craw
            GetAll(UrlType.Page);
            // get all item-urls in such pages
            GetAll(UrlType.Item);
            // get all image-urls in such items
            GetAll(UrlType.Image);
        }


        private void GetAll(UrlType type)
        {
            List<string> urls = new List<string>();
            switch (type)
            {
                case UrlType.Page:
                    urls.Add(SearchRoot);
                    break;
                case UrlType.Item:
                    urls.AddRange(UrlManager.GetAll(UrlType.Page));
                    break;
                case UrlType.Image:
                    urls.AddRange(UrlManager.GetAll(UrlType.Item));
                    break;
            }
            foreach (string url in urls)
            {
                string html = HtmlDownloader.DownloadHtml(url);
                string[] newUrls = HtmlParser.Parse(html, type);
                UrlManager.AddRange(newUrls, type);
            }
        }

        internal void Output()
        {
            // create .csv file to out-put image-detail list
            HtmlOutputer = new HtmlOutputer();
            HtmlOutputer.CreateCSV(UrlManager.GetAll(UrlType.Image));
            // download images to local
            foreach (string url in UrlManager.GetAll(UrlType.Image))
            {
                HtmlDownloader.DownloadImage(url);
            }
        }
    }
}