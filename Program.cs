// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using iniFileIO;
using System.IO;
using System.Runtime.InteropServices;
using static ChatClipV2.clipboard_update;



namespace ChatClipV2;


class Program
{
    bool stop_output_thread = false;
    int clipBoards_index;

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
        {
            return trim_Both_Ends(str.Substring(0, pos));
        }
        else
        {
            return trim_Both_Ends(str);
        }
    }

    private static string trim_Both_Ends(string str)
    {
        return str.Trim();
    }

    static string read_Text_file(string filePath)
    {
        return File.ReadAllText(filePath);
    }

    private static void checkClipboardUpdateThread(Object state)
    {
        while (true)
        {
            if (clipboard_update.check_clipboardupdate())
            {
                Console.WriteLine("CLIPBOARD CHANGED");
                Thread.Sleep(1000);
            }
        }
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
                prompt_Lines.Add(read_Text_file(ExePath() + "\\" +
                    remove_Comment_Section(str)));
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

        string? input_str = "";

        Thread t = new Thread(checkClipboardUpdateThread);
        t.IsBackground = true;
        t.Start();

        while (!triggers.Contains(input_str))
        {
            Console.Write("main: ");
            input_str = Console.ReadLine();
        }

        // Stop the clipboard monitoring thread
        //stop_output_thread = true;

        //t.Join();

        return;
    }
}


