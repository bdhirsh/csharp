using System;

namespace firstApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Stack<string> s = new Stack<string>();
            s.push("a");
            s.push("b");
            s.push("c");
            Console.WriteLine("size: " + s.size());
            Console.WriteLine("popped: " + s.pop());
            Console.WriteLine("popped: " + s.pop());
            Console.WriteLine("popped: " + s.pop());
        }
    }
}
