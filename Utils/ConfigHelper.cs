using System;
using System.Configuration;

namespace M.Core.Utils
{
    /// <summary>
    /// 读写配置文件
    /// </summary>
    public class ConfigHelper
    {
        #region 读写配置文件
        /// <summary>
        /// 修改配置文件中某项的值
        /// </summary>
        /// <param name="key">appSettings的key</param>
        /// <param name="value">appSettings的Value</param>
        public static void SetAppSettings(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!config.HasFile)
            {
                throw new ArgumentException("程序配置文件缺失！");
            }
            if (config.AppSettings.Settings[key] != null)
            {
                config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 读取配置文件某项的值
        /// </summary>
        /// <param name="key">appSettings的key</param>
        /// <returns>appSettings的Value</returns>
        public static string GetAppSettingsValue(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }
        #endregion
    }
}
