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
            // 首先需要一个爬虫
            string searchRoot = @"http://www.mimihhh.com/forumdisplay.php?fid=46";
            MimiSearcher mms = new MimiSearcher(searchRoot);
            // 开始爬
            mms.Craw();
            // 输出爬取结果
            mms.Output();
        }
    }
}
