namespace M.Core.Extensions
{
    public static class MySqlConnectionExtensions
    {
        ///// <summary>
        ///// 快速打开连接
        ///// </summary>
        ///// <param name="conn">连接对象</param>
        ///// <param name="timeout">等待时间 毫秒</param>
        ///// <returns></returns>
        //public static bool QuickOpen(this MySqlConnection conn, int timeout = 1500)
        //{
        //    try
        //    {
        //        // 开始计时
        //        Stopwatch sw = new Stopwatch();
        //        bool connectSuccess = false;

        //        // 尝试打开
        //        Thread t = new Thread(delegate ()
        //        {
        //            try
        //            {
        //                sw.Start();
        //                conn.Open();
        //                connectSuccess = true;
        //            }
        //            catch { }
        //        });

        //        // 开始打开链接
        //        t.IsBackground = true;
        //        t.Start();
        //        //等待时间判断
        //        while (timeout > sw.ElapsedMilliseconds)
        //        {
        //            if (connectSuccess)
        //            {
        //                break;
        //            }
        //        }
        //        return connectSuccess;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}
