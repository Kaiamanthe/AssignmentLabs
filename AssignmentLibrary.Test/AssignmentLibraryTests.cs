using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Models;

namespace AssignmentLibrary.Tests;

public class AssignmentLibraryTests
{
    // Assignment.cs model test
    [Fact]
    public void Constructor_ValidInput_ShouldCreateAssignment()
    {
        // Arrange
        var assignment = new Assignment("Read Chapter 2", "Summarize key points", "Test Notes", false);

        // Assert
        Assert.Equal("Read Chapter 2", assignment.Title);
        Assert.Equal("Summarize key points", assignment.Description);
    }

    [Fact]
    public void Constructor_BlankTitle_ShouldThrowException()
    {
        // Arrange
        var description = "Valid description";
        var notes = "Test Notes";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Assignment("", description, notes, false));

        // Assert
        Assert.Contains("Title cannot be blank", exception.Message);
    }

    [Fact]
    public void Update_ShouldThrow_IfDescIsBlank()
    {
        // Arrange
        var assignment = new Assignment("Read Chapter 2", "Summarize Key Points", "Test Notes", false, Priority.Medium);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            assignment.UpdateAssignment("Read Chapter 3", "", "Some Notes", false, Priority.Medium)
        );
    }

    [Fact]
    public void UpdateAssignment_ShouldUpdateAllFieldsCorrectly()
    {
        // Arrange
        var assignment = new Assignment("Starting Title", "Starting Description", "Starting Notes", false, Priority.Medium);

        // Act
        assignment.UpdateAssignment(
            newTitle: "New Title",
            newDescription: "New Description",
            notes: "New Notes",
            newCompletion: true,
            priority: Priority.High
        );

        // Assert
        Assert.Equal("New Title", assignment.Title);
        Assert.Equal("New Description", assignment.Description);
        Assert.Equal("New Notes", assignment.Notes);
        Assert.True(assignment.IsCompleted);
        Assert.Equal(Priority.High, assignment.Priority);
    }

    [Fact]
    public void UpdateNote_ShouldChangeNoteContent()
    {
        // Arrange
        var assignment = new Assignment("Note Test", "Testing note", "Initial Note", false, Priority.Low);

        // Act
        assignment.UpdateNote("New Note");

        // Assert
        Assert.Equal("New Note", assignment.Notes);
    }

    [Fact]
    public void UpdatePriority_PriorityShouldChange()
    {
        // Arrange
        var assignment = new Assignment("Priority Test", "Change priority", "Notes", false, Priority.Low);

        // Act
        var result = assignment.UpdatePriority(Priority.High);

        // Assert
        Assert.True(result);
        Assert.Equal(Priority.High, assignment.Priority);
    }

    [Fact]
    public void UpdatePriority_ShouldReturnFalse_WhenPriorityIsSame()
    {
        // Arrange
        var assignment = new Assignment("No Change", "Keep same priority", "Notes", false, Priority.Medium);

        // Act
        var result = assignment.UpdatePriority(Priority.Medium);

        // Assert
        Assert.False(result);
        Assert.Equal(Priority.Medium, assignment.Priority);
    }

    [Fact]
    public void MarkComplete_SetsIsCompletedToTrue()
    {
        // Arrange
        var assignment = new Assignment("Task", "Complete the lab", "Test Notes", false);

        // Act
        assignment.MarkComplete();

        // Assert
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
