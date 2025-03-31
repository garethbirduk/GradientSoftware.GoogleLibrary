using GoogleServices.CustomServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestCustomContactsService
    {
        public CustomContactsService CustomContactsService { get; set; } = new();

        [TestMethod]
        public void TestCleanup()
        {
            CustomContactsService.CleanupContacts();
        }
    }
}