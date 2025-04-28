namespace AssignmentManagement.Tests
{
    using Xunit;
    using AssignmentLibrary;

    public class AssignmentServiceTests
    {

        [Fact]
        public void AddAssignment_ShouldAddAssignmentToList()
        {
            // Arrange
            var service = new AssignmentService();
            var assignment = new Assignment("Read Chapter 3", "Answer question 1-8");

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
            var assignment1 = new Assignment("As 1 Read Chapter 1", "Anotate chapter 1");
            var assignment2 = new Assignment("Chapter 1 worksheet", "Answer question 1-9");

            service.AddAssignment(assignment1);
            service.AddAssignment(assignment2);

            // Act
            var assignments = service.ListAll();

            // Assert
            Assert.Equal(2, assignments.Count);
            Assert.Contains(assignment1, assignments);
            Assert.Contains(assignment2, assignments);
        }
    }
}
