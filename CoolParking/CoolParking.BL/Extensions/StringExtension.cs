using System;

namespace CoolParking.BL.Extensions
{
    public static class StringExtension
    {
        private static Random rnd = new Random();

        /// <summary>
        /// Generates specific count of random uppercase letters
        /// </summary>
        /// <param name="lettersCount">Count of letters to generate</param>
        /// <returns>New string with specific count of randomly generated uppercase letters</returns>
        public static string GenerateRandomUppercaseLetters(this string temp, int lettersCount)
        {
            string str = "";
            for (int i = 0; i < lettersCount; i++) str += (char)rnd.Next('A', 'Z' + 1);
            return str;
        }

        /// <summary>
        /// Generates specific count of random digits
        /// </summary>
        /// <param name="lettersCount">Count of digits to generate</param>
        /// <returns>New string with specific count of randomly generated digits</returns>
        public static string GenerateRandomInts(this string temp, int numbersCount)
        {
            string str = "";
            for (int i = 0; i < numbersCount; i++) str += rnd.Next(0, 10);
            return str;
        }
    }
}