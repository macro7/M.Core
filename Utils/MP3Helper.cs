using System;
using System.IO;
using System.Runtime.InteropServices;

namespace M.Core.Utils
{
    /// <summary>
    /// MP3播放器
    /// </summary>
    public static class MP3Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCommand"></param>
        /// <param name="strReturn"></param>
        /// <param name="iReturnLength"></param>
        /// <param name="hwndCallback"></param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, System.Text.StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="MP3_FileName"></param>
        /// <param name="Repeat"></param>
        public static void Play(string MP3_FileName, bool Repeat)
        {
            mciSendString("open \"" + MP3_FileName + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            mciSendString("play MediaFile" + (Repeat ? " repeat" : String.Empty), null, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="MP3_EmbeddedResource"></param>
        /// <param name="Repeat"></param>
        public static void Play(byte[] MP3_EmbeddedResource, bool Repeat)
        {
            extractResource(MP3_EmbeddedResource, Path.GetTempPath() + "resource.tmp");
            mciSendString("open \"" + Path.GetTempPath() + "resource.tmp" + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
            mciSendString("play MediaFile" + (Repeat ? " repeat" : String.Empty), null, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public static void Pause()
        {
            mciSendString("stop MediaFile", null, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            mciSendString("close MediaFile", null, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="filePath"></param>
        private static void extractResource(byte[] res, string filePath)
        {

            if (!File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        foreach (byte b in res)
                        {
                            bw.Write(b);
                        }
                        bw.Close();
                    }
                    fs.Close();
                }
            }
        }
    }
}
