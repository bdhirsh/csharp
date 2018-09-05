using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace scraper {
    class Scraper {

        //TODO: expand out the range of scraping capabilities

        public static HashSet<string> scrapeLinks(string uri) {
            using (WebClient wc = new WebClient()) {
                wc.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                byte[] arr = wc.DownloadData(uri);
                string download = Encoding.ASCII.GetString(arr);
                string contentEncoding = wc.ResponseHeaders["Content-Encoding"];

                return HtmlParser.extractLinks(download);
            }

        }

    }
}