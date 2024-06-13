using GoogleLibrary.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using System.Collections.Generic;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class TestBasicEvent
    {
        private readonly DateTime eventStart = new DateTime(2024, 2, 1, 8, 0, 0);

        private List<(TimeSpan Start, TimeSpan End)> silentPeriods =
        [
            (TimeSpan.FromHours(0), TimeSpan.FromHours(7)), // Silent period from 00:00 to 07:00
            (TimeSpan.FromHours(22), TimeSpan.FromHours(24)), // Silent period from 22:00 to 23:59:59, note that 24:00 is technically 00:00 next day
            (TimeSpan.FromHours(21), TimeSpan.FromHours(23).Add(TimeSpan.FromMinutes(10))), // Silent period from 21:00 to 23:10
            (TimeSpan.FromHours(18), TimeSpan.FromHours(19)), // Silent period from 18:00 to 19:00
        ];

        [TestMethod]
        public void FinalizeReminders_ShouldRemoveDuplicatesAndSort()
        {
            var reminders = new List<int> { 10, 20, 10, 5, 5 };
            var finalizedReminders = BasicEvent.FinalizeReminders(reminders);
            CollectionAssert.AreEqual(new List<int> { 5, 10, 20 }, finalizedReminders);
        }

        [TestMethod]
        public void TestSummary()
        {
            var e = new BasicEvent()
            {
                Title = "MyEvent"
            };
            e.SummaryPrefix.Add("first line");
            e.SummaryPrefix.Add("second line");
            e.SummarySuffix.Add("penultimate line");
            e.SummarySuffix.Add("ultimate line");

            var s = e.Summary;
            Assert.AreEqual("first line", s[0]);
            Assert.AreEqual("second line", s[1]);
            Assert.AreEqual("MyEvent", s[2]);
            Assert.AreEqual("penultimate line", s[3]);
            Assert.AreEqual("ultimate line", s[4]);
            Assert.AreEqual(5, s.Count);
        }

        //[TestMethod]
        //public void AdjustForSilentPeriods_ShouldAdjustTimeOutsideSilentPeriods()
        //{
        //    var reminderTime = new DateTime(2024, 1, 31, 18, 30, 0);
        //    var adjustedTime = BasicEvent.AdjustForSilentPeriods(reminderTime, silentPeriods);
        //    Assert.AreEqual(new DateTime(2024, 1, 31, 17, 59, 0), adjustedTime);
        //}

        //[TestMethod]
        //public void EnsureMinimumReminderTime_ShouldEnforceMinimumTime()
        //{
        //    var reminderTime = new DateTime(2024, 2, 1, 7, 30, 0); // 30 minutes before the event
        //    var adjustedTime = BasicEvent.EnsureMinimumReminderTime(reminderTime, eventStart, 60);
        //    Assert.AreEqual(new DateTime(2024, 2, 1, 7, 0, 0), adjustedTime); // 60 minutes before the event
        //}
        //[DataTestMethod]
        //[DataRow(0, 0)]   // February 1, 08:00 (unadjusted, no silent period)
        //[DataRow(10, 10)] // February 1, 07:50 (unadjusted, no silent period)
        //[DataRow(20, 20)] // February 1, 07:40 (unadjusted, no silent period)
        //[DataRow(30, 30)] // February 1, 07:30 (unadjusted, no silent period)
        //[DataRow(60, 60)] // February 1, 07:00 (on the limit of silent period, unadjusted)
        //public void GetAdjustedReminderTimes_ShouldCorrectlyAdjustReminders(int originalMinutes, int expectedMinutes)
        //{
        //    /*
        //    THESE ARE NOT WORKING AT THE MOMENT
        //    //[DataRow(90, 719)] // February 1, 06:30 (adjusted to just before silent period starts at 23:59 previous day)
        //    //[DataRow(630, 661)] // January 31, 21:30 (adjusted to January 31, 20:59 due to silent period from 21:00 to 23:10)
        //    //[DataRow(810, 810)] // January 31, 17:30 (unadjusted, no silent period)
        //    //[DataRow(870, 841)] // January 31, 18:30 (adjusted to January 31, 17:59 due to silent period from 18:00 to 19:00)
        //    */
        //    // Arrange
        //    List<int> originalReminderMinutes = new List<int> { originalMinutes };
        //    List<int> expectedAdjustedReminders = new List<int> { expectedMinutes };

        //    // Act
        //    List<int> actualAdjustedReminders = BasicEvent.GetAdjustedReminderTimes(eventStart, originalReminderMinutes, silentPeriods, 60);

        //    // Assert
        //    CollectionAssert.AreEqual(expectedAdjustedReminders, actualAdjustedReminders, $"Failed for original reminder {originalMinutes}");
        //}
    }
}