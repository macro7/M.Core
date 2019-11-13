using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
namespace M.Core.Utils
{
    /// <summary>
    /// ini文件处理帮助类
    /// </summary>
    public class IniHelper
    {
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
        /// <summary>  
        /// 将指定的键值对写到指定的节点，如果已经存在则替换。  
        /// </summary>  
        /// <param name="lpAppName">节点，如果不存在此节点，则创建此节点</param>  
        /// <param name="lpReturnedString">Item键值对，多个用\0分隔,形如key1=value1\0key2=value2  
        /// <para>如果为string.Empty，则删除指定节点下的所有内容，保留节点</para>  
        /// <para>如果为null，则删除指定节点下的所有内容，并且删除该节点</para>  
        /// </param>  
        /// <param name="nSize"></param>  
        /// <param name="lpFileName"></param>  
        /// <returns>是否成功写入</returns>  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

        private string sPath = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">ini文件路径</param>
        public IniHelper(string path)
        {
            sPath = path;
        }

        /// <summary>
        /// 写节点内容
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void IniWriteValue(string section, string key, string value)
        {
            // section=配置节，key=键名，value=键值，path=路径
            WritePrivateProfileString(section, key, value, sPath);
        }

        /// <summary>
        /// 读取节点内容
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string IniReadValue(string section, string key)
        {
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            // section=配置节，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, 255, sPath);
            return temp.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] IniReadValues(string section, string key)
        {
            byte[] temp = new byte[255];
            int i = GetPrivateProfileString(section, key, "", temp, 255, sPath);
            return temp;
        }
        /// <summary>
        /// 删除ini文件下所有段落
        /// </summary>
        public void IniClearAllSection()
        {
            IniWriteValue(null, null, null);
        }

