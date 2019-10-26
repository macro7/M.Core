using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace M.Core.Utils
{
    ///<summary>
    ///打开、保存文件对话框操作辅助类
    ///</summary>
    public class FileDialogHelper
    {
        public enum FilterString
        {
            /// <summary>
            /// 男
            /// </summary>
            [Description("Excel(*.xls)|*.xls|Excel(*.xlsx)|*.xlsx|All File(*.*)|*.*")]
            ExcelFilter = 0,
            /// <summary>
            /// 女
            /// </summary>
            [Description("Image Files(*.BMP;*.bmp;*.JPG;*.jpg;*.GIF;*.gif;*.png)|(*.BMP;*.bmp;*.JPG;*.jpg;*.GIF;*.gif;*.png)|All File(*.*)|*.*")]
            ImageFilter = 1,
            [Description("HTML files (*.html;*.htm)|*.html;*.htm|All files (*.*)|*.*")]
            HtmlFilter = 2,
            [Description("Access(*.mdb)|*.mdb|All File(*.*)|*.*")]
            AccessFilter = 3,
            [Description("Zip(*.zip)|*.zip|All files (*.*)|*.*")]
            ZipFillter = 4,
            [Description("配置文件(*.cfg)|*.cfg|All File(*.*)|*.*")]
            ConfigFilter = 5,
            [Description("(*.txt)|*.txt|All files (*.*)|*.*")]
            TxtFilter = 6
        }

        public string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
            {
                return value;
            }

            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        public static readonly string ExcelFilter = "Excel(*.xls)|*.xls|All File(*.*)|*.*";
        public static readonly string ImageFilter = "Image Files(*.BMP;*.bmp;*.JPG;*.jpg;*.GIF;*.gif;*.png)|(*.BMP;*.bmp;*.JPG;*.jpg;*.GIF;*.gif;*.png)|All File(*.*)|*.*";
        public static readonly string HtmlFilter = "HTML files (*.html;*.htm)|*.html;*.htm|All files (*.*)|*.*";
        public static readonly string AccessFilter = "Access(*.mdb)|*.mdb|All File(*.*)|*.*";
        public static readonly string ZipFillter = "Zip(*.zip)|*.zip|All files (*.*)|*.*";
        public static readonly string ConfigFilter = "配置文件(*.cfg)|*.cfg|All File(*.*)|*.*";
        public static readonly string TxtFilter = "(*.txt)|*.txt|All files (*.*)|*.*";

        ///<summary>
        ///Initializes a new instance of the <see cref="FileDialogHelper"/> class.
        ///</summary>
        private FileDialogHelper()
        {
        }

        #region 通用函数

        public static string OpenDir()
        {
            return OpenDir(string.Empty);
        }

        public static string OpenDir(string selectedPath)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择路径";
            dialog.SelectedPath = selectedPath;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 打开文件选择框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="filter">过滤扩展名如："(*.txt)|*.txt|All files (*.*)|*.*"</param>
        /// <param name="filename">文件路径</param>
        /// <returns>System.String.</returns>
        public static string OpenFileDialog(string title, string filter, string filename)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.Title = title;
            dialog.RestoreDirectory = true;
            dialog.FileName = filename;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 打开文件选择框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="filter">过滤扩展名："(*.txt)|*.txt|All files (*.*)|*.*"</param>
        /// <returns></returns>
        public static string OpenFileDialog(string title, string filter)
        {
            return OpenFileDialog(title, filter, string.Empty);
        }

        /// <summary>
        /// 打开保存文件路径选择框
        /// </summary>
        /// <param name="title">标题.</param>
        /// <param name="filter">过滤扩展名.："(*.txt)|*.txt|All files (*.*)|*.*"</param>
        /// <returns></returns>
        public static string SaveFileDialog(string title, string filter, string filename)
        {
            return SaveFileDialog(title, filter, filename, "");
        }

        /// <summary>
        /// 打开保存文件路径选择框
        /// </summary>
        /// <param name="title">标题.</param>
        /// <param name="filter">过滤扩展名.："(*.txt)|*.txt|All files (*.*)|*.*"</param>
        /// <param name="filename">文件路径.</param>
        /// <param name="initialDirectory">根目录.</param>
        /// <returns>System.String.</returns>
        public static string SaveFileDialog(string title, string filter, string filename, string initialDirectory)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = filter;
            dialog.Title = title;
            dialog.FileName = filename;
            dialog.RestoreDirectory = true;
            if (!string.IsNullOrEmpty(initialDirectory))
            {
                dialog.InitialDirectory = initialDirectory;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 打开保存文件路径选择框
        /// </summary>
        /// <param name="title">标题.</param>
        /// <param name="filter">过滤扩展名.："(*.txt)|*.txt|All files (*.*)|*.*"</param>
        /// <returns></returns>
        public static string SaveFileDialog(string title, string filter)
        {
            return SaveFileDialog(title, filter, string.Empty);
        }

        #endregion

        #region 获取颜色对话框的颜色
        /// <summary>
        /// 颜色
        /// </summary>
        /// <returns>Color.</returns>
        public static Color PickColor()
        {
            Color result = SystemColors.Control;
            ColorDialog form = new ColorDialog();
            if (DialogResult.OK == form.ShowDialog())
            {
                result = form.Color;
            }
            return result;
        }

        public static Color PickColor(Color color)
        {
            Color result = SystemColors.Control;
            ColorDialog form = new ColorDialog
            {
                Color = color
            };
            if (DialogResult.OK == form.ShowDialog())
            {
                result = form.Color;
            }
            return result;
        }
        #endregion
    }
}
