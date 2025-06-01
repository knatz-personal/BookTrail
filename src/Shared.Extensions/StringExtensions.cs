using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Shared.Extensions
{
    /// <summary>
    ///     Provides extension methods for string manipulation and evaluation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Converts a string to a slug format (URL-friendly).
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A URL-friendly slug string.</returns>
        public static string ToSlug(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Remove diacritics (accents)
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            // Convert to lowercase and replace spaces with hyphens
            string slug = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();

            // Replace non-alphanumeric characters with hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", string.Empty);

            // Replace multiple spaces with a single hyphen
            slug = Regex.Replace(slug, @"\s+", "-");

            // Replace multiple hyphens with a single hyphen
            slug = Regex.Replace(slug, @"-+", "-");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            return slug;
        }

        /// <summary>
        ///     Truncates a string to the specified maximum length and adds an ellipsis if truncated.
        /// </summary>
        /// <param name="input">The string to truncate.</param>
        /// <param name="maxLength">The maximum length of the resulting string, including the ellipsis.</param>
        /// <returns>
        ///     The truncated string with an ellipsis if truncation is applied, or the original string if no truncation is
        ///     necessary.
        /// </returns>
        public static string Truncate(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
            {
                return input;
            }

            return input.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        ///     Checks if a string is a valid email address.
        /// </summary>
        /// <param name="input">The string to check.</param>
        /// <returns>True if the string is a valid email address, otherwise false.</returns>
        public static bool IsValidEmail(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            try
            {
                // Use Regex for simple validation
                Regex regex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(input);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Converts a string to camel case.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>The string in camel case.</returns>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (input.Length == 1)
            {
                return input.ToLowerInvariant();
            }

            return char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        /// <summary>
        ///     Converts a string to title case based on the specified culture.
        /// </summary>
        /// <param name="source">The string to convert to title case.</param>
        /// <param name="culture">The culture to use for title casing. Defaults to "en" (English).</param>
        /// <returns>The string converted to title case.</returns>
        public static string ToTitleCase(this string source, string culture = "en")
        {
            TextInfo myTi = new CultureInfo(culture, false).TextInfo;
            return myTi.ToTitleCase(source.ToLower());
        }

        /// <summary>
        ///     Extracts and returns the initials from a given string.
        /// </summary>
        /// <param name="source">The input string to extract initials from.</param>
        /// <returns>A string containing the initials of the input string.</returns>
        public static string ToInitials(this string source)
        {
            Regex initials = new(@"(\b[a-zA-Z])[a-zA-Z]* ?");

            return initials.Replace(source, "$1");
        }

        /// <summary>
        ///     Converts a string to title case using an invariant culture.
        /// </summary>
        /// <param name="source">The input string to convert to title case.</param>
        /// <returns>A string converted to title case with invariant culture settings.</returns>
        public static string ToTitleCaseInvariant(this string source)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(source.ToLower());
        }

        /// <summary>
        ///     Converts a string to a byte array using UTF8 encoding.
        /// </summary>
        /// <param name="input">The string to convert.</param>
        /// <returns>A byte array representation of the string.</returns>
        public static byte[] ToByteArray(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return [];
            }

            return Encoding.UTF8.GetBytes(input);
        }

        /// <summary>
        ///     Encodes a string into its Base64 representation.
        /// </summary>
        /// <param name="source">The string to encode to Base64.</param>
        /// <returns>The Base64-encoded string.</returns>
        public static string EncodeToBase64String(this string source)
        {
            string s = source.Trim().Replace(" ", "+", StringComparison.OrdinalIgnoreCase);
            s = s.Length % 4 > 0 ? s.PadRight(s.Length + 4 - (s.Length % 4), '=') : s;
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }

        /// <summary>
        ///     Determines whether the provided HTML string is null, empty, or contains only whitespace after parsing the inner
        ///     text.
        /// </summary>
        /// <param name="source">The HTML string to evaluate.</param>
        /// <returns>
        ///     true if the HTML string is null, empty, or contains only whitespace; otherwise, false.
        /// </returns>
        public static bool IsNullOrWhiteSpaceHtml(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return true;
            }

            HtmlDocument document = new();
            document.LoadHtml(source.Trim());
            string textValue = document.DocumentNode.InnerText.Trim();
            bool returnVal = string.IsNullOrWhiteSpace(textValue);
            return returnVal;
        }

        /// <summary>
        ///     Converts an HTML string to plain text by stripping out all HTML tags.
        /// </summary>
        /// <param name="source">The HTML string to convert to plain text.</param>
        /// <returns>A plain text string without HTML tags.</returns>
        public static string ToPlainText(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return string.Empty;
            }

            HtmlDocument document = new();
            document.LoadHtml(source.Trim());
            string textValue = document.DocumentNode.InnerText.Trim();
            return textValue;
        }
    }
}