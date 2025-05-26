using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;

namespace AssignmentLibrary.UI
{
    public class ConsoleUI
    {
        private readonly IAssignmentService _assignmentService;

        public ConsoleUI(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nAssignment Manager Menu:");
                Console.WriteLine("1. Add Assignment");
                Console.WriteLine("2. Add Notes to Assignment");
                Console.WriteLine("3. List All Assignments");
                Console.WriteLine("4. List Incomplete Assignments");
                Console.WriteLine("5. List Assignments By Priority");
                Console.WriteLine("6. Mark Assignment as Complete");
                Console.WriteLine("7. Search Assignment by Title");
                Console.WriteLine("8. Update Assignment");
                Console.WriteLine("9. Delete Assignment");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddAssignment();
                        break;
                    case "2":
                        AddNoteToAssignment();
                        break;
                    case "3":
                        ListAllAssignments();
                        break;
                    case "4":
                        ListIncompleteAssignments();
                        break;
                    case "5":
                        ListAssignmentsByPriority();
                        break;
                    case "6":
                        MarkAssignmentComplete();
                        break;
                    case "7":
                        SearchAssignmentByTitle();
                        break;
                    case "8":
                        UpdateAssignment();
                        break;
                    case "9":
                        DeleteAssignment();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

            }
        }
        
        private Priority ConvertToPriority(string priorityInput)
        {
            return priorityInput switch
            {
                "L" => Priority.Low,
                "M" => Priority.Medium,
                "H" => Priority.High,
                _ => throw new ArgumentException("Invalid priority input. Use L, M, or H.")
            };
        }
        private void AddAssignment()
        {
            Console.WriteLine("Enter assignment title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter assignment description: ");
            var description = Console.ReadLine();
            Console.WriteLine("Enter assignment notes (optional [enter] to leave blank): ");
            var notes = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Enter Priority: (L)ow, (M)edium, or (H)igh");
            var priorityInput = Console.ReadLine()?.ToUpper();

            try
            {
                Priority priority = ConvertToPriority(priorityInput);

                var assignment = new Assignment(title, description, notes, false, priority);
                if (_assignmentService.AddAssignment(assignment))
                {
                    Console.WriteLine("Assignment added successfully.");
                }
                else
                {
                    Console.WriteLine("An assignment with this title already exists.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private void AddNoteToAssignment()
        {
            Console.Write("What assignment do you want to add a note to? (Enter assignment title): ");
            var title = Console.ReadLine();

            var assignment = _assignmentService.FindAssignmentByTitle(title);
            if (assignment == null)
            {
                Console.WriteLine("No assignment found with that title. Returning to main menu.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(assignment.Notes))
            {
                Console.Write("There's already a note in the assignment. Update the note? (Y/N): ");
                var response = Console.ReadLine()?.Trim().ToUpper();

                if (response != "Y")
                {
                    Console.WriteLine("Note not updated. Returning to main menu.");
                    return;
                }
            }

            Console.Write("Enter the note content: ");
            var newNote = Console.ReadLine() ?? string.Empty;

            assignment.Update(assignment.Title, assignment.Description, newNote, assignment.IsCompleted, assignment.Priority);
            Console.WriteLine($"Note updated - Assignment: {assignment.Title} Description: {assignment.Description}{(string.IsNullOrWhiteSpace(assignment.Notes) ? "" : $" | Notes: {assignment.Notes}")} | Priority: {assignment.Priority} | Completed: {assignment.IsCompleted}");
        }
        private void ListAllAssignments()
        {
            var assignments = _assignmentService.ListAll();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
                Console.WriteLine($"Assignment: {assignment.Title} Description: {assignment.Description}{(string.IsNullOrWhiteSpace(assignment.Notes) ? "" : $" | Notes: {assignment.Notes}")} | Priority: {assignment.Priority} | Completed: {assignment.IsCompleted}");
            }
        }
        private void ListIncompleteAssignments()
        {
            var assignments = _assignmentService.ListIncomplete();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No incomplete assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
                Console.WriteLine($"Assignment: {assignment.Title} Description: {assignment.Description}{(string.IsNullOrWhiteSpace(assignment.Notes) ? "" : $" | Notes: {assignment.Notes}")} | Priority: {assignment.Priority} | Completed: {assignment.IsCompleted}");
            }
        }
        private void ListAssignmentsByPriority()
        {
            var assignments = _assignmentService.ListAssignmentsByPriority();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
                Console.WriteLine($"Assignment: {assignment.Title} Description: {assignment.Description}{(string.IsNullOrWhiteSpace(assignment.Notes) ? "" : $" | Notes: {assignment.Notes}")} | Priority: {assignment.Priority} | Completed: {assignment.IsCompleted}");
            }
        }
        private void MarkAssignmentComplete()
        {
            Console.Write("Enter the title of the assignment to mark complete: ");
            var title = Console.ReadLine();
            if (_assignmentService.MarkAssignmentComplete(title))
            {
                Console.WriteLine("Assignment marked as complete.");
            }
            else
            {
                Console.WriteLine("Assignment not found.");
            }
        }
        private void SearchAssignmentByTitle()
        {
            Console.Write("Enter the title to search: ");
            var title = Console.ReadLine();
            var assignment = _assignmentService.FindAssignmentByTitle(title);

            if (assignment == null)
            {
                Console.WriteLine("Assignment not found.");
            }
            else
            {
                Console.WriteLine($"Assignment: {assignment.Title} Description: {assignment.Description}{(string.IsNullOrWhiteSpace(assignment.Notes) ? "" : $" | Notes: {assignment.Notes}")} | Priority: {assignment.Priority} | Completed: {assignment.IsCompleted}");
            }
        }
        private void UpdateAssignment()
        {
            Console.WriteLine("Enter the title of the assignment to update:");
            var oldTitle = Console.ReadLine();

            Console.WriteLine("Enter new title:");
            var newTitle = Console.ReadLine();

            Console.WriteLine("Enter new description:");
            var newDescription = Console.ReadLine();

            Console.WriteLine("Enter assignment notes (optional [enter] to leave blank): ");
            var notes = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Is the assignment complete?: (T)rue or (F)alse");
            string completetionString = Console.ReadLine();
            bool isCompleted = false;
            if (completetionString?.ToUpper() == "T" || completetionString?.ToUpper() == "TRUE")
            {
                isCompleted = true;
            }

            Console.WriteLine("Enter Priority: (L)ow, (M)edium, or (H)igh");
            var priorityInput = Console.ReadLine()?.ToUpper();

            try
            {
                Priority priority = ConvertToPriority(priorityInput);
                if (_assignmentService.UpdateAssignment(oldTitle, newTitle, newDescription, notes, isCompleted, priority))
                {
                    Console.WriteLine("Assignment updated successfully.");
                }
                else
                {
                    Console.WriteLine("Assignment not found or update failed.");
                }
            }
            catch
            {
                Console.WriteLine("Error: Invalid input. Please try again.");
            }
        }
        private void DeleteAssignment()
        {
            Console.Write("Enter the title of the assignment to delete: ");
            var title = Console.ReadLine();
            if (_assignmentService.DeleteAssignment(title))
            {
                Console.WriteLine("Assignment deleted successfully.");
            }
            else
            {
                Console.WriteLine("Assignment not found.");
            }
        }
    }
}
