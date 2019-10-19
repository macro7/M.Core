using System;
using System.Drawing;

namespace M.Core.Utils
{
    class ColorHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int ColorToDecimal(Color color)
        {
            return HexToDecimal(ColorToHex(color));
        }
        /// <summary>
        /// 颜色转string
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string ColorToHex(Color color)
        {
            return string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HexToColor(string hex)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            string a = string.Empty;

            if (hex.Length == 8)
            {
                a = hex.Substring(0, 2);
                hex = hex.Substring(2);
            }

            string r = hex.Substring(0, 2);
            string g = hex.Substring(2, 2);
            string b = hex.Substring(4, 2);

            if (string.IsNullOrEmpty(a))
            {
                return Color.FromArgb(HexToDecimal(r), HexToDecimal(g), HexToDecimal(b));
            }
            else
            {
                return Color.FromArgb(HexToDecimal(a), HexToDecimal(r), HexToDecimal(g), HexToDecimal(b));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static int HexToDecimal(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }
    }
}
