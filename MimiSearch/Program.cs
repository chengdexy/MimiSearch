using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MimiSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Are you ready?");
            string confirm = Console.ReadLine();
            if (confirm != "Let's Go!")
            {
                return;
            }

            string url = @"http://www.mimihhh.com/forumdisplay.php?fid=46";
            Uri uri = new Uri(url);
            string onePageHtmlString = WebClientHelper.GetHtml(uri);
            List<CartoonUrl> urlList = AnalysisHelper.GetCartoonUrlList(onePageHtmlString);

            FileStream fs = new FileStream("list.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            foreach (CartoonUrl item in urlList)
            {
                string outString = $"{item.Title} {item.CreatDate} {item.Url}\r\n";
                sw.WriteLine(outString);
            }
            sw.Close();
            System.Diagnostics.Process.Start("notepad.exe", "list.txt");
            Console.ReadKey();
        }
    }

    static class WebClientHelper
    {
        private static WebClient _wc = new WebClient();

        public static string GetHtml(Uri url)
        {
            Encoding enc = Encoding.GetEncoding("GB2312");
            byte[] pageData = _wc.DownloadData(url.ToString());
            return enc.GetString(pageData);
        }
    }

    static class AnalysisHelper
    {
        //正则:获取table.row
        private const string _regRows = "(<table cellspacing=\"0\" cellpadding=\"4\" class=\"row\")(.|\\n)*?(</table>)";
        //正则:解析table.row中内容
        private const string _regUrl = "(?<=href=\"viewthread.php\\?tid=)[0-9]{1,8}(?=&amp;extra=page)";
        private const string _regTitle = "(?<=&amp;extra=page%3D1\">).*?(?=/a)";
        private const string _regDate = "(?<=lighttxt\">)20[0-9]{2}-[0-9]{1,2}-[0-9]{1,2}(?=</span>)";

        public static List<CartoonUrl> GetCartoonUrlList(string html)
        {
            //拆分为若干个table.row
            List<string> rowList = RegularHelper.GetMatchList(html, _regRows);
            //处理每个row创建一个item对象
            List<CartoonUrl> resultList = new List<CartoonUrl>();
            foreach (string row in rowList)
            {
                //jiexi解析row中的信息,创建item对象
                string url = "TID: " + RegularHelper.GetFirstMatch(row, _regUrl);
                string title = RegularHelper.GetFirstMatch(row, _regTitle).Replace("<", "");
                string creatDate = RegularHelper.GetFirstMatch(row, _regDate).Replace("-", ".");
                resultList.Add(new CartoonUrl()
                {
                    Url = url,
                    Title = title,
                    CreatDate = creatDate
                });
            }
            return resultList;
        }
    }

    class CartoonUrl
    {
        private string _url;
        private string _title;
        private string _creatDate;

        public string Url { get => _url; set => _url = value; }
        public string Title { get => _title; set => _title = value; }
        public string CreatDate { get => _creatDate; set => _creatDate = value; }
    }

    static class RegularHelper
    {
        private static MatchCollection _matches;
        private static Match _match;
        private static Regex _regex;

        public static List<string> GetMatchList(string html, string regular)
        {
            _regex = new Regex(regular, RegexOptions.IgnoreCase);
            _matches = _regex.Matches(html);
            List<string> resultList = new List<string>();
            foreach (Match match in _matches)
            {
                resultList.Add(match.Value);
            }
            return resultList;
        }

        public static string GetFirstMatch(string row, string regular)
        {
            _regex = new Regex(regular, RegexOptions.IgnoreCase);
            _match = _regex.Match(row);
            return _match.Value;
        }
    }
}
