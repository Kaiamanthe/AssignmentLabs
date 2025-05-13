using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentLibrary.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly List<Assignment> assignments = new();

        public bool AddAssignment(Assignment assignment)
        {
            if (assignments.Any(a => a.Title.Equals(assignment.Title, StringComparison.OrdinalIgnoreCase)))
            {
                return false; // Duplicate title exists
            }

            assignments.Add(assignment);
            return true;
        }
        public List<Assignment> ListAll()
        {
            return new List<Assignment>(assignments);
        }

        public List<Assignment> ListIncomplete()
        {
            return assignments.Where(a => !a.IsCompleted).ToList();
        }

        public Assignment FindAssignmentByTitle(string title)
        {
            return assignments.FirstOrDefault(a =>
                a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
        public bool MarkAssignmentComplete(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment == null)
                return false;

            assignment.MarkComplete();
            return true;
        }
        public bool DeleteAssignment(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment == null)
                return false;

            assignments.Remove(assignment);
            return true;
        }

        public bool UpdateAssignment(string oldTitle, string newTitle, string newDescription)
        {
            var assignment = FindAssignmentByTitle(oldTitle);
            if (assignment == null)
                return false;

            if (!oldTitle.Equals(newTitle, StringComparison.OrdinalIgnoreCase) &&
                assignments.Any(a => a.Title.Equals(newTitle, StringComparison.OrdinalIgnoreCase)))
            {
                return false; // Conflict
            }

            assignment.Update(newTitle, newDescription);
            return true;
        }

        public Assignment? FindByTitle(string title)
        {
            return assignments.FirstOrDefault(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

    }
}
