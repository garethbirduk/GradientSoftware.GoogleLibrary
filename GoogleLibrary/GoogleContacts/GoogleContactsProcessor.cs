using Google.Apis.PeopleService.v1.Data;
using GoogleLibrary;
using GoogleLibrary.GoogleContacts;

public class GoogleContactsProcessor
{
    private static List<Person> NormalizeContacts(List<Person> contacts)
    {
        var normalizedContacts = new List<Person>();
        var addressDictionary = new Dictionary<string, Address>(); // Track unique addresses
        var phoneNumberDictionary = new Dictionary<string, string>(); // Track unique phone numbers

        foreach (var person in contacts)
        {
            var normalizedPerson = new Person
            {
                Names = person.Names
            };

            // Normalize phone numbers
            if (person.PhoneNumbers != null)
            {
                normalizedPerson.PhoneNumbers = person.PhoneNumbers
                    .Select(phone => NormalizePhoneNumber(phone, phoneNumberDictionary))
                    .ToList();
            }

            // Normalize addresses
            if (person.Addresses != null)
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

                normalizedPerson.Addresses = person.Addresses
                    .Select(address => standardizer.Standardize(address))
                    .ToList();
            }

            normalizedContacts.Add(normalizedPerson);
        }

        return normalizedContacts;
    }

    private static PhoneNumber NormalizePhoneNumber(PhoneNumber phoneNumber, Dictionary<string, string> phoneNumberDictionary)
    {
        var normalizedPhone = phoneNumber.Value;

        // Check if the phone number has already been normalized
        if (phoneNumberDictionary.ContainsKey(phoneNumber.Value))
        {
            normalizedPhone = phoneNumberDictionary[phoneNumber.Value];
        }
        else
        {
            // Add to dictionary for future normalization
            phoneNumberDictionary[phoneNumber.Value] = phoneNumber.Value;
        }

        return new PhoneNumber { Value = normalizedPhone };
    }

    public void ProcessContacts()
    {
        // Step 1: Load contacts from JSON file
        var contacts = JsonUtils.LoadFromFile<Person>(Path.Combine("c:\\", "temp", "list.json"));

        // Step 2: Normalize contacts
        var normalizedContacts = NormalizeContacts(contacts);

        // Step 3: Export normalized contacts to JSON file
        JsonUtils.SaveToFile(normalizedContacts, @"c:\temp\contacts_fixed.json");

        Console.WriteLine("Normalized contacts have been saved to c:\\temp\\contacts_fixed.json");
    }
}