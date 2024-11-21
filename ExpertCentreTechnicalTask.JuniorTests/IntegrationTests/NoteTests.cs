using ExpertCentreTechnicalTask.BaseTests;
using ExpertCentreTechnicalTask.JuniorTests.TestInfrastructure;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Priority;

namespace ExpertCentreTechnicalTask.JuniorTests.IntegrationTests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection("NoteTests")]
    public class NoteTests : IAsyncLifetime
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public NoteTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.HttpClient;
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
        /// Запрашиваем список заметок по указанному рабочему пространству и ожидаем получить отказ так как пользователь не авторизован.
        /// </summary>
        [Fact, Priority(-1)]
        public async Task Get_notes_by_workspace_and_return_unauthorized_user()
        {
            // Arrange
            var workspaceId = 1;

            // Act
            var response = await _client.GetAsync($"/api/workspaces/{workspaceId}/notes");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);

            AuthorizeUser();
        }

        /// <summary>
        /// Авторизовываем пользователя.
        /// </summary>
        public void AuthorizeUser()
        {
            int USERID = 9573;
            string USERNAME = "Ivanov";
            var jwtToken = JwtTokenGenerator.GenerateToken(USERNAME, USERID);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }

        /// <summary>
        /// Запрашиваем список заметок по указанному рабочему пространству и ожидаем получить список из 3-х заметок.
        /// </summary>
        [Fact, Priority(2)]
        public async Task Get_notes_by_workspace_and_return_3_notes()
        {
            // Arrange
            var workspaceId = 1;

            // Act
            var response = await _client.GetAsync($"/api/workspaces/{workspaceId}/notes");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseNodeListAsJson = await GetBodyFromFileAsJsonAsync("ResponseNoteList.json");
            Assert.Equal(expected: responseNodeListAsJson, actual: responseContent);
        }

        /// <summary>
        /// Запрашиваем список заметок по чужому рабочему пространству и получаем ошибку.
        /// </summary>
        /// <returns></returns>
        [Fact, Priority(3)]
        public async Task Get_notes_by_workspace_and_return_error_message_because_workspace_does_not_exist()
        {
            // Arrange
            var workspaceId = 10;

            // Act
            var response = await _client.GetAsync($"/api/workspaces/{workspaceId}/notes");
            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                Assert.Fail("Ожидается 404 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseNodeListAsJson = await GetBodyFromFileAsJsonAsync("ResponseFailBecauseWorkspaceIdDoesNotExist.json");
            Assert.Equal(expected: responseNodeListAsJson, actual: responseContent);
        }

        /// <summary>
        /// Успешно создаем пустую заметку.
        /// </summary>
        [Fact, Priority(4)]
        public async Task Create_note_with_workspace_and_return_details_of_new_note()
        {
            // Arrange
            var workspaceId = 1;
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/workspaces/{workspaceId}/notes", content);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Assert.Fail("Ожидается 201 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseCreatedNoteAsJson = await GetBodyFromFileAsJsonAsync("ResponseCreatedNote.json");
            Assert.Equal(expected: responseCreatedNoteAsJson, actual: responseContent);
        }

        /// <summary>
        /// Получаем подробную информацию о заметке.
        /// </summary>
        [Fact, Priority(5)]
        public async Task Get_detailed_note_and_return_detailed_node()
        {
            // Arrange
            var workspaceId = 1;
            var noteId = 4;

            // Act
            var response = await _client.GetAsync($"/api/workspaces/{workspaceId}/notes/{noteId}");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseDetailedNoteAsJson = await GetBodyFromFileAsJsonAsync("ResponseDetailedNote.json");
            Assert.Equal(expected: responseDetailedNoteAsJson, actual: responseContent);
        }

        /// <summary>
        /// Редактируем заметку и получаем в результате обновленную заметку.
        /// </summary>
        [Fact, Priority(6)]
        public async Task Update_note_and_return_detailed_node()
        {
            // Arrange
            var workspaceId = 1;
            var noteId = 4;
            var requestUpdateNoteAsJsonString = await GetBodyFromFileAsJsonAsync("RequestUpdateNote.json");
            var content = new StringContent(requestUpdateNoteAsJsonString, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/workspaces/{workspaceId}/notes/{noteId}", content);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseUpdatedNoteAsJson = await GetBodyFromFileAsJsonAsync("ResponseUpdatedNote.json");
            Assert.Equal(expected: responseUpdatedNoteAsJson, actual: responseContent);
        }

        /// <summary>
        /// Запрашиваем список заметок по указанному рабочему пространства и получаем список из 4-х заметок.
        /// </summary>
        [Fact, Priority(7)]
        public async Task Get_notes_by_workspace_after_creation_and_return_4_notes()
        {
            // Arrange
            var workspaceId = 1;

            // Act
            var response = await _client.GetAsync($"/api/workspaces/{workspaceId}/notes");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseNodeListAsJson = await GetBodyFromFileAsJsonAsync("ResponseListOf4NodesAfterCreation.json");
            Assert.Equal(expected: responseNodeListAsJson, actual: responseContent);
        }

        /// <summary>
        /// Редактируем заметку и получаем отказ так как заметка не найдена.
        /// </summary>
        /// <param name="workspaceId">Идентификатор рабочего пространства.</param>
        /// <param name="noteId">Идентификатор заметки.</param>
        [Theory, Priority(8)]
        [InlineData(10, 4)]
        [InlineData(1, 10)]
        public async Task Update_note_and_return_error_message_because_workspace_does_not_exist(int workspaceId, int noteId)
        {
            // Arrange
            var requestUpdateNoteAsJsonString = await GetBodyFromFileAsJsonAsync("RequestUpdateNote.json");
            var content = new StringContent(requestUpdateNoteAsJsonString, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/workspaces/{workspaceId}/notes/{noteId}", content);
            if (response.StatusCode != System.Net.HttpStatusCode.BadRequest)
            {
                Assert.Fail("Ожидается 400 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseUpdatedNoteAsJson = await GetBodyFromFileAsJsonAsync("ResponseFailUpdateNoteBecauseNoteIdDoesNotExist.json");
            Assert.Equal(expected: responseUpdatedNoteAsJson, actual: responseContent);
        }

        /// <summary>
        /// Удаляем заметку и ожидаем получить успешную операцию.
        /// </summary>
        [Fact, Priority(9)]
        public async Task Delete_note_return_deleted_noteid()
        {
            // Arrange
            var workspaceId = 1;
            var noteId = 1;

            // Act
            var response = await _client.DeleteAsync($"/api/workspaces/{workspaceId}/notes/{noteId}");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseDeletedNoteAsJson = await GetBodyFromFileAsJsonAsync("ResponseDeletedNote.json");
            Assert.Equal(expected: responseDeletedNoteAsJson, actual: responseContent);
        }

        /// <summary>
        /// Запрашиваем список заметок по указанному рабочему пространству и ожидаем получить список из 3-х заметок.
        /// </summary>
        [Fact, Priority(10)]
        public async Task Get_notes_by_workspace_after_deletion_and_return_3_notes()
        {
            // Arrange
            var workspaceId = 1;

            // Act
            var response = await _client.GetAsync($"/api/workspaces/{workspaceId}/notes");
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Assert.Fail("Ожидается 200 статус");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            var responseNodeListAsJson = await GetBodyFromFileAsJsonAsync("ResponseListOf3NodesAfterDeletion.json");
            Assert.Equal(expected: responseNodeListAsJson, actual: responseContent);
        }

        /// <summary>
        /// По имени JSON-файла загружает и возвращает содержимое файла.
        /// </summary>
        private async Task<string> GetBodyFromFileAsJsonAsync(string fileName)
        {
            var filePath = Path.Combine("IntegrationTests", "TestFiles", fileName);
            var fileContent = await File.ReadAllTextAsync(filePath);
            var jsonObject = JsonConvert.DeserializeObject(fileContent);
            var cleanJson = JsonConvert.SerializeObject(jsonObject, Formatting.None);

            return cleanJson;
        }
    }
}
