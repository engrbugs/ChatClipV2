﻿// See https://aka.ms/new-console-template for more information

using clipboardIO;
using ClipboardUpdate;
using iniFileIO;


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
        Console.Write(CONST.CONSOLE_MAIN_NAME);
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
            create_a_Prompt();
        }
    }

    static void revert()
    {
        clipBoards_index--;
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
            if (input_str == "revert")
            {
                revert();
                Console.WriteLine($"Index reduced ({clipBoards_index}/{clipBoards.Count}).");
            }

            Console.Write(CONST.CONSOLE_MAIN_NAME);
            input_str = Console.ReadLine().Trim().ToLower();
        }

        // Stop the clipboard monitoring thread
        //stop_output_thread = true;

        exit_called = true;
        t.Join(1000);

        return;
    }
}


