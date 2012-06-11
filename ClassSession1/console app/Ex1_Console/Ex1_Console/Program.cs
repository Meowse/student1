using System;

namespace Ex1_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Hello, " + args[0] + "!");
            Console.Out.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
