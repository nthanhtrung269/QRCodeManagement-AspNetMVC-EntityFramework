using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRCodeManagement.Extensions
{
    public static class StringExtention
    {
        public static string RemoveNonAlphaNumeric(this string input, int maxCharCount = -1)
        {
            string returnValue = input;
            if (!string.IsNullOrEmpty(input))
            {
                returnValue = string.Concat(input.Where(char.IsLetterOrDigit));

                if (maxCharCount > 0 && returnValue.Length > maxCharCount)
                {
                    returnValue = returnValue.Substring(0, maxCharCount);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Returns string with all non-numeric characters removed
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.String.</returns>
        public static string RemoveNonNumeric(this string s)
        {
            return string.IsNullOrEmpty(s) ? s : string.Concat(s.Where(char.IsDigit));
        }

    }
}