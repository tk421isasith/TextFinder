using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace WordScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WebClient();
            var url = args.Length > 0 && Uri.IsWellFormedUriString(args[0], UriKind.Absolute) ?
                args[0] : "http://www.philstar.com/nation";
            var keywords = args.Length > 1 ? args[1] : "corpse";
            var pageContent = client.DownloadString(url);
            var keywordLocation = pageContent.IndexOf(keywords, StringComparison.OrdinalIgnoreCase);
            if (keywordLocation >= 0)
            {
                Console.WriteLine(url + " are talking about " + keywords + " today.");
                Console.WriteLine("\nSnippet:\n" + pageContent.Substring(keywordLocation, 100));
            }
            else
            {
                Console.WriteLine("Keyword not found!");
            }
            Console.ReadKey();
        }
    }
}
