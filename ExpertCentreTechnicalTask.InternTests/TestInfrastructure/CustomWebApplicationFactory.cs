using ExpertCentreTechnicalTask.TestAssembly;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ExpertCentreTechnicalTask.InternTests.TestInfrastructure
{
    public class CustomWebApplicationFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        public HttpClient HttpClient { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            // Логика для инициализации тестового окружения

            HttpClient = CreateClient();
        }

        public new async Task DisposeAsync()
        {
            // Логика для очистки ресурсов после тестов
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Настройка окружения для тестового приложения
            builder.ConfigureServices(services =>
            {
                // Настройка зависимостей и сервисов для тестов
            });
        }
    }
}
