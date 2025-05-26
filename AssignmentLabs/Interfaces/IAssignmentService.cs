using AssignmentLibrary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentLibrary.Core.Interfaces
{
    public interface IAssignmentService
    {
        public bool AddAssignment(Assignment assignment);
        public List<Assignment> ListAll();
        public List<Assignment> ListIncomplete();
        public List<Assignment> ListAssignmentsByPriority();
        public Assignment FindAssignmentByTitle(string title);
        public bool MarkAssignmentComplete(string title);
        public bool DeleteAssignment(string title);
        bool UpdateAssignment(string originalTitle, string newTitle, string newDescription, string notes, bool isCompleted, Priority priority);
        Assignment? FindByTitle(string title);
    }
}
