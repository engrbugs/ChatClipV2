using ChatClipV2.Features;
using ChatClipV2.Utils;
using System.Text;
using System.Text.RegularExpressions;


namespace ChatClipV2;


class Program
{
    static void Revert()
    {
        variable.clipBoards_index--;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("List of commands: " + CONST.VER);

        FileReader.ReadFiles();

        CommandManager.DisplayCommands(CONST.COMMANDS);
        CommandManager.DisplayCommands(CONST.EXIT_COMMANDS);

        Thread clipboardThread = new Thread(ClipboardManager.CheckClipboardUpdate);
        clipboardThread.IsBackground = true;
        clipboardThread.Start();
        string input;
        while (true)
        {
            input = CommandManager.GetUserInput();

            if (variable.ask_cover_phase)
            {
                break;
            }
            else if (input == "revert")
            {
                Revert();
                Console.WriteLine($"Index reduced ({variable.clipBoards_index}" +
                    $"/{variable.clipBoards.Count}).");
            }
            else if (CommandManager.IsExitCommand(input))
            {
                break;
            }
        }
        clipboardThread.Join(1000);
        string cover;
        input = input.ToLower().Trim();

        if (CommandManager.ShouldGeneratePDF(input))
        {
            Console.Write("Will read the cover context from the clipboard [Press ENTER]");
            Console.ReadLine();
            cover = clipboard.GetClipboardText();
            string[] paragraphs = CoverManager.ReadCover(cover);

            DataTypes.SuccessReturn PDFSuccess = new DataTypes.SuccessReturn();

            while (!PDFSuccess) 
            {
                PDFSuccess = createPDF.CreatePDF(paragraphs);
                if (!PDFSuccess)
                {
                    Console.Write($"{PDFSuccess.ErrorMessage} [Press ENTER to retry or N NO to exit]");
                    string? PDFRetryCMD = Console.ReadLine();
                    if (PDFRetryCMD.ToLower().Trim() == "n" || 
                        PDFRetryCMD.ToLower().Trim() == "no") break;
                }
            } 
        }


    }

    

}


