using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatClipV2
{
    internal class CoverManager
    {
        public static void AskForCover()
        {
            variable.ask_cover_phase = true;
            // ask if want to create a PDF
            Console.Write("Do you want to create and overwrite PDF on path, before exit? [Y yes* or N X no]: ");
        }

        public static string[] ReadCover(string str)
        {
            int startIndex = FindStartIndex(str);
            int endIndex = FindEndIndex(str);
            string trimmedText = GetTrimmedText(str, startIndex, endIndex);
            string[] paragraphs = SplitIntoParagraphs(trimmedText);
            string[] nonEmptyParagraphs = RemoveEmptyParagraphs(paragraphs);
            return nonEmptyParagraphs;
        }
        static int FindStartIndex(string str)
        {
            foreach (string salutation in CONST.COVER_SALUTATION)
            {
                Regex regex = new Regex(salutation, RegexOptions.IgnoreCase); // Add RegexOptions.IgnoreCase to make the regex case-insensitive);
                Match match = regex.Match(str);

                if (match.Success)
                {
                    int startIndex = match.Index;
                    return str.IndexOf("\n", startIndex);
                }
            }

            return 0; // Default start index if no salutation found
        }

        static int FindEndIndex(string str)
        {
            int endIndex = str.Length;

            foreach (string closing in CONST.COVER_CLOSING)
            {
                Regex regex = new Regex(closing, RegexOptions.IgnoreCase); // Add RegexOptions.IgnoreCase to make the regex case-insensitive
                Match match = regex.Match(str);

                if (match.Success)
                {
                    endIndex = match.Index;
                    break; // Break the loop when the first closing is found
                }
            }

            return endIndex;
        }

        static string GetTrimmedText(string str, int startIndex, int endIndex)
        {
            string trimmedText = str.Substring(startIndex, endIndex - startIndex).Trim();
            return trimmedText;
        }

        static string[] SplitIntoParagraphs(string str)
        {
            string[] paragraphs = str.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return paragraphs;
        }

        static string[] RemoveEmptyParagraphs(string[] paragraphs)
        {
            List<string> nonEmptyParagraphs = new List<string>(paragraphs);
            nonEmptyParagraphs.RemoveAll(s => string.IsNullOrEmpty(s.Trim()));
            return nonEmptyParagraphs.ToArray();
        }

    }
}
