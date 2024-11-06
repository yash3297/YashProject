using Pozative.QRY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data.SqlServerCe;
using Pozative.BO;
using Pozative.UTL;
using System.Data.Odbc;
using System.Data.SqlClient;
using RestSharp;
using System.Net;
using System.Collections;
using System.Net.Security;

namespace Pozative.DAL
{
    public class SynchLocalDAL
    {

        public static bool Update_Sync_Table_Datetime(string TableName)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = SynchLocalQRY.GetLastUpdateDate;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    // sqlCommand.Transaction = transaction;

                    //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    sqlCommand.Parameters.AddWithValue("Sync_Table_Name", TableName);
                    CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);

                    DataTable sqlDt = new DataTable();
                    sqlDa.Fill(sqlDt);


                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    //sqlCommand.Transaction = transaction;

                    if (sqlDt.Rows.Count > 0)
                        sqlCommand.CommandText = SynchLocalQRY.Update_Sync_Table_DateTime;
                    else
                        sqlCommand.CommandText = SynchLocalQRY.Insert_Sync_Table_DateTime;
                    sqlCommand.Parameters.Clear();
                    sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    sqlCommand.Parameters.AddWithValue("Sync_Table_Name", TableName);
                    sqlCommand.ExecuteNonQuery();

                    //transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    // transaction.Rollback();
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = SynchLocalQRY.GetLastUpdateDate;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                            sqlCeCommand.Parameters.AddWithValue("Sync_Table_Name", TableName);
                            using (SqlCeDataAdapter sqlDa = new SqlCeDataAdapter(sqlCeCommand))
                            {
                                sqlDt = new DataTable();
                                sqlDa.Fill(sqlDt);
                            }
                        }
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            if (sqlDt.Rows.Count > 0)
                                sqlCeCommand.CommandText = SynchLocalQRY.Update_Sync_Table_DateTime;
                            else
                                sqlCeCommand.CommandText = SynchLocalQRY.Insert_Sync_Table_DateTime;
                            sqlCeCommand.Parameters.Clear();
                            sqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                            sqlCeCommand.Parameters.AddWithValue("Sync_Table_Name", TableName);
                            sqlCeCommand.ExecuteNonQuery();
                        }

                        ////Utility.WriteToErrorLogFromAll("Last_Synctime - after save in local sync_table- Module Name :"+ TableName.ToString() + "-Time :"+ dtCurrentDtTime.ToString());
                        ////rooja - add lst sync time API call function here
                        //#region LastSyncTime - https://app.asana.com/0/1204010716278938/1204625995328405/f
                        //string SectionName = "";
                        //SectionName = TableName;
                        //switch (SectionName)
                        //{
                        //    case "Appointment_Push":
                        //        Utility.Push_lastSyncAppointment = dtCurrentDtTime.ToString();
                        //        //Utility.WriteToErrorLogFromAll("1. Last_Synctime - Appointment_Push :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "Provider_Push":
                        //        Utility.Push_lastSyncProvider = dtCurrentDtTime.ToString();
                        //        // Utility.WriteToErrorLogFromAll("Last_Synctime - Provider_Push :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "ProviderHours_Push":
                        //        Utility.Push_provider_hours = dtCurrentDtTime.ToString();
                        //        // Utility.WriteToErrorLogFromAll("Last_Synctime - ProviderHours_Push :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "Operatory_Push":
                        //        Utility.Push_lastSyncOperatory = dtCurrentDtTime.ToString();
                        //        //Utility.WriteToErrorLogFromAll("Last_Synctime - Operatory_Push :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "OperatoryHours_Push":
                        //        Utility.Push_lastSyncOperatory_hours = dtCurrentDtTime.ToString();
                        //        //Utility.WriteToErrorLogFromAll("Last_Synctime - OperatoryHours_Push :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "ApptType_Push":
                        //        Utility.Push_lastSyncAppointment_Type = dtCurrentDtTime.ToString();
                        //        //      Utility.WriteToErrorLogFromAll("Last_Synctime - ApptType_Push :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "Patient_Push":
                        //        Utility.Push_lastSyncPatient = dtCurrentDtTime.ToString();
                        //        //Utility.Push_lastSyncPatient_status = dtCurrentDtTime.ToString();
                        //        //    Utility.WriteToErrorLogFromAll("Last_Synctime - Patient_Push :" + dtCurrentDtTime.ToString());
                        //        break;

                        //    case "PatientStatus":
                        //        Utility.Push_lastSyncPatient_status = dtCurrentDtTime.ToString();
                        //        //     Utility.WriteToErrorLogFromAll("Last_Synctime - PatientStatus :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "Appointment_Pull":
                        //        //Utility.pull_appointment= dtCurrentDtTime.ToString();
                        //        Utility.pull_appointment = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                        //        //Utility.WriteToErrorLogFromAll("Last_Synctime - Appointment_Pull :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "Pull_TreatmentDoc":
                        //        Utility.pull_treatmentplan_document = dtCurrentDtTime.ToString();
                        //        // Utility.WriteToErrorLogFromAll("Last_Synctime - Pull_TreatmentDoc :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "Patient_Form_Pull":
                        //        Utility.pull_patient_Form = dtCurrentDtTime.ToString();
                        //        Utility.pull_patient_document = dtCurrentDtTime.ToString();
                        //        // Utility.WriteToErrorLogFromAll("Last_Synctime - Patient_Form_Pull :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "PatientSMSCallLog":
                        //        Utility.Pull_lastSyncSMS_call_log = dtCurrentDtTime.ToString();
                        //        // Utility.WriteToErrorLogFromAll("Last_Synctime - PatientSMSCallLog :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "Push_lastSyncPatient_payment_log":
                        //        Utility.Push_lastSyncPatient_payment_log = dtCurrentDtTime.ToString();
                        //        //  Utility.WriteToErrorLogFromAll("Last_Synctime - Push_lastSyncPatient_payment_log :" + dtCurrentDtTime.ToString());
                        //        break;
                        //    case "Push_lastSyncConfirm_Appointment":
                        //        Utility.Push_lastSyncConfirm_Appointment = dtCurrentDtTime.ToString();
                        //        // Utility.WriteToErrorLogFromAll("Last_Synctime - Push_lastSyncConfirm_Appointment :" + dtCurrentDtTime.ToString());
                        //        break;
                        //}
                        //#endregion

                        //SqlCetx.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
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
        }

        #region  Treatment Documet
        public static bool Sync_check_for_treatmentDoct(string TreatmentPlanId,string _filename_EHR_treatmentplan_document="",string  _EHRLogdirectory_EHR_treatmentplan_document="")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = SynchLocalQRY.SelectDocId;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    // sqlCommand.Transaction = transaction;

                    //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    sqlCommand.Parameters.AddWithValue("TreatmentPlanId", TreatmentPlanId);
                    CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                    Utility.WriteSyncPullLog(_filename_EHR_treatmentplan_document,_EHRLogdirectory_EHR_treatmentplan_document, " Sync check for treatmentDoct for TreatmentPlanId=" + TreatmentPlanId.ToString ());
                    DataTable sqlDt = new DataTable();
                    sqlDa.Fill(sqlDt);


                    if (sqlDt.Rows.Count == 0)
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
                    // transaction.Rollback();
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = SynchLocalQRY.SelectDocId;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                            sqlCeCommand.Parameters.AddWithValue("TreatmentPlanId", TreatmentPlanId);
                            Utility.WriteSyncPullLog(_filename_EHR_treatmentplan_document, _EHRLogdirectory_EHR_treatmentplan_document, " Sync check for treatmentDoct for TreatmentPlanId=" + TreatmentPlanId);
                            using (SqlCeDataAdapter sqlDa = new SqlCeDataAdapter(sqlCeCommand))
                            {
                                sqlDt = new DataTable();
                                sqlDa.Fill(sqlDt);
                            }

                            if (sqlDt.Rows.Count == 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        public static void CreateRecordForTreatmentDoc(string PatientName, string SubmittedDate, string Patient_EHR_ID, string Patient_Web_ID, string TreatmentPlanId, string TreatmentPlanName, string Clinic_Number, string Service_Install_Id, string _filename_EHR_treatmentplan_document = "", string _EHRLogdirectory_EHR_treatmentplan_document = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = SynchLocalQRY.Insert_TreatmentDoc;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    // sqlCommand.Transaction = transaction;

                    sqlCommand.Parameters.AddWithValue("PatientName,", PatientName);
                    sqlCommand.Parameters.AddWithValue("SubmittedDate", SubmittedDate);
                    sqlCommand.Parameters.AddWithValue("Patient_EHR_ID", Patient_EHR_ID);
                    sqlCommand.Parameters.AddWithValue("Patient_Web_ID", Patient_Web_ID);
                    //sqlCommand.Parameters.AddWithValue("TreatmentDoc_Name", TreatmentDoc_Name);
                    sqlCommand.Parameters.AddWithValue("TreatmentPlanId", TreatmentPlanId);
                    sqlCommand.Parameters.AddWithValue("TreatmentPlanName", TreatmentPlanName);
                    sqlCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                    //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    //sqlCommand.Parameters.AddWithValue("Is_EHR_Updated", false);
                    //sqlCommand.Parameters.AddWithValue("Is_Adit_Updated", false);
                    sqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                    sqlCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    sqlCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_treatmentplan_document, _EHRLogdirectory_EHR_treatmentplan_document, " Create Record For TreatmentDoc  TreatmentPlanId=" + TreatmentPlanId + " and PatientName=" + PatientName + ",Patient_EHR_ID =" + Patient_EHR_ID);
                    DataTable sqlDt = new DataTable();
                    sqlDa.Fill(sqlDt);

                }
                catch (Exception ex)
                {
                    // transaction.Rollback();
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = SynchLocalQRY.Insert_TreatmentDoc;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;

                            sqlCeCommand.Parameters.AddWithValue("TreatmentDoc_Web_ID", TreatmentPlanId);
                            sqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", Patient_EHR_ID);
                            sqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", Patient_Web_ID);
                            //sqlCeCommand.Parameters.AddWithValue("TreatmentDoc_Name", TreatmentDoc_Name);
                            sqlCeCommand.Parameters.AddWithValue("TreatmentPlanId", TreatmentPlanId);
                            sqlCeCommand.Parameters.AddWithValue("TreatmentPlanName", TreatmentPlanName);
                            sqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);

                            sqlCeCommand.Parameters.AddWithValue("PatientName", PatientName);
                            sqlCeCommand.Parameters.AddWithValue("SubmittedDate", SubmittedDate);

                            sqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                            sqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            sqlCeCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_treatmentplan_document, _EHRLogdirectory_EHR_treatmentplan_document, " Create Record For TreatmentDoc with TreatmentPlanId=" + TreatmentPlanId + " and PatientName=" + PatientName + ",Patient_EHR_ID=" + Patient_EHR_ID);
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        public static void UPDATERecordForTreatmentDoc(string TreatmentPlan_Id, string FileName, string _filename_EHR_treatmentplan_document = "", string _EHRLogdirectory_EHR_treatmentplan_document = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = SynchLocalQRY.UpdateDocStatus;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    // sqlCommand.Transaction = transaction;

                    //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    sqlCommand.Parameters.AddWithValue("TreatmentDoc_Name", FileName);
                    sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    sqlCommand.Parameters.AddWithValue("Is_Pdf_Created", 1);
                    sqlCommand.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    // transaction.Rollback();
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = SynchLocalQRY.UpdateDocStatus;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            sqlCeCommand.Parameters.AddWithValue("TreatmentDoc_Name", FileName);
                            sqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                            sqlCeCommand.Parameters.AddWithValue("Is_Pdf_Created", 1);
                            sqlCeCommand.Parameters.AddWithValue("TreatmentPlanId", TreatmentPlan_Id);
                            sqlCeCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_treatmentplan_document, _EHRLogdirectory_EHR_treatmentplan_document, "Update TreatmentDoc status in local for TreatmentDoc_Name" + FileName + " and TreatmentPlanId=" + TreatmentPlan_Id.ToString() + " of Last_Sync_Date : (" + Convert.ToString(dtCurrentDtTime) + ")");
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        public static void UpdateTreatmentDocInlocal(string TreatmentPlan_Id, string _filename_EHR_treatmentplan_document = "", string _EHRLogdirectory_EHR_treatmentplan_document = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = SynchLocalQRY.UpdateDocAditUpdated;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    // sqlCommand.Transaction = transaction;

                    //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    sqlCommand.Parameters.AddWithValue("TreatmentPlanId", TreatmentPlan_Id);
                    sqlCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_treatmentplan_document, _EHRLogdirectory_EHR_treatmentplan_document, " Update Treatment Doc In local for TreatmentPlanId=" + TreatmentPlan_Id);
                }
                catch (Exception ex)
                {
                    // transaction.Rollback();
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = SynchLocalQRY.UpdateDocAditUpdated;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            sqlCeCommand.Parameters.AddWithValue("TreatmentPlanId", TreatmentPlan_Id);
                            sqlCeCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_treatmentplan_document, _EHRLogdirectory_EHR_treatmentplan_document, " Update Treatment Doc In local for TreatmentPlanId=" + TreatmentPlan_Id);
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        public static DataTable ChangeStatusForTreatmentDoc(string StatusType)
        {
            string sqlSelect;
            DataTable dt = new DataTable();
            if (StatusType == "Importing")
            {
                sqlSelect = SynchLocalQRY.ImportingTreatmentDocStatus;
            }
            else if (StatusType == "Completed")
            {
                sqlSelect = SynchLocalQRY.CompletedTreatmentDocStatus;
            }
            else
            {
                return dt;
            }


            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);
                DateTime ToDate = Utility.LastSyncDateAditServer;
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                    CommonDB.SqlServerDataAdapter(SqlCeCommand, ref SqlCeDa);
                    SqlCeDa.Fill(dt);

                    return dt;
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(sqlCeCommand))
                            {
                                SqlCeDa.Fill(dt);
                            }
                            return dt;
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        #endregion


        #region  Appointment

        public static DataTable GetLocalAppointmentData(string Service_Install_Id, string Appt_EHR_ID = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);
                DateTime ToDate = Utility.LastSyncDateAditServer;
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalAppointmentData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

                    DateTime ToDate = Utility.LastSyncDateAditServer;
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalAppointmentData;
                        if (!String.IsNullOrEmpty(Appt_EHR_ID))
                        {
                            SqlCeSelect = SqlCeSelect + " And Appt_EHR_ID = '" + Appt_EHR_ID + "';";
                            if (ToDate == default(DateTime))
                            {
                                ToDate = Utility.Datetimesetting().AddDays(-7);
                            }
                        }
                        else
                        {
                            SqlCeSelect = SqlCeSelect + ";";
                        }
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                            }
                            return SqlCeDt;
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
        public static DataTable GetLocalAppointmentDataWithEhrId(string appointment_id, string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalAppointmentDataAppointmentWise;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("appointment_id", appointment_id);
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                    SqlCeCommand SqlCeCommand = null;
                    SqlCeDataAdapter SqlCeDa = null;
                    //  CommonDB.LocalConnectionServer(ref conn);

                    if (conn.State == ConnectionState.Closed) conn.Open();

                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalAppointmentDataAppointmentWise;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.AddWithValue("appointment_id", appointment_id);
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter sqlDa = new SqlCeDataAdapter(sqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                            }
                            return SqlCeDt;
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

        public static DataTable GetLocalAppointmentData_AllRecords(string Service_Install_Id)
        {
            try
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalAppointmentData_AllRecords;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetLocalCompareForDeletedAppointmentData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                DateTime ToDate = Utility.LastSyncDateAditServer;

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalCompareForDeletedAppointmentData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                    DateTime ToDate = Utility.LastSyncDateAditServer;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetLocalCompareForDeletedAppointmentData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter sqlDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                sqlDa.Fill(SqlCeDt);
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

        public static DataTable GetLocalAppointmentConformStatusData(string appointment_id, string Service_Install_Id, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalAppointmentConformStatusData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("appointment_id", appointment_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter sqlDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            sqlDa.Fill(SqlCeDt);
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

        public static bool UpdateLocalAppointmentConformStatusData(string appointment_id, string Service_Install_Id, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.UpdateLocalAppointmentConformStatusData;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("appointment_id", appointment_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Update Local Appointment Conform Status for Appointment_id=" + appointment_id + " and Service_Install_Id : " + Service_Install_Id);
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

        public static DataTable GetLocalNewWebAppointmentData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalNewWebAppointmentData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

            #region SqlCEConnection
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetLocalNewWebAppointmentData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLastTwoDaysLocalAppointmentData(string strApptID)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                string SqlCeSelect = string.Empty;
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    if (Utility.Application_ID == 11)
                    {
                        SqlCeSelect = SynchLocalQRY.GetLastTowDaysLocalAppointmentDataForAbeldent;
                    }
                    else if (Utility.Application_ID == 4)
                    {
                        SqlCeSelect = SynchLocalQRY.GetLastTowDaysLocalAppointmentDataSoftdent;
                    }
                    else
                    {
                        SqlCeSelect = SynchLocalQRY.GetLastTowDaysLocalAppointmentData;
                    }

                    //SqlCeSelect = SqlCeSelect.Replace("?", "@ToDate");
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //SqlCeCommand.Parameters.AddWithValue("ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
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
                    string SqlCeSelect = string.Empty;
                    bool isFromEvent = !string.IsNullOrEmpty(strApptID) ? true : false;
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        if (Utility.Application_ID == 11)
                        {
                            SqlCeSelect = !isFromEvent ? SynchLocalQRY.GetLastTowDaysLocalAppointmentDataForAbeldent : SynchLocalQRY.GetLastTowDaysLocalAppointmentDataForAbeldentByAptID;
                        }
                        else
                        {
                            SqlCeSelect = !isFromEvent ? SynchLocalQRY.GetLastTowDaysLocalAppointmentData : SynchLocalQRY.GetLastTowDaysLocalAppointmentDataByAptID;
                        }
                        if (isFromEvent)
                        {
                            SqlCeSelect = SqlCeSelect.Replace("@Appt_EHR_ID", strApptID);
                        }
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            // SqlCeCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(sqlCeCommand))
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

        public static DataTable GetIs_Appt_DoubleBook_AppointmentData(string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();

                    string SqlCeSelect = SynchLocalQRY.GetIs_Appt_DoubleBook_AppointmentData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        // SqlCeCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
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

        public static DataTable GetLocalPozativeAppointmentData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalPozativeAppointmentData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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
                    try
                    {
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalPozativeAppointmentData;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(sqlCeCommand))
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

        public static bool Save_Appointment_Is_Appt_DoubleBook_In_Local(string Appt_Web_ID, string Service_Install_Id, DataTable dtApponitmentConflict, string appointment_EHR_id, string locationid)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                string Appointment_WEB_Id = "";
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Is_Appt_DoubleBook_flg;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_Web_ID", Appt_Web_ID.ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.SelectAppointmentEHRIdFromAppointment;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Appt_EHR_ID", appointment_EHR_id.ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

                        object result = SqlCeCommand.ExecuteScalar();
                        if (result != null)
                        {
                            Appointment_WEB_Id = result.ToString();
                        }
                    }
                    PushLiveDatabaseDAL.SendRecordsToAditAppForAppointmentDoubleBook(Appointment_WEB_Id, Appt_Web_ID, appointment_EHR_id, locationid, dtApponitmentConflict);

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Is_Appt_DoubleBook;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Web_ID", Appt_Web_ID.ToString());
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




        #endregion

        #region  OperatoryEvent

        public static DataTable GetLocalOperatoryEventData(string Service_Install_Id)
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
                    string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryEventData;
                    //SqlCeSelect = SqlCeSelect.Replace("?", "@ToDate");
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                        string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryEventData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetPushLocalOperatoryEventData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalOperatoryEventData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalOperatoryEventData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetPushLocalOperatoryDayOffData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalOperatoryEventData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalOperatoryDayOffData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable DeleteLocalOperatoryEventData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.DeleteAbeldentLocalOperatoryEventData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.DeleteAbeldentLocalOperatoryEventData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;                           
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

        public static void DeleteLocalOperatoryEventDataAll()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;                
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.DeleteAbeldentLocalOperatoryEventDataAll;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.ExecuteNonQuery();
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.DeleteAbeldentLocalOperatoryEventDataAll;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();                            
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

        #endregion

        #region  Provider

        public static DataTable GetLocalProviderData(string Clinic_Number, string ServiceInstallId)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalProviderData_Clinic : SynchLocalQRY.GetLocalProviderData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", ServiceInstallId);
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalProviderData_Clinic : SynchLocalQRY.GetLocalProviderData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", ServiceInstallId);
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetPushLocalProviderData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalProviderData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

            #region SqlCeconnection
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalProviderData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        #endregion

        #region  ProviderHours

        public static DataTable GetLocalOperatoryHoursData_BlankStructure()
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryHoursData_BlankStructure;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetLocalProviderHoursData_BlankStructure()
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalProviderHoursData_BlankData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetLocalProviderHoursData(string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalProviderHoursData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable DeleteAbeldentLocalProviderHoursData(string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.DeleteAbelDent_ProviderHours;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);                       
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

        public static void DeleteAbeldentLocalProviderHoursAll()
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.DeleteAbelDent_ProviderHoursAll;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;                        
                        SqlCeCommand.ExecuteNonQuery();
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

        public static DataTable GetPushLocalProviderHoursData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalProviderHoursData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        #region  Speciality

        public static DataTable GetLocalSpecialityData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalSpecialityData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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
                    try
                    {
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalSpecialityData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetPushLocalSpecialityData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalSpecialityData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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
                    try
                    {
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalSpecialityData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        #endregion

        #region FolderList Sync
        public static DataTable GetLocalFolderListData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalFolderListData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalFolderListData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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
        #endregion

        #region  Operatory

        public static DataTable GetLocalOperatoryData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetLocalOperatoryDataForHours(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);
                string SqlCeSelect = string.Empty;
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    SqlCeSelect = "SELECT * FROM Operatory where Service_Install_Id = @Service_Install_Id AND Operatory_EHR_ID in (@Operatoryids)";
                    string joinedstring = string.Join(",", Utility.CustomhourOperatoryIds);
                    //SqlCeSelect = SqlCeSelect.Replace("@Operatoryids", joinedstring);
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    SqlCeCommand.Parameters.AddWithValue("Operatoryids", joinedstring);
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
                    string SqlCeSelect = "";
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        SqlCeSelect = "SELECT * FROM Operatory where Operatory_EHR_ID in (@Operatoryids) AND Service_Install_Id = @Service_Install_Id";
                        string joinedstring = string.Join(",", Utility.CustomhourOperatoryIds);
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.Parameters.AddWithValue("Operatoryids", joinedstring);
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetPushLocalFolderListData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalFolderListData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

            #region SqlCeConnecdtion
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalFolderListData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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


        public static DataTable GetPushLocalOperatoryData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalOperatoryData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

            #region SqlCeConnecdtion
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalOperatoryData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetLocalOperatoryChairData(string Service_Install_Id)
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
                    string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryChairData;
                    SqlCeSelect = SqlCeSelect.Replace("?", "@ToDate");
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                        string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryChairData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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


        #endregion

        #region  OperatoryHours

        public static DataTable GetLocalOperatoryHoursData(string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryHoursData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetLocalOperatoryOfficeHoursData(string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalOperatoryOfficeHoursData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetPushLocalOperatoryHoursData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalOperatoryHoursData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetPushLocalOperatoryOfficeHoursData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalOperatoryOfficeHoursData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetLocalOperatoryHoursBlankStructure()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.OperatoryHoursBlankStructure;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLocalOperatoryOfficeHoursBlankStructure()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.OperatoryOfficeHoursBlankStructure;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable DeleteLocalOperatoryHoursData(string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.Delete_AbelDentOperatoryHours;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static void DeleteLocalOperatoryHoursAll()
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.Delete_AbelDentOperatoryHoursAll;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.ExecuteNonQuery();
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

        #region  Appointment Type

        public static DataTable GetLocalApptTypeData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalApptTypeData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                    try
                    {
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalApptTypeData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetPushLocalApptTypeData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalApptTypeData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalApptTypeData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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

        #endregion

        #region  Patient

        public static DataTable GetLocalInsertPatientData(string Service_Install_Id)
        {
            try
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string SqlCeSelect = SynchLocalQRY.GetLocalInsertPatientData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetLocalPatientData(string Service_Install_Id, string Pat_EHR_ID = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientData;
                    if (Utility.Application_Name.ToLower().Trim() == "eaglesoft")
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientDataEaglesoft;
                    }
                    //Utility.WriteToErrorLogFromAll("GetLocalPatientData Query:" + SqlCeSelect);
                    //Utility.WriteToErrorLogFromAll("Service_Install_Id:" + Service_Install_Id);
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    CommonDB.SqlServerDataAdapter(SqlCeCommand, ref SqlCeDa);
                    DataTable SqlCeDt = new DataTable();
                    SqlCeDa.Fill(SqlCeDt);

                    return SqlCeDt;
                }
                catch (Exception ex)
                {
                    //Utility.WriteToErrorLogFromAll("GetLocalPatientData Error:" + ex.Message + System.Environment.NewLine + ex.StackTrace);
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
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalPatientData;
                        if (Utility.Application_Name.ToLower().Trim() == "eaglesoft")
                        {
                            SqlCeSelect = SynchLocalQRY.GetLocalPatientDataEaglesoft;
                        }
                        if (!string.IsNullOrEmpty(Pat_EHR_ID))
                        {
                            SqlCeSelect = SqlCeSelect + " And Patient_EHR_ID = '" + Pat_EHR_ID + "'; ";
                        }
                        else
                        {
                            SqlCeSelect = SqlCeSelect + "";
                        }

                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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
        public static DataTable GetLocalNewPatientData(string Service_Install_Id)
        {
            #region SqlCeConnection
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalNewPatientData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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


            #endregion
        }
        public static DataTable GetLocalOpenDentalLanguageList()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalOpenDentalLanguageList;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalOpenDentalLanguageList;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLocalPatientCompareDeletedData(string Service_Install_Id)
        {
            #region SqlCeConnection
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientCompareDeletedData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

            #endregion
        }

        public static DataTable GetLocalPatientImagesData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientImagesData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetLocalPatientImagesData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientDataByPatientEHRID(string PatientEHRIDs, string Service_Install_Id)
        {
            #region SqlCeConnection

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientDataByPatientEHRIds;
                    SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDS", PatientEHRIDs);
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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


            #endregion
        }


        public static DataTable GetLocalPatientFormData(string Clinic_Number, string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientFormData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientFormDiseaseResponseToSaveINEHR(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientFormDiseaseResponseToSaveINEHR;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And DR.PatientForm_Web_ID = '" + strPatientFormID + "'";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientFormDiseaseDeleteResponseToSaveINEHR(string Service_Install_Id, string strPatientFormId = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientFormDiseaseDeleteResponseToSaveINEHR;
                    if (!string.IsNullOrEmpty(strPatientFormId))
                    {
                        SqlCeSelect = SqlCeSelect + " And DR.PatientForm_Web_ID = '" + strPatientFormId + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientFormDiseaseResponse(string Clinic_Number, string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalPatientFormDiseaseResponse_Clinic : SynchLocalQRY.GetLocalPatientFormDiseaseResponse;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientFormDiseaseDeleteResponse(string Clinic_Number, string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalPatientFormDiseaseDeleteResponse_Clinic : SynchLocalQRY.GetLocalPatientFormDiseaseDeleteResponse;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
        public static DataTable GetLocalPatientFormDocData(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientFormDocData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PF.PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientFormDocAttachmentData(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientFormDocAttachmentData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PD.PatientForm_Web_Id = '" + strPatientFormID + "'";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLivePatientFormDocData(string Service_Install_Id, string strPatientFormID = "")
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLivePatientFormDocData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PF.PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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
        public static DataTable GetLivePatientFormDocAttachmentData(string Service_Install_Id, string PatientForm_Web_ID)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLivePatientFormDocAttachmentData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.Add("PatientForm_Web_ID", PatientForm_Web_ID);
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

        public static bool CheckLivePatientFormDocDataSynced(string Service_Install_Id, string PatientDoc_Web_ID)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLivePatientFormDocDataSynced;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.Add("PatientDoc_Web_ID", PatientDoc_Web_ID);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            if (SqlCeDt.Rows.Count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
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
        }

        public static bool CheckLivePatientFormDocAttachmentDataSynced(string Service_Install_Id, string PatientDoc_Web_ID)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLivePatientFormDocAttachmentDataSynced;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.Add("PatientDoc_Web_ID", PatientDoc_Web_ID);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            if (SqlCeDt.Rows.Count > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
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
        }
        public static DataTable GetLiveDentrixPatientFormDocData(string Service_Install_Id, string strPatientFormID = "")
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveDentrixPatientFormDocData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PF.PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("ToDate", OdbcType.Date).Value = new DateTime(2023, 4, 12).ToString("yyyy/MM/dd");
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLiveDentrixPatientFormDocAttachmentData(string Service_Install_Id, string PatientForm_web_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveDentrixPatientFormDocAttachmentData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("ToDate", OdbcType.Date).Value = new DateTime(2023, 4, 12).ToString("yyyy/MM/dd");
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.Add("PatientForm_web_Id", PatientForm_web_Id);

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
        public static DataTable GetLocaleTreatmentDocData(string strTreatmentPlanID = "")
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveTreatmentFormDocData;
                    if (!string.IsNullOrEmpty(strTreatmentPlanID))
                    {
                        SqlCeSelect = SqlCeSelect + " And TreatmentDoc_Web_ID = '" + strTreatmentPlanID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocaleInsuranceCarrierDocData(string strInsuranceCarrierID = "", bool fromDentrix = false)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveInsuranceCarrierDocData;
                    if (fromDentrix)
                    {
                        SqlCeSelect = SynchLocalQRY.GetLiveDentrixInsruanceCarrierDocData;
                    }
                    if (!string.IsNullOrEmpty(strInsuranceCarrierID))
                    {
                        if (fromDentrix)
                        {
                            SqlCeSelect = SqlCeSelect + " And ICD.InsuranceCarrierDoc_Web_ID = '" + strInsuranceCarrierID + "' ";
                        }
                        else
                        {
                            SqlCeSelect = SqlCeSelect + " And InsuranceCarrierDoc_Web_ID = '" + strInsuranceCarrierID + "' ";
                        }
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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


        public static DataTable GetLiveDentrixPatientFormMedicalHistoryData(string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveDentrixPatientFormMedicalHistoryData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLiveAbelDentPatientFormMedicalHistoryData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveAbelDentPatientFormMedicalHistoryData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLiveEasyDentalPatientFormMedicalHistoryData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveEasyDentalPatientFormMedicalHistoryData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLiveCleardentPatientFormMedicalHistoryData(string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveCleardentPatientFormMedicalHistoryData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLiveOpenDentalPatientFormMedicalHistoryData(string Clinic_Number, string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLiveOpenDentalPatientFormMedicalHistoryData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_Id = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPendingPatientFormData(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPendingPatientFormData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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
        public static DataTable GetLocalPendingPatientFormDocAttachmentData(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPendingPatientFormDocAttachmentData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_Id = '" + strPatientFormID + "'";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPendingTreatmentDocData(string Service_Install_Id, string strTreatmentPlanID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPendingTreatmentDocData;
                    if (!string.IsNullOrEmpty(strTreatmentPlanID))
                    {
                        SqlCeSelect = SqlCeSelect + " And TreatmentDoc_Web_ID = '" + strTreatmentPlanID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetMedicalHistoryPatientWithForm(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetMedicalHistoryPatientWithForm;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PF.PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static bool UpdateMedicalHistorySubmitRecords(string patientform_web_id, string formMaster_EHR_id, Int64 formInstanceId, string DbString, string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                DataTable dtMedicalHistoryQuestion = new DataTable();
                DataTable dtMedicalHistoryAnswer = new DataTable();
                int resultvalue = 0;
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetMedicalHistorySubmit.Replace("MedicalHistorySubmitTable", "EagleSoft_MHF_Question_Submit");
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", patientform_web_id);
                        SqlCeCommand.Parameters.AddWithValue("FormMaster_EHR_id", formMaster_EHR_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        DataTable SqlCeDt = null;
                        Utility.WriteToSyncLogFile_All(SqlCeSelect);
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDa.Fill(dtMedicalHistoryQuestion);
                        }
                    }
                    SqlCeSelect = SynchLocalQRY.GetMedicalHistorySubmit.Replace("MedicalHistorySubmitTable", "EagleSoft_MHF_Answer_Submit");
                    Utility.WriteToSyncLogFile_All(SqlCeSelect);
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", patientform_web_id);
                        SqlCeCommand.Parameters.AddWithValue("FormMaster_EHR_id", formMaster_EHR_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDa.Fill(dtMedicalHistoryAnswer);
                        }
                    }
                    foreach (DataRow drRow in dtMedicalHistoryQuestion.Rows)
                    {
                        if (Convert.ToInt16(drRow["SectionItemType"]) == 0 && drRow["Question_type"] != null && drRow["Question_type"].ToString() != "" && Convert.ToInt16(drRow["Question_type"]) == 0 && Convert.ToInt16(drRow["Answer_Type"]) == 1)
                        {
                            foreach (DataRow drAns in dtMedicalHistoryAnswer.Select(" FormMaster_EHR_id = '" + formMaster_EHR_id.ToString() + "' AND  SectionItem_EHR_Id = '" + drRow["SectionItem_EHR_Id"].ToString() + "'"))
                            {
                                resultvalue = 0;
                                if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "TRUE")
                                {
                                    resultvalue = 1;
                                }
                                else if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "FALSE")
                                {
                                    resultvalue = 2;
                                }
                                if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "YES")
                                {
                                    resultvalue = 1;
                                }
                                else if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "NO")
                                {
                                    resultvalue = 2;
                                }
                                if (resultvalue > 0)
                                {                                   
                                    SynchEaglesoftDAL.UpdateAnswerInEagleSoftListAnswer("List_Item_Answer", formInstanceId, resultvalue, drAns["SectionItemOptionMaster_EHR_Id"].ToString(), DbString);
                                }
                            }
                        }
                        else if (Convert.ToInt16(drRow["SectionItemType"]) == 0 && drRow["Question_type"] != null && drRow["Question_type"].ToString() != "" && Convert.ToInt16(drRow["Question_type"]) == 0 && Convert.ToInt16(drRow["Answer_Type"]) == 0)
                        {
                            foreach (DataRow drAns in dtMedicalHistoryAnswer.Select(" FormMaster_EHR_id = '" + formMaster_EHR_id.ToString() + "' AND  SectionItem_EHR_Id = '" + drRow["SectionItem_EHR_Id"].ToString() + "'"))
                            {
                                resultvalue = 0;
                                if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "TRUE")
                                {
                                    resultvalue = 1;
                                }
                                else if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "FALSE")
                                {
                                    resultvalue = 2;
                                }

                                if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "YES")
                                {
                                    resultvalue = 1;
                                }
                                else if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "NO")
                                {
                                    resultvalue = 2;
                                }
                                if (resultvalue > 0)
                                {
                                    SynchEaglesoftDAL.UpdateAnswerInEagleSoftListAnswer("List_Item_Answer", formInstanceId, resultvalue, drAns["SectionItemOptionMaster_EHR_Id"].ToString(), DbString);
                                }
                            }
                        }
                        else if (Convert.ToInt16(drRow["SectionItemType"]) == 0 && drRow["Question_type"] != null && drRow["Question_type"].ToString() != "" && Convert.ToInt16(drRow["Question_type"]) == 1 && (Convert.ToInt16(drRow["Answer_Type"]) == 0 || Convert.ToInt16(drRow["Answer_Type"]) == 1))
                        {
                            resultvalue = 0;
                            if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "TRUE")
                            {
                                resultvalue = 1;
                            }
                            else if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "FALSE")
                            {
                                resultvalue = 2;
                            }
                            if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "YES")
                            {
                                resultvalue = 1;
                            }
                            else if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "NO")
                            {
                                resultvalue = 2;
                            }
                            if (resultvalue > 0)
                            {
                                SynchEaglesoftDAL.UpdateAnswerInEagleSoftSignleQuestionAnswer("Single_Question_answer ", formInstanceId, resultvalue, drRow["SectionItem_EHR_Id"].ToString(), drRow["AnsValue"].ToString(), DbString);
                            }
                        }
                        else if (Convert.ToInt16(drRow["SectionItemType"]) == 0 && drRow["Question_type"] != null && drRow["Question_type"].ToString() != "" && Convert.ToInt16(drRow["Answer_Type"]) == 2)
                        {
                            resultvalue = 0;
                            if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "TRUE")
                            {
                                resultvalue = 1;
                            }
                            else if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "FALSE")
                            {
                                resultvalue = 2;
                            }
                            if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "YES")
                            {
                                resultvalue = 1;
                            }
                            else if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "NO")
                            {
                                resultvalue = 2;
                            }
                            if (resultvalue > 0)
                            {
                                SynchEaglesoftDAL.UpdateAnswerInEagleSoftSignleQuestionAnswerFeelFree("Single_Question_answer ", formInstanceId, resultvalue, drRow["SectionItem_EHR_Id"].ToString(), drRow["AnsValue"].ToString(), drRow["AnsKey"].ToString(), DbString);
                            }
                        }
                        else if (Convert.ToInt16(drRow["SectionItemType"]) == 3 && drRow["Question_type"].ToString() == "" && drRow["Answer_Type"].ToString() == "")
                        {
                            SynchEaglesoftDAL.UpdateAnswerInEagleSoftCommentAnswer("Comment_answer", formInstanceId, drRow["AnsValue"].ToString(), drRow["SectionItem_EHR_Id"].ToString(), DbString);
                        }
                    }
                    SynchEaglesoftDAL.UpdateFormInstance(formInstanceId, DbString);

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
        }

        public static bool InsertMedicalHistorySubmitRecords(string patientform_web_id, string formMaster_EHR_id, string patient_ehr_id, string DbString, string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                DataTable dtMedicalHistoryQuestion = new DataTable();
                DataTable dtMedicalHistoryAnswer = new DataTable();
                int resultvalue = 0;
                Int64 formInstanceId = 0;
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetMedicalHistorySubmit.Replace("MedicalHistorySubmitTable", "EagleSoft_MHF_Question_Submit");
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        Utility.WriteToSyncLogFile_All(SqlCeSelect);
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", patientform_web_id);
                        SqlCeCommand.Parameters.AddWithValue("FormMaster_EHR_id", formMaster_EHR_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDa.Fill(dtMedicalHistoryQuestion);
                        }
                    }
                    SqlCeSelect = SynchLocalQRY.GetMedicalHistorySubmit.Replace("MedicalHistorySubmitTable", "EagleSoft_MHF_Answer_Submit");
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        Utility.WriteToSyncLogFile_All(SqlCeSelect);
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", patientform_web_id);
                        SqlCeCommand.Parameters.AddWithValue("FormMaster_EHR_id", formMaster_EHR_id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDa.Fill(dtMedicalHistoryAnswer);
                        }
                    }
                    #region First Insert Records in FormInstance Table
                    formInstanceId = SynchEaglesoftDAL.InsertFormInstance(formMaster_EHR_id, patient_ehr_id, DbString);
                    #endregion

                    foreach (DataRow drRow in dtMedicalHistoryQuestion.Rows)
                    {
                        if (Convert.ToInt16(drRow["SectionItemType"]) == 0 && drRow["Question_type"] != null && drRow["Question_type"].ToString() != "" && Convert.ToInt16(drRow["Question_type"]) == 0 && Convert.ToInt16(drRow["Answer_Type"]) == 1)
                        {
                            foreach (DataRow drAns in dtMedicalHistoryAnswer.Select(" FormMaster_EHR_id = '" + formMaster_EHR_id.ToString() + "' AND  SectionItem_EHR_Id = '" + drRow["SectionItem_EHR_Id"].ToString() + "'"))
                            {
                                resultvalue = 0;
                                if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "TRUE")
                                {
                                    resultvalue = 1;
                                }
                                else if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "FALSE")
                                {
                                    resultvalue = 2;
                                }

                                if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "YES")
                                {
                                    resultvalue = 1;
                                }
                                else if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "NO")
                                {
                                    resultvalue = 2;
                                }

                                SynchEaglesoftDAL.InsertAnswerInEagleSoftListAnswer("List_Item_Answer", formInstanceId, resultvalue, drAns["SectionItemOptionMaster_EHR_Id"].ToString(), DbString);
                                //Save Allergies and Problems in Alerts https://app.asana.com/0/1199184824722493/1207565872811101/f
                                if (resultvalue > 0)
                                {
                                    // insert into Patient_alert table in EHR by sectionID,join with alert id for diseasemaster Pk join.
                                    //inert into PatientDiseasemaster in local db for patient allergy note.
                                    try
                                    {
                                        //rooja 23-8-24 - save allergies in local and EHR for allergic question options selected here
                                        if (SynchEaglesoftDAL.SaveAllergiesFromMedicalHistoryFormToEaglesoft(DbString, Service_Install_Id, drAns["SectionItemOptionMaster_EHR_Id"].ToString(), patient_ehr_id, patientform_web_id))
                                        {
                                            Utility.WriteToSyncLogFile_All("Patient_Alert from Medical history Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                                        }
                                        else
                                        {
                                            Utility.WriteToErrorLogFromAll("[Patient_Alert from Medical history Form Sync (Local Database To " + Utility.Application_Name + ") ]");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Utility.WriteToErrorLogFromAll("[Patient_Alert from Medical history Form Sync (Local Database To " + Utility.Application_Name + ") ] Error :" + ex.Message.ToString());
                                    }
                                }
                            }
                        }
                        else if (Convert.ToInt16(drRow["SectionItemType"]) == 0 && drRow["Question_type"] != null && drRow["Question_type"].ToString() != "" && Convert.ToInt16(drRow["Question_type"]) == 0 && Convert.ToInt16(drRow["Answer_Type"]) == 0)
                        {
                            foreach (DataRow drAns in dtMedicalHistoryAnswer.Select(" FormMaster_EHR_id = '" + formMaster_EHR_id.ToString() + "' AND  SectionItem_EHR_Id = '" + drRow["SectionItem_EHR_Id"].ToString() + "'"))
                            {
                                resultvalue = 0;
                                if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "TRUE")
                                {
                                    resultvalue = 1;
                                }
                                else if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "FALSE")
                                {
                                    resultvalue = 2;
                                }

                                if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "YES")
                                {
                                    resultvalue = 1;
                                }
                                else if (drAns["AnsKey"] != null && drAns["AnsKey"].ToString().ToUpper() == "NO")
                                {
                                    resultvalue = 2;
                                }

                                SynchEaglesoftDAL.InsertAnswerInEagleSoftListAnswer("List_Item_Answer", formInstanceId, resultvalue, drAns["SectionItemOptionMaster_EHR_Id"].ToString(), DbString);
                            }
                        }
                        else if (Convert.ToInt16(drRow["SectionItemType"]) == 0 && drRow["Question_type"] != null && drRow["Question_type"].ToString() != "" && Convert.ToInt16(drRow["Question_type"]) == 1 && (Convert.ToInt16(drRow["Answer_Type"]) == 0 || Convert.ToInt16(drRow["Answer_Type"]) == 1))
                        {
                            resultvalue = 0;
                            if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "TRUE")
                            {
                                resultvalue = 1;
                            }
                            else if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "FALSE")
                            {
                                resultvalue = 2;
                            }

                            if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "YES")
                            {
                                resultvalue = 1;
                            }
                            else if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "NO")
                            {
                                resultvalue = 2;
                            }

                            SynchEaglesoftDAL.InsertAnswerInEagleSoftSingleQuestionAnswer("Single_Question_answer ", formInstanceId, resultvalue, drRow["SectionItem_EHR_Id"].ToString(), drRow["AnsValue"].ToString(), DbString);
                            //}
                        }
                        else if (Convert.ToInt16(drRow["SectionItemType"]) == 0 && drRow["Question_type"] != null && drRow["Question_type"].ToString() != "" && Convert.ToInt16(drRow["Question_type"]) == 1 && Convert.ToInt16(drRow["Answer_Type"]) == 2)
                        {
                            resultvalue = 0;
                            if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "TRUE")
                            {
                                resultvalue = 1;
                            }
                            else if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "FALSE")
                            {
                                resultvalue = 2;
                            }

                            if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "YES")
                            {
                                resultvalue = 1;
                            }
                            else if (drRow["AnsKey"] != null && drRow["AnsKey"].ToString().ToUpper() == "NO")
                            {
                                resultvalue = 2;
                            }

                            SynchEaglesoftDAL.InsertAnswerInEagleSoftSingleQuestionAnswerFeelFree("Single_Question_answer ", formInstanceId, resultvalue, drRow["SectionItem_EHR_Id"].ToString(), drRow["AnsValue"].ToString(), drRow["AnsKey"].ToString(), DbString);
                            //}
                        }
                        else if (Convert.ToInt16(drRow["SectionItemType"]) == 3 && drRow["Question_type"].ToString() == "" && drRow["Answer_Type"].ToString() == "")
                        {
                            SynchEaglesoftDAL.InsertAnswerInEagleSoftCommentAnswer("Comment_answer", formInstanceId, drRow["AnsValue"].ToString(), drRow["SectionItem_EHR_Id"].ToString(), DbString);
                        }
                    }
                    SynchEaglesoftDAL.UpdateFormInstance(formInstanceId, DbString);

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
        }


        public static bool UpdateDiseaseEHR_Updateflg(string dtEHRDiseaseLocalid, string allergypatientId, string patient_EHR_Id, string Disease_EHR_Id, string Service_Install_Id)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                string SqlCeSelect = string.Empty;
                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Disease_EHR_Update_Flg;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", dtEHRDiseaseLocalid);
                        SqlCeCommand.Parameters.AddWithValue("Allergy_Patient_EHR_Id", allergypatientId);
                        SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", patient_EHR_Id);
                        SqlCeCommand.Parameters.AddWithValue("Disease_EHR_Id", Disease_EHR_Id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        //SqlCetx.Commit();
                        // Utility.WriteToErrorLogFromAll(SqlCeCommand.CommandText + "," + dtEHRDiseaseLocalid + "," + allergypatientId + "," + patient_EHR_Id + "," + Service_Install_Id);
                        is_Update_PatientForm_Flg = true;
                    }
                }
                catch (Exception)
                {
                    // SqlCetx.Rollback();
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;

        }

        public static bool UpdateDeleteDiseaseEHR_Updateflg(string Disease_EHR_Id, string patient_EHR_Id, string Service_Install_Id)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                string SqlCeSelect = string.Empty;
                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_DeleteDisease_EHR_Update_Flg;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Disease_EHR_Id", Disease_EHR_Id);
                        SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", patient_EHR_Id);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        //SqlCetx.Commit();
                        is_Update_PatientForm_Flg = true;
                    }
                }
                catch (Exception)
                {
                    // SqlCetx.Rollback();
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;

        }

        public static DataTable GetLocalPendingPatientFormMedicalHistory(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPendingPatientFormMedicalHistory;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetPushLocalPatientData(string strPatID = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalPatientData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalPatientData;
                        if (!string.IsNullOrEmpty(strPatID))
                        {
                            SqlCeSelect = SqlCeSelect + " And Patient_EHR_ID in (" + strPatID + ")";
                        }
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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


        public static DataTable GetAllLocalPatientData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetAllLocalPatientData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetAllLocalPatientData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetAllLocalActivePatientData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetAllLocalActivePatientData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetAllLocalActivePatientData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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


        public static DataTable GetPushLocalPatientStatusData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalPatientStatusData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetPushLocalPatientStatusData(int Service_Install_Id, int clinicnumber)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalPatientStatusData_ClinicWise;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.Add("Clinic_Number", clinicnumber);
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

        #region Patient Form

        public static DataTable GetLocalNewWebPatient_FormData(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalNewWebPatient_FormData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_ID = '" + strPatientFormID + "'";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalWebPatientPaymentData(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalWebPatientPaymentData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetPatientPaymentTableBlankStructure()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPatientPaymentTableBlankStructure;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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



        //public static DataTable GetLocalWebPatientPaymentSplitData(string Service_Install_Id)
        //{
        //    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
        //    {
        //        if (conn.State == ConnectionState.Closed) conn.Open();
        //        try
        //        {
        //            string SqlCeSelect = SynchLocalQRY.GetLocalWebPatientPaymentSplitData;
        //            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
        //            {
        //                SqlCeCommand.CommandType = CommandType.Text;
        //                SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
        //                DataTable SqlCeDt = null;
        //                using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
        //                {
        //                    SqlCeDt = new DataTable();
        //                    SqlCeDa.Fill(SqlCeDt);
        //                    return SqlCeDt;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            if (conn.State == ConnectionState.Open) conn.Close();
        //        }
        //    }
        //}


        public static DataTable GetLocalWebPatientPaymentDataErroAPI(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalWebPatientPaymentDataErroAPI;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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
        public static DataTable GetLocalWebPatientSMSCallDataErroAPI(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalWebPatientSMSCallDataErroAPI;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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
        public static void UpdateWebPatientPaymentDataErroAPI()
        {
            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
            {
                #region Get Completed in EHR but Error IN API call Completed
                DataTable dtPatientPaymentErrAPI = SynchLocalDAL.GetLocalWebPatientPaymentDataErroAPI(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                foreach (DataRow drRow in dtPatientPaymentErrAPI.Rows)
                {
                    SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(drRow["Patient_Web_ID"].ToString().Trim(), drRow["PatientPaymentWebId"].ToString().Trim(), "completed", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim(), drRow["Clinic_Number"].ToString().Trim(), "", drRow["PaymentEHRId"].ToString(), "", "", "", Convert.ToInt32(drRow["TryInsert"]));
                }
                #endregion
            }
        }
        public static void UpdateWebPatientSMSCallDataErroAPI()
        {
            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
            {
                #region Get Completed in EHR but Error IN API call Completed
                DataTable dtPatientPaymentErrAPI = SynchLocalDAL.GetLocalWebPatientPaymentDataErroAPI(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                if (!dtPatientPaymentErrAPI.Columns.Contains("Log_Status"))
                {
                    dtPatientPaymentErrAPI.Columns.Add("Log_Status", typeof(string));
                    dtPatientPaymentErrAPI.Columns["Log_Status"].DefaultValue = "completed";
                }
                DataTable dtResultCopy = new DataTable();
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (dtPatientPaymentErrAPI.Rows.Count > 0)
                    {
                        dtResultCopy = dtPatientPaymentErrAPI.Select("Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' and Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                        if (dtResultCopy.Rows.Count > 0)
                        {
                            SynchLocalDAL.CallPatientSMSCallAPIForStatusCompleted(dtResultCopy, "completed", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString().Trim(), Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                        }
                    }
                }
                #endregion
            }
        }


        public static DataTable GetLocalWebPatientSMSCallLogData(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //Utility.WriteToErrorLogFromAll("Call Query to get local data " + SynchLocalQRY.GetLocalWebPatientSMSCallLogData.ToString());
                    string SqlCeSelect = SynchLocalQRY.GetLocalWebPatientSMSCallLogData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientForm_Importing_PendingData(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientForm_Importing_PendingData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientForm_completed_PendingData(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientForm_completed_PendingData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static bool UpdatePatientFormEHR_Updateflg(DataTable dtEHRUPdate_Data)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string SqlCeSelect = string.Empty;
                try
                {
                    foreach (DataRow dr in dtEHRUPdate_Data.Rows)
                    {
                        SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_EHR_Update_Flg;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_id", dr["PatientForm_Web_id"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString());
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    //SqlCetx.Commit();
                    is_Update_PatientForm_Flg = true;
                }
                catch (Exception)
                {
                    // SqlCetx.Rollback();
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;

        }


        public static bool UpdatePatientPortalEHR_Updateflg(DataTable dtEHRUPdate_Data)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string SqlCeSelect = string.Empty;
                try
                {
                    foreach (DataRow dr in dtEHRUPdate_Data.Rows)
                    {
                        SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientPortal_EHR_Update_Flg;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_id", dr["PatientForm_Web_id"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString());
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    //SqlCetx.Commit();
                    is_Update_PatientForm_Flg = true;
                }
                catch (Exception)
                {
                    // SqlCetx.Rollback();
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;

        }

        public static bool UpdatePatientFormMedicalHistory_Fields(DataTable dtEHRUPdate_Data)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string SqlCeSelect = string.Empty;
                try
                {
                    foreach (DataRow dr in dtEHRUPdate_Data.Rows)
                    {
                        SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_MedicalHistory_Field;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("MedicalHistorySubmission_Id", dr["MedicalHistorySubmission_Id"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_id", dr["PatientForm_Web_id"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", dr["Service_Install_Id"].ToString());
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    is_Update_PatientForm_Flg = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;
        }

        public static bool Update_PatientForm_MedicalHistory_Field_Pushed(string patientFormWebId, string Service_Install_Id)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string SqlCeSelect = string.Empty;
                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_MedicalHistory_Field_Pushed;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_id", patientFormWebId.ToString());
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        Utility.WriteToSyncLogFile_All(SqlCeCommand.CommandText);
                        SqlCeCommand.ExecuteNonQuery();
                        is_Update_PatientForm_Flg = true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;
        }

        public static bool UpdatePatientFormSyncValue(string Service_Install_Id)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string SqlCeSelect = string.Empty;
                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_Sync_Flg;
                        SqlCeCommand.ExecuteNonQuery();
                    }
                    //SqlCetx.Commit();
                    is_Update_PatientForm_Flg = true;
                }
                catch (Exception)
                {
                    // SqlCetx.Rollback();
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;

        }

        public static bool UpdatePatientFormImportingflg(string Service_Install_Id)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //     SqlCetx = conn.BeginTransaction();
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string SqlCeSelect = string.Empty;
                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientForm_Importing_Flg;
                        SqlCeCommand.ExecuteNonQuery();
                        // SqlCetx.Commit();
                        is_Update_PatientForm_Flg = true;
                    }
                }
                catch (Exception)
                {
                    //SqlCetx.Rollback();
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;

        }

        public static string Call_API_For_PatientFormDate_Importing(string Service_Install_Id, string strPatientFormID = "")
        {
            string _successfullstataus = string.Empty;
            try
            {
                string PatientFormDate_Importing = "";
                DataTable dtPF_Importing_PendingData = GetLocalPatientForm_Importing_PendingData(Service_Install_Id, strPatientFormID);

                if (dtPF_Importing_PendingData.Rows.Count > 0)
                {
                    foreach (DataRow drPF_CPD in dtPF_Importing_PendingData.Rows)
                    {
                        PatientFormDate_Importing = PatientFormDate_Importing + drPF_CPD["PatientForm_Web_ID"].ToString() + ";";
                    }


                    string[] Update_PatientForm_Record_IDs;
                    if (PatientFormDate_Importing == string.Empty)
                    {
                        Update_PatientForm_Record_IDs = new string[0];
                    }
                    else
                    {
                        if (PatientFormDate_Importing.Substring(PatientFormDate_Importing.Length - 1) == ";")
                        {
                            PatientFormDate_Importing = PatientFormDate_Importing.Remove(PatientFormDate_Importing.Length - 1, 1);
                        }

                        Update_PatientForm_Record_IDs = PatientFormDate_Importing.ToString().Trim().Split(';');
                    }

                    var sList = new ArrayList();

                    for (int i = 0; i < Update_PatientForm_Record_IDs.Length; i++)
                    {
                        if (sList.Contains(Update_PatientForm_Record_IDs[i]) == false)
                        {
                            sList.Add(Update_PatientForm_Record_IDs[i]);
                        }
                    }

                    var sNew = sList.ToArray();
                    string newPatList = "";
                    for (int i = 0; i < sNew.Length; i++)
                    {
                        newPatList = newPatList + sNew[i] + ";";
                    }

                    string[] Update_Distinct_Patien_IDs;

                    newPatList = newPatList.Remove(newPatList.Length - 1, 1);
                    Update_Distinct_Patien_IDs = newPatList.ToString().Trim().Split(';');

                    var JsonPatient_FormBOSO = new System.Text.StringBuilder();

                    Push_Patient_FormBO Patient_FormBOSO = new Push_Patient_FormBO
                    {
                        id = Update_Distinct_Patien_IDs,
                        status = "importing"
                    };
                    var javaScriptSerializerSO = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonPatient_FormBOSO.Append(javaScriptSerializerSO.Serialize(Patient_FormBOSO) + ",");

                    string jsonStringSO = JsonPatient_FormBOSO.ToString().Remove(JsonPatient_FormBOSO.Length - 1);
                    string RestURLSO = PullLiveDatabaseDAL.GetLiveRecord("PatientFormUpdateRecordID", "");
                    var requestSO = new RestRequest(Method.POST);
                    var clientSO = new RestClient(RestURLSO);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    requestSO.AddHeader("action", "EHRPFIMPORT");
                    requestSO.AddHeader("Content-Type", "application/json");
                    requestSO.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                    requestSO.AddParameter("application/json", jsonStringSO, ParameterType.RequestBody);
                    IRestResponse responseSO = clientSO.Execute(requestSO);

                    if (responseSO.Content.ToString().Contains("Import list update Successfully"))
                    {
                        bool is_UpdateFlg = SynchLocalDAL.UpdatePatientFormImportingflg(Service_Install_Id);
                        _successfullstataus = "Success";
                    }
                    else
                    {
                        if (responseSO.Content.ToString() == "")
                        {
                            _successfullstataus = responseSO.ErrorMessage.ToString();
                        }
                        else
                        {
                            _successfullstataus = responseSO.Content.ToString();
                        }
                    }
                }
                else
                {
                    _successfullstataus = "Success";
                }
            }
            catch (Exception)
            {
                _successfullstataus = "API Call Fail";
            }

            return _successfullstataus;
        }

        public static string Call_API_For_PatientFormDate_Completed(string Service_Install_Id, string strPatientFormID = "")
        {
            string _successfullstataus = string.Empty;

            try
            {
                string strPF_completed_PendingData = "";
                DataTable dtPF_completed_PendingData = GetLocalPatientForm_completed_PendingData(Service_Install_Id, strPatientFormID);

                if (dtPF_completed_PendingData.Rows.Count > 0)
                {
                    foreach (DataRow drPF_CPD in dtPF_completed_PendingData.Rows)
                    {
                        strPF_completed_PendingData = strPF_completed_PendingData + drPF_CPD["PatientForm_Web_ID"].ToString() + ";";
                    }

                    string[] Update_PatientForm_Record_IDs;

                    if (strPF_completed_PendingData == string.Empty)
                    {
                        Update_PatientForm_Record_IDs = new string[0];
                    }
                    else
                    {
                        if (strPF_completed_PendingData.Substring(strPF_completed_PendingData.Length - 1) == ";")
                        {
                            strPF_completed_PendingData = strPF_completed_PendingData.Remove(strPF_completed_PendingData.Length - 1, 1);
                        }

                        Update_PatientForm_Record_IDs = strPF_completed_PendingData.ToString().Trim().Split(';');
                    }

                    var sList = new ArrayList();

                    for (int i = 0; i < Update_PatientForm_Record_IDs.Length; i++)
                    {
                        if (sList.Contains(Update_PatientForm_Record_IDs[i]) == false)
                        {
                            sList.Add(Update_PatientForm_Record_IDs[i]);
                        }
                    }

                    var sNew = sList.ToArray();
                    string newPatList = "";
                    for (int i = 0; i < sNew.Length; i++)
                    {
                        newPatList = newPatList + sNew[i] + ";";
                    }

                    string[] Update_Distinct_Patien_IDs;

                    newPatList = newPatList.Remove(newPatList.Length - 1, 1);
                    Update_Distinct_Patien_IDs = newPatList.ToString().Trim().Split(';');

                    var JsonPatient_FormBO = new System.Text.StringBuilder();

                    Push_Patient_FormBO Patient_FormBO = new Push_Patient_FormBO
                    {
                        id = Update_Distinct_Patien_IDs,
                        status = "completed",
                    };
                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonPatient_FormBO.Append(javaScriptSerializer.Serialize(Patient_FormBO) + ",");

                    string jsonString = JsonPatient_FormBO.ToString().Remove(JsonPatient_FormBO.Length - 1);
                    string RestURL = PullLiveDatabaseDAL.GetLiveRecord("PatientFormUpdateRecordID", "");
                    var request = new RestRequest(Method.POST);
                    var client = new RestClient(RestURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    request.AddHeader("action", "EHRPFIMPORT");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                    IRestResponse response = client.Execute(request);


                    if (response.Content.ToString().Contains("Import list update Successfully"))
                    {
                        bool is_UpdateFlg = SynchLocalDAL.UpdatePatientFormSyncValue(Service_Install_Id);
                        _successfullstataus = "Success";
                    }
                    else
                    {
                        if (response.Content.ToString() == "")
                        {
                            _successfullstataus = response.ErrorMessage.ToString();
                        }
                        else
                        {
                            _successfullstataus = response.Content.ToString();
                        }
                    }

                }
                else
                {
                    _successfullstataus = "Success";
                }
            }
            catch (Exception)
            {
                _successfullstataus = "API Call Fail";
            }

            return _successfullstataus;
        }

        #endregion

        #region  Disease

        public static DataTable GetLocalPatientDiseaseData(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientDiseaseData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);

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

        public static DataTable GetPushLocalPatientDiseaseData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalDiseaseData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

            #region SqlCeconnection
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalPatientDiseaseData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static DataTable GetLocalDiseaseData(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalDiseaseData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);

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

        public static DataTable GetPushLocalDiseaseData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalDiseaseData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

            #region SqlCeconnection
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalDiseaseData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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


        #region Medication

        public static DataTable GetLocalMedicationData(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalMedicationData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);

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

        public static DataTable GetPushLocalMedicationData()
        {
            #region SqlCeconnection

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalMedicationData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

            #endregion
        }

        public static DataTable GetLocalPatientMedicationData(string Service_Install_Id, string Patient_EHR_IDS = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = "";
                    if (Patient_EHR_IDS == string.Empty || Patient_EHR_IDS == "")
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientMedicationData;
                    }
                    else
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientMedicationData + " and Patient_EHR_ID in (" + Patient_EHR_IDS + ")";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);

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

        public static DataTable GetPushLocalPatientMedicationData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalPatientMedicationData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

            #region SqlCeconnection
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalPatientMedicationData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
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

        public static bool Save_PatientMedication_EHR_To_Local(DataTable dtPatientMedication, string Service_Install_Id)
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
                        foreach (DataRow dr in dtPatientMedication.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_PatientMedication;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientMedication;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_PatientMedication;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("PatientMedication_EHR_ID", dr["PatientMedication_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_EHR_ID", dr["Medication_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("PatientMedication_EHR_ID", dr["PatientMedication_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Note", dr["Medication_Note"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Name", dr["Medication_Name"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Type", dr["Medication_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", dr["Provider_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Drug_Quantity", dr["Drug_Quantity"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Patient_Notes", dr["Patient_Notes"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Start_Date", (dr["Start_Date"].ToString().Trim() != "" ? dr["Start_Date"].ToString().Trim() : ""));
                                    SqlCeCommand.Parameters.AddWithValue("End_Date", (dr["End_Date"].ToString().Trim() != "" ? dr["End_Date"].ToString().Trim() : ""));
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", (dr["is_deleted"].ToString().Trim().ToUpper() == "TRUE" ? 1 : 0));
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", (dr["EHR_Entry_DateTime"].ToString().Trim() != "" ? dr["EHR_Entry_DateTime"].ToString().Trim() : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()));
                                    SqlCeCommand.Parameters.AddWithValue("Is_Active", (dr["Is_Active"].ToString().Trim().ToUpper() == "TRUE" ? 1 : 0));
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
        public static bool Save_Medication_EHR_To_Local(DataTable dtMedication, string Service_Install_Id)
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
                        foreach (DataRow dr in dtMedication.Rows)
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
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_Medication;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Medication;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_Medication;
                                        break;
                                }

                                SqlCeCommand.Parameters.Clear();
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Medication_EHR_ID", dr["Medication_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Type", dr["Medication_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dr["Clinic_Number"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlCeCommand.Parameters.AddWithValue("Medication_EHR_ID", dr["Medication_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Web_ID", "");
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Name", dr["Medication_Name"].ToString().Trim());

                                    SqlCeCommand.Parameters.AddWithValue("Medication_Description", dr["Medication_Description"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Notes", dr["Medication_Notes"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Sig", dr["Medication_Sig"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Parent_EHR_ID", dr["Medication_Parent_EHR_ID"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Refills", dr["Refills"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Is_Active", (dr["Is_Active"].ToString().ToUpper().Trim() == "TRUE" ? 1 : 0));

                                    SqlCeCommand.Parameters.AddWithValue("Drug_Quantity", dr["Drug_Quantity"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Type", dr["Medication_Type"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("is_deleted", (dr["is_deleted"].ToString().Trim().ToUpper() == "TRUE" ? 1 : 0));
                                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", dr["EHR_Entry_DateTime"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Allow_Generic_Sub", dr["Allow_Generic_Sub"].ToString().Trim());
                                    SqlCeCommand.Parameters.AddWithValue("Medication_Provider_ID", dr["Medication_Provider_ID"].ToString().Trim());
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
        #endregion

        #endregion

        #region  RecallType

        public static DataTable GetLocalRecallTypeData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalRecallTypeData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetLocalRecallTypeData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalUser(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalUserData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetLocalUserData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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
        public static DataTable GetPushLocalRecallTypeData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();


                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalRecallTypeData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalRecallTypeData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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

        #endregion
        #region User

        public static DataTable GetPushLocalUser()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();


                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalUserData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalUserData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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
        #endregion

        #region  Appointment Status

        public static DataTable GetLocalAppointmentStatusData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalAppointmentStatusData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetLocalAppointmentStatusData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetPushLocalAppointmentStatusData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalAppointmentStatusData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalAppointmentStatusData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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

        #endregion

        #region Insurance

        public static DataTable GetLocalInsuranceData(string Service_Install_Id)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalInsuranceData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetLocalInsuranceData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetPushLocalInsuranceData()
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalInsuranceData;
                    CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
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
                    try
                    {
                        string SqlCeSelect = SynchLocalQRY.GetPushLocalInsuranceData;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
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

        #endregion

        #region  Holidays

        public static DataTable GetLocalHolidayData(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalHolidayData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //SqlCeCommand.Parameters.Add("ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalEaglesoftHolidayData(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalEaglesoftHolidayData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        //SqlCeCommand.Parameters.Add("@FromDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        //SqlCeCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalDentrixHolidayData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalDentrixHolidayData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLocalDentrixOperatoryHolidayData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalDentrixOperatoryHolidayData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("@FromDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        SqlCeCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.AddMonths(6).ToString("yyyy/MM/dd");
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

        public static DataTable GetPushLocalHolidayData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalHolidayData;
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

        #region GetBlankStructureOfProviderOfficeHours

        public static DataTable GetLocalProviderOfficeHoursBlankStructure()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.ProviderOfficeHoursBlankStructure;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLocalProviderHoursBlankStructure()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.ProviderHoursBlankStructure;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLocalProviderOfficeHours(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.ProviderOfficeHours;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetPushLocalProviderOfficeHoursData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetPushProviderOfficeHours;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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


        public static DataTable GetLocalMedicalHistoryRecords(string tableName, string Service_Install_Id, bool isAllRecords = false, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = "";
                    if (isAllRecords)
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalMedicalHistoryAllRecords.Replace("MedicalHistoryTableName", tableName);
                    }
                    else
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalMedicalHistory.Replace("MedicalHistoryTableName", tableName);
                    }
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_Id = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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


        public static DataTable GetDentrixLocalMedicleFormData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetDentrixLocalMedicleFormData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetAbelDentLocalMedicleFormData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetAbelDentLocalMedicleFormData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetAbelDentLocalMedicleAnswerData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetDentrixLocalMedicleFormData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetEasyDentalLocalMedicleQuestionData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetEasyDentalLocalMedicleQuestionData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetDentrixLocalFormQuestionData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetDentrixLocalFormQuestionData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetAbelDentLocalFormQuestionData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetAbelDentLocalFormQuestionData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static DataTable GetLocalPatientWiseRecallTypeData(string Service_Install_Id)
        {
            #region SqlConnection
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientWiseRecallTypeData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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
            #endregion
        }


        public static DataTable GetLocalPatientWiseRecallTypeBlankData()
        {
            #region SqlConnection
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientWiseRecallTypeBlankData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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
            #endregion
        }

        public static DataTable GetLocalPatientWisePaymentLog(bool isblankStructure, string ServiceInstallId, string clinicNumber)
        {
            #region SqlConnection
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = "";
                    if (isblankStructure)
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientPaymebtLogStructure;
                    }
                    else
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientPaymebtLog;
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", ServiceInstallId);
                        SqlCeCommand.Parameters.Add("Clinic_Number", clinicNumber);
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
            #endregion
        }

        public static bool Save_PatientPaymentLog_To_Local(DataRow dr, string _filename_EHR_Payment = "", string EHRLogdirectory_EHR_Payment = "")
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
                        //foreach (DataRow dr in dtLivePatientPaymentLog.Rows)
                        //{
                        try
                        {
                            #region Check payment exist
                            int recordexist;
                            SqlCeCommand.CommandText = SynchLocalQRY.CheckPaymententryExist;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("PatientEHRId", dr["PatientEHRId"].ToString().Trim());
                            // SqlCeCommand.Parameters.AddWithValue("PaymentDate", Convert.ToDateTime(dr["PaymentDate"].ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("PaymentDate", Convert.ToString(dr["PaymentDate"].ToString().Trim()));
                            SqlCeCommand.Parameters.AddWithValue("Amount", dr["Amount"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PaymentNote", dr["PaymentNote"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("PaymentMode", dr["PaymentMode"].ToString());
                            recordexist = Convert.ToInt32(SqlCeCommand.ExecuteScalar());
                            if (recordexist > 0)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Check Payment entry Exist for PatientEHRId : (" + dr["PatientEHRId"] + ") and PaymentDate(" + dr["PaymentDate"].ToString() + ") of PaymentMode : (" + Convert.ToString(dr["PaymentMode"].ToString().Trim() + ")"));
                            }
                            #endregion

                            if (recordexist == 0)
                            {
                                SqlCeCommand.CommandText = SynchLocalQRY.InsertIntoPatientPaymentLog;
                                SqlCeCommand.Parameters.Clear();
                                // SqlCeCommand.Parameters.AddWithValue("Patient_LocalDB_ID", dr["Patient_LocalDB_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PatientEHRId", dr["PatientEHRId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", dr["Patient_Web_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("PatientPaymentWebId", dr["PatientPaymentWebId"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("ProviderEHRId", dr["ProviderEHRId"].ToString().Trim());
                                // SqlCeCommand.Parameters.AddWithValue("PaymentDate", Convert.ToDateTime(dr["PaymentDate"].ToString().Trim()));
                                SqlCeCommand.Parameters.AddWithValue("PaymentDate", Convert.ToString(dr["PaymentDate"].ToString().Trim()));
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
                                SqlCeCommand.Parameters.AddWithValue("TryInsert", dr["TryInsert"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PaymentMethod", dr["PaymentMethod"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("EHRSyncFinancialLogSetting", dr["EHRSyncFinancialLogSetting"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Save Patient Payment Log with PatientEHRId=" + dr["PatientEHRId"].ToString().Trim() + ", Patient Name : (" + dr["Firstname"] + "," + dr["LastName"] + ") and payment EHR Id(" + dr["PaymentEHRId"].ToString() + ") of PaymentDate : (" + Convert.ToString(dr["PaymentDate"].ToString().Trim() + ")"));
                            }


                            #region call API for

                            SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "ready", dr["Service_Install_Id"].ToString().Trim(), dr["Clinic_Number"].ToString().Trim(), "", "", "", "", "", Convert.ToInt32(dr["TryInsert"]), _filename_EHR_Payment, EHRLogdirectory_EHR_Payment);

                        }
                        catch (Exception ex)
                        {
                            SynchLocalDAL.CallPatientPaymentAPIForStatusCompleted(dr["Patient_Web_ID"].ToString().Trim(), dr["PatientPaymentWebId"].ToString().Trim(), "error", dr["Service_Install_Id"].ToString().Trim(), dr["Clinic_Number"].ToString().Trim(), ex.Message, "", "", "", ex.Message.ToString(), Convert.ToInt32(dr["TryInsert"]), _filename_EHR_Payment, EHRLogdirectory_EHR_Payment);
                        }

                        #endregion
                        // }
                    }
                    bool UpdateSync_Table_Datetime = Update_Sync_Table_Datetime("PatientPaymentLog");
                    if (UpdateSync_Table_Datetime)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "UpdateSync_Table_Datetime  status : Success");
                    }
                    else
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "UpdateSync_Table_Datetime  status : failed");
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


        public static bool CallPatientPaymentAPIForStatusCompleted(string PatientWebId, string PatientPaymentId, string status, string servericeInstallId, string clinicNumber, string errorCode, string paymentEHRId, string logEHRId, string discountId, string ehrErrorLog, int tryinsert, string _filename_EHR_Payment = "", string EHRLogdirectory_EHR_Payment = "")
        {
            try
            {
                string APIResponse = "";
                #region call API for
                try
                {

                    string jsonstringn = "";
                    APIResponse = "";
                    string strApiProviders = PullLiveDatabaseDAL.GetLiveRecord("PatientPaymentLogStatus", "");
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Call PatientPaymentStatus Confirmation API");
                    var client = new RestClient(strApiProviders);
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    var request = new RestRequest(Method.POST);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);

                    if (errorCode == "")
                    {
                        jsonstringn = "{\"patientId\":\"" + PatientWebId + "\",\"ehrSyncId\":\"" + PatientPaymentId + "\",\"status\":\"" + status + "\"}";
                    }
                    else
                    {
                        jsonstringn = "{\"patientId\":\"" + PatientWebId + "\",\"ehrSyncId\":\"" + PatientPaymentId + "\",\"status\":\"" + status + "\",\"error_code\":\"" + errorCode + "\"}";
                    }
                    request.AddParameter("application/json", jsonstringn, ParameterType.RequestBody);
                    Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Request Sent into the API " + " Authorization, TokenKey, action & json request: " + jsonstringn);
                    IRestResponse response = client.Execute(request);
                    if (response.Content != null)
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Response received from API (" + response.Content.ToString() + ")");
                    }
                    if (response.ErrorMessage != null)
                    {
                        APIResponse = "Err_API Call : " + response.ErrorMessage;
                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                        { }
                        else
                        {
                            Utility.WriteToSyncLogFile_All("[PatientPaymentLogStatus Sync LocalDb TO EHR_Err " + response.ErrorMessage + "  Service Install Id : " + servericeInstallId.Trim() + " And Clinic : " + clinicNumber.Trim() + "  " + response.ErrorMessage);
                        }
                    }
                    else
                    {
                        Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Payment Confirmed Acknowledgment sent back to Adit app for  Payment EHR Id(" + paymentEHRId.ToString() + ") of PaymentStatusCompletedDateTime : (" + DateTime.Now.ToString() + ")");
                    }

                }
                catch (Exception)
                {

                }

                #region update Local Table
                if (status == "completed")
                {
                    #endregion
                    if ((paymentEHRId != "" && paymentEHRId != "0") || (discountId != "" && discountId != "0") || (logEHRId != "" && logEHRId != "0"))
                    {
                        using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            DateTime dtCurrentDtTime = Utility.Datetimesetting();
                            string SqlCeSelect = string.Empty;
                            try
                            {
                                SqlCeSelect = SynchLocalQRY.UpdatePatientPaymentLogTable;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.Parameters.Add("EHRLogId", logEHRId.ToString());
                                    SqlCeCommand.Parameters.Add("EHRDiscountId", discountId.ToString());
                                    SqlCeCommand.Parameters.Add("ErrorMessage", APIResponse);
                                    SqlCeCommand.Parameters.Add("PaymentUpdatedEHR", 1);
                                    SqlCeCommand.Parameters.Add("PaymentUpdatedEHRDateTime", dtCurrentDtTime.ToString());
                                    SqlCeCommand.Parameters.Add("PatientPaymentWebId", PatientPaymentId.ToString());
                                    SqlCeCommand.Parameters.Add("PaymentStatusCompletedAdit", 1);
                                    SqlCeCommand.Parameters.Add("PaymentStatusCompletedDateTime", dtCurrentDtTime.ToString());
                                    SqlCeCommand.Parameters.Add("PaymentEHRId", paymentEHRId.ToString());
                                    SqlCeCommand.Parameters.Add("Service_Install_Id", servericeInstallId);
                                    SqlCeCommand.Parameters.Add("EHRErroLog", ehrErrorLog.ToString());//
                                    SqlCeCommand.Parameters.Add("TryInsert", tryinsert + 1);
                                    SqlCeCommand.ExecuteNonQuery();
                                    Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, "Update Patient Payment Log into local for EHRLogId=" + logEHRId.ToString() + ",Payment EHR Id(" + paymentEHRId.ToString() + ") of PaymentStatusCompletedDateTime : (" + dtCurrentDtTime.ToString() + ")");
                                }
                            }
                            catch (Exception ex)
                            {
                                // SqlCetx.Rollback();
                                throw;
                            }
                        }
                    }
                }
                if (ehrErrorLog != "")
                {
                    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;
                        try
                        {
                            SqlCeSelect = SynchLocalQRY.UpdatePatientPaymentLogError;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.Add("EHRErroLog", ehrErrorLog.ToString());
                                SqlCeCommand.Parameters.Add("PatientPaymentWebId", PatientPaymentId.ToString());
                                SqlCeCommand.Parameters.Add("Service_Install_Id", servericeInstallId);
                                SqlCeCommand.Parameters.Add("TryInsert", tryinsert + 1);
                                SqlCeCommand.ExecuteNonQuery();
                                Utility.WriteSyncPullLog(_filename_EHR_Payment, EHRLogdirectory_EHR_Payment, " Update Patient Payment LogError into local for PatientPaymentWebId=" + PatientPaymentId.ToString() + "), EHRErroLog=" + ehrErrorLog.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            // SqlCetx.Rollback();
                            throw;
                        }
                    }
                }
                #endregion

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public static bool UpdatePatientPaymentEHRId_In_Local(string PaymentEHRID, string PatientPaymentWebId, string Service_Install_Id, string _filename_EHR_Payment = "", string _EHRLogdirectory_EHR_Payment = "")
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
                            SqlCeCommand.CommandText = SynchLocalQRY.UpdatePatientPaymentEHRId_In_Local;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("PaymentEHRId", PaymentEHRID.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("PatientPaymentWebId", PatientPaymentWebId.ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_Payment, _EHRLogdirectory_EHR_Payment, " Update Patient Payment EHRId In Local for  PaymentEHRId=" + PaymentEHRID.ToString().Trim() + " and PatientPaymentWebId=" + PatientPaymentWebId.ToString().Trim());
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

        public static void UpdatePatient_Status(DataTable dtPatientStatus, string Service_Install_Id, string clinicNumber = "", string strPatID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlCeSelect = string.Empty;

                try
                {
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeSelect = SynchLocalQRY.UpdatePatientStatusAllBlank;
                        if (!string.IsNullOrEmpty(strPatID))
                        {
                            SqlCeSelect = SqlCeSelect + " AND Clinic_Number = @Clinic_Number And Patient_EHR_ID = @Patient_EHR_ID ";
                        }
                        SqlCeCommand.CommandText = SqlCeSelect;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        if (!string.IsNullOrEmpty(strPatID))
                        {
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", strPatID);
                        }
                        SqlCeCommand.ExecuteNonQuery();
                    }

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeSelect = SynchLocalQRY.UpdatePatientStatusAll;
                        if (!string.IsNullOrEmpty(strPatID))
                        {
                            SqlCeSelect = SqlCeSelect + " AND Clinic_Number = @Clinic_Number And Patient_EHR_ID = @Patient_EHR_ID ";
                        }
                        SqlCeCommand.CommandText = SqlCeSelect;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        if (!string.IsNullOrEmpty(strPatID))
                        {
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", strPatID);
                        }
                        SqlCeCommand.ExecuteNonQuery();
                    }

                    using (SqlCeCommand SqlCeComBulk = new SqlCeCommand("", conn))
                    {
                        SqlCeComBulk.CommandType = CommandType.TableDirect;
                        SqlCeComBulk.CommandText = "Patient";
                        SqlCeComBulk.Connection = conn;
                        SqlCeComBulk.IndexName = "unique_Patient_EHR_ID";

                        SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                        foreach (DataRow drRow in dtPatientStatus.Rows)
                        {
                            bool found = false;
                            try
                            {
                                found = rs.Seek(DbSeekOptions.FirstEqual, new { PatID = drRow["Patient_EHR_Id"].ToString(), CliNum = "0", ServiceInstalledID = Service_Install_Id });
                            }
                            catch (Exception exFound)
                            {
                                if (exFound.Message.ToUpper().Contains("OBJECT MUST IMPLEMENT ICONVERTIBLE"))
                                {
                                    found = rs.Seek(DbSeekOptions.FirstEqual, new object[] { drRow["Patient_EHR_Id"].ToString(), "0", Service_Install_Id });
                                }
                                else
                                {
                                    throw exFound;
                                }
                            }

                            if (found)
                            {
                                rs.Read();
                                string PatStatusComapre = Convert.ToString(rs.GetValue(rs.GetOrdinal("Patient_status_Compare")));
                                rs.SetValue(rs.GetOrdinal("Is_Status_Adit_Updated"), (PatStatusComapre.Trim().ToUpper() == "NEW" ? true : false));
                                rs.SetValue(rs.GetOrdinal("Patient_Status"), "New");
                                try
                                {
                                    rs.Update();
                                }
                                catch (Exception exduplicateupdate)
                                {
                                    if (exduplicateupdate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                    {
                                        Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient: " + drRow["Patient_EHR_Id"].ToString() + ", Clinic_Number:" + "0" + ", Service_Installed_ID:" + Service_Install_Id);
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
                    //-------------------------------------------------------------------------------------------------
                    //foreach (DataRow drRow in dtPatientStatus.Rows)
                    //{
                    //    SqlCeSelect = string.Empty;
                    //    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    //    {
                    //        //UPDATE Patient SET  Patient_Status = @Patient_Status,
                    //        //Is_Status_Adit_Updated = (case when Patient_status_Compare = 'New' then  1  else 0 end)
                    //        //WHERE Patient_EHR_ID = @Patient_EHR_ID And Service_Install_Id = @Service_Install_Id
                    //        SqlCeCommand.CommandType = CommandType.Text;
                    //        SqlCeCommand.CommandText = SynchLocalQRY.UpdatePatientStatus;
                    //        SqlCeCommand.Parameters.Clear();
                    //        SqlCeCommand.Parameters.AddWithValue("Patient_Status", "New");
                    //        SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", drRow["Patient_EHR_Id"].ToString());
                    //        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    //        SqlCeCommand.ExecuteNonQuery();
                    //    }
                    //}
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }




        public static void UpdatePatient_Status(DataTable dtPatientStatus, string Service_Install_Id, int clinicNumber)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlCeSelect = string.Empty;

                try
                {
                    //Utility.WriteToSyncLogFile_All("SynchDataOpenDental_PatientStatus: UpdatePatient_Status 1");
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.UpdatePatientStatusAllBlank_ClinicWise;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                        SqlCeCommand.ExecuteNonQuery();
                    }

                    //Utility.WriteToSyncLogFile_All("SynchDataOpenDental_PatientStatus: UpdatePatient_Status 2");
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.UpdatePatientStatusAll_ClinicWise;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                        SqlCeCommand.ExecuteNonQuery();
                    }

                    //Utility.WriteToSyncLogFile_All("SynchDataOpenDental_PatientStatus: UpdatePatient_Status 3 Loop Start");
                    foreach (DataRow drRow in dtPatientStatus.Rows)
                    {
                        SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.UpdatePatientStatus_ClinicWise;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Patient_Status", "New");
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", drRow["Patient_EHR_Id"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", clinicNumber);
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    //Utility.WriteToSyncLogFile_All("SynchDataOpenDental_PatientStatus: UpdatePatient_Status 4 Loop End");
                }
                catch (Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("Err in UpdatePatient_Stats: " + ex.Message);
                    throw ex;
                }
            }
        }

        public static bool Update_PatientBalance(DataTable dtPatientBalance, string Service_Install_Id)
        {
            bool is_Update_PatientBalance_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlCeSelect = string.Empty;

                try
                {
                    foreach (DataRow drRow in dtPatientBalance.Rows)
                    {
                        SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.UpdatePatientBalance;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("CurrentBal", drRow["CurrentBal"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("ThirtyDay", drRow["ThirtyDay"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("SixtyDay", drRow["SixtyDay"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("NinetyDay", drRow["NinetyDay"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Over90", drRow["Over90"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", drRow["Patient_EHR_Id"].ToString());
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.ExecuteNonQuery();
                            is_Update_PatientBalance_Flg = true;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return is_Update_PatientBalance_Flg;
        }

        public static bool CallPatientSMSCallAPIForStatusCompleted(DataTable dtPatientSMSLog, string status, string servericeInstallId, string clinicNumber, string Loc_Id, string LocationId, string _filename_EHR_patient_sms_call = "", string _EHRLogdirectory_EHR_patient_sms_call = "")
        {
            try
            {
                string APIResponse = "";
                if (dtPatientSMSLog.Rows.Count > 0)
                {
                    try
                    {

                        List<PatientSMSCallLogStatusUpdate> StatusData = new List<PatientSMSCallLogStatusUpdate>();
                        foreach (DataRow dr in dtPatientSMSLog.Rows)
                        {
                            PatientSMSCallLogStatusUpdate p = new PatientSMSCallLogStatusUpdate();
                            p.esId = dr["esId"].ToString();
                            p.smsId = dr["smsId"].ToString();
                            p.ehrsyncstatus = dr["Log_Status"].ToString();
                            StatusData.Add(p);
                        }

                        Push_PatientSMSCallLog Ps = new Push_PatientSMSCallLog();
                        Ps.locationId = Loc_Id;
                        Ps.statusupdate = StatusData;
                        #region call API for

                        string strApiProviders = PullLiveDatabaseDAL.GetLiveRecord("patientsmscalllogstatus", "");
                        Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, "Call ApiProviders (patientsmscalllogstatus) API");
                        var client = new RestClient(strApiProviders);
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        var request = new RestRequest(Method.POST);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("Authorization", Utility.WebAdminUserToken);
                        request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(LocationId));
                        var JsonPatient = new System.Text.StringBuilder();
                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        javaScriptSerializer.MaxJsonLength = 500000000;
                        JsonPatient.Append(javaScriptSerializer.Serialize(Ps));
                        request.AddParameter("application/json", JsonPatient, ParameterType.RequestBody);
                        Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, "Request Sent into the API " + " Authorization, TokenKey & action"); ;
                        IRestResponse response = client.Execute(request);

                        //Utility.WriteToSyncLogFile_All("Call API " + strApiProviders.ToString());

                        //Utility.WriteToSyncLogFile_All("Call API Response " + response.Content.ToString());

                        //Utility.WriteToSyncLogFile_All("Call API Response Error " + response.ErrorMessage.ToString());
                        if (response.Content != null)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, "Response received from API(" + response.Content.ToString() + ")");
                        }
                        if (response.ErrorMessage != null)
                        {
                            APIResponse = "Err_API Call : " + response.ErrorMessage;
                            if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                            { }
                            else
                            {
                                Utility.WriteToSyncLogFile_All("[PatientPaymentLogStatus Sync (Adit Server To Local Database)] Service Install Id : " + servericeInstallId.Trim() + " And Clinic : " + clinicNumber.Trim() + "  " + response.ErrorMessage);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        APIResponse = ex.Message;
                    }

                    #region update Local Table
                    if (status.ToLower() == "completed")
                    {
                        using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                        {

                            foreach (DataRow dr in dtPatientSMSLog.Rows)
                            {
                                if (dr["LogEHRId"].ToString() != "")
                                {

                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                    string SqlCeSelect = string.Empty;
                                    try
                                    {
                                        SqlCeSelect = SynchLocalQRY.UpdatePatientSMSCallLogTable;
                                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                        {
                                            SqlCeCommand.CommandType = CommandType.Text;
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.Parameters.Add("LogUpdatedEHR", 1);
                                            SqlCeCommand.Parameters.Add("LogUpdatedEHRDateTime", dtCurrentDtTime.ToString());
                                            SqlCeCommand.Parameters.Add("PatientSMSCallLogWebId", dr["PatientSMSCallLogWebId"].ToString().ToString());
                                            SqlCeCommand.Parameters.Add("LogStatusCompletedAdit", 1);
                                            SqlCeCommand.Parameters.Add("LogStatusCompletedDateTime", dtCurrentDtTime.ToString());
                                            SqlCeCommand.Parameters.Add("LogEHRId", dr["LogEHRId"].ToString());
                                            SqlCeCommand.Parameters.Add("Service_Install_Id", servericeInstallId);
                                            SqlCeCommand.Parameters.Add("ErrorMessage", APIResponse);
                                            SqlCeCommand.Parameters.Add("PatientEHRId", dr["PatientEHRId"].ToString().Trim());
                                            SqlCeCommand.ExecuteNonQuery();
                                            Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, " Update Patient SMS Call Log for PatientEHRId" + dr["PatientEHRId"].ToString().Trim() + " and LogEHRId=" + dr["LogEHRId"].ToString() + " and PatientSMSCallLogWebId" + dr["PatientSMSCallLogWebId"].ToString());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        // SqlCetx.Rollback();
                                        throw;
                                    }
                                    finally
                                    {
                                        if (conn.State == ConnectionState.Open) conn.Close();
                                    }
                                }

                            }
                        }
                    }
                    #endregion
                }
                #endregion
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public static bool CallPatientFollowUpStatusCompleted(DataTable dtPatientSMSLog, string status, string servericeInstallId, string clinicNumber, string Loc_Id, string LocationId, string _filename_EHR_PatientFollowUp = "", string _EHRLogdirectory_EHR_PatientFollowUp = "")
        {
            try
            {
                string APIResponse = "";
                if (dtPatientSMSLog.Rows.Count > 0)
                {
                    try
                    {

                        List<Push_PatientFollowUp_NoteId> StatusData = new List<Push_PatientFollowUp_NoteId>();
                        foreach (DataRow dr in dtPatientSMSLog.Rows)
                        {
                            Push_PatientFollowUp_NoteId p = new Push_PatientFollowUp_NoteId();
                            p.id = dr["esId"].ToString();

                            p.status = status.ToString();

                            p.error = dr["Log_Status"].ToString().ToLower() != "readytoimport" ? dr["Log_Status"].ToString() : "";
                            StatusData.Add(p);
                        }

                        Push_PatientFollowUp Ps = new Push_PatientFollowUp();
                        Ps.locationId = Loc_Id;
                        Ps.note_ids = StatusData;
                        Ps.organizationId = Utility.Organization_ID;
                        #region call API for

                        string strApiProviders = PullLiveDatabaseDAL.GetLiveRecord("patientfollowuplogstatus", "");
                        // Utility.WriteToSyncLogFile_All("API Called " + strApiProviders.ToString());
                        Utility.WriteSyncPullLog(_filename_EHR_PatientFollowUp, _EHRLogdirectory_EHR_PatientFollowUp, "Call patientfollowuplogstatus API ");
                        var client = new RestClient(strApiProviders);
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        var request = new RestRequest(Method.POST);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("Authorization", Utility.WebAdminUserToken);
                        request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(LocationId));
                        var JsonPatient = new System.Text.StringBuilder();
                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        javaScriptSerializer.MaxJsonLength = 500000000;
                        JsonPatient.Append(javaScriptSerializer.Serialize(Ps));
                        request.AddParameter("application/json", JsonPatient, ParameterType.RequestBody);
                        Utility.WriteSyncPullLog(_filename_EHR_PatientFollowUp, _EHRLogdirectory_EHR_PatientFollowUp, "Request Sent into the API  " + " Authorization, TokenKey & action");
                        IRestResponse response = client.Execute(request);
                        // Utility.WriteToSyncLogFile_All("API Called response" + response.Content.ToString());
                        if (response.ErrorMessage != null)
                        {
                            APIResponse = "Err_API Call : " + response.ErrorMessage;
                            if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                            { }
                            else
                            {
                                Utility.WriteToSyncLogFile_All("[PatientFollowUpStatus Sync (Local To Adit Database)] Service Install Id : " + servericeInstallId.Trim() + " And Clinic : " + clinicNumber.Trim() + "  " + response.ErrorMessage);
                            }
                        }
                        if (response.Content != null)
                        {
                            Utility.WriteSyncPullLog(_filename_EHR_PatientFollowUp, _EHRLogdirectory_EHR_PatientFollowUp, "Response received from API(" + response.Content.ToString() + ")");
                        }

                    }
                    catch (Exception ex)
                    {
                        //  Utility.WriteToSyncLogFile_All("API Called response" + APIResponse.ToString());
                        APIResponse = ex.Message;
                    }

                    #region update Local Table
                    if (status.ToLower() == "completed")
                    {
                        using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                        {

                            foreach (DataRow dr in dtPatientSMSLog.Rows)
                            {
                                //Utility.WriteToSyncLogFile_All("Update Records to LOcal" + APIResponse.ToString());
                                if (dr["LogEHRId"].ToString() != "")
                                {
                                    //Utility.WriteToSyncLogFile_All("Update Records to LOcal_1" + APIResponse.ToString());
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                    string SqlCeSelect = string.Empty;
                                    try
                                    {
                                        SqlCeSelect = SynchLocalQRY.UpdatePatientSMSCallLogTable;
                                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                        {
                                            SqlCeCommand.CommandType = CommandType.Text;
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.Parameters.Add("LogUpdatedEHR", true);
                                            SqlCeCommand.Parameters.Add("LogUpdatedEHRDateTime", dtCurrentDtTime.ToString());
                                            SqlCeCommand.Parameters.Add("PatientSMSCallLogWebId", dr["PatientSMSCallLogWebId"].ToString().ToString());
                                            SqlCeCommand.Parameters.Add("LogStatusCompletedAdit", true);
                                            SqlCeCommand.Parameters.Add("LogStatusCompletedDateTime", dtCurrentDtTime.ToString());
                                            SqlCeCommand.Parameters.Add("LogEHRId", dr["LogEHRId"].ToString().ToString());
                                            SqlCeCommand.Parameters.Add("Service_Install_Id", servericeInstallId);
                                            SqlCeCommand.Parameters.Add("ErrorMessage", APIResponse);
                                            SqlCeCommand.Parameters.Add("PatientEHRId", dr["PatientEHRId"].ToString().Trim());
                                            // Utility.WriteToSyncLogFile_All("Call UPdate Query" + SqlCeSelect.ToString());
                                            SqlCeCommand.ExecuteNonQuery();
                                            Utility.WriteSyncPullLog(_filename_EHR_PatientFollowUp, _EHRLogdirectory_EHR_PatientFollowUp, "Update Patient SMS CallLog for LogEHRId " + dr["LogEHRId"].ToString() + ", " + "PatientEHRId : (" + dr["PatientEHRId"].ToString().Trim() + ") of Log Status Completed DateTime: (" + dtCurrentDtTime.ToString() + ")");
                                            //Utility.WriteToSyncLogFile_All("Call UPdate Query Executed");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        // SqlCetx.Rollback();
                                        throw;
                                    }
                                    finally
                                    {
                                        if (conn.State == ConnectionState.Open) conn.Close();
                                    }
                                }

                            }
                        }
                    }
                    #endregion
                }
                #endregion
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public static DataTable GetLocalPatientWiseSMSCallLog(bool isblankStructure, string ServiceInstallId, string clinicNumber, int logType, string _filename_EHR_patient_sms_call = "", string _EHRLogdirectory_EHR_patient_sms_call = "")
        {
            #region SqlConnection
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = "";
                    if (isblankStructure)
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientWiseSMSCallLogStructure;
                    }
                    else
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientWiseSMSCallLog;
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", ServiceInstallId);
                        SqlCeCommand.Parameters.Add("Clinic_Number", clinicNumber);
                        SqlCeCommand.Parameters.Add("LogType", logType.ToString());
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            if (isblankStructure)
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, " Get Local PatientWise SMS CallLog Structure for LogType=" + logType.ToString());
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(_filename_EHR_patient_sms_call, _EHRLogdirectory_EHR_patient_sms_call, " Get Local PatientWise SMS CallLog for LogType=" + logType.ToString());
                            }
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
            #endregion
        }

        public static bool Update_PatientProfileImageStatus(string PatientWebId, string Service_Install_Id)
        {
            #region SqlCeConnection

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = "";
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientProfileImageStatus;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", PatientWebId);
                        //sqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            #endregion
        }

        public static DataTable GetLocalPatientProfileImageRecords(string Service_Install_Id, bool isAllRecords = false)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime ToDate = Utility.LastSyncDateAditServer;
                try
                {
                    string SqlCeSelect = "";
                    if (isAllRecords)
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientProfileImageAllRecords;
                    }
                    else
                    {
                        SqlCeSelect = SynchLocalQRY.GetLocalPatientProfileImageRecords;
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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


        public static bool Save_PatientProfileImage_EHR_To_Local(DataTable dtPatientProfileImage, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        string AppointmentStatus = string.Empty;
                        string destPatientProfileImage = CommonUtility.GetAditPatientProfileImagePath();
                        DataTable dtLocalPatient = SynchLocalDAL.GetAllLocalPatientData();
                        foreach (DataRow dr in dtPatientProfileImage.Rows)
                        {
                            if (dr["InsUptDlt"].ToString() == "")
                            {
                                dr["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1 || Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2)
                                {
                                    string destPatientProfileImageFile = "";
                                    string destPatientProfileImageFilePath = "";
                                    var PatientWebId = dtLocalPatient.AsEnumerable().Where(o => o.Field<object>("Patient_EHR_id").ToString() == dr["Patient_EHR_ID"].ToString().ToString()).Select(o => o.Field<object>("Patient_Web_ID")).FirstOrDefault();
                                    if (PatientWebId != null)
                                    {
                                        dr["Patient_Web_ID"] = PatientWebId;
                                        DataRow[] row = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Service_Install_Id.ToString() + "' And  Clinic_Number = '0' ");
                                        if (row != null && row.Length > 0)
                                        {
                                            destPatientProfileImageFile = Convert.ToString(row[0]["Organization_ID"].ToString() + "@" + row[0]["Loc_ID"].ToString() + "@" + PatientWebId.ToString().Trim() + ".jpeg"); //.Replace("-", "_")
                                            dr["Image_EHR_Name"] = destPatientProfileImageFile;
                                        }
                                        destPatientProfileImageFilePath = destPatientProfileImage + "\\" + destPatientProfileImageFile;
                                        CommonUtility.SaveImageEHRToLocal(dr, dr["SourceLocation"].ToString(), destPatientProfileImageFilePath);
                                    }
                                }
                                //if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                //{
                                //    string destPatientProfileImageFilePath = destPatientProfileImage + "\\" + dr["Image_EHR_Name"].ToString();
                                //    File.Delete(destPatientProfileImageFilePath);
                                //}
                                switch (Convert.ToInt32(dr["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_PatientProfileImageData;
                                        break;
                                    case 2:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientProfileImageData;
                                        break;
                                    case 3:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Delete_PatientProfileImageData;
                                        break;
                                }
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("Patient_Images_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Patient_Images_EHR_ID", dr["Patient_Images_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", dr["Patient_EHR_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", dr["Patient_Web_ID"].ToString().Trim());
                                SqlCeCommand.Parameters.AddWithValue("Image_EHR_Name", dr["Image_EHR_Name"].ToString().Trim());// 
                                SqlCeCommand.Parameters.AddWithValue("Patient_Images_FilePath", dr["Patient_Images_FilePath"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", string.IsNullOrEmpty(dr["Entry_DateTime"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(dr["Entry_DateTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("AditApp_Entry_DateTime", Utility.GetCurrentDatetimestring());
                                SqlCeCommand.Parameters.AddWithValue("Is_Deleted", Convert.ToBoolean(dr["Is_deleted"]));
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("clinic_Number", dr["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
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

        public static DataTable GetLocalPatientPortal_completed_PendingData(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientPortal_completed_PendingData;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static string Call_API_For_PatientPortalDate_Completed(string Service_Install_Id, string LocationId, string strPatientFormID = "")
        {
            string _successfullstataus = string.Empty;

            try
            {
                DataTable dtPF_completed_PendingData = GetLocalPatientPortal_completed_PendingData(Service_Install_Id, strPatientFormID);
                if (dtPF_completed_PendingData.Rows.Count > 0)
                {

                    var JsonPatient_FormBO = new System.Text.StringBuilder();


                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    Push_PatientPortal_StatusUpdate Patient_FormBO = new Push_PatientPortal_StatusUpdate();

                    Patient_FormBO.statusupdate = new List<Statusupdate>();
                    foreach (DataRow drPF_CPD in dtPF_completed_PendingData.Rows)
                    {
                        Statusupdate updatest = new Statusupdate();
                        updatest.esId = drPF_CPD["patientform_web_id"].ToString().Trim();
                        updatest.patientId = drPF_CPD["patient_web_Id"].ToString().Trim();
                        updatest.ehrsyncstatus = "completed";
                        Patient_FormBO.statusupdate.Add(updatest);
                    }
                    JsonPatient_FormBO.Append(javaScriptSerializer.Serialize(Patient_FormBO) + ",");
                    //}
                    string jsonString = "" + JsonPatient_FormBO.ToString().Remove(JsonPatient_FormBO.Length - 1) + "";
                    // string jsonString = JsonPatient_FormBO.ToString().Remove(JsonPatient_FormBO.Length - 1);
                    string RestURL = PullLiveDatabaseDAL.GetLiveRecord("PatientPortalUpdatedinEHR", "");
                    var request = new RestRequest(Method.POST);
                    var client = new RestClient(RestURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(LocationId));
                    request.AddHeader("action", "EHRPFIMPORT");
                    request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);


                    if (response.Content.ToString().Contains("Import list update Successfully"))
                    {
                        bool is_UpdateFlg = SynchLocalDAL.UpdatePatientFormSyncValue(Service_Install_Id);
                        _successfullstataus = "success";
                    }
                    else
                    {
                        if (response.Content.ToString() == "")
                        {
                            _successfullstataus = response.ErrorMessage.ToString();
                        }
                        else
                        {
                            if (UpdatePatientPortalEHR_Updateflg(dtPF_completed_PendingData))
                            {
                                _successfullstataus = "success";
                            }
                            else
                            {
                                _successfullstataus = "Problem to update status in Patient_Form after call Post to completed status";
                            }

                        }
                    }

                }
                else
                {
                    _successfullstataus = "success";
                }
            }
            catch (Exception)
            {
                _successfullstataus = "API Call Fail";
            }

            return _successfullstataus;
        }


        public static DataTable GetLocalPatientFormMedicationResponse(string Clinic_Number, string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalPatientFormMedicationResponse_Clinic : SynchLocalQRY.GetLocalPatientFormMedicationResponse;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientFormMedicationRemovedResponse(string Clinic_Number, string Service_Install_Id)
        {

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalPatientFormMedicationRemovedResponse_Clinic : SynchLocalQRY.GetLocalPatientFormMedicationRemovedResponse;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientFormMedicationRemovedResponseToSaveINEHR(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientFormMedicationRemovedResponseToSaveINEHR;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And MR.PatientForm_Web_ID = '" + strPatientFormID + "' ";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static DataTable GetLocalPatientFormMedicationResponseToSaveINEHR(string Service_Install_Id, string strPatientFormID = "")
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetLocalPatientFormMedicationResponseToSaveINEHR;
                    if (!string.IsNullOrEmpty(strPatientFormID))
                    {
                        SqlCeSelect = SqlCeSelect + " And MR.PatientForm_Web_ID = '" + strPatientFormID + "'";
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
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

        public static bool UpdateMedicationEHR_Updateflg(string pMedicationResponseLocalid, string pPatientMedicationId, string pPatientEhrId, string pMedicationEHRId, string Service_Install_Id)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                string SqlCeSelect = string.Empty;
                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Medication_EHR_Update_Flg;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("MedicationResponse_Local_Id", pMedicationResponseLocalid);
                        SqlCeCommand.Parameters.AddWithValue("PatientMedication_EHR_Id", pPatientMedicationId);
                        SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", pPatientEhrId);
                        SqlCeCommand.Parameters.AddWithValue("Medication_EHR_ID", pMedicationEHRId);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        //SqlCetx.Commit();
                        // Utility.WriteToErrorLogFromAll(SqlCeCommand.CommandText + "," + dtEHRDiseaseLocalid + "," + allergypatientId + "," + patient_EHR_Id + "," + Service_Install_Id);
                        is_Update_PatientForm_Flg = true;
                    }
                }
                catch (Exception)
                {
                    // SqlCetx.Rollback();
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;
        }

        public static bool UpdateRemovedMedicationEHR_Updateflg(string pMedicationResponseLocalid, string pPatientMedicationEHRId, string pPatientEhrId, string Service_Install_Id)
        {
            bool is_Update_PatientForm_Flg = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlCetx = conn.BeginTransaction();
                string SqlCeSelect = string.Empty;
                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Removed_Medication_EHR_Update_Flg;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("MedicationRemovedResponse_Local_Id", pMedicationResponseLocalid);
                        SqlCeCommand.Parameters.AddWithValue("PatientMedication_EHR_Id", pPatientMedicationEHRId);
                        SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", pPatientEhrId);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                        //SqlCetx.Commit();
                        // Utility.WriteToErrorLogFromAll(SqlCeCommand.CommandText + "," + dtEHRDiseaseLocalid + "," + allergypatientId + "," + patient_EHR_Id + "," + Service_Install_Id);
                        is_Update_PatientForm_Flg = true;
                    }
                }
                catch (Exception)
                {
                    // SqlCetx.Rollback();
                    throw;
                }
            }
            return is_Update_PatientForm_Flg;
        }

        public static DataTable GetAllLocalNoteId(string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = SynchLocalQRY.GetAllLocalNoteId;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
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

        public static bool CallUpdateNoteDataMoveToCorrespondInTracker(DataTable dtNote, string Installation_ID)
        {
            try
            {
                if (dtNote.Rows.Count > 0)
                {
                    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                    {

                        for (int i = 0; i < dtNote.Rows.Count; i++)
                        {
                            //Utility.WriteToSyncLogFile_All("Update Records to LOcal" + APIResponse.ToString());
                            if (dtNote.Rows[i]["LogEHRId"].ToString() != "" && dtNote.Rows[i]["NewLogEHRId"].ToString() != "")
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                try
                                {
                                    if (dtNote.Rows[i]["tblName"].ToString() == "PatientSMSCallLog")
                                    {
                                        SqlCeSelect = SynchLocalQRY.GetUpdateNewNoteIdPatientSMSCallLog;
                                    }
                                    else
                                    {
                                        SqlCeSelect = SynchLocalQRY.GetUpdateNewNoteIdPatientPayment;
                                    }
                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                    {
                                        SqlCeCommand.CommandType = CommandType.Text;
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.Add("NewLogEHRId", SqlDbType.NVarChar).Value = dtNote.Rows[i]["NewLogEHRId"].ToString();
                                        SqlCeCommand.Parameters.Add("LogEHRId", SqlDbType.NVarChar).Value = dtNote.Rows[i]["LogEHRId"].ToString();
                                        SqlCeCommand.Parameters.Add("Service_Install_Id", SqlDbType.NVarChar).Value = Installation_ID;
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception e)
                                {
                                    // SqlCetx.Rollback();
                                    throw e;
                                }
                                finally
                                {
                                    if (conn.State == ConnectionState.Open) conn.Close();
                                }
                            }

                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                //return false;
                throw ex;
            }
        }

        public static void Save_ApptPatientBalance(DataTable dtPatientBalance, string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlCeSelect = string.Empty;

                try
                {
                    foreach (DataRow drRow in dtPatientBalance.Rows)
                    {
                        SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            if (drRow["InsUptDlt"].ToString() == "")
                            {
                                drRow["InsUptDlt"] = "0";
                            }
                            if (Convert.ToInt32(drRow["InsUptDlt"].ToString()) != 0)
                            {
                                switch (Convert.ToInt32(drRow["InsUptDlt"].ToString()))
                                {
                                    case 1:
                                        SqlCeCommand.CommandText = SynchLocalQRY.Insert_ApptPatientBalance;
                                        break;
                                }

                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("Appointment_EHR_ID", drRow["Appointment_EHR_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ApptPatient_Web_ID", "");
                                SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", drRow["Patient_EHR_Id"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("CurrentBal", drRow["CurrentBal"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ThirtyDay", drRow["ThirtyDay"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SixtyDay", drRow["SixtyDay"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("NinetyDay", drRow["NinetyDay"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("Over90", drRow["Over90"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("remaining_benefit", drRow["remaining_benefit"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("used_benefit", drRow["used_benefit"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("collect_payment", drRow["collect_payment"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", drRow["Clinic_Number"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
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

        public static DataTable GetLocalApptPatientBalance(string Service_Install_Id, int clinicNumber)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    string SqlCeSelect = "";
                    SqlCeSelect = SynchLocalQRY.GetLocalApptPatientBalanceData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.Add("Clinic_Number", clinicNumber);
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

        public static DataTable GetPushLocalPatientBalanceData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalApptPatientBalanceData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static string Get_Patient_EHR_ID_from_Patient_Form(string strPatientFormWebID)
        {
            string PatientEHRID = "";
            try
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = "Select distinct Patient_EHR_ID from Patient_Form where PatientForm_Web_ID = @PatientForm_Web_ID";
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SqlCeSelect;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("PatientForm_Web_ID", strPatientFormWebID.ToString().Trim());
                        object result = SqlCeCommand.ExecuteScalar();
                        if (result != null)
                        {
                            PatientEHRID = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PatientEHRID;
        }
        public static bool Save_Patient_Live_To_LocalPatientDB(DataTable dtOpenDentalDataToSave, string Clinic_Number, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            string clinicNumber = "0";
            try
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.PatientDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCeCommand SqlCeComBulk = new SqlCeCommand("", conn))
                    {
                        SqlCeComBulk.CommandType = CommandType.TableDirect;
                        SqlCeComBulk.CommandText = "Patient";
                        SqlCeComBulk.Connection = conn;
                        SqlCeComBulk.IndexName = "unique_Patient_EHR_ID";
                        SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                        foreach (DataRow dr in dtOpenDentalDataToSave.Select("InsUptDlt=1"))
                        {
                            if (dtOpenDentalDataToSave.Columns.Contains("Clinic_Number"))
                            {
                                clinicNumber = dr["Clinic_Number"].ToString();
                            }
                            else
                            {
                                clinicNumber = "0";
                            }

                            try
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                                {
                                    bool found = false;
                                    try
                                    {
                                        found = rs.Seek(DbSeekOptions.FirstEqual, new { PatID = dr["Patient_EHR_ID"].ToString().Trim(), CliNum = clinicNumber.Trim(), ServiceInstalledID = Service_Install_Id });
                                    }
                                    catch (Exception exFound)
                                    {
                                        if (exFound.Message.ToUpper().Contains("OBJECT MUST IMPLEMENT ICONVERTIBLE"))
                                        {
                                            found = rs.Seek(DbSeekOptions.FirstEqual, new object[] { dr["Patient_EHR_ID"].ToString().Trim(), clinicNumber.ToString().Trim(), Service_Install_Id });
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
                                            rec.SetValue(rs.GetOrdinal("Entry_DateTime"), DateTime.Now);
                                            rec.SetValue(rs.GetOrdinal("Clinic_Number"), clinicNumber);
                                            rec.SetValue(rs.GetOrdinal("Service_Install_Id"), Service_Install_Id);
                                            rec.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 0);
                                            try
                                            {
                                                rs.Insert(rec);
                                            }
                                            catch (Exception exduplicate)
                                            {
                                                if (exduplicate.Message.ToString().ToUpper().Contains("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX."))
                                                {
                                                    Utility.WriteToErrorLogFromAll("A DUPLICATE VALUE CANNOT BE INSERTED INTO A UNIQUE INDEX: Patient [Patient DB]: " + dr["Patient_EHR_ID"].ToString().Trim() + ", Clinic_Number:" + Clinic_Number + ", Service_Installed_ID:" + Service_Install_Id);
                                                    continue;
                                                }
                                                else
                                                {
                                                    throw exduplicate;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                Utility.WriteToErrorLogFromAll("Err_Patient Insert For Patient [Patient DB] : " + dr["patient_ehr_id"].ToString() + "_" + ex1.Message.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                //SqlCetx.Rollback();
                // throw ex;
            }

            return _successfullstataus;
        }

        public static DataTable GetLocalProviderDataForHours(string Clinic_Number, string ServiceInstallId)
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);
                string SqlCeSelect = string.Empty;
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    if (Utility.is_scheduledCustomhourProvider)
                    {
                        SqlCeSelect = "SELECT * FROM Providers Where Provider_EHR_ID in (@Providerids)";
                        string joinedstring = string.Join(",", Utility.CustomhourProviderIds);
                        SqlCeSelect = SqlCeSelect.Replace("@Providerids", joinedstring);
                        CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                        CommonDB.SqlServerDataAdapter(SqlCeCommand, ref SqlCeDa);
                        DataTable SqlCeDt = new DataTable();
                        SqlCeDa.Fill(SqlCeDt);
                        return SqlCeDt;
                    }
                    else
                    {
                        SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalProviderData_Clinic : SynchLocalQRY.GetLocalProviderData;
                        CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", ServiceInstallId);
                        //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                        CommonDB.SqlServerDataAdapter(SqlCeCommand, ref SqlCeDa);
                        DataTable SqlCeDt = new DataTable();
                        SqlCeDa.Fill(SqlCeDt);
                        return SqlCeDt;
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
            #endregion

            #region SqlCeConnection
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {

                        if (Utility.is_scheduledCustomhourProvider)
                        {
                            //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                            string SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalProviderData_Clinic : SynchLocalQRY.GetLocalProviderData;
                            SqlCeSelect = SqlCeSelect + " AND Provider_EHR_ID in (@Providerids)";
                            string joinedstring = string.Join(",", Utility.CustomhourProviderIds);
                            SqlCeSelect = SqlCeSelect.Replace("@Providerids", joinedstring);
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", ServiceInstallId);
                                //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                                DataTable SqlCeDt = null;
                                using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                                {
                                    SqlCeDt = new DataTable();
                                    SqlCeDa.Fill(SqlCeDt);
                                    return SqlCeDt;
                                }
                            }
                        }
                        else
                        {
                            //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                            string SqlCeSelect = Clinic_Number != "" ? SynchLocalQRY.GetLocalProviderData_Clinic : SynchLocalQRY.GetLocalProviderData;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", ServiceInstallId);
                                //OdbcCommand.Parameters.AddWithValue("mst_employee_local_update_action", LocalUpdateAction);
                                DataTable SqlCeDt = null;
                                using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                                {
                                    SqlCeDt = new DataTable();
                                    SqlCeDa.Fill(SqlCeDt);
                                    return SqlCeDt;
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
            }
            #endregion
        }
        #region ZohoDetails
        public static DataTable GetLocalZohoDetailsData()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalZohoDetailsData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static bool Save_ZohoDetailsData(int Zoho_LocalDB_ID, string Organisation_ID, string Organisation_Name, string Location_ID, string Location_Name, string EHR_Pass, string EHR_User, string Server_User, string Server_Pass,
            bool is_Confirmed, bool is_Installed, bool is_Valid, string User_ID, string User_Name, string Clinic_Number, string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlCeSelect = string.Empty;

                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        if (Zoho_LocalDB_ID <= 0)
                        {
                            SqlCeCommand.CommandText = SynchLocalQRY.InsertLocalZohoDetailsData;
                        }
                        else
                        {
                            SqlCeCommand.CommandText = SynchLocalQRY.UpdateLocalZohoDetailsData;
                        }

                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Organisation_ID", Organisation_ID);
                        SqlCeCommand.Parameters.AddWithValue("Organisation_Name", Organisation_Name);
                        SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                        SqlCeCommand.Parameters.AddWithValue("Location_Name", Location_Name);
                        SqlCeCommand.Parameters.AddWithValue("EHR_Pass", EHR_Pass);
                        SqlCeCommand.Parameters.AddWithValue("EHR_User", EHR_User);
                        SqlCeCommand.Parameters.AddWithValue("Server_User", Server_User);
                        SqlCeCommand.Parameters.AddWithValue("Server_Pass", Server_Pass);
                        SqlCeCommand.Parameters.AddWithValue("is_Confirmed", is_Confirmed);
                        SqlCeCommand.Parameters.AddWithValue("is_Installed", is_Installed);
                        Utility.WriteToErrorLogFromAll("[InstallZohoAccess: CommandText: " + SqlCeCommand.CommandText + " ]");
                        Utility.WriteToErrorLogFromAll("[InstallZohoAccess: Is_Installed: " + is_Installed + " ]");
                        SqlCeCommand.Parameters.AddWithValue("is_Valid", is_Valid);
                        SqlCeCommand.Parameters.AddWithValue("User_ID", User_ID);
                        SqlCeCommand.Parameters.AddWithValue("User_Name", User_Name);
                        SqlCeCommand.Parameters.AddWithValue("Last_Updated_DateTime", DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        if (Zoho_LocalDB_ID > 0)
                        {
                            SqlCeCommand.Parameters.AddWithValue("Zoho_LocalDB_ID", Zoho_LocalDB_ID);
                        }
                        else
                        {
                            SqlCeCommand.Parameters.AddWithValue("Entry_DateTime", DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
                        }
                        SqlCeCommand.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public static DataTable GetLocalSystemUsersData(string Clinic_Number, string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetLocalSystemUsersData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
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

        public static bool Save_SystemUsersData(DataTable dtUsers, string Clinic_Number, string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlCeSelect = string.Empty;
                try
                {
                    SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        foreach (DataRow dr in dtUsers.Rows)
                        {
                            try
                            {
                                if (Convert.ToInt32(dr["InsUptDlt"].ToString()) != 0)
                                {
                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 1)
                                    {
                                        SqlCeCommand.CommandText = SynchLocalQRY.InsertLocalSystemUsersData;
                                    }
                                    else if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2)
                                    {
                                        SqlCeCommand.CommandText = SynchLocalQRY.UpdateLocalSystemUsersData;
                                    }
                                    else if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                    {
                                        SqlCeCommand.CommandText = SynchLocalQRY.DeleteLocalSystemUsersData;
                                    }

                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.Parameters.Clear();
                                    if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 3)
                                    {
                                        SqlCeCommand.Parameters.AddWithValue("Server_User_LocalDB_ID", dr["Server_User_LocalDB_ID"]);
                                    }
                                    else
                                    {
                                        SqlCeCommand.Parameters.AddWithValue("Server_User_Name", dr["Server_User_Name"]);
                                        SqlCeCommand.Parameters.AddWithValue("Server_User_Password", dr["Server_User_Password"]);
                                        SqlCeCommand.Parameters.AddWithValue("Is_Active", dr["Is_Active"]);
                                        SqlCeCommand.Parameters.AddWithValue("is_deleted", dr["is_deleted"]);
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        if (Convert.ToInt32(dr["InsUptDlt"].ToString()) == 2)
                                        {
                                            SqlCeCommand.Parameters.AddWithValue("Server_User_LocalDB_ID", dr["Server_User_LocalDB_ID"]);
                                        }
                                    }
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex2)
                            {
                                continue;
                            }
                        }

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static DataTable GetPushLocalSystemusersData(string Clinic_Number, string Service_Install_Id)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                    string SqlCeSelect = SynchLocalQRY.GetPushLocalSystemUsersData;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);

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
        //rooja Add last sync time for following hook - https://app.asana.com/0/1204010716278938/1204625995328405/f
        #region last sync time
        public static bool SynchDataLiveDB_Push_LastSyncTime()
        {
            bool IsLastSyncTime = false;
            try
            {

                string strLastSyncTime = "";
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                    {
                        var JsonSyncTime = new System.Text.StringBuilder();

                        Push_LastSyncDatetimeBO LastSyncDatetimeSub = new Push_LastSyncDatetimeBO
                        {
                            appointment = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncAppointment.ToString().Trim()),
                            appointment_type = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncAppointment_Type.ToString().Trim()),
                            confirm_appointment = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncConfirm_Appointment.ToString().Trim()),
                            operatory = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncOperatory.ToString().Trim()),
                            operatory_hours = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncOperatory_hours.ToString().Trim()),
                            patient = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncPatient.ToString().Trim()),
                            patient_payment_log = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncPatient_payment_log.ToString().Trim()),
                            patient_status = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncPatient_status.ToString().Trim()),
                            provider = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_lastSyncProvider.ToString().Trim()),
                            pull_patient_document = Utility.ConvertDatetimeToUTCaditFormat(Utility.pull_patient_document.ToString().Trim()),
                            pull_treatmentplan_document = Utility.ConvertDatetimeToUTCaditFormat(Utility.pull_treatmentplan_document.ToString().Trim()),
                            pull_appointment = Utility.ConvertDatetimeToUTCaditFormat(Utility.pull_appointment.ToString().Trim()),
                            sms_call_log = Utility.ConvertDatetimeToUTCaditFormat(Utility.Pull_lastSyncSMS_call_log.ToString().Trim()),
                            provider_hours = Utility.ConvertDatetimeToUTCaditFormat(Utility.Push_provider_hours.ToString().Trim()),
                            user = "adminapp",
                            currentDate = DateTime.Now.ToString("yyyyMMdd"),
                            locationId = Utility.DtLocationList.Rows[i]["Location_Id"].ToString(),
                        };

                        var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        JsonSyncTime.Append(javaScriptApptStatusSerializer.Serialize(LastSyncDatetimeSub) + ",");

                        if (JsonSyncTime.Length > 0)
                        {
                            string jsonString = JsonSyncTime.ToString().Remove(JsonSyncTime.Length - 1);
                            strLastSyncTime = PushLiveDatabaseDAL.Push_Local_To_LiveDatabase_LastSyncTime(jsonString, "ehrsynctime", Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_Id"].ToString());

                            if (strLastSyncTime.ToLower() != "Success".ToLower())
                            {
                                Utility.WriteToErrorLogFromAll("[Last_SyncTime Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + " - " + strLastSyncTime);
                            }
                        }
                        else
                        {
                            strLastSyncTime = "Success";
                        }
                    }

                    if (strLastSyncTime.ToLower() == "Success".ToLower())
                    {
                        IsLastSyncTime = true;
                    }
                    else
                    {
                        if (strLastSyncTime.Contains("The remote name could not be resolved:"))
                        {
                            IsLastSyncTime = false;
                        }
                        else
                        {
                            Utility.WriteToErrorLogFromAll("[Last_Synctime Sync (Local Database To Adit Server) ] : " + strLastSyncTime);
                            IsLastSyncTime = false;
                        }
                    }
                }

                if (IsLastSyncTime)
                {
                    //Utility.WriteToErrorLogFromAll("Last_Synctime Sync (Local Database To Adit Server) Successfully.");
                }
                //  }
                return IsLastSyncTime;
            }
            catch (Exception ex)
            {
                return IsLastSyncTime;
                Utility.WriteToErrorLogFromAll("[Last_Synctime Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }
        #endregion

        #region  Insurance Carrier
        public static bool Sync_check_for_InsuranceCarrier(string InsuranceCarrierId, string _filename_EHR_InsuranceCarrier_document = "", string _EHRLogdirectory_EHR_InsuranceCarrier_document = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = SynchLocalQRY.SelectInsuranceCarrierDocId;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    // sqlCommand.Transaction = transaction;

                    //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    sqlCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Web_ID", InsuranceCarrierId);
                    CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                    Utility.WriteSyncPullLog(_filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document, " Sync check for InsuranceCarrier for InsuranceCarrierId=" + InsuranceCarrierId.ToString());
                    DataTable sqlDt = new DataTable();
                    sqlDa.Fill(sqlDt);


                    if (sqlDt.Rows.Count == 0)
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
                    // transaction.Rollback();
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = SynchLocalQRY.SelectInsuranceCarrierDocId;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                            sqlCeCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Web_ID", InsuranceCarrierId);
                            Utility.WriteSyncPullLog(_filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document, " Sync check for InsuranceCarrier for InsuranceCarrierId=" + InsuranceCarrierId);
                            using (SqlCeDataAdapter sqlDa = new SqlCeDataAdapter(sqlCeCommand))
                            {
                                sqlDt = new DataTable();
                                sqlDa.Fill(sqlDt);
                            }

                            if (sqlDt.Rows.Count == 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        public static void CreateRecordForInsuranceCarrierDoc(string PatientName, string SubmittedDate, string Patient_EHR_ID, string Patient_Web_ID, string InsuranceCarrierId, string InsuranceCarrier_Doc_Name, string InsuranceCarrier_FolderName, string Clinic_Number, string Service_Install_Id, bool FileCreated, string _filename_EHR_InsuranceCarrier_document = "", string _EHRLogdirectory_EHR_InsuranceCarrier_document = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = SynchLocalQRY.Insert_InsuranceCarrierDoc;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    // sqlCommand.Transaction = transaction;

                    sqlCommand.Parameters.AddWithValue("PatientName,", PatientName);
                    sqlCommand.Parameters.AddWithValue("SubmittedDate", SubmittedDate);
                    sqlCommand.Parameters.AddWithValue("Patient_EHR_ID", Patient_EHR_ID);
                    sqlCommand.Parameters.AddWithValue("Patient_Web_ID", Patient_Web_ID);
                    sqlCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Name", InsuranceCarrier_Doc_Name);
                    sqlCommand.Parameters.AddWithValue("InsuranceCarrier_FolderName", InsuranceCarrier_FolderName);
                    sqlCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Web_Id", InsuranceCarrierId);
                    // sqlCommand.Parameters.AddWithValue("InsuranceCarrier_FolderName", InsuranceCarrier_FolderName);
                    sqlCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);
                    //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    //sqlCommand.Parameters.AddWithValue("Is_EHR_Updated", false);
                    //sqlCommand.Parameters.AddWithValue("Is_Adit_Updated", false);
                    sqlCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                    sqlCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    sqlCommand.Parameters.AddWithValue("@Is_PDF_Created", FileCreated);

                    sqlCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document, " Create Record For  InsuranceCarrier  InsuranceCarrierId=" + InsuranceCarrierId + " and PatientName=" + PatientName + ",Patient_EHR_ID =" + Patient_EHR_ID);
                    DataTable sqlDt = new DataTable();
                    sqlDa.Fill(sqlDt);

                }
                catch (Exception ex)
                {
                    // transaction.Rollback();
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = SynchLocalQRY.Insert_InsuranceCarrierDoc;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;


                            sqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", Patient_EHR_ID);
                            sqlCeCommand.Parameters.AddWithValue("Patient_Web_ID", Patient_Web_ID);
                            sqlCeCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Name", InsuranceCarrier_Doc_Name);
                            sqlCeCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Web_Id", InsuranceCarrierId);
                            sqlCeCommand.Parameters.AddWithValue("InsuranceCarrier_FolderName", InsuranceCarrier_FolderName);
                            sqlCeCommand.Parameters.AddWithValue("Entry_DateTime", dtCurrentDtTime);

                            sqlCeCommand.Parameters.AddWithValue("PatientName", PatientName);
                            sqlCeCommand.Parameters.AddWithValue("SubmittedDate", SubmittedDate);

                            sqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                            sqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            sqlCeCommand.Parameters.AddWithValue("Is_PDF_Created", FileCreated);
                            sqlCeCommand.ExecuteNonQuery();
                            //Utility.WriteSyncPullLog(_filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document, " Create Record For InsuranceCarrier with InsuranceCarrierId=" + InsuranceCarrierId + " and PatientName=" + PatientName + ",Patient_EHR_ID=" + Patient_EHR_ID);
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        public static void UpdateInsuranceCarrierDocInlocal(string InsuranceCarrier_Id, string _filename_EHR_InsuranceCarrier_document = "", string _EHRLogdirectory_EHR_InsuranceCarrier_document = "")
        {
            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = SynchLocalQRY.UpdateInsuranceCarrierDocAditUpdated;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    // sqlCommand.Transaction = transaction;

                    //sqlCommand.Parameters.AddWithValue("Last_Sync_Date", dtCurrentDtTime);
                    sqlCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_EHR_ID", InsuranceCarrier_Id);
                    sqlCommand.ExecuteNonQuery();
                    Utility.WriteSyncPullLog(_filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document, " Update InsuranceCarrier Doc In local for InsuranceCarrierId=" + InsuranceCarrier_Id);
                }
                catch (Exception ex)
                {
                    // transaction.Rollback();
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string sqlSelect = SynchLocalQRY.UpdateInsuranceCarrierDocAditUpdated;
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            sqlCeCommand.Parameters.AddWithValue("InsuranceCarrier_Doc_Web_ID", InsuranceCarrier_Id);
                            sqlCeCommand.ExecuteNonQuery();
                            Utility.WriteSyncPullLog(_filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document, " Update InsuranceCarrier Doc In local for InsuranceCarrierId=" + InsuranceCarrier_Id);
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        public static DataTable ChangeStatusForInsuranceCarrierDoc(string StatusType)
        {
            string sqlSelect;
            DataTable dt = new DataTable();
            if (StatusType == "Importing")
            {
                sqlSelect = SynchLocalQRY.ImportingInsuranceCarrierDocStatus;
            }
            else if (StatusType == "Completed")
            {
                sqlSelect = SynchLocalQRY.CompletedInsuranceCarrierDocStatus;
            }
            else
            {
                return dt;
            }


            #region SqlConnection
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlCeCommand = null;
                SqlDataAdapter SqlCeDa = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);
                DateTime ToDate = Utility.LastSyncDateAditServer;
                if (conn.State == ConnectionState.Closed) conn.Open();

                try
                {
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                    CommonDB.SqlServerDataAdapter(SqlCeCommand, ref SqlCeDa);
                    SqlCeDa.Fill(dt);

                    return dt;
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
                    //   SqlCetx = conn.BeginTransaction();

                    try
                    {
                        DataTable sqlDt = null;
                        // if (conn.State == ConnectionState.Closed) conn.Open(); 
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        using (SqlCeCommand sqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                        {
                            sqlCeCommand.CommandType = CommandType.Text;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(sqlCeCommand))
                            {
                                SqlCeDa.Fill(dt);
                            }
                            return dt;
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
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

        }

        #endregion      
    }

}

