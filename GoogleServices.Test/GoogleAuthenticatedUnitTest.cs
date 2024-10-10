using GoogleServices.CustomServices;
using GoogleServices.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    /// <summary>
    /// base class gives tests access to all the services derived classes may need.
    /// </summary>
    public class GoogleAuthenticatedUnitTest
    {
        protected string CalendarId { get; set; } = "";

        protected CustomCalendarService CustomCalendarService { get; set; } = new();

        protected CustomSpreadsheetService CustomSpreadsheetService { get; set; } = new();

        protected GoogleAllScopesService GoogleAllScopesService { get; set; } = new();

        protected GoogleCalendarEventsReadonlyService GoogleCalendarEventsReadonlyService { get; set; } = new();

        protected GoogleCalendarEventsService GoogleCalendarEventsService { get; set; } = new();

        protected GoogleCalendarReadonlyService GoogleCalendarReadonlyService { get; set; } = new();

        protected GoogleCalendarService GoogleCalendarService { get; set; } = new();

        protected GoogleCalendarsReadonlyService GoogleCalendarsReadonlyService { get; set; } = new();

        protected GoogleCalendarsService GoogleCalendarsService { get; set; } = new();

        protected GoogleSpreadsheetReadonlyService GoogleSpreadsheetReadonlyService { get; set; } = new();

        protected GoogleSpreadsheetService GoogleSpreadsheetService { get; set; } = new();

        protected GoogleSpreadsheetsReadonlyService GoogleSpreadsheetsReadonlyService { get; set; } = new();

        protected GoogleSpreadsheetsService GoogleSpreadsheetsService { get; set; } = new();

        protected string SpreadsheetId { get; set; } = "166KxWAwDKeMagoVh6RGdrc8BmzIaNmgM7i8W9IDCT7A";

        public GoogleAuthenticatedUnitTest()
        {
        }
    }
}