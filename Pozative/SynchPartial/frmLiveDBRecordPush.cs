
using Pozative.BAL;
using Pozative.BO;
using Pozative.UTL;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Security;
using Pozative.DAL;
using System.IO;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable

        public static bool IsProviderSyncPush = false;
        public static bool IsOperatorySyncPush = false;
        public static bool IsMedicalHistorySyncPush = false;
        public static bool IsApptTypeSyncPush = false;        
        //public static bool IsApptSyncingRunning = false;
        //public static bool IsPatientSyncingRunning = false;
        //public static bool IsPatientSyncingInQueue = false;
        #endregion

        #region Appointment

        public static void SynchDataLiveDB_Push_Appointment(string strApptID = "")
        {
            //if (!IsPatientSyncingRunning)
            //{
            //    IsApptSyncingRunning = true;
            if (Utility.IsExternalAppointmentSync)
            {
                IsProviderSyncPush = true;
                IsOperatorySyncPush = true;
                IsApptTypeSyncPush = true;
            }

            if (!string.IsNullOrEmpty(strApptID))
            {
                IsProviderSyncPush = true;
                IsOperatorySyncPush = true;
                IsApptTypeSyncPush = true;
            }

            if (IsProviderSyncPush && IsOperatorySyncPush && IsApptTypeSyncPush) //&& Utility.AditLocationSyncEnable
            {
                try
                {
                    bool IsSynchAppointment = true;
                    DataTable dtLocalApptType = new DataTable();
                    DataTable dtLocalOperatory = new DataTable();
                    DataTable dtLocalProvider = new DataTable();
                    DataTable dtLocalAppointment = SynchLocalBAL.GetLastTwoDaysLocalAppointmentData(strApptID);
                    Utility.WriteToSyncLogFile_All("Appointment Push Count " + dtLocalAppointment.Rows.Count.ToString());
                    if (dtLocalAppointment.Rows.Count > 0)
                    {
                        string Opt_Web_ID = string.Empty;
                        string Prod_IDs = string.Empty;
                        string ApptType_Web_ID = string.Empty;
                        string TmpAppt_DateTime = string.Empty;
                        string TmpAppt_EndDateTime = string.Empty;
                        string TmpEntry_DateTime = string.Empty;
                        string TmpEHREntry_DateTime = string.Empty;
                        int PushAppointmentRecord = 0;
                        int TotalPushPatientRecord = dtLocalAppointment.Rows.Count;
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalAppointment, 200);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalAppointment, Utility.mstSyncBatchSize.Appointment);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strAppointment = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    dtLocalProvider = SynchLocalBAL.GetLocalProviderData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());

                                    var JsonAppointment = new System.Text.StringBuilder();
                                    foreach (DataRow dtAppointmentRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        PushAppointmentRecord = PushAppointmentRecord + 1;
                                        Opt_Web_ID = string.Empty;
                                        Prod_IDs = string.Empty;
                                        ApptType_Web_ID = string.Empty;
                                        TmpAppt_DateTime = string.Empty;
                                        TmpAppt_EndDateTime = string.Empty;
                                        TmpEntry_DateTime = string.Empty;
                                        Opt_Web_ID = string.Empty;
                                        DataRow[] rowOpt = dtLocalOperatory.Select("Operatory_EHR_ID = '" + dtAppointmentRow["Operatory_EHR_ID"].ToString().Trim().Replace("'", "''") + "' ");
                                        if (rowOpt.Length > 0)
                                        {
                                            Opt_Web_ID = rowOpt[0]["Operatory_Web_ID"].ToString().Trim().Replace("'", "");
                                        }

                                        string[] Opt_Web_IDs;
                                        if (Opt_Web_ID == string.Empty)
                                        {
                                            Opt_Web_IDs = new string[0];
                                        }
                                        else
                                        {
                                            Opt_Web_IDs = Opt_Web_ID.ToString().Trim().Split(';');
                                        }

                                        Prod_IDs = string.Empty;
                                        string[] ProviderIDS = dtAppointmentRow["Provider_EHR_ID"].ToString().Trim().Split(';');
                                        foreach (string PID in ProviderIDS)
                                        {
                                            if (PID.Trim() != string.Empty)
                                            {
                                                DataRow[] rowProd = dtLocalProvider.Select("Provider_EHR_ID = '" + PID.Trim().Replace("'", "''") + "' AND Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' ");
                                                if (rowProd.Length > 0)
                                                {
                                                    Prod_IDs = Prod_IDs + rowProd[0]["Provider_Web_ID"].ToString().Trim() + ";";
                                                }
                                            }
                                        }
                                        if (Prod_IDs.Length > 0)
                                        {
                                            Prod_IDs = Prod_IDs.Substring(0, Prod_IDs.Length - 1);
                                        }

                                        string[] ApptProdIDs;
                                        if (Prod_IDs == string.Empty)
                                        {
                                            ApptProdIDs = new string[0];
                                        }
                                        else
                                        {
                                            ApptProdIDs = Prod_IDs.ToString().Trim().Split(';');
                                        }

                                        ApptType_Web_ID = string.Empty;
                                        DataRow[] rowApptType = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtAppointmentRow["ApptType_EHR_ID"].ToString().Trim().Replace("'", "''") + "' AND Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' ");
                                        if (rowApptType.Length > 0)
                                        {
                                            ApptType_Web_ID = rowApptType[0]["ApptType_Web_ID"].ToString().Trim();
                                        }

                                        TmpAppt_DateTime = dtAppointmentRow["Appt_DateTime"].ToString().Trim();
                                        if (TmpAppt_DateTime == string.Empty)
                                        {
                                            TmpAppt_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpAppt_DateTime = Convert.ToDateTime(TmpAppt_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpAppt_EndDateTime = dtAppointmentRow["Appt_EndDateTime"].ToString().Trim();
                                        if (TmpAppt_EndDateTime == string.Empty)
                                        {
                                            TmpAppt_EndDateTime = TmpAppt_DateTime;
                                        }
                                        else
                                        {
                                            TmpAppt_EndDateTime = Convert.ToDateTime(TmpAppt_EndDateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpEntry_DateTime = dtAppointmentRow["Entry_DateTime"].ToString().Trim();
                                        if (TmpEntry_DateTime == string.Empty)
                                        {
                                            TmpEntry_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpEntry_DateTime = Convert.ToDateTime(TmpEntry_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpEHREntry_DateTime = dtAppointmentRow["EHR_Entry_DateTime"].ToString().Trim();
                                        if (TmpEHREntry_DateTime == string.Empty)
                                        {
                                            TmpEHREntry_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpEHREntry_DateTime = Convert.ToDateTime(TmpEHREntry_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        bool isEHRUpdated = true;
                                        try
                                        {
                                            isEHRUpdated = Convert.ToBoolean(dtAppointmentRow["is_ehr_updated"].ToString().Trim());
                                        }
                                        catch
                                        {
                                            isEHRUpdated = true;
                                        }

                                        string tmpBirthdate = "";
                                        try
                                        {
                                            if (dtAppointmentRow["birth_date"] != null && dtAppointmentRow["birth_date"].ToString() != string.Empty)
                                            {
                                                tmpBirthdate = Convert.ToDateTime(dtAppointmentRow["birth_date"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            tmpBirthdate = "";

                                        }

                                        Push_AppointmentBO Appointment = new Push_AppointmentBO
                                        {
                                            First_Name = dtAppointmentRow["First_Name"].ToString().Trim(),
                                            Last_Name = dtAppointmentRow["Last_Name"].ToString().Trim(),
                                            Appt_Web_ID = dtAppointmentRow["Appt_Web_ID"].ToString().Trim(),
                                            Location_ID = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(), //Utility.Location_ID,
                                            MI = dtAppointmentRow["MI"].ToString().Trim(),
                                            Home_Contact = dtAppointmentRow["Home_Contact"].ToString().Trim(),
                                            Mobile_Contact = dtAppointmentRow["Mobile_Contact"].ToString().Trim(),
                                            Email = dtAppointmentRow["Email"].ToString().Trim(),
                                            Address = dtAppointmentRow["Address"].ToString().Trim(),
                                            City = dtAppointmentRow["City"].ToString().Trim(),
                                            ST = dtAppointmentRow["ST"].ToString().Trim(),
                                            Zip = dtAppointmentRow["Zip"].ToString().Trim(),
                                            Operatory_Name = Opt_Web_IDs,
                                            Provider_Name = ApptProdIDs,
                                            comment = dtAppointmentRow["comment"].ToString().Trim(),
                                            birth_date = tmpBirthdate,
                                            Appt_Type = ApptType_Web_ID,
                                            Appt_DateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpAppt_DateTime),
                                            Appt_EndDateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpAppt_EndDateTime),
                                            Status = dtAppointmentRow["Status"].ToString().Trim(),
                                            Is_Appt = dtAppointmentRow["Is_Appt"].ToString().Trim(),
                                            is_ehr_updated = isEHRUpdated,
                                            Entry_DateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpEntry_DateTime),
                                            ehr_entry_date = Convert.ToDateTime(TmpEHREntry_DateTime).ToString("yyyy-MM-dd"),
                                            Patient_Status = dtAppointmentRow["Patient_Status"].ToString().Trim(),
                                            appointment_status_ehr_key = dtAppointmentRow["appointment_status_ehr_key"].ToString().Trim(),
                                            Appointment_Status = dtAppointmentRow["Appointment_Status"].ToString().Trim(),
                                            confirmed_status_ehr_key = dtAppointmentRow["confirmed_status_ehr_key"].ToString().Trim(),
                                            confirmed_status = dtAppointmentRow["confirmed_status"].ToString().Trim(),
                                            unschedule_status_ehr_key = dtAppointmentRow["unschedule_status_ehr_key"].ToString().Trim(),
                                            unschedule_status = dtAppointmentRow["unschedule_status"].ToString().Trim(),
                                            Organization_ID = Utility.Organization_ID,
                                            appt_ehr_id = dtAppointmentRow["Appt_EHR_ID"].ToString(),
                                            appt_localdb_id = dtAppointmentRow["Appt_LocalDB_ID"].ToString(),
                                            patient_ehr_id = dtAppointmentRow["patient_ehr_id"].ToString(),
                                            created_by = Utility.User_ID,
                                            is_deleted = Convert.ToBoolean(dtAppointmentRow["is_deleted"].ToString()),
                                            ParentLocation_ID = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            is_asap = Convert.ToBoolean(dtAppointmentRow["is_asap"].ToString()),
                                            proceduredesc = dtAppointmentRow["proceduredesc"].ToString(),
                                            procedurecode = dtAppointmentRow["procedurecode"].ToString()
                                            //ehr_patient_status = dtAppointmentRow["Patient_Status"].ToString().Trim()
                                        };

                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonAppointment.Append(javaScriptSerializer.Serialize(Appointment) + ",");
                                    }
                                    ////var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    ////string jsonString = javaScriptSerializer.Serialize(Appointment);
                                    ////string staAppointmenttype = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase(jsonString, "appointment", dtAppointmentRow["Appt_EHR_ID"].ToString(), dtAppointmentRow["Appt_Web_ID"].ToString());

                                    if (JsonAppointment.Length > 0)
                                    {
                                        string jsonString = "[" + JsonAppointment.ToString().Remove(JsonAppointment.Length - 1) + "]";
                                        strAppointment = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Appointment(jsonString, "appointment", Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_ID"].ToString());
                                        JsonAppointment = new StringBuilder();
                                    }
                                    else
                                        strAppointment = "Success";

                                    if (strAppointment.ToLower() == "Success".ToLower())
                                    {
                                        IsSynchAppointment = true;
                                    }
                                    else
                                    {
                                        if (strAppointment.Contains("The remote name could not be resolved:"))
                                        {
                                            IsSynchAppointment = false;
                                        }
                                        else
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[Appointment Sync (Local Database To Adit Server) ] : " + strAppointment);
                                            IsSynchAppointment = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (IsSynchAppointment)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment_Push");
                        //SynchLocalDAL.SynchDataLiveDB_Push_LastSyncTime();
                        GoalBase.WriteToSyncLogFile_Static("Appointment Sync (Local Database To Adit Server) Successfully.");
                    }
                    //IsApptSyncingRunning = false;
                    //if (IsPatientSyncingInQueue)
                    //{
                    //    SynchDataLiveDB_Push_Patient();
                    //}
                }
                catch (Exception ex)
                {
                    // IsApptSyncingRunning = false;
                    GoalBase.WriteToErrorLogFile_Static("[Appointment Sync (Local Database To Adit Server) ] : " + ex.Message);
                }
            }
            // }
        }

        public static void SynchDataLiveDB_Push_AppointmentMultiLocation()
        {
            //if (!IsPatientSyncingRunning)
            //{
            //    IsApptSyncingRunning = true;
            if (Utility.IsExternalAppointmentSync)
            {
                IsProviderSyncPush = true;
                IsOperatorySyncPush = true;
                IsApptTypeSyncPush = true;
            }
            if (IsProviderSyncPush && IsOperatorySyncPush && IsApptTypeSyncPush) //&& Utility.AditLocationSyncEnable
            {
                try
                {

                    bool IsSynchAppointment = true;
                    DataTable dtLocalApptType = new DataTable();
                    DataTable dtLocalOperatory = new DataTable();
                    DataTable dtLocalProvider = new DataTable();
                    DataTable dtLocalAppointment = SynchLocalBAL.GetLastTwoDaysLocalAppointmentData();
                    Utility.WriteToSyncLogFile_All("Appointment Push Count " + dtLocalAppointment.Rows.Count.ToString());
                    if (dtLocalAppointment.Rows.Count > 0)
                    {
                        string Opt_Web_ID = string.Empty;
                        string Prod_IDs = string.Empty;
                        string ApptType_Web_ID = string.Empty;
                        string TmpAppt_DateTime = string.Empty;
                        string TmpAppt_EndDateTime = string.Empty;
                        string TmpEntry_DateTime = string.Empty;
                        string TmpEHREntry_DateTime = string.Empty;
                        int PushAppointmentRecord = 0;
                        int TotalPushPatientRecord = dtLocalAppointment.Rows.Count;
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalAppointment, 200);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalAppointment, Utility.mstSyncBatchSize.Appointment_Multi_location);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strAppointment = "";
                            string[] MultiLocation = Utility.DtLocationList.Copy().AsEnumerable().Select(r => r.Field<string>("Location_Id")).ToArray();
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    dtLocalProvider = SynchLocalBAL.GetLocalProviderData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());

                                    var JsonAppointment = new System.Text.StringBuilder();
                                    foreach (DataRow dtAppointmentRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        PushAppointmentRecord = PushAppointmentRecord + 1;
                                        Opt_Web_ID = string.Empty;
                                        Prod_IDs = string.Empty;
                                        ApptType_Web_ID = string.Empty;
                                        TmpAppt_DateTime = string.Empty;
                                        TmpAppt_EndDateTime = string.Empty;
                                        TmpEntry_DateTime = string.Empty;
                                        Opt_Web_ID = string.Empty;
                                        DataRow[] rowOpt = dtLocalOperatory.Select("Operatory_EHR_ID = '" + dtAppointmentRow["Operatory_EHR_ID"].ToString().Trim().Replace("'", "''") + "' ");
                                        if (rowOpt.Length > 0)
                                        {
                                            Opt_Web_ID = rowOpt[0]["Operatory_Web_ID"].ToString().Trim().Replace("'", "");
                                        }

                                        string[] Opt_Web_IDs;
                                        if (Opt_Web_ID == string.Empty)
                                        {
                                            Opt_Web_IDs = new string[0];
                                        }
                                        else
                                        {
                                            Opt_Web_IDs = Opt_Web_ID.ToString().Trim().Split(';');
                                        }

                                        Prod_IDs = string.Empty;
                                        string[] ProviderIDS = dtAppointmentRow["Provider_EHR_ID"].ToString().Trim().Split(';');
                                        foreach (string PID in ProviderIDS)
                                        {
                                            if (PID.Trim() != string.Empty)
                                            {
                                                DataRow[] rowProd = dtLocalProvider.Select("Provider_EHR_ID = '" + PID.Trim().Replace("'", "''") + "' AND Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' ");
                                                if (rowProd.Length > 0)
                                                {
                                                    Prod_IDs = Prod_IDs + rowProd[0]["Provider_Web_ID"].ToString().Trim() + ";";
                                                }
                                            }
                                        }
                                        if (Prod_IDs.Length > 0)
                                        {
                                            Prod_IDs = Prod_IDs.Substring(0, Prod_IDs.Length - 1);
                                        }

                                        string[] ApptProdIDs;
                                        if (Prod_IDs == string.Empty)
                                        {
                                            ApptProdIDs = new string[0];
                                        }
                                        else
                                        {
                                            ApptProdIDs = Prod_IDs.ToString().Trim().Split(';');
                                        }

                                        ApptType_Web_ID = string.Empty;
                                        DataRow[] rowApptType = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtAppointmentRow["ApptType_EHR_ID"].ToString().Trim().Replace("'", "''") + "' AND Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' ");
                                        if (rowApptType.Length > 0)
                                        {
                                            ApptType_Web_ID = rowApptType[0]["ApptType_Web_ID"].ToString().Trim();
                                        }

                                        TmpAppt_DateTime = dtAppointmentRow["Appt_DateTime"].ToString().Trim();
                                        if (TmpAppt_DateTime == string.Empty)
                                        {
                                            TmpAppt_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpAppt_DateTime = Convert.ToDateTime(TmpAppt_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpAppt_EndDateTime = dtAppointmentRow["Appt_EndDateTime"].ToString().Trim();
                                        if (TmpAppt_EndDateTime == string.Empty)
                                        {
                                            TmpAppt_EndDateTime = TmpAppt_DateTime;
                                        }
                                        else
                                        {
                                            TmpAppt_EndDateTime = Convert.ToDateTime(TmpAppt_EndDateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpEntry_DateTime = dtAppointmentRow["Entry_DateTime"].ToString().Trim();
                                        if (TmpEntry_DateTime == string.Empty)
                                        {
                                            TmpEntry_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpEntry_DateTime = Convert.ToDateTime(TmpEntry_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpEHREntry_DateTime = dtAppointmentRow["EHR_Entry_DateTime"].ToString().Trim();
                                        if (TmpEHREntry_DateTime == string.Empty)
                                        {
                                            TmpEHREntry_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpEHREntry_DateTime = Convert.ToDateTime(TmpEHREntry_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        bool isEHRUpdated = true;
                                        try
                                        {
                                            isEHRUpdated = Convert.ToBoolean(dtAppointmentRow["is_ehr_updated"].ToString().Trim());
                                        }
                                        catch
                                        {
                                            isEHRUpdated = true;
                                        }

                                        string tmpBirthdate = "";
                                        try
                                        {
                                            if (dtAppointmentRow["birth_date"] != null && dtAppointmentRow["birth_date"].ToString() != string.Empty)
                                            {
                                                tmpBirthdate = Convert.ToDateTime(dtAppointmentRow["birth_date"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            tmpBirthdate = "";

                                        }

                                        Push_AppointmentBOMultiLocation Appointment = new Push_AppointmentBOMultiLocation
                                        {
                                            First_Name = dtAppointmentRow["First_Name"].ToString().Trim(),
                                            Last_Name = dtAppointmentRow["Last_Name"].ToString().Trim(),
                                            Appt_Web_ID = dtAppointmentRow["Appt_Web_ID"].ToString().Trim(),
                                            Location_ID = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(), //Utility.Location_ID,
                                            MI = dtAppointmentRow["MI"].ToString().Trim(),
                                            Home_Contact = dtAppointmentRow["Home_Contact"].ToString().Trim(),
                                            Mobile_Contact = dtAppointmentRow["Mobile_Contact"].ToString().Trim(),
                                            Email = dtAppointmentRow["Email"].ToString().Trim(),
                                            Address = dtAppointmentRow["Address"].ToString().Trim(),
                                            City = dtAppointmentRow["City"].ToString().Trim(),
                                            ST = dtAppointmentRow["ST"].ToString().Trim(),
                                            Zip = dtAppointmentRow["Zip"].ToString().Trim(),
                                            Operatory_Name = Opt_Web_IDs,
                                            Provider_Name = ApptProdIDs,
                                            comment = dtAppointmentRow["comment"].ToString().Trim(),
                                            birth_date = tmpBirthdate,
                                            Appt_Type = ApptType_Web_ID,
                                            Appt_DateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpAppt_DateTime),
                                            Appt_EndDateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpAppt_EndDateTime),
                                            Status = dtAppointmentRow["Status"].ToString().Trim(),
                                            Is_Appt = dtAppointmentRow["Is_Appt"].ToString().Trim(),
                                            is_ehr_updated = isEHRUpdated,
                                            Entry_DateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpEntry_DateTime),
                                            ehr_entry_date = Convert.ToDateTime(TmpEHREntry_DateTime).ToString("yyyy-MM-dd"),
                                            Patient_Status = dtAppointmentRow["Patient_Status"].ToString().Trim(),
                                            appointment_status_ehr_key = dtAppointmentRow["appointment_status_ehr_key"].ToString().Trim(),
                                            Appointment_Status = dtAppointmentRow["Appointment_Status"].ToString().Trim(),
                                            confirmed_status_ehr_key = dtAppointmentRow["confirmed_status_ehr_key"].ToString().Trim(),
                                            confirmed_status = dtAppointmentRow["confirmed_status"].ToString().Trim(),
                                            unschedule_status_ehr_key = dtAppointmentRow["unschedule_status_ehr_key"].ToString().Trim(),
                                            unschedule_status = dtAppointmentRow["unschedule_status"].ToString().Trim(),
                                            Organization_ID = Utility.Organization_ID,
                                            appt_ehr_id = dtAppointmentRow["Appt_EHR_ID"].ToString(),
                                            appt_localdb_id = dtAppointmentRow["Appt_LocalDB_ID"].ToString(),
                                            patient_ehr_id = dtAppointmentRow["patient_ehr_id"].ToString(),
                                            created_by = Utility.User_ID,
                                            is_deleted = Convert.ToBoolean(dtAppointmentRow["is_deleted"].ToString()),
                                            ParentLocation_ID = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            is_asap = Convert.ToBoolean(dtAppointmentRow["is_asap"].ToString()),
                                            proceduredesc = dtAppointmentRow["proceduredesc"].ToString(),
                                            procedurecode = dtAppointmentRow["procedurecode"].ToString(),
                                            office_id = dtAppointmentRow["Clinic_Number"].ToString(),
                                            multilocation = MultiLocation
                                           // ehr_patient_status = dtAppointmentRow["Patient_Status"].ToString().Trim()
                                        };

                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonAppointment.Append(javaScriptSerializer.Serialize(Appointment) + ",");
                                    }
                                    ////var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    ////string jsonString = javaScriptSerializer.Serialize(Appointment);
                                    ////string staAppointmenttype = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase(jsonString, "appointment", dtAppointmentRow["Appt_EHR_ID"].ToString(), dtAppointmentRow["Appt_Web_ID"].ToString());

                                    if (JsonAppointment.Length > 0)
                                    {
                                        string jsonString = "[" + JsonAppointment.ToString().Remove(JsonAppointment.Length - 1) + "]";
                                        strAppointment = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Appointment(jsonString, "appointmentmultilocation", Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_ID"].ToString());
                                        JsonAppointment = new StringBuilder();
                                    }
                                    else
                                        strAppointment = "Success";

                                    if (strAppointment.ToLower() == "Success".ToLower())
                                    {
                                        IsSynchAppointment = true;
                                    }
                                    else
                                    {
                                        if (strAppointment.Contains("The remote name could not be resolved:"))
                                        {
                                            IsSynchAppointment = false;
                                        }
                                        else
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[Appointment Sync (Local Database To Adit Server) ] : " + strAppointment);
                                            IsSynchAppointment = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (IsSynchAppointment)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment_Push");
                        GoalBase.WriteToSyncLogFile_Static("Appointment Sync (Local Database To Adit Server) Successfully.");
                    }
                    //IsApptSyncingRunning = false;
                    //if (IsPatientSyncingInQueue)
                    //{
                    //    SynchDataLiveDB_Push_Patient();
                    //}
                }
                catch (Exception ex)
                {
                    // IsApptSyncingRunning = false;
                    GoalBase.WriteToErrorLogFile_Static("[Appointment Sync (Local Database To Adit Server) ] : " + ex.Message);
                }
            }
            // }
        }

        public static void SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchAppointmentIs_Appt_DoubleBook = true;

                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                    {
                        DataTable dtIs_Appt_DoubleBook_Appt = SynchLocalBAL.GetIs_Appt_DoubleBook_AppointmentData(Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString());
                        if (dtIs_Appt_DoubleBook_Appt.Rows.Count > 0)
                        {
                            string strAppt_Web_ID = string.Empty;
                            var JsonAppointment = new System.Text.StringBuilder();
                            Push_Is_Appt_DoubleBook Is_Appt_DoubleBook = new Push_Is_Appt_DoubleBook();

                            foreach (DataRow dtAppointmentRow in dtIs_Appt_DoubleBook_Appt.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                            {
                                Push_conflictAppointmentsListAry Appt = new Push_conflictAppointmentsListAry
                                {
                                    Appt_Web_ID = dtAppointmentRow["appt_Web_ID"].ToString().Trim(),
                                    Organization_ID = Utility.Organization_ID,
                                    Location_ID = Utility.DtLocationList.Rows[i]["Location_ID"].ToString(), //Utility.GetLocationIdByClinicNumber(dtAppointmentRow["Location_ID"].ToString()),
                                    created_by = Utility.User_ID
                                };
                                Is_Appt_DoubleBook.conflictAppointments.Add(Appt);
                            }

                            if (Is_Appt_DoubleBook.conflictAppointments.Count > 0)
                            {
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                string jsonString = javaScriptSerializer.Serialize(Is_Appt_DoubleBook);

                                string strAppointment = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Is_Appt_DoubleBook(jsonString, "is_appt_doublebook", Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                                if (strAppointment.ToLower() == "Success".ToLower())
                                {
                                    IsSynchAppointmentIs_Appt_DoubleBook = true;
                                }
                                else
                                {
                                    if (strAppointment.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchAppointmentIs_Appt_DoubleBook = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Appointment update double booking (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strAppointment);
                                        IsSynchAppointmentIs_Appt_DoubleBook = false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (IsSynchAppointmentIs_Appt_DoubleBook)
                {
                    GoalBase.WriteToSyncLogFile_Static("Appointment update double booking (Local Database To Adit Server) Successfully.");
                }
                // }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Appointment update double booking (Local Database To Adit Server) ] : " + ex.Message);
            }

        }

        #endregion

        #region OperatoryEvent

        public static void SynchDataLiveDB_Push_OperatoryEvent()
        {
            if (IsOperatorySyncPush) //&& Utility.AditLocationSyncEnable
            {
                try
                {
                    bool IsSynchOperatoryEvent = true;

                    DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetPushLocalOperatoryEventData();
                    string Opt_Web_ID = string.Empty;
                    string TmpAppt_DateTime = string.Empty;
                    string TmpAppt_EndDateTime = string.Empty;
                    string TmpEntry_DateTime = string.Empty;
                    if (dtLocalOperatoryEvent.Rows.Count > 0)
                    {
                        PushOperatoryEventRecord = 0;
                        TotalPushOperatoryEventRecord = dtLocalOperatoryEvent.Rows.Count;
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalOperatoryEvent, 200);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalOperatoryEvent, Utility.mstSyncBatchSize.OperatoryEvent);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strOperatoryEvent = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());

                                    var JsonOperatoryEvent = new System.Text.StringBuilder();
                                    foreach (DataRow dtOperatoryEventRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        PushOperatoryEventRecord = PushOperatoryEventRecord + 1;

                                        TmpAppt_DateTime = dtOperatoryEventRow["StartTime"].ToString().Trim();
                                        if (TmpAppt_DateTime == string.Empty)
                                        {
                                            TmpAppt_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpAppt_DateTime = Convert.ToDateTime(TmpAppt_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpAppt_EndDateTime = dtOperatoryEventRow["EndTime"].ToString().Trim();
                                        if (TmpAppt_EndDateTime == string.Empty)
                                        {
                                            TmpAppt_EndDateTime = TmpAppt_DateTime;
                                        }
                                        else
                                        {
                                            TmpAppt_EndDateTime = Convert.ToDateTime(TmpAppt_EndDateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpEntry_DateTime = dtOperatoryEventRow["Entry_DateTime"].ToString().Trim();
                                        if (TmpEntry_DateTime == string.Empty)
                                        {
                                            TmpEntry_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpEntry_DateTime = Convert.ToDateTime(TmpEntry_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }


                                        Opt_Web_ID = string.Empty;
                                        DataRow[] rowOpt = dtLocalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtOperatoryEventRow["Operatory_EHR_ID"].ToString().Trim() + "' ");
                                        if (rowOpt.Length > 0)
                                        {
                                            Opt_Web_ID = rowOpt[0]["Operatory_Web_ID"].ToString().Trim();
                                        }
                                        string[] Opt_Web_IDs;
                                        if (Opt_Web_ID == string.Empty)
                                        {
                                            Opt_Web_IDs = new string[0];
                                        }
                                        else
                                        {
                                            Opt_Web_IDs = Opt_Web_ID.ToString().Trim().Split(';');
                                        }
                                        bool Allow_Book_Appt_flg = false;
                                        if (Utility.Application_ID == 2)
                                        {
                                            if (dtOperatoryEventRow["Allow_Book_Appt"].ToString().Trim() == "0" || dtOperatoryEventRow["Allow_Book_Appt"].ToString().Trim() == "false")
                                            {
                                                Allow_Book_Appt_flg = false;
                                            }
                                            else
                                            {
                                                Allow_Book_Appt_flg = true;
                                            }
                                        }
                                        Push_OperatoryEventBO OperatoryEvent = new Push_OperatoryEventBO
                                        {
                                            Appt_Web_ID = dtOperatoryEventRow["OE_Web_ID"].ToString().Trim(),
                                            Location_ID = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.Location_ID,
                                            Operatory_Name = Opt_Web_IDs,
                                            comment = dtOperatoryEventRow["comment"].ToString().Trim(),
                                            Appt_DateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpAppt_DateTime),
                                            Appt_EndDateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpAppt_EndDateTime),
                                            Entry_DateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpEntry_DateTime),
                                            Organization_ID = Utility.Organization_ID,
                                            appt_ehr_id = dtOperatoryEventRow["OE_EHR_ID"].ToString(),
                                            appt_localdb_id = dtOperatoryEventRow["OE_LocalDB_ID"].ToString(),
                                            Appointment_Status = "Block",
                                            created_by = Utility.User_ID,
                                            is_deleted = Convert.ToBoolean(dtOperatoryEventRow["is_deleted"].ToString()),
                                            ParentLocation_ID = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(),//Utility.Loc_ID
                                            Allow_Book_Appt = Convert.ToBoolean(dtOperatoryEventRow["Allow_Book_Appt"].ToString().Trim())
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonOperatoryEvent.Append(javaScriptSerializer.Serialize(OperatoryEvent) + ",");

                                    }
                                    ////var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    ////string jsonString = javaScriptSerializer.Serialize(Appointment);
                                    ////string staAppointmenttype = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase(jsonString, "appointment", dtAppointmentRow["Appt_EHR_ID"].ToString(), dtAppointmentRow["Appt_Web_ID"].ToString());
                                    if (JsonOperatoryEvent.Length > 0)
                                    {
                                        string jsonString = "[" + JsonOperatoryEvent.ToString().Remove(JsonOperatoryEvent.Length - 1) + "]";
                                        strOperatoryEvent = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_OperatoryEvent(jsonString, "OperatoryEvent", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strOperatoryEvent.ToLower() != "Success".ToLower())
                                            GoalBase.WriteToErrorLogFile_Static("[OperatoryEvent Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strOperatoryEvent);
                                    }
                                    else
                                    {
                                        strOperatoryEvent = "Success";
                                    }
                                }

                                if (strOperatoryEvent.ToLower() == "Success".ToLower())
                                {
                                    IsSynchOperatoryEvent = true;
                                }
                                else
                                {
                                    if (strOperatoryEvent.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchOperatoryEvent = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[OperatoryEvent Sync (Local Database To Adit Server) ] : " + strOperatoryEvent);
                                        IsSynchOperatoryEvent = false;
                                    }
                                }
                            }
                        }
                    }
                    if (IsSynchOperatoryEvent)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent_Push");
                        GoalBase.WriteToSyncLogFile_Static("OperatoryEvent Sync (Local Database To Adit Server) Successfully.");
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[OperatoryEvent  Sync (Local Database To Adit Server) ] : " + ex.Message);
                }
            }
        }

        public static void SynchDataLiveDB_Push_OperatoryDayOff()
        {
            if (IsOperatorySyncPush) //&& Utility.AditLocationSyncEnable
            {
                try
                {
                    bool IsSynchOperatoryEvent = true;

                    DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetPushLocalOperatoryDayOffData();
                    string Opt_Web_ID = string.Empty;
                    string TmpAppt_DateTime = string.Empty;
                    string TmpAppt_EndDateTime = string.Empty;
                    string TmpEntry_DateTime = string.Empty;
                    string prov_Web_ID = string.Empty;
                    if (dtLocalOperatoryEvent.Rows.Count > 0)
                    {
                        PushOperatoryEventRecord = 0;
                        TotalPushOperatoryEventRecord = dtLocalOperatoryEvent.Rows.Count;
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalOperatoryEvent, 200);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalOperatoryEvent, Utility.mstSyncBatchSize.OperatoryDayOff);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strOperatoryEvent = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(),Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    var JsonOperatoryEvent = new System.Text.StringBuilder();
                                    foreach (DataRow dtOperatoryEventRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        PushOperatoryEventRecord = PushOperatoryEventRecord + 1;

                                        TmpAppt_DateTime = dtOperatoryEventRow["StartTime"].ToString().Trim();
                                        if (TmpAppt_DateTime == string.Empty)
                                        {
                                            TmpAppt_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpAppt_DateTime = Convert.ToDateTime(TmpAppt_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpAppt_EndDateTime = dtOperatoryEventRow["EndTime"].ToString().Trim();
                                        if (TmpAppt_EndDateTime == string.Empty)
                                        {
                                            TmpAppt_EndDateTime = TmpAppt_DateTime;
                                        }
                                        else
                                        {
                                            TmpAppt_EndDateTime = Convert.ToDateTime(TmpAppt_EndDateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }

                                        TmpEntry_DateTime = dtOperatoryEventRow["Entry_DateTime"].ToString().Trim();
                                        if (TmpEntry_DateTime == string.Empty)
                                        {
                                            TmpEntry_DateTime = DateTime.Now.ToString(Utility.ApplicationDatetimeFormat);
                                        }
                                        else
                                        {
                                            TmpEntry_DateTime = Convert.ToDateTime(TmpEntry_DateTime).ToString(Utility.ApplicationDatetimeFormat);
                                        }


                                        Opt_Web_ID = string.Empty;
                                        DataRow[] rowOpt = dtLocalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtOperatoryEventRow["Operatory_EHR_ID"].ToString().Trim() + "' ");
                                        if (rowOpt.Length > 0)
                                        {
                                            Opt_Web_ID = rowOpt[0]["Operatory_Web_ID"].ToString().Trim();
                                        }
                                        string[] Opt_Web_IDs;
                                        if (Opt_Web_ID == string.Empty)
                                        {
                                            Opt_Web_IDs = new string[0];
                                        }
                                        else
                                        {
                                            Opt_Web_IDs = Opt_Web_ID.ToString().Trim().Split(';');
                                        }

                                        prov_Web_ID = string.Empty;
                                        DataRow[] rowprov = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtOperatoryEventRow["Provider_EHR_ID"].ToString().Trim() + "' ");
                                        if (rowprov.Length > 0)
                                        {
                                            prov_Web_ID = rowprov[0]["Provider_Web_ID"].ToString().Trim();
                                        }
                                        string[] Prov_Web_IDs;
                                        if (prov_Web_ID == string.Empty)
                                        {
                                            Prov_Web_IDs = new string[0];
                                        }
                                        else
                                        {
                                            Prov_Web_IDs = prov_Web_ID.ToString().Trim().Split(';');
                                        }

                                        Push_OperatoryEventBO OperatoryEvent = new Push_OperatoryEventBO
                                        {
                                            Appt_Web_ID = dtOperatoryEventRow["OE_Web_ID"].ToString().Trim(),
                                            Location_ID = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.Location_ID,
                                            Operatory_Name = Opt_Web_IDs,
                                            comment = dtOperatoryEventRow["comment"].ToString().Trim(),
                                            Appt_DateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpAppt_DateTime),
                                            Appt_EndDateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpAppt_EndDateTime),
                                            Entry_DateTime = Utility.ConvertDatetimeToUTCaditFormat(TmpEntry_DateTime),
                                            Organization_ID = Utility.Organization_ID,
                                            appt_ehr_id = dtOperatoryEventRow["OE_EHR_ID"].ToString(),
                                            appt_localdb_id = dtOperatoryEventRow["OE_LocalDB_ID"].ToString(),
                                            Appointment_Status = "Block",
                                            created_by = Utility.User_ID,
                                            is_deleted = Convert.ToBoolean(dtOperatoryEventRow["is_deleted"].ToString()),
                                            ParentLocation_ID = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(),   //Utility.Loc_ID
                                            Provider_Name= Prov_Web_IDs
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonOperatoryEvent.Append(javaScriptSerializer.Serialize(OperatoryEvent) + ",");
                                    }
                                    ////var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    ////string jsonString = javaScriptSerializer.Serialize(Appointment);
                                    ////string staAppointmenttype = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase(jsonString, "appointment", dtAppointmentRow["Appt_EHR_ID"].ToString(), dtAppointmentRow["Appt_Web_ID"].ToString());
                                    if (JsonOperatoryEvent.Length > 0)
                                    {
                                        string jsonString = "[" + JsonOperatoryEvent.ToString().Remove(JsonOperatoryEvent.Length - 1) + "]";
                                        strOperatoryEvent = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_OperatoryDayOff(jsonString, "operatorydayoff", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strOperatoryEvent.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[OperatoryDayoff Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strOperatoryEvent);
                                        }
                                    }
                                    else
                                    {
                                        strOperatoryEvent = "Success";
                                    }
                                }

                                if (strOperatoryEvent.ToLower() == "Success".ToLower())
                                {
                                    IsSynchOperatoryEvent = true;
                                }
                                else
                                {
                                    if (strOperatoryEvent.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchOperatoryEvent = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[OperatoryDayoff Sync (Local Database To Adit Server) ] : " + strOperatoryEvent);
                                        IsSynchOperatoryEvent = false;
                                    }
                                }
                            }
                        }
                    }
                    if (IsSynchOperatoryEvent)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryDayoff_Push");
                        GoalBase.WriteToSyncLogFile_Static("OperatoryDayoff Sync (Local Database To Adit Server) Successfully.");
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[OperatoryEvent  Sync (Local Database To Adit Server) ] : " + ex.Message);
                }
            }
        }


        #endregion

        #region Provider

        public static void SynchDataLiveDB_Push_Provider()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchProvider = true;
                // DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData();
                DataTable dtLocalProvider = SynchLocalBAL.GetPushLocalProviderData();

                if (dtLocalProvider.Rows.Count > 0)
                {
                    //https://app.asana.com/0/751059797849097/1148870004681899
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalProvider, 200);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalProvider, Utility.mstSyncBatchSize.Provider);
                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strProvider = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                string proGender = "male";
                                var JsonProvider = new System.Text.StringBuilder();
                                foreach (DataRow dtProviderRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"] + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"] + "' "))
                                {
                                    proGender = dtProviderRow["gender"].ToString().Trim();
                                    if (proGender == string.Empty || proGender.ToLower().ToString() != "female")
                                    {
                                        proGender = "male";
                                    }

                                    string[] tmpaccredation;
                                    if (dtProviderRow["accreditation"].ToString().Trim() == string.Empty)
                                    {
                                        tmpaccredation = new string[0];
                                    }
                                    else
                                    {
                                        tmpaccredation = dtProviderRow["accreditation"].ToString().Trim().ToString().Trim().Split(';');
                                        //accredation = new string[] { dtProviderRow["accreditation"].ToString().Trim() },
                                    }

                                    string[] tmpmembership;
                                    if (dtProviderRow["membership"].ToString().Trim() == string.Empty)
                                    {
                                        tmpmembership = new string[0];
                                    }
                                    else
                                    {
                                        tmpmembership = dtProviderRow["membership"].ToString().Trim().ToString().Trim().Split(';');
                                        //accredation = new string[] { dtProviderRow["accreditation"].ToString().Trim() },
                                    }

                                    string[] tmplanguages;
                                    if (dtProviderRow["language"].ToString().Trim() == string.Empty)
                                    {
                                        tmplanguages = new string[0];
                                    }
                                    else
                                    {
                                        tmplanguages = dtProviderRow["language"].ToString().Trim().ToString().Trim().Split(';');
                                        // languages = new string[] { dtProviderRow["language"].ToString().Trim() },
                                    }

                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                                    Push_ProvidersBO Providers = new Push_ProvidersBO
                                    {
                                        display_name = dtProviderRow["First_Name"].ToString().Trim() + " " + dtProviderRow["Last_Name"].ToString().Trim(),
                                        first_name = dtProviderRow["First_Name"].ToString().Trim(),
                                        last_name = dtProviderRow["Last_Name"].ToString().Trim(),
                                        gender = proGender,
                                        locations = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                                        accredation = tmpaccredation,
                                        membership = tmpmembership,
                                        languages = tmplanguages,
                                        bio = dtProviderRow["bio"].ToString().Trim(),
                                        specialities = new List<Pozative.BO.Push_SpecialitiesListAry>() { new Pozative.BO.Push_SpecialitiesListAry() { name = dtProviderRow["provider_speciality"].ToString().Trim() } },
                                        treatment_min_age = dtProviderRow["age_treated_min"].ToString().Trim(),
                                        treatment_max_age = dtProviderRow["age_treated_max"].ToString().Trim(),
                                        organization = Utility.Organization_ID,
                                        provider_ehr_id = dtProviderRow["Provider_EHR_ID"].ToString(),
                                        provider_localdb_id = dtProviderRow["Provider_LocalDB_ID"].ToString(),
                                        Provider_Web_ID = dtProviderRow["Provider_Web_ID"].ToString(),
                                        created_by = Utility.User_ID,
                                        is_active = Convert.ToBoolean(dtProviderRow["is_active"])
                                    };
                                    JsonProvider.Append(javaScriptSerializer.Serialize(Providers) + ",");
                                }
                                if (JsonProvider.Length > 0)
                                {
                                    string jsonString = "[" + JsonProvider.ToString().Remove(JsonProvider.Length - 1) + "]";
                                    //string strProvider = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "provider");
                                    strProvider = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Provider(jsonString, "provider", Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strProvider.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Providers Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strProvider);
                                    }
                                }
                                else
                                {
                                    strProvider = "Success";
                                }
                            }

                            if (strProvider.ToLower() == "Success".ToLower())
                            {
                                IsSynchProvider = true;
                            }
                            else
                            {
                                if (strProvider.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchProvider = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Providers Sync (Local Database To Adit Server) ] : " + strProvider);
                                    IsSynchProvider = false;
                                }
                            }
                        }
                    }
                }
                if (IsSynchProvider)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider_Push");
                    GoalBase.WriteToSyncLogFile_Static("Providers Sync (Local Database To Adit Server) Successfully.");
                    IsProviderSyncPush = true;
                }
                else
                {
                    IsProviderSyncPush = false;
                }
                //}
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Providers Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region ProviderHours

        public static void SynchDataLiveDB_Push_ProviderHours()
        {
            try
            {
                //if (Utility.is_scheduledCustomhour)
                {
                    bool IsSynchProviderHours = true;
                    //Utility.WriteToSyncLogFile_All("Provider_Custom Hours Get records from Local");
                    DataTable dtLocalProviderHours = SynchLocalBAL.GetPushLocalProviderHoursData();
                    //Utility.WriteToSyncLogFile_All("Provider_Custom Hours Finish records from Local");
                    if (dtLocalProviderHours.Rows.Count > 0)
                    {
                        //Utility.WriteToSyncLogFile_All("Provider_Custom Hours Total Count to send to Adit app " + dtLocalProviderHours.Rows.Count.ToString());
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalProviderHours, Utility.ProviderHourPushCounter);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalProviderHours, Utility.mstSyncBatchSize.ProviderHours);
                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {

                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    var JsonProvider = new System.Text.StringBuilder();
                                    foreach (DataRow dtProviderHoursRow in splitdt[i].Select(" IS_Deleted = 1 AND Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        if (Convert.ToBoolean(dtProviderHoursRow["is_deleted"].ToString()) == true)
                                        {
                                            Push_ProviderHoursBO ProviderHours = new Push_ProviderHoursBO
                                            {
                                                ph_localdb_id = dtProviderHoursRow["PH_LocalDB_ID"].ToString().Trim(),
                                                ph_ehr_id = dtProviderHoursRow["PH_EHR_ID"].ToString().Trim(),
                                                ph_web_id = dtProviderHoursRow["PH_Web_ID"].ToString().Trim(),
                                                provider_ehr_id = dtProviderHoursRow["Provider_EHR_ID"].ToString().Trim(),
                                                operatory_ehr_id = dtProviderHoursRow["Operatory_EHR_ID"].ToString().Trim(),
                                                starttime = Convert.ToDateTime(dtProviderHoursRow["StartTime"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss"),  //as per bharatbhai UTC me convert nahi karna he
                                                endtime = Convert.ToDateTime(dtProviderHoursRow["EndTime"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss"),      //as per bharatbhai UTC me convert nahi karna he
                                                comment = dtProviderHoursRow["comment"].ToString(),
                                                is_deleted = Convert.ToBoolean(dtProviderHoursRow["is_deleted"].ToString()),

                                                organization = Utility.Organization_ID,
                                                location = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                                                created_by = Utility.User_ID
                                            };
                                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                            JsonProvider.Append(javaScriptSerializer.Serialize(ProviderHours) + ",");
                                        }
                                    }

                                    if (JsonProvider.Length > 1)
                                    {
                                        string jsonString = "[" + JsonProvider.ToString().Remove(JsonProvider.Length - 1) + "]";
                                        //Utility.WriteToSyncLogFile_All("Provider_Custom Hours Call API to push the reocrds");

                                        string strProvider = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_ProviderHours(jsonString, "providerhours", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strProvider.ToLower() == "Success".ToLower())
                                        {
                                            IsSynchProviderHours = true;
                                        }
                                        else
                                        {
                                            if (strProvider.Contains("The remote name could not be resolved:"))
                                            {
                                                IsSynchProviderHours = false;
                                            }
                                            else
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[ProviderHours Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  : " + strProvider);
                                                IsSynchProviderHours = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    var JsonProvider = new System.Text.StringBuilder();
                                    foreach (DataRow dtProviderHoursRow in splitdt[i].Select(" IS_Deleted = 0 AND Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        if (Convert.ToBoolean(dtProviderHoursRow["is_deleted"].ToString()) == false)
                                        {
                                            Push_ProviderHoursBO ProviderHours = new Push_ProviderHoursBO
                                            {
                                                ph_localdb_id = dtProviderHoursRow["PH_LocalDB_ID"].ToString().Trim(),
                                                ph_ehr_id = dtProviderHoursRow["PH_EHR_ID"].ToString().Trim(),
                                                ph_web_id = dtProviderHoursRow["PH_Web_ID"].ToString().Trim(),
                                                provider_ehr_id = dtProviderHoursRow["Provider_EHR_ID"].ToString().Trim(),
                                                operatory_ehr_id = dtProviderHoursRow["Operatory_EHR_ID"].ToString().Trim(),
                                                starttime = Convert.ToDateTime(dtProviderHoursRow["StartTime"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss"),  //as per bharatbhai UTC me convert nahi karna he
                                                endtime = Convert.ToDateTime(dtProviderHoursRow["EndTime"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss"),      //as per bharatbhai UTC me convert nahi karna he
                                                comment = dtProviderHoursRow["comment"].ToString(),
                                                is_deleted = Convert.ToBoolean(dtProviderHoursRow["is_deleted"].ToString()),

                                                organization = Utility.Organization_ID,
                                                location = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.GetLocationIdByClinicNumber(dtProviderHoursRow["Clinic_Number"].ToString()),
                                                created_by = Utility.User_ID
                                            };
                                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                            JsonProvider.Append(javaScriptSerializer.Serialize(ProviderHours) + ",");
                                        }
                                    }

                                    if (JsonProvider.Length > 1)
                                    {
                                        string jsonString = "[" + JsonProvider.ToString().Remove(JsonProvider.Length - 1) + "]";
                                        string strProvider = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_ProviderHours(jsonString, "providerhours", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strProvider.ToLower() == "Success".ToLower())
                                        {
                                            IsSynchProviderHours = true;
                                        }
                                        else
                                        {
                                            if (strProvider.Contains("The remote name could not be resolved:"))
                                            {
                                                IsSynchProviderHours = false;
                                            }
                                            else
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[ProviderHours Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strProvider);
                                                IsSynchProviderHours = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (IsSynchProviderHours)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours_Push");
                        GoalBase.WriteToSyncLogFile_Static("ProviderHours Sync (Local Database To Adit Server) Successfully.");
                        IsProviderSyncPush = true;
                    }
                    else
                    {
                        IsProviderSyncPush = false;
                    }
                }

            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[ProviderHours Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region Speciality

        public static void SynchDataLiveDB_Push_Speciality()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                var JsonSpeciality = new System.Text.StringBuilder();
                bool IsSynchSpeciality = true;
                //DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData();
                DataTable dtLocalSpeciality = SynchLocalBAL.GetPushLocalSpecialityData();

                if (dtLocalSpeciality.Rows.Count > 0)
                {
                    string[] Speciality_Name;
                    string strSpe_Name = "";
                    foreach (DataRow dtSpecialityRow in dtLocalSpeciality.Rows)
                    {
                        strSpe_Name = strSpe_Name + dtSpecialityRow["Speciality_Name"].ToString().Replace(",", "").Trim() + ";";
                    }
                    Speciality_Name = strSpe_Name.ToString().Remove(strSpe_Name.Length - 1).Trim().Split(';');

                    ////
                    Push_SpecialityBO Speciality = new Push_SpecialityBO
                    {
                        name = Speciality_Name,
                        organization = Utility.Organization_ID,
                        created_by = Utility.User_ID
                    };
                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonSpeciality.Append(javaScriptSerializer.Serialize(Speciality) + ",");
                    string jsonString = JsonSpeciality.ToString().Remove(JsonSpeciality.Length - 1);

                    //string strSpeciality = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "speciality");
                    string strSpeciality = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Speciality(jsonString, "speciality", Utility.Location_ID);
                    if (strSpeciality.ToLower() == "Success".ToLower())
                    {
                        IsSynchSpeciality = true;
                    }
                    else
                    {
                        if (strSpeciality.Contains("The remote name could not be resolved:"))
                        {
                            IsSynchSpeciality = false;
                        }
                        else
                        {
                            GoalBase.WriteToErrorLogFile_Static("[Speciality Sync (Local Database To Adit Server) ] : " + strSpeciality);
                            IsSynchSpeciality = false;
                        }
                    }
                }
                if (IsSynchSpeciality)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality_Push");
                    GoalBase.WriteToSyncLogFile_Static("Speciality Sync (Local Database To Adit Server) Successfully.");
                }
                //}
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Speciality  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region Operatory

        public static void SynchDataLiveDB_Push_Operatory()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchOperatory = true;
                // DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();
                DataTable dtLocalOperatory = SynchLocalBAL.GetPushLocalOperatoryData();

                if (dtLocalOperatory.Rows.Count > 0)
                {
                    string strOperatory = "";
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                        {
                            var JsonOperatory = new System.Text.StringBuilder();
                            foreach (DataRow dtOperatoryRow in dtLocalOperatory.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                            {
                                Push_OperatoryBO Operatory = new Push_OperatoryBO
                                {
                                    appointmentlocation = Utility.DtLocationList.Rows[i]["Location_Id"].ToString(),//Utility.GetLocationIdByClinicNumber(dtOperatoryRow["Clinic_Number"].ToString()),
                                    name = dtOperatoryRow["Operatory_Name"].ToString().Replace(",", "").Trim(),
                                    organization = Utility.Organization_ID,
                                    operatory_ehr_id = dtOperatoryRow["Operatory_EHR_ID"].ToString().Trim(),
                                    operatory_localdb_id = dtOperatoryRow["Operatory_LocalDB_ID"].ToString().Trim(),
                                    operatory_Web_ID = dtOperatoryRow["Operatory_Web_ID"].ToString().Trim(),
                                    is_deleted = Convert.ToBoolean(dtOperatoryRow["is_deleted"].ToString().Trim()),
                                    created_by = Utility.User_ID,
                                    sort_order = Convert.ToInt16(dtOperatoryRow["OperatoryOrder"].ToString().Trim())
                                };
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                JsonOperatory.Append(javaScriptSerializer.Serialize(Operatory) + ",");
                            }

                            if (JsonOperatory.Length > 0)
                            {
                                string jsonString = "[" + JsonOperatory.ToString().Remove(JsonOperatory.Length - 1) + "]";
                                //string strOperatory = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "operatory");
                                strOperatory = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Operatory(jsonString, "operatory", Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_Id"].ToString());

                                if (strOperatory.ToLower() != "Success".ToLower())
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Operatory Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "  And  Clinic : " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strOperatory);
                                }
                            }
                            else
                            {
                                strOperatory = "Success";
                            }
                        }

                        if (strOperatory.ToLower() == "Success".ToLower())
                        {
                            IsSynchOperatory = true;
                        }
                        else
                        {
                            if (strOperatory.Contains("The remote name could not be resolved:"))
                            {
                                IsSynchOperatory = false;
                            }
                            else
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Operatory Sync (Local Database To Adit Server) ] : " + strOperatory);
                                IsSynchOperatory = false;
                            }
                        }
                    }
                }
                if (IsSynchOperatory)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory_Push");
                    GoalBase.WriteToSyncLogFile_Static("Operatory Sync (Local Database To Adit Server) Successfully.");
                    IsOperatorySyncPush = true;
                }
                else
                {
                    IsOperatorySyncPush = false;
                }
                //}
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Operatory  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion
        #region FolderList

        public static void SynchDataLiveDB_Push_FolderList()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchFolderList = true;
                // DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();
                DataTable dtLocalFolderList = SynchLocalBAL.GetPushLocalFolderListData();

                if (dtLocalFolderList.Rows.Count > 0)
                {
                    string strFolderList = "";
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                        {
                            var JsonFolderList = new System.Text.StringBuilder();
                            foreach (DataRow dtFolderListRow in dtLocalFolderList.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                            {
                                Push_FolderListBO FolderList = new Push_FolderListBO
                                {
                                    folder_name = dtFolderListRow["Folder_Name"].ToString().Replace(",", "").Trim(),
                                    is_active = Convert.ToBoolean(dtFolderListRow["is_deleted"]) ? false : true,
                                    is_deleted = Convert.ToBoolean(dtFolderListRow["is_deleted"]),
                                    description = dtFolderListRow["Folder_Name"].ToString().Replace(",", "").Trim(),
                                    ehrfolder_ehr_id = dtFolderListRow["FolderList_EHR_ID"].ToString().Trim(),
                                    appointmentlocation = Utility.DtLocationList.Rows[i]["Location_Id"].ToString(),//Utility.GetLocationIdByClinicNumber(dtOperatoryRow["Clinic_Number"].ToString()),
                                    location = Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(),//Utility.GetLocationIdByClinicNumber(dtOperatoryRow["Clinic_Number"].ToString()),
                                    organization = Utility.Organization_ID,
                                    order_id = Convert.ToInt32(dtFolderListRow["FolderOrder"])
                                };
                                if (dtFolderListRow["FolderList_Web_ID"].ToString().Trim() != "")
                                {
                                    FolderList._id = dtFolderListRow["FolderList_Web_ID"].ToString();
                                }
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                JsonFolderList.Append(javaScriptSerializer.Serialize(FolderList) + ",");
                            }

                            if (JsonFolderList.Length > 0)
                            {
                                string jsonString = "[" + JsonFolderList.ToString().Remove(JsonFolderList.Length - 1) + "]";
                                //string strOperatory = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "operatory");
                                strFolderList = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_FolderList(jsonString, "FolderList", Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_Id"].ToString());

                                if (strFolderList.ToLower() != "Success".ToLower())
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[FolderList Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "  And  Clinic : " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strFolderList);
                                }
                            }
                            else
                            {
                                strFolderList = "Success";
                            }
                        }

                        if (strFolderList.ToLower() == "Success".ToLower())
                        {
                            IsSynchFolderList = true;
                        }
                        else
                        {
                            if (strFolderList.Contains("The remote name could not be resolved:"))
                            {
                                IsSynchFolderList = false;
                            }
                            else
                            {
                                GoalBase.WriteToErrorLogFile_Static("[FolderList Sync (Local Database To Adit Server) ] : " + strFolderList);
                                IsSynchFolderList = false;
                            }
                        }
                    }
                }
                if (IsSynchFolderList)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FolderList_Push");
                    GoalBase.WriteToSyncLogFile_Static("FolderList Sync (Local Database To Adit Server) Successfully.");
                    IsOperatorySyncPush = true;
                }
                else
                {
                    IsOperatorySyncPush = false;
                }
                //}
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[FolderList  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region OperatoryHours

        public static void SynchDataLiveDB_Push_OperatoryHours()
        {
            try
            {
                //if (Utility.is_scheduledCustomhour)
                {
                    bool IsSynchOperatoryHours = true;
                    DataTable dtLocalOperatoryHours = SynchLocalBAL.GetPushLocalOperatoryHoursData();

                    if (dtLocalOperatoryHours.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalOperatoryHours, 200);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalOperatoryHours, Utility.mstSyncBatchSize.OperatoryHours);
                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    string strOperatory = "";
                                    var JsonOperatory = new System.Text.StringBuilder();
                                    foreach (DataRow dtOperatoryHoursRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_OperatoryHoursBO OperatoryHours = new Push_OperatoryHoursBO
                                        {
                                            oh_localdb_id = dtOperatoryHoursRow["OH_LocalDB_ID"].ToString().Trim(),
                                            oh_ehr_id = dtOperatoryHoursRow["OH_EHR_ID"].ToString().Trim(),
                                            oh_web_id = dtOperatoryHoursRow["OH_Web_ID"].ToString().Trim(),
                                            operatory_ehr_id = dtOperatoryHoursRow["Operatory_EHR_ID"].ToString().Trim(),
                                            starttime = Convert.ToDateTime(dtOperatoryHoursRow["StartTime"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss"),  //as per bharatbhai UTC me convert nahi karna he
                                            endtime = Convert.ToDateTime(dtOperatoryHoursRow["EndTime"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss"),      //as per bharatbhai UTC me convert nahi karna he
                                            comment = dtOperatoryHoursRow["comment"].ToString(),
                                            is_deleted = Convert.ToBoolean(dtOperatoryHoursRow["is_deleted"].ToString()),

                                            organization = Utility.Organization_ID,
                                            location = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.GetLocationIdByClinicNumber(dtProviderHoursRow["Clinic_Number"].ToString()),
                                            created_by = Utility.User_ID
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonOperatory.Append(javaScriptSerializer.Serialize(OperatoryHours) + ",");
                                    }

                                    if (JsonOperatory.Length > 0)
                                    {
                                        string jsonString = "[" + JsonOperatory.ToString().Remove(JsonOperatory.Length - 1) + "]";
                                        strOperatory = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_OperatoryHours(jsonString, "operatoryhours", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strOperatory.ToLower() == "Success".ToLower())
                                        {
                                            IsSynchOperatoryHours = true;
                                        }
                                        else
                                        {
                                            if (strOperatory.Contains("The remote name could not be resolved:"))
                                            {
                                                IsSynchOperatoryHours = false;
                                            }
                                            else
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[OperatoryHours Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strOperatory);
                                                IsSynchOperatoryHours = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (IsSynchOperatoryHours)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryHours_Push");
                        GoalBase.WriteToSyncLogFile_Static("OperatoryHours Sync (Local Database To Adit Server) Successfully.");
                        IsOperatorySyncPush = true;
                    }
                    else
                    {
                        IsOperatorySyncPush = false;
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[OperatoryHours Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        public static void SynchDataLiveDB_Push_OperatoryOfficeHours()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchOperatoryHours = true;
                DataTable dtLocalOperatoryOfficeHours = SynchLocalBAL.GetPushLocalOperatoryOfficeHoursData();

                if (dtLocalOperatoryOfficeHours.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalOperatoryOfficeHours, 200);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalOperatoryOfficeHours, Utility.mstSyncBatchSize.OperatoryOfficeHours);
                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonOperatory = new System.Text.StringBuilder();
                                foreach (DataRow dtOperatoryOfficeHoursRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                {
                                    Push_OperatoryOfficeHoursBO OperatoryHours = new Push_OperatoryOfficeHoursBO
                                    {
                                        OOH_Localdb_Id = dtOperatoryOfficeHoursRow["OOH_LocalDB_ID"].ToString().Trim(),
                                        OOH_EHR_ID = dtOperatoryOfficeHoursRow["OOH_EHR_ID"].ToString().Trim(),
                                        OOH_Web_ID = dtOperatoryOfficeHoursRow["OOH_Web_ID"].ToString().Trim(),
                                        Operatory_EHR_Id = dtOperatoryOfficeHoursRow["Operatory_EHR_ID"].ToString().Trim(),
                                        WeekDay = dtOperatoryOfficeHoursRow["WeekDay"].ToString(),
                                        StartTime1 = Convert.ToDateTime(dtOperatoryOfficeHoursRow["StartTime1"]).ToString("HH:mm").ToString().Trim(),
                                        EndTime1 = Convert.ToDateTime(dtOperatoryOfficeHoursRow["EndTime1"]).ToString("HH:mm").ToString().Trim(),
                                        StartTime2 = Convert.ToDateTime(dtOperatoryOfficeHoursRow["StartTime2"]).ToString("HH:mm").ToString().Trim(),
                                        EndTime2 = Convert.ToDateTime(dtOperatoryOfficeHoursRow["EndTime2"]).ToString("HH:mm").ToString().Trim(),
                                        StartTime3 = Convert.ToDateTime(dtOperatoryOfficeHoursRow["StartTime3"]).ToString("HH:mm").ToString().Trim(),
                                        EndTime3 = Convert.ToDateTime(dtOperatoryOfficeHoursRow["EndTime3"]).ToString("HH:mm").ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtOperatoryOfficeHoursRow["is_deleted"].ToString()),

                                        Organization_ID = Utility.Organization_ID,
                                        Location_ID = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.Location_ID,
                                        created_by = Utility.User_ID
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonOperatory.Append(javaScriptSerializer.Serialize(OperatoryHours) + ",");
                                }
                                if (JsonOperatory.Length > 0)
                                {
                                    string jsonString = "[" + JsonOperatory.ToString().Remove(JsonOperatory.Length - 1) + "]";
                                    string strOperatory = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_OperatoryOfficeHours(jsonString, "operatoryofficehours", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strOperatory.ToLower() == "Success".ToLower())
                                    {
                                        IsSynchOperatoryHours = true;
                                    }
                                    else
                                    {
                                        if (strOperatory.Contains("The remote name could not be resolved:"))
                                        {
                                            IsSynchOperatoryHours = false;
                                        }
                                        else
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[OperatoryOfficeHours Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strOperatory);
                                            IsSynchOperatoryHours = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (IsSynchOperatoryHours)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryOfficeHours_Push");
                    GoalBase.WriteToSyncLogFile_Static("OperatoryOfficeHours Sync (Local Database To Adit Server) Successfully.");
                    IsOperatorySyncPush = true;
                }
                else
                {
                    IsOperatorySyncPush = false;
                }
                //}
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[OperatoryOfficeHours Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region Appointment Type

        public static void SynchDataLiveDB_Push_ApptType()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                var JsonAppointmentType = new System.Text.StringBuilder();
                bool IsSynchAppointmentType = true;
                //DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData();
                DataTable dtLocalApptType = SynchLocalBAL.GetPushLocalApptTypeData();
                if (dtLocalApptType.Rows.Count > 0)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                        {
                            string strAppointmentType = "";
                            foreach (DataRow dtApptTypeRow in dtLocalApptType.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                            {
                                AppointmentTypeBO AppointmentType = new AppointmentTypeBO
                                {
                                    organization = Utility.Organization_ID,
                                    appointmentlocation = Utility.DtLocationList.Rows[i]["Location_Id"].ToString(),//Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                                    name = dtApptTypeRow["Type_Name"].ToString().Trim().Replace(",", "").Trim(),
                                    is_new = false,
                                    apptype_ehr_id = dtApptTypeRow["ApptType_EHR_ID"].ToString().Trim(),
                                    apptype_localdb_id = dtApptTypeRow["ApptType_LocalDB_ID"].ToString().Trim(),
                                    ApptType_Web_ID = dtApptTypeRow["ApptType_Web_ID"].ToString().Trim(),
                                    created_by = Utility.User_ID,
                                    is_deleted = Convert.ToBoolean(dtApptTypeRow["is_deleted"])

                                };
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                JsonAppointmentType.Append(javaScriptSerializer.Serialize(AppointmentType) + ",");
                            }

                            if (JsonAppointmentType.Length > 0)
                            {
                                string jsonString = "[" + JsonAppointmentType.ToString().Remove(JsonAppointmentType.Length - 1) + "]";
                                //string strAppointmentType = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "Type");
                                strAppointmentType = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_ApptType(jsonString, "Type", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_Id"].ToString());

                                if (strAppointmentType.ToLower() == "Success".ToLower())
                                {
                                    IsSynchAppointmentType = true;
                                }
                                else
                                {
                                    if (strAppointmentType.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchAppointmentType = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Appointment Type Sync (Local Database To Adit Server) ] Service Install Id  : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "  And Clinic " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strAppointmentType);
                                        IsSynchAppointmentType = false;
                                    }
                                }
                            }
                        }
                    }
                }

                if (IsSynchAppointmentType)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType_Push");
                    GoalBase.WriteToSyncLogFile_Static("Appointment Type Sync (Local Database To Adit Server) Successfully.");
                    IsApptTypeSyncPush = true;
                }
                else
                {
                    IsApptTypeSyncPush = false;
                }
                //}
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Appointment Type Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region Patient

        public static void SynchDataLiveDB_Push_Patient(string strPatID = "")
        {

            //if (!IsApptSyncingRunning)
            //{
            //    IsPatientSyncingRunning = true;
            //    IsPatientSyncingInQueue = false;
            try
            {
                //IsParientFirstPushSync = false;
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchPatient = true;
                //DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData();
                DataTable dtLocalPatient = SynchLocalBAL.GetPushLocalPatientData(strPatID);

                //https://app.asana.com/0/751059797849097/1148328937003503
                //dtLocalPatient = Utility.CreateDistinctRecords(dtLocalPatient, "Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID", "Patient_EHR_Id");

                if (dtLocalPatient.Rows.Count > 0)
                {
                    string tmpFirstVisit_Date = string.Empty;
                    string tmpLastVisit_Date = string.Empty;
                    string nextvisit_date = string.Empty;
                    string due_date = string.Empty;
                    string tmpBirth_Date = string.Empty;
                    string respBirth_Date = string.Empty;
                    string arytmpdue_dates = string.Empty;
                    string arytmprecall_type = string.Empty;
                    string arytmprecall_typeId = string.Empty;
                    string driverlicense = string.Empty;

                    int totPatient = dtLocalPatient.Rows.Count;
                    int cntPatient = 0;

                    PushPatientRecord = 0;
                    TotalPushPatientRecord = dtLocalPatient.Rows.Count;
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, patientPushCounter);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, Utility.mstSyncBatchSize.Patient);
                    Utility.WriteToSyncLogFile_All("Patient Sync to Adit App Total Count is " + TotalPushPatientRecord.ToString());
                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        //https://app.asana.com/0/751059797849097/1148328937003503
                        splitdt[i] = Utility.CreateDistinctRecords(splitdt[i], "Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID", "Patient_EHR_Id");
                        string strPatient = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {

                            if (Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonPatient = new System.Text.StringBuilder();
                                foreach (DataRow dtPatientRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                {
                                    tmpFirstVisit_Date = string.Empty;
                                    tmpLastVisit_Date = string.Empty;
                                    nextvisit_date = string.Empty;
                                    due_date = string.Empty;
                                    tmpBirth_Date = string.Empty;
                                    respBirth_Date = string.Empty;
                                    arytmpdue_dates = string.Empty;
                                    arytmprecall_type = string.Empty;
                                    arytmprecall_typeId = string.Empty;
                                    driverlicense = string.Empty;

                                    PushPatientRecord = PushPatientRecord + 1;

                                    cntPatient = cntPatient + 1;
                                    tmpFirstVisit_Date = Utility.ConvertDatetimeToUTCaditFormat(dtPatientRow["FirstVisit_Date"].ToString().Trim());
                                    tmpLastVisit_Date = Utility.ConvertDatetimeToUTCaditFormat(dtPatientRow["LastVisit_Date"].ToString().Trim());

                                    nextvisit_date = Utility.ConvertDatetimeToUTCaditFormat(dtPatientRow["nextvisit_date"].ToString().Trim());
                                    due_date = Utility.ConvertDatetimeToUTCaditFormat(dtPatientRow["due_date"].ToString().Trim());

                                    try
                                    {
                                        if (dtPatientRow["Birth_Date"] != null && dtPatientRow["Birth_Date"].ToString() != string.Empty)
                                        {
                                            tmpBirth_Date = Convert.ToDateTime(dtPatientRow["Birth_Date"].ToString().Trim()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        tmpBirth_Date = "";
                                    }
                                    try
                                    {
                                        if (dtPatientRow["responsiblepartybirthdate"] != null && dtPatientRow["responsiblepartybirthdate"].ToString() != string.Empty)
                                        {
                                            respBirth_Date = Convert.ToDateTime(dtPatientRow["responsiblepartybirthdate"].ToString().Trim()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        respBirth_Date = "";
                                    }


                                    arytmpdue_dates = string.Empty;
                                    arytmprecall_type = string.Empty;
                                    arytmprecall_typeId = string.Empty;

                                    string[] arydue_date = dtPatientRow["due_date"].ToString().Trim().Split('|');

                                    foreach (string ada in arydue_date)
                                    {
                                        if (ada.Length > 0)
                                        {
                                            string[] ary_r_d = ada.ToString().Trim().Split('@');
                                            if (ary_r_d.Length > 0)
                                            {
                                                if (ary_r_d[0].ToString() == "")
                                                {
                                                    arytmpdue_dates = arytmpdue_dates + " " + ";";
                                                }
                                                else
                                                {
                                                    arytmpdue_dates = arytmpdue_dates + Utility.ConvertDatetimeToUTCaditFormat(ary_r_d[0].ToString()).ToString().Trim() + ";";
                                                }
                                            }
                                            if (ary_r_d.Length > 1)
                                            {
                                                if (ary_r_d[1].ToString() == "")
                                                {
                                                    arytmprecall_type = arytmprecall_type + " " + ";";
                                                }
                                                else
                                                {
                                                    arytmprecall_type = arytmprecall_type + ary_r_d[1].ToString() + ";";
                                                }
                                            }
                                            if (ary_r_d.Length > 2)
                                            {
                                                if (ary_r_d[2].ToString() == "")
                                                {
                                                    arytmprecall_typeId = arytmprecall_typeId + " " + ";";
                                                }
                                                else
                                                {
                                                    arytmprecall_typeId = arytmprecall_typeId + ary_r_d[2].ToString() + ";";
                                                }
                                            }
                                        }
                                    }

                                    string[] tmpdue_dates;
                                    if (arytmpdue_dates.Length > 0)
                                    {
                                        arytmpdue_dates = arytmpdue_dates.Substring(0, arytmpdue_dates.Length - 1);
                                        tmpdue_dates = arytmpdue_dates.ToString().Split(';');
                                    }
                                    else
                                    {
                                        tmpdue_dates = new string[0];
                                    }

                                    string[] tmptmprecall_type;
                                    if (arytmprecall_type.Length > 0)
                                    {
                                        arytmprecall_type = arytmprecall_type.Substring(0, arytmprecall_type.Length - 1);
                                        tmptmprecall_type = arytmprecall_type.ToString().Split(';');
                                    }
                                    else
                                    {
                                        tmptmprecall_type = new string[0];
                                    }

                                    string[] tmptmprecall_typeId;
                                    if (arytmprecall_typeId.Length > 0)
                                    {
                                        arytmprecall_typeId = arytmprecall_typeId.Substring(0, arytmprecall_typeId.Length - 1);
                                        tmptmprecall_typeId = arytmprecall_typeId.ToString().Split(';');
                                    }
                                    else
                                    {
                                        tmptmprecall_typeId = new string[0];
                                    }


                                    Push_PatientBO patient = new Push_PatientBO
                                    {
                                        organization = Utility.Organization_ID,
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(), //Utility.Loc_ID,
                                        created_by = Utility.User_ID,

                                        patient_localdb_id = dtPatientRow["Patient_LocalDB_ID"].ToString().Trim(),
                                        patient_ehr_id = dtPatientRow["Patient_EHR_ID"].ToString().Trim(),
                                        Patient_Web_ID = dtPatientRow["Patient_Web_ID"].ToString().Trim(),
                                        first_name = dtPatientRow["First_name"].ToString().Trim(),
                                        last_name = dtPatientRow["Last_name"].ToString().Trim(),
                                        middle_name = dtPatientRow["Middle_Name"].ToString().Trim(),
                                        salutation = dtPatientRow["Salutation"].ToString().Trim(),
                                        preferred_name = dtPatientRow["preferred_name"].ToString().Trim(),
                                        status = dtPatientRow["Status"].ToString().Trim(),
                                        sex = dtPatientRow["Sex"].ToString().Trim(),
                                        marital_status = dtPatientRow["MaritalStatus"].ToString().Trim(),
                                        birth_date = tmpBirth_Date,
                                        email = dtPatientRow["Email"].ToString().Trim(),
                                        mobile = dtPatientRow["Mobile"].ToString().Trim(),
                                        home_phone = dtPatientRow["Home_Phone"].ToString().Trim(),
                                        work_phone = dtPatientRow["Work_Phone"].ToString().Trim(),
                                        address_one = dtPatientRow["Address1"].ToString().Trim(),
                                        address_two = dtPatientRow["Address2"].ToString().Trim(),
                                        city = dtPatientRow["City"].ToString().Trim(),
                                        state = dtPatientRow["State"].ToString().Trim(),
                                        zipcode = dtPatientRow["Zipcode"].ToString().Trim(),
                                        responsibleparty_status = dtPatientRow["ResponsibleParty_Status"].ToString().Trim(),
                                        current_bal = dtPatientRow["CurrentBal"].ToString().Trim(),
                                        thirty_day = dtPatientRow["ThirtyDay"].ToString().Trim(),
                                        sixty_day = dtPatientRow["SixtyDay"].ToString().Trim(),
                                        ninety_day = dtPatientRow["NinetyDay"].ToString().Trim(),
                                        over_ninty = dtPatientRow["Over90"].ToString().Trim(),
                                        firstvisit_date = tmpFirstVisit_Date,
                                        lastvisit_date = tmpLastVisit_Date,
                                        primary_insurance = dtPatientRow["Primary_Insurance"].ToString().Trim(),
                                        primary_insurance_companyname = dtPatientRow["Primary_Insurance_CompanyName"].ToString().Trim(),
                                        primary_ins_subscriber_id = dtPatientRow["Primary_Ins_Subscriber_ID"].ToString().Trim(),
                                        secondary_insurance = dtPatientRow["Secondary_Insurance"].ToString().Trim(),
                                        secondary_insurance_companyname = dtPatientRow["Secondary_Insurance_CompanyName"].ToString().Trim(),
                                        secondary_ins_subscriber_id = dtPatientRow["Secondary_Ins_Subscriber_ID"].ToString().Trim(),
                                        secondary_insurancecompanyphonenumber= dtPatientRow["Sec_Ins_Company_Phonenumber"].ToString().Trim(),
                                        guar_id = dtPatientRow["Guar_ID"].ToString().Trim(),
                                        pri_provider_id = dtPatientRow["Pri_Provider_ID"].ToString().Trim(),
                                        sec_provider_id = dtPatientRow["Sec_Provider_ID"].ToString().Trim(),
                                        receive_sms = dtPatientRow["ReceiveSms"].ToString().Trim(),
                                        receive_email = dtPatientRow["ReceiveEmail"].ToString().Trim(),
                                        nextvisit_date = nextvisit_date,
                                        due_date = due_date,
                                        due_dates = tmpdue_dates,
                                        recall_type = tmptmprecall_type,
                                        ehr_key = tmptmprecall_typeId,
                                        used_benefit = dtPatientRow["used_benefit"].ToString().Trim(),
                                        remaining_benefit = dtPatientRow["remaining_benefit"].ToString().Trim(),
                                        collect_payment = dtPatientRow["collect_payment"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtPatientRow["is_deleted"]),
                                        ehr_status = dtPatientRow["EHR_Status"].ToString().Trim(),
                                        receive_voice_call = dtPatientRow["ReceiveVoiceCall"].ToString().Trim(),
                                        patient_status = dtPatientRow["Patient_status"].ToString().Trim().ToLower(),
                                        PreferredLanguage = dtPatientRow["PreferredLanguage"].ToString() != "" ? dtPatientRow["PreferredLanguage"].ToString().Trim().ToLower() : "english",
                                        patient_note = dtPatientRow["Patient_Note"].ToString().Trim(),

                                        ssn = (dtPatientRow["ssn"].ToString().Trim()),
                                        driverlicense = (dtPatientRow["driverlicense"].ToString().Trim()),
                                        groupid = dtPatientRow["groupid"].ToString().Trim(),
                                        insurancecompanyphonenumber = dtPatientRow["Prim_Ins_Company_Phonenumber"].ToString().Trim(),
                                        emergencycontactid = dtPatientRow["emergencycontactId"].ToString().Trim(),
                                        emergencycontactfirstname = dtPatientRow["EmergencyContact_First_Name"].ToString().Trim(),
                                        emergencycontactlastname = dtPatientRow["EmergencyContact_Last_Name"].ToString().Trim(),
                                        emergencycontactnumber = dtPatientRow["emergencycontactnumber"].ToString().Trim(),
                                        school = dtPatientRow["school"].ToString().Trim(),
                                        employer = dtPatientRow["employer"].ToString().Trim(),
                                        spouseid = dtPatientRow["spouseId"].ToString().Trim(),
                                        spousefirstname = dtPatientRow["Spouse_First_Name"].ToString().Trim(),
                                        spouselastname = dtPatientRow["Spouse_Last_Name"].ToString().Trim(),
                                        responsiblepartyid = dtPatientRow["responsiblepartyId"].ToString().Trim(),
                                        responsiblepartyfirstname = dtPatientRow["ResponsibleParty_First_Name"].ToString().Trim(),
                                        responsiblepartylastname = dtPatientRow["ResponsibleParty_Last_Name"].ToString().Trim(),
                                        responsiblepartyssn = (dtPatientRow["responsiblepartyssn"].ToString().Trim()),
                                        responsiblepartybirthdate = respBirth_Date.ToString().Trim()                                       
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonPatient.Append(javaScriptSerializer.Serialize(patient) + ",");
                                }

                                if (JsonPatient.Length > 0)
                                {
                                    string jsonString = "[" + JsonPatient.ToString().Remove(JsonPatient.Length - 1) + "]";
                                    //string strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "patient");
                                    strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Patient(jsonString, "patient", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strPatient.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Patient Sync (Local Database To Adit Server) ] Service Install Id " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  : " + strPatient);
                                    }
                                }
                                else
                                {
                                    strPatient = "Success";
                                }
                            }

                            if (strPatient.ToLower() == "Success".ToLower())
                            {
                                IsSynchPatient = true;
                            }
                            else
                            {
                                if (strPatient.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchPatient = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Patient Sync (Local Database To Adit Server) ] : " + strPatient);
                                    IsSynchPatient = false;
                                }
                            }
                        }
                    }
                }
                if (IsSynchPatient)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                    GoalBase.WriteToSyncLogFile_Static("Patient Sync (Local Database To Adit Server) Successfully.");
                }
                //  IsPatientSyncingRunning = false;
                //}
            }
            catch (Exception ex)
            {
                //IsPatientSyncingRunning = false;
                GoalBase.WriteToErrorLogFile_Static("[Patient Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
            //}
            //else
            //{
            //    IsPatientSyncingInQueue = true;
            //}
        }

        public static void SynchDataLiveDB_Push_PatientMultiLocation()
        {

            //if (!IsApptSyncingRunning)
            //{
            //    IsPatientSyncingRunning = true;
            //    IsPatientSyncingInQueue = false;
            try
            {
                //IsParientFirstPushSync = false;
                //if (Utility.AditLocationSyncEnable)
                //{              
                bool IsSynchPatient = true;
                //DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData();
                DataTable dtLocalPatient = SynchLocalBAL.GetPushLocalPatientData();

                //https://app.asana.com/0/751059797849097/1148328937003503
                //dtLocalPatient = Utility.CreateDistinctRecords(dtLocalPatient, "Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID", "Patient_EHR_Id");

                if (dtLocalPatient.Rows.Count > 0)
                {
                    string tmpFirstVisit_Date = string.Empty;
                    string tmpLastVisit_Date = string.Empty;
                    string nextvisit_date = string.Empty;
                    string due_date = string.Empty;
                    string tmpBirth_Date = string.Empty;
                    string respBirth_Date = string.Empty;
                    string arytmpdue_dates = string.Empty;
                    string arytmprecall_type = string.Empty;
                    string arytmprecall_typeId = string.Empty;
                    string driverlicense = string.Empty;

                    int totPatient = dtLocalPatient.Rows.Count;
                    int cntPatient = 0;

                    PushPatientRecord = 0;
                    TotalPushPatientRecord = dtLocalPatient.Rows.Count;
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, patientPushCounter);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, Utility.mstSyncBatchSize.Patient_Multi_Location);
                    Utility.WriteToSyncLogFile_All("Patient Sync to Adit App Total Count is " + TotalPushPatientRecord.ToString());
                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        //https://app.asana.com/0/751059797849097/1148328937003503
                        splitdt[i] = Utility.CreateDistinctRecords(splitdt[i], "Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID", "Patient_EHR_Id");
                        string strPatient = "";
                        string[] MultiLocation = Utility.DtLocationList.Copy().AsEnumerable().Select(r => r.Field<string>("Location_Id")).ToArray();
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {

                            if (Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonPatient = new System.Text.StringBuilder();
                                foreach (DataRow dtPatientRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                {
                                    tmpFirstVisit_Date = string.Empty;
                                    tmpLastVisit_Date = string.Empty;
                                    nextvisit_date = string.Empty;
                                    due_date = string.Empty;
                                    tmpBirth_Date = string.Empty;
                                    respBirth_Date = string.Empty;
                                    arytmpdue_dates = string.Empty;
                                    arytmprecall_type = string.Empty;
                                    arytmprecall_typeId = string.Empty;
                                    driverlicense = string.Empty;

                                    PushPatientRecord = PushPatientRecord + 1;

                                    cntPatient = cntPatient + 1;
                                    tmpFirstVisit_Date = Utility.ConvertDatetimeToUTCaditFormat(dtPatientRow["FirstVisit_Date"].ToString().Trim());
                                    tmpLastVisit_Date = Utility.ConvertDatetimeToUTCaditFormat(dtPatientRow["LastVisit_Date"].ToString().Trim());

                                    nextvisit_date = Utility.ConvertDatetimeToUTCaditFormat(dtPatientRow["nextvisit_date"].ToString().Trim());
                                    due_date = Utility.ConvertDatetimeToUTCaditFormat(dtPatientRow["due_date"].ToString().Trim());

                                    try
                                    {
                                        if (dtPatientRow["Birth_Date"] != null && dtPatientRow["Birth_Date"].ToString() != string.Empty)
                                        {
                                            tmpBirth_Date = Convert.ToDateTime(dtPatientRow["Birth_Date"].ToString().Trim()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        tmpBirth_Date = "";
                                    }
                                    try
                                    {
                                        if (dtPatientRow["responsiblepartybirthdate"] != null && dtPatientRow["responsiblepartybirthdate"].ToString() != string.Empty)
                                        {
                                            respBirth_Date = Convert.ToDateTime(dtPatientRow["responsiblepartybirthdate"].ToString().Trim()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        respBirth_Date = "";
                                    }


                                    arytmpdue_dates = string.Empty;
                                    arytmprecall_type = string.Empty;
                                    arytmprecall_typeId = string.Empty;

                                    string[] arydue_date = dtPatientRow["due_date"].ToString().Trim().Split('|');

                                    foreach (string ada in arydue_date)
                                    {
                                        if (ada.Length > 0)
                                        {
                                            string[] ary_r_d = ada.ToString().Trim().Split('@');
                                            if (ary_r_d.Length > 0)
                                            {
                                                if (ary_r_d[0].ToString() == "")
                                                {
                                                    arytmpdue_dates = arytmpdue_dates + " " + ";";
                                                }
                                                else
                                                {
                                                    arytmpdue_dates = arytmpdue_dates + Utility.ConvertDatetimeToUTCaditFormat(ary_r_d[0].ToString()).ToString().Trim() + ";";
                                                }
                                            }
                                            if (ary_r_d.Length > 1)
                                            {
                                                if (ary_r_d[1].ToString() == "")
                                                {
                                                    arytmprecall_type = arytmprecall_type + " " + ";";
                                                }
                                                else
                                                {
                                                    arytmprecall_type = arytmprecall_type + ary_r_d[1].ToString() + ";";
                                                }
                                            }
                                            if (ary_r_d.Length > 2)
                                            {
                                                if (ary_r_d[2].ToString() == "")
                                                {
                                                    arytmprecall_typeId = arytmprecall_typeId + " " + ";";
                                                }
                                                else
                                                {
                                                    arytmprecall_typeId = arytmprecall_typeId + ary_r_d[2].ToString() + ";";
                                                }
                                            }
                                        }
                                    }

                                    string[] tmpdue_dates;
                                    if (arytmpdue_dates.Length > 0)
                                    {
                                        arytmpdue_dates = arytmpdue_dates.Substring(0, arytmpdue_dates.Length - 1);
                                        tmpdue_dates = arytmpdue_dates.ToString().Split(';');
                                    }
                                    else
                                    {
                                        tmpdue_dates = new string[0];
                                    }

                                    string[] tmptmprecall_type;
                                    if (arytmprecall_type.Length > 0)
                                    {
                                        arytmprecall_type = arytmprecall_type.Substring(0, arytmprecall_type.Length - 1);
                                        tmptmprecall_type = arytmprecall_type.ToString().Split(';');
                                    }
                                    else
                                    {
                                        tmptmprecall_type = new string[0];
                                    }

                                    string[] tmptmprecall_typeId;
                                    if (arytmprecall_typeId.Length > 0)
                                    {
                                        arytmprecall_typeId = arytmprecall_typeId.Substring(0, arytmprecall_typeId.Length - 1);
                                        tmptmprecall_typeId = arytmprecall_typeId.ToString().Split(';');
                                    }
                                    else
                                    {
                                        tmptmprecall_typeId = new string[0];
                                    }


                                    Push_PatientBOMultiLocation patient = new Push_PatientBOMultiLocation
                                    {
                                        organization = Utility.Organization_ID,
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(), //Utility.Loc_ID,
                                        created_by = Utility.User_ID,

                                        patient_localdb_id = dtPatientRow["Patient_LocalDB_ID"].ToString().Trim(),
                                        patient_ehr_id = dtPatientRow["Patient_EHR_ID"].ToString().Trim(),
                                        Patient_Web_ID = dtPatientRow["Patient_Web_ID"].ToString().Trim(),
                                        first_name = dtPatientRow["First_name"].ToString().Trim(),
                                        last_name = dtPatientRow["Last_name"].ToString().Trim(),
                                        middle_name = dtPatientRow["Middle_Name"].ToString().Trim(),
                                        salutation = dtPatientRow["Salutation"].ToString().Trim(),
                                        preferred_name = dtPatientRow["preferred_name"].ToString().Trim(),
                                        status = dtPatientRow["Status"].ToString().Trim(),
                                        sex = dtPatientRow["Sex"].ToString().Trim(),
                                        marital_status = dtPatientRow["MaritalStatus"].ToString().Trim(),
                                        birth_date = tmpBirth_Date,
                                        email = dtPatientRow["Email"].ToString().Trim(),
                                        mobile = dtPatientRow["Mobile"].ToString().Trim(),
                                        home_phone = dtPatientRow["Home_Phone"].ToString().Trim(),
                                        work_phone = dtPatientRow["Work_Phone"].ToString().Trim(),
                                        address_one = dtPatientRow["Address1"].ToString().Trim(),
                                        address_two = dtPatientRow["Address2"].ToString().Trim(),
                                        city = dtPatientRow["City"].ToString().Trim(),
                                        state = dtPatientRow["State"].ToString().Trim(),
                                        zipcode = dtPatientRow["Zipcode"].ToString().Trim(),
                                        responsibleparty_status = dtPatientRow["ResponsibleParty_Status"].ToString().Trim(),
                                        current_bal = dtPatientRow["CurrentBal"].ToString().Trim(),
                                        thirty_day = dtPatientRow["ThirtyDay"].ToString().Trim(),
                                        sixty_day = dtPatientRow["SixtyDay"].ToString().Trim(),
                                        ninety_day = dtPatientRow["NinetyDay"].ToString().Trim(),
                                        over_ninty = dtPatientRow["Over90"].ToString().Trim(),
                                        firstvisit_date = tmpFirstVisit_Date,
                                        lastvisit_date = tmpLastVisit_Date,
                                        primary_insurance = dtPatientRow["Primary_Insurance"].ToString().Trim(),
                                        primary_insurance_companyname = dtPatientRow["Primary_Insurance_CompanyName"].ToString().Trim(),
                                        primary_ins_subscriber_id = dtPatientRow["Primary_Ins_Subscriber_ID"].ToString().Trim(),
                                        secondary_insurance = dtPatientRow["Secondary_Insurance"].ToString().Trim(),
                                        secondary_insurance_companyname = dtPatientRow["Secondary_Insurance_CompanyName"].ToString().Trim(),
                                        secondary_ins_subscriber_id = dtPatientRow["Secondary_Ins_Subscriber_ID"].ToString().Trim(),
                                        guar_id = dtPatientRow["Guar_ID"].ToString().Trim(),
                                        pri_provider_id = dtPatientRow["Pri_Provider_ID"].ToString().Trim(),
                                        sec_provider_id = dtPatientRow["Sec_Provider_ID"].ToString().Trim(),
                                        receive_sms = dtPatientRow["ReceiveSms"].ToString().Trim(),
                                        receive_email = dtPatientRow["ReceiveEmail"].ToString().Trim(),
                                        nextvisit_date = nextvisit_date,
                                        due_date = due_date,
                                        due_dates = tmpdue_dates,
                                        recall_type = tmptmprecall_type,
                                        ehr_key = tmptmprecall_typeId,
                                        used_benefit = dtPatientRow["used_benefit"].ToString().Trim(),
                                        remaining_benefit = dtPatientRow["remaining_benefit"].ToString().Trim(),
                                        collect_payment = dtPatientRow["collect_payment"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtPatientRow["is_deleted"]),
                                        ehr_status = dtPatientRow["EHR_Status"].ToString().Trim(),
                                        receive_voice_call = dtPatientRow["ReceiveVoiceCall"].ToString().Trim(),
                                        patient_status = dtPatientRow["Patient_status"].ToString().Trim().ToLower(),
                                        PreferredLanguage = dtPatientRow["PreferredLanguage"].ToString() != "" ? dtPatientRow["PreferredLanguage"].ToString().Trim().ToLower() : "english",
                                        patient_note = dtPatientRow["Patient_Note"].ToString().Trim(),

                                        ssn = (dtPatientRow["ssn"].ToString().Trim()),
                                        driverlicense = (dtPatientRow["driverlicense"].ToString().Trim()),
                                        groupid = dtPatientRow["groupid"].ToString().Trim(),
                                        insurancecompanyphonenumber = dtPatientRow["Prim_Ins_Company_Phonenumber"].ToString().Trim(),
                                        emergencycontactid = dtPatientRow["emergencycontactId"].ToString().Trim(),
                                        emergencycontactfirstname = dtPatientRow["EmergencyContact_First_Name"].ToString().Trim(),
                                        emergencycontactlastname = dtPatientRow["EmergencyContact_Last_Name"].ToString().Trim(),
                                        emergencycontactnumber = dtPatientRow["emergencycontactnumber"].ToString().Trim(),
                                        school = dtPatientRow["school"].ToString().Trim(),
                                        employer = dtPatientRow["employer"].ToString().Trim(),
                                        spouseid = dtPatientRow["spouseId"].ToString().Trim(),
                                        spousefirstname = dtPatientRow["Spouse_First_Name"].ToString().Trim(),
                                        spouselastname = dtPatientRow["Spouse_Last_Name"].ToString().Trim(),
                                        responsiblepartyid = dtPatientRow["responsiblepartyId"].ToString().Trim(),
                                        responsiblepartyfirstname = dtPatientRow["ResponsibleParty_First_Name"].ToString().Trim(),
                                        responsiblepartylastname = dtPatientRow["ResponsibleParty_Last_Name"].ToString().Trim(),
                                        responsiblepartyssn = (dtPatientRow["responsiblepartyssn"].ToString().Trim()),
                                        responsiblepartybirthdate = respBirth_Date.ToString().Trim(),
                                        office_id = dtPatientRow["Clinic_Number"].ToString().Trim(),
                                        multilocation = MultiLocation
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonPatient.Append(javaScriptSerializer.Serialize(patient) + ",");
                                }

                                if (JsonPatient.Length > 0)
                                {
                                    string jsonString = "[" + JsonPatient.ToString().Remove(JsonPatient.Length - 1) + "]";
                                    //string strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "patient");
                                    strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Patient(jsonString, "patientmultilocation", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strPatient.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Patient Sync (Local Database To Adit Server) ] Service Install Id " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  : " + strPatient);
                                    }
                                }
                                else
                                {
                                    strPatient = "Success";
                                }
                            }

                            if (strPatient.ToLower() == "Success".ToLower())
                            {
                                IsSynchPatient = true;
                            }
                            else
                            {
                                if (strPatient.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchPatient = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Patient Sync (Local Database To Adit Server) ] : " + strPatient);
                                    IsSynchPatient = false;
                                }
                            }
                        }
                    }
                }
                if (IsSynchPatient)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                    GoalBase.WriteToSyncLogFile_Static("Patient Sync (Local Database To Adit Server) Successfully.");
                }
                //  IsPatientSyncingRunning = false;
                //}
            }
            catch (Exception ex)
            {
                //IsPatientSyncingRunning = false;
                GoalBase.WriteToErrorLogFile_Static("[Patient Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
            //}
            //else
            //{
            //    IsPatientSyncingInQueue = true;
            //}
        }
        public static void SynchDataLiveDB_Push_PatientStatus()
        {


            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{

                bool IsSynchPatient = true;

                DataTable dtLocalPatient = SynchLocalBAL.GetPushLocalPatientStatusData();

                if (dtLocalPatient.Rows.Count > 0)
                {

                    int totPatient = dtLocalPatient.Rows.Count;
                    int cntPatient = 0;

                    PushPatientRecord = 0;
                    TotalPushPatientRecord = dtLocalPatient.Rows.Count;
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, patientPushCounter);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, Utility.mstSyncBatchSize.PatientStatus);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        //https://app.asana.com/0/751059797849097/1148328937003503
                        splitdt[i] = Utility.CreateDistinctRecords(splitdt[i], "Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID", "Patient_EHR_Id");
                        string strPatient = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {

                            if (Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonPatient = new System.Text.StringBuilder();
                                foreach (DataRow dtPatientRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                {

                                    PushPatientRecord = PushPatientRecord + 1;

                                    Push_PatientStatus patient = new Push_PatientStatus
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(), //Utility.Loc_ID,                                      
                                        patient_ehr_id = dtPatientRow["Patient_EHR_ID"].ToString().Trim(),
                                        patient_status = dtPatientRow["patient_status"].ToString().Trim().ToLower(),
                                        webid = dtPatientRow["Patient_Web_ID"].ToString().Trim()
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonPatient.Append(javaScriptSerializer.Serialize(patient) + ",");
                                }

                                if (JsonPatient.Length > 0)
                                {
                                    string jsonString = "[" + JsonPatient.ToString().Remove(JsonPatient.Length - 1) + "]";
                                    //string strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "patient");
                                    strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_PatientStatus(jsonString, "patientstatus", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), dtLocalPatient);

                                    if (strPatient.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Patient_Status Sync (Local Database To Adit Server) ] Service Install Id " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  : " + strPatient);
                                    }
                                }
                                else
                                {
                                    strPatient = "Success";
                                }
                            }

                            if (strPatient.ToLower() == "Success".ToLower())
                            {
                                IsSynchPatient = true;
                            }
                            else
                            {
                                if (strPatient.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchPatient = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Patient_Status Sync (Local Database To Adit Server) ] : " + strPatient);
                                    IsSynchPatient = false;
                                }
                            }
                        }
                    }
                }
                if (IsSynchPatient)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                    GoalBase.WriteToSyncLogFile_Static("Patient_Status Sync (Local Database To Adit Server) Successfully.");
                }
                // }
                //  IsPatientSyncingRunning = false;
            }
            catch (Exception ex)
            {
                //IsPatientSyncingRunning = false;
                GoalBase.WriteToErrorLogFile_Static("[Patient_Status Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
            //}
            //else
            //{
            //    IsPatientSyncingInQueue = true;
            //}
        }
        public static void SynchDataLiveDB_Push_PatientStatus(int serviceInstallid, int clinicnumber)
        {


            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                GoalBase.WriteToSyncLogFile_Static("Patient Status Sync Local To Adit App");
                bool IsSynchPatient = true;

                DataTable dtLocalPatient = SynchLocalBAL.GetPushLocalPatientStatusData(serviceInstallid, clinicnumber);

                if (dtLocalPatient.Rows.Count > 0)
                {

                    int totPatient = dtLocalPatient.Rows.Count;
                    int cntPatient = 0;

                    PushPatientRecord = 0;
                    TotalPushPatientRecord = dtLocalPatient.Rows.Count;
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, patientPushCounter);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, Utility.mstSyncBatchSize.PatientStatus);
                    GoalBase.WriteToSyncLogFile_Static("Patient Status Sync to Adit App Total count is " + TotalPushPatientRecord.ToString());
                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        //https://app.asana.com/0/751059797849097/1148328937003503
                        splitdt[i] = Utility.CreateDistinctRecords(splitdt[i], "Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID", "Patient_EHR_Id");
                        string strPatient = "";
                        DataRow drLoc = Utility.DtLocationList.Select("Clinic_number = " + clinicnumber.ToString() + " AND Service_Install_Id = " + serviceInstallid.ToString()).First();
                        //for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {

                            //if (Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonPatient = new System.Text.StringBuilder();
                                foreach (DataRow dtPatientRow in splitdt[i].Select("Clinic_Number = '" + clinicnumber.ToString() + "' And Service_Install_Id = '" + serviceInstallid.ToString() + "' "))
                                {

                                    PushPatientRecord = PushPatientRecord + 1;

                                    Push_PatientStatus patient = new Push_PatientStatus
                                    {
                                        appointmentlocation = drLoc["Location_Id"].ToString(),//Utility.Location_ID,
                                        location = drLoc["Loc_ID"].ToString(), //Utility.Loc_ID,                                      
                                        patient_ehr_id = dtPatientRow["Patient_EHR_ID"].ToString().Trim(),
                                        patient_status = dtPatientRow["patient_status"].ToString().Trim().ToLower(),
                                        webid = dtPatientRow["Patient_Web_ID"].ToString().Trim()
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonPatient.Append(javaScriptSerializer.Serialize(patient) + ",");
                                }

                                if (JsonPatient.Length > 0)
                                {
                                    string jsonString = "[" + JsonPatient.ToString().Remove(JsonPatient.Length - 1) + "]";
                                    //string strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "patient");
                                    strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_PatientStatus(jsonString, "patientstatus", serviceInstallid.ToString(), drLoc["Location_Id"].ToString(), dtLocalPatient);

                                    if (strPatient.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Patient_Status Sync (Local Database To Adit Server) ] Service Install Id " + serviceInstallid.ToString() + " And Clinic " + clinicnumber.ToString() + "  : " + strPatient);
                                    }
                                }
                                else
                                {
                                    strPatient = "Success";
                                }
                            }

                            if (strPatient.ToLower() == "Success".ToLower())
                            {
                                IsSynchPatient = true;
                            }
                            else
                            {
                                if (strPatient.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchPatient = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Patient_Status Sync (Local Database To Adit Server) ] : " + strPatient);
                                    IsSynchPatient = false;
                                }
                            }
                        }
                    }
                }
                if (IsSynchPatient)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                    GoalBase.WriteToSyncLogFile_Static("Patient_Status Sync (Local Database To Adit Server) Successfully.");
                }
                // }
                //  IsPatientSyncingRunning = false;
            }
            catch (Exception ex)
            {
                //IsPatientSyncingRunning = false;
                GoalBase.WriteToErrorLogFile_Static("[Patient_Status Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
            //}
            //else
            //{
            //    IsPatientSyncingInQueue = true;
            //}
        }


        #endregion

        #region Disease

        public static void SynchDataLiveDB_Push_PatientDisease()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchDisease = true;
                DataTable dtLocalDisease = SynchLocalBAL.GetPushLocalPatientDiseaseData();
                if (dtLocalDisease.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalDisease, 200);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalDisease, Utility.mstSyncBatchSize.PatientDisease);
                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strDisease = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonDisease = new System.Text.StringBuilder();
                                foreach (DataRow dtDiseaseRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                {
                                    Push_PatientDiseasesBO Diseases = new Push_PatientDiseasesBO
                                    {
                                        disease_name = dtDiseaseRow["Disease_Name"].ToString().Replace(",", "").Trim(),
                                        disease_type = dtDiseaseRow["Disease_Type"].ToString().Replace(",", "").Trim(),
                                        is_deleted = Convert.ToBoolean(dtDiseaseRow["is_deleted"].ToString().Replace(",", "").Trim()),
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                                        organization = Utility.Organization_ID,
                                        disease_ehr_id = dtDiseaseRow["Disease_EHR_ID"].ToString(),
                                        patient_ehr_id = dtDiseaseRow["Patient_EHR_ID"].ToString(),
                                        patientdisease_localdb_id = dtDiseaseRow["PatientDisease_LocalDB_ID"].ToString(),
                                        patientdisease_web_id = dtDiseaseRow["PatientDisease_Web_ID"].ToString(),
                                        created_by = Utility.User_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString()
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonDisease.Append(javaScriptSerializer.Serialize(Diseases) + ",");
                                }
                                if (JsonDisease.Length > 0)
                                {
                                    string jsonString = "[" + JsonDisease.ToString().Remove(JsonDisease.Length - 1) + "]";
                                    strDisease = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_PatientDisease(jsonString, "PatientDisease", Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());

                                    if (strDisease.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[PatientDisease Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strDisease);
                                    }
                                }
                                else
                                {
                                    strDisease = "Success";
                                }
                            }

                            if (strDisease.ToLower() == "Success".ToLower())
                            {
                                IsSynchDisease = true;
                            }
                            else
                            {
                                if (strDisease.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchDisease = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientDisease Sync (Local Database To Adit Server) ] : " + strDisease);
                                    IsSynchDisease = false;
                                }
                            }
                        }
                    }
                }
                if (IsSynchDisease)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientDisease_Push");
                    GoalBase.WriteToSyncLogFile_Static("PatientDisease Sync (Local Database To Adit Server) Successfully.");
                }
                //}
            }
            catch (Exception ex)
            {
                // GoalBase.WriteToErrorLogFile_Static("[Disease Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        public static void SynchDataLiveDB_Push_Disease()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchDisease = true;
                DataTable dtLocalDisease = SynchLocalBAL.GetPushLocalDiseaseData();

                if (dtLocalDisease.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalDisease, 200);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalDisease, Utility.mstSyncBatchSize.Disease);
                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strDisease = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonDisease = new System.Text.StringBuilder();
                                foreach (DataRow dtDiseaseRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                {
                                    Push_DiseasesBO Diseases = new Push_DiseasesBO
                                    {
                                        name = dtDiseaseRow["Disease_Name"].ToString().Replace(",", "").Trim(),
                                        disease_type = dtDiseaseRow["Disease_Type"].ToString().Replace(",", "").Trim(),
                                        is_deleted = Convert.ToBoolean(dtDiseaseRow["is_deleted"].ToString().Replace(",", "").Trim()),
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                                        organization = Utility.Organization_ID,
                                        disease_ehr_id = dtDiseaseRow["Disease_EHR_ID"].ToString(),
                                        disease_localdb_id = dtDiseaseRow["Disease_LocalDB_ID"].ToString(),
                                        disease_web_id = dtDiseaseRow["Disease_Web_ID"].ToString(),
                                        created_by = Utility.User_ID
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonDisease.Append(javaScriptSerializer.Serialize(Diseases) + ",");
                                }
                                if (JsonDisease.Length > 0)
                                {
                                    string jsonString = "[" + JsonDisease.ToString().Remove(JsonDisease.Length - 1) + "]";
                                    strDisease = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Disease(jsonString, "Disease", Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());

                                    if (strDisease.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Disease Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strDisease);
                                    }
                                }
                                else
                                {
                                    strDisease = "Success";
                                }
                            }

                            if (strDisease.ToLower() == "Success".ToLower())
                            {
                                IsSynchDisease = true;
                            }
                            else
                            {
                                if (strDisease.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchDisease = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Disease Sync (Local Database To Adit Server) ] : " + strDisease);
                                    IsSynchDisease = false;
                                }
                            }
                        }
                    }
                }
                if (IsSynchDisease)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease_Push");
                    GoalBase.WriteToSyncLogFile_Static("Disease Sync (Local Database To Adit Server) Successfully.");
                }
                //}
            }
            catch (Exception ex)
            {
                // GoalBase.WriteToErrorLogFile_Static("[Disease Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        public static void SynchDataLiveDB_Push_Medication()
        {
            try
            {
                bool IsSynchMedication = true;
                DataTable dtLocalMedication = SynchLocalBAL.GetPushLocalMedicationData();
                if (dtLocalMedication.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalMedication, 200);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalMedication, Utility.mstSyncBatchSize.Medication);
                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strMedication = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonMedication = new System.Text.StringBuilder();
                                foreach (DataRow dtMedicationRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                {
                                    Push_MedicationBO Medications = new Push_MedicationBO
                                    {
                                        medication_ehr_id = dtMedicationRow["Medication_EHR_ID"].ToString().Replace(",", "").Trim(),
                                        medication_name = dtMedicationRow["Medication_Name"].ToString().Replace(",", "").Trim(),
                                        medication_type = dtMedicationRow["Medication_Type"].ToString().Replace(",", "").Trim(),
                                        medication_description = dtMedicationRow["Medication_Description"].ToString().Replace(",", "").Trim(),
                                        medication_note = dtMedicationRow["Medication_Notes"].ToString().Replace(",", "").Trim(),
                                        medication_parent_id = dtMedicationRow["Medication_Parent_EHR_ID"].ToString().Replace(",", ""),
                                        sig = dtMedicationRow["Medication_Sig"].ToString().Replace(",", "").Trim(),
                                        drug_quantity = dtMedicationRow["Drug_Quantity"].ToString().Replace(",", "").Trim(),
                                        refills = dtMedicationRow["Refills"].ToString().Replace(",", "").Trim(),
                                        medication_localdb_id = dtMedicationRow["Medication_LocalDB_ID"].ToString().Replace(",", "").Trim(),
                                        service_install_id = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString().Replace(",", "").Trim(),
                                        is_active = dtMedicationRow["Is_Active"].ToString().ToUpper().Trim() == "TRUE" ? true : false,
                                        medication_provider_id = dtMedicationRow["Medication_Provider_ID"].ToString().Replace(",", "").Trim(),
                                        allow_generic_sub = dtMedicationRow["Allow_Generic_Sub"].ToString().ToUpper().Trim() == "TRUE" ? true : false,
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                                        location = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(),
                                        organization = Utility.Organization_ID,
                                        is_deleted = dtMedicationRow["Is_Deleted"].ToString().ToUpper().Trim() == "TRUE" ? true : false,
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonMedication.Append(javaScriptSerializer.Serialize(Medications) + ",");
                                }
                                if (JsonMedication.Length > 0)
                                {
                                    string jsonString = "[" + JsonMedication.ToString().Remove(JsonMedication.Length - 1) + "]";
                                    strMedication = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Medication(jsonString, "Medication", Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    if (strMedication.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Medication Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strMedication);
                                    }
                                }
                                else
                                {
                                    strMedication = "Success";
                                }
                            }
                            if (strMedication.ToLower() == "Success".ToLower())
                            {
                                IsSynchMedication = true;
                            }
                            else
                            {
                                if (strMedication.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchMedication = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Medication Sync (Local Database To Adit Server) ] : " + strMedication);
                                    IsSynchMedication = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // GoalBase.WriteToErrorLogFile_Static("[Disease Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        public static void SynchDataLiveDB_Push_PatientMedication()
        {
            try
            {
                var JsonPatientMedication = new System.Text.StringBuilder();
                bool IsSynchPatientMedication = true;
                string expirydate = string.Empty;
                string start_date = string.Empty;
                string stopdate = string.Empty;
                string end_date = string.Empty;
                string entered_date = string.Empty;
                string last_synchdate = string.Empty;
                DataTable dtLocalPatientMedication = SynchLocalBAL.GetPushLocalPatientMedicationData();
                if (dtLocalPatientMedication.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalPatientMedication, 200);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalPatientMedication, Utility.mstSyncBatchSize.PatientMedication);
                    for (int j = 0; j < splitdt.Count; j++)
                    {
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                JsonPatientMedication = new System.Text.StringBuilder();
                                string strAppointmentType = "";
                                foreach (DataRow dtPatMedicationRow in splitdt[j].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                                {
                                    try
                                    {
                                        if (dtPatMedicationRow["Start_Date"] != null && dtPatMedicationRow["Start_Date"].ToString() != string.Empty)
                                        {
                                            start_date = Convert.ToDateTime(dtPatMedicationRow["Start_Date"].ToString().Trim()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        start_date = "";
                                    }
                                    try
                                    {
                                        if (dtPatMedicationRow["End_Date"] != null && dtPatMedicationRow["End_Date"].ToString() != string.Empty)
                                        {
                                            end_date = Convert.ToDateTime(dtPatMedicationRow["End_Date"].ToString().Trim()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        end_date = "";
                                    }
                                    try
                                    {
                                        if (dtPatMedicationRow["Expiry_Date"] != null && dtPatMedicationRow["Expiry_Date"].ToString() != string.Empty)
                                        {
                                            expirydate = Convert.ToDateTime(dtPatMedicationRow["Expiry_Date"].ToString().Trim()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        expirydate = "";
                                    }

                                    try
                                    {
                                        if (dtPatMedicationRow["Last_Sync_Date"] != null && dtPatMedicationRow["Last_Sync_Date"].ToString() != string.Empty)
                                        {
                                            last_synchdate = Convert.ToDateTime(dtPatMedicationRow["Last_Sync_Date"].ToString().Trim()).ToString("yyyy-MM-dd");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        last_synchdate = "";
                                    }

                                    Push_PatientMedicationBO PatientMedication = new Push_PatientMedicationBO
                                    {
                                        organization = Utility.Organization_ID,
                                        appointmentlocation = Utility.DtLocationList.Rows[i]["Location_Id"].ToString(),
                                        location = Utility.DtLocationList.Rows[i]["Loc_ID"].ToString(),
                                        patientmedication_localdb_id = dtPatMedicationRow["PatientMedication_LocalDB_ID"].ToString().Trim().Replace(",", "").Trim(),
                                        patientmedication_ehr_id = dtPatMedicationRow["PatientMedication_EHR_ID"].ToString().Replace(",", "").Trim(),
                                        medication_ehr_id = dtPatMedicationRow["Medication_EHR_ID"].ToString().Replace(",", "").Trim(),
                                        medication_name = dtPatMedicationRow["Medication_Name"].ToString().Replace(",", "").Trim(),
                                        patient_ehr_id = dtPatMedicationRow["Patient_EHR_ID"].ToString().Replace(",", "").Trim(),
                                        providerid = dtPatMedicationRow["Provider_EHR_ID"].ToString().Replace(",", "").Trim(),
                                        patientnote = dtPatMedicationRow["Patient_Notes"].ToString().Replace(",", "").Trim(),
                                        notes = dtPatMedicationRow["Medication_Note"].ToString().Replace(",", "").Trim(),
                                        drug_quantity = (dtPatMedicationRow["Drug_Quantity"].ToString().Trim() == "" || dtPatMedicationRow["Drug_Quantity"].ToString().Trim() == string.Empty) ? "0" : dtPatMedicationRow["Drug_Quantity"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtPatMedicationRow["is_deleted"]),
                                        refills = (dtPatMedicationRow["Refills"].ToString().Trim() == "" || dtPatMedicationRow["Refills"].ToString().Trim() == string.Empty) ? "0" : dtPatMedicationRow["Refills"].ToString().Trim(),
                                        startdate = start_date,
                                        medicaldescription = dtPatMedicationRow["MedicalDescription"].ToString().Trim(),
                                        stopdate = end_date,
                                        last_synchdate = last_synchdate,
                                        expiry_date = expirydate,
                                        is_active = Convert.ToBoolean(Convert.ToInt32(dtPatMedicationRow["is_active"]))
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonPatientMedication.Append(javaScriptSerializer.Serialize(PatientMedication) + ",");
                                }

                                if (JsonPatientMedication.Length > 0)
                                {
                                    string jsonString = "[" + JsonPatientMedication.ToString().Remove(JsonPatientMedication.Length - 1) + "]";
                                    // strAppointmentType = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_PatientMedication(jsonString, "patientmedication", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_Id"].ToString());
                                    strAppointmentType = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_PatientMedication(jsonString, "patientmedication", Utility.DtLocationList.Rows[i]["Location_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString());
                                    if (strAppointmentType.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[PatientMedication Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strAppointmentType);
                                    }
                                }
                                else
                                {
                                    strAppointmentType = "Success";
                                }

                                if (strAppointmentType.ToLower() == "Success".ToLower())
                                {
                                    IsSynchPatientMedication = true;
                                }
                                else
                                {
                                    if (strAppointmentType.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchPatientMedication = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[PatientMedication Sync (Local Database To Adit Server) ] Service Install Id  : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "  And Clinic " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strAppointmentType);
                                        IsSynchPatientMedication = false;
                                    }
                                }
                            }
                        }
                    }
                }

                if (IsSynchPatientMedication)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("patientmedication");
                    GoalBase.WriteToSyncLogFile_Static("PatientMedication Sync (Local Database To Adit Server) Successfully.");
                    IsApptTypeSyncPush = true;
                }
                else
                {
                    IsApptTypeSyncPush = false;
                }
                //}
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PatientMedication Sync (Local Database To Adit Server) ] : " + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
        #endregion

        #region RecallType

        public static void SynchDataLiveDB_Push_RecallType()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchRecallType = true;
                DataTable dtLocalRecallType = SynchLocalBAL.GetPushLocalRecallTypeData();

                if (dtLocalRecallType.Rows.Count > 0)
                {
                    string strRecallType = "";
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                        {
                            string[] Loc_IDs;
                            Loc_IDs = Utility.DtLocationList.Rows[i]["Loc_Id"].ToString().Trim().Split(';');
                            var JsonRecallType = new System.Text.StringBuilder();
                            foreach (DataRow dtRecallTypeRow in dtLocalRecallType.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                            {
                                RecallTypeSubBO RecallTypeSub = new RecallTypeSubBO
                                {
                                    name = dtRecallTypeRow["RecallType_Name"].ToString().Trim(),
                                    key = dtRecallTypeRow["RecallType_EHR_ID"].ToString().Trim(),
                                    ehr_key = dtRecallTypeRow["RecallType_EHR_ID"].ToString().Trim(),
                                    is_deleted = Convert.ToBoolean(dtRecallTypeRow["is_deleted"])
                                };
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                JsonRecallType.Append(javaScriptSerializer.Serialize(RecallTypeSub) + ",");
                            }

                            if (JsonRecallType.Length > 0)
                            {
                                string jsonRecallTypeSub = "[" + JsonRecallType.ToString().Remove(JsonRecallType.Length - 1) + "]";

                                RecallTypeMainBO RecallType = new RecallTypeMainBO
                                {
                                    ap_status = "[]",
                                    recall = jsonRecallTypeSub,
                                    organization = Utility.Organization_ID,
                                    locationId = Loc_IDs,
                                    created_by = Utility.User_ID
                                };

                                var javaScriptRecallTypeSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                string jsonRecallTypeString = javaScriptRecallTypeSerializer.Serialize(RecallType);

                                strRecallType = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_RecallType(jsonRecallTypeString, "RecallType_ApptStatus", Utility.DtLocationList.Rows[i]["Location_Id"].ToString());

                                if (strRecallType.ToLower() != "Success".ToLower())
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[RecallType Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strRecallType);
                                }
                            }
                            else
                            {
                                strRecallType = "Success";
                            }
                        }

                        if (strRecallType.ToLower() == "Success".ToLower())
                        {
                            IsSynchRecallType = true;
                        }
                        else
                        {
                            if (strRecallType.Contains("The remote name could not be resolved:"))
                            {
                                IsSynchRecallType = false;
                            }
                            else
                            {
                                GoalBase.WriteToErrorLogFile_Static("[RecallType Sync (Local Database To Adit Server) ] : " + strRecallType);
                                IsSynchRecallType = false;
                            }
                        }
                    }
                }

                if (IsSynchRecallType)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType_Push");
                    GoalBase.WriteToSyncLogFile_Static("RecallType Sync (Local Database To Adit Server) Successfully.");
                }
                //  }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[RecallType Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion
        #region User
        public static void SynchDataLiveDB_Push_User()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsUser = true;
                DataTable dtLocalUserData = SynchLocalBAL.GetPushLocalUser();

                if (dtLocalUserData.Rows.Count > 0)
                {
                    string strUser = "";
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                        {
                            var JsonUser = new System.Text.StringBuilder();
                            foreach (DataRow dtUserdataRow in dtLocalUserData.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                            {
                                PushUserBO UserSub = new PushUserBO
                                {
                                    user_ehr_id = dtUserdataRow["User_EHR_ID"].ToString().Trim(),
                                    firstname = dtUserdataRow["First_Name"].ToString().Trim(),
                                    lastname = dtUserdataRow["Last_Name"].ToString().Trim(),
                                    password = dtUserdataRow["Password"].ToString().Trim(),
                                    is_active = Convert.ToBoolean(dtUserdataRow["is_active"]),
                                    is_deleted = Convert.ToBoolean(dtUserdataRow["is_deleted"]),
                                    organization = Utility.Organization_ID,
                                    location = Utility.DtLocationList.Rows[i]["Loc_Id"].ToString().Trim(),//Utility.Loc_ID,
                                    appointmentlocation = Utility.DtLocationList.Rows[i]["Location_ID"].ToString().Trim()//Utility.Location_ID,
                                };


                                var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                JsonUser.Append(javaScriptApptStatusSerializer.Serialize(UserSub) + ",");


                            }

                            if (JsonUser.Length > 0)
                            {
                                string jsonString = "[" + JsonUser.ToString().Remove(JsonUser.Length - 1) + "]";
                                strUser = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_User(jsonString, "users", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_Id"].ToString());

                                if (strUser.ToLower() != "Success".ToLower())
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[User Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strUser);
                                }
                            }
                            else
                            {
                                strUser = "Success";
                            }
                        }

                        if (strUser.ToLower() == "Success".ToLower())
                        {
                            IsUser = true;
                        }
                        else
                        {
                            if (strUser.Contains("The remote name could not be resolved:"))
                            {
                                IsUser = false;
                            }
                            else
                            {
                                GoalBase.WriteToErrorLogFile_Static("[User Sync (Local Database To Adit Server) ] : " + strUser);
                                IsUser = false;
                            }
                        }
                    }
                }

                if (IsUser)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("User_Push");
                    GoalBase.WriteToSyncLogFile_Static("User Sync (Local Database To Adit Server) Successfully.");
                }
                //  }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[User Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }
        #endregion
        #region ApptStatus

        public static void SynchDataLiveDB_Push_ApptStatus()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchApptStatus = true;
                DataTable dtLocalApptStatus = SynchLocalBAL.GetPushLocalAppointmentStatusData();
                if (dtLocalApptStatus.Rows.Count > 0)
                {
                    string strApptStatus = "";
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                        {
                            string[] Loc_IDs;
                            Loc_IDs = Utility.DtLocationList.Rows[i]["Loc_Id"].ToString().Trim().Split(';');
                            string jsonApptStatusSub = "";
                            var JsonApptStatus = new System.Text.StringBuilder();
                            foreach (DataRow dtApptStatusRow in dtLocalApptStatus.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                            {
                                RecallTypeSubBO ApptStatusSub = new RecallTypeSubBO
                                {
                                    name = dtApptStatusRow["ApptStatus_Name"].ToString().Trim().Replace(",", "").Trim(),
                                    key = dtApptStatusRow["ApptStatus_EHR_ID"].ToString().Trim(),
                                    ehr_key = dtApptStatusRow["ApptStatus_EHR_ID"].ToString().Trim(),
                                };
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                JsonApptStatus.Append(javaScriptSerializer.Serialize(ApptStatusSub) + ",");
                            }

                            if (JsonApptStatus.Length > 0)
                            {
                                jsonApptStatusSub = "[" + JsonApptStatus.ToString().Remove(JsonApptStatus.Length - 1) + "]";

                                RecallTypeMainBO ApptStatus = new RecallTypeMainBO
                                {
                                    ap_status = jsonApptStatusSub,
                                    recall = "[]",
                                    organization = Utility.Organization_ID,
                                    locationId = Loc_IDs,
                                    created_by = Utility.User_ID
                                };

                                var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                string jsonApptStatusString = javaScriptApptStatusSerializer.Serialize(ApptStatus);

                                strApptStatus = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_ApptStatus(jsonApptStatusString, "RecallType_ApptStatus", Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_Id"].ToString());

                                if (strApptStatus.ToLower() != "Success".ToLower())
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Appointment Status Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strApptStatus);
                                }
                            }
                            else
                            {
                                strApptStatus = "Success";
                            }
                        }

                        if (strApptStatus.ToLower() == "Success".ToLower())
                        {
                            IsSynchApptStatus = true;
                        }
                        else
                        {
                            if (strApptStatus.Contains("The remote name could not be resolved:"))
                            {
                                IsSynchApptStatus = false;
                            }
                            else
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Appointment Status Sync (Local Database To Adit Server) ] : " + strApptStatus);
                                IsSynchApptStatus = false;
                            }
                        }
                    }
                }

                if (IsSynchApptStatus)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus_Push");
                    GoalBase.WriteToSyncLogFile_Static("Appointment Status Sync (Local Database To Adit Server) Successfully.");
                }

                SynchDataLiveDB_Push_ApptStatus_With_ApptStatus_Type();
                //   }

            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Appointment Status Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        public static void SynchDataLiveDB_Push_ApptStatus_With_ApptStatus_Type()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsSynchApptStatus = true;
                DataTable dtLocalApptStatus = SynchLocalBAL.GetPushLocalAppointmentStatusData();
                DataTable dtApptStatus_normal = dtLocalApptStatus.Clone();
                DataTable dtApptStatus_confirm = dtLocalApptStatus.Clone();
                DataTable dtApptStatus_unshedule = dtLocalApptStatus.Clone();

                DataRow[] drApptStatus_normal = dtLocalApptStatus.Copy().Select("ApptStatus_Type = 'normal'");
                if (drApptStatus_normal.Length > 0)
                {
                    dtApptStatus_normal = drApptStatus_normal.CopyToDataTable();
                }

                DataRow[] drApptStatus_confirm = dtLocalApptStatus.Copy().Select("ApptStatus_Type = 'confirm'");
                if (drApptStatus_confirm.Length > 0)
                {
                    dtApptStatus_confirm = drApptStatus_confirm.CopyToDataTable();
                }

                DataRow[] drApptStatus_unshedule = dtLocalApptStatus.Copy().Select("ApptStatus_Type = 'unshedule'");
                if (drApptStatus_unshedule.Length > 0)
                {
                    dtApptStatus_unshedule = drApptStatus_unshedule.CopyToDataTable();
                }

                if (dtLocalApptStatus.Rows.Count > 0)
                {
                    string strApptStatus = "";
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                        {
                            string[] Loc_IDs;
                            Loc_IDs = Utility.DtLocationList.Rows[i]["Loc_Id"].ToString().Trim().Split(';');

                            var JsonApptStatus_normal = new System.Text.StringBuilder();

                            string jsonApptStatus_normal = "";
                            if (dtApptStatus_normal.Rows.Count > 0)
                            {
                                foreach (DataRow dtApptStatusRow in dtApptStatus_normal.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                                {
                                    ap_status_normalBO ApptStatusSub = new ap_status_normalBO
                                    {
                                        name = dtApptStatusRow["ApptStatus_Name"].ToString().Trim().Replace(",", "").Trim(),
                                        key = dtApptStatusRow["ApptStatus_EHR_ID"].ToString().Trim(),
                                        ehr_key = dtApptStatusRow["ApptStatus_EHR_ID"].ToString().Trim(),
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonApptStatus_normal.Append(javaScriptSerializer.Serialize(ApptStatusSub) + ",");
                                }

                                if (JsonApptStatus_normal.Length > 0)
                                {
                                    jsonApptStatus_normal = "[" + JsonApptStatus_normal.ToString().Remove(JsonApptStatus_normal.Length - 1) + "]";
                                }
                                else
                                {
                                    jsonApptStatus_normal = "[]";
                                }
                            }
                            else
                            {
                                jsonApptStatus_normal = "[]";
                            }

                            string jsonApptStatus_unshedule = string.Empty;

                            if (dtApptStatus_unshedule.Rows.Count > 0)
                            {
                                var JsonApptStatus_unshedule = new System.Text.StringBuilder();
                                foreach (DataRow dtApptStatusRow in dtApptStatus_unshedule.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                                {
                                    ap_status_unsheduleBO ApptStatusSub = new ap_status_unsheduleBO
                                    {
                                        name = dtApptStatusRow["ApptStatus_Name"].ToString().Trim().Replace(",", "").Trim(),
                                        key = dtApptStatusRow["ApptStatus_EHR_ID"].ToString().Trim(),
                                        ehr_key = dtApptStatusRow["ApptStatus_EHR_ID"].ToString().Trim(),
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonApptStatus_unshedule.Append(javaScriptSerializer.Serialize(ApptStatusSub) + ",");
                                }
                                if (JsonApptStatus_unshedule.Length > 0)
                                {
                                    jsonApptStatus_unshedule = "[" + JsonApptStatus_unshedule.ToString().Remove(JsonApptStatus_unshedule.Length - 1) + "]";
                                }
                                else
                                {
                                    jsonApptStatus_unshedule = "[]";
                                }
                            }
                            else
                            {
                                jsonApptStatus_unshedule = "[]";
                            }

                            string jsonApptStatus_confirm = string.Empty;
                            if (dtApptStatus_confirm.Rows.Count > 0)
                            {
                                var JsonApptStatus_confirm = new System.Text.StringBuilder();
                                foreach (DataRow dtApptStatusRow in dtApptStatus_confirm.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                                {
                                    ap_status_confirmBO ApptStatusSub = new ap_status_confirmBO
                                    {
                                        name = dtApptStatusRow["ApptStatus_Name"].ToString().Trim().Replace(",", "").Trim(),
                                        key = dtApptStatusRow["ApptStatus_EHR_ID"].ToString().Trim(),
                                        ehr_key = dtApptStatusRow["ApptStatus_EHR_ID"].ToString().Trim(),
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonApptStatus_confirm.Append(javaScriptSerializer.Serialize(ApptStatusSub) + ",");
                                }
                                if (JsonApptStatus_confirm.Length > 0)
                                    jsonApptStatus_confirm = "[" + JsonApptStatus_confirm.ToString().Remove(JsonApptStatus_confirm.Length - 1) + "]";
                                else
                                    jsonApptStatus_confirm = "[]";

                            }
                            else
                                jsonApptStatus_confirm = "[]";

                            AppointmentStatusWIthTypeBO ApptStatus = new AppointmentStatusWIthTypeBO
                            {
                                ap_status_confirm = jsonApptStatus_confirm,
                                ap_status_normal = jsonApptStatus_normal,
                                ap_status_unshedule = jsonApptStatus_unshedule,

                                organization = Utility.Organization_ID,
                                locationId = Utility.DtLocationList.Rows[i]["Loc_Id"].ToString().Trim(),//Utility.Loc_ID,
                                appointmentlocationid = Utility.DtLocationList.Rows[i]["Location_ID"].ToString().Trim(),//Utility.Location_ID,
                                created_by = Utility.User_ID
                            };

                            if (ApptStatus.ap_status_confirm.Length > 0 || ApptStatus.ap_status_normal.Length > 0 || ApptStatus.ap_status_unshedule.Length > 0)
                            {
                                var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                string jsonApptStatusString = javaScriptApptStatusSerializer.Serialize(ApptStatus);

                                strApptStatus = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_ApptStatus_With_Type(jsonApptStatusString, "ApptStatus_With_Type", Utility.DtLocationList.Rows[i]["Location_ID"].ToString().Trim());

                                if (strApptStatus.ToLower() != "Success".ToLower())
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[ApptStatus_with_Type Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strApptStatus);
                                }
                            }
                            else
                            {
                                strApptStatus = "Success";
                            }
                        }

                        if (strApptStatus.ToLower() == "Success".ToLower())
                        {
                            IsSynchApptStatus = true;
                        }
                        else
                        {
                            if (strApptStatus.Contains("The remote name could not be resolved:"))
                            {
                                IsSynchApptStatus = false;
                            }
                            else
                            {
                                GoalBase.WriteToErrorLogFile_Static("[ApptStatus_with_Type Sync (Local Database To Adit Server) ] : " + strApptStatus);
                                IsSynchApptStatus = false;
                            }
                        }
                    }
                }
                if (IsSynchApptStatus)
                {
                    GoalBase.WriteToSyncLogFile_Static("Appointment Status_with_Type Sync (Local Database To Adit Server) Successfully.");
                }
                // }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Appointment Status_with_Type Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion


        //rooja 19-4-24 - https://app.asana.com/0/1203599217474380/1207061756651636/f
        #region Insurance
        public static void SynchDataLiveDB_Push_Insurance()
        {
            try
            {
                bool IsSynchInsurance = true;
                DataTable dtLocalInsurance = SynchLocalBAL.GetPushLocalInsuranceData();
                if (dtLocalInsurance.Rows.Count > 0)
                {
                    string strInsurance = "";
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                        {
                            string[] Loc_IDs;
                            Loc_IDs = Utility.DtLocationList.Rows[i]["Loc_Id"].ToString().Trim().Split(';');
                            string jsonInsuranceSub = "";
                            var JsonInsurance = new System.Text.StringBuilder();
                            foreach (DataRow dtInsuranceRow in dtLocalInsurance.Select("Service_Install_Id = '" + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + "' "))
                            {
                                Push_InsuranceBO InsuranceSub = new Push_InsuranceBO
                                {
                                    location = Utility.DtLocationList.Rows[i]["Loc_Id"].ToString(),
                                    insurance_id = dtInsuranceRow["Insurance_EHR_ID"].ToString().Trim(),
                                    insurancename = dtInsuranceRow["Insurance_Name"].ToString().Trim().Replace(",", "").Trim(),
                                    address1 = dtInsuranceRow["Address"].ToString().Trim(),
                                    address2 = dtInsuranceRow["Address2"].ToString().Trim(),
                                    city = dtInsuranceRow["City"].ToString().Trim(),
                                    state = dtInsuranceRow["State"].ToString().Trim(),
                                    zip = dtInsuranceRow["Zipcode"].ToString().Trim(),
                                    phone = dtInsuranceRow["Phone"].ToString().Trim(),
                                    electid = dtInsuranceRow["ElectId"].ToString().Trim(),
                                    ehr_id = dtInsuranceRow["Insurance_EHR_ID"].ToString().Trim(),
                                    is_deleted = Convert.ToBoolean(dtInsuranceRow["Is_Deleted"].ToString().Trim()),
                                    employername = ""                                                                        
                                };
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                JsonInsurance.Append(javaScriptSerializer.Serialize(InsuranceSub) + ",");
                            }

                            if (JsonInsurance.Length > 0)
                            {
                                string jsonInsuranceString = "[" + JsonInsurance.ToString().Remove(JsonInsurance.Length - 1) + "]";
                                strInsurance = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Insurance(jsonInsuranceString, "Insurance", Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Location_Id"].ToString());

                                if (strInsurance.ToLower() != "Success".ToLower())
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Insurance Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strInsurance);
                                }
                            }
                            else
                            {
                                strInsurance = "Success";
                            }
                        }

                        if (strInsurance.ToLower() == "Success".ToLower())
                        {
                            IsSynchInsurance = true;
                        }
                        else
                        {
                            if (strInsurance.Contains("The remote name could not be resolved:"))
                            {
                                IsSynchInsurance = false;
                            }
                            else
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Insurance Sync (Local Database To Adit Server) ] : " + strInsurance);
                                IsSynchInsurance = false;
                            }
                        }
                    }
                }

                if (IsSynchInsurance)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Insurance_Push");
                    GoalBase.WriteToSyncLogFile_Static("Insurance Sync (Local Database To Adit Server) Successfully.");
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Insurance Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region Holiday

        public static void SynchDataLiveDB_Push_Holiday()
        {
            try
            {
                if (IsOperatorySyncPush) //&& Utility.AditLocationSyncEnable
                {
                    bool IsSynchHoliday = true;
                    DataTable dtLocalHoliday = SynchLocalBAL.GetPushLocalHolidayData();
                    if (dtLocalHoliday.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalHoliday, 200);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalHoliday, Utility.mstSyncBatchSize.Holiday);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strHoliday = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    var JsonHoliday = new System.Text.StringBuilder();
                                    foreach (DataRow dtHolidayRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And  Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "'"))
                                    {
                                        Push_HolidayBO HolidaySub = new Push_HolidayBO
                                        {
                                            H_EHR_ID = dtHolidayRow["H_EHR_ID"].ToString().Trim(),
                                            H_Web_ID = dtHolidayRow["H_Web_ID"].ToString().Trim(),
                                            H_Operatory_EHR_ID = dtHolidayRow["H_Operatory_EHR_ID"].ToString().Trim(),
                                            SchedDate = Convert.ToDateTime(dtHolidayRow["SchedDate"]).ToString("yyyy-MM-dd").Trim(),
                                            StartTime_1 = dtHolidayRow["StartTime_1"].ToString().Trim(),
                                            EndTime_1 = dtHolidayRow["EndTime_1"].ToString().Trim(),
                                            StartTime_2 = dtHolidayRow["StartTime_2"].ToString().Trim(),
                                            EndTime_2 = dtHolidayRow["EndTime_2"].ToString().Trim(),
                                            StartTime_3 = dtHolidayRow["StartTime_3"].ToString().Trim(),
                                            EndTime_3 = dtHolidayRow["EndTime_3"].ToString().Trim(),
                                            comment = dtHolidayRow["comment"].ToString().Trim(),
                                            is_deleted = Convert.ToBoolean(dtHolidayRow["is_deleted"].ToString()),
                                            Location_ID = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                                            created_by = Utility.User_ID,
                                            ParentLocation_ID = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), //Utility.Loc_ID,
                                            Organization_ID = Utility.Organization_ID
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonHoliday.Append(javaScriptSerializer.Serialize(HolidaySub) + ",");
                                    }

                                    if (JsonHoliday.Length > 0)
                                    {
                                        string jsonHolidaySub = "[" + JsonHoliday.ToString().Remove(JsonHoliday.Length - 1) + "]";

                                        //var javaScriptHolidaySerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        //  string jsonHolidayString = javaScriptHolidaySerializer.Serialize(jsonHolidaySub);
                                        strHoliday = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_Holiday(jsonHolidaySub, "Holiday", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strHoliday.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[Holiday Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strHoliday);
                                        }
                                    }
                                    else
                                    {
                                        strHoliday = "Success";
                                    }
                                }

                                if (strHoliday.ToLower() == "Success".ToLower())
                                {
                                    IsSynchHoliday = true;
                                }
                                else
                                {
                                    if (strHoliday.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchHoliday = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Holiday Sync (Local Database To Adit Server) ] : " + strHoliday);
                                        IsSynchHoliday = false;
                                    }
                                }
                            }
                        }
                    }
                    if (IsSynchHoliday)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday_Push");
                        GoalBase.WriteToSyncLogFile_Static("Holiday Sync (Local Database To Adit Server) Successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Holiday Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region ProviderOfficeHours

        public void SynchDataLiveDB_Push_ProviderOfficeHours()
        {
            try
            {
                if (IsProviderSyncPush) //&& Utility.AditLocationSyncEnable
                {
                    var JsonProviderOfficeHours = new System.Text.StringBuilder();
                    bool IsSyncProviderOfficeHours = true;
                    DataTable dtLocalProviderOfficeHours = SynchLocalBAL.GetPushLocalProviderOfficeHoursData();
                    if (dtLocalProviderOfficeHours.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalProviderOfficeHours, 200);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalProviderOfficeHours, Utility.mstSyncBatchSize.ProviderOfficeHours);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strHoliday = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    var JsonAppointment = new System.Text.StringBuilder();
                                    foreach (DataRow dtProviderOfficeHoursRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_ProviderOfficeHours ProviderOfficeHoursSub = new Push_ProviderOfficeHours
                                        {
                                            POH_EHR_ID = dtProviderOfficeHoursRow["POH_EHR_ID"].ToString().Trim(),
                                            POH_Web_ID = dtProviderOfficeHoursRow["POH_Web_ID"].ToString().Trim(),
                                            Provider_EHR_ID = dtProviderOfficeHoursRow["Provider_EHR_ID"].ToString().Trim(),
                                            WeekDay = dtProviderOfficeHoursRow["WeekDay"].ToString(),
                                            StartTime1 = Convert.ToDateTime(dtProviderOfficeHoursRow["StartTime1"]).ToString("HH:mm").ToString().Trim(),
                                            EndTime1 = Convert.ToDateTime(dtProviderOfficeHoursRow["EndTime1"]).ToString("HH:mm").ToString().Trim(),
                                            StartTime2 = Convert.ToDateTime(dtProviderOfficeHoursRow["StartTime2"]).ToString("HH:mm").ToString().Trim(),
                                            EndTime2 = Convert.ToDateTime(dtProviderOfficeHoursRow["EndTime2"]).ToString("HH:mm").ToString().Trim(),
                                            StartTime3 = Convert.ToDateTime(dtProviderOfficeHoursRow["StartTime3"]).ToString("HH:mm").ToString().Trim(),
                                            EndTime3 = Convert.ToDateTime(dtProviderOfficeHoursRow["EndTime3"]).ToString("HH:mm").ToString().Trim(),
                                            is_deleted = Convert.ToBoolean(dtProviderOfficeHoursRow["is_deleted"].ToString()),
                                            Location_ID = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.Location_ID,
                                            created_by = Utility.User_ID,
                                            ParentLocation_ID = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            Organization_ID = Utility.Organization_ID
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonProviderOfficeHours.Append(javaScriptSerializer.Serialize(ProviderOfficeHoursSub) + ",");
                                    }

                                    if (JsonProviderOfficeHours.Length > 0)
                                    {
                                        string jsonProviderOfficeHoursSub = "[" + JsonProviderOfficeHours.ToString().Remove(JsonProviderOfficeHours.Length - 1) + "]";

                                        //var javaScriptHolidaySerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        //  string jsonHolidayString = javaScriptHolidaySerializer.Serialize(jsonHolidaySub);

                                        strHoliday = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_ProviderOfficeHours(jsonProviderOfficeHoursSub, "ProviderOfficeHours", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strHoliday.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[ProviderOfficeHours Sync (Local Database To Adit Server) ] Service Install Id  :" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strHoliday);
                                        }
                                    }
                                    else
                                    {
                                        strHoliday = "Success";
                                    }
                                }

                                if (strHoliday.ToLower() == "Success".ToLower())
                                {
                                    IsSyncProviderOfficeHours = true;
                                }
                                else
                                {
                                    if (strHoliday.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSyncProviderOfficeHours = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[ProviderOfficeHours Sync (Local Database To Adit Server) ] : " + strHoliday);
                                        IsSyncProviderOfficeHours = false;
                                    }
                                }

                            }
                        }
                    }

                    if (IsSyncProviderOfficeHours)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderOfficeHours_Push");
                        GoalBase.WriteToSyncLogFile_Static("ProviderOfficeHours Sync (Local Database To Adit Server) Successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[ProviderOfficeHours Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region Comment

        //private BackgroundWorker bwSynchLiveDB_Push_Appointment = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_Appointment = null;

        //private BackgroundWorker bwSynchLiveDB_Push_OperatoryEvent = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_OperatoryEvent = null;

        //private BackgroundWorker bwSynchLiveDB_Push_Provider = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_Provider = null;

        //private BackgroundWorker bwSynchLiveDB_Push_Speciality = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_Speciality = null;

        //private BackgroundWorker bwSynchLiveDB_Push_Operatory = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_Operatory = null;

        //private BackgroundWorker bwSynchLiveDB_Push_ApptType = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_ApptType = null;

        //private BackgroundWorker bwSynchLiveDB_Push_Patient = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_Patient = null;

        //private BackgroundWorker bwSynchLiveDB_Push_RecallType = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_RecallType = null;

        //private BackgroundWorker bwSynchLiveDB_Push_ApptStatus = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_ApptStatus = null;

        //private BackgroundWorker bwSynchLiveDB_Push_Holiday = new BackgroundWorker();
        //private System.Timers.Timer timerSynchLiveDB_Push_Holiday = null;

        //private void CallSynchLiveDB_PushToLocal()
        //{
        //    if (Utility.AditSync)
        //    {

        //        //fncSyncDataLiveDB_Push_Provider();
        //        //timerSynchLiveDB_Push_Provider_Tick(null, null);

        //        //fncSyncDataLiveDB_Push_OperatoryEvent();
        //        //timerSynchLiveDB_Push_OperatoryEvent_Tick(null, null);

        //        //fncSyncDataLiveDB_Push_Speciality();
        //        //timerSynchLiveDB_Push_Speciality_Tick(null, null);

        //        //fncSyncDataLiveDB_Push_Operatory();
        //        //timerSynchLiveDB_Push_Operatory_Tick(null, null);

        //        //fncSyncDataLiveDB_Push_ApptType();
        //        //timerSynchLiveDB_Push_ApptType_Tick(null, null);

        //        //fncSyncDataLiveDB_Push_Appointment();
        //        //timerSynchLiveDB_Push_Appointment_Tick(null, null);

        //        ////fncSyncDataLiveDB_Push_Patient();
        //        ////timerSynchLiveDB_Push_Patient_Tick(null, null);

        //        //fncSyncDataLiveDB_Push_RecallType();
        //        //timerSynchLiveDB_Push_RecallType_Tick(null, null);

        //        //fncSyncDataLiveDB_Push_ApptStatus();
        //        //timerSynchLiveDB_Push_ApptStatus_Tick(null, null);
        //    }
        //}

        //private void fncSyncDataLiveDB_Push_Appointment()
        //{
        //    //  SynchDataLiveDB_Push_Appointment();
        //    InitBgWorkerLiveDB_Push_Appointment();
        //    InitBgTimerLiveDB_Push_Appointment();
        //}

        //private void InitBgTimerLiveDB_Push_Appointment()
        //{
        //    timerSynchLiveDB_Push_Appointment = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_Appointment.Interval = 1000 * GoalBase.intervalWebSynch_Push_Appointment;
        //    this.timerSynchLiveDB_Push_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_Appointment_Tick);
        //    timerSynchLiveDB_Push_Appointment.Enabled = true;
        //    timerSynchLiveDB_Push_Appointment.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_Appointment()
        //{
        //    bwSynchLiveDB_Push_Appointment.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_Appointment.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_Appointment.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_Appointment_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchLiveDB_Push_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_Appointment_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_Appointment_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_Appointment.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_Appointment();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_Appointment()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_Appointment);
        //    procThreadmainLiveDB_Push_Appointment.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_Appointment()
        //{
        //    if (bwSynchLiveDB_Push_Appointment.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_Appointment.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Appointment_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_Appointment.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }

        //        SynchDataLiveDB_Push_Appointment();

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_Appointment.Enabled = true;
        //}

        //private void fncSyncDataLiveDB_Push_OperatoryEvent()
        //{
        //    // SynchDataLiveDB_Push_OperatoryEvent();
        //    InitBgWorkerLiveDB_Push_OperatoryEvent();
        //    InitBgTimerLiveDB_Push_OperatoryEvent();
        //}

        //private void InitBgTimerLiveDB_Push_OperatoryEvent()
        //{
        //    timerSynchLiveDB_Push_OperatoryEvent = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_OperatoryEvent.Interval = 1000 * GoalBase.intervalWebSynch_Push_OperatoryEvent;
        //    this.timerSynchLiveDB_Push_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_OperatoryEvent_Tick);
        //    timerSynchLiveDB_Push_OperatoryEvent.Enabled = true;
        //    timerSynchLiveDB_Push_OperatoryEvent.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_OperatoryEvent()
        //{
        //    bwSynchLiveDB_Push_OperatoryEvent.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_OperatoryEvent.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_OperatoryEvent_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchLiveDB_Push_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_OperatoryEvent_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_OperatoryEvent_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_OperatoryEvent.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_OperatoryEvent();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_OperatoryEvent()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_OperatoryEvent);
        //    procThreadmainLiveDB_Push_OperatoryEvent.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_OperatoryEvent()
        //{
        //    if (bwSynchLiveDB_Push_OperatoryEvent.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_OperatoryEvent.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_OperatoryEvent.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_OperatoryEvent();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_OperatoryEvent.Enabled = true;
        //}
        //private void fncSyncDataLiveDB_Push_Provider()
        //{
        //    // SynchDataLiveDB_Push_Provider();
        //    InitBgWorkerLiveDB_Push_Provider();
        //    InitBgTimerLiveDB_Push_Provider();
        //}

        //private void InitBgTimerLiveDB_Push_Provider()
        //{

        //    timerSynchLiveDB_Push_Provider = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_Provider.Interval = 1000 * GoalBase.intervalWebSynch_Push_Provider;
        //    this.timerSynchLiveDB_Push_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_Provider_Tick);
        //    timerSynchLiveDB_Push_Provider.Enabled = true;
        //    timerSynchLiveDB_Push_Provider.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_Provider()
        //{
        //    bwSynchLiveDB_Push_Provider.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_Provider.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_Provider.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_Provider_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchLiveDB_Push_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_Provider_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_Provider_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_Provider.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_Provider();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_Provider()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_Provider = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_Provider);
        //    procThreadmainLiveDB_Push_Provider.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_Provider()
        //{
        //    if (bwSynchLiveDB_Push_Provider.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_Provider.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Provider_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_Provider.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_Provider();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_Provider.Enabled = true;
        //}


        //private void fncSyncDataLiveDB_Push_Speciality()
        //{
        //    // SynchDataLiveDB_Push_Speciality();
        //    InitBgWorkerLiveDB_Push_Speciality();
        //    InitBgTimerLiveDB_Push_Speciality();
        //}

        //private void InitBgTimerLiveDB_Push_Speciality()
        //{
        //    timerSynchLiveDB_Push_Speciality = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_Speciality.Interval = 1000 * GoalBase.intervalWebSynch_Push_Speciality;
        //    this.timerSynchLiveDB_Push_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_Speciality_Tick);
        //    timerSynchLiveDB_Push_Speciality.Enabled = true;
        //    timerSynchLiveDB_Push_Speciality.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_Speciality()
        //{
        //    bwSynchLiveDB_Push_Speciality.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_Speciality.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_Speciality.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_Speciality_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchLiveDB_Push_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_Speciality_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_Speciality_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_Speciality.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_Speciality();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_Speciality()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_Speciality = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_Speciality);
        //    procThreadmainLiveDB_Push_Speciality.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_Speciality()
        //{
        //    if (bwSynchLiveDB_Push_Speciality.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_Speciality.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Speciality_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_Speciality.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_Speciality();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_Speciality.Enabled = true;
        //}


        //private void fncSyncDataLiveDB_Push_Operatory()
        //{
        //    // SynchDataLiveDB_Push_Operatory();
        //    InitBgWorkerLiveDB_Push_Operatory();
        //    InitBgTimerLiveDB_Push_Operatory();
        //}

        //private void InitBgTimerLiveDB_Push_Operatory()
        //{
        //    timerSynchLiveDB_Push_Operatory = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_Operatory.Interval = 1000 * GoalBase.intervalWebSynch_Push_Operatory;
        //    this.timerSynchLiveDB_Push_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_Operatory_Tick);
        //    timerSynchLiveDB_Push_Operatory.Enabled = true;
        //    timerSynchLiveDB_Push_Operatory.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_Operatory()
        //{
        //    bwSynchLiveDB_Push_Operatory.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_Operatory.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_Operatory.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_Operatory_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchLiveDB_Push_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_Operatory_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_Operatory_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_Operatory.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_Operatory();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_Operatory()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_Operatory = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_Operatory);
        //    procThreadmainLiveDB_Push_Operatory.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_Operatory()
        //{
        //    if (bwSynchLiveDB_Push_Operatory.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_Operatory.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Operatory_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_Operatory.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_Operatory();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_Operatory.Enabled = true;
        //}


        //private void fncSyncDataLiveDB_Push_ApptType()
        //{
        //    InitBgWorkerLiveDB_Push_ApptType();
        //    InitBgTimerLiveDB_Push_ApptType();
        //}

        //private void InitBgTimerLiveDB_Push_ApptType()
        //{
        //    timerSynchLiveDB_Push_ApptType = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_ApptType.Interval = 1000 * GoalBase.intervalWebSynch_Push_ApptType;
        //    this.timerSynchLiveDB_Push_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_ApptType_Tick);
        //    timerSynchLiveDB_Push_ApptType.Enabled = true;
        //    timerSynchLiveDB_Push_ApptType.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_ApptType()
        //{
        //    bwSynchLiveDB_Push_ApptType.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_ApptType.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_ApptType.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_ApptType_DoWork);
        //    bwSynchLiveDB_Push_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_ApptType_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_ApptType_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_ApptType.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_ApptType();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_ApptType()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_ApptType = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_ApptType);
        //    procThreadmainLiveDB_Push_ApptType.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_ApptType()
        //{
        //    if (bwSynchLiveDB_Push_ApptType.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_ApptType.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_ApptType_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_ApptType.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_ApptType();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_ApptType.Enabled = true;
        //}


        //private void fncSyncDataLiveDB_Push_Patient()
        //{
        //    // SynchDataLiveDB_Push_Patient();
        //    InitBgWorkerLiveDB_Push_Patient();
        //    InitBgTimerLiveDB_Push_Patient();
        //}

        //private void InitBgTimerLiveDB_Push_Patient()
        //{
        //    timerSynchLiveDB_Push_Patient = new System.Timers.Timer();

        //    this.timerSynchLiveDB_Push_Patient.Interval = 1000 * GoalBase.intervalWebSynch_Push_Patient;
        //    this.timerSynchLiveDB_Push_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_Patient_Tick);
        //    timerSynchLiveDB_Push_Patient.Enabled = true;
        //    timerSynchLiveDB_Push_Patient.Start();
        //    timerSynchLiveDB_Push_Patient_Tick(null, null);
        //}

        //private void InitBgWorkerLiveDB_Push_Patient()
        //{
        //    bwSynchLiveDB_Push_Patient.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_Patient.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_Patient.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_Patient_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchLiveDB_Push_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_Patient_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_Patient_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_Patient.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_Patient();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_Patient()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_Patient = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_Patient);
        //    procThreadmainLiveDB_Push_Patient.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_Patient()
        //{
        //    if (bwSynchLiveDB_Push_Patient.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_Patient.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Patient_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_Patient.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_Patient();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_Patient.Enabled = true;
        //}

        //private void fncSyncDataLiveDB_Push_RecallType()
        //{
        //    InitBgWorkerLiveDB_Push_RecallType();
        //    InitBgTimerLiveDB_Push_RecallType();
        //}

        //private void InitBgTimerLiveDB_Push_RecallType()
        //{
        //    timerSynchLiveDB_Push_RecallType = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_RecallType.Interval = 1000 * GoalBase.intervalWebSynch_Push_RecallType;
        //    this.timerSynchLiveDB_Push_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_RecallType_Tick);
        //    timerSynchLiveDB_Push_RecallType.Enabled = true;
        //    timerSynchLiveDB_Push_RecallType.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_RecallType()
        //{
        //    bwSynchLiveDB_Push_RecallType.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_RecallType.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_RecallType.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_RecallType_DoWork);
        //    bwSynchLiveDB_Push_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_RecallType_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_RecallType_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_RecallType.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_RecallType();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_RecallType()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_RecallType = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_RecallType);
        //    procThreadmainLiveDB_Push_RecallType.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_RecallType()
        //{
        //    if (bwSynchLiveDB_Push_RecallType.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_RecallType.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_RecallType_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_RecallType.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_RecallType();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_RecallType.Enabled = true;
        //}


        //private void fncSyncDataLiveDB_Push_ApptStatus()
        //{
        //    InitBgWorkerLiveDB_Push_ApptStatus();
        //    InitBgTimerLiveDB_Push_ApptStatus();
        //}

        //private void InitBgTimerLiveDB_Push_ApptStatus()
        //{
        //    timerSynchLiveDB_Push_ApptStatus = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_ApptStatus.Interval = 1000 * GoalBase.intervalWebSynch_Push_ApptStatus;
        //    this.timerSynchLiveDB_Push_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_ApptStatus_Tick);
        //    timerSynchLiveDB_Push_ApptStatus.Enabled = true;
        //    timerSynchLiveDB_Push_ApptStatus.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_ApptStatus()
        //{
        //    bwSynchLiveDB_Push_ApptStatus.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_ApptStatus.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_ApptStatus_DoWork);
        //    bwSynchLiveDB_Push_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_ApptStatus_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_ApptStatus_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_ApptStatus.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_ApptStatus();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_ApptStatus()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_ApptStatus);
        //    procThreadmainLiveDB_Push_ApptStatus.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_ApptStatus()
        //{
        //    if (bwSynchLiveDB_Push_ApptStatus.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_ApptStatus.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_ApptStatus.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_ApptStatus();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_ApptStatus.Enabled = true;
        //}


        //private void fncSyncDataLiveDB_Push_Holiday()
        //{
        //    InitBgWorkerLiveDB_Push_Holiday();
        //    InitBgTimerLiveDB_Push_Holiday();
        //}

        //private void InitBgTimerLiveDB_Push_Holiday()
        //{
        //    timerSynchLiveDB_Push_Holiday = new System.Timers.Timer();
        //    this.timerSynchLiveDB_Push_Holiday.Interval = 1000 * GoalBase.intervalWebSynch_Push_Holiday;
        //    this.timerSynchLiveDB_Push_Holiday.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Push_Holiday_Tick);
        //    timerSynchLiveDB_Push_Holiday.Enabled = true;
        //    timerSynchLiveDB_Push_Holiday.Start();
        //}

        //private void InitBgWorkerLiveDB_Push_Holiday()
        //{
        //    bwSynchLiveDB_Push_Holiday.WorkerReportsProgress = true;
        //    bwSynchLiveDB_Push_Holiday.WorkerSupportsCancellation = true;
        //    bwSynchLiveDB_Push_Holiday.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Push_Holiday_DoWork);
        //    bwSynchLiveDB_Push_Holiday.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Push_Holiday_RunWorkerCompleted);
        //}

        //private void timerSynchLiveDB_Push_Holiday_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLiveDB_Push_Holiday.Enabled = false;
        //        MethodForCallSynchOrderLiveDB_Push_Holiday();
        //    }
        //}

        //public void MethodForCallSynchOrderLiveDB_Push_Holiday()
        //{
        //    System.Threading.Thread procThreadmainLiveDB_Push_Holiday = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Push_Holiday);
        //    procThreadmainLiveDB_Push_Holiday.Start();
        //}

        //public void CallSyncOrderTableLiveDB_Push_Holiday()
        //{
        //    if (bwSynchLiveDB_Push_Holiday.IsBusy != true)
        //    {
        //        bwSynchLiveDB_Push_Holiday.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Holiday_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLiveDB_Push_Holiday.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLiveDB_Push_Holiday();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchLiveDB_Push_Holiday_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLiveDB_Push_Holiday.Enabled = true;
        //}

        #endregion

        #region Medical History

        public static void SynchDataLiveDB_Push_MedicalHisotryTables(string tablename)
        {
            //if (Utility.AditLocationSyncEnable)
            //{
            //Utility.WriteToSyncLogFile_All("Start Push " + tablename.ToString());
            if (tablename.ToString() == "EagleSoftFormMaster")
            {
                SynchDataLiveDB_Push_FormMaster();
            }
            else if (tablename.ToString() == "EagleSoftSectionMaster")
            {
                SynchDataLiveDB_Push_SectionMaster();
            }
            else if (tablename.ToString() == "EagleSoftAlertMaster")
            {
                SynchDataLiveDB_Push_AlertMaster();
            }
            else if (tablename.ToString() == "EagleSoftSectionItemMaster")
            {
                SynchDataLiveDB_Push_EagleSoftSectionItemMaster();
            }
            else if (tablename.ToString() == "EagleSoftSectionItemOptionMaster")
            {
                SynchDataLiveDB_Push_EagleSoftSectionItemOptionMaster();
            }
            else if (tablename.ToString() == "Dentrix_Form")
            {
                SynchDataLiveDB_Push_Dentrix_FormMaster();
            }
            else if (tablename.ToString() == "Dentrix_FormQuestion")
            {
                SynchDataLiveDB_Push_Dentrix_FormQuestionMaster();
            }
            else if (tablename.ToString() == "AbelDent_Form")
            {
                SynchDataLiveDB_Push_AbelDent_FormMaster();
            }
            else if (tablename.ToString() == "AbelDent_FormQuestion")
            {
                SynchDataLiveDB_Push_AbelDent_FormQuestionMaster();
            }
            else if (tablename.ToString().ToUpper() == "OD_SHEETDEF")
            {
                SynchDataLiveDB_Push_OpenDentalSheet("OD_SHEETDEF");
            }
            else if (tablename.ToString().ToUpper() == "OD_SHEETFIELDDEF")
            {
                SynchDataLiveDB_Push_OpenDentalSheetField("OD_SHEETFIELDDEF");
            }
            else if (tablename.ToString().ToUpper() == "CD_FORMMASTER")
            {
                SynchDataLiveDB_Push_CD_FormMaster();
            }
            else if (tablename.ToString().ToUpper() == "CD_QUESTIONMASTER")
            {
                SynchDataLiveDB_Push_CD_QuestionMaster();
            }
            else if (tablename.ToString().ToUpper() == "EASYDENTAL_QUESTION")
            {
                SynchDataLiveDB_Push_EasyDental_Question();
            }
            else if (tablename.ToString().ToUpper() == "EASYDENTAL_FORM")
            {
                SynchDataLiveDB_Push_EasyDental_Form();
            }
            else if (tablename.ToString().ToUpper() == "PRACTICEWEB_SHEETDEF")
            {
                SynchDataLiveDB_Push_OpenDentalSheet("PRACTICEWEB_SHEETDEF");
            }
            else if (tablename.ToString().ToUpper() == "PRACTICEWEB_SHEETFIELDDEF")
            {
                SynchDataLiveDB_Push_OpenDentalSheetField("PRACTICEWEB_SHEETFIELDDEF");
            }
            //}
        }

        private static void SynchDataLiveDB_Push_FormMaster()
        {
            try
            {
                var JsonFormMaster = new System.Text.StringBuilder();
                bool IsSynchFormMaster = true;
                for (int k = 0; k < Utility.DtInstallServiceList.Rows.Count; k++)
                {
                    DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("EagleSoftFormMaster", Utility.DtInstallServiceList.Rows[k]["Installation_ID"].ToString());

                    if (dtLocalFormMaster.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.FormMaster);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strFormMaster = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id  = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_FormMasterBO FormMaster = new Push_FormMasterBO
                                        {
                                            appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                            location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            name = dtFormMasterRow["FormName"].ToString().Replace(",", "").Trim(),
                                            organization = Utility.Organization_ID,
                                            formmaster_ehr_id = dtFormMasterRow["Form_EHR_Id"].ToString().Trim(),
                                            formmaster_localdb_id = dtFormMasterRow["FormMaster_LocalDB_ID"].ToString().Trim(),
                                            Formmaster_Web_ID = dtFormMasterRow["FormMaster_web_Id"].ToString().Trim(),
                                            is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim()),
                                            is_default = Convert.ToBoolean(dtFormMasterRow["Is_Default"].ToString().Trim()),
                                            form_type_id = dtFormMasterRow["Form_Type_Id"].ToString().Trim(),
                                            is_active = Convert.ToBoolean(dtFormMasterRow["Is_Active"].ToString().Trim())
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                    }

                                    if (JsonFormMaster.Length > 0)
                                    {
                                        string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                        strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "EagleSoftFormMaster", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_ID"].ToString());

                                        if (strFormMaster.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[FormMaster Sync (Local Database To Adit Server) ] Service Install Id :  " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                        }
                                    }
                                    else
                                    {
                                        strFormMaster = "Success";
                                    }
                                }

                                if (strFormMaster.ToLower() == "Success".ToLower())
                                {
                                    IsSynchFormMaster = true;
                                }
                                else
                                {
                                    if (strFormMaster.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchFormMaster = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[FormMaster Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                        IsSynchFormMaster = false;
                                    }
                                }

                                if (IsSynchFormMaster)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FormMaster_Push");
                                    GoalBase.WriteToSyncLogFile_Static("FormMaster Sync (Local Database To Adit Server) Successfully.");
                                    IsMedicalHistorySyncPush = true;
                                }
                                else
                                {
                                    IsMedicalHistorySyncPush = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[FormMaster  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_SectionMaster()
        {
            try
            {
                //Utility.WriteToSyncLogFile_All("Start Push SynchDataLiveDB_Push_SectionMaster ");

                var JsonSectionMaster = new System.Text.StringBuilder();
                bool IsSynchSectionMaster = true;
                for (int k = 0; k < Utility.DtInstallServiceList.Rows.Count; k++)
                {
                    // Utility.WriteToSyncLogFile_All("Start Push Loop SynchDataLiveDB_Push_SectionMaster ");

                    DataTable dtLocalSectionMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("EagleSoftSectionMaster", Utility.DtInstallServiceList.Rows[k]["Installation_ID"].ToString());



                    if (dtLocalSectionMaster.Rows.Count > 0)
                    {
                        // Utility.WriteToSyncLogFile_All("Start Push Count > 0 SynchDataLiveDB_Push_SectionMaster ");

                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalSectionMaster, 100);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalSectionMaster, Utility.mstSyncBatchSize.SectionMaster);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strSectionMaster = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    foreach (DataRow dtSectionMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id  = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_SectionMasterBO SectionMaster = new Push_SectionMasterBO
                                        {
                                            appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                            location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            name = dtSectionMasterRow["SectionName"].ToString().Replace(",", "").Trim(),
                                            organization = Utility.Organization_ID,
                                            sectionmaster_ehr_id = dtSectionMasterRow["Section_EHR_Id"].ToString().Trim(),
                                            sectionmaster_localdb_id = dtSectionMasterRow["SectionMaster_LocalDB_ID"].ToString().Trim(),
                                            sectionmaster_Web_Id = dtSectionMasterRow["SectionMaster_web_Id"].ToString().Trim(),
                                            is_deleted = Convert.ToBoolean(dtSectionMasterRow["is_deleted"].ToString().Trim()),
                                            show_border = Convert.ToBoolean(dtSectionMasterRow["Show_Border"].ToString().Trim()),
                                            formmaster_ehr_id = dtSectionMasterRow["Form_EHR_Id"].ToString().Trim(),
                                            show_title = Convert.ToBoolean(dtSectionMasterRow["Show_Title"].ToString().Trim()),
                                            section_order = Convert.ToInt16(dtSectionMasterRow["Section_Order"].ToString().Trim()),
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonSectionMaster.Append(javaScriptSerializer.Serialize(SectionMaster) + ",");
                                    }

                                    if (JsonSectionMaster.Length > 0)
                                    {
                                        string jsonString = "[" + JsonSectionMaster.ToString().Remove(JsonSectionMaster.Length - 1) + "]";

                                        // Utility.WriteToSyncLogFile_All("Start Push SynchDataLiveDB_Push_SectionMaster Json " + jsonString);

                                        strSectionMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "EagleSoftSectionMaster", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strSectionMaster.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[SectionMaster Sync (Local Database To Adit Server) ]Service Install Id :  " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strSectionMaster);
                                        }
                                    }
                                    else
                                    {
                                        strSectionMaster = "Success";
                                    }
                                }

                                if (strSectionMaster.ToLower() == "Success".ToLower())
                                {
                                    IsSynchSectionMaster = true;
                                }
                                else
                                {
                                    if (strSectionMaster.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchSectionMaster = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[SectionMaster Sync (Local Database To Adit Server) ] : " + strSectionMaster);
                                        IsSynchSectionMaster = false;
                                    }
                                }

                                if (IsSynchSectionMaster)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SectionMaster_Push");
                                    GoalBase.WriteToSyncLogFile_Static("SectionMaster Sync (Local Database To Adit Server) Successfully.");
                                    IsMedicalHistorySyncPush = true;
                                }
                                else
                                {
                                    IsMedicalHistorySyncPush = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[SectionMaster  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_AlertMaster()
        {
            try
            {
                var JsonAlertMaster = new System.Text.StringBuilder();
                bool IsSynchAlertMaster = true;
                for (int k = 0; k < Utility.DtInstallServiceList.Rows.Count; k++)
                {
                    DataTable dtLocalAlertMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("EagleSoftAlertMaster", Utility.DtInstallServiceList.Rows[k]["Installation_ID"].ToString());

                    if (dtLocalAlertMaster.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalAlertMaster, 100);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalAlertMaster, Utility.mstSyncBatchSize.AlertMaster);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strAlertMaster = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    foreach (DataRow dtAlertMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id  = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_AlertMasterBO AlertMaster = new Push_AlertMasterBO
                                        {
                                            appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                            location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            name = dtAlertMasterRow["Alert_Name"].ToString().Replace(",", "").Trim(),
                                            organization = Utility.Organization_ID,
                                            alertmaster_ehr_id = dtAlertMasterRow["Alert_EHR_Id"].ToString().Trim(),
                                            alertmaster_localdb_id = dtAlertMasterRow["AlertMaster_LocalDB_ID"].ToString().Trim(),
                                            is_deleted = Convert.ToBoolean(dtAlertMasterRow["is_deleted"].ToString().Trim()),
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonAlertMaster.Append(javaScriptSerializer.Serialize(AlertMaster) + ",");
                                    }

                                    if (JsonAlertMaster.Length > 0)
                                    {
                                        string jsonString = "[" + JsonAlertMaster.ToString().Remove(JsonAlertMaster.Length - 1) + "]";
                                        strAlertMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "EagleSoftAlertMaster", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strAlertMaster.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[AlertMaster Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strAlertMaster);
                                        }
                                    }
                                    else
                                    {
                                        strAlertMaster = "Success";
                                    }
                                }

                                if (strAlertMaster.ToLower() == "Success".ToLower())
                                {
                                    IsSynchAlertMaster = true;
                                }
                                else
                                {
                                    if (strAlertMaster.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchAlertMaster = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[AlertMaster Sync (Local Database To Adit Server) ] : " + strAlertMaster);
                                        IsSynchAlertMaster = false;
                                    }
                                }

                                if (IsSynchAlertMaster)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("AlertMaster_Push");
                                    GoalBase.WriteToSyncLogFile_Static("AlertMaster Sync (Local Database To Adit Server) Successfully.");
                                    IsMedicalHistorySyncPush = true;
                                }
                                else
                                {
                                    IsMedicalHistorySyncPush = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[AlertMaster  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_EagleSoftSectionItemMaster()
        {
            try
            {
                var JsonSectionItemMaster = new System.Text.StringBuilder();
                bool IsSynchSectionItemMaster = true;
                for (int k = 0; k < Utility.DtInstallServiceList.Rows.Count; k++)
                {
                    // DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();
                    DataTable dtLocalSectionItemMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("EagleSoftSectionItemMaster", Utility.DtInstallServiceList.Rows[k]["Installation_ID"].ToString());

                    if (dtLocalSectionItemMaster.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalSectionItemMaster, 100);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalSectionItemMaster, Utility.mstSyncBatchSize.SectionItemMaster);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strSectionItemMaster = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    foreach (DataRow dtSectionItemMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id  = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_SectionItemMasterBO SectionItemMaster = new Push_SectionItemMasterBO
                                        {
                                            appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                            name = dtSectionItemMasterRow["SectionItemName"].ToString().Replace(",", "").Trim(),
                                            organization = Utility.Organization_ID,
                                            sectionitemtype = Convert.ToInt16(dtSectionItemMasterRow["SectionItemType"].ToString()),
                                            sectionitemorder = Convert.ToInt16(dtSectionItemMasterRow["SectionItemOrder"].ToString()),
                                            allowcomment = Convert.ToBoolean(dtSectionItemMasterRow["AllowComment"].ToString()),
                                            alertonyes = Convert.ToBoolean(dtSectionItemMasterRow["AlertOnYes"].ToString()),
                                            alertonno = Convert.ToBoolean(dtSectionItemMasterRow["AlertOnNo"].ToString()),
                                            alertmaster_ehr_id = dtSectionItemMasterRow["Alert_EHR_Id"].ToString(),
                                            question_type = dtSectionItemMasterRow["Question_Type"].ToString(),
                                            answer_type = dtSectionItemMasterRow["Answer_Type"].ToString(),
                                            alloweditcomment = Convert.ToBoolean(dtSectionItemMasterRow["AllowEditComment"].ToString()),
                                            numberofcolumns = dtSectionItemMasterRow["NumberOfColumns"].ToString(),
                                            formmaster_ehr_id = dtSectionItemMasterRow["Form_EHR_Id"].ToString(),
                                            sectionmaster_ehr_id = dtSectionItemMasterRow["Section_EHR_Id"].ToString(),
                                            sectionitemmaster_ehr_id = dtSectionItemMasterRow["SectionItem_EHR_Id"].ToString(),
                                            sectionitemmaster_localdb_id = dtSectionItemMasterRow["SectionItemMaster_LocalDB_ID"].ToString(),
                                            location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            is_deleted = Convert.ToBoolean(dtSectionItemMasterRow["is_deleted"].ToString())
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonSectionItemMaster.Append(javaScriptSerializer.Serialize(SectionItemMaster) + ",");

                                    }

                                    if (JsonSectionItemMaster.Length > 0)
                                    {
                                        string jsonString = "[" + JsonSectionItemMaster.ToString().Remove(JsonSectionItemMaster.Length - 1) + "]";
                                        //string strSectionItemMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "SectionItemMaster");
                                        strSectionItemMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "EagleSoftSectionItemMaster", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strSectionItemMaster.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[SectionItemMaster Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strSectionItemMaster);
                                        }
                                    }
                                    else
                                    {
                                        strSectionItemMaster = "Success";
                                    }
                                }

                                if (strSectionItemMaster.ToLower() == "Success".ToLower())
                                {
                                    IsSynchSectionItemMaster = true;
                                }
                                else
                                {
                                    if (strSectionItemMaster.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchSectionItemMaster = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[SectionItemMaster Sync (Local Database To Adit Server) ] : " + strSectionItemMaster);
                                        IsSynchSectionItemMaster = false;
                                    }
                                }
                                if (IsSynchSectionItemMaster)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SectionItemMaster_Push");
                                    GoalBase.WriteToSyncLogFile_Static("SectionItemMaster Sync (Local Database To Adit Server) Successfully.");
                                    IsMedicalHistorySyncPush = true;
                                }
                                else
                                {
                                    IsMedicalHistorySyncPush = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[SectionItemMaster  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_EagleSoftSectionItemOptionMaster()
        {
            try
            {
                var JsonSectionItemOptionMaster = new System.Text.StringBuilder();
                bool IsSynchSectionItemOptionMaster = true;
                for (int k = 0; k < Utility.DtInstallServiceList.Rows.Count; k++)
                {
                    // DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();
                    DataTable dtLocalSectionItemOptionMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("EagleSoftSectionItemOptionMaster", Utility.DtInstallServiceList.Rows[k]["Installation_ID"].ToString());

                    if (dtLocalSectionItemOptionMaster.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalSectionItemOptionMaster, 100);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalSectionItemOptionMaster, Utility.mstSyncBatchSize.SectionItemOptionMaster);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strSectionItemOptionMaster = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    foreach (DataRow dtSectionItemOptionMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id  = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_SectionItemOptionMasterBO SectionItemOptionMaster = new Push_SectionItemOptionMasterBO
                                        {
                                            appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                            name = dtSectionItemOptionMasterRow["SectionItemOptionName"].ToString().Replace(",", "").Trim(),
                                            organization = Utility.Organization_ID,
                                            sectionitemoptionmaster_ehr_id = dtSectionItemOptionMasterRow["SectionItemOption_EHR_Id"].ToString(),
                                            sectionitemoptionmaster_localdb_id = dtSectionItemOptionMasterRow["SectionItemOptionMaster_LocalDB_ID"].ToString(),
                                            formmaster_ehr_id = dtSectionItemOptionMasterRow["Form_EHR_Id"].ToString(),
                                            sectionmaster_ehr_id = dtSectionItemOptionMasterRow["Section_EHR_Id"].ToString(),
                                            sectionitemmaster_ehr_id = dtSectionItemOptionMasterRow["SectionItem_EHR_Id"].ToString(),
                                            question_type = dtSectionItemOptionMasterRow["Question_Type"].ToString(),
                                            answer_type = dtSectionItemOptionMasterRow["Answer_Type"].ToString(),

                                            numberofcolumns = dtSectionItemOptionMasterRow["NumberOfColumns"].ToString(),

                                            location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            is_deleted = Convert.ToBoolean(dtSectionItemOptionMasterRow["is_deleted"].ToString())
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonSectionItemOptionMaster.Append(javaScriptSerializer.Serialize(SectionItemOptionMaster) + ",");
                                    }

                                    if (JsonSectionItemOptionMaster.Length > 0)
                                    {
                                        string jsonString = "[" + JsonSectionItemOptionMaster.ToString().Remove(JsonSectionItemOptionMaster.Length - 1) + "]";
                                        //string strSectionItemOptionMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "SectionItemOptionMaster");
                                        strSectionItemOptionMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "EagleSoftSectionItemOptionMaster", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strSectionItemOptionMaster.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[SectionItemOptionMaster Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strSectionItemOptionMaster);
                                        }
                                    }
                                    else
                                    {
                                        strSectionItemOptionMaster = "Success";
                                    }
                                }

                                if (strSectionItemOptionMaster.ToLower() == "Success".ToLower())
                                {
                                    IsSynchSectionItemOptionMaster = true;
                                }
                                else
                                {
                                    if (strSectionItemOptionMaster.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchSectionItemOptionMaster = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[SectionItemOptionMaster Sync (Local Database To Adit Server) ] : " + strSectionItemOptionMaster);
                                        IsSynchSectionItemOptionMaster = false;
                                    }
                                }

                                if (IsSynchSectionItemOptionMaster)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SectionItemOptionMaster_Push");
                                    GoalBase.WriteToSyncLogFile_Static("SectionItemOptionMaster Sync (Local Database To Adit Server) Successfully.");
                                    IsMedicalHistorySyncPush = true;
                                }
                                else
                                {
                                    IsMedicalHistorySyncPush = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[SectionItemOptionMaster  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_Dentrix_FormMaster()
        {
            try
            {

                bool IsSynchFormMaster = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("Dentrix_Form", "1");

                if (dtLocalFormMaster.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.FormMaster);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strFormMaster = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonFormMaster = new System.Text.StringBuilder();
                                foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "'"))
                                {
                                    Push_Dentrix_Form FormMaster = new Push_Dentrix_Form
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        name = dtFormMasterRow["Dentrix_Form_Name"].ToString().Replace(",", "").Trim(),
                                        organization = Utility.Organization_ID,
                                        version = dtFormMasterRow["Version"].ToString().Trim(),
                                        version_date = Convert.ToDateTime((dtFormMasterRow["Version_Date"].ToString().Trim() == "" || dtFormMasterRow["Version_Date"].ToString().Trim() == string.Empty) ? new DateTime(2001, 01, 01).ToString() : dtFormMasterRow["Version_Date"].ToString().Trim()).ToString("yyyy-MM-dd"),
                                        formrespondenttype = dtFormMasterRow["FormRespondentType"].ToString().Trim(),
                                        categoryid = dtFormMasterRow["CategoryId"].ToString().Trim(),
                                        formflags = dtFormMasterRow["FormFlags"].ToString().Trim(),
                                        monthtoexpiration = dtFormMasterRow["MonthtoExpiration"].ToString().Trim(),
                                        dentrix_form_localdb_id = dtFormMasterRow["Dentrix_Form_LocalDB_ID"].ToString().Trim(),
                                        dentrix_form_ehrunique_id = dtFormMasterRow["Dentrix_Form_EHRUnique_ID"].ToString().Trim(),
                                        dentrix_form_ehr_id = dtFormMasterRow["Dentrix_Form_EHR_ID"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim()),
                                        is_active = Convert.ToBoolean(dtFormMasterRow["Is_Active"].ToString().Trim())

                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                }
                                if (JsonFormMaster.Length > 0)
                                {
                                    string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                    strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "Dentrix_Form", "1", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strFormMaster.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Dentrix_Form Sync (Local Database To Adit Server) ] Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                    }
                                }
                                else
                                {
                                    strFormMaster = "Success";
                                }
                            }

                            if (strFormMaster.ToLower() == "Success".ToLower())
                            {
                                IsSynchFormMaster = true;
                            }
                            else
                            {
                                if (strFormMaster.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchFormMaster = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Dentrix_Form Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                    IsSynchFormMaster = false;
                                }
                            }

                            if (IsSynchFormMaster)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FormMaster_Push");
                                GoalBase.WriteToSyncLogFile_Static("Dentrix_Form Sync (Local Database To Adit Server) Successfully.");
                                IsMedicalHistorySyncPush = true;
                            }
                            else
                            {
                                IsMedicalHistorySyncPush = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Dentrix_Form  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_Dentrix_FormQuestionMaster()
        {
            try
            {
                var JsonFormMaster = new System.Text.StringBuilder();
                bool IsSynchFormMaster = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("Dentrix_FormQuestion", "1");

                if (dtLocalFormMaster.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.FormQuestionMaster);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strFormMaster = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                JsonFormMaster = new System.Text.StringBuilder();
                                foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "'"))
                                {
                                    Push_Dentrix_FormQuestion FormMaster = new Push_Dentrix_FormQuestion
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        organization = Utility.Organization_ID,
                                        questionstypeid = dtFormMasterRow["Dentrix_QuestionsTypeId"].ToString().Trim(),
                                        questionstypename = dtFormMasterRow["Dentrix_QyestionTypeName"].ToString().Trim(),
                                        questionversion_date = Convert.ToDateTime(dtFormMasterRow["QuestionVersion_Date"].ToString().Trim()).ToString("yyyy-MM-dd"),
                                        responsetypeid = dtFormMasterRow["Dentrix_ResponsetypeId"].ToString().Trim(),
                                        questionname = dtFormMasterRow["Dentrix_QuestionName"].ToString().Trim(),
                                        question_defaultvalue = dtFormMasterRow["Dentrix_Question_DefaultValue"].ToString().Trim(),
                                        questionversion = dtFormMasterRow["QuestionVersion"].ToString().Trim(),
                                        inputtype = dtFormMasterRow["InputType"].ToString().Trim(),
                                        options = dtFormMasterRow["Options"].ToString().Trim(),
                                        questionorder = dtFormMasterRow["QuestionOrder"].ToString().Trim(),
                                        answer_value = dtFormMasterRow["Answer_Value"].ToString().Trim(),
                                        dentrix_formquestion_ehr_id = dtFormMasterRow["Dentrix_Question_EHR_ID"].ToString().Trim(),
                                        dentrix_formquestion_localdb_id = dtFormMasterRow["Dentrix_FormQuestion_LocalDB_ID"].ToString().Trim(),
                                        dentrix_form_ehrunique_id = dtFormMasterRow["Dentrix_Form_EHRUnique_ID"].ToString().Trim(),
                                        dentrix_formquestion_ehrunique_id = dtFormMasterRow["Dentrix_Question_EHRUnique_ID"].ToString().Trim(),
                                        is_multifield = Convert.ToBoolean(dtFormMasterRow["Is_MultiField"].ToString().Trim()),
                                        is_optionfiled = Convert.ToBoolean(dtFormMasterRow["Is_OptionField"].ToString().Trim()),
                                        is_required = Convert.ToBoolean(dtFormMasterRow["Is_Required"].ToString().Trim()),
                                        is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim())
                                        //  is_active = Convert.ToBoolean(dtFormMasterRow["Is_Active"].ToString().Trim())

                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                }

                                if (JsonFormMaster.Length > 0)
                                {
                                    string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                    strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "Dentrix_FormQuestion", "1", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strFormMaster.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Dentrix_FormQuestion Sync (Local Database To Adit Server) ] Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                    }
                                }
                                else
                                {
                                    strFormMaster = "Success";
                                }
                            }

                            if (strFormMaster.ToLower() == "Success".ToLower())
                            {
                                IsSynchFormMaster = true;
                            }
                            else
                            {
                                if (strFormMaster.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchFormMaster = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Dentrix_FormQuestion Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                    IsSynchFormMaster = false;
                                }
                            }

                            if (IsSynchFormMaster)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FormQuestionMaster_Push");

                                IsMedicalHistorySyncPush = true;
                            }
                            else
                            {
                                IsMedicalHistorySyncPush = false;
                            }
                        }
                    }
                }
                GoalBase.WriteToSyncLogFile_Static("Dentrix_FormQuestion Sync (Local Database To Adit Server) Successfully.");
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Dentrix_FormQuestion  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_AbelDent_FormMaster()
        {
            try
            {

                bool IsSynchFormMaster = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("AbelDent_Form", "1");

                if (dtLocalFormMaster.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.FormMaster);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strFormMaster = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonFormMaster = new System.Text.StringBuilder();
                                foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "'"))
                                {
                                    Push_AbelDent_Form FormMaster = new Push_AbelDent_Form
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        name = dtFormMasterRow["AbelDent_Form_Name"].ToString().Replace(",", "").Trim(),
                                        organization = Utility.Organization_ID,
                                        version = dtFormMasterRow["Version"].ToString().Trim(),
                                        version_date = Convert.ToDateTime((dtFormMasterRow["Version_Date"].ToString().Trim() == "" || dtFormMasterRow["Version_Date"].ToString().Trim() == string.Empty) ? new DateTime(2001, 01, 01).ToString() : dtFormMasterRow["Version_Date"].ToString().Trim()).ToString("yyyy-MM-dd"),
                                        formrespondenttype = dtFormMasterRow["FormRespondentType"].ToString().Trim(),
                                        categoryid = dtFormMasterRow["CategoryId"].ToString().Trim(),
                                        formflags = dtFormMasterRow["FormFlags"].ToString().Trim(),
                                        monthtoexpiration = dtFormMasterRow["MonthtoExpiration"].ToString().Trim(),
                                        abeldent_form_localdb_id = dtFormMasterRow["AbelDent_Form_LocalDB_ID"].ToString().Trim(),
                                        abeldent_form_ehrunique_id = dtFormMasterRow["AbelDent_Form_EHRUnique_ID"].ToString().Trim(),
                                        abeldent_form_ehr_id = dtFormMasterRow["AbelDent_Form_EHR_ID"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim()),
                                        is_active = Convert.ToBoolean(dtFormMasterRow["Is_Active"].ToString().Trim())
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                }
                                if (JsonFormMaster.Length > 0)
                                {
                                    string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                    strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "abeldent_form", "1", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strFormMaster.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[AbelDent_Form Sync (Local Database To Adit Server) ] Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                    }
                                }
                                else
                                {
                                    strFormMaster = "Success";
                                }
                            }

                            if (strFormMaster.ToLower() == "Success".ToLower())
                            {
                                IsSynchFormMaster = true;
                            }
                            else
                            {
                                if (strFormMaster.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchFormMaster = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[AbelDentMedical_Form Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                    IsSynchFormMaster = false;
                                }
                            }

                            if (IsSynchFormMaster)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FormMaster_Push");
                                GoalBase.WriteToSyncLogFile_Static("AbelDentMedical_Form Sync (Local Database To Adit Server) Successfully.");
                                IsMedicalHistorySyncPush = true;
                            }
                            else
                            {
                                IsMedicalHistorySyncPush = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[AbelDent_Form  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_AbelDent_FormQuestionMaster()
        {
            try
            {
                var JsonFormMaster = new System.Text.StringBuilder();
                bool IsSynchFormMaster = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("AbelDent_FormQuestion", "1");

                if (dtLocalFormMaster.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.FormQuestionMaster);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strFormMaster = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                JsonFormMaster = new System.Text.StringBuilder();
                                foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "'"))
                                {
                                    Push_AbelDent_FormQuestion FormMaster = new Push_AbelDent_FormQuestion
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        organization = Utility.Organization_ID,
                                        questionstypeid = dtFormMasterRow["AbelDent_QuestionsTypeId"].ToString().Trim(),
                                        questionstypename = dtFormMasterRow["AbelDent_QyestionTypeName"].ToString().Trim(),
                                        questionversion_date = Convert.ToDateTime(dtFormMasterRow["QuestionVersion_Date"].ToString().Trim()).ToString("yyyy-MM-dd"),
                                        responsetypeid = dtFormMasterRow["AbelDent_ResponsetypeId"].ToString().Trim(),
                                        questionname = dtFormMasterRow["AbelDent_QuestionName"].ToString().Trim(),
                                        question_defaultvalue = dtFormMasterRow["AbelDent_Question_DefaultValue"].ToString().Trim(),
                                        questionversion = dtFormMasterRow["QuestionVersion"].ToString().Trim(),
                                        inputtype = dtFormMasterRow["InputType"].ToString().Trim(),
                                        options = dtFormMasterRow["Options"].ToString().Trim(),
                                        questionorder = dtFormMasterRow["QuestionOrder"].ToString().Trim(),
                                        answer_value = dtFormMasterRow["Answer_Value"].ToString().Trim(),
                                        abeldent_formquestion_ehr_id = dtFormMasterRow["AbelDent_Question_EHR_ID"].ToString().Trim(),
                                        abeldent_formquestion_localdb_id = dtFormMasterRow["AbelDent_FormQuestion_LocalDB_ID"].ToString().Trim(),
                                        abeldent_form_ehrunique_id = dtFormMasterRow["AbelDent_Form_EHRUnique_ID"].ToString().Trim(),
                                        abeldent_formquestion_ehrunique_id = dtFormMasterRow["AbelDent_Question_EHRUnique_ID"].ToString().Trim(),
                                        is_multifield = Convert.ToBoolean(dtFormMasterRow["Is_MultiField"].ToString().Trim()),
                                        is_optionfiled = Convert.ToBoolean(dtFormMasterRow["Is_OptionField"].ToString().Trim()),
                                        is_required = Convert.ToBoolean(dtFormMasterRow["Is_Required"].ToString().Trim()),
                                        is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim())
                                        //is_active = Convert.ToBoolean(dtFormMasterRow["Is_Active"].ToString().Trim())
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                }

                                if (JsonFormMaster.Length > 0)
                                {
                                    string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                    strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "abeldent_formquestion", "1", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strFormMaster.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[AbelDent_FormQuestion Sync (Local Database To Adit Server) ] Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                    }
                                }
                                else
                                {
                                    strFormMaster = "Success";
                                }
                            }

                            if (strFormMaster.ToLower() == "Success".ToLower())
                            {
                                IsSynchFormMaster = true;
                            }
                            else
                            {
                                if (strFormMaster.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchFormMaster = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[AbelDent_FormQuestion Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                    IsSynchFormMaster = false;
                                }
                            }

                            if (IsSynchFormMaster)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FormQuestionMaster_Push");

                                IsMedicalHistorySyncPush = true;
                            }
                            else
                            {
                                IsMedicalHistorySyncPush = false;
                            }
                        }
                    }
                }
                GoalBase.WriteToSyncLogFile_Static("AbelDent_FormQuestion Sync (Local Database To Adit Server) Successfully.");
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[AbelDent_FormQuestion  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_OpenDentalSheet(string tablename)
        {
            try
            {
                var JsonSectionMaster = new System.Text.StringBuilder();
                bool IsSynchSectionMaster = true;

                for (int k = 0; k < Utility.DtInstallServiceList.Rows.Count; k++)
                {
                    DataTable dtLocalSectionMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("OD_SheetDef", Utility.DtInstallServiceList.Rows[k]["Installation_ID"].ToString());

                    if (dtLocalSectionMaster.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalSectionMaster, 100);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalSectionMaster, Utility.mstSyncBatchSize.Sheet);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strSectionMaster = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    foreach (DataRow dtSectionMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id  = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_OD_SheetDef SectionMaster = new Push_OD_SheetDef
                                        {
                                            appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                            location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            name = dtSectionMasterRow["Sheet_Name"].ToString().Replace(",", "").Trim(),
                                            organization = Utility.Organization_ID,
                                            sheetdefnum_ehr_id = dtSectionMasterRow["SheetDefNum_EHR_ID"].ToString().Trim(),
                                            sheetdefnum_localdb_id = dtSectionMasterRow["SheetDefNum_LocalDB_ID"].ToString().Trim(),
                                            sheetdefnum_web_id = dtSectionMasterRow["SheetDefNum_Web_ID"].ToString().Trim(),
                                            is_deleted = Convert.ToBoolean(dtSectionMasterRow["is_deleted"].ToString().Trim()),
                                            islandscape = Convert.ToBoolean(dtSectionMasterRow["IsLandscape"].ToString().Trim()),
                                            pagecount = Convert.ToBoolean(dtSectionMasterRow["PageCount"].ToString().Trim()),
                                            ismultipage = Convert.ToBoolean(dtSectionMasterRow["IsMultiPage"].ToString().Trim()),
                                            fontsize = dtSectionMasterRow["FontSize"].ToString().Trim(),
                                            fontname = dtSectionMasterRow["FontName"].ToString().Trim(),
                                            width = dtSectionMasterRow["Width"].ToString().Trim(),
                                            height = dtSectionMasterRow["Height"].ToString().Trim(),
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonSectionMaster.Append(javaScriptSerializer.Serialize(SectionMaster) + ",");
                                    }

                                    if (JsonSectionMaster.Length > 0)
                                    {
                                        string jsonString = "[" + JsonSectionMaster.ToString().Remove(JsonSectionMaster.Length - 1) + "]";
                                        strSectionMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, tablename, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                        if (strSectionMaster.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[" + tablename + " Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strSectionMaster);
                                        }
                                    }
                                    else
                                    {
                                        strSectionMaster = "Success";
                                    }
                                }

                                if (strSectionMaster.ToLower() == "Success".ToLower())
                                {
                                    IsSynchSectionMaster = true;
                                }
                                else
                                {
                                    if (strSectionMaster.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchSectionMaster = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[" + tablename + " Sync (Local Database To Adit Server) ] : " + strSectionMaster);
                                        IsSynchSectionMaster = false;
                                    }
                                }

                                if (IsSynchSectionMaster)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SectionMaster_Push");
                                    GoalBase.WriteToSyncLogFile_Static(tablename + "  Sync (Local Database To Adit Server) Successfully.");
                                    IsMedicalHistorySyncPush = true;
                                }
                                else
                                {
                                    IsMedicalHistorySyncPush = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[OD_SheetDef  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_OpenDentalSheetField(string tablename)
        {
            try
            {
                bool IsSynchAlertMaster = true;
                for (int k = 0; k < Utility.DtInstallServiceList.Rows.Count; k++)
                {
                    DataTable dtLocalAlertMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("OD_SheetFieldDef", Utility.DtInstallServiceList.Rows[k]["Installation_ID"].ToString());

                    if (dtLocalAlertMaster.Rows.Count > 0)
                    {
                        //List<DataTable> splitdt = Utility.SplitTable(dtLocalAlertMaster, 100);
                        List<DataTable> splitdt = Utility.SplitTable(dtLocalAlertMaster, Utility.mstSyncBatchSize.SheetField);

                        for (int i = 0; i < splitdt.Count; i++)
                        {
                            string strAlertMaster = "";
                            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                            {
                                if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                                {
                                    var JsonAlertMaster = new System.Text.StringBuilder();
                                    foreach (DataRow dtAlertMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id  = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                    {
                                        Push_OD_SheetFieldDef SheetFieldDef = new Push_OD_SheetFieldDef
                                        {
                                            appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                            location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                            organization = Utility.Organization_ID,
                                            fieldtype = dtAlertMasterRow["FieldType"].ToString().Trim(),
                                            fieldvalue = dtAlertMasterRow["FieldValue"].ToString().Trim(),
                                            fontsize = dtAlertMasterRow["FontSize"].ToString().Trim(),
                                            fontname = dtAlertMasterRow["FontName"].ToString().Trim(),
                                            fontisbold = dtAlertMasterRow["FontIsBold"].ToString().Trim(),
                                            xpos = dtAlertMasterRow["XPos"].ToString().Trim(),
                                            ypos = dtAlertMasterRow["YPos"].ToString().Trim(),
                                            width = dtAlertMasterRow["Width"].ToString().Trim(),
                                            height = dtAlertMasterRow["Height"].ToString().Trim(),
                                            growthbehavior = dtAlertMasterRow["GrowthBehavior"].ToString().Trim(),
                                            radiobuttonvalue = dtAlertMasterRow["RadioButtonValue"].ToString().Trim(),
                                            radiobuttongroup = dtAlertMasterRow["RadioButtonGroup"].ToString().Trim(),
                                            isrequired = dtAlertMasterRow["IsRequired"].ToString().Trim(),
                                            taborder = dtAlertMasterRow["TabOrder"].ToString().Trim(),
                                            reportablename = dtAlertMasterRow["ReportableName"].ToString().Trim(),
                                            textalign = Convert.ToBoolean(dtAlertMasterRow["TextAlign"].ToString()),
                                            itemcolor = dtAlertMasterRow["ItemColor"].ToString().Trim(),
                                            is_deleted = Convert.ToBoolean(dtAlertMasterRow["is_deleted"].ToString().Trim()),
                                            sheetfielddefnum_ehr_id = dtAlertMasterRow["sheetfielddefnum_ehr_id"].ToString().Trim(),
                                            sheetfielddefnum_localdb_id = dtAlertMasterRow["sheetfielddefnum_localdb_id"].ToString().Trim(),
                                            sheetdefnum_ehr_id = dtAlertMasterRow["sheetdefnum_ehr_id"].ToString().Trim(),
                                            sheettype = dtAlertMasterRow["sheettype"].ToString().Trim(),
                                            fieldname = dtAlertMasterRow["fieldname"].ToString().Trim(),
                                            islocked = Convert.ToBoolean(dtAlertMasterRow["islocked"].ToString().Trim()),
                                            tabordermobile = Convert.ToInt16(dtAlertMasterRow["tabordermobile"].ToString().Trim()),
                                            uilabelmobile = dtAlertMasterRow["uilabelmobile"].ToString().Trim(),
                                            uilabelmobileradiobutton = dtAlertMasterRow["uilabelmobileradiobutton"].ToString().Trim()
                                        };
                                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                        JsonAlertMaster.Append(javaScriptSerializer.Serialize(SheetFieldDef) + ",");
                                    }
                                    if (JsonAlertMaster.Length > 0)
                                    {
                                        string jsonString = "[" + JsonAlertMaster.ToString().Remove(JsonAlertMaster.Length - 1) + "]";
                                        strAlertMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, tablename, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                                        if (strAlertMaster.ToLower() != "Success".ToLower())
                                        {
                                            GoalBase.WriteToErrorLogFile_Static("[" + tablename + " Sync (Local Database To Adit Server) ] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strAlertMaster);
                                        }
                                    }
                                    else
                                    {
                                        strAlertMaster = "Success";
                                    }
                                }

                                if (strAlertMaster.ToLower() == "Success".ToLower())
                                {
                                    IsSynchAlertMaster = true;
                                }
                                else
                                {
                                    if (strAlertMaster.Contains("The remote name could not be resolved:"))
                                    {
                                        IsSynchAlertMaster = false;
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[" + tablename + " Sync (Local Database To Adit Server) ] : " + strAlertMaster);
                                        IsSynchAlertMaster = false;
                                    }
                                }

                                if (IsSynchAlertMaster)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("AlertMaster_Push");
                                    GoalBase.WriteToSyncLogFile_Static(tablename + " Sync (Local Database To Adit Server) Successfully.");
                                    IsMedicalHistorySyncPush = true;
                                }
                                else
                                {
                                    IsMedicalHistorySyncPush = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[OD_SheetFieldDef  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_CD_FormMaster()
        {
            try
            {

                bool IsSynchFormMaster = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("CD_FormMaster", "1");

                if (dtLocalFormMaster.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.FormMaster);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strFormMaster = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonFormMaster = new System.Text.StringBuilder();
                                foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "'"))
                                {
                                    Push_CD_FormMaster FormMaster = new Push_CD_FormMaster
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        name = dtFormMasterRow["FormName_Name"].ToString().Replace(",", "").Trim(),
                                        organization = Utility.Organization_ID,
                                        cd_formmaster_localdb_id = dtFormMasterRow["CD_FormMaster_Local_Id"].ToString().Trim(),
                                        cd_formmaster_ehr_id = dtFormMasterRow["CD_FormMaster_EHR_ID"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim())
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                }
                                if (JsonFormMaster.Length > 0)
                                {
                                    string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                    strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "CD_FormMaster", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strFormMaster.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[CD_FormMaster Sync (Local Database To Adit Server) ] Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                    }
                                }
                                else
                                {
                                    strFormMaster = "Success";
                                }
                            }

                            if (strFormMaster.ToLower() == "Success".ToLower())
                            {
                                IsSynchFormMaster = true;
                            }
                            else
                            {
                                if (strFormMaster.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchFormMaster = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[CD_FormMaster Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                    IsSynchFormMaster = false;
                                }
                            }

                            if (IsSynchFormMaster)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("CD_FormMaster_Push");
                                GoalBase.WriteToSyncLogFile_Static("CD_FormMaster Sync (Local Database To Adit Server) Successfully.");
                                IsMedicalHistorySyncPush = true;
                            }
                            else
                            {
                                IsMedicalHistorySyncPush = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[SynchDataLiveDB_Push_CD_FormMaster  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_CD_QuestionMaster()
        {
            try
            {

                bool IsSynchFormMaster = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("CD_QuestionMaster", "1");

                if (dtLocalFormMaster.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.QuestionMaster);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strFormMaster = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonFormMaster = new System.Text.StringBuilder();
                                foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "'"))
                                {
                                    Push_CD_QuestionMaster FormMaster = new Push_CD_QuestionMaster
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        organization = Utility.Organization_ID,
                                        question_description = dtFormMasterRow["Question_Description"].ToString().Trim(),
                                        question_sequence = Convert.ToInt16(dtFormMasterRow["Question_Sequence"].ToString().Trim()),
                                        question_warnings = Convert.ToBoolean(dtFormMasterRow["Question_Warnings"].ToString().Trim()),
                                        question_type = Convert.ToInt16(dtFormMasterRow["Question_Type"].ToString().Trim()),
                                        comment = dtFormMasterRow["Comment"].ToString().Trim(),
                                        answer_value = "",
                                        cd_form_name = dtFormMasterRow["FormName_Name"].ToString().Trim(),
                                        cd_formmaster_ehr_id = dtFormMasterRow["CD_FormMaster_EHR_ID"].ToString().Trim(),
                                        cd_questionmaster_ehr_id = dtFormMasterRow["CD_QuestionMaster_EHR_ID"].ToString().Trim(),
                                        cd_questionmaster_localdb_id = dtFormMasterRow["CD_QuestionMaster_Local_Id"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim())
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                }
                                if (JsonFormMaster.Length > 0)
                                {
                                    string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                    strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "CD_QuestionMaster", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strFormMaster.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[CD_QuestionMaster Sync (Local Database To Adit Server) ] Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                    }
                                }
                                else
                                {
                                    strFormMaster = "Success";
                                }
                            }

                            if (strFormMaster.ToLower() == "Success".ToLower())
                            {
                                IsSynchFormMaster = true;
                            }
                            else
                            {
                                if (strFormMaster.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchFormMaster = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[CD_QuestionMaster Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                    IsSynchFormMaster = false;
                                }
                            }

                            if (IsSynchFormMaster)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("CD_QuestionMaster_Push");
                                GoalBase.WriteToSyncLogFile_Static("CD_QuestionMaster Sync (Local Database To Adit Server) Successfully.");
                                IsMedicalHistorySyncPush = true;
                            }
                            else
                            {
                                IsMedicalHistorySyncPush = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[CD_QuestionMaster  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_EasyDental_Question()
        {
            try
            {

                bool IsSynchFormMaster = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetEasyDentalLocalMedicleQuestionData();

                if (dtLocalFormMaster.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.Question);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strFormMaster = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonFormMaster = new System.Text.StringBuilder();
                                foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "'"))
                                {
                                    Push_EasyDental_Question FormMaster = new Push_EasyDental_Question
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        organization = Utility.Organization_ID,
                                        easydentalformmaster = dtFormMasterRow["EasyDental_FormMaster_Web_ID"].ToString().Trim(),
                                        question = dtFormMasterRow["EasyDental_Question"].ToString().Trim(),
                                        questiontype = dtFormMasterRow["EasyDental_QuestionType"].ToString().Trim(),
                                        row = Convert.ToInt16(dtFormMasterRow["EasyDental_Row"].ToString().Trim()),
                                        defaultvalue = dtFormMasterRow["EasyDental_DefaultValue"].ToString().Trim(),
                                        easydental_form_localdb_id = dtFormMasterRow["EasyDental_Question_LocalDB_ID"].ToString().Trim(),
                                        easydental_questionid = dtFormMasterRow["EasyDental_QuestionId"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim()),
                                        is_active = true
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                }

                                if (JsonFormMaster.Length > 0)
                                {
                                    string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                    strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "EasyDental_Question", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                                    if (strFormMaster.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Easy Dental Question Form Sync (Local Database To Adit Server) ] Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                    }
                                }
                                else
                                {
                                    strFormMaster = "Success";
                                }
                            }

                            if (strFormMaster.ToLower() == "Success".ToLower())
                            {
                                IsSynchFormMaster = true;
                            }
                            else
                            {
                                if (strFormMaster.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchFormMaster = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Easy Dental Question Form Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                    IsSynchFormMaster = false;
                                }
                            }

                            if (IsSynchFormMaster)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FormMaster_Push");
                                GoalBase.WriteToSyncLogFile_Static("Easy Dental Question Form Sync (Local Database To Adit Server) Successfully.");
                                IsMedicalHistorySyncPush = true;
                            }
                            else
                            {
                                IsMedicalHistorySyncPush = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EasyDental_Form  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static void SynchDataLiveDB_Push_EasyDental_Form()
        {
            try
            {

                bool IsSynchFormMaster = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalMedicalHistoryRecords("CD_FormMaster", "1");

                if (dtLocalFormMaster.Rows.Count > 0)
                {
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, 100);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalFormMaster, Utility.mstSyncBatchSize.Form);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        string strFormMaster = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonFormMaster = new System.Text.StringBuilder();
                                foreach (DataRow dtFormMasterRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "'"))
                                {
                                    Push_EasyDental_Form FormMaster = new Push_EasyDental_Form
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(),//Utility.Location_ID,
                                        location = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        name = dtFormMasterRow["FormName_Name"].ToString().Replace(",", "").Trim(),
                                        organization = Utility.Organization_ID,
                                        easydental_formmaster_localdb_id = dtFormMasterRow["CD_FormMaster_Local_Id"].ToString().Trim(),
                                        easydental_formmasterid = dtFormMasterRow["CD_FormMaster_EHR_ID"].ToString().Trim(),
                                        is_deleted = Convert.ToBoolean(dtFormMasterRow["is_deleted"].ToString().Trim()),
                                        is_active = true
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonFormMaster.Append(javaScriptSerializer.Serialize(FormMaster) + ",");
                                }

                                if (JsonFormMaster.Length > 0)
                                {
                                    string jsonString = "[" + JsonFormMaster.ToString().Remove(JsonFormMaster.Length - 1) + "]";
                                    strFormMaster = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_MedicalHistory(jsonString, "EasyDental_Form", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                    if (strFormMaster.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Easy Dental Form Master Sync (Local Database To Adit Server) ] Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + strFormMaster);
                                    }
                                }
                                else
                                {
                                    strFormMaster = "Success";
                                }
                            }

                            if (strFormMaster.ToLower() == "Success".ToLower())
                            {
                                IsSynchFormMaster = true;
                            }
                            else
                            {
                                if (strFormMaster.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchFormMaster = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[EasyDental_Form Sync (Local Database To Adit Server) ] : " + strFormMaster);
                                    IsSynchFormMaster = false;
                                }
                            }

                            if (IsSynchFormMaster)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("EasyDental_Form_Push");
                                GoalBase.WriteToSyncLogFile_Static("EasyDental_Form Sync (Local Database To Adit Server) Successfully.");
                                IsMedicalHistorySyncPush = true;
                            }
                            else
                            {
                                IsMedicalHistorySyncPush = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EasyDental_Form  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static bool CheckandCreateLocationForPatientImage(string orgId, string locationId, string AppointmentLocationId)
        {
            try
            {
                #region Call API to check and create forlder

                bool result = true;
                string jsonString = "";
                string _successfullstataus = "";
                var JsonAddPatientProfile = new System.Text.StringBuilder();
                string RestURL = PushLiveDatabaseBAL.GetLive_Push_Record("createorglocdir");
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Push_CheckANDCreatePatientImageFolder patientAddImage = new Push_CheckANDCreatePatientImageFolder
                {
                    location = locationId.ToString(),
                    organization = orgId.ToString()

                };
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonAddPatientProfile.Append(javaScriptSerializer.Serialize(patientAddImage) + ",");
                if (JsonAddPatientProfile.Length > 0)
                {
                    jsonString = "[" + JsonAddPatientProfile.ToString().Remove(JsonAddPatientProfile.Length - 1) + "]";
                }

                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(AppointmentLocationId.ToString()));
                request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                    result = false;
                }
                if (response.Content.Contains("Fail"))
                {
                    result = false;
                }
                #endregion
                return result;
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PatientProfileImage  Sync (Local Database To Adit Server) ] : " + ex.Message);
                return false;
            }
        }

        private static void SynchDataLiveDB_Push_PatientImage(string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;
            try
            {

                bool IsSynchFormMaster = true;
                bool isFirstCalled = false;
                bool IscallFurther = true;
                DataTable dtLocalFormMaster = SynchLocalBAL.GetLocalPatientProfileImageRecords(Service_Install_Id);
                DataTable dtAddImage = new DataTable();
                dtAddImage.Columns.Add("OrgId");
                dtAddImage.Columns.Add("LocId");
                dtAddImage.Columns.Add("PatientId");
                dtAddImage.Columns.Add("PatientProfileId");
                string locationId = "";
                if (dtLocalFormMaster.Rows.Count > 0)
                {

                    string destPatientProfileImage = CommonUtility.GetAditPatientProfileImagePath();
                    string strFormMaster = "";
                    var JsonProvider = new System.Text.StringBuilder();
                    DataRow[] drDeleted = dtLocalFormMaster.Select("Is_Deleted = 1 ");
                    if (drDeleted != null && drDeleted.Length > 0)
                    {
                        foreach (DataRow dr in drDeleted)
                        {
                            var sp = dr["Image_EHR_Name"].ToString().Split('@');
                            Push_PatientProfileImageRemove ProviderHours = new Push_PatientProfileImageRemove
                            {
                                location = sp[1],
                                organization = sp[0],
                                patientId = sp[2].Replace(".jpeg", "")
                            };
                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            ;

                            string jsonString = "[" + javaScriptSerializer.Serialize(ProviderHours).ToString() + "]";
                            string RestURL = PushLiveDatabaseBAL.GetLive_Push_Record("patientprofileimageremove");


                            var request = new RestRequest(Method.POST);
                            var client = new RestClient(RestURL);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.Timeout = 900000;
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(dr["Location_ID"].ToString()));
                            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);

                            if (response.ErrorMessage != null)
                            {
                                _successfullstataus = response.ErrorMessage;
                            }

                            else
                            {
                                var ProvidersDto = JsonConvert.DeserializeObject<Pull_PatientProfileImageRemove>(response.Content);
                                if (ProvidersDto != null && ProvidersDto.data != null)
                                {
                                    foreach (var item in ProvidersDto.data)
                                    {
                                        SynchLocalBAL.Update_PatientProfileImageStatus(item._id, dr["Service_Install_Id"].ToString());
                                        if (File.Exists(destPatientProfileImage + "\\" + dr["Image_EHR_Name"].ToString().Trim()))
                                        {
                                            File.Delete(destPatientProfileImage + "\\" + dr["Image_EHR_Name"].ToString().Trim());
                                        }
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }

                    }
                    #region Insert Patient Profile Image
                    DataRow[] drInserted = dtLocalFormMaster.Select("Is_Deleted = 0 AND Is_ProfileImage_Update = 0 ");
                    dtAddImage.Rows.Clear();
                    if (drInserted != null && drInserted.Length > 0)
                    {
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (!isFirstCalled)
                            {
                                IscallFurther = CheckandCreateLocationForPatientImage(Utility.DtLocationList.Rows[i]["Organization_ID"].ToString(), Utility.DtLocationList.Rows[i]["Loc_id"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString());
                                isFirstCalled = true;
                                if (!IscallFurther)
                                {
                                    break;
                                }
                            }
                            //List<DataTable> splitpatientImage = Utility.SplitTable(drInserted.CopyToDataTable().Select("Location_ID = '" + Utility.DtLocationList.Rows[i]["Location_Id"].ToString() + "'").CopyToDataTable(), Utility.imageuploadbatch);
                            List<DataTable> splitpatientImage = Utility.SplitTable(drInserted.CopyToDataTable().Select("Location_ID = '" + Utility.DtLocationList.Rows[i]["Location_Id"].ToString() + "'").CopyToDataTable(), Utility.mstSyncBatchSize.PatientImage);
                            for (int j = 0; j < splitpatientImage.Count; j++)
                            {
                                var request = new RestRequest(Method.POST);
                                string RestURL = PushLiveDatabaseBAL.GetLive_Push_Record("multisync");
                                var client = new RestClient(RestURL);
                                foreach (DataRow dtFormMasterRow in splitpatientImage[j].Rows)
                                {
                                    if (File.Exists(destPatientProfileImage + "\\" + dtFormMasterRow["Image_EHR_Name"].ToString().Trim()))
                                    {
                                        request.AddFile("upload", destPatientProfileImage + "\\" + dtFormMasterRow["Image_EHR_Name"].ToString().Trim());//.Replace("@", "|")  
                                    }
                                }
                                request.AddParameter("organization", Utility.DtLocationList.Rows[i]["Organization_ID"].ToString());
                                request.AddParameter("location", Utility.DtLocationList.Rows[i]["Loc_id"].ToString());
                                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[i]["Location_ID"].ToString()));
                                IRestResponse response = client.Execute(request);
                                if (response.ErrorMessage != null)
                                {
                                    _successfullstataus = response.ErrorMessage;
                                }
                                else
                                {
                                    var ProvidersDto = JsonConvert.DeserializeObject<Pull_PatientProfileImage>(response.Content);
                                    if (ProvidersDto != null && ProvidersDto.data != null)
                                    {
                                        dtAddImage.Rows.Clear();
                                        foreach (var item in ProvidersDto.data)
                                        {
                                            var sp1 = item.originalname.ToString().Split('@');
                                            SynchLocalBAL.Update_PatientProfileImageStatus(sp1[2].ToString().Replace(".jpeg", ""), Service_Install_Id);
                                            DataRow drRow = dtAddImage.NewRow();
                                            drRow["OrgId"] = sp1[0].ToString();
                                            drRow["LocId"] = sp1[1].ToString();
                                            drRow["PatientId"] = sp1[2].ToString().Replace(".jpeg", "").ToString();
                                            drRow["PatientProfileId"] = item.originalname.ToString();
                                            dtAddImage.Rows.Add(drRow);
                                            if (File.Exists(destPatientProfileImage + "\\" + item.originalname.ToString()))
                                            {
                                                File.Delete(destPatientProfileImage + "\\" + item.originalname.ToString().Trim());
                                            }
                                        }

                                        #region Add Image
                                        var JsonAddPatientProfile = new System.Text.StringBuilder();
                                        foreach (DataRow drRow in dtAddImage.Rows)
                                        {
                                            Push_PatientProfileAddImageName patientAddImage = new Push_PatientProfileAddImageName
                                            {
                                                location = drRow["LocId"].ToString(),
                                                organization = drRow["OrgId"].ToString(),
                                                patientId = drRow["PatientId"].ToString(),
                                                profileimage = drRow["PatientProfileId"].ToString()
                                            };
                                            locationId = drRow["LocId"].ToString();
                                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                            JsonAddPatientProfile.Append(javaScriptSerializer.Serialize(patientAddImage) + ",");
                                        }
                                        string jsonString = "";
                                        if (JsonAddPatientProfile.Length > 0)
                                        {
                                            jsonString = "[" + JsonAddPatientProfile.ToString().Remove(JsonAddPatientProfile.Length - 1) + "]";
                                            RestURL = PushLiveDatabaseBAL.GetLive_Push_Record("addimagename");
                                        }
                                        var request1 = new RestRequest(Method.POST);
                                        var client1 = new RestClient(RestURL);
                                        ServicePointManager.Expect100Continue = true;
                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                        request1.AddHeader("cache-control", "no-cache");
                                        request1.AddHeader("content-type", "application/json");
                                        request1.Timeout = 900000;
                                        request1.AddHeader("Authorization", Utility.WebAdminUserToken);
                                        request1.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[i]["Location_ID"].ToString()));
                                        request1.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                        IRestResponse response1 = client1.Execute(request1);

                                        if (response1.ErrorMessage != null)
                                        {
                                            _successfullstataus = response1.ErrorMessage;
                                        }
                                        #endregion
                                    }
                                    _successfullstataus = "Success";
                                }
                            }
                        }
                    }
                    #endregion

                    #region Update Patient Profile Image
                    drInserted = dtLocalFormMaster.Select("Is_Deleted = 0 AND Is_ProfileImage_Update = 1 ");
                    dtAddImage.Rows.Clear();
                    if (drInserted != null && drInserted.Length > 0 && IscallFurther)
                    {
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {

                            //List<DataTable> splitpatientImage = Utility.SplitTable(drInserted.CopyToDataTable().Select("Location_ID = '" + Utility.DtLocationList.Rows[i]["Location_Id"].ToString() + "'").CopyToDataTable(), Utility.imageuploadbatch);
                            List<DataTable> splitpatientImage = Utility.SplitTable(drInserted.CopyToDataTable().Select("Location_ID = '" + Utility.DtLocationList.Rows[i]["Location_Id"].ToString() + "'").CopyToDataTable(), Utility.mstSyncBatchSize.PatientImage);
                            for (int j = 0; j < splitpatientImage.Count; j++)
                            {
                                var request = new RestRequest(Method.POST);
                                string RestURL = PushLiveDatabaseBAL.GetLive_Push_Record("multisyncupdate");
                                var client = new RestClient(RestURL);
                                foreach (DataRow dtFormMasterRow in splitpatientImage[j].Rows)
                                {
                                    if (File.Exists(destPatientProfileImage + "\\" + dtFormMasterRow["Image_EHR_Name"].ToString().Trim()))
                                    {
                                        request.AddFile("upload", destPatientProfileImage + "\\" + dtFormMasterRow["Image_EHR_Name"].ToString().Trim());//.Replace("@", "|")  
                                    }
                                }
                                request.AddParameter("organization", Utility.DtLocationList.Rows[i]["Organization_ID"].ToString());
                                request.AddParameter("location", Utility.DtLocationList.Rows[i]["Loc_id"].ToString());
                                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[i]["Location_ID"].ToString()));
                                IRestResponse response = client.Execute(request);
                                if (response.ErrorMessage != null)
                                {
                                    _successfullstataus = response.ErrorMessage;
                                }
                                else
                                {
                                    var ProvidersDto = JsonConvert.DeserializeObject<Pull_PatientProfileImage>(response.Content);
                                    if (ProvidersDto != null && ProvidersDto.data != null)
                                    {
                                        dtAddImage.Rows.Clear();
                                        foreach (var item in ProvidersDto.data)
                                        {
                                            var sp1 = item.originalname.ToString().Split('@');
                                            SynchLocalBAL.Update_PatientProfileImageStatus(sp1[2].ToString().Replace(".jpeg", ""), Service_Install_Id);
                                            DataRow drRow = dtAddImage.NewRow();
                                            drRow["OrgId"] = sp1[0].ToString();
                                            drRow["LocId"] = sp1[1].ToString();
                                            drRow["PatientId"] = sp1[2].ToString().Replace(".jpeg", "").ToString();
                                            drRow["PatientProfileId"] = item.originalname.ToString();
                                            dtAddImage.Rows.Add(drRow);
                                            if (File.Exists(destPatientProfileImage + "\\" + item.originalname.ToString()))
                                            {
                                                File.Delete(destPatientProfileImage + "\\" + item.originalname.ToString().Trim());
                                            }
                                        }

                                        #region Add Image
                                        var JsonAddPatientProfile = new System.Text.StringBuilder();
                                        foreach (DataRow drRow in dtAddImage.Rows)
                                        {
                                            Push_PatientProfileAddImageName patientAddImage = new Push_PatientProfileAddImageName
                                            {
                                                location = drRow["LocId"].ToString(),
                                                organization = drRow["OrgId"].ToString(),
                                                patientId = drRow["PatientId"].ToString(),
                                                profileimage = drRow["PatientProfileId"].ToString()
                                            };
                                            locationId = drRow["LocId"].ToString();
                                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                            JsonAddPatientProfile.Append(javaScriptSerializer.Serialize(patientAddImage) + ",");
                                        }
                                        string jsonString = "";
                                        if (JsonAddPatientProfile.Length > 0)
                                        {
                                            jsonString = "[" + JsonAddPatientProfile.ToString().Remove(JsonAddPatientProfile.Length - 1) + "]";
                                            RestURL = PushLiveDatabaseBAL.GetLive_Push_Record("updateimagename");
                                        }
                                        var request1 = new RestRequest(Method.POST);
                                        var client1 = new RestClient(RestURL);
                                        ServicePointManager.Expect100Continue = true;
                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                        request1.AddHeader("cache-control", "no-cache");
                                        request1.AddHeader("content-type", "application/json");
                                        request1.Timeout = 900000;
                                        request1.AddHeader("Authorization", Utility.WebAdminUserToken);
                                        request1.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[i]["Location_ID"].ToString()));
                                        request1.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                        IRestResponse response1 = client1.Execute(request1);

                                        if (response1.ErrorMessage != null)
                                        {
                                            _successfullstataus = response1.ErrorMessage;
                                        }
                                        #endregion
                                    }
                                    _successfullstataus = "Success";
                                }
                            }
                        }
                    }
                    #endregion


                }
                GoalBase.WriteToSyncLogFile_Static("PatientProfileImage  Sync (Local Database To Adit Server Successfully.) ");
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PatientProfileImage  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }
        #endregion

        #region Patient Balance
        public static void SynchDataLiveDB_Push_PatientBalance()
        {


            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{

                bool IsSynchPatient = true;

                DataTable dtLocalPatient = SynchLocalBAL.GetPushLocalPatientBalanceData();

                if (dtLocalPatient.Rows.Count > 0)
                {

                    int totPatient = dtLocalPatient.Rows.Count;
                    int cntPatient = 0;

                    PushPatientRecord = 0;
                    TotalPushPatientRecord = dtLocalPatient.Rows.Count;
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, patientPushCounter);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, Utility.mstSyncBatchSize.PatientBalance);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        splitdt[i] = Utility.CreateDistinctRecords(splitdt[i], "Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID", "Patient_EHR_Id");
                        string strPatient = "";
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {

                            if (Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                var JsonPatient = new System.Text.StringBuilder();
                                foreach (DataRow dtPatientRow in splitdt[i].Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                                {

                                    PushPatientRecord = PushPatientRecord + 1;

                                    Push_PatientBalance patient = new Push_PatientBalance
                                    {
                                        appointmentlocation = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),//Utility.Location_ID,
                                        organization = Utility.Organization_ID,
                                        patient_ehr_id = dtPatientRow["Patient_EHR_ID"].ToString().Trim(),
                                        current_bal = dtPatientRow["CurrentBal"].ToString().Trim(),
                                        thirty_day = dtPatientRow["ThirtyDay"].ToString().Trim(),
                                        sixty_day = dtPatientRow["SixtyDay"].ToString().Trim(),
                                        ninety_day = dtPatientRow["NinetyDay"].ToString().Trim(),
                                        over_ninty = dtPatientRow["Over90"].ToString().Trim(),
                                        remaining_benefit = dtPatientRow["remaining_benefit"].ToString().Trim(),
                                        used_benefit = dtPatientRow["used_benefit"].ToString().Trim(),
                                        collect_payment = dtPatientRow["collect_payment"].ToString().Trim()
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    JsonPatient.Append(javaScriptSerializer.Serialize(patient) + ",");
                                }

                                if (JsonPatient.Length > 0)
                                {
                                    string jsonString = "[" + JsonPatient.ToString().Remove(JsonPatient.Length - 1) + "]";
                                    //string strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "patient");
                                    strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_PatientBalance(jsonString, "patientbalance", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), dtLocalPatient);

                                    if (strPatient.ToLower() != "Success".ToLower())
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[Appointment_Patient_Balance Sync (Local Database To Adit Server) ] Service Install Id " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  : " + strPatient);
                                    }
                                }
                                else
                                {
                                    strPatient = "Success";
                                }
                            }

                            if (strPatient.ToLower() == "Success".ToLower())
                            {
                                IsSynchPatient = true;
                            }
                            else
                            {
                                if (strPatient.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchPatient = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[Appointment_Patient_Balance Sync (Local Database To Adit Server) ] : " + strPatient);
                                    IsSynchPatient = false;
                                }
                            }
                        }
                    }
                }
                if (IsSynchPatient)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment_Patient");
                    GoalBase.WriteToSyncLogFile_Static("Appointment_Patient_Balance Sync (Local Database To Adit Server) Successfully.");
                }
                // }
                //  IsPatientSyncingRunning = false;
            }
            catch (Exception ex)
            {
                //IsPatientSyncingRunning = false;
                GoalBase.WriteToErrorLogFile_Static("[Appointment_Patient_Balance Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
            //}
            //else
            //{
            //    IsPatientSyncingInQueue = true;
            //}
        }
        #endregion

        #region Pozative Configuration
        public static void SynchDataLiveDB_Push_PozativeConfiguraion()
        {
            try
            {
                bool IsSynchPozativeConf = true;
                string strpushConfigurationRoot = "";
                var objectsList = new List<object>();
                DataTable dtLocation = Utility.GetLocationData(true);
                DataTable dtOrganization = Utility.GetOrganizationData(true);
                DataTable dtServiceInstall = Utility.GetServiceInstallationData(true);

                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    var PushConfigurationRoot = new PushConfigurationRoot
                    {
                        location = new List<Push_LocationBO>(),
                        organization = new List<Push_OrganizationBO>(),
                        service_installation = new List<Push_ServiceInstallationBO>()
                    };

                    string jsonLocationSub = "";
                    if (dtLocation.Rows.Count > 0)
                    {
                        var JsonLocation = new System.Text.StringBuilder();
                        foreach (DataRow dtLocationRow in dtLocation.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'"))
                        {
                            var LocationSub = new Push_LocationBO
                            {
                                AditLocationSyncEnable = dtLocationRow["AditLocationSyncEnable"].ToString(),
                                AditSync = dtLocationRow["AditSync"].ToString().Trim(),
                                ApptAutoBook = dtLocationRow["ApptAutoBook"].ToString().Trim(),
                                Clinic_Number = dtLocationRow["Clinic_Number"].ToString(),
                                Loc_ID = dtLocationRow["Loc_ID"].ToString().Trim(),
                                Location_ID = dtLocationRow["Location_ID"].ToString().Trim(),
                                Organization_ID = dtLocationRow["Organization_ID"].ToString(),
                                Service_Install_Id = dtLocationRow["Service_Install_Id"].ToString().Trim(),
                                User_ID = dtLocationRow["User_ID"].ToString().Trim(),
                                address = dtLocationRow["address"].ToString(),
                                email = dtLocationRow["email"].ToString().Trim(),
                                google_address = dtLocationRow["google_address"].ToString().Trim(),
                                language = dtLocationRow["language"].ToString(),
                                location_numbers = dtLocationRow["location_numbers"].ToString().Trim(),
                                name = dtLocationRow["name"].ToString().Trim(),
                                owner = dtLocationRow["owner"].ToString().Trim(),
                                phone = dtLocationRow["phone"].ToString().Trim(),
                                website_url = dtLocationRow["website_url"].ToString().Trim()
                            };

                            PushConfigurationRoot.location.Add(LocationSub);
                        }
                    }

                    string jsonOrganization = string.Empty;
                    if (dtOrganization.Rows.Count > 0)
                    {
                        var JsonOrganization = new System.Text.StringBuilder();
                        foreach (DataRow dtOrganizationRow in dtOrganization.Rows)
                        {
                            var OrganizationSub = new Push_OrganizationBO
                            {
                                Adit_User_Email_ID = dtOrganizationRow["Adit_User_Email_ID"].ToString(),
                                Adit_User_Email_Password = dtOrganizationRow["Adit_User_Email_Password"].ToString().Trim(),
                                Name = dtOrganizationRow["Name"].ToString().Trim(),
                                Organization_ID = dtOrganizationRow["Organization_ID"].ToString().Trim(),
                                address = dtOrganizationRow["address"].ToString().Trim(),
                                currency = dtOrganizationRow["currency"].ToString().Trim(),
                                email = dtOrganizationRow["email"].ToString().Trim(),
                                info = dtOrganizationRow["info"].ToString().Trim(),
                                is_active = dtOrganizationRow["is_active"].ToString().Trim(),
                                owner = dtOrganizationRow["owner"].ToString().Trim(),
                                phone = dtOrganizationRow["phone"].ToString().Trim()
                            };

                            PushConfigurationRoot.organization.Add(OrganizationSub);
                        }

                    }


                    string jsonServiceInstallationSub = "";
                    var JsonServiceInstallation = new System.Text.StringBuilder();
                    foreach (DataRow dtServiceInstallationRow in dtServiceInstall.Rows)
                    {
                        var ServiceInstallationSub = new Push_ServiceInstallationBO
                        {
                            AditSync = dtServiceInstallationRow["AditSync"].ToString(),
                            AppIdleStartTime = dtServiceInstallationRow["AppIdleStartTime"].ToString().Trim(),
                            AppIdleStopTime = dtServiceInstallationRow["AppIdleStopTime"].ToString().Trim(),
                            ApplicationIdleTimeOff = dtServiceInstallationRow["ApplicationIdleTimeOff"].ToString().Trim(),
                            ApplicationInstalledTime = dtServiceInstallationRow["ApplicationInstalledTime"].ToString().Trim(),
                            Application_Name = dtServiceInstallationRow["Application_Name"].ToString().Trim(),
                            Application_Version = dtServiceInstallationRow["Application_Version"].ToString().Trim(),
                            ApptAutoBook = dtServiceInstallationRow["ApptAutoBook"].ToString().Trim(),
                            DBConnString = dtServiceInstallationRow["DBConnString"].ToString().Trim(),
                            Database = dtServiceInstallationRow["Database"].ToString().Trim(),
                            DentrixPDFConstring = dtServiceInstallationRow["DentrixPDFConstring"].ToString().Trim(),
                            DentrixPDFPassword = dtServiceInstallationRow["DentrixPDFPassword"].ToString().Trim(),
                            Document_Path = dtServiceInstallationRow["Document_Path"].ToString().Trim(),
                            DontAskPasswordOnSaveSetting = dtServiceInstallationRow["DontAskPasswordOnSaveSetting"].ToString().Trim(),
                            EHR_Sub_Version = dtServiceInstallationRow["EHR_Sub_Version"].ToString().Trim(),
                            EHR_VersionNumber = dtServiceInstallationRow["EHR_VersionNumber"].ToString().Trim(),
                            Hostname = dtServiceInstallationRow["Hostname"].ToString().Trim(),
                            IS_Install = dtServiceInstallationRow["IS_Install"].ToString().Trim(),
                            Installation_Date = dtServiceInstallationRow["Installation_Date"].ToString().Trim(),
                            Installation_ID = dtServiceInstallationRow["Installation_ID"].ToString().Trim(),
                            Installation_Modify_Date = dtServiceInstallationRow["Installation_Modify_Date"].ToString().Trim(),
                            IntegrationKey = dtServiceInstallationRow["IntegrationKey"].ToString().Trim(),
                            Location_ID = dtServiceInstallationRow["Location_ID"].ToString().Trim(),
                            NotAllowToChangeSystemDateFormat = dtServiceInstallationRow["NotAllowToChangeSystemDateFormat"].ToString().Trim(),
                            Organization_ID = dtServiceInstallationRow["Organization_ID"].ToString().Trim(),
                            Password = dtServiceInstallationRow["Password"].ToString().Trim(),
                            Port = dtServiceInstallationRow["Port"].ToString().Trim(),
                            PozativeEmail = dtServiceInstallationRow["PozativeEmail"].ToString().Trim(),
                            PozativeLocationID = dtServiceInstallationRow["PozativeLocationID"].ToString().Trim(),
                            PozativeLocationName = dtServiceInstallationRow["PozativeLocationName"].ToString().Trim(),
                            PozativeSync = dtServiceInstallationRow["PozativeSync"].ToString().Trim(),
                            System_Name = dtServiceInstallationRow["System_Name"].ToString().Trim(),
                            System_processorID = dtServiceInstallationRow["System_processorID"].ToString().Trim(),
                            UserId = dtServiceInstallationRow["UserId"].ToString().Trim(),
                            WebAdminUserToken = dtServiceInstallationRow["WebAdminUserToken"].ToString().Trim(),
                            Windows_Service_Version = dtServiceInstallationRow["Windows_Service_Version"].ToString().Trim(),
                            timezone = dtServiceInstallationRow["timezone"].ToString().Trim(),
                            Adit_User_Email_ID = dtServiceInstallationRow["AditUserEmailID"].ToString().Trim(),
                            Adit_User_Email_Password = dtServiceInstallationRow["AditUserEmailPassword"].ToString().Trim()
                        };

                        PushConfigurationRoot.service_installation.Add(ServiceInstallationSub);
                    }

                    var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonApptStatusString = javaScriptApptStatusSerializer.Serialize(PushConfigurationRoot);

                    strpushConfigurationRoot = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_PozativeConfiguration(jsonApptStatusString, "pozativeconfiguration", Utility.DtLocationList.Rows[i]["Location_ID"].ToString().Trim());

                    if (strpushConfigurationRoot.ToLower() != "Success".ToLower())
                    {
                        GoalBase.WriteToErrorLogFile_Static("[SynchDataLiveDB_Push_PozativeConfiguraion Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() + " And Clinic " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + " : " + strpushConfigurationRoot);
                    }


                    if (strpushConfigurationRoot.ToLower() == "Success".ToLower())
                    {
                        IsSynchPozativeConf = true;
                    }
                    else
                    {
                        if (strpushConfigurationRoot.Contains("SynchDataLiveDB_Push_PozativeConfiguraion : The remote name could not be resolved:"))
                        {
                            IsSynchPozativeConf = false;
                        }
                        else
                        {
                            GoalBase.WriteToErrorLogFile_Static("[SynchDataLiveDB_Push_PozativeConfiguraion Sync (Local Database To Adit Server) ] : " + strpushConfigurationRoot);
                            IsSynchPozativeConf = false;
                        }
                    }
                }

                if (IsSynchPozativeConf)
                {
                    GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Push_PozativeConfiguraion Sync (Local Database To Adit Server) Successfully.");
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[SynchDataLiveDB_Push_PozativeConfiguraion Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }
        #endregion

        #region AditEventListener
        public static void SynchDataLiveDB_Send_EventAcknowledgement(string strType, string strWebID, string Location_Id)
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                bool IsAcknowledge = true;
                string strAcknowledge = "";
                StringBuilder JsonAcknowledge = new StringBuilder();
                PushEventAcknowledgement AcknowledgeSub = new PushEventAcknowledgement
                {
                    id = strWebID,
                    type = strType
                };
                var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonAcknowledge.Append(javaScriptApptStatusSerializer.Serialize(AcknowledgeSub) + ",");

                if (JsonAcknowledge.Length > 0)
                {
                    string jsonString = "[" + JsonAcknowledge.ToString().Remove(JsonAcknowledge.Length - 1) + "]";
                    strAcknowledge = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_EventAcknowledgement(jsonString, "eventacknowledgement", Location_Id);

                    if (strAcknowledge.ToLower() != "Success".ToLower())
                    {
                        GoalBase.WriteToErrorLogFile_Static("[EventAcknowledgement Sync (Local Database To Adit Server) ] Location_Id: " + Location_Id + ", Web_Id:" + strWebID + ", Type:" + strType);
                    }
                }
                else
                {
                    strAcknowledge = "Success";
                }

                if (strAcknowledge.ToLower() == "Success".ToLower())
                {
                    IsAcknowledge = true;
                }
                else
                {
                    if (strAcknowledge.Contains("The remote name could not be resolved:"))
                    {
                        IsAcknowledge = false;
                    }
                    else
                    {
                        GoalBase.WriteToErrorLogFile_Static("[EventAcknowledgement Sync (Local Database To Adit Server) ] : " + strAcknowledge);
                        IsAcknowledge = false;
                    }
                }

                if (IsAcknowledge)
                {
                    GoalBase.WriteToSyncLogFile_Static("EventAcknowledgement Sync (Local Database To Adit Server) Successfully.");
                }
                //  }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EventAcknowledgement Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }
        #endregion

    }
}
