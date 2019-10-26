using System;
using System.IO;
using System.Net;
using System.Text;

namespace M.Core.Utils
{
    public class HttWebRequestpHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public string Post(string url, string args)
        {
            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                #region 添加Post 参数  
                byte[] data = Encoding.UTF8.GetBytes(args);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                #endregion

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容  
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }
            return result;
        }
    }
}
