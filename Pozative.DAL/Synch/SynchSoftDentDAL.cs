using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Pozative.QRY;
using System.Data.SqlServerCe;
using Pozative.UTL;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace Pozative.DAL.Synch
{
    public class SynchSoftDentDAL
    {

        private static string CreateQueryToInsert(string mode, DataTable dtInsertTable, DataRow dtRow, string tableName, string ignoreColumns, string primaryKeyColumnsName)
        {
            try
            {
                string StrSqlQuery = string.Empty;
                string columnsName = string.Empty;
                string values = string.Empty;

                string[] ignoreColumnsArr = new string[] { "" };
                ignoreColumnsArr = ignoreColumns.Split(',');

                if (mode.ToUpper() == "INSERT")
                {
                    StrSqlQuery = " INSERT INTO " + tableName + " ( ";
                    foreach (DataColumn dcCol in dtInsertTable.Columns)
                    {
                        var result = ignoreColumnsArr.AsEnumerable().Where(o => o.ToString().ToUpper() == dcCol.ColumnName.ToUpper());

                        if ((result != null && result.Count() == 0) && dcCol.ColumnName != "image" && dcCol.ColumnName != "InsUptDlt")
                        {
                            columnsName = columnsName == string.Empty ? dcCol.ColumnName : columnsName + "," + dcCol.ColumnName;

                            if (dcCol.DataType.ToString() == "System.String")
                            {
                                if (Convert.ToInt16(dtRow[dcCol.ColumnName].ToString().Length) > Convert.ToInt16(dcCol.ExtendedProperties["Size"]))
                                {
                                    values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString().Substring(0, Convert.ToInt16(dcCol.ExtendedProperties["Size"]) > 0 ? Convert.ToInt16(dcCol.ExtendedProperties["Size"]) : dtRow[dcCol.ColumnName].ToString().Length).Replace("'", "''") + "'" : values + ",'" + dtRow[dcCol.ColumnName].ToString().Substring(0, Convert.ToInt16(dcCol.ExtendedProperties["Size"]) > 0 ? Convert.ToInt16(dcCol.ExtendedProperties["Size"]) : dtRow[dcCol.ColumnName].ToString().Length).Replace("'", "''") + "'";
                                }
                                else
                                {
                                    values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString() + "'" : values + "," + "'" + dtRow[dcCol.ColumnName].ToString().Replace("'", "''") + "'";
                                }
                            }
                            else if (dcCol.DataType.ToString() == "System.Int64")
                            {
                                values = values == string.Empty ? dtRow[dcCol.ColumnName].ToString() : values + "," + dtRow[dcCol.ColumnName].ToString();
                            }
                            else if (dcCol.DataType.ToString() == "System.Boolean")
                            {
                                values = values == string.Empty ? (dtRow[dcCol.ColumnName].ToString() == "True" ? "1" : "0") : values + "," + (dtRow[dcCol.ColumnName].ToString() == "True" ? "1" : "0");
                            }
                            else if (dcCol.DataType.ToString() == "System.DateTime")
                            {
                                //values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString() + "'" : values + ",'" + dtRow[dcCol.ColumnName].ToString() + "'";

                                try
                                {
                                    values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString() + "'" : values + ",'" + Utility.CheckValidDatetime(dtRow[dcCol.ColumnName].ToString()) + "'";
                                }
                                catch (Exception)
                                {

                                }
                            }
                            else if (dcCol.DataType.ToString() == "System.Byte[]")
                            {
                                values = values == string.Empty ? dtRow[dcCol.ColumnName].ToString() : values + "," + dtRow[dcCol.ColumnName].ToString();
                            }
                            else
                            {
                                if (Convert.ToInt16(dtRow[dcCol.ColumnName].ToString().Length) > Convert.ToInt16(dcCol.ExtendedProperties["Size"]))
                                {
                                    values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString().Substring(0, Convert.ToInt16(dcCol.ExtendedProperties["Size"]) > 0 ? Convert.ToInt16(dcCol.ExtendedProperties["Size"]) : dtRow[dcCol.ColumnName].ToString().Length) + "'" : values + ",'" + dtRow[dcCol.ColumnName].ToString().Substring(0, Convert.ToInt16(dcCol.ExtendedProperties["Size"]) > 0 ? Convert.ToInt16(dcCol.ExtendedProperties["Size"]) : dtRow[dcCol.ColumnName].ToString().Length) + "'";
                                }
                                else
                                {
                                    values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString() + "'" : values + ",'" + dtRow[dcCol.ColumnName].ToString() + "'";
                                }
                            }
                        }
                    }
                    StrSqlQuery = StrSqlQuery + columnsName + ") Values (" + values + ")";
                }
                else if (mode.ToUpper() == "UPDATE")
                {
                    StrSqlQuery = " UPDATE " + tableName + " SET ";

                    foreach (DataColumn dcCol in dtInsertTable.Columns)
                    {
                        var result = ignoreColumnsArr.AsEnumerable().Where(o => o.ToString().ToUpper() == dcCol.ColumnName.ToUpper());

                        if ((result != null && result.Count() == 0) && dcCol.ColumnName != "image" && dcCol.ColumnName != "InsUptDlt")
                        {
                            columnsName = columnsName == string.Empty ? dcCol.ColumnName : columnsName + "," + dcCol.ColumnName;

                            if (dcCol.DataType.ToString() == "System.String")
                            {
                                if (Convert.ToInt16(dtRow[dcCol.ColumnName].ToString().Length) > Convert.ToInt16(dcCol.ExtendedProperties["Size"]))
                                {
                                    columnsName = columnsName + " = '" + dtRow[dcCol.ColumnName].ToString().Substring(0, Convert.ToInt16(dcCol.ExtendedProperties["Size"]) > 0 ? Convert.ToInt16(dcCol.ExtendedProperties["Size"]) : dtRow[dcCol.ColumnName].ToString().Length).Replace("'", "''") + "'";
                                }
                                else
                                {
                                    columnsName = columnsName + " = '" + dtRow[dcCol.ColumnName].ToString().Replace("'", "''") + "'";
                                }
                            }
                            else if (dcCol.DataType.ToString() == "System.Int64")
                            {
                                columnsName = columnsName + " = " + dtRow[dcCol.ColumnName].ToString() + "";
                            }
                            else if (dcCol.DataType.ToString() == "System.Boolean")
                            {
                                //values = values == string.Empty ? (dtRow[dcCol.ColumnName].ToString() == "True" ? "1" : "0") : values + "," + (dtRow[dcCol.ColumnName].ToString() == "True" ? "1" : "0");
                                columnsName = columnsName + " = " + (dtRow[dcCol.ColumnName].ToString() == "True" ? "1" : "0") + "";
                            }
                            else if (dcCol.DataType.ToString() == "System.DateTime")
                            {
                                // columnsName = columnsName + " = '" + dtRow[dcCol.ColumnName].ToString() + "'";

                                try
                                {
                                    columnsName = columnsName + " = '" + Utility.CheckValidDatetime(dtRow[dcCol.ColumnName].ToString()) + "'";
                                }
                                catch (Exception)
                                {
                                    //   outError = columnsName + " Value : " + Utility.CheckValidDatetime(dtRow[dcCol.ColumnName].ToString()) + " Err_Msg : " + ex2.Message.ToString();
                                }
                            }
                            else if (dcCol.DataType.ToString() == "System.Byte[]")
                            {
                                columnsName = columnsName + " = " + dtRow[dcCol.ColumnName].ToString() + "";
                            }
                            else
                            {
                                if (Convert.ToInt16(dtRow[dcCol.ColumnName].ToString().Length) > Convert.ToInt16(dcCol.ExtendedProperties["Size"]))
                                {
                                    columnsName = columnsName + " = '" + dtRow[dcCol.ColumnName].ToString().Substring(0, Convert.ToInt16(dcCol.ExtendedProperties["Size"]) > 0 ? Convert.ToInt16(dcCol.ExtendedProperties["Size"]) : dtRow[dcCol.ColumnName].ToString().Length) + "'";
                                }
                                else
                                {
                                    columnsName = columnsName + " = '" + dtRow[dcCol.ColumnName].ToString() + "'";
                                }
                            }
                        }
                    }
                    StrSqlQuery = StrSqlQuery + columnsName.ToString() + " WHERE " + primaryKeyColumnsName + " = " + dtRow[primaryKeyColumnsName];
                }
                else if (mode.ToUpper() == "DELETE" && (tableName == "Appointment" || tableName == "OperatoryEvent"))
                {
                    StrSqlQuery = " UPDATE " + tableName + " SET IS_Deleted = 1,IS_Adit_Updated = 0 WHERE " + primaryKeyColumnsName + " = " + dtRow[primaryKeyColumnsName];
                }
                else if (mode.ToUpper() == "DELETE" && tableName == "Patient")
                {
                    StrSqlQuery = " UPDATE " + tableName + " SET is_deleted = 1, Is_Adit_Updated = 0, Status = 'I' WHERE " + primaryKeyColumnsName + " = " + dtRow[primaryKeyColumnsName];
                }
                return StrSqlQuery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool DeleteByPatientID(string DeletedEHRIDs)
        {

            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                    SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", DeletedEHRIDs);
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SqlCeSelect;
                        SqlCeCommand.ExecuteNonQuery();
                    }

                    _successfullstataus = true;
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool Save_SoftDent_To_Local(DataTable dtSoftDentDataToSave, string tablename, string ignoreColumnsName, string primaryKeyColumnsName)
        {
            bool _successfullstataus = true;
            SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtSoftDentDataToSave, "0", "1");
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;

                    foreach (DataRow dr in dtSoftDentDataToSave.Rows)
                    {
                        // SqlCetx = conn.BeginTransaction();
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;

                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {

                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = CreateQueryToInsert("Insert", dtSoftDentDataToSave, dr, tablename, ignoreColumnsName, primaryKeyColumnsName);//"Appt_LocalDB_ID,Appt_Web_ID", ""
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = CreateQueryToInsert("Update", dtSoftDentDataToSave, dr, tablename, ignoreColumnsName, primaryKeyColumnsName);//"Appt_LocalDB_ID,Appt_Web_ID,Appt_EHR_ID", "Appt_LocalDB_ID"
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = CreateQueryToInsert("Delete", dtSoftDentDataToSave, dr, tablename, ignoreColumnsName, primaryKeyColumnsName);//"", "Appt_LocalDB_ID"
                                        break;
                                }
                                SqlCeCommand.ExecuteNonQuery();
                                try
                                {
                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2 && tablename == "Appointment")
                                    {
                                        if (DateTime.Now > Convert.ToDateTime(dr["Appt_EndDateTime"]) && dr["Appointment_Status"].ToString().ToUpper() == "CHECKEDIN")
                                        {
                                            SqlCeCommand.CommandText = "Update Appointment Set appointment_status_ehr_key = 4,Appointment_Status = 'Checkedout' WHere Appointment_Status = 'CheckedIn' AND  Appt_EHR_ID = " + dr["Appt_EHR_ID"].ToString();  //CreateQueryToInsert("Update", dtSoftDentDataToSave, dr, tablename, ignoreColumnsName, primaryKeyColumnsName);//"Appt_LocalDB_ID,Appt_Web_ID,Appt_EHR_ID", "Appt_LocalDB_ID"
                                            SqlCeCommand.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                            //SqlCetx.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

    }
}
