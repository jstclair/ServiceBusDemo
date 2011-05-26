using System;

namespace Utilities
{
    public static class ConsoleUtils
    {
        public static void WriteLine(string input)
        {
            WriteLine(Console.ForegroundColor, input);
        }

        public static void WriteLine(string format, params object[] arg)
        {
            WriteLine(Console.ForegroundColor, format, arg);
        }

        public static void WriteLine(ConsoleColor foreColor, string input)
        {
            ConsoleColor existing = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = foreColor;
                Console.WriteLine(input);
            }
            finally
            {
                Console.ForegroundColor = existing;
            }
        }

        public static void WriteLine(ConsoleColor foreColor, string format, params object[] arg)
        {
            ConsoleColor existing = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = foreColor;
                Console.WriteLine(format, arg);
            }
            finally
            {
                Console.ForegroundColor = existing;
            }
        }

        /// <summary>
        /// Blocking call!
        /// </summary>
        /// <param name="stringToMatch"></param>
        /// <returns></returns>
        public static bool UserHasNotEnteredQuitSequence(string stringToMatch)
        {
            return UserHasNotEnteredQuitSequence(Console.ReadLine(), stringToMatch);
        }

        /// <summary>
        /// Non-blocking call
        /// </summary>
        /// <param name="input"></param>
        /// <param name="stringToMatch"></param>
        /// <returns></returns>
        public static bool UserHasNotEnteredQuitSequence(string input, string stringToMatch)
        {
            return String.Compare(input, stringToMatch, StringComparison.OrdinalIgnoreCase) != 0;
        }
    }
}
