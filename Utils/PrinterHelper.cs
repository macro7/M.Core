using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace M.Core.Utils
{
    /// <summary>
    /// 打印帮助类
    /// </summary>
    public class PrinterHelper
    {
        /// <summary>
        /// 设置本次输出默认打印机（不更改系统默认打印机）
        /// </summary>
        /// <param name="printerName">打印机名称</param>
        public static void SetDocumentPrinter(string printerName)
        {
            PrintDocument prtdoc = new PrintDocument();
            prtdoc.PrinterSettings.PrinterName = printerName;
        }

        /// <summary>
        /// 获取默认打印机
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrinterName()
        {
            PrintDocument prtdoc = new PrintDocument();
            return prtdoc.PrinterSettings.PrinterName;//获取默认的打印机名
        }

        /// <summary>
        /// 校验是否存在名称为 printerName 的打印机
        /// </summary>
        /// <param name="printerName">打印机名称</param>
        /// <returns></returns>
        public static bool ExistsPrinterName(string printerName)
        {
            foreach (string ss in PrinterSettings.InstalledPrinters)
            {
                //在列表框中列出所有的打印机,
                if (ss.ToLower() == printerName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 调用windows系统函数，设置系统默认打印机
        /// </summary>
        /// <param name="printerName">打印机名称</param>
        /// <returns></returns>
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long SetDefaultPrinter(string printerName);

        ///// <summary>
        ///// 弹出打印窗体的预览对话框
        ///// </summary>
        ///// <param name="form">窗体对象</param>
        //public static void Print(Form form)
        //{
        //    ScreenCapture capture = new ScreenCapture();
        //    Image image = capture.CaptureWindow(form.Handle);
        //    ImagePrintHelper helper = new ImagePrintHelper(image);
        //    helper.PrintPreview();
        //}

        ///// <summary>
        ///// 打印窗体控件
        ///// </summary>
        ///// <param name="control">控件对象</param>
        //public static void Print(Control control)
        //{
        //    ScreenCapture capture = new ScreenCapture();
        //    Image image = capture.CaptureWindow(control.Handle);

        //    ImagePrintHelper helper = new ImagePrintHelper(image);
        //    helper.PrintPreview();
        //}

    }
}