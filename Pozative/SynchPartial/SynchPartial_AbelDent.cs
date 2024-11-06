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
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Data.SqlServerCe;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable



        bool IsAbelDentProviderSync = false;
        bool IsAbelDentOperatorySync = false;
        bool IsAbelDentApptTypeSync = false;
        bool IsAbelDentApptStatusSync = false;
        //bool Is_synched = false;
        //bool Is_synched_Provider = false;
        //bool Is_synched_Speciality = false;
        //bool Is_synched_Operatory = false;
        //bool Is_synched_OperatoryEvent = false;
        //bool Is_synched_Type = false;
        //bool Is_synched_Appointment = false;
        //bool Is_synched_RecallType = false;
        //bool Is_synched_ApptStatus = false;
        private BackgroundWorker bwSynchAbelDent_Appointment = null;
        private System.Timers.Timer timerSynchAbelDent_Appointment = null;

        private BackgroundWorker bwSynchAbelDent_OperatoryHours = null;
        private System.Timers.Timer timerSynchAbelDent_OperatoryHours = null;

        private BackgroundWorker bwSynchAbelDent_OperatoryEvent = null;
        private System.Timers.Timer timerSynchAbelDent_OperatoryEvent = null;

        private BackgroundWorker bwSynchAbelDent_Provider = null;
        private System.Timers.Timer timerSynchAbelDent_Provider = null;

        private BackgroundWorker bwSynchAbelDent_ProviderHours = null;
        private System.Timers.Timer timerSynchAbelDent_ProviderHours = null;

        private BackgroundWorker bwSynchAbelDent_Provider_OfficeHours = null;
        private System.Timers.Timer timerSynchAbelDent_Provider_OfficeHours = null;

        private BackgroundWorker bwSynchAbelDent_Operatory_OfficeHours = null;
        private System.Timers.Timer timerSynchAbelDent_Operatory_OfficeHours = null;

        private BackgroundWorker bwSynchAbelDent_User = null;
        private System.Timers.Timer timerSynchAbelDent_User = null;

        private BackgroundWorker bwSynchAbelDent_Speciality = null;
        private System.Timers.Timer timerSynchAbelDent_Speciality = null;

        private BackgroundWorker bwSynchAbelDent_Operatory = null;
        private System.Timers.Timer timerSynchAbelDent_Operatory = null;

        private BackgroundWorker bwSynchAbelDent_ApptType = null;
        private System.Timers.Timer timerSynchAbelDent_ApptType = null;

        private BackgroundWorker bwSynchAbelDent_Disease = null;
        private System.Timers.Timer timerSynchAbelDent_Disease = null;

        private BackgroundWorker bwSynchAbelDent_Patient = null;
        private System.Timers.Timer timerSynchAbelDent_Patient = null;

        private BackgroundWorker bwSynchAbelDent_Patient_New = null;
        private System.Timers.Timer timerSynchAbelDent_Patient_New = null;

        private BackgroundWorker bwSynchAbelDent_PatientBalance = null;
        private System.Timers.Timer timerSynchAbelDent_PatientBalance = null;

        private BackgroundWorker bwSynchAbelDent_RecallType = null;
        private System.Timers.Timer timerSynchAbelDent_RecallType = null;

        private BackgroundWorker bwSynchAbelDent_PatientRecall = null;
        private System.Timers.Timer timerSynchAbelDent_PatientRecall = null;

        private BackgroundWorker bwSynchAbelDent_ApptStatus = null;
        private System.Timers.Timer timerSynchAbelDent_ApptStatus = null;

        private BackgroundWorker bwSynchLocalToAbelDent_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchLocalToAbelDent_Appointment = null;

        private BackgroundWorker bwSynchAbelDent_Holiday = null;
        private System.Timers.Timer timerSynchAbelDent_Holiday = null;

        private BackgroundWorker bwSynchLocalToAbelDent_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToAbelDent_Patient_Form = null;

        private BackgroundWorker bwSynchAbelDent_MedicalHistory = null;
        private System.Timers.Timer timerSynchAbelDent_MedicalHistory = null;

        private BackgroundWorker bwSynchAbeldent_PatientPayment = null;
        private System.Timers.Timer timerSynchAbeldent_PatientPayment = null;
        #endregion

        #region OperatoryEvents Variable
        int _DaysToCheck = 14;
        int _MaximumOpenings = 6;
        int _MinimumOrphanedTimeUnits;
        int MaximumOpenings;
        int MinutesInUnit = 0;
        int _units = 1;
        static string _workingDay;
        char[] _days;
        static DateTime _dayEndTime;
        static DateTime _dayStartTime;
        static int _minutesInUnit;
        bool _isReservedTime = false;
        bool _isFreeBlock = true;
        #endregion
        private void CallSynchAbelDentToLocal()
        {
            if (Utility.AditSync)
            {                
                //fncSynchDataAbeldent_PatientPayment();
                SynchDataLiveDB_PatientPayment_LocalToAbelDent();

                if (Utility.ApptAutoBook)
                {
                    fncSynchDataLocalToAbelDent_Appointment();
                }

                SynchDataAbelDent_ApptType();
                fncSynchDataAbelDent_ApptType();

                SynchDataAbelDent_Provider();
                fncSynchDataAbelDent_Provider();

                SynchDataAbelDent_Operatory();
                fncSynchDataAbelDent_Operatory();

                //SynchDataAbelDent_Patient_New();
                //fncSynchDataAbelDent_Patient_New();

                SynchDataAbelDent_ApptStatus();
                fncSynchDataAbelDent_ApptStatus();

                //SynchDataAbelDent_Appointment();
                fncSynchDataAbelDent_Appointment();

                //SynchDataAbelDent_Patient();
                fncSynchDataAbelDent_Patient();

                SynchAbelDent_ProviderHours();
                fncSynchDataAbelDent_ProviderHours();

                SynchDataAbelDent_OperatoryHours();
                fncSynchDataAbelDent_OperatoryHours();

                SynchDataAbelDent_OperatoryEvent();
                fncSynchDataAbelDent_OperatoryEvent();

                SynchAbelDent_OperatoryOfficeHours();
                fncSynchDataAbelDent_Operatory_OfficeHours();

                SynchAbelDent_ProviderOfficeHours();
                fncSynchDataAbelDent_Provider_OfficeHours();

                //SynchDataAbelDent_MedicalHistory();
                fncSynchDataAbelDent_MedicalHistory();

                SynchDataAbelDent_Disease();
                fncSynchDataAbelDent_Disease();

                SynchDataAbelDent_Problems();

                SynchDataLocalToAbelDent_Patient_Form();
                fncSynchDataLocalToAbelDent_Patient_Form();

                SynchDataAbelDent_Speciality();
                fncSynchDataAbelDent_Speciality();

                SynchDataAbelDent_RecallType();
                fncSynchDataAbelDent_RecallType();

                SynchDataLiveDB_PatientSMSCallLog_LocalToAbelDent();

                //SynchDataAbelDent_User();
                fncSynchDataAbelDent_User();

            }
        }

        #region Local_Configuration
        public void Load_LocalData()
        {
            try
            {
                SynchAbelDentBAL.GetAbelDentSpecialityData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Synch Appointment

        private void fncSynchDataAbelDent_Appointment()
        {
            InitBgWorkerAbelDent_Appointment();
            InitBgTimerAbelDent_Appointment();
        }

        private void InitBgTimerAbelDent_Appointment()
        {
            timerSynchAbelDent_Appointment = new System.Timers.Timer();
            this.timerSynchAbelDent_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchAbelDent_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Appointment_Tick);
            timerSynchAbelDent_Appointment.Enabled = true;
            timerSynchAbelDent_Appointment.Start();
            timerSynchAbelDent_Appointment_Tick(null, null);
        }

        private void InitBgWorkerAbelDent_Appointment()
        {
            bwSynchAbelDent_Appointment = new BackgroundWorker();
            bwSynchAbelDent_Appointment.WorkerReportsProgress = true;
            bwSynchAbelDent_Appointment.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Appointment.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchAbelDent_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Appointment_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Appointment.Enabled = false;
                MethodForCallSynchOrderAbelDent_Appointment();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Appointment()
        {
            System.Threading.Thread procThreadmainAbelDent_Appointment = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Appointment);
            procThreadmainAbelDent_Appointment.Start();
        }

        public void CallSyncOrderTableAbelDent_Appointment()
        {
            if (bwSynchAbelDent_Appointment.IsBusy != true)
            {
                bwSynchAbelDent_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Appointment.Enabled = true;
        }

        public void SynchDataAbelDent_Appointment()
        {

            if (IsAbelDentProviderSync && IsAbelDentOperatorySync && IsAbelDentApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    if (IsAbelDentProviderSync && IsAbelDentOperatorySync && IsAbelDentApptTypeSync)
                    {
                        SynchAbelDentBAL.GetAbelDentSystemData(ref _dayStartTime, ref _dayEndTime, ref _minutesInUnit, ref _workingDay);

                        //SynchDataAbelDent_AppointmentsPatient();                        
                        SynchDataAbelDent_PatientStatus();
                        DataTable dtAbelDentAppointment = SynchAbelDentBAL.GetAbelDentAppointmentData("", _minutesInUnit.ToString());

                        dtAbelDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                        dtAbelDentAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                        dtAbelDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                        dtAbelDentAppointment.Columns.Add("ProcedureCode", typeof(string));


                        dtAbelDentAppointment.Columns["ProcedureCode"].DefaultValue = "";
                        dtAbelDentAppointment.Columns["ProcedureDesc"].DefaultValue = "";


                       

                        try
                        {
                            DataTable dtAbelDentApptStatus = SynchAbelDentBAL.GetAbelDentApptTypeData();

                            var joinedRows = from dtDtxRow in dtAbelDentAppointment.AsEnumerable()
                                             join statusRow in dtAbelDentApptStatus.AsEnumerable()
                                             on dtDtxRow["WorkToDo"].ToString().ToLower().Trim() equals statusRow["Type_Name"].ToString().ToLower().Trim()
                                             select new
                                             {
                                                 DtxRow = dtDtxRow,
                                                 StatusAppointmentId = statusRow["ApptType_EHR_ID"].ToString(),
                                                 StatusType = statusRow["Type_Name"].ToString()
                                             };

                            foreach (var row in joinedRows)
                            {
                                row.DtxRow["ApptType_EHR_ID"] = row.StatusAppointmentId;
                                row.DtxRow["ApptType"] = row.StatusType;
                            }
                        }
                        catch (Exception ex)
                        {
                            Utility.WriteToErrorLogFromAll("Mapped Appt Types with appointment Error: " + ex.Message);
                        }

                        DataTable DtAbelDentAppointment_Procedures_Data = SynchAbelDentBAL.GetAbelDentAppointment_Procedures_Data();
                        string ProcedureDesc = "";
                        string ProcedureCode = "";

                        foreach (DataRow dtDtxRow in dtAbelDentAppointment.Rows)
                        {
                            ProcedureDesc = "";
                            ProcedureCode = "";
                            DataRow[] dtCurApptProcedure = DtAbelDentAppointment_Procedures_Data.Select("Appointment_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");
                            foreach (var dtSinProc in dtCurApptProcedure.ToList())
                            {
                                ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                                ProcedureDesc = ProcedureDesc + dtSinProc["ProcedureDesc"].ToString().Trim();
                            }
                            dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                            dtDtxRow["ProcedureCode"] = ProcedureCode;

                        }

                        DataTable dtLocalAppointment = new DataTable();
                        DataTable dtTempResult = SynchLocalBAL.GetLocalAppointmentData("1");
                        if (dtTempResult.Rows.Count > 0)
                        {
                            try
                            {
                                //dtLocalAppointment = dtTempResult.Clone();
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
                        var itemsToBeAdded = (from AbelDentAppt in dtAbelDentAppointment.AsEnumerable()
                                              join LocalAppt in dtLocalAppointment.AsEnumerable()
                                              on AbelDentAppt["Appt_EHR_ID"].ToString().Trim() + "_" + AbelDentAppt["Clinic_Number"].ToString().Trim()
                                              equals LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                              into matchingRows
                                              from matchingRow in matchingRows.DefaultIfEmpty()
                                              where matchingRow == null
                                              select AbelDentAppt).ToList();
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
                                               join AbelDentAppt in dtAbelDentAppointment.AsEnumerable()
                                               on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                               equals AbelDentAppt["Appt_EHR_ID"].ToString().Trim() + "_" + AbelDentAppt["Clinic_Number"].ToString().Trim()
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
                                                join AbelDentAppt in dtAbelDentAppointment.AsEnumerable()
                                                on LocalAppt["Appt_EHR_ID"].ToString().Trim() + "_" + LocalAppt["Clinic_Number"].ToString().Trim()
                                                equals AbelDentAppt["Appt_EHR_ID"].ToString().Trim() + "_" + AbelDentAppt["Clinic_Number"].ToString().Trim()
                                                where
                                                (LocalAppt["First_name"] != DBNull.Value ? LocalAppt["First_name"].ToString().Trim() : "") != (AbelDentAppt["First_name"] != DBNull.Value ? AbelDentAppt["First_name"].ToString().Trim() : "") ||
                                                (LocalAppt["Last_name"] != DBNull.Value ? LocalAppt["Last_name"].ToString().Trim() : "") != (AbelDentAppt["Last_name"] != DBNull.Value ? AbelDentAppt["Last_name"].ToString().Trim() : "") ||
                                                (LocalAppt["MI"] != DBNull.Value ? LocalAppt["MI"].ToString().Trim() : "") != (AbelDentAppt["MI"] != DBNull.Value ? AbelDentAppt["MI"].ToString().Trim() : "") ||
                                                (LocalAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Home_Contact"].ToString().Trim()) : "") != (AbelDentAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(AbelDentAppt["Home_Contact"].ToString().Trim()) : "") ||
                                                (LocalAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Mobile_Contact"].ToString().Trim()) : "") != (AbelDentAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(AbelDentAppt["Mobile_Contact"].ToString().Trim()) : "") ||
                                                (LocalAppt["Email"] != DBNull.Value ? LocalAppt["Email"].ToString().Trim() : "") != (AbelDentAppt["Email"] != DBNull.Value ? AbelDentAppt["Email"].ToString().Trim() : "") ||
                                                (LocalAppt["Address"] != DBNull.Value ? LocalAppt["Address"].ToString().Trim() : "") != (AbelDentAppt["Address"] != DBNull.Value ? AbelDentAppt["Address"].ToString().Trim() : "") ||
                                                (LocalAppt["City"] != DBNull.Value ? LocalAppt["City"].ToString().Trim() : "") != (AbelDentAppt["City"] != DBNull.Value ? AbelDentAppt["City"].ToString().Trim() : "") ||
                                                (LocalAppt["Zip"] != DBNull.Value ? LocalAppt["Zip"].ToString().Trim() : "") != (AbelDentAppt["Zip"] != DBNull.Value ? AbelDentAppt["Zip"].ToString().Trim() : "") ||
                                                (LocalAppt["Operatory_EHR_ID"] != DBNull.Value ? LocalAppt["Operatory_EHR_ID"].ToString().Trim() : "") != (AbelDentAppt["Operatory_EHR_ID"] != DBNull.Value ? AbelDentAppt["Operatory_EHR_ID"].ToString().Trim() : "") ||
                                                (LocalAppt["Operatory_Name"] != DBNull.Value ? LocalAppt["Operatory_Name"].ToString().Trim() : "") != (AbelDentAppt["Operatory_Name"] != DBNull.Value ? AbelDentAppt["Operatory_Name"].ToString().Trim() : "") ||
                                                (LocalAppt["Provider_EHR_ID"] != DBNull.Value ? LocalAppt["Provider_EHR_ID"].ToString().Trim() : "") != (AbelDentAppt["Provider_EHR_ID"] != DBNull.Value ? AbelDentAppt["Provider_EHR_ID"].ToString().Trim() : "") ||
                                                (LocalAppt["Provider_Name"] != DBNull.Value ? LocalAppt["Provider_Name"].ToString().Trim() : "") != (AbelDentAppt["Provider_Name"] != DBNull.Value ? AbelDentAppt["Provider_Name"].ToString().Trim() : "") ||
                                                (LocalAppt["Comment"] != DBNull.Value ? LocalAppt["Comment"].ToString().Trim() : "") != (AbelDentAppt["Comment"] != DBNull.Value ? AbelDentAppt["Comment"].ToString().Trim() : "") ||
                                                (LocalAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["birth_date"].ToString().Trim()) : "") != (AbelDentAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(AbelDentAppt["birth_date"].ToString().Trim()) : "") ||
                                                (LocalAppt["ApptType_EHR_ID"] != DBNull.Value ? LocalAppt["ApptType_EHR_ID"].ToString().Trim() : "") != (AbelDentAppt["ApptType_EHR_ID"] != DBNull.Value ? AbelDentAppt["ApptType_EHR_ID"].ToString().Trim() : "") ||
                                                (LocalAppt["ApptType"] != DBNull.Value ? LocalAppt["ApptType"].ToString().Trim() : "") != (AbelDentAppt["ApptType"] != DBNull.Value ? AbelDentAppt["ApptType"].ToString().Trim() : "") ||
                                                (LocalAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_DateTime"].ToString().Trim()) : "") != (AbelDentAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(AbelDentAppt["Appt_DateTime"].ToString().Trim()) : "") ||
                                                (LocalAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_EndDateTime"].ToString().Trim()) : "") != (AbelDentAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(AbelDentAppt["Appt_EndDateTime"].ToString().Trim()) : "") ||
                                                (LocalAppt["Status"] != DBNull.Value ? LocalAppt["Status"].ToString().Trim() : "") != (AbelDentAppt["Status"] != DBNull.Value ? AbelDentAppt["Status"].ToString().Trim() : "") ||
                                                (LocalAppt["appointment_status_ehr_key"] != DBNull.Value ? LocalAppt["appointment_status_ehr_key"].ToString().Trim() : "") != (AbelDentAppt["appointment_status_ehr_key"] != DBNull.Value ? AbelDentAppt["appointment_status_ehr_key"].ToString().Trim() : "") ||
                                                (LocalAppt["Appointment_Status"] != DBNull.Value ? LocalAppt["Appointment_Status"].ToString().Trim() : "") != (AbelDentAppt["Appointment_Status"] != DBNull.Value ? AbelDentAppt["Appointment_Status"].ToString().Trim() : "") ||
                                                (LocalAppt["confirmed_status_ehr_key"] != DBNull.Value ? LocalAppt["confirmed_status_ehr_key"].ToString().Trim() : "") != (AbelDentAppt["confirmed_status_ehr_key"] != DBNull.Value ? AbelDentAppt["confirmed_status_ehr_key"].ToString().Trim() : "") ||
                                                (LocalAppt["confirmed_status"] != DBNull.Value ? LocalAppt["confirmed_status"].ToString().Trim() : "") != (AbelDentAppt["confirmed_status"] != DBNull.Value ? AbelDentAppt["confirmed_status"].ToString().Trim() : "") ||
                                                (LocalAppt["patient_ehr_id"] != DBNull.Value ? LocalAppt["patient_ehr_id"].ToString().Trim() : "") != (AbelDentAppt["patient_ehr_id"] != DBNull.Value ? AbelDentAppt["patient_ehr_id"].ToString().Trim() : "") ||
                                                (LocalAppt["ProcedureDesc"] != DBNull.Value ? LocalAppt["ProcedureDesc"].ToString().Trim() : "") != (AbelDentAppt["ProcedureDesc"] != DBNull.Value ? AbelDentAppt["ProcedureDesc"].ToString().Trim() : "") ||
                                                (LocalAppt["ProcedureCode"] != DBNull.Value ? LocalAppt["ProcedureCode"].ToString().Trim() : "") != (AbelDentAppt["ProcedureCode"] != DBNull.Value ? AbelDentAppt["ProcedureCode"].ToString().Trim() : "") ||
                                                (LocalAppt["is_asap"] != DBNull.Value ? LocalAppt["is_asap"].ToString().Trim() : "") != (AbelDentAppt["is_asap"] != DBNull.Value ? AbelDentAppt["is_asap"].ToString().Trim() : "")
                                                select AbelDentAppt).ToList();
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
                            dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
                            dtSaveRecords.Load(dtPatientToBeUpdated.Select().CopyToDataTable().CreateDataReader());
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

                            if (dtPatientToBeUpdated.Select("InsUptDlt = '2' or InsUptDlt = '3'").Length > 0)
                            {
                                dtSaveRecords.Load(dtPatientToBeUpdated.Select("InsUptDlt = '2' or InsUptDlt = '3'").CopyToDataTable().CreateDataReader());
                            }
                        }

                        bool status = true;
                        if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                        {
                            string Appt_EHR_Ids = string.Join("','", dtSaveRecords.AsEnumerable().Where(o => o.Field<object>("InsUptDlt").ToString() == "2").Select(p => p.Field<object>("Appt_EHR_Id").ToString()));
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "Appointment", "Appt_LocalDB_ID,Appt_Web_ID,WorkToDo", "Appt_LocalDB_ID");
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

        #region Create Appointment

        private void fncSynchDataLocalToAbelDent_Appointment()
        {
            InitBgWorkerLocalToAbelDent_Appointment();
            InitBgTimerLocalToAbelDent_Appointment();
        }

        private void InitBgTimerLocalToAbelDent_Appointment()
        {
            timerSynchLocalToAbelDent_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToAbelDent_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToAbelDent_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToAbelDent_Appointment_Tick);
            timerSynchLocalToAbelDent_Appointment.Enabled = true;
            timerSynchLocalToAbelDent_Appointment.Start();
            timerSynchLocalToAbelDent_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToAbelDent_Appointment()
        {
            bwSynchLocalToAbelDent_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToAbelDent_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToAbelDent_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToAbelDent_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToAbelDent_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToAbelDent_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToAbelDent_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToAbelDent_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToAbelDent_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToAbelDent_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToAbelDent_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToAbelDent_Appointment);
            procThreadmainLocalToAbelDent_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToAbelDent_Appointment()
        {
            if (bwSynchLocalToAbelDent_Appointment.IsBusy != true)
            {
                bwSynchLocalToAbelDent_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToAbelDent_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToAbelDent_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToAbelDent_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToAbelDent_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToAbelDent_Appointment.Enabled = true;
        }

        public void SynchDataLocalToAbelDent_Appointment()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData("1");
                    DataTable dtAbelDentPatient = SynchAbelDentBAL.GetAbelDentPatientListData();

                    SynchAbelDentBAL.GetAbelDentSystemData(ref _dayStartTime, ref _dayEndTime, ref _minutesInUnit, ref _workingDay);

                    int reqTime = 0;
                    Int64 tmpPatient_id = 0;
                    string tmpApptProv = "";
                    string PatientUniqID = string.Empty;
                    string tmpAppt_EHR_id = "";
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
                        }
                    }

                    foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                    {
                        tmpPatient_id = 0;
                        PatientUniqID = "";
                        tmpAppt_EHR_id = "";
                        TmpWebPatientName = "";
                        TmpWebRevPatientName = "";

                        //TmpWebPatientName = dtDtxRow["First_Name"].ToString().Trim();
                        //TmpWebRevPatientName = dtDtxRow["Last_Name"].ToString().Trim();

                        Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);

                        tmpApptProv = dtDtxRow["Provider_EHR_ID"].ToString().Trim();

                        #region Set Operatory
                        string Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim();
                        //string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');

                        DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                        DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                        TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;

                        int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);
                        reqTime = (tmpApptDurMinutes / _minutesInUnit);

                        DataTable dtBookOperatoryApptWiseDateTime = SynchAbelDentBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), _minutesInUnit);
                        string tmpIdealOperatory = "";
                        string appointment_EHR_id = "";

                        if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
                        {
                            string tmpCheckOpId = "";
                            bool IsConflict = false;
                            for (int i = 0; i < Operatory_EHR_IDs.Length; i++)
                            {
                                IsConflict = false;
                                tmpCheckOpId = Operatory_EHR_IDs[i].ToString();
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
                        #endregion

                        #region Set Patient
                        if (tmpIdealOperatory == "")
                        {
                            DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appointment_EHR_Id = '"+ appointment_EHR_id.ToString() + "'").CopyToDataTable();
                            bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());
                        }

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
                            tmpPatient_id = Convert.ToInt32(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtAbelDentPatient, PatientUniqID.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DBConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                        }
                        #endregion

                        TmpWebPatientName = SynchAbelDentBAL.GetPatientName(tmpPatient_id.ToString());
                        PatientUniqID = tmpPatient_id.ToString();


                        if (PatientUniqID != string.Empty || PatientUniqID != "" || tmpPatient_id != 0)
                        {
                            tmpAppt_EHR_id = SynchAbelDentBAL.Save_Appointment_Local_To_AbelDent(TmpWebPatientName, tmpStartTime, tmpEndTime, tmpPatient_id, PatientUniqID, Operatory_EHR_IDs, reqTime, "1", dtDtxRow["ApptType"].ToString().Trim(),Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), tmpApptProv, "0", false, false, false, false, dtDtxRow["appt_treatmentcode"].ToString(), (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "1"));

                            if (tmpAppt_EHR_id != "")
                            {
                                bool isApptId_Update = SynchAbelDentBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
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

        #region Synch Provider

        private void fncSynchDataAbelDent_Provider()
        {
            InitBgWorkerAbelDent_Provider();
            InitBgTimerAbelDent_Provider();
        }

        private void InitBgTimerAbelDent_Provider()
        {
            timerSynchAbelDent_Provider = new System.Timers.Timer();
            this.timerSynchAbelDent_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchAbelDent_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Provider_Tick);
            timerSynchAbelDent_Provider.Enabled = true;
            timerSynchAbelDent_Provider.Start();
        }

        private void InitBgWorkerAbelDent_Provider()
        {
            bwSynchAbelDent_Provider = new BackgroundWorker();
            bwSynchAbelDent_Provider.WorkerReportsProgress = true;
            bwSynchAbelDent_Provider.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Provider.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchAbelDent_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Provider_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Provider.Enabled = false;
                MethodForCallSynchOrderAbelDent_Provider();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Provider()
        {
            System.Threading.Thread procThreadmainAbelDent_Provider = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Provider);
            procThreadmainAbelDent_Provider.Start();
        }

        public void CallSyncOrderTableAbelDent_Provider()
        {
            if (bwSynchAbelDent_Provider.IsBusy != true)
            {
                bwSynchAbelDent_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SyncPullLogsAndSaveinAbelDent();
                SynchDataAbelDent_Provider();
                CommonFunction.GetMasterSync();

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SyncPullLogsAndSaveinAbelDent()
        {
            try
            {
                CheckCustomhoursForProviderOperatory();
                SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                SynchDataLiveDB_Pull_PatientFollowUp();
                SynchDataLiveDB_PatientSMSCallLog_LocalToAbelDent();
                fncPaymentSMSCallStatusUpdate();
                SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
            }
            catch (Exception)
            {

            }
        }


        private void bwSynchAbelDent_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Provider.Enabled = true;
        }

        public void SynchDataAbelDent_Provider()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtAbelDentProvider = SynchAbelDentBAL.GetAbelDentProviderData();
                    dtAbelDentProvider.Columns.Add("InsUptDlt", typeof(int));
                    dtAbelDentProvider.Columns["InsUptDlt"].DefaultValue = 0;
                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", "1");

                    if (!dtLocalProvider.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalProvider.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalProvider.Columns["InsUptDlt"].DefaultValue = 0;
                    }
                    dtLocalProvider = CompareDataTableRecords(ref dtAbelDentProvider, dtLocalProvider, "Provider_EHR_ID", "Provider_LocalDB_ID", "Provider_LocalDB_ID,Provider_EHR_ID,Provider_Web_ID,image,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    //dtLocalProvider.AsEnumerable().Where(o => string.IsNullOrEmpty(o.Field<object>("InsUptDlt").ToString()) == false && o.Field<object>("InsUptDlt").ToString() == "3").Count() > 0
                    if (dtLocalProvider.Select("InsUptDlt=3").Count() > 0)
                    {
                        dtAbelDentProvider.Load(dtLocalProvider.Select("InsUptDlt=3").CopyToDataTable().CreateDataReader());
                    }

                    dtAbelDentProvider.AcceptChanges();

                    if (dtAbelDentProvider != null && dtAbelDentProvider.Rows.Count > 0 && dtAbelDentProvider.AsEnumerable()
                        .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
                    {
                        DataTable dtSaveRecords = dtAbelDentProvider.Clone();
                        bool status = false;
                        if (dtAbelDentProvider.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtAbelDentProvider.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "Providers", "Provider_LocalDB_ID,Provider_Web_ID", "Provider_LocalDB_ID");
                        }
                        else
                        {
                            if (dtAbelDentProvider.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                            ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsAbelDentProviderSync = true;
                            SynchDataLiveDB_Push_Provider();
                        }
                        else
                        {
                            IsAbelDentProviderSync = false;
                        }
                    }


                    IsProviderSyncedFirstTime = true;
                    //SynchAbelDent_ProviderHours();                    
                }
                catch (Exception ex)
                {
                    IsAbelDentProviderSync = false;
                    IsProviderSyncedFirstTime = true;
                    ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database)]" + ex.Message);
                }
            }
            SynchDataAbelDent_Medication();
        }

        #endregion

        #region Synch ProviderHours

        private void fncSynchDataAbelDent_ProviderHours()
        {
            InitBgWorkerAbelDent_ProviderHours();
            InitBgTimerAbelDent_ProviderHours();
        }

        private void InitBgTimerAbelDent_ProviderHours()
        {
            timerSynchAbelDent_ProviderHours = new System.Timers.Timer();
            this.timerSynchAbelDent_ProviderHours.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchAbelDent_ProviderHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_ProviderHours_Tick);
            timerSynchAbelDent_ProviderHours.Enabled = true;
            timerSynchAbelDent_ProviderHours.Start();
        }

        private void InitBgWorkerAbelDent_ProviderHours()
        {
            bwSynchAbelDent_ProviderHours = new BackgroundWorker();
            bwSynchAbelDent_ProviderHours.WorkerReportsProgress = true;
            bwSynchAbelDent_ProviderHours.WorkerSupportsCancellation = true;
            bwSynchAbelDent_ProviderHours.DoWork += new DoWorkEventHandler(bwSynchAbelDent_ProviderHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchAbelDent_ProviderHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_ProviderHours_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_ProviderHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_ProviderHours.Enabled = false;
                MethodForCallSynchOrderAbelDent_ProviderHours();
            }
        }

        public void MethodForCallSynchOrderAbelDent_ProviderHours()
        {
            System.Threading.Thread procThreadmainAbelDent_ProviderHours = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_ProviderHours);
            procThreadmainAbelDent_ProviderHours.Start();
        }

        public void CallSyncOrderTableAbelDent_ProviderHours()
        {
            if (bwSynchAbelDent_ProviderHours.IsBusy != true)
            {
                bwSynchAbelDent_ProviderHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_ProviderHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_ProviderHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchAbelDent_ProviderHours();                
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_ProviderHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_ProviderHours.Enabled = true;
        }

        public void SynchAbelDent_ProviderHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {

                    SynchAbelDentBAL.GetAbelDentSystemData(ref _dayStartTime, ref _dayEndTime, ref _minutesInUnit, ref _workingDay);
                    List<String> columnList = SynchAbelDentBAL.GetAbelDentActiveColumnData();

                    #region Variables
                    _isFreeBlock = true;
                    bool status = false;
                    _days = _workingDay.ToCharArray();
                    int NumberOfOpenings = 20000;
                    int _unitNumbersPerDay = (int)(_dayEndTime - _dayStartTime).TotalMinutes / _minutesInUnit;
                    List<AbelOpenings> Hours = new List<AbelOpenings>();
                    #endregion

                    string fixedStartTime = _dayStartTime.ToString("HH:mm");//"07:45";
                    string fixedEndTime = _dayEndTime.ToString("HH:mm"); //"21:00";
                    string fixedStartDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string fixedEndDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
                    DateTime StartDate = DateTime.Parse(fixedStartDate + " " + fixedStartTime);
                    DateTime EndDate = Convert.ToDateTime("" + fixedEndDate + " " + fixedEndTime + "");

                    DataTable dtDeleteLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData("1");

                    if (dtDeleteLocalProviderHours.Rows.Count > 0)
                    {
                        SynchLocalBAL.DeleteAbeldentLocalProviderHoursData("1");
                        SynchDataLiveDB_Push_ProviderHours();
                        SynchLocalBAL.DeleteAbeldentLocalProviderHoursAll();
                    }

                    for (var day = StartDate.Date; day.Date <= EndDate.Date; day = day.AddDays(1))
                    {
                        fixedStartDate = day.ToString("yyyy-MM-dd");
                        fixedEndDate = day.ToString("yyyy-MM-dd");
                        DateTime StartDate1 = DateTime.Parse(fixedStartDate + " " + fixedStartTime);
                        DateTime EndDate1 = DateTime.Parse(fixedEndDate + " " + fixedEndTime);

                        Hours = SynchAbelDentBAL.GetAbelDentOpeingsList(_units, StartDate1, EndDate1, columnList, _workingDay, false, "", NumberOfOpenings, _unitNumbersPerDay, _dayStartTime, _dayEndTime, _minutesInUnit, _isFreeBlock);
                        DataTable dtLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData("1");

                        if (!dtLocalProviderHours.Columns.Contains("InsUptDlt"))
                        {
                            dtLocalProviderHours.Columns.Add("InsUptDlt", typeof(int));
                            dtLocalProviderHours.Columns["InsUptDlt"].DefaultValue = 1;
                        }

                        DataTable createNewDt = new DataTable();

                        if (Hours.Count > 0)
                        {
                            createNewDt.Columns.Add("PH_LocalDB_ID", typeof(int));
                            createNewDt.Columns["PH_LocalDB_ID"].DefaultValue = 0;
                            createNewDt.Columns.Add("PH_EHR_ID", typeof(string));
                            createNewDt.Columns.Add("PH_Web_ID", typeof(string));
                            createNewDt.Columns["PH_Web_ID"].DefaultValue = "";
                            createNewDt.Columns.Add("Provider_EHR_ID", typeof(string));
                            createNewDt.Columns.Add("Operatory_EHR_ID", typeof(string));
                            createNewDt.Columns.Add("StartTime", typeof(string));
                            createNewDt.Columns.Add("EndTime", typeof(string));
                            createNewDt.Columns.Add("comment", typeof(string));
                            createNewDt.Columns.Add("Entry_DateTime", typeof(string));
                            createNewDt.Columns.Add("Last_Sync_Date", typeof(string));
                            createNewDt.Columns["Last_Sync_Date"].DefaultValue = "";
                            createNewDt.Columns.Add("is_deleted", typeof(string));
                            createNewDt.Columns["is_deleted"].DefaultValue = "False";
                            createNewDt.Columns.Add("Is_Adit_Updated", typeof(int));
                            createNewDt.Columns["Is_Adit_Updated"].DefaultValue = 0;
                            createNewDt.Columns.Add("InsUptDlt", typeof(int));
                            createNewDt.Columns["InsUptDlt"].DefaultValue = 1;

                            for (int i = 0; i < Hours.Count; i++)
                            {
                                DataRow dataRow;

                                dataRow = createNewDt.NewRow();
                                dataRow["PH_EHR_ID"] = Hours[i].ColumnID + "_" + Hours[i].Time.ToString("yyyy-MM-dd-HH-mm").Replace('-', '_');
                                dataRow["Provider_EHR_ID"] = Hours[i].Provider;
                                dataRow["Operatory_EHR_ID"] = Hours[i].ColumnID;

                                if (Hours[i].StartTimeList.Count > 0)
                                {
                                    if (Hours[i].StartTimeList[0].StartTimeRecords.Count > 0)
                                    {
                                        int endtimeCount = Hours[i].StartTimeList[0].StartTimeRecords.Count;
                                        dataRow["StartTime"] = Hours[i].StartTimeList[0].StartTimeRecords[0].Time.ToString();
                                        dataRow["EndTime"] = Hours[i].StartTimeList[0].StartTimeRecords[endtimeCount - 1].Time.AddMinutes(_minutesInUnit).ToString();
                                    }
                                    else
                                    {
                                        if (Hours[i].AvailableTime.Contains("0hr"+ _minutesInUnit + ""))
                                        {
                                            dataRow["StartTime"] = Hours[i].Time.ToString();
                                            dataRow["EndTime"] = Hours[i].Time.AddMinutes(_minutesInUnit).ToString();
                                        }
                                        else
                                        {
                                            dataRow["StartTime"] = "";
                                            dataRow["EndTime"] = "";
                                        }
                                    }
                                    dataRow["Entry_DateTime"] = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
                                    createNewDt.Rows.Add(dataRow);
                                }
                                else
                                    continue;
                            }

                            createNewDt.AcceptChanges();
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(createNewDt, "ProviderHours", "PH_LocalDB_ID,PH_Web_ID", "PH_LocalDB_ID");                            
                        }
                    }

                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
                        ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchDataLiveDB_Push_ProviderHours();
                    }
                    IsEHRAllSync = true;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderHours Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        #endregion

        #region ProviderOfficeHours
        private void fncSynchDataAbelDent_Provider_OfficeHours()
        {
            InitBgWorkerAbelDent_Provider_OfficeHours();
            InitBgTimerAbelDent_Provider_OfficeHours();
        }

        private void InitBgTimerAbelDent_Provider_OfficeHours()
        {
            timerSynchAbelDent_Provider_OfficeHours = new System.Timers.Timer();
            this.timerSynchAbelDent_Provider_OfficeHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider_OfficeHours;
            this.timerSynchAbelDent_Provider_OfficeHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Provider_OfficeHours_Tick);
            timerSynchAbelDent_Provider_OfficeHours.Enabled = true;
            timerSynchAbelDent_Provider_OfficeHours.Start();
        }
        private void InitBgWorkerAbelDent_Provider_OfficeHours()
        {
            bwSynchAbelDent_Provider_OfficeHours = new BackgroundWorker();
            bwSynchAbelDent_Provider_OfficeHours.WorkerReportsProgress = true;
            bwSynchAbelDent_Provider_OfficeHours.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Provider_OfficeHours.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Provider_OfficeHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchAbelDent_Provider_OfficeHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Provider_OfficeHours_RunWorkerCompleted);
        }
        private void timerSynchAbelDent_Provider_OfficeHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Provider_OfficeHours.Enabled = false;
                MethodForCallSynchOrderAbelDent_Provider_OfficeHours();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Provider_OfficeHours()
        {
            System.Threading.Thread procThreadmainAbelDent_Provider_OfficeHours = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Provider_OfficeHours);
            procThreadmainAbelDent_Provider_OfficeHours.Start();
        }

        public void CallSyncOrderTableAbelDent_Provider_OfficeHours()
        {
            if (bwSynchAbelDent_Provider_OfficeHours.IsBusy != true)
            {
                bwSynchAbelDent_Provider_OfficeHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Provider_OfficeHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Provider_OfficeHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchAbelDent_ProviderOfficeHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }
        private void bwSynchAbelDent_Provider_OfficeHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Provider_OfficeHours.Enabled = true;
        }
        public void SynchAbelDent_ProviderOfficeHours()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtProviderOfficeHours = SynchAbelDentBAL.GetAbelDentProviderOfficeHours();
                    DataTable dtProviderOfficeHoursOP = SynchAbelDentBAL.GetAbelDentProviderOfficeHoursOP();
                    DataTable dtAbelDentLocalProviderOfficeHours = SynchLocalBAL.GetLocalProviderOfficeHours("1");

                    if (!dtAbelDentLocalProviderOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtAbelDentLocalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtAbelDentLocalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    DataTable dtOfficeHours = new DataTable();

                    dtOfficeHours.Columns.Add("POH_LocalDB_ID", typeof(int));
                    dtOfficeHours.Columns["POH_LocalDB_ID"].DefaultValue = 0;
                    dtOfficeHours.Columns.Add("POH_EHR_ID", typeof(string));
                    dtOfficeHours.Columns.Add("POH_Web_ID", typeof(string));
                    dtOfficeHours.Columns["POH_Web_ID"].DefaultValue = "";
                    dtOfficeHours.Columns.Add("Provider_EHR_ID", typeof(string));
                    dtOfficeHours.Columns.Add("WeekDay", typeof(string));
                    dtOfficeHours.Columns.Add("StartTime1", typeof(string));
                    dtOfficeHours.Columns["StartTime1"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("EndTime1", typeof(string));
                    dtOfficeHours.Columns["EndTime1"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("StartTime2", typeof(string));
                    dtOfficeHours.Columns["StartTime2"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("EndTime2", typeof(string));
                    dtOfficeHours.Columns["EndTime2"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("StartTime3", typeof(string));
                    dtOfficeHours.Columns["StartTime3"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("EndTime3", typeof(string));
                    dtOfficeHours.Columns["EndTime3"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("Entry_DateTime", typeof(string));
                    dtOfficeHours.Columns.Add("Last_Sync_Date", typeof(string));
                    dtOfficeHours.Columns["Last_Sync_Date"].DefaultValue = "";
                    dtOfficeHours.Columns.Add("is_deleted", typeof(string));
                    dtOfficeHours.Columns["is_deleted"].DefaultValue = 0;
                    dtOfficeHours.Columns.Add("Is_Adit_Updated", typeof(int));
                    dtOfficeHours.Columns["Is_Adit_Updated"].DefaultValue = 0;
                    dtOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;


                    foreach (DataRow providerRow in dtProviderOfficeHoursOP.Rows)
                    {
                        string ProviderEHRID = providerRow["Provider_EHR_ID"].ToString();

                        foreach (DataRow officeHoursRow in dtProviderOfficeHours.Rows)
                        {
                            // Create a new row for the combined DataTable
                            DataRow newRow = dtOfficeHours.NewRow();
                            newRow["POH_EHR_ID"] = officeHoursRow["POH_EHR_ID"] + "_" + ProviderEHRID;
                            newRow["Provider_EHR_ID"] = ProviderEHRID;
                            newRow["WeekDay"] = officeHoursRow["WeekDay"];
                            //newRow["StartTime1"] = officeHoursRow["StartTime1"];
                            //newRow["EndTime1"] = officeHoursRow["EndTime1"];
                            newRow["Entry_DateTime"] = officeHoursRow["Entry_DateTime"];
                            newRow["Last_Sync_Date"] = officeHoursRow["Last_Sync_Date"];
                            newRow["is_deleted"] = officeHoursRow["is_deleted"];

                            dtOfficeHours.Rows.Add(newRow);
                        }
                    }

                    dtOfficeHours.AcceptChanges();

                    dtAbelDentLocalProviderOfficeHours = CompareDataTableRecords(ref dtOfficeHours, dtAbelDentLocalProviderOfficeHours, "POH_EHR_ID", "POH_LocalDB_ID", "POH_LocalDB_ID,POH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,StartTime1,EndTime1,StartTime2,EndTime2,StartTime3,EndTime3,Last_Sync_Date,Clinic_Number,Service_Install_Id");

                    dtOfficeHours.AcceptChanges();

                    if (dtOfficeHours != null && dtOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtOfficeHours.Clone();
                        if (dtOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "ProviderOfficeHours", "POH_LocalDB_ID,POH_Web_ID", "POH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ProviderOfficeHours Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }
        #endregion      

        private DataTable CompareDataTablehourAbelDent(ref DataTable dtSource, DataTable dtDestination, string compareColumnName, string primarykeyColumns, string ignoreColumns)
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

        #region Synch Speciality

        private void fncSynchDataAbelDent_Speciality()
        {
            InitBgWorkerAbelDent_Speciality();
            InitBgTimerAbelDent_Speciality();
        }

        private void InitBgTimerAbelDent_Speciality()
        {
            timerSynchAbelDent_Speciality = new System.Timers.Timer();
            this.timerSynchAbelDent_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchAbelDent_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Speciality_Tick);
            timerSynchAbelDent_Speciality.Enabled = true;
            timerSynchAbelDent_Speciality.Start();
        }

        private void InitBgWorkerAbelDent_Speciality()
        {
            bwSynchAbelDent_Speciality = new BackgroundWorker();
            bwSynchAbelDent_Speciality.WorkerReportsProgress = true;
            bwSynchAbelDent_Speciality.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Speciality.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Speciality_DoWork);
            bwSynchAbelDent_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Speciality_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Speciality.Enabled = false;
                MethodForCallSynchOrderAbelDent_Speciality();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Speciality()
        {
            System.Threading.Thread procThreadmainAbelDent_Speciality = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Speciality);
            procThreadmainAbelDent_Speciality.Start();
        }

        public void CallSyncOrderTableAbelDent_Speciality()
        {
            if (bwSynchAbelDent_Speciality.IsBusy != true)
            {
                bwSynchAbelDent_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_Speciality();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Speciality.Enabled = true;
        }

        public void SynchDataAbelDent_Speciality()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtAbelDentSpeciality = SynchAbelDentBAL.GetAbelDentSpecialityData();

                    dtAbelDentSpeciality.Columns.Add("InsUptDlt", typeof(int));
                    dtAbelDentSpeciality.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData("1");
                    if (!dtLocalSpeciality.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalSpeciality.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalSpeciality.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalSpeciality = CompareDataTableRecords(ref dtAbelDentSpeciality, dtLocalSpeciality, "Speciality_EHR_ID", "Speciality_LocalDB_ID", "Speciality_LocalDB_ID,Speciality_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,EHR_Entry_DateTime,Clinic_Number,Service_Install_Id");

                    dtAbelDentSpeciality.AcceptChanges();

                    if (dtAbelDentSpeciality != null && dtAbelDentSpeciality.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtAbelDentSpeciality.Clone();
                        if (dtAbelDentSpeciality.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtAbelDentSpeciality.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "Speciality", "Speciality_LocalDB_ID,Speciality_Web_ID", "Speciality_LocalDB_ID");
                        }
                        else
                        {
                            if (dtAbelDentSpeciality.Select("InsUptDlt IN (4)").Count() > 0)
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

        #region Synch Disease
        private void fncSynchDataAbelDent_Disease()
        {
            InitBgWorkerAbelDent_Disease();
            InitBgTimerAbelDent_Disease();
        }

        private void InitBgTimerAbelDent_Disease()
        {
            timerSynchAbelDent_Disease = new System.Timers.Timer();
            this.timerSynchAbelDent_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchAbelDent_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Disease_Tick);
            timerSynchAbelDent_Disease.Enabled = true;
            timerSynchAbelDent_Disease.Start();
        }

        private void InitBgWorkerAbelDent_Disease()
        {
            bwSynchAbelDent_Disease = new BackgroundWorker();
            bwSynchAbelDent_Disease.WorkerReportsProgress = true;
            bwSynchAbelDent_Disease.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Disease.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Disease_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchAbelDent_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Disease_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_Disease_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Disease.Enabled = false;
                MethodForCallSynchOrderAbelDent_Disease();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Disease()
        {
            System.Threading.Thread procThreadmainAbelDent_Disease = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Disease);
            procThreadmainAbelDent_Disease.Start();
        }

        public void CallSyncOrderTableAbelDent_Disease()
        {
            if (bwSynchAbelDent_Disease.IsBusy != true)
            {
                bwSynchAbelDent_Disease.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Disease_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Disease.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_Disease(); 
                SynchDataAbelDent_Problems();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Disease.Enabled = true;
        }

        public void SynchDataAbelDent_Disease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    DataTable dtAbelDentDisease = SynchAbelDentBAL.GetAbelDentDiseaseData();
                    dtAbelDentDisease.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalDisease = SynchLocalBAL.GetLocalDiseaseData("1");

                    if (!dtLocalDisease.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalDisease.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalDisease.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalDisease = CompareDataTableRecords(ref dtAbelDentDisease, dtLocalDisease, "Disease_EHR_ID", "Disease_LocalDB_ID", "Disease_LocalDB_ID,Disease_Web_ID,Is_Adit_Updated,is_deleted,Last_Sync_Date,InsUptDlt,EHR_Entry_DateTime,Clinic_Number,Service_Install_Id");

                    dtAbelDentDisease.AcceptChanges();

                    if (dtAbelDentDisease != null && dtAbelDentDisease.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtAbelDentDisease.Clone();
                        if (dtAbelDentDisease.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtAbelDentDisease.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "DiseaseMaster", "Disease_LocalDB_ID,Disease_Web_ID", "Disease_LocalDB_ID");
                        }
                        else
                        {
                            if (dtAbelDentDisease.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                            ObjGoalBase.WriteToSyncLogFile("Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Disease();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        public void SynchDataAbelDent_Problems()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    DataTable dtAbelDentProblem = SynchAbelDentBAL.GetAbelDentProblemData();
                    dtAbelDentProblem.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalDisease = SynchLocalBAL.GetLocalDiseaseData("1");

                    if (!dtLocalDisease.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalDisease.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalDisease.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalDisease = CompareDataTableRecords(ref dtAbelDentProblem, dtLocalDisease, "Disease_EHR_ID", "Disease_LocalDB_ID", "Disease_LocalDB_ID,Disease_Web_ID,Is_Adit_Updated,is_deleted,Last_Sync_Date,InsUptDlt,EHR_Entry_DateTime,Clinic_Number,Service_Install_Id");

                    dtAbelDentProblem.AcceptChanges();

                    if (dtAbelDentProblem != null && dtAbelDentProblem.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtAbelDentProblem.Clone();
                        if (dtAbelDentProblem.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtAbelDentProblem.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "DiseaseMaster", "Disease_LocalDB_ID,Disease_Web_ID", "Disease_LocalDB_ID");
                        }
                        else
                        {
                            if (dtAbelDentProblem.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                            ObjGoalBase.WriteToSyncLogFile("Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Disease();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        #endregion   

        #region Synch Operatory

        private void fncSynchDataAbelDent_Operatory()
        {
            InitBgWorkerAbelDent_Operatory();
            InitBgTimerAbelDent_Operatory();
        }

        private void InitBgTimerAbelDent_Operatory()
        {
            timerSynchAbelDent_Operatory = new System.Timers.Timer();
            this.timerSynchAbelDent_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchAbelDent_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Operatory_Tick);
            timerSynchAbelDent_Operatory.Enabled = true;
            timerSynchAbelDent_Operatory.Start();
        }

        private void InitBgWorkerAbelDent_Operatory()
        {
            bwSynchAbelDent_Operatory = new BackgroundWorker();
            bwSynchAbelDent_Operatory.WorkerReportsProgress = true;
            bwSynchAbelDent_Operatory.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Operatory.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Operatory_DoWork);
            bwSynchAbelDent_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Operatory_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Operatory.Enabled = false;
                MethodForCallSynchOrderAbelDent_Operatory();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Operatory()
        {
            System.Threading.Thread procThreadmainAbelDent_Operatory = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Operatory);
            procThreadmainAbelDent_Operatory.Start();
        }

        public void CallSyncOrderTableAbelDent_Operatory()
        {
            if (bwSynchAbelDent_Operatory.IsBusy != true)
            {
                bwSynchAbelDent_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Operatory.Enabled = true;
        }

        public void SynchDataAbelDent_Operatory()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtAbelDentOperatory = SynchAbelDentBAL.GetAbelDentOperatoryData();
                    dtAbelDentOperatory.Columns.Add("InsUptDlt", typeof(int));
                    dtAbelDentOperatory.Columns["InsUptDlt"].DefaultValue = 0;
                    DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData("1");

                    if (!dtLocalOperatory.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalOperatory.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalOperatory.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalOperatory = CompareDataTableRecords(ref dtAbelDentOperatory, dtLocalOperatory, "Operatory_EHR_ID", "Operatory_LocalDB_ID", "Operatory_LocalDB_ID,Operatory_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,,Clinic_Number,Service_Install_Id");

                    if (dtLocalOperatory.Select("InsUptDlt=3").Count() > 0)
                    {
                        dtAbelDentOperatory.Load(dtLocalOperatory.Select("InsUptDlt=3").CopyToDataTable().CreateDataReader());
                    }

                    dtAbelDentOperatory.AcceptChanges();

                    if (dtAbelDentOperatory != null && dtAbelDentOperatory.Rows.Count > 0 && dtAbelDentOperatory.AsEnumerable()
                            .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtAbelDentOperatory.Clone();
                        if (dtAbelDentOperatory.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtAbelDentOperatory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "Operatory", "Operatory_LocalDB_ID,Operatory_Web_ID", "Operatory_LocalDB_ID");
                        }
                        else
                        {
                            if (dtAbelDentOperatory.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                            ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsAbelDentOperatorySync = true;
                            SynchDataLiveDB_Push_Operatory();
                        }
                        else
                        {
                            IsAbelDentOperatorySync = false;
                        }
                    }
                    //SynchDataAbelDent_OperatoryHours();
                    //SynchDataAbelDent_OperatoryTimeOff();

                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }
        #endregion

        #region Synch OperatoryHours

        private void fncSynchDataAbelDent_OperatoryHours()
        {
            InitBgWorkerAbelDent_OperatoryHours();
            InitBgTimerAbelDent_OperatoryHours();
        }

        private void InitBgTimerAbelDent_OperatoryHours()
        {
            timerSynchAbelDent_OperatoryHours = new System.Timers.Timer();
            this.timerSynchAbelDent_OperatoryHours.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchAbelDent_OperatoryHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_OperatoryHours_Tick);
            timerSynchAbelDent_OperatoryHours.Enabled = true;
            timerSynchAbelDent_OperatoryHours.Start();
        }

        private void InitBgWorkerAbelDent_OperatoryHours()
        {
            bwSynchAbelDent_OperatoryHours = new BackgroundWorker();
            bwSynchAbelDent_OperatoryHours.WorkerReportsProgress = true;
            bwSynchAbelDent_OperatoryHours.WorkerSupportsCancellation = true;
            bwSynchAbelDent_OperatoryHours.DoWork += new DoWorkEventHandler(bwSynchAbelDent_OperatoryHours_DoWork);
            bwSynchAbelDent_OperatoryHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_OperatoryHours_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_OperatoryHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_OperatoryHours.Enabled = false;
                MethodForCallSynchOrderAbelDent_OperatoryHours();
            }
        }

        public void MethodForCallSynchOrderAbelDent_OperatoryHours()
        {
            System.Threading.Thread procThreadmainAbelDent_OperatoryHours = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_OperatoryHours);
            procThreadmainAbelDent_OperatoryHours.Start();
        }

        public void CallSyncOrderTableAbelDent_OperatoryHours()
        {
            if (bwSynchAbelDent_OperatoryHours.IsBusy != true)
            {
                bwSynchAbelDent_OperatoryHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_OperatoryHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_OperatoryHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_OperatoryHours();                
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_OperatoryHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_OperatoryHours.Enabled = true;
        }

        public void SynchDataAbelDent_OperatoryHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    SynchAbelDentBAL.GetAbelDentSystemData(ref _dayStartTime, ref _dayEndTime, ref _minutesInUnit, ref _workingDay);
                    List<String> columnList = SynchAbelDentBAL.GetAbelDentActiveColumnData();

                    #region Variables
                    _isFreeBlock = true;
                    bool status = false;
                    _days = _workingDay.ToCharArray();
                    int NumberOfOpenings = 20000;
                    int _unitNumbersPerDay = (int)(_dayEndTime - _dayStartTime).TotalMinutes / _minutesInUnit;
                    List<AbelOpenings> Hours = new List<AbelOpenings>();
                    #endregion

                    string fixedStartTime = _dayStartTime.ToString("HH:mm");//"07:45";
                    string fixedEndTime = _dayEndTime.ToString("HH:mm"); //"21:00";
                    string fixedStartDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string fixedEndDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
                    DateTime StartDate = DateTime.Parse(fixedStartDate + " " + fixedStartTime);
                    DateTime EndDate = Convert.ToDateTime("" + fixedEndDate + " " + fixedEndTime + "");

                    DataTable dtDeleteLocalOperatoryHours = SynchLocalBAL.GetLocalOperatoryHoursData("1");
                    if (dtDeleteLocalOperatoryHours.Rows.Count > 0)
                    {
                        SynchLocalBAL.DeleteLocalOperatoryHoursData("1");
                        SynchDataLiveDB_Push_OperatoryHours();
                        SynchLocalBAL.DeleteLocalOperatoryHoursAll();
                    }

                    for (var day = StartDate.Date; day.Date <= EndDate.Date; day = day.AddDays(1))
                    {
                        fixedStartDate = day.ToString("yyyy-MM-dd");
                        fixedEndDate = day.ToString("yyyy-MM-dd");
                        DateTime StartDate1 = DateTime.Parse(fixedStartDate + " " + fixedStartTime);
                        DateTime EndDate1 = DateTime.Parse(fixedEndDate + " " + fixedEndTime);

                        Hours = SynchAbelDentBAL.GetAbelDentOpeingsList(_units, StartDate1, EndDate1, columnList, _workingDay, false, "", NumberOfOpenings, _unitNumbersPerDay, _dayStartTime, _dayEndTime, _minutesInUnit, _isFreeBlock);
                        DataTable dtLocalOperatoryHours = SynchLocalBAL.GetLocalOperatoryHoursData("1");

                        if (!dtLocalOperatoryHours.Columns.Contains("InsUptDlt"))
                        {
                            dtLocalOperatoryHours.Columns.Add("InsUptDlt", typeof(int));
                            dtLocalOperatoryHours.Columns["InsUptDlt"].DefaultValue = 1;
                        }

                        DataTable createNewDt = new DataTable();

                        if (Hours.Count > 0)
                        {
                            createNewDt.Columns.Add("OH_LocalDB_ID", typeof(int));
                            createNewDt.Columns["OH_LocalDB_ID"].DefaultValue = 0;
                            createNewDt.Columns.Add("OH_EHR_ID", typeof(string));
                            createNewDt.Columns.Add("OH_Web_ID", typeof(string));
                            createNewDt.Columns["OH_Web_ID"].DefaultValue = "";
                            createNewDt.Columns.Add("Operatory_EHR_ID", typeof(string));
                            createNewDt.Columns.Add("StartTime", typeof(string));
                            createNewDt.Columns.Add("EndTime", typeof(string));
                            createNewDt.Columns.Add("comment", typeof(string));
                            createNewDt.Columns.Add("Entry_DateTime", typeof(string));
                            createNewDt.Columns.Add("Last_Sync_Date", typeof(string));
                            createNewDt.Columns["Last_Sync_Date"].DefaultValue = "";
                            createNewDt.Columns.Add("is_deleted", typeof(string));
                            createNewDt.Columns["is_deleted"].DefaultValue = 0;
                            createNewDt.Columns.Add("Is_Adit_Updated", typeof(int));
                            createNewDt.Columns["Is_Adit_Updated"].DefaultValue = 0;
                            createNewDt.Columns.Add("InsUptDlt", typeof(int));
                            createNewDt.Columns["InsUptDlt"].DefaultValue = 1;

                            for (int i = 0; i < Hours.Count; i++)
                            {
                                DataRow dataRow;

                                dataRow = createNewDt.NewRow();
                                dataRow["OH_EHR_ID"] = Hours[i].ColumnID + "_" + Hours[i].Time.ToString("yyyy-MM-dd-HH-mm").Replace('-', '_');
                                dataRow["Operatory_EHR_ID"] = Hours[i].ColumnID;

                                if (Hours[i].StartTimeList.Count > 0)
                                {
                                    if (Hours[i].StartTimeList[0].StartTimeRecords.Count > 0)
                                    {
                                        int endtimeCount = Hours[i].StartTimeList[0].StartTimeRecords.Count;
                                        dataRow["StartTime"] = Hours[i].StartTimeList[0].StartTimeRecords[0].Time.ToString();
                                        dataRow["EndTime"] = Hours[i].StartTimeList[0].StartTimeRecords[endtimeCount - 1].Time.AddMinutes(_minutesInUnit).ToString();
                                    }
                                    else
                                    {
                                        if (Hours[i].AvailableTime.Contains("0hr"+ _minutesInUnit + ""))
                                        {
                                            dataRow["StartTime"] = Hours[i].Time.ToString();
                                            dataRow["EndTime"] = Hours[i].Time.AddMinutes(_minutesInUnit).ToString();
                                        }
                                        else
                                        {
                                            dataRow["StartTime"] = "";
                                            dataRow["EndTime"] = "";
                                        }
                                    }
                                    dataRow["Entry_DateTime"] = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
                                    createNewDt.Rows.Add(dataRow);
                                }
                                else
                                    continue;
                            }

                            createNewDt.AcceptChanges();

                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(createNewDt, "OperatoryHours", "OH_LocalDB_ID,OH_Web_ID", "OH_LocalDB_ID");                           
                        }
                    }

                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
                        ObjGoalBase.WriteToSyncLogFile("OperatoryHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchDataLiveDB_Push_OperatoryHours();
                    }
                    IsEHRAllSync = true;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[OperatoryHours (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        #endregion

        #region OperatoryOfficeHours
        private void fncSynchDataAbelDent_Operatory_OfficeHours()
        {
            InitBgWorkerAbelDent_Operatory_OfficeHours();
            InitBgTimerAbelDent_Operatory_OfficeHours();
        }

        private void InitBgTimerAbelDent_Operatory_OfficeHours()
        {
            timerSynchAbelDent_Operatory_OfficeHours = new System.Timers.Timer();
            this.timerSynchAbelDent_Operatory_OfficeHours.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory_OfficeHours;
            this.timerSynchAbelDent_Operatory_OfficeHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Operatory_OfficeHours_Tick);
            timerSynchAbelDent_Operatory_OfficeHours.Enabled = true;
            timerSynchAbelDent_Operatory_OfficeHours.Start();
        }
        private void InitBgWorkerAbelDent_Operatory_OfficeHours()
        {
            bwSynchAbelDent_Operatory_OfficeHours = new BackgroundWorker();
            bwSynchAbelDent_Operatory_OfficeHours.WorkerReportsProgress = true;
            bwSynchAbelDent_Operatory_OfficeHours.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Operatory_OfficeHours.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Operatory_OfficeHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchAbelDent_Operatory_OfficeHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Operatory_OfficeHours_RunWorkerCompleted);
        }
        private void timerSynchAbelDent_Operatory_OfficeHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Operatory_OfficeHours.Enabled = false;
                MethodForCallSynchOrderAbelDent_Operatory_OfficeHours();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Operatory_OfficeHours()
        {
            System.Threading.Thread procThreadmainAbelDent_Operatory_OfficeHours = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Operatory_OfficeHours);
            procThreadmainAbelDent_Operatory_OfficeHours.Start();
        }

        public void CallSyncOrderTableAbelDent_Operatory_OfficeHours()
        {
            if (bwSynchAbelDent_Operatory_OfficeHours.IsBusy != true)
            {
                bwSynchAbelDent_Operatory_OfficeHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Operatory_OfficeHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Operatory_OfficeHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchAbelDent_OperatoryOfficeHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_Operatory_OfficeHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Operatory_OfficeHours.Enabled = true;
        }
        public void SynchAbelDent_OperatoryOfficeHours()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtOperatoryOfficeHours = SynchAbelDentBAL.GetAbelDentOperatoryOfficeHours();
                    DataTable dtOperatoryOfficeHoursOP = SynchAbelDentBAL.GetAbelDentOperatoryOfficeHoursOP();
                    DataTable dtAbelDentLocalOperatoryOfficeHours = SynchLocalBAL.GetLocalOperatoryOfficeHoursData("1");

                    if (!dtAbelDentLocalOperatoryOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtAbelDentLocalOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtAbelDentLocalOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    DataTable dtOfficeHours = new DataTable();

                    dtOfficeHours.Columns.Add("OOH_LocalDB_ID", typeof(int));
                    dtOfficeHours.Columns["OOH_LocalDB_ID"].DefaultValue = 0;
                    dtOfficeHours.Columns.Add("OOH_EHR_ID", typeof(string));
                    dtOfficeHours.Columns.Add("OOH_Web_ID", typeof(string));
                    dtOfficeHours.Columns["OOH_Web_ID"].DefaultValue = "";
                    dtOfficeHours.Columns.Add("Operatory_EHR_ID", typeof(string));
                    dtOfficeHours.Columns.Add("WeekDay", typeof(string));
                    dtOfficeHours.Columns.Add("StartTime1", typeof(string));
                    dtOfficeHours.Columns["StartTime1"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("EndTime1", typeof(string));
                    dtOfficeHours.Columns["EndTime1"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("StartTime2", typeof(string));
                    dtOfficeHours.Columns["StartTime2"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("EndTime2", typeof(string));
                    dtOfficeHours.Columns["EndTime2"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("StartTime3", typeof(string));
                    dtOfficeHours.Columns["StartTime3"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("EndTime3", typeof(string));
                    dtOfficeHours.Columns["EndTime3"].DefaultValue = "01/01/2020";
                    dtOfficeHours.Columns.Add("Entry_DateTime", typeof(string));
                    dtOfficeHours.Columns.Add("Last_Sync_Date", typeof(string));
                    dtOfficeHours.Columns["Last_Sync_Date"].DefaultValue = "";
                    dtOfficeHours.Columns.Add("is_deleted", typeof(string));
                    dtOfficeHours.Columns["is_deleted"].DefaultValue = 0;
                    dtOfficeHours.Columns.Add("Is_Adit_Updated", typeof(int));
                    dtOfficeHours.Columns["Is_Adit_Updated"].DefaultValue = 0;
                    dtOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;


                    foreach (DataRow providerRow in dtOperatoryOfficeHoursOP.Rows)
                    {
                        string OperatoryEHRID = providerRow["Operatory_EHR_ID"].ToString();

                        foreach (DataRow officeHoursRow in dtOperatoryOfficeHours.Rows)
                        {
                            // Create a new row for the combined DataTable
                            DataRow newRow = dtOfficeHours.NewRow();
                            newRow["OOH_EHR_ID"] = officeHoursRow["OOH_EHR_ID"] + "_" + OperatoryEHRID;
                            newRow["Operatory_EHR_ID"] = OperatoryEHRID;
                            newRow["WeekDay"] = officeHoursRow["WeekDay"];
                            //newRow["StartTime1"] = officeHoursRow["StartTime1"];
                            //newRow["EndTime1"] = officeHoursRow["EndTime1"];
                            newRow["Entry_DateTime"] = officeHoursRow["Entry_DateTime"];
                            newRow["Last_Sync_Date"] = officeHoursRow["Last_Sync_Date"];
                            newRow["is_deleted"] = officeHoursRow["is_deleted"];

                            dtOfficeHours.Rows.Add(newRow);
                        }
                    }

                    dtOfficeHours.AcceptChanges();

                    dtAbelDentLocalOperatoryOfficeHours = CompareDataTableRecords(ref dtOfficeHours, dtAbelDentLocalOperatoryOfficeHours, "OOH_EHR_ID", "OOH_LocalDB_ID", "OOH_LocalDB_ID,OOH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,StartTime1,EndTime2,StartTime2,EndTime2,StartTime3,EndTime3,Last_Sync_Date,Clinic_Number,Service_Install_Id");

                    dtOfficeHours.AcceptChanges();

                    if (dtOfficeHours != null && dtOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtOfficeHours.Clone();
                        if (dtOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "OperatoryOfficeHours", "OOH_LocalDB_ID,OOH_Web_ID", "OOH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[OperatoryOfficeHours Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }
        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataAbelDent_OperatoryEvent()
        {
            InitBgWorkerAbelDent_OperatoryEvent();
            InitBgTimerAbelDent_OperatoryEvent();
        }

        private void InitBgTimerAbelDent_OperatoryEvent()
        {
            timerSynchAbelDent_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchAbelDent_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchAbelDent_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_OperatoryEvent_Tick);
            timerSynchAbelDent_OperatoryEvent.Enabled = true;
            timerSynchAbelDent_OperatoryEvent.Start();
            timerSynchAbelDent_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerAbelDent_OperatoryEvent()
        {
            bwSynchAbelDent_OperatoryEvent = new BackgroundWorker();
            bwSynchAbelDent_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchAbelDent_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchAbelDent_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchAbelDent_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchAbelDent_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderAbelDent_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderAbelDent_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainAbelDent_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_OperatoryEvent);
            procThreadmainAbelDent_OperatoryEvent.Start();
        }

        public void CallSyncOrderTableAbelDent_OperatoryEvent()
        {
            if (bwSynchAbelDent_OperatoryEvent.IsBusy != true)
            {
                bwSynchAbelDent_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_OperatoryEvent();                
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_OperatoryEvent.Enabled = true;
        }

        public void SynchDataAbelDent_OperatoryEvent()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    SynchAbelDentBAL.GetAbelDentSystemData(ref _dayStartTime, ref _dayEndTime, ref _minutesInUnit, ref _workingDay);
                    List<String> columnList = SynchAbelDentBAL.GetAbelDentActiveColumnData();

                    #region Variables
                    _isFreeBlock = false;
                    bool status = false;
                    _days = _workingDay.ToCharArray();
                    int NumberOfOpenings = 20000;
                    int _unitNumbersPerDay = (int)(_dayEndTime - _dayStartTime).TotalMinutes / _minutesInUnit;
                    List<AbelOpenings> Hours = new List<AbelOpenings>();
                    #endregion

                    string fixedStartTime = _dayStartTime.ToString("HH:mm");//"07:45";
                    string fixedEndTime = _dayEndTime.ToString("HH:mm"); //"21:00";
                    string fixedStartDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string fixedEndDate = DateTime.Now.AddDays(60).ToString("yyyy-MM-dd");
                    DateTime StartDate = DateTime.Parse(fixedStartDate + " " + fixedStartTime);
                    DateTime EndDate = Convert.ToDateTime("" + fixedEndDate + " " + fixedEndTime + "");

                    DataTable dtDeleteLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData("1");

                    if (dtDeleteLocalOperatoryEvent.Rows.Count > 0)
                    {
                        SynchLocalBAL.DeleteLocalOperatoryEventData("1");
                        SynchDataLiveDB_Push_OperatoryEvent();
                        SynchLocalBAL.DeleteLocalOperatoryEventDataAll();
                    }

                    for (var day = StartDate.Date; day.Date <= EndDate.Date; day = day.AddDays(1))
                    {
                        fixedStartDate = day.ToString("yyyy-MM-dd");
                        fixedEndDate = day.ToString("yyyy-MM-dd");
                        DateTime StartDate1 = DateTime.Parse(fixedStartDate + " " + fixedStartTime);
                        DateTime EndDate1 = DateTime.Parse(fixedEndDate + " " + fixedEndTime);

                        Hours = SynchAbelDentBAL.GetAbelDentOpeingsList(_units, StartDate1, EndDate1, columnList, _workingDay, false, "", NumberOfOpenings, _unitNumbersPerDay, _dayStartTime, _dayEndTime, _minutesInUnit, _isFreeBlock);
                        DataTable dtLocalOperatoryHours = SynchLocalBAL.GetLocalOperatoryEventData("1");

                        if (!dtLocalOperatoryHours.Columns.Contains("InsUptDlt"))
                        {
                            dtLocalOperatoryHours.Columns.Add("InsUptDlt", typeof(int));
                            dtLocalOperatoryHours.Columns["InsUptDlt"].DefaultValue = 1;
                        }

                        DataTable createNewDt = new DataTable();

                        if (Hours.Count > 0)
                        {
                            createNewDt.Columns.Add("OE_LocalDB_ID", typeof(int));
                            createNewDt.Columns["OE_LocalDB_ID"].DefaultValue = 0;
                            createNewDt.Columns.Add("OE_EHR_ID", typeof(string));
                            createNewDt.Columns.Add("OE_Web_ID", typeof(string));
                            createNewDt.Columns["OE_Web_ID"].DefaultValue = "";
                            createNewDt.Columns.Add("Operatory_EHR_ID", typeof(string));
                            createNewDt.Columns.Add("StartTime", typeof(string));
                            createNewDt.Columns.Add("EndTime", typeof(string));
                            createNewDt.Columns.Add("comment", typeof(string));
                            createNewDt.Columns.Add("Entry_DateTime", typeof(string));
                            createNewDt.Columns.Add("Last_Sync_Date", typeof(string));
                            createNewDt.Columns["Last_Sync_Date"].DefaultValue = "";
                            createNewDt.Columns.Add("is_deleted", typeof(string));
                            createNewDt.Columns["is_deleted"].DefaultValue = 0;
                            createNewDt.Columns.Add("Is_Adit_Updated", typeof(int));
                            createNewDt.Columns["Is_Adit_Updated"].DefaultValue = 0;
                            createNewDt.Columns.Add("InsUptDlt", typeof(int));
                            createNewDt.Columns["InsUptDlt"].DefaultValue = 1;

                            for (int i = 0; i < Hours.Count; i++)
                            {
                                DataRow dataRow;

                                dataRow = createNewDt.NewRow();
                                dataRow["OE_EHR_ID"] = Hours[i].ColumnID + "_" + Hours[i].Time.ToString("yyyy-MM-dd-HH-mm").Replace('-', '_');
                                dataRow["Operatory_EHR_ID"] = Hours[i].ColumnID;

                                if (Hours[i].StartTimeList.Count > 0)
                                {
                                    if (Hours[i].StartTimeList[0].StartTimeRecords.Count > 0)
                                    {
                                        int endtimeCount = Hours[i].StartTimeList[0].StartTimeRecords.Count;
                                        dataRow["StartTime"] = Hours[i].StartTimeList[0].StartTimeRecords[0].Time.ToString();
                                        dataRow["EndTime"] = Hours[i].StartTimeList[0].StartTimeRecords[endtimeCount - 1].Time.AddMinutes(_minutesInUnit).ToString();
                                    }
                                    else
                                    {
                                        if (Hours[i].AvailableTime.Contains("0hr" + _minutesInUnit + ""))
                                        {
                                            dataRow["StartTime"] = Hours[i].Time.ToString();
                                            dataRow["EndTime"] = Hours[i].Time.AddMinutes(_minutesInUnit).ToString();
                                        }
                                        else
                                        {
                                            dataRow["StartTime"] = "";
                                            dataRow["EndTime"] = "";
                                        }
                                    }
                                    dataRow["Entry_DateTime"] = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
                                    createNewDt.Rows.Add(dataRow);
                                }
                                else
                                    continue;
                            }

                            createNewDt.AcceptChanges();                            
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(createNewDt, "OperatoryEvent", "OE_LocalDB_ID,OE_Web_ID", "OE_LocalDB_ID");
                            
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
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[OperatoryHours (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Patient

        private void fncSynchDataAbelDent_Patient()
        {
            InitBgWorkerAbelDent_Patient();
            InitBgTimerAbelDent_Patient();
        }

        private void InitBgTimerAbelDent_Patient()
        {
            timerSynchAbelDent_Patient = new System.Timers.Timer();
            this.timerSynchAbelDent_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchAbelDent_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Patient_Tick);
            timerSynchAbelDent_Patient.Enabled = true;
            timerSynchAbelDent_Patient.Start();
            timerSynchAbelDent_Patient_Tick(null, null);
        }

        private void InitBgWorkerAbelDent_Patient()
        {
            bwSynchAbelDent_Patient = new BackgroundWorker();
            bwSynchAbelDent_Patient.WorkerReportsProgress = true;
            bwSynchAbelDent_Patient.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Patient.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Patient_DoWork);
            bwSynchAbelDent_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Patient_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Patient.Enabled = false;
                MethodForCallSynchOrderAbelDent_Patient();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Patient()
        {
            System.Threading.Thread procThreadmainAbelDent_Patient = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Patient);
            procThreadmainAbelDent_Patient.Start();
        }

        public void CallSyncOrderTableAbelDent_Patient()
        {
            if (bwSynchAbelDent_Patient.IsBusy != true)
            {
                bwSynchAbelDent_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataAbelDent_Patient();
                SynchDataAbelDent_Patient();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Patient.Enabled = true;
        }

        public void SynchDataAbelDent_Patient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    IsParientFirstSync = false;
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    SynchDataLiveDB_Pull_EHR_Patientoptout();
                    DataTable dtAbelDentPatientList = SynchAbelDentBAL.GetAbelDentPatientData();
                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");

                    string patientTableName = "Patient";
                    if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                    {
                        patientTableName = "PatientCompare";
                    }
                    if (dtAbelDentPatientList != null && dtAbelDentPatientList.Rows.Count > 0)
                    {

                        bool isPatientSave = SynchAbelDentBAL.Save_AbelDent_To_Local(dtAbelDentPatientList, patientTableName, dtLocalPatient, true);
                        if (isPatientSave)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Patient();

                        }
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                        bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                    }
                    IsPatientSyncedFirstTime = true;
                    Is_synched_Patient = false;
                    SynchDataAbelDent_PatientStatus();
                    SynchDataAbelDent_PatientDisease();
                    SynchDataAbelDent_PatientMedication("");
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

        public void SynchDataAbelDent_PatientStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtAbelDentPatientStatusList = SynchAbelDentBAL.GetAbelDentPatientStatusData();
                    if (dtAbelDentPatientStatusList != null && dtAbelDentPatientStatusList.Rows.Count > 0)
                    {
                        SynchLocalBAL.UpdatePatient_Status(dtAbelDentPatientStatusList, "1");
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
        public void SynchDataAbelDent_AppointmentsPatient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtAbelDentPatientList = SynchAbelDentBAL.GetAbelDentAppointmentsPatientData();

                    string patientTableName = "Patient";

                    string PatientEHRIDs = string.Join("','", dtAbelDentPatientList.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                    if (PatientEHRIDs != string.Empty)
                    {
                        Is_synched_AppointmentsPatient = true;

                        PatientEHRIDs = "'" + PatientEHRIDs + "'";

                        DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, "1");

                        if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                        {
                            patientTableName = "PatientCompare";
                        }
                        if (dtAbelDentPatientList != null && dtAbelDentPatientList.Rows.Count > 0)
                        {
                            bool isPatientSave = SynchAbelDentBAL.Save_AbelDent_To_Local(dtAbelDentPatientList, patientTableName, dtLocalPatient);
                            if (isPatientSave)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                ObjGoalBase.WriteToSyncLogFile("Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                                SynchDataLiveDB_Push_Patient();
                            }
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                        }

                        Is_synched_AppointmentsPatient = false;
                        if (Is_Synched_PatientCallHit)
                        {
                            //Is_Synched_PatientCallHit = false;
                            //SynchDataAbelDent_Patient();
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

        public void SynchDataAbelDent_PatientDisease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    DataTable dtAbelDentPatientDisease = SynchAbelDentBAL.GetAbelDentPatientDiseaseData();
                    dtAbelDentPatientDisease.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalPatientDisease = SynchLocalBAL.GetLocalPatientDiseaseData("1");

                    if (!dtLocalPatientDisease.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalPatientDisease.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalPatientDisease.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalPatientDisease = CompareDataTableRecords(ref dtAbelDentPatientDisease, dtLocalPatientDisease, "Disease_EHR_ID", "PatientDisease_LocalDB_ID", "PatientDisease_LocalDB_ID,Patient_EHR_ID,PatientDisease_Web_ID,Disease_Name,Disease_Type,EHR_Entry_DateTime,Is_Adit_Updated,is_deleted,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id,PatientDisease_EHR_ID");

                    dtLocalPatientDisease.AcceptChanges();

                    if (dtAbelDentPatientDisease != null && dtAbelDentPatientDisease.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtAbelDentPatientDisease.Clone();
                        if (dtAbelDentPatientDisease.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtAbelDentPatientDisease.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "PatientDiseaseMaster", "PatientDisease_LocalDB_ID,PatientDisease_Web_ID", "PatientDisease_LocalDB_ID");
                        }
                        else
                        {
                            if (dtAbelDentPatientDisease.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientDiseaseMaster");
                            ObjGoalBase.WriteToSyncLogFile("Patient_Disease_Master Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_PatientDisease();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatientDisease = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientDisease_Master Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch New Patient
        private void fncSynchDataAbelDent_Patient_New()
        {
            InitBgWorkerAbelDent_Patient_New();
            InitBgTimerAbelDent_Patient_New();
        }

        private void InitBgTimerAbelDent_Patient_New()
        {
            timerSynchAbelDent_Patient_New = new System.Timers.Timer();
            this.timerSynchAbelDent_Patient_New.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchAbelDent_Patient_New.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_Patient_New_Tick);
            timerSynchAbelDent_Patient_New.Enabled = true;
            timerSynchAbelDent_Patient_New.Start();
            timerSynchAbelDent_Patient_New_Tick(null, null);
        }

        private void InitBgWorkerAbelDent_Patient_New()
        {
            bwSynchAbelDent_Patient_New = new BackgroundWorker();
            bwSynchAbelDent_Patient_New.WorkerReportsProgress = true;
            bwSynchAbelDent_Patient_New.WorkerSupportsCancellation = true;
            bwSynchAbelDent_Patient_New.DoWork += new DoWorkEventHandler(bwSynchAbelDent_Patient_New_DoWork);
            bwSynchAbelDent_Patient_New.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_Patient_New_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_Patient_New_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_Patient_New.Enabled = false;
                MethodForCallSynchOrderAbelDent_Patient_New();
            }
        }

        public void MethodForCallSynchOrderAbelDent_Patient_New()
        {
            System.Threading.Thread procThreadmainAbelDent_Patient_New = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_Patient_New);
            procThreadmainAbelDent_Patient_New.Start();
        }

        public void CallSyncOrderTableAbelDent_Patient_New()
        {
            if (bwSynchAbelDent_Patient_New.IsBusy != true)
            {
                bwSynchAbelDent_Patient_New.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_Patient_New_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_Patient_New.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataAbelDent_Patient_New();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_Patient_New_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_Patient_New.Enabled = true;
        }

        //public void SynchDataAbelDent_Patient_New()
        //{
        //    try
        //    {
        //        DataTable dtEHRPatientID = SynchAbelDentBAL.GetAbelDentPatientIDData();
        //        //DataTable dtLocalPatientID = SynchLocalBAL.GetLocalPatientIdData("1");

        //        var ehrlist = dtEHRPatientID.AsEnumerable().Select(r => r["Patient_EHR_ID"].ToString());
        //        string ehrvalue = string.Join(",", ehrlist);

        //        var locallist = dtLocalPatientID.AsEnumerable().Select(r => r["Patient_EHR_ID"].ToString());
        //        string localvalue = string.Join(",", locallist);

        //        String[] strs1 = ehrvalue.Split(',');
        //        String[] strs2 = localvalue.Split(',');
        //        var res = strs1.Except(strs2).Union(strs2.Except(strs1));
        //        String missmatchedId = String.Join(",", res);

        //        if (missmatchedId != "" && missmatchedId != null)
        //        {
        //            DataTable dtAbelDentPatient = SynchAbelDentBAL.GetAbelDentPatientDataWithCondition("Where PA.pid in (" + missmatchedId + ")");

        //            bool isPatientSave = SynchAbelDentBAL.Save_Patient_AbelDent_To_Local(dtAbelDentPatient, "1");

        //            if (isPatientSave)
        //            {
        //                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
        //                ObjGoalBase.WriteToSyncLogFile("New Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                SynchDataLiveDB_Push_Patient();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile("[New Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}


        #endregion

        #region PF Medicle History

        private void fncSynchDataAbelDent_MedicalHistory()
        {
            InitBgWorkerAbelDent_MedicalHistory();
            InitBgTimerAbelDent_MedicalHistory();
        }

        private void InitBgTimerAbelDent_MedicalHistory()
        {
            timerSynchAbelDent_MedicalHistory = new System.Timers.Timer();
            this.timerSynchAbelDent_MedicalHistory.Interval = 1000 * GoalBase.intervalEHRSynch_MedicalHistory;
            this.timerSynchAbelDent_MedicalHistory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_MedicalHistory_Tick);
            timerSynchAbelDent_MedicalHistory.Enabled = true;
            timerSynchAbelDent_MedicalHistory.Start();
            timerSynchAbelDent_MedicalHistory_Tick(null, null);
        }

        private void InitBgWorkerAbelDent_MedicalHistory()
        {
            bwSynchAbelDent_MedicalHistory = new BackgroundWorker();
            bwSynchAbelDent_MedicalHistory.WorkerReportsProgress = true;
            bwSynchAbelDent_MedicalHistory.WorkerSupportsCancellation = true;
            bwSynchAbelDent_MedicalHistory.DoWork += new DoWorkEventHandler(bwSynchAbelDent_MedicalHistory_DoWork);
            bwSynchAbelDent_MedicalHistory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_MedicalHistory_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_MedicalHistory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_MedicalHistory.Enabled = false;
                MethodForCallSynchOrderAbelDent_MedicalHistory();
            }
        }

        public void MethodForCallSynchOrderAbelDent_MedicalHistory()
        {
            System.Threading.Thread procThreadmainAbelDent_MedicalHistory = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_MedicalHistory);
            procThreadmainAbelDent_MedicalHistory.Start();
        }

        public void CallSyncOrderTableAbelDent_MedicalHistory()
        {
            if (bwSynchAbelDent_MedicalHistory.IsBusy != true)
            {
                bwSynchAbelDent_MedicalHistory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_MedicalHistory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_MedicalHistory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_MedicalHistory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_MedicalHistory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_MedicalHistory.Enabled = true;
        }

        public void SynchDataAbelDent_MedicalHistory()
        {
            SynchDataAbelDent_MedicleHistory();
        }

        public void SynchDataAbelDent_MedicleHistory()
        {
            SynchDataAbelDent_MedicleFormData();
            //SynchDataAbelDent_MedicleAnswerData();
            SynchDataAbelDent_MedicleFormQuestionData();
        }

        public void SynchDataAbelDent_MedicleFormData()
        {
            try
            {
                if (Utility.Application_Version != "12.10.6")
                {
                    if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                    {
                        DataTable dtAbelDentMedicleHistory = SynchAbelDentBAL.GetAbelDentMedicleFormData();
                        DataTable dtAbelDentLocalMedicleHistory = SynchLocalBAL.GetAbelDentLocalMedicleFormData();

                        dtAbelDentMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                        dtAbelDentMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;

                        if (!dtAbelDentLocalMedicleHistory.Columns.Contains("InsUptDlt"))
                        {
                            dtAbelDentLocalMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                            dtAbelDentLocalMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;
                        }

                        dtAbelDentLocalMedicleHistory = CompareDataTableRecords(ref dtAbelDentMedicleHistory, dtAbelDentLocalMedicleHistory, "AbelDent_Form_EHRUnique_ID", "AbelDent_Form_LocalDB_ID", "AbelDent_Form_LocalDB_ID,Entry_DateTime,Last_Sync_Date,EHR_Entry_DateTime,AbelDent_Form_Web_ID,Is_Adit_Updated,Version_Date,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                        dtAbelDentMedicleHistory.AcceptChanges();

                        if (dtAbelDentMedicleHistory != null && dtAbelDentMedicleHistory.Rows.Count > 0)
                        {
                            bool status = false;
                            DataTable dtSaveRecords = dtAbelDentMedicleHistory.Clone();
                            if (dtAbelDentMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0 || dtAbelDentLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                if (dtAbelDentMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0)
                                {
                                    dtSaveRecords.Load(dtAbelDentMedicleHistory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                                }
                                if (dtAbelDentLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                                {
                                    dtSaveRecords.Load(dtAbelDentLocalMedicleHistory.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                                }
                                status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "AbelDent_Form", "AbelDent_Form_LocalDB_ID,AbelDent_Form_Web_ID", "AbelDent_Form_LocalDB_ID");
                            }
                            else
                            {
                                if (dtAbelDentMedicleHistory.Select("InsUptDlt IN (4)").Count() > 0)
                                {
                                    status = true;
                                }
                            }
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("AbelDent_Form");
                                ObjGoalBase.WriteToSyncLogFile("AbelDent_Form Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            }
                            SynchDataLiveDB_Push_MedicalHisotryTables("AbelDent_Form");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[AbelDent_Form Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataAbelDent_MedicleFormQuestionData()
        {
            try
            {
                if (Utility.Application_Version != "12.10.6")
                {
                    if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                    {

                        DataTable dtAbelDentMedicleHistory = SynchAbelDentBAL.GetAbelDentMedicleFormQuestionData();
                        DataTable dtAbelDentLocalMedicleHistory = SynchLocalBAL.GetAbelDentLocalFormQuestionData();

                        dtAbelDentMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                        dtAbelDentMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;

                        if (!dtAbelDentLocalMedicleHistory.Columns.Contains("InsUptDlt"))
                        {
                            dtAbelDentLocalMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                            dtAbelDentLocalMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;
                        }

                        dtAbelDentLocalMedicleHistory = CompareDataTableRecords(ref dtAbelDentMedicleHistory, dtAbelDentLocalMedicleHistory, "AbelDent_Question_EHRUnique_ID", "AbelDent_FormQuestion_LocalDB_ID", "AbelDent_FormQuestion_LocalDB_ID,AbelDent_FormQuestion_Web_ID,Is_Adit_Updated,EHR_Entry_DateTime,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Answer_Value,Service_Install_Id,QuestionVersion_Date");

                        dtAbelDentMedicleHistory.AcceptChanges();

                        if (dtAbelDentMedicleHistory != null && dtAbelDentMedicleHistory.Rows.Count > 0)
                        {
                            bool status = false;
                            DataTable dtSaveRecords = dtAbelDentMedicleHistory.Clone();
                            if (dtAbelDentMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0 || dtAbelDentLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                if (dtAbelDentMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0)
                                {
                                    dtSaveRecords.Load(dtAbelDentMedicleHistory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                                }
                                if (dtAbelDentLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                                {
                                    dtSaveRecords.Load(dtAbelDentLocalMedicleHistory.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                                }

                                status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "AbelDent_FormQuestion", "AbelDent_FormQuestion_LocalDB_ID,AbelDent_FormQuestion_Web_ID", "AbelDent_FormQuestion_LocalDB_ID");
                            }
                            else
                            {
                                if (dtAbelDentMedicleHistory.Select("InsUptDlt IN (4)").Count() > 0)
                                {
                                    status = true;
                                }
                            }
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("AbelDent_FormQuestion");
                                ObjGoalBase.WriteToSyncLogFile("AbelDent_FormQuestion Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            }
                            SynchDataLiveDB_Push_MedicalHisotryTables("AbelDent_FormQuestion");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[AbelDent_FormQuestion Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }


        #endregion

        #region Patient Form

        private void fncSynchDataLocalToAbelDent_Patient_Form()
        {
            InitBgWorkerLocalToAbelDent_Patient_Form();
            InitBgTimerLocalToAbelDent_Patient_Form();
        }

        private void InitBgTimerLocalToAbelDent_Patient_Form()
        {
            timerSynchLocalToAbelDent_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToAbelDent_Patient_Form.Interval = 1000 * GoalBase.intervalWebSynch_Pull_PatientForm;
            this.timerSynchLocalToAbelDent_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToAbelDent_Patient_Form_Tick);
            timerSynchLocalToAbelDent_Patient_Form.Enabled = true;
            timerSynchLocalToAbelDent_Patient_Form.Start();
            timerSynchLocalToAbelDent_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToAbelDent_Patient_Form()
        {
            bwSynchLocalToAbelDent_Patient_Form = new BackgroundWorker();
            bwSynchLocalToAbelDent_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToAbelDent_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToAbelDent_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToAbelDent_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToAbelDent_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToAbelDent_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToAbelDent_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToAbelDent_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToAbelDent_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToAbelDent_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToAbelDent_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToAbelDent_Patient_Form);
            procThreadmainLocalToAbelDent_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToAbelDent_Patient_Form()
        {
            if (bwSynchLocalToAbelDent_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToAbelDent_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToAbelDent_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToAbelDent_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToAbelDent_Patient_Form();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToAbelDent_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToAbelDent_Patient_Form.Enabled = true;
        }

        public void SynchDataLocalToAbelDent_Patient_Form()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    SynchDataLiveDB_Pull_PatientForm();
                    SynchDataLiveDB_Pull_PatientPortal();
                    //CheckEntryUserLoginIdExist();
                    DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData("1");

                    dtWebPatient_Form.Columns.Add("Table_Name", typeof(string));
                    dtWebPatient_Form.Columns["Table_Name"].DefaultValue = "pat";
                    dtWebPatient_Form.Columns.Add("pformfield", typeof(string));

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {

                        foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                        {
                            if (dtDtxRow["ehrfield"].ToString().Trim() == "preferred_name")
                            {
                                dtDtxRow["ehrfield"] = "infgivenname";
                                dtDtxRow["Table_Name"] = "inf";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "first_name")
                            {
                                dtDtxRow["ehrfield"] = "pfname";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "last_name")
                            {
                                dtDtxRow["ehrfield"] = "plname";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "middle_name")
                            {
                                dtDtxRow["ehrfield"] = "pinitial";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "sex")
                            {
                                dtDtxRow["ehrfield"] = "pgender";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                                {
                                    dtDtxRow["ehrfield_value"] = "F";
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MALE")
                                {
                                    dtDtxRow["ehrfield_value"] = "M";
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = " ";
                                }
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_one")
                            {
                                dtDtxRow["ehrfield"] = "pstreetadr";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_two")
                            {
                                dtDtxRow["ehrfield"] = "pstreetadr2";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "zipcode")
                            {
                                dtDtxRow["ehrfield"] = "ppostal";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "city")
                            {
                                dtDtxRow["ehrfield"] = "pcitycode";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "email")
                            {
                                dtDtxRow["ehrfield"] = "infemail";
                                dtDtxRow["Table_Name"] = "inf";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_email")
                            {
                                dtDtxRow["ehrfield"] = "infallownewsemail";
                                dtDtxRow["Table_Name"] = "inf";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO" || dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "N")
                                {
                                    dtDtxRow["ehrfield_value"] = "0";
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "YES" || dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "Y")
                                {
                                    dtDtxRow["ehrfield_value"] = "1";
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "work_phone")
                            {
                                dtDtxRow["ehrfield"] = "pworkphn";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "mobile")
                            {
                                dtDtxRow["ehrfield"] = "infmobile";
                                dtDtxRow["Table_Name"] = "inf";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "home_phone")
                            {
                                dtDtxRow["ehrfield"] = "pphone";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_sms")
                            {
                                dtDtxRow["ehrfield"] = "infallowtextmsg";
                                dtDtxRow["Table_Name"] = "inf";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO")
                                {
                                    dtDtxRow["ehrfield_value"] = "0";
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = "1";
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
                            {
                                dtDtxRow["ehrfield"] = "pdentist";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
                            {
                                dtDtxRow["ehrfield"] = "phygienist";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SALUTATION")
                            {
                                dtDtxRow["ehrfield"] = "pmrmrs";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance")
                            {
                                dtDtxRow["ehrfield"] = "ixiplanid_pri_ins";
                                dtDtxRow["Table_Name"] = "ixi";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance")
                            {
                                dtDtxRow["ehrfield"] = "ixiplanid_sec_ins";
                                dtDtxRow["Table_Name"] = "ixi";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                            {
                                dtDtxRow["ehrfield"] = "ixipid_pri_sub";
                                dtDtxRow["Table_Name"] = "ixi";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                            {
                                dtDtxRow["ehrfield"] = "ixipid_sec_sub";
                                dtDtxRow["Table_Name"] = "ixi";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim() == "birth_date")
                            {
                                dtDtxRow["ehrfield"] = "pbirth";
                                dtDtxRow["Table_Name"] = "pat";
                            }
                            else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "employer"))
                            {
                                dtDtxRow["ehrfield"] = "infemployer";
                                dtDtxRow["Table_Name"] = "inf";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance_companyname")
                            {
                                dtDtxRow["ehrfield"] = "pri_insurance_companyname";
                                dtDtxRow["Table_Name"] = "nsp";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance_companyname")
                            {
                                dtDtxRow["ehrfield"] = "sec_insurance_companyname";
                                dtDtxRow["Table_Name"] = "nsp";
                            }

                            dtWebPatient_Form.AcceptChanges();
                        }
                        if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                        {
                            bool Is_Record_Update = SynchAbelDentBAL.Save_Patient_Form_Local_To_AbelDent(dtWebPatient_Form);
                        }

                        try
                        {
                            if (SynchAbelDentBAL.SavePatientAllergies_To_AbelDent())
                            {
                                ObjGoalBase.WriteToSyncLogFile("Patient_Allergies Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Allergies Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]");
                            }
                        }
                        catch (Exception ex1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Allergies Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
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

                        bool isRecordSaved = false;
                        bool isSaveMedication = false;
                        string Patient_EHR_IDS = "";
                        string SavePatientEHRID = "";

                        try
                        {
                            SynchAbelDentBAL.SavePatientMedicationLocalToAbelDent(ref isRecordSaved, ref SavePatientEHRID);
                        }
                        catch (Exception exMed)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + exMed.Message);
                        }
                        if (isSaveMedication)
                        {
                            Patient_EHR_IDS = (SavePatientEHRID).TrimEnd(',');
                            if (Patient_EHR_IDS != "")
                            {
                                SynchDataAbelDent_PatientMedication(Patient_EHR_IDS);
                            }
                        }

                        Is_synched_PatinetForm = false;
                        try
                        {
                            GetMedicalAbelDentHistoryRecords();
                            SynchAbelDentBAL.SaveMedicalHistoryLocalToAbelDent();
                            ObjGoalBase.WriteToSyncLogFile("Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }

                        SynchDataLocalToAbelDent_Patient_Document();
                        ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
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

        public void SynchDataLiveDB_PatientSMSCallLog_LocalToAbelDent()
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
                        
                        #region Call API for EHR Entry Done
                        if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                            SynchAbelDentBAL.Save_PatientSMSCallLog_LocalToAbelDent(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientPayment_LocalTOAbelDent");
                            ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                        }

                        #endregion                     
                    }

                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[PatientSMSCallLog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Appointment Type

        private void fncSynchDataAbelDent_ApptType()
        {
            InitBgWorkerAbelDent_ApptType();
            InitBgTimerAbelDent_ApptType();
        }

        private void InitBgTimerAbelDent_ApptType()
        {
            timerSynchAbelDent_ApptType = new System.Timers.Timer();
            this.timerSynchAbelDent_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchAbelDent_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_ApptType_Tick);
            timerSynchAbelDent_ApptType.Enabled = true;
            timerSynchAbelDent_ApptType.Start();
        }

        private void InitBgWorkerAbelDent_ApptType()
        {
            bwSynchAbelDent_ApptType = new BackgroundWorker();
            bwSynchAbelDent_ApptType.WorkerReportsProgress = true;
            bwSynchAbelDent_ApptType.WorkerSupportsCancellation = true;
            bwSynchAbelDent_ApptType.DoWork += new DoWorkEventHandler(bwSynchAbelDent_ApptType_DoWork);
            bwSynchAbelDent_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_ApptType_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_ApptType.Enabled = false;
                MethodForCallSynchOrderAbelDent_ApptType();
            }
        }

        public void MethodForCallSynchOrderAbelDent_ApptType()
        {
            System.Threading.Thread procThreadmainAbelDent_ApptType = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_ApptType);
            procThreadmainAbelDent_ApptType.Start();
        }

        public void CallSyncOrderTableAbelDent_ApptType()
        {
            if (bwSynchAbelDent_ApptType.IsBusy != true)
            {
                bwSynchAbelDent_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_ApptType.Enabled = true;
        }

        public void SynchDataAbelDent_ApptType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtAbelDentApptStatus = SynchAbelDentBAL.GetAbelDentApptTypeData();

                    dtAbelDentApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtAbelDentApptStatus.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalApptTypeData("1");

                    if (!dtLocalApptStatus.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalApptStatus.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalApptStatus.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalApptStatus = CompareDataTableRecords(ref dtAbelDentApptStatus, dtLocalApptStatus, "ApptType_EHR_ID", "ApptType_LocalDB_ID", "ApptType_LocalDB_ID,ApptType_EHR_ID,ApptType_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    dtAbelDentApptStatus.AcceptChanges();

                    bool status = false;
                    DataTable dtSaveRecords = dtAbelDentApptStatus.Clone();
                    if (dtAbelDentApptStatus.Select("InsUptDlt IN (1,2)").Count() > 0)
                    {
                        dtSaveRecords.Load(dtAbelDentApptStatus.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                        status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "Appointment_Type", "ApptType_LocalDB_ID,ApptType_Web_ID", "ApptType_LocalDB_ID");
                    }
                    else
                    {
                        if (dtAbelDentApptStatus.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                        ObjGoalBase.WriteToSyncLogFile("ApptType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        IsAbelDentApptTypeSync = true;
                        SynchDataLiveDB_Push_ApptType();
                    }
                    else
                    {
                        IsAbelDentApptTypeSync = false;
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ApptType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion       

        #region Synch ApptStatus

        private void fncSynchDataAbelDent_ApptStatus()
        {
            InitBgWorkerAbelDent_ApptStatus();
            InitBgTimerAbelDent_ApptStatus();
        }

        private void InitBgTimerAbelDent_ApptStatus()
        {
            timerSynchAbelDent_ApptStatus = new System.Timers.Timer();
            this.timerSynchAbelDent_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchAbelDent_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_ApptStatus_Tick);
            timerSynchAbelDent_ApptStatus.Enabled = true;
            timerSynchAbelDent_ApptStatus.Start();
        }

        private void InitBgWorkerAbelDent_ApptStatus()
        {
            bwSynchAbelDent_ApptStatus = new BackgroundWorker();
            bwSynchAbelDent_ApptStatus.WorkerReportsProgress = true;
            bwSynchAbelDent_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchAbelDent_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchAbelDent_ApptStatus_DoWork);
            bwSynchAbelDent_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_ApptStatus.Enabled = false;
                MethodForCallSynchOrderAbelDent_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderAbelDent_ApptStatus()
        {
            System.Threading.Thread procThreadmainAbelDent_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_ApptStatus);
            procThreadmainAbelDent_ApptStatus.Start();
        }

        public void CallSyncOrderTableAbelDent_ApptStatus()
        {
            if (bwSynchAbelDent_ApptStatus.IsBusy != true)
            {
                bwSynchAbelDent_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_ApptStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_ApptStatus.Enabled = true;
        }

        public void SynchDataAbelDent_ApptStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtAbelDentApptStatus = SynchAbelDentBAL.GetAbelDentApptStatusData();

                    dtAbelDentApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtAbelDentApptStatus.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData("1");

                    if (!dtLocalApptStatus.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalApptStatus.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalApptStatus.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtLocalApptStatus = CompareDataTableRecords(ref dtAbelDentApptStatus, dtLocalApptStatus, "ApptStatus_EHR_ID", "ApptStatus_LocalDB_ID", "ApptStatus_LocalDB_ID,ApptStatus_EHR_ID,ApptStatus_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Clinic_Number,Service_Install_Id");

                    dtAbelDentApptStatus.AcceptChanges();

                    bool status = false;
                    DataTable dtSaveRecords = dtAbelDentApptStatus.Clone();
                    if (dtAbelDentApptStatus.Select("InsUptDlt IN (1,2)").Count() > 0)
                    {
                        dtSaveRecords.Load(dtAbelDentApptStatus.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                        status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "Appointment_Status", "ApptStatus_LocalDB_ID,ApptStatus_Web_ID", "ApptStatus_LocalDB_ID");
                    }
                    else
                    {
                        if (dtAbelDentApptStatus.Select("InsUptDlt IN (4)").Count() > 0)
                        {
                            status = true;
                        }
                    }
                    if (status)
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                        ObjGoalBase.WriteToSyncLogFile("ApptStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        IsAbelDentApptStatusSync = true;
                        SynchDataLiveDB_Push_ApptStatus();
                    }
                    else
                    {
                        IsAbelDentApptStatusSync = false;
                    }

                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ApptStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Patient Recall

        private void fncSynchDataAbelDent_PatientRecall()
        {
            InitBgWorkerAbelDent_PatientRecall();
            InitBgTimerAbelDent_PatientRecall();
        }

        private void InitBgTimerAbelDent_PatientRecall()
        {
            timerSynchAbelDent_PatientRecall = new System.Timers.Timer();
            this.timerSynchAbelDent_PatientRecall.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchAbelDent_PatientRecall.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_PatientRecall_Tick);
            timerSynchAbelDent_PatientRecall.Enabled = true;
            timerSynchAbelDent_PatientRecall.Start();
        }

        private void InitBgWorkerAbelDent_PatientRecall()
        {
            bwSynchAbelDent_PatientRecall = new BackgroundWorker();
            bwSynchAbelDent_PatientRecall.WorkerReportsProgress = true;
            bwSynchAbelDent_PatientRecall.WorkerSupportsCancellation = true;
            bwSynchAbelDent_PatientRecall.DoWork += new DoWorkEventHandler(bwSynchAbelDent_PatientRecall_DoWork);
            bwSynchAbelDent_PatientRecall.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_PatientRecall_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_PatientRecall_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_PatientRecall.Enabled = false;
                MethodForCallSynchOrderAbelDent_PatientRecall();
            }
        }

        public void MethodForCallSynchOrderAbelDent_PatientRecall()
        {
            System.Threading.Thread procThreadmainAbelDent_PatientRecall = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_PatientRecall);
            procThreadmainAbelDent_PatientRecall.Start();
        }

        public void CallSyncOrderTableAbelDent_PatientRecall()
        {
            if (bwSynchAbelDent_PatientRecall.IsBusy != true)
            {
                bwSynchAbelDent_PatientRecall.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_PatientRecall_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_PatientRecall.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_PatientRecall();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_PatientRecall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_PatientRecall.Enabled = true;
        }

        public void SynchDataAbelDent_PatientRecall()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtAbelDentPatientRecall = SynchAbelDentBAL.GetAbelDentPatient_recall();
                    dtAbelDentPatientRecall.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalRecallType = SynchLocalBAL.GetLocalPatientWiseRecallTypeData("1");

                    foreach (DataRow dtDtxRow in dtAbelDentPatientRecall.Rows)
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

                    dtAbelDentPatientRecall.AcceptChanges();

                    if (dtAbelDentPatientRecall != null && dtAbelDentPatientRecall.Rows.Count > 0)
                    {
                        bool status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtAbelDentPatientRecall, "Patient_RecallType", "Patient_RecallType_LocalDB_ID,Patient_Web_ID", "Patient_RecallType_LocalDB_ID");
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_RecallType");
                            ObjGoalBase.WriteToSyncLogFile("Patient_RecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            //SynchDataLiveDB_Push_RecallType();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_RecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch RecallType

        private void fncSynchDataAbelDent_RecallType()
        {
            InitBgWorkerAbelDent_RecallType();
            InitBgTimerAbelDent_RecallType();
        }

        private void InitBgTimerAbelDent_RecallType()
        {
            timerSynchAbelDent_RecallType = new System.Timers.Timer();
            this.timerSynchAbelDent_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchAbelDent_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_RecallType_Tick);
            timerSynchAbelDent_RecallType.Enabled = true;
            timerSynchAbelDent_RecallType.Start();
        }

        private void InitBgWorkerAbelDent_RecallType()
        {
            bwSynchAbelDent_RecallType = new BackgroundWorker();
            bwSynchAbelDent_RecallType.WorkerReportsProgress = true;
            bwSynchAbelDent_RecallType.WorkerSupportsCancellation = true;
            bwSynchAbelDent_RecallType.DoWork += new DoWorkEventHandler(bwSynchAbelDent_RecallType_DoWork);
            bwSynchAbelDent_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_RecallType_RunWorkerCompleted);
        }

        private void timerSynchAbelDent_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_RecallType.Enabled = false;
                MethodForCallSynchOrderAbelDent_RecallType();
            }
        }

        public void MethodForCallSynchOrderAbelDent_RecallType()
        {
            System.Threading.Thread procThreadmainAbelDent_RecallType = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_RecallType);
            procThreadmainAbelDent_RecallType.Start();
        }

        public void CallSyncOrderTableAbelDent_RecallType()
        {
            if (bwSynchAbelDent_RecallType.IsBusy != true)
            {
                bwSynchAbelDent_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbelDent_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbelDent_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_RecallType.Enabled = true;
        }

        public void SynchDataAbelDent_RecallType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtAbelDentPatientRecal = SynchAbelDentBAL.GetAbelDentRecallTypeData();
                    dtAbelDentPatientRecal.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData("1");

                    foreach (DataRow dtDtxRow in dtAbelDentPatientRecal.Rows)
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

                    dtAbelDentPatientRecal.AcceptChanges();

                    if (dtAbelDentPatientRecal != null && dtAbelDentPatientRecal.Rows.Count > 0)
                    {
                        bool status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtAbelDentPatientRecal, "RecallType", "RecallType_LocalDB_ID,Patient_Web_ID", "RecallType_LocalDB_ID");
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

        #region Synch User
        private void fncSynchDataAbelDent_User()
        {
            InitBgWorkerAbelDent_User();
            InitBgTimerAbelDent_User();
        }

        private void InitBgTimerAbelDent_User()
        {
            timerSynchAbelDent_User = new System.Timers.Timer();
            this.timerSynchAbelDent_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchAbelDent_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbelDent_User_Tick);
            timerSynchAbelDent_User.Enabled = true;
            timerSynchAbelDent_User.Start();
        }

        private void timerSynchAbelDent_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbelDent_User.Enabled = false;
                MethodForCallSynchOrderAbelDent_User();
            }
        }

        private void MethodForCallSynchOrderAbelDent_User()
        {
            System.Threading.Thread procThreadmainAbelDent_User = new System.Threading.Thread(this.CallSyncOrderTableAbelDent_User);
            procThreadmainAbelDent_User.Start();
        }

        private void CallSyncOrderTableAbelDent_User()
        {
            if (bwSynchAbelDent_User.IsBusy != true)
            {
                bwSynchAbelDent_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void InitBgWorkerAbelDent_User()
        {
            bwSynchAbelDent_User = new BackgroundWorker();
            bwSynchAbelDent_User.WorkerReportsProgress = true;
            bwSynchAbelDent_User.WorkerSupportsCancellation = true;
            bwSynchAbelDent_User.DoWork += new DoWorkEventHandler(bwSynchAbelDent_User_DoWork);
            bwSynchAbelDent_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbelDent_User_RunWorkerCompleted);
        }

        private void bwSynchAbelDent_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbelDent_User.Enabled = true;
        }

        private void bwSynchAbelDent_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbelDent_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataAbelDent_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SynchDataAbelDent_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtAbelDentUser = SynchAbelDentBAL.GetAbelDentUser();
                    dtAbelDentUser.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalUser = SynchLocalBAL.GetLocalUser("1");

                    foreach (DataRow dtDtxRow in dtAbelDentUser.Rows)
                    {
                        DataRow[] row = dtLocalUser.Copy().Select("User_EHR_ID = '" + dtDtxRow["User_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["Last_Name"].ToString().ToLower().Trim())
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
                        DataRow[] row = dtAbelDentUser.Copy().Select("User_EHR_ID = '" + dtDtxRow["User_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtAbelDentUser.NewRow();
                            BlcOptDtldr["User_EHR_ID"] = dtDtxRow["User_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtAbelDentUser.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtAbelDentUser.AcceptChanges();

                    if (dtAbelDentUser != null && dtAbelDentUser.Rows.Count > 0)
                    {
                        bool status = SynchAbelDentBAL.Save_Users_AbelDent_To_Local(dtAbelDentUser);
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
                    ObjGoalBase.WriteToErrorLogFile("[User Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);

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
        }

        #endregion

        #region PatientPayment
        private void fncSynchDataAbeldent_PatientPayment()
        {
            InitBgWorkerAbeldent_PatientPayment();
            InitBgTimerAbeldent_PatientPayment();
        }

        private void InitBgTimerAbeldent_PatientPayment()
        {
            timerSynchAbeldent_PatientPayment = new System.Timers.Timer();
            this.timerSynchAbeldent_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
            this.timerSynchAbeldent_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchAbeldent_PatientPayment_Tick);
            timerSynchAbeldent_PatientPayment.Enabled = true;
            timerSynchAbeldent_PatientPayment.Start();
        }

        private void InitBgWorkerAbeldent_PatientPayment()
        {
            bwSynchAbeldent_PatientPayment = new BackgroundWorker();
            bwSynchAbeldent_PatientPayment.WorkerReportsProgress = true;
            bwSynchAbeldent_PatientPayment.WorkerSupportsCancellation = true;
            bwSynchAbeldent_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchAbeldent_PatientPayment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchAbeldent_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchAbeldent_PatientPayment_RunWorkerCompleted);
        }

        private void timerSynchAbeldent_PatientPayment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchAbeldent_PatientPayment.Enabled = false;
                MethodForCallSynchOrderAbeldent_PatientPayment();
            }
        }

        private void MethodForCallSynchOrderAbeldent_PatientPayment()
        {
            System.Threading.Thread procThreadmainAbeldent_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTableAbeldent_PatientPayment);
            procThreadmainAbeldent_PatientPayment.Start();
        }

        private void CallSyncOrderTableAbeldent_PatientPayment()
        {
            if (bwSynchAbeldent_PatientPayment.IsBusy != true)
            {
                bwSynchAbeldent_PatientPayment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchAbeldent_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchAbeldent_PatientPayment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_PatientPayment_LocalToAbelDent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchAbeldent_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchAbeldent_PatientPayment.Enabled = true;
        }

        private void SynchDataLiveDB_PatientPayment_LocalToAbelDent()
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
                        DataTable dtPatientPayment = new DataTable();
                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            noteId = "";

                            #region Call API for EHR Entry Done
                            if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                            {                                
                                SynchAbelDentBAL.SavePatientPaymentTOEHR(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient Payment Log");
                                ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");
                            }
                            #endregion
                        }

                    }
                    IsPaymentSyncing = false;
                }
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Patient Payment Log Sync " + Ex.Message);
            }
        }

        #endregion

        #region Sync Patient Medication
        public void SynchDataAbelDent_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchAbelDentBAL.GetAbelDentMedicationData();
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
                            DataRow[] rowDis = dtMedication.Copy().Select("Medication_EHR_ID = '" + dtLPHRow["Medication_EHR_ID"] + "' AND Medication_Type = '" + dtLPHRow["Medication_Type"] + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"] + "'");
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
        public void SynchDataAbelDent_PatientMedication(string Patinet_EHR_IDS)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchAbelDentBAL.GetAbelDentPatientMedicationData(Patinet_EHR_IDS);
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patinet_EHR_IDS);

                        dtMedication.AcceptChanges();

                        DataTable dtSaveRecords = new DataTable();
                        dtSaveRecords = dtLocalMedication.Clone();

                        //Insert
                        var itemsToBeAdded = (from AbelDent in dtMedication.AsEnumerable()
                                              join Local in dtLocalMedication.AsEnumerable()
                                              on AbelDent["PatientMedication_EHR_ID"].ToString().Trim()
                                              equals Local["PatientMedication_EHR_ID"].ToString().Trim() into matchingRows
                                              from matchingRow in matchingRows.DefaultIfEmpty()
                                              where matchingRow == null
                                              select AbelDent).ToList();
                        DataTable dtPatientToBeAdded = dtLocalMedication.Clone();
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

                        ////Update
                        //var itemsToBeUpdated = (from AbelDent in dtMedication.AsEnumerable()
                        //                        join Local in dtLocalMedication.AsEnumerable()
                        //                        on AbelDent["Medication_EHR_ID"].ToString().Trim() + "_" + AbelDent["Clinic_Number"].ToString().Trim()
                        //                        equals Local["Medication_EHR_ID"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                        //                        where
                        //                        AbelDent["Medication_Name"].ToString().Trim() != Local["Medication_Name"].ToString().Trim() ||
                        //                        AbelDent["Drug_Quantity"].ToString().ToUpper().Trim() != Local["Drug_Quantity"].ToString().ToUpper().Trim() ||
                        //                        AbelDent["Medication_Description"].ToString().Trim() != Local["Medication_Description"].ToString().Trim() ||
                        //                        AbelDent["Medication_Notes"].ToString().Trim() != Local["Medication_Notes"].ToString().Trim() ||
                        //                        AbelDent["Medication_Sig"].ToString().ToUpper().Trim() != Local["Medication_Sig"].ToString().ToUpper().Trim() ||
                        //                        AbelDent["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim() != Local["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim() ||
                        //                        AbelDent["Medication_Type"].ToString().ToUpper().Trim() != Local["Medication_Type"].ToString().ToUpper().Trim() ||
                        //                        AbelDent["Allow_Generic_Sub"].ToString().ToUpper().Trim() != Local["Allow_Generic_Sub"].ToString().ToUpper().Trim() ||
                        //                        AbelDent["Refills"].ToString().ToUpper().Trim() != Local["Refills"].ToString().ToUpper().Trim() ||
                        //                        AbelDent["Is_Active"].ToString().ToUpper().Trim() != Local["Is_Active"].ToString().ToUpper().Trim() ||
                        //                        AbelDent["Medication_Provider_ID"].ToString().ToUpper().Trim() != Local["Medication_Provider_ID"].ToString().ToUpper().Trim()
                        //                        select AbelDent).ToList();

                        //DataTable dtPatientToBeUpdated = dtLocalMedication.Clone();
                        //if (itemsToBeUpdated.Count > 0)
                        //{
                        //    dtPatientToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
                        //}
                        //if (!dtPatientToBeUpdated.Columns.Contains("InsUptDlt"))
                        //{
                        //    dtPatientToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
                        //    dtPatientToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
                        //}
                        //if (dtPatientToBeUpdated.Rows.Count > 0)
                        //{
                        //    dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
                        //    dtSaveRecords.Load(dtPatientToBeUpdated.Select().CopyToDataTable().CreateDataReader());
                        //}

                        //Delete
                        var itemToBeDeleted = (from Local in dtLocalMedication.AsEnumerable()
                                               join AbelDent in dtMedication.AsEnumerable()
                                               on Local["PatientMedication_EHR_ID"].ToString().Trim() 
                                               equals AbelDent["PatientMedication_EHR_ID"].ToString().Trim()
                                               into matchingRows
                                               from matchingRow in matchingRows.DefaultIfEmpty()
                                               where Local["is_deleted"].ToString().Trim().ToUpper() == "FALSE" &&
                                                     matchingRow == null
                                               select Local).ToList();
                        DataTable dtPatientToBeDeleted = dtLocalMedication.Clone();
                        if (itemToBeDeleted.Count > 0)
                        {
                            dtPatientToBeDeleted = itemToBeDeleted.CopyToDataTable<DataRow>();
                        }
                        if (!dtPatientToBeDeleted.Columns.Contains("InsUptDlt"))
                        {
                            dtPatientToBeDeleted.Columns.Add("InsUptDlt", typeof(int));
                            dtPatientToBeDeleted.Columns["InsUptDlt"].DefaultValue = 0;
                        }

                        if (dtPatientToBeDeleted.Rows.Count > 0)
                        {
                            dtPatientToBeDeleted.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 3);
                            dtSaveRecords.Load(dtPatientToBeDeleted.Select().CopyToDataTable().CreateDataReader());
                        }


                        if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            bool status = SynchLocalBAL.Save_PatientMedication_EHR_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

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

        #region Sync Patient Document
        public void SynchDataLocalToAbelDent_Patient_Document()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    try
                    {
                        //live to local save data pull
                        SynchDataLiveDB_Pull_treatmentDoc();

                        //live to local save PDF pull
                        SyncTreatmentDocument();
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                        // throw;
                    }

                    GetPatientDocument("1");
                    GetPatientDocument_New("1");
                    SynchAbelDentBAL.Save_Document_in_AbelDent();

                    #region Treatment Document
                    try
                    {
                        SynchAbelDentBAL.Save_Treatment_Document_in_AbelDent();
                        #region change status as treatment doc impotred Completed
                        DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                        if (statusCompleted.Rows.Count > 0)
                        {
                            Change_Status_TreatmentDoc(statusCompleted, "Completed");
                        }
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Treatment_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }
                    #endregion
                    #endregion
                }
                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }
        #endregion

        #region Event Listener
        public static bool SynchDataLocalToAbelDent_Appointment(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id)
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

                DataTable dtAbelDentPatient = SynchAbelDentBAL.GetAbelDentPatientListData();

                SynchAbelDentBAL.GetAbelDentSystemData(ref _dayStartTime, ref _dayEndTime, ref _minutesInUnit, ref _workingDay);

                string tmpApptProv = "";
                int reqTime = 0;
                Int64 tmpPatient_id = 0;
                string PatientUniqID = string.Empty;
                string tmpAppt_EHR_id = "";
                //int tmpNewPatient = 1;
                DateTime ApptDateTime = DateTime.Now;
                DateTime ApptEndDateTime = DateTime.Now;

                string tmpLastName = "";
                string tmpFirstName = "";

                string TmpWebPatientName = "";
                string TmpWebRevPatientName = "";

                foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                {
                    tmpPatient_id = 0;
                    PatientUniqID = "";
                    tmpAppt_EHR_id = "";
                    TmpWebPatientName = "";
                    TmpWebRevPatientName = "";

                    Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);

                    tmpApptProv = dtDtxRow["Provider_EHR_ID"].ToString().Trim();

                    #region Set Operatory
                    string Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim();

                    DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                    DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                    TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;

                    int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);
                    reqTime = (tmpApptDurMinutes / _minutesInUnit);

                    DataTable dtBookOperatoryApptWiseDateTime = SynchAbelDentBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), _minutesInUnit);
                    string tmpIdealOperatory = "";
                    string appointment_EHR_id = "";

                    if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
                    {
                        string tmpCheckOpId = "";
                        bool IsConflict = false;
                        for (int i = 0; i < Operatory_EHR_IDs.Length; i++)
                        {
                            IsConflict = false;
                            tmpCheckOpId = Operatory_EHR_IDs[i].ToString();
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

                    #endregion

                    #region Set Patient
                    if (tmpIdealOperatory == "")
                    {
                        DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appointment_EHR_Id = " + appointment_EHR_id).CopyToDataTable();
                        bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());
                    }

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
                        tmpPatient_id = Convert.ToInt32(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtAbelDentPatient, PatientUniqID.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DBConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                    }
                    #endregion


                    TmpWebPatientName = SynchAbelDentBAL.GetPatientName(tmpPatient_id.ToString());
                    //PatientUniqID = SynchAbelDentBAL.GetPatientId(tmpPatient_id.ToString());
                    PatientUniqID = tmpPatient_id.ToString();


                    if (PatientUniqID != string.Empty || PatientUniqID != "" || tmpPatient_id != 0)
                    {
                        tmpAppt_EHR_id = SynchAbelDentBAL.Save_Appointment_Local_To_AbelDent(TmpWebPatientName, tmpStartTime, tmpEndTime, tmpPatient_id, PatientUniqID, Operatory_EHR_IDs, reqTime, "1", dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), tmpApptProv, "0", false, false, false, false, dtDtxRow["appt_treatmentcode"].ToString(), (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "1"));
                        if (tmpAppt_EHR_id != "")
                        {
                            bool isApptId_Update = SynchAbelDentBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                        }
                    }

                }
                //SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook();
                Utility.WritetoAditEventSyncLogFile_Static("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                return false;
            }
        }

        public static void SyncDataAbelDent_AppointmentFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        {
            try
            {
                SynchDataAbelDent_AppointmentsPatientFromEvent(strDbString, strPatID, Clinic_Number, Service_Install_Id);
                SyncDataAbelDent_PatientStatusFromEvent(strDbString, strPatID, Clinic_Number, Service_Install_Id);
                SynchDataLiveDB_Push_Patient(strPatID);
                SynchAbelDentBAL.GetAbelDentSystemData(ref _dayStartTime, ref _dayEndTime, ref _minutesInUnit, ref _workingDay);

                DataTable dtAbelDentAppointment = SynchAbelDentBAL.GetAbelDentAppointmentData(strApptID, _minutesInUnit.ToString());

                dtAbelDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                dtAbelDentAppointment.Columns["InsUptDlt"].DefaultValue = 0;
                dtAbelDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                dtAbelDentAppointment.Columns.Add("ProcedureCode", typeof(string));


                dtAbelDentAppointment.Columns["ProcedureCode"].DefaultValue = "";
                dtAbelDentAppointment.Columns["ProcedureDesc"].DefaultValue = "";


                DataTable DtAbelDentAppointment_Procedures_Data = SynchAbelDentBAL.GetAbelDentAppointment_Procedures_Data(strApptID);
                string ProcedureDesc = "";
                string ProcedureCode = "";

                foreach (DataRow dtDtxRow in dtAbelDentAppointment.Rows)
                {
                    ProcedureDesc = "";
                    ProcedureCode = "";
                    DataRow[] dtCurApptProcedure = DtAbelDentAppointment_Procedures_Data.Select("Appointment_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");
                    foreach (var dtSinProc in dtCurApptProcedure.ToList())
                    {
                        ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                        ProcedureDesc = ProcedureDesc + dtSinProc["ProcedureDesc"].ToString().Trim();
                    }
                    dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                    dtDtxRow["ProcedureCode"] = ProcedureCode;

                }

                DataTable dtLocalAppointment = new DataTable();
                DataTable dtTempResult = SynchLocalBAL.GetLocalAppointmentData(Service_Install_Id, strApptID);
                if (dtTempResult.Rows.Count > 0)
                {
                    try
                    {
                        //dtLocalAppointment = dtTempResult.Clone();
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

                dtLocalAppointment = CompareDataTableRecords_Static(ref dtAbelDentAppointment, dtLocalAppointment, "Appt_EHR_ID", "Appt_LocalDB_ID", "Appt_LocalDB_ID,Appt_Web_ID,Is_Adit_Updated,EHR_Entry_DateTime,Last_Sync_Date,InsUptDlt,Remind_DateTime,Patient_Status,Is_Appt_DoubleBook,InsuranceCompanyName,is_Status_Updated_From_Web,is_ehr_updated,Entry_DateTime,is_asap,Is_Appt,Clinic_Number,Service_Install_Id");

                dtAbelDentAppointment.AcceptChanges();
                dtLocalAppointment.AcceptChanges();
                bool status = false;
                DataTable dtSaveRecords = dtAbelDentAppointment.Clone();
                if (dtAbelDentAppointment.Select("InsUptDlt IN (1,2,3)").Count() > 0 || dtLocalAppointment.Select("InsUptDlt IN (3)").Count() > 0)
                {
                    if (dtAbelDentAppointment.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                    {
                        foreach (DataRow drrow in dtAbelDentAppointment.Select("InsUptDlt IN (1,2,3)"))
                        {
                            if (drrow["birth_date"].ToString() == "" || drrow["birth_date"] == string.Empty)
                            {
                                drrow["birth_date"] = DBNull.Value;
                                // dtSaveRecords.Rows.Add(drrow);
                            }
                        }
                        dtSaveRecords.Load(dtAbelDentAppointment.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
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
                        }
                        dtSaveRecords.Load(dtLocalAppointment.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                    }
                    status = SynchAbelDentBAL.Save_AbelDent_To_Local(dtSaveRecords, "Appointment", "Appt_LocalDB_ID,Appt_Web_ID", "Appt_LocalDB_ID");
                }
                else
                {
                    if (dtAbelDentAppointment.Select("InsUptDlt IN (4)").Count() > 0)
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
                Utility.WritetoAditEventErrorLogFile_Static("[SyncDataAbelDent_AppointmentFromEvent Sync (AbelDent to Local Database) ]" + ex.Message);
            }
        }

        public static void SynchDataAbelDent_AppointmentsPatientFromEvent(string strDbString, string strPatID, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                DataTable dtAbelDentPatientList = SynchAbelDentBAL.GetAbelDentAppointmentsPatientData(strPatID);
                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Service_Install_Id, strPatID);

                DataTable dtSaveRecords = new DataTable();
                dtSaveRecords = dtLocalPatient.Clone();

                var itemsToBeUpdated = (from AbelDentPatient in dtAbelDentPatientList.AsEnumerable()
                                        join LocalPatient in dtLocalPatient.AsEnumerable()
                                        on AbelDentPatient["Patient_EHR_ID"].ToString().Trim() + "_" + AbelDentPatient["Clinic_Number"].ToString().Trim()
                                        equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                        where
                                         Convert.ToDateTime(AbelDentPatient["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(LocalPatient["EHR_Entry_DateTime"])
                                         ||
                                         (AbelDentPatient["nextvisit_date"] != DBNull.Value && AbelDentPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(AbelDentPatient["nextvisit_date"]) : DateTime.Now)
                                         !=
                                         (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)
                                         ||
                                         (AbelDentPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())
                                         ||
                                         (AbelDentPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())
                                         || (AbelDentPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                         || (AbelDentPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                         || (AbelDentPatient["Home_Phone"].ToString().Trim()) != (LocalPatient["Home_Phone"].ToString().Trim())
                                         || (AbelDentPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                         || (AbelDentPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                         || (AbelDentPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                         || (AbelDentPatient["Mobile"].ToString().Trim()) != (LocalPatient["Mobile"].ToString().Trim())
                                         || (AbelDentPatient["ReceiveSMS"].ToString().Trim()) != (LocalPatient["ReceiveSMS"].ToString().Trim())
                                         || (AbelDentPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
                                        select AbelDentPatient).ToList();

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
                    bool status = SynchAbelDentBAL.Save_Patient_AbelDent_To_Local_New(dtSaveRecords, Clinic_Number, Service_Install_Id);
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataAbelDent_AppointmentsPatientFromEvent Sync (AbelDent to Local Database) ]" + ex.Message);
            }
        }

        public static void SyncDataAbelDent_PatientStatusFromEvent(string strDbString, string strPatID, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                #region "Patient Status"
                DataTable dtAbelDentPatientStatus = new DataTable();
                dtAbelDentPatientStatus = SynchAbelDentBAL.GetAbelDentPatientStatusData(strPatID);
                if (dtAbelDentPatientStatus != null && dtAbelDentPatientStatus.Rows.Count > 0)
                {
                    SynchLocalBAL.UpdatePatient_Status(dtAbelDentPatientStatus, Service_Install_Id, Clinic_Number, strPatID);
                    //SynchDataLiveDB_Push_PatientStatus(Convert.ToInt32(Service_Install_Id), Convert.ToInt32(Clinic_Number), strPatID);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncAbelDent_PatientStatus Sync (AbelDent to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataLocalToAbelDent_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
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

                dtWebPatient_Form.Columns.Add("Table_Name", typeof(string));
                dtWebPatient_Form.Columns["Table_Name"].DefaultValue = "pat";
                dtWebPatient_Form.Columns.Add("pformfield", typeof(string));

                for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                {

                    foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                    {
                        if (dtDtxRow["ehrfield"].ToString().Trim() == "preferred_name")
                        {
                            dtDtxRow["ehrfield"] = "infgivenname";
                            dtDtxRow["Table_Name"] = "inf";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "first_name")
                        {
                            dtDtxRow["ehrfield"] = "pfname";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "last_name")
                        {
                            dtDtxRow["ehrfield"] = "plname";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "middle_name")
                        {
                            dtDtxRow["ehrfield"] = "pinitial";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "sex")
                        {
                            dtDtxRow["ehrfield"] = "pgender";
                            if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                            {
                                dtDtxRow["ehrfield_value"] = "F";
                            }
                            else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MALE")
                            {
                                dtDtxRow["ehrfield_value"] = "M";
                            }
                            else
                            {
                                dtDtxRow["ehrfield_value"] = " ";
                            }
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_one")
                        {
                            dtDtxRow["ehrfield"] = "pstreetadr";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_two")
                        {
                            dtDtxRow["ehrfield"] = "pstreetadr2";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "zipcode")
                        {
                            dtDtxRow["ehrfield"] = "ppostal";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "city")
                        {
                            dtDtxRow["ehrfield"] = "pcitycode";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "email")
                        {
                            dtDtxRow["ehrfield"] = "infemail";
                            dtDtxRow["Table_Name"] = "inf";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_email")
                        {
                            dtDtxRow["ehrfield"] = "infallownewsemail";
                            dtDtxRow["Table_Name"] = "inf";
                            if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO" || dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "N")
                            {
                                dtDtxRow["ehrfield_value"] = "0";
                            }
                            else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "YES" || dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "Y")
                            {
                                dtDtxRow["ehrfield_value"] = "1";
                            }
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "work_phone")
                        {
                            dtDtxRow["ehrfield"] = "pworkphn";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "mobile")
                        {
                            dtDtxRow["ehrfield"] = "infmobile";
                            dtDtxRow["Table_Name"] = "inf";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "home_phone")
                        {
                            dtDtxRow["ehrfield"] = "pphone";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_sms")
                        {
                            dtDtxRow["ehrfield"] = "infallowtextmsg";
                            dtDtxRow["Table_Name"] = "inf";
                            if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO")
                            {
                                dtDtxRow["ehrfield_value"] = "0";
                            }
                            else
                            {
                                dtDtxRow["ehrfield_value"] = "1";
                            }
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
                        {
                            dtDtxRow["ehrfield"] = "pdentist";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
                        {
                            dtDtxRow["ehrfield"] = "phygienist";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SALUTATION")
                        {
                            dtDtxRow["ehrfield"] = "pmrmrs";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance")
                        {
                            dtDtxRow["ehrfield"] = "ixiplanid_pri_ins";
                            dtDtxRow["Table_Name"] = "ixi";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance")
                        {
                            dtDtxRow["ehrfield"] = "ixiplanid_sec_ins";
                            dtDtxRow["Table_Name"] = "ixi";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                        {
                            dtDtxRow["ehrfield"] = "ixipid_pri_sub";
                            dtDtxRow["Table_Name"] = "ixi";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                        {
                            dtDtxRow["ehrfield"] = "ixipid_sec_sub";
                            dtDtxRow["Table_Name"] = "ixi";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "birth_date")
                        {
                            dtDtxRow["ehrfield"] = "pbirth";
                            dtDtxRow["Table_Name"] = "pat";
                        }
                        else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "employer"))
                        {
                            dtDtxRow["ehrfield"] = "infemployer";
                            dtDtxRow["Table_Name"] = "inf";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance_companyname")
                        {
                            dtDtxRow["ehrfield"] = "pri_insurance_companyname";
                            dtDtxRow["Table_Name"] = "nsp";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance_companyname")
                        {
                            dtDtxRow["ehrfield"] = "sec_insurance_companyname";
                            dtDtxRow["Table_Name"] = "nsp";
                        }

                        dtWebPatient_Form.AcceptChanges();
                    }
                    if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                    {
                        bool Is_Record_Update = SynchAbelDentBAL.Save_Patient_Form_Local_To_AbelDent(dtWebPatient_Form);
                    }

                    try
                    {
                        if (SynchAbelDentBAL.SavePatientAllergies_To_AbelDent())
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient_Allergies Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Allergies Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Allergies Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
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

                    bool isRecordSaved = false;
                    bool isSaveMedication = false;
                    string Patient_EHR_IDS = "";
                    string SavePatientEHRID = "";

                    try
                    {
                        SynchAbelDentBAL.SavePatientMedicationLocalToAbelDent(ref isRecordSaved, ref SavePatientEHRID, strPatientFormID);
                    }
                    catch (Exception exMed)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + exMed.Message);
                    }
                    if (isSaveMedication)
                    {
                        Patient_EHR_IDS = (SavePatientEHRID).TrimEnd(',');
                        if (Patient_EHR_IDS != "")
                        {
                            SynchDataAbelDent_PatientMedication(Patient_EHR_IDS);
                        }
                    }

                    Is_synched_PatinetForm = false;
                    try
                    {
                        GetMedicalAbelDentHistoryRecords();
                        SynchAbelDentBAL.SaveMedicalHistoryLocalToAbelDent();
                        ObjGoalBase.WriteToSyncLogFile("Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }

                    SynchDataLocalToAbelDent_Patient_Document();
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
        #endregion

        //  }
    }
}
