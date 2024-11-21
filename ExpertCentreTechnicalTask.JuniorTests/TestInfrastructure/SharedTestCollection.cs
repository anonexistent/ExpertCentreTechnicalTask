using Xunit;

namespace ExpertCentreTechnicalTask.JuniorTests.TestInfrastructure
{
    [CollectionDefinition("NoteTests")]
    public class NoteTestCollection : ICollectionFixture<CustomWebApplicationFactory>
    {
        // Здесь ничего не нужно реализовывать, потому что xUnit сам управляет жизненным циклом фабрики.
    }
}
