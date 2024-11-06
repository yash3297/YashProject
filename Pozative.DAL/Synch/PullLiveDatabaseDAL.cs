using Pozative.QRY;
using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.SqlClient;
using RestSharp;
using System.Net;
using System.Net.Security;

namespace Pozative.DAL
{
    public class PullLiveDatabaseDAL
    {

        public static DataTable GetLiveDB_AppointmentDetail()
        {

            DataTable dtAppt = new DataTable();

            try
            {
                dtAppt.Columns.Add("Appt_Web_ID", typeof(string));
                dtAppt.Columns.Add("Location_ID", typeof(string));
                dtAppt.Columns.Add("Operatory_Name", typeof(string));
                dtAppt.Columns.Add("Provider_Name", typeof(string));
                dtAppt.Columns.Add("Last_Name", typeof(string));
                dtAppt.Columns.Add("First_Name", typeof(string));
                dtAppt.Columns.Add("MI", typeof(string));
                dtAppt.Columns.Add("Home_Contact", typeof(string));
                dtAppt.Columns.Add("Mobile_Contact", typeof(string));
                dtAppt.Columns.Add("Email", typeof(string));
                dtAppt.Columns.Add("Address", typeof(string));
                dtAppt.Columns.Add("City", typeof(string));
                dtAppt.Columns.Add("ST", typeof(string));
                dtAppt.Columns.Add("Zip", typeof(string));
                dtAppt.Columns.Add("ApptType", typeof(string));
                dtAppt.Columns.Add("Appt_DateTime", typeof(DateTime));
                dtAppt.Columns.Add("Status", typeof(string));
                dtAppt.Columns.Add("Patient_Status", typeof(string));
                dtAppt.Columns.Add("Appointment_Status", typeof(string));
                dtAppt.Columns.Add("Is_Appt", typeof(string));

                //string[] lines = File.ReadAllLines("E:\\Appointment\\Patient.txt");

                string AppointmentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string[] lines = File.ReadAllLines(AppointmentFolder + "\\Appointment\\Patient.txt");

                dtAppt.Rows.Add(lines[0].ToString(), Int64.Parse(lines[1].ToString()), lines[2].ToString(), lines[3].ToString(), lines[4].ToString(),
                                         lines[5].ToString(), lines[6].ToString(), lines[7].ToString(), lines[8].ToString(), lines[9].ToString(),
                                         lines[10].ToString(), lines[11].ToString(), lines[12].ToString(), lines[13].ToString(), lines[14].ToString(),
                                         lines[15].ToString(), Convert.ToDateTime(lines[16].ToString()), lines[17].ToString(), lines[18].ToString(),
                                         lines[19].ToString(), lines[20].ToString());

                File.Delete(AppointmentFolder + "\\Appointment\\Patient.txt");
                return dtAppt;



            }
            catch (Exception ex)
            {
                string exs = ex.Message;
                return dtAppt;

            }
            finally
            {

            }

        }

        public static string GetLiveRecord(string TableName, string LocationId)
        {
            string strApiLocOrg = LiveDatabaseAPI.LiveRecord_Pull_API(TableName, LocationId);
            return strApiLocOrg;
        }

        public static string GetTreatmentDocFromWeb(string TableName, string LocationId)
        {
            string strApiLocOrg = LiveDatabaseAPI.LiveRecord_Pull_API(TableName, LocationId);
            return strApiLocOrg;
        }

        //rooja        
        public static string UpdateStatusInsuranceCarrierDoc(string TableName)
        {
            string strApiLocOrg = LiveDatabaseAPI.LiveRecord_Pull_API(TableName, "");
            return strApiLocOrg;
        }

        public static string UpdateStatusTreatmentDoc(string TableName)
        {
            string strApiLocOrg = LiveDatabaseAPI.LiveRecord_Pull_API(TableName,"");
            return strApiLocOrg;
        }

