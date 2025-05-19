namespace AssignmentLibrary.Core.Models;

public class Assignment
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; } = false;

    public Assignment(string title, string description, bool iscomplete)
    {

        ValHelper(title, description, iscomplete);
        Title = title;
        Description = description;

    }

    public void Update(string newTitle, string newDescription, bool newcompletion)
    {
        ValHelper(newTitle, newDescription, newcompletion);

        Title = newTitle;
        Description = newDescription;
        IsCompleted = newcompletion;
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
