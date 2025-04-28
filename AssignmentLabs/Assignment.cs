namespace AssignmentLibrary;

public class Assignment
{
    public string Title { get; set; }
    public string Description { get; set; }

    public Assignment(string title, string description)
    {
        ValHelper(title, description);
    }

    public void Update(string newTitle, string newDescription)
    {
        ValHelper(newTitle, newDescription);
    }

    private void ValHelper(string TitleField, string DescriptionField)
    {
        Validate(TitleField, nameof(Title));
        Validate(DescriptionField, nameof(Description));


        Title = TitleField;
        Description = DescriptionField;
    }
    private void Validate(string input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException($"{fieldName} cannot be blank or whitespace.");
    }
}
