﻿namespace AssignmentLibrary.Core.Models;

public class Assignment
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Notes { get; set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public Priority Priority { get; private set; }

    public Assignment()
    {
        // Parameterless constructor for serialization or other purposes
    }

    public Assignment(string title, string description, string note, bool isCompleted = false, Priority priority = Priority.Medium)
    {
        ValHelper(title, description);
        Title = title;
        Description = description;
        Notes = note;
        IsCompleted = isCompleted;
        Priority = priority;
    }


    public void Update(string newTitle,  string newDescription, string notes, bool newCompletion, Priority priority)
    {
        ValHelper(newTitle, newDescription);
        Title = newTitle;
        Description = newDescription;
        IsCompleted = newCompletion;
        Priority = priority;
        Notes = notes ?? string.Empty;
    }


    public bool UpdatePriority(Priority newPriority)
    {
        if (Priority == newPriority)
            return false;
        Priority = newPriority;
        return true;
    }

    private void Validate(string input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException($"{fieldName} cannot be blank or whitespace.");
    }

    private void ValHelper(string titleField, string descriptionField)
    {
        Validate(titleField, nameof(Title));
        Validate(descriptionField, nameof(Description));
    }

    public void MarkComplete()
    {
        IsCompleted = true;
    }
}