        public static bool Save_Appointment_Live_To_Local(DataTable dtLiveAppointment,string  _filename_Appointment, string _EHRLogdirectory_Appointment)
        {
            bool _successfullstataus = true;
           
            #region SqlExpressDatabase
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlExCommand = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                //  SqlTransaction SqlExtx;
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlExtx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    string NotiStatus = "0";
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                    //   SqlExCommand.Transaction = SqlExtx;

                    foreach (DataRow dr in dtLiveAppointment.Rows)
                    {
                        if (Convert.ToInt32(dr["InsUptDlt"]) == 1 || Convert.ToInt32(dr["InsUptDlt"]) == 2)
                        {

                            switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                            {
                                case 1:
                                    SqlExCommand.CommandText = SynchLocalQRY.Insert_Appointment;
                                    break;
                                case 2:
                                    SqlExCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_Web_ID;
                                    break;
                                case 3:
                                    SqlExCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                    break;
                            }

                            NotiStatus = "0";
                            SqlExCommand.Parameters.Clear();
                            SqlExCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString());
                            SqlExCommand.Parameters.AddWithValue("patient_ehr_id", (string.IsNullOrEmpty(dr["patient_ehr_id"].ToString()) || dr["patient_ehr_id"].ToString() == "") ? "0" : Convert.ToString(dr["patient_ehr_id"].ToString()));
                            SqlExCommand.Parameters.AddWithValue("Appt_Web_ID", dr["Appt_Web_ID"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString());
                            SqlExCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString());
                            SqlExCommand.Parameters.AddWithValue("MI", dr["MI"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Home_Contact", dr["Home_Contact"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Mobile_Contact", dr["Mobile_Contact"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Email", dr["Email"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Address", dr["Address"].ToString());
                            SqlExCommand.Parameters.AddWithValue("City", dr["City"].ToString());
                            SqlExCommand.Parameters.AddWithValue("ST", dr["ST"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Zip", dr["Zip"].ToString());
                            SqlExCommand.Parameters.AddWithValue("birth_date", dr["birth_date"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Provider_Name", dr["Provider_Name"].ToString());
                            SqlExCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString());
                            SqlExCommand.Parameters.AddWithValue("ApptType", dr["ApptType"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Appt_DateTime", dr["Appt_DateTime"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Appt_EndDateTime", dr["Appt_EndDateTime"].ToString());
                            SqlExCommand.Parameters.AddWithValue("comment", dr["comment"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Status", NotiStatus);
                            SqlExCommand.Parameters.AddWithValue("Patient_Status", dr["Patient_Status"].ToString());
                            SqlExCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Appointment_Status", dr["Appointment_Status"].ToString());
                            SqlExCommand.Parameters.AddWithValue("confirmed_status_ehr_key", dr["confirmed_status_ehr_key"].ToString());
                            SqlExCommand.Parameters.AddWithValue("confirmed_status", dr["confirmed_status"].ToString());
                            SqlExCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
                            SqlExCommand.Parameters.AddWithValue("unschedule_status", "");
                            SqlExCommand.Parameters.AddWithValue("is_ehr_updated", false);
                            SqlExCommand.Parameters.AddWithValue("Is_Appt", dr["Is_Appt"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                            SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                            SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                            SqlExCommand.Parameters.AddWithValue("is_deleted", 0);
                            SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlExCommand.Parameters.AddWithValue("is_asap", 0);
                            SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                            SqlExCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString());
                            SqlExCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                            SqlExCommand.Parameters.AddWithValue("ProcedureDesc", "");
                            SqlExCommand.Parameters.AddWithValue("ProcedureCode", "");
                            SqlExCommand.ExecuteNonQuery();
                            try
                            {
                                Utility.WriteSyncPullLog(_filename_Appointment, _EHRLogdirectory_Appointment, "Save Appointment Live To Local to local " + dr["Appointment_Status"].ToString() + " in EHR for Patient Name : (" + dr["First_Name"].ToString() + "," + dr["Last_Name"].ToString() + ") and appointment_status_ehr_key (" + dr["appointment_status_ehr_key"].ToString() + ") of Appointment : (" + dr["Appt_DateTime"].ToString() + ")");
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    //SqlExtx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlExtx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            #endregion

            #region SqlCEDatabase
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;

                        string NotiStatus = "0";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            foreach (DataRow dr in dtLiveAppointment.Rows)
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"]) == 1 || Convert.ToInt32(dr["InsUptDlt"]) == 2)
                                {

                                    switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                    {
                                        case 1:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;
                                            break;
                                        case 2:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Where_Appt_Web_ID;
                                            break;
                                        case 3:
                                            SqlCeCommand.CommandText = SynchLocalQRY.Delete_Appointment;
                                            break;
                                    }

                                    NotiStatus = "0";
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", (string.IsNullOrEmpty(dr["patient_ehr_id"].ToString()) || dr["patient_ehr_id"].ToString() == "") ? "0" : Convert.ToString(dr["patient_ehr_id"].ToString()));
                                    SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", dr["Appt_Web_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString().Length > 100 ? dr["Last_Name"].ToString().Substring(0, 100) : dr["Last_Name"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString().Length > 100 ? dr["First_Name"].ToString().Substring(0, 100) : dr["First_Name"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("MI", dr["MI"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Home_Contact", dr["Home_Contact"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", dr["Mobile_Contact"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Address", dr["Address"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("City", dr["City"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("ST", dr["ST"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Zip", dr["Zip"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("birth_date", dr["birth_date"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_Name", dr["Provider_Name"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("ApptType", dr["ApptType"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", dr["Appt_DateTime"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", dr["Appt_EndDateTime"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("comment", dr["comment"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Status", NotiStatus);
                                    SqlCeCommand.Parameters.AddWithValue("Patient_Status", dr["Patient_Status"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", dr["appointment_status_ehr_key"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Appointment_Status", dr["Appointment_Status"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", dr["confirmed_status_ehr_key"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("confirmed_status", dr["confirmed_status"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
                                    SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
                                    SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", false);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Appt", dr["Is_Appt"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                    SqlCeCommand.Parameters.AddWithValue("is_asap", 0);
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("appt_treatmentcode", dr["appt_treatmentcode"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("ProcedureDesc", "");
                                    SqlCeCommand.Parameters.AddWithValue("ProcedureCode", "");

                                    SqlCeCommand.ExecuteNonQuery();
                                    //Utility.WriteSyncPullLog(_filename_Appointment,  _EHRLogdirectory_Appointment, " Save_Appointment_Live_To_Local query  (" + SqlCeCommand.CommandText + ") and patient_ehr_id=" + dr["patient_ehr_id"].ToString());
                                    try
                                    {
                                        Utility.WriteSyncPullLog(_filename_Appointment, _EHRLogdirectory_Appointment, "Appointment " + dr["Appointment_Status"].ToString() + " Imported in Local DB for Patient Name : (" + dr["First_Name"].ToString() + "," + dr["Last_Name"].ToString() + ") and appointment_status_ehr_key (" + dr["appointment_status_ehr_key"].ToString() + ") of Appointment : (" + dr["Appt_DateTime"].ToString() + ")");
                                    }
                                    catch (Exception)
                                    {

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
            }
            #endregion

            return _successfullstataus;
        }

        public static bool Save_Pull_EHRAppointment_WithOut_PatientID_Live_To_Local(DataTable dtLiveAppointment,string  _filename_ehr_appointment_without_patientid, string _EHRLogdirectory_ehr_appointment_without_patientid)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlCetx = conn.BeginTransaction();
                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;

                    string NotiStatus = "0";
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtLiveAppointment.Rows)
                        {
                            if (Convert.ToInt32(dr["InsUptDlt"]) == 1)
                            {

                                SqlCeCommand.CommandText = SynchLocalQRY.Insert_Appointment;

                                NotiStatus = "0";
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", dr["Appt_EHR_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", "0");
                                SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", dr["Appt_Web_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Name", "");
                                SqlCeCommand.Parameters.AddWithValue("First_Name", "");
                                SqlCeCommand.Parameters.AddWithValue("MI", "");
                                SqlCeCommand.Parameters.AddWithValue("Home_Contact", "");
                                SqlCeCommand.Parameters.AddWithValue("Mobile_Contact", "");
                                SqlCeCommand.Parameters.AddWithValue("Email", "");
                                SqlCeCommand.Parameters.AddWithValue("Address", "");
                                SqlCeCommand.Parameters.AddWithValue("City", "");
                                SqlCeCommand.Parameters.AddWithValue("ST", "");
                                SqlCeCommand.Parameters.AddWithValue("Zip", "");
                                SqlCeCommand.Parameters.AddWithValue("birth_date", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", "");
                                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Provider_Name", "");
                                SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("ApptType", "");
                                SqlCeCommand.Parameters.AddWithValue("Appt_DateTime", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Appt_EndDateTime", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("comment", "");
                                SqlCeCommand.Parameters.AddWithValue("Status", NotiStatus);
                                SqlCeCommand.Parameters.AddWithValue("Patient_Status", "");
                                SqlCeCommand.Parameters.AddWithValue("appointment_status_ehr_key", "");
                                SqlCeCommand.Parameters.AddWithValue("Appointment_Status", "");
                                SqlCeCommand.Parameters.AddWithValue("confirmed_status_ehr_key", "");
                                SqlCeCommand.Parameters.AddWithValue("confirmed_status", "");
                                SqlCeCommand.Parameters.AddWithValue("unschedule_status_ehr_key", "");
                                SqlCeCommand.Parameters.AddWithValue("unschedule_status", "");
                                SqlCeCommand.Parameters.AddWithValue("is_ehr_updated", false);
                                SqlCeCommand.Parameters.AddWithValue("Is_Appt", "");
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("is_deleted", 0);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("is_asap", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("appt_treatmentcode", "");
                                SqlCeCommand.Parameters.AddWithValue("ProcedureDesc", "");
                                SqlCeCommand.Parameters.AddWithValue("ProcedureCode", "");
                                SqlCeCommand.ExecuteNonQuery();
                                try
                                {
                                    Utility.WriteSyncPullLog(_filename_ehr_appointment_without_patientid, _EHRLogdirectory_ehr_appointment_without_patientid, "EHRAppointment WithOut PatientID Import in local for Appt_EHR_ID : (" + Convert.ToString(dr["Appt_EHR_ID"].ToString().Trim() + ")"));
                                }
                                catch (Exception)
                                {

                                }
                               
                                
                            }
                        }
                    }
                    // SqlCetx.Commit()
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

        public static bool Save_Provider_Live_To_Local(DataTable dtLiveProvider, string _filename_Provider = "", string _EHRLogdirectory_Provider = "")
        {
            bool _successfullstataus = true;

            #region SqlExpressDatabase
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlExCommand = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                //   SqlTransaction SqlExtx;
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlExtx = conn.BeginTransaction();
                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                    // SqlExCommand.Transaction = SqlExtx;

                    foreach (DataRow dr in dtLiveProvider.Rows)
                    {
                        if (Convert.ToInt32(dr["InsUptDlt"]) == 6)
                        {
                            continue;
                        }

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_Provider;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_Provider_Pull;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_Provider;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        // SqlExCommand.Parameters.AddWithValue("Provider_LocalDB_ID", dr["Provider_LocalDB_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Provider_Web_ID", dr["Provider_Web_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString());
                        SqlExCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString());
                        SqlExCommand.Parameters.AddWithValue("MI", "");
                        SqlExCommand.Parameters.AddWithValue("gender", dr["gender"].ToString());
                        SqlExCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString());
                        SqlExCommand.Parameters.AddWithValue("bio", dr["bio"].ToString());
                        SqlExCommand.Parameters.AddWithValue("education", dr["education"].ToString());
                        SqlExCommand.Parameters.AddWithValue("accreditation", dr["accreditation"].ToString());
                        SqlExCommand.Parameters.AddWithValue("membership", dr["membership"].ToString());
                        SqlExCommand.Parameters.AddWithValue("language", dr["language"].ToString());
                        SqlExCommand.Parameters.AddWithValue("age_treated_min", dr["age_treated_min"].ToString());
                        SqlExCommand.Parameters.AddWithValue("age_treated_max", dr["age_treated_max"].ToString());
                        SqlExCommand.Parameters.AddWithValue("is_active", dr["is_active"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 1);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"]);
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"]);

                        
                        try
                        {
                            if (_filename_Provider != "" && _EHRLogdirectory_Provider != "")
                            {
                                Utility.WriteSyncPullLog(_filename_Provider, _EHRLogdirectory_Provider, "Save Provider Live To Local  for  Name : (" + dr["Last_Name"] + "," + dr["First_Name"] + ") and Provider_EHR_ID : " + dr["Provider_Web_ID"].ToString());
                            }
                        }
                        catch (Exception)
                        {

                        }
                        SqlExCommand.ExecuteNonQuery();
                      
                    }
                    //  SqlExtx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //   SqlExtx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            #endregion

            #region SqlCeDatabase
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    //        SqlCetx = conn.BeginTransaction();
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            foreach (DataRow dr in dtLiveProvider.Rows)
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"]) == 6)
                                {
                                    continue;
                                }

                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Provider;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider_Pull;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Provider;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                // SqlCeCommand.Parameters.AddWithValue("Provider_LocalDB_ID", dr["Provider_LocalDB_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Provider_Web_ID", dr["Provider_Web_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Name", dr["Last_Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("First_Name", dr["First_Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("MI", "");
                                SqlCeCommand.Parameters.AddWithValue("gender", dr["gender"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("provider_speciality", dr["provider_speciality"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("bio", dr["bio"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("education", dr["education"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("accreditation", dr["accreditation"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("membership", dr["membership"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("language", dr["language"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("age_treated_min", dr["age_treated_min"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("age_treated_max", dr["age_treated_max"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("is_active", dr["is_active"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 1);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"]);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"]);

                                SqlCeCommand.ExecuteNonQuery();
                                try
                                {
                                    if (_filename_Provider != "" && _EHRLogdirectory_Provider != "")
                                    {

                                        Utility.WriteSyncPullLog(_filename_Provider, _EHRLogdirectory_Provider, "Changes Provider Live To Local  by query   : " + SqlCeCommand.CommandText);

                                        Utility.WriteSyncPullLog(_filename_Provider, _EHRLogdirectory_Provider, "Save Provider Live To Local  for  Name : (" + dr["Last_Name"] + "," + dr["First_Name"] + ") and Provider_EHR_ID : " + dr["Provider_Web_ID"].ToString());

                                    }
                                }
                                catch (Exception)
                                {

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
            }
            #endregion

            return _successfullstataus;
        }

        public static bool Save_Operatory_Live_To_Local(DataTable dtLiveOperatory, string Service_Install_Id, string _filename_Operatory = "", string EHRLogdirectory_EHR__Operatory = "")
        {
            bool _successfullstataus = true;

            #region SqlExpressDatabase
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlExCommand = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                //   SqlTransaction SqlExtx;
                if (conn.State == ConnectionState.Closed) conn.Open();
                //  SqlExtx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                    //  SqlExCommand.Transaction = SqlExtx;

                    foreach (DataRow dr in dtLiveOperatory.Rows)
                    {
                        if (Convert.ToInt32(dr["InsUptDlt"]) == 6)
                        {
                            continue;
                        }

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_Operatory;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_Operatory_Pull;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_Operatory;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        SqlExCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Operatory_Web_ID", dr["Operatory_Web_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 1);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", dr["Clinic_Number"].ToString());
                        try
                        {
                            if (_filename_Operatory != "" && EHRLogdirectory_EHR__Operatory != "")
                            {
                                Utility.WriteSyncPullLog(_filename_Operatory, EHRLogdirectory_EHR__Operatory, "Save_Operatory_Live_To_Local by query  (" + SqlExCommand.CommandText.ToString() + ")");
                                Utility.WriteSyncPullLog(_filename_Operatory, EHRLogdirectory_EHR__Operatory, "Save Operatory Live To Local for Operatory_Name : " + dr["Operatory_Name"].ToString() + " and Operatory_EHR_ID : " + dr["Operatory_EHR_ID"].ToString());
                            }
                        }
                        catch (Exception)
                        {

                        }
                       
                        SqlExCommand.ExecuteNonQuery();

                    }

                    //SqlExtx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //  SqlExtx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            #endregion

            #region SqlCEDatabase
            else
            {

                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    //  SqlCetx = conn.BeginTransaction();
                    try
                    {
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;

                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            foreach (DataRow dr in dtLiveOperatory.Rows)
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"]) == 6)
                                {
                                    continue;
                                }

                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Operatory;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory_Pull;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Operatory;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", dr["Operatory_EHR_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Name", dr["Operatory_Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Operatory_Web_ID", dr["Operatory_Web_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 1);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery(); 
                                try
                                {
                                    if (_filename_Operatory != "" && EHRLogdirectory_EHR__Operatory != "")
                                    {
                                        Utility.WriteSyncPullLog(_filename_Operatory, EHRLogdirectory_EHR__Operatory, " Save_Operatory_Live_To_Local  query  (" + SqlCeCommand.CommandText + ") and Operatory_EHR_ID=" + dr["Operatory_EHR_ID"].ToString());
                                        Utility.WriteSyncPullLog(_filename_Operatory, EHRLogdirectory_EHR__Operatory, "Operatory  Local DB for Operatory_EHR_ID : (" + dr["Operatory_EHR_ID"] + ") and Operatory_Name(" + dr["Operatory_Name"] + ") of Entry Date : (" + dtCurrentDtTime.ToString() + ")");

                                    }
                                }
                                catch (Exception)
                                {

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
            }
            #endregion

            return _successfullstataus;
        }

        public static bool Save_ApptType_Live_To_Local(DataTable dtLiveApptType, string _filename_ApptType = "", string _EHRLogdirectory_ApptType = "")
        {
            bool _successfullstataus = true;

            #region SqlExpressDatabase
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlExCommand = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                //   SqlTransaction SqlExtx;
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlExtx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                    // SqlExCommand.Transaction = SqlExtx;

                    foreach (DataRow dr in dtLiveApptType.Rows)
                    {
                        if (Convert.ToInt32(dr["InsUptDlt"]) == 6)
                        {
                            continue;
                        }

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_ApptType;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_ApptType_Pull;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_ApptType;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        // SqlExCommand.Parameters.AddWithValue("ApptType_LocalDB_ID", dr["ApptType_LocalDB_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("ApptType_Web_ID", dr["ApptType_Web_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 1);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"]);
                        SqlExCommand.ExecuteNonQuery();
                        try
                        {
                            if (_filename_ApptType != "" && _EHRLogdirectory_ApptType != "")
                            {
                                Utility.WriteSyncPullLog(_filename_ApptType, _EHRLogdirectory_ApptType, "Save_ApptType_Live_To_Local by query  (" + SqlExCommand.CommandText.ToString() + ")");
                                Utility.WriteSyncPullLog(_filename_ApptType, _EHRLogdirectory_ApptType, "Save ApptType Live To Local for Type_Name : " + dr["Type_Name"].ToString() + " and ApptType_EHR_ID : " + dr["ApptType_EHR_ID"].ToString() + " and Clinic_Number : " + dr["Clinic_Number"].ToString() + "and Service_Install_Id : " + dr["Service_Install_Id"].ToString());
                            }
                        }
                        catch (Exception)
                        {

                        }
                      
                    }

                    //SqlExtx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //SqlExtx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            #endregion

            #region SqlCeDatabase
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    //  SqlCetx = conn.BeginTransaction();

                    try
                    {
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            foreach (DataRow dr in dtLiveApptType.Rows)
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"]) == 6)
                                {
                                    continue;
                                }

                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptType;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_Pull;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_ApptType;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                // SqlCeCommand.Parameters.AddWithValue("ApptType_LocalDB_ID", dr["ApptType_LocalDB_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ApptType_EHR_ID", dr["ApptType_EHR_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ApptType_Web_ID", dr["ApptType_Web_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Type_Name", dr["Type_Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 1);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"]);
                                SqlCeCommand.ExecuteNonQuery();

                                try
                                {
                                    if (_filename_ApptType != "" && _EHRLogdirectory_ApptType != "")
                                    {
                                        Utility.WriteSyncPullLog(_filename_ApptType, _EHRLogdirectory_ApptType, "Save_ApptType_Live_To_Local by query  (" + SqlCeCommand.CommandText.ToString() + ")");
                                        Utility.WriteSyncPullLog(_filename_ApptType, _EHRLogdirectory_ApptType, "Save ApptType Live To Local for Type_Name : " + dr["Type_Name"].ToString() + " and ApptType_EHR_ID : " + dr["ApptType_EHR_ID"].ToString() + " and Clinic_Number : " + dr["Clinic_Number"].ToString() + "and Service_Install_Id : " + dr["Service_Install_Id"].ToString());
                                    }
                                }
                                catch (Exception)
                                {

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
            }
            #endregion

            return _successfullstataus;
        }

        public static bool Save_Patient_Live_To_Local(DataTable dtLivePatient,string _filename_Patient= "", string _EHRLogdirectory_Patient = "")
        {
            bool _successfullstataus = true;

            #region SqlExpressDatabase
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlExCommand = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                //    SqlTransaction SqlExtx;
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlExtx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;

                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                    //  SqlExCommand.Transaction = SqlExtx;

                    foreach (DataRow dr in dtLivePatient.Rows)
                    {
                        if (Convert.ToInt32(dr["InsUptDlt"]) == 6)
                        {
                            continue;
                        }

                        switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                        {
                            case 1:
                                SqlExCommand.CommandText = SynchLocalQRY.Insert_Patient;
                                break;
                            case 2:
                                SqlExCommand.CommandText = SynchLocalQRY.Update_Patient_Pull;
                                break;
                            case 3:
                                SqlExCommand.CommandText = SynchLocalQRY.Delete_Patient;
                                break;
                        }

                        SqlExCommand.Parameters.Clear();
                        // SqlExCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString());
                        SqlExCommand.Parameters.AddWithValue("patient_Web_ID", dr["patient_Web_ID"].ToString());
                        SqlExCommand.Parameters.AddWithValue("fullname", dr["fullname"].ToString());
                        SqlExCommand.Parameters.AddWithValue("email", dr["email"].ToString());
                        SqlExCommand.Parameters.AddWithValue("mobile", dr["mobile"].ToString());
                        SqlExCommand.Parameters.AddWithValue("phone", dr["phone"].ToString());
                        SqlExCommand.Parameters.AddWithValue("birth_date", dr["birth_date"].ToString());
                        SqlExCommand.Parameters.AddWithValue("last_visit", dr["last_visit"].ToString());
                        SqlExCommand.Parameters.AddWithValue("next_visit", dr["next_visit"].ToString());
                        SqlExCommand.Parameters.AddWithValue("revenue", dr["revenue"].ToString());
                        SqlExCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                        SqlExCommand.Parameters.AddWithValue("Is_Adit_Updated", 1);
                        SqlExCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"]);
                        SqlExCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"]);
                        SqlExCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                        SqlExCommand.ExecuteNonQuery();
                        try
                        {
                            if (_filename_Patient != "" && _EHRLogdirectory_Patient != "")
                            {
                                // Utility.WriteSyncPullLog(_filename_Patient, _EHRLogdirectory_Patient, "Save_Patient_Live_To_Local by query  (" + SqlExCommand.CommandText + ")");
                                Utility.WriteSyncPullLog(_filename_Patient, _EHRLogdirectory_Patient, "Save Patient Live To Local for fullname : " + dr["fullname"].ToString() + " and patient_ehr_id : " + dr["patient_ehr_id"].ToString() + " and Clinic_Number : " + dr["Clinic_Number"].ToString() + "and Service_Install_Id : " + dr["Service_Install_Id"].ToString());
                            }
                        }
                        catch (Exception)
                        {

                        }
                       
                    }

                    // SqlExtx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //  SqlExtx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            #endregion

            #region SqlCeDatabase
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;

                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            foreach (DataRow dr in dtLivePatient.Rows)
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"]) == 6)
                                {
                                    continue;
                                }

                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Pull;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dr["patient_ehr_id"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", dr["patient_Web_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("fullname", dr["fullname"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("email", dr["email"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("mobile", dr["mobile"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("phone", dr["phone"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("birth_date", dr["birth_date"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("last_visit", dr["last_visit"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("next_visit", dr["next_visit"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("revenue", dr["revenue"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dtCurrentDtTime);
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 1);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"]);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"]);
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", dr.Table.Columns.Contains("Is_Deleted") ? dr["Is_Deleted"] : 0);
                                SqlCeCommand.ExecuteNonQuery();
                                try
                                {
                                    if (_filename_Patient != "" && _EHRLogdirectory_Patient != "")
                                    {
                                        //Utility.WriteSyncPullLog(_filename_Patient, _EHRLogdirectory_Patient, "Save_Patient_Live_To_Local by query  (" + SqlCeCommand.CommandText + ")");
                                        Utility.WriteSyncPullLog(_filename_Patient, _EHRLogdirectory_Patient, "Save Patient Live To Local for fullname : " + dr["fullname"].ToString() + " and patient_ehr_id : " + dr["patient_ehr_id"].ToString() + " and Clinic_Number : " + dr["Clinic_Number"].ToString() + "and Service_Install_Id : " + dr["Service_Install_Id"].ToString());
                                    }
                                }
                                catch (Exception)
                                {

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
            }
            #endregion

            return _successfullstataus;
        }

        public static bool Save_PatientFormMedicalHistory_Live_To_Local(DataTable dtLocalPatientFormMedicalHistory)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtLocalPatientFormMedicalHistory.Rows)
                        {

                            switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                            {
                                case 1:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient_Form;
                                    break;
                                case 2:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Form_Pull;
                                    break;
                                case 3:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient_Form;
                                    break;
                            }

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", dr["PatientForm_Web_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", dr["Patient_Web_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("ehrfield", dr["ehrfield"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("ehrfield_value", dr["ehrfield_value"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                            SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("PatientType", 0);
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
            }
            return _successfullstataus;
        }

        public static bool Save_PatientForm_Live_To_Local(DataTable dtLivePatientForm, bool is_PatientPortal = false, string _filename_EHR_PatientFormt="", string _EHRLogdirectory_EHR_PatientForm="")
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtLivePatientForm.Rows)
                        {
                            switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                            {
                                case 1:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient_Form;
                                    break;
                                case 2:
                                    if (is_PatientPortal)
                                    {
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Portal_Pull;
                                    }
                                    else
                                    {
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Form_Pull;
                                    }
                                    break;
                                case 3:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient_Form;
                                    break;
                            }

                            SqlCeCommand.Parameters.Clear();
                            // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", dr["PatientForm_Web_ID"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", dr["Patient_Web_ID"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ehrfield", dr["ehrfield"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("ehrfield_value", dr["ehrfield_value"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                            SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                            if (dr["folder_ehr_id"].ToString() == "")
                            {
                                SqlCeCommand.Parameters.AddWithValue("folder_ehr_id", "0");
                            }
                            else
                            {
                                SqlCeCommand.Parameters.AddWithValue("folder_ehr_id", dr["folder_ehr_id"].ToString().Trim());
                            }

                            SqlCeCommand.Parameters.AddWithValue("folder_name", dr["folder_name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("DocNameFormat", dr["DocNameFormat"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Form_Name", dr["Form_Name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Patient_Name", dr["Patient_Name"].ToString().Trim());
                            if (dr["submit_time"].ToString() == "")
                            {
                                SqlCeCommand.Parameters.AddWithValue("submit_time", dtCurrentDtTime);
                            }
                            else
                            {
                                SqlCeCommand.Parameters.AddWithValue("submit_time", dr["submit_time"].ToString().Trim());
                            }
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("PatientType", dr["PatientType"].ToString().Trim() == "" ? 0 : Convert.ToInt16(dr["PatientType"].ToString().Trim()));

                            //SqlCeCommand.Parameters.AddWithValue("Submit_Time", dr["Submit_Time"].ToString().Trim());
                            SqlCeCommand.ExecuteNonQuery();

                            try
                            {
                                if (_filename_EHR_PatientFormt != "" && _EHRLogdirectory_EHR_PatientForm != "")
                                {
                                    // Utility.WriteSyncPullLog(_filename_EHR_PatientFormt, _EHRLogdirectory_EHR_PatientForm, "Save_PatientForm_Live_To_Local by query  (" + SqlCeCommand.CommandText + ")");
                                    Utility.WriteSyncPullLog(_filename_EHR_PatientFormt, _EHRLogdirectory_EHR_PatientForm, "Save PatientForm Live To Local for Patient_Name : " + dr["Patient_Name"].ToString() + " and,Patient_Web_ID= " + dr["Patient_Web_ID"].ToString() + " and  Patient_EHR_ID : " + dr["Patient_EHR_ID"].ToString() + " and Clinic_Number : " + dr["Clinic_Number"].ToString() + "and Service_Install_Id : " + dr["Service_Install_Id"].ToString());
                                }
                            }
                            catch (Exception)
                            {

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

        public static bool Save_PatientFormDoc_Live_To_Local(DataTable dtLivePatientFormDoc, string _filename_EHR_Patient_Document = "", string _EHRLogdirectory_EHR_Patient_Document = "")
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtLivePatientFormDoc.Rows)
                        {

                            switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                            {
                                case 1:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient_FormDoc;
                                    break;
                                case 2:
                                    //SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Form_PullDoc;
                                    break;
                                case 3:
                                    // SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient_FormDoc;
                                    break;
                            }

                            SqlCeCommand.Parameters.Clear();
                            // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PatientDoc_Web_ID", dr["PatientDoc_Web_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PatientDoc_EHR_ID", dr["PatientDoc_EHR_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", dr["Patient_Web_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PatientDoc_Name", dr["PatientDoc_Name"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("folder_ehr_id", dr["folder_ehr_id"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("folder_name", dr["folder_name"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("DocNameFormat", dr["DocNameFormat"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                            SqlCeCommand.Parameters.AddWithValue("Is_EHR_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString().Trim());
                            //rooja 10-4-23
                            SqlCeCommand.Parameters.AddWithValue("Patient_Name", dr["Patient_Name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Form_Name", dr["Form_Name"].ToString().Trim());
                            //SqlCeCommand.Parameters.AddWithValue("Form_Submitted_Time", dr["Form_Submitted_Time"].ToString().Trim());
                            SqlCeCommand.ExecuteNonQuery();
                            try
                            {
                                if (_filename_EHR_Patient_Document != "" && _EHRLogdirectory_EHR_Patient_Document != "")
                                {
                                    //Utility.WriteSyncPullLog(_filename_EHR_Patient_Document, _EHRLogdirectory_EHR_Patient_Document, "Save_PatientFormDoc_Live_To_Local by query  (" + SqlCeCommand.CommandText + ")");
                                    Utility.WriteSyncPullLog(_filename_EHR_Patient_Document, _EHRLogdirectory_EHR_Patient_Document, "Save PatientFormDoc Live To Local for PatientDoc_Name : " + dr["PatientDoc_Name"].ToString() + ",PatientDoc_EHR_ID : " + dr["PatientDoc_EHR_ID"].ToString() + " and Patient_EHR_ID : " + dr["Patient_EHR_ID"].ToString() + " and Clinic_Number : " + dr["Clinic_Number"].ToString() + "and Service_Install_Id : " + dr["Service_Install_Id"].ToString());
                                }
                            }
                            catch (Exception)
                            {

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

        public static bool Save_PatientFormDocAttachment_Live_To_Local(DataTable dtLivePatientFormDoc)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtLivePatientFormDoc.Rows)
                        {

                            switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                            {
                                case 1:
                                    SqlCeCommand.CommandText = SynchLocalQRY.Insert_Patient_FormDocAttachment;
                                    break;
                                case 2:
                                    //SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Form_PullDoc;
                                    break;
                                case 3:
                                    // SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient_FormDoc;
                                    break;
                            }

                            SqlCeCommand.Parameters.Clear();
                            // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PatientDoc_Web_ID", dr["PatientDoc_Web_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PatientDoc_EHR_ID", dr["PatientDoc_EHR_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", dr["Patient_Web_ID"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PatientDoc_Name", dr["PatientDoc_Name"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("folder_ehr_id", dr["folder_ehr_id"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("folder_name", dr["folder_name"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("DocNameFormat", dr["DocNameFormat"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                            SqlCeCommand.Parameters.AddWithValue("Is_EHR_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString().Trim());
                            //rooja 10-4-23
                            SqlCeCommand.Parameters.AddWithValue("Patient_Name", dr["Patient_Name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Form_Name", dr["Form_Name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_web_Id", dr["PatientForm_web_Id"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("DocType", dr["DocType"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Web_DocName", dr["Web_DocName"].ToString().Trim());
                            //SqlCeCommand.Parameters.AddWithValue("Form_Submitted_Time", dr["Form_Submitted_Time"].ToString().Trim());
                            SqlCeCommand.ExecuteNonQuery();
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
        public static bool Update_PatientFormDoc_Live_To_Local(string Patient_form_webId, string Service_Install_Id, string _filename_EHR_Patient_Document = "", string _EHRLogdirectory_EHR_Patient_Document = "")
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_PatientFormDoc_Live_To_Local;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_id", Patient_form_webId);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        try
                        {


                            if (_filename_EHR_Patient_Document != "" && _EHRLogdirectory_EHR_Patient_Document != "")
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Patient_Document, _EHRLogdirectory_EHR_Patient_Document, "Patient document Is_Pdf_Created in local for PatientForm Web Id : (" + Patient_form_webId + ")");
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
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
        public static bool Update_PatientFormDocAttachment_Live_To_Local(string Patient_form_webId, string Service_Install_Id)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_PatientFormDocAttachment_Live_To_Local;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_id", Patient_form_webId);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                    }
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
        public static bool Update_PatientDocNotFound_Live_To_Local(string Patient_form_webId, string Service_Install_Id)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_PatientDocNotFound_Live_To_Local;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_id", Patient_form_webId);
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        try
                        {
                            Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, "PatientDocNotFound in local received for PatientForm Web id (" + Patient_form_webId + " )");
                        }
                        catch (Exception)
                        {

                        }
                    }
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
        public static bool Update_PatientDocAttachmentNotFound_Live_To_Local(string Patient_form_webId, string Service_Install_Id)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_PatientDocAttachmentNotFound_Live_To_Local;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_id", Patient_form_webId);
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                    }
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
        public static bool Update_TreatrmentDocNotFound_Live_To_Local(string TreatmentPlanId)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_TreatmentDocNotFound_Live_To_Local;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("TreatmentPlanId", TreatmentPlanId);
                        //SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        try
                        {
                            Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "TreatrmentDocNotFound in Local DB for TreatmentPlanId(" + TreatmentPlanId + ")");
                        }
                        catch (Exception)
                        {

                        }
                    }
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
        public static bool Update_PatientFormDoc_Local_To_EHR(string PatientDoc_Web_ID, string PatientDoc_EHR_ID, string Service_Install_Id)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_PatientFormDoc_Local_To_EHR;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("PatientDoc_EHR_ID", PatientDoc_EHR_ID);
                        SqlCeCommand.Parameters.AddWithValue("PatientDoc_Web_ID", PatientDoc_Web_ID);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        try
                        {
                            Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, "PatientFormDoc in local for PatientDoc_EHR_ID : (" + PatientDoc_EHR_ID + ")");
                        }
                        catch (Exception)
                        {

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
        public static bool Update_PatientFormDocAttachment_Local_To_EHR(string PatientDoc_Web_ID, string PatientDoc_EHR_ID, string Service_Install_Id)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_PatientFormDocAttachment_Local_To_EHR;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("PatientDoc_EHR_ID", PatientDoc_EHR_ID);
                        SqlCeCommand.Parameters.AddWithValue("PatientDoc_Web_ID", PatientDoc_Web_ID);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
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
        public static bool Update_TreatmentFormDoc_Local_To_EHR(string TreatmentDoc_Web_ID, string TreatmentDoc_EHR_ID)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_TreatmentFormDoc_Local_To_EHR;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("TreatmentDoc_EHR_ID", TreatmentDoc_EHR_ID);
                        SqlCeCommand.Parameters.AddWithValue("TreatmentDoc_Web_ID", TreatmentDoc_Web_ID);
                        SqlCeCommand.ExecuteNonQuery();
                        try
                        {
                            Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "TreatrmentDoc in EHR for TreatmentDoc_Web_ID(" + TreatmentDoc_Web_ID + ")");
                        }
                        catch (Exception)
                        {

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
        public static bool Save_PatientPaymentLog_To_Local(DataTable dtLivePatientPaymentLog)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtLivePatientPaymentLog.Rows)
                        {
                            try
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.InsertIntoPatientPaymentLog;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.UpdatePatientPaymentLog;
                                        break;
                                    case 3:
                                        //   SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient_Form;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PatientEHRId", dr["PatientEHRId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", dr["Patient_Web_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("PatientPaymentWebId", dr["PatientPaymentWebId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ProviderEHRId", dr["ProviderEHRId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("PaymentDate", Convert.ToDateTime(dr["PaymentDate"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("Amount", dr["Amount"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentNote", dr["PaymentNote"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentMode", dr["PaymentMode"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentType", dr["PaymentType"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentInOut", dr["PaymentInOut"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("BankOrBranchName", dr["BankOrBranchName"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ChequeNumber", dr["ChequeNumber"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentReceivedLocal", dr["PaymentReceivedLocal"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentEntryDatetimeLocal", DateTime.Now);
                                SqlCeCommand.Parameters.AddWithValue("PaymentUpdatedEHR", dr["PaymentUpdatedEHR"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentUpdatedEHRDateTime", DateTime.Now);
                                SqlCeCommand.Parameters.AddWithValue("PaymentStatusCompletedAdit", dr["PaymentStatusCompletedAdit"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentStatusCompletedDateTime", DateTime.Now);
                                SqlCeCommand.Parameters.AddWithValue("PaymentEHRId", dr["PaymentEHRId"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("template", dr["template"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("EHRSyncPaymentLog", dr["EHRSyncPaymentLog"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("FirstName", dr["FirstName"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("LastName", dr["LastName"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Mobile", dr["Mobile"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Email", dr["Email"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Fees", Convert.ToDecimal(dr["Fees"]));
                                SqlCeCommand.Parameters.AddWithValue("Discount", Convert.ToDecimal(dr["Discount"]));
                                SqlCeCommand.Parameters.AddWithValue("EHRErroLog", dr["EHRErroLog"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                                try
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "Payment Confirmed in local for Patient Name : (" + dr["Firstname"] + "," + dr["LastName"] + ") and Amount (" + dr["Amount"].ToString().ToString() + ") of payment date : (" + Convert.ToString(dr["PaymentDate"].ToString().Trim() + ")"));
                                }
                                catch (Exception)
                                {

                                }

                                #region call API for

                                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "ready", dr["Service_Install_Id"].ToString().Trim(), dr["Clinic_Number"].ToString().Trim(), "", "", "", "", "", Convert.ToInt32(dr["TryInsert"]));

                            }
                            catch (Exception ex)
                            {
                                SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "error", dr["Service_Install_Id"].ToString().Trim(), dr["Clinic_Number"].ToString().Trim(), ex.Message, "", "", "", ex.Message.ToString(), Convert.ToInt32(dr["TryInsert"]));
                            }
                            //string strApiProviders = PullLiveDatabaseDAL.GetLiveRecord("PatientPaymentLogStatus","");
                            //var client = new RestClient(strApiProviders);
                            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            //var request = new RestRequest(Method.POST);                   
                            //ServicePointManager.Expect100Continue = true;
                            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            //request.AddHeader("cache-control", "no-cache");
                            //request.AddHeader("content-type", "application/json");
                            //request.AddHeader("Authorization", Utility.WebAdminUserToken);                    
                            //string jsonstringn = "{\"patientId\":\"" + dr["Patient_Web_ID"].ToString().Trim() + "\",\"ehrSyncId\":\"" + dr["PatientPaymentWebId"].ToString().Trim() + "\",\"status\":\"ready\"}";
                            //request.AddParameter("application/json", jsonstringn, ParameterType.RequestBody);
                            //IRestResponse response = client.Execute(request);
                            //if (response.ErrorMessage != null)
                            //{
                            //    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                            //    { }
                            //    else
                            //    {
                            //        Utility.WriteToSyncLogFile_All("[PatientPaymentLogStatus Sync (Adit Server To Local Database)] Service Install Id : " + dr["Service_Install_Id"].ToString().Trim() + " And Clinic : " + dr["Clinic_Number"].ToString().Trim() + "  " + response.ErrorMessage);
                            //    }
                            //}
                            #endregion
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

        public static bool Save_PatientSMSCallLog_To_Local(DataTable dtLivePatientSMSCallLog,string _filename_EHR_patient_sms_call= "", string _EHRLogdirectory_EHR_patient_sms_call = "")
        {
            bool _successfullstataus = true;
            int RecordsExistsCount = 0;
            //SqlCeConnection conn = null;
            // SqlCeCommand SqlCeCommand = null;
            // CommonDB.LocalConnectionServer(ref conn);

            //   SqlCeTransaction SqlCetx;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        foreach (DataRow dr in dtLivePatientSMSCallLog.Rows)
                        {

                            RecordsExistsCount = 0;
                            SqlCeCommand.CommandText = SynchLocalQRY.CheckSMSLogBeforeInsert;

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("PatientEHRId", dr["PatientEHRId"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("smsId", dr["smsId"].ToString());
                            RecordsExistsCount = Convert.ToInt16(SqlCeCommand.ExecuteScalar());

                            if (RecordsExistsCount == 0 && dr["Log_Status"].ToString() == string.Empty)
                            {
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.InsertIntoPatientSMSCallLog;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.UpdatePatientSMSCallLog;
                                        break;
                                    case 3:
                                        //   SqlCeCommand.CommandText = SynchLocalQRY.Delete_Patient_Form;
                                        break;
                                }

                                try
                                {
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.AddWithValue("esId", dr["esId"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("smsId", dr["smsId"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("PatientEHRId", dr["PatientEHRId"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", dr["Patient_Web_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("PatientMobile", dr["PatientMobile"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("PatientSMSCallLogWebId", dr["PatientSMSCallLogWebId"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("ProviderEHRId", dr["ProviderEHRId"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("app_alias", dr["app_alias"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("text", dr["text"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("message_direction", dr["message_direction"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("message_type", dr["message_type"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("PatientSMSCallLogDate", dr["PatientSMSCallLogDate"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("LogReceivedLocal", dr["LogReceivedLocal"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("LogEntryDatetimeLocal", DateTime.Now);
                                    if (dr["smsId"] == null || dr["PatientEHRId"] == null || dr["message_direction"] == null)
                                    {
                                        dr["Log_Status"] = "Err_smsId,Patient_Web_ID or message_direction is missing.";
                                        SqlCeCommand.Parameters.AddWithValue("LogUpdatedEHR", true);
                                        SqlCeCommand.Parameters.AddWithValue("ErrorMessage", "Err_smsId,PatientEHRId or message_direction is missing.");
                                        SqlCeCommand.Parameters.AddWithValue("LogStatusCompletedAdit", true);
                                        SqlCeCommand.Parameters.AddWithValue("LogStatusCompletedDateTime", DateTime.Now);
                                    }
                                    else if ((dr["smsId"] != null && string.IsNullOrEmpty(dr["smsId"].ToString())) ||
                                             (dr["PatientEHRId"] != null && string.IsNullOrEmpty(dr["PatientEHRId"].ToString())) ||
                                             (dr["message_direction"] != null && string.IsNullOrEmpty(dr["message_direction"].ToString())))
                                    {
                                        dr["Log_Status"] = "Err_smsId,PatientEHRId or message_direction is missing.";
                                        SqlCeCommand.Parameters.AddWithValue("LogUpdatedEHR", true);
                                        SqlCeCommand.Parameters.AddWithValue("ErrorMessage", "Err_smsId,PatientEHRId or message_direction is missing.");
                                        SqlCeCommand.Parameters.AddWithValue("LogStatusCompletedAdit", true);
                                        SqlCeCommand.Parameters.AddWithValue("LogStatusCompletedDateTime", DateTime.Now);
                                    }
                                    else
                                    {
                                        dr["Log_Status"] = "readytoimport";
                                        SqlCeCommand.Parameters.AddWithValue("LogUpdatedEHR", dr["LogUpdatedEHR"].ToString());
                                        SqlCeCommand.Parameters.AddWithValue("ErrorMessage", "");
                                        SqlCeCommand.Parameters.AddWithValue("LogStatusCompletedAdit", false);
                                        SqlCeCommand.Parameters.AddWithValue("LogStatusCompletedDateTime", DateTime.Now);
                                    }
                                    SqlCeCommand.Parameters.AddWithValue("LogUpdatedEHRDateTime", DateTime.Now);
                                    SqlCeCommand.Parameters.AddWithValue("LogEHRId", dr["LogEHRId"].ToString());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("LogType", dr["LogType"].ToString().Trim() == "" ? 0 : Convert.ToInt16(dr["LogType"].ToString().Trim()));
                                    SqlCeCommand.ExecuteNonQuery();
                                    try
                                    {
                                        if (_filename_EHR_patient_sms_call != "" && _EHRLogdirectory_EHR_patient_sms_call != "")
                                        {
                                            //Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, " payment query  (" + SqlCeCommand.CommandText + ") and LogEHRId=" + dr["LogEHRId"].ToString());
                                            Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, " Save SMS call log For Patient  " + dr["PatientEHRId"].ToString().Trim() + " in Local DB for smsId : (" + dr["smsId"] + ") and PatientSMSCallLogWebId (" + dr["PatientSMSCallLogWebId"].ToString() + ") of PatientSMSCallLogDate : (" + Convert.ToString(dr["PatientSMSCallLogDate"].ToString().Trim() + ")"));
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                    #region call API for

                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Length >= 100)
                                    {
                                        dr["Log_Status"] = "Err_" + ex.Message.Substring(0, 100);
                                    }
                                    else
                                    {
                                        dr["Log_Status"] = "Err_" + ex.Message.ToString();
                                    }
                                }
                            }

                            #endregion
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

        public static bool Update_InsuranceCarrierDocNotFound_Live_To_Local(string InsuranceCarrierId)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_InsuranceCarrierNotFound_Live_To_Local;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Web_ID", InsuranceCarrierId);
                        
                        SqlCeCommand.ExecuteNonQuery();
                        try
                        {
                            Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "InsuranceCarrierDocNotFound in Local DB for InsuranceCarrierId(" + InsuranceCarrierId + ")");
                        }
                        catch (Exception)
                        {

                        }
                    }
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

        public static bool Update_InsuranceCarrierDoc_Local_To_EHR(string InsuranceCarrierDoc_Web_ID, string InsuranceCarrierDoc_EHR_ID)
        {
            bool _successfullstataus = true;

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = SynchLocalQRY.Update_InsuranceCarrier_Local_To_EHR;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                        SqlCeCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_EHR_ID", InsuranceCarrierDoc_EHR_ID);
                        SqlCeCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Web_ID", InsuranceCarrierDoc_Web_ID);
                        SqlCeCommand.ExecuteNonQuery();
                        try
                        {
                            Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "InsuranceCarrierDoc in EHR for InsuranceCarrierDoc_Web_ID(" + InsuranceCarrierDoc_Web_ID + ")");
                        }
                        catch (Exception)
                        {

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
    }
}
