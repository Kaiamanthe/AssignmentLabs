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
                Console.WriteLine("2. List All Assignments");
                Console.WriteLine("3. List Incomplete Assignments");
                Console.WriteLine("4. List Assignments By Priority");
                Console.WriteLine("5. Mark Assignment as Complete");
                Console.WriteLine("6. Search Assignment by Title");
                Console.WriteLine("7. Update Assignment");
                Console.WriteLine("8. Delete Assignment");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddAssignment();
                        break;
                    case "2":
                        ListAllAssignments();
                        break;
                    case "3":
                        ListIncompleteAssignments();
                        break;
                    case "4":
                        ListAssignmentsByPriority();
                        break;
                    case "5":
                        MarkAssignmentComplete();
                        break;
                    case "6":
                        SearchAssignmentByTitle();
                        break;
                    case "7":
                        UpdateAssignment();
                        break;
                    case "8":
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
            Console.WriteLine("Enter Priority: (L)ow, (M)edium, or (H)igh");
            var priorityInput = Console.ReadLine()?.ToUpper();

            try
            {
                Priority priority = ConvertToPriority(priorityInput);

                var assignment = new Assignment(title, description, false, priority);
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
                Console.WriteLine($"- {assignment.Title}: {assignment.Description} (Completed: {assignment.IsCompleted})");
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
                Console.WriteLine($"- {assignment.Title}: {assignment.Description} (Completed: {assignment.IsCompleted})");
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
                Console.WriteLine($"Priority: {assignment.Priority.ToString()} - {assignment.Title}: {assignment.Description}");
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
                Console.WriteLine($"Found: {assignment.Title}: {assignment.Description} (Completed: {assignment.IsCompleted})");
            }
        }
        private void UpdateAssignment()
        {
            Console.WriteLine("Enter the title of the assignment to update:");
            var oldTitle = Console.ReadLine();
            Console.Write("Enter new title: ");
            var newTitle = Console.ReadLine();
            Console.Write("Enter new description: ");
            var newDescription = Console.ReadLine();
            Console.WriteLine("Enter Priority: (L)ow, (M)edium, or (H)igh");
            var priorityInput = Console.ReadLine()?.ToUpper();

            try
            {
                Priority priority = ConvertToPriority(priorityInput);
                if (_assignmentService.UpdateAssignment(oldTitle, newTitle, newDescription, priority))
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
