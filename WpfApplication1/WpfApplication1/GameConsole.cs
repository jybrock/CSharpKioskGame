using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Strokes {


    /// <summary>
    /// GameConsole Partial Class Hosting Control. Inherits from HwndHost.
    /// </summary>
    public partial class GameConsole : HwndHost {
        IntPtr hwndControl;
        IntPtr hwndHost;
        int hostHeight, hostWidth;
        public GameConsole(double height, double width) {
            hostHeight = (int)height;
            hostWidth = (int)width;
        }
        //
        // These constants are largely taken from Winuser.h, and allow you to use conventional names when calling Win32 functions.
        internal const int
          WS_CHILD = 0x40000000,
          WS_VISIBLE = 0x10000000,
          LBS_NOTIFY = 0x00000001,
          HOST_ID = 0x00000002,
          LISTBOX_ID = 0x00000001,
          WS_VSCROLL = 0x00200000,
          WS_BORDER = 0x00800000;

        /// <summary>
        /// The HWND of the control is exposed through a read-only property, such that the host page can use it to send messages to the control.
        /// </summary>
        public IntPtr hwndListBox {
            get { return hwndControl; }
        }


        /// <summary>
        /// The ListBox control is created as a child of the host window. 
        /// The height and width of both windows are set to the values passed to the constructor, discussed above. 
        /// This ensures that the size of the host window and control is identical to the reserved area on the page. 
        /// After the windows are created, the sample returns a HandleRef object that contains the HWND of the host window.
        /// </summary>
        /// <param name="hwndParent">Object</param>
        /// <returns>HandleRef Object</returns>
        protected override HandleRef BuildWindowCore(HandleRef hwndParent) {
            hwndControl = IntPtr.Zero;
            hwndHost = IntPtr.Zero;

            hwndHost = CreateWindowEx(0, "static", "",
                                      WS_CHILD | WS_VISIBLE,
                                      0, 0,
                                      hostWidth, hostHeight,
                                      hwndParent.Handle,
                                      (IntPtr)HOST_ID,
                                      IntPtr.Zero,
                                      0);

            hwndControl = CreateWindowEx(0, "listbox", "",
                                          WS_CHILD | WS_VISIBLE | LBS_NOTIFY
                                            | WS_VSCROLL | WS_BORDER,
                                          0, 0,
                                          hostWidth, hostHeight,
                                          hwndHost,
                                          (IntPtr)LISTBOX_ID,
                                          IntPtr.Zero,
                                          0);

            return new HandleRef(this, hwndHost);
        }
        

        /// <summary>
        /// PInvoke Declarations
        /// </summary>
        /// <param name="dwExStyle">Int32</param>
        /// <param name="lpszClassName">String</param>
        /// <param name="lpszWindowName">String</param>
        /// <param name="style">Int32</param>
        /// <param name="x">Int32</param>
        /// <param name="y">Int32</param>
        /// <param name="width">Int32</param>
        /// <param name="height">Int32</param>
        /// <param name="hwndParent">IntPtr</param>
        /// <param name="hMenu">IntPtr</param>
        /// <param name="hInst">IntPtr</param>
        /// <param name="pvParam">Object</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateWindowEx(int dwExStyle,
                                                      string lpszClassName,
                                                      string lpszWindowName,
                                                      int style,
                                                      int x, int y,
                                                      int width, int height,
                                                      IntPtr hwndParent,
                                                      IntPtr hMenu,
                                                      IntPtr hInst,
                                                      [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        /// <summary>
        /// In addition to BuildWindowCore, you must also override the WndProc and DestroyWindowCore methods of the HwndHost. 
        /// In this example, the messages for the control are handled by the MessageHook handler, thus the implementation of WndProc.
        /// In the case of WndProc, set handled to false to indicate that the message was not handled and return 0. 
        /// </summary>
        /// <param name="hwnd">IntPtr</param>
        /// <param name="msg">Int32</param>
        /// <param name="wParam">IntPtr</param>
        /// <param name="lParam">IntPtr</param>
        /// <param name="handled">Bool</param>
        /// <returns></returns>
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
            handled = false;
            return IntPtr.Zero;
        }

        /// <summary>
        /// In this example, the messages for the control are handled by the MessageHook handler, and DestroyWindowCore is minimal. 
        /// </summary>
        /// <param name="hwnd">HandleRef</param>
        protected override void DestroyWindowCore(HandleRef hwnd) {
            DestroyWindow(hwnd.Handle);
        }

        /// <summary>
        /// For DestroyWindowCore, simply destroy the window.
        /// </summary>
        /// <param name="hwnd">IntPtr</param>
        /// <returns>Int32</returns>
        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Unicode)]
        internal static extern bool DestroyWindow(IntPtr hwnd);


        // There is also a set of constants. 
        // These constants are largely taken from Winuser.h, and allow you to use conventional names when calling Win32 functions.
        internal const int
          LBN_SELCHANGE = 0x00000001,
          WM_COMMAND = 0x00000111,
          LB_GETCURSEL = 0x00000188,
          LB_GETTEXTLEN = 0x0000018A,
          LB_ADDSTRING = 0x00000180,
          LB_GETTEXT = 0x00000189,
          LB_DELETESTRING = 0x00000182,
          LB_GETCOUNT = 0x0000018B;

        /// <summary>
        /// Notice that there are two PInvoke declarations for SendMessage. 
        /// You need a separate declaration for each signature to ensure that the data is marshaled correctly. 
        /// </summary>
        /// <param name="hwnd">IntPtr</param>
        /// <param name="msg">Int32</param>
        /// <param name="wParam">IntPtr</param>
        /// <param name="lParam">IntPtr</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hwnd,
                                               int msg,
                                               IntPtr wParam,
                                               IntPtr lParam);

        /// <summary>
        /// This is necessary because one uses the wParam parameter to pass a String.
        /// </summary>
        /// <param name="hwnd">IntPtr</param>
        /// <param name="msg">Int32</param>
        /// <param name="wParam">Int32</param>
        /// <param name="lParam">StringBuilder</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern int SendMessage(IntPtr hwnd,
                                               int msg,
                                               int wParam,
                                               [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lParam);


        /// <summary>
        /// This is necessary because one uses the wParam parameter to pass an Integer
        /// </summary>
        /// <param name="hwnd">IntPtr</param>
        /// <param name="msg">Int32</param>
        /// <param name="wParam">IntPtr</param>
        /// <param name="lParam">String</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hwnd,
                                                  int msg,
                                                  IntPtr wParam,
                                                  String lParam);



    }
}