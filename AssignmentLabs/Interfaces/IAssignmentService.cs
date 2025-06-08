using AssignmentLibrary.Core.Models;

namespace AssignmentLibrary.Core.Interfaces
{
    /// <summary>
    /// Provides operations for managing assignment service.
    /// </summary>
    public interface IAssignmentService
    {
        /// <summary>
        /// Adds a new assignment if its title is unique.
        /// </summary>
        /// <param name="assignment">The assignment to add.</param>
        /// <returns>True if added successfully; false if a duplicate exists.</returns>
        bool AddAssignment(Assignment assignment);

        /// <summary>
        /// Retrieves all assignments.
        /// </summary>
        /// <returns>List of all assignments.</returns>
        List<Assignment> ListAll();

        /// <summary>
        /// Retrieves all incomplete assignments.
        /// </summary>
        /// <returns>Lists incomplete assignments.</returns>
        List<Assignment> ListIncomplete();

        /// <summary>
        /// Retrieves assignments ordered by priority (descending).
        /// </summary>
        /// <returns>A prioritized list of assignments.</returns>
        List<Assignment> ListAssignmentsByPriority();

        /// <summary>
        /// Finds an assignment by title.
        /// </summary>
        /// <param name="title">Title of the assignment.</param>
        /// <returns>Found assignment; otherwise null.</returns>
        Assignment FindAssignmentByTitle(string title);

        /// <summary>
        /// Marks assignment as complete.
        /// </summary>
        /// <param name="title">Title of the assignment to mark complete.</param>
        /// <returns>True if successful; otherwise false.</returns>
        bool MarkAssignmentComplete(string title);

        /// <summary>
        /// Deletes target assignment.
        /// </summary>
        /// <param name="title">Title of the assignment to delete.</param>
        /// <returns>True if deleted; otherwise false.</returns>
        bool DeleteAssignment(string title);

        /// <summary>
        /// Updates the details of assignment.
        /// </summary>
        /// <param name="originalTitle">Title to search.</param>
        /// <param name="newTitle">New title to apply.</param>
        /// <param name="newDescription">New description to apply.</param>
        /// <param name="notes">Updated notes.</param>
        /// <param name="isCompleted">Updated completion.</param>
        /// <param name="priority">Updated priority.</param>
        /// <returns>True if updated; false if not found or duplicate title exists.</returns>
        bool UpdateAssignment(string originalTitle, string newTitle, string newDescription, string notes, bool isCompleted, Priority priority);

        /// <summary>
        /// Updates note of assignment.
        /// </summary>
        /// <param name="title">Title to find assignment.</param>
        /// <param name="note">The new note content.</param>
        /// <returns>True if updated successfully; otherwise false.</returns>
        bool UpdateNote(string title, string note);
    }
}

