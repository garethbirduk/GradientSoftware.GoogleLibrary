using GoogleLibrary.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace GoogleLibrary.Test.Events
{
    [TestClass]
    public class DateTimeHelperTests
    {
        [TestMethod]
        public void Parse_CustomCultureInfo_ParsesCorrectly()
        {
            // Arrange
            var date = "01/01/2024";
            var cultureInfo = new CultureInfo("en-GB"); // dd/MM/yyyy format

            // Act
            var result = DateTimeHelper.Parse(date, cultureInfo);

            // Assert
            Assert.AreEqual(new DateTime(2024, 1, 1), result);
        }

        [TestMethod]
        public void Parse_FallsBackToDefaultParse()
        {
            // Arrange
            var date = "January 1, 2024";

            // Act
            var result = DateTimeHelper.Parse(date);

            // Assert
            Assert.AreEqual(new DateTime(2024, 1, 1), result);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidDateFormat_ThrowsFormatException()
        {
            // Arrange
            var invalidDate = "Invalid Date String";

            // Act
            DateTimeHelper.Parse(invalidDate);
        }

        [TestMethod]
        [DataRow("Mon 1 Jan", 2024, 1, 1)]
        [DataRow("Mon 30 Dec", 2024, 12, 30)]
        [DataRow("Monday 30 Dec", 2024, 12, 30)]
        [DataRow("Monday 30 Dec 2024", 2024, 12, 30)]
        [DataRow("Monday 30th Dec 2024", 2024, 12, 30)]
        [DataRow("Friday 14th June 2024", 2024, 06, 14)]
        [DataRow("Thursday 13th June 2024", 2024, 06, 13)]
        [DataRow("Thursday 13th Jun 2024", 2024, 06, 13)]
        [DataRow("1 Jan", 2024, 1, 1)]
        [DataRow("1st Jan", 2024, 1, 1)]
        [DataRow("31 Dec", 2024, 12, 31)]
        [DataRow("01/01/2024", 2024, 1, 1)]
        [DataRow("1/07/2024", 2024, 7, 1)]
        [DataRow("01/1/2024", 2024, 1, 1)]
        [DataRow("31/12/2024", 2024, 12, 31)]
        [DataRow("01/01/24", 2024, 1, 1)]
        [DataRow("1/07/24", 2024, 7, 1)]
        [DataRow("01/1/24", 2024, 1, 1)]
        [DataRow("31/12/24", 2024, 12, 31)]
        public void Parse(string input, int expectedYear, int expectedMonth, int expectedDay)
        {
            // Arrange
            var expected = new DateTime(expectedYear, expectedMonth, expectedDay);

            // Act
            var result = DateTimeHelper.Parse(input);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}