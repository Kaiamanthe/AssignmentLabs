using System;

namespace AssignmentLibrary.UI.UiUtilities
{
    /// <summary>
    /// Provides static helper methods for console input/output interactions.
    /// </summary>
    public static class CustomConsole
    {
        /// <summary>
        /// Prompts the user and reads input on the same line.
        /// </summary>
        /// <param name="prompt">The prompt display before input.</param>
        /// <returns>User input as a string. Never returns null.</returns>
        public static string Input(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        /// <summary>
        /// Prompts the user and reads input on the next line.
        /// </summary>
        /// <param name="prompt">The prompt to display above the input line.</param>
        /// <returns>User input as a string. Never returns null.</returns>
        public static string InputLine(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        /// <summary>
        /// Displays a message to the console.
        /// </summary>
        /// <param name="message">The message to display.</param>
        public static void Output(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes a blank line to the console.
        /// </summary>
        public static void BlankLine()
        {
            Console.WriteLine();
        }
    }
}

