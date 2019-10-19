namespace M.Core.Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// 转化成double去掉末尾0
        /// </summary>
        /// <param name="d">.</param>
        /// <returns></returns>
        public static string WithoutEndZero(this decimal d)
        {
            return d.ToString("G0");
        }
    }
}
