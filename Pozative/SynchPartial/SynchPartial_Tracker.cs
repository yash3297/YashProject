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
using System.Reflection;
using Pozative.DAL;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable

        private int appointmenetSyncFailedCounter = 0;
        bool IsTrackerProviderSync = false;
        bool IsTrackerOperatorySync = false;
        bool IsTrackerApptTypeSync = false;
        bool IsTrackerApptStatusSync = false;
        bool IsTrackerProviderCallFromApplicationStart = false;
        //bool Is_synched = false;
        //bool Is_synched_Provider = false;
        //bool Is_synched_Speciality = false;
        //bool Is_synched_Operatory = false;
        //bool Is_synched_OperatoryEvent = false;
        //bool Is_synched_Type = false;
        //bool Is_synched_Appointment = false;
        //bool Is_synched_RecallType = false;
        //bool Is_synched_ApptStatus = false;
        private BackgroundWorker bwSynchTracker_Appointment = null;
        private System.Timers.Timer timerSynchTracker_Appointment = null;

        private BackgroundWorker bwSynchTracker_OperatoryEvent = null;
        private System.Timers.Timer timerSynchTracker_OperatoryEvent = null;

        private BackgroundWorker bwSynchTracker_Provider = null;
        private System.Timers.Timer timerSynchTracker_Provider = null;

        private BackgroundWorker bwSynchTracker_Speciality = null;
        private System.Timers.Timer timerSynchTracker_Speciality = null;

        private BackgroundWorker bwSynchTracker_Operatory = null;
        private System.Timers.Timer timerSynchTracker_Operatory = null;

        private BackgroundWorker bwSynchTracker_ApptType = null;
        private System.Timers.Timer timerSynchTracker_ApptType = null;

        private BackgroundWorker bwSynchTracker_Patient = null;
        private System.Timers.Timer timerSynchTracker_Patient = null;

        private BackgroundWorker bwSynchTracker_RecallType = null;
        private System.Timers.Timer timerSynchTracker_RecallType = null;

        private BackgroundWorker bwSynchTracker_User = null;
        private System.Timers.Timer timerSynchTracker_User = null;

        private BackgroundWorker bwSynchTracker_ApptStatus = null;
        private System.Timers.Timer timerSynchTracker_ApptStatus = null;

        private BackgroundWorker bwSynchLocalToTracker_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchLocalToTracker_Appointment = null;

        private BackgroundWorker bwSynchTracker_Holiday = null;
        private System.Timers.Timer timerSynchTracker_Holiday = null;

        private BackgroundWorker bwSynchLocalToTracker_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToTracker_Patient_Form = null;

        private BackgroundWorker bwSynchTracker_PatientPayment = null;
        private System.Timers.Timer timerSynchTracker_PatientPayment = null;

        private BackgroundWorker bwSynchTracker_Insurance = null;
        private System.Timers.Timer timerSynchTracker_Insurance = null;

        #endregion

        private void CallSynchTrackerToLocal()
        {
            if (Utility.AditSync)
            {
                
                // SynchDataLiveDB_PatientPayment_LocalToTracker();
                fncSynchDataTracker_PatientPayment();

                IsTrackerProviderCallFromApplicationStart = true;
                SynchDataTracker_Provider();
                fncSynchDataTracker_Provider();

                SynchDataTracker_Speciality();
                fncSynchDataTracker_Speciality();

                SynchDataTracker_Operatory();
                fncSynchDataTracker_Operatory();

                SynchDataTracker_ApptType();
                fncSynchDataTracker_ApptType();

                // SynchDataTracker_OperatoryEvent();
                fncSynchDataTracker_OperatoryEvent();



                //SynchDataTracker_Appointment();
                //fncSynchDataTracker_Appointment();

                if (Utility.ApptAutoBook)
                {
                    fncSynchDataLocalToTracker_Appointment();
                }

                //SynchDataTracker_Patient();
                //fncSynchDataTracker_Patient();

                SynchDataTracker_RecallType();
                fncSynchDataTracker_RecallType();

                SynchDataTracker_User();
                fncSynchDataTracker_User();

                SynchDataTracker_ApptStatus();
                fncSynchDataTracker_ApptStatus();

                SynchDataTracker_Holiday();
                fncSynchDataTracker_Holiday();

                SynchDataLocalToTracker_Patient_Form();
                fncSynchDataLocalToTracker_Patient_Form();

                SynchDataTracker_Insurance();
                fncSynchDataTracker_Insurance();

                ////SynchDataTracker_OperatoryEvent();

                ////SynchDataTracker_Appointment();

                ////SynchDataTracker_RecallType();

                //// SynchDataLocalToTracker_Appointment();
                //SynchDataLiveDB_Pull_PatientFollowUp();
                //SynchDataLiveDB_PatientSMSCall_LocalToTracker();

            }
        }

        #region Synch Appointment

        private void fncSynchDataTracker_Appointment()
        {
            InitBgWorkerTracker_Appointment();
            InitBgTimerTracker_Appointment();
        }

        private void InitBgTimerTracker_Appointment()
        {
            timerSynchTracker_Appointment = new System.Timers.Timer();
            this.timerSynchTracker_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchTracker_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_Appointment_Tick);
            timerSynchTracker_Appointment.Enabled = true;
            timerSynchTracker_Appointment.Start();
            timerSynchTracker_Appointment_Tick(null, null);
        }

        private void InitBgWorkerTracker_Appointment()
        {
            bwSynchTracker_Appointment = new BackgroundWorker();
            bwSynchTracker_Appointment.WorkerReportsProgress = true;
            bwSynchTracker_Appointment.WorkerSupportsCancellation = true;
            bwSynchTracker_Appointment.DoWork += new DoWorkEventHandler(bwSynchTracker_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchTracker_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_Appointment_RunWorkerCompleted);
        }

        private void timerSynchTracker_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_Appointment.Enabled = false;
                MethodForCallSynchOrderTracker_Appointment();
            }
        }

        public void MethodForCallSynchOrderTracker_Appointment()
        {
            System.Threading.Thread procThreadmainTracker_Appointment = new System.Threading.Thread(this.CallSyncOrderTableTracker_Appointment);
            procThreadmainTracker_Appointment.Start();
        }

        public void CallSyncOrderTableTracker_Appointment()
        {
            if (bwSynchTracker_Appointment.IsBusy != true)
            {
                bwSynchTracker_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLiveDB_PatientPayment_LocalToTracker();
                try
                {
                    SynchDataTracker_Appointment_New();
                }
                catch
                {
                    SynchDataTracker_Appointment();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_Appointment.Enabled = true;
        }

        public void SynchDataTracker_Appointment_New()
        {
            if (IsTrackerProviderSync && IsTrackerOperatorySync && IsTrackerApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    if (IsTrackerProviderSync && IsTrackerOperatorySync && IsTrackerApptTypeSync)
                    {
                        try
                        {
                            SynchDataTracker_AppointmentsPatient_New();
                        }
                        catch (Exception ex)
                        {
                            SynchDataTracker_AppointmentsPatient();
                        }
                        if (IsPatientSyncedFirstTime)
                        {
                            SynchDataTracker_NewPatient();
                        }
                        SynchDataTracker_PatientStatus();

                        DataTable dtSoftDentAppointment = SynchTrackerBAL.GetTrackerAppointmentData();

                        dtSoftDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                        dtSoftDentAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                        dtSoftDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                        dtSoftDentAppointment.Columns.Add("ProcedureCode", typeof(string));

                        DataTable DtTrackerAppointment_Procedures_Data = SynchTrackerBAL.GetTrackerAppointment_Procedures_Data();

                        string ProcedureDesc = "";
                        string ProcedureCode = "";
                        ///////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                        foreach (DataRow dtDtxRow in dtSoftDentAppointment.Rows)
                        {
                            ProcedureDesc = "";
                            ProcedureCode = "";
                            DataRow[] dtCurApptProcedure = DtTrackerAppointment_Procedures_Data.Select("Appointmentid = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");
                            foreach (var dtSinProc in dtCurApptProcedure.ToList())
                            {
                                ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                            }
                            dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                            dtDtxRow["ProcedureCode"] = ProcedureCode;

                        }
                        /////////////////////

                        DataTable dtLocalAppointment = new DataTable();
                        DataTable dtTempResult = SynchLocalBAL.GetLocalAppointmentData("1");

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

                        if (!dtLocalAppointment.Columns.Contains("InsUptDlt"))
                        {
                            dtLocalAppointment.Columns.Add("InsUptDlt", typeof(int));
                            dtLocalAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                        }

                        DataTable dtSaveRecords = new DataTable();
                        dtSaveRecords = dtLocalAppointment.Clone();
                        ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Compare Start. Start for patient to be added.");
                        var itemsToBeAdded = (from TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                                              join LocalAppt in dtLocalAppointment.AsEnumerable()
                                              on TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                                              equals LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                              into matchingRows
                                              from matchingRow in matchingRows.DefaultIfEmpty()
                                              where matchingRow == null
                                              select TrackerAppt).ToList();
                        DataTable dtApptToBeAdded = dtLocalAppointment.Clone();
                        if (itemsToBeAdded.Count > 0)
                        {
                            dtApptToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
                        }

                        if (!dtApptToBeAdded.Columns.Contains("InsUptDlt"))
                        {
                            dtApptToBeAdded.Columns.Add("InsUptDlt", typeof(int));
                            dtApptToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
                        }
                        if (dtApptToBeAdded.Rows.Count > 0)
                        {
                            dtApptToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
                            dtSaveRecords.Load(dtApptToBeAdded.Select().CopyToDataTable().CreateDataReader());
                        }

                        var itemToBeDeleted = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                               join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                                               on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                               equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                                               into matchingRows
                                               from matchingRow in matchingRows.DefaultIfEmpty()
                                               where LocalAppt["is_deleted"].ToString().Trim().ToUpper() == "FALSE" && matchingRow == null
                                               select LocalAppt).ToList();
                        DataTable dtApptToBeDelete = dtLocalAppointment.Clone();
                        if (itemToBeDeleted.Count > 0)
                        {
                            dtApptToBeDelete = itemToBeDeleted.CopyToDataTable<DataRow>();
                        }
                        if (!dtApptToBeDelete.Columns.Contains("InsUptDlt"))
                        {
                            dtApptToBeDelete.Columns.Add("InsUptDlt", typeof(int));
                            dtApptToBeDelete.Columns["InsUptDlt"].DefaultValue = 0;
                        }

                        if (dtApptToBeDelete.Rows.Count > 0)
                        {
                            dtApptToBeDelete.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 3);
                            dtSaveRecords.Load(dtApptToBeDelete.Select().CopyToDataTable().CreateDataReader());
                        }

                        var itemsToBeUpdated = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                                join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                                                on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                                equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                                                where
                                                (LocalAppt["First_name"] != DBNull.Value ? LocalAppt["First_name"].ToString().Trim() : "") != (TrackerAppt["First_name"] != DBNull.Value ? TrackerAppt["First_name"].ToString().Trim() : "") ||
                                                (LocalAppt["Last_name"] != DBNull.Value ? LocalAppt["Last_name"].ToString().Trim() : "") != (TrackerAppt["Last_name"] != DBNull.Value ? TrackerAppt["Last_name"].ToString().Trim() : "") ||
                                                (LocalAppt["MI"] != DBNull.Value ? LocalAppt["MI"].ToString().Trim() : "") != (TrackerAppt["MI"] != DBNull.Value ? TrackerAppt["MI"].ToString().Trim() : "") ||
                                                (LocalAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Home_Contact"].ToString().Trim()) : "") != (TrackerAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(TrackerAppt["Home_Contact"].ToString().Trim()) : "") ||
                                                (LocalAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Mobile_Contact"].ToString().Trim()) : "") != (TrackerAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(TrackerAppt["Mobile_Contact"].ToString().Trim()) : "") ||
                                                (LocalAppt["Email"] != DBNull.Value ? LocalAppt["Email"].ToString().Trim() : "") != (TrackerAppt["Email"] != DBNull.Value ? TrackerAppt["Email"].ToString().Trim() : "") ||
                                                (LocalAppt["Address"] != DBNull.Value ? LocalAppt["Address"].ToString().Trim() : "") != (TrackerAppt["Address"] != DBNull.Value ? TrackerAppt["Address"].ToString().Trim() : "") ||
                                                (LocalAppt["City"] != DBNull.Value ? LocalAppt["City"].ToString().Trim() : "") != (TrackerAppt["City"] != DBNull.Value ? TrackerAppt["City"].ToString().Trim() : "") ||
                                                (LocalAppt["Zip"] != DBNull.Value ? LocalAppt["Zip"].ToString().Trim() : "") != (TrackerAppt["Zip"] != DBNull.Value ? TrackerAppt["Zip"].ToString().Trim() : "") ||
                                                (LocalAppt["Operatory_EHR_ID"] != DBNull.Value ? LocalAppt["Operatory_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Operatory_EHR_ID"] != DBNull.Value ? TrackerAppt["Operatory_EHR_ID"].ToString().Trim() : "") ||
                                                (LocalAppt["Operatory_Name"] != DBNull.Value ? LocalAppt["Operatory_Name"].ToString().Trim() : "") != (TrackerAppt["Operatory_Name"] != DBNull.Value ? TrackerAppt["Operatory_Name"].ToString().Trim() : "") ||
                                                (LocalAppt["Provider_EHR_ID"] != DBNull.Value ? LocalAppt["Provider_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Provider_EHR_ID"] != DBNull.Value ? TrackerAppt["Provider_EHR_ID"].ToString().Trim() : "") ||
                                                (LocalAppt["Provider_Name"] != DBNull.Value ? LocalAppt["Provider_Name"].ToString().Trim() : "") != (TrackerAppt["Provider_Name"] != DBNull.Value ? TrackerAppt["Provider_Name"].ToString().Trim() : "") ||
                                                (LocalAppt["Comment"] != DBNull.Value ? LocalAppt["Comment"].ToString().Trim() : "") != (TrackerAppt["Comment"] != DBNull.Value ? TrackerAppt["Comment"].ToString().Trim() : "") ||
                                                (LocalAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["birth_date"].ToString().Trim()) : "") != (TrackerAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["birth_date"].ToString().Trim()) : "") ||
                                                (LocalAppt["ApptType_EHR_ID"] != DBNull.Value ? LocalAppt["ApptType_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["ApptType_EHR_ID"] != DBNull.Value ? TrackerAppt["ApptType_EHR_ID"].ToString().Trim() : "") ||
                                                (LocalAppt["ApptType"] != DBNull.Value ? LocalAppt["ApptType"].ToString().Trim() : "") != (TrackerAppt["ApptType"] != DBNull.Value ? TrackerAppt["ApptType"].ToString().Trim() : "") ||
                                                (LocalAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_DateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_DateTime"].ToString().Trim()) : "") ||
                                                (LocalAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_EndDateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_EndDateTime"].ToString().Trim()) : "") ||
                                                (LocalAppt["Status"] != DBNull.Value ? LocalAppt["Status"].ToString().Trim() : "") != (TrackerAppt["Status"] != DBNull.Value ? TrackerAppt["Status"].ToString().Trim() : "") ||
                                                (LocalAppt["appointment_status_ehr_key"] != DBNull.Value ? LocalAppt["appointment_status_ehr_key"].ToString().Trim() : "") != (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") ||
                                                (LocalAppt["Appointment_Status"] != DBNull.Value ? LocalAppt["Appointment_Status"].ToString().Trim() : "") != (TrackerAppt["Appointment_Status"] != DBNull.Value ? TrackerAppt["Appointment_Status"].ToString().Trim() : "") ||
                                                (LocalAppt["confirmed_status_ehr_key"] != DBNull.Value ? LocalAppt["confirmed_status_ehr_key"].ToString().Trim() : "") != (TrackerAppt["confirmed_status_ehr_key"] != DBNull.Value ? TrackerAppt["confirmed_status_ehr_key"].ToString().Trim() : "") ||
                                                (LocalAppt["confirmed_status"] != DBNull.Value ? LocalAppt["confirmed_status"].ToString().Trim() : "") != (TrackerAppt["confirmed_status"] != DBNull.Value ? TrackerAppt["confirmed_status"].ToString().Trim() : "") ||
                                                (LocalAppt["patient_ehr_id"] != DBNull.Value ? LocalAppt["patient_ehr_id"].ToString().Trim() : "") != (TrackerAppt["patient_ehr_id"] != DBNull.Value ? TrackerAppt["patient_ehr_id"].ToString().Trim() : "") ||
                                                (LocalAppt["ProcedureDesc"] != DBNull.Value ? LocalAppt["ProcedureDesc"].ToString().Trim() : "") != (TrackerAppt["ProcedureDesc"] != DBNull.Value ? TrackerAppt["ProcedureDesc"].ToString().Trim() : "") ||
                                                (LocalAppt["ProcedureCode"] != DBNull.Value ? LocalAppt["ProcedureCode"].ToString().Trim() : "") != (TrackerAppt["ProcedureCode"] != DBNull.Value ? TrackerAppt["ProcedureCode"].ToString().Trim() : "") ||
                                                (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim() : "") != (TrackerAppt["is_deleted"] != DBNull.Value ? TrackerAppt["is_deleted"].ToString().Trim() : "") ||
                                                (LocalAppt["is_asap"] != DBNull.Value ? LocalAppt["is_asap"].ToString().Trim() : "") != (TrackerAppt["is_asap"] != DBNull.Value ? TrackerAppt["is_asap"].ToString().Trim() : "")
                                                select TrackerAppt).ToList();
                        DataTable dtPatientToBeUpdated = dtLocalAppointment.Clone();
                        if (itemsToBeUpdated.Count > 0)
                        {
                            dtPatientToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
                        }
                        if (!dtPatientToBeUpdated.Columns.Contains("InsUptDlt"))
                        {
                            dtPatientToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
                            dtPatientToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
                        }

                        //try
                        //{
                        //    DataTable dtCheck = new DataTable();
                        //    string Appt_EHR_Ids = "";
                        //    var FName = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                 join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                 on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                 equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                 where
                        //                 (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                 (LocalAppt["First_name"] != DBNull.Value ? LocalAppt["First_name"].ToString().Trim() : "") != (TrackerAppt["First_name"] != DBNull.Value ? TrackerAppt["First_name"].ToString().Trim() : "")
                        //                 select TrackerAppt).ToList();

                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (FName.Count > 0)
                        //    {
                        //        dtCheck = FName.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var LName = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                 join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                 on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                 equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                 where
                        //                 (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                 (LocalAppt["Last_name"] != DBNull.Value ? LocalAppt["Last_name"].ToString().Trim() : "") != (TrackerAppt["Last_name"] != DBNull.Value ? TrackerAppt["Last_name"].ToString().Trim() : "")
                        //                 select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (LName.Count > 0)
                        //    {
                        //        dtCheck = LName.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var MI = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //              join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //              on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //              equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //              where
                        //              (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //              (LocalAppt["MI"] != DBNull.Value ? LocalAppt["MI"].ToString().Trim() : "") != (TrackerAppt["MI"] != DBNull.Value ? TrackerAppt["MI"].ToString().Trim() : "")
                        //              select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (MI.Count > 0)
                        //    {
                        //        dtCheck = MI.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Home_Contact = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                        join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                        on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                        equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                        where
                        //                        (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                        (LocalAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Home_Contact"].ToString().Trim()) : "") != (TrackerAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(TrackerAppt["Home_Contact"].ToString().Trim()) : "")
                        //                        select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Home_Contact.Count > 0)
                        //    {
                        //        dtCheck = Home_Contact.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Mobile_Contact = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                          join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                          on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                          equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                          where
                        //                          (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                          (LocalAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Mobile_Contact"].ToString().Trim()) : "") != (TrackerAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(TrackerAppt["Mobile_Contact"].ToString().Trim()) : "")
                        //                          select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Mobile_Contact.Count > 0)
                        //    {
                        //        dtCheck = Mobile_Contact.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Email = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                 join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                 on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                 equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                 where
                        //                 (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                 (LocalAppt["Email"] != DBNull.Value ? LocalAppt["Email"].ToString().Trim() : "") != (TrackerAppt["Email"] != DBNull.Value ? TrackerAppt["Email"].ToString().Trim() : "")
                        //                 select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Email.Count > 0)
                        //    {
                        //        dtCheck = Email.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Address = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                   join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                   on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                   equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                   where
                        //                   (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                   (LocalAppt["Address"] != DBNull.Value ? LocalAppt["Address"].ToString().Trim() : "") != (TrackerAppt["Address"] != DBNull.Value ? TrackerAppt["Address"].ToString().Trim() : "")
                        //                   select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Address.Count > 0)
                        //    {
                        //        dtCheck = Address.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var City = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                where
                        //                (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                (LocalAppt["City"] != DBNull.Value ? LocalAppt["City"].ToString().Trim() : "") != (TrackerAppt["City"] != DBNull.Value ? TrackerAppt["City"].ToString().Trim() : "")
                        //                select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (City.Count > 0)
                        //    {
                        //        dtCheck = City.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Zip = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //               join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //               on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //               equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //               where
                        //               (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //               (LocalAppt["Zip"] != DBNull.Value ? LocalAppt["Zip"].ToString().Trim() : "") != (TrackerAppt["Zip"] != DBNull.Value ? TrackerAppt["Zip"].ToString().Trim() : "")
                        //               select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Zip.Count > 0)
                        //    {
                        //        dtCheck = Zip.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Operatory_EHR_ID = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                            join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                            on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                            equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                            where
                        //                            (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                            (LocalAppt["Operatory_EHR_ID"] != DBNull.Value ? LocalAppt["Operatory_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Operatory_EHR_ID"] != DBNull.Value ? TrackerAppt["Operatory_EHR_ID"].ToString().Trim() : "")
                        //                            select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Operatory_EHR_ID.Count > 0)
                        //    {
                        //        dtCheck = Operatory_EHR_ID.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Operatory_Name = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                          join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                          on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                          equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                          where
                        //                          (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                          (LocalAppt["Operatory_Name"] != DBNull.Value ? LocalAppt["Operatory_Name"].ToString().Trim() : "") != (TrackerAppt["Operatory_Name"] != DBNull.Value ? TrackerAppt["Operatory_Name"].ToString().Trim() : "")
                        //                          select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Operatory_Name.Count > 0)
                        //    {
                        //        dtCheck = Operatory_Name.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Provider_EHR_ID = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                           join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                           on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                           equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                           where
                        //                           (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                           (LocalAppt["Provider_EHR_ID"] != DBNull.Value ? LocalAppt["Provider_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Provider_EHR_ID"] != DBNull.Value ? TrackerAppt["Provider_EHR_ID"].ToString().Trim() : "")
                        //                           select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Provider_EHR_ID.Count > 0)
                        //    {
                        //        dtCheck = Provider_EHR_ID.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Provider_Name = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                         join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                         on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                         equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                         where
                        //                         (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "Upper" &&
                        //                         (LocalAppt["Provider_Name"] != DBNull.Value ? LocalAppt["Provider_Name"].ToString().Trim() : "") != (TrackerAppt["Provider_Name"] != DBNull.Value ? TrackerAppt["Provider_Name"].ToString().Trim() : "")
                        //                         select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Provider_Name.Count > 0)
                        //    {
                        //        dtCheck = Provider_Name.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Comment = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                   join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                   on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                   equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                   where
                        //                   (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                   (LocalAppt["Comment"] != DBNull.Value ? LocalAppt["Comment"].ToString().Trim() : "") != (TrackerAppt["Comment"] != DBNull.Value ? TrackerAppt["Comment"].ToString().Trim() : "")
                        //                   select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Comment.Count > 0)
                        //    {
                        //        dtCheck = Comment.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var birth_date = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                      join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                      on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                      equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                      where
                        //                      (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                      (LocalAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["birth_date"].ToString().Trim()) : "") != (TrackerAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["birth_date"].ToString().Trim()) : "")
                        //                      select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (birth_date.Count > 0)
                        //    {
                        //        dtCheck = birth_date.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var ApptType_EHR_ID = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                           join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                           on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                           equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                           where
                        //                           (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                           (LocalAppt["ApptType_EHR_ID"] != DBNull.Value ? LocalAppt["ApptType_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["ApptType_EHR_ID"] != DBNull.Value ? TrackerAppt["ApptType_EHR_ID"].ToString().Trim() : "")
                        //                           select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (ApptType_EHR_ID.Count > 0)
                        //    {
                        //        dtCheck = ApptType_EHR_ID.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var ApptType = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                    join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                    on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                    equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                    where
                        //                    (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                    (LocalAppt["ApptType"] != DBNull.Value ? LocalAppt["ApptType"].ToString().Trim() : "") != (TrackerAppt["ApptType"] != DBNull.Value ? TrackerAppt["ApptType"].ToString().Trim() : "")
                        //                    select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (ApptType.Count > 0)
                        //    {
                        //        dtCheck = ApptType.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Appt_DateTime = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                         join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                         on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                         equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                         where
                        //                         (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                         (LocalAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_DateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_DateTime"].ToString().Trim()) : "")
                        //                         select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Appt_DateTime.Count > 0)
                        //    {
                        //        dtCheck = Appt_DateTime.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Appt_EndDateTime = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                            join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                            on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                            equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                            where
                        //                            (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                            (LocalAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_EndDateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_EndDateTime"].ToString().Trim()) : "")
                        //                            select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Appt_EndDateTime.Count > 0)
                        //    {
                        //        dtCheck = Appt_EndDateTime.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Status = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                  join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                  on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                  equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                  where
                        //                  (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                  (LocalAppt["Status"] != DBNull.Value ? LocalAppt["Status"].ToString().Trim() : "") != (TrackerAppt["Status"] != DBNull.Value ? TrackerAppt["Status"].ToString().Trim() : "")
                        //                  select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Status.Count > 0)
                        //    {
                        //        dtCheck = Status.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var appointment_status_ehr_key = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                                      join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                                      on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                                      equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                                      where
                        //                                      (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                                      (LocalAppt["appointment_status_ehr_key"] != DBNull.Value ? LocalAppt["appointment_status_ehr_key"].ToString().Trim() : "") != (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "")
                        //                                      select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (appointment_status_ehr_key.Count > 0)
                        //    {
                        //        dtCheck = appointment_status_ehr_key.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var Appointment_Status = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                              join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                              on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                              equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                              where
                        //                              (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                              (LocalAppt["Appointment_Status"] != DBNull.Value ? LocalAppt["Appointment_Status"].ToString().Trim() : "") != (TrackerAppt["Appointment_Status"] != DBNull.Value ? TrackerAppt["Appointment_Status"].ToString().Trim() : "")
                        //                              select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (Appointment_Status.Count > 0)
                        //    {
                        //        dtCheck = Appointment_Status.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var confirmed_status_ehr_key = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                                    join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                                    on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                                    equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                                    where
                        //                                    (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                                    (LocalAppt["confirmed_status_ehr_key"] != DBNull.Value ? LocalAppt["confirmed_status_ehr_key"].ToString().Trim() : "") != (TrackerAppt["confirmed_status_ehr_key"] != DBNull.Value ? TrackerAppt["confirmed_status_ehr_key"].ToString().Trim() : "")
                        //                                    select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (confirmed_status_ehr_key.Count > 0)
                        //    {
                        //        dtCheck = confirmed_status_ehr_key.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var confirmed_status = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                            join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                            on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                            equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                            where
                        //                            (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                            (LocalAppt["confirmed_status"] != DBNull.Value ? LocalAppt["confirmed_status"].ToString().Trim() : "") != (TrackerAppt["confirmed_status"] != DBNull.Value ? TrackerAppt["confirmed_status"].ToString().Trim() : "")
                        //                            select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (confirmed_status.Count > 0)
                        //    {
                        //        dtCheck = confirmed_status.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                        //    var patient_ehr_id = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                        //                          join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                        //                          on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                        //                          equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                        //                          where
                        //                          (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE" &&
                        //                          (LocalAppt["patient_ehr_id"] != DBNull.Value ? LocalAppt["patient_ehr_id"].ToString().Trim() : "") != (TrackerAppt["patient_ehr_id"] != DBNull.Value ? TrackerAppt["patient_ehr_id"].ToString().Trim() : "")
                        //                          select TrackerAppt).ToList();
                        //    dtCheck = new DataTable();
                        //    dtCheck = dtLocalAppointment.Clone();
                        //    if (patient_ehr_id.Count > 0)
                        //    {
                        //        dtCheck = patient_ehr_id.CopyToDataTable<DataRow>();
                        //    }
                        //    Appt_EHR_Ids = string.Join("','", dtCheck.AsEnumerable().Select(p => p.Field<object>("Appt_EHR_Id").ToString()));
                        //}
                        //catch (Exception ex)
                        //{
                        //}



                        if (dtPatientToBeUpdated.Rows.Count > 0)
                        {
                            var updateLocalDBID = from r1 in dtPatientToBeUpdated.AsEnumerable()
                                                  join r2 in dtLocalAppointment.AsEnumerable()
                                                  on r1["Appt_EHR_ID"].ToString().Trim() equals r2["Appt_EHR_ID"].ToString().Trim()
                                                  select new { r1, r2 };
                            foreach (var x in updateLocalDBID)
                            {
                                x.r1.SetField("Appt_LocalDB_ID", x.r2["Appt_LocalDB_ID"].ToString().Trim());
                            }

                            //dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
                            var itemToUpdate2 = from TrackerAppt in dtPatientToBeUpdated.AsEnumerable()
                                                where
                                                (TrackerAppt["InsUptDlt"] != DBNull.Value ? TrackerAppt["InsUptDlt"].ToString().Trim() : "") != "1"
                                                && (
                                                    (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "3" &&
                                                    (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "5" &&
                                                    (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "6" &&
                                                    (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "7" &&
                                                    (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "8" &&
                                                    (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "9"
                                                )
                                                select new { TrackerAppt };
                            foreach (var x in itemToUpdate2)
                            {
                                x.TrackerAppt.SetField("InsUptDlt", "2");
                            }
                            var updateIsDeletedByStaus = from TrackerAppt in dtPatientToBeUpdated.AsEnumerable()
                                                         join LocalAppt in dtLocalAppointment.AsEnumerable()
                                                         on TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                                                         equals LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                                         where (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE"
                                                         && (
                                                         (TrackerAppt["InsUptDlt"] != DBNull.Value ? TrackerAppt["InsUptDlt"].ToString().Trim().ToUpper() : "") != "1"
                                                         && (
                                                               (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "3"
                                                               || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "5"
                                                               || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "6"
                                                               || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "7"
                                                               || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "8"
                                                               || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "9"
                                                            )
                                                         )
                                                         select new { TrackerAppt };
                            foreach (var x in updateIsDeletedByStaus)
                            {
                                x.TrackerAppt.SetField("InsUptDlt", "3");
                            }

                            int intDelete = updateIsDeletedByStaus.Count();

                            //dtPatientToBeUpdated.AsEnumerable().Where(o => o.Field<object>("InsUptDlt").ToString() != "1" && (o.Field<object>("appointment_status_ehr_key").ToString() == "3" || o.Field<object>("appointment_status_ehr_key").ToString() == "5" || o.Field<object>("appointment_status_ehr_key").ToString() == "6" || o.Field<object>("appointment_status_ehr_key").ToString() == "7" || o.Field<object>("appointment_status_ehr_key").ToString() == "8" || o.Field<object>("appointment_status_ehr_key").ToString() == "9")).All(o => { o["InsUptDlt"] = "3"; return true; });
                            if (dtPatientToBeUpdated.Select("InsUptDlt = '2' or InsUptDlt = '3'").Length > 0)
                            {
                                dtSaveRecords.Load(dtPatientToBeUpdated.Select("InsUptDlt = '2' or InsUptDlt = '3'").CopyToDataTable().CreateDataReader());
                            }
                        }

                        bool status = true;
                        if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                        {
                            string Appt_EHR_Ids = string.Join("','", dtSaveRecords.AsEnumerable().Where(o => o.Field<object>("InsUptDlt").ToString() == "2").Select(p => p.Field<object>("Appt_EHR_Id").ToString()));

                            var updateLanguageQuery = from r1 in dtSaveRecords.Select("InsUptDlt In ('2','3')").AsEnumerable()
                                                      join r2 in dtLocalAppointment.AsEnumerable()
                                                      on r1["Appt_EHR_ID"].ToString().Trim() + "_" + r1["Clinic_Number"].ToString().Trim()
                                                      equals r2["Appt_EHR_ID"].ToString().Trim() + "_" + r2["Clinic_Number"].ToString().Trim()
                                                      select new { r1, r2 };
                            foreach (var x in updateLanguageQuery)
                            {
                                x.r1.SetField("Is_Appt", x.r2.Field<string>("Is_Appt"));
                            }

                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Appointment", "Appt_LocalDB_ID,Appt_Web_ID", "Appt_LocalDB_ID");
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
                    if (appointmenetSyncFailedCounter < 10)
                    {
                        appointmenetSyncFailedCounter++;
                    }
                    else
                    {
                        if (ex.Message.ToString().Contains("OutOfMemoryException"))
                        {
                            System.Environment.Exit(1);
                        }
                    }
                }
                finally
                {
                    try
                    {
                        //Utility.WriteToDebugSyncLogFile_All("----------------------------------------------------------------------------------------------------", "SynchDataTracker_Appointment_New");
                    }
                    catch
                    {
                    }
                    Is_synched_Appointment = true;
                }
            }
        }

        public void SynchDataTracker_Appointment()
        {

            if (IsTrackerProviderSync && IsTrackerOperatorySync && IsTrackerApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    if (IsTrackerProviderSync && IsTrackerOperatorySync && IsTrackerApptTypeSync)
                    {
                        //New line by yogesh for sync appointments patients first
                        SynchDataTracker_AppointmentsPatient();
                        if (IsPatientSyncedFirstTime)
                        {
                            SynchDataTracker_NewPatient();
                        }
                        SynchDataTracker_PatientStatus();
                        DataTable dtSoftDentAppointment = SynchTrackerBAL.GetTrackerAppointmentData();

                        dtSoftDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                        dtSoftDentAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                        dtSoftDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                        dtSoftDentAppointment.Columns.Add("ProcedureCode", typeof(string));

                        DataTable DtTrackerAppointment_Procedures_Data = SynchTrackerBAL.GetTrackerAppointment_Procedures_Data();
                        string ProcedureDesc = "";
                        string ProcedureCode = "";
                        ///////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                        foreach (DataRow dtDtxRow in dtSoftDentAppointment.Rows)
                        {
                            ProcedureDesc = "";
                            ProcedureCode = "";
                            DataRow[] dtCurApptProcedure = DtTrackerAppointment_Procedures_Data.Select("Appointmentid = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");
                            foreach (var dtSinProc in dtCurApptProcedure.ToList())
                            {
                                ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                            }
                            dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                            dtDtxRow["ProcedureCode"] = ProcedureCode;

                        }
                        /////////////////////

                        DataTable dtLocalAppointment = new DataTable();
                        DataTable dtTempResult = SynchLocalBAL.GetLocalAppointmentData("1");
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

                        dtLocalAppointment = CompareDataTableRecords(ref dtSoftDentAppointment, dtLocalAppointment, "Appt_EHR_ID", "Appt_LocalDB_ID", "Appt_LocalDB_ID,Appt_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Remind_DateTime,Patient_Status,Is_Appt_DoubleBook,InsuranceCompanyName,is_Status_Updated_From_Web,is_ehr_updated,Entry_DateTime,Is_Appt,Clinic_Number,Service_Install_Id");

                        dtSoftDentAppointment.AsEnumerable().Where(o => o.Field<object>("InsUptDlt").ToString() != "1" && (o.Field<object>("appointment_status_ehr_key").ToString() == "3" || o.Field<object>("appointment_status_ehr_key").ToString() == "5" || o.Field<object>("appointment_status_ehr_key").ToString() == "6" || o.Field<object>("appointment_status_ehr_key").ToString() == "7" || o.Field<object>("appointment_status_ehr_key").ToString() == "8" || o.Field<object>("appointment_status_ehr_key").ToString() == "9")).All(o => { o["InsUptDlt"] = "3"; return true; });

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
                                        // dtSaveRecords.Rows.Add(drrow);
                                    }
                                    if (drrow["confirmed_status_ehr_key"].ToString() == "" || drrow["confirmed_status_ehr_key"] == string.Empty)
                                    {
                                        drrow["confirmed_status_ehr_key"] = DBNull.Value;
                                    }
                                }
                                dtSaveRecords.Load(dtLocalAppointment.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Appointment", "Appt_LocalDB_ID,Appt_Web_ID", "Appt_LocalDB_ID");
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
                    if (appointmenetSyncFailedCounter < 10)
                    {
                        appointmenetSyncFailedCounter++;
                    }
                    else
                    {
                        if (ex.Message.ToString().Contains("OutOfMemoryException"))
                        {
                            System.Environment.Exit(1);
                        }
                    }
                }
                finally
                {
                    Is_synched_Appointment = true;
                }
            }
        }

        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataTracker_OperatoryEvent()
        {
            InitBgWorkerTracker_OperatoryEvent();
            InitBgTimerTracker_OperatoryEvent();
        }

        private void InitBgTimerTracker_OperatoryEvent()
        {
            timerSynchTracker_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchTracker_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchTracker_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_OperatoryEvent_Tick);
            timerSynchTracker_OperatoryEvent.Enabled = true;
            timerSynchTracker_OperatoryEvent.Start();
            timerSynchTracker_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerTracker_OperatoryEvent()
        {
            bwSynchTracker_OperatoryEvent = new BackgroundWorker();
            bwSynchTracker_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchTracker_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchTracker_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchTracker_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchTracker_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchTracker_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderTracker_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderTracker_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainTracker_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableTracker_OperatoryEvent);
            procThreadmainTracker_OperatoryEvent.Start();
        }

        public void CallSyncOrderTableTracker_OperatoryEvent()
        {
            if (bwSynchTracker_OperatoryEvent.IsBusy != true)
            {
                bwSynchTracker_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_OperatoryEvent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_OperatoryEvent.Enabled = true;
        }

        public void SynchDataTracker_OperatoryEvent()
        {
            if (IsTrackerOperatorySync && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtTrackerOperatoryEvent = SynchTrackerBAL.GetTrackerOperatoryEventData();

                    dtTrackerOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                    dtTrackerOperatoryEvent.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData("1");

                    DataTable dtTempResult = SynchLocalBAL.GetLocalOperatoryEventData("1");

                    if (dtTempResult.Select("is_deleted = 0").Count() > 0)
                    {
                        dtLocalOperatoryEvent = dtTempResult.Select("is_deleted = 0 AND StartTime >= '" + Utility.LastSyncDateAditServer.ToShortDateString() + "'").CopyToDataTable();
                    }

                    if (!dtLocalOperatoryEvent.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalOperatoryEvent.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalOperatoryEvent = CompareDataTableRecords(ref dtTrackerOperatoryEvent, dtLocalOperatoryEvent, "OE_EHR_ID", "OE_LocalDB_ID", "OE_LocalDB_ID,OE_EHR_ID,OE_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");

                    dtTrackerOperatoryEvent.AcceptChanges();
                    dtLocalOperatoryEvent.AcceptChanges();

                    bool status = false;
                    DataTable dtSaveRecords = dtTrackerOperatoryEvent.Clone();
                    if (dtTrackerOperatoryEvent.Select("InsUptDlt IN (1,2,3)").Count() > 0 || dtLocalOperatoryEvent.Select("InsUptDlt IN (3)").Count() > 0)
                    {
                        if (dtTrackerOperatoryEvent.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtTrackerOperatoryEvent.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                        }
                        if (dtLocalOperatoryEvent.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtLocalOperatoryEvent.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                        }
                        //dtSaveRecords.Load(dtTrackerOperatoryEvent.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                        status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryEvent", "OE_LocalDB_ID,OE_Web_ID", "OE_LocalDB_ID");
                    }
                    else
                    {
                        if (dtTrackerOperatoryEvent.Select("InsUptDlt IN (4)").Count() > 0)
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

        private void fncSynchDataTracker_Provider()
        {
            InitBgWorkerTracker_Provider();
            InitBgTimerTracker_Provider();
        }

        private void InitBgTimerTracker_Provider()
        {
            timerSynchTracker_Provider = new System.Timers.Timer();
            this.timerSynchTracker_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchTracker_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_Provider_Tick);
            timerSynchTracker_Provider.Enabled = true;
            timerSynchTracker_Provider.Start();
        }

        private void InitBgWorkerTracker_Provider()
        {
            bwSynchTracker_Provider = new BackgroundWorker();
            bwSynchTracker_Provider.WorkerReportsProgress = true;
            bwSynchTracker_Provider.WorkerSupportsCancellation = true;
            bwSynchTracker_Provider.DoWork += new DoWorkEventHandler(bwSynchTracker_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchTracker_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_Provider_RunWorkerCompleted);
        }

        private void timerSynchTracker_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_Provider.Enabled = false;
                MethodForCallSynchOrderTracker_Provider();
            }
        }

        public void MethodForCallSynchOrderTracker_Provider()
        {
            System.Threading.Thread procThreadmainTracker_Provider = new System.Threading.Thread(this.CallSyncOrderTableTracker_Provider);
            procThreadmainTracker_Provider.Start();
        }

        public void CallSyncOrderTableTracker_Provider()
        {
            if (bwSynchTracker_Provider.IsBusy != true)
            {
                bwSynchTracker_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                IsTrackerProviderCallFromApplicationStart = false;
                SynchDataTracker_Provider();
                CommonFunction.GetMasterSync();

                NoteDataMoveToCorrespond();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_Provider.Enabled = true;
        }

        public void SynchDataTracker_Provider()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentProvider = SynchTrackerBAL.GetTrackerProviderData();
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
                            IsTrackerProviderSync = true;
                            SynchDataLiveDB_Push_Provider();

                        }
                        else
                        {
                            IsTrackerProviderSync = false;
                        }
                    }
                    Is_synched_Provider = false;

                    IsProviderSyncedFirstTime = true;
                    if (!IsTrackerProviderCallFromApplicationStart)
                    {
                        SynchDataTracker_ProviderOfficeHours();
                        //CheckCustomhoursForProviderOperatory();
                        //SynchDataTracker_OperatoryCustomHours();
                    }
                    else {
                        ObjGoalBase.WriteToSyncLogFile("Providers Office Hours will sync in next interval");
                    }
                }
                catch (Exception ex)
                {
                    Is_synched_Provider = false;
                    IsProviderSyncedFirstTime = true;
                    ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database)]" + ex.Message);
                }
            }
        }

        public void SynchDataTracker_ProviderCustomHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && IsTrackerProviderSync && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataSet dsTrackerRecords = new DataSet();
                    dsTrackerRecords = SynchTrackerBAL.GetProviderCustomHours();
                    DataTable dtTrackerProviderHours = dsTrackerRecords.Tables[1];//SynchTrackerBAL.GetProviderCustomHours();
                    DataTable dtTrackerLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData("1");

                    dtTrackerProviderHours.Columns.Add("InsUptDlt", typeof(int));
                    dtTrackerProviderHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtTrackerLocalProviderHours.Columns.Contains("InsUptDlt"))
                    {
                        dtTrackerLocalProviderHours.Columns.Add("InsUptDlt", typeof(int));
                        dtTrackerLocalProviderHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtTrackerLocalProviderHours = CompareDataTablehourTracker(ref dtTrackerProviderHours, dtTrackerLocalProviderHours, "PH_EHR_ID", "PH_LocalDB_ID", "PH_LocalDB_ID,PH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");

                    dtTrackerProviderHours.AcceptChanges();
                    dtTrackerLocalProviderHours.AcceptChanges();

                    if ((dtTrackerProviderHours != null && dtTrackerProviderHours.Rows.Count > 0) || (dtTrackerLocalProviderHours != null && dtTrackerLocalProviderHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtTrackerProviderHours.Clone();
                        if (dtTrackerProviderHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtTrackerLocalProviderHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtTrackerProviderHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerProviderHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtTrackerLocalProviderHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerLocalProviderHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderHours", "PH_LocalDB_ID,PH_Web_ID", "PH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtTrackerProviderHours.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
                            ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_ProviderHours();
                            SynchDataTracker_OperatoryCustomHours(dsTrackerRecords.Tables[0]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        private void SyncPullLogsAndSaveinTracker()
        {
            try
            {

                CheckCustomhoursForProviderOperatory();
                SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                SynchDataLiveDB_Pull_PatientFollowUp();
                SynchDataLiveDB_PatientSMSCall_LocalToTracker();
                fncPaymentSMSCallStatusUpdate();
                SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
            }
            catch (Exception)
            {

            }
        }


        public void SynchDataTracker_ProviderOfficeHours()
        {
            try
            {   // Need to open below comment
                SyncPullLogsAndSaveinTracker();
                //CheckCustomhoursForProviderOperatory();
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtTrackerProviderOfficeHours = SynchTrackerBAL.GetTrackerProviderOfficeHours();
                    DataTable dtTrackerLocalProviderOfficeHours = SynchLocalBAL.GetLocalProviderOfficeHours("1");
                    // DataTable dtTrackerProvider = SynchTrackerBAL.GetTrackerProviderData();

                    dtTrackerProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtTrackerProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtTrackerLocalProviderOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtTrackerLocalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtTrackerLocalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtTrackerLocalProviderOfficeHours = CompareDataTableRecords(ref dtTrackerProviderOfficeHours, dtTrackerLocalProviderOfficeHours, "POH_EHR_ID", "POH_LocalDB_ID", "StartTime2,EndTime2,StartTime3333,endTime3,POH_LocalDB_ID,POH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtTrackerProviderOfficeHours.AcceptChanges();
                    dtTrackerLocalProviderOfficeHours.AcceptChanges();

                    if ((dtTrackerProviderOfficeHours != null && dtTrackerProviderOfficeHours.Rows.Count > 0) || (dtTrackerLocalProviderOfficeHours != null && dtTrackerLocalProviderOfficeHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtTrackerProviderOfficeHours.Clone();
                        if (dtTrackerProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtTrackerLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtTrackerProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerProviderOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtTrackerLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerLocalProviderOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderOfficeHours", "POH_LocalDB_ID,POH_Web_ID", "POH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtTrackerProviderOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
                    }
                    SynchDataTracker_ProviderCustomHours();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }
        #endregion

        #region Synch Speciality

        private void fncSynchDataTracker_Speciality()
        {
            InitBgWorkerTracker_Speciality();
            InitBgTimerTracker_Speciality();
        }

        private void InitBgTimerTracker_Speciality()
        {
            timerSynchTracker_Speciality = new System.Timers.Timer();
            this.timerSynchTracker_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchTracker_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_Speciality_Tick);
            timerSynchTracker_Speciality.Enabled = true;
            timerSynchTracker_Speciality.Start();
        }

        private void InitBgWorkerTracker_Speciality()
        {
            bwSynchTracker_Speciality = new BackgroundWorker();
            bwSynchTracker_Speciality.WorkerReportsProgress = true;
            bwSynchTracker_Speciality.WorkerSupportsCancellation = true;
            bwSynchTracker_Speciality.DoWork += new DoWorkEventHandler(bwSynchTracker_Speciality_DoWork);
            bwSynchTracker_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_Speciality_RunWorkerCompleted);
        }

        private void timerSynchTracker_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_Speciality.Enabled = false;
                MethodForCallSynchOrderTracker_Speciality();
            }
        }

        public void MethodForCallSynchOrderTracker_Speciality()
        {
            System.Threading.Thread procThreadmainTracker_Speciality = new System.Threading.Thread(this.CallSyncOrderTableTracker_Speciality);
            procThreadmainTracker_Speciality.Start();
        }

        public void CallSyncOrderTableTracker_Speciality()
        {
            if (bwSynchTracker_Speciality.IsBusy != true)
            {
                bwSynchTracker_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_Speciality();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_Speciality.Enabled = true;
        }

        public void SynchDataTracker_Speciality()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentSpeciality = SynchTrackerBAL.GetTrackerSpecialityData();

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

        private void fncSynchDataTracker_Operatory()
        {
            InitBgWorkerTracker_Operatory();
            InitBgTimerTracker_Operatory();
        }

        private void InitBgTimerTracker_Operatory()
        {
            timerSynchTracker_Operatory = new System.Timers.Timer();
            this.timerSynchTracker_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchTracker_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_Operatory_Tick);
            timerSynchTracker_Operatory.Enabled = true;
            timerSynchTracker_Operatory.Start();
        }

        private void InitBgWorkerTracker_Operatory()
        {
            bwSynchTracker_Operatory = new BackgroundWorker();
            bwSynchTracker_Operatory.WorkerReportsProgress = true;
            bwSynchTracker_Operatory.WorkerSupportsCancellation = true;
            bwSynchTracker_Operatory.DoWork += new DoWorkEventHandler(bwSynchTracker_Operatory_DoWork);
            bwSynchTracker_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_Operatory_RunWorkerCompleted);
        }

        private void timerSynchTracker_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_Operatory.Enabled = false;
                MethodForCallSynchOrderTracker_Operatory();
            }
        }

        public void MethodForCallSynchOrderTracker_Operatory()
        {
            System.Threading.Thread procThreadmainTracker_Operatory = new System.Threading.Thread(this.CallSyncOrderTableTracker_Operatory);
            procThreadmainTracker_Operatory.Start();
        }

        public void CallSyncOrderTableTracker_Operatory()
        {
            if (bwSynchTracker_Operatory.IsBusy != true)
            {
                bwSynchTracker_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_Operatory.Enabled = true;
        }

        public void SynchDataTracker_Operatory()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {

                try
                {
                    DataTable dtTrackerOperatory = SynchTrackerBAL.GetTrackerOperatoryData();

                    dtTrackerOperatory.Columns.Add("InsUptDlt", typeof(int));
                    dtTrackerOperatory.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData("1");

                    if (!dtLocalOperatory.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalOperatory.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalOperatory.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalOperatory = CompareDataTableRecords(ref dtTrackerOperatory, dtLocalOperatory, "Operatory_Name", "Operatory_LocalDB_ID", "Operatory_LocalDB_ID,Operatory_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,,Clinic_Number,Service_Install_Id");

                    if (dtLocalOperatory.Select("InsUptDlt=3").Count() > 0)
                    {
                        dtTrackerOperatory.Load(dtLocalOperatory.Select("InsUptDlt=3").CopyToDataTable().CreateDataReader());
                    }

                    dtTrackerOperatory.AcceptChanges();

                    if (dtTrackerOperatory != null && dtTrackerOperatory.Rows.Count > 0 && dtTrackerOperatory.AsEnumerable()
                            .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtTrackerOperatory.Clone();
                        if (dtTrackerOperatory.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtTrackerOperatory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Operatory", "Operatory_LocalDB_ID,Operatory_Web_ID", "Operatory_LocalDB_ID");
                        }
                        else
                        {
                            if (dtTrackerOperatory.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                            ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsTrackerOperatorySync = true;
                            SynchDataLiveDB_Push_Operatory();
                        }
                        else
                        {
                            IsTrackerOperatorySync = false;
                        }
                    }
                    SynchDataTracker_OperatoryOfficeHours();

                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        public void SynchDataTracker_OperatoryCustomHours(DataTable dtTrackerOperatoryHours = null)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    // DataTable dtTrackerOperatoryHours = SynchTrackerBAL.GetOperatoryCustomHours();
                    // Below is the code for direct operatory hours sync
                    //DataSet dsTrackerRecords = new DataSet();
                    //dsTrackerRecords = SynchTrackerBAL.GetProviderCustomHours();
                    //dtTrackerOperatoryHours = dsTrackerRecords.Tables[0];
                    DataTable dtTrackerLocalOperatoryHours = SynchLocalBAL.GetLocalOperatoryHoursData("1");

                    dtTrackerOperatoryHours.Columns.Add("InsUptDlt", typeof(int));
                    dtTrackerOperatoryHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtTrackerLocalOperatoryHours.Columns.Contains("InsUptDlt"))
                    {
                        dtTrackerLocalOperatoryHours.Columns.Add("InsUptDlt", typeof(int));
                        dtTrackerLocalOperatoryHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtTrackerLocalOperatoryHours = CompareDataTablehourTracker(ref dtTrackerOperatoryHours, dtTrackerLocalOperatoryHours, "OH_EHR_ID", "OH_LocalDB_ID", "OH_LocalDB_ID,OH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");

                    dtTrackerOperatoryHours.AcceptChanges();
                    dtTrackerLocalOperatoryHours.AcceptChanges();

                    if ((dtTrackerOperatoryHours != null && dtTrackerOperatoryHours.Rows.Count > 0) || (dtTrackerLocalOperatoryHours != null && dtTrackerLocalOperatoryHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtTrackerOperatoryHours.Clone();
                        if (dtTrackerOperatoryHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtTrackerLocalOperatoryHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtTrackerOperatoryHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerOperatoryHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtTrackerLocalOperatoryHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerLocalOperatoryHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryHours", "OH_LocalDB_ID,PH_Web_ID", "OH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtTrackerOperatoryHours.Select("InsUptDlt IN (4)").Count() > 0)
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
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[OperatoryHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        private DataTable CompareDataTablehourTracker(ref DataTable dtSource, DataTable dtDestination, string compareColumnName, string primarykeyColumns, string ignoreColumns)
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

        public void SynchDataTracker_OperatoryOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtTrackerOperatoryOfficeHours = SynchTrackerBAL.GetTrackerOperatoryOfficeHours();
                    DataTable dtTrackerLocalOperatoryOfficeHours = SynchLocalBAL.GetLocalOperatoryOfficeHoursData("1");
                    DataTable dtTrackerOperatory = SynchTrackerBAL.GetTrackerOperatoryData();

                    dtTrackerOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtTrackerOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtTrackerLocalOperatoryOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtTrackerLocalOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtTrackerLocalOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtTrackerLocalOperatoryOfficeHours = CompareDataTableRecords(ref dtTrackerOperatoryOfficeHours, dtTrackerLocalOperatoryOfficeHours, "OOH_EHR_ID", "OOH_LocalDB_ID", "OOH_LocalDB_ID,OOH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtTrackerOperatoryOfficeHours.AcceptChanges();

                    if (dtTrackerOperatoryOfficeHours != null && dtTrackerOperatoryOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtTrackerOperatoryOfficeHours.Clone();
                        if (dtTrackerOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtTrackerLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtTrackerOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtTrackerLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryOfficeHours", "OOH_LocalDB_ID,OOH_Web_ID", "OOH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtTrackerOperatoryOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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


                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[OperatoryOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        #endregion

        #region Synch Appointment Type

        private void fncSynchDataTracker_ApptType()
        {
            InitBgWorkerTracker_ApptType();
            InitBgTimerTracker_ApptType();
        }

        private void InitBgTimerTracker_ApptType()
        {
            timerSynchTracker_ApptType = new System.Timers.Timer();
            this.timerSynchTracker_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchTracker_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_ApptType_Tick);
            timerSynchTracker_ApptType.Enabled = true;
            timerSynchTracker_ApptType.Start();
        }

        private void InitBgWorkerTracker_ApptType()
        {
            bwSynchTracker_ApptType = new BackgroundWorker();
            bwSynchTracker_ApptType.WorkerReportsProgress = true;
            bwSynchTracker_ApptType.WorkerSupportsCancellation = true;
            bwSynchTracker_ApptType.DoWork += new DoWorkEventHandler(bwSynchTracker_ApptType_DoWork);
            bwSynchTracker_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_ApptType_RunWorkerCompleted);
        }

        private void timerSynchTracker_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_ApptType.Enabled = false;
                MethodForCallSynchOrderTracker_ApptType();
            }
        }

        public void MethodForCallSynchOrderTracker_ApptType()
        {
            System.Threading.Thread procThreadmainTracker_ApptType = new System.Threading.Thread(this.CallSyncOrderTableTracker_ApptType);
            procThreadmainTracker_ApptType.Start();
        }

        public void CallSyncOrderTableTracker_ApptType()
        {
            if (bwSynchTracker_ApptType.IsBusy != true)
            {
                bwSynchTracker_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_ApptType.Enabled = true;
        }

        public void SynchDataTracker_ApptType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentApptStatus = SynchTrackerBAL.GetTrackerApptTypeData();

                    dtSoftDentApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentApptStatus.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalApptTypeData("1");

                    if (!dtLocalApptStatus.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalApptStatus.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalApptStatus.Columns["InsUptDlt"].DefaultValue = 0;
                    }
                    bool status = false;
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
                            IsTrackerApptTypeSync = true;
                            SynchDataLiveDB_Push_ApptType();
                        }
                        else
                        {
                            IsTrackerApptTypeSync = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ApptType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Patient

        private void fncSynchDataTracker_Patient()
        {
            InitBgWorkerTracker_Patient();
            InitBgTimerTracker_Patient();
        }

        private void InitBgTimerTracker_Patient()
        {
            timerSynchTracker_Patient = new System.Timers.Timer();
            this.timerSynchTracker_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchTracker_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_Patient_Tick);
            timerSynchTracker_Patient.Enabled = true;
            timerSynchTracker_Patient.Start();
            timerSynchTracker_Patient_Tick(null, null);
        }

        private void InitBgWorkerTracker_Patient()
        {
            bwSynchTracker_Patient = new BackgroundWorker();
            bwSynchTracker_Patient.WorkerReportsProgress = true;
            bwSynchTracker_Patient.WorkerSupportsCancellation = true;
            bwSynchTracker_Patient.DoWork += new DoWorkEventHandler(bwSynchTracker_Patient_DoWork);
            bwSynchTracker_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_Patient_RunWorkerCompleted);
        }

        private void timerSynchTracker_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_Patient.Enabled = false;
                MethodForCallSynchOrderTracker_Patient();
            }
        }

        public void MethodForCallSynchOrderTracker_Patient()
        {
            System.Threading.Thread procThreadmainTracker_Patient = new System.Threading.Thread(this.CallSyncOrderTableTracker_Patient);
            procThreadmainTracker_Patient.Start();
        }

        public void CallSyncOrderTableTracker_Patient()
        {
            if (bwSynchTracker_Patient.IsBusy != true)
            {
                bwSynchTracker_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_Patient();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_Patient.Enabled = true;
        }

        public void SynchDataTracker_Patient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    IsParientFirstSync = false;
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    SynchDataLiveDB_Pull_EHR_Patientoptout();
                    DataTable dtTrackerPatientList = SynchTrackerBAL.GetTrackerPatientData();

                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");

                    //dtTrackerPatientList.Columns.Add("InsUptDlt", typeof(int));
                    //dtTrackerPatientList.Columns["InsUptDlt"].DefaultValue = 0;


                    //if (!dtLocalPatient.Columns.Contains("InsUptDlt"))
                    //{
                    //    dtLocalPatient.Columns.Add("InsUptDlt", typeof(int));
                    //    dtLocalPatient.Columns["InsUptDlt"].DefaultValue = 0;
                    //}

                    string patientTableName = "Patient";
                    if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                    {
                        patientTableName = "PatientCompare";
                    }
                    if (dtTrackerPatientList != null && dtTrackerPatientList.Rows.Count > 0)
                    {
                        bool isPatientSave = SynchTrackerBAL.Save_Tracker_To_Local(dtTrackerPatientList, patientTableName, dtLocalPatient, true);
                        if (isPatientSave)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsGetParientRecordDone = true;

                            SynchDataLiveDB_Push_Patient();
                        }
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                        bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                        IsGetParientRecordDone = true;
                    }


                    ////dtLocalProvider = CompareDataTableRecords(ref dtSoftDentProvider, dtLocalProvider, "Provider_EHR_ID", "Provider_LocalDB_ID", "Provider_LocalDB_ID,Provider_EHR_ID,Provider_Web_ID,image,Is_Adit_Updated,Last_Sync_Date,InsUptDlt");

                    //dtLocalPatient = CompareDataTableRecords(ref dtSoftDentPatientList, dtLocalPatient, "Patient_EHR_ID", "Patient_LocalDB_ID", "Patient_LocalDB_ID,Patient_EHR_ID,Patient_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,ReceiveSms,ReceiveEmail");

                    ////DataTable dtSoftDentPatientdue_date = SynchSoftDentBAL.GetSoftDentPatientdue_date();

                    ////TotalPatientRecord = dtSoftDentPatient.Rows.Count;
                    ////GetPatientRecord = 0;

                    //dtSoftDentPatientList.AcceptChanges();


                    //bool status = false;
                    //DataTable dtSaveRecords = dtSoftDentPatientList.Clone();
                    //if (dtSoftDentPatientList.Select("InsUptDlt IN (1,2)").Count() > 0)
                    //{
                    //    dtSaveRecords.Load(dtSoftDentPatientList.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
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

                    IsPatientSyncedFirstTime = true;
                    Is_synched_Patient = false;
                    SynchDataTracker_PatientStatus();
                    SynchDataTracker_PatientMedication("");
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    Is_synched_Patient = false;
                    if (appointmenetSyncFailedCounter < 10)
                    {
                        appointmenetSyncFailedCounter++;
                    }
                    else
                    {
                        if (ex.Message.ToString().Contains("OutOfMemoryException"))
                        {
                            System.Environment.Exit(1);
                        }
                    }
                }
            }
            else if (Is_synched_AppointmentsPatient)
            {
                Is_Synched_PatientCallHit = true;
            }
        }
        public void SynchDataTracker_PatientStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtTrackerPatientStatusList = SynchTrackerBAL.GetTrackerPatientStatusData();
                    if (dtTrackerPatientStatusList != null && dtTrackerPatientStatusList.Rows.Count > 0)
                    {
                        SynchLocalBAL.UpdatePatient_Status(dtTrackerPatientStatusList, "1");
                    }
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientStatus");
                    ObjGoalBase.WriteToSyncLogFile("PatientStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    SynchDataLiveDB_Push_PatientStatus();
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }

        public void SynchDataTracker_NewPatient()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtTrackerPatientList = SynchTrackerBAL.GetTrackerPatientIDsData();
                    if (dtTrackerPatientList != null && dtTrackerPatientList.Rows.Count > 0)
                    {
                        DataTable dtLocalPatientList = SynchLocalBAL.GetLocalNewPatientData("1");
                        if ((dtLocalPatientList != null && dtLocalPatientList.Rows.Count > 0 && dtLocalPatientList.Select("Is_Adit_Updated = 1").Length > 0))
                        {
                            DataTable dtSaveRecords = new DataTable();
                            var itemsToBeAdded = (from TrackerPatient in dtTrackerPatientList.AsEnumerable()
                                                  join LocalPatient in dtLocalPatientList.AsEnumerable()
                                                  on new { PatID = TrackerPatient["Patient_EHR_ID"].ToString().Trim() }
                                                  equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim() }
                                                  into matchingRows
                                                  from matchingRow in matchingRows.DefaultIfEmpty()
                                                  where matchingRow == null
                                                  select TrackerPatient).ToList();
                            if (itemsToBeAdded.Count > 0)
                            {
                                string strPatID = String.Join(",", itemsToBeAdded.AsEnumerable().Select(x => x["Patient_EHR_ID"].ToString()).ToArray());
                                strPatID = strPatID.Replace(",", ",");
                                if (!string.IsNullOrEmpty(strPatID))
                                {
                                    dtSaveRecords = SynchTrackerBAL.GetTrackerPatientDatawithPatientId(strPatID);
                                }
                            }
                            if (!dtSaveRecords.Columns.Contains("InsUptDlt"))
                            {
                                dtSaveRecords.Columns.Add("InsUptDlt", typeof(int));
                                dtSaveRecords.Columns["InsUptDlt"].DefaultValue = 0;
                            }
                            if (dtSaveRecords.Rows.Count > 0)
                            {
                                bool status = SynchTrackerBAL.Save_Patient_Tracker_To_Local_New(dtSaveRecords, "0", "1");

                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("NewPatient");
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
        public void SynchDataTracker_AppointmentsPatient_New()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtTrackerPatientList = SynchTrackerBAL.GetTrackerAppointmentsPatientData();

                    string patientTableName = "Patient";
                    string PatientEHRIDs = string.Join("','", dtTrackerPatientList.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                    if (PatientEHRIDs != string.Empty)
                    {
                        Is_synched_AppointmentsPatient = true;

                        PatientEHRIDs = "'" + PatientEHRIDs + "'";
                        //DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, "1");
                        DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalPatientData("1");
                        DataTable dtLocalPatient = new DataTable();
                        dtLocalPatient = dtLocalPatientResult.Clone();
                        var LocalPatientByPatEHRID = (from LocalPatient in dtLocalPatientResult.AsEnumerable()
                                                      join OpenDentalPatient in dtTrackerPatientList.AsEnumerable()
                                                      on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                      equals OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                      where LocalPatient["Patient_EHR_ID"].ToString().Trim() == OpenDentalPatient["Patient_EHR_ID"].ToString().Trim()
                                                      select LocalPatient).ToList();
                        if (LocalPatientByPatEHRID.Count > 0)
                        {
                            dtLocalPatient = LocalPatientByPatEHRID.CopyToDataTable<DataRow>();
                        }
                        DataTable dtSaveRecords = new DataTable();
                        dtSaveRecords = dtTrackerPatientList.Clone();

                        var itemsToBeAdded = (from OpenDentalPatient in dtTrackerPatientList.AsEnumerable()
                                              join LocalPatient in dtLocalPatient.AsEnumerable()
                                              on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                              equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                              into matchingRows
                                              from matchingRow in matchingRows.DefaultIfEmpty()
                                              where matchingRow == null
                                              select OpenDentalPatient).ToList();
                        DataTable dtPatientToBeAdded = dtLocalPatient.Clone();
                        if (itemsToBeAdded.Count > 0)
                        {
                            dtPatientToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
                        }

                        if (!dtPatientToBeAdded.Columns.Contains("InsUptDlt"))
                        {
                            dtPatientToBeAdded.Columns.Add("InsUptDlt", typeof(int));
                            dtPatientToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
                        }
                        if (dtPatientToBeAdded.Rows.Count > 0)
                        {
                            dtPatientToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
                            dtSaveRecords.Load(dtPatientToBeAdded.Select().CopyToDataTable().CreateDataReader());
                        }

                        var itemsToBeUpdated = (from OpenDentalPatient in dtTrackerPatientList.AsEnumerable()
                                                join LocalPatient in dtLocalPatient.AsEnumerable()
                                                on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                where
                                                 (OpenDentalPatient["nextvisit_date"] != DBNull.Value && OpenDentalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(OpenDentalPatient["nextvisit_date"]) : DateTime.Now)
                                                 !=
                                                 (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)

                                                 ||

                                                 (OpenDentalPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())

                                                 ||

                                                 (OpenDentalPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())

                                                 || (OpenDentalPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                                 || (OpenDentalPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                                 || (Utility.ConvertContactNumber(OpenDentalPatient["Home_Phone"].ToString().Trim())) != (Utility.ConvertContactNumber(LocalPatient["Home_Phone"].ToString().Trim()))
                                                 || (OpenDentalPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                                 || (OpenDentalPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                                 || (OpenDentalPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                                 || (Utility.ConvertContactNumber(OpenDentalPatient["Mobile"].ToString().Trim())) != (Utility.ConvertContactNumber(LocalPatient["Mobile"].ToString().Trim()))
                                                 || (OpenDentalPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
                                                //First_name, Last_name, Home_Phone, Middle_Name, Status, Email, Mobile, ReceiveSMS, PreferredLanguage
                                                select OpenDentalPatient).ToList();

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

                        if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            bool isPatientSave = SynchTrackerBAL.Save_Tracker_Patient_To_Local_New(dtSaveRecords);
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
                        if (Is_Synched_PatientCallHit)
                        {
                            //Is_Synched_PatientCallHit = false;
                            //SynchDataTracker_Patient();
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
        public void SynchDataTracker_AppointmentsPatient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtTrackerPatientList = SynchTrackerBAL.GetTrackerAppointmentsPatientData();

                    string patientTableName = "Patient";

                    string PatientEHRIDs = string.Join("','", dtTrackerPatientList.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                    if (PatientEHRIDs != string.Empty)
                    {
                        Is_synched_AppointmentsPatient = true;

                        PatientEHRIDs = "'" + PatientEHRIDs + "'";

                        DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, "1");

                        if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                        {
                            patientTableName = "PatientCompare";
                        }
                        if (dtTrackerPatientList != null && dtTrackerPatientList.Rows.Count > 0)
                        {
                            bool isPatientSave = SynchTrackerBAL.Save_Tracker_To_Local(dtTrackerPatientList, patientTableName, dtLocalPatient);
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
                        if (Is_Synched_PatientCallHit)
                        {
                            //Is_Synched_PatientCallHit = false;
                            //SynchDataTracker_Patient();
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

        #endregion

        #region Synch User
        private void fncSynchDataTracker_User()
        {
            InitBgWorkerTracker_User();
            InitBgTimerTracker_User();
        }

        private void InitBgTimerTracker_User()
        {
            timerSynchTracker_User = new System.Timers.Timer();
            this.timerSynchTracker_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchTracker_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_User_Tick);
            timerSynchTracker_User.Enabled = true;
            timerSynchTracker_User.Start();
        }

        private void timerSynchTracker_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_User.Enabled = false;
                MethodForCallSynchOrderTracker_User();
            }
        }

        private void MethodForCallSynchOrderTracker_User()
        {
            System.Threading.Thread procThreadmainTracker_User = new System.Threading.Thread(this.CallSyncOrderTableTracker_User);
            procThreadmainTracker_User.Start();
        }

        private void CallSyncOrderTableTracker_User()
        {
            if (bwSynchTracker_User.IsBusy != true)
            {
                bwSynchTracker_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void InitBgWorkerTracker_User()
        {
            bwSynchTracker_User = new BackgroundWorker();
            bwSynchTracker_User.WorkerReportsProgress = true;
            bwSynchTracker_User.WorkerSupportsCancellation = true;
            bwSynchTracker_User.DoWork += new DoWorkEventHandler(bwSynchTracker_User_DoWork);
            bwSynchTracker_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_User_RunWorkerCompleted);
        }

        private void bwSynchTracker_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_User.Enabled = true;
        }

        private void bwSynchTracker_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SynchDataTracker_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtTrackerUser = SynchTrackerBAL.GetTrackerUser();
                    dtTrackerUser.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalUser = SynchLocalBAL.GetLocalUser("1");

                    foreach (DataRow dtDtxRow in dtTrackerUser.Rows)
                    {
                        DataRow[] row = dtLocalUser.Copy().Select("User_EHR_ID = '" + dtDtxRow["User_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Is_Active"].ToString().ToLower().Trim() != row[0]["Is_Active"].ToString().ToLower().Trim())
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
                    foreach (DataRow dtDtxRow in dtLocalUser.Rows)
                    {
                        DataRow[] row = dtTrackerUser.Copy().Select("User_EHR_ID = '" + dtDtxRow["User_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtTrackerUser.NewRow();
                            BlcOptDtldr["User_EHR_ID"] = dtDtxRow["User_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtTrackerUser.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtTrackerUser.AcceptChanges();

                    if (dtTrackerUser != null && dtTrackerUser.Rows.Count > 0)
                    {
                        //bool status = SynchTrackerBAL.Save_Tracker_To_Local(dtTrackerUser, "Users", "User_LocalDB_ID,User_Web_ID", "User_EHR_ID");
                        bool status = SynchTrackerBAL.Save_Users_Tracker_To_Local(dtTrackerUser);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Users");
                            ObjGoalBase.WriteToSyncLogFile("User Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            SynchDataLiveDB_Push_User();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[User Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }
        #endregion

        #region Synch RecallType

        private void fncSynchDataTracker_RecallType()
        {
            InitBgWorkerTracker_RecallType();
            InitBgTimerTracker_RecallType();
        }

        private void InitBgTimerTracker_RecallType()
        {
            timerSynchTracker_RecallType = new System.Timers.Timer();
            this.timerSynchTracker_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchTracker_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_RecallType_Tick);
            timerSynchTracker_RecallType.Enabled = true;
            timerSynchTracker_RecallType.Start();
        }

        private void InitBgWorkerTracker_RecallType()
        {
            bwSynchTracker_RecallType = new BackgroundWorker();
            bwSynchTracker_RecallType.WorkerReportsProgress = true;
            bwSynchTracker_RecallType.WorkerSupportsCancellation = true;
            bwSynchTracker_RecallType.DoWork += new DoWorkEventHandler(bwSynchTracker_RecallType_DoWork);
            bwSynchTracker_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_RecallType_RunWorkerCompleted);
        }

        private void timerSynchTracker_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_RecallType.Enabled = false;
                MethodForCallSynchOrderTracker_RecallType();
            }
        }

        public void MethodForCallSynchOrderTracker_RecallType()
        {
            System.Threading.Thread procThreadmainTracker_RecallType = new System.Threading.Thread(this.CallSyncOrderTableTracker_RecallType);
            procThreadmainTracker_RecallType.Start();
        }

        public void CallSyncOrderTableTracker_RecallType()
        {
            if (bwSynchTracker_RecallType.IsBusy != true)
            {
                bwSynchTracker_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_RecallType.Enabled = true;
        }

        public void SynchDataTracker_RecallType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtTrackerRecallType = SynchTrackerBAL.GetTrackerRecallTypeData();
                    dtTrackerRecallType.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData("1");

                    foreach (DataRow dtDtxRow in dtTrackerRecallType.Rows)
                    {
                        DataRow[] row = dtLocalRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["RecallType_Name"].ToString().ToLower().Trim() != row[0]["RecallType_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if ((dtDtxRow["is_deleted"].ToString().Trim()) != row[0]["is_deleted"].ToString().Trim())
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
                        DataRow[] row = dtTrackerRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtTrackerRecallType.NewRow();
                            BlcOptDtldr["RecallType_EHR_ID"] = dtDtxRow["RecallType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtTrackerRecallType.Rows.Add(BlcOptDtldr);
                        }
                    }

                    dtTrackerRecallType.AcceptChanges();

                    if (dtTrackerRecallType != null && dtTrackerRecallType.Rows.Count > 0)
                    {
                        bool status = SynchTrackerBAL.Save_Tracker_To_Local(dtTrackerRecallType, "RecallType", "RecallType_LocalDB_ID,RecallType_Web_ID", "RecallType_EHR_ID");
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

        private void fncSynchDataTracker_ApptStatus()
        {
            InitBgWorkerTracker_ApptStatus();
            InitBgTimerTracker_ApptStatus();
        }

        private void InitBgTimerTracker_ApptStatus()
        {
            timerSynchTracker_ApptStatus = new System.Timers.Timer();
            this.timerSynchTracker_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchTracker_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_ApptStatus_Tick);
            timerSynchTracker_ApptStatus.Enabled = true;
            timerSynchTracker_ApptStatus.Start();
        }

        private void InitBgWorkerTracker_ApptStatus()
        {
            bwSynchTracker_ApptStatus = new BackgroundWorker();
            bwSynchTracker_ApptStatus.WorkerReportsProgress = true;
            bwSynchTracker_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchTracker_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchTracker_ApptStatus_DoWork);
            bwSynchTracker_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchTracker_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_ApptStatus.Enabled = false;
                MethodForCallSynchOrderTracker_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderTracker_ApptStatus()
        {
            System.Threading.Thread procThreadmainTracker_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableTracker_ApptStatus);
            procThreadmainTracker_ApptStatus.Start();
        }

        public void CallSyncOrderTableTracker_ApptStatus()
        {
            if (bwSynchTracker_ApptStatus.IsBusy != true)
            {
                bwSynchTracker_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_ApptStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_ApptStatus.Enabled = true;
        }

        public void SynchDataTracker_ApptStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentApptStatus = SynchTrackerBAL.GetTrackerApptStatusData();

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
                        IsTrackerApptStatusSync = true;
                        SynchDataLiveDB_Push_ApptStatus();
                    }
                    else
                    {
                        IsTrackerApptStatusSync = false;
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

        private void fncSynchDataTracker_Holiday()
        {
            InitBgWorkerTracker_Holiday();
            InitBgTimerTracker_Holiday();
        }

        private void InitBgTimerTracker_Holiday()
        {
            timerSynchTracker_Holiday = new System.Timers.Timer();
            this.timerSynchTracker_Holiday.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchTracker_Holiday.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_Holiday_Tick);
            timerSynchTracker_Holiday.Enabled = true;
            timerSynchTracker_Holiday.Start();
        }

        private void InitBgWorkerTracker_Holiday()
        {
            bwSynchTracker_Holiday = new BackgroundWorker();
            bwSynchTracker_Holiday.WorkerReportsProgress = true;
            bwSynchTracker_Holiday.WorkerSupportsCancellation = true;
            bwSynchTracker_Holiday.DoWork += new DoWorkEventHandler(bwSynchTracker_Holiday_DoWork);
            bwSynchTracker_Holiday.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_Holiday_RunWorkerCompleted);
        }

        private void timerSynchTracker_Holiday_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_Holiday.Enabled = false;
                MethodForCallSynchOrderTracker_Holiday();
            }
        }

        public void MethodForCallSynchOrderTracker_Holiday()
        {
            System.Threading.Thread procThreadmainTracker_Holiday = new System.Threading.Thread(this.CallSyncOrderTableTracker_Holiday);
            procThreadmainTracker_Holiday.Start();
        }

        public void CallSyncOrderTableTracker_Holiday()
        {
            if (bwSynchTracker_Holiday.IsBusy != true)
            {
                bwSynchTracker_Holiday.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_Holiday_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_Holiday.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_Holiday();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_Holiday_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_Holiday.Enabled = true;
        }

        public void SynchDataTracker_Holiday()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtTrackerHoliday = SynchTrackerBAL.GetTrackerHolidayData();

                    dtTrackerHoliday.Columns.Add("InsUptDlt", typeof(int));
                    dtTrackerHoliday.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalHoliday = SynchLocalBAL.GetLocalHolidayData("1");
                    //DataTable dtLocalHoliday = SynchLocalBAL.GetLocalDefaultHolidayData("1");
                    dtTrackerHoliday = CommonUtility.AddHolidays(dtTrackerHoliday, dtLocalHoliday, "SchedDate", "Comment", "H_EHR_ID");
                    //rooja 5-5-23

                    if (!dtLocalHoliday.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalHoliday.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalHoliday.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    foreach (DataRow dtDtxRow in dtTrackerHoliday.Rows)
                    {
                        DataRow[] row = dtLocalHoliday.Select("SchedDate = '" + Convert.ToDateTime(dtDtxRow["SchedDate"].ToString()) + "'");

                        if (row.Length > 0)
                        {
                            if (Convert.ToString(dtDtxRow["comment"].ToString().Trim().ToUpper()) != Convert.ToString(row[0]["comment"]).ToUpper())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                                dtDtxRow["H_LocalDB_ID"] = row[0]["H_LocalDB_ID"].ToString();
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 0;
                            }
                        }
                        else
                        {                            
                            dtDtxRow["InsUptDlt"] = 1;
                            dtDtxRow["Clinic_Number"] = 0;
                            dtDtxRow["Service_Install_Id"] = 1;
                        }
                        try
                        {
                            if (dtTrackerHoliday.Columns.Contains("H_Operatory_EHR_ID") && dtTrackerHoliday.Columns["H_Operatory_EHR_ID"] != null && string.IsNullOrEmpty(dtDtxRow["H_Operatory_EHR_ID"].ToString()))
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
                        DataRow[] row = dtTrackerHoliday.Copy().Select("SchedDate = '" + dtDtxRow["SchedDate"] + "'");
                        if (row.Length <= 0)
                        {
                            dtDtxRow["InsUptDlt"] = 3;
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

                    dtTrackerHoliday.AcceptChanges();
                    dtLocalHoliday.AcceptChanges();

                    if ((dtTrackerHoliday != null && dtTrackerHoliday.Rows.Count > 0) || (dtLocalHoliday != null && dtLocalHoliday.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtTrackerHoliday.Clone();
                        dtSaveRecords.Columns.Remove("H_Operatory_EHR_ID");
                        dtSaveRecords.Columns.Add("H_Operatory_EHR_ID", typeof(string));
                        dtSaveRecords.Columns["H_Operatory_EHR_ID"].DefaultValue = "0";

                        if (dtTrackerHoliday.Select("InsUptDlt IN (1,2)").Count() > 0 || dtLocalHoliday.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtTrackerHoliday.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtTrackerHoliday.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtLocalHoliday.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtLocalHoliday.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Holiday", "H_LocalDB_ID,H_Web_ID", "H_LocalDB_ID");
                        }
                        else
                        {
                            if (dtTrackerHoliday.Select("InsUptDlt IN (4)").Count() > 0)
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

        private void fncSynchDataLocalToTracker_Appointment()
        {
            InitBgWorkerLocalToTracker_Appointment();
            InitBgTimerLocalToTracker_Appointment();
        }

        private void InitBgTimerLocalToTracker_Appointment()
        {
            timerSynchLocalToTracker_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToTracker_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToTracker_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToTracker_Appointment_Tick);
            timerSynchLocalToTracker_Appointment.Enabled = true;
            timerSynchLocalToTracker_Appointment.Start();
            timerSynchLocalToTracker_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToTracker_Appointment()
        {
            bwSynchLocalToTracker_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToTracker_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToTracker_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToTracker_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToTracker_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToTracker_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToTracker_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToTracker_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToTracker_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToTracker_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToTracker_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToTracker_Appointment);
            procThreadmainLocalToTracker_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToTracker_Appointment()
        {
            if (bwSynchLocalToTracker_Appointment.IsBusy != true)
            {
                bwSynchLocalToTracker_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToTracker_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToTracker_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToTracker_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToTracker_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToTracker_Appointment.Enabled = true;
        }

        public void SynchDataLocalToTracker_Appointment()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {

                    //CheckEntryUserLoginIdExist();
                    DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData("1");
                    DataTable dtTrackerPatient = SynchTrackerBAL.GetTrackerPatientListData();
                    DataTable dtIdelProv = SynchTrackerBAL.GetTrackerDefaultProviderData();

                    string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                    string tmpApptProv = "";
                    Int64 tmpPatient_id = 0;
                    int tmpPatient_Gur_id = 0;
                    Int64 tmpAppt_EHR_id = 0;
                    //int tmpNewPatient = 1;
                    DateTime ApptDateTime = DateTime.Now;
                    DateTime ApptEndDateTime = DateTime.Now;

                    string tmpLastName = "";
                    string tmpFirstName = "";

                    string TmpWebPatientName = "";
                    string TmpWebRevPatientName = "";

                    if(dtWebAppointment!=null)
                    {
                        if(dtWebAppointment.Rows.Count>0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                            if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                            {
                                Utility.EHR_UserLogin_ID = SynchTrackerDAL.GetTrackerUserLoginId();
                            }
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
                        //select * FROM Appointment where start_Time > '2020/02/13 00:00' AND End_Time < '2020/02/14 00:00'
                        DataTable dtBookOperatoryApptWiseDateTime = SynchTrackerBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
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
                                DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("location_id = '" + tmpCheckOpId + "'");
                                if (rowBookOpTime.Length > 0)
                                {
                                    for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                    {
                                        appointment_EHR_id = rowBookOpTime[Bop]["Appointment_EHR_Id"].ToString();
                                        ApptDateTime = Convert.ToDateTime(rowBookOpTime[Bop]["start_Time"].ToString());
                                        ApptEndDateTime = Convert.ToDateTime(rowBookOpTime[Bop]["End_Time"].ToString());

                                        if ((tmpStartTime == ApptDateTime || tmpEndTime == ApptEndDateTime))
                                        {
                                            IsConflict = true;
                                            break;
                                        }
                                        else if ((tmpStartTime > ApptDateTime && tmpStartTime < ApptEndDateTime) || (tmpEndTime > ApptDateTime && tmpEndTime < ApptEndDateTime))
                                        {
                                            IsConflict = true;
                                            break;
                                        }
                                        else if ((ApptDateTime > tmpStartTime && ApptDateTime < tmpEndTime) || (ApptEndDateTime > tmpStartTime && ApptEndDateTime < tmpEndTime))
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
                            DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appointment_EHR_Id = " + appointment_EHR_id).CopyToDataTable();
                            bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());
                        }
                        else
                        {
                            if (dtDtxRow["patient_ehr_id"] != null && !string.IsNullOrEmpty(dtDtxRow["patient_ehr_id"].ToString()))
                            {
                                tmpPatient_id = Convert.ToInt64(dtDtxRow["patient_ehr_id"].ToString());
                            }
                            else
                            {
                                tmpPatient_id = 0;
                            }
                            if (tmpPatient_id == 0)
                            {
                                tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtTrackerPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DBConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                                //DataRow[] row = dtTrackerPatient.Copy().Select("Mobile = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR Home_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR Work_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' ");
                                //if (row.Length > 0)
                                //{
                                //    for (int i = 0; i < row.Length; i++)
                                //    {
                                //        if (row[i]["Patient_Name"].ToString().Trim().ToUpper() == TmpWebPatientName.ToString().Trim().ToUpper())
                                //        {
                                //            tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                //        }
                                //        else if (row[i]["Patient_Name"].ToString().Trim().ToUpper() == TmpWebRevPatientName.ToString().Trim().ToUpper())
                                //        {
                                //            tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                //        }
                                //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                //        {
                                //            tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                //        }
                                //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == dtDtxRow["First_Name"].ToString().Trim().ToLower())
                                //        {
                                //            tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                //        }
                                //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == (TmpWebPatientName.ToString().IndexOf(" ") > 0 ? TmpWebPatientName.Substring(0, TmpWebPatientName.ToString().IndexOf(" ")).ToLower() : TmpWebPatientName))
                                //        {
                                //            tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                //        }
                                //        if (tmpPatient_id > 0)
                                //        {
                                //            break;
                                //        }
                                //    }

                                //    tmpPatient_Gur_id = Convert.ToInt32(row[0]["responsible_party"].ToString());
                                //}
                            }
                            //if (tmpPatient_id == 0)
                            //{
                            //    if (dtDtxRow["Last_Name"].ToString().Trim() == null || dtDtxRow["Last_Name"].ToString().Trim() == "")
                            //    {
                            //        //string[] tmpPatientName = dtDtxRow["First_Name"].ToString().Trim().Split(' ');

                            //        string tmpPatientName = dtDtxRow["First_Name"].ToString().Trim();
                            //        var firstSpaceIndex = tmpPatientName.IndexOf(" ");

                            //        if (tmpPatientName.Contains(" "))
                            //        {
                            //            tmpFirstName = tmpPatientName.Substring(0, firstSpaceIndex).ToString().Trim();
                            //            tmpLastName = tmpPatientName.Substring(firstSpaceIndex + 1).ToString().Trim();
                            //        }
                            //        else
                            //        {
                            //            tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                            //            tmpLastName = "Na";
                            //        }
                            //    }
                            //    else
                            //    {
                            //        tmpLastName = dtDtxRow["Last_Name"].ToString().Trim();
                            //        tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                            //    }

                            //    tmpPatient_id = SynchTrackerBAL.Save_Patient_Local_To_Tracker(tmpLastName.Trim(),
                            //                                                                        tmpFirstName,
                            //                                                                        dtDtxRow["MI"].ToString().Trim(),
                            //                                                                        dtDtxRow["Mobile_Contact"].ToString().Trim(),
                            //                                                                        dtDtxRow["Email"].ToString().Trim(),
                            //                                                                        tmpApptProv,
                            //                                                                        dtDtxRow["Appt_DateTime"].ToString().Trim(),
                            //                                                                        tmpPatient_Gur_id,
                            //                                                                        tmpIdealOperatory,
                            //                                                                        dtDtxRow["Birth_Date"].ToString().Trim());
                            //}
                            #endregion
                            TmpWebPatientName = SynchTrackerBAL.GetPatientName(Convert.ToInt32(tmpPatient_id));
                            if (tmpPatient_id > 0)
                            {

                                tmpAppt_EHR_id = SynchTrackerBAL.Save_Appointment_Local_To_Tracker(TmpWebPatientName, tmpStartTime, tmpEndTime, tmpPatient_id.ToString(), tmpIdealOperatory.ToString(), "1", dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                       Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), tmpApptProv, "0", false, false, false, false, (dtDtxRow["appt_treatmentcode"].ToString()), (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "1"));

                                if (tmpAppt_EHR_id > 0)
                                {
                                    bool isApptId_Update = SynchTrackerBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1");
                                }
                            }
                        }
                    }
                    //SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook();
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

        private void fncSynchDataLocalToTracker_Patient_Form()
        {
            InitBgWorkerLocalToTracker_Patient_Form();
            InitBgTimerLocalToTracker_Patient_Form();
        }

        private void InitBgTimerLocalToTracker_Patient_Form()
        {
            timerSynchLocalToTracker_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToTracker_Patient_Form.Interval = 1000 * GoalBase.intervalWebSynch_Pull_PatientForm;
            this.timerSynchLocalToTracker_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToTracker_Patient_Form_Tick);
            timerSynchLocalToTracker_Patient_Form.Enabled = true;
            timerSynchLocalToTracker_Patient_Form.Start();
            timerSynchLocalToTracker_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToTracker_Patient_Form()
        {
            bwSynchLocalToTracker_Patient_Form = new BackgroundWorker();
            bwSynchLocalToTracker_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToTracker_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToTracker_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToTracker_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToTracker_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToTracker_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToTracker_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToTracker_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToTracker_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToTracker_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToTracker_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToTracker_Patient_Form);
            procThreadmainLocalToTracker_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToTracker_Patient_Form()
        {
            if (bwSynchLocalToTracker_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToTracker_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToTracker_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToTracker_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToTracker_Patient_Form();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToTracker_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToTracker_Patient_Form.Enabled = true;
        }

        #region PatientPayment
        private void fncSynchDataTracker_PatientPayment()
        {
            InitBgWorkerTracker_PatientPayment();
            InitBgTimerTracker_PatientPayment();
        }

        private void InitBgTimerTracker_PatientPayment()
        {
            timerSynchTracker_PatientPayment = new System.Timers.Timer();
            this.timerSynchTracker_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
            this.timerSynchTracker_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_PatientPayment_Tick);
            timerSynchTracker_PatientPayment.Enabled = true;
            timerSynchTracker_PatientPayment.Start();
        }

        private void InitBgWorkerTracker_PatientPayment()
        {
            bwSynchTracker_PatientPayment = new BackgroundWorker();
            bwSynchTracker_PatientPayment.WorkerReportsProgress = true;
            bwSynchTracker_PatientPayment.WorkerSupportsCancellation = true;
            bwSynchTracker_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchTracker_PatientPayment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchTracker_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_PatientPayment_RunWorkerCompleted);
        }

        private void timerSynchTracker_PatientPayment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_PatientPayment.Enabled = false;
                MethodForCallSynchOrderTracker_PatientPayment();
            }
        }

        public void MethodForCallSynchOrderTracker_PatientPayment()
        {
            System.Threading.Thread procThreadmainTracker_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTableTracker_PatientPayment);
            procThreadmainTracker_PatientPayment.Start();
        }

        public void CallSyncOrderTableTracker_PatientPayment()
        {
            if (bwSynchTracker_PatientPayment.IsBusy != true)
            {
                bwSynchTracker_PatientPayment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_PatientPayment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_PatientPayment_LocalToTracker();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_PatientPayment.Enabled = true;
        }

        public void SynchDataLiveDB_PatientPayment_LocalToTracker()
        {
            try
            {
                if (!IsPaymentSyncing)
                {
                    IsPaymentSyncing = true;
                    if (Utility.IsApplicationIdleTimeOff)
                    {
                        Int64 TransactionHeaderId = 0;
                        string noteId = "";
                        SynchDataLiveDB_Pull_PatientPaymentLog();
                        //CheckEntryUserLoginIdExist();
                        DataTable dtPatientPayment = new DataTable();
                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            //DataTable dtWebPatientPaymentSplit= SynchLocalBAL.GetLocalWebPatientPaymentSplitData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            noteId = "";

                            #region Insert Payment Records in EHR
                            if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                            {
                                Utility.CheckEntryUserLoginIdExist();
                                if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                                {
                                    Utility.EHR_UserLogin_ID = SynchTrackerDAL.GetTrackerUserLoginId();
                                }
                                // SynchTrackerBAL.SavePatientPaymentTOEHR(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), dtWebPatientPayment, dtWebPatientPaymentSplit, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                SynchTrackerBAL.SavePatientPaymentTOEHR(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                ObjGoalBase.WriteToSyncLogFile("Patient Payment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");
                            }

                            #endregion

                            //#region Call API for EHR Entry Done
                            //if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                            //{
                            //    noteId = SynchTrackerBAL.Save_PatientPaymentLog_LocalToTracker(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            //    if (noteId != "")
                            //    {
                            //        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientPayment_LocalTOTracker");
                            //        ObjGoalBase.WriteToSyncLogFile("PatientPayment_LocalTOTracker Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            //    }
                            //    else
                            //    {
                            //        ObjGoalBase.WriteToErrorLogFile("[PatientPayment_LocalTOTracker Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                            //    }
                            //}                          
                            //else
                            //{
                            //    ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                            //}
                            //#endregion

                            #region Sync those patient whose payment done in EHR

                            #endregion
                            //  }
                        }

                    }
                    IsPaymentSyncing = false;
                }
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Patient Payment Log Sync " + Ex.Message);
            }
            finally { IsPaymentSyncing = false; }
        }
        #endregion

        public void SynchDataLiveDB_PatientSMSCall_LocalToTracker()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    Int64 TransactionHeaderId = 0;
                    string noteId = "";
                    //CheckEntryUserLoginIdExist();
                    DataTable dtPatientPayment = new DataTable();
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientSMSCallLogData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        //ObjGoalBase.WriteToSyncLogFile("Records Exists " + dtWebPatientPayment.Rows.Count.ToString());
                        #region Call API for EHR Entry Done
                        if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                            // ObjGoalBase.WriteToSyncLogFile("Call to save records to EHR");
                            SynchTrackerBAL.Save_PatientSMSCallLog_LocalToTracker(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            //}
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientPayment_LocalTOTracker");
                            ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                        }

                        #endregion

                        //  }
                    }

                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[PatientSMSCallLog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public void SynchDataLocalToTracker_Patient_Form()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    SynchDataLiveDB_Pull_PatientForm();
                    SynchDataLiveDB_Pull_PatientPortal();
                    //CheckEntryUserLoginIdExist();
                    //try
                    //{
                    //    //live to local save data pull
                    //    SynchDataLiveDB_Pull_treatmentDoc();

                    //    //live to local save PDF pull
                    //    SyncTreatmentDocument();
                    //}
                    //catch (Exception ex)
                    //{
                    //    ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                    //    // throw;
                    //}
                    DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData("1");

                    #region new Code for Patient Save
                    //                   DataTable dtTrackerColumns = new DataTable();
                    //                   DataTable dtLocalColumns = new DataTable();

                    //                   if(dtWebPatient_Form.Columns.Contains())

                    //                   SynchTrackerBAL.GetTrackerTableColumnList("Contact", ref dtTrackerColumns, ref dtLocalColumns);

                    //                   dtWebPatient_Form.AsEnumerable()
                    //                       .All(a =>
                    //                       {
                    //                           if (a["COLUMN_NAME"].ToString().ToUpper() == "PATIENT_EHR_ID")
                    //                           {
                    //                               a["EHRColumnName"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString() == "First_name")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "first_name";
                    //                               a["EHRColumnName"] = "FirstName";
                    //                               a["DefaultValue"] = "NA";

                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "LAST_NAME")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "last_name";
                    //                               a["EHRColumnName"] = "LastName";
                    //                               a["DefaultValue"] = "NA";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MIDDLE_NAME")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "middle_name";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "NA";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MOBILE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "mobile";
                    //                               a["EHRColumnName"] = "MobileNumber";
                    //                               a["DefaultValue"] = "0000000000";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "STATUS")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "IsActive";
                    //                               a["DefaultValue"] = "1";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ADDRESS1")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "address_one";
                    //                               a["EHRColumnName"] = "Address1";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ADDRESS2")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "address_two";
                    //                               a["EHRColumnName"] = "Address2";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "BIRTH_DATE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "birth_date";
                    //                               a["EHRColumnName"] = "BirthDate";
                    //                               a["DefaultValue"] = null;
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "MARITALSTATUS")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "marital_status";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "STATE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "state";
                    //                               a["EHRColumnName"] = "";
                    //                               //a["DefaultValue"] = null;
                    //                           }

                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "CITY")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "city";
                    //                               a["EHRColumnName"] = "City";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "CURRENTBAL")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "0";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "THIRTYDAY")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "0";
                    //                           }

                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SIXTYDAY")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "0";
                    //                           }

                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "NINETYDAY")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "0";
                    //                           }
                    ////                            select Column_Name AS ColumnName,Table_Name AS TableName,Data_Type AS DataType,IS_Nullable AS AllowNull,
                    ////(Case when Character_maximum_length is not null then Character_maximum_length else numeric_precision end ) AS Size   
                    ////from information_schema.columns where table_name like 'Contact'

                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "EMAIL")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "email";
                    //                               a["EHRColumnName"] = "Email";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "HOME_PHONE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "home_phone";
                    //                               a["EHRColumnName"] = "PhoneNumber";
                    //                               a["DefaultValue"] = "0000000000";
                    //                           }

                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PREFERRED_NAME")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "preferred_name";
                    //                               a["EHRColumnName"] = "NickName";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "pri_provider_id";
                    //                               a["EHRColumnName"] = "ProviderId";

                    //                               a["DefaultValue"] = "0";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVEEMAIL")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "receive_email";
                    //                               a["EHRColumnName"] = "HasEmailConsent";
                    //                               a["DefaultValue"] = "Y";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "RECEIVESMS")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "receive_sms";                                
                    //                               a["EHRColumnName"] = "IsSmsEnabled";
                    //                               a["DefaultValue"] = "Y";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SALUTATION")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "salutation"; 
                    //                               a["EHRColumnName"] = "Title";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "sec_provider_id"; 
                    //                               a["EHRColumnName"] = "Provider2Id";
                    //                               a["DefaultValue"] = "0";
                    //                               //a["EHRColumnName"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "SEX")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "SEX";
                    //                               a["EHRColumnName"] = "sex";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "WORK_PHONE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "work_phone";
                    //                               a["EHRColumnName"] = "WorkPhoneNumber";
                    //                               a["DefaultValue"] = "0000000000";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "ZIPCODE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "zipcode";
                    //                               a["EHRColumnName"] = "zipcode";
                    //                               a["DefaultValue"] = "";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "FIRSTVISIT_DATE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "first_visit_date";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "LASTVISIT_DATE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "last_date_seen";
                    //                           }
                    //                           if (a["COLUMN_NAME"].ToString().Trim().ToUpper() == "NEXTVISIT_DATE")
                    //                           {
                    //                               a["PatientFormColumnsName"] = "";
                    //                               a["EHRColumnName"] = "next_regular_appointment";
                    //                           }

                    //                           if (a["EHRColumnName"].ToString() != "" && a["PatientFormColumnsName"].ToString() != "" && dtWebPatient_Form.AsEnumerable().Where(o => o.Field<object>("ehrfield").ToString().ToUpper() == a["PatientFormColumnsName"].ToString().ToUpper()).Count() > 0 &&
                    //                               dtTrackerColumns.AsEnumerable().Where(r => r.Field<string>("ColumnName,").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Count() > 0 )
                    //                           {
                    //                               a["EHRColumnsvalue"] = dtWebPatient_Form.AsEnumerable().Where(o => o.Field<object>("ehrfield").ToString().ToUpper() == a["PatientFormColumnsName"].ToString().ToUpper()).Select(r => r.Field<string>("ehrfield_value")).First().ToString();
                    //                               a["Size"] = dtTrackerColumns.AsEnumerable().Where(r => r.Field<string>("ColumnName,").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<string>("Size")).First().ToString();
                    //                               a["EHRDataType"] = dtTrackerColumns.AsEnumerable().Where(r => r.Field<string>("ColumnName,").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<string>("DataType")).First().ToString();
                    //                               a["AllowNull"] = dtTrackerColumns.AsEnumerable().Where(r => r.Field<string>("ColumnName,").ToString().ToUpper() == a["EHRColumnName"].ToString().ToUpper()).Select(r => r.Field<string>("AllowNull")).First().ToString();
                    //                           }
                    //                           return true;
                    //                       });

                    #endregion

                    dtWebPatient_Form.Columns.Add("TableName", typeof(string));
                    dtWebPatient_Form.Columns["TableName"].DefaultValue = "Contact";
                    dtWebPatient_Form.Columns.Add("pformfield", typeof(string));

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
                            dtDtxRow["ehrfield"] = "FirstName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "last_name")
                        {
                            dtDtxRow["ehrfield"] = "LastName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "mobile")
                        {
                            dtDtxRow["ehrfield"] = "MobileNumber";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_one")
                        {
                            dtDtxRow["ehrfield"] = "Address1";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_two")
                        {
                            dtDtxRow["ehrfield"] = "Address2";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "birth_date")
                        {
                            dtDtxRow["ehrfield"] = "BirthDate";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "city")
                        {
                            dtDtxRow["ehrfield"] = "City";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "email")
                        {
                            dtDtxRow["ehrfield"] = "Email";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "home_phone")
                        {
                            dtDtxRow["ehrfield"] = "PhoneNumber";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "marital_status")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "middle_name")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "preferred_name")
                        {
                            dtDtxRow["ehrfield"] = "NickName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "pri_provider_id")
                        {
                            dtDtxRow["ehrfield"] = "ProviderId";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                        {
                            dtDtxRow["ehrfield"] = "PRIMARY_SUBSCRIBER_ID";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance_companyname")
                        {
                            dtDtxRow["ehrfield"] = "primary_insurance_companyname";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_email")
                        {
                            dtDtxRow["ehrfield"] = "HasEmailConsent";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_sms")
                        {
                            dtDtxRow["ehrfield"] = "IsSmsEnabled";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "salutation")
                        {
                            dtDtxRow["ehrfield"] = "Title";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "sec_provider_id")
                        {
                            dtDtxRow["ehrfield"] = "Provider2Id";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                        {
                            dtDtxRow["ehrfield"] = "SECONDARY_SUBSCRIBER_ID";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance_companyname")
                        {
                            dtDtxRow["ehrfield"] = "secondary_insurance_companyname";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "sex")
                        {
                            dtDtxRow["ehrfield"] = "sex";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "work_phone")
                        {
                            dtDtxRow["ehrfield"] = "WorkPhoneNumber";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "zipcode")
                        {
                            dtDtxRow["ehrfield"] = "PostalCode";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "ssn")
                        {
                            dtDtxRow["ehrfield"] = "IdentificationNumber";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "school")
                        {
                            dtDtxRow["ehrfield"] = "Company";
                            dtDtxRow["pformfield"] = "school";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "employer")
                        {
                            dtDtxRow["ehrfield"] = "Company";
                            dtDtxRow["pformfield"] = "employer";
                        }

                        dtWebPatient_Form.AcceptChanges();

                    }

                    if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                    {
                        bool Is_Record_Update = SynchTrackerBAL.Save_Patient_Form_Local_To_Tracker(dtWebPatient_Form);
                    }
                    string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing("1");
                    if (Call_Importing.ToLower() != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Importing status : " + Call_Importing);
                    }
                    //try
                    //{
                    //    GetPatientDocument("1");
                    //    SynchTrackerBAL.Save_Document_in_Tracker();
                    //}
                    //catch (Exception ex)
                    //{
                    //    ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);                        
                    //}
                    string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed("1");
                    if (Call_Completed.ToLower() != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Completed status : " + Call_Completed);
                    }

                    string Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed("1", Utility.Location_ID);
                    if (Call_PatientPortalCompleted != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile("[Patient_Portal API called with Completed status : " + Call_PatientPortalCompleted);
                    }

                    bool isRecordSaved = false, isRecordDeleted = false;
                    string Patient_EHR_IDS = "";
                    string DeletePatientEHRID = "";
                    string SavePatientEHRID = "";
                    try
                    {
                        if (SynchTrackerBAL.DeletePatientMedicationLocalToTracker(ref isRecordDeleted, ref DeletePatientEHRID))
                        {
                            ObjGoalBase.WriteToSyncLogFile("[Delete_Patient_Medication to EHR Status : True");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication to EHR Status : False");
                        }
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication to EHR Error Log] : " + ex.Message);
                    }
                    try
                    {
                        if (SynchTrackerBAL.SavePatientMedicationLocalToTracker(ref isRecordSaved, ref SavePatientEHRID))
                        {
                            ObjGoalBase.WriteToSyncLogFile("[Save_Patient_Medication to EHR Status : True");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Save_Patient_Medication to EHR Status : False");
                        }
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Save_Patient_Medication to EHR Error Log] : " + ex.Message);
                    }
                    if (isRecordSaved || isRecordDeleted)
                    {
                        Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                        if (Patient_EHR_IDS != "")
                        {
                            SynchDataTracker_PatientMedication(Patient_EHR_IDS);
                        }
                    }
                    SynchDataLocalToTracker_Patient_Document();
                    //try
                    //{
                    //    //Treatment Document Sync
                    //    if (SynchTrackerBAL.Save_TreatmentDocument_Form_Local_To_Tracker())
                    //    {
                    //        ObjGoalBase.WriteToSyncLogFile("[Patient_Treatment_Documen attachment Completed status : True");
                    //    }
                    //    #region change status as treatment doc impotred Completed
                    //    DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                    //    if (statusCompleted.Rows.Count > 0)
                    //    {
                    //        Change_Status_TreatmentDoc(statusCompleted, "Completed");
                    //    }
                    //    #endregion
                    //}
                    //catch (Exception ex)
                    //{
                    //    ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                    //    // throw;
                    //}
                    ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                if (appointmenetSyncFailedCounter < 10)
                {
                    appointmenetSyncFailedCounter++;
                }
                else
                {
                    if (ex.Message.ToString().Contains("OutOfMemoryException"))
                    {
                        System.Environment.Exit(1);
                    }
                }
            }

        }

        //public 

        public void SynchDataTracker_PatientMedication(string Patinet_EHR_IDS)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchTrackerBAL.GetTrackerPatientMedicationData(Patinet_EHR_IDS);
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patinet_EHR_IDS);

                        foreach (DataRow dtDtxRow in dtMedication.Rows)
                        {
                            DataRow[] row = dtLocalMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtDtxRow["PatientMedication_EHR_ID"].ToString() + "' And Medication_EHR_ID = '" + dtDtxRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
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
                            DataRow[] rowDis = dtMedication.Copy().Select("PatientMedication_EHR_ID = " + dtLPHRow["PatientMedication_EHR_ID"].ToString() + " And Medication_EHR_ID = '" + dtLPHRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtLPHRow["Patient_EHR_ID"].ToString() + "'");
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

        private void fncSynchDataTracker_Insurance()
        {
            InitBgWorkerTracker_Insurance();
            InitBgTimerTracker_Insurance();
        }

        private void InitBgTimerTracker_Insurance()
        {
            timerSynchTracker_Insurance = new System.Timers.Timer();
            this.timerSynchTracker_Insurance.Interval = 1000 * GoalBase.intervalEHRSynch_Insurance;
            this.timerSynchTracker_Insurance.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchTracker_Insurance_Tick);
            timerSynchTracker_Insurance.Enabled = true;
            timerSynchTracker_Insurance.Start();
        }

        private void InitBgWorkerTracker_Insurance()
        {
            bwSynchTracker_Insurance = new BackgroundWorker();
            bwSynchTracker_Insurance.WorkerReportsProgress = true;
            bwSynchTracker_Insurance.WorkerSupportsCancellation = true;
            bwSynchTracker_Insurance.DoWork += new DoWorkEventHandler(bwSynchTracker_Insurance_DoWork);
            bwSynchTracker_Insurance.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchTracker_Insurance_RunWorkerCompleted);
        }

        private void timerSynchTracker_Insurance_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchTracker_Insurance.Enabled = false;
                MethodForCallSynchOrderTracker_Insurance();
            }
        }

        public void MethodForCallSynchOrderTracker_Insurance()
        {
            System.Threading.Thread procThreadmainTracker_Insurance = new System.Threading.Thread(this.CallSyncOrderTableTracker_Insurance);
            procThreadmainTracker_Insurance.Start();
        }

        public void CallSyncOrderTableTracker_Insurance()
        {
            if (bwSynchTracker_Insurance.IsBusy != true)
            {
                bwSynchTracker_Insurance.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchTracker_Insurance_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchTracker_Insurance.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataTracker_Insurance();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchTracker_Insurance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchTracker_Insurance.Enabled = true;
        }

        public void SynchDataTracker_Insurance()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtSoftDentInsurance = SynchTrackerBAL.GetTrackerInsuranceData();

                    dtSoftDentInsurance.Columns.Add("InsUptDlt", typeof(int));
                    dtSoftDentInsurance.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalInsurance = SynchLocalBAL.GetLocalInsuranceData("1");

                    if (!dtLocalInsurance.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalInsurance.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalInsurance.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    foreach (DataRow dtDtxRow in dtSoftDentInsurance.Rows)
                    {
                        DataRow[] row = dtLocalInsurance.Copy().Select("Insurance_EHR_ID = '" + dtDtxRow["CarrierId"] + "' ");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Company"].ToString().ToLower().Trim() != row[0]["Insurance_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Address1"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Address2"].ToString().ToLower().Trim() != row[0]["Address2"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["City"].ToString().ToLower().Trim() != row[0]["City"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["RegionId"].ToString().ToLower().Trim() != row[0]["State"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["PostalCode"].ToString().ToLower().Trim() != row[0]["Zipcode"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["PhoneNumber"].ToString().ToLower().Trim() != row[0]["Phone"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["UniqueId"].ToString().ToLower().Trim() != row[0]["ElectId"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            //else if (dtDtxRow["EmployerName"].ToString().ToLower().Trim() != row[0]["EmployerName"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}
                            //else if (dtDtxRow["IsActive"].ToString().ToLower().Trim() != row[0]["Is_Deleted"].ToString().ToLower().Trim())
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
                        DataRow[] row = dtSoftDentInsurance.Copy().Select("CarrierId = '" + dtDtxRow["Insurance_EHR_ID"] + "' ");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtSoftDentInsurance.NewRow();
                            BlcOptDtldr["CarrierId"] = dtDtxRow["Insurance_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["Company"] = dtDtxRow["Insurance_Name"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtSoftDentInsurance.Rows.Add(BlcOptDtldr);
                        }
                    }

                    dtSoftDentInsurance.AcceptChanges();

                    if (dtSoftDentInsurance != null && dtSoftDentInsurance.Rows.Count > 0)
                    {
                        bool status = SynchTrackerBAL.Save_Insurance_Tracker_To_Local(dtSoftDentInsurance, "1");
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Insurance");
                            ObjGoalBase.WriteToSyncLogFile("Insurance Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Insurance();
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Insurance Sync (" + Utility.Application_Name + " to Local Database) ] Error.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Insurance Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion


        #region Sync Patient Document


        public void SynchDataLocalToTracker_Patient_Document()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    //CheckEntryUserLoginIdExist();
                    try
                    {
                        //live to local save data pull
                        SynchDataLiveDB_Pull_treatmentDoc();
                        SynchDataLiveDB_Pull_InsuranceCarrierDoc();
                        //live to local save PDF pull
                        SyncTreatmentDocument();
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                        // throw;
                    }

                    try
                    {
                        GetPatientDocument("1");
                        GetPatientDocument_New("1");
                        SynchTrackerBAL.Save_Document_in_Tracker();
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }
                    #region Treatment Document
                    try
                    {
                        //Treatment Document Sync
                        if (SynchTrackerBAL.Save_TreatmentDocument_Form_Local_To_Tracker())
                        {
                            ObjGoalBase.WriteToSyncLogFile("[Patient_Treatment_Documen attachment Completed status : True");
                        }
                        #region change status as treatment doc impotred Completed
                        DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                        if (statusCompleted.Rows.Count > 0)
                        {
                            Change_Status_TreatmentDoc(statusCompleted, "Completed");
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                        // throw;
                    }
                    #endregion

                    #region Insurance Carrier
                    try
                    {
                        if (SynchTrackerBAL.Save_InsuranceCarrierDocument_Form_Local_To_Tracker())
                        {
                            ObjGoalBase.WriteToSyncLogFile("[Patient_Insurance Carrier_Document attachment Completed status : True");
                        }
                        #region change status as Insurance Carrier doc impotred Completed
                        DataTable statusCompleted = SynchLocalBAL.ChangeStatusForInsuranceCarrierDoc("Completed");
                        if (statusCompleted.Rows.Count > 0)
                        {
                            Change_Status_InsuranceCarrierDoc(statusCompleted, "Completed");
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Insurance Carrier Document Error log] : " + ex.Message);
                        // throw;
                    }
                    #endregion
                    ObjGoalBase.WriteToSyncLogFile("Patient_Document Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                if (appointmenetSyncFailedCounter < 10)
                {
                    appointmenetSyncFailedCounter++;
                }
                else
                {
                    if (ex.Message.ToString().Contains("OutOfMemoryException"))
                    {
                        System.Environment.Exit(1);
                    }
                }
            }

        }

        #endregion

        #region NoteDataMoveToCorrespond
        public void NoteDataMoveToCorrespond()
        {
            try
            {
                if (Utility.TrackerNoteMove == 0)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtNote = SynchLocalBAL.GetAllLocalNoteId(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        if (dtNote != null)
                        {
                            dtNote.Columns.Add("NewLogEHRId", typeof(System.String));
                            dtNote.AcceptChanges();
                            ObjGoalBase.WriteToSyncLogFile("Note Data Move To Correspond Count : " + dtNote.Rows.Count);
                            if (dtNote.Rows.Count > 0)
                            {
                                DataTable dt = SynchTrackerBAL.Save_NoteDataMoveToCorrespondInTracker(dtNote, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                SynchLocalBAL.CallUpdateNoteDataMoveToCorrespondInTracker(dt, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                ObjGoalBase.WriteToSyncLogFile("Note Data Move To Correspond (Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                        }

                    }

                    try
                    {
                        Microsoft.Win32.RegistryKey key1 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerNoteMove");
                        Utility.TrackerNoteMove = 1;
                        key1.SetValue("TrackerNoteMove", Utility.TrackerNoteMove);
                    }
                    catch (Exception ex1)
                    {
                        GoalBase.WriteToErrorLogFile_Static("[NoteDataMoveToCorrespond Sync (Adit Server To Local Database)] : " + ex1.Message);
                        Utility.TrackerNoteMove = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[NoteDataMoveToCorrespond Sync (Adit Server To Local Database)] : " + ex.Message);
            }
        }
        #endregion

        #region Event Listener
        public static bool SynchDataLocalToTracker_AppointmentFromEvent(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id)
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

                DataTable dtTrackerPatient = SynchTrackerBAL.GetTrackerPatientListData();
                DataTable dtIdelProv = SynchTrackerBAL.GetTrackerDefaultProviderData();

                string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                string tmpApptProv = "";
                Int64 tmpPatient_id = 0;
                int tmpPatient_Gur_id = 0;
                Int64 tmpAppt_EHR_id = 0;
                //int tmpNewPatient = 1;
                DateTime ApptDateTime = DateTime.Now;
                DateTime ApptEndDateTime = DateTime.Now;

                string tmpLastName = "";
                string tmpFirstName = "";

                string TmpWebPatientName = "";
                string TmpWebRevPatientName = "";

                if (dtWebAppointment != null)
                {
                    if (dtWebAppointment.Rows.Count > 0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = SynchTrackerDAL.GetTrackerUserLoginId();
                        }
                    }
                }

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
                    //select * FROM Appointment where start_Time > '2020/02/13 00:00' AND End_Time < '2020/02/14 00:00'
                    DataTable dtBookOperatoryApptWiseDateTime = SynchTrackerBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
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
                            DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("location_id = '" + tmpCheckOpId + "'");
                            if (rowBookOpTime.Length > 0)
                            {
                                for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                {
                                    appointment_EHR_id = rowBookOpTime[Bop]["Appointment_EHR_Id"].ToString();
                                    ApptDateTime = Convert.ToDateTime(rowBookOpTime[Bop]["start_Time"].ToString());
                                    ApptEndDateTime = Convert.ToDateTime(rowBookOpTime[Bop]["End_Time"].ToString());

                                    if ((tmpStartTime == ApptDateTime || tmpEndTime == ApptEndDateTime))
                                    {
                                        IsConflict = true;
                                        break;
                                    }
                                    else if ((tmpStartTime > ApptDateTime && tmpStartTime < ApptEndDateTime) || (tmpEndTime > ApptDateTime && tmpEndTime < ApptEndDateTime))
                                    {
                                        IsConflict = true;
                                        break;
                                    }
                                    else if ((ApptDateTime > tmpStartTime && ApptDateTime < tmpEndTime) || (ApptEndDateTime > tmpStartTime && ApptEndDateTime < tmpEndTime))
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
                        DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appointment_EHR_Id = " + appointment_EHR_id).CopyToDataTable();
                        bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id, dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());
                    }
                    else
                    {
                        if (dtDtxRow["patient_ehr_id"] != null && !string.IsNullOrEmpty(dtDtxRow["patient_ehr_id"].ToString()))
                        {
                            tmpPatient_id = Convert.ToInt64(dtDtxRow["patient_ehr_id"].ToString());
                        }
                        else
                        {
                            tmpPatient_id = 0;
                        }
                        if (tmpPatient_id == 0)
                        {
                            tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtTrackerPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), strDbConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                        }
                        #endregion

                        TmpWebPatientName = SynchTrackerBAL.GetPatientName(Convert.ToInt32(tmpPatient_id));
                        if (tmpPatient_id > 0)
                        {

                            tmpAppt_EHR_id = SynchTrackerBAL.Save_Appointment_Local_To_Tracker(TmpWebPatientName, tmpStartTime, tmpEndTime, tmpPatient_id.ToString(), tmpIdealOperatory.ToString(), "1", dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                   Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), tmpApptProv, "0", false, false, false, false, (dtDtxRow["appt_treatmentcode"].ToString()), (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "1"));

                            if (tmpAppt_EHR_id > 0)
                            {
                                bool isApptId_Update = SynchTrackerBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1");
                            }
                        }

                        #region Appointment Sync
                        SynchDataTracker_AppointmentFromEvents(strDbConnString, Clinic_Number, Service_Install_Id, tmpAppt_EHR_id.ToString(), tmpPatient_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                        #endregion
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

        public static void SynchDataTracker_AppointmentFromEvents(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        {
            try
            {
                SynchDataTracker_AppointmentsPatientFromEvents(strDbString, Clinic_Number, Service_Install_Id, strPatID);
                SynchDataTracker_PatientStatusFromEvent(strDbString, Clinic_Number, Service_Install_Id, strPatID);
                SynchDataLiveDB_Push_Patient(strPatID);

                DataTable dtSoftDentAppointment = SynchTrackerBAL.GetTrackerAppointmentData(strApptID);

                dtSoftDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                dtSoftDentAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                dtSoftDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                dtSoftDentAppointment.Columns.Add("ProcedureCode", typeof(string));

                DataTable DtTrackerAppointment_Procedures_Data = SynchTrackerBAL.GetTrackerAppointment_Procedures_Data(strApptID);

                string ProcedureDesc = "";
                string ProcedureCode = "";
                ///////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                foreach (DataRow dtDtxRow in dtSoftDentAppointment.Rows)
                {
                    ProcedureDesc = "";
                    ProcedureCode = "";
                    DataRow[] dtCurApptProcedure = DtTrackerAppointment_Procedures_Data.Select("Appointmentid = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");
                    foreach (var dtSinProc in dtCurApptProcedure.ToList())
                    {
                        ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                    }
                    dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                    dtDtxRow["ProcedureCode"] = ProcedureCode;

                }
                /////////////////////

                DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Service_Install_Id, strApptID);

                DataTable dtSaveRecords = new DataTable();
                dtSaveRecords = dtLocalAppointment.Clone();
                var itemsToBeAdded = (from TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                                      join LocalAppt in dtLocalAppointment.AsEnumerable()
                                      on TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                                      equals LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                      into matchingRows
                                      from matchingRow in matchingRows.DefaultIfEmpty()
                                      where matchingRow == null
                                      select TrackerAppt).ToList();
                DataTable dtApptToBeAdded = dtLocalAppointment.Clone();
                if (itemsToBeAdded.Count > 0)
                {
                    dtApptToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
                }

                if (!dtApptToBeAdded.Columns.Contains("InsUptDlt"))
                {
                    dtApptToBeAdded.Columns.Add("InsUptDlt", typeof(int));
                    dtApptToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
                }
                if (dtApptToBeAdded.Rows.Count > 0)
                {
                    dtApptToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
                    dtSaveRecords.Load(dtApptToBeAdded.Select().CopyToDataTable().CreateDataReader());
                }

                var itemsToBeUpdated = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                        join TrackerAppt in dtSoftDentAppointment.AsEnumerable()
                                        on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                        equals TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                                        where
                                        (LocalAppt["First_name"] != DBNull.Value ? LocalAppt["First_name"].ToString().Trim() : "") != (TrackerAppt["First_name"] != DBNull.Value ? TrackerAppt["First_name"].ToString().Trim() : "") ||
                                        (LocalAppt["Last_name"] != DBNull.Value ? LocalAppt["Last_name"].ToString().Trim() : "") != (TrackerAppt["Last_name"] != DBNull.Value ? TrackerAppt["Last_name"].ToString().Trim() : "") ||
                                        (LocalAppt["MI"] != DBNull.Value ? LocalAppt["MI"].ToString().Trim() : "") != (TrackerAppt["MI"] != DBNull.Value ? TrackerAppt["MI"].ToString().Trim() : "") ||
                                        (LocalAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Home_Contact"].ToString().Trim()) : "") != (TrackerAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(TrackerAppt["Home_Contact"].ToString().Trim()) : "") ||
                                        (LocalAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Mobile_Contact"].ToString().Trim()) : "") != (TrackerAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(TrackerAppt["Mobile_Contact"].ToString().Trim()) : "") ||
                                        (LocalAppt["Email"] != DBNull.Value ? LocalAppt["Email"].ToString().Trim() : "") != (TrackerAppt["Email"] != DBNull.Value ? TrackerAppt["Email"].ToString().Trim() : "") ||
                                        (LocalAppt["Address"] != DBNull.Value ? LocalAppt["Address"].ToString().Trim() : "") != (TrackerAppt["Address"] != DBNull.Value ? TrackerAppt["Address"].ToString().Trim() : "") ||
                                        (LocalAppt["City"] != DBNull.Value ? LocalAppt["City"].ToString().Trim() : "") != (TrackerAppt["City"] != DBNull.Value ? TrackerAppt["City"].ToString().Trim() : "") ||
                                        (LocalAppt["Zip"] != DBNull.Value ? LocalAppt["Zip"].ToString().Trim() : "") != (TrackerAppt["Zip"] != DBNull.Value ? TrackerAppt["Zip"].ToString().Trim() : "") ||
                                        (LocalAppt["Operatory_EHR_ID"] != DBNull.Value ? LocalAppt["Operatory_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Operatory_EHR_ID"] != DBNull.Value ? TrackerAppt["Operatory_EHR_ID"].ToString().Trim() : "") ||
                                        (LocalAppt["Operatory_Name"] != DBNull.Value ? LocalAppt["Operatory_Name"].ToString().Trim() : "") != (TrackerAppt["Operatory_Name"] != DBNull.Value ? TrackerAppt["Operatory_Name"].ToString().Trim() : "") ||
                                        (LocalAppt["Provider_EHR_ID"] != DBNull.Value ? LocalAppt["Provider_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Provider_EHR_ID"] != DBNull.Value ? TrackerAppt["Provider_EHR_ID"].ToString().Trim() : "") ||
                                        (LocalAppt["Provider_Name"] != DBNull.Value ? LocalAppt["Provider_Name"].ToString().Trim() : "") != (TrackerAppt["Provider_Name"] != DBNull.Value ? TrackerAppt["Provider_Name"].ToString().Trim() : "") ||
                                        (LocalAppt["Comment"] != DBNull.Value ? LocalAppt["Comment"].ToString().Trim() : "") != (TrackerAppt["Comment"] != DBNull.Value ? TrackerAppt["Comment"].ToString().Trim() : "") ||
                                        (LocalAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["birth_date"].ToString().Trim()) : "") != (TrackerAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["birth_date"].ToString().Trim()) : "") ||
                                        (LocalAppt["ApptType_EHR_ID"] != DBNull.Value ? LocalAppt["ApptType_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["ApptType_EHR_ID"] != DBNull.Value ? TrackerAppt["ApptType_EHR_ID"].ToString().Trim() : "") ||
                                        (LocalAppt["ApptType"] != DBNull.Value ? LocalAppt["ApptType"].ToString().Trim() : "") != (TrackerAppt["ApptType"] != DBNull.Value ? TrackerAppt["ApptType"].ToString().Trim() : "") ||
                                        (LocalAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_DateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_DateTime"].ToString().Trim()) : "") ||
                                        (LocalAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_EndDateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_EndDateTime"].ToString().Trim()) : "") ||
                                        (LocalAppt["Status"] != DBNull.Value ? LocalAppt["Status"].ToString().Trim() : "") != (TrackerAppt["Status"] != DBNull.Value ? TrackerAppt["Status"].ToString().Trim() : "") ||
                                        (LocalAppt["appointment_status_ehr_key"] != DBNull.Value ? LocalAppt["appointment_status_ehr_key"].ToString().Trim() : "") != (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") ||
                                        (LocalAppt["Appointment_Status"] != DBNull.Value ? LocalAppt["Appointment_Status"].ToString().Trim() : "") != (TrackerAppt["Appointment_Status"] != DBNull.Value ? TrackerAppt["Appointment_Status"].ToString().Trim() : "") ||
                                        (LocalAppt["confirmed_status_ehr_key"] != DBNull.Value ? LocalAppt["confirmed_status_ehr_key"].ToString().Trim() : "") != (TrackerAppt["confirmed_status_ehr_key"] != DBNull.Value ? TrackerAppt["confirmed_status_ehr_key"].ToString().Trim() : "") ||
                                        (LocalAppt["confirmed_status"] != DBNull.Value ? LocalAppt["confirmed_status"].ToString().Trim() : "") != (TrackerAppt["confirmed_status"] != DBNull.Value ? TrackerAppt["confirmed_status"].ToString().Trim() : "") ||
                                        (LocalAppt["patient_ehr_id"] != DBNull.Value ? LocalAppt["patient_ehr_id"].ToString().Trim() : "") != (TrackerAppt["patient_ehr_id"] != DBNull.Value ? TrackerAppt["patient_ehr_id"].ToString().Trim() : "") ||
                                        (LocalAppt["ProcedureDesc"] != DBNull.Value ? LocalAppt["ProcedureDesc"].ToString().Trim() : "") != (TrackerAppt["ProcedureDesc"] != DBNull.Value ? TrackerAppt["ProcedureDesc"].ToString().Trim() : "") ||
                                        (LocalAppt["ProcedureCode"] != DBNull.Value ? LocalAppt["ProcedureCode"].ToString().Trim() : "") != (TrackerAppt["ProcedureCode"] != DBNull.Value ? TrackerAppt["ProcedureCode"].ToString().Trim() : "") ||
                                        (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim() : "") != (TrackerAppt["is_deleted"] != DBNull.Value ? TrackerAppt["is_deleted"].ToString().Trim() : "") ||
                                        (LocalAppt["is_asap"] != DBNull.Value ? LocalAppt["is_asap"].ToString().Trim() : "") != (TrackerAppt["is_asap"] != DBNull.Value ? TrackerAppt["is_asap"].ToString().Trim() : "")
                                        select TrackerAppt).ToList();
                DataTable dtPatientToBeUpdated = dtLocalAppointment.Clone();
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
                    var updateLocalDBID = from r1 in dtPatientToBeUpdated.AsEnumerable()
                                          join r2 in dtLocalAppointment.AsEnumerable()
                                          on r1["Appt_EHR_ID"].ToString().Trim() equals r2["Appt_EHR_ID"].ToString().Trim()
                                          select new { r1, r2 };
                    foreach (var x in updateLocalDBID)
                    {
                        x.r1.SetField("Appt_LocalDB_ID", x.r2["Appt_LocalDB_ID"].ToString().Trim());
                    }

                    //dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
                    var itemToUpdate2 = from TrackerAppt in dtPatientToBeUpdated.AsEnumerable()
                                        where
                                        (TrackerAppt["InsUptDlt"] != DBNull.Value ? TrackerAppt["InsUptDlt"].ToString().Trim() : "") != "1"
                                        && (
                                            (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "3" &&
                                            (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "5" &&
                                            (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "6" &&
                                            (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "7" &&
                                            (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "8" &&
                                            (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") != "9"
                                        )
                                        select new { TrackerAppt };
                    foreach (var x in itemToUpdate2)
                    {
                        x.TrackerAppt.SetField("InsUptDlt", "2");
                    }
                    var updateIsDeletedByStaus = from TrackerAppt in dtPatientToBeUpdated.AsEnumerable()
                                                 join LocalAppt in dtLocalAppointment.AsEnumerable()
                                                 on TrackerAppt["Appt_EHR_ID"].ToString().Trim() + "_" + TrackerAppt["Clinic_Number"].ToString().Trim()
                                                 equals LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                                 where (LocalAppt["is_deleted"] != DBNull.Value ? LocalAppt["is_deleted"].ToString().Trim().ToUpper() : "") == "FALSE"
                                                 && (
                                                 (TrackerAppt["InsUptDlt"] != DBNull.Value ? TrackerAppt["InsUptDlt"].ToString().Trim().ToUpper() : "") != "1"
                                                 && (
                                                       (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "3"
                                                       || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "5"
                                                       || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "6"
                                                       || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "7"
                                                       || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "8"
                                                       || (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim().ToUpper() : "") == "9"
                                                    )
                                                 )
                                                 select new { TrackerAppt };
                    foreach (var x in updateIsDeletedByStaus)
                    {
                        x.TrackerAppt.SetField("InsUptDlt", "3");
                    }

                    int intDelete = updateIsDeletedByStaus.Count();

                    //dtPatientToBeUpdated.AsEnumerable().Where(o => o.Field<object>("InsUptDlt").ToString() != "1" && (o.Field<object>("appointment_status_ehr_key").ToString() == "3" || o.Field<object>("appointment_status_ehr_key").ToString() == "5" || o.Field<object>("appointment_status_ehr_key").ToString() == "6" || o.Field<object>("appointment_status_ehr_key").ToString() == "7" || o.Field<object>("appointment_status_ehr_key").ToString() == "8" || o.Field<object>("appointment_status_ehr_key").ToString() == "9")).All(o => { o["InsUptDlt"] = "3"; return true; });
                    if (dtPatientToBeUpdated.Select("InsUptDlt = '2' or InsUptDlt = '3'").Length > 0)
                    {
                        dtSaveRecords.Load(dtPatientToBeUpdated.Select("InsUptDlt = '2' or InsUptDlt = '3'").CopyToDataTable().CreateDataReader());
                    }
                }

                bool status = true;
                if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                {
                    if (!dtSaveRecords.Columns.Contains("Appt_Web_ID"))
                    {
                        dtSaveRecords.Columns.Add("Appt_Web_ID");
                    }
                    dtSaveRecords.Rows[0]["Appt_Web_ID"] = strWebID;

                    var updateLanguageQuery = from r1 in dtSaveRecords.Select("InsUptDlt In ('2','3')").AsEnumerable()
                                              join r2 in dtLocalAppointment.AsEnumerable()
                                              on r1["Appt_EHR_ID"].ToString().Trim() + "_" + r1["Clinic_Number"].ToString().Trim()
                                              equals r2["Appt_EHR_ID"].ToString().Trim() + "_" + r2["Clinic_Number"].ToString().Trim()
                                              select new { r1, r2 };
                    foreach (var x in updateLanguageQuery)
                    {
                        x.r1.SetField("Is_Appt", x.r2.Field<string>("Is_Appt"));
                    }

                    status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Appointment", "Appt_LocalDB_ID,Appt_Web_ID", "Appt_LocalDB_ID");
                }

                if (status)
                {
                    Utility.WritetoAditEventSyncLogFile_Static("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    SynchDataLiveDB_Push_Appointment(strApptID);
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataTracker_AppointmentFromEvents Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public static void SynchDataTracker_AppointmentsPatientFromEvents(string strDbString, string Clinic_Number, string Service_Install_Id, string strPatID)
        {
            try
            {
                DataTable dtTrackerPatientList = SynchTrackerBAL.GetTrackerAppointmentsPatientData(strPatID);
                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Service_Install_Id, strPatID);

                DataTable dtSaveRecords = new DataTable();
                dtSaveRecords = dtTrackerPatientList.Clone();

                var itemsToBeAdded = (from TrackerPatient in dtTrackerPatientList.AsEnumerable()
                                      join LocalPatient in dtLocalPatient.AsEnumerable()
                                      on TrackerPatient["Patient_EHR_ID"].ToString().Trim() + "_" + TrackerPatient["Clinic_Number"].ToString().Trim()
                                      equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                      into matchingRows
                                      from matchingRow in matchingRows.DefaultIfEmpty()
                                      where matchingRow == null
                                      select TrackerPatient).ToList();
                DataTable dtPatientToBeAdded = dtLocalPatient.Clone();
                if (itemsToBeAdded.Count > 0)
                {
                    dtPatientToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
                }

                if (!dtPatientToBeAdded.Columns.Contains("InsUptDlt"))
                {
                    dtPatientToBeAdded.Columns.Add("InsUptDlt", typeof(int));
                    dtPatientToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
                }
                if (dtPatientToBeAdded.Rows.Count > 0)
                {
                    dtPatientToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
                    dtSaveRecords.Load(dtPatientToBeAdded.Select().CopyToDataTable().CreateDataReader());
                }

                var itemsToBeUpdated = (from TrackerPatient in dtTrackerPatientList.AsEnumerable()
                                        join LocalPatient in dtLocalPatient.AsEnumerable()
                                        on TrackerPatient["Patient_EHR_ID"].ToString().Trim() + "_" + TrackerPatient["Clinic_Number"].ToString().Trim()
                                        equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                        where
                                         (TrackerPatient["nextvisit_date"] != DBNull.Value && TrackerPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(TrackerPatient["nextvisit_date"]) : DateTime.Now)
                                         !=
                                         (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)

                                         ||

                                         (TrackerPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())

                                         ||

                                         (TrackerPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())

                                         || (TrackerPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                         || (TrackerPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                         || (Utility.ConvertContactNumber(TrackerPatient["Home_Phone"].ToString().Trim())) != (Utility.ConvertContactNumber(LocalPatient["Home_Phone"].ToString().Trim()))
                                         || (TrackerPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                         || (TrackerPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                         || (TrackerPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                         || (Utility.ConvertContactNumber(TrackerPatient["Mobile"].ToString().Trim())) != (Utility.ConvertContactNumber(LocalPatient["Mobile"].ToString().Trim()))
                                         || (TrackerPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
                                        //First_name, Last_name, Home_Phone, Middle_Name, Status, Email, Mobile, ReceiveSMS, PreferredLanguage
                                        select TrackerPatient).ToList();

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

                if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                {
                    bool isPatientSave = SynchTrackerBAL.Save_Tracker_Patient_To_Local_New(dtSaveRecords);
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
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataTracker_AppointmentsPatientFromEvents Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public static void SynchDataTracker_PatientStatusFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strPatID)
        {
            try
            {
                DataTable dtTrackerPatientStatus = SynchTrackerBAL.GetTrackerPatientStatusData(strPatID);
                if (dtTrackerPatientStatus != null && dtTrackerPatientStatus.Rows.Count > 0)
                {
                    SynchLocalBAL.UpdatePatient_Status(dtTrackerPatientStatus, Service_Install_Id, Clinic_Number, strPatID);
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataTracker_PatientStatus Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public void SynchDataLocalToTracker_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
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

                DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData("1", strPatientFormID);

                dtWebPatient_Form.Columns.Add("TableName", typeof(string));
                dtWebPatient_Form.Columns["TableName"].DefaultValue = "Contact";
                dtWebPatient_Form.Columns.Add("pformfield", typeof(string));

                if(dtWebPatient_Form!=null)
                {
                    if(dtWebPatient_Form.Rows.Count>0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                    }
                }

                foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                {

                    if (dtDtxRow["ehrfield"].ToString().Trim() == "first_name")
                    {
                        dtDtxRow["ehrfield"] = "FirstName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "last_name")
                    {
                        dtDtxRow["ehrfield"] = "LastName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "mobile")
                    {
                        dtDtxRow["ehrfield"] = "MobileNumber";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_one")
                    {
                        dtDtxRow["ehrfield"] = "Address1";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_two")
                    {
                        dtDtxRow["ehrfield"] = "Address2";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "birth_date")
                    {
                        dtDtxRow["ehrfield"] = "BirthDate";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "city")
                    {
                        dtDtxRow["ehrfield"] = "City";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "email")
                    {
                        dtDtxRow["ehrfield"] = "Email";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "home_phone")
                    {
                        dtDtxRow["ehrfield"] = "PhoneNumber";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "marital_status")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "middle_name")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "preferred_name")
                    {
                        dtDtxRow["ehrfield"] = "NickName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "pri_provider_id")
                    {
                        dtDtxRow["ehrfield"] = "ProviderId";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                    {
                        dtDtxRow["ehrfield"] = "PRIMARY_SUBSCRIBER_ID";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance_companyname")
                    {
                        dtDtxRow["ehrfield"] = "primary_insurance_companyname";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_email")
                    {
                        dtDtxRow["ehrfield"] = "HasEmailConsent";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_sms")
                    {
                        dtDtxRow["ehrfield"] = "IsSmsEnabled";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "salutation")
                    {
                        dtDtxRow["ehrfield"] = "Title";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "sec_provider_id")
                    {
                        dtDtxRow["ehrfield"] = "Provider2Id";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                    {
                        dtDtxRow["ehrfield"] = "SECONDARY_SUBSCRIBER_ID";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance_companyname")
                    {
                        dtDtxRow["ehrfield"] = "secondary_insurance_companyname";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "sex")
                    {
                        dtDtxRow["ehrfield"] = "sex";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "work_phone")
                    {
                        dtDtxRow["ehrfield"] = "WorkPhoneNumber";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "zipcode")
                    {
                        dtDtxRow["ehrfield"] = "PostalCode";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "ssn")
                    {
                        dtDtxRow["ehrfield"] = "IdentificationNumber";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "school")
                    {
                        dtDtxRow["ehrfield"] = "Company";
                        dtDtxRow["pformfield"] = "school";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim() == "employer")
                    {
                        dtDtxRow["ehrfield"] = "Company";
                        dtDtxRow["pformfield"] = "employer";
                    }

                    dtWebPatient_Form.AcceptChanges();

                }

                if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                {
                    bool Is_Record_Update = SynchTrackerBAL.Save_Patient_Form_Local_To_Tracker(dtWebPatient_Form);
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

                string Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed("1", Location_ID, strPatientFormID);
                if (Call_PatientPortalCompleted != "success")
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                }
                else
                {
                    ObjGoalBase.WriteToSyncLogFile("[Patient_Portal API called with Completed status : " + Call_PatientPortalCompleted);
                }

                bool isRecordSaved = false, isRecordDeleted = false;
                string Patient_EHR_IDS = "";
                string DeletePatientEHRID = "";
                string SavePatientEHRID = "";
                try
                {
                    if (SynchTrackerBAL.DeletePatientMedicationLocalToTracker(ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("[Delete_Patient_Medication to EHR Status : True");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication to EHR Status : False");
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication to EHR Error Log] : " + ex.Message);
                }
                try
                {
                    if (SynchTrackerBAL.SavePatientMedicationLocalToTracker(ref isRecordSaved, ref SavePatientEHRID, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("[Save_Patient_Medication to EHR Status : True");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Save_Patient_Medication to EHR Status : False");
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Save_Patient_Medication to EHR Error Log] : " + ex.Message);
                }
                if (isRecordSaved || isRecordDeleted)
                {
                    Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                    if (Patient_EHR_IDS != "")
                    {
                        SynchDataTracker_PatientMedication(Patient_EHR_IDS);
                    }
                }

                #region PatientInformation Document #endregion
                try
                {
                    GetPatientDocument("1", strPatientFormID);
                    GetPatientDocument_New("1", strPatientFormID);
                    SynchTrackerBAL.Save_Document_in_Tracker(strPatientFormID);
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
                #endregion

                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                if (appointmenetSyncFailedCounter < 10)
                {
                    appointmenetSyncFailedCounter++;
                }
                else
                {
                    if (ex.Message.ToString().Contains("OutOfMemoryException"))
                    {
                        System.Environment.Exit(1);
                    }
                }
            }

        }
        #endregion

    }
}
