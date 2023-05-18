using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClipV2.Utils;

namespace ChatClipV2
{
    internal class PromptManager
    {
        public static void CreateAPrompt()
        {
            StringBuilder promptBuilder = new StringBuilder();
            variable.clipBoards_index = 0;

            for (int i = 0; i < variable.prompt_Lines.Count; i++)
            {
                bool exists = variable.before_Locs.Contains(i);

                if (exists)
                {
                    promptBuilder.Append(variable.clipBoards[variable.clipBoards_index]);
                    variable.clipBoards_index++;
                }
                promptBuilder.Append(variable.prompt_Lines[i]);
            }

            string prompt = promptBuilder.ToString();
            clipboard.SetClipboardText(prompt);
            Console.WriteLine("Prompt saved in clipboard.");
        }
    }
}
