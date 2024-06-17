﻿using GoogleLibrary.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GoogleLibrary.Events.Tests
{
    [TestClass]
    public class TestFlightEvent
    {
        [TestMethod]
        public void AddCustomSummary_ReturnsCorrectSummary()
        {
            var flightEvent = new FlightEvent();
            flightEvent.FlightInformation.Number = "BA1234";
            flightEvent.FlightInformation.Carrier = "British Airways";
            flightEvent.Locations.Add(new AirportLocation("LHR"));
            flightEvent.Locations.Add(new AirportLocation("MAD"));
            flightEvent.Locations.Add(new AirportLocation("VVI"));

            var summary = flightEvent.AddCustomSummary();

            Assert.AreEqual(2, summary.Count);
            Assert.AreEqual("British Airways (BA1234)", summary[0]);
            Assert.AreEqual("LHR - VVI", summary[1]);
        }

        [TestMethod]
        public void Build_SetsFlightInformationCorrectly()
        {
            var flightEvent = new FlightEvent();
            var fields = new List<Tuple<string, EnumEventFieldType>>
            {
                new Tuple<string, EnumEventFieldType>("From", EnumEventFieldType.From),
                new Tuple<string, EnumEventFieldType>("Via", EnumEventFieldType.Via),
                new Tuple<string, EnumEventFieldType>("To", EnumEventFieldType.To),
                new Tuple<string, EnumEventFieldType>("Carrier", EnumEventFieldType.FlightCarrier),
                new Tuple<string, EnumEventFieldType>("Number", EnumEventFieldType.FlightNumber)
            };
            var data = new List<string> { "LHR", "MAD", "VVI", "Airline", "12345" };

            flightEvent.Build(fields, data);

            Assert.AreEqual("Airline", flightEvent.FlightInformation.Carrier);
            Assert.AreEqual("12345", flightEvent.FlightInformation.Number);
            Assert.IsTrue(flightEvent.CustomFields.ContainsKey("Flight"));
            Assert.IsTrue(flightEvent.CustomFields.ContainsKey("Flight tracker"));

            Assert.AreEqual("Airline (12345) LHR - VVI", flightEvent.Title);
        }

        [TestMethod]
        public void FlightEvent_DefaultRemindersInMinutes_AreSetCorrectly()
        {
            var flightEvent = new FlightEvent();

            var expectedReminders = new List<int> { 2 * 60, 4 * 60 };
            CollectionAssert.AreEqual(expectedReminders, flightEvent.DefaultRemindersInMinutes);
        }

        [TestMethod]
        public void FlightEvent_FlightInformation_IsInitialized()
        {
            var flightEvent = new FlightEvent();
            Assert.IsNotNull(flightEvent.FlightInformation);
        }
    }
}