using System;

namespace Ex1_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Out.WriteLine("Hello, World!");
            }
            else
            {
                Console.Out.WriteLine("Hello, " + args[0] + "!");
            }
            Console.Out.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
