using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome1419();
            Welcome0066();
            Console.ReadKey();
        }

        static partial void Welcome0066();

        private static void Welcome1419()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }
    }
}
