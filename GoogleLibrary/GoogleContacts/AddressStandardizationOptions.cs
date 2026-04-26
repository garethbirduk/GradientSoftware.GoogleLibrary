namespace GoogleLibrary.GoogleContacts
{
    [Flags]
    public enum AddressStandardizationOptions
    {
        TrimWhitespace = 1,
        NormalizeWhitespace = 2,
        RemoveHyphens = 4,
        RemoveWhitespaceAroundHyphens = 8,
        TitleCase = 16,
        StandardizeCommonNames = 32,
        CapitalizeAbbreviations = 64,
        TrimTrailingPunctuation = 128,
        IncludeSpaceAfterDelimiter = 256,
        TrimRepetative = 512,
        RemoveUnderscores = 1024,
    }

    [Flags]
    public enum EmailAddressStandardizationOptions
    {
        ToLowerCase = 1,
        NormalizeWhitespace = 2,
        TrimTrailingPunctuation = 4,
    }
}