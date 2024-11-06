using Pozative.BAL;
using Pozative.UTL;
using System;
using AditOfficeMate.BAL;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Pozative.DAL;
using Pozative.BO;
using RestSharp;
using System.Net;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable
        bool IsOfficeMateProviderSync = false;
        bool IsOfficeMateOperatorySync = false;
        bool IsOfficeMateApptTypeSync = false;

        private BackgroundWorker bwSynchOfficeMate_Appointment = null;
        private System.Timers.Timer timerSynchOfficeMate_Appointment = null;

        private BackgroundWorker bwSynchOfficeMate_Operatory = null;
        private System.Timers.Timer timerSynchOfficeMate_Operatory = null;

        private BackgroundWorker bwSynchOfficeMate_OperatoryEvent = null;
        private System.Timers.Timer timerSynchOfficeMate_OperatoryEvent = null;

        private BackgroundWorker bwSynchOfficeMate_Provider = null;
        private System.Timers.Timer timerSynchOfficeMate_Provider = null;

        private BackgroundWorker bwSynchOfficeMate_Speciality = null;
        private System.Timers.Timer timerSynchOfficeMate_Speciality = null;

        private BackgroundWorker bwSynchOfficeMate_ApptType = null;
        private System.Timers.Timer timerSynchOfficeMate_ApptType = null;

        private BackgroundWorker bwSynchOfficeMate_Patient = null;
        private System.Timers.Timer timerSynchOfficeMate_Patient = null;

        private BackgroundWorker bwSynchOfficeMate_RecallType = null;
        private System.Timers.Timer timerSynchOfficeMate_RecallType = null;

        private BackgroundWorker bwSynchOfficeMate_User = null;
        private System.Timers.Timer timerSynchOfficeMate_User = null;

        private BackgroundWorker bwSynchOfficeMate_ApptStatus = null;
        private System.Timers.Timer timerSynchOfficeMate_ApptStatus = null;

        private BackgroundWorker bwSynchOfficeMate_OrderStatus = null;
        private System.Timers.Timer timerSynchOfficeMate_OrderStatus = null;

        private BackgroundWorker bwSynchOfficeMate_Disease = null;
        private System.Timers.Timer timerSynchOfficeMate_Disease = null;

        private BackgroundWorker bwSynchLocalToOfficeMate_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchLocalToOfficeMate_Appointment = null;

        private BackgroundWorker bwSynchLocalToOfficeMate_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToOfficeMate_Patient_Form = null;

        private BackgroundWorker bwSynchOfficeMate_ProviderHours = null;
        private System.Timers.Timer timerSynchOfficeMate_ProviderHours = null;

        private BackgroundWorker bwSynchOfficeMate_Provider_OfficeHours = null;
        private System.Timers.Timer timerSynchOfficeMate_Provider_OfficeHours = null;

        private BackgroundWorker bwSynchOfficeMate_MedicalHistory = null;
        private System.Timers.Timer timerSynchOfficeMate_MedicalHistory = null;

        private BackgroundWorker bwSynchOfficeMate_PatientPayment = null;
        private System.Timers.Timer timerSynchOfficeMate_PatientPayment = null;

        private BackgroundWorker bwSynchOfficeMate_Holiday = null;
        private System.Timers.Timer timerSynchOfficeMate_Holiday = null;

        #endregion

        private void CallSynchOfficeMateToLocal()
        {
            if (Utility.AditSync)
            {
                //fncSynchDataLocalToOfficeMate_Appointment();
                //SynchDataOfficeMate_ProviderHours();


                fncSynchDataOfficeMate_PatientPayment();

                SynchDataOfficeMate_ApptStatus();
                fncSynchDataOfficeMate_ApptStatus();

                SynchDataOfficeMate_ApptType();
                fncSynchDataOfficeMate_ApptType();

                SynchDataOfficeMate_Provider();
                fncSynchDataOfficeMate_Provider();

                SynchDataOfficeMate_Operatory();
                fncSynchDataOfficeMate_Operatory();

                SynchDataOfficeMate_Speciality();
                fncSynchDataOfficeMate_Speciality();

                fncSynchDataLocalToOfficeMate_Appointment();

                SynchDataOfficeMate_Patient();
                fncSynchDataOfficeMate_Patient();

                SynchDataOfficeMate_Appointment();
                fncSynchDataOfficeMate_Appointment();

                SynchDataOfficeMate_RecallType();
                fncSynchDataOfficeMate_RecallType();

                SynchDataOfficeMate_OperatoryEvent();
                fncSynchDataOfficeMate_OperatoryEvent();

                SynchDataOfficeMate_ProviderHours();
                fncSynchDataOfficeMate_ProviderHours();

                SynchDataOfficeMate_Provider_OfficeHours();
                fncSynchDataOfficeMate_Provider_OfficeHours();

                SynchDataLocalToOfficeMate_Patient_Form();
                fncSynchDataLocalToOfficeMate_Patient_Form();

                SynchDataOfficeMate_Disease();
                fncSynchDataOfficeMate_Disease();

                

                SynchDataOfficeMate_User();
                fncSynchDataOfficeMate_User();

                SynchDataOfficeMate_OrderStatus();
                fncSynchDataOfficeMate_OrderStatus();
            }
        }

        #region Synch Appointment

        private void fncSynchDataOfficeMate_Appointment()
        {
            InitBgWorkerOfficeMate_Appointment();
            InitBgTimerOfficeMate_Appointment();
        }

        private void InitBgTimerOfficeMate_Appointment()
        {
            timerSynchOfficeMate_Appointment = new System.Timers.Timer();
            this.timerSynchOfficeMate_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchOfficeMate_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_Appointment_Tick);
            timerSynchOfficeMate_Appointment.Enabled = true;
            timerSynchOfficeMate_Appointment.Start();
            timerSynchOfficeMate_Appointment_Tick(null, null);
        }

        private void InitBgWorkerOfficeMate_Appointment()
        {
            bwSynchOfficeMate_Appointment = new BackgroundWorker();
            bwSynchOfficeMate_Appointment.WorkerReportsProgress = true;
            bwSynchOfficeMate_Appointment.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_Appointment.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOfficeMate_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_Appointment_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_Appointment.Enabled = false;
                MethodForCallSynchOrderOfficeMate_Appointment();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_Appointment()
        {
            System.Threading.Thread procThreadmainOfficeMate_Appointment = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_Appointment);
            procThreadmainOfficeMate_Appointment.Start();
        }

        public void CallSyncOrderTableOfficeMate_Appointment()
        {
            if (bwSynchOfficeMate_Appointment.IsBusy != true)
            {
                bwSynchOfficeMate_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLiveDB_PatientPaymentLog_LocalTOOfficeMate();
                SynchDataOfficeMate_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_Appointment.Enabled = true;
        }

        public void SynchDataOfficeMate_Appointment()
        {
            try
            {
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    Is_synched_Appointment = false;
                    DateTime ToDate = Utility.LastSyncDateAditServer;
                    Cls_Synch_Appointment.SynchDataOfficeMate_Appointment(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.LastSyncDateAditServer, Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                    ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment_Push");
                    GoalBase.WriteToSyncLogFile_Static("Appointment Sync (Local Database To Adit Server) Successfully.");
                }
                Is_synched_Appointment = true;
                IsEHRAllSync = true;
                try
                {
                    SynchDataOfficeMate_Order();
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Order Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message.ToString() + ex.InnerException != null ? ex.InnerException.ToString() : " ");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message.ToString() + ex.InnerException != null ? ex.InnerException.ToString() : " ");
            }
            finally
            {
                //SynchDataLiveDB_Push_Appointment();
                Is_synched_Appointment = true;
            }
        }

        #endregion

        #region Synch Operatory

        private void fncSynchDataOfficeMate_Operatory()
        {
            InitBgWorkerOfficeMate_Operatory();
            InitBgTimerOfficeMate_Operatory();
        }

        private void InitBgTimerOfficeMate_Operatory()
        {
            timerSynchOfficeMate_Operatory = new System.Timers.Timer();
            this.timerSynchOfficeMate_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchOfficeMate_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_Operatory_Tick);
            timerSynchOfficeMate_Operatory.Enabled = true;
            timerSynchOfficeMate_Operatory.Start();
        }

        private void InitBgWorkerOfficeMate_Operatory()
        {
            bwSynchOfficeMate_Operatory = new BackgroundWorker();
            bwSynchOfficeMate_Operatory.WorkerReportsProgress = true;
            bwSynchOfficeMate_Operatory.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_Operatory.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_Operatory_DoWork);
            bwSynchOfficeMate_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_Operatory_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_Operatory.Enabled = false;
                MethodForCallSynchOrderOfficeMate_Operatory();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_Operatory()
        {
            System.Threading.Thread procThreadmainOfficeMate_Operatory = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_Operatory);
            procThreadmainOfficeMate_Operatory.Start();
        }

        public void CallSyncOrderTableOfficeMate_Operatory()
        {
            if (bwSynchOfficeMate_Operatory.IsBusy != true)
            {
                bwSynchOfficeMate_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_Operatory.Enabled = true;
        }

        public void SynchDataOfficeMate_Operatory()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        AditOfficeMate.BAL.Cls_Synch_Operatory.SynchDataOfficeMate_Operatory(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                        Utility.WriteToSyncLogFile_All("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchLocalBAL.Update_Sync_Table_Datetime("Operatory_Push");
                        Utility.WriteToSyncLogFile_All("Operatory Sync (Local Database To Adit Server) Successfully.");
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }
        #endregion

        #region Synch Provider

        private void fncSynchDataOfficeMate_Provider()
        {
            InitBgWorkerOfficeMate_Provider();
            InitBgTimerOfficeMate_Provider();
        }

        private void InitBgTimerOfficeMate_Provider()
        {
            timerSynchOfficeMate_Provider = new System.Timers.Timer();
            this.timerSynchOfficeMate_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchOfficeMate_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_Provider_Tick);
            timerSynchOfficeMate_Provider.Enabled = true;
            timerSynchOfficeMate_Provider.Start();
        }

        private void InitBgWorkerOfficeMate_Provider()
        {
            bwSynchOfficeMate_Provider = new BackgroundWorker();
            bwSynchOfficeMate_Provider.WorkerReportsProgress = true;
            bwSynchOfficeMate_Provider.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_Provider.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOfficeMate_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_Provider_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_Provider.Enabled = false;
                MethodForCallSynchOrderOfficeMate_Provider();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_Provider()
        {
            System.Threading.Thread procThreadmainOfficeMate_Provider = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_Provider);
            procThreadmainOfficeMate_Provider.Start();
        }

        public void CallSyncOrderTableOfficeMate_Provider()
        {
            if (bwSynchOfficeMate_Provider.IsBusy != true)
            {
                bwSynchOfficeMate_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_Provider();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_Provider.Enabled = true;
        }

        public void SynchDataOfficeMate_Provider()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Provider.SynchDataOfficeMate_Provider(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                        ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        IsOfficeMateProviderSync = true;
                        UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider_Push");
                        GoalBase.WriteToSyncLogFile_Static("Providers Sync (Local Database To Adit Server) Successfully.");
                        IsProviderSyncPush = true;
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
            SynchDataOfficeMate_Medication();
            SynchDataOfficeMate_Insurance();
        }
        

        #endregion

        #region Synch Speciality

        private void fncSynchDataOfficeMate_Speciality()
        {
            InitBgWorkerOfficeMate_Speciality();
            InitBgTimerOfficeMate_Speciality();
        }

        private void InitBgTimerOfficeMate_Speciality()
        {
            timerSynchOfficeMate_Speciality = new System.Timers.Timer();
            this.timerSynchOfficeMate_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchOfficeMate_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_Speciality_Tick);
            timerSynchOfficeMate_Speciality.Enabled = true;
            timerSynchOfficeMate_Speciality.Start();
        }

        private void InitBgWorkerOfficeMate_Speciality()
        {
            bwSynchOfficeMate_Speciality = new BackgroundWorker();
            bwSynchOfficeMate_Speciality.WorkerReportsProgress = true;
            bwSynchOfficeMate_Speciality.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_Speciality.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_Speciality_DoWork);
            bwSynchOfficeMate_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_Speciality_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_Speciality.Enabled = false;
                MethodForCallSynchOrderOfficeMate_Speciality();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_Speciality()
        {
            System.Threading.Thread procThreadmainOfficeMate_Speciality = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_Speciality);
            procThreadmainOfficeMate_Speciality.Start();
        }

        public void CallSyncOrderTableOfficeMate_Speciality()
        {
            if (bwSynchOfficeMate_Speciality.IsBusy != true)
            {
                bwSynchOfficeMate_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_Speciality();
            }
            catch (Exception ex)
            {
                //ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_Speciality.Enabled = true;
        }

        public void SynchDataOfficeMate_Speciality()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {

                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        if (Cls_Synch_Speciality.SynchDataOfficeMate_EHRSpeciality(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString()))
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality");
                            ObjGoalBase.WriteToSyncLogFile("Speciality Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality_Push");
                            GoalBase.WriteToSyncLogFile_Static("Speciality Sync (Local Database To Adit Server) Successfully.");
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

        #region Synch OperatoryEvent

        private void fncSynchDataOfficeMate_OperatoryEvent()
        {
            InitBgWorkerOfficeMate_OperatoryEvent();
            InitBgTimerOfficeMate_OperatoryEvent();
        }

        private void InitBgTimerOfficeMate_OperatoryEvent()
        {
            timerSynchOfficeMate_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchOfficeMate_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchOfficeMate_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_OperatoryEvent_Tick);
            timerSynchOfficeMate_OperatoryEvent.Enabled = true;
            timerSynchOfficeMate_OperatoryEvent.Start();
            timerSynchOfficeMate_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerOfficeMate_OperatoryEvent()
        {
            bwSynchOfficeMate_OperatoryEvent = new BackgroundWorker();
            bwSynchOfficeMate_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchOfficeMate_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOfficeMate_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderOfficeMate_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainOfficeMate_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_OperatoryEvent);
            procThreadmainOfficeMate_OperatoryEvent.Start();
        }

        public void CallSyncOrderTableOfficeMate_OperatoryEvent()
        {
            if (bwSynchOfficeMate_OperatoryEvent.IsBusy != true)
            {
                bwSynchOfficeMate_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_OperatoryEvent();
                //SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                //SynchDataLiveDB_Pull_PatientFollowUp();
                // SynchDataLiveDB_PatientSMSCallLog_LocalTOOfficeMate();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_OperatoryEvent.Enabled = true;
        }

        public void SynchDataOfficeMate_OperatoryEvent()
        {
            if (IsOfficeMateProviderSync && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_OperatoryEvent.SynchDataOfficeMate_OperatoryEvent(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.LastSyncDateAditServer);
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
                        ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        IsEHRAllSync = true;
                        UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent_Push");
                        GoalBase.WriteToSyncLogFile_Static("OperatoryEvent Sync (Local Database To Adit Server) Successfully.");
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message.ToString() + ex.InnerException != null ? ex.InnerException.ToString() : " ");
                }
            }
        }

        #endregion

        #region Provider Hours

        private void fncSynchDataOfficeMate_ProviderHours()
        {
            //SynchDataOfficeMate_ProviderHours();
            //SynchDataOfficeMateToLocal_ProviderOfficeHours();
            InitBgWorkerOfficeMate_ProviderHours();
            InitBgTimerOfficeMate_ProviderHours();
        }

        private void InitBgTimerOfficeMate_ProviderHours()
        {
            timerSynchOfficeMate_ProviderHours = new System.Timers.Timer();
            this.timerSynchOfficeMate_ProviderHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchOfficeMate_ProviderHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_ProviderHours_Tick);
            timerSynchOfficeMate_ProviderHours.Enabled = true;
            timerSynchOfficeMate_ProviderHours.Start();
        }

        private void InitBgWorkerOfficeMate_ProviderHours()
        {
            bwSynchOfficeMate_ProviderHours = new BackgroundWorker();
            bwSynchOfficeMate_ProviderHours.WorkerReportsProgress = true;
            bwSynchOfficeMate_ProviderHours.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_ProviderHours.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_ProviderHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOfficeMate_ProviderHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_ProviderHours_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_ProviderHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_ProviderHours.Enabled = false;
                MethodForCallSynchOrderOfficeMate_ProviderHours();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_ProviderHours()
        {
            System.Threading.Thread procThreadmainOfficeMate_ProviderHours = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_ProviderHours);
            procThreadmainOfficeMate_ProviderHours.Start();
        }

        public void CallSyncOrderTableOfficeMate_ProviderHours()
        {
            if (bwSynchOfficeMate_ProviderHours.IsBusy != true)
            {
                bwSynchOfficeMate_ProviderHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_ProviderHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_ProviderHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SyncPullLogsAndSaveinOfficeMate();
                //SynchDataOfficeMateToLocal_ProviderOfficeHours();
                SynchDataOfficeMate_ProviderHours();
                // CallSynch_ProviderHoursHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SyncPullLogsAndSaveinOfficeMate()
        {
            try
            {
                CheckCustomhoursForProviderOperatory();
                //SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                //SynchDataLiveDB_Pull_PatientFollowUp();
                //SynchDataLiveDB_PatientSMSCallLog_LocalTOOfficeMate();
                //fncPaymentSMSCallStatusUpdate();
                SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
            }
            catch (Exception)
            {

            }
        }

        private void bwSynchOfficeMate_ProviderHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_ProviderHours.Enabled = true;
        }

        public void SynchDataOfficeMate_ProviderHours()
        {
            CallSynchOfficeMate_ProviderHours();

        }

        private void CallSynchOfficeMate_ProviderHours()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)//&& Utility.is_scheduledCustomhour)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_ProviderHours.SynchDataOfficeMate_ProviderHours(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.LastSyncDateAditServer);

                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
                        ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    }
                    //CallSynch_OfficeMateProviderChair();
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ProviderHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
            }
        }
        #endregion

        #region Provider Office Hours

        private void fncSynchDataOfficeMate_Provider_OfficeHours()
        {
            InitBgWorkerOfficeMate_Provider_OfficeHours();
            InitBgTimerOfficeMate_Provider_OfficeHours();
        }

        private void InitBgTimerOfficeMate_Provider_OfficeHours()
        {
            timerSynchOfficeMate_Provider_OfficeHours = new System.Timers.Timer();
            this.timerSynchOfficeMate_Provider_OfficeHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider_OfficeHours;
            this.timerSynchOfficeMate_Provider_OfficeHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_Provider_OfficeHours_Tick);
            timerSynchOfficeMate_Provider_OfficeHours.Enabled = true;
            timerSynchOfficeMate_Provider_OfficeHours.Start();
        }
        private void InitBgWorkerOfficeMate_Provider_OfficeHours()
        {
            bwSynchOfficeMate_Provider_OfficeHours = new BackgroundWorker();
            bwSynchOfficeMate_Provider_OfficeHours.WorkerReportsProgress = true;
            bwSynchOfficeMate_Provider_OfficeHours.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_Provider_OfficeHours.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_Provider_OfficeHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOfficeMate_Provider_OfficeHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_Provider_OfficeHours_RunWorkerCompleted);
        }
        private void timerSynchOfficeMate_Provider_OfficeHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_Provider_OfficeHours.Enabled = false;
                MethodForCallSynchOrderOfficeMate_Provider_OfficeHours();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_Provider_OfficeHours()
        {
            System.Threading.Thread procThreadmainOfficeMate_Provider_OfficeHours = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_Provider_OfficeHours);
            procThreadmainOfficeMate_Provider_OfficeHours.Start();
        }

        public void CallSyncOrderTableOfficeMate_Provider_OfficeHours()
        {
            if (bwSynchOfficeMate_Provider_OfficeHours.IsBusy != true)
            {
                bwSynchOfficeMate_Provider_OfficeHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_Provider_OfficeHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_Provider_OfficeHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_Provider_OfficeHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_Provider_OfficeHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_Provider_OfficeHours.Enabled = true;
        }

        public void SynchDataOfficeMate_Provider_OfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)// && Utility.is_scheduledCustomhour)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_ProviderOfficeHours.SynchDataOfficeMate_ProviderOfficeHours(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.LastSyncDateAditServer);
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderOfficeHours");
                        ObjGoalBase.WriteToSyncLogFile("ProviderOfficeHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        #endregion 

        #region Synch Patient

        private void fncSynchDataOfficeMate_Patient()
        {
            InitBgWorkerOfficeMate_Patient();
            InitBgTimerOfficeMate_Patient();
        }

        private void InitBgTimerOfficeMate_Patient()
        {
            timerSynchOfficeMate_Patient = new System.Timers.Timer();
            this.timerSynchOfficeMate_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchOfficeMate_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_Patient_Tick);
            timerSynchOfficeMate_Patient.Enabled = true;
            timerSynchOfficeMate_Patient.Start();
            timerSynchOfficeMate_Patient_Tick(null, null);
        }

        private void InitBgWorkerOfficeMate_Patient()
        {
            bwSynchOfficeMate_Patient = new BackgroundWorker();
            bwSynchOfficeMate_Patient.WorkerReportsProgress = true;
            bwSynchOfficeMate_Patient.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_Patient.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_Patient_DoWork);
            bwSynchOfficeMate_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_Patient_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_Patient.Enabled = false;
                MethodForCallSynchOrderOfficeMate_Patient();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_Patient()
        {
            System.Threading.Thread procThreadmainOfficeMate_Patient = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_Patient);
            procThreadmainOfficeMate_Patient.Start();
        }

        public void CallSyncOrderTableOfficeMate_Patient()
        {
            if (bwSynchOfficeMate_Patient.IsBusy != true)
            {
                bwSynchOfficeMate_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_Patient();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_Patient.Enabled = true;
        }

        public void SynchDataOfficeMate_Patient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;

                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Patient.SynchDataOfficeMate_Patient(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());

                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                        ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                        GoalBase.WriteToSyncLogFile_Static("Patient Sync (Local Database To Adit Server) Successfully.");
                        IsGetParientRecordDone = true;
                    }

                    IsPatientSyncedFirstTime = true;
                    Is_synched_Patient = false;
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


        public void SynchDataOfficeMate_AppointmentsPatient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    Is_synched_AppointmentsPatient = false;
                    ObjGoalBase.WriteToErrorLogFile("[Apointment Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        public void SynchDataOfficeMate_PatientImages()
        {
            try
            {
                if (!Is_synched_PatientImages && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    Is_synched_PatientImages = true;
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_PatientImage.SynchDataOfficeMate_PatientImage(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Images");
                        ObjGoalBase.WriteToSyncLogFile("PatientImage Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    }
                    Is_synched_PatientImages = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatientImages = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientImages Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }



        #endregion

        #region Synch RecallType

        private void fncSynchDataOfficeMate_RecallType()
        {
            InitBgWorkerOfficeMate_RecallType();
            InitBgTimerOfficeMate_RecallType();
        }

        private void InitBgTimerOfficeMate_RecallType()
        {
            timerSynchOfficeMate_RecallType = new System.Timers.Timer();
            this.timerSynchOfficeMate_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchOfficeMate_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_RecallType_Tick);
            timerSynchOfficeMate_RecallType.Enabled = true;
            timerSynchOfficeMate_RecallType.Start();
        }

        private void InitBgWorkerOfficeMate_RecallType()
        {
            bwSynchOfficeMate_RecallType = new BackgroundWorker();
            bwSynchOfficeMate_RecallType.WorkerReportsProgress = true;
            bwSynchOfficeMate_RecallType.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_RecallType.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_RecallType_DoWork);
            bwSynchOfficeMate_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_RecallType_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_RecallType.Enabled = false;
                MethodForCallSynchOrderOfficeMate_RecallType();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_RecallType()
        {
            System.Threading.Thread procThreadmainOfficeMate_RecallType = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_RecallType);
            procThreadmainOfficeMate_RecallType.Start();
        }

        public void CallSyncOrderTableOfficeMate_RecallType()
        {
            if (bwSynchOfficeMate_RecallType.IsBusy != true)
            {
                bwSynchOfficeMate_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_RecallType.Enabled = true;
        }

        public void SynchDataOfficeMate_RecallType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Recall_Type.SynchDataOfficeMate_EHRRecall_Types(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
                        ObjGoalBase.WriteToSyncLogFile("RecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType_Push");
                        GoalBase.WriteToSyncLogFile_Static("RecallType Sync (Local Database To Adit Server) Successfully.");
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
        private void fncSynchDataOfficeMate_User()
        {
            InitBgWorkerOfficeMate_User();
            InitBgTimerOfficeMate_User();
        }

        private void InitBgTimerOfficeMate_User()
        {
            timerSynchOfficeMate_User = new System.Timers.Timer();
            this.timerSynchOfficeMate_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchOfficeMate_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_User_Tick);
            timerSynchOfficeMate_User.Enabled = true;
            timerSynchOfficeMate_User.Start();
        }

        private void timerSynchOfficeMate_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_User.Enabled = false;
                MethodForCallSynchOrderOfficeMate_User();
            }
        }

        private void MethodForCallSynchOrderOfficeMate_User()
        {
            System.Threading.Thread procThreadmainOfficeMate_User = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_User);
            procThreadmainOfficeMate_User.Start();
        }

        private void CallSyncOrderTableOfficeMate_User()
        {
            if (bwSynchOfficeMate_User.IsBusy != true)
            {
                bwSynchOfficeMate_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void InitBgWorkerOfficeMate_User()
        {
            bwSynchOfficeMate_User = new BackgroundWorker();
            bwSynchOfficeMate_User.WorkerReportsProgress = true;
            bwSynchOfficeMate_User.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_User.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_User_DoWork);
            bwSynchOfficeMate_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_User_RunWorkerCompleted);
        }

        private void bwSynchOfficeMate_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_User.Enabled = true;
        }

        private void bwSynchOfficeMate_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SynchDataOfficeMate_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_User.SynchDataOfficeMate_User(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Users");
                        ObjGoalBase.WriteToSyncLogFile("User Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("User_Push");
                        GoalBase.WriteToSyncLogFile_Static("User Sync (Local Database To Adit Server) Successfully.");
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[User Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }
        #endregion

        #region Synch ApptStatus

        private void fncSynchDataOfficeMate_ApptStatus()
        {
            InitBgWorkerOfficeMate_ApptStatus();
            InitBgTimerOfficeMate_ApptStatus();
        }

        private void InitBgTimerOfficeMate_ApptStatus()
        {
            timerSynchOfficeMate_ApptStatus = new System.Timers.Timer();
            this.timerSynchOfficeMate_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchOfficeMate_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_ApptStatus_Tick);
            timerSynchOfficeMate_ApptStatus.Enabled = true;
            timerSynchOfficeMate_ApptStatus.Start();
        }

        private void InitBgWorkerOfficeMate_ApptStatus()
        {
            bwSynchOfficeMate_ApptStatus = new BackgroundWorker();
            bwSynchOfficeMate_ApptStatus.WorkerReportsProgress = true;
            bwSynchOfficeMate_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_ApptStatus_DoWork);
            bwSynchOfficeMate_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_ApptStatus.Enabled = false;
                MethodForCallSynchOrderOfficeMate_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_ApptStatus()
        {
            System.Threading.Thread procThreadmainOfficeMate_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_ApptStatus);
            procThreadmainOfficeMate_ApptStatus.Start();
        }

        public void CallSyncOrderTableOfficeMate_ApptStatus()
        {
            if (bwSynchOfficeMate_ApptStatus.IsBusy != true)
            {
                bwSynchOfficeMate_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_ApptStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_ApptStatus.Enabled = true;
        }

        public void SynchDataOfficeMate_ApptStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Appt_Status.SynchDataOfficeMate_Appt_Status(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());

                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                        ObjGoalBase.WriteToSyncLogFile("ApptStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus_Push");
                        GoalBase.WriteToSyncLogFile_Static("Appointment Status Sync (Local Database To Adit Server) Successfully.");

                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ApptStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch OrderStatus

        private void fncSynchDataOfficeMate_OrderStatus()
        {
            InitBgWorkerOfficeMate_OrderStatus();
            InitBgTimerOfficeMate_OrderStatus();
        }

        private void InitBgTimerOfficeMate_OrderStatus()
        {
            timerSynchOfficeMate_OrderStatus = new System.Timers.Timer();
            this.timerSynchOfficeMate_OrderStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchOfficeMate_OrderStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_OrderStatus_Tick);
            timerSynchOfficeMate_OrderStatus.Enabled = true;
            timerSynchOfficeMate_OrderStatus.Start();
        }

        private void InitBgWorkerOfficeMate_OrderStatus()
        {
            bwSynchOfficeMate_OrderStatus = new BackgroundWorker();
            bwSynchOfficeMate_OrderStatus.WorkerReportsProgress = true;
            bwSynchOfficeMate_OrderStatus.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_OrderStatus.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_OrderStatus_DoWork);
            bwSynchOfficeMate_OrderStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_OrderStatus_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_OrderStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_OrderStatus.Enabled = false;
                MethodForCallSynchOrderOfficeMate_OrderStatus();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_OrderStatus()
        {
            System.Threading.Thread procThreadmainOfficeMate_OrderStatus = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_OrderStatus);
            procThreadmainOfficeMate_OrderStatus.Start();
        }

        public void CallSyncOrderTableOfficeMate_OrderStatus()
        {
            if (bwSynchOfficeMate_OrderStatus.IsBusy != true)
            {
                bwSynchOfficeMate_OrderStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_OrderStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_OrderStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_OrderStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_OrderStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_OrderStatus.Enabled = true;
        }

        public void SynchDataOfficeMate_OrderStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Order_Status.SynchDataOfficeMate_Order_Status(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());

                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OrderStatus");
                        ObjGoalBase.WriteToSyncLogFile("OrderStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OrderStatus_Push");
                        GoalBase.WriteToSyncLogFile_Static("OrderStatus Sync (Local Database To Adit Server) Successfully.");

                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[OrderStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Order

        public void SynchDataOfficeMate_Order()
        {
            try
            {
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    Cls_Synch_Order.SynchDataOfficeMate_Order(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.LastSyncDateAditServer);
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Order");
                    ObjGoalBase.WriteToSyncLogFile("Order Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Order_Push");
                    GoalBase.WriteToSyncLogFile_Static("Order Sync (Local Database To Adit Server) Successfully.");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Order Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message.ToString() + ex.InnerException != null ? ex.InnerException.ToString() : " ");
            }
        }

        #endregion

        #region Synch ApptType

        private void fncSynchDataOfficeMate_ApptType()
        {
            InitBgWorkerOfficeMate_ApptType();
            InitBgTimerOfficeMate_ApptType();
        }

        private void InitBgTimerOfficeMate_ApptType()
        {
            timerSynchOfficeMate_ApptType = new System.Timers.Timer();
            this.timerSynchOfficeMate_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchOfficeMate_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_ApptType_Tick);
            timerSynchOfficeMate_ApptType.Enabled = true;
            timerSynchOfficeMate_ApptType.Start();
        }

        private void InitBgWorkerOfficeMate_ApptType()
        {
            bwSynchOfficeMate_ApptType = new BackgroundWorker();
            bwSynchOfficeMate_ApptType.WorkerReportsProgress = true;
            bwSynchOfficeMate_ApptType.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_ApptType.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_ApptType_DoWork);
            bwSynchOfficeMate_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_ApptType_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_ApptType.Enabled = false;
                MethodForCallSynchOrderOfficeMate_ApptType();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_ApptType()
        {
            System.Threading.Thread procThreadmainOfficeMate_ApptType = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_ApptType);
            procThreadmainOfficeMate_ApptType.Start();
        }

        public void CallSyncOrderTableOfficeMate_ApptType()
        {
            if (bwSynchOfficeMate_ApptType.IsBusy != true)
            {
                bwSynchOfficeMate_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_ApptType.Enabled = true;
        }

        public void SynchDataOfficeMate_ApptType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Appt_Type.SynchDataOfficeMate_Appt_Type(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                        Utility.WriteToSyncLogFile_All("Appointment Type Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[i]["Database"].ToString() + " to Local Database) Successfully.");
                        SynchLocalBAL.Update_Sync_Table_Datetime("ApptType_Push");
                        Utility.WriteToSyncLogFile_All("Appointment Type Sync (Local Database To Adit Server) Successfully.");
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Appointment_Type Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region synch Holidays

        private void fncSynchDataOfficeMate_Holiday()
        {
            InitBgWorkerOfficeMate_Holiday();
            InitBgTimerOfficeMate_Holiday();
        }

        private void InitBgTimerOfficeMate_Holiday()
        {
            timerSynchOfficeMate_Holiday = new System.Timers.Timer();
            this.timerSynchOfficeMate_Holiday.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchOfficeMate_Holiday.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_Holiday_Tick);
            timerSynchOfficeMate_Holiday.Enabled = true;
            timerSynchOfficeMate_Holiday.Start();
        }

        private void InitBgWorkerOfficeMate_Holiday()
        {
            bwSynchOfficeMate_Holiday = new BackgroundWorker();
            bwSynchOfficeMate_Holiday.WorkerReportsProgress = true;
            bwSynchOfficeMate_Holiday.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_Holiday.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_Holiday_DoWork);
            bwSynchOfficeMate_Holiday.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_Holiday_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_Holiday_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_Holiday.Enabled = false;
                MethodForCallSynchOrderOfficeMate_Holiday();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_Holiday()
        {
            System.Threading.Thread procThreadmainOfficeMate_Holiday = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_Holiday);
            procThreadmainOfficeMate_Holiday.Start();
        }

        public void CallSyncOrderTableOfficeMate_Holiday()
        {
            if (bwSynchOfficeMate_Holiday.IsBusy != true)
            {
                bwSynchOfficeMate_Holiday.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_Holiday_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_Holiday.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_Holiday();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_Holiday_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_Holiday.Enabled = true;
        }

        public void SynchDataOfficeMate_Holiday()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Holiday.SynchDataOfficeMate_Holiday(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");
                            ObjGoalBase.WriteToSyncLogFile("Holiday Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday_Push");
                            GoalBase.WriteToSyncLogFile_Static("Holiday Sync (Local Database To Adit Server) Successfully.");
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

        private void fncSynchDataLocalToOfficeMate_Appointment()
        {
            InitBgWorkerLocalToOfficeMate_Appointment();
            InitBgTimerLocalToOfficeMate_Appointment();
        }

        private void InitBgTimerLocalToOfficeMate_Appointment()
        {
            timerSynchLocalToOfficeMate_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToOfficeMate_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToOfficeMate_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToOfficeMate_Appointment_Tick);
            timerSynchLocalToOfficeMate_Appointment.Enabled = true;
            timerSynchLocalToOfficeMate_Appointment.Start();
            timerSynchLocalToOfficeMate_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToOfficeMate_Appointment()
        {
            bwSynchLocalToOfficeMate_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToOfficeMate_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToOfficeMate_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToOfficeMate_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToOfficeMate_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToOfficeMate_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToOfficeMate_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToOfficeMate_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToOfficeMate_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToOfficeMate_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToOfficeMate_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToOfficeMate_Appointment);
            procThreadmainLocalToOfficeMate_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToOfficeMate_Appointment()
        {
            if (bwSynchLocalToOfficeMate_Appointment.IsBusy != true)
            {
                bwSynchLocalToOfficeMate_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToOfficeMate_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToOfficeMate_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToOfficeMate_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToOfficeMate_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToOfficeMate_Appointment.Enabled = true;
        }

        public void SynchDataLocalToOfficeMate_Appointment(string _filename_Appointment = "", string _EHRLogdirectory_Appointment = "")
        {
            try
            {
                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                {
                    AditOfficeMate.BAL.Cls_Synch_CreatAppt.SynchDataLocalToEHR_Appointment(Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[i]["Location_ID"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[i]["Organization_ID"].ToString(), Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment);
                }
                ObjGoalBase.WriteToSyncLogFile("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #endregion

        #region Patient Form

        private void fncSynchDataLocalToOfficeMate_Patient_Form()
        {
            InitBgWorkerLocalToOfficeMate_Patient_Form();
            InitBgTimerLocalToOfficeMate_Patient_Form();
        }

        private void InitBgTimerLocalToOfficeMate_Patient_Form()
        {
            timerSynchLocalToOfficeMate_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToOfficeMate_Patient_Form.Interval = 1000 * GoalBase.intervalEHRSynch_PatientForm;
            this.timerSynchLocalToOfficeMate_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToOfficeMate_Patient_Form_Tick);
            timerSynchLocalToOfficeMate_Patient_Form.Enabled = true;
            timerSynchLocalToOfficeMate_Patient_Form.Start();
            timerSynchLocalToOfficeMate_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToOfficeMate_Patient_Form()
        {
            bwSynchLocalToOfficeMate_Patient_Form = new BackgroundWorker();
            bwSynchLocalToOfficeMate_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToOfficeMate_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToOfficeMate_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToOfficeMate_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToOfficeMate_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToOfficeMate_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToOfficeMate_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToOfficeMate_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToOfficeMate_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToOfficeMate_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToOfficeMate_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToOfficeMate_Patient_Form);
            procThreadmainLocalToOfficeMate_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToOfficeMate_Patient_Form()
        {
            if (bwSynchLocalToOfficeMate_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToOfficeMate_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToOfficeMate_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToOfficeMate_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToOfficeMate_Patient_Form();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToOfficeMate_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToOfficeMate_Patient_Form.Enabled = true;
        }

        public void SynchDataLocalToOfficeMate_Patient_Form()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_PatientForm.SynchDataOfficeMate_PatientFormToEHR(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.EHRDocPath, CommonUtility.GetAditDocTempPath());
                        SynchDataLocalToOfficeMate_Patient_Document();
                        SynchDataOfficeMate_InsuranceCarrier_Document();
                    }
                }

                try
                {
                    SynchDataOfficeMate_PatientDisease();
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientDisease Sync from Patient(" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }

                try
                {
                    SynchDataOfficeMate_PatientMedication();
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[PatientMedication Sync from Patient (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }


        #endregion

        #region Patient Document

        public void SynchDataLocalToOfficeMate_Patient_Document()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #region PatientPayment
        private void fncSynchDataOfficeMate_PatientPayment()
        {
            InitBgWorkerOfficeMate_PatientPayment();
            InitBgTimerOfficeMate_PatientPayment();
        }

        private void InitBgTimerOfficeMate_PatientPayment()
        {
            timerSynchOfficeMate_PatientPayment = new System.Timers.Timer();
            this.timerSynchOfficeMate_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
            this.timerSynchOfficeMate_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_PatientPayment_Tick);
            timerSynchOfficeMate_PatientPayment.Enabled = true;
            timerSynchOfficeMate_PatientPayment.Start();
        }

        private void InitBgWorkerOfficeMate_PatientPayment()
        {
            bwSynchOfficeMate_PatientPayment = new BackgroundWorker();
            bwSynchOfficeMate_PatientPayment.WorkerReportsProgress = true;
            bwSynchOfficeMate_PatientPayment.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_PatientPayment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOfficeMate_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_PatientPayment_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_PatientPayment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_PatientPayment.Enabled = false;
                MethodForCallSynchOrderOfficeMate_PatientPayment();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_PatientPayment()
        {
            System.Threading.Thread procThreadmainOfficeMate_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_PatientPayment);
            procThreadmainOfficeMate_PatientPayment.Start();
        }

        public void CallSyncOrderTableOfficeMate_PatientPayment()
        {
            if (bwSynchOfficeMate_PatientPayment.IsBusy != true)
            {
                bwSynchOfficeMate_PatientPayment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_PatientPayment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_PatientPaymentLog_Local_To_OfficeMate();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }
        private void bwSynchOfficeMate_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_PatientPayment.Enabled = true;
        }

        public void SynchDataLiveDB_PatientPaymentLog_Local_To_OfficeMate()
        {
            try
            {
                if (!IsPaymentSyncing)
                {
                    IsPaymentSyncing = true;
                    if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                    {
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            Cls_Synch_CreatePayment.SynchDataLocalToEHR_Payment(Utility.DBConnString, "", "", Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        }
                    }
                    IsPaymentSyncing = false;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Patient Payment Sync_Local_To_OfficeMate Error : " + ex.Message);
            }
            finally { IsPaymentSyncing = false; }

        }

        #endregion

        #region PatientPaymentSMSCllLogs
        public void SynchDataLiveDB_PatientSMSCallLog_LocalTOOfficeMate()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    Int64 TransactionHeaderId = 0;
                    string noteId = "";
                    DataTable dtPatientPayment = new DataTable();
                    //if (Cls_Synch_PatientSMSCall_Logs.SynchDataLiveDB_PatientSMSCallLog_LocalTOClearDent(Utility.DBConnString, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()))
                    //{
                    //    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientSMSCalllog_LocalTOOfficeMate");
                    //    ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    //}
                    //else
                    //{
                    //    ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                    //}
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_SMSCallLog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }
        #endregion

        #endregion

        #region Synch Disease
        private void fncSynchDataOfficeMate_Disease()
        {
            InitBgWorkerOfficeMate_Disease();
            InitBgTimerOfficeMate_Disease();
        }

        private void InitBgTimerOfficeMate_Disease()
        {
            timerSynchOfficeMate_Disease = new System.Timers.Timer();
            this.timerSynchOfficeMate_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchOfficeMate_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOfficeMate_Disease_Tick);
            timerSynchOfficeMate_Disease.Enabled = true;
            timerSynchOfficeMate_Disease.Start();
        }

        private void InitBgWorkerOfficeMate_Disease()
        {
            bwSynchOfficeMate_Disease = new BackgroundWorker();
            bwSynchOfficeMate_Disease.WorkerReportsProgress = true;
            bwSynchOfficeMate_Disease.WorkerSupportsCancellation = true;
            bwSynchOfficeMate_Disease.DoWork += new DoWorkEventHandler(bwSynchOfficeMate_Disease_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOfficeMate_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOfficeMate_Disease_RunWorkerCompleted);
        }

        private void timerSynchOfficeMate_Disease_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOfficeMate_Disease.Enabled = false;
                MethodForCallSynchOrderOfficeMate_Disease();
            }
        }

        public void MethodForCallSynchOrderOfficeMate_Disease()
        {
            System.Threading.Thread procThreadmainOfficeMate_Disease = new System.Threading.Thread(this.CallSyncOrderTableOfficeMate_Disease);
            procThreadmainOfficeMate_Disease.Start();
        }

        public void CallSyncOrderTableOfficeMate_Disease()
        {
            if (bwSynchOfficeMate_Disease.IsBusy != true)
            {
                bwSynchOfficeMate_Disease.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOfficeMate_Disease_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOfficeMate_Disease.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOfficeMate_Disease();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOfficeMate_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOfficeMate_Disease.Enabled = true;
        }

        public void SynchDataOfficeMate_Disease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Disease.SynchDataOfficeMate_Disease(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                        ObjGoalBase.WriteToSyncLogFile("Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    }
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataOfficeMate_PatientDisease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_PatientDisease.SynchDataOfficemate_PatientDisease(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientDisease");
                        ObjGoalBase.WriteToSyncLogFile("PatientDisease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
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

        public void SynchDataOfficeMate_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Medication.SynchDataOfficeMate_Medication(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Medication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataOfficeMate_Insurance()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_Insurance.SynchDataOfficeMate_Insurance(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Medication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataOfficeMate_InsuranceCarrier_Document()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_PatientDocument.Save_InsuranceCarrier_Document_in_officeMate(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), "", Utility.EHRDocPath, CommonUtility.GetAditDocTempPath());
                    }
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Medication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataOfficeMate_PatientMedication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;

                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        Cls_Synch_PatientMedication.SynchDataOfficeMate_PatientMedication(Utility.DBConnString, Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());

                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Medication");
                        ObjGoalBase.WriteToSyncLogFile("PatientMedication Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[i]["Database"].ToString() + " to Local Database) Successfully.");
                    }
                    Is_synched_PatientMedication = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatientMedication = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientMedication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message.ToString() + ex.InnerException != null ? ex.InnerException.ToString() : " ");
            }
        }

    }
}
