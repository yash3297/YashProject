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

        public bool IsOpenDentalProviderSync = false;
        public bool IsOpenDentalOperatorySync = false;
        public bool IsOpenDentalApptTypeSync = false;

        private BackgroundWorker bwSynchOpenDental_Appointment = null;
        private System.Timers.Timer timerSynchOpenDental_Appointment = null;

        private BackgroundWorker bwSynchOpenDental_OperatoryEvent = null;
        private System.Timers.Timer timerSynchOpenDental_OperatoryEvent = null;

        private BackgroundWorker bwSynchOpenDental_Provider = null;
        private System.Timers.Timer timerSynchOpenDental_Provider = null;

        private BackgroundWorker bwSynchOpenDental_Disease = null;
        private System.Timers.Timer timerSynchOpenDental_Disease = null;

        private BackgroundWorker bwSynchOpenDental_ProviderHours = null;
        private System.Timers.Timer timerSynchOpenDental_ProviderHours = null;

        private BackgroundWorker bwSynchOpenDental_PatientPayment = null;
        private System.Timers.Timer timerSynchOpenDental_PatientPayment = null;

        private BackgroundWorker bwSynchOpenDental_Speciality = null;
        private System.Timers.Timer timerSynchOpenDental_Speciality = null;

        private BackgroundWorker bwSynchOpenDental_Operatory = null;
        private System.Timers.Timer timerSynchOpenDental_Operatory = null;

        private BackgroundWorker bwSynchOpenDental_ApptType = null;
        private System.Timers.Timer timerSynchOpenDental_ApptType = null;

        private BackgroundWorker bwSynchOpenDental_Patient = null;
        private System.Timers.Timer timerSynchOpenDental_Patient = null;

        private BackgroundWorker bwSynchOpenDental_RecallType = null;
        private System.Timers.Timer timerSynchOpenDental_RecallType = null;

        private BackgroundWorker bwSynchOpendental_User = null;
        private System.Timers.Timer timerSynchOpendental_User = null;

        private BackgroundWorker bwSynchOpenDental_ApptStatus = null;
        private System.Timers.Timer timerSynchOpenDental_ApptStatus = null;

        private BackgroundWorker bwSynchOpenDental_Holiday = null;
        private System.Timers.Timer timerSynchOpenDental_Holiday = null;

        //  private BackgroundWorker bwSynchOpenDental_PatientWiseRecallDate = null;
        // private System.Timers.Timer timerSynchOpenDental_PatientWiseRecallDate = null;

        private BackgroundWorker bwSynchLocalToOpenDental_Appointment = null;
        private System.Timers.Timer timerSynchLocalToOpenDental_Appointment = null;

        private BackgroundWorker bwSynchLocalToOpenDental_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToOpenDental_Patient_Form = null;

        private BackgroundWorker bwSynchOpenDental_MedicalHistory = null;
        private System.Timers.Timer timerSynchOpenDental_MedicalHistory = null;

        private BackgroundWorker bwSynchOpenDental_Insurance = null;
        private System.Timers.Timer timerSynchOpenDental_Insurance = null;

        #endregion

        private void CallSynchOpenDentalToLocal()
        {
            if (Utility.AditSync)
            {
                Utility.WriteToSyncLogFile_All("Start Application Sync");
                //SynchDataOpenDental_Patient_New();
                //SynchDataPatientSMSCall_LocalTOOpenDental();
                //SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                //SynchDataLiveDB_Pull_PatientFollowUp();
                //SynchDataPatientSMSCall_LocalTOOpenDental();

                //SynchDataOpenDental_FolderList();

                // SynchDataLocalToOpenDental_PatientPayment();
                fncSynchDataOpenDental_PatientPayment();

                SynchDataOpenDental_Speciality();
                fncSynchDataOpenDental_Speciality();

                SynchDataOpenDental_Operatory();
                fncSynchDataOpenDental_Operatory();

                SynchDataOpenDental_Provider();
                fncSynchDataOpenDental_Provider();

                // SynchDataLocalToOpenDental_PatientPayment();
                //fncSynchDataOpenDental_PatientPayment();

                SynchDataOpenDental_Disease();
                fncSynchDataOpenDental_Disease();

                //SynchDataOpenDental_ProviderHours();
                fncSynchDataOpenDental_ProviderHours();

                SynchDataOpenDental_ApptType();
                fncSynchDataOpenDental_ApptType();

                //SynchDataLocalToOpenDental_Appointment();
                fncSynchDataLocalToOpenDental_Appointment();

                SynchDataOpenDental_RecallType();
                fncSynchDataOpenDental_RecallType();

                //SynchDataOpendental_User();
                fncSynchDataOpendental_User();

                SynchDataOpenDental_ApptStatus();
                fncSynchDataOpenDental_ApptStatus();

                //fncSynchDataOpenDental_Appointment();

                //SynchDataOpenDental_Holiday();
                fncSynchDataOpenDental_Holiday();

                //SynchDataLocalToOpenDental_Patient_Form();
                fncSynchDataLocalToOpenDental_Patient_Form();

                //SynchDataOpenDental_MedicalHistory();
                fncSynchDataOpenDental_MedicalHistory();

                //rooja 19-4-24 task link EHR Updates for RCM - https://app.asana.com/0/1199184824722493/1207061756651636/f
                SynchDataOpenDental_Insurance();
                fncSynchDataOpenDental_Insurance();

                //SynchDataOpenDental_PatientImages();

                // SynchDataOpenDental_PatientWiseRecallDate();
                //fncSynchDataOpenDental_PatientWiseRecallDate();
            }
        }

        #region Synch Appointment

        private void fncSynchDataOpenDental_Appointment()
        {
            InitBgWorkerOpenDental_Appointment();
            InitBgTimerOpenDental_Appointment();
        }

        private void InitBgTimerOpenDental_Appointment()
        {
            timerSynchOpenDental_Appointment = new System.Timers.Timer();
            this.timerSynchOpenDental_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchOpenDental_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_Appointment_Tick);
            timerSynchOpenDental_Appointment.Enabled = true;
            timerSynchOpenDental_Appointment.Start();
            timerSynchOpenDental_Appointment_Tick(null, null);
        }

        private void InitBgWorkerOpenDental_Appointment()
        {
            bwSynchOpenDental_Appointment = new BackgroundWorker();
            bwSynchOpenDental_Appointment.WorkerReportsProgress = true;
            bwSynchOpenDental_Appointment.WorkerSupportsCancellation = true;
            bwSynchOpenDental_Appointment.DoWork += new DoWorkEventHandler(bwSynchOpenDental_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOpenDental_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_Appointment_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {

                timerSynchOpenDental_Appointment.Enabled = false;
                MethodForCallSynchOrderOpenDental_Appointment();
            }
        }

        public void MethodForCallSynchOrderOpenDental_Appointment()
        {
            System.Threading.Thread procThreadmainOpenDental_Appointment = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_Appointment);
            procThreadmainOpenDental_Appointment.Start();
        }

        public void CallSyncOrderTableOpenDental_Appointment()
        {
            if (bwSynchOpenDental_Appointment.IsBusy != true)
            {
                bwSynchOpenDental_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLocalToOpenDental_PatientPayment();
                SynchDataOpenDental_Appointment();
                //SynchDataOpenDental_PatientStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_Appointment.Enabled = true;
        }

        public void SynchDataOpenDental_Appointment()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                IsOpenDentalProviderSync = true;
                IsOpenDentalOperatorySync = true;
                IsOpenDentalApptTypeSync = true;
                Is_synched_Appointment = true;
            }
            Utility.WriteToSyncLogFile_All(" Appointment Sync start at : " + System.DateTime.Now.ToString());

            if (IsOpenDentalProviderSync && IsOpenDentalOperatorySync && IsOpenDentalApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff) // && Utility.AditLocationSyncEnable 
            {
                try
                {
                    Is_synched_Appointment = false;
                    try
                    {
                        Utility.WriteToSyncLogFile_All("Start Appointment Patient sync : " + System.DateTime.Now.ToString());

                        try
                        {
                            SynchDataOpenDental_AppointmentPatient_New();
                        }
                        catch (Exception exPat)
                        {
                            SynchDataOpenDental_AppointmentPatient();
                        }
                        Utility.WriteToSyncLogFile_All("Appoitment Patient sync completed : " + System.DateTime.Now.ToString());

                        if (IsParientFirstSync)
                        {
                            SynchDataOpenDental_InsertPatient();
                        }
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[AppointmentPatient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex1.Message);
                    }
                    try
                    {
                        Utility.WriteToSyncLogFile_All("Start Appointment Patient Status Sync : " + System.DateTime.Now.ToString());
                        SynchDataOpenDental_PatientStatus();
                        Utility.WriteToSyncLogFile_All("Appointment Patient Status Sync Completed : " + System.DateTime.Now.ToString());
                    }
                    catch (Exception ex2)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex2.Message);
                    }

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        //Utility.WriteToSyncLogFile_All(" Get Appt records from OD : " + System.DateTime.Now.ToString());
                        DataTable dtOpenDentalAppointment = SynchOpenDentalBAL.GetOpenDentalAppointmentData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        // DataTable dtOpenDentalDeletedAppointment = SynchOpenDentalBAL.GetOpenDentalDeletedAppointmentData();                    

                        dtOpenDentalAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                        dtOpenDentalAppointment.Columns.Add("InsUptDlt", typeof(int));
                        // dtOpenDentalAppointment.Columns.Add("ProcedureDesc", typeof(string));
                        //dtOpenDentalAppointment.Columns.Add("ProcedureCode", typeof(string));

                        DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        //DataTable DtOpendentalAppointment_Procedures_Data = SynchOpenDentalBAL.GetOpendentalAppointment_Procedures_Data(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        string ProcedureDesc = "";
                        string ProcedureCode = "";

                        foreach (DataRow dtDtxRow in dtOpenDentalAppointment.Rows)
                        {
                            try
                            {
                                ///////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                                //ProcedureDesc = "";
                                //ProcedureCode = "";
                                //// DataTable dtCurApptProc = DtOpendentalAppointment_Procedures_Data.Select("aptnum = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'").CopyToDataTable();
                                //DataRow[] dtCurApptProcedure = DtOpendentalAppointment_Procedures_Data.Select("aptnum = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");
                                //foreach (var dtSinProc in dtCurApptProcedure.ToList())
                                //{
                                //    ProcedureDesc = ProcedureDesc + dtSinProc["ProcedureDesc"].ToString().Trim();
                                //    ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                                //}

                                //dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                                //dtDtxRow["ProcedureCode"] = ProcedureCode;

                                ////////////////////////////////

                                DataRow[] row = dtLocalAppointment.Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
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
                                ObjGoalBase.WriteToErrorLogFile("[Appointment (" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + ") Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                            }
                        }


                        dtOpenDentalAppointment.AcceptChanges();
                        Utility.WriteToSyncLogFile_All("Appointment Delete Comparison Finished at : " + System.DateTime.Now.ToString());
                        if (dtOpenDentalAppointment != null && dtOpenDentalAppointment.Rows.Count > 0 && dtOpenDentalAppointment.Select("InsUptDlt IN (1,5,4,7,3,6)").Count() > 0)
                        {
                            DataTable dtResult = dtOpenDentalAppointment.Select("InsUptDlt IN (1,5,4,7,3,6)").CopyToDataTable();
                            Utility.WriteToSyncLogFile_All("Appointment sent for Save and update : " + System.DateTime.Now.ToString() + " with Count " + dtResult.Rows.Count.ToString());

                            bool status = SynchOpenDentalBAL.Save_Appointment_OpenDental_To_Local(dtResult, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            Utility.WriteToSyncLogFile_All("Appointment inserted or updated : " + System.DateTime.Now.ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                                ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + "  to Local Database) Successfully.");

                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                        DataTable dtOpenDentalEHRDeletedAppointment = dtOpenDentalAppointment.Clone();
                        Utility.WriteToSyncLogFile_All("Appointment Comparison Finished at : " + System.DateTime.Now.ToString());
                        DataTable dtOpenDentalDeletedAppointment = SynchOpenDentalBAL.GetOpenDentalDeletedAppointmentData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                        DataTable dtLocalAppointmentAfterInsert = SynchLocalBAL.GetLocalAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        foreach (DataRow dtDtlRow in dtOpenDentalDeletedAppointment.Rows)
                        {
                            DataRow[] row = dtLocalAppointmentAfterInsert.Select("Appt_EHR_ID = '" + dtDtlRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length > 0)
                            {
                                if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                {
                                    DataRow ApptDtldr = dtOpenDentalEHRDeletedAppointment.NewRow();
                                    ApptDtldr["Appt_EHR_ID"] = dtDtlRow["Appt_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Clinic_Number"] = dtDtlRow["Clinic_Number"].ToString().Trim();
                                    ApptDtldr["InsUptDlt"] = 3;
                                    dtOpenDentalEHRDeletedAppointment.Rows.Add(ApptDtldr);
                                }
                            }
                            else
                            {
                                DataRow ApptDtldr = dtOpenDentalEHRDeletedAppointment.NewRow();
                                ApptDtldr["Appt_EHR_ID"] = dtDtlRow["Appt_EHR_ID"].ToString().Trim();
                                ApptDtldr["Clinic_Number"] = dtDtlRow["Clinic_Number"].ToString().Trim();
                                ApptDtldr["InsUptDlt"] = 6;
                                dtOpenDentalEHRDeletedAppointment.Rows.Add(ApptDtldr);
                            }
                        }
                        if (dtOpenDentalEHRDeletedAppointment != null && dtOpenDentalEHRDeletedAppointment.Rows.Count > 0 && dtOpenDentalEHRDeletedAppointment.Select("InsUptDlt IN (3,6)").Count() > 0)
                        {
                            DataTable dtResultDeleted = dtOpenDentalEHRDeletedAppointment.Select("InsUptDlt IN (3,6)").CopyToDataTable();
                            if (dtResultDeleted != null && dtResultDeleted.Rows.Count > 0)
                            {
                                //DataTable dtResult = dtOpenDentalAppointment.Select("InsUptDlt IN (3,6)").CopyToDataTable();
                                Utility.WriteToSyncLogFile_All("Appointment sent for Delete : " + System.DateTime.Now.ToString() + " with Count " + dtResultDeleted.Rows.Count.ToString());

                                bool status = SynchOpenDentalBAL.Save_Appointment_OpenDental_To_Local(dtResultDeleted, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                Utility.WriteToSyncLogFile_All("Appointment Deleted : " + System.DateTime.Now.ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                                    ObjGoalBase.WriteToSyncLogFile("Appointment Deleted Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + "  to Local Database) Successfully.");

                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Appointment Deleted Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                }
                            }
                        }

                    }
                    if (Utility.DtInstallServiceList.Rows.Count > 1)
                    {
                        SynchDataLiveDB_Push_AppointmentMultiLocation();
                    }
                    else
                    {
                        SynchDataLiveDB_Push_Appointment();
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
            else
            {
                ObjGoalBase.WriteToSyncLogFile("Appointment Sync All flag is not true OpenDentalProviderSync = " + IsOpenDentalProviderSync.ToString() + ", OpenDentalOperatorySync = " + IsOpenDentalOperatorySync.ToString() + ", OpenDentalApptTypeSync = " + IsOpenDentalApptTypeSync.ToString() + ", Synched_Appointment = " + Is_synched_Appointment.ToString() + ", ApplicationIdleTimeOff = " + Utility.IsApplicationIdleTimeOff.ToString());
            }
        }

        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataOpenDental_OperatoryEvent()
        {
            InitBgWorkerOpenDental_OperatoryEvent();
            InitBgTimerOpenDental_OperatoryEvent();
        }

        private void InitBgTimerOpenDental_OperatoryEvent()
        {
            timerSynchOpenDental_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchOpenDental_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchOpenDental_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_OperatoryEvent_Tick);
            timerSynchOpenDental_OperatoryEvent.Enabled = true;
            timerSynchOpenDental_OperatoryEvent.Start();
            timerSynchOpenDental_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerOpenDental_OperatoryEvent()
        {
            bwSynchOpenDental_OperatoryEvent = new BackgroundWorker();
            bwSynchOpenDental_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchOpenDental_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchOpenDental_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchOpenDental_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOpenDental_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderOpenDental_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderOpenDental_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainOpenDental_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_OperatoryEvent);
            procThreadmainOpenDental_OperatoryEvent.Start();
        }

        public void CallSyncOrderTableOpenDental_OperatoryEvent()
        {
            if (bwSynchOpenDental_OperatoryEvent.IsBusy != true)
            {
                bwSynchOpenDental_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_OperatoryEvent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_OperatoryEvent.Enabled = true;
        }

        public void SynchDataOpenDental_OperatoryEvent()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                IsOpenDentalOperatorySync = true;
                Is_synched_OperatoryEvent = false;
            }
            if (IsOpenDentalOperatorySync && !Is_synched_OperatoryEvent && Utility.IsApplicationIdleTimeOff) // && Utility.AditLocationSyncEnable
            {
                try
                {
                    Is_synched_OperatoryEvent = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalOperatoryEvent = SynchOpenDentalBAL.GetOpenDentalOperatoryEventData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        // DataTable dtOpenDentalOperatoryAllEvent = SynchOpenDentalBAL.GetOpenDentalOperatoryAllEventData();
                        dtOpenDentalOperatoryEvent.Columns.Add("OE_LocalDB_ID", typeof(int));
                        dtOpenDentalOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtOpenDentalOperatoryEvent.Rows)
                        {
                            DataRow[] row = dtLocalOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtDtxRow["ScheduleNum"].ToString().Trim() + "' ");
                            if (row.Length > 0)
                            {
                                if (Convert.ToDateTime(dtDtxRow["DateTStamp"].ToString().Trim()) != Convert.ToDateTime(row[0]["Entry_DateTime"]))
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Allow_Book_Appt"].ToString().Trim() != Convert.ToBoolean(row[0]["Allow_Book_Appt"].ToString().Trim()).ToString())
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
                            DataRow[] rowBlcOpt = dtOpenDentalOperatoryEvent.Copy().Select("ScheduleNum = '" + dtLOERow["OE_EHR_ID"].ToString().Trim() + "' ");
                            if (rowBlcOpt.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtOpenDentalOperatoryEvent.NewRow();
                                BlcOptDtldr["ScheduleNum"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtOpenDentalOperatoryEvent.Rows.Add(BlcOptDtldr);
                            }
                        }

                        dtOpenDentalOperatoryEvent.AcceptChanges();

                        if (dtOpenDentalOperatoryEvent != null && dtOpenDentalOperatoryEvent.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_OperatoryEvent_OpenDental_To_Local(dtOpenDentalOperatoryEvent, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        private void fncSynchDataOpenDental_Provider()
        {
            InitBgWorkerOpenDental_Provider();
            InitBgTimerOpenDental_Provider();
        }

        private void InitBgTimerOpenDental_Provider()
        {
            timerSynchOpenDental_Provider = new System.Timers.Timer();
            this.timerSynchOpenDental_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchOpenDental_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_Provider_Tick);
            timerSynchOpenDental_Provider.Enabled = true;
            timerSynchOpenDental_Provider.Start();
        }

        private void InitBgWorkerOpenDental_Provider()
        {
            bwSynchOpenDental_Provider = new BackgroundWorker();
            bwSynchOpenDental_Provider.WorkerReportsProgress = true;
            bwSynchOpenDental_Provider.WorkerSupportsCancellation = true;
            bwSynchOpenDental_Provider.DoWork += new DoWorkEventHandler(bwSynchOpenDental_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOpenDental_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_Provider_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_Provider.Enabled = false;
                MethodForCallSynchOrderOpenDental_Provider();
            }
        }

        public void MethodForCallSynchOrderOpenDental_Provider()
        {
            System.Threading.Thread procThreadmainOpenDental_Provider = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_Provider);
            procThreadmainOpenDental_Provider.Start();
        }

        public void CallSyncOrderTableOpenDental_Provider()
        {
            if (bwSynchOpenDental_Provider.IsBusy != true)
            {
                bwSynchOpenDental_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_Provider();
                CommonFunction.GetMasterSync();

                //  SynchDataOpenDental_ProviderHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_Provider.Enabled = true;
        }

        public void SynchDataOpenDental_Provider()
        {
            try
            {
                if (!Is_synched_Provider && Utility.IsApplicationIdleTimeOff) //&& Utility.AditLocationSyncEnable
                {
                    Is_synched_Provider = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalProvider = SynchOpenDentalBAL.GetOpenDentalProviderData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtOpenDentalProvider.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtOpenDentalProvider.Rows)
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

                        dtOpenDentalProvider.AcceptChanges();

                        if (dtOpenDentalProvider != null && dtOpenDentalProvider.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_Provider_OpenDental_To_Local(dtOpenDentalProvider, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                                ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                IsOpenDentalProviderSync = true;
                                SynchDataLiveDB_Push_Provider();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Providers Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                IsOpenDentalProviderSync = false;
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

        //public void SynchDataOpenDental_ProviderHours()
        //{
        //    try
        //    {
        //        if (Utility.IsApplicationIdleTimeOff && IsOpenDentalProviderSync && IsOpenDentalOperatorySync)
        //        {
        //            DataTable dtOpenDentalProviderHours = SynchOpenDentalBAL.GetOpenDentalProviderHoursData();
        //            dtOpenDentalProviderHours.Columns.Add("InsUptDlt", typeof(int));
        //            DataTable dtLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData();

        //            foreach (DataRow dtDtxRow in dtOpenDentalProviderHours.Rows)
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
        //            dtOpenDentalProviderHours.AcceptChanges();

        //            foreach (DataRow dtLPHRow in dtLocalProviderHours.Rows)
        //            {
        //                DataRow[] rowBlcOpt = dtOpenDentalProviderHours.Copy().Select("PH_EHR_ID = '" + dtLPHRow["PH_EHR_ID"].ToString().Trim() + "' ");
        //                if (rowBlcOpt.Length > 0)
        //                { }
        //                else
        //                {
        //                    DataRow BlcOptDtldr = dtOpenDentalProviderHours.NewRow();
        //                    BlcOptDtldr["PH_EHR_ID"] = dtLPHRow["PH_EHR_ID"].ToString().Trim();
        //                    BlcOptDtldr["InsUptDlt"] = 3;
        //                    dtOpenDentalProviderHours.Rows.Add(BlcOptDtldr);
        //                }
        //            }

        //            dtOpenDentalProviderHours.AcceptChanges();

        //            if (dtOpenDentalProviderHours != null && dtOpenDentalProviderHours.Rows.Count > 0)
        //            {
        //                bool status = SynchOpenDentalBAL.Save_ProviderHours_OpenDental_To_Local(dtOpenDentalProviderHours);

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

        private void fncSynchDataOpenDental_ProviderHours()
        {
            InitBgWorkerOpenDental_ProviderHours();
            InitBgTimerOpenDental_ProviderHours();
        }

        private void InitBgTimerOpenDental_ProviderHours()
        {
            timerSynchOpenDental_ProviderHours = new System.Timers.Timer();
            this.timerSynchOpenDental_ProviderHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchOpenDental_ProviderHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_ProviderHours_Tick);
            timerSynchOpenDental_ProviderHours.Enabled = true;
            timerSynchOpenDental_ProviderHours.Start();
        }

        private void InitBgWorkerOpenDental_ProviderHours()
        {
            bwSynchOpenDental_ProviderHours = new BackgroundWorker();
            bwSynchOpenDental_ProviderHours.WorkerReportsProgress = true;
            bwSynchOpenDental_ProviderHours.WorkerSupportsCancellation = true;
            bwSynchOpenDental_ProviderHours.DoWork += new DoWorkEventHandler(bwSynchOpenDental_ProviderHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOpenDental_ProviderHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_ProviderHours_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_ProviderHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_ProviderHours.Enabled = false;
                MethodForCallSynchOrderOpenDental_ProviderHours();
            }
        }

        public void MethodForCallSynchOrderOpenDental_ProviderHours()
        {
            System.Threading.Thread procThreadmainOpenDental_ProviderHours = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_ProviderHours);
            procThreadmainOpenDental_ProviderHours.Start();
        }

        public void CallSyncOrderTableOpenDental_ProviderHours()
        {
            if (bwSynchOpenDental_ProviderHours.IsBusy != true)
            {
                bwSynchOpenDental_ProviderHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_ProviderHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_ProviderHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SyncPullLogsAndSaveinOpenDental();
                SynchDataOpenDental_ProviderHours();                

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SyncPullLogsAndSaveinOpenDental()
        {
            try
            {
                CheckCustomhoursForProviderOperatory();
                SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                SynchDataLiveDB_Pull_PatientFollowUp();
                SynchDataPatientSMSCall_LocalTOOpenDental();
                fncPaymentSMSCallStatusUpdate();
                SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
            }
            catch (Exception)
            {

            }
        }

        private void bwSynchOpenDental_ProviderHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_ProviderHours.Enabled = true;
        }

        public void SynchDataOpenDental_ProviderHours()
        {
            try
            {
                //CheckCustomhoursForProviderOperatory();
                if (Utility.IsApplicationIdleTimeOff && IsOpenDentalProviderSync && IsOpenDentalOperatorySync && Utility.is_scheduledCustomhour)// && Utility.AditLocationSyncEnable
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalProviderHours = SynchOpenDentalBAL.GetOpenDentalProviderHoursData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtOpenDentalProviderHours.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtOpenDentalProviderHours.Rows)
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
                        dtOpenDentalProviderHours.AcceptChanges();

                        foreach (DataRow dtLPHRow in dtLocalProviderHours.Rows)
                        {
                            DataRow[] rowBlcOpt = dtOpenDentalProviderHours.Copy().Select("PH_EHR_ID = '" + dtLPHRow["PH_EHR_ID"].ToString().Trim() + "' ");
                            if (rowBlcOpt.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtOpenDentalProviderHours.NewRow();
                                BlcOptDtldr["PH_EHR_ID"] = dtLPHRow["PH_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["StartTime"] = dtLPHRow["StartTime"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtOpenDentalProviderHours.Rows.Add(BlcOptDtldr);
                            }
                        }

                        dtOpenDentalProviderHours.AcceptChanges();

                        if (dtOpenDentalProviderHours != null && dtOpenDentalProviderHours.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_ProviderHours_OpenDental_To_Local(dtOpenDentalProviderHours, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

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

        private void fncSynchDataOpenDental_Speciality()
        {
            InitBgWorkerOpenDental_Speciality();
            InitBgTimerOpenDental_Speciality();
        }

        private void InitBgTimerOpenDental_Speciality()
        {
            timerSynchOpenDental_Speciality = new System.Timers.Timer();
            this.timerSynchOpenDental_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchOpenDental_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_Speciality_Tick);
            timerSynchOpenDental_Speciality.Enabled = true;
            timerSynchOpenDental_Speciality.Start();
        }

        private void InitBgWorkerOpenDental_Speciality()
        {
            bwSynchOpenDental_Speciality = new BackgroundWorker();
            bwSynchOpenDental_Speciality.WorkerReportsProgress = true;
            bwSynchOpenDental_Speciality.WorkerSupportsCancellation = true;
            bwSynchOpenDental_Speciality.DoWork += new DoWorkEventHandler(bwSynchOpenDental_Speciality_DoWork);
            bwSynchOpenDental_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_Speciality_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_Speciality.Enabled = false;
                MethodForCallSynchOrderOpenDental_Speciality();
            }
        }

        public void MethodForCallSynchOrderOpenDental_Speciality()
        {
            System.Threading.Thread procThreadmainOpenDental_Speciality = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_Speciality);
            procThreadmainOpenDental_Speciality.Start();
        }

        public void CallSyncOrderTableOpenDental_Speciality()
        {
            if (bwSynchOpenDental_Speciality.IsBusy != true)
            {
                bwSynchOpenDental_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_Speciality();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_Speciality.Enabled = true;
        }

        public void SynchDataOpenDental_Speciality()
        {
            try
            {

                if (!Is_synched_Speciality && Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    Is_synched_Speciality = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalSpeciality = SynchOpenDentalBAL.GetOpenDentalProviderData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        DataView view = new DataView(dtOpenDentalSpeciality);
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
                            bool status = SynchOpenDentalBAL.Save_Speciality_OpenDental_To_Local(dtSpecialitydistinctValues, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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


        #region Sycn FolderList
        public void SynchDataOpenDental_FolderList()
        {
            try
            {
                if (!Is_synched_Operatory && Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    Is_synched_Operatory = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalFolderList = SynchOpenDentalBAL.GetOpenDentalFolderListData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtOpenDentalFolderList.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalFolderList = SynchLocalBAL.GetLocalFolderListData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtOpenDentalFolderList.Rows)
                        {
                            DataRow[] row = dtLocalFolderList.Copy().Select("FolderList_EHR_ID = '" + dtDtxRow["FolderList_EHR_ID"] + "'");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["Folder_Name"].ToString().Trim() != row[0]["Folder_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (Convert.ToInt32(dtDtxRow["FolderOrder"]) != Convert.ToInt32(row[0]["FolderOrder"]))
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
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }

                        dtOpenDentalFolderList.AcceptChanges();
                        if (dtOpenDentalFolderList != null && dtOpenDentalFolderList.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_FolderList_OpenDental_To_Local(dtOpenDentalFolderList, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FolderList");
                                ObjGoalBase.WriteToSyncLogFile("FolderList Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                IsOpenDentalOperatorySync = true;
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                IsOpenDentalOperatorySync = false;
                            }

                            #region Deleted FolderList
                            dtOpenDentalFolderList = dtOpenDentalFolderList.Clone();
                            DataTable dtOpenDentalDeletedFolderList = SynchOpenDentalBAL.GetOpenDentalDeletedFolderListData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            DataTable dtLocalFolderListAfterInsert = SynchLocalBAL.GetLocalFolderListData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            foreach (DataRow dtDtlRow in dtOpenDentalDeletedFolderList.Rows)
                            {
                                DataRow[] row = dtLocalFolderListAfterInsert.Copy().Select("FolderList_EHR_ID = '" + dtDtlRow["FolderList_EHR_ID"].ToString().Trim() + "'");
                                if (row.Length > 0)
                                {
                                    if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                    {
                                        DataRow ApptDtldr = dtOpenDentalFolderList.NewRow();
                                        ApptDtldr["FolderList_EHR_ID"] = dtDtlRow["FolderList_EHR_ID"].ToString().Trim();
                                        ApptDtldr["InsUptDlt"] = 3;
                                        dtOpenDentalFolderList.Rows.Add(ApptDtldr);
                                    }
                                }
                            }
                            if (dtOpenDentalFolderList != null && dtOpenDentalFolderList.Rows.Count > 0)
                            {
                                status = SynchOpenDentalBAL.Save_FolderList_OpenDental_To_Local(dtOpenDentalFolderList, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
                            }
                            #endregion
                            SynchDataLiveDB_Push_FolderList();
                        }
                    }
                    Is_synched_Operatory = false;
                }
            }
            catch (Exception ex)
            {
                Is_synched_Operatory = false;
                ObjGoalBase.WriteToErrorLogFile("[FolderList Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }


        #endregion

        #region Synch Operatory

        private void fncSynchDataOpenDental_Operatory()
        {
            InitBgWorkerOpenDental_Operatory();
            InitBgTimerOpenDental_Operatory();
        }

        private void InitBgTimerOpenDental_Operatory()
        {
            timerSynchOpenDental_Operatory = new System.Timers.Timer();
            this.timerSynchOpenDental_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchOpenDental_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_Operatory_Tick);
            timerSynchOpenDental_Operatory.Enabled = true;
            timerSynchOpenDental_Operatory.Start();
        }

        private void InitBgWorkerOpenDental_Operatory()
        {
            bwSynchOpenDental_Operatory = new BackgroundWorker();
            bwSynchOpenDental_Operatory.WorkerReportsProgress = true;
            bwSynchOpenDental_Operatory.WorkerSupportsCancellation = true;
            bwSynchOpenDental_Operatory.DoWork += new DoWorkEventHandler(bwSynchOpenDental_Operatory_DoWork);
            bwSynchOpenDental_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_Operatory_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_Operatory.Enabled = false;
                MethodForCallSynchOrderOpenDental_Operatory();
            }
        }

        public void MethodForCallSynchOrderOpenDental_Operatory()
        {
            System.Threading.Thread procThreadmainOpenDental_Operatory = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_Operatory);
            procThreadmainOpenDental_Operatory.Start();
        }

        public void CallSyncOrderTableOpenDental_Operatory()
        {
            if (bwSynchOpenDental_Operatory.IsBusy != true)
            {
                bwSynchOpenDental_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_Operatory.Enabled = true;
        }

        public void SynchDataOpenDental_Operatory()
        {
            try
            {
                SynchDataOpenDental_FolderList();
                if (!Is_synched_Operatory && Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    Is_synched_Operatory = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalOperatory = SynchOpenDentalBAL.GetOpenDentalOperatoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtOpenDentalOperatory.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtOpenDentalOperatory.Rows)
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

                        dtOpenDentalOperatory.AcceptChanges();
                        if (dtOpenDentalOperatory != null && dtOpenDentalOperatory.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_Operatory_OpenDental_To_Local(dtOpenDentalOperatory, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                                ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                IsOpenDentalOperatorySync = true;

                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                IsOpenDentalOperatorySync = false;
                            }

                            #region Deleted Operatory
                            dtOpenDentalOperatory = dtOpenDentalOperatory.Clone();
                            DataTable dtOpenDentalDeletedOperatory = SynchOpenDentalBAL.GetOpenDentalDeletedOperatoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            DataTable dtLocalOperatoryAfterInsert = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            foreach (DataRow dtDtlRow in dtOpenDentalDeletedOperatory.Rows)
                            {
                                DataRow[] row = dtLocalOperatoryAfterInsert.Copy().Select("Operatory_EHR_ID = '" + dtDtlRow["Operatory_EHR_ID"].ToString().Trim() + "'");
                                if (row.Length > 0)
                                {
                                    if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                    {
                                        DataRow ApptDtldr = dtOpenDentalOperatory.NewRow();
                                        ApptDtldr["Operatory_EHR_ID"] = dtDtlRow["Operatory_EHR_ID"].ToString().Trim();
                                        ApptDtldr["InsUptDlt"] = 3;
                                        dtOpenDentalOperatory.Rows.Add(ApptDtldr);
                                    }
                                }
                            }
                            if (dtOpenDentalOperatory != null && dtOpenDentalOperatory.Rows.Count > 0)
                            {
                                status = SynchOpenDentalBAL.Save_Operatory_OpenDental_To_Local(dtOpenDentalOperatory, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        private void fncSynchDataOpenDental_ApptType()
        {
            InitBgWorkerOpenDental_ApptType();
            InitBgTimerOpenDental_ApptType();
        }

        private void InitBgTimerOpenDental_ApptType()
        {
            timerSynchOpenDental_ApptType = new System.Timers.Timer();
            this.timerSynchOpenDental_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchOpenDental_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_ApptType_Tick);
            timerSynchOpenDental_ApptType.Enabled = true;
            timerSynchOpenDental_ApptType.Start();
        }

        private void InitBgWorkerOpenDental_ApptType()
        {
            bwSynchOpenDental_ApptType = new BackgroundWorker();
            bwSynchOpenDental_ApptType.WorkerReportsProgress = true;
            bwSynchOpenDental_ApptType.WorkerSupportsCancellation = true;
            bwSynchOpenDental_ApptType.DoWork += new DoWorkEventHandler(bwSynchOpenDental_ApptType_DoWork);
            bwSynchOpenDental_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_ApptType_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_ApptType.Enabled = false;
                MethodForCallSynchOrderOpenDental_ApptType();
            }
        }

        public void MethodForCallSynchOrderOpenDental_ApptType()
        {
            System.Threading.Thread procThreadmainOpenDental_ApptType = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_ApptType);
            procThreadmainOpenDental_ApptType.Start();
        }

        public void CallSyncOrderTableOpenDental_ApptType()
        {
            if (bwSynchOpenDental_ApptType.IsBusy != true)
            {
                bwSynchOpenDental_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_ApptType.Enabled = true;
        }

        public void SynchDataOpenDental_ApptType()
        {
            try
            {
                if (!Is_synched_ApptType && Utility.IsApplicationIdleTimeOff)// && Utility.AditLocationSyncEnable
                {
                    Is_synched_ApptType = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalApptType = SynchOpenDentalBAL.GetOpenDentalApptTypeData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtOpenDentalApptType.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString())
                            {
                                DataRow Addrow = dtOpenDentalApptType.NewRow();
                                Addrow["ApptType_EHR_ID"] = 0;
                                Addrow["Type_Name"] = "none";
                                Addrow["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"];
                                dtOpenDentalApptType.Rows.Add(Addrow);
                                dtOpenDentalApptType.AcceptChanges();
                            }
                        }

                        foreach (DataRow dtDtxRow in dtOpenDentalApptType.Rows)
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
                            DataRow[] row = dtOpenDentalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                            if (row.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtOpenDentalApptType.NewRow();
                                BlcOptDtldr["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["Type_Name"] = dtDtxRow["Type_Name"].ToString().Trim();
                                BlcOptDtldr["Clinic_Number"] = dtDtxRow["Clinic_Number"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtOpenDentalApptType.Rows.Add(BlcOptDtldr);
                            }
                        }
                        dtOpenDentalApptType.AcceptChanges();

                        if (dtOpenDentalApptType != null && dtOpenDentalApptType.Rows.Count > 0)
                        {
                            bool Type = SynchOpenDentalBAL.Save_ApptType_OpenDental_To_Local(dtOpenDentalApptType, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (Type)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                                ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                IsOpenDentalApptTypeSync = true;
                                SynchDataLiveDB_Push_ApptType();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Appointment Type Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                IsOpenDentalApptTypeSync = false;
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

        private void fncSynchDataOpenDental_Patient()
        {
            InitBgWorkerOpenDental_Patient();
            InitBgTimerOpenDental_Patient();
        }

        private void InitBgTimerOpenDental_Patient()
        {
            timerSynchOpenDental_Patient = new System.Timers.Timer();
            this.timerSynchOpenDental_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchOpenDental_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_Patient_Tick);
            timerSynchOpenDental_Patient.Enabled = true;
            timerSynchOpenDental_Patient.Start();
            timerSynchOpenDental_Patient_Tick(null, null);
        }

        private void InitBgWorkerOpenDental_Patient()
        {
            bwSynchOpenDental_Patient = new BackgroundWorker();
            bwSynchOpenDental_Patient.WorkerReportsProgress = true;
            bwSynchOpenDental_Patient.WorkerSupportsCancellation = true;
            bwSynchOpenDental_Patient.DoWork += new DoWorkEventHandler(bwSynchOpenDental_Patient_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOpenDental_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_Patient_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_Patient.Enabled = false;
                MethodForCallSynchOrderOpenDental_Patient();
            }
        }

        public void MethodForCallSynchOrderOpenDental_Patient()
        {
            System.Threading.Thread procThreadmainOpenDental_Patient = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_Patient);
            procThreadmainOpenDental_Patient.Start();
        }

        public void CallSyncOrderTableOpenDental_Patient()
        {
            if (bwSynchOpenDental_Patient.IsBusy != true)
            {
                bwSynchOpenDental_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                if (Utility.OpenDentalOldPatSync)
                {
                    SynchDataOpenDental_Patient_Old();
                }
                else
                {
                    SynchDataOpenDental_Patient_New();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_Patient.Enabled = true;
        }

        public void PatientCommonCode(DataRow dtDtxRow, DataTable dtOpenDentalPatientLastVisit_Date, DataTable dtOpenDentalPatientNextApptDate, DataTable dtOpenDentalPatient, DataTable dtOpenDentalPatientdue_date, DataTable dtLocalPatient, ref string patSEX, ref string MaritalStatus, ref string tmpdue_date, ref string ReceiveSMS, ref string PatientActiveInactive, ref bool _successfullstataus, ref string sqlSelect, ref string due_date, string UsedBenefit, string RemainingBenefit)
        {
            try
            {
                string birthdate = Utility.CheckValidDatetime(dtDtxRow["Birth_Date"].ToString().Trim());
                if (birthdate != "") dtDtxRow["Birth_Date"] = birthdate;

                string firstvisitdate = Utility.CheckValidDatetime(dtDtxRow["FirstVisit_Date"].ToString().Trim());
                if (firstvisitdate != "") dtDtxRow["FirstVisit_Date"] = firstvisitdate;

                string lastvisitdate = Utility.SetLastVisitDate(dtOpenDentalPatientLastVisit_Date, "PatNum", "Patient_EHR_ID", "lastvisit_date", dtDtxRow["Patient_EHR_ID"].ToString());
                if (lastvisitdate != "") dtDtxRow["LastVisit_Date"] = lastvisitdate;

                //https://app.asana.com/0/751059797849097/1149506260330945
                dtDtxRow["nextvisit_date"] = Utility.SetNextVisitDate(dtOpenDentalPatientNextApptDate, "PatNum", "Patient_EHR_ID", "nextvisit_date", dtDtxRow["Patient_EHR_ID"].ToString());

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
                    DataRow[] Patdue_date = dtOpenDentalPatientdue_date.Copy().Select("patient_id = '" + dtDtxRow["Patient_EHR_ID"] + "'");

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

                dtOpenDentalPatient.AcceptChanges();

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

        public void SynchDataOpenDental_Patient_New()
        {
            string PatEhrID = "", CliNum = "";
            string patientTableName = "";
            try
            {
                if (Utility.IsExternalAppointmentSync)
                {
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;
                }

                if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient)//&& Utility.AditLocationSyncEnable
                {
                    ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Start.");
                    DataTable dtLocalOpenDentalLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();
                    SynchDataLiveDB_Pull_EHR_Patientoptout();
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: dtLocalPatientResult Records = " + dtLocalPatientResult.Rows.Count);

                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            CliNum = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                            ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: ClinicNumber = " + CliNum);

                            if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {
                                    DataTable dtOpenDentalPatient = SynchOpenDentalBAL.GetOpenDentalPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), true);
                                    ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: dtOpenDentalPatient = " + dtOpenDentalPatient.Rows.Count);
                                    DataTable dtLocalPatient = new DataTable();
                                    try
                                    {
                                        if (dtLocalPatientResult != null && dtLocalPatientResult.Rows.Count > 0)
                                        {
                                            if (dtLocalPatientResult.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Count() > 0)
                                            {
                                                dtLocalPatient = dtLocalPatientResult.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                                            }
                                        }
                                    }
                                    catch (Exception ex2)
                                    {
                                        Utility.WriteToErrorLogFromAll("Err in New local filter: " + ex2.ToString());
                                    }

                                    ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: dtLocalPatient Records = " + dtLocalPatient.Rows.Count);

                                    if ((dtOpenDentalPatient != null && dtOpenDentalPatient.Rows.Count > 0) || (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0))
                                    {
                                        var updateLanguageQuery = from r1 in dtOpenDentalPatient.AsEnumerable()
                                                                  join r2 in dtLocalOpenDentalLanguageList.AsEnumerable()
                                                                  on r1.Field<string>("PreferredLanguage") equals r2.Field<string>("Language_Short_Name")
                                                                  select new { r1, r2 };
                                        foreach (var x in updateLanguageQuery)
                                        {
                                            x.r1.SetField("PreferredLanguage", x.r2.Field<string>("Language_Name"));
                                        }

                                        DataTable dtSaveRecords = new DataTable();
                                        dtSaveRecords = dtLocalPatient.Clone();
                                        ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Compare Start. Start for patient to be added.");
                                        var itemsToBeAdded = (from OpenDentalPatient in dtOpenDentalPatient.AsEnumerable()
                                                              join LocalPatient in dtLocalPatient.AsEnumerable()
                                                              on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                              equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                              //on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                                              //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
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


                                        ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: End for patient to be added = " + itemsToBeAdded.Count + ". Start for update patient.");
                                        //Update
                                        var itemsToBeUpdated = (from LocalPatient in dtLocalPatient.AsEnumerable()
                                                                join OpenDentalPatient in dtOpenDentalPatient.AsEnumerable()
                                                                on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                                equals OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                                //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                                                //equals new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                                                where
                                                                (LocalPatient["First_name"] != DBNull.Value ? LocalPatient["First_name"].ToString().Trim() : "") != (OpenDentalPatient["First_name"] != DBNull.Value ? OpenDentalPatient["First_name"].ToString().Trim() : "") ||
                                                                (LocalPatient["Last_name"] != DBNull.Value ? LocalPatient["Last_name"].ToString().Trim() : "") != (OpenDentalPatient["Last_name"] != DBNull.Value ? OpenDentalPatient["Last_name"].ToString().Trim() : "") ||
                                                                (LocalPatient["Middle_Name"] != DBNull.Value ? LocalPatient["Middle_Name"].ToString().Trim() : "") != (OpenDentalPatient["Middle_Name"] != DBNull.Value ? OpenDentalPatient["Middle_Name"].ToString().Trim() : "") ||
                                                                (LocalPatient["Salutation"] != DBNull.Value ? LocalPatient["Salutation"].ToString().Trim() : "") != (OpenDentalPatient["Salutation"] != DBNull.Value ? OpenDentalPatient["Salutation"].ToString().Trim() : "") ||
                                                                (LocalPatient["Status"] != DBNull.Value ? LocalPatient["Status"].ToString().Trim() : "") != (OpenDentalPatient["Status"] != DBNull.Value ? OpenDentalPatient["Status"].ToString().Trim() : "") ||
                                                                (LocalPatient["Sex"] != DBNull.Value ? LocalPatient["Sex"].ToString().Trim() : "") != (OpenDentalPatient["Sex"] != DBNull.Value ? OpenDentalPatient["Sex"].ToString().Trim() : "") ||
                                                                (LocalPatient["MaritalStatus"] != DBNull.Value ? LocalPatient["MaritalStatus"].ToString().Trim() : "") != (OpenDentalPatient["MaritalStatus"] != DBNull.Value ? OpenDentalPatient["MaritalStatus"].ToString().Trim() : "") ||
                                                                (LocalPatient["Birth_Date"] != DBNull.Value && LocalPatient["Birth_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["Birth_Date"].ToString().Trim()) : DateTime.Now) != (OpenDentalPatient["Birth_Date"] != DBNull.Value && OpenDentalPatient["Birth_Date"].ToString().Trim() != "" ? Convert.ToDateTime(OpenDentalPatient["Birth_Date"].ToString().Trim()) : DateTime.Now) ||
                                                                (LocalPatient["Email"] != DBNull.Value ? LocalPatient["Email"].ToString().Trim() : "") != (OpenDentalPatient["Email"] != DBNull.Value ? OpenDentalPatient["Email"].ToString().Trim() : "") ||
                                                                (LocalPatient["Mobile"] != DBNull.Value ? LocalPatient["Mobile"].ToString().Trim() : "") != (OpenDentalPatient["Mobile"] != DBNull.Value ? OpenDentalPatient["Mobile"].ToString().Trim() : "") ||
                                                                (LocalPatient["Home_Phone"] != DBNull.Value ? LocalPatient["Home_Phone"].ToString().Trim() : "") != (OpenDentalPatient["Home_Phone"] != DBNull.Value ? OpenDentalPatient["Home_Phone"].ToString().Trim() : "") ||
                                                                (LocalPatient["Work_Phone"] != DBNull.Value ? LocalPatient["Work_Phone"].ToString().Trim() : "") != (OpenDentalPatient["Work_Phone"] != DBNull.Value ? OpenDentalPatient["Work_Phone"].ToString().Trim() : "") ||
                                                                (LocalPatient["Address1"] != DBNull.Value ? LocalPatient["Address1"].ToString().Trim() : "") != (OpenDentalPatient["Address1"] != DBNull.Value ? OpenDentalPatient["Address1"].ToString().Trim() : "") ||
                                                                (LocalPatient["Address2"] != DBNull.Value ? LocalPatient["Address2"].ToString().Trim() : "") != (OpenDentalPatient["Address2"] != DBNull.Value ? OpenDentalPatient["Address2"].ToString().Trim() : "") ||
                                                                (LocalPatient["City"] != DBNull.Value ? LocalPatient["City"].ToString().Trim() : "") != (OpenDentalPatient["City"] != DBNull.Value ? OpenDentalPatient["City"].ToString().Trim() : "") ||
                                                                (LocalPatient["State"] != DBNull.Value ? LocalPatient["State"].ToString().Trim() : "") != (OpenDentalPatient["State"] != DBNull.Value ? OpenDentalPatient["State"].ToString().Trim() : "") ||
                                                                (LocalPatient["Zipcode"] != DBNull.Value ? LocalPatient["Zipcode"].ToString().Trim() : "") != (OpenDentalPatient["Zipcode"] != DBNull.Value ? OpenDentalPatient["Zipcode"].ToString().Trim() : "") ||
                                                                (LocalPatient["ResponsibleParty_Status"] != DBNull.Value ? LocalPatient["ResponsibleParty_Status"].ToString().Trim() : "") != (OpenDentalPatient["ResponsibleParty_Status"] != DBNull.Value ? OpenDentalPatient["ResponsibleParty_Status"].ToString().Trim() : "") ||
                                                                (LocalPatient["CurrentBal"] != DBNull.Value ? LocalPatient["CurrentBal"].ToString().Trim() : "") != (OpenDentalPatient["CurrentBal"] != DBNull.Value ? OpenDentalPatient["CurrentBal"].ToString().Trim() : "") ||
                                                                (LocalPatient["ThirtyDay"] != DBNull.Value ? LocalPatient["ThirtyDay"].ToString().Trim() : "") != (OpenDentalPatient["ThirtyDay"] != DBNull.Value ? OpenDentalPatient["ThirtyDay"].ToString().Trim() : "") ||
                                                                (LocalPatient["SixtyDay"] != DBNull.Value ? LocalPatient["SixtyDay"].ToString().Trim() : "") != (OpenDentalPatient["SixtyDay"] != DBNull.Value ? OpenDentalPatient["SixtyDay"].ToString().Trim() : "") ||
                                                                (LocalPatient["NinetyDay"] != DBNull.Value ? LocalPatient["NinetyDay"].ToString().Trim() : "") != (OpenDentalPatient["NinetyDay"] != DBNull.Value ? OpenDentalPatient["NinetyDay"].ToString().Trim() : "") ||
                                                                (LocalPatient["Over90"] != DBNull.Value ? LocalPatient["Over90"].ToString().Trim() : "") != (OpenDentalPatient["Over90"] != DBNull.Value ? OpenDentalPatient["Over90"].ToString().Trim() : "") ||
                                                                (LocalPatient["FirstVisit_Date"] != DBNull.Value && LocalPatient["FirstVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["FirstVisit_Date"].ToString().Trim()) : DateTime.Now) != (OpenDentalPatient["FirstVisit_Date"] != DBNull.Value && OpenDentalPatient["FirstVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(OpenDentalPatient["FirstVisit_Date"].ToString().Trim()) : DateTime.Now) ||
                                                                (LocalPatient["LastVisit_Date"] != DBNull.Value && LocalPatient["LastVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["LastVisit_Date"].ToString().Trim()) : DateTime.Now) != (OpenDentalPatient["LastVisit_Date"] != DBNull.Value && OpenDentalPatient["LastVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(OpenDentalPatient["LastVisit_Date"].ToString().Trim()) : DateTime.Now) ||
                                                                (LocalPatient["Primary_Insurance"] != DBNull.Value ? LocalPatient["Primary_Insurance"].ToString().Trim() : "") != (OpenDentalPatient["Primary_Insurance"] != DBNull.Value ? OpenDentalPatient["Primary_Insurance"].ToString().Trim() : "") ||
                                                                (LocalPatient["Primary_Insurance_CompanyName"] != DBNull.Value ? LocalPatient["Primary_Insurance_CompanyName"].ToString().Trim() : "") != (OpenDentalPatient["Primary_Insurance_CompanyName"] != DBNull.Value ? OpenDentalPatient["Primary_Insurance_CompanyName"].ToString().Trim() : "") ||
                                                                (LocalPatient["Secondary_Insurance"] != DBNull.Value ? LocalPatient["Secondary_Insurance"].ToString().Trim() : "") != (OpenDentalPatient["Secondary_Insurance"] != DBNull.Value ? OpenDentalPatient["Secondary_Insurance"].ToString().Trim() : "") ||
                                                                (LocalPatient["Secondary_Insurance_CompanyName"] != DBNull.Value ? LocalPatient["Secondary_Insurance_CompanyName"].ToString().Trim() : "") != (OpenDentalPatient["Secondary_Insurance_CompanyName"] != DBNull.Value ? OpenDentalPatient["Secondary_Insurance_CompanyName"].ToString().Trim() : "") ||
                                                                (LocalPatient["Guar_ID"] != DBNull.Value ? LocalPatient["Guar_ID"].ToString().Trim() : "") != (OpenDentalPatient["Guar_ID"] != DBNull.Value ? OpenDentalPatient["Guar_ID"].ToString().Trim() : "") ||
                                                                (LocalPatient["Pri_Provider_ID"] != DBNull.Value ? LocalPatient["Pri_Provider_ID"].ToString().Trim() : "") != (OpenDentalPatient["Pri_Provider_ID"] != DBNull.Value ? OpenDentalPatient["Pri_Provider_ID"].ToString().Trim() : "") ||
                                                                (LocalPatient["Sec_Provider_ID"] != DBNull.Value ? LocalPatient["Sec_Provider_ID"].ToString().Trim() : "") != (OpenDentalPatient["Sec_Provider_ID"] != DBNull.Value ? OpenDentalPatient["Sec_Provider_ID"].ToString().Trim() : "") ||
                                                                (LocalPatient["ReceiveSms"] != DBNull.Value ? LocalPatient["ReceiveSms"].ToString().Trim() : "") != (OpenDentalPatient["ReceiveSms"] != DBNull.Value ? OpenDentalPatient["ReceiveSms"].ToString().Trim() : "") ||
                                                                (LocalPatient["ReceiveEmail"] != DBNull.Value ? LocalPatient["ReceiveEmail"].ToString().Trim() : "") != (OpenDentalPatient["ReceiveEmail"] != DBNull.Value ? OpenDentalPatient["ReceiveEmail"].ToString().Trim() : "") ||
                                                                (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["nextvisit_date"].ToString().Trim()) : DateTime.Now) != (OpenDentalPatient["nextvisit_date"] != DBNull.Value && OpenDentalPatient["nextvisit_date"].ToString().Trim() != "" ? Convert.ToDateTime(OpenDentalPatient["nextvisit_date"].ToString().Trim()) : DateTime.Now) ||
                                                                (LocalPatient["due_date"] != DBNull.Value && LocalPatient["due_date"].ToString().Trim() != "" ? LocalPatient["due_date"].ToString().Trim() : "") != (OpenDentalPatient["due_date"] != DBNull.Value && OpenDentalPatient["due_date"].ToString().Trim() != "" ? OpenDentalPatient["due_date"].ToString().Trim() : "") ||
                                                                (LocalPatient["remaining_benefit"] != DBNull.Value ? LocalPatient["remaining_benefit"].ToString().Trim() : "") != (OpenDentalPatient["remaining_benefit"] != DBNull.Value ? OpenDentalPatient["remaining_benefit"].ToString().Trim() : "") ||
                                                                (LocalPatient["collect_payment"] != DBNull.Value ? LocalPatient["collect_payment"].ToString().Trim() : "") != (OpenDentalPatient["collect_payment"] != DBNull.Value ? OpenDentalPatient["collect_payment"].ToString().Trim() : "") ||
                                                                (LocalPatient["preferred_name"] != DBNull.Value ? LocalPatient["preferred_name"].ToString().Trim() : "") != (OpenDentalPatient["preferred_name"] != DBNull.Value ? OpenDentalPatient["preferred_name"].ToString().Trim() : "") ||
                                                                (LocalPatient["used_benefit"] != DBNull.Value ? LocalPatient["used_benefit"].ToString().Trim() : "") != (OpenDentalPatient["used_benefit"] != DBNull.Value ? OpenDentalPatient["used_benefit"].ToString().Trim() : "") ||
                                                                (LocalPatient["Secondary_Ins_Subscriber_ID"] != DBNull.Value ? LocalPatient["Secondary_Ins_Subscriber_ID"].ToString().Trim() : "") != (OpenDentalPatient["Secondary_Ins_Subscriber_ID"] != DBNull.Value ? OpenDentalPatient["Secondary_Ins_Subscriber_ID"].ToString().Trim() : "") ||
                                                                (LocalPatient["Primary_Ins_Subscriber_ID"] != DBNull.Value ? LocalPatient["Primary_Ins_Subscriber_ID"].ToString().Trim() : "") != (OpenDentalPatient["Primary_Ins_Subscriber_ID"] != DBNull.Value ? OpenDentalPatient["Primary_Ins_Subscriber_ID"].ToString().Trim() : "") ||
                                                                (LocalPatient["EHR_Status"] != DBNull.Value ? LocalPatient["EHR_Status"].ToString().Trim() : "") != (OpenDentalPatient["EHR_Status"] != DBNull.Value ? OpenDentalPatient["EHR_Status"].ToString().Trim() : "") ||
                                                                (LocalPatient["ReceiveVoiceCall"] != DBNull.Value ? LocalPatient["ReceiveVoiceCall"].ToString().Trim() : "") != (OpenDentalPatient["ReceiveVoiceCall"] != DBNull.Value ? OpenDentalPatient["ReceiveVoiceCall"].ToString().Trim() : "") ||
                                                                (LocalPatient["PreferredLanguage"] != DBNull.Value ? LocalPatient["PreferredLanguage"].ToString().Trim() : "") != (OpenDentalPatient["PreferredLanguage"] != DBNull.Value ? OpenDentalPatient["PreferredLanguage"].ToString().Trim() : "") ||
                                                                (LocalPatient["Patient_Note"] != DBNull.Value ? LocalPatient["Patient_Note"].ToString().Trim() : "") != (OpenDentalPatient["Patient_Note"] != DBNull.Value ? OpenDentalPatient["Patient_Note"].ToString().Trim() : "") ||
                                                                (LocalPatient["ssn"] != DBNull.Value ? LocalPatient["ssn"].ToString().Trim() : "") != (OpenDentalPatient["ssn"] != DBNull.Value ? OpenDentalPatient["ssn"].ToString().Trim() : "") ||
                                                                (LocalPatient["driverlicense"] != DBNull.Value ? LocalPatient["driverlicense"].ToString().Trim() : "") != (OpenDentalPatient["driverlicense"] != DBNull.Value ? OpenDentalPatient["driverlicense"].ToString().Trim() : "") ||
                                                                (LocalPatient["groupid"] != DBNull.Value ? LocalPatient["groupid"].ToString().Trim() : "") != (OpenDentalPatient["groupid"] != DBNull.Value ? OpenDentalPatient["groupid"].ToString().Trim() : "") ||
                                                                (LocalPatient["emergencycontactId"] != DBNull.Value ? LocalPatient["emergencycontactId"].ToString().Trim() : "") != (OpenDentalPatient["emergencycontactId"] != DBNull.Value ? OpenDentalPatient["emergencycontactId"].ToString().Trim() : "") ||
                                                                (LocalPatient["EmergencyContact_First_Name"] != DBNull.Value ? LocalPatient["EmergencyContact_First_Name"].ToString().Trim() : "") != (OpenDentalPatient["EmergencyContact_First_Name"] != DBNull.Value ? OpenDentalPatient["EmergencyContact_First_Name"].ToString().Trim() : "") ||
                                                                (LocalPatient["EmergencyContact_Last_Name"] != DBNull.Value ? LocalPatient["EmergencyContact_Last_Name"].ToString().Trim() : "") != (OpenDentalPatient["EmergencyContact_Last_Name"] != DBNull.Value ? OpenDentalPatient["EmergencyContact_Last_Name"].ToString().Trim() : "") ||
                                                                (LocalPatient["emergencycontactnumber"] != DBNull.Value ? LocalPatient["emergencycontactnumber"].ToString().Trim() : "") != (OpenDentalPatient["emergencycontactnumber"] != DBNull.Value ? OpenDentalPatient["emergencycontactnumber"].ToString().Trim() : "") ||
                                                                (LocalPatient["school"] != DBNull.Value ? LocalPatient["school"].ToString().Trim() : "") != (OpenDentalPatient["school"] != DBNull.Value ? OpenDentalPatient["school"].ToString().Trim() : "") ||
                                                                (LocalPatient["employer"] != DBNull.Value ? LocalPatient["employer"].ToString().Trim() : "") != (OpenDentalPatient["employer"] != DBNull.Value ? OpenDentalPatient["employer"].ToString().Trim() : "") ||
                                                                (LocalPatient["spouseId"] != DBNull.Value ? LocalPatient["spouseId"].ToString().Trim() : "") != (OpenDentalPatient["spouseId"] != DBNull.Value ? OpenDentalPatient["spouseId"].ToString().Trim() : "") ||
                                                                (LocalPatient["responsiblepartyId"] != DBNull.Value ? LocalPatient["responsiblepartyId"].ToString().Trim() : "") != (OpenDentalPatient["responsiblepartyId"] != DBNull.Value ? OpenDentalPatient["responsiblepartyId"].ToString().Trim() : "") ||
                                                                (LocalPatient["responsiblepartyssn"] != DBNull.Value ? LocalPatient["responsiblepartyssn"].ToString().Trim() : "") != (OpenDentalPatient["responsiblepartyssn"] != DBNull.Value ? OpenDentalPatient["responsiblepartyssn"].ToString().Trim() : "") ||
                                                                (LocalPatient["responsiblepartybirthdate"] != DBNull.Value && LocalPatient["responsiblepartybirthdate"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["responsiblepartybirthdate"].ToString().Trim()) : DateTime.Now) != (OpenDentalPatient["responsiblepartybirthdate"] != DBNull.Value && OpenDentalPatient["responsiblepartybirthdate"].ToString().Trim() != "" ? Convert.ToDateTime(OpenDentalPatient["responsiblepartybirthdate"].ToString().Trim()) : DateTime.Now) ||
                                                                (LocalPatient["Spouse_First_Name"] != DBNull.Value ? LocalPatient["Spouse_First_Name"].ToString().Trim() : "") != (OpenDentalPatient["Spouse_First_Name"] != DBNull.Value ? OpenDentalPatient["Spouse_First_Name"].ToString().Trim() : "") ||
                                                                (LocalPatient["Spouse_Last_Name"] != DBNull.Value ? LocalPatient["Spouse_Last_Name"].ToString().Trim() : "") != (OpenDentalPatient["Spouse_Last_Name"] != DBNull.Value ? OpenDentalPatient["Spouse_Last_Name"].ToString().Trim() : "") ||
                                                                (LocalPatient["ResponsibleParty_First_Name"] != DBNull.Value ? LocalPatient["ResponsibleParty_First_Name"].ToString().Trim() : "") != (OpenDentalPatient["ResponsibleParty_First_Name"] != DBNull.Value ? OpenDentalPatient["ResponsibleParty_First_Name"].ToString().Trim() : "") ||
                                                                (LocalPatient["ResponsibleParty_Last_Name"] != DBNull.Value ? LocalPatient["ResponsibleParty_Last_Name"].ToString().Trim() : "") != (OpenDentalPatient["ResponsibleParty_Last_Name"] != DBNull.Value ? OpenDentalPatient["ResponsibleParty_Last_Name"].ToString().Trim() : "") ||
                                                                (LocalPatient["Prim_Ins_Company_Phonenumber"] != DBNull.Value ? LocalPatient["Prim_Ins_Company_Phonenumber"].ToString().Trim() : "") != (OpenDentalPatient["Prim_Ins_Company_Phonenumber"] != DBNull.Value ? OpenDentalPatient["Prim_Ins_Company_Phonenumber"].ToString().Trim() : "") ||
                                                                (LocalPatient["Sec_Ins_Company_Phonenumber"] != DBNull.Value ? LocalPatient["Sec_Ins_Company_Phonenumber"].ToString().Trim() : "") != (OpenDentalPatient["Sec_Ins_Company_Phonenumber"] != DBNull.Value ? OpenDentalPatient["Sec_Ins_Company_Phonenumber"].ToString().Trim() : "") ||
                                                                (LocalPatient["Is_deleted"] != DBNull.Value ? Convert.ToInt32(LocalPatient["Is_deleted"]).ToString().Trim() : "0") != (OpenDentalPatient["Is_deleted"] != DBNull.Value ? OpenDentalPatient["Is_deleted"].ToString().Trim() : "0")
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
                                        ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: End for patient to be udpated = " + itemsToBeUpdated + ". Compare for Delete patient start.");
                                        //Deleted
                                        var itemToBeDeleted = (from LocalPatient in dtLocalPatient.AsEnumerable()
                                                               join OpenDentalPatient in dtOpenDentalPatient.AsEnumerable()
                                                               on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                               equals OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                               //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                                               //equals new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                                               into matchingRows
                                                               from matchingRow in matchingRows.DefaultIfEmpty()
                                                               where LocalPatient["is_deleted"].ToString().Trim().ToUpper() == "FALSE" && matchingRow == null
                                                               select LocalPatient).ToList();

                                        DataTable dtPatientToBeDelete = dtLocalPatient.Clone();
                                        if (itemToBeDeleted.Count > 0)
                                        {
                                            dtPatientToBeDelete = itemToBeDeleted.CopyToDataTable<DataRow>();
                                        }
                                        if (!dtPatientToBeDelete.Columns.Contains("InsUptDlt"))
                                        {
                                            dtPatientToBeDelete.Columns.Add("InsUptDlt", typeof(int));
                                            dtPatientToBeDelete.Columns["InsUptDlt"].DefaultValue = 0;
                                        }

                                        if (dtPatientToBeDelete.Rows.Count > 0)
                                        {
                                            dtPatientToBeDelete.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 3);
                                            dtSaveRecords.Load(dtPatientToBeDelete.Select().CopyToDataTable().CreateDataReader());
                                        }
                                        ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Compare for Delete Patient End = " + itemToBeDeleted.Count);
                                        bool status = false;
                                        if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                                        {
                                            try
                                            {
                                                ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Patient to be Added = " + dtSaveRecords.Select("InsUptDlt = '1'").Length);
                                                ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Patient to be Updated = " + dtSaveRecords.Select("InsUptDlt = '2'").Length);
                                                ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Patient to be Deleted = " + dtSaveRecords.Select("InsUptDlt = '3'").Length);
                                            }
                                            catch
                                            {
                                            }
                                            if (dtSaveRecords.Rows.Count > 0)
                                            {
                                                status = SynchOpenDentalBAL.Save_Patient_OpenDental_To_Local_New(dtSaveRecords, Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                            }
                                            else
                                            {
                                                status = true;
                                            }
                                        }
                                        if (status)
                                        {
                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                            IsGetParientRecordDone = true;
                                            ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                            IsGetParientRecordDone = true;
                                        }
                                        IsPatientSyncedFirstTime = true;
                                        IsParientFirstSync = true;
                                        Is_synched_Patient = false;

                                    }
                                }

                            }
                        }
                    }

                    try
                    {
                        if (Utility.DtLocationList.Rows.Count > 1)
                        {
                            SynchDataLiveDB_Push_PatientMultiLocation();
                        }
                        else
                        {
                            SynchDataLiveDB_Push_Patient();
                        }
                        bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                        SynchDataOpenDental_PatientStatus();
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex1.Message);
                    }
                    ObjGoalBase.WriteToSyncLogFile_All("All Patients Added to local database and Pushed with Patient Status.");

                    try
                    {
                        SynchDataOpenDental_PatientImages();
                        ObjGoalBase.WriteToSyncLogFile_All("PatientImages Done.");
                        SynchDataOpenDental_PatientDisease();
                        ObjGoalBase.WriteToSyncLogFile_All("PatientDisease Done.");
                        SynchDataOpenDental_PatientMedication("");
                        ObjGoalBase.WriteToSyncLogFile_All("PatientMedication Done.");
                    }
                    catch (Exception ex2)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Error in Images/Disease/PatMedication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex2.Message);
                    }
                }
                else if (Is_synched_AppointmentsPatient)
                {
                    Is_Synched_PatientCallHit = true;
                }
                Utility.WriteToSyncLogFile_All("Patient Sync Complete Without Error.");
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().StartsWith(("THE TIMEOUT PERIOD ELAPSED PRIOR TO COMPLETION OF THE OPERATION OR THE SERVER IS NOT RESPONDING.").ToUpper()))
                {
                    Utility.OpenDentalOldPatSync = true;
                    SynchDataOpenDental_Patient_Old();
                }
                else
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ] PatID:" + PatEhrID + ", Clinic_Number:" + CliNum +
                    System.Environment.NewLine + "Error: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                    Is_synched_Patient = false;
                    IsParientFirstSync = true;
                }
            }
            finally
            {
                Is_synched_Patient = false;
            }
        }

        public void SynchDataOpenDental_Patient_Old()
        {
            try
            {

                if (Utility.IsExternalAppointmentSync)
                {
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;
                }

                if (Utility.IsApplicationIdleTimeOff && IsParientFirstSync && !Is_synched_Patient)//&& Utility.AditLocationSyncEnable
                {
                    DataTable dtLocalOpenDentalLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();
                    SynchDataLiveDB_Pull_EHR_Patientoptout();
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;


                    string due_date = string.Empty;
                    string patSEX = string.Empty;
                    string MaritalStatus = string.Empty;
                    string tmpdue_date = string.Empty;
                    string ReceiveSMS = string.Empty;
                    string PatientActiveInactive = "I";
                    bool _successfullstataus = true;
                    string sqlSelect = string.Empty;

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {

                                    DataTable dtOpenDentalPatientNextApptDate = SynchOpenDentalBAL.GetOpenDentalPatientNextApptDate(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    DataTable dtOpenDentalPatientLastVisit_Date = SynchOpenDentalBAL.GetOpenDentalPatientLastVisit_Date(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    DataTable dtOpenDentalPatient = SynchOpenDentalBAL.GetOpenDentalPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), false);

                                    DataTable dtOpenDentalPatientWisePendingAmount = SynchOpenDentalBAL.GetPatientWisePendingAmount(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                                    dtOpenDentalPatient.Columns.Add("Primary_Insurance", typeof(string));
                                    dtOpenDentalPatient.Columns.Add("Primary_Insurance_CompanyName", typeof(string));
                                    dtOpenDentalPatient.Columns.Add("Primary_Ins_Subscriber_ID", typeof(string));
                                    dtOpenDentalPatient.Columns.Add("Secondary_Insurance", typeof(string));
                                    dtOpenDentalPatient.Columns.Add("Secondary_Insurance_CompanyName", typeof(string));
                                    dtOpenDentalPatient.Columns.Add("Secondary_Ins_Subscriber_ID", typeof(string));

                                    //dtOpenDentalPatient.Columns.Add("InsUptDlt", typeof(int));

                                    DataTable dtOpenDentalPatientdue_date = SynchOpenDentalBAL.GetOpenDentalPatientdue_date(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                                    string patientTableName = "Patient";

                                    DataTable dtLocalPatient = new DataTable();
                                    DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                    //Utility.WriteToErrorLogFromAll("Got Local DB patient" + dtLocalPatientResult.Rows.Count.ToString());

                                    try
                                    {
                                        if (dtLocalPatientResult != null && dtLocalPatientResult.Rows.Count > 0)
                                        {
                                            dtLocalPatient = dtLocalPatientResult.Select("Clinic_Number = " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString()).CopyToDataTable();
                                        }
                                    }
                                    catch (Exception ex2)
                                    {
                                        Utility.WriteToErrorLogFromAll("Err " + ex2.ToString());
                                    }


                                    if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                                    {
                                        patientTableName = "PatientCompare";
                                    }
                                    due_date = string.Empty;
                                    patSEX = string.Empty;
                                    MaritalStatus = string.Empty;
                                    tmpdue_date = string.Empty;
                                    ReceiveSMS = string.Empty;
                                    PatientActiveInactive = "I";
                                    _successfullstataus = true;
                                    sqlSelect = string.Empty;
                                    TotalPatientRecord = dtOpenDentalPatient.Rows.Count;
                                    GetPatientRecord = 0;

                                    DataTable dtOpenDentalInsuranceDataAll = SynchOpenDentalBAL.GetOpenDentalPatientInsuranceData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), "", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    DataTable dtBenafitAll = SynchOpenDentalBAL.GetOpenDentalPatientInsBenafit(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), "", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    DataTable dtOpenDentalInsuranceData = new DataTable();
                                    DataTable dtBenafit = new DataTable();
                                    DataRow[] myrow = null;
                                    DataRow[] drBenafit = null;
                                    #region SqlCeConnection
                                    if (!Utility.isSqlServer)
                                    {
                                        string UsedBenafit = "0";
                                        string RemainingBenafit = "0";
                                        using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                                        {
                                            foreach (DataRow dtDtxRow in dtOpenDentalPatient.Rows)
                                            {
                                                // CommonDB.LocalConnectionServer(ref conn);

                                                GetPatientRecord = GetPatientRecord + 1;

                                                myrow = dtOpenDentalInsuranceDataAll.Select("Patient_EHR_Id = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
                                                dtOpenDentalInsuranceData.Rows.Clear();
                                                if (myrow != null && myrow.Count() > 0)
                                                {
                                                    dtOpenDentalInsuranceData = myrow.CopyToDataTable();
                                                }

                                                drBenafit = dtBenafitAll.Select("Patient_EHR_Id = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
                                                dtBenafit.Rows.Clear();
                                                if (drBenafit != null && drBenafit.Count() > 0)
                                                {
                                                    dtBenafit = drBenafit.CopyToDataTable();
                                                }

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

                                                DataRow drLanguage = dtLocalOpenDentalLanguageList.Copy().Select("Language_Short_Name = '" + dtDtxRow["PreferredLanguage"].ToString() + "'").FirstOrDefault();
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

                                                if (dtOpenDentalInsuranceData.Rows.Count == 1)
                                                {
                                                    dtDtxRow["Primary_Insurance"] = dtOpenDentalInsuranceData.Rows[0]["Primary_Insurance"].ToString();
                                                    dtDtxRow["Primary_Insurance_CompanyName"] = dtOpenDentalInsuranceData.Rows[0]["Primary_Insurance_CompanyName"].ToString();
                                                    dtDtxRow["Primary_Ins_Subscriber_ID"] = dtOpenDentalInsuranceData.Rows[0]["SubscriberID"].ToString();
                                                    dtDtxRow["Prim_Ins_Company_Phonenumber"] = dtOpenDentalInsuranceData.Rows[0]["Prim_Ins_Company_Phonenumber"].ToString();
                                                    dtDtxRow["groupid"] = dtOpenDentalInsuranceData.Rows[0]["groupid"].ToString();


                                                }
                                                else if (dtOpenDentalInsuranceData.Rows.Count >= 2)
                                                {
                                                    dtDtxRow["Primary_Insurance"] = dtOpenDentalInsuranceData.Rows[0]["Primary_Insurance"].ToString();
                                                    dtDtxRow["Primary_Insurance_CompanyName"] = dtOpenDentalInsuranceData.Rows[0]["Primary_Insurance_CompanyName"].ToString();
                                                    dtDtxRow["Primary_Ins_Subscriber_ID"] = dtOpenDentalInsuranceData.Rows[0]["SubscriberID"].ToString();
                                                    dtDtxRow["Prim_Ins_Company_Phonenumber"] = dtOpenDentalInsuranceData.Rows[0]["Prim_Ins_Company_Phonenumber"].ToString();
                                                    dtDtxRow["groupid"] = dtOpenDentalInsuranceData.Rows[0]["groupid"].ToString();
                                                    dtDtxRow["Secondary_Insurance"] = dtOpenDentalInsuranceData.Rows[1]["Primary_Insurance"].ToString();
                                                    dtDtxRow["Secondary_Insurance_CompanyName"] = dtOpenDentalInsuranceData.Rows[1]["Primary_Insurance_CompanyName"].ToString();
                                                    dtDtxRow["Secondary_Ins_Subscriber_ID"] = dtOpenDentalInsuranceData.Rows[1]["SubscriberID"].ToString();
                                                    dtDtxRow["Sec_Ins_Company_Phonenumber"] = dtOpenDentalInsuranceData.Rows[1]["Prim_Ins_Company_Phonenumber"].ToString();
                                                }
                                                if (dtDtxRow["Primary_Insurance_CompanyName"].ToString() == "" && dtDtxRow["Secondary_Insurance_CompanyName"].ToString() == "")
                                                {
                                                    decimal curPatientcollect_payment = 0;
                                                    dtDtxRow["used_benefit"] = curPatientcollect_payment.ToString();
                                                    dtDtxRow["remaining_benefit"] = curPatientcollect_payment.ToString();
                                                }
                                                PatientCommonCode(dtDtxRow, dtOpenDentalPatientLastVisit_Date, dtOpenDentalPatientNextApptDate, dtOpenDentalPatient, dtOpenDentalPatientdue_date, dtLocalPatient, ref patSEX, ref MaritalStatus, ref tmpdue_date, ref ReceiveSMS, ref PatientActiveInactive, ref _successfullstataus, ref sqlSelect, ref due_date, UsedBenafit, RemainingBenafit);

                                                if (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()) != 0)
                                                {
                                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                                                    {
                                                        SqlCeCommand.CommandType = CommandType.Text;

                                                        var PatientWisePendingAmount = dtOpenDentalPatientWisePendingAmount.AsEnumerable().Where(o => Convert.ToInt64(o.Field<object>("PatNum")) == Convert.ToInt64(dtDtxRow["Patient_EHR_ID"]));
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
                                                string SqlCeSelect = PatientCompareQuery;
                                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                                {
                                                    SqlCeCommand.Parameters.Clear();
                                                    SqlCeCommand.CommandType = CommandType.Text;
                                                    SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                                    SqlCeCommand.Parameters.Add("Clinic_Number", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());

                                                    using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                                                    {
                                                        dtPatientCompareRec = new DataTable();
                                                        SqlCeDa.Fill(dtPatientCompareRec);
                                                    }
                                                    foreach (DataRow drRow in dtPatientCompareRec.Rows)
                                                    {
                                                        ExecuteQuery("Patient", drRow, SqlCeCommand, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                                    }


                                                    IEnumerable<string> PatientEHRIDs = dtOpenDentalPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
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

                                    dtOpenDentalPatient.AcceptChanges();

                                    if (dtOpenDentalPatient != null && dtOpenDentalPatient.Rows.Count > 0)
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
                            }
                        }
                    }
                    IsPatientSyncedFirstTime = true;
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;

                    try
                    {

                        SynchDataLiveDB_Push_Patient();
                        SynchDataOpenDental_PatientStatus();

                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex1.Message);
                    }
                    try
                    {

                        SynchDataOpenDental_PatientImages();
                        SynchDataOpenDental_PatientDisease();
                        SynchDataOpenDental_PatientMedication("");

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

        public void SynchDataOpenDental_Patient()
        {
            string PatEhrID = "", CliNum = "";
            try
            {
                //Utility.IsExternalAppointmentSync = true;
                Utility.WriteToSyncLogFile_All("Patient Sync Start.");

                if (Utility.IsExternalAppointmentSync)
                {
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;
                }

                if (Utility.IsApplicationIdleTimeOff && IsParientFirstSync && !Is_synched_Patient)//&& Utility.AditLocationSyncEnable
                {
                    DataTable dtLocalOpenDentalLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();
                    SynchDataLiveDB_Pull_EHR_Patientoptout();
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;


                    string due_date = string.Empty;
                    string patSEX = string.Empty;
                    string MaritalStatus = string.Empty;
                    string tmpdue_date = string.Empty;
                    string ReceiveSMS = string.Empty;
                    string PatientActiveInactive = "I";
                    bool _successfullstataus = true;
                    string sqlSelect = string.Empty;

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            string SqlCeSelect = PatientCompareQuery;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.CommandText = "Delete from PatientCompare where Service_Install_Id = @Service_Install_Id  ";
                                SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }

                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {
                                    DataTable dtOpenDentalPatient = SynchOpenDentalBAL.GetOpenDentalPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), false);

                                    var updateLanguageQuery = from r1 in dtOpenDentalPatient.AsEnumerable()
                                                              join r2 in dtLocalOpenDentalLanguageList.AsEnumerable()
                                                              on r1.Field<string>("PreferredLanguage") equals r2.Field<string>("Language_Short_Name")
                                                              select new { r1, r2 };
                                    foreach (var x in updateLanguageQuery)
                                    {
                                        x.r1.SetField("PreferredLanguage", x.r2.Field<string>("Language_Name"));
                                    }

                                    string patientTableName = "Patient";

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
                                        Utility.WriteToErrorLogFromAll("Err in local filter: " + ex2.ToString());
                                    }


                                    if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                                    {
                                        patientTableName = "PatientCompare";
                                    }
                                    //Utility.WriteToSyncLogFile_All("TaleName:" + patientTableName);

                                    due_date = string.Empty;
                                    patSEX = string.Empty;
                                    MaritalStatus = string.Empty;
                                    tmpdue_date = string.Empty;
                                    ReceiveSMS = string.Empty;
                                    PatientActiveInactive = "I";
                                    _successfullstataus = true;
                                    sqlSelect = string.Empty;
                                    TotalPatientRecord = dtOpenDentalPatient.Rows.Count;
                                    GetPatientRecord = 0;

                                    #region SqlCeConnection
                                    if (!Utility.isSqlServer)
                                    {
                                        string UsedBenafit = "0";
                                        string RemainingBenafit = "0";
                                        using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                                        {
                                            //Utility.WriteToSyncLogFile_All("dtOpenDentalPatient.Rows : " + dtOpenDentalPatient.Rows.Count);
                                            if (conn.State == ConnectionState.Closed) conn.Open();
                                            if (patientTableName.ToString().ToUpper().Trim() == "PATIENTCOMPARE")
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
                                                    DataColumnCollection columns = dtOpenDentalPatient.Columns;

                                                    foreach (DataRow dtDtxRow in dtOpenDentalPatient.Rows)
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
                                                foreach (DataRow dtDtxRow in dtOpenDentalPatient.Rows)
                                                {
                                                    if (Convert.ToInt32(dtDtxRow["InsUptDlt"].ToString()) != 0)
                                                    {
                                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(sqlSelect, conn))
                                                        {
                                                            SqlCeCommand.CommandType = CommandType.Text;

                                                            ExecuteQuery(patientTableName, dtDtxRow, SqlCeCommand, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

                                                    using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                                                    {
                                                        dtPatientCompareRec = new DataTable();
                                                        SqlCeDa.Fill(dtPatientCompareRec);
                                                    }
                                                    //ObjGoalBase.WriteToSyncLogFile_All("CompareRec Reords to Insert/Update/Delete:" + dtPatientCompareRec.Rows.Count);
                                                    foreach (DataRow drRow in dtPatientCompareRec.Rows)
                                                    {
                                                        ExecuteQuery("Patient", drRow, SqlCeCommand, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                                    }


                                                    IEnumerable<string> PatientEHRIDs = dtOpenDentalPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct();
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

                                                    //SqlCeCommand.Parameters.Clear();
                                                    //SqlCeCommand.CommandText = "ALTER TABLE [PatientCompare] ALTER COLUMN [Patient_LocalDB_ID] IDENTITY (1, 1)";
                                                    //SqlCeCommand.Parameters.Add("Service_Install_Id", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                                    //SqlCeCommand.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                        #endregion

                                    }
                                    #endregion

                                    dtOpenDentalPatient.AcceptChanges();

                                    if (dtOpenDentalPatient != null && dtOpenDentalPatient.Rows.Count > 0)
                                    {
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                        IsGetParientRecordDone = true;
                                        ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");


                                    }
                                    else
                                    {
                                        ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");

                                        IsGetParientRecordDone = true;
                                    }
                                }
                            }
                        }
                    }
                    IsPatientSyncedFirstTime = true;
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;

                    try
                    {
                        if (Utility.DtLocationList.Rows.Count > 1)
                        {
                            SynchDataLiveDB_Push_PatientMultiLocation();
                        }
                        else
                        {
                            SynchDataLiveDB_Push_Patient();
                        }
                        bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                        SynchDataOpenDental_PatientStatus();

                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex1.Message);
                    }
                    try
                    {

                        SynchDataOpenDental_PatientImages();
                        SynchDataOpenDental_PatientDisease();
                        SynchDataOpenDental_PatientMedication("");

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
                Utility.WriteToSyncLogFile_All("Patient Sync Complete Without Error.");
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ] PatID:" + PatEhrID + ", Clinic_Number:" + CliNum + ", Message:" + ex.Message);
                Is_synched_Patient = false;
                IsParientFirstSync = true;
            }
        }

        public void SynchDataOpenDental_PatientStatus()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    DataTable dtOpenDentalPatientStatus = new DataTable();
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        //for (int i = 0; i < Utility.DtLocationList.Select("Clinic_number = " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()).Count(); i++)
                        foreach (DataRow drClinic in Utility.DtLocationList.Select("Service_Install_Id = " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()))
                        {
                            ObjGoalBase.WriteToSyncLogFile("PatientStatus Sync For Clinic Num " + drClinic["Clinic_Number"].ToString());
                            if (drClinic["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(drClinic["AditLocationSyncEnable"]))
                            {
                                if (drClinic["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {
                                    dtOpenDentalPatientStatus = SynchOpenDentalBAL.GetOpenDentalPatientStatusData(drClinic["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    ObjGoalBase.WriteToSyncLogFile("PatientStatus Total Patient Without Appointment count is " + dtOpenDentalPatientStatus.Rows.Count.ToString() + " in Clinic Num " + drClinic["Clinic_Number"].ToString());
                                    if (dtOpenDentalPatientStatus != null && dtOpenDentalPatientStatus.Rows.Count > 0)
                                    {
                                        SynchLocalBAL.UpdatePatient_Status(dtOpenDentalPatientStatus, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Convert.ToInt32(drClinic["Clinic_Number"].ToString()));
                                    }
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientStatus");
                                    ObjGoalBase.WriteToSyncLogFile("PatientStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_PatientStatus(Convert.ToInt32(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()), Convert.ToInt32(drClinic["Clinic_Number"].ToString()));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);

            }
        }

        public void SynchDataOpenDental_InsertPatient()
        {
            string PatEhrID = "", CliNum = "";
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient) //&& Utility.AditLocationSyncEnable
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalInsertPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            CliNum = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
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
                                        Utility.WriteToErrorLogFromAll("[SynchDataOpenDental_InsertPatient] Err in New local filter: " + ex2.ToString());
                                    }
                                    if ((dtLocalPatient != null && dtLocalPatient.Rows.Count > 0 && dtLocalPatient.Select("Is_Adit_Updated = 1").Length > 0))
                                    {
                                        DataTable dtOpenDentalPatient = SynchOpenDentalBAL.GetOpenDentalInsertPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                        if ((dtOpenDentalPatient != null && dtOpenDentalPatient.Rows.Count > 0))
                                        {
                                            var itemsToBeAdded = (from OpenDentalPatient in dtOpenDentalPatient.AsEnumerable()
                                                                  join LocalPatient in dtLocalPatient.AsEnumerable()
                                                                  on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                                  equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                                  //on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                                                  //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
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
                                                    dtSaveRecords = SynchOpenDentalBAL.GetOpenDentalPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), true, strPatID);

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
                                                            status = SynchOpenDentalBAL.Save_Patient_OpenDental_To_Local_New(dtSaveRecords, Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                                        }
                                                        else
                                                        {
                                                            status = true;
                                                        }

                                                        if (status)
                                                        {
                                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                                            IsGetParientRecordDone = true;
                                                            ObjGoalBase.WriteToSyncLogFile("[SynchDataOpenDental_InsertPatient] Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                                        }
                                                        else
                                                        {
                                                            ObjGoalBase.WriteToSyncLogFile("[SynchDataOpenDental_InsertPatient] Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");

                                                            IsGetParientRecordDone = true;
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

                    try
                    {
                        if (Utility.DtLocationList.Rows.Count > 1)
                        {
                            SynchDataLiveDB_Push_PatientMultiLocation();
                        }
                        else
                        {
                            SynchDataLiveDB_Push_Patient();
                        }
                        bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[SynchDataOpenDental_InsertPatient]->[Push Patient] Error: " + ex1.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[SynchDataOpenDental_InsertPatient Sync (" + Utility.Application_Name + " to Local Database) ] PatID:" + PatEhrID + ", Clinic_Number:" + CliNum +
                    System.Environment.NewLine + "Error: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                Is_synched_Patient = false;
                IsParientFirstSync = true;
            }
        }

        public void SynchDataOpenDental_AppointmentPatient_New()
        {
            try
            {
                //Utility.WriteToSyncLogFile_All("Appointment Patient Sync Start : " + System.DateTime.Now.ToString());
                if (Utility.IsExternalAppointmentSync)
                {
                    Is_synched_Patient = false;
                    Is_synched_AppointmentsPatient = false;
                }
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient)//&& Utility.AditLocationSyncEnable
                {
                    DataTable dtLocalOpenDentalLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();

                    //Utility.WriteToSyncLogFile_All("Appointment Patient Sync First Condition True : " + System.DateTime.Now.ToString());
                    Is_synched_AppointmentsPatient = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        //Utility.WriteToSyncLogFile_All("dtLocalPatientResult:" + dtLocalPatientResult.Rows.Count);

                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {
                                    //Utility.WriteToSyncLogFile_All("Appointment Patient Get Records from OD : " + System.DateTime.Now.ToString());

                                    DataTable dtOpenDentalAppointmensPatient = SynchOpenDentalBAL.GetOpenDentalAppointmentsPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), true);
                                    //Utility.WriteToSyncLogFile_All("ToDate: " + Utility.LastSyncDateAditServer.ToString());
                                    //Utility.WriteToSyncLogFile_All("EHR Records :" + dtOpenDentalAppointmensPatient.Rows.Count + ", Clinic_Number: " + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());

                                    var updateLanguageQuery = from r1 in dtOpenDentalAppointmensPatient.AsEnumerable()
                                                              join r2 in dtLocalOpenDentalLanguageList.AsEnumerable()
                                                              on r1.Field<string>("PreferredLanguage") equals r2.Field<string>("Language_Short_Name")
                                                              select new { r1, r2 };
                                    foreach (var x in updateLanguageQuery)
                                    {
                                        x.r1.SetField("PreferredLanguage", x.r2.Field<string>("Language_Name"));
                                    }

                                    DataTable dtLocalPatient = new DataTable();
                                    try
                                    {
                                        if (dtLocalPatientResult != null && dtLocalPatientResult.Rows.Count > 0)
                                        {
                                            dtLocalPatient = dtLocalPatientResult.Clone();
                                            var LocalPatientByPatEHRID = (from LocalPatient in dtLocalPatientResult.AsEnumerable()
                                                                          join OpenDentalPatient in dtOpenDentalAppointmensPatient.AsEnumerable()
                                                                          on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                                          equals OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                                          //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                                                          //equals new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                                                          where LocalPatient["Patient_EHR_ID"].ToString().Trim() == OpenDentalPatient["Patient_EHR_ID"].ToString().Trim()
                                                                          select LocalPatient).ToList();
                                            if (LocalPatientByPatEHRID.Count > 0)
                                            {
                                                dtLocalPatient = LocalPatientByPatEHRID.CopyToDataTable<DataRow>();
                                            }
                                            //Utility.WriteToSyncLogFile_All("Local Records:" + dtLocalPatient.Rows.Count);
                                        }
                                    }
                                    catch (Exception ex2)
                                    {
                                        Utility.WriteToErrorLogFromAll("Err in New local filter: " + ex2.ToString());
                                        throw ex2;
                                        //dtLocalPatient = new DataTable();
                                        //dtLocalPatient = dtLocalPatientByClinic.Copy();
                                    }

                                    DataTable dtSaveRecords = new DataTable();
                                    dtSaveRecords = dtLocalPatient.Clone();
                                    //ObjGoalBase.WriteToSyncLogFile_All("Compare for New Patients Start.");
                                    var itemsToBeAdded = (from OpenDentalPatient in dtOpenDentalAppointmensPatient.AsEnumerable()
                                                          join LocalPatient in dtLocalPatient.AsEnumerable()
                                                          on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                          equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                          //on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                                          //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
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
                                        //ObjGoalBase.WriteToSyncLogFile_All("Table to Save Record count after insert :" + dtSaveRecords.Rows.Count);
                                    }
                                    //ObjGoalBase.WriteToSyncLogFile_All("Compare for New Patients End. Records to be added: " + dtPatientToBeAdded.Rows.Count);

                                    //ObjGoalBase.WriteToSyncLogFile_All("Compare for Updated Patient Start.");

                                    //Update
                                    var itemsToBeUpdated = (from OpenDentalPatient in dtOpenDentalAppointmensPatient.AsEnumerable()
                                                            join LocalPatient in dtLocalPatient.AsEnumerable()
                                                            on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                            equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                            //on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                                            //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                                            where
                                                             Convert.ToDateTime(OpenDentalPatient["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(LocalPatient["EHR_Entry_DateTime"])

                                                             ||

                                                             (OpenDentalPatient["nextvisit_date"] != DBNull.Value && OpenDentalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(OpenDentalPatient["nextvisit_date"]) : DateTime.Now)
                                                             !=
                                                             (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)

                                                             ||

                                                             (OpenDentalPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())

                                                             ||

                                                             (OpenDentalPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())

                                                             || (OpenDentalPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                                             || (OpenDentalPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                                             || (OpenDentalPatient["Home_Phone"].ToString().Trim()) != (LocalPatient["Home_Phone"].ToString().Trim())
                                                             || (OpenDentalPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                                             || (OpenDentalPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                                             || (OpenDentalPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                                             || (OpenDentalPatient["Mobile"].ToString().Trim()) != (LocalPatient["Mobile"].ToString().Trim())
                                                             || (OpenDentalPatient["ReceiveSMS"].ToString().Trim()) != (LocalPatient["ReceiveSMS"].ToString().Trim())
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
                                        //ObjGoalBase.WriteToSyncLogFile_All("Table to Save Record count after update :" + dtSaveRecords.Rows.Count);
                                    }

                                    //ObjGoalBase.WriteToSyncLogFile_All("Compare for updated records end. Records to be udpated:" + dtPatientToBeUpdated.Rows.Count);

                                    if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                                    {
                                        //DataTable dtResult = dtOpenDentalAppointmensPatient.Select("InsUptDlt IN (1,2,3)").CopyToDataTable();
                                        //Utility.WriteToSyncLogFile_All("Patient sent for Save and update : " + System.DateTime.Now.ToString() + " with Count " + dtSaveRecords.Rows.Count.ToString());
                                        //Utility.WriteToSyncLogFile_All("Save:" + dtSaveRecords.Select("InsUptDlt='1'").Length);
                                        //Utility.WriteToSyncLogFile_All("Update:" + dtSaveRecords.Select("InsUptDlt='2'").Length);

                                        //bool status = SynchOpenDentalBAL.Save_PatientAppointment_OpenDental_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                        bool status = SynchOpenDentalBAL.Save_Patient_OpenDental_To_Local_New(dtSaveRecords, Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                        //Utility.WriteToSyncLogFile_All("Patient inserted or updated : " + System.DateTime.Now.ToString());

                                        if (status)
                                        {
                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment Patient");
                                            ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + "  to Local Database) Successfully.");

                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[Appointment Patient Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                        }
                                    }
                                }
                            }
                            Is_synched_AppointmentsPatient = false;
                        }
                    }
                    if (Utility.DtLocationList.Rows.Count > 1)
                    {
                        SynchDataLiveDB_Push_PatientMultiLocation();
                    }
                    else
                    {
                        SynchDataLiveDB_Push_Patient();
                    }
                }
                Utility.WriteToSyncLogFile_All("Appointment Patient IsApplicationIdleTimeOff : " + Utility.IsApplicationIdleTimeOff.ToString() + " Is_synched_AppointmentsPatient : " + Is_synched_AppointmentsPatient.ToString() + " at " + System.DateTime.Now.ToString());

            }
            catch (Exception ex)
            {
                Is_synched_AppointmentsPatient = false;
                ObjGoalBase.WriteToErrorLogFile("[(New)Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                ObjGoalBase.WriteToErrorLogFile(ex.StackTrace);
                throw ex;
            }
        }

        public void SynchDataOpenDental_AppointmentPatient()
        {
            try
            {
                Utility.WriteToSyncLogFile_All("Appointment Patient Sync Start : " + System.DateTime.Now.ToString());
                if (Utility.IsExternalAppointmentSync)
                {
                    Is_synched_Patient = false;
                    Is_synched_AppointmentsPatient = false;
                }
                Utility.WriteToSyncLogFile_All("Appointment Patient IsApplicationIdleTimeOff : " + Utility.IsApplicationIdleTimeOff.ToString() + " Is_synched_AppointmentsPatient : " + Is_synched_AppointmentsPatient.ToString() + " at " + System.DateTime.Now.ToString());
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient)//&& Utility.AditLocationSyncEnable
                {
                    Utility.WriteToSyncLogFile_All("Appointment Patient Sync First Condition True : " + System.DateTime.Now.ToString());

                    Is_synched_AppointmentsPatient = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {

                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {
                                    Utility.WriteToSyncLogFile_All("Appointment Patient Get Records from OD : " + System.DateTime.Now.ToString());

                                    DataTable dtOpenDentalAppointmensPatient = SynchOpenDentalBAL.GetOpenDentalAppointmentsPatientData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), false);

                                    string patientTableName = "Patient";
                                    string PatientEHRIDs = string.Join("','", dtOpenDentalAppointmensPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                                    if (PatientEHRIDs != string.Empty)
                                    {
                                        PatientEHRIDs = "'" + PatientEHRIDs + "'";
                                        Utility.WriteToSyncLogFile_All("Appointment Patient Get Records from Local : " + System.DateTime.Now.ToString());
                                        DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                        #region SqlCeConnection
                                        if (!Utility.isSqlServer)
                                        {
                                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                                            {
                                                int m = 1;
                                                Utility.WriteToSyncLogFile_All("Appointment Patient Start Comparision : " + System.DateTime.Now.ToString());

                                                foreach (DataRow dtDtxRow in dtOpenDentalAppointmensPatient.Rows)
                                                {

                                                    DataRow[] row = dtLocalPatient.Select("patient_ehr_id = '" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "' ");
                                                    try
                                                    {
                                                        if (row.Length > 0)
                                                        {
                                                            //if (m < 1)
                                                            //{
                                                            //    Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + ", EHR_Entry_DateTime = " + dtDtxRow["EHR_Entry_DateTime"].ToString() + "&" + row[0]["EHR_Entry_DateTime"].ToString());
                                                            //    Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + ", nextvisit_date = " + dtDtxRow["nextvisit_date"].ToString() + "&" + row[0]["nextvisit_date"].ToString());
                                                            //    Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + ", EHR_Status = " + dtDtxRow["EHR_Status"].ToString() + "&" + row[0]["EHR_Status"].ToString());
                                                            //    Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + ", due_date = " + dtDtxRow["due_date"].ToString() + "&" + row[0]["due_date"].ToString());
                                                            //}
                                                            if (Convert.ToDateTime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EHR_Entry_DateTime"]))
                                                            {
                                                                dtDtxRow["InsUptDlt"] = 2;
                                                                //if (m < 50)
                                                                //{
                                                                //    Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + " EHR_Entry_DateTime Updated ");
                                                                //}
                                                            }
                                                            else if (Convert.ToDateTime(dtDtxRow["nextvisit_date"].ToString().Trim()) != (row[0]["nextvisit_date"] != null && row[0]["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(row[0]["nextvisit_date"]) : Convert.ToDateTime(dtDtxRow["nextvisit_date"].ToString().Trim())))
                                                            {
                                                                dtDtxRow["InsUptDlt"] = 2;
                                                                //if (m < 50)
                                                                //{
                                                                //    Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + " nextvisit_date Updated ");
                                                                //}
                                                            }
                                                            else if ((dtDtxRow["EHR_Status"].ToString().Trim()) != (row[0]["EHR_Status"].ToString().Trim()))
                                                            {
                                                                dtDtxRow["InsUptDlt"] = 2;
                                                                //if (m < 50)
                                                                //{
                                                                //    Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + " EHR_Status Updated ");
                                                                //}
                                                            }
                                                            else if ((dtDtxRow["due_date"].ToString().Trim()) != (row[0]["due_date"].ToString().Trim()))
                                                            {
                                                                dtDtxRow["InsUptDlt"] = 2;
                                                                //if (m < 50)
                                                                //{
                                                                //    Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + " due_date Updated ");
                                                                //}
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
                                                    catch (Exception)
                                                    {
                                                        Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + ", EHR_Entry_DateTime = " + dtDtxRow["EHR_Entry_DateTime"].ToString() + "&" + row[0]["EHR_Entry_DateTime"].ToString());
                                                        Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + ", nextvisit_date = " + dtDtxRow["nextvisit_date"].ToString() + "&" + row[0]["nextvisit_date"].ToString());
                                                        Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + ", EHR_Status = " + dtDtxRow["EHR_Status"].ToString() + "&" + row[0]["EHR_Status"].ToString());
                                                        Utility.WriteToSyncLogFile_All(" PatientEHRId : " + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + ", due_date = " + dtDtxRow["due_date"].ToString() + "&" + row[0]["due_date"].ToString());
                                                    }
                                                    m = m + 1;
                                                }
                                                Utility.WriteToSyncLogFile_All("Appointment Patient Check for Delete Patient : " + System.DateTime.Now.ToString());

                                                if (dtOpenDentalAppointmensPatient.Rows.Count > 0 && dtOpenDentalAppointmensPatient.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                                                {
                                                    DataTable dtResult = dtOpenDentalAppointmensPatient.Select("InsUptDlt IN (1,2,3)").CopyToDataTable();
                                                    Utility.WriteToSyncLogFile_All("Patient sent for Save and update : " + System.DateTime.Now.ToString() + " with Count " + dtResult.Rows.Count.ToString());

                                                    bool status = SynchOpenDentalBAL.Save_PatientAppointment_OpenDental_To_Local(dtResult, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                                    Utility.WriteToSyncLogFile_All("Patient inserted or updated : " + System.DateTime.Now.ToString());
                                                    if (status)
                                                    {
                                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment Patient");
                                                        ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + "  to Local Database) Successfully.");
                                                        // SynchDataLiveDB_Push_PatientMultiLocation();
                                                    }
                                                    else
                                                    {
                                                        ObjGoalBase.WriteToErrorLogFile("[Appointment Patient Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                                    }
                                                }

                                            }

                                        }
                                        #endregion
                                        Is_synched_AppointmentsPatient = false;
                                    }
                                }
                            }
                        }
                    }
                    if (Utility.DtLocationList.Rows.Count > 1)
                    {
                        SynchDataLiveDB_Push_PatientMultiLocation();
                    }
                    else
                    {
                        SynchDataLiveDB_Push_Patient();
                    }
                }

            }
            catch (Exception ex)
            {
                Is_synched_AppointmentsPatient = false;
                ObjGoalBase.WriteToErrorLogFile("[Appointment's Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }


        public static void ExecuteQuery(string InsertTableName, DataRow dtDtxRow, SqlCeCommand SqlCeCommand, string Service_Install_Id)
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


                        //if ( dtDtxRow["EHR_Status"].ToString().ToLower()=="active" && dtDtxRow["is_deleted"].ToString().ToLower() == "false")
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
                    //MessageBox.Show(SqlCeCommand.Parameters["Clinic_Number"].Value.ToString());
                    //MessageBox.Show(SqlCeCommand.Parameters["Service_Install_Id"].Value.ToString());
                    //MessageBox.Show(SqlCeCommand.Parameters["patient_ehr_id"].Value.ToString());
                    SqlCeCommand.ExecuteNonQuery();



                    if (InsertTableName.ToString().ToUpper() == "PATIENT")
                    {
                        dtDtxRow["Service_Install_Id"] = Service_Install_Id;
                        SqlCeCommand.CommandText = "Update Patient set Is_deleted = 1,Is_Adit_Updated = 0 where Patient_EHR_id = @Patient_EHR_id and Service_Install_Id = @Service_Install_Id and Clinic_Number != @Clinic_Number";
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Patient_EHR_id", dtDtxRow["patient_ehr_id"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", dtDtxRow["Clinic_Number"].ToString().Trim());
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                    }
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

        public void SynchDataOpenDental_PatientDisease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientDisease)
                {
                    Is_synched_PatientDisease = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalDiseaseMain = SynchOpenDentalBAL.GetOpenDentalPatientDiseaseData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtOpenDentalDiseaseMain.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalDiseaseMain = SynchLocalBAL.GetLocalPatientDiseaseData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {
                                    DataTable dtOpenDentalDisease = new DataTable(); //dtOpenDentalDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                                    if (dtOpenDentalDiseaseMain.Rows.Count > 0 && dtOpenDentalDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Length > 0)
                                    {
                                        dtOpenDentalDisease = dtOpenDentalDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                                    }
                                    else
                                    {
                                        dtOpenDentalDisease = dtOpenDentalDiseaseMain.Clone();
                                    }
                                    DataTable dtLocalDisease = new DataTable();
                                    if (dtLocalDiseaseMain.Rows.Count > 0 && dtLocalDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Length > 0)
                                    {
                                        dtLocalDisease = dtLocalDiseaseMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                                    }
                                    else
                                    {
                                        dtLocalDisease = dtLocalDiseaseMain.Clone();
                                    }

                                    DataTable dtSaveRecords = new DataTable();
                                    dtSaveRecords = dtLocalDisease.Clone();

                                    var itemsToBeAdded = (from OpenDental in dtOpenDentalDisease.AsEnumerable()
                                                          join Local in dtLocalDisease.AsEnumerable()
                                                          on OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDental["Disease_Type"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim() + "_" + OpenDental["EHR_Entry_DateTime"].ToString().Trim()
                                                          equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim() + "_" + Local["EHR_Entry_DateTime"].ToString().Trim()
                                                          //on new { DisId = OpenDental["Disease_EHR_ID"].ToString().Trim(), PatID = OpenDental["Patient_EHR_ID"].ToString().Trim(), DisType = OpenDental["Disease_Type"].ToString().Trim(), Clinic_Number = OpenDental["Clinic_Number"].ToString().Trim(), EHRDT = OpenDental["EHR_Entry_DateTime"].ToString().Trim() }
                                                          //equals new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), PatID = Local["Patient_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), Clinic_Number = Local["Clinic_Number"].ToString().Trim(), EHRDT = Local["EHR_Entry_DateTime"].ToString().Trim() }
                                                          into matchingRows
                                                          from matchingRow in matchingRows.DefaultIfEmpty()
                                                          where matchingRow == null
                                                          select OpenDental).ToList();
                                    DataTable dtitemToBeAdded = dtLocalDisease.Clone();
                                    if (itemsToBeAdded.Count > 0)
                                    {
                                        dtitemToBeAdded = itemsToBeAdded.CopyToDataTable<DataRow>();
                                    }
                                    if (!dtitemToBeAdded.Columns.Contains("InsUptDlt"))
                                    {
                                        dtitemToBeAdded.Columns.Add("InsUptDlt", typeof(int));
                                        dtitemToBeAdded.Columns["InsUptDlt"].DefaultValue = 0;
                                    }
                                    if (dtitemToBeAdded.Rows.Count > 0)
                                    {
                                        dtitemToBeAdded.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 1);
                                        dtSaveRecords.Load(dtitemToBeAdded.Select().CopyToDataTable().CreateDataReader());
                                    }

                                    //Update
                                    var itemsToBeUpdated = (from OpenDental in dtOpenDentalDisease.AsEnumerable()
                                                            join Local in dtLocalDisease.AsEnumerable()
                                                            on OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDental["Disease_Type"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim() + "_" + OpenDental["EHR_Entry_DateTime"].ToString().Trim()
                                                            equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim() + "_" + Local["EHR_Entry_DateTime"].ToString().Trim()
                                                            //on new { DisId = OpenDental["Disease_EHR_ID"].ToString().Trim(), PatID = OpenDental["Patient_EHR_ID"].ToString().Trim(), DisType = OpenDental["Disease_Type"].ToString().Trim(), Clinic_Number = OpenDental["Clinic_Number"].ToString().Trim(), EHRDT = OpenDental["EHR_Entry_DateTime"].ToString().Trim() }
                                                            //equals new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), PatID = Local["Patient_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), Clinic_Number = Local["Clinic_Number"].ToString().Trim(), EHRDT = Local["EHR_Entry_DateTime"].ToString().Trim() }
                                                            where
                                                           OpenDental["Patient_EHR_ID"].ToString().Trim() != Local["Patient_EHR_ID"].ToString().Trim() ||
                                                           OpenDental["Disease_EHR_ID"].ToString().Trim() != Local["Disease_EHR_ID"].ToString().Trim() ||
                                                           OpenDental["Disease_Name"].ToString().Trim() != Local["Disease_Name"].ToString().Trim() ||
                                                           OpenDental["Disease_Type"].ToString().Trim() != Local["Disease_Type"].ToString().Trim() ||
                                                           OpenDental["EHR_Entry_DateTime"].ToString().Trim() != Local["EHR_Entry_DateTime"].ToString().Trim()
                                                            select OpenDental).ToList();
                                    DataTable dtitemToBeUpdated = dtLocalDisease.Clone();
                                    if (itemsToBeUpdated.Count > 0)
                                    {
                                        dtitemToBeUpdated = itemsToBeUpdated.CopyToDataTable<DataRow>();
                                    }
                                    if (!dtitemToBeUpdated.Columns.Contains("InsUptDlt"))
                                    {
                                        dtitemToBeUpdated.Columns.Add("InsUptDlt", typeof(int));
                                        dtitemToBeUpdated.Columns["InsUptDlt"].DefaultValue = 0;
                                    }

                                    if (dtitemToBeUpdated.Rows.Count > 0)
                                    {
                                        dtitemToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 2);
                                        dtSaveRecords.Load(dtitemToBeUpdated.Select().CopyToDataTable().CreateDataReader());
                                    }

                                    //Delete
                                    var itemToBeDeleted = (from Local in dtLocalDisease.AsEnumerable()
                                                           join OpenDental in dtOpenDentalDisease.AsEnumerable()
                                                           on Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim() + "_" + Local["EHR_Entry_DateTime"].ToString().Trim()
                                                           equals OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDental["Disease_Type"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim() + "_" + OpenDental["EHR_Entry_DateTime"].ToString().Trim()
                                                           //on new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), PatID = Local["Patient_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), Clinic_Number = Local["Clinic_Number"].ToString().Trim(), EHRDT = Local["EHR_Entry_DateTime"].ToString().Trim() }
                                                           //equals new { DisId = OpenDental["Disease_EHR_ID"].ToString().Trim(), PatID = OpenDental["Patient_EHR_ID"].ToString().Trim(), DisType = OpenDental["Disease_Type"].ToString().Trim(), Clinic_Number = OpenDental["Clinic_Number"].ToString().Trim(), EHRDT = OpenDental["EHR_Entry_DateTime"].ToString().Trim() }
                                                           into matchingRows
                                                           from matchingRow in matchingRows.DefaultIfEmpty()
                                                           where Local["is_deleted"].ToString().Trim().ToUpper() == "FALSE" &&
                                                                 matchingRow == null
                                                           select Local).ToList();
                                    DataTable dtitemToBeDeleted = dtLocalDisease.Clone();
                                    if (itemToBeDeleted.Count > 0)
                                    {
                                        dtitemToBeDeleted = itemToBeDeleted.CopyToDataTable<DataRow>();
                                    }
                                    if (!dtitemToBeDeleted.Columns.Contains("InsUptDlt"))
                                    {
                                        dtitemToBeDeleted.Columns.Add("InsUptDlt", typeof(int));
                                        dtitemToBeDeleted.Columns["InsUptDlt"].DefaultValue = 0;
                                    }

                                    if (dtitemToBeDeleted.Rows.Count > 0)
                                    {
                                        dtitemToBeDeleted.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 3);
                                        dtSaveRecords.Load(dtitemToBeDeleted.Select().CopyToDataTable().CreateDataReader());
                                    }

                                    if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                                    {
                                        bool status = SynchOpenDentalBAL.Save_PatientDisease_OpenDental_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                        if (status)
                                        {
                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                                            ObjGoalBase.WriteToSyncLogFile("PatientDisease Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                            //SynchDataLiveDB_Push_PatientDisease();
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientDisease Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Is_synched_PatientDisease = false;
                    try
                    {
                        SynchDataLiveDB_Push_PatientDisease();
                    }
                    catch (Exception exPush)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientDisease Push " + exPush.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatientDisease = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientDisease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataOpenDental_PatientMedication(string Patient_EHR_IDS)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedicationMain = SynchOpenDentalBAL.GetOpenDentalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patient_EHR_IDS);
                        dtMedicationMain.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedicationMain = SynchLocalBAL.GetLocalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patient_EHR_IDS);

                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"].ToString() != null && Convert.ToBoolean(Utility.DtLocationList.Rows[i]["AditLocationSyncEnable"]))
                            {
                                if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString())
                                {
                                    DataTable dtMedication = new DataTable();
                                    if (dtMedicationMain.Rows.Count > 0 && dtMedicationMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Length > 0)
                                    {
                                        dtMedication = dtMedicationMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                                    }
                                    else
                                    {
                                        dtMedication = dtMedicationMain.Clone();
                                    }
                                    DataTable dtLocalMedication = new DataTable();
                                    if (dtLocalMedicationMain.Rows.Count > 0 && dtLocalMedicationMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").Length > 0)
                                    {
                                        dtLocalMedication = dtLocalMedicationMain.Select("Clinic_Number = '" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString() + "'").CopyToDataTable();
                                    }
                                    else
                                    {
                                        dtLocalMedication = dtLocalMedicationMain.Clone();
                                    }
                                    DataTable dtSaveRecords = new DataTable();
                                    dtSaveRecords = dtLocalMedication.Clone();

                                    //Insert
                                    var itemsToBeAdded = (from OpenDentalMedication in dtMedication.AsEnumerable()
                                                          join LocalMedication in dtLocalMedication.AsEnumerable()
                                                          on OpenDentalMedication["PatientMedication_EHR_ID"].ToString().Trim() equals LocalMedication["PatientMedication_EHR_ID"].ToString().Trim() into matchingRows
                                                          from matchingRow in matchingRows.DefaultIfEmpty()
                                                          where matchingRow == null
                                                          select OpenDentalMedication).ToList();
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
                                    //Update
                                    var itemsToBeUpdated = (from OpenDentalPatient in dtMedication.AsEnumerable()
                                                            join LocalPatient in dtLocalMedication.AsEnumerable()
                                                            on OpenDentalPatient["PatientMedication_EHR_ID"].ToString().Trim() equals LocalPatient["PatientMedication_EHR_ID"].ToString().Trim()
                                                            where
                                                            OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() != LocalPatient["Patient_EHR_ID"].ToString().Trim() ||
                                                            OpenDentalPatient["Medication_EHR_ID"].ToString().Trim() != LocalPatient["Medication_EHR_ID"].ToString().Trim() ||
                                                            OpenDentalPatient["Medication_Note"].ToString().Trim() != LocalPatient["Medication_Note"].ToString().Trim() ||
                                                            OpenDentalPatient["Medication_Name"].ToString().Trim() != LocalPatient["Medication_Name"].ToString().Trim() ||
                                                            OpenDentalPatient["Medication_Type"].ToString().Trim() != LocalPatient["Medication_Type"].ToString().Trim() ||
                                                            OpenDentalPatient["Provider_EHR_ID"].ToString().Trim() != LocalPatient["Provider_EHR_ID"].ToString().Trim() ||
                                                            OpenDentalPatient["Drug_Quantity"].ToString().Trim() != LocalPatient["Drug_Quantity"].ToString().Trim() ||
                                                            OpenDentalPatient["Start_Date"].ToString().Trim() != LocalPatient["Start_Date"].ToString().Trim() ||
                                                            OpenDentalPatient["End_Date"].ToString().Trim() != LocalPatient["End_Date"].ToString().Trim() ||
                                                            OpenDentalPatient["Patient_Notes"].ToString().Trim() != LocalPatient["Patient_Notes"].ToString().Trim() ||
                                                            OpenDentalPatient["is_active"].ToString().Trim() != LocalPatient["is_active"].ToString().Trim() ||
                                                            OpenDentalPatient["EHR_Entry_DateTime"].ToString().Trim() != LocalPatient["EHR_Entry_DateTime"].ToString().Trim()
                                                            select OpenDentalPatient).ToList();
                                    DataTable dtPatientToBeUpdated = dtLocalMedication.Clone();
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
                                    //Delete
                                    var itemToBeDeleted = (from LocalMedication in dtLocalMedication.AsEnumerable()
                                                           join OpenDentalMedication in dtMedication.AsEnumerable()
                                                           on LocalMedication["PatientMedication_EHR_ID"].ToString().Trim() equals OpenDentalMedication["PatientMedication_EHR_ID"].ToString().Trim() into matchingRows
                                                           from matchingRow in matchingRows.DefaultIfEmpty()
                                                           where LocalMedication["is_deleted"].ToString().Trim().ToUpper() == "FALSE" &&
                                                                 matchingRow == null
                                                           select LocalMedication).ToList();
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
                                            //SynchDataLiveDB_Push_PatientMedication();
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[PatientMedication Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                                        }
                                    }
                                }
                            }
                        }
                        Is_synched_PatientMedication = false;
                    }
                    try
                    {
                        SynchDataLiveDB_Push_PatientMedication();
                    }
                    catch (Exception exPush)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[PatientMedication Push (" + Utility.Application_Name + " to Local Database) ]" + exPush.Message);
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

        public static string Insert_Patient = " INSERT INTO Patient "
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

        public static string Update_Patient = " UPDATE Patient SET "
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
                                                       //+ " Clinic_Number = @Clinic_Number, "
                                                       + " [is_deleted] = @is_deleted, "
                                                       + " [EHR_Status] = @EHR_Status, "
                                                       + " [ReceiveVoiceCall] = @ReceiveVoiceCall, "
                                                       + " [PreferredLanguage] = @PreferredLanguage,[Patient_Note] = @Patient_Note, "
                                                       + " ssn = @ssn,driverlicense = @driverlicense,groupid = @groupid ,emergencycontactId = @emergencycontactId ,EmergencyContact_First_Name = @EmergencyContact_First_Name,EmergencyContact_Last_Name = @EmergencyContact_Last_Name , "
                                                       + " emergencycontactnumber = @emergencycontactnumber,school = @school ,employer = @employer ,spouseId = @spouseId ,responsiblepartyId = @responsiblepartyId , "
                                                       + " responsiblepartyssn = @responsiblepartyssn ,responsiblepartybirthdate = @responsiblepartybirthdate,Spouse_First_Name = @Spouse_First_Name , "
                                                       + " Spouse_Last_Name = @Spouse_Last_Name ,ResponsibleParty_First_Name = @ResponsibleParty_First_Name , ResponsibleParty_Last_Name = @ResponsibleParty_Last_Name, "
                                                       + " Prim_Ins_Company_Phonenumber = @Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber = @Sec_Ins_Company_Phonenumber "
                                                       + " WHERE patient_ehr_id = @patient_ehr_id and Service_Install_Id = @Service_Install_Id and Clinic_Number =@Clinic_Number ";


        public static string Delete_Patient = " Delete From Patient WHERE Patient_EHR_ID = @Patient_EHR_ID And Service_Install_Id = @Service_Install_Id   ";

        public static string PatientCompareQuery = " SELECT ACC.* FROM ( SELECT (CASE WHEN PT.Patient_EHR_id IS NULL THEN 1 ELSE 2 END) AS InsUptDlt, PC.* FROM (SELECT Distinct Service_Install_Id,[Patient_EHR_ID],Clinic_Number "
                                           + "  FROM (SELECT DISTINCT Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                           + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                           + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note] ,"
                                           + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber,Clinic_Number "
                                           + "  FROM Patient where Service_Install_Id = @Service_Install_Id and Clinic_Number = @Clinic_Number UNION All  "
                                           + "  SELECT DISTINCT Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                           + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                           + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note],  "
                                           + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber,Clinic_Number "
                                           + "   FROM PatientCompare where Service_Install_Id = @Service_Install_Id and Clinic_Number = @Clinic_Number) data "
                                           + "  GROUP BY  Service_Install_Id,[Patient_EHR_ID],[is_deleted],[First_name],[Last_name],[Middle_Name],[Salutation],[Status],[Sex],[MaritalStatus],[Birth_Date],[Email],[Mobile],[Home_Phone],[Work_Phone],[Address1],[Address2], "
                                           + "    [City],[State],[Zipcode],[ResponsibleParty_Status],[CurrentBal],[ThirtyDay],[SixtyDay],[NinetyDay],[Over90],[FirstVisit_Date],[LastVisit_Date],[Primary_Insurance],[Primary_Insurance_CompanyName],[Secondary_Insurance],[Secondary_Insurance_CompanyName], "
                                           + "  [Guar_ID],[Pri_Provider_ID],[Sec_Provider_ID],[ReceiveSms],[ReceiveEmail],[nextvisit_date],[due_date],[remaining_benefit],[collect_payment],[preferred_name],[used_benefit],[Secondary_Ins_Subscriber_ID],[Primary_Ins_Subscriber_ID],[EHR_Status],[ReceiveVoiceCall],[PreferredLanguage],[Patient_Note],  "
                                           + "  ssn ,driverlicense,groupid ,emergencycontactId ,EmergencyContact_First_Name,EmergencyContact_Last_Name ,emergencycontactnumber,school ,employer ,spouseId ,responsiblepartyId ,responsiblepartyssn ,responsiblepartybirthdate,Spouse_First_Name ,Spouse_Last_Name ,ResponsibleParty_First_Name ,ResponsibleParty_Last_Name, Prim_Ins_Company_Phonenumber ,Sec_Ins_Company_Phonenumber,Clinic_Number "
                                           + "  HAVING count(1) = 1 ) AS RS "
                                           + "  LEFT JOIN Patient PT ON Rs.Service_Install_Id = PT.Service_Install_Id And RS.Patient_EHR_Id = PT.Patient_EHR_Id And RS.Clinic_Number = PT.Clinic_Number"
                                           + "  LEFT JOIN PatientCompare PC ON Pc.Service_Install_Id = Rs.Service_Install_Id And PC.Patient_EHR_Id = RS.Patient_EHR_Id and PC.Clinic_Number = RS.Clinic_Number) AS ACC WHERE Patient_EHR_id != '' And Service_Install_Id = @Service_Install_Id and Clinic_Number = @Clinic_Number";

        #endregion

        #endregion

        #region Sync Patient Document

        public void SynchDataLocalToOpenDental_Patient_Document()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        try
                        {
                            //live to local data pull
                            SynchDataLiveDB_Pull_treatmentDoc();
                            SynchDataLiveDB_Pull_InsuranceCarrierDoc(); 
                            //live to local PDF pull
                            SyncTreatmentDocument();
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                            // throw;
                        }
                        DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtWebPatient_Form.Columns.Add(new DataColumn("Table_Name", typeof(string)));

                        try
                        {
                            // GetTreatmentDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            GetPatientDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            GetPatientDocument_New(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            SynchOpenDentalBAL.Save_Document_in_OpenDental(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());                          
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }

                        #region Treatment Document
                        try
                        {
                            //Sync data from local to opendental
                            SynchOpenDentalBAL.Save_Treatment_Document_in_OpenDental(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());

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
                            //Sync data from local to opendental
                            SynchOpenDentalBAL.Save_InsuranceCarrier_Document_in_OpenDental(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());

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
                            ObjGoalBase.WriteToErrorLogFile("[InsuranceCarrier Document Error log] : " + ex.Message);
                            // throw;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #endregion

        #region Patient Form

        private void fncSynchDataLocalToOpenDental_Patient_Form()
        {
            InitBgWorkerLocalToOpenDental_Patient_Form();
            InitBgTimerLocalToOpenDental_Patient_Form();
        }

        private void InitBgTimerLocalToOpenDental_Patient_Form()
        {
            timerSynchLocalToOpenDental_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToOpenDental_Patient_Form.Interval = 1000 * GoalBase.intervalEHRSynch_PatientForm;
            this.timerSynchLocalToOpenDental_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToOpenDental_Patient_Form_Tick);
            timerSynchLocalToOpenDental_Patient_Form.Enabled = true;
            timerSynchLocalToOpenDental_Patient_Form.Start();
            timerSynchLocalToOpenDental_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToOpenDental_Patient_Form()
        {
            bwSynchLocalToOpenDental_Patient_Form = new BackgroundWorker();
            bwSynchLocalToOpenDental_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToOpenDental_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToOpenDental_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToOpenDental_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToOpenDental_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToOpenDental_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToOpenDental_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToOpenDental_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToOpenDental_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToOpenDental_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToOpenDental_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToOpenDental_Patient_Form);
            procThreadmainLocalToOpenDental_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToOpenDental_Patient_Form()
        {
            if (bwSynchLocalToOpenDental_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToOpenDental_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToOpenDental_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToOpenDental_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToOpenDental_Patient_Form();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToOpenDental_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToOpenDental_Patient_Form.Enabled = true;
        }

        public void SynchDataLocalToOpenDental_Patient_Form()
        {
            try
            {
                 //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff)//&& Utility.AditLocationSyncEnable
                {
                    SynchDataLiveDB_Pull_PatientForm();
                    SynchDataLiveDB_Pull_PatientPortal();
                    //try
                    //{
                    //    //live to local data pull
                    //    SynchDataLiveDB_Pull_treatmentDoc();

                    //    //live to local PDF pull
                    //    SyncTreatmentDocument();
                    //}
                    //catch (Exception ex)
                    //{
                    //    ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                    //    // throw;
                    //}
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
                                    DataTable dtIdelProv = SynchOpenDentalBAL.GetOpenDentalIdelProvider(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                                    ObjGoalBase.WriteToErrorLogFile("Default Provider for PF is = " + tmpIdelProv);
                                    dtDtxRow["ehrfield_value"] = tmpIdelProv.Trim() == "" ? "1" : tmpIdelProv;
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
                            bool Is_Record_Update = SynchOpenDentalBAL.Save_Patient_Form_Local_To_OpenDental(dtWebPatient_Form, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }

                        try
                        {
                            GetMedicalOpenDentalHistoryRecords(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            SynchOpenDentalBAL.SaveMedicalHistoryLocalToOpenDental(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            ObjGoalBase.WriteToSyncLogFile("Medical_History_Save Sync ( Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".  Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        catch (Exception ex2)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Medical_History_Save Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex2.Message);
                        }

                        try
                        {
                            if (SynchOpenDentalBAL.SavePatientDisease(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()))
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
                            if (SynchOpenDentalBAL.DeletePatientDisease(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()))
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
                            if (SynchOpenDentalBAL.DeletePatientMedication(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), ref isRecordDeleted, ref DeletePatientEHRID))
                            {
                                ObjGoalBase.WriteToSyncLogFile("Delete_Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]");
                            }
                        }
                        catch (Exception ex1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                        }
                        try
                        {
                            if (SynchOpenDentalBAL.SavePatientMedication(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), ref isRecordSaved, ref SavePatientEHRID))
                            {
                                ObjGoalBase.WriteToSyncLogFile("Save Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Save Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]");
                            }
                        }
                        catch (Exception ex1)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Save Patient_Medication Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                        }
                        if (isRecordDeleted || isRecordSaved)
                        {
                            Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                            if (Patient_EHR_IDS != "")
                            {
                                SynchDataOpenDental_PatientMedication(Patient_EHR_IDS);
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

                        //try
                        //{
                        //    // GetTreatmentDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        //    GetPatientDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        //    SynchOpenDentalBAL.Save_Document_in_OpenDental(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());

                        //    //Sync data from local to opendental
                        //    SynchOpenDentalBAL.Save_Treatment_Document_in_OpenDental(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());
                        //}
                        //catch (Exception ex)
                        //{
                        //    ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        //}


                        SynchDataLocalToOpenDental_Patient_Document();
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
                    try
                    {
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

                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }
        #endregion

        #region PatientPayment
        private void fncSynchDataOpenDental_PatientPayment()
        {
            InitBgWorkerOpenDental_PatientPayment();
            InitBgTimerOpenDental_PatientPayment();
        }

        private void InitBgTimerOpenDental_PatientPayment()
        {
            timerSynchOpenDental_PatientPayment = new System.Timers.Timer();
            this.timerSynchOpenDental_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
            this.timerSynchOpenDental_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_PatientPayment_Tick);
            timerSynchOpenDental_PatientPayment.Enabled = true;
            timerSynchOpenDental_PatientPayment.Start();
        }

        private void InitBgWorkerOpenDental_PatientPayment()
        {
            bwSynchOpenDental_PatientPayment = new BackgroundWorker();
            bwSynchOpenDental_PatientPayment.WorkerReportsProgress = true;
            bwSynchOpenDental_PatientPayment.WorkerSupportsCancellation = true;
            bwSynchOpenDental_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchOpenDental_PatientPayment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOpenDental_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_PatientPayment_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_PatientPayment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_PatientPayment.Enabled = false;
                MethodForCallSynchOrderOpenDental_PatientPayment();
            }
        }

        public void MethodForCallSynchOrderOpenDental_PatientPayment()
        {
            System.Threading.Thread procThreadmainOpenDental_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_PatientPayment);
            procThreadmainOpenDental_PatientPayment.Start();
        }

        public void CallSyncOrderTableOpenDental_PatientPayment()
        {
            if (bwSynchOpenDental_PatientPayment.IsBusy != true)
            {
                bwSynchOpenDental_PatientPayment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_PatientPayment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToOpenDental_PatientPayment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_PatientPayment.Enabled = true;
        }

        public void SynchDataLocalToOpenDental_PatientPayment()
        {
            try
            {
                if (!IsPaymentSyncing)
                {
                    IsPaymentSyncing = true;
                    //CheckEntryUserLoginIdExist();
                    SynchDataLiveDB_Pull_PatientPaymentLog();
                    if (!Is_synched_PatinetForm)
                    {
                        if (Utility.IsApplicationIdleTimeOff)
                        {
                            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                            {
                                DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (dtWebPatientPayment.Rows.Count > 0 && dtWebPatientPayment.Rows.Count > 0)
                                {
                                    Utility.CheckEntryUserLoginIdExist();
                                    bool Is_Record_Update = SynchOpenDentalBAL.Save_PatientPayment_Local_To_OpenDental(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

                                ObjGoalBase.WriteToSyncLogFile("Patient Payment Sync (Service Install Id : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + ".Local Database To " + Utility.Application_Name + ") Successfully.");

                            }
                        }
                    }
                    IsPaymentSyncing = false;
                }
            }
            catch (Exception ex)
            {
                IsPaymentSyncing = false;
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
            finally { IsPaymentSyncing = false; }
        }

        public void SynchDataOpenDental_PatientImages()
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
                        DataTable dtOpenDentalPatientImages = SynchOpenDentalBAL.GetOpenDentalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtOpenDentalPatientImages.Columns.Add("InsUptDlt", typeof(int));
                        dtOpenDentalPatientImages.Columns.Add("SourceLocation", typeof(string));
                        dtOpenDentalPatientImages.Columns["InsUptDlt"].DefaultValue = 0;
                        DataTable dtLocalPatientImages = SynchLocalBAL.GetLocalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString());
                        Utility.EHRProfileImagePath = SynchOpenDentalDAL.GetOpenDentalDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        foreach (DataRow dtDtxRow in dtOpenDentalPatientImages.Rows)
                        {
                            if (Utility.EHRProfileImagePath == string.Empty || Utility.EHRProfileImagePath == "")
                            {
                                dtDtxRow["SourceLocation"] = "C:\\OpenDentImages\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                            }
                            else
                            {
                                if (!Directory.Exists(Utility.EHRProfileImagePath))
                                {
                                    //Utility.WriteToErrorLogFromAll(Utility.EHRProfileImagePath.ToString());
                                    dtDtxRow["SourceLocation"] = "C:\\OpenDentImages\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                                }
                                else
                                {
                                    dtDtxRow["SourceLocation"] = Utility.EHRProfileImagePath + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                                }
                            }
                            //Utility.WriteToErrorLogFromAll(dtDtxRow["SourceLocation"].ToString());
                            DataRow[] row = dtLocalPatientImages.Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
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
                            DataRow[] row = dtOpenDentalPatientImages.Select("Patient_EHR_ID = '" + dtDtlRow["Patient_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length <= 0)
                            {
                                if (!Convert.ToBoolean(dtDtlRow["Is_Deleted"]))
                                {
                                    DataRow ApptDtldr = dtOpenDentalPatientImages.NewRow();
                                    ApptDtldr["Patient_EHR_ID"] = dtDtlRow["Patient_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Patient_Images_EHR_ID"] = dtDtlRow["Patient_Images_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Image_EHR_Name"] = dtDtlRow["Image_EHR_Name"].ToString().Trim();
                                    ApptDtldr["Clinic_Number"] = dtDtlRow["Clinic_Number"].ToString().Trim();
                                    ApptDtldr["Service_Install_Id"] = dtDtlRow["Service_Install_Id"].ToString().Trim();
                                    ApptDtldr["Is_Deleted"] = 1;
                                    ApptDtldr["InsUptDlt"] = 3;
                                    dtOpenDentalPatientImages.Rows.Add(ApptDtldr);
                                }

                            }
                        }

                        dtOpenDentalPatientImages.AcceptChanges();
                        bool status = false;
                        DataTable dtSaveRecords = dtOpenDentalPatientImages.Clone();
                        if (dtOpenDentalPatientImages.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtOpenDentalPatientImages.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
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

        public void SynchDataPatientPayment_LocalTOOpenDental()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
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
                            noteId = "";


                            bool Is_Record_Update = SynchOpenDentalBAL.Save_PatientPayment_Local_To_OpenDental(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());


                            #region Call API for EHR Entry Done

                            //noteId = SynchOpenDentalBAL.Save_PatientPaymentLog_LocalToOpenDental(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            //if (noteId != "")
                            //{
                            //    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientPayment_LocalTOOpenDental");
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

        public void SynchDataPatientSMSCall_LocalTOOpenDental()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    //CheckEntryUserLoginIdExist();
                    Int64 TransactionHeaderId = 0;
                    string noteId = "";
                    DataTable dtPatientPayment = new DataTable();
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        SynchOpenDentalBAL.DeleteDuplicatePatientLog(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        // ObjGoalBase.WriteToSyncLogFile("Get Records");
                        DataTable dtWebPatientSMSCallLog = SynchLocalBAL.GetLocalWebPatientSMSCallLogData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        // ObjGoalBase.WriteToSyncLogFile("Total Records to be saved in EHR " + dtWebPatientSMSCallLog.Rows.Count.ToString());
                        #region Call API for EHR Entry Done
                        if (dtWebPatientSMSCallLog != null && dtWebPatientSMSCallLog.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                            // System.Windows.Forms.MessageBox.Show("0");
                            SynchOpenDentalBAL.Save_PatientSMSCallLog_LocalToOpenDental(dtWebPatientSMSCallLog, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            // }
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientSMSCallLog_LocalTOOpenDental");
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

        private void fncSynchDataOpenDental_Disease()
        {
            InitBgWorkerOpenDental_Disease();
            InitBgTimerOpenDental_Disease();
        }

        private void InitBgTimerOpenDental_Disease()
        {
            timerSynchOpenDental_Disease = new System.Timers.Timer();
            this.timerSynchOpenDental_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchOpenDental_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_Disease_Tick);
            timerSynchOpenDental_Disease.Enabled = true;
            timerSynchOpenDental_Disease.Start();
        }

        private void InitBgWorkerOpenDental_Disease()
        {
            bwSynchOpenDental_Disease = new BackgroundWorker();
            bwSynchOpenDental_Disease.WorkerReportsProgress = true;
            bwSynchOpenDental_Disease.WorkerSupportsCancellation = true;
            bwSynchOpenDental_Disease.DoWork += new DoWorkEventHandler(bwSynchOpenDental_Disease_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchOpenDental_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_Disease_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_Disease_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_Disease.Enabled = false;
                MethodForCallSynchOrderOpenDental_Disease();
            }
        }

        public void MethodForCallSynchOrderOpenDental_Disease()
        {
            System.Threading.Thread procThreadmainOpenDental_Disease = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_Disease);
            procThreadmainOpenDental_Disease.Start();
        }

        public void CallSyncOrderTableOpenDental_Disease()
        {
            if (bwSynchOpenDental_Disease.IsBusy != true)
            {
                bwSynchOpenDental_Disease.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_Disease_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_Disease.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_Disease();
                //  SynchDataOpenDental_DiseaseHours();
                //  SynchDataOpenDental_Medication();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_Disease.Enabled = true;
        }

        public void SynchDataOpenDental_Disease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalDisease = SynchOpenDentalBAL.GetOpenDentalDiseaseData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtOpenDentalDisease.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalDisease = SynchLocalBAL.GetLocalDiseaseData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        DataTable dtSaveRecords = new DataTable();
                        dtSaveRecords = dtLocalDisease.Clone();

                        //Insert
                        var itemsToBeAdded = (from OpenDental in dtOpenDentalDisease.AsEnumerable()
                                              join Local in dtLocalDisease.AsEnumerable()
                                              on OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Disease_Type"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim()
                                              equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                                              //on new { DisId = OpenDental["Disease_EHR_ID"].ToString().Trim(), DisType = OpenDental["Disease_Type"].ToString().Trim(), ClinicNum = OpenDental["Clinic_Number"].ToString().Trim() }
                                              //equals new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), ClinicNum = Local["Clinic_Number"].ToString().Trim() }
                                              into matchingRows
                                              from matchingRow in matchingRows.DefaultIfEmpty()
                                              where matchingRow == null
                                              select OpenDental).ToList();
                        DataTable dtPatientToBeAdded = dtLocalDisease.Clone();
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

                        //Update
                        var itemsToBeUpdated = (from OpenDental in dtOpenDentalDisease.AsEnumerable()
                                                join Local in dtLocalDisease.AsEnumerable()
                                                on OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Disease_Type"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim()
                                                equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                                                //on new { DisId = OpenDental["Disease_EHR_ID"].ToString().Trim(), DisType = OpenDental["Disease_Type"].ToString().Trim(), ClinicNum = OpenDental["Clinic_Number"].ToString().Trim() }
                                                //equals new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), ClinicNum = Local["Clinic_Number"].ToString().Trim() }
                                                where
                                               OpenDental["EHR_Entry_DateTime"].ToString().Trim() != Local["EHR_Entry_DateTime"].ToString().Trim()
                                                select OpenDental).ToList();
                        DataTable dtPatientToBeUpdated = dtLocalDisease.Clone();
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

                        //Delete
                        var itemToBeDeleted = (from Local in dtLocalDisease.AsEnumerable()
                                               join OpenDental in dtOpenDentalDisease.AsEnumerable()
                                               on Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                                               equals OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Disease_Type"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim()
                                              //on new { DisId = Local["Disease_EHR_ID"].ToString().Trim(), DisType = Local["Disease_Type"].ToString().Trim(), ClinicNum = Local["Clinic_Number"].ToString().Trim() }
                                              //equals new { DisId = OpenDental["Disease_EHR_ID"].ToString().Trim(), DisType = OpenDental["Disease_Type"].ToString().Trim(), ClinicNum = OpenDental["Clinic_Number"].ToString().Trim() }
                                              into matchingRows
                                               from matchingRow in matchingRows.DefaultIfEmpty()
                                               where Local["is_deleted"].ToString().Trim().ToUpper() == "FALSE" &&
                                                     matchingRow == null
                                               select Local).ToList();
                        DataTable dtPatientToBeDeleted = dtLocalDisease.Clone();
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

                        //foreach (DataRow dtDtxRow in dtOpenDentalDisease.Rows)
                        //{
                        //    DataRow[] row = dtLocalDisease.Copy().Select("Disease_EHR_ID = '" + dtDtxRow["Disease_EHR_ID"] + "' AND Disease_Type = '" + dtDtxRow["Disease_Type"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
                        //    if (row.Length > 0)
                        //    {
                        //        if (dtDtxRow["EHR_Entry_DateTime"].ToString().Trim() != row[0]["EHR_Entry_DateTime"].ToString().Trim())
                        //        {
                        //            dtDtxRow["InsUptDlt"] = 2;
                        //        }
                        //        else
                        //        {
                        //            dtDtxRow["InsUptDlt"] = 0;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        dtDtxRow["InsUptDlt"] = 1;
                        //    }
                        //}

                        //foreach (DataRow dtLPHRow in dtLocalDisease.Rows)
                        //{
                        //    DataRow[] rowDis = dtOpenDentalDisease.Copy().Select("Disease_EHR_ID = '" + dtLPHRow["Disease_EHR_ID"] + "' AND Disease_Type = '" + dtLPHRow["Disease_Type"] + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"] + "' ");
                        //    if (rowDis.Length > 0)
                        //    { }
                        //    else
                        //    {
                        //        DataRow rowDisDtldr = dtOpenDentalDisease.NewRow();
                        //        rowDisDtldr["Disease_EHR_ID"] = dtLPHRow["Disease_EHR_ID"].ToString().Trim();
                        //        rowDisDtldr["Disease_Type"] = dtLPHRow["Disease_Type"].ToString().Trim();
                        //        rowDisDtldr["Disease_Name"] = dtLPHRow["Disease_Name"].ToString().Trim();
                        //        //  rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
                        //        rowDisDtldr["InsUptDlt"] = 3;
                        //        dtOpenDentalDisease.Rows.Add(rowDisDtldr);
                        //    }
                        //}
                        //dtOpenDentalDisease.AcceptChanges();

                        if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_Disease_OpenDental_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

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
                    SynchDataOpenDental_Medication();
                }
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }


        public void SynchDataOpenDental_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchOpenDentalBAL.GetOpenDentalMedicationData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        DataTable dtSaveRecords = new DataTable();
                        dtSaveRecords = dtLocalMedication.Clone();

                        //Insert
                        var itemsToBeAdded = (from OpenDental in dtMedication.AsEnumerable()
                                              join Local in dtLocalMedication.AsEnumerable()
                                              on OpenDental["Medication_EHR_ID"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim()
                                              equals Local["Medication_EHR_ID"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim() into matchingRows
                                              from matchingRow in matchingRows.DefaultIfEmpty()
                                              where matchingRow == null
                                              select OpenDental).ToList();
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

                        //Update
                        var itemsToBeUpdated = (from OpenDental in dtMedication.AsEnumerable()
                                                join Local in dtLocalMedication.AsEnumerable()
                                                on OpenDental["Medication_EHR_ID"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim()
                                                equals Local["Medication_EHR_ID"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                                                where
                                                OpenDental["Medication_Name"].ToString().ToUpper().Trim() != Local["Medication_Name"].ToString().ToUpper().Trim() ||
                                                OpenDental["Drug_Quantity"].ToString().ToUpper().Trim() != Local["Drug_Quantity"].ToString().ToUpper().Trim() ||
                                                OpenDental["Medication_Description"].ToString().ToUpper().Trim() != Local["Medication_Description"].ToString().ToUpper().Trim() ||
                                                OpenDental["Medication_Notes"].ToString().ToUpper().Trim() != Local["Medication_Notes"].ToString().ToUpper().Trim() ||
                                                OpenDental["Medication_Sig"].ToString().ToUpper().Trim() != Local["Medication_Sig"].ToString().ToUpper().Trim() ||
                                                OpenDental["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim() != Local["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim() ||
                                                OpenDental["Medication_Type"].ToString().ToUpper().Trim() != Local["Medication_Type"].ToString().ToUpper().Trim() ||
                                                OpenDental["Allow_Generic_Sub"].ToString().ToUpper().Trim() != Local["Allow_Generic_Sub"].ToString().ToUpper().Trim() ||
                                                OpenDental["Refills"].ToString().ToUpper().Trim() != Local["Refills"].ToString().ToUpper().Trim() ||
                                                OpenDental["Is_Active"].ToString().ToUpper().Trim() != Local["Is_Active"].ToString().ToUpper().Trim() ||
                                                OpenDental["Medication_Provider_ID"].ToString().ToUpper().Trim() != Local["Medication_Provider_ID"].ToString().ToUpper().Trim()
                                                select OpenDental).ToList();
                        DataTable dtPatientToBeUpdated = dtLocalMedication.Clone();
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

                        //Delete
                        var itemToBeDeleted = (from Local in dtLocalMedication.AsEnumerable()
                                               join OpenDental in dtMedication.AsEnumerable()
                                               on Local["Medication_EHR_ID"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                                               equals OpenDental["Medication_EHR_ID"].ToString().Trim() + "_" + OpenDental["Clinic_Number"].ToString().Trim()
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
                            bool status = SynchLocalDAL.Save_Medication_EHR_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

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

        private void fncSynchDataOpenDental_RecallType()
        {
            InitBgWorkerOpenDental_RecallType();
            InitBgTimerOpenDental_RecallType();
        }

        private void InitBgTimerOpenDental_RecallType()
        {
            timerSynchOpenDental_RecallType = new System.Timers.Timer();
            this.timerSynchOpenDental_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchOpenDental_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_RecallType_Tick);
            timerSynchOpenDental_RecallType.Enabled = true;
            timerSynchOpenDental_RecallType.Start();
        }

        private void InitBgWorkerOpenDental_RecallType()
        {
            bwSynchOpenDental_RecallType = new BackgroundWorker();
            bwSynchOpenDental_RecallType.WorkerReportsProgress = true;
            bwSynchOpenDental_RecallType.WorkerSupportsCancellation = true;
            bwSynchOpenDental_RecallType.DoWork += new DoWorkEventHandler(bwSynchOpenDental_RecallType_DoWork);
            bwSynchOpenDental_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_RecallType_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_RecallType.Enabled = false;
                MethodForCallSynchOrderOpenDental_RecallType();
            }
        }

        public void MethodForCallSynchOrderOpenDental_RecallType()
        {
            System.Threading.Thread procThreadmainOpenDental_RecallType = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_RecallType);
            procThreadmainOpenDental_RecallType.Start();
        }

        public void CallSyncOrderTableOpenDental_RecallType()
        {
            if (bwSynchOpenDental_RecallType.IsBusy != true)
            {
                bwSynchOpenDental_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_RecallType.Enabled = true;
        }

        public void SynchDataOpenDental_RecallType()
        {
            try
            {
                if (!Is_synched_RecallType && Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_RecallType = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalRecallType = SynchOpenDentalBAL.GetOpenDentalRecallTypeData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtOpenDentalRecallType.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtOpenDentalRecallType.Rows)
                        {
                            DataRow[] row = dtLocalRecallType.Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "' And  Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
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
                            DataRow[] row = dtOpenDentalRecallType.Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                            if (row.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtOpenDentalRecallType.NewRow();
                                BlcOptDtldr["RecallType_EHR_ID"] = dtDtxRow["RecallType_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtOpenDentalRecallType.Rows.Add(BlcOptDtldr);
                            }
                        }
                        dtOpenDentalRecallType.AcceptChanges();

                        if (dtOpenDentalRecallType != null && dtOpenDentalRecallType.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_RecallType_OpenDental_To_Local(dtOpenDentalRecallType, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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
        private void fncSynchDataOpendental_User()
        {
            InitBgWorkerOpendental_User();
            InitBgTimerOpendental_User();
        }

        private void InitBgTimerOpendental_User()
        {
            timerSynchOpendental_User = new System.Timers.Timer();
            this.timerSynchOpendental_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchOpendental_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpendental_User_Tick);
            timerSynchOpendental_User.Enabled = true;
            timerSynchOpendental_User.Start();
        }

        private void timerSynchOpendental_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpendental_User.Enabled = false;
                MethodForCallSynchOrderOpendental_User();
            }
        }

        private void MethodForCallSynchOrderOpendental_User()
        {
            System.Threading.Thread procThreadmainOpendental_User = new System.Threading.Thread(this.CallSyncOrderTableOpendental_User);
            procThreadmainOpendental_User.Start();
        }

        private void CallSyncOrderTableOpendental_User()
        {
            if (bwSynchOpendental_User.IsBusy != true)
            {
                bwSynchOpendental_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void InitBgWorkerOpendental_User()
        {
            bwSynchOpendental_User = new BackgroundWorker();
            bwSynchOpendental_User.WorkerReportsProgress = true;
            bwSynchOpendental_User.WorkerSupportsCancellation = true;
            bwSynchOpendental_User.DoWork += new DoWorkEventHandler(bwSynchOpendental_User_DoWork);
            bwSynchOpendental_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpendental_User_RunWorkerCompleted);
        }

        private void bwSynchOpendental_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpendental_User.Enabled = true;
        }

        private void bwSynchOpendental_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpendental_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpendental_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SynchDataOpendental_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        Utility.WriteToSyncLogFile_All("user synch start");
                        DataTable dtOpendentalUserTemp = SynchOpenDentalBAL.GetOpenDentalUserData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtOpendentalUserTemp.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalUserTemp = SynchLocalBAL.GetLocalUser(Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString());
                        DataTable dtOpendentalUser = new DataTable();
                        DataTable dtLocalUser = new DataTable();

                        foreach (DataRow drRow in Utility.DtLocationList.Rows)
                        {
                            dtOpendentalUser = dtOpendentalUserTemp.Select("Clinic_Number = " + drRow["Clinic_Number"].ToString()).CopyToDataTable();
                            //dtLocalUser = dtLocalUserTemp.Select("Clinic_Number = " + drRow["Clinic_Number"].ToString()).CopyToDataTable();
                            DataRow[] foundRows = dtLocalUserTemp.Select("Clinic_Number = " + drRow["Clinic_Number"].ToString());
                            if (foundRows.Length > 0)
                            {
                                dtLocalUser = foundRows.CopyToDataTable();
                            }
                            else
                            {
                                dtLocalUser = dtLocalUserTemp.Clone();
                                dtLocalUser.Rows.Clear();
                            }
                            //if (dtLocalUser.Rows.Count == 0)
                            //{
                            //    continue;
                            //}


                            foreach (DataRow dtDtxRow in dtOpendentalUser.Rows)
                            {
                                DataRow[] row = dtLocalUser.Copy().Select("User_EHR_ID = '" + dtDtxRow["User_EHR_ID"] + "'");
                                if (row.Length > 0)
                                {
                                    if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (Convert.ToBoolean(dtDtxRow["Is_Active"]) != Convert.ToBoolean(row[0]["Is_Active"]))
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

                            if (!dtLocalUser.Columns.Contains("InsUptDlt"))
                            {
                                dtLocalUser.Columns.Add("InsUptDlt", typeof(int));
                            }

                            foreach (DataRow item in dtLocalUser.Rows)
                            {
                                DataRow[] row = dtOpendentalUser.Copy().Select("User_EHR_ID = '" + item["User_EHR_ID"] + "'");
                                if (row.Length > 0)
                                {
                                    item["InsUptDlt"] = 0;
                                }
                                else
                                {
                                    if (!Convert.ToBoolean(item["Is_Deleted"]))
                                    {
                                        item["InsUptDlt"] = 3;
                                    }
                                }
                            }
                            if (dtLocalUser.Rows.Count > 0 && dtLocalUser.Select("InsUptDlt = 3").Length > 0)
                            {
                                dtOpendentalUser.Load(dtLocalUser.Select("InsUptDlt = 3").CopyToDataTable().CreateDataReader());
                            }

                            dtOpendentalUser.AcceptChanges();

                            if (dtOpendentalUser != null && dtOpendentalUser.Rows.Count > 0)
                            {
                                // bool status = SynchTrackerBAL.Save_Tracker_To_Local(dtOpendentalUser, "Users", "User_LocalDB_ID,User_Web_ID", "User_EHR_ID");
                                bool status = SynchOpenDentalBAL.Save_User_OpenDental_To_Local(dtOpendentalUser, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Users");
                                    ObjGoalBase.WriteToSyncLogFile("User Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                                    SynchDataLiveDB_Push_User();
                                }
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

        private void fncSynchDataOpenDental_ApptStatus()
        {
            InitBgWorkerOpenDental_ApptStatus();
            InitBgTimerOpenDental_ApptStatus();
        }

        private void InitBgTimerOpenDental_ApptStatus()
        {
            timerSynchOpenDental_ApptStatus = new System.Timers.Timer();
            this.timerSynchOpenDental_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchOpenDental_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_ApptStatus_Tick);
            timerSynchOpenDental_ApptStatus.Enabled = true;
            timerSynchOpenDental_ApptStatus.Start();
        }

        private void InitBgWorkerOpenDental_ApptStatus()
        {
            bwSynchOpenDental_ApptStatus = new BackgroundWorker();
            bwSynchOpenDental_ApptStatus.WorkerReportsProgress = true;
            bwSynchOpenDental_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchOpenDental_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchOpenDental_ApptStatus_DoWork);
            bwSynchOpenDental_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_ApptStatus.Enabled = false;
                MethodForCallSynchOrderOpenDental_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderOpenDental_ApptStatus()
        {
            System.Threading.Thread procThreadmainOpenDental_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_ApptStatus);
            procThreadmainOpenDental_ApptStatus.Start();
        }

        public void CallSyncOrderTableOpenDental_ApptStatus()
        {
            if (bwSynchOpenDental_ApptStatus.IsBusy != true)
            {
                bwSynchOpenDental_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_ApptStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_ApptStatus.Enabled = true;
        }

        public void SynchDataOpenDental_ApptStatus()
        {
            try
            {
                if (!Is_synched_ApptStatus && Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_ApptStatus = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalApptStatus = SynchOpenDentalBAL.GetOpenDentalAppointmentStatus(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        dtOpenDentalApptStatus.Columns.Add("InsUptDlt", typeof(int));
                        for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                        {
                            if (Utility.DtLocationList.Rows[i]["Service_Install_Id"].ToString() == Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString())
                            {
                                dtOpenDentalApptStatus.Rows.Add(1, "Scheduled", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtOpenDentalApptStatus.Rows.Add(2, "Complete", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtOpenDentalApptStatus.Rows.Add(3, "UnschedList", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtOpenDentalApptStatus.Rows.Add(4, "ASAP", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtOpenDentalApptStatus.Rows.Add(5, "Broken", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtOpenDentalApptStatus.Rows.Add(7, "Patient Note", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtOpenDentalApptStatus.Rows.Add(8, "Cmp. Patient Note", "normal", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());
                                dtOpenDentalApptStatus.Rows.Add(11111, "none", "unshedule", Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString());

                                //dtOpenDentalApptStatus.Rows.Add(7, "Appointment Scheduled");
                                //dtOpenDentalApptStatus.Rows.Add(8, "Call Back, Not Ready");
                                //dtOpenDentalApptStatus.Rows.Add(9, "Patient Will Call Us");
                                //dtOpenDentalApptStatus.Rows.Add(10, "Bad Debt. Don't Call");
                                //dtOpenDentalApptStatus.Rows.Add(11, "Wait. See notes");
                                //dtOpenDentalApptStatus.Rows.Add(12, "Left Msg on Ans. Mach");
                                //dtOpenDentalApptStatus.Rows.Add(13, "Left Message with Fam");
                                //dtOpenDentalApptStatus.Rows.Add(14, "Discon Ph Num");
                                //dtOpenDentalApptStatus.Rows.Add(15, "Not Home, Call Again");
                                //dtOpenDentalApptStatus.Rows.Add(16, "Mailed Postcard");
                                //dtOpenDentalApptStatus.Rows.Add(17, "Sent Email");
                                //dtOpenDentalApptStatus.Rows.Add(18, "Not Called");
                                //dtOpenDentalApptStatus.Rows.Add(19, "Unconfirmed");
                                //dtOpenDentalApptStatus.Rows.Add(20, "Appointment Confirmed");
                                //dtOpenDentalApptStatus.Rows.Add(21, "Left Msg");
                                //dtOpenDentalApptStatus.Rows.Add(22, "Arrived");
                                //dtOpenDentalApptStatus.Rows.Add(23, "Ready to go back");
                                //dtOpenDentalApptStatus.Rows.Add(24, "In Treatment Room");
                                //dtOpenDentalApptStatus.Rows.Add(25, "Front Desk");
                                //dtOpenDentalApptStatus.Rows.Add(26, "E-mailed");
                                //dtOpenDentalApptStatus.Rows.Add(27, "Texted");
                            }
                        }
                        dtOpenDentalApptStatus.AcceptChanges();

                        DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtOpenDentalApptStatus.Rows)
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

                        dtOpenDentalApptStatus.AcceptChanges();

                        if (dtOpenDentalApptStatus != null && dtOpenDentalApptStatus.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_ApptStatus_OpenDental_To_Local(dtOpenDentalApptStatus, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        private void fncSynchDataOpenDental_Holiday()
        {
            InitBgWorkerOpenDental_Holiday();
            InitBgTimerOpenDental_Holiday();
        }

        private void InitBgTimerOpenDental_Holiday()
        {
            timerSynchOpenDental_Holiday = new System.Timers.Timer();
            this.timerSynchOpenDental_Holiday.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchOpenDental_Holiday.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_Holiday_Tick);
            timerSynchOpenDental_Holiday.Enabled = true;
            timerSynchOpenDental_Holiday.Start();
        }

        private void InitBgWorkerOpenDental_Holiday()
        {
            bwSynchOpenDental_Holiday = new BackgroundWorker();
            bwSynchOpenDental_Holiday.WorkerReportsProgress = true;
            bwSynchOpenDental_Holiday.WorkerSupportsCancellation = true;
            bwSynchOpenDental_Holiday.DoWork += new DoWorkEventHandler(bwSynchOpenDental_Holiday_DoWork);
            bwSynchOpenDental_Holiday.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_Holiday_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_Holiday_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_Holiday.Enabled = false;
                MethodForCallSynchOrderOpenDental_Holiday();
            }
        }

        public void MethodForCallSynchOrderOpenDental_Holiday()
        {
            System.Threading.Thread procThreadmainOpenDental_Holiday = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_Holiday);
            procThreadmainOpenDental_Holiday.Start();
        }

        public void CallSyncOrderTableOpenDental_Holiday()
        {
            if (bwSynchOpenDental_Holiday.IsBusy != true)
            {
                bwSynchOpenDental_Holiday.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_Holiday_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_Holiday.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_Holiday();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_Holiday_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_Holiday.Enabled = true;
        }

        public void SynchDataOpenDental_Holiday()
        {
            try
            {
                if (!Is_synched_Holidays && Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_Holidays = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalHoliday = SynchOpenDentalBAL.GetOpenDentalHolidayData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtOpenDentalHoliday.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalHoliday = SynchLocalBAL.GetLocalHolidayData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        dtOpenDentalHoliday = CommonUtility.AddHolidays(dtOpenDentalHoliday, dtLocalHoliday, "SchedDate", "Note", "ScheduleNum");

                        foreach (DataRow dtDtxRow in dtOpenDentalHoliday.Rows)
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
                                    //dtDtxRow["DateTStamp"] = Utility.GetCurrentDatetimestring();
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
                            DataRow[] row = dtOpenDentalHoliday.Copy().Select("ScheduleNum = '" + dtDtxRow["H_EHR_ID"] + "'");
                            if (row.Length <= 0)
                            {
                                DataRow ApptDtldr = dtOpenDentalHoliday.NewRow();
                                ApptDtldr["ScheduleNum"] = dtDtxRow["H_EHR_ID"].ToString().Trim();
                                ApptDtldr["InsUptDlt"] = 3;
                                dtOpenDentalHoliday.Rows.Add(ApptDtldr);
                            }
                        }

                        dtOpenDentalHoliday.AcceptChanges();

                        if (dtOpenDentalHoliday != null && dtOpenDentalHoliday.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_Holiday_OpenDental_To_Local(dtOpenDentalHoliday, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        private void fncSynchDataLocalToOpenDental_Appointment()
        {
            InitBgWorkerLocalToOpenDental_Appointment();
            InitBgTimerLocalToOpenDental_Appointment();
        }

        private void InitBgTimerLocalToOpenDental_Appointment()
        {
            timerSynchLocalToOpenDental_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToOpenDental_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToOpenDental_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToOpenDental_Appointment_Tick);
            timerSynchLocalToOpenDental_Appointment.Enabled = true;
            timerSynchLocalToOpenDental_Appointment.Start();
            timerSynchLocalToOpenDental_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToOpenDental_Appointment()
        {
            bwSynchLocalToOpenDental_Appointment = new BackgroundWorker();
            bwSynchLocalToOpenDental_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToOpenDental_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToOpenDental_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToOpenDental_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToOpenDental_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToOpenDental_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToOpenDental_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToOpenDental_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToOpenDental_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToOpenDental_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToOpenDental_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToOpenDental_Appointment);
            procThreadmainLocalToOpenDental_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToOpenDental_Appointment()
        {
            if (bwSynchLocalToOpenDental_Appointment.IsBusy != true)
            {
                bwSynchLocalToOpenDental_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToOpenDental_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToOpenDental_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToOpenDental_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToOpenDental_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToOpenDental_Appointment.Enabled = true;
        }

        public void SynchDataLocalToOpenDental_Appointment()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        DataTable dtOpenDentalPatient = SynchOpenDentalBAL.GetOpenDentalPatientID_NameData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        DataTable dtIdelProv = SynchOpenDentalBAL.GetOpenDentalIdelProvider(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

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

                            DataTable dtBookOperatoryApptWiseDateTime = SynchOpenDentalBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), dtDtxRow["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
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
                                    DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Select("OP = '" + tmpCheckOpId + "'");
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

                                    tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtOpenDentalPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));


                                    //DataRow[] row = dtOpenDentalPatient.Copy().Select("TRIM(Mobile) = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR TRIM(Home_Phone) = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR TRIM(Work_Phone) =  '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' ");
                                    //if (row.Length > 0)
                                    //{
                                    //    for (int i = 0; i < row.Length; i++)
                                    //    {
                                    //        if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                    //        {
                                    //            tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //        }
                                    //        else if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebRevPatientName.ToString().ToLower().Trim())
                                    //        {
                                    //            tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //        }
                                    //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                    //        {
                                    //            tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //        }
                                    //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == dtDtxRow["First_Name"].ToString().Trim().ToLower())
                                    //        {
                                    //            tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //        }
                                    //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == (TmpWebPatientName.ToString().IndexOf(" ") > 0 ? TmpWebPatientName.Substring(0, TmpWebPatientName.ToString().IndexOf(" ")).ToLower() : TmpWebPatientName))
                                    //        {
                                    //            tmpPatient_id = Convert.ToInt64(row[i]["Patient_EHR_ID"].ToString());
                                    //        }
                                    //        if (tmpPatient_id != 0)
                                    //        {
                                    //            break;
                                    //        }
                                    //    }

                                    //    tmpPatient_Gur_id = Convert.ToInt64(row[0]["Guarantor"].ToString());
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

                                //    tmpPatient_id = SynchOpenDentalBAL.Save_Patient_Local_To_OpenDental(tmpLastName.Trim(),
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

                                    //DataTable dtBookOperatoryApptWiseDateTime = SynchOpenDentalBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
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

                                    tmpAppt_EHR_id = SynchOpenDentalBAL.Save_Appointment_Local_To_OpenDental(tmpPatient_id.ToString(),
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
                                        bool isApptId_Update = SynchOpenDentalBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        private void fncSynchDataOpenDental_MedicalHistory()
        {
            InitBgWorkerOpenDental_MedicalHistory();
            InitBgTimerOpenDental_MedicalHistory();
        }

        private void InitBgTimerOpenDental_MedicalHistory()
        {
            timerSynchOpenDental_MedicalHistory = new System.Timers.Timer();
            this.timerSynchOpenDental_MedicalHistory.Interval = 1000 * GoalBase.intervalEHRSynch_MedicalHistory;
            this.timerSynchOpenDental_MedicalHistory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_MedicalHistory_Tick);
            timerSynchOpenDental_MedicalHistory.Enabled = true;
            timerSynchOpenDental_MedicalHistory.Start();
            timerSynchOpenDental_MedicalHistory_Tick(null, null);
        }

        private void InitBgWorkerOpenDental_MedicalHistory()
        {
            bwSynchOpenDental_MedicalHistory = new BackgroundWorker();
            bwSynchOpenDental_MedicalHistory.WorkerReportsProgress = true;
            bwSynchOpenDental_MedicalHistory.WorkerSupportsCancellation = true;
            bwSynchOpenDental_MedicalHistory.DoWork += new DoWorkEventHandler(bwSynchOpenDental_MedicalHistory_DoWork);
            bwSynchOpenDental_MedicalHistory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_MedicalHistory_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_MedicalHistory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_MedicalHistory.Enabled = false;
                MethodForCallSynchOrderOpenDental_MedicalHistory();
            }
        }

        public void MethodForCallSynchOrderOpenDental_MedicalHistory()
        {
            System.Threading.Thread procThreadmainOpenDental_MedicalHistory = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_MedicalHistory);
            procThreadmainOpenDental_MedicalHistory.Start();
        }

        public void CallSyncOrderTableOpenDental_MedicalHistory()
        {
            if (bwSynchOpenDental_MedicalHistory.IsBusy != true)
            {
                bwSynchOpenDental_MedicalHistory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_MedicalHistory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_MedicalHistory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_MedicalHistory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_MedicalHistory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_MedicalHistory.Enabled = true;
        }

        public void SynchDataOpenDental_MedicalHistory()
        {
            CallSynch_OpenDental_MedicalHistory();

        }

        private void CallSynch_OpenDental_MedicalHistory()
        {
            if (!Is_synched_MedicalHistory && Utility.IsApplicationIdleTimeOff)
            {
                string tablename = "";
                try
                {
                    Is_synched_MedicalHistory = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataSet dsOpenDentalMedicalHistory = SynchOpenDentalBAL.GetOpenDentalMedicalHistoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        DataTable dtMedicalHistory = new DataTable();
                        string ignoreColumnsList = "", primaryColumnList = "";

                        foreach (DataTable dtTable in dsOpenDentalMedicalHistory.Tables)
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
                                    SynchDataLiveDB_Push_MedicalHisotryTables(dtMedicalHistory.TableName.ToString());
                                }
                                else
                                {
                                    IsTrackerOperatorySync = false;
                                }
                            }
                            #endregion
                        }
                        SynchDataLiveDB_Push_MedicalHisotryTables("OD_SheetDef");
                        SynchDataLiveDB_Push_MedicalHisotryTables("OD_SheetFieldDef");

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

        //private void fncSynchDataOpenDental_PatientWiseRecallDate()
        //{
        //    InitBgWorkerOpenDental_PatientWiseRecallDate();
        //    InitBgTimerOpenDental_PatientWiseRecallDate();
        //}

        //private void InitBgTimerOpenDental_PatientWiseRecallDate()
        //{
        //    timerSynchOpenDental_PatientWiseRecallDate = new System.Timers.Timer();
        //    this.timerSynchOpenDental_PatientWiseRecallDate.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
        //    this.timerSynchOpenDental_PatientWiseRecallDate.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_PatientWiseRecallDate_Tick);
        //    timerSynchOpenDental_PatientWiseRecallDate.Enabled = true;
        //    timerSynchOpenDental_PatientWiseRecallDate.Start();
        //    timerSynchOpenDental_PatientWiseRecallDate_Tick(null, null);
        //}

        //private void InitBgWorkerOpenDental_PatientWiseRecallDate()
        //{
        //    bwSynchOpenDental_PatientWiseRecallDate = new BackgroundWorker();
        //    bwSynchOpenDental_PatientWiseRecallDate.WorkerReportsProgress = true;
        //    bwSynchOpenDental_PatientWiseRecallDate.WorkerSupportsCancellation = true;
        //    bwSynchOpenDental_PatientWiseRecallDate.DoWork += new DoWorkEventHandler(bwSynchOpenDental_PatientWiseRecallDate_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchOpenDental_PatientWiseRecallDate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_PatientWiseRecallDate_RunWorkerCompleted);
        //}

        //private void timerSynchOpenDental_PatientWiseRecallDate_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {

        //        timerSynchOpenDental_PatientWiseRecallDate.Enabled = false;
        //        MethodForCallSynchOrderOpenDental_PatientWiseRecallDate();
        //    }
        //}

        //public void MethodForCallSynchOrderOpenDental_PatientWiseRecallDate()
        //{
        //    System.Threading.Thread procThreadmainOpenDental_PatientWiseRecallDate = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_PatientWiseRecallDate);
        //    procThreadmainOpenDental_PatientWiseRecallDate.Start();
        //}

        //public void CallSyncOrderTableOpenDental_PatientWiseRecallDate()
        //{
        //    if (bwSynchOpenDental_PatientWiseRecallDate.IsBusy != true)
        //    {
        //        bwSynchOpenDental_PatientWiseRecallDate.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchOpenDental_PatientWiseRecallDate_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchOpenDental_PatientWiseRecallDate.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }

        //        SynchDataOpenDental_PatientWiseRecallDate(); 
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchOpenDental_PatientWiseRecallDate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchOpenDental_PatientWiseRecallDate.Enabled = true;
        //}

        //public void SynchDataOpenDental_PatientWiseRecallDate()
        //{

        //    try
        //    {

        //        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //        {
        //            DataTable dtOpenDentalPatientWiseRecallDate = SynchOpenDentalBAL.GetOpenDentalPatientWiseRecallDate(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //            DataTable dtLocalRecallType = SynchLocalBAL.GetLocalPatientWiseRecallTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

        //            foreach (DataRow dtDtxRow in dtOpenDentalPatientWiseRecallDate.Rows)
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


        //            dtOpenDentalPatientWiseRecallDate.AcceptChanges();

        //            if (dtOpenDentalPatientWiseRecallDate != null && dtOpenDentalPatientWiseRecallDate.Rows.Count > 0)
        //            {
        //                bool status = SynchEaglesoftBAL.SavePatientWiseRecallType_Eaglesoft_To_Local(dtOpenDentalPatientWiseRecallDate, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        private void fncSynchDataOpenDental_Insurance()
        {
            InitBgWorkerOpenDental_Insurance();
            InitBgTimerOpenDental_Insurance();
        }

        private void InitBgTimerOpenDental_Insurance()
        {
            timerSynchOpenDental_Insurance = new System.Timers.Timer();
            this.timerSynchOpenDental_Insurance.Interval = 1000 * GoalBase.intervalEHRSynch_Insurance;
            this.timerSynchOpenDental_Insurance.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchOpenDental_Insurance_Tick);
            timerSynchOpenDental_Insurance.Enabled = true;
            timerSynchOpenDental_Insurance.Start();
        }

        private void InitBgWorkerOpenDental_Insurance()
        {
            bwSynchOpenDental_Insurance = new BackgroundWorker();
            bwSynchOpenDental_Insurance.WorkerReportsProgress = true;
            bwSynchOpenDental_Insurance.WorkerSupportsCancellation = true;
            bwSynchOpenDental_Insurance.DoWork += new DoWorkEventHandler(bwSynchOpenDental_Insurance_DoWork);
            bwSynchOpenDental_Insurance.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchOpenDental_Insurance_RunWorkerCompleted);
        }

        private void timerSynchOpenDental_Insurance_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchOpenDental_Insurance.Enabled = false;
                MethodForCallSynchOrderOpenDental_Insurance();
            }
        }

        public void MethodForCallSynchOrderOpenDental_Insurance()
        {
            System.Threading.Thread procThreadmainOpenDental_Insurance = new System.Threading.Thread(this.CallSyncOrderTableOpenDental_Insurance);
            procThreadmainOpenDental_Insurance.Start();
        }

        public void CallSyncOrderTableOpenDental_Insurance()
        {
            if (bwSynchOpenDental_Insurance.IsBusy != true)
            {
                bwSynchOpenDental_Insurance.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchOpenDental_Insurance_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchOpenDental_Insurance.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataOpenDental_Insurance();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchOpenDental_Insurance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchOpenDental_Insurance.Enabled = true;
        }

        public void SynchDataOpenDental_Insurance()
        {
            try
            {
                if (!Is_synched_Insurance && Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_Insurance = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalInsurance = SynchOpenDentalBAL.GetOpenDentalInsuranceData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        dtOpenDentalInsurance.Columns.Add("InsUptDlt", typeof(int));

                        DataTable dtLocalInsurance = SynchLocalBAL.GetLocalInsuranceData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtOpenDentalInsurance.Rows)
                        {
                            DataRow[] row = dtLocalInsurance.Copy().Select("Insurance_EHR_ID = '" + dtDtxRow["CarrierNum"] + "' ");
                            if (row.Length > 0)
                            {
                                if (dtDtxRow["CarrierName"].ToString().Trim() != row[0]["Insurance_Name"].ToString().Trim())
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
                            DataRow[] row = dtOpenDentalInsurance.Copy().Select("CarrierNum = '" + dtDtxRow["Insurance_EHR_ID"] + "' ");
                            if (row.Length > 0)
                            { }
                            else
                            {
                                DataRow BlcOptDtldr = dtOpenDentalInsurance.NewRow();
                                BlcOptDtldr["CarrierNum"] = dtDtxRow["Insurance_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["CarrierName"] = dtDtxRow["Insurance_Name"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtOpenDentalInsurance.Rows.Add(BlcOptDtldr);
                            }
                        }

                        dtOpenDentalInsurance.AcceptChanges();

                        if (dtOpenDentalInsurance != null && dtOpenDentalInsurance.Rows.Count > 0)
                        {
                            bool status = SynchOpenDentalBAL.Save_Insurance_OpenDental_To_Local(dtOpenDentalInsurance, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

        #region EventListener
        public static bool SynchDataOpenDental_AppointmentFromEvent(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id, bool isTrueDebugLog = false)
        {
            string strDbConnString = "";
            string Location_ID = "";
            try
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                var dbConStr = Utility.DtInstallServiceList.Select("Installation_Id = '" + Service_Install_Id + "'");
                strDbConnString = dbConStr[0]["DBConnString"].ToString();

                var LocID = Utility.DtLocationList.Select("Clinic_Number = '" + Clinic_Number + "'");
                Location_ID = LocID[0]["Location_ID"].ToString();

                Utility.WritetoAditEventDebugLogFile_Static("1 SynchDataOpenDental_AppointmentFromEvent Start.");
                //CheckEntryUserLoginIdExist();
                Utility.WritetoAditEventDebugLogFile_Static("2");
                if (Utility.IsApplicationIdleTimeOff)
                {
                    Utility.WritetoAditEventDebugLogFile_Static("3");
                    //for (int l = 0; l < Utility.DtInstallServiceList.Rows.Count; j++)
                    //{
                    Utility.WritetoAditEventDebugLogFile_Static("4");
                    //DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                    DataTable dtOpenDentalPatient = SynchOpenDentalBAL.GetOpenDentalPatientID_NameData(strDbConnString);
                    Utility.WritetoAditEventDebugLogFile_Static("5");
                    DataTable dtIdelProv = SynchOpenDentalBAL.GetOpenDentalIdelProvider(strDbConnString);
                    Utility.WritetoAditEventDebugLogFile_Static("6");

                    string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                    Utility.WritetoAditEventDebugLogFile_Static("7");
                    string tmpApptProv = "";
                    Utility.WritetoAditEventDebugLogFile_Static("8");
                    Int64 tmpPatient_id = 0;
                    Utility.WritetoAditEventDebugLogFile_Static("9");
                    Int64 tmpPatient_Gur_id = 0;
                    Utility.WritetoAditEventDebugLogFile_Static("10");

                    Int64 tmpAppt_EHR_id = 0;
                    Utility.WritetoAditEventDebugLogFile_Static("11");
                    Int64 tmpNewPatient = 1;
                    Utility.WritetoAditEventDebugLogFile_Static("12");
                    string tmpLastName = "";
                    Utility.WritetoAditEventDebugLogFile_Static("13");
                    string tmpFirstName = "";
                    Utility.WritetoAditEventDebugLogFile_Static("14");
                    string TmpWebPatientName = "";
                    Utility.WritetoAditEventDebugLogFile_Static("15");
                    string TmpWebRevPatientName = "";
                    Utility.WritetoAditEventDebugLogFile_Static("16");
                    if (dtWebAppointment != null)
                    {
                        if (dtWebAppointment.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                        }
                    }
                    foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                    {
                        Utility.WritetoAditEventDebugLogFile_Static("17");
                        string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
                        Utility.WritetoAditEventDebugLogFile_Static("18");
                        DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                        Utility.WritetoAditEventDebugLogFile_Static("19");
                        DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                        Utility.WritetoAditEventDebugLogFile_Static("20");
                        TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                        Utility.WritetoAditEventDebugLogFile_Static("21");
                        int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);
                        Utility.WritetoAditEventDebugLogFile_Static("22");
                        string tmpApptDurPatern = "";
                        Utility.WritetoAditEventDebugLogFile_Static("23");
                        for (int i = 0; i < tmpApptDurMinutes / 5; i++)
                        {
                            tmpApptDurPatern = tmpApptDurPatern + "X";
                        }
                        Utility.WritetoAditEventDebugLogFile_Static("24");
                        DataTable dtBookOperatoryApptWiseDateTime = SynchOpenDentalBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), dtDtxRow["Clinic_Number"].ToString(), strDbConnString);
                        Utility.WritetoAditEventDebugLogFile_Static("25");
                        int tmpIdealOperatory = 0;
                        Utility.WritetoAditEventDebugLogFile_Static("26");
                        string appointment_EHR_id = "";
                        Utility.WritetoAditEventDebugLogFile_Static("27");

                        if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
                        {
                            Utility.WritetoAditEventDebugLogFile_Static("28");
                            int tmpCheckOpId = 0;
                            Utility.WritetoAditEventDebugLogFile_Static("29");
                            bool IsConflict = false;
                            Utility.WritetoAditEventDebugLogFile_Static("30");
                            for (int i = 0; i < Operatory_EHR_IDs.Length; i++)
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("30.1");
                                IsConflict = false;
                                Utility.WritetoAditEventDebugLogFile_Static("30.2");
                                tmpCheckOpId = Convert.ToInt32(Operatory_EHR_IDs[i].ToString());
                                Utility.WritetoAditEventDebugLogFile_Static("30.3");
                                DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Select("OP = '" + tmpCheckOpId + "'");
                                Utility.WritetoAditEventDebugLogFile_Static("30.4");
                                if (rowBookOpTime.Length > 0)
                                {
                                    Utility.WritetoAditEventDebugLogFile_Static("30.5");
                                    for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                    {
                                        Utility.WritetoAditEventDebugLogFile_Static("30.5.1");
                                        appointment_EHR_id = rowBookOpTime[Bop]["AptNum"].ToString();
                                        Utility.WritetoAditEventDebugLogFile_Static("30.5.2");
                                        if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
                                            && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                        {
                                            Utility.WritetoAditEventDebugLogFile_Static("30.5.3");
                                            IsConflict = true;
                                            break;
                                        }
                                        else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()))
                                            && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["AptDateTime"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                        {
                                            Utility.WritetoAditEventDebugLogFile_Static("30.5.4");
                                            IsConflict = true;
                                            break;
                                        }
                                    }
                                }
                                if (IsConflict == false)
                                {
                                    Utility.WritetoAditEventDebugLogFile_Static("30.5.5");
                                    tmpIdealOperatory = tmpCheckOpId;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Utility.WritetoAditEventDebugLogFile_Static("27");
                            tmpIdealOperatory = Convert.ToInt32(Operatory_EHR_IDs[0].ToString());
                        }

                        if (tmpIdealOperatory == 0)
                        {
                            Utility.WritetoAditEventDebugLogFile_Static("28, appointment_EHR_id:" + appointment_EHR_id);
                            DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("AptNum = " + appointment_EHR_id).CopyToDataTable();
                            Utility.WritetoAditEventDebugLogFile_Static("29");
                            bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id, dtTemp, appointment_EHR_id, Location_ID);
                            Utility.WritetoAditEventDebugLogFile_Static("30");
                        }
                        else
                        {
                            tmpPatient_id = 0;
                            tmpPatient_Gur_id = 0;
                            tmpAppt_EHR_id = 0;
                            tmpNewPatient = 1;
                            TmpWebPatientName = "";

                            Utility.WritetoAditEventDebugLogFile_Static("31");
                            //TmpWebPatientName = dtDtxRow["First_Name"].ToString().Trim();
                            //TmpWebRevPatientName = dtDtxRow["Last_Name"].ToString().Trim();
                            Utility.WritetoAditEventDebugLogFile_Static("32");
                            if (dtDtxRow["Patient_Status"].ToString().ToLower().Trim() == "new")
                            {
                                tmpNewPatient = 1;
                            }
                            else
                            {
                                tmpNewPatient = 0;
                            }
                            Utility.WritetoAditEventDebugLogFile_Static("33");
                            Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebRevPatientName);
                            Utility.WritetoAditEventDebugLogFile_Static("34");
                            tmpApptProv = dtDtxRow["Provider_EHR_ID"].ToString().Trim();
                            Utility.WritetoAditEventDebugLogFile_Static("35");
                            if (tmpApptProv == "" || tmpApptProv == "0" || tmpApptProv == "-")
                            {
                                tmpApptProv = tmpIdelProv;
                            }
                            Utility.WritetoAditEventDebugLogFile_Static("36");
                            tmpPatient_id = 0;
                            if (dtDtxRow["Patient_EHR_Id"] != null && dtDtxRow["Patient_EHR_Id"].ToString() != string.Empty && dtDtxRow["Patient_EHR_Id"].ToString() != "-")
                            {
                                tmpPatient_id = Convert.ToInt64(dtDtxRow["Patient_EHR_Id"].ToString());
                            }
                            Utility.WritetoAditEventDebugLogFile_Static("37");
                            if (tmpPatient_id == 0)
                            {

                                tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtOpenDentalPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), strDbConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                            }
                            Utility.WritetoAditEventDebugLogFile_Static("38 tmpPatient_id:" + tmpPatient_id);
                            if (tmpPatient_id > 0)
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("39 Save_Appointment_Local_To_OpenDental start.");
                                tmpAppt_EHR_id = SynchOpenDentalBAL.Save_Appointment_Local_To_OpenDental(tmpPatient_id.ToString(),
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
                                Utility.WritetoAditEventDebugLogFile_Static("40 AptID:" + tmpAppt_EHR_id);

                                if (tmpAppt_EHR_id > 0)
                                {
                                    bool isApptId_Update = SynchOpenDentalBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id);
                                }
                                SyncDataOpenDental_AppointmentFromEvent(strDbConnString, Clinic_Number, Service_Install_Id, tmpAppt_EHR_id.ToString(), tmpPatient_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                                Utility.WritetoAditEventDebugLogFile_Static("40 SynchDataOpenDental_AppointmentFromEvent End.");
                            }
                        }
                    }
                    //}
                    //SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook();                  
                    Utility.WritetoAditEventSyncLogFile_Static("SynchDataOpenDental_AppointmentFromEvent Sync (Local Database To OpenDental) Successfully.");
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataOpenDental_AppointmentFromEvent Sync (Local Database To OpenDental) ]" + ex.Message);
                return false;
            }
        }

        public static void SyncDataOpenDental_AppointmentFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        {
            try
            {
                Utility.WritetoAditEventDebugLogFile_Static("1 SyncDataOpenDental_AppointmentFromEvent Start.");
                try
                {
                    Utility.WritetoAditEventDebugLogFile_Static("2 Appointment Patient Start.");
                    #region "Appointment Patient"
                    DataTable dtLocalOpenDentalLanguageList = SynchLocalBAL.GetLocalOpenDentalLanguageList();
                    Utility.WritetoAditEventDebugLogFile_Static("2.1 GetLocalPatientDataByPatID start.");
                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Service_Install_Id, strPatID);
                    Utility.WritetoAditEventDebugLogFile_Static("2.2 GetLocalPatientDataByPatID End and GetOpenDentalPatientDataByPatID Start.");
                    DataTable dtOpenDentalAppointmensPatient = SynchOpenDentalBAL.GetOpenDentalPatientData(Clinic_Number, strDbString, true, strPatID);
                    Utility.WritetoAditEventDebugLogFile_Static("2.3 GetOpenDentalPatientDataByPatID End.");
                    var updateLanguageQuery = from r1 in dtOpenDentalAppointmensPatient.AsEnumerable()
                                              join r2 in dtLocalOpenDentalLanguageList.AsEnumerable()
                                              on r1.Field<string>("PreferredLanguage") equals r2.Field<string>("Language_Short_Name")
                                              select new { r1, r2 };
                    foreach (var x in updateLanguageQuery)
                    {
                        x.r1.SetField("PreferredLanguage", x.r2.Field<string>("Language_Name"));
                    }

                    DataTable dtSaveRecords = new DataTable();
                    dtSaveRecords = dtLocalPatient.Clone();

                    var itemsToBeAdded = (from OpenDentalPatient in dtOpenDentalAppointmensPatient.AsEnumerable()
                                          join LocalPatient in dtLocalPatient.AsEnumerable()
                                          on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                          equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                          //on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                          //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
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
                        //ObjGoalBase.WriteToSyncLogFile_All("Table to Save Record count after insert :" + dtSaveRecords.Rows.Count);
                    }

                    Utility.WritetoAditEventDebugLogFile_Static("2.4 Comparison start.");
                    var itemsToBeUpdated = (from OpenDentalPatient in dtOpenDentalAppointmensPatient.AsEnumerable()
                                            join LocalPatient in dtLocalPatient.AsEnumerable()
                                            on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                            equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                            where
                                             Convert.ToDateTime(OpenDentalPatient["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(LocalPatient["EHR_Entry_DateTime"])
                                             ||
                                             (OpenDentalPatient["nextvisit_date"] != DBNull.Value && OpenDentalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(OpenDentalPatient["nextvisit_date"]) : DateTime.Now)
                                             !=
                                             (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)
                                             ||
                                             (OpenDentalPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())
                                             ||
                                             (OpenDentalPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())
                                             || (OpenDentalPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                             || (OpenDentalPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                             || (OpenDentalPatient["Home_Phone"].ToString().Trim()) != (LocalPatient["Home_Phone"].ToString().Trim())
                                             || (OpenDentalPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                             || (OpenDentalPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                             || (OpenDentalPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                             || (OpenDentalPatient["Mobile"].ToString().Trim()) != (LocalPatient["Mobile"].ToString().Trim())
                                             || (OpenDentalPatient["ReceiveSMS"].ToString().Trim()) != (LocalPatient["ReceiveSMS"].ToString().Trim())
                                             || (OpenDentalPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
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
                    Utility.WritetoAditEventDebugLogFile_Static("2.6 Comparison End, records to be udpate:" + dtSaveRecords.Rows.Count);
                    if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                    {
                        bool status = SynchOpenDentalBAL.Save_Patient_OpenDental_To_Local_New(dtSaveRecords, Clinic_Number, Service_Install_Id);
                        Utility.WritetoAditEventDebugLogFile_Static("2.6.1 Patient Push Start.");
                        SynchDataLiveDB_Push_Patient(strPatID);
                        Utility.WritetoAditEventDebugLogFile_Static("2.6.2 Patient Push End.");
                    }
                    Utility.WritetoAditEventDebugLogFile_Static("2.7 Appointment Patient Sync Successfully.");

                    #endregion
                    Utility.WritetoAditEventDebugLogFile_Static("3 Appointment Patient End and PatientStatus Start.");
                    #region "Patient Status"
                    DataTable dtOpenDentalPatientStatus = new DataTable();
                    Utility.WritetoAditEventDebugLogFile_Static("3.1 GetOpenDentalPatientStatusDataByPatID Start.");
                    dtOpenDentalPatientStatus = SynchOpenDentalBAL.GetOpenDentalPatientStatusData(Clinic_Number, strDbString, strPatID);
                    Utility.WritetoAditEventDebugLogFile_Static("3.2 GetOpenDentalPatientStatusDataByPatID End. Records:" + dtOpenDentalPatientStatus.Rows.Count);
                    if (dtOpenDentalPatientStatus != null && dtOpenDentalPatientStatus.Rows.Count > 0)
                    {
                        Utility.WritetoAditEventDebugLogFile_Static("3.3 UpdatePatient_StatusByPatID Start.");
                        SynchLocalBAL.UpdatePatient_Status(dtOpenDentalPatientStatus, Service_Install_Id, Clinic_Number, strPatID);
                        Utility.WritetoAditEventDebugLogFile_Static("3.4 UpdatePatient_StatusByPatID End.");
                    }
                    #endregion
                    Utility.WritetoAditEventDebugLogFile_Static("4 PatientStatus End.");
                }
                catch (Exception exPat)
                {
                    Utility.WritetoAditEventErrorLogFile_Static("[SyncDataOpenDental_AppointmentFromEvent Error in Appointment Patient Sync]:" + exPat.Message);
                }

                Utility.WritetoAditEventDebugLogFile_Static("5 GetOpenDentalAppointmentData Start.");
                DataTable dtOpenDentalAppointment = SynchOpenDentalBAL.GetOpenDentalAppointmentData(strDbString, strApptID);
                dtOpenDentalAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                dtOpenDentalAppointment.Columns.Add("InsUptDlt", typeof(int));
                Utility.WritetoAditEventDebugLogFile_Static("6 GetOpenDentalAppointmentData End And GetLocalAppointmentData Start.");
                DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Service_Install_Id, strApptID);
                Utility.WritetoAditEventDebugLogFile_Static("7 GetLocalAppointmentData End Comparison Start.");

                foreach (DataRow dtDtxRow in dtOpenDentalAppointment.Rows)
                {
                    try
                    {
                        Utility.WritetoAditEventDebugLogFile_Static("8 Appt_EHR_ID: " + dtDtxRow["Appt_EHR_ID"].ToString().Trim());
                        DataRow[] row = dtLocalAppointment.Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            if (Convert.ToDateTime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EHR_Entry_DateTime"]))
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.1 Change in EHR_Entry_DateTime");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Last_Name"].ToString().ToLower().Trim() != row[0]["Last_Name"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.2 Change in Last_Name");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.2 Change in Last_Name");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["MI"].ToString().ToLower().Trim() != row[0]["MI"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.2 Change in Last_Name");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Utility.ConvertContactNumber(dtDtxRow["Home_Contact"].ToString().Replace("(", "").Replace(")", "").Replace("-", "").ToLower().Trim()) != Utility.ConvertContactNumber(row[0]["Home_Contact"].ToString().ToLower().Trim()))
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.3 Change in Home_Contact");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().ToLower().Trim()) != Utility.ConvertContactNumber(row[0]["Mobile_Contact"].ToString().ToLower().Trim()))
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.4 Change in Mobile_Contact");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Email"].ToString().ToLower().Trim() != row[0]["Email"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.5 Change in Email");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Address"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.6 Change in Address");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["City"].ToString().ToLower().Trim() != row[0]["City"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.7 Change in City");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ST"].ToString().ToLower().Trim() != row[0]["ST"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.8 Change in ST");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["is_asap"] != null && dtDtxRow["is_asap"].ToString() != string.Empty && Convert.ToBoolean(dtDtxRow["is_asap"]) != Convert.ToBoolean(row[0]["is_asap"]))
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.9 Change in is_asap");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Zip"].ToString().ToLower().Trim() != row[0]["Zip"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.10 Change in Zip");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ProcedureDesc"].ToString().ToLower().Trim() != row[0]["ProcedureDesc"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.11 Change in ProcedureDesc");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ProcedureCode"].ToString().ToLower().Trim() != row[0]["ProcedureCode"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.12 Change in ProcedureCode");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Clinic_Number"].ToString().ToLower().Trim() != row[0]["Clinic_Number"].ToString().ToLower().Trim())
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.13 Change in Clinic_Number");
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else
                            {
                                Utility.WritetoAditEventDebugLogFile_Static("8.14 No Change");
                                dtDtxRow["InsUptDlt"] = 0;
                            }
                        }
                        else
                        {
                            dtDtxRow["InsUptDlt"] = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.WritetoAditEventErrorLogFile_Static("[SyncDataOpenDental_AppointmentFromEvent (" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + ") Sync (OpenDental to Local Database) ]" + ex.Message);
                    }
                    Utility.WritetoAditEventDebugLogFile_Static("9 Comparison End");

                    dtOpenDentalAppointment.AcceptChanges();
                    if (dtOpenDentalAppointment != null && dtOpenDentalAppointment.Rows.Count > 0 && dtOpenDentalAppointment.Select("InsUptDlt IN (1,5,4,7,3,6)").Count() > 0)
                    {
                        DataTable dtResult = dtOpenDentalAppointment.Select("InsUptDlt IN (1,5,4,7,3,6)").CopyToDataTable();
                        Utility.WritetoAditEventDebugLogFile_Static("10 Appointment sent for Save and update : " + System.DateTime.Now.ToString() + " with Count " + dtResult.Rows.Count.ToString());

                        if (!dtResult.Columns.Contains("Appt_Web_ID"))
                        {
                            dtResult.Columns.Add("Appt_Web_ID");
                        }
                        dtResult.Rows[0]["Appt_Web_ID"] = strWebID;


                        bool status = SynchOpenDentalBAL.Save_Appointment_OpenDental_To_Local(dtResult, Service_Install_Id);
                        Utility.WritetoAditEventDebugLogFile_Static("11 Appointment updated : " + System.DateTime.Now.ToString());
                    }

                    Utility.WritetoAditEventDebugLogFile_Static("12 Appointment Push Start.");
                    bool temp = Utility.IsExternalAppointmentSync;
                    Utility.IsExternalAppointmentSync = true;
                    SynchDataLiveDB_Push_Appointment(strApptID);
                    Utility.IsExternalAppointmentSync = temp;
                    Utility.WritetoAditEventDebugLogFile_Static("13 Appointment Push End, SyncDataOpenDental_AppointmentFromEvent End.");
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncDataOpenDental_AppointmentFromEvent Sync (OpenDental to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataLocalToOpenDental_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
        {
            string strDbConnString = "";
            string Location_ID = "";
            string strDocumentPath = "";
            try
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

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
                            DataTable dtIdelProv = SynchOpenDentalBAL.GetOpenDentalIdelProvider(strDbConnString);
                            string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                            ObjGoalBase.WriteToErrorLogFile("Default Provider for PF is = " + tmpIdelProv);
                            dtDtxRow["ehrfield_value"] = tmpIdelProv.Trim() == "" ? "1" : tmpIdelProv;
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
                    bool Is_Record_Update = SynchOpenDentalBAL.Save_Patient_Form_Local_To_OpenDental(dtWebPatient_Form, strDbConnString, Service_Install_Id);
                }

                try
                {
                    GetMedicalOpenDentalHistoryRecords(Service_Install_Id, strPatientFormID);
                    SynchOpenDentalBAL.SaveMedicalHistoryLocalToOpenDental(strDbConnString, Service_Install_Id, strPatientFormID);
                    ObjGoalBase.WriteToSyncLogFile("Medical_History_Save Sync ( Service Install Id : " + Service_Install_Id + ".  Local Database To " + Utility.Application_Name + ") Successfully.");
                }
                catch (Exception ex2)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Medical_History_Save Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex2.Message);
                }

                try
                {
                    if (SynchOpenDentalBAL.SavePatientDisease(strDbConnString, Service_Install_Id, strPatientFormID))
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
                    if (SynchOpenDentalBAL.DeletePatientDisease(strDbConnString, Service_Install_Id, strPatientFormID))
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
                    if (SynchOpenDentalBAL.DeletePatientMedication(strDbConnString, Service_Install_Id, ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Delete_Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception ex1)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                }
                try
                {
                    if (SynchOpenDentalBAL.SavePatientMedication(strDbConnString, Service_Install_Id, ref isRecordSaved, ref SavePatientEHRID, strPatientFormID))
                    {
                        ObjGoalBase.WriteToSyncLogFile("Save Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Save Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]");
                    }
                }
                catch (Exception ex1)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Save Patient_Medication Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                }
                if (isRecordDeleted || isRecordSaved)
                {
                    Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                    if (Patient_EHR_IDS != "")
                    {
                        SynchDataOpenDental_PatientMedication(Patient_EHR_IDS);
                    }
                }

                #region PatientInformation Document
                try
                {
                    string strPatientID = "";
                    strPatientID = SynchLocalBAL.Get_Patient_EHR_ID_from_Patient_Form(strPatientFormID);
                    // GetTreatmentDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                    GetPatientDocument(Service_Install_Id, strPatientFormID);
                    GetPatientDocument_New(Service_Install_Id, strPatientFormID);
                    SynchOpenDentalBAL.Save_Document_in_OpenDental(strDbConnString, Service_Install_Id, strDocumentPath, strPatientFormID, strPatientID);
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form Document Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
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
