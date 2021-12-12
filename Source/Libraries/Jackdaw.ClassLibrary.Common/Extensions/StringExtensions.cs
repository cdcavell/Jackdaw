using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    /// <summary>
    /// Extension methods for existing string types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 0.0.0.1 | 12/11/2021 | Initial Development |~ 
    /// </revision>
    public static class StringExtensions
    {
        /// <summary>
        /// Method to determine if string is a valid email address
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>bool</returns>
        /// <method>IsValidEmail(this string value)</method>
        public static bool IsValidEmail(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                // Normalize the domain
                value = Regex.Replace(value, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(value,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to remove "Carriage Return" and "Line Feed" as well as Html filtering to provide proper neutralization.
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>string</returns>
        /// <method>Clean(this string value)</method>
        public static string Clean(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            string cleanString = HttpUtility.HtmlEncode(value.Replace("\r", string.Empty).Replace("\n", string.Empty));
            return string.Format("{0}", cleanString);
        }

        /// <summary>
        /// Method to determine if string is a valid Guid
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>bool</returns>
        /// <method>IsValidGuid(this string value)</method>
        public static bool IsValidGuid(this string value)
        {
            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
            bool isValid = false;

            if (!string.IsNullOrEmpty(value))
                if (isGuid.IsMatch(value))
                    isValid = true;

            return isValid;
        }

        /// <summary>
        /// Strip escape slash and beginning/ending quotes from Json result string
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>string</returns>
        /// <method>CleanJsonResult(this string result)</method>
        public static string CleanJsonResult(this string value)
        {
            return value.Replace("\\", string.Empty).Trim(new char[1] { '"' });
        }

        /// <summary>
        /// Strip suffix from end of string
        /// </summary>
        /// <param name="input">this string</param>
        /// <param name="suffixToRemove">string</param>
        /// <param name="comparisonType">StringComparison</param>
        /// <returns>string</returns>
        /// <method>TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)</method>
        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }

            return input;
        }

        /// <summary>
        /// Strip all from end of string starting at first char
        /// </summary>
        /// <param name="input">this string</param>
        /// <param name="startTrim">string</param>
        /// <returns>string</returns>
        /// <method>TrimFromFirstChar(this string input, char startTrim)</method>
        public static string TrimFromFirstChar(this string input, char startTrim)
        {
            int index = input.IndexOf(startTrim);
            if (index > 0)
                input = input.Substring(0, index);

            return input;
        }

        /// <summary>
        /// UTF8 Encoding
        /// </summary>
        /// <param name="input">this string</param>
        /// <returns>string</returns>
        /// <method>UTF8(this string input)</method>
        public static string UTF8(this string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
