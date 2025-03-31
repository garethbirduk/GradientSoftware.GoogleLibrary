using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using GoogleLibrary.GoogleContacts;
using GoogleServices.GoogleServices;

namespace GoogleServices.CustomServices
{
    public class CustomContactsService : GoogleAuthorizationService
    {
        public static List<string> RequiredScopes =
            GoogleContactsService.RequiredScopes
            .ToList();

        public CustomContactsService() : base(RequiredScopes)
        {
            GoogleContactsService.Initialize();
        }

        public GoogleContactsService GoogleContactsService { get; } = new();

        public void CleanupContacts()
        {
            CleanupContacts(GoogleContactsService.GetContacts());
        }

        public void CleanupContacts(IEnumerable<Person> contacts)
        {
            foreach (var contact in contacts)
            {
                ContactUpdateFields fieldsToUpdate = ContactUpdateFields.None;

                if (contact?.Addresses != null)
                {
                    var options = AddressStandardizationOptions.NormalizeWhitespace
                        | AddressStandardizationOptions.RemoveHyphens
                        | AddressStandardizationOptions.RemoveUnderscores
                        | AddressStandardizationOptions.RemoveWhitespaceAroundHyphens
                        | AddressStandardizationOptions.TitleCase
                        | AddressStandardizationOptions.StandardizeCommonNames
                        | AddressStandardizationOptions.CapitalizeAbbreviations
                        | AddressStandardizationOptions.TrimTrailingPunctuation
                        | AddressStandardizationOptions.IncludeSpaceAfterDelimiter
                        | AddressStandardizationOptions.TrimRepetative;
                    var standardizer = new AddressStandardizer(options);
                    var addresses = new List<Address>();
                    foreach (var address in contact.Addresses)
                    {
                        addresses.Add(standardizer.Standardize(address));
                        fieldsToUpdate |= ContactUpdateFields.Addresses;
                    }
                    contact.Addresses = addresses;
                }

                if (contact?.EmailAddresses != null)
                {
                    var options = EmailAddressStandardizationOptions.NormalizeWhitespace
                        | EmailAddressStandardizationOptions.ToLowerCase
                        | EmailAddressStandardizationOptions.TrimTrailingPunctuation;
                    var standardizer = new EmailAddressStandardizer(options);
                    var emailsAddresses = new List<EmailAddress>();
                    foreach (var emailAddress in contact.EmailAddresses)
                    {
                        emailsAddresses.Add(standardizer.Standardize(emailAddress));
                        fieldsToUpdate |= ContactUpdateFields.EmailAddresses;
                    }
                    contact.EmailAddresses = emailsAddresses;
                }

                if (fieldsToUpdate != ContactUpdateFields.None)
                {
                    GoogleContactsService.UpdateContact(contact, fieldsToUpdate);
                }
            }
        }

        public override void SetupExternalServices(BaseClientService.Initializer initializer)
        {
        }
    }
}