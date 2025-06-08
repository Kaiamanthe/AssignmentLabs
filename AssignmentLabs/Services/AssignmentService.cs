using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;


namespace AssignmentLibrary.Core.Services
{
    /// <summary>
    /// Provides services for managing assignment operations such as adding, updating, deleting, and retrieving assignments.
    /// </summary>
    public class AssignmentService : IAssignmentService
    {
        private readonly IAppLogger _logger;
        private readonly IAssignmentFormatter _formatter;
        private readonly List<Assignment> assignments = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssignmentService"/> class.
        /// </summary>
        /// <param name="formatter">Formatter for displaying assignment information.</param>
        /// <param name="logger">Logger for tracking assignment operations.</param>
        public AssignmentService(IAssignmentFormatter formatter, IAppLogger logger)
        {
            _logger = logger;
            _formatter = formatter;
        }

        /// <inheritdoc/>
        public bool AddAssignment(Assignment assignment)
        {
            if (assignments.Any(a => a.Title.Equals(assignment.Title, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.Log($"Add failed (duplicate title: {assignment.Title})");
                return false;
            }

            assignments.Add(assignment);
            _logger.Log($"Added: {_formatter.Format(assignment)}");
            return true;
        }

        /// <inheritdoc/>
        public List<Assignment> ListAll() => assignments;

        /// <inheritdoc/>
        public List<Assignment> ListIncomplete() =>
            assignments.Where(a => !a.IsCompleted).ToList();

        /// <inheritdoc/>
        public List<Assignment> ListAssignmentsByPriority() =>
            assignments.OrderByDescending(a => a.Priority).ToList();

        /// <inheritdoc/>
        public Assignment FindAssignmentByTitle(string title) =>
            assignments.FirstOrDefault(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        /// <inheritdoc/>
        public bool MarkAssignmentComplete(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment == null) return false;

            assignment.MarkComplete();
            _logger.Log($"Marked complete: {_formatter.Format(assignment)}");
            return true;
        }

        /// <inheritdoc/>
        public bool DeleteAssignment(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment == null) return false;

            assignments.Remove(assignment);
            _logger.Log($"Deleted: {_formatter.Format(assignment)}");
            return true;
        }

        /// <inheritdoc/>
        public bool UpdateAssignment(string oldTitle, string newTitle, string newDescription, string notes, bool isComplete, Priority priority)
        {
            var assignment = FindAssignmentByTitle(oldTitle);
            if (assignment == null)
            {
                _logger.Log($"Update failed: '{oldTitle}' not found.");
                return false;
            }

            if (!string.Equals(oldTitle, newTitle, StringComparison.OrdinalIgnoreCase) &&
                assignments.Any(a => a.Title.Equals(newTitle, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.Log($"Update failed: Duplicate title '{newTitle}' exists.");
                return false;
            }

            assignment.UpdateAssignment(newTitle, newDescription, notes, isComplete, priority);
            _logger.Log($"Updated: {_formatter.Format(assignment)}");
            return true;
        }

        /// <inheritdoc/>
        public bool UpdateNote(string title, string note)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment == null)
            {
                _logger.Log($"Note update failed: '{title}' not found.");
                return false;
            }

            assignment.UpdateNote(note);
            _logger.Log($"Note updated: {_formatter.Format(assignment)}");
            return true;
        }
    }
}
