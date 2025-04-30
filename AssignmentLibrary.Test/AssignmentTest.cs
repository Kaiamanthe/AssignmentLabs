using Xunit;
using AssignmentLibrary.Core;
using System.Reflection;

namespace AssignmentLibrary.Tests;

public class AssignmentTests
{
    [Fact]
    public void Constructor_ValidInput_ShouldCreateAssignment()
    {
        var assignment = new Assignment("Read Chapter 2", "Summarize key points");
        Assert.Equal("Read Chapter 2", assignment.Title);
        Assert.Equal("Summarize key points", assignment.Description);
    }

    [Fact]
    public void Constructor_BlankTitle_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => new Assignment("", "Valid description"));
    }

    [Fact]
    public void Update_ShouldThrow_IfDescIsBlank()
    {
        //Arrange
        var assignment = new Assignment("Read Chapter 2", "Summarize Key Points");

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            assignment.Update("Read Chapter 3", "")
        ); //Looks to see if the description is blank
    }

    [Fact]
    public void MarkComplete_SetsIsCompletedToTrue()
    {
        var assignment = new Assignment("Task", "Complete the lab", false);
        assignment.MarkComplete();
        Assert.True(assignment.IsCompleted);
    }




}
