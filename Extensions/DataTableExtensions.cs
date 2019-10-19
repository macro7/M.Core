using System;
using System.Data;

namespace M.Core.Extensions
{
    /// <summary>
    /// DataTable 对象扩张方法
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 相当于select distinct fieldName from sourceTable 
        /// </summary> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldName">列名,用逗号隔开 例："column1,column2"</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldName指明的列</returns> 
        public static DataTable SelectDistinct(this DataTable sourceTable, string fieldName)
        {
            string tableName = string.IsNullOrEmpty(sourceTable.TableName) ? "tempTableName" : sourceTable.TableName;
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(fieldName, sourceTable.Columns[fieldName].DataType);

            object lastValue = null;
            foreach (DataRow dr in sourceTable.Select("", fieldName))
            {
                if (lastValue == null || !(ColumnEqual(lastValue, dr[fieldName])))
                {
                    lastValue = dr[fieldName];
                    dt.Rows.Add(new object[] { lastValue });
                }
            }
            return dt;
        }
        /// <summary>
        /// 列比较
        /// </summary>
        /// <param name="objectA"></param>
        /// <param name="objectB"></param>
        /// <returns></returns>
        private static bool ColumnEqual(object objectA, object objectB)
        {
            if (objectA == DBNull.Value && objectB == DBNull.Value)
            {
                return true;
            }
            if (objectA == DBNull.Value || objectB == DBNull.Value)
            {
                return false;
            }
            return (objectA.Equals(objectB));
        }
    }
}
