using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace M.Core.Utils
{
    public class WindowsHelper
    {
        /// <summary>
        /// 截屏
        /// </summary>
        /// <returns></returns>
        public static Bitmap ShotScreen()
        {
            try
            {
                System.Threading.Thread.Sleep(200);
                Bitmap bit = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bit);
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), bit.Size);
                g.Dispose();
                return bit;
            }
            catch
            {
                return null;
            }
        }

        #region 设置软件自动启动
        static RegistryKey registryKeyApp = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

        /// <summary>
        /// 软件是否设置系统自动启动
        /// </summary>
        /// <param name="app">软件名称</param>
        /// <returns></returns>
        public static bool WillRunAutoStartup(string app)
        {
            try
            {
                return string.Equals(registryKeyApp.GetValue(app), Environment.CommandLine);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 系统设置/取消自动启动
        /// </summary>
        /// <param name="app">软件名称</param>
        /// <param name="shouldRun">设置/取消自动启动</param>
        public static void RunAutoStartup(string app, bool shouldRun)
        {
            RunAutoStartup(app, shouldRun, Environment.CommandLine);
        }

        /// <summary>
        /// 系统设置/取消自动启动
        /// </summary>
        /// <param name="app">软件名称</param>
        /// <param name="shouldRun">是否设置</param>
        /// <param name="exePath">系统执行路径（可增加配置参数）</param>
        public static void RunAutoStartup(string app, bool shouldRun, string exePath)
        {
            try
            {
                if (shouldRun)
                {
                    registryKeyApp.SetValue(app, exePath);
                }
                else
                {
                    registryKeyApp.DeleteValue(app, false);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Unable to RunAutoStartup: " + ex);
            }
        }

        #endregion

        #region 只运行一个实例

        /// <summary>
        /// 获取软件运行的系统进程对象
        /// Process instance = WindowsHelper.RunningInstance();
        /// if (instance == null)
        /// {
        ///     //正常启动的代码
        /// }
        /// else
        /// {
        ///      WindowsHelper.HandleRunningInstance(instance);
        /// }
        /// </summary>
        /// <returns></returns>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表 
            foreach (Process process in processes)
            {
                //Ignore the current process 
                if (process.Id != current.Id)
                {
                    return process;
                }
            }
            return null;
        }

        /// <summary>
        /// 处理重复运行的事件
        /// </summary>
        /// <param name="instance">系统进程对象</param>
        /// <param name="message">提示消息</param>
        public static void HandleRunningInstance(Process instance)
        {
            HandleRunningInstance(instance, null);
        }

        /// <summary>
        /// 处理重复运行的事件
        /// </summary>
        /// <param name="instance">系统进程对象</param>
        /// <param name="message">提示消息</param>
        public static void HandleRunningInstance(Process instance, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL); //调用api函数，正常显示窗口 
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端。 
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);

        /// <summary>
        /// 设置指定窗体前置
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(System.IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;

        /// <summary>
        /// 将被激活的最顶层窗口
        /// </summary>
        /// <param name="hWnd">窗口的句柄</param>
        /// <returns>若函数调用成功，则返回原先活动窗口的句柄。若函数调用失败，则返回值为NULL。若要获得更多错误信息，可以调用GetLastError函数。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);
        public static void ActivateWindow(IntPtr handle)
        {
            SetForegroundWindow(handle);
            SetActiveWindow(handle);
        }

        #endregion 

        #region 辅助函数
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        /// <summary>
        /// 获取当前进程
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// 提升进程权限
        /// </summary>
        /// <param name="h">第一参数是要修改访问权限的进程句柄</param>
        /// <param name="acc">第二个参数指定你要进行的操作类型，如要修改令牌我们要指定第二个参数为TOKEN_ADJUST_PRIVILEGES（其它一些参数可参考Platform SDK）。通过这个函数我们就可以得到当前进程的访问令牌的句柄（指定函数的第一个参数为GetCurrentProcess()就可以了）。</param>
        /// <param name="phtok">第三个参数就是返回的访问令牌指针</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        /// <summary>
        /// 查看系统权限的特权值，返回信息到一个LUID结构体里
        /// </summary>
        /// <param name="host">第一个参数表示所要查看的系统，本地系统直接用NULL</param>
        /// <param name="name">第二个参数表示所要查看的特权信息的名称，定义在winnt.h中，具体指请MSDN索引“windows nt privileges”</param>
        /// <param name="pluid">第三个参数用来接收所返回的制定特权名称的信息</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        /// <summary>
        /// 启用或禁止 指定访问令牌的特权
        /// </summary>
        /// <param name="TokenHandle">包含要修改特权的访问令牌的标识(句柄).这个句柄必须有TOKEN_ADJUST_PRIVILEGES访问令牌.如果PreviousState不是NULL,这个句柄还必须有TOKEN_QUERY访问特权. </param>
        /// <param name="DisableAllPrivileges">标志这个函数是否禁用该令牌的所有特权.如果为TRUE,这个函数禁用所有特权，NewState参数无效.如果为假，以NewState参数指针的信息为基础来修改特权. </param>
        /// <param name="NewState">一个TOKEN_PRIVILEGES结构体的指针指定了一组特权和他们的属性. 
        　　    /// 如果参数DisableAllPrivileges为FALSE,AdjustTokenPrivileges 启用或禁用这些令牌的特权. 
        　　    /// 如果你给一个特权设置了SE_PRIVILEGE_ENABLED的属性,这个函数将启动特权,否则禁用特权. 
        /// 如果DisableAllPrivileges为TRUE,这个参数无效. </param>
        /// <param name="BufferLength">标志参数PreviousState指针以字节大小缓存区(sizeof). 如果参数PreviousState是NULL,这个参数可以为NULL. </param>
        /// <param name="PreviousState">这个函数填充一个TOKEN_PRIVILEGES结构体【指针】,它包括该函数修改之前任何特权状态.这个参数可以为NULL. 
        　　    /// 如果指定的缓冲区太小，无法收到完整的修改权限列表，这个函数失败并不会修改任何特权. 
        /// 这个函数设置了一个 拥有修改权限完成列表【 参数ReturnLength 】的字节数 的指针变量.&#91;结果的Buffer&#93; </param>
        /// <param name="ReturnLength">接收 参数PreviousState的缓存区指针的 字节大小 的 变量指针(长度指针). 
        /// 如果PreviousState为NULL,这个参数可以为NULL. </param>
        /// <returns>如果这个函数成功,返回非0.为了确定这个函数是否修改了所有指定的特权,可以调用GetLastError函数,当这个函数返回下面的值之一时就代表函数成功: </returns>
        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TokPriv1Luid NewState, int BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        /// <summary>
        /// 退出、重启或注销系统
        /// </summary>
        /// <param name="uFlags">关闭参数
        /// 1.EWX_FORCE
        　　    /// 强制终止进程。当此标志设置，Windows不会发送消息WM_QUERYENDSESSION和WM_ENDSESSION的消息给目前在系统中运行的程序。这可能会导致应用程序丢失数据。因此，你应该只在紧急情况下使用此标志。
        　　    /// 2.EWX_LOGOFF
        　　    /// 关闭所有进程，然后注销用户。
        　　    /// 3.EWX_POWEROFF
        　　    /// 关闭系统并关闭电源。该系统必须支持断电。
        　　    /// Windows要求：
        　　    /// Windows NT中调用进程必须有 SE_SHUTDOWN_NAME 特权。
        　　    /// Windows 9X中：可以直接调用。
        　　    /// 4.EWX_REBOOT
        　　    /// 关闭系统，然后重新启动系统。
        　　    /// Windows要求：
        　　    /// Windows NT中：调用进程必须有SE_SHUTDOWN_NAME特权。
        　　    /// Windows 9X中：可以直接调用。
        　　    /// 5.EWX_SHUTDOWN
        　　    /// 关闭系统，安全地关闭电源。所有文件缓冲区已经刷新到磁盘上，所有正在运行的进程已经停止。
        　　    /// Windows要求：
        　　    /// Windows NT中：调用进程必须有SE_SHUTDOWN_NAME特权。
        　　    /// 1.EWX_FORCE
        　　    /// 强制终止进程。当此标志设置，Windows不会发送消息WM_QUERYENDSESSION和WM_ENDSESSION的消息给目前在系统中运行的程序。这可能会导致应用程序丢失数据。因此，你应该只在紧急情况下使用此标志。
        　　    /// 2.EWX_LOGOFF
        　　    /// 关闭所有进程，然后注销用户。
        　　    /// 3.EWX_POWEROFF
        　　    /// 关闭系统并关闭电源。该系统必须支持断电。
        　　    /// Windows要求：
        　　    /// Windows NT中调用进程必须有 SE_SHUTDOWN_NAME 特权。
        　　    /// Windows 9X中：可以直接调用。
        　　    /// 4.EWX_REBOOT
        　　    /// 关闭系统，然后重新启动系统。
        /// Windows要求：
        　　    /// Windows NT中：调用进程必须有SE_SHUTDOWN_NAME特权。
        　　    /// Windows 9X中：可以直接调用。
        　　    /// 5.EWX_SHUTDOWN
        　　    /// 关闭系统，安全地关闭电源。所有文件缓冲区已经刷新到磁盘上，所有正在运行的进程已经停止。
        　　    /// Windows要求：
        　　    /// Windows NT中：调用进程必须有SE_SHUTDOWN_NAME特权。
        　　    /// Windows 9X中：可以直接调用。Windows 9X中：可以直接调用。</param>
        /// <param name="dwReserved">系统保留,一般取0</param>
        /// <returns>如果函数成功，返回值为非零。如果函数失败，返回值是零。想获得更多错误信息，请调用GetLastError函数</returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int uFlags, int dwReserved);

        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        internal const int EWX_LOGOFF = 0x00000000;
        internal const int EWX_SHUTDOWN = 0x00000001;
        internal const int EWX_REBOOT = 0x00000002;
        internal const int EWX_FORCE = 0x00000004;
        internal const int EWX_POWEROFF = 0x00000008;
        internal const int EWX_FORCEIFHUNG = 0x00000010;

        #endregion

        private static void DoExitWin(int flg)
        {
            bool ok;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ok = ExitWindowsEx(flg, 0);
        }

        /// <summary>
        /// 计算机重启
        /// </summary>
        public static void Reboot()
        {
            DoExitWin(EWX_FORCE | EWX_REBOOT);
        }

        /// <summary>
        /// 计算机关电源
        /// </summary>
        public static void PowerOff()
        {
            DoExitWin(EWX_FORCE | EWX_POWEROFF);
        }

        /// <summary>
        /// 计算机注销
        /// </summary>
        public static void LogoOff()
        {
            DoExitWin(EWX_FORCE | EWX_LOGOFF);
        }

        #region 关闭显示器
        /// <summary>
        /// 关闭显示器
        /// </summary>
        public static void CloseMonitor()
        {
            // 2 为关闭显示器， －1则打开显示器
            SendMessage(HWND_HANDLE, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
        }

        private const uint WM_SYSCOMMAND = 0x0112;
        private const uint SC_MONITORPOWER = 0xF170;
        private static readonly IntPtr HWND_HANDLE = new IntPtr(0xffff);
        [DllImport("user32")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, uint wParam, int lParam);

        #endregion

        #region 杀进程

        /// <summary>
        /// 杀进程
        /// </summary>
        /// <param name="exe">应用程序名称</param>
        public static void KillProcess(string exe)
        {
            try
            {
                Process[] processes = Process.GetProcesses();//
                if (processes != null && processes.Length > 0)
                {
                    foreach (Process p in processes)
                    {
                        if (p.ProcessName == exe)
                        {
                            p.Kill();
                        }
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// 杀进程
        /// </summary>
        /// <param name="processId">应用程序进程ID</param>
        public static void KillProcess(int processId)
        {
            try
            {
                Process.GetProcessById(processId).Kill();
            }
            catch { }
        }
        #endregion

        #region 调用系统API切换应用程序

        #region 调用系统API（系统快捷键 Alt+Tab）切换应用程序
        /// <summary>
        /// 调用系统API（系统快捷键 Alt+Tab）切换应用程序
        /// </summary>
        /// <param name="process">程序进程</param>
        private static void SetForegroundWin(Process process)
        {
            try
            {
                ShowWindowAsync(process.MainWindowHandle, 1);  //调用api函数，正常显示窗口
                SetForegroundWindow(process.MainWindowHandle); //将窗口放置最前端
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
            }
        }
        /// <summary>
        /// 调用系统API（系统快捷键 Alt+Tab）切换应用程序，如果该程序没有启动，则自动启动
        /// </summary>
        /// <param name="process">应用程序名称</param>
        /// <param name="fileName">应用程序运行路径</param>
        private static void SetForegroundWin(Process process, string fileName)
        {
            try
            {
                ShowWindowAsync(process.MainWindowHandle, 1);  //调用api函数，正常显示窗口
                SetForegroundWindow(process.MainWindowHandle); //将窗口放置最前端
                if (!(Process.GetProcesses().Any(a => a.ProcessName == process.ProcessName)))
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
            }
        }
        /// <summary>
        /// 调用系统API（系统快捷键 Alt+Tab）切换应用程序
        /// </summary>
        /// <param name="processName">应用程序名称</param>
        private static void SetForegroundWin(string processName)
        {
            try
            {
                Process[] processes = Process.GetProcesses();//
                if (processes != null && processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        if (process.ProcessName == processName)
                        {
                            ShowWindowAsync(process.MainWindowHandle, 1);  //调用api函数，正常显示窗口
                            SetForegroundWindow(process.MainWindowHandle); //将窗口放置最前端
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
            }
        }
        /// <summary>
        /// 调用系统API（系统快捷键 Alt+Tab）切换应用程序，如果该程序没有启动，则自动启动
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="fileName">应用程序运行路径</param>
        private static void SetForegroundWin(string processName, string fileName)
        {
            try
            {
                bool exists = false;
                processName = processName.Replace(".exe", "");
                Process[] processes = Process.GetProcesses();//
                if (processes != null && processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        if (process.ProcessName == processName)
                        {
                            exists = true;
                            ShowWindowAsync(process.MainWindowHandle, 1);  //调用api函数，正常显示窗口
                            SetForegroundWindow(process.MainWindowHandle); //将窗口放置最前端
                        }
                    }
                }
                if (!exists)
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fAltTab"></param>
        [DllImport("User32.dll")]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        /// <summary>
        /// 调用系统API（系统快捷键 Alt+Tab）切换应用程序
        /// </summary>
        /// <param name="process">程序进程</param>
        public static void SwitchToThisWin(Process process)
        {
            try
            {
                SwitchToThisWindow(process.MainWindowHandle, true);
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
            }
        }
        /// <summary>
        /// 调用系统API（系统快捷键 Alt+Tab）切换应用程序，如果该程序没有启动，则自动启动
        /// </summary>
        /// <param name="processName">应用程序名称</param>
        /// <param name="fileName">应用程序运行路径</param>
        public static void SwitchToThisWin(Process processName, string fileName)
        {
            try
            {
                SwitchToThisWindow(processName.MainWindowHandle, true);
                if (!(Process.GetProcesses().Any(a => a.ProcessName == processName.ProcessName)))
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
            }
        }
        /// <summary>
        /// 调用系统API（系统快捷键 Alt+Tab）切换应用程序
        /// </summary>
        /// <param name="processName">应用程序名称</param>
        public static void SwitchToThisWin(string processName)
        {
            try
            {
                if (processName.ToLower().Contains(".exe"))
                {
                    processName = processName.Substring(processName.Length - ".exe".Length, ".exe".Length);
                }
                Process[] processes = Process.GetProcesses();//
                if (processes != null && processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        if (process.ProcessName == processName)
                        {
                            SwitchToThisWindow(process.MainWindowHandle, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
            }
        }
        /// <summary>
        /// 调用系统API（系统快捷键 Alt+Tab）切换应用程序，如果该程序没有启动，则自动启动
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="fileName">应用程序运行路径</param>
        public static void SwitchToThisWin(string processName, string fileName)
        {
            try
            {
                bool exists = false;
                if (processName.ToLower().Contains(".exe"))
                {
                    processName = processName.Substring(0, processName.Length - ".exe".Length);
                }
                Process[] processes = Process.GetProcessesByName(processName);//
                if (processes != null && processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        if (process.ProcessName == processName)
                        {
                            exists = true;
                            SwitchToThisWindow(process.MainWindowHandle, true);
                            return;
                        }
                    }
                }
                if (!exists)
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
            }
        }
        #endregion

        #region 内存回收
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="process"></param>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary> 
        /// 释放内存
        /// </summary> 
        public static void GCCollect()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
            }
            catch { }
        }
        #endregion


        /// <summary>
        /// 允许Cmd命令
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="createNoWindow">是否静默运行</param>
        /// <returns>System.String.</returns>
        public static string RunCmd(string command, bool createNoWindow = true)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";           //設定程序名
            p.StartInfo.Arguments = "/c " + command;    //設定程式執行參數
            p.StartInfo.UseShellExecute = false;        //關閉Shell的使用
            p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入
            p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出
            p.StartInfo.RedirectStandardError = true;   //重定向錯誤輸出
            p.StartInfo.CreateNoWindow = createNoWindow;          //設置不顯示窗口
            p.Start();   //啟動   
            return p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果
        }

        /// <summary>
        /// 注册DLL文件
        /// </summary>
        /// <param name="fileName">注册目标文件.</param>
        /// <param name="noInfomation">是否不用谈消息框</param>
        /// <param name="createNoWindow">設置不顯示窗口</param>
        /// <returns>System.String.</returns>
        public static string DllRegisterServer(string fileName, bool noInfomation, bool createNoWindow)
        {
            string command = "regsvr32 " + fileName + (noInfomation ? " /s" : "");
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";           //設定程序名
            p.StartInfo.Arguments = "/c " + command;    //設定程式執行參數
            p.StartInfo.UseShellExecute = false;        //關閉Shell的使用
            p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入
            p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出
            p.StartInfo.RedirectStandardError = true;   //重定向錯誤輸出
            p.StartInfo.CreateNoWindow = createNoWindow;          //設置不顯示窗口
            p.StartInfo.Verb = "RunAs";
            p.Start();   //啟動   
            return p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果
        }

        /// <summary>
        /// 注册DLL文件
        /// </summary>
        /// <param name="fileName">注册目标文件.</param>
        /// <returns>System.String.</returns>
        public static string DllRegisterServer(string fileName)
        {
            string command = "regsvr32 " + fileName;
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";           //設定程序名
            p.StartInfo.Arguments = "/s " + command;    //設定程式執行參數
            p.Start();   //啟動   
            return p.StandardOutput.ReadToEnd();        //從輸出流取得命令執行結果
        }


    }
}
