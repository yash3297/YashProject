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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using IniParser;
using IniParser.Model;
using System.Threading.Tasks;

namespace Pozative.DAL
{
    public class SynchAbelDentDAL
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
                else if (mode.ToUpper() == "DELETE" && (tableName == "AbelDent_Form" || tableName == "AbelDent_FormQuestion" || tableName == "EagleSoftAlertMaster" || tableName == "EagleSoftFormMaster" || tableName == "EagleSoftSectionItemMaster" || tableName == "EagleSoftSectionItemOptionMaster" || tableName == "EagleSoftSectionMaster" || tableName == "Appointment" || tableName == "OperatoryEvent" || tableName == "ProviderHours" || tableName == "OperatoryHours" || tableName == "ProviderOfficeHours" || tableName == "OperatoryOfficeHours" || tableName == "CD_FormMaster" || tableName == "CD_QuestionMaster" || tableName == "Holiday" || tableName == "OD_SheetDef" || tableName == "OD_SheetFieldDef" || tableName == "EasyDental_Question"))
                {
                    StrSqlQuery = " UPDATE " + tableName + " SET IS_Deleted = 1,IS_Adit_Updated = 0 WHERE IS_Deleted = 0 and " + primaryKeyColumnsName + " = " + dtRow[primaryKeyColumnsName];
                }
                return StrSqlQuery;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetAbelDentMedicationData()
        {

            //DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                string SqlSelect = "";
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if (Utility.Application_Version == "15")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMedicationMaster_15;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMedicationMaster;
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static DataTable GetAbelDentPatientMedicationData(string Patient_EHR_IDS)
        {

            //DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                if (Utility.Application_Version == "15")
                {
                    if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentPatientMedicationData_15;
                    }
                    else
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentPatientMedicationData_15 + " where ClinicalPatient.LegacyPID in (" + Patient_EHR_IDS + ")";
                    }
                }
                else
                {
                    if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentPatientMedicationData;
                    }
                    else
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentPatientMedicationData + " where ClinicalPatient.LegacyPID in (" + Patient_EHR_IDS + ")";
                    }
                }

                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static bool Save_Provider_AbelDent_To_Local(DataTable dtAbelDentDataToSave)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string sqlSelect = string.Empty;
                    string Provider_Speciality = "";
                    foreach (DataRow dr in dtAbelDentDataToSave.Rows)
                    {
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Provider;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Provider;
                                        break;
                                }

                                try
                                {
                                    Provider_Speciality = dr["Provider_Speciality"].ToString();
                                    if (Provider_Speciality.Length > 10)
                                    {
                                        Provider_Speciality = dr["Provider_Speciality"].ToString().Trim().Substring(12, dr["Provider_Speciality"].ToString().Trim().Length - 12);
                                    }
                                }
                                catch (Exception)
                                {
                                    Provider_Speciality = "";
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Provider_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Last_Name", "");
                                SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("MI", "");
                                SqlCeCommand.Parameters.AddWithValue("gender", "");
                                SqlCeCommand.Parameters.AddWithValue("Provider_Speciality", Provider_Speciality);
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("is_active", Convert.ToBoolean(dr["is_active"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
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

        public static void ExecuteQuery(string InsertTableName, DataRow dr, SqlCeCommand SqlCeCommand)
        {
            try
            {
                string sqlSelect = string.Empty;
                //string MaritalStatus = string.Empty;
                string Status = string.Empty;
                string tmpBirthDate = string.Empty;
                // decimal curPatientcollect_payment = 0;
                string tmpReceive_Sms_Email = string.Empty;
                string Gender = "Unknown";
                string MaritalStatus = "Single";

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

                //tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());
                tmpBirthDate = dr["birth_date"].ToString().Trim();

                if (tmpBirthDate != "")
                {
                    tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
                    if (tmpBirthDate == "01/01/1753")
                        tmpBirthDate = "01/01/0001";
                }

                try
                {
                    if (dr["Primary_Insurance"].ToString().Trim() == "0")
                        dr["Primary_Insurance"] = "" ;
                    if (dr["Guar_ID"].ToString().Trim() == "0")
                        dr["Guar_ID"] = "";
                }
                catch (Exception ex)
                {

                }

                try
                {
                    Gender = dr["Sex"].ToString().Trim();
                }
                catch (Exception ex)
                { Gender = ""; }

                try
                {
                    MaritalStatus = dr["MaritalStatus"].ToString().Trim();

                }
                catch (Exception ex)
                { MaritalStatus = ""; }


                try
                {
                    Status = dr["Status"].ToString().Trim();

                }
                catch (Exception ex)
                { Status = ""; }

                //if (Status == "" || Status == "1")
                //{ Status = "A"; }
                //else
                //{ Status = "I"; }
                //Utility.WriteToSyncLogFile_All("PatientName " + dr["First_name"].ToString() + " " + dr["Last_name"].ToString() + " Gender " + Gender + " MaritalStatus " + MaritalStatus + " dr " + dr["sex"].ToString() + " " + dr["MaritalStatus"].ToString() );

                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", Convert.ToInt64(dr["patient_ehr_id"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
                SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Status", Status);
                SqlCeCommand.Parameters.AddWithValue("Sex", Gender);
                SqlCeCommand.Parameters.AddWithValue("MaritalStatus", MaritalStatus);
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
                SqlCeCommand.Parameters.AddWithValue("CurrentBal", dr["CurrentBal"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("NinetyDay",dr["NinetyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString());
                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dr["ReceiveSms"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", dr["ReceiveEmail"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", dr["Secondary_Ins_Subscriber_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", dr["Primary_Ins_Subscriber_ID"].ToString());
                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                SqlCeCommand.Parameters.AddWithValue("EHR_Status", dr["EHR_Status"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveVoiceCall", dr["ReceiveVoiceCall"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("PreferredLanguage", dr["PreferredLanguage"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                SqlCeCommand.Parameters.AddWithValue("Patient_Note", dr["Patient_Note"].ToString().Trim().Length > 3000 ? dr["Patient_Note"].ToString().Substring(0, 3000).Trim() : dr["Patient_Note"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ssn", dr["ssn"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("driverlicense", dr["driverlicense"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("groupid", dr["groupid"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("collect_payment", dr["collect_payment"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", dr["remaining_benefit"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("used_benefit", dr["used_benefit"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("emergencycontactId", dr["emergencycontactId"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("EmergencyContact_First_Name", dr["EmergencyContact_First_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("EmergencyContact_Last_Name", dr["EmergencyContact_Last_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("emergencycontactnumber", dr["emergencycontactnumber"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("school", dr["school"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("employer", dr["employer"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("spouseId", dr["spouseId"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Spouse_First_Name", dr["Spouse_First_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Spouse_Last_Name", dr["Spouse_Last_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("responsiblepartyId", dr["responsiblepartyId"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("responsiblepartyssn", dr["responsiblepartyssn"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("responsiblepartybirthdate", dr["responsiblepartybirthdate"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_First_Name", dr["ResponsibleParty_First_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Last_Name", dr["ResponsibleParty_Last_Name"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Prim_Ins_Company_Phonenumber", dr["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Sec_Ins_Company_Phonenumber", dr["Sec_Ins_Company_Phonenumber"].ToString().Trim());
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

        public static bool Save_Patient_AbelDent_To_Local(DataTable dtAbelDentPatient, string Service_Install_Id)
        {
            bool _successfullstataus = true;
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
                        foreach (DataRow dr in dtAbelDentPatient.Rows)
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

                                //Insert
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
                                //rec.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
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
                                rs.Insert(rec);

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

        public static DataTable GetAbelDentPatient_Balance()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 600;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentPatientBalance_15;
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

        public static DataTable GetAbelDentApt_Data(string appt_EHR_id)
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 600;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "Select * from Apt where aidentifier = '" + appt_EHR_id + "'";
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

        public static DataTable GetAbelDentPatient_LastVisitDate()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 600;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAelDentLastVisit_Date;
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


        public static bool GetAbelDentConnection()
        {

            SqlConnection conn = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            bool IsConnected = false;
            try
            {
                conn.Open();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                #region Check user and create user if not created
                Console.WriteLine(ex.Message);
                #endregion

                IsConnected = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsConnected;
        }

        public static DataTable GetAbelDentAppointmentData(string strApptID = "",string _minutesInUnit = "15")
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;
            SqlDataAdapter SqlDa = null;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                if (!string.IsNullOrEmpty(strApptID))
                {
                    if (Utility.Application_Version == "12.10.6")
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentDataByApptID_12_10_6;
                    }
                    else if(Utility.Application_Version == "15")
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentDataByApptID_15;
                    }
                    else
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentDataByApptID;
                    }
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                else
                {
                    if (Utility.Application_Version == "12.10.6")
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentData_12_10_6;
                    }
                    else if (Utility.Application_Version == "15")
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentData_15;
                    }
                    else
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentData;
                    }
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                SqlCommand.Parameters.Add("@_minutesInUnit", SqlDbType.NVarChar).Value = _minutesInUnit;
                if (!string.IsNullOrEmpty(strApptID))
                {
                    SqlCommand.Parameters.Add("@Appt_EHR_ID", SqlDbType.NVarChar).Value = strApptID;
                }
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

        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime dateTime,int _minutesInUnit)
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;
            SqlDataAdapter SqlDa = null;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.getAppointmentDateTimeWise;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@DateTime", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                SqlCommand.Parameters.Add("@_minutesInUnit", SqlDbType.Int).Value = _minutesInUnit;
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

        public static DataTable GetAbelDentAppointment_Procedures_Data(string strApptID = "")
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 600;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                if (!string.IsNullOrEmpty(strApptID))
                {
                    SqlSelect = SynchAbelDentQRY.AbelDentAppointment_Procedures_DataByApptID;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.AbelDentAppointment_Procedures_Data;
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

        public static DataTable GetAbelDentAppointmentIds()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentEhrIds;
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

        ////public static DataTable GetAbelDentDeletedAppointmentData()
        ////{
        ////    DateTime ToDate = Utility.LastSyncDateAditServer;

        ////    SqlConnection conn = null;
        ////    SqlCommand SqlCommand = new SqlCommand();
        ////    SqlDataAdapter SqlDa = null;
        ////    CommonDB.AbelDentSQLConnectionServer(ref conn);
        ////    try
        ////    {
        ////        SqlCommand.CommandTimeout = 200;
        ////        if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string SqlSelect = SynchAbelDentQRY.GetAbelDentDeletedAppointmentData;
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
        ////public static DataTable GetAbelDentDeletedOperatoryEventData()
        ////{
        ////    DateTime ToDate = Utility.LastSyncDateAditServer;

        ////    SqlConnection conn = null;
        ////    SqlCommand SqlCommand = new SqlCommand();
        ////    SqlDataAdapter SqlDa = null;
        ////    CommonDB.AbelDentSQLConnectionServer(ref conn);
        ////    try
        ////    {
        ////        SqlCommand.CommandTimeout = 200;
        ////        if (conn.State == ConnectionState.Closed) conn.Open();
        ////        string SqlSelect = SynchAbelDentQRY.GetAbelDentDeletedOperatoryEventData;
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

        public static DataTable GetAbelDentProviderData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable SqlDt = null;

                string SqlSelect = SynchAbelDentQRY.GetAbelDentProviderData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                SqlDt = new DataTable();
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

        public static DataTable GetAbelDentDefaultProviderData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentDefaultProviderData;
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

        public static bool Save_Operatory_AbelDent_To_Local(DataTable dtAbelDentOperatory, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //        SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtAbelDentOperatory.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Operatory;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Operatory;
                                        break;
                                    case 4:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_False_Operatory;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("OperatoryOrder", dr["OperatoryOrder"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    //SqlCetx.Commit();
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

        public static DataTable GetAbelDentOperatoryData()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 500;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentOperatoryData;
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

        public static DataTable GetAbelDentOperatoryOfficeHours()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 500;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetOperatoryOfficeHours;
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

        public static DataTable GetAbelDentOperatoryOfficeHoursOP()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 500;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetOperatoryOfficeHoursOP;
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

        public static DataTable GetAbelDentProviderOfficeHours()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 500;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetProviderOfficeHours;
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

        public static DataTable GetAbelDentProviderOfficeHoursOP()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 500;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetProviderOfficeHoursOP;
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

        public static DataTable GetAbelDentSpecialityData()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentSpecialty;
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

        public static DataTable GetAbelDentApptTypeData()
        {
            DataTable dtApptType = new DataTable();
            try
            {
                // SELECT 0 AS Clinic_Number,1 as Service_Install_Id,'' AS ApptType_LocalDB_ID, Id AS ApptType_EHR_ID,'' AS ApptType_Web_ID, Name AS[Type_Name],0 AS Is_Adit_Updated from WorkToDoGlobal WITH(NOLOCK)
                int i = 1;
                string basePath = "";
                if (Utility.EHRDocPath != "")
                {
                    basePath = Directory.GetParent(Directory.GetParent(Utility.EHRDocPath).FullName).FullName;
                }
                else
                {
                    basePath = @"C:\ABELDent\";
                }
                string iniFilePath = "" + basePath + "\\sched.ini";


                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(iniFilePath);
                var wtdSection = data["WTD"];

                dtApptType.Columns.Add("ApptType_LocalDB_ID", typeof(string));
                dtApptType.Columns.Add("ApptType_EHR_ID", typeof(string));
                dtApptType.Columns.Add("Type_Name", typeof(string));
                dtApptType.Columns.Add("Clinic_Number", typeof(string));
                dtApptType.Columns.Add("Service_Install_Id", typeof(string));
                dtApptType.Columns.Add("Is_Adit_Updated", typeof(string));                    

                foreach (var key in wtdSection)
                {
                    DataRow row = dtApptType.NewRow();
                    row["ApptType_LocalDB_ID"] = "";
                    row["ApptType_EHR_ID"] = i++;
                    row["Type_Name"] = key.KeyName;
                    row["Clinic_Number"] = "0";
                    row["Service_Install_Id"] = "1";
                    row["Is_Adit_Updated"] = "0";
                    dtApptType.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtApptType;       
        }

        public static bool Save_PatientAppointment_OpenDental_To_Local(DataTable dtAbelDentPatient, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        foreach (DataRow dtDtxRow in dtAbelDentPatient.Rows)
                        {
                            switch (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()))
                            {
                                case 1:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptPatient;
                                    break;
                                case 2:
                                    if (dtDtxRow["EHR_Status"].ToString().ToLower() == "active")
                                    {
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptPatient;
                                    }
                                    else
                                    {
                                        SqlCeCommand.CommandText = "";
                                    }
                                    break;
                                case 3:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                                    break;
                            }

                            #region SqlCEConnection

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dtDtxRow["patient_ehr_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("First_name", dtDtxRow["First_name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Last_name", dtDtxRow["Last_name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Middle_Name", dtDtxRow["Middle_Name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Status", dtDtxRow["Status"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Email", dtDtxRow["Email"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dtDtxRow["Mobile"].ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dtDtxRow["Home_Phone"].ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dtDtxRow["LastVisit_Date"].ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dtDtxRow["ReceiveSMS"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", "Y");
                            SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dtDtxRow["nextvisit_date"].ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("due_date", Convert.ToString(dtDtxRow["due_date"].ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dtDtxRow["Clinic_Number"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.Parameters.AddWithValue("EHR_Status", dtDtxRow["EHR_Status"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.CheckValidDatetime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("ReceiveVoiceCall", "Y");
                            SqlCeCommand.Parameters.AddWithValue("PreferredLanguage", dtDtxRow["PreferredLanguage"].ToString().Trim());
                            if (SqlCeCommand.Connection.State != ConnectionState.Open)
                                SqlCeCommand.Connection.Open();
                            SqlCeCommand.ExecuteNonQuery();
                            //  }
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    // SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return _successfullstataus;
            }
        }

        public static DataTable GetAbelDentPatientData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            string SqlSelect = string.Empty;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 2000;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if(Utility.Application_Version == "12.10.6")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientDataWithCondition_12_10_6;
                }
                else if(Utility.Application_Version == "15")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientData_15;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientData;
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

        public static DataTable GetAbelDentPatientDataWithCondition(string condition)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            string SqlSelect = string.Empty;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 2000;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if(Utility.Application_Version == "12.10.6")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientDataWithCondition_12_10_6 + condition;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientDataWithCondition + condition;
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

        public static DataTable GetAbelDentPatientIdData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 2000;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentPatientIdData;
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

        public static DataTable GetAbelDentPatientStatusData(string strPatID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                if (!string.IsNullOrEmpty(strPatID))
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientStatusNew_ExistingByPatID;

                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientStatusNew_Existing;
                }

                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                if (!string.IsNullOrEmpty(strPatID))
                {
                    SqlCommand.Parameters.Add("@Patient_EHR_ID", SqlDbType.NVarChar).Value = strPatID;
                }
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

        public static DataTable GetAbelDentAppointmentsPatientData(string strPatID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                if (Utility.Application_Version == "12.10.6")
                {
                    if (!string.IsNullOrEmpty(strPatID))
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentsPatientDataByPatID_12_10_6;
                    }
                    else
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentsPatientData_12_10_6;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(strPatID))
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentsPatientDataByPatID;
                    }
                    else
                    {
                        SqlSelect = SynchAbelDentQRY.GetAbelDentAppointmentsPatientData;
                    }
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate.ToString("yyyy/MM/dd");
                if (!string.IsNullOrEmpty(strPatID))
                {
                    SqlCommand.Parameters.Add("@Patient_EHR_ID", SqlDbType.NVarChar).Value = strPatID;
                }
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


        public static DataTable GetAbelDentPatientListData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentPatientListData;
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

        public static void GetAbelDentTableColumnList(string tableName, ref DataTable dtAbelDentColumns)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);

            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetPatientTableColumnsName;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.Clear();
                //SqlCommand.Parameters.AddWithValue("@TableName", "Contact");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                //DataTable SqlDt = new DataTable();
                SqlDa.Fill(dtAbelDentColumns);
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

        public static bool Save_AbelDent_To_Local(DataTable dtAbelDentDataToSave, string tablename, string ignoreColumnsName, string primaryKeyColumnsName)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string sqlSelect = string.Empty;

                    foreach (DataRow dr in dtAbelDentDataToSave.Rows)
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
                                        SqlCeCommand.CommandText = CreateQueryToInsert("Insert", dtAbelDentDataToSave, dr, tablename, ignoreColumnsName, primaryKeyColumnsName);//"Appt_LocalDB_ID,Appt_Web_ID", ""
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = CreateQueryToInsert("Update", dtAbelDentDataToSave, dr, tablename, ignoreColumnsName, primaryKeyColumnsName);//"Appt_LocalDB_ID,Appt_Web_ID,Appt_EHR_ID", "Appt_LocalDB_ID"
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = CreateQueryToInsert("Delete", dtAbelDentDataToSave, dr, tablename, ignoreColumnsName, primaryKeyColumnsName);//"", "Appt_LocalDB_ID"
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

        public static bool Save_AbelDent_To_Local(DataTable dtAbelDentDataToSave, string InsertTableName, DataTable dtLocalPatient, bool bSetDeleted = false)
        {
            bool _successfullstataus = true;
            SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtAbelDentDataToSave, "0", "1");
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

                        foreach (DataRow dr in dtAbelDentDataToSave.Rows)
                        {
                            if (dr["Primary_Insurance_CompanyName"].ToString() == "" && dr["Secondary_Insurance_CompanyName"].ToString() == "")
                            {
                                //decimal curPatientcollect_payment = 0;
                                //dr["used_benefit"] = curPatientcollect_payment.ToString();
                                //dr["remaining_benefit"] = curPatientcollect_payment.ToString();
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

                        if (bSetDeleted)
                        {
                            IEnumerable<string> PatientEHRIDs = dtAbelDentDataToSave.AsEnumerable().Where(sid => sid["Service_Install_Id"].ToString() == "1").Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
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

        public static DataTable GetAbelDentRecallTypeData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            string SqlSelect = string.Empty;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if (Utility.Application_Version == "12.10.6")
                {
                   SqlSelect = SynchAbelDentQRY.GetAbelDentRecallType_12_10_6;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentRecallType;
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

        public static DataTable GetAbelDentPatient_recall()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentPatientRecall;
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

        public static DataTable GetAbelDentPatientDiseaseData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                string SqlSelect = "";
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if(Utility.Application_Version == "15")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientDiseaseData_15;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentPatientDiseaseData;
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

        public static DataTable GetAbelDentDiseaseData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                string SqlSelect = "";
                if (conn.State == ConnectionState.Closed) conn.Open();
                if (Utility.Application_Version == "15")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentAllergies_15;
                }
                else
                {
                     SqlSelect = SynchAbelDentQRY.GetAbelDentAllergies;
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

        public static DataTable GetAbelDentProblemData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentProblems;
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

        public static bool Save_ApptStatus_AbelDent_To_Local(DataTable dtAbelDentApptStatus, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtAbelDentApptStatus.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_AppointmentStatus;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_AppointmentStatus;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_AppointmentStatus;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_EHR_ID", dr["ApptStatus_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Name", dr["ApptStatus_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptStatus_Type", dr["ApptStatus_Type"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //SqlCetx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;

                    //  SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static DataTable GetAbelDentApptStatusData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentApptStatusData;
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

        public static bool Save_Provider_AbelDent_To_Local(DataTable dtAbelDentProvider, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtAbelDentProvider.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Provider;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Provider;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Provider_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("MI", dr["mi"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("gender", "");
                                SqlCeCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("is_active", Convert.ToInt16(dr["is_active"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", 0);
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


        public static string GetEHR_VersionNumber()
        {
            string version = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetEHRActualVersionAbelDent;
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

        public static bool Save_RecallType_Local_To_Abeldent(DataTable dtAbelDentRecallType)
        {
            bool _successfullstataus = true;
            SqlCeConnection conn = null;
            SqlCeCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer(ref conn);

            SqlCeTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCetx = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;

                CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtAbelDentRecallType.Rows)
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
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_RecallType;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_RecallType;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_RecallType;
                                break;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("RecallType_EHR_ID", dr["RecallType_EHR_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("RecallType_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("RecallType_Name", dr["RecallType_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("RecallType_Descript", dr["RecallType_Descript"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;

                SqlCetx.Rollback();
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static DataTable GetAbelDentHolidaysData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                //  MySqlCommand.CommandTimeout = 120;
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable SqlDt = null;

                string SqlSelect = SynchAbelDentQRY.GetAbelDentHolidayData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                //SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);

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

        public static bool Save_Disease_AbelDent_To_Local(DataTable dtAbeldentDisease, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {

                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtAbeldentDisease.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Disease;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Disease;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Disease;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", Convert.ToInt32(dr["is_deleted"].ToString().Trim()));
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
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

        public static string Save_Patient_Local_To_AbelDent(string LastName, string FirstName, string MiddleName,ref Int64 PatientID,string MobileNo, string Email, string ApptProv, string AppointmentDateTime,int Patient_Gur_id, string OperatoryId, string Birth_Date)
        {
            try
            {                
                //if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                //{
                Utility.EHR_UserLogin_ID = GetAbelDentUserLoginId();
                //}

                Int64 PatientId = 0;
                string PatientUniqID = "";
                //Int64 DefaultPatientId = 0;

                if (LastName.Length > 20)
                    LastName = LastName.Substring(0,20);
                if (FirstName.Length > 16)
                    FirstName = FirstName.Substring(0, 16);
                if (MiddleName.Length > 2)
                    MiddleName = MiddleName.Substring(0, 2);

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
                CommonDB.AbelDentSQLConnectionServer(ref conn);
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string sqlSelect = string.Empty;
                    if (Utility.Application_Version.ToLower() == "14.8.2".ToLower())
                    {
                        sqlSelect = string.Empty;
                        System.Guid guid = System.Guid.NewGuid();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        sqlSelect= "INSERT INTO pat(pid, plname, pfname, pinitial, pdentist, pstreetadr, pcitycode, ppostal, pphone, pworkphn, pworkext, pbirth,"
                                                              + "pchargeto,paptinvl,plastckp,paptflag,pgender,pmrmrs,pbestphone,pshortnotice,pmedical,phygienist,pnormunits,pstatus,plnamcase,pfnamcase,"
                                                              + "passistant,punlisted,pjrsr,pbesttime,presides,paltid,pnativetongue,pstudent,phandicapped,pataptfrom,pataptsto,pataptdays,preferdrid,"
                                                              + "plateflag,ppremedflag,pinactive,pnonpatient,psince,pmailingname,pstreetadr2,WebPassword,PasswordExpiration,pscalinginvl,pscalingunits,pphoneldprefix,"
                                                              + "pworkphnldprefix,ppersonalid,PasswordHash2,Salt2)VALUES(((select MAX(ISNULL(pid,0)) AS pid from pat WITH(NOLOCK)) + 1), '@LastName', '@FirstName','@MName', '@ProviderID','','', "
                                                              + "NULL, @MobileNo, NULL, NULL, @BirthDate, NULL, NULL, NULL, NULL,'','','', NULL, 0, '',0, NULL,1,1, '',0, NULL, NULL, NULL, NULL, NULL,0,0, "
                                                              + "GETDATE(), GETDATE(), NULL, NULL,0,0,0,1,0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, @UniqID)";
                        sqlSelect = sqlSelect.Replace("@LastName", LastName);
                        sqlSelect = sqlSelect.Replace("@FirstName", FirstName);
                        sqlSelect = sqlSelect.Replace("@MName", MiddleName);
                        sqlSelect = sqlSelect.Replace("@MobileNo", MobileNo);
                        sqlSelect = sqlSelect.Replace("@UniqID", guid.ToString());

                        if (Birth_Date == "" || Birth_Date == string.Empty)
                        {
                            sqlSelect = sqlSelect.Replace("@BirthDate", "");
                        }
                        else
                        {
                            sqlSelect = sqlSelect.Replace("@BirthDate", Birth_Date);
                        }
                        if (ApptProv == "")
                        {
                            sqlSelect = sqlSelect.Replace("@ProviderId", "(select top 1 did from dnt)");
                        }
                        else
                        {
                            sqlSelect = sqlSelect.Replace("@ProviderID", ApptProv);
                        }

                        SqlCommand.CommandText = sqlSelect;
                    }
                    else
                    {
                        sqlSelect = string.Empty;
                        System.Guid guid = System.Guid.NewGuid();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        sqlSelect = "INSERT INTO pat (pid,plname,pfname,pinitial,pdentist,pstreetadr,pcitycode,ppostal,pphone,pworkphn,pworkext,pbirth,"
                                                    + "pchargeto,paptinvl,plastckp,paptflag,pgender,pmrmrs,pbestphone,pshortnotice,pmedical,phygienist,pnormunits,pstatus,plnamcase,pfnamcase,"
                                                    + "passistant,punlisted,pjrsr,pbesttime,presides,paltid,pnativetongue,pstudent,phandicapped,pataptfrom,pataptsto,pataptdays,preferdrid,"
                                                    + "plateflag,ppremedflag,pinactive,pnonpatient,psince,pmailingname,pstreetadr2,WebPassword,PasswordExpiration,pscalinginvl,pscalingunits,pphoneldprefix,"
                                                    + "pworkphnldprefix,ppersonalid)VALUES(((select MAX(ISNULL(pid,0)) AS pid from pat WITH(NOLOCK)) + 1), '@LastName', '@FirstName','@MName', '@ProviderID','','', "
                                                    + "NULL, @MobileNo, NULL, NULL, @BirthDate, NULL, NULL, NULL, NULL,'','','', NULL, 0, '',0, NULL,1,1, '',0, NULL, NULL, NULL, NULL, NULL,0,0, "
                                                    + "GETDATE(), GETDATE(), NULL, NULL,0,0,0,1,0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)";

                        sqlSelect = sqlSelect.Replace("@LastName", LastName);
                        sqlSelect = sqlSelect.Replace("@FirstName", FirstName);
                        sqlSelect = sqlSelect.Replace("@MName", MiddleName);
                        sqlSelect = sqlSelect.Replace("@MobileNo", MobileNo);

                        if (Birth_Date == "" || Birth_Date == string.Empty)
                        {
                            sqlSelect = sqlSelect.Replace("@BirthDate", "");
                        }
                        else
                        {
                            sqlSelect = sqlSelect.Replace("@BirthDate", Birth_Date);
                        }
                        if (ApptProv == "")
                        {
                            sqlSelect = sqlSelect.Replace("@ProviderId", "(select top 1 did from dnt)");
                        }
                        else
                        {
                            sqlSelect = sqlSelect.Replace("@ProviderID", ApptProv);
                        }
                        
                        SqlCommand.CommandText = sqlSelect;
                    }

                    SqlCommand.ExecuteNonQuery();

                    string QueryIdentity = "Select MAX(ISNULL(pid,0)) from pat";
                    CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                    PatientID = Convert.ToInt64(SqlCommand.ExecuteScalar());

                    PatientUniqID = Convert.ToString(PatientID);
                    //string QueryIdentity2 = "Select Salt2 from pat where pid = '" + PatientID + "' ";
                    //CommonDB.SqlServerCommand(QueryIdentity2, conn, ref SqlCommand, "txt");
                    //PatientUniqID = Convert.ToString(SqlCommand.ExecuteScalar());

                    sqlSelect = "INSERT INTO inf (infpid,infnote,infemployer,infoccupation,infmedical,infemail,infmobile,infemailpref) VALUES ('" + PatientID.ToString() + "','','','','','" + Email + "','" + MobileNo.ToString() + "',NULL)";
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    PatientID = 0;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return PatientUniqID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetPatientName(string PatientId)
        {
            string patientname = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.AbelDentSQLConnectionServer(ref conn);

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetPatientName;
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

        public static string GetPatientId(string PatientId)
        {
            string patientId = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.AbelDentSQLConnectionServer(ref conn);

            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetPatientUniqeId;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("@PatientId", PatientId);
                patientId = Convert.ToString(SqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return patientId;
        }

        public static string Save_Patient_UniqID(int PatientID)
        {
            try
            {
                string PatientUniqID = string.Empty;
                SqlConnection conn = null;
                SqlCommand SqlCommand = null;
                CommonDB.AbelDentSQLConnectionServer(ref conn);
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string QueryIdentity = "Select Salt2 from pat where pid = '" + PatientID + "' ";
                    CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                    PatientUniqID = Convert.ToString(SqlCommand.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    PatientUniqID = "";
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return PatientUniqID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool Save_PatientDisease_AbelDent_To_Local(DataTable dtAbelDentPatientDisease, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {

                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtAbelDentPatientDisease.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_PatientDisease;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientDisease;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_PatientDisease;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_EHR_ID", dr["Disease_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Name", dr["Disease_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Disease_Type", dr["Disease_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", dr["is_deleted"].ToString().Trim() == "" || string.IsNullOrEmpty(dr["is_deleted"].ToString()) ? 0 : Convert.ToInt16(dr["is_deleted"].ToString().Trim()));
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
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

        public static string Save_Appointment_Local_To_AbelDent(string FirstNameLastName, DateTime AppointmentStartTime, DateTime AppointmentEndTime, Int64 PatientID, string PatUniqId, string OperatoryId,int reqTime,
          string classification, string ApptType, DateTime AppointedDateTime, string ProvNum, string AppointmentConfirmationStatus, bool allday_event, bool sooner_if_possible, bool privateAppointment, bool auto_confirm_sent, string procedureCode, string apptStatusId)
        {
            
            //if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            //{
            Utility.EHR_UserLogin_ID = GetAbelDentUserLoginId();
            //}

            string Appointment_Id = "";
            string proccodeid = "";
            string AmountProc = "";
            string proccodedec = "";
            Int64 chartID = 0;
            Int64 planID = 0;
            Int64 TransID = 0;
            PatUniqId = "";
            int procid = 0;
            DataTable SqlDt = new DataTable();
            SqlDataAdapter SqlDa = null;
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;

            string ApptDate = AppointmentStartTime.ToString("yyyy-MM-dd") + " 00:00:00:000";
            string ApptTime = "1899-12-30 " + AppointmentStartTime.ToString("HH:mm:ss:fff");

            string CurrentDate = DateTime.Now.Date.ToString("yyyy-MM-dd") + " 00:00:00:000";
            string CurrentTime = "1899-12-30 " + DateTime.Now.ToString("HH:mm:ss") + ":000";

            CommonDB.AbelDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                System.Guid guid = System.Guid.NewGuid();
                PatUniqId = guid.ToString();
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                if (Utility.Application_Version == "12.10.6")
                {
                    SqlCommand.CommandText = "INSERT INTO apt (apid,adate,achair,atime,astatus,atimereq,apxfee,apwork,apwrk2,adid,apallocate,aconfstat,alabstat,ashortstat,afuture,Expire,aduedate,aidentifier) " +
                                                "VALUES('@PatientID', '@ApptDate', '@OperatoryId','@ApptTime','A',@ReqTime,0,'@ApptType','', '@ProviderId', @ReqTime, '', NULL, NULL, NULL, NULL, GETDATE(),'@UniqID')";
                }
                else
                {
                    SqlCommand.CommandText = "INSERT INTO apt (apid,adate,achair,atime,astatus,atimereq,apxfee,apwork,apwrk2,adid,apallocate,aconfstat,alabstat,ashortstat,afuture,Expire,aduedate,aidentifier,ArrivalTime,EndOfWaitTime,EndOfAppointmentTime,UpdateInfo) " +
                                                    "VALUES('@PatientID', '@ApptDate', '@OperatoryId','@ApptTime','A',@ReqTime,0,'@ApptType','', '@ProviderId', @ReqTime, '', NULL, NULL, NULL, NULL, GETDATE(),'@UniqID', NULL, NULL, NULL,0)";
                }
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientID", PatientID.ToString());
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", ProvNum);
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@OperatoryId", OperatoryId);
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ApptType", ApptType);
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ReqTime", reqTime.ToString());
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@UniqID", PatUniqId.ToString());
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ApptDate", (ApptDate.ToString()));
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ApptTime", (ApptTime.ToString()));
                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ApptDueDate", CurrentDate.ToString());

                if (SqlCommand.ExecuteNonQuery() == 1)
                {
                    sqlSelect = string.Empty;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = "INSERT INTO apn ( apnpid,apndate,apncol,apntime,apntype,apnline,apnsubtype,apnentrydate,apnentrytime,apninit,apnnote,apnloggedinuser,apndisplayflag,apnreason) "
                                            + " VALUES('@PatientID', '@ApptDate', '@OperatoryId', '@ApptTime', 'E',0,'N', '@TodayD', '@TodayT','','Booked','@UserName',0, NULL) ";

                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientID", PatientID.ToString());
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@OperatoryId", OperatoryId);
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ApptDate", (ApptDate.ToString()));
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ApptTime", (ApptTime.ToString()));
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@TodayD", CurrentDate.ToString());
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@TodayT", CurrentTime.ToString());
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@UserName", "Adit");

                    SqlCommand.ExecuteNonQuery();
                }

                Appointment_Id = PatUniqId.ToString();
                int i = 0;
                #region New Code 

                if (procedureCode != string.Empty && procedureCode != "")
                {
                    foreach (var treatcode in procedureCode.Split(','))
                    {
                        proccodeid = treatcode.Substring(0, treatcode.IndexOf('_'));

                        sqlSelect = string.Empty;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "select jcode as procode,jdesc2 as description,jnaddfee as fee from jcf where jcode = '" + proccodeid + "' ";                        
                        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                        SqlDt = new DataTable();
                        SqlDa.Fill(SqlDt);
                        if (SqlDt != null && SqlDt.Rows.Count > 0)
                        {
                            proccodedec = SqlDt.Rows[0][1].ToString();
                            AmountProc = SqlDt.Rows[0][2].ToString();
                        }

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "IF NOT EXISTS(select 1 from Charts where patID = '"+ PatientID + "' and Convert(Date,Date)='"+ DateTime.Now.Date.ToString("yyyy-MM-dd") + "') BEGIN INSERT INTO dbo.[Charts] ([patID], [ChartNum], [Date], [DateClosed], [DateCertified], [Certifier], [ChartDesc] ) VALUES ('" + PatientID + "', ((select MAX(ISNULL(ChartNum,0)) AS chartNum from Charts WITH(NOLOCK)) + 1), '" + CurrentDate + "', NULL, NULL, NULL, NULL ); select ChartNum from Charts where patID = '" + PatientID + "' and Convert(Date,Date)='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'; END ELSE BEGIN  select ChartNum from Charts where patID = '" + PatientID + "' and Convert(Date,Date)='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'; END ";

                        chartID = Convert.ToInt64(SqlCommand.ExecuteScalar());

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "IF NOT EXISTS(select 1 from Plans where patID = '" + PatientID + "' and Convert(Date,Date)='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "') BEGIN INSERT INTO dbo.[Plans] ([PatID], [PlanNum], [Date], [Descr], [State] ) VALUES ('" + PatientID.ToString() + "', ((select MAX(ISNULL(PlanNum,0)) AS itemNum from Plans WITH(NOLOCK)) + 1), '" + CurrentDate.ToString() + "', 'Plan from Adit', 1 ); select PlanNum from Plans where patID = '" + PatientID + "' and Convert(Date,Date)='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'; END ELSE BEGIN  select PlanNum from Plans where patID = '" + PatientID + "' and Convert(Date,Date)='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'; END ";
                        planID = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        
                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "INSERT INTO dbo.[Transactions] ([Date], [patID], [ChartNum], [Grp], [ToothNum], [ProvID], [Code], [ChartCode], [Extra], [Descr], [Billed], [Units], [Surfaces], [Deleted], [PlanNum], [Phase], [Type], [Appt], [Applied], [RespProvID], [ItemNum], [DatePosted], [Modifier], [GroupAssociation], [IsRamq] ) VALUES('@Datenow','@PatientEHRId','"+ chartID + "',0,0,'@ProviderEHRId','@Code',233,NULL,'@Descr',@AmountProc,0," +
                            " '',0,0,0,'',0,0,'RC   ',0,'@Datenow','',0,0) ";

                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientEHRId", PatientID.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Descr", proccodedec);
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AmountProc", AmountProc.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Datenow", CurrentDate.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", ProvNum.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Code", proccodeid);
                        SqlCommand.ExecuteNonQuery();

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "INSERT INTO dbo.[Transactions] ([Date], [patID], [ChartNum], [Grp], [ToothNum], [ProvID], [Code], [ChartCode], [Extra], [Descr], [Billed], [Units], [Surfaces], [Deleted], [PlanNum], [Phase], [Type], [Appt], [Applied], [RespProvID], [ItemNum], [DatePosted], [Modifier], [GroupAssociation], [IsRamq] ) "
                            + " Values ('@Datenow','@PatientEHRId',0,0,0,'@ProviderEHRId','@Code',233,NULL,'@Descr',@AmountProc,0," +
                            " '',0," + planID + ",1,'P',1,0,'RC   ',((select MAX(ISNULL(ItemNum,0)) AS itemNum from Transactions WITH(NOLOCK)) + 1),'@Datenow','',0,0) ";

                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientEHRId", PatientID.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Descr", proccodedec);
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AmountProc", AmountProc.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Datenow", CurrentDate.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", ProvNum.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Code", proccodeid);
                        SqlCommand.ExecuteNonQuery();

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "select MAX(ISNULL(TransID,0)) AS TransID from Transactions WITH(NOLOCK)";
                        TransID = Convert.ToInt64(SqlCommand.ExecuteScalar());

                        #region Insert tdi Details

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "INSERT INTO tdi (ipid,itrid,idate,iitem,ijcode,itooth,isurf,idid,iresppvdr,itime,iefee,ilabfee,iinschg,itype,iinsprt,itypecodes,imodifier,iidentifier)"
                                                        + " VALUES (@PatientEHRId,99999998,'@Datenow',((select MAX(ISNULL(iitem,0)) AS itemNum from tdi WITH(NOLOCK)) + 1),'@Code',0,'','@ProviderEHRId','RC',0,@AmountProc,0,0,'P','@TransactionID','','',NULL)";

                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientEHRId", PatientID.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Datenow", CurrentDate.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Code", proccodeid);
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@TransactionID", TransID.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", ProvNum.ToString());
                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AmountProc", AmountProc.ToString());
                        SqlCommand.ExecuteNonQuery();

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "select MAX(ISNULL(iitem,0)) AS itemNum from tdi WITH(NOLOCK) where ipid = '"+ PatientID + "' and Convert(Date,idate)='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'";
                        i = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        sqlSelect = string.Empty;
                        System.Guid apptGuid = System.Guid.NewGuid();
                        apptGuid = Guid.Parse(Appointment_Id);
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "UPDATE dbo.[TDI] SET [iidentifier] = @Guid WHERE [ipid] = '" + PatientID + "' AND [itrid] = 99999998 AND [idate] = '" + CurrentDate.ToString() + "' AND [iitem] = " + i + "";
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@Guid", apptGuid);
                        SqlCommand.ExecuteNonQuery();

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "UPDATE dbo.[Transactions] SET [Identifier] = @Guid WHERE[TransID] = '" + TransID.ToString() + "'";
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@Guid", apptGuid);
                        SqlCommand.ExecuteNonQuery();

                        #endregion

                    }
                    
                }

                #endregion
            }
            catch (Exception ex)
            {
                Appointment_Id = "";
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return Appointment_Id;
        }

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    string sqlSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_EHR_ID;

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", tmpAppt_EHR_id);
                        SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", tmpAppt_Web_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    // SqlCetx.Commit();
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


        public static bool Update_Status_EHR_Appointment_Live_To_AbelDentEHR(DataTable dtLiveAppointment,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {

                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");

                foreach (DataRow dr in dtLiveAppointment.Rows)
                {
                    sqlSelect = SynchAbelDentQRY.UpdateAppointmentStatusFromWeb;
                    
                    if (dr["confirmed_status_ehr_key"].ToString() != "")
                    {
                        sqlSelect = sqlSelect.Replace("@Status", "Y");
                    }
                    //Arrived
                    else if (dr["confirmed_status_ehr_key"].ToString() == "A")
                    {
                        sqlSelect = sqlSelect.Replace("@Status", "A");
                    }
                    //Done
                    else if(dr["confirmed_status_ehr_key"].ToString() == "D")
                    {
                        sqlSelect = sqlSelect.Replace("@Status", "D");
                    }
                    //Preconfirmed
                    else if (dr["confirmed_status_ehr_key"].ToString() == "P")
                    {
                        sqlSelect = sqlSelect.Replace("@Status", "P");
                    }
                    //Confirmed
                    else if(dr["confirmed_status_ehr_key"].ToString() == "Y")
                    {
                        sqlSelect = sqlSelect.Replace("@Status", "Y");
                    }
                    //Left-Message
                    else if (dr["confirmed_status_ehr_key"].ToString() == "Z")
                    {
                        sqlSelect = sqlSelect.Replace("@Status", "Z");
                    }
                    else
                    {
                        sqlSelect = sqlSelect.Replace("@Status", "" + dr["confirmed_status_ehr_key"].ToString() + "");
                    }
                   
                    sqlSelect = sqlSelect.Replace("@AppointmentId", dr["Appt_EHR_ID"].ToString());
                    SqlCommand.CommandText = sqlSelect;
                    SqlCommand.ExecuteNonQuery();
                    if (_filename_EHR_appointment != "" && _EHRLogdirectory_EHR_appointment != "")
                    {
                         Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Update Appointment Status From Web EHRappointment Confirmed in EHR for Appt_EHR_ID: " + dr["Appt_EHR_ID"].ToString() + " and confirmed_status_ehr_key : " + dr["confirmed_status_ehr_key"]);
                    }
                    bool isApptConformStatus = SynchLocalDAL.UpdateLocalAppointmentConformStatusData(dr["Appt_EHR_ID"].ToString(), dr["Service_Install_Id"].ToString(),_filename_EHR_appointment,_EHRLogdirectory_EHR_appointment);
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

        public static bool Insert_Status_EHR_Appointment_To_AbelDentEHR(DataTable dtData,string _filename_EHR_appointment = "",string _EHRLogdirectory_EHR_appointment = "")
        {

            //if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            //{
            //Utility.EHR_UserLogin_ID = GetAbelDentUserLoginId();
            //}

            string CurrentDate = DateTime.Now.Date.ToString("yyyy-MM-dd") + " 00:00:00.000";
            string CurrentTime = "1899-12-30 " + DateTime.Now.ToString("HH:mm:ss") + ".000";

            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();


            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");

                foreach (DataRow dr in dtData.Rows)
                {
                    DataTable aptData = GetAbelDentApt_Data(dr["Appt_EHR_ID"].ToString());

                    if(aptData.Rows.Count > 0)
                    {
                        foreach (DataRow drtab in aptData.Rows)
                        {

                            SqlCommand.CommandText = "INSERT INTO apn ( apnpid,apndate,apncol,apntime,apntype,apnline,apnsubtype,apnentrydate,apnentrytime,apninit,apnnote,apnloggedinuser,apndisplayflag,apnreason)"
                                                    + "VALUES(@PatientID, '@ApptDate', '@Operatory', '@ApptTime', 'S',((select MAX(ISNULL(apnline,0))from apn) + 1),'C','@TodayD', '@TodayT',NULL,'@Note','Adit',0, NULL)";

                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientID", drtab["apid"].ToString());
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ApptDate", drtab["adate"].ToString());
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Operatory", drtab["achair"].ToString());
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ApptTime", drtab["atime"].ToString());
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@TodayD", CurrentDate.ToString());
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@TodayT", CurrentTime.ToString());
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Note", dr["confirmed_status"].ToString());
                            SqlCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Save Status EHR Appointment To AbelDentEHR Confirmed  for PatientID=" + drtab["apid"].ToString() + ",ApptDate : " + drtab["adate"].ToString() + ") and Operatory : " + drtab["achair"].ToString() );
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Update_Receive_SMS_Patient_EHR_Live_To_AbelDentEHR(DataTable dtLiveAppointment, string Locationid, string Loc_ID)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.AbelDentSQLConnectionServer(ref conn);
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
                        sqlSelect = SynchAbelDentQRY.Update_Receive_SMS_Patient_EHR_Live_To_AbelDentEHR;
                        SqlCommand.Parameters.AddWithValue("@receives_sms", dr["receive_sms"].ToString() == "Y" ? 1 : 0);
                        SqlCommand.Parameters.AddWithValue("@patient_id", dr["patient_ehr_id"].ToString());
                        SqlCommand.CommandText = sqlSelect;
                        SqlCommand.ExecuteNonQuery();
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


        private static void UpdatePatientInsurance(string curPatinetInsurance_Name, long PatientId, int InsuranceCount, long FamilyId, string SubScriber)
        {
            SqlConnection conn = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
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

                    SqlCommand.CommandText = SynchAbelDentQRY.InsertPatient_InsurancePlan;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@CarrierId", InsCarrId);
                    SqlCommand.Parameters.AddWithValue("@ModifiedMachineName", Environment.MachineName);
                    SqlCommand.Parameters.AddWithValue("@CreatedMachineName", Environment.MachineName);
                    SqlCommand.ExecuteNonQuery();
                    Insplanid = GetId(conn, "InsurancePlan", "planid");
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
        private static string GetValueSizeWise(DataTable dtAbelDentColumns, string columnsName, string ehrFieldValue)
        {
            string returnResult = "";
            try
            {
                var result = dtAbelDentColumns.AsEnumerable().Where(o => o.Field<string>("EHRColumnName").ToString().ToUpper() == columnsName.ToString().ToUpper()).Select(a => a.Field<string>("EHRDataType")).First();
                var resultSize = dtAbelDentColumns.AsEnumerable().Where(o => o.Field<string>("EHRColumnName").ToString().ToUpper() == columnsName.ToString().ToUpper()).Select(a => a.Field<Int32>("Size")).First();

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
        public static Int64 SavePatientPaymentTOEHR(string DbString, DataTable DtTable, string ServiceInstallationId,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {
            Utility.CheckEntryUserLoginIdExist();
            //if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            //{
            Utility.EHR_UserLogin_ID = GetAbelDentUserLoginId();
            //}

            Int64 rwpTransactionId = 0;
            Int64 tNoTransactionId = 0;
            Int64 DiscountId = 0;
            SqlConnection conn = null;

            SqlCommand SqlCommand = null;

            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                Int64 tno = 0;
                Int64 MethodId = 0, DiscountTypeId = 0;
                string EHRLogId = "0";
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                foreach (DataRow drRow in DtTable.Rows)
                {
                    EHRLogId = "0";
                    rwpTransactionId = 0;
                    tNoTransactionId = 0;
                    DiscountId = 0;
                    decimal amount = 0;
                    decimal discount = 0;
                    try
                    {
                        discount = Convert.ToDecimal(drRow["Discount"]);
                        if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                            amount = -Convert.ToDecimal(drRow["Amount"]) - discount;
                        else
                            amount = Convert.ToDecimal(drRow["Amount"]) - discount;

                        //if (amount > 0 || amount < 0)
                        {
                            #region Add PaymentMode 'Adit Pay' To EHR

                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchAbelDentQRY.CheckPaymentModeExistAsAditPay;
                            MethodId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                      
                            #endregion

                            #region Insert Payment 

                            string note = drRow["template"].ToString().Length > 50 ? drRow["template"].ToString().Substring(0, 50) : drRow["template"].ToString();
                            if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 2 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                            {
                                if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                                {
                                    if (amount != 0)
                                    {
                                        //DateTime dt = Convert.ToDateTime(drRow["PaymentDate"]).TimeOfDay;

                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertPaymentTo_rwpTable;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                        SqlCommand.Parameters.AddWithValue("@Amount", amount * 100);
                                        SqlCommand.Parameters.AddWithValue("@date_entered", Convert.ToDateTime(drRow["PaymentDate"]).Date);
                                        SqlCommand.Parameters.AddWithValue("@time_entered", Convert.ToDateTime(drRow["PaymentDate"]).TimeOfDay);
                                        SqlCommand.Parameters.AddWithValue("@TransactionType", 'B');       // Adit Pay

                                        if (drRow["FirstName"].ToString() == string.Empty || drRow["FirstName"].ToString() == "" || drRow["LastName"].ToString() == string.Empty || drRow["LastName"].ToString() == "")
                                        {
                                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientName", "( SELECT CONCAT(plname, ', ' , pfname) AS full_name FROM pat where pid = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                                        }
                                        else
                                        {
                                            SqlCommand.Parameters.AddWithValue("@PatientName", "" + drRow["LastName"].ToString() + " , " + drRow["FirstName"].ToString() + "");
                                        }

                                        // string test = SqlCommand.CommandText;

                                        if (SqlCommand.ExecuteNonQuery() == 1)
                                        {
                                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Payment TO EHR for PatientEHRId = " + drRow["PatientEHRId"].ToString() + ",FirstName=" + drRow["FirstName"].ToString() + ",LastName=" +  drRow["LastName"].ToString() + " and ProviderEHRId=" + drRow["ProviderEHRId"].ToString());
                                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                            SqlCommand.CommandText = SynchAbelDentQRY.InsertPaymentTo_trnTable;
                                            SqlCommand.Parameters.Clear();
                                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                            SqlCommand.Parameters.AddWithValue("@Amount", amount * 100);
                                            SqlCommand.Parameters.AddWithValue("@date_entered", drRow["PaymentDate"].ToString());
                                            SqlCommand.Parameters.AddWithValue("@Prepay", 1);

                                            if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                                            {
                                                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT pdentist from pat where pid = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                                            }
                                            else
                                            {
                                                SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                                            }
                                            SqlCommand.ExecuteNonQuery();
                                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Saved Patient Payment To Transaction with PatientEHRId = " + drRow["PatientEHRId"].ToString() + " and ProviderEHRId=" + drRow["ProviderEHRId"].ToString());                       
                                        }
                                    }                                  
                                    if (discount > 0 &&  amount == 0)
                                    {

                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertPaymentTo_trnTable_discount;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                        SqlCommand.Parameters.AddWithValue("@Amount", -(discount * 100));
                                        SqlCommand.Parameters.AddWithValue("@date_entered", drRow["PaymentDate"].ToString());
                                        SqlCommand.Parameters.AddWithValue("@Prepay", 0);

                                        if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                                        {
                                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT pdentist from pat where pid = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                                        }
                                        else
                                        {
                                            SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                                        }
                                        SqlCommand.ExecuteNonQuery();
                                        
                                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get Payment Transactions Discount with PatientEHRId=" + drRow["PatientEHRId"].ToString() );
                                        string QueryIdentity = "Select MAX(ISNULL(tno,0))from trn";
                                        CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                                        tno = Convert.ToInt64(SqlCommand.ExecuteScalar());

                                        if (tno != 0)
                                        {
                                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                            SqlCommand.CommandText = SynchAbelDentQRY.InsertPaymentTransactionsForDiscount;
                                            SqlCommand.Parameters.Clear();
                                            SqlCommand.Parameters.AddWithValue("@TranNo", tno);
                                            SqlCommand.Parameters.AddWithValue("@DiscountString", "Discount -"+ discount);
                                            SqlCommand.ExecuteNonQuery();
                                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Saved Payment Transactions For Discount with TranNo=" + tno);
                                        }
                                    }

                                    if(discount == 0 && amount != 0)
                                    {
                                        string QueryIdentity = "Select MAX(ISNULL(tno,0))from trn";
                                        CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                                        tno = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                    }
                                }
                                else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                                {
                                    if(amount != 0)
                                    {
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertPaymentTo_trnTable_refund;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"].ToString());
                                        SqlCommand.Parameters.AddWithValue("@Amount", amount * 100);
                                        SqlCommand.Parameters.AddWithValue("@date_entered", drRow["PaymentDate"].ToString());
                                        SqlCommand.Parameters.AddWithValue("@Prepay", 0);

                                        if (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")
                                        {
                                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderEHRId", "( SELECT pdentist from pat where pid = " + Convert.ToInt32(drRow["PatientEHRId"].ToString()) + ")");
                                        }
                                        else
                                        {
                                            SqlCommand.Parameters.AddWithValue("@ProviderEHRId", drRow["ProviderEHRId"].ToString());
                                        }
                                        SqlCommand.ExecuteNonQuery();
                                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get Payment to Transactions refund with PatientEHRId=" + drRow["PatientEHRId"].ToString() + " and ProviderEHRId=" + drRow["ProviderEHRId"].ToString());
                                        string QueryIdentity = "Select MAX(ISNULL(tno,0))from trn";
                                        CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                                        tno = Convert.ToInt64(SqlCommand.ExecuteScalar());

                                        if (tno != 0)
                                        {
                                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                            SqlCommand.CommandText = SynchAbelDentQRY.InsertPaymentTransactionsForRefund;
                                            SqlCommand.Parameters.Clear();
                                            SqlCommand.Parameters.AddWithValue("@TranNo", tno);
                                            SqlCommand.ExecuteNonQuery();
                                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Saved Payment Transactions For Refund with TranNo=" + tno.ToString());
                                        }
                                    }
                                }
                                drRow["PaymentEHRId"] = tno.ToString();
                                drRow["PaymentUpdatedEHR"] = true;
                                drRow["PaymentUpdatedEHRDateTime"] = DateTime.Now.ToString();
                                //SynchLocalDAL.UpdatePatientPaymentEHRId_In_Local(tno.ToString(), drRow["PatientPaymentWebId"].ToString().Trim(), ServiceInstallationId);
                            }
                            if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                            {
                                EHRLogId = Save_PatientPaymentLog_LocalToAbelDent(drRow, DbString, ServiceInstallationId,_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                            }                            
                            if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 0)
                            {
                                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "Sync Log and Payment is disabled from Adit App", rwpTransactionId.ToString(), EHRLogId.ToString(), DiscountId.ToString(), "Sync Log and Payment is disabled from Adit App",Convert.ToInt32(drRow["TryInsert"]),_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                            }
                            bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow,_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                            SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", "1", "0", "", rwpTransactionId.ToString(), EHRLogId.ToString(), DiscountId.ToString(), "", Convert.ToInt32(drRow["TryInsert"]),_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                            #endregion
                        }
                    }
                    catch (Exception ex1)
                    {
                        bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow,_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                        SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", "1", "0", "400", "", "", "", ex1.Message.ToString(), Convert.ToInt32(drRow["TryInsert"]),_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                        Utility.WriteToErrorLogFromAll("Save_PatientPaymentLog : " + ex1.Message.ToString());
                    }
                }
                return rwpTransactionId;

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

        public static bool SavePatientAllergies_To_AbelDent(string strPatientFormID = "")
        {
            string DiseaseId = string.Empty;
            Int64 PatientID = 0;
            bool IsSuccessInsert = false;
            bool isPatientInsert = false;
            SqlConnection conn = null;
            SqlDataAdapter SqlDa = null;
            SqlCommand SqlCommand = null;
            string sqlSelect = string.Empty;
            string CurDate = DateTime.Now.ToString("yyyy-MM-dd");// + " 00:00:00:000";

            try
            {
                DataTable dtPatientDiseaseResponse = SynchLocalDAL.GetLocalPatientFormDiseaseResponseToSaveINEHR("1", strPatientFormID);

                if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                {
                    Utility.EHR_UserLogin_ID = SynchAbelDentDAL.GetAbelDentUserLoginId();
                }

                System.Threading.Thread.Sleep(240000);

                if (dtPatientDiseaseResponse != null)
                {
                    if (dtPatientDiseaseResponse.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtPatientDiseaseResponse.Rows)
                        {
                            CommonDB.AbelDentSQLConnectionServer(ref conn);
                            if (conn.State == ConnectionState.Closed) conn.Open();

                            string LegacyPID = "";
                            LegacyPID = dr["PatientEHRID"].ToString();
                            LegacyPID = LegacyPID.PadLeft(6, '0');

                            if (dr["Disease_Type"].ToString() == "A")
                            {
                                if (Utility.Application_Version == "15")
                                {
                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentAllergy_15;
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Timezone",Utility.LocationTimeZone);
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@PatientEHRID", dr["PatientEHRID"].ToString());
                                    SqlCommand.Parameters.AddWithValue("@AllergenName", dr["Name"].ToString());
                                    SqlCommand.ExecuteNonQuery();

                                    int ehrDiseaseId = 0;
                                    string SqlSelect = "SELECT [Id] FROM [Allergy] where PatientId = '"+ dr["PatientEHRID"].ToString() + "' and Allergen = '"+ dr["Name"].ToString() + "'";
                                    SqlCommand = new SqlCommand();
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                    DataTable SqlDt = new DataTable();
                                    SqlDa.Fill(SqlDt);

                                    if (SqlDt != null && SqlDt.Rows.Count > 0)
                                    {
                                        ehrDiseaseId = Convert.ToInt32(SqlDt.Rows[0][0].ToString());
                                    }

                                    SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : ehrDiseaseId.ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), "1");
                                }
                                else
                                {
                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy start - 1", "SavePatientAllergies_To_AbelDent");                                    
                                    string ClinicalConcept = "";
                                    System.Guid ConceptGuid = System.Guid.NewGuid();
                                    System.Guid Guid = System.Guid.NewGuid();
                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalConcept_Allergy_1;
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AllergyTextCapi", dr["Name"].ToString().ToUpper());
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AllergyTextSmall", dr["Name"].ToString().ToLower());
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@GuidId", ConceptGuid);
                                    ClinicalConcept = Convert.ToString(SqlCommand.ExecuteScalar());
                                    ConceptGuid = Guid.Parse(ClinicalConcept);

                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy Inprogress - 2", "SavePatientAllergies_To_AbelDent");                                    

                                    System.Guid ActGuID = System.Guid.NewGuid();
                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAct_Allergy_2;
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AllergyText", dr["Name"].ToString().ToLower());
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                    SqlCommand.Parameters.AddWithValue("@CurDateTime", CurDate);
                                    SqlCommand.Parameters.AddWithValue("@ClinicalConceptID", ConceptGuid);
                                    SqlCommand.Parameters.AddWithValue("@UserID", Utility.EHR_UserLogin_ID);
                                    SqlCommand.ExecuteNonQuery();

                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy Inprogress - 3", "SavePatientAllergies_To_AbelDent");

                                    string patientid = "";
                                    string SqlSelect = "";
                                    System.Guid PatientGuid;
                                    sqlSelect = SynchAbelDentQRY.GetAbelDentClinicalPatient;
                                    sqlSelect = sqlSelect.Replace("@PatientId", LegacyPID);
                                    SqlCommand = new SqlCommand();
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                    DataTable SqlDt = new DataTable();
                                    SqlDa.Fill(SqlDt);

                                    if (SqlDt != null && SqlDt.Rows.Count > 0)
                                    {
                                        patientid = SqlDt.Rows[0][0].ToString();
                                    }
                                    
                                    PatientGuid = Guid.Parse(patientid);

                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy Inprogress - 4 PatientID : " + LegacyPID + "", "SavePatientAllergies_To_AbelDent");                 

                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAllergyAct_4;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@PatientUniqID", PatientGuid);
                                    SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                    SqlCommand.ExecuteNonQuery();

                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy Inprogress - 5", "SavePatientAllergies_To_AbelDent");

                                    System.Guid RelationshipGuID = System.Guid.NewGuid();
                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAdverseReactionAllergy_Allergy_5;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                    SqlCommand.Parameters.AddWithValue("@ClinicalCoceptID", ConceptGuid);
                                    SqlCommand.ExecuteNonQuery();

                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy Inprogress - 6", "SavePatientAllergies_To_AbelDent");

                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalObservation_Allergy_6;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                    SqlCommand.ExecuteNonQuery();

                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy Inprogress - 7", "SavePatientAllergies_To_AbelDent");

                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalActRelationship_Problem_Allergy_7;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                    SqlCommand.ExecuteNonQuery();

                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy Inprogress - 8", "SavePatientAllergies_To_AbelDent");

                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalParticipation_Allergy_8;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                    SqlCommand.ExecuteNonQuery();

                                    Utility.WriteToDebugSyncLogFile_All("Insert Allergy Success - 9", "SavePatientAllergies_To_AbelDent");                                    

                                    SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : ActGuID.ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), "1");
                                }
                            }
                            else
                            {
                                if (Utility.Application_Version == "15")
                                {
                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentAllergy_15;
                                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Timezone", Utility.LocationTimeZone);
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@PatientEHRID", dr["PatientEHRID"].ToString());
                                    SqlCommand.Parameters.AddWithValue("@AllergenName", dr["Name"].ToString());
                                    SqlCommand.ExecuteNonQuery();

                                    int ehrDiseaseId = 0;
                                    string SqlSelect = "SELECT [Id] FROM [Allergy] where PatientId = '" + dr["PatientEHRID"].ToString() + "' and Allergen = '" + dr["Name"].ToString() + "'";
                                    SqlCommand = new SqlCommand();
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                    DataTable SqlDt = new DataTable();
                                    SqlDa.Fill(SqlDt);

                                    if (SqlDt != null && SqlDt.Rows.Count > 0)
                                    {
                                        ehrDiseaseId = Convert.ToInt32(SqlDt.Rows[0][0].ToString());
                                    }

                                    SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : ehrDiseaseId.ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), "1");
                                }
                                //string ClinicalConcept = "";
                                //System.Guid ConceptGuid = System.Guid.NewGuid();
                                //System.Guid Guid = System.Guid.NewGuid();
                                //SqlCommand = new SqlCommand();
                                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalConcept_Allergy_1;
                                //SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AllergyTextCapi", dr["Name"].ToString().ToUpper());
                                //SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AllergyTextSmall", dr["Name"].ToString().ToLower());
                                //SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@GuidId", ConceptGuid);
                                //ClinicalConcept = Convert.ToString(SqlCommand.ExecuteScalar());
                                //ConceptGuid = Guid.Parse(ClinicalConcept);

                                //System.Guid ActGuID = System.Guid.NewGuid();
                                //SqlCommand = new SqlCommand();
                                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAct_Allergy_2;
                                //SqlCommand.CommandText = SqlCommand.CommandText.Replace("@AllergyText", dr["Name"].ToString().ToLower());
                                //SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                //SqlCommand.Parameters.AddWithValue("@CurDateTime", CurDate);
                                //SqlCommand.Parameters.AddWithValue("@ClinicalConceptID", ConceptGuid);
                                //SqlCommand.Parameters.AddWithValue("@UserID", Utility.EHR_UserLogin_ID);
                                //SqlCommand.ExecuteNonQuery();

                                //string patientid = "";
                                //System.Guid PatientGuid;
                                //string SqlSelect = SynchAbelDentQRY.GetAbelDentClinicalPatient_Allergy_3;
                                //SqlSelect = SqlSelect.Replace("@PatientId", LegacyPID);
                                //SqlCommand = new SqlCommand();
                                //if (conn.State == ConnectionState.Closed) conn.Open();
                                //CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                //CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                //DataTable SqlDt = new DataTable();
                                //SqlDa.Fill(SqlDt);

                                //if (SqlDt != null && SqlDt.Rows.Count > 0)
                                //{
                                //    patientid = SqlDt.Rows[0][0].ToString();
                                //}
                                //PatientGuid = Guid.Parse(patientid);

                                //SqlCommand = new SqlCommand();
                                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAllergyAct_4;
                                //SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@PatientUniqID", PatientGuid);
                                //SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                //SqlCommand.ExecuteNonQuery();

                                //System.Guid RelationshipGuID = System.Guid.NewGuid();
                                //SqlCommand = new SqlCommand();
                                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAdverseReactionAllergy_Allergy_5;
                                //SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                //SqlCommand.Parameters.AddWithValue("@ClinicalCoceptID", ConceptGuid);
                                //SqlCommand.ExecuteNonQuery();


                                //SqlCommand = new SqlCommand();
                                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalObservation_Allergy_6;
                                //SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                //SqlCommand.ExecuteNonQuery();

                                //SqlCommand = new SqlCommand();
                                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalActRelationship_Problem_Allergy_7;
                                //SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                //SqlCommand.ExecuteNonQuery();

                                //SqlCommand = new SqlCommand();
                                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalParticipation_Allergy_8;
                                //SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                //SqlCommand.ExecuteNonQuery();


                                //SynchLocalDAL.UpdateDiseaseEHR_Updateflg(dr["PatientForm_Web_ID"].ToString(), DiseaseId.ToString() == "" ? dr["PatientEHRID"].ToString() + "_" + dr["Disease_EHR_Id"].ToString() : ActGuID.ToString(), dr["PatientEHRID"].ToString(), dr["Disease_EHR_Id"].ToString(), "1");
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("SavePatientAllergies_To_AbelDent : Eception : " + ex.Message);
                return false;
            }
            finally
            {
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_Document_in_AbelDent(string strPatientFormID = "")
        {
            int DocId = 0;
            bool IsDocUpdate = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocData("1", strPatientFormID);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                    {
                        Utility.EHR_UserLogin_ID = SynchAbelDentDAL.GetAbelDentUserLoginId();
                    }

                    string documentTypeID = "";
                    string adjustPatientEHRID = "";
                    string PatientEHRIDPartOne = "";
                    string PatientEHRIDPartTwo = "";
                    string PatientEHRIDPartThree = "";
                    string destPatientDocPath = "";
                    string dstnLocation = "";
                    string tmpFileOrgName = "";
                    string tmpFileName = ""; 
                    string FormName, FolderName = "";
                    string FileExtension = "", RenameFileName = "";
                    string sourceFileExtenstion = "";
                    string sqlSelect = string.Empty;

                    Utility.WriteToDebugSyncLogFile_All("Patient-Doc Insert Start 1", "CallSynch_PatientDoc");

                    System.Threading.Thread.Sleep(240000); // Pauses for 4 minutes .Sleep(300000); // 300,000 milliseconds = 5 minutes

                    Utility.WriteToDebugSyncLogFile_All("Patient-Doc Insert ThreadSleep End", "CallSynch_PatientDoc");

                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        destPatientDocPath = "";
                        if (Convert.ToInt32(dr["Patient_EHR_ID"].ToString()) != 6)
                            adjustPatientEHRID = dr["Patient_EHR_ID"].ToString().PadLeft(6, '0');
                        else
                            adjustPatientEHRID = dr["Patient_EHR_ID"].ToString();

                        if (adjustPatientEHRID.Length == 6)
                        {
                            PatientEHRIDPartOne = adjustPatientEHRID.Substring(0, 2);
                            PatientEHRIDPartTwo = adjustPatientEHRID.Substring(2, 2);
                            PatientEHRIDPartThree = adjustPatientEHRID.Substring(4, 2);
                        }

                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            if (Utility.Application_Version == "15")
                            {
                                destPatientDocPath = @"C:\ABELDent\LocalStorage\Documents\" + adjustPatientEHRID  + "";
                            }
                            else
                            {
                                if (PatientEHRIDPartOne.Substring(0, 1) == "0")
                                    destPatientDocPath = @"C:\ABELDent\Data\DOCUMENTS\PN" + PatientEHRIDPartOne.Substring(1, 1) + "\\" + PatientEHRIDPartTwo + "\\" + PatientEHRIDPartThree + "";
                                else
                                    destPatientDocPath = @"C:\ABELDent\Data\DOCUMENTS\PN" + PatientEHRIDPartOne.Substring(0, 2) + "\\" + PatientEHRIDPartTwo + "\\" + PatientEHRIDPartThree + "";
                            }

                        }
                        else
                        {
                            if (Utility.Application_Version == "15")
                            {
                                destPatientDocPath = Utility.EHRDocPath + "\\" + adjustPatientEHRID + "";
                            }
                            else
                            {
                                if (PatientEHRIDPartOne.Substring(0, 1) == "0")
                                    destPatientDocPath = Utility.EHRDocPath + "\\PN" + PatientEHRIDPartOne.Substring(1, 1) + "\\" + PatientEHRIDPartTwo + "\\" + PatientEHRIDPartThree + "";
                                else
                                    destPatientDocPath = Utility.EHRDocPath + "\\PN" + PatientEHRIDPartOne.Substring(0, 2) + "\\" + PatientEHRIDPartTwo + "\\" + PatientEHRIDPartThree + "";
                            }
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

                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        FolderName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";

                        #region UpdateDocuemtnType
                        System.Guid documentguid = System.Guid.NewGuid();
                        System.Guid documenttypeGuid;
                        string SqlSelect = SynchAbelDentQRY.GetAbelDentDocumentTypeID;
                        SqlSelect = SqlSelect.Replace("@DocumentName", FormName);
                        SqlCommand = new SqlCommand();
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@UniqID", documentguid);
                        documentTypeID = SqlCommand.ExecuteScalar().ToString();
                        documenttypeGuid = Guid.Parse(documentTypeID);
                        #endregion

                        if (Utility.Application_Version == "15")
                        {
                            #region InsertINToDocuemtn
                            System.Guid Docguid = System.Guid.NewGuid();
                            tmpFileOrgName = Path.GetFileName(sourceLocation);

                            SqlCommand = new SqlCommand();
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentDocument_15;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@DocumentID", Docguid);
                            SqlCommand.Parameters.AddWithValue("@FileName", tmpFileOrgName);
                            SqlCommand.Parameters.AddWithValue("@DocumentTypeID", documenttypeGuid);//DocumentTypeID
                            SqlCommand.Parameters.AddWithValue("@PatientEHRID", Convert.ToInt32(adjustPatientEHRID));
                            SqlCommand.ExecuteNonQuery();
                            #endregion

                            sourceFileExtenstion = tmpFileOrgName.Substring(tmpFileOrgName.IndexOf("."));
                            tmpFileName = tmpFileOrgName.Substring(0, tmpFileOrgName.IndexOf("."));

                            tmpFileName =  tmpFileName + Docguid.ToString() + sourceFileExtenstion;

                            sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + tmpFileOrgName;
                            dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(tmpFileName));

                            if (!System.IO.Directory.Exists(destPatientDocPath))
                            {
                                System.IO.Directory.CreateDirectory(destPatientDocPath);
                            }

                            System.IO.File.Copy(sourceLocation, dstnLocation, true);

                            FileInfo fi = new FileInfo(dstnLocation);
                            FileExtension = fi.Extension;
                            RenameFileName = dr["PatientDoc_Web_ID"].ToString() + FileExtension;


                            PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), Docguid.ToString(), "1");
                            File.Delete(sourceLocation);
                        }
                        else
                        {
                           
                            string patientid = "";
                            sqlSelect = "";
                            bool isDataFound = false;
                            Guid? PatientGuid = null;
                            //System.Guid PatientGuid;
                            DataTable SqlDt1 = new DataTable();
                            SqlConnection conn1 = null;
                            SqlCommand SqlCommand1 = new SqlCommand();
                            SqlDataAdapter SqlDa1 = null;
                            string SqlSelect1 = "";
                            CommonDB.AbelDentSQLConnectionServer(ref conn1);

                            TimeSpan timeLimit = TimeSpan.FromMinutes(10);
                            DateTime startTime = DateTime.Now;

                            while ((DateTime.Now - startTime) < timeLimit)
                            {
                                Utility.WriteToDebugSyncLogFile_All("Insert Doc Start While Loop for 10 min: ", "CallSynch_PatientDoc");
                                #region GetPatientUniqID
                                try
                                {
                                    if (conn1.State == ConnectionState.Closed) conn1.Open();
                                    SqlSelect1 = " SELECT * FROM [ClinicalPatient] where RTRIM(LTRIM(LegacyPID)) = '" + adjustPatientEHRID + "'";
                                    Utility.WriteToDebugSyncLogFile_All("GETLEGACYID Query : " + SqlSelect1 + "", "CallSynch_PatientDoc");
                                    CommonDB.SqlServerCommand(SqlSelect1, conn1, ref SqlCommand1, "txt");
                                    CommonDB.SqlServerDataAdapter(SqlCommand1, ref SqlDa1);
                                    SqlDt1 = new DataTable();
                                    SqlDa1.Fill(SqlDt1);                                

                                    if (SqlDt1.Rows.Count > 0)
                                    {
                                                                                
                                        patientid = SqlDt1.Rows[0][0].ToString();
                                        PatientGuid = Guid.Parse(patientid);
                                        isDataFound = true;
                                        Utility.WriteToDebugSyncLogFile_All("Insert Doc PatientGuid from Auto Insert : " + PatientGuid + "", "CallSynch_PatientDoc");
                                        Utility.WriteToDebugSyncLogFile_All("Insert Doc Patient ID from Auto Insert : " + patientid + "", "CallSynch_PatientDoc");
                                        break;                                        
                                    }
                                }
                                catch (Exception ex)
                                {
                                    isDataFound = false;
                                    Utility.WriteToDebugSyncLogFile_All("GETLEGACYID Exception : " + ex.Message + "", "CallSynch_PatientDoc");
                                }

                                //await Task.Delay(30000); 

                                #region BackupManulInsert
                                //else
                                //{
                                //    string LegacyPID = "";
                                //    LegacyPID = Convert.ToString(adjustPatientEHRID);
                                //    LegacyPID = LegacyPID.PadLeft(6, '0');
                                //    sqlSelect = "";

                                //    System.Guid ClinicEntityguID = System.Guid.NewGuid();
                                //    SqlCommand = new SqlCommand();
                                //    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //    SqlCommand.CommandText = "Insert into ClinicalEntity  (ID,ImplementingClass) VALUES (@ClinicEntityguID,'ClinicalPerson')";
                                //    SqlCommand.Parameters.Clear();
                                //    SqlCommand.Parameters.AddWithValue("@ClinicEntityguID", ClinicEntityguID);
                                //    SqlCommand.ExecuteNonQuery();

                                //    System.Guid ClinicRoleguID = System.Guid.NewGuid();
                                //    SqlCommand = new SqlCommand();
                                //    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //    SqlCommand.CommandText = "Insert into ClinicalRole(ID, StatusCode, EntityID, ImplementingClass, SecurityRoleName, ClassCode, NegationInd) values (@ClinicRoleguID,null,@ClinicEntityguID,'ClinicalRole',NULL,NULL,NULL)";
                                //    SqlCommand.Parameters.Clear();
                                //    SqlCommand.Parameters.AddWithValue("@ClinicRoleguID", ClinicRoleguID);
                                //    SqlCommand.Parameters.AddWithValue("@ClinicEntityguID", ClinicEntityguID);
                                //    SqlCommand.ExecuteNonQuery();

                                //    SqlCommand = new SqlCommand();
                                //    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                //    SqlCommand.CommandText = "INSERT into ClinicalPatient (ID,ConfidentialityCode,IsVIP,LegacyPID,PrincipalPhysicianRoleID,PreferredPharmacyID) VALUES (@ClinicRoleguID, '*', 0, '" + LegacyPID + "', NULL, NULL)";
                                //    SqlCommand.Parameters.Clear();
                                //    SqlCommand.Parameters.AddWithValue("@ClinicRoleguID", ClinicRoleguID);
                                //    SqlCommand.ExecuteNonQuery();

                                //    SqlCommand = new SqlCommand();
                                //    if (conn1.State == ConnectionState.Closed) conn1.Open();
                                //    SqlSelect1 = "SELECT * FROM [ClinicalPatient] where LTRIM(REPLACE(LegacyPID, ' ', '')) = " + adjustPatientEHRID + "";
                                //    CommonDB.SqlServerCommand(SqlSelect1, conn1, ref SqlCommand1, "txt");
                                //    CommonDB.SqlServerDataAdapter(SqlCommand1, ref SqlDa1);
                                //    SqlDt1 = new DataTable();
                                //    SqlDa1.Fill(SqlDt1);                                

                                //    if (SqlDt1.Rows.Count > 0)
                                //    {
                                //        try
                                //        {
                                //            if (SqlDt1.Rows.Count > 1)
                                //            {
                                //                Utility.WriteToDebugSyncLogFile_All("Patient ID found! 2nd but Access 1st", "CallSynch_PatientDoc");
                                //                patientid = SqlDt1.Rows[1][0].ToString();
                                //            }
                                //            else
                                //            {
                                //                patientid = SqlDt1.Rows[0][0].ToString();
                                //            }
                                //        }
                                //        catch (Exception ex)
                                //        {
                                //            Utility.WriteToDebugSyncLogFile_All("Getting Exception from Manual Insert Access 1st : " + ex.Message + "", "CallSynch_PatientDoc");               
                                //            patientid = SqlDt1.Rows[0][0].ToString();
                                //            Utility.WriteToDebugSyncLogFile_All("Now Access 2st", "CallSynch_PatientDoc");
                                //        }

                                //    }
                                //    Utility.WriteToDebugSyncLogFile_All("Insert Doc Manual Insert Success: " + patientid + "", "CallSynch_PatientDoc");                                
                                //    PatientGuid = Guid.Parse(patientid);
                                //}
                                #endregion                                

                                #endregion
                            }

                            Utility.WriteToDebugSyncLogFile_All("Insert Doc Finish While Loop for 10 min: ", "CallSynch_PatientDoc");
                            if (isDataFound)                           
                            {
                                if (patientid != "" && PatientGuid != null)
                                {

                                    #region InsertINToClinicalAct
                                    string defaultprovider = "94A8B483-630A-42DF-81D6-9B52D2425E27";
                                    System.Guid defPro;
                                    defPro = Guid.Parse(defaultprovider);
                                    System.Guid Actguid = System.Guid.NewGuid();

                                    Utility.WriteToDebugSyncLogFile_All("Insert Doc 2", "CallSynch_PatientDoc");

                                    string CurDate = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00:000";
                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAct_1;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@UniqID", Actguid);
                                    SqlCommand.Parameters.AddWithValue("@Time", CurDate);
                                    SqlCommand.Parameters.AddWithValue("@UserID", defPro);
                                    SqlCommand.ExecuteNonQuery();
                                    #endregion

                                    Utility.WriteToDebugSyncLogFile_All("Insert Doc 3", "CallSynch_PatientDoc");

                                    #region InsertINToClinicalParticipation
                                    System.Guid Participationguid = System.Guid.NewGuid();

                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalParticipation_2;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@UniqID", Participationguid);
                                    SqlCommand.Parameters.AddWithValue("@ActID", Actguid);
                                    SqlCommand.Parameters.AddWithValue("@PatientUniqID", PatientGuid);
                                    SqlCommand.ExecuteNonQuery();
                                    #endregion

                                    Utility.WriteToDebugSyncLogFile_All("Insert Doc 4", "CallSynch_PatientDoc");

                                    #region InsertINToClinicalParticipation
                                    System.Guid Participationguid1 = System.Guid.NewGuid();
                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalParticipation_2;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@UniqID", Participationguid1);
                                    SqlCommand.Parameters.AddWithValue("@ActID", Actguid);
                                    SqlCommand.Parameters.AddWithValue("@PatientUniqID", defPro);
                                    SqlCommand.ExecuteNonQuery();
                                    #endregion

                                    Utility.WriteToDebugSyncLogFile_All("Insert Doc 5", "CallSynch_PatientDoc");

                                    #region InsertINToDocuemtn

                                    tmpFileOrgName = Path.GetFileName(sourceLocation);

                                    SqlCommand = new SqlCommand();
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentDocument_3;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@ActID", Actguid);
                                    SqlCommand.Parameters.AddWithValue("@FileName", tmpFileOrgName);
                                    SqlCommand.Parameters.AddWithValue("@DocumentTypeID", documenttypeGuid);//DocumentTypeID
                                    SqlCommand.ExecuteNonQuery();
                                    #endregion

                                    Utility.WriteToDebugSyncLogFile_All("Insert Doc 6", "CallSynch_PatientDoc");

                                    sourceFileExtenstion = tmpFileOrgName.Substring(tmpFileOrgName.IndexOf("."));
                                    tmpFileName = tmpFileOrgName.Substring(0, tmpFileOrgName.IndexOf("."));

                                    tmpFileName = tmpFileName + Actguid.ToString() + sourceFileExtenstion;

                                    sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + tmpFileOrgName;
                                    dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(tmpFileName));

                                    if (!System.IO.Directory.Exists(destPatientDocPath))
                                    {
                                        System.IO.Directory.CreateDirectory(destPatientDocPath);
                                    }

                                    System.IO.File.Copy(sourceLocation, dstnLocation, true);

                                    Utility.WriteToDebugSyncLogFile_All("Insert Doc 7 - Done replace path : " + dstnLocation + "", "CallSynch_PatientDoc");

                                    FileInfo fi = new FileInfo(dstnLocation);
                                    FileExtension = fi.Extension;
                                    RenameFileName = dr["PatientDoc_Web_ID"].ToString() + FileExtension;


                                    PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), Actguid.ToString(), "1");
                                    File.Delete(sourceLocation);
                                }
                                else
                                {
                                    Utility.WriteToDebugSyncLogFile_All("Insert Doc 1.1  - patientid is blank: " + patientid + "", "CallSynch_PatientDoc");
                                }
                            }
                            else
                            {
                                Utility.WriteToDebugSyncLogFile_All("Insert Doc Not Found PatientID from Auto Insert: ", "CallSynch_PatientDoc");
                            }
                        }
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

        public static bool Save_Treatment_Document_in_AbelDent(string strTreatmentPlanID = "")
        {
            bool IsDocUpdate = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLocaleTreatmentDocData(strTreatmentPlanID);

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                System.Threading.Thread.Sleep(240000);

                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string documentTypeID = "";
                        string adjustPatientEHRID = "";
                        string PatientEHRIDPartOne = "";
                        string PatientEHRIDPartTwo = "";
                        string PatientEHRIDPartThree = "";
                        string destPatientDocPath = "";
                        string dstnLocation = "";
                        string tmpFileOrgName = "";
                        string tmpFileName = "";
                        string FormName, FolderName = "";
                        string FileExtension = "", RenameFileName = "";
                        string sourceFileExtenstion = "";
                        string sqlSelect = string.Empty;


                        if (Convert.ToInt32(dr["Patient_EHR_ID"].ToString()) != 6)
                            adjustPatientEHRID = dr["Patient_EHR_ID"].ToString().PadLeft(6, '0');
                        else
                            adjustPatientEHRID = dr["Patient_EHR_ID"].ToString();

                        if (adjustPatientEHRID.Length == 6)
                        {
                            PatientEHRIDPartOne = adjustPatientEHRID.Substring(0, 2);
                            PatientEHRIDPartTwo = adjustPatientEHRID.Substring(2, 2);
                            PatientEHRIDPartThree = adjustPatientEHRID.Substring(4, 2);
                        }

                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\ABELDent\Data\DOCUMENTS\PN" + PatientEHRIDPartOne + "\\" + PatientEHRIDPartTwo + "\\" + PatientEHRIDPartThree + "";
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "" + PatientEHRIDPartTwo + "\\" + PatientEHRIDPartThree + "\\";
                        }
                        string sourceLocation = CommonUtility.GetAditTreatmentDocTempPath() + "\\" + dr["TreatmentDoc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            PullLiveDatabaseDAL.Update_TreatrmentDocNotFound_Live_To_Local(dr["TreatmentPlanId"].ToString());
                            continue;
                        }

                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        FolderName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";

                        #region UpdateDocuemtnType
                        System.Guid documentguid = System.Guid.NewGuid();
                        System.Guid documenttypeGuid;
                        string SqlSelect = SynchAbelDentQRY.GetAbelDentDocumentTypeID;
                        SqlSelect = SqlSelect.Replace("@DocumentName", FormName);
                        SqlCommand = new SqlCommand();
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@UniqID", documentguid);
                        documentTypeID = SqlCommand.ExecuteScalar().ToString();
                        documenttypeGuid = Guid.Parse(documentTypeID);
                        #endregion

                        #region GetPatientUniqID
                        string patientid = "";
                        System.Guid PatientGuid;
                        SqlCommand = new SqlCommand();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = "select ID from ClinicalPatient where LTRIM(REPLACE(LegacyPID, ' ', '')) = '" + adjustPatientEHRID + "'";
                        SqlCommand.Parameters.Clear();
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                        DataTable SqlDt = new DataTable();
                        SqlDa.Fill(SqlDt);

                        if (SqlDt != null && SqlDt.Rows.Count > 0)
                        {
                            patientid = SqlDt.Rows[0][0].ToString();
                        }
                        PatientGuid = Guid.Parse(patientid);
                        #endregion

                        #region InsertINToClinicalAct
                        string defaultprovider = "94A8B483-630A-42DF-81D6-9B52D2425E27";
                        System.Guid defPro;
                        defPro = Guid.Parse(defaultprovider);
                        System.Guid Actguid = System.Guid.NewGuid();

                        string CurDate = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00:000";
                        SqlCommand = new SqlCommand();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAct_1;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@UniqID", Actguid);
                        SqlCommand.Parameters.AddWithValue("@Time", CurDate);
                        SqlCommand.Parameters.AddWithValue("@UserID", defPro);
                        SqlCommand.ExecuteNonQuery();
                        #endregion

                        #region InsertINToClinicalParticipation
                        System.Guid Participationguid = System.Guid.NewGuid();

                        SqlCommand = new SqlCommand();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalParticipation_2;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@UniqID", Participationguid);
                        SqlCommand.Parameters.AddWithValue("@ActID", Actguid);
                        SqlCommand.Parameters.AddWithValue("@PatientUniqID", PatientGuid);
                        SqlCommand.ExecuteNonQuery();
                        #endregion

                        #region InsertINToClinicalParticipation
                        System.Guid Participationguid1 = System.Guid.NewGuid();
                        SqlCommand = new SqlCommand();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalParticipation_2;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@UniqID", Participationguid1);
                        SqlCommand.Parameters.AddWithValue("@ActID", Actguid);
                        SqlCommand.Parameters.AddWithValue("@PatientUniqID", defPro);
                        SqlCommand.ExecuteNonQuery();
                        #endregion

                        #region InsertINToDocuemtn

                        tmpFileOrgName = Path.GetFileName(sourceLocation);

                        SqlCommand = new SqlCommand();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentDocument_3;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@ActID", Actguid);
                        SqlCommand.Parameters.AddWithValue("@FileName", tmpFileOrgName);
                        SqlCommand.Parameters.AddWithValue("@DocumentTypeID", documenttypeGuid);//DocumentTypeID
                        SqlCommand.ExecuteNonQuery();
                        #endregion                                        

                        sourceFileExtenstion = tmpFileOrgName.Substring(tmpFileOrgName.IndexOf("."));
                        tmpFileName = tmpFileOrgName.Substring(0, tmpFileOrgName.IndexOf("."));
                        tmpFileName = tmpFileName + Actguid.ToString() + sourceFileExtenstion;

                        sourceLocation = CommonUtility.GetAditDocTempPath() + "\\" + tmpFileOrgName;
                        dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(tmpFileName));

                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }

                        System.IO.File.Copy(sourceLocation, dstnLocation, true);

                        FileInfo fi = new FileInfo(dstnLocation);
                        FileExtension = fi.Extension;
                        RenameFileName = dr["PatientDoc_Web_ID"].ToString() + FileExtension;

                        PullLiveDatabaseDAL.Update_TreatmentFormDoc_Local_To_EHR(dr["TreatmentDoc_Web_ID"].ToString(), Actguid.ToString());
                        File.Delete(sourceLocation);
                    }
                }
                IsDocUpdate = true;
            }
            catch (Exception ex)
            {
                IsDocUpdate = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsDocUpdate;
        }

        public static int GetPatientEHRID(int PatientId, string email, string firstName)
        {
            int PatientEHRID = 0;
            SqlConnection conn = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (PatientId != 0)
                {
                    string sqlSelect = "Select infpid as newId from ContactPhone where infpid = " + PatientId;
                    SqlCommand SqlCommand = new SqlCommand();
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    PatientEHRID = Convert.ToInt32(SqlCommand.ExecuteScalar());
                }
                if (PatientEHRID == null || PatientEHRID == 0)
                {
                    string sqlSelect = "insert into inf (ContactId, PhoneType, PhoneNumber, Extension, IsDefault, IsSmsEnabled, IsLongDistance, "
                                        + " PhoneDescription, PhoneNote) values ( " + PatientId + ",1,'0000000000',NULL,0,0,0,NULL,NULL ) select @@identity ";
                    SqlCommand SqlCommand = new SqlCommand();
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    PatientEHRID = Convert.ToInt32(SqlCommand.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                PatientEHRID = 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return PatientEHRID;
        }
        public static bool Save_Patient_Form_Local_To_AbelDent(DataTable dtWebPatient_Form)
        {

            //if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            //{
            //Utility.EHR_UserLogin_ID = GetAbelDentUserLoginId();
            //}

            string Update_PatientForm_Record_ID = "";
            string _successfullstataus = string.Empty;
            string InsCode = "";
            string InsSubID = "";
            bool is_Record_Update = false;
            bool isResponsiblePatient = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.AbelDentSQLConnectionServer(ref conn);

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                foreach (DataRow dr in dtWebPatient_Form.Rows)
                {
                    if (dr["Patient_EHR_ID"].ToString() == "")
                    {
                        dr["Patient_EHR_ID"] = "0";
                    }
                    if(Convert.ToInt64(dr["Patient_EHR_ID"].ToString()) != 0)
                    {

                        if (dr["ehrfield"].ToString().ToLower().Trim() == "ixipid_pri_sub".ToLower().Trim() || dr["ehrfield"].ToString().ToLower().Trim() == "ixipid_sec_sub".ToLower().Trim() ||
                            dr["ehrfield"].ToString().ToLower().Trim() == "ixiplanid_pri_ins".ToLower().Trim() || dr["ehrfield"].ToString().ToLower().Trim() == "ixiplanid_sec_ins".ToLower().Trim())
                        {

                        }
                        else
                        {
                            string strQauery = "";                            
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CommonDB.SqlServerCommand("select presides from pat where pid = "+ Convert.ToInt64(dr["Patient_EHR_ID"].ToString()) + "", conn, ref SqlCommand, "txt");                            
                            Int64 ResponsiblePatienID = Convert.ToInt64(SqlCommand.ExecuteScalar().ToString());

                            Utility.WriteToDebugSyncLogFile_All("Check ResponsiblePatientID : " + ResponsiblePatienID + "", "Save_Patient_Form_Local_To_AbelDent");                            

                            if (ResponsiblePatienID != 0)
                            {
                                isResponsiblePatient = true;
                                //pphone,pcitycode,pstreetadr,pstreetadr2,ppostal
                                if (dr["Table_Name"].ToString().ToLower().Trim() == "pat".ToLower().Trim())
                                {
                                    string tmpFieldValue = "";
                                    if (dr["ehrfield"].ToString().ToLower() == "pphone")
                                    {
                                        if (isResponsiblePatient)
                                        {
                                            tmpFieldValue = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");
                                            strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;

                                            strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                            if (strQauery != "" && strQauery != String.Empty)
                                            {
                                                if (conn.State == ConnectionState.Closed) conn.Open();
                                                CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                                SqlCommand.CommandText = strQauery;
                                                SqlCommand.Parameters.Clear();
                                                SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", ResponsiblePatienID.ToString());
                                                SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                                SqlCommand.ExecuteNonQuery();

                                                Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                            }
                                        }
                                    }
                                    else if (dr["ehrfield"].ToString().ToLower() == "pcitycode")
                                    {
                                        if (isResponsiblePatient)
                                        {
                                            tmpFieldValue = dr["ehrfield_value"].ToString().Trim();
                                            string tmpNewPatInsQry = "IF NOT EXISTS (select ccode from cty where cdesc = '" + tmpFieldValue + "' ) BEGIN Insert Into cty (ccode,cdesc) Values (((select MAX(ISNULL(ccode,0))from cty) + 1),'" + tmpFieldValue + "') END ELSE BEGIN  select ccode from cty where cdesc = '" + tmpFieldValue + "'  END ";
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            CommonDB.SqlServerCommand(tmpNewPatInsQry, conn, ref SqlCommand, "txt");
                                            long cityCode = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                            tmpFieldValue = Convert.ToString(cityCode);

                                            strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                            strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                            if (strQauery != "" && strQauery != String.Empty)
                                            {
                                                if (conn.State == ConnectionState.Closed) conn.Open();
                                                CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                                SqlCommand.CommandText = strQauery;
                                                SqlCommand.Parameters.Clear();
                                                SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", ResponsiblePatienID.ToString());
                                                SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                                SqlCommand.ExecuteNonQuery();
                                                Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                            }
                                        }
                                    }
                                    else if (dr["ehrfield"].ToString().ToLower() == "ppostal")
                                    {
                                        if (isResponsiblePatient)
                                        {
                                            if (dr["ehrfield_value"].ToString().Length >= 9)
                                                tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 9) + "'";
                                            else
                                                tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                            strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                            strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                            if (strQauery != "" && strQauery != String.Empty)
                                            {
                                                if (conn.State == ConnectionState.Closed) conn.Open();
                                                CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                                strQauery = strQauery.Replace("@Patient_EHR_ID", ResponsiblePatienID.ToString());
                                                strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                                SqlCommand.CommandText = strQauery;
                                                SqlCommand.ExecuteNonQuery();
                                                Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                            }
                                        }
                                    }
                                    else if (dr["ehrfield"].ToString().ToLower() == "pstreetadr" || dr["ehrfield"].ToString().ToLower() == "pstreetadr2")
                                    {
                                        if (isResponsiblePatient)
                                        {
                                            if (dr["ehrfield_value"].ToString().Length >= 30)
                                                tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 29) + "'";
                                            else
                                                tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                            strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                            strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                            if (strQauery != "" && strQauery != String.Empty)
                                            {
                                                if (conn.State == ConnectionState.Closed) conn.Open();
                                                CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                                strQauery = strQauery.Replace("@Patient_EHR_ID", ResponsiblePatienID.ToString());
                                                strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                                SqlCommand.CommandText = strQauery;
                                                SqlCommand.ExecuteNonQuery();
                                                Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                isResponsiblePatient = false;
                            }

                            if (dr["Table_Name"].ToString().ToLower().Trim() == "pat".ToLower().Trim())
                            {
                                string tmpFieldValue = "";
                                if (dr["ehrfield"].ToString().ToLower() == "pphone")
                                {
                                    if (!isResponsiblePatient)
                                    {
                                        tmpFieldValue = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");
                                        strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;

                                        strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                        if (strQauery != "" && strQauery != String.Empty)
                                        {
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                            SqlCommand.CommandText = strQauery;
                                            SqlCommand.Parameters.Clear();
                                            SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                            SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                            SqlCommand.ExecuteNonQuery();

                                            Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                        }
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "pworkphn")
                                {
                                    tmpFieldValue = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");
                                    strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;

                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                        SqlCommand.ExecuteNonQuery();

                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "pfname")
                                {
                                    if (dr["ehrfield_value"].ToString().Length >= 16)
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 16) + "'";
                                    else
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                    strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.ExecuteNonQuery();
                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "plname")
                                {
                                    if (dr["ehrfield_value"].ToString().Length >= 20)
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 20) + "'";
                                    else
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                    strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.ExecuteNonQuery();
                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "pcitycode")
                                {
                                    if (!isResponsiblePatient)
                                    {
                                        tmpFieldValue = dr["ehrfield_value"].ToString().Trim();
                                        string tmpNewPatInsQry = "IF NOT EXISTS (select ccode from cty where cdesc = '" + tmpFieldValue + "' ) BEGIN Insert Into cty (ccode,cdesc) Values (((select MAX(ISNULL(ccode,0))from cty) + 1),'" + tmpFieldValue + "') END ELSE BEGIN  select ccode from cty where cdesc = '" + tmpFieldValue + "'  END ";
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(tmpNewPatInsQry, conn, ref SqlCommand, "txt");
                                        long cityCode = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                        tmpFieldValue = Convert.ToString(cityCode);

                                        strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                        strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                        if (strQauery != "" && strQauery != String.Empty)
                                        {
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                            SqlCommand.CommandText = strQauery;
                                            SqlCommand.Parameters.Clear();
                                            SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                            SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                            SqlCommand.ExecuteNonQuery();
                                            Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                        }
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "pinitial")
                                {
                                    if (dr["ehrfield_value"].ToString().Length >= 2)
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 2) + "'";
                                    else
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                    strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.ExecuteNonQuery();
                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "pdentist")
                                {
                                    if (dr["ehrfield_value"].ToString().Length >= 5)
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 5) + "'";
                                    else
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                    strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.ExecuteNonQuery();
                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "phygienist")
                                {
                                    if (dr["ehrfield_value"].ToString().Length >= 6)
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 6) + "'";
                                    else
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                    strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.ExecuteNonQuery();
                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "ppostal")
                                {
                                    if (!isResponsiblePatient)
                                    {
                                        if (dr["ehrfield_value"].ToString().Length >= 9)
                                            tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 9) + "'";
                                        else
                                            tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                        strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                        strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                        if (strQauery != "" && strQauery != String.Empty)
                                        {
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                            strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                            strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                            SqlCommand.CommandText = strQauery;
                                            SqlCommand.ExecuteNonQuery();
                                            Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                        }
                                    }
                                }
                                else if(dr["ehrfield"].ToString().ToLower() == "pstreetadr" || dr["ehrfield"].ToString().ToLower() == "pstreetadr2")
                                {
                                    if (!isResponsiblePatient)
                                    {
                                        if (dr["ehrfield_value"].ToString().Length >= 30)
                                            tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString().Substring(0, 29) + "'";
                                        else
                                            tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                        strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                        strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());

                                        if (strQauery != "" && strQauery != String.Empty)
                                        {
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                            strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                            strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                            SqlCommand.CommandText = strQauery;
                                            SqlCommand.ExecuteNonQuery();
                                            Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    if (!isResponsiblePatient)
                                    {
                                        tmpFieldValue = tmpFieldValue + "'" + dr["ehrfield_value"].ToString() + "'";

                                        strQauery = SynchAbelDentQRY.Update_Patient_Record_By_Patient_Form;
                                        strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());


                                        if (strQauery != "" && strQauery != String.Empty)
                                        {
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                            strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                            strQauery = strQauery.Replace("@ehrfield_value", tmpFieldValue);
                                            SqlCommand.CommandText = strQauery;
                                            SqlCommand.ExecuteNonQuery();
                                            Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                        }
                                    }
                                }
                            }
                            else if (dr["Table_Name"].ToString().ToLower().Trim() == "inf".ToLower().Trim())
                            {
                                string tmpFieldValue = "";
                                if (dr["ehrfield"].ToString().ToLower() == "infmobile")
                                {
                                    tmpFieldValue = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");
                                    if (dr["ehrfield_value"].ToString().Length > 10)
                                    {
                                        tmpFieldValue = tmpFieldValue.Substring(0, 10);
                                    }                                   

                                    strQauery = SynchAbelDentQRY.Update_Patient_Info_Record_By_Patient_Form;

                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                        SqlCommand.ExecuteNonQuery();

                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if(dr["ehrfield"].ToString().ToLower() == "infemployer")
                                {
                                    if (dr["ehrfield_value"].ToString().Length >= 20)
                                        tmpFieldValue = tmpFieldValue + "" + dr["ehrfield_value"].ToString().Substring(0, 20) + "";
                                    else
                                        tmpFieldValue = tmpFieldValue + "" + dr["ehrfield_value"].ToString() + "";

                                    strQauery = SynchAbelDentQRY.Update_Patient_Info_Record_By_Patient_Form;


                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                        SqlCommand.ExecuteNonQuery();

                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "infgivenname")
                                {

                                    if (dr["ehrfield_value"].ToString().Length >= 40)
                                        tmpFieldValue = tmpFieldValue + "" + dr["ehrfield_value"].ToString().Substring(0, 40) + "";
                                    else
                                        tmpFieldValue = tmpFieldValue + "" + dr["ehrfield_value"].ToString() + "";

                                    strQauery = SynchAbelDentQRY.Update_Patient_Info_Record_By_Patient_Form;


                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                        SqlCommand.ExecuteNonQuery();

                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else if (dr["ehrfield"].ToString().ToLower() == "infemail")
                                {

                                    if (dr["ehrfield_value"].ToString().Length >= 50)
                                        tmpFieldValue = tmpFieldValue + "" + dr["ehrfield_value"].ToString().Substring(0, 50) + "";
                                    else
                                        tmpFieldValue = tmpFieldValue + "" + dr["ehrfield_value"].ToString() + "";

                                    strQauery = SynchAbelDentQRY.Update_Patient_Info_Record_By_Patient_Form;


                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                        SqlCommand.ExecuteNonQuery();

                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                                else
                                {
                                    strQauery = SynchAbelDentQRY.Update_Patient_Info_Record_By_Patient_Form;

                                    tmpFieldValue = tmpFieldValue + "" + dr["ehrfield_value"].ToString() + "";

                                    strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                                    if (strQauery != "" && strQauery != String.Empty)
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = strQauery;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                        SqlCommand.Parameters.AddWithValue("ehrfield_value", tmpFieldValue);
                                        SqlCommand.ExecuteNonQuery();
                                        Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                                    }
                                }
                            }
                        }
                    }
                }

                DataView NewPatientListdv = new DataView(dtWebPatient_Form);
                NewPatientListdv.RowFilter = "Patient_EHR_ID = '0'";
                DataTable distinctValues = NewPatientListdv.ToTable(true, "PatientForm_Web_ID", "Clinic_Number");

                foreach (DataRow dr in distinctValues.Rows)
                {
                    string tmpNewPat = dr["PatientForm_Web_ID"].ToString();
                    DataView NewPatientdv = new DataView(dtWebPatient_Form);
                    NewPatientdv.RowFilter = "PatientForm_Web_ID = '" + tmpNewPat + "'";

                    DataTable newPatientDt = NewPatientdv.ToTable();

                    string tmpField = "";
                    string tmpFiendValue = "";
                    string tmpField1 = "";
                    string tmpFiendValue1 = "";
                    Int64 PatientId = 0;
                    string tmpNewPatInsQry = "";
                    string tmpNewPriInsId = "";
                    string tmpNewSecInsId = "";
                    string tmpNewPatQry = "", tmpNewPatQry1 = "";

                    DataView NewPatientFielddv = new DataView(newPatientDt);
                    DataTable NewPatientFielddt = NewPatientFielddv.ToTable(true, "ehrfield", "ehrfield_value", "Table_Name");


                    string Pri_SubID = "";
                    string Pri_ins = "";
                    string Pri_ins_companyname = "";
                    string Sec_SubID  = "";
                    string Sec_ins  = "";
                    string Sec_ins_companyname = "";
                    string city = "";

                    foreach (DataRow drNPat in NewPatientFielddt.Rows)
                    {

                        if ((drNPat["Table_Name"].ToString().ToLower() == "pat"))
                        {
                            if (drNPat["ehrfield"].ToString().ToLower() == "pphone")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                if (drNPat["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Length >= 10)
                                {
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Substring(0, 10) + "'" + ",";
                                }
                                else
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";                                                           
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "pdentist")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().ToUpper() + "'" + ",";
                            }
                            else if(drNPat["ehrfield"].ToString().ToLower() == "phygienist")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().ToUpper() + "'" + ",";
                            }
                            else if(drNPat["ehrfield"].ToString().ToLower() == "pworkphn")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                if (drNPat["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Length >= 10)
                                {
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Substring(0, 10) + "'" + ",";
                                }
                                else
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "pinitial")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                if(drNPat["ehrfield_value"].ToString().Length > 2)
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().Substring(0,2) + "'" + ",";
                                else
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "pfname")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                if (drNPat["ehrfield_value"].ToString().Length >= 16)
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().Substring(0, 16) + "'" + ",";
                                else
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "plname")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                if (drNPat["ehrfield_value"].ToString().Length >= 20)
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().Substring(0, 20) + "'" + ",";
                                else
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "pdentist")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                if (drNPat["ehrfield_value"].ToString().Length >= 5)
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().Substring(0, 5) + "'" + ",";
                                else
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }
                            else if (drNPat["ehrfield"].ToString().ToLower() == "phygienist")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                if (drNPat["ehrfield_value"].ToString().Length >= 6)
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().Substring(0, 6) + "'" + ",";
                                else
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }

                            else if (drNPat["ehrfield"].ToString().ToLower() == "pstreetadr" || drNPat["ehrfield"].ToString().ToLower() == "pstreetadr2")
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                if (drNPat["ehrfield_value"].ToString().Length >= 30)
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString().Substring(0, 29) + "'" + ",";
                                else
                                    tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }
                            else if(drNPat["ehrfield"].ToString().ToLower() == "pcitycode")
                            {
                                try
                                {
                                    city = drNPat["ehrfield_value"].ToString();
                                    tmpNewPatInsQry = "IF NOT EXISTS (select ccode from cty where cdesc = '" + city + "' ) BEGIN Insert Into cty (ccode,cdesc) Values (((select MAX(ISNULL(ccode,0))from cty) + 1),'" + city + "') END ELSE BEGIN  select ccode from cty where cdesc = '" + city + "'  END ";
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    CommonDB.SqlServerCommand(tmpNewPatInsQry, conn, ref SqlCommand, "txt");
                                    //city = SqlCommand.ExecuteScalar().ToString();
                                    int citycode = Convert.ToInt32(SqlCommand.ExecuteScalar().ToString());
                                    city = citycode.ToString();
                                    tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                    tmpFiendValue = tmpFiendValue + "'" + city + "'" + ",";
                                }
                                catch (Exception ex)
                                {
                                    Utility.WriteToDebugSyncLogFile_All("Getting Exeption from Insert CITY in PatientFrom " + ex.Message + "", "Save_Patient_Form_Local_To_AbelDent"); 
                                }                                
                            }
                            else
                            {
                                tmpField = tmpField + drNPat["ehrfield"].ToString() + ",";
                                tmpFiendValue = tmpFiendValue + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                            }
                        }
                        else if((drNPat["Table_Name"].ToString().ToLower() == "inf"))
                        {
                            string a = drNPat["ehrfield"].ToString();
                            //if (drNPat["ehrfield"].ToString().ToLower() == "infmobile" || drNPat["ehrfield"].ToString().ToLower() == "infemail" || drNPat["ehrfield"].ToString().ToLower() == "infgivenname" || drNPat["ehrfield"].ToString().ToLower() == "infallownewsemail" || drNPat["ehrfield"].ToString().ToLower() == "infallowtestmsg" || drNPat["ehrfield"].ToString().ToLower() == "infemployer")
                            {
                                if (drNPat["ehrfield"].ToString().ToLower() == "infmobile")
                                {
                                    tmpField1 = tmpField1 + drNPat["ehrfield"].ToString() + ",";
                                    if (drNPat["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Length >= 10)
                                    {
                                        tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "").Substring(0, 10) + "'" + ",";
                                    }
                                    else
                                        tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                                }
                                else if (drNPat["ehrfield"].ToString().ToLower() == "infemployer")
                                {
                                    tmpField1 = tmpField1 + drNPat["ehrfield"].ToString() + ",";
                                    if(drNPat["ehrfield_value"].ToString().Length >= 23)
                                        tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString().Substring(0,23) + "'" + ",";
                                    else
                                        tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                                }
                                else if (drNPat["ehrfield"].ToString().ToLower() == "infgivenname")
                                {
                                    tmpField1 = tmpField1 + drNPat["ehrfield"].ToString() + ",";
                                    if (drNPat["ehrfield_value"].ToString().Length >= 40)
                                        tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString().Substring(0, 40) + "'" + ",";
                                    else
                                        tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                                }
                                else if (drNPat["ehrfield"].ToString().ToLower() == "infemail")
                                {
                                    tmpField1 = tmpField1 + drNPat["ehrfield"].ToString() + ",";
                                    if (drNPat["ehrfield_value"].ToString().Length >= 50)
                                        tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString().Substring(0, 50) + "'" + ",";
                                    else
                                        tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                                }
                                else
                                {
                                    string k = drNPat["ehrfield"].ToString();
                                    tmpField1 = tmpField1 + drNPat["ehrfield"].ToString() + ",";
                                    tmpFiendValue1 = tmpFiendValue1 + "'" + drNPat["ehrfield_value"].ToString() + "'" + ",";
                                }

                            }
                        }
                    }

                    tmpField = tmpField.Remove(tmpField.Length - 1, 1);
                    tmpFiendValue = tmpFiendValue.Remove(tmpFiendValue.Length - 1, 1);

                    tmpNewPatQry = "Insert Into Pat (pid," + tmpField + ") Values (((select MAX(ISNULL(pid,0))from pat) + 1)," + tmpFiendValue + ")";                    
                    Utility.WriteToDebugSyncLogFile_All("PatientForm Pat Insert Query : " + tmpNewPatQry + "", "Save_Patient_Form_Local_To_AbelDent");
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    CommonDB.SqlServerCommand(tmpNewPatQry, conn, ref SqlCommand, "txt");
                    SqlCommand.ExecuteNonQuery();

                    string QueryIdentity = "Select MAX(ISNULL(pid,0))from pat";
                    CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                    PatientId = Convert.ToInt64(SqlCommand.ExecuteScalar());

                    if (tmpField1 != "" && tmpFiendValue1 != "")
                    {
                        tmpField1 = tmpField1.Remove(tmpField1.Length - 1, 1);
                        tmpFiendValue1 = tmpFiendValue1.Remove(tmpFiendValue1.Length - 1, 1);

                        tmpNewPatQry1 = "Insert Into inf (infpid," + tmpField1 + ") Values " + "(" + PatientId + "," + tmpFiendValue1 + ")";                        
                        Utility.WriteToDebugSyncLogFile_All("PatientForm Inf Insert Query : " + tmpNewPatQry1 + "", "Save_Patient_Form_Local_To_AbelDent");
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.SqlServerCommand(tmpNewPatQry1, conn, ref SqlCommand, "txt");
                        SqlCommand.ExecuteNonQuery();
                    }

                    /*if (PatientId != 0)
                    {
                        SqlDataAdapter SqlDa = null;
                        string LegacyPID = "";
                        LegacyPID = Convert.ToString(PatientId);
                        LegacyPID = LegacyPID.PadLeft(6, '0');
                        string sqlSelect = "";

                        string patientid = "";
                        sqlSelect = "";
                        System.Guid PatientGuid;

                        sqlSelect = SynchAbelDentQRY.GetAbelDentClinicalPatient;
                        sqlSelect = sqlSelect.Replace("@PatientId", LegacyPID);
                        Utility.WriteToErrorLogFromAll("GetPatientLegacyPID From Save_PatientForm Query: " + sqlSelect);
                        SqlCommand = new SqlCommand();
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                        DataTable SqlDt = new DataTable();
                        SqlDa.Fill(SqlDt);

                        if (SqlDt != null && SqlDt.Rows.Count > 0)
                        {
                            patientid = SqlDt.Rows[0][0].ToString();
                        }
                        else
                        {
                            Utility.WriteToErrorLogFromAll("From PatientForm Loop Patientguid is null so insert into legacy table : " + LegacyPID);

                            System.Guid ClinicEntityguID = System.Guid.NewGuid();
                            SqlCommand = new SqlCommand();
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = "Insert into ClinicalEntity  (ID,ImplementingClass) VALUES (@ClinicEntityguID,'ClinicalPerson')";
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@ClinicEntityguID", ClinicEntityguID);
                            SqlCommand.ExecuteNonQuery();

                            System.Guid ClinicRoleguID = System.Guid.NewGuid();
                            SqlCommand = new SqlCommand();
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = "Insert into ClinicalRole(ID, StatusCode, EntityID, ImplementingClass, SecurityRoleName, ClassCode, NegationInd) values (@ClinicRoleguID,null,@ClinicEntityguID,'ClinicalRole',NULL,NULL,NULL)";
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@ClinicRoleguID", ClinicRoleguID);
                            SqlCommand.Parameters.AddWithValue("@ClinicEntityguID", ClinicEntityguID);
                            SqlCommand.ExecuteNonQuery();

                            SqlCommand = new SqlCommand();
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = "INSERT into ClinicalPatient (ID,ConfidentialityCode,IsVIP,LegacyPID,PrincipalPhysicianRoleID,PreferredPharmacyID) VALUES (@ClinicRoleguID, '*', 0, '" + LegacyPID + "', NULL, NULL)";
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@ClinicRoleguID", ClinicRoleguID);
                            SqlCommand.ExecuteNonQuery();
                        }
                    }*/

                    foreach (DataRow drNPat in NewPatientFielddt.Rows)
                    {
                        if((drNPat["Table_Name"].ToString().ToLower() == "nsp") || (drNPat["Table_Name"].ToString().ToLower() == "ixi"))
                        {
                            if((drNPat["ehrfield"].ToString().ToLower() == "pri_insurance_companyname") || (drNPat["ehrfield"].ToString().ToLower() == "sec_insurance_companyname"))
                            {
                                if(drNPat["ehrfield"].ToString().ToLower() == "pri_insurance_companyname")
                                {
                                    Pri_ins_companyname = drNPat["ehrfield_value"].ToString();
                                    //tmpNewPriInsId = "IF NOT EXISTS (select nid from nsp where nplanname = '" + Pri_ins_companyname + "' ) BEGIN Insert Into nsp (nid,nplanname,nplnno,ninscoid,ndivsect,noperhandle,nfeeyear,nchargesched,ncopaysched,nspdeductibles,nspcarryover,nfeereplace,nnotes,nnots2,nanniv,nspmaximums,nshowcover,nassigned,nformid,niswelfare,nskipstate,nagerule,nspexclusive,nspdiagreqd,nspsigreqd,nspplantype,nsppreventive,nspbasic,nspmajor,nsportho,nsporthoage,nsporthostudent,nspdefltremainder,nsppercentagelimit) Values ('ADIT','" + Pri_ins_companyname + "','','','',0,'','','','',0,'','','',0000,'',0,1,'',0,0,'','N','N','','',100,100,100,100,0,0,'',NULL) END ELSE BEGIN  select nid from nsp where nplanname = '" + Pri_ins_companyname + "'  END ";
                                    tmpNewPriInsId = "Select ISNULL(inscoid,'') as inscoid from ins where insname Like '%" + Pri_ins_companyname + "%'";
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    CommonDB.SqlServerCommand(tmpNewPriInsId, conn, ref SqlCommand, "txt");
                                    try
                                    {
                                        Pri_ins = (String) SqlCommand.ExecuteScalar();
                                    }
                                    catch
                                    {
                                        Pri_ins = "";
                                    }
                                }
                                else if (drNPat["ehrfield"].ToString().ToLower() == "sec_insurance_companyname")
                                {
                                    Sec_ins_companyname = drNPat["ehrfield_value"].ToString();
                                    //tmpNewSecInsId = "IF NOT EXISTS (select nid from nsp where nplanname = '" + Sec_ins_companyname + "' ) BEGIN Insert Into nsp (nid,nplanname,nplnno,ninscoid,ndivsect,noperhandle,nfeeyear,nchargesched,ncopaysched,nspdeductibles,nspcarryover,nfeereplace,nnotes,nnots2,nanniv,nspmaximums,nshowcover,nassigned,nformid,niswelfare,nskipstate,nagerule,nspexclusive,nspdiagreqd,nspsigreqd,nspplantype,nsppreventive,nspbasic,nspmajor,nsportho,nsporthoage,nsporthostudent,nspdefltremainder,nsppercentagelimit) Values ('ADIT','" + Sec_ins_companyname + "','','','',0,'','','','',0,'','','',0000,'',0,1,'',0,0,'','N','N','','',100,100,100,100,0,0,'',NULL) END ELSE BEGIN  select nid from nsp where nplanname = '" + Sec_ins_companyname + "'  END ";
                                    tmpNewSecInsId = "Select ISNULL(inscoid,'') as inscoid from ins where insname Like '%" + Sec_ins_companyname + "%'";
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    CommonDB.SqlServerCommand(tmpNewSecInsId, conn, ref SqlCommand, "txt");
                                    try
                                    {
                                        Sec_ins = (String)SqlCommand.ExecuteScalar();
                                    }
                                    catch
                                    {
                                        Sec_ins = "";
                                    }
                                }
                            }

                            if((Pri_ins_companyname != "") || (Sec_ins_companyname != ""))
                            {
                                if (drNPat["Table_Name"].ToString().ToLower() == "ixi")
                                {
                                    if (drNPat["ehrfield"].ToString().ToLower() == "ixipid_pri_sub".ToLower())
                                    {
                                        Pri_SubID = drNPat["ehrfield_value"].ToString();
                                    }
                                    else if (drNPat["ehrfield"].ToString().ToLower() == "ixipid_sec_sub".ToLower())
                                    {
                                        Sec_SubID = drNPat["ehrfield_value"].ToString();
                                    }
                                }

                                if ((Pri_ins != "") && (Pri_ins != null))
                                {
                                    if (Pri_SubID == "")
                                        Pri_SubID = PatientId.ToString();
                                    InsUpdatePatientInsurance(Pri_ins, Sec_ins, Pri_SubID, Sec_SubID, PatientId, 1, "Insert");
                                    Pri_ins = "";
                                    Pri_SubID = "";
                                }
                                else if ((Sec_ins != "") && (Sec_ins != null))
                                {
                                    if (Sec_SubID == "")
                                        Sec_SubID = PatientId.ToString();
                                    InsUpdatePatientInsurance(Pri_ins, Sec_ins, Pri_SubID, Sec_SubID, PatientId, 2, "Insert");
                                    Sec_ins = "";
                                    Sec_SubID = "";
                                }
                            }
                            else
                            {
                                if (drNPat["ehrfield"].ToString().ToLower() == "ixipid_pri_sub".ToLower())
                                {
                                    Pri_SubID = drNPat["ehrfield_value"].ToString();
                                    if (Pri_SubID != "")
                                        InsUpdatePatientInsurance(Pri_ins, Sec_ins, Pri_SubID, Sec_SubID, PatientId, 1, "Update");
                                }
                                else if (drNPat["ehrfield"].ToString().ToLower() == "ixipid_sec_sub".ToLower())
                                {
                                    Sec_SubID = drNPat["ehrfield_value"].ToString();
                                    if (Sec_SubID != "")
                                        InsUpdatePatientInsurance(Pri_ins, Sec_ins, Pri_SubID, Sec_SubID, PatientId, 2, "Update");
                                }
                            }
                        }
                    }
                    UpdatePatientEHRIdINPatientForm(PatientId.ToString(), dr["PatientForm_Web_ID"].ToString().Trim());
                    Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + dr["PatientForm_Web_ID"].ToString().Trim() + ";";
                }

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

        private static void InsUpdatePatientInsurance(string pri_ins, string sec_ins, string pri_sub_id, string sec_sub_id, long PatientId, int InsuranceCount, string InsUpd)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                if ((pri_ins != null) && (sec_ins != null))
                {


                    if (InsUpd == "Insert")
                    {
                        pri_ins = pri_ins.Trim();
                        sec_ins = sec_ins.Trim();
                        if (pri_ins.Length >= 8)
                            pri_ins = pri_ins.ToString().Substring(0, 7);
                        if (sec_ins.Length >= 8)
                            sec_ins = sec_ins.ToString().Substring(0, 7);

                        if (InsuranceCount == 1)
                        {
                            string strQauery = "Insert Into ixi (ixipid,ixiplno,ixiplanid,ixisubpid,ixireltosub) Values (" + PatientId + ",1,'" + pri_ins + "'," + pri_sub_id + ",'D')";
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                            SqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            string strQauery = "Insert Into ixi (ixipid,ixiplno,ixiplanid,ixisubpid,ixireltosub) Values (" + PatientId + ",2,'" + sec_ins + "'," + sec_sub_id + ",'I')";
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                            SqlCommand.ExecuteNonQuery();
                        }
                    }
                    else // Update patient ins sub id
                    {
                        if (InsuranceCount == 1)
                        {

                            string strQauery = "Update ixi Set ixisubpid = " + pri_sub_id + " where ixipid = '" + PatientId + "'";
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                            SqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            string strQauery = "Update ixi Set ixisubpid = " + sec_sub_id + " where ixipid = '" + PatientId + "'";
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                            SqlCommand.ExecuteNonQuery();
                        }
                    }
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

        #region Privider OfficeHours & CustomHour

        public static DataSet GetProviderCustomHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetProviderCustomHours;
                SqlSelect = SqlSelect.Replace("@ToDate", ToDate.ToString("yyyy/MM/dd"));
                //  SqlSelect = SqlSelect.Replace("@AbelDentScheduleInterval", Utility.AbelDentScheduleInterval.ToString());
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
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

        public static DataTable GetAbelDentAptMapedData(string ColumnID,DateTime date)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "Select adate ,atime ,achair ,adid ,atimereq from apt where achair in ('" + ColumnID + "') and adate = '" + date.ToString("yyyy-MM-dd") + "' order by adate";
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

        public static DataTable GetAbelDentAptMapedDataBlocks(string ColumnID, DateTime date)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "Select adate ,atime ,achair ,adid ,atimereq from apt where achair in ('" + ColumnID + "') and adate = '" + date.ToString("yyyy-MM-dd") + "' and astatus = 'R' order by adate";
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

        public static DataTable GetColumnProviderData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.ColumnProviderData;
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

        public static void GetAbelDentSystemData(ref DateTime _startDate, ref DateTime _endDate, ref int _unitparMin, ref string _workingDays)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.SystemData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);

                foreach (DataRow item in SqlDt.Rows)
                {
                    _startDate = Convert.ToDateTime(item["startDate"].ToString());
                    _endDate = Convert.ToDateTime(item["endDate"].ToString());
                    _unitparMin = Convert.ToInt32(item["unitParday"].ToString());
                    _workingDays = item["workingDay"].ToString();
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

        public static List<string> GetAbelDentActiveColumnData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            string Column = string.Empty;
            List<string> ListColumn = new List<string>();
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetSlotOPColumns;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //Column = Convert.ToString(SqlCommand.ExecuteScalar());
                //if (Column.Contains('|'))
                //    Column = Column.Replace('|', ',');
                //ListColumn = Column.Split(',').ToList();
                using (SqlDataReader reader = SqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListColumn.Add(reader[0].ToString());
                    }
                }
                return ListColumn;
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

        public static String GetAbelDentProviderName(string ProviderID)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            string Column = "";
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "Select scurrmess from sys where s30daymess = '" + ProviderID + "';";
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                Column = Convert.ToString(SqlCommand.ExecuteScalar());

                return Column;
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

        public static List<AbelOpenings> OpeningsList(int TimeUnits, DateTime StartDate, DateTime EndDate, List<string> ColumnIDs, string Days, bool ReservTime, string ReservWork, int NumberOfOpenings, int _unitNumbersPerDay, DateTime _dayStartTime, DateTime _dayEndTime,int _minutesInUnit,bool isFreeBlock)
        {
            try
            {
                int MinutesInUnit = _minutesInUnit;
                bool flag = true;
                if (TimeUnits == 0)
                    flag = false;
                if (ReservTime && ReservWork == null)
                    flag = false;
                if (flag && string.IsNullOrEmpty(Days))
                    flag = false;
                if (string.IsNullOrEmpty(Days) || Days.Length != 7 || !new Regex("[YN]+").Match(Days).Success)
                    flag = false;
                if (StartDate == DateTime.MinValue || EndDate == DateTime.MinValue || StartDate > EndDate)
                    flag = false;
                if (NumberOfOpenings <= 0)
                    flag = false;
                if (ColumnIDs == null || !ColumnIDs.Any<string>())
                    flag = false;
                if (!flag)
                    return null;

                List<AbelOpenings> source = new List<AbelOpenings>();
                if (!ReservTime)
                    source.AddRange((IEnumerable<AbelOpenings>)FindOpenings(TimeUnits, StartDate, EndDate, ColumnIDs, ValidWeekDays(Days), NumberOfOpenings, _unitNumbersPerDay, _dayStartTime, _dayEndTime, _minutesInUnit, isFreeBlock));

                foreach (AbelOpenings openings in source)
                {
                    openings.StartTimeList = new ObservableCollection<StartTimes>();
                    StartTimes startTimes = new StartTimes()
                    {
                        Time = openings.Time,
                        StartTimeRecords = new ObservableCollection<StartTimes>()
                    };
                    if (openings.Units - TimeUnits > 0)
                    {
                        for (int index = 0; openings.Units - TimeUnits >= index; ++index)
                            startTimes.StartTimeRecords.Add(new StartTimes()
                            {
                                Time = openings.Time.AddMinutes((double)(index * MinutesInUnit)),
                                StartTimeRecords = new ObservableCollection<StartTimes>()
                            });
                    }
                    openings.StartTimeList.Add(startTimes);
                }
                return source;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<AbelOpenings> FindOpenings(int TimeUnits, DateTime StartDate, DateTime EndDate, List<string> ColumnIDs, char[] days, int NumberOfOpenings,int _unitNumbersPerDay,DateTime _dayStartTime,DateTime _dayEndTime,int _minutesInUnit, bool isFreeBlock)
        {
            List<AbelOpenings> source = new List<AbelOpenings>();
            if (TimeUnits > _unitNumbersPerDay)
                TimeUnits = _unitNumbersPerDay;
            DateTime dateTime1;
            if (StartDate.TimeOfDay < _dayStartTime.TimeOfDay)
            {
                dateTime1 = StartDate.Date.AddHours((double)_dayStartTime.Hour);
                StartDate = dateTime1.AddMinutes((double)_dayStartTime.Minute);
            }
            TimeSpan timeOfDay1 = EndDate.TimeOfDay;
            dateTime1 = _dayEndTime;
            TimeSpan timeOfDay2 = dateTime1.TimeOfDay;
            if (timeOfDay1 > timeOfDay2)
            {
                dateTime1 = EndDate.Date;
                dateTime1 = dateTime1.AddHours((double)_dayEndTime.Hour);
                EndDate = dateTime1.AddMinutes((double)_dayEndTime.Minute);
            }

            try
            {
                DateTime dateTime2 = StartDate;
                TimeSpan timeOfDay3 = StartDate.TimeOfDay;
                dateTime1 = _dayStartTime;
                TimeSpan timeOfDay4 = dateTime1.TimeOfDay;
                TimeSpan timeSpan1 = timeOfDay3 - timeOfDay4;
                TimeSpan timeOfDay5 = EndDate.TimeOfDay;
                dateTime1 = _dayStartTime;
                TimeSpan timeOfDay6 = dateTime1.TimeOfDay;
                TimeSpan timeSpan2 = timeOfDay5 - timeOfDay6;
                int int32_1 = Convert.ToInt32(timeSpan1.TotalMinutes / (double)_minutesInUnit);
                int int32_2 = Convert.ToInt32(timeSpan2.TotalMinutes / (double)_minutesInUnit);
                if (int32_2 - int32_1 < TimeUnits)
                    return source;

                do
                {
                    while (!IsWorkingDay(dateTime2) || days[(int)dateTime2.DayOfWeek].Equals('N'))
                        dateTime2 = dateTime2.AddDays(1.0);
                    if (dateTime2 >= EndDate)
                        return source;
                    if(isFreeBlock)
                    {
                        foreach (AbelProviderColumnAptMap populateAllColumnsAppt in (List<AbelProviderColumnAptMap>)PopulateAllColumnsApptMap(ColumnIDs, StartDate, _dayStartTime, _minutesInUnit, _unitNumbersPerDay))
                            source.AddRange(FreeTimeSearchBlock(populateAllColumnsAppt.AptMap, TimeUnits, int32_1, int32_2, dateTime2, EndDate, populateAllColumnsAppt.ColumnID, populateAllColumnsAppt.ProviderID, _dayStartTime, _minutesInUnit));
                    }
                    else
                    {
                        foreach (AbelProviderColumnAptMap populateAllColumnsAppt in (List<AbelProviderColumnAptMap>)PopulateAllColumnsApptMapBlocks(ColumnIDs, dateTime2, _dayStartTime, _minutesInUnit, _unitNumbersPerDay))
                            source.AddRange(BlockTimeSearchBlock(populateAllColumnsAppt.AptMap, TimeUnits, int32_1, int32_2, dateTime2, EndDate, populateAllColumnsAppt.ColumnID, populateAllColumnsAppt.ProviderID, _dayStartTime, _minutesInUnit));
                    }

                    source.OrderBy<AbelOpenings, DateTime>((Func<AbelOpenings, DateTime>)(x => x.Time));
                    dateTime2 = dateTime2.AddDays(1.0);
                }
                while (source.Count<AbelOpenings>() <= NumberOfOpenings);
                return source;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<AbelOpenings> FreeTimeSearchBlockTest(int[] AptMap,int TimeUnits,int BeginUnits,int EndUnits,DateTime currentDate,DateTime EndDate,string columnID,string provider,DateTime _dayStartTime,int _minutesInUnit)
        {
            List<AbelOpenings> openingsList = new List<AbelOpenings>();
            int num = 0;
            int index = BeginUnits;
            for (; index < EndUnits; ++index)
            {
                ++num;
                if (AptMap[index] != 0 || index + 1 == EndUnits)
                {
                    int timeUnits = num - 1;
                    if (index + 1 == EndUnits && AptMap[index] == 0)
                    {
                        ++timeUnits;
                        ++index;
                    }
                    if (timeUnits >= TimeUnits)
                    {
                        DateTime dateTime1 = _dayStartTime;
                        DateTime dateTime2 = dateTime1.AddMinutes((double)(_minutesInUnit * (index - timeUnits)));
                        dateTime1 = currentDate.Date;
                        DateTime dateTime3 = dateTime1.Add(dateTime2.TimeOfDay);
                        if (dateTime3.Date <= EndDate.Date)
                        {
                            dateTime1 = dateTime3.AddMinutes((double)(_minutesInUnit * timeUnits));
                            if (dateTime1.TimeOfDay <= EndDate.TimeOfDay)
                            {
                                AbelOpenings openings = new AbelOpenings()
                                {
                                    ColumnID = columnID,
                                    Provider = provider,
                                    Units = timeUnits,
                                    Date = dateTime3.Date,
                                    Time = dateTime3,
                                    AvailableTime = TimeUnitsAsHoursMinutes(timeUnits, _minutesInUnit)
                                };
                                openingsList.Add(openings);
                            }
                        }
                    }
                    num = 0;
                }
            }
            return openingsList;
        }

        public static List<AbelOpenings> FreeTimeSearchBlock(int[] AptMap, int TimeUnits, int BeginUnits, int EndUnits, DateTime currentDate, DateTime EndDate, string columnID, string provider, DateTime _dayStartTime, int _minutesInUnit)
        {
            List<AbelOpenings> openingsList = new List<AbelOpenings>();
            int num = 0;
            int index = BeginUnits;
            for (; index < EndUnits; ++index)
            {
                ++num;
                if (AptMap[index] != 0 || index + 1 == EndUnits)
                {
                    int timeUnits = num - 1;
                    if (index + 1 == EndUnits && AptMap[index] == 0)
                    {
                        ++timeUnits;
                        ++index;
                    }
                    if (timeUnits >= TimeUnits)
                    {
                        DateTime dateTime1 = _dayStartTime;
                        DateTime dateTime2 = dateTime1.AddMinutes((double)(_minutesInUnit * (index - timeUnits)));
                        dateTime1 = currentDate.Date;
                        DateTime dateTime3 = dateTime1.Add(dateTime2.TimeOfDay);
                        if (dateTime3.Date <= EndDate.Date)
                        {
                            dateTime1 = dateTime3.AddMinutes((double)(_minutesInUnit * timeUnits));
                            if (dateTime1.TimeOfDay <= EndDate.TimeOfDay)
                            {
                                AbelOpenings openings = new AbelOpenings()
                                {
                                    ColumnID = columnID,
                                    Provider = provider,
                                    Units = timeUnits,
                                    Date = dateTime3.Date,
                                    Time = dateTime3,
                                    AvailableTime = TimeUnitsAsHoursMinutes(timeUnits, _minutesInUnit)
                                };
                                openingsList.Add(openings);
                            }
                        }
                    }
                    num = 0;
                }
            }
            return openingsList;
        }

        public static List<AbelOpenings> BlockTimeSearchBlock(int[] AptMap, int TimeUnits, int BeginUnits, int EndUnits, DateTime currentDate, DateTime EndDate, string columnID, string provider, DateTime _dayStartTime, int _minutesInUnit)
        {
            List<AbelOpenings> openingsList = new List<AbelOpenings>();
            int num = 0;
            int index = BeginUnits;
            for (; index < EndUnits; ++index)
            {
                ++num;
                if (AptMap[index] != 1 || index + 1 == EndUnits)
                {
                    int timeUnits = num - 1;
                    if (index + 1 == EndUnits && AptMap[index] == 1)
                    {
                        ++timeUnits;
                        ++index;
                    }
                    if (timeUnits >= TimeUnits)
                    {
                        DateTime dateTime1 = _dayStartTime;
                        DateTime dateTime2 = dateTime1.AddMinutes((double)(_minutesInUnit * (index - timeUnits)));
                        dateTime1 = currentDate.Date;
                        DateTime dateTime3 = dateTime1.Add(dateTime2.TimeOfDay);
                        if (dateTime3.Date <= EndDate.Date)
                        {
                            dateTime1 = dateTime3.AddMinutes((double)(_minutesInUnit * timeUnits));
                            if (dateTime1.TimeOfDay <= EndDate.TimeOfDay)
                            {
                                AbelOpenings openings = new AbelOpenings()
                                {
                                    ColumnID = columnID,
                                    Provider = provider,
                                    Units = timeUnits,
                                    Date = dateTime3.Date,
                                    Time = dateTime3,
                                    AvailableTime = TimeUnitsAsHoursMinutes(timeUnits, _minutesInUnit)
                                };
                                openingsList.Add(openings);
                            }
                        }
                    }
                    num = 0;
                }
            }
            return openingsList;
        }

        public static List<AbelProviderColumnAptMap> PopulateAllColumnsApptMap(List<string> ColumnIDs,DateTime Date, DateTime _dayStartTime,int _minutesInUnit,int _unitNumbersPerDay)
        {
            AbelProviderColumnAptMapList source = new AbelProviderColumnAptMapList(ColumnIDs, _unitNumbersPerDay);

            for (int i = 0; i < ColumnIDs.Count; i++)
            {
                DataTable dt = GetAbelDentAptMapedData(ColumnIDs[i].ToString(), Date);
                try
                {
                    foreach (DataRow dtDtxRow in dt.Rows)
                    {
                        string Achair = dtDtxRow["achair"].ToString().Trim();
                        AbelProviderColumnAptMap providerColumnAptMap = source.FirstOrDefault<AbelProviderColumnAptMap>((Func<AbelProviderColumnAptMap, bool>)(x => x.ColumnID.Equals(Achair)));
                        if (providerColumnAptMap != null)
                        {
                            int num1 = (int)(Convert.ToDateTime(dtDtxRow["atime"].ToString()) - _dayStartTime).TotalMinutes / _minutesInUnit;
                            int num2 = num1 + Convert.ToInt32(dtDtxRow["atimereq"]);
                            for (int index = num1; index < num2 && index < _unitNumbersPerDay; ++index)
                                providerColumnAptMap.AptMap[index] = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return source;
        }

        public static List<AbelProviderColumnAptMap> PopulateAllColumnsApptMapBlocks(List<string> ColumnIDs, DateTime Date, DateTime _dayStartTime, int _minutesInUnit, int _unitNumbersPerDay)
        {
            AbelProviderColumnAptMapList source = new AbelProviderColumnAptMapList(ColumnIDs, _unitNumbersPerDay);

            for (int i = 0; i < ColumnIDs.Count; i++)
            {
                DataTable dt = GetAbelDentAptMapedDataBlocks(ColumnIDs[i].ToString(), Date);
                try
                {
                    foreach (DataRow dtDtxRow in dt.Rows)
                    {
                        string Achair = dtDtxRow["achair"].ToString().Trim();
                        AbelProviderColumnAptMap providerColumnAptMap = source.FirstOrDefault<AbelProviderColumnAptMap>((Func<AbelProviderColumnAptMap, bool>)(x => x.ColumnID.Equals(Achair)));
                        if (providerColumnAptMap != null)
                        {
                            int num1 = (int)(Convert.ToDateTime(dtDtxRow["atime"].ToString()) - _dayStartTime).TotalMinutes / _minutesInUnit;
                            int num2 = num1 + Convert.ToInt32(dtDtxRow["atimereq"]);
                            for (int index = num1; index < num2 && index < _unitNumbersPerDay; ++index)
                                providerColumnAptMap.AptMap[index] = 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return source;
        }

        public static string TimeUnitsAsHoursMinutes(int timeUnits, int _minutesInUnit)
        {
            _ = string.Empty;
            int num = timeUnits * _minutesInUnit;
            int num2 = (num - num % 60) / 60;
            int num3 = num % 60;
            return num2 + "hr" + num3;
        }

        public static bool IsWorkingDay(DateTime date)
        {
            bool flag = false;

            DayOfWeek dayOfWeek = date.DayOfWeek;
            if (!flag && dayOfWeek == DayOfWeek.Monday)    //Monday    
                flag = true;
            if (!flag && dayOfWeek == DayOfWeek.Tuesday)   //Tuesday
                flag = true;
            if (!flag && dayOfWeek == DayOfWeek.Wednesday) //Wednesday
                flag = true;
            if (!flag && dayOfWeek == DayOfWeek.Thursday)  //Thrusday
                flag = true;
            if (!flag && dayOfWeek == DayOfWeek.Friday)    //Friday
                flag = true;
            if (!flag && dayOfWeek == DayOfWeek.Saturday)  //Saturday
                flag = true;
            if (!flag && dayOfWeek == DayOfWeek.Sunday)    //Sunday
                flag = true;
            return flag;
        }

        private static char[] ValidWeekDays(string Days)
        {
            char[] chArray = new char[7];
            if (!string.IsNullOrEmpty(Days))
            {
                Days = Days.ToUpper();
                if (Days.Length == 7)
                {
                    chArray[0] = Days[6];
                    for (int index = 0; index < 6; ++index)
                        chArray[index + 1] = Days[index];
                }
            }
            return chArray;
        }



        #endregion

        #region MedicleForm

        public static DataTable GetAbelDentMedicleFormData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                string SqlSelect = "";
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if (Utility.Application_Version == "15")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMedicleFormData_15;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMedicleFormData;
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

        public static DataTable GetAbelDentMedicalAnswerData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
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

        public static DataTable GetAbelDentMedicleFormQuestionData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                string SqlSelect = "";
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if (Utility.Application_Version == "15")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMedicleFormQuestionData_15;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMedicleFormQuestionData;
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);

                DataTable PartialAbelDentControldt = SqlDt.Copy();
                foreach (DataRow dr in SqlDt.Rows)
                {
                    DataTable PartialControldt = GetAbelDentMediclePartialQuestionData(dr["AbelDent_Question_EHR_ID"].ToString());

                    if (PartialControldt.Rows.Count > 0)
                    {
                        foreach (DataRow partialrow in PartialControldt.Rows)
                        {

                            bool is_required = false;

                            DataRow addnewrow = PartialAbelDentControldt.NewRow();
                            addnewrow["AbelDent_FormQuestion_LocalDB_ID"] = dr["AbelDent_FormQuestion_LocalDB_ID"].ToString();
                            addnewrow["AbelDent_FormQuestion_Web_ID"] = dr["AbelDent_FormQuestion_Web_ID"].ToString();
                            addnewrow["AbelDent_Form_EHRUnique_ID"] = dr["AbelDent_Form_EHRUnique_ID"].ToString();
                            addnewrow["AbelDent_Question_EHR_ID"] = dr["AbelDent_Question_EHR_ID"].ToString();
                            if (Utility.Application_Version == "15")
                            {
                                addnewrow["AbelDent_Question_EHRUnique_ID"] = dr["AbelDent_Form_EHRUnique_ID"].ToString() + "_" + dr["AbelDent_Question_EHRUnique_ID"].ToString();
                            }
                            else
                            {
                                addnewrow["AbelDent_Question_EHRUnique_ID"] = dr["AbelDent_Form_EHRUnique_ID"].ToString() + "_" + dr["AbelDent_Question_EHRUnique_ID"].ToString() + "_" + dr["AbelDent_Question_EHR_ID"].ToString() + "_" + dr["AbelDent_QuestionsTypeId"].ToString() + "_" + partialrow["AbelDent_ResponsetypeId"].ToString();
                            }
                            addnewrow["AbelDent_QuestionsTypeId"] = dr["AbelDent_QuestionsTypeId"].ToString();
                            addnewrow["AbelDent_QyestionTypeName"] = partialrow["AbelDent_QuestionTypeName"].ToString();
                            addnewrow["AbelDent_ResponsetypeId"] = partialrow["AbelDent_ResponsetypeId"].ToString();
                            addnewrow["AbelDent_QuestionName"] = partialrow["AbelDent_QuestionName"].ToString();
                            addnewrow["AbelDent_Question_DefaultValue"] = dr["AbelDent_Question_DefaultValue"].ToString();
                            addnewrow["QuestionVersion"] = dr["QuestionVersion"].ToString();
                            addnewrow["QuestionVersion_Date"] = (dr["QuestionVersion_Date"].ToString() == "" || dr["QuestionVersion_Date"].ToString() == string.Empty) ? DateTime.MinValue : Convert.ToDateTime(dr["QuestionVersion_Date"].ToString());
                            addnewrow["InputType"] = partialrow["InputType"].ToString();
                            addnewrow["Is_OptionField"] = Convert.ToBoolean(partialrow["Is_OptionField"].ToString());
                            addnewrow["Options"] = partialrow["Options"].ToString(); ;
                            addnewrow["Is_Required"] = is_required;
                            addnewrow["QuestionOrder"] = dr["QuestionOrder"].ToString();
                            addnewrow["EHR_Entry_DateTime"] = dr["EHR_Entry_DateTime"].ToString();
                            addnewrow["Last_Sync_Date"] = dr["Last_Sync_Date"].ToString();
                            addnewrow["Entry_DateTime"] = dr["Entry_DateTime"].ToString();
                            addnewrow["Is_Adit_Updated"] = dr["Is_Adit_Updated"].ToString();
                            addnewrow["is_deleted"] = dr["is_deleted"].ToString();
                            addnewrow["Is_MultiField"] = true;
                            addnewrow["Clinic_Number"] = "0";
                            addnewrow["Service_Install_Id"] = "1";
                            PartialAbelDentControldt.Rows.Add(addnewrow);
                        }
                        DataRow deletedr = PartialAbelDentControldt.Select("AbelDent_Form_EHRUnique_ID = '" + dr["AbelDent_Form_EHRUnique_ID"].ToString() + "' and AbelDent_Question_EHRUnique_ID = '" + dr["AbelDent_Question_EHRUnique_ID"].ToString() + "' and AbelDent_ResponsetypeId = 0").FirstOrDefault();
                        PartialAbelDentControldt.Rows.Remove(deletedr);
                    }
                    else
                    {
                        DataRow Editdr = PartialAbelDentControldt.Select("AbelDent_Form_EHRUnique_ID = '" + dr["AbelDent_Form_EHRUnique_ID"].ToString() + "' and AbelDent_Question_EHRUnique_ID = '" + dr["AbelDent_Question_EHRUnique_ID"].ToString() + "'").FirstOrDefault();
                        GetAbelDentMedicleStaticQuestionData(PartialAbelDentControldt, ref Editdr);
                    }
                }
                return PartialAbelDentControldt;
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

        public static DataTable GetAbelDentMediclePartialQuestionData(string AbelDent_QuestionsTypeId)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                string SqlSelect = "";
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if (Utility.Application_Version == "15")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMediclePartialQuestionData_15;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMediclePartialQuestionData;
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("AbelDent_Question_EHR_ID", AbelDent_QuestionsTypeId);
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

        public static bool SaveMedicalHistoryLocalToAbelDent()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            bool isfirstTimeinsert = false;

            try
            {
                DataTable dtWebPatient_FormMedicalHistory = SynchLocalDAL.GetLiveAbelDentPatientFormMedicalHistoryData();
                if (dtWebPatient_FormMedicalHistory != null)
                {
                    if (dtWebPatient_FormMedicalHistory.Rows.Count > 0)
                    {
                       // Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 2");
                        DataTable LocalRespoanseSetDt = dtWebPatient_FormMedicalHistory.Copy().DefaultView.ToTable(true, "Patient_EHR_ID", "AbelDent_Form_EHRUnique_ID", "PatientForm_Web_ID");
                        //Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 3");
                        if (LocalRespoanseSetDt != null)
                        {
                            if (Utility.Application_Version == "15")
                            {
                                foreach (DataRow dr in LocalRespoanseSetDt.Rows)
                                {
                                   // Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 4");
                                    int ResponsesetId = 0;
                                    string AnswerResponseEHRId = "";

                                    if (!isfirstTimeinsert)
                                    {
                                       // Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 6");
                                        string sqlSelect = string.Empty;
                                        string ResponseId = string.Empty;
                                        CommonDB.AbelDentSQLConnectionServer(ref conn);
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentHealthHistoryResponse_15;
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Timezone", Utility.LocationTimeZone);
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@Patient_EHR_id", dr["Patient_EHR_ID"].ToString());
                                        SqlCommand.Parameters.AddWithValue("@AbelDent_FormUniqueId", dr["AbelDent_Form_EHRUnique_ID"].ToString());
                                        ResponsesetId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                                        sqlSelect = string.Empty;
                                        CommonDB.AbelDentSQLConnectionServer(ref conn);
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentHealthHistoryReview_15;
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ResponseEHRID", ResponsesetId.ToString());
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Timezone", Utility.LocationTimeZone);
                                        SqlCommand.ExecuteNonQuery();
                                        //Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 7");
                                        isfirstTimeinsert = true;
                                    }

                                    DataRow[] LocalRespoanseDt = dtWebPatient_FormMedicalHistory.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString() + "' and AbelDent_Form_EHRUnique_ID = '" + dr["AbelDent_Form_EHRUnique_ID"].ToString() + "'");
                                    //Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 9");

                                    if (LocalRespoanseDt != null)
                                    {
                                        foreach (DataRow drRespoanse in LocalRespoanseDt)
                                        {
                                          //  Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 10");
                                            string AnswerId = "";
                                            if (drRespoanse["Answer_Value"].ToString().Trim() != "" || drRespoanse["Answer_Value"].ToString().Trim() != string.Empty)
                                            {
                                                string[] arques = drRespoanse["AbelDent_Question_EHRUnique_ID"].ToString().Split('_');

                                              //  Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 11");

                                                AbelDentQuestionIds AllQues = new AbelDentQuestionIds();
                                                AllQues.AbelDent_Form_EHRUnique_ID = arques[0];
                                                AllQues.AbelDent_Question_EHRUnique_ID = arques[1];

                                                string sqlSelect = string.Empty;
                                                sqlSelect = string.Empty;
                                                CommonDB.AbelDentSQLConnectionServer(ref conn);
                                                if (conn.State == ConnectionState.Closed) conn.Open();
                                                sqlSelect = SynchAbelDentQRY.GetAbelDentMedicationQuestionDate_15;
                                                sqlSelect = sqlSelect.Replace("@AnswerText", drRespoanse["Answer_Value"].ToString().Trim());
                                                sqlSelect = sqlSelect.Replace("@QuestionEHRID", AllQues.AbelDent_Question_EHRUnique_ID);
                                                SqlCommand = new SqlCommand();                                                
                                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                                DataTable SqlDt = new DataTable();
                                                SqlDa.Fill(SqlDt);

                                                if (SqlDt != null && SqlDt.Rows.Count > 0)
                                                {
                                                    AnswerId = SqlDt.Rows[0][0].ToString();
                                                }

                                                if(AnswerId == "")
                                                {
                                                    sqlSelect = string.Empty;
                                                    sqlSelect = string.Empty;
                                                    CommonDB.AbelDentSQLConnectionServer(ref conn);
                                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                                    sqlSelect = "select Id from HealthHistoryAnswerOption where Text = '' and QuestionId = '@QuestionEHRID'";
                                                    sqlSelect = sqlSelect.Replace("@QuestionEHRID", AllQues.AbelDent_Question_EHRUnique_ID);
                                                    SqlCommand = new SqlCommand();
                                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                                    SqlDt = new DataTable();
                                                    SqlDa.Fill(SqlDt);

                                                    if (SqlDt != null && SqlDt.Rows.Count > 0)
                                                    {
                                                        AnswerId = SqlDt.Rows[0][0].ToString();
                                                    }
                                                }

                                               // Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 12");

                                                
                                              //  Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 13");

                                                CommonDB.AbelDentSQLConnectionServer(ref conn);
                                                if (conn.State == ConnectionState.Closed) conn.Open();
                                                System.Guid guid = System.Guid.NewGuid();
                                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                                //if (drRespoanse["InputType"].ToString().Trim().ToLower() == "textbox")
                                                //{
                                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentHealthHistoryAnswerResponse_15;
                                                    SqlCommand.Parameters.Clear();
                                                    SqlCommand.Parameters.AddWithValue("@ResponseID", ResponsesetId);
                                                    SqlCommand.Parameters.AddWithValue("@AnswerId", AnswerId);
                                                    SqlCommand.Parameters.AddWithValue("@Text", drRespoanse["Answer_Value"].ToString());
                                                    AnswerResponseEHRId = SqlCommand.ExecuteScalar().ToString();
                                                //}
                                                //else
                                                //{
                                                    //SqlCommand.CommandText = "Insert into HealthHistoryAnswerResponse (AnswerId,ResponseId) VALUES (@AnswerId,@ResponseID);Select @@Identity";
                                                    //SqlCommand.Parameters.Clear();
                                                    //SqlCommand.Parameters.AddWithValue("@ResponseID", ResponsesetId);
                                                    //SqlCommand.Parameters.AddWithValue("@AnswerId", AnswerId);                                                        
                                                    //AnswerResponseEHRId = SqlCommand.ExecuteScalar().ToString();
                                                //}

                                                sqlSelect = string.Empty;
                                                CommonDB.AbelDentSQLConnectionServer(ref conn);
                                                if (conn.State == ConnectionState.Closed) conn.Open();
                                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                                SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentHealthHistoryReviewDetails_15;
                                                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Timezone", Utility.LocationTimeZone);
                                                SqlCommand.Parameters.Clear();
                                                SqlCommand.Parameters.AddWithValue("@HealthHistoryReviewId", ResponsesetId);
                                                SqlCommand.Parameters.AddWithValue("@HealthHistoryId", dr["AbelDent_Form_EHRUnique_ID"].ToString());
                                                SqlCommand.Parameters.AddWithValue("@QuestionId", AllQues.AbelDent_Question_EHRUnique_ID);
                                                SqlCommand.ExecuteNonQuery();

                                              //  Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 14");

                                               
                                                //sqlSelect = string.Empty;
                                                //CommonDB.AbelDentSQLConnectionServer(ref conn);
                                                //if (conn.State == ConnectionState.Closed) conn.Open();
                                                //System.Guid guid = System.Guid.NewGuid();
                                                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                                //SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentHealthHistoryResponse_15;
                                                //SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Timezone", Utility.LocationTimeZone);
                                                //SqlCommand.Parameters.Clear();
                                                //SqlCommand.Parameters.AddWithValue("@Patient_EHR_id", dr["Patient_EHR_ID"].ToString());
                                                //SqlCommand.Parameters.AddWithValue("@AbelDent_FormUniqueId", dr["AbelDent_Form_EHRUnique_ID"].ToString());
                                                //ResponsesetId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                                //Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 15");
                                                                                              
                                            }
                                            else
                                            {
                                                AnswerResponseEHRId = "0";
                                            }

                                            //Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 16");
                                            UpdateResponseUniqueEHRIdInAbelDent_Response(AnswerResponseEHRId, drRespoanse["AbelDent_Question_EHRUnique_ID"].ToString());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow dr in LocalRespoanseSetDt.Rows)
                                {
                                    DataTable AbelDentRespoanseSetDt = GetAbelDentMediclePartialResponseData(dr["AbelDent_Form_EHRUnique_ID"].ToString(), dr["Patient_EHR_ID"].ToString());
                                    string ResponsesetId = "";
                                    if (AbelDentRespoanseSetDt.Rows.Count == 0)
                                    {
                                        string sqlSelect = string.Empty;
                                        string ResponseId = string.Empty;
                                        CommonDB.AbelDentSQLConnectionServer(ref conn);
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentMedicleResponseSetData;                                        
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@Patient_EHR_id", dr["Patient_EHR_ID"].ToString());
                                        SqlCommand.Parameters.AddWithValue("@AbelDent_FormUniqueId", dr["AbelDent_Form_EHRUnique_ID"].ToString());
                                        SqlCommand.ExecuteNonQuery();

                                        string QueryIdentity = "Select max(Id) as newId from SurveyResponse";
                                        CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                                        ResponsesetId = Convert.ToString(SqlCommand.ExecuteScalar());
                                    }
                                    else
                                    {
                                        ResponsesetId = Convert.ToString(AbelDentRespoanseSetDt.Rows[0]["responsesetuniqueidMain"].ToString());
                                    }

                                    DataRow[] LocalRespoanseDt = dtWebPatient_FormMedicalHistory.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString() + "' and AbelDent_Form_EHRUnique_ID = '" + dr["AbelDent_Form_EHRUnique_ID"].ToString() + "'");

                                    if (LocalRespoanseDt != null)
                                    {
                                        foreach (DataRow drRespoanse in LocalRespoanseDt)
                                        {
                                            string responseuniqueid = "";
                                            string AnswerId = "";
                                            if (drRespoanse["Answer_Value"].ToString().Trim() != "" || drRespoanse["Answer_Value"].ToString().Trim() != string.Empty)
                                            {
                                                string[] arques = drRespoanse["AbelDent_Question_EHRUnique_ID"].ToString().Split('_');

                                                AbelDentQuestionIds AllQues = new AbelDentQuestionIds();
                                                AllQues.AbelDent_Form_EHRUnique_ID = arques[0];
                                                AllQues.AbelDent_Question_EHRUnique_ID = arques[1];
                                                AllQues.AbelDent_Question_EHR_ID = arques[2];
                                                AllQues.AbelDent_QuestionsTypeId = arques[3];
                                                AllQues.AbelDent_ResponsetypeId = arques[4];

                                                string QueryIdentity = "select Id as AnswerId from SurveyAnswer where QuestionId = '" + AllQues.AbelDent_Question_EHRUnique_ID + "'";
                                                CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                                                AnswerId = Convert.ToString(SqlCommand.ExecuteScalar());

                                                if (AnswerId != String.Empty)
                                                {
                                                    DataRow[] AbelDentRespoanseDr = AbelDentRespoanseSetDt.Copy().Select("responsesetuniqueid = '" + ResponsesetId + "' and questionuniqueid = '" + AllQues.AbelDent_Question_EHRUnique_ID + "' and responsetype = '" + AllQues.AbelDent_ResponsetypeId + "'");
                                                    if (AbelDentRespoanseDr == null || AbelDentRespoanseDr.Count() == 0)
                                                    {
                                                        string sqlSelect = string.Empty;
                                                        CommonDB.AbelDentSQLConnectionServer(ref conn);
                                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                                        System.Guid guid = System.Guid.NewGuid();
                                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                                        if (Utility.Application_Version == "15")
                                                        {
                                                            SqlCommand.CommandText = "";//SynchAbelDentQRY.InsertAbelDentMedicleResponseData_15;
                                                        }
                                                        else
                                                        {
                                                            SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentMedicleResponseData;
                                                        }
                                                        SqlCommand.Parameters.Clear();

                                                        SqlCommand.Parameters.AddWithValue("@GuidId", guid);
                                                        SqlCommand.Parameters.AddWithValue("@ResponseID", ResponsesetId);
                                                        SqlCommand.Parameters.AddWithValue("@AnswerId", AnswerId);
                                                        SqlCommand.Parameters.AddWithValue("@Text", drRespoanse["Answer_Value"].ToString());
                                                        SqlCommand.ExecuteNonQuery();

                                                    }
                                                    else
                                                    {
                                                        string sqlSelect = string.Empty;
                                                        CommonDB.AbelDentSQLConnectionServer(ref conn);
                                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                                        System.Guid guid = System.Guid.NewGuid();
                                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentMedicleResponseData;
                                                        SqlCommand.Parameters.Clear();

                                                        SqlCommand.Parameters.AddWithValue("@GuidId", guid);
                                                        SqlCommand.Parameters.AddWithValue("@ResponseID", ResponsesetId);
                                                        SqlCommand.Parameters.AddWithValue("@AnswerId", AnswerId);
                                                        SqlCommand.Parameters.AddWithValue("@Text", drRespoanse["Answer_Value"].ToString());
                                                        SqlCommand.ExecuteNonQuery();
                                                    }
                                                }
                                                else
                                                {
                                                    responseuniqueid = "0";
                                                }

                                                string QryIdentity = "Select Id as newId from SurveyAnswerResponse where AnswerId = '" + AnswerId + "'";
                                                CommonDB.SqlServerCommand(QueryIdentity, conn, ref SqlCommand, "txt");
                                                responseuniqueid = Convert.ToString(SqlCommand.ExecuteScalar());

                                            }
                                            else
                                            {
                                                responseuniqueid = "0";
                                            }
                                            UpdateResponseUniqueEHRIdInAbelDent_Response(responseuniqueid, drRespoanse["AbelDent_Question_EHRUnique_ID"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Insert HistoryLocal to Abeldent Step - 17 Exception : " + ex.Message);
                return false;
                
                throw ex;
            }
            finally
            {
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static bool UpdateResponseUniqueEHRIdInAbelDent_Response(string responseuniqueid, string AbelDent_Question_EHRUnique_ID)
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
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_AbelDent_Response_Response_EHR_ID;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("AbelDent_Response_EHR_ID", responseuniqueid);
                            SqlCeCommand.Parameters.AddWithValue("AbelDent_Question_EHRUnique_ID", AbelDent_Question_EHRUnique_ID);
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        _successfullstataus = true;

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

        public static void GetAbelDentMedicleStaticQuestionData(DataTable PartialAbelDentControldt, ref DataRow dr)
        {
            DataRow Editdr = PartialAbelDentControldt.Select("AbelDent_Form_EHRUnique_ID = '" + dr["AbelDent_Form_EHRUnique_ID"].ToString() + "' and AbelDent_Question_EHRUnique_ID = '" + dr["AbelDent_Question_EHRUnique_ID"].ToString() + "'").FirstOrDefault();
            string AbelDent_QuestionsTypeId = dr["AbelDent_QuestionsTypeId"].ToString();
            string option = "";
            string DefaultResponseText = "";
            try
            {
                DataSet ds = new DataSet();
                StringReader theReader = new StringReader(dr["questioninfo"].ToString());
                ds.ReadXml(theReader);
                Editdr["AbelDent_Question_EHRUnique_ID"] = dr["AbelDent_Form_EHRUnique_ID"].ToString() + "_" + Editdr["AbelDent_Question_EHRUnique_ID"].ToString() + "_" + Editdr["AbelDent_Question_EHR_ID"].ToString() + "_" + Editdr["AbelDent_QuestionsTypeId"].ToString() + "_" + Editdr["AbelDent_QuestionsTypeId"].ToString();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains("QuestionText"))
                    {
                        Editdr["AbelDent_QyestionTypeName"] = ds.Tables[0].Rows[0]["QuestionText"].ToString();
                        Editdr["AbelDent_QuestionName"] = ds.Tables[0].Rows[0]["QuestionText"].ToString();
                    }
                    else
                    {
                        Editdr["AbelDent_QyestionTypeName"] = "";
                        Editdr["AbelDent_QuestionName"] = "";
                    }
                    //   Editdr["AbelDent_QyestionTypeName"] = ds.Tables[0].Rows[0]["QuestionText"].ToString();
                    Editdr["AbelDent_ResponsetypeId"] = dr["AbelDent_QuestionsTypeId"].ToString();
                    //  
                    if (ds.Tables[0].Columns.Contains("IsRequired"))
                    {
                        Editdr["Is_Required"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRequired"].ToString());
                    }
                    else
                    {
                        Editdr["Is_Required"] = 0;
                    }
                    DefaultResponseText = ds.Tables[0].Rows[0]["DefaultResponseText"].ToString().Trim();
                }
                else
                {
                    Editdr["AbelDent_QyestionTypeName"] = "";
                    Editdr["AbelDent_QuestionName"] = "";
                    Editdr["Is_Required"] = 0;
                    DefaultResponseText = "";
                }
            }
            catch
            {

            }
            //    Editdr["Is_Required"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRequired"].ToString());
            Editdr["QuestionOrder"] = dr["QuestionOrder"].ToString();
            switch (AbelDent_QuestionsTypeId)
            {
                case "3":   // Body Text
                    Editdr["InputType"] = "Label";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    break;
                case "4": // Header Text
                    Editdr["InputType"] = "Label";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    break;
                case "5": // Sub Header Text
                    Editdr["InputType"] = "Label";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    break;
                case "6": // Consered Text
                    Editdr["InputType"] = "Label";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    break;
                case "7": // Note Response 
                    Editdr["InputType"] = "TextBox";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    Editdr["AbelDent_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "8": // Short Text Response
                    Editdr["InputType"] = "TextBox";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    Editdr["AbelDent_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "9": // DateTime
                    Editdr["InputType"] = "DateTime";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    Editdr["AbelDent_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "10": // Yes Or No Response
                    Editdr["InputType"] = "RadioButton";
                    Editdr["Is_OptionField"] = true;
                    Editdr["Options"] = "Yes[1],No[0]";
                    Editdr["AbelDent_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "11": // Number Response
                    Editdr["InputType"] = "Number";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "0";
                    Editdr["AbelDent_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "12": // Amount Response
                    Editdr["InputType"] = "Number";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "0.00";
                    Editdr["AbelDent_Question_DefaultValue"] = DefaultResponseText;
                    break;
                case "13": //One choice from list
                    Editdr["InputType"] = "RadioButton";
                    Editdr["Is_OptionField"] = true;
                    //option = ds.Tables[0].Rows[0]["DefaultResponseText"].ToString().Replace("]", "").Replace(Environment.NewLine, ",");
                    string[] rbtn = DefaultResponseText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    option = "";
                    foreach (string s in rbtn)
                    {
                        option = option + s.Replace(",", " Or ") + "[" + Array.IndexOf(rbtn, s).ToString() + "],";
                    }
                    option = option.TrimEnd(',');
                    Editdr["Options"] = option;
                    break;
                case "14": // Checkbox List
                    Editdr["InputType"] = "CheckBox";
                    Editdr["Is_OptionField"] = true;
                    // option = ds.Tables[0].Rows[0]["DefaultResponseText"].ToString().Replace("]", "]").Replace(Environment.NewLine, ",");
                    string[] cbhktn = DefaultResponseText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    option = "";
                    foreach (string s in cbhktn)
                    {
                        option = option + s.Replace(",", " Or ") + "[" + Array.IndexOf(cbhktn, s).ToString() + "],";
                    }
                    option = option.TrimEnd(',');
                    Editdr["Options"] = option;
                    break;
                case "15": // Confirmation
                    Editdr["InputType"] = "Confirmation";
                    Editdr["Is_OptionField"] = false;
                    Editdr["Options"] = "";
                    Editdr["AbelDent_Question_DefaultValue"] = DefaultResponseText;
                    break;

            }

        }

        public static DataTable GetAbelDentMediclePartialResponseData(string AbelDent_FormUniqueId, string Patient_EHR_id)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                string SqlSelect = "";
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                if(Utility.Application_Version == "15")
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMediclePartialResponseData_15;
                }
                else
                {
                    SqlSelect = SynchAbelDentQRY.GetAbelDentMediclePartialResponseData;
                }

                SqlSelect = SqlSelect.Replace("@PatientID", Patient_EHR_id);
                SqlSelect = SqlSelect.Replace("@SurveryID", AbelDent_FormUniqueId);

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

        public static bool SavePatientMedicationLocalToAbelDent(ref bool isRecordSaved, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            string sqlSelect = "";
            string DoseQuantityValue = "";
            string DoseQuantityUnits = "";
            bool IsSaveMedication = false;
            string MedicationPatientId;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            DataTable dtPatientMedication;
            string CurDate = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00:000";
            string CurDateAdd = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd") + " 00:00:00:000";
            try
            {

                dtPatientMedication = GetAbelDentPatientMedicationData("");

                System.Threading.Thread.Sleep(240000);


                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR(Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), strPatientFormID);
                    if (dtPatientMedicationResponse != null)
                    {
                        if (dtPatientMedicationResponse.Rows.Count > 0)
                        {
                            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                            {
                                Utility.EHR_UserLogin_ID = GetAbelDentUserLoginId();
                            }

                            foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                            {
                                MedicationPatientId = "";
                                string LegacyPID = "";
                                LegacyPID = dr["PatientEHRId"].ToString();
                                LegacyPID = LegacyPID.PadLeft(6, '0');

                                string MedicationNote = "Medication: " + dr["Medication_Name"].ToString() + ", Comment:" + dr["Medication_Note"].ToString();

                                if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "0";
                                if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" && dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                                {
                                    MedicationPatientId = dr["PatientMedication_EHR_Id"].ToString();
                                }

                                if (MedicationPatientId == "")
                                {
                                    DataRow[] drPatMedRow = dtPatientMedication.Copy().Select("Patient_EHR_ID = " + dr["PatientEHRId"].ToString().Trim() + " And Medication_Name = '" + dr["Medication_Name"].ToString().Trim() + "' And Is_Active=true ");
                                    if (drPatMedRow.Length > 0)
                                    {
                                        MedicationPatientId = drPatMedRow[0]["PatientMedication_EHR_ID"].ToString();
                                    }
                                }

                                if (MedicationPatientId == "")
                                {
                                    if (Utility.Application_Version == "15")
                                    {
                                        string DrugName = "";
                                        string DrugStrength = "";
                                        string SqlSelect = SynchAbelDentQRY.GetMedicationRxUserDefinedDrug_15;
                                        SqlSelect = SqlSelect.Replace("@MedicationName", dr["Medication_Name"].ToString());

                                        SqlCommand = new SqlCommand();
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                        DataTable SqlDt = new DataTable();
                                        SqlDa.Fill(SqlDt);

                                        if (SqlDt != null && SqlDt.Rows.Count > 0)
                                        {
                                            DrugName = SqlDt.Rows[0][0].ToString();
                                            DrugStrength = SqlDt.Rows[0][1].ToString();
                                        }

                                        System.Guid RxDetailsGuid = System.Guid.NewGuid();
                                        SqlCommand = new SqlCommand();
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentRxDetails_Medication_15;
                                        //(@UniqeID,'@DrugName','@Strength',0,NULL,'','',0,'@Notes',0,'','',0,1,'')
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@DrugName", DrugName);
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Strength", DrugStrength);
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@Notes", dr["Medication_Note"].ToString());

                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@UniqeID", RxDetailsGuid);
                                        SqlCommand.ExecuteNonQuery();

                                        SqlCommand = new SqlCommand();
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentRx_Medication_15;
                                        //(@UnqID, @PatientEHRID, @RxDetailsUniqID,0,1, @UserID, GETDATE(), NULL, NULL, @UserID, GETDATE(),0, NULL, NULL)
                                        
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@PatientEHRID", dr["PatientEHRId"].ToString());
                                        SqlCommand.Parameters.AddWithValue("@RxDetailsUniqID", RxDetailsGuid);
                                        SqlCommand.Parameters.AddWithValue("@UserID", Utility.EHR_UserLogin_ID);
                                        SqlCommand.ExecuteNonQuery();

                                        string ehrMedicationPatientId = "";
                                        SqlSelect = "select ID from rx where RxDetailsID = '"+ RxDetailsGuid + "'";
                                        SqlCommand = new SqlCommand();
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                        SqlDt = new DataTable();
                                        SqlDa.Fill(SqlDt);

                                        if (SqlDt != null && SqlDt.Rows.Count > 0)
                                        {
                                            ehrMedicationPatientId = SqlDt.Rows[0][0].ToString();
                                        }

                                        //select ID from rx where RxDetailsID = '0d1e5062-fd84-40f3-83ab-f14be32265ec'

                                        MedicationPatientId = ehrMedicationPatientId;
                                    }
                                    else
                                    {
                                        System.Guid Guid = System.Guid.NewGuid();
                                        string providerid = "";
                                        System.Guid ProviderGuid;
                                        SqlCommand = new SqlCommand();
                                        SqlCommand.CommandTimeout = 200;
                                        string SqlSelect = SynchAbelDentQRY.GetAbelDentClinicalProviderUniqID;
                                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                        DataTable SqlDt = new DataTable();
                                        SqlDa.Fill(SqlDt);

                                        if (SqlDt != null && SqlDt.Rows.Count > 0)
                                        {
                                            providerid = SqlDt.Rows[0][0].ToString();
                                        }
                                        ProviderGuid = Guid.Parse(providerid);

                                        string ClinicalConcept = "";
                                        System.Guid ConceptGuid = System.Guid.NewGuid();

                                        SqlCommand = new SqlCommand();
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.GetMedicationConceptGuid_Medication_1;
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@MedicationNameINCAP", dr["Medication_Name"].ToString().ToUpper());
                                        ClinicalConcept = Convert.ToString(SqlCommand.ExecuteScalar());
                                        ConceptGuid = Guid.Parse(ClinicalConcept);

                                        System.Guid ActGuID = System.Guid.NewGuid();
                                        SqlCommand = new SqlCommand();
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalAct_Medication_2;

                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@DateTime", CurDate);
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@EffTimeEnd", CurDateAdd);

                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@GuidId", ActGuID);
                                        SqlCommand.Parameters.AddWithValue("@ConceptGuID", ConceptGuid);
                                        SqlCommand.Parameters.AddWithValue("@PatientNote", dr["Medication_Note"]);
                                        SqlCommand.Parameters.AddWithValue("@ProviderUniqID", ProviderGuid);
                                        SqlCommand.ExecuteNonQuery();

                                        MedicationPatientId = ActGuID.ToString();

                                        string patientid = "";
                                        System.Guid PatientGuid;
                                        SqlCommand = new SqlCommand();
                                        SqlCommand.CommandTimeout = 200;
                                        SqlSelect = SynchAbelDentQRY.GetPatientUniqIDfor_Medication_3;
                                        SqlSelect = SqlSelect.Replace("@PatientId", LegacyPID);
                                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                        SqlDt = new DataTable();
                                        SqlDa.Fill(SqlDt);

                                        if (SqlDt != null && SqlDt.Rows.Count > 0)
                                        {
                                            patientid = SqlDt.Rows[0][0].ToString();
                                        }
                                        PatientGuid = Guid.Parse(patientid);

                                        SqlCommand = new SqlCommand();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalParticipation_Medication_4;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                        SqlCommand.Parameters.AddWithValue("@PatientGuid", PatientGuid);
                                        SqlCommand.ExecuteNonQuery();

                                        SqlCommand = new SqlCommand();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbeldentClinicalParticipation_Medication_4_1;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                        SqlCommand.Parameters.AddWithValue("@ConceptGuID", ConceptGuid);
                                        SqlCommand.ExecuteNonQuery();

                                        SqlCommand = new SqlCommand();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbeldentClinicalParticipation_Medication_4_2;
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                        SqlCommand.Parameters.AddWithValue("@ProviderUniqID", ProviderGuid);
                                        SqlCommand.ExecuteNonQuery();

                                        SqlCommand = new SqlCommand();
                                        SqlCommand.CommandTimeout = 200;
                                        SqlSelect = SynchAbelDentQRY.SelectAbelDentUserDefinedDrugData_Medication_5;
                                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@Medication_Name", dr["Medication_Name"].ToString());
                                        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                        SqlDt = new DataTable();
                                        SqlDa.Fill(SqlDt);
                                        if (SqlDt != null && SqlDt.Rows.Count > 0)
                                        {
                                            DoseQuantityValue = SqlDt.Rows[0][1].ToString();
                                            DoseQuantityUnits = SqlDt.Rows[0][2].ToString();
                                        }

                                        System.Guid ParticipatGuID = System.Guid.NewGuid();
                                        SqlCommand = new SqlCommand();
                                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.CommandText = SynchAbelDentQRY.InsertAbelDentClinicalSubstanceAdministration_Medication_6;
                                        SqlCommand.CommandText = SqlCommand.CommandText.Replace("@DateTime", CurDate);
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("@ActID", ActGuID);
                                        SqlCommand.Parameters.AddWithValue("@DoseQunValue", Convert.ToInt64(DoseQuantityValue));
                                        SqlCommand.Parameters.AddWithValue("@DoseQunUnits", DoseQuantityUnits);
                                        //SqlCommand.Parameters.AddWithValue("@DateTime", CurDate);
                                        SqlCommand.ExecuteNonQuery();
                                    }
                                }
                                else
                                {

                                }
                                SynchLocalDAL.UpdateMedicationEHR_Updateflg(dr["MedicationResponse_Local_ID"].ToString(), MedicationPatientId.ToString(), dr["PatientEHRID"].ToString(), dr["Medication_EHR_Id"].ToString(), "1");
                            }
                        }
                        IsSaveMedication = true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Save Patient Medication to AbelDent Error: " + ex.Message);
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        #endregion

        #region User
        public static DataTable GetAbelDentUser()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchAbelDentQRY.GetAbelDentUser;
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

        public static string GetAbelDentUserLoginId()
        {
            string UserLoginID = string.Empty;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            try
            {
                System.Guid guid = System.Guid.NewGuid();

                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                if (Utility.Application_Version.ToLower() == "14.8.2".ToLower())
                {
                    string SqlSelect = SynchAbelDentQRY.GetAbelDentUserLoginId;

                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@UniqID", guid.ToString());
                    guid = System.Guid.NewGuid();
                    SqlCommand.Parameters.AddWithValue("@AssociateID", guid.ToString());
                    guid = System.Guid.NewGuid();
                    SqlCommand.Parameters.AddWithValue("@SaltID", guid.ToString());
                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                    DataTable SqlDt = new DataTable();
                    SqlDa.Fill(SqlDt);
                    if (SqlDt != null && SqlDt.Rows.Count > 0)
                    {
                        UserLoginID = SqlDt.Rows[0][0].ToString();
                    }
                }
                else if (Utility.Application_Version.ToLower() == "12.10.6".ToLower())
                {
                    string SqlSelect = SynchAbelDentQRY.GetAbelDentUserLoginId_12_10_6;

                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@UniqID", guid.ToString());
                    guid = System.Guid.NewGuid();
                    SqlCommand.Parameters.AddWithValue("@AssociateID", guid.ToString());
                    guid = System.Guid.NewGuid();
                    SqlCommand.Parameters.AddWithValue("@SaltID", guid.ToString());
                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                    DataTable SqlDt = new DataTable();
                    SqlDa.Fill(SqlDt);
                    if (SqlDt != null && SqlDt.Rows.Count > 0)
                    {
                        UserLoginID = SqlDt.Rows[0][0].ToString();
                    }
                }
                else
                {
                    string SqlSelect = SynchAbelDentQRY.GetAbelDentUserLoginId_14_4_2;
                    CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@UniqID", guid.ToString());
                    guid = System.Guid.NewGuid();
                    SqlCommand.Parameters.AddWithValue("@AssociateID", guid.ToString());
                    guid = System.Guid.NewGuid();
                    SqlCommand.Parameters.AddWithValue("@SaltID", guid.ToString());
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

        public static bool Save_Users_AbelDent_To_Local(DataTable dtTrackerUser)
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
        #endregion

        public static string Save_PatientPaymentLog_LocalToAbelDent(DataRow drRow, string DbString, string ServiceInstallationId,string _filename_EHR_Payment = "",string _EHRLogdirectory_EHR_Payment = "")
        {
            
            //if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            //{
            Utility.EHR_UserLogin_ID = GetAbelDentUserLoginId();
            //}

            string noteId = "";
            SqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            SqlCommand SqlCommand = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                string CategoryID = "51CF254F-43C0-4F53-977B-A5CEB583DCBB";
                try
                {
                    System.Guid guid = System.Guid.NewGuid();

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchAbelDentQRY.InsertPatientPaymentLog;
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientNote", drRow["template"].ToString());
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@PatNum", drRow["PatientEHRId"]);
                    SqlCommand.Parameters.AddWithValue("@GuidId", guid.ToString());
                    SqlCommand.Parameters.AddWithValue("@Importance", 'N');
                    SqlCommand.Parameters.AddWithValue("@CategoryID", CategoryID);                    
                    SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                    SqlCommand.Parameters.AddWithValue("@User", Utility.EHR_UserLogin_ID);
                    SqlCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Payment Log Local To AbelDent with PatientEHRId=" + drRow["PatientEHRId"] + " and CategoryID="+  CategoryID + ",User="+ Utility.EHR_UserLogin_ID);
                    SqlCommand.CommandText = SynchAbelDentQRY.GetInsertPatientPaymentLog_Id;
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientNote", drRow["template"].ToString());
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@PatNum", drRow["PatientEHRId"]);
                    SqlCommand.Parameters.AddWithValue("@CategoryID", CategoryID);                    
                    noteId = SqlCommand.ExecuteScalar().ToString();
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get Patient PaymentLog Id for PatientEHRId=" + drRow["PatientEHRId"] + ",CategoryID=" + CategoryID);
                   
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", ServiceInstallationId.Trim(), drRow["Clinic_Number"].ToString().Trim(), "", noteId, "", "", "",Convert.ToInt32(drRow["TryInsert"]),_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
                }
                catch (Exception ex1)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", ServiceInstallationId.Trim(), drRow["Clinic_Number"].ToString().Trim(), ex1.Message, noteId, "", "", ex1.Message.ToString(),Convert.ToInt32(drRow["TryInsert"]),_filename_EHR_Payment,_EHRLogdirectory_EHR_Payment);
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

        public static string Save_PatientSMSCallLog_LocalToAbelDent(DataTable dtWebPatientPayment, string DbString, string ServiceInstallationId)
        {            
            //if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            //{
            Utility.EHR_UserLogin_ID = GetAbelDentUserLoginId();
            //}

            string noteId = "";
            SqlConnection conn = null;
            
            //MySqlCommand MySqlCommand = new MySqlCommand();
            SqlCommand SqlCommand = null;
            CommonDB.AbelDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                System.Guid CategoryGuid;
                string CategoryID = "672A32AC-4083-4EEE-BC79-7A4C18E4DBB2";
                CategoryGuid = Guid.Parse(CategoryID);
                if (!dtWebPatientPayment.Columns.Contains("Log_Status"))
                {
                    dtWebPatientPayment.Columns.Add("Log_Status", typeof(string));
                }


                DataTable dtResultCopy = new DataTable();
                //Utility.WriteToErrorLogFromAll("Call to save records to EHR_DLL");
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    dtResultCopy = dtWebPatientPayment.Select("Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' and Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();

                    foreach (DataRow drRow in dtResultCopy.Rows)
                    {
                        if (drRow["PatientEHRId"] != null && drRow["PatientEHRId"].ToString() != string.Empty && drRow["PatientEHRId"].ToString() != "")
                        {
                            try
                            {
                                System.Guid guid = System.Guid.NewGuid();
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                if (Utility.Application_Version == "12.10.6")
                                {
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertPatientSMSCallLog_12_10_6;
                                }
                                else
                                {
                                    SqlCommand.CommandText = SynchAbelDentQRY.InsertPatientSMSCallLog;
                                }

                                if (drRow["text"].ToString().Contains("'"))
                                    drRow["text"] = drRow["text"].ToString().Replace("'", "''");

                                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@PatientNote", drRow["text"].ToString());


                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatNum", drRow["PatientEHRId"]);
                                SqlCommand.Parameters.AddWithValue("@GuidId", guid);                                                                
                                SqlCommand.Parameters.AddWithValue("@CategoryID", CategoryGuid);                                                               

                                SqlCommand.ExecuteScalar();
                                noteId = guid.ToString();
                                //Utility.WriteToSyncLogFile_All("Query Executed");
                                drRow["LogEHRId"] = noteId;
                                drRow["Log_Status"] = "completed";

                                Utility.WriteToErrorLogFromAll("Payment Local to EHR = 5 QUERY DONE");
                            }
                            catch (Exception ex1)
                            {
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
                    if (dtResultCopy.Rows.Count > 0)
                    {
                        try
                        {
                            if (dtResultCopy.Select("LogType = 0").Count() > 0)
                            {
                                SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy.Select("LogType = 0").CopyToDataTable(), "completed", "1", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                            }
                            if (dtResultCopy.Select("LogType = 1").Count() > 0)
                            {
                                SynchLocalDAL.CallPatientFollowUpStatusCompleted(dtResultCopy.Select("LogType = 1").CopyToDataTable(), "completed", ServiceInstallationId.Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                            }
                        }
                        catch (Exception ex2)
                        {
                            Utility.WriteToErrorLogFromAll("ApptType zero records exists " + ex2.ToString());
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

        #region Event Listener
        public static bool Save_Patient_AbelDent_To_Local_New(DataTable dtSaveRecords, string Clinic_Number, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            try
            {
                SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtSaveRecords, "0", "1");
            }
            catch (Exception)
            {

            }
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string sqlSelect = string.Empty;
                    string Status = string.Empty;
                    string tmpBirthDate = string.Empty;
                    string tmpReceive_Sms_Email = string.Empty;
                    string Gender = "Unknown";
                    string MaritalStatus = "Single";

                    using (SqlCeCommand SqlCeComBulk = new SqlCeCommand("", conn))
                    {
                        SqlCeComBulk.CommandType = CommandType.TableDirect;
                        SqlCeComBulk.CommandText = "Patient";
                        SqlCeComBulk.Connection = conn;
                        SqlCeComBulk.IndexName = "unique_Patient_EHR_ID";

                        SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                        foreach (DataRow dr in dtSaveRecords.Rows)
                        {
                            try
                            {
                                tmpReceive_Sms_Email = "Y";

                                //tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());
                                tmpBirthDate = dr["birth_date"].ToString().Trim();

                                if (tmpBirthDate != "")
                                {
                                    tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
                                    if (tmpBirthDate == "01/01/1753")
                                        tmpBirthDate = "01/01/0001";
                                }

                                try
                                {
                                    if (dr["Primary_Insurance"].ToString().Trim() == "0")
                                        dr["Primary_Insurance"] = "";
                                    if (dr["Guar_ID"].ToString().Trim() == "0")
                                        dr["Guar_ID"] = "";
                                }
                                catch (Exception ex)
                                {

                                }

                                try
                                {
                                    Gender = dr["Sex"].ToString().Trim();
                                }
                                catch (Exception ex)
                                { Gender = ""; }

                                try
                                {
                                    MaritalStatus = dr["MaritalStatus"].ToString().Trim();

                                }
                                catch (Exception ex)
                                { MaritalStatus = ""; }


                                try
                                {
                                    Status = dr["Status"].ToString().Trim();

                                }
                                catch (Exception ex)
                                { Status = ""; }

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
                                            rec.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                                            rec.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rec.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rec.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            rec.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
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
                                            rec.SetValue(rs.GetOrdinal("is_deleted"), isDeleted);
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
                                            rec.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), dr["responsiblepartybirthdate"].ToString().Trim());
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
                                            rs.SetValue(rs.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dr["EHR_Entry_DateTime"].ToString().Trim()));
                                            rs.SetValue(rs.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            rs.SetValue(rs.GetOrdinal("Clinic_Number"), dr["Clinic_Number"].ToString().Trim());
                                            rs.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
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
                                            rs.SetValue(rs.GetOrdinal("is_deleted"), Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3 ? true : isDeleted);
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
                                            rs.SetValue(rs.GetOrdinal("responsiblepartybirthdate"), dr["responsiblepartybirthdate"].ToString().Trim());
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
        #endregion
    }

    public class AbelDentQuestionIds
    {
        public string AbelDent_Form_EHRUnique_ID { get; set; }
        public string AbelDent_Question_EHRUnique_ID { get; set; }
        public string AbelDent_Question_EHR_ID { get; set; }
        public string AbelDent_QuestionsTypeId { get; set; }
        public string AbelDent_ResponsetypeId { get; set; }
    }

    public class StartTimes
    {
        public DateTime Time { get; set; }

        public DateTime OATime => DateTime.FromOADate(0.0).Add(Time.TimeOfDay);

        public ObservableCollection<StartTimes> StartTimeRecords { get; set; }
    }

    public class AbelOpenings
    {
        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public DateTime OATime => DateTime.FromOADate(0.0).Add(this.Time.TimeOfDay);

        public string Provider { get; set; }

        public string ColumnID { get; set; }

        public int Units { get; set; }

        public string AvailableTime { get; set; }

        public ObservableCollection<StartTimes> StartTimeList { get; set; }
    }

    public class AbelProviderColumnAptMapList : List<AbelProviderColumnAptMap>
    {
        public AbelProviderColumnAptMapList(List<string> columnIDs, int maxUnitsForDay)
        {
            SchedSetupProvidersList source = new SchedSetupProvidersList();
            foreach (string clm in columnIDs)
            {
                if (!string.IsNullOrEmpty(clm))
                {
                    SchedSetupProvider schedSetupProvider = source.FirstOrDefault((SchedSetupProvider x) => x.Column == clm);
                    if (schedSetupProvider != null)
                    {
                        Add(new AbelProviderColumnAptMap
                        {
                            ProviderID = schedSetupProvider.ProviderID,
                            ColumnID = clm,
                            AptMap = new int[maxUnitsForDay]
                        });
                    }
                }
            }
        }
    }

    public class SchedSetupProvidersList : List<SchedSetupProvider>
    {
        public string All => "";

        public string Hygienist => "";

        public SchedSetupProvidersList(bool addAllToList = true)
        {

            DataTable dtProviderData = SynchAbelDentDAL.GetColumnProviderData();
            List<string> activeColumnsList = SynchAbelDentDAL.GetAbelDentActiveColumnData();

            try
            {
                foreach (DataRow itemList in dtProviderData.Rows)
                {
                    SchedSetupProvider pv = new SchedSetupProvider();
                    pv.Column = itemList["Operatory_EHR_ID"].ToString().Trim();
                    pv.ProviderID = itemList["Provider_EHR_ID"].ToString().Trim();
                    if (!string.IsNullOrEmpty(activeColumnsList.FirstOrDefault((string x) => x.Equals(pv.Column))))
                    {
                        Add(pv);
                    }
                }

                Add(new SchedSetupProvider
                {
                    ProviderID = All,
                    ProviderName = Hygienist
                });
            }
            catch (Exception ex)
            {
                //AppLog.LogException(ex);
            }
            finally
            {

            }
        }
    }

    public class SchedSetupProvider
    {
        public string _providerID;

        public string Column { get; set; }

        public string ProviderID
        {
            get
            {
                return _providerID;
            }
            set
            {
                _providerID = value;
                ProviderName = SynchAbelDentDAL.GetAbelDentProviderName(_providerID);
            }
        }

        public string ProviderName { get; set; }

        public string ProviderIDName
        {
            get
            {
                if (!string.IsNullOrEmpty(_providerID) && !string.IsNullOrEmpty(ProviderName))
                {
                    return $"{_providerID} - {ProviderName}";
                }
                return "";
            }
        }


    }
}

