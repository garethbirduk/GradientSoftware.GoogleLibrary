using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace GoogleLibrary.GoogleServices
{
    internal class GooglePersonalData : IGooglePersonalData
    {
        public List<string> ReservedList { get; set; } =
        [
            "garethbird",
            "garethbirdmaster",
            "garethandvivian",
            "gareth.and.vivian",
            "viviancamacho",
            "testreserved",
            "family",
            "bird",
            "vivian",
            "gareth",
            "rafael",
            "zoe",
            "bramcote",
            "home",
            "camacho",
        ];
    }

    public class GoogleCalendarsService : GoogleCalendarsReadonlyService, IGoogleCalendarsService
    {
        public IGooglePersonalData GooglePersonalData { get; set; } = new GooglePersonalData();

        public override IEnumerable<string> Scopes => new List<string>() { CalendarService.Scope.Calendar };

        public bool CheckCanDeleteCalendar(string calendarId, params CannotDeleteCalendarReasons[] cannotDeleteCalendarReasons)
        {
            if (cannotDeleteCalendarReasons.Length == 0)
                cannotDeleteCalendarReasons = Enum.GetValues<CannotDeleteCalendarReasons>();

            if (cannotDeleteCalendarReasons.Contains(CannotDeleteCalendarReasons.CalendarIsPrimary))
                if (calendarId.Contains("primary", StringComparison.InvariantCultureIgnoreCase))
                    throw new CannotDeleteCalendarException(calendarId, CannotDeleteCalendarReasons.CalendarIsPrimary);

            if (cannotDeleteCalendarReasons.Contains(CannotDeleteCalendarReasons.CalendarHasReservedName))
                if (GooglePersonalData.ReservedList.Contains(calendarId))
                    throw new CannotDeleteCalendarException(calendarId, CannotDeleteCalendarReasons.CalendarHasReservedName);

            foreach (var item in GooglePersonalData.ReservedList)
            {
                if (item.StartsWith(calendarId, StringComparison.InvariantCultureIgnoreCase) || calendarId.StartsWith(item, StringComparison.InvariantCultureIgnoreCase))
                    throw new CannotDeleteCalendarException(calendarId, CannotDeleteCalendarReasons.CalendarStartsWithReservedName);
                if (item.Contains(calendarId, StringComparison.InvariantCultureIgnoreCase) || calendarId.Contains(item, StringComparison.InvariantCultureIgnoreCase))
                    throw new CannotDeleteCalendarException(calendarId, CannotDeleteCalendarReasons.CalendarContainsReservedName);
            }

            return true;
        }

        public CalendarListEntry CreateCalendar(string summary)
        {
            var calendar = new Calendar()
            {
                Summary = summary,
            };
            var newCalendar = GoogleService.Calendars.Insert(calendar).Execute();

            var calendarListEntry = new CalendarListEntry
            {
                Id = newCalendar.Id,
            };

            var newCalendarListEntry = GoogleService.CalendarList.Insert(calendarListEntry).Execute();
            return GetCalendar(newCalendarListEntry.Id);
        }

        public async Task<CalendarListEntry> CreateCalendarAsync(string summary, bool allowDuplicate = false)
        {
            await Task.CompletedTask;
            if (allowDuplicate)
                return CreateCalendar(summary);
            var calendar = GetCalendarBySummary(summary);
            return calendar ?? throw new NotSupportedException($"Cannot create a calendar that is same as existing calendar.");
        }

        public async Task<CalendarListEntry> CreateOrGetCalendarAsync(string summary, bool clear = false)
        {
            var calendar = GetCalendarBySummary(summary);
            if (calendar == null)
                return CreateCalendar(summary);
            else if (clear)
            {
                var s = new GoogleCalendarEventsService
                {
                    ClientSecrets = ClientSecrets
                };
                s.SetupToken();
                s.SetupExternalServices();
                await s.ClearEvents(calendar.Id);
            }
            return calendar;
        }

        public async Task DeleteCalendarAsync(string calendarId)
        {
            if (CheckCanDeleteCalendar(calendarId))
                await GoogleService.Calendars.Delete(calendarId).ExecuteAsync();
        }

        public async Task DeleteCalendarsAsync(Func<CalendarListEntry, bool> predicate)
        {
            var calendarIds = GetCalendars(predicate).Items.Select(x => x.Id).ToList();
            foreach (var calendarId in calendarIds)
                await DeleteCalendarAsync(calendarId);
        }
    }
}