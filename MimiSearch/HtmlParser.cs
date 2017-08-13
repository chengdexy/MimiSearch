using System;
using System.Collections.Generic;

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
            return list;
        }

        private List<string> ParsePage(string html)
        {
            // find all item-urls in html
            List<string> list = new List<string>();
            // ...
            return list;

        }

        private List<string> ParseRoot(string html)
        {
            // find all page-urls in html
            List<string> list = new List<string>();
            // ...
            return list;
        }
    }
}