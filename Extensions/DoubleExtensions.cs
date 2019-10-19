namespace M.Core.Extensions
{
    /// <summary>
    /// double类型扩展类
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// 转化成double去掉末尾0
        /// </summary>
        /// <param name="d">.</param>
        /// <returns></returns>
        public static string WithoutEndZero(this double d)
        {
            return d.ToString("G0");
        }
    }
}
