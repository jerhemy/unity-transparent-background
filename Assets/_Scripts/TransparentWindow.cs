using UnityEngine;
using System.Runtime.InteropServices;
 
public class TransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
    static extern int SetWindowLong(int hwnd, int nIndex, long dwNewLong);
    [DllImport("user32.dll")]
    static extern bool ShowWindowAsync(int hWnd, int nCmdShow);
    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    static extern int SetLayeredWindowAttributes(int hwnd, int crKey, byte bAlpha, int dwFlags);
    [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
    private static extern int GetActiveWindow();
    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern long GetWindowLong(int hwnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern int SetWindowPos(int hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
 
    void Start()
    {
        Application.runInBackground = true;
         
        if (!Application.isEditor)
        {
            int handle = GetActiveWindow();
            int fWidth = Screen.width;
            int fHeight = Screen.height;
            
            //Remove title bar
            long lCurStyle = GetWindowLong(handle, -16); // GWL_STYLE=-16
            int a = 12582912; //WS_CAPTION = 0x00C00000L
            int b = 1048576; //WS_HSCROLL = 0x00100000L
            int c = 2097152; //WS_VSCROLL = 0x00200000L
            int d = 524288; //WS_SYSMENU = 0x00080000L
            int e = 16777216; //WS_MAXIMIZE = 0x01000000L
            
            lCurStyle &= ~(a | b | c | d);
            lCurStyle &= e;
            
            SetWindowLong(handle, -16, lCurStyle); // GWL_STYLE=-16
            
            // Transparent windows with click through
            SetWindowLong(handle, -20, 524288); //GWL_EXSTYLE=-20; WS_EX_LAYERED=524288=&h80000, WS_EX_TRANSPARENT=32=0x00000020L
            // SetLayeredWindowAttributes(handle, 0, 51, 2); // Transparency=51=20%, LWA_ALPHA=2
            SetLayeredWindowAttributes(handle, 0, 51, 1); // Transparency=51=20%, LWA_ALPHA=2
            
            int SWP_NOSIZE = 1; //0x0001;
            int SWP_NOMOVE = 2; //0x0002;
            int TOPMOST_FLAGS = (SWP_NOMOVE | SWP_NOSIZE | 32 | 64);
            
            SetWindowPos(handle, -1, 0, 0, fWidth, fHeight, TOPMOST_FLAGS); //SWP_FRAMECHANGED = 0x0020 (32); //SWP_SHOWWINDOW = 0x0040 (64)
            ShowWindowAsync(handle, 3); //Forces window to show in case of unresponsive app    // SW_SHOWMAXIMIZED(3)
        }
    }
}
