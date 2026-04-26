using Google.Apis.PeopleService.v1.Data;
using GoogleLibrary.GoogleContacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoogleLibrary.Test.GoogleContacts
{
    [TestClass]
    public class AddressStandardizerTests
    {
        [TestMethod]
        public void Standardize()
        {
            var address = new Address()
            {
                StreetAddress = "15 St. John's st., Glendale park,",
                City = "Bloxwich, Walsall",
                Region = "Staffordshire",
                Country = "UK",
                PostalCode = "BH22 4LX",
            };

            var options = AddressStandardizationOptions.NormalizeWhitespace
                | AddressStandardizationOptions.RemoveHyphens
                | AddressStandardizationOptions.RemoveWhitespaceAroundHyphens
                | AddressStandardizationOptions.TitleCase
                | AddressStandardizationOptions.StandardizeCommonNames
                | AddressStandardizationOptions.CapitalizeAbbreviations
                | AddressStandardizationOptions.TrimTrailingPunctuation
                | AddressStandardizationOptions.IncludeSpaceAfterDelimiter
                | AddressStandardizationOptions.TrimRepetative;

            var standardizer = new AddressStandardizer(options);

            var output = standardizer.Standardize(address);

            File.WriteAllText("c:\\temp\\output.json", Newtonsoft.Json.JsonConvert.SerializeObject(output));
        }
    }
}