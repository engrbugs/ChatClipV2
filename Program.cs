using clipboardIO;
using ClipboardUpdate;
using iniFileIO;
using PDFIO;
using System.Runtime.InteropServices;

namespace ChatClipV2;

class Reader
{
    private static Thread inputThread;
    private static AutoResetEvent getInput, gotInput;
    private static string input;
    private static bool stopFlag = false;


    static Reader()
    {
        getInput = new AutoResetEvent(false);
        gotInput = new AutoResetEvent(false);
        inputThread = new Thread(reader);
        inputThread.IsBackground = true;
        inputThread.Start();
    }

    private static void reader()
    {
        while (true)
        {
            getInput.WaitOne();
            input = Console.ReadLine();
            gotInput.Set();
        }
    }

    // omit the parameter to read a line without a timeout
    public static string ReadLine()
    {
        bool success = false;
        getInput.Set();
        while (!stopFlag && !success)
        {
             success = gotInput.WaitOne(1000);
        }
        if (success)
            return input;
        else
            throw new TimeoutException("User did not provide input within the timelimit.");
    }
    public static void StopReading()
    {
        stopFlag = true;
        getInput.Set();
    }
}

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

        Reader.StopReading();

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
        }

    }

    static void revert()
    {
        clipBoards_index--;
    }
    private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        //e.Cancel = true; // cancel the Ctrl+C key press event
        exit_called = true; // set the input string to "x" to exit the loop that reads from the console
    }

    static void Main(string[] args)
    {
        createPDF.create_PDF("hello");
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
        
        while (input_str != "x" && input_str != "exit" && exit_called != true)
        {
            if (input_str == "revert")
            {
                revert();
                Console.WriteLine($"Index reduced ({clipBoards_index}/{clipBoards.Count}).");
            }

            try
            {
                Console.Write(CONST.CONSOLE_MAIN_NAME);
                input_str = Reader.ReadLine().Trim().ToLower();
            }
            catch (TimeoutException e)
            {
                // Handle the timeout exception here...
            }
            catch (Exception e)
            {
                // Handle any other exceptions here...
            }

        }

        // Stop the clipboard monitoring thread
        //stop_output_thread = true;

        exit_called = true;
        t.Join(1000);


        // ask if want to create a PDF
        Console.Write("Do you want to create and overwrite PDF on path, before exit? [Y yes* or N X no]: ");
        string create_pdf = Console.ReadLine().Trim().ToLower();

        if (create_pdf == "" || create_pdf == "y" || create_pdf == "yes")
        {
            createPDF.create_PDF("hello");
        }

        return;
    }
    
}