        /// <summary>
        /// 删除ini文件下指定段落下的所有键
        /// </summary>
        /// <param name="Section"></param>
        public void IniDeleteSection(string Section)
        {
            IniWriteValue(Section, null, null);
        }
        /// <summary>  
        /// 根据section取所有key  
        /// </summary>  
        /// <param name="section"></param>  
        /// <param name="filePath"></param>  
        /// <returns></returns>  
        public static string[] IniReadIniAllKeys(string section, string filePath)
        {
            UInt32 MAX_BUFFER = 32767;
            string[] items = new string[0];
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));
            UInt32 bytesReturned = GetPrivateProfileSection(section, pReturnedString, MAX_BUFFER, filePath);
            if (!(bytesReturned == MAX_BUFFER - 2) || (bytesReturned == 0))
            {
                string returnedString = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned);
                items = returnedString.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            Marshal.FreeCoTaskMem(pReturnedString);
            return items;
        }
        /// <summary>  
        /// 根据section，key取值  
        /// </summary>  
        /// <param name="section"></param>  
        /// <param name="keys"></param>  
        /// <param name="filePath">ini文件路径</param>  
        /// <returns></returns>  
        public static string IniReadIniKeys(string section, string keys, string filePath)
        {
            return ReadString(section, keys, "", filePath);
        }
        private static string ReadString(string section, string key, string def, string filePath)
        {
            StringBuilder temp = new StringBuilder(1024);
            try
            {
                GetPrivateProfileString(section, key, def, temp, 1024, filePath);
            }
            catch { }
            return temp.ToString();
        }
        /// <summary>  
        /// 保存ini  
        /// </summary>  
        /// <param name="section"></param>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        /// <param name="filePath">ini文件路径</param>  
        public static void IniWriteKeyValue(string section, string key, string value, string filePath)
        {
            WritePrivateProfileString(section, key, value, filePath);
        }
        /// <summary>  
        /// 在INI文件中，删除指定节点中的指定的键。  
        /// </summary>  
        /// <param name="filePath">INI文件</param>  
        /// <param name="section">节点</param>  
        /// <param name="key">键</param>  
        /// <returns>操作是否成功</returns>  
        public static bool IniDeleteKey(string filePath, string section, string key)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("必须指定键名称", "key");
            }
            WritePrivateProfileString(section, key, null, filePath);
            return true;
        }
        /// <summary>  
        /// 在INI文件中，删除指定的节点。  
        /// </summary>  
        /// <param name="filePath">INI文件</param>  
        /// <param name="section">节点</param>  
        /// <returns>操作是否成功</returns>  
        public static bool IniDeleteSection(string filePath, string section)
        {
            if (string.IsNullOrEmpty(section))
            {
                throw new ArgumentException("必须指定节点名称", "section");
            }
            WritePrivateProfileString(section, null, null, filePath);
            return true;
        }

        /// <summary>
        /// 清除节点内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static bool IniClearSection(string filePath, string section)
        {
            WritePrivateProfileString(null, null, null, filePath);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpAppName"></param>
        /// <param name="lpReturnedString"></param>
        /// <param name="nSize"></param>
        /// <param name="lpFileName"></param>
        /// <returns></returns>
        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileSection")]
        protected internal static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string lpFileName);
        /// <summary>
        /// 获取节点下所有属性值
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static List<String> GetSectionValues(String filename, String section)
        {
            List<String> items = new List<String>();
            byte[] buffer = new byte[32768];
            int bufLen = 0;
            bufLen = GetPrivateProfileSection(section, buffer, buffer.GetUpperBound(0), filename);
            if (bufLen > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bufLen; i++)
                {
                    if (buffer[i] != 0)
                    {
                        sb.Append((char)buffer[i]);
                    }
                    else
                    {
                        if (sb.Length > 0)
                        {
                            items.Add(sb.ToString());
                            sb = new StringBuilder();
                        }
                    }
                }
            }
            return items;
        }
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public List<string> GetSectionValues(String section)
        {
            List<String> items = new List<String>();
            byte[] buffer = new byte[32768];
            int bufLen = 0;
            bufLen = GetPrivateProfileSection(section, buffer, buffer.GetUpperBound(0), sPath);
            if (bufLen > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bufLen; i++)
                {
                    if (buffer[i] != 0)
                    {
                        sb.Append((char)buffer[i]);
                    }
                    else
                    {
                        if (sb.Length > 0)
                        {
                            items.Add(sb.ToString());
                            sb = new StringBuilder();
                        }
                    }
                }
            }
            return items;
        }


        /// <summary>  
        /// 获取所有节点名称(Section)  
        /// </summary>  
        /// <param name="lpszReturnBuffer">存放节点名称的内存地址,每个节点之间用\0分隔</param>  
        /// <param name="nSize">内存大小(characters)</param>  
        /// <param name="lpFileName">Ini文件</param>  
        /// <returns>内容的实际长度,为0表示没有内容,为nSize-2表示内存大小不够</returns>  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, uint nSize, string lpFileName);

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <param name="fileNName"></param>
        /// <returns></returns>
        public static string[] GetSectionNames(string fileNName)
        {
            uint MAX_BUFFER = 32767;    //默认为32767  
            string[] sections = new string[0];      //返回值  
                                                    //申请内存  
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, fileNName);
            if (bytesReturned != 0)
            {
                //读取指定内存的内容  
                string local = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned).ToString();
                //每个节点之间用\0分隔,末尾有一个\0  
                sections = local.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            //释放内存  
            Marshal.FreeCoTaskMem(pReturnedString);
            return sections;
        }
        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        public string[] GetSectionNames()
        {
            uint MAX_BUFFER = 32767;    //默认为32767  
            string[] sections = new string[0];      //返回值  
                                                    //申请内存  
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, sPath);
            if (bytesReturned != 0)
            {
                //读取指定内存的内容  
                string local = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned).ToString();
                //每个节点之间用\0分隔,末尾有一个\0  
                sections = local.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            //释放内存  
            Marshal.FreeCoTaskMem(pReturnedString);
            return sections;
        }
    }
}
