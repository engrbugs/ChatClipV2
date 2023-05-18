using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClipV2
{
    internal class variable
    {
        public static bool exit_called = false;
        public static int clipBoards_index = 0;
        public static bool ask_cover_phase = false;


        public static List<string> prompt_Lines = new List<string>();
        public static List<int> before_Locs = new List<int>();
        public static List<string> clipBoards = new List<string>();
    }
}
