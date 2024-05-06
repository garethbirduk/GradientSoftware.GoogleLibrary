using Utils.Attributes;

namespace GoogleLibrary.Custom
{
    public enum EventFieldTypeAdditional
    {
        [Alias("Type")]
        EventType = 0,

        Status = 1,

        [Alias("End, Location2")]
        To = 2,

        Via = 3,

        Via2 = 4,

        FromAddress = 5,

        ToAddress = 6,

        ViaAddress = 7,

        ViaAddress2 = 8,

        [Alias("Flight", "Flight Number", "Flight Nr", "Flight No", "FlightNo", "FlightNr")]
        FlightNumber = 9,

        [Alias("Who")]
        ContactableAttendees = 10,
    }
}