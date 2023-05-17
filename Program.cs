using clipboardIO;
using ClipboardUpdate;
using iniFileIO;
using PDFIO;
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

    static void create_a_Prompt()
    {
        string prompt = "";
        clipBoards_index = 0;
        for (int i = 0; i <= prompt_Lines.Count - 1; i++)
        {
            bool exists = before_Locs.Contains(i);
            if (exists)
            {
                prompt += clipBoards[clipBoards_index];
                clipBoards_index++;
            }
            prompt += prompt_Lines[i];
        }
        clipboard.SetClipboardText(prompt);
        Console.WriteLine($"Prompt saved in clipboard.");

    }

    private static void checkClipboardUpdateThread(Object state)
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
            create_a_Prompt();
            ask_for_cover();
        }

    }

    static void revert()
    {
        clipBoards_index--;
    }

    static bool ask_cover_phase = false;
    static void ask_for_cover()
    {
        ask_cover_phase = true;
        // ask if want to create a PDF
        Console.Write("Do you want to create and overwrite PDF on path, before exit? [Y yes* or N X no]: ");

    }




    static string[] separated_para(string str)
    {
    int startIndex = 0;
    foreach (string salutation in CONST.COVER_SALUTATION)
    {
        Regex regex = new Regex(salutation);
        Match match = regex.Match(str);

        if (match.Success)
        {
            startIndex = match.Index;
            break; // Break the loop when the first salutation is found
        }
    }

    int nIndex_afterStart = str.IndexOf("\n", startIndex);


        int endIndex = str.Length();
        foreach (string closing in CONST.COVER_CLOSING)
        {
            Regex regex = new Regex(closing);
            Match match = regex.Match(str);

            if (match.Success)
            {
                endIndex = match.Index + match.Length;
                break; // Break the loop when the first closing is found
            }
        }


        string trimmedText = str.Substring(nIndex_afterStart, endIndex - nIndex_afterStart).Trim();
        // Console.WriteLine(trimmedText.Trim());

        // Console.WriteLine("#########################################");

        string[] para = trimmedText.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        List<string> paraList = new List<string>(para);

        paraList.RemoveAll(s => string.IsNullOrEmpty(s.Trim()));

        para = paraList.ToArray();
        //foreach (string paragraph in para)
        //{
        //    Console.WriteLine(paragraph);
        //}
        return para;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("List of commands: " + CONST.VER);
        for (int i = 1; i <= 9; ++i)
        {
            string str = "";

            // read for [Files] category 
            str = iniFIle.Read(ExePath() + CONST.FILES_TO_READ, "Files", i.ToString());
            if (!string.IsNullOrEmpty(str))
            {
                prompt_Lines.Add(File.ReadAllText((ExePath() + "\\" +
                    remove_Comment_Section(str))));
            }

            // read for [Before] category 
            str = iniFIle.Read(ExePath() + CONST.FILES_TO_READ, "Before", i.ToString());
            if (!string.IsNullOrEmpty(str))
            {
                //  minus 1 because i'm using zero-indexed but my ini file is in one-based
                before_Locs.Add(int.Parse(remove_Comment_Section(str)) - 1);
                clipBoards.Add("");
            }
        }

        List<string> triggers = new List<string>();
        foreach (KeyValuePair<string, string[]> x in CONST.COMMANDS)
        {
            Console.WriteLine(x.Key + ": [" + x.Value[1] + "]" + x.Value[0]);
            triggers.Add(x.Value[0]);
            triggers.Add(x.Value[1]);
        }
        foreach (KeyValuePair<string, string[]> x in CONST.EXIT_COMMANDS)
        {
            Console.WriteLine(x.Key + ": [" + x.Value[1] + "]" + x.Value[0]);
        }

        string? input_str = "";

        Thread t = new Thread(checkClipboardUpdateThread);
        t.IsBackground = true;
        t.Start();


        while (input_str != "x" && input_str != "exit")
        {
            if (ask_cover_phase)
            {
                break;
            }
            else if (input_str == "revert")
            {
                revert();
                Console.WriteLine($"Index reduced ({clipBoards_index}/{clipBoards.Count}).");
            }

            Console.Write(CONST.CONSOLE_MAIN_NAME);
            input_str = Console.ReadLine().Trim().ToLower();

        }

        string cover;
        if (input_str.ToLower().Trim() == "" ||
        input_str.ToLower().Trim() == "y" ||
        input_str.ToLower().Trim() == "yes")
        {
            Console.Write("Will read on clipboard for the cover context [Press ENTER]");
            Console.ReadLine();
            //createPDF.create_PDF("hello");
            cover = clipboard.GetClipboardText();
            string[] paras = separated_para(cover);
            createPDF.create_PDF(paras);
        }

        t.Join(1000);

        return;
    }

}


