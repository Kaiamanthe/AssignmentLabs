namespace AssignmentLibrary.Tests
{
    using AssignmentLibrary.Core;
    using AssignmentLibrary.Core.Interfaces;
    using AssignmentLibrary.Core.Models;
    using AssignmentLibrary.Core.Services;
    using AssignmentLibrary.UI.UiUtilities;
    using Moq;
    using Xunit;

    public class AssignmentServiceTests
    {

        [Fact]
        public void AddAssignment_ShouldAddAssignmentToList()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment = new Assignment("Read Chapter 3", "Answer question 1-8", "Test Notes", false);

            // Act
            service.AddAssignment(assignment);
            var assignments = service.ListAll();

            // Assert
            Assert.Contains(assignment, assignments);
        }

        [Fact]
        public void AddAssignment_ShouldFail_WhenDuplicateTitleExists()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Formatted");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment1 = new Assignment("Test", "Desc 1", "Notes", false);
            var assignment2 = new Assignment("TEST", "Desc 2", "More Notes", false);

            service.AddAssignment(assignment1);

            // Act
            var result = service.AddAssignment(assignment2);
            var assignments = service.ListAll();

            // Assert
            Assert.False(result);
            Assert.Single(assignments);
            mockLogger.Verify(l => l.Log(It.Is<string>(msg => msg.Contains("duplicate title", StringComparison.OrdinalIgnoreCase))), Times.Once);
        }

        [Fact]
        public void ListAll_ShouldReturnAllAssignments()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment1 = new Assignment("As 1 Read Chapter 1", "Anotate chapter 1", "Test Notes", false);
            var assignment2 = new Assignment("Chapter 1 worksheet", "Answer question 1-9", "Test Notes", false);

            service.AddAssignment(assignment1);
            service.AddAssignment(assignment2);

            // Act
            var assignments = service.ListAll();

            // Assert
            Assert.Equal(2, assignments.Count);
            Assert.Contains(assignment1, assignments);
            Assert.Contains(assignment2, assignments);
        }

        [Fact]
        public void ListIncomplete_ShouldReturnOnlyIncompleteAssignments()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment1 = new Assignment("As 1 Read Chapter 1", "Anotate chapter 1", "Test Notes", false);
            var assignment2 = new Assignment("Chapter 1 worksheet", "Answer question 1-9", "Test Notes", false);


            // Act
            assignment2.MarkComplete();
            service.AddAssignment(assignment1);
            service.AddAssignment(assignment2);
            var incompleteAssignments = service.ListIncomplete();

            // Assert
            Assert.Single(incompleteAssignments);
            Assert.Contains(assignment1, incompleteAssignments);
            Assert.DoesNotContain(assignment2, incompleteAssignments);
        }

        [Fact]
        public void ListIncomplete_ShouldReturnEmptyList_NoAssignment()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);

            // Act
            var incompleteAssignments = service.ListIncomplete();

            // Assert
            Assert.Empty(incompleteAssignments);
        }

        [Fact]
        public void ListIncomplete_ShouldReturnOnlyIncomplete_WhenMixofInAndCompleted()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var incompleteAssignment = new Assignment("Chapter 1", "Annotate Chapter 1", "Test Notes", false);
            var completedAssignment = new Assignment("WorkSheet 1", "Do questions 1-12", "Test Notes", false);

            // Act
            completedAssignment.MarkComplete();
            service.AddAssignment(incompleteAssignment);
            service.AddAssignment(completedAssignment);

            // Assert
            Assert.Single(service.ListIncomplete());
            Assert.Contains(incompleteAssignment, service.ListIncomplete());
            Assert.DoesNotContain(completedAssignment, service.ListIncomplete());
        }

        [Fact]
        public void ListAssignmentsByPriority_ShouldReturnAssignmentsSortedByPriority()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Formatted");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);

            var lowPriority = new Assignment("Low Priority", "Task A", "Notes", false, Priority.Low);
            var highPriority = new Assignment("High Priority", "Task B", "Notes", false, Priority.High);
            var mediumPriority = new Assignment("Medium Priority", "Task C", "Notes", false, Priority.Medium);

            service.AddAssignment(lowPriority);
            service.AddAssignment(highPriority);
            service.AddAssignment(mediumPriority);

            // Act
            var sorted = service.ListAssignmentsByPriority();

            // Assert
            Assert.Equal(3, sorted.Count);
            Assert.Equal("High Priority", sorted[0].Title);
            Assert.Equal("Medium Priority", sorted[1].Title);
            Assert.Equal("Low Priority", sorted[2].Title);
        }

        [Fact]
        public void FindAssignmentByTitle_ShouldReturnCorrectAssignment()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment = new Assignment("Test Title", "Test Description", "Test Notes", false);
            service.AddAssignment(assignment);

            // Act
            var result = service.FindAssignmentByTitle("Test Title");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
        }

        [Fact]
        public void FindAssignmentByTitle_ShouldReturnAssignmentNotFound()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Formatted");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);

            var assignment = new Assignment("Existing Task", "Test desc", "Notes", false);
            service.AddAssignment(assignment);

            // Act
            var result = service.FindAssignmentByTitle("Nonexistent Title");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void MarkAssignmentComplete_ShouldMarkAssignmentAsCompleted()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment = new Assignment("Test Title", "Test Description", "Test Notes", true);
            service.AddAssignment(assignment);

            // Act
            var result = service.MarkAssignmentComplete("Test Title");

            // Assert
            Assert.True(result);
            Assert.True(assignment.IsCompleted);
        }

        [Fact]
        public void DeleteAssignment_ShouldRemoveAssignment()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment = new Assignment("Test Title", "Test Description", "Test Notes", false);
            service.AddAssignment(assignment);

            // Act
            var result = service.DeleteAssignment("Test Title");

            // Assert
            Assert.True(result);
            Assert.Null(service.FindAssignmentByTitle("Test Title"));
        }

        [Fact]
        public void UpdateAssignment_ShouldChangeTitleAndDescription()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("Mocked Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment = new Assignment("Old Title", "Old Description", "Test Notes", false);
            service.AddAssignment(assignment);

            // Act
            service.UpdateAssignment("Old title", "New Title", "New Description", "Noted", true, Priority.Medium);

            // Assert
            Assert.Equal("New Title", assignment.Title);
            Assert.Equal("New Description", assignment.Description);
        }

        [Fact]
        public void UpdateAssignment_ShouldReturnAssignmentNotFound()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("formatted");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);

            // Act
            var result = service.UpdateAssignment("Missing Title", "New Title", "Desc", "Notes", false, Priority.Low);

            // Assert
            Assert.False(result);
            mockLogger.Verify(l => l.Log("Update failed: 'Missing Title' not found."), Times.Once);
        }

        [Fact]
        public void UpdateAssignment_ShouldFail_WhenAssignmentIsDuplicate()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("formatted");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment1 = new Assignment("Original", "Desc", "Notes", false);
            var assignment2 = new Assignment("NewTitle", "Desc", "Notes", false);

            service.AddAssignment(assignment1);
            service.AddAssignment(assignment2);

            // Act
            var result = service.UpdateAssignment("Original", "NewTitle", "Updated Desc", "Updated Notes", true, Priority.High);

            // Assert
            Assert.False(result);
            mockLogger.Verify(l => l.Log("Update failed: Duplicate title 'NewTitle' exists."), Times.Once);
        }

        [Fact]
        public void Format_ShouldReturnFormattedString()
        {
            // Arrange
            var assignment = new Assignment("Test Title", "Test Description", "Test Notes", false);
            var formatter = new AssignmentFormatter();

            // Act
            var result = formatter.Format(assignment);

            // Assert
            var expected = $"Assignment ID: {assignment.Id}, Title: Test Title, Description: Test Description";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AddAssignment_MoqShouldCallFormatter()
        {
            // Arrange

            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();

            var assignment = new Assignment("Test Title", "Test Description", "Test Notes", false);
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>()))
                         .Returns("Formatted Assignment");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);

            // Act

            service.AddAssignment(assignment);

            // Assert

            mockFormatter.Verify(f => f.Format(It.Is<Assignment>(a => a.Title == "Test Title")), Times.Once);

        }

        [Fact]
        public void AddAssignment_ShouldCallLogger()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("formatted");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment = new Assignment("Test", "Test Desc", "Test Notes", false);

            // Act
            service.AddAssignment(assignment);

            // Assert
            mockLogger.Verify(l => l.Log("Added: formatted"), Times.Once);
        }

        [Fact]
        public void DeleteAssignment_ShouldCallLogger()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("formatted");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment = new Assignment("Test", "Test Desc", "Test Notes", false);
            service.AddAssignment(assignment);

            // Act
            service.DeleteAssignment("Test");

            // Assert
            mockLogger.Verify(l => l.Log("Deleted: formatted"), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallLogger()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            mockFormatter.Setup(f => f.Format(It.IsAny<Assignment>())).Returns("formatted");

            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);
            var assignment = new Assignment("Old Title", "Old Desc", "Test Notes", false);
            service.AddAssignment(assignment);

            mockLogger.Invocations.Clear();

            // Act
            service.UpdateAssignment("Old Title", "New Title", "New Desc", "Noted", false, Priority.Medium);

            // Assert
            mockLogger.Verify(l => l.Log("Updated: formatted"), Times.Once);
        }

    }
}
