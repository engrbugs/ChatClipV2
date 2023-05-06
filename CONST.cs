public static class CONST
{
    public const string VER = "0.6b";
    public const string COMMENT_SYMBOL = "//"; // in ini files
    public const string FILES_TO_READ = "\\files.ini";

    public static Dictionary<string, string[]> COMMANDS = new Dictionary<string, string[]>()
    {
        {"Cover Letter", new string[] {"cover", "c"}},
        {"Exit", new string[] {"exit", "x"}}
    };
}