using System;
using System.Linq;

namespace Ed_Fi.Credential.Business.Common.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("input was null or empty", "input");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

       
    }
}
