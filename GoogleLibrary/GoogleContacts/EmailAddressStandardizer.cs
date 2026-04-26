using Google.Apis.PeopleService.v1.Data;
using System.Text.RegularExpressions;

namespace GoogleLibrary.GoogleContacts
{
    public class EmailAddressStandardizer
    {
        private readonly EmailAddressStandardizationOptions _options;

        /// <summary>
        /// Basic email validation using a regex pattern.
        /// </summary>
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
        }

        public EmailAddressStandardizer(EmailAddressStandardizationOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Standardizes an email address.
        /// </summary>
        /// <param name="inputEmail">The email address to standardize.</param>
        /// <returns>A standardized EmailAddress object.</returns>
        public EmailAddress Standardize(EmailAddress inputEmail)
        {
            if (inputEmail == null || string.IsNullOrWhiteSpace(inputEmail.Value))
                return inputEmail;

            var email = inputEmail.Value.Trim();

            if (_options.HasFlag(EmailAddressStandardizationOptions.ToLowerCase))
            {
                email = email.ToLowerInvariant();
            }

            if (_options.HasFlag(EmailAddressStandardizationOptions.NormalizeWhitespace))
            {
                email = Regex.Replace(email, @"\s+", ""); // Remove all spaces
            }

            if (_options.HasFlag(EmailAddressStandardizationOptions.TrimTrailingPunctuation))
            {
                email = email.Trim('.', ',', '!', '?', ';', ':');
            }

            return new EmailAddress { Value = email };
        }
    }
}