using System;

namespace Ex1_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = (args.Length == 0) ? "World" : args[0];
            Console.Out.WriteLine("Hello, " + name + "!");
            Console.Out.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
