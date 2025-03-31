using Google.Apis.PeopleService.v1.Data;
using GoogleLibrary;
using GoogleServices.GoogleServices;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleContactsReadonlyService
    {
        // Helper method to construct the FormattedValue from structured fields.
        private string ConstructFormattedValue(Address address)
        {
            var components = new List<string>();

            if (!string.IsNullOrWhiteSpace(address.StreetAddress))
            {
                components.Add(address.StreetAddress);
            }

            if (!string.IsNullOrWhiteSpace(address.City))
            {
                components.Add(address.City);
            }

            if (!string.IsNullOrWhiteSpace(address.Region))
            {
                components.Add(address.Region);
            }

            if (!string.IsNullOrWhiteSpace(address.PostalCode))
            {
                components.Add(address.PostalCode);
            }

            if (!string.IsNullOrWhiteSpace(address.Country))
            {
                components.Add(address.Country);
            }

            // Join all non-empty components with a comma and space
            return string.Join(", ", components);
        }

        private Address MergeAddresses(params Address[] addresses)
        {
            var merged = new Address();

            foreach (var address in addresses)
            {
                if (!string.IsNullOrWhiteSpace(address.StreetAddress))
                {
                    merged.StreetAddress = address.StreetAddress;
                }

                if (!string.IsNullOrWhiteSpace(address.City))
                {
                    merged.City = address.City;
                }

                if (!string.IsNullOrWhiteSpace(address.Region))
                {
                    merged.Region = address.Region;
                }

                if (!string.IsNullOrWhiteSpace(address.PostalCode))
                {
                    merged.PostalCode = address.PostalCode;
                }

                if (!string.IsNullOrWhiteSpace(address.Country))
                {
                    merged.Country = address.Country;
                }

                if (!string.IsNullOrWhiteSpace(address.CountryCode))
                {
                    merged.CountryCode = address.CountryCode;
                }

                // If formattedValue exists and the structured data seems incomplete or misformed, use formattedValue to correct
                if (!string.IsNullOrWhiteSpace(address.FormattedValue))
                {
                    // Attempt to split formattedValue into components based on commas
                    var components = address.FormattedValue.Split(',').Select(c => c.Trim()).ToArray();

                    // Check for missing fields and use formattedValue components to fill them
                    if (string.IsNullOrWhiteSpace(merged.StreetAddress) && components.Length > 0)
                    {
                        merged.StreetAddress = components[0];
                    }

                    if (string.IsNullOrWhiteSpace(merged.City) && components.Length > 1)
                    {
                        merged.City = components[1];
                    }

                    // Assuming region comes before country
                    if (string.IsNullOrWhiteSpace(merged.Region) && components.Length > 2)
                    {
                        merged.Region = components[2];
                    }

                    if (string.IsNullOrWhiteSpace(merged.Country) && components.Length > 3)
                    {
                        merged.Country = components[3];
                    }
                }

                // Special logic to handle cases where region contains country (e.g., "Gwynedd, UK")
                if (!string.IsNullOrWhiteSpace(merged.Region) && merged.Region.Contains(","))
                {
                    var regionParts = merged.Region.Split(',').Select(r => r.Trim()).ToArray();
                    merged.Region = regionParts[0]; // Keep the first part as the region
                    if (string.IsNullOrWhiteSpace(merged.Country) && regionParts.Length > 1)
                    {
                        merged.Country = regionParts[1]; // Move the second part to country
                    }
                }
            }

            // Construct the FormattedValue from the corrected structured fields
            merged.FormattedValue = ConstructFormattedValue(merged);

            return merged;
        }

        // Helper to normalize strings by trimming and converting to upper case.
        private string NormalizeString(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToUpper();
        }

        protected static GoogleContactsReadonlyService GoogleContactsReadonlyService { get; set; } = new();

        [TestMethod]
        public void TestGetGoogleContactReadonlyService()
        {
            var service = GoogleContactsReadonlyService;
            var contacts = GoogleContactsReadonlyService.GetContacts();

            //var contact = GoogleContactsReadonlyService.GetContactByResourceName(contacts.First().ResourceName);
            var contact = contacts
                .Where(x => x.Names != null)
                .Where(x => x.Names.FirstOrDefault() != null)
                .Where(x => x.Names.FirstOrDefault()?.GivenName == "Sonia")
                .SingleOrDefault();
            var list = new List<Person>() { contact };
            JsonUtils.SaveToFile(list, @"c:\temp\contact.json");
        }

        [TestMethod]
        public void TestGetGoogleContactsReadonlyService()
        {
            var service = GoogleContactsReadonlyService;
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
            // Arrange: Create three address snapshots with misformed and missing fields based on your examples.
            var address1 = new Address
            {
                City = "Bangor",
                Country = "", // No country in this address
                PostalCode = "LL57 4BL",
                Region = "Gwynedd, UK", // Misformed region containing country
                StreetAddress = "10 Main Street",
                FormattedValue = "10 Main Street, Bangor, Gwynedd, LL57 4BL, UK"
            };

            var address2 = new Address
            {
                City = "Bangor",
                Country = "UK", // Has the country
                PostalCode = "LL57 4BL",
                Region = "", // No region in this address
                StreetAddress = "10 Main Street",
                FormattedValue = "10 Main Street, Bangor, LL57 4BL, UK"
            };

            var address3 = new Address
            {
                City = "Bangor",
                Country = "",
                PostalCode = "", // No postal code in this address
                Region = "Gwynedd",
                StreetAddress = "10 Main Street",
                FormattedValue = "10 Main Street, Bangor, Gwynedd, UK"
            };

            // Act: Merge all three addresses.
            var mergedAddress = MergeAddresses(address1, address2, address3);

            // Assert: Verify the merged address contains the most complete data.
            Assert.AreEqual("Bangor", mergedAddress.City);               // City should be "Bangor"
            Assert.AreEqual("UK", mergedAddress.Country);                // Country should be "UK" from formattedValue or address2
            Assert.AreEqual("LL57 4BL", mergedAddress.PostalCode);       // Postal code should be "LL57 4BL" from address1 or address2
            Assert.AreEqual("Gwynedd", mergedAddress.Region);            // Region should be "Gwynedd", extracted from address1's region or formattedValue
            Assert.AreEqual("10 Main Street", mergedAddress.StreetAddress); // StreetAddress should remain "10 Main Street"

            // Assert: Verify that FormattedValue is constructed correctly after merge
            Assert.AreEqual("10 Main Street, Bangor, Gwynedd, LL57 4BL, UK", mergedAddress.FormattedValue);

            JsonUtils.SaveToFile<Address>(new List<Address>() { mergedAddress }, @"c:\temp\addresses.json");
        }

        [TestMethod]
        public void TestNormalizeAndSaveGoogleContacts()
        {
            // Step 1: Load list from the existing JSON file.
            var contacts = JsonUtils.LoadFromFile<Person>(Path.Combine("c:\\", "temp", "list.json"));

            // Step 2: Normalize addresses by removing duplicates based on key fields, with smart handling for missing fields.
            foreach (var contact in contacts)
            {
                if (contact.Addresses != null && contact.Addresses.Count > 0)
                {
                    // Group addresses by normalized key fields, allowing for empty/missing fields.
                    contact.Addresses = contact.Addresses
                        .GroupBy(a => new
                        {
                            StreetAddress = NormalizeString(a.StreetAddress),
                            City = NormalizeString(a.City),
                            PostalCode = NormalizeString(a.PostalCode),
                            CountryCode = NormalizeString(a.CountryCode)
                        })
                        .Select(g => g.Aggregate((first, second) =>
                        {
                            // Prefer the address with more complete data.
                            return MergeAddresses(first, second);
                        }))
                        .ToList();
                }
            }

            // Step 4: Save the modified list list back to a new JSON file.
            JsonUtils.SaveToFile(contacts, @"c:\temp\contacts_fixed.json");
        }
    }
}