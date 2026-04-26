using Google.Apis.PeopleService.v1.Data;
using GoogleLibrary;
using GoogleLibrary.GoogleContacts;

public class GoogleContactsProcessor
{
    private static List<Person> NormalizeContacts(List<Person> contacts)
    {
        var normalizedContacts = new List<Person>();

        foreach (var person in contacts)
        {
            var normalizedPerson = new Person
            {
                Names = person.Names,
                PhoneNumbers = person.PhoneNumbers
            };

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