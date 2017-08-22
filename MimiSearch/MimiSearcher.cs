using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
                    string[] tmpUrls = UrlManager.GetAll(UrlType.Item);
                    tmpUrls = ClearExsitItems(tmpUrls);
                    if (tmpUrls.Length == 0)
                    {
                        PassOutputAndStopRunning();
                    }
                    urls.AddRange(tmpUrls);
                    break;
            }
            foreach (string url in urls)
            {
                string html = HtmlDownloader.DownloadHtml(url);
                string[] newUrls = HtmlParser.Parse(html, type);
                UrlManager.AddRange(newUrls, type);
            }
        }

        private void PassOutputAndStopRunning()
        {
            Environment.Exit(0);
        }

        private string[] ClearExsitItems(string[] tmpUrls)
        {
            SqlConnection cnn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=MimiSearcher;User ID=sa;Password=darkmoon1");
            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            List<string> tmpList = new List<string>(tmpUrls);
            List<string> tmpList2 = new List<string>(tmpUrls);
            foreach (var url in tmpList2)
            {
                string sqlStr = $"select count(*) from Old_Item_Url where item_url='{url}'";
                cmd.CommandText = sqlStr;
                if ((int)cmd.ExecuteScalar() > 0)
                {
                    tmpList.Remove(url);
                }
            }
            cnn.Close();
            return tmpList.ToArray();
        }

        internal void Output()
        {
            HtmlOutputer = new HtmlOutputer();
            // save all item-urls into database
            HtmlOutputer.SaveItemUrls(UrlManager.GetAll(UrlType.Item));
            // build a html file with these image-urls
            HtmlOutputer.OutputHtml(UrlManager.GetAll(UrlType.Image));


            //
            // since 2017-8-21 i dont need download any image,
            // instead build a html with these urls to send email
            //
            //
            //foreach (string url in UrlManager.GetAll(UrlType.Image))
            //{
            //    HtmlDownloader.DownloadImage(url);
            //}
        }
    }
}