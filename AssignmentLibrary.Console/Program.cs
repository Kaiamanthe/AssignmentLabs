using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Services;
using AssignmentLibrary.UI;
using AssignmentLibrary.UI.UiUtilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AssignmentLibrary.Console
{
    internal class Program
    {
        public static void Main(string[] args) // 🔥 Static Main method — this is required
        {
            var services = new ServiceCollection();

            services.AddSingleton<IAssignmentService, AssignmentService>();
            services.AddSingleton<IAppLogger, ConsoleAppLogger>();
            services.AddSingleton<IAssignmentFormatter, AssignmentFormatter>();
            services.AddSingleton<ConsoleUI>();

            var serviceProvider = services.BuildServiceProvider();
            var consoleUI = serviceProvider.GetRequiredService<ConsoleUI>();

            consoleUI.Run();
        }
    }
}
