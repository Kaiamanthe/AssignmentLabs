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
            var testAssignment = new Assignment("New Assignment", "New Description", "Test Notes", false, Priority.Medium);

            mockService.Setup(x => x.AddAssignment(testAssignment)).Returns(true);

            // Act
            var result = mockService.Object.AddAssignment(testAssignment);

            // Assert
            mockService.Verify(m => m.AddAssignment(It.IsAny<Assignment>()), Times.Once);
        }


        [Fact]
        public void SearchAssignmentByTitle_MoqObjectShouldReturnObjectIfTitleFound()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();

            var expectedAssignment = new Assignment("Test Title", "Test Description", "Test Notes", false);

            mockService.Setup(s => s.FindAssignmentByTitle("Test Title"))
                       .Returns(expectedAssignment);

            // Act
            var result = mockService.Object.FindAssignmentByTitle("Test Title");

            // Assert
            Assert.NotNull(result);
            mockService.Verify(m => m.FindAssignmentByTitle("Test Title"), Times.Once);
        }

        [Fact]
        public void DeleteAssignment_ShouldRemoveAssignmentFromSimulatedStore()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            var assignments = new List<Assignment>();

            // Sim AddAssignment
            mockService.Setup(x => x.AddAssignment(It.IsAny<Assignment>()))
                       .Callback<Assignment>(a => assignments.Add(a))
                       .Returns(true);

            // Sim DeleteAssignment
            mockService.Setup(x => x.DeleteAssignment(It.IsAny<string>()))
                       .Callback<string>(title =>
                       {
                           var item = assignments.FirstOrDefault(a => a.Title == title);
                           if (item != null)
                           {
                               assignments.Remove(item);
                           }
                       })
                       .Returns(true);

            var testAssignment = new Assignment("Test Title", "Test Description", "Test Notes", false);

            // Act
            mockService.Object.AddAssignment(testAssignment);
            mockService.Object.DeleteAssignment("Test Title");

            // Assert
            mockService.Verify(m => m.AddAssignment(It.IsAny<Assignment>()), Times.Once);
            mockService.Verify(m => m.DeleteAssignment("Test Title"), Times.Once);
            Assert.DoesNotContain(assignments, a => a.Title == "Test Title");
        }

        [Fact]
        public void AddNoteToAssignment_ShouldUpdateNote_WhenNoteIsEmpty()
        {
            // Arrange
            var assignment = new Assignment("Test Assignment", "Desc", "", false, Priority.Medium);

            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.FindAssignmentByTitle("Test Assignment")).Returns(assignment);

            var ui = new ConsoleUI(mockService.Object);

            // Sim user input
            var input = new StringReader("Test Assignment\nNew test note\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI)
                .GetMethod("AddNoteToAssignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(ui, null);

            // Assert
            Assert.Equal("New test note", assignment.Notes);
            Assert.Contains("Note updated - Assignment: Test Assignment", output.ToString());
        }

        [Fact]
        public void AddNoteToAssignment_ShouldProduceConsoleOutput()
        {
            // Arrange
            var assignment = new Assignment("Test Assignment", "Desc", "", false, Priority.Medium);
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.FindAssignmentByTitle("Test Assignment")).Returns(assignment);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Test Assignment\nSome test note content\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI)
                .GetMethod("AddNoteToAssignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(ui, null);

            // Assert
            var consoleOutput = output.ToString();
            Assert.False(string.IsNullOrWhiteSpace(consoleOutput), "Console output should not be empty.");
            Assert.Contains("Assignment: Test Assignment", consoleOutput);
        }


    }
}
