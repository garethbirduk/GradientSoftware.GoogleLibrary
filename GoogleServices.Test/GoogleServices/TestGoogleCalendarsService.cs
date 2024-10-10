using Google.Apis.Calendar.v3.Data;
using GoogleServices.GoogleServices;

namespace GoogleServices.Test.GoogleServices
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

        [DataTestMethod]
        [DataRow("_deleteme_")]
        public async Task TestDeleteCalendars_WithPredicate(string startsWithPredicateExpression)
        {
            Func<CalendarListEntry, bool> predicate = x => x.Summary.StartsWith(startsWithPredicateExpression);
            var calendarIds = GoogleCalendarsService.GetCalendars(predicate).Items.Select(x => x.Id).ToList();
            if (!calendarIds.Any())
                await GoogleCalendarsService.CreateOrGetCalendarAsync(startsWithPredicateExpression);
            await GoogleCalendarsService.DeleteCalendarsAsync(predicate);
        }
    }
}