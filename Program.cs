using HtmlAgilityPack;
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
            var urlList = new List<string>()
            {
                 @"http://m.bom.gov.au/vic/melbourne/",
                 @"http://m.bom.gov.au/nsw/sydney/",
                 @"http://m.bom.gov.au/qld/brisbane/"
            };
            var sb = new StringBuilder();

            foreach (string url in urlList)
            {
                var result = await DownloadPageAsync(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(result);

                var temp = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='current-temp']").InnerText.Replace(@"&deg;", "").Replace(@"\t", "").Replace(@"\n", "").Trim();
                var location = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='location-name has-icon']").InnerText.Replace(@"\t", "").Replace(@"\n", "").Trim();

                var precis = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='summary has-icon']").InnerText.Replace(@"\t", "").Replace(@"\n", "").Trim();
                var city = url.Split('/').Reverse().Skip(1).Take(1).First().ToTitleCase();

                Console.WriteLine("------------------------");
                Console.WriteLine($"City: { city }");
                Console.WriteLine($"Temperature: { temp }");
                Console.WriteLine(precis);
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
