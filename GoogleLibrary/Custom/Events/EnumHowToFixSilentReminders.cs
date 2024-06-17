namespace GoogleLibrary.Custom.Events
{
    /// <summary>
    /// An enum for defining how to deal with reminders that are with the SlientRemindersPeriods.
    /// </summary>
    public enum EnumHowToFixSilentReminders
    {
        /// <summary>
        /// Don't do anything with reminders. They will appear regardless of any SlientRemindersPeriods.
        /// </summary>
        Ignore = 0,

        /// <summary>
        /// Remove reminders set within any SlientRemindersPeriods.
        /// </summary>
        Remove = 1,

        /// <summary>
        /// For each reminder set within the SlientRemindersPeriods:
        /// bring forward any reminders for events that occur within the period;
        /// delay any reminders for events that occur after the period;
        /// all subject to the DefaultRemindersInMinutes.
        /// </summary>
        Fix
    }
}