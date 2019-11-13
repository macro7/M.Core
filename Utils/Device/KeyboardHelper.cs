using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using SendKeysProxy = System.Windows.Forms.SendKeys;

namespace M.Core.Utils
{
    /// <summary>
    /// 键盘操作辅助类，提供属性访问敲击那个键，以及发送软键盘消息等操作。
    /// </summary>
    [HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
    public class KeyboardHelper
    {

        const uint KEYEVENTF_EXTENDEDKEY = 0x1;
        const uint KEYEVENTF_KEYUP = 0x2;

        [DllImport("user32.dll")]
        static extern short GetKeyState(int nVirtKey);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public enum VirtualKeys : byte
        {
            /// <summary>
            /// 数字锁定键
            /// </summary>
            VK_NUMLOCK = 0x90, //数字锁定键
            /// <summary>
            /// 滚动锁定
            /// </summary>
            VK_SCROLL = 0x91,  //滚动锁定
            /// <summary>
            /// 大小写锁定
            /// </summary>
            VK_CAPITAL = 0x14, //大小写锁定
            VK_A = 62
        }

        /// <summary>
        /// 获取键盘状态是否开启或者关闭
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static bool GetState(VirtualKeys Key)
        {
            return (GetKeyState((int)Key) == 1);
        }

        /// <summary>
        /// 开启关闭键盘
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="State"></param>
        public static void SetState(VirtualKeys Key, bool State)
        {
            if (State != GetState(Key))
            {
                keybd_event((byte)Key, 0x45, KEYEVENTF_EXTENDEDKEY | 0, 0);
                keybd_event((byte)Key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            }
        }

        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        /// <summary>
        /// 
        #region 键盘属性

        /// <summary>
        /// 判断ALT建是否按下
        /// </summary>
        public static bool AltKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Alt) > Keys.None);
            }
        }

        /// <summary>
        /// 判断Caps Lock大写键是否打开
        /// </summary>
        public static bool CapsLock
        {
            get
            {
                return GetKeyState((int)VirtualKeys.VK_CAPITAL) == 1;
            }
        }

        public static void SwitchCapsLock(bool open)
        {
            SetState(VirtualKeys.VK_CAPITAL, open);
        }


        /// <summary>
        /// 判断CTRL键是否按下
        /// </summary>
        public static bool CtrlKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Control) > Keys.None);
            }
        }

        /// <summary>
        /// 判断Num Lock 数字键是否打开
        /// </summary>
        public static bool NumLock
        {
            get
            {
                return GetKeyState((int)VirtualKeys.VK_NUMLOCK) == 1;
            }
        }


        /// 判断Scroll Lock滚动锁定键是否打开
        /// </summary>
        public static bool ScrollLock
        {
            get
            {
                return GetKeyState((int)VirtualKeys.VK_SCROLL) == 1;
            }
        }

        /// <summary>
        /// 判断Shift键是否按下
        /// </summary>
        public static bool ShiftKeyDown
        {
            get
            {
                return ((Control.ModifierKeys & Keys.Shift) > Keys.None);
            }
        }


        #endregion

        #region 操作方法

        /// <summary>
        /// 发送一个或多个击键到活动窗口。
        /// </summary>
        /// <param name="keys">定义发送键的字符串。特殊控制键代码SHIFT(+),CTRL(^), ALT(%)。Send("{SPACE}")、Send("+{TAB}")</param>
        public static void SendKeys(string keys)
        {
            SendKeys(keys, false);
        }

        /// <summary>
        /// 发送一个或多个击键到活动窗口
        /// </summary>
        /// <param name="keys">定义发送键的字符串.特殊控制键代码SHIFT(+),CTRL(^), ALT(%)。Send("{SPACE}")、Send("+{TAB}")</param>
        /// <param name="wait">指定是否要等待应用程序继续之前得到处理的击键。默认为True。
        /// </param>
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
    }
}
