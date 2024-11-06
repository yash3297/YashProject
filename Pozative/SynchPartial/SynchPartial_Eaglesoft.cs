using Pozative.BAL;
using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Patterson.Eaglesoft.Library.Dtos;
using System.Net.Http.Headers;
using System.Windows.Forms;
using Pozative.DAL;
using Newtonsoft.Json;
using System.Reflection;

namespace Pozative
{
    public partial class frmPozative  
    {
         
        #region Variable

        bool IsEaglesoftProviderSync = false;
        bool IsEaglesoftOperatorySync = false;
        bool IsEaglesoftApptTypeSync = false;


        private BackgroundWorker bwSynchEaglesoft_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_Appointment = null;

        private BackgroundWorker bwSynchEaglesoft_OperatoryEvent = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_OperatoryEvent = null;

        private BackgroundWorker bwSynchEaglesoft_Operatory = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_Operatory = null;

        private BackgroundWorker bwSynchEaglesoft_Provider = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_Provider = null;

        private BackgroundWorker bwSynchEaglesoft_Speciality = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_Speciality = null;

        private BackgroundWorker bwSynchEaglesoft_ApptType = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_ApptType = null;

        private BackgroundWorker bwSynchEaglesoft_Patient = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_Patient = null;

        private BackgroundWorker bwSynchEaglesoft_RecallType = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_RecallType = null;

        private BackgroundWorker bwSynchEaglesoft_ApptStatus = new BackgroundWorker();
        private System.Timers.Timer timerSynchEaglesoft_ApptStatus = null;


        #endregion

        private void CallSynchEaglesoftToLocal()
        {
            if (Utility.AditSync)
            {
                CallSynch_Provider();
                fncSynchDataEaglesoft_Provider();

                CallSynch_Speciality();
                fncSynchDataEaglesoft_Speciality();

                CallSynch_Operatory();
                fncSynchDataEaglesoft_Operatory();

                CallSynch_OperatoryEvent();
                fncSynchDataEaglesoft_OperatoryEvent();

                CallSynch_Type();
                fncSynchDataEaglesoft_ApptType();

                // CallSynch_Appointment();
                fncSynchDataEaglesoft_Appointment();

                //CallSynch_Patient();
                //fncSynchDataEaglesoft_Patient();

                SynchDataEaglesoft_RecallType();
                fncSynchDataEaglesoft_RecallType();

                SynchDataEaglesoft_ApptStatus();
                fncSynchDataEaglesoft_ApptStatus();

                //CallSynch_Provider();
                //CallSynch_Speciality();
                //CallSynch_Operatory();
                //CallSynch_OperatoryEvent();
                //CallSynch_Type();
                //CallSynch_Appointment();
                //CallSynch_RecallType();
                //SynchDataEaglesoft_ApptStatus();
                //CallSynchLiveDB_PushToLocal();
                //Application.DoEvents();
                //fncSynchDataEaglesoft_Appointment();

            }
        }

        #region Synch Appointment

        private void fncSynchDataEaglesoft_Appointment()
        {
            //  SynchDataEaglesoft_Appointment();
            InitBgWorkerEaglesoft_Appointment();
            InitBgTimerEaglesoft_Appointment();
        }

        private void InitBgTimerEaglesoft_Appointment()
        {
            timerSynchEaglesoft_Appointment = new System.Timers.Timer();
            this.timerSynchEaglesoft_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchEaglesoft_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_Appointment_Tick);
            timerSynchEaglesoft_Appointment.Enabled = true;
            timerSynchEaglesoft_Appointment.Start();
            timerSynchEaglesoft_Appointment_Tick(null, null);
        }

        private void InitBgWorkerEaglesoft_Appointment()
        {
            bwSynchEaglesoft_Appointment.WorkerReportsProgress = true;
            bwSynchEaglesoft_Appointment.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_Appointment.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_Appointment_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_Appointment.Enabled = false;
                MethodForCallSynchOrderEaglesoft_Appointment();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_Appointment()
        {
            System.Threading.Thread procThreadmainEaglesoft_Appointment = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_Appointment);
            procThreadmainEaglesoft_Appointment.Start();
        }

