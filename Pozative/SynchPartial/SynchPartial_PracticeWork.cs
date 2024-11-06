using Pozative.BAL;
using Pozative.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pozative.DAL;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.IO;

namespace Pozative
{
    public partial class frmPozative
    {

        #region Variable

        bool IsPracticeWorkProviderSync = false;
        bool IsPracticeWorkOperatorySync = false;
        bool IsPracticeWorkApptTypeSync = false;
        bool IsPracticeWorkApptStatusSync = false;

        private BackgroundWorker bwSynchPracticeWork_Appointment = null;
        private System.Timers.Timer timerSynchPracticeWork_Appointment = null;

        private BackgroundWorker bwSynchPracticeWork_OperatoryEvent = null;
        private System.Timers.Timer timerSynchPracticeWork_OperatoryEvent = null;

        private BackgroundWorker bwSynchPracticeWork_Provider = null;
        private System.Timers.Timer timerSynchPracticeWork_Provider = null;

        private BackgroundWorker bwSynchPracticeWork_Speciality = null;
        private System.Timers.Timer timerSynchPracticeWork_Speciality = null;

        private BackgroundWorker bwSynchPracticeWork_Operatory = null;
        private System.Timers.Timer timerSynchPracticeWork_Operatory = null;

        private BackgroundWorker bwSynchPracticeWork_ApptType = null;
        private System.Timers.Timer timerSynchPracticeWork_ApptType = null;

        private BackgroundWorker bwSynchPracticeWork_Patient = null;
        private System.Timers.Timer timerSynchPracticeWork_Patient = null;

        private BackgroundWorker bwSynchPracticeWork_RecallType = null;
        private System.Timers.Timer timerSynchPracticeWork_RecallType = null;

        private BackgroundWorker bwSynchPracticeWork_ApptStatus = null;
        private System.Timers.Timer timerSynchPracticeWork_ApptStatus = null;

        private BackgroundWorker bwSynchLocalToPracticeWork_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchLocalToPracticeWork_Appointment = null;

        private BackgroundWorker bwSynchPracticeWork_Holiday = null;
        private System.Timers.Timer timerSynchPracticeWork_Holiday = null;

        private BackgroundWorker bwSynchLocalToPracticeWork_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToPracticeWork_Patient_Form = null;

        private BackgroundWorker bwSynchPracticeWork_Disease = null;
        private System.Timers.Timer timerSynchPracticeWork_Disease = null;

        private BackgroundWorker bwSynchPracticeWork_Insurance = null;
        private System.Timers.Timer timerSynchPracticeWork_Insurance = null;

        #endregion

        private void CallSynchPracticeWorkToLocal()
        {
            if (Utility.AditSync)
            {
                SynchDataPracticeWork_Operatory();
                fncSynchDataPracticeWork_Operatory();
                Application.DoEvents();
                SynchDataPracticeWork_Disease();
                fncSynchDataPracticeWork_Disease();
                Application.DoEvents();
                SynchDataPracticeWork_Provider();
                fncSynchDataPracticeWork_Provider();
                Application.DoEvents();
                SynchDataPracticeWork_Speciality();
                fncSynchDataPracticeWork_Speciality();
                Application.DoEvents();

                SynchDataPracticeWork_ApptType();
                fncSynchDataPracticeWork_ApptType();
                Application.DoEvents();
                // SynchDataPracticeWork_OperatoryEvent();
                fncSynchDataPracticeWork_OperatoryEvent();
                Application.DoEvents();
                //fncSynchDataLocalToPracticeWork_Appointment();
                // SynchDataLocalToPracticeWork_Appointment();
                Application.DoEvents();
                if (Utility.ApptAutoBook)
                {
                    fncSynchDataLocalToPracticeWork_Appointment();
                }
                Application.DoEvents();
                // SynchDataPracticeWork_Patient();
                fncSynchDataPracticeWork_Patient();
                Application.DoEvents();
                SynchDataPracticeWork_ApptStatus();
                fncSynchDataPracticeWork_ApptStatus();
                Application.DoEvents();
                //SynchDataPracticeWork_Appointment();
                fncSynchDataPracticeWork_Appointment();
                Application.DoEvents();
                // SynchDataPracticeWork_RecallType();
                fncSynchDataPracticeWork_RecallType();
                Application.DoEvents();

                //SynchDataPracticeWork_Holiday();
                fncSynchDataPracticeWork_Holiday();
                Application.DoEvents();
                SynchDataLocalToPracticeWork_Patient_Form();
                fncSynchDataLocalToPracticeWork_Patient_Form();
                Application.DoEvents();

                SynchDataPracticeWork_Insurance();
                fncSynchDataPracticeWork_Insurance();
                Application.DoEvents();
            }
        }

        #region Synch Appointment

        private void fncSynchDataPracticeWork_Appointment()
        {
            InitBgWorkerPracticeWork_Appointment();
            InitBgTimerPracticeWork_Appointment();
        }

        private void InitBgTimerPracticeWork_Appointment()
        {
            timerSynchPracticeWork_Appointment = new System.Timers.Timer();
            this.timerSynchPracticeWork_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchPracticeWork_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_Appointment_Tick);
            timerSynchPracticeWork_Appointment.Enabled = true;
            timerSynchPracticeWork_Appointment.Start();
            timerSynchPracticeWork_Appointment_Tick(null, null);
        }

        private void InitBgWorkerPracticeWork_Appointment()
        {
            bwSynchPracticeWork_Appointment = new BackgroundWorker();
            bwSynchPracticeWork_Appointment.WorkerReportsProgress = true;
            bwSynchPracticeWork_Appointment.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_Appointment.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWork_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_Appointment_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_Appointment.Enabled = false;
                MethodForCallSynchOrderPracticeWork_Appointment();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_Appointment()
        {
            System.Threading.Thread procThreadmainPracticeWork_Appointment = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_Appointment);
            procThreadmainPracticeWork_Appointment.Start();
        }

        public void CallSyncOrderTablePracticeWork_Appointment()
        {
            if (bwSynchPracticeWork_Appointment.IsBusy != true)
            {
                bwSynchPracticeWork_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }

                SynchDataPracticeWork_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_Appointment.Enabled = true;
        }

