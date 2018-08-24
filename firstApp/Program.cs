using System;

namespace firstApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n---- Stack Example ----\n");
            Stack<string> s = new Stack<string>();
            s.push("a");
            s.push("b");
            s.push("c");
            Console.WriteLine("size: " + s.size());
            Console.WriteLine("popped: " + s.pop());
            Console.WriteLine("popped: " + s.pop());
            Console.WriteLine("popped: " + s.pop());

            Console.WriteLine("\n---- Complex Example ----\n");
            Complex a = new Complex(1, 0);
            Complex b = new Complex(0, 1);
            Complex c = 3*a + 2*b;
            Complex c_conj = Complex.conjugate(c);
            Console.WriteLine("output: " + c_conj.real() + ", " + c_conj.imag());
        }
    }
}
