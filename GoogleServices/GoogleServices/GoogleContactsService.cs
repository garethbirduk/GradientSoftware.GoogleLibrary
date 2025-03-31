using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using GoogleLibrary.GoogleContacts;

namespace GoogleServices.GoogleServices
{
    public class GoogleContactsService : GoogleContactsReadonlyService
    {
        public static List<string> RequiredScopes = new List<string>()
            { PeopleServiceService.Scope.Contacts };

        public GoogleContactsService(params string[] scopes) : base(scopes.Union(RequiredScopes).ToArray())
        {
        }

        public string BuildUpdatePersonFields(ContactUpdateFields fields)

        {
            var fieldMappings = new Dictionary<ContactUpdateFields, string>
            {
                { ContactUpdateFields.Addresses, "addresses" },
                { ContactUpdateFields.Biographies, "biographies" },
                { ContactUpdateFields.Birthdays, "birthdays" },
                { ContactUpdateFields.CalendarUrls, "calendarUrls" },
                { ContactUpdateFields.ClientData, "clientData" },
                { ContactUpdateFields.EmailAddresses, "emailAddresses" },
                { ContactUpdateFields.Events, "events" },
                { ContactUpdateFields.ExternalIds, "externalIds" },
                { ContactUpdateFields.Genders, "genders" },
                { ContactUpdateFields.ImClients, "imClients" },
                { ContactUpdateFields.Interests, "interests" },
                { ContactUpdateFields.Locales, "locales" },
                { ContactUpdateFields.Locations, "locations" },
                { ContactUpdateFields.Memberships, "memberships" },
                { ContactUpdateFields.MiscKeywords, "miscKeywords" },
                { ContactUpdateFields.Names, "names" },
                { ContactUpdateFields.Nicknames, "nicknames" },
                { ContactUpdateFields.Occupations, "occupations" },
                { ContactUpdateFields.Organizations, "organizations" },
                { ContactUpdateFields.PhoneNumbers, "phoneNumbers" },
                { ContactUpdateFields.Relations, "relations" },
                { ContactUpdateFields.SipAddresses, "sipAddresses" },
                { ContactUpdateFields.Urls, "urls" },
                { ContactUpdateFields.UserDefined, "userDefined" }
            };

            var selectedFields = fieldMappings
                .Where(pair => (fields & pair.Key) == pair.Key)
                .Select(pair => pair.Value);

            return string.Join(",", selectedFields);
        }

        public Person CreateContact(Person contact)
        {
            try
            {
                var request = GooglePeopleService.People.CreateContact(contact);
                return request.Execute();
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine($"Error fetching contact: {ex.Message}");
                return null;
            }
        }

        public async Task DeleteContactAsync(Person contact)
        {
            try
            {
                var request = GooglePeopleService.People.DeleteContact(contact.ResourceName);
                await request.ExecuteAsync();
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine($"Error fetching contact: {ex.Message}");
                await Task.CompletedTask;
            }
        }

        public override void SetupExternalServices(BaseClientService.Initializer initializer)
        {
            GooglePeopleService = new PeopleServiceService(initializer);
        }

        public Person UpdateContact(Person contact, ContactUpdateFields fieldsToUpdate)
        {
            if (fieldsToUpdate == ContactUpdateFields.None)
            {
                Console.WriteLine("No fields to update.");
                return null;
            }

            try
            {
                var request = GooglePeopleService.People.UpdateContact(contact, contact.ResourceName);
                request.UpdatePersonFields = BuildUpdatePersonFields(fieldsToUpdate);
                return request.Execute();
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine($"Error updating contact: {ex.Message}");
                return null;
            }
        }
    }
}