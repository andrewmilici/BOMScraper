using System;
using System.Collections.Generic;
using System.Text;

namespace BOMScraper
{
    public static class StringExtension
    {
        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;
            
            var retVal = "";
            var newWord = true;
            foreach (char c in input)
            {
                if (newWord)
                {
                    retVal += Char.ToUpper(c);
                    newWord = false;
                }
                else
                    retVal += Char.ToLower(c);

                if (c == ' ')
                    newWord = true;
            }

            return retVal;
        }
    }
}
