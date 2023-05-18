using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClipV2.Utils
{
    internal class FileReader
    {
        static string RemoveCommentSection(string str)
        {
            int pos = str.IndexOf(CONST.COMMENT_SYMBOL);
            if (pos != -1)
                return str.Substring(0, pos).Trim();
            else
                return str.Trim();
        }
        static string ExePath()
        {
            return Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
        }
        public static void ReadFiles()
        {
            for (int i = 1; i <= 9; ++i)
            {
                string file = iniFIle.Read(ExePath() + CONST.FILES_TO_READ, "Files", i.ToString());
                if (!string.IsNullOrEmpty(file))
                {
                    string promptLine = File.ReadAllText((ExePath() + "\\" + RemoveCommentSection(file)));
                    variable.prompt_Lines.Add(promptLine);
                }

                string before = iniFIle.Read(ExePath() + CONST.FILES_TO_READ, "Before", i.ToString());
                if (!string.IsNullOrEmpty(before))
                {
                    int beforeLoc = int.Parse(RemoveCommentSection(before)) - 1;
                    variable.before_Locs.Add(beforeLoc);
                    variable.clipBoards.Add("");
                }
            }
        }
    }
}