        public void CallSyncOrderTableEaglesoft_Appointment()
        {
            if (bwSynchEaglesoft_Appointment.IsBusy != true)
            {
                bwSynchEaglesoft_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_Appointment.Enabled = true;
        }


        public void SynchDataEaglesoft_Appointment()
        {
            CallSynch_Appointment();
           
        }

        private async void CallSynch_Appointment()
        {

            if (IsEaglesoftProviderSync && IsEaglesoftOperatorySync && IsEaglesoftApptTypeSync && !Is_synched_Appointment)
            {
                Is_synched_Appointment = true;
                DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData();
                DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();
                // DateTime dtCurrentDtTime = Utility.Datetimesetting();

                DateTime dtCurrentDtTime = Utility.LastSyncDateAditServer;


                using (var client = new HttpClient())
                {
                    try
                    {
                        DataTable dtEaglesoftAppointment = new DataTable();
                        client.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                        var response = await client.GetAsync("Schedule/GetAppointmentsByDateRange/" + dtCurrentDtTime.AddDays(-2).ToString("yyyy-MM-dd") + "/" + dtCurrentDtTime.AddDays(185).ToString("yyyy-MM-dd"));

                        if (response.ReasonPhrase == "Not Found")
                        {
                            Is_synched_Appointment = false;
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                            //ObjGoalBase.WriteToSyncLogFile("Appointment Sync (EagleSoft To Local Database) Successfully.");
                            IsEHRAllSync = true;
                            
                            return;
                        }

                        #region If
                        if (response.IsSuccessStatusCode)
                        {
                            var lstAppointment = await response.Content.ReadAsAsync<List<AppointmentDataDto>>();
                            dtEaglesoftAppointment = CommonFunction.Convert_ListToDataTable(lstAppointment);

                            dtEaglesoftAppointment.Columns.Add("Last_Name", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("First_Name", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("MI", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Home_Contact", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Mobile_Contact", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Email", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Address", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("City", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("ST", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Zip", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("ApptType_EHR_ID", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("ApptType", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Operatory_EHR_ID", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Provider_EHR_ID", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("ProviderName", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Appt_EHR_ID", typeof(string));
                            dtEaglesoftAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                            dtEaglesoftAppointment.Columns.Add("InsUptDlt", typeof(int));

                            DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData();

                            int cntCurRecord = 0;

                            foreach (DataRow dtDtxRow in dtEaglesoftAppointment.Rows)
                            {
                                if (dtDtxRow["PatientId"].ToString().Trim() != "0" && dtDtxRow["PatientId"].ToString().Trim() != "" && dtDtxRow["PatientId"].ToString().Trim() != null)
                                {

                                    HttpClient clientpatient = new HttpClient();
                                    clientpatient.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                                    clientpatient.DefaultRequestHeaders.Accept.Clear();
                                    clientpatient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    //clientpatient.DefaultRequestHeaders.Add("sessiontoken", sessiontoken.Content.ReadAsStringAsync().Result.Replace("\"", ""));
                                    clientpatient.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                                    var responsePatient = await clientpatient.GetAsync("Patient/GetPersonById?id=" + dtDtxRow["PatientId"].ToString().Trim());
                                    if (responsePatient.IsSuccessStatusCode)
                                    {
                                        var patient = await responsePatient.Content.ReadAsAsync<PatientDto>();

                                        DataRow[] rowBlcOpt = dtLocalOperatory.Copy().Select("Operatory_Name = '" + dtDtxRow["ChairName"].ToString().Trim() + "' ");
                                        if (rowBlcOpt.Length > 0)
                                        {
                                            dtDtxRow["Operatory_EHR_ID"] = rowBlcOpt[0]["Operatory_EHR_ID"].ToString();
                                        }

                                        if (patient.LastName != string.Empty && patient.LastName != null)
                                        {
                                            dtDtxRow["Last_Name"] = patient.LastName.ToString().Trim();
                                        }
                                        else
                                        {
                                            dtDtxRow["Last_Name"] = string.Empty;
                                        }

                                        if (patient.FirstName != string.Empty && patient.FirstName != null)
                                        {
                                            dtDtxRow["First_Name"] = patient.FirstName.ToString().Trim();
                                        }
                                        else
                                        {
                                            dtDtxRow["First_Name"] = string.Empty;
                                        }

                                        if (patient.MiddleInitial != string.Empty && patient.MiddleInitial != null)
                                        {
                                            dtDtxRow["MI"] = patient.MiddleInitial.ToString().Trim();
                                        }
                                        else
                                        {
                                            dtDtxRow["MI"] = string.Empty;
                                        }

                                        if (patient.HomePhone != string.Empty && patient.HomePhone != null)
                                        {
                                            dtDtxRow["Home_Contact"] = patient.HomePhone.ToString().Trim();
                                        }
                                        else
                                        {
                                            dtDtxRow["Home_Contact"] = string.Empty;
                                        }

                                        if (patient.CellPhone != string.Empty && patient.CellPhone != null)
                                        {
                                            dtDtxRow["Mobile_Contact"] = patient.CellPhone.ToString().Trim();
                                        }
                                        else
                                        {
                                            dtDtxRow["Mobile_Contact"] = string.Empty;
                                        }

                                        if (patient.EmailAddress != string.Empty && patient.EmailAddress != null)
                                        {
                                            dtDtxRow["Email"] = patient.EmailAddress;
                                        }
                                        else
                                        {
                                            dtDtxRow["Email"] = string.Empty;
                                        }

                                        if (patient.Address1 != string.Empty && patient.Address1 != null)
                                        {
                                            dtDtxRow["Address"] = patient.Address1;
                                        }
                                        else
                                        {
                                            dtDtxRow["Address"] = string.Empty;
                                        }

                                        if (patient.City != string.Empty && patient.City != null)
                                        {
                                            dtDtxRow["City"] = patient.City;
                                        }
                                        else
                                        {
                                            dtDtxRow["City"] = string.Empty;
                                        }

                                        if (patient.State != string.Empty && patient.State != null)
                                        {
                                            dtDtxRow["ST"] = patient.State;
                                        }
                                        else
                                        {
                                            dtDtxRow["ST"] = string.Empty;
                                        }

                                        if (patient.Zipcode != string.Empty && patient.Zipcode != null)
                                        {
                                            dtDtxRow["Zip"] = patient.Zipcode;
                                        }
                                        else
                                        {
                                            dtDtxRow["Zip"] = string.Empty;
                                        }

                                        string ProvIDList = string.Empty;

                                        for (int i = 0; i < lstAppointment[cntCurRecord].ProviderAppointmentList.Count; i++)
                                        {
                                            ProvIDList = ProvIDList + lstAppointment[cntCurRecord].ProviderAppointmentList[i].ProviderId + " ; ";
                                        }

                                        if (ProvIDList.Length > 0)
                                        {
                                            ProvIDList = ProvIDList.Substring(0, ProvIDList.Length - 1);
                                        }

                                        string ProvNameList = string.Empty;

                                        for (int i = 0; i < lstAppointment[cntCurRecord].ProviderAppointmentList.Count; i++)
                                        {
                                            ProvNameList = ProvNameList + lstAppointment[cntCurRecord].ProviderAppointmentList[i].ProviderName + " ; ";
                                        }

                                        if (ProvNameList.Length > 0)
                                        {
                                            ProvNameList = ProvNameList.Substring(0, ProvNameList.Length - 1);
                                        }

                                        cntCurRecord = cntCurRecord + 1;
                                        dtDtxRow["Provider_EHR_ID"] = ProvIDList;
                                        dtDtxRow["ProviderName"] = ProvNameList;
                                        dtDtxRow["ApptType_EHR_ID"] = dtDtxRow["AppointmentTypeId"].ToString();

                                        DataRow[] rowOpt = dtLocalOperatory.Copy().Select("Operatory_Name = '" + dtDtxRow["ChairName"].ToString().Trim() + "' ");
                                        if (rowOpt.Length > 0)
                                        {
                                            dtDtxRow["Operatory_EHR_ID"] = rowOpt[0]["Operatory_EHR_ID"].ToString();
                                        }

                                        DataRow[] ApptTypeRow = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["AppointmentTypeId"].ToString() + "'");
                                        if (ApptTypeRow.Length > 0)
                                        {
                                            dtDtxRow["ApptType"] = ApptTypeRow[0]["Type_Name"].ToString();
                                            dtDtxRow["ApptType_EHR_ID"] = dtDtxRow["AppointmentTypeId"].ToString();
                                        }
                                    }
                                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                                    {


                                        CallSynch_Appointment();
                                    }
                                }

                                DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appointmentid"].ToString().Trim() + "' ");
                                if (row.Length > 0)
                                {

                                    int TmpAppointmentStatus = 0;
                                    string tmpAppointmentArrivalStatus = "";
                                    string tmpappointment_status = "";
                                    tmpappointment_status = row[0]["Appointment_Status"].ToString().ToLower().Trim();
                                    if (tmpappointment_status == "completed")
                                    {
                                        tmpAppointmentArrivalStatus = "4";
                                    }
                                    else
                                    {
                                        TmpAppointmentStatus = Convert.ToInt32(row[0]["appointment_status_ehr_key"].ToString().ToLower().Trim());
                                    }

                                    int commentlen = 1999;
                                    if (dtDtxRow["AppointmentNotes"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dtDtxRow["AppointmentNotes"].ToString().Trim().Length;
                                    }

                                    if (dtDtxRow["LocationId"].ToString().Trim() == "-1")
                                    {
                                        dtDtxRow["InsUptDlt"] = 3;
                                    }
                                    else if (dtDtxRow["Last_Name"].ToString().Trim() != row[0]["Last_Name"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["First_Name"].ToString().Trim() != row[0]["First_Name"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["MI"].ToString().Trim() != row[0]["MI"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Utility.ConvertContactNumber(dtDtxRow["Home_Contact"].ToString().Trim()) != Utility.ConvertContactNumber(row[0]["Home_Contact"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) != Utility.ConvertContactNumber(row[0]["Mobile_Contact"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Email"].ToString().Trim() != row[0]["Email"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Address"].ToString().Trim() != row[0]["Address"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["City"].ToString().Trim() != row[0]["City"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["ST"].ToString().Trim() != row[0]["ST"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Zip"].ToString().Trim() != row[0]["Zip"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Provider_EHR_ID"].ToString().Trim() != row[0]["Provider_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["AppointmentNotes"].ToString().ToLower().Trim().Substring(0, commentlen) != row[0]["comment"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["ApptType_EHR_ID"].ToString().Trim() != row[0]["ApptType_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Convert.ToDateTime(dtDtxRow["StartTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Appt_DateTime"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Convert.ToDateTime(dtDtxRow["EndTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Appt_EndDateTime"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["AppointmentArrivalStatus"].ToString().Trim() != row[0]["AppointmentArrivalStatus"].ToString().Trim())
                                    {
                                       
                                            dtDtxRow["InsUptDlt"] = 4;
                                       
                                    }                                 
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 0;
                                    }
                                }
                                else
                                {
                                    DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) + "' AND ISNULL(Appt_EHR_ID,0 ) =0 ");

                                    if (rowCon.Length > 0)
                                    {
                                        dtDtxRow["InsUptDlt"] = 5;
                                        dtDtxRow["Appt_LocalDB_ID"] = rowCon[0]["Appt_LocalDB_ID"];
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 1;
                                    }
                                }
                            }

                            dtEaglesoftAppointment.AcceptChanges();

                            if (dtEaglesoftAppointment != null && dtEaglesoftAppointment.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_Appointment_Eaglesoft_To_Local(dtEaglesoftAppointment);

                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                                    ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                                    SynchDataLiveDB_Push_Appointment();
                                }
                            }

                            Is_synched_Appointment = false;
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            Is_synched_Appointment = false;
                            CallSynch_Appointment();
                        }
                        #endregion
                        IsEHRAllSync = true;
                    }
                    catch (Exception ex)
                    {
                        Is_synched_Appointment = false;
                        ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataEaglesoft_OperatoryEvent()
        {
            //  SynchDataEaglesoft_OperatoryEvent();
            InitBgWorkerEaglesoft_OperatoryEvent();
            InitBgTimerEaglesoft_OperatoryEvent();
        }

        private void InitBgTimerEaglesoft_OperatoryEvent()
        {
            timerSynchEaglesoft_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchEaglesoft_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchEaglesoft_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_OperatoryEvent_Tick);
            timerSynchEaglesoft_OperatoryEvent.Enabled = true;
            timerSynchEaglesoft_OperatoryEvent.Start();
            timerSynchEaglesoft_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerEaglesoft_OperatoryEvent()
        {
            bwSynchEaglesoft_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchEaglesoft_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderEaglesoft_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainEaglesoft_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_OperatoryEvent);
            procThreadmainEaglesoft_OperatoryEvent.Start();
        }

        public void CallSyncOrderTableEaglesoft_OperatoryEvent()
        {
            if (bwSynchEaglesoft_OperatoryEvent.IsBusy != true)
            {
                bwSynchEaglesoft_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_OperatoryEvent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_OperatoryEvent.Enabled = true;
        }

        public void SynchDataEaglesoft_OperatoryEvent()
        {
            CallSynch_OperatoryEvent();
           
        }

        private async void CallSynch_OperatoryEvent()
        {
            if (IsEaglesoftOperatorySync && !Is_synched_OperatoryEvent)
            {
                Is_synched_OperatoryEvent = true;
                DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();
                //DateTime dtCurrentDtTime = Utility.Datetimesetting();

                DateTime dtCurrentDtTime = Utility.LastSyncDateAditServer;

                using (var clientBlocks = new HttpClient())
                {
                    try
                    {

                        DataTable dtEaglesoftOperatoryEvent = new DataTable();

                        clientBlocks.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        clientBlocks.DefaultRequestHeaders.Accept.Clear();
                        clientBlocks.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        clientBlocks.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                        var Blocksresponse = await clientBlocks.GetAsync("Schedule/BlocksByDate/" + dtCurrentDtTime.AddDays(-2).ToString("yyyy-MM-dd") + "/" + dtCurrentDtTime.AddDays(185).ToString("yyyy-MM-dd"));

                        if (Blocksresponse.ReasonPhrase == "Not Found")
                        {
                            Is_synched_OperatoryEvent = false;
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
                            IsEHRAllSync = true;
                            return;
                        }

                        if (Blocksresponse.IsSuccessStatusCode)
                        {
                            var lstBlocksByDate = await Blocksresponse.Content.ReadAsAsync<List<AppointmentDataDto>>();
                            dtEaglesoftOperatoryEvent = CommonFunction.Convert_ListToDataTable(lstBlocksByDate);

                            dtEaglesoftOperatoryEvent.Columns.Add("Operatory_EHR_ID", typeof(string));
                            dtEaglesoftOperatoryEvent.Columns.Add("OE_EHR_ID", typeof(string));
                            dtEaglesoftOperatoryEvent.Columns.Add("OE_LocalDB_ID", typeof(int));
                            dtEaglesoftOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));

                            DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData();

                            int cntCurRecord = 0;

                            foreach (DataRow dtDtxRow in dtEaglesoftOperatoryEvent.Rows)
                            {

                                DataRow[] rowBlcOpt = dtLocalOperatory.Copy().Select("Operatory_Name = '" + dtDtxRow["ChairName"].ToString().Trim() + "' ");
                                if (rowBlcOpt.Length > 0)
                                {
                                    dtDtxRow["Operatory_EHR_ID"] = rowBlcOpt[0]["Operatory_EHR_ID"].ToString();
                                }

                                DataRow[] row = dtLocalOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtDtxRow["Appointmentid"].ToString().Trim() + "' ");
                                if (row.Length > 0)
                                {

                                    int commentlen = 1999;
                                    if (dtDtxRow["AppointmentNotes"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dtDtxRow["AppointmentNotes"].ToString().Trim().Length;
                                    }

                                    if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }

                                    else if (dtDtxRow["AppointmentNotes"].ToString().ToLower().Trim().Substring(0, commentlen) != row[0]["comment"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }

                                    else if (Convert.ToDateTime(dtDtxRow["StartTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["StartTime"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (Convert.ToDateTime(dtDtxRow["EndTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EndTime"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 0;
                                    }
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 1;
                                }
                            }

                            foreach (DataRow dtLOERow in dtLocalOperatoryEvent.Rows)
                            {

                                DataRow[] rowBlcOpt = dtEaglesoftOperatoryEvent.Copy().Select("Appointmentid = '" + dtLOERow["OE_EHR_ID"].ToString().Trim() + "' ");
                                if (rowBlcOpt.Length > 0)
                                { }
                                else
                                {
                                    DataRow BlcOptDtldr = dtEaglesoftOperatoryEvent.NewRow();
                                    BlcOptDtldr["Appointmentid"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                                    BlcOptDtldr["InsUptDlt"] = 3;
                                    dtEaglesoftOperatoryEvent.Rows.Add(BlcOptDtldr);
                                }
                            }

                            dtEaglesoftOperatoryEvent.AcceptChanges();

                            if (dtEaglesoftOperatoryEvent != null && dtEaglesoftOperatoryEvent.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_OperatoryEvent_Eaglesoft_To_Local(dtEaglesoftOperatoryEvent);
                                if (status)
                                {
                                    ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_OperatoryEvent();
                                }
                            }
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
                            Is_synched_OperatoryEvent = false;
                        }
                        else if (Blocksresponse.StatusCode == HttpStatusCode.Forbidden)
                        {
                            Is_synched_OperatoryEvent = false;
                            CallSynch_OperatoryEvent();
                        }
                        IsEHRAllSync = true;
                    }
                    catch (Exception ex)
                    {
                        Is_synched_OperatoryEvent = false;
                        ObjGoalBase.WriteToErrorLogFile("[OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Synch Provider

        private void fncSynchDataEaglesoft_Provider()
        {
            //SynchDataEaglesoft_Provider();
            InitBgWorkerEaglesoft_Provider();
            InitBgTimerEaglesoft_Provider();
        }

        private void InitBgTimerEaglesoft_Provider()
        {
            timerSynchEaglesoft_Provider = new System.Timers.Timer();
            this.timerSynchEaglesoft_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchEaglesoft_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_Provider_Tick);
            timerSynchEaglesoft_Provider.Enabled = true;
            timerSynchEaglesoft_Provider.Start();
        }

        private void InitBgWorkerEaglesoft_Provider()
        {
            bwSynchEaglesoft_Provider.WorkerReportsProgress = true;
            bwSynchEaglesoft_Provider.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_Provider.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_Provider_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_Provider.Enabled = false;
                MethodForCallSynchOrderEaglesoft_Provider();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_Provider()
        {
            System.Threading.Thread procThreadmainEaglesoft_Provider = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_Provider);
            procThreadmainEaglesoft_Provider.Start();
        }

        public void CallSyncOrderTableEaglesoft_Provider()
        {
            if (bwSynchEaglesoft_Provider.IsBusy != true)
            {
                bwSynchEaglesoft_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_Provider();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_Provider.Enabled = true;
        }

        public void SynchDataEaglesoft_Provider()
        {
            CallSynch_Provider();
            
        }

        private async void CallSynch_Provider()
        {
            if (!Is_synched_Provider)
            {
                Is_synched_Provider = true;
                using (var client = new HttpClient())
                {
                    try
                    {
                        DataTable dtEaglesoftProvider = new DataTable();

                        client.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //var sessiontoken = await SynchEaglesoftDAL.Authenticate();
                        //client.DefaultRequestHeaders.Add("sessiontoken", sessiontoken.Content.ReadAsStringAsync().Result.Replace("\"", ""));
                        client.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                        var response = await client.GetAsync("Provider/GetProviderList");
                        if (response.IsSuccessStatusCode)
                        {
                            var lstProvider = await response.Content.ReadAsAsync<List<ProviderDto>>();
                            dtEaglesoftProvider = CommonFunction.Convert_ListToDataTable(lstProvider);
                            dtEaglesoftProvider.Columns.Add("InsUptDlt", typeof(int));
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            Is_synched_Provider = false;
                            CallSynch_Provider();
                        }

                        DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData();

                        foreach (DataRow dtDtxRow in dtEaglesoftProvider.Rows)
                        {
                            DataRow[] row = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtDtxRow["Providerid"].ToString().Trim() + "'");
                            //if (row.Length > 0)
                            //{
                            //     dtDtxRow["InsUptDlt"] = 2;
                            //}
                            //else
                            //{
                            //     dtDtxRow["InsUptDlt"] = 1;
                            //}

                            if (row.Length > 0)
                            {
                                string strTmpGender = "";
                                if (dtDtxRow["sex"].ToString().ToLower().Trim() == "M".ToString().ToLower())
                                {
                                    strTmpGender = "male";
                                }
                                else
                                {
                                    strTmpGender = "female";
                                }

                                if (dtDtxRow["LastName"].ToString().Trim() != row[0]["Last_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["FirstName"].ToString().Trim() != row[0]["First_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (strTmpGender != row[0]["gender"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["PositionName"].ToString().Trim() != row[0]["provider_speciality"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 0;
                                }
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }

                        dtEaglesoftProvider.AcceptChanges();

                        if (dtEaglesoftProvider != null && dtEaglesoftProvider.Rows.Count > 0)
                        {
                            bool status = SynchEaglesoftBAL.Save_Provider_Eaglesoft_To_Local(dtEaglesoftProvider);

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                                ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                IsEaglesoftProviderSync = true;
                                SynchDataLiveDB_Push_Provider();
                                
                            }
                            else
                            {
                                IsEaglesoftProviderSync = false;
                            }
                        }
                        Is_synched_Provider = false;
                    }
                    catch (Exception ex)
                    {
                        Is_synched_Provider = false;
                        ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database)]" + ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Synch Speciality

        private void fncSynchDataEaglesoft_Speciality()
        {
            //SynchDataEaglesoft_Speciality();
            InitBgWorkerEaglesoft_Speciality();
            InitBgTimerEaglesoft_Speciality();
        }

        private void InitBgTimerEaglesoft_Speciality()
        {
            timerSynchEaglesoft_Speciality = new System.Timers.Timer();
            this.timerSynchEaglesoft_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchEaglesoft_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_Speciality_Tick);
            timerSynchEaglesoft_Speciality.Enabled = true;
            timerSynchEaglesoft_Speciality.Start();
        }

        private void InitBgWorkerEaglesoft_Speciality()
        {
            bwSynchEaglesoft_Speciality.WorkerReportsProgress = true;
            bwSynchEaglesoft_Speciality.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_Speciality.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_Speciality_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_Speciality_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_Speciality.Enabled = false;
                MethodForCallSynchOrderEaglesoft_Speciality();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_Speciality()
        {
            System.Threading.Thread procThreadmainEaglesoft_Speciality = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_Speciality);
            procThreadmainEaglesoft_Speciality.Start();
        }

        public void CallSyncOrderTableEaglesoft_Speciality()
        {
            if (bwSynchEaglesoft_Speciality.IsBusy != true)
            {
                bwSynchEaglesoft_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_Speciality();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_Speciality.Enabled = true;
        }

        public void SynchDataEaglesoft_Speciality()
        {
            CallSynch_Speciality();
            
        }

        private async void CallSynch_Speciality()
        {
            if (!Is_synched_Speciality)
            {
                Is_synched_Speciality = true;
                using (var client = new HttpClient())
                {
                    try
                    {
                        DataTable dtEaglesoftSpeciality = new DataTable();
                        client.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //var sessiontoken = await SynchEaglesoftDAL.Authenticate();
                        //client.DefaultRequestHeaders.Add("sessiontoken", sessiontoken.Content.ReadAsStringAsync().Result.Replace("\"", ""));
                        client.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                        var response = await client.GetAsync("Provider/GetProviderList");
                        DataTable dtSpecialitydistinctValues = new DataTable();
                        if (response.IsSuccessStatusCode)
                        {
                            var lstProvider = await response.Content.ReadAsAsync<List<ProviderDto>>();
                            dtEaglesoftSpeciality = CommonFunction.Convert_ListToDataTable(lstProvider);
                            DataView view = new DataView(dtEaglesoftSpeciality);
                            dtSpecialitydistinctValues = view.ToTable(true, "PositionName");
                            dtSpecialitydistinctValues.Columns.Add("InsUptDlt", typeof(int));
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            Is_synched_Speciality = false;
                            CallSynch_Speciality();
                        }

                        DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData();

                        foreach (DataRow dtDtxRow in dtSpecialitydistinctValues.Rows)
                        {
                            DataRow[] row = dtLocalSpeciality.Copy().Select("speciality_Name = '" + dtDtxRow["PositionName"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        dtSpecialitydistinctValues.AcceptChanges();

                        if (dtSpecialitydistinctValues != null && dtSpecialitydistinctValues.Rows.Count > 0)
                        {
                            bool status = SynchEaglesoftBAL.Save_Speciality_Eaglesoft_To_Local(dtSpecialitydistinctValues);

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality");
                                ObjGoalBase.WriteToSyncLogFile("Speciality Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_Speciality();
                              
                            }
                        }
                        Is_synched_Speciality = false;
                    }
                    catch (Exception ex)
                    {
                        Is_synched_Speciality = false;
                        ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " to Local Database)]" + ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Synch Operatory

        private void fncSynchDataEaglesoft_Operatory()
        {
            //SynchDataEaglesoft_Operatory();
            InitBgWorkerEaglesoft_Operatory();
            InitBgTimerEaglesoft_Operatory();
        }

        private void InitBgTimerEaglesoft_Operatory()
        {
            timerSynchEaglesoft_Operatory = new System.Timers.Timer();
            this.timerSynchEaglesoft_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchEaglesoft_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_Operatory_Tick);
            timerSynchEaglesoft_Operatory.Enabled = true;
            timerSynchEaglesoft_Operatory.Start();
        }

        private void InitBgWorkerEaglesoft_Operatory()
        {
            bwSynchEaglesoft_Operatory.WorkerReportsProgress = true;
            bwSynchEaglesoft_Operatory.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_Operatory.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_Operatory_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_Operatory_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_Operatory.Enabled = false;
                MethodForCallSynchOrderEaglesoft_Operatory();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_Operatory()
        {
            System.Threading.Thread procThreadmainEaglesoft_Operatory = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_Operatory);
            procThreadmainEaglesoft_Operatory.Start();
        }

        public void CallSyncOrderTableEaglesoft_Operatory()
        {
            if (bwSynchEaglesoft_Operatory.IsBusy != true)
            {
                bwSynchEaglesoft_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_Operatory.Enabled = true;
        }

        public void SynchDataEaglesoft_Operatory()
        {
            CallSynch_Operatory();
          
        }

        private async void CallSynch_Operatory()
        {
            if (!Is_synched_Operatory)
            {
                Is_synched_Operatory = true;
                using (var client = new HttpClient())
                {
                    try
                    {
                        DataTable dtEaglesoftOperatory = new DataTable();

                        client.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // var sessiontoken = await SynchEaglesoftDAL.Authenticate(Utility.EHRHostname, Utility.EHRIntegrationKey, Utility.EHRUserId, Utility.EHRPassword);
                        // client.DefaultRequestHeaders.Add("sessiontoken", sessiontoken.Content.ReadAsStringAsync().Result.Replace("\"", ""));
                        client.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                        var response = await client.GetAsync("Schedule/GetChairs");
                        if (response.IsSuccessStatusCode)
                        {
                            var lstOperatory = await response.Content.ReadAsAsync<List<ChairDto>>();
                            dtEaglesoftOperatory = CommonFunction.Convert_ListToDataTable(lstOperatory);
                            dtEaglesoftOperatory.Columns.Add("InsUptDlt", typeof(int));
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            Is_synched_Operatory = false;
                            CallSynch_Operatory();
                        }

                        DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();
                        foreach (DataRow dtDtxRow in dtEaglesoftOperatory.Rows)
                        {
                            DataRow[] row = dtLocalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtDtxRow["ChairNum"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["ChairName"].ToString().Trim() != row[0]["Operatory_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 0;
                                }
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }

                        //foreach (DataRow dtLocalRow in dtLocalOperatory.Rows)
                        //{
                        //    DataRow[] row = dtEaglesoftOperatory.Copy().Select("ChairNum = " + dtLocalRow["Operatory_EHR_ID"]);
                        //    if (row.Length <= 0)
                        //    {
                        //        dtEaglesoftOperatory.Rows.Add(dtLocalRow["Operatory_EHR_ID"], dtLocalRow["Operatory_Name"], "D");
                        //    }
                        //}

                        dtEaglesoftOperatory.AcceptChanges();

                        if (dtEaglesoftOperatory != null && dtEaglesoftOperatory.Rows.Count > 0)
                        {
                            bool status = SynchEaglesoftBAL.Save_Operatory_Eaglesoft_To_Local(dtEaglesoftOperatory);
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                                ObjGoalBase.WriteToSyncLogFile("Chairs (Operatory) Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                IsEaglesoftOperatorySync = true;
                                SynchDataLiveDB_Push_Operatory();
                            }
                            else
                            {
                                IsEaglesoftOperatorySync = false;
                            }
                        }
                        Is_synched_Operatory = false;
                    }
                    catch (Exception ex)
                    {
                        Is_synched_Operatory = false;
                        ObjGoalBase.WriteToErrorLogFile("[Chairs (Operatory) Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    }
                }
            }

        }

        #endregion

        #region Synch Appointment Type

        private void fncSynchDataEaglesoft_ApptType()
        {
            InitBgWorkerEaglesoft_ApptType();
            InitBgTimerEaglesoft_ApptType();
        }

        private void InitBgTimerEaglesoft_ApptType()
        {
            timerSynchEaglesoft_ApptType = new System.Timers.Timer();
            this.timerSynchEaglesoft_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchEaglesoft_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_ApptType_Tick);
            timerSynchEaglesoft_ApptType.Enabled = true;
            timerSynchEaglesoft_ApptType.Start();
        }

        private void InitBgWorkerEaglesoft_ApptType()
        {
            bwSynchEaglesoft_ApptType.WorkerReportsProgress = true;
            bwSynchEaglesoft_ApptType.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_ApptType.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_ApptType_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_ApptType_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_ApptType.Enabled = false;
                MethodForCallSynchOrderEaglesoft_ApptType();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_ApptType()
        {
            System.Threading.Thread procThreadmainEaglesoft_ApptType = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_ApptType);
            procThreadmainEaglesoft_ApptType.Start();
        }

        public void CallSyncOrderTableEaglesoft_ApptType()
        {
            if (bwSynchEaglesoft_ApptType.IsBusy != true)
            {
                bwSynchEaglesoft_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_ApptType.Enabled = true;
        }

        public void SynchDataEaglesoft_ApptType()
        {
            CallSynch_Type();
           
        }

        private async void CallSynch_Type()
        {
            if (!Is_synched_Type)
            {
                Is_synched_Type = true;
                using (var client = new HttpClient())
                {
                    try
                    {
                        DataTable dtEaglesoftApptType = new DataTable();
                        client.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //var sessiontoken = await SynchEaglesoftDAL.Authenticate(Utility.EHRHostname, Utility.EHRIntegrationKey, Utility.EHRUserId, Utility.EHRPassword);
                        //client.DefaultRequestHeaders.Add("sessiontoken", sessiontoken.Content.ReadAsStringAsync().Result.Replace("\"", ""));
                        client.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                        var response = await client.GetAsync("Schedule/ApptTypes");
                        if (response.IsSuccessStatusCode)
                        {
                            var lstApptType = await response.Content.ReadAsAsync<List<AppointmentTypesDto>>();
                            dtEaglesoftApptType = CommonFunction.Convert_ListToDataTable(lstApptType);
                            dtEaglesoftApptType.Columns.Add("InsUptDlt", typeof(int));
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            Is_synched_Type = false;
                            CallSynch_Type();
                        }

                        DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData();

                        foreach (DataRow dtDtxRow in dtEaglesoftApptType.Rows)
                        {
                            DataRow[] row = dtLocalApptType.Select("ApptType_EHR_ID = '" + dtDtxRow["TypeId"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Description"].ToString().Trim() != row[0]["Type_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 0;
                                }
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                            dtEaglesoftApptType.AcceptChanges();
                        }

                        //foreach (DataRow dtLocalRow in dtLocalApptType.Rows)
                        //{
                        //    DataRow[] row = dtEaglesoftApptType.Copy().Select("TypeId = " + dtLocalRow["ApptType_EHR_ID"]);
                        //    if (row.Length <= 0)
                        //    {
                        //        dtEaglesoftApptType.Rows.Add(dtLocalRow["ApptType_EHR_ID"], dtLocalRow["Type_Name"], "D");
                        //    }
                        //}

                        dtEaglesoftApptType.AcceptChanges();

                        if (dtEaglesoftApptType != null && dtEaglesoftApptType.Rows.Count > 0)
                        {
                            bool Type = SynchEaglesoftBAL.Save_ApptType_Eaglesoft_To_Local(dtEaglesoftApptType);

                            if (Type)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                                ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                IsEaglesoftApptTypeSync = true;
                                SynchDataLiveDB_Push_ApptType();
                               
                            }
                            else
                            {
                                IsEaglesoftApptTypeSync = false;
                            }
                        }
                        Is_synched_Type = false;
                    }
                    catch (Exception ex)
                    {
                        Is_synched_Type = false;
                        ObjGoalBase.WriteToErrorLogFile("[Appointment Type Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Synch Patient

        private void fncSynchDataEaglesoft_Patient()
        {
            InitBgWorkerEaglesoft_Patient();
            InitBgTimerEaglesoft_Patient();
        }

        private void InitBgTimerEaglesoft_Patient()
        {
            timerSynchEaglesoft_Patient = new System.Timers.Timer();
            this.timerSynchEaglesoft_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchEaglesoft_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_Patient_Tick);
            timerSynchEaglesoft_Patient.Enabled = true;
            timerSynchEaglesoft_Patient.Start();
            timerSynchEaglesoft_Patient_Tick(null, null);
        }

        private void InitBgWorkerEaglesoft_Patient()
        {
            bwSynchEaglesoft_Patient.WorkerReportsProgress = true;
            bwSynchEaglesoft_Patient.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_Patient.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_Patient_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_Patient_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_Patient.Enabled = false;
                MethodForCallSynchOrderEaglesoft_Patient();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_Patient()
        {
            System.Threading.Thread procThreadmainEaglesoft_Patient = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_Patient);
            procThreadmainEaglesoft_Patient.Start();
        }

        public void CallSyncOrderTableEaglesoft_Patient()
        {
            if (bwSynchEaglesoft_Patient.IsBusy != true)
            {
                bwSynchEaglesoft_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_Patient();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_Patient.Enabled = true;
        }

        public void SynchDataEaglesoft_Patient()
        {
            CallSynch_Patient();
            
        }

        private async void CallSynch_Patient()
        {
            using (var client = new HttpClient())
            {
                try
                {

                    DataTable dtEaglesoftPatient = new DataTable();
                    client.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var sessiontoken = await SynchEaglesoftDAL.Authenticate(Utility.EHRHostname, Utility.EHRIntegrationKey, Utility.EHRUserId, Utility.EHRPassword);
                    //client.DefaultRequestHeaders.Add("sessiontoken", sessiontoken.Content.ReadAsStringAsync().Result.Replace("\"", ""));
                    client.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                    client.Timeout = TimeSpan.FromMinutes(60);

                    var response = await client.GetAsync("Patient/GetPatientList");
                    if (response.IsSuccessStatusCode)
                    {
                        var lstPatient = await response.Content.ReadAsAsync<List<PatientDto>>();
                        dtEaglesoftPatient = CommonFunction.Convert_ListToDataTable(lstPatient);
                        
                        dtEaglesoftPatient.Columns.Add("tmpSex", typeof(string));
                        dtEaglesoftPatient.Columns.Add("tmpMaritalStatus", typeof(string));
                        dtEaglesoftPatient.Columns.Add("tmpReceiveSMS", typeof(string));
                        dtEaglesoftPatient.Columns.Add("tmpReceiveEmail", typeof(string));

                        dtEaglesoftPatient.Columns.Add("tmpBirth_Date", typeof(string));
                        dtEaglesoftPatient.Columns.Add("tmpFirstVisit_Date", typeof(string));
                        dtEaglesoftPatient.Columns.Add("tmpLastVisit_Date", typeof(string));
                        dtEaglesoftPatient.Columns.Add("tmpnextvisit_date", typeof(string));

                        dtEaglesoftPatient.Columns.Add("due_date", typeof(string));
                        dtEaglesoftPatient.Columns.Add("remaining_benefit", typeof(string));
                        dtEaglesoftPatient.Columns.Add("used_benefit", typeof(string));
                        dtEaglesoftPatient.Columns.Add("collect_payment", typeof(string));
                        dtEaglesoftPatient.Columns.Add("InsUptDlt", typeof(int));
                    }
                    else if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        CallSynch_Patient();
                    }

                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData();
                    DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData();

                    ///////////////////////// Get Patient Next Appointment //////////
                    DataTable dtEaglesoftPatientNextApptDate = new DataTable();
                    HttpClient clientpatientNextAppt = new HttpClient();
                    clientpatientNextAppt.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    clientpatientNextAppt.DefaultRequestHeaders.Accept.Clear();
                    clientpatientNextAppt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    clientpatientNextAppt.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                    clientpatientNextAppt.Timeout = TimeSpan.FromMinutes(60);
                    var responsePatientNextAppt = await clientpatientNextAppt.GetAsync("Schedule/GetAppointmentsByDateRange/" + DateTime.Now.ToString("yyyy-MM-dd") + "/" + DateTime.Now.AddDays(210).ToString("yyyy-MM-dd"));
                    if (responsePatientNextAppt.IsSuccessStatusCode)
                    {
                        var patientNextAppt = await responsePatientNextAppt.Content.ReadAsAsync<List<AppointmentDataDto>>();

                        if (patientNextAppt.Count > 0)
                        {
                            dtEaglesoftPatientNextApptDate = CommonFunction.Convert_ListToDataTable(patientNextAppt);
                        }
                    }
                    /////////////////////////////////////////////////////////////////

                    string sqlSelect = string.Empty;
                    string patSEX = string.Empty;
                    string ReceiveSMS = string.Empty;
                    string ReceiveEmail = string.Empty;
                    string Status = string.Empty;
                    string MaritalStatus = string.Empty;

                    string tmpRecallType = string.Empty;
                    string tmpRecallTypeId = string.Empty;
                    string tmpDueDate = string.Empty;
                    double tmpremaining_benefit = 0;
                    double tmplocremaining_benefit = 0;

                    double tmpused_benefit = 0;
                    double tmplocused_benefit = 0;

                    string RecallType_DueDate = string.Empty;
                    TotalPatientRecord = dtEaglesoftPatient.Rows.Count;
                    GetPatientRecord = 0;
                    foreach (DataRow dtDtxRow in dtEaglesoftPatient.Rows)
                    {
                        GetPatientRecord = GetPatientRecord + 1;
                        ///////////////////////// Get Patient RecallType and Due Date //////////
                        DataTable dtEaglesoftPatientRecallTpyeDueDate = new DataTable();
                        HttpClient clientpatientRecallTpyeDueDate = new HttpClient();
                        clientpatientRecallTpyeDueDate.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        clientpatientRecallTpyeDueDate.DefaultRequestHeaders.Accept.Clear();
                        clientpatientRecallTpyeDueDate.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        clientpatientRecallTpyeDueDate.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                        clientpatientRecallTpyeDueDate.Timeout = TimeSpan.FromMinutes(60);
                        var responseDueDate = await clientpatientRecallTpyeDueDate.GetAsync("Patient/PatientRecallByPatientId?patientId=" + dtDtxRow["patientId"].ToString().Trim());
                        if (responseDueDate.IsSuccessStatusCode)
                        {
                            var patientRecallTpyeDueDate = await responseDueDate.Content.ReadAsAsync<List<PatientRecallsDto>>();

                            if (patientRecallTpyeDueDate.Count > 0)
                            {
                                dtEaglesoftPatientRecallTpyeDueDate = CommonFunction.Convert_ListToDataTable(patientRecallTpyeDueDate);
                            }
                        }

                        RecallType_DueDate = "";
                        tmpDueDate = "";
                        tmpRecallType = "";
                        tmpRecallTypeId = "";

                        if (dtEaglesoftPatientRecallTpyeDueDate != null && dtEaglesoftPatientRecallTpyeDueDate.Rows.Count > 0)
                        {
                            DataRow[] RecallTpyeDueDaterow = dtEaglesoftPatientRecallTpyeDueDate.Copy().Select("DefaultRecallYn = 'Y'");
                            if (RecallTpyeDueDaterow.Length > 0)
                            {
                                tmpDueDate = RecallTpyeDueDaterow[0]["NextRecallDate"].ToString();
                                DataRow[] RecallTpyerow = dtLocalRecallType.Copy().Select("RecallType_EHR_ID = '" + RecallTpyeDueDaterow[0]["RecallId"].ToString() + "'");
                                if (RecallTpyerow.Length > 0)
                                {
                                    tmpRecallType = RecallTpyerow[0]["RecallType_Name"].ToString();
                                    tmpRecallTypeId = RecallTpyeDueDaterow[0]["RecallId"].ToString();
                                }
                                if (tmpDueDate.Length > 0)
                                {
                                    RecallType_DueDate = tmpDueDate + "@" + tmpRecallType + "@" + tmpRecallTypeId;
                                }
                            }
                        }
                        dtDtxRow["due_date"] = RecallType_DueDate;

                        /////////////////////////////////////////////////////////////////

                        dtDtxRow["tmpBirth_Date"] = Utility.CheckValidDatetime(dtDtxRow["BirthDate"].ToString().Trim());
                        dtDtxRow["tmpFirstVisit_Date"] = Utility.CheckValidDatetime(dtDtxRow["FirstVisitDate"].ToString().Trim());
                        dtDtxRow["tmpLastVisit_Date"] = Utility.CheckValidDatetime(dtDtxRow["LastDateSeen"].ToString().Trim());

                        //DataTable dtOpenDentalPatientNextApptDateTemp = dtEaglesoftPatientNextApptDate.Select("AppointmentArrivalStatus <> 1 ").CopyToDataTable();
                        //https://app.asana.com/0/751059797849097/1149506260330945
                        dtDtxRow["tmpnextvisit_date"] = Utility.SetNextVisitDate(dtEaglesoftPatientNextApptDate, "patientId", "patientId", "StartTime", dtDtxRow["patientId"].ToString());

                        dtDtxRow["tmpnextvisit_date"] = Utility.CheckValidDatetime(dtDtxRow["tmpnextvisit_date"].ToString());

                        try
                        {
                            patSEX = dtDtxRow["Sex"].ToString().Trim();
                        }
                        catch (Exception)
                        { patSEX = "M"; }

                        if (patSEX == "M")
                        { dtDtxRow["tmpSex"] = "Male"; }
                        else if (patSEX == "F")
                        { dtDtxRow["tmpSex"] = "Female"; }
                        else
                        { dtDtxRow["tmpSex"] = "Unknown"; }

                        try
                        {
                            MaritalStatus = dtDtxRow["MaritalStatus"].ToString().Trim();
                        }
                        catch (Exception)
                        { MaritalStatus = "S"; }

                        if (MaritalStatus == "S" || MaritalStatus == "")
                        { dtDtxRow["tmpMaritalStatus"] = "Single"; }
                        else if (MaritalStatus == "M")
                        { dtDtxRow["tmpMaritalStatus"] = "Married"; }
                        else if (MaritalStatus == "C")
                        { dtDtxRow["tmpMaritalStatus"] = "Child"; }
                        else if (MaritalStatus == "W")
                        { dtDtxRow["tmpMaritalStatus"] = "Widowed"; }
                        else if (MaritalStatus == "D")
                        { dtDtxRow["tmpMaritalStatus"] = "Divorced"; }
                        else
                        { dtDtxRow["tmpMaritalStatus"] = "Single"; }

                        try
                        {
                            ReceiveSMS = dtDtxRow["ReceivesSms"].ToString().Trim();
                        }
                        catch (Exception)
                        { ReceiveSMS = "0"; }

                        if (ReceiveSMS == "0" || ReceiveSMS == "" || ReceiveSMS == "Y")
                        { dtDtxRow["tmpReceiveSMS"] = "Y"; }
                        else
                        { dtDtxRow["tmpReceiveSMS"] = "N"; }

                        try
                        {
                            ReceiveEmail = dtDtxRow["ReceiveEmail"].ToString().Trim();
                        }
                        catch (Exception)
                        { ReceiveEmail = "0"; }

                        if (ReceiveEmail == "0" || ReceiveEmail == "" || ReceiveEmail == "Y")
                        { dtDtxRow["tmpReceiveEmail"] = "Y"; }
                        else
                        { dtDtxRow["tmpReceiveEmail"] = "N"; }

                        try
                        {
                            Status = dtDtxRow["Status"].ToString().Trim();
                        }
                        catch (Exception)
                        { Status = "A"; }

                        if (Status == "A")
                        { Status = "A"; }
                        else
                        { Status = "I"; }

                        try
                        {
                            tmpused_benefit = (Convert.ToDouble(dtDtxRow["PrimBenefitsRemaining"].ToString().Trim()) + Convert.ToDouble(dtDtxRow["SecBenefitsRemaining"].ToString().Trim()));
                        }
                        catch (Exception)
                        {
                            tmpused_benefit = 0;
                        }
                        dtDtxRow["remaining_benefit"] = tmpused_benefit.ToString();

                        try
                        {
                            tmpused_benefit = (Convert.ToDouble(dtDtxRow["PrimOutstandingBalance"].ToString().Trim()) + Convert.ToDouble(dtDtxRow["SecOutstandingBalance"].ToString().Trim()));
                        }
                        catch (Exception)
                        {
                            tmpused_benefit = 0;
                        }
                        dtDtxRow["used_benefit"] = tmpused_benefit.ToString();

                        DataRow[] row = dtLocalPatient.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["patientId"].ToString().Trim() + "'");
                        if (row.Length > 0)
                        {

                            try
                            {
                                tmplocremaining_benefit = Convert.ToDouble(row[0]["remaining_benefit"].ToString().Trim());
                            }
                            catch (Exception)
                            {
                                tmplocremaining_benefit = 0;
                            }

                            try
                            {
                                tmplocused_benefit = Convert.ToDouble(row[0]["used_benefit"].ToString().Trim());
                            }
                            catch (Exception)
                            {
                                tmplocused_benefit = 0;
                            }
                            //dtDtxRow["tmpBirth_Date"] = Utility.CheckValidDatetime(dtDtxRow["BirthDate"].ToString().Trim());
                            //dtDtxRow["tmpFirstVisit_Date"] = Utility.CheckValidDatetime(dtDtxRow["FirstVisitDate"].ToString().Trim());
                            //dtDtxRow["tmpLastVisit_Date"] = Utility.CheckValidDatetime(dtDtxRow["LastDateSeen"].ToString().Trim());

                            if (dtDtxRow["Firstname"].ToString().Trim() != row[0]["First_name"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Lastname"].ToString().Trim() != row[0]["Last_name"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["MiddleInitial"].ToString().Trim() != row[0]["Middle_Name"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["ResponsiblePartyStatus"].ToString().Trim() != row[0]["ResponsibleParty_Status"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Salutation"].ToString().Trim() != row[0]["Salutation"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["PreferredName"].ToString().Trim() != row[0]["preferred_name"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Status.Trim() != row[0]["Status"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["tmpSex"].ToString().Trim() != row[0]["Sex"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["tmpMaritalStatus"].ToString().Trim() != row[0]["MaritalStatus"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }

                            else if (dtDtxRow["EmailAddress"].ToString().Trim() != row[0]["Email"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["CellPhone"].ToString().Trim() != row[0]["Mobile"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["HomePhone"].ToString().Trim() != row[0]["Home_Phone"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["WorkPhone"].ToString().Trim() != row[0]["Work_Phone"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Address1"].ToString().Trim() != row[0]["Address1"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Address2"].ToString().Trim() != row[0]["Address2"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["City"].ToString().Trim() != row[0]["City"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["State"].ToString().Trim() != row[0]["State"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Zipcode"].ToString().Trim() != row[0]["Zipcode"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToDecimal(dtDtxRow["CurrentBal"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["CurrentBal"].ToString().Trim()).ToString("0.##"))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToDecimal(dtDtxRow["ThirtyDay"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["ThirtyDay"].ToString().Trim()).ToString("0.##"))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToDecimal(dtDtxRow["SixtyDay"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["SixtyDay"].ToString().Trim()).ToString("0.##"))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToDecimal(dtDtxRow["NinetyDay"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["NinetyDay"].ToString().Trim()).ToString("0.##"))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToDouble(tmpremaining_benefit) != Convert.ToDouble(tmplocremaining_benefit))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToDouble(tmpused_benefit) != Convert.ToDouble(tmplocused_benefit))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }

                            else if (dtDtxRow["PrimaryInsuranceCompanyId"].ToString().Trim() != row[0]["Primary_Insurance"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["PrimaryInsuranceCompanyName"].ToString().Trim() != row[0]["Primary_Insurance_CompanyName"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }

                            else if (dtDtxRow["SecondaryInsuranceCompanyId"].ToString().Trim() != row[0]["Secondary_Insurance"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["SecondaryInsuranceCompanyName"].ToString().Trim() != row[0]["Secondary_Insurance_CompanyName"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["ResponsibleParty"].ToString().Trim() != row[0]["Guar_ID"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["tmpReceiveSMS"].ToString().Trim() != row[0]["ReceiveSMS"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["tmpReceiveEmail"].ToString().Trim() != row[0]["ReceiveEmail"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 0;
                            }

                            if (Utility.DateDiffBetweenTwoDate(dtDtxRow["tmpBirth_Date"].ToString().Trim(), row[0]["Birth_Date"].ToString().Trim()))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            if (Utility.DateDiffBetweenTwoDate(dtDtxRow["tmpFirstVisit_Date"].ToString().Trim(), row[0]["FirstVisit_Date"].ToString().Trim()))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            if (Utility.DateDiffBetweenTwoDate(dtDtxRow["tmpLastVisit_Date"].ToString().Trim(), row[0]["LastVisit_Date"].ToString().Trim()))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            if (Utility.DateDiffBetweenTwoDate(dtDtxRow["tmpnextvisit_date"].ToString().Trim(), row[0]["nextvisit_date"].ToString().Trim()))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }

                            if (dtDtxRow["due_date"].ToString().Trim() != row[0]["due_date"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                        }
                        else
                        {
                            dtDtxRow["InsUptDlt"] = 1;
                        }
                    }

                    dtEaglesoftPatient.AcceptChanges();

                    if (dtEaglesoftPatient != null && dtEaglesoftPatient.Rows.Count > 0)
                    {
                        bool Patient = SynchEaglesoftBAL.Save_Patient_Eaglesoft_To_Local(dtEaglesoftPatient);

                        if (Patient)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsGetParientRecordDone = true;
                           
                        }
                    }
                    else
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                        IsGetParientRecordDone = true;
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch RecallType

        private void fncSynchDataEaglesoft_RecallType()
        {
            InitBgWorkerEaglesoft_RecallType();
            InitBgTimerEaglesoft_RecallType();
        }

        private void InitBgTimerEaglesoft_RecallType()
        {
            timerSynchEaglesoft_RecallType = new System.Timers.Timer();
            this.timerSynchEaglesoft_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchEaglesoft_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_RecallType_Tick);
            timerSynchEaglesoft_RecallType.Enabled = true;
            timerSynchEaglesoft_RecallType.Start();
        }

        private void InitBgWorkerEaglesoft_RecallType()
        {
            bwSynchEaglesoft_RecallType.WorkerReportsProgress = true;
            bwSynchEaglesoft_RecallType.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_RecallType.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_RecallType_DoWork);
            bwSynchEaglesoft_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_RecallType_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_RecallType.Enabled = false;
                MethodForCallSynchOrderEaglesoft_RecallType();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_RecallType()
        {
            System.Threading.Thread procThreadmainEaglesoft_RecallType = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_RecallType);
            procThreadmainEaglesoft_RecallType.Start();
        }

        public void CallSyncOrderTableEaglesoft_RecallType()
        {
            if (bwSynchEaglesoft_RecallType.IsBusy != true)
            {
                bwSynchEaglesoft_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_RecallType.Enabled = true;
        }

        public void SynchDataEaglesoft_RecallType()
        {
            CallSynch_RecallType();
           
        }

        private async void CallSynch_RecallType()
        {
            if (!Is_synched_RecallType)
            {
                Is_synched_RecallType = true;
                using (var client = new HttpClient())
                {
                    try
                    {
                        DataTable dtEaglesoftRecallType = new DataTable();

                        client.BaseAddress = new Uri("https://" + Utility.EHRHostname + ":9888/");
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // client.DefaultRequestHeaders.Add("sessiontoken", Utility.APISessionToken);
                        var sessiontoken = await SynchEaglesoftDAL.Authenticate(Utility.EHRHostname, Utility.EHRIntegrationKey, Utility.EHRUserId, Utility.EHRPassword);
                        client.DefaultRequestHeaders.Add("sessiontoken", sessiontoken.Content.ReadAsStringAsync().Result.Replace("\"", ""));
                        var response = await client.GetAsync("Patient/RecallsList");
                        if (response.IsSuccessStatusCode)
                        {
                            var lstRecall = await response.Content.ReadAsAsync<List<RecallDto>>();
                            dtEaglesoftRecallType = CommonFunction.Convert_ListToDataTable(lstRecall);
                            dtEaglesoftRecallType.Columns.Add("InsUptDlt", typeof(int));
                        }
                        else if (response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            Is_synched_RecallType = false;
                            CallSynch_RecallType();
                        }
                        DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData();
                        foreach (DataRow dtDtxRow in dtEaglesoftRecallType.Rows)
                        {
                            DataRow[] row = dtLocalRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallId"] + "'");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Description"].ToString().ToLower().Trim() != row[0]["RecallType_Name"].ToString().ToLower().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 0;
                                }
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }

                        dtEaglesoftRecallType.AcceptChanges();

                        if (dtEaglesoftRecallType != null && dtEaglesoftRecallType.Rows.Count > 0)
                        {
                            bool status = SynchEaglesoftBAL.Save_RecallType_Eaglesoft_To_Local(dtEaglesoftRecallType);
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
                                ObjGoalBase.WriteToSyncLogFile("RecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_RecallType();
                            }
                        }
                        Is_synched_RecallType = false;
                    }
                    catch (Exception ex)
                    {
                        Is_synched_RecallType = false;
                        ObjGoalBase.WriteToErrorLogFile("[RecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Synch ApptStatus

        private void fncSynchDataEaglesoft_ApptStatus()
        {
            InitBgWorkerEaglesoft_ApptStatus();
            InitBgTimerEaglesoft_ApptStatus();
        }

        private void InitBgTimerEaglesoft_ApptStatus()
        {
            timerSynchEaglesoft_ApptStatus = new System.Timers.Timer();
            this.timerSynchEaglesoft_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchEaglesoft_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_ApptStatus_Tick);
            timerSynchEaglesoft_ApptStatus.Enabled = true;
            timerSynchEaglesoft_ApptStatus.Start();
        }

        private void InitBgWorkerEaglesoft_ApptStatus()
        {
            bwSynchEaglesoft_ApptStatus.WorkerReportsProgress = true;
            bwSynchEaglesoft_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_ApptStatus_DoWork);
            bwSynchEaglesoft_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_ApptStatus.Enabled = false;
                MethodForCallSynchOrderEaglesoft_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_ApptStatus()
        {
            System.Threading.Thread procThreadmainEaglesoft_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_ApptStatus);
            procThreadmainEaglesoft_ApptStatus.Start();
        }

        public void CallSyncOrderTableEaglesoft_ApptStatus()
        {
            if (bwSynchEaglesoft_ApptStatus.IsBusy != true)
            {
                bwSynchEaglesoft_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_ApptStatus();
              
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_ApptStatus.Enabled = true;
        }

        public void SynchDataEaglesoft_ApptStatus()
        {
            if (!Is_synched_ApptStatus)
            {
                Is_synched_ApptStatus = true;
                try
                {
                    DataTable dtEaglesoftAppointmentStatus = new DataTable();
                    dtEaglesoftAppointmentStatus.Clear();
                    dtEaglesoftAppointmentStatus.Columns.Add("ApptStatus_EHR_ID", typeof(int));
                    dtEaglesoftAppointmentStatus.Columns.Add("ApptStatus_Name", typeof(string));
                    dtEaglesoftAppointmentStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtEaglesoftAppointmentStatus.Rows.Add(0, "Unconfirmed");
                    dtEaglesoftAppointmentStatus.Rows.Add(1, "Confirmed");
                    dtEaglesoftAppointmentStatus.Rows.Add(2, "Sent Email");
                    dtEaglesoftAppointmentStatus.Rows.Add(3, "Left Message");
                    dtEaglesoftAppointmentStatus.Rows.Add(4, "No Answer");
                    dtEaglesoftAppointmentStatus.Rows.Add(5, "Phone Busy");
                    dtEaglesoftAppointmentStatus.Rows.Add(6, "Waiting For Callback");
                    dtEaglesoftAppointmentStatus.Rows.Add(7, "Other");
                    dtEaglesoftAppointmentStatus.Rows.Add(8, "Mark As Walked Out");

                    dtEaglesoftAppointmentStatus.AcceptChanges();

                    DataTable dtLocalAppointmentStatus = SynchLocalBAL.GetLocalAppointmentStatusData();

                    foreach (DataRow dtDtxRow in dtEaglesoftAppointmentStatus.Rows)
                    {
                        DataRow[] row = dtLocalAppointmentStatus.Copy().Select("ApptStatus_EHR_ID = '" + dtDtxRow["ApptStatus_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["ApptStatus_Name"].ToString().ToLower().Trim() != row[0]["ApptStatus_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 0;
                            }
                        }
                        else
                        {
                            dtDtxRow["InsUptDlt"] = 1;
                        }
                    }

                    dtEaglesoftAppointmentStatus.AcceptChanges();

                    if (dtEaglesoftAppointmentStatus != null && dtEaglesoftAppointmentStatus.Rows.Count > 0)
                    {
                        bool status = SynchEaglesoftBAL.Save_AppointmentStatus_Eaglesoft_To_Local(dtEaglesoftAppointmentStatus);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                            ObjGoalBase.WriteToSyncLogFile("AppointmentStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_ApptStatus();
                        }
                    }
                    Is_synched_ApptStatus = false;
                }
                catch (Exception ex)
                {
                    Is_synched_ApptStatus = false;
                    ObjGoalBase.WriteToErrorLogFile("[AppointmentStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

    }
}
