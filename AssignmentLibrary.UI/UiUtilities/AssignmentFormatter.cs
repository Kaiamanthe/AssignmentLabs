using System;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;

namespace AssignmentLibrary.UI.UiUtilities
{
    /// <summary>
    /// Provides string formatter for <see cref="Assignment"/> objects.
    /// </summary>
    public class AssignmentFormatter : IAssignmentFormatter
    {
        /// <summary>
        /// Returns a string representation of <see cref="Assignment"/>.
        /// </summary>
        /// <param name="assignment">Assignment to format.</param>
        /// <returns>A formatted string with ID, title, and description.</returns>
        public string Format(Assignment assignment)
        {
            return $"Assignment ID: {assignment.Id}, Title: {assignment.Title}, Description: {assignment.Description}";
        }
    }
}
