using System;
using System.Net;
using System.Text;
using System.IO;

namespace scraper {
    class Scraper {

        public static string scrape(string uri) {
            using (WebClient wc = new WebClient()) {
                wc.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                byte[] arr = wc.DownloadData("http://www.dotnetperls.com/");
                string download = Encoding.ASCII.GetString(arr);

                string contentEncoding = wc.ResponseHeaders["Content-Encoding"];

                return contentEncoding;
            }


            
            // Stream myStream = wc.OpenRead(uri);
            // StreamReader sr = new StreamReader(myStream);
            // string download = sr.ReadToEnd();
            // myStream.Close();
            // return download;

            // byte[] myDataBuffer = wc.DownloadData (uri);
            // string download = Encoding.ASCII.GetString(myDataBuffer);
            // return download;
        }

    }
}