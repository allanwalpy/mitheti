using System;

namespace Mitheti.Core
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //Test1();
            Test2();
        }

        public static void Test1()
        {
            GetActiveWindow Test = new GetActiveWindow();

            for (int i = 0; i < 10; i++)
            {
                Test.Test();
                Console.WriteLine($"-------- end of test # {i} ----------");
                System.Threading.Thread.Sleep(10000);
            }

            Console.WriteLine("------ test is done -----");
        }

        public static void Test2()
        {
            for (int i = 0; i < 10; i++)
            {
                System.Threading.Thread.Sleep(10000);
                string title = Test2Class.GetActiveWindow();

                if (String.IsNullOrEmpty(title))
                {
                    Console.WriteLine("No window detected");
                    continue;
                }

                Console.WriteLine($"Detected window {title}");
            }
        }
    }
}
