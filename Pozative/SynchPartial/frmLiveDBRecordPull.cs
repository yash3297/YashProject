using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pozative.BAL;
using Pozative.BO;
using Pozative.UTL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable

        static bool IsProviderSyncPull = true;
        DataTable dtLiveEaglesoftEHR_appointment = new DataTable();

        private BackgroundWorker bwSynchLiveDB_Pull_Appointment = null;
        private System.Timers.Timer timerSynchLiveDB_Pull_Appointment = null;

        private BackgroundWorker bwSynchLiveDB_CheckANDDelete_Appointment = null;

        private BackgroundWorker bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID = null;
        private System.Timers.Timer timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID = null;

        private BackgroundWorker bwSynchLiveDB_Pull_EHR_appointment = null;
        private System.Timers.Timer timerSynchLiveDB_Pull_EHR_appointment = null;

        private BackgroundWorker bwSynchLiveDB_Pull_Provider = null;
        private System.Timers.Timer timerSynchLiveDB_Pull_Provider = null;

        private BackgroundWorker bwSynchLiveDB_Pull_ApptType = null;
        private System.Timers.Timer timerSynchLiveDB_Pull_ApptType = null;

        private BackgroundWorker bwSynchLiveDB_Pull_Patient = null;
        private System.Timers.Timer timerSynchLiveDB_Pull_Patient = null;

        private BackgroundWorker bwSynchLiveDB_Pull_Operatory = null;
        private System.Timers.Timer timerSynchLiveDB_Pull_Operatory = null;

        private BackgroundWorker bwSynchLiveDB_Pull_PatientForm = null;
        private System.Timers.Timer timerSynchLiveDB_Pull_PatientForm = null;


        #endregion

        private void CallSynchLiveDB_PullToLocal()
        {
            if (Utility.AditSync)
            {
                if ((Utility.Application_ID == 3 || Utility.Application_ID == 1) && (Utility.ApplicationInstalledTime.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")))
                {
                    SynchDataLiveDB_Pull_EHRAppointment_WithOut_PatientID();
                    fncSyncDataLiveDB_Pull_EHRAppointment_WithOut_PatientID();

                    if (!IsMedicalHistoryRecordsPulled && Utility.Application_ID == 1)
                    {
                        GetEaglesoftMedicalHistoryRecords();
                    }
                }

                //SynchDataLiveDB_Pull_Provider();//Remove comment for testing
                //fncSyncDataLiveDB_Pull_Provider();
               
                SynchDataLiveDB_Pull_Appointment();
                fncSyncDataLiveDB_Pull_Appointment();

                fncSyncDataLiveDB_CheckANDDelete_Appointment();
                // SynchDataLiveDB_Pull_PatientForm();
                //fncSyncDataLiveDB_Pull_PatientForm();

                // if (Utility.ApptAutoBook)
                //{
                //  SynchDataLiveDB_Pull_EHR_appointment();
                fncSyncDataLiveDB_Pull_EHR_appointment();
                //}

            }
        }

        #region EHRAppointment_WithOut_PatientID

        private void fncSyncDataLiveDB_Pull_EHRAppointment_WithOut_PatientID()
        {
            // SynchDataLiveDB_Pull_EHRAppointment_WithOut_PatientID();
            InitBgWorkerLiveDB_Pull_EHRAppointment_WithOut_PatientID();
            InitBgTimerLiveDB_Pull_EHRAppointment_WithOut_PatientID();
        }

        private void InitBgTimerLiveDB_Pull_EHRAppointment_WithOut_PatientID()
        {
            timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID = new System.Timers.Timer();
            this.timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.Interval = 1000 * 60 * 400;
            this.timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID_Tick);
            timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.Enabled = true;
            timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.Start();
        }

        private void InitBgWorkerLiveDB_Pull_EHRAppointment_WithOut_PatientID()
        {
            bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID = new BackgroundWorker();
            bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.WorkerReportsProgress = true;
            bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.WorkerSupportsCancellation = true;
            bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID_RunWorkerCompleted);
        }

        private void timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.Enabled = false;
                MethodForCallSynchOrderLiveDB_Pull_EHRAppointment_WithOut_PatientID();
            }
        }

        public void MethodForCallSynchOrderLiveDB_Pull_EHRAppointment_WithOut_PatientID()
        {
            System.Threading.Thread procThreadmainLiveDB_Pull_EHRAppointment_WithOut_PatientID = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Pull_EHRAppointment_WithOut_PatientID);
            procThreadmainLiveDB_Pull_EHRAppointment_WithOut_PatientID.Start();
        }

        public void CallSyncOrderTableLiveDB_Pull_EHRAppointment_WithOut_PatientID()
        {
            if (bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.IsBusy != true)
            {
                bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_Pull_EHRAppointment_WithOut_PatientID();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLiveDB_Pull_EHRAppointment_WithOut_PatientID.Enabled = true;
        }

        public void SynchDataLiveDB_Pull_EHRAppointment_WithOut_PatientID()
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{

                Int32 totalCount = 0;
                int batchsize = 0;
                double RemainPaging = 0;
                int pagestart = 0;

                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                {
                    if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                    {

                        #region GetCount
                        string strpatientnotexistcount = PullLiveDatabaseBAL.GetLiveRecord("patientnotexistcount", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                        var client1 = new RestClient(strpatientnotexistcount);
                        Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Call Appointment WithOut PatientID (patientnotexistcount) API");
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        var request1 = new RestRequest(Method.GET);
                        //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                        //request.AddHeader("cache-control", "no-cache");
                        //request.AddHeader("Authorization", Utility.WebAdminUserToken);

                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        request1.AddHeader("cache-control", "no-cache");
                        request1.AddHeader("content-type", "application/json");
                        request1.AddHeader("Authorization", Utility.WebAdminUserToken);
                        request1.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                        Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Request Sent into the API (patientnotexistcount) " + " Authorization, TokenKey & action");
                        IRestResponse response1 = client1.Execute(request1);
                        if (response1.ErrorMessage != null)
                        {
                            if (response1.ErrorMessage.Contains("The remote name could not be resolved:"))
                            { }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Pull EHRAppointment_WithOut_PatientID Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + response1.ErrorMessage);
                            }
                            Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Error Response received from API (" + response1.ErrorMessage.ToString() + ")");
                            return;
                        }

                        if (response1.Content != null)
                        {
                            Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Response received from API (" + response1.Content.ToString() + ")");
                        }
                        else
                        {
                            Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Response is null");
                        }
                        totalCount = 0;
                        batchsize = 0;
                        RemainPaging = 0;
                        pagestart = 0;
                        Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "------------------------------------Deserialize Response-------------------------------------");
                        var EHRAppointment_WithOut_PatientIDDto1 = JsonConvert.DeserializeObject<Patientnotexist_Response>(response1.Content);

                        if (EHRAppointment_WithOut_PatientIDDto1 != null)
                        {
                            Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Deserialize response EHRAppointment_WithOut_PatientIDDto1 count :  (" + EHRAppointment_WithOut_PatientIDDto1.total + ")");

                            totalCount = Convert.ToInt32(EHRAppointment_WithOut_PatientIDDto1.total);
                            batchsize = Convert.ToInt32(EHRAppointment_WithOut_PatientIDDto1.batchsize);
                        }
                        #endregion

                        RemainPaging = Math.Ceiling(Convert.ToDouble(totalCount) / batchsize);

                        DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                        DataTable dtLiveAppointment = dtLocalAppointment.Clone();
                        dtLiveAppointment.Columns.Add("InsUptDlt", typeof(int));


                        for (double i = 0; i < RemainPaging; i++)
                        {

                            string strApiEHRAppointment_WithOut_PatientID = PullLiveDatabaseBAL.GetLiveRecord("ehr_appointment_without_patientid", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                            strApiEHRAppointment_WithOut_PatientID = strApiEHRAppointment_WithOut_PatientID + "&pagestart=" + pagestart.ToString() + "&pagelimit=" + batchsize.ToString();

                            var client = new RestClient(strApiEHRAppointment_WithOut_PatientID);
                            Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Call Appointment WithOut PatientID (ehr_appointment_without_patientid) API");
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                            //request.AddHeader("cache-control", "no-cache");
                            //request.AddHeader("Authorization", Utility.WebAdminUserToken);

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Request Sent into the API  Authorization, TokenKey & action");
                            IRestResponse response = client.Execute(request);
                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                { }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Pull EHRAppointment_WithOut_PatientID Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : " + response.ErrorMessage);
                                }
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Response received from API (" + response.Content.ToString() + ")");

                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Response is null");
                            }
                            Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "------------------------------------Deserialize Response-------------------------------------");
                            var EHRAppointment_WithOut_PatientIDDto = JsonConvert.DeserializeObject<Pull_AppointmentBO>(response.Content);


                            if (EHRAppointment_WithOut_PatientIDDto != null && EHRAppointment_WithOut_PatientIDDto.data != null)
                            {
                                if (EHRAppointment_WithOut_PatientIDDto.data.Count ==0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Deserialize response AppointmentDto count :  (" + EHRAppointment_WithOut_PatientIDDto.data.Count + ") no record ");
                                }
                                //Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Deserialize Response to json (" + EHRAppointment_WithOut_PatientIDDto.message.ToString() + ")");

                                foreach (var item in EHRAppointment_WithOut_PatientIDDto.data)
                                {

                                    DataRow RowPro = dtLiveAppointment.NewRow();
                                    //RowPro["Provider_LocalDB_ID"] = item.provider_localdb_id;
                                    RowPro["Appt_EHR_ID"] = item.appt_ehr_id;
                                    RowPro["Appt_Web_ID"] = item._id;
                                    RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                    RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    dtLiveAppointment.Rows.Add(RowPro);
                                    dtLiveAppointment.AcceptChanges();
                                }
                            }
                            pagestart = pagestart + batchsize;
                        }

                        foreach (DataRow dtDtxRow in dtLiveAppointment.Rows)
                        {
                            if (Utility.CheckEHR_ID(dtDtxRow["Appt_EHR_ID"].ToString().Trim()) == string.Empty)
                            {
                                dtDtxRow["InsUptDlt"] = 0;
                                continue;
                            }

                            DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' And Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' ");
                            if (row.Length > 0)
                            {
                                dtDtxRow["InsUptDlt"] = 0;
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        dtLiveAppointment.AcceptChanges();

                        if (dtLiveAppointment != null && dtLiveAppointment.Rows.Count > 0)
                        {
                            Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Save EHRAppointment_WithOut_PatientID_Live_To_Local start");
                            bool status = PullLiveDatabaseBAL.Save_Pull_EHRAppointment_WithOut_PatientID_Live_To_Local(dtLiveAppointment, Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid);
                            if (status)
                            {
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider_Pull");
                                if (UpdateSync_Table_Datetime)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "UpdateSync_Table_Datetime  status : Success");
                                }
                                else
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "UpdateSync_Table_Datetime  status : failed");
                                }

                                ObjGoalBase.WriteToSyncLogFile("Pull EHRAppointment_WithOut_PatientID Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                IsProviderSyncPull = true;
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_ehr_appointment_without_patientid, Utility._EHRLogdirectory_ehr_appointment_without_patientid, "Save EHRAppointment_WithOut_PatientID_Live_To_Local failed");
                                IsProviderSyncPull = false;
                            }
                        }
                        else
                        {
                            IsProviderSyncPull = true;
                        }

                    }
                }

            }
            //}
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Pull EHRAppointment_WithOut_PatientID Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        #endregion


        #region Delete Appointment on Reconfigration
        private void fncSyncDataLiveDB_CheckANDDelete_Appointment()
        {
            // SynchDataLiveDB_Pull_Appointment();
            InitBgWorkerLiveDB_CheckANDDelete_Appointment();

        }
        private void InitBgWorkerLiveDB_CheckANDDelete_Appointment()
        {
            bwSynchLiveDB_CheckANDDelete_Appointment = new BackgroundWorker();
            bwSynchLiveDB_CheckANDDelete_Appointment.WorkerReportsProgress = true;
            bwSynchLiveDB_CheckANDDelete_Appointment.WorkerSupportsCancellation = true;
            bwSynchLiveDB_CheckANDDelete_Appointment.DoWork += new DoWorkEventHandler(bwSynchLiveDB_CheckANDDelete_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            //bwSynchLiveDB_CheckANDDelete_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_CheckANDDelete_Appointment_RunWorkerCompleted);
            if (bwSynchLiveDB_CheckANDDelete_Appointment.IsBusy != true)
            {
                bwSynchLiveDB_CheckANDDelete_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLiveDB_CheckANDDelete_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SynchDataLiveDB_Pull_DeletedAppointmentFromWeb();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }
        public void SynchDataLiveDB_Pull_DeletedAppointmentFromWeb()
        {
            try
            {
                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                {
                    if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                    {
                        DataTable ehrapptdata = new DataTable();
                        switch (Utility.Application_ID)
                        {
                            case 1:
                                //Loop in location table but getting data from installation table 
                                DataRow[] dra = Utility.DtInstallServiceList.Select("Installation_ID  = " + Utility.DtLocationList.Rows[j]["Service_Install_Id"]);
                                ehrapptdata = SynchEaglesoftBAL.GetEaglesoftAppointmentIds(dra[0]["DBConnString"].ToString());
                                //Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "GetEaglesoftAppointmentIds ehrapptdata count : " + ehrapptdata.Rows.Count);
                                break;
                            case 2:
                                DataRow[] drb = Utility.DtInstallServiceList.Select("Installation_ID  = " + Utility.DtLocationList.Rows[j]["Service_Install_Id"]);
                                ehrapptdata = SynchOpenDentalBAL.GetOpenDentalAppointmentIds(drb[0]["DBConnString"].ToString());
                                // Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "GetOpenDentalAppointmentIds ehrapptdata count : " + ehrapptdata.Rows.Count);
                                break;
                            case 3:
                                ehrapptdata = SynchDentrixBAL.GetDentrixAppointmentIds();
                                // Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "GetDentrixAppointmentIds ehrapptdata count : " + ehrapptdata.Rows.Count);
                                break;
                            case 5:
                                ehrapptdata = SynchClearDentBAL.GetClearDentAppointmentIds();
                                // Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "GetClearDentAppointmentIds ehrapptdata count : " + ehrapptdata.Rows.Count);
                                break;
                            case 6:
                                ehrapptdata = SynchTrackerBAL.GetTrackerAppointmentIds();
                                // Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "GetTrackerAppointmentIds ehrapptdata count : " + ehrapptdata.Rows.Count);
                                break;
                            case 8:
                                ehrapptdata = SynchEasyDentalBAL.GetEasyDentalAppointmentIds();
                                // Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "GetEasyDentalAppointmentIds ehrapptdata count : " + ehrapptdata.Rows.Count);
                                break;
                            case 7:
                                ehrapptdata = SynchPracticeWorkBAL.GetPracticeWorkAppointmentIds();
                                // Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "GetPracticeWorkAppointmentIds ehrapptdata count : " + ehrapptdata.Rows.Count);
                                break;
                        }
                        string strApiOperatory = PullLiveDatabaseBAL.GetLiveRecord("deleteappointmentfromweb", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                        var client = new RestClient(strApiOperatory);
                        Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Call deleteappointmentfromweb  API");
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        var request = new RestRequest(Method.GET);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("Authorization", Utility.WebAdminUserToken);
                        request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                        Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Request Sent into the API " + " Authorization, TokenKey & action");
                        IRestResponse response = client.Execute(request);
                        if (response.Content != null)
                        {
                            Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Response received from API( " + response.Content.ToString() + ")");
                        }
                        else
                        {
                            Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Response is null");
                        }
                        if (response.ErrorMessage != null)
                        {
                            GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_DeletedAppointmentFromWeb Pull_Response_ErrorMessage" + response.ErrorMessage.ToString());

                        }
                        Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "---------------------------------Deserialize response----------------------------------------");
                        var EHR_appointmentEhrIds = JsonConvert.DeserializeObject<Pull_CompareAppointment>(response.Content);

                        if (EHR_appointmentEhrIds != null && EHR_appointmentEhrIds.data != null)
                        {

                            if (EHR_appointmentEhrIds.data.Count == 0)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Deserialize response EHR_appointmentEhrIds count :  (" + EHR_appointmentEhrIds.data.Count + ") no record ");
                            }

                            //Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, " EHR_appointmentEhrIds : " + EHR_appointmentEhrIds.message.ToString());


                            DataTable WebApptIds = new DataTable();
                            WebApptIds.Columns.Add("Appt_EHR_ID", typeof(string));

                            //converted into datatable
                            foreach (var item in EHR_appointmentEhrIds.data)
                            {
                                WebApptIds.Rows.Add(item.appt_ehr_id);
                            }

                            List<Push_DeletedAppt> DeleteApptdataList = new List<Push_DeletedAppt>();
                            foreach (DataRow dtDtxRow in WebApptIds.Rows)
                            {
                                DataRow dr = ehrapptdata.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"] + "'").FirstOrDefault();
                                if (dr == null)
                                {
                                    Push_DeletedAppt DeleteApptdata = new Push_DeletedAppt
                                    {
                                        Location_ID = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                                        Organization_ID = Utility.Organization_ID,
                                        appt_ehr_id = dtDtxRow["Appt_EHR_ID"].ToString(),
                                        created_by = Utility.User_ID,
                                    };
                                    if (DeleteApptdata != null)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Push  DeleteApptdataList for  appt_ehr_id :" + DeleteApptdata.appt_ehr_id);
                                    }
                                    DeleteApptdataList.Add(DeleteApptdata);
                                }
                            }
                            if (DeleteApptdataList.Count > 0)
                            {
                                string strApptDelete = PullLiveDatabaseBAL.GetLiveRecord("appointmentidsfordelete", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Call appointmentidsfordelete  API");
                                string jsonString = javaScriptSerializer.Serialize(DeleteApptdataList);
                                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                var clientdoc = new RestClient(strApptDelete);
                                var requestdoc = new RestRequest(Method.POST);
                                ServicePointManager.Expect100Continue = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                requestdoc.AddHeader("Content-Type", "application/json");
                                requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_ID"].ToString()));
                                Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist , "Request Sent into the API " + " Authorization, TokenKey & action");
                                IRestResponse pushresponse = clientdoc.Execute(requestdoc);
                                if (pushresponse.ErrorMessage != null)
                                {
                                    if (pushresponse.ErrorMessage.Contains("The remote name could not be resolved:"))
                                    {
                                        ObjGoalBase.WriteToErrorLogFile("[SynchDataLiveDB_Pull_DeletedAppointmentFromWeb synch error : " + pushresponse.ErrorMessage);
                                    }
                                    else
                                    {
                                        ObjGoalBase.WriteToErrorLogFile("[SynchDataLiveDB_Pull_DeletedAppointmentFromWeb Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + pushresponse.ErrorMessage);
                                    }

                                    return;
                                }
                                if (pushresponse.Content != null)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Response received from API(" + pushresponse.Content.ToString() + ")");
                                }
                                else
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_StatusAppointmentlist, Utility._EHRLogdirectory_StatusAppointmentlist, "Response is null");
                                }
                                Utility.WriteToSyncLogFile_All("SynchDataLiveDB_Pull_DeletedAppointmentFromWeb Response Received.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Err_Compare and Delete Adit App Appointment : " + ex.Message);
            }

        }

        #endregion

        #region Appointment

        private void fncSyncDataLiveDB_Pull_Appointment()
        {
            // SynchDataLiveDB_Pull_Appointment();
            InitBgWorkerLiveDB_Pull_Appointment();
            InitBgTimerLiveDB_Pull_Appointment();
        }

        private void InitBgTimerLiveDB_Pull_Appointment()
        {
            timerSynchLiveDB_Pull_Appointment = new System.Timers.Timer();
            this.timerSynchLiveDB_Pull_Appointment.Interval = 1000 * GoalBase.intervalWebSynch_Pull_Appointment;
            this.timerSynchLiveDB_Pull_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Pull_Appointment_Tick);
            timerSynchLiveDB_Pull_Appointment.Enabled = true;
            timerSynchLiveDB_Pull_Appointment.Start();
        }

        private void InitBgWorkerLiveDB_Pull_Appointment()
        {
            bwSynchLiveDB_Pull_Appointment = new BackgroundWorker();
            bwSynchLiveDB_Pull_Appointment.WorkerReportsProgress = true;
            bwSynchLiveDB_Pull_Appointment.WorkerSupportsCancellation = true;
            bwSynchLiveDB_Pull_Appointment.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Pull_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLiveDB_Pull_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Pull_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLiveDB_Pull_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLiveDB_Pull_Appointment.Enabled = false;
                MethodForCallSynchOrderLiveDB_Pull_Appointment();
            }
        }

        public void MethodForCallSynchOrderLiveDB_Pull_Appointment()
        {
            System.Threading.Thread procThreadmainLiveDB_Pull_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Pull_Appointment);
            procThreadmainLiveDB_Pull_Appointment.Start();
        }

        public void CallSyncOrderTableLiveDB_Pull_Appointment()
        {
            if (bwSynchLiveDB_Pull_Appointment.IsBusy != true)
            {
                bwSynchLiveDB_Pull_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLiveDB_Pull_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLiveDB_Pull_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_Pull_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLiveDB_Pull_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLiveDB_Pull_Appointment.Enabled = true;
        }

        public void SynchDataLiveDB_Pull_Appointment()
        {
            if (Utility.IsApplicationIdleTimeOff) //&& Utility.AditLocationSyncEnable
            {
                try
                {
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                            DataTable dtapptStatus = SynchLocalBAL.GetLocalAppointmentStatusData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                            string strApiOperatory = PullLiveDatabaseBAL.GetLiveRecord("Appointment", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                            //Utility.WriteToSyncLogFile_All("Pull Appointment API " + strApiOperatory);
                            Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Call Appointment Write API ");
                            var client = new RestClient(strApiOperatory);
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                            //request.AddHeader("cache-control", "no-cache");
                            //request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Request Sent into the API (Appointment)  Authorization, TokenKey & action");
                            IRestResponse response = client.Execute(request);

                            //Utility.WriteToSyncLogFile_All("Pull Appointment Response " + response.ToString());
                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                { }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Response received from API (" + response.Content.ToString() + ")");
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Response is null");
                            }
                            var AppointmentDto = JsonConvert.DeserializeObject<Pull_AppointmentBO>(response.Content);

                            if (AppointmentDto != null && AppointmentDto.data != null && AppointmentDto.data.Count > 0)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "-----------------------------Deserialize response ------------------------");
                                if (AppointmentDto.data.Count == 0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Deserialize response AppointmentDto count :  (" + AppointmentDto.data.Count + ") no record ");
                                }
                                DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData_AllRecords(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                DataTable dtLiveAppointment = dtLocalAppointment.Clone();
                                dtLiveAppointment.Columns.Add("InsUptDlt", typeof(int));

                                string Appt_Operatory_Web_Id = string.Empty;
                                string Appt_Operatory_EHR_Id = string.Empty;
                                string Appt_Operatory = string.Empty;
                                string Appt_Status_EHR_Id = string.Empty;
                                string Appt_Status = string.Empty;
                                string Appt_Provider_Web_Id = string.Empty;
                                string Appt_Provider_EHR_Id = string.Empty;
                                string Appt_Providers = string.Empty;
                                int cur_Appt_EHR_ID = 0;


                                foreach (var item in AppointmentDto.data)
                                {

                                    Appt_Operatory = string.Empty;
                                    if (item.operatory.Count > 0)
                                    {
                                        Appt_Operatory = item.operatory[0].name.ToString();
                                    }

                                    Appt_Operatory_EHR_Id = string.Empty;
                                    if (item.operatory != null)
                                    {
                                        for (int i = 0; i < item.operatory.Count; i++)
                                        {
                                            Appt_Operatory_EHR_Id = Appt_Operatory_EHR_Id + item.operatory[i].Operatory_EHR_ID.ToString() + ";";
                                        }
                                        if (Appt_Operatory_EHR_Id.Length > 0)
                                        {
                                            Appt_Operatory_EHR_Id = Appt_Operatory_EHR_Id.Substring(0, Appt_Operatory_EHR_Id.Length - 1);
                                        }
                                    }

                                    Appt_Provider_EHR_Id = "0";
                                    if (item.provider != null)
                                    {
                                        if (item.provider.Count > 0)
                                        {
                                            Appt_Provider_EHR_Id = item.provider[0].Provider_EHR_ID.ToString();
                                        }
                                    }

                                    Appt_Providers = string.Empty;
                                    if (item.provider != null)
                                    {
                                        for (int i = 0; i < item.provider.Count; i++)
                                        {
                                            Appt_Providers = Appt_Providers + item.provider[i].display_name.ToString() + ";";
                                        }
                                        if (Appt_Providers.Length > 0)
                                        {
                                            Appt_Providers = Appt_Providers.Substring(0, Appt_Providers.Length - 1);
                                        }
                                    }

                                    //Appt_Providers = string.Empty;
                                    //if (item.provider != null)
                                    //{
                                    //    for (int i = 0; i < item.provider.Count; i++)
                                    //    {
                                    //        Appt_Providers = Appt_Providers + item.provider[i].display_name.ToString() + ";";
                                    //    }
                                    //    if (Appt_Providers.Length > 0)
                                    //    {
                                    //        Appt_Providers = Appt_Providers.Substring(0, Appt_Providers.Length - 1);
                                    //    }
                                    //}

                                    cur_Appt_EHR_ID = 0;
                                    try
                                    {
                                        cur_Appt_EHR_ID = Int32.Parse(item.appt_ehr_id);
                                    }
                                    catch (Exception)
                                    {
                                        cur_Appt_EHR_ID = 0;
                                    }

                                    DateTime curAppt_DateTime = Utility.Datetimesetting();
                                    try
                                    {
                                        //curAppt_DateTime = DateTime.Parse(item.shedule_time.ToString());
                                        curAppt_DateTime = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.shedule_time.ToString()));
                                    }
                                    catch (Exception)
                                    {
                                        curAppt_DateTime = Utility.Datetimesetting();
                                    }

                                    DateTime curAppt_EndDateTime = Utility.Datetimesetting();
                                    try
                                    {
                                        //curAppt_DateTime = DateTime.Parse(item.shedule_time.ToString());
                                        curAppt_EndDateTime = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.end_time.ToString()));
                                    }
                                    catch (Exception)
                                    {
                                        curAppt_EndDateTime = curAppt_DateTime.AddMinutes(30);
                                    }
                                    if (curAppt_DateTime > curAppt_EndDateTime)
                                    {
                                        curAppt_EndDateTime = curAppt_DateTime.AddMinutes(10);
                                    }
                                    string patBirthDate = "";
                                    try
                                    {
                                        patBirthDate = DateTime.Parse(Utility.ConvertApptPatientBirthdateToCurrentLocationFormat(item.birth_date.ToString())).ToString();
                                    }
                                    catch (Exception)
                                    {
                                        patBirthDate = "";
                                    }

                                    string ApptTypeEHRId = string.Empty;
                                    string ApptTypeName = string.Empty;
                                    try
                                    {
                                        //*Shruti  Need to verify
                                        if (item.appointmenttype.apptype_ehr_id != null)
                                        {
                                            ApptTypeEHRId = item.appointmenttype.apptype_ehr_id.ToString();
                                        }
                                        else
                                        {
                                            ApptTypeEHRId = "0";
                                        }
                                        if (item.appointmenttype.name != null)
                                        {
                                            ApptTypeName = item.appointmenttype.name.ToString();
                                        }
                                        else
                                        {
                                            ApptTypeName = "none";
                                        }

                                    }
                                    catch (Exception)
                                    {
                                        ApptTypeEHRId = "0";
                                        ApptTypeName = "none";
                                    }

                                    DataRow RowAppt = dtLiveAppointment.NewRow();

                                    RowAppt["Appt_EHR_ID"] = cur_Appt_EHR_ID;
                                    if (item.patient_ehr_id.ToString() != "-" || item.patient_ehr_id.ToString() != "")
                                    {
                                        RowAppt["patient_ehr_id"] = item.patient_ehr_id;
                                    }
                                    else
                                    {
                                        RowAppt["patient_ehr_id"] = "0";
                                    }
                                    RowAppt["Appt_Web_ID"] = item._id;
                                    RowAppt["Last_Name"] = item.last_name;
                                    RowAppt["First_Name"] = item.first_name;
                                    RowAppt["MI"] = string.Empty;
                                    RowAppt["birth_date"] = patBirthDate;
                                    RowAppt["Home_Contact"] = string.Empty;
                                    RowAppt["Mobile_Contact"] = item.mobile;
                                    RowAppt["Email"] = item.email;
                                    RowAppt["Address"] = string.Empty;
                                    RowAppt["City"] = string.Empty;
                                    RowAppt["ST"] = string.Empty;
                                    RowAppt["Zip"] = string.Empty;
                                    if (Appt_Operatory_EHR_Id == null || Appt_Operatory_EHR_Id.ToString() == string.Empty)
                                    {
                                        DataTable dtoperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                        if (dtoperatory.Rows.Count > 0)
                                        {
                                            Appt_Operatory_EHR_Id = dtoperatory.Rows[0]["Operatory_EHR_ID"].ToString();
                                        }
                                    }
                                    RowAppt["Operatory_EHR_ID"] = Appt_Operatory_EHR_Id;
                                    RowAppt["Operatory_Name"] = Appt_Operatory;
                                    RowAppt["Provider_EHR_ID"] = Appt_Provider_EHR_Id;
                                    RowAppt["Provider_Name"] = Appt_Providers;
                                    RowAppt["ApptType_EHR_ID"] = ApptTypeEHRId;
                                    RowAppt["ApptType"] = ApptTypeName;
                                    RowAppt["Appt_DateTime"] = curAppt_DateTime;
                                    RowAppt["Appt_EndDateTime"] = curAppt_EndDateTime;
                                    RowAppt["comment"] = item.comment;
                                    RowAppt["Status"] = "0";
                                    RowAppt["Patient_Status"] = item.patient_status;
                                    if (item.is_appointment.ToString().ToLower() == "pa")
                                    {

                                        try
                                        {
                                            if (item.appointment_status != null && item.appointment_status.ToString() != string.Empty)
                                            {
                                                DataRow drapptstatus = dtapptStatus.Copy().Select("ApptStatus_Name = '" + item.appointment_status + "'").FirstOrDefault();
                                                if (drapptstatus != null)
                                                {
                                                    Appt_Status_EHR_Id = drapptstatus["ApptStatus_EHR_ID"].ToString();
                                                    Appt_Status = item.appointment_status;
                                                }
                                            }
                                            else
                                            {
                                                Appt_Status = "pending";
                                                Appt_Status_EHR_Id = "0";
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            Appt_Status = "pending";
                                            Appt_Status_EHR_Id = "0";
                                        }
                                        try
                                        {
                                            if (item.patient_ehr_id != null && item.patient_ehr_id.ToString() != string.Empty)
                                            {
                                                RowAppt["patient_ehr_id"] = item.patient_ehr_id;
                                            }
                                            else
                                            {
                                                RowAppt["patient_ehr_id"] = "";
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            RowAppt["patient_ehr_id"] = "";
                                        }
                                        RowAppt["appointment_status_ehr_key"] = Appt_Status_EHR_Id;
                                        RowAppt["Appointment_Status"] = Appt_Status; // item.appointment_status;
                                    }
                                    else
                                    {
                                        switch (Utility.Application_ID)
                                        {
                                            case 1: // Eaglesoft
                                                RowAppt["appointment_status_ehr_key"] = "0";
                                                RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                                break;
                                            case 2: // opendental
                                                RowAppt["appointment_status_ehr_key"] = "0";
                                                RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                                break;
                                            case 3: // Dentrix
                                                RowAppt["appointment_status_ehr_key"] = "0";
                                                RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                                break;
                                            case 5: // Cleardent
                                                RowAppt["appointment_status_ehr_key"] = "1";
                                                RowAppt["Appointment_Status"] = "Booked"; // item.appointment_status;  
                                                break;
                                            case 6: // Tracker
                                                RowAppt["appointment_status_ehr_key"] = "1";
                                                RowAppt["Appointment_Status"] = "Booked"; // item.appointment_status;  
                                                break;
                                            case 8: // EzDental
                                                RowAppt["appointment_status_ehr_key"] = "0";
                                                RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                                break;
                                            case 7: // PracticeWork 
                                                RowAppt["appointment_status_ehr_key"] = "0";
                                                RowAppt["Appointment_Status"] = "pending"; // item.appointment_status;  
                                                break;
                                        }
                                    }
                                    RowAppt["Is_Appt"] = item.is_appointment;
                                    RowAppt["appt_treatmentcode"] = item.appt_treatmentcode;
                                    RowAppt["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                    RowAppt["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    RowAppt["confirmed_status_ehr_key"] = item.confirmed_status_ehr_key;
                                    RowAppt["confirmed_status"] = item.confirmed_status;
                                    dtLiveAppointment.Rows.Add(RowAppt);
                                    dtLiveAppointment.AcceptChanges();
                                }

                                if (dtLiveAppointment != null)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Datatable dtLiveAppointment count  :  (" + dtLiveAppointment.Rows.Count.ToString() + ")");
                                }

                                //ObjGoalBase.WriteToErrorLogFile("Pull Appointment Loop END ");

                                foreach (DataRow dtDtxRow in dtLiveAppointment.Rows)
                                {
                                    DataRow[] row = dtLocalAppointment.Copy().Select("Appt_Web_ID = '" + dtDtxRow["Appt_Web_ID"].ToString() + "' And  Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' ");
                                    if (row.Length > 0)
                                    {
                                        if (row[0]["Appt_EHR_ID"].ToString() == "0" && Convert.ToBoolean(row[0]["Is_Appt_DoubleBook"].ToString()) == false)
                                        {
                                            dtDtxRow["InsUptDlt"] = 2;
                                        }
                                        else
                                        {
                                            dtDtxRow["InsUptDlt"] = 7;
                                        }
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 1;
                                    }
                                }
                                //ObjGoalBase.WriteToErrorLogFile("Pull Appointment Loop END  another");
                                dtLiveAppointment.AcceptChanges();

                                if (dtLiveAppointment != null && dtLiveAppointment.Rows.Count > 0)
                                {
                                    //Utility.WriteToSyncLogFile_All("Send To save appointment");
                                    Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Save Datatable dtLiveAppointment(Appointment) into  live to local start");
                                    bool status = PullLiveDatabaseBAL.Save_Appointment_Live_To_Local(dtLiveAppointment, Utility._filename_appointment, Utility._EHRLogdirectory_appointment);
                                    if (status)
                                    {
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment_Pull");
                                        ObjGoalBase.WriteToSyncLogFile("Appointment Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                    }
                                    if (Utility.Application_ID == 5)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Sync start local to cleardent");
                                        SynchDataLocalToClearDent_Appointment(Utility._filename_appointment, Utility._EHRLogdirectory_appointment);
                                    }

                                }
                                else
                                {
                                    ObjGoalBase.WriteToSyncLogFile("Appointment Sync (Adit Server To Local Database).. No Records Found. Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                }
                            }
                            else
                            {
                                ObjGoalBase.WriteToSyncLogFile("Appointment Sync (Adit Server To Local Database).. No Records Found. Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Appointment Sync (Adit Server To Local Database).. No Records Found. Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (Adit Server To Local Database)] : " + ex.Message);
                }
            }
        }


        #endregion

        #region EHR_appointment Update_Status

        private void fncSyncDataLiveDB_Pull_EHR_appointment()
        {
            // SynchDataLiveDB_Pull_EHR_appointment();
            InitBgWorkerLiveDB_Pull_EHR_appointment();
            InitBgTimerLiveDB_Pull_EHR_appointment();
        }

        private void InitBgTimerLiveDB_Pull_EHR_appointment()
        {
            timerSynchLiveDB_Pull_EHR_appointment = new System.Timers.Timer();
            this.timerSynchLiveDB_Pull_EHR_appointment.Interval = 1000 * GoalBase.intervalWebSynch_Pull_Appointment;
            this.timerSynchLiveDB_Pull_EHR_appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Pull_EHR_appointment_Tick);
            timerSynchLiveDB_Pull_EHR_appointment.Enabled = true;
            timerSynchLiveDB_Pull_EHR_appointment.Start();
        }

        private void InitBgWorkerLiveDB_Pull_EHR_appointment()
        {
            bwSynchLiveDB_Pull_EHR_appointment = new BackgroundWorker();
            bwSynchLiveDB_Pull_EHR_appointment.WorkerReportsProgress = true;
            bwSynchLiveDB_Pull_EHR_appointment.WorkerSupportsCancellation = true;
            bwSynchLiveDB_Pull_EHR_appointment.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Pull_EHR_appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLiveDB_Pull_EHR_appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Pull_EHR_appointment_RunWorkerCompleted);
        }

        private void timerSynchLiveDB_Pull_EHR_appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLiveDB_Pull_EHR_appointment.Enabled = false;
                MethodForCallSynchOrderLiveDB_Pull_EHR_appointment();
            }
        }

        public void MethodForCallSynchOrderLiveDB_Pull_EHR_appointment()
        {
            System.Threading.Thread procThreadmainLiveDB_Pull_EHR_appointment = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Pull_EHR_appointment);
            procThreadmainLiveDB_Pull_EHR_appointment.Start();
        }

        public void CallSyncOrderTableLiveDB_Pull_EHR_appointment()
        {
            if (bwSynchLiveDB_Pull_EHR_appointment.IsBusy != true)
            {
                bwSynchLiveDB_Pull_EHR_appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLiveDB_Pull_EHR_appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLiveDB_Pull_EHR_appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_Pull_EHR_appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLiveDB_Pull_EHR_appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLiveDB_Pull_EHR_appointment.Enabled = true;
        }

        public void SynchDataLiveDB_Pull_EHR_appointment()
        {
            try
            {
                GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Start");
                if (Utility.IsApplicationIdleTimeOff)  //&& Utility.AditLocationSyncEnable
                {
                    // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appppointment IsApplicationIdleTimeOff True");
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Loop Started " + j.ToString());
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment AditLocation Sync Enable " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["ApptAutoBook"].ToString()))
                            {
                                string strApiappointment = PullLiveDatabaseBAL.GetLiveRecord("EHR_appointment", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                                // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Pull_" + strApiappointment);
                                var client = new RestClient(strApiappointment);
                                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Call EHRAppointment Confirm API");
                                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                var request = new RestRequest(Method.GET);

                                ServicePointManager.Expect100Continue = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                request.AddHeader("cache-control", "no-cache");
                                request.AddHeader("content-type", "application/json");
                                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Request Sent into the API  " + " Authorization, TokenKey & action");
                                //Utility.WriteToErrorLogFromAll("API Url " + strApiappointment.ToString());
                                IRestResponse response = client.Execute(request);
                                //Utility.WriteToErrorLogFromAll("Pull Appointment Response " + response.ToString());

                                if (response.ErrorMessage != null)
                                {
                                    // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Pull_Response_ErrorMessage" + response.ErrorMessage.ToString());
                                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                    {
                                        ObjGoalBase.WriteToErrorLogFile("[EHR_appointment Adit App to EHR Err  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "   " + response.ErrorMessage);
                                    }
                                    else
                                    {
                                        ObjGoalBase.WriteToErrorLogFile("[EHR_appointment Adit App to EHR Err  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "   " + response.ErrorMessage);
                                    }
                                    return;
                                }
                                if (response.Content != null)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Response received from API(" + response.Content.ToString() + ")");
                                }
                                else
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Response is null");
                                }

                                //GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Pull_Response_Content" + response.Content.ToString());
                                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "----------------------------Deserialize repsonse--------------------------------");
                                var EHR_appointmentDto = JsonConvert.DeserializeObject<Pull_AppointmentBO>(response.Content);

                                if (EHR_appointmentDto != null && EHR_appointmentDto.data != null)
                                {
                                    if (EHR_appointmentDto.data.Count == 0)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Deserialize response EHR_appointmentDto count :  (" + EHR_appointmentDto.data.Count + ") no record ");
                                    }

                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Deserialize repsonse : (" + EHR_appointmentDto.message.ToString() + ")");
                                    if (Utility.Application_ID == 11)
                                    {
                                        // Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Application_ID : " + Utility.Application_ID.ToString() + ")");
                                        DataTable dtLiveEHR_appointment = new DataTable();
                                        dtLiveEHR_appointment.Columns.Add("Appt_EHR_ID", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("Appt_Web_ID", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("confirmed_status_ehr_key", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("confirmed_status", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("InsUptDlt", typeof(int));
                                        dtLiveEHR_appointment.Columns.Add("Clinic_Number", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("Service_Install_ID", typeof(string));

                                        string cur_Appt_EHR_ID = string.Empty;


                                        foreach (var item in EHR_appointmentDto.data)
                                        {
                                            cur_Appt_EHR_ID = "";
                                            try
                                            {
                                                cur_Appt_EHR_ID = item.appt_ehr_id;
                                            }
                                            catch (Exception)
                                            {
                                                cur_Appt_EHR_ID = "";
                                            }

                                            if (cur_Appt_EHR_ID != "" && cur_Appt_EHR_ID != string.Empty)
                                            {
                                                DataTable AppointmentConformStatus = SynchLocalBAL.GetLocalAppointmentConformStatusData(cur_Appt_EHR_ID.ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());

                                                if (AppointmentConformStatus != null && AppointmentConformStatus.Rows.Count > 0)
                                                {
                                                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "AppointmentConfirmStatus count : (" + AppointmentConformStatus.Rows.Count.ToString() + ")");
                                                    DataRow RowAppt = dtLiveEHR_appointment.NewRow();
                                                    RowAppt["Appt_EHR_ID"] = cur_Appt_EHR_ID;
                                                    RowAppt["Appt_Web_ID"] = item._id;
                                                    RowAppt["confirmed_status_ehr_key"] = item.confirmed_status_ehr_key;
                                                    RowAppt["confirmed_status"] = item.confirmed_status;
                                                    RowAppt["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowAppt["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                    dtLiveEHR_appointment.Rows.Add(RowAppt);
                                                    dtLiveEHR_appointment.AcceptChanges();
                                                    GoalBase.WriteToSyncLogFile_Static("StatusAppointmentlist(" + cur_Appt_EHR_ID + "-" + item.confirmed_status_ehr_key + ")");
                                                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "dtLiveEHR_appointment count : (" + dtLiveEHR_appointment.Rows.Count.ToString() + ")");
                                                }
                                            }
                                        }

                                        if (dtLiveEHR_appointment != null && dtLiveEHR_appointment.Rows.Count > 0)
                                        {
                                            //CheckEntryUserLoginIdExist();
                                            bool status = false;
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Update_Status_EHR_Appointment_Live_To_AbelDentEHR count : (" + dtLiveEHR_appointment.Rows.Count.ToString() + ")");
                                            status = SynchAbelDentBAL.Update_Status_EHR_Appointment_Live_To_AbelDentEHR(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            if(status)
                                            {

                                                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Update_Status_EHR_Appointment_Live_To_AbelDentEHR  : Success");

                                                if(SynchAbelDentBAL.Insert_Status_EHR_Appointment_To_AbelDentEHR(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment))
                                                {
                                                    if (status)
                                                    {
                                                        Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Insert_Status_EHR_Appointment_Live_To_AbelDentEHR  : Success");
                                                        Update_Status_EHR_Appointment_EHR_To_Live(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                                    }
                                                    else if (Utility.AppointmentEHRIds.ToString() != "")
                                                    {
                                                        GoalBase.WriteToSyncLogFile_Static("StatusAppointmentlist(" + Utility.AppointmentEHRIds.ToString() + ") Sync Update on Adit Server With Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                                    }
                                                }
                                                else
                                                {
                                                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Insert_Status_EHR_Appointment_Live_To_AbelDentEHR  : failde");
                                                }
                                            }
                                            else
                                            {
                                                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Update_Status_EHR_Appointment_Live_To_AbelDentEHR  : failed");
                                            }
                                        }

                                    }
                                    else
                                    {

                                        DataTable dtLiveEHR_appointment = new DataTable();
                                        dtLiveEHR_appointment.Columns.Add("Appt_EHR_ID", typeof(Int64));
                                        dtLiveEHR_appointment.Columns.Add("Appt_Web_ID", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("confirmed_status_ehr_key", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("confirmed_status", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("InsUptDlt", typeof(int));
                                        dtLiveEHR_appointment.Columns.Add("Clinic_Number", typeof(string));
                                        dtLiveEHR_appointment.Columns.Add("Service_Install_ID", typeof(string));

                                        Int64 cur_Appt_EHR_ID = 0;

                                        foreach (var item in EHR_appointmentDto.data)
                                        {
                                            cur_Appt_EHR_ID = 0;
                                            try
                                            {
                                                cur_Appt_EHR_ID = Convert.ToInt64(item.appt_ehr_id);
                                            }
                                            catch (Exception)
                                            {
                                                cur_Appt_EHR_ID = 0;
                                            }

                                            if (cur_Appt_EHR_ID != 0)
                                            {
                                                DataTable AppointmentConformStatus = SynchLocalBAL.GetLocalAppointmentConformStatusData(cur_Appt_EHR_ID.ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);

                                                if (AppointmentConformStatus != null && AppointmentConformStatus.Rows.Count > 0)
                                                {
                                                    DataRow RowAppt = dtLiveEHR_appointment.NewRow();
                                                    RowAppt["Appt_EHR_ID"] = cur_Appt_EHR_ID;
                                                    RowAppt["Appt_Web_ID"] = item._id;
                                                    RowAppt["confirmed_status_ehr_key"] = item.confirmed_status_ehr_key;
                                                    RowAppt["confirmed_status"] = item.confirmed_status;
                                                    RowAppt["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowAppt["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                    dtLiveEHR_appointment.Rows.Add(RowAppt);
                                                    dtLiveEHR_appointment.AcceptChanges();
                                                }
                                                //Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "GetLocalAppointmentConformStatusData dtLiveEHR_appointment count : " + dtLiveEHR_appointment.Rows.Count);
                                            }
                                        }
                                        // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Count " + dtLiveEHR_appointment.Rows.Count.ToString());
                                        if (dtLiveEHR_appointment != null && dtLiveEHR_appointment.Rows.Count > 0)
                                        {
                                            Utility.CheckEntryUserLoginIdExist();
                                            bool status = false;

                                            if (Utility.Application_ID == 1)
                                            {
                                                Utility.AppointmentEHRIds = "";
                                                // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment Send to Save " + dtLiveEHR_appointment.Rows.Count.ToString());
                                                status = SynchEaglesoftBAL.Update_Status_EHR_Appointment_Live_To_Eaglesoft(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            else if (Utility.Application_ID == 2)
                                            {
                                                status = SynchOpenDentalBAL.Update_Status_EHR_Appointment_Live_To_Opendental(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            else if (Utility.Application_ID == 3)
                                            {
                                                status = SynchDentrixBAL.Update_Status_EHR_Appointment_Live_To_DentrixEHR(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            else if (Utility.Application_ID == 5)
                                            {
                                                status = SynchClearDentBAL.Update_Status_EHR_Appointment_Live_To_ClearDentEHR(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            else if (Utility.Application_ID == 6)
                                            {
                                                status = SynchTrackerBAL.Update_Status_EHR_Appointment_Live_To_TrackerEHR(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            else if (Utility.Application_ID == 7)
                                            {
                                                status = SynchPracticeWorkBAL.Update_Status_EHR_Appointment_Live_To_PracticeWorkEHR(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            else if (Utility.Application_ID == 8)
                                            {
                                                status = SynchEasyDentalBAL.Update_Status_EHR_Appointment_Live_To_EasyDentalEHR(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            else if (Utility.Application_ID == 10)
                                            {
                                                status = SynchPracticeWebBAL.Update_Status_EHR_Appointment_Live_To_PracticeWeb(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            if (status && Utility.Application_ID != 1)
                                            {
                                                Update_Status_EHR_Appointment_EHR_To_Live(dtLiveEHR_appointment, Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                                            }
                                            else if (Utility.AppointmentEHRIds.ToString() != "" && Utility.Application_ID == 1)
                                            {
                                                GoalBase.WriteToSyncLogFile_Static("StatusAppointmentlist(" + Utility.AppointmentEHRIds.ToString() + ") Sync Update on Adit Server With Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[EHR_appointment Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        public static void Update_Status_EHR_Appointment_EHR_To_Live(DataTable dtStatusAppointmentlist, string _filename_EHR_appointment = "", string _EHRLogdirectory_EHR_appointment = "")
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                var JsonStatusAppointmentlist = new System.Text.StringBuilder();
                bool IsSynchStatusAppointmentlist = true;
                string StatusAppointmentlistForLog = "";
                if (dtStatusAppointmentlist != null)
                {
                    Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "dtStatusAppointmentlist count  : " + dtStatusAppointmentlist.Rows.Count);
                }
                if (dtStatusAppointmentlist.Rows.Count > 0)
                {
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            string strStatusAppointmentlist = "";
                            foreach (DataRow dtStatusAppointmentlistrow in dtStatusAppointmentlist.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "'  "))
                            {
                                Push_ChangeStatusAppointmentListBO StatusAppointmentlist = new Push_ChangeStatusAppointmentListBO
                                {
                                    Location_ID = Utility.DtLocationList.Rows[j]["Location_ID"].ToString(), //Utility.Location_ID,
                                    Organization_ID = Utility.Organization_ID,
                                    appt_ehr_id = dtStatusAppointmentlistrow["Appt_EHR_ID"].ToString().Trim(),
                                    created_by = Utility.User_ID
                                };
                                if (StatusAppointmentlist != null)
                                {
                                    Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Push Change Status Appointment List  appt_ehr_id  : " + StatusAppointmentlist.appt_ehr_id);
                                }
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                JsonStatusAppointmentlist.Append(javaScriptSerializer.Serialize(StatusAppointmentlist) + ",");

                                StatusAppointmentlistForLog = dtStatusAppointmentlistrow["Appt_EHR_ID"].ToString().Trim() + ", " + StatusAppointmentlistForLog;
                                Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Push StatusAppointmentlistForLog  : " + StatusAppointmentlistForLog);
                            }

                            if (JsonStatusAppointmentlist.Length > 0)
                            {
                                string jsonString = "[" + JsonStatusAppointmentlist.ToString().Remove(JsonStatusAppointmentlist.Length - 1) + "]";

                                Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Send json request   : " + jsonString);
                                Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Push_Local_To_LiveDatabase_StatusAppointmentlist  start ");
                                strStatusAppointmentlist = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_StatusAppointmentlist(jsonString, "StatusAppointmentlist", Utility.DtLocationList.Rows[j]["Location_ID"].ToString(), _filename_EHR_appointment, _EHRLogdirectory_EHR_appointment);
                            }

                            if (strStatusAppointmentlist.ToLower() == "Success".ToLower())
                            {
                                IsSynchStatusAppointmentlist = true;

                                Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Push_Local_To_LiveDatabase_StatusAppointmentlist status   : " + strStatusAppointmentlist.ToLower());
                            }
                            else
                            {
                                if (strStatusAppointmentlist.Contains("The remote name could not be resolved:"))
                                {
                                    IsSynchStatusAppointmentlist = false;
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[StatusAppointmentlist Sync (Local Database To Adit Server) ] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic :  " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + strStatusAppointmentlist);
                                    IsSynchStatusAppointmentlist = false;
                                    Utility.WriteSyncPullLog(_filename_EHR_appointment, _EHRLogdirectory_EHR_appointment, "Push_Local_To_LiveDatabase_StatusAppointmentlist status   : " + IsSynchStatusAppointmentlist.ToString ());
                                }

                            }

                        }
                    }
                }
                if (IsSynchStatusAppointmentlist)
                {
                    GoalBase.WriteToSyncLogFile_Static("StatusAppointmentlist(" + StatusAppointmentlistForLog + ") Sync Update on Adit Server Successfully.");
                }
            }
            //}
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[StatusAppointmentlist  Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #region Provider

        private void fncSyncDataLiveDB_Pull_Provider()
        {
            //SynchDataLiveDB_Pull_Provider();
            InitBgWorkerLiveDB_Pull_Provider();
            InitBgTimerLiveDB_Pull_Provider();
        }

        private void InitBgTimerLiveDB_Pull_Provider()
        {

            timerSynchLiveDB_Pull_Provider = new System.Timers.Timer();
            this.timerSynchLiveDB_Pull_Provider.Interval = 1000 * GoalBase.intervalWebSynch_Pull_Provider;
            this.timerSynchLiveDB_Pull_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Pull_Provider_Tick);
            timerSynchLiveDB_Pull_Provider.Enabled = true;
            timerSynchLiveDB_Pull_Provider.Start();
        }

        private void InitBgWorkerLiveDB_Pull_Provider()
        {
            bwSynchLiveDB_Pull_Provider = new BackgroundWorker();
            bwSynchLiveDB_Pull_Provider.WorkerReportsProgress = true;
            bwSynchLiveDB_Pull_Provider.WorkerSupportsCancellation = true;
            bwSynchLiveDB_Pull_Provider.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Pull_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLiveDB_Pull_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Pull_Provider_RunWorkerCompleted);
        }

        private void timerSynchLiveDB_Pull_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLiveDB_Pull_Provider.Enabled = false;
                MethodForCallSynchOrderLiveDB_Pull_Provider();
            }
        }

        public void MethodForCallSynchOrderLiveDB_Pull_Provider()
        {
            System.Threading.Thread procThreadmainLiveDB_Pull_Provider = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Pull_Provider);
            procThreadmainLiveDB_Pull_Provider.Start();
        }

        public void CallSyncOrderTableLiveDB_Pull_Provider()
        {
            if (bwSynchLiveDB_Pull_Provider.IsBusy != true)
            {
                bwSynchLiveDB_Pull_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLiveDB_Pull_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLiveDB_Pull_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_Pull_Provider();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLiveDB_Pull_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLiveDB_Pull_Provider.Enabled = true;
        }

        public void SynchDataLiveDB_Pull_Provider()
        {
            try
            {


                if (Utility.IsApplicationIdleTimeOff) //&& Utility.AditLocationSyncEnable
                {
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            string strApiProviders = PullLiveDatabaseBAL.GetLiveRecord("Provider", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                            var client = new RestClient(strApiProviders);
                            Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Call Provider  API");
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                            //request.AddHeader("cache-control", "no-cache");
                            //request.AddHeader("Authorization", Utility.WebAdminUserToken);

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Request Sent into the API  " + " Authorization, TokenKey & action");
                            IRestResponse response = client.Execute(request);
                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                { }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Provider Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                return;
                            }

                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Response received from API(" + response.Content.ToString() + ")");
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Response is null");
                            }
                            Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "----------------------------Deserialize repsonse--------------------------------");
                            var ProvidersDto = JsonConvert.DeserializeObject<Pull_ProvidersBO>(response.Content);


                            if (ProvidersDto != null && ProvidersDto.data != null)
                            {
                                if (ProvidersDto.data.Count == 0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Deserialize response ProvidersDto count :  (" + ProvidersDto.data.Count + ") no record ");
                                }
                                //Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Deserialize repsonse from API(" + ProvidersDto.message.ToString() + ")");

                                DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                DataTable dtLiveProvider = dtLocalProvider.Clone();
                                dtLiveProvider.Columns.Add("InsUptDlt", typeof(int));

                                string Prov_Specialities = string.Empty;
                                string Prov_education = string.Empty;
                                string accreditation = string.Empty;
                                string membership = string.Empty;
                                string language = string.Empty;
                                foreach (var item in ProvidersDto.data)
                                {
                                    Prov_Specialities = string.Empty;
                                    Prov_education = string.Empty;
                                    for (int i = 0; i < item.Specialities.Count; i++)
                                    {
                                        Prov_Specialities = Prov_Specialities + item.Specialities[i].name.ToString() + ";";
                                    }
                                    if (Prov_Specialities.Length > 0)
                                    {
                                        Prov_Specialities = Prov_Specialities.Substring(0, Prov_Specialities.Length - 1);
                                    }
                                    for (int i = 0; i < item.education.Count; i++)
                                    {
                                        Prov_education = Prov_education + item.education[i].degree.ToString() + ";";
                                    }
                                    if (Prov_education.Length > 0)
                                    {
                                        Prov_education = Prov_education.Substring(0, Prov_education.Length - 1);
                                    }

                                    accreditation = string.Empty;
                                    try
                                    {
                                        if (item != null && item.accredation.Count() > 0)
                                        {
                                            accreditation = item.accredation[0].ToString();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        accreditation = string.Empty;
                                    }

                                    membership = string.Empty;
                                    try
                                    {
                                        if (item != null && item.membership.Count() > 0)
                                        {
                                            membership = item.membership[0].ToString();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        membership = string.Empty;
                                    }

                                    language = string.Empty;
                                    try
                                    {
                                        if (item != null && item.languages.Count() > 0)
                                        {
                                            language = item.languages[0].ToString();
                                        }

                                        //language = Utility.ConvertvarToString(item.languages[0].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        language = string.Empty;
                                    }

                                    DataRow RowPro = dtLiveProvider.NewRow();
                                    //RowPro["Provider_LocalDB_ID"] = item.provider_localdb_id;
                                    RowPro["Provider_EHR_ID"] = item.provider_ehr_id;
                                    RowPro["Provider_Web_ID"] = item._id;
                                    RowPro["Last_Name"] = item.last_name;
                                    RowPro["First_Name"] = item.first_name;
                                    RowPro["MI"] = string.Empty;
                                    RowPro["gender"] = item.gender;
                                    RowPro["provider_speciality"] = Prov_Specialities;
                                    RowPro["bio"] = item.bio;
                                    RowPro["education"] = Prov_education;
                                    RowPro["accreditation"] = accreditation;
                                    RowPro["membership"] = membership;
                                    RowPro["language"] = language;
                                    RowPro["age_treated_min"] = item.treatment_min_age;
                                    RowPro["age_treated_max"] = item.treatment_max_age;
                                    RowPro["is_active"] = item.is_active;
                                    RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                    RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    dtLiveProvider.Rows.Add(RowPro);
                                    dtLiveProvider.AcceptChanges();
                                }

                                foreach (DataRow dtDtxRow in dtLiveProvider.Rows)
                                {
                                    if (Utility.CheckEHR_ID(dtDtxRow["Provider_EHR_ID"].ToString().Trim()) != string.Empty)
                                    {
                                        dtDtxRow["InsUptDlt"] = 6;
                                        continue;
                                    }

                                    DataRow[] row = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtDtxRow["Provider_EHR_ID"].ToString().Trim() + "'");
                                    if (row.Length > 0)
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 1;
                                    }
                                }

                                dtLiveProvider.AcceptChanges();
                                if (dtLiveProvider != null && dtLiveProvider.Rows.Count > 0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "data table dtLiveProvider count  (" + dtLiveProvider.Rows .Count  + ")");
                                    //Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Deserialize repsonse from API(" + ProvidersDto.message.ToString() + ")");

                                    bool status = PullLiveDatabaseBAL.Save_Provider_Live_To_Local(dtLiveProvider, Utility._filename_Provider , Utility._EHRLogdirectory_Provider );

                                    if (status)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Save_Provider_Live_To_Local status  : Success ");
                                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider_Pull");
                                        if(UpdateSync_Table_Datetime)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Update_Sync_Table_Datetime status  : Success ");
                                        }
                                        else
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Update_Sync_Table_Datetime status  : failed ");
                                        }
                                        ObjGoalBase.WriteToSyncLogFile("Providers Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                        IsProviderSyncPull = true;
                                    }
                                    else
                                    {
                                        IsProviderSyncPull = false;
                                        Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "Save_Provider_Live_To_Local status  : failed ");
                                    }
                                }
                                else
                                {
                                    IsProviderSyncPull = true;
                                    Utility.WriteSyncPullLog(Utility._filename_Provider, Utility._EHRLogdirectory_Provider, "ProviderSyncPul status  : success ");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Provider Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        #endregion

        #region Operatory

        private void fncSyncDataLiveDB_Pull_Operatory()
        {
            // SynchDataLiveDB_Pull_Operatory();
            InitBgWorkerLiveDB_Pull_Operatory();
            InitBgTimerLiveDB_Pull_Operatory();
        }

        private void InitBgTimerLiveDB_Pull_Operatory()
        {
            timerSynchLiveDB_Pull_Operatory = new System.Timers.Timer();
            this.timerSynchLiveDB_Pull_Operatory.Interval = 1000 * GoalBase.intervalWebSynch_Pull_Operatory;
            this.timerSynchLiveDB_Pull_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Pull_Operatory_Tick);
            timerSynchLiveDB_Pull_Operatory.Enabled = true;
            timerSynchLiveDB_Pull_Operatory.Start();
        }

        private void InitBgWorkerLiveDB_Pull_Operatory()
        {
            bwSynchLiveDB_Pull_Operatory = new BackgroundWorker();
            bwSynchLiveDB_Pull_Operatory.WorkerReportsProgress = true;
            bwSynchLiveDB_Pull_Operatory.WorkerSupportsCancellation = true;
            bwSynchLiveDB_Pull_Operatory.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Pull_Operatory_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLiveDB_Pull_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Pull_Operatory_RunWorkerCompleted);
        }

        private void timerSynchLiveDB_Pull_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLiveDB_Pull_Operatory.Enabled = false;
                MethodForCallSynchOrderLiveDB_Pull_Operatory();
            }
        }

        public void MethodForCallSynchOrderLiveDB_Pull_Operatory()
        {
            System.Threading.Thread procThreadmainLiveDB_Pull_Operatory = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Pull_Operatory);
            procThreadmainLiveDB_Pull_Operatory.Start();
        }

        public void CallSyncOrderTableLiveDB_Pull_Operatory()
        {
            if (bwSynchLiveDB_Pull_Operatory.IsBusy != true)
            {
                bwSynchLiveDB_Pull_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLiveDB_Pull_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLiveDB_Pull_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_Pull_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLiveDB_Pull_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLiveDB_Pull_Operatory.Enabled = true;
        }

        public void SynchDataLiveDB_Pull_Operatory()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            string strApiOperatory = PullLiveDatabaseBAL.GetLiveRecord("Operatory", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                            var client = new RestClient(strApiOperatory);
                            Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "Call Operatory  API");
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                            //request.AddHeader("cache-control", "no-cache");
                            //request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "Request Sent into the API"   + " Authorization, TokenKey & action");
                            IRestResponse response = client.Execute(request);

                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                { }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                return;
                            }

                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "Response received from API(" + response.Content.ToString() + ")");
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "Response is null");
                            }

                            Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "----------------------------Deserialize repsonse--------------------------------");
                            var OperatoryDto = JsonConvert.DeserializeObject<Pull_OperatoryBO>(response.Content);




                            if (OperatoryDto != null && OperatoryDto.data != null)
                            {
                                // Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "Deserialize repsonse from API(" + OperatoryDto.message.ToString() + ")");
                                if (OperatoryDto.data.Count == 0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "Deserialize response OperatoryDto count :  (" + OperatoryDto.data.Count + ") no record ");
                                }

                                DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                DataTable dtLiveOperatory = dtLocalOperatory.Clone();
                                dtLiveOperatory.Columns.Add("InsUptDlt", typeof(int));

                                foreach (var item in OperatoryDto.data)
                                {
                                    DataRow RowPro = dtLiveOperatory.NewRow();
                                    //RowPro["Operatory_LocalDB_ID"] = item.operatory_localdb_id;
                                    RowPro["Operatory_EHR_ID"] = item.operatory_ehr_id;
                                    RowPro["Operatory_Web_ID"] = item._id;
                                    RowPro["Operatory_Name"] = item.name;
                                    RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                    RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    dtLiveOperatory.Rows.Add(RowPro);
                                    dtLiveOperatory.AcceptChanges();
                                }
                                //Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "data table OperatoryDto count  (" + dtLiveOperatory.Rows.Count + ")");
                                foreach (DataRow dtDtxRow in dtLiveOperatory.Rows)
                                {
                                    if (Utility.CheckEHR_ID(dtDtxRow["Operatory_EHR_ID"].ToString().Trim()) != string.Empty)
                                    {
                                        dtDtxRow["InsUptDlt"] = 6;
                                        continue;
                                    }

                                    DataRow[] row = dtLocalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtDtxRow["Operatory_EHR_ID"].ToString().Trim() + "'");
                                    if (row.Length > 0)
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 1;
                                    }
                                }
                                dtLiveOperatory.AcceptChanges();

                                if (dtLiveOperatory != null && dtLiveOperatory.Rows.Count > 0)
                                {
                                    bool status = PullLiveDatabaseBAL.Save_Operatory_Live_To_Local(dtLiveOperatory, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility._filename_Operatory , Utility._EHRLogdirectory_Operatory);

                                    if (status)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "data table dtLiveOperatory count  (" + dtLiveOperatory.Rows.Count + ")");
                                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory_Pull");

                                        if(UpdateSync_Table_Datetime)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_Operatory, Utility._EHRLogdirectory_Operatory, "UpdateSync Table Datetime  status  : " + UpdateSync_Table_Datetime.ToString ());
                                        }
                                        ObjGoalBase.WriteToSyncLogFile("Operatory Sync (Adit Server To Local Database) Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (Adit Server To Local Database) ] : " + ex.Message);
            }
        }

        #endregion

        #region Appointment Type

        private void fncSyncDataLiveDB_Pull_ApptType()
        {
            InitBgWorkerLiveDB_Pull_ApptType();
            InitBgTimerLiveDB_Pull_ApptType();
        }

        private void InitBgTimerLiveDB_Pull_ApptType()
        {
            timerSynchLiveDB_Pull_ApptType = new System.Timers.Timer();
            this.timerSynchLiveDB_Pull_ApptType.Interval = 1000 * GoalBase.intervalWebSynch_Pull_ApptType;
            this.timerSynchLiveDB_Pull_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Pull_ApptType_Tick);
            timerSynchLiveDB_Pull_ApptType.Enabled = true;
            timerSynchLiveDB_Pull_ApptType.Start();
        }

        private void InitBgWorkerLiveDB_Pull_ApptType()
        {
            bwSynchLiveDB_Pull_ApptType = new BackgroundWorker();
            bwSynchLiveDB_Pull_ApptType.WorkerReportsProgress = true;
            bwSynchLiveDB_Pull_ApptType.WorkerSupportsCancellation = true;
            bwSynchLiveDB_Pull_ApptType.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Pull_ApptType_DoWork);
            bwSynchLiveDB_Pull_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Pull_ApptType_RunWorkerCompleted);
        }

        private void timerSynchLiveDB_Pull_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLiveDB_Pull_ApptType.Enabled = false;
                MethodForCallSynchOrderLiveDB_Pull_ApptType();
            }
        }

        public void MethodForCallSynchOrderLiveDB_Pull_ApptType()
        {
            System.Threading.Thread procThreadmainLiveDB_Pull_ApptType = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Pull_ApptType);
            procThreadmainLiveDB_Pull_ApptType.Start();
        }

        public void CallSyncOrderTableLiveDB_Pull_ApptType()
        {
            if (bwSynchLiveDB_Pull_ApptType.IsBusy != true)
            {
                bwSynchLiveDB_Pull_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLiveDB_Pull_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLiveDB_Pull_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_Pull_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLiveDB_Pull_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLiveDB_Pull_ApptType.Enabled = true;
        }

        public void SynchDataLiveDB_Pull_ApptType()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff) //&& Utility.AditLocationSyncEnable
                {
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            string strApiApptTypes = PullLiveDatabaseBAL.GetLiveRecord("type", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                            var client = new RestClient(strApiApptTypes);
                            Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "Call ApptType Confirmation API");
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                            //request.AddHeader("cache-control", "no-cache");
                            //request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "Request Sent into the API " + " Authorization, TokenKey & action");
                            IRestResponse response = client.Execute(request);
                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                { }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Appointment Type Sync (Adit Server To Local Database)] Service Install Id :  " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + response.ErrorMessage);
                                }
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "Response received from API(" + response.Content.ToString() + ")");
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "Response is null");
                            }
                            Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "----------------------------Deserialize repsonse--------------------------------");
                            var ApptTypeDto = JsonConvert.DeserializeObject<Pull_ApptTypeBO>(response.Content);

                            if (ApptTypeDto != null && ApptTypeDto.data != null)
                            {
                                //Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "Deserialize repsonse from API(" + ApptTypeDto.message.ToString() + ")");
                                if (ApptTypeDto.data.Count == 0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "Deserialize response ApptTypeDto count :  (" + ApptTypeDto.data.Count + ") no record ");
                                }

                                DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                DataTable dtLiveApptType = dtLocalApptType.Clone();
                                dtLiveApptType.Columns.Add("InsUptDlt", typeof(int));

                                foreach (var item in ApptTypeDto.data)
                                {
                                    DataRow RowType = dtLiveApptType.NewRow();
                                    //RowType["ApptType_LocalDB_ID"] = item.ApptType_localdb_id;
                                    RowType["ApptType_EHR_ID"] = item.Apptype_ehr_id;
                                    RowType["ApptType_Web_ID"] = item._id;
                                    RowType["Type_Name"] = item.name;
                                    RowType["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                    RowType["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    dtLiveApptType.Rows.Add(RowType);
                                    dtLiveApptType.AcceptChanges();
                                }
                                //Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "data table dtLiveApptType count  (" + dtLiveApptType.Rows.Count + ")");
                                foreach (DataRow dtDtxRow in dtLiveApptType.Rows)
                                {
                                    if (Utility.CheckEHR_ID(dtDtxRow["ApptType_EHR_ID"].ToString().Trim()) != string.Empty)
                                    {
                                        dtDtxRow["InsUptDlt"] = 6;
                                        continue;
                                    }

                                    DataRow[] row = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"].ToString().Trim() + "' And Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' ");
                                    if (row.Length > 0)
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 1;
                                    }
                                }

                                dtLiveApptType.AcceptChanges();

                                if (dtLiveApptType != null && dtLiveApptType.Rows.Count > 0)
                                {
                                    bool status = PullLiveDatabaseBAL.Save_ApptType_Live_To_Local(dtLiveApptType, Utility._filename_ApptType , Utility._EHRLogdirectory_ApptType );

                                    if (status)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "Save_ApptType_Live_To_Local status : success");

                                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType_Pull");

                                        if(UpdateSync_Table_Datetime)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "UpdateSync_Table_Datetime status : success");
                                        }
                                        else
                                        {

                                        }
                                        ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (Adit Server To Local Database) Service Install Id :  " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                    }
                                    else
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_ApptType, Utility._EHRLogdirectory_ApptType, "Save_ApptType_Live_To_Local status : falied");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Appointment Type Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        #endregion

        #region Patient

        private void fncSyncDataLiveDB_Pull_Patient()
        {
            InitBgWorkerLiveDB_Pull_Patient();
            InitBgTimerLiveDB_Pull_Patient();
        }

        private void InitBgTimerLiveDB_Pull_Patient()
        {

            timerSynchLiveDB_Pull_Patient = new System.Timers.Timer();
            this.timerSynchLiveDB_Pull_Patient.Interval = 1000 * GoalBase.intervalWebSynch_Pull_Patient;
            this.timerSynchLiveDB_Pull_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLiveDB_Pull_Patient_Tick);
            timerSynchLiveDB_Pull_Patient.Enabled = true;
            timerSynchLiveDB_Pull_Patient.Start();
        }

        private void InitBgWorkerLiveDB_Pull_Patient()
        {
            bwSynchLiveDB_Pull_Patient = new BackgroundWorker();
            bwSynchLiveDB_Pull_Patient.WorkerReportsProgress = true;
            bwSynchLiveDB_Pull_Patient.WorkerSupportsCancellation = true;
            bwSynchLiveDB_Pull_Patient.DoWork += new DoWorkEventHandler(bwSynchLiveDB_Pull_Patient_DoWork);
            bwSynchLiveDB_Pull_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLiveDB_Pull_Patient_RunWorkerCompleted);
        }

        private void timerSynchLiveDB_Pull_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLiveDB_Pull_Patient.Enabled = false;
                MethodForCallSynchOrderLiveDB_Pull_Patient();
            }
        }

        public void MethodForCallSynchOrderLiveDB_Pull_Patient()
        {
            System.Threading.Thread procThreadmainLiveDB_Pull_Patient = new System.Threading.Thread(this.CallSyncOrderTableLiveDB_Pull_Patient);
            procThreadmainLiveDB_Pull_Patient.Start();
        }

        public void CallSyncOrderTableLiveDB_Pull_Patient()
        {
            if (bwSynchLiveDB_Pull_Patient.IsBusy != true)
            {
                bwSynchLiveDB_Pull_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLiveDB_Pull_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLiveDB_Pull_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_Pull_Patient();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLiveDB_Pull_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLiveDB_Pull_Patient.Enabled = true;
        }

        public void SynchDataLiveDB_Pull_Patient()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            string strApiPatient = PullLiveDatabaseBAL.GetLiveRecord("Patient", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                            var client = new RestClient(strApiPatient);
                            Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Call Patient Confirmation API");
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                            //request.AddHeader("cache-control", "no-cache");
                            //request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Request Sent into the API " + " Authorization, TokenKey & action");
                            IRestResponse response = client.Execute(request);
                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                { }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Patient Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + response.ErrorMessage);
                                }
                                return;
                            }

                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Response received from API(" + response.Content.ToString() + ")");
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Response is null");
                            }
                            Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "----------------------------Deserialize repsonse--------------------------------");
                            var ApptPatientDto = JsonConvert.DeserializeObject<Pull_PatientBO>(response.Content);


                            if (ApptPatientDto != null && ApptPatientDto.data != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Deserialize repsonse from API(" + ApptPatientDto.message.ToString() + ")");
                                if (ApptPatientDto.data.Count == 0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "Deserialize response ApptPatientDto count :  (" + ApptPatientDto.data.Count + ") no record ");
                                }

                                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                DataTable dtLivePatient = dtLocalPatient.Clone();
                                dtLivePatient.Columns.Add("InsUptDlt", typeof(int));

                                string tmpLast_Visit = string.Empty;
                                string tmpNext_Visit = string.Empty;
                                string tmpBirth_Date = string.Empty;

                                foreach (var item in ApptPatientDto.data)
                                {
                                    tmpLast_Visit = Utility.ConvertDatetimeToCurrentLocationFormat(item.last_visit.ToString().Trim());
                                    tmpNext_Visit = Utility.ConvertDatetimeToCurrentLocationFormat(item.next_visit.ToString().Trim());
                                    tmpBirth_Date = Utility.ConvertDatetimeToCurrentLocationFormat(item.birth_date.ToString().Trim());

                                    DataRow RowPatient = dtLivePatient.NewRow();
                                    //RowPatient["Patient_LocalDB_ID"] = item.patient_localdb_id;
                                    RowPatient["Patient_EHR_ID"] = item.patient_ehr_id;
                                    RowPatient["Patient_Web_ID"] = item._id;
                                    RowPatient["fullname"] = item.fullname;
                                    RowPatient["email"] = item.email;
                                    RowPatient["mobile"] = item.mobile;
                                    RowPatient["phone"] = item.phone;
                                    RowPatient["birth_date"] = tmpBirth_Date;
                                    RowPatient["last_visit"] = tmpLast_Visit;
                                    RowPatient["next_visit"] = tmpNext_Visit;
                                    RowPatient["revenue"] = item.revenue;
                                    RowPatient["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                    RowPatient["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    dtLivePatient.Rows.Add(RowPatient);
                                    dtLivePatient.AcceptChanges();
                                }
                                Utility.WriteSyncPullLog(Utility._filename_appointment, Utility._EHRLogdirectory_appointment, "data table dtLivePatient count  (" + dtLivePatient.Rows.Count + ")");
                                foreach (DataRow dtDtxRow in dtLivePatient.Rows)
                                {
                                    if (Utility.CheckEHR_ID(dtDtxRow["Patient_EHR_ID"].ToString().Trim()) != string.Empty)
                                    {
                                        dtDtxRow["InsUptDlt"] = 6;
                                        continue;
                                    }

                                    DataRow[] row = dtLocalPatient.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "'");
                                    if (row.Length > 0)
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 1;
                                    }
                                }

                                dtLivePatient.AcceptChanges();

                                if (dtLivePatient != null && dtLivePatient.Rows.Count > 0)
                                {
                                    bool status = PullLiveDatabaseBAL.Save_Patient_Live_To_Local(dtLivePatient, Utility._filename_appointment, Utility._EHRLogdirectory_appointment);

                                    if (status)
                                    {
                                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Pull");
                                        ObjGoalBase.WriteToSyncLogFile("Patient Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  Successfully.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        #endregion

        #region Treatment Document

        public static void SynchDataLiveDB_Pull_treatmentDoc()
        {
            try
            {
                Utility.WriteToSyncLogFile_All("Call Treatment Doc PUll");
                if (!IS_EHRDocPulled)
                {
                    //Utility.WriteToSyncLogFile_All("EHR Pulled Condition Falsed");
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        //Utility.WriteToSyncLogFile_All("Start Location Loop");
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            //Utility.WriteToSyncLogFile_All("Adit App Sync is Enabled");
                            IS_EHRDocPulled = true;
                            string strApiTreatmentDocForm = PullLiveDatabaseBAL.GetLiveRecord("treatmentplan_document", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                            string ab = Utility.WebAdminUserToken;
                            var client = new RestClient(strApiTreatmentDocForm);

                            PatientDoc PatientpdfDoc = new PatientDoc
                            {
                                locationId = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                //locationId = "69a1280c-e189-4985-99de-efdf20d2e04f"
                            };
                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                            ///ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.POST);
                            Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Call treatmentplan document  API");
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            GoalBase.WriteToPaymentLogFromAll_Static("PatientPortal_Request Called " + strApiTreatmentDocForm.ToString());
                            Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Request Sent into the API " + " Authorization, TokenKey & action");
                            IRestResponse response = client.Execute(request);

                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync_ResponseError (Adit Server To Local Database)] : " + response.ErrorMessage);
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                IS_EHRDocPulled = false;
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Response received from API(" + response.Content.ToString() + ")");
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Response is null");
                            }


                            Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "----------------------------Deserialize repsonse--------------------------------");

                            GoalBase.WriteToPaymentLogFromAll_Static("Response Received " + response.Content.ToString());
                            var AppTreatmentDocDto = JsonConvert.DeserializeObject<Pull_TreatmentDocBO>(response.Content);


                            if (AppTreatmentDocDto != null && AppTreatmentDocDto.data != null)
                            {
                                if (AppTreatmentDocDto.data != null && AppTreatmentDocDto.data.Count() > 0)
                                {
                                    if (AppTreatmentDocDto.data.Count == 0)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Deserialize response AppTreatmentDocDto count :  (" + AppTreatmentDocDto.data.Count + ") no record ");
                                    }
                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Deserialize repsonse from API(" + AppTreatmentDocDto.message.ToString() + ")");

                                    int count = 0;
                                    foreach (var item in AppTreatmentDocDto.data)
                                    {
                                        //check for sync is done or not
                                        bool DocSynced = SynchLocalBAL.Sync_check_for_treatmentDoct(item.treatmentPlanId, Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document);

                                        if (DocSynced)
                                        {
                                            if (item.patient_ehr_id != null && item.patient_ehr_id != "")
                                            {
                                                count++;
                                                string Patient_Web_ID = item.patientId;
                                                string TreatmentPlanId = item.treatmentPlanId;
                                                string TreatmentPlanName = item.planName;
                                                string Clinic_Number = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(); ;
                                                string Service_Install_Id = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                string Patient_EHR_ID = item.patient_ehr_id;
                                                string PatientName = item.patientName;
                                                string SubmittedDate = Convert.ToString(DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.treatment_plan_submitted_at)));

                                                SynchLocalBAL.CreateRecordForTreatmentDoc(PatientName, SubmittedDate, Patient_EHR_ID, Patient_Web_ID, TreatmentPlanId, TreatmentPlanName, Clinic_Number, Service_Install_Id, Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    GoalBase.WriteToSyncLogFile_Static("TreatmentDoc Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"] + " Pending Records Not found on Adit Server");
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                                    if (UpdateSync_Table_Datetime)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "UpdateSync_Table_Datetime  status : Success");
                                    }
                                    else
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "UpdateSync_Table_Datetime  status : failed");
                                    }
                                }

                            }
                            else
                            {
                                Utility.WriteToSyncLogFile_All("TreatmentDoc_AditLocationSyncEnable False : " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                            }
                        }
                        IS_EHRDocPulled = false;
                    }
                }
                else
                {
                    Utility.WriteToSyncLogFile_All("TreatmentDoc_FirstCondition false : ProviderSyncFirstTime = " + IsProviderSyncedFirstTime.ToString() + ", EHR Doc pull : " + IS_EHRDocPulled.ToString());
                }
            }
            catch (Exception ex)
            {
                IS_EHRDocPulled = false;
                GoalBase.WriteToErrorLogFile_Static("[TreatmentDoc Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        public static void Change_Status_TreatmentDoc(DataTable Daata, string status)
        {
            try
            {
                Utility.WriteToSyncLogFile_All("Call Treatment Doc PUll");
                if (!IS_EHRDocPulled)
                {
                    Utility.WriteToSyncLogFile_All("EHR Pulled Condition Falsed");
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        Utility.WriteToSyncLogFile_All("Start Location Loop");
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            Utility.WriteToSyncLogFile_All("Adit App Sync is Enabled");

                            List<string> docids = new List<string>();
                            foreach (DataRow dr in Daata.Rows)
                            {
                                docids.Add(dr["TreatmentPlanId"].ToString());
                            }

                            string strApiUpdateTreatmentDocStatus = PullLiveDatabaseBAL.UpdateTreatmentDocStatus("change_treatmentplan_doc_status");
                            Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Call change_treatmentplan_doc_status API");
                            string ab = Utility.WebAdminUserToken;
                            var client = new RestClient(strApiUpdateTreatmentDocStatus);

                            TreatmentDocStatusChange treatmentDocStatusChange = new TreatmentDocStatusChange
                            {
                                id = docids,
                                status = status.ToLower()
                            };
                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            string jsonString = javaScriptSerializer.Serialize(treatmentDocStatusChange);
                            var request = new RestRequest(Method.POST);

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            request.AddHeader("action", "EHRPFIMPORT");
                            Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document , "Request Sent into the API  " + " Authorization, TokenKey & action");
                            GoalBase.WriteToPaymentLogFromAll_Static("PatientPortal_Request Called " + treatmentDocStatusChange.ToString());
                            IRestResponse response = client.Execute(request);

                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Response received from API (" + response.Content.ToString() + ")");

                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Response is null");
                            }

                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync_ResponseError (Adit Server To Local Database)] : " + response.ErrorMessage);
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                IS_EHRDocPulled = false;
                                return;
                            }

                            GoalBase.WriteToPaymentLogFromAll_Static("Response Received " + response.Content.ToString());
                            var AppTreatmentDocDto = JsonConvert.DeserializeObject<Pull_statusTreatmentDocBO>(response.Content);



                            if (AppTreatmentDocDto != null && AppTreatmentDocDto.data != null)
                            {

                                if (AppTreatmentDocDto.data.Count() == 0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Deserialize response AppTreatmentDocDto count :  (" + AppTreatmentDocDto.data.Count() + ") no record ");
                                }
                                if (AppTreatmentDocDto.status == true)
                                {
                                    if (status == "Completed")
                                    {
                                        UpdateTreatmentDocumentAditUpdated(Daata, Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Utility.WriteToSyncLogFile_All("TreatmentDoc_AditLocationSyncEnable False : " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                        }
                    }
                }
                else
                {
                    Utility.WriteToSyncLogFile_All("TreatmentDoc_FirstCondition false : ProviderSyncFirstTime = " + IsProviderSyncedFirstTime.ToString() + ", EHR Doc pull : " + IS_EHRDocPulled.ToString());
                }
            }
            catch (Exception ex)
            {
                IS_EHRDocPulled = false;
                GoalBase.WriteToErrorLogFile_Static("[TreatmentDoc Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        private static void UpdateTreatmentDocumentAditUpdated(DataTable dt, string _filename_EHR_treatmentplan_document = "", string _EHRLogdirectory_EHR_treatmentplan_document = "")
        {
            foreach (DataRow dr in dt.Rows)
            {
                SynchLocalBAL.UpdateTreatmentDocInlocal(dr["TreatmentPlanId"].ToString(), _filename_EHR_treatmentplan_document, _EHRLogdirectory_EHR_treatmentplan_document);
            }
        }


        private void SyncTreatmentDocument()
        {
            #region SavePatientDoc
            if (IsDocUpdated)
            {
                try
                {
                    IsDocUpdated = false;

                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        DataTable dtLocalPenddingTreatmentDocs = SynchLocalBAL.GetLocalPendingTreatmentDocData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                        foreach (DataRow dr in dtLocalPenddingTreatmentDocs.Rows)
                        {
                            #region TreatmentDocPullLive
                            try
                            {
                                string FileName = dr["SubmittedDate"].ToString().Trim() + "-" + dr["TreatmentPlanName"].ToString().Trim() + "-" + dr["PatientName"].ToString().Trim() + ".pdf";
                                FileName = FileName.Replace(":", "");
                                FileName = FileName.Replace("/", "-");

                                string filepath = CommonUtility.GetAditTreatmentDocTempPath() + "\\" + FileName;

                                if (!File.Exists(filepath))
                                {
                                    string strApiPatientForm = PullLiveDatabaseBAL.GetTreatmentDocFromWeb("treatmentplan_pdf", dr["TreatmentPlanId"].ToString());

                                    TreatmentDoc treatmentDoc = new TreatmentDoc
                                    {
                                        returnType = "base64"
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    string jsonString = javaScriptSerializer.Serialize(treatmentDoc);
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Call ApiPatientForm (treatmentplan_pdf) document  API");
                                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                    var clientdoc = new RestClient(strApiPatientForm);
                                    var requestdoc = new RestRequest(Method.POST);
                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                    requestdoc.AddHeader("Authorization", Utility.WebAdminUserToken);
                                    requestdoc.AddHeader("cache-control", "no-cache");
                                    requestdoc.AddHeader("Content-Type", "application/json");
                                    requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                    requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Request Sent into the API" + " Authorization, TokenKey & action");
                                    Utility.WriteToSyncLogFile_All("PatientTreatmentPlanDocument_Request (Patient_EHR_ID = " + dr["TreatmentDoc_Web_ID"] + ") Called " + strApiPatientForm.ToString());
                                    IRestResponse response = clientdoc.Execute(requestdoc);
                                    if (response.Content != null)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Response received from API (" + response.Content.ToString() + " )");

                                    }
                                    else
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Response is null");
                                    }
                                    if (response.ErrorMessage != null)
                                    {
                                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientForm_Sync_Error : " + response.ErrorMessage);
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientForm Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + response.ErrorMessage);
                                        }
                                        IsDocUpdated = true;
                                        return;
                                    }
                                    Utility.WriteToSyncLogFile_All("PatientDocument_Request Response Received.");// + response.Content.ToString());
                                    var jObject = JObject.Parse(response.Content);
                                    if (jObject != null)
                                    {
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "Response received in json (" + jObject.ToString() + " )");

                                        string DocData = jObject.GetValue("data").ToString();
                                        DocData = DocData.Replace("data:application/pdf;base64,", String.Empty);

                                        // Document. 
                                        bool Docstatus = WriteByteArrayToPdf(DocData, CommonUtility.GetAditTreatmentDocTempPath(),FileName);
                                        if (Docstatus)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document, "WriteByteArrayToPdf status :success");
                                            SynchLocalBAL.UPDATERecordForTreatmentDoc(dr["TreatmentPlanId"].ToString().Trim(), FileName, Utility._filename_EHR_treatmentplan_document, Utility._EHRLogdirectory_EHR_treatmentplan_document);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[PatientFormDocument Sync (Adit Server To Local Database)] for Id " + dr["PatientForm_Web_ID"].ToString() + " Error " + ex1.Message);
                            }
                            #endregion

                        }


                    }
                    IsDocUpdated = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientFormDocument Sync (Adit Server To Local Database)] : " + ex.Message);
                    IsDocUpdated = true;
                }
            }

            #endregion
        }



        #endregion

        #region PatientForm

        static bool IS_EHRDocPulled = false;


        public static void InsertRowINtodatatable(ref DataTable dtPatientForm, string ehrFields, string ehrFieldsValue, string patientForm_webId, string patientEHrId, string patientWebId, string Clinic_Number, string Service_Install_Id, string folder_ehr_id, string folder_name, string DocNameFormat, string Form_Name, string Patient_Name, string submit_time, int PatientType = 0)
        {
            try
            {
                DataRow drNew = dtPatientForm.NewRow();
                drNew["ehrfield"] = ehrFields;
                drNew["ehrfield_value"] = ehrFieldsValue.Replace("'", "''");
                drNew["PatientForm_Web_ID"] = patientForm_webId;
                drNew["Patient_EHR_ID"] = patientEHrId;
                drNew["Patient_Web_ID"] = patientWebId;
                drNew["Clinic_Number"] = Clinic_Number;
                drNew["Service_Install_Id"] = Service_Install_Id;
                drNew["PatientType"] = PatientType;

                drNew["folder_name"] = folder_name;
                drNew["DocNameFormat"] = DocNameFormat;
                drNew["Form_Name"] = Form_Name;
                drNew["Patient_Name"] = Patient_Name;
                drNew["submit_time"] = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(submit_time.ToString()));

                if (folder_ehr_id == "")
                {
                    drNew["folder_ehr_id"] = 0;
                }
                else
                {
                    drNew["folder_ehr_id"] = Convert.ToInt32(folder_ehr_id);
                }

                dtPatientForm.Rows.Add(drNew);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void AddinfoinForm(ref DataRow DR, string folder_ehr_id, string folder_name, string DocNameFormat, string Form_Name, string Patient_Name, string submit_time)
        {
            if (folder_ehr_id == "")
            {
                DR["folder_ehr_id"] = 0;
            }
            else
            {
                DR["folder_ehr_id"] = Convert.ToInt32(folder_ehr_id);
            }

            DR["folder_name"] = folder_name;
            DR["DocNameFormat"] = DocNameFormat;
            DR["Form_Name"] = Form_Name;
            DR["Patient_Name"] = Patient_Name;
            DR["submit_time"] = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(submit_time.ToString()));

        }

        public static void SynchDataLiveDB_Pull_PatientForm()
        {
            try
            {
                string firstname = "", lastname = "";
                DataTable dtEHRPatientData = new DataTable();
                //Utility.WriteToSyncLogFile_All("Call Patient form PUll");
                if (IsProviderSyncedFirstTime && !IS_EHRDocPulled)
                {
                    //Utility.WriteToSyncLogFile_All("EHR Pulled Condition Falsed");
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        //Utility.WriteToSyncLogFile_All("Start Location Loop");
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            //Utility.WriteToSyncLogFile_All("Adit App Sync is Enabled");
                            IS_EHRDocPulled = true;
                            string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("PatientForm", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Call PatientForm  API");
                            string ab = Utility.WebAdminUserToken;
                            var client = new RestClient(strApiPatientForm);
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            // Utility.WriteToSyncLogFile_All("API NAME " + strApiPatientForm.ToString());
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            request.AddHeader("action", "EHRPFIMPORT");
                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Request Sent into the API : " + " Authorization,TokenKey & action");
                            GoalBase.WriteToPaymentLogFromAll_Static("PatientForm_Request Called ");
                            IRestResponse response = client.Execute(request);

                            if (response.ErrorMessage != null)
                            {
                                //GoalBase.WriteToPaymentLogFromAll_Static("PatientForm Response " + response.ErrorMessage.ToString());
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientForm Sync_ResponseError (Adit Server To Local Database)] : " + response.ErrorMessage);
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientForm Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                IS_EHRDocPulled = false;
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Response received from API(" + response.Content.ToString() + ")");
                                GoalBase.WriteToPaymentLogFromAll_Static("PatientForm Response " + response.Content.ToString());
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Response is null");
                            }

                            //  ObjGoalBase.WriteToSyncLogFile("API NAME " + response.Content.ToString());
                            //Utility.WriteToSyncLogFile_All("Response Received " + response.Content.ToString());

                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "----------------------------Deserialize repsonse--------------------------------");

                            var ApptPatientFormDto = JsonConvert.DeserializeObject<Pull_PatientFormBO>(response.Content);


                            if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                            {
                                if (ApptPatientFormDto.data != null && ApptPatientFormDto.data.Count() > 0)
                                {
                                    if (ApptPatientFormDto.data.Count == 0)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Deserialize response ApptPatientFormDto count :  (" + ApptPatientFormDto.data.Count + ") no record ");
                                    }


                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Deserialize repsonse from API(" + ApptPatientFormDto.message.ToString() + ")");
                                    //Utility.WriteToSyncLogFile_All("Response Received count " + ApptPatientFormDto.data.Count().ToString());

                                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPatientFormData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    dtEHRPatientData.Rows.Clear();
                                    DataTable dtEagleSoftInsuranceName = new DataTable();
                                    // ObjGoalBase.WriteToSyncLogFile("DONE");
                                    //EagleSoft
                                    if (Utility.Application_ID == 1)
                                    {
                                        //dtEHRPatientData = SynchEaglesoftBAL.GetEaglesoftPatientData(Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
                                        dtEHRPatientData = SynchEaglesoftBAL.GetEaglesoftPatientData(Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()));
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetEaglesoftPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");
                                        dtEagleSoftInsuranceName = SynchEaglesoftBAL.GetInsuratnce_CompanyName(Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()));
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetInsuratnce_CompanyName data table dtEagleSoftInsuranceName count  (" + dtEagleSoftInsuranceName.Rows.Count + ")");
                                    }
                                    // OpenDental
                                    else if (Utility.Application_ID == 2)
                                    {
                                        dtEHRPatientData = SynchOpenDentalBAL.GetOpenDentalPatientData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), (Utility.OpenDentalOldPatSync ? false : true));
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetOpenDentalPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");
                                    }
                                    // Dentrix
                                    else if (Utility.Application_ID == 3)
                                    {
                                        dtEHRPatientData = SynchDentrixBAL.GetDentrixPatientData();
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetDentrixPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");
                                    }
                                    //Softdent
                                    else if (Utility.Application_ID == 4)
                                    {
                                        dtEHRPatientData = SynchLocalBAL.GetLocalPatientData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetLocalPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");
                                    }
                                    //ClearDent
                                    else if (Utility.Application_ID == 5)
                                    {
                                        dtEHRPatientData = SynchClearDentBAL.GetClearDentPatientData();
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetClearDentPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");
                                    }
                                    //Tracker
                                    else if (Utility.Application_ID == 6)
                                    {
                                        dtEHRPatientData = SynchTrackerBAL.GetTrackerPatientData();
                                        //.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetTrackerPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");
                                    }
                                    // PracticeWork
                                    else if (Utility.Application_ID == 7)
                                    {
                                        dtEHRPatientData = SynchPracticeWorkBAL.GetPracticeWorkPatientData();
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetPracticeWorkPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");
                                    }
                                    //EasyDental
                                    else if (Utility.Application_ID == 8)
                                    {
                                        dtEHRPatientData = SynchEasyDentalBAL.GetEasyDentalPatientData();
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetEasyDentalPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");

                                    }
                                    else if (Utility.Application_ID == 10)
                                    {
                                        dtEHRPatientData = SynchPracticeWebBAL.GetPracticeWebPatientData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()));
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetPracticeWebPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");

                                    }
                                    //AbelDent
                                    else if (Utility.Application_ID == 11)
                                    {
                                        dtEHRPatientData = SynchAbelDentBAL.GetAbelDentPatientData();
                                        // Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetAbelDentPatientData data table dtEHRPatientData count  (" + dtEHRPatientData.Rows.Count + ")");


                                    }
                                    if (!dtEHRPatientData.Columns.Contains("Patient_Web_ID"))
                                    {
                                        dtEHRPatientData.Columns.Add("Patient_Web_ID", typeof(string));
                                    }

                                    DataTable dtLocalProviderData = SynchLocalBAL.GetLocalProviderData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    DataTable dtLivePatientForm = dtLocalPatientForm.Clone();
                                    dtLivePatientForm.Columns.Add("InsUptDlt", typeof(int));

                                    DataTable dtPatientFormDiseaseResponse = SynchLocalBAL.GetLocalPatientFormDiseaseResponse(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    DataTable dtLivePatientFormDiseaseResponse = dtPatientFormDiseaseResponse.Clone();
                                    dtLivePatientFormDiseaseResponse.Columns.Add("InsUptDlt", typeof(int));
                                    // Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetLocalPatientFormDiseaseResponse data table dtLivePatientFormDiseaseResponse count  (" + dtLivePatientFormDiseaseResponse.Rows.Count + ")");

                                    DataTable dtPatientFormDiseaseDeleteResponse = SynchLocalBAL.GetLocalPatientFormDiseaseDeleteResponse(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    DataTable dtLivePatientFormDiseaseDeleteResponse = dtPatientFormDiseaseDeleteResponse.Clone();
                                    dtLivePatientFormDiseaseDeleteResponse.Columns.Add("InsUptDlt", typeof(int));
                                    //   Utility.WriteToSyncLogFile_All("Data Received " + ApptPatientFormDto.data.ToString());

                                    DataTable dtPatientFormMedicationResponse = SynchLocalBAL.GetLocalPatientFormMedicationResponse(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    DataTable dtLivePatientFormMedicationResponse = dtPatientFormMedicationResponse.Clone();
                                    dtLivePatientFormMedicationResponse.Columns.Add("InsUptDlt", typeof(int));
                                    // Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "GetLocalPatientFormMedicationResponse data table dtLivePatientFormMedicationResponse count  (" + dtLivePatientFormMedicationResponse.Rows.Count + ")");

                                    DataTable dtPatientFormMedicationRemovedResponse = SynchLocalBAL.GetLocalPatientFormMedicationRemovedResponse(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    DataTable dtLivePatientFormMedicationRemovedResponse = dtPatientFormMedicationRemovedResponse.Clone();
                                    dtLivePatientFormMedicationRemovedResponse.Columns.Add("InsUptDlt", typeof(int));

                                    int folder_ehr_id = 0;

                                    string folder_name = "";
                                    string DocNameFormat = "";
                                    string Form_Name = "";
                                    string Patient_Name = "";
                                    DateTime submit_time = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(DateTime.Now.ToString()));
                                    foreach (var item in ApptPatientFormDto.data)
                                    {
                                        try
                                        {
                                            folder_ehr_id = item.folder_ehr_id == "" ? 0 : Convert.ToInt32(item.folder_ehr_id);
                                            folder_name = item.folder_name;
                                            DocNameFormat = item.form_name_format;
                                            Form_Name = item.form_name;
                                            Patient_Name = item.patient_name;
                                            submit_time = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.submit_time.ToString()));
                                        }
                                        catch (Exception dp)
                                        {
                                            folder_ehr_id = 0;
                                            folder_name = "";
                                            DocNameFormat = "";
                                            Form_Name = "";
                                            Patient_Name = "";
                                            submit_time = DateTime.Now;
                                            Utility.WriteToErrorLogFromAll("Error Getting in PatientForm DOCAttachment info done by dipika " + dp.Message.ToString());
                                        }
                                        if (item.ehrmap != null && item.ehrmap.Count() > 0)
                                        {
                                            foreach (var subitem in item.ehrmap)
                                            {
                                                lastname = ""; firstname = "";
                                                if (subitem.ehrField.ToString().Trim().ToUpper() == "FIRST_NAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "LAST_NAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "MOBILE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "ADDRESS_ONE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "ADDRESS_TWO" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "BIRTH_DATE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "CITY" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "EMAIL" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "HOME_PHONE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "MARITAL_STATUS" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "MIDDLE_NAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "PREFERRED_NAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "PRI_PROVIDER_ID" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "RECEIVE_EMAIL" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "RECEIVE_SMS" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SALUTATION" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SEC_PROVIDER_ID" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SEX" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "WORK_PHONE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "STATE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "ZIPCODE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SSN" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SCHOOL" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "EMPLOYER" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "DRIVERLICENSE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER")
                                                {
                                                    if (subitem.value.ToString() != "")
                                                    {
                                                        DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                                        if (item.pinfo != null && item.pinfo.patient_ehr_id !=null && item.pinfo.patient_ehr_id.ToString() != "-")
                                                        {
                                                            RowPatientForm["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                            RowPatientForm["Patient_Web_ID"] = item.pinfo._id;
                                                        }
                                                        RowPatientForm["PatientForm_Web_ID"] = item._id;
                                                        try
                                                        {
                                                            AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);
                                                        }
                                                        catch (Exception)
                                                        {

                                                        }

                                                        //RowPatientForm["submit_time"] = Convert.ToDateTime(item.submit_time).ToString("MM/dd/yyyy");
                                                        RowPatientForm["ehrfield"] = subitem.ehrField;
                                                        if ((RowPatientForm["ehrfield"].ToString().ToLower() == "primary_insurance_companyname" || RowPatientForm["ehrfield"].ToString().ToLower() == "secondary_insurance_companyname") && Utility.Application_ID == 1 && Utility.Application_ID == 8)
                                                        {
                                                            RowPatientForm["ehrfield_value"] = subitem.value.ToString();
                                                        }
                                                        else
                                                        {
                                                            RowPatientForm["ehrfield_value"] = subitem.value.ToString().Replace("'", "");
                                                        }

                                                        try
                                                        {
                                                            if (RowPatientForm["ehrfield"].ToString().ToLower() == "birth_date")
                                                            {
                                                                RowPatientForm["ehrfield_value"] = Convert.ToDateTime(Utility.CheckValidDatetime(RowPatientForm["ehrfield_value"].ToString())).ToString("yyyy/MM/dd");
                                                            }
                                                        }
                                                        catch (Exception)
                                                        {

                                                        }
                                                        RowPatientForm["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        RowPatientForm["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);

                                                        dtLivePatientForm.Rows.Add(RowPatientForm);
                                                        dtLivePatientForm.AcceptChanges();
                                                    }
                                                }
                                                //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "data table dtLivePatientForm count  (" + dtLivePatientForm.Rows.Count + ")");
                                                // Utility.WriteToSyncLogFile_All("Set Allergies");
                                                if (subitem.Alg_Prb_value != null)
                                                {
                                                    foreach (var Allergy in subitem.Alg_Prb_value)
                                                    {
                                                        DataRow drRepDiese = dtLivePatientFormDiseaseResponse.NewRow();
                                                        drRepDiese["DiseaseMaster_Web_ID"] = Allergy._id.ToString();
                                                        drRepDiese["PatientForm_Web_ID"] = item._id.ToString();
                                                        if (item.pinfo != null)
                                                        {
                                                            drRepDiese["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                        }
                                                        else
                                                        {
                                                            drRepDiese["Patient_EHR_ID"] = "";
                                                        }
                                                        drRepDiese["Disease_EHR_Id"] = Allergy.disease_ehr_id.ToString();
                                                        drRepDiese["Disease_Type"] = Allergy.disease_type.ToString();
                                                        drRepDiese["Name"] = Allergy.name.ToString();
                                                        drRepDiese["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        drRepDiese["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        dtLivePatientFormDiseaseResponse.Rows.Add(drRepDiese);
                                                    }
                                                    // Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "data table dtLivePatientFormDiseaseResponse count  (" + dtLivePatientFormDiseaseResponse.Rows.Count + ")");
                                                }
                                                if (subitem.Removed_Alg_Prb_value != null)
                                                {
                                                    foreach (var Allergy in subitem.Removed_Alg_Prb_value)
                                                    {
                                                        DataRow drRepDiese = dtLivePatientFormDiseaseDeleteResponse.NewRow();
                                                        //drRepDiese["DiseaseDeleteResponse_Web_ID"] = Allergy._id.ToString();
                                                        drRepDiese["PatientForm_Web_ID"] = item._id.ToString();
                                                        drRepDiese["Patient_EHR_ID"] = Allergy.patient_ehr_id.ToString();
                                                        drRepDiese["Disease_EHR_Id"] = Allergy.disease_ehr_id.ToString();
                                                        drRepDiese["Disease_Type"] = Allergy.disease_type.ToString();
                                                        drRepDiese["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        drRepDiese["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        dtLivePatientFormDiseaseDeleteResponse.Rows.Add(drRepDiese);
                                                    }
                                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "data table dtLivePatientFormDiseaseDeleteResponse count  (" + dtLivePatientFormDiseaseDeleteResponse.Rows.Count + ")");

                                                }
                                                // Utility.WriteToSyncLogFile_All("Set Allergies Completed");

                                                if (subitem.Removed_Medication_value != null)
                                                {
                                                    foreach (var RemovedMedValue in subitem.Removed_Medication_value)
                                                    {
                                                        DataRow drMedicationRemoved = dtLivePatientFormMedicationRemovedResponse.NewRow();
                                                        drMedicationRemoved["PatientForm_Web_ID"] = item._id.ToString();
                                                        drMedicationRemoved["Patient_EHR_ID"] = item.pinfo == null ? "" : item.pinfo.patient_ehr_id.ToString().Trim();
                                                        string MedicationID = "0";
                                                        if (RemovedMedValue.medication_ehr_id != null)
                                                        {
                                                            MedicationID = RemovedMedValue.medication_ehr_id.ToString();
                                                        }
                                                        if (MedicationID.Trim() != "" && MedicationID.Trim() != "0")
                                                        {
                                                            drMedicationRemoved["Medication_EHR_Id"] = MedicationID.Trim();
                                                        }
                                                        else
                                                        {
                                                            if (Utility.Application_ID != 6)
                                                                continue;
                                                            else
                                                                drMedicationRemoved["Medication_EHR_Id"] = "0";
                                                        }
                                                        drMedicationRemoved["Medication_Name"] = RemovedMedValue.medication_name.ToString();
                                                        drMedicationRemoved["Medication_Note"] = RemovedMedValue.patientnote.ToString();
                                                        drMedicationRemoved["PatientMedication_EHR_ID"] = RemovedMedValue.patientmedication_ehr_id.ToString();
                                                        drMedicationRemoved["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        drMedicationRemoved["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        dtLivePatientFormMedicationRemovedResponse.Rows.Add(drMedicationRemoved);
                                                    }
                                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "data table dtLivePatientFormMedicationRemovedResponse count  (" + dtLivePatientFormMedicationRemovedResponse.Rows.Count + ")");

                                                }

                                                if (subitem.Medication_value != null)
                                                {
                                                    foreach (var medicationValue in subitem.Medication_value)
                                                    {
                                                        DataRow drMedication = dtLivePatientFormMedicationResponse.NewRow();
                                                        drMedication["MedicationMaster_Web_ID"] = ""; //medicationValue._id.ToString();
                                                        drMedication["PatientForm_Web_ID"] = item._id.ToString();
                                                        drMedication["Patient_EHR_ID"] = item.pinfo == null ? "" : item.pinfo.patient_ehr_id.ToString().Trim();
                                                        string MedicationID = "0";
                                                        if (medicationValue.medication_ehr_id != null)
                                                        {
                                                            MedicationID = medicationValue.medication_ehr_id.ToString();
                                                        }
                                                        if (MedicationID.Trim() != "" && MedicationID.Trim() != "0")
                                                        {
                                                            drMedication["Medication_EHR_Id"] = MedicationID.Trim();
                                                        }
                                                        else
                                                        {
                                                            if (Utility.Application_ID != 6)
                                                                continue;
                                                            else
                                                                drMedication["Medication_EHR_Id"] = "0";
                                                        }

                                                        string PatientMedicationID = "0";
                                                        if (medicationValue.patientmedication_ehr_id != null)
                                                        {
                                                            PatientMedicationID = medicationValue.patientmedication_ehr_id.ToString();
                                                        }
                                                        if (PatientMedicationID.Trim() != "" && PatientMedicationID.Trim() != "0")
                                                        {
                                                            drMedication["PatientMedication_EHR_Id"] = PatientMedicationID.Trim();
                                                        }

                                                        drMedication["Medication_Type"] = medicationValue.medication_type.ToString();
                                                        drMedication["Medication_Name"] = medicationValue.medication_name.ToString();
                                                        drMedication["Medication_Note"] = medicationValue.medication_note.ToString();
                                                        drMedication["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        drMedication["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        dtLivePatientFormMedicationResponse.Rows.Add(drMedication);
                                                    }
                                                    // Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "data table dtLivePatientFormMedicationResponse count  (" + dtLivePatientFormMedicationResponse.Rows.Count + ")");
                                                }

                                            }
                                            if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && item.pinfo != null)
                                                || (((item.ehrmap.Where(s => s.Alg_Prb_value != null).Count() > 0) || (item.ehrmap.Where(s => s.Removed_Alg_Prb_value != null).Count() > 0)
                                                || (item.ehrmap.Where(s => s.Medication_value != null).Count() > 0) || (item.ehrmap.Where(s => s.Removed_Medication_value != null).Count() > 0)) && item.pinfo != null))
                                            {
                                                var result = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item._id.ToUpper()
                                                && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "FIRST_NAME");

                                                if (result == null || (result != null && result.Count() == 0))
                                                {
                                                    InsertRowINtodatatable(ref dtLivePatientForm, "first_name", item.pinfo.first_name, item._id, item.pinfo.patient_ehr_id, item.pinfo._id, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);
                                                }
                                                var result1 = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item._id.ToUpper()
                                                   && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "LAST_NAME");

                                                if (result1 == null || (result1 != null && result1.Count() == 0))
                                                {
                                                    if (result1 == null)
                                                    {
                                                        lastname = "NA";
                                                    }
                                                    else if (result1 != null && result1.Count() == 0 && item.pinfo.last_name.ToString() == "")
                                                    {
                                                        lastname = "NA";
                                                    }
                                                    else
                                                    {
                                                        lastname = item.pinfo.last_name;
                                                    }
                                                    InsertRowINtodatatable(ref dtLivePatientForm, "last_name", lastname, item._id, item.pinfo.patient_ehr_id, item.pinfo._id, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);
                                                }
                                                var result2 = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item._id.ToUpper()
                                                   && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "MOBILE");

                                                if (result2 == null || (result2 != null && result2.Count() == 0))
                                                {
                                                    InsertRowINtodatatable(ref dtLivePatientForm, "mobile", item.pinfo.mobile == null ? "0000000000" : item.pinfo.mobile, item._id, item.pinfo.patient_ehr_id, item.pinfo._id, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.pinfo != null)
                                            {
                                                DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                                if (item.pinfo != null)
                                                {
                                                    RowPatientForm["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                    RowPatientForm["Patient_Web_ID"] = item.pinfo._id;
                                                    RowPatientForm["ehrfield_value"] = item.pinfo.first_name.Replace("'", "''");
                                                    RowPatientForm["ehrfield"] = "first_name";
                                                    RowPatientForm["PatientForm_Web_ID"] = item._id;
                                                    RowPatientForm["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPatientForm["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                }

                                                //rooja 2-5-23
                                                try
                                                {
                                                    AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);

                                                }
                                                catch (Exception)
                                                {

                                                }

                                                dtLivePatientForm.Rows.Add(RowPatientForm);
                                                dtLivePatientForm.AcceptChanges();

                                                DataRow RowPatientForm1 = dtLivePatientForm.NewRow();
                                                if (item.pinfo != null)
                                                {
                                                    RowPatientForm1["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                    RowPatientForm1["Patient_Web_ID"] = item.pinfo._id;
                                                    RowPatientForm1["ehrfield_value"] = item.pinfo.mobile == null ? "0000000000" : item.pinfo.mobile;
                                                    RowPatientForm1["ehrfield"] = "mobile";
                                                    RowPatientForm1["PatientForm_Web_ID"] = item._id;
                                                    RowPatientForm1["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPatientForm1["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                }


                                                //rooja 2-5-23
                                                try
                                                {
                                                    AddinfoinForm(ref RowPatientForm1, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);

                                                }
                                                catch (Exception)
                                                {

                                                }
                                                dtLivePatientForm.Rows.Add(RowPatientForm1);
                                                dtLivePatientForm.AcceptChanges();

                                                DataRow RowPatientForm2 = dtLivePatientForm.NewRow();
                                                if (item.pinfo != null)
                                                {
                                                    if (item.pinfo.last_name.ToString() == "")
                                                    {
                                                        lastname = "NA";
                                                    }
                                                    else
                                                    {
                                                        lastname = item.pinfo.last_name.Replace("'", "''");
                                                    }
                                                    RowPatientForm2["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                    RowPatientForm2["Patient_Web_ID"] = item.pinfo._id;
                                                    RowPatientForm2["ehrfield_value"] = lastname;
                                                    RowPatientForm2["ehrfield"] = "last_name";
                                                    RowPatientForm2["PatientForm_Web_ID"] = item._id;
                                                    RowPatientForm2["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPatientForm2["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                }
                                                //rooja 2-5-23
                                                try
                                                {
                                                    AddinfoinForm(ref RowPatientForm2, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);

                                                }
                                                catch (Exception)
                                                {

                                                }
                                                dtLivePatientForm.Rows.Add(RowPatientForm2);
                                                dtLivePatientForm.AcceptChanges();
                                            }
                                        }
                                        if (item.ehr_value != null)
                                        {
                                            firstname = "";
                                            lastname = "";
                                            if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty && item.ehr_value.patientName.Contains(" "))
                                            {
                                                firstname = item.ehr_value.patientName.Substring(0, item.ehr_value.patientName.IndexOf(" ")).Trim();
                                                lastname = item.ehr_value.patientName.Substring(item.ehr_value.patientName.IndexOf(" "), (item.ehr_value.patientName.Length - item.ehr_value.patientName.IndexOf(" "))).Trim();
                                            }
                                            else if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty && !item.ehr_value.patientName.Contains(" "))
                                            {
                                                firstname = item.ehr_value.patientName.ToString();
                                                lastname = "NA";
                                            }

                                            if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty)
                                            {
                                                if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item._id.ToString() + "' AND ehrfield = 'first_name'").Count() == 0))
                                                {
                                                    if (firstname != string.Empty)
                                                    {
                                                        DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                                        RowPatientForm["ehrfield_value"] = firstname.Replace("'", "''");
                                                        RowPatientForm["ehrfield"] = "first_name";
                                                        RowPatientForm["PatientForm_Web_ID"] = item._id;
                                                        RowPatientForm["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        RowPatientForm["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();

                                                        AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);

                                                        dtLivePatientForm.Rows.Add(RowPatientForm);
                                                    }
                                                }
                                            }


                                            if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty)
                                            {
                                                if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item._id.ToString() + "' AND ehrfield = 'last_name'").Count() == 0))
                                                {
                                                    if (lastname != string.Empty)
                                                    {
                                                        DataRow RowPatientForm1 = dtLivePatientForm.NewRow();
                                                        RowPatientForm1["ehrfield_value"] = lastname.Replace("'", "''"); ;
                                                        RowPatientForm1["ehrfield"] = "last_name";
                                                        RowPatientForm1["PatientForm_Web_ID"] = item._id;
                                                        RowPatientForm1["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        RowPatientForm1["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        AddinfoinForm(ref RowPatientForm1, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);

                                                        dtLivePatientForm.Rows.Add(RowPatientForm1);
                                                    }
                                                }
                                            }


                                            if (item.ehr_value.phone != null && item.ehr_value.phone != string.Empty)
                                            {
                                                if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item._id.ToString() + "' AND ehrfield = 'mobile'").Count() == 0))
                                                {
                                                    DataRow RowPatientForm2 = dtLivePatientForm.NewRow();
                                                    RowPatientForm2["ehrfield_value"] = item.ehr_value.phone;
                                                    RowPatientForm2["ehrfield"] = "mobile";
                                                    RowPatientForm2["PatientForm_Web_ID"] = item._id;
                                                    RowPatientForm2["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPatientForm2["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                    AddinfoinForm(ref RowPatientForm2, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);

                                                    dtLivePatientForm.Rows.Add(RowPatientForm2);
                                                }
                                            }

                                        }
                                    }
                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Updated data table dtLivePatientForm count  (" + dtLivePatientForm.Rows.Count + ")");
                                    //ObjGoalBase.WriteToSyncLogFile("Comapre patient");
                                    DataTable dtPatientFormCopy = dtLivePatientForm.Clone();
                                    dtPatientFormCopy.Load(dtLivePatientForm.CreateDataReader());
                                    string firstName = "";
                                    string LastName = "";
                                    string MiddleName = "";
                                    string mobileNo = "";
                                    string primaryProviderFirstName = "", secondaryProviderFirstName = "";

                                    //ObjGoalBase.WriteToSyncLogFile("Start Set Patient Insurance ." + dtPatientFormCopy.Rows.Count.ToString());

                                    dtPatientFormCopy.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                                         o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                                       .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                                       .All(o =>
                                       {
                                           var resultFirst_Name = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                && a.Field<string>("ehrfield").ToString().ToUpper() == "FIRST_NAME");
                                           var resultLastName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                               && a.Field<string>("ehrfield").ToString().ToUpper() == "LAST_NAME");
                                           var resultMobile = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                               && a.Field<string>("ehrfield").ToString().ToUpper() == "MOBILE");

                                           var primaryInsurance = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                               && a.Field<string>("ehrfield").ToString().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME");

                                           var secondaryInsurance = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                               && a.Field<string>("ehrfield").ToString().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME");

                                           if (resultFirst_Name.Count() == 0)
                                           {
                                               IndertDefaultRowForFirstNameLastNameMobile(o.ToString(), "first_name", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           }
                                           if (resultLastName.Count() == 0)
                                           {
                                               IndertDefaultRowForFirstNameLastNameMobile(o.ToString(), "last_name", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           }
                                           if (resultMobile.Count() == 0)
                                           {
                                               IndertDefaultRowForFirstNameLastNameMobile(o.ToString(), "mobile", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                           }
                                           #region Get Patient INsurance
                                           if (primaryInsurance.Count() > 0)
                                           {
                                               var primaryInsuranceName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                               && a.Field<string>("ehrfield").ToString().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME").Select(b => b.Field<object>("ehrfield_value").ToString());
                                               if (primaryInsuranceName.Count() > 0)
                                               {
                                                   var primararyInsuranceName = dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == primaryInsuranceName.First().ToString().ToUpper());
                                                   if (primararyInsuranceName.Count() > 0)
                                                   {
                                                       InsertPatientInsuranceColumns(o.ToString(), "prim_relationship", "S", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "prim_employer_id", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == primaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("employer_id")).First().ToString(), ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "prim_outstanding_balance", "0", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "prim_benefits_remaining", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == primaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("benefits_remaining")).First().ToString(), ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "prim_remaining_deductible", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == primaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("remaining_deductible")).First().ToString(), ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "patient_status", "Y", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "policy_holder_status", "Y", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                   }
                                                   else if (Utility.Application_ID == 1)
                                                   {
                                                       var primarySubScriber = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                              && a.Field<string>("ehrfield").ToString().ToUpper() == "PRIMARY_SUBSCRIBER_ID");
                                                       if (primarySubScriber != null && primarySubScriber.Count() > 0)
                                                       {
                                                           primarySubScriber.All(v => { v["ehrfield_value"] = ""; return true; });
                                                       }
                                                   }
                                               }
                                           }
                                           else if (primaryInsurance.Count() <= 0)
                                           {
                                               var primarySubScriber = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                      && a.Field<string>("ehrfield").ToString().ToUpper() == "PRIMARY_SUBSCRIBER_ID");
                                               if (primarySubScriber != null && primarySubScriber.Count() > 0)
                                               {
                                                   primarySubScriber.All(v => { v["ehrfield_value"] = ""; return true; });
                                               }
                                           }
                                           if (secondaryInsurance.Count() > 0)
                                           {
                                               var secondaryInsuranceName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                               && a.Field<string>("ehrfield").ToString().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME").Select(b => b.Field<object>("ehrfield_value").ToString());

                                               if (secondaryInsuranceName.Count() > 0)
                                               {
                                                   var secInsuranceName = dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == secondaryInsuranceName.First().ToString().ToUpper());
                                                   if (secInsuranceName.Count() > 0)
                                                   {
                                                       InsertPatientInsuranceColumns(o.ToString(), "sec_relationship", "S", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "sec_employer_id", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == secondaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("employer_id")).First().ToString(), ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "sec_outstanding_balance", "0", ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "sec_benefits_remaining", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == secondaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("benefits_remaining")).First().ToString(), ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                       InsertPatientInsuranceColumns(o.ToString(), "sec_remaining_deductible", dtEagleSoftInsuranceName.AsEnumerable().Where(c => c.Field<object>("name").ToString().ToUpper() == secondaryInsuranceName.First().ToString().ToUpper()).Select(d => d.Field<object>("remaining_deductible")).First().ToString(), ref dtLivePatientForm, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), folder_ehr_id, folder_name, DocNameFormat, Form_Name, Patient_Name, submit_time);
                                                   }
                                               }
                                               else
                                               {
                                                   var primarySubScriber = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                             && a.Field<string>("ehrfield").ToString().ToUpper() == "SECONDARY_SUBSCRIBER_ID");
                                                   if (primarySubScriber != null && primarySubScriber.Count() > 0)
                                                   {
                                                       primarySubScriber.All(v => { v["ehrfield_value"] = ""; return true; });
                                                   }
                                               }
                                           }
                                           else if (secondaryInsurance.Count() <= 0)
                                           {
                                               var primarySubScriber = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                      && a.Field<string>("ehrfield").ToString().ToUpper() == "SECONDARY_SUBSCRIBER_ID");
                                               if (primarySubScriber != null && primarySubScriber.Count() > 0)
                                               {
                                                   primarySubScriber.All(v => { v["ehrfield_value"] = ""; return true; });
                                               }
                                           }
                                           #endregion
                                           return
                                       true;
                                       });
                                    dtLivePatientForm.AcceptChanges();
                                    //ObjGoalBase.WriteToSyncLogFile("1123");
                                    dtPatientFormCopy.Clear();
                                    dtPatientFormCopy = dtLivePatientForm.Clone();
                                    dtPatientFormCopy.Load(dtLivePatientForm.CreateDataReader());

                                    // ObjGoalBase.WriteToSyncLogFile("Start Set FirstName,LastName,MiddleName,Mobile & Providers ." + dtPatientFormCopy.Rows.Count.ToString());

                                    dtPatientFormCopy.AsEnumerable()
                                        .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                                        .All(o =>
                                        {
                                            var result = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                && a.Field<string>("ehrfield").ToString().ToUpper() == "FIRST_NAME");
                                            var result1 = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                && a.Field<string>("ehrfield").ToString().ToUpper() == "LAST_NAME");
                                            var result2 = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                && a.Field<string>("ehrfield").ToString().ToUpper() == "MIDDLE_NAME");
                                            var result3 = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                && a.Field<string>("ehrfield").ToString().ToUpper() == "MOBILE");
                                            var resultPreProvider = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                && a.Field<string>("ehrfield").ToString().ToUpper() == "PRI_PROVIDER_ID");
                                            var resultSecondaryProvider = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString()
                                                && a.Field<string>("ehrfield").ToString().ToUpper() == "SEC_PROVIDER_ID");

                                            //ObjGoalBase.WriteToSyncLogFile("Loop PatientName " + result.Count().ToString() + " " + result1.Count().ToString() + " " + result3.Count().ToString());

                                            if (result != null && result1 != null && result3 != null && result.Count() > 0 && result1.Count() > 0 && result3.Count() > 0)
                                            {
                                                firstName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "FIRST_NAME").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                                LastName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "LAST_NAME").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                                //MiddleName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "MIDDLE_NAME").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                                mobileNo = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "MOBILE").Select(b => b.Field<string>("EHRField_Value")).First().ToString();

                                                // ObjGoalBase.WriteToSyncLogFile("Condition True " + firstName.ToString() +  " " + LastName.ToString() + " " + mobileNo.ToString() );
                                                //dtEHRPatientData.Select(" First_name = '" + firstName.ToString() + "' AND Last_name = '" + LastName.ToString() + "' AND ( ( (Mobile = '' OR Mobile = null) AND " + Utility.ConvertContactNumber(mobileNo.ToString().Trim()) + " = '0000000000') OR ( Mobile <> '' AND Mobile <> null AND Mobile = '" + Utility.ConvertContactNumber(mobileNo.ToString().Trim()) + "' ))")
                                                if (dtEHRPatientData.Select(" First_name = '" + firstName.ToString() + "' AND Last_name = '" + LastName.ToString() + "'").Count() > 0)
                                                {

                                                    bool ismatchedrecords = false;
                                                    //rooja for task https://app.asana.com/0/1204010716278938/1204810921234162/f
                                                    foreach (DataRow drRow in dtEHRPatientData.Select(" First_name = '" + firstName.ToString() + "' AND Last_name = '" + LastName.ToString() + "' and EHR_STATUS = 'Active'"))
                                                    {
                                                        if (drRow["Mobile"] != null && drRow["Mobile"].ToString() != "" && Utility.ConvertContactNumber(drRow["Mobile"].ToString().ToUpper().Trim()) == Utility.ConvertContactNumber(mobileNo.ToString().Trim()).ToUpper())
                                                        {
                                                            ismatchedrecords = true;
                                                        }
                                                        else if (drRow["Home_Phone"] != null && drRow["Home_Phone"].ToString() != "" && Utility.ConvertContactNumber(drRow["Home_Phone"].ToString().ToUpper().Trim()) == Utility.ConvertContactNumber(mobileNo.ToString().Trim()).ToUpper())
                                                        {
                                                            ismatchedrecords = true;
                                                        }
                                                        else if (drRow["Work_Phone"] != null && drRow["Work_Phone"].ToString() != "" && Utility.ConvertContactNumber(drRow["Work_Phone"].ToString().ToUpper().Trim()) == Utility.ConvertContactNumber(mobileNo.ToString().Trim()).ToUpper())
                                                        {
                                                            ismatchedrecords = true;
                                                        }
                                                        if (ismatchedrecords)
                                                        {
                                                            dtLivePatientForm.AsEnumerable().Where(a =>
                                                            (a.Field<object>("Patient_EHR_ID") == null || (a.Field<object>("Patient_EHR_ID") != null
                                                            && a.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (a.Field<object>("Patient_EHR_ID") != null
                                                            && a.Field<object>("Patient_EHR_ID").ToString() == "0")) && a.Field<string>("PatientForm_Web_ID") == o.ToString())
                                                                .All(d =>
                                                                {
                                                                    d["Patient_EHR_ID"] = drRow["Patient_EHR_ID"];
                                                                    return true;
                                                                });
                                                            break;
                                                        }
                                                    }

                                                }
                                            }
                                            //ObjGoalBase.WriteToSyncLogFile("Check Provider " + resultPreProvider.Count().ToString());
                                            if ((resultPreProvider != null && resultPreProvider.Count() > 0) || (resultSecondaryProvider != null && resultSecondaryProvider.Count() > 0))
                                            {
                                                //ObjGoalBase.WriteToSyncLogFile(" Provider condition true " + resultPreProvider.Count().ToString());
                                                if (resultPreProvider.Count() > 0)
                                                {
                                                    primaryProviderFirstName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "PRI_PROVIDER_ID").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                                    SetPrimarySecondaryProviderId(primaryProviderFirstName.ToUpper(), dtLocalProviderData, "PRI_PROVIDER_ID", o.ToString(), ref dtLivePatientForm);
                                                }
                                                if (resultSecondaryProvider.Count() > 0)
                                                {
                                                    secondaryProviderFirstName = dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("EHRField").ToString().ToUpper() == "SEC_PROVIDER_ID").Select(b => b.Field<string>("EHRField_Value")).First().ToString();
                                                    SetPrimarySecondaryProviderId(secondaryProviderFirstName.ToUpper(), dtLocalProviderData, "SEC_PROVIDER_ID", o.ToString(), ref dtLivePatientForm);
                                                }
                                            }

                                            return true;
                                        });
                                    //ObjGoalBase.WriteToSyncLogFile("Check for Update Records ." + dtLivePatientForm.Rows.Count.ToString());
                                    foreach (DataRow dtDtxRow in dtLivePatientForm.Rows)
                                    {
                                        DataRow[] row = dtLocalPatientForm.Copy().Select("PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND ehrfield = '" + dtDtxRow["ehrfield"].ToString().Trim() + "' ");
                                        if (row.Length > 0)
                                        {
                                            dtDtxRow["InsUptDlt"] = 2;
                                        }
                                        else
                                        {
                                            dtDtxRow["InsUptDlt"] = 1;
                                        }
                                    }

                                    foreach (DataRow dtDtxRow in dtLivePatientFormDiseaseResponse.Rows)
                                    {
                                        DataRow[] row = dtPatientFormDiseaseResponse.Copy().Select("DiseaseMaster_Web_ID = '" + dtDtxRow["DiseaseMaster_Web_ID"].ToString().Trim() + "' AND PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND Disease_EHR_Id = '" + dtDtxRow["Disease_EHR_Id"].ToString().Trim() + "'");
                                        if (row.Length > 0)
                                        {
                                            dtDtxRow["InsUptDlt"] = 2;
                                            dtDtxRow["DiseaseResponse_Local_ID"] = row[0]["DiseaseResponse_Local_ID"];
                                        }
                                        else
                                        {
                                            dtDtxRow["InsUptDlt"] = 1;
                                        }
                                    }
                                    foreach (DataRow dtDtxRow in dtLivePatientFormDiseaseDeleteResponse.Rows)
                                    {
                                        DataRow[] row = dtPatientFormDiseaseDeleteResponse.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "' AND PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND Disease_EHR_Id = '" + dtDtxRow["Disease_EHR_Id"].ToString().Trim() + "' AND Disease_Type = '" + dtDtxRow["Disease_Type"].ToString().Trim() + "'");
                                        if (row.Length > 0)
                                        {
                                            dtDtxRow["InsUptDlt"] = 2;
                                            dtDtxRow["DiseaseDeleteResponse_Local_ID"] = row[0]["DiseaseDeleteResponse_Local_ID"];
                                        }
                                        else
                                        {
                                            dtDtxRow["InsUptDlt"] = 1;
                                        }
                                    }

                                    foreach (DataRow dtDtxRow in dtLivePatientFormMedicationRemovedResponse.Rows)
                                    {
                                        DataRow[] row = dtPatientFormMedicationRemovedResponse.Copy().Select("PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND PatientMedication_EHR_Id = '" + dtDtxRow["PatientMedication_EHR_Id"].ToString().Trim() + "'");
                                        if (row.Length > 0)
                                        {
                                            dtDtxRow["InsUptDlt"] = 2;
                                            dtDtxRow["MedicationRemovedResponse_Local_ID"] = row[0]["MedicationRemovedResponse_Local_ID"];
                                        }
                                        else
                                        {
                                            dtDtxRow["InsUptDlt"] = 1;
                                        }
                                    }
                                    foreach (DataRow dtDtxRow in dtLivePatientFormMedicationResponse.Rows)
                                    {
                                        DataRow[] row = dtPatientFormMedicationResponse.Copy().Select("PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND PatientMedication_EHR_Id = '" + dtDtxRow["PatientMedication_EHR_Id"].ToString().Trim() + "'");
                                        if (row.Length > 0)
                                        {
                                            dtDtxRow["InsUptDlt"] = 2;
                                            dtDtxRow["MedicationResponse_Local_ID"] = row[0]["MedicationResponse_Local_ID"];
                                        }
                                        else
                                        {
                                            dtDtxRow["InsUptDlt"] = 1;
                                        }
                                    }
                                    dtLivePatientForm.AcceptChanges();
                                    // Utility.WriteToSyncLogFile_All("Check to save records in Local " + dtLivePatientForm.Rows.Count.ToString());
                                    if (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0)
                                    {
                                        DataTable dtSaveRecords = dtLivePatientForm.Clone();
                                        if (dtLivePatientForm.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                                        {
                                            dtSaveRecords.Load(dtLivePatientForm.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                                        }
                                        // Utility.WriteToSyncLogFile_All("Send to save records in Local " + dtLivePatientForm.Rows.Count.ToString());
                                        // Check SSN Condition
                                        dtSaveRecords = CheckPatientWiseSSNvalidation(dtSaveRecords, dtEHRPatientData);
                                        //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "Updated data table dtSaveRecords count  (" + dtSaveRecords.Rows.Count + ")");
                                        bool status = PullLiveDatabaseBAL.Save_PatientForm_Live_To_Local(dtSaveRecords,false, Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm);

                                        if (status)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm," Save_PatientForm_Live_To_Local  status :  success");
                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                                            if (UpdateSync_Table_Datetime)
                                            {
                                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, " UpdateSync_Table_Datetime  status :  success");
                                            }
                                            else
                                            {
                                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, " UpdateSync_Table_Datetime  status :  failed");
                                            }
                                            SynchTrackerBAL.Save_Tracker_To_Local(dtLivePatientFormDiseaseResponse, "DiseaseResponse", "DiseaseResponse_Local_ID", "DiseaseResponse_Local_ID", Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm);
                                            SynchTrackerBAL.Save_Tracker_To_Local(dtLivePatientFormDiseaseDeleteResponse, "DiseaseDeleteResponse", "DiseaseDeleteResponse_Local_ID", "DiseaseDeleteResponse_Local_ID", Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm);
                                            SynchTrackerBAL.Save_Tracker_To_Local(dtLivePatientFormMedicationResponse, "MedicationResponse", "MedicationResponse_Local_ID", "MedicationResponse_Local_ID", Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm);
                                            SynchTrackerBAL.Save_Tracker_To_Local(dtLivePatientFormMedicationRemovedResponse, "MedicationRemovedResponse", "MedicationRemovedResponse_Local_ID", "MedicationRemovedResponse_Local_ID", Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm);

                                            DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                            // bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientForm_Pull");
                                            GoalBase.WriteToSyncLogFile_Static("PatientForm Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                        }
                                        //dtLivePatientForm.Select("InsUptDlt IN (1,2,3)")

                                    }
                                    else
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, " Save_PatientForm_Live_To_Local  status :  failed");
                                        GoalBase.WriteToSyncLogFile_Static("PatientForm Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"] + " Records Not found on Adit Server");
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                                        if (UpdateSync_Table_Datetime)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, " UpdateSync_Table_Datetime  status :  success");
                                        }
                                        else
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, " UpdateSync_Table_Datetime  status :  failed");
                                        }
                                    }
                                    // GetPatientDocument();
                                }
                                else
                                {
                                    GoalBase.WriteToSyncLogFile_Static("PatientForm Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"] + " Pending Records Not found on Adit Server");
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                                }
                            }
                            else
                            {
                                Utility.WriteToSyncLogFile_All("PatientForm_AditLocationSyncEnable False : " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                            }
                        }
                        IS_EHRDocPulled = false;
                        Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFormt, Utility._EHRLogdirectory_EHR_PatientForm, "IS_EHRDocPulled  : false");
                    }
                }
                else
                {
                    Utility.WriteToSyncLogFile_All("PatientForm_FirstCondition false : ProviderSyncFirstTime = " + IsProviderSyncedFirstTime.ToString() + ", EHR Doc pull : " + IS_EHRDocPulled.ToString());
                }
            }
            catch (Exception ex)
            {
                IS_EHRDocPulled = false;
                GoalBase.WriteToErrorLogFile_Static("[PatientForm Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        private static DataTable CheckPatientWiseSSNvalidation(DataTable dtPatientFormLive, DataTable dtEHRPatientData)
        {
            try
            {
                string patientEHRId = "";
                DataTable dtPatientTemp = dtPatientFormLive.Copy();
                DataTable dtRemoveRecords = dtPatientFormLive.Copy();
                dtPatientTemp.AsEnumerable()
                  .Where(o => o.Field<object>("Patient_EHR_ID") != null &&
                         o.Field<object>("Patient_EHR_ID").ToString() != string.Empty &&
                         o.Field<object>("PatientForm_Web_ID") != null &&
                         o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                       .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                       .All(o =>
                       {

                           var patientehr = dtPatientTemp.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString());//.Select(b => b.Field<string>("Patient_EHR_ID")).FirstOrDefault();
                           var patientssn = dtPatientTemp.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("Patient_EHR_ID").ToString() == patientehr.First().Field<string>("Patient_EHR_ID").ToString() && a.Field<string>("ehrfield").ToString().ToUpper() == "SSN").Select(b => b.Field<string>("ehrfield_value")).FirstOrDefault();
                           if (patientehr != null && patientehr.Count() > 0 && patientssn != null && patientssn.ToString() != "")
                           {
                               var matchssn = dtEHRPatientData.AsEnumerable().Where(c => c.Field<object>("Patient_EHR_ID").ToString().Trim() != patientehr.First().Field<object>("Patient_EHR_ID").ToString().Trim()
                                                                                      && c.Field<object>("ssn") != null && c.Field<object>("ssn").ToString() != string.Empty
                                                                                      && c.Field<object>("ssn").ToString().ToUpper().Trim() == patientssn.ToString().ToString().ToUpper().Trim()
                                                                                      //&& c.Field<object>("first_name").ToString().ToUpper() == patientehr.First().Field<object>("first_name").ToString().ToUpper()
                                                                                      //&& c.Field<object>("last_name").ToString().ToUpper() == patientehr.First().Field<object>("last_name").ToString().ToUpper()
                                                                                      //&& Utility.ConvertContactNumber( c.Field<object>("mobile").ToString().ToUpper()) == Utility.ConvertContactNumber(patientehr.First().Field<object>("mobile").ToString().ToUpper())
                                                                                      );
                               if (matchssn != null && matchssn.Count() > 0)
                               {
                                   var result = from rw in dtRemoveRecords.AsEnumerable()
                                                where
                                                rw.Field<string>("Patient_EHR_ID").ToString().Trim() == patientehr.First().Field<object>("Patient_EHR_ID").ToString().Trim()
                                                && rw.Field<string>("PatientForm_Web_ID").ToString().Trim() == patientehr.First().Field<object>("PatientForm_Web_ID").ToString().Trim()
                                                 && rw.Field<string>("ehrfield").ToString().ToUpper().Trim() == "SSN"
                                                //&& rw.Field<string>("first_name") == patientehr.First().Field<object>("first_name").ToString()
                                                //&& rw.Field<string>("last_name") == patientehr.First().Field<object>("last_name").ToString()
                                                //&& Utility.ConvertContactNumber(rw.Field<object>("mobile").ToString().ToUpper()) == Utility.ConvertContactNumber(patientehr.First().Field<object>("mobile").ToString().ToUpper())
                                                select rw;

                                   dtRemoveRecords.Rows.Remove(result.First());
                               }
                               else
                               {


                               }
                           }

                           return true;
                       });
                if (
              dtPatientTemp.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                   o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty).Count() > 0)
                {
                    dtPatientTemp.AsEnumerable().Where(o => (o.Field<object>("Patient_EHR_ID") == null || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == string.Empty) || (o.Field<object>("Patient_EHR_ID") != null && o.Field<object>("Patient_EHR_ID").ToString() == "0")) &&
                     o.Field<object>("PatientForm_Web_ID").ToString() != string.Empty)
                         .Select(c => c.Field<string>("PatientForm_Web_ID")).Distinct()
                         .All(o =>
                         {

                             var patientehr = dtPatientTemp.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString());//.Select(b => b.Field<string>("Patient_EHR_ID")).FirstOrDefault();
                             var patientssn = dtPatientTemp.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == o.ToString() && a.Field<string>("ehrfield").ToString().ToUpper().Trim() == "SSN").Select(b => b.Field<string>("ehrfield_value")).FirstOrDefault();
                             if (patientehr != null && patientehr.Count() > 0 && patientssn != null && patientssn.ToString() != "")
                             {
                                 var matchssn = dtEHRPatientData.AsEnumerable().Where(c => c.Field<object>("ssn") != null && c.Field<object>("ssn").ToString() != string.Empty && c.Field<object>("ssn").ToString().ToUpper().Trim() == patientssn.ToString().ToString().ToUpper().Trim()
                                                                                        //&& c.Field<object>("first_name").ToString().ToUpper().Trim() != patientehr.First().Field<object>("first_name").ToString().ToUpper().Trim()
                                                                                        //&& c.Field<object>("last_name").ToString().ToUpper().Trim() != patientehr.First().Field<object>("last_name").ToString().ToUpper().Trim()
                                                                                        //&& Utility.ConvertContactNumber(c.Field<object>("mobile").ToString().ToUpper()) != Utility.ConvertContactNumber(patientehr.First().Field<object>("mobile").ToString().ToUpper())
                                                                                        );
                                 if (matchssn != null && matchssn.Count() > 0)
                                 {
                                     var result = from rw in dtRemoveRecords.AsEnumerable()
                                                  where rw.Field<string>("PatientForm_Web_ID").ToString().Trim() == patientehr.First().Field<object>("PatientForm_Web_ID").ToString().Trim()
                                                  && rw.Field<string>("ehrfield").ToString().ToUpper().Trim() == "SSN"
                                                  //&& rw.Field<string>("last_name") == patientehr.First().Field<object>("last_name").ToString()
                                                  //&& Utility.ConvertContactNumber(rw.Field<object>("mobile").ToString().ToUpper()) == Utility.ConvertContactNumber(patientehr.First().Field<object>("mobile").ToString().ToUpper())
                                                  select rw;

                                     dtRemoveRecords.Rows.Remove(result.First());
                                 }
                                 else
                                 {


                                 }
                             }

                             return true;
                         });
                }

                return dtRemoveRecords;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void SynchDataLiveDB_Pull_PatientPortal()
        {
            try
            {

                Utility.WriteToSyncLogFile_All("Call Patient form PUll");
                if (IsProviderSyncedFirstTime && !IS_EHRDocPulled)
                {
                    Utility.WriteToSyncLogFile_All("EHR Pulled Condition Falsed");
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        Utility.WriteToSyncLogFile_All("Start Location Loop");
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            Utility.WriteToSyncLogFile_All("Adit App Sync is Enabled");
                            IS_EHRDocPulled = true;
                            string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("PatientPortal", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                            string ab = Utility.WebAdminUserToken;
                            var client = new RestClient(strApiPatientForm);
                            Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "Call PatientPortal API");
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            request.AddHeader("action", "EHRPFIMPORT");
                            Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "Request Sent into the API " + " Authorization, TokenKey & action");
                            GoalBase.WriteToPaymentLogFromAll_Static("PatientPortal_Request Called " + strApiPatientForm.ToString());
                            IRestResponse response = client.Execute(request);

                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync_ResponseError (Adit Server To Local Database)] : " + response.ErrorMessage);
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                IS_EHRDocPulled = false;
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "Response received from API (" + response.Content.ToString() + ")");
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "Response is null");
                            }
                            Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "----------------------------Deserialize repsonse--------------------------------");
                            GoalBase.WriteToPaymentLogFromAll_Static("Response Received " + response.Content.ToString());
                            var ApptPatientFormDto = JsonConvert.DeserializeObject<Pull_PatientFormBO>(response.Content);

                            if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                            {
                                if (ApptPatientFormDto.data.Count == 0)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "Deserialize response ApptPatientFormDto count :  (" + ApptPatientFormDto.data.Count + ") no record ");
                                }
                                if (ApptPatientFormDto.data != null && ApptPatientFormDto.data.Count() > 0)
                                {
                                    //Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "Deserialize repsonse from API(" + ApptPatientFormDto.message.ToString() + ")");
                                    Utility.WriteToSyncLogFile_All("Response Received count " + ApptPatientFormDto.data.Count().ToString());

                                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPatientFormData(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    //Utility.WriteToSyncLogFile_All("GetLocalPatientFormData dtLocalPatientForm count " + dtLocalPatientForm.Rows.Count.ToString());
                                    DataTable dtEHRPatientData = new DataTable();
                                    DataTable dtEagleSoftInsuranceName = new DataTable();

                                    if (!dtEHRPatientData.Columns.Contains("Patient_Web_ID"))
                                    {
                                        dtEHRPatientData.Columns.Add("Patient_Web_ID", typeof(string));
                                    }

                                    DataTable dtLivePatientForm = dtLocalPatientForm.Clone();
                                    dtLivePatientForm.Columns.Add("InsUptDlt", typeof(int));
                                    //dtLivePatientForm.Columns.Add("PatientType", typeof(int));

                                    foreach (var item in ApptPatientFormDto.data)
                                    {
                                        if (item.ehrmap != null && item.ehrmap.Count() > 0)
                                        {
                                            foreach (var subitem in item.ehrmap)
                                            {
                                                if (subitem.ehrField.ToString().Trim().ToUpper() == "FIRST_NAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "LAST_NAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "MOBILE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "ADDRESS_ONE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "ADDRESS_TWO" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "BIRTH_DATE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "CITY" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "EMAIL" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "HOME_PHONE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "MARITAL_STATUS" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "MIDDLE_NAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "PREFERRED_NAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "PRI_PROVIDER_ID" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "RECEIVE_EMAIL" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "RECEIVE_SMS" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SALUTATION" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SEC_PROVIDER_ID" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SEX" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "WORK_PHONE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "STATE" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID" ||
                                                    subitem.ehrField.ToString().Trim().ToUpper() == "ZIPCODE")
                                                {
                                                    if (subitem.value.ToString() != "")
                                                    {
                                                        DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                                        if (item.pinfo != null)
                                                        {
                                                            RowPatientForm["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                            RowPatientForm["Patient_Web_ID"] = item.pinfo._id;
                                                        }
                                                        RowPatientForm["PatientForm_Web_ID"] = item.esId;
                                                        RowPatientForm["ehrfield"] = subitem.ehrField;

                                                        RowPatientForm["ehrfield_value"] = subitem.value.ToString().Replace("'", "");

                                                        try
                                                        {
                                                            if (RowPatientForm["ehrfield"].ToString().ToLower() == "birth_date")
                                                            {
                                                                RowPatientForm["ehrfield_value"] = Convert.ToDateTime(Utility.CheckValidDatetime(RowPatientForm["ehrfield_value"].ToString())).ToString("yyyy/MM/dd");
                                                            }
                                                        }
                                                        catch (Exception)
                                                        {

                                                        }
                                                        RowPatientForm["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        RowPatientForm["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        RowPatientForm["PatientType"] = 1;

                                                        AddinfoinForm(ref RowPatientForm, item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time);

                                                        dtLivePatientForm.Rows.Add(RowPatientForm);
                                                        dtLivePatientForm.AcceptChanges();
                                                    }
                                                }

                                            }
                                            if (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && item.pinfo != null)
                                            {
                                                var result = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item.esId.ToUpper()
                                                    && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "FIRST_NAME");

                                                if (result == null || (result != null && result.Count() == 0))
                                                {
                                                    InsertRowINtodatatable(ref dtLivePatientForm, "first_name", item.pinfo.first_name, item.esId, item.pinfo.patient_ehr_id, item.pinfo._id, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time, 1);
                                                }
                                                var result1 = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item.esId.ToUpper()
                                                   && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "LAST_NAME");

                                                if (result1 == null || (result1 != null && result1.Count() == 0))
                                                {
                                                    InsertRowINtodatatable(ref dtLivePatientForm, "last_name", item.pinfo.last_name, item.esId, item.pinfo.patient_ehr_id, item.pinfo._id, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time, 1);
                                                }
                                                var result2 = dtLivePatientForm.AsEnumerable().Where(o => o.Field<object>("PatientForm_Web_ID").ToString().ToUpper() == item.esId.ToUpper()
                                                   && o.Field<object>("ehrfield").ToString().Trim().ToUpper() == "MOBILE");

                                                if (result2 == null || (result2 != null && result2.Count() == 0))
                                                {
                                                    InsertRowINtodatatable(ref dtLivePatientForm, "mobile", item.pinfo.mobile == null ? "0000000000" : item.pinfo.mobile, item.esId, item.pinfo.patient_ehr_id, item.pinfo._id, Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), item.folder_ehr_id, item.folder_name, item.form_name_format, item.form_name, item.patient_name, item.submit_time, 1);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (item.pinfo != null)
                                            {
                                                DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                                if (item.pinfo != null)
                                                {
                                                    RowPatientForm["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                    RowPatientForm["Patient_Web_ID"] = item.pinfo._id;
                                                    RowPatientForm["ehrfield_value"] = item.pinfo.first_name;
                                                    RowPatientForm["ehrfield"] = "first_name";
                                                    RowPatientForm["PatientForm_Web_ID"] = item.esId;
                                                    RowPatientForm["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPatientForm["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                }
                                                dtLivePatientForm.Rows.Add(RowPatientForm);
                                                dtLivePatientForm.AcceptChanges();

                                                DataRow RowPatientForm1 = dtLivePatientForm.NewRow();
                                                if (item.pinfo != null)
                                                {
                                                    RowPatientForm1["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                    RowPatientForm1["Patient_Web_ID"] = item.pinfo._id;
                                                    RowPatientForm1["ehrfield_value"] = item.pinfo.mobile == null ? "0000000000" : item.pinfo.mobile;
                                                    RowPatientForm1["ehrfield"] = "mobile";
                                                    RowPatientForm1["PatientForm_Web_ID"] = item.esId;
                                                    RowPatientForm1["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPatientForm1["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                }

                                                dtLivePatientForm.Rows.Add(RowPatientForm1);
                                                dtLivePatientForm.AcceptChanges();

                                                DataRow RowPatientForm2 = dtLivePatientForm.NewRow();
                                                if (item.pinfo != null)
                                                {
                                                    RowPatientForm2["Patient_EHR_ID"] = item.pinfo.patient_ehr_id.ToString().Trim();
                                                    RowPatientForm2["Patient_Web_ID"] = item.pinfo._id;
                                                    RowPatientForm2["ehrfield_value"] = item.pinfo.last_name;
                                                    RowPatientForm2["ehrfield"] = "last_name";
                                                    RowPatientForm2["PatientForm_Web_ID"] = item.esId;
                                                    RowPatientForm2["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPatientForm2["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                }

                                                dtLivePatientForm.Rows.Add(RowPatientForm2);
                                                dtLivePatientForm.AcceptChanges();
                                            }
                                        }
                                        if (item.ehr_value != null)
                                        {
                                            string firstname = "", lastname = "";
                                            if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty && item.ehr_value.patientName.Contains(" "))
                                            {
                                                firstname = item.ehr_value.patientName.Substring(0, item.ehr_value.patientName.IndexOf(" ")).Trim();
                                                lastname = item.ehr_value.patientName.Substring(item.ehr_value.patientName.IndexOf(" "), (item.ehr_value.patientName.Length - item.ehr_value.patientName.IndexOf(" "))).Trim();
                                            }
                                            else if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty && !item.ehr_value.patientName.Contains(" "))
                                            {
                                                firstname = item.ehr_value.patientName.ToString();
                                                lastname = "NA";
                                            }

                                            if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty)
                                            {
                                                if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item.esId.ToString() + "' AND ehrfield = 'first_name'").Count() == 0))
                                                {
                                                    if (firstname != string.Empty)
                                                    {
                                                        DataRow RowPatientForm = dtLivePatientForm.NewRow();
                                                        RowPatientForm["ehrfield_value"] = firstname;
                                                        RowPatientForm["ehrfield"] = "first_name";
                                                        RowPatientForm["PatientForm_Web_ID"] = item.esId;
                                                        RowPatientForm["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        RowPatientForm["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        dtLivePatientForm.Rows.Add(RowPatientForm);
                                                    }
                                                }
                                            }


                                            if (item.ehr_value.patientName != null && item.ehr_value.patientName != string.Empty)
                                            {
                                                if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item.esId.ToString() + "' AND ehrfield = 'last_name'").Count() == 0))
                                                {
                                                    if (lastname != string.Empty)
                                                    {
                                                        DataRow RowPatientForm1 = dtLivePatientForm.NewRow();
                                                        RowPatientForm1["ehrfield_value"] = lastname;
                                                        RowPatientForm1["ehrfield"] = "last_name";
                                                        RowPatientForm1["PatientForm_Web_ID"] = item.esId;
                                                        RowPatientForm1["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                        RowPatientForm1["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                        dtLivePatientForm.Rows.Add(RowPatientForm1);
                                                    }
                                                }
                                            }


                                            if (item.ehr_value.phone != null && item.ehr_value.phone != string.Empty)
                                            {
                                                if ((dtLivePatientForm != null && dtLivePatientForm.Rows.Count == 0) || (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0 && dtLivePatientForm.Select("PatientForm_Web_ID = '" + item.esId.ToString() + "' AND ehrfield = 'mobile'").Count() == 0))
                                                {
                                                    DataRow RowPatientForm2 = dtLivePatientForm.NewRow();
                                                    RowPatientForm2["ehrfield_value"] = item.ehr_value.phone;
                                                    RowPatientForm2["ehrfield"] = "mobile";
                                                    RowPatientForm2["PatientForm_Web_ID"] = item.esId;
                                                    RowPatientForm2["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPatientForm2["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                    dtLivePatientForm.Rows.Add(RowPatientForm2);
                                                }
                                            }

                                        }
                                    }
                                    //Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "data table dtLivePatientForm count  (" + dtLivePatientForm.Rows.Count + ")");
                                    //ObjGoalBase.WriteToSyncLogFile("Comapre patient");
                                    DataTable dtPatientFormCopy = dtLivePatientForm.Clone();
                                    dtPatientFormCopy.Load(dtLivePatientForm.CreateDataReader());
                                    string firstName = "";
                                    string LastName = "";
                                    string MiddleName = "";
                                    string mobileNo = "";
                                    string primaryProviderFirstName = "", secondaryProviderFirstName = "";


                                    dtLivePatientForm.AcceptChanges();
                                    //ObjGoalBase.WriteToSyncLogFile("1123");
                                    dtPatientFormCopy.Clear();
                                    dtPatientFormCopy = dtLivePatientForm.Clone();
                                    dtPatientFormCopy.Load(dtLivePatientForm.CreateDataReader());

                                    //ObjGoalBase.WriteToSyncLogFile("Check for Update Records ." + dtLivePatientForm.Rows.Count.ToString());
                                    foreach (DataRow dtDtxRow in dtLivePatientForm.Rows)
                                    {
                                        DataRow[] row = dtLocalPatientForm.Copy().Select("PatientForm_Web_ID = '" + dtDtxRow["PatientForm_Web_ID"].ToString().Trim() + "' AND ehrfield = '" + dtDtxRow["ehrfield"].ToString().Trim() + "' ");
                                        if (row.Length > 0)
                                        {
                                            dtDtxRow["InsUptDlt"] = 2;
                                        }
                                        else
                                        {
                                            dtDtxRow["InsUptDlt"] = 1;
                                        }
                                    }



                                    dtLivePatientForm.AcceptChanges();
                                    Utility.WriteToSyncLogFile_All("Check to save records in Local " + dtLivePatientForm.Rows.Count.ToString());
                                    if (dtLivePatientForm != null && dtLivePatientForm.Rows.Count > 0)
                                    {
                                        DataTable dtSaveRecords = dtLivePatientForm.Clone();
                                        if (dtLivePatientForm.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                                        {
                                            dtSaveRecords.Load(dtLivePatientForm.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                                        }
                                        Utility.WriteToSyncLogFile_All("Send to save records in Local " + dtLivePatientForm.Rows.Count.ToString());
                                        if (dtLivePatientForm != null)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "data table dtLivePatientForm count  (" + dtLivePatientForm.Rows.Count + ")");
                                        }
                                        bool status = PullLiveDatabaseBAL.Save_PatientForm_Live_To_Local(dtSaveRecords, true, Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal);
                                        if (status)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "Save_Patientform__Live_To_Local  status : Success");
                                        }
                                        else
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "Save_Patientform__Live_To_Local  status : failed");
                                        }
                                    }
                                    else
                                    {
                                        GoalBase.WriteToSyncLogFile_Static("PatientPortal Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"] + " Records Not found on Adit Server");
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");
                                        if (UpdateSync_Table_Datetime)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "UpdateSync_Table_Datetime  status : Success");
                                        }
                                        else
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "UpdateSync_Table_Datetime  status : failed");
                                        }
                                    }
                                    // GetPatientDocument();
                                }
                                else
                                {
                                    GoalBase.WriteToSyncLogFile_Static("PatientPortal Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"] + " Pending Records Not found on Adit Server");
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Form_Pull");

                                    if (UpdateSync_Table_Datetime)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "UpdateSync_Table_Datetime  status : Success");
                                    }
                                    else
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "UpdateSync_Table_Datetime  status : failed");
                                    }
                                }
                            }
                            else
                            {
                                Utility.WriteToSyncLogFile_All("PatientPortal_AditLocationSyncEnable False : " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                            }
                        }
                        IS_EHRDocPulled = false;
                    }
                }
                else
                {
                    Utility.WriteToSyncLogFile_All("PatientPortal_FirstCondition false : ProviderSyncFirstTime = " + IsProviderSyncedFirstTime.ToString() + ", EHR Doc pull : " + IS_EHRDocPulled.ToString());
                }
            }
            catch (Exception ex)
            {
                IS_EHRDocPulled = false;
                GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        bool IsDocUpdated = true;
        private void GetPatientDocument(string Service_Install_Id, string strPatientFormID = "")
        {
            #region SavePatientDoc
            if (IsDocUpdated)
            {
                try
                {
                    IsDocUpdated = false;
                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPendingPatientFormData(Service_Install_Id, strPatientFormID);
                    DataTable dtLivePatientForm = dtLocalPatientForm.DefaultView.ToTable(true, "PatientForm_Web_ID", "Patient_EHR_ID", "Patient_Web_ID", "Clinic_Number", "Service_Install_Id", "folder_name", "folder_ehr_id", "DocNameFormat", "Form_Name", "Patient_Name");
                    DataTable dtLocalPatientFormDoc = SynchLocalBAL.GetLocalPatientFormDocData(Service_Install_Id, strPatientFormID);
                    DataTable dtLivePatientFormDoc = dtLocalPatientFormDoc.Clone();
                    dtLivePatientFormDoc.Columns.Add("InsUptDlt", typeof(int));
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        foreach (DataRow dr in dtLivePatientForm.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                        {
                            #region
                            try
                            {
                                DataRow[] row = dtLocalPatientFormDoc.Copy().Select("PatientDoc_Web_ID = '" + dr["PatientForm_Web_ID"].ToString().Trim() + "'");
                                if (row.Count() <= 0)
                                {
                                    if (File.Exists(CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientForm_Web_ID"].ToString().Trim() + ".pdf"))
                                    {
                                        File.Delete(CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientForm_Web_ID"].ToString().Trim() + ".pdf");
                                    }
                                }
                                if (!File.Exists(CommonUtility.GetAditDocTempPath() + "\\" + dr["PatientForm_Web_ID"].ToString().Trim() + ".pdf"))
                                {
                                    dtLivePatientFormDoc.Rows.Clear();
                                    DataRow RowPatientFormDoc = dtLivePatientFormDoc.NewRow();
                                    RowPatientFormDoc["PatientDoc_Web_ID"] = dr["PatientForm_Web_ID"].ToString().Trim();
                                    RowPatientFormDoc["InsUptDlt"] = 1;
                                    RowPatientFormDoc["Patient_EHR_ID"] = dr["Patient_EHR_ID"].ToString().Trim();
                                    RowPatientFormDoc["Patient_Web_ID"] = dr["Patient_Web_ID"].ToString().Trim();
                                    if (dr["folder_ehr_id"] == null)
                                    {
                                        RowPatientFormDoc["folder_ehr_id"] = "";// string.IsNullOrEmpty(dr["folder_ehr_id"].ToString()) ? 0 : Convert.ToInt32(dr["folder_ehr_id"]);
                                    }
                                    else
                                    {
                                        RowPatientFormDoc["folder_ehr_id"] = string.IsNullOrEmpty(dr["folder_ehr_id"].ToString()) ? 0 : Convert.ToInt32(dr["folder_ehr_id"]);
                                    }
                                    RowPatientFormDoc["folder_name"] = dr["folder_name"].ToString().Trim();
                                    RowPatientFormDoc["DocNameFormat"] = dr["DocNameFormat"].ToString().Trim();
                                    RowPatientFormDoc["PatientDoc_Name"] = dr["PatientForm_Web_ID"].ToString().Trim() + ".pdf";
                                    RowPatientFormDoc["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                    RowPatientFormDoc["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    RowPatientFormDoc["Form_Name"] = dr["Form_Name"].ToString().Replace("\\", "_");
                                    RowPatientFormDoc["Patient_Name"] = dr["Patient_Name"].ToString();

                                    //rooja 11-4-23
                                    dtLivePatientFormDoc.Rows.Add(RowPatientFormDoc);
                                    dtLivePatientFormDoc.AcceptChanges();

                                    string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("Patient_Document", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, "Call Patient Document API");
                                    PatientDoc PatientpdfDoc = new PatientDoc
                                    {
                                        locationId = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        sid = dr["PatientForm_Web_ID"].ToString().Trim(),
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                                    //MessageBox.Show(jsonString);
                                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                    var clientdoc = new RestClient(strApiPatientForm);
                                    var requestdoc = new RestRequest(Method.POST);
                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                    requestdoc.AddHeader("Authorization", Utility.WebAdminUserToken);
                                    requestdoc.AddHeader("cache-control", "no-cache");
                                    requestdoc.AddHeader("Content-Type", "application/json");
                                    requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                    requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                    //MessageBox.Show("application // json" + jsonString);
                                    //MessageBox.Show("PatientDocument_Request (Patient_EHR_ID = " + RowPatientFormDoc["Patient_EHR_ID"] + ") Called " + strApiPatientForm.ToString());
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, "Request Sent into the API  " +  " Authorization, TokenKey & action");
                                    IRestResponse response = clientdoc.Execute(requestdoc);
                                    if (response.ErrorMessage != null)
                                    {
                                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientForm_Sync_Error : " + response.ErrorMessage);
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientForm Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + response.ErrorMessage);
                                        }
                                        IsDocUpdated = true;
                                        return;
                                    }
                                    if(response.Content != null)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, "Response received from API(" + response.Content.ToString() + ")");
                                    }
                                    else
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, "Response is null");
                                    }
                                    //MessageBox.Show("PatientDocument_Request Response Received." + response.Content.ToString());// + response.Content.ToString());
                                    var jObject = JObject.Parse(response.Content);
                                    if (jObject != null)
                                    {
                                        string DocData = jObject.GetValue("data").ToString();
                                        DocData = DocData.Replace("data:application/pdf;base64,", String.Empty);
                                        // Document. 
                                        bool Docstatus = WriteByteArrayToPdf(DocData, CommonUtility.GetAditDocTempPath(), dr["PatientForm_Web_ID"].ToString().Trim() + ".pdf");
                                        if (Docstatus)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, " WriteByteArrayToPdf : Success");
                                            bool statusDoc = true;
                                            if (row.Length == 0) // temp comment
                                            {
                                                statusDoc = PullLiveDatabaseBAL.Save_PatientFormDoc_Live_To_Local(dtLivePatientFormDoc, Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document);
                                            }
                                            if (statusDoc)
                                            {
                                                Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, " Save_PatientFormDoc_Live_To_Local : Success");
                                                PullLiveDatabaseBAL.Update_PatientFormDoc_Live_To_Local(dr["PatientForm_Web_ID"].ToString().Trim(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document);
                                            }
                                            else
                                            {
                                                Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, " Save_PatientFormDoc_Live_To_Local : false");

                                            }

                                        }
                                        else
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_Patient_Document, Utility._EHRLogdirectory_EHR_Patient_Document, " WriteByteArrayToPdf : failed");
                                        }
                                    }
                                    //IsDocUpdated = true;
                                }
                            }
                            catch (Exception ex1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[PatientFormDocument Sync (Adit Server To Local Database)] for Id " + dr["PatientForm_Web_ID"].ToString() + " Error " + ex1.Message);
                            }
                            #endregion
                        }


                    }
                    IsDocUpdated = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientFormDocument Sync (Adit Server To Local Database)] : " + ex.Message);
                    IsDocUpdated = true;
                }
            }

            #endregion
        }
        private void GetPatientDocument_New(string Service_Install_Id, string strPatientFormID = "")
        {
            #region SavePatientDoc
            if (IsDocUpdated)
            {
                string content = "";
                try
                {
                    IsDocUpdated = false;
                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPendingPatientFormDocAttachmentData(Service_Install_Id, strPatientFormID);
                    DataTable dtLivePatientForm = dtLocalPatientForm.DefaultView.ToTable(true, "PatientForm_Web_ID", "Patient_EHR_ID", "Patient_Web_ID", "Clinic_Number", "Service_Install_Id", "folder_name", "folder_ehr_id", "DocNameFormat", "Form_Name", "Patient_Name");
                    DataTable dtLocalPatientFormDoc = SynchLocalBAL.GetLocalPatientFormDocAttachmentData(Service_Install_Id, strPatientFormID);
                    DataTable dtLivePatientFormDoc = dtLocalPatientFormDoc.Clone();
                    dtLivePatientFormDoc.Columns.Add("InsUptDlt", typeof(int));
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        foreach (DataRow dr in dtLivePatientForm.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                        {
                            bool statusDoc = false;
                            string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("patientformdocattachment_list", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                            //PatientDoc PatientpdfDoc = new PatientDoc
                            //{                                   
                            //    sid = dr["PatientForm_Web_ID"].ToString().Trim(),
                            //};
                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            // string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                            //MessageBox.Show(jsonString);
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var clientdoc = new RestClient(strApiPatientForm);
                            var requestdoc = new RestRequest(Method.POST);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            requestdoc.AddHeader("Authorization", Utility.WebAdminUserToken);
                            requestdoc.AddHeader("cache-control", "no-cache");
                            requestdoc.AddHeader("Content-Type", "application/json");
                            requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            requestdoc.AddParameter("application/json", "{\"submissionId\": \"" + dr["PatientForm_Web_ID"].ToString().Trim() + "\"}", ParameterType.RequestBody);
                            //MessageBox.Show("application // json" + jsonString);
                            //MessageBox.Show("PatientDocument_Request (Patient_EHR_ID = " + RowPatientFormDoc["Patient_EHR_ID"] + ") Called " + strApiPatientForm.ToString());
                            IRestResponse response = clientdoc.Execute(requestdoc);
                            if (response.ErrorMessage == null)
                            {

                                content = response.Content.ToString();
                                ObjGoalBase.WriteToErrorLogFile("PatientFormAttachment List Response " + content);
                                var patientFormDocResp = JsonConvert.DeserializeObject<PullPatientFormDocResponse>(response.Content);
                                if (patientFormDocResp.data != null)
                                {
                                    #region
                                    try
                                    {
                                        foreach (PullPatientFormDoc doc in patientFormDocResp.data.fileList)
                                        {
                                            string PatientDoc_Web_ID = doc.name.Split('.')[0];
                                            DataRow[] row = dtLocalPatientFormDoc.Copy().Select("PatientDoc_Web_ID = '" + PatientDoc_Web_ID.ToString().Trim() + "'");
                                            bool Is_EHR_updated = true;
                                            if (row.Count() <= 0)
                                            {
                                                Is_EHR_updated = false;
                                                if (File.Exists(CommonUtility.GetAditDocTempPath() + "\\" + doc.name))
                                                {
                                                    File.Delete(CommonUtility.GetAditDocTempPath() + "\\" + doc.name);
                                                }
                                                dtLivePatientFormDoc.Rows.Clear();
                                                DataRow RowPatientFormDoc = dtLivePatientFormDoc.NewRow();
                                                RowPatientFormDoc["PatientDoc_Web_ID"] = PatientDoc_Web_ID.ToString().Trim();
                                                RowPatientFormDoc["InsUptDlt"] = 1;
                                                RowPatientFormDoc["Patient_EHR_ID"] = dr["Patient_EHR_ID"].ToString().Trim();
                                                RowPatientFormDoc["Patient_Web_ID"] = dr["Patient_Web_ID"].ToString().Trim();
                                                if (dr["folder_ehr_id"] == null)
                                                {
                                                    RowPatientFormDoc["folder_ehr_id"] = "";// string.IsNullOrEmpty(dr["folder_ehr_id"].ToString()) ? 0 : Convert.ToInt32(dr["folder_ehr_id"]);
                                                }
                                                else
                                                {
                                                    RowPatientFormDoc["folder_ehr_id"] = string.IsNullOrEmpty(dr["folder_ehr_id"].ToString()) ? 0 : Convert.ToInt32(dr["folder_ehr_id"]);
                                                }
                                                RowPatientFormDoc["folder_name"] = dr["folder_name"].ToString().Trim();
                                                RowPatientFormDoc["DocNameFormat"] = "FN-PN-DS";
                                                RowPatientFormDoc["PatientDoc_Name"] = doc.name;
                                                RowPatientFormDoc["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                RowPatientFormDoc["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                RowPatientFormDoc["Form_Name"] = doc.fieldName.ToString().Replace("\\", "_");
                                                RowPatientFormDoc["Patient_Name"] = dr["Patient_Name"].ToString();
                                                RowPatientFormDoc["DocType"] = doc.fileExtension;
                                                RowPatientFormDoc["Web_DocName"] = doc.fileName.ToString().Replace("\\", "_");
                                                RowPatientFormDoc["PatientForm_web_Id"] = dr["PatientForm_web_Id"].ToString();

                                                //rooja 11-4-23
                                                dtLivePatientFormDoc.Rows.Add(RowPatientFormDoc);
                                                dtLivePatientFormDoc.AcceptChanges();
                                            }
                                            else
                                            {
                                                if (!Convert.ToBoolean(row[0]["Is_EHR_Updated"]))
                                                {
                                                    Is_EHR_updated = false;
                                                }
                                            }
                                            if (!Is_EHR_updated)
                                            {
                                                strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("patientformdocattachment", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());

                                                //PatientDoc PatientpdfDoc = new PatientDoc
                                                //{
                                                //    locationId = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                                //    sid = dr["PatientForm_Web_ID"].ToString().Trim(),
                                                //};
                                                javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                                string jsonString = javaScriptSerializer.Serialize(doc);
                                                //MessageBox.Show(jsonString);
                                                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                                clientdoc = new RestClient(strApiPatientForm);
                                                requestdoc = new RestRequest(Method.POST);
                                                ServicePointManager.Expect100Continue = true;
                                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                                requestdoc.AddHeader("Authorization", Utility.WebAdminUserToken);
                                                requestdoc.AddHeader("cache-control", "no-cache");
                                                requestdoc.AddHeader("Content-Type", "application/json");
                                                requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                                requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                                //MessageBox.Show("application // json" + jsonString);
                                                //MessageBox.Show("PatientDocument_Request (Patient_EHR_ID = " + RowPatientFormDoc["Patient_EHR_ID"] + ") Called " + strApiPatientForm.ToString());
                                                response = clientdoc.Execute(requestdoc);
                                                if (response.ErrorMessage != null)
                                                {
                                                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                                    {
                                                        ObjGoalBase.WriteToErrorLogFile("[PatientFormDocumentAttachments_resp_Sync_Error : " + response.ErrorMessage);
                                                    }
                                                    else
                                                    {
                                                        ObjGoalBase.WriteToErrorLogFile("[PatientFormDocumentAttachments_Resp Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + response.ErrorMessage);
                                                    }
                                                    IsDocUpdated = true;
                                                    return;
                                                }
                                                //MessageBox.Show("PatientDocument_Request Response Received." + response.Content.ToString());// + response.Content.ToString());
                                                var jObject = JObject.Parse(response.Content);
                                                if (jObject != null)
                                                {
                                                    string Data = jObject.GetValue("data").ToString();
                                                    if (Data != null)
                                                    {
                                                        var jdocobject = JObject.Parse(Data);
                                                        string DocData = jdocobject.GetValue("buffer").ToString();
                                                        if (DocData != null)
                                                        {
                                                            if (DocData == "")
                                                            {
                                                                ObjGoalBase.WriteToErrorLogFile("[PatientFormDocumentAttachments_resp_Sync_Error : buffer data getting Blank : Json is " + jsonString.ToString());
                                                                continue;
                                                            }
                                                            DocData = DocData.Replace("data:application/pdf;base64,", String.Empty);
                                                            // Document. 
                                                            bool Docstatus = WriteByteArrayToPdf(DocData, CommonUtility.GetAditDocTempPath(), doc.name);
                                                            if (Docstatus)
                                                            {
                                                                if (row.Count() <= 0)
                                                                {
                                                                    statusDoc = PullLiveDatabaseBAL.Save_PatientFormDocAttachment_Live_To_Local(dtLivePatientFormDoc);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ObjGoalBase.WriteToErrorLogFile("[PatientFormDocumentAttachments_resp_Sync_Error : " + response.Content.ToString());
                                                    }
                                                }
                                            }
                                            //IsDocUpdated = true;
                                        }
                                        statusDoc = true;
                                    }
                                    catch (Exception ex1)
                                    {
                                        statusDoc = false;
                                        ObjGoalBase.WriteToErrorLogFile("[PatientFormDocumentAttachments Sync (Adit Server To Local Database)] for Id " + dr["PatientForm_Web_ID"].ToString() + " Error " + ex1.Message);
                                    }
                                    if (statusDoc)
                                    {
                                        PullLiveDatabaseBAL.Update_PatientFormDocAttachment_Live_To_Local(dr["PatientForm_Web_ID"].ToString().Trim(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                    }
                                }
                            }
                            #endregion
                        }


                    }
                    IsDocUpdated = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientFormDocument Sync (Adit Server To Local Database)] : " + ex.Message + ", Response : " + content);
                    IsDocUpdated = true;
                }
            }

            #endregion
        }

        public bool WriteByteArrayToPdf(string inPDFByteArrayStream, string pdflocation, string fileName)
        {
            try
            {
                byte[] data = Convert.FromBase64String(inPDFByteArrayStream);
                if (!Directory.Exists(pdflocation))
                {
                    Directory.CreateDirectory(pdflocation);
                }
                pdflocation = pdflocation + "\\" + fileName;
                using (FileStream Writer = new System.IO.FileStream(pdflocation, FileMode.Create, FileAccess.Write))
                {
                    Writer.Write(data, 0, data.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[PatientFormDocument Sync (Adit Server To Local Database)] : " + ex.Message);
                return false;
            }
        }

        private static void IndertDefaultRowForFirstNameLastNameMobile(string patientFormWebId, string ehrFieldName, ref DataTable dtLivePatientForm, string Clinic_Number, string Service_Install_Id, int folder_ehr_id, string folder_name, string DocNameFormat, string Form_Name, string Patient_Name, DateTime submit_time)
        {
            try
            {
                DataRow drNew = dtLivePatientForm.NewRow();
                drNew["PatientForm_Web_ID"] = patientFormWebId.ToString();
                drNew["ehrfield"] = ehrFieldName;
                drNew["ehrfield_value"] = ehrFieldName.ToString().ToUpper() == "MOBILE" ? "0000000000" : "NA";
                drNew["Clinic_Number"] = Clinic_Number;
                drNew["Service_Install_Id"] = Service_Install_Id;
                drNew["folder_ehr_id"] = folder_ehr_id;
                drNew["Form_Name"] = Form_Name;
                drNew["DocNameFormat"] = DocNameFormat;
                drNew["folder_name"] = folder_name;
                drNew["Patient_Name"] = Patient_Name;
                drNew["submit_time"] = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(submit_time.ToString()));
                dtLivePatientForm.Rows.Add(drNew);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void InsertPatientInsuranceColumns(string patientFormWebId, string ehrFieldName, string ehrvalue, ref DataTable dtLivePatientForm, string Clinic_Number, string Service_Install_Id, int folder_ehr_id, string folder_name, string DocNameFormat, string Form_Name, string Patient_Name, DateTime submit_time)
        {
            try
            {
                DataRow drNew = dtLivePatientForm.NewRow();
                drNew["PatientForm_Web_ID"] = patientFormWebId.ToString();
                drNew["ehrfield"] = ehrFieldName;
                drNew["ehrfield_value"] = ehrvalue;
                drNew["Clinic_Number"] = Clinic_Number;
                drNew["Service_Install_Id"] = Service_Install_Id;
                drNew["folder_ehr_id"] = folder_ehr_id;
                drNew["Form_Name"] = Form_Name;
                drNew["DocNameFormat"] = DocNameFormat;
                drNew["folder_name"] = folder_name;
                drNew["Patient_Name"] = Patient_Name;
                drNew["submit_time"] = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(submit_time.ToString()));
                dtLivePatientForm.Rows.Add(drNew);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void SetPrimarySecondaryProviderId(string providerFirstName, DataTable dtLocalProviderData, string setProviderName, string patientFormWebId, ref DataTable dtLivePatientForm)
        {
            try
            {
                string providerLastName = "";
                bool isSpace = false;

                if (Utility.Application_ID == 11)
                {

                }
                else
                {
                    if (providerFirstName.Contains(" "))
                    {
                        if (Utility.Application_ID != 5)
                        {
                            providerLastName = providerFirstName.Substring(providerFirstName.IndexOf(" "), providerFirstName.Length - providerFirstName.IndexOf(" ")).Trim().ToString().ToUpper();
                            providerFirstName = providerFirstName.Substring(0, providerFirstName.IndexOf(" ")).Trim().ToUpper();
                            isSpace = true;
                        }

                    }
                    else
                    {

                    }
                }



                if (Utility.Application_ID == 11)
                {
                    var primaryProviderResult = dtLocalProviderData.AsEnumerable().Where(c => c.Field<string>("First_Name").ToUpper() == providerFirstName.ToUpper().Trim());
                    if (primaryProviderResult.Count() > 0)
                    {
                        dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == patientFormWebId.ToString() && a.Field<string>("ehrfield").ToUpper() == setProviderName.ToUpper())
                           .All(d =>
                           {
                               d["ehrfield_value"] = primaryProviderResult.Select(a => a.Field<string>("Provider_EHR_ID")).First().ToString();
                               return true;
                           });
                    }
                    else
                    {
                        dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == patientFormWebId.ToString() && a.Field<string>("ehrfield").ToUpper() == setProviderName.ToUpper())
                        .All(d =>
                        {
                            d["ehrfield_value"] = "";
                            return true;
                        });
                    }
                }
                else
                {
                    var primaryProviderResult = dtLocalProviderData.AsEnumerable().Where(c => (isSpace == true ? (c.Field<string>("First_Name").ToUpper() + " " + c.Field<string>("Last_Name").ToUpper() == providerFirstName.ToUpper() + " " + providerLastName.ToUpper()) : (c.Field<string>("First_Name").ToUpper() == providerFirstName)));
                    if (primaryProviderResult.Count() > 0)
                    {
                        dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == patientFormWebId.ToString() && a.Field<string>("ehrfield").ToUpper() == setProviderName.ToUpper())
                           .All(d =>
                           {
                               d["ehrfield_value"] = primaryProviderResult.Select(a => a.Field<string>("Provider_EHR_ID")).First().ToString();
                               return true;
                           });
                    }
                    else
                    {
                        dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == patientFormWebId.ToString() && a.Field<string>("ehrfield").ToUpper() == setProviderName.ToUpper())
                        .All(d =>
                        {
                            d["ehrfield_value"] = "";
                            return true;
                        });
                    }
                }



                //    var primaryProviderResult = dtLocalProviderData.AsEnumerable().Where(c => (isSpace == true ? (c.Field<string>("First_Name").ToUpper() == providerFirstName && c.Field<string>("Last_Name").ToUpper() == providerLastName.ToUpper()) : (c.Field<string>("First_Name").ToUpper() == providerFirstName)));
                //    if (primaryProviderResult.Count() > 0)
                //    {
                //        dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == patientFormWebId.ToString() && a.Field<string>("ehrfield").ToUpper() == setProviderName.ToUpper())
                //           .All(d =>
                //           {
                //               d["ehrfield_value"] = primaryProviderResult.Select(a => a.Field<string>("Provider_EHR_ID")).First().ToString();
                //               return true;
                //           });
                //    }
                //    else
                //    {
                //        dtLivePatientForm.AsEnumerable().Where(a => a.Field<string>("PatientForm_Web_ID") == patientFormWebId.ToString() && a.Field<string>("ehrfield").ToUpper() == setProviderName.ToUpper())
                //        .All(d =>
                //        {
                //            d["ehrfield_value"] = "";
                //            return true;
                //        });
                //    }


            }
            catch (Exception)
            {
                throw;
            }
        }

        bool IsMedicalHistoryFetched = true;

        public void GetMedicalHistoryRecords(string Service_Install_ID, string strPatientFormID = "")
        {
            #region SavePatientDoc
            if (IsMedicalHistoryFetched)
            {
                try
                {
                    IsMedicalHistoryFetched = false;
                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPendingPatientFormMedicalHistory(Service_Install_ID, strPatientFormID);
                    DataTable dtLivePatientForm = dtLocalPatientForm.DefaultView.ToTable(true, "PatientForm_Web_ID", "Patient_EHR_ID", "Patient_Web_ID", "MedicalHistorySubmission_Id", "Clinic_Number", "Service_Install_Id");
                    DataTable dtMedicalHistory_Question_Submit = SynchLocalBAL.GetLocalMedicalHistoryRecords("EagleSoft_MHF_Question_Submit", Service_Install_ID, true, strPatientFormID);
                    DataTable dtMedicalHistory_Answer_Submit = SynchLocalBAL.GetLocalMedicalHistoryRecords("EagleSoft_MHF_Answer_Submit", Service_Install_ID, true, strPatientFormID);
                    dtMedicalHistory_Question_Submit.Rows.Clear();
                    dtMedicalHistory_Answer_Submit.Rows.Clear();
                    dtMedicalHistory_Question_Submit.Columns.Add("InsUptDlt", typeof(int));
                    dtMedicalHistory_Answer_Submit.Columns.Add("InsUptDlt", typeof(int));

                    #region

                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        foreach (DataRow dr in dtLivePatientForm.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                        {
                            try
                            {
                                string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("getEagleSoftEHRForms", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());

                                PatientMedicalHistoryBO PatientpdfDoc = new PatientMedicalHistoryBO
                                {
                                    loc = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(),//Utility.Loc_ID,
                                    org = Utility.Organization_ID,
                                    id = dr["PatientForm_Web_ID"].ToString().Trim(),
                                };
                                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                var clientdoc = new RestClient(strApiPatientForm);
                                var requestdoc = new RestRequest(Method.POST);
                                ServicePointManager.Expect100Continue = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                requestdoc.AddHeader("Content-Type", "application/json");
                                requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_ID"].ToString()));
                                IRestResponse response = clientdoc.Execute(requestdoc);
                                if (response.ErrorMessage != null)
                                {
                                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                    {
                                        ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory_Err : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                    }
                                    else
                                    {
                                        ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                    }
                                    IsMedicalHistoryFetched = true;
                                    return;
                                }

                                var ApptPatientFormDto = JsonConvert.DeserializeObject<Pull_PatientInfo_MedicalHistoryBO>(response.Content);
                                if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                                {
                                    dr["MedicalHistorySubmission_Id"] = ApptPatientFormDto.data._id;
                                    if (ApptPatientFormDto.data.Question != null)
                                    {
                                        foreach (var item in ApptPatientFormDto.data.Question)
                                        {
                                            DataRow drNew = dtMedicalHistory_Question_Submit.NewRow();
                                            drNew["EagleSoft_MHF_Question_Submit_WEB_Id"] = item._id;
                                            drNew["PatientForm_Web_ID"] = dr["PatientForm_Web_ID"].ToString();
                                            drNew["SectionItem_EHR_Id"] = item.sectionitemmaster_ehr_id.ToString();
                                            drNew["FormMaster_EHR_id"] = item.formmaster_ehr_id;
                                            drNew["SectionItemName"] = item.name;
                                            drNew["Section_EHR_Id"] = item.sectionmaster_ehr_id;
                                            drNew["AllowComment"] = item.allowcomment;
                                            drNew["AlertOnYes"] = item.alertonyes;
                                            drNew["AlertOnNo"] = item.alertonno;
                                            drNew["Alert_EHR_Id"] = item.alertmaster_ehr_id;
                                            drNew["Question_Type"] = item.question_type;
                                            drNew["Answer_Type"] = item.answer_type;
                                            drNew["AnsValue"] = item.AnsValue;
                                            drNew["AnsKey"] = item.AnsKey;
                                            drNew["AllowEditComment"] = item.alloweditcomment;
                                            drNew["NumberOfColumns"] = item.numberofcolumns;
                                            drNew["Entry_DateTime"] = System.DateTime.Now;
                                            drNew["Last_Sync_Date"] = System.DateTime.Now;
                                            drNew["Is_EHR_Updated"] = 0;
                                            drNew["InsUptDlt"] = 1;
                                            drNew["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                            drNew["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                            dtMedicalHistory_Question_Submit.Rows.Add(drNew);

                                            foreach (var item1 in item.sectionItemOptionMaster)
                                            {
                                                DataRow drAns = dtMedicalHistory_Answer_Submit.NewRow();
                                                drAns["EagleSoft_MHF_Answer_Submit_WEB_Id"] = item1._id;
                                                drAns["EagleSoft_MHF_Question_Submit_WEB_Id"] = item._id;
                                                drAns["SectionItemOptionMaster_EHR_Id"] = item1.sectionitemoptionmaster_ehr_id;
                                                drAns["PatientForm_Web_ID"] = dr["PatientForm_Web_ID"].ToString();
                                                drAns["SectionItem_EHR_Id"] = item1.sectionitemmaster_ehr_id.ToString();
                                                drAns["SectionItemOptionName"] = item1.name.ToString();
                                                drAns["FormMaster_EHR_id"] = item1.formmaster_ehr_id;
                                                drAns["Question_Type"] = item1.question_type;
                                                drAns["Answer_Type"] = item1.answer_type;
                                                if (item1.AnsKey != null && item1.AnsKey.ToString() != string.Empty && item1.AnsKey.Substring(item1.AnsKey.IndexOf('_') + 1, 1).ToString().ToUpper() == "Y")
                                                {
                                                    drAns["AnsKey"] = "Yes";
                                                }
                                                else if (item1.AnsKey != null && item1.AnsKey.ToString() != string.Empty && item1.AnsKey.Substring(item1.AnsKey.IndexOf('_') + 1, 1).ToString().ToUpper() == "N")
                                                {
                                                    drAns["AnsKey"] = "No";
                                                }
                                                else
                                                {
                                                    drAns["AnsKey"] = item1.AnsKey;
                                                }

                                                drAns["Section_EHR_Id"] = item1.sectionmaster_ehr_id;
                                                drAns["Is_EHR_Updated"] = 0;
                                                drAns["InsUptDlt"] = 1;
                                                drAns["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                drAns["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                dtMedicalHistory_Answer_Submit.Rows.Add(drAns);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + ex1.Message + "_" + dr["PatientForm_Web_ID"].ToString());
                            }
                        }
                    }

                    SynchTrackerBAL.Save_Tracker_To_Local(dtMedicalHistory_Question_Submit, "EagleSoft_MHF_Question_Submit", "EagleSoft_MHF_Question_Submit_LocalDB_ID", "EagleSoft_MHF_Question_Submit_LocalDB_ID");
                    SynchTrackerBAL.Save_Tracker_To_Local(dtMedicalHistory_Answer_Submit, "EagleSoft_MHF_Answer_Submit", "EagleSoft_MHF_Answer_Submit_LocalDB_ID", "EagleSoft_MHF_Answer_Submit_LocalDB_ID");
                    SynchLocalBAL.UpdatePatientFormMedicalHistory_Fields(dtLivePatientForm);
                    #endregion
                    IsMedicalHistoryFetched = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] : " + ex.Message);
                    IsMedicalHistoryFetched = true;
                }
            }

            #endregion
        }

        public void GetMedicalDentrixHistoryRecords(string strPatientFormID = "")
        {
            #region SavePatientMedicleHistory

            if (IsMedicalHistoryFetched)
            {
                try
                {
                    IsMedicalHistoryFetched = false;
                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPendingPatientFormMedicalHistory("1", strPatientFormID);
                    DataTable dtLivePatientForm = dtLocalPatientForm.DefaultView.ToTable(true, "PatientForm_Web_ID", "Patient_EHR_ID", "Patient_Web_ID", "MedicalHistorySubmission_Id", "Clinic_Number", "Service_Install_Id");
                    DataTable dtMedicalHistory_Response_Submit = SynchLocalBAL.GetLocalMedicalHistoryRecords("Dentrix_Response", "1", true, strPatientFormID);
                    dtMedicalHistory_Response_Submit.Rows.Clear();
                    dtMedicalHistory_Response_Submit.Columns.Add("InsUptDlt", typeof(int));
                    #region

                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            foreach (DataRow dr in dtLivePatientForm.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' "))
                            {

                                try
                                {
                                    string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("getEagleSoftEHRForms", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                                    PatientMedicalHistoryBO PatientpdfDoc = new PatientMedicalHistoryBO
                                    {
                                        //    loc = "a2f1b7d2-a529-4860-a53a-6c5772290d73",
                                        //    org = "462a2c0a-cb0b-4727-bfca-f172b384a04c",
                                        //    id = "03d1f1e4-f8ac-4212-bc39-500a63486b13",

                                        loc = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        org = Utility.Organization_ID,
                                        id = dr["PatientForm_Web_ID"].ToString().Trim(),
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                    var clientdoc = new RestClient(strApiPatientForm);
                                    var requestdoc = new RestRequest(Method.POST);
                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                    requestdoc.AddHeader("Content-Type", "application/json");
                                    requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                    requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                    IRestResponse response = clientdoc.Execute(requestdoc);
                                    if (response.ErrorMessage != null)
                                    {
                                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory_Err : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + response.ErrorMessage);
                                        }
                                        IsMedicalHistoryFetched = true;
                                        return;
                                    }

                                    var ApptPatientFormDto = JsonConvert.DeserializeObject<Pull_PatientInfo_MedicalHistoryBO>(response.Content);
                                    if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                                    {
                                        dr["MedicalHistorySubmission_Id"] = ApptPatientFormDto.data._id;
                                        List<Question> AllResponse = new List<Question>();
                                        AllResponse = ApptPatientFormDto.data.Question.Where(x => x._type == "dentrixformquestion").ToList();
                                        foreach (var item in AllResponse)
                                        {
                                            DataRow drNew = dtMedicalHistory_Response_Submit.NewRow();
                                            drNew["Dentrix_Response_Web_ID"] = item._id;
                                            drNew["PatientForm_Web_ID"] = dr["PatientForm_Web_ID"].ToString();
                                            drNew["Dentrix_Response_EHR_ID"] = "";
                                            drNew["Dentrix_Form_EHRUnique_ID"] = item.dentrix_form_ehrunique_id;
                                            drNew["Patient_EHR_ID"] = dr["Patient_EHR_ID"].ToString();
                                            drNew["Dentrix_Question_EHR_ID"] = item.dentrix_formquestion_ehr_id;
                                            drNew["Dentrix_Question_EHRUnique_ID"] = item.dentrix_formquestion_ehrunique_id;
                                            drNew["Dentrix_QuestionsTypeId"] = item.questionstypeid;
                                            drNew["Dentrix_ResponsetypeId"] = item.responsetypeid;
                                            drNew["Entry_DateTime"] = System.DateTime.Now;
                                            // drNew["Last_Sync_Date"] = System.DateTime.Now;
                                            drNew["Is_EHR_Updated"] = 0;
                                            drNew["InsUptDlt"] = 1;
                                            drNew["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                            drNew["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                            drNew["InputType"] = item.inputtype;

                                            if (item.answer_value is string)
                                            {
                                                try
                                                {
                                                    if (item.inputtype == "RadioButton" || item.inputtype == "Confirmation")
                                                    {
                                                        //  drNew["Answer_Value"] = item.answer_value.ToString().Substring(item.answer_value.ToString().IndexOf('_') + 1, 1);
                                                        if (item.inputtype == "RadioButton" && Convert.ToString(item.answer_value.ToString()).Split('_')[1] == "2")
                                                        {
                                                            drNew["Answer_Value"] = 0;
                                                        }
                                                        else
                                                        {
                                                            drNew["Answer_Value"] = Convert.ToString(item.answer_value.ToString()).Split('_')[1];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        drNew["Answer_Value"] = Convert.ToString(item.answer_value);
                                                    }
                                                }
                                                catch
                                                {
                                                    drNew["Answer_Value"] = "";
                                                }
                                            }
                                            else if (item.inputtype == "CheckBox")
                                            {
                                                try
                                                {
                                                    string Answer_value = "";
                                                    foreach (var item1 in item.answer_value)
                                                    {
                                                        Answer_value = Answer_value.ToString() + Convert.ToString(item1.ToString()).Split('_')[1] + ",";
                                                    }
                                                    Answer_value = Answer_value.TrimEnd(',');
                                                    drNew["Answer_Value"] = Convert.ToString(Answer_value);
                                                }
                                                catch
                                                {
                                                    drNew["Answer_Value"] = "";
                                                }
                                            }

                                            dtMedicalHistory_Response_Submit.Rows.Add(drNew);
                                        }
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + ex1.Message + "_" + dr["PatientForm_Web_ID"].ToString());
                                }
                            }

                            SynchTrackerBAL.Save_Tracker_To_Local(dtMedicalHistory_Response_Submit, "Dentrix_Response", "Dentrix_Response_LocalDB_ID", "Dentrix_Response_LocalDB_ID");
                            SynchLocalBAL.UpdatePatientFormMedicalHistory_Fields(dtLivePatientForm);
                            #endregion
                            IsMedicalHistoryFetched = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] : " + ex.Message);
                    IsMedicalHistoryFetched = true;
                }
            }

            #endregion
        }

        public void GetMedicalAbelDentHistoryRecords()
        {
            #region SavePatientMedicleHistory

            if (IsMedicalHistoryFetched)
            {
                try
                {
                    IsMedicalHistoryFetched = false;
                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPendingPatientFormMedicalHistory("1");
                    DataTable dtLivePatientForm = dtLocalPatientForm.DefaultView.ToTable(true, "PatientForm_Web_ID", "Patient_EHR_ID", "Patient_Web_ID", "MedicalHistorySubmission_Id", "Clinic_Number", "Service_Install_Id");
                    DataTable dtMedicalHistory_Response_Submit = SynchLocalBAL.GetLocalMedicalHistoryRecords("AbelDent_Response", "1", true);
                    dtMedicalHistory_Response_Submit.Rows.Clear();
                    dtMedicalHistory_Response_Submit.Columns.Add("InsUptDlt", typeof(int));
                    #region

                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            foreach (DataRow dr in dtLivePatientForm.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' "))
                            {

                                try
                                {
                                    string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("getEagleSoftEHRForms", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                                    PatientMedicalHistoryBO PatientpdfDoc = new PatientMedicalHistoryBO
                                    {
                                        //    loc = "a2f1b7d2-a529-4860-a53a-6c5772290d73",
                                        //    org = "462a2c0a-cb0b-4727-bfca-f172b384a04c",
                                        //    id = "03d1f1e4-f8ac-4212-bc39-500a63486b13",

                                        loc = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        org = Utility.Organization_ID,
                                        id = dr["PatientForm_Web_ID"].ToString().Trim(),
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                    var clientdoc = new RestClient(strApiPatientForm);
                                    var requestdoc = new RestRequest(Method.POST);
                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                    requestdoc.AddHeader("Content-Type", "application/json");
                                    requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                    requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                    IRestResponse response = clientdoc.Execute(requestdoc);
                                    if (response.ErrorMessage != null)
                                    {
                                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory_Err : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + response.ErrorMessage);
                                        }
                                        IsMedicalHistoryFetched = true;
                                        return;
                                    }

                                    var ApptPatientFormDto = JsonConvert.DeserializeObject<Pull_PatientInfo_MedicalHistoryBO>(response.Content);
                                    if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                                    {
                                        dr["MedicalHistorySubmission_Id"] = ApptPatientFormDto.data._id;
                                        List<Question> AllResponse = new List<Question>();
                                        AllResponse = ApptPatientFormDto.data.Question.Where(x => x._type == "abeldentformquestion").ToList();
                                        foreach (var item in AllResponse)
                                        {
                                            DataRow drNew = dtMedicalHistory_Response_Submit.NewRow();
                                            drNew["AbelDent_Response_Web_ID"] = item._id;
                                            drNew["PatientForm_Web_ID"] = dr["PatientForm_Web_ID"].ToString();
                                            drNew["AbelDent_Response_EHR_ID"] = "";
                                            drNew["AbelDent_Form_EHRUnique_ID"] = item.abeldent_form_ehrunique_id;
                                            drNew["Patient_EHR_ID"] = dr["Patient_EHR_ID"].ToString();
                                            drNew["AbelDent_Question_EHR_ID"] = item.abeldent_formquestion_ehr_id;
                                            drNew["AbelDent_Question_EHRUnique_ID"] = item.abeldent_formquestion_ehrunique_id;
                                            drNew["AbelDent_QuestionsTypeId"] = item.questionstypeid;
                                            drNew["AbelDent_ResponsetypeId"] = item.responsetypeid;
                                            drNew["Entry_DateTime"] = System.DateTime.Now;
                                            // drNew["Last_Sync_Date"] = System.DateTime.Now;
                                            drNew["Is_EHR_Updated"] = 0;
                                            drNew["InsUptDlt"] = 1;
                                            drNew["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                            drNew["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();

                                            if (item.answer_value is string)
                                            {
                                                try
                                                {
                                                    if (item.inputtype.ToLower() == "multiplechoice")
                                                    {
                                                        if (item.inputtype.ToLower() == "multiplechoice" && Convert.ToString(item.answer_value.ToString()).Split('_')[1] == "2")
                                                        {
                                                            drNew["Answer_Value"] = 0;
                                                        }
                                                        else
                                                        {
                                                            drNew["Answer_Value"] = Convert.ToString(item.answer_value.ToString()).Split('_')[1];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        drNew["Answer_Value"] = Convert.ToString(item.answer_value);
                                                    }
                                                }
                                                catch
                                                {
                                                    drNew["Answer_Value"] = "";
                                                }
                                            }
                                            else if (item.inputtype.ToLower() == "checkbox")
                                            {
                                                try
                                                {
                                                    string Answer_value = "";
                                                    foreach (var item1 in item.answer_value)
                                                    {
                                                        Answer_value = Answer_value.ToString() + Convert.ToString(item1.ToString()).Split('_')[1] + ",";
                                                    }
                                                    Answer_value = Answer_value.TrimEnd(',');
                                                    drNew["Answer_Value"] = Convert.ToString(Answer_value);
                                                }
                                                catch
                                                {
                                                    drNew["Answer_Value"] = "";
                                                }
                                            }

                                            dtMedicalHistory_Response_Submit.Rows.Add(drNew);
                                        }
                                        dtMedicalHistory_Response_Submit.AcceptChanges();
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + ex1.Message + "_" + dr["PatientForm_Web_ID"].ToString());
                                }
                            }

                            SynchTrackerBAL.Save_Tracker_To_Local(dtMedicalHistory_Response_Submit, "AbelDent_Response", "AbelDent_Response_LocalDB_ID", "AbelDent_Response_LocalDB_ID");
                            SynchLocalBAL.UpdatePatientFormMedicalHistory_Fields(dtLivePatientForm);
                            #endregion
                            IsMedicalHistoryFetched = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] : " + ex.Message);
                    IsMedicalHistoryFetched = true;
                }
            }

            #endregion
        }

        #endregion

        #region Insurance Carrier Document

        public static void SynchDataLiveDB_Pull_InsuranceCarrierDoc()
        {
            try
            {
                Utility.WriteToSyncLogFile_All("Call InsuranceCarrier Doc PUll");
                if (!IS_EHRDocPulled)
                {
                    //Utility.WriteToSyncLogFile_All("EHR Pulled Condition Falsed");
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        //Utility.WriteToSyncLogFile_All("Start Location Loop");
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            //Utility.WriteToSyncLogFile_All("Adit App Sync is Enabled");
                            IS_EHRDocPulled = true;
                            string strApiInsuranceCarrierDocForm = PullLiveDatabaseBAL.GetLiveRecord("InsuranceCarrier_document", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                            string ab = Utility.WebAdminUserToken;
                            var client = new RestClient(strApiInsuranceCarrierDocForm);

                            PatientDoc PatientpdfDoc = new PatientDoc
                            {
                                locationId = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                //locationId = "69a1280c-e189-4985-99de-efdf20d2e04f"
                            };
                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Call InsuranceCarrier document  API");
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            //request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            GoalBase.WriteToPaymentLogFromAll_Static("InsuranceCarrier_Request Called " + strApiInsuranceCarrierDocForm.ToString());
                            Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Request Sent into the API " + " Authorization, TokenKey & action");
                            
                            IRestResponse response = client.Execute(request);
                            
                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync_ResponseError (Adit Server To Local Database)] : " + response.ErrorMessage);
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                IS_EHRDocPulled = false;
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Response received from API(" + response.Content.ToString() + ")");
                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Response is null");
                            }


                            Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "----------------------------Deserialize repsonse--------------------------------");

                            GoalBase.WriteToPaymentLogFromAll_Static("Response Received " + response.Content.ToString());
                            var AppInsuranceCarrierDocDto = JsonConvert.DeserializeObject<Pull_InsuranceCarrierDocBO>(response.Content);


                            string PatientName = "";
                            string SubmittedDate = "";
                            string Patient_EHR_ID = "0";
                            string Patient_Web_ID = "";
                            string InsuranceCarrierId = "";

                            string InsuranceCarrierDocName = "";
                            string InsuranceCarrierFolderName = "";
                            string Clinic_Number = "0";
                            string Service_Install_Id = "1";
                            bool FileCreated = false;
                            string filepath = "";

                            if (AppInsuranceCarrierDocDto != null && AppInsuranceCarrierDocDto.data != null)
                            {
                                if (AppInsuranceCarrierDocDto.data != null && AppInsuranceCarrierDocDto.data.Count() > 0)
                                {
                                    if (AppInsuranceCarrierDocDto.data.Count == 0)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Deserialize response AppInsuranceCarrierDocDto count :  (" + AppInsuranceCarrierDocDto.data.Count + ") no record ");
                                    }
                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Deserialize repsonse from API(" + AppInsuranceCarrierDocDto.message.ToString() + ")");

                                    int count = 0;
                                    foreach (var item in AppInsuranceCarrierDocDto.data)
                                    {
                                        //check for sync is done or not
                                        bool DocSynced =  SynchLocalBAL.Sync_check_for_InsuranceCarrier(item._id, Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document);

                                        if (DocSynced)
                                        {
                                            if (item.patientEhrId != null && item.patientEhrId != "")
                                            {
                                               
                                                 PatientName = item.patientFullName;
                                                 SubmittedDate = Convert.ToString(DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(item.submitted_at)));
                                                 Patient_EHR_ID = item.patientEhrId;
                                                 Patient_Web_ID = item.patientId;
                                                 InsuranceCarrierId = item._id;                                             
                                               
                                                 InsuranceCarrierDocName = item.pdfName+".pdf";
                                                 InsuranceCarrierFolderName = item.foldername;
                                                 Clinic_Number = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(); ;
                                                 Service_Install_Id = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();

                                                 filepath = CommonUtility.GetAditInsuranceCarrierDocTempPath() + "\\" + item.pdfName+".pdf";
                                                 FileCreated = false;
                                                GoalBase.WriteToErrorLogFile_Static("[InsuranceCarrierDoc Sync (Adit Server To Local Database)] : before PDF file copy from adit to local");

                                                Utility.DownloadFileWithProgress(item.pdffile, filepath);
                                                //clientdownload.DownloadFile(item.pdffile, filepath);
                                                GoalBase.WriteToErrorLogFile_Static("[InsuranceCarrierDoc Sync (Adit Server To Local Database)] : after download file PDF file copy from adit to local");
                                                if (File.Exists(filepath))
                                                {
                                                    FileCreated = true;
                                                }
                                                
                                                GoalBase.WriteToErrorLogFile_Static("[InsuranceCarrierDoc Sync (Adit Server To Local Database)] : after PDF file copy from adit to local");
                                                SynchLocalBAL.CreateRecordForInsuranceCarrierDoc(PatientName, SubmittedDate, Patient_EHR_ID, Patient_Web_ID, InsuranceCarrierId, InsuranceCarrierDocName, InsuranceCarrierFolderName, Clinic_Number, Service_Install_Id, FileCreated, Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document);

                                                if (Utility.Application_ID == 3)
                                                {
                                                    GoalBase.WriteToPaymentLogFromAll_Static("Call Funcation to save in EHR Dentrix " + InsuranceCarrierId.ToString());
                                                    SynchDentrixBAL.Save_InsuranceCarrier_Document_in_Dentrix(InsuranceCarrierId);
                                                    GoalBase.WriteToPaymentLogFromAll_Static("Call Funcation to save in EHR Dentrix Done " + InsuranceCarrierId.ToString());
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    GoalBase.WriteToSyncLogFile_Static("InsuranceCarrierDoc Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"] + " Pending Records Not found on Adit Server");
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Insurance_carrier_Doc_Pull");
                                    if (UpdateSync_Table_Datetime)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "UpdateSync_Table_Datetime  status : Success");
                                    }
                                    else
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_Patient_Portal, Utility._EHRLogdirectory_Patient_Portal, "UpdateSync_Table_Datetime  status : failed");
                                    }
                                }

                            }
                            else
                            {
                                Utility.WriteToSyncLogFile_All("InsuranceCarrierDoc_AditLocationSyncEnable False : " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                            }
                        }
                        IS_EHRDocPulled = false;
                    }
                }
                else
                {
                    Utility.WriteToSyncLogFile_All("InsuranceCarrierDoc_FirstCondition false : ProviderSyncFirstTime = " + IsProviderSyncedFirstTime.ToString() + ", EHR Doc pull : " + IS_EHRDocPulled.ToString());
                }
            }
            catch (Exception ex)
            {
                IS_EHRDocPulled = false;
                GoalBase.WriteToErrorLogFile_Static("[InsuranceCarrierDoc Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        public static void Change_Status_InsuranceCarrierDoc(DataTable Daata, string status)
        {
            try
            {
                Utility.WriteToSyncLogFile_All("Call InsuranceCarrier Doc PUll");
                if (!IS_EHRDocPulled)
                {
                    Utility.WriteToSyncLogFile_All("EHR Pulled Condition Falsed");
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        Utility.WriteToSyncLogFile_All("Start Location Loop");
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            Utility.WriteToSyncLogFile_All("Adit App Sync is Enabled");

                            List<string> docids = new List<string>();
                            foreach (DataRow dr in Daata.Rows)
                            {
                                docids.Add(dr["InsuranceCarrier_Doc_Web_ID"].ToString());
                            }

                            string strApiUpdateInsuranceCarrierDocStatus = PullLiveDatabaseBAL.UpdateInsuranceCarrierDocStatus("change_insurancecarrier_doc_status");
                            Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Call change_InsuranceCarrier_doc_status API");
                            string ab = Utility.WebAdminUserToken;
                            var client = new RestClient(strApiUpdateInsuranceCarrierDocStatus);

                            InsuranceCarrierDocStatusChange InsuranceCarrierDocStatusChange = new InsuranceCarrierDocStatusChange
                            {
                                _id = docids,                                
                                status = status.ToLower()
                            };
                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            string jsonString = javaScriptSerializer.Serialize(InsuranceCarrierDocStatusChange);
                            var request = new RestRequest(Method.POST);

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddParameter("application/json", " [ "+jsonString+" ] ", ParameterType.RequestBody);
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            request.AddHeader("action", "EHRPFIMPORT");
                            Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Request Sent into the API  " + " Authorization, TokenKey & action");
                            GoalBase.WriteToPaymentLogFromAll_Static("InsuranceCarrier_Request Called " + InsuranceCarrierDocStatusChange.ToString());
                            IRestResponse response = client.Execute(request);

                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Response received from API (" + response.Content.ToString() + ")");

                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document, "Response is null");
                            }

                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[InsuranceCarrierDoc Sync_ResponseError (Adit Server To Local Database)] : " + response.ErrorMessage);
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[InsuranceCarrierDoc Sync (Adit Server To Local Database)] Service Install Id  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  :" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                IS_EHRDocPulled = false;
                                return;
                            }

                            GoalBase.WriteToPaymentLogFromAll_Static("Response Received " + response.Content.ToString());


                            if (response.Content.ToString().Contains("Success"))
                            {
                                UpdateInsuranceCarrierDocumentAditUpdated(Daata, Utility._filename_EHR_InsuranceCarrier_document, Utility._EHRLogdirectory_EHR_InsuranceCarrier_document);
                                GoalBase.WriteToSyncLogFile_Static("[InsuranceCarrierDoc Sync_Response (Adit Server To Local Database)] : " + "Success");//_successfullstataus = "Success";
                            }
                            else
                            {
                                if (response.Content.ToString() == "")
                                {
                                    GoalBase.WriteToErrorLogFile_Static("Error:"+ response.ErrorMessage.ToString());
                                }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("Error:" + response.Content.ToString());                                 
                                }
                            }
                        }
                        else
                        {
                            Utility.WriteToSyncLogFile_All("InsuranceCarrierDoc_AditLocationSyncEnable False : " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                        }
                    }
                }
                else
                {
                    Utility.WriteToSyncLogFile_All("InsuranceCarrierDoc_FirstCondition false : ProviderSyncFirstTime = " + IsProviderSyncedFirstTime.ToString() + ", EHR Doc pull : " + IS_EHRDocPulled.ToString());
                }
            }
            catch (Exception ex)
            {
                IS_EHRDocPulled = false;
                GoalBase.WriteToErrorLogFile_Static("[InsuranceCarrierDoc Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        private static void UpdateInsuranceCarrierDocumentAditUpdated(DataTable dt, string _filename_EHR_InsuranceCarrier_document = "", string _EHRLogdirectory_EHR_InsuranceCarrier_document = "")
        {
            foreach (DataRow dr in dt.Rows)
            {
                SynchLocalBAL.UpdateInsuranceCarrierDocInlocal(dr["InsuranceCarrier_Doc_Web_ID"].ToString(), _filename_EHR_InsuranceCarrier_document, _EHRLogdirectory_EHR_InsuranceCarrier_document);
            }
        }
       
        #endregion

        public void GetMedicalClearDentHistoryRecords(string strPatientFormID = "")
        {
            #region SavePatientMedicleHistory

            if (IsMedicalHistoryFetched) //&& Utility.AditLocationSyncEnable
            {
                try
                {
                    IsMedicalHistoryFetched = false;
                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPendingPatientFormMedicalHistory("1", strPatientFormID);
                    DataTable dtLivePatientForm = dtLocalPatientForm.DefaultView.ToTable(true, "PatientForm_Web_ID", "Patient_EHR_ID", "Patient_Web_ID", "MedicalHistorySubmission_Id", "Clinic_Number", "Service_Install_Id");
                    DataTable dtMedicalHistory_Response_Submit = SynchLocalBAL.GetLocalMedicalHistoryRecords("CD_Response", "1", true, strPatientFormID);
                    dtMedicalHistory_Response_Submit.Rows.Clear();
                    dtMedicalHistory_Response_Submit.Columns.Add("InsUptDlt", typeof(int));
                    #region

                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            foreach (DataRow dr in dtLivePatientForm.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' "))
                            {
                                try
                                {
                                    string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("getEagleSoftEHRForms", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                                    PatientMedicalHistoryBO PatientpdfDoc = new PatientMedicalHistoryBO
                                    {
                                        //    loc = "a2f1b7d2-a529-4860-a53a-6c5772290d73",
                                        //    org = "462a2c0a-cb0b-4727-bfca-f172b384a04c",
                                        //    id = "03d1f1e4-f8ac-4212-bc39-500a63486b13",

                                        loc = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        org = Utility.Organization_ID,
                                        id = dr["PatientForm_Web_ID"].ToString().Trim(),
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                    var clientdoc = new RestClient(strApiPatientForm);
                                    var requestdoc = new RestRequest(Method.POST);
                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                    requestdoc.AddHeader("Content-Type", "application/json");
                                    requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                    requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                                    IRestResponse response = clientdoc.Execute(requestdoc);
                                    if (response.ErrorMessage != null)
                                    {
                                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory_Err : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                        }
                                        IsMedicalHistoryFetched = true;
                                        return;
                                    }

                                    var ApptPatientFormDto = JsonConvert.DeserializeObject<Pull_PatientInfo_MedicalHistoryBO>(response.Content);
                                    if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                                    {
                                        dr["MedicalHistorySubmission_Id"] = ApptPatientFormDto.data._id;
                                        List<Question> AllResponse = new List<Question>();
                                        AllResponse = ApptPatientFormDto.data.Question.Where(x => x._type == "cleardentquestionmaster").ToList();
                                        foreach (var item in AllResponse)
                                        {
                                            DataRow drNew = dtMedicalHistory_Response_Submit.NewRow();
                                            drNew["CD_Response_Web_ID"] = item._id;
                                            drNew["PatientForm_Web_ID"] = dr["PatientForm_Web_ID"].ToString();
                                            drNew["CD_intPatMedId"] = "";
                                            drNew["CD_FormMaster_EHR_ID"] = item.cd_formmaster_ehr_id;
                                            drNew["Patient_EHR_ID"] = dr["Patient_EHR_ID"].ToString();
                                            drNew["FormName_Name"] = item.cd_form_name;
                                            drNew["CD_QuestionMaster_EHR_ID"] = item.cd_questionmaster_ehr_id;
                                            drNew["Question_Description"] = item.question_description;
                                            drNew["Question_Sequence"] = item.question_sequence;
                                            drNew["Question_Warnings"] = item.question_warnings;
                                            drNew["Answer_Value"] = item.comment;
                                            drNew["Entry_DateTime"] = System.DateTime.Now;
                                            // drNew["Last_Sync_Date"] = System.DateTime.Now;
                                            drNew["Is_EHR_Updated"] = 0;
                                            drNew["InsUptDlt"] = 1;
                                            drNew["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                            drNew["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                            if (item.answer_value is string)
                                            {
                                                try
                                                {
                                                    drNew["Question_blnAnswer"] = Convert.ToString(item.answer_value.ToString()).Split('_')[1] == "1" ? true : false;
                                                }
                                                catch
                                                {
                                                    drNew["Question_blnAnswer"] = false;
                                                }
                                            }
                                            dtMedicalHistory_Response_Submit.Rows.Add(drNew);
                                        }
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + ex1.Message + "_" + dr["PatientForm_Web_ID"].ToString());
                                }
                            }
                        }
                    }
                    SynchTrackerBAL.Save_Tracker_To_Local(dtMedicalHistory_Response_Submit, "CD_Response", "CD_Response_LocalDB_ID", "CD_Response_LocalDB_ID");
                    SynchLocalBAL.UpdatePatientFormMedicalHistory_Fields(dtLivePatientForm);
                    #endregion
                    IsMedicalHistoryFetched = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] : " + ex.Message);
                    IsMedicalHistoryFetched = true;
                }
            }

            #endregion
        }

        public void GetMedicalOpenDentalHistoryRecords(string Service_Install_Id, string strPatientFormID = "")
        {
            #region SavePatientMedicleHistory

            if (IsMedicalHistoryFetched)//&& Utility.AditLocationSyncEnable
            {
                try
                {
                    IsMedicalHistoryFetched = false;
                    DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPendingPatientFormMedicalHistory(Service_Install_Id, strPatientFormID);
                    DataTable dtLivePatientForm = dtLocalPatientForm.DefaultView.ToTable(true, "PatientForm_Web_ID", "Patient_EHR_ID", "Patient_Web_ID", "MedicalHistorySubmission_Id", "Clinic_Number", "Service_Install_Id");
                    DataTable dtMedicalHistory_Response_Submit = SynchLocalBAL.GetLocalMedicalHistoryRecords("OD_Response", Service_Install_Id, true, strPatientFormID);
                    dtMedicalHistory_Response_Submit.Rows.Clear();
                    dtMedicalHistory_Response_Submit.Columns.Add("InsUptDlt", typeof(int));
                    #region

                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            foreach (DataRow dr in dtLivePatientForm.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' And Service_Install_Id = '" + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + "' "))
                            {
                                try
                                {
                                    string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("getEagleSoftEHRForms", Utility.DtLocationList.Rows[j]["Loc_ID"].ToString());
                                    PatientMedicalHistoryBO PatientpdfDoc = new PatientMedicalHistoryBO
                                    {
                                        //    loc = "a2f1b7d2-a529-4860-a53a-6c5772290d73",
                                        //    org = "462a2c0a-cb0b-4727-bfca-f172b384a04c",
                                        //    id = "03d1f1e4-f8ac-4212-bc39-500a63486b13",

                                        loc = Utility.DtLocationList.Rows[j]["Loc_ID"].ToString(),//Utility.Loc_ID,
                                        org = Utility.Organization_ID,
                                        id = dr["PatientForm_Web_ID"].ToString().Trim(),
                                    };
                                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                    var clientdoc = new RestClient(strApiPatientForm);
                                    var requestdoc = new RestRequest(Method.POST);
                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                    requestdoc.AddHeader("Content-Type", "application/json");
                                    requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                                    requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                    IRestResponse response = clientdoc.Execute(requestdoc);
                                    if (response.ErrorMessage != null)
                                    {
                                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory_Err Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                        }
                                        IsMedicalHistoryFetched = true;
                                        return;
                                    }

                                    var ApptPatientFormDto = JsonConvert.DeserializeObject<Pull_PatientInfo_MedicalHistoryBO>(response.Content);
                                    if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                                    {
                                        dr["MedicalHistorySubmission_Id"] = ApptPatientFormDto.data._id;
                                        List<Question> AllResponse = new List<Question>();
                                        AllResponse = ApptPatientFormDto.data.Question.Where(x => x._type == "opendentalsheetfield").ToList();
                                        foreach (var item in AllResponse)
                                        {
                                            DataRow drNew = dtMedicalHistory_Response_Submit.NewRow();
                                            drNew["OD_Response_Web_ID"] = item._id;
                                            drNew["PatientForm_Web_ID"] = dr["PatientForm_Web_ID"].ToString();
                                            drNew["Patient_EHR_ID"] = dr["Patient_EHR_ID"].ToString();
                                            drNew["SheetFieldResNum_EHR_ID"] = "";
                                            drNew["SheetFieldDefNum_EHR_ID"] = item.sheetfielddefnum_ehr_id;
                                            drNew["SheetDefNum_EHR_ID"] = item.sheetdefnum_ehr_id;
                                            drNew["FieldType"] = item.fieldtype;
                                            drNew["FieldName"] = "0";
                                            // drNew["FieldName"] = item.fieldname;
                                            drNew["FieldValue"] = item.fieldvalue;
                                            drNew["FontSize"] = item.fontsize;
                                            drNew["FontName"] = item.fontname;
                                            drNew["FontIsBold"] = item.fontisbold;
                                            drNew["XPos"] = item.xpos;
                                            drNew["YPos"] = item.ypos;
                                            drNew["Width"] = item.width;
                                            drNew["Height"] = item.height;
                                            drNew["GrowthBehavior"] = item.growthbehavior;
                                            drNew["RadioButtonValue"] = item.radiobuttonvalue;
                                            drNew["RadioButtonGroup"] = item.radiobuttongroup;
                                            drNew["IsRequired"] = item.isrequired;
                                            drNew["TabOrder"] = item.taborder;
                                            drNew["TextAlign"] = item.textalign;
                                            drNew["ItemColor"] = item.itemcolor;
                                            drNew["IsLocked"] = 0;
                                            drNew["TabOrderMobile"] = 0;
                                            drNew["UiLabelMobile"] = "";
                                            drNew["UiLabelMobileRadioButton"] = "";
                                            drNew["Entry_DateTime"] = System.DateTime.Now;
                                            // drNew["Last_Sync_Date"] = System.DateTime.Now;
                                            drNew["Is_EHR_Updated"] = 0;
                                            drNew["InsUptDlt"] = 1;
                                            drNew["SheetType"] = item.sheettype;
                                            drNew["FieldName"] = item.fieldname;
                                            drNew["IsLocked"] = item.islocked;
                                            drNew["TabOrderMobile"] = item.tabordermobile;
                                            drNew["UiLabelMobile"] = item.uilabelmobile;
                                            drNew["UiLabelMobileRadioButton"] = item.uilabelmobileradiobutton;
                                            drNew["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                            drNew["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                            if (item.answer_value != null)
                                            {
                                                if (item.answer_value.ToString() != "" || item.answer_value.ToString() != string.Empty)
                                                {
                                                    if (item.fieldtype.ToString() == "8")
                                                    {
                                                        try
                                                        {
                                                            drNew["FieldValue"] = Convert.ToString(item.answer_value.ToString()).Split('_')[1] == "1" ? true : false;

                                                        }
                                                        catch
                                                        {
                                                            drNew["FieldValue"] = "";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        try
                                                        {
                                                            drNew["FieldValue"] = item.answer_value.ToString();
                                                        }
                                                        catch
                                                        {
                                                            drNew["FieldValue"] = "";
                                                        }
                                                    }
                                                }
                                            }

                                            dtMedicalHistory_Response_Submit.Rows.Add(drNew);
                                        }
                                    }
                                }
                                catch (Exception ex1)
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " " + ex1.Message + "_" + dr["PatientForm_Web_ID"].ToString());
                                }
                            }
                        }
                    }
                    SynchTrackerBAL.Save_Tracker_To_Local(dtMedicalHistory_Response_Submit, "OD_Response", "OD_Response_LocalDB_ID", "OD_Response_LocalDB_ID");
                    SynchLocalBAL.UpdatePatientFormMedicalHistory_Fields(dtLivePatientForm);
                    #endregion
                    IsMedicalHistoryFetched = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] : " + ex.Message);
                    IsMedicalHistoryFetched = true;
                }
            }

            #endregion
        }

        public static void GetMedicalEasyDentalHistoryRecords()
        {
            #region SavePatientMedicleHistory
            try
            {

                DataTable dtLocalPatientForm = SynchLocalBAL.GetLocalPendingPatientFormMedicalHistory("1");
                DataTable dtLivePatientForm = dtLocalPatientForm.DefaultView.ToTable(true, "PatientForm_Web_ID", "Patient_EHR_ID", "Patient_Web_ID", "MedicalHistorySubmission_Id", "Clinic_Number", "Service_Install_Id");
                DataTable dtMedicalHistory_Response_Submit = SynchLocalBAL.GetLocalMedicalHistoryRecords("EasyDental_Response", "1", true);
                dtMedicalHistory_Response_Submit.Rows.Clear();
                dtMedicalHistory_Response_Submit.Columns.Add("InsUptDlt", typeof(int));
                #region

                foreach (DataRow dr in dtLivePatientForm.Rows)
                {
                    try
                    {
                        string strApiPatientForm = PullLiveDatabaseBAL.GetLiveRecord("getEagleSoftEHRForms", Utility.Location_ID);

                        PatientMedicalHistoryBO PatientpdfDoc = new PatientMedicalHistoryBO
                        {
                            //    loc = "a2f1b7d2-a529-4860-a53a-6c5772290d73",
                            //    org = "462a2c0a-cb0b-4727-bfca-f172b384a04c",
                            //    id = "03d1f1e4-f8ac-4212-bc39-500a63486b13",

                            loc = Utility.Loc_ID,
                            org = Utility.Organization_ID,
                            id = dr["PatientForm_Web_ID"].ToString().Trim(),
                        };
                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string jsonString = javaScriptSerializer.Serialize(PatientpdfDoc);
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        var clientdoc = new RestClient(strApiPatientForm);
                        var requestdoc = new RestRequest(Method.POST);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        requestdoc.AddHeader("Content-Type", "application/json");
                        requestdoc.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                        requestdoc.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                        IRestResponse response = clientdoc.Execute(requestdoc);
                        if (response.ErrorMessage != null)
                        {
                            if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                            { }
                            else
                            {
                                GoalBase.WriteToErrorLogFile_Static("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] : " + response.ErrorMessage);
                            }
                            return;
                        }

                        var ApptPatientFormDto = JsonConvert.DeserializeObject<Pull_PatientInfo_MedicalHistoryBO>(response.Content);
                        if (ApptPatientFormDto != null && ApptPatientFormDto.data != null)
                        {
                            dr["MedicalHistorySubmission_Id"] = ApptPatientFormDto.data._id;
                            List<Question> AllResponse = new List<Question>();
                            AllResponse = ApptPatientFormDto.data.Question.Where(x => x._type == "easydentalform").ToList();
                            if (AllResponse != null)
                            {
                                foreach (var item in AllResponse)
                                {
                                    DataRow drNew = dtMedicalHistory_Response_Submit.NewRow();
                                    drNew["EasyDental_Response_Web_ID"] = item._id;
                                    drNew["PatientForm_Web_ID"] = dr["PatientForm_Web_ID"].ToString();
                                    drNew["Patient_EHR_ID"] = dr["Patient_EHR_ID"].ToString();
                                    drNew["EasyDental_QuestionId"] = item.easydental_questionid;
                                    drNew["EasyDental_QuestionType"] = item.questiontype;
                                    drNew["Answer_Value"] = Convert.ToString(item.AnsValue.ToString());
                                    drNew["Entry_DateTime"] = System.DateTime.Now;
                                    // drNew["Last_Sync_Date"] = System.DateTime.Now;
                                    drNew["Is_EHR_Updated"] = 0;
                                    drNew["InsUptDlt"] = 1;
                                    if (item.questiontype == "3")
                                    {
                                        string Bool = "";
                                        try
                                        {
                                            Bool = Convert.ToString(item.AnsValue.ToString()).Split('_')[1].ToString();
                                        }
                                        catch
                                        {
                                            Bool = "0";
                                        }
                                        drNew["Answer_Value"] = Bool;
                                    }
                                    drNew["Clinic_Number"] = "0";
                                    drNew["Service_Install_Id"] = "1";
                                    dtMedicalHistory_Response_Submit.Rows.Add(drNew);
                                }
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        GoalBase.WriteToErrorLogFile_Static("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] : " + ex1.Message + "_" + dr["PatientForm_Web_ID"].ToString());
                        //return;
                    }
                }

                SynchTrackerBAL.Save_Tracker_To_Local(dtMedicalHistory_Response_Submit, "EasyDental_Response", "EasyDental_Response_LocalDB_ID", "EasyDental_Response_LocalDB_ID");
                SynchLocalBAL.UpdatePatientFormMedicalHistory_Fields(dtLivePatientForm);
                #endregion

            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PatientFormMedicalHistory Sync (Adit Server To Local Database)] : " + ex.Message);

            }
            #endregion
        }

        #region GetMedicalHistoryFieldsFromWEb

        private void GetEaglesoftMedicalHistoryRecords()
        {
            try
            {
                GetEaglesoftSectionMasterFromAdit("EagleSoftSectionMaster", "Section_EHR_Id", "SectionMaster_Web_Id", "SectionMaster_LocalDB_ID", "sectionmaster");
                GetEaglesoftSectionMasterFromAdit("EagleSoftAlertMaster", "Alert_EHR_Id", "AlertMaster_web_Id", "AlertMaster_LocalDB_ID", "alertmaster");
                GetEaglesoftSectionMasterFromAdit("EagleSoftFormMaster", "Form_EHR_Id", "FormMaster_web_Id", "FormMaster_LocalDB_ID", "formmaster");
                GetEaglesoftSectionMasterFromAdit("EagleSoftSectionItemMaster", "SectionItem_EHR_Id", "SectionItem_WEB_Id", "SectionItemMaster_LocalDB_ID", "sectionitemmaster");
                GetEaglesoftSectionMasterFromAdit("EagleSoftSectionItemOptionMaster", "SectionItemOption_EHR_Id", "SectionItemOption_WEB_Id", "SectionItemOptionMaster_LocalDB_ID", "sectionitemoptionmaster");
                IsMedicalHistoryRecordsPulled = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetEaglesoftSectionMasterFromAdit(string tablename, string ehrIdColumnName, string webIdColumnName, string localIdColumnsName, string APIName)
        {
            try
            {
                //if (Utility.AditLocationSyncEnable)
                //{
                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                {
                    if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                    {
                        string strApiOperatory = PullLiveDatabaseBAL.GetLiveRecord("eaglesoftsectionmaster", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                        strApiOperatory = strApiOperatory.Replace("MedicalHistoryAPI", APIName);

                        var client = new RestClient(strApiOperatory);
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        var request = new RestRequest(Method.GET);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        request.AddHeader("apptype", "aditehr");
                        request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));

                        IRestResponse response = client.Execute(request);

                        if (response.ErrorMessage != null)
                        {
                            if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                            { }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[" + tablename + " Sync (Adit Server To Local Database)] Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                            }
                            return;
                        }

                        var AppointmentDto = JsonConvert.DeserializeObject<Response_EaglesoftMedicalHistory>(response.Content);
                        if (AppointmentDto != null)
                        {
                            if (AppointmentDto.data != null)
                            {
                                DataTable dtLocalAppointment = SynchLocalBAL.GetLocalMedicalHistoryRecords(tablename, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), true);
                                DataTable dtLiveAppointment = dtLocalAppointment.Clone();
                                dtLiveAppointment.Columns.Add("InsUptDlt", typeof(int));

                                foreach (var item in AppointmentDto.data)
                                {
                                    DataRow RowPro = dtLiveAppointment.NewRow();

                                    if (tablename.ToUpper() == "EAGLESOFTSECTIONMASTER")
                                    {
                                        RowPro[ehrIdColumnName] = item.sectionmaster_ehr_id;
                                    }
                                    else if (tablename.ToUpper() == "EAGLESOFTALERTMASTER")
                                    {
                                        RowPro[ehrIdColumnName] = item.alertmaster_ehr_id;
                                    }
                                    else if (tablename.ToUpper() == "EAGLESOFTFORMMASTER")
                                    {
                                        RowPro[ehrIdColumnName] = item.formmaster_ehr_id;
                                    }
                                    else if (tablename.ToUpper() == "EAGLESOFTSECTIONITEMMASTER")
                                    {
                                        RowPro[ehrIdColumnName] = item.sectionitemmaster_ehr_id;
                                    }
                                    else if (tablename.ToUpper() == "EAGLESOFTSECTIONITEMOPTIONMASTER")
                                    {
                                        RowPro[ehrIdColumnName] = item.sectionitemoptionmaster_ehr_id;
                                    }

                                    RowPro[webIdColumnName] = item._id;
                                    RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                    RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                    dtLiveAppointment.Rows.Add(RowPro);
                                    dtLiveAppointment.AcceptChanges();
                                }

                                foreach (DataRow dtDtxRow in dtLiveAppointment.Rows)
                                {
                                    if (Utility.CheckEHR_ID(dtDtxRow[ehrIdColumnName].ToString().Trim()) == string.Empty)
                                    {
                                        dtDtxRow["InsUptDlt"] = 0;
                                        continue;
                                    }

                                    DataRow[] row = dtLocalAppointment.Copy().Select("" + ehrIdColumnName + " = '" + dtDtxRow[ehrIdColumnName].ToString().Trim() + "' And Clinic_Number = '" + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "' ");
                                    if (row.Length > 0)
                                    {
                                        dtDtxRow["InsUptDlt"] = 0;
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 1;
                                    }
                                }
                                dtLiveAppointment.AcceptChanges();

                                if (dtLiveAppointment != null && dtLiveAppointment.Rows.Count > 0 && dtLiveAppointment.AsEnumerable().Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1).Count() > 0)
                                {
                                    // bool status = PullLiveDatabaseBAL.Save_Pull_EHRAppointment_WithOut_PatientID_Live_To_Local(dtLiveAppointment);
                                    bool status = SynchTrackerBAL.Save_Tracker_To_Local(dtLiveAppointment, tablename, localIdColumnsName, "" + localIdColumnsName + "");
                                    if (status)
                                    {
                                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime(tablename + "_Pull");
                                        ObjGoalBase.WriteToSyncLogFile("Pull " + tablename + " Sync (Adit Server To Local Database) Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //}
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region User
        public static void CheckEntryUserLoginIdExist()
        {

            try
            {
                Utility.dtLocationWiseUser.Rows.Clear();
                if (!Utility.dtLocationWiseUser.Columns.Contains("ClinicNumber"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("ClinicNumber");
                }
                if (!Utility.dtLocationWiseUser.Columns.Contains("LocationId"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("LocationId");
                }
                if (!Utility.dtLocationWiseUser.Columns.Contains("EHR_UserLogin_ID"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("EHR_UserLogin_ID");
                }
                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                {
                    DataRow drNew = Utility.dtLocationWiseUser.NewRow();
                    drNew["ClinicNumber"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                    drNew["LocationId"] = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString();
                    string strGetEntryUserId = PullLiveDatabaseBAL.GetLiveRecord("entryuserid", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                    var clientuser = new RestClient(strGetEntryUserId);
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                    IRestResponse response = clientuser.Execute(request);

                    if (response.ErrorMessage != null)
                    {
                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                        { }
                        else
                        {
                            GoalBase.WriteToErrorLogFile_Static("[Users Sync (Adit Server)] : " + response.ErrorMessage);
                        }
                    }
                    //GoalBase.WriteToErrorLogFile_Static("[Users Sync Response] : " + response.Content);
                    var UserVar = JsonConvert.DeserializeObject<Pull_UsersBO>(response.Content);

                    if (UserVar.data != null)
                    {
                        if (UserVar.data.Count > 0)
                        {
                            for (int i = 0; i < UserVar.data.Count; i++)
                            {
                                if (UserVar.data[i]._id.Equals(Utility.DtLocationList.Rows[j]["Loc_Id"].ToString()))
                                {
                                    Utility.EHR_UserLogin_ID = UserVar.data[i].default_ehruser;
                                    drNew["EHR_UserLogin_ID"] = UserVar.data[i].default_ehruser;
                                    // Utility.DtLocationList.Rows[j]["EHR_User_Id"] = UserVar.data._id;
                                }
                            }
                        }

                    }
                    Utility.dtLocationWiseUser.Rows.Add(drNew);
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("GetEntryUserLoginId : " + ex.Message);
            }
        }
        #endregion

        #region PatientPaymentlog

        public static void SynchDataLiveDB_Pull_PatientPayments(string strApiPayments)
        {

            try
            {
                if (Utility.IsApplicationIdleTimeOff) // && Utility.AditLocationSyncEnable
                {
                    Utility.WriteToSyncLogFile_All("Patient Payment Log Sync Pull payment from Adit App");
                    DataTable dtLivePatientPaymentLog = new DataTable();
                    DataTable dtLocalPatientPaymentLog = new DataTable();
                    DataTable dtWebPatientPayment = new DataTable();
                    // dtWebPatientPayment = Pozative.BAL.SynchLocalBAL.GetLocalWebPatientPaymentData()
                    dtWebPatientPayment = Pozative.BAL.SynchLocalBAL.GetPatientPaymentTableBlankStructure();

                    for (int i = 0; i < Utility.DtInstallServiceList.Rows.Count; i++)
                    {
                        for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                            {
                                // strApiPayments = PullLiveDatabaseBAL.GetLiveRecord("PatientPaymentLog", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                                Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "Call Payment API");
                                var client = new RestClient(strApiPayments);
                                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                var request = new RestRequest(Method.POST);
                                ServicePointManager.Expect100Continue = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                request.AddHeader("cache-control", "no-cache");
                                request.AddHeader("content-type", "application/json");
                                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                string jsonstringn = "{\"locationId\":\"" + Utility.DtLocationList.Rows[j]["Loc_Id"].ToString() + "\",\"only_ready_to_import\":false}";
                                request.AddParameter("application/json", jsonstringn, ParameterType.RequestBody);
                                Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "Request Sent into the API " + " Authorization, TokenKey, action & json Request " + jsonstringn.ToString());
                                GoalBase.WriteToPaymentLogFromAll_Static(strApiPayments);
                                GoalBase.WriteToPaymentLogFromAll_Static(Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                GoalBase.WriteToPaymentLogFromAll_Static(jsonstringn);
                                IRestResponse response = client.Execute(request);

                                if (response.Content != null)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "Response received from API (" + response.Content.ToString() + ")");

                                }
                                else
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "Response is null");
                                }
                                if (response.ErrorMessage != null)
                                {
                                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                    { }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[PatientPaymentLog Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                    }
                                    return;
                                }
                                GoalBase.WriteToPaymentLogFromAll_Static(response.Content.ToString());
                                Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "----------------------------Deserialize repsonse--------------------------------");
                                var ProvidersDto = JsonConvert.DeserializeObject<Pull_PatientPayment>(response.Content);

                                if (ProvidersDto != null && ProvidersDto.data != null && ProvidersDto.data.Count > 0)
                                {
                                    if (ProvidersDto.data.Count == 0)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "Deserialize response ProvidersDto count :  (" + ProvidersDto.data.Count + ") no record ");
                                    }
                                    // Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "Deserialize repsonse from API(" + ProvidersDto.message.ToString() + ")");
                                    dtWebPatientPayment.Rows.Clear();
                                    foreach (var item in ProvidersDto.data)
                                    {
                                        if (item.patient_ehr_id != null || item.patient_first_name != null)
                                        {
                                            if ((item.payment_status.ToString().ToUpper() == "PAID" || item.payment_status.ToString().ToUpper() == "PARTIAL-PAID" || item.payment_status.ToString().ToUpper() == "REFUNDED" || item.payment_status.ToString().ToUpper() == "PARTIAL-REFUNDED"))
                                            {
                                                DataRow RowPro = dtLivePatientPaymentLog.NewRow();
                                                if (item.patient_ehr_id != null && item.patient_ehr_id.ToString() != string.Empty && item.patient_ehr_id.ToString().Trim() != "-")
                                                {
                                                    DataRow dataRow;
                                                    dataRow = dtWebPatientPayment.NewRow();

                                                    dataRow["PatientEHRId"] = item.patient_ehr_id.ToString();
                                                    dataRow["Patient_Web_ID"] = item.patientId.ToString();
                                                    dataRow["ProviderEHRId"] = "";
                                                    dataRow["PaymentDate"] = item.created_at.ToString();
                                                    if (item.amount != null)
                                                    {
                                                        dataRow["Amount"] = item.amount.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow["Amount"] = 0;
                                                    }
                                                    dataRow["PaymentNote"] = item.template.ToString();
                                                    dataRow["PaymentMode"] = item.payment_status.ToString();
                                                    dataRow["PaymentType"] = item.payment_type.ToString();
                                                    dataRow["template"] = item.template.ToString()+ "-[UQID:" + item.apEhrSyncId+"]";  //[UQID:12j3213hk21hk3hk1223j432h4kj23h4] 
                                                    if (item.fees != null)
                                                    {
                                                        dataRow["Fees"] = item.fees.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow["Fees"] = 0;
                                                    }
                                                    dataRow["Discount"] = item.discount.ToString();
                                                    dataRow["FirstName"] = item.patient_first_name.ToString();
                                                    dataRow["LastName"] = item.patient_last_name.ToString();
                                                    dataRow["Mobile"] = item.patient_number.ToString();
                                                    dataRow["Email"] = item.patient_email.ToString();
                                                    dataRow["EHRSyncPaymentLog"] = ProvidersDto.pms_ehr_log_setting;
                                                    dataRow["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    dataRow["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                    dataRow["ChequeNumber"] = "";
                                                    dataRow["BankOrBranchName"] = "";
                                                    dataRow["PaymentInOut"] = "In";
                                                    dataRow["PaymentReceivedLocal"] = 1;
                                                    dataRow["PaymentEntryDatetimeLocal"] = DateTime.Now.ToString();
                                                    dataRow["PaymentUpdatedEHR"] = 0;
                                                    dataRow["PaymentUpdatedEHRDateTime"] = DBNull.Value;
                                                    dataRow["PaymentStatusCompletedAdit"] = 0;
                                                    dataRow["PaymentStatusCompletedDateTime"] = DBNull.Value;
                                                    dataRow["PaymentEHRId"] = "";
                                                    dataRow["PatientPaymentWebId"] = item.apEhrSyncId;
                                                    dataRow["EHRErroLog"] = "";
                                                    dataRow["TryInsert"] = 0;
                                                    dataRow["PaymentMethod"] = item.payment_method.ToString();
                                                    dataRow["EHRSyncFinancialLogSetting"] = ProvidersDto.pms_ehr_log_financing_setting; //DBNull.Value;
                                                    dtWebPatientPayment.Rows.Add(dataRow);
                                                }
                                            }
                                        }

                                    }

                                    dtWebPatientPayment.AcceptChanges();
                                    if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                                    {
                                        Utility.CheckEntryUserLoginIdExist();
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment, "data table dtWebPatientPayment count  (" + dtWebPatientPayment.Rows.Count + ")");

                                        // save payment to ehr
                                        if (Utility.Application_ID == 1)
                                        {
                                            //save payment to eaglesoft
                                            try
                                            {
                                                SynchEaglesoftBAL.SavePatientPaymentTOEHR(Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), dtWebPatientPayment, Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment);
                                            }
                                            catch (Exception ex)
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                                            }
                                        }
                                        if (Utility.Application_ID == 2)
                                        {
                                            //save payment to opendental
                                            try
                                            {
                                                bool Is_Record_Update = SynchOpenDentalBAL.Save_PatientPayment_Local_To_OpenDental(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment);
                                            }
                                            catch (Exception ex)
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                                            }

                                        }
                                        if (Utility.Application_ID == 3)
                                        {
                                            //save payment to dentrix
                                            try
                                            {
                                                string Guar_ID = "";
                                                if (!dtWebPatientPayment.Columns.Contains("Guar_ID"))
                                                {
                                                    dtWebPatientPayment.Columns.Add("Guar_ID", typeof(string));
                                                }
                                                foreach (DataRow drRow in dtWebPatientPayment.Rows)
                                                {
                                                    Guar_ID = SynchDentrixBAL.GetPatGuaridAndProviders(drRow["PatientEHRId"].ToString());
                                                    drRow["Guar_ID"] = Guar_ID;
                                                }
                                                SynchDentrixBAL.SavePatientPaymentTOEHR(dtWebPatientPayment, Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment);
                                            }
                                            catch (Exception ex)
                                            {

                                                GoalBase.WriteToErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                                            }
                                        }
                                        if (Utility.Application_ID == 5)
                                        {
                                            //save payment to cleardent
                                            try
                                            {
                                                SynchClearDentBAL.Save_PatientPayment_Local_To_ClearDent(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment);
                                            }
                                            catch (Exception ex)
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                                            }

                                        }
                                        if (Utility.Application_ID == 6)
                                        {
                                            //save payment to tracker
                                            try
                                            {
                                                SynchTrackerBAL.SavePatientPaymentTOEHR(Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), dtWebPatientPayment, Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment);
                                            }
                                            catch (Exception ex)
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                                            }

                                        }
                                        if (Utility.Application_ID == 7)
                                        {
                                            //save payment to Practicework
                                        }
                                        if (Utility.Application_ID == 8)
                                        {
                                            //save payment to easydental
                                            try
                                            {
                                                string Guar_ID = "";
                                                if (!dtWebPatientPayment.Columns.Contains("Guar_ID"))
                                                {
                                                    dtWebPatientPayment.Columns.Add("Guar_ID", typeof(string));
                                                }
                                                foreach (DataRow drRow in dtWebPatientPayment.Rows)
                                                {
                                                    Guar_ID = SynchEasyDentalBAL.GetPatientGuarid(drRow["PatientEHRId"].ToString());
                                                    drRow["Guar_ID"] = Guar_ID;
                                                }
                                                SynchEasyDentalBAL.SavePatientPaymentTOEHR(dtWebPatientPayment, Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment);
                                            }
                                            catch (Exception ex)
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                                            }

                                        }
                                        if (Utility.Application_ID == 10)
                                        {
                                            //save payment to practiceweb
                                            try
                                            {
                                                bool Is_Record_Update = SynchPracticeWebBAL.Save_PatientPayment_Local_To_PracticeWeb(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment);
                                            }
                                            catch (Exception ex)
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                                            }

                                        }
                                        if (Utility.Application_ID == 11)
                                        {
                                            //save payment to abeldent
                                            try
                                            {
                                                SynchAbelDentBAL.SavePatientPaymentTOEHR(Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), dtWebPatientPayment, Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility._filename_EHR_Payment, Utility._EHRLogdirectory_EHR_Payment);
                                            }
                                            catch (Exception ex)
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[Error In PatientPayment]: " + ex.Message);
                                            }

                                        }
                                    }

                                }
                                #region Code Commented by shruti For Need to improve sync time for payments to ledger
                                //if (ProvidersDto != null && ProvidersDto.data != null)
                                //{
                                //    dtLivePatientPaymentLog = SynchLocalBAL.GetLocalPatientWisePaymentLog(true, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
                                //    DataTable EHRPatientTable = new DataTable();
                                //    if (Utility.Application_ID == 1)
                                //    {
                                //        EHRPatientTable = SynchEaglesoftBAL.GetPatientListFromEagleSoft(Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()));
                                //    }
                                //    // OpenDental
                                //    else if (Utility.Application_ID == 2)
                                //    {
                                //        EHRPatientTable = SynchOpenDentalBAL.GetOpenDentalPatientID_NameData(Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()));

                                //    }
                                //    // Dentrix
                                //    else if (Utility.Application_ID == 3)
                                //    {
                                //        EHRPatientTable = SynchDentrixBAL.GetDentrixPatientID_NameData();
                                //    }
                                //    //Softdent
                                //    else if (Utility.Application_ID == 4)
                                //    {
                                //        EHRPatientTable = SynchLocalBAL.GetLocalPatientData(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                                //    }
                                //    //ClearDent
                                //    else if (Utility.Application_ID == 5)
                                //    {
                                //        EHRPatientTable = SynchClearDentBAL.GetClearDentPatientID_NameData();
                                //    }
                                //    //Tracker
                                //    else if (Utility.Application_ID == 6)
                                //    {
                                //        EHRPatientTable = SynchTrackerBAL.GetTrackerPatientListData();
                                //    }
                                //    // PracticeWork
                                //    else if (Utility.Application_ID == 7)
                                //    {
                                //        EHRPatientTable = SynchPracticeWorkBAL.GetPracticeWorkPatientListData();
                                //    }
                                //    //EasyDental
                                //    else if (Utility.Application_ID == 8)
                                //    {
                                //        EHRPatientTable = SynchEasyDentalBAL.GetEasyDentalPatientID_NameData();
                                //    }
                                //    else if (Utility.Application_ID == 10)
                                //    {
                                //        EHRPatientTable = SynchPracticeWebBAL.GetPracticeWebPatientID_NameData(Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()));

                                //    }
                                //    //Abeldent
                                //    else if (Utility.Application_ID == 11)
                                //    {
                                //        EHRPatientTable = SynchAbelDentBAL.GetAbelDentPatientData();
                                //    }
                                //    string patientEHRID = "";
                                //    foreach (var item in ProvidersDto.data)
                                //    {
                                //        if (item.patient_ehr_id != null || item.patient_first_name != null)
                                //        {
                                //            if ((item.payment_status.ToString().ToUpper() == "PAID" || item.payment_status.ToString().ToUpper() == "PARTIAL-PAID" || item.payment_status.ToString().ToUpper() == "REFUNDED" || item.payment_status.ToString().ToUpper() == "PARTIAL-REFUNDED"))
                                //            {
                                //                DataRow RowPro = dtLivePatientPaymentLog.NewRow();
                                //                if (item.patient_ehr_id != null && item.patient_ehr_id.ToString() != string.Empty && item.patient_ehr_id.ToString().Trim() != "-")
                                //                {
                                //                    RowPro["PatientEHRId"] = item.patient_ehr_id.ToString(); //GetPatientEHRID(EHRPatientTable, item.patient_ehr_id, item.patient_number, item.patient_first_name, "", item.patient_last_name, item.patient_email, Utility.DtLocationList.Rows[j]["DBConnString"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
                                //                }
                                //                else
                                //                {
                                //                    try
                                //                    {
                                //                        patientEHRID = GetPatientEHRID(DateTime.Now.ToShortDateString(), EHRPatientTable, item.patient_ehr_id, item.patient_number, item.patient_first_name, "", item.patient_last_name, item.patient_email, Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), DateTime.Now, "");
                                //                        RowPro["PatientEHRId"] = patientEHRID;
                                //                    }
                                //                    catch (Exception ex2)
                                //                    {
                                //                        RowPro["EHRErroLog"] = "Error while creating patient : " + ex2.Message.ToString();
                                //                    }
                                //                }
                                //                RowPro["FirstName"] = item.patient_first_name;
                                //                RowPro["LastName"] = item.patient_last_name;
                                //                RowPro["Mobile"] = item.patient_number;
                                //                RowPro["Email"] = item.patient_email;
                                //                if (item.fees != null)
                                //                {
                                //                    RowPro["Fees"] = item.fees;
                                //                }
                                //                else
                                //                {
                                //                    RowPro["Fees"] = 0;
                                //                }
                                //                RowPro["Discount"] = item.discount;
                                //                RowPro["Patient_Web_ID"] = item.patientId;
                                //                RowPro["PatientPaymentWebId"] = item.apEhrSyncId;
                                //                RowPro["ProviderEHRId"] = "";//item.ProviderEHRId;
                                //                                             //var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToDouble(item.paid_date) / 1000d)).ToLocalTime();
                                //                RowPro["PaymentDate"] = item.created_at;// dt.ToString(); //Utility.DateTryParse(item.paid_date,"yyyy/MM/dd");                                
                                //                if (item.amount != null)
                                //                {
                                //                    RowPro["Amount"] = item.amount;
                                //                }
                                //                else
                                //                {
                                //                    RowPro["Amount"] = 0;
                                //                }
                                //                RowPro["PaymentNote"] = "";//item.;
                                //                RowPro["PaymentMode"] = item.payment_status;
                                //                RowPro["PaymentType"] = item.payment_type;
                                //                RowPro["PaymentInOut"] = "In";// item.PaymentInOut;
                                //                RowPro["BankOrBranchName"] = "";// item.BankOrBranchName;
                                //                RowPro["ChequeNumber"] = "";// item.ChequeNumber;
                                //                RowPro["PaymentReceivedLocal"] = 1;
                                //                RowPro["PaymentEntryDatetimeLocal"] = DateTime.Now.ToString();
                                //                RowPro["PaymentUpdatedEHR"] = 0;
                                //                RowPro["PaymentUpdatedEHRDateTime"] = DBNull.Value;
                                //                RowPro["PaymentStatusCompletedAdit"] = 0;
                                //                RowPro["PaymentStatusCompletedDateTime"] = DBNull.Value;
                                //                RowPro["PaymentEHRId"] = "";
                                //                RowPro["template"] = item.template;
                                //                RowPro["EHRSyncPaymentLog"] = ProvidersDto.pms_ehr_log_setting;
                                //                RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                //                RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                //                dtLivePatientPaymentLog.Rows.Add(RowPro);
                                //                dtLivePatientPaymentLog.AcceptChanges();
                                //            }
                                //        }
                                //    }
                                //}

                                //dtLocalPatientPaymentLog = SynchLocalBAL.GetLocalPatientWisePaymentLog(false, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());

                                //if (!dtLivePatientPaymentLog.Columns.Contains("InsUptDlt"))
                                //{
                                //    dtLivePatientPaymentLog.Columns.Add("InsUptDlt", typeof(string));
                                //}

                                //foreach (DataRow dtDtxRow in dtLivePatientPaymentLog.Rows)
                                //{

                                //    DataRow[] row = dtLocalPatientPaymentLog.Copy().Select("PatientPaymentWebId = '" + dtDtxRow["PatientPaymentWebId"].ToString().Trim() + "'");
                                //    if (row.Length > 0)
                                //    {
                                //        dtDtxRow["InsUptDlt"] = 2;
                                //    }
                                //    else
                                //    {
                                //        dtDtxRow["InsUptDlt"] = 1;
                                //    }
                                //}

                                //dtLivePatientPaymentLog.AcceptChanges();

                                //if (dtLivePatientPaymentLog != null && dtLivePatientPaymentLog.Rows.Count > 0)
                                //{
                                //    bool status = PullLiveDatabaseBAL.Save_PatientPaymentLog_To_Local(dtLivePatientPaymentLog);
                                //    if (status)
                                //    {
                                //        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                //        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientPaymentLog");
                                //        GoalBase.WriteToSyncLogFile_Static("PatientPaymentLog Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                //        IsProviderSyncPull = true;
                                //    }
                                //    else
                                //    {
                                //        IsProviderSyncPull = false;
                                //    }
                                //}
                                //else
                                //{
                                //    IsProviderSyncPull = true;
                                //}
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PatientPaymentLog Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }
        public static void SynchDataLiveDB_Pull_PatientPaymentLog()
        {
            string StrAditpayApiPayments = string.Empty;
            string StrCareCreditApiPayments = string.Empty;
            try
            {
                for (int i = 0; i < Utility.DtInstallServiceList.Rows.Count; i++)
                {
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            StrAditpayApiPayments = PullLiveDatabaseBAL.GetLiveRecord("PatientPaymentLog", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                            StrCareCreditApiPayments = PullLiveDatabaseBAL.GetLiveRecord("financialpatientpayment", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                            SynchDataLiveDB_Pull_PatientPayments(StrAditpayApiPayments);
                            SynchDataLiveDB_Pull_PatientPayments(StrCareCreditApiPayments);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Error in SynchDataLiveDB_Pull_PatientPayments Method] : " + ex.Message);
            }

        }
        #endregion
        public static void ComparePatientWithMobileAndEmail(DataTable PatientTable, string MobileContact, string Email, string TmpWebPatientName, string TmpWebRevPatientName, string First_Name, string Last_Name, DateTime DOB, ref string tmpPatient_id, ref string tmpPatient_Gur_id, ref string tmpFirstName, ref string tmpLastName)
        {
            try
            {
                int dboresult = 0;
                int mobileresult = 0;
                DataRow[] rowDOB = PatientTable.Copy().Select("birth_date = '" + DOB.ToShortDateString() + "'");
                MobileContact = MobileContact.Replace("-", "").Replace("/", "").Trim();
                DataRow[] rowMobile = PatientTable.Copy().Select("TRIM(Mobile) = '" + MobileContact.Trim() + "' OR TRIM(Work_Phone) = '" + MobileContact + "' OR TRIM(Home_Phone) = '" + MobileContact + "'");
                if (rowDOB.Length > 0)
                {
                    #region Check DOB,FirstName,LastName 
                    dboresult = ComparePatientName(TmpWebPatientName, TmpWebRevPatientName, rowDOB, First_Name, Last_Name, false, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);

                    #region if Found more then 1 patient records based on DOB,FirstName & LastName
                    if (dboresult > 1)
                    {
                        MobileContact = MobileContact.Replace("-", "").Replace("/", "").Trim();
                        DataRow[] row = rowDOB.CopyToDataTable().Copy().Select("TRIM(Mobile) = '" + MobileContact.Trim() + "' OR TRIM(Work_Phone) = '" + MobileContact + "' OR TRIM(Home_Phone) = '" + MobileContact + "'");
                        if (row.Length > 0)
                        {
                            mobileresult = ComparePatientName(TmpWebPatientName, TmpWebRevPatientName, row, First_Name, Last_Name, true, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);

                            #region Records not found based on Mobile,Firstname & Lastname
                            if (mobileresult == 0)
                            {
                                #region Set first Patient with DOB, Firstname & LastName
                                ComparePatientName(TmpWebPatientName, TmpWebRevPatientName, rowDOB, First_Name, Last_Name, true, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                                #endregion
                            }
                            else
                            {
                                // Do Nothing
                            }
                            #endregion
                        }
                        else
                        {
                            #region Set first Patient with DOB, Firstname & LastName
                            ComparePatientName(TmpWebPatientName, TmpWebRevPatientName, rowDOB, First_Name, Last_Name, true, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                            #endregion
                        }
                    }
                    #endregion
                    else if (dboresult == 0)
                    {
                        ComparePatientWithMobileFirstNameLastName(rowMobile, PatientTable, MobileContact, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                    }
                    #endregion
                }
                else if (rowMobile.Length > 0)
                {
                    ComparePatientWithMobileFirstNameLastName(rowMobile, PatientTable, MobileContact, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                }
                else
                {
                    #region IF Records Not match with DOB,Firstname,LastName OR Mobile,FIrstName,LastName Then Compare with FirstName, DOB and Mobile
                    ComparePatientWithDOBMobileFirstname(PatientTable, First_Name, DOB, MobileContact, ref tmpPatient_id);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static void ComparePatientWithMobileFirstNameLastName(DataRow[] rowMobile, DataTable PatientTable, string MobileContact, string TmpWebPatientName, string TmpWebRevPatientName, string First_Name, string Last_Name, DateTime DOB, ref string tmpPatient_id, ref string tmpPatient_Gur_id, ref string tmpFirstName, ref string tmpLastName)
        {
            try
            {
                int dboresult = 0;
                int mobileresult = 0;
                #region If Records not found with DOB,FirstName& LastName then Check with Mobile Number, FirstName, LastName                       
                if (rowMobile.Length > 0)
                {
                    dboresult = ComparePatientName(TmpWebPatientName, TmpWebRevPatientName, rowMobile, First_Name, Last_Name, false, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);

                    #region if Found more then 1 patient records based on Mobile,FirstName & LastName
                    if (dboresult > 1)
                    {
                        DataRow[] row = rowMobile.CopyToDataTable().Copy().Select("birth_date = '" + DOB.ToShortDateString() + "'");
                        if (row.Length > 0)
                        {
                            mobileresult = ComparePatientName(TmpWebPatientName, TmpWebRevPatientName, row, First_Name, Last_Name, true, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);

                            #region Records not found based on DOB,Firstname & Lastname
                            if (mobileresult == 0)
                            {
                                #region Set first Patient with Mobile, Firstname & LastName
                                ComparePatientName(TmpWebPatientName, TmpWebRevPatientName, rowMobile, First_Name, Last_Name, true, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                                #endregion
                            }
                            else
                            {
                                // Do Nothing
                            }
                            #endregion
                        }
                        else
                        {
                            #region Set first Patient with Mobile, Firstname & LastName
                            ComparePatientName(TmpWebPatientName, TmpWebRevPatientName, rowMobile, First_Name, Last_Name, true, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                            #endregion
                        }
                    }
                    else if (dboresult == 0)
                    {
                        #region IF Records Not match with DOB,Firstname,LastName OR Mobile,FIrstName,LastName Then Compare with FirstName, DOB and Mobile
                        ComparePatientWithDOBMobileFirstname(PatientTable, First_Name, DOB, MobileContact, ref tmpPatient_id);
                        #endregion
                    }
                    #endregion
                }
                #endregion
                else
                {
                    #region IF Records Not match with DOB,Firstname,LastName OR Mobile,FIrstName,LastName Then Compare with FirstName, DOB and Mobile
                    ComparePatientWithDOBMobileFirstname(PatientTable, First_Name, DOB, MobileContact, ref tmpPatient_id);
                    #endregion
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private static void ComparePatientWithDOBMobileFirstname(DataTable PatientTable, string First_Name, DateTime DOB, string MobileContact, ref string tmpPatient_id)
        {
            try
            {
                #region IF Records Not match with DOB,Firstname,LastName OR Mobile,FIrstName,LastName Then Compare with FirstName, DOB and Mobile
                PatientTable.CaseSensitive = false;
                DataRow[] rowDOBMobile = PatientTable.Copy().Select("FirstName = '" + First_Name + "' AND birth_date = '" + DOB.ToShortDateString() + "' AND (TRIM(Mobile) = '" + MobileContact.Trim() + "' OR TRIM(Work_Phone) = '" + MobileContact + "' OR TRIM(Home_Phone) = '" + MobileContact + "')");
                if (rowDOBMobile.Length > 0)
                {
                    tmpPatient_id = rowDOBMobile[0]["Patient_EHR_ID"].ToString();
                }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int ComparePatientName(string TmpWebPatientName, string TmpWebRevPatientName, DataRow[] row, string First_Name, string Last_Name, bool fromMobile, ref string tmpPatient_id, ref string tmpPatient_Gur_id, ref string tmpFirstName, ref string tmpLastName)
        {
            try
            {
                int result = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                    {
                        result = result + 1;
                        tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                    }
                    else if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebRevPatientName.ToString().ToLower().Trim())
                    {
                        result = result + 1;
                        tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                    }
                    else if (row[i]["FirstName"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                    {
                        result = result + 1;
                        tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                    }
                    //else if (row[i]["FirstName"].ToString().ToLower().Trim() == First_Name.Trim().ToLower())
                    //{
                    //    result = result + 1;
                    //    tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                    //}
                    //else if (row[i]["FirstName"].ToString().ToLower().Trim() == (TmpWebPatientName.ToString().IndexOf(" ") > 0 ? TmpWebPatientName.Substring(0, TmpWebPatientName.ToString().IndexOf(" ")).ToLower() : TmpWebPatientName))
                    //{
                    //    result = result + 1;
                    //    tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                    //}
                    if (tmpPatient_id != "0" && fromMobile)
                    {
                        break;
                    }
                }

                tmpPatient_Gur_id = "0";

                if (tmpPatient_id.ToString() == "0" || tmpPatient_id.ToString() == "")
                {
                    if (Last_Name.Trim() == null || Last_Name.Trim() == "")
                    {
                        string tmpPatientName = First_Name.Trim();
                        var firstSpaceIndex = tmpPatientName.IndexOf(" ");

                        if (tmpPatientName.Contains(" "))
                        {
                            tmpFirstName = tmpPatientName.Substring(0, firstSpaceIndex).ToString().Trim();
                            tmpLastName = tmpPatientName.Substring(firstSpaceIndex + 1).ToString().Trim();
                        }
                        else
                        {
                            tmpFirstName = First_Name.Trim();
                            tmpLastName = "Na";
                        }
                    }
                    else
                    {
                        tmpLastName = Last_Name.Trim();
                        tmpFirstName = First_Name.Trim();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetPatientEHRID(string FirstDateTime, DataTable PatientTable, string PatientEHRID, string MobileContact, string First_Name, string MI, string Last_Name, string Email, string DBConnectionString, string Clinic_number, DateTime DOB, string providerId)
        {
            try
            {
                if (PatientEHRID == "" || PatientEHRID == "0")
                {
                    bool status = false;
                    string tmpFirstName = "";
                    string tmpLastName = "";
                    string TmpWebPatientName = "";
                    Int64 tmpPatient_id_test = 0;
                    string TmpWebRevPatientName = "";
                    string tmpPatient_id = "";
                    string tmpPatient_Gur_id = "0";
                    Utility.CreatePatientNameTOCompare(First_Name.Trim(), Last_Name.Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);

                    if (Utility.Application_ID == 1)//eaglesoft
                    {
                        ComparePatientWithMobileAndEmail(PatientTable, MobileContact, Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);

                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            //*Commented for EagleSoft NSite*  Start
                            //tmpPatient_id = SynchEaglesoftBAL.Save_Patient_Local_To_EagleSoft(Last_Name, First_Name, MI, MobileContact, Email, providerId, FirstDateTime.ToString(), tmpPatient_Gur_id.ToString(), 0, DOB.ToShortDateString(), Clinic_number, DBConnectionString);
                            tmpPatient_id = SynchEaglesoftBAL.Save_Patient_Local_To_EagleSoft(Last_Name, First_Name, MI, MobileContact, Email, providerId, FirstDateTime.ToString(), tmpPatient_Gur_id.ToString(), 0, DOB.ToShortDateString(), DBConnectionString);
                            //*Commented for EagleSoft NSite*  End
                        }
                    }

                    else if (Utility.Application_ID == 2)
                    {
                        if (PatientTable.Columns.Contains("Guarantor"))
                        {
                            PatientTable.Columns["Guarantor"].ColumnName = "responsible_party";
                        }
                        ComparePatientWithMobileAndEmail(PatientTable, MobileContact, Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            tmpPatient_id = SynchOpenDentalBAL.Save_Patient_Local_To_OpenDental(Last_Name, First_Name, MI, MobileContact, Email, providerId, FirstDateTime, Convert.ToInt64(tmpPatient_Gur_id), DOB.ToShortDateString(), Clinic_number, DBConnectionString).ToString();
                        }
                    }
                    else if (Utility.Application_ID == 3)
                    {
                        if (PatientTable.Columns.Contains("Guarantor"))
                        {
                            PatientTable.Columns["Guarantor"].ColumnName = "responsible_party";
                        }
                        ComparePatientWithMobileAndEmail(PatientTable, MobileContact, Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            tmpPatient_id = SynchDentrixBAL.Save_Patient_Local_To_Dentrix(Last_Name, First_Name, MI, MobileContact, Email, providerId, FirstDateTime.ToString(), Convert.ToInt32(tmpPatient_Gur_id), DOB.ToShortDateString()).ToString();
                        }

                    }
                    else if (Utility.Application_ID == 5)
                    {
                        if (PatientTable.Columns.Contains("Guarantor"))
                        {
                            PatientTable.Columns["Guarantor"].ColumnName = "responsible_party";
                        }
                        ComparePatientWithMobileAndEmail(PatientTable, MobileContact, Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            // Utility.WriteToErrorLogFromAll("Going to save new patient in actual method");
                            tmpPatient_id = SynchClearDentBAL.Save_Patient_Local_To_ClearDent(Last_Name, First_Name, MI, MobileContact, Email, providerId, "", Convert.ToInt64(tmpPatient_Gur_id), DOB.ToShortDateString()).ToString();
                        }
                    }
                    else if (Utility.Application_ID == 6)
                    {
                        ComparePatientWithMobileAndEmail(PatientTable, MobileContact, Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);

                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            tmpPatient_id = SynchTrackerBAL.Save_Patient_Local_To_Tracker(Last_Name, First_Name, MI, MobileContact, Email, providerId, "", Convert.ToInt32(tmpPatient_Gur_id), 0, DOB.ToShortDateString()).ToString();
                        }
                    }
                    else if (Utility.Application_ID == 7)
                    {
                        ComparePatientWithMobileAndEmail(PatientTable, MobileContact.Replace("-", ""), Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);

                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            tmpPatient_id = SynchPracticeWorkBAL.Save_Patient_Local_To_PracticeWork(Last_Name, First_Name, MI, MobileContact.Replace("-", ""), Email, providerId, "", Convert.ToInt64(tmpPatient_Gur_id), 0, DOB.ToShortDateString()).ToString();
                        }
                    }
                    else if (Utility.Application_ID == 8)
                    {
                        if (PatientTable.Columns.Contains("Guar_id"))
                        {
                            PatientTable.Columns["Guar_id"].ColumnName = "responsible_party";
                        }
                        ComparePatientWithMobileAndEmail(PatientTable, SynchEasyDentalBAL.SetFormatePhoneNumber(MobileContact), Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            tmpPatient_id = SynchEasyDentalBAL.Save_Patient_Local_To_EasyDental(Last_Name, First_Name, MI, MobileContact, Email, providerId, FirstDateTime.ToString(), Convert.ToInt32(tmpPatient_Gur_id), DOB.ToShortDateString()).ToString();
                        }
                    }
                    else if (Utility.Application_ID == 10)
                    {
                        if (PatientTable.Columns.Contains("Guarantor"))
                        {
                            PatientTable.Columns["Guarantor"].ColumnName = "responsible_party";
                        }
                        ComparePatientWithMobileAndEmail(PatientTable, MobileContact, Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            tmpPatient_id = SynchPracticeWebBAL.Save_Patient_Local_To_PracticeWeb(Last_Name, First_Name, MI, MobileContact, Email, providerId, "", Convert.ToInt64(tmpPatient_Gur_id), DOB.ToShortDateString(), Clinic_number, DBConnectionString).ToString();
                        }
                    }
                    else if (Utility.Application_ID == 11)
                    {
                        if (PatientTable.Columns.Contains("Guarantor"))
                        {
                            PatientTable.Columns["Guarantor"].ColumnName = "responsible_party";
                        }
                        ComparePatientWithMobileAndEmail(PatientTable, MobileContact, Email, TmpWebPatientName, TmpWebRevPatientName, First_Name, Last_Name, DOB, ref tmpPatient_id, ref tmpPatient_Gur_id, ref tmpFirstName, ref tmpLastName);
                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                        {
                            tmpPatient_id = SynchAbelDentBAL.Save_Patient_Local_To_AbelDent(Last_Name, First_Name, MI, ref tmpPatient_id_test, MobileContact, Email, providerId, "", Convert.ToInt32(tmpPatient_Gur_id), "", DOB.ToShortDateString()).ToString();
                        }
                    }

                    GoalBase.WriteToSyncLogFile_Static("[PatientPayment : Patient Created in EHR with PatientEHRId : " + tmpPatient_id_test + " Name : " + First_Name + " " + MI + " " + Last_Name + " Mobile : MobileContact " + MobileContact);
                    return tmpPatient_id;
                }
                else
                {
                    GoalBase.WriteToErrorLogFile_Static("[PatientPayment : Error while Patient Create in EHR of Name : " + First_Name + " " + MI + " " + Last_Name + " Mobile : MobileContact " + MobileContact);
                    return PatientEHRID;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in compare existing patient or save new patient " + ex.Message);
                return PatientEHRID;
                throw;
            }

        }
        public static void SynchDataLiveDB_Pull_PatientPaymentSMSCall()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)// && Utility.AditLocationSyncEnable
                {
                    DataTable dtLivePatientSMSCallLog = new DataTable();
                    DataTable dtLocalPatientSMSCallLog = new DataTable();
                    DataTable dtLocalPatient = SynchLocalBAL.GetAllLocalActivePatientData();
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        //GoalBase.WriteToSyncLogFile_Static(" Call PatientPaymentSMSCall API " + Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"].ToString());
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            dtLivePatientSMSCallLog = new DataTable();
                            //dtLocalPatientSMSCallLog = new DataTable();
                            string strApiProviders = PullLiveDatabaseBAL.GetLiveRecord("patientsmscalllog", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                            //Utility.WriteToErrorLogFromAll("patientsmscalllog string url  "+strApiProviders);
                            var client = new RestClient(strApiProviders);
                            Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Call patientsmscall API");
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.POST);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));

                            //GoalBase.WriteToSyncLogFile_Static(" Token : " + Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));

                            string jsonstringn = "{\"locationId\":\"" + Utility.DtLocationList.Rows[j]["Loc_Id"].ToString() + "\",\"pageNo\":1,\"limit\":" + GoalBase.intervalEHRSynch_CallSmsLog.ToString() + ",\"ehrsyncstatus\":[\"pending\"]}";
                            request.AddParameter("application/json", jsonstringn, ParameterType.RequestBody);
                            Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Request Sent into the API  " + " Authorization, TokenKey, action & Json Request " + jsonstringn.ToString() );
                            IRestResponse response = client.Execute(request);
                            //Utility.WriteToErrorLogFromAll("patientsmscalllog response is =  " + response.Content.ToString());
                            // GoalBase.WriteToSyncLogFile_Static(" API : " + strApiProviders);
                            //GoalBase.WriteToSyncLogFile_Static(" json string : " + jsonstringn);

                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                { }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientSMSCallLog Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Response received from API (" + response.Content.ToString() + ")");

                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Response is null");
                            }
                            var ProvidersDto = JsonConvert.DeserializeObject<Pull_PatientSMSCallLog>(response.Content);

                            //GoalBase.WriteToErrorLogFile_Static(" response.Content : " + response.Content.ToString());

                            if (ProvidersDto.message.ToString().ToUpper() == "EHR SYNC FOR LOGS  IS DISABLED!")
                            {
                                GoalBase.WriteToSyncLogFile_Static("PatientSMSCallLog Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : EHR sync for logs  is disabled!.");
                            }
                            else
                            {
                                double RemainPaging = 0;
                                if (ProvidersDto != null && ProvidersDto.data != null)
                                {
                                    dtLivePatientSMSCallLog = SynchLocalBAL.GetLocalPatientWiseSMSCallLog(true, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), 0, Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call);
                                    if (!dtLivePatientSMSCallLog.Columns.Contains("InsUptDlt"))
                                    {
                                        dtLivePatientSMSCallLog.Columns.Add("InsUptDlt", typeof(string));
                                    }

                                    if (!dtLivePatientSMSCallLog.Columns.Contains("Log_Status"))
                                    {
                                        dtLivePatientSMSCallLog.Columns.Add("Log_Status", typeof(string));
                                    }

                                    Int32 TotalLength = ProvidersDto.totalLength;
                                    foreach (var item in ProvidersDto.data)
                                    {
                                        try
                                        {


                                            if (item.patientId != null && item.patientId != "" && !string.IsNullOrEmpty(item.patientId))
                                            {
                                                DataRow RowPro = dtLivePatientSMSCallLog.NewRow();
                                                RowPro["Patient_Web_ID"] = item.patientId;
                                                RowPro["PatientSMSCallLogWebId"] = item.smsId;
                                                RowPro["esId"] = item.esId;
                                                RowPro["smsId"] = item.smsId;
                                                RowPro["PatientMobile"] = item.patientMobile;//item.ProviderEHRId;
                                                                                             //var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToDouble(item.paid_date) / 1000d)).ToLocalTime();
                                                RowPro["PatientSMSCallLogDate"] = Convert.ToDateTime(item.created_at_date_time);// dt.ToString(); //Uti;lity.DateTryParse(item.paid_date,"yyyy/MM/dd");   
                                                RowPro["ProviderEHRId"] = "";
                                                RowPro["app_alias"] = item.description;
                                                RowPro["text"] = item.text;
                                                RowPro["message_direction"] = item.message_direction;
                                                RowPro["message_type"] = item.message_type;
                                                RowPro["LogReceivedLocal"] = 1;
                                                RowPro["LogEntryDatetimeLocal"] = DateTime.Now.ToString();
                                                RowPro["LogUpdatedEHR"] = false;
                                                RowPro["LogUpdatedEHRDateTime"] = DBNull.Value;
                                                RowPro["LogStatusCompletedAdit"] = 0;
                                                RowPro["LogStatusCompletedDateTime"] = DBNull.Value;
                                                RowPro["LogEHRId"] = "";
                                                RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                //RowPro["PatientEHRId"] = dtLocalPatient.AsEnumerable().Where(o => o.Field<object>("Patient_Web_ID").ToString() == item.patientId.ToString()).Select(o => o.Field<object>("Patient_EHR_id")).FirstOrDefault();
                                                //if (RowPro["PatientEHRId"] == null)
                                                //{
                                                //GoalBase.WriteToSyncLogFile_Static("1. Patient Mobile " + item.patientMobile.ToString() + " SMS ID " + item.smsId);
                                                if (item.patientMobile.ToString() == string.Empty)
                                                {
                                                    //GoalBase.WriteToSyncLogFile_Static("2. Patient Mobile " + item.patientMobile.ToString() + " SMS ID " + item.smsId);
                                                    //GoalBase.WriteToSyncLogFile_Static("Set  Patient's Mobile Number is blank " + item.smsId);
                                                    RowPro["Log_Status"] = "Patient's Mobile Number is blank";
                                                    dtLivePatientSMSCallLog.Rows.Add(RowPro);
                                                }
                                                else
                                                {
                                                    //GoalBase.WriteToSyncLogFile_Static("3. Patient Mobile " + item.patientMobile.ToString() + " SMS ID " + item.smsId);
                                                    var drAllPatient = dtLocalPatient.AsEnumerable().Where(o => Utility.ConvertContactNumber(o.Field<object>("Mobile").ToString()) == Utility.ConvertContactNumber(item.patientMobile.ToString()));
                                                    //GoalBase.WriteToSyncLogFile_Static("3.1 Records Found " + drAllPatient.Count().ToString() + " Patient Mobile "  + item.patientMobile.ToString() + " SMS ID " + item.smsId);

                                                    if (drAllPatient != null && drAllPatient.Count() > 0 && item.patientMobile.ToString() != string.Empty)
                                                    {
                                                        //GoalBase.WriteToSyncLogFile_Static("5. Patient Mobile " + item.patientMobile.ToString() + " SMS ID " + item.smsId);
                                                        int i = 0;
                                                        foreach (DataRow dr in drAllPatient)
                                                        {
                                                            if (i == 0)
                                                            {
                                                                //GoalBase.WriteToSyncLogFile_Static("6. Patient Mobile " + item.patientMobile.ToString() + " SMS ID " + item.smsId);
                                                                RowPro["PatientEHRId"] = dr["Patient_EHR_id"];
                                                                dtLivePatientSMSCallLog.Rows.Add(RowPro);
                                                            }
                                                            else
                                                            {
                                                                //GoalBase.WriteToSyncLogFile_Static("8. Patient Mobile " + item.patientMobile.ToString() + " SMS ID " + item.smsId);
                                                                DataRow dtP = dtLivePatientSMSCallLog.NewRow();
                                                                dtP.ItemArray = RowPro.ItemArray;
                                                                dtP["PatientEHRId"] = dr["Patient_EHR_id"];
                                                                dtLivePatientSMSCallLog.Rows.Add(dtP);
                                                            }
                                                            i++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //GoalBase.WriteToSyncLogFile_Static("4. Patient Mobile " + item.patientMobile.ToString() + " SMS ID " + item.smsId);
                                                        //GoalBase.WriteToSyncLogFile_Static("Set  Patient's Mobile Not Exists in EHR " + item.smsId);
                                                        RowPro["Log_Status"] = "Patient's Mobile Not Exists in EHR";
                                                        dtLivePatientSMSCallLog.Rows.Add(RowPro);
                                                    }
                                                }
                                                dtLivePatientSMSCallLog.AcceptChanges();
                                                //GoalBase.WriteToSyncLogFile_Static("7. DataTable Count " + dtLivePatientSMSCallLog.Rows.Count.ToString());
                                                //}
                                                //else
                                                //{
                                                //    dtLivePatientSMSCallLog.Rows.Add(RowPro);
                                                //    dtLivePatientSMSCallLog.AcceptChanges();
                                                //}

                                            }
                                        }
                                        catch (Exception)
                                        {

                                        }
                                    }
                                    RemainPaging = Math.Ceiling(Convert.ToDouble(TotalLength) / Convert.ToDouble(GoalBase.intervalEHRSynch_CallSmsLog));

                                }
                                if (dtLivePatientSMSCallLog != null)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "dtLivePatientSMSCallLog count " + dtLivePatientSMSCallLog.Rows.Count.ToString());
                                }
                                if (RemainPaging > 0)
                                {
                                    for (double i = 2; i <= RemainPaging; i++)
                                    {
                                        strApiProviders = PullLiveDatabaseBAL.GetLiveRecord("patientsmscalllog", Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                                        client = new RestClient(strApiProviders);
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Call ApiProviders (patientsmscalllog) Confirmation API");
                                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                        request = new RestRequest(Method.POST);
                                        ServicePointManager.Expect100Continue = true;
                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                        request.AddHeader("cache-control", "no-cache");
                                        request.AddHeader("content-type", "application/json");
                                        request.AddHeader("Authorization", Utility.WebAdminUserToken);
                                        request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                        jsonstringn = "{\"locationId\":\"" + Utility.DtLocationList.Rows[j]["Loc_Id"].ToString() + "\",\"pageNo\":" + i.ToString() + ",\"limit\":" + GoalBase.intervalEHRSynch_CallSmsLog.ToString() + ",\"ehrsyncstatus\":[\"pending\"]}";
                                        request.AddParameter("application/json", jsonstringn, ParameterType.RequestBody);
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Request Sent into the API"  + " Authorization, TokenKey, action & Json Request " + jsonstringn.ToString());;
                                        response = client.Execute(request);
                                        if (response.ErrorMessage != null)
                                        {
                                            if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                            { }
                                            else
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[PatientSMSCallLog Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                            }
                                            return;
                                        }
                                        if (response.Content != null)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Response received from API (" + response.Content.ToString() + ")");
                                        }
                                        else
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Response is null");
                                        }
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "----------------------------Deserialize repsonse--------------------------------");
                                        ProvidersDto = JsonConvert.DeserializeObject<Pull_PatientSMSCallLog>(response.Content);
                                        if (ProvidersDto != null && ProvidersDto.data != null)
                                        {
                                            if (ProvidersDto.data.Count == 0)
                                            {
                                                Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Deserialize response ProvidersDto count :  (" + ProvidersDto.data.Count + ") no record ");
                                            }
                                            // Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "Deserialize repsonse from API(" + ProvidersDto.message.ToString() + ")");
                                            foreach (var item in ProvidersDto.data)
                                            {
                                                if (item.patientId != null && item.patientId != "" && !string.IsNullOrEmpty(item.patientId))
                                                {
                                                    DataRow RowPro = dtLivePatientSMSCallLog.NewRow();

                                                    //RowPro["PatientEHRId"] = dtLocalPatient.AsEnumerable().Where(o => o.Field<object>("Patient_Web_ID").ToString() == item.patientId.ToString()).Select(o => o.Field<object>("Patient_EHR_id")).FirstOrDefault();
                                                    RowPro["Patient_Web_ID"] = item.patientId;
                                                    RowPro["PatientSMSCallLogWebId"] = item.smsId;
                                                    RowPro["esId"] = item.esId;
                                                    RowPro["smsId"] = item.smsId;
                                                    RowPro["PatientMobile"] = item.patientMobile;//item.ProviderEHRId;
                                                                                                 //var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToDouble(item.paid_date) / 1000d)).ToLocalTime();
                                                    RowPro["PatientSMSCallLogDate"] = Convert.ToDateTime(item.created_at_date_time);// dt.ToString(); //Uti;lity.DateTryParse(item.paid_date,"yyyy/MM/dd");   
                                                    RowPro["ProviderEHRId"] = "";
                                                    RowPro["app_alias"] = item.description;
                                                    RowPro["text"] = item.text;
                                                    RowPro["message_direction"] = item.message_direction;
                                                    RowPro["message_type"] = item.message_type;
                                                    RowPro["LogReceivedLocal"] = 1;
                                                    RowPro["LogEntryDatetimeLocal"] = DateTime.Now.ToString();
                                                    RowPro["LogUpdatedEHR"] = false;
                                                    RowPro["LogUpdatedEHRDateTime"] = DBNull.Value;
                                                    RowPro["LogStatusCompletedAdit"] = false;
                                                    RowPro["LogStatusCompletedDateTime"] = DBNull.Value;
                                                    RowPro["LogEHRId"] = "";
                                                    RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                    //RowPro["PatientEHRId"] = dtLocalPatient.AsEnumerable().Where(o => o.Field<object>("Patient_Web_ID").ToString() == item.patientId.ToString()).Select(o => o.Field<object>("Patient_EHR_id")).FirstOrDefault();
                                                    //if (RowPro["PatientEHRId"] == null)
                                                    //{
                                                    if (item.patientMobile.ToString() == string.Empty)
                                                    {
                                                        //GoalBase.WriteToSyncLogFile_Static("Set  Patient's Mobile Number is blank " + item.smsId);
                                                        RowPro["Log_Status"] = "Patient's Mobile Number is blank";
                                                        dtLivePatientSMSCallLog.Rows.Add(RowPro);
                                                    }
                                                    else
                                                    {
                                                        var drAllPatient = dtLocalPatient.AsEnumerable().Where(o => Utility.ConvertContactNumber(o.Field<object>("Mobile").ToString()) == Utility.ConvertContactNumber(item.patientMobile.ToString()));
                                                        if (drAllPatient != null && drAllPatient.Count() > 0 && item.patientMobile.ToString() != string.Empty)
                                                        {
                                                            int K = 0;
                                                            foreach (DataRow dr in drAllPatient)
                                                            {
                                                                if (K == 0)
                                                                {
                                                                    RowPro["PatientEHRId"] = dr["Patient_EHR_id"];
                                                                    dtLivePatientSMSCallLog.Rows.Add(RowPro);
                                                                }
                                                                else
                                                                {
                                                                    DataRow dtP = dtLivePatientSMSCallLog.NewRow();
                                                                    dtP.ItemArray = RowPro.ItemArray;
                                                                    dtP["PatientEHRId"] = dr["Patient_EHR_id"];
                                                                    dtLivePatientSMSCallLog.Rows.Add(dtP);
                                                                }
                                                                K++;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //GoalBase.WriteToSyncLogFile_Static("Set  Patient's Mobile Not Exists in EHR " + item.smsId);
                                                            RowPro["Log_Status"] = "Patient's Mobile Not Exists in EHR";
                                                            dtLivePatientSMSCallLog.Rows.Add(RowPro);
                                                        }
                                                    }
                                                    dtLivePatientSMSCallLog.AcceptChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (dtLivePatientSMSCallLog != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call, "data table dtLivePatientSMSCallLog count  (" + dtLivePatientSMSCallLog.Rows.Count + ")");
                            }
                            dtLocalPatientSMSCallLog = SynchLocalBAL.GetLocalPatientWiseSMSCallLog(false, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), 0, Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call);

                            foreach (DataRow dtDtxRow in dtLivePatientSMSCallLog.Rows)
                            {

                                DataRow row = dtLocalPatientSMSCallLog.Copy().Select("PatientSMSCallLogWebId = '" + dtDtxRow["PatientSMSCallLogWebId"].ToString().Trim() + "' and PatientEHRId = '" + dtDtxRow["PatientEHRId"].ToString().Trim() + "'").FirstOrDefault();
                                if (row != null)
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 1;
                                }
                            }

                            dtLivePatientSMSCallLog.AcceptChanges();
                            //GoalBase.WriteToSyncLogFile_Static(" Records to be updated "  + dtLivePatientSMSCallLog.Rows.Count.ToString());
                            if (dtLivePatientSMSCallLog != null && dtLivePatientSMSCallLog.Rows.Count > 0)
                            {
                                bool status = PullLiveDatabaseBAL.Save_PatientSMSCallLog_To_Local(dtLivePatientSMSCallLog, Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call);
                                if (status)
                                {
                                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientSMSCallLog");
                                    GoalBase.WriteToSyncLogFile_Static("PatientSMSCallLog Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                }
                                //GoalBase.WriteToSyncLogFile_Static(" Call Function ");
                                SynchLocalBAL.CallPatientSMSCallAPIForStatusCompleted(dtLivePatientSMSCallLog, "readytoimport", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_ID"].ToString(), Utility._filename_EHR_patient_sms_call, Utility._EHRLogdirectory_EHR_patient_sms_call);
                                //GoalBase.WriteToSyncLogFile_Static(" Called Function ");
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PatientSMSCallLog Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        public static void SynchDataLiveDB_Pull_PatientFollowUp()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)// && Utility.AditLocationSyncEnable
                {
                    DataTable dtLivePatientFollowup = new DataTable();
                    DataTable dtLocalPatientFollowUp = new DataTable();
                    DataTable dtLocalPatient = SynchLocalBAL.GetAllLocalPatientData();
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            dtLivePatientFollowup = new DataTable();
                            string strApiRequest = PullLiveDatabaseBAL.GetLiveRecord("patientfollowup", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Call patientfollowup API");
                            strApiRequest = strApiRequest.Replace("limit=10", "limit=" + GoalBase.intervalEHRSynch_PatientFollowUp.ToString() + "");
                            var client = new RestClient(strApiRequest);
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            var request = new RestRequest(Method.GET);
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("Authorization", Utility.WebAdminUserToken);
                            request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Request Sent into the API  " + " Authorization, TokenKey & action");
                            IRestResponse response = client.Execute(request);

                            //GoalBase.WriteToSyncLogFile_Static(" Followup API : " + strApiRequest);
                            //GoalBase.WriteToSyncLogFile_Static("Followup response : " + response.Content.ToString());

                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                { }
                                else
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[PatientFollowUp Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                }
                                return;
                            }
                            if (response.Content != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Response received from API (" + response.Content.ToString() + ")");

                            }
                            else
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Response is null");
                            }
                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "----------------------------Deserialize repsonse--------------------------------");
                            var ProvidersDto = JsonConvert.DeserializeObject<Pull_PatientFollowUp>(response.Content);

                            if (ProvidersDto.message.ToString().ToUpper() == "EHR SYNC FOR LOGS  IS DISABLED!")
                            {
                                GoalBase.WriteToSyncLogFile_Static("PatientFollowUp Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " : EHR sync for logs  is disabled!.");
                            }
                            else
                            {
                                double RemainPaging = 0;
                                int calllimit = 0;
                                if (ProvidersDto != null && ProvidersDto.data != null)
                                {
                                    if (ProvidersDto.data.Count == 0)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Deserialize response ProvidersDto count :  (" + ProvidersDto.data.Count + ") no record ");
                                    }
                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Deserialize repsonse from API(" + ProvidersDto.message.ToString() + ")");
                                    dtLivePatientFollowup = SynchLocalBAL.GetLocalPatientWiseSMSCallLog(true, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), 1);
                                    if (!dtLivePatientFollowup.Columns.Contains("InsUptDlt"))
                                    {
                                        dtLivePatientFollowup.Columns.Add("InsUptDlt", typeof(string));
                                    }
                                    if (!dtLivePatientFollowup.Columns.Contains("Log_Status"))
                                    {
                                        dtLivePatientFollowup.Columns.Add("Log_Status", typeof(string));
                                    }
                                    Int32 TotalLength = ProvidersDto.totalLength;
                                    foreach (var item in ProvidersDto.data)
                                    {
                                        if (item.patient._ref != null && item.patient._ref != "" && !string.IsNullOrEmpty(item.patient._ref))
                                        {
                                            DataRow RowPro = dtLivePatientFollowup.NewRow();
                                            RowPro["Patient_Web_ID"] = item.patient._ref;
                                            RowPro["PatientSMSCallLogWebId"] = item._id;
                                            RowPro["esId"] = item._id;
                                            RowPro["smsId"] = item._id;
                                            RowPro["PatientMobile"] = "";
                                            RowPro["PatientSMSCallLogDate"] = Convert.ToDateTime(item.created_at);// dt.ToString(); //Uti;lity.DateTryParse(item.paid_date,"yyyy/MM/dd");   
                                            RowPro["ProviderEHRId"] = "";
                                            RowPro["app_alias"] = item.title;
                                            if (item.message != null && item.message != "")
                                            {
                                                RowPro["text"] = item.message;
                                            }
                                            else
                                            {
                                                RowPro["text"] = item.title;
                                                //RowPro["Log_Status"] = "Follow up message is blank";
                                            }
                                            RowPro["message_direction"] = "followup";
                                            RowPro["message_type"] = "followup";
                                            RowPro["LogReceivedLocal"] = 1;
                                            RowPro["LogEntryDatetimeLocal"] = DateTime.Now.ToString();
                                            RowPro["LogUpdatedEHR"] = false;
                                            RowPro["LogUpdatedEHRDateTime"] = DBNull.Value;
                                            RowPro["LogStatusCompletedAdit"] = 0;
                                            RowPro["LogStatusCompletedDateTime"] = DBNull.Value;
                                            RowPro["LogEHRId"] = "";
                                            RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                            RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                            RowPro["LogType"] = 1;

                                            if (item.patient._ref.ToString() == string.Empty)
                                            {
                                                RowPro["Log_Status"] = "Patient Web Id is Blank";
                                                dtLivePatientFollowup.Rows.Add(RowPro);
                                            }
                                            else
                                            {
                                                var drAllPatient = dtLocalPatient.AsEnumerable().Where(o => o.Field<object>("Patient_Web_ID").ToString() == item.patient._ref.ToString());
                                                if (drAllPatient != null && drAllPatient.Count() > 0 && item.patient._ref.ToString() != string.Empty)
                                                {
                                                    RowPro["PatientEHRId"] = drAllPatient.First().Field<string>("Patient_EHR_id").ToString();
                                                    dtLivePatientFollowup.Rows.Add(RowPro);
                                                }
                                                else
                                                {
                                                    RowPro["Log_Status"] = "Patient not exists in EHR";
                                                    dtLivePatientFollowup.Rows.Add(RowPro);
                                                }
                                            }
                                            dtLivePatientFollowup.AcceptChanges();
                                        }
                                    }
                                    RemainPaging = Math.Ceiling(Convert.ToDouble(TotalLength) / Convert.ToDouble(GoalBase.intervalEHRSynch_PatientFollowUp));
                                    calllimit = GoalBase.intervalEHRSynch_PatientFollowUp;
                                }

                                if (dtLivePatientFollowup != null)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "data table dtLivePatientFollowup count  (" + dtLivePatientFollowup.Rows.Count + ")");
                                }
                                if (RemainPaging > 0)
                                {
                                    for (double i = 2; i <= RemainPaging; i++)
                                    {
                                        strApiRequest = PullLiveDatabaseBAL.GetLiveRecord("patientfollowup", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Call patientfollowup API");
                                        strApiRequest = strApiRequest.Replace("limit=10", "limit=" + GoalBase.intervalEHRSynch_PatientFollowUp.ToString() + "");
                                        strApiRequest = strApiRequest.Replace("skip=0", "skip=" + calllimit.ToString() + "");
                                        client = new RestClient(strApiRequest);
                                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                        request = new RestRequest(Method.GET);
                                        ServicePointManager.Expect100Continue = true;
                                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                        request.AddHeader("cache-control", "no-cache");
                                        request.AddHeader("content-type", "application/json");
                                        request.AddHeader("Authorization", Utility.WebAdminUserToken);
                                        request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Request Sent into the API  " + " Authorization, TokenKey & action");

                                        response = client.Execute(request);
                                        if (response.Content != null)
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Response received from API (" + response.Content.ToString() + ")");
                                        }
                                        else
                                        {
                                            Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Response is null");
                                        }
                                        if (response.ErrorMessage != null)
                                        {
                                            if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                            { }
                                            else
                                            {
                                                GoalBase.WriteToErrorLogFile_Static("[PatientSMSCallLog Sync (Adit Server To Local Database)] Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "  " + response.ErrorMessage);
                                            }
                                            return;
                                        }
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "----------------------------Deserialize repsonse--------------------------------");

                                        ProvidersDto = JsonConvert.DeserializeObject<Pull_PatientFollowUp>(response.Content);
                                        if (ProvidersDto != null && ProvidersDto.data != null)
                                        {
                                            if (ProvidersDto.data.Count == 0)
                                            {
                                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "Deserialize response ProvidersDto count :  (" + ProvidersDto.data.Count + ") no record ");
                                            }
                                            foreach (var item in ProvidersDto.data)
                                            {
                                                if (item.patient._ref != null && item.patient._ref != "" && !string.IsNullOrEmpty(item.patient._ref))
                                                {
                                                    DataRow RowPro = dtLivePatientFollowup.NewRow();
                                                    RowPro["Patient_Web_ID"] = item.patient._ref;
                                                    RowPro["PatientSMSCallLogWebId"] = item._id;
                                                    RowPro["esId"] = item._id;
                                                    RowPro["smsId"] = item._id;
                                                    RowPro["PatientMobile"] = "";
                                                    RowPro["PatientSMSCallLogDate"] = Convert.ToDateTime(item.created_at);// dt.ToString(); //Uti;lity.DateTryParse(item.paid_date,"yyyy/MM/dd");   
                                                    RowPro["ProviderEHRId"] = "";
                                                    RowPro["app_alias"] = item.title;
                                                    if (item.message != null && item.message != "")
                                                    {
                                                        RowPro["text"] = item.message;
                                                    }
                                                    else
                                                    {
                                                        RowPro["text"] = item.title;
                                                        // RowPro["Log_Status"] = "Follow up message is blank";
                                                    }
                                                    RowPro["message_direction"] = "followup";
                                                    RowPro["message_type"] = "followup";
                                                    RowPro["LogReceivedLocal"] = 1;
                                                    RowPro["LogEntryDatetimeLocal"] = DateTime.Now.ToString();
                                                    RowPro["LogUpdatedEHR"] = false;
                                                    RowPro["LogUpdatedEHRDateTime"] = DBNull.Value;
                                                    RowPro["LogStatusCompletedAdit"] = 0;
                                                    RowPro["LogStatusCompletedDateTime"] = DBNull.Value;
                                                    RowPro["LogEHRId"] = "";
                                                    RowPro["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                                    RowPro["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                                    RowPro["LogType"] = 1;

                                                    if (item.patient._ref.ToString() == string.Empty)
                                                    {
                                                        RowPro["Log_Status"] = "Patient Web Id is Blank";
                                                        dtLivePatientFollowup.Rows.Add(RowPro);
                                                    }
                                                    else
                                                    {
                                                        var drAllPatient = dtLocalPatient.AsEnumerable().Where(o => o.Field<object>("Patient_Web_ID").ToString() == item.patient._ref.ToString());
                                                        if (drAllPatient != null && drAllPatient.Count() > 0 && item.patient._ref.ToString() != string.Empty)
                                                        {
                                                            RowPro["PatientEHRId"] = drAllPatient.First().Field<string>("Patient_EHR_id").ToString();
                                                            dtLivePatientFollowup.Rows.Add(RowPro);
                                                        }
                                                        else
                                                        {
                                                            RowPro["Log_Status"] = "Patient not exists in EHR";
                                                            dtLivePatientFollowup.Rows.Add(RowPro);
                                                        }
                                                    }
                                                    dtLivePatientFollowup.AcceptChanges();

                                                }
                                            }
                                        }
                                        calllimit = calllimit + GoalBase.intervalEHRSynch_PatientFollowUp;
                                    }
                                }
                            }


                            dtLocalPatientFollowUp = SynchLocalBAL.GetLocalPatientWiseSMSCallLog(false, Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), 1, Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp);
                            if (dtLocalPatientFollowUp != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "GetLocalPatientWiseSMSCallLog dtLocalPatientFollowUp count :  (" + dtLivePatientFollowup.Rows.Count + ") ");
                            }


                            foreach (DataRow dtDtxRow in dtLivePatientFollowup.Rows)
                            {
                                DataRow row = dtLocalPatientFollowUp.Copy().Select("PatientSMSCallLogWebId = '" + dtDtxRow["PatientSMSCallLogWebId"].ToString().Trim() + "' and Patient_Web_ID = '" + dtDtxRow["Patient_Web_ID"].ToString().Trim() + "'").FirstOrDefault();
                                if (row != null)
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 1;
                                }
                            }

                            dtLivePatientFollowup.AcceptChanges();
                            if (dtLivePatientFollowup != null)
                            {
                                Utility.WriteSyncPullLog(Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp, "dtLivePatientFollowup count :  (" + dtLivePatientFollowup.Rows.Count + ") ");
                            }
                            if (dtLivePatientFollowup != null && dtLivePatientFollowup.Rows.Count > 0)
                            {
                                bool status = PullLiveDatabaseBAL.Save_PatientSMSCallLog_To_Local(dtLivePatientFollowup);
                                if (status)
                                {
                                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientFollowup");
                                    GoalBase.WriteToSyncLogFile_Static("PatientSMSCallLog Sync (Adit Server To Local Database) Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                }
                                SynchLocalBAL.CallPatientFollowUpStatusCompleted(dtLivePatientFollowup, "readytoimport", Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_ID"].ToString(), Utility._filename_EHR_PatientFollowUp, Utility._EHRLogdirectory_EHR_PatientFollowUp);
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PatientFollowup Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }


        #region patientoptout

        public static void SynchDataLiveDB_Pull_EHR_Patientoptout()
        {
            return;
            try
            {
                GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_Patientoptout Start");
                if (Utility.IsApplicationIdleTimeOff)  //&& Utility.AditLocationSyncEnable
                {
                    // GoalBase.WriteToSyncLogFile_Static("SynchDataLiveDB_Pull_EHR_appointment IsApplicationIdleTimeOff True");
                    for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                    {
                        if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]))
                        {
                            if (Convert.ToBoolean(Utility.DtLocationList.Rows[j]["ApptAutoBook"].ToString()))
                            {
                                string strApiappointment = PullLiveDatabaseBAL.GetLiveRecord("patientoptout", Utility.DtLocationList.Rows[j]["Loc_Id"].ToString());
                                var client = new RestClient(strApiappointment);
                                Utility.WriteSyncPullLog(Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout, "Call patientoptout API");
                                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                                var request = new RestRequest(Method.GET);
                                ServicePointManager.Expect100Continue = true;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                request.AddHeader("cache-control", "no-cache");
                                request.AddHeader("content-type", "application/json");
                                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                                Utility.WriteSyncPullLog(Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout, "Request Sent into the API " + " Authorization, TokenKey & action");
                                IRestResponse response = client.Execute(request);
                                if (response.Content != null)
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout, "Response received from API(" + response.Content.ToString() + ")");
                                }
                                else
                                {
                                    Utility.WriteSyncPullLog(Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout, "Response is null");
                                }
                                if (response.ErrorMessage != null)
                                {
                                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[EHR_patientoptout Adit App to EHR Err  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "   " + response.ErrorMessage);
                                    }
                                    else
                                    {
                                        GoalBase.WriteToErrorLogFile_Static("[EHR_patientoptout Adit App to EHR Err  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "   " + response.ErrorMessage);
                                    }
                                    return;
                                }
                                else if (response.Content.ToString().ToUpper() == "NOT FOUND")
                                {
                                    GoalBase.WriteToErrorLogFile_Static("[EHR_patientoptout Adit App to Response  : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic  : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + "   " + response.Content.ToString());
                                    return;
                                }
                                Utility.WriteSyncPullLog(Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout, "----------------------------Deserialize repsonse--------------------------------");

                                var EHR_appointmentDto = JsonConvert.DeserializeObject<Pull_Patient_OptOutBO>(response.Content);

                                if (EHR_appointmentDto != null && EHR_appointmentDto.data != null && EHR_appointmentDto.data.Count > 0)
                                {
                                    if (EHR_appointmentDto.data.Count == 0)
                                    {
                                        Utility.WriteSyncPullLog(Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout, "Deserialize response EHR_appointmentDto count :  (" + EHR_appointmentDto.data.Count + ") no record ");
                                    }
                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout, "Deserialize repsonse from API(" + EHR_appointmentDto.message.ToString() + ")");
                                    DataTable dtLiveEHR_appointment = new DataTable();
                                    dtLiveEHR_appointment.Columns.Add("patient_ehr_id", typeof(int));
                                    dtLiveEHR_appointment.Columns.Add("receive_sms", typeof(string));
                                    dtLiveEHR_appointment.Columns.Add("esid", typeof(string));
                                    dtLiveEHR_appointment.Columns.Add("patientid", typeof(string));
                                    dtLiveEHR_appointment.Columns.Add("InsUptDlt", typeof(int));
                                    dtLiveEHR_appointment.Columns.Add("Clinic_Number", typeof(string));
                                    dtLiveEHR_appointment.Columns.Add("Service_Install_ID", typeof(string));

                                    foreach (var item in EHR_appointmentDto.data)
                                    {

                                        if (item.receive_sms != null && item.receive_sms != "" && item.patient_ehr_id != null && item.patient_ehr_id != "0")
                                        {
                                            DataRow RowAppt = dtLiveEHR_appointment.NewRow();
                                            RowAppt["patient_ehr_id"] = item.patient_ehr_id;
                                            RowAppt["receive_sms"] = item.receive_sms;
                                            RowAppt["esid"] = item.esid;
                                            RowAppt["patientid"] = item.patientid;
                                            RowAppt["Clinic_Number"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                                            RowAppt["Service_Install_Id"] = Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString();
                                            dtLiveEHR_appointment.Rows.Add(RowAppt);
                                            dtLiveEHR_appointment.AcceptChanges();
                                        }

                                    }
                                    //Utility.WriteSyncPullLog(Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout, "data table dtLiveEHR_appointment count  (" + dtLiveEHR_appointment.Rows.Count + ")");
                                    if (dtLiveEHR_appointment != null && dtLiveEHR_appointment.Rows.Count > 0)
                                    {
                                        bool status = false;

                                        if (Utility.Application_ID == 1)
                                        {
                                            status = SynchEaglesoftBAL.Update_Receive_SMS_Patient_EHR_Live_To_Eaglesoft(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout);
                                        }
                                        else if (Utility.Application_ID == 2)
                                        {
                                            status = SynchOpenDentalBAL.Update_Receive_SMS_Patient_EHR_Live_To_Opendental(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString() , Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout);
                                        }
                                        else if (Utility.Application_ID == 3)
                                        {
                                            status = SynchDentrixBAL.Update_Receive_SMS_Patient_EHR_Live_To_DentrixEHR(dtLiveEHR_appointment, Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout);
                                        }
                                        else if (Utility.Application_ID == 5)
                                        {
                                            status = SynchClearDentBAL.Update_Receive_SMS_Patient_EHR_Live_To_ClearDentEHR(dtLiveEHR_appointment, Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout);
                                        }
                                        else if (Utility.Application_ID == 6)
                                        {
                                            status = SynchTrackerBAL.Update_Receive_SMS_Patient_EHR_Live_To_TrackerEHR(dtLiveEHR_appointment, Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout);
                                        }
                                        else if (Utility.Application_ID == 7)
                                        {
                                            // status = SynchPracticeWorkBAL.Update_Status_EHR_Appointment_Live_To_PracticeWorkEHR(dtLiveEHR_appointment, Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
                                        }
                                        else if (Utility.Application_ID == 8)
                                        {
                                            status = SynchEasyDentalBAL.Update_Receive_SMS_Patient_EHR_Live_To_EasyDentalEHR(dtLiveEHR_appointment, Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout);
                                        }
                                        else if (Utility.Application_ID == 10)
                                        {
                                            status = SynchPracticeWebBAL.Update_Receive_SMS_Patient_EHR_Live_To_PracticeWeb(dtLiveEHR_appointment, Utility.GetDataBaseConnectionByServicesInstallId(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString()), Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.DtLocationList.Rows[j]["Loc_Id"].ToString(), Utility._filename_EHR_patientoptout, Utility._EHRLogdirectory_EHR_patientoptout);
                                        }
                                        else if (Utility.AppointmentEHRIds.ToString() != "" && Utility.Application_ID == 1)
                                        {
                                            GoalBase.WriteToSyncLogFile_Static("PatientOptOut(" + Utility.AppointmentEHRIds.ToString() + ") Sync Update on Adit Server With Service Install Id : " + Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString() + " And  Clinic : " + Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString() + " Successfully.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EHR_appointment Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }

        #endregion



        #region PullPozativeConfiguration
        public static void GetPozativeConfiguration()
        {

            try
            {
                string locationID = "";
                string organizationID = "";
                string installationID = "";

                string appKey = @"SOFTWARE\PozativeSync";
                //RegistryKey key = Registry.LocalMachine.OpenSubKey(appKey);
                List<string> Service_Install_IDs = new List<string>();
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                {
                    if (registryKey != null)
                    {
                        foreach (string registry in registryKey.GetValueNames())
                        {
                            if (registry.Contains("ServiceInstallation"))
                            {
                                Service_Install_IDs.Add(registry.Split('~')[1].ToString());
                            }
                        }

                    }
                }
                List<Pull_Configuration> configurationdata = new List<Pull_Configuration>();
                foreach (string Service_Install_id in Service_Install_IDs)
                {
                    appKey = @"SOFTWARE\PozativeSync";
                    List<string> Location_IDs = new List<string>();
                    List<string> Organization_IDs = new List<string>();
                    using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            foreach (string registry in registryKey.GetValueNames())
                            {
                                if (registry.Contains("locationID~" + Service_Install_id))
                                {
                                    Location_IDs.Add(registry.Split('~')[2].ToString());
                                    Pull_Configuration data = new Pull_Configuration();
                                    data.locationId = registry.Split('~')[2].ToString();
                                    data.organizationId = registryKey.GetValue(registry).ToString();
                                    data.installationId = Service_Install_id;
                                    configurationdata.Add(data);
                                }
                            }
                        }
                    }
                }

                DataTable dtConfigLocationResponse = Utility.GetLocationData(true);
                DataTable dtLiveConfigLocationResponse = dtConfigLocationResponse.Clone();

                DataTable dtConfigOrganizationResponse = Utility.GetOrganizationData(true);
                DataTable dtLiveConfigOrganizationResponse = dtConfigOrganizationResponse.Clone();

                DataTable dtConfigServiceInstallationResponse = Utility.GetServiceInstallationData(true);
                DataTable dtLiveConfigServiceInstallationResponse = dtConfigServiceInstallationResponse.Clone();

                foreach (Pull_Configuration config in configurationdata)
                {

                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(config);
                    string strApiPatient = PullLiveDatabaseBAL.GetLiveRecord("getehrconfiguration", locationID);
                    var client = new RestClient(strApiPatient);
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    var request = new RestRequest(Method.POST);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(locationID));
                    request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    if (response.ErrorMessage != null)
                    {
                        if (response.ErrorMessage.Contains("GetPozativeConfiguration: The remote name could not be resolved:"))
                        { }
                        else
                        {
                            Utility.WriteToErrorLogFromAll("[GetPozativeConfiguration] " + response.ErrorMessage);
                        }
                        return;
                    }

                    var ConfigResponse = JsonConvert.DeserializeObject<PozativeConfigurationRoot>(response.Content);

                    DateTime curAppt_DateTime = Utility.Datetimesetting();

                    if (ConfigResponse != null && ConfigResponse.data != null)
                    {
                        //foreach (var item in ConfigResponse.data.)
                        {
                            if (ConfigResponse.data.location != null && ConfigResponse.data.location.Count() > 0 && ConfigResponse.data.organization != null && ConfigResponse.data.organization.Count() > 0 && ConfigResponse.data.service_installation != null && ConfigResponse.data.service_installation.Count() > 0)
                            {
                                foreach (var sublocation in ConfigResponse.data.location)
                                {
                                    DataRow RowConfLocation = dtLiveConfigLocationResponse.NewRow();
                                    RowConfLocation["Location_ID"] = sublocation.Location_ID.ToString();
                                    RowConfLocation["name"] = sublocation.name.ToString();
                                    RowConfLocation["google_address"] = sublocation.google_address.ToString();
                                    RowConfLocation["phone"] = sublocation.email.ToString();
                                    RowConfLocation["address"] = sublocation.address.ToString();
                                    RowConfLocation["website_url"] = sublocation.website_url.ToString();
                                    RowConfLocation["language"] = sublocation.language.ToString();
                                    RowConfLocation["owner"] = sublocation.owner.ToString();
                                    RowConfLocation["location_numbers"] = sublocation.location_numbers.ToString();
                                    RowConfLocation["Organization_ID"] = sublocation.Organization_ID.ToString();
                                    RowConfLocation["User_ID"] = sublocation.User_ID.ToString();
                                    RowConfLocation["Clinic_Number"] = sublocation.Clinic_Number.ToString();
                                    RowConfLocation["Service_Install_Id"] = sublocation.Service_Install_Id.ToString();
                                    RowConfLocation["AditSync"] = sublocation.AditSync.ToString();
                                    RowConfLocation["ApptAutoBook"] = sublocation.ApptAutoBook.ToString();
                                    RowConfLocation["AditLocationSyncEnable"] = sublocation.AditLocationSyncEnable.ToString();
                                    dtLiveConfigLocationResponse.Rows.Add(RowConfLocation);
                                    dtLiveConfigLocationResponse.AcceptChanges();
                                }

                                foreach (var suborgnization in ConfigResponse.data.organization)
                                {
                                    DataRow RowConfOrg = dtLiveConfigOrganizationResponse.NewRow();
                                    RowConfOrg["Organization_ID"] = suborgnization.Organization_ID.ToString();
                                    RowConfOrg["Name"] = suborgnization.Name.ToString();
                                    RowConfOrg["phone"] = suborgnization.phone.ToString();
                                    RowConfOrg["address"] = suborgnization.address.ToString();
                                    RowConfOrg["currency"] = suborgnization.currency.ToString();
                                    RowConfOrg["info"] = suborgnization.info.ToString();
                                    RowConfOrg["is_active"] = suborgnization.is_active.ToString();
                                    RowConfOrg["owner"] = suborgnization.owner.ToString();
                                    RowConfOrg["Adit_User_Email_ID"] = suborgnization.Adit_User_Email_ID.ToString();
                                    RowConfOrg["Adit_User_Email_Password"] = suborgnization.Adit_User_Email_Password.ToString();
                                    dtLiveConfigOrganizationResponse.Rows.Add(RowConfOrg);
                                    dtLiveConfigOrganizationResponse.AcceptChanges();
                                }

                                foreach (var subservice in ConfigResponse.data.service_installation)
                                {
                                    DataRow RowConfServiceInstall = dtLiveConfigServiceInstallationResponse.NewRow();
                                    RowConfServiceInstall["Installation_ID"] = subservice.Installation_ID.ToString();
                                    RowConfServiceInstall["Organization_ID"] = subservice.Organization_ID.ToString();
                                    RowConfServiceInstall["Location_ID"] = subservice.Location_ID.ToString();
                                    RowConfServiceInstall["Application_Name"] = subservice.Application_Name.ToString();
                                    RowConfServiceInstall["Application_Version"] = subservice.Application_Version.ToString();
                                    RowConfServiceInstall["System_Name"] = subservice.System_Name.ToString();
                                    RowConfServiceInstall["System_processorID"] = subservice.System_processorID.ToString();
                                    RowConfServiceInstall["Hostname"] = subservice.Hostname.ToString();
                                    RowConfServiceInstall["Database"] = subservice.Database.ToString();
                                    RowConfServiceInstall["IntegrationKey"] = subservice.IntegrationKey.ToString();
                                    RowConfServiceInstall["UserId"] = subservice.UserId.ToString();
                                    RowConfServiceInstall["Password"] = subservice.Password.ToString();
                                    RowConfServiceInstall["Port"] = subservice.Port.ToString();
                                    RowConfServiceInstall["WebAdminUserToken"] = subservice.WebAdminUserToken.ToString();
                                    RowConfServiceInstall["timezone"] = subservice.timezone.ToString();
                                    RowConfServiceInstall["IS_Install"] = subservice.IS_Install.ToString();
                                    RowConfServiceInstall["Installation_Date"] = curAppt_DateTime;//Convert.ToDateTime(subservice.Installation_Date.ToString()); 
                                    RowConfServiceInstall["Installation_Modify_Date"] = curAppt_DateTime; //Convert.ToDateTime(subservice.Installation_Modify_Date.ToString()); 
                                    RowConfServiceInstall["AditSync"] = subservice.AditSync.ToString();
                                    RowConfServiceInstall["PozativeSync"] = subservice.PozativeSync.ToString();
                                    RowConfServiceInstall["ApptAutoBook"] = subservice.ApptAutoBook.ToString();
                                    RowConfServiceInstall["PozativeEmail"] = subservice.PozativeEmail.ToString();
                                    RowConfServiceInstall["PozativeLocationID"] = subservice.PozativeLocationID.ToString();
                                    RowConfServiceInstall["PozativeLocationName"] = subservice.PozativeLocationName.ToString();
                                    RowConfServiceInstall["DBConnString"] = subservice.DBConnString.ToString();
                                    RowConfServiceInstall["Document_Path"] = subservice.Document_Path.ToString();
                                    RowConfServiceInstall["Windows_Service_Version"] = subservice.Windows_Service_Version.ToString();
                                    RowConfServiceInstall["ApplicationIdleTimeOff"] = subservice.ApplicationIdleTimeOff.ToString();
                                    RowConfServiceInstall["AppIdleStartTime"] = curAppt_DateTime; //Convert.ToDateTime(subservice.AppIdleStartTime.ToString());
                                    RowConfServiceInstall["AppIdleStopTime"] = curAppt_DateTime; //Convert.ToDateTime(subservice.AppIdleStopTime.ToString());
                                    RowConfServiceInstall["ApplicationInstalledTime"] = curAppt_DateTime;//Convert.ToDateTime(subservice.ApplicationInstalledTime.ToString());
                                    RowConfServiceInstall["EHR_Sub_Version"] = subservice.EHR_Sub_Version.ToString();
                                    RowConfServiceInstall["EHR_VersionNumber"] = subservice.EHR_VersionNumber.ToString();
                                    RowConfServiceInstall["NotAllowToChangeSystemDateFormat"] = subservice.NotAllowToChangeSystemDateFormat.ToString();
                                    RowConfServiceInstall["DontAskPasswordOnSaveSetting"] = subservice.DontAskPasswordOnSaveSetting.ToString();
                                    RowConfServiceInstall["Adit_User_Email_ID"] = subservice.Adit_User_Email_ID.ToString();
                                    RowConfServiceInstall["Adit_User_Email_Password"] = subservice.Adit_User_Email_Password.ToString();
                                    RowConfServiceInstall["DentrixPDFConstring"] = subservice.DentrixPDFConstring.ToString();
                                    RowConfServiceInstall["DentrixPDFPassword"] = subservice.DentrixPDFPassword.ToString();
                                    dtLiveConfigServiceInstallationResponse.Rows.Add(RowConfServiceInstall);
                                    dtLiveConfigServiceInstallationResponse.AcceptChanges();
                                }
                            }
                            else
                            {
                                GoalBase.WriteToSyncLogFile_Static("Pozative Configuration Sync (Adit Server To Local Database) Records Not found on Adit Server");
                            }
                        }
                    }
                    else
                    {
                        Utility.WriteToSyncLogFile_All("Pozative Configuration  No Responce from API DATA NULL");
                    }
                }

                if (dtLiveConfigServiceInstallationResponse.Rows.Count > 0 && dtLiveConfigLocationResponse.Rows.Count > 0 && dtLiveConfigOrganizationResponse.Rows.Count > 0)
                {
                    Utility.Save_LocationData(dtLiveConfigLocationResponse, false);
                    Utility.Save_OrganizationData(dtLiveConfigOrganizationResponse, false);
                    Utility.Save_ServiceInstallationData(dtLiveConfigServiceInstallationResponse, false);

                    GoalBase.WriteToSyncLogFile_Static("Pozative Configuration Sync (Adit Server To Local Database) Successfully.");
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("Pozative Configuration : " + ex.Message);
            }
        }
        #endregion

    }
}
