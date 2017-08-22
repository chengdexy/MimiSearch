using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Text;

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

        // when downloading images, save item-urls into database first
        internal void SaveItemUrls(string[] urls)
        {
            SqlConnection cnn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=MimiSearcher;User ID=sa;Password=darkmoon1");
            cnn.Open();
            SqlCommand cmd = new SqlCommand
            {
                Connection = cnn
            };
            foreach (var url in urls)
            {
                string sqlStr = $"insert into Old_Item_Url(item_url) values ('{url}')";
                cmd.CommandText = sqlStr;
                cmd.ExecuteNonQuery();
            }
            cnn.Close();
        }

        internal void OutputHtml(string[] urls)
        {
            List<string> urlList = new List<string>(urls);
            urlList.Reverse();
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<body>");
            foreach (var url in urlList)
            {
                sb.Append($"<img src=\"{url}\" />");
                sb.Append("<hr />");
            }
            sb.Append("</body>");
            sb.Append("</html>");

            MailSender ms = new MailSender
            {
                SendTo="hi@chengdexy.cn",
                IsHtml=true,
                BodyHtml = sb.ToString()
            };
            try
            {
                ms.Send();
            }
            catch (SmtpException)
            {
                throw;
            }
        }
    }
}