        public void SynchDataPracticeWork_Appointment()
        {
            if (IsPracticeWorkProviderSync && IsPracticeWorkOperatorySync && IsPracticeWorkApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    if (IsPracticeWorkProviderSync && IsPracticeWorkOperatorySync && IsPracticeWorkApptTypeSync)
                    {
                        //New line by yogesh for sync appointments patients first
                        SynchDataPracticeWork_AppointmentsPatient();
                        SynchDataPracticeWork_PatientStatus();
                        if (IsParientFirstSync)
                        {
                            SynchDataPracticeWork_NewPatient();
                        }
                        DataTable dtSoftDentAppointment = SynchPracticeWorkBAL.GetPracticeWorkAppointmentData();
                        dtSoftDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                        dtSoftDentAppointment.Columns.Add("ProcedureCode", typeof(string));

                        string ProcedureDesc = "";
                        string ProcedureCode = "";

                        DataTable DtPracticeWorkAppointment_Procedures_Data = SynchPracticeWorkBAL.GetPracticeWorkAppointment_Procedures_Data();


                        Utility.WriteToSyncLogFile_All("Start Loop");
                        foreach (DataRow drrow in dtSoftDentAppointment.Rows)
                        {
                            try
                            {


                                //Utility.WriteToSyncLogFile_All("Appt Datetime " + drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM", "") + " Appt Start TIme " + drrow["Start Time"].ToString());
                                //drrow["Appt_datetime"] = Convert.ToDateTime(drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM","")).AddHours(Convert.ToDateTime(drrow["Start Time"].ToString()));
                                drrow["Appt_datetime"] = Convert.ToDateTime(drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["Start Time"].ToString());
                                //Utility.WriteToSyncLogFile_All("Appt Datetime is " + drrow["Appt_datetime"].ToString());
                                drrow["Appt_EndDateTime"] = Convert.ToDateTime(drrow["Appt_EndDateTime"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["End Time"].ToString()).AddMinutes(1).ToString();
                                //Utility.WriteToSyncLogFile_All("Appt Datetime is " + drrow["Appt_EndDateTime"].ToString());

                                ////////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                                ProcedureDesc = "";
                                ProcedureCode = "";

                                DataRow[] dtCurApptProcedure = DtPracticeWorkAppointment_Procedures_Data.Select("Appt_EHR_ID = '" + drrow["Appt_EHR_ID"].ToString().Trim() + "'");

                                foreach (var dtSinProc in dtCurApptProcedure.ToList())
                                {
                                    ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim() + ',';
                                }

                                if (ProcedureCode.ToString().Length > 1)
                                {
                                    ProcedureCode = ProcedureCode.Substring(0, ProcedureCode.Length - 1);
                                    drrow["ProcedureCode"] = ProcedureCode;
                                    drrow["ProcedureDesc"] = "";
                                }
                                else
                                {
                                    drrow["ProcedureDesc"] = "";
                                    drrow["ProcedureCode"] = "";
                                }
                                /////////////////////////////////////

                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToSyncLogFile_All("Appt Error " + ex.Message);
                            }

                            if (drrow["CheckoutTime"].ToString().Trim() != "00:00:00")
                            {
                                drrow["appointment_status_ehr_key"] = "5";
                                drrow["Appointment_Status"] = "CheckedOut";
                            }
                            else if (drrow["SeatedTime"].ToString().Trim() != "00:00:00")
                            {
                                drrow["appointment_status_ehr_key"] = "4";
                                drrow["Appointment_Status"] = "Seated";
                            }
                            else if (drrow["CheckinTime"].ToString().Trim() != "00:00:00")
                            {
                                drrow["appointment_status_ehr_key"] = "3";
                                drrow["Appointment_Status"] = "CheckIn";
                            }
                            else if (drrow["Confirmed Date"].ToString().Trim() != "")
                            {
                                drrow["appointment_status_ehr_key"] = "2";
                                drrow["Appointment_Status"] = "Confirmed";
                            }
                            else
                            {
                                drrow["appointment_status_ehr_key"] = "1";
                                drrow["Appointment_Status"] = "NotConfirm";
                            }

                            if (drrow["AppointmentActStatus"].ToString().Trim() == "3")
                            {
                                drrow["appointment_status_ehr_key"] = "5";
                                drrow["Appointment_Status"] = "CheckedOut";
                            }

                        }

                        dtSoftDentAppointment.Columns.Remove("CheckoutTime");
                        dtSoftDentAppointment.Columns.Remove("Confirmed Date");
                        dtSoftDentAppointment.Columns.Remove("SeatedTime");
                        dtSoftDentAppointment.Columns.Remove("CheckinTime");
                        dtSoftDentAppointment.Columns.Remove("Start Time");
                        dtSoftDentAppointment.Columns.Remove("End Time");
                        dtSoftDentAppointment.Columns.Remove("AppointmentActStatus");

                        dtSoftDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                        dtSoftDentAppointment.Columns["InsUptDlt"].DefaultValue = 0;

                        DataTable dtLocalAppointment = new DataTable();
                        DataTable dtTempResult = SynchLocalBAL.GetLocalAppointmentData("1");
                        //Utility.WriteToSyncLogFile_All("Get Local Appointment");
                        if (dtTempResult.Rows.Count > 0)
                        {
                            try
                            {
                                dtLocalAppointment = dtTempResult.Select("Appt_DateTime >= '" + Utility.LastSyncDateAditServer.ToShortDateString() + "'").CopyToDataTable();
                            }
                            catch (Exception exe1)
                            {
                                dtLocalAppointment = dtTempResult.Clone();
                            }
                        }

                        //dtLocalAppointment.AsEnumerable() // .Where(o => o.Field<object>("birth_date") == null || (o.Field<object>("birth_date") != null && string.IsNullOrEmpty(o.Field<object>("birth_date").ToString()) == true))
                        //    .All(o => {
                        //        if (o["birth_date"] == null || (o["birth_date"] != null && string.IsNullOrEmpty(o["birth_date"].ToString()) == true))
                        //        {
                        //             o["birth_date"] = Convert.ToDateTime("01/01/1900 00:00:00");

                        //        }
                        //        // o["appointment_status_ehr_key"] = 0;
                        //        return true;
                        //    });

                        if (!dtLocalAppointment.Columns.Contains("InsUptDlt"))
                        {
                            dtLocalAppointment.Columns.Add("InsUptDlt", typeof(int));
                            dtLocalAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                        }
                        //Utility.WriteToSyncLogFile_All("Start Compare Appointment");
                        dtLocalAppointment = CompareDataTableRecords(ref dtSoftDentAppointment, dtLocalAppointment, "Appt_EHR_ID", "Appt_LocalDB_ID", "Appt_LocalDB_ID,Appt_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Remind_DateTime,Patient_Status,Is_Appt_DoubleBook,InsuranceCompanyName,is_Status_Updated_From_Web,is_ehr_updated,is_deleted,EHR_Entry_DateTime,Entry_DateTime,Is_Appt,Clinic_Number,Service_Install_Id");
                        //Utility.WriteToSyncLogFile_All("Start Compare Appointment End");
                        //dtSoftDentAppointment.AsEnumerable().Where(o => o.Field<object>("InsUptDlt").ToString() != "1" && (o.Field<object>("appointment_status_ehr_key").ToString() == "3" || o.Field<object>("appointment_status_ehr_key").ToString() == "6" || o.Field<object>("appointment_status_ehr_key").ToString() == "7" || o.Field<object>("appointment_status_ehr_key").ToString() == "8" || o.Field<object>("appointment_status_ehr_key").ToString() == "9")).All(o => { o["InsUptDlt"] = "3"; return true; });

                        dtSoftDentAppointment.AcceptChanges();
                        dtLocalAppointment.AcceptChanges();

                        bool status = false;
                        DataTable dtSaveRecords = dtSoftDentAppointment.Clone();
                        //Utility.WriteToSyncLogFile_All("Start To Set BirthDate");
                        if (dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").Count() > 0 || dtLocalAppointment.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                            {
                                foreach (DataRow drrow in dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)"))
                                {
                                    if (drrow["birth_date"].ToString() == "" || drrow["birth_date"] == string.Empty)
                                    {
                                        drrow["birth_date"] = DBNull.Value;
                                        // dtSaveRecords.Rows.Add(drrow);
                                    }
                                    if (drrow["confirmed_status_ehr_key"].ToString() == "" || drrow["confirmed_status_ehr_key"] == string.Empty)
                                    {
                                        drrow["confirmed_status_ehr_key"] = DBNull.Value;
                                    }
                                }
                                dtSaveRecords.Load(dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtLocalAppointment.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                foreach (DataRow drrow in dtLocalAppointment.Select("InsUptDlt IN (3)"))
                                {
                                    if (drrow["birth_date"].ToString() == "" || drrow["birth_date"] == string.Empty)
                                    {
                                        drrow["birth_date"] = DBNull.Value;
                                        // dtSaveRecords.Rows.Add(drrow);
                                    }
                                    if (drrow["confirmed_status_ehr_key"].ToString() == "" || drrow["confirmed_status_ehr_key"] == string.Empty)
                                    {
                                        drrow["confirmed_status_ehr_key"] = DBNull.Value;
                                    }
                                }
                                //Utility.WriteToErrorLogFromAll("Record going to save 1");
                                dtSaveRecords.Load(dtLocalAppointment.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                                //Utility.WriteToErrorLogFromAll("Record going to save 2");
                            }
                            //Utility.WriteToSyncLogFile_All("Send to save in Local DB");
                            status = SynchPracticeWorkBAL.Save_Appointment_PracticeWork_To_Local(dtSaveRecords);// "Appointment", "Appt_LocalDB_ID,Appt_Web_ID", "Appt_LocalDB_ID");
                            //Utility.WriteToSyncLogFile_All("Records Save in local DB");
                        }

                        else
                        {
                            if (dtSoftDentAppointment.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                            ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsApptTypeSyncPush = true;
                            SynchDataLiveDB_Push_Appointment();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
                finally
                {
                    Is_synched_Appointment = true;
                }
            }
        }

        public void SynchDataPracticeWork_NewPatient()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtPracticeWorkPatient = SynchPracticeWorkBAL.GetPracticeWorkNewPatientData();
                    if (dtPracticeWorkPatient != null && dtPracticeWorkPatient.Rows.Count > 0)
                    {
                        DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalNewPatientData("1");
                        if ((dtLocalPatientResult != null && dtLocalPatientResult.Rows.Count > 0 && dtLocalPatientResult.Select("Is_Adit_Updated = 1").Length > 0))
                        {
                            DataTable dtSaveRecords = new DataTable();
                            var itemsToBeAdded = (from DentrixPatient in dtPracticeWorkPatient.AsEnumerable()
                                                  join LocalPatient in dtLocalPatientResult.AsEnumerable()
                                                  on new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim() }
                                                  equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim() }
                                                  into matchingRows
                                                  from matchingRow in matchingRows.DefaultIfEmpty()
                                                  where matchingRow == null
                                                  select DentrixPatient).ToList();
                            DataTable dtPatientToBeAdded = dtLocalPatientResult.Clone();
                            if (itemsToBeAdded.Count > 0)
                            {
                                string strPatID = String.Join(",", itemsToBeAdded.AsEnumerable().Select(x => x["Patient_EHR_ID"].ToString()).ToArray());
                                strPatID = "'" + strPatID.Replace(",", "','") + "'";
                                if (!string.IsNullOrEmpty(strPatID))
                                {
                                    dtSaveRecords = SynchPracticeWorkBAL.GetPracticeWorkPatientData(strPatID);
                                }
                            }
                            if (!dtSaveRecords.Columns.Contains("InsUptDlt"))
                            {
                                dtSaveRecords.Columns.Add("InsUptDlt", typeof(int));
                                dtSaveRecords.Columns["InsUptDlt"].DefaultValue = 0;
                            }
                            if (dtSaveRecords.Rows.Count > 0)
                            {
                                bool status = SynchPracticeWorkBAL.Save_Patient_PracticeWork_To_Local_New(dtSaveRecords, "0", "1");
                                //Utility.WriteToSyncLogFile_All("Patient inserted or updated : " + System.DateTime.Now.ToString());

                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("New Patient");
                                    ObjGoalBase.WriteToSyncLogFile("New Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_Patient();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[New Patient Sync (" + Utility.Application_Name + " to Local Database) ] Error...");
                                }

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[New Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataPracticeWork_OperatoryEvent()
        {
            InitBgWorkerPracticeWork_OperatoryEvent();
            InitBgTimerPracticeWork_OperatoryEvent();
        }

        private void InitBgTimerPracticeWork_OperatoryEvent()
        {
            timerSynchPracticeWork_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchPracticeWork_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchPracticeWork_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_OperatoryEvent_Tick);
            timerSynchPracticeWork_OperatoryEvent.Enabled = true;
            timerSynchPracticeWork_OperatoryEvent.Start();
            timerSynchPracticeWork_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerPracticeWork_OperatoryEvent()
        {
            bwSynchPracticeWork_OperatoryEvent = new BackgroundWorker();
            bwSynchPracticeWork_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchPracticeWork_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWork_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderPracticeWork_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainPracticeWork_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_OperatoryEvent);
            procThreadmainPracticeWork_OperatoryEvent.Start();
        }

        public void CallSyncOrderTablePracticeWork_OperatoryEvent()
        {
            if (bwSynchPracticeWork_OperatoryEvent.IsBusy != true)
            {
                bwSynchPracticeWork_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_OperatoryEvent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_OperatoryEvent.Enabled = true;
        }

        public void SynchDataPracticeWork_OperatoryEvent()
        {
            // Utility.WriteToSyncLogFile_All("Start Operatory Event");
            if (IsPracticeWorkOperatorySync && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                Utility.WriteToSyncLogFile_All("Start Operatory Event_1");
                try
                {
                    DataTable dtPracticeWorkOperatoryEvent = SynchPracticeWorkBAL.GetPracticeWorkOperatoryEventData();
                    // Utility.WriteToSyncLogFile_All("Start Operatory Event_2");
                    if (!dtPracticeWorkOperatoryEvent.Columns.Contains("InsUptDlt"))
                    {
                        dtPracticeWorkOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                        dtPracticeWorkOperatoryEvent.Columns["InsUptDlt"].DefaultValue = 0;
                    }
                    // Utility.WriteToSyncLogFile_All("Start Operatory Event_3");
                    // Utility.WriteToSyncLogFile_All("Start Loop");
                    foreach (DataRow drrow in dtPracticeWorkOperatoryEvent.Rows)
                    {
                        try
                        {
                            //Utility.WriteToSyncLogFile_All("BlockDate Datetime " + drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "") + " Appt Start TIme " + drrow["StartTime"].ToString());
                            //drrow["Appt_datetime"] = Convert.ToDateTime(drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM","")).AddHours(Convert.ToDateTime(drrow["Start Time"].ToString()));
                            drrow["StartTime"] = Convert.ToDateTime(drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["StartTimeS"].ToString());
                            // Utility.WriteToSyncLogFile_All("OPeratory EHR ID " + Convert.ToDateTime(drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "")).Year.ToString().Substring(2,2) + "_" + Convert.ToDateTime(drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "")).Month.ToString() + "_" + Convert.ToDateTime(drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "")).Day.ToString() + "_" + drrow["Operatory_EHR_ID"].ToString());
                            drrow["OE_EHR_ID"] = Convert.ToDateTime(drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "")).Year.ToString().Substring(2, 2) + "_" + Convert.ToDateTime(drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "")).Month.ToString() + "_" + Convert.ToDateTime(drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "")).Day.ToString() + "_" + drrow["Operatory_EHR_ID"].ToString();

                            drrow["EndTime"] = Convert.ToDateTime(drrow["BlockDate"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["EndTimeE"].ToString()).AddMinutes(1);
                            //o["InsUptDlt"] = 1;
                            // Utility.WriteToSyncLogFile_All("Block Datetime is " + drrow["EndTime"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Utility.WriteToSyncLogFile_All("Appt Error " + ex.Message);
                        }
                    }
                    // Utility.WriteToSyncLogFile_All("End Loop");
                    dtPracticeWorkOperatoryEvent.Columns.Remove("BlockDate");
                    dtPracticeWorkOperatoryEvent.Columns.Remove("StartTimeS");
                    dtPracticeWorkOperatoryEvent.Columns.Remove("EndTimeE");
                    //  Utility.WriteToSyncLogFile_All("Start Operatory Event_3");
                    DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData("1");
                    //  Utility.WriteToSyncLogFile_All("Start Operatory Event_4");
                    DataTable dtTempResult = SynchLocalBAL.GetLocalOperatoryEventData("1");
                    //  Utility.WriteToSyncLogFile_All("Start Operatory Event_5");
                    if (dtTempResult.Select("is_deleted = 0").Count() > 0)
                    {
                        dtLocalOperatoryEvent = dtTempResult.Select("is_deleted = 0 AND StartTime >= '" + Utility.LastSyncDateAditServer.ToShortDateString() + "'").CopyToDataTable();
                    }

                    if (!dtLocalOperatoryEvent.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalOperatoryEvent.Columns["InsUptDlt"].DefaultValue = 0;
                    }
                    //  Utility.WriteToSyncLogFile_All("Start Operatory Event_6");
                    dtLocalOperatoryEvent = CompareDataTableRecords(ref dtPracticeWorkOperatoryEvent, dtLocalOperatoryEvent, "OE_EHR_ID", "OE_LocalDB_ID", "OE_LocalDB_ID,OE_EHR_ID,OE_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");
                    //   Utility.WriteToSyncLogFile_All("Start Operatory Event_7");
                    dtPracticeWorkOperatoryEvent.AcceptChanges();
                    dtLocalOperatoryEvent.AcceptChanges();

                    bool status = false;
                    DataTable dtSaveRecords = dtPracticeWorkOperatoryEvent.Clone();
                    Utility.WriteToSyncLogFile_All("Start Operatory Event_8 Total Count " + dtPracticeWorkOperatoryEvent.Select("InsUptDlt IN (1,2,3)").Count().ToString());
                    Utility.WriteToSyncLogFile_All("Start Operatory Event_9 Total Count " + dtPracticeWorkOperatoryEvent.Select("InsUptDlt IN (3)").Count().ToString());
                    if (dtPracticeWorkOperatoryEvent.Select("InsUptDlt IN (1,2,3)").Count() > 0 || dtLocalOperatoryEvent.Select("InsUptDlt IN (3)").Count() > 0)
                    {
                        // Utility.WriteToSyncLogFile_All("Start Operatory Event_8");
                        if (dtPracticeWorkOperatoryEvent.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtPracticeWorkOperatoryEvent.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                        }
                        if (dtLocalOperatoryEvent.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtLocalOperatoryEvent.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                        }
                        //dtSaveRecords.Load(dtPracticeWorkOperatoryEvent.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                        // Utility.WriteToSyncLogFile_All("Start Operatory Event_9");
                        status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryEvent", "OE_LocalDB_ID,OE_Web_ID", "OE_LocalDB_ID");
                    }
                    else
                    {
                        if (dtPracticeWorkOperatoryEvent.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
                        ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchDataLiveDB_Push_OperatoryEvent();
                    }
                    IsEHRAllSync = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Provider

        private void fncSynchDataPracticeWork_Provider()
        {
            InitBgWorkerPracticeWork_Provider();
            InitBgTimerPracticeWork_Provider();
        }

        private void InitBgTimerPracticeWork_Provider()
        {
            timerSynchPracticeWork_Provider = new System.Timers.Timer();
            this.timerSynchPracticeWork_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchPracticeWork_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_Provider_Tick);
            timerSynchPracticeWork_Provider.Enabled = true;
            timerSynchPracticeWork_Provider.Start();
        }

        private void InitBgWorkerPracticeWork_Provider()
        {
            bwSynchPracticeWork_Provider = new BackgroundWorker();
            bwSynchPracticeWork_Provider.WorkerReportsProgress = true;
            bwSynchPracticeWork_Provider.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_Provider.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWork_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_Provider_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_Provider.Enabled = false;
                MethodForCallSynchOrderPracticeWork_Provider();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_Provider()
        {
            System.Threading.Thread procThreadmainPracticeWork_Provider = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_Provider);
            procThreadmainPracticeWork_Provider.Start();
        }

        public void CallSyncOrderTablePracticeWork_Provider()
        {
            if (bwSynchPracticeWork_Provider.IsBusy != true)
            {
                bwSynchPracticeWork_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_Provider();
                CommonFunction.GetMasterSync();

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_Provider.Enabled = true;
        }

        public void SynchDataPracticeWork_Provider()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentProvider = SynchPracticeWorkBAL.GetPracticeWorkProviderData();
                    dtSoftDentProvider.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentProvider.Columns["InsUptDlt"].DefaultValue = 0;
                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", "1");

                    if (!dtLocalProvider.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalProvider.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalProvider.Columns["InsUptDlt"].DefaultValue = 0;
                    }
                    dtLocalProvider = CompareDataTableRecords(ref dtSoftDentProvider, dtLocalProvider, "Provider_EHR_ID", "Provider_LocalDB_ID", "Provider_LocalDB_ID,Provider_EHR_ID,Provider_Web_ID,image,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    //dtLocalProvider.AsEnumerable().Where(o => string.IsNullOrEmpty(o.Field<object>("InsUptDlt").ToString()) == false && o.Field<object>("InsUptDlt").ToString() == "3").Count() > 0
                    if (dtLocalProvider.Select("InsUptDlt=3").Count() > 0)
                    {
                        dtSoftDentProvider.Load(dtLocalProvider.Select("InsUptDlt=3").CopyToDataTable().CreateDataReader());
                    }

                    dtSoftDentProvider.AcceptChanges();

                    if (dtSoftDentProvider != null && dtSoftDentProvider.Rows.Count > 0 && dtSoftDentProvider.AsEnumerable()
                        .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
                    {
                        DataTable dtSaveRecords = dtSoftDentProvider.Clone();
                        bool status = false;
                        if (dtSoftDentProvider.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtSoftDentProvider.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Providers", "Provider_LocalDB_ID,Provider_Web_ID", "Provider_LocalDB_ID");
                        }
                        else
                        {
                            if (dtSoftDentProvider.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                            ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsPracticeWorkProviderSync = true;
                            SynchDataLiveDB_Push_Provider();

                        }
                        else
                        {
                            IsPracticeWorkProviderSync = false;
                        }
                    }
                    Is_synched_Provider = false;

                    IsProviderSyncedFirstTime = true;
                    SynchDataPracticeWork_ProviderCustomHours();
                    SynchDataPracticeWork_ProviderOfficeHours();
                }
                catch (Exception ex)
                {
                    Is_synched_Provider = false;
                    IsProviderSyncedFirstTime = true;
                    ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database)]" + ex.Message);
                }
            }
        }

        public void SynchDataPracticeWork_ProviderCustomHours()
        {
            try
            {
                ObjGoalBase.WriteToSyncLogFile("Start Provider Custom Hours");
                if (Utility.IsApplicationIdleTimeOff && IsPracticeWorkProviderSync && IsPracticeWorkOperatorySync && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    SynchDataLiveDB_Push_ProviderHours();
                    ObjGoalBase.WriteToSyncLogFile("Start Provider Custom Hours 001");
                    DataTable dtPracticeWorkProviderHours = SynchPracticeWorkBAL.GetProviderCustomHours();
                    ObjGoalBase.WriteToSyncLogFile("Records Fetched");
                    DataTable dtPracticeWorkLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData("1");

                    dtPracticeWorkProviderHours.Columns.Add("InsUptDlt", typeof(int));
                    dtPracticeWorkProviderHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtPracticeWorkLocalProviderHours.Columns.Contains("InsUptDlt"))
                    {
                        dtPracticeWorkLocalProviderHours.Columns.Add("InsUptDlt", typeof(int));
                        dtPracticeWorkLocalProviderHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtPracticeWorkLocalProviderHours = CompareDataTablehourPracticeWork(ref dtPracticeWorkProviderHours, dtPracticeWorkLocalProviderHours, "PH_EHR_ID", "PH_LocalDB_ID", "PH_LocalDB_ID,PH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");

                    dtPracticeWorkProviderHours.AcceptChanges();
                    dtPracticeWorkLocalProviderHours.AcceptChanges();

                    if ((dtPracticeWorkProviderHours != null && dtPracticeWorkProviderHours.Rows.Count > 0) || (dtPracticeWorkLocalProviderHours != null && dtPracticeWorkLocalProviderHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtPracticeWorkProviderHours.Clone();
                        if (dtPracticeWorkProviderHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtPracticeWorkLocalProviderHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtPracticeWorkProviderHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkProviderHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtPracticeWorkLocalProviderHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkLocalProviderHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderHours", "PH_LocalDB_ID,PH_Web_ID", "PH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtPracticeWorkProviderHours.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
                            ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_ProviderHours();
                        }
                    }
                }
                SynchDataLiveDB_Push_ProviderHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public void SynchDataPracticeWork_ProviderOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {

                    SynchDataLiveDB_Push_ProviderOfficeHours();

                    DataTable dtPracticeWorkProviderOfficeHours = SynchPracticeWorkBAL.GetPracticeWorkProviderOfficeHours();
                    DataTable dtPracticeWorkLocalProviderOfficeHours = SynchLocalBAL.GetLocalProviderOfficeHours("1");
                    // DataTable dtPracticeWorkProvider = SynchPracticeWorkBAL.GetPracticeWorkProviderData();

                    dtPracticeWorkProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtPracticeWorkProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtPracticeWorkLocalProviderOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtPracticeWorkLocalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtPracticeWorkLocalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtPracticeWorkLocalProviderOfficeHours = CompareDataTableRecords(ref dtPracticeWorkProviderOfficeHours, dtPracticeWorkLocalProviderOfficeHours, "POH_EHR_ID", "POH_LocalDB_ID", "StartTime2,EndTime2,StartTime3333,endTime3,POH_LocalDB_ID,POH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtPracticeWorkProviderOfficeHours.AcceptChanges();
                    dtPracticeWorkLocalProviderOfficeHours.AcceptChanges();

                    if ((dtPracticeWorkProviderOfficeHours != null && dtPracticeWorkProviderOfficeHours.Rows.Count > 0) || (dtPracticeWorkLocalProviderOfficeHours != null && dtPracticeWorkLocalProviderOfficeHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtPracticeWorkProviderOfficeHours.Clone();
                        if (dtPracticeWorkProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtPracticeWorkLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtPracticeWorkProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkProviderOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtPracticeWorkLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkLocalProviderOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderOfficeHours", "POH_LocalDB_ID,POH_Web_ID", "POH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtPracticeWorkProviderOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderOfficeHours");
                            ObjGoalBase.WriteToSyncLogFile("ProviderOfficeHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_ProviderOfficeHours();
                        }
                        SynchDataPracticeWork_ProviderCustomHours();
                    }
                    SynchDataLiveDB_Push_ProviderOfficeHours();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }
        #endregion


        #region Synch Speciality

        private void fncSynchDataPracticeWork_Speciality()
        {
            InitBgWorkerPracticeWork_Speciality();
            InitBgTimerPracticeWork_Speciality();
        }

        private void InitBgTimerPracticeWork_Speciality()
        {
            timerSynchPracticeWork_Speciality = new System.Timers.Timer();
            this.timerSynchPracticeWork_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchPracticeWork_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_Speciality_Tick);
            timerSynchPracticeWork_Speciality.Enabled = true;
            timerSynchPracticeWork_Speciality.Start();
        }

        private void InitBgWorkerPracticeWork_Speciality()
        {
            bwSynchPracticeWork_Speciality = new BackgroundWorker();
            bwSynchPracticeWork_Speciality.WorkerReportsProgress = true;
            bwSynchPracticeWork_Speciality.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_Speciality.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_Speciality_DoWork);
            bwSynchPracticeWork_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_Speciality_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_Speciality.Enabled = false;
                MethodForCallSynchOrderPracticeWork_Speciality();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_Speciality()
        {
            System.Threading.Thread procThreadmainPracticeWork_Speciality = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_Speciality);
            procThreadmainPracticeWork_Speciality.Start();
        }

        public void CallSyncOrderTablePracticeWork_Speciality()
        {
            if (bwSynchPracticeWork_Speciality.IsBusy != true)
            {
                bwSynchPracticeWork_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_Speciality();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_Speciality.Enabled = true;
        }

        public void SynchDataPracticeWork_Speciality()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentSpeciality = SynchPracticeWorkBAL.GetPracticeWorkSpecialityData();

                    dtSoftDentSpeciality.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentSpeciality.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData("1");
                    if (!dtLocalSpeciality.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalSpeciality.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalSpeciality.Columns["InsUptDlt"].DefaultValue = 0;
                    }


                    dtLocalSpeciality = CompareDataTableRecords(ref dtSoftDentSpeciality, dtLocalSpeciality, "Speciality_Name", "Speciality_LocalDB_ID", "Speciality_LocalDB_ID,Speciality_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,EHR_Entry_DateTime,Clinic_Number,Service_Install_Id");

                    dtSoftDentSpeciality.AcceptChanges();

                    if (dtSoftDentSpeciality != null && dtSoftDentSpeciality.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtSoftDentSpeciality.Clone();
                        if (dtSoftDentSpeciality.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtSoftDentSpeciality.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Speciality", "Speciality_LocalDB_ID,Speciality_Web_ID", "Speciality_LocalDB_ID");
                        }
                        else
                        {
                            if (dtSoftDentSpeciality.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality");
                            ObjGoalBase.WriteToSyncLogFile("Speciality Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Speciality();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }

        #endregion

        #region Synch Operatory

        private void fncSynchDataPracticeWork_Operatory()
        {
            InitBgWorkerPracticeWork_Operatory();
            InitBgTimerPracticeWork_Operatory();
        }

        private void InitBgTimerPracticeWork_Operatory()
        {
            timerSynchPracticeWork_Operatory = new System.Timers.Timer();
            this.timerSynchPracticeWork_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchPracticeWork_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_Operatory_Tick);
            timerSynchPracticeWork_Operatory.Enabled = true;
            timerSynchPracticeWork_Operatory.Start();
        }

        private void InitBgWorkerPracticeWork_Operatory()
        {
            bwSynchPracticeWork_Operatory = new BackgroundWorker();
            bwSynchPracticeWork_Operatory.WorkerReportsProgress = true;
            bwSynchPracticeWork_Operatory.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_Operatory.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_Operatory_DoWork);
            bwSynchPracticeWork_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_Operatory_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_Operatory.Enabled = false;
                MethodForCallSynchOrderPracticeWork_Operatory();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_Operatory()
        {
            System.Threading.Thread procThreadmainPracticeWork_Operatory = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_Operatory);
            procThreadmainPracticeWork_Operatory.Start();
        }

        public void CallSyncOrderTablePracticeWork_Operatory()
        {
            if (bwSynchPracticeWork_Operatory.IsBusy != true)
            {
                bwSynchPracticeWork_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_Operatory.Enabled = true;
        }

        public void SynchDataPracticeWork_Operatory()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {

                try
                {
                    DataTable dtPracticeWorkOperatory = SynchPracticeWorkBAL.GetPracticeWorkOperatoryData();

                    dtPracticeWorkOperatory.Columns.Add("InsUptDlt", typeof(int));
                    dtPracticeWorkOperatory.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData("1");

                    if (!dtLocalOperatory.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalOperatory.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalOperatory.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalOperatory = CompareDataTableRecords(ref dtPracticeWorkOperatory, dtLocalOperatory, "Operatory_EHR_ID", "Operatory_LocalDB_ID", "Operatory_LocalDB_ID,Operatory_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    if (dtLocalOperatory.Select("InsUptDlt=3").Count() > 0)
                    {
                        dtPracticeWorkOperatory.Load(dtLocalOperatory.Select("InsUptDlt=3").CopyToDataTable().CreateDataReader());
                    }

                    dtPracticeWorkOperatory.AcceptChanges();

                    if (dtPracticeWorkOperatory != null && dtPracticeWorkOperatory.Rows.Count > 0 && dtPracticeWorkOperatory.AsEnumerable()
                            .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtPracticeWorkOperatory.Clone();
                        if (dtPracticeWorkOperatory.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtPracticeWorkOperatory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Operatory", "Operatory_LocalDB_ID,Operatory_Web_ID", "Operatory_LocalDB_ID");
                        }
                        else
                        {
                            if (dtPracticeWorkOperatory.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                            ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsPracticeWorkOperatorySync = true;
                            SynchDataLiveDB_Push_Operatory();
                        }
                        else
                        {
                            IsPracticeWorkOperatorySync = false;
                        }
                    }
                    SynchDataPracticeWork_OperatoryCustomHours();
                    SynchDataPracticeWork_OperatoryOfficeHours();
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        public void SynchDataPracticeWork_OperatoryCustomHours()
        {
            try
            {
                SynchDataLiveDB_Push_OperatoryHours();
                ObjGoalBase.WriteToSyncLogFile("Start OPeratory Custom Hours");
                if (Utility.IsApplicationIdleTimeOff && IsPracticeWorkOperatorySync && IsPracticeWorkOperatorySync && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    ObjGoalBase.WriteToSyncLogFile("Start OPeratory Custom Hours 001");
                    DataTable dtPracticeWorkOperatoryHours = SynchPracticeWorkBAL.GetOperatoryCustomHours();
                    ObjGoalBase.WriteToSyncLogFile("Got Operatory Custom Hours");
                    DataTable dtPracticeWorkLocalOperatoryHours = SynchLocalBAL.GetLocalOperatoryHoursData("1");

                    dtPracticeWorkOperatoryHours.Columns.Add("InsUptDlt", typeof(int));
                    dtPracticeWorkOperatoryHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtPracticeWorkLocalOperatoryHours.Columns.Contains("InsUptDlt"))
                    {
                        dtPracticeWorkLocalOperatoryHours.Columns.Add("InsUptDlt", typeof(int));
                        dtPracticeWorkLocalOperatoryHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtPracticeWorkLocalOperatoryHours = CompareDataTablehourPracticeWork(ref dtPracticeWorkOperatoryHours, dtPracticeWorkLocalOperatoryHours, "OH_EHR_ID", "OH_LocalDB_ID", "OH_LocalDB_ID,OH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");

                    dtPracticeWorkOperatoryHours.AcceptChanges();
                    dtPracticeWorkLocalOperatoryHours.AcceptChanges();

                    if ((dtPracticeWorkOperatoryHours != null && dtPracticeWorkOperatoryHours.Rows.Count > 0) || (dtPracticeWorkLocalOperatoryHours != null && dtPracticeWorkLocalOperatoryHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtPracticeWorkOperatoryHours.Clone();
                        if (dtPracticeWorkOperatoryHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtPracticeWorkLocalOperatoryHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtPracticeWorkOperatoryHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkOperatoryHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtPracticeWorkLocalOperatoryHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkLocalOperatoryHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryHours", "OH_LocalDB_ID,PH_Web_ID", "OH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtPracticeWorkOperatoryHours.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryHours");
                            ObjGoalBase.WriteToSyncLogFile("OperatoryHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_OperatoryHours();
                        }

                    }
                }
                SynchDataLiveDB_Push_OperatoryHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[OperatoryHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        private DataTable CompareDataTablehourPracticeWork(ref DataTable dtSource, DataTable dtDestination, string compareColumnName, string primarykeyColumns, string ignoreColumns)
        {
            try
            {
                DataTable dtSourceTemp = dtSource;

                #region Provider available in Local Database Or Not available in local Database
                string[] ignoreColumnsArr = new string[] { "" };
                ignoreColumnsArr = ignoreColumns.Split(',');

                dtSource.AsEnumerable()
                    .All(o =>
                    {
                        var result = dtDestination.AsEnumerable().Where(a => a.Field<object>(compareColumnName).ToString().Trim() == o.Field<object>(compareColumnName).ToString().Trim());
                        if (result != null && result.Count() > 0)
                        {
                            foreach (DataColumn dcCol in dtSourceTemp.Columns)
                            {
                                var result1 = ignoreColumnsArr.AsEnumerable().Where(b => b.ToString().ToUpper() == dcCol.ColumnName.ToUpper());

                                if ((result1 != null && result1.Count() == 0))
                                {

                                    if (dcCol.ColumnName.ToString().ToUpper() == "STARTTIME" || dcCol.ColumnName.ToString().ToUpper() == "ENDTIME")
                                    {
                                        if (result.First().Field<object>(dcCol.ColumnName) != null && string.IsNullOrEmpty(result.First().Field<object>(dcCol.ColumnName).ToString()) == false
                                            && Convert.ToDateTime(o[dcCol.ColumnName].ToString()) != Convert.ToDateTime(result.First().Field<object>(dcCol.ColumnName).ToString()))
                                        {
                                            o["InsUptDlt"] = 2;
                                            o["Last_Sync_Date"] = DateTime.Now.ToString();
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                            break;
                                        }
                                        else
                                        {
                                            o["InsUptDlt"] = 4;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                        }
                                    }
                                    else
                                    {
                                        if (result.First().Field<object>(dcCol.ColumnName) != null && string.IsNullOrEmpty(result.First().Field<object>(dcCol.ColumnName).ToString()) == false && o[dcCol.ColumnName].ToString().Trim().ToLower() != result.First().Field<object>(dcCol.ColumnName).ToString().Trim().ToLower())
                                        {
                                            o["InsUptDlt"] = 2;
                                            o["Last_Sync_Date"] = DateTime.Now.ToString();
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                            break;
                                        }
                                        else
                                        {
                                            o["InsUptDlt"] = 4;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            o["Entry_DateTime"] = DateTime.Now.ToString();
                            o["Last_Sync_Date"] = DateTime.Now.ToString();
                            o["InsUptDlt"] = 1;
                        }
                        return true;
                    });

                #endregion

                #region Records available in local but not available in SoftDent
                dtDestination.AsEnumerable()
                    .All(o =>
                    {
                        var result = dtSourceTemp.AsEnumerable().Where(a => a.Field<object>(compareColumnName).ToString().Trim() == o.Field<object>(compareColumnName).ToString().Trim());
                        if (result != null && result.Count() == 0)
                        {
                            if (Convert.ToBoolean(o["is_deleted"]) == false)
                            {
                                o["InsUptDlt"] = 3;
                            }
                            else
                            {
                                o["InsUptDlt"] = 4;
                            }
                        }
                        else if (result == null)
                        {
                            if (Convert.ToBoolean(o["is_deleted"]) == false)
                            {
                                o["InsUptDlt"] = 3;
                            }
                            else
                            {
                                o["InsUptDlt"] = 4;
                            }
                        }
                        return true;
                    });
                #endregion

                return dtDestination;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SynchDataPracticeWork_OperatoryOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    SynchDataLiveDB_Push_OperatoryOfficeHours();

                    DataTable dtPracticeWorkOperatoryOfficeHours = SynchPracticeWorkBAL.GetPracticeWorkOperatoryOfficeHours();
                    DataTable dtPracticeWorkLocalOperatoryOfficeHours = SynchLocalBAL.GetLocalOperatoryOfficeHoursData("1");
                    DataTable dtPracticeWorkOperatory = SynchPracticeWorkBAL.GetPracticeWorkOperatoryData();

                    dtPracticeWorkOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtPracticeWorkOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtPracticeWorkLocalOperatoryOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtPracticeWorkLocalOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtPracticeWorkLocalOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtPracticeWorkLocalOperatoryOfficeHours = CompareDataTableRecords(ref dtPracticeWorkOperatoryOfficeHours, dtPracticeWorkLocalOperatoryOfficeHours, "OOH_EHR_ID", "OOH_LocalDB_ID", "OOH_LocalDB_ID,OOH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtPracticeWorkOperatoryOfficeHours.AcceptChanges();

                    if (dtPracticeWorkOperatoryOfficeHours != null && dtPracticeWorkOperatoryOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtPracticeWorkOperatoryOfficeHours.Clone();
                        if (dtPracticeWorkOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtPracticeWorkLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtPracticeWorkOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtPracticeWorkLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryOfficeHours", "OOH_LocalDB_ID,OOH_Web_ID", "OOH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtPracticeWorkOperatoryOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryOfficeHours");
                            ObjGoalBase.WriteToSyncLogFile("OperatoryOfficeHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_OperatoryOfficeHours();
                        }
                    }

                    SynchDataLiveDB_Push_OperatoryOfficeHours();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[OperatoryOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }


        //private void CallSynch_OperatoryChair()
        //{
        //    try
        //    {
        //        DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();

        //        DateTime dtCurrentDtTime = Utility.LastSyncDateAditServer;

        //        DataTable dtPracticeWorkOperatoryChair = SynchPracticeWorkBAL.GetPracticeWorkOperatoryChairData();

        //        dtPracticeWorkOperatoryChair.Columns.Add("OE_LocalDB_ID", typeof(int));
        //        dtPracticeWorkOperatoryChair.Columns.Add("InsUptDlt", typeof(int));
        //        DataTable dtLocalOperatoryChair = SynchLocalBAL.GetLocalOperatoryChairData();
        //        foreach (DataRow dtDtxRow in dtPracticeWorkOperatoryChair.Rows)
        //        {
        //            DataRow[] row = dtLocalOperatoryChair.Copy().Select("OE_EHR_ID = '" + dtDtxRow["OE_EHR_ID"].ToString().Trim() + "' ");
        //            if (row.Length > 0)
        //            {

        //                int commentlen = 1999;
        //                if (dtDtxRow["comment"].ToString().Trim().Length < commentlen)
        //                {
        //                    commentlen = dtDtxRow["comment"].ToString().Trim().Length;
        //                }

        //                if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
        //                {
        //                    dtDtxRow["InsUptDlt"] = 2;
        //                }

        //                else if (dtDtxRow["comment"].ToString().ToLower().Trim().Substring(0, commentlen) != row[0]["comment"].ToString().ToLower().Trim())
        //                {
        //                    dtDtxRow["InsUptDlt"] = 2;
        //                }

        //                else if (Convert.ToDateTime(dtDtxRow["StartTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["StartTime"].ToString().Trim()))
        //                {
        //                    dtDtxRow["InsUptDlt"] = 2;
        //                }
        //                else if (Convert.ToDateTime(dtDtxRow["EndTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EndTime"].ToString().Trim()))
        //                {
        //                    dtDtxRow["InsUptDlt"] = 2;
        //                }
        //                else
        //                {
        //                    dtDtxRow["InsUptDlt"] = 0;
        //                }
        //            }
        //            else
        //            {
        //                dtDtxRow["InsUptDlt"] = 1;
        //            }

        //        }
        //        foreach (DataRow dtLOERow in dtLocalOperatoryChair.Rows)
        //        {

        //            DataRow[] rowBlcOpt = dtPracticeWorkOperatoryChair.Copy().Select("OE_EHR_ID = '" + dtLOERow["OE_EHR_ID"].ToString().Trim() + "' ");
        //            if (rowBlcOpt.Length > 0)
        //            { }
        //            else
        //            {
        //                DataRow BlcOptDtldr = dtPracticeWorkOperatoryChair.NewRow();
        //                BlcOptDtldr["OE_EHR_ID"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
        //                BlcOptDtldr["InsUptDlt"] = 3;
        //                dtPracticeWorkOperatoryChair.Rows.Add(BlcOptDtldr);
        //            }
        //        }

        //        dtPracticeWorkOperatoryChair.AcceptChanges();

        //        if (dtPracticeWorkOperatoryChair != null && dtPracticeWorkOperatoryChair.Rows.Count > 0)
        //        {
        //            bool status = SynchPracticeWorkBAL.Save_OperatoryDayOff_PracticeWork_To_Local(dtPracticeWorkOperatoryChair);
        //            if (status)
        //            {
        //                ObjGoalBase.WriteToSyncLogFile("OperatoryDayOff Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                SynchDataLiveDB_Push_OperatoryDayOff();
        //            }
        //        }
        //        else
        //        {
        //            bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryDayOff_Push");
        //        }
        //        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryDayOff");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        #endregion

        #region Synch Appointment Type

        private void fncSynchDataPracticeWork_ApptType()
        {
            InitBgWorkerPracticeWork_ApptType();
            InitBgTimerPracticeWork_ApptType();
        }

        private void InitBgTimerPracticeWork_ApptType()
        {
            timerSynchPracticeWork_ApptType = new System.Timers.Timer();
            this.timerSynchPracticeWork_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchPracticeWork_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_ApptType_Tick);
            timerSynchPracticeWork_ApptType.Enabled = true;
            timerSynchPracticeWork_ApptType.Start();
        }

        private void InitBgWorkerPracticeWork_ApptType()
        {
            bwSynchPracticeWork_ApptType = new BackgroundWorker();
            bwSynchPracticeWork_ApptType.WorkerReportsProgress = true;
            bwSynchPracticeWork_ApptType.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_ApptType.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_ApptType_DoWork);
            bwSynchPracticeWork_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_ApptType_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_ApptType.Enabled = false;
                MethodForCallSynchOrderPracticeWork_ApptType();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_ApptType()
        {
            System.Threading.Thread procThreadmainPracticeWork_ApptType = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_ApptType);
            procThreadmainPracticeWork_ApptType.Start();
        }

        public void CallSyncOrderTablePracticeWork_ApptType()
        {
            if (bwSynchPracticeWork_ApptType.IsBusy != true)
            {
                bwSynchPracticeWork_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_ApptType.Enabled = true;
        }

        public void SynchDataPracticeWork_ApptType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentApptStatus = SynchPracticeWorkBAL.GetPracticeWorkApptTypeData();

                    dtSoftDentApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentApptStatus.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalApptTypeData("1");

                    if (!dtLocalApptStatus.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalApptStatus.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalApptStatus.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    //  dtLocalApptStatus = CompareDataTableRecords(ref dtSoftDentApptStatus, dtLocalApptStatus, "Type_Name", "ApptType_LocalDB_ID", "ApptType_LocalDB_ID,ApptType_EHR_ID,ApptType_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    bool status = false;
                    #region New ApptType Code
                    foreach (DataRow dtDtxRow in dtSoftDentApptStatus.Rows)
                    {
                        DataRow[] row = dtLocalApptStatus.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Type_Name"].ToString().ToLower().Trim() != row[0]["Type_Name"].ToString().ToLower().Trim())
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
                    foreach (DataRow dtDtxRow in dtLocalApptStatus.Rows)
                    {
                        DataRow[] row = dtSoftDentApptStatus.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtSoftDentApptStatus.NewRow();
                            BlcOptDtldr["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["Type_Name"] = dtDtxRow["Type_Name"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtSoftDentApptStatus.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtSoftDentApptStatus.AcceptChanges();
                    if (dtSoftDentApptStatus != null && dtSoftDentApptStatus.Rows.Count > 0)
                    {
                        status = SynchTrackerBAL.Save_Tracker_To_Local(dtSoftDentApptStatus, "Appointment_Type", "ApptType_LocalDB_ID,ApptType_Web_ID", "ApptType_EHR_ID");
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                            ObjGoalBase.WriteToSyncLogFile("ApptType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsPracticeWorkApptTypeSync = true;
                            SynchDataLiveDB_Push_ApptType();
                        }
                        else
                        {
                            IsPracticeWorkApptTypeSync = false;
                        }
                    }
                }
                #endregion
                //    DataTable dtSaveRecords = dtSoftDentApptStatus.Clone();
                //    if (dtSoftDentApptStatus.Select("InsUptDlt IN (1,2)").Count() > 0)
                //    {
                //        dtSaveRecords.Load(dtSoftDentApptStatus.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                //        status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Appointment_Type", "ApptType_LocalDB_ID,ApptType_Web_ID", "ApptType_LocalDB_ID");
                //    }
                //    else
                //    {
                //        if (dtSoftDentApptStatus.Select("InsUptDlt IN (4)").Count() > 0)
                //        {
                //            status = true;
                //        }
                //    }
                //    if (status)
                //    {
                //        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                //        ObjGoalBase.WriteToSyncLogFile("ApptType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                //        IsPracticeWorkApptTypeSync = true;
                //        SynchDataLiveDB_Push_ApptType();
                //    }
                //    else
                //    {
                //        IsPracticeWorkApptTypeSync = false;
                //    }
                //}
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ApptType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Patient

        private void fncSynchDataPracticeWork_Patient()
        {
            InitBgWorkerPracticeWork_Patient();
            InitBgTimerPracticeWork_Patient();
        }

        private void InitBgTimerPracticeWork_Patient()
        {
            timerSynchPracticeWork_Patient = new System.Timers.Timer();
            this.timerSynchPracticeWork_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchPracticeWork_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_Patient_Tick);
            timerSynchPracticeWork_Patient.Enabled = true;
            timerSynchPracticeWork_Patient.Start();
            timerSynchPracticeWork_Patient_Tick(null, null);
        }

        private void InitBgWorkerPracticeWork_Patient()
        {
            bwSynchPracticeWork_Patient = new BackgroundWorker();
            bwSynchPracticeWork_Patient.WorkerReportsProgress = true;
            bwSynchPracticeWork_Patient.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_Patient.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_Patient_DoWork);
            bwSynchPracticeWork_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_Patient_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_Patient.Enabled = false;
                MethodForCallSynchOrderPracticeWork_Patient();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_Patient()
        {
            System.Threading.Thread procThreadmainPracticeWork_Patient = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_Patient);
            procThreadmainPracticeWork_Patient.Start();
        }

        public void CallSyncOrderTablePracticeWork_Patient()
        {
            if (bwSynchPracticeWork_Patient.IsBusy != true)
            {
                bwSynchPracticeWork_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_Patient();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_Patient.Enabled = true;
        }

        public void SynchDataPracticeWork_Patient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;
                    ObjGoalBase.WriteToSyncLogFile("Patient Sync SynchPartial_PracticeWork Start");
                    DataTable dtSoftDentPatientList = SynchPracticeWorkBAL.GetPracticeWorkPatientData();

                    string patientTableName = "Patient";
                    ObjGoalBase.WriteToSyncLogFile("Patient count " + dtSoftDentPatientList.Rows.Count.ToString());
                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");
                    ObjGoalBase.WriteToSyncLogFile("Patient Local count " + dtSoftDentPatientList.Rows.Count.ToString());
                    if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                    {
                        patientTableName = "PatientCompare";
                    }
                    ObjGoalBase.WriteToSyncLogFile("Patient TableName " + dtSoftDentPatientList.Rows.Count.ToString());
                    if (dtSoftDentPatientList != null && dtSoftDentPatientList.Rows.Count > 0)
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient Send to save");

                        bool isPatientSave = SynchClearDentBAL.Save_Patient_ClearDent_To_Local(dtSoftDentPatientList, patientTableName, true);
                        if (isPatientSave)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsGetParientRecordDone = true;
                            SynchDataLiveDB_Push_Patient();

                            //  SynchDataLiveDB_Push_Patient_ASync();                           
                        }
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                        bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                        IsGetParientRecordDone = true;
                    }
                    IsPatientSyncedFirstTime = true;
                    Is_synched_Patient = false;

                    SynchDataPracticeWork_PatientStatus();
                    SynchDataPracticeWork_PatientDisease();
                    SynchDataPracticeWork_PatientMedication("");
                    // ObjGoalBase.WriteToSyncLogFile("Patient Sync GetPracticeWorkPatientData");
                    //dtSoftDentPatientList.Columns.Add("InsUptDlt", typeof(int));
                    //dtSoftDentPatientList.Columns["InsUptDlt"].DefaultValue = 0;

                    //DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData();

                    //if (!dtLocalPatient.Columns.Contains("InsUptDlt"))
                    //{
                    //    dtLocalPatient.Columns.Add("InsUptDlt", typeof(int));
                    //    dtLocalPatient.Columns["InsUptDlt"].DefaultValue = 0;
                    //}

                    ////dtLocalProvider = CompareDataTableRecords(ref dtSoftDentProvider, dtLocalProvider, "Provider_EHR_ID", "Provider_LocalDB_ID", "Provider_LocalDB_ID,Provider_EHR_ID,Provider_Web_ID,image,Is_Adit_Updated,Last_Sync_Date,InsUptDlt");

                    //dtLocalPatient = CompareDataTableRecords(ref dtSoftDentPatientList, dtLocalPatient, "Patient_EHR_ID", "Patient_LocalDB_ID", "Patient_LocalDB_ID,Patient_EHR_ID,Patient_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,ReceiveSms,ReceiveEmail");

                    ////DataTable dtSoftDentPatientdue_date = SynchSoftDentBAL.GetSoftDentPatientdue_date();

                    ////TotalPatientRecord = dtSoftDentPatient.Rows.Count;
                    ////GetPatientRecord = 0;

                    //dtSoftDentPatientList.AcceptChanges();


                    //bool status = false;
                    //DataTable dtSaveRecords = dtSoftDentPatientList.Clone();
                    //ObjGoalBase.WriteToSyncLogFile("Patient Sync SynchPartial_PracticeWork Send to save");
                    //if (dtSoftDentPatientList.Select("InsUptDlt IN (1,2)").Count() > 0)
                    //{
                    //    dtSaveRecords.Load(dtSoftDentPatientList.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                    //    ObjGoalBase.WriteToSyncLogFile("Patient Sync SynchPartial_PracticeWork Send to save BAL");
                    //    status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Patient", "Patient_LocalDB_ID,Patient_Web_ID", "Patient_LocalDB_ID");
                    //}
                    //else
                    //{
                    //    if (dtSoftDentPatientList.Select("InsUptDlt IN (4)").Count() > 0)
                    //    {
                    //        status = true;
                    //    }
                    //}
                    //if (status)
                    //{
                    //    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                    //    ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    //    SynchDataLiveDB_Push_Patient();
                    //}

                    ////if (dtSoftDentPatientList != null && dtSoftDentPatientList.Rows.Count > 0)
                    ////{
                    ////    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                    ////    IsGetParientRecordDone = true;
                    ////}
                    ////else
                    ////{
                    ////    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                    ////    IsGetParientRecordDone = true;
                    ////}
                    //IsPatientSyncedFirstTime = true;
                }
                catch (Exception ex)
                {
                    Is_synched_Patient = false;
                    ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
            else if (Is_synched_AppointmentsPatient)
            {
                Is_Synched_PatientCallHit = true;
            }
        }

        public void SynchDataPracticeWork_AppointmentsPatient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    ObjGoalBase.WriteToSyncLogFile("Appintment's Patient Sync SynchPartial_PracticeWork Start");
                    DataTable dtSoftDentPatientList = SynchPracticeWorkBAL.GetPracticeWorkAppointmentsPatientData();

                    string patientTableName = "Patient";
                    string PatientEHRIDs = string.Join("','", dtSoftDentPatientList.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                    if (PatientEHRIDs != string.Empty)
                    {
                        Is_synched_AppointmentsPatient = true;

                        PatientEHRIDs = "'" + PatientEHRIDs + "'";

                        ObjGoalBase.WriteToSyncLogFile("Appintment's Patient count " + dtSoftDentPatientList.Rows.Count.ToString());
                        DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, "1");
                        ObjGoalBase.WriteToSyncLogFile("Appintment's Patient Local count " + dtSoftDentPatientList.Rows.Count.ToString());
                        if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                        {
                            patientTableName = "PatientCompare";
                        }
                        ObjGoalBase.WriteToSyncLogFile("Appintment's Patient TableName " + dtSoftDentPatientList.Rows.Count.ToString());
                        if (dtSoftDentPatientList != null && dtSoftDentPatientList.Rows.Count > 0)
                        {
                            ObjGoalBase.WriteToSyncLogFile("Appintment's Patient Send to save");

                            bool isPatientSave = SynchClearDentBAL.Save_Patient_ClearDent_To_Local(dtSoftDentPatientList, patientTableName);
                            if (isPatientSave)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                ObjGoalBase.WriteToSyncLogFile("Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                IsGetParientRecordDone = true;
                                SynchDataLiveDB_Push_Patient();
                            }
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                            IsGetParientRecordDone = true;
                        }
                        Is_synched_AppointmentsPatient = false;
                        //  SynchDataPracticeWork_PatientDisease();
                        if (Is_Synched_PatientCallHit)
                        {
                            //Is_Synched_PatientCallHit = false;
                            //SynchDataPracticeWork_Patient();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Is_synched_AppointmentsPatient = false;
                    ObjGoalBase.WriteToErrorLogFile("[Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        public void SynchDataPracticeWork_PatientStatus()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWorkPatientStatus = SynchPracticeWorkBAL.GetPracticeWorkPatientStatusData("", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        if (dtPracticeWorkPatientStatus != null && dtPracticeWorkPatientStatus.Rows.Count > 0)
                        {
                            SynchLocalBAL.UpdatePatient_Status(dtPracticeWorkPatientStatus, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientStatus");
                        ObjGoalBase.WriteToSyncLogFile("PatientStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    }
                    SynchDataLiveDB_Push_PatientStatus();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);

            }
        }

        #endregion

        #region Synch RecallType

        private void fncSynchDataPracticeWork_RecallType()
        {
            InitBgWorkerPracticeWork_RecallType();
            InitBgTimerPracticeWork_RecallType();
        }

        private void InitBgTimerPracticeWork_RecallType()
        {
            timerSynchPracticeWork_RecallType = new System.Timers.Timer();
            this.timerSynchPracticeWork_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchPracticeWork_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_RecallType_Tick);
            timerSynchPracticeWork_RecallType.Enabled = true;
            timerSynchPracticeWork_RecallType.Start();
        }

        private void InitBgWorkerPracticeWork_RecallType()
        {
            bwSynchPracticeWork_RecallType = new BackgroundWorker();
            bwSynchPracticeWork_RecallType.WorkerReportsProgress = true;
            bwSynchPracticeWork_RecallType.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_RecallType.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_RecallType_DoWork);
            bwSynchPracticeWork_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_RecallType_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_RecallType.Enabled = false;
                MethodForCallSynchOrderPracticeWork_RecallType();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_RecallType()
        {
            System.Threading.Thread procThreadmainPracticeWork_RecallType = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_RecallType);
            procThreadmainPracticeWork_RecallType.Start();
        }

        public void CallSyncOrderTablePracticeWork_RecallType()
        {
            if (bwSynchPracticeWork_RecallType.IsBusy != true)
            {
                bwSynchPracticeWork_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_RecallType.Enabled = true;
        }

        public void SynchDataPracticeWork_RecallType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtPracticeWorkRecallType = SynchPracticeWorkBAL.GetPracticeWorkRecallTypeData();
                    dtPracticeWorkRecallType.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData("1");

                    foreach (DataRow dtDtxRow in dtPracticeWorkRecallType.Rows)
                    {
                        DataRow[] row = dtLocalRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["RecallType_Name"].ToString().ToLower().Trim() != row[0]["RecallType_Name"].ToString().ToLower().Trim())
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
                    foreach (DataRow dtDtxRow in dtLocalRecallType.Rows)
                    {
                        DataRow[] row = dtPracticeWorkRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtPracticeWorkRecallType.NewRow();
                            BlcOptDtldr["RecallType_EHR_ID"] = dtDtxRow["RecallType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtPracticeWorkRecallType.Rows.Add(BlcOptDtldr);
                        }
                    }

                    dtPracticeWorkRecallType.AcceptChanges();

                    if (dtPracticeWorkRecallType != null && dtPracticeWorkRecallType.Rows.Count > 0)
                    {
                        bool status = SynchTrackerBAL.Save_Tracker_To_Local(dtPracticeWorkRecallType, "RecallType", "RecallType_LocalDB_ID,RecallType_Web_ID", "RecallType_LocalDB_ID");
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
                            ObjGoalBase.WriteToSyncLogFile("RecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            SynchDataLiveDB_Push_RecallType();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[RecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch ApptStatus

        private void fncSynchDataPracticeWork_ApptStatus()
        {
            InitBgWorkerPracticeWork_ApptStatus();
            InitBgTimerPracticeWork_ApptStatus();
        }

        private void InitBgTimerPracticeWork_ApptStatus()
        {
            timerSynchPracticeWork_ApptStatus = new System.Timers.Timer();
            this.timerSynchPracticeWork_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchPracticeWork_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_ApptStatus_Tick);
            timerSynchPracticeWork_ApptStatus.Enabled = true;
            timerSynchPracticeWork_ApptStatus.Start();
        }

        private void InitBgWorkerPracticeWork_ApptStatus()
        {
            bwSynchPracticeWork_ApptStatus = new BackgroundWorker();
            bwSynchPracticeWork_ApptStatus.WorkerReportsProgress = true;
            bwSynchPracticeWork_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_ApptStatus_DoWork);
            bwSynchPracticeWork_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_ApptStatus.Enabled = false;
                MethodForCallSynchOrderPracticeWork_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_ApptStatus()
        {
            System.Threading.Thread procThreadmainPracticeWork_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_ApptStatus);
            procThreadmainPracticeWork_ApptStatus.Start();
        }

        public void CallSyncOrderTablePracticeWork_ApptStatus()
        {
            if (bwSynchPracticeWork_ApptStatus.IsBusy != true)
            {
                bwSynchPracticeWork_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_ApptStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_ApptStatus.Enabled = true;
        }

        public void SynchDataPracticeWork_ApptStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentApptStatus = SynchPracticeWorkBAL.GetPracticeWorkApptStatusData();

                    dtSoftDentApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentApptStatus.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData("1");

                    if (!dtLocalApptStatus.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalApptStatus.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalApptStatus.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalApptStatus = CompareDataTableRecords(ref dtSoftDentApptStatus, dtLocalApptStatus, "ApptStatus_Name", "ApptStatus_LocalDB_ID", "ApptStatus_LocalDB_ID,ApptStatus_EHR_ID,ApptStatus_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    dtSoftDentApptStatus.AcceptChanges();

                    bool status = false;
                    DataTable dtSaveRecords = dtSoftDentApptStatus.Clone();
                    if (dtSoftDentApptStatus.Select("InsUptDlt IN (1,2)").Count() > 0)
                    {
                        dtSaveRecords.Load(dtSoftDentApptStatus.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                        status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Appointment_Status", "ApptStatus_LocalDB_ID,ApptStatus_Web_ID", "ApptStatus_LocalDB_ID");
                    }
                    else
                    {
                        if (dtSoftDentApptStatus.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                        ObjGoalBase.WriteToSyncLogFile("ApptStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        IsPracticeWorkApptStatusSync = true;
                        SynchDataLiveDB_Push_ApptStatus();
                    }
                    else
                    {
                        IsPracticeWorkApptStatusSync = false;
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ApptStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Holiday

        private void fncSynchDataPracticeWork_Holiday()
        {
            InitBgWorkerPracticeWork_Holiday();
            InitBgTimerPracticeWork_Holiday();
        }

        private void InitBgTimerPracticeWork_Holiday()
        {
            timerSynchPracticeWork_Holiday = new System.Timers.Timer();
            this.timerSynchPracticeWork_Holiday.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchPracticeWork_Holiday.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_Holiday_Tick);
            timerSynchPracticeWork_Holiday.Enabled = true;
            timerSynchPracticeWork_Holiday.Start();
        }

        private void InitBgWorkerPracticeWork_Holiday()
        {
            bwSynchPracticeWork_Holiday = new BackgroundWorker();
            bwSynchPracticeWork_Holiday.WorkerReportsProgress = true;
            bwSynchPracticeWork_Holiday.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_Holiday.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_Holiday_DoWork);
            bwSynchPracticeWork_Holiday.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_Holiday_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_Holiday_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_Holiday.Enabled = false;
                MethodForCallSynchOrderPracticeWork_Holiday();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_Holiday()
        {
            System.Threading.Thread procThreadmainPracticeWork_Holiday = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_Holiday);
            procThreadmainPracticeWork_Holiday.Start();
        }

        public void CallSyncOrderTablePracticeWork_Holiday()
        {
            if (bwSynchPracticeWork_Holiday.IsBusy != true)
            {
                bwSynchPracticeWork_Holiday.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_Holiday_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_Holiday.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_Holiday();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_Holiday_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_Holiday.Enabled = true;
        }

        public void SynchDataPracticeWork_Holiday()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtPracticeWorkHoliday = SynchPracticeWorkBAL.GetPracticeWorkHolidayData();

                    if (dtPracticeWorkHoliday != null && dtPracticeWorkHoliday.Rows.Count > 0 && dtPracticeWorkHoliday.Columns != null && dtPracticeWorkHoliday.Columns.Count > 0)
                    {
                        dtPracticeWorkHoliday.Columns.Add("InsUptDlt", typeof(int));
                        dtPracticeWorkHoliday.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    DataTable dtLocalHoliday = SynchLocalBAL.GetLocalHolidayData("1");
                    if(dtPracticeWorkHoliday == null)
                    {
                        dtLocalHoliday.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalHoliday.Columns["InsUptDlt"].DefaultValue = 0;
                    }
                    dtPracticeWorkHoliday = dtLocalHoliday.Clone();
                    dtPracticeWorkHoliday = CommonUtility.AddHolidays(dtPracticeWorkHoliday, dtLocalHoliday, "SchedDate", "comment", "H_EHR_ID");


                    foreach (DataRow dtDtxRow in dtPracticeWorkHoliday.Rows)
                    {
                        DataRow[] row = dtLocalHoliday.Copy().Select("SchedDate = '" + Convert.ToDateTime(dtDtxRow["SchedDate"].ToString()) + "'");
                        if (row.Length > 0)
                        {
                            if (Convert.ToString(dtDtxRow["comment"].ToString().Trim().ToUpper()) != Convert.ToString(row[0]["comment"]).ToUpper())
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
                        try
                        {
                            if (dtPracticeWorkHoliday.Columns.Contains("H_Operatory_EHR_ID") && dtPracticeWorkHoliday.Columns["H_Operatory_EHR_ID"] != null && string.IsNullOrEmpty(dtDtxRow["H_Operatory_EHR_ID"].ToString()))
                            {
                                dtDtxRow["H_Operatory_EHR_ID"] = "0";
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    foreach (DataRow dtDtxRow in dtLocalHoliday.Rows)
                    {
                        DataRow[] row = dtPracticeWorkHoliday.Copy().Select("SchedDate = '" + dtDtxRow["SchedDate"] + "'");
                        if (row.Length <= 0)
                        {
                            DataRow ApptDtldr = dtPracticeWorkHoliday.NewRow();
                            ApptDtldr["SchedDate"] = dtDtxRow["SchedDate"].ToString().Trim();
                            ApptDtldr["InsUptDlt"] = 3;
                            dtPracticeWorkHoliday.Rows.Add(ApptDtldr);
                        }
                        try
                        {
                            if (dtLocalHoliday.Columns.Contains("H_Operatory_EHR_ID") && dtLocalHoliday.Columns["H_Operatory_EHR_ID"] != null && string.IsNullOrEmpty(dtDtxRow["H_Operatory_EHR_ID"].ToString()))
                            {
                                dtDtxRow["H_Operatory_EHR_ID"] = "0";
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (!dtLocalHoliday.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalHoliday.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalHoliday.Columns["InsUptDlt"].DefaultValue = 0;
                    }


                    //dtLocalHoliday = CompareDataTableRecords(ref dtPracticeWorkHoliday, dtLocalHoliday, "H_EHR_ID", "H_LocalDB_ID", "H_LocalDB_ID,H_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    dtPracticeWorkHoliday.AcceptChanges();
                    dtLocalHoliday.AcceptChanges();

                    if ((dtPracticeWorkHoliday != null && dtPracticeWorkHoliday.Rows.Count > 0) || (dtLocalHoliday != null && dtLocalHoliday.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtPracticeWorkHoliday.Clone();
                        if (dtPracticeWorkHoliday.Select("InsUptDlt IN (1,2)").Count() > 0 || dtLocalHoliday.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtPracticeWorkHoliday.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtPracticeWorkHoliday.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtLocalHoliday.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtLocalHoliday.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Holiday", "H_LocalDB_ID,H_Web_ID", "H_LocalDB_ID");
                        }
                        else
                        {
                            if (dtPracticeWorkHoliday.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");
                            ObjGoalBase.WriteToSyncLogFile("Holiday Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Holiday();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }

        #endregion

        #region Create Appointment

        private void fncSynchDataLocalToPracticeWork_Appointment()
        {
            InitBgWorkerLocalToPracticeWork_Appointment();
            InitBgTimerLocalToPracticeWork_Appointment();
        }

        private void InitBgTimerLocalToPracticeWork_Appointment()
        {
            timerSynchLocalToPracticeWork_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToPracticeWork_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToPracticeWork_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToPracticeWork_Appointment_Tick);
            timerSynchLocalToPracticeWork_Appointment.Enabled = true;
            timerSynchLocalToPracticeWork_Appointment.Start();
            timerSynchLocalToPracticeWork_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToPracticeWork_Appointment()
        {
            bwSynchLocalToPracticeWork_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToPracticeWork_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToPracticeWork_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToPracticeWork_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToPracticeWork_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToPracticeWork_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToPracticeWork_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToPracticeWork_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToPracticeWork_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToPracticeWork_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToPracticeWork_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToPracticeWork_Appointment);
            procThreadmainLocalToPracticeWork_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToPracticeWork_Appointment()
        {
            if (bwSynchLocalToPracticeWork_Appointment.IsBusy != true)
            {
                bwSynchLocalToPracticeWork_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToPracticeWork_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToPracticeWork_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToPracticeWork_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToPracticeWork_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToPracticeWork_Appointment.Enabled = true;
        }

        public void SynchDataLocalToPracticeWork_Appointment()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    //Utility.WriteToSyncLogFile_All("Start Insert Appointment_Fetch Appointment");

                    DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData("1");
                    //Utility.WriteToSyncLogFile_All("GetPracticeWorkPatientListData");
                    DataTable dtPracticeWorkPatient = SynchPracticeWorkBAL.GetPracticeWorkPatientListData();
                    // Utility.WriteToSyncLogFile_All("GetPracticeWorkDefaultProviderData");
                    DataTable dtIdelProv = SynchPracticeWorkBAL.GetPracticeWorkDefaultProviderData();
                    // Utility.WriteToSyncLogFile_All("End_GetPracticeWorkDefaultProviderData");

                    string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                    string tmpApptProv = "";
                    Int64 tmpPatient_id = 0;
                    Int64 tmpPatient_Gur_id = 0;
                    Int64 tmpAppt_EHR_id = 0;
                    //int tmpNewPatient = 1;

                    string tmpLastName = "";
                    string tmpFirstName = "";

                    string TmpWebPatientName = "";
                    string TmpWebRevPatientName = "";

                    // Utility.WriteToSyncLogFile_All("Start For Loop");

                    if (dtWebAppointment != null)
                    {
                        if (dtWebAppointment.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                        }
                    }

                    foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                    {
                        tmpPatient_id = 0;
                        tmpPatient_Gur_id = 0;
                        tmpAppt_EHR_id = 0;
                        TmpWebPatientName = "";
                        TmpWebRevPatientName = "";

                        //TmpWebPatientName = dtDtxRow["First_Name"].ToString().Trim();
                        //TmpWebRevPatientName = dtDtxRow["Last_Name"].ToString().Trim();

                        Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);

                        tmpApptProv = dtDtxRow["Provider_EHR_ID"].ToString().Trim();

                        if (tmpApptProv == "" || tmpApptProv == "0" || tmpApptProv == "-")
                        {
                            tmpApptProv = tmpIdelProv;
                        }

                        #region Set Operatory
                        // Utility.WriteToSyncLogFile_All("Set Operatory");
                        string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
                        // Utility.WriteToSyncLogFile_All("Set Operatory 1.1");
                        DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                        // Utility.WriteToSyncLogFile_All("Set Operatory 1.1.2");
                        DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                        TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                        // Utility.WriteToSyncLogFile_All("Set Appointment TIme " + tmpStartTime.ToString() + " " + tmpEndTime.ToString());
                        // Utility.WriteToSyncLogFile_All("Set Operatory 1.2");
                        int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);
                        //  Utility.WriteToSyncLogFile_All("Set Operatory 1.3");
                        string tmpApptDurPatern = "";
                        for (int i = 0; i < tmpApptDurMinutes / 5; i++)
                        {
                            tmpApptDurPatern = tmpApptDurPatern + "X";
                        }
                        //select * FROM Appointment where start_Time > '2020/02/13 00:00' AND End_Time < '2020/02/14 00:00'
                        // Utility.WriteToSyncLogFile_All("Get Operatory 1.4" + dtDtxRow["Appt_DateTime"].ToString());
                        DataTable dtBookOperatoryApptWiseDateTime = SynchPracticeWorkBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
                        //Utility.WriteToSyncLogFile_All("Get Operatory 1.5");
                        foreach (DataRow drrow in dtBookOperatoryApptWiseDateTime.Rows)
                        {
                            try
                            {
                                //Utility.WriteToSyncLogFile_All("Appt Datetime " + drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM", "") + " Appt Start TIme " + drrow["Start Time"].ToString());
                                //drrow["Appt_datetime"] = Convert.ToDateTime(drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM","")).AddHours(Convert.ToDateTime(drrow["Start Time"].ToString()));
                                drrow["Appt_datetime"] = Convert.ToDateTime(drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["Start Time"].ToString());
                                //Utility.WriteToSyncLogFile_All("Appt Datetime is " + drrow["Appt_datetime"].ToString());


                                drrow["Appt_EndDateTime"] = Convert.ToDateTime(drrow["Appt_EndDateTime"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["End Time"].ToString());
                                //Utility.WriteToSyncLogFile_All("Appt Datetime is " + drrow["Appt_EndDateTime"].ToString());
                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToSyncLogFile_All("Appt Error " + ex.Message);
                            }
                        }
                        int tmpIdealOperatory = 0;
                        string appointment_EHR_id = "";
                        // Utility.WriteToSyncLogFile_All("Set Operatory 2");
                        if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
                        {
                            int tmpCheckOpId = 0;
                            bool IsConflict = false;
                            for (int i = 0; i < Operatory_EHR_IDs.Length; i++)
                            {
                                IsConflict = false;
                                tmpCheckOpId = Convert.ToInt32(Operatory_EHR_IDs[i].ToString());
                                DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("Operatory_EHR_Id = '" + tmpCheckOpId + "'");
                                if (rowBookOpTime.Length > 0)
                                {
                                    for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                    {
                                        appointment_EHR_id = rowBookOpTime[Bop]["Appt_EHR_ID"].ToString();
                                        if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["Appt_DateTime"].ToString()))
                                            && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["Appt_EndDateTime"].ToString())))
                                        {
                                            IsConflict = true;
                                            break;
                                        }
                                        else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["Appt_DateTime"].ToString()))
                                            && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["Appt_EndDateTime"].ToString())))
                                        {
                                            IsConflict = true;
                                            break;
                                        }
                                    }
                                }
                                if (IsConflict == false)
                                {
                                    tmpIdealOperatory = tmpCheckOpId;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            tmpIdealOperatory = Convert.ToInt32(Operatory_EHR_IDs[0].ToString());
                        }


                        #endregion
                        // Utility.WriteToSyncLogFile_All("Set Operatory 3");
                        #region Set Patient
                        if (tmpIdealOperatory == 0)
                        {
                            DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appt_EHR_ID = " + appointment_EHR_id).CopyToDataTable();
                            bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());
                        }
                        else
                        {
                            tmpPatient_id = 0;
                            // Utility.WriteToSyncLogFile_All("Set Patient 1");
                            DataRow[] row = dtPracticeWorkPatient.Copy().Select("Mobile = '" + dtDtxRow["Mobile_Contact"].ToString().Trim().Replace("-", "") + "' OR Home_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim().Replace("-", "") + "' OR Work_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim().Replace("-", "") + "' ");
                            // Utility.WriteToSyncLogFile_All("Set Patient 1.1");
                            if (row.Length > 0)
                            {
                                //Utility.WriteToSyncLogFile_All("Found Patient 1.1");
                                // Utility.WriteToSyncLogFile_All("Start Loop to compare Patient 1.1");
                                for (int i = 0; i < row.Length; i++)
                                {
                                    if (row[i]["Patient_Name"].ToString().Trim().ToUpper() == TmpWebPatientName.ToString().Trim().ToUpper())
                                    {
                                        // Utility.WriteToSyncLogFile_All("PatientName Matched " + row[i]["Patient_Name"].ToString().Trim().ToUpper() + " " + TmpWebPatientName.ToString().Trim().ToUpper());
                                        tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                    }
                                    else if (row[i]["Patient_Name"].ToString().Trim().ToUpper() == TmpWebRevPatientName.ToString().Trim().ToUpper())
                                    {
                                        //Utility.WriteToSyncLogFile_All("PatientRevName Matched " + row[i]["Patient_Name"].ToString().Trim().ToUpper() + " " + TmpWebRevPatientName.ToString().Trim().ToUpper());
                                        tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                    }
                                    else if (row[i]["FirstName"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                    {
                                        // Utility.WriteToSyncLogFile_All("FirstName Matched " + row[i]["FirstName"].ToString().Trim().ToUpper() + " " + TmpWebPatientName.ToString().Trim().ToUpper());
                                        tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                    }
                                    else if (row[i]["FirstName"].ToString().ToLower().Trim() == dtDtxRow["First_Name"].ToString().Trim().ToLower())
                                    {
                                        //Utility.WriteToSyncLogFile_All("FirstName Matched 1.1 " + row[i]["FirstName"].ToString().Trim().ToUpper() + " " + dtDtxRow["First_Name"].ToString().Trim().ToLower());
                                        tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                    }
                                    else if (row[i]["FirstName"].ToString().ToLower().Trim() == (TmpWebPatientName.ToString().IndexOf(" ") > 0 ? TmpWebPatientName.Substring(0, TmpWebPatientName.ToString().IndexOf(" ")).ToLower() : TmpWebPatientName))
                                    {
                                        // Utility.WriteToSyncLogFile_All("FirstName Matched 1.2");
                                        tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                    }
                                    if (tmpPatient_id > 0)
                                    {
                                        break;
                                    }
                                }
                                // Utility.WriteToSyncLogFile_All("Found Party Id : " + tmpPatient_id.ToString());
                                // Utility.WriteToSyncLogFile_All("End Loop to compare Patient 1.1");
                                tmpPatient_Gur_id = Convert.ToInt32(row[0]["responsible_party"].ToString());
                                // Utility.WriteToSyncLogFile_All("Set Responsible Party " + tmpPatient_Gur_id.ToString());
                            }
                            if (tmpPatient_id == 0)
                            {
                                // Utility.WriteToSyncLogFile_All("Patient Not found in EHR 1.1");
                                if (dtDtxRow["Last_Name"].ToString().Trim() == null || dtDtxRow["Last_Name"].ToString().Trim() == "")
                                {
                                    //string[] tmpPatientName = dtDtxRow["First_Name"].ToString().Trim().Split(' ');

                                    string tmpPatientName = dtDtxRow["First_Name"].ToString().Trim();
                                    var firstSpaceIndex = tmpPatientName.IndexOf(" ");

                                    if (tmpPatientName.Contains(" "))
                                    {
                                        tmpFirstName = tmpPatientName.Substring(0, firstSpaceIndex).ToString().Trim();
                                        tmpLastName = tmpPatientName.Substring(firstSpaceIndex + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                                        tmpLastName = "Na";
                                    }
                                }
                                else
                                {
                                    tmpLastName = dtDtxRow["Last_Name"].ToString().Trim();
                                    tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                                }
                                // Utility.WriteToSyncLogFile_All("Patient Not Exists so Send to Save in EHR ");
                                tmpPatient_id = SynchPracticeWorkBAL.Save_Patient_Local_To_PracticeWork(tmpLastName.Trim(),
                                                                                                    tmpFirstName,
                                                                                                    dtDtxRow["MI"].ToString().Trim(),
                                                                                                    dtDtxRow["Mobile_Contact"].ToString().Trim().Replace("-", ""),
                                                                                                    dtDtxRow["Email"].ToString().Trim(),
                                                                                                    tmpApptProv,
                                                                                                    dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                                                                                    tmpPatient_Gur_id,
                                                                                                    tmpIdealOperatory,
                                                                                                    dtDtxRow["Birth_Date"].ToString().Trim());
                            }
                            #endregion
                            // Utility.WriteToSyncLogFile_All("END Set Patient");
                            if (tmpPatient_id > 0)
                            {
                                // Utility.WriteToSyncLogFile_All("Send to Save Appointment");
                                tmpAppt_EHR_id = SynchPracticeWorkBAL.Save_Appointment_Local_To_PracticeWork(TmpWebPatientName, tmpStartTime, tmpEndTime, tmpPatient_id.ToString(), tmpIdealOperatory.ToString(), "1", dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                       Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), tmpApptProv, dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "0", false, false, false, false);
                                // Utility.WriteToSyncLogFile_All("Appointment Saved");
                                if (tmpAppt_EHR_id > 0)
                                {
                                    bool isApptId_Update = SynchPracticeWorkBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1");
                                }
                                // Utility.WriteToSyncLogFile_All("Appointment Saved _ 1");
                            }
                        }
                    }

                    ObjGoalBase.WriteToSyncLogFile("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #endregion

        #region Patient Form

        private void fncSynchDataLocalToPracticeWork_Patient_Form()
        {
            InitBgWorkerLocalToPracticeWork_Patient_Form();
            InitBgTimerLocalToPracticeWork_Patient_Form();
        }

        private void InitBgTimerLocalToPracticeWork_Patient_Form()
        {
            timerSynchLocalToPracticeWork_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToPracticeWork_Patient_Form.Interval = 1000 * GoalBase.intervalWebSynch_Pull_PatientForm;
            this.timerSynchLocalToPracticeWork_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToPracticeWork_Patient_Form_Tick);
            timerSynchLocalToPracticeWork_Patient_Form.Enabled = true;
            timerSynchLocalToPracticeWork_Patient_Form.Start();
            timerSynchLocalToPracticeWork_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToPracticeWork_Patient_Form()
        {
            bwSynchLocalToPracticeWork_Patient_Form = new BackgroundWorker();
            bwSynchLocalToPracticeWork_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToPracticeWork_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToPracticeWork_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToPracticeWork_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToPracticeWork_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToPracticeWork_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToPracticeWork_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToPracticeWork_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToPracticeWork_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToPracticeWork_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToPracticeWork_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToPracticeWork_Patient_Form);
            procThreadmainLocalToPracticeWork_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToPracticeWork_Patient_Form()
        {
            if (bwSynchLocalToPracticeWork_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToPracticeWork_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToPracticeWork_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToPracticeWork_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToPracticeWork_Patient_Form();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToPracticeWork_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToPracticeWork_Patient_Form.Enabled = true;
        }

        public void SynchDataLocalToPracticeWork_Patient_Form()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    //Utility.WriteToSyncLogFile_All("Start SynchDataLocalToPracticeWork_Patient_Form");

                    SynchDataLiveDB_Pull_PatientForm();
                    SynchDataLiveDB_Pull_PatientPortal();

                    //Utility.WriteToSyncLogFile_All("GOt SynchDataLiveDB_Pull_PatientForm");

                    DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData("1");

                    //Utility.WriteToSyncLogFile_All("GOt Local Ptaitn Form data");

                    dtWebPatient_Form.Columns.Add("IsPatient", typeof(string));
                    dtWebPatient_Form.Columns.Add("OriginalFieldName", typeof(string));

                    //Utility.WriteToSyncLogFile_All("Loop Starts");

                    if (dtWebPatient_Form != null)
                    {
                        if (dtWebPatient_Form.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                        }
                    }

                    foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                    {

                        if (dtDtxRow["ehrfield"].ToString().Trim() == "first_name")
                        {
                            dtDtxRow["OriginalFieldName"] = "First name";
                            dtDtxRow["IsPatient"] = "Both";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "last_name")
                        {
                            dtDtxRow["OriginalFieldName"] = "Last name";
                            dtDtxRow["IsPatient"] = "Both";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "mobile")
                        {
                            if (Utility.Is_PWPatientCellPhoneAvailable)
                            {
                                dtDtxRow["OriginalFieldName"] = "CellPhone";
                            }
                            else
                            {
                                dtDtxRow["OriginalFieldName"] = "Filler";
                            }
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_one")
                        {
                            dtDtxRow["OriginalFieldName"] = "Address";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_two")
                        {
                            dtDtxRow["OriginalFieldName"] = "Address 2";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "birth_date")
                        {
                            dtDtxRow["OriginalFieldName"] = "Birthdate";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "city")
                        {
                            dtDtxRow["OriginalFieldName"] = "City";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "STATE")
                        {
                            dtDtxRow["OriginalFieldName"] = "State";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "email")
                        {
                            dtDtxRow["OriginalFieldName"] = "EMail address";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "home_phone")
                        {
                            dtDtxRow["OriginalFieldName"] = "Home phone";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "marital_status")
                        {
                            dtDtxRow["OriginalFieldName"] = "Marital status";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "middle_name")
                        {
                            dtDtxRow["OriginalFieldName"] = "Mid init";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "preferred_name")
                        {
                            dtDtxRow["OriginalFieldName"] = "LegalName";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "pri_provider_id")
                        {
                            dtDtxRow["OriginalFieldName"] = "Provider Emp ID";
                            dtDtxRow["IsPatient"] = "Patient";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance_companyname")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_email")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_sms")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "salutation")
                        {
                            dtDtxRow["OriginalFieldName"] = "Title";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "sec_provider_id")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance_companyname")
                        {
                            dtDtxRow["OriginalFieldName"] = "";
                            dtDtxRow["IsPatient"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "sex")
                        {
                            dtDtxRow["OriginalFieldName"] = "Sex";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "work_phone")
                        {
                            dtDtxRow["OriginalFieldName"] = "Work phone 1";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "zipcode")
                        {
                            dtDtxRow["OriginalFieldName"] = "ZIP";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SSN")
                        {
                            dtDtxRow["OriginalFieldName"] = "SSN";
                            dtDtxRow["IsPatient"] = "Person";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SCHOOL")
                        {
                            dtDtxRow["OriginalFieldName"] = " School Name";
                            dtDtxRow["IsPatient"] = "Patient";
                        }

                        dtWebPatient_Form.AcceptChanges();

                    }
                    //Utility.WriteToSyncLogFile_All("Loop END");
                    if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                    {
                        //Utility.WriteToSyncLogFile_All("Send TO save");
                        bool Is_Record_Update = SynchPracticeWorkBAL.Save_Patient_Form_Local_To_PracticeWork(dtWebPatient_Form, "1");
                        //Utility.WriteToSyncLogFile_All("Records saved to EHR");
                    }

                    try
                    {
                        if (SynchPracticeWorkBAL.SaveAllergiesToPracticeWork("1"))
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        if (SynchPracticeWorkBAL.DeleteAllergiesToPracticeWork("1"))
                        {
                            ObjGoalBase.WriteToSyncLogFile("Delete_Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                    }

                    bool isRecordDeleted = false, isRecordSaved = false;
                    string Patient_EHR_IDS = "";
                    string DeletePatientEHRID = "";
                    string SavePatientEHRID = "";
                    try
                    {
                        if (SynchPracticeWorkBAL.DeleteMedicationToPracticeWork("1", ref isRecordDeleted, ref DeletePatientEHRID))
                        {
                            ObjGoalBase.WriteToSyncLogFile("Delete_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") ]");
                    }
                    try
                    {
                        if (SynchPracticeWorkBAL.SaveMedicationToPracticeWork("1", ref isRecordSaved, ref SavePatientEHRID))
                        {
                            ObjGoalBase.WriteToSyncLogFile("Save_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Save_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Save_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") ]");
                    }
                    if (isRecordSaved || isRecordDeleted)
                    {
                        Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                        if (Patient_EHR_IDS != "")
                        {
                            SynchDataPracticeWork_PatientMedication(Patient_EHR_IDS);
                        }
                    }

                    string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing("1");
                    if (Call_Importing.ToLower() != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Importing status : " + Call_Importing);
                    }

                    string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed("1");
                    if (Call_Completed.ToLower() != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Completed status : " + Call_Completed);
                    }
                    //  GetPatientDocument();
                    ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");

                    string Call_PatientPortalCompleted = "";
                    try
                    {
                        Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed("1", Utility.Location_ID);
                        if (Call_PatientPortalCompleted != "success")
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("[Patient_Portal API called with Completed status : " + Call_PatientPortalCompleted);
                        }
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted + ex.Message.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        #endregion

        #region Synch Disease

        private void fncSynchDataPracticeWork_Disease()
        {
            InitBgWorkerPracticeWork_Disease();
            InitBgTimerPracticeWork_Disease();
        }

        private void InitBgTimerPracticeWork_Disease()
        {
            timerSynchPracticeWork_Disease = new System.Timers.Timer();
            this.timerSynchPracticeWork_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchPracticeWork_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_Disease_Tick);
            timerSynchPracticeWork_Disease.Enabled = true;
            timerSynchPracticeWork_Disease.Start();
        }

        private void InitBgWorkerPracticeWork_Disease()
        {
            bwSynchPracticeWork_Disease = new BackgroundWorker();
            bwSynchPracticeWork_Disease.WorkerReportsProgress = true;
            bwSynchPracticeWork_Disease.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_Disease.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_Disease_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWork_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_Disease_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_Disease_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_Disease.Enabled = false;
                MethodForCallSynchOrderPracticeWork_Disease();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_Disease()
        {
            System.Threading.Thread procThreadmainPracticeWork_Disease = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_Disease);
            procThreadmainPracticeWork_Disease.Start();
        }

        public void CallSyncOrderTablePracticeWork_Disease()
        {
            if (bwSynchPracticeWork_Disease.IsBusy != true)
            {
                bwSynchPracticeWork_Disease.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_Disease_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_Disease.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_Disease();
                SynchDataPracticeWork_PatientDisease();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_Disease.Enabled = true;
        }

        #region patientdisease
        public void SynchDataPracticeWork_PatientDisease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && !Is_synched_PatientDisease)
                {
                    Is_synched_PatientDisease = true;
                    DataTable dtPracticeWorkPatientDisease = new DataTable();
                    try
                    {
                        dtPracticeWorkPatientDisease = SynchPracticeWorkBAL.GetPracticeWorkPatientDiseaseData();
                    }
                    catch (Exception ex)
                    {
                        Is_synched_PatientDisease = false;
                        ObjGoalBase.WriteToErrorLogFile("[PatientDisease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                        return;
                    }
                    //  DataTable dtPracticeWorkPatientDisease = SynchPracticeWorkBAL.GetPracticeWorkPatientDiseaseData();
                    dtPracticeWorkPatientDisease.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalPatientDiseaseData = SynchLocalBAL.GetLocalPatientDiseaseData("1");
                    foreach (DataRow dtDtxRow in dtPracticeWorkPatientDisease.Rows)
                    {
                        DataRow[] row = dtLocalPatientDiseaseData.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "' And Disease_EHR_ID='" + dtDtxRow["Disease_EHR_ID"].ToString() + "'");

                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Patient_EHR_ID"].ToString().Trim() == row[0]["Patient_EHR_ID"].ToString().Trim() &&
                                dtDtxRow["Disease_EHR_ID"].ToString().Trim() != row[0]["Disease_EHR_ID"].ToString().Trim())
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
                    foreach (DataRow dtRow in dtLocalPatientDiseaseData.Rows)
                    {
                        DataRow rowBlcOpt = dtPracticeWorkPatientDisease.Copy().Select("Disease_EHR_ID = '" + dtRow["Disease_EHR_ID"].ToString().Trim() + "' and Patient_EHR_ID = '" + dtRow["Patient_EHR_ID"].ToString() + "'").FirstOrDefault();
                        if (rowBlcOpt == null)
                        {
                            DataRow dtxtldr = dtPracticeWorkPatientDisease.NewRow();
                            dtxtldr["Patient_EHR_ID"] = dtRow["Patient_EHR_ID"].ToString().Trim();
                            dtxtldr["Disease_EHR_ID"] = dtRow["Disease_EHR_ID"].ToString().Trim();
                            dtxtldr["Disease_Type"] = dtRow["Disease_Type"].ToString().Trim();
                            dtxtldr["Clinic_Number"] = dtRow["Clinic_Number"].ToString().Trim();
                            dtxtldr["is_deleted"] = 1;
                            dtxtldr["Is_Adit_Updated"] = 0;
                            dtxtldr["InsUptDlt"] = 3;
                            dtPracticeWorkPatientDisease.Rows.Add(dtxtldr);
                        }
                    }
                    dtPracticeWorkPatientDisease.AcceptChanges();
                    if (dtPracticeWorkPatientDisease != null && dtPracticeWorkPatientDisease.Rows.Count > 0)
                    {
                        bool status = SynchEaglesoftBAL.Save_PatientDisease_EagleSoft_To_Local(dtPracticeWorkPatientDisease, "1");

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                            ObjGoalBase.WriteToSyncLogFile("Patient Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_PatientDisease();
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient Disease Sync (" + Utility.Application_Name + " Db : " + "1" + " to Local Database) ] Error.");
                        }
                    }
                    Is_synched_PatientDisease = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatientDisease = false;
                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        #endregion

        public void SynchDataPracticeWork_Disease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    //Utility.WriteToSyncLogFile_All("Start GetPracticeWorkDiseaseData");
                    DataTable dtPracticeWorkDisease = SynchPracticeWorkBAL.GetPracticeWorkDiseaseData();
                    //Utility.WriteToSyncLogFile_All("Got GetPracticeWorkDiseaseData");
                    dtPracticeWorkDisease.Columns.Add("InsUptDlt", typeof(int));
                    dtPracticeWorkDisease.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalDiseaseData = SynchLocalBAL.GetLocalDiseaseData("1");

                    if (!dtLocalDiseaseData.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalDiseaseData.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalDiseaseData.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalDiseaseData = CompareDataTableRecords(ref dtPracticeWorkDisease, dtLocalDiseaseData, "Disease_Name", "Disease_LocalDB_ID", "Disease_LocalDB_ID,Disease_EHR_ID,Disease_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    dtPracticeWorkDisease.AcceptChanges();

                    bool status = false;
                    DataTable dtSaveRecords = dtPracticeWorkDisease.Clone();
                    if (dtPracticeWorkDisease.Select("InsUptDlt IN (1,2)").Count() > 0)
                    {
                        dtSaveRecords.Load(dtPracticeWorkDisease.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                        status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "DiseaseMaster", "Disease_LocalDB_ID,Disease_Web_ID", "Disease_LocalDB_ID");
                    }
                    else
                    {
                        if (dtPracticeWorkDisease.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("DiseaseMaster");
                        ObjGoalBase.WriteToSyncLogFile("Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        IsPracticeWorkApptTypeSync = true;
                        SynchDataLiveDB_Push_Disease();
                    }
                    else
                    {
                        IsPracticeWorkApptTypeSync = false;
                    }
                    SynchDataPracticeWork_Medication();
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataPracticeWork_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchPracticeWorkBAL.GetPracticeWorkMedicationData();
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtMedication.Rows)
                        {
                            DataRow[] row = dtLocalMedication.Copy().Select("Medication_EHR_ID = '" + dtDtxRow["Medication_EHR_ID"] + "' AND Medication_Type = '" + dtDtxRow["Medication_Type"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Medication_Name"].ToString().Trim() != row[0]["Medication_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Drug_Quantity"].ToString().ToUpper().Trim() != row[0]["Drug_Quantity"].ToString().ToUpper().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_Description"].ToString().Trim() != row[0]["Medication_Description"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_Notes"].ToString().Trim() != row[0]["Medication_Notes"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_Sig"].ToString().ToUpper().Trim() != row[0]["Medication_Sig"].ToString().ToUpper().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim() != row[0]["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_Type"].ToString().ToUpper().Trim() != row[0]["Medication_Type"].ToString().ToUpper().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Allow_Generic_Sub"].ToString().ToUpper().Trim() != row[0]["Allow_Generic_Sub"].ToString().ToUpper().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Refills"].ToString().ToUpper().Trim() != row[0]["Refills"].ToString().ToUpper().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Is_Active"].ToString().ToUpper().Trim() != row[0]["Is_Active"].ToString().ToUpper().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_Provider_ID"].ToString().ToUpper().Trim() != row[0]["Medication_Provider_ID"].ToString().ToUpper().Trim())
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

                        foreach (DataRow dtLPHRow in dtLocalMedication.Rows)
                        {
                            DataRow[] rowDis = dtMedication.Copy().Select("Medication_EHR_ID = '" + dtLPHRow["Medication_EHR_ID"] + "' AND Medication_Type = '" + dtLPHRow["Medication_Type"] + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"] + "' ");
                            if (rowDis.Length > 0)
                            { }
                            else
                            {
                                DataRow rowDisDtldr = dtMedication.NewRow();
                                rowDisDtldr["Medication_EHR_ID"] = dtLPHRow["Medication_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Medication_Type"] = dtLPHRow["Medication_Type"].ToString().Trim();
                                rowDisDtldr["Medication_Name"] = dtLPHRow["Medication_Name"].ToString().Trim();
                                rowDisDtldr["Clinic_Number"] = dtLPHRow["Clinic_Number"].ToString().Trim();
                                //  rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
                                rowDisDtldr["InsUptDlt"] = 3;
                                dtMedication.Rows.Add(rowDisDtldr);
                            }
                        }
                        dtMedication.AcceptChanges();

                        if (dtMedication != null && dtMedication.Rows.Count > 0)
                        {
                            bool status = SynchLocalDAL.Save_Medication_EHR_To_Local(dtMedication, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Medication");
                                ObjGoalBase.WriteToSyncLogFile("Medication Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_Medication();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Medication Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Medication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataPracticeWork_PatientMedication(string Patient_EHR_IDS)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchPracticeWorkBAL.GetPracticeWorkPatientMedicationData(Patient_EHR_IDS);
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patient_EHR_IDS);

                        foreach (DataRow dtDtxRow in dtMedication.Rows)
                        {
                            DataRow[] row = dtLocalMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtDtxRow["PatientMedication_EHR_ID"].ToString().Trim() + "' And Medication_EHR_ID = '" + dtDtxRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Patient_EHR_ID"].ToString().Trim() != row[0]["Patient_EHR_ID"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_EHR_ID"].ToString().Trim() != row[0]["Medication_EHR_ID"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_Note"].ToString().Trim() != row[0]["Medication_Note"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Provider_EHR_ID"].ToString().Trim() != row[0]["Provider_EHR_ID"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Drug_Quantity"].ToString().Trim() != row[0]["Drug_Quantity"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Start_Date"].ToString().Trim() != row[0]["Start_Date"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["End_Date"].ToString().Trim() != row[0]["End_Date"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Patient_Notes"].ToString().Trim() != row[0]["Patient_Notes"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["is_active"].ToString().Trim() != row[0]["is_active"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["EHR_Entry_DateTime"].ToString().Trim() != row[0]["EHR_Entry_DateTime"].ToString().Trim())
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

                        foreach (DataRow dtLPHRow in dtLocalMedication.Rows)
                        {
                            DataRow[] rowDis = dtMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtLPHRow["PatientMedication_EHR_ID"].ToString() + "' And Medication_EHR_ID = '" + dtLPHRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtLPHRow["Patient_EHR_ID"].ToString() + "'");
                            if (rowDis.Length > 0)
                            { }
                            else
                            {
                                DataRow rowDisDtldr = dtMedication.NewRow();
                                rowDisDtldr["PatientMedication_EHR_ID"] = dtLPHRow["PatientMedication_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Patient_EHR_ID"] = dtLPHRow["Patient_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Medication_EHR_ID"] = dtLPHRow["Medication_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Medication_Type"] = dtLPHRow["Medication_Type"].ToString().Trim();
                                rowDisDtldr["Medication_Name"] = dtLPHRow["Medication_Name"].ToString().Trim();
                                rowDisDtldr["Clinic_Number"] = dtLPHRow["Clinic_Number"].ToString().Trim();
                                //  rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
                                rowDisDtldr["InsUptDlt"] = 3;
                                dtMedication.Rows.Add(rowDisDtldr);
                            }
                        }
                        dtMedication.AcceptChanges();

                        if (dtMedication != null && dtMedication.Rows.Count > 0)
                        {
                            bool status = SynchLocalBAL.Save_PatientMedication_EHR_To_Local(dtMedication, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Medication");
                                ObjGoalBase.WriteToSyncLogFile("PatientMedication Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_PatientMedication();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[PatientMedication Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }

                    }
                    Is_synched_PatientMedication = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatientMedication = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientMedication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        #endregion

        #region Synch Insurance

        private void fncSynchDataPracticeWork_Insurance()
        {
            InitBgWorkerPracticeWork_Insurance();
            InitBgTimerPracticeWork_Insurance();
        }

        private void InitBgTimerPracticeWork_Insurance()
        {
            timerSynchPracticeWork_Insurance = new System.Timers.Timer();
            this.timerSynchPracticeWork_Insurance.Interval = 1000 * GoalBase.intervalEHRSynch_Insurance;
            this.timerSynchPracticeWork_Insurance.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWork_Insurance_Tick);
            timerSynchPracticeWork_Insurance.Enabled = true;
            timerSynchPracticeWork_Insurance.Start();
        }

        private void InitBgWorkerPracticeWork_Insurance()
        {
            bwSynchPracticeWork_Insurance = new BackgroundWorker();
            bwSynchPracticeWork_Insurance.WorkerReportsProgress = true;
            bwSynchPracticeWork_Insurance.WorkerSupportsCancellation = true;
            bwSynchPracticeWork_Insurance.DoWork += new DoWorkEventHandler(bwSynchPracticeWork_Insurance_DoWork);
            bwSynchPracticeWork_Insurance.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWork_Insurance_RunWorkerCompleted);
        }

        private void timerSynchPracticeWork_Insurance_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWork_Insurance.Enabled = false;
                MethodForCallSynchOrderPracticeWork_Insurance();
            }
        }

        public void MethodForCallSynchOrderPracticeWork_Insurance()
        {
            System.Threading.Thread procThreadmainPracticeWork_Insurance = new System.Threading.Thread(this.CallSyncOrderTablePracticeWork_Insurance);
            procThreadmainPracticeWork_Insurance.Start();
        }

        public void CallSyncOrderTablePracticeWork_Insurance()
        {
            if (bwSynchPracticeWork_Insurance.IsBusy != true)
            {
                bwSynchPracticeWork_Insurance.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWork_Insurance_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWork_Insurance.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWork_Insurance();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWork_Insurance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWork_Insurance.Enabled = true;
        }

        public void SynchDataPracticeWork_Insurance()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentInsurance = SynchPracticeWorkBAL.GetPracticeWorkInsuranceData();

                    dtSoftDentInsurance.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentInsurance.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalInsurance = SynchLocalBAL.GetLocalInsuranceData("1");

                    if (!dtLocalInsurance.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalInsurance.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalInsurance.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    //  dtLocalInsurance = CompareDataTableRecords(ref dtSoftDentInsurance, dtLocalInsurance, "Type_Name", "Insurance_LocalDB_ID", "Insurance_LocalDB_ID,Insurance_EHR_ID,Insurance_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    bool status = false;
                    #region New Insurance Code
                    foreach (DataRow dtDtxRow in dtSoftDentInsurance.Rows)
                    {
                        DataRow[] row = dtLocalInsurance.Copy().Select("Insurance_EHR_ID = '" + dtDtxRow["Insurance Co ID"] + "' ");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Company Name"].ToString().ToLower().Trim() != row[0]["Insurance_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Company Address1"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Company Address2"].ToString().ToLower().Trim() != row[0]["Address2"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Company City"].ToString().ToLower().Trim() != row[0]["City"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Company State"].ToString().ToLower().Trim() != row[0]["State"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Company ZIP"].ToString().ToLower().Trim() != row[0]["Zipcode"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Company phone"].ToString().ToLower().Trim() != row[0]["Phone"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Elect Claims ID"].ToString().ToLower().Trim() != row[0]["ElectId"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            //else if (dtDtxRow["EmployerName"].ToString().ToLower().Trim() != row[0]["EmployerName"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}
                            //else if (dtDtxRow["Inactive"].ToString().ToLower().Trim() != row[0]["Is_Deleted"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 3;
                            //}
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
                    foreach (DataRow dtDtxRow in dtLocalInsurance.Rows)
                    {
                        DataRow[] row = dtSoftDentInsurance.Copy().Select("Insurance Co ID = '" + dtDtxRow["Insurance_EHR_ID"] + "' ");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtSoftDentInsurance.NewRow();
                            BlcOptDtldr["Insurance Co ID"] = dtDtxRow["Insurance_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["CarrierName"] = dtDtxRow["Insurance_Name"].ToString().Trim();
                            // BlcOptDtldr["Clinic_Number"] = dtDtxRow["Clinic_Number"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtSoftDentInsurance.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtSoftDentInsurance.AcceptChanges();
                    if (dtSoftDentInsurance != null && dtSoftDentInsurance.Rows.Count > 0)
                    {
                        status = SynchPracticeWorkBAL.Save_Insurance_PracticeWork_To_Local(dtSoftDentInsurance, "1");
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Insurance");
                            ObjGoalBase.WriteToSyncLogFile("Insurance Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Insurance();
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Insurance Sync (" + Utility.Application_Name + " to Local Database) ] Error...");
                        }
                        //status = SynchTrackerBAL.Save_Tracker_To_Local(dtSoftDentInsurance, "Appointment_Type", "Insurance_LocalDB_ID,Insurance_Web_ID", "Insurance_EHR_ID");
                        //if (status)
                        //{
                        //    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Insurance");
                        //    ObjGoalBase.WriteToSyncLogFile("Insurance Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        //    IsPracticeWorkInsuranceSync = true;
                        //    SynchDataLiveDB_Push_Insurance();
                        //}
                        //else
                        //{
                        //    IsPracticeWorkInsuranceSync = false;
                        //}
                    }
                }
                #endregion

                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ApptType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion


        #region Event Listener
        public static bool SynchDataLocalToPracticeWork_Appointment(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id)
        {
            string strDbConnString = "";
            string Location_ID = "";
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_Id + "'");
                strDbConnString = dbConStr[0]["DBConnString"].ToString();

                var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                Location_ID = LocID[0]["Location_ID"].ToString();

                //CheckEntryUserLoginIdExist();

                DataTable dtPracticeWorkPatient = SynchPracticeWorkBAL.GetPracticeWorkPatientListData();
                DataTable dtIdelProv = SynchPracticeWorkBAL.GetPracticeWorkDefaultProviderData();

                string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                string tmpApptProv = "";
                Int64 tmpPatient_id = 0;
                Int64 tmpPatient_Gur_id = 0;
                Int64 tmpAppt_EHR_id = 0;
                string tmpLastName = "";
                string tmpFirstName = "";
                string TmpWebPatientName = "";
                string TmpWebRevPatientName = "";

                //if (dtWebAppointment != null)
                //{
                //    if (dtWebAppointment.Rows.Count > 0)
                //    {
                //        Utility.CheckEntryUserLoginIdExist();
                //    }
                //}

                foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                {
                    tmpPatient_id = 0;
                    tmpPatient_Gur_id = 0;
                    tmpAppt_EHR_id = 0;
                    TmpWebPatientName = "";
                    TmpWebRevPatientName = "";

                    Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);

                    tmpApptProv = dtDtxRow["Provider_EHR_ID"].ToString().Trim();

                    if (tmpApptProv == "" || tmpApptProv == "0" || tmpApptProv == "-")
                    {
                        tmpApptProv = tmpIdelProv;
                    }

                    #region Set Operatory
                    string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
                    DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                    DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                    TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                    int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);
                    string tmpApptDurPatern = "";
                    for (int i = 0; i < tmpApptDurMinutes / 5; i++)
                    {
                        tmpApptDurPatern = tmpApptDurPatern + "X";
                    }
                    DataTable dtBookOperatoryApptWiseDateTime = SynchPracticeWorkBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
                    foreach (DataRow drrow in dtBookOperatoryApptWiseDateTime.Rows)
                    {
                        try
                        {
                            drrow["Appt_datetime"] = Convert.ToDateTime(drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["Start Time"].ToString());
                            drrow["Appt_EndDateTime"] = Convert.ToDateTime(drrow["Appt_EndDateTime"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["End Time"].ToString());
                        }
                        catch (Exception ex)
                        {
                            Utility.WritetoAditEventErrorLogFile_Static("Appt Error " + ex.Message);
                        }
                    }
                    int tmpIdealOperatory = 0;
                    string appointment_EHR_id = "";
                    if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
                    {
                        int tmpCheckOpId = 0;
                        bool IsConflict = false;
                        for (int i = 0; i < Operatory_EHR_IDs.Length; i++)
                        {
                            IsConflict = false;
                            tmpCheckOpId = Convert.ToInt32(Operatory_EHR_IDs[i].ToString());
                            DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("Operatory_EHR_Id = '" + tmpCheckOpId + "'");
                            if (rowBookOpTime.Length > 0)
                            {
                                for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                {
                                    appointment_EHR_id = rowBookOpTime[Bop]["Appt_EHR_ID"].ToString();
                                    if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["Appt_DateTime"].ToString()))
                                        && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["Appt_EndDateTime"].ToString())))
                                    {
                                        IsConflict = true;
                                        break;
                                    }
                                    else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["Appt_DateTime"].ToString()))
                                        && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["Appt_EndDateTime"].ToString())))
                                    {
                                        IsConflict = true;
                                        break;
                                    }
                                }
                            }
                            if (IsConflict == false)
                            {
                                tmpIdealOperatory = tmpCheckOpId;
                                break;
                            }
                        }
                    }
                    else
                    {
                        tmpIdealOperatory = Convert.ToInt32(Operatory_EHR_IDs[0].ToString());
                    }


                    #endregion
                    #region Set Patient
                    if (tmpIdealOperatory == 0)
                    {
                        DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appt_EHR_ID = " + appointment_EHR_id).CopyToDataTable();
                        bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());
                    }
                    else
                    {
                        tmpPatient_id = 0;
                        DataRow[] row = dtPracticeWorkPatient.Select("Mobile = '" + dtDtxRow["Mobile_Contact"].ToString().Trim().Replace("-", "") + "' OR Home_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim().Replace("-", "") + "' OR Work_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim().Replace("-", "") + "' ");
                        if (row.Length > 0)
                        {
                            for (int i = 0; i < row.Length; i++)
                            {
                                if (row[i]["Patient_Name"].ToString().Trim().ToUpper() == TmpWebPatientName.ToString().Trim().ToUpper())
                                {
                                    tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                }
                                else if (row[i]["Patient_Name"].ToString().Trim().ToUpper() == TmpWebRevPatientName.ToString().Trim().ToUpper())
                                {
                                    tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                }
                                else if (row[i]["FirstName"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                {
                                    tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                }
                                else if (row[i]["FirstName"].ToString().ToLower().Trim() == dtDtxRow["First_Name"].ToString().Trim().ToLower())
                                {
                                    tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                }
                                else if (row[i]["FirstName"].ToString().ToLower().Trim() == (TmpWebPatientName.ToString().IndexOf(" ") > 0 ? TmpWebPatientName.Substring(0, TmpWebPatientName.ToString().IndexOf(" ")).ToLower() : TmpWebPatientName))
                                {
                                    tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                }
                                if (tmpPatient_id > 0)
                                {
                                    break;
                                }
                            }
                            tmpPatient_Gur_id = Convert.ToInt32(row[0]["responsible_party"].ToString());
                        }
                        if (tmpPatient_id == 0)
                        {
                            if (dtDtxRow["Last_Name"].ToString().Trim() == null || dtDtxRow["Last_Name"].ToString().Trim() == "")
                            {
                                string tmpPatientName = dtDtxRow["First_Name"].ToString().Trim();
                                var firstSpaceIndex = tmpPatientName.IndexOf(" ");

                                if (tmpPatientName.Contains(" "))
                                {
                                    tmpFirstName = tmpPatientName.Substring(0, firstSpaceIndex).ToString().Trim();
                                    tmpLastName = tmpPatientName.Substring(firstSpaceIndex + 1).ToString().Trim();
                                }
                                else
                                {
                                    tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                                    tmpLastName = "Na";
                                }
                            }
                            else
                            {
                                tmpLastName = dtDtxRow["Last_Name"].ToString().Trim();
                                tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                            }
                            tmpPatient_id = SynchPracticeWorkBAL.Save_Patient_Local_To_PracticeWork(tmpLastName.Trim(),
                                                                                                tmpFirstName,
                                                                                                dtDtxRow["MI"].ToString().Trim(),
                                                                                                dtDtxRow["Mobile_Contact"].ToString().Trim().Replace("-", ""),
                                                                                                dtDtxRow["Email"].ToString().Trim(),
                                                                                                tmpApptProv,
                                                                                                dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                                                                                tmpPatient_Gur_id,
                                                                                                tmpIdealOperatory,
                                                                                                dtDtxRow["Birth_Date"].ToString().Trim());
                        }
                        #endregion
                        if (tmpPatient_id > 0)
                        {
                            tmpAppt_EHR_id = SynchPracticeWorkBAL.Save_Appointment_Local_To_PracticeWork(TmpWebPatientName, tmpStartTime, tmpEndTime, tmpPatient_id.ToString(), tmpIdealOperatory.ToString(), "1", dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                   Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), tmpApptProv, dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "0", false, false, false, false);
                            if (tmpAppt_EHR_id > 0)
                            {
                                bool isApptId_Update = SynchPracticeWorkBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1");
                            }
                        }
                    }
                }

                Utility.WritetoAditEventSyncLogFile_Static("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");


                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                return false;
            }
        }

        public static void SyncDataPracticeWork_AppointmentFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        {
            try
            {
                SynchDataPracticeWork_AppointmentsPatientFromEvent(strDbString, strPatID, Clinic_Number, Service_Install_Id);
                SynchDataPracticeWork_PatientStatusFromEvent(strDbString, strPatID, Clinic_Number, Service_Install_Id);
                SynchDataLiveDB_Push_Patient(strPatID);

                DataTable dtSoftDentAppointment = SynchPracticeWorkBAL.GetPracticeWorkAppointmentData(strApptID);
                dtSoftDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                dtSoftDentAppointment.Columns.Add("ProcedureCode", typeof(string));

                string ProcedureDesc = "";
                string ProcedureCode = "";

                DataTable DtPracticeWorkAppointment_Procedures_Data = SynchPracticeWorkBAL.GetPracticeWorkAppointment_Procedures_Data(strApptID);

                foreach (DataRow drrow in dtSoftDentAppointment.Rows)
                {
                    try
                    {
                        drrow["Appt_datetime"] = Convert.ToDateTime(drrow["Appt_datetime"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["Start Time"].ToString());
                        drrow["Appt_EndDateTime"] = Convert.ToDateTime(drrow["Appt_EndDateTime"].ToString().Replace(" 12:00:00 AM", "") + " " + drrow["End Time"].ToString()).AddMinutes(1).ToString();

                        ////////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                        ProcedureDesc = "";
                        ProcedureCode = "";

                        DataRow[] dtCurApptProcedure = DtPracticeWorkAppointment_Procedures_Data.Select("Appt_EHR_ID = '" + drrow["Appt_EHR_ID"].ToString().Trim() + "'");

                        foreach (var dtSinProc in dtCurApptProcedure.ToList())
                        {
                            ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim() + ',';
                        }

                        if (ProcedureCode.ToString().Length > 1)
                        {
                            ProcedureCode = ProcedureCode.Substring(0, ProcedureCode.Length - 1);
                            drrow["ProcedureCode"] = ProcedureCode;
                            drrow["ProcedureDesc"] = "";
                        }
                        else
                        {
                            drrow["ProcedureDesc"] = "";
                            drrow["ProcedureCode"] = "";
                        }
                        /////////////////////////////////////

                    }
                    catch (Exception ex)
                    {
                        Utility.WritetoAditEventErrorLogFile_Static("Appt Error Procedure Desc - Procedure Code, Error Message: " + ex.Message);
                    }

                    if (drrow["CheckoutTime"].ToString().Trim() != "00:00:00")
                    {
                        drrow["appointment_status_ehr_key"] = "5";
                        drrow["Appointment_Status"] = "CheckedOut";
                    }
                    else if (drrow["SeatedTime"].ToString().Trim() != "00:00:00")
                    {
                        drrow["appointment_status_ehr_key"] = "4";
                        drrow["Appointment_Status"] = "Seated";
                    }
                    else if (drrow["CheckinTime"].ToString().Trim() != "00:00:00")
                    {
                        drrow["appointment_status_ehr_key"] = "3";
                        drrow["Appointment_Status"] = "CheckIn";
                    }
                    else if (drrow["Confirmed Date"].ToString().Trim() != "")
                    {
                        drrow["appointment_status_ehr_key"] = "2";
                        drrow["Appointment_Status"] = "Confirmed";
                    }
                    else
                    {
                        drrow["appointment_status_ehr_key"] = "1";
                        drrow["Appointment_Status"] = "NotConfirm";
                    }

                    if (drrow["AppointmentActStatus"].ToString().Trim() == "3")
                    {
                        drrow["appointment_status_ehr_key"] = "5";
                        drrow["Appointment_Status"] = "CheckedOut";
                    }

                }

                dtSoftDentAppointment.Columns.Remove("CheckoutTime");
                dtSoftDentAppointment.Columns.Remove("Confirmed Date");
                dtSoftDentAppointment.Columns.Remove("SeatedTime");
                dtSoftDentAppointment.Columns.Remove("CheckinTime");
                dtSoftDentAppointment.Columns.Remove("Start Time");
                dtSoftDentAppointment.Columns.Remove("End Time");
                dtSoftDentAppointment.Columns.Remove("AppointmentActStatus");

                dtSoftDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                dtSoftDentAppointment.Columns["InsUptDlt"].DefaultValue = 0;

                DataTable dtLocalAppointment = new DataTable();
                DataTable dtTempResult = SynchLocalBAL.GetLocalAppointmentData(Service_Install_Id, strApptID);
                if (dtTempResult.Rows.Count > 0)
                {
                    try
                    {
                        dtLocalAppointment = dtTempResult.Select("Appt_DateTime >= '" + Utility.Datetimesetting().AddDays(-7).ToShortDateString() + "'").CopyToDataTable();
                    }
                    catch (Exception exe1)
                    {
                        dtLocalAppointment = dtTempResult.Clone();
                    }
                }

                if (!dtLocalAppointment.Columns.Contains("InsUptDlt"))
                {
                    dtLocalAppointment.Columns.Add("InsUptDlt", typeof(int));
                    dtLocalAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                }
                dtLocalAppointment = CompareDataTableRecords_Static(ref dtSoftDentAppointment, dtLocalAppointment, "Appt_EHR_ID", "Appt_LocalDB_ID", "Appt_LocalDB_ID,Appt_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Remind_DateTime,Patient_Status,Is_Appt_DoubleBook,InsuranceCompanyName,is_Status_Updated_From_Web,is_ehr_updated,is_deleted,EHR_Entry_DateTime,Entry_DateTime,Is_Appt,Clinic_Number,Service_Install_Id");

                dtSoftDentAppointment.AcceptChanges();
                dtLocalAppointment.AcceptChanges();

                bool status = false;
                DataTable dtSaveRecords = dtSoftDentAppointment.Clone();
                if (dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").Count() > 0 || dtLocalAppointment.Select("InsUptDlt IN (3)").Count() > 0)
                {
                    if (dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                    {
                        foreach (DataRow drrow in dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)"))
                        {
                            if (drrow["birth_date"].ToString() == "" || drrow["birth_date"] == string.Empty)
                            {
                                drrow["birth_date"] = DBNull.Value;
                                // dtSaveRecords.Rows.Add(drrow);
                            }
                            if (drrow["confirmed_status_ehr_key"].ToString() == "" || drrow["confirmed_status_ehr_key"] == string.Empty)
                            {
                                drrow["confirmed_status_ehr_key"] = DBNull.Value;
                            }
                        }
                        dtSaveRecords.Load(dtSoftDentAppointment.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                    }
                    if (dtLocalAppointment.Select("InsUptDlt IN (3)").Count() > 0)
                    {
                        foreach (DataRow drrow in dtLocalAppointment.Select("InsUptDlt IN (3)"))
                        {
                            if (drrow["birth_date"].ToString() == "" || drrow["birth_date"] == string.Empty)
                            {
                                drrow["birth_date"] = DBNull.Value;
                            }
                            if (drrow["confirmed_status_ehr_key"].ToString() == "" || drrow["confirmed_status_ehr_key"] == string.Empty)
                            {
                                drrow["confirmed_status_ehr_key"] = DBNull.Value;
                            }
                        }
                        dtSaveRecords.Load(dtLocalAppointment.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                    }
                    status = SynchPracticeWorkBAL.Save_Appointment_PracticeWork_To_Local(dtSaveRecords);
                }
                else
                {
                    if (dtSoftDentAppointment.Select("InsUptDlt IN (4)").Count() > 0)
                    {
                        status = true;
                    }
                }
                if (status)
                {
                    Utility.WritetoAditEventSyncLogFile_Static("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    SynchDataLiveDB_Push_Appointment(strApptID);
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncDataPracticeWork_AppointmentFromEvent Sync (PracticeWork to Local Database) ]" + ex.Message);
            }
        }

        public static void SynchDataPracticeWork_AppointmentsPatientFromEvent(string strDbString, string strPatID, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                DataTable dtPracticeWorkPatientList = SynchPracticeWorkBAL.GetPracticeWorkAppointmentsPatientData(strPatID);

                string patientTableName = "Patient";
                string PatientEHRIDs = string.Join("','", dtPracticeWorkPatientList.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                if (PatientEHRIDs != string.Empty)
                {
                    PatientEHRIDs = "'" + PatientEHRIDs + "'";

                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Service_Install_Id, strPatID);

                    DataTable dtSaveRecords = new DataTable();
                    dtSaveRecords = dtLocalPatient.Clone();

                    var itemsToBeUpdated = (from PracticeWorkPatient in dtPracticeWorkPatientList.AsEnumerable()
                                            join LocalPatient in dtLocalPatient.AsEnumerable()
                                            on PracticeWorkPatient["Patient_EHR_ID"].ToString().Trim() + "_" + PracticeWorkPatient["Clinic_Number"].ToString().Trim()
                                            equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                            where
                                             Convert.ToDateTime(PracticeWorkPatient["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(LocalPatient["EHR_Entry_DateTime"])
                                             ||
                                             (PracticeWorkPatient["nextvisit_date"] != DBNull.Value && PracticeWorkPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(PracticeWorkPatient["nextvisit_date"]) : DateTime.Now)
                                             !=
                                             (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)
                                             ||
                                             (PracticeWorkPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())
                                             ||
                                             (PracticeWorkPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())
                                             || (PracticeWorkPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                             || (PracticeWorkPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                             || (PracticeWorkPatient["Home_Phone"].ToString().Trim()) != (LocalPatient["Home_Phone"].ToString().Trim())
                                             || (PracticeWorkPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                             || (PracticeWorkPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                             || (PracticeWorkPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                             || (PracticeWorkPatient["Mobile"].ToString().Trim()) != (LocalPatient["Mobile"].ToString().Trim())
                                             || (PracticeWorkPatient["ReceiveSMS"].ToString().Trim()) != (LocalPatient["ReceiveSMS"].ToString().Trim())
                                             || (PracticeWorkPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
                                            select PracticeWorkPatient).ToList();

                    DataTable dtPatientToBeUpdated = dtLocalPatient.Clone();
                    if (itemsToBeUpdated.Count > 0)
                    {
                        dtPatientToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
                    }
                    if (!dtPatientToBeUpdated.Columns.Contains("InsUptDlt"))
                    {
                        dtPatientToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
                        dtPatientToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    if (dtPatientToBeUpdated.Rows.Count > 0)
                    {
                        dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
                        dtSaveRecords.Load(dtPatientToBeUpdated.Select().CopyToDataTable().CreateDataReader());
                    }


                    if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                    {
                        bool isPatientSave = SynchPracticeWorkBAL.Save_Patient_PracticeWork_To_Local_New(dtSaveRecords, Clinic_Number, Service_Install_Id);
                        if (isPatientSave)
                        {
                            Utility.WritetoAditEventSyncLogFile_Static("Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        }
                    }
                    else
                    {
                        Utility.WritetoAditEventSyncLogFile_Static("Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataPracticeWork_AppointmentsPatientFromEvent Sync (" + Utility.Application_Name + " to Local Database) ] Error: " + ex.Message);
            }
        }

        public static void SynchDataPracticeWork_PatientStatusFromEvent(string strDbString, string strPatID, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                #region "Patient Status"
                DataTable dtPracticeWorkPatientStatus = new DataTable();
                Utility.WritetoAditEventDebugLogFile_Static("3.1 GetPracticeWorkPatientStatusDataByPatID Start.");
                dtPracticeWorkPatientStatus = SynchPracticeWorkBAL.GetPracticeWorkPatientStatusData(Clinic_Number, strDbString, strPatID);
                Utility.WritetoAditEventDebugLogFile_Static("3.2 GetPracticeWorkPatientStatusDataByPatID End. Records:" + dtPracticeWorkPatientStatus.Rows.Count);
                if (dtPracticeWorkPatientStatus != null && dtPracticeWorkPatientStatus.Rows.Count > 0)
                {
                    Utility.WritetoAditEventDebugLogFile_Static("3.3 UpdatePatient_StatusByPatID Start.");
                    SynchLocalBAL.UpdatePatient_Status(dtPracticeWorkPatientStatus, Service_Install_Id, Clinic_Number, strPatID);
                    Utility.WritetoAditEventDebugLogFile_Static("3.4 UpdatePatient_StatusByPatID End.");
                    //SynchDataLiveDB_Push_PatientStatus(Convert.ToInt32(Service_Install_Id), Convert.ToInt32(Clinic_Number), strPatID);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncPracticeWork_PatientStatus Sync (PracticeWork to Local Database) ]" + ex.Message);
            }
        }

        private static DataTable CompareDataTableRecords_Static(ref DataTable dtSource, DataTable dtDestination, string compareColumnName, string primarykeyColumns, string ignoreColumns)
        {
            try
            {
                DataTable dtSourceTemp = dtSource;

                #region Provider available in Local Database Or Not available in local Database
                string[] ignoreColumnsArr = new string[] { "" };
                ignoreColumnsArr = ignoreColumns.Split(',');

                dtSource.AsEnumerable()
                    .All(o =>
                    {
                        var result = dtDestination.AsEnumerable().Where(a => a.Field<object>(compareColumnName).ToString().Trim() == o.Field<object>(compareColumnName).ToString().Trim());
                        if (result != null && result.Count() > 0)
                        {
                            foreach (DataColumn dcCol in dtSourceTemp.Columns)
                            {
                                var result1 = ignoreColumnsArr.AsEnumerable().Where(b => b.ToString().ToUpper() == dcCol.ColumnName.ToUpper());

                                if ((result1 != null && result1.Count() == 0))
                                {
                                    if (dcCol.DataType.Name.ToString().ToLower() == "boolean" || dcCol.DataType.Name.ToString().ToLower() == "sbyte")
                                    {
                                        if (result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName] != null && o[dcCol.ColumnName].ToString() != string.Empty && result.First().Field<object>(dcCol.ColumnName).ToString() != string.Empty
                                            && Convert.ToBoolean(o[dcCol.ColumnName]) != Convert.ToBoolean(result.First().Field<object>(dcCol.ColumnName)))
                                        {
                                            o["InsUptDlt"] = 2;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                            break;
                                        }
                                        else
                                        {
                                            o["InsUptDlt"] = 4;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                        }
                                    }
                                    else
                                    {
                                        if (dcCol.ColumnName.ToLower() == "birth_date")
                                        {
                                            try
                                            {
                                                if (result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName] != null
                                                    && string.IsNullOrEmpty(o[dcCol.ColumnName].ToString()) == false
                                                    && string.IsNullOrEmpty(result.First().Field<object>(dcCol.ColumnName).ToString()) == false)
                                                {
                                                    if (Convert.ToDateTime(o[dcCol.ColumnName]).ToShortDateString() != Convert.ToDateTime(result.First().Field<object>(dcCol.ColumnName)).ToShortDateString())
                                                    {
                                                        o["InsUptDlt"] = 2;
                                                        o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                        break;
                                                    }
                                                }
                                                else if (o[dcCol.ColumnName] == null && result.First().Field<object>(dcCol.ColumnName) != null)
                                                {
                                                    o["InsUptDlt"] = 2;
                                                    o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                    break;
                                                }
                                                else if (o[dcCol.ColumnName] != null && result.First().Field<object>(dcCol.ColumnName) == null)
                                                {
                                                    o["InsUptDlt"] = 2;
                                                    o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                    break;
                                                }
                                                else if (o[dcCol.ColumnName] != null && result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName].ToString() == "" && result.First().Field<object>(dcCol.ColumnName) != "")
                                                {
                                                    o["InsUptDlt"] = 2;
                                                    o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                    break;
                                                }
                                                else if (o[dcCol.ColumnName] != null && result.First().Field<object>(dcCol.ColumnName) != null && o[dcCol.ColumnName].ToString() != "" && result.First().Field<object>(dcCol.ColumnName) == "")
                                                {
                                                    o["InsUptDlt"] = 2;
                                                    o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                                    break;
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }
                                        else if (result.First().Field<object>(dcCol.ColumnName) != null && string.IsNullOrEmpty(result.First().Field<object>(dcCol.ColumnName).ToString()) == false && o[dcCol.ColumnName].ToString().Trim().ToLower() != result.First().Field<object>(dcCol.ColumnName).ToString().Trim().ToLower())
                                        {
                                            o["InsUptDlt"] = 2;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                            break;
                                        }
                                        else if ((result.First().Field<object>(dcCol.ColumnName) == null || (result.First().Field<object>(dcCol.ColumnName) != null && result.First().Field<object>(dcCol.ColumnName).ToString() == string.Empty))
                                            && o[dcCol.ColumnName] != null && o[dcCol.ColumnName].ToString().Trim().ToLower() != string.Empty)
                                        {
                                            o["InsUptDlt"] = 2;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                            break;
                                        }
                                        else
                                        {
                                            o["InsUptDlt"] = 4;
                                            o[primarykeyColumns] = result.First().Field<object>(primarykeyColumns);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            o["InsUptDlt"] = 1;
                        }
                        return true;
                    });

                #endregion

                #region Records available in local but not available in SoftDent
                dtDestination.AsEnumerable().Where(o => o.Field<object>(compareColumnName).ToString() != "0" && o.Field<object>(compareColumnName).ToString() != "")
                    .All(o =>
                    {
                        var result = dtSourceTemp.AsEnumerable().Where(a => a.Field<object>(compareColumnName).ToString().Trim() == o.Field<object>(compareColumnName).ToString().Trim());
                        if (result != null && result.Count() == 0)
                        {
                            o["InsUptDlt"] = 3;
                        }
                        else if (result == null)
                        {
                            o["InsUptDlt"] = 3;
                        }
                        return true;
                    });
                #endregion

                return dtDestination;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SynchDataLocalToPracticeWork_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
        {
            string strDbConnString = "";
            string Location_ID = "";
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_Id + "'");
                strDbConnString = dbConStr[0]["DBConnString"].ToString();

                var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                Location_ID = LocID[0]["Location_ID"].ToString();

                DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData("1", strPatientFormID);

                dtWebPatient_Form.Columns.Add("IsPatient", typeof(string));
                dtWebPatient_Form.Columns.Add("OriginalFieldName", typeof(string));

                if (dtWebPatient_Form != null)
                {
                    if (dtWebPatient_Form.Rows.Count > 0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                    }
                }

                foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                {

                    if (dtDtxRow["ehrfield"].ToString().Trim() == "first_name")
                    {
                        dtDtxRow["OriginalFieldName"] = "First name";
                        dtDtxRow["IsPatient"] = "Both";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "last_name")
                    {
                        dtDtxRow["OriginalFieldName"] = "Last name";
                        dtDtxRow["IsPatient"] = "Both";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "mobile")
                    {
                        if (Utility.Is_PWPatientCellPhoneAvailable)
                        {
                            dtDtxRow["OriginalFieldName"] = "CellPhone";
                        }
                        else
                        {
                            dtDtxRow["OriginalFieldName"] = "Filler";
                        }
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_one")
                    {
                        dtDtxRow["OriginalFieldName"] = "Address";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_two")
                    {
                        dtDtxRow["OriginalFieldName"] = "Address 2";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "birth_date")
                    {
                        dtDtxRow["OriginalFieldName"] = "Birthdate";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "city")
                    {
                        dtDtxRow["OriginalFieldName"] = "City";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "STATE")
                    {
                        dtDtxRow["OriginalFieldName"] = "State";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "email")
                    {
                        dtDtxRow["OriginalFieldName"] = "EMail address";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "home_phone")
                    {
                        dtDtxRow["OriginalFieldName"] = "Home phone";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "marital_status")
                    {
                        dtDtxRow["OriginalFieldName"] = "Marital status";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "middle_name")
                    {
                        dtDtxRow["OriginalFieldName"] = "Mid init";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "preferred_name")
                    {
                        dtDtxRow["OriginalFieldName"] = "LegalName";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "pri_provider_id")
                    {
                        dtDtxRow["OriginalFieldName"] = "Provider Emp ID";
                        dtDtxRow["IsPatient"] = "Patient";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance_companyname")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_email")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_sms")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "salutation")
                    {
                        dtDtxRow["OriginalFieldName"] = "Title";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "sec_provider_id")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance_companyname")
                    {
                        dtDtxRow["OriginalFieldName"] = "";
                        dtDtxRow["IsPatient"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "sex")
                    {
                        dtDtxRow["OriginalFieldName"] = "Sex";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "work_phone")
                    {
                        dtDtxRow["OriginalFieldName"] = "Work phone 1";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "zipcode")
                    {
                        dtDtxRow["OriginalFieldName"] = "ZIP";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SSN")
                    {
                        dtDtxRow["OriginalFieldName"] = "SSN";
                        dtDtxRow["IsPatient"] = "Person";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SCHOOL")
                    {
                        dtDtxRow["OriginalFieldName"] = " School Name";
                        dtDtxRow["IsPatient"] = "Patient";
                    }

                    dtWebPatient_Form.AcceptChanges();

                }
                //Utility.WriteToSyncLogFile_All("Loop END");
                if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                {
                    //Utility.WriteToSyncLogFile_All("Send TO save");
                    bool Is_Record_Update = SynchPracticeWorkBAL.Save_Patient_Form_Local_To_PracticeWork(dtWebPatient_Form, "1");
                    //Utility.WriteToSyncLogFile_All("Records saved to EHR");
                }

                try
                {
                    if (SynchPracticeWorkBAL.SaveAllergiesToPracticeWork("1", strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception)
                {

                }
                try
                {
                    if (SynchPracticeWorkBAL.DeleteAllergiesToPracticeWork("1", strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Delete_Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                }

                bool isRecordDeleted = false, isRecordSaved = false;
                string Patient_EHR_IDS = "";
                string DeletePatientEHRID = "";
                string SavePatientEHRID = "";
                try
                {
                    if (SynchPracticeWorkBAL.DeleteMedicationToPracticeWork("1", ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Delete_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Delete_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") ]");
                }
                try
                {
                    if (SynchPracticeWorkBAL.SaveMedicationToPracticeWork("1", ref isRecordSaved, ref SavePatientEHRID, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Save_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Save_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Save_Medication_To_PracticeWork Sync (Local Database To " + Utility.Application_Name + ") ]");
                }
                if (isRecordSaved || isRecordDeleted)
                {
                    Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                    if (Patient_EHR_IDS != "")
                    {
                        SynchDataPracticeWork_PatientMedication(Patient_EHR_IDS);
                    }
                }

                string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing("1", strPatientFormID);
                if (Call_Importing.ToLower() != "success")
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Importing status : " + Call_Importing);
                }

                string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed("1", strPatientFormID);
                if (Call_Completed.ToLower() != "success")
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Completed status : " + Call_Completed);
                }
                //  GetPatientDocument();
                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");

                string Call_PatientPortalCompleted = "";
                try
                {
                    Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed("1", Location_ID, strPatientFormID);
                    if (Call_PatientPortalCompleted != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile("[Patient_Portal API called with Completed status : " + Call_PatientPortalCompleted);
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted + ex.Message.ToString());
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        #endregion

    }
}
