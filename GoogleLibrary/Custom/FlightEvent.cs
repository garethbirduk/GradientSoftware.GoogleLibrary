﻿using System;
using System.Collections.Generic;

namespace GoogleLibrary.Custom
{
    public class FlightEvent : TravelEvent
    {
        protected override List<int> DefaultRemindersInMinutes { get; set; } = new List<int>() { 2 * 60, 4 * 60, };

        internal override List<string> AddCustomSummary()
        {
            var list = new List<string>
            {
                FlightInformation.FlightSummary,
                RouteSummary
            };
            return list;
        }

        public FlightInformation FlightInformation { get; private set; } = new FlightInformation();

        public override void Build(List<Tuple<string, EnumEventFieldType>> fields, List<string> data)
        {
            base.Build(fields, data);
            FlightInformation.Carrier = GetString(EnumEventFieldType.FlightCarrier, fields, data);
            FlightInformation.Number = GetString(EnumEventFieldType.FlightNumber, fields, data);
            CustomFields.Add("Flight", FlightInformation.FlightDetails);
            CustomFields.Add("Flight tracker", FlightInformation.FlightTracker);
        }
    }
}