using System.ComponentModel.DataAnnotations;

namespace GoogleLibrary.Services
{
    public enum CannotDeleteCalendarReasons
    {
        /// <summary>
        /// Do not allow deletion of primary calendar
        /// </summary>
        [Display(Description = "Calendar is primary calendar.")]
        CalendarIsPrimary = 0,

        /// <summary>
        /// Do not allow deletion of reserved calendars
        /// </summary>
        [Display(Description = "Calendar name is a reserved name.")]
        CalendarHasReservedName = 1,

        /// <summary>
        /// Do not allow deletion of if name starts with a reserved calendar name
        /// </summary>
        [Display(Description = "Calendar name starts with a reserved name.")]
        CalendarStartsWithReservedName = 2,

        /// <summary>
        /// Do not allow deletion of if name contains that of a reserved calendar name
        /// </summary>
        [Display(Description = "Calendar name contains a reserved name.")]
        CalendarContainsReservedName = 3,

        /// <summary>
        /// Do not allow deletion of if calendar because it does not exist; used for exception handling
        /// </summary>
        [Display(Description = "Calendar does not exist.")]
        CalendarDoesNotExist = 4,
    }
}