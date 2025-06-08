namespace AssignmentLibrary.Core.Interfaces
{
    /// <summary>
    /// Defines a logging abstraction for logging application messages.
    /// </summary>
    public interface IAppLogger
    {
        /// <summary>
        /// Logs the specified message to the configured output target.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void Log(string message);
    }
}
