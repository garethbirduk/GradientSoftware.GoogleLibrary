using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using GoogleServices.GoogleServices;
using Microsoft.Extensions.Configuration;

namespace GoogleServices.Tests
{
    [TestClass]
    public class GoogleCalendarServiceIntegrationTests
    {
        private GoogleCalendarsReadonlyService calendarsReadonlyService;

        [TestMethod]
        public async Task AuthorizeAsync_ShouldGrantReadAccessToCalendar()
        {
            try
            {
                // Example check: Fetch a calendar using the read-only service to verify permissions
                CalendarListEntry calendar = calendarsReadonlyService.GetCalendar("primary");
                Assert.IsNotNull(calendar, "Calendar object should not be null");
                Console.WriteLine($"Calendar Summary: {calendar.Summary}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during read access authorization: {ex.Message}");
                throw; // Rethrow to let MSTest handle the exception
            }
        }

        [TestInitialize]
        public void Setup()
        {
            // Initialize the GoogleCalendarReadonlyService with the real client secrets and initial scopes
            calendarsReadonlyService = new GoogleCalendarsReadonlyService();
        }
    }
}