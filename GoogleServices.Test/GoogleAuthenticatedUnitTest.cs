using Google.Apis.Calendar.v3.Data;
using GoogleLibrary.EventsServices;
using GoogleServices;

namespace GoogleLibrary.IntegrationTest
{
    /// <summary>
    /// base class gives tests access to all the services derived classes may need.
    /// </summary>
    public class GoogleAuthenticatedUnitTest
    {
        protected CalendarListEntry Calendar { get; set; }
        protected string CalendarId { get; set; }
        protected CustomCalendarService CustomCalendarService { get; set; }
        protected CustomSpreadsheetService CustomSpreadsheetService { get; set; }
        protected GoogleAllScopesService GoogleAllScopesService { get; set; } = new GoogleAllScopesService();
        protected GoogleCalendarEventsReadonlyService GoogleCalendarEventsReadonlyService { get; set; } = new GoogleCalendarEventsReadonlyService();
        protected GoogleCalendarEventsService GoogleCalendarEventsService { get; set; } = new GoogleCalendarEventsService();
        protected GoogleCalendarReadonlyService GoogleCalendarReadonlyService { get; set; } = new GoogleCalendarReadonlyService();
        protected GoogleCalendarService GoogleCalendarService { get; set; } = new GoogleCalendarService();
        protected GoogleCalendarsReadonlyService GoogleCalendarsReadonlyService { get; set; } = new GoogleCalendarsReadonlyService();
        protected GoogleCalendarsService GoogleCalendarsService { get; set; } = new GoogleCalendarsService();
        protected GoogleSpreadsheetReadonlyService GoogleSpreadsheetReadonlyService { get; set; } = new GoogleSpreadsheetReadonlyService();
        protected GoogleSpreadsheetService GoogleSpreadsheetService { get; set; } = new GoogleSpreadsheetService();
        protected GoogleSpreadsheetsReadonlyService GoogleSpreadsheetsReadonlyService { get; set; } = new GoogleSpreadsheetsReadonlyService();
        protected GoogleSpreadsheetsService GoogleSpreadsheetsService { get; set; } = new GoogleSpreadsheetsService();
        protected GoogleUserInformationService GoogleUserInformationService { get; set; }
        protected string SpreadsheetId { get; set; } = "166KxWAwDKeMagoVh6RGdrc8BmzIaNmgM7i8W9IDCT7A";
    }
}