using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Models;
using AssignmentLibrary.Core.Services;
using AssignmentLibrary.UI;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentLibrary.Tests
{
    public class ConsoleUITest
    {
        [Fact]
        public void AddAssignment_ShouldCallServiceAssignmentData()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.AddAssignment(It.IsAny<Assignment>())).Returns(true);

            var ui = new ConsoleUI(mockService.Object);

            // Simulate user input
            var input = new StringReader("Test Title\nTest Description\nTest Notes\nH\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("AddAssignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(ui, null);

            // Assert
            mockService.Verify(s => s.AddAssignment(It.Is<Assignment>(a =>
                a.Title == "Test Title" &&
                a.Description == "Test Description" &&
                a.Notes == "Test Notes" &&
                a.Priority == Priority.High &&
                a.IsCompleted == false
            )), Times.Once);

            Assert.Contains("Assignment added successfully.", output.ToString());
        }

        [Fact]
        public void AddAssignment_ShouldPrintError_WhenFail()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.AddAssignment(It.IsAny<Assignment>())).Returns(false);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Title\nDescription\n\nL\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("AddAssignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(ui, null);

            // Assert
            Assert.Contains("An assignment with this title already exists.", output.ToString());
        }

        [Fact]
        public void AddAssignment_ShouldUseDefaultPriority_WhenInputIsInvalid()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.AddAssignment(It.IsAny<Assignment>())).Returns(true);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Title\nDesc\n\n\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("AddAssignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(ui, null);

            // Assert
            mockService.Verify(s => s.AddAssignment(It.Is<Assignment>(a =>
                a.Notes == "" &&
                a.Priority == Priority.Medium
            )), Times.Once);
        }

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
        public void AddNoteToAssignment_ShouldUpdateNote_WhenNoteIsEmpty()
        {
            // Arrange
            var assignment = new Assignment("Test Assignment", "Desc", "", false, Priority.Medium);

            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.FindAssignmentByTitle("Test Assignment")).Returns(assignment);
            mockService.Setup(s => s.UpdateNote("Test Assignment", "New test note")).Returns(true);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Test Assignment\nNew test note\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            typeof(ConsoleUI)
                .GetMethod("AddNoteToAssignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(ui, null);

            // Assert
            Assert.Contains("Note updated - Assignment: Test Assignment", output.ToString());
            mockService.Verify(s => s.UpdateNote("Test Assignment", "New test note"), Times.Once);
        }

        [Fact]
        public void AddNoteToAssignment_ShouldProduceConsoleOutput()
        {
            // Arrange
            var assignment = new Assignment("Test Assignment", "Desc", "", false, Priority.Medium);

            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.FindAssignmentByTitle("Test Assignment")).Returns(assignment);
            mockService.Setup(s => s.UpdateNote(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Test Assignment\nSome test note content\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            typeof(ConsoleUI)
                .GetMethod("AddNoteToAssignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(ui, null);

            // Assert
            var consoleOutput = output.ToString();
            Assert.Contains("Note updated - Assignment: Test Assignment", consoleOutput);
        }

        [Fact]
        public void MarkAssignmentComplete_ShouldMarkAsComplete_WhenFound()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.MarkAssignmentComplete("Test Assignment")).Returns(true);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Test Assignment\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("MarkAssignmentComplete", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(ui, null);

            // Assert
            var result = output.ToString();
            Assert.Contains("Assignment marked as complete.", result);
            mockService.Verify(s => s.MarkAssignmentComplete("Test Assignment"), Times.Once);
        }

        [Fact]
        public void MarkAssignmentComplete_ShouldFail_WhenNotFound()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.MarkAssignmentComplete("Missing Assignment")).Returns(false);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Missing Assignment\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("MarkAssignmentComplete", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(ui, null);

            // Assert
            var result = output.ToString();
            Assert.Contains("Assignment not found.", result);
            mockService.Verify(s => s.MarkAssignmentComplete("Missing Assignment"), Times.Once);
        }

        [Fact]
        public void FindAssignmentByTitle_ShouldDisplayAssignment()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            var assignment = new Assignment("Title", "Desc", "Note", false, Priority.Medium);

            mockService.Setup(s => s.FindAssignmentByTitle("Title")).Returns(assignment);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Title\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("FindAssignmentByTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(ui, null);

            // Assert
            var consoleOutput = output.ToString();
            Assert.Contains("Assignment: Title Description: Desc", consoleOutput);
            Assert.Contains("Notes: Note", consoleOutput);
            Assert.Contains("Priority: Medium", consoleOutput);
            Assert.Contains("Completed: False", consoleOutput);
        }

        [Fact]
        public void FindAssignmentByTitle_ShouldFail_WhenNotFound()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.FindAssignmentByTitle("Title")).Returns((Assignment)null!);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Title\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("FindAssignmentByTitle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(ui, null);

            // Assert
            var consoleOutput = output.ToString();
            Assert.Contains("Assignment not found.", consoleOutput);
        }

        [Fact]
        public void FindAssignmentByTitle_MoqObjectShouldReturnObjectIfTitleFound()
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
        public void UpdateAssignment_ShouldFail_WhenFalse()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            var assignment = new Assignment("Test", "Old Desc", "Old Notes", false, Priority.Low);
            mockService.Setup(s => s.FindAssignmentByTitle("Test")).Returns(assignment);
            mockService.Setup(s => s.UpdateAssignment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<Priority>())).Returns(false);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Test\nNew Title\nNew Desc\nNew Notes\nn\nM\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("UpdateAssignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(ui, null);

            // Assert
            var consoleOut = output.ToString();
            Assert.Contains("Update failed.", consoleOut);
        }

        [Fact]
        public void DeleteAssignment_ShouldDelete()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.DeleteAssignment("Test Assignment")).Returns(true);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Test Assignment\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("DeleteAssignment", BindingFlags.Instance | BindingFlags.NonPublic);
            method!.Invoke(ui, null);

            // Assert
            var consoleOut = output.ToString();
            Assert.Contains("Assignment deleted successfully.", consoleOut);
            mockService.Verify(s => s.DeleteAssignment("Test Assignment"), Times.Once);
        }

        [Fact]
        public void DeleteAssignment_ShouldFail_WhenAssignmentNotFound()
        {
            // Arrange
            var mockService = new Mock<IAssignmentService>();
            mockService.Setup(s => s.DeleteAssignment("Missing Assignment")).Returns(false);

            var ui = new ConsoleUI(mockService.Object);

            var input = new StringReader("Missing Assignment\n");
            Console.SetIn(input);

            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            var method = typeof(ConsoleUI).GetMethod("DeleteAssignment", BindingFlags.Instance | BindingFlags.NonPublic);
            method!.Invoke(ui, null);

            // Assert
            var consoleOut = output.ToString();
            Assert.Contains("Assignment not found.", consoleOut);
            mockService.Verify(s => s.DeleteAssignment("Missing Assignment"), Times.Once);
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


    }
}
