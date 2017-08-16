using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MimiSearch
{
    internal class HtmlParser
    {
        public HtmlParser()
        {
        }

        internal string[] Parse(string html, MimiSearcher.UrlType type)
        {
            List<string> urls = new List<string>();
            switch (type)
            {
                // parse page-urls from page-url
                case MimiSearcher.UrlType.Page:
                    urls = ParseRoot(html);
                    break;
                // parse item-urls from page-url
                case MimiSearcher.UrlType.Item:
                    urls = ParsePage(html);
                    break;
                // parse image-urls from item-url
                case MimiSearcher.UrlType.Image:
                    urls = ParseItem(html);
                    break;
                default:
                    break;
            }
            return urls.ToArray();
        }

        private List<string> ParseItem(string html)
        {
            // find all image-urls in html
            List<string> list = new List<string>();
            // ...
            if (!html.Contains("www.ded22.com"))
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var divNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='t_msgfont']");
                var imgNodes = divNode.SelectNodes("img | font/img | font/font/img");
                foreach (var node in imgNodes)
                {
                    string text = node.GetAttributeValue("src", "");
                    if (text.Contains("%"))
                    {
                        text = text.Split('%')[0];
                    }
                    string url = text;
                    list.Add(url);
                }
            }
            return list;
        }

        private List<string> ParsePage(string html)
        {
            // find all item-urls in html
            List<string> list = new List<string>();
            // ...
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//td[@class='f_title']/div[@class='right']");
            foreach (var node in nodes)
            {
                if (node.SelectSingleNode("img[@alt='本版置顶']") == null)
                {
                    // TODO: save urls with there titles after filte by white names
                    string title = node.SelectSingleNode("../a").InnerText;
                    if (title.Contains("黑白中文") || title.Contains("彩漫中文"))
                    {
                        string text = node.SelectSingleNode("../a").GetAttributeValue("href", "");
                        string url = "http://www.mimihhh.com/" + Regex.Match(text, @"viewthread\.php\?tid=\d{1,7}").Value;
                        list.Add(url);
                    }
                }
            }
            return list;

        }

        private List<string> ParseRoot(string html)
        {
            // find all page-urls in html
            List<string> list = new List<string>();
            // ...
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var urls = htmlDoc.DocumentNode.SelectNodes("//a[@class='p_num']").Select(p => p.GetAttributeValue("href", ""));
            foreach (var url in urls)
            {
                if (!string.IsNullOrEmpty(url))
                {
                    list.Add(@"http://www.mimihhh.com/" + url);
                }
            }
            return list;
        }
    }
}