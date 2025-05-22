using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssignmentLibrary.UI;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;
using AssignmentLibrary.Core.Services;
using AssignmentLibrary.Core;

namespace AssignmentLibrary.Tests
{
    public class ConsoleUITest
    {
        //[Fact] Test with Moq before interface revision
        //public void AddAssignment_ShouldPassIfMoqObjectAdded()
        //{
        //    // Arrange
        //    var mockService = new Mock<AssignmentService> { CallBase = true };

        //    // Act
        //    mockService.Object.AddAssignment(new Assignment("Test Title", "Test Description", false));

        //    // Assert
        //    Assert.NotNull(mockService);
        //}

        //[Fact]
        //public void SearchAssignmentByTitle_MoqObjectShouldReturnObjectIfTitleFound()
        //{
        //    // Arrange
        //    var mockService = new Mock<AssignmentService>();
        //    mockService.Object.AddAssignment(new Assignment("Test Title", "Test Description", false));

        //    // Act
        //    var result = mockService.Object.FindAssignmentByTitle("Test Title");

        //    // Assert
        //    Assert.Equal("Test Title", result.Title);
        //}

        //[Fact]
        //public void DeleteAssignment_ShouldRemovedMockObject()
        //{
        //    // Arrange
        //    var mockService = new Mock<AssignmentService>();

        //    // Act
        //    mockService.Object.DeleteAssignment("Test Title");

        //    // Assert
        //    Assert.Empty(mockService.Object.ListAll());
        //}

        [Fact]
        public void AddAssignment_ShouldPassIfMoqObjectAdded()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();

            // Act
            mockService.Setup(x => x.AddAssignment(new Assignment("New Assignment", "New Description", false, Priority.Medium))).Returns(true);
            mockService.Object.AddAssignment(null);

            // Assert
            mockService.Verify(m => m.AddAssignment(It.IsAny<Assignment>()), Times.Once);
        }

        [Fact]
        public void SearchAssignmentByTitle_MoqObjectShouldReturnObjectIfTitleFound()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Object.AddAssignment(new Assignment("Test Title", "Test Description", false));

            // Act
            mockService.Setup(s => s.FindAssignmentByTitle("Test Title"))
                       .Returns(new Assignment("Test Title", "Test Description", false));
            mockService.Object.FindAssignmentByTitle("Test Title");

            // Assert
            mockService.Verify(m => m.FindAssignmentByTitle("Test Title"), Times.Once);
        }
        [Fact]
        public void DeleteAssignment_ShouldRemovedMockObject()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();

            // Act
            mockService.Object.AddAssignment(new Assignment("Test Title", "Test Description", false));
            mockService.Setup(x => x.DeleteAssignment("Test Title")).Returns(true);
            mockService.Object.DeleteAssignment("Test Title");

            // Assert
            mockService.Verify(m => m.AddAssignment(It.IsAny<Assignment>()), Times.Once);
            mockService.Verify(m => m.DeleteAssignment("Test Title"), Times.Once);
        }


        // Formatter Test


    }
}
