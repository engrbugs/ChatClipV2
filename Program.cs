using clipboardIO;
using ClipboardUpdate;
using iniFileIO;
using PDFIO;
using System.Text;
using System.Text.RegularExpressions;


namespace ChatClipV2;


class Program
{


    static bool exit_called = false;
    static int clipBoards_index = 0;

    static List<string> prompt_Lines = new List<string>();
    static List<int> before_Locs = new List<int>();
    static List<string> clipBoards = new List<string>();

    static string ExePath()
    {
        return Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().Location);
    }

    public static string remove_Comment_Section(string str)
    {
        int pos = str.IndexOf(CONST.COMMENT_SYMBOL);
        if (pos != -1)
            return str.Substring(0, pos).Trim();
        else
            return str.Trim();
    }

    static void CreateAPrompt()
    {
        StringBuilder promptBuilder = new StringBuilder();
        clipBoards_index = 0;

        for (int i = 0; i < prompt_Lines.Count; i++)
        {
            bool exists = before_Locs.Contains(i);

            if (exists)
            {
                promptBuilder.Append(clipBoards[clipBoards_index]);
                clipBoards_index++;
            }

            promptBuilder.Append(prompt_Lines[i]);
        }

        string prompt = promptBuilder.ToString();
        clipboard.SetClipboardText(prompt);
        Console.WriteLine("Prompt saved in clipboard.");
    }


    private static void CheckClipboardUpdate(Object state)
    {
        while (clipBoards_index != clipBoards.Count)
        {
            if (exit_called)
            {

                // Stop everything and return from the function
                return;
            }
            if (clipboard_update.check_clipboardupdate())
            {
                clipBoards_index++;
                Console.WriteLine($"Clipboard saved ({clipBoards_index}/{clipBoards.Count}).");
                clipBoards[clipBoards_index - 1] = clipboard.GetClipboardText();
                Console.Write(CONST.CONSOLE_MAIN_NAME);
                Thread.Sleep(1000);
            }
        }
        if (exit_called != true)
        {
            exit_called = true;
            CreateAPrompt();
            AskForCover();
        }

    }

    static void Revert()
    {
        clipBoards_index--;
    }

    static bool ask_cover_phase = false;
    static void AskForCover()
    {
        ask_cover_phase = true;
        // ask if want to create a PDF
        Console.Write("Do you want to create and overwrite PDF on path, before exit? [Y yes* or N X no]: ");

    }




    static string[] SeparateParagraphs(string str)
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


    static void Main(string[] args)
    {
        Console.WriteLine("List of commands: " + CONST.VER);

        ReadFiles();

        DisplayCommands(CONST.COMMANDS);
        DisplayCommands(CONST.EXIT_COMMANDS);

        Thread clipboardThread = new Thread(CheckClipboardUpdate);
        clipboardThread.IsBackground = true;
        clipboardThread.Start();
        string input;
        while (true)
        {
            input = GetUserInput();

            if (ask_cover_phase)
            {
                break;
            }
            else if (input == "revert")
            {
                Revert();
                Console.WriteLine($"Index reduced ({clipBoards_index}/{clipBoards.Count}).");
            }
            else if (IsExitCommand(input))
            {
                break;
            }
        }

        string cover;
        input = input.ToLower().Trim();

        if (ShouldGeneratePDF(input))
        {
            Console.Write("Will read the cover context from the clipboard [Press ENTER]");
            Console.ReadLine();
            cover = clipboard.GetClipboardText();
            string[] paragraphs = SeparateParagraphs(cover);
            createPDF.create_PDF(paragraphs);
        }

        clipboardThread.Join(1000);
    }

    static void ReadFiles()
    {
        for (int i = 1; i <= 9; ++i)
        {
            string file = iniFIle.Read(ExePath() + CONST.FILES_TO_READ, "Files", i.ToString());
            if (!string.IsNullOrEmpty(file))
            {
                string promptLine = File.ReadAllText((ExePath() + "\\" + remove_Comment_Section(file)));
                prompt_Lines.Add(promptLine);
            }

            string before = iniFIle.Read(ExePath() + CONST.FILES_TO_READ, "Before", i.ToString());
            if (!string.IsNullOrEmpty(before))
            {
                int beforeLoc = int.Parse(remove_Comment_Section(before)) - 1;
                before_Locs.Add(beforeLoc);
                clipBoards.Add("");
            }
        }
    }

    static void DisplayCommands(Dictionary<string, string[]> commands)
    {
        foreach (KeyValuePair<string, string[]> command in commands)
        {
            Console.WriteLine(command.Key + ": [" + command.Value[1] + "]" + command.Value[0]);
        }
    }

    static string GetUserInput()
    {
        Console.Write(CONST.CONSOLE_MAIN_NAME);
        return Console.ReadLine().Trim().ToLower();
    }


    static bool IsExitCommand(string input)
    {
        return input == "x" || input == "exit";
    }

    static bool ShouldGeneratePDF(string input)
    {
        return string.IsNullOrEmpty(input) || input == "y" || input == "yes";
    }
}


