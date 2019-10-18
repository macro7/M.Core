using System;
using System.Diagnostics;

namespace M.Core.Utils
{
    class FreezeWindowUtils
    {

        private uint processId;

        public FreezeWindowUtils(IntPtr windowHandle)
        {
            NativeMethods.GetWindowThreadProcessId(windowHandle, out processId);
            FreezeThreads((int)processId);
        }

        public void Dispose()
        {
            UnfreezeThreads((int)processId);
        }

        /// <summary>
        /// 对进程的窗体进行冻结
        /// </summary>
        /// <param name="intPID"></param>
        public static void FreezeThreads(int intPID)
        {
            if (intPID != 0 && Process.GetCurrentProcess().Id != intPID)
            {
                Process pProc = Process.GetProcessById(intPID);
                if (!string.IsNullOrEmpty(pProc.ProcessName) && pProc.ProcessName != "explorer")
                {
                    foreach (ProcessThread pT in pProc.Threads)
                    {
                        IntPtr ptrOpenThread = NativeMethods.OpenThread(NativeMethods.ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);
                        if (ptrOpenThread != null)
                        {
                            NativeMethods.SuspendThread(ptrOpenThread);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 解冻进程的窗体
        /// </summary>
        /// <param name="intPID"></param>
        public static void UnfreezeThreads(int intPID)
        {
            if (intPID != 0 && Process.GetCurrentProcess().Id != intPID)
            {
                Process pProc = Process.GetProcessById(intPID);
                if (!string.IsNullOrEmpty(pProc.ProcessName))
                {
                    foreach (ProcessThread pT in pProc.Threads)
                    {
                        IntPtr ptrOpenThread = NativeMethods.OpenThread(NativeMethods.ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);
                        if (ptrOpenThread != null)
                        {
                            NativeMethods.ResumeThread(ptrOpenThread);
                        }
                    }
                }
            }
        }
    }
}
