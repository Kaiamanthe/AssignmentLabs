using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;
using AssignmentLibrary.UI.UiUtilities;

namespace AssignmentLibrary.UI
{
    /// <summary>
    /// Console-based user interface for managing assignments.
    /// Provides menu options for adding, updating, deleting, and listing assignments.
    /// </summary>
    public class ConsoleUI
    {
        private readonly IAssignmentService _assignmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleUI"/> class.
        /// </summary>
        /// <param name="assignmentService">Service for assignment operations.</param>
        public ConsoleUI(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        /// <summary>
        /// Starts the console menu loop for user interaction.
        /// </summary>
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
                        FindAssignmentByTitle();
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

        /// <summary>
        /// Prompts the user for assignment priority and converts the input into a <see cref="Priority"/> enum.
        /// </summary>
        /// <param name="priority">Returns resolved priority value.</param>
        /// <returns>Always returns true. Default input is Medium if input is not L or H.</returns>
        private bool PriorityHandler(out Priority priority)
        {
            var input = CustomConsole.InputLine("Enter Priority: (L)ow, (M)edium, or (H)igh [default M]:")
                .Trim()
                .ToUpper();

            priority = input switch
            {
                "L" => Priority.Low,
                "H" => Priority.High,
                _ => Priority.Medium
            };

            return true;
        }

        /// <summary>
        /// Prompts the user for assignment details and adds a new assignment.
        /// </summary>
        private void AddAssignment()
        {
            var title = CustomConsole.InputLine("Enter assignment title:");
            var description = CustomConsole.InputLine("Enter assignment description:");
            var notes = CustomConsole.InputLine("Enter assignment notes (optional [enter] to leave blank):");

            if (!PriorityHandler(out var priority))
                return;

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

        /// <summary>
        /// Prompts the user for an assignment title and adds or updates its note.
        /// </summary>
        private void AddNoteToAssignment()
        {
            var title = CustomConsole.InputLine("What assignment do you want to add a note to? (Enter assignment title):");
            var assignment = _assignmentService.FindAssignmentByTitle(title);

            if (assignment == null)
            {
                Console.WriteLine("No assignment found with that title. Returning to main menu.");
                return;
            }

            if (!UpdateNotePrompt(assignment, out var newNote))
                return;

            if (_assignmentService.UpdateNote(title, newNote))
            {
                Console.WriteLine($"Note updated - Assignment: {assignment.Title}");
            }
            else
            {
                Console.WriteLine("Failed to update note.");
            }
        }

        /// <summary>
        /// Displays all assignments.
        /// </summary>
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

        /// <summary>
        /// Displays all incomplete assignments.
        /// </summary>
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

        /// <summary>
        /// Displays all assignments by priority (descending).
        /// </summary>
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

        /// <summary>
        /// Marks assignment as complete by title user input.
        /// </summary>
        private void MarkAssignmentComplete()
        {
            var title = CustomConsole.InputLine("Enter the title of the assignment to mark complete: ");
            if (_assignmentService.MarkAssignmentComplete(title))
            {
                Console.WriteLine("Assignment marked as complete.");
            }
            else
            {
                Console.WriteLine("Assignment not found.");
            }
        }

        /// <summary>
        /// Searches for and displays an assignment by title.
        /// </summary>
        private void FindAssignmentByTitle()
        {
            var title = CustomConsole.InputLine("Enter title of assignment for search: ");
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

        /// <summary>
        /// Prompts the user to update an existing assignment.
        /// </summary>
        private void UpdateAssignment()
        {
            var oldTitle = CustomConsole.InputLine("Enter the title of the assignment to update: ");
            var assignment = _assignmentService.FindAssignmentByTitle(oldTitle);

            if (assignment == null)
            {
                Console.WriteLine("Assignment not found.");
                return;
            }

            var newTitle = CustomConsole.InputLine("Enter new title: ");
            var newDescription = CustomConsole.InputLine("Enter new description: ");

            if (!UpdateNotePrompt(assignment, out var newNote))
                return;

            var completionInput = CustomConsole.InputLine("Is the assignment complete?: (T)rue or (F)alse").ToUpper();
            bool isCompleted = completionInput == "T" || completionInput == "TRUE";

            if (!PriorityHandler(out var priority))
                return;

            if (_assignmentService.UpdateAssignment(oldTitle, newTitle, newDescription, newNote, isCompleted, priority))
            {
                Console.WriteLine("Assignment updated successfully.");
            }
            else
            {
                Console.WriteLine("Update failed.");
            }
        }

        /// <summary>
        /// Prompts the user to delete an assignment by title.
        /// </summary>
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

        /// <summary>
        /// Prompts the user to add or update the note of an assignment.
        /// </summary>
        /// <param name="assignment">The assignment to update.</param>
        /// <param name="updatedNote">The resulting note value.</param>
        /// <returns>Always returns true. If the user cancels update, returns the current note unchanged.</returns>
        private bool UpdateNotePrompt(Assignment assignment, out string updatedNote)
        {
            if (!string.IsNullOrWhiteSpace(assignment.Notes))
            {
                var response = CustomConsole.InputLine("There's already a note in the assignment. Update the note? (Y/N):")
                    .Trim().ToUpper();

                if (response != "Y")
                {
                    Console.WriteLine("Note not updated.");
                    updatedNote = assignment.Notes;
                    return true;
                }

                updatedNote = CustomConsole.InputLine("Enter a note (Press 'enter' to leave blank): ");
                return true;
            }

            Console.WriteLine("No note exists for this assignment.");
            updatedNote = CustomConsole.InputLine("Enter a note (Press 'enter' to leave blank): ");
            return true;
        }
    }
}
