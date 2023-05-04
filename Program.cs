// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using iniFileIO;
using System.IO;
using System.Runtime.InteropServices;

namespace ChatClipV2;

class Program
{

    bool stop_output_thread = false;
    int clipBoards_index;

    static List<string> prompt_Lines = new List<string>();
    static List<int> before_Locs = new List<int>();
    static List<string> clipBoards = new List<string>();

    static Dictionary<string, string[]> COMMANDS = new Dictionary<string, string[]>()
    {
        {"Cover Letter", new string[] {"cover", "c"}},
        {"Exit", new string[] {"exit", "x"}}
    };

    const string VER = "0.6b";
    const string COMMENT_SYMBOL = "//"; // in ini files
    const string FILES_TO_READ = "\\files.ini";


    static string ExePath()
    {
        return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    }

    public static string remove_Comment_Section(string str)
    {
        int pos = str.IndexOf(COMMENT_SYMBOL);
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
    /// <summary>
    /// Places the given window in the system-maintained clipboard format listener list.
    /// </summary>
    delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll", SetLastError = true, EntryPoint = "CreateWindowEx")]
    public static extern IntPtr CreateWindowEx(
           int dwExStyle,
           UInt16 regResult,
           //string lpClassName,
           string lpWindowName,
           UInt32 dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

    void ThreadTask()
    {
        IntPtr hwnd = CreateWindowEx(0, "STATIC", "", 0, 0, 0, 0, 0, HWND_MESSAGE, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
    }

    static void Main(string[] args)
    {
        Console.WriteLine("List of commands: " + VER);
        for (int i = 1; i <= 9; ++i)
        {
            string str = "";

            // read for [Files] category 

            str = iniFIle.Read(ExePath() + FILES_TO_READ, "Files", i.ToString());
            if (!string.IsNullOrEmpty(str))
            {
                prompt_Lines.Add(read_Text_file(ExePath() + "\\" +
                    remove_Comment_Section(str)));
            }

            // read for [Before] category 
            str = iniFIle.Read(ExePath() + FILES_TO_READ, "Before", i.ToString());
            if (!string.IsNullOrEmpty(str))
            {
                //  minus 1 because i'm using zero-indexed but my ini file is in one-based
                before_Locs.Add(int.Parse(remove_Comment_Section(str)) - 1);
                clipBoards.Add("");
            }
        }

        List<string> triggers = new List<string>();
        foreach (KeyValuePair<string, string[]> x in COMMANDS)
        {
            Console.WriteLine(x.Key + ": [" + x.Value[1] + "]" + x.Value[0]);
            triggers.Add(x.Value[0]);
            triggers.Add(x.Value[1]);
        }

        string? input_str = "";

        ////Thread t = new Thread(output_thread);
        ////t.Start();
        //Thread trd = new Thread(new ThreadStart(ThreadTask));
        //trd.IsBackground = true;
        //trd.Start();


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


