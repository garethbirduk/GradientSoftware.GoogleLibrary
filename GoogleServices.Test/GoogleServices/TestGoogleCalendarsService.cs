using Google.Apis.Calendar.v3.Data;
using GoogleServices.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarsService
    {
        private static GoogleCalendarsService GoogleCalendarsService = new();

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            GoogleCalendarsService = new GoogleCalendarsService();
            GoogleCalendarsService.Initialize();
        }

        [DataTestMethod]
        [DataRow("primary", "Calendar is primary calendar")]
        [DataRow("primary2", "Calendar is primary calendar")]
        [DataRow("garethbird", "Calendar name is a reserved name")]
        [DataRow("gareth", "Calendar name is a reserved name")]
        [DataRow("garethx", "Calendar name starts with a reserved name")]
        [DataRow("mytestreserved", "Calendar name contains a reserved name")]
        public void TestCheckDeleteCalendarExceptions(string name, string reason)
        {
            var ex = Assert.ThrowsException<CannotDeleteCalendarException>(() => GoogleCalendarsService.CheckCanDeleteCalendar(name));
            Assert.IsTrue(ex.Message.Contains(reason), $"Message: \"{ex.Message}\" does not contain \"{reason}\"");
        }

        [DataTestMethod]
        [DataRow("aaa")]
        [DataRow("bbb")]
        public void TestCheckDeleteCalendarOk(string name)
        {
            Assert.IsTrue(GoogleCalendarsService.CheckCanDeleteCalendar(name));
        }

        [TestMethod]
        public async Task TestCreateDeleteCalendar()
        {
            var calendarName = TestHelpers.RandomCalendarName();
            var calendar = await GoogleCalendarsService.CreateOrGetCalendarAsync(calendarName);
            try
            {
                Assert.IsTrue(calendar.Summary == calendarName);
            }
            finally
            {
                await GoogleCalendarsService.DeleteCalendarAsync(calendar.Id);
            }
        }

        [TestMethod]
        public async Task TestDeleteCalendars_WithPredicate()
        {
            // Use a per-run unique prefix so this test doesn't sweep other tests'
            // `_deleteme_*` calendars while they're in-flight.
            var prefix = $"_predtest_{Guid.NewGuid():N}_";
            Func<CalendarListEntry, bool> predicate = x => x.Summary.StartsWith(prefix);

            await GoogleCalendarsService.CreateOrGetCalendarAsync($"{prefix}a");
            await GoogleCalendarsService.CreateOrGetCalendarAsync($"{prefix}b");
            Assert.AreEqual(2, GoogleCalendarsService.GetCalendars(predicate).Items.Count);

            // Verifies the delete-many-by-predicate path completes without throwing.
            // Not asserting post-delete count because Google's CalendarList has eventual
            // consistency — the deleted calendars may still appear briefly.
            await GoogleCalendarsService.DeleteCalendarsAsync(predicate);
        }
    }
}