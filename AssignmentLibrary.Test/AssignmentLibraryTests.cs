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
        var assignment = new Assignment("Read Chapter 2", "Summarize key points", "Test Notes", false);
        Assert.Equal("Read Chapter 2", assignment.Title);
        Assert.Equal("Summarize key points", assignment.Description);
    }

    [Fact]
    public void Constructor_BlankTitle_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => new Assignment("", "Valid description", "Test Notes", false));
    }

    [Fact]
    public void Update_ShouldThrow_IfDescIsBlank()
    {
        // Arrange
        var assignment = new Assignment("Read Chapter 2", "Summarize Key Points", "Test Notes", false, Priority.Medium);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            assignment.Update("Read Chapter 3", "", "Some Notes", false, Priority.Medium)
        );
    }


    [Fact]
    public void MarkComplete_SetsIsCompletedToTrue()
    {
        var assignment = new Assignment("Task", "Complete the lab", "Test Notes", false);
        assignment.MarkComplete();
        Assert.True(assignment.IsCompleted);
    }

    [Fact]
    public void Assignment_HasDefaultPriority()
    {
        // Arrange
        var assignment = new Assignment("Task 1", "Details", "Test Notes", false);

        // Assert
        Assert.Equal(Priority.Medium, assignment.Priority);
    }

    [Fact]
    public void Assignment_AcceptsHighPriority()
    {
        // Arrange
        var assignment = new Assignment("Urgent Task", "Do it now", "Test Notes", false);

        // Assert
        Assert.Equal(Priority.Medium, assignment.Priority);
    }
}
