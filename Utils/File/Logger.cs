using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace M.Core.Utils
{
    /* ========================================================================
   * 【本类功能概述】   Logger 日志帮助类，提供日志记录方法（错误日志，操作日志，服务调用日志），包括记录本地日志（生成txt记事本）、记录到数据库
   * 
   * 作者：张宏杰       时间：2016/6/5 14:14:54
   * 文件名：XtraFramework.Utils.Logger
   * CLR版本：4.0.30319.235
   *
   * 修改者：           时间：              
   * 修改说明：
   * ========================================================================*/
    /// <summary>
    /// 
    /// </summary>
    public sealed class Logger
    {
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="exception">异常类</param>
        public static void WriteErrorLog(Exception exception)
        {
            try
            {
                DeleteLogInfo();
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(fileName))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(fileName);
                }
                //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
                fileName += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                WriteLogInfo(exception, fileName);
            }
            catch { }
        }
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="logString">异常类</param>
        public static void WriteErrorLog(string logString)
        {
            try
            {
                DeleteLogInfo();
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(fileName))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(fileName);
                }
                //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
                fileName += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                MethodBase methodName = new StackTrace().GetFrame(1).GetMethod();
                WriteLogInfo(methodName, logString, fileName);
            }
            catch { }
        }
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="exception">异常类</param>
        /// <param name="logType">日志类型</param>
        public static void WriteErrorLog(Exception exception, LogType logType)
        {
            try
            {
                DeleteLogInfo();
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                switch (logType)
                {
                    case LogType.Error:
                        fileName = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), "Error");
                        break;
                    case LogType.Message:
                        fileName = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), "LogFiles");
                        break;
                }
                if (!Directory.Exists(fileName))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(fileName);
                }
                //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
                fileName += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                WriteLogInfo(exception, fileName);
            }
            catch { }
        }
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="logString">异常类</param>
        /// <param name="logType">日志类型</param>
        public static void WriteLog(string logString, LogType logType)
        {
            try
            {
                DeleteLogInfo();
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                switch (logType)
                {
                    case LogType.Error:
                        fileName = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), "Error");
                        break;
                    case LogType.Message:
                        fileName = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), "LogFiles");
                        break;
                }
                if (!Directory.Exists(fileName))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(fileName);
                }
                MethodBase methodName = new StackTrace().GetFrame(1).GetMethod();
                //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
                fileName += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                WriteLogInfo(methodName, logString, fileName);
            }
            catch { }
        }
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="folderPath">日志文件目录</param>
        /// <param name="exception">异常类</param>
        public static void WriteErrorLog(string folderPath, Exception exception)
        {
            try
            {
                DeleteLogInfo();
                if (!Directory.Exists(folderPath))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(folderPath);
                }
                //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
                folderPath += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                WriteLogInfo(exception, folderPath);
            }
            catch { }
        }
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="folderPath">日志目标文件夹</param>
        /// <param name="fileName">日志文件名称</param>
        /// <param name="logString">日志内容</param>
        public static void WriteLog(string folderPath, string fileName, string logString)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(folderPath);
                }
                fileName = Path.Combine(folderPath, fileName);
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }

                StreamWriter sw = null;
                try
                {
                    using (sw = new StreamWriter(fileName, true, Encoding.Default))
                    {
                        //sw.WriteLine("*****************************************【"
                        //              + DateTime.Now.ToLongTimeString()
                        //              + "】*****************************************");
                        if (!string.IsNullOrEmpty(logString))
                        {
                            sw.WriteLine(logString);
                        }
                        else
                        {
                            sw.WriteLine("Exception is empty");
                        }
                        sw.WriteLine();
                        sw.Dispose();
                        sw.Close();
                    }
                }
                catch
                {
                }
                finally
                {
                    //3、关闭流
                    sw.Close();
                }
            }
            catch { }
        }
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="logString">异常类</param>
        public static void WriteLog(string logString)
        {
            try
            {
                DeleteLogInfo();
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(folderPath))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(folderPath);
                }
                //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
                folderPath += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                WriteLogInfo(logString, folderPath);
            }
            catch { }
        }
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="exception">异常类</param>
        public static void WriteLog(Exception exception)
        {
            try
            {
                DeleteLogInfo();
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(folderPath))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(folderPath);
                }
                //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
                folderPath += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                WriteLogInfo(exception, folderPath);
            }
            catch { }
        }
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="folderPath">日志文件目录</param>
        /// <param name="exception">异常类</param>
        public static void WriteLog(string folderPath, string exception)
        {
            try
            {
                DeleteLogInfo();
                if (!Directory.Exists(folderPath))
                {
                    //创建日志文件夹
                    Directory.CreateDirectory(folderPath);
                }
                //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
                folderPath += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                MethodBase methodName = new StackTrace().GetFrame(1).GetMethod();
                WriteLogInfo(methodName, exception, folderPath);
            }
            catch { }
        }
        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="exception">异常类</param>
        /// <param name="fileName">日志文件存放路径</param>
        private static void WriteLogInfo(Exception exception, string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            StreamWriter sw = null;
            try
            {
                using (sw = new StreamWriter(fileName, true, Encoding.Default))
                {
                    sw.WriteLine("*****************************************【"
                                  + DateTime.Now.ToLongTimeString()
                                  + "】*****************************************");
                    if (exception != null)
                    {
                        sw.WriteLine("【实例的运行时类型】" + exception.GetType());
                        sw.WriteLine("【导致异常的应用程序或对象名称】" + exception.Source.Trim());
                        sw.WriteLine("【引发异常的方法】" + exception.TargetSite);
                        sw.WriteLine("【异常信息】" + exception.Message.Trim());
                        sw.WriteLine("【堆栈】" + exception.StackTrace.Trim());
                    }
                    else
                    {
                        sw.WriteLine("Exception is NULL");
                    }
                    sw.WriteLine();
                    sw.Dispose();
                    sw.Close();
                }
            }
            catch
            {
            }
            finally
            {
                //3、关闭流
                sw.Close();
            }
        }
        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="methodName">调用者</param>
        /// <param name="logString">异常类</param>
        /// <param name="fileName">日志文件存放路径</param>
        private static void WriteLogInfo(MethodBase methodName, string logString, string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            StreamWriter sw = null;
            try
            {
                using (sw = new StreamWriter(fileName, true, Encoding.Default))
                {
                    sw.WriteLine("*****************************************【"
                                  + DateTime.Now.ToLongTimeString()
                                  + "】*****************************************");
                    if (logString != null)
                    {
                        sw.WriteLine("【实例的运行时类型】" + logString.GetType());
                        sw.WriteLine("【导致异常的应用程序或对象名称】" + methodName.Module.ToString());
                        sw.WriteLine("【引发异常的方法】" + methodName.Name);
                        sw.WriteLine("【异常信息】" + logString);
                        sw.WriteLine("【堆栈】" + methodName.ReflectedType.Namespace);
                    }
                    else
                    {
                        sw.WriteLine("Exception is NULL");
                    }
                    sw.WriteLine();
                    sw.Dispose();
                    sw.Close();
                }
            }
            catch
            {
            }
            finally
            {
                //3、关闭流
                sw.Close();
            }
        }
        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="logString">异常类</param>
        /// <param name="fileName">日志文件存放路径</param>
        private static void WriteLogInfo(string logString, string fileName)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            StreamWriter sw = null;
            try
            {
                using (sw = new StreamWriter(fileName, true, Encoding.Default))
                {
                    sw.WriteLine("*****************************************【"
                                  + DateTime.Now.ToLongTimeString()
                                  + "】*****************************************");
                    if (!string.IsNullOrEmpty(logString))
                    {
                        sw.WriteLine("【内容】" + logString);
                    }
                    else
                    {
                        sw.WriteLine("Exception is empty");
                    }
                    sw.WriteLine();
                    sw.Dispose();
                    sw.Close();
                }
            }
            catch
            {
            }
            finally
            {
                //3、关闭流
                sw.Close();
            }
        }
        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="logString">The log string.</param>
        public static void WriteOperationLog(string logString)
        {
            DeleteLogInfo();
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(folderPath))
            {
                //创建日志文件夹
                Directory.CreateDirectory(folderPath);
            }
            string fileName = Path.Combine(folderPath, DateTime.Now.ToString("操作日志yyyyMMdd") + ".log");
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }
            //发生异常每天都创建一个单独的日志文件[*.log],每天的错误信息都在这一个文件里。方便查找
            StreamWriter sw = null;
            try
            {
                using (sw = new StreamWriter(fileName, true, Encoding.Default))
                {
                    if (!string.IsNullOrEmpty(logString))
                    {
                        sw.Write(DateTime.Now.ToLongTimeString() + "【内容】" + logString);
                    }
                    else
                    {
                        sw.WriteLine("Exception is empty");
                    }
                    sw.WriteLine();
                    sw.Dispose();
                    sw.Close();
                }
            }
            catch
            {
            }
            finally
            {
                //3、关闭流
                sw.Close();
            }
        }
        /// <summary>
        /// 删除30天前日志文件
        /// </summary>
        private static void DeleteLogInfo()
        {
            try
            {
                string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                string[] strFiles = System.IO.Directory.GetFiles(strPath);
                //遍历所有文件
                foreach (string strFile in strFiles)
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(strFile);
                    //删除文件
                    if ((DateTime.Now - file.CreationTime).Days > 30)
                    {
                        file.Delete();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// 
            /// </summary>
            Error = 1,
            /// <summary>
            /// 
            /// </summary>
            Message = 0
        };


        private static string GetVersion()
        {
            try
            {
                return Application.ProductVersion;
            }
            catch
            {
                return Environment.Version.ToString();
            }
        }
    }
}
