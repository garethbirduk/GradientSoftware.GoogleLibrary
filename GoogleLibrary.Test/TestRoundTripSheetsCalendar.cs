using GoogleLibrary.CustomServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using GoogleLibrary.Services;
using GoogleLibrary.Custom;
using System;
using GoogleLibrary.GoogleAuthentication;

namespace GoogleLibrary.Test
{
    [TestClass]
    public class TestRoundTripSheetsCalendar : GoogleAuthenticatedUnitTest
    {
        [TestInitialize]
        public async Task TestInitialize()
        {
            await GoogleOAuthAuthenticatorHelper.CreateAsync(
                GoogleCalendarService, GoogleCalendarsService, GoogleCalendarReadonlyService,
                GoogleSpreadsheetService, GoogleSpreadsheetReadonlyService);
        }

        [TestMethod]
        public async Task TestRoundTrip()
        {
            var calendarName = Utils.RandomName();
            var worksheetName = "ExampleSimple";

            CalendarId = GoogleCalendarsService.CreateCalendar(calendarName).Id;
            var customSpreadsheetService = new CustomSpreadsheetService(GoogleSpreadsheetReadonlyService, GoogleCalendarService);
            var customCalendarEventsService = new CustomCalendarService(GoogleCalendarReadonlyService, GoogleSpreadsheetService);

            try
            {
                await customSpreadsheetService.WorksheetToCalendarAsync(SpreadsheetId, worksheetName, CalendarId, maxEvents: 2);
                await customCalendarEventsService.CalendarToWorksheetAsync(CalendarId, SpreadsheetId, calendarName);
            }
            finally
            {
                await GoogleCalendarsService.DeleteCalendarAsync(CalendarId);
                await GoogleSpreadsheetService.DeleteWorksheetsAsync(SpreadsheetId, calendarName);
            }
        }
    }
}