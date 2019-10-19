using System.Runtime.InteropServices;

namespace M.Core.Utils
{
    /// <summary>
    /// 鼠标控制类
    /// </summary>
    public class CursorUtil
    {
        /// <summary>
        /// 鼠标显示隐藏
        /// </summary>
        /// <param name="status">1：显示；0：隐藏</param>
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public static extern void ShowCursor(int status);
    }
}
