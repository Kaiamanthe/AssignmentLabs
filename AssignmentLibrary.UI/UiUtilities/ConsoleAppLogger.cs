using System;
using AssignmentLibrary.Core.Interfaces;

namespace AssignmentLibrary.UI.UiUtilities
{
    /// <summary>
    /// Logs messages to the console with timestamp.
    /// Implements <see cref="IAppLogger"/> interface.
    /// </summary>
    public class ConsoleAppLogger : IAppLogger
    {
        /// <summary>
        /// Logs the specified message to the console with a timestamp.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public void Log(string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"[LOG {timestamp}] {message}");
        }
    }
}
