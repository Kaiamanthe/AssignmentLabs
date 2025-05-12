using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using AssignmentLibrary.Api.Models;
using System.Text;
using System.Text.Json;

namespace AssignmentWork.Tests
{
    public class WorkApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public WorkApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task Can_Create_And_Get_Work()
        {
            // Create work
            var workJson = new StringContent(
                JsonSerializer.Serialize(new { Content = "Intergration Test Work" }),
                Encoding.UTF8,"application/json");

            var response = await _client.PostAsync("/api/Work", workJson);
            response.EnsureSuccessStatusCode();

            // Get work
            var getResponse = await _client.GetAsync("/api/Work/GetAll");
            getResponse.EnsureSuccessStatusCode();

            var responseBody = await getResponse.Content.ReadAsStringAsync();
            var works = JsonSerializer.Deserialize<List<Work>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            Assert.Contains(works, n => n.Content == "Intergration Test Work");
        }
    }
}
