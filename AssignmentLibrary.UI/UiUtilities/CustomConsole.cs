using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentLibrary.UI.UiUtilities
{
    public static class CustomConsole
    {
        public static string Input(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        public static string InputLine(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        public static void Output(string message)
        {
            Console.WriteLine(message);
        }

        public static void BlankLine()
        {
            Console.WriteLine();
        }
    }
}
