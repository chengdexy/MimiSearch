using System;
using System.Collections.Generic;

namespace MimiSearch
{
    internal class UrlManager
    {
        private Set _pages;
        private Set _items;
        private Set _images;

        public UrlManager()
        {
            _pages = new Set();
            _items = new Set();
            _images = new Set();
        }

        internal string[] GetAll(MimiSearcher.UrlType type)
        {
            string[] urls = null;
            switch (type)
            {
                case MimiSearcher.UrlType.Page:
                    urls = _pages.ToArray();
                    break;
                case MimiSearcher.UrlType.Item:
                    urls = _items.ToArray();
                    break;
                case MimiSearcher.UrlType.Image:
                    urls = _images.ToArray();
                    break;
            }
            return urls;
        }

        internal void Add(string url, MimiSearcher.UrlType type)
        {
            switch (type)
            {
                case MimiSearcher.UrlType.Page:
                    _pages.Add(url);
                    break;
                case MimiSearcher.UrlType.Item:
                    _items.Add(url);
                    break;
                case MimiSearcher.UrlType.Image:
                    _images.Add(url);
                    break;
                default:
                    break;
            }
        }

        internal void AddRange(string[] urls, MimiSearcher.UrlType type)
        {
            switch (type)
            {
                case MimiSearcher.UrlType.Page:
                    foreach (string url in urls)
                    {
                        _pages.Add(url);
                    }
                    break;
                case MimiSearcher.UrlType.Item:
                    foreach (string url in urls)
                    {
                        _items.Add(url);
                    }
                    break;
                case MimiSearcher.UrlType.Image:
                    foreach (string url in urls)
                    {
                        _images.Add(url);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}