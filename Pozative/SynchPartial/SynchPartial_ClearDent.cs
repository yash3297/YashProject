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
using Pozative.BO;
using RestSharp;
using System.Net;


namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable

        bool IsClearDentProviderSync = false;
        bool IsClearDentOperatorySync = false;
        bool IsClearDentApptTypeSync = false;
        //bool Is_synched = false;
        //bool Is_synched_Provider = false;
        //bool Is_synched_Speciality = false;
        //bool Is_synched_Operatory = false;
        //bool Is_synched_OperatoryEvent = false;
        //bool Is_synched_Type = false;
        //bool Is_synched_Appointment = false;
        //bool Is_synched_RecallType = false;
        //bool Is_synched_ApptStatus = false;
        private BackgroundWorker bwSynchClearDent_Appointment = null;
        private System.Timers.Timer timerSynchClearDent_Appointment = null;

        private BackgroundWorker bwSynchClearDent_OperatoryEvent = null;
        private System.Timers.Timer timerSynchClearDent_OperatoryEvent = null;

        private BackgroundWorker bwSynchClearDent_Provider = null;
        private System.Timers.Timer timerSynchClearDent_Provider = null;

        private BackgroundWorker bwSynchClearDent_Speciality = null;
        private System.Timers.Timer timerSynchClearDent_Speciality = null;

        private BackgroundWorker bwSynchClearDent_Operatory = null;
        private System.Timers.Timer timerSynchClearDent_Operatory = null;

        private BackgroundWorker bwSynchClearDent_ApptType = null;
        private System.Timers.Timer timerSynchClearDent_ApptType = null;

        private BackgroundWorker bwSynchClearDent_Patient = null;
        private System.Timers.Timer timerSynchClearDent_Patient = null;

        private BackgroundWorker bwSynchClearDent_RecallType = null;
        private System.Timers.Timer timerSynchClearDent_RecallType = null;

        private BackgroundWorker bwSynchCleardent_User = null;
        private System.Timers.Timer timerSynchCleardent_User = null;

        private BackgroundWorker bwSynchClearDent_ApptStatus = null;
        private System.Timers.Timer timerSynchClearDent_ApptStatus = null;

        private BackgroundWorker bwSynchLocalToClearDent_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchLocalToClearDent_Appointment = null;

        private BackgroundWorker bwSynchLocalToClearDent_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToClearDent_Patient_Form = null;

        private BackgroundWorker bwSynchClearDent_OperatoryHours = null;
        private System.Timers.Timer timerSynchClearDent_OperatoryHours = null;

        private BackgroundWorker bwSynchClearDent_ProviderHours = null;
        private System.Timers.Timer timerSynchClearDent_ProviderHours = null;

        private BackgroundWorker bwSynchClearDent_Provider_OfficeHours = null;
        private System.Timers.Timer timerSynchClearDent_Provider_OfficeHours = null;

        private BackgroundWorker bwSynchClearDent_Operatory_OfficeHours = null;
        private System.Timers.Timer timerSynchClearDent_Operatory_OfficeHours = null;

        private BackgroundWorker bwSynchClearDent_MedicalHistory = null;
        private System.Timers.Timer timerSynchClearDent_MedicalHistory = null;

        private BackgroundWorker bwSynchCleardent_PatientPayment = null;
        private System.Timers.Timer timerSynchCleardent_PatientPayment = null;

        private BackgroundWorker bwSynchClearDent_Holiday = null;
        private System.Timers.Timer timerSynchClearDent_Holiday = null;

        private BackgroundWorker bwSynchClearDent_Insurance = null;
        private System.Timers.Timer timerSynchClearDent_Insurance = null;

        #endregion

        private void CallSynchClearDentToLocal()
        {
            if (Utility.AditSync)
            {
                fncSynchDataCleardent_PatientPayment();

                //SynchDataLocalToClearDent_Patient_Form();               

                SynchDataClearDent_Provider();
                fncSynchDataClearDent_Provider();

                fncSynchDataLocalToClearDent_Patient_Form();

                SynchDataClearDent_Operatory();
                fncSynchDataClearDent_Operatory();

                //SynchDataClearDent_ProviderHours();
                fncSynchDataClearDent_ProviderHours();

                //SynchDataClearDent_Provider_OfficeHours();
                fncSynchDataClearDent_Provider_OfficeHours();

                SynchDataClearDent_Speciality();
                fncSynchDataClearDent_Speciality();

                //SynchDataClearDent_OperatoryHours();
                fncSynchDataClearDent_OperatoryHours();

                //SynchDataClearDent_Operatory_OfficeHours();
                fncSynchDataClearDent_Operatory_OfficeHours();

                SynchDataClearDent_ApptType();
                fncSynchDataClearDent_ApptType();

                // SynchDataClearDent_OperatoryEvent();
                fncSynchDataClearDent_OperatoryEvent();


                //SynchDataClearDent_Appointment();
                //fncSynchDataClearDent_Appointment();

                if (Utility.ApptAutoBook)
                {
                    //if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower())
                    //{
                    //    return;
                    //}
                    fncSynchDataLocalToClearDent_Appointment();
                }

                //SynchDataClearDent_Patient();
                //fncSynchDataClearDent_Patient();

                SynchDataClearDent_RecallType();
                fncSynchDataClearDent_RecallType();

                SynchDataCleardent_User();
                fncSynchDataCleardent_User();

                SynchDataClearDent_ApptStatus();
                fncSynchDataClearDent_ApptStatus();

                SynchDataClearDent_Holiday();
                fncSynchDataClearDent_Holiday();

                Application.DoEvents();
                CallSynch_ClearDent_MedicalHistory();
                fncSynchDataClearDent_MedicalHistory();

                //rooja 23-4-24 task link EHR Updates for RCM - https://app.asana.com/0/1199184824722493/1207061756651636/f
                SynchDataClearDent_Insurance();
                fncSynchDataClearDent_Insurance();
            }
        }

        #region Synch Appointment

        private void fncSynchDataClearDent_Appointment()
        {
            InitBgWorkerClearDent_Appointment();
            InitBgTimerClearDent_Appointment();
        }

        private void InitBgTimerClearDent_Appointment()
        {
            timerSynchClearDent_Appointment = new System.Timers.Timer();
            this.timerSynchClearDent_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchClearDent_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Appointment_Tick);
            timerSynchClearDent_Appointment.Enabled = true;
            timerSynchClearDent_Appointment.Start();
            timerSynchClearDent_Appointment_Tick(null, null);
        }

        private void InitBgWorkerClearDent_Appointment()
        {
            bwSynchClearDent_Appointment = new BackgroundWorker();
            bwSynchClearDent_Appointment.WorkerReportsProgress = true;
            bwSynchClearDent_Appointment.WorkerSupportsCancellation = true;
            bwSynchClearDent_Appointment.DoWork += new DoWorkEventHandler(bwSynchClearDent_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchClearDent_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Appointment_RunWorkerCompleted);
        }

        private void timerSynchClearDent_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Appointment.Enabled = false;
                MethodForCallSynchOrderClearDent_Appointment();
            }
        }

        public void MethodForCallSynchOrderClearDent_Appointment()
        {
            System.Threading.Thread procThreadmainClearDent_Appointment = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Appointment);
            procThreadmainClearDent_Appointment.Start();
        }

        public void CallSyncOrderTableClearDent_Appointment()
        {
            if (bwSynchClearDent_Appointment.IsBusy != true)
            {
                bwSynchClearDent_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLiveDB_PatientPaymentLog_LocalTOClearDent();
                SynchDataClearDent_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Appointment.Enabled = true;
        }

        public void SynchDataClearDent_Appointment()
        {
            if (IsClearDentProviderSync && IsClearDentOperatorySync && IsClearDentApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    Is_synched_Appointment = false;

                    //New line by yogesh for sync appointments patients first
                    SynchDataClearDent_AppointmentsPatient();
                    if (IsPatientSyncedFirstTime)
                    {
                        SynchDataCleardent_NewPatient();
                    }
                    SynchDataClearDent_PatientStatus();
                    DataTable dtClearDentAppointment = SynchClearDentBAL.GetClearDentAppointmentData();
                    dtClearDentAppointment.Columns.Add("Provider_EHR_ID", typeof(string));
                    dtClearDentAppointment.Columns.Add("ProviderName", typeof(string));
                    dtClearDentAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                    dtClearDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                    dtClearDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                    dtClearDentAppointment.Columns.Add("ProcedureCode", typeof(string));

                    DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData("1");
                    DataTable dtLocalCompareForDeletedAppointment = SynchLocalBAL.GetLocalCompareForDeletedAppointmentData("1");
                    DataTable dtClearDentOperatory = SynchClearDentBAL.GetClearDentOperatoryData();

                    string ProcedureDesc = "";
                    string ProcedureCode = "";
                    DataTable DtClearDentAppointment_Procedures_Data = SynchClearDentBAL.GetClearDentAppointment_Procedures_Data();

                    string MobileEmail = string.Empty;
                    string Mobile_Contact = string.Empty;
                    string Email = string.Empty;
                    int cntCurRecord = 0;
                    foreach (DataRow dtDtxRow in dtClearDentAppointment.Rows)
                    {
                        string ProvIDList = string.Empty;
                        string ProvNameList = string.Empty;

                        ProvIDList = ProvIDList + dtDtxRow["provider_id1"].ToString() + ";";
                        ProvNameList = ProvNameList + dtDtxRow["ProviderName1"].ToString() + ";";
                        if (!string.IsNullOrEmpty(dtDtxRow["provider_id2"].ToString()))
                        {
                            ProvIDList = ProvIDList + dtDtxRow["provider_id2"].ToString() + ";";
                            ProvNameList = ProvNameList + dtDtxRow["ProviderName2"].ToString() + ";";
                        }
                        if (ProvIDList.Length > 0)
                        {
                            ProvIDList = ProvIDList.Substring(0, ProvIDList.Length - 1);
                        }
                        if (ProvNameList.Length > 0)
                        {
                            ProvNameList = ProvNameList.Substring(0, ProvNameList.Length - 1);
                        }
                        cntCurRecord = cntCurRecord + 1;
                        dtDtxRow["Provider_EHR_ID"] = ProvIDList;
                        dtDtxRow["ProviderName"] = ProvNameList;

                        ///////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                        ProcedureDesc = "";
                        ProcedureCode = "";

                        DataRow[] dtCurApptProcedure = DtClearDentAppointment_Procedures_Data.Select("appointment_id = '" + dtDtxRow["appointment_id"].ToString().Trim() + "'");
                        foreach (var dtSinProc in dtCurApptProcedure.ToList())
                        {
                            ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                        }

                        dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                        dtDtxRow["ProcedureCode"] = ProcedureCode;
                        /////////////////////////////////

                        DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appointment_id"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            // dtDtxRow["InsUptDlt"] = "UID";                        
                        }
                        else
                        {
                            DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Mobile_Contact + "'  AND ISNULL(Appt_EHR_ID,0) = 0 ");
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

                        if (row.Length > 0)
                        {                           
                            int commentlen = 1999;
                            if (dtDtxRow["comment"].ToString().Trim().Length < commentlen)
                            {
                                commentlen = dtDtxRow["comment"].ToString().Trim().Length;
                            }
                            if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                            {                               
                                dtDtxRow["InsUptDlt"] = 4;                                
                            }                           
                            else if (dtDtxRow["patient_ehr_id"].ToString().Trim() != row[0]["patient_ehr_id"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["Provider_EHR_ID"].ToString().Trim() != row[0]["Provider_EHR_ID"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["comment"].ToString().ToLower().Trim().Substring(0, commentlen) != row[0]["comment"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Convert.ToBoolean(row[0]["is_deleted"]) != false)
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Convert.ToBoolean(dtDtxRow["is_asap"]) != Convert.ToBoolean(row[0]["is_asap"]))
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
                            else if (row[0]["appointment_status_ehr_key"].ToString().Trim() == "99" && ((dtDtxRow["Confirmed"].ToString().ToLower() == "false") || dtDtxRow["appointment_status_ehr_key"].ToString() == "7"))
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (row[0]["appointment_status_ehr_key"].ToString().Trim() == "1" && ((dtDtxRow["Confirmed"].ToString().ToLower() == "true") || dtDtxRow["appointment_status_ehr_key"].ToString() == "7"))//nidhi task
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["appointment_status_ehr_key"].ToString() != "7" && dtDtxRow["Confirmed"].ToString().ToLower() == "true" && row[0]["appointment_status_ehr_key"].ToString().Trim() != "99")
                            {
                                dtDtxRow["appointment_status_ehr_key"] = 99;
                                dtDtxRow["Appointment_Status"] = "Confirmed";
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ProcedureDesc"].ToString().Trim() != row[0]["ProcedureDesc"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ProcedureCode"].ToString().Trim() != row[0]["ProcedureCode"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["appointment_status_ehr_key"].ToString() != "7" && dtDtxRow["Confirmed"].ToString().ToLower() != "true" && row[0]["appointment_status_ehr_key"].ToString().Trim() != "99")
                            {
                                if ((dtDtxRow["appointment_status_ehr_key"].ToString().Trim() != row[0]["appointment_status_ehr_key"].ToString().Trim()))
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



                    dtClearDentAppointment.AcceptChanges();


                    ///////////////////////////

                    DataView dvDTXAppt = new DataView(dtClearDentAppointment.Copy());
                    DataTable dtDTXAppt = dvDTXAppt.ToTable(true, "appointment_id");
                    DataTable dtLocal = dtLocalCompareForDeletedAppointment.Copy();
                    DataTable dtDTX = dtDTXAppt.Copy();

                    foreach (DataRow rw in dtLocal.Select())
                    {
                        string strPrnt = rw[0].ToString();
                        foreach (DataRow row in dtDTX.Select())
                        {
                            string strchild = row[0].ToString();
                            if (strPrnt == strchild)
                            {
                                rw.Delete();
                            }
                        }
                    }
                    dtLocal.AcceptChanges();

                    foreach (DataRow dtDtlRow in dtLocal.Rows)
                    {
                        if (dtDtlRow["appointment_id"].ToString().Trim() != "" && dtDtlRow["appointment_id"].ToString().Trim() != "0" && dtDtlRow["appointment_id"].ToString().Trim() != "-")
                        {
                            DataRow ApptDtldr = dtClearDentAppointment.NewRow();
                            ApptDtldr["appointment_id"] = dtDtlRow["appointment_id"].ToString().Trim();
                            ApptDtldr["InsUptDlt"] = 3;
                            dtClearDentAppointment.Rows.Add(ApptDtldr);
                        }
                    }

                    ///////////////////////////

                    if (dtClearDentAppointment != null && dtClearDentAppointment.Rows.Count > 0)
                    {                       
                        bool status = SynchClearDentBAL.Save_Appointment_ClearDent_To_Local(dtClearDentAppointment);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                            ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                        }
                    }

                    //////////////////////

                    if (Utility.Application_Version.ToLower() == "9.10+".ToLower())
                    {
                        DataTable dtClearDentDeletedAppointment = SynchClearDentBAL.GetClearDentDeletedAppointmentData();
                        dtClearDentDeletedAppointment.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalAppointmentAfterInsert = SynchLocalBAL.GetLocalAppointmentData("1");
                        bool Save_DeletedAppointment_status = false;

                        foreach (DataRow dtDtxRow in dtClearDentDeletedAppointment.Rows)
                        {
                            DataRow[] row = dtLocalAppointmentAfterInsert.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length > 0)
                            {
                                if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        dtClearDentDeletedAppointment.AcceptChanges();
                        // dtClearDentDeletedAppointment.Columns["Appointment_id"].ColumnName = "Appt_EHR_ID";
                        if (dtClearDentDeletedAppointment != null && dtClearDentDeletedAppointment.Rows.Count > 0)
                        {
                            Save_DeletedAppointment_status = SynchClearDentBAL.Update_DeletedAppointment_ClearDent_To_Local(dtClearDentDeletedAppointment);
                        }
                    }
                    else
                    {

                        DataTable dtLocalAppointmentAfterInsert = SynchLocalBAL.GetLocalAppointmentData("1");
                        dtLocalAppointmentAfterInsert.Columns.Add("InsUptDlt", typeof(int));
                        bool Save_DeletedAppointment_status = false;

                        foreach (DataRow dtDtxRow in dtLocalAppointmentAfterInsert.Rows)
                        {
                            DataRow[] row = dtClearDentAppointment.Copy().Select("Appointment_id = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length == 0)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                        }
                        dtLocalAppointmentAfterInsert.AcceptChanges();
                        var lst = dtLocalAppointmentAfterInsert.Select("InsUptDlt = 2").ToList();
                        if (lst.Count > 0)
                        {
                            DataTable dtClearDentDeletedAppointment = lst.CopyToDataTable();
                            if (dtClearDentDeletedAppointment != null && dtClearDentDeletedAppointment.Rows.Count > 0)
                            {
                                Save_DeletedAppointment_status = SynchClearDentBAL.Update_DeletedAppointment_ClearDent_To_Local(dtClearDentDeletedAppointment);
                            }
                        }

                    }

                    /////////////////
                    SynchDataLiveDB_Push_Appointment();
                    Is_synched_Appointment = true;
                    IsEHRAllSync = true;
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
                finally
                {
                    SynchDataLiveDB_Push_Appointment();
                    Is_synched_Appointment = true;
                }
            }
        }

        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataClearDent_OperatoryEvent()
        {
            InitBgWorkerClearDent_OperatoryEvent();
            InitBgTimerClearDent_OperatoryEvent();
        }

        private void InitBgTimerClearDent_OperatoryEvent()
        {
            timerSynchClearDent_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchClearDent_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchClearDent_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_OperatoryEvent_Tick);
            timerSynchClearDent_OperatoryEvent.Enabled = true;
            timerSynchClearDent_OperatoryEvent.Start();
            timerSynchClearDent_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerClearDent_OperatoryEvent()
        {
            bwSynchClearDent_OperatoryEvent = new BackgroundWorker();
            bwSynchClearDent_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchClearDent_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchClearDent_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchClearDent_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchClearDent_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchClearDent_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderClearDent_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderClearDent_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainClearDent_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableClearDent_OperatoryEvent);
            procThreadmainClearDent_OperatoryEvent.Start();
        }

        public void CallSyncOrderTableClearDent_OperatoryEvent()
        {
            if (bwSynchClearDent_OperatoryEvent.IsBusy != true)
            {
                bwSynchClearDent_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_OperatoryEvent();
                //SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                //SynchDataLiveDB_Pull_PatientFollowUp();
                // SynchDataLiveDB_PatientSMSCallLog_LocalTOClearDent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_OperatoryEvent.Enabled = true;
        }

        public void SynchDataClearDent_OperatoryEvent()
        {
            if (IsClearDentOperatorySync && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtClearDentOperatoryEvent = SynchClearDentBAL.GetClearDentOperatoryEventData();
                    dtClearDentOperatoryEvent.Columns.Add("OE_LocalDB_ID", typeof(int));
                    dtClearDentOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));

                    DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData("1");

                    foreach (DataRow dtDtxRow in dtClearDentOperatoryEvent.Rows)
                    {

                        DataRow[] row = dtLocalOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtDtxRow["OE_EHR_ID"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            if (Convert.ToDateTime(dtDtxRow["StartTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["StartTime"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToDateTime(dtDtxRow["EndTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EndTime"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToString(dtDtxRow["comment"].ToString().ToLower().Trim()) != Convert.ToString(row[0]["comment"]).ToLower())
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




                    dtClearDentOperatoryEvent.AcceptChanges();

                    if (dtClearDentOperatoryEvent != null && dtClearDentOperatoryEvent.Rows.Count > 0)
                    {
                        bool status = SynchClearDentBAL.Save_OperatoryEvent_ClearDent_To_Local(dtClearDentOperatoryEvent);

                        if (status)
                        {
                            ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_OperatoryEvent();
                        }
                    }
                    else
                    {
                        bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent_Push");
                    }

                    if (Utility.Application_Version.ToLower() == "9.10+".ToLower())
                    {
                        DataTable dtDeletedClearDentOperatoryEvent = SynchClearDentBAL.GetClearDentDeletedOperatoryEventData();
                        dtDeletedClearDentOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalOperatoryEventAfterInsert = SynchClearDentBAL.GetLocalOperatoryEventData();
                        bool Save_DeletedAppointment_status = false;

                        foreach (DataRow dtDtxRow in dtDeletedClearDentOperatoryEvent.Rows)
                        {
                            DataRow[] row = dtLocalOperatoryEventAfterInsert.Copy().Select("OE_EHR_ID = '" + dtDtxRow["OE_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length > 0)
                            {
                                if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        dtDeletedClearDentOperatoryEvent.AcceptChanges();

                        if (dtDeletedClearDentOperatoryEvent != null && dtDeletedClearDentOperatoryEvent.Rows.Count > 0)
                        {
                            Save_DeletedAppointment_status = SynchClearDentBAL.Update_DeletedOperatoryEvent_ClearDent_To_Local(dtDeletedClearDentOperatoryEvent);
                            if (Save_DeletedAppointment_status)
                            {
                                SynchDataLiveDB_Push_OperatoryEvent();
                            }
                        }

                    }
                    else
                    {
                        DataTable dtLocalOperatoryEventAfterInsert = SynchLocalBAL.GetLocalOperatoryEventData("1");
                        dtLocalOperatoryEventAfterInsert.Columns.Add("InsUptDlt", typeof(int));
                        bool Save_DeletedAppointment_status = false;

                        foreach (DataRow dtDtxRow in dtLocalOperatoryEventAfterInsert.Rows)
                        {
                            DataRow[] row = dtClearDentOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtDtxRow["OE_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length == 0)
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                        }
                        dtLocalOperatoryEventAfterInsert.AcceptChanges();
                        var lst = dtLocalOperatoryEventAfterInsert.Select("InsUptDlt = 2").ToList();
                        if (lst.Count > 0)
                        {
                            DataTable dtDeletedClearDentOperatoryEvent = lst.CopyToDataTable();
                            if (dtDeletedClearDentOperatoryEvent != null && dtDeletedClearDentOperatoryEvent.Rows.Count > 0)
                            {
                                Save_DeletedAppointment_status = SynchClearDentBAL.Update_DeletedOperatoryEvent_ClearDent_To_Local(dtDeletedClearDentOperatoryEvent);
                            }
                            if (Save_DeletedAppointment_status)
                            {
                                SynchDataLiveDB_Push_OperatoryEvent();
                            }
                        }
                    }


                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
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

        private void fncSynchDataClearDent_Provider()
        {
            InitBgWorkerClearDent_Provider();
            InitBgTimerClearDent_Provider();
        }

        private void InitBgTimerClearDent_Provider()
        {
            timerSynchClearDent_Provider = new System.Timers.Timer();
            this.timerSynchClearDent_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchClearDent_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Provider_Tick);
            timerSynchClearDent_Provider.Enabled = true;
            timerSynchClearDent_Provider.Start();
        }

        private void InitBgWorkerClearDent_Provider()
        {
            bwSynchClearDent_Provider = new BackgroundWorker();
            bwSynchClearDent_Provider.WorkerReportsProgress = true;
            bwSynchClearDent_Provider.WorkerSupportsCancellation = true;
            bwSynchClearDent_Provider.DoWork += new DoWorkEventHandler(bwSynchClearDent_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchClearDent_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Provider_RunWorkerCompleted);
        }

        private void timerSynchClearDent_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Provider.Enabled = false;
                MethodForCallSynchOrderClearDent_Provider();
            }
        }

        public void MethodForCallSynchOrderClearDent_Provider()
        {
            System.Threading.Thread procThreadmainClearDent_Provider = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Provider);
            procThreadmainClearDent_Provider.Start();
        }

        public void CallSyncOrderTableClearDent_Provider()
        {
            if (bwSynchClearDent_Provider.IsBusy != true)
            {
                bwSynchClearDent_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_Provider();
                CommonFunction.GetMasterSync();

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Provider.Enabled = true;
        }

        public void SynchDataClearDent_Provider()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtClearDentProvider = SynchClearDentBAL.GetClearDentProviderData();
                    dtClearDentProvider.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", "1");

                    foreach (DataRow dtDtxRow in dtClearDentProvider.Rows)
                    {
                        DataRow[] row = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtDtxRow["Provider_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Provider_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToByte(dtDtxRow["is_active"].ToString().Trim()) != Convert.ToByte(row[0]["is_active"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            //else if (dtDtxRow["mi"].ToString().Trim() != row[0]["MI"].ToString().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}
                            //else if (dtDtxRow["provider_speciality"].ToString().Trim() != row[0]["provider_speciality"].ToString().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
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

                    dtClearDentProvider.AcceptChanges();

                    if (dtClearDentProvider != null && dtClearDentProvider.Rows.Count > 0)
                    {
                        bool status = SynchClearDentBAL.Save_Provider_ClearDent_To_Local(dtClearDentProvider);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                            ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsClearDentProviderSync = true;

                            SynchDataLiveDB_Push_Provider();
                            //SynchDataClearDent_ProviderOfficeHours();
                        }
                        else
                        {
                            IsClearDentProviderSync = false;
                        }
                    }
                    IsProviderSyncedFirstTime = true;
                }
                catch (Exception ex)
                {
                    IsProviderSyncedFirstTime = true;
                    ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
            SynchDataCleardent_Medication();
        }

        public void SynchDataClearDent_ProviderOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    DataTable dtClearDentProviderOfficeHours = SynchClearDentBAL.GetClearDentProviderOfficeHours();
                    DataTable dtClearDentLocalProviderOfficeHours = SynchLocalBAL.GetLocalProviderOfficeHours("1");
                    //DataTable dtClearDentProvider = SynchClearDentBAL.GetClearDentProviderData();

                    dtClearDentProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtClearDentProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtClearDentLocalProviderOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtClearDentLocalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtClearDentLocalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtClearDentLocalProviderOfficeHours = CompareDataTableRecords(ref dtClearDentProviderOfficeHours, dtClearDentLocalProviderOfficeHours, "POH_EHR_ID", "POH_LocalDB_ID", "POH_LocalDB_ID,POH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtClearDentProviderOfficeHours.AcceptChanges();

                    if (dtClearDentProviderOfficeHours != null && dtClearDentProviderOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtClearDentProviderOfficeHours.Clone();
                        if (dtClearDentProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtClearDentLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtClearDentProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentProviderOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtClearDentLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentLocalProviderOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderOfficeHours", "POH_LocalDB_ID,POH_Web_ID", "POH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtClearDentProviderOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        #endregion

        #region Synch Speciality

        private void fncSynchDataClearDent_Speciality()
        {
            InitBgWorkerClearDent_Speciality();
            InitBgTimerClearDent_Speciality();
        }

        private void InitBgTimerClearDent_Speciality()
        {
            timerSynchClearDent_Speciality = new System.Timers.Timer();
            this.timerSynchClearDent_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchClearDent_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Speciality_Tick);
            timerSynchClearDent_Speciality.Enabled = true;
            timerSynchClearDent_Speciality.Start();
        }

        private void InitBgWorkerClearDent_Speciality()
        {
            bwSynchClearDent_Speciality = new BackgroundWorker();
            bwSynchClearDent_Speciality.WorkerReportsProgress = true;
            bwSynchClearDent_Speciality.WorkerSupportsCancellation = true;
            bwSynchClearDent_Speciality.DoWork += new DoWorkEventHandler(bwSynchClearDent_Speciality_DoWork);
            bwSynchClearDent_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Speciality_RunWorkerCompleted);
        }

        private void timerSynchClearDent_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Speciality.Enabled = false;
                MethodForCallSynchOrderClearDent_Speciality();
            }
        }

        public void MethodForCallSynchOrderClearDent_Speciality()
        {
            System.Threading.Thread procThreadmainClearDent_Speciality = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Speciality);
            procThreadmainClearDent_Speciality.Start();
        }

        public void CallSyncOrderTableClearDent_Speciality()
        {
            if (bwSynchClearDent_Speciality.IsBusy != true)
            {
                bwSynchClearDent_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_Speciality();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Speciality.Enabled = true;
        }

        public void SynchDataClearDent_Speciality()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {

                try
                {

                    DataTable dtClearDentSpeciality = SynchClearDentBAL.GetClearDentProviderData();
                    DataView view = new DataView(dtClearDentSpeciality);
                    DataTable dtSpecialitydistinctValues = view.ToTable(true, "provider_speciality");
                    dtSpecialitydistinctValues.Columns.Add("InsUptDlt", typeof(int));

                    DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData("1");

                    foreach (DataRow dtDtxRow in dtSpecialitydistinctValues.Rows)
                    {
                        DataRow[] row = dtLocalSpeciality.Copy().Select("speciality_Name = '" + dtDtxRow["provider_speciality"] + "'");
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
                        bool status = SynchClearDentBAL.Save_Speciality_ClearDent_To_Local(dtSpecialitydistinctValues);
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

        private void fncSynchDataClearDent_Operatory()
        {
            InitBgWorkerClearDent_Operatory();
            InitBgTimerClearDent_Operatory();
        }

        private void InitBgTimerClearDent_Operatory()
        {
            timerSynchClearDent_Operatory = new System.Timers.Timer();
            this.timerSynchClearDent_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchClearDent_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Operatory_Tick);
            timerSynchClearDent_Operatory.Enabled = true;
            timerSynchClearDent_Operatory.Start();
        }

        private void InitBgWorkerClearDent_Operatory()
        {
            bwSynchClearDent_Operatory = new BackgroundWorker();
            bwSynchClearDent_Operatory.WorkerReportsProgress = true;
            bwSynchClearDent_Operatory.WorkerSupportsCancellation = true;
            bwSynchClearDent_Operatory.DoWork += new DoWorkEventHandler(bwSynchClearDent_Operatory_DoWork);
            bwSynchClearDent_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Operatory_RunWorkerCompleted);
        }

        private void timerSynchClearDent_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Operatory.Enabled = false;
                MethodForCallSynchOrderClearDent_Operatory();
            }
        }

        public void MethodForCallSynchOrderClearDent_Operatory()
        {
            System.Threading.Thread procThreadmainClearDent_Operatory = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Operatory);
            procThreadmainClearDent_Operatory.Start();
        }

        public void CallSyncOrderTableClearDent_Operatory()
        {
            if (bwSynchClearDent_Operatory.IsBusy != true)
            {
                bwSynchClearDent_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Operatory.Enabled = true;
        }

        public void SynchDataClearDent_Operatory()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                Sync_FolderList();

                try
                {
                    DataTable dtClearDentOperatory = SynchClearDentBAL.GetClearDentOperatoryData();
                    dtClearDentOperatory.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData("1");

                    foreach (DataRow dtDtxRow in dtClearDentOperatory.Rows)
                    {
                        DataRow[] row = dtLocalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtDtxRow["Operatory_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Operatory_Name"].ToString().ToLower().Trim() != row[0]["Operatory_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToString(dtDtxRow["OperatoryOrder"].ToString()).Trim() != Convert.ToString(row[0]["OperatoryOrder"].ToString()).Trim())
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
                    //    DataRow[] row = dtClearDentOperatory.Copy().Select("op_id = '" + dtLocalRow["Operatory_EHR_ID"] + "'");
                    //    if (row.Length <= 0)
                    //    {
                    //        dtClearDentOperatory.Rows.Add(dtLocalRow["Operatory_EHR_ID"], dtLocalRow["Operatory_Name"], "D");
                    //    }
                    //}

                    foreach (DataRow dtRow in dtLocalOperatory.Rows)
                    {
                        DataRow rowBlcOpt = dtClearDentOperatory.Copy().Select("Operatory_EHR_ID = '" + dtRow["Operatory_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                        if (rowBlcOpt == null)
                        {
                            DataRow ESApptDtldr = dtClearDentOperatory.NewRow();
                            ESApptDtldr["Operatory_EHR_ID"] = dtRow["Operatory_EHR_ID"].ToString().Trim();
                            ESApptDtldr["InsUptDlt"] = 3;
                            dtClearDentOperatory.Rows.Add(ESApptDtldr);
                        }
                    }

                    dtClearDentOperatory.AcceptChanges();

                    if (dtClearDentOperatory != null && dtClearDentOperatory.Rows.Count > 0)
                    {
                        bool status = SynchClearDentBAL.Save_Operatory_ClearDent_To_Local(dtClearDentOperatory);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                            ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsClearDentOperatorySync = true;

                            SynchDataLiveDB_Push_Operatory();
                        }
                        else
                        {
                            IsClearDentOperatorySync = false;
                        }
                        #region Deleted Operatory
                        dtClearDentOperatory = dtClearDentOperatory.Clone();
                        DataTable dtClearDentDeletedOperatory = SynchClearDentBAL.GetClearDentDeletedOperatoryData();
                        DataTable dtLocalOperatoryAfterInsert = SynchLocalBAL.GetLocalOperatoryData("1");
                        foreach (DataRow dtDtlRow in dtClearDentDeletedOperatory.Rows)
                        {
                            DataRow[] row = dtLocalOperatoryAfterInsert.Copy().Select("Operatory_EHR_ID = '" + dtDtlRow["Operatory_EHR_ID"].ToString().Trim() + "'");
                            if (row.Length > 0)
                            {
                                if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                {
                                    DataRow ApptDtldr = dtClearDentOperatory.NewRow();
                                    ApptDtldr["Operatory_EHR_ID"] = dtDtlRow["Operatory_EHR_ID"].ToString().Trim();
                                    ApptDtldr["InsUptDlt"] = 3;
                                    dtClearDentOperatory.Rows.Add(ApptDtldr);
                                }
                            }
                        }
                        if (dtClearDentOperatory != null && dtClearDentOperatory.Rows.Count > 0)
                        {
                            status = SynchClearDentBAL.Save_Operatory_ClearDent_To_Local(dtClearDentOperatory);
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Operatory Hours

        private void fncSynchDataClearDent_OperatoryHours()
        {
            //SynchDataClearDent_OperatoryHours();
            //SynchDataClearDentToLocal_OperatoryOfficeHours();
            InitBgWorkerClearDent_OperatoryHours();
            InitBgTimerClearDent_OperatoryHours();
        }

        private void InitBgTimerClearDent_OperatoryHours()
        {
            timerSynchClearDent_OperatoryHours = new System.Timers.Timer();
            this.timerSynchClearDent_OperatoryHours.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchClearDent_OperatoryHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_OperatoryHours_Tick);
            timerSynchClearDent_OperatoryHours.Enabled = true;
            timerSynchClearDent_OperatoryHours.Start();
        }

        private void InitBgWorkerClearDent_OperatoryHours()
        {
            bwSynchClearDent_OperatoryHours = new BackgroundWorker();
            bwSynchClearDent_OperatoryHours.WorkerReportsProgress = true;
            bwSynchClearDent_OperatoryHours.WorkerSupportsCancellation = true;
            bwSynchClearDent_OperatoryHours.DoWork += new DoWorkEventHandler(bwSynchClearDent_OperatoryHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchClearDent_OperatoryHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_OperatoryHours_RunWorkerCompleted);
        }

        private void timerSynchClearDent_OperatoryHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_OperatoryHours.Enabled = false;
                MethodForCallSynchOrderClearDent_OperatoryHours();
            }
        }

        public void MethodForCallSynchOrderClearDent_OperatoryHours()
        {
            System.Threading.Thread procThreadmainClearDent_OperatoryHours = new System.Threading.Thread(this.CallSyncOrderTableClearDent_OperatoryHours);
            procThreadmainClearDent_OperatoryHours.Start();
        }

        public void CallSyncOrderTableClearDent_OperatoryHours()
        {
            if (bwSynchClearDent_OperatoryHours.IsBusy != true)
            {
                bwSynchClearDent_OperatoryHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_OperatoryHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_OperatoryHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataClearDentToLocal_OperatoryOfficeHours();
                SynchDataClearDent_OperatoryHours();
                // CallSynch_OperatoryHoursHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_OperatoryHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_OperatoryHours.Enabled = true;
        }

        public void SynchDataClearDent_OperatoryHours()
        {
            CallSynchClearDent_OperatoryHours();

        }

        #region Sync FolderList

        public void Sync_FolderList()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {

                try
                {
                    DataTable dtClearDentFolderList = SynchClearDentBAL.GetClearDentFolderListData();
                    dtClearDentFolderList.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalFolderList = SynchLocalBAL.GetLocalFolderListData("1");

                    foreach (DataRow dtDtxRow in dtClearDentFolderList.Rows)
                    {
                        DataRow[] row = dtLocalFolderList.Copy().Select("FolderList_EHR_ID = '" + dtDtxRow["FolderList_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Folder_Name"].ToString().Trim() != row[0]["Folder_Name"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToBoolean( dtDtxRow["Is_Deleted"]) != Convert.ToBoolean(row[0]["Is_Deleted"]))
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

                    foreach (DataRow dtRow in dtLocalFolderList.Rows)
                    {
                        DataRow rowBlcOpt = dtClearDentFolderList.Copy().Select("FolderList_EHR_ID = '" + dtRow["FolderList_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                        if (rowBlcOpt == null)
                        {
                            if (!Convert.ToBoolean(dtRow["is_deleted"]))
                            {
                                DataRow ESApptDtldr = dtClearDentFolderList.NewRow();
                                ESApptDtldr["FolderList_EHR_ID"] = dtRow["FolderList_EHR_ID"].ToString().Trim();
                                ESApptDtldr["InsUptDlt"] = 3;
                                dtClearDentFolderList.Rows.Add(ESApptDtldr);
                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(dtRow["is_deleted"]))
                            {
                                DataRow ESApptDtldr = dtClearDentFolderList.NewRow();
                                ESApptDtldr["FolderList_EHR_ID"] = dtRow["FolderList_EHR_ID"].ToString().Trim();
                                ESApptDtldr["InsUptDlt"] = 4;
                                dtClearDentFolderList.Rows.Add(ESApptDtldr);
                            }
                        }
                    }

                    dtClearDentFolderList.AcceptChanges();

                    if (dtClearDentFolderList != null && dtClearDentFolderList.Rows.Count > 0)
                    {
                        bool status = SynchClearDentBAL.Save_FolderList_ClearDent_To_Local(dtClearDentFolderList, "1", "0");
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FolderList");
                            ObjGoalBase.WriteToSyncLogFile("FolderList Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsEaglesoftOperatorySync = true;
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("FolderList Sync (" + Utility.Application_Name + " Db : " + "1" + " to Local Database) ] Error.");

                            IsEaglesoftOperatorySync = false;
                        }
                        SynchDataLiveDB_Push_FolderList();
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[FolderList Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        private void CallSynchClearDent_OperatoryHours()
         {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
            {
                try
                {
                    DataTable dtClearDentCustomeHours = SynchClearDentBAL.GetClearDentOperatoryHours();
                    DataTable dtClearDentLocalCustomeHours = SynchLocalBAL.GetLocalOperatoryHoursData("1");

                    dtClearDentCustomeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtClearDentCustomeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtClearDentCustomeHours.Columns.Contains("Is_Deleted"))
                    {
                        dtClearDentCustomeHours.Columns.Add("Is_Deleted", typeof(int));
                        dtClearDentCustomeHours.Columns["Is_Deleted"].DefaultValue = 1;
                    }

                    if (!dtClearDentLocalCustomeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtClearDentLocalCustomeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtClearDentLocalCustomeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    if (!dtClearDentCustomeHours.Columns.Contains("Is_Adit_Updated"))
                    {
                        dtClearDentCustomeHours.Columns.Add("Is_Adit_Updated", typeof(int));
                        dtClearDentCustomeHours.Columns["Is_Adit_Updated"].DefaultValue = 0;
                    }

                    dtClearDentLocalCustomeHours = CompareDataTablewithDateOnly(ref dtClearDentCustomeHours, dtClearDentLocalCustomeHours, "OH_EHR_ID", "OH_LocalDB_ID", "OH_LocalDB_ID,OH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Clinic_Number,Service_Install_Id,Provider_EHR_ID");

                    dtClearDentCustomeHours.AcceptChanges();
                    dtClearDentLocalCustomeHours.AcceptChanges();

                    if ((dtClearDentCustomeHours != null && dtClearDentCustomeHours.Rows.Count > 0) || (dtClearDentLocalCustomeHours != null && dtClearDentLocalCustomeHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtClearDentCustomeHours.Clone();
                        if (dtClearDentCustomeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtClearDentLocalCustomeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtClearDentCustomeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentCustomeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtClearDentLocalCustomeHours.Select("InsUptDlt IN (3) AND IS_DELETED = 'FALSE'").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentLocalCustomeHours.Select("InsUptDlt IN (3) AND IS_DELETED = 'FALSE'").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryHours", "OH_LocalDB_ID,OH_Web_ID,Provider_EHR_ID", "OH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtClearDentCustomeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
                    CallSynch_CleardentOperatoryChair();
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[OperatoryHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
            }
        }
        private void CallSynch_CleardentOperatoryChair()
        {
            try
            {
                for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                {
                    // MessageBox.Show("Call Sync CallSynch_CleardentOperatoryChair");
                    DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                    if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                    {
                        //DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        DateTime dtCurrentDtTime = Utility.LastSyncDateAditServer;

                        DataTable dtEaglesoftOperatoryChair = SynchClearDentBAL.GetClearDentOperatoryTimeOffHours();
                        if (dtEaglesoftOperatoryChair != null && dtEaglesoftOperatoryChair.Rows.Count > 0)
                        {
                            //dtEaglesoftOperatoryChair.Columns.Add("Operatory_EHR_ID", typeof(string));
                            //dtEaglesoftOperatoryChair.Columns.Add("OE_EHR_ID", typeof(string));
                            dtEaglesoftOperatoryChair.Columns.Add("OE_LocalDB_ID", typeof(int));
                            dtEaglesoftOperatoryChair.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalOperatoryChair = SynchLocalBAL.GetLocalOperatoryChairData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            dtEaglesoftOperatoryChair.Columns["OH_EHR_ID"].ColumnName = "OE_EHR_ID";
                            foreach (DataRow dtDtxRow in dtEaglesoftOperatoryChair.Rows)
                            {
                                DataRow[] row = dtLocalOperatoryChair.Copy().Select("OE_EHR_ID = '" + dtDtxRow["OE_EHR_ID"].ToString().Trim() + "' ");
                                if (row.Length > 0)
                                {

                                    int commentlen = 1999;
                                    if (dtDtxRow["comment"].ToString().Trim().Length < commentlen)
                                    {
                                        commentlen = dtDtxRow["comment"].ToString().Trim().Length;
                                    }

                                    if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["Provider_EHR_ID"].ToString().Trim() != row[0]["Provider_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["comment"].ToString().ToLower().Trim().Substring(0, commentlen) != row[0]["comment"].ToString().ToLower().Trim())
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
                            foreach (DataRow dtLOERow in dtLocalOperatoryChair.Rows)
                            {
                                DataRow[] rowBlcOpt = dtEaglesoftOperatoryChair.Copy().Select("OE_EHR_ID = '" + dtLOERow["OE_EHR_ID"].ToString().Trim() + "' ");
                                if (rowBlcOpt.Length > 0)
                                { }
                                else
                                {
                                    DataRow BlcOptDtldr = dtEaglesoftOperatoryChair.NewRow();
                                    BlcOptDtldr["OE_EHR_ID"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                                    BlcOptDtldr["InsUptDlt"] = 3;
                                    dtEaglesoftOperatoryChair.Rows.Add(BlcOptDtldr);
                                }
                            }

                            dtEaglesoftOperatoryChair.AcceptChanges();


                            bool status = SynchEaglesoftBAL.Save_OperatoryDayOff_Eaglesoft_To_Local(dtEaglesoftOperatoryChair, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                ObjGoalBase.WriteToSyncLogFile("OperatoryDayOff Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                ObjGoalBase.WriteToSyncLogFile("Operatory Time off Start Push Records.");
                                SynchDataLiveDB_Push_OperatoryDayOff();
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("[OperatoryDayOff Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                            }
                        }
                        else
                        {
                            bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryDayOff_Push");
                        }

                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryDayOff");
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Err " + ex.Message.ToString());
                throw;
            }
        }

        #endregion

        #region Operatory Office Hours

        private void fncSynchDataClearDent_Operatory_OfficeHours()
        {
            InitBgWorkerClearDent_Operatory_OfficeHours();
            InitBgTimerClearDent_Operatory_OfficeHours();
        }

        private void InitBgTimerClearDent_Operatory_OfficeHours()
        {
            timerSynchClearDent_Operatory_OfficeHours = new System.Timers.Timer();
            this.timerSynchClearDent_Operatory_OfficeHours.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory_OfficeHours;
            this.timerSynchClearDent_Operatory_OfficeHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Operatory_OfficeHours_Tick);
            timerSynchClearDent_Operatory_OfficeHours.Enabled = true;
            timerSynchClearDent_Operatory_OfficeHours.Start();
        }
        private void InitBgWorkerClearDent_Operatory_OfficeHours()
        {
            bwSynchClearDent_Operatory_OfficeHours = new BackgroundWorker();
            bwSynchClearDent_Operatory_OfficeHours.WorkerReportsProgress = true;
            bwSynchClearDent_Operatory_OfficeHours.WorkerSupportsCancellation = true;
            bwSynchClearDent_Operatory_OfficeHours.DoWork += new DoWorkEventHandler(bwSynchClearDent_Operatory_OfficeHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchClearDent_Operatory_OfficeHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Operatory_OfficeHours_RunWorkerCompleted);
        }
        private void timerSynchClearDent_Operatory_OfficeHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Operatory_OfficeHours.Enabled = false;
                MethodForCallSynchOrderClearDent_Operatory_OfficeHours();
            }
        }

        public void MethodForCallSynchOrderClearDent_Operatory_OfficeHours()
        {
            System.Threading.Thread procThreadmainClearDent_Operatory_OfficeHours = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Operatory_OfficeHours);
            procThreadmainClearDent_Operatory_OfficeHours.Start();
        }

        public void CallSyncOrderTableClearDent_Operatory_OfficeHours()
        {
            if (bwSynchClearDent_Operatory_OfficeHours.IsBusy != true)
            {
                bwSynchClearDent_Operatory_OfficeHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Operatory_OfficeHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Operatory_OfficeHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_Operatory_OfficeHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Operatory_OfficeHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Operatory_OfficeHours.Enabled = true;
        }

        public void SynchDataClearDent_Operatory_OfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtClearDentOperatoryOfficeHours = SynchClearDentBAL.GetClearDentOperatoryOfficeHours();
                    DataTable dtClearDentLocalOperatoryOfficeHours = SynchLocalBAL.GetLocalOperatoryOfficeHoursData("1");

                    dtClearDentOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtClearDentOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtClearDentLocalOperatoryOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtClearDentLocalOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtClearDentLocalOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtClearDentLocalOperatoryOfficeHours = CompareDataTableRecords(ref dtClearDentOperatoryOfficeHours, dtClearDentLocalOperatoryOfficeHours, "OOH_EHR_ID", "OOH_LocalDB_ID", "OOH_LocalDB_ID,OOH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");
                    dtClearDentOperatoryOfficeHours.AcceptChanges();

                    if (dtClearDentOperatoryOfficeHours != null && dtClearDentOperatoryOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtClearDentOperatoryOfficeHours.Clone();
                        if (dtClearDentOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtClearDentLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtClearDentOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtClearDentLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryOfficeHours", "OOH_LocalDB_ID,OOH_Web_ID", "OOH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtClearDentOperatoryOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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

        #endregion Operatory Office Hours

        #region Synch ProviderHours

        private void fncSynchDataClearDent_ProviderHours()
        {
            InitBgWorkerClearDent_ProviderHours();
            InitBgTimerClearDent_ProviderHours();
        }

        private void InitBgTimerClearDent_ProviderHours()
        {
            timerSynchClearDent_ProviderHours = new System.Timers.Timer();
            this.timerSynchClearDent_ProviderHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchClearDent_ProviderHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_ProviderHours_Tick);
            timerSynchClearDent_ProviderHours.Enabled = true;
            timerSynchClearDent_ProviderHours.Start();
        }

        private void InitBgWorkerClearDent_ProviderHours()
        {
            bwSynchClearDent_ProviderHours = new BackgroundWorker();
            bwSynchClearDent_ProviderHours.WorkerReportsProgress = true;
            bwSynchClearDent_ProviderHours.WorkerSupportsCancellation = true;
            bwSynchClearDent_ProviderHours.DoWork += new DoWorkEventHandler(bwSynchClearDent_ProviderHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchClearDent_ProviderHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_ProviderHours_RunWorkerCompleted);
        }

        private void timerSynchClearDent_ProviderHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_ProviderHours.Enabled = false;
                MethodForCallSynchOrderClearDent_ProviderHours();
            }
        }

        public void MethodForCallSynchOrderClearDent_ProviderHours()
        {
            System.Threading.Thread procThreadmainClearDent_ProviderHours = new System.Threading.Thread(this.CallSyncOrderTableClearDent_ProviderHours);
            procThreadmainClearDent_ProviderHours.Start();
        }

        public void CallSyncOrderTableClearDent_ProviderHours()
        {
            if (bwSynchClearDent_ProviderHours.IsBusy != true)
            {
                bwSynchClearDent_ProviderHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_ProviderHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_ProviderHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SyncPullLogsAndSaveinCleardent();
                SynchDataClearDent_ProviderHours();

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SyncPullLogsAndSaveinCleardent()
        {
            try
            {
                CheckCustomhoursForProviderOperatory();
                SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                SynchDataLiveDB_Pull_PatientFollowUp();
                SynchDataLiveDB_PatientSMSCallLog_LocalTOClearDent();
                fncPaymentSMSCallStatusUpdate();
                SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
            }
            catch (Exception)
            {

            }
        }

        private void bwSynchClearDent_ProviderHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_ProviderHours.Enabled = true;
        }

        public void SynchDataClearDent_ProviderHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && IsClearDentProviderSync && IsClearDentOperatorySync && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {


                    DataTable dtClearDentCustomeHours = SynchClearDentBAL.GetClearDentProviderHours();
                    DataTable dtClearDentLocalCustomeHours = SynchLocalBAL.GetLocalProviderHoursData("1");

                    dtClearDentCustomeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtClearDentCustomeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtClearDentLocalCustomeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtClearDentLocalCustomeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtClearDentLocalCustomeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    if (!dtClearDentCustomeHours.Columns.Contains("Is_Deleted"))
                    {
                        dtClearDentCustomeHours.Columns.Add("Is_Deleted", typeof(int));
                        dtClearDentCustomeHours.Columns["Is_Deleted"].DefaultValue = 1;
                    }
                    if (!dtClearDentCustomeHours.Columns.Contains("Is_Adit_Updated"))
                    {
                        dtClearDentCustomeHours.Columns.Add("Is_Adit_Updated", typeof(int));
                        dtClearDentCustomeHours.Columns["Is_Adit_Updated"].DefaultValue = 0;
                    }

                    dtClearDentLocalCustomeHours = CompareDataTablewithDateOnly(ref dtClearDentCustomeHours, dtClearDentLocalCustomeHours, "PH_EHR_ID", "PH_LocalDB_ID", "PH_LocalDB_ID,PH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Clinic_Number,Service_Install_Id");

                    dtClearDentCustomeHours.AcceptChanges();
                    dtClearDentLocalCustomeHours.AcceptChanges();

                    if ((dtClearDentCustomeHours != null && dtClearDentCustomeHours.Rows.Count > 0) || (dtClearDentLocalCustomeHours != null && dtClearDentLocalCustomeHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtClearDentCustomeHours.Clone();
                        if (dtClearDentCustomeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtClearDentLocalCustomeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtClearDentCustomeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentCustomeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtClearDentLocalCustomeHours.Select("InsUptDlt IN (3) AND IS_DELETED = 'FALSE'").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentLocalCustomeHours.Select("InsUptDlt IN (3) AND IS_DELETED = 'FALSE'").CopyToDataTable().CreateDataReader());
                            }
                           
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderHours", "PH_LocalDB_ID,PH_Web_ID", "PH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtClearDentCustomeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
            }
            catch (Exception ex)
            {
                Is_synched_Provider = false;
                ObjGoalBase.WriteToErrorLogFile("[Provider Hours Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Provider Office Hours

        private void fncSynchDataClearDent_Provider_OfficeHours()
        {
            InitBgWorkerClearDent_Provider_OfficeHours();
            InitBgTimerClearDent_Provider_OfficeHours();
        }

        private void InitBgTimerClearDent_Provider_OfficeHours()
        {
            timerSynchClearDent_Provider_OfficeHours = new System.Timers.Timer();
            this.timerSynchClearDent_Provider_OfficeHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider_OfficeHours;
            this.timerSynchClearDent_Provider_OfficeHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Provider_OfficeHours_Tick);
            timerSynchClearDent_Provider_OfficeHours.Enabled = true;
            timerSynchClearDent_Provider_OfficeHours.Start();
        }
        private void InitBgWorkerClearDent_Provider_OfficeHours()
        {
            bwSynchClearDent_Provider_OfficeHours = new BackgroundWorker();
            bwSynchClearDent_Provider_OfficeHours.WorkerReportsProgress = true;
            bwSynchClearDent_Provider_OfficeHours.WorkerSupportsCancellation = true;
            bwSynchClearDent_Provider_OfficeHours.DoWork += new DoWorkEventHandler(bwSynchClearDent_Provider_OfficeHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchClearDent_Provider_OfficeHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Provider_OfficeHours_RunWorkerCompleted);
        }
        private void timerSynchClearDent_Provider_OfficeHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Provider_OfficeHours.Enabled = false;
                MethodForCallSynchOrderClearDent_Provider_OfficeHours();
            }
        }

        public void MethodForCallSynchOrderClearDent_Provider_OfficeHours()
        {
            System.Threading.Thread procThreadmainClearDent_Provider_OfficeHours = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Provider_OfficeHours);
            procThreadmainClearDent_Provider_OfficeHours.Start();
        }

        public void CallSyncOrderTableClearDent_Provider_OfficeHours()
        {
            if (bwSynchClearDent_Provider_OfficeHours.IsBusy != true)
            {
                bwSynchClearDent_Provider_OfficeHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Provider_OfficeHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Provider_OfficeHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_Provider_OfficeHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Provider_OfficeHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Provider_OfficeHours.Enabled = true;
        }

        public void SynchDataClearDent_Provider_OfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtClearDentProviderOfficeHours = SynchClearDentBAL.GetClearDentProviderOfficeHours();
                    DataTable dtClearDentLocalProviderOfficeHours = SynchLocalBAL.GetLocalProviderOfficeHours("1");

                    dtClearDentProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtClearDentProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtClearDentLocalProviderOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtClearDentLocalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtClearDentLocalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtClearDentLocalProviderOfficeHours = CompareDataTableRecords(ref dtClearDentProviderOfficeHours, dtClearDentLocalProviderOfficeHours, "POH_EHR_ID", "POH_LocalDB_ID", "POH_LocalDB_ID,POH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtClearDentProviderOfficeHours.AcceptChanges();

                    if (dtClearDentProviderOfficeHours != null && dtClearDentProviderOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtClearDentProviderOfficeHours.Clone();
                        if (dtClearDentProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtClearDentLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtClearDentProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentProviderOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtClearDentLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentLocalProviderOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderOfficeHours", "POH_LocalDB_ID,POH_Web_ID", "POH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtClearDentProviderOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        #endregion Provider Office Hours

        #region Synch Appointment Type

        private void fncSynchDataClearDent_ApptType()
        {
            InitBgWorkerClearDent_ApptType();
            InitBgTimerClearDent_ApptType();
        }

        private void InitBgTimerClearDent_ApptType()
        {
            timerSynchClearDent_ApptType = new System.Timers.Timer();
            this.timerSynchClearDent_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchClearDent_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_ApptType_Tick);
            timerSynchClearDent_ApptType.Enabled = true;
            timerSynchClearDent_ApptType.Start();
        }

        private void InitBgWorkerClearDent_ApptType()
        {
            bwSynchClearDent_ApptType = new BackgroundWorker();
            bwSynchClearDent_ApptType.WorkerReportsProgress = true;
            bwSynchClearDent_ApptType.WorkerSupportsCancellation = true;
            bwSynchClearDent_ApptType.DoWork += new DoWorkEventHandler(bwSynchClearDent_ApptType_DoWork);
            bwSynchClearDent_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_ApptType_RunWorkerCompleted);
        }

        private void timerSynchClearDent_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_ApptType.Enabled = false;
                MethodForCallSynchOrderClearDent_ApptType();
            }
        }

        public void MethodForCallSynchOrderClearDent_ApptType()
        {
            System.Threading.Thread procThreadmainClearDent_ApptType = new System.Threading.Thread(this.CallSyncOrderTableClearDent_ApptType);
            procThreadmainClearDent_ApptType.Start();
        }

        public void CallSyncOrderTableClearDent_ApptType()
        {
            if (bwSynchClearDent_ApptType.IsBusy != true)
            {
                bwSynchClearDent_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_ApptType.Enabled = true;
        }

        public void SynchDataClearDent_ApptType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtClearDentApptType = SynchClearDentBAL.GetClearDentApptTypeData();
                    dtClearDentApptType.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData("1");

                    foreach (DataRow dtDtxRow in dtClearDentApptType.Rows)
                    {
                        DataRow[] row = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
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
                    foreach (DataRow dtDtxRow in dtLocalApptType.Rows)
                    {
                        DataRow[] row = dtClearDentApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtClearDentApptType.NewRow();
                            BlcOptDtldr["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["Type_Name"] = dtDtxRow["Type_Name"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtClearDentApptType.Rows.Add(BlcOptDtldr);
                        }
                    }

                    dtClearDentApptType.AcceptChanges();

                    if (dtClearDentApptType != null && dtClearDentApptType.Rows.Count > 0)
                    {
                        bool Type = SynchClearDentBAL.Save_ApptType_ClearDent_To_Local(dtClearDentApptType);

                        if (Type)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                            ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsClearDentApptTypeSync = true;

                            SynchDataLiveDB_Push_ApptType();
                        }
                        else
                        {
                            IsClearDentApptTypeSync = false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Appointment_Type Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Patient

        private void fncSynchDataClearDent_Patient()
        {
            InitBgWorkerClearDent_Patient();
            InitBgTimerClearDent_Patient();
        }

        private void InitBgTimerClearDent_Patient()
        {
            timerSynchClearDent_Patient = new System.Timers.Timer();
            this.timerSynchClearDent_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchClearDent_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Patient_Tick);
            timerSynchClearDent_Patient.Enabled = true;
            timerSynchClearDent_Patient.Start();
            timerSynchClearDent_Patient_Tick(null, null);
        }

        private void InitBgWorkerClearDent_Patient()
        {
            bwSynchClearDent_Patient = new BackgroundWorker();
            bwSynchClearDent_Patient.WorkerReportsProgress = true;
            bwSynchClearDent_Patient.WorkerSupportsCancellation = true;
            bwSynchClearDent_Patient.DoWork += new DoWorkEventHandler(bwSynchClearDent_Patient_DoWork);
            bwSynchClearDent_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Patient_RunWorkerCompleted);
        }

        private void timerSynchClearDent_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Patient.Enabled = false;
                MethodForCallSynchOrderClearDent_Patient();
            }
        }

        public void MethodForCallSynchOrderClearDent_Patient()
        {
            System.Threading.Thread procThreadmainClearDent_Patient = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Patient);
            procThreadmainClearDent_Patient.Start();
        }

        public void CallSyncOrderTableClearDent_Patient()
        {
            if (bwSynchClearDent_Patient.IsBusy != true)
            {
                bwSynchClearDent_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_Patient();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Patient.Enabled = true;
        }

        public void SynchDataClearDent_Patient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;
                    SynchDataLiveDB_Pull_EHR_Patientoptout();
                    DataTable dtClearDentPatient = SynchClearDentBAL.GetClearDentPatientData();
                    //DataTable dtClearDentPatientdue_date = SynchClearDentBAL.GetClearDentPatientdue_date();
                    //DataTable dtClearDentPatientInsuranceDataAllPat = SynchClearDentBAL.GetClearDentPatientInsuranceData("11");
                    //DataTable dtClearDentPatientcollect_payment = SynchClearDentBAL.GetClearDentPatientcollect_payment();
                    string patientTableName = "Patient";

                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");

                    if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                    {
                        patientTableName = "PatientCompare";
                    }

                    if (dtClearDentPatient != null && dtClearDentPatient.Rows.Count > 0)
                    {
                        bool isPatientSave = SynchClearDentBAL.Save_Patient_ClearDent_To_Local(dtClearDentPatient, patientTableName, true);
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
                    SynchDataClearDent_PatientStatus();
                    SynchDataClearDent_PatientImages();
                    SynchCleardent_PatientMedication("");
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

        public void SynchDataClearDent_PatientStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtClearDentPatientStatus = SynchClearDentBAL.GetClearDentPatientStatusData();
                    if (dtClearDentPatientStatus != null && dtClearDentPatientStatus.Rows.Count > 0)
                    {
                        SynchLocalBAL.UpdatePatient_Status(dtClearDentPatientStatus, "1");
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

        public void SynchDataCleardent_NewPatient()
        {

            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtCleardentPatientList = SynchClearDentBAL.GetClearDentPatientIdsData();
                    if (dtCleardentPatientList != null && dtCleardentPatientList.Rows.Count > 0)
                    {
                        DataTable dtLocalPatientList = SynchLocalBAL.GetLocalNewPatientData("1");
                        if ((dtLocalPatientList != null && dtLocalPatientList.Rows.Count > 0 && dtLocalPatientList.Select("Is_Adit_Updated = 1").Length > 0))
                        {
                            DataTable dtSaveRecords = new DataTable();
                            var itemsToBeAdded = (from CleardentPatient in dtCleardentPatientList.AsEnumerable()
                                                  join LocalPatient in dtLocalPatientList.AsEnumerable()
                                                  on new { PatID = CleardentPatient["Patient_EHR_ID"].ToString().Trim() }
                                                  equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim() }
                                                  into matchingRows
                                                  from matchingRow in matchingRows.DefaultIfEmpty()
                                                  where matchingRow == null
                                                  select CleardentPatient).ToList();
                            if (itemsToBeAdded.Count > 0)
                            {
                                string strPatID = String.Join(",", itemsToBeAdded.AsEnumerable().Select(x => x["Patient_EHR_ID"].ToString()).ToArray());
                                strPatID = strPatID.Replace(",", ",");
                                if (!string.IsNullOrEmpty(strPatID))
                                {
                                    dtSaveRecords = SynchClearDentBAL.GetCleardentPatientDataOfPatientId(strPatID);
                                }
                            }
                            if (!dtSaveRecords.Columns.Contains("InsUptDlt"))
                            {
                                dtSaveRecords.Columns.Add("InsUptDlt", typeof(int));
                                dtSaveRecords.Columns["InsUptDlt"].DefaultValue = 0;
                            }
                            if (dtSaveRecords.Rows.Count > 0)
                            {
                                bool status = SynchClearDentBAL.Save_Patient_Cleardent_To_Local_New(dtSaveRecords, "0", "1");

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

        public void SynchDataClearDent_AppointmentsPatient()
        {
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtClearDentPatient = SynchClearDentBAL.GetClearDentAppointmentsPatientData();
                    //DataTable dtClearDentPatientdue_date = SynchClearDentBAL.GetClearDentPatientdue_date();
                    //DataTable dtClearDentPatientInsuranceDataAllPat = SynchClearDentBAL.GetClearDentPatientInsuranceData("11");
                    //DataTable dtClearDentPatientcollect_payment = SynchClearDentBAL.GetClearDentPatientcollect_payment();
                    string patientTableName = "Patient";

                    string PatientEHRIDs = string.Join("','", dtClearDentPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                    if (PatientEHRIDs != string.Empty)
                    {
                        Is_synched_AppointmentsPatient = true;

                        PatientEHRIDs = "'" + PatientEHRIDs + "'";

                        DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, "1");

                        if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                        {
                            patientTableName = "PatientCompare";
                        }

                        if (dtClearDentPatient != null && dtClearDentPatient.Rows.Count > 0)
                        {
                            bool isPatientSave = SynchClearDentBAL.Save_Patient_ClearDent_To_Local(dtClearDentPatient, patientTableName);
                            if (isPatientSave)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                IsGetParientRecordDone = true;
                                SynchDataLiveDB_Push_Patient();

                                //  SynchDataLiveDB_Push_Patient_ASync();                           
                            }
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                            IsGetParientRecordDone = true;
                        }
                        Is_synched_AppointmentsPatient = false;
                        if (Is_Synched_PatientCallHit)
                        {
                            //Is_Synched_PatientCallHit = false;
                            //SynchDataClearDent_Patient();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Is_synched_AppointmentsPatient = false;
                    ObjGoalBase.WriteToErrorLogFile("[Apointment Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        public void SynchDataClearDent_PatientImages()
        {
            try
            {
                if (!Is_synched_PatientImages && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    // SynchDataLiveDB_Push_PatientImage();

                    Is_synched_PatientImages = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtClearDentPatientImages = SynchClearDentBAL.GetClearDentPatientImagesData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        dtClearDentPatientImages.Columns.Add("InsUptDlt", typeof(int));
                        dtClearDentPatientImages.Columns.Add("SourceLocation", typeof(string));
                        dtClearDentPatientImages.Columns["InsUptDlt"].DefaultValue = 0;
                        DataTable dtLocalPatientImages = SynchLocalBAL.GetLocalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString());
                        Utility.EHRProfileImagePath = SynchClearDentDAL.GetClearDentProfileImagePath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        foreach (DataRow dtDtxRow in dtClearDentPatientImages.Rows)
                        {
                            if (Utility.EHRProfileImagePath == string.Empty || Utility.EHRProfileImagePath == "")
                            {
                                dtDtxRow["SourceLocation"] = @"C:\Program Files (x86)\Prococious Technology Inc\ClearDent\Images\" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "\\-1\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                            }
                            else
                            {
                                dtDtxRow["SourceLocation"] = Utility.EHRProfileImagePath.ToString().Trim() + "\\" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "\\-1\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                            }

                            DataRow[] row = dtLocalPatientImages.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
                            if (row.Length > 0)
                            {
                                if (!Convert.ToBoolean(row[0]["Is_Deleted"]))
                                {
                                    if (dtDtxRow["Patient_Images_EHR_ID"].ToString().Trim() != row[0]["Patient_Images_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 0;
                                    }
                                    if (dtDtxRow["Patient_Images_FilePath"].ToString() != row[0]["Patient_Images_FilePath"].ToString())
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
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                        foreach (DataRow dtDtlRow in dtLocalPatientImages.Rows)
                        {
                            DataRow[] row = dtClearDentPatientImages.Copy().Select("Patient_EHR_ID = '" + dtDtlRow["Patient_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length <= 0)
                            {
                                if (!Convert.ToBoolean(dtDtlRow["Is_Deleted"]))
                                {
                                    DataRow ApptDtldr = dtClearDentPatientImages.NewRow();
                                    ApptDtldr["Patient_EHR_ID"] = dtDtlRow["Patient_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Patient_Images_EHR_ID"] = dtDtlRow["Patient_Images_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Image_EHR_Name"] = dtDtlRow["Image_EHR_Name"].ToString().Trim();
                                    ApptDtldr["Clinic_Number"] = dtDtlRow["Clinic_Number"].ToString().Trim();
                                    ApptDtldr["Service_Install_Id"] = dtDtlRow["Service_Install_Id"].ToString().Trim();
                                    ApptDtldr["Is_Deleted"] = 1;
                                    ApptDtldr["InsUptDlt"] = 3;
                                    dtClearDentPatientImages.Rows.Add(ApptDtldr);
                                }

                            }
                        }

                        dtClearDentPatientImages.AcceptChanges();
                        bool status = false;
                        DataTable dtSaveRecords = dtClearDentPatientImages.Clone();
                        if (dtClearDentPatientImages.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtClearDentPatientImages.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
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


        #region Patient

        public static void SynchDataLiveDB_Push_Patient_ASync()
        {

            try
            {
                IsParientFirstPushSync = false;

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
                    string arytmpdue_dates = string.Empty;
                    string arytmprecall_type = string.Empty;
                    string arytmprecall_typeId = string.Empty;

                    int totPatient = dtLocalPatient.Rows.Count;
                    int cntPatient = 0;

                    PushPatientRecord = 0;
                    TotalPushPatientRecord = dtLocalPatient.Rows.Count;
                    //List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, patientPushCounter);
                    List<DataTable> splitdt = Utility.SplitTable(dtLocalPatient, Utility.mstSyncBatchSize.Patient_Async);

                    for (int i = 0; i < splitdt.Count; i++)
                    {
                        //https://app.asana.com/0/751059797849097/1148328937003503
                        splitdt[i] = Utility.CreateDistinctRecords(splitdt[i], "Last_Sync_Date,EHR_Entry_DateTime,Patient_LocalDB_ID", "Patient_EHR_Id");

                        var JsonPatient = new System.Text.StringBuilder();
                        foreach (DataRow dtPatientRow in splitdt[i].Rows)
                        {
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

                            arytmpdue_dates = string.Empty;
                            arytmprecall_type = string.Empty;
                            arytmprecall_typeId = string.Empty;

                            string[] arydue_date = dtPatientRow["due_date"].ToString().Trim().Split('|');

                            foreach (string ada in arydue_date)
                            {
                                if (ada.Length > 0)
                                {
                                    string[] ary_r_d = ada.ToString().Trim().Split('@');
                                    if (ary_r_d[0].ToString() == "")
                                    {
                                        arytmpdue_dates = arytmpdue_dates + " " + ";";
                                    }
                                    else
                                    {
                                        arytmpdue_dates = arytmpdue_dates + Utility.ConvertDatetimeToUTCaditFormat(ary_r_d[0].ToString()).ToString().Trim() + ";";
                                    }

                                    if (ary_r_d[1].ToString() == "")
                                    {
                                        arytmprecall_type = arytmprecall_type + " " + ";";
                                    }
                                    else
                                    {
                                        arytmprecall_type = arytmprecall_type + ary_r_d[1].ToString() + ";";
                                    }

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
                                appointmentlocation = Utility.Location_ID,
                                location = Utility.Loc_ID,
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
                                secondary_insurance = dtPatientRow["Secondary_Insurance"].ToString().Trim(),
                                secondary_insurance_companyname = dtPatientRow["Secondary_Insurance_CompanyName"].ToString().Trim(),
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
                                remaining_benefit = dtPatientRow["remaining_benefit"].ToString().Trim(),
                                collect_payment = dtPatientRow["collect_payment"].ToString().Trim()
                            };
                            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            JsonPatient.Append(javaScriptSerializer.Serialize(patient) + ",");
                        }

                        string jsonString = "[" + JsonPatient.ToString().Remove(JsonPatient.Length - 1) + "]";
                        //string strPatient = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_WithList(jsonString, "patient");
                        string strPatient = SynchClearDentBAL.Push_Local_To_LiveDatabase_Patient_Async(jsonString, "patient");

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
                if (IsSynchPatient)
                {
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                    GoalBase.WriteToSyncLogFile_Static("Patient Sync (Local Database To Adit Server) Successfully.");
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Patient Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        #endregion

        #endregion

        #region Synch RecallType

        private void fncSynchDataClearDent_RecallType()
        {
            InitBgWorkerClearDent_RecallType();
            InitBgTimerClearDent_RecallType();
        }

        private void InitBgTimerClearDent_RecallType()
        {
            timerSynchClearDent_RecallType = new System.Timers.Timer();
            this.timerSynchClearDent_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchClearDent_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_RecallType_Tick);
            timerSynchClearDent_RecallType.Enabled = true;
            timerSynchClearDent_RecallType.Start();
        }

        private void InitBgWorkerClearDent_RecallType()
        {
            bwSynchClearDent_RecallType = new BackgroundWorker();
            bwSynchClearDent_RecallType.WorkerReportsProgress = true;
            bwSynchClearDent_RecallType.WorkerSupportsCancellation = true;
            bwSynchClearDent_RecallType.DoWork += new DoWorkEventHandler(bwSynchClearDent_RecallType_DoWork);
            bwSynchClearDent_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_RecallType_RunWorkerCompleted);
        }

        private void timerSynchClearDent_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_RecallType.Enabled = false;
                MethodForCallSynchOrderClearDent_RecallType();
            }
        }

        public void MethodForCallSynchOrderClearDent_RecallType()
        {
            System.Threading.Thread procThreadmainClearDent_RecallType = new System.Threading.Thread(this.CallSyncOrderTableClearDent_RecallType);
            procThreadmainClearDent_RecallType.Start();
        }

        public void CallSyncOrderTableClearDent_RecallType()
        {
            if (bwSynchClearDent_RecallType.IsBusy != true)
            {
                bwSynchClearDent_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_RecallType.Enabled = true;
        }

        public void SynchDataClearDent_RecallType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtClearDentRecallType = SynchClearDentBAL.GetClearDentRecallTypeData();
                    dtClearDentRecallType.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData("1");

                    foreach (DataRow dtDtxRow in dtClearDentRecallType.Rows)
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
                        DataRow[] row = dtClearDentRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtClearDentRecallType.NewRow();
                            BlcOptDtldr["RecallType_EHR_ID"] = dtDtxRow["RecallType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtClearDentRecallType.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtClearDentRecallType.AcceptChanges();

                    if (dtClearDentRecallType != null && dtClearDentRecallType.Rows.Count > 0)
                    {
                        bool status = SynchClearDentBAL.Save_RecallType_ClearDent_To_Local(dtClearDentRecallType);
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
        private void fncSynchDataCleardent_User()
        {
            InitBgWorkerCleardent_User();
            InitBgTimerCleardent_User();
        }

        private void InitBgTimerCleardent_User()
        {
            timerSynchCleardent_User = new System.Timers.Timer();
            this.timerSynchCleardent_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchCleardent_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCleardent_User_Tick);
            timerSynchCleardent_User.Enabled = true;
            timerSynchCleardent_User.Start();
        }

        private void timerSynchCleardent_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchCleardent_User.Enabled = false;
                MethodForCallSynchOrderCleardent_User();
            }
        }

        private void MethodForCallSynchOrderCleardent_User()
        {
            System.Threading.Thread procThreadmainCleardent_User = new System.Threading.Thread(this.CallSyncOrderTableCleardent_User);
            procThreadmainCleardent_User.Start();
        }

        private void CallSyncOrderTableCleardent_User()
        {
            if (bwSynchCleardent_User.IsBusy != true)
            {
                bwSynchCleardent_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void InitBgWorkerCleardent_User()
        {
            bwSynchCleardent_User = new BackgroundWorker();
            bwSynchCleardent_User.WorkerReportsProgress = true;
            bwSynchCleardent_User.WorkerSupportsCancellation = true;
            bwSynchCleardent_User.DoWork += new DoWorkEventHandler(bwSynchCleardent_User_DoWork);
            bwSynchCleardent_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCleardent_User_RunWorkerCompleted);
        }

        private void bwSynchCleardent_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchCleardent_User.Enabled = true;
        }

        private void bwSynchCleardent_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchCleardent_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataCleardent_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SynchDataCleardent_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtCleardentUser = SynchClearDentBAL.GetClearDentUserData();
                    dtCleardentUser.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalUser = SynchLocalBAL.GetLocalUser("1");

                    foreach (DataRow dtDtxRow in dtCleardentUser.Rows)
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

                    dtCleardentUser.AcceptChanges();

                    if (dtCleardentUser != null && dtCleardentUser.Rows.Count > 0)
                    {
                        bool status = SynchClearDentBAL.Save_User_ClearDent_To_Local(dtCleardentUser);
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

        #region Synch ApptStatus

        private void fncSynchDataClearDent_ApptStatus()
        {
            InitBgWorkerClearDent_ApptStatus();
            InitBgTimerClearDent_ApptStatus();
        }

        private void InitBgTimerClearDent_ApptStatus()
        {
            timerSynchClearDent_ApptStatus = new System.Timers.Timer();
            this.timerSynchClearDent_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchClearDent_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_ApptStatus_Tick);
            timerSynchClearDent_ApptStatus.Enabled = true;
            timerSynchClearDent_ApptStatus.Start();
        }

        private void InitBgWorkerClearDent_ApptStatus()
        {
            bwSynchClearDent_ApptStatus = new BackgroundWorker();
            bwSynchClearDent_ApptStatus.WorkerReportsProgress = true;
            bwSynchClearDent_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchClearDent_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchClearDent_ApptStatus_DoWork);
            bwSynchClearDent_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchClearDent_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_ApptStatus.Enabled = false;
                MethodForCallSynchOrderClearDent_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderClearDent_ApptStatus()
        {
            System.Threading.Thread procThreadmainClearDent_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableClearDent_ApptStatus);
            procThreadmainClearDent_ApptStatus.Start();
        }

        public void CallSyncOrderTableClearDent_ApptStatus()
        {
            if (bwSynchClearDent_ApptStatus.IsBusy != true)
            {
                bwSynchClearDent_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_ApptStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_ApptStatus.Enabled = true;
        }

        public void SynchDataClearDent_ApptStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtClearDentApptStatus = SynchClearDentBAL.GetClearDentApptStatusData();
                    dtClearDentApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    //dtClearDentApptStatus.Rows.Add(0, "<none>");
                    dtClearDentApptStatus.Rows.Add("99", "Confirmed", "confirm");
                    dtClearDentApptStatus.AcceptChanges();

                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData("1");

                    foreach (DataRow dtDtxRow in dtClearDentApptStatus.Rows)
                    {
                        DataRow[] row = dtLocalApptStatus.Copy().Select("ApptStatus_EHR_ID = '" + dtDtxRow["ApptStatus_EHR_ID"] + "'");
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

                    dtClearDentApptStatus.AcceptChanges();

                    if (dtClearDentApptStatus != null && dtClearDentApptStatus.Rows.Count > 0)
                    {
                        bool status = SynchClearDentBAL.Save_ApptStatus_ClearDent_To_Local(dtClearDentApptStatus);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                            ObjGoalBase.WriteToSyncLogFile("ApptStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            SynchDataLiveDB_Push_ApptStatus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[ApptStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Create Appointment

        private void fncSynchDataLocalToClearDent_Appointment()
        {
            InitBgWorkerLocalToClearDent_Appointment();
            InitBgTimerLocalToClearDent_Appointment();
        }

        private void InitBgTimerLocalToClearDent_Appointment()
        {
            timerSynchLocalToClearDent_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToClearDent_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToClearDent_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToClearDent_Appointment_Tick);
            timerSynchLocalToClearDent_Appointment.Enabled = true;
            timerSynchLocalToClearDent_Appointment.Start();
            timerSynchLocalToClearDent_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToClearDent_Appointment()
        {
            bwSynchLocalToClearDent_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToClearDent_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToClearDent_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToClearDent_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToClearDent_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToClearDent_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToClearDent_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToClearDent_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToClearDent_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToClearDent_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToClearDent_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToClearDent_Appointment);
            procThreadmainLocalToClearDent_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToClearDent_Appointment()
        {
            if (bwSynchLocalToClearDent_Appointment.IsBusy != true)
            {
                bwSynchLocalToClearDent_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToClearDent_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToClearDent_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToClearDent_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToClearDent_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToClearDent_Appointment.Enabled = true;
        }

        public void SynchDataLocalToClearDent_Appointment(string _filename_Appointment="", string _EHRLogdirectory_Appointment="")
        {
            try
            {
                   // CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData("1");
                    DataTable dtClearDentPatient = SynchClearDentBAL.GetClearDentPatientID_NameData();
                    DataTable dtIdelProv = SynchClearDentBAL.GetClearDentIdelProvider();

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

                    if(dtWebAppointment!=null)
                    {
                        if(dtWebAppointment.Rows.Count>0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                        }
                    }

                    foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                    {
                        string PatientName = tmpLastName + ", " + tmpFirstName;
                        string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
                        DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                        DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                        TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                        int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);

                        // string tmpApptDurPatern = "";
                        //for (int i = 0; i < tmpApptDurMinutes / 5; i++)
                        //{
                        //    tmpApptDurPatern = tmpApptDurPatern + "X";
                        //}

                        DataTable dtBookOperatoryApptWiseDateTime = SynchClearDentBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
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
                                DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("operatory_id = '" + tmpCheckOpId + "'");
                                if (rowBookOpTime.Length > 0)
                                {
                                    for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                    {
                                        appointment_EHR_id = rowBookOpTime[Bop]["appointment_id"].ToString();
                                        if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["StartTime"].ToString()))
                                            && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["EndTime"].ToString())))
                                        {
                                            IsConflict = true;
                                            break;
                                        }
                                        if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["StartTime"].ToString()))
                                            && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["EndTime"].ToString())))
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
                            tmpIdealOperatory = Operatory_EHR_IDs[0].ToString();
                        }
                        if (tmpIdealOperatory == "")
                        {
                            DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("appointment_id = " + appointment_EHR_id).CopyToDataTable();
                            bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());
                            if(_filename_Appointment != ""  &&  _EHRLogdirectory_Appointment  != "")
                            {
                                if (status)
                                {
                                    Utility.WriteSyncPullLog(_filename_Appointment, _EHRLogdirectory_Appointment, "Save Appointment Is_Appt_DoubleBook_In_Local success ");
                                }
                                else
                                {
                                    Utility.WriteSyncPullLog(_filename_Appointment, _EHRLogdirectory_Appointment, "Save Appointment Is_Appt_DoubleBook_In_Local failed ");
                                }

                            }
                            
                        


                        }
                        else
                        {

                            tmpPatient_id = 0;
                            tmpPatient_Gur_id = 0;
                            tmpAppt_EHR_id = 0;
                            tmpNewPatient = 1;
                            TmpWebPatientName = "";
                            TmpWebRevPatientName = "";

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
                            if (dtDtxRow["Patient_EHR_id"].ToString() != "-")
                            {
                                tmpPatient_id = Convert.ToInt64(dtDtxRow["Patient_EHR_id"].ToString());
                            }
                            else
                            {
                                tmpPatient_id = 0;
                            }
                            //string bdate = dtDtxRow["birth_date"].ToString();
                            //if (dtDtxRow["birth_date"].ToString() == "")
                            //{
                            //    bdate = "01/01/0001 00:00:00";
                            //}
                            if (tmpPatient_id == 0)
                            {
                                tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtClearDentPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DBConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                                //DataRow[] row = dtClearDentPatient.Copy().Select("Mobile = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR Home_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR Work_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' ");
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

                                //    tmpPatient_Gur_id = Convert.ToInt32(row[0]["Guarantor"].ToString());
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
                            //    tmpPatient_id = SynchClearDentBAL.Save_Patient_Local_To_ClearDent(tmpLastName.Trim(),
                            //                                                                        tmpFirstName,
                            //                                                                        dtDtxRow["MI"].ToString().Trim(),
                            //                                                                        dtDtxRow["Mobile_Contact"].ToString().Trim(),
                            //                                                                        dtDtxRow["Email"].ToString().Trim(),
                            //                                                                        tmpApptProv,
                            //                                                                        dtDtxRow["Appt_DateTime"].ToString().Trim(),
                            //                                                                        tmpPatient_Gur_id,
                            //                                                                        dtDtxRow["Birth_Date"].ToString().Trim());
                            //}
                            if (tmpPatient_id > 0)
                            {

                                tmpAppt_EHR_id = SynchClearDentBAL.Save_Appointment_Local_To_ClearDent(tmpPatient_id.ToString(), tmpApptDurMinutes, tmpIdealOperatory.ToString(), tmpApptProv,
                                   tmpStartTime, tmpEndTime, dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDtxRow["ApptType_EHR_ID"].ToString().Trim(), PatientName, dtDtxRow["comment"].ToString().Trim(), dtDtxRow["appt_treatmentcode"].ToString().Trim(), Convert.ToInt16((dtDtxRow["appointment_status_ehr_key"].ToString() == "" || dtDtxRow["appointment_status_ehr_key"].ToString() == "0") ? 1 : dtDtxRow["appointment_status_ehr_key"]));
                                if (tmpAppt_EHR_id > 0)
                                {

                                    bool isApptId_Update = SynchClearDentBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(),  _filename_Appointment , _EHRLogdirectory_Appointment);

                                    if (isApptId_Update)
                                    {
                                        Utility.WriteSyncPullLog(_filename_Appointment, _EHRLogdirectory_Appointment, "update Appointment Is_Appt_DoubleBook_In_Local success ");
                                    }
                                    else
                                    {
                                        Utility.WriteSyncPullLog(_filename_Appointment, _EHRLogdirectory_Appointment, "update Appointment Is_Appt_DoubleBook_In_Local failed ");
                                    }
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

        private void fncSynchDataLocalToClearDent_Patient_Form()
        {
            InitBgWorkerLocalToClearDent_Patient_Form();
            InitBgTimerLocalToClearDent_Patient_Form();
        }

        private void InitBgTimerLocalToClearDent_Patient_Form()
        {
            timerSynchLocalToClearDent_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToClearDent_Patient_Form.Interval = 1000 * GoalBase.intervalEHRSynch_PatientForm;
            this.timerSynchLocalToClearDent_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToClearDent_Patient_Form_Tick);
            timerSynchLocalToClearDent_Patient_Form.Enabled = true;
            timerSynchLocalToClearDent_Patient_Form.Start();
            timerSynchLocalToClearDent_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToClearDent_Patient_Form()
        {
            bwSynchLocalToClearDent_Patient_Form = new BackgroundWorker();
            bwSynchLocalToClearDent_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToClearDent_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToClearDent_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToClearDent_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToClearDent_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToClearDent_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToClearDent_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToClearDent_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToClearDent_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToClearDent_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToClearDent_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToClearDent_Patient_Form);
            procThreadmainLocalToClearDent_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToClearDent_Patient_Form()
        {
            if (bwSynchLocalToClearDent_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToClearDent_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToClearDent_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToClearDent_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToClearDent_Patient_Form();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToClearDent_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToClearDent_Patient_Form.Enabled = true;
        }

        public void SynchDataLocalToClearDent_Patient_Form()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    SynchDataLiveDB_Pull_PatientForm();
                    SynchDataLiveDB_Pull_PatientPortal();
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

                    foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                    {

                        if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "first_name")
                        {
                            dtDtxRow["ehrfield"] = "fld_strFName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "last_name")
                        {
                            dtDtxRow["ehrfield"] = "fld_strLName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "mobile")
                        {
                            dtDtxRow["ehrfield"] = "fld_strMTel";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "birth_date")
                        {
                            dtDtxRow["ehrfield"] = "fld_dtmBth";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "email")
                        {
                            dtDtxRow["ehrfield"] = "fld_strEmail";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "middle_name")
                        {
                            dtDtxRow["ehrfield"] = "fld_strMIni";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "preferred_name")
                        {
                            dtDtxRow["ehrfield"] = "fld_strPName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sex")
                        {
                            dtDtxRow["ehrfield"] = "fld_strSex";
                            if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MALE")
                            {
                                dtDtxRow["ehrfield_value"] = "M";
                            }
                            else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                            {
                                dtDtxRow["ehrfield_value"] = "F";
                            }
                            else
                            {
                                dtDtxRow["ehrfield_value"] = DBNull.Value;
                            }
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "status")
                        {
                            dtDtxRow["ehrfield"] = "fld_bytStatus";
                            if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "INACTIVE")
                            {
                                dtDtxRow["ehrfield_value"] = "0";
                            }
                            else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "ACTIVE")
                            {
                                dtDtxRow["ehrfield_value"] = "1";
                            }
                            else
                            {
                                dtDtxRow["ehrfield_value"] = "1";

                            }
                        }

                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "work_phone")
                        {
                            dtDtxRow["ehrfield"] = "fld_strOTel";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "zipcode")
                        {
                            dtDtxRow["ehrfield"] = "fld_strPCode";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_one")
                        {
                            dtDtxRow["ehrfield"] = "fld_strAddr1";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_two")
                        {
                            dtDtxRow["ehrfield"] = "fld_strAddr2";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "city")
                        {
                            dtDtxRow["ehrfield"] = "fld_strCity";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "state")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "home_phone")
                        {
                            dtDtxRow["ehrfield"] = "fld_strHTel";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "pri_provider_id")
                        {
                            dtDtxRow["ehrfield"] = "fld_shtPrId";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sec_provider_id")
                        {
                            dtDtxRow["ehrfield"] = "fld_shtHyId";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "salutation")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance")
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

                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "receive_email")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "receive_sms")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance")
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
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "ssn")
                        {
                            dtDtxRow["ehrfield"] = "fld_intSIN";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "emergencycontactname")
                        {
                            dtDtxRow["ehrfield"] = "fld_strEmergencyName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "emergencycontactnumber")
                        {
                            dtDtxRow["ehrfield"] = "fld_strEmergencyContact";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "school")
                        {
                            dtDtxRow["ehrfield"] = "fld_strSchool";
                        }



                        dtWebPatient_Form.AcceptChanges();

                    }
                    if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                        bool Is_Record_Update = SynchClearDentBAL.Save_Patient_Form_Local_To_ClearDent(dtWebPatient_Form);
                    }
                    GetMedicalClearDentHistoryRecords();
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

                    string Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed("1", Utility.Location_ID);
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
                    bool isDeletedMedication = false;
                    bool isSaveMedication = false;
                    string Patient_EHR_IDS = "";
                    string DeletePatientEHRID = "";
                    string SavePatientEHRID = "";
                    try
                    {
                        SynchClearDentBAL.DeletePatientMedicationLocalToClearDent("1",ref isDeletedMedication, ref DeletePatientEHRID);
                    }
                    catch (Exception exMed)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + exMed.Message);
                    }
                    try
                    {
                        SynchClearDentBAL.SavePatientMedicationLocalToClearDent("1",ref isSaveMedication, ref SavePatientEHRID);
                    }
                    catch (Exception exMed)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + exMed.Message);
                    }
                    if (isDeletedMedication || isSaveMedication)
                    {
                        Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                        if (Patient_EHR_IDS != "")
                        {
                            SynchCleardent_PatientMedication(Patient_EHR_IDS);
                        }
                    }

                    try
                    {

                        SynchClearDentBAL.SaveMedicalHistoryLocalToClearDent();

                    }
                    catch (Exception ex)
                    {
                        IsDocumentUpload = false;
                        ObjGoalBase.WriteToErrorLogFile("[Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }

                    ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    //GetPatientDocument("1");

                    //SynchClearDentBAL.Save_Document_in_ClearDent();

                    //#region Treatment Document
                    //try
                    //{
                    //    SynchClearDentBAL.Save_Treatment_Document_in_ClearDent();
                    //    #region change status as treatment doc impotred Completed
                    //    DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                    //    if (statusCompleted.Rows.Count > 0)
                    //    {
                    //        Change_Status_TreatmentDoc(statusCompleted, "Completed");
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    ObjGoalBase.WriteToErrorLogFile("[Patient_Treatment_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    //}
                    //#endregion
                    //#endregion
                    SynchDataLocalToClearDent_Patient_Document();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }


        #endregion

        #region Patient Document

        public void SynchDataLocalToClearDent_Patient_Document()
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
                        SynchDataLiveDB_Pull_InsuranceCarrierDoc();
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
                    SynchClearDentBAL.Save_Document_in_ClearDent();

                    #region Treatment Document
                    try
                    {
                        SynchClearDentBAL.Save_Treatment_Document_in_ClearDent();
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
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Treatment_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }

                    #endregion

                    #region Insurance Carrier
                    try
                    {
                        Utility.CheckEntryUserLoginIdExist();
                        if (Utility.EHR_UserLogin_ID == "" || Utility.EHR_UserLogin_ID == "0")
                        {
                            Utility.EHR_UserLogin_ID = SynchClearDentDAL.GetClearDentUserLoginId();
                        }
                        SynchClearDentBAL.Save_InsuranceCarrier_Document_in_ClearDent();
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
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Insurance_Carrier_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }

                    #endregion

                }
                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #region PatientPayment
        private void fncSynchDataCleardent_PatientPayment()
        {
            InitBgWorkerCleardent_PatientPayment();
            InitBgTimerCleardent_PatientPayment();
        }

        private void InitBgTimerCleardent_PatientPayment()
        {
            timerSynchCleardent_PatientPayment = new System.Timers.Timer();
            this.timerSynchCleardent_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
            this.timerSynchCleardent_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchCleardent_PatientPayment_Tick);
            timerSynchCleardent_PatientPayment.Enabled = true;
            timerSynchCleardent_PatientPayment.Start();
        }

        private void InitBgWorkerCleardent_PatientPayment()
        {
            bwSynchCleardent_PatientPayment = new BackgroundWorker();
            bwSynchCleardent_PatientPayment.WorkerReportsProgress = true;
            bwSynchCleardent_PatientPayment.WorkerSupportsCancellation = true;
            bwSynchCleardent_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchCleardent_PatientPayment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchCleardent_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchCleardent_PatientPayment_RunWorkerCompleted);
        }

        private void timerSynchCleardent_PatientPayment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchCleardent_PatientPayment.Enabled = false;
                MethodForCallSynchOrderCleardent_PatientPayment();
            }
        }

        public void MethodForCallSynchOrderCleardent_PatientPayment()
        {
            System.Threading.Thread procThreadmainCleardent_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTableCleardent_PatientPayment);
            procThreadmainCleardent_PatientPayment.Start();
        }

        public void CallSyncOrderTableCleardent_PatientPayment()
        {
            if (bwSynchCleardent_PatientPayment.IsBusy != true)
            {
                bwSynchCleardent_PatientPayment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchCleardent_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchCleardent_PatientPayment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLiveDB_PatientPaymentLog_LocalTOClearDent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }
        private void bwSynchCleardent_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchCleardent_PatientPayment.Enabled = true;
        }

        public void SynchDataLiveDB_PatientPaymentLog_LocalTOClearDent()
        {
            try
            {
                if (!IsPaymentSyncing)
                {
                    IsPaymentSyncing = true;
                    if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                    {
                        SynchDataLiveDB_Pull_PatientPaymentLog();
                        //CheckEntryUserLoginIdExist();
                        Int64 TransactionHeaderId = 0;
                        string noteId = "";
                        DataTable dtPatientPayment = new DataTable();
                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            noteId = "";
                            #region Call API for EHR Entry Done
                            if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                            {
                                Utility.CheckEntryUserLoginIdExist();
                                noteId = SynchClearDentBAL.Save_PatientPayment_Local_To_ClearDent(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (noteId != "")
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientPayment_LocalTOClearDent");
                                    ObjGoalBase.WriteToSyncLogFile("Patient Payment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Patient Payment Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                            else
                            {
                                ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                            }
                            #endregion

                            #region Sync those patient whose payment done in EHR

                            #endregion
                            //  }
                        }

                    }
                    IsPaymentSyncing = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { IsPaymentSyncing = false; }

        }

        #endregion


        public void SynchDataLiveDB_PatientSMSCallLog_LocalTOClearDent()
       {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    Int64 TransactionHeaderId = 0;
                    string noteId = "";
                    DataTable dtPatientPayment = new DataTable();
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        SynchClearDentBAL.DeleteDuplicatePatientLog(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        DataTable dtWebPatientSMSCallLog = SynchLocalBAL.GetLocalWebPatientSMSCallLogData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        ObjGoalBase.WriteToSyncLogFile("Total Patient Logs Count is " + dtWebPatientSMSCallLog.Rows.Count.ToString());
                        #region Call API for EHR Entry Done
                        if (dtWebPatientSMSCallLog != null && dtWebPatientSMSCallLog.Rows.Count > 0)
                        {
                            ObjGoalBase.WriteToSyncLogFile("Save SMS Call Log in EHR ");
                            Utility.CheckEntryUserLoginIdExist();
                            SynchClearDentBAL.Save_PatientSMSCall_Local_To_ClearDent(dtWebPatientSMSCallLog, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            //}
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("SynchDataPatientSMSCalllog_LocalTOClearDent");
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
                ObjGoalBase.WriteToErrorLogFile("[Patient_SMSCallLog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        #endregion

        #region MedicalHistory Fields

        private void fncSynchDataClearDent_MedicalHistory()
        {
            InitBgWorkerClearDent_MedicalHistory();
            InitBgTimerClearDent_MedicalHistory();
        }

        private void InitBgTimerClearDent_MedicalHistory()
        {
            timerSynchClearDent_MedicalHistory = new System.Timers.Timer();
            this.timerSynchClearDent_MedicalHistory.Interval = 1000 * GoalBase.intervalEHRSynch_MedicalHistory;
            this.timerSynchClearDent_MedicalHistory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_MedicalHistory_Tick);
            timerSynchClearDent_MedicalHistory.Enabled = true;
            timerSynchClearDent_MedicalHistory.Start();
            timerSynchClearDent_MedicalHistory_Tick(null, null);
        }

        private void InitBgWorkerClearDent_MedicalHistory()
        {
            bwSynchClearDent_MedicalHistory = new BackgroundWorker();
            bwSynchClearDent_MedicalHistory.WorkerReportsProgress = true;
            bwSynchClearDent_MedicalHistory.WorkerSupportsCancellation = true;
            bwSynchClearDent_MedicalHistory.DoWork += new DoWorkEventHandler(bwSynchClearDent_MedicalHistory_DoWork);
            bwSynchClearDent_MedicalHistory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_MedicalHistory_RunWorkerCompleted);
        }

        private void timerSynchClearDent_MedicalHistory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_MedicalHistory.Enabled = false;
                MethodForCallSynchOrderClearDent_MedicalHistory();
            }
        }

        public void MethodForCallSynchOrderClearDent_MedicalHistory()
        {
            System.Threading.Thread procThreadmainClearDent_MedicalHistory = new System.Threading.Thread(this.CallSyncOrderTableClearDent_MedicalHistory);
            procThreadmainClearDent_MedicalHistory.Start();
        }

        public void CallSyncOrderTableClearDent_MedicalHistory()
        {
            if (bwSynchClearDent_MedicalHistory.IsBusy != true)
            {
                bwSynchClearDent_MedicalHistory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_MedicalHistory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_MedicalHistory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_MedicalHistory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_MedicalHistory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_MedicalHistory.Enabled = true;
        }

        public void SynchDataClearDent_MedicalHistory()
        {
            CallSynch_ClearDent_MedicalHistory();

        }

        private void CallSynch_ClearDent_MedicalHistory()
        {
            if (!Is_synched_MedicalHistory && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                string tablename = "";
                try
                {
                    Is_synched_MedicalHistory = true;
                    DataSet dsClearDentMedicalHistory = SynchClearDentBAL.GetClearDentMedicalHistoryData();

                    DataTable dtMedicalHistory = new DataTable();
                    string ignoreColumnsList = "", primaryColumnList = "";
                    foreach (DataTable dtTable in dsClearDentMedicalHistory.Tables)
                    {
                        DataTable dtLocalDbRecords = new DataTable();

                        dtTable.Columns.Add("InsUptDlt", typeof(int));
                        dtTable.Columns["InsUptDlt"].DefaultValue = 0;

                        dtLocalDbRecords = SynchLocalBAL.GetLocalMedicalHistoryRecords(dtTable.TableName.ToString(), "1", true);
                        dtMedicalHistory = dtTable;
                        tablename = dtMedicalHistory.TableName.ToString();
                        #region Compare Records

                        if (!dtLocalDbRecords.Columns.Contains("InsUptDlt"))
                        {
                            dtLocalDbRecords.Columns.Add("InsUptDlt", typeof(int));
                            dtLocalDbRecords.Columns["InsUptDlt"].DefaultValue = 0;
                        }

                        if (dtMedicalHistory.TableName.ToString() == "CD_FormMaster")
                        {
                            ignoreColumnsList = "CD_FormMaster_Local_Id,CD_FormMaster_Web_ID";
                            primaryColumnList = "CD_FormMaster_Local_Id";
                            dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "CD_FormMaster_EHR_ID", "CD_FormMaster_Local_Id", "CD_FormMaster_Local_Id,CD_FormMaster_Web_ID,Is_Adit_Updated,Is_deleted,Last_Sync_Date,EHR_Entry_DateTime,InsUptDlt,Clinic_Number,Service_Install_Id");
                        }
                        else if (dtMedicalHistory.TableName.ToString() == "CD_QuestionMaster")
                        {
                            ignoreColumnsList = "CD_QuestionMaster_Local_Id,CD_QuestionMaster_Web_ID";
                            primaryColumnList = "CD_QuestionMaster_Local_Id";
                            dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "CD_QuestionMaster_EHR_ID", "CD_QuestionMaster_Local_Id", "CD_QuestionMaster_Local_Id,CD_QuestionMaster_Web_ID,Is_Adit_Updated,Is_deleted,Last_Sync_Date,EHR_Entry_DateTime,InsUptDlt,Clinic_Number,Service_Install_Id");
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
                    SynchDataLiveDB_Push_MedicalHisotryTables("CD_FormMaster");
                    SynchDataLiveDB_Push_MedicalHisotryTables("CD_QuestionMaster");

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

        #region synch Holidays

        private void fncSynchDataClearDent_Holiday()
        {
            InitBgWorkerClearDent_Holiday();
            InitBgTimerClearDent_Holiday();
        }

        private void InitBgTimerClearDent_Holiday()
        {
            timerSynchClearDent_Holiday = new System.Timers.Timer();
            this.timerSynchClearDent_Holiday.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchClearDent_Holiday.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Holiday_Tick);
            timerSynchClearDent_Holiday.Enabled = true;
            timerSynchClearDent_Holiday.Start();
        }

        private void InitBgWorkerClearDent_Holiday()
        {
            bwSynchClearDent_Holiday = new BackgroundWorker();
            bwSynchClearDent_Holiday.WorkerReportsProgress = true;
            bwSynchClearDent_Holiday.WorkerSupportsCancellation = true;
            bwSynchClearDent_Holiday.DoWork += new DoWorkEventHandler(bwSynchClearDent_Holiday_DoWork);
            bwSynchClearDent_Holiday.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Holiday_RunWorkerCompleted);
        }

        private void timerSynchClearDent_Holiday_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Holiday.Enabled = false;
                MethodForCallSynchOrderClearDent_Holiday();
            }
        }

        public void MethodForCallSynchOrderClearDent_Holiday()
        {
            System.Threading.Thread procThreadmainClearDent_Holiday = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Holiday);
            procThreadmainClearDent_Holiday.Start();
        }

        public void CallSyncOrderTableClearDent_Holiday()
        {
            if (bwSynchClearDent_Holiday.IsBusy != true)
            {
                bwSynchClearDent_Holiday.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Holiday_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Holiday.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_Holiday();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Holiday_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Holiday.Enabled = true;
        }

        public void SynchDataClearDent_Holiday()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtClearDentHoliday = SynchLocalBAL.GetLocalHolidayData("1");

                    dtClearDentHoliday.Columns.Add("InsUptDlt", typeof(int));
                    dtClearDentHoliday.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalHoliday = SynchLocalBAL.GetLocalHolidayData("1");
                    //DataTable dtLocalHoliday = SynchLocalBAL.GetLocalDefaultHolidayData("1");
                    dtClearDentHoliday = CommonUtility.AddHolidays(dtClearDentHoliday, dtLocalHoliday, "SchedDate", "Comment", "H_EHR_ID");
                    //rooja 5-5-23

                    if (!dtLocalHoliday.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalHoliday.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalHoliday.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    foreach (DataRow dtDtxRow in dtClearDentHoliday.Rows)
                    {
                        DataRow[] row = dtLocalHoliday.Copy().Select("SchedDate = '" + Convert.ToDateTime(dtDtxRow["SchedDate"].ToString()) + "'");
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
                            if (dtClearDentHoliday.Columns.Contains("H_Operatory_EHR_ID") && dtClearDentHoliday.Columns["H_Operatory_EHR_ID"] != null && string.IsNullOrEmpty(dtDtxRow["H_Operatory_EHR_ID"].ToString()))
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
                        DataRow[] row = dtClearDentHoliday.Copy().Select("SchedDate = '" + dtDtxRow["SchedDate"] + "'");
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

                    dtClearDentHoliday.AcceptChanges();
                    dtLocalHoliday.AcceptChanges();

                    if ((dtClearDentHoliday != null && dtClearDentHoliday.Rows.Count > 0) || (dtLocalHoliday != null && dtLocalHoliday.Rows.Count > 0))
                    {
                        bool status = false;

                        //=====================existing code for save holidays======================
                        DataTable dtSaveRecords = dtClearDentHoliday.Clone();
                        if (dtClearDentHoliday.Select("InsUptDlt IN (1,2)").Count() > 0 || dtLocalHoliday.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtClearDentHoliday.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtClearDentHoliday.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtLocalHoliday.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtLocalHoliday.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Holiday", "H_LocalDB_ID,H_Web_ID", "H_LocalDB_ID");
                        }
                        else
                        {
                            if (dtClearDentHoliday.Select("InsUptDlt IN (4)").Count() > 0)
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
                        //===================
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }

        #endregion

        #region Synch Insurance

        private void fncSynchDataClearDent_Insurance()
        {
            InitBgWorkerClearDent_Insurance();
            InitBgTimerClearDent_Insurance();
        }

        private void InitBgTimerClearDent_Insurance()
        {
            timerSynchClearDent_Insurance = new System.Timers.Timer();
            this.timerSynchClearDent_Insurance.Interval = 1000 * GoalBase.intervalEHRSynch_Insurance;
            this.timerSynchClearDent_Insurance.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchClearDent_Insurance_Tick);
            timerSynchClearDent_Insurance.Enabled = true;
            timerSynchClearDent_Insurance.Start();
        }

        private void InitBgWorkerClearDent_Insurance()
        {
            bwSynchClearDent_Insurance = new BackgroundWorker();
            bwSynchClearDent_Insurance.WorkerReportsProgress = true;
            bwSynchClearDent_Insurance.WorkerSupportsCancellation = true;
            bwSynchClearDent_Insurance.DoWork += new DoWorkEventHandler(bwSynchClearDent_Insurance_DoWork);
            bwSynchClearDent_Insurance.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchClearDent_Insurance_RunWorkerCompleted);
        }

        private void timerSynchClearDent_Insurance_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchClearDent_Insurance.Enabled = false;
                MethodForCallSynchOrderClearDent_Insurance();
            }
        }

        public void MethodForCallSynchOrderClearDent_Insurance()
        {
            System.Threading.Thread procThreadmainClearDent_Insurance = new System.Threading.Thread(this.CallSyncOrderTableClearDent_Insurance);
            procThreadmainClearDent_Insurance.Start();
        }

        public void CallSyncOrderTableClearDent_Insurance()
        {
            if (bwSynchClearDent_Insurance.IsBusy != true)
            {
                bwSynchClearDent_Insurance.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchClearDent_Insurance_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchClearDent_Insurance.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataClearDent_Insurance();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchClearDent_Insurance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchClearDent_Insurance.Enabled = true;
        }

        public void SynchDataClearDent_Insurance()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtClearDentInsurance = SynchClearDentBAL.GetClearDentInsuranceData();
                    dtClearDentInsurance.Columns.Add("InsUptDlt", typeof(int));

                    dtClearDentInsurance.AcceptChanges();

                    DataTable dtLocalInsurance = SynchLocalBAL.GetLocalInsuranceData("1");

                    foreach (DataRow dtDtxRow in dtClearDentInsurance.Rows)
                    {
                        DataRow[] row = dtLocalInsurance.Copy().Select("Insurance_EHR_ID = '" + dtDtxRow["fld_auto_intCarrId"] + "' ");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["fld_strName"].ToString().Trim() != row[0]["Insurance_Name"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["fld_strAddr"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            //else if (dtDtxRow["address_2"].ToString().ToLower().Trim() != row[0]["Address2"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}
                            //else if (dtDtxRow["city"].ToString().ToLower().Trim() != row[0]["City"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}
                            //else if (dtDtxRow["state"].ToString().ToLower().Trim() != row[0]["State"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}
                            //else if (dtDtxRow["zipcode"].ToString().ToLower().Trim() != row[0]["Zipcode"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}
                            else if (dtDtxRow["fld_strTel"].ToString().ToLower().Trim() != row[0]["Phone"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["fld_intEDICarrIdNo"].ToString().ToLower().Trim() != row[0]["ElectId"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            //else if (dtDtxRow["EmployerName"].ToString().ToLower().Trim() != row[0]["EmployerName"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}
                            //else if (dtDtxRow["status"].ToString().ToLower().Trim() != row[0]["Is_Deleted"].ToString().ToLower().Trim())
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
                        DataRow[] row = dtClearDentInsurance.Copy().Select("fld_auto_intCarrId = '" + dtDtxRow["Insurance_EHR_ID"] + "' ");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtClearDentInsurance.NewRow();
                            BlcOptDtldr["fld_auto_intCarrId"] = dtDtxRow["Insurance_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["fld_strName"] = dtDtxRow["Insurance_Name"].ToString().Trim();                            
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtClearDentInsurance.Rows.Add(BlcOptDtldr);
                        }
                    }

                    dtClearDentInsurance.AcceptChanges();

                    if (dtClearDentInsurance != null && dtClearDentInsurance.Rows.Count > 0)
                    {
                        bool status = SynchClearDentBAL.Save_Insurance_ClearDent_To_Local(dtClearDentInsurance);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Insurance");
                            ObjGoalBase.WriteToSyncLogFile("Insurance Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            SynchDataLiveDB_Push_Insurance();
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

        public void SynchDataCleardent_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchClearDentBAL.GetCleardentMedicationData();
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
        public void SynchCleardent_PatientMedication(string PatientEHRID)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchClearDentBAL.GetCleardentPatientMedicationData(PatientEHRID);
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), PatientEHRID);

                        foreach (DataRow dtDtxRow in dtMedication.Rows)
                        {
                            DataRow[] row = dtLocalMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtDtxRow["PatientMedication_EHR_ID"].ToString().Trim() + "' And Medication_EHR_ID = '" + dtDtxRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"].ToString() + "' And Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
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
                            DataRow[] rowDis = dtMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtLPHRow["PatientMedication_EHR_ID"].ToString() + "' And Medication_EHR_ID = '" + dtLPHRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtLPHRow["Patient_EHR_ID"] + "'");
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

        #region Event Listener
        public static bool SynchDataLocalToClearDent_AppointmentFromEvent(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id)
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

                DataTable dtClearDentPatient = SynchClearDentBAL.GetClearDentPatientID_NameData();
                DataTable dtIdelProv = SynchClearDentBAL.GetClearDentIdelProvider();

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
                    string PatientName = tmpLastName + ", " + tmpFirstName;
                    string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
                    DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                    DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                    TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                    int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);

                    DataTable dtBookOperatoryApptWiseDateTime = SynchClearDentBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
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
                            DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("operatory_id = '" + tmpCheckOpId + "'");
                            if (rowBookOpTime.Length > 0)
                            {
                                for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                {
                                    appointment_EHR_id = rowBookOpTime[Bop]["appointment_id"].ToString();
                                    if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["StartTime"].ToString()))
                                        && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["EndTime"].ToString())))
                                    {
                                        IsConflict = true;
                                        break;
                                    }
                                    if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["StartTime"].ToString()))
                                        && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["EndTime"].ToString())))
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
                        tmpIdealOperatory = Operatory_EHR_IDs[0].ToString();
                    }
                    if (tmpIdealOperatory == "")
                    {
                        DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("appointment_id = " + appointment_EHR_id).CopyToDataTable();
                        bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());

                    }
                    else
                    {

                        tmpPatient_id = 0;
                        tmpPatient_Gur_id = 0;
                        tmpAppt_EHR_id = 0;
                        tmpNewPatient = 1;
                        TmpWebPatientName = "";
                        TmpWebRevPatientName = "";

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
                        if (dtDtxRow["Patient_EHR_id"].ToString() != "-")
                        {
                            tmpPatient_id = Convert.ToInt64(dtDtxRow["Patient_EHR_id"].ToString());
                        }
                        else
                        {
                            tmpPatient_id = 0;
                        }

                        if (tmpPatient_id == 0)
                        {
                            tmpPatient_id = Convert.ToInt64(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtClearDentPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DBConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                        }

                        if (tmpPatient_id > 0)
                        {

                            tmpAppt_EHR_id = SynchClearDentBAL.Save_Appointment_Local_To_ClearDent(tmpPatient_id.ToString(), tmpApptDurMinutes, tmpIdealOperatory.ToString(), tmpApptProv,
                               tmpStartTime, tmpEndTime, dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDtxRow["ApptType_EHR_ID"].ToString().Trim(), PatientName, dtDtxRow["comment"].ToString().Trim(), dtDtxRow["appt_treatmentcode"].ToString().Trim(), Convert.ToInt16((dtDtxRow["appointment_status_ehr_key"].ToString() == "" || dtDtxRow["appointment_status_ehr_key"].ToString() == "0") ? 1 : dtDtxRow["appointment_status_ehr_key"]));
                            if (tmpAppt_EHR_id > 0)
                            {
                                bool isApptId_Update = SynchClearDentBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                            }
                        }

                        #region Sync Appointment
                        SyncDataClearDent_AppointmentFromEvent(strDbConnString, Clinic_Number, Service_Install_Id, tmpAppt_EHR_id.ToString(), tmpPatient_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                        #endregion
                    }
                }
                Utility.WritetoAditEventErrorLogFile_Static("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                return false;
            }
        }

        public static void SyncDataClearDent_AppointmentFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        {
            try
            {
                SynchDataClearDent_AppointmentsPatientFromEvents(strDbString, Clinic_Number, Service_Install_Id, strPatID);
                SynchDataClearDent_PatientStatusFromEvent(strDbString, Clinic_Number, Service_Install_Id, strPatID);
                SynchDataLiveDB_Push_Patient(strPatID);

                DataTable dtClearDentAppointment = SynchClearDentBAL.GetClearDentAppointmentData(strApptID);
                dtClearDentAppointment.Columns.Add("Provider_EHR_ID", typeof(string));
                dtClearDentAppointment.Columns.Add("ProviderName", typeof(string));
                dtClearDentAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                dtClearDentAppointment.Columns.Add("InsUptDlt", typeof(int));
                dtClearDentAppointment.Columns.Add("ProcedureDesc", typeof(string));
                dtClearDentAppointment.Columns.Add("ProcedureCode", typeof(string));

                DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Service_Install_Id, strApptID);

                DataTable dtClearDentOperatory = SynchClearDentBAL.GetClearDentOperatoryData();

                string ProcedureDesc = "";
                string ProcedureCode = "";
                DataTable DtClearDentAppointment_Procedures_Data = SynchClearDentBAL.GetClearDentAppointment_Procedures_Data(strApptID);

                string MobileEmail = string.Empty;
                string Mobile_Contact = string.Empty;
                string Email = string.Empty;
                int cntCurRecord = 0;
                foreach (DataRow dtDtxRow in dtClearDentAppointment.Rows)
                {
                    string ProvIDList = string.Empty;
                    string ProvNameList = string.Empty;

                    ProvIDList = ProvIDList + dtDtxRow["provider_id1"].ToString() + ";";
                    ProvNameList = ProvNameList + dtDtxRow["ProviderName1"].ToString() + ";";
                    if (!string.IsNullOrEmpty(dtDtxRow["provider_id2"].ToString()))
                    {
                        ProvIDList = ProvIDList + dtDtxRow["provider_id2"].ToString() + ";";
                        ProvNameList = ProvNameList + dtDtxRow["ProviderName2"].ToString() + ";";
                    }
                    if (ProvIDList.Length > 0)
                    {
                        ProvIDList = ProvIDList.Substring(0, ProvIDList.Length - 1);
                    }
                    if (ProvNameList.Length > 0)
                    {
                        ProvNameList = ProvNameList.Substring(0, ProvNameList.Length - 1);
                    }
                    cntCurRecord = cntCurRecord + 1;
                    dtDtxRow["Provider_EHR_ID"] = ProvIDList;
                    dtDtxRow["ProviderName"] = ProvNameList;

                    ///////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                    ProcedureDesc = "";
                    ProcedureCode = "";

                    DataRow[] dtCurApptProcedure = DtClearDentAppointment_Procedures_Data.Select("appointment_id = '" + dtDtxRow["appointment_id"].ToString().Trim() + "'");
                    foreach (var dtSinProc in dtCurApptProcedure.ToList())
                    {
                        ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim();
                    }

                    dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                    dtDtxRow["ProcedureCode"] = ProcedureCode;
                    /////////////////////////////////

                    DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appointment_id"].ToString().Trim() + "' ");
                    if (row.Length > 0)
                    {
                        // dtDtxRow["InsUptDlt"] = "UID";                        
                    }
                    else
                    {
                        DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Mobile_Contact + "'  AND ISNULL(Appt_EHR_ID,0) = 0 ");
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

                    if (row.Length > 0)
                    {
                        int commentlen = 1999;
                        if (dtDtxRow["comment"].ToString().Trim().Length < commentlen)
                        {
                            commentlen = dtDtxRow["comment"].ToString().Trim().Length;
                        }
                        if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        if (dtDtxRow["patient_ehr_id"].ToString().Trim() != row[0]["patient_ehr_id"].ToString().Trim())
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (dtDtxRow["Provider_EHR_ID"].ToString().Trim() != row[0]["Provider_EHR_ID"].ToString().Trim())
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (dtDtxRow["comment"].ToString().ToLower().Trim().Substring(0, commentlen) != row[0]["comment"].ToString().ToLower().Trim())
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (Convert.ToBoolean(row[0]["is_deleted"]) != false)
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (Convert.ToBoolean(dtDtxRow["is_asap"]) != Convert.ToBoolean(row[0]["is_asap"]))
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
                        else if (row[0]["appointment_status_ehr_key"].ToString().Trim() == "99" && ((dtDtxRow["Confirmed"].ToString().ToLower() == "false") || dtDtxRow["appointment_status_ehr_key"].ToString() == "7"))
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (row[0]["appointment_status_ehr_key"].ToString().Trim() == "1" && ((dtDtxRow["Confirmed"].ToString().ToLower() == "true") || dtDtxRow["appointment_status_ehr_key"].ToString() == "7"))//nidhi task
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (dtDtxRow["appointment_status_ehr_key"].ToString() != "7" && dtDtxRow["Confirmed"].ToString().ToLower() == "true" && row[0]["appointment_status_ehr_key"].ToString().Trim() != "99")
                        {
                            dtDtxRow["appointment_status_ehr_key"] = 99;
                            dtDtxRow["Appointment_Status"] = "Confirmed";
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (dtDtxRow["ProcedureDesc"].ToString().Trim() != row[0]["ProcedureDesc"].ToString().Trim())
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (dtDtxRow["ProcedureCode"].ToString().Trim() != row[0]["ProcedureCode"].ToString().Trim())
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (dtDtxRow["appointment_status_ehr_key"].ToString() != "7" && dtDtxRow["Confirmed"].ToString().ToLower() != "true" && row[0]["appointment_status_ehr_key"].ToString().Trim() != "99")
                        {
                            if ((dtDtxRow["appointment_status_ehr_key"].ToString().Trim() != row[0]["appointment_status_ehr_key"].ToString().Trim()))
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



                dtClearDentAppointment.AcceptChanges();

                if (dtClearDentAppointment != null && dtClearDentAppointment.Rows.Count > 0)
                {
                    if (!dtClearDentAppointment.Columns.Contains("Appt_Web_ID"))
                    {
                        dtClearDentAppointment.Columns.Add("Appt_Web_ID");
                    }
                    dtClearDentAppointment.Rows[0]["Appt_Web_ID"] = strWebID;

                    bool status = SynchClearDentBAL.Save_Appointment_ClearDent_To_Local(dtClearDentAppointment);

                    if (status)
                    {
                        Utility.WritetoAditEventSyncLogFile_Static("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchDataLiveDB_Push_Appointment(strApptID);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncDataClearDent_AppointmentFromEvent Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public static void SynchDataClearDent_AppointmentsPatientFromEvents(string strDbString, string Clinic_Number, string Service_Install_Id, string strPatID)
        {
            try
            {
                DataTable dtClearDentPatient = SynchClearDentBAL.GetClearDentAppointmentsPatientData(strPatID);
                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Service_Install_Id, strPatID);

                DataTable dtSaveRecords = new DataTable();
                dtSaveRecords = dtLocalPatient.Clone();

                var itemsToBeAdded = (from ClearDentPatient in dtClearDentPatient.AsEnumerable()
                                      join LocalPatient in dtLocalPatient.AsEnumerable()
                                      on ClearDentPatient["Patient_EHR_ID"].ToString().Trim() + "_" + ClearDentPatient["Clinic_Number"].ToString().Trim()
                                      equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                      //on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                      //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                      into matchingRows
                                      from matchingRow in matchingRows.DefaultIfEmpty()
                                      where matchingRow == null
                                      select ClearDentPatient).ToList();
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

                var itemsToBeUpdated = (from ClearDentPatient in dtClearDentPatient.AsEnumerable()
                                        join LocalPatient in dtLocalPatient.AsEnumerable()
                                        on ClearDentPatient["Patient_EHR_ID"].ToString().Trim() + "_" + ClearDentPatient["Clinic_Number"].ToString().Trim()
                                        equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                        where
                                         Convert.ToDateTime(ClearDentPatient["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(LocalPatient["EHR_Entry_DateTime"])
                                         ||
                                         (ClearDentPatient["nextvisit_date"] != DBNull.Value && ClearDentPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(ClearDentPatient["nextvisit_date"]) : DateTime.Now)
                                         !=
                                         (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)
                                         ||
                                         (ClearDentPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())
                                         ||
                                         (ClearDentPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())
                                         || (ClearDentPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                         || (ClearDentPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                         || (ClearDentPatient["Home_Phone"].ToString().Trim()) != (LocalPatient["Home_Phone"].ToString().Trim())
                                         || (ClearDentPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                         || (ClearDentPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                         || (ClearDentPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                         || (ClearDentPatient["Mobile"].ToString().Trim()) != (LocalPatient["Mobile"].ToString().Trim())
                                         || (ClearDentPatient["ReceiveSMS"].ToString().Trim()) != (LocalPatient["ReceiveSMS"].ToString().Trim())
                                         || (ClearDentPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
                                        select ClearDentPatient).ToList();

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
                    bool status = SynchClearDentBAL.Save_Patient_Cleardent_To_Local_New(dtSaveRecords, Clinic_Number, Service_Install_Id);
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataClearDent_AppointmentsPatientFromEvents Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public static void SynchDataClearDent_PatientStatusFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strPatID)
        {
            try
            {
                DataTable dtClearDentPatientStatus = SynchClearDentBAL.GetClearDentPatientStatusData(strPatID);
                if (dtClearDentPatientStatus != null && dtClearDentPatientStatus.Rows.Count > 0)
                {
                    SynchLocalBAL.UpdatePatient_Status(dtClearDentPatientStatus, Service_Install_Id, Clinic_Number, strPatID);
                    //SynchDataLiveDB_Push_PatientStatus(Convert.ToInt32(Service_Install_Id), Convert.ToInt32(Clinic_Number), strPatID);
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataEagleSoft_PatientStatus Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public void SynchDataLocalToClearDent_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
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

                foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                {

                    if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "first_name")
                    {
                        dtDtxRow["ehrfield"] = "fld_strFName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "last_name")
                    {
                        dtDtxRow["ehrfield"] = "fld_strLName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "mobile")
                    {
                        dtDtxRow["ehrfield"] = "fld_strMTel";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "birth_date")
                    {
                        dtDtxRow["ehrfield"] = "fld_dtmBth";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "email")
                    {
                        dtDtxRow["ehrfield"] = "fld_strEmail";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "middle_name")
                    {
                        dtDtxRow["ehrfield"] = "fld_strMIni";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "preferred_name")
                    {
                        dtDtxRow["ehrfield"] = "fld_strPName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sex")
                    {
                        dtDtxRow["ehrfield"] = "fld_strSex";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MALE")
                        {
                            dtDtxRow["ehrfield_value"] = "M";
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                        {
                            dtDtxRow["ehrfield_value"] = "F";
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = DBNull.Value;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "status")
                    {
                        dtDtxRow["ehrfield"] = "fld_bytStatus";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "INACTIVE")
                        {
                            dtDtxRow["ehrfield_value"] = "0";
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "ACTIVE")
                        {
                            dtDtxRow["ehrfield_value"] = "1";
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = "1";

                        }
                    }

                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "work_phone")
                    {
                        dtDtxRow["ehrfield"] = "fld_strOTel";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "zipcode")
                    {
                        dtDtxRow["ehrfield"] = "fld_strPCode";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_one")
                    {
                        dtDtxRow["ehrfield"] = "fld_strAddr1";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_two")
                    {
                        dtDtxRow["ehrfield"] = "fld_strAddr2";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "city")
                    {
                        dtDtxRow["ehrfield"] = "fld_strCity";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "state")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "home_phone")
                    {
                        dtDtxRow["ehrfield"] = "fld_strHTel";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "pri_provider_id")
                    {
                        dtDtxRow["ehrfield"] = "fld_shtPrId";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sec_provider_id")
                    {
                        dtDtxRow["ehrfield"] = "fld_shtHyId";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "salutation")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance")
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

                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "receive_email")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "receive_sms")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance")
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
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "ssn")
                    {
                        dtDtxRow["ehrfield"] = "fld_intSIN";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "emergencycontactname")
                    {
                        dtDtxRow["ehrfield"] = "fld_strEmergencyName";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "emergencycontactnumber")
                    {
                        dtDtxRow["ehrfield"] = "fld_strEmergencyContact";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "school")
                    {
                        dtDtxRow["ehrfield"] = "fld_strSchool";
                    }



                    dtWebPatient_Form.AcceptChanges();

                }

                if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                {
                    Utility.CheckEntryUserLoginIdExist();
                    bool Is_Record_Update = SynchClearDentBAL.Save_Patient_Form_Local_To_ClearDent(dtWebPatient_Form);
                }

                GetMedicalClearDentHistoryRecords(strPatientFormID);

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
                bool isDeletedMedication = false;
                bool isSaveMedication = false;
                string Patient_EHR_IDS = "";
                string DeletePatientEHRID = "";
                string SavePatientEHRID = "";
                try
                {
                    SynchClearDentBAL.DeletePatientMedicationLocalToClearDent("1", ref isDeletedMedication, ref DeletePatientEHRID, strPatientFormID);
                }
                catch (Exception exMed)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + exMed.Message);
                }
                try
                {
                    SynchClearDentBAL.SavePatientMedicationLocalToClearDent("1", ref isSaveMedication, ref SavePatientEHRID, strPatientFormID);
                }
                catch (Exception exMed)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + exMed.Message);
                }
                if (isDeletedMedication || isSaveMedication)
                {
                    Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                    if (Patient_EHR_IDS != "")
                    {
                        SynchCleardent_PatientMedication(Patient_EHR_IDS);
                    }
                }

                try
                {
                    SynchClearDentBAL.SaveMedicalHistoryLocalToClearDent(strPatientFormID);
                }
                catch (Exception ex)
                {
                    IsDocumentUpload = false;
                    ObjGoalBase.WriteToErrorLogFile("[Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }

                #region PatientInformation Document
                try
                {
                    GetPatientDocument("1", strPatientFormID);
                    GetPatientDocument_New("1", strPatientFormID);
                    SynchClearDentBAL.Save_Document_in_ClearDent(strPatientFormID);                    
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form Document Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
                #endregion

                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                throw ex;
            }
        }
        #endregion


    }
}
