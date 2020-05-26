using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BOMScraper
{
    class Program
    {

        static async Task Main(string[] args)
        {

            var result = await DownloadPageAsync(@"http://www.bom.gov.au/index.php");
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(result);

            var capitalNodes = htmlDoc.DocumentNode.Descendants(0).Where(n => n.HasClass("capital")).ToList();

            var cities = new List<string>() { "Sydney", "Melbourne", "Brisbane" };


            foreach (var node in capitalNodes)
            {

                var city = node.ChildNodes.Elements("h3").FirstOrDefault().InnerText;

                if (!cities.Contains(city))
                    continue;

                var now = node.Descendants(0).Where(n => n.HasClass("now")).FirstOrDefault().Descendants(0).Where(m => m.HasClass("val")).FirstOrDefault().InnerText.Replace("&deg", "").Replace(";","");
                var precis = node.Descendants(0).Where(n => n.HasClass("precis")).FirstOrDefault().InnerText;


                Console.WriteLine($"City: { city }");
                Console.WriteLine($"Temperature: { now }");
                Console.WriteLine(precis);
                Console.WriteLine("--------------------------");
            }

            Console.ReadKey();
        }

        private static async Task<string> DownloadPageAsync(string url)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                client.Dispose();
                if (result != null && result.Length >= 50)
                    return result;
                else
                    return "";
            }
        }
    }
}
