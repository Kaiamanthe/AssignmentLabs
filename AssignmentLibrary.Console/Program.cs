using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Services;
using AssignmentLibrary.UI;
using AssignmentLibrary.UI.UiUtilities;
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentLibrary.Console
{
    /// <summary>
    /// Entry point for AssignmentLibrary Console.
    /// Responsible for registering services and launching the Console UI.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method initializes application.
        /// </summary>
        /// <param name="args">Command-line arguments (not used).</param>
        public static void Main(string[] args)
        {
            // Set up the dependency injection container
            var services = new ServiceCollection();

            /// <summary>
            /// Register core services as singletons:
            /// - AssignmentService: Business logic and in-memory storage
            /// - ConsoleAppLogger: Outputs logs to the console
            /// - AssignmentFormatter: Formats assignments for display
            /// - ConsoleUI: Console-based interface for user interaction
            /// </summary>
            services.AddSingleton<IAssignmentService, AssignmentService>();
            services.AddSingleton<IAppLogger, ConsoleAppLogger>();
            services.AddSingleton<IAssignmentFormatter, AssignmentFormatter>();
            services.AddSingleton<ConsoleUI>();

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // Resolve the Console UI and run it
            var consoleUI = serviceProvider.GetRequiredService<ConsoleUI>();
            consoleUI.Run();
        }
    }
}
