///*************************************************************************//
/// System/Module Name: GatewayActiveNode
/// FileName:          
/// Author:WXG
///
/// Change History: 
/// Version	    	Date		           By	             Details
/// 1.0.1       	00/00/2013 00:00:00 PM                      Dematic-WXG      Create
/// 1.0.2       	00/00/2013 00:00:00 PM                      Dematic-WXG      Modify
///***************************************************************************//
/// Function: 
/// 
/// ModifyDetail:
/// @1.0.1:Create
/// @1.0.2:Modify Update For If UpdateString is Empty Then the Update Successed!
///***************************************************************************//
using System;
using System.Collections;
using System.Data;
using System.Text;
using LIB_Common;

namespace LIB_DB
{
    /// <summary>
    /// 主表数据抽象应用类
    /// </summary>
    public class DataOperationOracle : DataOperation
    {
        #region 属性
        /// <summary>
        /// 得到Inert操作的字段串
        /// </summary>
        public override string InsertFieldsString
        {
            get
            {
                StringBuilder fs = new StringBuilder();
                string strcomma = ",";
                bool badded = false;
                for (int i = 0; i < Fields.Count; i++)
                {
                    strcomma = (badded ? "," : "");
                    if (Fields[i].ReadOnly) continue;
                    fs.Append(strcomma + FunBase.ChkField(Fields[i].FieldName, false));
                    badded = true;
                    //if (i < Fields.Count - 1) fs.Append(",");
                }
                return fs.ToString();
            }
        }
        /// <summary>
        /// 得到Inert操作的Values串
        /// </summary>
        public override string InsertValuesString
        {
            get
            {
                StringBuilder vs = new StringBuilder();
                string strcomma = ",";
                bool badded = false;
                for (int i = 0; i < Fields.Count; i++)
                {
                    strcomma = (badded ? "," : "");
                    if (Fields[i].ReadOnly) continue;
                    if (!Fields[i].AllowModify) continue;       //WXG2014010@Dematic.Office
                    if (Fields[i].Value == null) Fields[i].Value = DBNull.Value;
                    if (Fields[i].type == typeof(string))
                        vs.Append(strcomma + (Fields[i].IsSafe ? Fields[i].Value.ToString() : FunBase.ChkField(Fields[i].Value.ToString(), true)));
                    else if (Fields[i].type == typeof(DateTime))
                    {
                        //WXG20140805@Basf.Site
                        //if (Fields[i].Value == DBNull.Value || ((DateTime)Fields[i].Value == new DateTime()))
                        if (Fields[i].Value == DBNull.Value || ((Fields[i].Value is DateTime) && (DateTime)Fields[i].Value == new DateTime()))
                        {
                            //WXG20140410@Dematic.Office
                            //vs.Append(strcomma + "systimestamp"); 
                            vs.Append(strcomma + "''");
                        }
                        else
                        {
                            if (Fields[i].Value is DateTime)
                            {
                                vs.Append(strcomma + string.Format("To_timestamp('{0}','{1}')",
                                          ((DateTime)Fields[i].Value).ToString(FunBase.STR_DATETIME_TO_DB_ORC),
                                          FunBase.STR_SAVE_DB_ORC));
                            }
                            else
                            {
                                vs.Append(strcomma + string.Format("To_timestamp('{0}','{1}')",
                                                        (DateTime.Parse(Fields[i].Value.ToString())).ToString(FunBase.STR_DATETIME_TO_DB_ORC),
                                                       FunBase.STR_SAVE_DB_ORC));
                            }
                        }
                    }
                    else if (Fields[i].type == typeof(Int32))
                    {
                        vs.Append(strcomma + (Fields[i].IsSafe ? (Fields[i].GetValueAsInt().ToString()) : FunBase.ChkField(Fields[i].GetValueAsInt().ToString(), true)));
                    }
                    else
                    {
                        vs.Append(strcomma + (Fields[i].IsSafe ? (Fields[i].Value.ToString()) : FunBase.ChkField(Fields[i].Value.ToString(), true)));

                    }
                    badded = true;
                    //if (i < Fields.Count - 1) vs.Append(",");
                }
                return vs.ToString();
            }
        }
        /// <summary>
        /// 得到Where条件串
        /// </summary>
        public override string WhereSql
        {
            get
            {
                StringBuilder sbWhere = new StringBuilder();
                if (WhereFields == null || WhereFields.Count <= 0)
                {
                    for (int i = 0; i < Fields.Count; i++)
                    {
                        if (this.Fields[i].IsKey)
                        {
                            if (sbWhere.Length > 0)
                            {
                                sbWhere.Append(" and ");
                            }
                            sbWhere.Append(FunBase.ChkField(this.Fields[i].FieldName));
                            sbWhere.Append(" = ");
                            //WXG20130319
                            sbWhere.Append(Fields[i].IsSafe ? Fields[i].GetValueAsString() : FunBase.ChkField(this.Fields[i].GetValueAsString(), this.Fields[i].type == typeof(string)));
                            //sbWhere.Append(Fields[i].IsSafe ? Fields[i].Old_Value.ToString() : FunBase.ChkField(this.Fields[i].Old_Value.ToString(), this.Fields[i].type == typeof(string)));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < WhereFields.Count; i++)
                    {
                        if (this.Fields[i].IsKey)
                        {
                            if (sbWhere.Length > 0)
                            {
                                sbWhere.Append(" and ");
                            }
                            sbWhere.Append(FunBase.ChkField(this.WhereFields[i].FieldName));
                            sbWhere.Append(" = ");
                            //WXG20130319
                            //sbWhere.Append(Fields[i].IsSafe ? Fields[i].Old_Value.ToString() : FunBase.ChkField(this.Fields[i].Old_Value.ToString(), this.WhereFields[i].type == typeof(string)));
                            sbWhere.Append(Fields[i].IsSafe ? Fields[i].GetValueAsString() : FunBase.ChkField(this.Fields[i].GetValueAsString(), this.Fields[i].type == typeof(string)));
                        }
                    }
                }

                return sbWhere.ToString();
            }
        }
        /// <summary>
        /// 得到Update中Set字段的串
        /// </summary>
        public override string UpdateFieldsString
        {
            get
            {
                StringBuilder sbUpdte = new StringBuilder();

                for (int i = 0; i < this.Fields.Count; i++)
                {
                    if (Fields[i].ReadOnly) continue;
                    if (!Fields[i].AllowModify) continue;       //WXG2014010@Dematic.Office
                    if (this.Fields[i].IsUpdate || IsForceUpdate)
                    {
                        sbUpdte.Append(FunBase.ChkField(this.Fields[i].FieldName));
                        sbUpdte.Append(" = ");
                        if (Fields[i].Value == null) Fields[i].Value = DBNull.Value;
                        if (Fields[i].type == typeof(string))
                            sbUpdte.Append(Fields[i].IsSafe ? (Fields[i].Value.ToString() + ",") : FunBase.ChkField(Fields[i].Value.ToString(), true, true));
                        else if (Fields[i].type == typeof(DateTime))
                        {
                            //WXG20140805@Basf.Site
                            //if (Fields[i].Value == DBNull.Value || ((DateTime)Fields[i].Value == new DateTime()))
                            if (Fields[i].Value == DBNull.Value || ((Fields[i].Value is DateTime) && (DateTime)Fields[i].Value == new DateTime()))
                            {
                                //WXG20140410@Dematic.Office
                                //sbUpdte.Append("systimestamp,");
                                //vs.Append(strcomma + "''");
                                sbUpdte.Append("'',");
                            }
                            else

                                //sbUpdte.Append(string.Format("To_Date('{0}','{1}'),",
                                //          ((DateTime)Fields[i].Value).ToString("yyyy-MM-dd HH:mm:ss"),
                                //          "YYYY-MM-DD HH24:MI:SS"));       
                                if (Fields[i].Value is DateTime)
                                {
                                    sbUpdte.Append(string.Format("To_timestamp('{0}','{1}'),",
                                              ((DateTime)Fields[i].Value).ToString(FunBase.STR_DATETIME_TO_DB_ORC),
                                              FunBase.STR_SAVE_DB_ORC));
                                }
                                else
                                {
                                    sbUpdte.Append(string.Format("To_timestamp('{0}','{1}'),",
                                                            (DateTime.Parse(Fields[i].Value.ToString())).ToString(FunBase.STR_DATETIME_TO_DB_ORC),
                                                           FunBase.STR_SAVE_DB_ORC));
                                }

                        }
                        else
                        {
                            //sbUpdte.Append(Fields[i].Value.ToString()+",");
                            sbUpdte.Append(Fields[i].IsSafe ? (Fields[i].Value.ToString() + ",") : FunBase.ChkField(Fields[i].Value.ToString(), true, true));
                        }

                        //sbUpdte.Append(Fields[i].IsSafe ? (Fields[i].Value.ToString() +",")  : FunBase.ChkField(this.Fields[i].Value.ToString(), true, true));
                    }
                }
                if (sbUpdte.ToString() == "")
                {
                    return "";
                }
                else
                {
                    return sbUpdte.ToString().Remove(sbUpdte.ToString().Length - 1, 1);
                }
            }
        }
        /// <summary>
        /// 得到Select语句要显示的字段串
        /// </summary>
        public override string SelectFieldsString
        {
            get
            {
                StringBuilder sbSelect = new StringBuilder();
                for (int i = 0; i < Fields.Count; i++)
                {
                    if (i == this.Fields.Count - 1)
                    {
                        sbSelect.Append(FunBase.ChkField(this.Fields[i].FieldName, false, false));
                    }
                    else
                    {
                        sbSelect.Append(FunBase.ChkField(this.Fields[i].FieldName, false, true));
                    }

                }
                return sbSelect.ToString();
            }
        }

        #endregion
        #region 构造函数
        /// <summary>
        /// 自动读数据库载入数据的构造函数
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="db">操作数据类</param>
        /// <param name="IsLoadDt">是否从数据库载入数据到类中</param>
        /// <param name="fields">Field集合</param>
        public DataOperationOracle(string tablename, DBService db, bool IsLoadDt, FieldCollection fields)
            : base(tablename, db, IsLoadDt, fields)
        {
        }

        #endregion
        #region  Public 方法
        /// <summary>
        /// 在数据库中更新
        /// </summary>
        /// <returns>是否成功</returns>
        public override bool Update()
        {
            //if (UpdateFieldsString.ToString().Trim().Length <= 0)
            //{
            //    return false;
            //}
            string tmpSql = "";
            string whereSql = WhereSql;
            string updateString = UpdateFieldsString.Trim();
            //--------------------WXG201301114-15:00@Office  For V1.0.2--------------------
            if (updateString.Length == 0)
            {
                //tmpSql = "Update " + TableName + " Set " + whereSql + " Where " + whereSql;
                WhereFields.Clear();
                return true;
            }
            else
            {
                tmpSql = "Update " + TableName + " Set " + updateString + " Where " + whereSql;
            }
            //--------------------WXG201301114-15:00@Office  For V1.0.2--------------------
            theDB.UpdataDataBase(tmpSql, out errCode, out changeNum);
            if (errCode == "0")
            {
                WhereFields.Clear();
                this.SyncFields();
                return true;
            }
            else
            {
                WhereFields.Clear();
                return false;
            }
        }
        /// <summary>
        /// 在数据库中插入Data中数据(根据WhereSql)
        /// </summary>
        /// <returns>是否成功</returns>
        public override bool Insert()
        {
            string tmpSql = "Insert Into " + TableName + "(" + InsertFieldsString + ") Values(" + InsertValuesString + ")";
            theDB.UpdataDataBase(tmpSql, out errCode, out changeNum);
            if (errCode == "0")
            {
                WhereFields.Clear();
                this.SyncFields();
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool Insert(out object id)
        {
            string tmpSql = "Insert Into " + TableName + "(" + InsertFieldsString + ") Values(" + InsertValuesString + ")";
            theDB.UpdataDataBase(tmpSql, out errCode, out changeNum);
            if (errCode == "0")
            {
                this.SyncFields();
                id = theDB.SelectScalar("Select ID From " + TableName + " Where " + WhereSql);
                WhereFields.Clear();
                return true;
            }
            else
            {
                id = null;
                return false;
            }
        }
        /// <summary>
        /// 在数据库中删除Data中数据(根据WhereSql)
        /// </summary>
        /// <returns>是否成功</returns>
        public override bool Delete()
        {
            string tmpSql = "Delete " + TableName + " Where " + WhereSql;
            theDB.UpdataDataBase(tmpSql, out errCode, out changeNum);
            if (errCode == "0")
            {
                WhereFields.Clear();
                this.SyncFields();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 得到字段和值得hashtable
        /// </summary>
        /// <returns></returns>
        public override Hashtable ToHashTable()
        {
            return Fields.ToHashByValue();
        }
        /// <summary>
        /// 得到数据DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        public override DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < Fields.Count; i++)
            {
                dt.Columns.Add(Fields[i].FieldName, Fields[i].type);
            }
            DataRow dr = dt.NewRow();
            for (int i = 0; i < Fields.Count; i++)
            {
                dr[i] = Fields[i].Value;
            }
            dt.Rows.Add(dr);
            return dt;
        }
        /// <summary>
        /// 得到数据DataTableByDesc
        /// </summary>
        /// <returns>DataTable</returns>
        public override DataTable ToDataTableByDesc()
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < Fields.Count; i++)
            {
                dt.Columns.Add(Fields[i].Desc == null ? Fields[i].FieldName : Fields[i].Desc, Fields[i].type);
            }
            DataRow dr = dt.NewRow();
            for (int i = 0; i < Fields.Count; i++)
            {
                dr[i] = Fields[i].Value;
            }
            dt.Rows.Add(dr);
            return dt;
        }

        /// <summary>
        /// 同步Field的Value和Old_Value;
        /// </summary>

        protected override void GetDisplayText()
        {
            for (int i = 0; i < Fields.Count; i++)
            {
                if (Fields[i].RelationshipTable == null || Fields[i].RelationshipTable.Length == 0 || Fields[i].Value == null)
                {
                    Fields[i].DisplayText = (Fields[i].Value == null ? "" : Fields[i].Value.ToString());

                }
                else
                {
                    string sql = string.Format("Select {0} From {1} Where {2}={3}",
                                                Fields[i].RelationshipDisField,
                                                Fields[i].RelationshipTable,
                                                Fields[i].RelationshipValField,
                                                FunBase.ChkField(Fields[i].Value.ToString(), true));
                    object obj = theDB.SelectScalar(sql);
                    Fields[i].DisplayText = (obj == null ? "" : obj.ToString());
                }
            }
        }
        #endregion
        #region protected
        /// <summary>
        /// 从数据库得到初始化数据
        /// </summary>
        protected override void InitData()
        {
            string sql = string.Format("Select {0} From {1} Where {2}", SelectFieldsString, this.TableName, WhereSql);
            this.theDB.DB_Open();
            DataTable dt = this.theDB.SelectDataSet(sql, out errCode).Tables[0];
            if (dt.Rows.Count == 1)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    Fields[i].Value = dt.Rows[0][Fields[i].FieldName];
                    Fields[i].SyncField();
                }
                this._isGetData = true;
            }
            else if (dt.Rows.Count == 0)
            {
                this._isGetData = false;
            }
            else
            {
                this._isGetData = false;
                throw WAPExceptionHelper.GetWAPException(10001053, sql, null);
            }
        }
        #endregion
        /// <summary>
        /// 根据DataSet批量插入数据
        /// </summary>
        /// <param name="Ds"></param>
        /// <returns></returns>
        public override bool InsertByDataSet(DataSet Ds)
        {
            try
            {
                for (int i = 0; i < Ds.Tables.Count; i++)
                {
                    StringBuilder sqlb = new StringBuilder();
                    if (Ds.Tables[i] == null) return false;
                    sqlb.Append(string.Format("Insert into {0}(", Ds.Tables[i].TableName));
                    //bool isfirst0 = true;
                    foreach (DataColumn thecol in Ds.Tables[i].Columns)
                    {
                        sqlb.Append(thecol.ColumnName);
                        sqlb.Append(",");
                    }
                    sqlb.Remove(sqlb.ToString().Length - 1, 1);
                    sqlb.Append(") ");
                    foreach (DataRow therow in Ds.Tables[i].Rows)
                    {
                        sqlb.Append(" Select ");
                        for (int j = 0; j < Ds.Tables[i].Columns.Count; i++)
                        {
                            sqlb.Append(string.Format("'{0}',", FunBase.ChkField(therow[j].ToString(), Ds.Tables[i].Columns[j].DataType == typeof(string), false)));
                        }
                        sqlb.Remove(sqlb.ToString().Length - 1, 1);
                        sqlb.Append(" union ");
                    }
                    sqlb.Remove(sqlb.ToString().Length - 6, 6);
                    theDB.UpdataDataBase(sqlb.ToString(), out errCode, out changeNum);
                }
                return true;
            }
            catch (Exception ex)
            {
                this.errMessage = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 根据DataSet批量更新数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dBTable"></param>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public override bool UpdateByDataSet(DataTable dt, string dBTable, string KeyWord)
        {
            try
            {
                StringList UpdateSqls = new StringList();
                StringBuilder strbUpdateSet;

                int irow;
                strbUpdateSet = new StringBuilder("Update Set");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        strbUpdateSet.Append(string.Format("{0}={1},",
                                            dt.Columns[j].ColumnName,
                                            FunBase.ChkField(dt.Rows[i].ToString(), false, false)));
                    }
                    strbUpdateSet.Remove(strbUpdateSet.ToString().Length - 1, 1);

                    UpdateSqls[i] = string.Format("Update  Set {0} From {1}  Where {2}='{3}' ",
                                    strbUpdateSet.ToString(),
                                    dBTable,
                                    KeyWord,
                                    FunBase.ChkField(dt.Rows[i][KeyWord].ToString(), false, false));
                }
                if (theDB.BatchSQL(UpdateSqls, out irow))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.errMessage = ex.Message;
                return false;
            }
        }
        public override bool DeleteByDataSet(string dBTable, string whereKeyName, ArrayList listKeyValue)
        {
            try
            {
                StringList deleteSqls = new StringList();
                int irow;
                for (int i = 0; i < listKeyValue.Count; i++)
                {
                    deleteSqls[i] = string.Format("Delete {0}  Where {1}={2} ",
                                    dBTable,
                                    whereKeyName,
                                    FunBase.ChkField((string)listKeyValue[i], false, false));
                }
                if (theDB.BatchSQL(deleteSqls, out irow))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.errMessage = ex.Message;
                return false;
            }
        }
    }
}
