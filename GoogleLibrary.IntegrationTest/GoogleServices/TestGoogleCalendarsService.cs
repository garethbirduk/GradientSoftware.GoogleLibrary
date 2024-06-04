using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.GoogleAuthentication;
using GoogleLibrary.GoogleServices;

namespace GoogleLibrary.IntegrationTest.GoogleServices
{
    [TestClass]
    public class TestGoogleCalendarsService : GoogleAuthenticatedUnitTest
    {
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
            var summary = Guid.NewGuid().ToString();
            var calendar = await GoogleCalendarsService.CreateOrGetCalendarAsync(summary);
            try
            {
                Assert.IsTrue(calendar.Summary == summary);
            }
            finally
            {
                await GoogleCalendarsService.DeleteCalendarAsync(calendar.Id);
            }
        }

        [TestMethod]
        public async Task TestDeleteCalendars_WithPredicate()
        {
            Func<CalendarListEntry, bool> predicate = x => x.Summary.StartsWith("_");
            var calendarIds = GoogleCalendarsService.GetCalendars(predicate).Items.Select(x => x.Id).ToList();
            if (!calendarIds.Any())
                await GoogleCalendarsService.CreateCalendarAsync("_deleteme_");
            await GoogleCalendarsService.DeleteCalendarsAsync(predicate);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleCalendarsService);
        }
    }
}