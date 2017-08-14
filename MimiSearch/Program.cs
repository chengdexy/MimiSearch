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
            //
            //尝试用C#还原学到的简单爬虫
            //

            // 首先需要一个爬虫
            string searchRoot = @"http://www.mimihhh.com/forumdisplay.php?fid=46";
            MimiSearcher mms = new MimiSearcher(searchRoot);
            // 开始爬
            Console.WriteLine($"成功生成爬虫, 已派往{searchRoot}...");
            mms.Craw();
            // 输出爬取结果
            Console.WriteLine($"爬虫已返回, 共爬取{mms.UrlManager.GetAll(MimiSearcher.UrlType.Page).Count() }页{mms.UrlManager.GetAll(MimiSearcher.UrlType.Item).Count()}项{mms.UrlManager.GetAll(MimiSearcher.UrlType.Image).Count()}张图片, 是否开始下载图片?[Y/N]");
            if (Console.ReadLine().ToLower() == "y")
            {
                mms.Output();
                Console.WriteLine("下载完成!");
            }
            else
            {
                Console.WriteLine("下载已取消, 按任意键退出.");
                Console.ReadKey();
            }
        }
    }
}
