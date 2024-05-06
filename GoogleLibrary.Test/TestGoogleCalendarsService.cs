using GoogleLibrary.CustomServices;
using GoogleLibrary.GoogleAuthentication;
using GoogleLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace GoogleLibrary.Test
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
        public async Task TestDeleteCalendar()
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

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleCalendarsService);
        }
    }
}