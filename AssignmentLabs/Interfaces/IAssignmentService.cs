using AssignmentLibrary.Core.Models;

namespace AssignmentLibrary.Core.Interfaces
{
    public interface IAssignmentService
    {
        bool AddAssignment(Assignment assignment);
        List<Assignment> ListAll();
        List<Assignment> ListIncomplete();
        List<Assignment> ListAssignmentsByPriority();
        Assignment FindAssignmentByTitle(string title);
        bool MarkAssignmentComplete(string title);
        bool DeleteAssignment(string title);
        bool UpdateAssignment(string originalTitle, string newTitle, string newDescription, string notes, bool isCompleted, Priority priority);
        bool UpdateNote(string title, string note);
    }

}
