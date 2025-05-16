using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssignmentLibrary.Core.Interfaces;

namespace AssignmentLibrary.UI
{
    public class ConsoleAppLogger : IAppLogger
    {
        public void Log(string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"[LOG {timestamp}] {message}");
        }
    }
}
