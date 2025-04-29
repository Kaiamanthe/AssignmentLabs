using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentLibrary.Tests
{
    public class ConsoleUITest
    {
        [Fact]
        public void AddAssignment_MoqAssertSuccess()
        {
            // Arrange
            var mockService = new Mock<AssignmentService> { CallBase = true };
            var assignment = new Assignment("Test Title", "Test Description", false);

            // Act
            var result = mockService.Object.AddAssignment(assignment);

            // Assert
            Assert.True(result);
        }

    }
}
