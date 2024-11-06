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
using RestSharp;
using System.Net;
using Pozative.BO;
using System.Collections;
using System.Data.Odbc;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace Pozative.DAL
{
    public class SynchClearDentDAL
    {

        public static bool GetClearDentConnection()
        {

            SqlConnection conn = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            bool IsConnected = false;
            try
            {
                conn.Open();
                IsConnected = true;
            }
            catch (Exception)
            {
                IsConnected = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return IsConnected;
        }

        #region Treatment Document
        public static bool Save_Treatment_Document_in_ClearDent(string strTreatmentPlanID = "")
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
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string destPatientDocPath = "";
                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\Program Files (x86)\Prococious Technology Inc\ClearDent\AttachedFile\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        }
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
                        // string RenameFileName = dr["TreatmentDoc_Name"].ToString();


                        string sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");

                        // I have to change only last field cat number 
                        string QryDoc = SynchClearDentQRY.InsertPatientTreatmentDocData;
                        string showindFileName = dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim();
                        SqlCommand.CommandText = QryDoc;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("fld_intPatId", dr["Patient_EHR_ID"].ToString().Trim());
                        SqlCommand.Parameters.AddWithValue("fld_strOFileName", showindFileName);
                        SqlCommand.Parameters.AddWithValue("fld_strDBFileName", tmpFileOrgName.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_dtmCreate", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_shtCreateUser", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("fld_dtmLastMod", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_shtLastModUser", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("fld_shtFileCatId", "16");

                        SqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from tbl_File";
                        SqlCommand.CommandText = QryIdentity;
                        SqlCommand.CommandType = CommandType.Text;
                        DocId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + tmpFileOrgName);

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
        public static bool Save_InsuranceCarrier_Document_in_ClearDent(string strInsuranceCarrierID = "")
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
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        string destPatientDocPath = "";
                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\Program Files (x86)\Prococious Technology Inc\ClearDent\AttachedFile\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        }
                        else
                        {
                            destPatientDocPath = Utility.EHRDocPath + "\\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
                        }
                        string sourceLocation = CommonUtility.GetAditInsuranceCarrierDocTempPath() + "\\" + dr["InsuranceCarrier_Doc_Name"].ToString();
                        if (!System.IO.File.Exists(sourceLocation))
                        {
                            PullLiveDatabaseDAL.Update_TreatrmentDocNotFound_Live_To_Local(dr["InsuranceCarrier_Doc_Web_ID"].ToString());
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
                        // string RenameFileName = dr["InsuranceCarrierDoc_Name"].ToString();


                        string sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");

                        // I have to change only last field cat number 
                        string QryDoc = SynchClearDentQRY.InsertPatientInsuranceCarrierDocData;
                        string showindFileName = dr["InsuranceCarrier_Doc_Name"].ToString().Trim();
                        SqlCommand.CommandText = QryDoc;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("fld_intPatId", dr["Patient_EHR_ID"].ToString().Trim());
                        SqlCommand.Parameters.AddWithValue("fld_strOFileName", showindFileName);
                        SqlCommand.Parameters.AddWithValue("fld_strDBFileName", tmpFileOrgName.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_dtmCreate", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_shtCreateUser", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("fld_dtmLastMod", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_shtLastModUser", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("fld_shtFileCatId", dr["InsuranceCarrier_FolderName"].ToString().Trim());

                        SqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from tbl_File";
                        SqlCommand.CommandText = QryIdentity;
                        SqlCommand.CommandType = CommandType.Text;
                        DocId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + tmpFileOrgName);

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

        #region EHR_VersionNumber


        public static string GetClearDentEHR_VersionNumber()
        {
            string version = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetEHRActualVersionCleardent;
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
                throw ex;
                version = "";
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return version;
        }



        #endregion

        #region Appointment

        public static DataTable GetClearDentAppointmentData(string strApptID = "")
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentAppointmentData;
                if (!string.IsNullOrEmpty(strApptID))
                {
                    SqlSelect = SqlSelect + " And  [fld_auto_intAppId] = '" + strApptID + "'";
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Clear();
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


        public static DataTable GetClearDentAppointmentIds()
        {

            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentAppointmentEhrIds;
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

        public static DataTable GetClearDentAppointment_Procedures_Data(string strApptID = "")
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentAppointment_Procedures_Data;
                if (!string.IsNullOrEmpty(strApptID))
                {
                    SqlSelect = SynchClearDentQRY.GetClearDentAppointment_Procedures_DataByAptID;
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
                    }
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static bool Save_Appointment_ClearDent_To_Local(DataTable dtClearDentAppointment)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        bool is_ehr_updated = false;
                        string Apptdate = string.Empty;
                        string ApptTime = string.Empty;
                        string Mobile_Contact = string.Empty;
                        string Email = string.Empty;
                        string Home_Contact = string.Empty;
                        string Address = string.Empty;
                        string City = string.Empty;
                        string State = string.Empty;
                        string Zipcode = string.Empty;
                        string patient_first_name = string.Empty;
                        string patient_last_name = string.Empty;
                        string patient_mi_name = string.Empty;
                        string AppointmentStatus = string.Empty;

                        foreach (DataRow dr in dtClearDentAppointment.Rows)

                        {
                            is_ehr_updated = false;

                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }

                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {

                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;
                                        is_ehr_updated = true;
                                        break;
                                    case 5:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;
                                        is_ehr_updated = true;
                                        break;
                                    case 4:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;
                                        is_ehr_updated = true;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                        break;
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    Mobile_Contact = string.Empty;
                                    Email = string.Empty;
                                    Home_Contact = string.Empty;
                                    Address = string.Empty;
                                    City = string.Empty;
                                    State = string.Empty;
                                    Zipcode = string.Empty;

                                    Mobile_Contact = dr["Mobile_Contact"].ToString();
                                    Email = dr["Email"].ToString();
                                    Home_Contact = dr["Home_Contact"].ToString().Trim();
                                    Address = dr["Address1"].ToString().Trim();
                                    City = dr["City"].ToString().Trim();
                                    State = dr["State"].ToString().Trim();
                                    Zipcode = dr["Zipcode"].ToString().Trim();

                                    string birthdate = string.Empty;
                                    if (!string.IsNullOrEmpty(dr["birth_date"].ToString()))
                                    {
                                        birthdate = dr["birth_date"].ToString();
                                    }

                                    if (dr["appointment_status_ehr_key"].ToString().Trim() == "7")
                                    {
                                        AppointmentStatus = "Completed";
                                    }
                                    else if (dr["appointment_status_ehr_key"].ToString().Trim() == "1")
                                    {
                                        AppointmentStatus = "Booked";
                                    }
                                    else
                                    {
                                        AppointmentStatus = dr["Appointment_Status"].ToString().Trim();
                                    }
                                    int commentlen = 1999;
                                    if (dr["comment"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["comment"].ToString().Trim().Length;
                                    }
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", string.Empty);
                                    SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("MI", dr["Middle_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(Home_Contact.ToString().Trim()));
                                    SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(Mobile_Contact.Trim()));
                                    SqlCeCommand.Parameters.AddWithValue("Email", Email.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Address", Address.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("City", City.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ST", State.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Zip", Zipcode.ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["ProviderName"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType"].ToString().Trim().Replace(",", " "));
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("birth_date", birthdate);
                                    SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", Convert.ToDateTime(dr["StartTime"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", Convert.ToDateTime(dr["EndTime"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("Status", "1");
                                    SqlCeCommand.Parameters.AddWithValue("Patient_Status", "new");
                                    SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
                                    SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", "");
                                    SqlCeCommand.Parameters.AddWithValue("confirmed_status", "");
                                    SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
                                    SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
                                    SqlCeCommand.Parameters.AddWithValue("Is_Appt", "EHR");
                                    SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Convert.ToDateTime(dr["EHR_Entry_DateTime"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_asap", Convert.ToInt16(dr["is_asap"]) == 0 ? false : true);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                                    SqlCeCommand.Parameters.AddWithValue("ProcedureDesc", dr["ProcedureDesc"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ProcedureCode", dr["ProcedureCode"].ToString().Trim());
                                    try
                                    {
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    catch (Exception ex1)
                                    {
                                        Utility.WriteToErrorLogFromAll("Error_Appointment Sync Cleardent to Local Database Appt Id = " + dr["appointment_id"].ToString() + " Err - " + ex1.Message.ToString());
                                    }

                                }
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

        #region DeletedAppointment

        public static DataTable GetClearDentDeletedAppointmentData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentDeletedAppointmentData;
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

        public static bool Update_DeletedAppointment_ClearDent_To_Local(DataTable dtClearDentDeletedAppointment)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        string AppointmentStatus = string.Empty;
                        foreach (DataRow dr in dtClearDentDeletedAppointment.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }

                            if (dr["InsUptDlt"].ToString() == "1")
                            {
                                SqlCeCommand.CommandText = SynchLocalQRY.InsertAppointment_With_DeleteFlag;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                try
                                {
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex2)
                                {
                                    Utility.WriteToErrorLogFromAll("Error IN Insert Appt  Appt Id = " + dr["Appt_EHR_ID"].ToString().Trim() + " Err " + ex2.Message.ToString());
                                    SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                    Utility.WriteToSyncLogFile_All("Updated Local appointment with flag is_deleted for appt = " + dr["Appt_EHR_ID"].ToString().Trim() + " Err " + ex2.Message.ToString());
                                }

                            }
                            else if (dr["InsUptDlt"].ToString() == "2")
                            {
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }

                        }
                    }
                    //  SqlCetx.Commit();
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

        #endregion

        #region OperatoryEvent

        public static DataTable GetClearDentOperatoryEventData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string SqlSelect = SynchClearDentQRY.GetClearDentOperatoryEventData;
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

        public static bool Save_OperatoryEvent_ClearDent_To_Local(DataTable dtClearDentOperatoryEvent)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //  SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        bool is_ehr_updated = false;

                        foreach (DataRow dr in dtClearDentOperatoryEvent.Rows)
                        {
                            is_ehr_updated = false;

                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }

                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {

                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData;
                                        is_ehr_updated = false;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryEventData;
                                        is_ehr_updated = true;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
                                        break;
                                }

                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    DateTime OE_Date = Convert.ToDateTime(dr["StartTime"].ToString());
                                    int commentlen = 1999;
                                    if (dr["comment"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dr["comment"].ToString().Trim().Length;
                                    }
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", string.Empty);
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                    SqlCeCommand.Parameters.AddWithValue("StartTime", Convert.ToDateTime(dr["StartTime"].ToString()));
                                    if (dr["EndTime"].ToString() == "")
                                    {
                                        SqlCeCommand.Parameters.AddWithValue("EndTime", DBNull.Value);
                                        Utility.WriteToErrorLogFromAll("EndTime is null entered in operatory event record.");
                                    }
                                    else
                                    {
                                        SqlCeCommand.Parameters.AddWithValue("EndTime", Convert.ToDateTime(dr["EndTime"].ToString()));
                                    }
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Convert.ToDateTime(OE_Date));
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                    SqlCeCommand.Parameters.AddWithValue("Allow_Book_Appt", Convert.ToBoolean(false));
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    //  SqlCetx.Commit();
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

        #region DeletedOperatoryEvent

        public static DataTable GetLocalOperatoryEventData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                DateTime ToDate = Utility.LastSyncDateAditServer;

                try
                {
                    string SqlCeSelect = SynchClearDentQRY.GetLocalOperatoryEventData;
                    SqlCeSelect = SqlCeSelect.Replace("?", "@ToDate");
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    CommonDB.SqlServerDataAdapter(SqlCeCommand, ref SqlCeDa);
                    DataTable SqlCeDt = new DataTable();
                    SqlCeDa.Fill(SqlCeDt);

                    return SqlCeDt;
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

            #region SqlCeConnection
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    DateTime ToDate = Utility.LastSyncDateAditServer;
                    try
                    {
                        string SqlCeSelect = SynchClearDentQRY.GetLocalOperatoryEventData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
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
            }
            #endregion
        }

        public static DataTable GetClearDentDeletedOperatoryEventData()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentDeletedOperatoryEventData;
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
        public static bool Update_DeletedOperatoryEvent_ClearDent_To_Local(DataTable dtClearDentDeletedOperatoryEvent)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //  SqlCetx = conn.BeginTransaction();
                try
                {
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        string AppointmentStatus = string.Empty;
                        foreach (DataRow dr in dtClearDentDeletedOperatoryEvent.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }

                            if (dr["InsUptDlt"].ToString() == "1")
                            {
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData_With_DeleteFlag;
                                SqlCeCommand.Parameters.Clear();
                                DateTime OE_Date = Convert.ToDateTime(dr["StartTime"].ToString());
                                int commentlen = 1999;
                                if (dr["comment"].ToString().Trim().Length < commentlen)
                                {
                                    commentlen = dr["comment"].ToString().Trim().Length;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", string.Empty);
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                                SqlCeCommand.Parameters.AddWithValue("StartTime", Convert.ToDateTime(dr["StartTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("EndTime", Convert.ToDateTime(dr["EndTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Convert.ToDateTime(OE_Date));
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                            else if (dr["InsUptDlt"].ToString() == "2")
                            {
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["OE_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }

                        }
                    }
                    //  SqlCetx.Commit();
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


        #endregion

        #endregion

        #region Provider

        public static DataTable GetClearDentProviderData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentProviderData;
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

        public static bool Save_Provider_ClearDent_To_Local(DataTable dtClearDentProvider)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //  SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = string.Empty;
                    string Provider_Speciality = "";
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtClearDentProvider.Rows)
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
                                try
                                {
                                    Provider_Speciality = dr["provider_speciality"].ToString();
                                    if (Provider_Speciality.Length > 10)
                                    {
                                        Provider_Speciality = dr["provider_speciality"].ToString().Trim().Substring(12, dr["provider_speciality"].ToString().Trim().Length - 12);
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
                                SqlCeCommand.Parameters.AddWithValue("First_Name", dr["Provider_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("MI", "");
                                SqlCeCommand.Parameters.AddWithValue("gender", "");
                                SqlCeCommand.Parameters.AddWithValue("provider_speciality", Provider_Speciality);
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("is_active", Convert.ToInt16(dr["is_active"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //   SqlCetx.Commit();
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

        #region ProviderOfficeHours

        public static DataTable GetClearDentProviderOfficeHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = new SqlDataAdapter();
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DataTable dtProviderOfficeHours = new DataTable();
            DataTable dtProvider = new DataTable();
            DataTable OdbcDt = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentProviderOfficeHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //OdbcCommand.Parameters.AddWithValue("provider_Id", drRow["Provider_EHR_Id"].ToString());
                DataTable SqlDt = new DataTable();
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                SqlDa.Fill(SqlDt);

                return CreateTableOfProviderOfficeHours(SqlDt);

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

        private static string GetWeekDays(int weekDayIndex)
        {
            string returnweekdaysName = "";
            try
            {
                switch (weekDayIndex)
                {
                    case 1:
                        returnweekdaysName = "Monday";
                        break;
                    case 2:
                        returnweekdaysName = "Tuesday";
                        break;
                    case 3:
                        returnweekdaysName = "Wednesday";
                        break;
                    case 4:
                        returnweekdaysName = "Thursday";
                        break;
                    case 5:
                        returnweekdaysName = "Friday";
                        break;
                    case 6:
                        returnweekdaysName = "Saturday";
                        break;
                    case 7:
                        returnweekdaysName = "Sunday";
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnweekdaysName;
        }

        public static DataTable GetClearDentNonWorkingHours()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentOfficeNonWorkingHours;
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

        public static DataTable GetClearDentOfficeWorkingHours()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentOfficeWorkingHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);

                DataSet theDataSet = new DataSet();
                DataTable dtResultOfficeWorkingHours = new DataTable();

                CreateTableDTOfficeHours(ref dtResultOfficeWorkingHours);

                for (int i = 0; i < SqlDt.Rows.Count; i++)
                {
                    StringReader theReader = new StringReader(SqlDt.Rows[i]["fld_strRecurrence"].ToString());
                    theDataSet = new DataSet();
                    if (theDataSet.Tables.Count > 0 && theDataSet.Tables[0] != null)
                    {
                        theDataSet.Tables[0].Rows.Clear();
                        theDataSet.Tables.RemoveAt(0);
                    }

                    theDataSet.ReadXml(theReader);

                    if ((theDataSet.Tables[0].Columns.Contains("RecurrenceType") && theDataSet.Tables[0].Rows[0]["RecurrenceType"].ToString().ToUpper() == "WEEKLY") || theDataSet.Tables[0].Columns.Contains("DayOfWeek"))
                    {
                        DateTime PatternStartDate = DateTime.Now;
                        DateTime PatternEndDate = DateTime.Now;

                        SetPatternStartEndDateProvider(theDataSet.Tables[0], ref PatternStartDate, ref PatternEndDate);

                        if (PatternStartDate < PatternEndDate)
                        {
                            string[] array = new string[] { };

                            if (theDataSet.Tables[0].Columns.Contains("DayOfWeek"))
                                array = theDataSet.Tables[0].Rows[0]["DayOfWeek"].ToString().Split(',');
                            else
                                array = "Monday".Split(',');

                            array.AsEnumerable()
                            .All(o =>
                            {
                                DataRow Dr = dtResultOfficeWorkingHours.NewRow();

                                Dr["SchID"] = SqlDt.Rows[i]["fld_auto_intSchPrId"].ToString();
                                Dr["WeekDay"] = o.ToString().Trim();
                                Dr["StartTime"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(theDataSet.Tables[0].Rows[0]["StartTime"]).ToString("HH:mm"));
                                Dr["EndTime"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(theDataSet.Tables[0].Rows[0]["EndTime"]).ToString("HH:mm"));

                                dtResultOfficeWorkingHours.Rows.Add(Dr);
                                return true;
                            });
                        }
                    }
                }
                return dtResultOfficeWorkingHours;
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

        private static DataTable CreateTableOfProviderOfficeHours(DataTable dtProviderOfficeHours)
        {
            DataTable dtResultProviderOfficeHours = new DataTable();
            DataTable dtResultOfficeNonWorkingHours = new DataTable();
            DataTable dtResultOfficeWorkingHours = new DataTable();

            DataSet theDataSet = new DataSet();
            DataTable dtCustomeHours = new DataTable();

            try
            {
                #region GetDistinct Provider & Get ProviderHour Structure
                DataTable dtProvider = GetClearDentProviderData();
                dtResultProviderOfficeHours = SynchLocalDAL.GetLocalProviderOfficeHoursBlankStructure();

                dtResultOfficeNonWorkingHours = GetClearDentNonWorkingHours();
                dtResultOfficeWorkingHours = GetClearDentOfficeWorkingHours();

                #endregion

                #region Create PRovider & Daywise Datatable
                dtProvider.AsEnumerable()
                    .All(o =>
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            DataRow drNewRow = dtResultProviderOfficeHours.NewRow();
                            drNewRow["POH_EHR_ID"] = o["Provider_EHR_ID"].ToString() + "_" + i.ToString();
                            drNewRow["Provider_EHR_ID"] = o["Provider_EHR_ID"].ToString();
                            drNewRow["WeekDay"] = GetWeekDays(i).ToString();
                            drNewRow["Clinic_Number"] = "0";
                            drNewRow["Service_Install_Id"] = "1";
                            dtResultProviderOfficeHours.Rows.Add(drNewRow);
                        }
                        return true;
                    });
                #endregion

                #region Update Start & EndDatetime in Provider Daywise DataTable
                string daystart1 = "", dayEnd1 = "";

                dtResultProviderOfficeHours.AsEnumerable()
                    .All(o =>
                    {
                        var resultProvider = dtResultOfficeWorkingHours.AsEnumerable().Where(a => a.Field<object>("WeekDay").ToString().ToUpper() == o["WeekDay"].ToString().ToUpper());

                        if (resultProvider.Count() > 0)
                        {
                            o["StartTime1"] = resultProvider.Select(b => b.Field<object>("StartTime")).First();
                            o["StartTime2"] = "01/01/1900 00:00:00";
                            o["StartTime3"] = "01/01/1900 00:00:00";

                            o["EndTime1"] = resultProvider.Select(b => b.Field<object>("EndTime")).First();
                            o["EndTime2"] = "01/01/1900 00:00:00";
                            o["EndTime3"] = "01/01/1900 00:00:00";
                        }
                        else
                        {
                            daystart1 = "fld_dtm" + o["WeekDay"].ToString().Substring(0, 3) + "Start";
                            dayEnd1 = "fld_dtm" + o["WeekDay"].ToString().Substring(0, 3) + "End";

                            resultProvider = dtProviderOfficeHours.AsEnumerable().Where(a => a.Field<object>("fld_auto_shtPrId").ToString().ToUpper() == o["Provider_EHR_ID"].ToString().ToUpper());

                            if (resultProvider.Count() > 0)
                            {
                                if (resultProvider.Select(b => b.Field<object>(daystart1)).First() != null && resultProvider.Select(b => b.Field<object>(daystart1)).First().ToString() != "")
                                {
                                    o["StartTime1"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(resultProvider.Select(b => b.Field<object>(daystart1)).First()).ToString("HH:mm"));
                                }
                                else
                                {
                                    o["StartTime1"] = "01/01/1900 00:00:00";
                                }
                                o["StartTime2"] = "01/01/1900 00:00:00";
                                o["StartTime3"] = "01/01/1900 00:00:00";

                                if (resultProvider.Select(b => b.Field<object>(dayEnd1)).First() != null && resultProvider.Select(b => b.Field<object>(dayEnd1)).First().ToString() != "")
                                {
                                    o["EndTime1"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(resultProvider.Select(b => b.Field<object>(dayEnd1)).First()).ToString("HH:mm"));
                                }
                                else
                                {
                                    o["EndTime1"] = "01/01/1900 00:00:00";
                                }

                                o["EndTime2"] = "01/01/1900 00:00:00";
                                o["EndTime3"] = "01/01/1900 00:00:00";

                            }
                            else
                            {
                                o["StartTime1"] = "01/01/1900 00:00:00";
                                o["EndTime1"] = "01/01/1900 00:00:00";
                                o["StartTime2"] = "01/01/1900 00:00:00";
                                o["EndTime2"] = "01/01/1900 00:00:00";
                                o["StartTime3"] = "01/01/1900 00:00:00";
                                o["EndTime3"] = "01/01/1900 00:00:00";
                            }
                        }
                        return true;
                    });
                #endregion

                #region Non Working Hours Calculate
                dtProvider.AsEnumerable()
                   .All(o =>
                   {
                       var resultProvider = dtResultOfficeNonWorkingHours.AsEnumerable().Where(a => a.Field<object>("fld_shtPrId").ToString().ToUpper() == o["Provider_EHR_ID"].ToString().ToUpper());

                       if (resultProvider.Count() == 0)
                           resultProvider = dtResultOfficeNonWorkingHours.AsEnumerable().Where(a => a.Field<object>("fld_shtPrId").ToString().ToUpper() == "0");

                       if (resultProvider.Count() > 0)
                       {
                           resultProvider.AsEnumerable()
                               .All(p =>
                               {
                                   StringReader theReader = new StringReader(p["fld_strRecurrence"].ToString());
                                   theDataSet = new DataSet();
                                   if (theDataSet.Tables.Count > 0 && theDataSet.Tables[0] != null)
                                   {
                                       try
                                       {
                                           theDataSet.Tables[0].Rows.Clear();
                                           theDataSet.Tables.RemoveAt(0);
                                       }
                                       catch
                                       {
                                           theDataSet.Clear();
                                       }
                                       //for (int i = theDataSet.Tables.Count-1; i >=0 ; i--)
                                       //{
                                       //    if (theDataSet.Tables.Count > 0 && theDataSet.Tables[i] != null)
                                       //    {
                                       //        theDataSet.Tables[i].Rows.Clear();
                                       //        theDataSet.Tables.RemoveAt(i);
                                       //    }
                                       //}                                        
                                   }

                                   theDataSet.ReadXml(theReader);
                                   dtCustomeHours = theDataSet.Tables[0];

                                   if ((dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "WEEKLY") || dtCustomeHours.Columns.Contains("DayOfWeek"))
                                   {
                                       DateTime PatternStartDate = DateTime.Now;
                                       DateTime PatternEndDate = DateTime.Now;

                                       SetPatternStartEndDateProvider(dtCustomeHours, ref PatternStartDate, ref PatternEndDate);

                                       if (PatternStartDate < PatternEndDate)
                                       {
                                           string[] array = new string[] { };

                                           if (dtCustomeHours.Columns.Contains("DayOfWeek"))
                                               array = dtCustomeHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                                           else
                                               array = "Monday".Split(',');

                                           if (array.Length > 0)
                                           {
                                               dtResultProviderOfficeHours.AsEnumerable()
                                                   .All(x =>
                                                   {
                                                       var resultOfficeHour = array.AsEnumerable().Where(b => x.Field<object>("Provider_EHR_ID").ToString().ToUpper() == o["Provider_EHR_ID"].ToString().ToUpper() && x.Field<object>("WeekDay").ToString().ToUpper() == b.ToString().Trim().ToUpper());

                                                       if (resultOfficeHour.Count() > 0)
                                                       {
                                                           if (Convert.ToDateTime(x["StartTime2"]).ToString() == "01/01/1900 12:00:00 AM")
                                                           {
                                                               if (Convert.ToDateTime(Convert.ToDateTime(x["EndTime1"]).ToShortTimeString()) < Convert.ToDateTime(Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToShortTimeString()))
                                                                   x["EndTime2"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToString("HH:mm"));
                                                               else
                                                                   x["EndTime2"] = x["EndTime1"];

                                                               x["EndTime1"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["StartTime"]).ToString("HH:mm"));
                                                               x["StartTime2"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToString("HH:mm"));
                                                           }
                                                           else if (Convert.ToDateTime(x["StartTime3"]).ToString() == "01/01/1900 12:00:00 AM")
                                                           {
                                                               if (Convert.ToDateTime(Convert.ToDateTime(x["EndTime2"]).ToShortTimeString()) < Convert.ToDateTime(Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToShortTimeString()))
                                                                   x["EndTime3"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToString("HH:mm"));
                                                               else
                                                                   x["EndTime3"] = x["EndTime2"];

                                                               x["EndTime2"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["StartTime"]).ToString("HH:mm"));
                                                               x["StartTime3"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToString("HH:mm"));
                                                           }
                                                       }
                                                       return true;
                                                   });
                                           }
                                       }
                                   }
                                   return true;
                               });
                       }
                       return true;
                   });
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
            return dtResultProviderOfficeHours;
        }

        public static void CreateTableDTOfficeHours(ref DataTable dtResult)
        {
            DataColumn dcCol1 = new DataColumn();
            dcCol1.ColumnName = "SchID";
            DataColumn dcCol2 = new DataColumn();
            dcCol2.ColumnName = "WeekDay";
            DataColumn dcCol3 = new DataColumn();
            dcCol3.ColumnName = "StartTime";
            DataColumn dcCol4 = new DataColumn();
            dcCol4.ColumnName = "EndTime";

            dtResult.Columns.Add(dcCol1);
            dtResult.Columns.Add(dcCol2);
            dtResult.Columns.Add(dcCol3);
            dtResult.Columns.Add(dcCol4);
        }
        #endregion

        #region ProviderHours
        public static DataTable GetClearDentProviderHours()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentProviderHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return CreateTableOfProviderHours(SqlDt);

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

        public static DataTable CreateTableOfProviderHours(DataTable dtProviderHours)
        {
            DataSet theDataSet = new DataSet();
            DataTable dtResultProviderHours = new DataTable();
            DataTable dtCustomeHours = new DataTable();
            DataTable dtResult = new DataTable();
            DataTable dtReccurenceExpectionDates = new DataTable();
            DataRow drRow = null;
            try
            {
                if (dtProviderHours.Rows.Count > 0)
                {
                    CreateTableDTProviderHours(ref dtResult);
                    for (int iCount = 0; iCount <= dtProviderHours.Rows.Count - 1; iCount++)
                    {

                        StringReader theReader = new StringReader(dtProviderHours.Rows[iCount]["fld_strRecurrence"].ToString());
                        drRow = dtProviderHours.Rows[iCount];
                        theDataSet = new DataSet();
                        if (theDataSet.Tables.Count > 0 && theDataSet.Tables[0] != null)
                        {
                            try
                            {
                                theDataSet.Tables[0].Rows.Clear();
                                theDataSet.Tables.RemoveAt(0);
                            }
                            catch (Exception)
                            {
                                theDataSet.Clear();
                            }

                        }

                        theDataSet.ReadXml(theReader);
                        dtCustomeHours = theDataSet.Tables[0];

                        if (theDataSet.Tables.Contains("Appointment"))
                        {
                            dtReccurenceExpectionDates = theDataSet.Tables["Appointment"];
                        }
                        #region FirstCheck whether Daily or Weekly
                        if (dtCustomeHours.Columns.Contains("DailyRecurrenceMode") && !dtCustomeHours.Columns.Contains("RecurrenceType"))
                        {
                            CalculateDailyProviderCustomHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "WEEKLY")
                        {
                            CalculateWeeklyProviderCustomHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (!dtCustomeHours.Columns.Contains("DailyRecurrenceMode") && !dtCustomeHours.Columns.Contains("RecurrenceType"))
                        {
                            CalculateDailyProviderCustomHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "MONTHLY")
                        {
                            CalculateMonthlyCustomHoursProvider(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "MONTNTH")
                        {
                            CalculateMonthNthCustomHoursProvider(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "YEARLY")
                        {
                            CalculateYearlyCustomHoursProvider(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "YEARNTH")
                        {
                            CalculateYearNthCustomHoursProvider(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                    }
                    #endregion

                    //for (int i = 0; i < dtResult.Rows.Count; i++)
                    //{
                    //    dtResult.Rows[i]["Clinic_Number"] = "0";
                    //    dtResult.Rows[i]["Service_Install_Id"] = "1";
                    //}
                }
                DataView dv = new DataView(dtResult);
                if (dtResult.Rows.Count > 0)
                {
                    dv.RowFilter = "StartTime <> EndTime";
                }
                return dv.ToTable();
                //return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResultProviderHours;
        }

        public static void CreateTableDTProviderHours(ref DataTable dtResult)
        {
            DataColumn dcCol1 = new DataColumn();
            dcCol1.ColumnName = "PH_LocalDB_ID";
            DataColumn dcCol2 = new DataColumn();
            dcCol2.ColumnName = "PH_EHR_ID";
            DataColumn dcCol3 = new DataColumn();
            dcCol3.ColumnName = "PH_Web_ID";
            DataColumn dcCol4 = new DataColumn();
            dcCol4.ColumnName = "Provider_EHR_ID";
            DataColumn dcCol5 = new DataColumn();
            dcCol5.ColumnName = "Operatory_EHR_ID";

            DataColumn dcCol6 = new DataColumn();
            dcCol6.ColumnName = "StartTime";
            DataColumn dcCol7 = new DataColumn();
            dcCol7.ColumnName = "EndTime";
            DataColumn dcCol8 = new DataColumn();
            dcCol8.ColumnName = "comment";
            DataColumn dcCol9 = new DataColumn();
            dcCol9.ColumnName = "Entry_DateTime";
            DataColumn dcCol10 = new DataColumn();
            dcCol10.ColumnName = "Last_Sync_Date";

            DataColumn dcCol11 = new DataColumn();
            dcCol11.ColumnName = "Clinic_Number";

            DataColumn dcCol12 = new DataColumn();
            dcCol12.ColumnName = "Service_Install_Id";

            dtResult.Columns.Add(dcCol1);
            dtResult.Columns.Add(dcCol2);
            dtResult.Columns.Add(dcCol3);
            dtResult.Columns.Add(dcCol4);
            dtResult.Columns.Add(dcCol5);
            dtResult.Columns.Add(dcCol6);
            dtResult.Columns.Add(dcCol7);
            dtResult.Columns.Add(dcCol8);
            dtResult.Columns.Add(dcCol9);
            dtResult.Columns.Add(dcCol10);
            dtResult.Columns.Add(dcCol11);
            dtResult.Columns.Add(dcCol12);
        }

        private static void CalculateWeeklyProviderCustomHours(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDateProvider(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;
                bool startrecurrance = false;
                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                }

                //  MessageBox.Show("PatternStartDate " + PatternStartDate.ToShortDateString() + " PatterEndDate " + PatternEndDate.ToShortDateString());

                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    // MessageBox.Show("PatternStartDate " + PatternStartDate.ToShortDateString() + " PatterEndDate " + PatternEndDate.ToShortDateString());
                    if (CheckforWorkingWeeklyProvider(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        //if (recInterval > 0)
                        //{
                        //    startrecurrance = true;
                        //    recInterval = 0;
                        //}
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            // AddNewRowToResult(drRow, dtDailyHours);
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResultProvider(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                            }
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfWeek"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                        }
                        else
                        {
                            array = "Monday".Split(',');
                        }
                        //var array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.DayOfWeek.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                        recEndInterval++;
                    }
                    PatternStartDate = PatternStartDate.AddDays(1);
                    if (RecurrenceInterval > 0)
                    {
                        //if (startrecurrance)
                        //{
                        //    recInterval++;
                        //}
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }

                //dataGridView3.DataSource = dtResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool CheckforWorkingWeeklyProvider(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                //var array[]; 
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfWeek"))
                {
                    array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                }
                else
                {
                    array = "Monday".Split(',');
                }

                if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.DayOfWeek.ToString().ToUpper()).Count() > 0)
                {
                    AllowCreateCustomHours = true;
                }
                //if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatterDate.DayOfWeek.ToString().ToUpper())
                //{
                //    recInterval++;
                //}                     
                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void CalculateMonthlyCustomHoursProvider(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;
                bool startrecurrance = false;
                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                    RecurrenceEndInterval = RecurrenceEndInterval - 1;
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                        RecurrenceEndInterval = RecurrenceEndInterval - 1;
                    }
                }

                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (PatternStartDate > DateTime.Now.AddYears(1)) break;
                    if (CheckforWorkingMonthlyProvider(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResultProvider(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                            }
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfMonth"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfMonth"].ToString().Split(',');
                        }
                        else
                        {
                            array = "1".Split(',');
                        }
                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.Day.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                        recEndInterval++;
                    }
                    PatternStartDate = PatternStartDate.AddDays(1);
                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingMonthlyProvider(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfMonth"))
                {
                    array = dtDailyHours.Rows[0]["DayOfMonth"].ToString().Split(',');
                }
                else
                {
                    array = "1".Split(',');
                }

                if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.Day.ToString().ToUpper()).Count() > 0)
                {
                    AllowCreateCustomHours = true;
                }
                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CalculateMonthNthCustomHoursProvider(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;

                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("OccurrenceInMonth"))
                {
                    string strOIM = dtDailyHours.Rows[0]["OccurrenceInMonth"].ToString();
                }

                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                    RecurrenceEndInterval = RecurrenceEndInterval - 1;
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                        RecurrenceEndInterval = RecurrenceEndInterval - 1;
                    }
                }

                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (PatternStartDate > DateTime.Now.AddYears(1)) break;
                    if (CheckforWorkingMonthNthProvider(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResultProvider(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                            }
                            recEndInterval++;
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfWeek"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                        }
                        else
                        {
                            array = "Monday".Split(',');
                        }

                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.DayOfWeek.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                    }
                    PatternStartDate = PatternStartDate.AddDays(1);
                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingMonthNthProvider(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfWeek"))
                {
                    array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                }
                else
                {
                    array = "Monday".Split(',');
                }


                string strOccuranceInMonth = "";
                if (dtDailyHours.Columns.Contains("OccurrenceInMonth"))
                {
                    strOccuranceInMonth = dtDailyHours.Rows[0]["OccurrenceInMonth"].ToString();
                }
                else
                {
                    strOccuranceInMonth = "First";
                }

                if (PatterDate.Date == new DateTime(2024, 09, 23).Date)
                {
                    string str = "1";
                }

                if (array.Length > 0)
                {
                    if (IsDayOfWeekOccurenceInMonth(PatterDate, array, strOccuranceInMonth))
                    {
                        AllowCreateCustomHours = true;
                    }
                }

                //if (array.Length == 1)
                //{
                //    if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.DayOfWeek.ToString().ToUpper()).Count() > 0)
                //    {
                //        if (IsDayOfWeekOccurrenceInMonth(PatterDate, PatterDate.DayOfWeek, strOccuranceInMonth))
                //        {
                //            AllowCreateCustomHours = true;
                //        }
                //    }
                //}
                //else
                //{
                //    if (IsDayOfWeekOccurenceInMonth(PatterDate, array, strOccuranceInMonth))
                //    {
                //        AllowCreateCustomHours = true;
                //    }
                //}

                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CalculateYearlyCustomHoursProvider(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;
                bool startrecurrance = false;
                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                        RecurrenceEndInterval = RecurrenceEndInterval - 1;
                    }
                }

                bool blnAddYears = false;
                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (PatternStartDate > DateTime.Now.AddYears(1)) break;
                    if (CheckforWorkingYearlyProvider(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResultProvider(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                blnAddYears = true;
                            }
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfMonth"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfMonth"].ToString().Split(',');
                        }
                        else
                        {
                            array = "1".Split(',');
                        }
                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.Day.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                        recEndInterval++;
                    }
                    if (!blnAddYears)
                    {
                        PatternStartDate = PatternStartDate.AddDays(1);
                    }
                    else
                    {
                        PatternStartDate = new DateTime(PatternStartDate.Year + 1, PatternStartDate.Month, 1);
                    }

                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingYearlyProvider(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfMonth") & dtDailyHours.Columns.Contains("MonthOfYear"))
                {
                    array = (dtDailyHours.Rows[0]["DayOfMonth"].ToString() + "-" + dtDailyHours.Rows[0]["MonthOfYear"].ToString()).Split(',');
                }
                else
                {
                    array = "1-1".Split(',');
                }

                if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.Day.ToString().ToUpper() + "-" + PatterDate.Month.ToString().ToUpper()).Count() > 0)
                {
                    AllowCreateCustomHours = true;
                }
                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CalculateYearNthCustomHoursProvider(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;

                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("OccurrenceInMonth"))
                {
                    string strOIM = dtDailyHours.Rows[0]["OccurrenceInMonth"].ToString();
                }

                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                    RecurrenceEndInterval = RecurrenceEndInterval - 1;
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                        RecurrenceEndInterval = RecurrenceEndInterval - 1;
                    }
                }

                bool blnAddYears = false;
                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (PatternStartDate > DateTime.Now.AddYears(1)) break;
                    if (CheckforWorkingYearNthProvider(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResultProvider(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                blnAddYears = true;
                            }
                            recEndInterval++;
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfWeek"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                        }
                        else
                        {
                            array = "Monday".Split(',');
                        }

                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.DayOfWeek.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                    }
                    if (!blnAddYears)
                    {
                        PatternStartDate = PatternStartDate.AddDays(1);
                    }
                    else
                    {
                        PatternStartDate = new DateTime(PatternStartDate.Year + 1, PatternStartDate.Month, 01);
                        blnAddYears = false;
                    }
                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingYearNthProvider(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfWeek"))
                {
                    array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                }
                else
                {
                    array = "Monday".Split(',');
                }


                string strOccuranceInMonth = "";
                if (dtDailyHours.Columns.Contains("OccurrenceInMonth"))
                {
                    strOccuranceInMonth = dtDailyHours.Rows[0]["OccurrenceInMonth"].ToString();
                }
                else
                {
                    strOccuranceInMonth = "First";
                }

                if (IsDayOfWeekOccurenceInMonth(PatterDate, array, strOccuranceInMonth))
                {
                    int Month = Convert.ToInt32(dtDailyHours.Rows[0]["MonthOfYear"]);
                    if (Month == PatterDate.Month)
                    {
                        AllowCreateCustomHours = true;
                    }
                }

                #region Commented
                //if (array.Length == 1)
                //{
                //    if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.DayOfWeek.ToString().ToUpper()).Count() > 0)
                //    {
                //        if (IsDayOfWeekOccurrenceInMonth(PatterDate, PatterDate.DayOfWeek, strOccuranceInMonth))
                //        {
                //            if (dtDailyHours.Columns.Contains("MonthOfYear"))
                //            {
                //                int Month = Convert.ToInt32(dtDailyHours.Rows[0]["MonthOfYear"]);
                //                if (Month == PatterDate.Month)
                //                {
                //                    AllowCreateCustomHours = true;
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    if (IsDayOfWeekOccurenceInMonth(PatterDate, array, strOccuranceInMonth))
                //    {
                //        int Month = Convert.ToInt32(dtDailyHours.Rows[0]["MonthOfYear"]);
                //        if (Month == PatterDate.Month)
                //        {
                //            AllowCreateCustomHours = true;
                //        }
                //    }
                //}
                #endregion

                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static void CalculateDailyProviderCustomHours(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;
                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (!dtDailyHours.Columns.Contains("Occurrences") && !dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    RecurrenceEndInterval = 10;
                }
                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                }
                if (dtDailyHours.Columns.Contains("DailyRecurrenceMode"))
                {
                    if (dtDailyHours.Rows[0]["DailyRecurrenceMode"].ToString().ToUpper() == "WEEKDAYS" || dtDailyHours.Rows[0]["DailyRecurrenceMode"].ToString().ToUpper() == "WEEKENDDAYS")
                    {
                        while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                        {
                            if (AllowToCreateCustomHoursProvider(dtDailyHours.Rows[0]["DailyRecurrenceMode"].ToString().ToUpper(), PatternStartDate, dtReccurenceExpectionDates))
                            {
                                //if (recInterval == 0)
                                //{
                                if (PatternStartDate.Date >= DateTime.Now.Date)
                                {
                                    AddNewRowToResultProvider(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                }
                                //}
                                recEndInterval++;
                                //if (dtDailyHours.Rows[0]["DailyRecurrenceMode"].ToString().ToUpper() == "WEEKENDDAYS")
                                //{
                                //    if (RecurrenceInterval > 0)
                                //    {
                                //        recInterval++;
                                //        if (recInterval == RecurrenceInterval)
                                //        {
                                //            recInterval = 0;
                                //        }
                                //    }
                                //}
                            }
                            PatternStartDate = PatternStartDate.AddDays(1);
                            if (RecurrenceInterval > 0)
                            {
                                recInterval++;
                                if (recInterval == RecurrenceInterval)
                                {
                                    recInterval = 0;
                                }
                            }
                        }
                    }
                }
                // For all the days between PatternStart & EndDate
                else
                {
                    while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                    {

                        if (dtReccurenceExpectionDates.Rows.Count > 0)
                        {
                            if (dtReccurenceExpectionDates.AsEnumerable()
                                .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() == 0)
                            {
                                if (recInterval == 0)
                                {
                                    AddNewRowToResultProvider(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                    recEndInterval++;
                                }
                            }
                            else
                            {
                                recEndInterval++;
                            }
                        }
                        else
                        {
                            if (recInterval == 0 || RecurrenceInterval == 0)
                            {
                                AddNewRowToResultProvider(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                recEndInterval++;
                            }
                        }
                        PatternStartDate = PatternStartDate.AddDays(1);
                        if (RecurrenceInterval > 0)
                        {
                            recInterval++;
                            if (recInterval == RecurrenceInterval)
                            {
                                recInterval = 0;
                            }
                        }
                    }

                    //while( PatternStartDate <= PatternEndDate)
                    //{
                    //    AddNewRowToResultProvider(drRow, dtDailyHours);
                    //    PatternStartDate = PatternStartDate.AddDays(1);
                    //}
                }

                //dataGridView3.DataSource = dtResult;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool AllowToCreateCustomHoursProvider(string RecurrenceType, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool isAllowToEnterEntry = false;
            try
            {

                switch (PatternStartDate.DayOfWeek.ToString().ToUpper())
                {
                    case "MONDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "TUESDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "WEDNESDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "THURSDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "FRIDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "SATURDAY":
                        if (RecurrenceType.ToUpper() == "WEEKENDDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "SUNDAY":
                        if (RecurrenceType.ToUpper() == "WEEKENDDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                }
                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        isAllowToEnterEntry = false;
                    }
                }
                return isAllowToEnterEntry;
            }
            catch (Exception)
            {
                return isAllowToEnterEntry;
                throw;
            }
        }

        public static void SetPatternStartEndDateProvider(DataTable dtCustomHours, ref DateTime PatternStartDate, ref DateTime PatternEndDate)
        {
            try
            {
                if (dtCustomHours.Columns.Contains("PatternStartDate"))
                {
                    PatternStartDate = Convert.ToDateTime(dtCustomHours.Rows[0]["PatternStartDate"].ToString());
                    if (PatternStartDate < DateTime.Now)
                    {
                        PatternStartDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    }
                }
                if (dtCustomHours.Columns.Contains("RecurrenceEndMode"))
                {

                    if (dtCustomHours.Rows[0]["RecurrenceEndMode"].ToString().ToUpper() == "ENDDATE")
                    {
                        PatternEndDate = Convert.ToDateTime(dtCustomHours.Rows[0]["PatternEndDate"].ToString());
                    }
                    else if (dtCustomHours.Rows[0]["RecurrenceEndMode"].ToString().ToUpper() == "NOENDDATE")
                    {
                        PatternEndDate = DateTime.Now.AddYears(1);
                    }
                }
                else
                {
                    PatternEndDate = DateTime.Now.AddYears(1);
                }
                // MessageBox.Show("PatternStartDate " + PatternStartDate.ToShortDateString() + " PatterEndDate " + PatternEndDate.ToShortDateString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddNewRowToResultProvider(DataRow drRow, DataTable dtDailyHours, ref DataTable dtResult, ref DateTime PatternStartDate)
        {
            try
            {
                //var array = drRow["fld_strWorkingChairs"].ToString().Split(',');
                //foreach (var item in array)
                //{
                DataRow drNew = dtResult.NewRow();
                drNew["PH_LocalDB_ID"] = "";
                drNew["PH_EHR_ID"] = drRow["fld_auto_intSchPrId"] + "/" + drRow["fld_shtPrId"] + "_" + PatternStartDate.Year.ToString() + "" + PatternStartDate.Month.ToString("00") + "" + PatternStartDate.Day.ToString("00");
                //drNew["OH_EHR_ID"] = Guid.NewGuid().ToString("n");
                drNew["PH_Web_ID"] = "";
                drNew["Provider_EHR_ID"] = drRow["fld_shtPrId"].ToString();
                drNew["Operatory_EHR_ID"] = "";
                drNew["StartTime"] = PatternStartDate.ToShortDateString() + " " + (dtDailyHours.Columns.Contains("StartTime") ? dtDailyHours.Rows[0]["StartTime"].ToString() : "");
                drNew["EndTime"] = PatternStartDate.ToShortDateString() + " " + (dtDailyHours.Columns.Contains("EndTime") ? dtDailyHours.Rows[0]["EndTime"].ToString() : "");
                drNew["comment"] = drRow["fld_strDescription"].ToString();
                drNew["Clinic_Number"] = "0";
                drNew["Service_Install_Id"] = "1";
                dtResult.Rows.Add(drNew);
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetClearDentOperatoryTimeOffHours()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentOperatoryTimeOffHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return CreateTableOfOperatoryHours(SqlDt);

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

        #endregion

        #region Folder List

        public static DataTable GetClearDentFolderListData()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentFolderListData;
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

        #endregion

        #region FolderList

        public static bool Save_FolderList_ClearDent_To_Local(DataTable dtClearDentOperatory, string Service_Install_Id, string clinicNumber)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();    
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtClearDentOperatory.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_FolderList;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_FolderList;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_FolderList;
                                        break;
                                    case 4:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_False_FolderList;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("FolderList_EHR_ID", dr["FolderList_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Folder_Name", dr["Folder_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.Parameters.AddWithValue("FolderOrder", 0);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    //  SqlCetx.Commit();
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

        public static DataTable GetClearDentDeletedFolderListData()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetOpenDentalDeletedFolderListData;
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

        #endregion


        #region Operatory

        public static DataTable GetClearDentOperatoryData()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentOperatoryData;
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

        public static DataTable GetClearDentDeletedOperatoryData()
        {

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetOpenDentalDeletedOperatoryData;
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

        public static bool Save_Operatory_ClearDent_To_Local(DataTable dtClearDentOperatory)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open();    
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtClearDentOperatory.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (dr["OperatoryOrder"] != null && dr["OperatoryOrder"].ToString().Trim() == "")
                            {
                                dr["OperatoryOrder"] = "0";
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
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("OperatoryOrder", Convert.ToInt16(dr["OperatoryOrder"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    //  SqlCetx.Commit();
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

        #region OperatoryHours
        public static DataTable GetClearDentOperatoryHours()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentOperatoryHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);
                return CreateTableOfOperatoryHours(SqlDt);

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

        public static DataTable CreateTableOfOperatoryHours(DataTable dtOperatoryHours)
        {
            DataSet theDataSet = new DataSet();
            DataTable dtResultOperatoryHours = new DataTable();
            DataTable dtCustomeHours = new DataTable();
            DataTable dtResult = new DataTable();
            DataTable dtReccurenceExpectionDates = new DataTable();
            DataRow drRow = null;
            try
            {
                if (dtOperatoryHours.Rows.Count > 0)
                {
                    CreateTableDTOperatoryHours(ref dtResult);
                    for (int iCount = 0; iCount <= dtOperatoryHours.Rows.Count - 1; iCount++)
                    {
                        StringReader theReader = new StringReader(dtOperatoryHours.Rows[iCount]["fld_strRecurrence"].ToString());
                        drRow = dtOperatoryHours.Rows[iCount];
                        theDataSet = new DataSet();
                        if (theDataSet.Tables.Count > 0 && theDataSet.Tables[0] != null)
                        {
                            theDataSet.Tables[0].Rows.Clear();
                            theDataSet.Tables.RemoveAt(0);
                        }
                        theDataSet.ReadXml(theReader);
                        dtCustomeHours = theDataSet.Tables[0];
                        if (theDataSet.Tables.Contains("Appointment"))
                        {
                            dtReccurenceExpectionDates = theDataSet.Tables["Appointment"];
                        }

                        #region FirstCheck whether Daily or Weekly
                        if (dtCustomeHours.Columns.Contains("DailyRecurrenceMode") && !dtCustomeHours.Columns.Contains("RecurrenceType"))
                        {
                            CalculateDailyCustomeHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "WEEKLY")
                        {
                            CalculateWeeklyCustomHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (!dtCustomeHours.Columns.Contains("DailyRecurrenceMode") && !dtCustomeHours.Columns.Contains("RecurrenceType"))
                        {
                            CalculateDailyCustomeHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "MONTHLY")
                        {
                            CalculateMonthlyCustomHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "MONTNTH")
                        {
                            CalculateMonthNthCustomHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "YEARLY")
                        {
                            CalculateYearlyCustomHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        else if (dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "YEARNTH")
                        {
                            CalculateYearNthCustomHours(drRow, dtCustomeHours, dtReccurenceExpectionDates, dtResult);
                        }
                        #endregion
                    }
                }
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtResultOperatoryHours;
        }

        public static void CreateTableDTOperatoryHours(ref DataTable dtResult)
        {
            DataColumn dcCol1 = new DataColumn();
            dcCol1.ColumnName = "OH_LocalDB_ID";
            DataColumn dcCol2 = new DataColumn();
            dcCol2.ColumnName = "OH_EHR_ID";
            DataColumn dcCol3 = new DataColumn();
            dcCol3.ColumnName = "OH_Web_ID";
            DataColumn dcCol4 = new DataColumn();
            dcCol4.ColumnName = "Operatory_EHR_ID";
            DataColumn dcCol5 = new DataColumn();
            dcCol5.ColumnName = "StartTime";
            DataColumn dcCol6 = new DataColumn();
            dcCol6.ColumnName = "EndTime";
            DataColumn dcCol7 = new DataColumn();
            dcCol7.ColumnName = "comment";
            DataColumn dcCol8 = new DataColumn();
            dcCol8.ColumnName = "Entry_DateTime";
            DataColumn dcCol9 = new DataColumn();
            dcCol9.ColumnName = "Last_Sync_Date";

            DataColumn dcCol10 = new DataColumn();
            dcCol10.ColumnName = "Clinic_Number";

            DataColumn dcCol11 = new DataColumn();
            dcCol11.ColumnName = "Service_Install_Id";

            DataColumn dcCol12 = new DataColumn();
            dcCol12.ColumnName = "Provider_EHR_ID";

            dtResult.Columns.Add(dcCol1);
            dtResult.Columns.Add(dcCol2);
            dtResult.Columns.Add(dcCol3);
            dtResult.Columns.Add(dcCol4);
            dtResult.Columns.Add(dcCol5);
            dtResult.Columns.Add(dcCol6);
            dtResult.Columns.Add(dcCol7);
            dtResult.Columns.Add(dcCol8);
            dtResult.Columns.Add(dcCol9);
            dtResult.Columns.Add(dcCol10);
            dtResult.Columns.Add(dcCol11);
            dtResult.Columns.Add(dcCol12);
        }

        private static void CalculateWeeklyCustomHours(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;
                bool startrecurrance = false;
                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                    }
                }

                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (CheckforWorkingWeekly(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResult(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                            }
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfWeek"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                        }
                        else
                        {
                            array = "Monday".Split(',');
                        }
                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.DayOfWeek.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                        recEndInterval++;
                    }
                    PatternStartDate = PatternStartDate.AddDays(1);
                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingWeekly(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfWeek"))
                {
                    array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                }
                else
                {
                    array = "Monday".Split(',');
                }

                if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.DayOfWeek.ToString().ToUpper()).Count() > 0)
                {
                    AllowCreateCustomHours = true;
                }
                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CalculateMonthlyCustomHours(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;
                bool startrecurrance = false;
                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                    RecurrenceEndInterval = RecurrenceEndInterval - 1;
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                        RecurrenceEndInterval = RecurrenceEndInterval - 1;
                    }
                }

                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (PatternStartDate > DateTime.Now.AddYears(1)) break;
                    if (CheckforWorkingMonthly(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResult(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                            }
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfMonth"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfMonth"].ToString().Split(',');
                        }
                        else
                        {
                            array = "1".Split(',');
                        }
                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.Day.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                        recEndInterval++;
                    }
                    PatternStartDate = PatternStartDate.AddDays(1);
                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingMonthly(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfMonth"))
                {
                    array = dtDailyHours.Rows[0]["DayOfMonth"].ToString().Split(',');
                }
                else
                {
                    array = "1".Split(',');
                }

                if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.Day.ToString().ToUpper()).Count() > 0)
                {
                    AllowCreateCustomHours = true;
                }
                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CalculateMonthNthCustomHours(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;

                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("OccurrenceInMonth"))
                {
                    string strOIM = dtDailyHours.Rows[0]["OccurrenceInMonth"].ToString();
                }

                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                    RecurrenceEndInterval = RecurrenceEndInterval - 1;
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                        RecurrenceEndInterval = RecurrenceEndInterval - 1;
                    }
                }

                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (PatternStartDate > DateTime.Now.AddYears(1)) break;
                    if (CheckforWorkingMonthNth(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResult(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                            }
                            recEndInterval++;
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfWeek"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                        }
                        else
                        {
                            array = "Monday".Split(',');
                        }

                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.DayOfWeek.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                    }
                    PatternStartDate = PatternStartDate.AddDays(1);
                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingMonthNth(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfWeek"))
                {
                    array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                }
                else
                {
                    array = "Monday".Split(',');
                }


                string strOccuranceInMonth = "";
                if (dtDailyHours.Columns.Contains("OccurrenceInMonth"))
                {
                    strOccuranceInMonth = dtDailyHours.Rows[0]["OccurrenceInMonth"].ToString();
                }
                else
                {
                    strOccuranceInMonth = "First";
                }

                if (PatterDate.Date == new DateTime(2024, 09, 23).Date)
                {
                    string str = "1";
                }

                if (array.Length > 0)
                {
                    if (IsDayOfWeekOccurenceInMonth(PatterDate, array, strOccuranceInMonth))
                    {
                        AllowCreateCustomHours = true;
                    }
                }

                //if (array.Length == 1)
                //{
                //    if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.DayOfWeek.ToString().ToUpper()).Count() > 0)
                //    {
                //        if (IsDayOfWeekOccurrenceInMonth(PatterDate, PatterDate.DayOfWeek, strOccuranceInMonth))
                //        {
                //            AllowCreateCustomHours = true;
                //        }
                //    }
                //}
                //else
                //{
                //    if (IsDayOfWeekOccurenceInMonth(PatterDate, array, strOccuranceInMonth))
                //    {
                //        AllowCreateCustomHours = true;
                //    }
                //}

                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static bool IsDayOfWeekOccurenceInMonth(DateTime date, string[] AllowedDays, string occurrence)
        {
            try
            {
                List<DateTime> MonthlyDates = GetAllMonthDates(AllowedDays, date.Month, date.Year);
                int Ordinal = GetOrdinalFromString(occurrence);
                DateTime dtToGet;
                if (Ordinal > 0)
                {
                    dtToGet = MonthlyDates[Ordinal - 1];
                }
                else
                {
                    dtToGet = MonthlyDates[MonthlyDates.Count - 1];
                }
                if (date.Date == dtToGet.Date)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static List<DateTime> GetAllMonthDates(string[] AllowedDays, int month, int year)
        {
            List<DateTime> ReturnDate = new List<DateTime>();
            try
            {
                foreach (string str in AllowedDays)
                {
                    ReturnDate.AddRange(Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                        .Select(day => new DateTime(year, month, day))
                        .Where(dt => dt.DayOfWeek.ToString().Trim().ToUpper() == str.Trim().ToUpper())
                        .ToList());
                }
                ReturnDate.Sort();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ReturnDate;
        }

        private static void CalculateYearlyCustomHours(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;
                bool startrecurrance = false;
                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                        RecurrenceEndInterval = RecurrenceEndInterval - 1;
                    }
                }

                bool blnAddYears = false;
                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (PatternStartDate > DateTime.Now.AddYears(1)) break;
                    if (CheckforWorkingYearly(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResult(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                blnAddYears = true;
                            }
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfMonth"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfMonth"].ToString().Split(',');
                        }
                        else
                        {
                            array = "1".Split(',');
                        }
                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.Day.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                        recEndInterval++;
                    }
                    if (!blnAddYears)
                    {
                        PatternStartDate = PatternStartDate.AddDays(1);
                    }
                    else
                    {
                        PatternStartDate = new DateTime(PatternStartDate.Year + 1, PatternStartDate.Month, 1);
                    }

                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingYearly(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfMonth") & dtDailyHours.Columns.Contains("MonthOfYear"))
                {
                    array = (dtDailyHours.Rows[0]["DayOfMonth"].ToString() + "-" + dtDailyHours.Rows[0]["MonthOfYear"].ToString()).Split(',');
                }
                else
                {
                    array = "1-1".Split(',');
                }

                if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.Day.ToString().ToUpper() + "-" + PatterDate.Month.ToString().ToUpper()).Count() > 0)
                {
                    AllowCreateCustomHours = true;
                }
                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void CalculateYearNthCustomHours(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;

                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (dtDailyHours.Columns.Contains("OccurrenceInMonth"))
                {
                    string strOIM = dtDailyHours.Rows[0]["OccurrenceInMonth"].ToString();
                }

                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                    RecurrenceEndInterval = RecurrenceEndInterval - 1;
                }

                if (dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    if (dtDailyHours.Rows[0]["RecurrenceEndMode"].ToString().Trim().ToUpper() == "NOENDDATE")
                    {
                        RecurrenceEndInterval = 0;
                    }
                }
                else
                {
                    if (!dtDailyHours.Columns.Contains("Occurrences"))
                    {
                        RecurrenceEndInterval = 10;
                        RecurrenceEndInterval = RecurrenceEndInterval - 1;
                    }
                }

                bool blnAddYears = false;
                while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                {
                    if (PatternStartDate.Date > DateTime.Now.Date.AddYears(1)) break;
                    if (CheckforWorkingYearNth(drRow, dtDailyHours, PatternStartDate, ref recInterval, PatternStartDate, dtReccurenceExpectionDates))
                    {
                        if (recInterval == 0 || RecurrenceInterval == 0)
                        {
                            if (PatternStartDate.Date >= DateTime.Now.Date)
                            {
                                AddNewRowToResult(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                blnAddYears = true;
                            }
                            recEndInterval++;
                        }
                        string[] array = new string[] { };
                        if (dtDailyHours.Columns.Contains("DayOfWeek"))
                        {
                            array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                        }
                        else
                        {
                            array = "Monday".Split(',');
                        }

                        if (array.AsEnumerable().Last().ToString().ToUpper().Trim() == PatternStartDate.DayOfWeek.ToString().ToUpper())
                        {
                            recInterval++;
                        }
                    }
                    if (!blnAddYears)
                    {
                        PatternStartDate = PatternStartDate.AddDays(1);
                    }
                    else
                    {
                        PatternStartDate = new DateTime(PatternStartDate.Year + 1, PatternStartDate.Month, 01);
                        blnAddYears = false;
                    }
                    if (RecurrenceInterval > 0)
                    {
                        if (recInterval == RecurrenceInterval)
                        {
                            recInterval = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckforWorkingYearNth(DataRow drRow, DataTable dtDailyHours, DateTime PatterDate, ref int recInterval, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool AllowCreateCustomHours = false;
            try
            {
                string[] array = new string[] { };
                if (dtDailyHours.Columns.Contains("DayOfWeek"))
                {
                    array = dtDailyHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                }
                else
                {
                    array = "Monday".Split(',');
                }


                string strOccuranceInMonth = "";
                if (dtDailyHours.Columns.Contains("OccurrenceInMonth"))
                {
                    strOccuranceInMonth = dtDailyHours.Rows[0]["OccurrenceInMonth"].ToString();
                }
                else
                {
                    strOccuranceInMonth = "First";
                }

                if (IsDayOfWeekOccurenceInMonth(PatterDate, array, strOccuranceInMonth))
                {
                    int Month = Convert.ToInt32(dtDailyHours.Rows[0]["MonthOfYear"]);
                    if (Month == PatterDate.Month)
                    {
                        AllowCreateCustomHours = true;
                    }
                }

                #region Commented
                //if (array.Length == 1)
                //{
                //    if (array.AsEnumerable().Where(o => o.ToUpper().Trim() == PatterDate.DayOfWeek.ToString().ToUpper()).Count() > 0)
                //    {
                //        if (IsDayOfWeekOccurrenceInMonth(PatterDate, PatterDate.DayOfWeek, strOccuranceInMonth))
                //        {
                //            if (dtDailyHours.Columns.Contains("MonthOfYear"))
                //            {
                //                int Month = Convert.ToInt32(dtDailyHours.Rows[0]["MonthOfYear"]);
                //                if (Month == PatterDate.Month)
                //                {
                //                    AllowCreateCustomHours = true;
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    if (IsDayOfWeekOccurenceInMonth(PatterDate, array, strOccuranceInMonth))
                //    {
                //        int Month = Convert.ToInt32(dtDailyHours.Rows[0]["MonthOfYear"]);
                //        if (Month == PatterDate.Month)
                //        {
                //            AllowCreateCustomHours = true;
                //        }
                //    }
                //}
                #endregion

                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        AllowCreateCustomHours = false;
                    }
                }
                return AllowCreateCustomHours;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static int GetOrdinalFromString(string ordinalString)
        {
            switch (ordinalString.ToLower())
            {
                case "first":
                    return 1;
                case "second":
                    return 2;
                case "third":
                    return 3;
                case "fourth":
                    return 4;
                case "last":
                    return -1;
                default:
                    throw new ArgumentException("Invalid ordinal string");
            }
        }

        //static bool IsDayOfWeekOccurrenceInMonth(DateTime date, DayOfWeek dayOfWeek, string occurrence)
        //{
        //    if (string.IsNullOrWhiteSpace(occurrence))
        //        throw new ArgumentException("Occurrence cannot be null or empty.");

        //    int count = 0;
        //    DateTime currentDate = new DateTime(date.Year, date.Month, 1);

        //    while (currentDate.Month == date.Month)
        //    {
        //        if (currentDate.DayOfWeek == dayOfWeek)
        //        {
        //            count++;
        //            if (GetOrdinal(count) == occurrence && currentDate == date)
        //                return true;
        //        }

        //        currentDate = currentDate.AddDays(1);
        //    }

        //    return false;
        //}

        //static string GetOrdinal(int number)
        //{
        //    switch (number)
        //    {
        //        case 1:
        //            return "First";
        //        case 2:
        //            return "Second";
        //        case 3:
        //            return "Third";
        //        case 4:
        //            return "Fourth";
        //        default:
        //            return "Last";
        //    }
        //}

        public static void CalculateDailyCustomeHours(DataRow drRow, DataTable dtDailyHours, DataTable dtReccurenceExpectionDates, DataTable dtResult)
        {
            DateTime PatternStartDate = DateTime.Now;
            DateTime PatternEndDate = DateTime.Now;

            int RecurrenceInterval = 0;
            int RecurrenceEndInterval = 0;
            try
            {
                SetPatternStartEndDate(dtDailyHours, ref PatternStartDate, ref PatternEndDate);
                int recInterval = 0, recEndInterval = 0;
                // Check for WeekDays OR WeekendDays
                if (dtDailyHours.Columns.Contains("Interval"))
                {
                    RecurrenceInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Interval"]);
                }
                if (!dtDailyHours.Columns.Contains("Occurrences") && !dtDailyHours.Columns.Contains("RecurrenceEndMode"))
                {
                    RecurrenceEndInterval = 10;
                }
                if (dtDailyHours.Columns.Contains("Occurrences"))
                {
                    RecurrenceEndInterval = Convert.ToInt32(dtDailyHours.Rows[0]["Occurrences"]);
                }

                if (dtDailyHours.Columns.Contains("DailyRecurrenceMode"))
                {
                    if (dtDailyHours.Rows[0]["DailyRecurrenceMode"].ToString().ToUpper() == "WEEKDAYS" || dtDailyHours.Rows[0]["DailyRecurrenceMode"].ToString().ToUpper() == "WEEKENDDAYS")
                    {
                        while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                        {
                            if (AllowToCreateCustomHours(dtDailyHours.Rows[0]["DailyRecurrenceMode"].ToString().ToUpper(), PatternStartDate, dtReccurenceExpectionDates))
                            {
                                if (PatternStartDate.Date >= DateTime.Now.Date)
                                {
                                    AddNewRowToResult(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                }
                                recEndInterval++;
                            }
                            PatternStartDate = PatternStartDate.AddDays(1);
                            if (RecurrenceInterval > 0)
                            {
                                recInterval++;
                                if (recInterval == RecurrenceInterval)
                                {
                                    recInterval = 0;
                                }
                            }
                        }
                    }
                }
                // For all the days between PatternStart & EndDate
                else
                {
                    while ((PatternStartDate <= PatternEndDate && RecurrenceEndInterval == 0) || recEndInterval < RecurrenceEndInterval)
                    {
                        if (dtReccurenceExpectionDates.Rows.Count > 0)
                        {
                            if (dtReccurenceExpectionDates.AsEnumerable()
                                .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() == 0)
                            {
                                if (recInterval == 0)
                                {
                                    if (PatternStartDate.Date >= DateTime.Now.Date)
                                    {
                                        AddNewRowToResult(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                    }
                                    recEndInterval++;
                                }
                            }
                            else
                            {
                                recEndInterval++;
                            }
                        }
                        else
                        {
                            if (recInterval == 0 || RecurrenceInterval == 0)
                            {
                                if (PatternStartDate.Date >= DateTime.Now.Date)
                                {
                                    AddNewRowToResult(drRow, dtDailyHours, ref dtResult, ref PatternStartDate);
                                }
                                recEndInterval++;
                            }
                        }
                        PatternStartDate = PatternStartDate.AddDays(1);
                        if (RecurrenceInterval > 0)
                        {
                            recInterval++;
                            if (recInterval == RecurrenceInterval)
                            {
                                recInterval = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AllowToCreateCustomHours(string RecurrenceType, DateTime PatternStartDate, DataTable dtReccurenceExpectionDates)
        {
            bool isAllowToEnterEntry = false;
            try
            {

                switch (PatternStartDate.DayOfWeek.ToString().ToUpper())
                {
                    case "MONDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "TUESDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "WEDNESDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "THURSDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "FRIDAY":
                        if (RecurrenceType.ToUpper() == "WEEKDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "SATURDAY":
                        if (RecurrenceType.ToUpper() == "WEEKENDDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                    case "SUNDAY":
                        if (RecurrenceType.ToUpper() == "WEEKENDDAYS")
                        {
                            isAllowToEnterEntry = true;
                        }
                        break;
                }
                if (dtReccurenceExpectionDates.Rows.Count > 0)
                {
                    if (dtReccurenceExpectionDates.AsEnumerable()
                        .Where(o => Convert.ToDateTime(Convert.ToDateTime(o.Field<object>("StartTime")).ToShortDateString()) == Convert.ToDateTime(PatternStartDate.ToShortDateString())).Count() > 0)
                    {
                        isAllowToEnterEntry = false;
                    }
                }
                return isAllowToEnterEntry;
            }
            catch (Exception)
            {
                return isAllowToEnterEntry;
                throw;
            }
        }

        public static void SetPatternStartEndDate(DataTable dtCustomHours, ref DateTime PatternStartDate, ref DateTime PatternEndDate)
        {
            try
            {
                if (dtCustomHours.Columns.Contains("PatternStartDate"))
                {
                    PatternStartDate = Convert.ToDateTime(dtCustomHours.Rows[0]["PatternStartDate"].ToString());
                    //if (PatternStartDate < DateTime.Now)
                    //{
                    //    PatternStartDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    //}
                }
                if (dtCustomHours.Columns.Contains("RecurrenceEndMode"))
                {

                    if (dtCustomHours.Rows[0]["RecurrenceEndMode"].ToString().ToUpper() == "ENDDATE")
                    {
                        PatternEndDate = Convert.ToDateTime(dtCustomHours.Rows[0]["PatternEndDate"].ToString());
                    }
                    else if (dtCustomHours.Rows[0]["RecurrenceEndMode"].ToString().ToUpper() == "NOENDDATE")
                    {
                        PatternEndDate = DateTime.Now.AddYears(1);
                    }
                }
                else
                {
                    PatternEndDate = DateTime.Now.AddYears(1);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void AddNewRowToResult(DataRow drRow, DataTable dtDailyHours, ref DataTable dtResult, ref DateTime PatternStartDate)
        {
            try
            {
                var array = drRow["fld_strWorkingChairs"].ToString().Split(',');
                foreach (var item in array)
                {
                    DataRow drNew = dtResult.NewRow();
                    drNew["OH_LocalDB_ID"] = "";
                    drNew["OH_EHR_ID"] = drRow["fld_auto_intSchPrId"] + "/" + drRow["fld_shtPrId"] + "/" + item.ToString() + "_" + PatternStartDate.Year.ToString() + "" + PatternStartDate.Month.ToString("00") + "" + PatternStartDate.Day.ToString("00");
                    //drNew["OH_EHR_ID"] = Guid.NewGuid().ToString("n");
                    drNew["Provider_EHR_ID"] = drRow["fld_shtPrId"].ToString();
                    drNew["OH_Web_ID"] = "";
                    drNew["Operatory_EHR_ID"] = item.ToString();
                    drNew["StartTime"] = PatternStartDate.ToShortDateString() + " " + (dtDailyHours.Columns.Contains("StartTime") ? dtDailyHours.Rows[0]["StartTime"].ToString() : "");
                    drNew["EndTime"] = PatternStartDate.ToShortDateString() + " " + (dtDailyHours.Columns.Contains("EndTime") ? dtDailyHours.Rows[0]["EndTime"].ToString() : "");
                    drNew["comment"] = drRow["fld_strDescription"].ToString();
                    drNew["Clinic_Number"] = "0";
                    drNew["Service_Install_Id"] = "1";
                    dtResult.Rows.Add(drNew);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Operatory OfficeHours

        public static DataTable GetClearDentOperatoryOfficeHours()
        {
            DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = new SqlDataAdapter();
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DataTable dtOperatoryOfficeHours = new DataTable();
            DataTable dtOperatory = new DataTable();
            DataTable OdbcDt = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentOperatoryOfficeHours;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //OdbcCommand.Parameters.AddWithValue("provider_Id", drRow["Provider_EHR_Id"].ToString());
                DataTable SqlDt = new DataTable();
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                SqlDa.Fill(SqlDt);

                return CreateTableOfOperatoryOfficeHours(SqlDt);

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

        private static DataTable CreateTableOfOperatoryOfficeHours(DataTable dtOperatoryOfficeHours)
        {
            DataTable dtResultOperatoryOfficeHours = new DataTable();
            DataTable dtResultOfficeNonWorkingHours = new DataTable();
            DataTable dtResultOfficeWorkingHours = new DataTable();

            DataSet theDataSet = new DataSet();
            DataTable dtCustomeHours = new DataTable();
            try
            {
                #region GetDistinct Operatory & Get Operatory Office Hours Structure
                DataTable dtOperatory = GetClearDentOperatoryData();
                dtResultOperatoryOfficeHours = SynchLocalDAL.GetLocalOperatoryOfficeHoursBlankStructure();

                dtResultOfficeNonWorkingHours = GetClearDentNonWorkingHours();
                dtResultOfficeWorkingHours = GetClearDentOfficeWorkingHours();

                #endregion

                #region Create Operatory & Daywise Datatable
                dtOperatory.AsEnumerable()
                    .All(o =>
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            DataRow drNewRow = dtResultOperatoryOfficeHours.NewRow();
                            drNewRow["OOH_EHR_ID"] = o["Operatory_EHR_ID"].ToString() + "_" + i.ToString();
                            drNewRow["Operatory_EHR_ID"] = o["Operatory_EHR_ID"].ToString();
                            drNewRow["WeekDay"] = GetWeekDays(i).ToString();
                            drNewRow["Clinic_Number"] = "0";
                            drNewRow["Service_Install_Id"] = "1";
                            dtResultOperatoryOfficeHours.Rows.Add(drNewRow);
                        }
                        return true;
                    });
                #endregion

                #region Update Start & EndDatetime in Provider Daywise DataTable
                string daystart1 = "", dayEnd1 = "";

                dtResultOperatoryOfficeHours.AsEnumerable()
                    .All(o =>
                    {
                        var resultProvider = dtResultOfficeWorkingHours.AsEnumerable().Where(a => a.Field<object>("WeekDay").ToString().ToUpper() == o["WeekDay"].ToString().ToUpper());

                        if (resultProvider.Count() > 0)
                        {
                            o["StartTime1"] = resultProvider.Select(b => b.Field<object>("StartTime")).First();
                            o["StartTime2"] = "01/01/1900 00:00:00";
                            o["StartTime3"] = "01/01/1900 00:00:00";

                            o["EndTime1"] = resultProvider.Select(b => b.Field<object>("EndTime")).First();
                            o["EndTime2"] = "01/01/1900 00:00:00";
                            o["EndTime3"] = "01/01/1900 00:00:00";
                        }
                        else
                        {
                            daystart1 = "fld_dtm" + o["WeekDay"].ToString().Substring(0, 3) + "Start";
                            dayEnd1 = "fld_dtm" + o["WeekDay"].ToString().Substring(0, 3) + "End";

                            resultProvider = dtOperatoryOfficeHours.AsEnumerable().Where(a => a.Field<object>("fld_auto_shtChId").ToString().ToUpper() == o["Operatory_EHR_ID"].ToString().ToUpper());

                            if (resultProvider.Count() > 0)
                            {
                                if (resultProvider.Select(b => b.Field<object>(daystart1)).First() != null && resultProvider.Select(b => b.Field<object>(daystart1)).First().ToString() != "")
                                {
                                    o["StartTime1"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(resultProvider.Select(b => b.Field<object>(daystart1)).First()).ToString("HH:mm"));
                                }
                                else
                                {
                                    o["StartTime1"] = "01/01/1900 00:00:00";
                                }
                                o["StartTime2"] = "01/01/1900 00:00:00";
                                o["StartTime3"] = "01/01/1900 00:00:00";

                                if (resultProvider.Select(b => b.Field<object>(dayEnd1)).First() != null && resultProvider.Select(b => b.Field<object>(dayEnd1)).First().ToString() != "")
                                {
                                    o["EndTime1"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(resultProvider.Select(b => b.Field<object>(dayEnd1)).First()).ToString("HH:mm"));
                                }
                                else
                                {
                                    o["EndTime1"] = "01/01/1900 00:00:00";
                                }

                                o["EndTime2"] = "01/01/1900 00:00:00";
                                o["EndTime3"] = "01/01/1900 00:00:00";

                            }
                            else
                            {
                                o["StartTime1"] = "01/01/1900 00:00:00";
                                o["EndTime1"] = "01/01/1900 00:00:00";
                                o["StartTime2"] = "01/01/1900 00:00:00";
                                o["EndTime2"] = "01/01/1900 00:00:00";
                                o["StartTime3"] = "01/01/1900 00:00:00";
                                o["EndTime3"] = "01/01/1900 00:00:00";
                            }
                        }
                        return true;
                    });
                #endregion

                #region Non Working Hours Calculate

                dtOperatory.AsEnumerable()
                   .All(o =>
                   {
                       var resultOperatory = dtResultOfficeNonWorkingHours.AsEnumerable().Where(a => a.Field<object>("fld_strWorkingChairs").ToString().ToUpper() == "");

                       if (resultOperatory.Count() > 0)
                       {
                           //int IntI = 1;

                           resultOperatory.AsEnumerable()
                               .All(p =>
                               {
                                   StringReader theReader = new StringReader(p["fld_strRecurrence"].ToString());
                                   theDataSet = new DataSet();
                                   if (theDataSet.Tables.Count > 0 && theDataSet.Tables[0] != null)
                                   {
                                       try
                                       {
                                           theDataSet.Tables[0].Rows.Clear();
                                           theDataSet.Tables.RemoveAt(0);
                                       }
                                       catch
                                       {
                                           theDataSet.Clear();
                                       }
                                       //theDataSet.Tables[0].Rows.Clear();
                                       //theDataSet.Tables.RemoveAt(0);
                                   }

                                   theDataSet.ReadXml(theReader);
                                   dtCustomeHours = theDataSet.Tables[0];

                                   if ((dtCustomeHours.Columns.Contains("RecurrenceType") && dtCustomeHours.Rows[0]["RecurrenceType"].ToString().ToUpper() == "WEEKLY") || dtCustomeHours.Columns.Contains("DayOfWeek"))
                                   {
                                       DateTime PatternStartDate = DateTime.Now;
                                       DateTime PatternEndDate = DateTime.Now;

                                       SetPatternStartEndDateProvider(dtCustomeHours, ref PatternStartDate, ref PatternEndDate);

                                       if (PatternStartDate < PatternEndDate)
                                       {
                                           string[] array = new string[] { };

                                           if (dtCustomeHours.Columns.Contains("DayOfWeek"))
                                               array = dtCustomeHours.Rows[0]["DayOfWeek"].ToString().Split(',');
                                           else
                                               array = "Monday".Split(',');

                                           if (array.Length > 0)
                                           {
                                               dtResultOperatoryOfficeHours.AsEnumerable()
                                                   .All(x =>
                                                   {
                                                       var resultOfficeHour = array.AsEnumerable().Where(b => x.Field<object>("Operatory_EHR_ID").ToString().ToUpper() == o["Operatory_EHR_ID"].ToString().ToUpper() && x.Field<object>("WeekDay").ToString().ToUpper() == b.ToString().Trim().ToUpper());

                                                       if (resultOfficeHour.Count() > 0)
                                                       {
                                                           if (Convert.ToDateTime(x["StartTime2"]).ToString() == "01/01/1900 12:00:00 AM")
                                                           {
                                                               if (Convert.ToDateTime(Convert.ToDateTime(x["EndTime1"]).ToShortTimeString()) < Convert.ToDateTime(Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToShortTimeString()))
                                                                   x["EndTime2"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToString("HH:mm"));
                                                               else
                                                                   x["EndTime2"] = x["EndTime1"];

                                                               x["EndTime1"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["StartTime"]).ToString("HH:mm"));
                                                               x["StartTime2"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToString("HH:mm"));
                                                           }
                                                           else if (Convert.ToDateTime(x["StartTime3"]).ToString() == "01/01/1900 12:00:00 AM")
                                                           {
                                                               if (Convert.ToDateTime(Convert.ToDateTime(x["EndTime2"]).ToShortTimeString()) < Convert.ToDateTime(Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToShortTimeString()))
                                                                   x["EndTime3"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToString("HH:mm"));
                                                               else
                                                                   x["EndTime3"] = x["EndTime2"];

                                                               x["EndTime2"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["StartTime"]).ToString("HH:mm"));
                                                               x["StartTime3"] = Convert.ToDateTime("01/01/1900" + " " + Convert.ToDateTime(dtCustomeHours.Rows[0]["EndTime"]).ToString("HH:mm"));
                                                           }
                                                       }
                                                       return true;
                                                   });
                                           }
                                       }
                                   }
                                   return true;
                               });
                       }
                       return true;
                   });
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
            return dtResultOperatoryOfficeHours;
        }

        #endregion

        #region ApptType

        public static DataTable GetClearDentApptTypeData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentApptTypeData;
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

        public static bool Save_ApptType_ClearDent_To_Local(DataTable dtClearDentApptType)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open();  
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtClearDentApptType.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptType;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ApptType;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ApptType_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
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

        #region Patient

        public static DataTable GetClearDentPatientData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentPatientData;
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

        public static DataTable GetCleardentPatientDataOfPatientId(string PatientIds)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentPatientDataOfPatientIds;
                SqlSelect = SqlSelect.Replace("@PatientIds", PatientIds);
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

        public static bool Save_Patient_Cleardent_To_Local_New(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
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
        public static DataTable GetClearDentPatientStatusData(string strPatID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentPatientStatusNew_Existing;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    SqlSelect = SqlSelect + " And fld_auto_intPatId = '" + strPatID + "'";
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

        public static DataTable GetClearDentAppointmentsPatientData(string strPatID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentAppointmentsPatientData;
                if (!string.IsNullOrEmpty(strPatID))
                {
                    SqlSelect = SynchClearDentQRY.GetClearDentAppointmentsPatientDataByPatID;
                    if (ToDate == default(DateTime))
                    {
                        ToDate = Utility.Datetimesetting().AddDays(-7);
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


        public static DataTable GetClearDentPatientIdsData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DateTime ToDate = Utility.LastSyncDateAditServer;
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClerdentPatientIds;
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
        public static DataTable GetClearDentPatientInsuranceData(string PatientID)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentPatientInsuranceData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.AddWithValue("@PatientId", PatientID);
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

        public static DataTable GetClearDentPatientcollect_payment()
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string SqlSelect = SynchClearDentQRY.GetClearDentPatientcollect_payment;
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

        public static DataTable GetClearDentPatient_recall()
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentPatient_recall;
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

        public static DataTable GetClearDentPatientdue_date()
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                SqlSelect = SynchClearDentQRY.GetClearDentPatientdue_date;
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

        public static DataTable GetClearDentPatient_RecallType()
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            DateTime ToDate = dtCurrentDtTime;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentPatient_RecallType;
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

        public static bool Save_Patient_ClearDent_To_Local(DataTable dtClearDentPatient, string InsertTableName, bool bSetDeleted = false)
        {
            bool _successfullstataus = true;
            SynchLocalDAL.Save_Patient_Live_To_LocalPatientDB(dtClearDentPatient, "0", "1");
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //Utility.WriteToSyncLogFile_All("Save Patient to local " + InsertTableName);
                        //Utility.WriteToSyncLogFile_All("Total Records to save " + dtClearDentPatient.Rows.Count.ToString());

                        if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                        {
                            SqlCeCommand.CommandText = "Delete from PatientCompare";
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        //Utility.WriteToSyncLogFile_All("Patient Insert into " + InsertTableName);
                        foreach (DataRow dr in dtClearDentPatient.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (dr["Primary_Insurance_CompanyName"].ToString() == "" && dr["Secondary_Insurance_CompanyName"].ToString() == "")
                            {
                                decimal curPatientcollect_payment = 0;
                                dr["used_benefit"] = curPatientcollect_payment.ToString();
                                dr["remaining_benefit"] = curPatientcollect_payment.ToString();
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                ExecuteQuery(InsertTableName, dr, SqlCeCommand);
                            }
                        }


                        //if (bSetDeleted)
                        //{
                        //    string PatientEHRIDs = string.Join("','", dtClearDentPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));
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
                            IEnumerable<string> PatientEHRIDs = dtClearDentPatient.AsEnumerable().Where(sid => sid["Service_Install_Id"].ToString() == "1").Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                            if (PatientEHRIDs != null && PatientEHRIDs.Any())
                            {

                                DataTable dtLocalPatient = SynchLocalDAL.GetLocalPatientData("1");
                                IEnumerable<string> LocalEHRIDs = dtLocalPatient.AsEnumerable()
                                    .Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                                if (LocalEHRIDs != null && LocalEHRIDs.Any())
                                {

                                    string DeletedEHRIDs = string.Join("','", LocalEHRIDs.Except(PatientEHRIDs).ToList());
                                    if (DeletedEHRIDs != string.Empty)
                                    {
                                        DeletedEHRIDs = "'" + DeletedEHRIDs + "'";
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        SqlCeSelect = SynchLocalQRY.Delete_Patient_By_PatientEHRIDs;
                                        SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", DeletedEHRIDs);
                                        SqlCeCommand.CommandText = SqlCeSelect;
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                    }

                    #region Get Records from PatientCompareTAble
                    //Utility.WriteToSyncLogFile_All("Patient Insert into " + InsertTableName);
                    if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                    {
                        DataTable dtPatientCompareRec = new DataTable();
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        SqlCeSelect = SynchLocalQRY.PatientCompareQuery;

                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", "1");
                            DataTable SqlCeDt = null;
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

                tmpBirthDate = Utility.CheckValidDatetime(dr["birth_date"].ToString().Trim());

                if (tmpBirthDate != "")
                {
                    tmpBirthDate = Convert.ToDateTime(tmpBirthDate).ToString("MM/dd/yyyy");
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
                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", dr["ThirtyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("SixtyDay", dr["SixtyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("NinetyDay", dr["NinetyDay"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Over90", dr["Over90"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("CurrentBal", dr["CurrentBal"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dr["FirstVisit_Date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dr["LastVisit_Date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dr["Primary_Insurance"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dr["Primary_Insurance_CompanyName"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dr["Secondary_Insurance"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dr["Secondary_Insurance_CompanyName"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Guar_ID", dr["Guar_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dr["Pri_Provider_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dr["Sec_Provider_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dr["ReceiveSms"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", dr["ReceiveEmail"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dr["nextvisit_date"].ToString().Trim()));
                SqlCeCommand.Parameters.AddWithValue("due_date", dr["due_date"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", dr["remaining_benefit"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("used_benefit", dr["used_benefit"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("collect_payment", dr["collect_payment"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", dr["Secondary_Ins_Subscriber_ID"].ToString().Trim());
                SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", dr["Primary_Ins_Subscriber_ID"].ToString().Trim());
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


        public static string Push_Local_To_LiveDatabase_Patient_Async(string JsonString, string TableName)
        {
            string _successfullstataus = string.Empty;

            try
            {
                // JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    if (Utility.isSqlServer)
                    {
                        SqlConnection conn = null;
                        SqlCommand SqlExCommand = null;
                        CommonDB.LocalConnectionServer_SqlServer(ref conn);
                        //   SqlTransaction SqlExTransaction;
                        if (conn.State == ConnectionState.Closed) conn.Open();

                        //     SqlExTransaction = conn.BeginTransaction();

                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = string.Empty;

                        var ResMessagePatient = JsonConvert.DeserializeObject<Pull_PatientBO>(response.Content);
                        try
                        {
                            if (ResMessagePatient.data != null)
                            {
                                foreach (var item in ResMessagePatient.data)
                                {
                                    // UpdateLocalTableWebID(TableName, item.patient_ehr_id.ToString(), item._id.ToString());  
                                    sqlSelect = string.Empty;
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                                    // SqlExCommand.Transaction = SqlExTransaction;
                                    SqlExCommand.CommandText = SynchLocalQRY.Update_Patient_Web_Id;
                                    SqlExCommand.Parameters.Clear();
                                    SqlExCommand.Parameters.AddWithValue("EHR_ID", Convert.ToInt64(item.patient_ehr_id.ToString()));
                                    SqlExCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                    SqlExCommand.ExecuteNonQuery();
                                }
                                _successfullstataus = "Success";
                            }
                            else
                            {
                                _successfullstataus = ResMessagePatient.message.ToString();
                            }
                            // SqlExTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            //  SqlExTransaction.Rollback();
                            throw;
                        }
                    }
                    else
                    {
                        using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            //     SqlCetx = conn.BeginTransaction();
                            DateTime dtCurrentDtTime = Utility.Datetimesetting();
                            string SqlCeSelect = string.Empty;
                            var ResMessagePatient = JsonConvert.DeserializeObject<Pull_PatientBO>(response.Content);
                            try
                            {
                                if (ResMessagePatient.data != null)
                                {
                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                    {
                                        SqlCeCommand.CommandType = CommandType.Text;
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Web_Id;
                                        foreach (var item in ResMessagePatient.data)
                                        {
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.Parameters.AddWithValue("EHR_ID", Convert.ToInt64(item.patient_ehr_id.ToString()));
                                            SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                            SqlCeCommand.ExecuteNonQuery();
                                        }
                                    }
                                    _successfullstataus = "Success";
                                }
                                else
                                {
                                    _successfullstataus = ResMessagePatient.message.ToString();
                                }
                                //SqlCetx.Commit();
                            }
                            catch (Exception)
                            {
                                // SqlCetx.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;

                throw ex;
            }

            return _successfullstataus;
        }

        #endregion

        #region RecallType

        public static DataTable GetClearDentRecallTypeData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentRecallTypeData;
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

        public static bool Save_RecallType_ClearDent_To_Local(DataTable dtClearDentRecallType)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //  SqlCetx = conn.BeginTransaction();
                try
                {
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtClearDentRecallType.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
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

        #region User

        public static string GetClearDentUserLoginId()
        {
            string UserLoginId = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetEHRCleardentUserId;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                DataTable SqlDt = new DataTable();
                SqlDa.Fill(SqlDt);

                if (SqlDt != null && SqlDt.Rows.Count > 0)
                {
                    UserLoginId = SqlDt.Rows[0][0].ToString();
                }

                return UserLoginId;
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

        public static DataTable GetClearDentUserData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentUserData;
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

        public static bool Save_User_ClearDent_To_Local(DataTable dtCleardentUser)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //  SqlCetx = conn.BeginTransaction();
                try
                {
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtCleardentUser.Rows)
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

        #region ApptStatus

        public static DataTable GetClearDentApptStatusData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentApptStatusData;
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

        public static bool Save_ApptStatus_ClearDent_To_Local(DataTable dtClearDentApptStatus)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtClearDentApptStatus.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.Add("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //  SqlCetx.Commit();
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

        #region Speciality

        public static bool Save_Speciality_ClearDent_To_Local(DataTable dtClearDentSpeciality)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open();   
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtClearDentSpeciality.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                            {
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Speciality;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Speciality_Name", dr["provider_speciality"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
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

        #region SqlServer

        public static bool Save_Appointment_ClearDent_To_Local_SqlServer(DataTable dtClearDentAppointment)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            // SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //  SqlCetx = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open(); 
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                bool is_ehr_updated = false;
                string Apptdate = string.Empty;
                string ApptTime = string.Empty;
                string Mobile_Contact = string.Empty;
                string Email = string.Empty;
                string Home_Contact = string.Empty;
                string Address = string.Empty;
                string City = string.Empty;
                string State = string.Empty;
                string Zipcode = string.Empty;
                string patient_first_name = string.Empty;
                string patient_last_name = string.Empty;
                string patient_mi_name = string.Empty;
                string AppointmentStatus = string.Empty;

                foreach (DataRow dr in dtClearDentAppointment.Rows)
                {
                    is_ehr_updated = false;

                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }

                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;
                                is_ehr_updated = false;
                                break;
                            case 5:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Contact;
                                is_ehr_updated = true;
                                break;
                            case 4:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_EHR_ID;
                                is_ehr_updated = true;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                break;
                        }

                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                        {
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            string[] patientinfo = dr["patient_name"].ToString().Split(',');
                            patient_last_name = patientinfo[0].ToString().Trim();
                            string[] patienfirstMI = patientinfo[1].ToString().Trim().Split(' ');
                            if (patienfirstMI.Length == 2)
                            {
                                patient_first_name = patienfirstMI[0].ToString().Trim();
                                patient_mi_name = patienfirstMI[1].ToString().Trim();
                            }
                            else
                            {
                                patient_first_name = patienfirstMI[0].ToString().Trim();
                                patient_mi_name = string.Empty;
                            }

                            Apptdate = Convert.ToDateTime(dr["appointment_date"].ToString()).ToString("dd/MM/yyyy");
                            ApptTime = Convert.ToInt32(dr["start_hour"].ToString()).ToString("00") + ":" + Convert.ToInt32(dr["start_minute"].ToString()).ToString("00");

                            Mobile_Contact = string.Empty;
                            Email = string.Empty;
                            Home_Contact = string.Empty;
                            Address = string.Empty;
                            City = string.Empty;
                            State = string.Empty;
                            Zipcode = string.Empty;

                            if (dr["patId"].ToString() == "0" || dr["patId"].ToString() == string.Empty)
                            {

                                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                                {
                                    Mobile_Contact = Utility.ConvertContactNumber(dr["Phone"].ToString().Trim());
                                    Email = "";
                                }
                                else
                                {
                                    string MobileEmail = dr["notetext"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace("\n", "");
                                    Mobile_Contact = string.Empty;
                                    Email = string.Empty;

                                    if (MobileEmail != string.Empty)
                                    {
                                        Mobile_Contact = MobileEmail.Substring(0, 10);
                                        Email = MobileEmail.Substring(10, MobileEmail.Length - 10);

                                        try
                                        {
                                            double isMobile = Convert.ToDouble(Mobile_Contact);
                                        }
                                        catch (FormatException)
                                        {
                                            Mobile_Contact = string.Empty;
                                            Email = MobileEmail.ToString();
                                        }

                                    }
                                    else
                                    {
                                        Mobile_Contact = dr["patMobile"].ToString();
                                        Email = dr["patEmail"].ToString();
                                    }
                                }

                                Home_Contact = dr["patient_phone"].ToString().Trim();
                                Address = dr["street1"].ToString().Trim();
                                City = dr["city"].ToString().Trim();
                                State = dr["state"].ToString().Trim();
                                Zipcode = dr["zipcode"].ToString().Trim();
                            }
                            else
                            {
                                Mobile_Contact = dr["patMobile"].ToString();
                                Email = dr["patEmail"].ToString();
                                Home_Contact = dr["patHomephone"].ToString().Trim();
                                Address = dr["patAddress"].ToString().Trim();
                                City = dr["patCity"].ToString().Trim();
                                State = dr["patState"].ToString().Trim();
                                Zipcode = dr["patZipcode"].ToString().Trim();
                            }

                            DateTime ApptDateTime = DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            DateTime ApptEndDateTime = ApptDateTime.AddMinutes(Convert.ToInt32(dr["length"].ToString().Trim()));

                            string birthdate = string.Empty;
                            if (!string.IsNullOrEmpty(dr["birth_date"].ToString()))
                            {
                                birthdate = dr["birth_date"].ToString();
                            }

                            if (dr["appointment_status_ehr_key"].ToString().Trim() == "-106")
                            {
                                AppointmentStatus = "completed";
                            }
                            else if (dr["appointment_status_ehr_key"].ToString().Trim() == "0")
                            {
                                AppointmentStatus = "none";
                            }
                            else
                            {
                                AppointmentStatus = dr["Appointment_Status"].ToString().Trim();
                            }


                            int commentlen = 1999;
                            if (dr["comment"].ToString().Trim().Length < commentlen)
                            {
                                commentlen = dr["comment"].ToString().Trim().Length;
                            }

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["appointment_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", string.Empty);
                            SqlCeCommand.Parameters.AddWithValue("Last_Name", patient_last_name.Trim());
                            SqlCeCommand.Parameters.AddWithValue("First_Name", patient_first_name.Trim());
                            SqlCeCommand.Parameters.AddWithValue("MI", patient_mi_name.Trim());
                            SqlCeCommand.Parameters.AddWithValue("Home_Contact", Utility.ConvertContactNumber(Home_Contact.ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", Utility.ConvertContactNumber(Mobile_Contact.Trim()));
                            SqlCeCommand.Parameters.AddWithValue("Email", Email.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Address", Address.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("City", City.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ST", State.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Zip", Zipcode.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["op_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["op_title"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["provider_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["provider_last_name"].ToString().Trim() + " " + dr["provider_first_name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType_Name"].ToString().Trim().Replace(",", " "));
                            SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString().Trim().Substring(0, commentlen));
                            SqlCeCommand.Parameters.AddWithValue("birth_date", birthdate);
                            SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", ApptDateTime.ToString());
                            SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", ApptEndDateTime.ToString());
                            SqlCeCommand.Parameters.AddWithValue("Status", "1");
                            SqlCeCommand.Parameters.AddWithValue("Patient_Status", "new");
                            SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Appointment_Status", AppointmentStatus);
                            SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", "");
                            SqlCeCommand.Parameters.AddWithValue("confirmed_status", "");
                            SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
                            SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
                            SqlCeCommand.Parameters.AddWithValue("Is_Appt", "EHR");
                            SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", is_ehr_updated);
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                            SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                            SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                            SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Appt_LocalDB_ID", dr["Appt_LocalDB_ID"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patId"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                            SqlCeCommand.Parameters.AddWithValue("ProcedureDesc", "");
                            SqlCeCommand.Parameters.AddWithValue("ProcedureCode", "");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                }
                // SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                //  SqlCetx.Rollback();
                // throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_OperatoryEvent_ClearDent_To_Local_SqlServer(DataTable dtClearDentOperatoryEvent)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //  SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            // SqlCetx = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open(); 
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                bool is_ehr_updated = false;

                foreach (DataRow dr in dtClearDentOperatoryEvent.Rows)
                {
                    is_ehr_updated = false;

                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }

                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_OperatoryEventData;
                                is_ehr_updated = false;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryEventData;
                                is_ehr_updated = true;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_OperatoryEventData;
                                break;
                        }

                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                        {
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["event_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            DateTime OE_Date = Convert.ToDateTime(dr["event_date"].ToString());

                            int OE_StartHour = Convert.ToInt32(dr["start_time"].ToString()) / 12;
                            int OE_StartMin = (Convert.ToInt32(dr["start_time"].ToString()) % 12) * 5;

                            int OE_EndHour = Convert.ToInt32(dr["end_time"].ToString()) / 12;
                            int OE_EndMin = (Convert.ToInt32(dr["end_time"].ToString()) % 12) * 5;


                            //DateTime StartTime = Convert.ToDateTime(OE_Date.ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(Convert.ToDateTime(OE_StartHour.ToString()).ToString("HH") + ":" + Convert.ToDateTime(OE_StartMin.ToString()).ToString("mm")).ToString("HH:mm"));
                            //DateTime EndTime = Convert.ToDateTime(OE_Date.ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(Convert.ToDateTime(OE_EndHour.ToString()).ToString("HH") + ":" + Convert.ToDateTime(OE_EndMin.ToString()).ToString("mm")).ToString("HH: mm"));

                            DateTime StartTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToString("MM/dd/yyyy") + " " + OE_StartHour.ToString() + ":" + OE_StartMin.ToString()).ToString("MM/dd/yyyy HH:mm"));
                            DateTime EndTime = Convert.ToDateTime(Convert.ToDateTime(Convert.ToDateTime(OE_Date).ToString("MM/dd/yyyy") + " " + OE_EndHour.ToString() + ":" + OE_EndMin.ToString()).ToString("MM/dd/yyyy HH:mm"));



                            int commentlen = 1999;
                            if (dr["note"].ToString().Trim().Length < commentlen)
                            {
                                commentlen = dr["note"].ToString().Trim().Length;
                            }

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("OE_EHR_ID", dr["event_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("OE_Web_ID", string.Empty);
                            SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["operatory_id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("comment", dr["note"].ToString().Trim().Substring(0, commentlen));
                            SqlCeCommand.Parameters.AddWithValue("StartTime", StartTime.ToString());
                            SqlCeCommand.Parameters.AddWithValue("EndTime", EndTime.ToString());
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
                            SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                            SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                            SqlCeCommand.Parameters.AddWithValue("Allow_Book_Appt", Convert.ToBoolean(false));
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
                // throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Save_Provider_ClearDent_To_Local_SqlServer(DataTable dtClearDentProvider)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //  SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //  SqlCetx = conn.BeginTransaction();
            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open(); 
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtClearDentProvider.Rows)
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
                        SqlCeCommand.Parameters.AddWithValue("MI", dr["MI"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("gender", "");
                        SqlCeCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString().Trim().Substring(12, dr["provider_speciality"].ToString().Trim().Length - 12));
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
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
            return _successfullstataus;
        }

        public static bool Save_Speciality_ClearDent_To_Local_SqlServer(DataTable dtClearDentSpeciality)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //  SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //  SqlCetx = conn.BeginTransaction();
            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open();   
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtClearDentSpeciality.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                    {
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Speciality;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Speciality_Name", dr["provider_speciality"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
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
            return _successfullstataus;
        }

        public static bool Save_Operatory_ClearDent_To_Local_SqlServer(DataTable dtClearDentOperatory)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            // SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            // SqlCetx = conn.BeginTransaction();
            try
            {
                //if (conn.State == ConnectionState.Closed) conn.Open();    
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtClearDentOperatory.Rows)
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
                        }
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }

                // SqlCetx.Commit();
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
            return _successfullstataus;
        }

        public static bool Save_Patient_ClearDent_To_Local_SqlServer(DataTable dtClearDentPatient)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //  SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //  SqlCetx = conn.BeginTransaction();
            try
            {
                // if (conn.State == ConnectionState.Closed) conn.Open();   
                string sqlSelect = string.Empty;
                string MaritalStatus = string.Empty;
                string Status = string.Empty;
                string tmpBirthDate = string.Empty;

                string tmpReceive_Sms_Email = string.Empty;
                int tmpprivacy_flags = 0;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtClearDentPatient.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                    {

                        tmpReceive_Sms_Email = "Y";
                        tmpprivacy_flags = Convert.ToInt32(dr["privacy_flags"].ToString());
                        if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
                        {
                            tmpReceive_Sms_Email = "N";
                        }

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

                        if (Status == "" || Status == "1")
                        { Status = "A"; }
                        else
                        { Status = "I"; }


                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
                                break;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("First_name", dr["First_name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_name", dr["Last_name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Middle_Name", dr["Middle_Name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Salutation", dr["Salutation"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("preferred_name", dr["preferred_name"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Status", Status);
                        SqlCeCommand.Parameters.AddWithValue("Sex", "Unknown");
                        SqlCeCommand.Parameters.AddWithValue("MaritalStatus", "Single");
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
                        SqlCeCommand.Parameters.AddWithValue("CurrentBal", "");
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
                        SqlCeCommand.Parameters.AddWithValue("remaining_benefit", "");
                        SqlCeCommand.Parameters.AddWithValue("collect_payment", "");
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", dr["Secondary_Ins_Subscriber_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", dr["Primary_Ins_Subscriber_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }

                //  SqlCetx.Commit();
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
            return _successfullstataus;
        }

        public static bool Save_ApptType_ClearDent_To_Local_SqlServer(DataTable dtClearDentApptType)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //  SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            // SqlCetx = conn.BeginTransaction();
            try
            {
                //  if (conn.State == ConnectionState.Closed) conn.Open();  
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtClearDentApptType.Rows)
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
                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptType;
                                break;
                            case 2:
                                SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType;
                                break;
                            case 3:
                                SqlCeCommand.CommandText = SynchLocalQRY.Delete_ApptType;
                                break;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("ApptType_Web_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString().Trim().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                //  SqlCetx.Commit();

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
            return _successfullstataus;
        }

        public static bool Save_ApptStatus_ClearDent_To_Local_SqlServer(DataTable dtClearDentApptStatus)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            // SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            // SqlCetx = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtClearDentApptStatus.Rows)
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
                        SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                // SqlCetx.Commit();
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
            return _successfullstataus;
        }

        public static bool Save_RecallType_ClearDent_To_Local_SqlServer(DataTable dtClearDentRecallType)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //  SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            // SqlCetx = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                foreach (DataRow dr in dtClearDentRecallType.Rows)
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
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", "0");
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                // SqlCetx.Commit();
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
            return _successfullstataus;
        }

        public static bool Update_DeletedAppointment_ClearDent_To_Local_SqlServer(DataTable dtClearDentDeletedAppointment)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            //  SqlTransaction SqlCetx;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //  SqlCetx = conn.BeginTransaction();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                string AppointmentStatus = string.Empty;
                foreach (DataRow dr in dtClearDentDeletedAppointment.Rows)
                {
                    if (dr["InsUptDlt"].ToString() == "")
                    {
                        dr["InsUptDlt"] = "0";
                    }
                    if (dr["InsUptDlt"].ToString() == "2")
                    {
                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appointment_id"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Operatory_Name", "");
                        SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", "");
                        SqlCeCommand.Parameters.AddWithValue("Provider_Name", "");
                        SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        SqlCeCommand.ExecuteNonQuery();
                    }

                }
                //  SqlCetx.Commit();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                //  SqlCetx.Rollback();
                // throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        #endregion

        //#region  Holidays

        //public static DataTable GetClearDentHolidaysData()
        //{
        //    SqlConnection conn = null;
        //    SqlCommand SqlCommand = new SqlCommand();
        //    SqlDataAdapter SqlDa = null;
        //    CommonDB.ClearDentSQLConnectionServer(ref conn);

        //    DateTime ToDate = Utility.LastSyncDateAditServer;
        //    try
        //    {
        //        //  MySqlCommand.CommandTimeout = 120;
        //        SqlCommand.CommandTimeout = 200;
        //        if (conn.State == ConnectionState.Closed) conn.Open();
        //        string SqlSelect = SynchClearDentQRY.GetClearDentHolidayData;
        //        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        //        SqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
        //        SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
        //        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        //        DataTable SqlDt = new DataTable();
        //        SqlDa.Fill(SqlDt);
        //        SqlDt.DefaultView.RowFilter = "closed_flag in (2,3,4)";
        //        return SqlDt.DefaultView.ToTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open) conn.Close();
        //    }
        //}

        ////public static DataTable GetClearDentOperatoryHolidaysData(DataTable dtOperatory)
        ////{
        ////    SqlConnection conn = null;
        ////    CommonDB.ClearDentSQLConnectionServer(ref conn);

        ////    DateTime ToDate = Utility.LastSyncDateAditServer;
        ////    try
        ////    {
        ////        DataTable SqlDt = new DataTable();
        ////        foreach (DataRow dr in dtOperatory.Rows)
        ////        {
        ////            SqlCommand SqlCommand = new SqlCommand();
        ////            SqlDataAdapter SqlDa = null;
        ////            //  MySqlCommand.CommandTimeout = 120;
        ////            SqlCommand.CommandTimeout = 200;
        ////            if (conn.State == ConnectionState.Closed) conn.Open();
        ////            string SqlSelect = SynchClearDentQRY.GetClearDentOperatoryHolidaysData;
        ////            CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
        ////            SqlCommand.Parameters.AddWithValue("@H_Operatory_EHR_ID", dr["operatory_ehr_id"].ToString());
        ////            SqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = ToDate.ToString("yyyy/MM/dd");
        ////            SqlCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
        ////            CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
        ////            DataTable dt = new DataTable();
        ////            SqlDa.Fill(dt);
        ////            if (SqlDt.Rows.Count > 0)
        ////            {
        ////                SqlDt.Merge(dt);
        ////            }
        ////            else
        ////            {
        ////                SqlDt = dt;
        ////            }

        ////        }
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

        //public static bool Save_Holidays_ClearDent_To_Local(DataTable dtEaglesoftHoliday)
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
        //        string AppointmentStatus = string.Empty;
        //        foreach (DataRow dr in dtEaglesoftHoliday.Rows)
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
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_HolidayData;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ClearDent_HolidayData;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ClearDent_HolidayData;
        //                        break;
        //                }

        //                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
        //                {
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("SchedDate", dr["Sched_exception_date"].ToString().Trim());
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //                else
        //                {

        //                    int commentlen = 1999;
        //                    if (dr["practice_name"].ToString().Trim().Length < commentlen)
        //                    {
        //                        commentlen = dr["practice_name"].ToString().Trim().Length;
        //                    }
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", "");
        //                    SqlCeCommand.Parameters.AddWithValue("H_Web_ID", "");
        //                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", "");
        //                    SqlCeCommand.Parameters.AddWithValue("comment", dr["practice_name"].ToString().Trim().Substring(0, commentlen));
        //                    SqlCeCommand.Parameters.AddWithValue("SchedDate", Utility.CheckValidDatetime(dr["Sched_exception_date"].ToString()));
        //                    SqlCeCommand.Parameters.AddWithValue("StartTime_1", dr["start_time1"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("EndTime_1", dr["end_time1"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("StartTime_2", dr["start_time2"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("EndTime_2", dr["end_time2"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("StartTime_3", dr["start_time3"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("EndTime_3", dr["end_time3"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
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

        //public static bool Save_Opeatory_Holidays_ClearDent_To_Local(DataTable dtEaglesoftHoliday)
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
        //        string AppointmentStatus = string.Empty;
        //        foreach (DataRow dr in dtEaglesoftHoliday.Rows)
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
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_HolidayData;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 2:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ClearDent_Operatory_HolidayData;
        //                        is_ehr_updated = true;
        //                        break;
        //                    case 3:
        //                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ClearDent_Operatory_HolidayData;
        //                        break;
        //                }

        //                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
        //                {
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", dr["op_id"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("SchedDate", dr["Sched_exception_date"].ToString().Trim());
        //                    SqlCeCommand.ExecuteNonQuery();
        //                }
        //                else
        //                {
        //                    int commentlen = 1999;
        //                    if (dr["op_title"].ToString().Trim().Length < commentlen)
        //                    {
        //                        commentlen = dr["op_title"].ToString().Trim().Length;
        //                    }
        //                    SqlCeCommand.Parameters.Clear();
        //                    SqlCeCommand.Parameters.AddWithValue("H_EHR_ID", "");
        //                    SqlCeCommand.Parameters.AddWithValue("H_Web_ID", "");
        //                    SqlCeCommand.Parameters.AddWithValue("H_Operatory_EHR_ID", dr["op_id"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("comment", dr["op_title"].ToString().Trim().Substring(0, commentlen));
        //                    SqlCeCommand.Parameters.AddWithValue("SchedDate", Utility.CheckValidDatetime(dr["Sched_exception_date"].ToString()));
        //                    SqlCeCommand.Parameters.AddWithValue("StartTime_1", dr["start_time1"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("EndTime_1", dr["end_time1"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("StartTime_2", dr["start_time2"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("EndTime_2", dr["end_time2"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("StartTime_3", dr["start_time3"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("EndTime_3", dr["end_time3"].ToString());
        //                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
        //                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", Utility.GetCurrentDatetimestring());
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


        //#endregion

        #region Create Appointment

        public static bool Update_Appointment_EHR_Id_Web_Book_Appointment(string tmpAppt_EHR_id, string tmpAppt_Web_id, string _filename_Appointment = "", string _EHRLogdirectory_Appointment = "")
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_EHR_ID;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", tmpAppt_EHR_id);
                        SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", tmpAppt_Web_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                        if (_filename_Appointment != "" && _EHRLogdirectory_Appointment != "")
                        {
                            Utility.WriteSyncPullLog(_filename_Appointment, _EHRLogdirectory_Appointment, "Update Appointment EHR Id Web Book Appointment for " + "Appt_EHR_ID : " + tmpAppt_EHR_id + " and Appt_Web_ID : " + tmpAppt_Web_id);
                        }
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    // SqlCetx.Commit();
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

        public static int Save_Patient_Local_To_ClearDent(string LastName, string FirstName, string MiddleName, string Mobile, string Email, string PriProv, string DateFirstVisit, long tmpPatient_Gur_id, string Birth_Date)
        {
            int PatientId = 0;
            long FamilyId = 0;
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
            }
            if (LastName.Length == 0)
            {
                LastName += "NA";
            }

            SqlConnection conn = null;
            //MySqlCommand MySqlCommand = new MySqlCommand();
            SqlCommand SqlCommand = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            string patBirthDate = "";
            try
            {
                patBirthDate = Convert.ToDateTime(Birth_Date.ToString()).ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {
                patBirthDate = "";
            }

            if (conn.State == ConnectionState.Closed) conn.Open();

            try
            {


                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                if (patBirthDate == "")
                {
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatientDetails;

                }
                else
                {
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatientDetails_With_Birthdate;
                }
                // SqlCommand.CommandText = SynchClearDentQRY.InsertPatientDetails;
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("@lastname", LastName);
                SqlCommand.Parameters.AddWithValue("@firstname", FirstName);
                SqlCommand.Parameters.AddWithValue("@mi", MiddleName);
                if (PriProv == "" || PriProv == "0")
                {
                    SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", "(Select top 1 fld_auto_shtPrId from [tbl_Pr] where fld_bytstatus = 1 and fld_bytPrTypeId = 1 order by fld_strname)");
                }
                else
                {
                    SqlCommand.Parameters.AddWithValue("@provid1", PriProv);
                }
                SqlCommand.Parameters.AddWithValue("@emailaddr", Email);
                SqlCommand.Parameters.AddWithValue("@pager", Mobile);
                if (patBirthDate != "")
                {
                    SqlCommand.Parameters.AddWithValue("@Birth_Date", patBirthDate);
                }
                SqlCommand.ExecuteNonQuery();
                string QryIdentity = "Select max(fld_auto_intPatId) as newId from tbl_PatInfo";//"Select @@Identity as newId from patient";
                SqlCommand.CommandText = QryIdentity;
                SqlCommand.CommandType = CommandType.Text;
                SqlCommand.Connection = conn;
                PatientId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                if (tmpPatient_Gur_id == 0)
                {
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatientGuarantorID;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@fld_intHeadPatId", PatientId);
                    SqlCommand.ExecuteNonQuery();
                    QryIdentity = "Select max([fld_auto_intFamId]) as newId from [tbl_PatFam]";//"Select @@Identity as newId from patient";
                    SqlCommand.CommandText = QryIdentity;
                    SqlCommand.CommandType = CommandType.Text;
                    SqlCommand.Connection = conn;
                    FamilyId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                }
                else
                {
                    FamilyId = tmpPatient_Gur_id;
                }
                SqlCommand.CommandText = SynchClearDentQRY.UpdatePatientGuarantorID;
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("@famid", FamilyId);
                SqlCommand.Parameters.AddWithValue("@patid", PatientId);
                SqlCommand.ExecuteNonQuery();

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_PatInfoAccessRecord;
                SqlCommand.Parameters.Clear();

                SqlCommand.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                SqlCommand.Parameters.AddWithValue("@Description", "Patient Created by Adit");
                SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                SqlCommand.Parameters.AddWithValue("@PatientEHRId", PatientId);
                SqlCommand.ExecuteScalar();
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

        public static int Save_Appointment_Local_To_ClearDent(string patid, int length, string opid, string provid, DateTime StartTime, DateTime EndTime,
                                                                 string createdate, string appttypeid, string PatientName, string comment, string TreatmentCodes, int appointmentstatuskey)
        {
            int Appointment_Id = 0;
            Int64 TreatmentPlanId = 0;
            int treatmetplan_key = 0;
            string procid;
            Int64 TreatmentPlanProcedureId = 0;
            SqlConnection conn = null;
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
            }
            //MySqlCommand MySqlCommand = new MySqlCommand();
            SqlCommand SqlCommand = null;
            SqlDataAdapter SqlDa = null;
            SqlDataReader read = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = SynchClearDentQRY.InsertAppointmentDetails;
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("@patid", patid);
                SqlCommand.Parameters.AddWithValue("@provid", provid);
                SqlCommand.Parameters.AddWithValue("@opid", opid);
                SqlCommand.Parameters.AddWithValue("@appttype", appttypeid);
                SqlCommand.Parameters.AddWithValue("@StartTime", Convert.ToDateTime(StartTime));
                SqlCommand.Parameters.AddWithValue("@EndTime", Convert.ToDateTime(EndTime));
                SqlCommand.Parameters.AddWithValue("@fld_strNotes", comment);
                // SqlCommand.Parameters.AddWithValue("@Confirmed", Confirmed);
                SqlCommand.Parameters.AddWithValue("@createdate", Convert.ToDateTime(createdate));
                SqlCommand.Parameters.AddWithValue("@apptstatus", Convert.ToInt16(appointmentstatuskey));
                // SqlCommand.Parameters.AddWithValue("IsNewPatient", IsNewPatient);
                SqlCommand.ExecuteNonQuery();

                string QryIdentity = "Select max(fld_auto_intAppId) as newId from tbl_SchApp";
                SqlCommand.CommandText = QryIdentity;
                SqlCommand.CommandType = CommandType.Text;
                SqlCommand.Connection = conn;
                Appointment_Id = Convert.ToInt32(SqlCommand.ExecuteScalar());

                #region Saving TreatmentPlans and Procedure Mapping to Appointments
                //30_9,31_7,25_0
                if (TreatmentCodes != null && TreatmentCodes.Length > 0)
                {
                    foreach (var treatcode in TreatmentCodes.Split(','))
                    {
                        procid = treatcode.Substring(0, treatcode.IndexOf('_'));
                        treatmetplan_key = Convert.ToInt32(treatcode.Substring(treatcode.LastIndexOf('_') + 1));
                        TreatmentPlanProcedureId = 0;

                        if (treatmetplan_key == 0)//&& TreatmentPlanId == 0)
                        {
                            #region If TretmentPlan Key is zero then Insert Records in tbl_TrPlan, tbl_TrPlanProc & tbl_SchApp_TrPlanProc
                            if (TreatmentPlanId == 0)
                            {
                                try
                                {
                                    SqlCommand.CommandText = SynchClearDentQRY.InsertTreatmentPlan;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@PatID", patid);
                                    SqlCommand.Parameters.AddWithValue("@ProvID", provid);
                                    SqlCommand.ExecuteNonQuery();

                                    SqlCommand.CommandText = SynchClearDentQRY.GetTreatmentPlanKey;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@PatID", patid);
                                    SqlCommand.Parameters.AddWithValue("@ProvID", provid);
                                    //read = SqlCommand.ExecuteReader();
                                    //TreatmentPlanId = Convert.ToInt64(read[0]);
                                    TreatmentPlanId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                }
                                catch (Exception ex)
                                {
                                    Utility.WriteToErrorLogFromAll("[TreatmentPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                                }
                            }

                            try
                            {
                                SqlCommand.CommandText = SynchClearDentQRY.InsertProcedureForNewPlan;
                                SqlCommand.Parameters.Clear();
                                //SqlCommand.Parameters.AddWithValue("@PatID", patid);
                                SqlCommand.Parameters.AddWithValue("@ProvID", provid);
                                SqlCommand.Parameters.AddWithValue("@ProcCode", procid);
                                SqlCommand.Parameters.AddWithValue("@fld_intTransId", TreatmentPlanId);
                                SqlCommand.ExecuteNonQuery();

                                SqlCommand.CommandText = SynchClearDentQRY.GetTreatmentPlanProcedureLog;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@TreatmentPlanId", TreatmentPlanId);
                                SqlCommand.Parameters.AddWithValue("@ProcCode", procid);

                                TreatmentPlanProcedureId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("[ProcedureForPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }

                            try
                            {
                                SqlCommand.CommandText = SynchClearDentQRY.InsertProcedureLog;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@fld_intTrPlanProcId", TreatmentPlanProcedureId);
                                SqlCommand.Parameters.AddWithValue("@AptID", Appointment_Id);

                                SqlCommand.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("[ProcedureForPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }

                            #endregion
                        }
                        #region Commented Code
                        //else if (treatmetplan_key == 0 && TreatmentPlanId > 0)
                        //{
                        //    #region IF Tretment Plan key is > 0 then Insert records in tbl_SchApp_TrPlanProc
                        //    try
                        //    {
                        //        SqlCommand.CommandText = SynchClearDentQRY.InsertProcedureForNewPlan;
                        //        SqlCommand.Parameters.Clear();
                        //        //SqlCommand.Parameters.AddWithValue("@PatID", patid);
                        //        SqlCommand.Parameters.AddWithValue("@ProvID", provid);
                        //        SqlCommand.Parameters.AddWithValue("@ProcCode", procid);
                        //        SqlCommand.Parameters.AddWithValue("@fld_intTransId", TreatmentPlanId);
                        //        SqlCommand.ExecuteNonQuery();

                        //        SqlCommand.CommandText = SynchClearDentQRY.GetTreatmentPlanProcedureLog;
                        //        SqlCommand.Parameters.Clear();
                        //        SqlCommand.Parameters.AddWithValue("@TreatmentPlanId", TreatmentPlanId);
                        //        SqlCommand.Parameters.AddWithValue("@ProcCode", procid);

                        //        TreatmentPlanProcedureId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        //        if (TreatmentCodes != null && TreatmentCodes.Length > 0)
                        //        {
                        //            SqlCommand.CommandText = SynchClearDentQRY.InsertAppointmentTreatmentPlan;
                        //            SqlCommand.Parameters.Clear();
                        //            SqlCommand.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                        //            SqlCommand.Parameters.AddWithValue("@PatientId", patid);
                        //            SqlCommand.Parameters.AddWithValue("@ProviderId", provid);
                        //            TreatmentPlanId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        //            for (int i = 0; i <= TreatmentCodes.Split(',').Length - 1; i++)
                        //            {
                        //                try
                        //                {
                        //                    SqlCommand.CommandText = SynchClearDentQRY.InsertAppointmentTreatmentPlanProc;
                        //                    SqlCommand.Parameters.Clear();
                        //                    SqlCommand.Parameters.AddWithValue("@TreatmentPlanId", TreatmentPlanId);
                        //                    SqlCommand.Parameters.AddWithValue("@ServiceDate", DateTime.Now);
                        //                    SqlCommand.Parameters.AddWithValue("@ProviderId", provid);//treatcode.Substring(0, treatcode.IndexOf('_')
                        //                    SqlCommand.Parameters.AddWithValue("@ProcCode", TreatmentCodes.Split(',')[i].ToString().Substring(0, treatcode.IndexOf('_')));
                        //                    //SqlCommand.Parameters.AddWithValue("@ProcCode", TreatmentCodes.Split(',')[i].ToString().Trim().PadLeft(5, '0'));
                        //                    SqlCommand.Parameters.AddWithValue("@LineNumber", i);
                        //                    SqlCommand.Parameters.AddWithValue("@AppointmentId", Appointment_Id);
                        //                    SqlCommand.ExecuteNonQuery();
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    Utility.WriteToSyncLogFile_All("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        //                }
                        //            }
                        //        }

                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Utility.WriteToErrorLogFromAll("[ProcedureForPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        //    }


                        //    try
                        //    {
                        //        SqlCommand.CommandTimeout = 200;
                        //        if (conn.State == ConnectionState.Closed) conn.Open();
                        //        string SqlSelect = SynchClearDentQRY.GetTreatmentPlanProcedureLog;
                        //        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                        //        SqlCommand.Parameters.AddWithValue("@TreatmentPlanId", TreatmentPlanId);
                        //        SqlCommand.Parameters.AddWithValue("@ProcCode", procid);
                        //        CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                        //        DataTable SqlDt = new DataTable();
                        //        SqlDa.Fill(SqlDt);

                        //        foreach (DataRow drRow in SqlDt.Rows)
                        //        {
                        //            try
                        //            {
                        //                SqlCommand.CommandText = SynchClearDentQRY.InsertProcedureLog;
                        //                SqlCommand.Parameters.Clear();
                        //                SqlCommand.Parameters.AddWithValue("@fld_intTrPlanProcId", drRow["fld_auto_intTransProcId"].ToString());
                        //                SqlCommand.Parameters.AddWithValue("@AptID", Appointment_Id);
                        //                SqlCommand.ExecuteNonQuery();
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                Utility.WriteToErrorLogFromAll("[ProcedureForPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        //            }
                        //        }



                        //    #endregion
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Utility.WriteToErrorLogFromAll("[ProcedureForPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        //    }
                        //}
                        #endregion
                        else if (treatmetplan_key > 0 && TreatmentPlanId == 0)
                        {
                            #region IF Tretment Plan key is > 0 then Insert records in tbl_SchApp_TrPlanProc

                            try
                            {
                                //SqlCommand.CommandTimeout = 200;
                                //if (conn.State == ConnectionState.Closed) conn.Open();
                                //string SqlSelect = SynchClearDentQRY.GetTreatmentPlanProcedureLog;
                                //CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                //SqlCommand.Parameters.AddWithValue("@TreatmentPlanId", treatmetplan_key);
                                //SqlCommand.Parameters.AddWithValue("@ProcCode", procid);
                                //CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                                //DataTable SqlDt = new DataTable();
                                //SqlDa.Fill(SqlDt);

                                //foreach (DataRow drRow in SqlDt.Rows)
                                //{
                                try
                                {
                                    SqlCommand.CommandText = SynchClearDentQRY.InsertProcedureLog;
                                    SqlCommand.Parameters.Clear();
                                    //SqlCommand.Parameters.AddWithValue("@fld_intTrPlanProcId", drRow["fld_auto_intTransProcId"].ToString());
                                    SqlCommand.Parameters.AddWithValue("@fld_intTrPlanProcId", treatmetplan_key);
                                    SqlCommand.Parameters.AddWithValue("@AptID", Appointment_Id);
                                    SqlCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Utility.WriteToErrorLogFromAll("[ProcedureForPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                                }
                                // }

                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("[ProcedureForPlan Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }
                            #endregion
                        }


                    }

                }

                SqlCommand.Dispose();

                #endregion
                try
                {


                    #region adding adit user logs for appointment
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchClearDentQRY.InsertAppointmentLogToRecAtt;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", patid);
                    SqlCommand.Parameters.AddWithValue("@ContactDate", DateTime.Now);
                    SqlCommand.Parameters.AddWithValue("@shtmtd", 7);
                    SqlCommand.Parameters.AddWithValue("@User_EHR_Id", Utility.EHR_UserLogin_ID);
                    SqlCommand.Parameters.AddWithValue("@Description", "Appointment made by Adit");
                    SqlCommand.Parameters.AddWithValue("@ApptId", Appointment_Id);
                    SqlCommand.ExecuteScalar();

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_TransAudTr;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@AudDate", DateTime.Now);
                    SqlCommand.Parameters.AddWithValue("@Description", "Appointment made by Adit");
                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", patid);
                    SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                    SqlCommand.Parameters.AddWithValue("@Amount", 0.00);
                    SqlCommand.Parameters.AddWithValue("@TransDate", DateTime.Now);
                    SqlCommand.Parameters.AddWithValue("@PaymentId", DBNull.Value);
                    SqlCommand.ExecuteScalar();

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_PatInfoAccessRecord;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                    SqlCommand.Parameters.AddWithValue("@Description", "Appointment Made by Adit");
                    SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", patid);
                    SqlCommand.ExecuteScalar();

                    #endregion
                }
                catch (Exception ex1)
                {
                    Utility.WriteToErrorLogFromAll("Error in Appointment Logs  ->" + ex1.Message);
                }
            }

            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in Appointment saving is  ->" + ex.Message);
                Appointment_Id = 0;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return Appointment_Id;

        }



        public static DataTable GetBookOperatoryAppointmenetWiseDateTime(DateTime ApptDate)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetBookOperatoryAppointmenetWiseDateTime;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.AddWithValue("@ToDate", ApptDate.ToString("yyyy-MM-dd"));
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

        public static DataTable GetClearDentPatientID_NameData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentPatientID_NameData;
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

        public static DataTable GetClearDentIdelProvider()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();

                string SqlSelect = SynchClearDentQRY.GetClearDentIdelProvider;
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

        public static bool Update_Status_EHR_Appointment_Live_To_ClearDentEHR(DataTable dtLiveAppointment, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
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
                    //if (Is_Update_Status_EHR_Appointment_Live_To_EHR(dr["Appt_EHR_ID"].ToString()))
                    //{
                    SqlCommand.CommandText = SynchClearDentQRY.Update_Status_EHR_Appointment_Live_To_Local;
                    SqlCommand.Parameters.Clear();
                    // SqlCommand.Parameters.AddWithValue("@status", "1"); // 7:For Completed && 1:For Booked
                    SqlCommand.Parameters.AddWithValue("@Apptid", dr["Appt_EHR_ID"].ToString());
                    SqlCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Update Status EHR Appointment Live To ClearDent EHR  Confirmed in EHR for Apptid=" + dr["Appt_EHR_ID"].ToString());

                    bool isApptConformStatus = SynchLocalDAL.UpdateLocalAppointmentConformStatusData(dr["Appt_EHR_ID"].ToString(), dr["Service_Install_Id"].ToString());
                    // }
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

        public static bool Update_Receive_SMS_Patient_EHR_Live_To_ClearDentEHR(DataTable dtLiveAppointment, string Locationid, string Loc_ID, string _filename_EHR_patient_sms_call = "", string _EHRLogdirectory_EHR_patient_sms_call = "")
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
                        SqlCommand.CommandText = SynchClearDentQRY.Update_Receive_SMS_Patient_EHR_Live_To_ClearDentEHR;
                        SqlCommand.Parameters.AddWithValue("@receives_sms", dr["receive_sms"].ToString() == "Y" ? 1 : 0);
                        SqlCommand.Parameters.AddWithValue("@patient_id", dr["patient_ehr_id"].ToString());
                        SqlCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, " Update Receive SMS Patient EHR Live To ClearDentEHR for patient_ehr_id=" + dr["patient_ehr_id"].ToString());
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
        #endregion

        #region Patient_Form

        public static bool Save_Patient_Form_Local_To_ClearDent(DataTable dtWebPatient_Form)
        {
            string _successfullstataus = string.Empty;
            bool is_Record_Update = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.ClearDentSQLConnectionServer(ref conn);

            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
            }

            if (conn.State == ConnectionState.Closed) conn.Open();
            DataTable dtClearDentPatentColumns = GetClearDentTableColumnName("tbl_PatInfo");
            try
            {
                //OdbcCommand.CommandTimeout = 200;
                string strQauery = string.Empty;
                string Update_PatientForm_Record_ID = "";
                string phoneNo = "";

                Int64 patient_EHR_Id = 0;

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
                             foreach (DataRow dr in dtWebPatient_Form.Select(" PatientForm_Web_ID = '" + o.ToString() + "' "))
                             {
                                 if (dr["ehrfield_value"].ToString().Trim() != string.Empty)
                                 {
                                     if (dr["ehrfield"].ToString().Trim() != string.Empty)
                                     {
                                         if (dr["Patient_EHR_ID"].ToString() != "" && Convert.ToInt32(dr["Patient_EHR_ID"].ToString()) != 0)
                                         {
                                             if (dtClearDentPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper()).FirstOrDefault() != null)
                                             {
                                                 if (dr["ehrfield"].ToString().Trim().ToUpper() != "PRIMARY_SUBSCRIBER_ID" && dr["ehrfield"].ToString().Trim().ToUpper() != "PRIMARY_INSURANCE_COMPANYNAME" && dr["ehrfield"].ToString().Trim().ToUpper() != "SECONDARY_SUBSCRIBER_ID" && dr["ehrfield"].ToString().Trim().ToUpper() != "SECONDARY_INSURANCE_COMPANYNAME")
                                                 {
                                                     int ColumnSize = Convert.ToInt16(dtClearDentPatentColumns.AsEnumerable().Where(r => r.Field<string>("EHRColumnName").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper() && r.Field<object>("ColumnSize") != null).Select(r => r.Field<object>("ColumnSize")).First().ToString());
                                                     // string ColumnType = dtClearDentPatentColumns.AsEnumerable().Where(r => r.Field<string>("COLUMNNAME").ToString().ToUpper() == dr["ehrfield"].ToString().ToUpper()).Select(r => r.Field<string>("COLUMNNAME").GetType()).First().ToString();

                                                     patient_EHR_Id = Convert.ToInt32(dr["Patient_EHR_ID"].ToString());
                                                     strQauery = SynchClearDentQRY.Update_Patinet_Record_By_Patient_Form;
                                                     strQauery = strQauery.Replace("ColumnName", dr["ehrfield"].ToString().Trim());
                                                     if (dr["ehrfield"] != null && dr["ehrfield"].ToString() != "")
                                                     {
                                                         if (dr["ehrfield"].ToString().Trim().ToLower() == "fld_strhtel" || dr["ehrfield"].ToString().Trim().ToLower() == "fld_strmtel" || dr["ehrfield"].ToString().Trim().ToLower() == "fld_strotel")
                                                         {
                                                             phoneNo = dr["ehrfield_value"].ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");
                                                             if (phoneNo.Length >= ColumnSize)
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "'" + phoneNo.Substring(0, ColumnSize) + "'");
                                                             }
                                                             else
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "'" + phoneNo + "'");
                                                             }
                                                         }
                                                         else if (dr["ehrfield"].ToString().Trim().ToLower() == "fld_intsin")
                                                         {
                                                             try
                                                             {
                                                                 if (!(dr["ehrfield"].ToString().Trim().ToLower().All(char.IsLetterOrDigit)))
                                                                 {

                                                                     if (dr["ehrfield_value"].ToString().Length > 9)
                                                                     {
                                                                         strQauery = strQauery.Replace("@ehrfield_value", dr["ehrfield_value"].ToString().Substring(0, 9));
                                                                     }
                                                                     else
                                                                     {
                                                                         strQauery = strQauery.Replace("@ehrfield_value", dr["ehrfield_value"].ToString());
                                                                     }
                                                                 }
                                                                 else
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "NULL");
                                                                 }

                                                             }
                                                             catch
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "NULL");

                                                             }
                                                         }
                                                         else if (dr["ehrfield"].ToString().Trim().ToLower() == "fld_strpcode")
                                                         {
                                                             try
                                                             {
                                                                 string ZipCode = dr["ehrfield_value"].ToString().Replace(" ", "");
                                                                 if (ZipCode.Length >= ColumnSize)
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + ZipCode.Substring(0, ColumnSize) + "'");
                                                                 }
                                                                 else
                                                                 {
                                                                     strQauery = strQauery.Replace("@ehrfield_value", "'" + ZipCode + "'");
                                                                 }

                                                             }
                                                             catch
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "NULL");

                                                             }
                                                         }

                                                         else if (dr["ehrfield"].ToString().Trim().ToLower() == "fld_dtmbth")
                                                         {
                                                             try
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "'" + Convert.ToDateTime(dr["ehrfield_value"]).ToString("yyyy/MM/dd HH:mm").ToString() + "'");

                                                             }
                                                             catch
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "NULL");

                                                             }
                                                         }
                                                         else
                                                         {
                                                             if (dr["ehrfield_value"].ToString().Trim().Length >= ColumnSize)
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim().Substring(0, ColumnSize) + "'");
                                                             }
                                                             else
                                                             {
                                                                 strQauery = strQauery.Replace("@ehrfield_value", "'" + dr["ehrfield_value"].ToString().Trim() + "'");
                                                             }
                                                         }
                                                     }
                                                     else
                                                     {
                                                         strQauery = strQauery.Replace("@ehrfield_value", "''");
                                                     }
                                                     strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());

                                                     //strQauery = strQauery.Replace("@Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                                     if (conn.State == ConnectionState.Closed) conn.Open();
                                                     CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                                                     SqlCommand.ExecuteNonQuery();
                                                     conn.Close();
                                                 }
                                             }
                                         }
                                     }
                                 }
                             }
                             UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim());
                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";
                             return true;
                         });
                }

                #region Insert Patient Into ClearDent

                patient_EHR_Id = 0;


                //string ClearDentAddress = "";
                //OdbcConnection conn = null;
                //OdbcCommand OdbcCommand = new OdbcCommand();
                //CommonDB.OdbcClearDentConnectionServer(ref conn);
                if (
                dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {
                    dtWebPatient_Form.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {
                             string PrimaryInsuraceCompanyName = "";
                             string SecondaryInsuraceCompanyName = "";
                             string PrimaryInsuraceSubScriberId = "";
                             string SecondaryInsuraceSubScriberId = "";
                             strQauery = CreatePatientInsertQuery(dtWebPatient_Form, dtClearDentPatentColumns, o.ToString(), "tbl_patinfo", ref PrimaryInsuraceCompanyName, ref SecondaryInsuraceCompanyName, ref PrimaryInsuraceSubScriberId, ref SecondaryInsuraceSubScriberId);

                             if (conn.State == ConnectionState.Closed) conn.Open();

                             CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                             SqlCommand.ExecuteNonQuery();

                             string QryIdentity = "Select @@Identity as newId from tbl_PatInfo";//"Select @@Identity as newId from patient";
                             SqlCommand.CommandText = QryIdentity;
                             SqlCommand.CommandType = CommandType.Text;
                             SqlCommand.Connection = conn;
                             patient_EHR_Id = Convert.ToInt32(SqlCommand.ExecuteScalar());

                             CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                             SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_PatInfoAccessRecord;
                             SqlCommand.Parameters.Clear();

                             SqlCommand.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                             SqlCommand.Parameters.AddWithValue("@Description", "Patient Created by Adit");
                             SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                             SqlCommand.Parameters.AddWithValue("@PatientEHRId", patient_EHR_Id);

                             SqlCommand.ExecuteScalar();

                             // conn.Close();
                             try
                             {

                                 Int32 tmpPatient_Gur_id = 0;

                                 QryIdentity = "SELECT [fld_auto_intFamId] FROM [tbl_PatFam] where [fld_intHeadPatId] = " + patient_EHR_Id;
                                 SqlCommand.CommandText = QryIdentity;
                                 SqlCommand.CommandType = CommandType.Text;
                                 SqlCommand.Connection = conn;
                                 tmpPatient_Gur_id = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                 if (tmpPatient_Gur_id == 0)
                                 {
                                     SqlCommand.CommandText = SynchClearDentQRY.InsertPatientGuarantorID;
                                     SqlCommand.Parameters.Clear();
                                     SqlCommand.Parameters.AddWithValue("@fld_intHeadPatId", patient_EHR_Id);
                                     SqlCommand.ExecuteNonQuery();
                                     QryIdentity = "Select max([fld_auto_intFamId]) as newId from [tbl_PatFam]";//"Select @@Identity as newId from patient";
                                     SqlCommand.CommandText = QryIdentity;
                                     SqlCommand.CommandType = CommandType.Text;
                                     SqlCommand.Connection = conn;
                                     tmpPatient_Gur_id = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                 }
                                 SqlCommand.CommandText = SynchClearDentQRY.UpdatePatientGuarantorID;
                                 SqlCommand.Parameters.Clear();
                                 SqlCommand.Parameters.AddWithValue("@famid", tmpPatient_Gur_id);
                                 SqlCommand.Parameters.AddWithValue("@patid", patient_EHR_Id);
                                 SqlCommand.ExecuteNonQuery();
                                 conn.Close();


                                 UpdatePatientInsurance(PrimaryInsuraceCompanyName, patient_EHR_Id, 1, tmpPatient_Gur_id, PrimaryInsuraceSubScriberId);
                                 UpdatePatientInsurance(SecondaryInsuraceCompanyName, patient_EHR_Id, 2, tmpPatient_Gur_id, SecondaryInsuraceSubScriberId);
                             }
                             catch
                             { }
                             //patient_EHR_Id = Convert.ToInt64(OdbcCommand.ExecuteScalar());

                             //if (Convert.ToInt64(patient_EHR_Id) > 0)
                             //{
                             //    strQauery = SynchClearDentQRY.Update_Patinet_Record_By_Patient_Form;
                             //    strQauery = strQauery.Replace("@Patient_Id", patient_EHR_Id.ToString());
                             //    CommonDB.OdbcCommandServer(strQauery, conn, ref OdbcCommand, "txt");
                             //    OdbcCommand.ExecuteNonQuery();
                             //}
                             UpdatePatientEHRIdINPatientForm(patient_EHR_Id.ToString(), o.ToString().Trim());

                             Update_PatientForm_Record_ID = Update_PatientForm_Record_ID + o.ToString().Trim() + ";";

                             return true;
                         });
                }
                #endregion

                SynchLocalDAL.UpdatePatientFormEHR_Updateflg(dtWebPatient_Form);



                is_Record_Update = true;
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

        private static void UpdatePatientInsurance(string curPatinetInsurance_Name, long PatientId, int InsuranceCount, Int32 FamilyId, string SubScriber)
        {
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
                string SqlSelect = "select TOP 1 [fld_auto_intCarrId] from tbl_InsCarr where replace([fld_strName],'''','') = '" + curPatinetInsurance_Name + "'";
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                InsCarrId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                if (InsCarrId > 0)
                {
                    Int32 InsCovId = 0;
                    Int32 InsPolId = 0;
                    Int32 PatFamPolId = 0;
                    Int32 PatPolId = 0;
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatient_InsCov;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.ExecuteNonQuery();
                    InsCovId = GetId(conn, "tbl_InsCov", "fld_auto_intCovId");

                    CheckConnection(conn);
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatient_InsPol;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@fld_intCarrId", InsCarrId);
                    SqlCommand.Parameters.AddWithValue("@fld_intCovId", InsCovId);
                    SqlCommand.ExecuteNonQuery();
                    InsPolId = GetId(conn, "tbl_InsPol", "fld_auto_intInsPolId");

                    CheckConnection(conn);
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatient_PatFamPol;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@fld_intFamId", FamilyId);
                    SqlCommand.Parameters.AddWithValue("@fld_intInsPolId", InsPolId);
                    SqlCommand.Parameters.AddWithValue("@fld_intSubPatId", PatientId);
                    SqlCommand.ExecuteNonQuery();
                    PatFamPolId = GetId(conn, "tbl_PatFamPol", "fld_auto_intFamPolId");

                    CheckConnection(conn);
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatient_PatPol;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@fld_intPatId", PatientId);
                    SqlCommand.Parameters.AddWithValue("@fld_intFamPolId", PatFamPolId);
                    SqlCommand.Parameters.AddWithValue("@fld_strSubIdNo", SubScriber.Trim().Length > 12 ? SubScriber.Trim().Substring(0, 12) : SubScriber.Trim());
                    SqlCommand.ExecuteNonQuery();
                    PatPolId = GetId(conn, "tbl_PatPol", "fld_auto_intPatPolId");

                    if (InsuranceCount == 1)
                    {
                        SqlCommand.CommandText = SynchClearDentQRY.Insert_paitent_primaryinsurance_patplan;
                    }
                    else
                    {
                        SqlCommand.CommandText = SynchClearDentQRY.Insert_paitent_secondaryinsurance_patplan;
                    }
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@insuredid", PatPolId);
                    SqlCommand.Parameters.AddWithValue("@patid", PatientId);
                    CheckConnection(conn);
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

        private static string CreatePatientInsertQuery(DataTable dtWebPatient_Form, DataTable dtClearDentPatentColumns, string patientFormWebId, string tableName, ref string PrimaryInsuranceCompanyName, ref string SecondaryInsuranceCompanyName, ref string PrimaryInsuranceSubScriberId, ref string SecondaryInsuraceSubScriberId)
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


                dtClearDentPatentColumns.AsEnumerable().Where(z => z.Field<string>("EHRColumnName") != "")
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
                        #endregion
                        else
                        {
                            ColumnList = ColumnList + e.Field<object>("EHRColumnName").ToString().Trim() + ",";

                            if (dtColumnsExists != null && dtColumnsExists.Count() > 0)
                            {
                                if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "fld_strotel" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "fld_strmtel" || e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "fld_strhtel")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        string Phone = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Replace("(", "").Replace(")", "").Replace("-", "").Trim().Replace(" ", "");

                                        if (Phone.Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                        {
                                            ValueList = ValueList + "'" + Phone.Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                        }
                                        else
                                        {
                                            ValueList = ValueList + "'" + Phone + "'" + ",";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "''" + ",";
                                    }
                                }
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "fld_strpcode")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        string ZipCode = dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Replace(" ", "");

                                        if (ZipCode.Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                        {
                                            ValueList = ValueList + "'" + ZipCode.Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                        }
                                        else
                                        {
                                            ValueList = ValueList + "'" + ZipCode + "'" + ",";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "''" + ",";
                                    }
                                }
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "fld_intsin")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        try
                                        {
                                            if (dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Length > 9)
                                            {
                                                ValueList = ValueList + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Substring(0, 9) + ",";
                                            }
                                            else
                                            {
                                                ValueList = ValueList + dtColumnsExists.First().Field<object>("ehrfield_value").ToString() + ",";
                                            }

                                        }
                                        catch
                                        {
                                            ValueList = ValueList + "NULL,";

                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "''" + ",";
                                    }
                                }
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "fld_dtmbth")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        try
                                        {
                                            ValueList = ValueList + "'" + Convert.ToDateTime(dtColumnsExists.First().Field<object>("ehrfield_value")).ToString("yyyy/MM/dd HH:mm").ToString() + "'" + ",";
                                        }
                                        catch
                                        {

                                            ValueList = ValueList + "NULL" + ",";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "NULL" + ",";
                                    }
                                }
                                else if (e.Field<object>("EHRColumnName").ToString().Trim().ToLower() == "fld_shtprid")
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null && dtColumnsExists.First().Field<object>("ehrfield_value").ToString() != string.Empty)
                                    {
                                        try
                                        {
                                            ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                        }
                                        catch
                                        {

                                            ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                    }
                                }

                                else
                                {
                                    if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                    {
                                        if (dtColumnsExists.First().Field<object>("ehrfield_value") != null)
                                        {
                                            if (dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Length >= Convert.ToInt16(e.Field<object>("ColumnSize").ToString()))
                                            {
                                                ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim().Substring(0, Convert.ToInt16(e.Field<object>("ColumnSize").ToString())) + "'" + ",";
                                            }
                                            else
                                            {
                                                ValueList = ValueList + "'" + dtColumnsExists.First().Field<object>("ehrfield_value").ToString().Trim() + "'" + ",";
                                            }
                                        }
                                        else
                                        {
                                            ValueList = ValueList + "''" + ",";
                                        }

                                    }
                                    else if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                                    {
                                        if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SMALLINT" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT16" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT32" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                        {
                                            ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                        }
                                        else
                                        {
                                            ValueList = ValueList + "'" + e.Field<object>("DefaultValue").ToString() + "'" + ",";
                                        }
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "NULL" + ",";
                                    }

                                }
                            }
                            else
                            {
                                if (e.Field<object>("EHRDataType") != null && (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DATETIME" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DATETIME"))
                                {
                                    ValueList = ValueList + "NULL" + ",";
                                }
                                else if (e.Field<object>("DefaultValue") != null && e.Field<object>("DefaultValue").ToString().Trim().ToUpper() != "")
                                {
                                    if (e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SMALLINT" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT16" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.INT32" || e.Field<object>("EHRDataType").ToString().Trim().ToUpper() == "SYSTEM.DECIMAL")
                                    {
                                        ValueList = ValueList + e.Field<object>("DefaultValue").ToString() + ",";
                                    }
                                    else
                                    {
                                        ValueList = ValueList + "'" + e.Field<object>("DefaultValue").ToString() + "'" + ",";
                                    }
                                }
                                else if (e.Field<object>("AllowNull").ToString().Trim().ToUpper() == "TRUE")
                                {
                                    ValueList = ValueList + "NULL" + ",";
                                }
                                else
                                {
                                    ValueList = ValueList + "''" + ",";
                                }
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
                strQauery = " Insert into " + tableName + " ( " + ColumnList + " ) VALUES ( " + ValueList + " )";
                return strQauery;
            }
            catch (Exception)
            {
                throw;
            }
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
                        string SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_PatientEHRId;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_Id", PatientEHRId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", PatientFormWebId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
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

        private static DataTable GetClearDentTableColumnName(string tableName)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            SqlDataAdapter SqlDa = new SqlDataAdapter();

            using (SqlCeConnection conn1 = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {

                    //CommonDB.OdbcConnectionServer(ref conn);

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string strQauery = "SELECT COLUMN_NAME,DATA_TYPE as DataType,case when CHARACTER_MAXIMUM_LENGTH is null then NUMERIC_PRECISION else CHARACTER_MAXIMUM_LENGTH end AS ColumnSize,IS_NULLABLE as AllowDbNull FROM INFORMATION_SCHEMA. COLUMNS WHERE TABLE_NAME = '" + tableName + "'";
                    CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandTimeout = 200;
                    CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                    //DbDataReader reader = SqlCommand.ExecuteReader();
                    //DataTable table = reader.GetSchemaTable();
                    DataTable dtResult = new DataTable();
                    SqlDa.Fill(dtResult);
                    DataTable OdbcDt = null;
                    string SqlCeSelect = " SELECT COLUMN_NAME,'' AS EHRColumnName,'' AS EHRDataType,'' AS AllowNull,'' AS DefaultValue,0 as ColumnSize  FROM information_Schema.columns where table_name = 'Patient'"; ;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn1))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            OdbcDt = new DataTable();
                            SqlCeDa.Fill(OdbcDt);
                        }
                    }
                    //OdbcCommand.Parameters.AddWithValue("lastname", LastName);
                    //OdbcCommand.Parameters.AddWithValue("firstname", FirstName);
                    //OdbcCommand.Parameters.AddWithValue("mi", MiddleName);
                    //OdbcCommand.Parameters.AddWithValue("provid1", PriProv);
                    //OdbcCommand.Parameters.AddWithValue("isguar", 0);
                    //OdbcCommand.Parameters.AddWithValue("gender", 1);
                    //OdbcCommand.Parameters.AddWithValue("firstvisitdate", Convert.ToDateTime(DateFirstVisit).ToString("yyyy-MM-dd"));
                    //OdbcCommand.Parameters.AddWithValue("emailaddr", Email);
                    //OdbcCommand.Parameters.AddWithValue("pager", Mobile);
                    //OdbcCommand.Parameters.AddWithValue("patguid", Guid.NewGuid().ToString());
                    //OdbcCommand.Parameters.AddWithValue("addrid", AddressID);

                    OdbcDt.AsEnumerable()
                        .All(a =>
                        {
                            if (a["COLUMN_NAME"].ToString().ToUpper() == "PATIENT_EHR_ID")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString() == "First_name")
                            {
                                a["EHRColumnName"] = "fld_strFName";
                                a["DefaultValue"] = "NA";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "LAST_NAME")
                            {
                                a["EHRColumnName"] = "fld_strLName";
                                a["DefaultValue"] = "NA";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MOBILE")
                            {
                                a["EHRColumnName"] = "fld_strMTel";
                                a["DefaultValue"] = "000000000";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "STATUS")
                            {
                                a["EHRColumnName"] = "fld_bytStatus";
                                a["DefaultValue"] = 1;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToLower() == "address1")
                            {
                                a["EHRColumnName"] = "fld_strAddr1";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToLower() == "address2")
                            {
                                a["EHRColumnName"] = "fld_strAddr2";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "BIRTH_DATE")
                            {
                                a["EHRColumnName"] = "fld_dtmBth";
                                a["DefaultValue"] = null;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MARITALSTATUS")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "STATE")
                            {
                                a["EHRColumnName"] = "";
                                //a["DefaultValue"] = null;
                            }


                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "CITY")
                            {
                                a["EHRColumnName"] = "fld_strCity";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "CURRENT_BAL")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "0";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "THIRTY_DAY")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "0";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SIXTY_DAY")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "0";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "NINETY_DAY")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "0";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMAIL")
                            {
                                a["EHRColumnName"] = "fld_strEmail";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "HOME_PHONE")
                            {
                                a["EHRColumnName"] = "fld_strHTel";
                            }

                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MIDDLE_NAME")
                            {
                                a["EHRColumnName"] = "fld_strMIni";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PREFERRED_NAME")
                            {
                                a["EHRColumnName"] = "fld_strPName";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
                            {
                                a["EHRColumnName"] = "fld_shtPrId";
                                a["DefaultValue"] = "(Select top 1 fld_auto_shtPrId from [tbl_Pr] where fld_bytstatus = 1 and fld_bytPrTypeId = 1 order by fld_strName)";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INS_SUBSCRIBER_ID")
                            {
                                a["EHRColumnName"] = "PRIMARY_SUBSCRIBER_ID";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
                            {
                                a["EHRColumnName"] = "PRIMARY_INSURANCE_COMPANYNAME";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVE_EMAIL")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVE_SMS")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME")
                            {
                                a["EHRColumnName"] = "fld_strEmergencyName";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SALUTATION")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
                            {
                                a["EHRColumnName"] = "fld_shtHyId";
                                a["DefaultValue"] = 0;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE")
                            {
                                a["EHRColumnName"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INS_SUBSCRIBER_ID")
                            {
                                a["EHRColumnName"] = "SECONDARY_SUBSCRIBER_ID";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
                            {
                                a["EHRColumnName"] = "SECONDARY_INSURANCE_COMPANYNAME";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SEX")
                            {
                                a["EHRColumnName"] = "fld_strSex";
                                a["DefaultValue"] = 0;
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "WORK_PHONE")
                            {
                                a["EHRColumnName"] = "fld_strOTel";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ZIPCODE")
                            {
                                a["EHRColumnName"] = "fld_strPCode";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SSN")
                            {
                                a["EHRColumnName"] = "fld_intSIN";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMERGENCYCONTACT_FIRST_NAME")
                            {
                                a["EHRColumnName"] = "fld_strEmergencyName";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMERGENCYCONTACT_LAST_NAME")
                            {
                                a["EHRColumnName"] = "";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER")
                            {
                                a["EHRColumnName"] = "fld_strEmergencyContact";
                                a["DefaultValue"] = "";
                            }
                            if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SCHOOL")
                            {
                                a["EHRColumnName"] = "fld_strSchool";
                                a["DefaultValue"] = "";
                            }


                            if (a["EHRColumnName"].ToString() != "")
                            {
                                if (dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMN_NAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Count() > 0)
                                {
                                    a["EHRDataType"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMN_NAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<string>("DataType")).First().ToString();
                                    a["AllowNull"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMN_NAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<object>("AllowDbNull")).First().ToString();
                                    if (a["EHRDataType"].ToString().ToLower() != "smalldatetime" && a["EHRDataType"].ToString().ToLower() != "bit" && a["EHRDataType"].ToString().ToLower() != "datetime")
                                    {
                                        a["ColumnSize"] = dtResult.AsEnumerable().Where(r => r.Field<string>("COLUMN_NAME").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<object>("ColumnSize")).First().ToString();
                                    }

                                }
                                else
                                {
                                    a["EHRDataType"] = "System.String";
                                    a["AllowNull"] = "Yes";
                                }
                            }

                            return true;
                        });


                    //DataRow drNewRow = OdbcDt.NewRow();
                    //drNewRow["COLUMN_NAME"] = "AddressId";
                    //drNewRow["EHRColumnName"] = "addrid";
                    //drNewRow["EHRDataType"] = "System.String";
                    //drNewRow["AllowNull"] = "No";
                    //drNewRow["DefaultValue"] = "@addrid";
                    //OdbcDt.Rows.Add(drNewRow);


                    //DataRow drNewRow1 = OdbcDt.NewRow();
                    //drNewRow1["COLUMN_NAME"] = "patguid";
                    //drNewRow1["EHRColumnName"] = "patguid";
                    //drNewRow1["EHRDataType"] = "System.String";
                    //drNewRow1["AllowNull"] = "No";
                    //drNewRow1["DefaultValue"] = Guid.NewGuid().ToString();
                    //OdbcDt.Rows.Add(drNewRow1);

                    return OdbcDt;

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

        public static bool Save_Document_in_ClearDent(string strPatientFormID = "")
        {
            int DocId = 0;
            bool IsDocUpdate = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocData("1", strPatientFormID);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    string destPatientDocPath = "";
                    string dstnLocation = "";
                    string tmpFileOrgName = "";
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    string FileExtension = "", RenameFileName = "";
                    string sqlSelect = string.Empty;
                    string QryDoc = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        destPatientDocPath = "";
                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\Program Files (x86)\Prococious Technology Inc\ClearDent\AttachedFile\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
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
                        dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }
                        tmpFileOrgName = Path.GetFileName(sourceLocation);

                        #region Convert PDF to TIF & again TIF to PDF for chinese fonts

                        if (Utility.IsChinesePDF)
                        {
                            // Utility.WriteToErrorLogFromAll("3.Save_Document_in_ClearDent inside Utility.IsChinesePDF : " + Utility.IsChinesePDF.ToString());
                            //call ConvertPDF exe
                            bool IsChinesePDF_Converted = false;
                            var directory = Application.StartupPath;
                            try
                            {
                                string exePath = Path.Combine(directory, "ChinesePDFConvert", "ChinesePDFConvert.exe");
                                string PDFFile_Path = sourceLocation;
                                string FolderName = "PatientDocument";
                                string Patient_Form_Id = dr["PatientDoc_Web_ID"].ToString();
                                // bool status = ConvertPDF(exePath, PDFFile_Path, FolderName, Patient_Form_Id);
                                if (File.Exists(exePath))
                                {
                                    //Utility.WriteToErrorLogFromAll("4.Save_Document_in_ClearDent Utility.IsChinesePDF true if condition exe found exePath ");
                                    using (Process process = new Process())
                                    {
                                        //process.StartInfo.FileName = @"D:\ChinesePDF_FINAL_CODE\ChienesePDFConvert\ChienesePDFConvert\bin\Debug\ChinesePDFConvert.exe";
                                        process.StartInfo.FileName = exePath;
                                        process.StartInfo.CreateNoWindow = false;
                                        process.StartInfo.UseShellExecute = false;
                                        process.StartInfo.RedirectStandardOutput = true;

                                        process.StartInfo.Arguments = string.Format("{0} {1} {2} {3} {4}", "\"" + CommonUtility.GetAditDocTempPath() + "\"", "\"" + sourceLocation + "\"", "\"" + dstnLocation + "\"", "\"" + FolderName + "\"", "\"" + Patient_Form_Id + "\"");
                                        process.Start();
                                        process.WaitForExit();
                                        string PatientDocument_Web_Id = CommonUtility.GetAditDocTempPath() + "\\" + FolderName + "\\" + Patient_Form_Id + "\\";
                                        Directory.Delete(PatientDocument_Web_Id, true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            Utility.WriteToErrorLogFromAll("5.Save_Document_in_ClearDent else condition Utility.IsChinesePDF : " + Utility.IsChinesePDF.ToString() + " sourceLocation - " + sourceLocation.ToString() + " dstnLocation - " + dstnLocation.ToString());
                            System.IO.File.Copy(sourceLocation, dstnLocation, true);
                        }
                        #endregion

                        //Guid tmpGUID = Guid.NewGuid();
                        FileInfo fi = new FileInfo(dstnLocation);
                        FileExtension = fi.Extension;
                        RenameFileName = dr["PatientDoc_Web_ID"].ToString() + FileExtension;

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        QryDoc = SynchClearDentQRY.InsertPatientDocData;

                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM/dd/yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM/dd/yy");
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";

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
                            //case "DS":
                            //    ShowingName = SubmitedDate;
                            //    break;
                            default:
                                ShowingName = SubmitedDate;
                                break;
                        }

                        SqlCommand.CommandText = QryDoc;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("fld_intPatId", dr["Patient_EHR_ID"].ToString().Trim());
                        //SqlCommand.Parameters.AddWithValue("fld_strOFileName", dr["Patient_EHR_ID"].ToString().Trim() + '-' + DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_strOFileName", ShowingName);
                        SqlCommand.Parameters.AddWithValue("fld_strDBFileName", tmpFileOrgName.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_dtmCreate", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_shtCreateUser", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("fld_dtmLastMod", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_shtLastModUser", Utility.EHR_UserLogin_ID);

                        if (dr["folder_ehr_id"].ToString().Trim() == "" || dr["folder_ehr_id"].ToString().Trim() == "0")
                        {
                            SqlCommand.Parameters.AddWithValue("fld_shtFileCatId", "9");
                        }
                        else
                        {
                            SqlCommand.Parameters.AddWithValue("fld_shtFileCatId", dr["folder_ehr_id"].ToString().Trim());
                        }

                        SqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from tbl_File";
                        SqlCommand.CommandText = QryIdentity;
                        SqlCommand.CommandType = CommandType.Text;
                        DocId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        System.IO.File.Move(dstnLocation, destPatientDocPath + "\\" + RenameFileName);

                        PullLiveDatabaseDAL.Update_PatientFormDoc_Local_To_EHR(dr["PatientDoc_Web_ID"].ToString(), DocId.ToString(), "1");
                        File.Delete(sourceLocation);
                        Save_PatientFormDocAttachment_in_ClearDent(dr["PatientDoc_Web_ID"].ToString());
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

        public static bool Save_PatientFormDocAttachment_in_ClearDent(string PatientForm_web_Id)
        {
            int DocId = 0;
            bool IsDocUpdate = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DataTable dtWebPatient_FormDoc = SynchLocalDAL.GetLivePatientFormDocAttachmentData("1", PatientForm_web_Id);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (dtWebPatient_FormDoc.Rows.Count > 0)
                {
                    string destPatientDocPath = "";
                    string dstnLocation = "";
                    string tmpFileOrgName = "";
                    string ShowingName, SubmitedDate, FormName, PatientName = "";
                    string FileExtension = "", RenameFileName = "";
                    string sqlSelect = string.Empty;
                    string QryDoc = "";
                    foreach (DataRow dr in dtWebPatient_FormDoc.Rows)
                    {
                        destPatientDocPath = "";
                        if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                        {
                            destPatientDocPath = @"C:\Program Files (x86)\Prococious Technology Inc\ClearDent\AttachedFile\" + dr["Patient_EHR_ID"].ToString().Trim() + "\\";
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
                                continue;
                            }
                        }
                        dstnLocation = string.Format(destPatientDocPath + "\\{0}", Path.GetFileName(sourceLocation));
                        if (!System.IO.Directory.Exists(destPatientDocPath))
                        {
                            System.IO.Directory.CreateDirectory(destPatientDocPath);
                        }
                        tmpFileOrgName = Path.GetFileName(sourceLocation);
                        System.IO.File.Copy(sourceLocation, dstnLocation, true);
                        //Guid tmpGUID = Guid.NewGuid();
                        FileInfo fi = new FileInfo(dstnLocation);
                        FileExtension = fi.Extension;
                        RenameFileName = dr["PatientDoc_Web_ID"].ToString() + FileExtension;

                        sqlSelect = string.Empty;
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        QryDoc = SynchClearDentQRY.InsertPatientDocData;

                        SubmitedDate = dr["submit_time"] != null ? Convert.ToDateTime(dr["submit_time"]).ToString("MM/dd/yy") : Convert.ToDateTime(dr["Entry_DateTime"]).ToString("MM/dd/yy");
                        FormName = dr["Form_Name"] != null ? dr["Form_Name"].ToString() : "";
                        PatientName = dr["Patient_Name"] != null ? dr["Patient_Name"].ToString() : "";

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
                            //case "DS":
                            //    ShowingName = SubmitedDate;
                            //    break;
                            default:
                                ShowingName = SubmitedDate;
                                break;
                        }
                        if (ShowingName.Length > 255)
                        {
                            ShowingName = ShowingName.Substring(0, 254);
                        }
                        SqlCommand.CommandText = QryDoc;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("fld_intPatId", dr["Patient_EHR_ID"].ToString().Trim());
                        //SqlCommand.Parameters.AddWithValue("fld_strOFileName", dr["Patient_EHR_ID"].ToString().Trim() + '-' + DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_strOFileName", ShowingName);
                        SqlCommand.Parameters.AddWithValue("fld_strDBFileName", tmpFileOrgName.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_dtmCreate", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_shtCreateUser", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("fld_dtmLastMod", DateTime.Now.ToString());
                        SqlCommand.Parameters.AddWithValue("fld_shtLastModUser", Utility.EHR_UserLogin_ID);

                        if (dr["folder_ehr_id"].ToString().Trim() == "" || dr["folder_ehr_id"].ToString().Trim() == "0")
                        {
                            SqlCommand.Parameters.AddWithValue("fld_shtFileCatId", "9");
                        }
                        else
                        {
                            SqlCommand.Parameters.AddWithValue("fld_shtFileCatId", dr["folder_ehr_id"].ToString().Trim());
                        }

                        SqlCommand.ExecuteNonQuery();

                        string QryIdentity = "Select @@Identity as newId from tbl_File";
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
        }

        public static string GetClearDentDocPath(string ConString)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            conn = new SqlConnection(ConString);
            SqlDataAdapter SqlDa = new SqlDataAdapter();

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string strQauery = "SELECT [fld_strOptions] FROM [ClearDent_Cfg].[dbo].[tbl_CfgOption] where fld_strOptName = 'AttachFileLocation'";
                CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                SqlCommand.CommandTimeout = 200;
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                string Docpath = Convert.ToString(SqlCommand.ExecuteScalar());
                return Docpath;
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
        public static string GetClearDentProfileImagePath(string ConString)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            conn = new SqlConnection(ConString);
            SqlDataAdapter SqlDa = new SqlDataAdapter();

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string strQauery = "SELECT [fld_strOptions] FROM [ClearDent_Cfg].[dbo].[tbl_CfgOption] where fld_strOptName = 'DigImgFileLocation'";
                CommonDB.SqlServerCommand(strQauery, conn, ref SqlCommand, "txt");
                SqlCommand.CommandTimeout = 200;
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                string Docpath = Convert.ToString(SqlCommand.ExecuteScalar());
                return Docpath;
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

        #endregion


        public static DataSet GetClearDentMedicalHistoryData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            DataSet dsResult = new DataSet();
            DataTable dtFormMaster = new DataTable("CD_FormMaster");
            DataTable dtQuestionMaster = new DataTable("CD_QuestionMaster");
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentMedicalFormMaster;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                SqlDa.Fill(dtFormMaster);
                dsResult.Tables.Add(dtFormMaster);

                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlSelect = SynchClearDentQRY.GetClearDentMedicalQuestionMaster;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                SqlDa.Fill(dtQuestionMaster);
                dsResult.Tables.Add(dtQuestionMaster);

                return dsResult;
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


        public static DataTable GetCleardentMedicationData()
        {

            //DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetCleardentMedicationMaster;
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


        public static DataTable GetCleardentPatientMedicationData(string Patient_EHR_IDS)
        {

            //DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                {
                    SqlSelect = SynchClearDentQRY.GetCleardentPatientMedicationMaster;
                }
                else
                {
                    SqlSelect = SynchClearDentQRY.GetCleardentPatientMedicationMaster + " where pp.fld_intPatId in (" + Patient_EHR_IDS + ")";
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

        #region LocalToTracker MediclerHistory
        public static bool SaveMedicalHistoryLocalToClearDent(string strPatientFormID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            try
            {
                DataTable dtWebPatient_FormMedicalHistory = SynchLocalDAL.GetLiveCleardentPatientFormMedicalHistoryData(strPatientFormID);
                if (dtWebPatient_FormMedicalHistory != null)
                {
                    if (dtWebPatient_FormMedicalHistory.Rows.Count > 0)
                    {
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
                        }
                        DataTable LocalRespoanseSetDt = dtWebPatient_FormMedicalHistory.Copy().DefaultView.ToTable(true, "Patient_EHR_ID", "FormName_Name", "PatientForm_Web_ID");
                        if (LocalRespoanseSetDt != null)
                        {
                            foreach (DataRow dr in LocalRespoanseSetDt.Rows)
                            {
                                DataTable DentrixRespoanseSetDt = GetCleardentMedicleResponseData(dr["FormName_Name"].ToString(), dr["Patient_EHR_ID"].ToString());
                                string ResponsesetId = "";
                                if (DentrixRespoanseSetDt.Rows.Count == 0)
                                {
                                    string sqlSelect = string.Empty;
                                    CommonDB.TrackerSQLConnectionServer(ref conn);
                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchClearDentQRY.InsertCDPatMedicleResponseData;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("fld_intPatId", dr["Patient_EHR_ID"].ToString());
                                    SqlCommand.Parameters.AddWithValue("fld_strTempDesc", dr["FormName_Name"].ToString());
                                    CheckConnection(conn);
                                    SqlCommand.ExecuteNonQuery();

                                    string QryIdentity = "Select max(fld_auto_intPatMedQGroupId) as newId from tbl_PatMedQGroup";
                                    SqlCommand.CommandText = QryIdentity;
                                    SqlCommand.CommandType = CommandType.Text;
                                    SqlCommand.Connection = conn;
                                    CheckConnection(conn);
                                    ResponsesetId = Convert.ToString(SqlCommand.ExecuteScalar());

                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_TransAudTr;
                                    SqlCommand.Parameters.Clear();
                                    SqlCommand.Parameters.AddWithValue("@AudDate", DateTime.Now);
                                    SqlCommand.Parameters.AddWithValue("@Description", dr["FormName_Name"].ToString());
                                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", dr["Patient_EHR_ID"].ToString());
                                    SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                                    SqlCommand.Parameters.AddWithValue("@Amount", 0.00);
                                    SqlCommand.Parameters.AddWithValue("@TransDate", DateTime.Now);
                                    SqlCommand.Parameters.AddWithValue("@PaymentId", DBNull.Value);

                                    SqlCommand.ExecuteScalar();

                                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                    SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_PatInfoAccessRecord;
                                    SqlCommand.Parameters.Clear();

                                    SqlCommand.Parameters.AddWithValue("@PaymentDate", DateTime.Now);
                                    SqlCommand.Parameters.AddWithValue("@Description", dr["FormName_Name"].ToString());
                                    SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", dr["Patient_EHR_ID"].ToString());

                                    SqlCommand.ExecuteScalar();
                                }
                                else
                                {
                                    ResponsesetId = Convert.ToString(DentrixRespoanseSetDt.Rows[0]["fld_auto_intPatMedQGroupId"].ToString());
                                }

                                DataRow[] LocalRespoanseDt = dtWebPatient_FormMedicalHistory.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"].ToString() + "' and FormName_Name = '" + dr["FormName_Name"].ToString() + "'");

                                if (LocalRespoanseDt != null)
                                {
                                    foreach (DataRow drRespoanse in LocalRespoanseDt)
                                    {
                                        string responseuniqueid = "";

                                        DataRow[] DentrixRespoanseDr = DentrixRespoanseSetDt.Copy().Select("fld_intPatMedQGroupId = '" + ResponsesetId + "' and fld_strMedQ = '" + drRespoanse["Question_Description"].ToString() + "'");
                                        if (DentrixRespoanseDr == null || DentrixRespoanseDr.Count() == 0)
                                        {
                                            string sqlSelect = string.Empty;
                                            CommonDB.TrackerSQLConnectionServer(ref conn);
                                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                            SqlCommand.CommandText = SynchClearDentQRY.InsertCDPatMedicleQuestionResponseData;
                                            SqlCommand.Parameters.Clear();
                                            SqlCommand.Parameters.AddWithValue("fld_intPatMedQGroupId", ResponsesetId);
                                            SqlCommand.Parameters.AddWithValue("fld_strMedQ", drRespoanse["Question_Description"].ToString());
                                            SqlCommand.Parameters.AddWithValue("fld_shtMedQSeq", Convert.ToInt16(drRespoanse["Question_Sequence"].ToString()));
                                            SqlCommand.Parameters.AddWithValue("fld_blnAnswer", Convert.ToBoolean(drRespoanse["Question_blnAnswer"].ToString()));
                                            SqlCommand.Parameters.AddWithValue("fld_blnWarn", Convert.ToBoolean(drRespoanse["Question_Warnings"].ToString()));
                                            SqlCommand.Parameters.AddWithValue("fld_strAnswer", drRespoanse["Answer_Value"].ToString());
                                            CheckConnection(conn);
                                            SqlCommand.ExecuteNonQuery();

                                            string QryIdentity = "Select fld_auto_intPatMedId as newId from tbl_PatMedQ where fld_intPatMedQGroupId = '" + ResponsesetId + "' and fld_strMedQ = '" + drRespoanse["Question_Description"].ToString() + "'";
                                            CommonDB.SqlServerCommand(QryIdentity, conn, ref SqlCommand, "txt");
                                            //SqlCommand.CommandText = QryIdentity;
                                            SqlCommand.CommandType = CommandType.Text;
                                            SqlCommand.Connection = conn;
                                            CheckConnection(conn);
                                            responseuniqueid = Convert.ToString(SqlCommand.ExecuteScalar());
                                        }
                                        else
                                        {
                                            responseuniqueid = Convert.ToString(DentrixRespoanseSetDt.Rows[0]["fld_auto_intPatMedId"].ToString());
                                        }

                                        UpdateResponseUniqueEHRIdInCD_Response(responseuniqueid, drRespoanse["CD_FormMaster_EHR_ID"].ToString(), dr["Patient_EHR_ID"].ToString(), drRespoanse["CD_QuestionMaster_EHR_ID"].ToString());
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
                return false;
                throw ex;
            }
            finally
            {
                // if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private static bool UpdateResponseUniqueEHRIdInCD_Response(string responseuniqueid, string CD_FormMaster_EHR_ID, string Patient_EHR_ID, string CD_QuestionMaster_EHR_ID)
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
                        string SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Cleardent_Response_EHR_ID;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("CD_intPatMedId", responseuniqueid);
                            SqlCeCommand.Parameters.AddWithValue("CD_FormMaster_EHR_ID", CD_FormMaster_EHR_ID);
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", Patient_EHR_ID);
                            SqlCeCommand.Parameters.AddWithValue("CD_QuestionMaster_EHR_ID", CD_QuestionMaster_EHR_ID);
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



        public static DataTable GetCleardentMedicleResponseData(string CD_FormName, string Patient_EHR_id)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.TrackerSQLConnectionServer(ref conn);
            SqlDataAdapter SqlDa = null;
            try
            {

                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SynchClearDentQRY.GetClearDentMedicleResponseData;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.AddWithValue("fld_strFormName", CD_FormName);
                SqlCommand.Parameters.AddWithValue("fld_intPatId", Patient_EHR_id);
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

        #endregion


        //public static void deleteduplicates(string tablename,string colmname,Int64 id)
        //{
        //    SqlConnection conn = null;
        //    SqlCommand SqlCommand = null;
        //    CommonDB.ClearDentSQLConnectionServer(ref conn);
        //    if (conn.State == ConnectionState.Closed) conn.Open();
        //    string sqlSelect = string.Empty;
        //    try
        //    {
        //        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
        //        SqlCommand.CommandText = "Delete from "+tablename.Trim()+" where "+colmname.Trim()+"="+id+" ";
        //        SqlCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static string Save_PatientPayment_Local_To_ClearDent(DataTable dtWebPatientPayment, string DbConnectionString, string ServiceInstalltionId, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            Int64 noteId = 0;
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;

            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
            }

            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                string sqlSelect = string.Empty;
                DateTime PostDate;
                PostDate = DateTime.Now;
                Int64 paymentModeId = 0;
                Int64 CareCreditModeId = 0;
                Int64 refundId = 0;
                Int64 AdjustmenttypeId = 0;
                Int64 CareCreditrefundId = 0;
                Int64 CareCreditAdjustmenttypeId = 0;
                string providerId = "";

                #region check adit pay payment categeries



                #region Check or create Payment Mode Adit Pay
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = "IF NOT EXISTS ( select 1 from tbl_CfgPmtType where fld_strDescription = 'Adit Pay' ) BEGIN INSERT INTO tbl_CfgPmtType (fld_strDescription,fld_blnInsBulkPmt,fld_blnPatPmt,fld_blnInsPmt,fld_blnLedgerColorPatPmt) values ('Adit Pay',NULL,1,0,1) select @@IDENTITY  END ELSE BEGIN  select fld_bytPmtMthdId from tbl_CfgPmtType where fld_strDescription = 'Adit Pay' END ";
                paymentModeId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                if (paymentModeId > 0)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Create Payment Mode Adit Pay fld_strDescription = Adit Pay with payment id " + paymentModeId.ToString());
                }
                #endregion

                #region Check or create Payment Mode Adit Pay Refund
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = "select fld_bytAdjType from tbl_CfgAdjType where fld_strDescription = 'Refund Patient Payment'";
                refundId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                if (refundId > 0)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Create Payment Mode Adit Pay fld_strDescription = 'Refund Patient Payment' with refund id " + refundId.ToString());
                }
                #endregion

                #region Check or create Payment Mode for Discount
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = "select fld_bytAdjType from tbl_CfgAdjType where fld_strDescription like 'Write Off Patient'";
                AdjustmenttypeId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                if (AdjustmenttypeId > 0)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Create Payment Mode Adit Pay fld_strDescription like 'Write Off Patient' with Adjustment id " + AdjustmenttypeId.ToString());
                }

                #endregion
                #endregion

                #region check CareCredit payment categeries

                #region Check or create Payment Mode CareCredit
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = "IF NOT EXISTS ( select 1 from tbl_CfgPmtType where fld_strDescription = 'CareCredit' ) BEGIN INSERT INTO tbl_CfgPmtType (fld_strDescription,fld_blnInsBulkPmt,fld_blnPatPmt,fld_blnInsPmt,fld_blnLedgerColorPatPmt) values ('CareCredit',NULL,1,0,1) select @@IDENTITY  END ELSE BEGIN  select fld_bytPmtMthdId from tbl_CfgPmtType where fld_strDescription = 'CareCredit' END ";
                CareCreditModeId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                if (CareCreditModeId > 0)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Create Payment Mode CareCredit fld_strDescription = CareCredit with payment id " + CareCreditModeId.ToString());
                }

                #endregion
                #endregion

                #region check CareCredit payment categeries

                #region Check or create Payment Mode CareCredit
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.CommandText = "IF NOT EXISTS ( select 1 from tbl_CfgPmtType where fld_strDescription = 'CareCredit' ) BEGIN INSERT INTO tbl_CfgPmtType (fld_strDescription,fld_blnInsBulkPmt,fld_blnPatPmt,fld_blnInsPmt,fld_blnLedgerColorPatPmt) values ('CareCredit',NULL,1,0,1) select @@IDENTITY  END ELSE BEGIN  select fld_bytPmtMthdId from tbl_CfgPmtType where fld_strDescription = 'CareCredit' END ";
                CareCreditModeId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                if (CareCreditModeId > 0)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Create Payment Mode CareCredit fld_strDescription = CareCredit with payment id " + CareCreditModeId.ToString());
                }
                #endregion

                #region Check or create Payment Mode CareCredit Refund
                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.CommandText = "IF NOT EXISTS ( select 1 from tbl_CfgPmtType where fld_strDescription = 'CareCredit Refund' ) BEGIN INSERT INTO tbl_CfgPmtType (fld_strDescription,fld_blnInsBulkPmt,fld_blnPatPmt,fld_blnInsPmt,fld_blnLedgerColorPatPmt) values ('CareCredit Refund',NULL,1,0,1) select @@IDENTITY  END ELSE BEGIN  select fld_bytPmtMthdId from tbl_CfgPmtType where fld_strDescription = 'CareCredit Refund' END ";
                CareCreditrefundId = refundId;//Convert.ToInt64(SqlCommand.ExecuteScalar());
                //if (CareCreditrefundId > 0)
                //{
                //    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Create Payment Mode Adit Pay fld_strDescription = 'CareCredit Refund' with refund id " + CareCreditrefundId.ToString());
                //}
                #endregion

                #region Check or create Payment Mode for CareCredit Discount
                //CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.CommandText = "IF NOT EXISTS ( select 1 from tbl_CfgPmtType where fld_strDescription = 'CareCredit Discount' ) BEGIN INSERT INTO tbl_CfgPmtType (fld_strDescription,fld_blnInsBulkPmt,fld_blnPatPmt,fld_blnInsPmt,fld_blnLedgerColorPatPmt) values ('CareCredit Discount',NULL,1,0,1) select @@IDENTITY  END ELSE BEGIN  select fld_bytPmtMthdId from tbl_CfgPmtType where fld_strDescription = 'CareCredit Discount' END ";
                CareCreditAdjustmenttypeId = AdjustmenttypeId;// Convert.ToInt64(SqlCommand.ExecuteScalar());
                //if (CareCreditAdjustmenttypeId > 0)
                //{
                //    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Create Payment Mode Adit Pay fld_strDescription like 'CareCredit Discount' with Adjustment id " + CareCreditAdjustmenttypeId.ToString());
                //}

                #endregion

                #endregion
                foreach (DataRow drRow in dtWebPatientPayment.Rows)
                {
                    decimal Amount = Convert.ToDecimal(drRow["Amount"]) - Convert.ToDecimal(drRow["Discount"]);
                    // Amount = Convert.ToDecimal(drRow["Amount"]) - Convert.ToDecimal(drRow["Discount"]);
                    // Amount = Amount * -1;
                    #region get primary provider for patient
                    if (drRow["ProviderEHRId"] == null || (drRow["ProviderEHRId"] != null && (drRow["ProviderEHRId"].ToString() == string.Empty || drRow["ProviderEHRId"].ToString() == "0")))
                    {
                        providerId = " (Select isnull( fld_shtPrId,0) from tbl_patinfo where fld_auto_intPatId=" + drRow["PatientEHRId"] + ")";
                    }
                    else
                    {
                        providerId = drRow["ProviderEHRId"].ToString();
                    }

                    #endregion

                    try
                    {
                        if (drRow["PaymentMethod"].ToString().ToLower() == "carecredit")
                        {
                            SaveCareCreditPaymentToEHR(drRow, providerId, DbConnectionString, ServiceInstalltionId, Amount, CareCreditModeId, CareCreditAdjustmenttypeId, CareCreditrefundId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        }
                        else
                        {
                            SaveAditPayPaymentToEHR(drRow, providerId, DbConnectionString, ServiceInstalltionId, Amount, paymentModeId, AdjustmenttypeId, refundId, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                        }
                    }
                    catch (Exception ex1)
                    {
                        bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow);

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

        public static void SaveCareCreditPaymentToEHR(DataRow drRow, string providerId, string DbConnectionString, string ServiceInstalltionId, decimal Amount, Int64 CareCreditModeId, Int64 CareCreditAdjustmenttypeId, Int64 CareCreditrefundId, string _filename_EHR_Payment, string _EHRLogdirectory_EHR_Payment)
        {
            Int64 noteId = 0;
            Int32 TransId = 0;
            Int64 TransProcId = 0;
            Int64 PaymentId = 0;
            Int64 DiscountId = 0;
            bool istransEntryEnteredNow = false;
            bool ispaymentEntryEnteredNow = false;
            DateTime PostDate;
            PostDate = DateTime.Now;
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            string sqlSelect = string.Empty;
            try
            {
                #region Create Payment Log entry bcoz Financial setting is only create log or Both
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 1 || Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 3)
                {

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPaymentLog;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                    SqlCommand.Parameters.AddWithValue("@User_EHR_Id", Convert.ToString(Utility.EHR_UserLogin_ID));
                    SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                    SqlCommand.Parameters.AddWithValue("@PatientNote", drRow["template"]);
                    SqlCommand.Parameters.AddWithValue("@Method", 7);
                    noteId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                    if (noteId > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Payment Log  PatientEHRId=" + drRow["PatientEHRId"] + " and User_EHR_Id=" + Convert.ToString(Utility.EHR_UserLogin_ID) + " with Log id " + noteId.ToString());
                    }
                }
                #endregion

                #region Create Payment Entry in EHR if Payment Mode is Paid or Partial Paid and Log seeting is Ledger or Both
                if (Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 2 || Convert.ToInt16(drRow["EHRSyncFinancialLogSetting"]) == 3)
                {
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {

                        #region Check and Insert Records in Transaction Table
                        try
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.checktransentryexist;
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", providerId);
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", Convert.ToInt16(drRow["PatientEHRId"]));
                            SqlCommand.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"].ToString());
                            //SqlCommand.Parameters.AddWithValue("@ProviderId", 0);
                            SqlCommand.Parameters.AddWithValue("@PostDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                            TransId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                            if (TransId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Check trans entry exist for PatientEHRId=" + drRow["PatientEHRId"] + ",PaymentMode=" + drRow["PaymentMode"].ToString() + " and TransId=" + Convert.ToString(TransId));
                            }
                            if (TransId == 0)
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_Tranc;
                                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", providerId);
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                                SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                                SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                                //SqlCommand.Parameters.AddWithValue("@ProviderId", drRow["ProviderEHRId"].ToString());
                                SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);

                                TransId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                if (TransId > 0)
                                {
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment Tranc for PatientEHRId=" + drRow["PatientEHRId"] + ",ProviderId=" + providerId + "and TransId=" + Convert.ToString(TransId));
                                }
                                istransEntryEnteredNow = true;
                            }


                        }
                        catch (Exception ex)
                        {
                            Utility.WriteToSyncLogFile_All("error in InsertPatientPayment_Tranc=   " + ex.Message);

                            //throw;
                        }
                        #endregion

                        #region If Entry exists in Transaction Table then enter in Transaction procedure
                        if (TransId > 0 && istransEntryEnteredNow)
                        {
                            try
                            {
                                //@TransId ,'CREDT' , @CurrentDate,'C', @ProviderId, 0.00, @PostDate ,  @ProcessDate
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_TransProc;
                                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", providerId);
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@TransId", TransId);
                                SqlCommand.Parameters.AddWithValue("@CurrentDate", PostDate);
                                //SqlCommand.Parameters.AddWithValue("@ProviderId", drRow["ProviderEHRId"]);
                                SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);
                                SqlCommand.Parameters.AddWithValue("@ProcessDate", drRow["PaymentDate"]);
                                TransProcId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                if (TransProcId > 0)
                                {
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment TransProc for  ProviderId=" + providerId + " and TransId=" + Convert.ToString(TransId));
                                }
                                istransEntryEnteredNow = false;
                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToSyncLogFile_All("7 error in InsertPatientPayment_TransProc=   " + ex.Message);
                                // deleteduplicates("tbl_trans", "fld_auto_intTransId",TransId);
                                // continue;
                                //throw;

                            }

                        }
                        #endregion

                    }
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        #region Get First Refund id from Transaction Procedure in case of refund entry                       
                        CommonDB.SqlServerCommand("select max(fld_auto_intTransProcId) from tbl_TransProc where fld_intTransId= (select max(fld_auto_intTransId) from tbl_Trans where flt_intPatId=" + drRow["PatientEHRId"] + ")", conn, ref SqlCommand, "txt");

                        TransProcId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        if (TransProcId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get TransProcId =" + Convert.ToString(TransProcId) + " for PatientEHRId=" + drRow["PatientEHRId"]);
                        }
                        #endregion
                    }

                    try
                    {
                        #region Check the payment already exists or not compare with Patient, Payment date, Patient and Payment mode
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchClearDentQRY.checkpaymententryexist;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                        SqlCommand.Parameters.AddWithValue("@Description", drRow["template"].ToString());
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                        SqlCommand.Parameters.AddWithValue("@PostDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                        //SqlCommand.Parameters.AddWithValue("@PatientPaymentWebId", 0);
                        if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            SqlCommand.Parameters.AddWithValue("@PaymentMode", CareCreditModeId);
                            SqlCommand.Parameters.AddWithValue("@fldblnisadj", 0);
                            SqlCommand.Parameters.AddWithValue("@fldbytAdjType", DBNull.Value);
                        }
                        else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                        {
                            SqlCommand.Parameters.AddWithValue("@PaymentMode", DBNull.Value);
                            SqlCommand.Parameters.AddWithValue("@fldblnisadj", 1);
                            SqlCommand.Parameters.AddWithValue("@fldbytAdjType", CareCreditrefundId);
                        }
                        PaymentId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        Utility.WriteToSyncLogFile_All("paymentid= " + PaymentId.ToString());
                        #endregion

                        #region Create payment entry if payment not exists in EHR
                        if (PaymentId == 0)
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_Payment;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"].ToString());
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                            SqlCommand.Parameters.AddWithValue("@PostDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                            //SqlCommand.Parameters.AddWithValue("@PatientPaymentWebId", 0);
                            if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                SqlCommand.Parameters.AddWithValue("@PaymentMode", CareCreditModeId);
                                SqlCommand.Parameters.AddWithValue("@fldblnisadj", 0);
                                SqlCommand.Parameters.AddWithValue("@fldbytAdjType", DBNull.Value);
                            }
                            else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                            {
                                SqlCommand.Parameters.AddWithValue("@PaymentMode", DBNull.Value);
                                SqlCommand.Parameters.AddWithValue("@fldblnisadj", 1);
                                SqlCommand.Parameters.AddWithValue("@fldbytAdjType", CareCreditrefundId);
                            }
                            PaymentId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                            if (PaymentId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",PaymentMode=" + drRow["PaymentMode"].ToString() + " Payment Id " + PaymentId.ToString());
                            }

                            ispaymentEntryEnteredNow = true;
                        }
                        #endregion

                    }
                    catch (Exception)
                    {
                    }

                    #region Create discount entry if discount is greater then zero
                    if (Convert.ToDecimal(drRow["Discount"]) > 0)
                    {
                        try
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_Payment;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                            SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@PaymentMode", CareCreditModeId);
                            SqlCommand.Parameters.AddWithValue("@fldblnisadj", 1);
                            SqlCommand.Parameters.AddWithValue("@fldbytAdjType", CareCreditAdjustmenttypeId);
                            DiscountId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                            if (DiscountId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Insert Patient Payment for PatientEHRId=" + drRow["PatientEHRId"] + ",PaymentMode=" + CareCreditModeId + "  and DiscountId=" + Convert.ToString(DiscountId));
                            }
                        }
                        catch (Exception)
                        {

                        }

                    }
                    #endregion

                    #region create entry in Pay SMT add paymentid, transactionid and amount
                    if (PaymentId > 0 && ispaymentEntryEnteredNow)
                    {
                        try
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_PaySmt;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PaymentId", PaymentId);
                            // SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                            SqlCommand.Parameters.AddWithValue("@TransProcId", TransProcId);
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                            if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                SqlCommand.Parameters.AddWithValue("@Amount", -Amount);
                            }
                            else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                            {
                                SqlCommand.Parameters.AddWithValue("@Amount", Amount);
                            }
                            SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@ProcessDate", drRow["PaymentDate"]);
                            SqlCommand.ExecuteScalar();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment PaySmt with PaymentId=" + PaymentId.ToString() + ",PaymentMode=" + drRow["PaymentMode"].ToString() + ",TransProcId=" + Convert.ToString(TransProcId));
                            ispaymentEntryEnteredNow = false;
                        }
                        catch (Exception ex)
                        {
                        }


                        if (Convert.ToDecimal(drRow["Discount"]) > 0)
                        {
                            try
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_PaySmt;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PaymentId", DiscountId);
                                SqlCommand.Parameters.AddWithValue("@TransProcId", TransProcId);
                                SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                                SqlCommand.Parameters.AddWithValue("@Amount", "-" + Convert.ToDecimal(drRow["Discount"]).ToString());
                                SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);
                                SqlCommand.Parameters.AddWithValue("@ProcessDate", drRow["PaymentDate"]);
                                SqlCommand.ExecuteScalar();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Insert Patient Payment TransProcId=" + TransProcId.ToString() + " and PaymentId=" + Convert.ToString(DiscountId));
                            }
                            catch (Exception)
                            {
                            }

                        }
                    }
                    #endregion

                    #region Create entry in Transaction Audit Trial for Transaction Id and payment Id
                    if (TransProcId > 0 && PaymentId > 0)
                    {
                        try
                        {
                            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                            {
                                Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
                            }
                            //@PaymentDate,NULL,@Description ,@PatientEHRId, @Amount,@PaymentDate,@PaymentId
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_TransAudTr;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@AudDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                            if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                SqlCommand.Parameters.AddWithValue("@Amount", -Amount);
                            }
                            else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                            {
                                SqlCommand.Parameters.AddWithValue("@Amount", Amount);
                            }
                            SqlCommand.Parameters.AddWithValue("@TransDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@PaymentId", PaymentId);
                            SqlCommand.ExecuteScalar();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Log To TransAudTr with UserName=" + Utility.EHR_UserLogin_ID + ",PaymentId = " + PaymentId.ToString() + " and PatientEHRId=" + Convert.ToString(drRow["PatientEHRId"]));
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    #endregion

                    #region Create log entry if transacton entry created or already exists
                    if ((drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID") && TransId > 0)
                    {
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
                        }
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_PatInfoAccessRecord;
                        SqlCommand.Parameters.Clear();

                        SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                        SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                        // SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                        SqlCommand.ExecuteScalar();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Log To Patient Info Access Record  PatientEHRId=" + Convert.ToString(drRow["PatientEHRId"]) + ",UserName=" + Utility.EHR_UserLogin_ID);
                    }
                    #endregion
                }
                #endregion

                drRow["PaymentEHRId"] = PaymentId.ToString();
                drRow["PaymentUpdatedEHR"] = true;
                drRow["PaymentUpdatedEHRDateTime"] = DateTime.Now.ToString();

                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                if (issavedlocalstatus)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient PaymentLog To Local  PaymentEHRId=" + Convert.ToString(drRow["PaymentEHRId"]));
                }
                if (PaymentId > 0 || noteId > 0)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", ServiceInstalltionId.Trim(), drRow["Clinic_Number"].ToString().Trim(), "", PaymentId.ToString(), noteId.ToString(), "", "", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
            }
            catch (Exception ex1)
            {
                Utility.WriteToErrorLogFromAll("error in payment " + ex1.Message);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", ServiceInstalltionId.Trim(), drRow["Clinic_Number"].ToString().Trim(), ex1.Message, PaymentId.ToString(), "", "", ex1.Message, Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
        }

        public static void SaveAditPayPaymentToEHR(DataRow drRow, string providerId, string DbConnectionString, string ServiceInstalltionId, decimal Amount, Int64 paymentModeId, Int64 AdjustmenttypeId, Int64 refundId, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
        {
            Int64 noteId = 0;
            Int32 TransId = 0;
            Int64 TransProcId = 0;
            Int64 PaymentId = 0;
            Int64 DiscountId = 0;
            bool istransEntryEnteredNow = false;
            bool ispaymentEntryEnteredNow = false;
            DateTime PostDate;
            PostDate = DateTime.Now;
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            string sqlSelect = string.Empty;
            try
            {
                #region Create Payment Log entry in case Payment setting is Log or Both
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 1 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                {

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                    SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPaymentLog;
                    SqlCommand.Parameters.Clear();
                    SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                    SqlCommand.Parameters.AddWithValue("@User_EHR_Id", Convert.ToString(Utility.EHR_UserLogin_ID));
                    SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                    SqlCommand.Parameters.AddWithValue("@PatientNote", drRow["template"]);
                    SqlCommand.Parameters.AddWithValue("@Method", 7);
                    noteId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                    if (noteId > 0)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Save Patient Payment Log  PatientEHRId=" + drRow["PatientEHRId"] + " and User_EHR_Id=" + Convert.ToString(Utility.EHR_UserLogin_ID) + " with Log id " + noteId.ToString());
                    }
                }
                #endregion

                #region Create Payment Entry if Setting is Payment or Both
                if (Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 2 || Convert.ToInt16(drRow["EHRSyncPaymentLog"]) == 3)
                {
                    #region If Payment Mode is Paid or Partial Paid
                    if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                    {
                        try
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.checktransentryexist;
                            SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", providerId);
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", Convert.ToInt16(drRow["PatientEHRId"]));
                            SqlCommand.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"].ToString());
                            //SqlCommand.Parameters.AddWithValue("@ProviderId", 0);
                            SqlCommand.Parameters.AddWithValue("@PostDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                            TransId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                            if (TransId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Check trans entry exist for PatientEHRId=" + drRow["PatientEHRId"] + ",PaymentMode=" + drRow["PaymentMode"].ToString() + " and TransId=" + Convert.ToString(TransId));
                            }
                            if (TransId == 0)
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_Tranc;
                                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", providerId);
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                                SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                                SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                                //SqlCommand.Parameters.AddWithValue("@ProviderId", drRow["ProviderEHRId"].ToString());
                                SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);

                                TransId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                if (TransId > 0)
                                {
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment Tranc for PatientEHRId=" + drRow["PatientEHRId"] + ",ProviderId=" + providerId + "and TransId=" + Convert.ToString(TransId));
                                }
                                istransEntryEnteredNow = true;
                            }


                        }
                        catch (Exception ex)
                        {
                            Utility.WriteToSyncLogFile_All("error in InsertPatientPayment_Tranc=   " + ex.Message);

                            //throw;
                        }


                        if (TransId > 0 && istransEntryEnteredNow)
                        {
                            try
                            {
                                //@TransId ,'CREDT' , @CurrentDate,'C', @ProviderId, 0.00, @PostDate ,  @ProcessDate
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_TransProc;
                                SqlCommand.CommandText = SqlCommand.CommandText.Replace("@ProviderId", providerId);
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@TransId", TransId);
                                SqlCommand.Parameters.AddWithValue("@CurrentDate", PostDate);
                                //SqlCommand.Parameters.AddWithValue("@ProviderId", drRow["ProviderEHRId"]);
                                SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);
                                SqlCommand.Parameters.AddWithValue("@ProcessDate", drRow["PaymentDate"]);
                                TransProcId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                if (TransProcId > 0)
                                {
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment TransProc for  ProviderId=" + providerId + " and TransId=" + Convert.ToString(TransId));
                                }
                                istransEntryEnteredNow = false;
                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToSyncLogFile_All("7 error in InsertPatientPayment_TransProc=   " + ex.Message);
                                // deleteduplicates("tbl_trans", "fld_auto_intTransId",TransId);
                                // continue;
                                //throw;

                            }

                        }


                    }
                    #endregion

                    #region if Payment Mode if Refund or Partial Refund
                    else if (drRow["PaymentMode"].ToString().ToUpper() == "REFUNDED" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-REFUNDED")
                    {
                        CommonDB.SqlServerCommand("select max(fld_auto_intTransProcId) from tbl_TransProc where fld_intTransId= (select max(fld_auto_intTransId) from tbl_Trans where flt_intPatId=" + drRow["PatientEHRId"] + ")", conn, ref SqlCommand, "txt");

                        TransProcId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        if (TransProcId > 0)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Get TransProcId =" + Convert.ToString(TransProcId) + " for PatientEHRId=" + drRow["PatientEHRId"]);
                        }
                    }
                    #endregion

                    try
                    {
                        #region Check and Create payment Entry
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchClearDentQRY.checkpaymententryexist;
                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                        SqlCommand.Parameters.AddWithValue("@Description", drRow["template"].ToString());
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                        SqlCommand.Parameters.AddWithValue("@PostDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                        //SqlCommand.Parameters.AddWithValue("@PatientPaymentWebId", 0);
                        if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                        {
                            SqlCommand.Parameters.AddWithValue("@PaymentMode", paymentModeId);
                            SqlCommand.Parameters.AddWithValue("@fldblnisadj", 0);
                            SqlCommand.Parameters.AddWithValue("@fldbytAdjType", DBNull.Value);
                        }
                        else
                        {
                            SqlCommand.Parameters.AddWithValue("@PaymentMode", DBNull.Value);
                            SqlCommand.Parameters.AddWithValue("@fldblnisadj", 1);
                            SqlCommand.Parameters.AddWithValue("@fldbytAdjType", refundId);
                        }
                        PaymentId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                        Utility.WriteToSyncLogFile_All("paymentid= " + PaymentId.ToString());


                        if (PaymentId == 0)
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_Payment;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PaymentDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"].ToString());
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                            SqlCommand.Parameters.AddWithValue("@PostDate", Convert.ToDateTime(drRow["PaymentDate"]).ToString("MM/dd/yyyy HH:mm:ss"));
                            //SqlCommand.Parameters.AddWithValue("@PatientPaymentWebId", 0);
                            if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                SqlCommand.Parameters.AddWithValue("@PaymentMode", paymentModeId);
                                SqlCommand.Parameters.AddWithValue("@fldblnisadj", 0);
                                SqlCommand.Parameters.AddWithValue("@fldbytAdjType", DBNull.Value);
                            }
                            else
                            {
                                SqlCommand.Parameters.AddWithValue("@PaymentMode", DBNull.Value);
                                SqlCommand.Parameters.AddWithValue("@fldblnisadj", 1);
                                SqlCommand.Parameters.AddWithValue("@fldbytAdjType", refundId);
                            }
                            PaymentId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                            if (PaymentId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment PatientEHRId=" + drRow["PatientEHRId"].ToString() + ",PaymentMode=" + drRow["PaymentMode"].ToString() + " Payment Id " + PaymentId.ToString());
                            }
                            ispaymentEntryEnteredNow = true;
                        }
                        #endregion

                    }
                    catch (Exception)
                    {
                    }

                    #region Create Discount Entry if discount is greater then zero
                    if (Convert.ToDecimal(drRow["Discount"]) > 0)
                    {
                        try
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_Payment;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                            SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@PaymentMode", paymentModeId);
                            SqlCommand.Parameters.AddWithValue("@fldblnisadj", 1);
                            SqlCommand.Parameters.AddWithValue("@fldbytAdjType", AdjustmenttypeId);
                            DiscountId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                            if (DiscountId > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Insert Patient Payment for PatientEHRId=" + drRow["PatientEHRId"] + ",PaymentMode=" + paymentModeId + "  and DiscountId=" + Convert.ToString(DiscountId));
                            }
                        }
                        catch (Exception)
                        {

                        }

                    }
                    #endregion

                    #region Create entry in PaySMT if payment is already done
                    if (PaymentId > 0 && ispaymentEntryEnteredNow)
                    {
                        try
                        {
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_PaySmt;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@PaymentId", PaymentId);
                            // SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                            SqlCommand.Parameters.AddWithValue("@TransProcId", TransProcId);
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                            if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                SqlCommand.Parameters.AddWithValue("@Amount", -Amount);
                            }
                            else
                            {
                                SqlCommand.Parameters.AddWithValue("@Amount", Amount);
                            }
                            SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@ProcessDate", drRow["PaymentDate"]);
                            SqlCommand.ExecuteScalar();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient Payment PaySmt with PaymentId=" + PaymentId.ToString() + ",PaymentMode=" + drRow["PaymentMode"].ToString() + ",TransProcId=" + Convert.ToString(TransProcId));
                            ispaymentEntryEnteredNow = false;
                        }
                        catch (Exception ex)
                        {
                        }


                        if (Convert.ToDecimal(drRow["Discount"]) > 0)
                        {
                            try
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchClearDentQRY.InsertPatientPayment_PaySmt;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PaymentId", DiscountId);
                                SqlCommand.Parameters.AddWithValue("@TransProcId", TransProcId);
                                SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                                SqlCommand.Parameters.AddWithValue("@Amount", "-" + Convert.ToDecimal(drRow["Discount"]).ToString());
                                SqlCommand.Parameters.AddWithValue("@PostDate", drRow["PaymentDate"]);
                                SqlCommand.Parameters.AddWithValue("@ProcessDate", drRow["PaymentDate"]);
                                SqlCommand.ExecuteScalar();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Insert Patient Payment TransProcId=" + TransProcId.ToString() + " and PaymentId=" + Convert.ToString(DiscountId));
                            }
                            catch (Exception)
                            {
                            }

                        }
                    }
                    #endregion

                    #region Create enetry in Transaction Audit If Payment and Transaction Entry exists
                    if (TransProcId > 0 && PaymentId > 0)
                    {
                        try
                        {
                            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                            {
                                Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
                            }
                            //@PaymentDate,NULL,@Description ,@PatientEHRId, @Amount,@PaymentDate,@PaymentId
                            CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_TransAudTr;
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("@AudDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                            SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                            SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                            if (drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID")
                            {
                                SqlCommand.Parameters.AddWithValue("@Amount", -Amount);
                            }
                            else
                            {
                                SqlCommand.Parameters.AddWithValue("@Amount", Amount);
                            }
                            SqlCommand.Parameters.AddWithValue("@TransDate", drRow["PaymentDate"]);
                            SqlCommand.Parameters.AddWithValue("@PaymentId", PaymentId);
                            SqlCommand.ExecuteScalar();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Log To TransAudTr with UserName=" + Utility.EHR_UserLogin_ID + ",PaymentId = " + PaymentId.ToString() + " and PatientEHRId=" + Convert.ToString(drRow["PatientEHRId"]));
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    #endregion

                    #region Create Enetry in Pat info in case payment mode is Paid Or Partial Padi
                    if ((drRow["PaymentMode"].ToString().ToUpper() == "PAID" || drRow["PaymentMode"].ToString().ToUpper() == "PARTIAL-PAID") && TransId > 0)
                    {
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
                        }
                        CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                        SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_PatInfoAccessRecord;
                        SqlCommand.Parameters.Clear();

                        SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PaymentDate"]);
                        SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                        // SqlCommand.Parameters.AddWithValue("@Description", drRow["template"]);
                        SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                        SqlCommand.ExecuteScalar();
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Log To Patient Info Access Record  PatientEHRId=" + Convert.ToString(drRow["PatientEHRId"]) + ",UserName=" + Utility.EHR_UserLogin_ID);
                    }
                    #endregion
                }
                #endregion

                drRow["PaymentEHRId"] = PaymentId.ToString();
                drRow["PaymentUpdatedEHR"] = true;
                drRow["PaymentUpdatedEHRDateTime"] = DateTime.Now.ToString();

                bool issavedlocalstatus = SynchLocalDAL.Save_PatientPaymentLog_To_Local(drRow, _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                if (issavedlocalstatus)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, "Save Patient PaymentLog To Local  PaymentEHRId=" + Convert.ToString(drRow["PaymentEHRId"]));
                }
                if (PaymentId > 0 || noteId > 0)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", ServiceInstalltionId.Trim(), drRow["Clinic_Number"].ToString().Trim(), "", PaymentId.ToString(), noteId.ToString(), "", "", Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
                }
            }
            catch (Exception ex1)
            {
                Utility.WriteToErrorLogFromAll("error in payment " + ex1.Message);
                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "error", ServiceInstalltionId.Trim(), drRow["Clinic_Number"].ToString().Trim(), ex1.Message, PaymentId.ToString(), "", "", ex1.Message, Convert.ToInt32(drRow["TryInsert"]), _filename_EHR_Payment, _EHRLogdirectory_EHR_Payment);
            }
        }


        public static string Save_PatientSMSCall_Local_To_ClearDent(DataTable dtWebPatientPayment, string DbConnectionString, string ServiceInstalltionId)
        {
            string noteId = "";
            SqlConnection conn = null;
            SqlCommand SqlCommand = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            if (conn.State == ConnectionState.Closed) conn.Open();
            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
            {
                Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
            }
            try
            {
                string sqlSelect = string.Empty;
                if (!dtWebPatientPayment.Columns.Contains("Log_Status"))
                {
                    dtWebPatientPayment.Columns.Add("Log_Status", typeof(string));
                }
                DataTable dtResultCopy = new DataTable();
                int message_type = 0;
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    dtResultCopy = dtWebPatientPayment.Select("Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' and Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                    message_type = 0;
                    foreach (DataRow drRow in dtResultCopy.Rows)
                    {
                        if (drRow["PatientEHRId"] != null && drRow["PatientEHRId"].ToString() != string.Empty && drRow["PatientEHRId"].ToString() != "")
                        {
                            try
                            {
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchClearDentQRY.InsertSMSCallLog;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                                SqlCommand.Parameters.AddWithValue("@PaymentDate", drRow["PatientSMSCallLogDate"]);
                                SqlCommand.Parameters.AddWithValue("@PatientNote", drRow["text"]);
                                SqlCommand.Parameters.AddWithValue("@EHR_User_Id", Utility.EHR_UserLogin_ID);
                                if (drRow["message_type"].ToString().ToLower() == "call" || drRow["message_type"].ToString().ToLower() == "appointment_reminder_call")
                                {
                                    message_type = GetcontactMethod(true);
                                    SqlCommand.Parameters.AddWithValue("@Method", message_type == 0 ? 3 : message_type);
                                }
                                else
                                {
                                    message_type = GetcontactMethod(false);
                                    SqlCommand.Parameters.AddWithValue("@Method", message_type == 0 ? 10 : message_type);
                                }
                                // Utility.WriteToSyncLogFile_All("Log Insert Query " + SqlCommand.CommandText.ToString());
                                noteId = SqlCommand.ExecuteScalar().ToString();
                                drRow["LogEHRId"] = noteId.ToString();
                                drRow["Log_Status"] = "completed";

                                #region adding adit user logs for appointment
                                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCommand, "txt");
                                SqlCommand.CommandText = SynchClearDentQRY.InsertLogTo_TransAudTr;
                                SqlCommand.Parameters.Clear();
                                SqlCommand.Parameters.AddWithValue("@AudDate", DateTime.Now);
                                SqlCommand.Parameters.AddWithValue("@Description", "made by Adit");
                                SqlCommand.Parameters.AddWithValue("@PatientEHRId", drRow["PatientEHRId"]);
                                SqlCommand.Parameters.AddWithValue("@UserName", Utility.EHR_UserLogin_ID);
                                SqlCommand.Parameters.AddWithValue("@Amount", 0.00);
                                SqlCommand.Parameters.AddWithValue("@TransDate", DateTime.Now);
                                SqlCommand.Parameters.AddWithValue("@PaymentId", DBNull.Value);
                                SqlCommand.ExecuteScalar();
                                #endregion

                            }
                            catch (Exception ex1)
                            {
                                Utility.WriteToSyncLogFile_All("Log Insert Query Err_" + ex1.Message.ToString());
                                if (ex1.InnerException.Message.Length >= 100)
                                {
                                    drRow["Log_Status"] = "Err_" + ex1.InnerException.Message.Substring(0, 100);
                                }
                                else
                                {
                                    drRow["Log_Status"] = "Err_" + ex1.InnerException.Message.ToString();
                                }
                            }
                        }
                    }
                    if (dtResultCopy.Rows.Count > 0)
                    {
                        if (dtResultCopy.Select("LogType = 0").Count() > 0)
                        {
                            SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy.Select("LogType = 0").CopyToDataTable(), "completed", ServiceInstalltionId.Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                        if (dtResultCopy.Select("LogType = 1").Count() > 0)
                        {
                            SynchLocalDAL.CallPatientFollowUpStatusCompleted(dtResultCopy.Select("LogType = 1").CopyToDataTable(), "completed", ServiceInstalltionId.Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                    }

                }
                return noteId;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error In Save_PatientSMSCall_Local_To_ClearDent in final catch    " + ex.Message);
                return "";
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static int GetcontactMethod(Boolean IsCall)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            int Methodid = 0;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                if (IsCall)
                {
                    SqlSelect = "If Not EXISTS (SELECT [fld_bytContactId] FROM [tbl_CfgContactMethod] where [fld_strContactMethod] = @Method) select 3 ELSE (SELECT [fld_bytContactId] FROM [tbl_CfgContactMethod] where [fld_strContactMethod] = @Method)";
                }
                else
                {
                    SqlSelect = "If Not EXISTS (SELECT [fld_bytContactId] FROM [tbl_CfgContactMethod] where [fld_strContactMethod] = @Method) select 10 ELSE (SELECT [fld_bytContactId] FROM [tbl_CfgContactMethod] where [fld_strContactMethod] = @Method)";
                }
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                SqlCommand.Parameters.Clear();
                SqlCommand.Parameters.AddWithValue("Method", IsCall ? "M Phone" : "Text");
                Methodid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                return Methodid;
            }
            catch (Exception ex)
            {
                Methodid = 0;
                return Methodid;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetClearDentPatientImagesData(string dbstring)
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            DataTable dsResult = new DataTable();
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetClearDentPatientImagesData;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCommand, ref SqlDa);
                SqlDa.Fill(dsResult);
                return dsResult;
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

        public static void DeleteDuplicatePatientLog(string DbConnectionString, string ServiceInstalltionId)
        {
            //  bool is_Record_Update = false;
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.ClearDentSQLConnectionServer(ref conn);


            DateTime datetimeTemp = DateTime.Now;
            if (conn.State == ConnectionState.Closed) conn.Open();
            //rooja           
            try
            {
                string sqlSelect = string.Empty;

                DataTable dtDuplicateRecords = GetDuplicateRecords(DbConnectionString, ServiceInstalltionId);
                if (dtDuplicateRecords != null)
                {
                    if (dtDuplicateRecords.Rows.Count > 0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = GetClearDentUserLoginId();
                        }
                    }
                }

                foreach (DataRow drRow in dtDuplicateRecords.Rows)
                {

                    try
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        CommonDB.SqlServerCommand("", conn, ref SqlCommand, "txt");
                        if (drRow["Mobile"].ToString() == "")
                        {
                            SqlCommand.CommandText = SynchClearDentQRY.DeleteDuplicateLogsBlankMobileWithDate;
                        }
                        else
                        {
                            SqlCommand.CommandText = SynchClearDentQRY.DeleteDuplicateLogsWithDate;
                        }

                        SqlCommand.Parameters.Clear();
                        SqlCommand.Parameters.AddWithValue("PatientEHRId", drRow["fld_intPatId"].ToString());
                        datetimeTemp = Convert.ToDateTime(drRow["fld_dtmContactDateTime"].ToString());

                        SqlCommand.Parameters.AddWithValue("LogDate", Convert.ToDateTime(datetimeTemp.ToLongDateString()));
                        SqlCommand.Parameters.AddWithValue("Method", drRow["fld_shtMethod"].ToString());
                        //rooja
                        SqlCommand.Parameters.AddWithValue("@EHR_User_Id", Utility.EHR_UserLogin_ID);
                        SqlCommand.Parameters.AddWithValue("Note", drRow["fld_strNote"].ToString());
                        SqlCommand.Parameters.AddWithValue("LogId", drRow["logId"].ToString());
                        SqlCommand.ExecuteNonQuery();

                    }
                    catch (Exception ex1)
                    {
                        Utility.WriteToSyncLogFile_All("Error While Delete " + ex1.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                // return "";
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetDuplicateRecords(string DbConnectionString, string ServiceInstalltionId)
        {
            //DateTime ToDate = Utility.LastSyncDateAditServer;

            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();

            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = "";
                SqlSelect = SynchClearDentQRY.GetDuplicateRecords;
                CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                //SqlCommand.Parameters.AddWithValue("ToDate", ToDate.ToString("yyyy-MM-dd"));
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

        public static bool SavePatientMedicationLocalToClearDent(string ServiceInstalltionId, ref bool IsSaveMedication, ref string SavePatientEHRID, string strPatientFormID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            Int64 MedicationPatientId = 0;
            Int64 MedicationNum = 0;
            string SqlSelect = "";
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            DataTable dtMedication = new DataTable();
            DataTable dtPatientMedication = new DataTable();
            Int64 GroupID = 0;
            SavePatientEHRID = "";
            try
            {
                DataTable dtPatientMedicationResponseAll = SynchLocalDAL.GetLocalPatientFormMedicationResponseToSaveINEHR(ServiceInstalltionId, strPatientFormID);
                if (dtPatientMedicationResponseAll != null)
                {
                    if (dtPatientMedicationResponseAll.Rows.Count > 0)
                    {
                        dtMedication = GetCleardentMedicationData();
                        dtPatientMedication = GetCleardentPatientMedicationData("");
                        //List<DataTable> result = dtPatientMedicationResponseAll.AsEnumerable()
                        //                        .GroupBy(row => row.Field<string>("PatientEHRId"))
                        //                        .Select(g => g.CopyToDataTable())
                        //                        .ToList();
                        List<DataTable> result = dtPatientMedicationResponseAll.AsEnumerable()
                                                .GroupBy(row => row.Field<string>("PatientForm_Web_ID"))
                                                .Select(g => g.CopyToDataTable())
                                                .ToList();
                        foreach (DataTable dt in result)
                        {
                            int SeqNum = 0;
                            GroupID = 0;
                            DataTable[] splittedtables = dt.AsEnumerable()
                                                        .Select((row, index) => new { row, index })
                                                        .GroupBy(x => x.index / 4)  // integer division, the fractional part is truncated
                                                        .Select(g => g.Select(x => x.row).CopyToDataTable())
                                                        .ToArray();
                            foreach (DataTable dtPatientMedicationResponse in splittedtables)
                            {
                                SeqNum = 0;
                                GroupID = 0;
                                if (conn.State == ConnectionState.Closed) conn.Open();

                                foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                                {
                                    MedicationNum = 0;
                                    MedicationPatientId = 0;

                                    if (dr["Medication_EHR_Id"] == DBNull.Value) dr["Medication_EHR_Id"] = "0";
                                    if (dr["Medication_EHR_Id"].ToString().Trim() == "" || dr["Medication_EHR_Id"].ToString().Trim() == "0")
                                    {
                                        Utility.WriteToErrorLogFromAll("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());
                                        continue;

                                        DataRow[] drMedRow = dtMedication.Copy().Select("Medication_Name = '" + dr["Medication_Name_Org"].ToString().Trim() + "'");

                                        if (drMedRow.Length > 0)
                                        {
                                            MedicationNum = Convert.ToInt64(drMedRow[0]["Medication_EHR_ID"]);
                                        }

                                        if (MedicationNum <= 0)
                                        {
                                            Utility.WriteToErrorLogFromAll("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());
                                            continue;
                                            //throw new Exception("Medication '" + dr["Medication_Name"].ToString().Trim() + "' not found. Medication ID: " + dr["Medication_EHR_Id"].ToString().Trim());

                                            //SqlSelect = SynchClearDentQRY.InsertMedication;
                                            //CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                            //SqlCommand.Parameters.Clear();
                                            ////@MedicationName,@GenericNum,@MedicationNote
                                            //SqlCommand.Parameters.AddWithValue("MedicationName", dr["Medication_Name"].ToString().Trim());
                                            //SqlCommand.Parameters.AddWithValue("MedicationNote", dr["Medication_Note"].ToString().Trim());
                                            //MedicationNum = Convert.ToInt64(SqlCommand.ExecuteScalar());

                                            //DataRow newRow = dtMedication.NewRow();
                                            //newRow["Medication_EHR_ID"] = MedicationNum;
                                            //newRow["Medication_Name"] = dr["Medication_Name"].ToString().Trim();
                                            //newRow["Medication_Description"] = dr["Medication_Name"].ToString().Trim();
                                            //newRow["Medication_Notes"] = dr["Medication_Note"].ToString().Trim();
                                            //dtMedication.Rows.Add(newRow);
                                            //dtMedication.AcceptChanges();
                                        }
                                        dr["Medication_EHR_Id"] = MedicationNum.ToString();
                                    }
                                    else
                                    {
                                        MedicationNum = Convert.ToInt64(dr["Medication_EHR_Id"]);
                                    }

                                    if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "0";
                                    if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" && dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                                    {
                                        MedicationPatientId = Convert.ToInt64(dr["PatientMedication_EHR_Id"]);
                                    }

                                    if (MedicationPatientId <= 0)
                                    {
                                        string strSelect = "Patient_EHR_ID = " + dr["PatientEHRID"].ToString().Trim() +
                                                    " And Medication_EHR_ID = " + MedicationNum + " And is_active='True' ";

                                        DataRow[] drPatMedRow = dtPatientMedication.Copy().Select(strSelect);
                                        if (drPatMedRow.Length > 0)
                                        {
                                            MedicationPatientId = Convert.ToInt64(drPatMedRow[0]["PatientMedication_EHR_ID"].ToString().Trim());
                                        }
                                    }

                                    if (MedicationPatientId <= 0)
                                    {
                                        SqlSelect = SynchClearDentQRY.InsertPatientMedication;
                                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                        //@PatNum,@MedicationNum,@PatNote,@ProvNum
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["PatientEHRID"].ToString().Trim());
                                        SqlCommand.Parameters.AddWithValue("MedicationNote", dr["Medication_Note"].ToString().Trim());
                                        SqlCommand.Parameters.AddWithValue("MedicationName", dr["Medication_Name_Org"].ToString().Trim());
                                        DataRow[] drMedRow = dtMedication.Copy().Select("Medication_EHR_ID = " + dr["Medication_EHR_ID"].ToString().Trim());
                                        if (drMedRow.Length > 0)
                                        { SqlCommand.Parameters.AddWithValue("@Medication_SIG", drMedRow[0]["Medication_Sig"].ToString().Trim()); }
                                        else
                                        { SqlCommand.Parameters.AddWithValue("@Medication_SIG", ""); }

                                        //SqlCommand.Parameters.AddWithValue("Provider_EHR_ID", ProviderID);
                                        MedicationPatientId = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                        dr["PatientMedication_EHR_ID"] = MedicationPatientId.ToString().Trim();

                                        DataRow NewRow = dtPatientMedication.NewRow();
                                        NewRow["Patient_EHR_ID"] = dr["PatientEHRID"].ToString();
                                        NewRow["Medication_EHR_ID"] = MedicationNum.ToString();
                                        NewRow["PatientMedication_EHR_ID"] = MedicationPatientId.ToString();
                                        NewRow["Medication_Name"] = dr["Medication_Name_Org"].ToString();
                                        //NewRow["Medication_Type"] = "Drug";                                
                                        NewRow["Medication_Note"] = dr["Medication_Note"].ToString();
                                        NewRow["is_active"] = "True";
                                        dtPatientMedication.Rows.Add(NewRow);
                                        dtPatientMedication.AcceptChanges();

                                        if (SeqNum > 3 || GroupID <= 0)
                                        {
                                            SqlSelect = "Select Max(fld_intPatPreGroupId) + 1 as GroupID from tbl_PatPresGroup";
                                            CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                            SqlCommand.Parameters.Clear();
                                            GroupID = Convert.ToInt64(SqlCommand.ExecuteScalar());
                                            SeqNum = 0;
                                        }

                                        SqlSelect = "INSERT INTO [dbo].[tbl_PatPresGroup]([fld_intPatPreGroupId],[fld_bytGroupSeq],[fld_intPatPreId]) VALUES(@GroupId, @GroupSeq, @PatMedicationId)";
                                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("GroupId", GroupID);
                                        SqlCommand.Parameters.AddWithValue("GroupSeq", SeqNum);
                                        SqlCommand.Parameters.AddWithValue("PatMedicationId", MedicationPatientId);
                                        SqlCommand.ExecuteNonQuery();
                                        SeqNum = SeqNum + 1;
                                    }
                                    else
                                    {
                                        string strMedicationName = "";
                                        string strSig = "";
                                        string strNote = dr["Medication_Note"].ToString().Trim();

                                        DataRow[] drPatMedRow = dtPatientMedication.Copy().Select("PatientMedication_EHR_ID = " + MedicationPatientId);

                                        MedicationNum = Convert.ToInt64(dr["Medication_EHR_ID"].ToString().Trim());
                                        DataRow[] drMedRow = dtMedication.Copy().Select("Medication_EHR_Id = " + MedicationNum);

                                        if (drMedRow.Length > 0)
                                        {
                                            strMedicationName = drMedRow[0]["Medication_Name"].ToString().Trim();
                                            strSig = drMedRow[0]["Medication_SIG"].ToString().Trim();
                                        }
                                        else
                                        {
                                            strMedicationName = dr["Medication_Name_Org"].ToString().Trim();
                                            strSig = drPatMedRow[0]["Medication_SIG"].ToString().Trim();
                                        }

                                        SqlSelect = SynchClearDentQRY.UpdatePatientMedicationNotes;
                                        CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                                        SqlCommand.Parameters.Clear();
                                        SqlCommand.Parameters.AddWithValue("PatientMedication_EHR_ID", MedicationPatientId);
                                        SqlCommand.Parameters.AddWithValue("MedicationNote", strNote);
                                        SqlCommand.Parameters.AddWithValue("MedicationName", strMedicationName);
                                        SqlCommand.Parameters.AddWithValue("Medication_SIG", strSig);
                                        //SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["PatientEHRId"].ToString().Trim());
                                        SqlCommand.ExecuteNonQuery();
                                    }
                                    if (!SavePatientEHRID.Contains("'" + dr["PatientEHRID"].ToString() + "'"))
                                    {
                                        SavePatientEHRID = SavePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                                    }
                                    SynchLocalDAL.UpdateMedicationEHR_Updateflg(dr["MedicationResponse_Local_ID"].ToString(), MedicationPatientId.ToString(), dr["PatientEHRID"].ToString(), dr["Medication_EHR_Id"].ToString(), "1");
                                }
                            }
                        }
                        IsSaveMedication = true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Save Patient Medication to ClearDent Error: " + ex.Message);
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }
        public static bool DeletePatientMedicationLocalToClearDent(string ServiceInstalltionId, ref bool IsDeletedMedication, ref string DeletePatientEHRID, string strPatientFormID = "")
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            string SqlSelect = "";
            DeletePatientEHRID = "";
            try
            {
                DataTable dtPatientMedicationResponse = SynchLocalDAL.GetLocalPatientFormMedicationRemovedResponseToSaveINEHR(ServiceInstalltionId, strPatientFormID);
                if (dtPatientMedicationResponse != null)
                {
                    foreach (DataRow dr in dtPatientMedicationResponse.Rows)
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        if (dr["PatientMedication_EHR_Id"] == DBNull.Value) dr["PatientMedication_EHR_Id"] = "";

                        if (dr["PatientMedication_EHR_Id"].ToString().Trim() != "" || dr["PatientMedication_EHR_Id"].ToString().Trim() != "0")
                        {
                            SqlSelect = SynchClearDentQRY.DeletePatientMedication;
                            CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("PatientMedication_EHR_ID", dr["PatientMedication_EHR_ID"].ToString().Trim());
                            SqlCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["PatientEHRID"].ToString().Trim());
                            SqlCommand.ExecuteNonQuery();

                            SqlSelect = "Delete From [tbl_PatPresGroup] where fld_intPatPreId = @PatientMedication_EHR_ID;";
                            CommonDB.SqlServerCommand(SqlSelect, conn, ref SqlCommand, "txt");
                            SqlCommand.Parameters.Clear();
                            SqlCommand.Parameters.AddWithValue("PatientMedication_EHR_ID", dr["PatientMedication_EHR_ID"].ToString().Trim());
                            SqlCommand.ExecuteNonQuery();

                            if (!DeletePatientEHRID.ToUpper().Trim().Contains("'" + dr["PatientEHRID"].ToString().Trim() + "'"))
                            {
                                DeletePatientEHRID = DeletePatientEHRID + "'" + dr["PatientEHRID"].ToString() + "',";
                            }
                            SynchLocalDAL.UpdateRemovedMedicationEHR_Updateflg(dr["MedicationRemovedResponse_Local_ID"].ToString(), dr["PatientMedication_EHR_Id"].ToString(), dr["PatientEHRID"].ToString(), ServiceInstalltionId);
                        }
                        IsDeletedMedication = true;
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

        #region Insurance

        public static DataTable GetClearDentInsuranceData()
        {
            SqlConnection conn = null;
            SqlCommand SqlCommand = new SqlCommand();
            SqlDataAdapter SqlDa = null;
            CommonDB.ClearDentSQLConnectionServer(ref conn);
            try
            {
                SqlCommand.CommandTimeout = 200;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlSelect = SynchClearDentQRY.GetCleardentInsuranceData;
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

        public static bool Save_Insurance_ClearDent_To_Local(DataTable dtClearDentInsurance)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                // SqlCetx = conn.BeginTransaction();
                try
                {
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtClearDentInsurance.Rows)
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
                                SqlCeCommand.Parameters.AddWithValue("Insurance_EHR_ID", dr["fld_auto_intCarrId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Insurance_Name", dr["fld_strName"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address", dr["fld_strAddr"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Address2", "");
                                SqlCeCommand.Parameters.AddWithValue("City", "");
                                SqlCeCommand.Parameters.AddWithValue("State", "");
                                SqlCeCommand.Parameters.AddWithValue("Zipcode", "");
                                SqlCeCommand.Parameters.AddWithValue("Phone", dr["fld_strTel"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ElectId", dr["fld_intEDICarrIdNo"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("EmployerName", "");
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", false);
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", "1");
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    //  SqlCetx.Commit();
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

    }
}

