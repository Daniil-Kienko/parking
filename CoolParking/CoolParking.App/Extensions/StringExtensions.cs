namespace CoolParking.App.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns string containing first parameter and array given as second parameter
        /// in stringified version looking like list of options with numerations
        /// </summary>
        /// <param name="str">Label</param>
        /// <param name="arr">Array for creating options</param>
        /// <returns>String containing first parameter and array given as second parameter
        /// in stringified version looking like list of options with numerations</returns>
        public static string InsertArrayWithNumeration(this string str, string [] arr)
        {
            string temp = str + " (\n";
            for (int i = 0; i < arr.Length; i++)
            {
                temp += $"{arr[i]} - {i + 1}\n";
                if (i == arr.Length - 1) temp += "): ";
            }
            return temp;
        }
    }
}
