using AssignmentLibrary.Core.Models;

namespace AssignmentLibrary.Core.Interfaces
{
    /// <summary>
    /// Provides formatting logic for assignments.
    /// </summary>
    public interface IAssignmentFormatter
    {
        /// <summary>
        /// Formats the given assignment into a string representation.
        /// </summary>
        /// <param name="assignment">The assignment to format.</param>
        /// <returns>A formatted string representing the assignment.</returns>
        string Format(Assignment assignment);
    }
}
