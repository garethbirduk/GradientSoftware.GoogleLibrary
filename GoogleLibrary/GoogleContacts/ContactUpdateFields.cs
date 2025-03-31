namespace GoogleLibrary.GoogleContacts
{
    [Flags]
    public enum ContactUpdateFields
    {
        None = 0,
        Addresses = 1 << 0,
        Biographies = 1 << 1,
        Birthdays = 1 << 2,
        CalendarUrls = 1 << 3,
        ClientData = 1 << 4,
        EmailAddresses = 1 << 5,
        Events = 1 << 6,
        ExternalIds = 1 << 7,
        Genders = 1 << 8,
        ImClients = 1 << 9,
        Interests = 1 << 10,
        Locales = 1 << 11,
        Locations = 1 << 12,
        Memberships = 1 << 13,
        MiscKeywords = 1 << 14,
        Names = 1 << 15,
        Nicknames = 1 << 16,
        Occupations = 1 << 17,
        Organizations = 1 << 18,
        PhoneNumbers = 1 << 19,
        Relations = 1 << 20,
        SipAddresses = 1 << 21,
        Urls = 1 << 22,
        UserDefined = 1 << 23,

        // Shortcut for common updates (Example)
        BasicInfo = Names | Nicknames | EmailAddresses | PhoneNumbers,

        All = Addresses | Biographies | Birthdays | CalendarUrls | ClientData | EmailAddresses | Events |
              ExternalIds | Genders | ImClients | Interests | Locales | Locations | Memberships | MiscKeywords |
              Names | Nicknames | Occupations | Organizations | PhoneNumbers | Relations | SipAddresses | Urls | UserDefined
    }
}