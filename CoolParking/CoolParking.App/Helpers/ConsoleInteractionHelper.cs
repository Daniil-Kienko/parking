using System;

namespace CoolParking.App.Helpers
{
    public static class ConsoleInteractionService
    {
        /// <summary>
        /// Write text with a red background to the console
        /// </summary>
        /// <param name="str">Text to write</param>
        public static void WriteError(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Write text with a green background to the console
        /// </summary>
        /// <param name="str">Text to write</param>
        public static void WriteSuccess(string str)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Ask the user for a digit in the specified range
        /// </summary>
        /// <param name="str">Question</param>
        /// <param name="from">Range start</param>
        /// <param name="to">Range end</param>
        /// <returns>User entered number in the specified range</returns>
        public static int GetIntInputInRange(string str, int from, int to)
        {
            while (true)
            {
                Console.Write(str);
                int choice;
                bool parsed = int.TryParse(Console.ReadLine(), out choice);
                if (parsed && choice >= from && choice <= to) return choice;
                Console.WriteLine("Your input is not valid");
                Console.WriteLine("Please, try again");
            }
        }

        /// <summary>
        /// Ask user to input Text
        /// </summary>
        /// <param name="str">Question</param>
        /// <returns>User entered text</returns>
        public static string GetStringInput(string str)
        {
            Console.Write(str);
            return Console.ReadLine();
        }

        /// <summary>
        /// Ask user to input Decimal
        /// </summary>
        /// <param name="str">Question</param>
        /// <returns>User entered Decimal</returns>
        public static decimal GetDecimalInput(string str)
        {
            while (true)
            {
                Console.Write(str);
                decimal balance;
                bool parsed = decimal.TryParse(Console.ReadLine(), out balance);
                if (parsed) return balance;
                Console.WriteLine("Your input is not valid");
                Console.WriteLine("Please, try again");
            }
        }
    }
}
