public static class CONST
{
    public const string VER = "0.6b";
    public const string COMMENT_SYMBOL = "//"; // in ini files
    public const string FILES_TO_READ = "\\files.ini";

    public static Dictionary<string, string[]> COMMANDS = new Dictionary<string, string[]>()
    {
        {"Revert", new string[] {"revert", "r"}},
    };
    public static Dictionary<string, string[]> EXIT_COMMANDS = new Dictionary<string, string[]>()
    {
        {"Exit", new string[] {"exit", "x"}}
    };
    public const string CONSOLE_MAIN_NAME = "main: ";
}