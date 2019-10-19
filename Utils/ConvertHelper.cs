using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace M.Core.Utils
{
    /// <summary>
    /// 处理数据类型转换，数制转换、编码转换相关的类
    /// </summary>
    public sealed class ConvertHelper
    {
        #region 各进制数间转换
        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            if (!isBaseNumber(from))
            {
                throw new ArgumentException("参数from只能是2,8,10,16四个值。");
            }

            if (!isBaseNumber(to))
            {
                throw new ArgumentException("参数to只能是2,8,10,16四个值。");
            }

            int intValue = Convert.ToInt32(value, from);  //先转成10进制
            string result = Convert.ToString(intValue, to);  //再转成目标进制
            if (to == 2)
            {
                int resultLength = result.Length;  //获取二进制的长度
                switch (resultLength)
                {
                    case 7:
                        result = "0" + result;
                        break;
                    case 6:
                        result = "00" + result;
                        break;
                    case 5:
                        result = "000" + result;
                        break;
                    case 4:
                        result = "0000" + result;
                        break;
                    case 3:
                        result = "00000" + result;
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断是否是  2 8 10 16
        /// </summary>
        /// <param name="baseNumber"></param>
        /// <returns></returns>
        private static bool isBaseNumber(int baseNumber)
        {
            if (baseNumber == 2 || baseNumber == 8 || baseNumber == 10 || baseNumber == 16)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 使用指定字符集将string转换成byte[]

        /// <summary>
        /// 将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        public static byte[] StringToBytes(string text)
        {
            return Encoding.Default.GetBytes(text);
        }

        /// <summary>
        /// 使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        #endregion

        #region 使用指定字符集将byte[]转换成string

        /// <summary>
        /// 将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        public static string BytesToString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }
        #endregion

        #region 将byte[]转换成int
        /// <summary>
        /// 将byte[]转换成int
        /// </summary>
        /// <param name="data">需要转换成整数的byte数组</param>
        public static int BytesToInt32(byte[] data)
        {
            //如果传入的字节数组长度小于4,则返回0
            if (data.Length < 4)
            {
                return 0;
            }

            //定义要返回的整数
            int num = 0;

            //如果传入的字节数组长度大于4,需要进行处理
            if (data.Length >= 4)
            {
                //创建一个临时缓冲区
                byte[] tempBuffer = new byte[4];

                //将传入的字节数组的前4个字节复制到临时缓冲区
                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                //将临时缓冲区的值转换成整数，并赋给num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //返回整数
            return num;
        }
        #endregion

        #region 将数据转换为整型

        /// <summary>
        /// 将数据转换为整型   转换失败返回默认值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static int ToInt32<T>(T data, int defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToInt32(data);
            }
            catch
            {
                return defValue;
            }

        }

        /// <summary>
        /// 将数据转换为整型   转换失败返回默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static int ToInt32(string data, int defValue)
        {
            //如果为空则返回默认值
            if (string.IsNullOrEmpty(data))
            {
                return defValue;
            }

            int temp = 0;
            if (Int32.TryParse(data, out temp))
            {
                return temp;
            }
            else
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为整型  转换失败返回默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static int ToInt32(object data, int defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToInt32(data);
            }
            catch
            {
                return defValue;
            }
        }

        #endregion

        #region 将数据转换为布尔型

        /// <summary>
        /// 将数据转换为布尔类型  转换失败返回默认值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static bool ToBoolean<T>(T data, bool defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToBoolean(data);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为布尔类型  转换失败返回 默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static bool ToBoolean(string data, bool defValue)
        {
            //如果为空则返回默认值
            if (string.IsNullOrEmpty(data))
            {
                return defValue;
            }

            bool temp = false;
            if (bool.TryParse(data, out temp))
            {
                return temp;
            }
            else
            {
                return defValue;
            }
        }


        /// <summary>
        /// 将数据转换为布尔类型  转换失败返回 默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static bool ToBoolean(object data, bool defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToBoolean(data);
            }
            catch
            {
                return defValue;
            }
        }


        #endregion

        #region 将数据转换为单精度浮点型


        /// <summary>
        /// 将数据转换为单精度浮点型  转换失败 返回默认值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static float ToFloat<T>(T data, float defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToSingle(data);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为单精度浮点型   转换失败返回默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static float ToFloat(object data, float defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToSingle(data);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为单精度浮点型   转换失败返回默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static float ToFloat(string data, float defValue)
        {
            //如果为空则返回默认值
            if (string.IsNullOrEmpty(data))
            {
                return defValue;
            }

            float temp = 0;

            if (float.TryParse(data, out temp))
            {
                return temp;
            }
            else
            {
                return defValue;
            }
        }


        #endregion

        #region 将数据转换为双精度浮点型


        /// <summary>
        /// 将数据转换为双精度浮点型   转换失败返回默认值
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static double ToDouble<T>(T data, double defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDouble(data);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为双精度浮点型,并设置小数位   转换失败返回默认值
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static double ToDouble<T>(T data, int decimals, double defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Math.Round(Convert.ToDouble(data), decimals);
            }
            catch
            {
                return defValue;
            }
        }



        /// <summary>
        /// 将数据转换为双精度浮点型  转换失败返回默认值
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static double ToDouble(object data, double defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDouble(data);
            }
            catch
            {
                return defValue;
            }

        }

        /// <summary>
        /// 将数据转换为双精度浮点型  转换失败返回默认值
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static double ToDouble(string data, double defValue)
        {
            //如果为空则返回默认值
            if (string.IsNullOrEmpty(data))
            {
                return defValue;
            }

            double temp = 0;

            if (double.TryParse(data, out temp))
            {
                return temp;
            }
            else
            {
                return defValue;
            }

        }


        /// <summary>
        /// 将数据转换为双精度浮点型,并设置小数位  转换失败返回默认值
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static double ToDouble(object data, int decimals, double defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Math.Round(Convert.ToDouble(data), decimals);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为双精度浮点型,并设置小数位  转换失败返回默认值
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <param name="decimals">小数的位数</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static double ToDouble(string data, int decimals, double defValue)
        {
            //如果为空则返回默认值
            if (string.IsNullOrEmpty(data))
            {
                return defValue;
            }

            double temp = 0;

            if (double.TryParse(data, out temp))
            {
                return Math.Round(temp, decimals);
            }
            else
            {
                return defValue;
            }
        }


        #endregion

        #region 将数据转换为指定类型
        /// <summary>
        /// 将数据转换为指定类型
        /// </summary>
        /// <param name="data">转换的数据</param>
        /// <param name="targetType">转换的目标类型</param>
        public static object ConvertTo(object data, Type targetType)
        {
            if (data == null || Convert.IsDBNull(data))
            {
                return null;
            }

            Type type2 = data.GetType();
            if (targetType == type2)
            {
                return data;
            }
            if (((targetType == typeof(Guid)) || (targetType == typeof(Guid?))) && (type2 == typeof(string)))
            {
                if (string.IsNullOrEmpty(data.ToString()))
                {
                    return null;
                }
                return new Guid(data.ToString());
            }

            if (targetType.IsEnum)
            {
                try
                {
                    return Enum.Parse(targetType, data.ToString(), true);
                }
                catch
                {
                    return Enum.ToObject(targetType, data);
                }
            }

            if (targetType.IsGenericType)
            {
                targetType = targetType.GetGenericArguments()[0];
            }

            return Convert.ChangeType(data, targetType);
        }

        /// <summary>
        /// 将数据转换为指定类型
        /// </summary>
        /// <typeparam name="T">转换的目标类型</typeparam>
        /// <param name="data">转换的数据</param>
        public static T ConvertTo<T>(object data)
        {
            if (data == null || Convert.IsDBNull(data))
            {
                return default(T);
            }

            object obj = ConvertTo(data, typeof(T));
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }
        #endregion

        #region 将数据转换Decimal

        /// <summary>
        /// 将数据转换为Decimal  转换失败返回默认值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Decimal ToDecimal<T>(T data, Decimal defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDecimal(data);
            }
            catch
            {
                return defValue;
            }
        }


        /// <summary>
        /// 将数据转换为Decimal  转换失败返回 默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Decimal ToDecimal(object data, Decimal defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDecimal(data);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为Decimal  转换失败返回 默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Decimal ToDecimal(string data, Decimal defValue)
        {
            //如果为空则返回默认值
            if (string.IsNullOrEmpty(data))
            {
                return defValue;
            }

            decimal temp = 0;

            if (decimal.TryParse(data, out temp))
            {
                return temp;
            }
            else
            {
                return defValue;
            }
        }


        #endregion

        #region 将数据转换为DateTime

        /// <summary>
        /// 将数据转换为DateTime  转换失败返回默认值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime<T>(T data, DateTime defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDateTime(data);
            }
            catch
            {
                return defValue;
            }
        }


        /// <summary>
        /// 将数据转换为DateTime  转换失败返回 默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object data, DateTime defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDateTime(data);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将数据转换为DateTime  转换失败返回 默认值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(string data, DateTime defValue)
        {
            //如果为空则返回默认值
            if (string.IsNullOrEmpty(data))
            {
                return defValue;
            }

            DateTime temp = DateTime.Now;

            if (DateTime.TryParse(data, out temp))
            {
                return temp;
            }
            else
            {
                return defValue;
            }
        }

        #endregion

        #region 半角全角转换
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ConvertToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }


        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ConvertToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }
        #endregion

        #region 实体类转换成DataTable
        /// <summary>
        /// 实体类转换成DataTable
        /// 调用示例：DataTable dt= ConvertEntitiesToDataTable(Entitylist.ToList());
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public static DataTable ConvertEntitiesToDataTable<T>(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = ConvertEntityToDataTable(modelList[0]);//创建表结构
            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// 将实体类转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static DataRow ConvertEntitiesToDataTable<T>(T model)
        {
            if (model == null)
            {
                return null;
            }
            DataTable dt = ConvertEntityToDataTable(model);//创建表结构
            DataRow dataRow = dt.NewRow();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
            }
            return dataRow;
        }

        /// <summary>
        /// 将实体类转换成DataTable
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        static DataTable ConvertEntityToDataTable<T>(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (propertyInfo.Name != "CTimestamp")//些字段为oracle中的Timesstarmp类型
                {
                    dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
                }
                else
                {
                    dataTable.Columns.Add(new DataColumn(propertyInfo.Name, typeof(DateTime)));
                }
            }
            return dataTable;
        }
        #endregion

        #region DataTable转换成实体类
        /// <summary>
        /// 将DataRow转换成实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static T ConvertDataRowToEntity<T>(DataRow dataRow) where T : new()
        {
            T entity = new T();
            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in entity.GetType().GetProperties())
            {
                //检查DataTable是否包含此列（列名==对象的属性名）     
                if (dataRow.Table.Columns.Contains(pi.Name))
                {
                    // 判断此属性是否有Setter   
                    if (!pi.CanWrite)
                    {
                        continue;//该属性不可写，直接跳出   
                    }
                    //取值   
                    object value = ConvertToPropertyType(pi.PropertyType.Name, dataRow[pi.Name]);
                    //object value = dataRow[pi.Name];
                    //如果非空，则赋给对象的属性   
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(entity, value, null);
                    }
                }
            }
            return entity;
        }
        /// <summary>
        /// 将DataRow转换成实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ConvertDataRowToEntity<T>(DataTable dt) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return new T();
            }
            T entity = new T();
            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in entity.GetType().GetProperties())
            {
                //检查DataTable是否包含此列（列名==对象的属性名）     
                if (dt.Columns.Contains(pi.Name))
                {
                    // 判断此属性是否有Setter   
                    if (!pi.CanWrite)
                    {
                        continue;//该属性不可写，直接跳出   
                    }
                    //取值   
                    object value = ConvertToPropertyType(pi.PropertyType.Name, dt.Rows[0][pi.Name]);
                    //object value = dataRow[pi.Name];
                    //如果非空，则赋给对象的属性   
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(entity, value, null);
                    }
                }
            }
            return entity;
        }
        /// <summary>
        /// 将DataRow转换成实体类（实体类字段都必须为string类型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        [Obsolete("请使用ConvertDataRowToEntity", true)]
        public static T ConvertDataRowToStrEntity<T>(DataRow dataRow) where T : new()
        {
            T entity = new T();
            //遍历该对象的所有属性 
            foreach (PropertyInfo pi in entity.GetType().GetProperties())
            {
                //检查DataTable是否包含此列（列名==对象的属性名）     
                if (dataRow.Table.Columns.Contains(pi.Name))
                {
                    // 判断此属性是否有Setter   
                    if (!pi.CanWrite)
                    {
                        continue;//该属性不可写，直接跳出   
                    }
                    //取值   
                    object value = dataRow[pi.Name];
                    //如果非空，则赋给对象的属性   
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(entity, (value ?? "").ToString(), null);
                    }
                }
            }
            return entity;
        }
        /// <summary>
        /// 将DataTable转换成实体类集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTableToEntities<T>(DataTable dataTable) where T : new()
        {
            if (dataTable == null)
            {
                return null;
            }
            if (dataTable.Rows.Count == 0)
            {
                return new List<T>();
            }

            List<T> entities = new List<T>();
            foreach (DataRow dr in dataTable.Rows)
            {
                T entity = new T();
                foreach (PropertyInfo pi in entity.GetType().GetProperties())
                {
                    //检查DataTable是否包含此列（列名==对象的属性名）     
                    if (dataTable.Columns.Contains(pi.Name))
                    {
                        // 判断此属性是否有Setter   
                        if (!pi.CanWrite)
                        {
                            continue;//该属性不可写，直接跳出   
                        }
                        //取值   
                        object value = ConvertToPropertyType(pi.PropertyType.Name, dr[pi.Name]);
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(entity, value, null);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }

        /// <summary>
        /// 数据库字段类型对应C#数据类型
        /// </summary>
        /// <param name="propertyTypeName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ConvertToPropertyType(string propertyTypeName, object value)
        {
            switch (propertyTypeName.ToUpper())
            {
                case "LONG":
                    return value is DBNull ? 0 : Convert.ToInt64(value);
                case "SHORT":
                    return value is DBNull ? 0 : Convert.ToInt16(value);
                case "INT":
                    return value is DBNull ? 0 : Convert.ToInt32(value);
                case "INT16":
                    return value is DBNull ? 0 : Convert.ToInt16(value);
                case "INT32":
                    return value is DBNull ? 0 : Convert.ToInt32(value);
                case "INT64":
                    return value is DBNull ? 0 : Convert.ToInt64(value);
                case "STRING":
                    return Convert.ToString(value);
                case "SINGLE":
                    return Convert.ToSingle(value);
                case "BYTE":
                    return Convert.ToByte(value);
                case "CHAR":
                    return Convert.ToChar(value);
                case "DATETIME":
                    DateTime t;
                    if (DateTime.TryParse(value.ToString(), out t))
                    {
                        return Convert.ToDateTime(value);
                    }
                    return null;
                case "DECIMAL":
                    return value is DBNull ? 0 : Convert.ToDecimal(value);
                case "FLOAT":
                    return value is DBNull ? 0 : Convert.ToSingle(value);
                case "DOUBLE":
                    return value is DBNull ? 0 : Convert.ToDouble(value);
                case "BOOLEAN":
                    return value is DBNull ? false : Convert.ToBoolean(value);
                case "BOOL":
                    return value is DBNull ? false : Convert.ToBoolean(value);
                case "GUID":
                    return Guid.Parse(value.ToString());
            }
            return value;
        }
        /// <summary>
        /// 将DataTable转换成实体类集合（实体类字段都必须为string类型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        [Obsolete("请使用ConvertDataTableToEntities", true)]
        public static List<T> ConvertDataTableToStrEntities<T>(DataTable dataTable) where T : new()
        {
            if (dataTable == null)
            {
                return null;
            }

            if (dataTable.Rows.Count == 0)
            {
                return new List<T>();
            }
            List<T> entities = new List<T>();
            foreach (DataRow dr in dataTable.Rows)
            {
                T entity = new T();
                foreach (PropertyInfo pi in entity.GetType().GetProperties())
                {
                    //检查DataTable是否包含此列（列名==对象的属性名）     
                    if (dataTable.Columns.Contains(pi.Name))
                    {
                        // 判断此属性是否有Setter   
                        if (!pi.CanWrite)
                        {
                            continue;//该属性不可写，直接跳出   
                        }
                        //取值   
                        object value = dr[pi.Name].ToString();
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(entity, value.ToString(), null);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }
        /// <summary>
        /// 将DataTable转换成实体类集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSet"></param>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTableToEntities<T>(DataSet dataSet, int tableIndex) where T : new()
        {
            if (dataSet == null || dataSet.Tables.Count == 0)
            {
                return null;
            }
            if (dataSet.Tables[tableIndex] == null || dataSet.Tables[tableIndex].Rows.Count == 0)
            {
                return new List<T>();
            }
            List<T> entities = new List<T>();
            foreach (DataRow dr in dataSet.Tables[tableIndex].Rows)
            {
                T entity = new T();
                foreach (PropertyInfo pi in entity.GetType().GetProperties())
                {
                    //检查DataTable是否包含此列（列名==对象的属性名）     
                    if (dataSet.Tables[tableIndex].Columns.Contains(pi.Name))
                    {
                        // 判断此属性是否有Setter   
                        if (!pi.CanWrite)
                        {
                            continue;//该属性不可写，直接跳出   
                        }
                        //取值   
                        object value = dr[pi.Name];
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(entity, value, null);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }
        #endregion

        #region byte[]转成Base64、将Base64编码文本转换成Byte[]
        /// <summary>
        /// byte[]转成Base64
        /// </summary>
        /// <param name="binBuffer"></param>
        /// <returns></returns>
        public static string BytesToBase64(byte[] binBuffer)
        {
            int base64ArraySize = (int)Math.Ceiling(binBuffer.Length / 3d) * 4;
            char[] charBuffer = new char[base64ArraySize];
            Convert.ToBase64CharArray(binBuffer, 0, binBuffer.Length, charBuffer, 0);
            string s = new string(charBuffer);
            return s;
        }
        /// <summary>
        /// 将Base64编码文本转换成Byte[]
        /// </summary>
        /// <param name="base64">Base64编码文本</param>
        /// <returns></returns>
        public static Byte[] Base64ToBytes(string base64)
        {
            char[] charBuffer = base64.ToCharArray();
            byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
            return bytes;
        }
        #endregion
    }
}

}
