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

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable

        public bool IsPracticeWebProviderSync = false;
        public bool IsPracticeWebOperatorySync = false;
        public bool IsPracticeWebApptTypeSync = false;

        private BackgroundWorker bwSynchPracticeWeb_Appointment = null;
        private System.Timers.Timer timerSynchPracticeWeb_Appointment = null;

        private BackgroundWorker bwSynchPracticeWeb_OperatoryEvent = null;
        private System.Timers.Timer timerSynchPracticeWeb_OperatoryEvent = null;

        private BackgroundWorker bwSynchPracticeWeb_Provider = null;
        private System.Timers.Timer timerSynchPracticeWeb_Provider = null;

        private BackgroundWorker bwSynchPracticeWeb_Disease = null;
        private System.Timers.Timer timerSynchPracticeWeb_Disease = null;

        private BackgroundWorker bwSynchPracticeWeb_ProviderHours = null;
        private System.Timers.Timer timerSynchPracticeWeb_ProviderHours = null;

        private BackgroundWorker bwSynchPracticeWeb_PatientPayment = null;
        private System.Timers.Timer timerSynchPracticeWeb_PatientPayment = null;

        private BackgroundWorker bwSynchPracticeWeb_Speciality = null;
        private System.Timers.Timer timerSynchPracticeWeb_Speciality = null;

        private BackgroundWorker bwSynchPracticeWeb_Operatory = null;
        private System.Timers.Timer timerSynchPracticeWeb_Operatory = null;

        private BackgroundWorker bwSynchPracticeWeb_ApptType = null;
        private System.Timers.Timer timerSynchPracticeWeb_ApptType = null;

        private BackgroundWorker bwSynchPracticeWeb_Patient = null;
        private System.Timers.Timer timerSynchPracticeWeb_Patient = null;

        private BackgroundWorker bwSynchPracticeWeb_RecallType = null;
        private System.Timers.Timer timerSynchPracticeWeb_RecallType = null;

        private BackgroundWorker bwSynchPracticeWeb_User = null;
        private System.Timers.Timer timerSynchPracticeWeb_User = null;

        private BackgroundWorker bwSynchPracticeWeb_ApptStatus = null;
        private System.Timers.Timer timerSynchPracticeWeb_ApptStatus = null;

        private BackgroundWorker bwSynchPracticeWeb_Holiday = null;
        private System.Timers.Timer timerSynchPracticeWeb_Holiday = null;

        //  private BackgroundWorker bwSynchPracticeWeb_PatientWiseRecallDate = null;
        // private System.Timers.Timer timerSynchPracticeWeb_PatientWiseRecallDate = null;

        private BackgroundWorker bwSynchLocalToPracticeWeb_Appointment = null;
        private System.Timers.Timer timerSynchLocalToPracticeWeb_Appointment = null;

        private BackgroundWorker bwSynchLocalToPracticeWeb_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToPracticeWeb_Patient_Form = null;

        private BackgroundWorker bwSynchPracticeWeb_MedicalHistory = null;
        private System.Timers.Timer timerSynchPracticeWeb_MedicalHistory = null;

        private BackgroundWorker bwSynchPracticeWeb_Insurance = null;
        private System.Timers.Timer timerSynchPracticeWeb_Insurance = null;

        #endregion

        private void CallSynchPracticeWebToLocal()
        {
            if (Utility.AditSync)
            {
                // SynchDataLocalToPracticeWeb_PatientPayment();                
                //SynchDataPatientSMSCall_LocalTOPracticeWeb();
                //SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                //SynchDataLiveDB_Pull_PatientFollowUp();
                //SynchDataPatientSMSCall_LocalTOPracticeWeb();

                // SynchDataLocalToOpenDental_PatientPayment();
                fncSynchDataPracticeWeb_PatientPayment();

                SynchDataPracticeWeb_Speciality();
                fncSynchDataPracticeWeb_Speciality();

                SynchDataPracticeWeb_Operatory();
                fncSynchDataPracticeWeb_Operatory();

                SynchDataPracticeWeb_Provider();
                fncSynchDataPracticeWeb_Provider();

                SynchDataPracticeWeb_Disease();
                fncSynchDataPracticeWeb_Disease();

                //SynchDataPracticeWeb_ProviderHours();
                fncSynchDataPracticeWeb_ProviderHours();

                SynchDataPracticeWeb_ApptType();
                fncSynchDataPracticeWeb_ApptType();

                //SynchDataLocalToPracticeWeb_Appointment();
                fncSynchDataLocalToPracticeWeb_Appointment();

                SynchDataPracticeWeb_RecallType();
                fncSynchDataPracticeWeb_RecallType();

                SynchDataPracticeWeb_User();
                fncSynchDataPracticeWeb_User();

                SynchDataPracticeWeb_ApptStatus();
                fncSynchDataPracticeWeb_ApptStatus();

                //fncSynchDataPracticeWeb_Appointment();

                SynchDataPracticeWeb_Holiday();
                fncSynchDataPracticeWeb_Holiday();

                SynchDataLocalToPracticeWeb_Patient_Form();
                fncSynchDataLocalToPracticeWeb_Patient_Form();

                SynchDataPracticeWeb_MedicalHistory();
                fncSynchDataPracticeWeb_MedicalHistory();

                //rooja 19-4-24 task link EHR Updates for RCM - https://app.asana.com/0/1199184824722493/1207061756651636/f
                SynchDataPracticeWeb_Insurance();
                fncSynchDataPracticeWeb_Insurance();

                //SynchDataPracticeWeb_PatientImages();

                // SynchDataPracticeWeb_PatientWiseRecallDate();
                //fncSynchDataPracticeWeb_PatientWiseRecallDate();
            }
        }

        #region Synch Appointment

        private void fncSynchDataPracticeWeb_Appointment()
        {
            InitBgWorkerPracticeWeb_Appointment();
            InitBgTimerPracticeWeb_Appointment();
        }

        private void InitBgTimerPracticeWeb_Appointment()
        {
            timerSynchPracticeWeb_Appointment = new System.Timers.Timer();
            this.timerSynchPracticeWeb_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchPracticeWeb_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_Appointment_Tick);
            timerSynchPracticeWeb_Appointment.Enabled = true;
            timerSynchPracticeWeb_Appointment.Start();
            timerSynchPracticeWeb_Appointment_Tick(null, null);
        }

        private void InitBgWorkerPracticeWeb_Appointment()
        {
            bwSynchPracticeWeb_Appointment = new BackgroundWorker();
            bwSynchPracticeWeb_Appointment.WorkerReportsProgress = true;
            bwSynchPracticeWeb_Appointment.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_Appointment.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWeb_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_Appointment_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {

                timerSynchPracticeWeb_Appointment.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_Appointment();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_Appointment()
        {
            System.Threading.Thread procThreadmainPracticeWeb_Appointment = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_Appointment);
            procThreadmainPracticeWeb_Appointment.Start();
        }

        public void CallSyncOrderTablePracticeWeb_Appointment()
        {
            if (bwSynchPracticeWeb_Appointment.IsBusy != true)
            {
                bwSynchPracticeWeb_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLocalToPracticeWeb_PatientPayment();
                SynchDataPracticeWeb_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_Appointment.Enabled = true;
        }

        public void SynchDataPracticeWeb_Appointment()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                IsPracticeWebProviderSync = true;
                IsPracticeWebOperatorySync = true;
                IsPracticeWebApptTypeSync = true;
                Is_synched_Appointment = true;
            }
            if (IsPracticeWebProviderSync && IsPracticeWebOperatorySync && IsPracticeWebApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff) // && Utility.AditLocationSyncEnable 
            {
                try
                {
                    try
                    {
                        SynchDataPracticeWeb_AppointmentPatient();
                        if (IsParientFirstSync)
                        {
                            SynchDataPracticeWeb_InsertPatient();
                        }
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[AppointmentPatient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex1.Message);
                    }
                    try
                    {
                        SynchDataPracticeWeb_PatientStatus();
                    }
                    catch (Exception ex2)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex2.Message);
                    }
                    Is_synched_Appointment = false;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebAppointment = SynchPracticeWebBAL.GetPracticeWebAppointmentData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        // DataTable dtPracticeWebDeletedAppointment = SynchPracticeWebBAL.GetPracticeWebDeletedAppointmentData();                    

                        dtPracticeWebAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                        dtPracticeWebAppointment.Columns.Add("InsUptDlt", typeof(int));
                        dtPracticeWebAppointment.Columns.Add("ProcedureDesc", typeof(string));
                        dtPracticeWebAppointment.Columns.Add("ProcedureCode", typeof(string));

                        DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        DataTable DtPracticeWebAppointment_Procedures_Data = SynchPracticeWebBAL.GetPracticeWebAppointment_Procedures_Data(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        string ProcedureDesc = "";
                        string ProcedureCode = "";

                        foreach (DataRow dtDtxRow in dtPracticeWebAppointment.Rows)
                        {
                            try
                            {
                                ///////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                                ProcedureDesc = "";
                                ProcedureCode = "";
                                // DataTable dtCurApptProc = DtPracticeWebAppointment_Procedures_Data.Select("aptnum = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'").CopyToDataTable();
                                DataRow[] dtCurApptProcedure = DtPracticeWebAppointment_Procedures_Data.Select("aptnum = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");
                                foreach (var dtSinProc in dtCurApptProcedure.ToList())
                                {
                                    ProcedureDesc = ProcedureDesc + dtSinProc["ProcedureDesc"].ToString().Trim();
                                    ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                                }

                                dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                                dtDtxRow["ProcedureCode"] = ProcedureCode;

                                ////////////////////////////////

                                DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                                if (row.Length > 0)
                                {
                                    //DataRow dr = row.FirstOrDefault(a =>  a["Clinic_Number"].ToString() ==  dtDtxRow["Clinic_Number"].ToString());
                                    //if (dr == null)
                                    //    dtDtxRow["InsUptDlt"] = 3;
                                    //else

                                    if (Convert.ToDateTime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EHR_Entry_DateTime"]))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Last_Name"].ToString().ToLower().Trim() != row[0]["Last_Name"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["MI"].ToString().ToLower().Trim() != row[0]["MI"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Utility.ConvertContactNumber(dtDtxRow["Home_Contact"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").ToLower().Trim()) != Utility.ConvertContactNumber(row[0]["Home_Contact"].ToString().ToLower().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().ToLower().Trim()) != Utility.ConvertContactNumber(row[0]["Mobile_Contact"].ToString().ToLower().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Email"].ToString().ToLower().Trim() != row[0]["Email"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Address"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["City"].ToString().ToLower().Trim() != row[0]["City"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["ST"].ToString().ToLower().Trim() != row[0]["ST"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["is_asap"] != null && dtDtxRow["is_asap"].ToString() != string.Empty && Convert.ToBoolean(dtDtxRow["is_asap"]) != Convert.ToBoolean(row[0]["is_asap"]))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["Zip"].ToString().ToLower().Trim() != row[0]["Zip"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["ProcedureDesc"].ToString().ToLower().Trim() != row[0]["ProcedureDesc"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["ProcedureCode"].ToString().ToLower().Trim() != row[0]["ProcedureCode"].ToString().ToLower().Trim())
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
                                    DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) + "'  AND ISNULL(Appt_EHR_ID, '0') = '0' ");
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
                            catch (Exception ex)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Appointment (" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + ") Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                            }
                        }

                        DataTable dtPracticeWebDeletedAppointment = SynchPracticeWebBAL.GetPracticeWebDeletedAppointmentData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        DataTable dtLocalAppointmentAfterInsert = SynchLocalBAL.GetLocalAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        foreach (DataRow dtDtlRow in dtPracticeWebDeletedAppointment.Rows)
                        {
                            DataRow[] row = dtLocalAppointmentAfterInsert.Copy().Select("Appt_EHR_ID = '" + dtDtlRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length > 0)
                            {
                                if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                {
                                    DataRow ApptDtldr = dtPracticeWebAppointment.NewRow();
                                    ApptDtldr["Appt_EHR_ID"] = dtDtlRow["Appt_EHR_ID"].ToString().Trim();
                                    ApptDtldr["InsUptDlt"] = 3;
                                    dtPracticeWebAppointment.Rows.Add(ApptDtldr);
                                }
                            }
                            else
                            {
                                DataRow ApptDtldr = dtPracticeWebAppointment.NewRow();
                                ApptDtldr["Appt_EHR_ID"] = dtDtlRow["Appt_EHR_ID"].ToString().Trim();
                                ApptDtldr["InsUptDlt"] = 6;
                                dtPracticeWebAppointment.Rows.Add(ApptDtldr);
                            }
                        }

                        dtPracticeWebAppointment.AcceptChanges();

                        if (dtPracticeWebAppointment != null && dtPracticeWebAppointment.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_Appointment_PracticeWeb_To_Local(dtPracticeWebAppointment, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                                ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + "  to Local Database) Successfully.");
                                SynchDataLiveDB_Push_Appointment();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                    }

                    Is_synched_Appointment = true;

                    IsEHRAllSync = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
                finally
                {
                    Is_synched_Appointment = true;
                    // this.Dispose();
                }
            }
        }

        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataPracticeWeb_OperatoryEvent()
        {
            InitBgWorkerPracticeWeb_OperatoryEvent();
            InitBgTimerPracticeWeb_OperatoryEvent();
        }

        private void InitBgTimerPracticeWeb_OperatoryEvent()
        {
            timerSynchPracticeWeb_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchPracticeWeb_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchPracticeWeb_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_OperatoryEvent_Tick);
            timerSynchPracticeWeb_OperatoryEvent.Enabled = true;
            timerSynchPracticeWeb_OperatoryEvent.Start();
            timerSynchPracticeWeb_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerPracticeWeb_OperatoryEvent()
        {
            bwSynchPracticeWeb_OperatoryEvent = new BackgroundWorker();
            bwSynchPracticeWeb_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchPracticeWeb_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWeb_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainPracticeWeb_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_OperatoryEvent);
            procThreadmainPracticeWeb_OperatoryEvent.Start();
        }

        public void CallSyncOrderTablePracticeWeb_OperatoryEvent()
        {
            if (bwSynchPracticeWeb_OperatoryEvent.IsBusy != true)
            {
                bwSynchPracticeWeb_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_OperatoryEvent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_OperatoryEvent.Enabled = true;
        }

        public void SynchDataPracticeWeb_OperatoryEvent()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                IsPracticeWebOperatorySync = true;
                Is_synched_OperatoryEvent = false;
            }
            if (IsPracticeWebOperatorySync && !Is_synched_OperatoryEvent && Utility.IsApplicationIdleTimeOff) // && Utility.AditLocationSyncEnable
            {
                try
                {
                    Is_synched_OperatoryEvent = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebOperatoryEvent = SynchPracticeWebBAL.GetPracticeWebOperatoryEventData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        // DataTable dtPracticeWebOperatoryAllEvent = SynchPracticeWebBAL.GetPracticeWebOperatoryAllEventData();
                        dtPracticeWebOperatoryEvent.Columns.Add("OE_LocalDB_ID", typeof(int));
                        dtPracticeWebOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebOperatoryEvent.Rows)
                        {
                            DataRow[] row = dtLocalOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtDtxRow["ScheduleNum"].ToString().Trim() + "' ");
                            if (row.Length > 0)
                            {
                                if (Convert.ToDateTime(dtDtxRow["DateTStamp"].ToString().Trim()) != Convert.ToDateTime(row[0]["Entry_DateTime"]))
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
                            DataRow[] rowBlcOpt = dtPracticeWebOperatoryEvent.Copy().Select("ScheduleNum = '" + dtLOERow["OE_EHR_ID"].ToString().Trim() + "' ");
                            if (rowBlcOpt.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtPracticeWebOperatoryEvent.NewRow();
                                BlcOptDtldr["ScheduleNum"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtPracticeWebOperatoryEvent.Rows.Add(BlcOptDtldr);
                            }
                        }

                        dtPracticeWebOperatoryEvent.AcceptChanges();

                        if (dtPracticeWebOperatoryEvent != null && dtPracticeWebOperatoryEvent.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_OperatoryEvent_PracticeWeb_To_Local(dtPracticeWebOperatoryEvent, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_OperatoryEvent();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[OperatoryEvent Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                        else
                        {
                            bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent_Push");
                        }
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
                        IsEHRAllSync = true;
                    }
                    Is_synched_OperatoryEvent = false;
                }
                catch (Exception ex)
                {
                    Is_synched_OperatoryEvent = false;
                    ObjGoalBase.WriteToErrorLogFile("[OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Provider

        private void fncSynchDataPracticeWeb_Provider()
        {
            InitBgWorkerPracticeWeb_Provider();
            InitBgTimerPracticeWeb_Provider();
        }

        private void InitBgTimerPracticeWeb_Provider()
        {
            timerSynchPracticeWeb_Provider = new System.Timers.Timer();
            this.timerSynchPracticeWeb_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchPracticeWeb_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_Provider_Tick);
            timerSynchPracticeWeb_Provider.Enabled = true;
            timerSynchPracticeWeb_Provider.Start();
        }

        private void InitBgWorkerPracticeWeb_Provider()
        {
            bwSynchPracticeWeb_Provider = new BackgroundWorker();
            bwSynchPracticeWeb_Provider.WorkerReportsProgress = true;
            bwSynchPracticeWeb_Provider.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_Provider.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWeb_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_Provider_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_Provider.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_Provider();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_Provider()
        {
            System.Threading.Thread procThreadmainPracticeWeb_Provider = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_Provider);
            procThreadmainPracticeWeb_Provider.Start();
        }

        public void CallSyncOrderTablePracticeWeb_Provider()
        {
            if (bwSynchPracticeWeb_Provider.IsBusy != true)
            {
                bwSynchPracticeWeb_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_Provider();
                CommonFunction.GetMasterSync();

                //  SynchDataPracticeWeb_ProviderHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_Provider.Enabled = true;
        }

        public void SynchDataPracticeWeb_Provider()
        {
            try
            {
                if (!Is_synched_Provider && Utility.IsApplicationIdleTimeOff) //&& Utility.AditLocationSyncEnable
                {
                    Is_synched_Provider = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebProvider = SynchPracticeWebBAL.GetPracticeWebProviderData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtPracticeWebProvider.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebProvider.Rows)
                        {
                            DataRow[] row = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtDtxRow["Provider_EHR_ID"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Last_Name"].ToString().Trim() != row[0]["Last_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["First_Name"].ToString().Trim() != row[0]["First_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["MI"].ToString().Trim() != row[0]["MI"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["provider_speciality"].ToString().Trim() != row[0]["provider_speciality"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (Convert.ToByte(dtDtxRow["is_active"].ToString().Trim()) != Convert.ToByte(row[0]["is_active"]))
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

                        dtPracticeWebProvider.AcceptChanges();

                        if (dtPracticeWebProvider != null && dtPracticeWebProvider.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_Provider_PracticeWeb_To_Local(dtPracticeWebProvider, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                                ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                IsPracticeWebProviderSync = true;
                                SynchDataLiveDB_Push_Provider();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Providers Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                IsPracticeWebProviderSync = false;
                            }
                        }
                    }
                    Is_synched_Provider = false;
                    IsProviderSyncedFirstTime = true;
                }
            }
            catch (Exception ex)
            {
                Is_synched_Provider = false;
                IsProviderSyncedFirstTime = true;
                ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        //public void SynchDataPracticeWeb_ProviderHours()
        //{
        //    try
        //    {
        //        if (Utility.IsApplicationIdleTimeOff && IsPracticeWebProviderSync && IsPracticeWebOperatorySync)
        //        {
        //            DataTable dtPracticeWebProviderHours = SynchPracticeWebBAL.GetPracticeWebProviderHoursData();
        //            dtPracticeWebProviderHours.Columns.Add("InsUptDlt", typeof(int));
        //            DataTable dtLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData();

        //            foreach (DataRow dtDtxRow in dtPracticeWebProviderHours.Rows)
        //            {
        //                DataRow[] row = dtLocalProviderHours.Copy().Select("PH_EHR_ID = '" + dtDtxRow["PH_EHR_ID"] + "'");
        //                if (row.Length > 0)
        //                {
        //                    if (Convert.ToDateTime(dtDtxRow["Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Entry_DateTime"].ToString().Trim()))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                }
        //                else
        //                {
        //                    dtDtxRow["InsUptDlt"] = 1;
        //                }
        //            }
        //            dtPracticeWebProviderHours.AcceptChanges();

        //            foreach (DataRow dtLPHRow in dtLocalProviderHours.Rows)
        //            {
        //                DataRow[] rowBlcOpt = dtPracticeWebProviderHours.Copy().Select("PH_EHR_ID = '" + dtLPHRow["PH_EHR_ID"].ToString().Trim() + "' ");
        //                if (rowBlcOpt.Length > 0)
        //                { }
        //                else
        //                {
        //                    DataRow BlcOptDtldr = dtPracticeWebProviderHours.NewRow();
        //                    BlcOptDtldr["PH_EHR_ID"] = dtLPHRow["PH_EHR_ID"].ToString().Trim();
        //                    BlcOptDtldr["InsUptDlt"] = 3;
        //                    dtPracticeWebProviderHours.Rows.Add(BlcOptDtldr);
        //                }
        //            }

        //            dtPracticeWebProviderHours.AcceptChanges();

        //            if (dtPracticeWebProviderHours != null && dtPracticeWebProviderHours.Rows.Count > 0)
        //            {
        //                bool status = SynchPracticeWebBAL.Save_ProviderHours_PracticeWeb_To_Local(dtPracticeWebProviderHours);

        //                if (status)
        //                {
        //                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
        //                    ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

        //                    SynchDataLiveDB_Push_ProviderHours();
        //                }

        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Is_synched_Provider = false;
        //        ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //}


        #endregion

        #region Synch ProviderHours

        private void fncSynchDataPracticeWeb_ProviderHours()
        {
            InitBgWorkerPracticeWeb_ProviderHours();
            InitBgTimerPracticeWeb_ProviderHours();
        }

        private void InitBgTimerPracticeWeb_ProviderHours()
        {
            timerSynchPracticeWeb_ProviderHours = new System.Timers.Timer();
            this.timerSynchPracticeWeb_ProviderHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchPracticeWeb_ProviderHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_ProviderHours_Tick);
            timerSynchPracticeWeb_ProviderHours.Enabled = true;
            timerSynchPracticeWeb_ProviderHours.Start();
        }

        private void InitBgWorkerPracticeWeb_ProviderHours()
        {
            bwSynchPracticeWeb_ProviderHours = new BackgroundWorker();
            bwSynchPracticeWeb_ProviderHours.WorkerReportsProgress = true;
            bwSynchPracticeWeb_ProviderHours.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_ProviderHours.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_ProviderHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWeb_ProviderHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_ProviderHours_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_ProviderHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_ProviderHours.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_ProviderHours();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_ProviderHours()
        {
            System.Threading.Thread procThreadmainPracticeWeb_ProviderHours = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_ProviderHours);
            procThreadmainPracticeWeb_ProviderHours.Start();
        }

        public void CallSyncOrderTablePracticeWeb_ProviderHours()
        {
            if (bwSynchPracticeWeb_ProviderHours.IsBusy != true)
            {
                bwSynchPracticeWeb_ProviderHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_ProviderHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_ProviderHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SyncPullLogsAndSaveinPracticeWeb();
                SynchDataPracticeWeb_ProviderHours();

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SyncPullLogsAndSaveinPracticeWeb()
        {
            try
            {
                CheckCustomhoursForProviderOperatory();
                SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                SynchDataLiveDB_Pull_PatientFollowUp();
                SynchDataPatientSMSCall_LocalTOPracticeWeb();
                fncPaymentSMSCallStatusUpdate();
                SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
            }
            catch (Exception)
            {

            }
        }

        private void bwSynchPracticeWeb_ProviderHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_ProviderHours.Enabled = true;
        }

        public void SynchDataPracticeWeb_ProviderHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && IsPracticeWebProviderSync && IsPracticeWebOperatorySync && Utility.is_scheduledCustomhour)// && Utility.AditLocationSyncEnable
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebProviderHours = SynchPracticeWebBAL.GetPracticeWebProviderHoursData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtPracticeWebProviderHours.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebProviderHours.Rows)
                        {
                            DataRow[] row = dtLocalProviderHours.Copy().Select("PH_EHR_ID = '" + dtDtxRow["PH_EHR_ID"] + "'");
                            if (row.Length > 0)
                            {
                                if (Convert.ToDateTime(dtDtxRow["Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Entry_DateTime"].ToString().Trim()))
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        dtPracticeWebProviderHours.AcceptChanges();

                        foreach (DataRow dtLPHRow in dtLocalProviderHours.Rows)
                        {
                            DataRow[] rowBlcOpt = dtPracticeWebProviderHours.Copy().Select("PH_EHR_ID = '" + dtLPHRow["PH_EHR_ID"].ToString().Trim() + "' ");
                            if (rowBlcOpt.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtPracticeWebProviderHours.NewRow();
                                BlcOptDtldr["PH_EHR_ID"] = dtLPHRow["PH_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["StartTime"] = dtLPHRow["StartTime"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtPracticeWebProviderHours.Rows.Add(BlcOptDtldr);
                            }
                        }

                        dtPracticeWebProviderHours.AcceptChanges();

                        if (dtPracticeWebProviderHours != null && dtPracticeWebProviderHours.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_ProviderHours_PracticeWeb_To_Local(dtPracticeWebProviderHours, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
                                ObjGoalBase.WriteToSyncLogFile("ProviderHours Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[ProviderHours Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                    }
                    SynchDataLiveDB_Push_ProviderHours();
                    SynchDataLiveDB_Push_OperatoryHours();
                }
            }
            catch (Exception ex)
            {
                Is_synched_Provider = false;
                ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Speciality

        private void fncSynchDataPracticeWeb_Speciality()
        {
            InitBgWorkerPracticeWeb_Speciality();
            InitBgTimerPracticeWeb_Speciality();
        }

        private void InitBgTimerPracticeWeb_Speciality()
        {
            timerSynchPracticeWeb_Speciality = new System.Timers.Timer();
            this.timerSynchPracticeWeb_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchPracticeWeb_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_Speciality_Tick);
            timerSynchPracticeWeb_Speciality.Enabled = true;
            timerSynchPracticeWeb_Speciality.Start();
        }

        private void InitBgWorkerPracticeWeb_Speciality()
        {
            bwSynchPracticeWeb_Speciality = new BackgroundWorker();
            bwSynchPracticeWeb_Speciality.WorkerReportsProgress = true;
            bwSynchPracticeWeb_Speciality.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_Speciality.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_Speciality_DoWork);
            bwSynchPracticeWeb_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_Speciality_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_Speciality.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_Speciality();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_Speciality()
        {
            System.Threading.Thread procThreadmainPracticeWeb_Speciality = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_Speciality);
            procThreadmainPracticeWeb_Speciality.Start();
        }

        public void CallSyncOrderTablePracticeWeb_Speciality()
        {
            if (bwSynchPracticeWeb_Speciality.IsBusy != true)
            {
                bwSynchPracticeWeb_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_Speciality();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_Speciality.Enabled = true;
        }

        public void SynchDataPracticeWeb_Speciality()
        {
            try
            {

                if (!Is_synched_Speciality && Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    Is_synched_Speciality = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebSpeciality = SynchPracticeWebBAL.GetPracticeWebProviderData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        DataView view = new DataView(dtPracticeWebSpeciality);
                        DataTable dtSpecialitydistinctValues = view.ToTable(true, "provider_speciality", "Clinic_Number");
                        dtSpecialitydistinctValues.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtSpecialitydistinctValues.Rows)
                        {
                            DataRow[] row = dtLocalSpeciality.Copy().Select("speciality_Name = '" + dtDtxRow["provider_speciality"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
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
                            bool status = SynchPracticeWebBAL.Save_Speciality_PracticeWeb_To_Local(dtSpecialitydistinctValues, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality");
                                ObjGoalBase.WriteToSyncLogFile("Speciality Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + "  to Local Database) Successfully.");
                                SynchDataLiveDB_Push_Speciality();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                    }
                    Is_synched_Speciality = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_Speciality = false;
                ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Operatory

        private void fncSynchDataPracticeWeb_Operatory()
        {
            InitBgWorkerPracticeWeb_Operatory();
            InitBgTimerPracticeWeb_Operatory();
        }

        private void InitBgTimerPracticeWeb_Operatory()
        {
            timerSynchPracticeWeb_Operatory = new System.Timers.Timer();
            this.timerSynchPracticeWeb_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchPracticeWeb_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_Operatory_Tick);
            timerSynchPracticeWeb_Operatory.Enabled = true;
            timerSynchPracticeWeb_Operatory.Start();
        }

        private void InitBgWorkerPracticeWeb_Operatory()
        {
            bwSynchPracticeWeb_Operatory = new BackgroundWorker();
            bwSynchPracticeWeb_Operatory.WorkerReportsProgress = true;
            bwSynchPracticeWeb_Operatory.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_Operatory.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_Operatory_DoWork);
            bwSynchPracticeWeb_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_Operatory_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_Operatory.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_Operatory();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_Operatory()
        {
            System.Threading.Thread procThreadmainPracticeWeb_Operatory = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_Operatory);
            procThreadmainPracticeWeb_Operatory.Start();
        }

        public void CallSyncOrderTablePracticeWeb_Operatory()
        {
            if (bwSynchPracticeWeb_Operatory.IsBusy != true)
            {
                bwSynchPracticeWeb_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_Operatory.Enabled = true;
        }

        public void SynchDataPracticeWeb_Operatory()
        {
            try
            {
                if (!Is_synched_Operatory && Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    Is_synched_Operatory = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebOperatory = SynchPracticeWebBAL.GetPracticeWebOperatoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtPracticeWebOperatory.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebOperatory.Rows)
                        {
                            DataRow[] row = dtLocalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtDtxRow["Operatory_EHR_ID"] + "'");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Operatory_Name"].ToString().Trim() != row[0]["Operatory_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }

                                else if (Convert.ToInt16(dtDtxRow["OperatoryOrder"]) != Convert.ToInt16(row[0]["OperatoryOrder"]))
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (Convert.ToBoolean(dtDtxRow["Is_Deleted"]) != Convert.ToBoolean(row[0]["Is_Deleted"]))
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 0;
                                }

                                if (Convert.ToInt32(dtDtxRow["Clinic_Number"]) != Convert.ToInt32(row[0]["Clinic_Number"]))
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }

                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }

                        dtPracticeWebOperatory.AcceptChanges();
                        if (dtPracticeWebOperatory != null && dtPracticeWebOperatory.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_Operatory_PracticeWeb_To_Local(dtPracticeWebOperatory, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                                ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                IsPracticeWebOperatorySync = true;

                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                IsPracticeWebOperatorySync = false;
                            }

                            #region Deleted Operatory
                            dtPracticeWebOperatory = dtPracticeWebOperatory.Clone();
                            DataTable dtPracticeWebDeletedOperatory = SynchPracticeWebBAL.GetPracticeWebDeletedOperatoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            DataTable dtLocalOperatoryAfterInsert = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            foreach (DataRow dtDtlRow in dtPracticeWebDeletedOperatory.Rows)
                            {
                                DataRow[] row = dtLocalOperatoryAfterInsert.Copy().Select("Operatory_EHR_ID = '" + dtDtlRow["Operatory_EHR_ID"].ToString().Trim() + "'");
                                if (row.Length > 0)
                                {
                                    if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                    {
                                        DataRow ApptDtldr = dtPracticeWebOperatory.NewRow();
                                        ApptDtldr["Operatory_EHR_ID"] = dtDtlRow["Operatory_EHR_ID"].ToString().Trim();
                                        ApptDtldr["InsUptDlt"] = 3;
                                        dtPracticeWebOperatory.Rows.Add(ApptDtldr);
                                    }
                                }
                            }
                            if (dtPracticeWebOperatory != null && dtPracticeWebOperatory.Rows.Count > 0)
                            {
                                status = SynchPracticeWebBAL.Save_Operatory_PracticeWeb_To_Local(dtPracticeWebOperatory, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            }
                            #endregion

                            SynchDataLiveDB_Push_Operatory();
                        }
                    }
                    Is_synched_Operatory = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_Operatory = false;
                ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Appointment Type

        private void fncSynchDataPracticeWeb_ApptType()
        {
            InitBgWorkerPracticeWeb_ApptType();
            InitBgTimerPracticeWeb_ApptType();
        }

        private void InitBgTimerPracticeWeb_ApptType()
        {
            timerSynchPracticeWeb_ApptType = new System.Timers.Timer();
            this.timerSynchPracticeWeb_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchPracticeWeb_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_ApptType_Tick);
            timerSynchPracticeWeb_ApptType.Enabled = true;
            timerSynchPracticeWeb_ApptType.Start();
        }

        private void InitBgWorkerPracticeWeb_ApptType()
        {
            bwSynchPracticeWeb_ApptType = new BackgroundWorker();
            bwSynchPracticeWeb_ApptType.WorkerReportsProgress = true;
            bwSynchPracticeWeb_ApptType.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_ApptType.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_ApptType_DoWork);
            bwSynchPracticeWeb_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_ApptType_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_ApptType.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_ApptType();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_ApptType()
        {
            System.Threading.Thread procThreadmainPracticeWeb_ApptType = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_ApptType);
            procThreadmainPracticeWeb_ApptType.Start();
        }

        public void CallSyncOrderTablePracticeWeb_ApptType()
        {
            if (bwSynchPracticeWeb_ApptType.IsBusy != true)
            {
                bwSynchPracticeWeb_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_ApptType.Enabled = true;
        }

        public void SynchDataPracticeWeb_ApptType()
        {
            try
            {
                if (!Is_synched_ApptType && Utility.IsApplicationIdleTimeOff)// && Utility.AditLocationSyncEnable
                {
                    Is_synched_ApptType = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebApptType = SynchPracticeWebBAL.GetPracticeWebApptTypeData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtPracticeWebApptType.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString())
                            {
                                DataRow Addrow = dtPracticeWebApptType.NewRow();
                                Addrow["ApptType_EHR_ID"] = 0;
                                Addrow["Type_Name"] = "none";
                                Addrow["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"];
                                dtPracticeWebApptType.Rows.Add(Addrow);
                                dtPracticeWebApptType.AcceptChanges();
                            }
                        }

                        foreach (DataRow dtDtxRow in dtPracticeWebApptType.Rows)
                        {
                            DataRow[] row = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Type_Name"].ToString().Trim() != row[0]["Type_Name"].ToString().Trim())
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
                        foreach (DataRow dtDtxRow in dtLocalApptType.Rows)
                        {
                            DataRow[] row = dtPracticeWebApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                            if (row.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtPracticeWebApptType.NewRow();
                                BlcOptDtldr["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["Type_Name"] = dtDtxRow["Type_Name"].ToString().Trim();
                                BlcOptDtldr["Clinic_Number"] = dtDtxRow["Clinic_Number"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtPracticeWebApptType.Rows.Add(BlcOptDtldr);
                            }
                        }

                        dtPracticeWebApptType.AcceptChanges();

                        if (dtPracticeWebApptType != null && dtPracticeWebApptType.Rows.Count > 0)
                        {
                            bool Type = SynchPracticeWebBAL.Save_ApptType_PracticeWeb_To_Local(dtPracticeWebApptType, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (Type)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                                ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                IsPracticeWebApptTypeSync = true;
                                SynchDataLiveDB_Push_ApptType();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Appointment Type Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                IsPracticeWebApptTypeSync = false;
                            }
                        }
                    }
                    Is_synched_ApptType = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_ApptType = false;
                ObjGoalBase.WriteToErrorLogFile("[Appointment_Type Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Patient

        private void fncSynchDataPracticeWeb_Patient()
        {
            InitBgWorkerPracticeWeb_Patient();
            InitBgTimerPracticeWeb_Patient();
        }

        private void InitBgTimerPracticeWeb_Patient()
        {
            timerSynchPracticeWeb_Patient = new System.Timers.Timer();
            this.timerSynchPracticeWeb_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchPracticeWeb_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_Patient_Tick);
            timerSynchPracticeWeb_Patient.Enabled = true;
            timerSynchPracticeWeb_Patient.Start();
            timerSynchPracticeWeb_Patient_Tick(null, null);
        }

        private void InitBgWorkerPracticeWeb_Patient()
        {
            bwSynchPracticeWeb_Patient = new BackgroundWorker();
            bwSynchPracticeWeb_Patient.WorkerReportsProgress = true;
            bwSynchPracticeWeb_Patient.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_Patient.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_Patient_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWeb_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_Patient_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_Patient.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_Patient();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_Patient()
        {
            System.Threading.Thread procThreadmainPracticeWeb_Patient = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_Patient);
            procThreadmainPracticeWeb_Patient.Start();
        }

        public void CallSyncOrderTablePracticeWeb_Patient()
        {
            if (bwSynchPracticeWeb_Patient.IsBusy != true)
            {
                bwSynchPracticeWeb_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_Patient();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_Patient.Enabled = true;
        }

        public void PracticeWebPatientCommonCode(DataRow dtDtxRow, DataTable dtPracticeWebPatientLastVisit_Date, DataTable dtPracticeWebPatientNextApptDate, DataTable dtPracticeWebPatient, DataTable dtPracticeWebPatientdue_date, DataTable dtLocalPatient, ref string patSEX, ref string MaritalStatus, ref string tmpdue_date, ref string ReceiveSMS, ref string PatientActiveInactive, ref bool _successfullstataus, ref string sqlSelect, ref string due_date, string UsedBenefit, string RemainingBenefit)
        {
            try
            {
                string birthdate = Utility.CheckValidDatetime(dtDtxRow["Birth_Date"].ToString().Trim());
                if (birthdate != "") dtDtxRow["Birth_Date"] = birthdate;

                string firstvisitdate = Utility.CheckValidDatetime(dtDtxRow["FirstVisit_Date"].ToString().Trim());
                if (firstvisitdate != "") dtDtxRow["FirstVisit_Date"] = firstvisitdate;

                string lastvisitdate = Utility.SetLastVisitDate(dtPracticeWebPatientLastVisit_Date, "PatNum", "Patient_EHR_ID", "lastvisit_date", dtDtxRow["Patient_EHR_ID"].ToString());
                if (lastvisitdate != "") dtDtxRow["LastVisit_Date"] = lastvisitdate;

                //https://app.asana.com/0/751059797849097/1149506260330945
                dtDtxRow["nextvisit_date"] = Utility.SetNextVisitDate(dtPracticeWebPatientNextApptDate, "PatNum", "Patient_EHR_ID", "nextvisit_date", dtDtxRow["Patient_EHR_ID"].ToString());

                try
                {
                    patSEX = dtDtxRow["Sex"].ToString().Trim();
                }
                catch (Exception)
                { patSEX = "0"; }

                if (patSEX == "0")
                { dtDtxRow["Sex"] = "Male"; }
                else if (patSEX == "1")
                { dtDtxRow["Sex"] = "Female"; }
                else
                { dtDtxRow["Sex"] = "Unknown"; }

                try
                {
                    MaritalStatus = dtDtxRow["MaritalStatus"].ToString().Trim();
                }
                catch (Exception)
                { MaritalStatus = "0"; }


                switch (MaritalStatus)
                {
                    case "0":
                        dtDtxRow["MaritalStatus"] = "Single";
                        break;
                    case "1":
                        dtDtxRow["MaritalStatus"] = "Married";
                        break;
                    case "2":
                        dtDtxRow["MaritalStatus"] = "Child";
                        break;
                    case "3":
                        dtDtxRow["MaritalStatus"] = "Widowed";
                        break;
                    case "4":
                        dtDtxRow["MaritalStatus"] = "Divorced";
                        break;
                    default:
                        dtDtxRow["MaritalStatus"] = "Single";
                        break;
                }

                try
                {
                    DataRow[] Patdue_date = dtPracticeWebPatientdue_date.Copy().Select("patient_id = '" + dtDtxRow["Patient_EHR_ID"] + "'");

                    tmpdue_date = string.Empty;

                    if (Patdue_date.Length > 0)
                    {
                        if (Patdue_date.Length > 5)
                        {
                            DataTable tmpDatatableduedate = new DataTable();
                            tmpDatatableduedate = Patdue_date.CopyToDataTable();
                            DataView view = tmpDatatableduedate.DefaultView;
                            view.Sort = "due_date desc";
                            DataTable SortPatdue_date = view.ToTable();

                            for (int i = 0; i < 5; i++)
                            {
                                tmpdue_date = SortPatdue_date.Rows[i]["due_date"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type"].ToString() + "@" + SortPatdue_date.Rows[i]["RecallTypeNum"].ToString() + "|" + tmpdue_date;
                            }

                            dtDtxRow["due_date"] = tmpdue_date;
                        }
                        else
                        {
                            foreach (DataRow due_row in Patdue_date)
                            {
                                tmpdue_date = due_row["due_date"].ToString() + "@" + due_row["recall_type"].ToString() + "@" + due_row["RecallTypeNum"].ToString() + "|" + tmpdue_date;
                            }
                            dtDtxRow["due_date"] = tmpdue_date;
                        }
                    }
                }
                catch (Exception)
                {
                    tmpdue_date = "";
                }

                try
                {
                    ReceiveSMS = dtDtxRow["ReceiveSMS"].ToString().Trim();
                }
                catch (Exception)
                { ReceiveSMS = "0"; }

                if (ReceiveSMS == "0" || ReceiveSMS == "" || ReceiveSMS == "1")
                {
                    dtDtxRow["ReceiveSMS"] = "Y";
                }
                else
                {
                    dtDtxRow["ReceiveSMS"] = "N";
                }





                //DataRow[] row = dtLocalPatient.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
                //if (row.Length > 0)
                //{
                //    if (Utility.DateDiffBetweenTwoDate(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim(), row[0]["EHR_Entry_DateTime"].ToString().Trim()))
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (Utility.DateDiffBetweenTwoDate(dtDtxRow["Birth_Date"].ToString().Trim(), row[0]["Birth_Date"].ToString().Trim()))
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (Utility.DateDiffBetweenTwoDate(dtDtxRow["nextvisit_date"].ToString().Trim(), row[0]["NextVisit_date"].ToString().Trim()))
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (Utility.DateDiffBetweenTwoDate(dtDtxRow["LastVisit_Date"].ToString().Trim(), row[0]["LastVisit_Date"].ToString().Trim()))
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (Convert.ToDecimal(dtDtxRow["CurrentBal"].ToString().Trim()) != Convert.ToDecimal(row[0]["CurrentBal"].ToString().Trim()))
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (Convert.ToDecimal(row[0]["used_benefit"].ToString().Trim()) != Convert.ToDecimal(UsedBenefit))
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (Convert.ToDecimal(row[0]["remaining_benefit"].ToString().Trim()) != Convert.ToDecimal(RemainingBenefit))
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (tmpdue_date != row[0]["due_date"].ToString().Trim())
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (dtDtxRow["Primary_Insurance"].ToString().Trim() != row[0]["Primary_Insurance"].ToString().Trim())
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (dtDtxRow["Primary_Insurance_CompanyName"].ToString().ToLower().Trim() != row[0]["Primary_Insurance_CompanyName"].ToString().ToLower().Trim())
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (dtDtxRow["Primary_Ins_Subscriber_ID"].ToString().ToLower().Trim() != row[0]["Primary_Ins_Subscriber_ID"].ToString().ToLower().Trim())
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (dtDtxRow["Secondary_Insurance"].ToString().Trim() != row[0]["Secondary_Insurance"].ToString().Trim())
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (dtDtxRow["Secondary_Insurance_CompanyName"].ToString().ToLower().Trim() != row[0]["Secondary_Insurance_CompanyName"].ToString().ToLower().Trim())
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (dtDtxRow["Secondary_Ins_Subscriber_ID"].ToString().ToLower().Trim() != row[0]["Secondary_Ins_Subscriber_ID"].ToString().ToLower().Trim())
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //    else if (dtDtxRow["Clinic_Number"].ToString().ToLower().Trim() != row[0]["Clinic_Number"].ToString().ToLower().Trim())
                //    {
                //        dtDtxRow["InsUptDlt"] = 2;
                //    }
                //}
                //else
                //{
                //    dtDtxRow["InsUptDlt"] = 1;
                //}

                dtPracticeWebPatient.AcceptChanges();

                if (dtDtxRow["InsUptDlt"].ToString() == "")
                {
                    dtDtxRow["InsUptDlt"] = "0";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SynchDataPracticeWeb_InsertPatient()
        {
            string PatEhrID = "", CliNum = "";
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalInsertPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {
                                    DataTable dtLocalPatient = new DataTable();
                                    try
                                    {
                                        if (dtLocalPatientResult != null && dtLocalPatientResult.Rows.Count > 0)
                                        {
                                            dtLocalPatient = dtLocalPatientResult.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                                        }
                                    }
                                    catch (Exception ex2)
                                    {
                                        Utility.WriteToErrorLogFromAll("[SynchDataPracticeWeb_InsertPatient] Err in New local filter: " + ex2.ToString());
                                    }
                                    if ((dtLocalPatient != null && dtLocalPatient.Rows.Count > 0 && dtLocalPatient.Select("Is_Adit_Updated = 1").Length > 0))
                                    {
                                        DataTable dtPracticeWebPatient = SynchPracticeWebBAL.GetPracticeWebInsertPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                        if ((dtPracticeWebPatient != null && dtPracticeWebPatient.Rows.Count > 0))
                                        {
                                            var itemsToBeAdded = (from OpenDentalPatient in dtPracticeWebPatient.AsEnumerable()
                                                                  join LocalPatient in dtLocalPatient.AsEnumerable()
                                                                  on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                                                  equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                                                  into matchingRows
                                                                  from matchingRow in matchingRows.DefaultIfEmpty()
                                                                  where matchingRow == null
                                                                  select OpenDentalPatient).ToList();
                                            if (itemsToBeAdded.Count > 0)
                                            {
                                                string strPatID = String.Join(",", itemsToBeAdded.AsEnumerable().Select(x => x["Patient_EHR_ID"].ToString()).ToArray());
                                                strPatID = "'" + strPatID.Replace(",", "','") + "'";
                                                if (!string.IsNullOrEmpty(strPatID))
                                                {
                                                    DataTable dtSaveRecords = new DataTable();
                                                    dtSaveRecords = SynchPracticeWebBAL.GetPracticeWebPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), strPatID);

                                                    if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                                                    {
                                                        #region "Update Language"
                                                        DataTable dtLocalOpenDentalLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();
                                                        var updateLanguageQuery = from r1 in dtSaveRecords.AsEnumerable()
                                                                                  join r2 in dtLocalOpenDentalLanguageList.AsEnumerable()
                                                                                  on r1.Field<string>("PreferredLanguage") equals r2.Field<string>("Language_Short_Name")
                                                                                  select new { r1, r2 };
                                                        foreach (var x in updateLanguageQuery)
                                                        {
                                                            x.r1.SetField("PreferredLanguage", x.r2.Field<string>("Language_Name"));
                                                        }
                                                        #endregion

                                                        if (!dtSaveRecords.Columns.Contains("InsUptDlt"))
                                                        {
                                                            dtSaveRecords.Columns.Add("InsUptDlt", typeof(int));
                                                            dtSaveRecords.Columns["InsUptDlt"].DefaultValue = 0;
                                                        }
                                                        if (dtSaveRecords.Rows.Count > 0)
                                                        {
                                                            dtSaveRecords.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
                                                        }

                                                        bool status = false;
                                                        if (dtSaveRecords.Rows.Count > 0)
                                                        {
                                                            status = SynchPracticeWebBAL.Save_Patient_PracticeWeb_To_Local_New(dtSaveRecords, Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                                        }
                                                        else
                                                        {
                                                            status = true;
                                                        }

                                                        if (status)
                                                        {
                                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                                            IsGetParientRecordDone = true;
                                                            ObjGoalBase.WriteToSyncLogFile("[SynchDataPracticeWeb_InsertPatient] Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                                        }
                                                        else
                                                        {
                                                            ObjGoalBase.WriteToSyncLogFile("[SynchDataPracticeWeb_InsertPatient] Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                                            bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                                                            IsGetParientRecordDone = true;
                                                        }

                                                        try
                                                        {
                                                            if (status)
                                                            {
                                                                SynchDataLiveDB_Push_Patient();
                                                            }
                                                        }
                                                        catch (Exception ex1)
                                                        {
                                                            ObjGoalBase.WriteToErrorLogFile("[SynchDataPracticeWeb_InsertPatient]->[Push Patient] Error: " + ex1.Message);
                                                        }
                                                    }

                                                }
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
                ObjGoalBase.WriteToErrorLogFile("[SynchDataPracticeWeb_InsertPatient Sync (" + Utility.Application_Name + " to Local Database) ] PatID:" + PatEhrID + ", Clinic_Number:" + CliNum +
                    System.Environment.NewLine + "Error: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                Is_synched_Patient = false;
                IsParientFirstSync = true;
            }
        }

        public void SynchDataPracticeWeb_Patient()
        {
            string PatEhrID = "", CliNum = "";
            try
            {
                //Utility.IsExternalAppointmentSync = true;


                if (Utility.IsExternalAppointmentSync)
                {
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;
                    Is_synched_AppointmentsPatient = false;
                }
                if (Utility.IsApplicationIdleTimeOff && IsParientFirstSync && !Is_synched_Patient && !Is_synched_AppointmentsPatient)//&& Utility.AditLocationSyncEnable
                {
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        SynchDataLiveDB_Pull_EHR_Patientoptout();
                        DataTable dtPracticeWebPatient = SynchPracticeWebBAL.GetPracticeWebPatientData("", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        string patientTableName = "Patient";

                        DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        DataTable dtLocalPracticeWebLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();

                        var updateLanguageQuery = from r1 in dtPracticeWebPatient.AsEnumerable()
                                                  join r2 in dtLocalPracticeWebLanguageList.AsEnumerable()
                                                  on r1.Field<string>("PreferredLanguage") equals r2.Field<string>("Language_Short_Name")
                                                  select new { r1, r2 };
                        foreach (var x in updateLanguageQuery)
                        {
                            x.r1.SetField("PreferredLanguage", x.r2.Field<string>("Language_Name"));
                        }

                        if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                        {
                            patientTableName = "PatientCompare";
                        }

                        string sqlSelect = string.Empty;
                        #region SqlCeConnection
                        if (!Utility.isSqlServer)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                //CommonDB.LocalConnectionServer(ref conn);
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                if (patientTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                                {
                                    using (SqlCeCommand SqlCeComBulk = new SqlCeCommand(sqlSelect, conn))
                                    {
                                        SqlCeComBulk.CommandText = "Select patient_ehr_id, patient_Web_ID, First_name, Last_name, Middle_Name, Salutation, preferred_name, Status, Sex, MaritalStatus, Birth_Date, Email, Mobile, Home_Phone, Work_Phone, Address1, Address2, City, State, Zipcode, ResponsibleParty_Status, CurrentBal, ThirtyDay, SixtyDay, NinetyDay, Over90, FirstVisit_Date, LastVisit_Date, Primary_Insurance, Primary_Insurance_CompanyName, Primary_Ins_Subscriber_ID, Secondary_Insurance, Secondary_Insurance_CompanyName, Secondary_Ins_Subscriber_ID, Guar_ID, Pri_Provider_ID, Sec_Provider_ID, ReceiveSms, ReceiveEmail, nextvisit_date, due_date, remaining_benefit, used_benefit, collect_payment, EHR_Entry_DateTime, Last_Sync_Date, Is_Adit_Updated,Clinic_Number, " +
                                                                   " Service_Install_Id,is_deleted,[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note], ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber from " + patientTableName;
                                        //SqlCeComBulk.IndexName = "patient_ehr_id";

                                        SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                                        DataTable SqlCeDt = new DataTable();
                                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeComBulk))
                                        {
                                            SqlCeDt = new DataTable();
                                            SqlCeDa.Fill(SqlCeDt);
                                        }
                                        DataColumnCollection columns = dtPracticeWebPatient.Columns;

                                        foreach (DataRow dtDtxRow in dtPracticeWebPatient.Rows)
                                        {
                                            if (dtDtxRow["CurrentBal"].ToString() == "")
                                            {
                                                dtDtxRow["CurrentBal"] = "0";
                                            }
                                            if (dtDtxRow["ThirtyDay"].ToString() == "")
                                            {
                                                dtDtxRow["ThirtyDay"] = "0";
                                            }
                                            if (dtDtxRow["SixtyDay"].ToString() == "")
                                            {
                                                dtDtxRow["SixtyDay"] = "0";
                                            }
                                            if (dtDtxRow["NinetyDay"].ToString() == "")
                                            {
                                                dtDtxRow["NinetyDay"] = "0";
                                            }
                                            if (dtDtxRow["Over90"].ToString() == "")
                                            {
                                                dtDtxRow["Over90"] = "0";
                                            }

                                            object responsiblebirthdte = null;
                                            try
                                            {
                                                responsiblebirthdte = Convert.ToDateTime(dtDtxRow["responsiblepartybirthdate"]);
                                            }
                                            catch (Exception ex)
                                            {
                                                responsiblebirthdte = DBNull.Value;
                                            }

                                            PatEhrID = dtDtxRow["patient_ehr_id"].ToString().Trim();
                                            SqlCeUpdatableRecord rec = rs.CreateRecord();
                                            rec.SetValue(rec.GetOrdinal("patient_ehr_id"), dtDtxRow["patient_ehr_id"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("patient_Web_ID"), "");
                                            rec.SetValue(rec.GetOrdinal("First_name"), dtDtxRow["First_name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Last_name"), dtDtxRow["Last_name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Middle_Name"), dtDtxRow["Middle_Name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Salutation"), dtDtxRow["Salutation"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("preferred_name"), dtDtxRow["preferred_name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Status"), dtDtxRow["Status"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Sex"), dtDtxRow["Sex"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("MaritalStatus"), dtDtxRow["MaritalStatus"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Birth_Date"), Convert.ToString(dtDtxRow["Birth_Date"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("Email"), dtDtxRow["Email"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Mobile"), Utility.ConvertContactNumber(dtDtxRow["Mobile"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("Home_Phone"), Utility.ConvertContactNumber(dtDtxRow["Home_Phone"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("Work_Phone"), Utility.ConvertContactNumber(dtDtxRow["Work_Phone"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("Address1"), dtDtxRow["Address1"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Address2"), dtDtxRow["Address2"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("City"), dtDtxRow["City"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("State"), dtDtxRow["State"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Zipcode"), dtDtxRow["Zipcode"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("ResponsibleParty_Status"), "");
                                            rec.SetValue(rec.GetOrdinal("CurrentBal"), Math.Round(double.Parse(dtDtxRow["CurrentBal"].ToString().Trim()), 2));
                                            rec.SetValue(rec.GetOrdinal("ThirtyDay"), Math.Round(double.Parse(dtDtxRow["ThirtyDay"].ToString().Trim()), 2));
                                            rec.SetValue(rec.GetOrdinal("SixtyDay"), Math.Round(double.Parse(dtDtxRow["SixtyDay"].ToString().Trim()), 2));
                                            rec.SetValue(rec.GetOrdinal("NinetyDay"), Math.Round(double.Parse(dtDtxRow["NinetyDay"].ToString().Trim()), 2));
                                            rec.SetValue(rec.GetOrdinal("Over90"), Math.Round(double.Parse(dtDtxRow["Over90"].ToString().Trim()), 2));
                                            rec.SetValue(rec.GetOrdinal("FirstVisit_Date"), Utility.CheckValidDatetime(dtDtxRow["FirstVisit_Date"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("LastVisit_Date"), Utility.CheckValidDatetime(dtDtxRow["LastVisit_Date"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("Primary_Insurance"), dtDtxRow["Primary_Insurance"].ToString());
                                            rec.SetValue(rec.GetOrdinal("Primary_Insurance_CompanyName"), dtDtxRow["Primary_Insurance_CompanyName"].ToString());
                                            rec.SetValue(rec.GetOrdinal("Primary_Ins_Subscriber_ID"), dtDtxRow["Primary_Ins_Subscriber_ID"].ToString());
                                            rec.SetValue(rec.GetOrdinal("Secondary_Insurance"), dtDtxRow["Secondary_Insurance"].ToString());
                                            rec.SetValue(rec.GetOrdinal("Secondary_Insurance_CompanyName"), dtDtxRow["Secondary_Insurance_CompanyName"].ToString());
                                            rec.SetValue(rec.GetOrdinal("Secondary_Ins_Subscriber_ID"), dtDtxRow["Secondary_Ins_Subscriber_ID"].ToString());
                                            rec.SetValue(rec.GetOrdinal("Guar_ID"), dtDtxRow["Guar_ID"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Pri_Provider_ID"), dtDtxRow["Pri_Provider_ID"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Sec_Provider_ID"), dtDtxRow["Sec_Provider_ID"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("ReceiveSMS"), dtDtxRow["ReceiveSMS"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("ReceiveEmail"), "Y");
                                            rec.SetValue(rec.GetOrdinal("nextvisit_date"), Utility.CheckValidDatetime(dtDtxRow["nextvisit_date"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("due_date"), Convert.ToString(dtDtxRow["due_date"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("collect_payment"), dtDtxRow["collect_payment"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("remaining_benefit"), Math.Round(double.Parse(dtDtxRow["remaining_benefit"].ToString().Trim()), 2));
                                            rec.SetValue(rec.GetOrdinal("used_benefit"), Math.Round(double.Parse(dtDtxRow["used_benefit"].ToString().Trim()), 2));
                                            rec.SetValue(rec.GetOrdinal("EHR_Entry_DateTime"), Utility.CheckValidDatetime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()));
                                            rec.SetValue(rec.GetOrdinal("Last_Sync_Date"), Utility.GetCurrentDatetimestring());
                                            rec.SetValue(rec.GetOrdinal("Is_Adit_Updated"), 0);
                                            rec.SetValue(rec.GetOrdinal("Clinic_Number"), dtDtxRow["Clinic_Number"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Service_Install_Id"), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                            rec.SetValue(rec.GetOrdinal("is_deleted"), Convert.ToBoolean(dtDtxRow["is_deleted"]));
                                            rec.SetValue(rec.GetOrdinal("EHR_Status"), dtDtxRow["EHR_Status"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("ReceiveVoiceCall"), dtDtxRow["ReceiveVoiceCall"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("PreferredLanguage"), dtDtxRow["PreferredLanguage"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Patient_Note"), dtDtxRow["Patient_Note"].ToString().Length > 3000 ? dtDtxRow["Patient_Note"].ToString().Substring(0, 3000).ToString().Trim() : dtDtxRow["Patient_Note"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("ssn"), dtDtxRow["ssn"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("driverlicense"), dtDtxRow["driverlicense"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("groupid"), dtDtxRow["groupid"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("emergencycontactId"), dtDtxRow["emergencycontactId"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("EmergencyContact_First_Name"), dtDtxRow["EmergencyContact_First_Name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("EmergencyContact_Last_Name"), dtDtxRow["EmergencyContact_Last_Name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("emergencycontactnumber"), dtDtxRow["emergencycontactnumber"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("school"), dtDtxRow["school"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("employer"), dtDtxRow["employer"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("spouseId"), dtDtxRow["spouseId"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Spouse_First_Name"), dtDtxRow["Spouse_First_Name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Spouse_Last_Name"), dtDtxRow["Spouse_Last_Name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("responsiblepartyId"), dtDtxRow["responsiblepartyId"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("responsiblepartyssn"), dtDtxRow["responsiblepartyssn"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("responsiblepartybirthdate"), responsiblebirthdte);
                                            rec.SetValue(rec.GetOrdinal("ResponsibleParty_First_Name"), dtDtxRow["ResponsibleParty_First_Name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("ResponsibleParty_Last_Name"), dtDtxRow["ResponsibleParty_Last_Name"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Prim_Ins_Company_Phonenumber"), dtDtxRow["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                                            rec.SetValue(rec.GetOrdinal("Sec_Ins_Company_Phonenumber"), dtDtxRow["Sec_Ins_Company_Phonenumber"].ToString().Trim());

                                            rs.Insert(rec);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (DataRow dtDtxRow in dtPracticeWebPatient.Rows)
                                    {
                                        if (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()) != 0)
                                        {
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand("", conn))
                                            {
                                                SqlCeCommand.CommandType = CommandType.Text;
                                                PracticeWebExecuteQuery(patientTableName, dtDtxRow, SqlCeCommand, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                            }
                                        }
                                    }
                                }
                            }


                            #region Get Records from PatientCompareTAble
                            if (patientTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                            {
                                DataTable dtPatientCompareRec = new DataTable();
                                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                                {
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    string SqlCeSelect = PatientCompareQuery;
                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.CommandType = CommandType.Text;
                                        SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                        SqlCeCommand.Parameters.Add("Clinic_Number", "0");

                                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                                        {
                                            dtPatientCompareRec = new DataTable();
                                            SqlCeDa.Fill(dtPatientCompareRec);
                                        }
                                        foreach (DataRow drRow in dtPatientCompareRec.Rows)
                                        {
                                            PracticeWebExecuteQuery("Patient", drRow, SqlCeCommand, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                        }


                                        IEnumerable<string> PatientEHRIDs = dtPracticeWebPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                                        if (PatientEHRIDs != null && PatientEHRIDs.Any())
                                        {
                                            IEnumerable<string> LocalEHRIDs = dtLocalPatient.AsEnumerable()
                                                .Where(sid => sid["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString())
                                                .Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                                            if (LocalEHRIDs != null && LocalEHRIDs.Any())
                                            {
                                                string DeletedEHRIDs = string.Join("','", LocalEHRIDs.Except(PatientEHRIDs).ToList());

                                                DataTable dtCompareDeleted = SynchLocalBAL.GetLocalPatientCompareDeletedData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                                IEnumerable<string> CompareDeletedData = dtCompareDeleted.AsEnumerable()
                                                .Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
                                                string CompareDeletedIDs = string.Join("','", CompareDeletedData.ToList());
                                                if (CompareDeletedIDs != string.Empty)
                                                {
                                                    //CompareDeletedIDs = "'" + CompareDeletedIDs + "'";
                                                    DeletedEHRIDs = DeletedEHRIDs + (DeletedEHRIDs != string.Empty ? "','" : "") + CompareDeletedIDs;
                                                }


                                                if (DeletedEHRIDs != string.Empty)
                                                {
                                                    DeletedEHRIDs = "'" + DeletedEHRIDs + "'";
                                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                                    SqlCeSelect = "Update Patient Set is_deleted = 1, Is_Adit_Updated = 0, Status = 'I',EHR_Status='Deleted'  Where Patient_EHR_ID In (@PatientEHRIDs) and is_deleted = 0";
                                                    SqlCeSelect = SqlCeSelect.Replace("@PatientEHRIDs", DeletedEHRIDs);
                                                    SqlCeCommand.CommandText = SqlCeSelect;
                                                    SqlCeCommand.ExecuteNonQuery();
                                                }
                                            }
                                        }


                                        //SqlCeCommand.Parameters.Clear();
                                        //SqlCeCommand.CommandText = "Update Patient set  [Status] = 'I',is_deleted = 1, Is_Adit_Updated = 0 where Patient_EHR_id Not in (Select Patient_EHR_id from PatientCompare where Service_Install_Id = @Service_Install_Id) and Service_Install_Id = @Service_Install_Id  ";
                                        //SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                        //SqlCeCommand.ExecuteNonQuery();

                                        //SqlCeCommand.Parameters.Clear();
                                        //SqlCeCommand.CommandText = "Update Patient set  [Status] = 'I', is_deleted = 1, Is_Adit_Updated = 0 where Patient_EHR_id in (Select Patient_EHR_id from PatientCompare where Service_Install_Id = @Service_Install_Id and Is_Deleted = 1) and Service_Install_Id = @Service_Install_Id  ";
                                        //SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                        //SqlCeCommand.ExecuteNonQuery();


                                        //if (conn.State == ConnectionState.Closed) conn.Open();
                                        //CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = @Service_Install_Id  ";
                                        SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            #endregion

                        }
                        #endregion

                        #region SqlExpressConnection
                        //if (Utility.isSqlServer)
                        //{
                        //    SqlConnection SqlConn = null;
                        //    SqlCommand sqlExpressCommand = null;
                        //    CommonDB.LocalConnectionServer_SqlServer(ref SqlConn);
                        //    if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
                        //    // SqlTransaction SqlExpressTransaction = SqlConn.BeginTransaction("");
                        //    CommonDB.SqlServerCommand(sqlSelect, SqlConn, ref sqlExpressCommand, "txt");
                        //    //  sqlExpressCommand.Transaction = SqlExpressTransaction;

                        //    try
                        //    {
                        //        foreach (DataRow dtDtxRow in dtPracticeWebPatient.Rows)
                        //        {
                        //            GetPatientRecord = GetPatientRecord + 1;

                        //            PatientCommonCode(dtDtxRow, dtPracticeWebPatientLastVisit_Date, dtPracticeWebPatientLastVisit_Date, dtPracticeWebPatient, dtPracticeWebPatientdue_date, dtLocalPatient, ref  patSEX, ref  MaritalStatus, ref  tmpdue_date, ref  ReceiveSMS, ref  PatientActiveInactive, ref  _successfullstataus, ref  sqlSelect, ref  due_date);

                        //            if (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()) != 0)
                        //            {
                        //                var PatientWisePendingAmount = dtPracticeWebPatientWisePendingAmount.AsEnumerable().Where(o => Convert.ToInt64(o.Field<object>("PatNum")) == Convert.ToInt64(dtDtxRow["Patient_EHR_ID"]));

                        //                switch (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()))
                        //                {
                        //                    case 1:
                        //                        sqlExpressCommand.CommandText = Insert_Patient;
                        //                        break;
                        //                    case 2:
                        //                        sqlExpressCommand.CommandText = Update_Patient;
                        //                        break;
                        //                    case 3:
                        //                        sqlExpressCommand.CommandText = Delete_Patient;
                        //                        break;
                        //                }

                        //                if (dtDtxRow["CurrentBal"].ToString() == "")
                        //                {
                        //                    dtDtxRow["CurrentBal"] = "0";
                        //                }
                        //                if (dtDtxRow["ThirtyDay"].ToString() == "")
                        //                {
                        //                    dtDtxRow["ThirtyDay"] = "0";
                        //                }
                        //                if (dtDtxRow["SixtyDay"].ToString() == "")
                        //                {
                        //                    dtDtxRow["SixtyDay"] = "0";
                        //                }
                        //                if (dtDtxRow["NinetyDay"].ToString() == "")
                        //                {
                        //                    dtDtxRow["NinetyDay"] = "0";
                        //                }
                        //                if (dtDtxRow["Over90"].ToString() == "")
                        //                {
                        //                    dtDtxRow["Over90"] = "0";
                        //                }

                        //                #region SqlExpressConnection
                        //                if (Utility.isSqlServer)
                        //                {
                        //                    sqlExpressCommand.Parameters.Clear();
                        //                    sqlExpressCommand.Parameters.AddWithValue("patient_ehr_id", dtDtxRow["patient_ehr_id"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("patient_Web_ID", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("First_name", dtDtxRow["First_name"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Last_name", dtDtxRow["Last_name"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Middle_Name", dtDtxRow["Middle_Name"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Salutation", dtDtxRow["Salutation"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("preferred_name", dtDtxRow["preferred_name"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Status", PatientActiveInactive);
                        //                    sqlExpressCommand.Parameters.AddWithValue("Sex", dtDtxRow["tmpSex"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("MaritalStatus", dtDtxRow["tmpMaritalStatus"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Birth_Date", Convert.ToString(dtDtxRow["tmpBirth_Date"].ToString().Trim()));
                        //                    sqlExpressCommand.Parameters.AddWithValue("Email", dtDtxRow["Email"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dtDtxRow["Mobile"].ToString().Trim()));
                        //                    sqlExpressCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dtDtxRow["Home_Phone"].ToString().Trim()));
                        //                    sqlExpressCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dtDtxRow["Work_Phone"].ToString().Trim()));
                        //                    sqlExpressCommand.Parameters.AddWithValue("Address1", dtDtxRow["Address1"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Address2", dtDtxRow["Address2"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("City", dtDtxRow["City"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("State", dtDtxRow["State"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Zipcode", dtDtxRow["Zipcode"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("CurrentBal", Math.Round(double.Parse(dtDtxRow["ThirtyDay"].ToString().Trim()), 2));
                        //                    sqlExpressCommand.Parameters.AddWithValue("ThirtyDay", Math.Round(double.Parse(dtDtxRow["SixtyDay"].ToString().Trim()), 2));
                        //                    sqlExpressCommand.Parameters.AddWithValue("SixtyDay", Math.Round(double.Parse(dtDtxRow["NinetyDay"].ToString().Trim()), 2));
                        //                    sqlExpressCommand.Parameters.AddWithValue("NinetyDay", Math.Round(double.Parse(dtDtxRow["Over90"].ToString().Trim()), 2));
                        //                    sqlExpressCommand.Parameters.AddWithValue("Over90", 0);
                        //                    sqlExpressCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dtDtxRow["tmpFirstVisit_Date"].ToString().Trim()));
                        //                    sqlExpressCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dtDtxRow["tmpLastVisit_Date"].ToString().Trim()));
                        //                    sqlExpressCommand.Parameters.AddWithValue("Primary_Insurance", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("Secondary_Insurance", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("Guar_ID", dtDtxRow["Guar_ID"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Pri_Provider_ID", dtDtxRow["Pri_Provider_ID"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Sec_Provider_ID", dtDtxRow["Sec_Provider_ID"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("ReceiveSMS", dtDtxRow["tmpReceiveSMS"].ToString().Trim());
                        //                    sqlExpressCommand.Parameters.AddWithValue("ReceiveEmail", "Y");
                        //                    sqlExpressCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dtDtxRow["tmpnextvisit_date"].ToString().Trim()));
                        //                    sqlExpressCommand.Parameters.AddWithValue("due_date", tmpdue_date);
                        //                    //sqlExpressCommand.Parameters.AddWithValue("remaining_benefit", PatientWisePendingAmount.Count() > 0 ? PatientWisePendingAmount.First().Field<object>("Remaining_Benefit").ToString() : "0");
                        //                    //sqlExpressCommand.Parameters.AddWithValue("collect_payment", PatientWisePendingAmount.Count() > 0 ? PatientWisePendingAmount.First().Field<object>("Current_Payment").ToString() : "0");
                        //                    sqlExpressCommand.Parameters.AddWithValue("remaining_benefit", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("used_benefit", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("collect_payment", "");
                        //                    sqlExpressCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.CheckValidDatetime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()));
                        //                    sqlExpressCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                        //                    sqlExpressCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                        //                    sqlExpressCommand.ExecuteNonQuery();
                        //                }
                        //                #endregion
                        //            }
                        //        }
                        //        //  SqlExpressTransaction.Commit();
                        //    }
                        //    catch (Exception)
                        //    {
                        //        //  SqlExpressTransaction.Rollback();
                        //    }
                        //}
                        #endregion

                        dtPracticeWebPatient.AcceptChanges();

                        if (dtPracticeWebPatient != null && dtPracticeWebPatient.Rows.Count > 0)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            IsGetParientRecordDone = true;
                            ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");


                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                            IsGetParientRecordDone = true;
                        }
                    }
                    IsPatientSyncedFirstTime = true;
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;

                    try
                    {

                        SynchDataLiveDB_Push_Patient();
                        SynchDataPracticeWeb_PatientStatus();

                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex1.Message);
                    }
                    try
                    {

                        SynchDataPracticeWeb_PatientImages();
                        SynchDataPracticeWeb_PatientDisease();
                        SynchDataPracticeWeb_PatientMedication("");

                    }
                    catch (Exception ex2)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientImages Sync (" + Utility.Application_Name + " to Local Database) ]" + ex2.Message);
                    }

                }
                else if (Is_synched_AppointmentsPatient)
                {
                    Is_Synched_PatientCallHit = true;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                Is_synched_Patient = false;
                IsParientFirstSync = true;
            }
        }

        public void SynchDataPracticeWeb_PatientStatus()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebPatientStatus = SynchPracticeWebBAL.GetPracticeWebPatientStatusData("", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        if (dtPracticeWebPatientStatus != null && dtPracticeWebPatientStatus.Rows.Count > 0)
                        {
                            SynchLocalBAL.UpdatePatient_Status(dtPracticeWebPatientStatus, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        public void SynchDataPracticeWeb_AppointmentPatient()
        {
            try
            {
                if (Utility.IsExternalAppointmentSync)
                {
                    Is_synched_Patient = false;
                    Is_synched_AppointmentsPatient = false;
                }

                if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient && !Is_synched_AppointmentsPatient)//&& Utility.AditLocationSyncEnable
                {
                    Is_synched_Patient = true;
                    Is_synched_AppointmentsPatient = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebAppointmensPatientNextApptDate = SynchPracticeWebBAL.GetPracticeWebPatientNextApptDate(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        DataTable dtPracticeWebAppointmensPatientLastVisit_Date = SynchPracticeWebBAL.GetPracticeWebPatientLastVisit_Date(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        DataTable dtPracticeWebAppointmensPatient = SynchPracticeWebBAL.GetPracticeWebAppointmentsPatientData("", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        DataTable dtPracticeWebAppointmensPatientWisePendingAmount = SynchPracticeWebBAL.GetPatientWisePendingAmount(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        dtPracticeWebAppointmensPatient.Columns.Add("Primary_Insurance", typeof(string));
                        dtPracticeWebAppointmensPatient.Columns.Add("Primary_Insurance_CompanyName", typeof(string));
                        dtPracticeWebAppointmensPatient.Columns.Add("Primary_Ins_Subscriber_ID", typeof(string));
                        dtPracticeWebAppointmensPatient.Columns.Add("Secondary_Insurance", typeof(string));
                        dtPracticeWebAppointmensPatient.Columns.Add("Secondary_Insurance_CompanyName", typeof(string));
                        dtPracticeWebAppointmensPatient.Columns.Add("Secondary_Ins_Subscriber_ID", typeof(string));

                        DataTable dtPracticeWebAppointmensPatientdue_date = SynchPracticeWebBAL.GetPracticeWebPatientdue_date(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        string patientTableName = "Patient";

                        string PatientEHRIDs = string.Join("','", dtPracticeWebAppointmensPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                        if (PatientEHRIDs != string.Empty)
                        {



                            PatientEHRIDs = "'" + PatientEHRIDs + "'";

                            DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                            {
                                patientTableName = "PatientCompare";
                            }


                            string patSEX = string.Empty;
                            string MaritalStatus = string.Empty;
                            string tmpdue_date = string.Empty;
                            string ReceiveSMS = string.Empty;
                            string PatientActiveInactive = "I";
                            bool _successfullstataus = true;
                            string sqlSelect = string.Empty;
                            string due_date = string.Empty;

                            TotalPatientRecord = dtPracticeWebAppointmensPatient.Rows.Count;
                            GetPatientRecord = 0;

                            #region SqlCeConnection
                            if (!Utility.isSqlServer)
                            {
                                string UsedBenafit = "0";
                                string RemainingBenafit = "0";
                                DataTable dtLocalPracticeWebLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();
                                DataTable dtPracticeWebInsuranceDataAll = SynchPracticeWebBAL.GetPracticeWebPatientInsuranceData("0", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                DataTable dtBenafitAll = SynchPracticeWebBAL.GetPracticeWebPatientInsBenafit("0", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                DataTable dtPracticeWebInsuranceData = new DataTable();
                                DataTable dtBenafit = new DataTable();
                                DataRow[] myrow = null;
                                DataRow[] drBenafit = null;
                                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                                {
                                    foreach (DataRow dtDtxRow in dtPracticeWebAppointmensPatient.Rows)
                                    {
                                        // CommonDB.LocalConnectionServer(ref conn);
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        GetPatientRecord = GetPatientRecord + 1;

                                        myrow = dtPracticeWebInsuranceDataAll.Select("Patient_EHR_Id = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
                                        dtPracticeWebInsuranceData.Rows.Clear();
                                        if (myrow != null && myrow.Count() > 0)
                                        {
                                            dtPracticeWebInsuranceData = myrow.CopyToDataTable();
                                        }

                                        drBenafit = dtBenafitAll.Select("Patient_EHR_Id = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
                                        dtBenafit.Rows.Clear();
                                        if (drBenafit != null && drBenafit.Count() > 0)
                                        {
                                            dtBenafit = drBenafit.CopyToDataTable();
                                        }
                                        //dtBenafit = dtBenafitAll.Select("Patient_EHR_Id = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'").CopyToDataTable();
                                        UsedBenafit = "0";
                                        RemainingBenafit = "0";
                                        if (dtBenafit.Rows.Count > 0)
                                        {
                                            UsedBenafit = Convert.ToDouble(dtBenafit.Rows[0]["UsedBenafit"].ToString()).ToString();
                                            RemainingBenafit = (Convert.ToDouble(dtBenafit.Rows[0]["TotalBenafit"].ToString()) - Convert.ToDouble(dtBenafit.Rows[0]["UsedBenafit"].ToString())).ToString();
                                            if (Convert.ToDouble(RemainingBenafit) < 0)
                                            {
                                                RemainingBenafit = "0";
                                            }
                                        }
                                        dtDtxRow["used_benefit"] = UsedBenafit;
                                        dtDtxRow["remaining_benefit"] = RemainingBenafit;


                                        DataRow drLanguage = dtLocalPracticeWebLanguageList.Copy().Select("Language_Short_Name = '" + dtDtxRow["PreferredLanguage"].ToString() + "'").FirstOrDefault();
                                        if (drLanguage != null)
                                        {
                                            dtDtxRow["PreferredLanguage"] = drLanguage["Language_Name"].ToString();
                                        }
                                        else
                                        {
                                            dtDtxRow["PreferredLanguage"] = "English";
                                        }
                                        dtDtxRow["Primary_Insurance"] = 0;
                                        dtDtxRow["Primary_Insurance_CompanyName"] = "";
                                        dtDtxRow["Primary_Ins_Subscriber_ID"] = "";
                                        dtDtxRow["Secondary_Insurance"] = 0;
                                        dtDtxRow["Secondary_Insurance_CompanyName"] = "";
                                        dtDtxRow["Secondary_Ins_Subscriber_ID"] = "";
                                        dtDtxRow["groupid"] = "";
                                        dtDtxRow["Prim_Ins_Company_Phonenumber"] = "";
                                        dtDtxRow["Sec_Ins_Company_Phonenumber"] = "";

                                        if (dtPracticeWebInsuranceData.Rows.Count == 1)
                                        {
                                            dtDtxRow["Primary_Insurance"] = dtPracticeWebInsuranceData.Rows[0]["Primary_Insurance"].ToString();
                                            dtDtxRow["Primary_Insurance_CompanyName"] = dtPracticeWebInsuranceData.Rows[0]["Primary_Insurance_CompanyName"].ToString();
                                            dtDtxRow["Primary_Ins_Subscriber_ID"] = dtPracticeWebInsuranceData.Rows[0]["SubscriberID"].ToString();
                                            dtDtxRow["Prim_Ins_Company_Phonenumber"] = dtPracticeWebInsuranceData.Rows[0]["Prim_Ins_Company_Phonenumber"].ToString();
                                            dtDtxRow["groupid"] = dtPracticeWebInsuranceData.Rows[0]["groupid"].ToString();
                                        }
                                        else if (dtPracticeWebInsuranceData.Rows.Count >= 2)
                                        {
                                            dtDtxRow["Primary_Insurance"] = dtPracticeWebInsuranceData.Rows[0]["Primary_Insurance"].ToString();
                                            dtDtxRow["Primary_Insurance_CompanyName"] = dtPracticeWebInsuranceData.Rows[0]["Primary_Insurance_CompanyName"].ToString();
                                            dtDtxRow["Primary_Ins_Subscriber_ID"] = dtPracticeWebInsuranceData.Rows[0]["SubscriberID"].ToString();
                                            dtDtxRow["Secondary_Insurance"] = dtPracticeWebInsuranceData.Rows[1]["Primary_Insurance"].ToString();
                                            dtDtxRow["Secondary_Insurance_CompanyName"] = dtPracticeWebInsuranceData.Rows[1]["Primary_Insurance_CompanyName"].ToString();
                                            dtDtxRow["Secondary_Ins_Subscriber_ID"] = dtPracticeWebInsuranceData.Rows[1]["SubscriberID"].ToString();
                                            dtDtxRow["Prim_Ins_Company_Phonenumber"] = dtPracticeWebInsuranceData.Rows[0]["Prim_Ins_Company_Phonenumber"].ToString();
                                            dtDtxRow["groupid"] = dtPracticeWebInsuranceData.Rows[0]["groupid"].ToString();
                                            dtDtxRow["Sec_Ins_Company_Phonenumber"] = dtPracticeWebInsuranceData.Rows[1]["Prim_Ins_Company_Phonenumber"].ToString();
                                        }
                                        if (dtDtxRow["Primary_Insurance_CompanyName"].ToString() == "" && dtDtxRow["Secondary_Insurance_CompanyName"].ToString() == "")
                                        {
                                            decimal curPatientcollect_payment = 0;
                                            dtDtxRow["used_benefit"] = curPatientcollect_payment.ToString();
                                            dtDtxRow["remaining_benefit"] = curPatientcollect_payment.ToString();
                                        }
                                        PatientCommonCode(dtDtxRow, dtPracticeWebAppointmensPatientLastVisit_Date, dtPracticeWebAppointmensPatientNextApptDate, dtPracticeWebAppointmensPatient, dtPracticeWebAppointmensPatientdue_date, dtLocalPatient, ref patSEX, ref MaritalStatus, ref tmpdue_date, ref ReceiveSMS, ref PatientActiveInactive, ref _successfullstataus, ref sqlSelect, ref due_date, UsedBenafit, RemainingBenafit);

                                        if (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()) != 0)
                                        {
                                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                                            {
                                                SqlCeCommand.CommandType = CommandType.Text;
                                                var PatientWisePendingAmount = dtPracticeWebAppointmensPatientWisePendingAmount.AsEnumerable().Where(o => Convert.ToInt64(o.Field<object>("PatNum")) == Convert.ToInt64(dtDtxRow["Patient_EHR_ID"]));
                                                SqlCeCommand.Parameters.AddWithValue("collect_payment", PatientWisePendingAmount.Count() > 0 ? PatientWisePendingAmount.First().Field<object>("Current_Payment").ToString() : "0");
                                                ExecuteQuery(patientTableName, dtDtxRow, SqlCeCommand, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                            }
                                        }
                                    }
                                }
                                #region Get Records from PatientCompareTAble
                                if (patientTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                                {
                                    DataTable dtPatientCompareRec = new DataTable();
                                    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                                    {
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        string SqlCeSelect = " SELECT ACC.* FROM ( SELECT (CASE WHEN PT.Patient_EHR_id IS NULL THEN 1 ELSE 2 END) AS InsUptDlt, PC.* FROM (SELECT Distinct Service_Install_Id,[Patient_EHR_ID],Clinic_Number "
                                          + "  FROM (SELECT DISTINCT Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                          + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                          + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note] ,"
                                          + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber,Clinic_Number "
                                          + "  FROM Patient where Service_Install_Id = @Service_Install_Id UNION All  "
                                          + "  SELECT DISTINCT Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                          + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                          + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note],  "
                                          + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber,Clinic_Number "
                                          + "   FROM PatientCompare where Service_Install_Id = @Service_Install_Id) data "
                                          + "  GROUP BY  Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                          + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                          + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note],  "
                                          + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber,Clinic_Number "
                                          + "  HAVING count(1) = 1 ) AS RS "
                                          + "  LEFT JOIN Patient PT ON Rs.Service_Install_Id = PT.Service_Install_Id And RS.Patient_EHR_Id = PT.Patient_EHR_Id And RS.Clinic_Number = PT.Clinic_Number"
                                          + "  LEFT JOIN PatientCompare PC ON Pc.Service_Install_Id = Rs.Service_Install_Id And PC.Patient_EHR_Id = RS.Patient_EHR_Id and PC.Clinic_Number = RS.Clinic_Number) AS ACC WHERE Patient_EHR_id != '' And Service_Install_Id = @Service_Install_Id";

                                        //string SqlCeSelect = PatientCompareQuery;
                                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                        {
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.CommandType = CommandType.Text;
                                            SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                            DataTable SqlCeDt = null;
                                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                                            {
                                                dtPatientCompareRec = new DataTable();
                                                SqlCeDa.Fill(dtPatientCompareRec);
                                            }
                                            foreach (DataRow drRow in dtPatientCompareRec.Rows)
                                            {
                                                ExecuteQuery("Patient", drRow, SqlCeCommand, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                            }

                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.CommandText = "Update Patient set  [Status] = 'I', is_deleted = 1, Is_Adit_Updated = 0 where Patient_EHR_id in (Select Patient_EHR_id from PatientCompare where Service_Install_Id = @Service_Install_Id and Is_Deleted = 1) and Service_Install_Id = @Service_Install_Id  ";
                                            SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                            SqlCeCommand.ExecuteNonQuery();


                                            //if (conn.State == ConnectionState.Closed) conn.Open();
                                            //CommonDB.SqlCeCommandServer(sqlSelect, conn, ref SqlCeCommand, "txt");
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = @Service_Install_Id  ";
                                            SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                            SqlCeCommand.ExecuteNonQuery();
                                        }
                                    }
                                }
                                #endregion

                            }
                            #endregion

                            dtPracticeWebAppointmensPatient.AcceptChanges();

                            if (dtPracticeWebAppointmensPatient != null && dtPracticeWebAppointmensPatient.Rows.Count > 0)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                IsGetParientRecordDone = true;
                                ObjGoalBase.WriteToSyncLogFile("Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                                SynchDataLiveDB_Push_Patient();
                            }
                            else
                            {
                                ObjGoalBase.WriteToSyncLogFile("Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                                IsGetParientRecordDone = true;
                            }

                            Is_synched_AppointmentsPatient = false;
                            //  SynchDataPracticeWeb_PatientDisease();
                            Is_synched_Patient = false;
                            if (Is_Synched_PatientCallHit)
                            {
                                //Is_Synched_PatientCallHit = false;
                                //SynchDataPracticeWeb_Patient();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Is_synched_AppointmentsPatient = false;
                ObjGoalBase.WriteToErrorLogFile("[Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public static void PracticeWebExecuteQuery(string InsertTableName, DataRow dtDtxRow, SqlCeCommand SqlCeCommand, string Service_Install_Id)
        {
            try
            {
                string sqlSelect = string.Empty;
                string MaritalStatus = string.Empty;
                string Status = string.Empty;
                string tmpBirthDate = string.Empty;
                // decimal curPatientcollect_payment = 0;
                string tmpReceive_Sms_Email = string.Empty;

                switch (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()))
                {
                    case 1:
                        SqlCeCommand.CommandText = Insert_Patient;
                        if (InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE")
                        {
                            SqlCeCommand.CommandText = SqlCeCommand.CommandText.Replace("INSERT INTO Patient", "INSERT INTO PatientCompare");
                        }
                        break;
                    case 2:


                        /*  if ( dtDtxRow["EHR_Status"].ToString().ToLower()=="deleted" && dtDtxRow["is_deleted"].ToString().ToLower() == "true")
                          {
                              SqlCeCommand.CommandText = Update_PatientMultiClinic; 
                          }
                          else 
                          {
                              SqlCeCommand.CommandText = Update_Patient;
                          }*/
                        //if (dtDtxRow["is_deleted"].ToString().ToLower() == "false") //dtDtxRow["EHR_Status"].ToString().ToLower() == "active" &&
                        //{
                        SqlCeCommand.CommandText = Update_Patient;
                        //}
                        //else
                        //{
                        //    SqlCeCommand.CommandText = "";
                        //}

                        break;
                    case 3:
                        SqlCeCommand.CommandText = Delete_Patient;
                        break;
                }

                if (dtDtxRow["CurrentBal"].ToString() == "")
                {
                    dtDtxRow["CurrentBal"] = "0";
                }
                if (dtDtxRow["ThirtyDay"].ToString() == "")
                {
                    dtDtxRow["ThirtyDay"] = "0";
                }
                if (dtDtxRow["SixtyDay"].ToString() == "")
                {
                    dtDtxRow["SixtyDay"] = "0";
                }
                if (dtDtxRow["NinetyDay"].ToString() == "")
                {
                    dtDtxRow["NinetyDay"] = "0";
                }
                if (dtDtxRow["Over90"].ToString() == "")
                {
                    dtDtxRow["Over90"] = "0";
                }

                object responsiblebirthdte = null;
                try
                {
                    responsiblebirthdte = Convert.ToDateTime(dtDtxRow["responsiblepartybirthdate"]);
                }
                catch (Exception ex)
                {
                    responsiblebirthdte = DBNull.Value;
                }

                #region SqlCEConnection
                if (SqlCeCommand.CommandText == Insert_Patient || InsertTableName.ToString().ToUpper() == "PATIENTCOMPARE" || SqlCeCommand.CommandText == Update_Patient || SqlCeCommand.CommandText == Delete_Patient)
                {

                    SqlCeCommand.Parameters.Clear();
                    SqlCeCommand.Parameters.AddWithValue("patient_ehr_id", dtDtxRow["patient_ehr_id"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("patient_Web_ID", "");
                    SqlCeCommand.Parameters.AddWithValue("First_name", dtDtxRow["First_name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Last_name", dtDtxRow["Last_name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Middle_Name", dtDtxRow["Middle_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Salutation", dtDtxRow["Salutation"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("preferred_name", dtDtxRow["preferred_name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Status", dtDtxRow["Status"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Sex", dtDtxRow["Sex"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("MaritalStatus", dtDtxRow["MaritalStatus"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Birth_Date", Convert.ToString(dtDtxRow["Birth_Date"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Email", dtDtxRow["Email"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Mobile", Utility.ConvertContactNumber(dtDtxRow["Mobile"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Home_Phone", Utility.ConvertContactNumber(dtDtxRow["Home_Phone"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Work_Phone", Utility.ConvertContactNumber(dtDtxRow["Work_Phone"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Address1", dtDtxRow["Address1"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Address2", dtDtxRow["Address2"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("City", dtDtxRow["City"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("State", dtDtxRow["State"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Zipcode", dtDtxRow["Zipcode"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Status", "");
                    SqlCeCommand.Parameters.AddWithValue("CurrentBal", Math.Round(double.Parse(dtDtxRow["CurrentBal"].ToString().Trim()), 2));
                    SqlCeCommand.Parameters.AddWithValue("ThirtyDay", Math.Round(double.Parse(dtDtxRow["ThirtyDay"].ToString().Trim()), 2));
                    SqlCeCommand.Parameters.AddWithValue("SixtyDay", Math.Round(double.Parse(dtDtxRow["SixtyDay"].ToString().Trim()), 2));
                    SqlCeCommand.Parameters.AddWithValue("NinetyDay", Math.Round(double.Parse(dtDtxRow["NinetyDay"].ToString().Trim()), 2));
                    SqlCeCommand.Parameters.AddWithValue("Over90", Math.Round(double.Parse(dtDtxRow["Over90"].ToString().Trim()), 2));
                    SqlCeCommand.Parameters.AddWithValue("FirstVisit_Date", Utility.CheckValidDatetime(dtDtxRow["FirstVisit_Date"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("LastVisit_Date", Utility.CheckValidDatetime(dtDtxRow["LastVisit_Date"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Primary_Insurance", dtDtxRow["Primary_Insurance"].ToString());
                    SqlCeCommand.Parameters.AddWithValue("Primary_Insurance_CompanyName", dtDtxRow["Primary_Insurance_CompanyName"].ToString());
                    SqlCeCommand.Parameters.AddWithValue("Primary_Ins_Subscriber_ID", dtDtxRow["Primary_Ins_Subscriber_ID"].ToString());
                    SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance", dtDtxRow["Secondary_Insurance"].ToString());
                    SqlCeCommand.Parameters.AddWithValue("Secondary_Insurance_CompanyName", dtDtxRow["Secondary_Insurance_CompanyName"].ToString());
                    SqlCeCommand.Parameters.AddWithValue("Secondary_Ins_Subscriber_ID", dtDtxRow["Secondary_Ins_Subscriber_ID"].ToString());
                    SqlCeCommand.Parameters.AddWithValue("Guar_ID", dtDtxRow["Guar_ID"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Pri_Provider_ID", dtDtxRow["Pri_Provider_ID"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Sec_Provider_ID", dtDtxRow["Sec_Provider_ID"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ReceiveSMS", dtDtxRow["ReceiveSMS"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ReceiveEmail", "Y");
                    SqlCeCommand.Parameters.AddWithValue("nextvisit_date", Utility.CheckValidDatetime(dtDtxRow["nextvisit_date"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("due_date", Convert.ToString(dtDtxRow["due_date"].ToString().Trim()));
                    //SqlCeCommand.Parameters.AddWithValue("remaining_benefit", PatientWisePendingAmount.Count() > 0 ? PatientWisePendingAmount.First().Field<object>("Remaining_Benefit").ToString() : "0");
                    SqlCeCommand.Parameters.AddWithValue("collect_payment", dtDtxRow["collect_payment"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("remaining_benefit", Math.Round(double.Parse(dtDtxRow["remaining_benefit"].ToString().Trim()), 2));
                    SqlCeCommand.Parameters.AddWithValue("used_benefit", Math.Round(double.Parse(dtDtxRow["used_benefit"].ToString().Trim()), 2));
                    SqlCeCommand.Parameters.AddWithValue("EHR_Entry_DateTime", Utility.CheckValidDatetime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()));
                    SqlCeCommand.Parameters.AddWithValue("Last_Sync_Date", Utility.GetCurrentDatetimestring());
                    SqlCeCommand.Parameters.AddWithValue("Is_Adit_Updated", 0);
                    SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dtDtxRow["Clinic_Number"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    SqlCeCommand.Parameters.AddWithValue("is_deleted", Convert.ToBoolean(dtDtxRow["is_deleted"]));
                    SqlCeCommand.Parameters.AddWithValue("EHR_Status", dtDtxRow["EHR_Status"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ReceiveVoiceCall", dtDtxRow["ReceiveVoiceCall"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("PreferredLanguage", dtDtxRow["PreferredLanguage"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Patient_Note", dtDtxRow["Patient_Note"].ToString().Length > 3000 ? dtDtxRow["Patient_Note"].ToString().Substring(0, 3000).ToString().Trim() : dtDtxRow["Patient_Note"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ssn", dtDtxRow["ssn"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("driverlicense", dtDtxRow["driverlicense"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("groupid", dtDtxRow["groupid"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("emergencycontactId", dtDtxRow["emergencycontactId"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("EmergencyContact_First_Name", dtDtxRow["EmergencyContact_First_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("EmergencyContact_Last_Name", dtDtxRow["EmergencyContact_Last_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("emergencycontactnumber", dtDtxRow["emergencycontactnumber"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("school", dtDtxRow["school"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("employer", dtDtxRow["employer"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("spouseId", dtDtxRow["spouseId"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Spouse_First_Name", dtDtxRow["Spouse_First_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Spouse_Last_Name", dtDtxRow["Spouse_Last_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("responsiblepartyId", dtDtxRow["responsiblepartyId"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("responsiblepartyssn", dtDtxRow["responsiblepartyssn"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("responsiblepartybirthdate", responsiblebirthdte);
                    SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_First_Name", dtDtxRow["ResponsibleParty_First_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("ResponsibleParty_Last_Name", dtDtxRow["ResponsibleParty_Last_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Prim_Ins_Company_Phonenumber", dtDtxRow["Prim_Ins_Company_Phonenumber"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Sec_Ins_Company_Phonenumber", dtDtxRow["Sec_Ins_Company_Phonenumber"].ToString().Trim());
                    SqlCeCommand.ExecuteNonQuery();
                }

                #endregion

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("unique_Patient_EHR_ID"))
                {
                }
                else
                {
                    throw;
                }
            }
        }

        public void SynchDataPracticeWeb_PatientDisease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientDisease)
                {
                    Is_synched_PatientDisease = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebDisease = SynchPracticeWebBAL.GetPracticeWebPatientDiseaseData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtPracticeWebDisease.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalDisease = SynchLocalBAL.GetLocalPatientDiseaseData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebDisease.Rows)
                        {
                            DataRow[] row = dtLocalDisease.Copy().Select("Disease_EHR_ID = '" + dtDtxRow["Disease_EHR_ID"].ToString() + "' AND Disease_Type = '" + dtDtxRow["Disease_Type"].ToString() + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Patient_EHR_ID"].ToString().Trim() != row[0]["Patient_EHR_ID"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Disease_EHR_ID"].ToString().Trim() != row[0]["Disease_EHR_ID"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Disease_Name"].ToString().Trim() != row[0]["Disease_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Disease_Type"].ToString().Trim() != row[0]["Disease_Type"].ToString().Trim())
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

                        foreach (DataRow dtLPHRow in dtLocalDisease.Rows)
                        {
                            DataRow[] rowDis = dtPracticeWebDisease.Copy().Select("Disease_EHR_ID = '" + dtLPHRow["Disease_EHR_ID"].ToString() + "' AND Disease_Type = '" + dtLPHRow["Disease_Type"].ToString() + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtLPHRow["Patient_EHR_ID"] + "'");
                            if (rowDis.Length > 0)
                            { }
                            else
                            {
                                DataRow rowDisDtldr = dtPracticeWebDisease.NewRow();
                                rowDisDtldr["Patient_EHR_ID"] = dtLPHRow["Patient_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Disease_EHR_ID"] = dtLPHRow["Disease_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Disease_Type"] = dtLPHRow["Disease_Type"].ToString().Trim();
                                rowDisDtldr["Disease_Name"] = dtLPHRow["Disease_Name"].ToString().Trim();
                                rowDisDtldr["Clinic_Number"] = dtLPHRow["Clinic_Number"].ToString().Trim();
                                //  rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
                                rowDisDtldr["InsUptDlt"] = 3;
                                dtPracticeWebDisease.Rows.Add(rowDisDtldr);
                            }
                        }


                        dtPracticeWebDisease.AcceptChanges();

                        if (dtPracticeWebDisease != null && dtPracticeWebDisease.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_PatientDisease_PracticeWeb_To_Local(dtPracticeWebDisease, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                                ObjGoalBase.WriteToSyncLogFile("PatientDisease Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_PatientDisease();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[PatientDisease Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                        Is_synched_PatientDisease = false;

                    }
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatientDisease = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientDisease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataPracticeWeb_PatientMedication(string Patient_EHR_IDS)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchPracticeWebBAL.GetPracticeWebPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patient_EHR_IDS);
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patient_EHR_IDS);

                        foreach (DataRow dtDtxRow in dtMedication.Rows)
                        {
                            DataRow[] row = dtLocalMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtDtxRow["PatientMedication_EHR_ID"].ToString().Trim() + "' And Medication_EHR_ID = '" + dtDtxRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
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
                                else if (dtDtxRow["Medication_Name"].ToString().Trim() != row[0]["Medication_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Medication_Type"].ToString().Trim() != row[0]["Medication_Type"].ToString().Trim())
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
                        Is_synched_PatientMedication = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatientMedication = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientMedication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #region Patient Query

        public static string PracticeWebInsert_Patient = " INSERT INTO Patient "
                                                       + "(patient_ehr_id, "
                                                       + " patient_Web_ID, "
                                                       + " First_name, "
                                                       + " Last_name, "
                                                       + " Middle_Name, "
                                                       + " Salutation, "
                                                       + " preferred_name, "
                                                       + " Status, "
                                                       + " Sex, "
                                                       + " MaritalStatus, "
                                                       + " Birth_Date, "
                                                       + " Email, "
                                                       + " Mobile, "
                                                       + " Home_Phone, "
                                                       + " Work_Phone, "
                                                       + " Address1, "
                                                       + " Address2, "
                                                       + " City, "
                                                       + " State, "
                                                       + " Zipcode, "
                                                       + " ResponsibleParty_Status, "
                                                       + " CurrentBal, "
                                                       + " ThirtyDay, "
                                                       + " SixtyDay, "
                                                       + " NinetyDay, "
                                                       + " Over90, "
                                                       + " FirstVisit_Date, "
                                                       + " LastVisit_Date, "
                                                       + " Primary_Insurance, "
                                                       + " Primary_Insurance_CompanyName, "
                                                       + " Primary_Ins_Subscriber_ID, "
                                                       + " Secondary_Insurance, "
                                                       + " Secondary_Insurance_CompanyName, "
                                                       + " Secondary_Ins_Subscriber_ID, "
                                                       + " Guar_ID, "
                                                       + " Pri_Provider_ID, "
                                                       + " Sec_Provider_ID, "
                                                       + " ReceiveSms, "
                                                       + " ReceiveEmail, "
                                                       + " nextvisit_date, "
                                                       + " due_date, "
                                                       + " remaining_benefit, "
                                                       + " used_benefit, "
                                                       + " collect_payment, "
                                                       + " EHR_Entry_DateTime, "
                                                       + " Last_Sync_Date, "
                                                       + " Is_Adit_Updated,Clinic_Number,Service_Install_Id,is_deleted,[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note], "
                                                       + " ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber ) "
                                                       + " VALUES "
                                                       + "(@patient_ehr_id, "
                                                       + " @patient_Web_ID, "
                                                       + " @First_name, "
                                                       + " @Last_name, "
                                                       + " @Middle_Name, "
                                                       + " @Salutation, "
                                                       + " @preferred_name, "
                                                       + " @Status, "
                                                       + " @Sex, "
                                                       + " @MaritalStatus,  "
                                                       + " @Birth_Date, "
                                                       + " @Email, "
                                                       + " @Mobile, "
                                                       + " @Home_Phone, "
                                                       + " @Work_Phone, "
                                                       + " @Address1, "
                                                       + " @Address2, "
                                                       + " @City, "
                                                       + " @State, "
                                                       + " @Zipcode, "
                                                       + " @ResponsibleParty_Status, "
                                                       + " @CurrentBal, "
                                                       + " @ThirtyDay, "
                                                       + " @SixtyDay, "
                                                       + " @NinetyDay, "
                                                       + " @Over90, "
                                                       + " @FirstVisit_Date, "
                                                       + " @LastVisit_Date, "
                                                       + " @Primary_Insurance, "
                                                       + " @Primary_Insurance_CompanyName, "
                                                       + " @Primary_Ins_Subscriber_ID, "
                                                       + " @Secondary_Insurance, "
                                                       + " @Secondary_Insurance_CompanyName, "
                                                       + " @Secondary_Ins_Subscriber_ID, "
                                                       + " @Guar_ID, "
                                                       + " @Pri_Provider_ID, "
                                                       + " @Sec_Provider_ID, "
                                                       + " @ReceiveSms, "
                                                       + " @ReceiveEmail, "
                                                       + " @nextvisit_date, "
                                                       + " @due_date, "
                                                       + " @remaining_benefit, "
                                                       + " @used_benefit, "
                                                       + " @collect_payment, "
                                                       + " @EHR_Entry_DateTime, "
                                                       + " @Last_Sync_Date,"
                                                       + " @Is_Adit_Updated,@Clinic_Number,@Service_Install_Id,@is_deleted,@EHR_Status,@ReceiveVoiceCall,@PreferredLanguage,@Patient_Note, "
                                                       + " @ssn,@driverlicense,@groupid ,@emergencycontactId ,@EmergencyContact_First_Name,@EmergencyContact_Last_Name  ,@emergencycontactnumber,@school ,@employer ,@spouseId ,@responsiblepartyId ,@responsiblepartyssn ,@responsiblepartybirthdate,@Spouse_First_Name ,@Spouse_Last_Name ,@ResponsibleParty_First_Name ,@ResponsibleParty_Last_Name,@Prim_Ins_Company_Phonenumber ,@Sec_Ins_Company_Phonenumber ) ";

        public static string PracticeWebUpdate_Patient = " UPDATE Patient SET "
                                                       + " First_name = @First_name , "
                                                       + " Last_name = @Last_name, "
                                                       + " Middle_Name = @Middle_Name, "
                                                       + " Salutation = @Salutation, "
                                                       + " preferred_name = @preferred_name, "
                                                       + " Status = @Status, "
                                                       + " Sex = @Sex, "
                                                       + " MaritalStatus = @MaritalStatus, "
                                                       + " Birth_Date = @Birth_Date, "
                                                       + " Email = @Email, "
                                                       + " Mobile = @Mobile, "
                                                       + " Home_Phone = @Home_Phone, "
                                                       + " Work_Phone= @Work_Phone, "
                                                       + " Address1 = @Address1, "
                                                       + " Address2 = @Address2, "
                                                       + " City = @City, "
                                                       + " State = @State, "
                                                       + " Zipcode= @Zipcode, "
                                                       + " ResponsibleParty_Status = @ResponsibleParty_Status, "
                                                       + " CurrentBal = @CurrentBal, "
                                                       + " ThirtyDay= @ThirtyDay, "
                                                       + " SixtyDay = @SixtyDay, "
                                                       + " NinetyDay = @NinetyDay, "
                                                       + " Over90 = @Over90, "
                                                       + " FirstVisit_Date = @FirstVisit_Date, "
                                                       + " LastVisit_Date = @LastVisit_Date, "
                                                       + " Primary_Insurance= @Primary_Insurance,"
                                                       + " Primary_Insurance_CompanyName= @Primary_Insurance_CompanyName, "
                                                       + " Primary_Ins_Subscriber_ID = @Primary_Ins_Subscriber_ID, "
                                                       + " Secondary_Insurance = @Secondary_Insurance, "
                                                       + " Secondary_Insurance_CompanyName= @Secondary_Insurance_CompanyName, "
                                                       + " Secondary_Ins_Subscriber_ID = @Secondary_Ins_Subscriber_ID, "
                                                       + " Guar_ID= @Guar_ID, "
                                                       + " Pri_Provider_ID = @Pri_Provider_ID, "
                                                       + " Sec_Provider_ID= @Sec_Provider_ID, "
                                                       + " ReceiveSms= @ReceiveSms, "
                                                       + " ReceiveEmail = @ReceiveEmail, "
                                                       + " nextvisit_date = @nextvisit_date, "
                                                       + " due_date = @due_date, "
                                                       + " remaining_benefit = @remaining_benefit, "
                                                       + " used_benefit = @used_benefit, "
                                                       + " collect_payment = @collect_payment, "
                                                       + " EHR_Entry_DateTime = @EHR_Entry_DateTime,"
                                                       + " Is_Adit_Updated = @Is_Adit_Updated ,"
                                                       + " Clinic_Number = @Clinic_Number, "
                                                       + " [is_deleted] = @is_deleted, "
                                                       + " [EHR_Status] = @EHR_Status, "
                                                       + " [ReceiveVoiceCall] = @ReceiveVoiceCall, "
                                                       + " [PreferredLanguage] = @PreferredLanguage,[Patient_Note] = @Patient_Note, "
                                                       + " ssn = @ssn,driverlicense = @driverlicense,groupid = @groupid ,emergencycontactId = @emergencycontactId ,EmergencyContact_First_Name = @EmergencyContact_First_Name,EmergencyContact_Last_Name = @EmergencyContact_Last_Name , "
                                                       + " emergencycontactnumber = @emergencycontactnumber,school = @school ,employer = @employer ,spouseId = @spouseId ,responsiblepartyId = @responsiblepartyId , "
                                                       + " responsiblepartyssn = @responsiblepartyssn ,responsiblepartybirthdate = @responsiblepartybirthdate,Spouse_First_Name = @Spouse_First_Name , "
                                                       + " Spouse_Last_Name = @Spouse_Last_Name ,ResponsibleParty_First_Name = @ResponsibleParty_First_Name , ResponsibleParty_Last_Name = @ResponsibleParty_Last_Name, "
                                                       + " Prim_Ins_Company_Phonenumber = @Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber = @Sec_Ins_Company_Phonenumber "
                                                       + " WHERE patient_ehr_id = @patient_ehr_id and Service_Install_Id = @Service_Install_Id  ";


        public static string PracticeWebDelete_Patient = " Delete From Patient WHERE Patient_EHR_ID = @Patient_EHR_ID And Service_Install_Id = @Service_Install_Id   ";

        public static string PracticeWebPatientCompareQuery = " SELECT ACC.* FROM ( SELECT (CASE WHEN PT.Patient_EHR_id IS NULL THEN 1 ELSE 2 END) AS InsUptDlt, PC.* FROM (SELECT Distinct Service_Install_Id,[Patient_EHR_ID] "
                                           + "  FROM (SELECT DISTINCT Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                           + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                           + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note] ,"
                                           + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber "
                                           + "  FROM Patient where Service_Install_Id = @Service_Install_Id UNION All  "
                                           + "  SELECT DISTINCT Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                           + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                           + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note],  "
                                           + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber "
                                           + "   FROM PatientCompare where Service_Install_Id = @Service_Install_Id) data "
                                           + "  GROUP BY  Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                           + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                           + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note],  "
                                           + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber "
                                           + "  HAVING count(1) = 1 ) AS RS "
                                           + "  LEFT JOIN Patient PT ON Rs.Service_Install_Id = PT.Service_Install_Id And RS.Patient_EHR_Id = PT.Patient_EHR_Id "
                                           + "  LEFT JOIN PatientCompare PC ON Pc.Service_Install_Id = Rs.Service_Install_Id And PC.Patient_EHR_Id = RS.Patient_EHR_Id  ) AS ACC WHERE Patient_EHR_id != '' And Service_Install_Id = @Service_Install_Id ";

        #endregion

        #endregion

        #region Patient Form

        private void fncSynchDataLocalToPracticeWeb_Patient_Form()
        {
            InitBgWorkerLocalToPracticeWeb_Patient_Form();
            InitBgTimerLocalToPracticeWeb_Patient_Form();
        }

        private void InitBgTimerLocalToPracticeWeb_Patient_Form()
        {
            timerSynchLocalToPracticeWeb_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToPracticeWeb_Patient_Form.Interval = 1000 * GoalBase.intervalEHRSynch_PatientForm;
            this.timerSynchLocalToPracticeWeb_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToPracticeWeb_Patient_Form_Tick);
            timerSynchLocalToPracticeWeb_Patient_Form.Enabled = true;
            timerSynchLocalToPracticeWeb_Patient_Form.Start();
            timerSynchLocalToPracticeWeb_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToPracticeWeb_Patient_Form()
        {
            bwSynchLocalToPracticeWeb_Patient_Form = new BackgroundWorker();
            bwSynchLocalToPracticeWeb_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToPracticeWeb_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToPracticeWeb_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToPracticeWeb_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToPracticeWeb_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToPracticeWeb_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToPracticeWeb_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToPracticeWeb_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToPracticeWeb_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToPracticeWeb_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToPracticeWeb_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToPracticeWeb_Patient_Form);
            procThreadmainLocalToPracticeWeb_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToPracticeWeb_Patient_Form()
        {
            if (bwSynchLocalToPracticeWeb_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToPracticeWeb_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToPracticeWeb_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToPracticeWeb_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToPracticeWeb_Patient_Form();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToPracticeWeb_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToPracticeWeb_Patient_Form.Enabled = true;
        }

        public void SynchDataLocalToPracticeWeb_Patient_Form()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    SynchDataLiveDB_Pull_PatientForm();
                    SynchDataLiveDB_Pull_PatientPortal();
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

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtWebPatient_Form.Columns.Add(new DataColumn("Table_Name", typeof(string)));

                        if (dtWebPatient_Form != null)
                        {
                            if (dtWebPatient_Form.Rows.Count > 0)
                            {
                                Utility.CheckEntryUserLoginIdExist();
                            }
                        }

                        foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                        {
                            dtDtxRow["Table_Name"] = "patient";

                            if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "FIRST_NAME")
                            {
                                dtDtxRow["ehrfield"] = "FName";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "LAST_NAME")
                            {
                                dtDtxRow["ehrfield"] = "LName";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MOBILE")
                            {
                                dtDtxRow["ehrfield"] = "WirelessPhone";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ADDRESS_ONE")
                            {
                                dtDtxRow["ehrfield"] = "Address";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ADDRESS_TWO")
                            {
                                dtDtxRow["ehrfield"] = "Address2";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "BIRTH_DATE")
                            {
                                dtDtxRow["ehrfield"] = "Birthdate";
                                try
                                {
                                    dtDtxRow["ehrfield_value"] = Convert.ToDateTime(dtDtxRow["ehrfield_value"].ToString().Trim()).ToString("yyyy-MM-dd");

                                }
                                catch (Exception)
                                {
                                    dtDtxRow["ehrfield_value"] = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "CITY")
                            {
                                dtDtxRow["ehrfield"] = "City";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMAIL")
                            {
                                dtDtxRow["ehrfield"] = "Email";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "HOME_PHONE")
                            {
                                dtDtxRow["ehrfield"] = "HmPhone";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MARITAL_STATUS")
                            {
                                dtDtxRow["ehrfield"] = "Position";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "SINGLE")
                                {
                                    dtDtxRow["ehrfield_value"] = 0;
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MARRIED")
                                {
                                    dtDtxRow["ehrfield_value"] = 1;
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "CHILD")
                                {
                                    dtDtxRow["ehrfield_value"] = 2;
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "WIDOWED")
                                {
                                    dtDtxRow["ehrfield_value"] = 3;
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "DIVORCED")
                                {
                                    dtDtxRow["ehrfield_value"] = 4;
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = 0;
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MIDDLE_NAME")
                            {
                                dtDtxRow["ehrfield"] = "MiddleI";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PREFERRED_NAME")
                            {
                                dtDtxRow["ehrfield"] = "preferred";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
                            {
                                dtDtxRow["ehrfield"] = "PriProv";
                                try
                                {
                                    dtDtxRow["ehrfield_value"] = Convert.ToInt64(dtDtxRow["ehrfield_value"].ToString().Trim());
                                }
                                catch (Exception)
                                {
                                    dtDtxRow["ehrfield_value"] = 1;
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE")
                            {
                                dtDtxRow["ehrfield"] = "PRIMARY_INSURANCE";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
                            {
                                dtDtxRow["ehrfield"] = "PRIMARY_INSURANCE_COMPANYNAME";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                            {
                                dtDtxRow["ehrfield"] = "PRIMARY_INS_SUBSCRIBER_ID";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "RECEIVE_EMAIL")
                            {
                                dtDtxRow["ehrfield"] = "TxtMsgOk";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO")
                                {
                                    dtDtxRow["ehrfield_value"] = "2";
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = "1";
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "RECEIVE_SMS")
                            {
                                dtDtxRow["ehrfield"] = "TxtMsgOk";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO")
                                {
                                    dtDtxRow["ehrfield_value"] = "2";
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = "1";
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SALUTATION")
                            {
                                dtDtxRow["ehrfield"] = "Salutation";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
                            {
                                dtDtxRow["ehrfield"] = "SecProv";
                                try
                                {
                                    dtDtxRow["ehrfield_value"] = Convert.ToInt64(dtDtxRow["ehrfield_value"].ToString().Trim());
                                }
                                catch (Exception)
                                {
                                    dtDtxRow["ehrfield_value"] = 1;
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE")
                            {
                                dtDtxRow["ehrfield"] = "SECONDARY_INSURANCE";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
                            {
                                dtDtxRow["ehrfield"] = "SECONDARY_INSURANCE_COMPANYNAME";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                            {
                                dtDtxRow["ehrfield"] = "SECONDARY_INS_SUBSCRIBER_ID";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SEX")
                            {
                                dtDtxRow["ehrfield"] = "Gender";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                                {
                                    dtDtxRow["ehrfield_value"] = 1;
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = 0;
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "WORK_PHONE")
                            {
                                dtDtxRow["ehrfield"] = "WkPhone";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "STATE")
                            {
                                dtDtxRow["ehrfield"] = "State";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ZIPCODE")
                            {
                                dtDtxRow["ehrfield"] = "Zip";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SSN")
                            {
                                dtDtxRow["ehrfield"] = "SSN";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SCHOOL")
                            {
                                dtDtxRow["ehrfield"] = "SchoolName";
                            }

                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME")
                            {
                                dtDtxRow["ehrfield"] = "ICEName";
                                dtDtxRow["Table_Name"] = "patientnote";


                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER")
                            {
                                dtDtxRow["ehrfield"] = "ICEPhone";
                                dtDtxRow["Table_Name"] = "patientnote";

                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMPLOYER")
                            {
                                dtDtxRow["ehrfield"] = "EmpName";
                                dtDtxRow["Table_Name"] = "employer";

                            }

                            dtWebPatient_Form.AcceptChanges();
                        }
                        if (dtWebPatient_Form.Rows.Count > 0)
                        {
                            bool Is_Record_Update = SynchPracticeWebBAL.Save_Patient_Form_Local_To_PracticeWeb(dtWebPatient_Form, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }

                        try
                        {
                            GetMedicalOpenDentalHistoryRecords(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            SynchPracticeWebBAL.SaveMedicalHistoryLocalToPracticeWeb(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            ObjGoalBase.WriteToSyncLogFile("Medical_History_Save Sync ( Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".  Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        catch (Exception ex2)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Medical_History_Save Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex2.Message);
                        }

                        try
                        {
                            if (SynchPracticeWebBAL.SavePatientDisease(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()))
                            {
                                ObjGoalBase.WriteToSyncLogFile("Patient_Alert Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]");
                            }
                        }
                        catch (Exception ex1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                        }
                        try
                        {
                            if (SynchPracticeWebBAL.DeletePatientDisease(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()))
                            {
                                ObjGoalBase.WriteToSyncLogFile("Delete_Patient_Alert Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Alert Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]");
                            }
                        }
                        catch (Exception ex1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Alert Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                        }

                        bool isRecordSaved = false, isRecordDeleted = false;
                        string Patient_EHR_IDS = "";
                        string DeletePatientEHRID = "";
                        string SavePatientEHRID = "";
                        try
                        {
                            if (SynchPracticeWebBAL.DeletePatientMedication(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), ref isRecordDeleted, ref DeletePatientEHRID))
                            {
                                ObjGoalBase.WriteToSyncLogFile("Delete Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]");
                            }
                        }
                        catch (Exception ex1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                        }
                        try
                        {
                            if (SynchPracticeWebBAL.SavePatientMedication(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), ref isRecordSaved, ref SavePatientEHRID))
                            {
                                ObjGoalBase.WriteToSyncLogFile("Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]");
                            }
                        }
                        catch (Exception ex1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                        }
                        if (isRecordSaved || isRecordDeleted)
                        {
                            Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                            if (Patient_EHR_IDS != "")
                            {
                                SynchDataPracticeWeb_PatientMedication(Patient_EHR_IDS);
                            }
                        }

                        string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        if (Call_Importing.ToLower() != "success")
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Importing status. Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " : " + Call_Importing);
                        }

                        string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        if (Call_Completed.ToLower() != "success")
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Completed status.Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " : " + Call_Completed);
                        }

                        try
                        {
                            GetPatientDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            GetPatientDocument_New(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            SynchPracticeWebBAL.Save_Document_in_PracticeWeb(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());                           
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }

                        #region Treatment Document
                        try
                        {
                            SynchPracticeWebBAL.Save_Treatment_Document_in_PracticeWeb(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());
                            //SynchClearDentBAL.Save_Treatment_Document_in_ClearDent();
                            #region change status as treatment doc impotred Completed
                            DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                            if (statusCompleted.Rows.Count > 0)
                            {
                                Change_Status_TreatmentDoc(statusCompleted, "Completed");
                            }
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Treatment_Document Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }
                        #endregion
                        #endregion

                        #region Insurance Carrier
                        try
                        {
                            SynchPracticeWebBAL.Save_InsuranceCarrier_Document_in_PracticeWeb(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());

                            #region change status as InsuranceCarrier doc impotred Completed
                            DataTable statusCompleted = SynchLocalBAL.ChangeStatusForInsuranceCarrierDoc("Completed");
                            if (statusCompleted.Rows.Count > 0)
                            {
                                Change_Status_InsuranceCarrierDoc(statusCompleted, "Completed");
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_InsuranceCarrier_Document Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }
                       
                        #endregion

                        string Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.Location_ID);
                        try
                        {
                            if (Call_PatientPortalCompleted != "success")
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                            }
                            else
                            {
                                ObjGoalBase.WriteToSyncLogFile("[Patient_Portal API called with Completed status : " + Call_PatientPortalCompleted);
                            }
                        }
                        catch (Exception)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                        }
                        ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");

                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }
        #region PatientPayment
        private void fncSynchDataPracticeWeb_PatientPayment()
        {
            InitBgWorkerPracticeWeb_PatientPayment();
            InitBgTimerPracticeWeb_PatientPayment();
        }
        private void InitBgTimerPracticeWeb_PatientPayment()
        {
            timerSynchPracticeWeb_PatientPayment = new System.Timers.Timer();
            this.timerSynchPracticeWeb_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
            this.timerSynchPracticeWeb_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_PatientPayment_Tick);
            timerSynchPracticeWeb_PatientPayment.Enabled = true;
            timerSynchPracticeWeb_PatientPayment.Start();
        }

        private void InitBgWorkerPracticeWeb_PatientPayment()
        {
            bwSynchPracticeWeb_PatientPayment = new BackgroundWorker();
            bwSynchPracticeWeb_PatientPayment.WorkerReportsProgress = true;
            bwSynchPracticeWeb_PatientPayment.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_PatientPayment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWeb_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_PatientPayment_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_PatientPayment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_PatientPayment.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_PatientPayment();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_PatientPayment()
        {
            System.Threading.Thread procThreadmainPracticeWeb_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_PatientPayment);
            procThreadmainPracticeWeb_PatientPayment.Start();
        }

        public void CallSyncOrderTablePracticeWeb_PatientPayment()
        {
            if (bwSynchPracticeWeb_PatientPayment.IsBusy != true)
            {
                bwSynchPracticeWeb_PatientPayment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_PatientPayment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToPracticeWeb_PatientPayment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_PatientPayment.Enabled = true;
        }

        public void SynchDataLocalToPracticeWeb_PatientPayment()
        {
            try
            {
                if (!IsPaymentSyncing)
                {
                    IsPaymentSyncing = true;
                    if (!Is_synched_PatinetForm)
                    {
                        if (Utility.IsApplicationIdleTimeOff)
                        {
                            SynchDataLiveDB_Pull_PatientPaymentLog();

                            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                            {
                                DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (dtWebPatientPayment.Rows.Count > 0)
                                {
                                    Utility.CheckEntryUserLoginIdExist();
                                    bool Is_Record_Update = SynchPracticeWebBAL.Save_PatientPayment_Local_To_PracticeWeb(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                }

                                //string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                //if (Call_Importing.ToLower() != "success")
                                //{
                                //    ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Importing status. Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " : " + Call_Importing);
                                //}

                                //string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                //if (Call_Completed.ToLower() != "success")
                                //{
                                //    ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Completed status.Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " : " + Call_Completed);
                                //}

                                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");

                            }
                        }
                    }
                    IsPaymentSyncing = false;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
            finally { IsPaymentSyncing = false; }
        }
        #endregion

        public void SynchDataPracticeWeb_PatientImages()
        {
            try
            {
                if (Utility.IsExternalAppointmentSync)
                {
                    Is_synched_PatientImages = false;
                }
                if (!Is_synched_PatientImages && Utility.IsApplicationIdleTimeOff)
                {
                    // SynchDataLiveDB_Push_PatientImage();

                    Is_synched_PatientImages = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebPatientImages = SynchPracticeWebBAL.GetPracticeWebPatientImagesData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtPracticeWebPatientImages.Columns.Add("InsUptDlt", typeof(int));
                        dtPracticeWebPatientImages.Columns.Add("SourceLocation", typeof(string));
                        dtPracticeWebPatientImages.Columns["InsUptDlt"].DefaultValue = 0;
                        DataTable dtLocalPatientImages = SynchLocalBAL.GetLocalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString());
                        Utility.EHRProfileImagePath = SynchPracticeWebDAL.GetPracticeWebDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        foreach (DataRow dtDtxRow in dtPracticeWebPatientImages.Rows)
                        {
                            if (Utility.EHRProfileImagePath == string.Empty || Utility.EHRProfileImagePath == "")
                            {
                                dtDtxRow["SourceLocation"] = "C:\\FreeDentalImages\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                            }
                            else
                            {
                                dtDtxRow["SourceLocation"] = Utility.EHRProfileImagePath + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                            }
                            DataRow[] row = dtLocalPatientImages.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
                            if (row.Length > 0)
                            {
                                if (!Convert.ToBoolean(row[0]["Is_Deleted"]))
                                {
                                    if (dtDtxRow["Patient_Images_EHR_ID"].ToString().Trim() != row[0]["Patient_Images_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                        dtDtxRow["Is_Adit_Updated"] = 0;
                                    }
                                    else if (dtDtxRow["Patient_Images_FilePath"].ToString() != row[0]["Patient_Images_FilePath"].ToString())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                        dtDtxRow["Is_Adit_Updated"] = 0;
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
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        foreach (DataRow dtDtlRow in dtLocalPatientImages.Rows)
                        {
                            DataRow[] row = dtPracticeWebPatientImages.Copy().Select("Patient_EHR_ID = '" + dtDtlRow["Patient_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length <= 0)
                            {
                                if (!Convert.ToBoolean(dtDtlRow["Is_Deleted"]))
                                {
                                    DataRow ApptDtldr = dtPracticeWebPatientImages.NewRow();
                                    ApptDtldr["Patient_EHR_ID"] = dtDtlRow["Patient_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Patient_Images_EHR_ID"] = dtDtlRow["Patient_Images_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Image_EHR_Name"] = dtDtlRow["Image_EHR_Name"].ToString().Trim();
                                    ApptDtldr["Clinic_Number"] = dtDtlRow["Clinic_Number"].ToString().Trim();
                                    ApptDtldr["Service_Install_Id"] = dtDtlRow["Service_Install_Id"].ToString().Trim();
                                    ApptDtldr["InsUptDlt"] = 3;
                                    ApptDtldr["Is_Deleted"] = 1;
                                    dtPracticeWebPatientImages.Rows.Add(ApptDtldr);
                                }

                            }
                        }

                        dtPracticeWebPatientImages.AcceptChanges();
                        bool status = false;
                        DataTable dtSaveRecords = dtPracticeWebPatientImages.Clone();
                        if (dtPracticeWebPatientImages.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtPracticeWebPatientImages.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                            status = SynchLocalBAL.Save_PatientProfileImage_EHR_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Images");
                            ObjGoalBase.WriteToSyncLogFile("PatientImage Sync (" + Utility.Application_Name + " to Local Database) Successfully.");


                        }
                        SynchDataLiveDB_Push_PatientImage(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        public void SynchDataPatientPayment_LocalTOPracticeWeb()
        {
            try
            {
               // CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff)
                {
                    Int64 TransactionHeaderId = 0;
                    string noteId = "";
                    DataTable dtPatientPayment = new DataTable();
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                        {
                            //Utility.CheckEntryUserLoginIdExist();
                            noteId = "";


                            bool Is_Record_Update = SynchPracticeWebBAL.Save_PatientPayment_Local_To_PracticeWeb(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());


                            #region Call API for EHR Entry Done

                            //noteId = SynchPracticeWebBAL.Save_PatientPaymentLog_LocalToPracticeWeb(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            //if (noteId != "")
                            //{
                            //    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientPayment_LocalTOPracticeWeb");
                            //    ObjGoalBase.WriteToSyncLogFile("PatientPaymentLog Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            //    IsGetParientRecordDone = true;
                            //    SynchDataLiveDB_Push_Patient();
                            //}
                            //else
                            //{
                            //    ObjGoalBase.WriteToErrorLogFile("[PatientPaymentLog Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                            //}
                            #endregion

                            #region Sync those patient whose payment done in EHR

                            #endregion
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                        }
                        //  }
                    }

                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient Payment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public void SynchDataPatientSMSCall_LocalTOPracticeWeb()
        {
            try
            {
               // CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff)
                {
                    Int64 TransactionHeaderId = 0;
                    string noteId = "";
                    DataTable dtPatientPayment = new DataTable();
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        SynchPracticeWebBAL.DeleteDuplicatePatientLog(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        // ObjGoalBase.WriteToSyncLogFile("Get Records");
                        DataTable dtWebPatientSMSCallLog = SynchLocalBAL.GetLocalWebPatientSMSCallLogData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        // ObjGoalBase.WriteToSyncLogFile("Total Records to be saved in EHR " + dtWebPatientSMSCallLog.Rows.Count.ToString());
                        #region Call API for EHR Entry Done
                        if (dtWebPatientSMSCallLog != null && dtWebPatientSMSCallLog.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                            // System.Windows.Forms.MessageBox.Show("0");
                            SynchPracticeWebBAL.Save_PatientSMSCallLog_LocalToPracticeWeb(dtWebPatientSMSCallLog, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            // }
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientSMSCallLog_LocalTOPracticeWeb");
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

        #region Synch Disease

        private void fncSynchDataPracticeWeb_Disease()
        {
            InitBgWorkerPracticeWeb_Disease();
            InitBgTimerPracticeWeb_Disease();
        }

        private void InitBgTimerPracticeWeb_Disease()
        {
            timerSynchPracticeWeb_Disease = new System.Timers.Timer();
            this.timerSynchPracticeWeb_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchPracticeWeb_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_Disease_Tick);
            timerSynchPracticeWeb_Disease.Enabled = true;
            timerSynchPracticeWeb_Disease.Start();
        }

        private void InitBgWorkerPracticeWeb_Disease()
        {
            bwSynchPracticeWeb_Disease = new BackgroundWorker();
            bwSynchPracticeWeb_Disease.WorkerReportsProgress = true;
            bwSynchPracticeWeb_Disease.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_Disease.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_Disease_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchPracticeWeb_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_Disease_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_Disease_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_Disease.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_Disease();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_Disease()
        {
            System.Threading.Thread procThreadmainPracticeWeb_Disease = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_Disease);
            procThreadmainPracticeWeb_Disease.Start();
        }

        public void CallSyncOrderTablePracticeWeb_Disease()
        {
            if (bwSynchPracticeWeb_Disease.IsBusy != true)
            {
                bwSynchPracticeWeb_Disease.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_Disease_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_Disease.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_Disease();
                //  SynchDataPracticeWeb_DiseaseHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_Disease.Enabled = true;
        }

        public void SynchDataPracticeWeb_Disease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebDisease = SynchPracticeWebBAL.GetPracticeWebDiseaseData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtPracticeWebDisease.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalDisease = SynchLocalBAL.GetLocalDiseaseData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebDisease.Rows)
                        {
                            DataRow[] row = dtLocalDisease.Copy().Select("Disease_EHR_ID = '" + dtDtxRow["Disease_EHR_ID"] + "' AND Disease_Type = '" + dtDtxRow["Disease_Type"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["EHR_Entry_DateTime"].ToString().Trim() != row[0]["EHR_Entry_DateTime"].ToString().Trim())
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

                        foreach (DataRow dtLPHRow in dtLocalDisease.Rows)
                        {
                            DataRow[] rowDis = dtPracticeWebDisease.Copy().Select("Disease_EHR_ID = '" + dtLPHRow["Disease_EHR_ID"] + "' AND Disease_Type = '" + dtLPHRow["Disease_Type"] + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"] + "' ");
                            if (rowDis.Length > 0)
                            { }
                            else
                            {
                                DataRow rowDisDtldr = dtPracticeWebDisease.NewRow();
                                rowDisDtldr["Disease_EHR_ID"] = dtLPHRow["Disease_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Disease_Type"] = dtLPHRow["Disease_Type"].ToString().Trim();
                                rowDisDtldr["Disease_Name"] = dtLPHRow["Disease_Name"].ToString().Trim();
                                //  rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
                                rowDisDtldr["InsUptDlt"] = 3;
                                dtPracticeWebDisease.Rows.Add(rowDisDtldr);
                            }
                        }


                        dtPracticeWebDisease.AcceptChanges();

                        if (dtPracticeWebDisease != null && dtPracticeWebDisease.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_Disease_PracticeWeb_To_Local(dtPracticeWebDisease, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                                ObjGoalBase.WriteToSyncLogFile("Disease Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_Disease();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }

                    }
                    SynchDataPracticeWeb_Medication();
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }


        public void SynchDataPracticeWeb_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchPracticeWebBAL.GetPracticeWebMedicationData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        #endregion

        #region Synch RecallType

        private void fncSynchDataPracticeWeb_RecallType()
        {
            InitBgWorkerPracticeWeb_RecallType();
            InitBgTimerPracticeWeb_RecallType();
        }

        private void InitBgTimerPracticeWeb_RecallType()
        {
            timerSynchPracticeWeb_RecallType = new System.Timers.Timer();
            this.timerSynchPracticeWeb_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchPracticeWeb_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_RecallType_Tick);
            timerSynchPracticeWeb_RecallType.Enabled = true;
            timerSynchPracticeWeb_RecallType.Start();
        }

        private void InitBgWorkerPracticeWeb_RecallType()
        {
            bwSynchPracticeWeb_RecallType = new BackgroundWorker();
            bwSynchPracticeWeb_RecallType.WorkerReportsProgress = true;
            bwSynchPracticeWeb_RecallType.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_RecallType.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_RecallType_DoWork);
            bwSynchPracticeWeb_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_RecallType_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_RecallType.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_RecallType();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_RecallType()
        {
            System.Threading.Thread procThreadmainPracticeWeb_RecallType = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_RecallType);
            procThreadmainPracticeWeb_RecallType.Start();
        }

        public void CallSyncOrderTablePracticeWeb_RecallType()
        {
            if (bwSynchPracticeWeb_RecallType.IsBusy != true)
            {
                bwSynchPracticeWeb_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_RecallType.Enabled = true;
        }

        public void SynchDataPracticeWeb_RecallType()
        {
            try
            {
                if (!Is_synched_RecallType && Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_RecallType = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebRecallType = SynchPracticeWebBAL.GetPracticeWebRecallTypeData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtPracticeWebRecallType.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebRecallType.Rows)
                        {
                            DataRow[] row = dtLocalRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "' And  Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
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

                        dtPracticeWebRecallType.AcceptChanges();

                        if (dtPracticeWebRecallType != null && dtPracticeWebRecallType.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_RecallType_PracticeWeb_To_Local(dtPracticeWebRecallType, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
                                ObjGoalBase.WriteToSyncLogFile("RecallType Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");

                                SynchDataLiveDB_Push_RecallType();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Recall Type Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                    }
                    Is_synched_RecallType = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_RecallType = false;
                ObjGoalBase.WriteToErrorLogFile("[RecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch User
        private void fncSynchDataPracticeWeb_User()
        {
            InitBgWorkerPracticeWeb_User();
            InitBgTimerPracticeWeb_User();
        }

        private void InitBgTimerPracticeWeb_User()
        {
            timerSynchPracticeWeb_User = new System.Timers.Timer();
            this.timerSynchPracticeWeb_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchPracticeWeb_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_User_Tick);
            timerSynchPracticeWeb_User.Enabled = true;
            timerSynchPracticeWeb_User.Start();
        }

        private void timerSynchPracticeWeb_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_User.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_User();
            }
        }

        private void MethodForCallSynchOrderPracticeWeb_User()
        {
            System.Threading.Thread procThreadmainPracticeWeb_User = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_User);
            procThreadmainPracticeWeb_User.Start();
        }

        private void CallSyncOrderTablePracticeWeb_User()
        {
            if (bwSynchPracticeWeb_User.IsBusy != true)
            {
                bwSynchPracticeWeb_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void InitBgWorkerPracticeWeb_User()
        {
            bwSynchPracticeWeb_User = new BackgroundWorker();
            bwSynchPracticeWeb_User.WorkerReportsProgress = true;
            bwSynchPracticeWeb_User.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_User.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_User_DoWork);
            bwSynchPracticeWeb_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_User_RunWorkerCompleted);
        }

        private void bwSynchPracticeWeb_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_User.Enabled = true;
        }

        private void bwSynchPracticeWeb_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SynchDataPracticeWeb_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebUser = SynchPracticeWebBAL.GetPracticeWebUserData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtPracticeWebUser.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalUser = SynchLocalBAL.GetLocalUser(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebUser.Rows)
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

                        dtPracticeWebUser.AcceptChanges();

                        if (dtPracticeWebUser != null && dtPracticeWebUser.Rows.Count > 0)
                        {
                            // bool status = SynchTrackerBAL.Save_Tracker_To_Local(dtOpendentalUser, "Users", "User_LocalDB_ID,User_Web_ID", "User_EHR_ID");
                            bool status = SynchPracticeWebBAL.Save_User_PracticeWeb_To_Local(dtPracticeWebUser, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Users");
                                ObjGoalBase.WriteToSyncLogFile("User Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                                SynchDataLiveDB_Push_User();
                            }
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

        #region Synch ApptStatus

        private void fncSynchDataPracticeWeb_ApptStatus()
        {
            InitBgWorkerPracticeWeb_ApptStatus();
            InitBgTimerPracticeWeb_ApptStatus();
        }

        private void InitBgTimerPracticeWeb_ApptStatus()
        {
            timerSynchPracticeWeb_ApptStatus = new System.Timers.Timer();
            this.timerSynchPracticeWeb_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchPracticeWeb_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_ApptStatus_Tick);
            timerSynchPracticeWeb_ApptStatus.Enabled = true;
            timerSynchPracticeWeb_ApptStatus.Start();
        }

        private void InitBgWorkerPracticeWeb_ApptStatus()
        {
            bwSynchPracticeWeb_ApptStatus = new BackgroundWorker();
            bwSynchPracticeWeb_ApptStatus.WorkerReportsProgress = true;
            bwSynchPracticeWeb_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_ApptStatus_DoWork);
            bwSynchPracticeWeb_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_ApptStatus.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_ApptStatus()
        {
            System.Threading.Thread procThreadmainPracticeWeb_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_ApptStatus);
            procThreadmainPracticeWeb_ApptStatus.Start();
        }

        public void CallSyncOrderTablePracticeWeb_ApptStatus()
        {
            if (bwSynchPracticeWeb_ApptStatus.IsBusy != true)
            {
                bwSynchPracticeWeb_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_ApptStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_ApptStatus.Enabled = true;
        }

        public void SynchDataPracticeWeb_ApptStatus()
        {
            try
            {
                if (!Is_synched_ApptStatus && Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_ApptStatus = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebApptStatus = SynchPracticeWebBAL.GetPracticeWebAppointmentStatus(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        dtPracticeWebApptStatus.Columns.Add("InsUptDlt", typeof(int));
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString())
                            {
                                dtPracticeWebApptStatus.Rows.Add(1, "Scheduled", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtPracticeWebApptStatus.Rows.Add(2, "Complete", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtPracticeWebApptStatus.Rows.Add(3, "UnschedList", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtPracticeWebApptStatus.Rows.Add(4, "ASAP", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtPracticeWebApptStatus.Rows.Add(5, "Broken", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtPracticeWebApptStatus.Rows.Add(7, "Patient Note", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtPracticeWebApptStatus.Rows.Add(8, "Cmp. Patient Note", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtPracticeWebApptStatus.Rows.Add(11111, "none", "unshedule", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());

                                //dtPracticeWebApptStatus.Rows.Add(7, "Appointment Scheduled");
                                //dtPracticeWebApptStatus.Rows.Add(8, "Call Back, Not Ready");
                                //dtPracticeWebApptStatus.Rows.Add(9, "Patient Will Call Us");
                                //dtPracticeWebApptStatus.Rows.Add(10, "Bad Debt. Don't Call");
                                //dtPracticeWebApptStatus.Rows.Add(11, "Wait. See notes");
                                //dtPracticeWebApptStatus.Rows.Add(12, "Left Msg on Ans. Mach");
                                //dtPracticeWebApptStatus.Rows.Add(13, "Left Message with Fam");
                                //dtPracticeWebApptStatus.Rows.Add(14, "Discon Ph Num");
                                //dtPracticeWebApptStatus.Rows.Add(15, "Not Home, Call Again");
                                //dtPracticeWebApptStatus.Rows.Add(16, "Mailed Postcard");
                                //dtPracticeWebApptStatus.Rows.Add(17, "Sent Email");
                                //dtPracticeWebApptStatus.Rows.Add(18, "Not Called");
                                //dtPracticeWebApptStatus.Rows.Add(19, "Unconfirmed");
                                //dtPracticeWebApptStatus.Rows.Add(20, "Appointment Confirmed");
                                //dtPracticeWebApptStatus.Rows.Add(21, "Left Msg");
                                //dtPracticeWebApptStatus.Rows.Add(22, "Arrived");
                                //dtPracticeWebApptStatus.Rows.Add(23, "Ready to go back");
                                //dtPracticeWebApptStatus.Rows.Add(24, "In Treatment Room");
                                //dtPracticeWebApptStatus.Rows.Add(25, "Front Desk");
                                //dtPracticeWebApptStatus.Rows.Add(26, "E-mailed");
                                //dtPracticeWebApptStatus.Rows.Add(27, "Texted");
                            }
                        }
                        dtPracticeWebApptStatus.AcceptChanges();

                        DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebApptStatus.Rows)
                        {
                            DataRow[] row = dtLocalApptStatus.Copy().Select("ApptStatus_EHR_ID = '" + dtDtxRow["ApptStatus_EHR_ID"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"].ToString() + "' ");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["ApptStatus_Name"].ToString().ToLower().Trim() != row[0]["ApptStatus_Name"].ToString().ToLower().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["ApptStatus_Type"].ToString().ToLower().Trim() != row[0]["ApptStatus_Type"].ToString().ToLower().Trim())
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

                        dtPracticeWebApptStatus.AcceptChanges();

                        if (dtPracticeWebApptStatus != null && dtPracticeWebApptStatus.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_ApptStatus_PracticeWeb_To_Local(dtPracticeWebApptStatus, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                                ObjGoalBase.WriteToSyncLogFile("ApptStatus Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_ApptStatus();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[ApptStatus Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                    }
                    Is_synched_ApptStatus = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_ApptStatus = false;
                ObjGoalBase.WriteToErrorLogFile("[ApptStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Holiday

        private void fncSynchDataPracticeWeb_Holiday()
        {
            InitBgWorkerPracticeWeb_Holiday();
            InitBgTimerPracticeWeb_Holiday();
        }

        private void InitBgTimerPracticeWeb_Holiday()
        {
            timerSynchPracticeWeb_Holiday = new System.Timers.Timer();
            this.timerSynchPracticeWeb_Holiday.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchPracticeWeb_Holiday.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_Holiday_Tick);
            timerSynchPracticeWeb_Holiday.Enabled = true;
            timerSynchPracticeWeb_Holiday.Start();
        }

        private void InitBgWorkerPracticeWeb_Holiday()
        {
            bwSynchPracticeWeb_Holiday = new BackgroundWorker();
            bwSynchPracticeWeb_Holiday.WorkerReportsProgress = true;
            bwSynchPracticeWeb_Holiday.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_Holiday.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_Holiday_DoWork);
            bwSynchPracticeWeb_Holiday.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_Holiday_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_Holiday_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_Holiday.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_Holiday();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_Holiday()
        {
            System.Threading.Thread procThreadmainPracticeWeb_Holiday = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_Holiday);
            procThreadmainPracticeWeb_Holiday.Start();
        }

        public void CallSyncOrderTablePracticeWeb_Holiday()
        {
            if (bwSynchPracticeWeb_Holiday.IsBusy != true)
            {
                bwSynchPracticeWeb_Holiday.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_Holiday_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_Holiday.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_Holiday();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_Holiday_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_Holiday.Enabled = true;
        }

        public void SynchDataPracticeWeb_Holiday()
        {
            try
            {
                if (!Is_synched_Holidays && Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_Holidays = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebHoliday = SynchPracticeWebBAL.GetPracticeWebHolidayData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtPracticeWebHoliday.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalHoliday = SynchLocalBAL.GetLocalHolidayData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        dtPracticeWebHoliday = CommonUtility.AddHolidays(dtPracticeWebHoliday, dtLocalHoliday, "SchedDate", "Note", "ScheduleNum");

                        foreach (DataRow dtDtxRow in dtPracticeWebHoliday.Rows)
                        {
                            DataRow[] row = dtLocalHoliday.Copy().Select("H_EHR_ID = '" + dtDtxRow["ScheduleNum"] + "'");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Note"].ToString().Trim() != (row[0]["comment"]).ToString().Trim())
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
                                dtDtxRow["Clinic_Number"] = 0;
                                dtDtxRow["DateTStamp"] = Utility.GetCurrentDatetimestring();
                            }
                        }

                        foreach (DataRow dtDtxRow in dtLocalHoliday.Rows)
                        {
                            DataRow[] row = dtPracticeWebHoliday.Copy().Select("ScheduleNum = '" + dtDtxRow["H_EHR_ID"] + "'");
                            if (row.Length <= 0)
                            {
                                DataRow ApptDtldr = dtPracticeWebHoliday.NewRow();
                                ApptDtldr["ScheduleNum"] = dtDtxRow["H_EHR_ID"].ToString().Trim();
                                ApptDtldr["InsUptDlt"] = 3;
                                dtPracticeWebHoliday.Rows.Add(ApptDtldr);
                            }
                        }

                        dtPracticeWebHoliday.AcceptChanges();

                        if (dtPracticeWebHoliday != null && dtPracticeWebHoliday.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_Holiday_PracticeWeb_To_Local(dtPracticeWebHoliday, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                SynchDataLiveDB_Push_Holiday();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Holiday Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                        else
                        {
                            bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday_Push");
                        }
                        ObjGoalBase.WriteToSyncLogFile("Holiday Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");
                    }
                    Is_synched_Holidays = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_Holidays = false;
                ObjGoalBase.WriteToErrorLogFile("[Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Create Appointment

        private void fncSynchDataLocalToPracticeWeb_Appointment()
        {
            InitBgWorkerLocalToPracticeWeb_Appointment();
            InitBgTimerLocalToPracticeWeb_Appointment();
        }

        private void InitBgTimerLocalToPracticeWeb_Appointment()
        {
            timerSynchLocalToPracticeWeb_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToPracticeWeb_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToPracticeWeb_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToPracticeWeb_Appointment_Tick);
            timerSynchLocalToPracticeWeb_Appointment.Enabled = true;
            timerSynchLocalToPracticeWeb_Appointment.Start();
            timerSynchLocalToPracticeWeb_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToPracticeWeb_Appointment()
        {
            bwSynchLocalToPracticeWeb_Appointment = new BackgroundWorker();
            bwSynchLocalToPracticeWeb_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToPracticeWeb_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToPracticeWeb_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToPracticeWeb_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToPracticeWeb_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToPracticeWeb_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToPracticeWeb_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToPracticeWeb_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToPracticeWeb_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToPracticeWeb_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToPracticeWeb_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToPracticeWeb_Appointment);
            procThreadmainLocalToPracticeWeb_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToPracticeWeb_Appointment()
        {
            if (bwSynchLocalToPracticeWeb_Appointment.IsBusy != true)
            {
                bwSynchLocalToPracticeWeb_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToPracticeWeb_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToPracticeWeb_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToPracticeWeb_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToPracticeWeb_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToPracticeWeb_Appointment.Enabled = true;
        }

        public void SynchDataLocalToPracticeWeb_Appointment()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        DataTable dtPracticeWebPatient = SynchPracticeWebBAL.GetPracticeWebPatientID_NameData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        DataTable dtIdelProv = SynchPracticeWebBAL.GetPracticeWebIdelProvider(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                        string tmpApptProv = "";
                        Int64 tmpPatient_id = 0;
                        Int64 tmpPatient_Gur_id = 0;
                        Int64 tmpAppt_EHR_id = 0;
                        Int64 tmpNewPatient = 1;

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

                            DataTable dtBookOperatoryApptWiseDateTime = SynchPracticeWebBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), dtDtxRow["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
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
                                    DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("OP = '" + tmpCheckOpId + "'");
                                    if (rowBookOpTime.Length > 0)
                                    {
                                        for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                        {
                                            appointment_EHR_id = rowBookOpTime[Bop]["AptNum"].ToString();
                                            if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
                                                && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                            {
                                                IsConflict = true;
                                                break;
                                            }
                                            else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
                                                && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
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

                            if (tmpIdealOperatory == 0)
                            {
                                DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("AptNum = " + appointment_EHR_id).CopyToDataTable();

                                bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[j]["Location_ID"].ToString());
                            }
                            else
                            {
                                tmpPatient_id = 0;
                                tmpPatient_Gur_id = 0;
                                tmpAppt_EHR_id = 0;
                                tmpNewPatient = 1;
                                TmpWebPatientName = "";


                                //TmpWebPatientName = dtDtxRow["First_Name"].ToString().Trim();
                                //TmpWebRevPatientName = dtDtxRow["Last_Name"].ToString().Trim();

                                if (dtDtxRow["Patient_Status"].ToString().ToLower().Trim() == "new")
                                {
                                    tmpNewPatient = 1;
                                }
                                else
                                {
                                    tmpNewPatient = 0;
                                }

                                Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);

                                tmpApptProv = dtDtxRow["Provider_EHR_ID"].ToString().Trim();

                                if (tmpApptProv == "" || tmpApptProv == "0" || tmpApptProv == "-")
                                {
                                    tmpApptProv = tmpIdelProv;
                                }

                                tmpPatient_id = 0;
                                if (dtDtxRow["Patient_EHR_Id"] != null && dtDtxRow["Patient_EHR_Id"].ToString() != string.Empty && dtDtxRow["Patient_EHR_Id"].ToString() != "-")
                                {
                                    tmpPatient_id = Convert.ToInt64(dtDtxRow["Patient_EHR_Id"].ToString());
                                }
                                if (tmpPatient_id == 0)
                                {
                                    tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtPracticeWebPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                                    //    DataRow[] row = dtPracticeWebPatient.Copy().Select("TRIM(Mobile) = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR TRIM(Home_Phone) = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR TRIM(Work_Phone) =  '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' ");
                                    //    if (row.Length > 0)
                                    //    {
                                    //        for (int i = 0; i < row.Length; i++)
                                    //        {
                                    //            if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                    //            {
                                    //                tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //            }
                                    //            else if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebRevPatientName.ToString().ToLower().Trim())
                                    //            {
                                    //                tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //            }
                                    //            else if (row[i]["FirstName"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                    //            {
                                    //                tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //            }
                                    //            else if (row[i]["FirstName"].ToString().ToLower().Trim() == dtDtxRow["First_Name"].ToString().Trim().ToLower())
                                    //            {
                                    //                tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //            }
                                    //            else if (row[i]["FirstName"].ToString().ToLower().Trim() == (TmpWebPatientName.ToString().IndexOf(" ") > 0 ? TmpWebPatientName.Substring(0, TmpWebPatientName.ToString().IndexOf(" ")).ToLower() : TmpWebPatientName))
                                    //            {
                                    //                tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //            }
                                    //            if (tmpPatient_id != 0)
                                    //            {
                                    //                break;
                                    //            }
                                    //        }

                                    //        tmpPatient_Gur_id = Convert.ToInt64(row[0]["Guarantor"].ToString());
                                    //    }
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

                                //    tmpPatient_id = SynchPracticeWebBAL.Save_Patient_Local_To_PracticeWeb(tmpLastName.Trim(),
                                //                                                                        tmpFirstName,
                                //                                                                        dtDtxRow["MI"].ToString().Trim(),
                                //                                                                        dtDtxRow["Mobile_Contact"].ToString().Trim(),
                                //                                                                        dtDtxRow["Email"].ToString().Trim(),
                                //                                                                        tmpApptProv,
                                //                                                                        dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                //                                                                        tmpPatient_Gur_id,
                                //                                                                            dtDtxRow["Birth_Date"].ToString().Trim(),
                                //                                                                            dtDtxRow["Clinic_Number"].ToString().Trim(),
                                //                                                                            Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                //}

                                if (tmpPatient_id > 0)
                                {
                                    //string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');

                                    //DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                                    //DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                                    //TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;

                                    //int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);

                                    //string tmpApptDurPatern = "";
                                    //for (int i = 0; i < tmpApptDurMinutes / 5; i++)
                                    //{
                                    //    tmpApptDurPatern = tmpApptDurPatern + "X";
                                    //}

                                    //DataTable dtBookOperatoryApptWiseDateTime = SynchPracticeWebBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
                                    //int tmpIdealOperatory = 0;

                                    //if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
                                    //{
                                    //    int tmpCheckOpId = 0;
                                    //    bool IsConflict = false;
                                    //    for (int i = 0; i < Operatory_EHR_IDs.Length; i++)
                                    //    {
                                    //        IsConflict = false;
                                    //        tmpCheckOpId = Convert.ToInt32(Operatory_EHR_IDs[i].ToString());
                                    //        DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("OP = '" + tmpCheckOpId + "'");
                                    //        if (rowBookOpTime.Length > 0)
                                    //        {
                                    //            for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                    //            {
                                    //                if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
                                    //                    && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                    //                {
                                    //                    IsConflict = true;
                                    //                    break;
                                    //                }
                                    //                else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
                                    //                    && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                    //                {
                                    //                    IsConflict = true;
                                    //                    break;
                                    //                }
                                    //            DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact =
                                    //        }
                                    //        if (IsConflict == false)
                                    //        {
                                    //            tmpIdealOperatory = tmpCheckOpId;
                                    //            break;
                                    //        }
                                    //    }
                                    //}

                                    //if (tmpIdealOperatory == 0)
                                    //{
                                    //    tmpIdealOperatory = Convert.ToInt32(Operatory_EHR_IDs[0].ToString());
                                    //}

                                    tmpAppt_EHR_id = SynchPracticeWebBAL.Save_Appointment_Local_To_PracticeWeb(tmpPatient_id.ToString(),
                                                                                                        (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "1"),
                                                                                                        tmpApptDurPatern,
                                                                                                             (dtDtxRow["confirmed_status_ehr_key"].ToString() == string.Empty ? "19" : dtDtxRow["confirmed_status_ehr_key"].ToString()),
                                                                                                        tmpIdealOperatory.ToString(),
                                                                                                        tmpApptProv,
                                                                                                        dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                                                                                        tmpNewPatient.ToString(),
                                                                                                        dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                                                                                        dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                        dtDtxRow["comment"].ToString().Trim(),
                                                                                                        dtDtxRow["Clinic_Number"].ToString().Trim(),
                                                                                                        (dtDtxRow["appt_treatmentcode"].ToString()),
                                                                                                        Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                                    if (tmpAppt_EHR_id > 0)
                                    {
                                        bool isApptId_Update = SynchPracticeWebBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    //  SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook();
                    ObjGoalBase.WriteToSyncLogFile("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #endregion

        #region Sync MedicalHistory Form

        private void fncSynchDataPracticeWeb_MedicalHistory()
        {
            InitBgWorkerPracticeWeb_MedicalHistory();
            InitBgTimerPracticeWeb_MedicalHistory();
        }

        private void InitBgTimerPracticeWeb_MedicalHistory()
        {
            timerSynchPracticeWeb_MedicalHistory = new System.Timers.Timer();
            this.timerSynchPracticeWeb_MedicalHistory.Interval = 1000 * GoalBase.intervalEHRSynch_MedicalHistory;
            this.timerSynchPracticeWeb_MedicalHistory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_MedicalHistory_Tick);
            timerSynchPracticeWeb_MedicalHistory.Enabled = true;
            timerSynchPracticeWeb_MedicalHistory.Start();
            timerSynchPracticeWeb_MedicalHistory_Tick(null, null);
        }

        private void InitBgWorkerPracticeWeb_MedicalHistory()
        {
            bwSynchPracticeWeb_MedicalHistory = new BackgroundWorker();
            bwSynchPracticeWeb_MedicalHistory.WorkerReportsProgress = true;
            bwSynchPracticeWeb_MedicalHistory.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_MedicalHistory.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_MedicalHistory_DoWork);
            bwSynchPracticeWeb_MedicalHistory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_MedicalHistory_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_MedicalHistory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_MedicalHistory.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_MedicalHistory();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_MedicalHistory()
        {
            System.Threading.Thread procThreadmainPracticeWeb_MedicalHistory = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_MedicalHistory);
            procThreadmainPracticeWeb_MedicalHistory.Start();
        }

        public void CallSyncOrderTablePracticeWeb_MedicalHistory()
        {
            if (bwSynchPracticeWeb_MedicalHistory.IsBusy != true)
            {
                bwSynchPracticeWeb_MedicalHistory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_MedicalHistory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_MedicalHistory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_MedicalHistory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_MedicalHistory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_MedicalHistory.Enabled = true;
        }

        public void SynchDataPracticeWeb_MedicalHistory()
        {
            CallSynch_PracticeWeb_MedicalHistory();

        }

        private void CallSynch_PracticeWeb_MedicalHistory()
        {
            if (!Is_synched_MedicalHistory && Utility.IsApplicationIdleTimeOff)
            {
                string tablename = "";
                try
                {
                    Is_synched_MedicalHistory = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataSet dsPracticeWebMedicalHistory = SynchPracticeWebBAL.GetPracticeWebMedicalHistoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        DataTable dtMedicalHistory = new DataTable();
                        string ignoreColumnsList = "", primaryColumnList = "";

                        foreach (DataTable dtTable in dsPracticeWebMedicalHistory.Tables)
                        {
                            DataTable dtLocalDbRecords = new DataTable();

                            dtTable.Columns.Add("InsUptDlt", typeof(int));
                            dtTable.Columns["InsUptDlt"].DefaultValue = 0;

                            dtLocalDbRecords = SynchLocalBAL.GetLocalMedicalHistoryRecords(dtTable.TableName.ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), true);
                            dtMedicalHistory = dtTable;
                            tablename = dtMedicalHistory.TableName.ToString();
                            #region Compare Records

                            if (!dtLocalDbRecords.Columns.Contains("InsUptDlt"))
                            {
                                dtLocalDbRecords.Columns.Add("InsUptDlt", typeof(int));
                                dtLocalDbRecords.Columns["InsUptDlt"].DefaultValue = 0;
                            }

                            if (dtMedicalHistory.TableName.ToString() == "OD_SheetDef")
                            {
                                ignoreColumnsList = "SheetDefNum_LocalDB_ID,SheetDefNum_Web_ID";
                                primaryColumnList = "SheetDefNum_LocalDB_ID";
                                //dtLocalDbRecords = CompareDataTablzeRecords(ref dtMedicalHistory, dtLocalDbRecords, "SheetDefNum_EHR_ID", "SheetDefNum_LocalDB_ID", "SheetDefNum_LocalDB_ID,SheetDefNum_Web_ID,EHR_Entry_DateTime, SheetDefNum_EHR_ID,PageCount,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt");
                                dtLocalDbRecords = CompareDataTableRecordsWithTwoColumns(ref dtMedicalHistory, dtLocalDbRecords, "SheetDefNum_EHR_ID", "Clinic_Number", "SheetDefNum_LocalDB_ID", "SheetDefNum_LocalDB_ID,SheetDefNum_Web_ID,EHR_Entry_DateTime, SheetDefNum_EHR_ID,PageCount,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Service_Install_Id");
                            }
                            else if (dtMedicalHistory.TableName.ToString() == "OD_SheetFieldDef")
                            {
                                ignoreColumnsList = "SheetFieldDefNum_LocalDB_ID,SheetFieldDefNum_Web_ID";
                                primaryColumnList = "SheetFieldDefNum_LocalDB_ID";
                                //dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "SheetFieldDefNum_EHR_ID", "SheetFieldDefNum_LocalDB_ID", "SheetFieldDefNum_LocalDB_ID,SheetFieldDefNum_Web_ID,EHR_Entry_DateTime,FontIsBold,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt");
                                dtLocalDbRecords = CompareDataTableRecordsWithTwoColumns(ref dtMedicalHistory, dtLocalDbRecords, "SheetFieldDefNum_EHR_ID", "Clinic_Number", "SheetFieldDefNum_LocalDB_ID", "SheetFieldDefNum_LocalDB_ID,SheetFieldDefNum_Web_ID,EHR_Entry_DateTime,FontIsBold,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Service_Install_Id");
                            }

                            dtMedicalHistory.AcceptChanges();

                            if (dtMedicalHistory != null && dtMedicalHistory.Rows.Count > 0 && dtMedicalHistory.AsEnumerable()
                                    .Where(o => Convert.ToInt16(o.Field<object>("InsUptDlt")) == 1 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 2 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 3 || Convert.ToInt16(o.Field<object>("InsUptDlt")) == 4).Count() > 0)
                            {
                                bool status = false;
                                DataTable dtSaveRecords = dtMedicalHistory.Clone();

                                if (dtMedicalHistory.Select("InsUptDlt IN (1,2)").Count() > 0 || dtLocalDbRecords.Select("InsUptDlt IN (3)").Count() > 0)
                                {
                                    if (dtMedicalHistory.Select("InsUptDlt IN (1,2)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtMedicalHistory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                                    }
                                    if (dtLocalDbRecords.Select("InsUptDlt IN (3)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtLocalDbRecords.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                                    }
                                    status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, dtMedicalHistory.TableName, ignoreColumnsList, primaryColumnList);
                                }
                                else
                                {
                                    if (dtMedicalHistory.Select("InsUptDlt IN (4)").Count() > 0)
                                    {
                                        status = true;
                                    }
                                }
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime(tablename);
                                    ObjGoalBase.WriteToSyncLogFile("" + tablename + " Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    IsTrackerOperatorySync = true;
                                    //SynchDataLiveDB_Push_MedicalHisotryTables(dtMedicalHistory.TableName.ToString());
                                }
                                else
                                {
                                    IsTrackerOperatorySync = false;
                                }
                            }
                            #endregion
                        }
                        SynchDataLiveDB_Push_MedicalHisotryTables("PracticeWeb_SheetDef");
                        SynchDataLiveDB_Push_MedicalHisotryTables("PracticeWeb_SheetFieldDef");

                    }
                    Is_synched_MedicalHistory = false;
                }
                catch (Exception ex)
                {
                    Is_synched_MedicalHistory = false;
                    ObjGoalBase.WriteToErrorLogFile("[" + tablename + " Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch PatientWiseRecall

        //private void fncSynchDataPracticeWeb_PatientWiseRecallDate()
        //{
        //    InitBgWorkerPracticeWeb_PatientWiseRecallDate();
        //    InitBgTimerPracticeWeb_PatientWiseRecallDate();
        //}

        //private void InitBgTimerPracticeWeb_PatientWiseRecallDate()
        //{
        //    timerSynchPracticeWeb_PatientWiseRecallDate = new System.Timers.Timer();
        //    this.timerSynchPracticeWeb_PatientWiseRecallDate.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
        //    this.timerSynchPracticeWeb_PatientWiseRecallDate.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_PatientWiseRecallDate_Tick);
        //    timerSynchPracticeWeb_PatientWiseRecallDate.Enabled = true;
        //    timerSynchPracticeWeb_PatientWiseRecallDate.Start();
        //    timerSynchPracticeWeb_PatientWiseRecallDate_Tick(null, null);
        //}

        //private void InitBgWorkerPracticeWeb_PatientWiseRecallDate()
        //{
        //    bwSynchPracticeWeb_PatientWiseRecallDate = new BackgroundWorker();
        //    bwSynchPracticeWeb_PatientWiseRecallDate.WorkerReportsProgress = true;
        //    bwSynchPracticeWeb_PatientWiseRecallDate.WorkerSupportsCancellation = true;
        //    bwSynchPracticeWeb_PatientWiseRecallDate.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_PatientWiseRecallDate_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchPracticeWeb_PatientWiseRecallDate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_PatientWiseRecallDate_RunWorkerCompleted);
        //}

        //private void timerSynchPracticeWeb_PatientWiseRecallDate_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {

        //        timerSynchPracticeWeb_PatientWiseRecallDate.Enabled = false;
        //        MethodForCallSynchOrderPracticeWeb_PatientWiseRecallDate();
        //    }
        //}

        //public void MethodForCallSynchOrderPracticeWeb_PatientWiseRecallDate()
        //{
        //    System.Threading.Thread procThreadmainPracticeWeb_PatientWiseRecallDate = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_PatientWiseRecallDate);
        //    procThreadmainPracticeWeb_PatientWiseRecallDate.Start();
        //}

        //public void CallSyncOrderTablePracticeWeb_PatientWiseRecallDate()
        //{
        //    if (bwSynchPracticeWeb_PatientWiseRecallDate.IsBusy != true)
        //    {
        //        bwSynchPracticeWeb_PatientWiseRecallDate.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchPracticeWeb_PatientWiseRecallDate_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchPracticeWeb_PatientWiseRecallDate.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }

        //        SynchDataPracticeWeb_PatientWiseRecallDate(); 
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchPracticeWeb_PatientWiseRecallDate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchPracticeWeb_PatientWiseRecallDate.Enabled = true;
        //}

        //public void SynchDataPracticeWeb_PatientWiseRecallDate()
        //{

        //    try
        //    {

        //        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //        {
        //            DataTable dtPracticeWebPatientWiseRecallDate = SynchPracticeWebBAL.GetPracticeWebPatientWiseRecallDate(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //            DataTable dtLocalRecallType = SynchLocalBAL.GetLocalPatientWiseRecallTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //            foreach (DataRow dtDtxRow in dtPracticeWebPatientWiseRecallDate.Rows)
        //            {
        //                DataRow[] row = dtLocalRecallType.Copy().Select("Patient_Recall_Id = '" + dtDtxRow["Patient_Recall_Id"] + "'");
        //                if (row.Length > 0)
        //                {
        //                    if (dtDtxRow["RecallType_Name"].ToString().ToLower().Trim() != row[0]["RecallType_Name"].ToString().ToLower().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Default_Recall"].ToString().ToLower().Trim() != row[0]["Default_Recall"].ToString().ToLower().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Recall_Date"].ToString() != "" && row[0]["Recall_Date"].ToString() != "" && Convert.ToDateTime(dtDtxRow["Recall_Date"].ToString().Trim()) != Convert.ToDateTime(row[0]["Recall_Date"].ToString().Trim()))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 0;
        //                    }

        //                }
        //                else
        //                {
        //                    dtDtxRow["InsUptDlt"] = 1;
        //                }
        //            }


        //            dtPracticeWebPatientWiseRecallDate.AcceptChanges();

        //            if (dtPracticeWebPatientWiseRecallDate != null && dtPracticeWebPatientWiseRecallDate.Rows.Count > 0)
        //            {
        //                bool status = SynchEaglesoftBAL.SavePatientWiseRecallType_Eaglesoft_To_Local(dtPracticeWebPatientWiseRecallDate, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                if (status)
        //                {
        //                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientRecallType");
        //                    ObjGoalBase.WriteToSyncLogFile("PatientRecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                    //SynchDataLiveDB_PushPatientWiseRecallType();
        //                }
        //                else
        //                {
        //                    ObjGoalBase.WriteToErrorLogFile("[PatientRecallType Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile("[PatientRecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //    }
        //    finally
        //    {

        //    }
        //    // }
        //}

        #endregion

        //rooja add new master for insurance
        #region Synch Insurance

        private void fncSynchDataPracticeWeb_Insurance()
        {
            InitBgWorkerPracticeWeb_Insurance();
            InitBgTimerPracticeWeb_Insurance();
        }

        private void InitBgTimerPracticeWeb_Insurance()
        {
            timerSynchPracticeWeb_Insurance = new System.Timers.Timer();
            this.timerSynchPracticeWeb_Insurance.Interval = 1000 * GoalBase.intervalEHRSynch_Insurance;
            this.timerSynchPracticeWeb_Insurance.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchPracticeWeb_Insurance_Tick);
            timerSynchPracticeWeb_Insurance.Enabled = true;
            timerSynchPracticeWeb_Insurance.Start();
        }

        private void InitBgWorkerPracticeWeb_Insurance()
        {
            bwSynchPracticeWeb_Insurance = new BackgroundWorker();
            bwSynchPracticeWeb_Insurance.WorkerReportsProgress = true;
            bwSynchPracticeWeb_Insurance.WorkerSupportsCancellation = true;
            bwSynchPracticeWeb_Insurance.DoWork += new DoWorkEventHandler(bwSynchPracticeWeb_Insurance_DoWork);
            bwSynchPracticeWeb_Insurance.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchPracticeWeb_Insurance_RunWorkerCompleted);
        }

        private void timerSynchPracticeWeb_Insurance_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchPracticeWeb_Insurance.Enabled = false;
                MethodForCallSynchOrderPracticeWeb_Insurance();
            }
        }

        public void MethodForCallSynchOrderPracticeWeb_Insurance()
        {
            System.Threading.Thread procThreadmainPracticeWeb_Insurance = new System.Threading.Thread(this.CallSyncOrderTablePracticeWeb_Insurance);
            procThreadmainPracticeWeb_Insurance.Start();
        }

        public void CallSyncOrderTablePracticeWeb_Insurance()
        {
            if (bwSynchPracticeWeb_Insurance.IsBusy != true)
            {
                bwSynchPracticeWeb_Insurance.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchPracticeWeb_Insurance_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchPracticeWeb_Insurance.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataPracticeWeb_Insurance();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchPracticeWeb_Insurance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchPracticeWeb_Insurance.Enabled = true;
        }

        public void SynchDataPracticeWeb_Insurance()
        {
            try
            {
                if (!Is_synched_Insurance && Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_Insurance = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPracticeWebInsurance = SynchPracticeWebBAL.GetPracticeWebInsuranceData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        dtPracticeWebInsurance.Columns.Add("InsUptDlt", typeof(int));

                        DataTable dtLocalInsurance = SynchLocalBAL.GetLocalInsuranceData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtPracticeWebInsurance.Rows)
                        {
                            DataRow[] row = dtLocalInsurance.Copy().Select("Insurance_EHR_ID = '" + dtDtxRow["CarrierNum"] + "' ");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["CarrierName"].ToString().ToLower().Trim() != row[0]["Insurance_Name"].ToString().ToLower().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Address"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
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
                                else if (dtDtxRow["State"].ToString().ToLower().Trim() != row[0]["State"].ToString().ToLower().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Zip"].ToString().ToLower().Trim() != row[0]["Zipcode"].ToString().ToLower().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Phone"].ToString().ToLower().Trim() != row[0]["Phone"].ToString().ToLower().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["ElectId"].ToString().ToLower().Trim() != row[0]["ElectId"].ToString().ToLower().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 0;
                                }
                                //else if (dtDtxRow["EmployerName"].ToString().ToLower().Trim() != row[0]["EmployerName"].ToString().ToLower().Trim())
                                //{
                                //    dtDtxRow["InsUptDlt"] = 2;
                                //}
                                //if (dtDtxRow["IsHidden"].ToString().ToLower().Trim() != row[0]["Is_Deleted"].ToString().ToLower().Trim())
                                //{
                                //    dtDtxRow["InsUptDlt"] = 3;
                                //}

                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        foreach (DataRow dtDtxRow in dtLocalInsurance.Rows)
                        {
                            DataRow[] row = dtPracticeWebInsurance.Copy().Select("CarrierNum = '" + dtDtxRow["Insurance_EHR_ID"] + "' ");
                            if (row.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtPracticeWebInsurance.NewRow();
                                BlcOptDtldr["CarrierNum"] = dtDtxRow["Insurance_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["CarrierName"] = dtDtxRow["Insurance_Name"].ToString().Trim();
                                // BlcOptDtldr["Clinic_Number"] = dtDtxRow["Clinic_Number"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtPracticeWebInsurance.Rows.Add(BlcOptDtldr);
                            }
                        }

                        dtPracticeWebInsurance.AcceptChanges();

                        if (dtPracticeWebInsurance != null && dtPracticeWebInsurance.Rows.Count > 0)
                        {
                            bool status = SynchPracticeWebBAL.Save_Insurance_PracticeWeb_To_Local(dtPracticeWebInsurance, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Insurance");
                                ObjGoalBase.WriteToSyncLogFile("Insurance Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_Insurance();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Insurance Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                    }
                    Is_synched_Insurance = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_Insurance = false;
                ObjGoalBase.WriteToErrorLogFile("[Insurance Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Event Listener
        public static bool SynchDataLocalToPracticeWeb_Appointment(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id)
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
                //for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                //{
                DataTable dtPracticeWebPatient = SynchPracticeWebBAL.GetPracticeWebPatientID_NameData(strDbConnString);
                DataTable dtIdelProv = SynchPracticeWebBAL.GetPracticeWebIdelProvider(strDbConnString);

                string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                string tmpApptProv = "";
                Int64 tmpPatient_id = 0;
                Int64 tmpPatient_Gur_id = 0;
                Int64 tmpAppt_EHR_id = 0;
                Int64 tmpNewPatient = 1;

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

                    DataTable dtBookOperatoryApptWiseDateTime = SynchPracticeWebBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), dtDtxRow["Clinic_Number"].ToString(), strDbConnString);
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
                            DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("OP = '" + tmpCheckOpId + "'");
                            if (rowBookOpTime.Length > 0)
                            {
                                for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                {
                                    appointment_EHR_id = rowBookOpTime[Bop]["AptNum"].ToString();
                                    if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
                                        && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                    {
                                        IsConflict = true;
                                        break;
                                    }
                                    else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
                                        && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
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

                    if (tmpIdealOperatory == 0)
                    {
                        DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("AptNum = " + appointment_EHR_id).CopyToDataTable();

                        bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id, dtTemp, appointment_EHR_id, Location_ID);
                    }
                    else
                    {
                        tmpPatient_id = 0;
                        tmpPatient_Gur_id = 0;
                        tmpAppt_EHR_id = 0;
                        tmpNewPatient = 1;
                        TmpWebPatientName = "";

                        if (dtDtxRow["Patient_Status"].ToString().ToLower().Trim() == "new")
                        {
                            tmpNewPatient = 1;
                        }
                        else
                        {
                            tmpNewPatient = 0;
                        }

                        Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);

                        tmpApptProv = dtDtxRow["Provider_EHR_ID"].ToString().Trim();

                        if (tmpApptProv == "" || tmpApptProv == "0" || tmpApptProv == "-")
                        {
                            tmpApptProv = tmpIdelProv;
                        }

                        tmpPatient_id = 0;
                        if (dtDtxRow["Patient_EHR_Id"] != null && dtDtxRow["Patient_EHR_Id"].ToString() != string.Empty && dtDtxRow["Patient_EHR_Id"].ToString() != "-")
                        {
                            tmpPatient_id = Convert.ToInt64(dtDtxRow["Patient_EHR_Id"].ToString());
                        }
                        if (tmpPatient_id == 0)
                        {
                            tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtPracticeWebPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), strDbConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                        }

                        if (tmpPatient_id > 0)
                        {


                            tmpAppt_EHR_id = SynchPracticeWebBAL.Save_Appointment_Local_To_PracticeWeb(tmpPatient_id.ToString(),
                                                                                                (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "1"),
                                                                                                tmpApptDurPatern,
                                                                                                     (dtDtxRow["confirmed_status_ehr_key"].ToString() == string.Empty ? "19" : dtDtxRow["confirmed_status_ehr_key"].ToString()),
                                                                                                tmpIdealOperatory.ToString(),
                                                                                                tmpApptProv,
                                                                                                dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                                                                                tmpNewPatient.ToString(),
                                                                                                dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                                                                                dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                dtDtxRow["comment"].ToString().Trim(),
                                                                                                dtDtxRow["Clinic_Number"].ToString().Trim(),
                                                                                                (dtDtxRow["appt_treatmentcode"].ToString()),
                                                                                                strDbConnString);

                            if (tmpAppt_EHR_id > 0)
                            {
                                bool isApptId_Update = SynchPracticeWebBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id);
                            }
                        }

                        #region Appointment Sync
                        SyncDataPracticeWeb_AppointmentFromEvent(strDbConnString, Clinic_Number, Service_Install_Id, tmpAppt_EHR_id.ToString(), tmpPatient_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                        #endregion
                    }
                }
                return true;
                //}
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                return false;
            }
        }

        public static void SyncDataPracticeWeb_AppointmentFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        {
            try
            {
                try
                {
                    SyncPracticeweb_PatientFromEvent(strDbString, strPatID, Clinic_Number, Service_Install_Id);
                    SyncPracticeWeb_PatientStatusFromEvent(strDbString, strPatID, Clinic_Number, Service_Install_Id);
                    SynchDataLiveDB_Push_Patient(strPatID);
                }
                catch (Exception exPat)
                {
                    Utility.WritetoAditEventErrorLogFile_Static("[SyncDataPracticeWeb_AppointmentFromEvent Error in Appointment Patient Sync]:" + exPat.Message);
                }

                DataTable dtPracticeWebAppointment = SynchPracticeWebBAL.GetPracticeWebAppointmentData(strDbString, strApptID);
                dtPracticeWebAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                dtPracticeWebAppointment.Columns.Add("InsUptDlt", typeof(int));
                DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Service_Install_Id, strApptID);

                foreach (DataRow dtDtxRow in dtPracticeWebAppointment.Rows)
                {
                    try
                    {
                        DataRow[] row = dtLocalAppointment.Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            if (Convert.ToDateTime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EHR_Entry_DateTime"]))
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Last_Name"].ToString().ToLower().Trim() != row[0]["Last_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["MI"].ToString().ToLower().Trim() != row[0]["MI"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Utility.ConvertContactNumber(dtDtxRow["Home_Contact"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").ToLower().Trim()) != Utility.ConvertContactNumber(row[0]["Home_Contact"].ToString().ToLower().Trim()))
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().ToLower().Trim()) != Utility.ConvertContactNumber(row[0]["Mobile_Contact"].ToString().ToLower().Trim()))
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Email"].ToString().ToLower().Trim() != row[0]["Email"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Address"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["City"].ToString().ToLower().Trim() != row[0]["City"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ST"].ToString().ToLower().Trim() != row[0]["ST"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["is_asap"] != null && dtDtxRow["is_asap"].ToString() != string.Empty && Convert.ToBoolean(dtDtxRow["is_asap"]) != Convert.ToBoolean(row[0]["is_asap"]))
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Zip"].ToString().ToLower().Trim() != row[0]["Zip"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ProcedureDesc"].ToString().ToLower().Trim() != row[0]["ProcedureDesc"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ProcedureCode"].ToString().ToLower().Trim() != row[0]["ProcedureCode"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Clinic_Number"].ToString().ToLower().Trim() != row[0]["Clinic_Number"].ToString().ToLower().Trim())
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
                            DataRow[] rowCon = dtLocalAppointment.Select("Mobile_Contact = '" + Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) + "'  AND ISNULL(Appt_EHR_ID, '0') = '0' ");
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
                    catch (Exception ex)
                    {
                        Utility.WritetoAditEventErrorLogFile_Static("[SyncDataPracticeWeb_AppointmentFromEvent (" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + ") Sync (PracticeWeb to Local Database) ]" + ex.Message);
                    }

                    dtPracticeWebAppointment.AcceptChanges();
                    if (dtPracticeWebAppointment != null && dtPracticeWebAppointment.Rows.Count > 0 && dtPracticeWebAppointment.Select("InsUptDlt IN (1,5,4,7,3,6)").Count() > 0)
                    {
                        DataTable dtResult = dtPracticeWebAppointment.Select("InsUptDlt IN (1,5,4,7,3,6)").CopyToDataTable();

                        if (!dtResult.Columns.Contains("Appt_Web_ID"))
                        {
                            dtResult.Columns.Add("Appt_Web_ID");
                        }
                        dtResult.Rows[0]["Appt_Web_ID"] = strWebID;

                        bool status = SynchPracticeWebBAL.Save_Appointment_PracticeWeb_To_Local(dtResult, Service_Install_Id);
                    }

                    //bool temp = Utility.IsExternalAppointmentSync;
                    //Utility.IsExternalAppointmentSync = true;
                    SynchDataLiveDB_Push_Appointment(strApptID);
                    //Utility.IsExternalAppointmentSync = temp;                    
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncDataPracticeWeb_AppointmentFromEvent Sync (PracticeWeb to Local Database) ]" + ex.Message);
            }
        }

        public static void SyncPracticeweb_PatientFromEvent(string strDbString, string strPatID, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                #region "Appointment Patient"
                DataTable dtLocalPracticeWebLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();
                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Service_Install_Id, strPatID);
                DataTable dtPracticeWebAppointmensPatient = SynchPracticeWebBAL.GetPracticeWebPatientData(Clinic_Number, strDbString, strPatID);
                var updateLanguageQuery = from r1 in dtPracticeWebAppointmensPatient.AsEnumerable()
                                          join r2 in dtLocalPracticeWebLanguageList.AsEnumerable()
                                          on r1.Field<string>("PreferredLanguage") equals r2.Field<string>("Language_Short_Name")
                                          select new { r1, r2 };
                foreach (var x in updateLanguageQuery)
                {
                    x.r1.SetField("PreferredLanguage", x.r2.Field<string>("Language_Name"));
                }

                DataTable dtSaveRecords = new DataTable();
                dtSaveRecords = dtLocalPatient.Clone();

                var itemsToBeAdded = (from PracticeWebPatient in dtPracticeWebAppointmensPatient.AsEnumerable()
                                      join LocalPatient in dtLocalPatient.AsEnumerable()
                                      on PracticeWebPatient["Patient_EHR_ID"].ToString().Trim() + "_" + PracticeWebPatient["Clinic_Number"].ToString().Trim()
                                      equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                      //on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                      //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                      into matchingRows
                                      from matchingRow in matchingRows.DefaultIfEmpty()
                                      where matchingRow == null
                                      select PracticeWebPatient).ToList();
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
                    //ObjGoalBase.WriteToSyncLogFile_All("Table to Save Record count after insert :" + dtSaveRecords.Rows.Count);
                }

                var itemsToBeUpdated = (from PracticeWebPatient in dtPracticeWebAppointmensPatient.AsEnumerable()
                                        join LocalPatient in dtLocalPatient.AsEnumerable()
                                        on PracticeWebPatient["Patient_EHR_ID"].ToString().Trim() + "_" + PracticeWebPatient["Clinic_Number"].ToString().Trim()
                                        equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                        where
                                         Convert.ToDateTime(PracticeWebPatient["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(LocalPatient["EHR_Entry_DateTime"])
                                         ||
                                         (PracticeWebPatient["nextvisit_date"] != DBNull.Value && PracticeWebPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(PracticeWebPatient["nextvisit_date"]) : DateTime.Now)
                                         !=
                                         (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)
                                         ||
                                         (PracticeWebPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())
                                         ||
                                         (PracticeWebPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())
                                         || (PracticeWebPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                         || (PracticeWebPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                         || (PracticeWebPatient["Home_Phone"].ToString().Trim()) != (LocalPatient["Home_Phone"].ToString().Trim())
                                         || (PracticeWebPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                         || (PracticeWebPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                         || (PracticeWebPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                         || (PracticeWebPatient["Mobile"].ToString().Trim()) != (LocalPatient["Mobile"].ToString().Trim())
                                         || (PracticeWebPatient["ReceiveSMS"].ToString().Trim()) != (LocalPatient["ReceiveSMS"].ToString().Trim())
                                         || (PracticeWebPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
                                        select PracticeWebPatient).ToList();

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
                    bool status = SynchPracticeWebBAL.Save_Patient_PracticeWeb_To_Local_New(dtSaveRecords, Clinic_Number, Service_Install_Id);
                }

                #endregion
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncPracticeweb_Patient Sync (PracticeWeb to Local Database) ]" + ex.Message);
            }
        }

        public static void SyncPracticeWeb_PatientStatusFromEvent(string strDbString, string strPatID, string Clinic_Number, string Service_Install_Id)
        {
            try
            {
                #region "Patient Status"
                DataTable dtPracticeWebPatientStatus = new DataTable();
                dtPracticeWebPatientStatus = SynchPracticeWebBAL.GetPracticeWebPatientStatusData(Clinic_Number, strDbString, strPatID);
                if (dtPracticeWebPatientStatus != null && dtPracticeWebPatientStatus.Rows.Count > 0)
                {
                    SynchLocalBAL.UpdatePatient_Status(dtPracticeWebPatientStatus, Service_Install_Id, Clinic_Number, strPatID);
                    //SynchDataLiveDB_Push_PatientStatus(Convert.ToInt32(Service_Install_Id), Convert.ToInt32(Clinic_Number), strPatID);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncPracticeWeb_PatientStatus Sync (PracticeWeb to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataLocalToPracticeWeb_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
        {
            string strDbConnString = "";
            string Location_ID = "";
            string strDocumentPath = "";
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_Id + "'");
                strDbConnString = dbConStr[0]["DBConnString"].ToString();

                var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                Location_ID = LocID[0]["Location_ID"].ToString();
                strDocumentPath = dbConStr[0]["Document_Path"].ToString();

                //CheckEntryUserLoginIdExist();

                DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData(Service_Install_Id, strPatientFormID);
                dtWebPatient_Form.Columns.Add(new DataColumn("Table_Name", typeof(string)));
                if (dtWebPatient_Form != null)
                {
                    if (dtWebPatient_Form.Rows.Count > 0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                    }
                }
                foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                {
                    dtDtxRow["Table_Name"] = "patient";

                    if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "FIRST_NAME")
                    {
                        dtDtxRow["ehrfield"] = "FName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "LAST_NAME")
                    {
                        dtDtxRow["ehrfield"] = "LName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MOBILE")
                    {
                        dtDtxRow["ehrfield"] = "WirelessPhone";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ADDRESS_ONE")
                    {
                        dtDtxRow["ehrfield"] = "Address";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ADDRESS_TWO")
                    {
                        dtDtxRow["ehrfield"] = "Address2";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "BIRTH_DATE")
                    {
                        dtDtxRow["ehrfield"] = "Birthdate";
                        try
                        {
                            dtDtxRow["ehrfield_value"] = Convert.ToDateTime(dtDtxRow["ehrfield_value"].ToString().Trim()).ToString("yyyy-MM-dd");

                        }
                        catch (Exception)
                        {
                            dtDtxRow["ehrfield_value"] = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "CITY")
                    {
                        dtDtxRow["ehrfield"] = "City";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMAIL")
                    {
                        dtDtxRow["ehrfield"] = "Email";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "HOME_PHONE")
                    {
                        dtDtxRow["ehrfield"] = "HmPhone";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MARITAL_STATUS")
                    {
                        dtDtxRow["ehrfield"] = "Position";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "SINGLE")
                        {
                            dtDtxRow["ehrfield_value"] = 0;
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MARRIED")
                        {
                            dtDtxRow["ehrfield_value"] = 1;
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "CHILD")
                        {
                            dtDtxRow["ehrfield_value"] = 2;
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "WIDOWED")
                        {
                            dtDtxRow["ehrfield_value"] = 3;
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "DIVORCED")
                        {
                            dtDtxRow["ehrfield_value"] = 4;
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = 0;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "MIDDLE_NAME")
                    {
                        dtDtxRow["ehrfield"] = "MiddleI";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PREFERRED_NAME")
                    {
                        dtDtxRow["ehrfield"] = "preferred";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRI_PROVIDER_ID")
                    {
                        dtDtxRow["ehrfield"] = "PriProv";
                        try
                        {
                            dtDtxRow["ehrfield_value"] = Convert.ToInt64(dtDtxRow["ehrfield_value"].ToString().Trim());
                        }
                        catch (Exception)
                        {
                            dtDtxRow["ehrfield_value"] = 1;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE")
                    {
                        dtDtxRow["ehrfield"] = "PRIMARY_INSURANCE";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_INSURANCE_COMPANYNAME")
                    {
                        dtDtxRow["ehrfield"] = "PRIMARY_INSURANCE_COMPANYNAME";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                    {
                        dtDtxRow["ehrfield"] = "PRIMARY_INS_SUBSCRIBER_ID";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "RECEIVE_EMAIL")
                    {
                        dtDtxRow["ehrfield"] = "TxtMsgOk";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO")
                        {
                            dtDtxRow["ehrfield_value"] = "2";
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = "1";
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "RECEIVE_SMS")
                    {
                        dtDtxRow["ehrfield"] = "TxtMsgOk";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "NO")
                        {
                            dtDtxRow["ehrfield_value"] = "2";
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = "1";
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SALUTATION")
                    {
                        dtDtxRow["ehrfield"] = "Salutation";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SEC_PROVIDER_ID")
                    {
                        dtDtxRow["ehrfield"] = "SecProv";
                        try
                        {
                            dtDtxRow["ehrfield_value"] = Convert.ToInt64(dtDtxRow["ehrfield_value"].ToString().Trim());
                        }
                        catch (Exception)
                        {
                            dtDtxRow["ehrfield_value"] = 1;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE")
                    {
                        dtDtxRow["ehrfield"] = "SECONDARY_INSURANCE";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_INSURANCE_COMPANYNAME")
                    {
                        dtDtxRow["ehrfield"] = "SECONDARY_INSURANCE_COMPANYNAME";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                    {
                        dtDtxRow["ehrfield"] = "SECONDARY_INS_SUBSCRIBER_ID";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SEX")
                    {
                        dtDtxRow["ehrfield"] = "Gender";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                        {
                            dtDtxRow["ehrfield_value"] = 1;
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = 0;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "WORK_PHONE")
                    {
                        dtDtxRow["ehrfield"] = "WkPhone";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "STATE")
                    {
                        dtDtxRow["ehrfield"] = "State";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "ZIPCODE")
                    {
                        dtDtxRow["ehrfield"] = "Zip";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SSN")
                    {
                        dtDtxRow["ehrfield"] = "SSN";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SCHOOL")
                    {
                        dtDtxRow["ehrfield"] = "SchoolName";
                    }

                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNAME")
                    {
                        dtDtxRow["ehrfield"] = "ICEName";
                        dtDtxRow["Table_Name"] = "patientnote";


                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMERGENCYCONTACTNUMBER")
                    {
                        dtDtxRow["ehrfield"] = "ICEPhone";
                        dtDtxRow["Table_Name"] = "patientnote";

                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "EMPLOYER")
                    {
                        dtDtxRow["ehrfield"] = "EmpName";
                        dtDtxRow["Table_Name"] = "employer";

                    }

                    dtWebPatient_Form.AcceptChanges();
                }
                if (dtWebPatient_Form.Rows.Count > 0)
                {
                    bool Is_Record_Update = SynchPracticeWebBAL.Save_Patient_Form_Local_To_PracticeWeb(dtWebPatient_Form, strDbConnString, Service_Install_Id);
                }

                try
                {
                    GetMedicalOpenDentalHistoryRecords(Service_Install_Id, strPatientFormID);
                    SynchPracticeWebBAL.SaveMedicalHistoryLocalToPracticeWeb(strDbConnString, Service_Install_Id, strPatientFormID);
                    ObjGoalBase.WriteToSyncLogFile("Medical_History_Save Sync ( Service Install Id : " + Service_Install_Id + ".  Local Database To " + Utility.Application_Name + ") Successfully.");
                }
                catch (Exception ex2)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Medical_History_Save Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex2.Message);
                }

                try
                {
                    if (SynchPracticeWebBAL.SavePatientDisease(strDbConnString, Service_Install_Id, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient_Alert Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception ex1)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                }
                try
                {
                    if (SynchPracticeWebBAL.DeletePatientDisease(strDbConnString, Service_Install_Id, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Delete_Patient_Alert Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Alert Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception ex1)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Alert Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                }

                bool isRecordSaved = false, isRecordDeleted = false;
                string Patient_EHR_IDS = "";
                string DeletePatientEHRID = "";
                string SavePatientEHRID = "";
                try
                {
                    if (SynchPracticeWebBAL.DeletePatientMedication(strDbConnString, Service_Install_Id, ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Delete Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception ex1)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                }
                try
                {
                    if (SynchPracticeWebBAL.SavePatientMedication(strDbConnString, Service_Install_Id, ref isRecordSaved, ref SavePatientEHRID, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception ex1)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                }
                if (isRecordSaved || isRecordDeleted)
                {
                    Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                    if (Patient_EHR_IDS != "")
                    {
                        SynchDataPracticeWeb_PatientMedication(Patient_EHR_IDS);
                    }
                }

                #region PatientInformation Document
                try
                {
                    string strPatientID = "";
                    strPatientID = SynchLocalBAL.Get_Patient_EHR_ID_from_Patient_Form(strPatientFormID);
                    GetPatientDocument(Service_Install_Id, strPatientFormID);
                    GetPatientDocument_New(Service_Install_Id, strPatientFormID);
                    SynchPracticeWebBAL.Save_Document_in_PracticeWeb(strDbConnString, Service_Install_Id, strDocumentPath, strPatientFormID, strPatientID);
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
                #endregion

                string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing(Service_Install_Id, strPatientFormID);
                if (Call_Importing.ToLower() != "success")
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Importing status. Service Install Id : " + Service_Install_Id + " : " + Call_Importing);
                }

                string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed(Service_Install_Id, strPatientFormID);
                if (Call_Completed.ToLower() != "success")
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Completed status.Service Install Id : " + Service_Install_Id + " : " + Call_Completed);
                }

                string Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed(Service_Install_Id, Location_ID, strPatientFormID);
                try
                {
                    if (Call_PatientPortalCompleted != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile("[Patient_Portal API called with Completed status : " + Call_PatientPortalCompleted);
                    }
                }
                catch (Exception)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                }
                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") Successfully.");


            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }
        #endregion
        
    }
}
