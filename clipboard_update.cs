using System;
using System.Runtime.InteropServices;

namespace ClipboardUpdate;
class clipboard_update
{
    [DllImport("user32.dll")]
    static extern IntPtr CreateWindowEx(
        uint dwExStyle, string lpClassName, string lpWindowName,
        uint dwStyle, int x, int y, int nWidth, int nHeight,
        IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam
    );

    [DllImport("user32.dll")]
    static extern bool DestroyWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern bool AddClipboardFormatListener(IntPtr hwnd);

    [DllImport("user32.dll")]
    static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

    [DllImport("user32.dll")]
    static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    static extern bool TranslateMessage(ref MSG lpMsg);

    [DllImport("user32.dll")]
    static extern IntPtr DispatchMessage(ref MSG lpMsg);

    const int WM_CLIPBOARDUPDATE = 0x031D;
    static IntPtr HWND_MESSAGE = new IntPtr(-3);

    //static void Main()
    //{
    //    bool result = check_clipboardupdate();
    //    Console.WriteLine(result ? "Clipboard updated!" : "Failed to detect clipboard update.");
    //}

    public static bool check_clipboardupdate()
    {
        IntPtr hwnd = CreateWindowEx(0, "STATIC", "", 0, 0, 0, 0, 0, HWND_MESSAGE, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

        if (hwnd == IntPtr.Zero)
        {
            return false;
        }

        if (!AddClipboardFormatListener(hwnd))
        {
            DestroyWindow(hwnd);
            return false;
        }

        MSG msg;
        bool ret;

        while ((ret = GetMessage(out msg, hwnd, 0, 0)) != false)
        {
            if (ret == null)
            {
                // Error
                break;
            }

            TranslateMessage(ref msg);
            DispatchMessage(ref msg);

            if (msg.message == WM_CLIPBOARDUPDATE)
            {
                RemoveClipboardFormatListener(hwnd);
                DestroyWindow(hwnd);
                return true;
            }
        }

        RemoveClipboardFormatListener(hwnd);
        DestroyWindow(hwnd);

        return false;
    }
}

struct MSG
{
    public IntPtr hwnd;
    public uint message;
    public IntPtr wParam;
    public IntPtr lParam;
    public uint time;
    public POINT pt;
}

struct POINT
{
    public int x;
    public int y;
}
