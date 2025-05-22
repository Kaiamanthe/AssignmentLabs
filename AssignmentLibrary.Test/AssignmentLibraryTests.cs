using Xunit;
using Moq;
using System.Reflection;
using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Models;
using AssignmentLibrary.Core.Interfaces;

namespace AssignmentLibrary.Tests;

public class AssignmentLibraryTests
{
    [Fact]
    public void Constructor_ValidInput_ShouldCreateAssignment()
    {
        var assignment = new Assignment("Read Chapter 2", "Summarize key points", false);
        Assert.Equal("Read Chapter 2", assignment.Title);
        Assert.Equal("Summarize key points", assignment.Description);
    }

    [Fact]
    public void Constructor_BlankTitle_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => new Assignment("", "Valid description", false));
    }

    [Fact]
    public void Update_ShouldThrow_IfDescIsBlank()
    {
        //Arrange
        var assignment = new Assignment("Read Chapter 2", "Summarize Key Points", false, Priority.Medium);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            assignment.Update("Read Chapter 3", "", false, Priority.Medium)
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
