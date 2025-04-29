namespace AssignmentManagement.Tests
{
    using Xunit;
    using AssignmentLibrary;

    public class AssignmentServiceTest
    {

        [Fact]
        public void AddAssignment_ShouldAddAssignmentToList()
        {
            // Arrange
            var service = new AssignmentService();
            var assignment = new Assignment("Read Chapter 3", "Answer question 1-8", false);

            // Act
            service.AddAssignment(assignment);

            var assignments = service.ListAll();

            // Assert
            Assert.Contains(assignment, assignments);
        }

        [Fact]
        public void ListAll_ShouldReturnAllAssignments()
        {
            // Arrange
            var service = new AssignmentService();
            var assignment1 = new Assignment("As 1 Read Chapter 1", "Anotate chapter 1", false);
            var assignment2 = new Assignment("Chapter 1 worksheet", "Answer question 1-9", false);

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
            var service = new AssignmentService();
            var assignment1 = new Assignment("As 1 Read Chapter 1", "Anotate chapter 1", false);
            var assignment2 = new Assignment("Chapter 1 worksheet", "Answer question 1-9", false);


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
            var service = new AssignmentService();

            // Act
            var incompleteAssignments = service.ListIncomplete();

            // Assert
            Assert.Empty(incompleteAssignments);
        }

        [Fact]
        public void ListIncomplete_ShouldReturnOnlyIncomplete_WhenMixofInAndCompleted()
        {
            // Arrange
            var service = new AssignmentService();
            var incompleteAssignment = new Assignment("Chapter 1", "Annotate Chapter 1", false);
            var completedAssignment = new Assignment("WorkSheet 1", "Do questions 1-12", false);

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
        public void FindAssignmentByTitle_ShouldReturnCorrectAssignment()
        {
            // Arrange
            var service = new AssignmentService();
            var assignment = new Assignment("Test Title", "Test Description");
            service.AddAssignment(assignment);

            // Act
            var result = service.FindAssignmentByTitle("Test Title");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
        }

        [Fact]
        public void MarkAssignmentComplete_ShouldMarkAssignmentAsCompleted()
        {
            // Arrange
            var service = new AssignmentService();
            var assignment = new Assignment("Test Title", "Test Description");
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
            var service = new AssignmentService();
            var assignment = new Assignment("Test Title", "Test Description");
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
            var service = new AssignmentService();
            var assignment = new Assignment("Old Title", "Old Description");
            service.AddAssignment(assignment);

            // Act
            assignment.Update("New Title", "New Description");

            // Assert
            Assert.Equal("New Title", assignment.Title);
            Assert.Equal("New Description", assignment.Description);
        }

    }
}
