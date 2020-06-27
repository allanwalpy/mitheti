using System;

namespace Mitheti.Core
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            GetActiveWindow Test = new GetActiveWindow();

            for (int i = 0; i < 10; i++)
            {
                Test.Test();
                Console.WriteLine($"-------- end of test # {i} ----------");
            }

            Console.WriteLine("------ test is done -----");
        }
    }
}
