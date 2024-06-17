using GoogleLibrary.Custom.Events;
using GoogleLibrary.Custom.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TestBasicEventExtensions
    {
        [TestMethod]
        public void GetCustomFieldsAsDescription_WithValidCustomFields_ReturnsFormattedDescription()
        {
            var baseEvent = new BasicEvent();
            baseEvent.CustomFields.Add("Key1", "Value1");
            baseEvent.CustomFields.Add("Key2", "Value2");

            var result = BasicEventExtensions.GetCustomFieldsAsDescription(baseEvent);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Key1: Value1", result[0]);
            Assert.AreEqual("Key2: Value2", result[1]);
        }

        [TestMethod]
        public void ToDescriptionString_WithValidData_ReturnsFormattedString()
        {
            var baseEvent = new BasicEvent
            {
                Description = "Event Description",
            };
            baseEvent.CustomFields.Add("Key1", "Value1");
            baseEvent.AdditionalData.Add("Additional Data 1");
            baseEvent.AdditionalData.Add("Additional Data 2");

            var result = baseEvent.ToDescriptionString();

            var expected = "Event Description\r\nKey1: Value1\r\nAdditional Data 1\r\nAdditional Data 2";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToLocationString_WithEmptyLocations_ReturnsEmptyString()
        {
            var baseEvent = new BasicEvent
            {
                Locations = new List<Location>()
            };

            var result = baseEvent.ToLocationString();

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ToLocationString_WithValidLocations_ReturnsFormattedString()
        {
            var baseEvent = new BasicEvent
            {
                Locations = new List<Location>
                {
                    new Location { Address = "Address 1" },
                    new Location { Address = "Address 2" }
                }
            };

            var result = baseEvent.ToLocationString();

            var expected = "https://www.google.com/maps/dir/Address+1/Address+2";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToReminderOverrides_WithValidReminders_ReturnsEventReminders()
        {
            var baseEvent = new BasicEvent
            {
                Reminders = new List<int> { 10, 20, 30 }
            };

            var result = baseEvent.ToReminderOverrides();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("popup", result[0].Method);
            Assert.AreEqual(10, result[0].Minutes);
            Assert.AreEqual("popup", result[1].Method);
            Assert.AreEqual(20, result[1].Minutes);
            Assert.AreEqual("popup", result[2].Method);
            Assert.AreEqual(30, result[2].Minutes);
        }
    }
}