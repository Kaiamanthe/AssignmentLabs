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
    }
}
