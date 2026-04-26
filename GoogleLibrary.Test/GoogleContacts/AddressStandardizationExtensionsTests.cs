using GoogleLibrary.GoogleContacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.GoogleContacts
{
    [TestClass]
    public class AddressStandardizationExtensionsTests
    {
        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("10 High Street", "10 High Street")]
        [DataRow("10 High Street, London", "10 High Street, London")]
        [DataRow("10 High Street,  London", "10 High Street, London")]
        [DataRow("10 High Street,  London,", "10 High Street, London")]
        [DataRow("10 high Street,  london,", "10 High Street, London")]
        [DataRow("10 High St,  london,", "10 High Street, London")]
        [DataRow("10 Big ave,  london,", "10 Big Avenue, London")]
        [DataRow("10 Ave of the Americas,  london,", "10 Avenue of the Americas, London")]
        [DataRow("10 St John St,  london,", "10 St John Street, London")]
        [DataRow("10 the Willows,  london,", "10 The Willows, London")]
        [DataRow("10 home of the Willows,  london,", "10 Home of the Willows, London")]
        [DataRow("home of the Willows,  london,", "Home of the Willows, London")]
        [DataRow("the Willows,  london,", "The Willows, London")]
        [DataRow("100 4 the Willows,  london,", "100 4 The Willows, London")]
        [DataRow("10 St John St.,  london,", "10 St John Street, London")]
        [DataRow("10 St John St..,  london,", "10 St John Street, London")]
        [DataRow("10 St John St..,  london,London", "10 St John Street, London")]
        [DataRow("10 Ave of_the Americas,  london,", "10 Avenue of the Americas, London")]
        [DataRow("10 Ave of-the Americas,  london,", "10 Avenue of the Americas, London")]
        public void TrimDelimitedAddress(string input, string expected)
        {
            var options = AddressStandardizationOptions.NormalizeWhitespace
                | AddressStandardizationOptions.RemoveHyphens
                | AddressStandardizationOptions.RemoveUnderscores
                | AddressStandardizationOptions.TitleCase
                | AddressStandardizationOptions.StandardizeCommonNames
                | AddressStandardizationOptions.CapitalizeAbbreviations
                | AddressStandardizationOptions.TrimTrailingPunctuation
                | AddressStandardizationOptions.IncludeSpaceAfterDelimiter
                | AddressStandardizationOptions.TrimRepetative;

            var standardizer = new AddressStandardizer(options);

            Assert.AreEqual(expected, AddressStandardizationExtensions.TrimDelimitedAddress(input, ",", options));
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("abc", "abc")]
        [DataRow("abc ", "abc ")]
        [DataRow("abc.", "abc")]
        [DataRow("abc .", "abc ")]
        [DataRow("abc. ", "abc. ")]
        [DataRow("abc , def", "abc , def")]
        [DataRow("abc., def", "abc, def")]
        [DataRow("abc ., def", "abc , def")]
        [DataRow("abc. def.", "abc. def")]
        public void TrimDelimitedTrailingPunctuation(string input, string expected)
        {
            Assert.AreEqual(expected, AddressStandardizationExtensions.TrimDelimitedTrailingPunctuation(input, ","));
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("abc ", "abc")]
        [DataRow("abc, ", "abc")]
        [DataRow("abc, a ", "abc,a")]
        [DataRow("abc ,  a ", "abc,a")]
        [DataRow("abc st. ,  a ", "abc st.,a")]
        [DataRow("west street. ,  a ", "west street.,a")]
        [DataRow("St. John St. ,  a ", "St. John St.,a")]
        [DataRow("10 High Street", "10 High Street")]
        public void TrimDelimitedWhitespace(string input, string expected)
        {
            Assert.AreEqual(expected, AddressStandardizationExtensions.TrimDelimitedWhitespace(input, ","));
        }

        //// Arrange
        //var address = new Address
        //{
        //    StreetAddress = "   123  main_St.,  45 cres_Cr, Oak Cl, Western Rd, ",
        //    City = " london ",
        //    Region = " greater_london",
        //    PostalCode = " W1A 1AA ",
        //    Country = " uk "
        //};

        //var options = AddressStandardizationOptions.TrimWhitespaceAndTrailingPunctuation
        //            | AddressStandardizationOptions.NormalizeWhitespace
        //            | AddressStandardizationOptions.RemoveHyphens
        //            | AddressStandardizationOptions.RemoveWhitespaceAroundHyphens
        //            | AddressStandardizationOptions.TitleCase
        //            | AddressStandardizationOptions.StandardizeCommonNames
        //            | AddressStandardizationOptions.CapitalizeAbbreviations
        //            | AddressStandardizationOptions.TrimTrailingPunctuation;

        //var standardizer = new AddressStandardizer(options);

        //// Act
        //var result = standardizer.Standardize(address);

        //// Assert
        //Assert.AreEqual("123 Main Street, 45 Crescent, Oak Close, Western Road", result.StreetAddress);
        //Assert.AreEqual("London", result.City);
        //Assert.AreEqual("Greater London", result.Region);
        //Assert.AreEqual("W1A 1AA", result.PostalCode);
        //Assert.AreEqual("UK", result.Country);
    }
}