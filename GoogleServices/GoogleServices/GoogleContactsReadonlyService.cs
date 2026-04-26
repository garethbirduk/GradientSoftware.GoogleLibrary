using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;

namespace GoogleServices.GoogleServices
{
    public class GoogleContactsReadonlyService : GoogleAuthorizationService
    {
        public static List<string> RequiredScopes = new List<string>()
            { PeopleServiceService.Scope.ContactsReadonly };

        public GoogleContactsReadonlyService(params string[] scopes) : base(scopes.Union(RequiredScopes).ToArray())
        {
        }

        /// <summary>
        /// The google people service for accessing the contacts
        /// </summary>
        public PeopleServiceService GooglePeopleService { get; set; }

        public Person GetContactByResourceName(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentException("Person ID cannot be null or empty", nameof(resourceName));
            }

            try
            {
                // Define request to get the specific contact
                PeopleResource.GetRequest request = GooglePeopleService.People.Get(resourceName);
                request.PersonFields = "names,emailAddresses,phoneNumbers,addresses,birthdays,events,genders,photos,occupations,organizations,urls";

                // Execute request and return the contact
                return request.Execute();
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine($"Error fetching contact: {ex.Message}");
                return null;
            }
        }

        public List<Person> GetContacts()
        {
            // List to store all contacts
            var allContacts = new List<Person>();

            string nextPageToken = null;
            do
            {
                // Define request parameters.
                PeopleResource.ConnectionsResource.ListRequest request = GooglePeopleService.People.Connections.List("people/me");
                request.PersonFields = "names,emailAddresses,phoneNumbers,addresses,birthdays,events,genders,photos,occupations,organizations,urls";
                request.PageSize = 100;  // You can change this to a larger value (max is 100)
                request.PageToken = nextPageToken;  // Set the page token to continue where the last page left off

                // Execute the request and get the response
                ListConnectionsResponse response = request.Execute();

                // Add the retrieved contacts to the list
                if (response.Connections != null && response.Connections.Count > 0)
                {
                    allContacts.AddRange(response.Connections);
                }

                // Update the nextPageToken to get the next batch of results
                nextPageToken = response.NextPageToken;
            } while (!string.IsNullOrEmpty(nextPageToken));  // Continue until there are no more pages

            return allContacts;
        }

        public override void SetupExternalServices(BaseClientService.Initializer initializer)
        {
            GooglePeopleService = new PeopleServiceService(initializer);
        }
    }
}