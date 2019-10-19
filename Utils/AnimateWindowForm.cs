using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace M.Core.Utils
{
    /// <summary>
    /// 窗体动画动态效果辅助类
    /// </summary>
    public class AnimateWindowForm
    {
        #region 显示窗体淡入淡出效果
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwTime"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        /// <summary>
        /// 从左到右打开窗口
        /// </summary>
        public const int AW_HOR_POSITIVE = 0x0001;
        /// <summary>
        /// 从右到左打开窗口
        /// </summary>
        public const int AW_HOR_NEGATIVE = 0x0002;
        /// <summary>
        /// 从上到下打开窗口
        /// </summary>
        public const int AW_VER_POSITIVE = 0x0004;
        /// <summary>
        /// 从下到上打开窗口
        /// </summary>
        public const int AW_VER_NEGATIVE = 0x0008;
        /// <summary>
        /// 从中央打开
        /// </summary>
        public const int AW_CENTER = 0x0010;
        /// <summary>
        /// 隐藏窗体
        /// </summary>
        public const int AW_HIDE = 0x10000;
        /// <summary>
        /// 显示窗体
        /// </summary>
        public const int AW_ACTIVATE = 0x20000;
        /// <summary>
        /// 
        /// </summary>
        public const int AW_SLIDE = 0x40000;
        /// <summary>
        /// 淡入淡出效果
        /// </summary>
        public const int AW_BLEND = 0x80000;

        /// <summary>
        /// 显示窗体淡入淡出效果
        /// </summary>
        /// <param name="from">窗体</param>
        /// <param name="isMax">最大化显示</param>
        /// <param name="feed">动画速度</param>
        public static void ShowFrom(Form from, bool isMax = true, int feed = 200)
        {
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height;
            if (isMax)
            {
                from.Size = new Size(x, y);//设置窗体大小;
                from.Location = new Point(0, 0);//设置窗体位置;
            }
            else
            {
                from.Location = new Point(x / 2 - from.Width / 2, y / 2 - from.Height / 2);//设置窗体位置;
            }
            AnimateWindow(from.Handle, feed, AW_ACTIVATE | AW_CENTER);//200指明动画持续的时间（以微秒计），完成一个动画的标准时间为200微秒。
        }
        #endregion
    }
}
