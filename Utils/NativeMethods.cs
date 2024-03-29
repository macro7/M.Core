﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace M.Core.Utils
{
    /// <summary>
    /// 包含互操作方法调用的应用程序中使用。
    /// </summary>
    public sealed class NativeMethods
    {
        /// 私有构造函数，而不是用于实例
        private NativeMethods()
        {
        }

        /// <summary>
        /// PlaySound API可以用来播放一个WAV文件。长期需要根据映射表加以固定。
        /// PlaySound("c:\\win95\\media\\The Microsoft Sound.wav", 0,0);
        /// </summary>
        /// <param name="lpszName"></param>
        /// <param name="hModule"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        public static extern long PlaySound(String lpszName, long hModule, long dwFlags);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beepType"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern Boolean MessageBeep(UInt32 beepType);
        /// <summary>
        /// 设置由不同线程产生的窗口的显示状态
        /// ShowWindowAsync(this.Handle, 1);
        /// </summary>
        /// <param name="hWnd">线程句柄</param>
        /// <param name="nCmdShow">
        /// 0：隐藏;
        /// 1：正常;
        /// 3 ：最大化;
        /// 4;显示但不激活
        /// 5 显示 ;
        /// 6 ：最小化;
        /// 9 ：还原;
        /// 10：显示默认</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        /// <summary>
        /// 确定给定窗口是否是最小化（图标化）的窗口
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        internal static int SW_RESTORE = 9;

        /// <summary>
        /// 把指定的窗口提到最前面
        /// </summary>
        /// <param name="hWnd"></param>
        public static void BringToFront(IntPtr hWnd)
        {
            if (NativeMethods.IsIconic(hWnd))
            {
                NativeMethods.ShowWindowAsync(hWnd, NativeMethods.SW_RESTORE);
            }
            NativeMethods.SetForegroundWindow(hWnd);
        }

        /// <summary>
        /// 将指定的窗口设置到Z序的顶部。如果窗口为顶层窗口，则该窗口被激活；如果窗口为子窗口，则相应的顶级父窗口被激活
        /// </summary>
        /// <param name="hWnd">设置到Z序的顶部的窗口句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        /// <summary>
        /// 复制应用程序图标
        /// </summary>
        /// <param name="hIcon"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        /// <summary>
        /// 销毁一个图标并且释放该图标所占用的内存。
        /// </summary>
        /// <param name="hIcon"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "DestroyIcon")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyIcon(IntPtr handle);

        /// <summary>
        /// 枚举一个父窗口的所有子窗口。
        /// </summary>
        /// <param name="hwndParent">父窗口句柄</param>
        /// <param name="lpEnumFunc">回调函数的地址</param>
        /// <param name="lParam">自定义的参数</param>
        /// <returns>如果回调函数返回true，则枚举继续直到枚举完成；如果返回false，则将会中止枚举。</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// 该函数枚举所有与一个线程相关联的非子窗口，办法是先将句柄传送给每一个窗口，随后传送给应用程序定义的回调函数。EnumThreadWindows函数继续直到所有窗口枚举完为止或回调函数返回FALSE为止。要枚举一个特定窗口的所有子窗口，使用EnumChildWindows函数。
        /// </summary>
        /// <param name="dwThreadId">标识将被列举窗口的线程。</param>
        /// <param name="lpfn">指向一个应用程序定义的回调函数指针</param>
        /// <param name="lParam">指定一个传递给回调函数的应用程序定义值。</param>
        /// <returns>如果函数成功，返回值为非零；如果函数失败，返回值为零。若想获得更多错误信息，请调用GetLastError函数。</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumThreadWindows(uint dwThreadId, EnumWindowsProc lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// 该函数获得指定窗口所属的类的类名。
        /// </summary>
        /// <param name="hWnd">窗口的句柄及间接给出的窗口所属的类。</param>
        /// <param name="lpClassName">指向接收窗口类名字符串的缓冲区的指针。</param>
        /// <param name="nMaxCount">指定由参数lpClassName指示的缓冲区的字节数。如果类名字符串大于缓冲区的长度，则多出的部分被截断。</param>
        /// <returns>返回值为拷贝到指定缓冲区的字符个数：如果函数失败，返回值为0。若想获得更多错误信息，请调用GetLastError函数。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        /// 将你打开的APP中客户区的坐标点信息转换为整个屏幕的坐标，其中：所有的坐标（无论是屏幕坐标还是客户区坐标）其坐标原点都是左上角为（0，0）。
        /// </summary>
        /// <param name="hWnd">用户区域用于转换的窗口句柄。</param>
        /// <param name="lpRect">指向一个含有要转换的用户坐标的结构的指针，如果函数调用成功，新屏幕坐标复制到此结构。</param>
        /// <returns>如果函数调用成功，返回值为非零值，否则为零。</returns>
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        /// 检索有关的全局游标的信息。
        /// </summary>
        /// <param name="pci"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out CursorInfo pci);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        /// <summary>
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。 
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        /// <summary>
        /// 获取一个前台窗口的句柄（窗口与用户当前的工作）。该系统分配给其他线程比它的前台窗口的线程创建一个稍微更高的优先级。
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool GetIconInfo(IntPtr hIcon, out IconInfo piconinfo);

        /// <summary>
        /// 该函数返回指定窗口的显示状态以及被恢复的、最大化的和最小化的窗口位置。
        /// </summary>
        /// <param name="hWnd">窗口句柄。</param>
        /// <param name="lpwndpl">指向WINDOWPLACEMENT结构的指针，该结构存贮显示状态和位置信息。</param>
        /// <returns>如果函数成功，返回值为非零；如果函数失败，返回值为零。若想获得更多错误信息，请调用GetlastError函数。</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
        /// <summary>
        /// 返回与指定窗口有特定关系（如Z序或所有者）的窗口句柄。
        /// </summary>
        /// <param name="hWnd">窗口句柄。这个函数要返回的窗口句柄是依据nCmd参数值相对于hWnd参数的关系。</param>
        /// <param name="wCmd">说明指定窗口与要获得句柄的窗口之间的关系。该参数值可以是下列之一：
        /// GW_CHILD(&H5)：如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。函数仅检查指定父窗口的子窗口，不检查继承窗口。
        /// GW_ENABLEDPOPUP(&H6)：（WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；如果无使能窗口，则获得的句柄与指定窗口相同。
        /// GW_HWNDFIRST(&H0)：返回的句柄标识了在Z序最高端的相同类型的窗口。如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。
        /// GW_HWNDLAST(&H1)：返回的句柄标识了在z序最低端的相同类型的窗口。如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。
        /// GW_HWNDNEXT(&H2)：返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。
        /// GW HWNDPREV(&H3)：返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。
        /// GW_OWNER(&H4)：返回的句柄标识了指定窗口的所有者窗口（如果存在）。GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。例如：例如有时对话框的控件的GW_OWNER，是不存在的。</param>
        /// <returns>如果函数成功，返回值为窗口句柄；如果与指定窗口有特定关系的窗口不存在，则返回值为NULL。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowConstants wCmd);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int smIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern ulong GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);
        /// <summary>
        /// GetWindowThreadProcessId
        /// </summary>
        /// <param name="hWnd">（向函数提供的）被查找窗口的句柄.</param>
        /// <param name="lpdwProcessId">进程号的存放地址（变量地址） </param>
        /// <returns>返回线程号，注意，lpdwProcessId 是存放进程号的变量。返回值是线程号，lpdwProcessId 是进程号存放处。</returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        /// <summary>
        /// 获得指定窗口的可视状态，即显示或者隐藏。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="hwnd">Wnd可以是窗口、控件等的handle</param>
        /// <param name="hDC"></param>
        /// <param name="nFlags"></param>
        /// <returns></returns>
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        /// 从当前线程中的窗口释放鼠标捕获，并恢复通常的鼠标输入处理。
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        /// <summary>
        /// 该函数将指定的消息发送到一个或多个窗口。此函数为指定的窗口调用窗口程序，直到窗口程序处理完消息再返回。该函数是应用程序和应用程序之间进行消息传递的主要手段之一。
        /// </summary>
        /// <param name="hWnd">其窗口程序将接收消息的窗口的句柄。如果此参数为HWND_BROADCAST，则消息将被发送到系统中所有顶层窗口，包括无效或不可见的非自身拥有的窗口、被覆盖的窗口和弹出式窗口，但消息不被发送到子窗口。</param>
        /// <param name="Msg">指定被发送的消息。</param>
        /// <param name="wParam">指定附加的消息指定信息。</param>
        /// <param name="lParam">指定附加的消息指定信息</param>
        /// <returns>返回值指定消息处理的结果，依赖于所发送的消息。</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// 将被激活的最顶层窗口
        /// </summary>
        /// <param name="hWnd">窗口的句柄</param>
        /// <returns>若函数调用成功，则返回原先活动窗口的句柄。若函数调用失败，则返回值为NULL。若要获得更多错误信息，可以调用GetLastError函数。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);
        /// <summary>
        /// 创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>
        /// <param name="hWnd">窗口的句柄</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        /// <summary>
        /// 弹出式窗口或顶层窗口的尺寸，位置和Z序。子窗口，弹出式窗口，及顶层窗口根据它们在屏幕上出现的顺序排序、顶层窗口设置的级别最高，并且被设置为Z序的第一个窗口。
        /// </summary>
        /// <param name="hWnd">用于标识在z-顺序的此 CWnd 对象之前的 CWnd 对象。</param>
        /// <param name="hWndInsertAfter">
        /// 在z序中的位于被置位的窗口前的窗口句柄。该参数必须为一个窗口句柄，或下列值之一：
        /// HWND_BOTTOM：将窗口置于Z序的底部。如果参数hWnd标识了一个顶层窗口，则窗口失去顶级位置，并且被置在其他窗口的底部。
        /// HWND_NOTOPMOST：将窗口置于所有非顶层窗口之上（即在所有顶层窗口之后）。如果窗口已经是非顶层窗口则该标志不起作用。
        /// HWND_TOP:将窗口置于Z序的顶部。
        /// HWND_TOPMOST:将窗口置于所有非顶层窗口之上。即使窗口未被激活窗口也将保持顶级位置。</param>
        /// <param name="X">以客户坐标指定窗口新位置的左边界。</param>
        /// <param name="Y">以客户坐标指定窗口新位置的顶边界。</param>
        /// <param name="cx">以像素指定窗口的新的宽度。</param>
        /// <param name="cy">以像素指定窗口的新的高度。</param>
        /// <param name="uFlags">
        /// 窗口尺寸和定位的标志。该参数可以是下列值的组合：
        /// 1:SWP_ASYNCWINDOWPOS：如果调用进程不拥有窗口，系统会向拥有窗口的线程发出需求。这就防止调用线程在其他线程处理需求的时候发生死锁。
        /// 2:SWP_DEFERERASE：防止产生WM_SYNCPAINT消息。
        /// 3:SWP_DRAWFRAME：在窗口周围画一个边框（定义在窗口类描述中）。
        /// 4:SWP_FRAMECHANGED：给窗口发送WM_NCCALCSIZE消息，即使窗口尺寸没有改变也会发送该消息。如果未指定这个标志，只有在改变了窗口尺寸时才发送WM_NCCALCSIZE。
        /// 5:SWP_HIDEWINDOW;隐藏窗口。
        /// 6:SWP_NOACTIVATE：不激活窗口。如果未设置标志，则窗口被激活，并被设置到其他最高级窗口或非最高级组的顶部（根据参数hWndlnsertAfter设置）。
        /// 7:SWP_NOCOPYBITS：清除客户区的所有内容。如果未设置该标志，客户区的有效内容被保存并且在窗口尺寸更新和重定位后拷贝回客户区。
        /// 8:SWP_NOMOVE：维持当前位置（忽略X和Y参数）。
        /// 9:SWP_NOOWNERZORDER：不改变z序中的所有者窗口的位置。
        /// 10:SWP_NOREDRAW:不重画改变的内容。如果设置了这个标志，则不发生任何重画动作。适用于客户区和非客户区（包括标题栏和滚动条）和任何由于窗回移动而露出的父窗口的所有部分。如果设置了这个标志，应用程序必须明确地使窗口无效并区重画窗口的任何部分和父窗口需要重画的部分。
        /// 11:SWP_NOREPOSITION：与SWP_NOOWNERZORDER标志相同。
        /// 12:SWP_NOSENDCHANGING：防止窗口接收WM_WINDOWPOSCHANGING消息。
        /// 13:SWP_NOSIZE：维持当前尺寸（忽略cx和Cy参数）。
        /// 14:SWP_NOZORDER：维持当前Z序（忽略hWndlnsertAfter参数）。
        /// 15:SWP_SHOWWINDOW：显示窗口。
        ///</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern int SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetProcessWorkingSetSize(IntPtr handle, IntPtr min, IntPtr max);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst,
           ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pptSrc, uint crKey,
           [In] ref BLENDFUNCTION pblend, uint dwFlags);

        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = 0x0001,
            SUSPEND_RESUME = 0x0002,
            GET_CONTEXT = 0x0008,
            SET_CONTEXT = 0x0010,
            SET_INFORMATION = 0x0020,
            QUERY_INFORMATION = 0x0040,
            SET_THREAD_TOKEN = 0x0080,
            IMPERSONATE = 0x0100,
            DIRECT_IMPERSONATION = 0x0200
        }

        public struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public enum SHOWWINDOW : uint
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        public enum GetWindowConstants : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6,
            GW_MAX = 6
        }

        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        /// <summary>Enumeration of the different ways of showing a window using
        /// ShowWindow</summary>
        public enum WindowShowStyle : uint
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,
            /// <summary>Activates and displays a window. If the window is minimized
            /// or maximized, the system restores it to its original size and
            /// position. An application should specify this flag when displaying
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,
            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,
            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,
            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,
            /// <summary>Displays a window in its most recent size and position.
            /// This value is similar to "ShowNormal", except the window is not
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,
            /// <summary>Activates the window and displays it in its current size
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,
            /// <summary>Minimizes the specified window and activates the next
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,
            /// <summary>Displays the window as a minimized window. This value is
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,
            /// <summary>Displays the window in its current size and position. This
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,
            /// <summary>Activates and displays the window. If the window is
            /// minimized or maximized, the system restores it to its original size
            /// and position. An application should specify this flag when restoring
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,
            /// <summary>Sets the show state based on the SW_ value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
            /// that owns the window is hung. This flag should only be used when
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }

        public enum RegionType
        {
            ERROR = 0,
            NULLREGION = 1,
            SIMPLEREGION = 2,
            COMPLEXREGION = 3
        }

        public enum PRF
        {
            CHECKVISIBLE = 0x01,
            NONCLIENT = 0x02,
            CLIENT = 0x04,
            ERASEBKGND = 0x08,
            CHILDREN = 0x10,
            OWNED = 0x20
        }

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MINIMIZE = 0xF020;

        // From winuser.h
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 SWP_NOZORDER = 0x0004;
        public const UInt32 SWP_NOREDRAW = 0x0008;
        public const UInt32 SWP_NOACTIVATE = 0x0010;
        public const UInt32 SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        public const UInt32 SWP_SHOWWINDOW = 0x0040;
        public const UInt32 SWP_HIDEWINDOW = 0x0080;
        public const UInt32 SWP_NOCOPYBITS = 0x0100;
        public const UInt32 SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        public const UInt32 SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);

        #region Windows Messages

        /// <summary>
        /// Windows Messages
        /// Defined in winuser.h from Windows SDK v6.1
        /// Documentation pulled from MSDN.
        /// </summary>
        public enum WM : uint
        {
            /// <summary>
            /// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
            /// </summary>
            NULL = 0x0000,
            /// <summary>
            /// The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.
            /// </summary>
            CREATE = 0x0001,
            /// <summary>
            /// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen.
            /// This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message, it can be assumed that all child windows still exist.
            /// /// </summary>
            DESTROY = 0x0002,
            /// <summary>
            /// The WM_MOVE message is sent after a window has been moved.
            /// </summary>
            MOVE = 0x0003,
            /// <summary>
            /// The WM_SIZE message is sent to a window after its size has changed.
            /// </summary>
            SIZE = 0x0005,
            /// <summary>
            /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately.
            /// </summary>
            ACTIVATE = 0x0006,
            /// <summary>
            /// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus.
            /// </summary>
            SETFOCUS = 0x0007,
            /// <summary>
            /// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus.
            /// </summary>
            KILLFOCUS = 0x0008,
            /// <summary>
            /// The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed.
            /// </summary>
            ENABLE = 0x000A,
            /// <summary>
            /// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn.
            /// </summary>
            SETREDRAW = 0x000B,
            /// <summary>
            /// An application sends a WM_SETTEXT message to set the text of a window.
            /// </summary>
            SETTEXT = 0x000C,
            /// <summary>
            /// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller.
            /// </summary>
            GETTEXT = 0x000D,
            /// <summary>
            /// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window.
            /// </summary>
            GETTEXTLENGTH = 0x000E,
            /// <summary>
            /// The WM_PAINT message is sent when the system or another application makes a request to paint a portion of an application's window. The message is sent when the UpdateWindow or RedrawWindow function is called, or by the DispatchMessage function when the application obtains a WM_PAINT message by using the GetMessage or PeekMessage function.
            /// </summary>
            PAINT = 0x000F,
            /// <summary>
            /// The WM_CLOSE message is sent as a signal that a window or an application should terminate.
            /// </summary>
            CLOSE = 0x0010,
            /// <summary>
            /// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero, the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero.
            /// After processing this message, the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.
            /// </summary>
            QUERYENDSESSION = 0x0011,
            /// <summary>
            /// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.
            /// </summary>
            QUERYOPEN = 0x0013,
            /// <summary>
            /// The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.
            /// </summary>
            ENDSESSION = 0x0016,
            /// <summary>
            /// The WM_QUIT message indicates a request to terminate an application and is generated when the application calls the PostQuitMessage function. It causes the GetMessage function to return zero.
            /// </summary>
            QUIT = 0x0012,
            /// <summary>
            /// The WM_ERASEBKGND message is sent when the window background must be erased (for example, when a window is resized). The message is sent to prepare an invalidated portion of a window for painting.
            /// </summary>
            ERASEBKGND = 0x0014,
            /// <summary>
            /// This message is sent to all top-level windows when a change is made to a system color setting.
            /// </summary>
            SYSCOLORCHANGE = 0x0015,
            /// <summary>
            /// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
            /// </summary>
            SHOWWINDOW = 0x0018,
            /// <summary>
            /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
            /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
            /// </summary>
            WININICHANGE = 0x001A,
            /// <summary>
            /// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
            /// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
            /// </summary>
            SETTINGCHANGE = WM.WININICHANGE,
            /// <summary>
            /// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings.
            /// </summary>
            DEVMODECHANGE = 0x001B,
            /// <summary>
            /// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
            /// </summary>
            ACTIVATEAPP = 0x001C,
            /// <summary>
            /// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources.
            /// </summary>
            FONTCHANGE = 0x001D,
            /// <summary>
            /// A message that is sent whenever there is a change in the system time.
            /// </summary>
            TIMECHANGE = 0x001E,
            /// <summary>
            /// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For example, the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example, the EnableWindow function sends this message when disabling the specified window.
            /// </summary>
            CANCELMODE = 0x001F,
            /// <summary>
            /// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.
            /// </summary>
            SETCURSOR = 0x0020,
            /// <summary>
            /// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.
            /// </summary>
            MOUSEACTIVATE = 0x0021,
            /// <summary>
            /// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated, moved, or sized.
            /// </summary>
            CHILDACTIVATE = 0x0022,
            /// <summary>
            /// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure.
            /// </summary>
            QUEUESYNC = 0x0023,
            /// <summary>
            /// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size.
            /// </summary>
            GETMINMAXINFO = 0x0024,
            /// <summary>
            /// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows, except in unusual circumstances explained in the Remarks.
            /// </summary>
            PAINTICON = 0x0026,
            /// <summary>
            /// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.
            /// </summary>
            ICONERASEBKGND = 0x0027,
            /// <summary>
            /// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box.
            /// </summary>
            NEXTDLGCTL = 0x0028,
            /// <summary>
            /// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue.
            /// </summary>
            SPOOLERSTATUS = 0x002A,
            /// <summary>
            /// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box, list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.
            /// </summary>
            DRAWITEM = 0x002B,
            /// <summary>
            /// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control, or menu item when the control or menu is created.
            /// </summary>
            MEASUREITEM = 0x002C,
            /// <summary>
            /// Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.
            /// </summary>
            DELETEITEM = 0x002D,
            /// <summary>
            /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message.
            /// </summary>
            VKEYTOITEM = 0x002E,
            /// <summary>
            /// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message.
            /// </summary>
            CHARTOITEM = 0x002F,
            /// <summary>
            /// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.
            /// </summary>
            SETFONT = 0x0030,
            /// <summary>
            /// An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text.
            /// </summary>
            GETFONT = 0x0031,
            /// <summary>
            /// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key, the system activates the window.
            /// </summary>
            SETHOTKEY = 0x0032,
            /// <summary>
            /// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window.
            /// </summary>
            GETHOTKEY = 0x0033,
            /// <summary>
            /// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.
            /// </summary>
            QUERYDRAGICON = 0x0037,
            /// <summary>
            /// The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item, the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style.
            /// </summary>
            COMPAREITEM = 0x0039,
            /// <summary>
            /// Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application.
            /// Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint, AccessibleObjectFromEvent, or AccessibleObjectFromWindow. However, server applications handle this message.
            /// </summary>
            GETOBJECT = 0x003D,
            /// <summary>
            /// The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.
            /// </summary>
            COMPACTING = 0x0041,
            /// <summary>
            /// WM_COMMNOTIFY is Obsolete for Win32-Based Applications
            /// </summary>
            [Obsolete]
            COMMNOTIFY = 0x0044,
            /// <summary>
            /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
            /// </summary>
            WINDOWPOSCHANGING = 0x0046,
            /// <summary>
            /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
            /// </summary>
            WINDOWPOSCHANGED = 0x0047,
            /// <summary>
            /// Notifies applications that the system, typically a battery-powered personal computer, is about to enter a suspended mode.
            /// Use: POWERBROADCAST
            /// </summary>
            [Obsolete]
            POWER = 0x0048,
            /// <summary>
            /// An application sends the WM_COPYDATA message to pass data to another application.
            /// </summary>
            COPYDATA = 0x004A,
            /// <summary>
            /// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle.
            /// </summary>
            CANCELJOURNAL = 0x004B,
            /// <summary>
            /// Sent by a common control to its parent window when an event has occurred or the control requires some information.
            /// </summary>
            NOTIFY = 0x004E,
            /// <summary>
            /// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language, either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately.
            /// </summary>
            INPUTLANGCHANGEREQUEST = 0x0050,
            /// <summary>
            /// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function, which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows, and so on.
            /// </summary>
            INPUTLANGCHANGE = 0x0051,
            /// <summary>
            /// Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.
            /// </summary>
            TCARD = 0x0052,
            /// <summary>
            /// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently active window.
            /// </summary>
            HELP = 0x0053,
            /// <summary>
            /// The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off, the system updates the user-specific settings. The system sends this message immediately after updating the settings.
            /// </summary>
            USERCHANGED = 0x0054,
            /// <summary>
            /// Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.
            /// </summary>
            NOTIFYFORMAT = 0x0055,
            /// <summary>
            /// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.
            /// </summary>
            CONTEXTMENU = 0x007B,
            /// <summary>
            /// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.
            /// </summary>
            STYLECHANGING = 0x007C,
            /// <summary>
            /// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
            /// </summary>
            STYLECHANGED = 0x007D,
            /// <summary>
            /// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.
            /// </summary>
            DISPLAYCHANGE = 0x007E,
            /// <summary>
            /// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption.
            /// </summary>
            GETICON = 0x007F,
            /// <summary>
            /// An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption.
            /// </summary>
            SETICON = 0x0080,
            /// <summary>
            /// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.
            /// </summary>
            NCCREATE = 0x0081,
            /// <summary>
            /// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window.
            /// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast, WM_DESTROY is sent before the child windows are destroyed.
            /// </summary>
            NCDESTROY = 0x0082,
            /// <summary>
            /// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
            /// </summary>
            NCCALCSIZE = 0x0083,
            /// <summary>
            /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
            /// </summary>
            NCHITTEST = 0x0084,
            /// <summary>
            /// The WM_NCPAINT message is sent to a window when its frame must be painted.
            /// </summary>
            NCPAINT = 0x0085,
            /// <summary>
            /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
            /// </summary>
            NCACTIVATE = 0x0086,
            /// <summary>
            /// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default, the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior, the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.
            /// </summary>
            GETDLGCODE = 0x0087,
            /// <summary>
            /// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.
            /// </summary>
            SYNCPAINT = 0x0088,
            /// <summary>
            /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCMOUSEMOVE = 0x00A0,
            /// <summary>
            /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCLBUTTONDOWN = 0x00A1,
            /// <summary>
            /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCLBUTTONUP = 0x00A2,
            /// <summary>
            /// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCLBUTTONDBLCLK = 0x00A3,
            /// <summary>
            /// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCRBUTTONDOWN = 0x00A4,
            /// <summary>
            /// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCRBUTTONUP = 0x00A5,
            /// <summary>
            /// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCRBUTTONDBLCLK = 0x00A6,
            /// <summary>
            /// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCMBUTTONDOWN = 0x00A7,
            /// <summary>
            /// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCMBUTTONUP = 0x00A8,
            /// <summary>
            /// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCMBUTTONDBLCLK = 0x00A9,
            /// <summary>
            /// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCXBUTTONDOWN = 0x00AB,
            /// <summary>
            /// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCXBUTTONUP = 0x00AC,
            /// <summary>
            /// The WM_NCXBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
            /// </summary>
            NCXBUTTONDBLCLK = 0x00AD,
            /// <summary>
            /// The WM_INPUT_DEVICE_CHANGE message is sent to the window that registered to receive raw input. A window receives this message through its WindowProc function.
            /// </summary>
            INPUT_DEVICE_CHANGE = 0x00FE,
            /// <summary>
            /// The WM_INPUT message is sent to the window that is getting raw input.
            /// </summary>
            INPUT = 0x00FF,
            /// <summary>
            /// This message filters for keyboard messages.
            /// </summary>
            KEYFIRST = 0x0100,
            /// <summary>
            /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
            /// </summary>
            KEYDOWN = 0x0100,
            /// <summary>
            /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus.
            /// </summary>
            KEYUP = 0x0101,
            /// <summary>
            /// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.
            /// </summary>
            CHAR = 0x0102,
            /// <summary>
            /// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot), that is combined with another character to form a composite character. For example, the umlaut-O character (Ö) is generated by typing the dead key for the umlaut character, and then typing the O key.
            /// </summary>
            DEADCHAR = 0x0103,
            /// <summary>
            /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
            /// </summary>
            SYSKEYDOWN = 0x0104,
            /// <summary>
            /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
            /// </summary>
            SYSKEYUP = 0x0105,
            /// <summary>
            /// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is, a character key that is pressed while the ALT key is down.
            /// </summary>
            SYSCHAR = 0x0106,
            /// <summary>
            /// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is, a dead key that is pressed while holding down the ALT key.
            /// </summary>
            SYSDEADCHAR = 0x0107,
            /// <summary>
            /// The WM_UNICHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_UNICHAR message contains the character code of the key that was pressed.
            /// The WM_UNICHAR message is equivalent to WM_CHAR, but it uses Unicode Transformation Format (UTF)-32, whereas WM_CHAR uses UTF-16. It is designed to send or post Unicode characters to ANSI windows and it can can handle Unicode Supplementary Plane characters.
            /// </summary>
            UNICHAR = 0x0109,
            /// <summary>
            /// This message filters for keyboard messages.
            /// </summary>
            KEYLAST = 0x0109,
            /// <summary>
            /// Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function.
            /// </summary>
            IME_STARTCOMPOSITION = 0x010D,
            /// <summary>
            /// Sent to an application when the IME ends composition. A window receives this message through its WindowProc function.
            /// </summary>
            IME_ENDCOMPOSITION = 0x010E,
            /// <summary>
            /// Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function.
            /// </summary>
            IME_COMPOSITION = 0x010F,
            IME_KEYLAST = 0x010F,
            /// <summary>
            /// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box.
            /// </summary>
            INITDIALOG = 0x0110,
            /// <summary>
            /// The WM_COMMAND message is sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated.
            /// </summary>
            COMMAND = 0x0111,
            /// <summary>
            /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu) or when the user chooses the maximize button, minimize button, restore button, or close button.
            /// </summary>
            SYSCOMMAND = 0x0112,
            /// <summary>
            /// The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function.
            /// </summary>
            TIMER = 0x0113,
            /// <summary>
            /// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control.
            /// </summary>
            HSCROLL = 0x0114,
            /// <summary>
            /// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control.
            /// </summary>
            VSCROLL = 0x0115,
            /// <summary>
            /// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed.
            /// </summary>
            INITMENU = 0x0116,
            /// <summary>
            /// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed, without changing the entire menu.
            /// </summary>
            INITMENUPOPUP = 0x0117,
            /// <summary>
            /// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item.
            /// </summary>
            MENUSELECT = 0x011F,
            /// <summary>
            /// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu.
            /// </summary>
            MENUCHAR = 0x0120,
            /// <summary>
            /// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages.
            /// </summary>
            ENTERIDLE = 0x0121,
            /// <summary>
            /// The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item.
            /// </summary>
            MENURBUTTONUP = 0x0122,
            /// <summary>
            /// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item.
            /// </summary>
            MENUDRAG = 0x0123,
            /// <summary>
            /// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item.
            /// </summary>
            MENUGETOBJECT = 0x0124,
            /// <summary>
            /// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed.
            /// </summary>
            UNINITMENUPOPUP = 0x0125,
            /// <summary>
            /// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu.
            /// </summary>
            MENUCOMMAND = 0x0126,
            /// <summary>
            /// An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.
            /// </summary>
            CHANGEUISTATE = 0x0127,
            /// <summary>
            /// An application sends the WM_UPDATEUISTATE message to change the user interface (UI) state for the specified window and all its child windows.
            /// </summary>
            UPDATEUISTATE = 0x0128,
            /// <summary>
            /// An application sends the WM_QUERYUISTATE message to retrieve the user interface (UI) state for a window.
            /// </summary>
            QUERYUISTATE = 0x0129,
            /// <summary>
            /// The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message, the owner window can set the text and background colors of the message box by using the given display device context handle.
            /// </summary>
            CTLCOLORMSGBOX = 0x0132,
            /// <summary>
            /// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the edit control.
            /// </summary>
            CTLCOLOREDIT = 0x0133,
            /// <summary>
            /// Sent to the parent window of a list box before the system draws the list box. By responding to this message, the parent window can set the text and background colors of the list box by using the specified display device context handle.
            /// </summary>
            CTLCOLORLISTBOX = 0x0134,
            /// <summary>
            /// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However, only owner-drawn buttons respond to the parent window processing this message.
            /// </summary>
            CTLCOLORBTN = 0x0135,
            /// <summary>
            /// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message, the dialog box can set its text and background colors using the specified display device context handle.
            /// </summary>
            CTLCOLORDLG = 0x0136,
            /// <summary>
            /// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control.
            /// </summary>
            CTLCOLORSCROLLBAR = 0x0137,
            /// <summary>
            /// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the static control.
            /// </summary>
            CTLCOLORSTATIC = 0x0138,
            /// <summary>
            /// Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.
            /// </summary>
            MOUSEFIRST = 0x0200,
            /// <summary>
            /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            MOUSEMOVE = 0x0200,
            /// <summary>
            /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            LBUTTONDOWN = 0x0201,
            /// <summary>
            /// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            LBUTTONUP = 0x0202,
            /// <summary>
            /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            LBUTTONDBLCLK = 0x0203,
            /// <summary>
            /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            RBUTTONDOWN = 0x0204,
            /// <summary>
            /// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            RBUTTONUP = 0x0205,
            /// <summary>
            /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            RBUTTONDBLCLK = 0x0206,
            /// <summary>
            /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            MBUTTONDOWN = 0x0207,
            /// <summary>
            /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            MBUTTONUP = 0x0208,
            /// <summary>
            /// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            MBUTTONDBLCLK = 0x0209,
            /// <summary>
            /// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
            /// </summary>
            MOUSEWHEEL = 0x020A,
            /// <summary>
            /// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            XBUTTONDOWN = 0x020B,
            /// <summary>
            /// The WM_XBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            XBUTTONUP = 0x020C,
            /// <summary>
            /// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
            /// </summary>
            XBUTTONDBLCLK = 0x020D,
            /// <summary>
            /// The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
            /// </summary>
            MOUSEHWHEEL = 0x020E,
            /// <summary>
            /// Use WM_MOUSELAST to specify the last mouse message. Used with PeekMessage() Function.
            /// </summary>
            MOUSELAST = 0x020E,
            /// <summary>
            /// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed, or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.
            /// </summary>
            PARENTNOTIFY = 0x0210,
            /// <summary>
            /// The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered.
            /// </summary>
            ENTERMENULOOP = 0x0211,
            /// <summary>
            /// The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited.
            /// </summary>
            EXITMENULOOP = 0x0212,
            /// <summary>
            /// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu.
            /// </summary>
            NEXTMENU = 0x0213,
            /// <summary>
            /// The WM_SIZING message is sent to a window that the user is resizing. By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position.
            /// </summary>
            SIZING = 0x0214,
            /// <summary>
            /// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.
            /// </summary>
            CAPTURECHANGED = 0x0215,
            /// <summary>
            /// The WM_MOVING message is sent to a window that the user is moving. By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position.
            /// </summary>
            MOVING = 0x0216,
            /// <summary>
            /// Notifies applications that a power-management event has occurred.
            /// </summary>
            POWERBROADCAST = 0x0218,
            /// <summary>
            /// Notifies an application of a change to the hardware configuration of a device or the computer.
            /// </summary>
            DEVICECHANGE = 0x0219,
            /// <summary>
            /// An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window.
            /// </summary>
            MDICREATE = 0x0220,
            /// <summary>
            /// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window.
            /// </summary>
            MDIDESTROY = 0x0221,
            /// <summary>
            /// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window.
            /// </summary>
            MDIACTIVATE = 0x0222,
            /// <summary>
            /// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size.
            /// </summary>
            MDIRESTORE = 0x0223,
            /// <summary>
            /// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window.
            /// </summary>
            MDINEXT = 0x0224,
            /// <summary>
            /// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar, and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window.
            /// </summary>
            MDIMAXIMIZE = 0x0225,
            /// <summary>
            /// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format.
            /// </summary>
            MDITILE = 0x0226,
            /// <summary>
            /// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format.
            /// </summary>
            MDICASCADE = 0x0227,
            /// <summary>
            /// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized.
            /// </summary>
            MDIICONARRANGE = 0x0228,
            /// <summary>
            /// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window.
            /// </summary>
            MDIGETACTIVE = 0x0229,
            /// <summary>
            /// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window, to replace the window menu of the frame window, or both.
            /// </summary>
            MDISETMENU = 0x0230,
            /// <summary>
            /// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
            /// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows is enabled.
            /// </summary>
            ENTERSIZEMOVE = 0x0231,
            /// <summary>
            /// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
            /// </summary>
            EXITSIZEMOVE = 0x0232,
            /// <summary>
            /// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.
            /// </summary>
            DROPFILES = 0x0233,
            /// <summary>
            /// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window.
            /// </summary>
            MDIREFRESHMENU = 0x0234,
            /// <summary>
            /// Sent to an application when a window is activated. A window receives this message through its WindowProc function.
            /// </summary>
            IME_SETCONTEXT = 0x0281,
            /// <summary>
            /// Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function.
            /// </summary>
            IME_NOTIFY = 0x0282,
            /// <summary>
            /// Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message, the application calls the SendMessage function with the following parameters.
            /// </summary>
            IME_CONTROL = 0x0283,
            /// <summary>
            /// Sent to an application when the IME window finds no space to extend the area for the composition window. A window receives this message through its WindowProc function.
            /// </summary>
            IME_COMPOSITIONFULL = 0x0284,
            /// <summary>
            /// Sent to an application when the operating system is about to change the current IME. A window receives this message through its WindowProc function.
            /// </summary>
            IME_SELECT = 0x0285,
            /// <summary>
            /// Sent to an application when the IME gets a character of the conversion result. A window receives this message through its WindowProc function.
            /// </summary>
            IME_CHAR = 0x0286,
            /// <summary>
            /// Sent to an application to provide commands and request information. A window receives this message through its WindowProc function.
            /// </summary>
            IME_REQUEST = 0x0288,
            /// <summary>
            /// Sent to an application by the IME to notify the application of a key press and to keep message order. A window receives this message through its WindowProc function.
            /// </summary>
            IME_KEYDOWN = 0x0290,
            /// <summary>
            /// Sent to an application by the IME to notify the application of a key release and to keep message order. A window receives this message through its WindowProc function.
            /// </summary>
            IME_KEYUP = 0x0291,
            /// <summary>
            /// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
            /// </summary>
            MOUSEHOVER = 0x02A1,
            /// <summary>
            /// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
            /// </summary>
            MOUSELEAVE = 0x02A3,
            /// <summary>
            /// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the window for the period of time specified in a prior call to TrackMouseEvent.
            /// </summary>
            NCMOUSEHOVER = 0x02A0,
            /// <summary>
            /// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
            /// </summary>
            NCMOUSELEAVE = 0x02A2,
            /// <summary>
            /// The WM_WTSSESSION_CHANGE message notifies applications of changes in session state.
            /// </summary>
            WTSSESSION_CHANGE = 0x02B1,
            TABLET_FIRST = 0x02c0,
            TABLET_LAST = 0x02df,
            /// <summary>
            /// An application sends a WM_CUT message to an edit control or combo box to delete (cut) the current selection, if any, in the edit control and copy the deleted text to the clipboard in CF_TEXT format.
            /// </summary>
            CUT = 0x0300,
            /// <summary>
            /// An application sends the WM_COPY message to an edit control or combo box to copy the current selection to the clipboard in CF_TEXT format.
            /// </summary>
            COPY = 0x0301,
            /// <summary>
            /// An application sends a WM_PASTE message to an edit control or combo box to copy the current content of the clipboard to the edit control at the current caret position. Data is inserted only if the clipboard contains data in CF_TEXT format.
            /// </summary>
            PASTE = 0x0302,
            /// <summary>
            /// An application sends a WM_CLEAR message to an edit control or combo box to delete (clear) the current selection, if any, from the edit control.
            /// </summary>
            CLEAR = 0x0303,
            /// <summary>
            /// An application sends a WM_UNDO message to an edit control to undo the last operation. When this message is sent to an edit control, the previously deleted text is restored or the previously added text is deleted.
            /// </summary>
            UNDO = 0x0304,
            /// <summary>
            /// The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData function.
            /// </summary>
            RENDERFORMAT = 0x0305,
            /// <summary>
            /// The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats. For the content of the clipboard to remain available to other applications, the clipboard owner must render data in all the formats it is capable of generating, and place the data on the clipboard by calling the SetClipboardData function.
            /// </summary>
            RENDERALLFORMATS = 0x0306,
            /// <summary>
            /// The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard.
            /// </summary>
            DESTROYCLIPBOARD = 0x0307,
            /// <summary>
            /// The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard.
            /// </summary>
            DRAWCLIPBOARD = 0x0308,
            /// <summary>
            /// The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area needs repainting.
            /// </summary>
            PAINTCLIPBOARD = 0x0309,
            /// <summary>
            /// The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
            /// </summary>
            VSCROLLCLIPBOARD = 0x030A,
            /// <summary>
            /// The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size.
            /// </summary>
            SIZECLIPBOARD = 0x030B,
            /// <summary>
            /// The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.
            /// </summary>
            ASKCBFORMATNAME = 0x030C,
            /// <summary>
            /// The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when a window is being removed from the chain.
            /// </summary>
            CHANGECBCHAIN = 0x030D,
            /// <summary>
            /// The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window. This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
            /// </summary>
            HSCROLLCLIPBOARD = 0x030E,
            /// <summary>
            /// This message informs a window that it is about to receive the keyboard focus, giving the window the opportunity to realize its logical palette when it receives the focus.
            /// </summary>
            QUERYNEWPALETTE = 0x030F,
            /// <summary>
            /// The WM_PALETTEISCHANGING message informs applications that an application is going to realize its logical palette.
            /// </summary>
            PALETTEISCHANGING = 0x0310,
            /// <summary>
            /// This message is sent by the OS to all top-level and overlapped windows after the window with the keyboard focus realizes its logical palette.
            /// This message enables windows that do not have the keyboard focus to realize their logical palettes and update their client areas.
            /// </summary>
            PALETTECHANGED = 0x0311,
            /// <summary>
            /// The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey function. The message is placed at the top of the message queue associated with the thread that registered the hot key.
            /// </summary>
            HOTKEY = 0x0312,
            /// <summary>
            /// The WM_PRINT message is sent to a window to request that it draw itself in the specified device context, most commonly in a printer device context.
            /// </summary>
            PRINT = 0x0317,
            /// <summary>
            /// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified device context, most commonly in a printer device context.
            /// </summary>
            PRINTCLIENT = 0x0318,
            /// <summary>
            /// The WM_APPCOMMAND message notifies a window that the user generated an application command event, for example, by clicking an application command button using the mouse or typing an application command key on the keyboard.
            /// </summary>
            APPCOMMAND = 0x0319,
            /// <summary>
            /// The WM_THEMECHANGED message is broadcast to every window following a theme change event. Examples of theme change events are the activation of a theme, the deactivation of a theme, or a transition from one theme to another.
            /// </summary>
            THEMECHANGED = 0x031A,
            /// <summary>
            /// Sent when the contents of the clipboard have changed.
            /// </summary>
            CLIPBOARDUPDATE = 0x031D,
            /// <summary>
            /// The system will send a window the WM_DWMCOMPOSITIONCHANGED message to indicate that the availability of desktop composition has changed.
            /// </summary>
            DWMCOMPOSITIONCHANGED = 0x031E,
            /// <summary>
            /// WM_DWMNCRENDERINGCHANGED is called when the non-client area rendering status of a window has changed. Only windows that have set the flag DWM_BLURBEHIND.fTransitionOnMaximized to true will get this message.
            /// </summary>
            DWMNCRENDERINGCHANGED = 0x031F,
            /// <summary>
            /// Sent to all top-level windows when the colorization color has changed.
            /// </summary>
            DWMCOLORIZATIONCOLORCHANGED = 0x0320,
            /// <summary>
            /// WM_DWMWINDOWMAXIMIZEDCHANGE will let you know when a DWM composed window is maximized. You also have to register for this message as well. You'd have other windowd go opaque when this message is sent.
            /// </summary>
            DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
            /// <summary>
            /// Sent to request extended title bar information. A window receives this message through its WindowProc function.
            /// </summary>
            GETTITLEBARINFOEX = 0x033F,
            HANDHELDFIRST = 0x0358,
            HANDHELDLAST = 0x035F,
            AFXFIRST = 0x0360,
            AFXLAST = 0x037F,
            PENWINFIRST = 0x0380,
            PENWINLAST = 0x038F,
            /// <summary>
            /// The WM_APP constant is used by applications to help define private messages, usually of the form WM_APP+X, where X is an integer value.
            /// </summary>
            APP = 0x8000,
            /// <summary>
            /// The WM_USER constant is used by applications to help define private messages for use by private window classes, usually of the form WM_USER+X, where X is an integer value.
            /// </summary>
            USER = 0x0400,
            /// <summary>
            /// An application sends the WM_CPL_LAUNCH message to Windows Control Panel to request that a Control Panel application be started.
            /// </summary>
            CPL_LAUNCH = USER + 0x1000,
            /// <summary>
            /// The WM_CPL_LAUNCHED message is sent when a Control Panel application, started by the WM_CPL_LAUNCH message, has closed. The WM_CPL_LAUNCHED message is sent to the window identified by the wParam parameter of the WM_CPL_LAUNCH message that started the application.
            /// </summary>
            CPL_LAUNCHED = USER + 0x1001,
            /// <summary>
            /// WM_SYSTIMER is a well-known yet still undocumented message. Windows uses WM_SYSTIMER for internal actions like scrolling.
            /// </summary>
            SYSTIMER = 0x118
        }

        #endregion

        #region Window Styles

        /// <summary>
        /// Window Styles.
        /// The following styles can be specified wherever a window style is required. After the control has been created, these styles cannot be modified, except as noted.
        /// </summary>
        [Flags]
        public enum WindowStyles : uint
        {
            /// <summary>
            /// Creates an overlapped window. An overlapped window usually has a caption and a border.
            /// </summary>
            WS_OVERLAPPED = 0x00000000,

            /// <summary>
            /// Creates a pop-up window. Cannot be used with the <see cref="WS_CHILD"/> style.
            /// </summary>
            WS_POPUP = 0x80000000,

            /// <summary>
            /// Creates a child window. Cannot be used with the <see cref="WS_POPUP"/> style.
            /// </summary>
            WS_CHILD = 0x40000000,

            /// <summary>
            /// Creates a window that is initially minimized. For use with the <see cref="WS_OVERLAPPED"/> style only.
            /// </summary>
            WS_MINIMIZE = 0x20000000,

            /// <summary>
            /// Creates a window that is initially visible.
            /// </summary>
            WS_VISIBLE = 0x10000000,

            /// <summary>
            /// Creates a window that is initially disabled.
            /// </summary>
            WS_DISABLED = 0x08000000,

            /// <summary>
            /// Clips child windows relative to each other; that is, when a particular child window receives a paint message, the WS_CLIPSIBLINGS style clips all other overlapped child windows out of the region of the child window to be updated. (If WS_CLIPSIBLINGS is not given and child windows overlap, when you draw within the client area of a child window, it is possible to draw within the client area of a neighboring child window.) For use with the <see cref="WS_CHILD"/> style only.
            /// </summary>
            WS_CLIPSIBLINGS = 0x04000000,

            /// <summary>
            /// Excludes the area occupied by child windows when you draw within the parent window.
            /// Used when you create the parent window.
            /// </summary>
            WS_CLIPCHILDREN = 0x02000000,

            /// <summary>
            /// Creates a window of maximum size.
            /// </summary>
            WS_MAXIMIZE = 0x01000000,

            /// <summary>
            /// Creates a window that has a border.
            /// </summary>
            WS_BORDER = 0x00800000,

            /// <summary>
            /// Creates a window with a double border but no title.
            /// </summary>
            WS_DLGFRAME = 0x00400000,

            /// <summary>
            /// Creates a window that has a vertical scroll bar.
            /// </summary>
            WS_VSCROLL = 0x00200000,

            /// <summary>
            /// Creates a window that has a horizontal scroll bar.
            /// </summary>
            WS_HSCROLL = 0x00100000,

            /// <summary>
            /// Creates a window that has a Control-menu box in its title bar. Used only for windows with title bars.
            /// </summary>
            WS_SYSMENU = 0x00080000,

            /// <summary>
            /// Creates a window with a thick frame that can be used to size the window.
            /// </summary>
            WS_THICKFRAME = 0x00040000,

            /// <summary>
            /// Specifies the first control of a group of controls in which the user can move from one control to the next with the arrow keys. All controls defined with the WS_GROUP style FALSE after the first control belong to the same group. The next control with the WS_GROUP style starts the next group (that is, one group ends where the next begins).
            /// </summary>
            WS_GROUP = 0x00020000,

            /// <summary>
            /// Specifies one of any number of controls through which the user can move by using the TAB key. The TAB key moves the user to the next control specified by the WS_TABSTOP style.
            /// </summary>
            WS_TABSTOP = 0x00010000,

            /// <summary>
            /// Creates a window that has a Minimize button.
            /// </summary>
            WS_MINIMIZEBOX = 0x00020000,

            /// <summary>
            /// Creates a window that has a Maximize button.
            /// </summary>
            WS_MAXIMIZEBOX = 0x00010000,

            /// <summary>
            /// Creates a window that has a title bar (implies the <see cref="WS_BORDER"/> style).
            /// Cannot be used with the <see cref="WS_DLGFRAME"/> style.
            /// </summary>
            WS_CAPTION = WS_BORDER | WS_DLGFRAME,

            /// <summary>
            /// Creates an overlapped window. An overlapped window has a title bar and a border. Same as the <see cref="WS_OVERLAPPED"/> style.
            /// </summary>
            WS_TILED = WS_OVERLAPPED,

            /// <summary>
            /// Creates a window that is initially minimized. Same as the <see cref="WS_MINIMIZE"/> style.
            /// </summary>
            WS_ICONIC = WS_MINIMIZE,

            /// <summary>
            /// Creates a window that has a sizing border. Same as the <see cref="WS_THICKFRAME"/> style.
            /// </summary>
            WS_SIZEBOX = WS_THICKFRAME,

            /// <summary>
            /// Creates an overlapped window with the <see cref="WS_OVERLAPPED"/>, <see cref="WS_CAPTION"/>, <see cref="WS_SYSMENU"/>, <see cref="WS_THICKFRAME"/>, <see cref="WS_MINIMIZEBOX"/>, and <see cref="WS_MAXIMIZEBOX"/> styles. Same as the <see cref="WS_OVERLAPPEDWINDOW"/> style.
            /// </summary>
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            /// <summary>
            /// Creates an overlapped window with the <see cref="WS_OVERLAPPED"/>, <see cref="WS_CAPTION"/>, <see cref="WS_SYSMENU"/>, <see cref="WS_THICKFRAME"/>, <see cref="WS_MINIMIZEBOX"/>, and <see cref="WS_MAXIMIZEBOX"/> styles.
            /// </summary>
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

            /// <summary>
            /// Creates a pop-up window with the <see cref="WS_BORDER"/>, <see cref="WS_POPUP"/>, and <see cref="WS_SYSMENU"/> styles. The WS_CAPTION style must be combined with the <see cref="WS_POPUPWINDOW"/> style to make the Control menu visible.
            /// </summary>
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,

            /// <summary>
            /// Same as the <see cref="WS_CHILD"/> style.
            /// </summary>
            WS_CHILDWINDOW = WS_CHILD,

            //Extended Window Styles

            /// <summary>
            /// Designates a window with a double border that may (optionally) be created with a title bar when you specify the <see cref="WS_CAPTION"/> style flag in the dwStyle parameter.
            /// </summary>
            WS_EX_DLGMODALFRAME = 0x00000001,

            /// <summary>
            /// Specifies that a child window created with this style will not send the <see cref="WM_PARENTNOTIFY"/> message to its parent window when the child window is created or destroyed.
            /// </summary>
            WS_EX_NOPARENTNOTIFY = 0x00000004,

            /// <summary>
            /// Specifies that a window created with this style should be placed above all nontopmost windows and stay above them even when the window is deactivated. An application can use the <see cref="SetWindowPos"/> member function to add or remove this attribute.
            /// </summary>
            WS_EX_TOPMOST = 0x00000008,

            /// <summary>
            /// Specifies that a window created with this style accepts drag-and-drop files.
            /// </summary>
            WS_EX_ACCEPTFILES = 0x00000010,

            /// <summary>
            /// Specifies that a window created with this style is to be transparent. That is, any windows that are beneath the window are not obscured by the window. A window created with this style receives <see cref="WM_PAINT"/> messages only after all sibling windows beneath it have been updated.
            /// </summary>
            WS_EX_TRANSPARENT = 0x00000020,

            //#if(WINVER >= 0x0400)

            /// <summary>
            /// Creates an MDI child window.
            /// </summary>
            WS_EX_MDICHILD = 0x00000040,

            /// <summary>
            /// Creates a tool window, which is a window intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the task bar or in the window that appears when the user presses ALT+TAB.
            /// </summary>
            WS_EX_TOOLWINDOW = 0x00000080,

            /// <summary>
            /// Specifies that a window has a border with a raised edge.
            /// </summary>
            WS_EX_WINDOWEDGE = 0x00000100,

            /// <summary>
            /// Specifies that a window has a 3D look — that is, a border with a sunken edge.
            /// </summary>
            WS_EX_CLIENTEDGE = 0x00000200,

            /// <summary>
            /// Includes a question mark in the title bar of the window. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a <see cref="WM_HELP"/> message.
            /// </summary>
            WS_EX_CONTEXTHELP = 0x00000400,

            /// <summary>
            /// Gives a window generic right-aligned properties. This depends on the window class.
            /// </summary>
            WS_EX_RIGHT = 0x00001000,

            /// <summary>
            /// Gives window generic left-aligned properties. This is the default.
            /// </summary>
            WS_EX_LEFT = 0x00000000,

            /// <summary>
            /// Displays the window text using right-to-left reading order properties.
            /// </summary>
            WS_EX_RTLREADING = 0x00002000,

            /// <summary>
            /// Displays the window text using left-to-right reading order properties. This is the default.
            /// </summary>
            WS_EX_LTRREADING = 0x00000000,

            /// <summary>
            /// Places a vertical scroll bar to the left of the client area.
            /// </summary>
            WS_EX_LEFTSCROLLBAR = 0x00004000,

            /// <summary>
            /// Places a vertical scroll bar (if present) to the right of the client area. This is the default.
            /// </summary>
            WS_EX_RIGHTSCROLLBAR = 0x00000000,

            /// <summary>
            /// Allows the user to navigate among the child windows of the window by using the TAB key.
            /// </summary>
            WS_EX_CONTROLPARENT = 0x00010000,

            /// <summary>
            /// Creates a window with a three-dimensional border style intended to be used for items that do not accept user input.
            /// </summary>
            WS_EX_STATICEDGE = 0x00020000,

            /// <summary>
            /// Forces a top-level window onto the taskbar when the window is visible.
            /// </summary>
            WS_EX_APPWINDOW = 0x00040000,

            /// <summary>
            /// Combines the <see cref="WS_EX_CLIENTEDGE"/> and <see cref="WS_EX_WINDOWEDGE"/> styles.
            /// </summary>
            WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),

            /// <summary>
            /// Combines the <see cref="WS_EX_WINDOWEDGE"/> and <see cref="WS_EX_TOPMOST"/> styles.
            /// </summary>
            WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
            //#endif /* WINVER >= 0x0400 */

            //#if(_WIN32_WINNT >= 0x0500)
            /// <summary>
            /// Windows 2000/XP: Creates a layered window. Note that this cannot be used for child windows. Also, this cannot be used if the window has a class style of either <see cref="CS_OWNDC"/> or <see cref="CS_CLASSDC"/>.
            /// </summary>
            WS_EX_LAYERED = 0x00080000,
            //#endif /* _WIN32_WINNT >= 0x0500 */

            //#if(WINVER >= 0x0500)
            /// <summary>
            /// Windows 2000/XP: A window created with this style does not pass its window layout to its child windows.
            /// </summary>
            WS_EX_NOINHERITLAYOUT = 0x00100000,

            /// <summary>
            /// Arabic and Hebrew versions of Windows 98/Me, Windows 2000/XP: Creates a window whose horizontal origin is on the right edge. Increasing horizontal values advance to the left.
            /// </summary>
            WS_EX_LAYOUTRTL = 0x00400000,
            //#endif /* WINVER >= 0x0500 */

            //#if(_WIN32_WINNT >= 0x0500)
            /// <summary>
            /// Windows XP: Paints all descendants of a window in bottom-to-top painting order using double-buffering. For more information, see Remarks. This cannot be used if the window has a class style of either <see cref="CS_OWNDC"/> or <see cref="CS_CLASSDC"/>.
            /// </summary>
            WS_EX_COMPOSITED = 0x02000000,

            /// <summary>
            /// Windows 2000/XP: A top-level window created with this style does not become the foreground window when the user clicks it. The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
            /// To activate the window, use the <see cref="SetActiveWindow"/> or <see cref="SetForegroundWindow"/> function.
            /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the <see cref="WS_EX_APPWINDOW"/> style.
            /// </summary>
            WS_EX_NOACTIVATE = 0x08000000
            //#endif /* _WIN32_WINNT >= 0x0500 */
        }

        #endregion

        [Flags]
        public enum ClassStyles : uint
        {
            CS_VREDRAW = 0x0001,
            CS_HREDRAW = 0x0002,
            CS_DBLCLKS = 0x0008,
            CS_OWNDC = 0x0020,
            CS_CLASSDC = 0x0040,
            CS_PARENTDC = 0x0080,
            CS_NOCLOSE = 0x0200,
            CS_SAVEBITS = 0x0800,
            CS_BYTEALIGNCLIENT = 0x1000,
            CS_BYTEALIGNWINDOW = 0x2000,
            CS_GLOBALCLASS = 0x4000,
            CS_IME = 0x00010000,
            CS_DROPSHADOW = 0x00020000
        }

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int CURSOR_SHOWING = 0x00000001;
        public const int GWL_STYLE = -16;
        public const ulong TARGETWINDOW = (uint)WindowStyles.WS_BORDER | (uint)WindowStyles.WS_VISIBLE;

        [StructLayout(LayoutKind.Sequential)]
        public struct IconInfo
        {
            public bool fIcon;         // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
            public Int32 xHotspot;     // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 
            public Int32 yHotspot;     // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 
            public IntPtr hbmMask;     // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 
            public IntPtr hbmColor;    // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CursorInfo
        {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
            public IntPtr hCursor;      // Handle to the cursor. 
            public Point ptScreenPos;   // A POINT structure that receives the screen coordinates of the cursor. 
        }

        [DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint SHAppBarMessage(int dwMessage, out APPBARDATA pData);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static string GetWindowLabel()
        {
            const int numOfChars = 256;
            IntPtr handle = GetForegroundWindow();
            StringBuilder sb = new StringBuilder(numOfChars);

            if (GetWindowText(handle, sb, numOfChars) > 0)
            {
                return sb.ToString();
            }

            return string.Empty;
        }

        public static IntPtr GetWindowHandle()
        {
            const int numOfChars = 256;
            IntPtr handle = GetForegroundWindow();
            StringBuilder sb = new StringBuilder(numOfChars);

            if (GetWindowText(handle, sb, numOfChars) > 0)
            {
                return handle;
            }

            return IntPtr.Zero;
        }

        public static string GetClassName(IntPtr handle)
        {
            const int numOfChars = 100;
            StringBuilder sb = new StringBuilder(numOfChars);

            if (GetClassName(handle, sb, numOfChars) > 0)
            {
                return sb.ToString();
            }

            return string.Empty;
        }

        public static MyCursor CaptureCursor()
        {
            CursorInfo cursorInfo = new CursorInfo();
            cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);

            if (GetCursorInfo(out cursorInfo) && cursorInfo.flags == CURSOR_SHOWING)
            {
                cursorInfo.ptScreenPos = NativeMethods.ConvertPoint(Cursor.Position);

                IntPtr hicon = CopyIcon(cursorInfo.hCursor);
                if (hicon != IntPtr.Zero)
                {
                    IconInfo iconInfo;
                    if (GetIconInfo(hicon, out iconInfo))
                    {
                        Point position = new Point(cursorInfo.ptScreenPos.X - iconInfo.xHotspot, cursorInfo.ptScreenPos.Y - iconInfo.yHotspot);

                        using (Bitmap maskBitmap = Bitmap.FromHbitmap(iconInfo.hbmMask))
                        {
                            Bitmap resultBitmap = null;

                            if (IsCursorMonochrome(maskBitmap))
                            {
                                resultBitmap = new Bitmap(maskBitmap.Width, maskBitmap.Width);

                                Graphics desktopGraphics = Graphics.FromHwnd(GetDesktopWindow());
                                IntPtr desktopHdc = desktopGraphics.GetHdc();

                                IntPtr maskHdc = GDI.CreateCompatibleDC(desktopHdc);
                                IntPtr oldPtr = GDI.SelectObject(maskHdc, maskBitmap.GetHbitmap());

                                using (Graphics resultGraphics = Graphics.FromImage(resultBitmap))
                                {
                                    IntPtr resultHdc = resultGraphics.GetHdc();

                                    // These two operation will result in a black cursor over a white background.
                                    // Later in the code, a call to MakeTransparent() will get rid of the white background.
                                    GDI.BitBlt(resultHdc, 0, 0, 32, 32, maskHdc, 0, 32, CopyPixelOperation.SourceCopy);
                                    GDI.BitBlt(resultHdc, 0, 0, 32, 32, maskHdc, 0, 0, CopyPixelOperation.SourceInvert);

                                    resultGraphics.ReleaseHdc(resultHdc);
                                }

                                IntPtr newPtr = GDI.SelectObject(maskHdc, oldPtr);
                                GDI.DeleteDC(newPtr);
                                GDI.DeleteDC(maskHdc);
                                desktopGraphics.ReleaseHdc(desktopHdc);

                                // Remove the white background from the BitBlt calls,
                                // resulting in a black cursor over a transparent background.
                                resultBitmap.MakeTransparent(Color.White);
                            }
                            else
                            {
                                resultBitmap = Icon.FromHandle(hicon).ToBitmap();
                            }

                            return new MyCursor(new Cursor(cursorInfo.hCursor), position, resultBitmap);
                        }
                    }
                }
            }

            return new MyCursor();
        }

        private static bool IsCursorMonochrome(Bitmap bmp)
        {
            bool isMonochrome = bmp.Height == bmp.Width * 2;
            Color white = Color.FromArgb(255, 255, 255, 255);
            for (int y = 0; y < bmp.Height / 2; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    isMonochrome &= bmp.GetPixel(x, y) == white;
                    if (!isMonochrome)
                    {
                        return isMonochrome;
                    }
                }
            }

            return isMonochrome;
        }

        public static bool GetWindowRegion(IntPtr hWnd, out Region region)
        {
            IntPtr hRgn = GDI.CreateRectRgn(0, 0, 0, 0);
            RegionType regionType = (RegionType)GetWindowRgn(hWnd, hRgn);
            region = Region.FromHrgn(hRgn);
            return regionType != RegionType.ERROR && regionType != RegionType.NULLREGION;
        }

        public class MyCursor : IDisposable
        {
            public Cursor Cursor;
            public Point Position;
            public Bitmap Bitmap;

            public MyCursor()
            {
                Cursor = Cursor.Current;
                Position = new Point(Cursor.Position.X - Cursor.HotSpot.X, Cursor.Position.Y - Cursor.HotSpot.Y);
                Bitmap = Icon.FromHandle(Cursor.Handle).ToBitmap();
            }

            public MyCursor(Cursor cursor, Point position, Bitmap bitmap)
            {
                Cursor = cursor;
                Position = position;
                Bitmap = bitmap;
            }

            public void Dispose()
            {
                Cursor.Dispose();
                Bitmap.Dispose();
            }
        }

        [Flags]
        public enum DwmWindowAttribute
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Last
        }

        [Flags]
        public enum DwmNCRenderingPolicy
        {
            UseWindowStyle,
            Disabled,
            Enabled,
            Last
        }
        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);
        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out bool pvAttribute, int cbAttribute);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        [DllImport("dwmapi.dll")]
        public static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

        [StructLayout(LayoutKind.Sequential)]
        public struct DWM_BLURBEHIND
        {
            public DWM_BB dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTransitionOnMaximized;
        }

        [Flags]
        public enum DWM_BB
        {
            Enable = 1,
            BlurRegion = 2,
            TransitionMaximized = 4
        }

        [DllImport("dwmapi.dll")]
        public static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport("dwmapi.dll")]
        public static extern int DwmUnregisterThumbnail(IntPtr thumb);

        [DllImport("dwmapi.dll")]
        public static extern int DwmQueryThumbnailSourceSize(IntPtr thumb, out SIZE size);

        [DllImport("dwmapi.dll")]
        public static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DWM_THUMBNAIL_PROPERTIES props);

        public static readonly int DWM_TNP_RECTDESTINATION = 0x1;
        public static readonly int DWM_TNP_OPACITY = 0x4;
        public static readonly int DWM_TNP_VISIBLE = 0x8;

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetDxFrameDuration(IntPtr hwnd, uint cRefreshes);

        [StructLayout(LayoutKind.Sequential)]
        public struct DWM_THUMBNAIL_PROPERTIES
        {
            public int dwFlags;
            public RECT rcDestination;
            public RECT rcSource;
            public byte opacity;
            public bool fVisible;
            public bool fSourceClientAreaOnly;
        }
        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public int Height { get { return Bottom - Top; } }
            public int Width { get { return Right - Left; } }

            public Size Size { get { return new Size(Width, Height); } }
            public Point Location { get { return new Point(Left, Top); } }

            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }

            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                  ^ ((Width << 0x1a) | (Width >> 6))
                  ^ ((Height << 7) | (Height >> 0x19));
            }

            #region Operator overloads

            public static implicit operator Rectangle(RECT rect)
            {
                return rect.ToRectangle();
            }

            public static implicit operator RECT(Rectangle rect)
            {
                return FromRectangle(rect);
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int x;
            public int y;

            public SIZE(int width, int height)
            {
                x = width;
                y = height;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static explicit operator Point(POINT p)
            {
                return new Point(p.X, p.Y);
            }

            public static explicit operator POINT(Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }

        public static bool GetExtendedFrameBounds(IntPtr handle, out Rectangle rectangle)
        {
            RECT rect;
            int result = DwmGetWindowAttribute(handle, (int)DwmWindowAttribute.ExtendedFrameBounds, out rect, Marshal.SizeOf(typeof(RECT)));
            rectangle = rect.ToRectangle();
            return result >= 0;
        }

        public static bool DWMWA_NCRENDERING_ENABLED(IntPtr handle)
        {
            bool enabled;
            int result = DwmGetWindowAttribute(handle, (int)DwmWindowAttribute.NCRenderingEnabled, out enabled, sizeof(bool));
            //if (result < 0) throw new Exception("Error: DWMWA_NCRENDERING_ENABLED");
            return enabled;
        }

        public static void SetDWMWindowAttributeNCRenderingPolicy(IntPtr handle, DwmNCRenderingPolicy renderingPolicy)
        {
            int renderPolicy = (int)renderingPolicy;
            DwmSetWindowAttribute(handle, (int)DwmWindowAttribute.NCRenderingPolicy, ref renderPolicy, sizeof(int));
        }

        public static Rectangle GetWindowRect(IntPtr handle)
        {
            RECT rect;
            GetWindowRect(handle, out rect);
            return rect.ToRectangle();
        }

        public static Rectangle GetWindowRectangle(IntPtr handle)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                Rectangle rect;
                if (GetExtendedFrameBounds(handle, out rect))
                {
                    return rect;
                }
            }

            return GetWindowRect(handle);
        }

        public static Rectangle MaximizedWindowFix(IntPtr handle, Rectangle windowRect)
        {
            if (NativeMethods.IsWindowMaximized(handle))
            {
                Rectangle screenRect = Screen.FromRectangle(windowRect).Bounds;

                if (windowRect.X < screenRect.X)
                {
                    windowRect.Width -= (screenRect.X - windowRect.X) * 2;
                    windowRect.X = screenRect.X;
                }

                if (windowRect.Y < screenRect.Y)
                {
                    windowRect.Height -= (screenRect.Y - windowRect.Y) * 2;
                    windowRect.Y = screenRect.Y;
                }

                windowRect.Intersect(screenRect);
            }

            return windowRect;
        }

        public static void ActivateWindow(IntPtr handle)
        {
            SetForegroundWindow(handle);
            SetActiveWindow(handle);
        }

        public static void ActivateWindowRepeat(IntPtr handle, int count)
        {
            for (int i = 0; NativeMethods.GetForegroundWindow() != handle && i < count; i++)
            {
                NativeMethods.BringWindowToTop(handle);
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
            }
        }

        #region Taskbar

        [StructLayout(LayoutKind.Sequential)]
        public struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }

        public enum ABMsg
        {
            ABM_NEW = 0,
            ABM_REMOVE = 1,
            ABM_QUERYPOS = 2,
            ABM_SETPOS = 3,
            ABM_GETSTATE = 4,
            ABM_GETTASKBARPOS = 5,
            ABM_ACTIVATE = 6,
            ABM_GETAUTOHIDEBAR = 7,
            ABM_SETAUTOHIDEBAR = 8,
            ABM_WINDOWPOSCHANGED = 9,
            ABM_SETSTATE = 10
        }

        public enum ABEdge
        {
            ABE_LEFT = 0,
            ABE_TOP,
            ABE_RIGHT,
            ABE_BOTTOM
        }

        public enum ABState
        {
            ABS_MANUAL = 0,
            ABS_AUTOHIDE = 1,
            ABS_ALWAYSONTOP = 2,
            ABS_AUTOHIDEANDONTOP = 3,
        }

        public enum TaskBarEdge
        {
            Bottom,
            Top,
            Left,
            Right
        }

        public static Rectangle GetTaskbarRectangle()
        {
            APPBARDATA abd = new APPBARDATA();
            SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, out abd);
            return abd.rc.ToRectangle();
        }

        /// <summary>
        /// Method returns information about the Window's TaskBar.
        /// </summary>
        /// <param name="taskBarEdge">Location of the TaskBar(Top,Bottom,Left,Right).</param>
        /// <param name="height">Height of the TaskBar.</param>
        /// <param name="autoHide">AutoHide property of the TaskBar.</param>
        private static void GetTaskBarInfo(out TaskBarEdge taskBarEdge, out int height, out bool autoHide)
        {
            APPBARDATA abd = new APPBARDATA();

            height = 0;
            taskBarEdge = TaskBarEdge.Bottom;
            autoHide = false;

            uint ret = SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, out abd);
            switch (abd.uEdge)
            {
                case (int)ABEdge.ABE_BOTTOM:
                    taskBarEdge = TaskBarEdge.Bottom;
                    height = abd.rc.Height;
                    break;
                case (int)ABEdge.ABE_TOP:
                    taskBarEdge = TaskBarEdge.Top;
                    height = abd.rc.Bottom;
                    break;
                case (int)ABEdge.ABE_LEFT:
                    taskBarEdge = TaskBarEdge.Left;
                    height = abd.rc.Width;
                    break;
                case (int)ABEdge.ABE_RIGHT:
                    taskBarEdge = TaskBarEdge.Right;
                    height = abd.rc.Width;
                    break;
            }

            abd = new APPBARDATA();
            uint uState = SHAppBarMessage((int)ABMsg.ABM_GETSTATE, out abd);
            switch (uState)
            {
                case (int)ABState.ABS_ALWAYSONTOP:
                    autoHide = false;
                    break;
                case (int)ABState.ABS_AUTOHIDE:
                    autoHide = true;
                    break;
                case (int)ABState.ABS_AUTOHIDEANDONTOP:
                    autoHide = true;
                    break;
                case (int)ABState.ABS_MANUAL:
                    autoHide = false;
                    break;
            }
        }

        #endregion

        public static void TrimMemoryUse()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, (IntPtr)(-1), (IntPtr)(-1));
        }

        public static bool IsWindowMaximized(IntPtr handle)
        {
            NativeMethods.WINDOWPLACEMENT wp = new NativeMethods.WINDOWPLACEMENT();
            NativeMethods.GetWindowPlacement(handle, ref wp);
            return wp.showCmd == (int)NativeMethods.SHOWWINDOW.SW_MAXIMIZE;
        }

        public static Point ConvertPoint(Point p)
        {
            int x = 0, y = 0;

            foreach (Screen screen in Screen.AllScreens)
            {
                x = Math.Min(x, screen.Bounds.X);
                y = Math.Min(y, screen.Bounds.Y);
            }

            return new Point(p.X - x, p.Y - y);
        }


        //创建一个窗口   
        const int WM_CREATE = 0x01;
        //当一个窗口被破坏时发送   
        const int WM_DESTROY = 0x02;
        //移动一个窗口   
        const int WM_MOVE = 0x03;
        //改变一个窗口的大小   
        const int WM_SIZE = 0x05;
        //一个窗口被激活或失去激活状态   
        const int WM_ACTIVATE = 0x06;
        //一个窗口获得焦点   
        const int WM_SETFOCUS = 0x07;
        //一个窗口失去焦点   
        const int WM_KILLFOCUS = 0x08;
        //一个窗口改变成Enable状态   
        const int WM_ENABLE = 0x0A;
        //设置窗口是否能重画   
        const int WM_SETREDRAW = 0x0B;
        //应用程序发送此消息来设置一个窗口的文本   
        const int WM_SETTEXT = 0x0C;
        //应用程序发送此消息来复制对应窗口的文本到缓冲区   
        const int WM_GETTEXT = 0x0D;
        //得到与一个窗口有关的文本的长度（不包含空字符）   
        const int WM_GETTEXTLENGTH = 0x0E;
        //要求一个窗口重画自己   
        const int WM_PAINT = 0x0F;
        //当一个窗口或应用程序要关闭时发送一个信号   
        const int WM_CLOSE = 0x10;
        //当用户选择结束对话框或程序自己调用ExitWindows函数   
        const int WM_QUERYENDSESSION = 0x11;
        //用来结束程序运行   
        const int WM_QUIT = 0x12;
        //当用户窗口恢复以前的大小位置时，把此消息发送给某个图标   
        const int WM_QUERYOPEN = 0x13;
        //当窗口背景必须被擦除时（例在窗口改变大小时）   
        const int WM_ERASEBKGND = 0x14;
        //当系统颜色改变时，发送此消息给所有顶级窗口   
        const int WM_SYSCOLORCHANGE = 0x15;
        //当系统进程发出WM_QUERYENDSESSION消息后，此消息发送给应用程序，通知它对话是否结束   
        const int WM_ENDSESSION = 0x16;
        //当隐藏或显示窗口是发送此消息给这个窗口   
        const int WM_SHOWWINDOW = 0x18;
        //发此消息给应用程序哪个窗口是激活的，哪个是非激活的   
        const int WM_ACTIVATEAPP = 0x1C;
        //当系统的字体资源库变化时发送此消息给所有顶级窗口   
        const int WM_FONTCHANGE = 0x1D;
        //当系统的时间变化时发送此消息给所有顶级窗口   
        const int WM_TIMECHANGE = 0x1E;
        //发送此消息来取消某种正在进行的摸态（操作）   
        const int WM_CANCELMODE = 0x1F;
        //如果鼠标引起光标在某个窗口中移动且鼠标输入没有被捕获时，就发消息给某个窗口   
        const int WM_SETCURSOR = 0x20;
        //当光标在某个非激活的窗口中而用户正按着鼠标的某个键发送此消息给//当前窗口   
        const int WM_MOUSEACTIVATE = 0x21;
        //发送此消息给MDI子窗口//当用户点击此窗口的标题栏，或//当窗口被激活，移动，改变大小   
        const int WM_CHILDACTIVATE = 0x22;
        //此消息由基于计算机的训练程序发送，通过WH_JOURNALPALYBACK的hook程序分离出用户输入消息   
        const int WM_QUEUESYNC = 0x23;
        //此消息发送给窗口当它将要改变大小或位置   
        const int WM_GETMINMAXINFO = 0x24;
        //发送给最小化窗口当它图标将要被重画   
        const int WM_PAINTICON = 0x26;
        //此消息发送给某个最小化窗口，仅//当它在画图标前它的背景必须被重画   
        const int WM_ICONERASEBKGND = 0x27;
        //发送此消息给一个对话框程序去更改焦点位置   
        const int WM_NEXTDLGCTL = 0x28;
        //每当打印管理列队增加或减少一条作业时发出此消息    
        const int WM_SPOOLERSTATUS = 0x2A;
        //当button，combobox，listbox，menu的可视外观改变时发送   
        const int WM_DRAWITEM = 0x2B;
        //当button, combo box, list box, list view control, or menu item 被创建时   
        const int WM_MEASUREITEM = 0x2C;
        //此消息有一个LBS_WANTKEYBOARDINPUT风格的发出给它的所有者来响应WM_KEYDOWN消息    
        const int WM_VKEYTOITEM = 0x2E;
        //此消息由一个LBS_WANTKEYBOARDINPUT风格的列表框发送给他的所有者来响应WM_CHAR消息    
        const int WM_CHARTOITEM = 0x2F;
        //当绘制文本时程序发送此消息得到控件要用的颜色   
        const int WM_SETFONT = 0x30;
        //应用程序发送此消息得到当前控件绘制文本的字体   
        const int WM_GETFONT = 0x31;
        //应用程序发送此消息让一个窗口与一个热键相关连    
        const int WM_SETHOTKEY = 0x32;
        //应用程序发送此消息来判断热键与某个窗口是否有关联   
        const int WM_GETHOTKEY = 0x33;
        //此消息发送给最小化窗口，当此窗口将要被拖放而它的类中没有定义图标，应用程序能返回一个图标或光标的句柄，当用户拖放图标时系统显示这个图标或光标   
        const int WM_QUERYDRAGICON = 0x37;
        //发送此消息来判定combobox或listbox新增加的项的相对位置   
        const int WM_COMPAREITEM = 0x39;
        //显示内存已经很少了   
        const int WM_COMPACTING = 0x41;
        //发送此消息给那个窗口的大小和位置将要被改变时，来调用setwindowpos函数或其它窗口管理函数   
        const int WM_WINDOWPOSCHANGING = 0x46;
        //发送此消息给那个窗口的大小和位置已经被改变时，来调用setwindowpos函数或其它窗口管理函数   
        const int WM_WINDOWPOSCHANGED = 0x47;
        //当系统将要进入暂停状态时发送此消息   
        const int WM_POWER = 0x48;
        //当一个应用程序传递数据给另一个应用程序时发送此消息   
        const int WM_COPYDATA = 0x4A;
        //当某个用户取消程序日志激活状态，提交此消息给程序   
        const int WM_CANCELJOURNA = 0x4B;
        //当某个控件的某个事件已经发生或这个控件需要得到一些信息时，发送此消息给它的父窗口    
        const int WM_NOTIFY = 0x4E;
        //当用户选择某种输入语言，或输入语言的热键改变   
        const int WM_INPUTLANGCHANGEREQUEST = 0x50;
        //当平台现场已经被改变后发送此消息给受影响的最顶级窗口   
        const int WM_INPUTLANGCHANGE = 0x51;
        //当程序已经初始化windows帮助例程时发送此消息给应用程序   
        const int WM_TCARD = 0x52;
        //此消息显示用户按下了F1，如果某个菜单是激活的，就发送此消息个此窗口关联的菜单，否则就发送给有焦点的窗口，如果//当前都没有焦点，就把此消息发送给//当前激活的窗口   
        const int WM_HELP = 0x53;
        //当用户已经登入或退出后发送此消息给所有的窗口，//当用户登入或退出时系统更新用户的具体设置信息，在用户更新设置时系统马上发送此消息   
        const int WM_USERCHANGED = 0x54;
        //公用控件，自定义控件和他们的父窗口通过此消息来判断控件是使用ANSI还是UNICODE结构   
        const int WM_NOTIFYFORMAT = 0x55;
        //当用户某个窗口中点击了一下右键就发送此消息给这个窗口   
        //const int WM_CONTEXTMENU = ??;   
        //当调用SETWINDOWLONG函数将要改变一个或多个 窗口的风格时发送此消息给那个窗口   
        const int WM_STYLECHANGING = 0x7C;
        //当调用SETWINDOWLONG函数一个或多个 窗口的风格后发送此消息给那个窗口   
        const int WM_STYLECHANGED = 0x7D;
        //当显示器的分辨率改变后发送此消息给所有的窗口   
        const int WM_DISPLAYCHANGE = 0x7E;
        //此消息发送给某个窗口来返回与某个窗口有关连的大图标或小图标的句柄   
        const int WM_GETICON = 0x7F;
        //程序发送此消息让一个新的大图标或小图标与某个窗口关联   
        const int WM_SETICON = 0x80;
        //当某个窗口第一次被创建时，此消息在WM_CREATE消息发送前发送   
        const int WM_NCCREATE = 0x81;
        //此消息通知某个窗口，非客户区正在销毁    
        const int WM_NCDESTROY = 0x82;
        //当某个窗口的客户区域必须被核算时发送此消息   
        const int WM_NCCALCSIZE = 0x83;
        //移动鼠标，按住或释放鼠标时发生   
        const int WM_NCHITTEST = 0x84;
        //程序发送此消息给某个窗口当它（窗口）的框架必须被绘制时   
        const int WM_NCPAINT = 0x85;
        //此消息发送给某个窗口仅当它的非客户区需要被改变来显示是激活还是非激活状态   
        const int WM_NCACTIVATE = 0x86;
        //发送此消息给某个与对话框程序关联的控件，widdows控制方位键和TAB键使输入进入此控件通过应   
        const int WM_GETDLGCODE = 0x87;
        //当光标在一个窗口的非客户区内移动时发送此消息给这个窗口 非客户区为：窗体的标题栏及窗 的边框体   
        const int WM_NCMOUSEMOVE = 0xA0;
        //当光标在一个窗口的非客户区同时按下鼠标左键时提交此消息   
        //const int WM_NCLBUTTONDOWN = 0xA1;
        //当用户释放鼠标左键同时光标某个窗口在非客户区十发送此消息    
        const int WM_NCLBUTTONUP = 0xA2;
        //当用户双击鼠标左键同时光标某个窗口在非客户区十发送此消息   
        const int WM_NCLBUTTONDBLCLK = 0xA3;
        //当用户按下鼠标右键同时光标又在窗口的非客户区时发送此消息   
        const int WM_NCRBUTTONDOWN = 0xA4;
        //当用户释放鼠标右键同时光标又在窗口的非客户区时发送此消息   
        const int WM_NCRBUTTONUP = 0xA5;
        //当用户双击鼠标右键同时光标某个窗口在非客户区十发送此消息   
        const int WM_NCRBUTTONDBLCLK = 0xA6;
        //当用户按下鼠标中键同时光标又在窗口的非客户区时发送此消息   
        const int WM_NCMBUTTONDOWN = 0xA7;
        //当用户释放鼠标中键同时光标又在窗口的非客户区时发送此消息   
        const int WM_NCMBUTTONUP = 0xA8;
        //当用户双击鼠标中键同时光标又在窗口的非客户区时发送此消息   
        const int WM_NCMBUTTONDBLCLK = 0xA9;
        //WM_KEYDOWN 按下一个键   
        const int WM_KEYDOWN = 0x0100;
        //释放一个键   
        const int WM_KEYUP = 0x0101;
        //按下某键，并已发出WM_KEYDOWN， WM_KEYUP消息   
        const int WM_CHAR = 0x102;
        //当用translatemessage函数翻译WM_KEYUP消息时发送此消息给拥有焦点的窗口   
        const int WM_DEADCHAR = 0x103;
        //当用户按住ALT键同时按下其它键时提交此消息给拥有焦点的窗口   
        const int WM_SYSKEYDOWN = 0x104;
        //当用户释放一个键同时ALT 键还按着时提交此消息给拥有焦点的窗口   
        const int WM_SYSKEYUP = 0x105;
        //当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后提交此消息给拥有焦点的窗口   
        const int WM_SYSCHAR = 0x106;
        //当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后发送此消息给拥有焦点的窗口   
        const int WM_SYSDEADCHAR = 0x107;
        //在一个对话框程序被显示前发送此消息给它，通常用此消息初始化控件和执行其它任务   
        const int WM_INITDIALOG = 0x110;
        //当用户选择一条菜单命令项或当某个控件发送一条消息给它的父窗口，一个快捷键被翻译   
        const int WM_COMMAND = 0x111;
        //当用户选择窗口菜单的一条命令或//当用户选择最大化或最小化时那个窗口会收到此消息   
        //const int WM_SYSCOMMAND = 0x112;
        //发生了定时器事件   
        const int WM_TIMER = 0x113;
        //当一个窗口标准水平滚动条产生一个滚动事件时发送此消息给那个窗口，也发送给拥有它的控件   
        const int WM_HSCROLL = 0x114;
        //当一个窗口标准垂直滚动条产生一个滚动事件时发送此消息给那个窗口也，发送给拥有它的控件   
        const int WM_VSCROLL = 0x115;
        //当一个菜单将要被激活时发送此消息，它发生在用户菜单条中的某项或按下某个菜单键，它允许程序在显示前更改菜单   
        const int WM_INITMENU = 0x116;
        //当一个下拉菜单或子菜单将要被激活时发送此消息，它允许程序在它显示前更改菜单，而不要改变全部   
        const int WM_INITMENUPOPUP = 0x117;
        //当用户选择一条菜单项时发送此消息给菜单的所有者（一般是窗口）   
        const int WM_MENUSELECT = 0x11F;
        //当菜单已被激活用户按下了某个键（不同于加速键），发送此消息给菜单的所有者   
        const int WM_MENUCHAR = 0x120;
        //当一个模态对话框或菜单进入空载状态时发送此消息给它的所有者，一个模态对话框或菜单进入空载状态就是在处理完一条或几条先前的消息后没有消息它的列队中等待   
        const int WM_ENTERIDLE = 0x121;
        //在windows绘制消息框前发送此消息给消息框的所有者窗口，通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置消息框的文本和背景颜色   
        const int WM_CTLCOLORMSGBOX = 0x132;
        //当一个编辑型控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置编辑框的文本和背景颜色   
        const int WM_CTLCOLOREDIT = 0x133;
        //当一个列表框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置列表框的文本和背景颜色   
        const int WM_CTLCOLORLISTBOX = 0x134;
        //当一个按钮控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置按纽的文本和背景颜色   
        const int WM_CTLCOLORBTN = 0x135;
        //当一个对话框控件将要被绘制前发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置对话框的文本背景颜色   
        const int WM_CTLCOLORDLG = 0x136;
        //当一个滚动条控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以通过使用给定的相关显示设备的句柄来设置滚动条的背景颜色   
        const int WM_CTLCOLORSCROLLBAR = 0x137;
        //当一个静态控件将要被绘制时发送此消息给它的父窗口通过响应这条消息，所有者窗口可以 通过使用给定的相关显示设备的句柄来设置静态控件的文本和背景颜色   
        const int WM_CTLCOLORSTATIC = 0x138;
        //当鼠标轮子转动时发送此消息个当前有焦点的控件   
        const int WM_MOUSEWHEEL = 0x20A;
        //双击鼠标中键   
        const int WM_MBUTTONDBLCLK = 0x209;
        //释放鼠标中键   
        const int WM_MBUTTONUP = 0x208;
        //移动鼠标时发生，同WM_MOUSEFIRST   
        const int WM_MOUSEMOVE = 0x200;
        //按下鼠标左键   
        const int WM_LBUTTONDOWN = 0x201;
        //释放鼠标左键   
        const int WM_LBUTTONUP = 0x202;
        //双击鼠标左键   
        const int WM_LBUTTONDBLCLK = 0x203;
        //按下鼠标右键   
        const int WM_RBUTTONDOWN = 0x204;
        //释放鼠标右键   
        const int WM_RBUTTONUP = 0x205;
        //双击鼠标右键   
        const int WM_RBUTTONDBLCLK = 0x206;
        //按下鼠标中键   
        const int WM_MBUTTONDOWN = 0x207;
    }
}
