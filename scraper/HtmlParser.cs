using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace scraper {

    class HtmlParser {

        //TODO: expand class to actually parse HTML into DOM tree

        public static HashSet<string> extractLinks(string text) {
            Regex linksMatch = new Regex(@" href=""http(s?)([^""]+)""");
            HashSet<string> links = new HashSet<string>();
            MatchCollection matches = linksMatch.Matches(text);
            //System.IO.File.WriteAllText(@"./website.txt", text);
            Console.WriteLine("\nmatches: " + matches.Count);
            foreach (Match m in matches) {
                //Console.WriteLine(m.Groups.Count + ".  val: " + m.Groups[0].Value + "..." + m.Groups[2].Value);
                links.Add("http" + m.Groups[1].Value + m.Groups[2].Value);
            }

            return links;
        }

        
    }
}