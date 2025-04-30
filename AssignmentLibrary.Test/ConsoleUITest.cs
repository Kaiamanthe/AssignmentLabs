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
        public void AddAssignment_ShouldPassIfMoqObjectAdded()
        {
            // Arrange
            var mockService = new Mock<AssignmentService> { CallBase = true };

            // Act
            mockService.Object.AddAssignment(new Assignment("Test Title", "Test Description", false));

            // Assert
            Assert.NotNull(mockService);
        }

        [Fact]
        public void SearchAssignmentByTitle_MoqObjectShouldReturnObjectIfTitleFound()
        {
            // Arrange
            var mockService = new Mock<AssignmentService>();
            mockService.Object.AddAssignment(new Assignment("Test Title", "Test Description", false));

            // Act
            var result = mockService.Object.FindAssignmentByTitle("Test Title");

            // Assert
            Assert.Equal("Test Title", result.Title);
        }

    }
}
