using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClipV2
{
    internal class CommandManager
    {
        public static void DisplayCommands(Dictionary<string, string[]> commands)
        {
            foreach (KeyValuePair<string, string[]> command in commands)
            {
                Console.WriteLine(command.Key + ": [" + command.Value[1] + "]" + command.Value[0]);
            }
        }

        public static string GetUserInput()
        {
            Console.Write(CONST.CONSOLE_MAIN_NAME);
            return Console.ReadLine().Trim().ToLower();
        }


        public static bool IsExitCommand(string input)
        {
            return input == "x" || input == "exit";
        }

        public static bool ShouldGeneratePDF(string input)
        {
            return string.IsNullOrEmpty(input) || input == "y" || input == "yes";
        }
    }
}
