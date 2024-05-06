namespace GoogleLibrary.Custom
{
    /// <summary>
    /// Enum for the status of an event
    /// </summary>
    public enum EventStatus
    {
        /// <summary>
        /// There is no status defined.
        /// </summary>
        None,

        /// <summary>
        /// The event is just an idea.
        /// </summary>
        Idea,

        /// <summary>
        /// The event is planned.
        /// </summary>
        Planned,

        /// <summary>
        /// The event has been confirmed.
        /// </summary>
        Confirmed,

        /// <summary>
        /// The event has been paid for.
        /// </summary>
        Paid,

        /// <summary>
        /// The event has been cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The event has been reserved.
        /// </summary>
        Reserved,
    }
}