using ExpertCentreTechnicalTask.InternTests.TestInfrastructure;
using Newtonsoft.Json;
using System.Text;
using Xunit;
using Xunit.Priority;

namespace ExpertCentreTechnicalTask.InternTests.IntegrationTests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection("WorkspaceTests")]
    public class WorkspaceTests : IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public WorkspaceTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.HttpClient;
        }

        public async Task InitializeAsync()
        {
            // Асинхронная инициализация перед тестами, если требуется
        }

        public async Task DisposeAsync()
        {
            // Асинхронная очистка после тестов, если требуется
        }

        /// <summary>
        /// Вернет список рабочих пространств из 2 элементов.
        /// </summary>
        [Fact, Priority(1)]
        public async Task Get_workspaces_and_return_workspaces()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"/api/workspaces");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseWorkspaceListAsJsonString = await GetFileAsJsonAsync("ResponseWorkspaceList.json");
            Assert.Equal(expected: responseWorkspaceListAsJsonString, actual: responseContent);
        }

        /// <summary>
        /// Создаст рабочее пространство и в ответе получим детальную информацию о новом рабочем пространстве.
        /// </summary>
        [Fact, Priority(2)]
        public async Task Create_workspace_and_return_details_of_new_workspace()
        {
            // Arrange
            var workspace = new
            {
                Name = "Personal"
            };
            var workspaceAsJson = JsonConvert.SerializeObject(workspace);
            var content = new StringContent(workspaceAsJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/workspaces", content);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Assert.Fail("Ожидается 201 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseCreatedWorkspaceAsJson = await GetFileAsJsonAsync("ResponseCreatedWorkspace.json");
            Assert.Equal(expected: responseCreatedWorkspaceAsJson, actual: responseContent);
        }

        /// <summary>
        /// Вернет содержимое заметки по новому созданному идентификатору заметки.
        /// </summary>
        [Fact, Priority(3)]
        public async Task Get_detailed_workspace_and_return_detailed_workspace()
        {
            // Arrange
            var workspaceId = 3;

            // Act
            var response = await _client.GetAsync($"/api/workspaces/{workspaceId}");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseDetailedNoteAsJson = await GetFileAsJsonAsync("ResponseDetailedWorkspace.json");
            Assert.Equal(expected: responseDetailedNoteAsJson, actual: responseContent);
        }

        /// <summary>
        /// Обновить содержимое заметки.
        /// </summary>
        [Fact, Priority(4)]
        public async Task Update_workspace_and_return_updated_workspace()
        {
            // Arrange
            var workspaceId = 3;
            var requestUpdateWorkspaceAsJson = await GetFileAsJsonAsync("RequestUpdateWorkspace.json");
            var content = new StringContent(requestUpdateWorkspaceAsJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/workspaces/{workspaceId}", content);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseUpdatedWorkspaceAsJson = await GetFileAsJsonAsync("ResponseUpdatedWorkspace.json");
            Assert.Equal(expected: responseUpdatedWorkspaceAsJson, actual: responseContent);
        }

        /// <summary>
        /// Получит отказ так как указанного рабочего пространства не существует.
        /// </summary>
        [Fact, Priority(5)]
        public async Task Update_workspace_and_return_error_message_because_workspace_does_not_exist()
        {
            // Arrange
            var nonExistingWorkspaceId = 10;
            var requestUpdateWorkspaceAsJson = await GetFileAsJsonAsync("RequestUpdateWorkspace.json");
            var content = new StringContent(requestUpdateWorkspaceAsJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/workspaces/{nonExistingWorkspaceId}", content);
            if (response.StatusCode != System.Net.HttpStatusCode.BadRequest)
            {
                Assert.Fail("Ожидается 400 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseUpdatedWorkspaceAsJson = await GetFileAsJsonAsync("ResponseFailBecauseWorkspaceIdDoesNotExist.json");
            Assert.Equal(expected: responseUpdatedWorkspaceAsJson, actual: responseContent);
        }

        /// <summary>
        /// Удаляет рабочее пространство.
        /// </summary>
        [Fact, Priority(6)]
        public async Task Delete_workspace_return_deleted_workspace_id()
        {
            // Arrange
            var workspaceId = 3;

            // Act
            var response = await _client.DeleteAsync($"/api/workspaces/{workspaceId}/");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseDeletedWorkspaceAsJson = await GetFileAsJsonAsync("ResponseDeletedWorkspace.json");
            Assert.Equal(expected: responseDeletedWorkspaceAsJson, actual: responseContent);
        }

        private async Task<string> GetFileAsJsonAsync(string fileName)
        {
            var filePath = Path.Combine("IntegrationTests", "TestFiles", fileName); 
            var fileContent = await File.ReadAllTextAsync(filePath);
            var jsonObject = JsonConvert.DeserializeObject(fileContent);
            var cleanJson = JsonConvert.SerializeObject(jsonObject, Formatting.None);

            return cleanJson;
        }
    }
}
