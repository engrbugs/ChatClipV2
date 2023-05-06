using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace clipboardIO
{
    class clipboard
    {
        [DllImport("user32.dll")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        public static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        public static extern bool CloseClipboard();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("user32.dll")]
        public static extern bool EmptyClipboard();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("user32.dll")]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);


        public static string GetClipboardText()
        {
            string text = "";
            // Try opening the clipboard
            if (OpenClipboard(IntPtr.Zero))
            {
                try
                {
                    IntPtr hData = GetClipboardData(13); // CF_UNICODETEXT value
                    if (hData != IntPtr.Zero)
                    {
                        IntPtr pText = GlobalLock(hData);
                        if (pText != IntPtr.Zero)
                        {
                            // Save text in a string instance
                            if (Marshal.SystemDefaultCharSize == 1)
                            {
                                text = Marshal.PtrToStringAnsi(pText);
                            }
                            else
                            {
                                text = Marshal.PtrToStringUni(pText);
                            }
                            GlobalUnlock(hData);
                        }
                    }
                }
                finally
                {
                    // Release the clipboard
                    CloseClipboard();
                }
            }
            return text;
        }

        public static void SetClipboardText(string str)
        {
            if (OpenClipboard(IntPtr.Zero))
            {
                try
                {
                    EmptyClipboard();
                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str);
                    IntPtr hMem = GlobalAlloc(0x0002 /* GMEM_MOVEABLE */, (UIntPtr)(bytes.Length + 1));
                    if (hMem != IntPtr.Zero)
                    {
                        IntPtr pMem = GlobalLock(hMem);
                        if (pMem != IntPtr.Zero)
                        {
                            // Copy the bytes to the global memory
                            Marshal.Copy(bytes, 0, pMem, bytes.Length);
                            // Add null terminator
                            Marshal.WriteByte(pMem + bytes.Length, 0);
                            GlobalUnlock(hMem);
                            SetClipboardData(1 /* CF_TEXT */, hMem);
                        }
                    }
                }
                finally
                {
                    // Release the clipboard
                    CloseClipboard();
                }
            }
        }

    }


}
