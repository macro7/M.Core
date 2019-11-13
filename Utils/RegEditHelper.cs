using Microsoft.Win32;
using System;

namespace M.Core.Utils
{
    /// <summary>
    /// 读写注册表帮助类
    /// </summary>
    public class RegEditHelper
    {
        /// <summary>
        /// 从注册表中得到一些值
        /// </summary>
        /// <param name="section">子目录</param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">假如无，则默认值</param>
        /// <returns></returns>
        public static string ReadRegVal(string section, string key, string defaultValue = "")
        {
            try
            {
                RegistryKey reg = Registry.LocalMachine;
                //在HKEY_LOCAL_MACHINE\SOFTWARE下新建名为section的注册表项。如果已经存在则不影响！  
                RegistryKey software = reg.CreateSubKey("software\\" + section.Trim());
                defaultValue = software.GetValue(key, "0").ToString();
                reg.Close();
                return defaultValue;
            }
            catch (Exception e)
            {
                Logger.WriteErrorLog(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// 把特定值存入注册表中;该注册键值存在本机LocalMachine\software目录下
        /// </summary>
        /// <param name="section">子目录</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void WriteRegVal(string section, string key, string value)
        {
            RegistryKey reg = Registry.LocalMachine;
            //在HKEY_LOCAL_MACHINE\SOFTWARE下新建名为section的注册表项。如果已经存在则不影响！  
            RegistryKey software = reg.CreateSubKey("software\\" + section.Trim());
            software.SetValue(key, value);
            // 注意：SetValue()还有第三个参数，主要是用于设置键值的类型，如：字符串，二进制，Dword等等~~默认是字符串。如：  
            // software.SetValue("test", "0", RegistryValueKind.DWord); //二进制信息  
            reg.Close();
        }
    }
}
