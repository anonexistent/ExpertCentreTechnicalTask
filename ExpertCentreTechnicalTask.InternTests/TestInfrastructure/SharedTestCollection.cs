using Xunit;

namespace ExpertCentreTechnicalTask.InternTests.TestInfrastructure
{
    [CollectionDefinition("WorkspaceTests")]
    public class WorkspaceTestCollection : ICollectionFixture<CustomWebApplicationFactory>
    {
        // Здесь ничего не нужно реализовывать, потому что xUnit сам управляет жизненным циклом фабрики.
    }
}
