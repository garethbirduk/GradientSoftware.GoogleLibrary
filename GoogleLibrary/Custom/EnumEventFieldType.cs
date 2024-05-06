using System;
using Utils.Attributes;

namespace GoogleLibrary.Custom
{
    public enum EnumEventFieldType
    {
        None,

        [Description]
        Unknown,

        [Alias("Title", "Event", "Name")]
        Summary,

        [Type(typeof(DateTime))]
        [Alias("Start Date", "Date", "Date1")]
        StartDate,

        [Alias("Start Time", "Time", "Time1")]
        StartTime,

        [Alias("End Time", "Time2")]
        EndTime,

        [Type(typeof(DateTime))]
        [Alias("End Date", "Date2")]
        EndDate,

        [Alias("Start", "Location", "Location1")]
        From,

        [Alias("Notes")]
        Description,

        [Alias("Reminder", "Minutes", "Notifications")]
        Reminders,

        [Alias("Type", "Event Type", "EventType")]
        Category,

        Status,

        [Alias("End, Location2")]
        To,

        Via,

        Via2,

        FromAddress,

        ToAddress,

        ViaAddress,

        ViaAddress2,

        [Alias("Flight", "Flight Number", "Flight Nr", "Flight No", "FlightNo", "FlightNr")]
        FlightNumber,

        [Alias("Carrier", "Flight Carrier")]
        FlightCarrier,

        [Alias("Who")]
        ContactableAttendees,

        [Description]
        [Type(typeof(Currency))]
        [Alias("$", "USD")]
        PriceDollars,

        [Description]
        [Type(typeof(Currency))]
        [Alias("£", "GBP")]
        PricePounds,

        [Description]
        [Type(typeof(Currency))]
        [Alias("E", "EUR")]
        PriceEuros,
    }

    public static class Currency
    {
    }
}