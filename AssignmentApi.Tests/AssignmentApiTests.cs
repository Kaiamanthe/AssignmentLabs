using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AssignmentLibrary.Core;
using AssignmentLibrary.Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;
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
            // Arrange
            var assignment = new
            {
                title = "Integration Test Assignment",
                description = "Created during test",
                isCompleted = false,
                priority = "Medium"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/assignment", assignment);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Can_Retrieve_All_Assignments()
        {
            // Act
            var response = await _client.GetAsync("/api/assignment");

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

            var assignments = JsonSerializer.Deserialize<List<Assignment>>(content, options);

            Assert.NotNull(assignments);
            Assert.Contains(assignments, a => a.Title == "Integration Test Assignment");
        }


    }
}
