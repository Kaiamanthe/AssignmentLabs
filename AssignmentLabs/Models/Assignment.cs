namespace AssignmentLibrary.Core.Models;

public class Assignment
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; } = false;
    public Priority Priority { get; private set; }
    

    public Assignment(string title, string description, bool iscomplete, Priority priority = Priority.Medium)
    {

        ValHelper(title, description, iscomplete);
        Title = title;
        Description = description;
        Priority = priority;

    }

    public void Update(string newTitle, string newDescription, bool newcompletion, Priority priority)
    {
        ValHelper(newTitle, newDescription, newcompletion);

        Title = newTitle;
        Description = newDescription;
        IsCompleted = newcompletion;
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

    private void ValHelper(string TitleField, string DescriptionField, bool IsCompletedField)
    {
        Validate(TitleField, nameof(Title));
        Validate(DescriptionField, nameof(Description));
        Validate(IsCompletedField.ToString(), nameof(IsCompleted));


        Title = TitleField;
        Description = DescriptionField;
        IsCompleted = IsCompletedField;
    }

    public void MarkComplete()
    {
        IsCompleted = true;
    }
}
