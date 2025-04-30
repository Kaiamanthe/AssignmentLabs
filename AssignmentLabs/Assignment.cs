namespace AssignmentLibrary.Core;

public class Assignment
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }

    public Assignment(string title, string description)
    {
        Validate(title, nameof(title));
        Validate(description, nameof(description));

        Title = title;
        Description = description;
        IsCompleted = false;
    }
    public Assignment(string title, string description, bool completion)
    {
        ValHelper(title, description, completion);
    }
    public void Update(string newTitle, string newDescription)
    {
        Validate(newTitle, nameof(newTitle));
        Validate(newDescription, nameof(newDescription));

        Title = newTitle;
        Description = newDescription;
        IsCompleted = false;
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
