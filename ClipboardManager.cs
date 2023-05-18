using ChatClipV2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClipV2
{
    internal class ClipboardManager
    {
        public static void CheckClipboardUpdate(Object state)
        {
            while (variable.clipBoards_index != variable.clipBoards.Count)
            {
                if (variable.exit_called)
                {

                    // Stop everything and return from the function
                    return;
                }
                if (ClipboardUpdateModule.check_clipboardupdate())
                {
                    variable.clipBoards_index++;
                    Console.WriteLine($"Clipboard saved ({variable.clipBoards_index}/" +
                        $"{variable.clipBoards.Count}).");
                    variable.clipBoards[variable.clipBoards_index - 1] = clipboard.GetClipboardText();
                    Console.Write(CONST.CONSOLE_MAIN_NAME);
                    Thread.Sleep(1000);
                }
            }
            if (variable.exit_called != true)
            {
                variable.exit_called = true;
                PromptManager.CreateAPrompt();
                CoverManager.AskForCover();
            }

        }
    }
}
