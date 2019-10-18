using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace M.Core.Extensions
{
    /// <summary>
    /// MsSql数据库连接扩张方法
    /// </summary>
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// 快速打开连接
        /// </summary>
        /// <param name="conn">连接对象</param>
        /// <param name="timeout">等待时间 毫秒</param>
        /// <returns></returns>
        public static bool QuickOpen(this SqlConnection conn, int timeout = 1500)
        {
            try
            {
                // 开始计时
                Stopwatch sw = new Stopwatch();
                bool connectSuccess = false;

                // 尝试打开
                Thread t = new Thread(delegate ()
                {
                    try
                    {
                        sw.Start();
                        conn.Open();
                        connectSuccess = true;
                    }
                    catch { }
                });

                // 开始打开链接
                t.IsBackground = true;
                t.Start();
                //等待时间判断
                while (timeout > sw.ElapsedMilliseconds)
                {
                    if (connectSuccess)
                    {
                        break;
                    }
                }
                return connectSuccess;
            }
            catch
            {
                return false;
            }
        }
    }
}
