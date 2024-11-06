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
using System.Collections;
using Pozative.BO;
using RestSharp;
using System.Net;
using System.IO;
using Microsoft.Win32;

namespace Pozative.DAL
{
    public class SynchTrackerDAL
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
                                    if (!Utility.NotAllowToChangeSystemDateFormat)
                                    {
                                        values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString() + "'" : values + ",'" + Utility.CheckValidDatetime(dtRow[dcCol.ColumnName].ToString()) + "'";
                                    }
                                    else
                                    {
                                        string dt = Utility.CheckValidDatetime(dtRow[dcCol.ColumnName].ToString());
                                        if (dt == string.Empty)
                                        {
                                            values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString() + "'" : values + ",'" + dt + "'";
                                        }
                                        else
                                        {
                                            values = values == string.Empty ? "'" + dtRow[dcCol.ColumnName].ToString() + "'" : values + ",'" + Convert.ToDateTime(dt).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
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
                    columnsName = string.Empty;
                    foreach (DataColumn dcCol in dtInsertTable.Columns)
                    {
                        var result = ignoreColumnsArr.AsEnumerable().Where(o => o.ToString().ToUpper() == dcCol.ColumnName.ToUpper());

                        if ((result != null && result.Count() == 0) && dcCol.ColumnName != "image" && dcCol.ColumnName != "InsUptDlt" && (dcCol.ColumnName != "Patient_Status"))
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
                                    if (!Utility.NotAllowToChangeSystemDateFormat)
                                    {
                                        columnsName = columnsName + " = '" + Utility.CheckValidDatetime(dtRow[dcCol.ColumnName].ToString()) + "'";
                                    }
                                    else
                                    {
                                        string dt = Utility.CheckValidDatetime(dtRow[dcCol.ColumnName].ToString());
                                        if (dt == string.Empty)
                                        {
                                            columnsName = columnsName + " = '" + dt + "'";
                                        }
                                        else
                                        {
                                            columnsName = columnsName + " = '" + Convert.ToDateTime(dt).ToString("yyyy/MM/dd HH:mm:ss") + "'";
                                        }
                                    }
                                }
                                catch (Exception ex)
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
                else if (mode.ToUpper() == "DELETE" && (tableName == "Dentrix_Form" || tableName == "Dentrix_FormQuestion" || tableName == "EagleSoftAlertMaster" || tableName == "EagleSoftFormMaster" || tableName == "EagleSoftSectionItemMaster" || tableName == "EagleSoftSectionItemOptionMaster" || tableName == "EagleSoftSectionMaster" || tableName == "Appointment" || tableName == "OperatoryEvent" || tableName == "ProviderHours" || tableName == "OperatoryHours" || tableName == "ProviderOfficeHours" || tableName == "OperatoryOfficeHours" || tableName == "CD_FormMaster" || tableName == "CD_QuestionMaster" || tableName == "Holiday" || tableName == "OD_SheetDef" || tableName == "OD_SheetFieldDef" || tableName == "EasyDental_Question"))
                {
                    StrSqlQuery = " UPDATE " + tableName + " SET IS_Deleted = 1,IS_Adit_Updated = 0 WHERE IS_Deleted = 0 and " + primaryKeyColumnsName + " = " + dtRow[primaryKeyColumnsName];
                }
                else if (mode.ToUpper() == "DELETE" && (tableName == "RecallType" || tableName == "Appointment_Type"))
                {
                    StrSqlQuery = " UPDATE " + tableName + " SET IS_Deleted = 1,IS_Adit_Updated = 0 WHERE " + primaryKeyColumnsName + " = " + dtRow[primaryKeyColumnsName];
                    //StrSqlQuery = " Delete from " + tableName + " WHERE " + primaryKeyColumnsName + " = " + dtRow[primaryKeyColumnsName];
                }
                return StrSqlQuery;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetTrackerUserLoginId()
        {
            string UserLoginID = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                if (UserLoginID == "")
                {
                    SqlCommand.CommandTimeout = 200;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlSelect = SynchTrackerQRY.GetTrackerUserLoginId;
                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                    DataTable SqlDt = new DataTable();
                    SqlDa.Fill(SqlDt);
                    if (SqlDt != null && SqlDt.Rows.Count > 0)
                    {
                        UserLoginID = SqlDt.Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                UserLoginID = "Adit";
                throw ex;

            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return UserLoginID;
        }

        public static bool Save_Users_Tracker_To_Local(DataTable dtTrackerUser)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtTrackerUser.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_User;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_User;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_User;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("User_EHR_ID", dr["User_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("User_EHR_ID", dr["User_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("User_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Updated_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Active", Convert.ToBoolean(dr["Is_Active"]));
                                    SqlCeCommand.Parameters.AddWithValue("Is_Deleted", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                }
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
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

        public static string GetPatientName(int PatientId)
        {
            string patientname = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetPatientName;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("@PatientId", PatientId);
                patientname = Convert.ToString(SqlCommand.ExecuteScalar());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return patientname;
        }

        public static bool Save_Tracker_To_Local(DataTable dtSoftDentDataToSave, string tablename, string ignoreColumnsName, string primaryKeyColumnsName, string _filename_EHR_PatientFormt = "", string _EHRLogdirectory_EHR_PatientForm = "")
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;

                    foreach (DataRow dr in dtSoftDentDataToSave.Rows)
                    {
                        //  SqlCetx = conn.BeginTransaction();
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
                                //if (tablename == "Appointment")
                                //{
                                //    Utility.WriteToSyncLogFile_All("Save Records string " + SqlCeCommand.CommandText.ToString());
                                //}                        
                                SqlCeCommand.ExecuteNonQuery();                                
                            }
                        }
                        //   SqlCetx.Commit();
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

        public static bool Save_Tracker_Patient_To_Local_New(DataTable dtSoftDentDataToSave)
        {
            bool _successfullstataus = true;
            string MaritalStatus = string.Empty;
            string Status = string.Empty;
            string tmpBirthDate = string.Empty;
            // decimal curPatientcollect_payment = 0;
            string tmpReceive_Sms_Email = string.Empty;
            try
            {


                SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtSoftDentDataToSave, "0", "1");
            }
            catch (Exception)
            {

            }
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    string sqlSelect = string.Empty;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCeCommand SqlCeComBulk = new SqlCeCommand("", conn))
                    {
                        SqlCeComBulk.CommandType = CommandType.TableDirect;
                        SqlCeComBulk.CommandText = "Patient";
                        SqlCeComBulk.Connection = conn;
                        SqlCeComBulk.IndexName = "unique_Patient_EHR_ID";

                        SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                        foreach (DataRow dr in dtSoftDentDataToSave.Rows)
                        {
                            try
                            {
                                if (dr["Primary_Insurance_CompanyName"].ToString() == "" && dr["Secondary_Insurance_CompanyName"].ToString() == "")
                                {
                                    decimal curPatientcollect_payment = 0;
                                    dr["used_benefit"] = curPatientcollect_payment.ToString();
                                    dr["remaining_benefit"] = curPatientcollect_payment.ToString();
                                }
                                if (dr["InsUptDlt"].ToString() == "")
                                {
                                    dr["InsUptDlt"] = "0";
                                }

                                tmpReceive_Sms_Email = "Y";

                                tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

                                if (tmpBirthDate != "")
                                {
                                    tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
                                }

                                try
                                {
                                    Status = dr["Status"].ToString().Trim();
                                }
                                catch (Exception)
                                { Status = ""; }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                                {
                                    bool found = false;
                                    try
                                    {
                                        found = rs.Seek(DbSeekOptions.FirstEqual, new { PatID = dr["Patient_EHR_ID"].ToString().Trim(), CliNum = "0", ServiceInstalledID = "1" });
                                    }
                                    catch (Exception exFound)
                                    {
                                        if (exFound.Message.ToUpper().Contains("OBJECT MUST IMPLEMENT ICONVERTIBLE"))
                                        {
                                            found = rs.Seek(DbSeekOptions.FirstEqual, new object[] { dr["Patient_EHR_ID"].ToString().Trim(), "0", "1" });
                                        }
                                        else
                                        {
                                            throw exFound;
                                        }
                                    }

                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                                    {
                                        //Insert
                                        if (found == false)
                                        {
                                            SqlCeUpdatableRecord rec = rs.CreateRecord();

                                            rec.SetValue(rs.GetOrdinal("patient_ehr_id"), Convert.ToInt64(dr["patient_ehr_id"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("patient_Web_ID"), "");
                                            rec.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Salutation"), dr["Salutation"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("preferred_name"), dr["preferred_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Status"), Status);
                                            rec.SetValue(rs.GetOrdinal("Sex"), dr["sex"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("MaritalStatus"), dr["MaritalStatus"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Birth_Date"), tmpBirthDate);
                                            rec.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Address1"), dr["Address1"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Address2"), dr["Address2"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("City"), dr["City"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("State"), dr["State"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Zipcode"), dr["Zipcode"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_Status"), "");
                                            rec.SetValue(rs.GetOrdinal("CurrentBal"), "0");
                                            rec.SetValue(rs.GetOrdinal("ThirtyDay"), dr["ThirtyDay"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("SixtyDay"), dr["SixtyDay"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("NinetyDay"), dr["NinetyDay"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Over90"), dr["Over90"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ReceiveSMS"), tmpReceive_Sms_Email);
                                            rec.SetValue(rs.GetOrdinal("ReceiveEmail"), tmpReceive_Sms_Email);
                                            rec.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("due_date"), dr["due_date"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("remaining_benefit"), dr["remaining_benefit"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("used_benefit"), dr["used_benefit"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.GetCurrentDatetimestring());
                                            rec.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rec.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rec.SetValue(rs.GetOrdinal("ssn"), dr["ssn"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("school"), dr["School"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("employer"), dr["Employer"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("emergencycontactId"), dr["emergencycontactId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("EmergencyContact_First_Name"), dr["EmergencyContact_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("EmergencyContact_Last_Name"), dr["EmergencyContact_Last_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("emergencycontactnumber"), dr["emergencycontactnumber"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("responsiblepartyId"), dr["responsiblepartyId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_First_Name"), dr["ResponsibleParty_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_Last_Name"), dr["ResponsibleParty_Last_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("responsiblepartyssn"), dr["responsiblepartyssn"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), Utility.CheckValidDatetime(dr["responsiblepartybirthdate"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("spouseId"), dr["spouseId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Spouse_First_Name"), dr["Spouse_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Spouse_Last_Name"), dr["Spouse_Last_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("driverlicense"), dr["driverlicense"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("groupid"), dr["groupid"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Prim_Ins_Company_Phonenumber"), dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Sec_Ins_Company_Phonenumber"), dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), dr["Secondary_Insurance"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), dr["Primary_Insurance"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Clinic_Number"), "0");
                                            rec.SetValue(rs.GetOrdinal("Service_Install_Id"), "1");
                                            rec.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("PreferredLanguage"), dr["PreferredLanguage"].ToString().Trim());
                                            bool isDeleted = false;
                                            try
                                            {
                                                if (dr.Table.Columns.Contains("Is_Deleted"))
                                                {
                                                    if (dr["Is_Deleted"] == DBNull.Value)
                                                    {
                                                        isDeleted = false;
                                                    }
                                                    else
                                                    {
                                                        isDeleted = Convert.ToBoolean(dr["Is_Deleted"]);
                                                    }
                                                }
                                            }
                                            catch
                                            { }
                                            rec.SetValue(rs.GetOrdinal("Is_Deleted"), isDeleted);
                                            rec.SetValue(rs.GetOrdinal("Patient_Note"), dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());

                                            try
                                            {
                                                rs.Insert(rec);
                                            }
                                            catch (Exception exduplicate)
                                            {
                                                if (exduplicate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                                {
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + "0" + ", Service_Installed_ID:" + "1");
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicate;
                                                }
                                            }
                                        }
                                    }
                                    else if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2 || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                    {
                                        //Update, Delete
                                        if (found == true)
                                        {
                                            rs.Read();

                                            rs.SetValue(rs.GetOrdinal("patient_ehr_id"), Convert.ToInt64(dr["patient_ehr_id"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("patient_Web_ID"), "");
                                            rs.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Salutation"), dr["Salutation"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("preferred_name"), dr["preferred_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Status"), Status);
                                            rs.SetValue(rs.GetOrdinal("Sex"), dr["sex"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("MaritalStatus"), dr["MaritalStatus"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Birth_Date"), tmpBirthDate);
                                            rs.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Address1"), dr["Address1"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Address2"), dr["Address2"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("City"), dr["City"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("State"), dr["State"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Zipcode"), dr["Zipcode"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_Status"), "");
                                            rs.SetValue(rs.GetOrdinal("CurrentBal"), "0");
                                            rs.SetValue(rs.GetOrdinal("ThirtyDay"), dr["ThirtyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("SixtyDay"), dr["SixtyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("NinetyDay"), dr["NinetyDay"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Over90"), dr["Over90"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveSMS"), tmpReceive_Sms_Email);
                                            rs.SetValue(rs.GetOrdinal("ReceiveEmail"), tmpReceive_Sms_Email);
                                            rs.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("due_date"), dr["due_date"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("remaining_benefit"), dr["remaining_benefit"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("used_benefit"), dr["used_benefit"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.GetCurrentDatetimestring());
                                            rs.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rs.SetValue(rs.GetOrdinal("ssn"), dr["ssn"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("school"), dr["School"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("employer"), dr["Employer"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("emergencycontactId"), dr["emergencycontactId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EmergencyContact_First_Name"), dr["EmergencyContact_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EmergencyContact_Last_Name"), dr["EmergencyContact_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("emergencycontactnumber"), dr["emergencycontactnumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartyId"), dr["responsiblepartyId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_First_Name"), dr["ResponsibleParty_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_Last_Name"), dr["ResponsibleParty_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartyssn"), dr["responsiblepartyssn"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), Utility.CheckValidDatetime(dr["responsiblepartybirthdate"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("spouseId"), dr["spouseId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Spouse_First_Name"), dr["Spouse_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Spouse_Last_Name"), dr["Spouse_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("driverlicense"), dr["driverlicense"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("groupid"), dr["groupid"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Prim_Ins_Company_Phonenumber"), dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sec_Ins_Company_Phonenumber"), dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), dr["Secondary_Insurance"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), dr["Primary_Insurance"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Clinic_Number"), "0");
                                            rs.SetValue(rs.GetOrdinal("Service_Install_Id"), "1");
                                            rs.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("PreferredLanguage"), dr["PreferredLanguage"].ToString().Trim());
                                            bool isDeleted = false;
                                            try
                                            {
                                                if (dr.Table.Columns.Contains("Is_Deleted"))
                                                {
                                                    if (dr["Is_Deleted"] == DBNull.Value)
                                                    {
                                                        isDeleted = false;
                                                    }
                                                    else
                                                    {
                                                        isDeleted = Convert.ToBoolean(dr["Is_Deleted"]);
                                                    }
                                                }
                                            }
                                            catch
                                            { }
                                            rs.SetValue(rs.GetOrdinal("Is_Deleted"), Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3 ? true : isDeleted);
                                            rs.SetValue(rs.GetOrdinal("Patient_Note"), dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());

                                            try
                                            {
                                                rs.Update();
                                            }
                                            catch (Exception exduplicateupdate)
                                            {
                                                if (exduplicateupdate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                                {
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + "0" + ", Service_Installed_ID:" + "1");
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicateupdate;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            catch (Exception ex1)
                            {
                                Utility.WriteToErrorLogFromAll("Save_Tracker_To_Local --> Err_Patient Insert For Patient : " + dr["patient_ehr_id"].ToString() + "_" + ex1.Message.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
            }
            return _successfullstataus;
        }

        public static bool Save_Tracker_To_Local(DataTable dtSoftDentDataToSave, string InsertTableName, DataTable dtLocalPatient, bool bSetDeleted = false)
        {
            bool _successfullstataus = true;
            SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtSoftDentDataToSave, "0", "1");
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {

                string sqlSelect = string.Empty;
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                        {
                            SqlCeCommand.CommandText = "Delete from PatientCompare";
                            SqlCeCommand.ExecuteNonQuery();
                        }

                        foreach (DataRow dr in dtSoftDentDataToSave.Rows)
                        {
                            //    DataRow[] row = dtLocalPatient.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");
                            //    if (row.Length > 0)
                            //    {
                            //        if (Utility.DateDiffBetweenTwoDate(dr["Birth_Date"].ToString().Trim(), row[0]["Birth_Date"].ToString().Trim()))
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //        if (Utility.DateDiffBetweenTwoDate(dr["FirstVisit_Date"].ToString().Trim(), row[0]["FirstVisit_Date"].ToString().Trim()))
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //        if (Utility.DateDiffBetweenTwoDate(dr["LastVisit_Date"].ToString().Trim(), row[0]["LastVisit_Date"].ToString().Trim()))
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //        if (Utility.DateDiffBetweenTwoDate(dr["nextvisit_date"].ToString().Trim(), row[0]["nextvisit_date"].ToString().Trim()))
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //        if (row[0]["ReceiveSms"].ToString().Trim() != dr["ReceiveSms"].ToString().Trim())
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //        else if (row[0]["ReceiveEmail"].ToString().Trim() != dr["ReceiveEmail"].ToString().Trim())
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //        else if (dr["Status"].ToString().Trim() != row[0]["Status"].ToString().Trim())
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //        if (dr["due_date"].ToString().Trim() != row[0]["due_date"].ToString().Trim())
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //        if (Convert.ToDecimal(dr["collect_payment"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["collect_payment"].ToString().Trim()).ToString("0.##"))
                            //        {
                            //            dr["InsUptDlt"] = 2;
                            //        }
                            //    }
                            if (dr["Primary_Insurance_CompanyName"].ToString() == "" && dr["Secondary_Insurance_CompanyName"].ToString() == "")
                            {
                                decimal curPatientcollect_payment = 0;
                                dr["used_benefit"] = curPatientcollect_payment.ToString();
                                dr["remaining_benefit"] = curPatientcollect_payment.ToString();
                            }
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                ExecuteQuery(InsertTableName, dr, SqlCeCommand);
                            }
                        }


                        //if (bSetDeleted)
                        //{
                        //    string PatientEHRIDs = string.Join("','", dtSoftDentDataToSave.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));
                        //    if (PatientEHRIDs != string.Empty)
                        //    {
                        //        PatientEHRIDs = "'" + PatientEHRIDs + "'";
                        //        if (conn.State == ConnectionState.Closed) conn.Open();
                        //        string SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                        //        SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", PatientEHRIDs);
                        //        SqlCeCommand.CommandText = SqlCeSelect;
                        //        SqlCeCommand.ExecuteNonQuery();
                        //    }
                        //}

                        if (bSetDeleted)
                        {
                            IEnumerable<string> PatientEHRIDs = dtSoftDentDataToSave.AsEnumerable().Where(sid => sid["Service_Install_Id"].ToString() == "1").Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                            if (PatientEHRIDs != null && PatientEHRIDs.Any())
                            {

                                IEnumerable<string> LocalEHRIDs = dtLocalPatient.AsEnumerable()
                                    .Where(sid => sid["Service_Install_Id"].ToString() == "1")
                                    .Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                                if (LocalEHRIDs != null && LocalEHRIDs.Any())
                                {

                                    string DeletedEHRIDs = string.Join("','", LocalEHRIDs.Except(PatientEHRIDs).ToList());
                                    if (DeletedEHRIDs != string.Empty)
                                    {
                                        DeletedEHRIDs = "'" + DeletedEHRIDs + "'";
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        string SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                                        SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", DeletedEHRIDs);
                                        SqlCeCommand.CommandText = SqlCeSelect;
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }


                    #region Get Records from PatientCompareTAble
                    if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                    {
                        DataTable dtPatientCompareRec = new DataTable();
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        string SqlCeSelect = SynchLocalQRY.PatientCompareQuery;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Clear();

                            SqlCeCommand.Parameters.Add("Service_Install_Id", "1");

                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                dtPatientCompareRec = new DataTable();
                                SqlCeDa.Fill(dtPatientCompareRec);
                            }
                            foreach (DataRow drRow in dtPatientCompareRec.Rows)
                            {
                                ExecuteQuery("Patient", drRow, SqlCeCommand);
                            }

                            //if (conn.State == ConnectionState.Closed) conn.Open();
                            //CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.Add("Service_Install_Id", "1");
                            SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = @Service_Install_Id ";
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }


        public static void ExecuteQuery(string InsertTableName, DataRow dr, SqlCeCommand SqlCeCommand)
        {
            try
            {
                string sqlSelect = string.Empty;
                string MaritalStatus = string.Empty;
                string Status = string.Empty;
                string tmpBirthDate = string.Empty;
                // decimal curPatientcollect_payment = 0;
                string tmpReceive_Sms_Email = string.Empty;

                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                {
                    case 1:
                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
                        if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                        {
                            SqlCeCommand.CommandText = SqlCeCommand.CommandText.Replace("INSERT INTO Patient", "INSERT INTO PatientCompare");
                        }
                        break;
                    case 2:
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
                        break;
                    case 3:
                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
                        break;
                }

                tmpReceive_Sms_Email = "Y";

                tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

                if (tmpBirthDate != "")
                {
                    tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
                }

                try
                {
                    Status = dr["Status"].ToString().Trim();
                }
                catch (Exception)
                { Status = ""; }

                //if (Status == "" || Status == "1")
                //{ Status = "A"; }
                //else
                //{ Status = "I"; }

                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", Convert.ToInt64(dr["patient_ehr_id"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
                SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Status", Status);
                SqlCeCommand.Parameters.AddWithValue("Sex", dr["sex"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("MaritalStatus", dr["MaritalStatus"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Birth_Date", tmpBirthDate);
                SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
                SqlCeCommand.Parameters.AddWithValue("CurrentBal", "0");
                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", tmpReceive_Sms_Email);
                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", tmpReceive_Sms_Email);
                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", dr["remaining_benefit"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("used_benefit", dr["used_benefit"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("collect_payment", dr["collect_payment"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                SqlCeCommand.Parameters.AddWithValue("ssn", dr["ssn"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("school", dr["School"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("employer", dr["Employer"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("emergencycontactId", dr["emergencycontactId"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("EmergencyContact_First_Name", dr["EmergencyContact_First_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("EmergencyContact_Last_Name", dr["EmergencyContact_Last_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("emergencycontactnumber", dr["emergencycontactnumber"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("responsiblepartyId", dr["responsiblepartyId"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_First_Name", dr["ResponsibleParty_First_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Last_Name", dr["ResponsibleParty_Last_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("responsiblepartyssn", dr["responsiblepartyssn"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("responsiblepartybirthdate", Utility.CheckValidDatetime(dr["responsiblepartybirthdate"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("spouseId", dr["spouseId"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Spouse_First_Name", dr["Spouse_First_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Spouse_Last_Name", dr["Spouse_Last_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("driverlicense", dr["driverlicense"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("groupid", dr["groupid"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Prim_Ins_Company_Phonenumber", dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Sec_Ins_Company_Phonenumber", dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", dr["Secondary_Ins_Subscriber_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", dr["Primary_Ins_Subscriber_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                SqlCeCommand.Parameters.AddWithValue("EHR_Status", dr["EHR_Status"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveVoiceCall", dr["ReceiveVoiceCall"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("PreferredLanguage", dr["PreferredLanguage"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                SqlCeCommand.Parameters.AddWithValue("Patient_Note", dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());
                SqlCeCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unique_Patient_EHR_ID"))
                {
                }
                else
                {
                    throw;
                }
            }
        }


        public static bool GetTrackerConnection()
        {

            SqlConnection conn = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            bool IsConnected = false;
            try
            {
                conn.Open();
                IsConnected = true;
            }
            catch (Exception)
            {
                #region Check user and create user if not created

                #endregion

                IsConnected = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsConnected;
        }

        public static DataTable GetTrackerAppointmentData(string strApptID = "")
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerAppointmentData;
                if (!string.IsNullOrEmpty(strApptID))
                {
                    SqlSelect = SqlSelect + " and AP.AppointmentId = '" + strApptID + "'";
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerAppointment_Procedures_Data(string strApptID = "")
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.TrackerAppointment_Procedures_Data;
                if (!string.IsNullOrEmpty(strApptID))
                {
                    SqlSelect = SynchTrackerQRY.TrackerAppointment_Procedures_DataByApptID;
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                if (!string.IsNullOrEmpty(strApptID))
                {
                    SqlCommand.Parameters.Add("@Appt_EHR_ID", SqlDbType.NVarChar).Value = strApptID;
                }
                // SqlCommand.Parameters.Add("@Appt_EHR_ID", Appt_EHR_ID);
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerAppointmentIds()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerAppointmentEhrIds;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        ////public static DataTable GetTrackerDeletedAppointmentData()
        ////{
        ////    DateTime ToDate = Utility.LastSyncDateAditServer;

        ////    SqlConnection conn = null;
        ////    SqlCommand SqlCommand = new SqlCommand();
        ////    SqlDataAdapter SqlDa = null;
        ////    CommonDB.TrackerSQLConnectionServer(ref conn);
        ////    try
        ////    {
        ////        SqlCommand.CommandTimeout = 200;
        ////        if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string SqlSelect = SynchTrackerQRY.GetTrackerDeletedAppointmentData;
        ////        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        ////        SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
        ////        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        ////        DataTable SqlDt = new DataTable();
        ////        SqlDa.Fill(SqlDt);
        ////        return SqlDt;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        if (conn.State == ConnectionState.Open) conn.Close();
        ////    }
        ////}
        ////public static DataTable GetTrackerDeletedOperatoryEventData()
        ////{
        ////    DateTime ToDate = Utility.LastSyncDateAditServer;

        ////    SqlConnection conn = null;
        ////    SqlCommand SqlCommand = new SqlCommand();
        ////    SqlDataAdapter SqlDa = null;
        ////    CommonDB.TrackerSQLConnectionServer(ref conn);
        ////    try
        ////    {
        ////        SqlCommand.CommandTimeout = 200;
        ////        if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string SqlSelect = SynchTrackerQRY.GetTrackerDeletedOperatoryEventData;
        ////        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        ////        SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
        ////        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        ////        DataTable SqlDt = new DataTable();
        ////        SqlDa.Fill(SqlDt);
        ////        return SqlDt;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        if (conn.State == ConnectionState.Open) conn.Close();
        ////    }
        ////}
        public static DataTable GetTrackerOperatoryEventData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string SqlSelect = SynchTrackerQRY.GetTrackerOperatoryEventData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerProviderData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable SqlDt = null;
                try
                {
                    string SqlSelect = SynchTrackerQRY.GetTrackerProviderData_11_29;
                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                    //SqlCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                    SqlDt = new DataTable();
                    SqlDa.Fill(SqlDt);
                }
                catch
                {
                    string SqlSelect = SynchTrackerQRY.GetTrackerProviderData_11_27;
                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                    //SqlCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                    SqlDt = new DataTable();
                    SqlDa.Fill(SqlDt);

                }
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerDefaultProviderData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerDefaultProviderData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerOperatoryData()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerOperatoryData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerSpecialityData()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerSpecialty;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerApptTypeData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerApptTypeData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerPatientData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerPatientData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerPatientDataOfPatientId(string PatientIds)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerPatientData + " and CT.ContactId in(" + PatientIds + ")";
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Patient_Tracker_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            try
            {


                SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtOpenDentalDataToSave, "0", "1");
            }
            catch (Exception)
            {
            }
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    using (SqlCeCommand SqlCeComBulk = new SqlCeCommand("", conn))
                    {
                        SqlCeComBulk.CommandType = CommandType.TableDirect;
                        SqlCeComBulk.CommandText = "Patient";
                        SqlCeComBulk.Connection = conn;
                        SqlCeComBulk.IndexName = "unique_Patient_EHR_ID";

                        SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                        foreach (DataRow dr in dtOpenDentalDataToSave.Rows)
                        {
                            try
                            {
                                if (dr["CurrentBal"].ToString() == "")
                                {
                                    dr["CurrentBal"] = "0";
                                }
                                if (dr["ThirtyDay"].ToString() == "")
                                {
                                    dr["ThirtyDay"] = "0";
                                }
                                if (dr["SixtyDay"].ToString() == "")
                                {
                                    dr["SixtyDay"] = "0";
                                }
                                if (dr["NinetyDay"].ToString() == "")
                                {
                                    dr["NinetyDay"] = "0";
                                }
                                if (dr["Over90"].ToString() == "")
                                {
                                    dr["Over90"] = "0";
                                }

                                object responsiblebirthdte = null;
                                try
                                {
                                    responsiblebirthdte = Convert.ToDateTime(dr["responsiblepartybirthdate"]);
                                }
                                catch (Exception ex)
                                {
                                    responsiblebirthdte = DBNull.Value;
                                }

                                if (dr["InsUptDlt"].ToString() == "")
                                {
                                    dr["InsUptDlt"] = "0";
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                                {
                                    bool found = false;
                                    try
                                    {
                                        found = rs.Seek(DbSeekOptions.FirstEqual, new { PatID = dr["Patient_EHR_ID"].ToString().Trim(), CliNum = Clinic_Number, ServiceInstalledID = Service_Install_Id });
                                    }
                                    catch (Exception exFound)
                                    {
                                        if (exFound.Message.ToUpper().Contains("OBJECT MUST IMPLEMENT ICONVERTIBLE"))
                                        {
                                            found = rs.Seek(DbSeekOptions.FirstEqual, new object[] { dr["Patient_EHR_ID"].ToString().Trim(), Clinic_Number, Service_Install_Id });
                                        }
                                        else
                                        {
                                            throw exFound;
                                        }
                                    }
                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                                    {
                                        //Insert
                                        if (found == false)
                                        {
                                            SqlCeUpdatableRecord rec = rs.CreateRecord();
                                            rec.SetValue(rs.GetOrdinal("patient_ehr_id"), dr["patient_ehr_id"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("patient_Web_ID"), "");
                                            rec.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Salutation"), dr["Salutation"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("preferred_name"), dr["preferred_name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Status"), dr["Status"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Sex"), dr["Sex"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("MaritalStatus"), dr["MaritalStatus"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Birth_Date"), Convert.ToString(dr["Birth_Date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Address1"), dr["Address1"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Address2"), dr["Address2"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("City"), dr["City"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("State"), dr["State"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Zipcode"), dr["Zipcode"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_Status"), "");
                                            rec.SetValue(rs.GetOrdinal("CurrentBal"), Math.Round(double.Parse(dr["CurrentBal"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("ThirtyDay"), Math.Round(double.Parse(dr["ThirtyDay"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("SixtyDay"), Math.Round(double.Parse(dr["SixtyDay"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("NinetyDay"), Math.Round(double.Parse(dr["NinetyDay"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("Over90"), Math.Round(double.Parse(dr["Over90"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString());
                                            rec.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString());
                                            rec.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), dr["Primary_Ins_Subscriber_ID"].ToString());
                                            rec.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString());
                                            rec.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString());
                                            rec.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), dr["Secondary_Ins_Subscriber_ID"].ToString());
                                            rec.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ReceiveEmail"), "Y");
                                            rec.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("due_date"), Convert.ToString(dr["due_date"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("remaining_benefit"), Math.Round(double.Parse(dr["remaining_benefit"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("used_benefit"), Math.Round(double.Parse(dr["used_benefit"].ToString().Trim()), 2));
                                            rec.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(DateTime.Now.ToString()));
                                            rec.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rec.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rec.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
                                            rec.SetValue(rs.GetOrdinal("is_deleted"), Convert.ToBoolean(dr["is_deleted"]));
                                            rec.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("PreferredLanguage"), dr["PreferredLanguage"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Patient_Note"), dr["Patient_Note"].ToString().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).ToString().Trim() : dr["Patient_Note"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ssn"), dr["ssn"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("driverlicense"), dr["driverlicense"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("groupid"), dr["groupid"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("emergencycontactId"), dr["emergencycontactId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("EmergencyContact_First_Name"), dr["EmergencyContact_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("EmergencyContact_Last_Name"), dr["EmergencyContact_Last_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("emergencycontactnumber"), dr["emergencycontactnumber"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("school"), dr["school"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("employer"), dr["employer"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("spouseId"), dr["spouseId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Spouse_First_Name"), dr["Spouse_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Spouse_Last_Name"), dr["Spouse_Last_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("responsiblepartyId"), dr["responsiblepartyId"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("responsiblepartyssn"), dr["responsiblepartyssn"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), responsiblebirthdte);
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_First_Name"), dr["ResponsibleParty_First_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("ResponsibleParty_Last_Name"), dr["ResponsibleParty_Last_Name"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Prim_Ins_Company_Phonenumber"), dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Sec_Ins_Company_Phonenumber"), dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                                            try
                                            {
                                                rs.Insert(rec);
                                            }
                                            catch (Exception exduplicate)
                                            {
                                                if (exduplicate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                                {
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + Clinic_Number + ", Service_Installed_ID:" + Service_Install_Id);
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicate;
                                                }
                                            }
                                        }
                                    }
                                    else if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2 || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                    {
                                        //Update, Delete
                                        if (found == true)
                                        {
                                            rs.Read();
                                            //rs.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Status"), dr["Status"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("ReceiveEmail"), "Y");
                                            //rs.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("due_date"), Convert.ToString(dr["due_date"].ToString().Trim()));
                                            //rs.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            //rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            //rs.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                                            ////rs.SetValue(rs.GetOrdinal("patient_ehr_id"), dr["patient_ehr_id"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("patient_Web_ID"), "");
                                            rs.SetValue(rs.GetOrdinal("First_name"), dr["First_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Last_name"), dr["Last_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Middle_Name"), dr["Middle_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Salutation"), dr["Salutation"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("preferred_name"), dr["preferred_name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Status"), dr["Status"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sex"), dr["Sex"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("MaritalStatus"), dr["MaritalStatus"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Birth_Date"), Convert.ToString(dr["Birth_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Email"), dr["Email"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Address1"), dr["Address1"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Address2"), dr["Address2"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("City"), dr["City"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("State"), dr["State"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Zipcode"), dr["Zipcode"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_Status"), "");
                                            rs.SetValue(rs.GetOrdinal("CurrentBal"), Math.Round(double.Parse(dr["CurrentBal"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("ThirtyDay"), Math.Round(double.Parse(dr["ThirtyDay"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("SixtyDay"), Math.Round(double.Parse(dr["SixtyDay"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("NinetyDay"), Math.Round(double.Parse(dr["NinetyDay"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("Over90"), Math.Round(double.Parse(dr["Over90"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Primary_Insurance"), dr["Primary_Insurance"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Primary_Insurance_CompanyName"), dr["Primary_Insurance_CompanyName"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Primary_Ins_Subscriber_ID"), dr["Primary_Ins_Subscriber_ID"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Insurance"), dr["Secondary_Insurance"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Insurance_CompanyName"), dr["Secondary_Insurance_CompanyName"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Secondary_Ins_Subscriber_ID"), dr["Secondary_Ins_Subscriber_ID"].ToString());
                                            rs.SetValue(rs.GetOrdinal("Guar_ID"), dr["Guar_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Pri_Provider_ID"), dr["Pri_Provider_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sec_Provider_ID"), dr["Sec_Provider_ID"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveSMS"), dr["ReceiveSMS"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveEmail"), "Y");
                                            rs.SetValue(rs.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("due_date"), Convert.ToString(dr["due_date"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("collect_payment"), dr["collect_payment"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("remaining_benefit"), Math.Round(double.Parse(dr["remaining_benefit"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("used_benefit"), Math.Round(double.Parse(dr["used_benefit"].ToString().Trim()), 2));
                                            rs.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(DateTime.Now.ToString()));
                                            rs.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rs.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
                                            rs.SetValue(rs.GetOrdinal("is_deleted"), Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3 ? true : Convert.ToBoolean(dr["is_deleted"]));
                                            rs.SetValue(rs.GetOrdinal("EHR_Status"), dr["EHR_Status"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ReceiveVoiceCall"), dr["ReceiveVoiceCall"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("PreferredLanguage"), dr["PreferredLanguage"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Patient_Note"), dr["Patient_Note"].ToString().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).ToString().Trim() : dr["Patient_Note"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ssn"), dr["ssn"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("driverlicense"), dr["driverlicense"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("groupid"), dr["groupid"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("emergencycontactId"), dr["emergencycontactId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EmergencyContact_First_Name"), dr["EmergencyContact_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("EmergencyContact_Last_Name"), dr["EmergencyContact_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("emergencycontactnumber"), dr["emergencycontactnumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("school"), dr["school"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("employer"), dr["employer"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("spouseId"), dr["spouseId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Spouse_First_Name"), dr["Spouse_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Spouse_Last_Name"), dr["Spouse_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartyId"), dr["responsiblepartyId"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartyssn"), dr["responsiblepartyssn"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), responsiblebirthdte);
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_First_Name"), dr["ResponsibleParty_First_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("ResponsibleParty_Last_Name"), dr["ResponsibleParty_Last_Name"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Prim_Ins_Company_Phonenumber"), dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Sec_Ins_Company_Phonenumber"), dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                                            try
                                            {
                                                rs.Update();
                                            }
                                            catch (Exception exduplicateupdate)
                                            {
                                                if (exduplicateupdate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                                {
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + Clinic_Number + ", Service_Installed_ID:" + Service_Install_Id);
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicateupdate;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                Utility.WriteToErrorLogFromAll("Err_Patient Insert For Patient : " + dr["patient_ehr_id"].ToString() + "_" + ex1.Message.ToString());
                            }
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
        public static DataTable GetTrackerPatientStatusData(string strPatID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerPatientStatusNew_Existing;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    SqlSelect = SqlSelect + " And CT.ContactId = '" + strPatID + "'";
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static DataTable GetTrackerAppointmentsPatientData(string strPatID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerAppointmentsPatientData;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    SqlSelect = SqlSelect + " And CT.ContactId = '" + strPatID + "'";
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerPatientIDsData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerPatientIdsData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerPatientListData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerPatientListData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static void GetTrackerTableColumnList(string tableName, ref DataTable dtTrackerColumns)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);


            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetPatientTableColumnsName;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.Clear();
                //SqlCommand.Parameters.AddWithValue("@TableName", "Contact");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                //DataTable SqlDt = new DataTable();
                SqlDa.Fill(dtTrackerColumns);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        ////public static DataTable GetTrackerPatientInsuranceData(string PatientID)
        ////{
        ////    SqlConnection conn = null;
        ////    SqlCommand SqlCommand = new SqlCommand();
        ////    SqlDataAdapter SqlDa = null;
        ////    CommonDB.TrackerSQLConnectionServer(ref conn);
        ////    try
        ////    {
        ////        SqlCommand.CommandTimeout = 200;
        ////        if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string SqlSelect = SynchTrackerQRY.GetTrackerPatientInsuranceData;
        ////        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        ////        SqlCommand.Parameters.AddWithValue("@PatientId", PatientID);
        ////        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        ////        DataTable SqlDt = new DataTable();
        ////        SqlDa.Fill(SqlDt);
        ////        return SqlDt;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        if (conn.State == ConnectionState.Open) conn.Close();
        ////    }
        ////}

        ////public static DataTable GetTrackerPatientcollect_payment()
        ////{
        ////    DateTime dtCurrentDtTime = Utility.Datetimesetting();
        ////    DateTime ToDate = dtCurrentDtTime;
        ////    SqlConnection conn = null;
        ////    SqlCommand SqlCommand = new SqlCommand();
        ////    SqlDataAdapter SqlDa = null;
        ////    CommonDB.TrackerSQLConnectionServer(ref conn);
        ////    try
        ////    {
        ////        SqlCommand.CommandTimeout = 200;
        ////        if (conn.State == ConnectionState.Closed) conn.Open();

        ////        string SqlSelect = SynchTrackerQRY.GetTrackerPatientcollect_payment;
        ////        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        ////        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        ////        DataTable SqlDt = new DataTable();
        ////        SqlDa.Fill(SqlDt);
        ////        return SqlDt;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        if (conn.State == ConnectionState.Open) conn.Close();
        ////    }
        ////}

        ////public static DataTable GetTrackerPatient_recall()
        ////{
        ////    DateTime dtCurrentDtTime = Utility.Datetimesetting();
        ////    DateTime ToDate = dtCurrentDtTime;
        ////    SqlConnection conn = null;
        ////    SqlCommand SqlCommand = new SqlCommand();
        ////    SqlDataAdapter SqlDa = null;
        ////    CommonDB.TrackerSQLConnectionServer(ref conn);
        ////    try
        ////    {
        ////        SqlCommand.CommandTimeout = 200;
        ////        if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string SqlSelect = SynchTrackerQRY.GetTrackerPatient_recall;
        ////        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        ////        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        ////        DataTable SqlDt = new DataTable();
        ////        SqlDa.Fill(SqlDt);
        ////        return SqlDt;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        if (conn.State == ConnectionState.Open) conn.Close();
        ////    }
        ////}

        //////public static DataTable GetTrackerPatient_RecallType()
        //////{
        //////    DateTime dtCurrentDtTime = Utility.Datetimesetting();
        //////    DateTime ToDate = dtCurrentDtTime;
        //////    SqlConnection conn = null;
        //////    SqlCommand SqlCommand = new SqlCommand();
        //////    SqlDataAdapter SqlDa = null;
        //////    CommonDB.TrackerSQLConnectionServer(ref conn);
        //////    try
        //////    {
        //////        SqlCommand.CommandTimeout = 200;
        //////        if (conn.State == ConnectionState.Closed) conn.Open();
        //////        string SqlSelect = SynchTrackerQRY.GetTrackerRecallType;
        //////        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        //////        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        //////        DataTable SqlDt = new DataTable();
        //////        SqlDa.Fill(SqlDt);
        //////        return SqlDt;
        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        throw ex;
        //////    }
        //////    finally
        //////    {
        //////        if (conn.State == ConnectionState.Open) conn.Close();
        //////    }
        //////}

        public static DataTable GetTrackerRecallTypeData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerRecallType;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerUser()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerUser;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static DataTable GetTrackerApptStatusData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerApptStatusData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }



        public static string GetEHR_VersionNumber()
        {
            string version = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetEHRActualVersionTracker;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                if (SqlDt != null && SqlDt.Rows.Count > 0)
                {
                    version = SqlDt.Rows[0][0].ToString();
                }
                return version;
            }
            catch (Exception ex)
            {
                version = "";
                throw ex;

            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return version;
        }
        //#region SqlCompactConnect

        //public static bool Save_Appointment_Tracker_To_Local(DataTable dtTrackerAppointment)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //if (conn.State == ConnectionState.Closed) conn.Open(); 
        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");

        //        bool is_ehr_updated = false;
        //        string Apptdate = string.Empty;
        //        string ApptTime = string.Empty;
        //        string Mobile_Contact = string.Empty;
        //        string Email = string.Empty;
        //        string Home_Contact = string.Empty;
        //        string Address = string.Empty;
        //        string City = string.Empty;
        //        string State = string.Empty;
        //        string Zipcode = string.Empty;
        //        string patient_first_name = string.Empty;
        //        string patient_last_name = string.Empty;
        //        string patient_mi_name = string.Empty;
        //        string AppointmentStatus = string.Empty;

        //        foreach (DataRow dr in dtTrackerAppointment.Rows)
        //        {
        //            is_ehr_updated = false;

        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }

        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {

        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 5:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 4:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
        //                        break;
        //                }

        //                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
        //                {
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    Mobile_Contact = string.Empty;
        //                    Email = string.Empty;
        //                    Home_Contact = string.Empty;
        //                    Address = string.Empty;
        //                    City = string.Empty;
        //                    State = string.Empty;
        //                    Zipcode = string.Empty;

        //                    Mobile_Contact = dr["Mobile_Contact"].ToString();
        //                    Email = dr["Email"].ToString();
        //                    Home_Contact = dr["Home_Contact"].ToString().Trim();
        //                    Address = dr["Address1"].ToString().Trim();
        //                    City = dr["City"].ToString().Trim();
        //                    State = dr["State"].ToString().Trim();
        //                    Zipcode = dr["Zipcode"].ToString().Trim();

        //                    string birthdate = string.Empty;
        //                    if (!string.IsNullOrEmpty(dr["birth_date"].ToString()))
        //                    {
        //                        birthdate = dr["birth_date"].ToString();
        //                    }

        //                    if (dr["appointment_status_ehr_key"].ToString().Trim() == "7")
        //                    {
        //                        AppointmentStatus = "Completed";
        //                    }
        //                    else if (dr["appointment_status_ehr_key"].ToString().Trim() == "1")
        //                    {
        //                        AppointmentStatus = "Booked";
        //                    }
        //                    else
        //                    {
        //                        AppointmentStatus = dr["Appointment_Status"].ToString().Trim();
        //                    }
        //                    int commentlen = 1999;
        //                    if (dr["comment"].ToString().Trim().Length < commentlen)
        //                    {
        //                        commentlen = dr["comment"].ToString().Trim().Length;
        //                    }
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", string.Empty);
        //                    SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("MI", dr["Middle_Name"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(Home_Contact.ToString().Trim()));
        //                    SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(Mobile_Contact.Trim()));
        //                    SqlCeCommand.Parameters.AddWithValue("Email", Email.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Address", Address.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("City", City.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("ST", State.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Zip", Zipcode.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["ProviderName"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType"].ToString().Trim().Replace(",", " "));
        //                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
        //                    SqlCeCommand.Parameters.AddWithValue("birth_date", birthdate);
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", Convert.ToDateTime(dr["StartTime"].ToString()));
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", Convert.ToDateTime(dr["EndTime"].ToString()));
        //                    SqlCeCommand.Parameters.AddWithValue("Status", "1");
        //                    SqlCeCommand.Parameters.AddWithValue("Patient_Status", "new");
        //                    SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
        //                    SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", "");
        //                    SqlCeCommand.Parameters.AddWithValue("confirmed_status", "");
        //                    SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
        //                    SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
        //                    SqlCeCommand.Parameters.AddWithValue("Is_Appt", "EHR");
        //                    SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
        //                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Convert.ToDateTime(dr["EHR_Entry_DateTime"].ToString()));
        //                    SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
        //                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_OperatoryEvent_Tracker_To_Local(DataTable dtTrackerOperatoryEvent)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //if (conn.State == ConnectionState.Closed) conn.Open(); 
        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");

        //        bool is_ehr_updated = false;

        //        foreach (DataRow dr in dtTrackerOperatoryEvent.Rows)
        //        {
        //            is_ehr_updated = false;

        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }

        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {

        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData;
        //                        is_ehr_updated = false;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryEventData;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
        //                        break;
        //                }

        //                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
        //                {
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    DateTime OE_Date = Convert.ToDateTime(dr["StartTime"].ToString());
        //                    int commentlen = 1999;
        //                    if (dr["comment"].ToString().Trim().Length < commentlen)
        //                    {
        //                        commentlen = dr["comment"].ToString().Trim().Length;
        //                    }
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", string.Empty);
        //                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
        //                    SqlCeCommand.Parameters.AddWithValue("StartTime", Convert.ToDateTime(dr["StartTime"].ToString()));
        //                    SqlCeCommand.Parameters.AddWithValue("EndTime", Convert.ToDateTime(dr["EndTime"].ToString()));
        //                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Convert.ToDateTime(OE_Date));
        //                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_Provider_Tracker_To_Local(DataTable dtTrackerProvider)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        // if (conn.State == ConnectionState.Closed) conn.Open(); 
        //        string sqlSelect = string.Empty;

        //        string Provider_Speciality = "";

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerProvider.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Provider;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Provider;
        //                        break;
        //                }

        //                try
        //                {
        //                    Provider_Speciality = dr["provider_speciality"].ToString();
        //                    if (Provider_Speciality.Length > 10)
        //                    {
        //                        Provider_Speciality = dr["provider_speciality"].ToString().Trim().Substring(12, dr["provider_speciality"].ToString().Trim().Length - 12);
        //                    }

        //                }
        //                catch (Exception)
        //                {
        //                    Provider_Speciality = "";
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Provider_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Last_Name", "");
        //                SqlCeCommand.Parameters.AddWithValue("First_Name", dr["Provider_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("MI", "");
        //                SqlCeCommand.Parameters.AddWithValue("gender", "");
        //                SqlCeCommand.Parameters.AddWithValue("provider_speciality", Provider_Speciality);
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;

        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_Speciality_Tracker_To_Local(DataTable dtTrackerSpeciality)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        // if (conn.State == ConnectionState.Closed) conn.Open();   
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerSpeciality.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
        //            {
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Speciality;
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Speciality_Name", dr["provider_speciality"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }

        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_Operatory_Tracker_To_Local(DataTable dtTrackerOperatory)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //if (conn.State == ConnectionState.Closed) conn.Open();    
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerOperatory.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Operatory;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Operatory;
        //                        break;
        //                }
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }

        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_Patient_Tracker_To_Local(DataTable dtTrackerPatient)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        // if (conn.State == ConnectionState.Closed) conn.Open();   
        //        string sqlSelect = string.Empty;
        //        string MaritalStatus = string.Empty;
        //        string Status = string.Empty;
        //        string tmpBirthDate = string.Empty;
        //        // decimal curPatientcollect_payment = 0;
        //        string tmpReceive_Sms_Email = string.Empty;
        //        int tmpprivacy_flags = 0;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerPatient.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {

        //                tmpReceive_Sms_Email = "Y";
        //                tmpprivacy_flags = Convert.ToInt32(dr["privacy_flags"].ToString());
        //                if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
        //                {
        //                    tmpReceive_Sms_Email = "N";
        //                }

        //                tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

        //                if (tmpBirthDate != "")
        //                {
        //                    tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
        //                }

        //                try
        //                {
        //                    Status = dr["Status"].ToString().Trim();
        //                }
        //                catch (Exception)
        //                { Status = ""; }

        //                if (Status == "" || Status == "1")
        //                { Status = "A"; }
        //                else
        //                { Status = "I"; }

        //                //try
        //                //{
        //                //    curPatientcollect_payment = 0;

        //                //    if (dr["Patientcollect_payment"].ToString().Trim() != "" && dr["Patientcollect_payment"].ToString().Trim() != "0")
        //                //    {
        //                //        curPatientcollect_payment = Convert.ToDecimal(dr["Patientcollect_payment"].ToString().Trim());
        //                //        curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
        //                //        curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
        //                //    }
        //                //    else
        //                //    {
        //                //        curPatientcollect_payment = 0;
        //                //    }
        //                //}
        //                //catch (Exception)
        //                //{
        //                //    curPatientcollect_payment = 0;
        //                //}


        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Status", Status);
        //                SqlCeCommand.Parameters.AddWithValue("Sex", "Unknown");
        //                SqlCeCommand.Parameters.AddWithValue("MaritalStatus", "Single");
        //                SqlCeCommand.Parameters.AddWithValue("Birth_Date", tmpBirthDate);
        //                SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
        //                SqlCeCommand.Parameters.AddWithValue("CurrentBal", "0");
        //                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", tmpReceive_Sms_Email);
        //                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", tmpReceive_Sms_Email);
        //                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", dr["remaining_benefit"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("used_benefit", dr["used_benefit"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("collect_payment", dr["collect_payment"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }

        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_ApptType_Tracker_To_Local(DataTable dtTrackerApptType)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //  if (conn.State == ConnectionState.Closed) conn.Open();
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerApptType.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptType;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ApptType;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ApptType_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        SqlCetx.Commit();

        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_ApptStatus_Tracker_To_Local(DataTable dtTrackerApptStatus)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerApptStatus.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_AppointmentStatus;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_AppointmentStatus;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_AppointmentStatus;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("ApptStatus_EHR_ID", dr["ApptStatus_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Name", dr["ApptStatus_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;

        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_RecallType_Tracker_To_Local(DataTable dtTrackerRecallType)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerRecallType.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_RecallType;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_RecallType;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_RecallType;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("RecallType_EHR_ID", dr["RecallType_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("RecallType_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("RecallType_Name", dr["RecallType_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("RecallType_Descript", dr["RecallType_Descript"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;

        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Update_DeletedAppointment_Tracker_To_Local(DataTable dtTrackerDeletedAppointment)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        string AppointmentStatus = string.Empty;
        //        foreach (DataRow dr in dtTrackerDeletedAppointment.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }

        //            if (dr["InsUptDlt"].ToString() == "1")
        //            {
        //                SqlCeCommand.CommandText = SynchLocalQRY.InsertAppointment_With_DeleteFlag;
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appointment_id"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //            else if (dr["InsUptDlt"].ToString() == "2")
        //            {
        //                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appointment_id"].ToString().Trim());
        //                SqlCeCommand.ExecuteNonQuery();
        //            }

        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Update_DeletedOperatoryEvent_Tracker_To_Local(DataTable dtTrackerDeletedOperatoryEvent)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        string AppointmentStatus = string.Empty;
        //        foreach (DataRow dr in dtTrackerDeletedOperatoryEvent.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }

        //            if (dr["InsUptDlt"].ToString() == "1")
        //            {
        //                SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData_With_DeleteFlag;
        //                SqlCeCommand.Parameters.Clear();
        //                DateTime OE_Date = Convert.ToDateTime(dr["StartTime"].ToString());
        //                int commentlen = 1999;
        //                if (dr["comment"].ToString().Trim().Length < commentlen)
        //                {
        //                    commentlen = dr["comment"].ToString().Trim().Length;
        //                }
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", string.Empty);
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
        //                SqlCeCommand.Parameters.AddWithValue("StartTime", Convert.ToDateTime(dr["StartTime"].ToString()));
        //                SqlCeCommand.Parameters.AddWithValue("EndTime", Convert.ToDateTime(dr["EndTime"].ToString()));
        //                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Convert.ToDateTime(OE_Date));
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //            else if (dr["InsUptDlt"].ToString() == "2")
        //            {
        //                SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.ExecuteNonQuery();
        //            }

        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //#endregion

        //#region SqlServer

        //public static bool Save_Appointment_Tracker_To_Local_SqlServer(DataTable dtTrackerAppointment)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //if (conn.State == ConnectionState.Closed) conn.Open(); 
        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

        //        bool is_ehr_updated = false;
        //        string Apptdate = string.Empty;
        //        string ApptTime = string.Empty;
        //        string Mobile_Contact = string.Empty;
        //        string Email = string.Empty;
        //        string Home_Contact = string.Empty;
        //        string Address = string.Empty;
        //        string City = string.Empty;
        //        string State = string.Empty;
        //        string Zipcode = string.Empty;
        //        string patient_first_name = string.Empty;
        //        string patient_last_name = string.Empty;
        //        string patient_mi_name = string.Empty;
        //        string AppointmentStatus = string.Empty;

        //        foreach (DataRow dr in dtTrackerAppointment.Rows)
        //        {
        //            is_ehr_updated = false;

        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }

        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {

        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;
        //                        is_ehr_updated = false;
        //                        break;
        //                    case 5:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 4:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
        //                        break;
        //                }

        //                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
        //                {
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    string[] patientinfo = dr["patient_name"].ToString().Split(',');
        //                    patient_last_name = patientinfo[0].ToString().Trim();
        //                    string[] patienfirstMI = patientinfo[1].ToString().Trim().Split(' ');
        //                    if (patienfirstMI.Length == 2)
        //                    {
        //                        patient_first_name = patienfirstMI[0].ToString().Trim();
        //                        patient_mi_name = patienfirstMI[1].ToString().Trim();
        //                    }
        //                    else
        //                    {
        //                        patient_first_name = patienfirstMI[0].ToString().Trim();
        //                        patient_mi_name = string.Empty;
        //                    }

        //                    Apptdate = Convert.ToDateTime(dr["appointment_date"].ToString()).ToString("dd/MM/yyyy");
        //                    ApptTime = Convert.ToInt32(dr["start_hour"].ToString()).ToString("00") + ":" + Convert.ToInt32(dr["start_minute"].ToString()).ToString("00");

        //                    Mobile_Contact = string.Empty;
        //                    Email = string.Empty;
        //                    Home_Contact = string.Empty;
        //                    Address = string.Empty;
        //                    City = string.Empty;
        //                    State = string.Empty;
        //                    Zipcode = string.Empty;

        //                    if (dr["patId"].ToString() == "0" || dr["patId"].ToString() == string.Empty)
        //                    {

        //                        if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
        //                        {
        //                            Mobile_Contact = Utility.ConvertContactNumber(dr["Phone"].ToString().Trim());
        //                            Email = "";
        //                        }
        //                        else
        //                        {
        //                            string MobileEmail = dr["notetext"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace("\n", "");
        //                            Mobile_Contact = string.Empty;
        //                            Email = string.Empty;

        //                            if (MobileEmail != string.Empty)
        //                            {
        //                                Mobile_Contact = MobileEmail.Substring(0, 10);
        //                                Email = MobileEmail.Substring(10, MobileEmail.Length - 10);

        //                                try
        //                                {
        //                                    double isMobile = Convert.ToDouble(Mobile_Contact);
        //                                }
        //                                catch (FormatException)
        //                                {
        //                                    Mobile_Contact = string.Empty;
        //                                    Email = MobileEmail.ToString();
        //                                }

        //                            }
        //                            else
        //                            {
        //                                Mobile_Contact = dr["patMobile"].ToString();
        //                                Email = dr["patEmail"].ToString();
        //                            }
        //                        }

        //                        Home_Contact = dr["patient_phone"].ToString().Trim();
        //                        Address = dr["street1"].ToString().Trim();
        //                        City = dr["city"].ToString().Trim();
        //                        State = dr["state"].ToString().Trim();
        //                        Zipcode = dr["zipcode"].ToString().Trim();
        //                    }
        //                    else
        //                    {
        //                        Mobile_Contact = dr["patMobile"].ToString();
        //                        Email = dr["patEmail"].ToString();
        //                        Home_Contact = dr["patHomephone"].ToString().Trim();
        //                        Address = dr["patAddress"].ToString().Trim();
        //                        City = dr["patCity"].ToString().Trim();
        //                        State = dr["patState"].ToString().Trim();
        //                        Zipcode = dr["patZipcode"].ToString().Trim();
        //                    }

        //                    DateTime ApptDateTime = DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        //                    DateTime ApptEndDateTime = ApptDateTime.AddMinutes(Convert.ToInt32(dr["length"].ToString().Trim()));

        //                    string birthdate = string.Empty;
        //                    if (!string.IsNullOrEmpty(dr["birth_date"].ToString()))
        //                    {
        //                        birthdate = dr["birth_date"].ToString();
        //                    }

        //                    if (dr["appointment_status_ehr_key"].ToString().Trim() == "-106")
        //                    {
        //                        AppointmentStatus = "completed";
        //                    }
        //                    else if (dr["appointment_status_ehr_key"].ToString().Trim() == "0")
        //                    {
        //                        AppointmentStatus = "none";
        //                    }
        //                    else
        //                    {
        //                        AppointmentStatus = dr["Appointment_Status"].ToString().Trim();
        //                    }


        //                    int commentlen = 1999;
        //                    if (dr["comment"].ToString().Trim().Length < commentlen)
        //                    {
        //                        commentlen = dr["comment"].ToString().Trim().Length;
        //                    }

        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", string.Empty);
        //                    SqlCeCommand.Parameters.AddWithValue("Last_Name", patient_last_name.Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("First_Name", patient_first_name.Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("MI", patient_mi_name.Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(Home_Contact.ToString().Trim()));
        //                    SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(Mobile_Contact.Trim()));
        //                    SqlCeCommand.Parameters.AddWithValue("Email", Email.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Address", Address.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("City", City.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("ST", State.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Zip", Zipcode.ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["op_id"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["op_title"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["provider_id"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["provider_last_name"].ToString().Trim() + " " + dr["provider_first_name"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType_Name"].ToString().Trim().Replace(",", " "));
        //                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
        //                    SqlCeCommand.Parameters.AddWithValue("birth_date", birthdate);
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", ApptDateTime.ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", ApptEndDateTime.ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("Status", "1");
        //                    SqlCeCommand.Parameters.AddWithValue("Patient_Status", "new");
        //                    SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
        //                    SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", "");
        //                    SqlCeCommand.Parameters.AddWithValue("confirmed_status", "");
        //                    SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
        //                    SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
        //                    SqlCeCommand.Parameters.AddWithValue("Is_Appt", "EHR");
        //                    SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
        //                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
        //                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                    SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patId"].ToString().Trim());
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        // throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_OperatoryEvent_Tracker_To_Local_SqlServer(DataTable dtTrackerOperatoryEvent)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //if (conn.State == ConnectionState.Closed) conn.Open(); 
        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

        //        bool is_ehr_updated = false;

        //        foreach (DataRow dr in dtTrackerOperatoryEvent.Rows)
        //        {
        //            is_ehr_updated = false;

        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }

        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {

        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData;
        //                        is_ehr_updated = false;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryEventData;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
        //                        break;
        //                }

        //                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
        //                {
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["event_id"].ToString().Trim());
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    DateTime OE_Date = Convert.ToDateTime(dr["event_date"].ToString());

        //                    int OE_StartHour = Convert.ToInt32(dr["start_time"].ToString()) / 12;
        //                    int OE_StartMin = (Convert.ToInt32(dr["start_time"].ToString()) % 12) * 5;

        //                    int OE_EndHour = Convert.ToInt32(dr["end_time"].ToString()) / 12;
        //                    int OE_EndMin = (Convert.ToInt32(dr["end_time"].ToString()) % 12) * 5;


        //                    //DateTime StartTime = Convert.ToDateTime(OE_Date.ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(Convert.ToDateTime(OE_StartHour.ToString()).ToString("HH") + ":" + Convert.ToDateTime(OE_StartMin.ToString()).ToString("mm")).ToString("HH:mm"));
        //                    //DateTime EndTime = Convert.ToDateTime(OE_Date.ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(Convert.ToDateTime(OE_EndHour.ToString()).ToString("HH") + ":" + Convert.ToDateTime(OE_EndMin.ToString()).ToString("mm")).ToString("HH: mm"));

        //                    DateTime StartTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToString("MM/dd/yyyy") + " " + OE_StartHour.ToString() + ":" + OE_StartMin.ToString()).ToString("MM/dd/yyyy HH:mm"));
        //                    DateTime EndTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToString("MM/dd/yyyy") + " " + OE_EndHour.ToString() + ":" + OE_EndMin.ToString()).ToString("MM/dd/yyyy HH:mm"));



        //                    int commentlen = 1999;
        //                    if (dr["note"].ToString().Trim().Length < commentlen)
        //                    {
        //                        commentlen = dr["note"].ToString().Trim().Length;
        //                    }

        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["event_id"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", string.Empty);
        //                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["operatory_id"].ToString().Trim());
        //                    SqlCeCommand.Parameters.AddWithValue("comment", dr["note"].ToString().Trim().Substring(0, commentlen));
        //                    SqlCeCommand.Parameters.AddWithValue("StartTime", StartTime.ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("EndTime", EndTime.ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        // throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_Provider_Tracker_To_Local_SqlServer(DataTable dtTrackerProvider)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        // if (conn.State == ConnectionState.Closed) conn.Open(); 
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerProvider.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Provider;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Provider;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Provider_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("MI", dr["MI"].ToString());
        //                SqlCeCommand.Parameters.AddWithValue("gender", "");
        //                SqlCeCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString().Trim().Substring(12, dr["provider_speciality"].ToString().Trim().Length - 12));
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;

        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_Speciality_Tracker_To_Local_SqlServer(DataTable dtTrackerSpeciality)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        // if (conn.State == ConnectionState.Closed) conn.Open();   
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerSpeciality.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
        //            {
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Speciality;
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Speciality_Name", dr["provider_speciality"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }

        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_Operatory_Tracker_To_Local_SqlServer(DataTable dtTrackerOperatory)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //if (conn.State == ConnectionState.Closed) conn.Open();    
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerOperatory.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Operatory;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Operatory;
        //                        break;
        //                }
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }

        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_Patient_Tracker_To_Local_SqlServer(DataTable dtTrackerPatient)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        // if (conn.State == ConnectionState.Closed) conn.Open();   
        //        string sqlSelect = string.Empty;
        //        string MaritalStatus = string.Empty;
        //        string Status = string.Empty;
        //        string tmpBirthDate = string.Empty;

        //        string tmpReceive_Sms_Email = string.Empty;
        //        int tmpprivacy_flags = 0;

        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerPatient.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {

        //                tmpReceive_Sms_Email = "Y";
        //                tmpprivacy_flags = Convert.ToInt32(dr["privacy_flags"].ToString());
        //                if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
        //                {
        //                    tmpReceive_Sms_Email = "N";
        //                }

        //                tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

        //                if (tmpBirthDate != "")
        //                {
        //                    tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
        //                }

        //                try
        //                {
        //                    Status = dr["Status"].ToString().Trim();
        //                }
        //                catch (Exception)
        //                { Status = ""; }

        //                if (Status == "" || Status == "1")
        //                { Status = "A"; }
        //                else
        //                { Status = "I"; }


        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Status", Status);
        //                SqlCeCommand.Parameters.AddWithValue("Sex", "Unknown");
        //                SqlCeCommand.Parameters.AddWithValue("MaritalStatus", "Single");
        //                SqlCeCommand.Parameters.AddWithValue("Birth_Date", tmpBirthDate);
        //                SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dr["Mobile"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dr["Home_Phone"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dr["Work_Phone"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Address1", dr["Address1"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("State", dr["State"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["Zipcode"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
        //                SqlCeCommand.Parameters.AddWithValue("CurrentBal", "");
        //                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", tmpReceive_Sms_Email);
        //                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", tmpReceive_Sms_Email);
        //                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
        //                SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", "");
        //                SqlCeCommand.Parameters.AddWithValue("collect_payment", "");
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }

        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_ApptType_Tracker_To_Local_SqlServer(DataTable dtTrackerApptType)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        //  if (conn.State == ConnectionState.Closed) conn.Open();  
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerApptType.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptType;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ApptType;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ApptType_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        SqlCetx.Commit();

        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_ApptStatus_Tracker_To_Local_SqlServer(DataTable dtTrackerApptStatus)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerApptStatus.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_AppointmentStatus;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_AppointmentStatus;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_AppointmentStatus;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("ApptStatus_EHR_ID", dr["ApptStatus_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Name", dr["ApptStatus_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;

        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Save_RecallType_Tracker_To_Local_SqlServer(DataTable dtTrackerRecallType)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        foreach (DataRow dr in dtTrackerRecallType.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        //            {
        //                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        //                {
        //                    case 1:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_RecallType;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_RecallType;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_RecallType;
        //                        break;
        //                }

        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("RecallType_EHR_ID", dr["RecallType_EHR_ID"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("RecallType_Web_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("RecallType_Name", dr["RecallType_Name"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("RecallType_Descript", dr["RecallType_Descript"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }
        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;

        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static bool Update_DeletedAppointment_Tracker_To_Local_SqlServer(DataTable dtTrackerDeletedAppointment)
        //{
        //    bool _successfullstataus = true;
        //    SqlConnection conn = null;
        //    SqlCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer_SqlServer(ref conn);

        //    SqlTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        string AppointmentStatus = string.Empty;
        //        foreach (DataRow dr in dtTrackerDeletedAppointment.Rows)
        //        {
        //            if (dr["InsUptDlt"].ToString() == "")
        //            {
        //                dr["InsUptDlt"] = "0";
        //            }
        //            if (dr["InsUptDlt"].ToString() == "2")
        //            {
        //                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
        //                SqlCeCommand.Parameters.Clear();
        //                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appointment_id"].ToString().Trim());
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", "");
        //                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", "");
        //                SqlCeCommand.Parameters.AddWithValue("Provider_Name", "");
        //                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
        //                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        //                SqlCeCommand.ExecuteNonQuery();
        //            }

        //        }
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        // throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //#endregion

        ////#region  Holidays

        public static DataTable GetTrackerHolidaysData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable SqlDt = null;
                try
                {
                    string SqlSelect = SynchTrackerQRY.GetTrackerHolidayData_11_29;
                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    //SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                    SqlDt = new DataTable();
                    SqlDa.Fill(SqlDt);
                }
                catch
                {
                    string SqlSelect = SynchTrackerQRY.GetTrackerHolidayData_11_27;
                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    // SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                    SqlDt = new DataTable();
                    SqlDa.Fill(SqlDt);

                }
                //SqlDt.DefaultView.RowFilter = "closed_flag in (2,3,4)";
                return SqlDt.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        //////public static DataTable GetTrackerOperatoryHolidaysData(DataTable dtOperatory)
        //////{
        //////    SqlConnection conn = null;
        //////    CommonDB.TrackerSQLConnectionServer(ref conn);

        //////    DateTime ToDate = Utility.LastSyncDateAditServer;
        //////    try
        //////    {
        //////        DataTable SqlDt = new DataTable();
        //////        foreach (DataRow dr in dtOperatory.Rows)
        //////        {
        //////            SqlCommand SqlCommand = new SqlCommand();
        //////            SqlDataAdapter SqlDa = null;
        //////            //  MySqlCommand.CommandTimeout = 120;
        //////            SqlCommand.CommandTimeout = 200;
        //////            if (conn.State == ConnectionState.Closed) conn.Open();
        //////            string SqlSelect = SynchTrackerQRY.GetTrackerOperatoryHolidaysData;
        //////            CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        //////            SqlCommand.Parameters.AddWithValue("@H_Operatory_EHR_ID", dr["operatory_ehr_id"].ToString());
        //////            SqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
        //////            SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
        //////            CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        //////            DataTable dt = new DataTable();
        //////            SqlDa.Fill(dt);
        //////            if (SqlDt.Rows.Count > 0)
        //////            {
        //////                SqlDt.Merge(dt);
        //////            }
        //////            else
        //////            {
        //////                SqlDt = dt;
        //////            }

        //////        }
        //////        return SqlDt;
        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        throw ex;
        //////    }
        //////    finally
        //////    {
        //////        if (conn.State == ConnectionState.Open) conn.Close();
        //////    }
        //////}

        ////public static bool Save_Holidays_Tracker_To_Local(DataTable dtEaglesoftHoliday)
        ////{
        ////    bool _successfullstataus = true;
        ////    SqlCeConnection conn = null;
        ////    SqlCeCommand SqlCeCommand = null;
        ////    CommonDB.LocalConnectionServer(ref conn);

        ////    SqlCeTransaction SqlCetx;
        ////    if (conn.State == ConnectionState.Closed) conn.Open();
        ////    SqlCetx = conn.BeginTransaction();
        ////    try
        ////    {
        ////        //if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string sqlSelect = string.Empty;

        ////        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        ////        bool is_ehr_updated = false;
        ////        string AppointmentStatus = string.Empty;
        ////        foreach (DataRow dr in dtEaglesoftHoliday.Rows)
        ////        {
        ////            is_ehr_updated = false;
        ////            if (dr["InsUptDlt"].ToString() == "")
        ////            {
        ////                dr["InsUptDlt"] = "0";
        ////            }
        ////            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        ////            {
        ////                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        ////                {
        ////                    case 1:
        ////                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_HolidayData;
        ////                        is_ehr_updated = true;
        ////                        break;
        ////                    case 2:
        ////                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Tracker_HolidayData;
        ////                        is_ehr_updated = true;
        ////                        break;
        ////                    case 3:
        ////                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Tracker_HolidayData;
        ////                        break;
        ////                }

        ////                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
        ////                {
        ////                    SqlCeCommand.Parameters.Clear();
        ////                    SqlCeCommand.Parameters.AddWithValue("SchedDate", dr["Sched_exception_date"].ToString().Trim());
        ////                    SqlCeCommand.ExecuteNonQuery();
        ////                }
        ////                else
        ////                {

        ////                    int commentlen = 1999;
        ////                    if (dr["practice_name"].ToString().Trim().Length < commentlen)
        ////                    {
        ////                        commentlen = dr["practice_name"].ToString().Trim().Length;
        ////                    }
        ////                    SqlCeCommand.Parameters.Clear();
        ////                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", "");
        ////                    SqlCeCommand.Parameters.AddWithValue("H_Web_ID", "");
        ////                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", "");
        ////                    SqlCeCommand.Parameters.AddWithValue("comment", dr["practice_name"].ToString().Trim().Substring(0, commentlen));
        ////                    SqlCeCommand.Parameters.AddWithValue("SchedDate", Utility.CheckValidDatetime(dr["Sched_exception_date"].ToString()));
        ////                    SqlCeCommand.Parameters.AddWithValue("StartTime_1", dr["start_time1"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("EndTime_1", dr["end_time1"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("StartTime_2", dr["start_time2"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("EndTime_2", dr["end_time2"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("StartTime_3", dr["start_time3"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("EndTime_3", dr["end_time3"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        ////                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
        ////                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        ////                    SqlCeCommand.ExecuteNonQuery();
        ////                }
        ////            }
        ////        }

        ////        SqlCetx.Commit();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _successfullstataus = false;
        ////        SqlCetx.Rollback();
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        if (conn.State == ConnectionState.Open) conn.Close();
        ////    }
        ////    return _successfullstataus;
        ////}

        ////public static bool Save_Opeatory_Holidays_Tracker_To_Local(DataTable dtEaglesoftHoliday)
        ////{
        ////    bool _successfullstataus = true;
        ////    SqlCeConnection conn = null;
        ////    SqlCeCommand SqlCeCommand = null;
        ////    CommonDB.LocalConnectionServer(ref conn);

        ////    SqlCeTransaction SqlCetx;
        ////    if (conn.State == ConnectionState.Closed) conn.Open();
        ////    SqlCetx = conn.BeginTransaction();
        ////    try
        ////    {
        ////        //if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string sqlSelect = string.Empty;

        ////        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        ////        bool is_ehr_updated = false;
        ////        string AppointmentStatus = string.Empty;
        ////        foreach (DataRow dr in dtEaglesoftHoliday.Rows)
        ////        {
        ////            is_ehr_updated = false;
        ////            if (dr["InsUptDlt"].ToString() == "")
        ////            {
        ////                dr["InsUptDlt"] = "0";
        ////            }
        ////            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
        ////            {
        ////                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
        ////                {
        ////                    case 1:
        ////                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_HolidayData;
        ////                        is_ehr_updated = true;
        ////                        break;
        ////                    case 2:
        ////                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Tracker_Operatory_HolidayData;
        ////                        is_ehr_updated = true;
        ////                        break;
        ////                    case 3:
        ////                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Tracker_Operatory_HolidayData;
        ////                        break;
        ////                }

        ////                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
        ////                {
        ////                    SqlCeCommand.Parameters.Clear();
        ////                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", dr["op_id"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("SchedDate", dr["Sched_exception_date"].ToString().Trim());
        ////                    SqlCeCommand.ExecuteNonQuery();
        ////                }
        ////                else
        ////                {
        ////                    int commentlen = 1999;
        ////                    if (dr["op_title"].ToString().Trim().Length < commentlen)
        ////                    {
        ////                        commentlen = dr["op_title"].ToString().Trim().Length;
        ////                    }
        ////                    SqlCeCommand.Parameters.Clear();
        ////                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", "");
        ////                    SqlCeCommand.Parameters.AddWithValue("H_Web_ID", "");
        ////                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", dr["op_id"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("comment", dr["op_title"].ToString().Trim().Substring(0, commentlen));
        ////                    SqlCeCommand.Parameters.AddWithValue("SchedDate", Utility.CheckValidDatetime(dr["Sched_exception_date"].ToString()));
        ////                    SqlCeCommand.Parameters.AddWithValue("StartTime_1", dr["start_time1"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("EndTime_1", dr["end_time1"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("StartTime_2", dr["start_time2"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("EndTime_2", dr["end_time2"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("StartTime_3", dr["start_time3"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("EndTime_3", dr["end_time3"].ToString());
        ////                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        ////                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
        ////                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
        ////                    SqlCeCommand.ExecuteNonQuery();
        ////                }
        ////            }
        ////        }

        ////        SqlCetx.Commit();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _successfullstataus = false;
        ////        SqlCetx.Rollback();
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        if (conn.State == ConnectionState.Open) conn.Close();
        ////    }
        ////    return _successfullstataus;
        ////}


        ////#endregion

        //#region Create Appointment

        //public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    SqlCeTransaction SqlCetx;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    SqlCetx = conn.BeginTransaction();
        //    try
        //    {
        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
        //        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_EHR_ID;
        //        SqlCeCommand.Parameters.Clear();
        //        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", tmpAppt_EHR_id);
        //        SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", tmpAppt_Web_id);
        //        SqlCeCommand.ExecuteNonQuery();
        //        SqlCetx.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        SqlCetx.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}

        //public static int Save_Patient_Local_To_Tracker(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, int tmpPatient_Gur_id)
        //{
        //    int PatientId = 0;
        //    int FamilyId = 0;
        //    SqlConnection conn = null;
        //    //MySqlCommand MySqlCommand = new MySqlCommand();
        //    SqlCommand SqlCommand = null;
        //    CommonDB.TrackerSQLConnectionServer(ref conn);

        //    if (conn.State == ConnectionState.Closed) conn.Open();

        //    try
        //    {

        //        string sqlSelect = string.Empty;
        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
        //        SqlCommand.CommandText = SynchTrackerQRY.InsertPatientDetails;
        //        SqlCommand.Parameters.Clear();
        //        SqlCommand.Parameters.AddWithValue("@lastname", LastName);
        //        SqlCommand.Parameters.AddWithValue("@firstname", FirstName);
        //        SqlCommand.Parameters.AddWithValue("@mi", MiddleName);
        //        SqlCommand.Parameters.AddWithValue("@provid1", PriProv);
        //        SqlCommand.Parameters.AddWithValue("@emailaddr", Email);
        //        SqlCommand.Parameters.AddWithValue("@pager", Mobile);

        //        SqlCommand.ExecuteNonQuery();
        //        string QryIdentity = "Select max(fld_auto_intPatId) as newId from tbl_PatInfo";//"Select @@Identity as newId from patient";
        //        SqlCommand.CommandText = QryIdentity;
        //        SqlCommand.CommandType = CommandType.Text;
        //        SqlCommand.Connection = conn;
        //        PatientId = Convert.ToInt32(SqlCommand.ExecuteScalar());
        //        if (tmpPatient_Gur_id == 0)
        //        {
        //            SqlCommand.CommandText = SynchTrackerQRY.InsertPatientGuarantorID;
        //            SqlCommand.Parameters.Clear();
        //            SqlCommand.Parameters.AddWithValue("@fld_intHeadPatId", PatientId);
        //            SqlCommand.ExecuteNonQuery();
        //            QryIdentity = "Select max([fld_auto_intFamId]) as newId from [tbl_PatFam]";//"Select @@Identity as newId from patient";
        //            SqlCommand.CommandText = QryIdentity;
        //            SqlCommand.CommandType = CommandType.Text;
        //            SqlCommand.Connection = conn;
        //            FamilyId = Convert.ToInt32(SqlCommand.ExecuteScalar());
        //        }
        //        else
        //        {
        //            FamilyId = tmpPatient_Gur_id;
        //        }
        //        SqlCommand.CommandText = SynchTrackerQRY.UpdatePatientGuarantorID;
        //        SqlCommand.Parameters.Clear();
        //        SqlCommand.Parameters.AddWithValue("@famid", FamilyId);
        //        SqlCommand.Parameters.AddWithValue("@patid", PatientId);
        //        SqlCommand.ExecuteNonQuery();

        //    }
        //    catch (Exception ex)
        //    {
        //        PatientId = 0;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return PatientId;
        //}

        //public static int Save_Appointment_Local_To_Tracker(string patid, int length, string opid, string provid, DateTime StartTime, DateTime EndTime,
        //                                                         string createdate, string appttypeid, string PatientName)
        //{
        //    int Appointment_Id = 0;
        //    //SqlConnection conn = null;
        //    ////MySqlCommand MySqlCommand = new MySqlCommand();
        //    //SqlCommand SqlCommand = null;
        //    //CommonDB.TrackerSQLConnectionServer(ref conn);
        //    //if (conn.State == ConnectionState.Closed) conn.Open();
        //    //try
        //    //{
        //    //    string sqlSelect = string.Empty;
        //    //    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
        //    //    SqlCommand.CommandText = SynchTrackerQRY.InsertAppointmentDetails;
        //    //    SqlCommand.Parameters.Clear();
        //    //    SqlCommand.Parameters.AddWithValue("@patid", patid);
        //    //    SqlCommand.Parameters.AddWithValue("@provid", provid);
        //    //    SqlCommand.Parameters.AddWithValue("@opid", opid);
        //    //    SqlCommand.Parameters.AddWithValue("@appttype", appttypeid);
        //    //    SqlCommand.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
        //    //    SqlCommand.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
        //    //    // SqlCommand.Parameters.AddWithValue("@Confirmed", Confirmed);
        //    //    SqlCommand.Parameters.AddWithValue("@createdate", Convert.ToDateTime(createdate));
        //    //    // SqlCommand.Parameters.AddWithValue("IsNewPatient", IsNewPatient);            
        //    //    SqlCommand.ExecuteNonQuery();

        //    //    string QryIdentity = "Select max(fld_auto_intAppId) as newId from tbl_SchApp";
        //    //    SqlCommand.CommandText = QryIdentity;
        //    //    SqlCommand.CommandType = CommandType.Text;
        //    //    SqlCommand.Connection = conn;
        //    //    Appointment_Id = Convert.ToInt32(SqlCommand.ExecuteScalar());

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Appointment_Id = 0;
        //    //    throw ex;
        //    //}
        //    //finally
        //    //{
        //    //    if (conn.State == ConnectionState.Open) conn.Close();
        //    //}
        //    return Appointment_Id;
        //}

        ////public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate)
        ////{
        ////    SqlConnection conn = null;
        ////    SqlCommand SqlCommand = new SqlCommand();
        ////    SqlDataAdapter SqlDa = null;
        ////    CommonDB.TrackerSQLConnectionServer(ref conn);
        ////    try
        ////    {
        ////        if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string SqlSelect = SynchTrackerQRY.GetBookOperatoryAppointmenetWiseDateTime;
        ////        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        ////        SqlCommand.Parameters.AddWithValue("@ToDate", ApptDate.ToString("yyyy-MM-dd"));
        ////        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        ////        DataTable SqlDt = new DataTable();
        ////        SqlDa.Fill(SqlDt);
        ////        return SqlDt;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////    finally
        ////    {
        ////        if (conn.State == ConnectionState.Open) conn.Close();
        ////    }
        ////}

        //public static bool Update_Status_EHR_Appointment_Live_To_TrackerEHR(DataTable dtLiveAppointment)
        //{
        //    bool _successfullstataus = true;
        //    //SqlConnection conn = null;
        //    //SqlCommand SqlCommand = new SqlCommand();
        //    //CommonDB.TrackerSQLConnectionServer(ref conn);
        //    //if (conn.State == ConnectionState.Closed) conn.Open();
        //    //try
        //    //{
        //    //    string sqlSelect = string.Empty;
        //    //    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
        //    //    foreach (DataRow dr in dtLiveAppointment.Rows)
        //    //    {
        //    //        //if (Is_Update_Status_EHR_Appointment_Live_To_EHR(dr["Appt_EHR_ID"].ToString()))
        //    //        //{
        //    //        SqlCommand.CommandText = SynchTrackerQRY.Update_Status_EHR_Appointment_Live_To_Local;
        //    //        SqlCommand.Parameters.Clear();
        //    //        SqlCommand.Parameters.AddWithValue("@status", "1"); // 7:For Completed && 1:For Booked
        //    //        SqlCommand.Parameters.AddWithValue("@Apptid", dr["Appt_EHR_ID"].ToString());
        //    //        SqlCommand.ExecuteNonQuery();

        //    //        bool isApptConformStatus = UpdateLocalAppointmentConformStatusData(dr["Appt_EHR_ID"].ToString());
        //    //        // }
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    _successfullstataus = false;
        //    //    throw ex;
        //    //}
        //    //finally
        //    //{
        //    //    if (conn.State == ConnectionState.Open) conn.Close();
        //    //}
        //    return _successfullstataus;
        //}

        //public static bool UpdateLocalAppointmentConformStatusData(string appointment_id)
        //{
        //    bool _successfullstataus = true;
        //    SqlCeConnection conn = null;
        //    SqlCeCommand SqlCeCommand = null;
        //    CommonDB.LocalConnectionServer(ref conn);

        //    try
        //    {
        //        if (conn.State == ConnectionState.Closed) conn.Open();
        //        string sqlSelect = string.Empty;

        //        CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");

        //        SqlCeCommand.CommandText = SynchLocalQRY.UpdateLocalAppointmentConformStatusData;

        //        SqlCeCommand.Parameters.Clear();
        //        SqlCeCommand.Parameters.AddWithValue("appointment_id", appointment_id);
        //        SqlCeCommand.ExecuteNonQuery();

        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = false;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //    return _successfullstataus;
        //}
        //#endregion


        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime dateTime)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetOperatoryDateTimeWise;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@DateTime", SqlDbType.Date).Value = DateTime.Now.ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                //SqlDt.DefaultView.RowFilter = "closed_flag in (2,3,4)";
                return SqlDt.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static string GetPrimaryProviderId(string PatientId)
        {
            string providerid = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {

                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetPrimaryPovider;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("@ContactId", PatientId);
                providerid = Convert.ToString(SqlCommand.ExecuteScalar());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return providerid;
        }
        public static Int64 Save_Patient_Local_To_Tracker(string LastName, string FirstName, string MiddleName, string MobileNo, string Email, string ApptProv, string AppointmentDateTime, int Patient_Gur_id, int OperatoryId, string Birth_Date)
        {
            try
            {
                if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                {
                    Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
                }
                if (LastName.Length == 0)
                {
                    LastName += "NA";
                }

                Int64 PatientId = 0;
                Int64 DefaultContactId = 0;

                SqlConnection conn = null;

                SqlCommand SqlCommand = null;
                string patBirthDate = "";
                try
                {
                    if (Birth_Date != "")
                    {
                        patBirthDate = Convert.ToDateTime(Birth_Date.ToString()).ToString("yyyy-MM-dd");
                    }
                }
                catch (Exception)
                {
                    patBirthDate = "";
                }
                CommonDB.TrackerSQLConnectionServer(ref conn);
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //SqlCommand.CommandTimeout = 200;
                    //string StrSql= SynchTrackerQRY.InsertPatientDetails;
                    //StrSql = StrSql.Replace("@LastName", "'" + LastName + "'");
                    //StrSql = StrSql.Replace("@FirstName", "'" + FirstName + "'");
                    //StrSql = StrSql.Replace("@MobileNo", "'" + MobileNo + "'");
                    //StrSql = StrSql.Replace("@Email", "'" + Email + "'");
                    //StrSql = StrSql.Replace("@ProviderId", "'" + ApptProv + "'");

                    string sqlSelect = string.Empty;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    string strSql = SynchTrackerQRY.InsertPatientDetails;
                    if (Patient_Gur_id <= 0)
                    {
                        strSql = strSql.Replace("@Patient_Gur_id", "((select MAX(ISNULL( ContactID,0)) AS ClientId from Contact) + 1)");
                    }
                    SqlCommand.CommandText = strSql;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@LastName", LastName);
                    SqlCommand.Parameters.AddWithValue("@FirstName", FirstName);
                    SqlCommand.Parameters.AddWithValue("@MobileNo", MobileNo);
                    SqlCommand.Parameters.AddWithValue("@Email", Email);
                    SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                    SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                    if (Patient_Gur_id > 0)
                    {
                        SqlCommand.Parameters.AddWithValue("@Patient_Gur_id", Patient_Gur_id);
                    }

                    if (Birth_Date == "" || Birth_Date == string.Empty)
                    {
                        SqlCommand.Parameters.AddWithValue("@BirthDate", DBNull.Value);
                    }
                    else
                    {

                        SqlCommand.Parameters.AddWithValue("@BirthDate", Birth_Date);
                    }
                    if (ApptProv == "" || ApptProv == "0")
                    {

                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", "(select top 1 providerid from Provider where isactive = 1 order by ProviderName )");
                    }
                    else
                    {

                        SqlCommand.Parameters.AddWithValue("@ProviderId", ApptProv);
                    }

                    PatientId = Convert.ToInt64(SqlCommand.ExecuteScalar());

                    sqlSelect = SynchTrackerQRY.InsertPatientContactDetails;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@ContactId", PatientId);
                    SqlCommand.Parameters.AddWithValue("@MobileNo", MobileNo);
                    //SqlCommand.ExecuteNonQuery();
                    DefaultContactId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                    //Utility.WriteToSyncLogFile_All("Patient DefaultContactId:" + DefaultContactId);

                    sqlSelect = SynchTrackerQRY.UpdatePatientDefaultPhone;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@ContactId", PatientId);
                    SqlCommand.Parameters.AddWithValue("@DefaultPhoneId", DefaultContactId);
                    SqlCommand.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    PatientId = 0;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return PatientId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Int64 Save_Appointment_Local_To_Tracker(string FirstNameLastName, DateTime AppointmentStartTime, DateTime AppointmentEndTime, string PatNum, string OperatoryId,
          string classification, string ApptTypeId, DateTime AppointedDateTime, DateTime dob, string ProvNum, string AppointmentConfirmationStatus, bool allday_event, bool sooner_if_possible, bool privateAppointment, bool auto_confirm_sent, string procedureCode, string apptStatusId)
        {
            Int64 Appointment_Id = 0, Estimate_Id = 0;

            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
            }
            string pid; int pkey;

            SqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            SqlCommand SqlCommand = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = SynchTrackerQRY.InsertAppointmentDetails;
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("@ContactId", PatNum);
                SqlCommand.Parameters.AddWithValue("@ProviderId", ProvNum);
                SqlCommand.Parameters.AddWithValue("@ScheduleColumnId", OperatoryId);
                SqlCommand.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(AppointmentStartTime));
                SqlCommand.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(AppointmentEndTime));
                SqlCommand.Parameters.AddWithValue("@StatusId", apptStatusId);
                SqlCommand.Parameters.AddWithValue("@BookedUserId", Utility.EHR_UserLogin_ID);
                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                SqlCommand.Parameters.AddWithValue("@reasionId", ApptTypeId);
                // SqlCommand.Parameters.AddWithValue("@Confirmed", Confirmed);

                // SqlCommand.Parameters.AddWithValue("IsNewPatient", IsNewPatient);            
                Appointment_Id = Convert.ToInt64(SqlCommand.ExecuteScalar());


                if (procedureCode != null && procedureCode.Length > 0)
                {
                    try
                    {
                        foreach (var treatmentCode in procedureCode.Split(','))
                        {
                            pid = treatmentCode.Substring(0, treatmentCode.IndexOf('_'));
                            pkey = Convert.ToInt32(treatmentCode.Substring(treatmentCode.LastIndexOf('_') + 1));

                            if (pkey == 0)  //if procedure id is 0 then insert new estimate
                            {
                                sqlSelect = string.Empty;
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchTrackerQRY.InsertEstimate;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@ProcedureCodeId", pid);
                                SqlCommand.Parameters.AddWithValue("@Patient_EHR_Id", PatNum);
                                SqlCommand.Parameters.AddWithValue("@Appointment_DateTime", Convert.ToDateTime(AppointmentStartTime));
                                SqlCommand.Parameters.AddWithValue("@Provider_EHR_Id", ProvNum);
                                SqlCommand.Parameters.AddWithValue("@Comment", "Adit App Appointment");
                                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                Estimate_Id = Convert.ToInt64(SqlCommand.ExecuteScalar());

                                try
                                {
                                    sqlSelect = string.Empty;
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchTrackerQRY.InsertEstimateEntry;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@EstimateId", Estimate_Id);
                                    SqlCommand.Parameters.AddWithValue("@Patient_EHR_Id", PatNum);
                                    SqlCommand.Parameters.AddWithValue("@Appointment_EHR_Id", Appointment_Id);
                                    SqlCommand.Parameters.AddWithValue("@Provider_EHR_Id", ProvNum);
                                    SqlCommand.Parameters.AddWithValue("@ProcedureCodeId", pid);
                                    SqlCommand.Parameters.AddWithValue("@Appointment_DateTime", Convert.ToDateTime(AppointmentStartTime));
                                    SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                    SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                    SqlCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Utility.WriteToSyncLogFile_All("[Appointment Sync_EstimateEntry  (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                                }
                            }
                            else
                            {
                                // update AppointmentId into the Estimate Etnry Table
                                try
                                {
                                    sqlSelect = string.Empty;
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchTrackerQRY.UpdateEstimateEntry;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@Appointment_EHR_Id", Appointment_Id);
                                    SqlCommand.Parameters.AddWithValue("@Patient_EHR_Id", PatNum);
                                    SqlCommand.Parameters.AddWithValue("@ProcedureCodeId", pid);
                                    SqlCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Utility.WriteToSyncLogFile_All("[Appointment Sync_EstimateEntry  (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                Appointment_Id = 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return Appointment_Id;
        }


        public static bool Update_Status_EHR_Appointment_Live_To_TrackerEHR(DataTable dtLiveAppointment, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                foreach (DataRow dr in dtLiveAppointment.Rows)
                {
                    sqlSelect = SynchTrackerQRY.UpdateAppointmentStatusFromWeb;


                    if (string.IsNullOrEmpty(dr["confirmed_status_ehr_key"].ToString()) || (!string.IsNullOrEmpty(dr["confirmed_status_ehr_key"].ToString()) && Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 0))
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " isconfirmed = 1 ");
                    }
                    //Arrived
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1001)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " FlowState = 'Arrived',FlowChange = '" + System.DateTime.Now + "' ");
                    }
                    //Check In
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1002)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " FlowState = 'Checked In',CheckIn = '" + System.DateTime.Now.ToShortTimeString() + "',FlowChange = '" + System.DateTime.Now + "' ");
                    }
                    //On Deck
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1003)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " FlowState = 'On Deck',FlowChange = '" + System.DateTime.Now + "' ");
                    }
                    //In Chair
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1004)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " FlowState = 'In Chair',InChair = '" + System.DateTime.Now.ToShortTimeString() + "',FlowChange = '" + System.DateTime.Now + "' ");
                    }
                    //Completed
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1005)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " FlowState = 'Completed',CheckOut = '" + System.DateTime.Now.ToShortTimeString() + "',FlowChange = '" + System.DateTime.Now + "' ");
                    }
                    //Preconfirmed
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1006)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " ispreconfirmed = 1 ");
                    }
                    //Confirmed
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1007)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " isconfirmed = 1 ");
                    }
                    //Personal
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1008)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " FlowState = 'X-Rays',FlowChange = '" + System.DateTime.Now + "' ");
                    }
                    //Recall
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1009)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " FlowState = 'Reception',FlowChange = '" + System.DateTime.Now + "' ");
                    }
                    //Recovery
                    else if (Convert.ToInt16(dr["confirmed_status_ehr_key"]) == 1010)
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " FlowState = 'Recovery',OutChair = '" + System.DateTime.Now.ToShortTimeString() + "',FlowChange = '" + System.DateTime.Now + "' ");
                    }
                    else
                    {
                        sqlSelect = sqlSelect.Replace("@FieldToUpdate", " StatusId = " + dr["confirmed_status_ehr_key"].ToString() + "");
                    }
                    sqlSelect = sqlSelect.Replace("@AppointmentId", dr["Appt_EHR_ID"].ToString());
                    SqlCommand.CommandText = sqlSelect;
                    SqlCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Confirm EHR Appointment Live To TrackerEHR for confirmed_status_ehr_key=" + dr["confirmed_status_ehr_key"] + " and AppointmentId (" + dr["Appt_EHR_ID"].ToString());
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Update_Receive_SMS_Patient_EHR_Live_To_TrackerEHR(DataTable dtLiveAppointment, string Locationid, string Loc_ID, string _filename_EHR_patientoptout = "", string _EHRLogdirectory_EHR_patientoptout = "")
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                Patient_OptOutBO_StatusUpdate updatedPatientid = new Patient_OptOutBO_StatusUpdate();
                List<Patientids_OptOutBO_StatusUpdate> Patient_StatusUpdate = new List<Patientids_OptOutBO_StatusUpdate>();
                foreach (DataRow dr in dtLiveAppointment.Rows)
                {
                    try
                    {
                        sqlSelect = SynchTrackerQRY.Update_Receive_SMS_Patient_EHR_Live_To_TrackerEHR;
                        SqlCommand.Parameters.AddWithValue("@receives_sms", dr["receive_sms"].ToString() == "Y" ? 1 : 0);
                        SqlCommand.Parameters.AddWithValue("@patient_id", dr["patient_ehr_id"].ToString());
                        SqlCommand.CommandText = sqlSelect;
                        SqlCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_patientoptout, _EHRLogdirectory_EHR_patientoptout, " Confirm Receive SMS Patient EHR Live To TrackerEHR for  patient_id=" + dr["patient_ehr_id"].ToString() + " and receive_sms=" + dr["receive_sms"].ToString());
                        Patientids_OptOutBO_StatusUpdate Patientids = new Patientids_OptOutBO_StatusUpdate();
                        Patientids.esId = dr["esid"].ToString();
                        Patientids.patientId = dr["patientid"].ToString();
                        Patient_StatusUpdate.Add(Patientids);
                    }
                    catch (Exception)
                    {
                    }

                }
                if (Patient_StatusUpdate.Count > 0)
                {
                    updatedPatientid.locationId = Loc_ID;
                    updatedPatientid.organizationId = Utility.Organization_ID;
                    updatedPatientid.patientIds = Patient_StatusUpdate;
                    PushLiveDatabaseDAL.UpdatePatientReceive_SMStStatusToWeb(updatedPatientid, Locationid, Loc_ID);
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }
        private static bool UpdatePatientEHRIdINPatientForm(string PatientEHRId, string PatientFormWebId)
        {
            try
            {
                bool _successfullstataus = true;
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string sqlSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_PatientEHRId;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_Id", PatientEHRId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", PatientFormWebId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.ExecuteNonQuery();
                            _successfullstataus = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        _successfullstataus = false;
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
                return _successfullstataus;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static DataTable GetTrackerTableColumnName(string tableName, DataTable dtWebPatient_Form)
        {            
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
            }
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.TrackerSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();

            using (SqlCeConnection conn1 = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                SqlCeCommand SqlCeCommand = null;
                SqlCeDataAdapter SqlCeDa = null;
                //CommonDB.LocalConnectionServer(ref conn1);

                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    DataTable dtTrackerColumns = new DataTable();
                    // DataTable OdbcDt = new DataTable();
                    GetTrackerTableColumnList("Patient", ref dtTrackerColumns);

                    dtTrackerColumns.AsEnumerable()
                        .All(o =>
                        {
                            if (o["EHRColumnName"].ToString().ToUpper() == "CLIENTID")
                            {
                                o["DefaultValue"] = "0";
                                o["PatientFormColumnsName"] = "";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "TITLE")
                            {
                                o["DefaultValue"] = "";
                                o["PatientFormColumnsName"] = "salutation";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "FIRSTNAME")
                            {
                                o["DefaultValue"] = "NA";
                                o["PatientFormColumnsName"] = "first_name";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "LASTNAME")
                            {
                                o["DefaultValue"] = "NA";
                                o["PatientFormColumnsName"] = "last_name";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "NICKNAME")
                            {
                                o["DefaultValue"] = "";
                                o["PatientFormColumnsName"] = "preferred_name";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "JRSR")
                            {
                                o["DefaultValue"] = "";
                                o["PatientFormColumnsName"] = "";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "PRONUNCIATION")
                            {
                                o["DefaultValue"] = "";
                                o["PatientFormColumnsName"] = "";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ADDRESS1")
                            {
                                o["DefaultValue"] = "";
                                o["PatientFormColumnsName"] = "address_one";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ADDRESS2")
                            {
                                o["DefaultValue"] = "";
                                o["PatientFormColumnsName"] = "address_two";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "CITY")
                            {
                                o["DefaultValue"] = "";
                                o["PatientFormColumnsName"] = "city";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "REGIONID")
                            {
                                o["DefaultValue"] = "1";
                                o["PatientFormColumnsName"] = "";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "POSTALCODE")
                            {
                                o["PatientFormColumnsName"] = "zipcode";
                                o["DefaultValue"] = "";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "PHONENUMBER")
                            {
                                o["PatientFormColumnsName"] = "home_phone";
                                o["DefaultValue"] = "0000000000";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "WORKPHONENUMBER")
                            {
                                o["PatientFormColumnsName"] = "work_phone";
                                o["DefaultValue"] = "0000000000";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "FAXNUMBER")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0000000000";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "OTHERPHONENUMBER")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0000000000";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "EMAIL")
                            {
                                o["PatientFormColumnsName"] = "email";
                                o["DefaultValue"] = "";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "HASEMAILCONSENT")
                            {
                                o["PatientFormColumnsName"] = "receive_email";
                                o["DefaultValue"] = "1";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "LANGUAGE")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "ENG";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "USERECALL")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "1";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "USESHORTRECALL")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "USESTATEMENT")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "1";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ISACTIVE")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "1";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ENTEREDDATE")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "GETDATE()";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ENTEREDDATE")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "GETDATE()";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ISPATIENT")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "1";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ISVENDOR")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ISPROFESSIONAL")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ISOTHER")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "BIRTHDATE")
                            {
                                o["PatientFormColumnsName"] = "birth_date";
                                o["DefaultValue"] = "";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "SEX")
                            {
                                o["PatientFormColumnsName"] = "sex";
                                o["DefaultValue"] = "M";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "PRACTICEID")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "ISNULL((SELECT TOP 1 PracticeId FROM Contact where PracticeId is not null),0)";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "PROVIDERID")
                            {
                                o["PatientFormColumnsName"] = "pri_provider_id";
                                o["DefaultValue"] = "ISNULL((SELECT TOP 1 ProviderId FROM Contact where ProviderId is not null),0)";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "PROVIDER2ID")
                            {
                                o["PatientFormColumnsName"] = "sec_provider_id";
                                o["DefaultValue"] = "ISNULL((SELECT TOP 1 ProviderId FROM Contact where ProviderId is not null),0)";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "RECALLLENGTH")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "6";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "RECALLINTERVAL")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "Month";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "EXCLUDEINTEREST")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ISSIGNATURE")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ISHANDICAPPED")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "ISSTUDENT")
                            {
                                o["PatientFormColumnsName"] = " ";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "MODIFIEDUSERID")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = Utility.EHR_UserLogin_ID;
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "MODIFIEDTIMESTAMP")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "GETDATE()";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "CREATEDUSERID")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = Utility.EHR_UserLogin_ID;
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "CREATEDTIMESTAMP")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "GETDATE()";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "MODIFIEDMACHINENAME")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "ISNULL((SELECT TOP 1 MODIFIEDMACHINENAME FROM CONTACT),'')";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "CREATEDMACHINENAME")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "ISNULL((SELECT TOP 1 CREATEDMACHINENAME FROM CONTACT),'')";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "MOBILENUMBER")
                            {
                                o["PatientFormColumnsName"] = "mobile";
                                o["DefaultValue"] = "0000000000";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "CONTACTMETHODAPPT")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "1";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "CONTACTMETHODFINANCIAL")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "2";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "CONTACTMETHODMARKETING")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "2";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "DEFAULTPHONEID")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "(SELECT TOP 1 DefaultPHoneId From COntact)";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "BookSameDayOnly")
                            {
                                o["PatientFormColumnsName"] = "";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "IDENTIFICATIONNUMBER")
                            {
                                o["PatientFormColumnsName"] = "ssn";
                                o["DefaultValue"] = "0";
                            }
                            if (o["EHRColumnName"].ToString().ToUpper() == "COMPANY")
                            {
                                foreach (DataRow dr in dtWebPatient_Form.Rows)
                                {
                                    if (dr["pformfield"].ToString().Trim().ToString().ToUpper() == "SCHOOL")
                                    {
                                        o["PatientFormColumnsName"] = "school";
                                        o["DefaultValue"] = " ";
                                        break;
                                    }
                                    else
                                    {
                                        o["PatientFormColumnsName"] = "employer";
                                        o["DefaultValue"] = " ";
                                    }
                                }

                            }

                            return true;
                        });

                    DataRow drNewRow = dtTrackerColumns.NewRow();
                    //    drNewRow["COLUMN_NAME"] = "SECONDARY_INS_SUBSCRIBER_ID";
                    drNewRow["EHRColumnName"] = "SECONDARY_SUBSCRIBER_ID";
                    drNewRow["EHRDataType"] = "System.String";
                    drNewRow["AllowNull"] = "Yes";
                    dtTrackerColumns.Rows.Add(drNewRow);
                    drNewRow = dtTrackerColumns.NewRow();
                    // drNewRow["COLUMN_NAME"] = "SECONDARY_INSURANCE_COMPANYNAME";
                    drNewRow["EHRColumnName"] = "SECONDARY_INSURANCE_COMPANYNAME";
                    drNewRow["EHRDataType"] = "System.String";
                    drNewRow["AllowNull"] = "Yes";
                    dtTrackerColumns.Rows.Add(drNewRow);
                    drNewRow = dtTrackerColumns.NewRow();
                    //drNewRow["COLUMN_NAME"] = "PRIMARY_INS_SUBSCRIBER_ID";
                    drNewRow["EHRColumnName"] = "PRIMARY_SUBSCRIBER_ID";
                    drNewRow["EHRDataType"] = "System.String";
                    drNewRow["AllowNull"] = "Yes";
                    dtTrackerColumns.Rows.Add(drNewRow);
                    drNewRow = dtTrackerColumns.NewRow();
                    //drNewRow["COLUMN_NAME"] = "PRIMARY_INSURANCE_COMPANYNAME";
                    drNewRow["EHRColumnName"] = "PRIMARY_INSURANCE_COMPANYNAME";
                    drNewRow["EHRDataType"] = "System.String";
                    drNewRow["AllowNull"] = "Yes";
                    dtTrackerColumns.Rows.Add(drNewRow);
                    return dtTrackerColumns;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                    if (conn1.State == ConnectionState.Open) conn1.Close();
                }
            }
        }

        private static string AssigneValueCompitibleTOEHR(string fieldValue, string EHRColumnsName)
        {
            try
            {
                string returnvalue = "";

                switch (fieldValue.ToString().Trim().ToUpper())
                {
                    case "MALE":
                        returnvalue = "M";
                        break;
                    case "FEMALE":
                        returnvalue = "F";
                        break;
                    case "YES":
                        returnvalue = "1";
                        break;
                    case "NO":
                        returnvalue = "0";
                        break;
                    case "TRUE":
                        returnvalue = "1";
                        break;
                    case "FALSE":
                        returnvalue = "0";
                        break;
                    default:
                        if (EHRColumnsName == "HASEMAILCONSENT" || EHRColumnsName == "ISSMSENABLED")
                        {
                            returnvalue = "1";
                        }
                        else if (EHRColumnsName == "MARITAL_STATUS")
                        {
                            returnvalue = "0";
                        }
                        else if (EHRColumnsName == "SEX")
                        {
                            returnvalue = "M";
                        }
                        break;
                }
                return returnvalue;
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }

        private static string CreatePatientInsertQuery(DataTable dtWebPatient_Form, DataTable dtTrackerPatentColumns, string patientFormWebId, string tableName, ref string PrimaryInsuranceCompanyName, ref string SecondaryInsuranceCompanyName, ref string PrimaryInsuranceSubScriberId, ref string SecondaryInsuraceSubScriberId)
        {
            try
            {
                string strQauery = "";
                string ColumnList = "";
                string ValueList = "";
                string PInsuranceCompanyName = "";
                string SInsuranceCompanyName = "";
                string PInsuranceSubScriberId = "";
                string SInsuranceSubScriberId = "";
                //string strQaueryContactPhone = "", ColumnListContactPhone = "", ValueListContactPhone = "";

                dtTrackerPatentColumns.AsEnumerable()//.Where(z => z.Field<string>("EHRColumnName") != "")
                    .All(e =>
                    {
                        var dtColumnsExists = dtWebPatient_Form.AsEnumerable()
                            .Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == patientFormWebId.ToString() && a.Field<object>("ehrfield").ToString().ToUpper() == e.Field<object>("EHRColumnName").ToString().ToUpper());

                        #region Insurance
                        if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME" || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME"
                            || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID" || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                        {
                            if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
                            {
                                if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                {
                                    PInsuranceCompanyName = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                }
                                else
                                {
                                    PInsuranceCompanyName = "";
                                }
                            }
                            else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
                            {
                                if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                {
                                    SInsuranceCompanyName = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                }
                                else
                                {
                                    SInsuranceCompanyName = "";
                                }
                            }
                            else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                            {
                                if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                {
                                    PInsuranceSubScriberId = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                }
                                else
                                {
                                    PInsuranceSubScriberId = "";
                                }
                            }
                            else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                            {
                                if (dtColumnsExists != null && dtColumnsExists.Count() > 0 && dtColumnsExists.FirstOrDefault() != null && dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                {
                                    SInsuranceSubScriberId = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim();
                                }
                                else
                                {
                                    SInsuranceSubScriberId = "";
                                }
                            }
                        }
                        //else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "ISSTUDENT")
                        //{
                        //    ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";

                        //    //var resultschool = dtWebPatient_Form.AsEnumerable()
                        //    //.Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == patientFormWebId.ToString() && a.Field<object>("pformfield").ToString().ToUpper() == "SCHOOL");


                        //    DataRow resultschool = dtWebPatient_Form.Select("PatientForm_Web_ID = '" + patientFormWebId.ToString() + "' AND pformfield = 'School'").FirstOrDefault();
                        //    //.Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == patientFormWebId.ToString() && a.Field<object>("pformfield").ToString().ToUpper() == "SCHOOL")
                        //    DataRow resultemployer = dtWebPatient_Form.Select("PatientForm_Web_ID = '" + patientFormWebId.ToString() + "' AND pformfield = 'Employer'").FirstOrDefault();
                        //    //var resultemployer = dtWebPatient_Form.AsEnumerable()
                        //    //.Where(a => a.Field<object>("PatientForm_Web_ID").ToString() == patientFormWebId.ToString() && a.Field<object>("pformfield").ToString().ToUpper() == "EMPLOYER");

                        //    if (resultschool != null && resultschool.Field<object>("pformfield") != null && resultschool.Field<object>("pformfield").ToString().ToUpper() == "SCHOOL")
                        //    {
                        //        ValueList = ValueList + "1,";
                        //    }
                        //    else if (resultemployer != null && resultemployer.Field<object>("pformfield") != null && resultemployer.Field<object>("pformfield").ToString().ToUpper() == "EMPLOYER")
                        //    {
                        //        ValueList = ValueList + "0,";
                        //    }
                        //}
                        #endregion
                        else if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                        {
                            ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";
                            if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "PHONENUMBER" ||
                                e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "WORKPHONENUMBER" ||
                                e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "MOBILENUMBER")
                            {
                                ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "") + "'" + ",";
                            }
                            else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "BIRTHDATE")
                            {
                                ValueList = ValueList + "'" + Convert.ToDateTime(dtColumnsExists.First().Field<object>("ehrfield_value")).ToString("yyyy/MM/dd HH:mm").ToString() + "'" + ",";
                            }
                            else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "ISSTUDENT")
                            {
                                if (dtColumnsExists.First().Field<object>("pformfield") != null)
                                {
                                    if (dtColumnsExists.First().Field<object>("pformfield").ToString().ToUpper() == "SCHOOL")
                                    {
                                        ValueList = ValueList + "1,";
                                    }
                                    else if (dtColumnsExists.First().Field<object>("pformfield").ToString().ToUpper() == "EMPLOYER")
                                    {
                                        ValueList = ValueList + "0,";
                                    }
                                }
                            }
                            else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "COMPANY")
                            {
                                if (dtColumnsExists.First().Field<object>("ehrfield_value") != null && dtColumnsExists.First().Field<object>("ehrfield_value").ToString() != string.Empty)
                                {
                                    if (dtColumnsExists.First().Field<object>("pformfield") != null)
                                    {
                                        ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "''" + ",";
                                    }
                                }

                            }
                            else if (e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "HASEMAILCONSENT"
                                || e.Field<object>("EHRColumnName").ToString().Trim().ToUpper() == "SEX")
                            {
                                if (dtColumnsExists.First().Field<object>("ehrfield_value") != null && dtColumnsExists.First().Field<object>("ehrfield_value").ToString() != string.Empty)
                                {
                                    ValueList = ValueList + "'" + AssigneValueCompitibleTOEHR(dtColumnsExists.First().Field<object>("ehrfield_value").ToString(), e.Field<object>("EHRColumnName").ToString().ToUpper()) + "'" + ",";
                                }
                            }
                            else
                            {
                                if (dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() == string.Empty)
                                {
                                    if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "INT")
                                    {
                                        ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                    }
                                    else
                                    {
                                        if (e.Field<object>("DefaultValue").ToString().ToUpper().Contains("ISNULL((SELECT TOP 1"))
                                        {
                                            ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                        }
                                        else
                                        {
                                            ValueList = ValueList + "'" + e.Field<object>("DefaultValue").ToString() + "'" + ",";
                                        }
                                    }
                                    //ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                }
                                else
                                {
                                    ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                }
                            }
                        }
                        else
                        {
                            ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";
                            if (e.Field<object>("EHRDataType") != null && (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "DATETIME"))
                            {
                                if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                                {
                                    ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                }
                                else
                                {
                                    ValueList = ValueList + "NULL" + ",";
                                }
                            }
                            else if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                            {
                                if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "INT")
                                {
                                    ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                }
                                else
                                {
                                    if (e.Field<object>("DefaultValue").ToString().ToUpper().Contains("ISNULL((SELECT TOP 1"))
                                    {
                                        ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "'" + e.Field<object>("DefaultValue").ToString() + "'" + ",";
                                    }
                                }
                            }
                            else if (e.Field<object>("AllowNull").ToString().Trim().ToUpper() == "YES")
                            {
                                ValueList = ValueList + "NULL" + ",";
                            }
                            //!!!!
                            else if ((e.Field<object>("EHRDataType") != null && (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "BIT")))//&& e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                            {
                                ValueList = ValueList + "0" + ",";
                            }
                            //!!!

                            else
                            {
                                ValueList = ValueList + "''" + ",";
                            }
                        }
                        return true;
                    });

                ColumnList = ColumnList.Substring(0, ColumnList.Length - 1);
                ValueList = ValueList.Substring(0, ValueList.Length - 1);
                PrimaryInsuranceCompanyName = PInsuranceCompanyName;
                SecondaryInsuranceCompanyName = SInsuranceCompanyName;
                PrimaryInsuranceSubScriberId = PInsuranceSubScriberId;
                SecondaryInsuraceSubScriberId = SInsuranceSubScriberId;
                strQauery = " Insert into Contact ( " + ColumnList + " ) VALUES ( " + ValueList + " )";
                return strQauery;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static void UpdatePatientInsurance(string curPatinetInsurance_Name, long PatientId, int InsuranceCount, long FamilyId, string SubScriber)
        {           
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
            }
            SqlConnection conn = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                if (curPatinetInsurance_Name == "")
                {
                    return;
                }
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                DateTime ToDate = dtCurrentDtTime;

                SqlCommand SqlCommand = new SqlCommand();
                SqlDataAdapter SqlDa = null;

                Int32 InsCarrId = 0;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "select TOP 1 [carrierid] from Carrier where replace([Company],'''','') = '" + curPatinetInsurance_Name + "'";
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                InsCarrId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                if (InsCarrId > 0)
                {
                    Int32 Insplanid = 0;
                    Int32 InsSubscriberId = 0;

                    SqlCommand.CommandText = SynchTrackerQRY.InsertPatient_InsurancePlan;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@CarrierId", InsCarrId);
                    SqlCommand.Parameters.AddWithValue("@ModifiedMachineName", Environment.MachineName);
                    SqlCommand.Parameters.AddWithValue("@CreatedMachineName", Environment.MachineName);
                    SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                    SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                    SqlCommand.ExecuteNonQuery();
                    Insplanid = GetId(conn, "InsurancePlan", "planid");

                    CheckConnection(conn);
                    SqlCommand.CommandText = SynchTrackerQRY.InsertPatient_Subscriber;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@ContactId", PatientId);
                    SqlCommand.Parameters.AddWithValue("@InsuranceCode", SubScriber);
                    SqlCommand.Parameters.AddWithValue("@InsurancePlanId", Insplanid);
                    SqlCommand.ExecuteNonQuery();
                    InsSubscriberId = GetId(conn, "Subscriber", "SubscriberId");

                    CheckConnection(conn);
                    SqlCommand.CommandText = SynchTrackerQRY.InsertPatient_Coverage;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@SubscriberId", InsSubscriberId);
                    SqlCommand.Parameters.AddWithValue("@ContactId", PatientId);
                    if (InsuranceCount == 1)
                    {
                        SqlCommand.Parameters.AddWithValue("@CoverageType", 1);
                    }
                    else
                    {
                        SqlCommand.Parameters.AddWithValue("@CoverageType", 2);
                    }
                    SqlCommand.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static int GetId(SqlConnection CON, string TableName, string FieldName, string Condition = "", bool CastInt = false)
        {
            try
            {
                if (CON.State != ConnectionState.Open)
                    CON.Open();
                if (CastInt)
                    FieldName = "Cast(" + FieldName + " as numeric)";
                SqlCommand CMD = new SqlCommand("Select max(" + FieldName + ") from " + TableName + " " + Condition, CON);
                return (System.Convert.ToInt32(CMD.ExecuteScalar()));
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                CON.Close();
            }
        }
        public static void CheckConnection(SqlConnection CON)
        {
            if (CON.State != ConnectionState.Open)
                CON.Open();
        }
        private static string GetValueSizeWise(DataTable dtTrackerColumns, string columnsName, string ehrFieldValue)
        {
            string returnResult = "";
            try
            {
                var result = dtTrackerColumns.AsEnumerable().Where(o => o.Field<string>("EHRColumnName").ToString().ToUpper() == columnsName.ToString().ToUpper()).Select(a => a.Field<string>("EHRDataType")).First();
                var resultSize = dtTrackerColumns.AsEnumerable().Where(o => o.Field<string>("EHRColumnName").ToString().ToUpper() == columnsName.ToString().ToUpper()).Select(a => a.Field<Int32>("Size")).First();

                if (result != null && result.ToString() != string.Empty && resultSize != null && resultSize.ToString() != string.Empty)
                {
                    switch (result.ToString().ToUpper())
                    {
                        case "VARCHAR":
                            if (ehrFieldValue.Length > Convert.ToInt32(resultSize) && Convert.ToInt32(resultSize) > 0)
                            {
                                returnResult = "'" + ehrFieldValue.Substring(0, resultSize.ToString().Length) + "'";
                            }
                            else
                            {
                                returnResult = "'" + ehrFieldValue + "'";
                            }
                            break;
                        case "BIT":
                            returnResult = ehrFieldValue;
                            break;
                        case "CHAR":
                            returnResult = "'" + ehrFieldValue + "'";
                            break;
                        case "DATETIME":
                            returnResult = "'" + ehrFieldValue + "'";
                            break;
                        case "INT":
                            returnResult = ehrFieldValue;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnResult;
        }

        //public static void deleteduplicates(string tablename,string columnname,Int64 id)
        //{
        //    SqlConnection conn = null;
        //    SqlCommand SqlCommand = null;

        //    CommonDB.TrackerSQLConnectionServer(ref conn);
        //    string sqlSelect = string.Empty;
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
        //    SqlCommand.CommandText = "Delete from " + tablename.Trim() + " where " + columnname.Trim() + "=" + id + " "; 
        //    SqlCommand.ExecuteNonQuery();

        //}
        public static Int64 SavePatientPaymentTOEHR(string DbString, DataTable DtTable, string ServiceInstallationId, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {

            Int64 FinancialTransactionId = 0;
            Int64 DiscountId = 0;
            SqlConnection conn = null;

            SqlCommand SqlCommand = null;

            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
            }

            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                Int64 MethodId = 0, DiscountTypeId = 0 , CareCreditDiscountTypeID=0;
                Int64 CareCreditMethodId = 0;
                string EHRLogId = "0";
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                #region Check for the 'Adit Pay'Categeries

                #region check for the Adit Pay Mode
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = SynchTrackerQRY.CheckPaymentModeExistAsAditPay;
                MethodId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                if (MethodId > 0)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check Payment Mode Exist As AditPay and MethodId=" + MethodId.ToString());
                }
                #endregion

                #region check for the Adit Pay Discount Mode
                var result = DtTable.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                if (result != null)
                {
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchTrackerQRY.CheckPaymentModeExistAsAditPayDiscount;
                    DiscountTypeId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                    if (DiscountTypeId > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check Payment Mode Exist As AditPay Discount and DiscountTypeId=" + DiscountTypeId.ToString());
                    }
                }
                #endregion

                #endregion

                #region Check for the 'Adit Pay'Categeries

                #region Add PaymentMode 'Care Credit' To EHR
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = SynchTrackerQRY.CheckPaymentModeExistAsCareCredit;
                CareCreditMethodId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                if (CareCreditMethodId > 0)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check Payment Mode Exist As CareCredit and MethodId=" + CareCreditMethodId.ToString());
                }
                #endregion

                #region Check for the CareCredit Discount Mode 
                var resultt = DtTable.AsEnumerable().Where(o => o.Field<decimal>("Discount") > 0).FirstOrDefault();
                if (resultt != null)
                {
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchTrackerQRY.CheckPaymentModeExistAsCareCreditDiscount;
                    CareCreditDiscountTypeID = Convert.ToInt64(SqlCommand.ExecuteScalar());
                    if (CareCreditDiscountTypeID > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Check Payment Mode Exist As CareCredit Discount and CareCreditDiscountTypeID=" + CareCreditDiscountTypeID.ToString());
                    }
                }
                #endregion

                #endregion  

                foreach (DataRow drRow in DtTable.Rows)
                {
                    EHRLogId = "0";
                    FinancialTransactionId = 0;
                    DiscountId = 0;
                    decimal amount = 0;
                    try
                    {
                        amount = Convert.ToDecimal(drRow["Amount"]) - Convert.ToDecimal(drRow["Discount"]);
                        
                        decimal balance = 0;
                        #region GetPatientBalance
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchTrackerQRY.GetPatientBalance;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                        balance = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get Patient Balance for PatientEHRId=" + drRow["PatientEHRId"].ToString());
                        #endregion

                        string note = drRow["template"].ToString().Length > 50 ? drRow["template"].ToString().Substring(0, 50) : drRow["template"].ToString();

                        if (drRow["PaymentMethod"].ToString().ToLower() == "carecredit")
                        {
                            SaveCareCreditPaymentToEHR(DbString,ServiceInstallationId, drRow,amount,CareCreditMethodId,balance,note, CareCreditDiscountTypeID,_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment,EHRLogId);
                        }
                        else
                        {
                            SaveAditPayPaymentToEHR(DbString,ServiceInstallationId, drRow, amount,MethodId, balance, note,DiscountTypeId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment,EHRLogId);
                        }
                        
                    }
                    catch (Exception ex1)
                    {
                        bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow);
                       
                    }


                }
                return FinancialTransactionId;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static void SaveCareCreditPaymentToEHR(string DbString, string ServiceInstallationId,DataRow drRow, decimal amount,Int64 CareCreditMethodId,decimal balance, string note,Int64 CareCreditDiscountTypeID, string _filename_EHR_Payment,string _EHRLogdirectory_EHR_Payment,string EHRLogId)
        {
            Int64 FinancialTransactionId = 0;
            Int64 DiscountId = 0;
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            string sqlSelect = string.Empty;

            try
            {
                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 2 || Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 3)
                {
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        #region get bankid for patient
                        Int64 bankid = 0;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchTrackerQRY.GetBankIdFromFinancialTransaction;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                        bankid = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        if (bankid > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get BankId From Financial Transaction for PatientEHRId=" + drRow["PatientEHRId"].ToString() + " and get bankid=" + bankid.ToString());
                        }
                        #endregion

                        #region save payment to FinancialTransaction table
                        if (amount != 0)//&& balance > 0)
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchTrackerQRY.InsertPaymentToFinancialTransaction;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                            SqlCommand.Parameters.AddWithValue("@Amount", Convert.ToDecimal(drRow["Amount"]) - Convert.ToDecimal(drRow["Discount"]));
                            SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"].ToString());
                            SqlCommand.Parameters.AddWithValue("@PaymentNote", drRow["template"].ToString());
                            SqlCommand.Parameters.AddWithValue("@ChequeNumber", drRow["ChequeNumber"].ToString());
                            SqlCommand.Parameters.AddWithValue("@BankId", bankid);
                            SqlCommand.Parameters.AddWithValue("@MethodId", CareCreditMethodId);
                            SqlCommand.Parameters.AddWithValue("@TransactionType", 2);
                            SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                            SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                            FinancialTransactionId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                            if (FinancialTransactionId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment To Financial Transaction PatientEHRId=" + drRow["PatientEHRId"].ToString() + ", and BankId=" + bankid.ToString() + " and MethodId=" + CareCreditMethodId.ToString() + " get FinancialTransactionId=" + FinancialTransactionId.ToString());
                            }
                        }
                        #endregion

                        #region If FinancialTransactionId is greater then save entry to TransactionDetail table
                        if (FinancialTransactionId > 0)
                        {
                            try
                            {
                                SqlCommand.CommandText = SynchTrackerQRY.InsertDiscount;   //InsertDiscount;//insertPaymentToTDetail; //entry to transactiondetail
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                // SqlCommand.Parameters.AddWithValue("@Amount", -amount); 
                                SqlCommand.Parameters.AddWithValue("@DiscountAmount", amount);
                                if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                                {
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT ProviderId From CONTACT where ContactId = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                                }
                                else
                                {
                                    SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                                }
                                SqlCommand.Parameters.AddWithValue("@PaymentNote", note.ToString());
                                if (balance > 0)
                                {
                                    SqlCommand.Parameters.AddWithValue("@TransactionType", 2);
                                }
                                else
                                {
                                    SqlCommand.Parameters.AddWithValue("@TransactionType", 10);
                                }

                                SqlCommand.Parameters.AddWithValue("@FinancialTransactionId", FinancialTransactionId);
                                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Discount Confirmed in local for PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",ProviderEHRId=" + drRow["ProviderEHRId"].ToString() + ",FinancialTransactionId=" + FinancialTransactionId.ToString());
                                // }

                            }
                            catch (Exception ex)
                            {
                                //deleteduplicates("FinancialTransaction", "FinancialTransactionId", FinancialTransactionId);
                                //continue;
                            }

                        }
                        #endregion

                        #region If discount is greater then zero then save entry to FinancialTransaction and TransactionDetail Table
                        if (Convert.ToDecimal(drRow["Discount"]) > 0)//&& balance > 0)
                        {
                            try
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchTrackerQRY.InsertPaymentToFinancialTransaction;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                SqlCommand.Parameters.AddWithValue("@Amount", Convert.ToDecimal(drRow["Discount"]));
                                SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"].ToString());
                                SqlCommand.Parameters.AddWithValue("@PaymentNote", drRow["template"].ToString());
                                SqlCommand.Parameters.AddWithValue("@ChequeNumber", drRow["ChequeNumber"].ToString());
                                SqlCommand.Parameters.AddWithValue("@BankId", bankid);
                                SqlCommand.Parameters.AddWithValue("@TransactionType", 3);
                                SqlCommand.Parameters.AddWithValue("@MethodId", CareCreditDiscountTypeID);
                                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                DiscountId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                if (DiscountId > 0)
                                {
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Payment To Financial Transaction  Confirmed in EHR for PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",BankId=" + bankid.ToString() + ",MethodId=" + CareCreditMethodId.ToString());
                                }

                            }
                            catch (Exception)
                            {
                                //deleteduplicates("TransactionDetail", "FinancialTransactionId", FinancialTransactionId);
                                //deleteduplicates("FinancialTransaction", "FinancialTransactionId", FinancialTransactionId);
                                //continue;
                            }

                            try
                            {
                                SqlCommand.CommandText = SynchTrackerQRY.InsertDiscount;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                SqlCommand.Parameters.AddWithValue("@DiscountAmount", Convert.ToDecimal(drRow["Discount"]));
                                if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                                {
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT ProviderId From CONTACT where ContactId = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                                }
                                else
                                {
                                    SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                                }
                                SqlCommand.Parameters.AddWithValue("@PaymentNote", note.ToString());
                                SqlCommand.Parameters.AddWithValue("@TransactionType", 3);
                                SqlCommand.Parameters.AddWithValue("@FinancialTransactionId", DiscountId);
                                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Discount Confirmed in EHR for  PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",ProviderEHRId=" + drRow["ProviderEHRId"].ToString() + ",FinancialTransactionId=" + DiscountId.ToString());
                            }
                            catch (Exception)
                            {
                                //deleteduplicates("TransactionDetail", "FinancialTransactionId", FinancialTransactionId);
                                //deleteduplicates("FinancialTransaction", "FinancialTransactionId", FinancialTransactionId);
                                //deleteduplicates("FinancialTransaction", "FinancialTransactionId", DiscountId);
                                //continue;
                            }

                        }
                        #endregion
                    }
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        #region If PaymentMode is Refunded then save entry to TransactionDetail Table only
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchTrackerQRY.InsertPaymentToTransactionDetailForRefund;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                        SqlCommand.Parameters.AddWithValue("@Amount", Convert.ToDecimal(drRow["Amount"].ToString()));
                        if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                        {
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT ProviderId From CONTACT where ContactId = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                        }
                        else
                        {
                            SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                        }

                        SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"].ToString());
                        SqlCommand.Parameters.AddWithValue("@TransactionType", 6);
                        SqlCommand.Parameters.AddWithValue("@PaymentNote", note);
                        SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Payment To Transaction Detail For Refund Confirmed in EHR for PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",PaymentMode =" + drRow["PaymentMode"].ToString().ToUpper() + " on Payment date=" + drRow["PaymentDate"].ToString());

                        #endregion
                    }
                }
                #endregion  
                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 1 || Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 3)
                {
                    EHRLogId = Save_PatientPaymentLog_LocalToTracker(drRow, DbString, ServiceInstallationId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                #endregion  
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 0)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "Sync Log and Payment is disabled from Adit App", FinancialTransactionId.ToString(), EHRLogId.ToString(), CareCreditMethodId.ToString(), "Sync Log and Payment is disabled from Adit App", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", FinancialTransactionId.ToString(), EHRLogId.ToString(), DiscountId.ToString(), "", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
            catch (Exception ex1)
            {
                Utility.WriteToErrorLogFromAll("Error during Payment Method");
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", ex1.Message.ToString(), "", "", "", ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]));
            }

           
        }
        public static void SaveAditPayPaymentToEHR(string DbString, string ServiceInstallationId,DataRow drRow, decimal amount,Int64 MethodId, decimal balance, string note,Int64 DiscountTypeId, string _filename_EHR_Payment, string _EHRLogdirectory_EHR_Payment, string EHRLogId)
        {
            Int64 FinancialTransactionId = 0;
            EHRLogId = "0";
            Int64 DiscountId = 0;
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            string sqlSelect = string.Empty;

            try
            {
                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 2 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                {
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        #region get bankid for patient
                        Int64 bankid = 0;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchTrackerQRY.GetBankIdFromFinancialTransaction;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                        bankid = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        if (bankid > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get BankId From Financial Transaction for PatientEHRId=" + drRow["PatientEHRId"].ToString() + " and get bankid=" + bankid.ToString());
                        }
                        #endregion

                        #region save payment to FinancialTransaction table
                        if (amount != 0)
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchTrackerQRY.InsertPaymentToFinancialTransaction;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                            SqlCommand.Parameters.AddWithValue("@Amount", Convert.ToDecimal(drRow["Amount"]) - Convert.ToDecimal(drRow["Discount"]));
                            SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"].ToString());
                            SqlCommand.Parameters.AddWithValue("@PaymentNote", drRow["template"].ToString());
                            SqlCommand.Parameters.AddWithValue("@ChequeNumber", drRow["ChequeNumber"].ToString());
                            SqlCommand.Parameters.AddWithValue("@BankId", bankid);
                            SqlCommand.Parameters.AddWithValue("@MethodId", MethodId);
                            SqlCommand.Parameters.AddWithValue("@TransactionType", 2);
                            SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                            SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                            FinancialTransactionId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                            if (FinancialTransactionId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Payment To Financial Transaction PatientEHRId=" + drRow["PatientEHRId"].ToString() + ", and BankId=" + bankid.ToString() + " and MethodId=" + MethodId.ToString() + " get FinancialTransactionId=" + FinancialTransactionId.ToString());
                            }
                        }
                        #endregion

                        #region If FinancialTransactionId is greater then save entry to TransactionDetail table
                        if (FinancialTransactionId > 0)
                        {
                            try
                            {
                                SqlCommand.CommandText = SynchTrackerQRY.InsertDiscount;   //InsertDiscount;//insertPaymentToTDetail; //entry to transactiondetail
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                // SqlCommand.Parameters.AddWithValue("@Amount", -amount); 
                                SqlCommand.Parameters.AddWithValue("@DiscountAmount", amount);
                                if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                                {
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT ProviderId From CONTACT where ContactId = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                                }
                                else
                                {
                                    SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                                }
                                SqlCommand.Parameters.AddWithValue("@PaymentNote", note.ToString());
                                if (balance > 0)
                                {
                                    SqlCommand.Parameters.AddWithValue("@TransactionType", 2);
                                }
                                else
                                {
                                    SqlCommand.Parameters.AddWithValue("@TransactionType", 10);
                                }

                                SqlCommand.Parameters.AddWithValue("@FinancialTransactionId", FinancialTransactionId);
                                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Discount Confirmed in local for PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",ProviderEHRId=" + drRow["ProviderEHRId"].ToString() + ",FinancialTransactionId=" + FinancialTransactionId.ToString());
                                // }

                            }
                            catch (Exception ex)
                            {
                                //deleteduplicates("FinancialTransaction", "FinancialTransactionId", FinancialTransactionId);
                                //continue;
                            }

                        }
                        #endregion

                        #region If discount is greater then zero then save entry to FinancialTransaction and TransactionDetail Table
                        if (Convert.ToDecimal(drRow["Discount"]) > 0)//&& balance > 0)
                        {
                            try
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchTrackerQRY.InsertPaymentToFinancialTransaction;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                SqlCommand.Parameters.AddWithValue("@Amount", Convert.ToDecimal(drRow["Discount"]));
                                SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"].ToString());
                                SqlCommand.Parameters.AddWithValue("@PaymentNote", drRow["template"].ToString());
                                SqlCommand.Parameters.AddWithValue("@ChequeNumber", drRow["ChequeNumber"].ToString());
                                SqlCommand.Parameters.AddWithValue("@BankId", bankid);
                                SqlCommand.Parameters.AddWithValue("@TransactionType", 3);
                                SqlCommand.Parameters.AddWithValue("@MethodId", DiscountTypeId);
                                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                DiscountId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                if (DiscountId > 0)
                                {
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Payment To Financial Transaction  Confirmed in EHR for PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",BankId=" + bankid.ToString() + ",MethodId=" + DiscountTypeId.ToString());
                                }

                            }
                            catch (Exception)
                            {
                                //deleteduplicates("TransactionDetail", "FinancialTransactionId", FinancialTransactionId);
                                //deleteduplicates("FinancialTransaction", "FinancialTransactionId", FinancialTransactionId);
                                //continue;
                            }

                            try
                            {
                                SqlCommand.CommandText = SynchTrackerQRY.InsertDiscount;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                SqlCommand.Parameters.AddWithValue("@DiscountAmount", Convert.ToDecimal(drRow["Discount"]));
                                if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                                {
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT ProviderId From CONTACT where ContactId = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                                }
                                else
                                {
                                    SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                                }
                                SqlCommand.Parameters.AddWithValue("@PaymentNote", note.ToString());
                                SqlCommand.Parameters.AddWithValue("@TransactionType", 3);
                                SqlCommand.Parameters.AddWithValue("@FinancialTransactionId", DiscountId);
                                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Discount Confirmed in EHR for  PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",ProviderEHRId=" + drRow["ProviderEHRId"].ToString() + ",FinancialTransactionId=" + DiscountId.ToString());
                            }
                            catch (Exception)
                            {
                                //deleteduplicates("TransactionDetail", "FinancialTransactionId", FinancialTransactionId);
                                //deleteduplicates("FinancialTransaction", "FinancialTransactionId", FinancialTransactionId);
                                //deleteduplicates("FinancialTransaction", "FinancialTransactionId", DiscountId);
                                //continue;
                            }

                        }
                        #endregion

                    }
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        #region If PaymentMode is Refunded then save entry to TransactionDetail Table only
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchTrackerQRY.InsertPaymentToTransactionDetailForRefund;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                        SqlCommand.Parameters.AddWithValue("@Amount", Convert.ToDecimal(drRow["Amount"].ToString()));
                        if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                        {
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT ProviderId From CONTACT where ContactId = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                        }
                        else
                        {
                            SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                        }

                        SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"].ToString());
                        SqlCommand.Parameters.AddWithValue("@TransactionType", 6);
                        SqlCommand.Parameters.AddWithValue("@PaymentNote", note);
                        SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Payment To Transaction Detail For Refund Confirmed in EHR for PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",PaymentMode =" + drRow["PaymentMode"].ToString().ToUpper() + " on Payment date=" + drRow["PaymentDate"].ToString());

                        #endregion
                    }
                }
                #endregion

                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                {
                    EHRLogId = Save_PatientPaymentLog_LocalToTracker(drRow, DbString, ServiceInstallationId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                #endregion  
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 0)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "Sync Log and Payment is disabled from Adit App", FinancialTransactionId.ToString(), EHRLogId.ToString(), DiscountId.ToString(), "Sync Log and Payment is disabled from Adit App", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }

                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", FinancialTransactionId.ToString(), EHRLogId.ToString(), DiscountId.ToString(), "", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
            catch (Exception ex1)
            {
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", ex1.Message.ToString(), "", "", "", ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]));
            }

        }
        public static int GetContactPhoneId(int PatientId, int PhoneType)
        {
            int ContactPhoneId = 0;
            SqlConnection conn = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (PatientId != 0)
                {
                    string sqlSelect = "Select contactphoneid as newId from ContactPhone where phonetype = " + PhoneType + " and contactid = " + PatientId;
                    SqlCommand SqlCommand = new SqlCommand();
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    ContactPhoneId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                }
                if (ContactPhoneId == null || ContactPhoneId == 0)
                {
                    string sqlSelect = "insert into ContactPhone (ContactId, PhoneType, PhoneNumber, Extension, IsDefault, IsSmsEnabled, IsLongDistance, "
                                        + " PhoneDescription, PhoneNote) values ( " + PatientId + "," + PhoneType + ",'0000000000',NULL,0,0,0,NULL,NULL ) select @@identity ";
                    SqlCommand SqlCommand = new SqlCommand();
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    ContactPhoneId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                ContactPhoneId = 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return ContactPhoneId;
        }
        public static bool Save_Patient_Form_Local_To_Tracker(DataTable dtWebPatient_Form)
        {
            string _successfullstataus = string.Empty;
            bool is_Record_Update = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                //OdbcCommand.CommandTimeout = 200;
                string strQauery = string.Empty;
                string Update_PatientForm_Record_ID = "";
                string updateColumnList = "";
                Int64 patient_EHR_Id = 0;
                string strQaueryContactPhone = "";
                string receivesms = "1";
                Int64 patientPhoneContactId = 0, patientMobileContactId = 0;
                DataTable dtTrackerPatientColumns = GetTrackerTableColumnName("Patient", dtWebPatient_Form);

                if (dtWebPatient_Form.AsEnumerable()
                    .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty &&
                           Convert.ToInt64(o.Field<object>("Patient_EHR_ID")) > 0
                           ).Count() > 0)
                {

                    dtWebPatient_Form.AsEnumerable()
                    .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                           o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                           o.Field<object>("PatientForm_Web_ID") != null &&
                           o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty &&
                           Convert.ToInt64(o.Field<object>("Patient_EHR_ID")) > 0
                           )
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {

                             patient_EHR_Id = 0;
                             foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                             {
                                 patient_EHR_Id = Convert.ToInt64(dr["Patient_EHR_ID"].ToString());
                                 if (dr["ehrfield"].ToString().Trim() != string.Empty && dr["ehrfield_value"].ToString().Trim() != string.Empty)
                                 {
                                     if (dr["ehrfield"].ToString().Trim().ToUpper() != "PRIMARY_SUBSCRIBER_ID" && dr["ehrfield"].ToString().Trim().ToUpper() != "PRIMARY_INSURANCE_COMPANYNAME" && dr["ehrfield"].ToString().Trim().ToUpper() != "SECONDARY_SUBSCRIBER_ID" && dr["ehrfield"].ToString().Trim().ToUpper() != "SECONDARY_INSURANCE_COMPANYNAME" && dr["ehrfield"].ToString().Trim().ToUpper() != "ISSMSENABLED" && dr["ehrfield"].ToString().Trim().ToUpper() != "SECONDARY_INSURANCE_COMPANYNAME" && dr["ehrfield"].ToString().Trim().ToUpper() != "ISSMSENABLED")
                                     {
                                         if (dr["ehrfield"].ToString().Trim().ToUpper() == "PHONENUMBER" || dr["ehrfield"].ToString().Trim().ToUpper() == "WORKPHONENUMBER" ||
                                                           dr["ehrfield"].ToString().Trim().ToUpper() == "MOBILENUMBER")
                                         {
                                             string updateContactPhone = " Update ContactPhone SET PHONENUMBER = '@PHONENUMBER' where contactphoneid = @contactphoneid";
                                             Int64 ContactPhoneId = 0;
                                             if (dr["ehrfield"].ToString().Trim().ToUpper() == "PHONENUMBER")
                                             {
                                                 ContactPhoneId = GetContactPhoneId(Convert.ToInt32(dr["Patient_EHR_ID"].ToString()), 1);

                                             }
                                             if (dr["ehrfield"].ToString().Trim().ToUpper() == "MOBILENUMBER")
                                             {
                                                 updateContactPhone = " Update ContactPhone SET PHONENUMBER = '@PHONENUMBER' , IsSmsEnabled = 1 , IsDefault = 1  where contactphoneid = @contactphoneid";
                                                 ContactPhoneId = GetContactPhoneId(Convert.ToInt32(dr["Patient_EHR_ID"].ToString()), 3);

                                             }
                                             if (dr["ehrfield"].ToString().Trim().ToUpper() == "WORKPHONENUMBER")
                                             {
                                                 ContactPhoneId = GetContactPhoneId(Convert.ToInt32(dr["Patient_EHR_ID"].ToString()), 2);

                                             }
                                             updateContactPhone = updateContactPhone.Replace("@PHONENUMBER", dr["ehrfield_value"].ToString().Trim());
                                             updateContactPhone = updateContactPhone.Replace("@contactphoneid", ContactPhoneId.ToString());
                                             CommonDB.SqlServerCommand(updateContactPhone, conn, ref SqlCommand, "txt");
                                             SqlCommand.Parameters.Clear();
                                             SqlCommand.ExecuteNonQuery();
                                         }
                                         else if (dr["ehrfield"].ToString().Trim().ToString().ToUpper() == "COMPANY")
                                         {
                                             string UpdateCompanyDetail = "";
                                             if (dr["pformfield"].ToString().Trim().ToString().ToUpper() == "SCHOOL")
                                             {
                                                 UpdateCompanyDetail = "Update Contact SET IsStudent = 1,company = '" + dr["ehrfield_value"] + "' where ContactId = " + patient_EHR_Id.ToString() + "";
                                             }
                                             else if (dr["pformfield"].ToString().Trim().ToString().ToUpper() == "EMPLOYER")
                                             {
                                                 UpdateCompanyDetail = "Update Contact SET IsStudent = 0,company = '" + dr["ehrfield_value"] + "' where ContactId = " + patient_EHR_Id.ToString() + "";
                                             }
                                             CommonDB.SqlServerCommand(UpdateCompanyDetail, conn, ref SqlCommand, "txt");
                                             SqlCommand.Parameters.Clear();
                                             SqlCommand.ExecuteNonQuery();
                                         }
                                         else
                                         {

                                             var resultColumns = dtTrackerPatientColumns.AsEnumerable().Where(a => a.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().Trim().ToString().ToUpper());
                                             if (resultColumns.Count() > 0)
                                             {
                                                 updateColumnList = " Update Contact SET ";
                                                 updateColumnList = updateColumnList + "[" + dr["ehrfield"].ToString().Trim() + "]" + " = ";
                                                 if (dr["ehrfield"].ToString().Trim().ToString().ToUpper() == "HASEMAILCONSENT" || dr["ehrfield"].ToString().Trim().ToString().ToUpper() == "SEX")
                                                 {
                                                     if (dr["ehrfield"].ToString().Trim().ToString().ToUpper() == "SEX")
                                                     {
                                                         updateColumnList = updateColumnList + "'" + AssigneValueCompitibleTOEHR(dr["ehrfield_value"].ToString().Trim(), dr["ehrfield"].ToString().Trim().ToString().ToUpper()) + "'" + ",";
                                                     }
                                                     else
                                                     {
                                                         updateColumnList = updateColumnList + AssigneValueCompitibleTOEHR(dr["ehrfield_value"].ToString().Trim(), dr["ehrfield"].ToString().Trim().ToString().ToUpper()) + ",";
                                                     }
                                                 }
                                                 else
                                                 {
                                                     updateColumnList = updateColumnList + GetValueSizeWise(dtTrackerPatientColumns, dr["ehrfield"].ToString().Trim().ToString().ToUpper(), dr["ehrfield_value"].ToString().Trim()) + ",";
                                                 }
                                                 updateColumnList = updateColumnList.Substring(0, updateColumnList.Length - 1);

                                                 updateColumnList = updateColumnList + " Where ContactId = " + patient_EHR_Id.ToString() + "";
                                                 CommonDB.SqlServerCommand(updateColumnList, conn, ref SqlCommand, "txt");
                                                 SqlCommand.Parameters.Clear();
                                                 SqlCommand.ExecuteNonQuery();

                                             }


                                         }
                                     }
                                     #region old Code for update
                                     //if (Convert.ToInt32(dr["Patient_EHR_ID"].ToString()) != 0)
                                     //{
                                     //    strQauery = SynchTrackerQRY.Update_Patinet_Record_By_Patient_Form;
                                     //    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                     //    SqlCommand.CommandText = strQauery;
                                     //    SqlCommand.Parameters.Clear();                                         
                                     //    SqlCommand.Parameters.AddWithValue("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                     //    if (dr["ehrfield"].ToString().Trim().ToUpper() == "HOME_PHONE" || dr["ehrfield"].ToString().Trim().ToUpper() == "WORK_PHONE" || dr["ehrfield"].ToString().Trim().ToUpper() == "CELL_PHONE")
                                     //    {
                                     //        phoneNo = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");
                                     //        SqlCommand.Parameters.AddWithValue("@ehrfield_value", "'" + phoneNo + "'");
                                     //    }
                                     //    else
                                     //    {
                                     //        SqlCommand.Parameters.AddWithValue("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                                     //    }
                                     //    SqlCommand.ExecuteNonQuery();
                                     //}
                                     #endregion
                                 }
                             }

                             UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim());
                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";
                             return true;
                         });
                }

                #region Insert Patient Into Tracker

                patient_EHR_Id = 0;
                string PrimaryInsuraceCompanyName = "";
                string SecondaryInsuraceCompanyName = "";
                string PrimaryInsuraceSubScriberId = "";
                string SecondaryInsuraceSubScriberId = "";

                if (
                dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {
                    dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             receivesms = "1";
                             strQauery = CreatePatientInsertQuery(dtWebPatient_Form, dtTrackerPatientColumns, o.ToString(), "Patient", ref PrimaryInsuraceCompanyName, ref SecondaryInsuraceCompanyName, ref PrimaryInsuraceSubScriberId, ref SecondaryInsuraceSubScriberId);

                             strQauery = strQauery + " SELECT @@Identity ";

                             if (conn.State == ConnectionState.Closed) conn.Open();
                             CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                             SqlCommand.Parameters.Clear();
                             patient_EHR_Id = Convert.ToInt64(Convert.ToInt64(SqlCommand.ExecuteScalar()));
                             #region update clientid
                             strQauery = "UPDATE Contact SET ClientId = " + patient_EHR_Id + " WHERE ContactId = " + patient_EHR_Id + "";
                             CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                             SqlCommand.ExecuteNonQuery();
                             #endregion

                             #region Insert Records into ContactPhone

                             if (Convert.ToInt64(patient_EHR_Id) > 0)
                             {
                                 var patientisSmsReceive = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "ISSMSENABLED");
                                 if (patientisSmsReceive.Count() > 0)
                                 {
                                     var resultSMSReceive = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "ISSMSENABLED").Select(b => b.Field<string>("ehrfield_value")).First();
                                     if (resultSMSReceive != null && resultSMSReceive.ToString() != string.Empty)
                                     {
                                         receivesms = AssigneValueCompitibleTOEHR(resultSMSReceive.ToString(), "ISSMSENABLED");
                                     }
                                 }
                                 var patientPhoneNo = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "PHONENUMBER");
                                 if (patientPhoneNo.Count() > 0)
                                 {
                                     var resultPhoneNo = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "PHONENUMBER").Select(b => b.Field<string>("ehrfield_value")).First();
                                     if (resultPhoneNo != null && resultPhoneNo.ToString() != string.Empty)
                                     {
                                         strQaueryContactPhone = "Insert INto ContactPhone (ContactId,PhoneType,PhoneNumber,Extension,IsDefault,IsSmsEnabled,IsLongDistance,PhoneDescription,PhoneNote) "
                                                                + " values ( " + patient_EHR_Id + ",1,'" + resultPhoneNo.ToString() + "',NULL,1," + receivesms + ",0,NULL,NULL) SELECT @@Identity";
                                     }
                                 }
                                 if (strQaueryContactPhone != "")
                                 {
                                     CommonDB.SqlServerCommand(strQaueryContactPhone, conn, ref SqlCommand, "txt");
                                     SqlCommand.Parameters.Clear();
                                     if (conn.State != ConnectionState.Open)
                                     {
                                         conn.Open();
                                     }
                                     patientPhoneContactId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                     strQaueryContactPhone = "";
                                 }

                                 var patientMobileNo = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "MOBILENUMBER");
                                 if (patientMobileNo.Count() > 0)
                                 {
                                     var resultMobileNo = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "MOBILENUMBER").Select(b => b.Field<string>("ehrfield_value")).First();
                                     if (resultMobileNo != null && resultMobileNo.ToString() != string.Empty)
                                     {
                                         strQaueryContactPhone = "Insert INto ContactPhone (ContactId,PhoneType,PhoneNumber,Extension,IsDefault,IsSmsEnabled,IsLongDistance,PhoneDescription,PhoneNote) "
                                             + " values ( " + patient_EHR_Id + ",3,'" + resultMobileNo.ToString() + "',NULL," + (patientPhoneNo.Count() > 0 ? "0" : "1") + "," + (patientPhoneNo.Count() > 0 ? "0" : receivesms) + ",0,NULL,NULL) SELECT @@Identity  ";
                                     }
                                 }
                                 if (strQaueryContactPhone != "")
                                 {
                                     CommonDB.SqlServerCommand(strQaueryContactPhone, conn, ref SqlCommand, "txt");
                                     SqlCommand.Parameters.Clear();
                                     patientMobileContactId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                     strQaueryContactPhone = "";
                                 }
                                 var patientWokrPhoneNo = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "WORKPHONENUMBER");
                                 if (patientWokrPhoneNo.Count() > 0)
                                 {
                                     var resultMobileNo = dtWebPatient_Form.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID").ToString() == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "WORKPHONENUMBER").Select(b => b.Field<string>("ehrfield_value")).First();
                                     if (resultMobileNo != null && resultMobileNo.ToString() != string.Empty)
                                     {
                                         strQaueryContactPhone = "Insert INto ContactPhone (ContactId,PhoneType,PhoneNumber,Extension,IsDefault,IsSmsEnabled,IsLongDistance,PhoneDescription,PhoneNote) "
                                             + " values ( " + patient_EHR_Id + ",2,'" + resultMobileNo.ToString() + "',NULL,0,0,0,NULL,NULL) SELECT @@Identity  ";
                                     }
                                 }
                                 if (strQaueryContactPhone != "")
                                 {
                                     CommonDB.SqlServerCommand(strQaueryContactPhone, conn, ref SqlCommand, "txt");
                                     SqlCommand.Parameters.Clear();
                                     patientMobileContactId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                     strQaueryContactPhone = "";
                                 }
                                 strQaueryContactPhone = SynchTrackerQRY.UpdatePatientDefaultPhone;
                                 CommonDB.SqlServerCommand(strQaueryContactPhone, conn, ref SqlCommand, "txt");
                                 SqlCommand.Parameters.Clear();
                                 SqlCommand.Parameters.AddWithValue("@ContactId", patient_EHR_Id);
                                 SqlCommand.Parameters.AddWithValue("@DefaultPhoneId", (patientPhoneNo.Count() > 0 ? patientPhoneContactId : patientMobileContactId));
                                 SqlCommand.ExecuteNonQuery();
                                 //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Update Patient Default Phone for patient_EHR_Id=" + patient_EHR_Id + ",DefaultPhoneId=" + (patientPhoneNo.Count() > 0 ? patientPhoneContactId : patientMobileContactId));
                                 strQaueryContactPhone = "";
                                 UpdatePatientInsurance(PrimaryInsuraceCompanyName, patient_EHR_Id, 1, patient_EHR_Id, PrimaryInsuraceSubScriberId);
                                 UpdatePatientInsurance(SecondaryInsuraceCompanyName, patient_EHR_Id, 2, patient_EHR_Id, SecondaryInsuraceSubScriberId);
                             }
                             #endregion

                             UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim());

                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";

                             return true;
                         });
                }
                #endregion

                SynchLocalDAL.UpdatePatientFormEHR_Updateflg(dtWebPatient_Form);
            }
            catch (Exception ex)
            {
                is_Record_Update = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return is_Record_Update;
        }

        #region Treatment Document
        public static bool Save_TreatmentDocument_Form_Local_To_Tracker(string strTreatmentPlanID = "")
        {    
            int DocId = 0;
            bool IsDocUpdate = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLocaleTreatmentDocData(strTreatmentPlanID);       
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    TrackerDocPath("1");
                   // Utility.CheckEntryUserLoginIdExist();
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
                    }
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string destPatientDocPath = "";
                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            //rooja
                            string DocPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\ENVIRONMENT", "TrackerMedia", null);
                            if (DocPath != null)
                            {
                                // Do stuff
                                destPatientDocPath = DocPath + @"\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            }
                            else
                            {
                                destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            }
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        }


                        //string destPatientDocPath = "";
                        //if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        //{
                        //    destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        //}
                        //else
                        //{
                        //    destPatientDocPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        //}
                        string sourceLocation = CommonUtility.GetAditTreatmentDocTempPath() + "\\" + dr["TreatmentDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            PullLiveDatabaseDAL.Update_TreatrmentDocNotFound_Live_To_Local(dr["TreatmentPlanId"].ToString());
                            continue;
                        }

                        string dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }
                        string tmpFileOrgName = Path.GetFileName(sourceLocation);
                        System.IO.File.Copy(sourceLocation, dstnLocation, true);
                        //Guid tmpGUID = Guid.NewGuid();
                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;
                        string RenameFileName = dr["TreatmentDoc_Name"].ToString();


                        string sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");

                        // I have to change only last field cat number 
                        string QryDoc = SynchTrackerQRY.InsertPatientTreatmentDocument;
                        string details = dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim();
                        SqlCommand.CommandText = QryDoc;
                        SqlCommand.Parameters.Clear();

                        SqlCommand.Parameters.AddWithValue("Type", "Treatment Document");
                        SqlCommand.Parameters.AddWithValue("ContactId", dr["Patient_EHR_ID"].ToString().Trim());
                        SqlCommand.Parameters.AddWithValue("Message", details);
                        SqlCommand.Parameters.AddWithValue("AttachmentFilePath", dr["Patient_EHR_ID"].ToString().Trim() + @"\" + tmpFileOrgName.ToString());
                        SqlCommand.Parameters.AddWithValue("ModifiedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("ModifiedTimeStamp", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("CreatedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("CreatedTimeStamp", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("ModifiedMachineName", Environment.MachineName.ToString());
                        SqlCommand.Parameters.AddWithValue("CreatedMachineName", "Sync");
                        SqlCommand.Parameters.AddWithValue("SerializedTeeth", "0");
                        SqlCommand.Parameters.AddWithValue("IsActive", "True");
                        SqlCommand.Parameters.AddWithValue("FileType", "1");
                        SqlCommand.ExecuteNonQuery();


                        string QryIdentity = "Select @@Identity as newId from Activity";
                        SqlCommand.CommandText = QryIdentity;
                        SqlCommand.CommandType = CommandType.Text;
                        DocId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        PullLiveDatabaseDAL.Update_TreatmentFormDoc_Local_To_EHR(dr["TreatmentDoc_Web_ID"].ToString(), DocId.ToString());
                        File.Delete(sourceLocation);
                    }
                }
                IsDocUpdate = true;
            }
            catch (Exception ex)
            {
                DocId = 0;
                IsDocUpdate = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsDocUpdate;
        }
        #endregion

        #region Insurance Carrier
        public static bool Save_InsuranceCarrierDocument_Form_Local_To_Tracker(string strInsuranceCarrierID = "")
        {
            int DocId = 0;
            bool IsDocUpdate = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLocaleInsuranceCarrierDocData(strInsuranceCarrierID);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    TrackerDocPath("1");
                    // Utility.CheckEntryUserLoginIdExist();
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
                    }
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string destPatientDocPath = "";
                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            //rooja
                            string DocPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\ENVIRONMENT", "TrackerMedia", null);
                            if (DocPath != null)
                            {
                                // Do stuff
                                destPatientDocPath = DocPath + @"\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            }
                            else
                            {
                                destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            }
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        }


                        //string destPatientDocPath = "";
                        //if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        //{
                        //    destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        //}
                        //else
                        //{
                        //    destPatientDocPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        //}
                        string sourceLocation = CommonUtility.GetAditInsuranceCarrierDocTempPath() + "\\" + dr["InsuranceCarrier_Doc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            PullLiveDatabaseDAL.Update_InsuranceCarrierDocNotFound_Live_To_Local(dr["InsuranceCarrier_Doc_Web_ID"].ToString());
                            continue;
                        }

                        string dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }
                        string tmpFileOrgName = Path.GetFileName(sourceLocation);
                        System.IO.File.Copy(sourceLocation, dstnLocation, true);
                        //Guid tmpGUID = Guid.NewGuid();
                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;
                        string RenameFileName = dr["InsuranceCarrier_Doc_Name"].ToString();


                        string sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");

                        // I have to change only last field cat number 
                        string QryDoc = SynchTrackerQRY.InsertPatientInsuranceCarrierDocument;
                        string details = dr["InsuranceCarrier_Doc_Name"].ToString().Trim();
                        SqlCommand.CommandText = QryDoc;
                        SqlCommand.Parameters.Clear();

                        SqlCommand.Parameters.AddWithValue("Type", "InsuranceCarrier Document");
                        SqlCommand.Parameters.AddWithValue("ContactId", dr["Patient_EHR_ID"].ToString().Trim());
                        SqlCommand.Parameters.AddWithValue("Message", details);
                        SqlCommand.Parameters.AddWithValue("AttachmentFilePath", dr["Patient_EHR_ID"].ToString().Trim() + @"\" + tmpFileOrgName.ToString());
                        SqlCommand.Parameters.AddWithValue("ModifiedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("ModifiedTimeStamp", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("CreatedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("CreatedTimeStamp", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("ModifiedMachineName", Environment.MachineName.ToString());
                        SqlCommand.Parameters.AddWithValue("CreatedMachineName", "Sync");
                        SqlCommand.Parameters.AddWithValue("SerializedTeeth", "0");
                        SqlCommand.Parameters.AddWithValue("IsActive", "True");
                        SqlCommand.Parameters.AddWithValue("FileType", "1");
                        SqlCommand.ExecuteNonQuery();


                        string QryIdentity = "Select @@Identity as newId from Activity";
                        SqlCommand.CommandText = QryIdentity;
                        SqlCommand.CommandType = CommandType.Text;
                        DocId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        PullLiveDatabaseDAL.Update_InsuranceCarrierDoc_Local_To_EHR(dr["InsuranceCarrier_Doc_Web_ID"].ToString(), DocId.ToString());
                        File.Delete(sourceLocation);
                    }
                }
                IsDocUpdate = true;
            }
            catch (Exception ex)
            {
                DocId = 0;
                IsDocUpdate = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsDocUpdate;
        }
        #endregion


        #region Privider OfficeHours & CustomHour

        public static DataSet GetProviderCustomHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetProviderCustomHours;
                SqlSelect = SqlSelect.Replace("@ToDate", ToDate.ToString("yyyy/MM/dd"));
                SqlSelect = SqlSelect.Replace("@TrackerScheduleInterval", Utility.TrackerScheduleInterval.ToString());
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandTimeout = 200;
                //SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataSet SqlDt = new DataSet();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetOperatoryCustomHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetOperatoryCustomHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static DataTable GetTrackerProviderOfficeHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            DataTable dtProvider = new DataTable();
            DataTable OdbcDt = new DataTable();
            DataTable dtProviderOfficeHours = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetProviderOfficeHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                dtProviderOfficeHours = SqlDt.Clone();
                dtProvider = SynchLocalDAL.GetLocalProviderData("", "1");
                foreach (DataRow dr in dtProvider.Rows)
                {
                    DataTable dtTemp = new DataTable();
                    dtTemp = SqlDt.Copy();
                    foreach (DataRow dr1 in dtTemp.Rows)
                    {
                        dr1["POH_EHR_ID"] = dr1["POH_EHR_ID"] + "_" + dr["Provider_EHR_ID"];
                        dr1["Provider_EHR_ID"] = dr["Provider_EHR_ID"];
                    }
                    dtProviderOfficeHours.Load(dtTemp.CreateDataReader());
                }
                return dtProviderOfficeHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static DataTable GetTrackerOperatoryOfficeHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            DataTable dtOperatory = new DataTable();
            DataTable OdbcDt = new DataTable();
            DataTable dtOperatoryOfficeHours = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetOperatoryOfficeHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                dtOperatoryOfficeHours = SqlDt.Clone();
                dtOperatory = SynchLocalDAL.GetLocalOperatoryData("1");
                foreach (DataRow dr in dtOperatory.Rows)
                {
                    DataTable dtTemp = new DataTable();
                    dtTemp = SqlDt.Copy();
                    foreach (DataRow dr1 in dtTemp.Rows)
                    {
                        dr1["OOH_EHR_ID"] = dr1["OOH_EHR_ID"] + "_" + dr["Operatory_EHR_ID"];
                        dr1["Operatory_EHR_ID"] = dr["Operatory_EHR_ID"];
                    }
                    dtOperatoryOfficeHours.Load(dtTemp.CreateDataReader());
                }
                return dtOperatoryOfficeHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        #endregion

        public static string Save_PatientPaymentLog_LocalToTracker(DataRow drRow, string DbString, string ServiceInstallationId, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
            }
            string noteId = "";
            SqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            SqlCommand SqlCommand = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;

                try
                {
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchTrackerQRY.InsertPatientNotes;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                    //SqlCommand.Parameters.AddWithValue("@NoteType", 4);
                    SqlCommand.Parameters.AddWithValue("@NoteType", "Financial");
                    if (drRow["template"].ToString().Length > 500)
                    {
                        SqlCommand.Parameters.AddWithValue("@PatientNote", drRow["template"].ToString().Substring(0, 499));
                        SqlCommand.Parameters.AddWithValue("@Comments", drRow["template"]);
                    }
                    else
                    {
                        SqlCommand.Parameters.AddWithValue("@PatientNote", drRow["template"]);
                        SqlCommand.Parameters.AddWithValue("@Comments", "");
                    }
                    //SqlCommand.Parameters.AddWithValue("@PatientNote", drRow["template"].ToString());
                    SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                    SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                    SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                    noteId = SqlCommand.ExecuteScalar().ToString();
                    if (noteId != "")
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Notes with  PatientEHRId=" + drRow["PatientEHRId"] + ",NoteType=Financial,CreatedUserId=" + Utility.EHR_UserLogin_ID);
                    }
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", ServiceInstallationId.Trim(), drRow["Clinic_Number"].ToString().Trim(), "", noteId, "", "", "", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                catch (Exception ex1)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", ServiceInstallationId.Trim(), drRow["Clinic_Number"].ToString().Trim(), ex1.Message, noteId, "", "", ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
                return noteId;

            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static string Save_PatientSMSCallLog_LocalToTracker(DataTable dtWebPatientPayment, string DbString, string ServiceInstallationId)
        {
            string noteId = "";
            SqlConnection conn = null;
            
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
            }
            //MySqlCommand MySqlCommand = new MySqlCommand();
            SqlCommand SqlCommand = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                if (!dtWebPatientPayment.Columns.Contains("Log_Status"))
                {
                    dtWebPatientPayment.Columns.Add("Log_Status", typeof(string));
                }
                DataTable dtResultCopy = new DataTable();
                //Utility.WriteToSyncLogFile_All("Call to save records to EHR_DLL");
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    dtResultCopy = dtWebPatientPayment.Select("Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' and Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                    foreach (DataRow drRow in dtResultCopy.Rows)
                    {
                        if (drRow["PatientEHRId"] != null && drRow["PatientEHRId"].ToString() != string.Empty && drRow["PatientEHRId"].ToString() != "")
                        {
                            try
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchTrackerQRY.InsertPatientNotes;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                                //SqlCommand.Parameters.AddWithValue("@NoteType", 0);
                                SqlCommand.Parameters.AddWithValue("@NoteType", "Other");
                                if (drRow["text"].ToString().Length > 500)
                                {
                                    SqlCommand.Parameters.AddWithValue("@PatientNote", drRow["text"].ToString().Substring(0, 499));
                                    SqlCommand.Parameters.AddWithValue("@Comments", drRow["text"]);
                                }
                                else
                                {
                                    SqlCommand.Parameters.AddWithValue("@PatientNote", drRow["text"]);
                                    SqlCommand.Parameters.AddWithValue("@Comments", "");
                                }
                                SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PatientSMSCallLogDate"]);
                                SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                //Utility.WriteToSyncLogFile_All("Query To be executed " + SqlCommand.CommandText);
                                noteId = SqlCommand.ExecuteScalar().ToString();
                                //Utility.WriteToSyncLogFile_All("Query Executed");
                                drRow["LogEHRId"] = noteId.ToString();
                                drRow["Log_Status"] = "completed";

                            }
                            catch (Exception ex1)
                            {
                                // Utility.WriteToSyncLogFile_All("Error to save " + ex1.ToString());
                                if (ex1.InnerException.Message.Length >= 100)
                                {
                                    drRow["Log_Status"] = "Err_" + ex1.InnerException.Message.Substring(0, 100);
                                }
                                else
                                {
                                    drRow["Log_Status"] = "Err_" + ex1.InnerException.Message.ToString();
                                }
                                noteId = "";
                            }
                        }
                    }
                    //Utility.WriteToSyncLogFile_All("Records Inserted");
                    if (dtResultCopy.Rows.Count > 0)
                    {
                        //Utility.WriteToSyncLogFile_All("Call First Function to completed");
                        try
                        {
                            if (dtResultCopy.Select("LogType = 0").Count() > 0)
                            {
                                //Utility.WriteToSyncLogFile_All("ApptType zero records exists ");
                                SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy.Select("LogType = 0").CopyToDataTable(), "completed", "1", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                            }
                            // Utility.WriteToSyncLogFile_All("Call Second Function to completed");
                            if (dtResultCopy.Select("LogType = 1").Count() > 0)
                            {
                                // Utility.WriteToSyncLogFile_All("ApptType one records exists ");
                                SynchLocalDAL.CallPatientFollowUpStatusCompleted(dtResultCopy.Select("LogType = 1").CopyToDataTable(), "completed", ServiceInstallationId.Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                            }
                        }
                        catch (Exception ex2)
                        {
                            Utility.WriteToSyncLogFile_All("ApptType zero records exists " + ex2.ToString());
                        }
                    }
                }
                return noteId.ToString();
            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Document_in_Tracker(string strPatientFormID = "")             
        {           
            int DocId = 0;
            bool IsDocUpdate = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocData("1", strPatientFormID);            
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    TrackerDocPath("1");
                    //Utility.CheckEntryUserLoginIdExist();
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
                    }
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string destPatientDocPath = "";
                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            //rooja
                            string DocPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\ENVIRONMENT", "TrackerMedia", null);
                            if (DocPath != null)
                            {
                                // Do stuff
                                destPatientDocPath = DocPath + @"\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            }
                            else
                            {
                                destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            }
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        }
                        string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            if (SynchLocalDAL.CheckLivePatientFormDocDataSynced("1", dr["PatientDoc_Web_ID"].ToString()))
                            {
                                PullLiveDatabaseDAL.Update_PatientDocNotFound_Live_To_Local(dr["PatientDoc_Web_ID"].ToString(), "1");
                            }
                            continue;
                        }

                        string dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }
                        string tmpFileOrgName = Path.GetFileName(sourceLocation);
                        System.IO.File.Copy(sourceLocation, dstnLocation, true);
                        //Guid tmpGUID = Guid.NewGuid();
                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;
                        string RenameFileName = dr["PatientDoc_Name"].ToString();


                        string sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");

                        // I have to change only last field cat number 
                        string QryDoc = SynchTrackerQRY.InsertPatientFormDocData;


                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM/dd/yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM/dd/yy");
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";
                        FormName = FormName.Length > 50 ? FormName.Substring(0, 49) : FormName;
                        switch (dr["DocNameFormat"].ToString().Trim())
                        {
                            case "DS-FN-PN":
                                //SubmitedDate +"-"+
                                ShowingName = SubmitedDate + "-" + FormName + "-" + PatientName;
                                break;
                            case "DS-PN-FN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + PatientName + "-" + FormName;
                                break;
                            case "DS-PN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + PatientName;
                                break;
                            case "DS-FN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + FormName;
                                break;
                            case "DS":
                                ShowingName = SubmitedDate;
                                break;
                            default:
                                ShowingName = SubmitedDate;
                                break;
                        }

                        //string details = DateTime.Now.ToString("MM/dd/yyyy") + "-" + dr["Form_Name"].ToString().Trim() + "-" + dr["Patient_Name"].ToString().Trim();
                        SqlCommand.CommandText = QryDoc;
                        SqlCommand.Parameters.Clear();

                        SqlCommand.Parameters.AddWithValue("Type", FormName);
                        SqlCommand.Parameters.AddWithValue("ContactId", dr["Patient_EHR_ID"].ToString().Trim());
                        SqlCommand.Parameters.AddWithValue("Message", ShowingName);
                        SqlCommand.Parameters.AddWithValue("AttachmentFilePath", dr["Patient_EHR_ID"].ToString().Trim() + @"\" + tmpFileOrgName.ToString());
                        SqlCommand.Parameters.AddWithValue("ModifiedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("ModifiedTimeStamp", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("CreatedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("CreatedTimeStamp", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("ModifiedMachineName", Environment.MachineName.ToString());
                        SqlCommand.Parameters.AddWithValue("CreatedMachineName", Environment.MachineName.ToString());
                        SqlCommand.Parameters.AddWithValue("SerializedTeeth", "0");
                        SqlCommand.Parameters.AddWithValue("IsActive", "True");
                        SqlCommand.Parameters.AddWithValue("FileType", "3");
                        SqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from Activity";
                        SqlCommand.CommandText = QryIdentity;
                        SqlCommand.CommandType = CommandType.Text;
                        DocId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), DocId.ToString(), "1");
                        File.Delete(sourceLocation);
                        Save_DocumentAttachment_in_Tracker(dr["PatientDoc_Web_ID"].ToString());
                    }
                }
                IsDocUpdate = true;
            }
            catch (Exception ex)
            {
                DocId = 0;
                IsDocUpdate = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsDocUpdate;

            //return true;
        }

        public static bool Save_DocumentAttachment_in_Tracker(string PatientForm_web_Id)
        {           
            int DocId = 0;
            bool IsDocUpdate = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocAttachmentData("1", PatientForm_web_Id);      
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                   // TrackerDocPath("1");
                    //Utility.CheckEntryUserLoginIdExist();
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
                    }
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string destPatientDocPath = "";
                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            //rooja
                            string DocPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\ENVIRONMENT", "TrackerMedia", null);
                            if (DocPath != null)
                            {
                                // Do stuff
                                destPatientDocPath = DocPath + @"\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            }
                            else
                            {
                                destPatientDocPath = @"C:\Dental\Tracker\Docs\Saved\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                            }
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        }
                        string sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            if (SynchLocalDAL.CheckLivePatientFormDocAttachmentDataSynced("1", dr["PatientDoc_Web_ID"].ToString()))
                            {
                                PullLiveDatabaseDAL.Update_PatientDocAttachmentNotFound_Live_To_Local(dr["PatientForm_web_Id"].ToString(), "1");
                            }
                            continue;
                        }
                        else
                        {
                            long length = new System.IO.FileInfo(sourceLocation).Length;
                            if (length <= 0)
                            {                               
                                PullLiveDatabaseDAL.Update_PatientDocAttachmentNotFound_Live_To_Local(dr["PatientForm_web_Id"].ToString(), "1");
                                File.Delete(sourceLocation);
                                continue;
                            }
                        }

                        string dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }
                        string tmpFileOrgName = Path.GetFileName(sourceLocation);
                        System.IO.File.Copy(sourceLocation, dstnLocation, true);
                        //Guid tmpGUID = Guid.NewGuid();
                        FileInfo fi = new FileInfo(dstnLocation);
                        string FileExtension = fi.Extension;
                        string RenameFileName = dr["PatientDoc_Name"].ToString();


                        string sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");

                        // I have to change only last field cat number 
                        string QryDoc = SynchTrackerQRY.InsertPatientFormDocData;


                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM/dd/yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM/dd/yy");
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";
                        FormName = FormName.Length > 50 ? FormName.Substring(0, 49) : FormName;
                        switch (dr["DocNameFormat"].ToString().Trim())
                        {
                            case "FN-PN-DS":
                                //SubmitedDate +"-"+
                                ShowingName = FormName + "-" + PatientName + "-" + SubmitedDate;
                                break;
                            case "DS-FN-PN":
                                //SubmitedDate +"-"+
                                ShowingName = SubmitedDate + "-" + FormName + "-" + PatientName;
                                break;
                            case "DS-PN-FN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + PatientName + "-" + FormName;
                                break;
                            case "DS-PN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + PatientName;
                                break;
                            case "DS-FN":
                                //SubmitedDate + "-" +
                                ShowingName = SubmitedDate + "-" + FormName;
                                break;
                            case "DS":
                                ShowingName = SubmitedDate;
                                break;
                            default:
                                ShowingName = SubmitedDate;
                                break;
                        }

                        //string details = DateTime.Now.ToString("MM/dd/yyyy") + "-" + dr["Form_Name"].ToString().Trim() + "-" + dr["Patient_Name"].ToString().Trim();
                        SqlCommand.CommandText = QryDoc;
                        SqlCommand.Parameters.Clear();

                        SqlCommand.Parameters.AddWithValue("Type", FormName);
                        SqlCommand.Parameters.AddWithValue("ContactId", dr["Patient_EHR_ID"].ToString().Trim());
                        SqlCommand.Parameters.AddWithValue("Message", ShowingName);
                        SqlCommand.Parameters.AddWithValue("AttachmentFilePath", dr["Patient_EHR_ID"].ToString().Trim() + @"\" + tmpFileOrgName.ToString());
                        SqlCommand.Parameters.AddWithValue("ModifiedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("ModifiedTimeStamp", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("CreatedUserId", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("CreatedTimeStamp", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("ModifiedMachineName", Environment.MachineName.ToString());
                        SqlCommand.Parameters.AddWithValue("CreatedMachineName", Environment.MachineName.ToString());
                        SqlCommand.Parameters.AddWithValue("SerializedTeeth", "0");
                        SqlCommand.Parameters.AddWithValue("IsActive", "True");
                        SqlCommand.Parameters.AddWithValue("FileType", "3");
                        SqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from Activity";
                        SqlCommand.CommandText = QryIdentity;
                        SqlCommand.CommandType = CommandType.Text;
                        DocId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        PullLiveDatabaseDAL.Update_PatientFormDocAttachment_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), DocId.ToString(), "1");
                        File.Delete(sourceLocation);
                    }
                }
                IsDocUpdate = true;
            }
            catch (Exception ex)
            {
                DocId = 0;
                IsDocUpdate = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsDocUpdate;

            //return true;
        }

        public static bool SavePatientMedicationLocalToTracker(ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            string sqlSelect = "";
            Int64 MedicationPatientId;
            DataTable dtPatientMedication;
            SavePatientEHRID = "";
            try
            {               
                dtPatientMedication = GetTrackerPatientMedicationData("");

                CommonDB.ClearDentSQLConnectionServer(ref conn);
                if (conn.State == ConnectionState.Closed) conn.Open();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR(Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), strPatientFormID);
                    if (dtPatientMedicationResponse != null)
                    {
                        if (dtPatientMedicationResponse.Rows.Count > 0)
                        {
                            //Utility.CheckEntryUserLoginIdExist();
                            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                            {
                                Utility.EHR_UserLogin_ID = GetTrackerUserLoginId();
                            }

                            foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                            {
                                MedicationPatientId = 0;
                                string MedicationNote = "Medication: " + dr["Medication_Name"].ToString() + ", Comment:" + dr["Medication_Note"].ToString();
                                string MedicationNoteRTF = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Segoe UI;}}  \viewkind4\uc1\pard\f0\fs20 ' " + MedicationNote + @" '\par  }";

                                if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "0";
                                if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" && dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                                {
                                    MedicationPatientId = Convert.ToInt64(dr["PatientMedication_EHR_Id"]);
                                }

                                if (MedicationPatientId <= 0)
                                {
                                    DataRow[] drPatMedRow = dtPatientMedication.Copy().Select("Patient_EHR_ID = " + dr["PatientEHRId"].ToString().Trim() + " And Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "' And Is_Active=true ");
                                    if (drPatMedRow.Length > 0)
                                    {
                                        MedicationPatientId = Convert.ToInt64(drPatMedRow[0]["PatientMedication_EHR_ID"].ToString().Trim());
                                    }
                                }

                                if (MedicationPatientId <= 0)
                                {
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchTrackerQRY.InsertPatientMedication;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", dr["PatientEHRId"]);
                                    SqlCommand.Parameters.AddWithValue("@NoteType", 8);
                                    SqlCommand.Parameters.AddWithValue("@PatientNote", MedicationNote);
                                    SqlCommand.Parameters.AddWithValue("@PatientNoteRTF", MedicationNoteRTF);
                                    SqlCommand.Parameters.AddWithValue("@CreatedUserId", Utility.EHR_UserLogin_ID);
                                    SqlCommand.Parameters.AddWithValue("@ModifiedUserId", Utility.EHR_UserLogin_ID);
                                    MedicationPatientId = Convert.ToInt64(SqlCommand.ExecuteScalar());

                                    DataRow NewRow = dtPatientMedication.NewRow();
                                    NewRow["Patient_EHR_ID"] = dr["PatientEHRID"].ToString();
                                    NewRow["Medication_EHR_ID"] = "0";
                                    NewRow["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                    NewRow["Medication_Name"] = dr["Medication_Name"].ToString();
                                    //NewRow["Medication_Type"] = "Drug";
                                    NewRow["Drug_Quantity"] = "0";
                                    NewRow["Medication_Note"] = dr["Medication_Note"].ToString();
                                    NewRow["Provider_EHR_ID"] = "0";
                                    NewRow["is_active"] = "True";

                                    dtPatientMedication.Rows.Add(NewRow);
                                    dtPatientMedication.AcceptChanges();
                                }
                                else
                                {
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchTrackerQRY.UpdatePatientMedicationNotes;
                                    SqlCommand.Parameters.Clear();
                                    //SqlCommand.Parameters.AddWithValue("@PatientEHRId", dr["PatientEHRId"]);
                                    SqlCommand.Parameters.AddWithValue("@NoteType", 8);
                                    SqlCommand.Parameters.AddWithValue("@PatientNote", MedicationNote);
                                    SqlCommand.Parameters.AddWithValue("@PatientMedication_EHR_ID", MedicationPatientId);
                                    SqlCommand.ExecuteNonQuery();
                                }
                                if (!SavePatientEHRID.ToUpper().Trim().Contains("'" + dr["PatientEHRID"].ToString().Trim() + "'"))
                                {
                                    SavePatientEHRID = SavePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                                }
                                SynchLocalDAL.UpdateMedicationEHR_Updateflg(dr["MedicationResponse_Local_ID"].ToString(), MedicationPatientId.ToString(), dr["PatientEHRID"].ToString(), "0", "1");
                            }
                            isRecordSaved = true;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool DeletePatientMedicationLocalToTracker(ref bool isRecordDeleted, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            string sqlSelect = "";
            DeletePatientEHRID = "";
            try
            {
                CommonDB.ClearDentSQLConnectionServer(ref conn);
                if (conn.State == ConnectionState.Closed) conn.Open();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationRemovedResponseToSaveINEHR(Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), strPatientFormID);
                    if (dtPatientMedicationResponse != null)
                    {
                        if (dtPatientMedicationResponse.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchTrackerQRY.DeletePatientMedication;
                                SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@PatientEHRId", dr["PatientEHRId"].ToString());
                                SqlCommand.Parameters.AddWithValue("@PatientMedication_EHR_Id", dr["PatientMedication_EHR_Id"].ToString());
                                SqlCommand.Parameters.AddWithValue("@NoteType", 8);
                                SqlCommand.ExecuteNonQuery();
                                if (!DeletePatientEHRID.ToUpper().Trim().Contains("'" + dr["PatientEHRID"].ToString().Trim() + "'"))
                                {
                                    DeletePatientEHRID = DeletePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                                }
                                SynchLocalDAL.UpdateRemovedMedicationEHR_Updateflg(dr["MedicationRemovedResponse_Local_ID"].ToString(), dr["PatientMedication_EHR_Id"].ToString(), dr["PatientEHRID"].ToString(), "1");
                            }
                            isRecordDeleted = true;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetTrackerPatientMedicationData(string Patient_EHR_IDS)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            //DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                {
                    SqlSelect = SynchTrackerQRY.GetTrackerPatientMedicationData;
                }
                else
                {
                    SqlSelect = SynchTrackerQRY.GetTrackerPatientMedicationData + " And ContactID in (" + Patient_EHR_IDS + ")";
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate.ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                foreach (DataRow row in SqlDt.Rows)
                {
                    string strPatientNotes = row["Patient_Notes"].ToString().Trim();
                    if (strPatientNotes.Contains("Medication:") && strPatientNotes.Contains(", Comment:"))
                    {
                        string strMedicationName = strPatientNotes.Substring(0, strPatientNotes.IndexOf(", Comment:"));
                        string strComment = strPatientNotes.Substring(strPatientNotes.IndexOf(", Comment:"), (strPatientNotes.Length - strPatientNotes.IndexOf(", Comment:")));
                        row["Medication_Name"] = strMedicationName.Replace("Medication:", "").ToString().Trim();
                        row["Patient_Notes"] = strComment.Replace(", Comment:", "").ToString().Trim();
                        row["Medication_Note"] = strComment.Replace(", Comment:", "").ToString().Trim();
                    }
                    else
                    {
                        row["Medication_Name"] = strPatientNotes.ToString().Trim();
                        row["Patient_Notes"] = "";
                    }
                }
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable Save_NoteDataMoveToCorrespondInTracker(DataTable dtNote, string DbString)
        {
            SqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            SqlCommand SqlCommand = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                for (int i = 0; i < dtNote.Rows.Count; i++)
                {
                    string sqlSelect = string.Empty;
                    try
                    {
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchTrackerQRY.NoteDataMoveToCorrespondInTracker;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@NoteId", dtNote.Rows[i]["LogEHRId"]); ;
                        dtNote.Rows[i]["NewLogEHRId"] = Convert.ToString(SqlCommand.ExecuteScalar());
                        dtNote.AcceptChanges();
                    }
                    catch (Exception ex1)
                    {
                        throw ex1;
                    }
                }
                return dtNote;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        #region Insurance 
        public static DataTable GetTrackerInsuranceData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerInsuranceData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                //return SqlDt;
                //rooja 20-8-24
                //MySqlDa.Fill(MySqlDt);

                DataTable Dttmp = SqlDt.Clone();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == "1")
                    {
                        for (int j = 0; j < SqlDt.Rows.Count; j++)
                        {
                            DataRow Dr = Dttmp.NewRow();
                            Dr.ItemArray = SqlDt.Rows[j].ItemArray;
                            Dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                            Dttmp.Rows.Add(Dr);
                        }
                    }
                }
                return Dttmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Insurance_Tracker_To_Local(DataTable dtTrackerInsurance, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtTrackerInsurance.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Insurance;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Insurance;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Insurance;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Insurance_EHR_ID", dr["CarrierId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Name", dr["Company"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address", dr["Address1"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address2", dr["Address2"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("State", dr["RegionId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Zipcode", dr["PostalCode"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Phone", dr["PhoneNumber"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ElectId", dr["UniqueId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EmployerName", "");
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr["IsActive"].ToString().Trim() == "1" || dr["IsActive"].ToString().Trim().ToLower() == "true" ? false : true);
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    // SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        #endregion

        #region TrackerDocPath
        public static void TrackerDocPath(string Service_Install_Id)
        {
            try
            {
                bool regmismatch = false;
                string destPatientDocPath = "";
                bool EHROption_PatDoc_set = false;
                string PatientDocsLocationPath = "";
                string DocPath = "";

                DataTable dtTrackerPatientDocsLocationPath = GetTrackerPatDocLocation();
                if (dtTrackerPatientDocsLocationPath != null)
                {
                    if (dtTrackerPatientDocsLocationPath.Rows.Count > 0)
                    {
                        PatientDocsLocationPath = dtTrackerPatientDocsLocationPath.Rows[0]["ApplicationOptionValue"].ToString().Trim();
                        if (PatientDocsLocationPath != "")
                        {
                            DocPath = PatientDocsLocationPath.ToString().Trim();
                            EHROption_PatDoc_set = true;
                        }
                        else
                        {
                            DocPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\ENVIRONMENT", "TrackerMedia", null);
                        }

                    }
                }
                if (EHROption_PatDoc_set)
                {
                    if (DocPath != null)
                    {
                        DataTable dtService_InstallData = SystemDAL.GetInstallServeiceDetail();
                        if (dtService_InstallData != null)
                        {
                            if (dtService_InstallData.Rows.Count > 0)
                            {
                                destPatientDocPath = DocPath;
                                if (dtService_InstallData.Rows[0]["Document_Path"].ToString().ToLower() != destPatientDocPath.ToString().ToLower())
                                {
                                    regmismatch = true;
                                }
                            }
                        }
                    }
                    if (regmismatch)
                    {
                        if (!string.IsNullOrEmpty(destPatientDocPath))
                        {
                            Utility.EHRDocPath = destPatientDocPath;
                            SystemDAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, Service_Install_Id);
                        }
                        else
                        {
                            Utility.WriteToErrorLogFromAll("EHR Doc Path : Patient Document Path Not Found. DocPath : '" + DocPath + "', destPatientDocPath : '" + destPatientDocPath + "'");
                        }
                    }
                }
                else
                {
                    DocPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\ENVIRONMENT", "TrackerMedia", null);
                    if (DocPath != null)
                    {
                        DataTable dtService_InstallData = SystemDAL.GetInstallServeiceDetail();
                        if (dtService_InstallData != null)
                        {
                            if (dtService_InstallData.Rows.Count > 0)
                            {
                                destPatientDocPath = DocPath + @"\Docs\Saved\";
                                if (dtService_InstallData.Rows[0]["Document_Path"].ToString().ToLower() != destPatientDocPath.ToString().ToLower())
                                {
                                    regmismatch = true;
                                }
                            }
                        }
                    }
                    if (regmismatch)
                    {
                        if (!string.IsNullOrEmpty(destPatientDocPath))
                        {
                            Utility.EHRDocPath = destPatientDocPath;
                            SystemDAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, Service_Install_Id);
                        }
                        else
                        {
                            Utility.WriteToErrorLogFromAll("EHR Doc Path : Patient Document Path Not Found. DocPath : '" + DocPath + "', destPatientDocPath : '" + destPatientDocPath + "'");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("EHR Doc Path : Patient Document Path Not Found." + ex.Message);
            }
        }
        #endregion

        //rooja 22-5-24 custom fix for The Dental Group of Galesburg(Galesburg)
        public static DataTable GetTrackerPatDocLocation()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchTrackerQRY.GetTrackerPatientDocLocation;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return SqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
    }
}

