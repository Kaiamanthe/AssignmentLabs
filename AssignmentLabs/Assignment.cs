namespace AssignmentLibrary;

public class Assignment
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; } = false;

    public Assignment(string title, string description, bool completion)
    {
        ValHelper(title, description, completion);
    }

    public void Update(string newTitle, string newDescription, bool newcompletion)
    {
        ValHelper(newTitle, newDescription, newcompletion);
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

    private void Validate(string input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException($"{fieldName} cannot be blank or whitespace.");
    }
}
