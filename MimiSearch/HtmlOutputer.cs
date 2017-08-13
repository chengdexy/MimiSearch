using System;
using System.IO;

namespace MimiSearch
{
    internal class HtmlOutputer
    {
        public HtmlOutputer()
        {
        }

        internal void CreateCSV(string[] urls)
        {
            FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "result.csv", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            foreach (string url in urls)
            {
                sw.WriteLine(url);
            }
            sw.Close();
            fs.Close();
        }
    }
}