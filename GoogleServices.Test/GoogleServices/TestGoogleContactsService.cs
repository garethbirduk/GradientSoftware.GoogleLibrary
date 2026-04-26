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
        [TestCategory("Manual")]
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
        [TestCategory("Manual")]
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
        [TestCategory("Manual")]
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
        [TestCategory("Manual")]
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