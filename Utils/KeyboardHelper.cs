
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using SendKeysProxy = System.Windows.Forms.SendKeys;

namespace M.Core.Utils
{
    /// <summary>
    /// 提供访问键盘当前状态的属性，
    /// 如什么键当前按下，提供了一种方法，以发送击键到活动窗口。
    /// </summary>
    [HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
    public class KeyboardHelper
    {
        #region 属性
        /// <summary>获取一个布尔值，表示如果ALT键是向下。</summary>
        /// <returns>一个布尔值：如果ALT键，否则为false。</returns>
        public static bool AltKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Alt) > Keys.None);
            }
        }

        /// <summary>获取一个布尔值，指示，如果已开启CAPS LOCK键。 </summary>
        /// <returns>一个布尔值：如果已开启CAPS LOCK键，则为true，否则为false。</returns>
        /// <filterpriority>1</filterpriority>
        public static bool CapsLock
        {
            get
            {
                return ((GetKeyState(20) & 1) > 0);
            }
        }

        /// <summary>获取一个布尔值，表示如果CTRL键是向下。</summary>
        /// <returns>一个布尔值。真正如果CTRL键，否则为false。</returns>
        /// <filterpriority>1</filterpriority>
        public static bool CtrlKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Control) > Keys.None);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="KeyCode"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetKeyState(int KeyCode);
        /// <summary>获取一个布尔值，表示如果NUM LOCK键是。</summary>
        /// <returns>一个布尔值。true，如果数字锁定，否则为false。</returns>
        /// <filterpriority>1</filterpriority>
        public static bool NumLock
        {
            get
            {
                return ((GetKeyState(0x90) & 1) > 0);
            }
        }

        /// <summary>获取一个布尔值，指示是否SCROLL LOCK键是。 </summary>
        /// <returns>一个布尔值。True如果滚动锁被，否则为false。</returns>
        /// <filterpriority>1</filterpriority>
        public static bool ScrollLock
        {
            get
            {
                return ((GetKeyState(0x91) & 1) > 0);
            }
        }

        /// <summary>获取一个布尔值，表示如果SHIFT键是向下。</summary>
        /// <returns>一个布尔值。真正如果SHIFT键是向下，否则为false。</returns>
        /// <filterpriority>1</filterpriority>
        public static bool ShiftKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Shift) > Keys.None);
            }
        }


        #endregion

        #region Methods
        /// <summary>发送一个或多个击键到活动窗口，如果在键盘上输入。</summary>
        /// <param name="keys">一个字符串，定义发送键。</param>
        public static void SendKeys(string keys)
        {
            SendKeys(keys, false);
        }

        /// <summary>发送一个或多个击键到活动窗口，如果在键盘上输入。</summary>
        /// <param name="keys">一个字符串，定义发送键。</param>
        /// <param name="wait">可选的。一个布尔值，指定是否等待的应用程序继续之前得到处理的击键。默认为true。</param>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /></PermissionSet>
        public static void SendKeys(string keys, bool wait)
        {
            if (wait)
            {
                SendKeysProxy.SendWait(keys);
            }
            else
            {
                SendKeysProxy.Send(keys);
            }
        }

        #endregion

        #region 全局注册热键，
        int hHook;
        Win32Api.HookProc KeyboardHookDelegate;
        /// <summary>
        /// KeyDownEvent
        /// </summary>
        public event KeyEventHandler OnKeyDownEvent;
        /// <summary>
        /// KeyUpEvent
        /// </summary>
        public event KeyEventHandler OnKeyUpEvent;
        /// <summary>
        /// KeyPressEvent
        /// </summary>
        public event KeyPressEventHandler OnKeyPressEvent;
        /// <summary>
        /// 
        /// </summary>
        public KeyboardHelper() { }
        /// <summary>
        /// 
        /// </summary>
        public void SetHook()
        {
            KeyboardHookDelegate = new Win32Api.HookProc(KeyboardHookProc);
            Process cProcess = Process.GetCurrentProcess();
            ProcessModule cModule = cProcess.MainModule;
            var mh = Win32Api.GetModuleHandle(cModule.ModuleName);
            hHook = Win32Api.SetWindowsHookEx(Win32Api.WH_KEYBOARD_LL, KeyboardHookDelegate, mh, 0);
        }
        /// <summary>
        /// 卸载钩子
        /// </summary>
        public void UnHook()
        {
            Win32Api.UnhookWindowsHookEx(hHook);
        }
        private List<Keys> preKeysList = new List<Keys>();//存放被按下的控制键，用来生成具体的键
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //如果该消息被丢弃（nCode<0）或者没有事件绑定处理程序则不会触发事件
            if ((nCode >= 0) && (OnKeyDownEvent != null || OnKeyUpEvent != null || OnKeyPressEvent != null))
            {
                Win32Api.KeyboardHookStruct KeyDataFromHook = (Win32Api.KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.KeyboardHookStruct));
                Keys keyData = (Keys)KeyDataFromHook.vkCode;
                //按下控制键
                if ((OnKeyDownEvent != null || OnKeyPressEvent != null) && (wParam == Win32Api.WM_KEYDOWN || wParam == Win32Api.WM_SYSKEYDOWN))
                {
                    if (IsCtrlAltShiftKeys(keyData) && preKeysList.IndexOf(keyData) == -1)
                    {
                        preKeysList.Add(keyData);
                    }
                }
                //WM_KEYDOWN和WM_SYSKEYDOWN消息，将会引发OnKeyDownEvent事件
                if (OnKeyDownEvent != null && (wParam == Win32Api.WM_KEYDOWN || wParam == Win32Api.WM_SYSKEYDOWN))
                {
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(keyData));

                    OnKeyDownEvent(this, e);
                }
                //WM_KEYDOWN消息将引发OnKeyPressEvent 
                if (OnKeyPressEvent != null && wParam == Win32Api.WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    Win32Api.GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (Win32Api.ToAscii(KeyDataFromHook.vkCode, KeyDataFromHook.scanCode, keyState, inBuffer, KeyDataFromHook.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                        OnKeyPressEvent(this, e);
                    }
                }
                //松开控制键
                if ((OnKeyDownEvent != null || OnKeyPressEvent != null) && (wParam == Win32Api.WM_KEYUP || wParam == Win32Api.WM_SYSKEYUP))
                {
                    if (IsCtrlAltShiftKeys(keyData))
                    {
                        for (int i = preKeysList.Count - 1; i >= 0; i--)
                        {
                            if (preKeysList[i] == keyData) { preKeysList.RemoveAt(i); }
                        }
                    }
                }
                //WM_KEYUP和WM_SYSKEYUP消息，将引发OnKeyUpEvent事件 
                if (OnKeyUpEvent != null && (wParam == Win32Api.WM_KEYUP || wParam == Win32Api.WM_SYSKEYUP))
                {
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(keyData));
                    OnKeyUpEvent(this, e);
                }
            }
            return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
        }
        //根据已经按下的控制键生成key
        private Keys GetDownKeys(Keys key)
        {
            Keys rtnKey = Keys.None;
            foreach (Keys i in preKeysList)
            {
                if (i == Keys.LControlKey || i == Keys.RControlKey) { rtnKey = rtnKey | Keys.Control; }
                if (i == Keys.LMenu || i == Keys.RMenu) { rtnKey = rtnKey | Keys.Alt; }
                if (i == Keys.LShiftKey || i == Keys.RShiftKey) { rtnKey = rtnKey | Keys.Shift; }
            }
            return rtnKey | key;
        }
        private Boolean IsCtrlAltShiftKeys(Keys key)
        {
            if (key == Keys.LControlKey || key == Keys.RControlKey || key == Keys.LMenu || key == Keys.RMenu || key == Keys.LShiftKey || key == Keys.RShiftKey) { return true; }
            return false;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class Win32Api
    {
        #region 常数和结构
        /// <summary>
        /// 
        /// </summary>
        public const int WM_KEYDOWN = 0x100;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_KEYUP = 0x101;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_SYSKEYDOWN = 0x104;
        /// <summary>
        /// 
        /// </summary>
        public const int WM_SYSKEYUP = 0x105;
        /// <summary>
        /// 
        /// </summary>
        public const int WH_KEYBOARD_LL = 13;

        /// <summary>
        /// 声明键盘钩子的封送结构类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            /// <summary>
            /// 表示一个在1到254间的虚似键盘码 
            /// </summary>
            public int vkCode; //
                               /// <summary>
                               /// 表示硬件扫描码
                               /// </summary>
            public int scanCode; //
                                 /// <summary>
                                 /// 
                                 /// </summary>
            public int flags;
            /// <summary>
            /// 
            /// </summary>
            public int time;
            /// <summary>
            /// 
            /// </summary>
            public int dwExtraInfo;
        }
        #endregion
        #region Api
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        /// <summary>
        /// 安装钩子的函数
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn"></param>
        /// <param name="hInstance"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        /// <summary>
        /// 卸下钩子的函数
        /// </summary>
        /// <param name="idHook"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);
        /// <summary>
        /// 下一个钩挂的函数
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uVirtKey"></param>
        /// <param name="uScanCode"></param>
        /// <param name="lpbKeyState"></param>
        /// <param name="lpwTransKey"></param>
        /// <param name="fuState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
    }
}
