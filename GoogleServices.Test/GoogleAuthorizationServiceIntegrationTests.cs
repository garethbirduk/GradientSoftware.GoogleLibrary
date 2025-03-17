using Google.Apis.Calendar.v3.Data;
using GoogleServices.GoogleServices;

namespace GoogleServices.Tests
{
    [TestClass]
    public class GoogleCalendarServiceIntegrationTests
    {
        private GoogleCalendarsReadonlyService GoogleCalendarsReadonlyService = new();

        [TestInitialize]
        public void Setup()
        {
            // Initialize the GoogleCalendarReadonlyService with the real client secrets and initial scopes
            GoogleCalendarsReadonlyService = new GoogleCalendarsReadonlyService();
            GoogleCalendarsReadonlyService.Initialize();
        }

        [TestMethod]
        public void ShouldGrantReadAccessToCalendar()
        {
            try
            {
                // Example check: Fetch a calendar using the read-only service to verify permissions
                CalendarListEntry calendar = GoogleCalendarsReadonlyService.GetCalendar("primary");
                Assert.IsNotNull(calendar, "Calendar object should not be null");
                Console.WriteLine($"Calendar Summary: {calendar.Summary}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during read access authorization: {ex.Message}");
                throw; // Rethrow to let MSTest handle the exception
            }
        }
    }
}