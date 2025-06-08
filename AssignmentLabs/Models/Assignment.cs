namespace AssignmentLibrary.Core.Models;

/// <summary>
/// Represents an individual assignment with metadata such as title, description, notes, completion status, and priority.
/// </summary>
public class Assignment
{
    /// <summary>
    /// Unique identifier for the assignment.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Title of Assignment.
    /// </summary>
    public string Title { get; private set; } = default!;

    /// <summary>
    /// Description of the assignment.
    /// </summary>
    public string Description { get; private set; } = default!;

    /// <summary>
    /// Notes associated with the assignment.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Bool to indicate whether assignment is completed
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// Priority level of the assignment, attach to Priority enum file.
    /// </summary>
    public Priority Priority { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Assignment"/> class with the specified title, description, notes, completion status, and priority.
    /// </summary>
    /// <param name="title">Title of the assignment.</param>
    /// <param name="description">Description of the assignment.</param>
    /// <param name="note">Optional notes for the assignment.</param>
    /// <param name="isCompleted">Whether the assignment is completed. Defaults to false.</param>
    /// <param name="priority">Priority level of the assignment, attach to Priority enum file.. Defaults to Medium.</param>
    public Assignment(string title, string description, string note, bool isCompleted = false, Priority priority = Priority.Medium)
    {
        ValHelper(title, description);
        Title = title;
        Description = description;
        Notes = note;
        IsCompleted = isCompleted;
        Priority = priority;
    }

    /// <summary>
    /// Updates the assignment's title, description, notes, completion status, and priority.
    /// </summary>
    public void UpdateAssignment(string newTitle, string newDescription, string notes, bool newCompletion, Priority priority)
    {
        ValHelper(newTitle, newDescription);
        Title = newTitle;
        Description = newDescription;
        IsCompleted = newCompletion;
        Priority = priority;
        Notes = notes ?? string.Empty;
    }

    /// <summary>
    /// Updates the notes associated with the assignment.
    /// </summary>
    public void UpdateNote(string note)
    {
        Notes = note ?? string.Empty;
    }

    /// <summary>
    /// Updates the priority of the assignment if it differs from the current one.
    /// </summary>
    /// <param name="newPriority">The new priority level.</param>
    /// <returns>True if the priority was updated, otherwise false.</returns>
    public bool UpdatePriority(Priority newPriority)
    {
        if (Priority == newPriority)
            return false;
        Priority = newPriority;
        return true;
    }

    /// <summary>
    /// Marks the assignment as complete.
    /// </summary>
    public void MarkComplete()
    {
        IsCompleted = true;
    }

    /// <summary>
    /// Validates that the specified input is not null, empty, or whitespace.
    /// Throws an <see cref="ArgumentException"/> if the check fails.
    /// </summary>
    /// <param name="input">The string value to validate.</param>
    /// <param name="fieldName">The name of the field being validated (used in the error message).</param>
    private void Validate(string input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException($"{fieldName} cannot be blank or whitespace.");
    }

    /// <summary>
    /// Validates both the title and description fields of an assignment using <see cref="Validate"/>.
    /// </summary>
    /// <param name="titleField">The title of the assignment to validate.</param>
    /// <param name="descriptionField">The description of the assignment to validate.</param>
    private void ValHelper(string titleField, string descriptionField)
    {
        Validate(titleField, nameof(Title));
        Validate(descriptionField, nameof(Description));
    }
}
