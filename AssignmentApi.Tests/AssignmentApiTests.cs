using AssignmentLibrary.Api.Models;
using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace AssignmentManagement.ApiTests
{
    public class AssignmentApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AssignmentApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Can_Create_Assignment()
        {
            var assignment = new
            {
                title = $"Assignment-{Guid.NewGuid()}",
                description = "Created during test",
                isCompleted = false,
                priority = "Medium"
            };

            var response = await _client.PostAsJsonAsync("/api/assignment", assignment);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Can_Retrieve_All_Assignments()
        {
            var title = $"RetrieveAll-{Guid.NewGuid()}";
            var dto = new AssignmentDto
            {
                Title = title,
                Description = "Test Desc",
                Notes = "Test notes",
                IsCompleted = false,
                Priority = Priority.Medium
            };

            await _client.PostAsJsonAsync("/api/assignment", dto);

            var response = await _client.GetAsync("/api/assignment");
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());

            var assignments = await response.Content.ReadFromJsonAsync<List<AssignmentDto>>(options);
            Assert.NotNull(assignments);
            Assert.Contains(assignments, a => a.Title == title);
        }

        [Fact]
        public async Task Can_List_Incomplete_Assignments()
        {
            var title = $"Incomplete-{Guid.NewGuid()}";
            var dto = new AssignmentDto
            {
                Title = title,
                Description = "Test Desc",
                Notes = "",
                IsCompleted = false,
                Priority = Priority.Low
            };

            await _client.PostAsJsonAsync("/api/assignment", dto);

            var response = await _client.GetAsync("/api/assignment/incomplete");
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());

            var assignments = await response.Content.ReadFromJsonAsync<List<AssignmentDto>>(options);
            Assert.Contains(assignments, a => a.Title == title && !a.IsCompleted);
        }

        [Fact]
        public async Task Can_List_Assignments_Sorted_By_Priority()
        {
            // Arrange
            var titleLow = $"PriorityLow-{Guid.NewGuid()}";
            var titleHigh = $"PriorityHigh-{Guid.NewGuid()}";
            var titleMedium = $"PriorityMedium-{Guid.NewGuid()}";

            var assignments = new List<AssignmentDto>
            {
                new AssignmentDto
                {
                    Title = titleLow,
                    Description = "Low Priority",
                    Notes = "",
                    IsCompleted = false,
                    Priority = Priority.Low
                },
                new AssignmentDto
                {
                    Title = titleHigh,
                    Description = "High Priority",
                    Notes = "",
                    IsCompleted = false,
                    Priority = Priority.High
                },
                new AssignmentDto
                {
                    Title = titleMedium,
                    Description = "Medium Priority",
                    Notes = "",
                    IsCompleted = false,
                    Priority = Priority.Medium
                }
            };

            foreach (var dto in assignments)
            {
                var response = await _client.PostAsJsonAsync("/api/assignment", dto);
                Assert.True(response.IsSuccessStatusCode);
            }

            // Act
            var responseList = await _client.GetAsync("/api/assignment/by-priority");
            responseList.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            var result = await responseList.Content.ReadFromJsonAsync<List<AssignmentDto>>(options);

            var returnedTitles = result
                .Where(a => a.Title == titleLow || a.Title == titleMedium || a.Title == titleHigh)
                .Select(a => a.Title)
                .ToList();

            // Assert
            var expectedOrder = new List<string> { titleHigh, titleMedium, titleLow };
            Assert.Equal(expectedOrder, returnedTitles);
        }

        public async Task Can_Find_Assignment_By_Title()
        {
            var title = $"FindMe-{Guid.NewGuid()}";
            var dto = new AssignmentDto
            {
                Title = title,
                Description = "Searchable",
                Notes = "",
                IsCompleted = false,
                Priority = Priority.Medium
            };

            await _client.PostAsJsonAsync("/api/assignment", dto);

            var response = await _client.GetAsync($"/api/assignment/{Uri.EscapeDataString(title)}");
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());

            var assignment = await response.Content.ReadFromJsonAsync<AssignmentDto>(options);
            Assert.Equal(title, assignment.Title);
        }

        [Fact]
        public async Task Can_Update_Assignment()
        {
            var oldTitle = $"UpdateTest-{Guid.NewGuid()}";
            var newTitle = $"{oldTitle}-New";

            var original = new AssignmentDto
            {
                Title = oldTitle,
                Description = "Old Desc",
                Notes = "",
                IsCompleted = false,
                Priority = Priority.Medium
            };

            await _client.PostAsJsonAsync("/api/assignment", original);

            var updated = new AssignmentDto
            {
                Title = newTitle,
                Description = "New Desc",
                Notes = "Updated Notes",
                IsCompleted = true,
                Priority = Priority.High
            };

            var response = await _client.PutAsJsonAsync($"/api/assignment/{Uri.EscapeDataString(oldTitle)}", updated);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

            var verify = await _client.GetAsync($"/api/assignment/{Uri.EscapeDataString(newTitle)}");
            verify.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());

            var result = await verify.Content.ReadFromJsonAsync<AssignmentDto>(options);

            Assert.Equal(newTitle, result.Title);
            Assert.Equal("New Desc", result.Description);
            Assert.Equal("Updated Notes", result.Notes);
            Assert.True(result.IsCompleted);
            Assert.Equal(Priority.High, result.Priority);
        }

        [Fact]
        public async Task Can_Delete_Assignment()
        {
            var title = $"DeleteMe-{Guid.NewGuid()}";
            var dto = new AssignmentDto
            {
                Title = title,
                Description = "To be deleted",
                Notes = "",
                IsCompleted = false,
                Priority = Priority.Medium
            };

            await _client.PostAsJsonAsync("/api/assignment", dto);

            var deleteResponse = await _client.DeleteAsync($"/api/assignment/{Uri.EscapeDataString(title)}");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/assignment/{Uri.EscapeDataString(title)}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task Can_Add_Note_To_Assignment()
        {
            var title = $"AddNote-{Guid.NewGuid()}";
            var original = new AssignmentDto
            {
                Title = title,
                Description = "Original Description",
                Notes = "",
                IsCompleted = false,
                Priority = Priority.Medium
            };

            await _client.PostAsJsonAsync("/api/assignment", original);

            var noteContent = JsonContent.Create("This is the new note.");
            var patchResponse = await _client.PatchAsync($"/api/assignment/{Uri.EscapeDataString(title)}/note", noteContent);
            Assert.True(patchResponse.IsSuccessStatusCode);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());

            var verifyResponse = await _client.GetAsync($"/api/assignment/{Uri.EscapeDataString(title)}");
            verifyResponse.EnsureSuccessStatusCode();

            var updatedAssignment = await verifyResponse.Content.ReadFromJsonAsync<AssignmentDto>(options);
            Assert.Equal("This is the new note.", updatedAssignment.Notes);
        }
        [Fact]
        public async Task Add_Note_To_Assignment_Returns_Success_Message()
        {
            // Arrange
            var title = $"AddNoteResponseTest-{Guid.NewGuid()}";
            var original = new AssignmentDto
            {
                Title = title,
                Description = "Test Description",
                Notes = "",
                IsCompleted = false,
                Priority = Priority.Medium
            };

            // Act
            var postResponse = await _client.PostAsJsonAsync("/api/assignment", original);
            Assert.True(postResponse.IsSuccessStatusCode);

            var noteContent = JsonContent.Create("Response note check");
            var patchResponse = await _client.PatchAsync($"/api/assignment/{Uri.EscapeDataString(title)}/note", noteContent);

            // Assert
            Assert.True(patchResponse.IsSuccessStatusCode);

            var message = await patchResponse.Content.ReadAsStringAsync();
            Assert.Equal("Note added or updated successfully.", message);
        }
        [Fact]
        public async Task Creating_Assignment_With_Notes_Should_Persist_Notes()
        {
            // Arrange
            var title = $"WithNotes-{Guid.NewGuid()}";
            var notes = "These are the initial notes.";
            var dto = new AssignmentDto
            {
                Title = title,
                Description = "Testing note persistence",
                Notes = notes,
                IsCompleted = false,
                Priority = Priority.Medium
            };

            // Act
            var createResponse = await _client.PostAsJsonAsync("/api/assignment", dto);
            Assert.True(createResponse.IsSuccessStatusCode);

            var getResponse = await _client.GetAsync($"/api/assignment/{Uri.EscapeDataString(title)}");
            getResponse.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            var result = await getResponse.Content.ReadFromJsonAsync<AssignmentDto>(options);

            // Assert
            Assert.Equal(notes, result.Notes);
        }
        [Fact]
        public async Task Notes_Should_Be_Updated_And_Retrievable()
        {
            // Arrange
            var title = $"PatchNotes-{Guid.NewGuid()}";
            var initial = new AssignmentDto
            {
                Title = title,
                Description = "Patch test",
                Notes = "",
                IsCompleted = false,
                Priority = Priority.Medium
            };

            var newNote = "This note was added later.";

            // Act
            var createResponse = await _client.PostAsJsonAsync("/api/assignment", initial);
            Assert.True(createResponse.IsSuccessStatusCode);

            var patchContent = JsonContent.Create(newNote);
            var patchResponse = await _client.PatchAsync($"/api/assignment/{Uri.EscapeDataString(title)}/note", patchContent);
            Assert.True(patchResponse.IsSuccessStatusCode);

            var getResponse = await _client.GetAsync($"/api/assignment/{Uri.EscapeDataString(title)}");
            getResponse.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            var result = await getResponse.Content.ReadFromJsonAsync<AssignmentDto>(options);

            // Assert
            Assert.Equal(newNote, result.Notes);
        }

    }
}
