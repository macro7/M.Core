using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace M.Core.Utils
{
    /// <summary> 
    /// DataSet助手 
    /// </summary> 
    public class DataSetHelper
    {
        private class FieldInfo
        {
            public string RelationName;
            public string FieldName;
            public string FieldAlias;
            public string Aggregate;
        }

        private DataSet ds;
        private ArrayList m_FieldInfo;
        private string m_FieldList;
        private ArrayList GroupByFieldInfo;
        private string GroupByFieldList;
        public DataSet DataSet
        {
            get { return ds; }
        }

        #region Construction

        public DataSetHelper()
        {
            ds = null;
        }

        public DataSetHelper(ref DataSet dataSet)
        {
            ds = dataSet;
        }

        #endregion

        #region Private Methods

        private bool ColumnEqual(object objectA, object objectB)
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

        private bool RowEqual(DataRow rowA, DataRow rowB, DataColumnCollection columns)
        {
            bool result = true;
            for (int i = 0; i < columns.Count; i++)
            {
                result &= ColumnEqual(rowA[columns[i].ColumnName], rowB[columns[i].ColumnName]);
            }
            return result;
        }

        private void ParseFieldList(string fieldList, bool allowRelation)
        {
            if (m_FieldList == fieldList)
            {
                return;
            }
            m_FieldInfo = new ArrayList();
            m_FieldList = fieldList;
            FieldInfo Field;
            string[] FieldParts;
            string[] Fields = fieldList.Split(',');
            for (int i = 0; i <= Fields.Length - 1; i++)
            {
                Field = new FieldInfo();
                FieldParts = Fields[i].Trim().Split(' ');
                switch (FieldParts.Length)
                {
                    case 1:
                        //to be set at the end of the loop 
                        break;
                    case 2:
                        Field.FieldAlias = FieldParts[1];
                        break;
                    default:
                        return;
                }
                FieldParts = FieldParts[0].Split('.');
                switch (FieldParts.Length)
                {
                    case 1:
                        Field.FieldName = FieldParts[0];
                        break;
                    case 2:
                        if (allowRelation == false)
                        {
                            return;
                        }
                        Field.RelationName = FieldParts[0].Trim();
                        Field.FieldName = FieldParts[1].Trim();
                        break;
                    default:
                        return;
                }
                if (Field.FieldAlias == null)
                {
                    Field.FieldAlias = Field.FieldName;
                }
                m_FieldInfo.Add(Field);
            }
        }

        private DataTable CreateTable(string tableName, DataTable sourceTable, string fieldList)
        {
            DataTable dt;
            if (fieldList.Trim() == "")
            {
                dt = sourceTable.Clone();
                dt.TableName = tableName;
            }
            else
            {
                dt = new DataTable(tableName);
                ParseFieldList(fieldList, false);
                DataColumn dc;
                foreach (FieldInfo Field in m_FieldInfo)
                {
                    dc = sourceTable.Columns[Field.FieldName];
                    DataColumn column = new DataColumn();
                    column.ColumnName = Field.FieldAlias;
                    column.DataType = dc.DataType;
                    column.MaxLength = dc.MaxLength;
                    column.Expression = dc.Expression;
                    dt.Columns.Add(column);
                }
            }
            if (ds != null)
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        private void InsertInto(DataTable destTable, DataTable sourceTable,
                                string fieldList, string rowFilter, string sort)
        {
            ParseFieldList(fieldList, false);
            DataRow[] rows = sourceTable.Select(rowFilter, sort);
            DataRow destRow;
            foreach (DataRow sourceRow in rows)
            {
                destRow = destTable.NewRow();
                if (fieldList == "")
                {
                    foreach (DataColumn dc in destRow.Table.Columns)
                    {
                        if (dc.Expression == "")
                        {
                            destRow[dc] = sourceRow[dc.ColumnName];
                        }
                    }
                }
                else
                {
                    foreach (FieldInfo field in m_FieldInfo)
                    {
                        destRow[field.FieldAlias] = sourceRow[field.FieldName];
                    }
                }
                destTable.Rows.Add(destRow);
            }
        }

        private void ParseGroupByFieldList(string FieldList)
        {
            if (GroupByFieldList == FieldList)
            {
                return;
            }
            GroupByFieldInfo = new ArrayList();
            FieldInfo Field;
            string[] FieldParts;
            string[] Fields = FieldList.Split(',');
            for (int i = 0; i <= Fields.Length - 1; i++)
            {
                Field = new FieldInfo();
                FieldParts = Fields[i].Trim().Split(' ');
                switch (FieldParts.Length)
                {
                    case 1:
                        //to be set at the end of the loop 
                        break;
                    case 2:
                        Field.FieldAlias = FieldParts[1];
                        break;
                    default:
                        return;
                }

                FieldParts = FieldParts[0].Split('(');
                switch (FieldParts.Length)
                {
                    case 1:
                        Field.FieldName = FieldParts[0];
                        break;
                    case 2:
                        Field.Aggregate = FieldParts[0].Trim().ToLower();
                        Field.FieldName = FieldParts[1].Trim(' ', ')');
                        break;
                    default:
                        return;
                }
                if (Field.FieldAlias == null)
                {
                    if (Field.Aggregate == null)
                    {
                        Field.FieldAlias = Field.FieldName;
                    }
                    else
                    {
                        Field.FieldAlias = Field.Aggregate + "of" + Field.FieldName;
                    }
                }
                GroupByFieldInfo.Add(Field);
            }
            GroupByFieldList = FieldList;
        }

        private DataTable CreateGroupByTable(string tableName, DataTable sourceTable, string fieldList)
        {
            if (fieldList == null || fieldList.Length == 0)
            {
                return sourceTable.Clone();
            }
            else
            {
                DataTable dt = new DataTable(tableName);
                ParseGroupByFieldList(fieldList);
                foreach (FieldInfo Field in GroupByFieldInfo)
                {
                    DataColumn dc = sourceTable.Columns[Field.FieldName];
                    if (Field.Aggregate == null)
                    {
                        dt.Columns.Add(Field.FieldAlias, dc.DataType, dc.Expression);
                    }
                    else
                    {
                        dt.Columns.Add(Field.FieldAlias, dc.DataType);
                    }
                }
                if (ds != null)
                {
                    ds.Tables.Add(dt);
                }
                return dt;
            }
        }

        private void InsertGroupByInto(DataTable destTable, DataTable sourceTable, string fieldList,
                                       string rowFilter, string groupBy)
        {
            if (fieldList == null || fieldList.Length == 0)
            {
                return;
            }
            ParseGroupByFieldList(fieldList);
            ParseFieldList(groupBy, false);
            DataRow[] rows = sourceTable.Select(rowFilter, groupBy);
            DataRow lastSourceRow = null, destRow = null;
            bool sameRow;
            int rowCount = 0;
            foreach (DataRow sourceRow in rows)
            {
                sameRow = false;
                if (lastSourceRow != null)
                {
                    sameRow = true;
                    foreach (FieldInfo Field in m_FieldInfo)
                    {
                        if (!ColumnEqual(lastSourceRow[Field.FieldName], sourceRow[Field.FieldName]))
                        {
                            sameRow = false;
                            break;
                        }
                    }
                    if (!sameRow)
                    {
                        destTable.Rows.Add(destRow);
                    }
                }
                if (!sameRow)
                {
                    destRow = destTable.NewRow();
                    rowCount = 0;
                }
                rowCount += 1;
                foreach (FieldInfo field in GroupByFieldInfo)
                {
                    if (field.Aggregate == null)
                    {
                        destRow[field.FieldAlias] = sourceRow[field.FieldName];
                    }
                    else
                    {
                        switch (field.Aggregate.ToLower())
                        {
                            case null:
                            case "":
                            case "last":
                                destRow[field.FieldAlias] = sourceRow[field.FieldName];
                                break;
                            case "first":
                                if (rowCount == 1)
                                {
                                    destRow[field.FieldAlias] = sourceRow[field.FieldName];
                                }
                                break;
                            case "count":
                                destRow[field.FieldAlias] = rowCount;
                                break;
                            case "sum":
                                destRow[field.FieldAlias] = Add(destRow[field.FieldAlias], sourceRow[field.FieldName]);
                                break;
                            case "max":
                                destRow[field.FieldAlias] = Max(destRow[field.FieldAlias], sourceRow[field.FieldName]);
                                break;
                            case "min":
                                if (rowCount == 1)
                                {
                                    destRow[field.FieldAlias] = sourceRow[field.FieldName];
                                }
                                else
                                {
                                    destRow[field.FieldAlias] = Min(destRow[field.FieldAlias], sourceRow[field.FieldName]);
                                }
                                break;
                        }
                    }
                }
                lastSourceRow = sourceRow;
            }
            if (destRow != null)
            {
                destTable.Rows.Add(destRow);
            }
        }

        private object Min(object a, object b)
        {
            if ((a is DBNull) || (b is DBNull))
            {
                return DBNull.Value;
            }
            if (((IComparable)a).CompareTo(b) == -1)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        private object Max(object a, object b)
        {
            if (a is DBNull)
            {
                return b;
            }
            if (b is DBNull)
            {
                return a;
            }
            if (((IComparable)a).CompareTo(b) == 1)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        private object Add(object a, object b)
        {
            if (a is DBNull)
            {
                return b;
            }
            if (b is DBNull)
            {
                return a;
            }
            return (Convert.ToDecimal(a) + Convert.ToDecimal(b));
        }

        private DataTable CreateJoinTable(string tableName, DataTable sourceTable, string fieldList)
        {
            if (fieldList == null)
            {
                return sourceTable.Clone();
            }
            else
            {
                DataTable dt = new DataTable(tableName);
                ParseFieldList(fieldList, true);
                foreach (FieldInfo field in m_FieldInfo)
                {
                    if (field.RelationName == null)
                    {
                        DataColumn dc = sourceTable.Columns[field.FieldName];
                        dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
                    }
                    else
                    {
                        DataColumn dc = sourceTable.ParentRelations[field.RelationName].ParentTable.Columns[field.FieldName];
                        dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
                    }
                }
                if (ds != null)
                {
                    ds.Tables.Add(dt);
                }
                return dt;
            }
        }

        private void InsertJoinInto(DataTable destTable, DataTable sourceTable,
                                    string fieldList, string rowFilter, string sort)
        {
            if (fieldList == null)
            {
                return;
            }
            else
            {
                ParseFieldList(fieldList, true);
                DataRow[] Rows = sourceTable.Select(rowFilter, sort);
                foreach (DataRow SourceRow in Rows)
                {
                    DataRow DestRow = destTable.NewRow();
                    foreach (FieldInfo Field in m_FieldInfo)
                    {
                        if (Field.RelationName == null)
                        {
                            DestRow[Field.FieldName] = SourceRow[Field.FieldName];
                        }
                        else
                        {
                            DataRow ParentRow = SourceRow.GetParentRow(Field.RelationName);
                            DestRow[Field.FieldName] = ParentRow[Field.FieldName];
                        }
                    }
                    destTable.Rows.Add(DestRow);
                }
            }
        }

        #endregion

        #region SelectDistinct / Distinct

        /**/
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 相当于select distinct fieldName from sourceTable 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldName">列名</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldName指明的列</returns> 
        public DataTable SelectDistinct(string tableName, DataTable sourceTable, string fieldName)
        {
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
            if (ds != null && !ds.Tables.Contains(tableName))
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        /**/
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 相当于select distinct fieldName1,fieldName2,,fieldNamen from sourceTable 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldNames">列名数组</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldNames中指明的列</returns> 
        public DataTable SelectDistinct(string tableName, DataTable sourceTable, string[] fieldNames)
        {
            DataTable dt = new DataTable(tableName);
            object[] values = new object[fieldNames.Length];
            string fields = "";
            for (int i = 0; i < fieldNames.Length; i++)
            {
                dt.Columns.Add(fieldNames[i], sourceTable.Columns[fieldNames[i]].DataType);
                fields += fieldNames[i] + ",";
            }
            fields = fields.Remove(fields.Length - 1, 1);
            DataRow lastRow = null;
            foreach (DataRow dr in sourceTable.Select("", fields))
            {
                if (lastRow == null || !(RowEqual(lastRow, dr, dt.Columns)))
                {
                    lastRow = dr;
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        values[i] = dr[fieldNames[i]];
                    }
                    dt.Rows.Add(values);
                }
            }
            if (ds != null && !ds.Tables.Contains(tableName))
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 相当于select distinct fieldName1,fieldName2,,fieldNamen from sourceTable 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldNames">列名数组</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldNames中指明的列</returns> 
        public DataTable SelectDistinct(string tableName, DataTable sourceTable, List<string> fieldNames)
        {
            return SelectDistinct(tableName, sourceTable, fieldNames.ToArray());
        }

        /**/
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 并且包含sourceTable中所有的列。 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源表</param> 
        /// <param name="fieldName">字段</param> 
        /// <returns>一个新的不含重复行的DataTable</returns> 
        public DataTable Distinct(string tableName, DataTable sourceTable, string fieldName)
        {
            DataTable dt = sourceTable.Clone();
            dt.TableName = tableName;

            object lastValue = null;
            foreach (DataRow dr in sourceTable.Select("", fieldName))
            {
                if (lastValue == null || !(ColumnEqual(lastValue, dr[fieldName])))
                {
                    lastValue = dr[fieldName];
                    dt.Rows.Add(dr.ItemArray);
                }
            }
            if (ds != null && !ds.Tables.Contains(tableName))
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        /**/
        /// <summary> 
        /// 按照fieldNames从sourceTable中选择出不重复的行， 
        /// 并且包含sourceTable中所有的列。 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源表</param> 
        /// <param name="fieldNames">字段</param> 
        /// <returns>一个新的不含重复行的DataTable</returns> 
        public DataTable Distinct(string tableName, DataTable sourceTable, string[] fieldNames)
        {
            DataTable dt = sourceTable.Clone();
            dt.TableName = tableName;
            string fields = "";
            for (int i = 0; i < fieldNames.Length; i++)
            {
                fields += fieldNames[i] + ",";
            }
            fields = fields.Remove(fields.Length - 1, 1);
            DataRow lastRow = null;
            foreach (DataRow dr in sourceTable.Select("", fields))
            {
                if (lastRow == null || !(RowEqual(lastRow, dr, dt.Columns)))
                {
                    lastRow = dr;
                    dt.Rows.Add(dr.ItemArray);
                }
            }
            if (ds != null && !ds.Tables.Contains(tableName))
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }
        /// <summary> 
        /// 按照fieldNames从sourceTable中选择出不重复的行， 
        /// 并且包含sourceTable中所有的列。 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源表</param> 
        /// <param name="fieldNames">字段</param> 
        /// <returns>一个新的不含重复行的DataTable</returns> 
        public DataTable Distinct(string tableName, DataTable sourceTable, List<string> fieldNames)
        {
            return Distinct(tableName, sourceTable, fieldNames.ToArray());
        }
        #endregion

        #region Select Table Into

        /**/
        /// <summary> 
        /// 按sort排序，按rowFilter过滤sourceTable， 
        /// 复制fieldList中指明的字段的数据到新DataTable，并返回之 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源表</param> 
        /// <param name="fieldList">字段列表</param> 
        /// <param name="rowFilter">过滤条件</param> 
        /// <param name="sort">排序</param> 
        /// <returns>新DataTable</returns> 
        public DataTable SelectInto(string tableName, DataTable sourceTable,
                                    string fieldList, string rowFilter, string sort)
        {
            DataTable dt = CreateTable(tableName, sourceTable, fieldList);
            InsertInto(dt, sourceTable, fieldList, rowFilter, sort);
            return dt;
        }

        #endregion

        #region Group By Table
        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sourceTable"></param>
        /// <param name="fieldList"></param>
        /// <param name="rowFilter"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        public DataTable SelectGroupByInto(string tableName, DataTable sourceTable, string fieldList,
                                           string rowFilter, string groupBy)
        {
            DataTable dt = CreateGroupByTable(tableName, sourceTable, fieldList);
            InsertGroupByInto(dt, sourceTable, fieldList, rowFilter, groupBy);
            return dt;
        }

        #endregion

        #region Join Tables

        public DataTable SelectJoinInto(string tableName, DataTable sourceTable, string fieldList, string rowFilter, string sort)
        {
            DataTable dt = CreateJoinTable(tableName, sourceTable, fieldList);
            InsertJoinInto(dt, sourceTable, fieldList, rowFilter, sort);
            return dt;
        }

        #endregion

        #region Create Table

        /// <summary>
        /// 创建DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldList"></param>
        /// <returns></returns>
        public DataTable CreateTable(string tableName, string fieldList)
        {
            DataTable dt = new DataTable(tableName);
            DataColumn dc;
            string[] Fields = fieldList.Split(',');
            string[] FieldsParts;
            string Expression;
            foreach (string Field in Fields)
            {
                FieldsParts = Field.Trim().Split(" ".ToCharArray(), 3); // allow for spaces in the expression 
                                                                        // add fieldname and datatype 
                if (FieldsParts.Length == 2)
                {
                    dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true));
                    dc.AllowDBNull = true;
                }
                else if (FieldsParts.Length == 3) // add fieldname, datatype, and expression 
                {
                    Expression = FieldsParts[2].Trim();
                    if (Expression.ToUpper() == "REQUIRED")
                    {
                        dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true));
                        dc.AllowDBNull = false;
                    }
                    else
                    {
                        dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true), Expression);
                    }
                }
                else
                {
                    return null;
                }
            }
            if (ds != null)
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        /// <summary>
        /// 创建DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldList"></param>
        /// <param name="keyFieldList"></param>
        /// <returns></returns>
        public DataTable CreateTable(string tableName, string fieldList, string keyFieldList)
        {
            DataTable dt = CreateTable(tableName, fieldList);
            string[] KeyFields = keyFieldList.Split(',');
            if (KeyFields.Length > 0)
            {
                DataColumn[] KeyFieldColumns = new DataColumn[KeyFields.Length];
                int i;
                for (i = 1; i == KeyFields.Length - 1; ++i)
                {
                    KeyFieldColumns[i] = dt.Columns[KeyFields[i].Trim()];
                }
                dt.PrimaryKey = KeyFieldColumns;
            }
            return dt;
        }

        #endregion



        /// <summary>  
        /// 将DataTable对象转换成XML字符串  
        /// </summary>  
        /// <param name="dt">DataTable对象</param>  
        /// <returns>XML字符串</returns>  
        public static string ConvertDataToXml(DataTable dt)
        {
            if (dt != null)
            {
                MemoryStream ms = null;
                XmlTextWriter XmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt  
                    XmlWt = new XmlTextWriter(ms, Encoding.Unicode);
                    //获取ds中的数据  
                    dt.WriteXml(XmlWt);
                    int count = (int)ms.Length;
                    byte[] temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本  
                    UnicodeEncoding ucode = new UnicodeEncoding();
                    string returnValue = ucode.GetString(temp).Trim();
                    return returnValue;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源  
                    if (XmlWt != null)
                    {
                        XmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            else
            {
                return "";
            }
        }
        /**/
        /// <summary>  
        /// 将DataSet对象中指定的Table转换成XML字符串  
        /// </summary>  
        /// <param name="ds">DataSet对象</param>  
        /// <param name="tableIndex">DataSet对象中的Table索引</param>  
        /// <returns>XML字符串</returns>  
        public static string ConvertDataToXml(DataSet ds, int tableIndex)
        {
            if (tableIndex != -1)
            {
                return ConvertDataToXml(ds.Tables[tableIndex]);
            }
            else
            {
                return ConvertDataToXml(ds.Tables[0]);
            }
        }
        /**/
        /// <summary>  
        /// 将DataSet对象转换成XML字符串  
        /// </summary>  
        /// <param name="ds">DataSet对象</param>  
        /// <returns>XML字符串</returns>  
        public static string ConvertDataToXml(DataSet ds)
        {
            return ConvertDataToXml(ds, -1);
        }
        /**/
        /// <summary>  
        /// 将DataView对象转换成XML字符串  
        /// </summary>  
        /// <param name="dv">DataView对象</param>  
        /// <returns>XML字符串</returns>  
        public static string ConvertDataToXml(DataView dv)
        {
            return ConvertDataToXml(dv.Table);
        }

        /**/
        /// <summary>  
        /// 将DataSet对象数据保存为XML文件  
        /// </summary>  
        /// <param name="dt">DataSet</param>  
        /// <param name="xmlFilePath">XML文件路径</param>  
        /// <returns>bool值</returns>  
        public static bool ConvertDataToXmlFile(DataTable dt, string xmlFilePath)
        {
            if ((dt != null) && (!string.IsNullOrEmpty(xmlFilePath)))
            {
                string path = HttpContent.Current.Server.MapPath(xmlFilePath);
                MemoryStream ms = null;
                XmlTextWriter XmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt  
                    XmlWt = new XmlTextWriter(ms, Encoding.Unicode);
                    //获取ds中的数据  
                    dt.WriteXml(XmlWt);
                    int count = (int)ms.Length;
                    byte[] temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本  
                    UnicodeEncoding ucode = new UnicodeEncoding();
                    //写文件  
                    StreamWriter sw = new StreamWriter(path);
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    sw.WriteLine(ucode.GetString(temp).Trim());
                    sw.Close();
                    return true;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源  
                    if (XmlWt != null)
                    {
                        XmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            else
            {
                return false;
            }
        }
        /**/
        /// <summary>  
        /// 将DataSet对象中指定的Table转换成XML文件  
        /// </summary>  
        /// <param name="ds">DataSet对象</param>  
        /// <param name="tableIndex">DataSet对象中的Table索引</param>  
        /// <param name="xmlFilePath">xml文件路径</param>  
        /// <returns>bool]值</returns>  
        public static bool ConvertDataToXmlFile(DataSet ds, int tableIndex, string xmlFilePath)
        {
            if (tableIndex != -1)
            {
                return ConvertDataToXmlFile(ds.Tables[tableIndex], xmlFilePath);
            }
            else
            {
                return ConvertDataToXmlFile(ds.Tables[0], xmlFilePath);
            }
        }
        /**/
        /// <summary>  
        /// 将DataSet对象转换成XML文件  
        /// </summary>  
        /// <param name="ds">DataSet对象</param>  
        /// <param name="xmlFilePath">xml文件路径</param>  
        /// <returns>bool]值</returns>  
        public static bool ConvertDataToXmlFile(DataSet ds, string xmlFilePath)
        {
            return ConvertDataToXmlFile(ds, -1, xmlFilePath);
        }
        /**/
        /// <summary>  
        /// 将DataView对象转换成XML文件  
        /// </summary>  
        /// <param name="dv">DataView对象</param>  
        /// <param name="xmlFilePath">xml文件路径</param>  
        /// <returns>bool]值</returns>  
        public static bool ConvertDataToXmlFile(DataView dv, string xmlFilePath)
        {
            return ConvertDataToXmlFile(dv.Table, xmlFilePath);
        }
        //C#代码
        private string ConvertDataTableToXML(DataTable xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream, Encoding.Default);
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                UTF8Encoding utf = new UTF8Encoding();
                return utf.GetString(arr).Trim();
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

    }
}