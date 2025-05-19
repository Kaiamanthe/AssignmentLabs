using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;


namespace AssignmentLibrary.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAppLogger _logger;
        private readonly IAssignmentFormatter _formatter;
        private readonly List<Assignment> assignments = new();

        public AssignmentService(IAssignmentFormatter formatter, IAppLogger logger)
        {
            _logger = logger;
            _formatter = formatter;
        }

        public bool AddAssignment(Assignment assignment)
        {
            if (assignments.Any(a => a.Title.Equals(assignment.Title, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.Log($"Add failed (duplicate title: {assignment.Title}");
                return false; // Duplicate title exists
            }

            assignments.Add(assignment);
            _logger.Log($"Added: {_formatter.Format(assignment)}");
            return true;
        }
        public List<Assignment> ListAll()
        {
            _logger.Log("Retrieved all assignments.");
            return new List<Assignment>(assignments);
        }

        public List<Assignment> ListIncomplete()
        {
            _logger.Log("Retrieved all incomplete assignment.");
            return assignments.Where(a => !a.IsCompleted).ToList();
        }

        public Assignment FindAssignmentByTitle(string title)
        {
            var assignment = assignments.FirstOrDefault(a =>
                a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            _logger.Log(assignment == null
                ? $"Find failed: '{title}' not found."
                : $"Found: {_formatter.Format(assignment)}");
            return assignment;
        }
        public bool MarkAssignmentComplete(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment == null)
            {
                _logger.Log($"Marked complete failed: '{title}' not found.");
                return false;
            }

            assignment.MarkComplete();
            _logger.Log($"Mark complete: {_formatter.Format(assignment)}");
            return true;
        }

        public bool DeleteAssignment(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment == null)
            {
                _logger.Log($"Delete failed: '{title}' not found.");
                return false;
            }

            assignments.Remove(assignment);
            _logger.Log($"Deleted: {_formatter.Format(assignment)}");
            return true;
        }


        public bool UpdateAssignment(string oldTitle, string newTitle, string newDescription)
        {
            var assignment = FindAssignmentByTitle(oldTitle);
            if (assignment == null)
            {
                _logger.Log($"Update failed: '{oldTitle}' not found.");
                return false;
            }

            if (!oldTitle.Equals(newTitle, StringComparison.OrdinalIgnoreCase) &&
                assignments.Any(a => a.Title.Equals(newTitle, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.Log($"Update failed: new title '{newTitle}' conflicts with existing assignment.");
                return false;
            }

            bool isCompleted = assignment.IsCompleted;

            assignment.Update(newTitle, newDescription, isCompleted);
            _logger.Log($"Updated: {_formatter.Format(assignment)}");
            return true;
        }

        public Assignment? FindByTitle(string title)
        {
            var assignment = assignments.FirstOrDefault(a =>
                a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            _logger.Log(assignment == null
                ? $"FindByTitle failed: '{title}' not found."
                : $"Found by title: {_formatter.Format(assignment)}");
            return assignment;
        }

    }
}
