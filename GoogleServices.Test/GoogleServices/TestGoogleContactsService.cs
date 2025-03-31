using Google.Apis.PeopleService.v1.Data;
using GoogleLibrary;
using GoogleLibrary.GoogleContacts;
using GoogleServices.CustomServices;
using GoogleServices.GoogleServices;
using Newtonsoft.Json;

namespace GoogleServices.Test.GoogleServices
{
    [TestClass]
    public class TestGoogleContactsService
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
        protected static GoogleContactsService GoogleContactsService { get; set; } = new();

        public static List<Person> LoadContactsFromJson(string filePath)
        {
            // Ensure the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            // Read the JSON file content
            var json = File.ReadAllText(filePath);

            // Deserialize the JSON content into a list of Person objects
            return JsonConvert.DeserializeObject<List<Person>>(json);
        }

        [TestMethod]
        public void FixLists()
        {
            var contacts_current = JsonUtils.LoadFromFile<Person>(Path.Combine("c:\\", "temp", "list.json"));
            var contacts_git = LoadContactsFromJson(Path.Combine("c:\\", "Git", "contacts.json"));

            foreach (var contact_current in contacts_current)
            {
                if (contact_current.Names != null)
                {
                    var name = contact_current.Names.First().DisplayName;
                    var contact_git = contacts_git.SingleOrDefault(x =>
                        x.Names != null
                        && x.Names.First().DisplayName.Equals(name));

                    if (contact_git != null)
                    {
                        contact_current.Addresses = contact_git.Addresses;
                    }
                }
            }

            JsonUtils.SaveToFile(contacts_current, Path.Combine("c:\\", "temp", "list2.json"));

            new CustomContactsService().CleanupContacts(contacts_current);
        }

        [TestMethod]
        public async Task TestCreateDeleteContact()
        {
            var contact = new Person()
            {
                Names = new[]
                {
                    new Name()
                    {
                        GivenName = "Bob"
                    }
                },
                EmailAddresses = new[]
                {
                    new EmailAddress()
                    {
                        FormattedType = "Home",
                        Value = "tenuki@tenuki.nl"
                    }
                }
            };

            var contact2 = GoogleContactsService.CreateContact(contact);
            Assert.IsTrue(contact.ResourceName != contact2.ResourceName);

            await GoogleContactsService.DeleteContactAsync(contact2);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            GoogleContactsReadonlyService.Initialize();
            GoogleContactsService.Initialize();
        }

        [TestMethod]
        public async Task TestUpdateContact()
        {
            var contact = new Person()
            {
                Names = new[]
                {
                    new Name()
                    {
                        GivenName = "DeleteMe"
                    }
                },
                EmailAddresses = new[]
                {
                    new EmailAddress()
                    {
                        FormattedType = "Home",
                        Value = "tenuki@tenuki.nl"
                    }
                }
            };

            Person? contact2 = null;
            try
            {
                contact2 = GoogleContactsService.CreateContact(contact);
                Assert.IsTrue(contact.ResourceName != contact2.ResourceName);

                contact2.Names.First().FamilyName = "Smith";
                GoogleContactsService.UpdateContact(contact2, ContactUpdateFields.Names);
            }
            finally
            {
                if (contact2 != null)
                {
                    await GoogleContactsService.DeleteContactAsync(contact2);
                }
            }
        }

        [TestMethod]
        public async Task TestUpdateContact2()
        {
            var contact = GoogleContactsService.GetContactByResourceName("people/c41931351444101877");
            var service = new CustomContactsService();
            service.CleanupContacts(new List<Person>()
            {
                contact,
            });
        }
    }
}