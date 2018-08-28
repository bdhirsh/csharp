﻿using System;
using System.Collections.Generic;

namespace practice
{
    class Program
    {
        static void Main(string[] args)
        {

            // type conversion examples
            int bigVal = 100;
            try {
                byte smallVal = Convert.ToByte(bigVal);
            } catch (Exception e) {
                Console.WriteLine("overflow error: %s", e.Message);
            }

            // collections examples
            List<int> l1 = new List<int>{1, 2};
            l1.Add(5);
            l1.AddRange(new List<int>{3, 4});
            l1.Sort(new practice.SortNegative());
            Dictionary<string, List<int>> d = new Dictionary<string, List<int>>();
            d.Add("l1", l1);
            d.Add("l2", new List<int>{8, 9, 10});
            Console.WriteLine(d.ToDebugString());
            HashSet<string> hs = new HashSet<string>(new string[] {"hello", "hi", "goodbye", "hi", "hi", "goodbye"});
            foreach (var str in hs) {
                Console.Write(str + " ");
            }
            Console.WriteLine();

            //indexer example
            practice.StringStore store = new practice.StringStore(5);
            store[0] = "a";
            store[1] = "b"; //store now looks like {"a", "b", null, null, null}
            Console.WriteLine(store.ToString());
            store.size = 2; //resizes store to be of length 2, preserving the first 2 entries
            Console.WriteLine(store.ToString());

            // scraper example
            Console.WriteLine(scraper.Scraper.scrape("nytimes.com"));

            // server example
            Server server = new Server(5000, "127.0.0.1");
            server.start();
        }
    }
}
