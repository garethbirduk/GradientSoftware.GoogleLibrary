using Google.Apis.PeopleService.v1.Data;
using GoogleLibrary;
using GoogleServices.GoogleServices;
using GoogleServices.Test.Helpers;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleContactsReadonlyService
    {
        protected static GoogleContactsReadonlyService GoogleContactsReadonlyService { get; set; } = new();

        [TestMethod]
        [TestCategory("Manual")]
        public void TestGetGoogleContactReadonlyService()
        {
            var contacts = GoogleContactsReadonlyService.GetContacts();
            var contact = contacts
                .Where(x => x.Names != null)
                .Where(x => x.Names.FirstOrDefault() != null)
                .Where(x => x.Names.FirstOrDefault()?.GivenName == "Sonia")
                .SingleOrDefault();
            var list = new List<Person>() { contact };
            JsonUtils.SaveToFile(list, @"c:\temp\contact.json");
        }

        [TestMethod]
        [TestCategory("Manual")]
        public void TestGetGoogleContactsReadonlyService()
        {
            var list = GoogleContactsReadonlyService.GetContacts();
            JsonUtils.SaveToFile(list, @"c:\temp\list.json");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            GoogleContactsReadonlyService.Initialize();
        }

        [TestMethod]
        public void TestMergeMultipleAddressesWithFormattedValueCorrection()
        {
            var address1 = new Address
            {
                City = "Bangor",
                Country = "",
                PostalCode = "LL57 4BL",
                Region = "Gwynedd, UK",
                StreetAddress = "10 Main Street",
                FormattedValue = "10 Main Street, Bangor, Gwynedd, LL57 4BL, UK"
            };

            var address2 = new Address
            {
                City = "Bangor",
                Country = "UK",
                PostalCode = "LL57 4BL",
                Region = "",
                StreetAddress = "10 Main Street",
                FormattedValue = "10 Main Street, Bangor, LL57 4BL, UK"
            };

            var address3 = new Address
            {
                City = "Bangor",
                Country = "",
                PostalCode = "",
                Region = "Gwynedd",
                StreetAddress = "10 Main Street",
                FormattedValue = "10 Main Street, Bangor, Gwynedd, UK"
            };

            var mergedAddress = AddressMergeHelpers.MergeAddresses(address1, address2, address3);

            Assert.AreEqual("Bangor", mergedAddress.City);
            Assert.AreEqual("UK", mergedAddress.Country);
            Assert.AreEqual("LL57 4BL", mergedAddress.PostalCode);
            Assert.AreEqual("Gwynedd", mergedAddress.Region);
            Assert.AreEqual("10 Main Street", mergedAddress.StreetAddress);
            Assert.AreEqual("10 Main Street, Bangor, Gwynedd, LL57 4BL, UK", mergedAddress.FormattedValue);
        }

        [TestMethod]
        [TestCategory("Manual")]
        public void TestNormalizeAndSaveGoogleContacts()
        {
            var contacts = JsonUtils.LoadFromFile<Person>(Path.Combine("c:\\", "temp", "list.json"));

            foreach (var contact in contacts)
            {
                if (contact.Addresses != null && contact.Addresses.Count > 0)
                {
                    contact.Addresses = contact.Addresses
                        .GroupBy(a => new
                        {
                            StreetAddress = AddressMergeHelpers.NormalizeString(a.StreetAddress),
                            City = AddressMergeHelpers.NormalizeString(a.City),
                            PostalCode = AddressMergeHelpers.NormalizeString(a.PostalCode),
                            CountryCode = AddressMergeHelpers.NormalizeString(a.CountryCode)
                        })
                        .Select(g => g.Aggregate((first, second) => AddressMergeHelpers.MergeAddresses(first, second)))
                        .ToList();
                }
            }

            JsonUtils.SaveToFile(contacts, @"c:\temp\contacts_fixed.json");
        }
    }
}
