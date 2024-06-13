//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections.Generic;
//using System;

//namespace GoogleLibrary.Events.Test
//{
//    [TestClass]
//    public class TestRemindersHelper
//    {
//        private static readonly DateTime eventStart = new DateTime(2024, 1, 1, 8, 0, 0); // 08:00

//        private static readonly List<(TimeSpan Start, TimeSpan End)> silentPeriods = new List<(TimeSpan Start, TimeSpan End)>
//        {
//            (TimeSpan.FromHours(0), TimeSpan.FromHours(7)),  // 00:00 to 07:00
//            (TimeSpan.FromHours(23), TimeSpan.FromHours(24)), // 23:00 to 00:00
//            (TimeSpan.FromHours(21), TimeSpan.FromHours(21.5)), // 21:00 to 21:30
//            (TimeSpan.FromHours(22), TimeSpan.FromHours(23.5)), // 22:00 to 23:30
//        };

//        [DataTestMethod]
//        [DataRow("2024-01-01T00:00:00", true)]  // Exactly at the start of the period
//        [DataRow("2024-01-01T06:59:59", true)]  // Just before the end of the period
//        [DataRow("2024-01-01T07:00:00", false)]  // Just after the end of the period
//        [DataRow("2024-01-01T07:00:01", false)] // Just after the end of the period
//        [DataRow("2024-01-01T20:59:59", false)] // Just before the start of the period
//        [DataRow("2024-01-01T21:00:00", true)]  // Exactly at the start of the period
//        [DataRow("2024-01-01T21:29:59", true)]  // Exactly at the end of the period
//        [DataRow("2024-01-01T21:30:00", false)] // Just after the end of the period
//        [DataRow("2024-01-01T21:30:01", false)] // Just after the end of the period
//        [DataRow("2024-01-01T21:59:59", false)] // Just before the start of the period
//        [DataRow("2024-01-01T22:00:00", true)]  // Exactly at the start of the period
//        [DataRow("2024-01-01T22:59:59", true)] // Just before the start of the period, but exists in other period
//        [DataRow("2024-01-01T23:00:00", true)]  // Exactly at the start of the period
//        [DataRow("2024-01-01T23:30:00", true)]  // Exactly at the end of the period
//        [DataRow("2024-01-01T23:30:01", true)] // Just after the end of the period, but exists in other period
//        [DataRow("2024-01-02T00:00:00", true)]  // Exactly at the end of the period
//        [DataRow("2024-01-02T00:00:01", true)] // Just after the end of the period, but exists in other period
//        public void TestReminderDateTimeIsInSilentPeriod(string reminder, bool expected)
//        {
//            bool result = RemindersHelper.ReminderIsInSilentPeriod(DateTime.ParseAdvanced(reminder), silentPeriods);
//            Assert.AreEqual(expected, result, $"Testing for reminder at {reminder}. Expected {expected}, got {result}.");
//        }

//        [DataTestMethod]
//        // For the period 00:00 to 07:00
//        [DataRow(480, true)]  // 8 hours before 08:00 (00:00), exactly at the start of the period
//        [DataRow(61, true)]   // 61 minutes before 08:00 (06:59), last minute inside the period
//        [DataRow(60, false)]  // 60 minutes before 08:00 (07:00), exactly at the end of the period, outside due to exclusivity

//        // For the period 23:00 to 00:00
//        [DataRow(540, true)]  // 9 hours before 08:00 (23:00), exactly at the start of the period
//        [DataRow(480, true)]  // 8 hours before 08:00 (00:00), exactly at the end of the period, includes 00:00 since it’s technically the next day
//        [DataRow(479, true)]  // 7 hours and 59 minutes before 08:00 (00:01), just after the end of the period, but falls within the 00:00 to 07:00 period

//        // For the period 21:00 to 21:30
//        [DataRow(660, true)]  // 660 minutes before 08:00 (21:00), exactly at the start of the period
//        [DataRow(631, true)]  // 631 minutes before 08:00 (21:29), last minute inside the period
//        [DataRow(630, false)] // 630 minutes before 08:00 (21:30), just after the end of the period, outside due to exclusivity

//        // For the period 22:00 to 23:30
//        [DataRow(600, true)]  // 600 minutes before 08:00 (22:00), exactly at the start of the period
//        [DataRow(510, true)]  // 510 minutes before 08:00 (23:30), last minute inside the period
//        [DataRow(509, true)]  // 509 minutes before 08:00 (23:31), just after the end of the period, but still within the period 23:00 to 00:00
//        public void TestReminderIsInSilentPeriod(int reminderMinutesBefore, bool expected)
//        {
//            DateTime eventStart = new DateTime(2024, 1, 1, 8, 0, 0);
//            bool result = RemindersHelper.ReminderIsInSilentPeriod(eventStart.AddMinutes(-reminderMinutesBefore), silentPeriods);
//            Assert.AreEqual(expected, result, $"Testing for reminder {reminderMinutesBefore} minutes before event. Expected {expected}, got {result}.");
//        }
//    }
//}