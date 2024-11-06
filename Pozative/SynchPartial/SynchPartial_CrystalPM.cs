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
using System.Threading;
using Microsoft.Win32;
using System.Diagnostics;
using System.Timers;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable

        public bool IsCrystalPMProviderSync = false;
        public bool IsCrystalPMOperatorySync = false;
        public bool IsCrystalPMApptTypeSync = false;
        
        #endregion
        #region commentcode

        //private BackgroundWorker bwSynchCrystalPM_Appointment = null;
        //private System.Timers.Timer timerSynchCrystalPM_Appointment = null;

        //private BackgroundWorker bwSynchCrystalPM_OperatoryEvent = null;
        //private System.Timers.Timer timerSynchCrystalPM_OperatoryEvent = null;

        //private BackgroundWorker bwSynchCrystalPM_Provider = null;
        //private System.Timers.Timer timerSynchCrystalPM_Provider = null;

        //private BackgroundWorker bwSynchCrystalPM_Disease = null;
        //private System.Timers.Timer timerSynchCrystalPM_Disease = null;

        //private BackgroundWorker bwSynchCrystalPM_ProviderHours = null;
        //private System.Timers.Timer timerSynchCrystalPM_ProviderHours = null;

        //private BackgroundWorker bwSynchCrystalPM_PatientPayment = null;
        //private System.Timers.Timer timerSynchCrystalPM_PatientPayment = null;

        //private BackgroundWorker bwSynchCrystalPM_Speciality = null;
        //private System.Timers.Timer timerSynchCrystalPM_Speciality = null;

        //private BackgroundWorker bwSynchCrystalPM_Operatory = null;
        //private System.Timers.Timer timerSynchCrystalPM_Operatory = null;

        //private BackgroundWorker bwSynchCrystalPM_ApptType = null;
        //private System.Timers.Timer timerSynchCrystalPM_ApptType = null;

        //private BackgroundWorker bwSynchCrystalPM_Patient = null;
        //private System.Timers.Timer timerSynchCrystalPM_Patient = null;

        //private BackgroundWorker bwSynchCrystalPM_RecallType = null;
        //private System.Timers.Timer timerSynchCrystalPM_RecallType = null;

        //private BackgroundWorker bwSynchCrystalPM_User = null;
        //private System.Timers.Timer timerSynchCrystalPM_User = null;

        //private BackgroundWorker bwSynchCrystalPM_ApptStatus = null;
        //private System.Timers.Timer timerSynchCrystalPM_ApptStatus = null;

        //private BackgroundWorker bwSynchCrystalPM_Holiday = null;
        //private System.Timers.Timer timerSynchCrystalPM_Holiday = null;

        //  private BackgroundWorker bwSynchCrystalPM_PatientWiseRecallDate = null;
        // private System.Timers.Timer timerSynchCrystalPM_PatientWiseRecallDate = null;

        //private BackgroundWorker bwSynchLocalToCrystalPM_Appointment = null;
        //private System.Timers.Timer timerSynchLocalToCrystalPM_Appointment = null;

        //private BackgroundWorker bwSynchLocalToCrystalPM_Patient_Form = null;
        //private System.Timers.Timer timerSynchLocalToCrystalPM_Patient_Form = null;

        //private BackgroundWorker bwSynchCrystalPM_MedicalHistory = null;
        //private System.Timers.Timer timerSynchCrystalPM_MedicalHistory = null;
        //private void CallSynchCrystalPMToLocal()
        //{
        //    if (Utility.AditSync)
        //    {
        //        Utility.WriteToSyncLogFile_All("Start Application Sync");

        //        //fncSynchDataCrystalPM_PatientPayment();

        //        SynchDataCrystalPM_Speciality();
        //        fncSynchDataCrystalPM_Speciality();

        //        SynchDataCrystalPM_Operatory();
        //        fncSynchDataCrystalPM_Operatory();

        //        SynchDataCrystalPM_Provider();
        //        fncSynchDataCrystalPM_Provider();

        //        SynchDataCrystalPM_ApptType();
        //        fncSynchDataCrystalPM_ApptType();

        //        SynchDataCrystalPM_ApptStatus();
        //        fncSynchDataCrystalPM_ApptStatus();

        //        SynchDataCrystalPM_RecallType();
        //        fncSynchDataCrystalPM_RecallType();

        //        SynchDataCrystalPM_User();
        //        fncSynchDataCrystalPM_User();

        //        SynchDataCrystalPM_OperatoryEvent();
        //        fncSynchDataCrystalPM_OperatoryEvent();

        //       // SynchDataCrystalPM_Patient_New();

        //        // SynchDataCrystalPM_Appointment();
        //        //SynchDataCrystalPM_Disease();
        //        //fncSynchDataCrystalPM_Disease();

        //        SynchDataCrystalPM_ProviderHours();
        //        fncSynchDataCrystalPM_ProviderHours();
        //        ////fncSynchDataCrystalPM_Appointment();

        //        ////SynchDataCrystalPM_Holiday();
        //        //fncSynchDataCrystalPM_Holiday();

        //        SynchDataLocalToCrystalPM_Patient_Form();
        //        fncSynchDataLocalToCrystalPM_Patient_Form();

        //        ////SynchDataCrystalPM_MedicalHistory();
        //        //fncSynchDataCrystalPM_MedicalHistory();

        //        ////SynchDataCrystalPM_PatientImages();

        //        //// SynchDataCrystalPM_PatientWiseRecallDate();
        //        ////fncSynchDataCrystalPM_PatientWiseRecallDate();
        //    }
        //}   

        //#region Synch Appointment

        //private void fncSynchDataCrystalPM_Appointment()
        //{
        //    InitBgWorkerCrystalPM_Appointment();
        //    InitBgTimerCrystalPM_Appointment();
        //}

        //private void InitBgTimerCrystalPM_Appointment()
        //{
        //    timerSynchCrystalPM_Appointment = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
        //    this.timerSynchCrystalPM_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_Appointment_Tick);
        //    timerSynchCrystalPM_Appointment.Enabled = true;
        //    timerSynchCrystalPM_Appointment.Start();
        //    timerSynchCrystalPM_Appointment_Tick(null, null);
        //}

        //private void InitBgWorkerCrystalPM_Appointment()
        //{
        //    bwSynchCrystalPM_Appointment = new BackgroundWorker();
        //    bwSynchCrystalPM_Appointment.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_Appointment.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_Appointment.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_Appointment_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchCrystalPM_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_Appointment_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_Appointment_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {

        //        timerSynchCrystalPM_Appointment.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_Appointment();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_Appointment()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_Appointment = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_Appointment);
        //    procThreadmainCrystalPM_Appointment.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_Appointment()
        //{
        //    if (bwSynchCrystalPM_Appointment.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_Appointment.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_Appointment_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_Appointment.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        //SynchDataLocalToCrystalPM_PatientPayment();
        //        SynchDataCrystalPM_Appointment();
        //        //SynchDataCrystalPM_PatientStatus();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_Appointment.Enabled = true;
        //}

        //public void SynchDataCrystalPM_Appointment()
        //{
        //    ObjGoalBase.WriteToSyncLogFile(" Appointment Sync start at : " + Utility.LastSyncDateAditServer.ToString());

        //    //if (IsCrystalPMProviderSync && IsCrystalPMOperatorySync && IsCrystalPMApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff) // && Utility.AditLocationSyncEnable 
        //    //{
        //    try
        //    {
        //        Is_synched_Appointment = false;

        //        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //        {
        //            AditCrystalPM.BAL.Cls_Synch_Appt.SynchDataCrystalPM_Appt(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), DateTime.Now);
        //            SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
        //            ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + "  to Local Database) Successfully.");
        //            SynchLocalBAL.Update_Sync_Table_Datetime("Appointment_Push");
        //            ObjGoalBase.WriteToSyncLogFile("Appointment Sync (Local Database To Adit Server) Successfully.");
        //        }
        //        Is_synched_Appointment = true;
        //        //if (Utility.DtInstallServiceList.Rows.Count > 1)
        //        //{
        //        //    SynchDataLiveDB_Push_AppointmentMultiLocation();
        //        //}
        //        //else
        //        //{
        //        //    SynchDataLiveDB_Push_Appointment();
        //        //}
        //        IsEHRAllSync = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_Appointment = true;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //    finally
        //    {
        //        Is_synched_Appointment = true;
        //        // this.Dispose();
        //    }
        //    //}
        //    //else
        //    //{
        //    //    ObjGoalBase.WriteToSyncLogFile("Appointment Sync All flag is not true CrystalPMProviderSync = " + IsCrystalPMProviderSync.ToString() + ", CrystalPMOperatorySync = " + IsCrystalPMOperatorySync.ToString() + ", CrystalPMApptTypeSync = " + IsCrystalPMApptTypeSync.ToString() + ", Synched_Appointment = " + Is_synched_Appointment.ToString() + ", ApplicationIdleTimeOff = " + Utility.IsApplicationIdleTimeOff.ToString());
        //    //}
        //}

        //#endregion

        //#region Synch OperatoryEvent

        //private void fncSynchDataCrystalPM_OperatoryEvent()
        //{
        //    InitBgWorkerCrystalPM_OperatoryEvent();
        //    InitBgTimerCrystalPM_OperatoryEvent();
        //}

        //private void InitBgTimerCrystalPM_OperatoryEvent()
        //{
        //    timerSynchCrystalPM_OperatoryEvent = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
        //    this.timerSynchCrystalPM_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_OperatoryEvent_Tick);
        //    timerSynchCrystalPM_OperatoryEvent.Enabled = true;
        //    timerSynchCrystalPM_OperatoryEvent.Start();
        //    timerSynchCrystalPM_OperatoryEvent_Tick(null, null);
        //}

        //private void InitBgWorkerCrystalPM_OperatoryEvent()
        //{
        //    bwSynchCrystalPM_OperatoryEvent = new BackgroundWorker();
        //    bwSynchCrystalPM_OperatoryEvent.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_OperatoryEvent.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_OperatoryEvent_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchCrystalPM_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_OperatoryEvent_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_OperatoryEvent_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_OperatoryEvent.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_OperatoryEvent();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_OperatoryEvent()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_OperatoryEvent);
        //    procThreadmainCrystalPM_OperatoryEvent.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_OperatoryEvent()
        //{
        //    if (bwSynchCrystalPM_OperatoryEvent.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_OperatoryEvent.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_OperatoryEvent.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_OperatoryEvent();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_OperatoryEvent.Enabled = true;
        //}

        //public void SynchDataCrystalPM_OperatoryEvent()
        //{
        //    if (Utility.IsExternalAppointmentSync)
        //    {
        //        IsCrystalPMOperatorySync = true;
        //        Is_synched_OperatoryEvent = false;
        //    }
        //    if (IsCrystalPMOperatorySync && !Is_synched_OperatoryEvent && Utility.IsApplicationIdleTimeOff) // && Utility.AditLocationSyncEnable
        //    {
        //        try
        //        {
        //            Is_synched_OperatoryEvent = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                AditCrystalPM.BAL.Cls_Synch_OperatoryEvent.SynchDataCrystalPM_OperatoryEvent(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.LastSyncDateAditServer);
        //                SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
        //                ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent_Push");
        //                ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (Local Database To Adit Server) Successfully.");
        //                IsEHRAllSync = true;
        //            }
        //            Is_synched_OperatoryEvent = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            Is_synched_OperatoryEvent = false;
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //        }
        //    }
        //}

        //#endregion

        //#region Synch Operatory

        //private void fncSynchDataCrystalPM_Operatory()
        //{
        //    InitBgWorkerCrystalPM_Operatory();
        //    InitBgTimerCrystalPM_Operatory();
        //}

        //private void InitBgTimerCrystalPM_Operatory()
        //{
        //    timerSynchCrystalPM_Operatory = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
        //    this.timerSynchCrystalPM_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_Operatory_Tick);
        //    timerSynchCrystalPM_Operatory.Enabled = true;
        //    timerSynchCrystalPM_Operatory.Start();
        //}

        //private void InitBgWorkerCrystalPM_Operatory()
        //{
        //    bwSynchCrystalPM_Operatory = new BackgroundWorker();
        //    bwSynchCrystalPM_Operatory.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_Operatory.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_Operatory.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_Operatory_DoWork);
        //    bwSynchCrystalPM_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_Operatory_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_Operatory_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_Operatory.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_Operatory();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_Operatory()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_Operatory = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_Operatory);
        //    procThreadmainCrystalPM_Operatory.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_Operatory()
        //{
        //    if (bwSynchCrystalPM_Operatory.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_Operatory.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_Operatory_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_Operatory.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_Operatory();
        //    }
        //    catch (Exception ex)
        //    {
        //        //ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_Operatory.Enabled = true;
        //}

        //public void SynchDataCrystalPM_Operatory()
        //{
        //    try
        //    {
        //        if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
        //        {
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                AditCrystalPM.BAL.Cls_Synch_Operatory.SynchDataCrystalPM_Operatory(Utility.DBConnString, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
        //                ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                IsCrystalPMOperatorySync = true;
        //                SynchLocalBAL.Update_Sync_Table_Datetime("Operatory_Push");
        //                ObjGoalBase.WriteToSyncLogFile("Operatory Sync (Local Database To Adit Server) Successfully.");
        //                IsOperatorySyncPush = true;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Synch Provider

        //private void fncSynchDataCrystalPM_Provider()
        //{
        //    InitBgWorkerCrystalPM_Provider();
        //    InitBgTimerCrystalPM_Provider();
        //}

        //private void InitBgTimerCrystalPM_Provider()
        //{
        //    timerSynchCrystalPM_Provider = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
        //    this.timerSynchCrystalPM_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_Provider_Tick);
        //    timerSynchCrystalPM_Provider.Enabled = true;
        //    timerSynchCrystalPM_Provider.Start();
        //}

        //private void InitBgWorkerCrystalPM_Provider()
        //{
        //    bwSynchCrystalPM_Provider = new BackgroundWorker();
        //    bwSynchCrystalPM_Provider.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_Provider.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_Provider.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_Provider_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchCrystalPM_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_Provider_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_Provider_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_Provider.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_Provider();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_Provider()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_Provider = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_Provider);
        //    procThreadmainCrystalPM_Provider.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_Provider()
        //{
        //    if (bwSynchCrystalPM_Provider.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_Provider.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_Provider_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_Provider.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_Provider();
        //        //  SynchDataCrystalPM_ProviderHours();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_Provider.Enabled = true;
        //}

        //public void SynchDataCrystalPM_Provider()
        //{
        //    try
        //    {
        //        if (!Is_synched_Provider && Utility.IsApplicationIdleTimeOff) //&& Utility.AditLocationSyncEnable
        //        {
        //            Is_synched_Provider = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                AditCrystalPM.BAL.Cls_Synch_Provider.SynchDataCrystalPM_Provider(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
        //                ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                IsCrystalPMProviderSync = true;
        //                SynchLocalBAL.Update_Sync_Table_Datetime("Provider_Push");
        //                ObjGoalBase.WriteToSyncLogFile("Providers Sync (Local Database To Adit Server) Successfully.");
        //                IsProviderSyncPush = true;
        //            }
        //        }
        //        Is_synched_Provider = false;
        //        IsProviderSyncedFirstTime = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_Provider = false;
        //        IsProviderSyncedFirstTime = true;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        ////public void SynchDataCrystalPM_ProviderHours()
        ////{
        ////    try
        ////    {
        ////        if (Utility.IsApplicationIdleTimeOff && IsCrystalPMProviderSync && IsCrystalPMOperatorySync)
        ////        {
        ////            DataTable dtCrystalPMProviderHours = SynchCrystalPMBAL.GetCrystalPMProviderHoursData();
        ////            dtCrystalPMProviderHours.Columns.Add("InsUptDlt", typeof(int));
        ////            DataTable dtLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData();

        ////            foreach (DataRow dtDtxRow in dtCrystalPMProviderHours.Rows)
        ////            {
        ////                DataRow[] row = dtLocalProviderHours.Copy().Select("PH_EHR_ID = '" + dtDtxRow["PH_EHR_ID"] + "'");
        ////                if (row.Length > 0)
        ////                {
        ////                    if (Convert.ToDateTime(dtDtxRow["Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Entry_DateTime"].ToString().Trim()))
        ////                    {
        ////                        dtDtxRow["InsUptDlt"] = 2;
        ////                    }
        ////                }
        ////                else
        ////                {
        ////                    dtDtxRow["InsUptDlt"] = 1;
        ////                }
        ////            }
        ////            dtCrystalPMProviderHours.AcceptChanges();

        ////            foreach (DataRow dtLPHRow in dtLocalProviderHours.Rows)
        ////            {
        ////                DataRow[] rowBlcOpt = dtCrystalPMProviderHours.Copy().Select("PH_EHR_ID = '" + dtLPHRow["PH_EHR_ID"].ToString().Trim() + "' ");
        ////                if (rowBlcOpt.Length > 0)
        ////                { }
        ////                else
        ////                {
        ////                    DataRow BlcOptDtldr = dtCrystalPMProviderHours.NewRow();
        ////                    BlcOptDtldr["PH_EHR_ID"] = dtLPHRow["PH_EHR_ID"].ToString().Trim();
        ////                    BlcOptDtldr["InsUptDlt"] = 3;
        ////                    dtCrystalPMProviderHours.Rows.Add(BlcOptDtldr);
        ////                }
        ////            }

        ////            dtCrystalPMProviderHours.AcceptChanges();

        ////            if (dtCrystalPMProviderHours != null && dtCrystalPMProviderHours.Rows.Count > 0)
        ////            {
        ////                bool status = SynchCrystalPMBAL.Save_ProviderHours_CrystalPM_To_Local(dtCrystalPMProviderHours);

        ////                if (status)
        ////                {
        ////                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
        ////                    ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

        ////                    SynchDataLiveDB_Push_ProviderHours();
        ////                }

        ////            }

        ////        }

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Is_synched_Provider = false;
        ////        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        ////    }
        ////}


        //#endregion

        //#region Synch ProviderHours

        //private void fncSynchDataCrystalPM_ProviderHours()
        //{
        //    InitBgWorkerCrystalPM_ProviderHours();
        //    InitBgTimerCrystalPM_ProviderHours();
        //}

        //private void InitBgTimerCrystalPM_ProviderHours()
        //{
        //    timerSynchCrystalPM_ProviderHours = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_ProviderHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
        //    this.timerSynchCrystalPM_ProviderHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_ProviderHours_Tick);
        //    timerSynchCrystalPM_ProviderHours.Enabled = true;
        //    timerSynchCrystalPM_ProviderHours.Start();
        //}

        //private void InitBgWorkerCrystalPM_ProviderHours()
        //{
        //    bwSynchCrystalPM_ProviderHours = new BackgroundWorker();
        //    bwSynchCrystalPM_ProviderHours.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_ProviderHours.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_ProviderHours.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_ProviderHours_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchCrystalPM_ProviderHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_ProviderHours_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_ProviderHours_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_ProviderHours.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_ProviderHours();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_ProviderHours()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_ProviderHours = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_ProviderHours);
        //    procThreadmainCrystalPM_ProviderHours.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_ProviderHours()
        //{
        //    if (bwSynchCrystalPM_ProviderHours.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_ProviderHours.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_ProviderHours_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_ProviderHours.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_ProviderHours();
        //        // SyncPullLogsAndSaveinCrystalPM();

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void SyncPullLogsAndSaveinCrystalPM()
        //{
        //    try
        //    {
        //        CheckCustomhoursForProviderOperatory();
        //        SynchDataLiveDB_Pull_PatientPaymentSMSCall();
        //        SynchDataLiveDB_Pull_PatientFollowUp();
        //        SynchDataPatientSMSCall_LocalTOCrystalPM();
        //        fncPaymentSMSCallStatusUpdate();
        //        SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
        //        SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        //private void bwSynchCrystalPM_ProviderHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_ProviderHours.Enabled = true;
        //}

        //public void SynchDataCrystalPM_ProviderHours()
        //{
        //    try
        //    {
        //        //CheckCustomhoursForProviderOperatory();
        //        //if (Utility.IsApplicationIdleTimeOff && IsCrystalPMProviderSync && IsCrystalPMOperatorySync && Utility.is_scheduledCustomhour)// && Utility.AditLocationSyncEnable
        //        //{
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                //AditCrystalPM.BAL.Cls_Synch_Providerhous.SynchDataCrystalPM_ProviderHours(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.LastSyncDateAditServer);
        //                SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
        //                ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours_Push");
        //                ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (Local Database To Adit Server) Successfully.");
        //                IsProviderSyncPush = true;
        //            }                   
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_Provider = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Synch Speciality

        //private void fncSynchDataCrystalPM_Speciality()
        //{
        //    InitBgWorkerCrystalPM_Speciality();
        //    InitBgTimerCrystalPM_Speciality();
        //}

        //private void InitBgTimerCrystalPM_Speciality()
        //{
        //    timerSynchCrystalPM_Speciality = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
        //    this.timerSynchCrystalPM_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_Speciality_Tick);
        //    timerSynchCrystalPM_Speciality.Enabled = true;
        //    timerSynchCrystalPM_Speciality.Start();
        //}

        //private void InitBgWorkerCrystalPM_Speciality()
        //{
        //    bwSynchCrystalPM_Speciality = new BackgroundWorker();
        //    bwSynchCrystalPM_Speciality.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_Speciality.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_Speciality.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_Speciality_DoWork);
        //    bwSynchCrystalPM_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_Speciality_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_Speciality_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_Speciality.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_Speciality();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_Speciality()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_Speciality = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_Speciality);
        //    procThreadmainCrystalPM_Speciality.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_Speciality()
        //{
        //    if (bwSynchCrystalPM_Speciality.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_Speciality.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_Speciality_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_Speciality.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_Speciality();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_Speciality.Enabled = true;
        //}

        //public void SynchDataCrystalPM_Speciality()
        //{
        //    try
        //    {

        //        if (!Is_synched_Speciality && Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
        //        {
        //            Is_synched_Speciality = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                AditCrystalPM.BAL.Cls_Synch_Speciality.SynchDataCrystalPM_Speciality(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality");
        //                ObjGoalBase.WriteToSyncLogFile("Speciality Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + "  to Local Database) Successfully.");
        //                UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality_Push");
        //                ObjGoalBase.WriteToSyncLogFile("Speciality Sync (Local Database To Adit Server) Successfully.");

        //            }
        //            Is_synched_Speciality = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_Speciality = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Speciality Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Sycn FolderList
        //public void SynchDataCrystalPM_FolderList()
        //{
        //    try
        //    {
        //        if (!Is_synched_Operatory && Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
        //        {
        //            Is_synched_Operatory = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtCrystalPMFolderList = null;// SynchCrystalPMBAL.GetCrystalPMFolderListData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
        //                dtCrystalPMFolderList.Columns.Add("InsUptDlt", typeof(int));
        //                DataTable dtLocalFolderList = SynchLocalBAL.GetLocalFolderListData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                foreach (DataRow dtDtxRow in dtCrystalPMFolderList.Rows)
        //                {
        //                    DataRow[] row = dtLocalFolderList.Copy().Select("FolderList_EHR_ID = '" + dtDtxRow["FolderList_EHR_ID"] + "'");
        //                    if (row.Length > 0)
        //                    {
        //                        if (dtDtxRow["Folder_Name"].ToString().Trim() != row[0]["Folder_Name"].ToString().Trim())
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 2;
        //                        }
        //                        else if (Convert.ToInt32(dtDtxRow["FolderOrder"]) != Convert.ToInt32(row[0]["FolderOrder"]))
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 2;
        //                        }
        //                        else if (Convert.ToBoolean(dtDtxRow["Is_Deleted"]) != Convert.ToBoolean(row[0]["Is_Deleted"]))
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 4;
        //                        }
        //                        else
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 0;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 1;
        //                    }
        //                }

        //                dtCrystalPMFolderList.AcceptChanges();
        //                if (dtCrystalPMFolderList != null && dtCrystalPMFolderList.Rows.Count > 0)
        //                {
        //                    bool status = false;// SynchCrystalPMBAL.Save_FolderList_CrystalPM_To_Local(dtCrystalPMFolderList, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
        //                    if (status)
        //                    {
        //                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FolderList");
        //                        ObjGoalBase.WriteToSyncLogFile("FolderList Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                        IsCrystalPMOperatorySync = true;
        //                    }
        //                    else
        //                    {
        //                        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Speciality Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
        //                        IsCrystalPMOperatorySync = false;
        //                    }

        //                    #region Deleted FolderList
        //                    dtCrystalPMFolderList = dtCrystalPMFolderList.Clone();
        //                    DataTable dtCrystalPMDeletedFolderList = null;// SynchCrystalPMBAL.GetCrystalPMDeletedFolderListData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
        //                    DataTable dtLocalFolderListAfterInsert = SynchLocalBAL.GetLocalFolderListData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                    foreach (DataRow dtDtlRow in dtCrystalPMDeletedFolderList.Rows)
        //                    {
        //                        DataRow[] row = dtLocalFolderListAfterInsert.Copy().Select("FolderList_EHR_ID = '" + dtDtlRow["FolderList_EHR_ID"].ToString().Trim() + "'");
        //                        if (row.Length > 0)
        //                        {
        //                            if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
        //                            {
        //                                DataRow ApptDtldr = dtCrystalPMFolderList.NewRow();
        //                                ApptDtldr["FolderList_EHR_ID"] = dtDtlRow["FolderList_EHR_ID"].ToString().Trim();
        //                                ApptDtldr["InsUptDlt"] = 3;
        //                                dtCrystalPMFolderList.Rows.Add(ApptDtldr);
        //                            }
        //                        }
        //                    }
        //                    if (dtCrystalPMFolderList != null && dtCrystalPMFolderList.Rows.Count > 0)
        //                    {
        //                        status = false;// SynchCrystalPMBAL.Save_FolderList_CrystalPM_To_Local(dtCrystalPMFolderList, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
        //                    }
        //                    #endregion
        //                    SynchDataLiveDB_Push_FolderList();
        //                }
        //            }
        //            Is_synched_Operatory = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_Operatory = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[FolderList Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}


        //#endregion

        //#region Synch Appointment Type

        //private void fncSynchDataCrystalPM_ApptType()
        //{
        //    InitBgWorkerCrystalPM_ApptType();
        //    InitBgTimerCrystalPM_ApptType();
        //}

        //private void InitBgTimerCrystalPM_ApptType()
        //{
        //    timerSynchCrystalPM_ApptType = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
        //    this.timerSynchCrystalPM_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_ApptType_Tick);
        //    timerSynchCrystalPM_ApptType.Enabled = true;
        //    timerSynchCrystalPM_ApptType.Start();
        //}

        //private void InitBgWorkerCrystalPM_ApptType()
        //{
        //    bwSynchCrystalPM_ApptType = new BackgroundWorker();
        //    bwSynchCrystalPM_ApptType.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_ApptType.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_ApptType.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_ApptType_DoWork);
        //    bwSynchCrystalPM_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_ApptType_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_ApptType_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_ApptType.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_ApptType();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_ApptType()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_ApptType = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_ApptType);
        //    procThreadmainCrystalPM_ApptType.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_ApptType()
        //{
        //    if (bwSynchCrystalPM_ApptType.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_ApptType.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_ApptType_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_ApptType.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_ApptType();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_ApptType.Enabled = true;
        //}

        //public void SynchDataCrystalPM_ApptType()
        //{
        //    try
        //    {
        //        if (!Is_synched_ApptType && Utility.IsApplicationIdleTimeOff)// && Utility.AditLocationSyncEnable
        //        {
        //            Is_synched_ApptType = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                AditCrystalPM.BAL.Cls_Synch_Appt_Type.SynchDataCrystalPM_Appt_Types(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
        //                ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                IsCrystalPMApptTypeSync = true;
        //                UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType_Push");
        //                ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (Local Database To Adit Server) Successfully.");
        //                IsApptTypeSyncPush = true;

        //            }
        //            Is_synched_ApptType = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_ApptType = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Appointment_Type Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Synch Patient

        //private void fncSynchDataCrystalPM_Patient()
        //{
        //    InitBgWorkerCrystalPM_Patient();
        //    InitBgTimerCrystalPM_Patient();
        //}

        //private void InitBgTimerCrystalPM_Patient()
        //{
        //    timerSynchCrystalPM_Patient = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
        //    this.timerSynchCrystalPM_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_Patient_Tick);
        //    timerSynchCrystalPM_Patient.Enabled = true;
        //    timerSynchCrystalPM_Patient.Start();
        //    timerSynchCrystalPM_Patient_Tick(null, null);
        //}

        //private void InitBgWorkerCrystalPM_Patient()
        //{
        //    bwSynchCrystalPM_Patient = new BackgroundWorker();
        //    bwSynchCrystalPM_Patient.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_Patient.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_Patient.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_Patient_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchCrystalPM_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_Patient_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_Patient_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_Patient.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_Patient();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_Patient()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_Patient = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_Patient);
        //    procThreadmainCrystalPM_Patient.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_Patient()
        //{
        //    if (bwSynchCrystalPM_Patient.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_Patient.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_Patient_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_Patient.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_Patient_New();

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_Patient.Enabled = true;
        //}
        //public void SynchDataCrystalPM_Patient_New()
        //{
        //    try
        //    {
        //        //if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient)//&& Utility.AditLocationSyncEnable
        //        //{
        //        ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Start.");
        //        Is_Synched_PatientCallHit = false;
        //        IsParientFirstSync = false;

        //        for (int i = 0; i < Utility.DtInstallServiceList.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
        //            {
        //                Is_synched_Patient = true;
        //                AditCrystalPM.BAL.Cls_Synch_Patient.SynchDataCrystalPM_Patient(Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
        //                SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
        //                ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[i]["Database"].ToString() + " to Local Database");
        //                SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
        //                ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[i]["Database"].ToString() + " to Local Database");
        //            }
        //        }
        //        Is_synched_Patient = false;
        //        //}
        //        //else if (Is_synched_AppointmentsPatient)
        //        //{
        //        //    Is_Synched_PatientCallHit = true;
        //        //}
        //        Utility.WriteToSyncLogFile_All("Patient Sync Complete Without Error.");
        //    }
        //    catch (Exception ex)
        //    {
        //        //Is_synched_Patient = false;
        //        //IsParientFirstSync = true;
        //    }
        //    finally
        //    {
        //        Is_synched_Patient = false;
        //    }
        //}

        //public void SynchDataCrystalPM_PatientDisease()
        //{
        //    try
        //    {
        //        if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientDisease)
        //        {
        //            Is_synched_PatientDisease = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtCrystalPMDiseaseMain = null;// SynchCrystalPMBAL.GetCrystalPMPatientDiseaseData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                dtCrystalPMDiseaseMain.Columns.Add("InsUptDlt", typeof(int));
        //                DataTable dtLocalDiseaseMain = SynchLocalBAL.GetLocalPatientDiseaseData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
        //                {
        //                    if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
        //                    {
        //                        if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
        //                        {
        //                            DataTable dtCrystalPMDisease = new DataTable(); //dtCrystalPMDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
        //                            if (dtCrystalPMDiseaseMain.Rows.Count > 0 && dtCrystalPMDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Length > 0)
        //                            {
        //                                dtCrystalPMDisease = dtCrystalPMDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
        //                            }
        //                            else
        //                            {
        //                                dtCrystalPMDisease = dtCrystalPMDiseaseMain.Clone();
        //                            }
        //                            DataTable dtLocalDisease = new DataTable();
        //                            if (dtLocalDiseaseMain.Rows.Count > 0 && dtLocalDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Length > 0)
        //                            {
        //                                dtLocalDisease = dtLocalDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
        //                            }
        //                            else
        //                            {
        //                                dtLocalDisease = dtLocalDiseaseMain.Clone();
        //                            }

        //                            DataTable dtSaveRecords = new DataTable();
        //                            dtSaveRecords = dtLocalDisease.Clone();

        //                            var itemsToBeAdded = (from CrystalPM in dtCrystalPMDisease.AsEnumerable()
        //                                                  join Local in dtLocalDisease.AsEnumerable()
        //                                                  on CrystalPM["Disease_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Patient_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Disease_Type"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim() + "_" + CrystalPM["EHR_Entry_DateTime"].ToString().Trim()
        //                                                  equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim() + "_" + Local["EHR_Entry_DateTime"].ToString().Trim()
        //                                                  //on new { DisId = CrystalPM["Disease_EHR_ID"].ToString().Trim(), PatID = CrystalPM["Patient_EHR_ID"].ToString().Trim(), DisType = CrystalPM["Disease_Type"].ToString().Trim(), Clinic_Number = CrystalPM["Clinic_Number"].ToString().Trim(), EHRDT = CrystalPM["EHR_Entry_DateTime"].ToString().Trim() }
        //                                                  //equals new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), PatID = Local["Patient_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), Clinic_Number = Local["Clinic_Number"].ToString().Trim(), EHRDT = Local["EHR_Entry_DateTime"].ToString().Trim() }
        //                                                  into matchingRows
        //                                                  from matchingRow in matchingRows.DefaultIfEmpty()
        //                                                  where matchingRow == null
        //                                                  select CrystalPM).ToList();
        //                            DataTable dtitemToBeAdded = dtLocalDisease.Clone();
        //                            if (itemsToBeAdded.Count > 0)
        //                            {
        //                                dtitemToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
        //                            }
        //                            if (!dtitemToBeAdded.Columns.Contains("InsUptDlt"))
        //                            {
        //                                dtitemToBeAdded.Columns.Add("InsUptDlt", typeof(int));
        //                                dtitemToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
        //                            }
        //                            if (dtitemToBeAdded.Rows.Count > 0)
        //                            {
        //                                dtitemToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
        //                                dtSaveRecords.Load(dtitemToBeAdded.Select().CopyToDataTable().CreateDataReader());
        //                            }

        //                            //Update
        //                            var itemsToBeUpdated = (from CrystalPM in dtCrystalPMDisease.AsEnumerable()
        //                                                    join Local in dtLocalDisease.AsEnumerable()
        //                                                    on CrystalPM["Disease_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Patient_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Disease_Type"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim() + "_" + CrystalPM["EHR_Entry_DateTime"].ToString().Trim()
        //                                                    equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim() + "_" + Local["EHR_Entry_DateTime"].ToString().Trim()
        //                                                    //on new { DisId = CrystalPM["Disease_EHR_ID"].ToString().Trim(), PatID = CrystalPM["Patient_EHR_ID"].ToString().Trim(), DisType = CrystalPM["Disease_Type"].ToString().Trim(), Clinic_Number = CrystalPM["Clinic_Number"].ToString().Trim(), EHRDT = CrystalPM["EHR_Entry_DateTime"].ToString().Trim() }
        //                                                    //equals new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), PatID = Local["Patient_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), Clinic_Number = Local["Clinic_Number"].ToString().Trim(), EHRDT = Local["EHR_Entry_DateTime"].ToString().Trim() }
        //                                                    where
        //                                                   CrystalPM["Patient_EHR_ID"].ToString().Trim() != Local["Patient_EHR_ID"].ToString().Trim() ||
        //                                                   CrystalPM["Disease_EHR_ID"].ToString().Trim() != Local["Disease_EHR_ID"].ToString().Trim() ||
        //                                                   CrystalPM["Disease_Name"].ToString().Trim() != Local["Disease_Name"].ToString().Trim() ||
        //                                                   CrystalPM["Disease_Type"].ToString().Trim() != Local["Disease_Type"].ToString().Trim() ||
        //                                                   CrystalPM["EHR_Entry_DateTime"].ToString().Trim() != Local["EHR_Entry_DateTime"].ToString().Trim()
        //                                                    select CrystalPM).ToList();
        //                            DataTable dtitemToBeUpdated = dtLocalDisease.Clone();
        //                            if (itemsToBeUpdated.Count > 0)
        //                            {
        //                                dtitemToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
        //                            }
        //                            if (!dtitemToBeUpdated.Columns.Contains("InsUptDlt"))
        //                            {
        //                                dtitemToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
        //                                dtitemToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
        //                            }

        //                            if (dtitemToBeUpdated.Rows.Count > 0)
        //                            {
        //                                dtitemToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
        //                                dtSaveRecords.Load(dtitemToBeUpdated.Select().CopyToDataTable().CreateDataReader());
        //                            }

        //                            //Delete
        //                            var itemToBeDeleted = (from Local in dtLocalDisease.AsEnumerable()
        //                                                   join CrystalPM in dtCrystalPMDisease.AsEnumerable()
        //                                                   on Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim() + "_" + Local["EHR_Entry_DateTime"].ToString().Trim()
        //                                                   equals CrystalPM["Disease_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Patient_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Disease_Type"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim() + "_" + CrystalPM["EHR_Entry_DateTime"].ToString().Trim()
        //                                                   //on new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), PatID = Local["Patient_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), Clinic_Number = Local["Clinic_Number"].ToString().Trim(), EHRDT = Local["EHR_Entry_DateTime"].ToString().Trim() }
        //                                                   //equals new { DisId = CrystalPM["Disease_EHR_ID"].ToString().Trim(), PatID = CrystalPM["Patient_EHR_ID"].ToString().Trim(), DisType = CrystalPM["Disease_Type"].ToString().Trim(), Clinic_Number = CrystalPM["Clinic_Number"].ToString().Trim(), EHRDT = CrystalPM["EHR_Entry_DateTime"].ToString().Trim() }
        //                                                   into matchingRows
        //                                                   from matchingRow in matchingRows.DefaultIfEmpty()
        //                                                   where Local["is_deleted"].ToString().Trim().ToUpper() == "FALSE" &&
        //                                                         matchingRow == null
        //                                                   select Local).ToList();
        //                            DataTable dtitemToBeDeleted = dtLocalDisease.Clone();
        //                            if (itemToBeDeleted.Count > 0)
        //                            {
        //                                dtitemToBeDeleted = itemToBeDeleted.CopyToDataTable<DataRow>();
        //                            }
        //                            if (!dtitemToBeDeleted.Columns.Contains("InsUptDlt"))
        //                            {
        //                                dtitemToBeDeleted.Columns.Add("InsUptDlt", typeof(int));
        //                                dtitemToBeDeleted.Columns["InsUptDlt"].DefaultValue = 0;
        //                            }

        //                            if (dtitemToBeDeleted.Rows.Count > 0)
        //                            {
        //                                dtitemToBeDeleted.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 3);
        //                                dtSaveRecords.Load(dtitemToBeDeleted.Select().CopyToDataTable().CreateDataReader());
        //                            }

        //                            if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
        //                            {
        //                                bool status = false;// SynchCrystalPMBAL.Save_PatientDisease_CrystalPM_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                                if (status)
        //                                {
        //                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
        //                                    ObjGoalBase.WriteToSyncLogFile("PatientDisease Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                                    //SynchDataLiveDB_Push_PatientDisease();
        //                                }
        //                                else
        //                                {
        //                                    ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[PatientDisease Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            Is_synched_PatientDisease = false;
        //            try
        //            {
        //                SynchDataLiveDB_Push_PatientDisease();
        //            }
        //            catch (Exception exPush)
        //            {
        //                ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[PatientDisease Push " + exPush.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_PatientDisease = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[PatientDisease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //public void SynchDataCrystalPM_PatientMedication(string Patient_EHR_IDS)
        //{
        //    try
        //    {
        //        if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
        //        {
        //            Is_synched_PatientMedication = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtMedicationMain = null;// SynchCrystalPMBAL.GetCrystalPMPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patient_EHR_IDS);
        //                dtMedicationMain.Columns.Add("InsUptDlt", typeof(int));
        //                DataTable dtLocalMedicationMain = SynchLocalBAL.GetLocalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patient_EHR_IDS);

        //                for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
        //                {
        //                    if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
        //                    {
        //                        if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
        //                        {
        //                            DataTable dtMedication = new DataTable();
        //                            if (dtMedicationMain.Rows.Count > 0 && dtMedicationMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Length > 0)
        //                            {
        //                                dtMedication = dtMedicationMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
        //                            }
        //                            else
        //                            {
        //                                dtMedication = dtMedicationMain.Clone();
        //                            }
        //                            DataTable dtLocalMedication = new DataTable();
        //                            if (dtLocalMedicationMain.Rows.Count > 0 && dtLocalMedicationMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Length > 0)
        //                            {
        //                                dtLocalMedication = dtLocalMedicationMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
        //                            }
        //                            else
        //                            {
        //                                dtLocalMedication = dtLocalMedicationMain.Clone();
        //                            }
        //                            DataTable dtSaveRecords = new DataTable();
        //                            dtSaveRecords = dtLocalMedication.Clone();

        //                            //Insert
        //                            var itemsToBeAdded = (from CrystalPMMedication in dtMedication.AsEnumerable()
        //                                                  join LocalMedication in dtLocalMedication.AsEnumerable()
        //                                                  on CrystalPMMedication["PatientMedication_EHR_ID"].ToString().Trim() equals LocalMedication["PatientMedication_EHR_ID"].ToString().Trim() into matchingRows
        //                                                  from matchingRow in matchingRows.DefaultIfEmpty()
        //                                                  where matchingRow == null
        //                                                  select CrystalPMMedication).ToList();
        //                            DataTable dtPatientToBeAdded = dtLocalMedication.Clone();
        //                            if (itemsToBeAdded.Count > 0)
        //                            {
        //                                dtPatientToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
        //                            }
        //                            if (!dtPatientToBeAdded.Columns.Contains("InsUptDlt"))
        //                            {
        //                                dtPatientToBeAdded.Columns.Add("InsUptDlt", typeof(int));
        //                                dtPatientToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
        //                            }
        //                            if (dtPatientToBeAdded.Rows.Count > 0)
        //                            {
        //                                dtPatientToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
        //                                dtSaveRecords.Load(dtPatientToBeAdded.Select().CopyToDataTable().CreateDataReader());
        //                            }
        //                            //Update
        //                            var itemsToBeUpdated = (from CrystalPMPatient in dtMedication.AsEnumerable()
        //                                                    join LocalPatient in dtLocalMedication.AsEnumerable()
        //                                                    on CrystalPMPatient["PatientMedication_EHR_ID"].ToString().Trim() equals LocalPatient["PatientMedication_EHR_ID"].ToString().Trim()
        //                                                    where
        //                                                    CrystalPMPatient["Patient_EHR_ID"].ToString().Trim() != LocalPatient["Patient_EHR_ID"].ToString().Trim() ||
        //                                                    CrystalPMPatient["Medication_EHR_ID"].ToString().Trim() != LocalPatient["Medication_EHR_ID"].ToString().Trim() ||
        //                                                    CrystalPMPatient["Medication_Note"].ToString().Trim() != LocalPatient["Medication_Note"].ToString().Trim() ||
        //                                                    CrystalPMPatient["Medication_Name"].ToString().Trim() != LocalPatient["Medication_Name"].ToString().Trim() ||
        //                                                    CrystalPMPatient["Medication_Type"].ToString().Trim() != LocalPatient["Medication_Type"].ToString().Trim() ||
        //                                                    CrystalPMPatient["Provider_EHR_ID"].ToString().Trim() != LocalPatient["Provider_EHR_ID"].ToString().Trim() ||
        //                                                    CrystalPMPatient["Drug_Quantity"].ToString().Trim() != LocalPatient["Drug_Quantity"].ToString().Trim() ||
        //                                                    CrystalPMPatient["Start_Date"].ToString().Trim() != LocalPatient["Start_Date"].ToString().Trim() ||
        //                                                    CrystalPMPatient["End_Date"].ToString().Trim() != LocalPatient["End_Date"].ToString().Trim() ||
        //                                                    CrystalPMPatient["Patient_Notes"].ToString().Trim() != LocalPatient["Patient_Notes"].ToString().Trim() ||
        //                                                    CrystalPMPatient["is_active"].ToString().Trim() != LocalPatient["is_active"].ToString().Trim() ||
        //                                                    CrystalPMPatient["EHR_Entry_DateTime"].ToString().Trim() != LocalPatient["EHR_Entry_DateTime"].ToString().Trim()
        //                                                    select CrystalPMPatient).ToList();
        //                            DataTable dtPatientToBeUpdated = dtLocalMedication.Clone();
        //                            if (itemsToBeUpdated.Count > 0)
        //                            {
        //                                dtPatientToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
        //                            }
        //                            if (!dtPatientToBeUpdated.Columns.Contains("InsUptDlt"))
        //                            {
        //                                dtPatientToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
        //                                dtPatientToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
        //                            }

        //                            if (dtPatientToBeUpdated.Rows.Count > 0)
        //                            {
        //                                dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
        //                                dtSaveRecords.Load(dtPatientToBeUpdated.Select().CopyToDataTable().CreateDataReader());
        //                            }
        //                            //Delete
        //                            var itemToBeDeleted = (from LocalMedication in dtLocalMedication.AsEnumerable()
        //                                                   join CrystalPMMedication in dtMedication.AsEnumerable()
        //                                                   on LocalMedication["PatientMedication_EHR_ID"].ToString().Trim() equals CrystalPMMedication["PatientMedication_EHR_ID"].ToString().Trim() into matchingRows
        //                                                   from matchingRow in matchingRows.DefaultIfEmpty()
        //                                                   where LocalMedication["is_deleted"].ToString().Trim().ToUpper() == "FALSE" &&
        //                                                         matchingRow == null
        //                                                   select LocalMedication).ToList();
        //                            DataTable dtPatientToBeDeleted = dtLocalMedication.Clone();
        //                            if (itemToBeDeleted.Count > 0)
        //                            {
        //                                dtPatientToBeDeleted = itemToBeDeleted.CopyToDataTable<DataRow>();
        //                            }
        //                            if (!dtPatientToBeDeleted.Columns.Contains("InsUptDlt"))
        //                            {
        //                                dtPatientToBeDeleted.Columns.Add("InsUptDlt", typeof(int));
        //                                dtPatientToBeDeleted.Columns["InsUptDlt"].DefaultValue = 0;
        //                            }

        //                            if (dtPatientToBeDeleted.Rows.Count > 0)
        //                            {
        //                                dtPatientToBeDeleted.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 3);
        //                                dtSaveRecords.Load(dtPatientToBeDeleted.Select().CopyToDataTable().CreateDataReader());
        //                            }

        //                            if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
        //                            {
        //                                bool status = SynchLocalBAL.Save_PatientMedication_EHR_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                                if (status)
        //                                {
        //                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Medication");
        //                                    ObjGoalBase.WriteToSyncLogFile("PatientMedication Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                                    //SynchDataLiveDB_Push_PatientMedication();
        //                                }
        //                                else
        //                                {
        //                                    ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[PatientMedication Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                Is_synched_PatientMedication = false;
        //            }
        //            try
        //            {
        //                SynchDataLiveDB_Push_PatientMedication();
        //            }
        //            catch (Exception exPush)
        //            {
        //                ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[PatientMedication Push (" + Utility.Application_Name + " to Local Database) ]" + exPush.Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_PatientMedication = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[PatientMedication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Sync Patient Document

        //public void SynchDataLocalToCrystalPM_Patient_Document()
        //{
        //    try
        //    {
        //        //CheckEntryUserLoginIdExist();
        //        if (Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
        //        {
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                try
        //                {
        //                    //live to local data pull
        //                    SynchDataLiveDB_Pull_treatmentDoc();

        //                    //live to local PDF pull
        //                    SyncTreatmentDocument();
        //                }
        //                catch (Exception ex)
        //                {
        //                    ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Treatment Document Error log] : " + ex.Message);
        //                    // throw;
        //                }
        //                DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                dtWebPatient_Form.Columns.Add(new DataColumn("Table_Name", typeof(string)));

        //                try
        //                {
        //                    // GetTreatmentDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                    GetPatientDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                    GetPatientDocument_New(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                    // SynchCrystalPMBAL.Save_Document_in_CrystalPM(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());
        //                }
        //                catch (Exception ex)
        //                {
        //                    ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //                }
        //                try
        //                {
        //                    //Sync data from local to CrystalPM
        //                    // SynchCrystalPMBAL.Save_Treatment_Document_in_CrystalPM(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());

        //                    #region change status as treatment doc impotred Completed
        //                    DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
        //                    if (statusCompleted.Rows.Count > 0)
        //                    {
        //                        Change_Status_TreatmentDoc(statusCompleted, "Completed");
        //                    }
        //                    #endregion
        //                }
        //                catch (Exception ex)
        //                {
        //                    ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Treatment Document Error log] : " + ex.Message);
        //                    // throw;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Patient Form

        //private void fncSynchDataLocalToCrystalPM_Patient_Form()
        //{
        //    InitBgWorkerLocalToCrystalPM_Patient_Form();
        //    InitBgTimerLocalToCrystalPM_Patient_Form();
        //}

        //private void InitBgTimerLocalToCrystalPM_Patient_Form()
        //{
        //    timerSynchLocalToCrystalPM_Patient_Form = new System.Timers.Timer();
        //    this.timerSynchLocalToCrystalPM_Patient_Form.Interval = 1000 * GoalBase.intervalEHRSynch_PatientForm;
        //    this.timerSynchLocalToCrystalPM_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToCrystalPM_Patient_Form_Tick);
        //    timerSynchLocalToCrystalPM_Patient_Form.Enabled = true;
        //    timerSynchLocalToCrystalPM_Patient_Form.Start();
        //    timerSynchLocalToCrystalPM_Patient_Form_Tick(null, null);
        //}

        //private void InitBgWorkerLocalToCrystalPM_Patient_Form()
        //{
        //    bwSynchLocalToCrystalPM_Patient_Form = new BackgroundWorker();
        //    bwSynchLocalToCrystalPM_Patient_Form.WorkerReportsProgress = true;
        //    bwSynchLocalToCrystalPM_Patient_Form.WorkerSupportsCancellation = true;
        //    bwSynchLocalToCrystalPM_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToCrystalPM_Patient_Form_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchLocalToCrystalPM_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToCrystalPM_Patient_Form_RunWorkerCompleted);
        //}

        //private void timerSynchLocalToCrystalPM_Patient_Form_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLocalToCrystalPM_Patient_Form.Enabled = false;
        //        MethodForCallSynchOrderLocalToCrystalPM_Patient_Form();
        //    }
        //}

        //public void MethodForCallSynchOrderLocalToCrystalPM_Patient_Form()
        //{
        //    System.Threading.Thread procThreadmainLocalToCrystalPM_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToCrystalPM_Patient_Form);
        //    procThreadmainLocalToCrystalPM_Patient_Form.Start();
        //}

        //public void CallSyncOrderTableLocalToCrystalPM_Patient_Form()
        //{
        //    if (bwSynchLocalToCrystalPM_Patient_Form.IsBusy != true)
        //    {
        //        bwSynchLocalToCrystalPM_Patient_Form.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLocalToCrystalPM_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLocalToCrystalPM_Patient_Form.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLocalToCrystalPM_Patient_Form();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchLocalToCrystalPM_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLocalToCrystalPM_Patient_Form.Enabled = true;
        //}

        //public void SynchDataLocalToCrystalPM_Patient_Form()
        //{
        //    try
        //    {
        //        //CheckEntryUserLoginIdExist();
        //        if (Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
        //        {
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                AditCrystalPM.BAL.Cls_Synch_PatientForm.SynchDataCrystalPM_PatientFormToEHR(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), "0");
        //                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //    }

        //}
        //#endregion

        //#region PatientPayment
        //private void fncSynchDataCrystalPM_PatientPayment()
        //{
        //    InitBgWorkerCrystalPM_PatientPayment();
        //    InitBgTimerCrystalPM_PatientPayment();
        //}

        //private void InitBgTimerCrystalPM_PatientPayment()
        //{
        //    timerSynchCrystalPM_PatientPayment = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
        //    this.timerSynchCrystalPM_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_PatientPayment_Tick);
        //    timerSynchCrystalPM_PatientPayment.Enabled = true;
        //    timerSynchCrystalPM_PatientPayment.Start();
        //}

        //private void InitBgWorkerCrystalPM_PatientPayment()
        //{
        //    bwSynchCrystalPM_PatientPayment = new BackgroundWorker();
        //    bwSynchCrystalPM_PatientPayment.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_PatientPayment.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_PatientPayment_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchCrystalPM_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_PatientPayment_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_PatientPayment_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_PatientPayment.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_PatientPayment();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_PatientPayment()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_PatientPayment);
        //    procThreadmainCrystalPM_PatientPayment.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_PatientPayment()
        //{
        //    if (bwSynchCrystalPM_PatientPayment.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_PatientPayment.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_PatientPayment.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLocalToCrystalPM_PatientPayment();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_PatientPayment.Enabled = true;
        //}

        //public void SynchDataLocalToCrystalPM_PatientPayment()
        //{
        //    try
        //    {
        //        if (!IsPaymentSyncing)
        //        {
        //            IsPaymentSyncing = true;
        //            //CheckEntryUserLoginIdExist();
        //            SynchDataLiveDB_Pull_PatientPaymentLog();
        //            if (!Is_synched_PatinetForm)
        //            {
        //                if (Utility.IsApplicationIdleTimeOff)
        //                {
        //                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //                    {
        //                        DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                        if (dtWebPatientPayment.Rows.Count > 0 && dtWebPatientPayment.Rows.Count > 0)
        //                        {
        //                            Utility.CheckEntryUserLoginIdExist();
        //                            bool Is_Record_Update = false;//SynchCrystalPMBAL.Save_PatientPayment_Local_To_CrystalPM(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                        }

        //                        //string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                        //if (Call_Importing.ToLower() != "success")
        //                        //{
        //                        //    ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form API error with Importing status. Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " : " + Call_Importing);
        //                        //}

        //                        //string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                        //if (Call_Completed.ToLower() != "success")
        //                        //{
        //                        //    ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form API error with Completed status.Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " : " + Call_Completed);
        //                        //}

        //                        ObjGoalBase.WriteToSyncLogFile("Patient Payment Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");

        //                    }
        //                }
        //            }
        //            IsPaymentSyncing = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        IsPaymentSyncing = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //    }
        //    finally { IsPaymentSyncing = false; }
        //}

        //public void SynchDataPatientPayment_LocalTOCrystalPM()
        //{
        //    try
        //    {
        //        //CheckEntryUserLoginIdExist();
        //        if (Utility.IsApplicationIdleTimeOff)
        //        {
        //            Int64 TransactionHeaderId = 0;
        //            string noteId = "";
        //            DataTable dtPatientPayment = new DataTable();
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
        //                {
        //                    noteId = "";
        //                    bool Is_Record_Update = false;// SynchCrystalPMBAL.Save_PatientPayment_Local_To_CrystalPM(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                }
        //                else
        //                {
        //                    ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

        //                }
        //                //  }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient Payment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //    }
        //}

        //#endregion
        //public void SynchDataCrystalPM_PatientImages()
        //{
        //    try
        //    {
        //        if (Utility.IsExternalAppointmentSync)
        //        {
        //            Is_synched_PatientImages = false;
        //        }
        //        if (!Is_synched_PatientImages && Utility.IsApplicationIdleTimeOff)
        //        {
        //            // SynchDataLiveDB_Push_PatientImage();

        //            Is_synched_PatientImages = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtCrystalPMPatientImages = null;// SynchCrystalPMBAL.GetCrystalPMPatientImagesData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
        //                dtCrystalPMPatientImages.Columns.Add("InsUptDlt", typeof(int));
        //                dtCrystalPMPatientImages.Columns.Add("SourceLocation", typeof(string));
        //                dtCrystalPMPatientImages.Columns["InsUptDlt"].DefaultValue = 0;
        //                DataTable dtLocalPatientImages = SynchLocalBAL.GetLocalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString());
        //                Utility.EHRProfileImagePath = "";// SynchCrystalPMDAL.GetCrystalPMDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
        //                foreach (DataRow dtDtxRow in dtCrystalPMPatientImages.Rows)
        //                {
        //                    if (Utility.EHRProfileImagePath == string.Empty || Utility.EHRProfileImagePath == "")
        //                    {
        //                        dtDtxRow["SourceLocation"] = "C:\\OpenDentImages\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
        //                    }
        //                    else
        //                    {
        //                        if (!Directory.Exists(Utility.EHRProfileImagePath))
        //                        {
        //                            //Utility.WriteToErrorLogFromAll(Utility.EHRProfileImagePath.ToString());
        //                            dtDtxRow["SourceLocation"] = "C:\\OpenDentImages\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
        //                        }
        //                        else
        //                        {
        //                            dtDtxRow["SourceLocation"] = Utility.EHRProfileImagePath + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
        //                        }
        //                    }
        //                    //Utility.WriteToErrorLogFromAll(dtDtxRow["SourceLocation"].ToString());
        //                    DataRow[] row = dtLocalPatientImages.Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
        //                    if (row.Length > 0)
        //                    {
        //                        if (!Convert.ToBoolean(row[0]["Is_Deleted"]))
        //                        {
        //                            if (dtDtxRow["Patient_Images_EHR_ID"].ToString().Trim() != row[0]["Patient_Images_EHR_ID"].ToString().Trim())
        //                            {
        //                                dtDtxRow["InsUptDlt"] = 2;
        //                                dtDtxRow["Is_Adit_Updated"] = 0;
        //                            }
        //                            else if (dtDtxRow["Patient_Images_FilePath"].ToString() != row[0]["Patient_Images_FilePath"].ToString())
        //                            {
        //                                dtDtxRow["InsUptDlt"] = 2;
        //                                dtDtxRow["Is_Adit_Updated"] = 0;
        //                            }
        //                            else
        //                            {
        //                                dtDtxRow["InsUptDlt"] = 0;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 1;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 1;
        //                    }
        //                }
        //                foreach (DataRow dtDtlRow in dtLocalPatientImages.Rows)
        //                {
        //                    DataRow[] row = dtCrystalPMPatientImages.Select("Patient_EHR_ID = '" + dtDtlRow["Patient_EHR_ID"].ToString().Trim() + "' ");
        //                    if (row.Length <= 0)
        //                    {
        //                        if (!Convert.ToBoolean(dtDtlRow["Is_Deleted"]))
        //                        {
        //                            DataRow ApptDtldr = dtCrystalPMPatientImages.NewRow();
        //                            ApptDtldr["Patient_EHR_ID"] = dtDtlRow["Patient_EHR_ID"].ToString().Trim();
        //                            ApptDtldr["Patient_Images_EHR_ID"] = dtDtlRow["Patient_Images_EHR_ID"].ToString().Trim();
        //                            ApptDtldr["Image_EHR_Name"] = dtDtlRow["Image_EHR_Name"].ToString().Trim();
        //                            ApptDtldr["Clinic_Number"] = dtDtlRow["Clinic_Number"].ToString().Trim();
        //                            ApptDtldr["Service_Install_Id"] = dtDtlRow["Service_Install_Id"].ToString().Trim();
        //                            ApptDtldr["Is_Deleted"] = 1;
        //                            ApptDtldr["InsUptDlt"] = 3;
        //                            dtCrystalPMPatientImages.Rows.Add(ApptDtldr);
        //                        }

        //                    }
        //                }

        //                dtCrystalPMPatientImages.AcceptChanges();
        //                bool status = false;
        //                DataTable dtSaveRecords = dtCrystalPMPatientImages.Clone();
        //                if (dtCrystalPMPatientImages.Select("InsUptDlt IN (1,2,3)").Count() > 0)
        //                {
        //                    dtSaveRecords.Load(dtCrystalPMPatientImages.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
        //                    status = SynchLocalBAL.Save_PatientProfileImage_EHR_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                }

        //                if (status)
        //                {
        //                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Images");
        //                    ObjGoalBase.WriteToSyncLogFile("PatientImage Sync (" + Utility.Application_Name + " to Local Database) Successfully.");


        //                }
        //                SynchDataLiveDB_Push_PatientImage(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //            }
        //            Is_synched_PatientImages = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_PatientImages = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[PatientImages Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //public void SynchDataPatientSMSCall_LocalTOCrystalPM()
        //{
        //    try
        //    {
        //        if (Utility.IsApplicationIdleTimeOff)
        //        {
        //            //CheckEntryUserLoginIdExist();
        //            Int64 TransactionHeaderId = 0;
        //            string noteId = "";
        //            DataTable dtPatientPayment = new DataTable();
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                // SynchCrystalPMBAL.DeleteDuplicatePatientLog(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                // ObjGoalBase.WriteToSyncLogFile("Get Records");
        //                DataTable dtWebPatientSMSCallLog = SynchLocalBAL.GetLocalWebPatientSMSCallLogData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                // ObjGoalBase.WriteToSyncLogFile("Total Records to be saved in EHR " + dtWebPatientSMSCallLog.Rows.Count.ToString());
        //                #region Call API for EHR Entry Done
        //                if (dtWebPatientSMSCallLog != null && dtWebPatientSMSCallLog.Rows.Count > 0)
        //                {
        //                    Utility.CheckEntryUserLoginIdExist();
        //                    // System.Windows.Forms.MessageBox.Show("0");
        //                    // SynchCrystalPMBAL.Save_PatientSMSCallLog_LocalToCrystalPM(dtWebPatientSMSCallLog, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                    // }
        //                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientSMSCallLog_LocalTOCrystalPM");
        //                    ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
        //                }
        //                else
        //                {
        //                    ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

        //                }

        //                #endregion
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[PatientSMSCallLog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //    }
        //}


        //#region Synch Disease

        //private void fncSynchDataCrystalPM_Disease()
        //{
        //    InitBgWorkerCrystalPM_Disease();
        //    InitBgTimerCrystalPM_Disease();
        //}

        //private void InitBgTimerCrystalPM_Disease()
        //{
        //    timerSynchCrystalPM_Disease = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
        //    this.timerSynchCrystalPM_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_Disease_Tick);
        //    timerSynchCrystalPM_Disease.Enabled = true;
        //    timerSynchCrystalPM_Disease.Start();
        //}

        //private void InitBgWorkerCrystalPM_Disease()
        //{
        //    bwSynchCrystalPM_Disease = new BackgroundWorker();
        //    bwSynchCrystalPM_Disease.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_Disease.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_Disease.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_Disease_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchCrystalPM_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_Disease_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_Disease_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_Disease.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_Disease();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_Disease()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_Disease = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_Disease);
        //    procThreadmainCrystalPM_Disease.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_Disease()
        //{
        //    if (bwSynchCrystalPM_Disease.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_Disease.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_Disease_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_Disease.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_Disease();
        //        //  SynchDataCrystalPM_DiseaseHours();
        //        //  SynchDataCrystalPM_Medication();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_Disease.Enabled = true;
        //}

        //public void SynchDataCrystalPM_Disease()
        //{
        //    try
        //    {
        //        if (Utility.IsApplicationIdleTimeOff)
        //        {
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtCrystalPMDisease = null;// SynchCrystalPMBAL.GetCrystalPMDiseaseData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                dtCrystalPMDisease.Columns.Add("InsUptDlt", typeof(int));
        //                DataTable dtLocalDisease = SynchLocalBAL.GetLocalDiseaseData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                DataTable dtSaveRecords = new DataTable();
        //                dtSaveRecords = dtLocalDisease.Clone();

        //                //Insert
        //                var itemsToBeAdded = (from CrystalPM in dtCrystalPMDisease.AsEnumerable()
        //                                      join Local in dtLocalDisease.AsEnumerable()
        //                                      on CrystalPM["Disease_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Disease_Type"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim()
        //                                      equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
        //                                      //on new { DisId = CrystalPM["Disease_EHR_ID"].ToString().Trim(), DisType = CrystalPM["Disease_Type"].ToString().Trim(), ClinicNum = CrystalPM["Clinic_Number"].ToString().Trim() }
        //                                      //equals new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), ClinicNum = Local["Clinic_Number"].ToString().Trim() }
        //                                      into matchingRows
        //                                      from matchingRow in matchingRows.DefaultIfEmpty()
        //                                      where matchingRow == null
        //                                      select CrystalPM).ToList();
        //                DataTable dtPatientToBeAdded = dtLocalDisease.Clone();
        //                if (itemsToBeAdded.Count > 0)
        //                {
        //                    dtPatientToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
        //                }
        //                if (!dtPatientToBeAdded.Columns.Contains("InsUptDlt"))
        //                {
        //                    dtPatientToBeAdded.Columns.Add("InsUptDlt", typeof(int));
        //                    dtPatientToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
        //                }
        //                if (dtPatientToBeAdded.Rows.Count > 0)
        //                {
        //                    dtPatientToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
        //                    dtSaveRecords.Load(dtPatientToBeAdded.Select().CopyToDataTable().CreateDataReader());
        //                }

        //                //Update
        //                var itemsToBeUpdated = (from CrystalPM in dtCrystalPMDisease.AsEnumerable()
        //                                        join Local in dtLocalDisease.AsEnumerable()
        //                                        on CrystalPM["Disease_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Disease_Type"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim()
        //                                        equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
        //                                        //on new { DisId = CrystalPM["Disease_EHR_ID"].ToString().Trim(), DisType = CrystalPM["Disease_Type"].ToString().Trim(), ClinicNum = CrystalPM["Clinic_Number"].ToString().Trim() }
        //                                        //equals new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), ClinicNum = Local["Clinic_Number"].ToString().Trim() }
        //                                        where
        //                                       CrystalPM["EHR_Entry_DateTime"].ToString().Trim() != Local["EHR_Entry_DateTime"].ToString().Trim()
        //                                        select CrystalPM).ToList();
        //                DataTable dtPatientToBeUpdated = dtLocalDisease.Clone();
        //                if (itemsToBeUpdated.Count > 0)
        //                {
        //                    dtPatientToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
        //                }
        //                if (!dtPatientToBeUpdated.Columns.Contains("InsUptDlt"))
        //                {
        //                    dtPatientToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
        //                    dtPatientToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
        //                }

        //                if (dtPatientToBeUpdated.Rows.Count > 0)
        //                {
        //                    dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
        //                    dtSaveRecords.Load(dtPatientToBeUpdated.Select().CopyToDataTable().CreateDataReader());
        //                }

        //                //Delete
        //                var itemToBeDeleted = (from Local in dtLocalDisease.AsEnumerable()
        //                                       join CrystalPM in dtCrystalPMDisease.AsEnumerable()
        //                                       on Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
        //                                       equals CrystalPM["Disease_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Disease_Type"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim()
        //                                      //on new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), ClinicNum = Local["Clinic_Number"].ToString().Trim() }
        //                                      //equals new { DisId = CrystalPM["Disease_EHR_ID"].ToString().Trim(), DisType = CrystalPM["Disease_Type"].ToString().Trim(), ClinicNum = CrystalPM["Clinic_Number"].ToString().Trim() }
        //                                      into matchingRows
        //                                       from matchingRow in matchingRows.DefaultIfEmpty()
        //                                       where Local["is_deleted"].ToString().Trim().ToUpper() == "FALSE" &&
        //                                             matchingRow == null
        //                                       select Local).ToList();
        //                DataTable dtPatientToBeDeleted = dtLocalDisease.Clone();
        //                if (itemToBeDeleted.Count > 0)
        //                {
        //                    dtPatientToBeDeleted = itemToBeDeleted.CopyToDataTable<DataRow>();
        //                }
        //                if (!dtPatientToBeDeleted.Columns.Contains("InsUptDlt"))
        //                {
        //                    dtPatientToBeDeleted.Columns.Add("InsUptDlt", typeof(int));
        //                    dtPatientToBeDeleted.Columns["InsUptDlt"].DefaultValue = 0;
        //                }

        //                if (dtPatientToBeDeleted.Rows.Count > 0)
        //                {
        //                    dtPatientToBeDeleted.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 3);
        //                    dtSaveRecords.Load(dtPatientToBeDeleted.Select().CopyToDataTable().CreateDataReader());
        //                }

        //                //foreach (DataRow dtDtxRow in dtCrystalPMDisease.Rows)
        //                //{
        //                //    DataRow[] row = dtLocalDisease.Copy().Select("Disease_EHR_ID = '" + dtDtxRow["Disease_EHR_ID"] + "' AND Disease_Type = '" + dtDtxRow["Disease_Type"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
        //                //    if (row.Length > 0)
        //                //    {
        //                //        if (dtDtxRow["EHR_Entry_DateTime"].ToString().Trim() != row[0]["EHR_Entry_DateTime"].ToString().Trim())
        //                //        {
        //                //            dtDtxRow["InsUptDlt"] = 2;
        //                //        }
        //                //        else
        //                //        {
        //                //            dtDtxRow["InsUptDlt"] = 0;
        //                //        }
        //                //    }
        //                //    else
        //                //    {
        //                //        dtDtxRow["InsUptDlt"] = 1;
        //                //    }
        //                //}

        //                //foreach (DataRow dtLPHRow in dtLocalDisease.Rows)
        //                //{
        //                //    DataRow[] rowDis = dtCrystalPMDisease.Copy().Select("Disease_EHR_ID = '" + dtLPHRow["Disease_EHR_ID"] + "' AND Disease_Type = '" + dtLPHRow["Disease_Type"] + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"] + "' ");
        //                //    if (rowDis.Length > 0)
        //                //    { }
        //                //    else
        //                //    {
        //                //        DataRow rowDisDtldr = dtCrystalPMDisease.NewRow();
        //                //        rowDisDtldr["Disease_EHR_ID"] = dtLPHRow["Disease_EHR_ID"].ToString().Trim();
        //                //        rowDisDtldr["Disease_Type"] = dtLPHRow["Disease_Type"].ToString().Trim();
        //                //        rowDisDtldr["Disease_Name"] = dtLPHRow["Disease_Name"].ToString().Trim();
        //                //        //  rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
        //                //        rowDisDtldr["InsUptDlt"] = 3;
        //                //        dtCrystalPMDisease.Rows.Add(rowDisDtldr);
        //                //    }
        //                //}
        //                //dtCrystalPMDisease.AcceptChanges();

        //                if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
        //                {
        //                    bool status = false; // SynchCrystalPMBAL.Save_Disease_CrystalPM_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                    if (status)
        //                    {
        //                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
        //                        ObjGoalBase.WriteToSyncLogFile("Disease Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                        SynchDataLiveDB_Push_Disease();
        //                    }
        //                    else
        //                    {
        //                        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Disease Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
        //                    }
        //                }

        //            }
        //            SynchDataCrystalPM_Medication();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}


        //public void SynchDataCrystalPM_Medication()
        //{
        //    try
        //    {
        //        if (Utility.IsApplicationIdleTimeOff)
        //        {
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtMedication = null;// SynchCrystalPMBAL.GetCrystalPMMedicationData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                dtMedication.Columns.Add("InsUptDlt", typeof(int));
        //                DataTable dtLocalMedication = SynchLocalBAL.GetLocalMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                DataTable dtSaveRecords = new DataTable();
        //                dtSaveRecords = dtLocalMedication.Clone();

        //                //Insert
        //                var itemsToBeAdded = (from CrystalPM in dtMedication.AsEnumerable()
        //                                      join Local in dtLocalMedication.AsEnumerable()
        //                                      on CrystalPM["Medication_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim()
        //                                      equals Local["Medication_EHR_ID"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim() into matchingRows
        //                                      from matchingRow in matchingRows.DefaultIfEmpty()
        //                                      where matchingRow == null
        //                                      select CrystalPM).ToList();
        //                DataTable dtPatientToBeAdded = dtLocalMedication.Clone();
        //                if (itemsToBeAdded.Count > 0)
        //                {
        //                    dtPatientToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
        //                }
        //                if (!dtPatientToBeAdded.Columns.Contains("InsUptDlt"))
        //                {
        //                    dtPatientToBeAdded.Columns.Add("InsUptDlt", typeof(int));
        //                    dtPatientToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
        //                }
        //                if (dtPatientToBeAdded.Rows.Count > 0)
        //                {
        //                    dtPatientToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
        //                    dtSaveRecords.Load(dtPatientToBeAdded.Select().CopyToDataTable().CreateDataReader());
        //                }

        //                //Update
        //                var itemsToBeUpdated = (from CrystalPM in dtMedication.AsEnumerable()
        //                                        join Local in dtLocalMedication.AsEnumerable()
        //                                        on CrystalPM["Medication_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim()
        //                                        equals Local["Medication_EHR_ID"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
        //                                        where
        //                                        CrystalPM["Medication_Name"].ToString().ToUpper().Trim() != Local["Medication_Name"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Drug_Quantity"].ToString().ToUpper().Trim() != Local["Drug_Quantity"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Medication_Description"].ToString().ToUpper().Trim() != Local["Medication_Description"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Medication_Notes"].ToString().ToUpper().Trim() != Local["Medication_Notes"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Medication_Sig"].ToString().ToUpper().Trim() != Local["Medication_Sig"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim() != Local["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Medication_Type"].ToString().ToUpper().Trim() != Local["Medication_Type"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Allow_Generic_Sub"].ToString().ToUpper().Trim() != Local["Allow_Generic_Sub"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Refills"].ToString().ToUpper().Trim() != Local["Refills"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Is_Active"].ToString().ToUpper().Trim() != Local["Is_Active"].ToString().ToUpper().Trim() ||
        //                                        CrystalPM["Medication_Provider_ID"].ToString().ToUpper().Trim() != Local["Medication_Provider_ID"].ToString().ToUpper().Trim()
        //                                        select CrystalPM).ToList();
        //                DataTable dtPatientToBeUpdated = dtLocalMedication.Clone();
        //                if (itemsToBeUpdated.Count > 0)
        //                {
        //                    dtPatientToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
        //                }
        //                if (!dtPatientToBeUpdated.Columns.Contains("InsUptDlt"))
        //                {
        //                    dtPatientToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
        //                    dtPatientToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
        //                }

        //                if (dtPatientToBeUpdated.Rows.Count > 0)
        //                {
        //                    dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
        //                    dtSaveRecords.Load(dtPatientToBeUpdated.Select().CopyToDataTable().CreateDataReader());
        //                }

        //                //Delete
        //                var itemToBeDeleted = (from Local in dtLocalMedication.AsEnumerable()
        //                                       join CrystalPM in dtMedication.AsEnumerable()
        //                                       on Local["Medication_EHR_ID"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
        //                                       equals CrystalPM["Medication_EHR_ID"].ToString().Trim() + "_" + CrystalPM["Clinic_Number"].ToString().Trim()
        //                                       into matchingRows
        //                                       from matchingRow in matchingRows.DefaultIfEmpty()
        //                                       where Local["is_deleted"].ToString().Trim().ToUpper() == "FALSE" &&
        //                                             matchingRow == null
        //                                       select Local).ToList();
        //                DataTable dtPatientToBeDeleted = dtLocalMedication.Clone();
        //                if (itemToBeDeleted.Count > 0)
        //                {
        //                    dtPatientToBeDeleted = itemToBeDeleted.CopyToDataTable<DataRow>();
        //                }
        //                if (!dtPatientToBeDeleted.Columns.Contains("InsUptDlt"))
        //                {
        //                    dtPatientToBeDeleted.Columns.Add("InsUptDlt", typeof(int));
        //                    dtPatientToBeDeleted.Columns["InsUptDlt"].DefaultValue = 0;
        //                }

        //                if (dtPatientToBeDeleted.Rows.Count > 0)
        //                {
        //                    dtPatientToBeDeleted.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 3);
        //                    dtSaveRecords.Load(dtPatientToBeDeleted.Select().CopyToDataTable().CreateDataReader());
        //                }

        //                if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
        //                {
        //                    bool status = SynchLocalDAL.Save_Medication_EHR_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                    if (status)
        //                    {
        //                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Medication");
        //                        ObjGoalBase.WriteToSyncLogFile("Medication Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                        SynchDataLiveDB_Push_Medication();
        //                    }
        //                    else
        //                    {
        //                        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Medication Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Medication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Synch RecallType

        //private void fncSynchDataCrystalPM_RecallType()
        //{
        //    InitBgWorkerCrystalPM_RecallType();
        //    InitBgTimerCrystalPM_RecallType();
        //}

        //private void InitBgTimerCrystalPM_RecallType()
        //{
        //    timerSynchCrystalPM_RecallType = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
        //    this.timerSynchCrystalPM_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_RecallType_Tick);
        //    timerSynchCrystalPM_RecallType.Enabled = true;
        //    timerSynchCrystalPM_RecallType.Start();
        //}

        //private void InitBgWorkerCrystalPM_RecallType()
        //{
        //    bwSynchCrystalPM_RecallType = new BackgroundWorker();
        //    bwSynchCrystalPM_RecallType.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_RecallType.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_RecallType.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_RecallType_DoWork);
        //    bwSynchCrystalPM_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_RecallType_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_RecallType_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_RecallType.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_RecallType();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_RecallType()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_RecallType = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_RecallType);
        //    procThreadmainCrystalPM_RecallType.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_RecallType()
        //{
        //    if (bwSynchCrystalPM_RecallType.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_RecallType.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_RecallType_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_RecallType.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_RecallType();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_RecallType.Enabled = true;
        //}

        //public void SynchDataCrystalPM_RecallType()
        //{
        //    try
        //    {
        //        if (!Is_synched_RecallType && Utility.IsApplicationIdleTimeOff)
        //        {
        //            Is_synched_RecallType = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                AditCrystalPM.BAL.Cls_Synch_Recall_Type.SynchDataCrystalPM_Recall_Types(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
        //                SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
        //                ObjGoalBase.WriteToSyncLogFile("RecallType Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                SynchLocalBAL.Update_Sync_Table_Datetime("RecallType_Push");
        //                ObjGoalBase.WriteToSyncLogFile("RecallType Sync (Local Database To Adit Server) Successfully.");
        //            }
        //            Is_synched_RecallType = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_RecallType = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[RecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Synch User
        //private void fncSynchDataCrystalPM_User()
        //{
        //    InitBgWorkerCrystalPM_User();
        //    InitBgTimerCrystalPM_User();
        //}

        //private void InitBgTimerCrystalPM_User()
        //{
        //    timerSynchCrystalPM_User = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
        //    this.timerSynchCrystalPM_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_User_Tick);
        //    timerSynchCrystalPM_User.Enabled = true;
        //    timerSynchCrystalPM_User.Start();
        //}

        //private void timerSynchCrystalPM_User_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_User.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_User();
        //    }
        //}

        //private void MethodForCallSynchOrderCrystalPM_User()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_User = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_User);
        //    procThreadmainCrystalPM_User.Start();
        //}

        //private void CallSyncOrderTableCrystalPM_User()
        //{
        //    if (bwSynchCrystalPM_User.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_User.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void InitBgWorkerCrystalPM_User()
        //{
        //    bwSynchCrystalPM_User = new BackgroundWorker();
        //    bwSynchCrystalPM_User.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_User.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_User.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_User_DoWork);
        //    bwSynchCrystalPM_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_User_RunWorkerCompleted);
        //}

        //private void bwSynchCrystalPM_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_User.Enabled = true;
        //}

        //private void bwSynchCrystalPM_User_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_User.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_User();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void SynchDataCrystalPM_User()
        //{
        //    if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
        //    {
        //        try
        //        {
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                Utility.WriteToSyncLogFile_All("user synch start");
        //                AditCrystalPM.BAL.Cls_Synch_User.SynchDataCrystalPM_User(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
        //                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Users");
        //                ObjGoalBase.WriteToSyncLogFile("User Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("User_Push");
        //                ObjGoalBase.WriteToSyncLogFile("User Sync (Local Database To Adit Server) Successfully.");
        //                SynchDataLiveDB_Push_User();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[User Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //        }
        //    }

        //}
        //#endregion

        //#region Synch ApptStatus

        //private void fncSynchDataCrystalPM_ApptStatus()
        //{
        //    InitBgWorkerCrystalPM_ApptStatus();
        //    InitBgTimerCrystalPM_ApptStatus();
        //}

        //private void InitBgTimerCrystalPM_ApptStatus()
        //{
        //    timerSynchCrystalPM_ApptStatus = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
        //    this.timerSynchCrystalPM_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_ApptStatus_Tick);
        //    timerSynchCrystalPM_ApptStatus.Enabled = true;
        //    timerSynchCrystalPM_ApptStatus.Start();
        //}

        //private void InitBgWorkerCrystalPM_ApptStatus()
        //{
        //    bwSynchCrystalPM_ApptStatus = new BackgroundWorker();
        //    bwSynchCrystalPM_ApptStatus.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_ApptStatus.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_ApptStatus_DoWork);
        //    bwSynchCrystalPM_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_ApptStatus_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_ApptStatus_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_ApptStatus.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_ApptStatus();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_ApptStatus()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_ApptStatus);
        //    procThreadmainCrystalPM_ApptStatus.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_ApptStatus()
        //{
        //    if (bwSynchCrystalPM_ApptStatus.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_ApptStatus.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_ApptStatus.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_ApptStatus();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_ApptStatus.Enabled = true;
        //}

        //public void SynchDataCrystalPM_ApptStatus()
        //{
        //    try
        //    {
        //        if (!Is_synched_ApptStatus && Utility.IsApplicationIdleTimeOff)
        //        {
        //            Is_synched_ApptStatus = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                AditCrystalPM.BAL.Cls_Synch_Appt_Status.SynchDataCrystalPM_Appt_Status(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
        //                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
        //                ObjGoalBase.WriteToSyncLogFile("ApptStatus Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus_Push");
        //                ObjGoalBase.WriteToSyncLogFile("Appointment Status Sync (Local Database To Adit Server) Successfully.");
        //            }

        //            Is_synched_ApptStatus = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_ApptStatus = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[ApptStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Synch Holiday

        //private void fncSynchDataCrystalPM_Holiday()
        //{
        //    InitBgWorkerCrystalPM_Holiday();
        //    InitBgTimerCrystalPM_Holiday();
        //}

        //private void InitBgTimerCrystalPM_Holiday()
        //{
        //    timerSynchCrystalPM_Holiday = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_Holiday.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
        //    this.timerSynchCrystalPM_Holiday.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_Holiday_Tick);
        //    timerSynchCrystalPM_Holiday.Enabled = true;
        //    timerSynchCrystalPM_Holiday.Start();
        //}

        //private void InitBgWorkerCrystalPM_Holiday()
        //{
        //    bwSynchCrystalPM_Holiday = new BackgroundWorker();
        //    bwSynchCrystalPM_Holiday.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_Holiday.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_Holiday.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_Holiday_DoWork);
        //    bwSynchCrystalPM_Holiday.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_Holiday_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_Holiday_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_Holiday.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_Holiday();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_Holiday()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_Holiday = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_Holiday);
        //    procThreadmainCrystalPM_Holiday.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_Holiday()
        //{
        //    if (bwSynchCrystalPM_Holiday.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_Holiday.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_Holiday_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_Holiday.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_Holiday();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_Holiday_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_Holiday.Enabled = true;
        //}

        //public void SynchDataCrystalPM_Holiday()
        //{
        //    try
        //    {
        //        if (!Is_synched_Holidays && Utility.IsApplicationIdleTimeOff)
        //        {
        //            Is_synched_Holidays = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtCrystalPMHoliday = null;// SynchCrystalPMBAL.GetCrystalPMHolidayData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
        //                dtCrystalPMHoliday.Columns.Add("InsUptDlt", typeof(int));
        //                DataTable dtLocalHoliday = SynchLocalBAL.GetLocalHolidayData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //                dtCrystalPMHoliday = CommonUtility.AddHolidays(dtCrystalPMHoliday, dtLocalHoliday, "SchedDate", "Note", "ScheduleNum");

        //                foreach (DataRow dtDtxRow in dtCrystalPMHoliday.Rows)
        //                {
        //                    DataRow[] row = dtLocalHoliday.Copy().Select("H_EHR_ID = '" + dtDtxRow["ScheduleNum"] + "'");
        //                    if (row.Length > 0)
        //                    {
        //                        if (dtDtxRow["Note"].ToString().Trim() != (row[0]["comment"]).ToString().Trim())
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 2;
        //                        }
        //                        else
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 0;
        //                            //dtDtxRow["DateTStamp"] = Utility.GetCurrentDatetimestring();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 1;
        //                        dtDtxRow["Clinic_Number"] = 0;
        //                        dtDtxRow["DateTStamp"] = Utility.GetCurrentDatetimestring();
        //                    }
        //                }

        //                foreach (DataRow dtDtxRow in dtLocalHoliday.Rows)
        //                {
        //                    DataRow[] row = dtCrystalPMHoliday.Copy().Select("ScheduleNum = '" + dtDtxRow["H_EHR_ID"] + "'");
        //                    if (row.Length <= 0)
        //                    {
        //                        DataRow ApptDtldr = dtCrystalPMHoliday.NewRow();
        //                        ApptDtldr["ScheduleNum"] = dtDtxRow["H_EHR_ID"].ToString().Trim();
        //                        ApptDtldr["InsUptDlt"] = 3;
        //                        dtCrystalPMHoliday.Rows.Add(ApptDtldr);
        //                    }
        //                }

        //                dtCrystalPMHoliday.AcceptChanges();

        //                if (dtCrystalPMHoliday != null && dtCrystalPMHoliday.Rows.Count > 0)
        //                {
        //                    bool status = false;// SynchCrystalPMBAL.Save_Holiday_CrystalPM_To_Local(dtCrystalPMHoliday, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                    if (status)
        //                    {
        //                        SynchDataLiveDB_Push_Holiday();
        //                    }
        //                    else
        //                    {
        //                        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Holiday Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
        //                    }
        //                }
        //                else
        //                {
        //                    bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday_Push");
        //                }
        //                ObjGoalBase.WriteToSyncLogFile("Holiday Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
        //                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");
        //            }
        //            Is_synched_Holidays = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_Holidays = false;
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Create Appointment

        //private void fncSynchDataLocalToCrystalPM_Appointment()
        //{
        //    InitBgWorkerLocalToCrystalPM_Appointment();
        //    InitBgTimerLocalToCrystalPM_Appointment();
        //}

        //private void InitBgTimerLocalToCrystalPM_Appointment()
        //{
        //    timerSynchLocalToCrystalPM_Appointment = new System.Timers.Timer();
        //    this.timerSynchLocalToCrystalPM_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
        //    this.timerSynchLocalToCrystalPM_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToCrystalPM_Appointment_Tick);
        //    timerSynchLocalToCrystalPM_Appointment.Enabled = true;
        //    timerSynchLocalToCrystalPM_Appointment.Start();
        //    timerSynchLocalToCrystalPM_Appointment_Tick(null, null);
        //}

        //private void InitBgWorkerLocalToCrystalPM_Appointment()
        //{
        //    bwSynchLocalToCrystalPM_Appointment = new BackgroundWorker();
        //    bwSynchLocalToCrystalPM_Appointment.WorkerReportsProgress = true;
        //    bwSynchLocalToCrystalPM_Appointment.WorkerSupportsCancellation = true;
        //    bwSynchLocalToCrystalPM_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToCrystalPM_Appointment_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchLocalToCrystalPM_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToCrystalPM_Appointment_RunWorkerCompleted);
        //}

        //private void timerSynchLocalToCrystalPM_Appointment_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchLocalToCrystalPM_Appointment.Enabled = false;
        //        MethodForCallSynchOrderLocalToCrystalPM_Appointment();
        //    }
        //}

        //public void MethodForCallSynchOrderLocalToCrystalPM_Appointment()
        //{
        //    System.Threading.Thread procThreadmainLocalToCrystalPM_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToCrystalPM_Appointment);
        //    procThreadmainLocalToCrystalPM_Appointment.Start();
        //}

        //public void CallSyncOrderTableLocalToCrystalPM_Appointment()
        //{
        //    if (bwSynchLocalToCrystalPM_Appointment.IsBusy != true)
        //    {
        //        bwSynchLocalToCrystalPM_Appointment.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchLocalToCrystalPM_Appointment_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchLocalToCrystalPM_Appointment.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataLocalToCrystalPM_Appointment();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchLocalToCrystalPM_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchLocalToCrystalPM_Appointment.Enabled = true;
        //}

        //public void SynchDataLocalToCrystalPM_Appointment()
        //{
        //    try
        //    {
        //        //CheckEntryUserLoginIdExist();
        //        if (Utility.IsApplicationIdleTimeOff)
        //        {
        //            for (int i = 0; i < Utility.DtInstallServiceList.Rows.Count; i++)
        //            {
        //                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
        //                {
        //                    AditCrystalPM.BAL.Cls_Synch_CreatAppt.SynchDataLocalToEHR_Appointment(Utility.DtInstallServiceList.Rows[i]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[i]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString());
        //                }
        //            }
        //            //  SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook();
        //            ObjGoalBase.WriteToSyncLogFile("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //    }
        //}

        //#endregion

        //#region Sync MedicalHistory Form

        //private void fncSynchDataCrystalPM_MedicalHistory()
        //{
        //    InitBgWorkerCrystalPM_MedicalHistory();
        //    InitBgTimerCrystalPM_MedicalHistory();
        //}

        //private void InitBgTimerCrystalPM_MedicalHistory()
        //{
        //    timerSynchCrystalPM_MedicalHistory = new System.Timers.Timer();
        //    this.timerSynchCrystalPM_MedicalHistory.Interval = 1000 * GoalBase.intervalEHRSynch_MedicalHistory;
        //    this.timerSynchCrystalPM_MedicalHistory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCrystalPM_MedicalHistory_Tick);
        //    timerSynchCrystalPM_MedicalHistory.Enabled = true;
        //    timerSynchCrystalPM_MedicalHistory.Start();
        //    timerSynchCrystalPM_MedicalHistory_Tick(null, null);
        //}

        //private void InitBgWorkerCrystalPM_MedicalHistory()
        //{
        //    bwSynchCrystalPM_MedicalHistory = new BackgroundWorker();
        //    bwSynchCrystalPM_MedicalHistory.WorkerReportsProgress = true;
        //    bwSynchCrystalPM_MedicalHistory.WorkerSupportsCancellation = true;
        //    bwSynchCrystalPM_MedicalHistory.DoWork += new DoWorkEventHandler(bwSynchCrystalPM_MedicalHistory_DoWork);
        //    bwSynchCrystalPM_MedicalHistory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCrystalPM_MedicalHistory_RunWorkerCompleted);
        //}

        //private void timerSynchCrystalPM_MedicalHistory_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchCrystalPM_MedicalHistory.Enabled = false;
        //        MethodForCallSynchOrderCrystalPM_MedicalHistory();
        //    }
        //}

        //public void MethodForCallSynchOrderCrystalPM_MedicalHistory()
        //{
        //    System.Threading.Thread procThreadmainCrystalPM_MedicalHistory = new System.Threading.Thread(this.CallSyncOrderTableCrystalPM_MedicalHistory);
        //    procThreadmainCrystalPM_MedicalHistory.Start();
        //}

        //public void CallSyncOrderTableCrystalPM_MedicalHistory()
        //{
        //    if (bwSynchCrystalPM_MedicalHistory.IsBusy != true)
        //    {
        //        bwSynchCrystalPM_MedicalHistory.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchCrystalPM_MedicalHistory_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchCrystalPM_MedicalHistory.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }
        //        SynchDataCrystalPM_MedicalHistory();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation(ex.Message);
        //    }
        //}

        //private void bwSynchCrystalPM_MedicalHistory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchCrystalPM_MedicalHistory.Enabled = true;
        //}

        //public void SynchDataCrystalPM_MedicalHistory()
        //{
        //    CallSynch_CrystalPM_MedicalHistory();

        //}

        //private void CallSynch_CrystalPM_MedicalHistory()
        //{
        //    if (!Is_synched_MedicalHistory && Utility.IsApplicationIdleTimeOff)
        //    {
        //        string tablename = "";
        //        try
        //        {
        //            Is_synched_MedicalHistory = true;
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataSet dsCrystalPMMedicalHistory = null;// SynchCrystalPMBAL.GetCrystalPMMedicalHistoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                DataTable dtMedicalHistory = new DataTable();
        //                string ignoreColumnsList = "", primaryColumnList = "";

        //                foreach (DataTable dtTable in dsCrystalPMMedicalHistory.Tables)
        //                {
        //                    DataTable dtLocalDbRecords = new DataTable();

        //                    dtTable.Columns.Add("InsUptDlt", typeof(int));
        //                    dtTable.Columns["InsUptDlt"].DefaultValue = 0;

        //                    dtLocalDbRecords = SynchLocalBAL.GetLocalMedicalHistoryRecords(dtTable.TableName.ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), true);
        //                    dtMedicalHistory = dtTable;
        //                    tablename = dtMedicalHistory.TableName.ToString();
        //                    #region Compare Records

        //                    if (!dtLocalDbRecords.Columns.Contains("InsUptDlt"))
        //                    {
        //                        dtLocalDbRecords.Columns.Add("InsUptDlt", typeof(int));
        //                        dtLocalDbRecords.Columns["InsUptDlt"].DefaultValue = 0;
        //                    }

        //                    if (dtMedicalHistory.TableName.ToString() == "OD_SheetDef")
        //                    {
        //                        ignoreColumnsList = "SheetDefNum_LocalDB_ID,SheetDefNum_Web_ID";
        //                        primaryColumnList = "SheetDefNum_LocalDB_ID";
        //                        //dtLocalDbRecords = CompareDataTablzeRecords(ref dtMedicalHistory, dtLocalDbRecords, "SheetDefNum_EHR_ID", "SheetDefNum_LocalDB_ID", "SheetDefNum_LocalDB_ID,SheetDefNum_Web_ID,EHR_Entry_DateTime, SheetDefNum_EHR_ID,PageCount,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt");
        //                        dtLocalDbRecords = CompareDataTableRecordsWithTwoColumns(ref dtMedicalHistory, dtLocalDbRecords, "SheetDefNum_EHR_ID", "Clinic_Number", "SheetDefNum_LocalDB_ID", "SheetDefNum_LocalDB_ID,SheetDefNum_Web_ID,EHR_Entry_DateTime, SheetDefNum_EHR_ID,PageCount,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Service_Install_Id");
        //                    }
        //                    else if (dtMedicalHistory.TableName.ToString() == "OD_SheetFieldDef")
        //                    {
        //                        ignoreColumnsList = "SheetFieldDefNum_LocalDB_ID,SheetFieldDefNum_Web_ID";
        //                        primaryColumnList = "SheetFieldDefNum_LocalDB_ID";
        //                        //dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "SheetFieldDefNum_EHR_ID", "SheetFieldDefNum_LocalDB_ID", "SheetFieldDefNum_LocalDB_ID,SheetFieldDefNum_Web_ID,EHR_Entry_DateTime,FontIsBold,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt");
        //                        dtLocalDbRecords = CompareDataTableRecordsWithTwoColumns(ref dtMedicalHistory, dtLocalDbRecords, "SheetFieldDefNum_EHR_ID", "Clinic_Number", "SheetFieldDefNum_LocalDB_ID", "SheetFieldDefNum_LocalDB_ID,SheetFieldDefNum_Web_ID,EHR_Entry_DateTime,FontIsBold,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Service_Install_Id");
        //                    }

        //                    dtMedicalHistory.AcceptChanges();

        //                    if (dtMedicalHistory != null && dtMedicalHistory.Rows.Count > 0 && dtMedicalHistory.AsEnumerable()
        //                            .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
        //                    {
        //                        bool status = false;
        //                        DataTable dtSaveRecords = dtMedicalHistory.Clone();

        //                        if (dtMedicalHistory.Select("InsUptDlt IN (1,2)").Count() > 0 || dtLocalDbRecords.Select("InsUptDlt IN (3)").Count() > 0)
        //                        {
        //                            if (dtMedicalHistory.Select("InsUptDlt IN (1,2)").Count() > 0)
        //                            {
        //                                dtSaveRecords.Load(dtMedicalHistory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
        //                            }
        //                            if (dtLocalDbRecords.Select("InsUptDlt IN (3)").Count() > 0)
        //                            {
        //                                dtSaveRecords.Load(dtLocalDbRecords.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
        //                            }
        //                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, dtMedicalHistory.TableName, ignoreColumnsList, primaryColumnList);
        //                        }
        //                        else
        //                        {
        //                            if (dtMedicalHistory.Select("InsUptDlt IN (4)").Count() > 0)
        //                            {
        //                                status = true;
        //                            }
        //                        }
        //                        if (status)
        //                        {
        //                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime(tablename);
        //                            ObjGoalBase.WriteToSyncLogFile("" + tablename + " Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                            IsTrackerOperatorySync = true;
        //                            SynchDataLiveDB_Push_MedicalHisotryTables(dtMedicalHistory.TableName.ToString());
        //                        }
        //                        else
        //                        {
        //                            IsTrackerOperatorySync = false;
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                SynchDataLiveDB_Push_MedicalHisotryTables("OD_SheetDef");
        //                SynchDataLiveDB_Push_MedicalHisotryTables("OD_SheetFieldDef");

        //            }
        //            Is_synched_MedicalHistory = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            Is_synched_MedicalHistory = false;
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[" + tablename + " Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //        }
        //    }
        //}

        //#endregion

        //#region EventListener
        //public static bool SynchDataCrystalPM_AppointmentFromEvent(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id, bool isTrueDebugLog = false)
        //{
        //    string strDbConnString = "";
        //    string Location_ID = "";
        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        //        var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_Id + "'");
        //        strDbConnString = dbConStr[0]["DBConnString"].ToString();

        //        var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
        //        Location_ID = LocID[0]["Location_ID"].ToString();

        //        Utility.WritetoAditEventDebugLogFile_Static("1 SynchDataCrystalPM_AppointmentFromEvent Start.");
        //        //CheckEntryUserLoginIdExist();
        //        Utility.WritetoAditEventDebugLogFile_Static("2");
        //        if (Utility.IsApplicationIdleTimeOff)
        //        {
        //            Utility.WritetoAditEventDebugLogFile_Static("3");
        //            //for (int l = 0; l < Utility.DtInstallServiceList.Rows.Count; j++)
        //            //{
        //            Utility.WritetoAditEventDebugLogFile_Static("4");
        //            //DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //            DataTable dtCrystalPMPatient = null;// SynchCrystalPMBAL.GetCrystalPMPatientID_NameData(strDbConnString);
        //            Utility.WritetoAditEventDebugLogFile_Static("5");
        //            DataTable dtIdelProv = null;// SynchCrystalPMBAL.GetCrystalPMIdelProvider(strDbConnString);
        //            Utility.WritetoAditEventDebugLogFile_Static("6");

        //            string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
        //            Utility.WritetoAditEventDebugLogFile_Static("7");
        //            string tmpApptProv = "";
        //            Utility.WritetoAditEventDebugLogFile_Static("8");
        //            Int64 tmpPatient_id = 0;
        //            Utility.WritetoAditEventDebugLogFile_Static("9");
        //            Int64 tmpPatient_Gur_id = 0;
        //            Utility.WritetoAditEventDebugLogFile_Static("10");

        //            Int64 tmpAppt_EHR_id = 0;
        //            Utility.WritetoAditEventDebugLogFile_Static("11");
        //            Int64 tmpNewPatient = 1;
        //            Utility.WritetoAditEventDebugLogFile_Static("12");
        //            string tmpLastName = "";
        //            Utility.WritetoAditEventDebugLogFile_Static("13");
        //            string tmpFirstName = "";
        //            Utility.WritetoAditEventDebugLogFile_Static("14");
        //            string TmpWebPatientName = "";
        //            Utility.WritetoAditEventDebugLogFile_Static("15");
        //            string TmpWebRevPatientName = "";
        //            Utility.WritetoAditEventDebugLogFile_Static("16");
        //            if (dtWebAppointment != null)
        //            {
        //                if (dtWebAppointment.Rows.Count > 0)
        //                {
        //                    Utility.CheckEntryUserLoginIdExist();
        //                }
        //            }
        //            foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
        //            {
        //                Utility.WritetoAditEventDebugLogFile_Static("17");
        //                string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
        //                Utility.WritetoAditEventDebugLogFile_Static("18");
        //                DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
        //                Utility.WritetoAditEventDebugLogFile_Static("19");
        //                DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
        //                Utility.WritetoAditEventDebugLogFile_Static("20");
        //                TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
        //                Utility.WritetoAditEventDebugLogFile_Static("21");
        //                int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);
        //                Utility.WritetoAditEventDebugLogFile_Static("22");
        //                string tmpApptDurPatern = "";
        //                Utility.WritetoAditEventDebugLogFile_Static("23");
        //                for (int i = 0; i < tmpApptDurMinutes / 5; i++)
        //                {
        //                    tmpApptDurPatern = tmpApptDurPatern + "X";
        //                }
        //                Utility.WritetoAditEventDebugLogFile_Static("24");
        //                DataTable dtBookOperatoryApptWiseDateTime = null;// SynchCrystalPMBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), dtDtxRow["Clinic_Number"].ToString(), strDbConnString);
        //                Utility.WritetoAditEventDebugLogFile_Static("25");
        //                int tmpIdealOperatory = 0;
        //                Utility.WritetoAditEventDebugLogFile_Static("26");
        //                string appointment_EHR_id = "";
        //                Utility.WritetoAditEventDebugLogFile_Static("27");

        //                if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
        //                {
        //                    Utility.WritetoAditEventDebugLogFile_Static("28");
        //                    int tmpCheckOpId = 0;
        //                    Utility.WritetoAditEventDebugLogFile_Static("29");
        //                    bool IsConflict = false;
        //                    Utility.WritetoAditEventDebugLogFile_Static("30");
        //                    for (int i = 0; i < Operatory_EHR_IDs.Length; i++)
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("30.1");
        //                        IsConflict = false;
        //                        Utility.WritetoAditEventDebugLogFile_Static("30.2");
        //                        tmpCheckOpId = Convert.ToInt32(Operatory_EHR_IDs[i].ToString());
        //                        Utility.WritetoAditEventDebugLogFile_Static("30.3");
        //                        DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Select("OP = '" + tmpCheckOpId + "'");
        //                        Utility.WritetoAditEventDebugLogFile_Static("30.4");
        //                        if (rowBookOpTime.Length > 0)
        //                        {
        //                            Utility.WritetoAditEventDebugLogFile_Static("30.5");
        //                            for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
        //                            {
        //                                Utility.WritetoAditEventDebugLogFile_Static("30.5.1");
        //                                appointment_EHR_id = rowBookOpTime[Bop]["AptNum"].ToString();
        //                                Utility.WritetoAditEventDebugLogFile_Static("30.5.2");
        //                                if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
        //                                    && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
        //                                {
        //                                    Utility.WritetoAditEventDebugLogFile_Static("30.5.3");
        //                                    IsConflict = true;
        //                                    break;
        //                                }
        //                                else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
        //                                    && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
        //                                {
        //                                    Utility.WritetoAditEventDebugLogFile_Static("30.5.4");
        //                                    IsConflict = true;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        if (IsConflict == false)
        //                        {
        //                            Utility.WritetoAditEventDebugLogFile_Static("30.5.5");
        //                            tmpIdealOperatory = tmpCheckOpId;
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Utility.WritetoAditEventDebugLogFile_Static("27");
        //                    tmpIdealOperatory = Convert.ToInt32(Operatory_EHR_IDs[0].ToString());
        //                }

        //                if (tmpIdealOperatory == 0)
        //                {
        //                    Utility.WritetoAditEventDebugLogFile_Static("28, appointment_EHR_id:" + appointment_EHR_id);
        //                    DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("AptNum = " + appointment_EHR_id).CopyToDataTable();
        //                    Utility.WritetoAditEventDebugLogFile_Static("29");
        //                    bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id, dtTemp, appointment_EHR_id, Location_ID);
        //                    Utility.WritetoAditEventDebugLogFile_Static("30");
        //                }
        //                else
        //                {
        //                    tmpPatient_id = 0;
        //                    tmpPatient_Gur_id = 0;
        //                    tmpAppt_EHR_id = 0;
        //                    tmpNewPatient = 1;
        //                    TmpWebPatientName = "";

        //                    Utility.WritetoAditEventDebugLogFile_Static("31");
        //                    //TmpWebPatientName = dtDtxRow["First_Name"].ToString().Trim();
        //                    //TmpWebRevPatientName = dtDtxRow["Last_Name"].ToString().Trim();
        //                    Utility.WritetoAditEventDebugLogFile_Static("32");
        //                    if (dtDtxRow["Patient_Status"].ToString().ToLower().Trim() == "new")
        //                    {
        //                        tmpNewPatient = 1;
        //                    }
        //                    else
        //                    {
        //                        tmpNewPatient = 0;
        //                    }
        //                    Utility.WritetoAditEventDebugLogFile_Static("33");
        //                    Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);
        //                    Utility.WritetoAditEventDebugLogFile_Static("34");
        //                    tmpApptProv = dtDtxRow["Provider_EHR_ID"].ToString().Trim();
        //                    Utility.WritetoAditEventDebugLogFile_Static("35");
        //                    if (tmpApptProv == "" || tmpApptProv == "0" || tmpApptProv == "-")
        //                    {
        //                        tmpApptProv = tmpIdelProv;
        //                    }
        //                    Utility.WritetoAditEventDebugLogFile_Static("36");
        //                    tmpPatient_id = 0;
        //                    if (dtDtxRow["Patient_EHR_Id"] != null && dtDtxRow["Patient_EHR_Id"].ToString() != string.Empty && dtDtxRow["Patient_EHR_Id"].ToString() != "-")
        //                    {
        //                        tmpPatient_id = Convert.ToInt64(dtDtxRow["Patient_EHR_Id"].ToString());
        //                    }
        //                    Utility.WritetoAditEventDebugLogFile_Static("37");
        //                    if (tmpPatient_id == 0)
        //                    {

        //                        tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtCrystalPMPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), strDbConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
        //                    }
        //                    Utility.WritetoAditEventDebugLogFile_Static("38 tmpPatient_id:" + tmpPatient_id);
        //                    if (tmpPatient_id > 0)
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("39 Save_Appointment_Local_To_CrystalPM start.");
        //                        tmpAppt_EHR_id = 0;
        //                        //SynchCrystalPMBAL.Save_Appointment_Local_To_CrystalPM(tmpPatient_id.ToString(),
        //                        //                                                                (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "1"),
        //                        //                                                                tmpApptDurPatern,
        //                        //                                                                     (dtDtxRow["confirmed_status_ehr_key"].ToString() == string.Empty ? "19" : dtDtxRow["confirmed_status_ehr_key"].ToString()),
        //                        //                                                                tmpIdealOperatory.ToString(),
        //                        //                                                                tmpApptProv,
        //                        //                                                                dtDtxRow["Appt_DateTime"].ToString().Trim(),
        //                        //                                                                tmpNewPatient.ToString(),
        //                        //                                                                dtDtxRow["Appt_DateTime"].ToString().Trim(),
        //                        //                                                                dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
        //                        //                                                                dtDtxRow["comment"].ToString().Trim(),
        //                        //                                                                dtDtxRow["Clinic_Number"].ToString().Trim(),
        //                        //                                                                (dtDtxRow["appt_treatmentcode"].ToString()),
        //                        //                                                                strDbConnString);
        //                        Utility.WritetoAditEventDebugLogFile_Static("40 AptID:" + tmpAppt_EHR_id);

        //                        if (tmpAppt_EHR_id > 0)
        //                        {
        //                            bool isApptId_Update = false;// SynchCrystalPMBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id);
        //                        }
        //                        SyncDataCrystalPM_AppointmentFromEvent(strDbConnString, Clinic_Number, Service_Install_Id, tmpAppt_EHR_id.ToString(), tmpPatient_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
        //                        Utility.WritetoAditEventDebugLogFile_Static("40 SynchDataCrystalPM_AppointmentFromEvent End.");
        //                    }
        //                }
        //            }
        //            //}
        //            //SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook();                  
        //            Utility.WritetoAditEventSyncLogFile_Static("SynchDataCrystalPM_AppointmentFromEvent Sync (Local Database To CrystalPM) Successfully.");
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.WritetoAditEventErrorLogFile_Static("[SynchDataCrystalPM_AppointmentFromEvent Sync (Local Database To CrystalPM) ]" + ex.Message);
        //        return false;
        //    }
        //}

        //public static void SyncDataCrystalPM_AppointmentFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        //{
        //    try
        //    {
        //        Utility.WritetoAditEventDebugLogFile_Static("1 SyncDataCrystalPM_AppointmentFromEvent Start.");
        //        try
        //        {
        //            Utility.WritetoAditEventDebugLogFile_Static("2 Appointment Patient Start.");
        //            #region "Appointment Patient"
        //            DataTable dtLocalCrystalPMLanguageList = null;// SynchLocalBAL.GetLocalCrystalPMLanguageList();
        //            Utility.WritetoAditEventDebugLogFile_Static("2.1 GetLocalPatientDataByPatID start.");
        //            DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Service_Install_Id, strPatID);
        //            Utility.WritetoAditEventDebugLogFile_Static("2.2 GetLocalPatientDataByPatID End and GetCrystalPMPatientDataByPatID Start.");
        //            DataTable dtCrystalPMAppointmensPatient = null;// SynchCrystalPMBAL.GetCrystalPMPatientData(Clinic_Number, strDbString, true, strPatID);
        //            Utility.WritetoAditEventDebugLogFile_Static("2.3 GetCrystalPMPatientDataByPatID End.");
        //            var updateLanguageQuery = from r1 in dtCrystalPMAppointmensPatient.AsEnumerable()
        //                                      join r2 in dtLocalCrystalPMLanguageList.AsEnumerable()
        //                                      on r1.Field<string>("PreferredLanguage") equals r2.Field<string>("Language_Short_Name")
        //                                      select new { r1, r2 };
        //            foreach (var x in updateLanguageQuery)
        //            {
        //                x.r1.SetField("PreferredLanguage", x.r2.Field<string>("Language_Name"));
        //            }

        //            DataTable dtSaveRecords = new DataTable();
        //            dtSaveRecords = dtLocalPatient.Clone();

        //            var itemsToBeAdded = (from CrystalPMPatient in dtCrystalPMAppointmensPatient.AsEnumerable()
        //                                  join LocalPatient in dtLocalPatient.AsEnumerable()
        //                                  on CrystalPMPatient["Patient_EHR_ID"].ToString().Trim() + "_" + CrystalPMPatient["Clinic_Number"].ToString().Trim()
        //                                  equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
        //                                  //on new { PatID = CrystalPMPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = CrystalPMPatient["Clinic_Number"].ToString().Trim() }
        //                                  //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
        //                                  into matchingRows
        //                                  from matchingRow in matchingRows.DefaultIfEmpty()
        //                                  where matchingRow == null
        //                                  select CrystalPMPatient).ToList();
        //            DataTable dtPatientToBeAdded = dtLocalPatient.Clone();
        //            if (itemsToBeAdded.Count > 0)
        //            {
        //                dtPatientToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
        //            }

        //            if (!dtPatientToBeAdded.Columns.Contains("InsUptDlt"))
        //            {
        //                dtPatientToBeAdded.Columns.Add("InsUptDlt", typeof(int));
        //                dtPatientToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
        //            }
        //            if (dtPatientToBeAdded.Rows.Count > 0)
        //            {
        //                dtPatientToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
        //                dtSaveRecords.Load(dtPatientToBeAdded.Select().CopyToDataTable().CreateDataReader());
        //                //ObjGoalBase.WriteToSyncLogFile_All("Table to Save Record count after insert :" + dtSaveRecords.Rows.Count);
        //            }

        //            Utility.WritetoAditEventDebugLogFile_Static("2.4 Comparison start.");
        //            var itemsToBeUpdated = (from CrystalPMPatient in dtCrystalPMAppointmensPatient.AsEnumerable()
        //                                    join LocalPatient in dtLocalPatient.AsEnumerable()
        //                                    on CrystalPMPatient["Patient_EHR_ID"].ToString().Trim() + "_" + CrystalPMPatient["Clinic_Number"].ToString().Trim()
        //                                    equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
        //                                    where
        //                                     Convert.ToDateTime(CrystalPMPatient["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(LocalPatient["EHR_Entry_DateTime"])
        //                                     ||
        //                                     (CrystalPMPatient["nextvisit_date"] != DBNull.Value && CrystalPMPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(CrystalPMPatient["nextvisit_date"]) : DateTime.Now)
        //                                     !=
        //                                     (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)
        //                                     ||
        //                                     (CrystalPMPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())
        //                                     ||
        //                                     (CrystalPMPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())
        //                                     || (CrystalPMPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
        //                                     || (CrystalPMPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
        //                                     || (CrystalPMPatient["Home_Phone"].ToString().Trim()) != (LocalPatient["Home_Phone"].ToString().Trim())
        //                                     || (CrystalPMPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
        //                                     || (CrystalPMPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
        //                                     || (CrystalPMPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
        //                                     || (CrystalPMPatient["Mobile"].ToString().Trim()) != (LocalPatient["Mobile"].ToString().Trim())
        //                                     || (CrystalPMPatient["ReceiveSMS"].ToString().Trim()) != (LocalPatient["ReceiveSMS"].ToString().Trim())
        //                                     || (CrystalPMPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
        //                                    select CrystalPMPatient).ToList();

        //            DataTable dtPatientToBeUpdated = dtLocalPatient.Clone();
        //            if (itemsToBeUpdated.Count > 0)
        //            {
        //                dtPatientToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
        //            }
        //            if (!dtPatientToBeUpdated.Columns.Contains("InsUptDlt"))
        //            {
        //                dtPatientToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
        //                dtPatientToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
        //            }

        //            if (dtPatientToBeUpdated.Rows.Count > 0)
        //            {
        //                dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
        //                dtSaveRecords.Load(dtPatientToBeUpdated.Select().CopyToDataTable().CreateDataReader());
        //            }
        //            Utility.WritetoAditEventDebugLogFile_Static("2.6 Comparison End, records to be udpate:" + dtSaveRecords.Rows.Count);
        //            if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
        //            {
        //                bool status = false;// SynchCrystalPMBAL.Save_Patient_CrystalPM_To_Local_New(dtSaveRecords, Clinic_Number, Service_Install_Id);
        //                Utility.WritetoAditEventDebugLogFile_Static("2.6.1 Patient Push Start.");
        //                SynchDataLiveDB_Push_Patient(strPatID);
        //                Utility.WritetoAditEventDebugLogFile_Static("2.6.2 Patient Push End.");
        //            }
        //            Utility.WritetoAditEventDebugLogFile_Static("2.7 Appointment Patient Sync Successfully.");

        //            #endregion
        //            Utility.WritetoAditEventDebugLogFile_Static("3 Appointment Patient End and PatientStatus Start.");
        //            #region "Patient Status"
        //            DataTable dtCrystalPMPatientStatus = new DataTable();
        //            Utility.WritetoAditEventDebugLogFile_Static("3.1 GetCrystalPMPatientStatusDataByPatID Start.");
        //            dtCrystalPMPatientStatus = null;// SynchCrystalPMBAL.GetCrystalPMPatientStatusData(Clinic_Number, strDbString, strPatID);
        //            Utility.WritetoAditEventDebugLogFile_Static("3.2 GetCrystalPMPatientStatusDataByPatID End. Records:" + dtCrystalPMPatientStatus.Rows.Count);
        //            if (dtCrystalPMPatientStatus != null && dtCrystalPMPatientStatus.Rows.Count > 0)
        //            {
        //                Utility.WritetoAditEventDebugLogFile_Static("3.3 UpdatePatient_StatusByPatID Start.");
        //                SynchLocalBAL.UpdatePatient_Status(dtCrystalPMPatientStatus, Service_Install_Id, Clinic_Number, strPatID);
        //                Utility.WritetoAditEventDebugLogFile_Static("3.4 UpdatePatient_StatusByPatID End.");
        //            }
        //            #endregion
        //            Utility.WritetoAditEventDebugLogFile_Static("4 PatientStatus End.");
        //        }
        //        catch (Exception exPat)
        //        {
        //            Utility.WritetoAditEventErrorLogFile_Static("[SyncDataCrystalPM_AppointmentFromEvent Error in Appointment Patient Sync]:" + exPat.Message);
        //        }

        //        Utility.WritetoAditEventDebugLogFile_Static("5 GetCrystalPMAppointmentData Start.");
        //        DataTable dtCrystalPMAppointment = null;// SynchCrystalPMBAL.GetCrystalPMAppointmentData(strDbString, strApptID);
        //        dtCrystalPMAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
        //        dtCrystalPMAppointment.Columns.Add("InsUptDlt", typeof(int));
        //        Utility.WritetoAditEventDebugLogFile_Static("6 GetCrystalPMAppointmentData End And GetLocalAppointmentData Start.");
        //        DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Service_Install_Id, strApptID);
        //        Utility.WritetoAditEventDebugLogFile_Static("7 GetLocalAppointmentData End Comparison Start.");

        //        foreach (DataRow dtDtxRow in dtCrystalPMAppointment.Rows)
        //        {
        //            try
        //            {
        //                Utility.WritetoAditEventDebugLogFile_Static("8 Appt_EHR_ID: " + dtDtxRow["Appt_EHR_ID"].ToString().Trim());
        //                DataRow[] row = dtLocalAppointment.Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
        //                if (row.Length > 0)
        //                {
        //                    if (Convert.ToDateTime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EHR_Entry_DateTime"]))
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.1 Change in EHR_Entry_DateTime");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["Last_Name"].ToString().ToLower().Trim() != row[0]["Last_Name"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.2 Change in Last_Name");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.2 Change in Last_Name");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["MI"].ToString().ToLower().Trim() != row[0]["MI"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.2 Change in Last_Name");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (Utility.ConvertContactNumber(dtDtxRow["Home_Contact"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").ToLower().Trim()) != Utility.ConvertContactNumber(row[0]["Home_Contact"].ToString().ToLower().Trim()))
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.3 Change in Home_Contact");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().ToLower().Trim()) != Utility.ConvertContactNumber(row[0]["Mobile_Contact"].ToString().ToLower().Trim()))
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.4 Change in Mobile_Contact");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["Email"].ToString().ToLower().Trim() != row[0]["Email"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.5 Change in Email");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["Address"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.6 Change in Address");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["City"].ToString().ToLower().Trim() != row[0]["City"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.7 Change in City");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["ST"].ToString().ToLower().Trim() != row[0]["ST"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.8 Change in ST");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["is_asap"] != null && dtDtxRow["is_asap"].ToString() != string.Empty && Convert.ToBoolean(dtDtxRow["is_asap"]) != Convert.ToBoolean(row[0]["is_asap"]))
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.9 Change in is_asap");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["Zip"].ToString().ToLower().Trim() != row[0]["Zip"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.10 Change in Zip");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["ProcedureDesc"].ToString().ToLower().Trim() != row[0]["ProcedureDesc"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.11 Change in ProcedureDesc");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["ProcedureCode"].ToString().ToLower().Trim() != row[0]["ProcedureCode"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.12 Change in ProcedureCode");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else if (dtDtxRow["Clinic_Number"].ToString().ToLower().Trim() != row[0]["Clinic_Number"].ToString().ToLower().Trim())
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.13 Change in Clinic_Number");
        //                        dtDtxRow["InsUptDlt"] = 4;
        //                    }
        //                    else
        //                    {
        //                        Utility.WritetoAditEventDebugLogFile_Static("8.14 No Change");
        //                        dtDtxRow["InsUptDlt"] = 0;
        //                    }
        //                }
        //                else
        //                {
        //                    dtDtxRow["InsUptDlt"] = 1;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Utility.WritetoAditEventErrorLogFile_Static("[SyncDataCrystalPM_AppointmentFromEvent (" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + ") Sync (CrystalPM to Local Database) ]" + ex.Message);
        //            }
        //            Utility.WritetoAditEventDebugLogFile_Static("9 Comparison End");

        //            dtCrystalPMAppointment.AcceptChanges();
        //            if (dtCrystalPMAppointment != null && dtCrystalPMAppointment.Rows.Count > 0 && dtCrystalPMAppointment.Select("InsUptDlt IN (1,5,4,7,3,6)").Count() > 0)
        //            {
        //                DataTable dtResult = dtCrystalPMAppointment.Select("InsUptDlt IN (1,5,4,7,3,6)").CopyToDataTable();
        //                Utility.WritetoAditEventDebugLogFile_Static("10 Appointment sent for Save and update : " + System.DateTime.Now.ToString() + " with Count " + dtResult.Rows.Count.ToString());

        //                if (!dtResult.Columns.Contains("Appt_Web_ID"))
        //                {
        //                    dtResult.Columns.Add("Appt_Web_ID");
        //                }
        //                dtResult.Rows[0]["Appt_Web_ID"] = strWebID;


        //                bool status = false;// SynchCrystalPMBAL.Save_Appointment_CrystalPM_To_Local(dtResult, Service_Install_Id);
        //                Utility.WritetoAditEventDebugLogFile_Static("11 Appointment updated : " + System.DateTime.Now.ToString());
        //            }

        //            Utility.WritetoAditEventDebugLogFile_Static("12 Appointment Push Start.");
        //            bool temp = Utility.IsExternalAppointmentSync;
        //            Utility.IsExternalAppointmentSync = true;
        //            SynchDataLiveDB_Push_Appointment(strApptID);
        //            Utility.IsExternalAppointmentSync = temp;
        //            Utility.WritetoAditEventDebugLogFile_Static("13 Appointment Push End, SyncDataCrystalPM_AppointmentFromEvent End.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.WritetoAditEventErrorLogFile_Static("[SyncDataCrystalPM_AppointmentFromEvent Sync (CrystalPM to Local Database) ]" + ex.Message);
        //    }
        //}

        //public void SynchDataLocalToCrystalPM_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
        //{
        //    string strDbConnString = "";
        //    string Location_ID = "";
        //    string strDocumentPath = "";
        //    try
        //    {
        //        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

        //        var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_Id + "'");
        //        strDbConnString = dbConStr[0]["DBConnString"].ToString();

        //        var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
        //        Location_ID = LocID[0]["Location_ID"].ToString();
        //        strDocumentPath = dbConStr[0]["Document_Path"].ToString();

        //        //CheckEntryUserLoginIdExist();

        //        DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData(Service_Install_Id, strPatientFormID);
        //        dtWebPatient_Form.Columns.Add(new DataColumn("Table_Name", typeof(string)));

        //        if (dtWebPatient_Form != null)
        //        {
        //            if (dtWebPatient_Form.Rows.Count > 0)
        //            {
        //                Utility.CheckEntryUserLoginIdExist();
        //            }
        //        }

        //        foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
        //        {
        //            dtDtxRow["Table_Name"] = "patient";

        //            if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "FIRST_NAME")
        //            {
        //                dtDtxRow["ehrfield"] = "FName";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "LAST_NAME")
        //            {
        //                dtDtxRow["ehrfield"] = "LName";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MOBILE")
        //            {
        //                dtDtxRow["ehrfield"] = "WirelessPhone";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ADDRESS_ONE")
        //            {
        //                dtDtxRow["ehrfield"] = "Address";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ADDRESS_TWO")
        //            {
        //                dtDtxRow["ehrfield"] = "Address2";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "BIRTH_DATE")
        //            {
        //                dtDtxRow["ehrfield"] = "Birthdate";
        //                try
        //                {
        //                    dtDtxRow["ehrfield_value"] = Convert.ToDateTime(dtDtxRow["ehrfield_value"].ToString().Trim()).ToString("yyyy-MM-dd");

        //                }
        //                catch (Exception)
        //                {
        //                    dtDtxRow["ehrfield_value"] = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
        //                }
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "CITY")
        //            {
        //                dtDtxRow["ehrfield"] = "City";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMAIL")
        //            {
        //                dtDtxRow["ehrfield"] = "Email";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "HOME_PHONE")
        //            {
        //                dtDtxRow["ehrfield"] = "HmPhone";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MARITAL_STATUS")
        //            {
        //                dtDtxRow["ehrfield"] = "Position";
        //                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "SINGLE")
        //                {
        //                    dtDtxRow["ehrfield_value"] = 0;
        //                }
        //                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MARRIED")
        //                {
        //                    dtDtxRow["ehrfield_value"] = 1;
        //                }
        //                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "CHILD")
        //                {
        //                    dtDtxRow["ehrfield_value"] = 2;
        //                }
        //                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "WIDOWED")
        //                {
        //                    dtDtxRow["ehrfield_value"] = 3;
        //                }
        //                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "DIVORCED")
        //                {
        //                    dtDtxRow["ehrfield_value"] = 4;
        //                }
        //                else
        //                {
        //                    dtDtxRow["ehrfield_value"] = 0;
        //                }
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MIDDLE_NAME")
        //            {
        //                dtDtxRow["ehrfield"] = "MiddleI";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PREFERRED_NAME")
        //            {
        //                dtDtxRow["ehrfield"] = "preferred";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
        //            {
        //                dtDtxRow["ehrfield"] = "PriProv";
        //                try
        //                {
        //                    dtDtxRow["ehrfield_value"] = Convert.ToInt64(dtDtxRow["ehrfield_value"].ToString().Trim());
        //                }
        //                catch (Exception)
        //                {
        //                    DataTable dtIdelProv = null;// SynchCrystalPMBAL.GetCrystalPMIdelProvider(strDbConnString);
        //                    string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
        //                    ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("Default Provider for PF is = " + tmpIdelProv);
        //                    dtDtxRow["ehrfield_value"] = tmpIdelProv.Trim() == "" ? "1" : tmpIdelProv;
        //                }
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE")
        //            {
        //                dtDtxRow["ehrfield"] = "PRIMARY_INSURANCE";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
        //            {
        //                dtDtxRow["ehrfield"] = "PRIMARY_INSURANCE_COMPANYNAME";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
        //            {
        //                dtDtxRow["ehrfield"] = "PRIMARY_INS_SUBSCRIBER_ID";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "RECEIVE_EMAIL")
        //            {
        //                dtDtxRow["ehrfield"] = "TxtMsgOk";
        //                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO")
        //                {
        //                    dtDtxRow["ehrfield_value"] = "2";
        //                }
        //                else
        //                {
        //                    dtDtxRow["ehrfield_value"] = "1";
        //                }
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "RECEIVE_SMS")
        //            {
        //                dtDtxRow["ehrfield"] = "TxtMsgOk";
        //                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO")
        //                {
        //                    dtDtxRow["ehrfield_value"] = "2";
        //                }
        //                else
        //                {
        //                    dtDtxRow["ehrfield_value"] = "1";
        //                }
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SALUTATION")
        //            {
        //                dtDtxRow["ehrfield"] = "Salutation";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
        //            {
        //                dtDtxRow["ehrfield"] = "SecProv";
        //                try
        //                {
        //                    dtDtxRow["ehrfield_value"] = Convert.ToInt64(dtDtxRow["ehrfield_value"].ToString().Trim());
        //                }
        //                catch (Exception)
        //                {
        //                    dtDtxRow["ehrfield_value"] = 1;
        //                }
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE")
        //            {
        //                dtDtxRow["ehrfield"] = "SECONDARY_INSURANCE";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
        //            {
        //                dtDtxRow["ehrfield"] = "SECONDARY_INSURANCE_COMPANYNAME";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
        //            {
        //                dtDtxRow["ehrfield"] = "SECONDARY_INS_SUBSCRIBER_ID";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SEX")
        //            {
        //                dtDtxRow["ehrfield"] = "Gender";
        //                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
        //                {
        //                    dtDtxRow["ehrfield_value"] = 1;
        //                }
        //                else
        //                {
        //                    dtDtxRow["ehrfield_value"] = 0;
        //                }
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "WORK_PHONE")
        //            {
        //                dtDtxRow["ehrfield"] = "WkPhone";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "STATE")
        //            {
        //                dtDtxRow["ehrfield"] = "State";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ZIPCODE")
        //            {
        //                dtDtxRow["ehrfield"] = "Zip";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SSN")
        //            {
        //                dtDtxRow["ehrfield"] = "SSN";
        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SCHOOL")
        //            {
        //                dtDtxRow["ehrfield"] = "SchoolName";
        //            }

        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME")
        //            {
        //                dtDtxRow["ehrfield"] = "ICEName";
        //                dtDtxRow["Table_Name"] = "patientnote";


        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER")
        //            {
        //                dtDtxRow["ehrfield"] = "ICEPhone";
        //                dtDtxRow["Table_Name"] = "patientnote";

        //            }
        //            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMPLOYER")
        //            {
        //                dtDtxRow["ehrfield"] = "EmpName";
        //                dtDtxRow["Table_Name"] = "employer";

        //            }

        //            dtWebPatient_Form.AcceptChanges();
        //        }
        //        if (dtWebPatient_Form.Rows.Count > 0)
        //        {
        //            bool Is_Record_Update = false;// SynchCrystalPMBAL.Save_Patient_Form_Local_To_CrystalPM(dtWebPatient_Form, strDbConnString, Service_Install_Id);
        //        }

        //        try
        //        {
        //            // GetMedicalCrystalPMHistoryRecords(Service_Install_Id, strPatientFormID);
        //            // SynchCrystalPMBAL.SaveMedicalHistoryLocalToCrystalPM(strDbConnString, Service_Install_Id, strPatientFormID);
        //            ObjGoalBase.WriteToSyncLogFile("Medical_History_Save Sync ( Service Install Id : " + Service_Install_Id + ".  Local Database To " + Utility.Application_Name + ") Successfully.");
        //        }
        //        catch (Exception ex2)
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Medical_History_Save Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex2.Message);
        //        }

        //        try
        //        {

        //        }
        //        catch (Exception ex1)
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Alert Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
        //        }
        //        try
        //        {
        //        }
        //        catch (Exception ex1)
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Delete_Patient_Alert Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
        //        }

        //        bool isRecordSaved = false, isRecordDeleted = false;
        //        string Patient_EHR_IDS = "";
        //        string DeletePatientEHRID = "";
        //        string SavePatientEHRID = "";
        //        try
        //        {
        //        }
        //        catch (Exception ex1)
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Delete_Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
        //        }
        //        try
        //        {

        //        }
        //        catch (Exception ex1)
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Save Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
        //        }
        //        if (isRecordDeleted || isRecordSaved)
        //        {
        //            Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
        //            if (Patient_EHR_IDS != "")
        //            {
        //                SynchDataCrystalPM_PatientMedication(Patient_EHR_IDS);
        //            }
        //        }

        //        #region PatientInformation Document
        //        try
        //        {
        //            string strPatientID = "";
        //            strPatientID = SynchLocalBAL.Get_Patient_EHR_ID_from_Patient_Form(strPatientFormID);
        //            // GetTreatmentDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //            GetPatientDocument(Service_Install_Id, strPatientFormID);
        //            GetPatientDocument_New(Service_Install_Id, strPatientFormID);
        //            //SynchCrystalPMBAL.Save_Document_in_CrystalPM(strDbConnString, Service_Install_Id, strDocumentPath, strPatientFormID, strPatientID);
        //        }
        //        catch (Exception ex)
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form Document Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //        }
        //        #endregion


        //        string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing(Service_Install_Id, strPatientFormID);
        //        if (Call_Importing.ToLower() != "success")
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form API error with Importing status. Service Install Id : " + Service_Install_Id + " : " + Call_Importing);
        //        }

        //        string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed(Service_Install_Id, strPatientFormID);
        //        if (Call_Completed.ToLower() != "success")
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form API error with Completed status.Service Install Id : " + Service_Install_Id + " : " + Call_Completed);
        //        }

        //        string Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed(Service_Install_Id, Location_ID, strPatientFormID);
        //        try
        //        {
        //            if (Call_PatientPortalCompleted != "success")
        //            {
        //                ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
        //            }
        //            else
        //            {
        //                ObjGoalBase.WriteToSyncLogFile("[Patient_Portal API called with Completed status : " + Call_PatientPortalCompleted);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
        //        }

        //        ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") Successfully.");


        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFromAllWithoutValidation("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
        //    }
        //}
        //#endregion

        #endregion

        public void CrystalPMSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool IsSyncing = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
            {
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("IsCrystalPMSyncing").ToString());
                }
            }
            if (!IsCrystalPMSyncing && !IsSyncing)
            {
                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                {
                    key1.SetValue("IsCrystalPMSyncing", true);
                }

                CrystalPMSync.Enabled = false;
                IsCrystalPMSyncing = true;
                try
                {
                    using (Process myProcess = new Process())
                    {
                        try
                        {
                            ObjGoalBase.WriteToSyncLogFile("Appointment Syncing Start ");
                            IsProviderSyncedFirstTime = true;
                            //CallSynchCrystalPMToLocal();
                            myProcess.StartInfo.UseShellExecute = false;
                            // You can start any process, HelloWorld is a do-nothing example.
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\CrystalPM.exe";
                            myProcess.StartInfo.Arguments = "APPOITMENT";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.Start();
                            // This code assumes the process you are starting will terminate itself.
                            // Given that is is started without a window so you cannot terminate it
                            // on the desktop, it must terminate itself or you can do it programmatically
                            // from this application using the Kill method.
                            IsCrystalPMSyncing = false;
                            ObjGoalBase.WriteToSyncLogFile("Appointment Syncing Successfully. ");
                        }
                        catch (Exception e1)
                        {
                            IsCrystalPMSyncing = false;
                            ObjGoalBase.WriteToErrorLogFile("CrystalPMSync_Elapsed_Err " + e1.Message);
                            throw;
                        }
                    }

                    //SynchDataLiveDB_Push_Provider();
                    //SynchDataLiveDB_Push_Operatory();
                    //SynchDataLiveDB_Push_Speciality();
                    //SynchDataLiveDB_Push_Patient();
                    //SynchDataLiveDB_Push_ApptType();
                    //SynchDataLiveDB_Push_Appointment();
                    //SynchDataLiveDB_Push_OperatoryEvent();
                    //SynchDataLiveDB_Push_ApptStatus();
                }
                catch (Exception ex)
                {
                    CrystalPMSync.Enabled = true;
                    IsCrystalPMSyncing = false;
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsCrystalPMSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("CrystalPMSync_Elapsed_Err " + ex.Message);
                }
                finally
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsCrystalPMSyncing", false);
                    }
                    CrystalPMSync.Enabled = true;
                }
            }
        }

        #region CrystalPM payment code by shrutii
        public void CrystalPMPayment_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool IsSyncing = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
            {
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("IsCrystalPMPaymentSyncing").ToString());
                }
            }
            if (!IsCrystalPMPaymentSyncing && !IsSyncing)
            {
                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                {
                    key1.SetValue("IsCrystalPMPaymentSyncing", true);
                }

                tmrCrystalPMPayment.Enabled = false;
                IsCrystalPMPaymentSyncing = true;
                try
                {
                    using (Process myProcess = new Process())
                    {
                        try
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient Payment Syncing Start ");
                            IsProviderSyncedFirstTime = true;
                            myProcess.StartInfo.UseShellExecute = false;
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\CrystalPM.exe";
                            myProcess.StartInfo.Arguments = "PAYMENT";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.Start();
                            IsCrystalPMPaymentSyncing = false;
                            ObjGoalBase.WriteToSyncLogFile("Payment Syncing Successfully. ");
                        }
                        catch (Exception e1)
                        {
                            IsCrystalPMPaymentSyncing = false;
                            ObjGoalBase.WriteToErrorLogFile("tmrCrystalPMPayment_Elapsed_Err " + e1.Message);
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    tmrCrystalPMPayment.Enabled = true;
                    IsCrystalPMPaymentSyncing = false;
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsCrystalPMPaymentSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("CrystalPMPayment_Elapsed_Err " + ex.Message);
                }
                finally
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsCrystalPMPaymentSyncing", false);
                    }
                    tmrCrystalPMPayment.Enabled = true;
                }
            }
        }
        #endregion

        public void CrystalPMPatientSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
            bool IsSyncing = false;
            if (key != null)
            {
                IsSyncing = bool.Parse(key.GetValue("IsCrystalPMPatientSyncing").ToString());
            }

            if (!IsCrystalPMPatientSyncing && !IsSyncing)
            {
                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                {
                    key1.SetValue("IsCrystalPMPatientSyncing", true);
                }

                CrystalPMPatientSync.Enabled = false;
                IsCrystalPMPatientSyncing = true;
                try
                {
                    using (Process myProcess = new Process())
                    {

                        try
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient Syncing Start ");
                            // SynchDataCrystalPM_Patient();
                            myProcess.StartInfo.UseShellExecute = false;
                            // You can start any process, HelloWorld is a do-nothing example.
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\CrystalPM.exe";
                            myProcess.StartInfo.Arguments = "PATIENT";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.Start();
                            // This code assumes the process you are starting will terminate itself.
                            // Given that is is started without a window so you cannot terminate it
                            // on the desktop, it must terminate itself or you can do it programmatically
                            // from this application using the Kill method.
                        }
                        catch (Exception e1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("CrystalPMSync_Elapsed_Err " + e1.Message);
                            throw;
                        }
                    }

                    //SynchDataLiveDB_Push_Provider();
                    //SynchDataLiveDB_Push_Operatory();
                    //SynchDataLiveDB_Push_Speciality();
                    //SynchDataLiveDB_Push_Patient();
                    //SynchDataLiveDB_Push_ApptType();
                    //SynchDataLiveDB_Push_Appointment();
                    //SynchDataLiveDB_Push_OperatoryEvent();
                    //SynchDataLiveDB_Push_ApptStatus();

                    IsCrystalPMPatientSyncing = false;
                    //RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                    //key2.SetValue("IsCrystalPMPatientSyncing", false);
                    ObjGoalBase.WriteToSyncLogFile("Patient Synched Successfully..");
                }
                catch (Exception ex)
                {
                    CrystalPMPatientSync.Enabled = true;
                    IsCrystalPMPatientSyncing = false;
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsCrystalPMPatientSyncing", false);
                        ObjGoalBase.WriteToErrorLogFile("CrystalPMSync_Elapsed_Err " + ex.Message);
                    }
                }
                finally
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsCrystalPMPatientSyncing", false);
                    }
                    CrystalPMPatientSync.Enabled = true;
                }
            }
        }

        public void SyncCrystalPMPatientRecordsInitialy()
        {

            if (!IsCrystalPMPatientSyncing && Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    bool IsSyncing = false;
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        if (key != null)
                        {
                            IsSyncing = bool.Parse(key.GetValue("IsCrystalPMPatientSyncing").ToString());
                        }
                    }
                    if (!IsSyncing)
                    {
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsCrystalPMPatientSyncing", true);
                        }
                        IsCrystalPMPatientSyncing = true;
                        //   SynchDataCrystalPM_Patient();
                        using (Process myProcess = new Process())
                        {

                            try
                            {
                                //SynchDataCrystalPM_Patient();
                                myProcess.StartInfo.UseShellExecute = false;
                                // You can start any process, HelloWorld is a do-nothing example.
                                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\CrystalPM.exe";
                                myProcess.StartInfo.Arguments = "PATIENT";
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.Start();
                                //This code assumes the process you are starting will terminate itself.
                                //Given that is is started without a window so you cannot terminate it
                                //on the desktop, it must terminate itself or you can do it programmatically
                                //from this application using the Kill method.

                                //SynchDataLiveDB_Push_Provider();
                                //SynchDataLiveDB_Push_Operatory();
                                //SynchDataLiveDB_Push_Speciality();
                                //SynchDataLiveDB_Push_Patient();
                                //IsApptTypeSyncPush = true;
                                //SynchDataLiveDB_Push_Appointment();
                                //SynchDataLiveDB_Push_OperatoryEvent();
                                //SynchDataLiveDB_Push_ApptStatus();
                                IsCrystalPMPatientSyncing = false;

                            }
                            catch (Exception e1)
                            {
                                IsCrystalPMPatientSyncing = false;
                                using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key2.SetValue("IsCrystalPMPatientSyncing", false);
                                }
                                ObjGoalBase.WriteToErrorLogFile("CrystalPMSync_Elapsed_Err " + e1.Message);
                                throw;
                            }
                        }
                    }
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsCrystalPMPatientSyncing", false);
                    }
                }
                catch (Exception ex)
                {
                    IsCrystalPMPatientSyncing = false;
                    using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key2.SetValue("IsCrystalPMPatientSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("CrystalPMSync.exe_Elapsed_Err " + ex.Message);
                    throw;
                }
            }
        }

        public void SyncCrystalPMPatientPaymentRecordsInitialy()
        {

            if (!IsCrystalPMPaymentSyncing && Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    bool IsSyncing = false;
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        if (key != null)
                        {
                            IsSyncing = bool.Parse(key.GetValue("IsCrystalPMPaymentSyncing").ToString());
                        }
                    }
                    if (!IsSyncing)
                    {
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsCrystalPMPaymentSyncing", true);
                        }
                        IsCrystalPMPaymentSyncing = true;
                        using (Process myProcess = new Process())
                        {

                            try
                            {
                                myProcess.StartInfo.UseShellExecute = false;
                                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\CrystalPM.exe";
                                myProcess.StartInfo.Arguments = "PAYMENT";
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.Start();
                                IsCrystalPMPaymentSyncing = false;

                            }
                            catch (Exception e1)
                            {
                                IsCrystalPMPaymentSyncing = false;
                                using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key2.SetValue("IsCrystalPMPaymentSyncing", false);
                                }
                                ObjGoalBase.WriteToErrorLogFile("CrystalPMSync_Elapsed_Err " + e1.Message);
                                throw;
                            }
                        }
                    }
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsCrystalPMPaymentSyncing", false);
                    }
                }
                catch (Exception ex)
                {
                    IsCrystalPMPaymentSyncing = false;
                    using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key2.SetValue("IsCrystalPMPaymentSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("CrystalPMSync.exe_Elapsed_Err " + ex.Message);
                    throw;
                }
            }
        }
        public void SyncCrystalPMRecordsInitialy()
        {

            if (!IsCrystalPMSyncing && Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    bool IsSyncing = false;
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        if (key != null)
                        {
                            IsSyncing = bool.Parse(key.GetValue("IsCrystalPMSyncing").ToString());
                        }
                    }
                    if (!IsSyncing)
                    {
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsCrystalPMSyncing", true);
                        }
                        IsCrystalPMSyncing = true;
                        using (Process myProcess = new Process())
                        {

                            try
                            {
                                //SynchDataLiveDB_Pull_PatientForm();
                                // CallSynchCrystalPMToLocal();
                                IsProviderSyncedFirstTime = true;
                                // CallSynchCrystalPMToLocal();
                                //SynchDataLiveDB_Push_Appointment();
                                myProcess.StartInfo.UseShellExecute = false;
                                // You can start any process, HelloWorld is a do-nothing example.
                                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\CrystalPM.exe";
                                myProcess.StartInfo.Arguments = "APPOITMENT";
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.Start();
                                //This code assumes the process you are starting will terminate itself.
                                //Given that is is started without a window so you cannot terminate it
                                //on the desktop, it must terminate itself or you can do it programmatically
                                //from this application using the Kill method.

                                //SynchDataLiveDB_Push_Provider();
                                //SynchDataLiveDB_Push_Operatory();
                                //SynchDataLiveDB_Push_Speciality();
                                //SynchDataLiveDB_Push_Patient();
                                //IsApptTypeSyncPush = true;
                                //SynchDataLiveDB_Push_Appointment();
                                //SynchDataLiveDB_Push_OperatoryEvent();
                                //SynchDataLiveDB_Push_ApptStatus();
                                IsCrystalPMSyncing = false;
                                using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key2.SetValue("IsCrystalPMSyncing", false);
                                }


                            }
                            catch (Exception e1)
                            {
                                IsCrystalPMSyncing = false;
                                using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key2.SetValue("IsCrystalPMSyncing", false);
                                }
                                ObjGoalBase.WriteToErrorLogFile("CrystalPMSync_Elapsed_Err " + e1.Message);
                                throw;
                            }
                        }
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsCrystalPMSyncing", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsCrystalPMSyncing = false;
                    using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key2.SetValue("IsCrystalPMSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("CrystalPMSync.exe_Elapsed_Err " + ex.Message);
                    throw;
                }
            }
        }

    }
}
