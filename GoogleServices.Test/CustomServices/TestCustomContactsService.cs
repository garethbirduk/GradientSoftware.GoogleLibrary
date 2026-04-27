using GoogleServices.CustomServices;

namespace GoogleServices.Test.CustomServices
{
    [TestClass]
    public class TestCustomContactsService
    {
        public CustomContactsService CustomContactsService { get; set; } = new();

        [TestMethod]
        [TestCategory("Manual")]
        public void TestCleanup()
        {
            CustomContactsService.CleanupContacts();
        }
    }
}