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


        private BackgroundWorker bwSynchEaglesoft_Appointment = null;
        private System.Timers.Timer timerSynchEaglesoft_Appointment = null;

        private BackgroundWorker bwSynchEaglesoft_OperatoryEvent = null;
        private System.Timers.Timer timerSynchEaglesoft_OperatoryEvent = null;

        private BackgroundWorker bwSynchEaglesoft_Provider = null;
        private System.Timers.Timer timerSynchEaglesoft_Provider = null;

        private BackgroundWorker bwSynchEaglesoft_PatientPayment = null;
        private System.Timers.Timer timerSynchEaglesoft_PatientPayment = null;

        private BackgroundWorker bwSynchEaglesoft_Speciality = null;
        private System.Timers.Timer timerSynchEaglesoft_Speciality = null;

        private BackgroundWorker bwSynchEaglesoft_Operatory = null;
        private System.Timers.Timer timerSynchEaglesoft_Operatory = null;

        private BackgroundWorker bwSynchEaglesoft_OperatoryHours = null;
        private System.Timers.Timer timerSynchEaglesoft_OperatoryHours = null;

        private BackgroundWorker bwSynchEaglesoft_ApptType = null;
        private System.Timers.Timer timerSynchEaglesoft_ApptType = null;

        private BackgroundWorker bwSynchEaglesoft_Patient = null;
        private System.Timers.Timer timerSynchEaglesoft_Patient = null;

        private BackgroundWorker bwSynchEaglesoft_RecallType = null;
        private System.Timers.Timer timerSynchEaglesoft_RecallType = null;

        private BackgroundWorker bwSynchEagleSoft_User = null;
        private System.Timers.Timer timerSynchEagleSoft_User = null;

        private BackgroundWorker bwSynchEaglesoft_ApptStatus = null;
        private System.Timers.Timer timerSynchEaglesoft_ApptStatus = null;

        private BackgroundWorker bwSynchEaglesoft_Holidays = null;
        private System.Timers.Timer timerSynchEaglesoft_Holidays = null;

        private BackgroundWorker bwSynchEaglesoft_MedicalHistory = null;
        private System.Timers.Timer timerSynchEaglesoft_MedicalHistory = null;

        private BackgroundWorker bwSynchLocalToEagleSoft_Appointment = null;
        private System.Timers.Timer timerSynchLocalToEagleSoft_Appointment = null;

        private BackgroundWorker bwSynchLocalToEagleSoft_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToEagleSoft_Patient_Form = null;

        private BackgroundWorker bwSynchEagleSoft_Disease = null;
        private System.Timers.Timer timerSynchEagleSoft_Disease = null;

        private BackgroundWorker bwSynchEagleSoftToLocal_ProviderOfficeHours = null;
        private System.Timers.Timer timerSynchEagleSoftToLocal_ProviderOfficeHours = null;

        private BackgroundWorker bwSynchEagleSoftToLocalPatientWiseRecallType = null;
        private System.Timers.Timer timerSynchEagleSoftToLocalPatientWiseRecallType = null;

        private BackgroundWorker bwSynchEagleSoft_Insurance = null;
        private System.Timers.Timer timerSynchEagleSoft_Insurance = null;

        #endregion

        private void CallSynchEaglesoftToLocal()
        {
            if (Utility.AditSync)
            {
                //SynchDataLocalToEagleSoft_Patient_Payment();

                //try
                //{
                //    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                //    {
                //        string qry;
                //        for (int i = 0; i < 10; i++)
                //        {
                //            DataTable CustPrompt = SynchEaglesoftBAL.GetEagleSoftCustPrompts(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                //            System.Text.StringBuilder b = new System.Text.StringBuilder();
                //            foreach (System.Data.DataRow r in CustPrompt.Rows)
                //            {
                //                foreach (System.Data.DataColumn c in CustPrompt.Columns)
                //                {
                //                    b.Append(c.ColumnName.ToString() + ":" + r[c.ColumnName].ToString() + " | ");
                //                }
                //                b.Append(Environment.NewLine);
                //            }
                //            MessageBox.Show(b.ToString());
                //            qry = Clipboard.GetText();
                //            if (qry.ToLower().Trim() == "exit") continue;
                //            if (qry.ToUpper().IndexOf("UPDATE") >= 0)
                //            {
                //                using (System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString()))
                //                {
                //                    using (System.Data.Odbc.OdbcCommand OdbcCommand = new System.Data.Odbc.OdbcCommand(qry, conn))
                //                    {
                //                        OdbcCommand.CommandTimeout = 200;
                //                        OdbcCommand.CommandType = CommandType.Text;
                //                        if (conn.State == ConnectionState.Closed) conn.Open();
                //                        OdbcCommand.ExecuteNonQuery();
                //                    }
                //                }

                //            }
                //        }
                //    }
                //}
                //catch (Exception x)
                //{
                //    MessageBox.Show(x.Message);
                //    throw;
                //}
                CallSynch_Provider();
                fncSynchDataEaglesoft_Provider();

                fncSynchDataEaglesoft_PatientPayment();

                SynchDataLocalToEagleSoft_Patient_Form();
                fncSynchDataLocalToEagleSoft_Patient_Form();

                SynchDataEagleSoft_Disease();
                //SynchDataEagleSoft_PatientDisease();

                fncSynchDataEagleSoft_Disease();

                CallSynch_Operatory();
                fncSynchDataEaglesoft_Operatory();



                CallSynch_Speciality();
                fncSynchDataEaglesoft_Speciality();

                //CallSynch_OperatoryEvent();
                fncSynchDataEaglesoft_OperatoryEvent();

                CallSynch_Type();
                fncSynchDataEaglesoft_ApptType();

                //CallSynch_Appointment();
                // fncSynchDataEaglesoft_Appointment();

                //CallSynch_Patient();
                //fncSynchDataEaglesoft_Patient();

                SynchDataEaglesoft_RecallType();
                fncSynchDataEaglesoft_RecallType();

                SynchDataEagleSoft_User();
                fncSynchDataEagleSoft_User();

                // SynchDataEaglesoftPatientWiseRecallType();
                fncSynchDataEaglesoft_PatientWiseRecallType();

                SynchDataEaglesoft_ApptStatus();
                fncSynchDataEaglesoft_ApptStatus();

                SynchDataEaglesoft_Holidays();
                fncSynchDataEaglesoft_Holidays();

                ///////////////////////////////////////////

                //CallSynch_Patient();
                //fncSynchDataEaglesoft_Patient();

                //SynchDataEagleSoftToLocal_ProviderOfficeHours();
                fncSynchDataEagleSoftToLocal_ProviderOfficeHours();

                fncSynchDataEaglesoft_OperatoryHours();

                fncSynchDataLocalToEagleSoft_Appointment();

                //Skipped Application.DoEvents(); by conditioning AppoitmentLocationID, for client Vacek Family Dentistry.
                if (Utility.Loc_ID != "acfd6257-cac2-4e5f-8730-320970fcda11")
                {
                    Application.DoEvents();
                }

                //CallSynch_Eaglesoft_MedicalHistory();
                fncSynchDataEaglesoft_MedicalHistory();

                //rooja 23-4-24 task link EHR Updates for RCM - https://app.asana.com/0/1199184824722493/1207061756651636/f
                SynchDataEaglesoft_Insurance();
                fncSynchDataEaglesoft_Insurance();
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
            bwSynchEaglesoft_Appointment = new BackgroundWorker();
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
                //SynchDataLocalToEagleSoft_Patient_Payment();
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
            try
            {
                CallSynch_Appointment_New();
            }
            catch (Exception exAp)
            {
                try
                {
                    CallSynch_Appointment();
                }
                catch
                {
                    Is_synched_Appointment = true;
                }
            }
        }

        public void CallSynch_Appointment()
        {
            try
            {
                if (Utility.IsExternalAppointmentSync)
                {
                    IsEaglesoftProviderSync = true;
                    IsEaglesoftOperatorySync = true;
                    IsEaglesoftApptTypeSync = true;
                    Is_synched_Appointment = true;
                }
                if (IsEaglesoftProviderSync && IsEaglesoftOperatorySync && IsEaglesoftApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff)
                {
                    try
                    {
                        Is_synched_Appointment = false;
                        //DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData();
                        //DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData();

                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        //New line by yogesh for sync appointments patients first
                        try
                        {
                            CallSynch_AppointmentsPatient_New();
                        }
                        catch (Exception exPat)
                        {
                            CallSynch_AppointmentsPatient();
                            Is_synched_Appointment = true;
                        }

                        try
                        {
                            if (IsParientFirstSync)
                            {
                                SynchDataEagleSoft_InsertPatient();
                            }
                        }
                        catch (Exception exPat)
                        {
                            Is_synched_Appointment = true;
                            ObjGoalBase.WriteToErrorLogFile("[CallSynch_Appointment]->[SynchDataEagleSoft_InsertPatient] Error: " + exPat.Message);
                        }

                        try
                        {
                            SynchDataEagleSoft_PatientStatus();
                        }
                        catch (Exception exPatStat)
                        {
                            Is_synched_Appointment = true;
                            ObjGoalBase.WriteToErrorLogFile("[CallSynch_Appointment]->[SynchDataEagleSoft_PatientStatus] Error: " + exPatStat.Message);
                        }


                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            try
                            {
                                DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                                if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                                {
                                    DataTable dtEaglesoftAppointment = SynchEaglesoftBAL.GetEaglesoftAppointmentData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    dtEaglesoftAppointment.Columns.Add("Provider_EHR_ID", typeof(string));
                                    dtEaglesoftAppointment.Columns.Add("ProviderName", typeof(string));
                                    dtEaglesoftAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                                    dtEaglesoftAppointment.Columns.Add("ProcedureDesc", typeof(string));
                                    dtEaglesoftAppointment.Columns.Add("ProcedureCode", typeof(string));
                                    dtEaglesoftAppointment.Columns.Add("InsUptDlt", typeof(int));

                                    string ProcedureDesc = "";
                                    string ProcedureCode = "";

                                    DataTable DtEaglesoftAppointment_Procedures_Data = SynchEaglesoftBAL.GetEaglesoftAppointment_Procedures_Data(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                                    #region If
                                    DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                    int cntCurRecord = 0;

                                    foreach (DataRow dtDtxRow in dtEaglesoftAppointment.Rows)
                                    {
                                        DataTable dtEaglesoftAppointmentProider = SynchEaglesoftBAL.GetEaglesoftAppointmentProviderData(dtDtxRow["Appt_EHR_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                        string ProvIDList = string.Empty;
                                        string ProvNameList = string.Empty;
                                        foreach (DataRow dtr in dtEaglesoftAppointmentProider.Rows)
                                        {
                                            ProvIDList = ProvIDList + dtr["provider_id"].ToString() + " ; ";
                                            ProvNameList = ProvNameList + dtr["ProviderName"].ToString() + " ; ";
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
                                        dtDtxRow["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString();

                                        ////////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                                        ProcedureDesc = "";
                                        ProcedureCode = "";

                                        DataRow[] dtCurApptProcedure = DtEaglesoftAppointment_Procedures_Data.Select("appointment_id = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");

                                        foreach (var dtSinProc in dtCurApptProcedure.ToList())
                                        {
                                            ProcedureDesc = ProcedureDesc + dtSinProc["tooth"].ToString() + dtSinProc["surface"].ToString() + dtSinProc["schedabbr"].ToString();
                                            ProcedureCode = ProcedureCode + dtSinProc["ADA_Code"].ToString();
                                        }
                                        if (ProcedureDesc.ToString().Length > 1)
                                        {
                                            ProcedureDesc = ProcedureDesc.Substring(0, ProcedureDesc.Length - 1);
                                            dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                                        }
                                        if (ProcedureCode.ToString().Length > 1)
                                        {
                                            ProcedureCode = ProcedureCode.Substring(0, ProcedureCode.Length - 1);
                                            dtDtxRow["ProcedureCode"] = ProcedureCode;
                                        }

                                        //////////////////////////////////

                                        DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                                        if (row.Length > 0)
                                        {
                                            int commentlen = 1999;
                                            if (dtDtxRow["comment"].ToString().Trim().Length < commentlen)
                                            {
                                                commentlen = dtDtxRow["comment"].ToString().Trim().Length;
                                            }
                                            if (dtDtxRow["Last_Name"].ToString().Trim() != row[0]["Last_Name"].ToString().Trim())
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
                                                if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() == "-1" || dtDtxRow["Operatory_EHR_ID"].ToString().Trim() == "" || dtDtxRow["classification"].ToString().Trim() == "16" || dtDtxRow["classification"].ToString().Trim() == "32" || dtDtxRow["classification"].ToString().Trim() == "64")
                                                {
                                                    if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                                    {
                                                        dtDtxRow["InsUptDlt"] = 3;
                                                    }
                                                }
                                                else
                                                {
                                                    dtDtxRow["InsUptDlt"] = 4;
                                                }
                                            }
                                            else if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == true && dtDtxRow["classification"].ToString().Trim() == "1")
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
                                            else if (dtDtxRow["ApptType_EHR_ID"].ToString().Trim() != row[0]["ApptType_EHR_ID"].ToString().Trim())
                                            {
                                                dtDtxRow["InsUptDlt"] = 4;
                                            }
                                            else if (Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Appt_DateTime"].ToString().Trim()))
                                            {
                                                dtDtxRow["InsUptDlt"] = 4;
                                            }
                                            else if (Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Appt_EndDateTime"].ToString().Trim()))
                                            {
                                                dtDtxRow["InsUptDlt"] = 4;
                                            }
                                            else if (!string.IsNullOrEmpty(dtDtxRow["Birth_date"].ToString()) && string.IsNullOrEmpty(row[0]["Birth_date"].ToString()))
                                            {
                                                dtDtxRow["InsUptDlt"] = 4;
                                            }
                                            else if (string.IsNullOrEmpty(dtDtxRow["Birth_date"].ToString()) && !string.IsNullOrEmpty(row[0]["Birth_date"].ToString()))
                                            {
                                                dtDtxRow["InsUptDlt"] = 4;
                                            }
                                            else if (!string.IsNullOrEmpty(dtDtxRow["Birth_date"].ToString()) && !string.IsNullOrEmpty(row[0]["Birth_date"].ToString()) && (Convert.ToDateTime(dtDtxRow["Birth_date"].ToString().Trim()) != Convert.ToDateTime(row[0]["Birth_date"].ToString().Trim())))
                                            {
                                                dtDtxRow["InsUptDlt"] = 4;
                                            }
                                            else if (dtDtxRow["appointment_status_ehr_key"].ToString().Trim() != row[0]["appointment_status_ehr_key"].ToString().Trim())
                                            {
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
                                            else if (dtDtxRow["is_asap"].ToString().Trim() != row[0]["is_asap"].ToString().Trim())
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
                                            //rooja 28-4-23- https://app.asana.com/0/0/1204426000385832/f - //condition for ' '  or null cell_phone in ehr
                                            try
                                            {
                                                DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) + "' AND ISNULL(Appt_EHR_ID,'0') ='0'");

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
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    foreach (DataRow dtlApptRow in dtLocalAppointment.Rows)
                                    {
                                        try
                                        {
                                            DataRow[] rowBlcOpt = dtEaglesoftAppointment.Copy().Select("Appt_EHR_ID = '" + dtlApptRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                                            if (rowBlcOpt.Length > 0)
                                            { }
                                            else
                                            {
                                                if (Convert.ToBoolean(dtlApptRow["is_deleted"].ToString().Trim()) == false && (dtlApptRow["Appt_EHR_ID"].ToString().Trim()) != "0")
                                                {
                                                    DataRow ESApptDtldr = dtEaglesoftAppointment.NewRow();
                                                    ESApptDtldr["Appt_EHR_ID"] = dtlApptRow["Appt_EHR_ID"].ToString().Trim();
                                                    ESApptDtldr["InsUptDlt"] = 3;
                                                    dtEaglesoftAppointment.Rows.Add(ESApptDtldr);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("CallSynch_Appointment - Appt_EHR_ID error : " + ex.Message.ToString());
                                            ObjGoalBase.WriteToErrorLogFile("Appt_EHR_ID : " + dtlApptRow["Appt_EHR_ID"].ToString().Trim());
                                        }
                                    }
                                    dtEaglesoftAppointment.AcceptChanges();

                                    if (dtEaglesoftAppointment != null && dtEaglesoftAppointment.Rows.Count > 0)
                                    {
                                        bool status = SynchEaglesoftBAL.Save_Appointment_Eaglesoft_To_Local(dtEaglesoftAppointment, (Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()));

                                        if (status)
                                        {
                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                                            ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                                            SynchDataLiveDB_Push_Appointment();
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                        }
                                    }

                                    Is_synched_Appointment = true;

                                    #endregion
                                    IsEHRAllSync = true;
                                    Is_synched_Appointment = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                Is_synched_Appointment = true;
                                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                            }
                            finally
                            {
                                Is_synched_Appointment = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Is_synched_Appointment = true;
                        ObjGoalBase.WriteToErrorLogFile("[Appointment Sync Diff (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    }
                    finally
                    {
                        Is_synched_Appointment = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Appointment Sync Main (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        public void CallSynch_Appointment_New()
        {
            try
            {
                if (Utility.IsExternalAppointmentSync)
                {
                    IsEaglesoftProviderSync = true;
                    IsEaglesoftOperatorySync = true;
                    IsEaglesoftApptTypeSync = true;
                    Is_synched_Appointment = true;
                }

                if (IsEaglesoftProviderSync && IsEaglesoftOperatorySync && IsEaglesoftApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff)
                {
                    try
                    {
                        Utility.isDebugLogRequired = false;
                        try
                        {
                            Utility.isDebugLogRequired = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("isDebugLogRequired").ToString());
                        }
                        catch (Exception ex)
                        {
                            Utility.isDebugLogRequired = false;
                        }

                        //Utility.WriteToDebugSyncLogFile_All("1 Start.", "CallSynch_Appointment_New");
                        Is_synched_Appointment = false;
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        //Utility.WriteToDebugSyncLogFile_All("2 Start CallSynch_AppointmentsPatient_New.", "CallSynch_Appointment_New");
                        try
                        {
                            CallSynch_AppointmentsPatient_New();
                        }
                        catch (Exception ex)
                        {
                            Is_synched_Appointment = true;
                        }

                        // Utility.WriteToDebugSyncLogFile_All("3 End CallSynch_AppointmentsPatient_New.", "CallSynch_Appointment_New");
                        try
                        {
                            if (IsParientFirstSync)
                            {
                                //Utility.WriteToDebugSyncLogFile_All("3.1 Start SynchDataEagleSoft_InsertPatient.", "CallSynch_Appointment_New");
                                SynchDataEagleSoft_InsertPatient();
                                //Utility.WriteToDebugSyncLogFile_All("3.2 End SynchDataEagleSoft_InsertPatient.", "CallSynch_Appointment_New");
                            }
                        }
                        catch (Exception exPat)
                        {
                            Is_synched_Appointment = true;
                            //Utility.WriteToDebugSyncLogFile_All("3.3 Error in SynchDataEagleSoft_InsertPatient, Message:" + exPat.Message, "CallSynch_Appointment_New");
                            ObjGoalBase.WriteToErrorLogFile("[CallSynch_Appointment_New]->[SynchDataEagleSoft_InsertPatient] Error: " + exPat.Message);
                        }

                        try
                        {
                            //Utility.WriteToDebugSyncLogFile_All("4 Start SynchDataEagleSoft_PatientStatus.", "CallSynch_Appointment_New");
                            SynchDataEagleSoft_PatientStatus();
                            //Utility.WriteToDebugSyncLogFile_All("5 End SynchDataEagleSoft_PatientStatus.", "CallSynch_Appointment_New");
                        }
                        catch (Exception exPatStat)
                        {
                            Is_synched_Appointment = true;
                            ObjGoalBase.WriteToErrorLogFile("[CallSynch_Appointment_New]->[SynchDataEagleSoft_PatientStatus] Error: " + exPatStat.Message);
                        }


                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            try
                            {
                                //Utility.WriteToDebugSyncLogFile_All("6 Appointment Sync Start.", "CallSynch_Appointment_New");
                                DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                                // Utility.WriteToDebugSyncLogFile_All("7 AditLocationSyncEnable : " + Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()), "CallSynch_Appointment_New");
                                if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                                {
                                    // Utility.WriteToDebugSyncLogFile_All("8 GetEaglesoftAppointmentData Start.", "CallSynch_Appointment_New");
                                    DataTable dtEaglesoftAppointment = SynchEaglesoftBAL.GetEaglesoftAppointmentData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    // Utility.WriteToDebugSyncLogFile_All("9 GetEaglesoftAppointmentData End. Count: " + dtEaglesoftAppointment.Rows.Count, "CallSynch_Appointment_New");
                                    dtEaglesoftAppointment.Columns.Add("Provider_EHR_ID", typeof(string));
                                    dtEaglesoftAppointment.Columns.Add("ProviderName", typeof(string));
                                    dtEaglesoftAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                                    dtEaglesoftAppointment.Columns.Add("ProcedureDesc", typeof(string));
                                    dtEaglesoftAppointment.Columns.Add("ProcedureCode", typeof(string));
                                    dtEaglesoftAppointment.Columns.Add("InsUptDlt", typeof(int));

                                    string ProcedureDesc = "";
                                    string ProcedureCode = "";
                                    //Utility.WriteToDebugSyncLogFile_All("10 DtEaglesoftAppointment_Procedures_Data Start.", "CallSynch_Appointment_New");
                                    DataTable DtEaglesoftAppointment_Procedures_Data = SynchEaglesoftBAL.GetEaglesoftAppointment_Procedures_Data(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    // Utility.WriteToDebugSyncLogFile_All("11 DtEaglesoftAppointment_Procedures_Data End. Count:" + DtEaglesoftAppointment_Procedures_Data.Rows.Count, "CallSynch_Appointment_New");

                                    // Utility.WriteToDebugSyncLogFile_All("12 GetEaglesoftAppointmentProviderData_New Start.", "CallSynch_Appointment_New");
                                    DataTable dtEaglesoftAppointmentProider = SynchEaglesoftBAL.GetEaglesoftAppointmentProviderData_New(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                    //  Utility.WriteToDebugSyncLogFile_All("12.1 GetEaglesoftAppointmentProviderData_New End. Count:" + dtEaglesoftAppointmentProider.Rows.Count, "CallSynch_Appointment_New");

                                    // Utility.WriteToDebugSyncLogFile_All("12.2 Provider and Procedure Data Start.", "CallSynch_Appointment_New");
                                    foreach (DataRow dtDtxRow in dtEaglesoftAppointment.Rows)
                                    {
                                        DataRow[] dtCurApptProvider = dtEaglesoftAppointmentProider.Select("appointment_id = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'"); //DataTable dtEaglesoftAppointmentProider = SynchEaglesoftBAL.GetEaglesoftAppointmentProviderData(dtDtxRow["Appt_EHR_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                        string ProvIDList = string.Empty;
                                        string ProvNameList = string.Empty;
                                        foreach (var dtr in dtCurApptProvider)
                                        {
                                            ProvIDList = ProvIDList + dtr["provider_id"].ToString() + " ; ";
                                            ProvNameList = ProvNameList + dtr["ProviderName"].ToString() + " ; ";
                                        }
                                        if (ProvIDList.Length > 0)
                                        {
                                            ProvIDList = ProvIDList.Substring(0, ProvIDList.Length - 1);
                                        }
                                        if (ProvNameList.Length > 0)
                                        {
                                            ProvNameList = ProvNameList.Substring(0, ProvNameList.Length - 1);
                                        }

                                        dtDtxRow["Provider_EHR_ID"] = ProvIDList;
                                        dtDtxRow["ProviderName"] = ProvNameList;
                                        dtDtxRow["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString();

                                        ////////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                                        ProcedureDesc = "";
                                        ProcedureCode = "";

                                        DataRow[] dtCurApptProcedure = DtEaglesoftAppointment_Procedures_Data.Select("appointment_id = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");

                                        foreach (var dtSinProc in dtCurApptProcedure.ToList())
                                        {
                                            ProcedureDesc = ProcedureDesc + dtSinProc["tooth"].ToString() + dtSinProc["surface"].ToString() + dtSinProc["schedabbr"].ToString();
                                            ProcedureCode = ProcedureCode + dtSinProc["ADA_Code"].ToString();
                                        }
                                        if (ProcedureDesc.ToString().Length > 1)
                                        {
                                            ProcedureDesc = ProcedureDesc.Substring(0, ProcedureDesc.Length - 1);
                                            dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                                        }
                                        if (ProcedureCode.ToString().Length > 1)
                                        {
                                            ProcedureCode = ProcedureCode.Substring(0, ProcedureCode.Length - 1);
                                            dtDtxRow["ProcedureCode"] = ProcedureCode;
                                        }
                                        dtEaglesoftAppointment.AcceptChanges();
                                        /////////////////////////////////////
                                    }
                                    // Utility.WriteToDebugSyncLogFile_All("13 Provider and Procedure Data End.", "CallSynch_Appointment_New");

                                    //Utility.WriteToDebugSyncLogFile_All("14 GetLocalAppointmentData Start.", "CallSynch_Appointment_New");
                                    DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                    // Utility.WriteToDebugSyncLogFile_All("15 GetLocalAppointmentData End.", "CallSynch_Appointment_New");

                                    //  Utility.WriteToDebugSyncLogFile_All("16 itemsToBeAdded Comparison Start.", "CallSynch_Appointment_New");
                                    DataTable dtSaveRecords = new DataTable();
                                    dtSaveRecords = dtLocalAppointment.Clone();
                                    ObjGoalBase.WriteToSyncLogFile("PatientSyncNew: Compare Start. Start for patient to be added.");
                                    var itemsToBeAdded = (from TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                                          join LocalAppt in dtLocalAppointment.AsEnumerable()
                                                          on TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                                          equals LocalAppt["Appt_EHR_ID"].ToString().Trim()
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
                                    //Utility.WriteToDebugSyncLogFile_All("17 itemsToBeAdded Comparison End. Count:" + itemsToBeAdded.Count, "CallSynch_Appointment_New");

                                    //Utility.WriteToDebugSyncLogFile_All("18 itemToBeDeleted Comparison Start.", "CallSynch_Appointment_New");
                                    var itemToBeDeleted = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                                           join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                                           on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                                           equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
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
                                    //  Utility.WriteToDebugSyncLogFile_All("19 itemToBeDeleted Comparison End. Count: " + itemToBeDeleted.Count, "CallSynch_Appointment_New");

                                    // Utility.WriteToDebugSyncLogFile_All("20 itemsToBeUpdated Comparison Start.", "CallSynch_Appointment_New");
                                    var itemsToBeUpdated = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                                            join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                                            on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                                            equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
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
                                                            (LocalAppt["Provider_EHR_ID"] != DBNull.Value ? LocalAppt["Provider_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Provider_EHR_ID"] != DBNull.Value ? TrackerAppt["Provider_EHR_ID"].ToString().Trim() : "") ||
                                                            (LocalAppt["Comment"] != DBNull.Value ? LocalAppt["Comment"].ToString().Trim() : "") != (TrackerAppt["Comment"] != DBNull.Value ? TrackerAppt["Comment"].ToString().Trim().Substring(0, TrackerAppt["Comment"].ToString().Trim().Length > 1999 ? 1999 : TrackerAppt["Comment"].ToString().Trim().Length) : "") ||
                                                            (LocalAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["birth_date"].ToString().Trim()) : "") != (TrackerAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["birth_date"].ToString().Trim()) : "") ||
                                                            (LocalAppt["ApptType_EHR_ID"] != DBNull.Value ? LocalAppt["ApptType_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["ApptType_EHR_ID"] != DBNull.Value ? TrackerAppt["ApptType_EHR_ID"].ToString().Trim() : "") ||
                                                            (LocalAppt["ApptType"] != DBNull.Value ? LocalAppt["ApptType"].ToString().Trim() : "") != (TrackerAppt["ApptType"] != DBNull.Value ? TrackerAppt["ApptType"].ToString().Trim() : "") ||
                                                            (LocalAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_DateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_DateTime"].ToString().Trim()) : "") ||
                                                            (LocalAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_EndDateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_EndDateTime"].ToString().Trim()) : "") ||
                                                            (LocalAppt["appointment_status_ehr_key"] != DBNull.Value ? LocalAppt["appointment_status_ehr_key"].ToString().Trim() : "") != (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "") ||
                                                            (LocalAppt["ProcedureDesc"] != DBNull.Value ? LocalAppt["ProcedureDesc"].ToString().Trim() : "") != (TrackerAppt["ProcedureDesc"] != DBNull.Value ? TrackerAppt["ProcedureDesc"].ToString().Trim() : "") ||
                                                            (LocalAppt["ProcedureCode"] != DBNull.Value ? LocalAppt["ProcedureCode"].ToString().Trim() : "") != (TrackerAppt["ProcedureCode"] != DBNull.Value ? TrackerAppt["ProcedureCode"].ToString().Trim() : "") ||
                                                            (LocalAppt["is_asap"] != DBNull.Value ? LocalAppt["is_asap"].ToString().Trim() : "") != (TrackerAppt["is_asap"] != DBNull.Value ? TrackerAppt["is_asap"].ToString().Trim() : "") ||
                                                            (Convert.ToBoolean(LocalAppt["is_deleted"].ToString().Trim()) != (TrackerAppt["classification"].ToString().Trim() == "1" ? false : true)) //||
                                                                                                                                                                                                      //(Convert.ToBoolean(LocalAppt["is_deleted"].ToString().Trim()) !=
                                                                                                                                                                                                      //    ((TrackerAppt["operatoryEHRID"].ToString().Trim() == " - 1" || TrackerAppt["operatoryEHRID"].ToString().Trim() == "" ||
                                                                                                                                                                                                      //      TrackerAppt["classification"].ToString().Trim() == "16" || TrackerAppt["classification"].ToString().Trim() == "32" ||
                                                                                                                                                                                                      //      TrackerAppt["classification"].ToString().Trim() == "64") ? true : false))
                                                            select TrackerAppt).ToList();
                                    DataTable dtPatientToBeUpdated = dtLocalAppointment.Clone();
                                    //Utility.WriteToDebugSyncLogFile_All("21 itemsToBeUpdated End. Count:" + itemsToBeUpdated.Count, "CallSynch_Appointment_New");
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
                                        dtPatientToBeUpdated.Select().ToList<DataRow>().ForEach(r => r["InsUptDlt"] = 4);
                                        dtSaveRecords.Load(dtPatientToBeUpdated.Select().CopyToDataTable().CreateDataReader());
                                    }

                                    if (!dtSaveRecords.Columns.Contains("InsUptDlt"))
                                    {
                                        dtSaveRecords.Columns.Add("InsUptDlt", typeof(int));
                                        dtSaveRecords.Columns["InsUptDlt"].DefaultValue = 0;
                                    }
                                    foreach (DataRow dtDtxRow in dtSaveRecords.Select("InsUptDlt = '4'"))
                                    {
                                        DataRow[] row = dtLocalAppointment.Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                                        if (row.Length > 0)
                                        {
                                            if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                                            {
                                                if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() == "-1" || dtDtxRow["Operatory_EHR_ID"].ToString().Trim() == "" || dtDtxRow["classification"].ToString().Trim() == "16" || dtDtxRow["classification"].ToString().Trim() == "32" || dtDtxRow["classification"].ToString().Trim() == "64")
                                                {
                                                    if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                                    {
                                                        dtDtxRow["InsUptDlt"] = 3;
                                                    }
                                                    else
                                                    {
                                                        dtDtxRow["InsUptDlt"] = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    dtDtxRow["InsUptDlt"] = 4;
                                                }
                                            }
                                        }
                                    }

                                    #region Test
                                    //try
                                    //{
                                    //    var One = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //               join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //               on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //               equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //               where
                                    //               (LocalAppt["First_name"] != DBNull.Value ? LocalAppt["First_name"].ToString().Trim() : "") != (TrackerAppt["First_name"] != DBNull.Value ? TrackerAppt["First_name"].ToString().Trim() : "")
                                    //               select TrackerAppt).ToList();
                                    //    if (One.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("First Name: " + One.Count, "CallSynch_Appointment_New");
                                    //    }
                                    //    var Two = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //               join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //               on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //               equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //               where
                                    //               (LocalAppt["Last_name"] != DBNull.Value ? LocalAppt["Last_name"].ToString().Trim() : "") != (TrackerAppt["Last_name"] != DBNull.Value ? TrackerAppt["Last_name"].ToString().Trim() : "")
                                    //               select TrackerAppt).ToList();
                                    //    if (Two.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Last_name: " + Two.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Three = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                 join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                 on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                 equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                 where
                                    //                 (LocalAppt["MI"] != DBNull.Value ? LocalAppt["MI"].ToString().Trim() : "") != (TrackerAppt["MI"] != DBNull.Value ? TrackerAppt["MI"].ToString().Trim() : "")
                                    //                 select TrackerAppt).ToList();
                                    //    if (Three.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("MI: " + Three.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Four = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                where
                                    //                (LocalAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Home_Contact"].ToString().Trim()) : "") != (TrackerAppt["Home_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(TrackerAppt["Home_Contact"].ToString().Trim()) : "")
                                    //                select TrackerAppt).ToList();
                                    //    if (Four.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Home_Contact: " + Four.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Five = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                where
                                    //                (LocalAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(LocalAppt["Mobile_Contact"].ToString().Trim()) : "") != (TrackerAppt["Mobile_Contact"] != DBNull.Value ? Utility.ConvertContactNumber(TrackerAppt["Mobile_Contact"].ToString().Trim()) : "")
                                    //                select TrackerAppt).ToList();
                                    //    if (Five.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Mobile_Contact: " + Five.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Six = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //               join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //               on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //               equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //               where
                                    //               (LocalAppt["Email"] != DBNull.Value ? LocalAppt["Email"].ToString().Trim() : "") != (TrackerAppt["Email"] != DBNull.Value ? TrackerAppt["Email"].ToString().Trim() : "")
                                    //               select TrackerAppt).ToList();
                                    //    if (Six.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Email: " + Six.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Seven = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                 join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                 on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                 equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                 where
                                    //                 (LocalAppt["Zip"] != DBNull.Value ? LocalAppt["Zip"].ToString().Trim() : "") != (TrackerAppt["Zip"] != DBNull.Value ? TrackerAppt["Zip"].ToString().Trim() : "")
                                    //                 select TrackerAppt).ToList();
                                    //    if (Seven.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Zip: " + Seven.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Eight = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                 join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                 on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                 equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                 where
                                    //                 (LocalAppt["Operatory_EHR_ID"] != DBNull.Value ? LocalAppt["Operatory_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Operatory_EHR_ID"] != DBNull.Value ? TrackerAppt["Operatory_EHR_ID"].ToString().Trim() : "")
                                    //                 select TrackerAppt).ToList();
                                    //    if (Eight.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Operatory_EHR_ID: " + Eight.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Ten = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //               join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //               on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //               equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //               where
                                    //               (LocalAppt["Provider_EHR_ID"] != DBNull.Value ? LocalAppt["Provider_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["Provider_EHR_ID"] != DBNull.Value ? TrackerAppt["Provider_EHR_ID"].ToString().Trim() : "")
                                    //               select TrackerAppt).ToList();
                                    //    if (Ten.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Provider_EHR_ID: " + Ten.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Twelve = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                  join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                  on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                  equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                  where
                                    //                 (LocalAppt["Comment"] != DBNull.Value ? LocalAppt["Comment"].ToString().Trim() : "") != (TrackerAppt["Comment"] != DBNull.Value ? TrackerAppt["Comment"].ToString().Trim().Substring(0, TrackerAppt["Comment"].ToString().Trim().Length > 1999 ? 1999 : TrackerAppt["Comment"].ToString().Trim().Length) : "")
                                    //                  select TrackerAppt).ToList();
                                    //    if (Twelve.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Comment: " + Twelve.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Thirteen = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                    join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                    on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                    equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                    where
                                    //                    (LocalAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["birth_date"].ToString().Trim()) : "") != (TrackerAppt["birth_date"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["birth_date"].ToString().Trim()) : "")
                                    //                    select TrackerAppt).ToList();
                                    //    if (Thirteen.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("birth_date: " + Thirteen.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Fourteen = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                    join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                    on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                    equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                    where
                                    //                    (LocalAppt["ApptType_EHR_ID"] != DBNull.Value ? LocalAppt["ApptType_EHR_ID"].ToString().Trim() : "") != (TrackerAppt["ApptType_EHR_ID"] != DBNull.Value ? TrackerAppt["ApptType_EHR_ID"].ToString().Trim() : "")
                                    //                    select TrackerAppt).ToList();
                                    //    if (Fourteen.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("ApptType_EHR_ID: " + Fourteen.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Sixteen = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                   join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                   on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                   equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                   where
                                    //                   (LocalAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_DateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_DateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_DateTime"].ToString().Trim()) : "")
                                    //                   select TrackerAppt).ToList();
                                    //    if (Sixteen.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Appt_DateTime: " + Sixteen.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Seventeen = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                     join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                     on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                     equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                     where
                                    //                     (LocalAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(LocalAppt["Appt_EndDateTime"].ToString().Trim()) : "") != (TrackerAppt["Appt_EndDateTime"] != DBNull.Value ? Utility.CheckValidDatetime(TrackerAppt["Appt_EndDateTime"].ToString().Trim()) : "")
                                    //                     select TrackerAppt).ToList();
                                    //    if (Seventeen.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("Appt_EndDateTime: " + Seventeen.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var Nineteen = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                    join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                    on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                    equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                    where
                                    //                    (LocalAppt["appointment_status_ehr_key"] != DBNull.Value ? LocalAppt["appointment_status_ehr_key"].ToString().Trim() : "") != (TrackerAppt["appointment_status_ehr_key"] != DBNull.Value ? TrackerAppt["appointment_status_ehr_key"].ToString().Trim() : "")
                                    //                    select TrackerAppt).ToList();
                                    //    if (Nineteen.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("appointment_status_ehr_key: " + Nineteen.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var TwentyFive = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                      join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                      on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                      equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                      where
                                    //                      (LocalAppt["ProcedureDesc"] != DBNull.Value ? LocalAppt["ProcedureDesc"].ToString().Trim() : "") != (TrackerAppt["ProcedureDesc"] != DBNull.Value ? TrackerAppt["ProcedureDesc"].ToString().Trim() : "")
                                    //                      select TrackerAppt).ToList();
                                    //    if (TwentyFive.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("ProcedureDesc: " + TwentyFive.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var TwentySix = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                     join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                     on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                     equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                     where
                                    //                     (LocalAppt["ProcedureCode"] != DBNull.Value ? LocalAppt["ProcedureCode"].ToString().Trim() : "") != (TrackerAppt["ProcedureCode"] != DBNull.Value ? TrackerAppt["ProcedureCode"].ToString().Trim() : "")
                                    //                     select TrackerAppt).ToList();
                                    //    if (TwentySix.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("ProcedureCode: " + TwentySix.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var TwentySeven = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                       join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                       on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                       equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                       where
                                    //                       (Convert.ToBoolean(LocalAppt["is_deleted"].ToString().Trim()) != (TrackerAppt["classification"].ToString().Trim() == "1" ? false : true))
                                    //                       select TrackerAppt).ToList();
                                    //    if (TwentySeven.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("is_deleted: " + TwentySeven.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var TwentyEight = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                       join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                       on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                       equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                       where
                                    //                       (LocalAppt["is_asap"] != DBNull.Value ? LocalAppt["is_asap"].ToString().Trim() : "") != (TrackerAppt["is_asap"] != DBNull.Value ? TrackerAppt["is_asap"].ToString().Trim() : "")
                                    //                       select TrackerAppt).ToList();
                                    //    if (TwentyEight.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("is_asap: " + TwentyEight.Count, "CallSynch_Appointment_New");
                                    //    }

                                    //    var TwentyNine = (from LocalAppt in dtLocalAppointment.AsEnumerable()
                                    //                      join TrackerAppt in dtEaglesoftAppointment.AsEnumerable()
                                    //                      on LocalAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                      equals TrackerAppt["Appt_EHR_ID"].ToString().Trim()
                                    //                      where
                                    //   (Convert.ToBoolean(LocalAppt["is_deleted"].ToString().Trim()) !=
                                    //                           ((TrackerAppt["Operatory_EHR_ID"].ToString().Trim() == " - 1" || TrackerAppt["Operatory_EHR_ID"].ToString().Trim() == "" ||
                                    //                             TrackerAppt["classification"].ToString().Trim() == "16" || TrackerAppt["classification"].ToString().Trim() == "32" ||
                                    //                             TrackerAppt["classification"].ToString().Trim() == "64") ? true : false))
                                    //                      select TrackerAppt).ToList();
                                    //    if (TwentyNine.Count > 0)
                                    //    {
                                    //        Utility.WriteToDebugSyncLogFile_All("operatoryEHRID classification: " + TwentyNine.Count, "CallSynch_Appointment_New");
                                    //    }
                                    //}
                                    //catch (Exception exTest)
                                    //{
                                    //    Utility.WriteToDebugSyncLogFile_All("Test Error:" + exTest.Message, "CallSynch_Appointment_New");
                                    //}
                                    #endregion

                                    if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                                    {
                                        //Utility.WriteToDebugSyncLogFile_All("22 Count of items Add : " + dtSaveRecords.Select("InsUptDlt = '1'").Length, "CallSynch_Appointment_New");
                                        //Utility.WriteToDebugSyncLogFile_All("23 Count of items Update : " + dtSaveRecords.Select("InsUptDlt = '4'").Length, "CallSynch_Appointment_New");
                                        //Utility.WriteToDebugSyncLogFile_All("24 Count of items Delete : " + dtSaveRecords.Select("InsUptDlt = '3'").Length, "CallSynch_Appointment_New");

                                        bool status = SynchEaglesoftBAL.Save_Appointment_Eaglesoft_To_Local(dtSaveRecords, (Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()));
                                        if (status)
                                        {
                                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                                            ObjGoalBase.WriteToSyncLogFile("Appointment Sync New (" + Utility.Application_Name + " to Local Database) Successfully.");

                                            SynchDataLiveDB_Push_Appointment();
                                        }
                                        else
                                        {
                                            ObjGoalBase.WriteToErrorLogFile("[Appointment Sync New 2 (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                        }
                                    }
                                    else
                                    {
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                                        UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment_Push");
                                        ObjGoalBase.WriteToSyncLogFile("Appointment Sync New (" + Utility.Application_Name + " to Local Database) Successfully. No Records Found in diffrence.");
                                    }
                                    //Utility.WriteToDebugSyncLogFile_All("25 Appointment Sync End.", "CallSynch_Appointment_New");
                                }
                            }
                            catch (Exception ex)
                            {
                                Is_synched_Appointment = true;
                                ObjGoalBase.WriteToErrorLogFile("[Appointment New Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                                throw ex;
                            }
                            finally
                            {
                                Is_synched_Appointment = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Is_synched_Appointment = true;
                        ObjGoalBase.WriteToErrorLogFile("[Appointment New Sync Diff (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                        throw ex;
                    }
                    finally
                    {
                        Is_synched_Appointment = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Appointment New Sync Main (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                Is_synched_Appointment = true;
                throw ex;
            }
        }

        public void SynchDataEagleSoft_InsertPatient()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtLocalPatient = SynchLocalBAL.GetLocalInsertPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if ((dtLocalPatient != null && dtLocalPatient.Rows.Count > 0 && dtLocalPatient.Select("Is_Adit_Updated = 1").Length > 0))
                            {
                                DataTable dtEaglesoftPatient = SynchEaglesoftBAL.GetEaglesoftInsertPatientData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                if ((dtEaglesoftPatient != null && dtEaglesoftPatient.Rows.Count > 0))
                                {
                                    var itemsToBeAdded = (from OpenDentalPatient in dtEaglesoftPatient.AsEnumerable()
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
                                            dtSaveRecords = SynchEaglesoftBAL.GetEaglesoftPatientData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), strPatID);
                                            if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                                            {
                                                bool status = false;
                                                if (dtSaveRecords.Rows.Count > 0)
                                                {
                                                    SynchEaglesoftBAL.DecryptSSN(ref dtSaveRecords);
                                                    status = SynchEaglesoftBAL.Save_Patient_Eaglesoft_To_Local_New(dtSaveRecords, "0", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), false);
                                                }
                                                else
                                                {
                                                    status = true;
                                                }

                                                if (status)
                                                {
                                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                                    ObjGoalBase.WriteToSyncLogFile("[SynchDataEagleSoft_InsertPatient Sync (" + Utility.Application_Name + " to Local Database) Successfully.]");
                                                    IsGetParientRecordDone = true;

                                                    SynchDataLiveDB_Push_Patient();
                                                }
                                                else
                                                {
                                                    ObjGoalBase.WriteToErrorLogFile("[SynchDataEagleSoft_InsertPatient Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
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
                throw;
            }
        }

        private void CallSynch_AppointmentsPatient()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                Is_synched_Patient = false;
                Is_synched_AppointmentsPatient = false;
            }
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient)
            {
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftPatient = SynchEaglesoftBAL.GetEaglesoftAppointmentsPatientData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                            string patientTableName = "Patient";

                            string PatientEHRIDs = string.Join("','", dtEaglesoftPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                            if (PatientEHRIDs != string.Empty)
                            {
                                Is_synched_AppointmentsPatient = true;

                                PatientEHRIDs = "'" + PatientEHRIDs + "'";

                                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                                {
                                    patientTableName = "PatientCompare";
                                }

                                if (dtEaglesoftPatient != null && dtEaglesoftPatient.Rows.Count > 0)
                                {
                                    bool Patient = SynchEaglesoftBAL.Save_Patient_Eaglesoft_To_Local(dtEaglesoftPatient, patientTableName, dtLocalPatient, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                    if (Patient)
                                    {
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                        ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                        SynchDataLiveDB_Push_Patient();
                                    }
                                    else
                                    {
                                        ObjGoalBase.WriteToErrorLogFile("[Appointment Patient Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                    }
                                }
                                else
                                {
                                    ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                    bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                                }
                                Is_synched_AppointmentsPatient = false;
                                //SynchDataEagleSoft_PatientDisease();
                                //SynchDataEagleSoft_PatientMedication();
                                if (Is_Synched_PatientCallHit)
                                {
                                    //Is_Synched_PatientCallHit = false;
                                    //CallSynch_Patient();
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Is_synched_AppointmentsPatient = false;
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    throw;
                }
            }
        }

        private void CallSynch_AppointmentsPatient_New()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                Is_synched_Patient = false;
                Is_synched_AppointmentsPatient = false;
            }
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient)
            {
                try
                {
                    //Utility.WriteToDebugSyncLogFile_All("1 Start.", "CallSynch_AppointmentsPatient_New");
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        //Utility.WriteToDebugSyncLogFile_All("2 Service Install ID : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), "CallSynch_AppointmentsPatient_New");
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        //Utility.WriteToDebugSyncLogFile_All("3 AditLocationSyncEnable :" + Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()), "CallSynch_AppointmentsPatient_New");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            // Utility.WriteToDebugSyncLogFile_All("4 GetEaglesoftAppointmentsPatientData Start.", "CallSynch_AppointmentsPatient_New");
                            DataTable dtEaglesoftPatient = SynchEaglesoftBAL.GetEaglesoftAppointmentsPatientData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            // Utility.WriteToDebugSyncLogFile_All("5 GetEaglesoftAppointmentsPatientData End. Count:" + dtEaglesoftPatient.Rows.Count, "CallSynch_AppointmentsPatient_New");

                            string patientTableName = "Patient";

                            string PatientEHRIDs = string.Join("','", dtEaglesoftPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()));

                            if (PatientEHRIDs != string.Empty)
                            {
                                Is_synched_AppointmentsPatient = true;
                                // Utility.WriteToDebugSyncLogFile_All("6 GetLocalPatientData Start.", "CallSynch_AppointmentsPatient_New");
                                DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                //  Utility.WriteToDebugSyncLogFile_All("7 GetLocalPatientData End. Total Pats: " + dtLocalPatientResult.Rows.Count, "CallSynch_AppointmentsPatient_New");
                                DataTable dtLocalPatient = new DataTable();
                                dtLocalPatient = dtLocalPatientResult.Clone();
                                var LocalPatientByPatEHRID = (from LocalPatient in dtLocalPatientResult.AsEnumerable()
                                                              join OpenDentalPatient in dtEaglesoftPatient.AsEnumerable()
                                                              on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                              equals OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                              where LocalPatient["Patient_EHR_ID"].ToString().Trim() == OpenDentalPatient["Patient_EHR_ID"].ToString().Trim()
                                                              select LocalPatient).ToList();

                                if (LocalPatientByPatEHRID.Count > 0)
                                {
                                    dtLocalPatient = LocalPatientByPatEHRID.CopyToDataTable<DataRow>();
                                }
                                //  Utility.WriteToDebugSyncLogFile_All("8 Pats to comapre:" + dtLocalPatient.Rows.Count, "CallSynch_AppointmentsPatient_New");

                                //  Utility.WriteToDebugSyncLogFile_All("9 itemsToBeAdded Start.", "CallSynch_AppointmentsPatient_New");
                                DataTable dtSaveRecords = new DataTable();
                                dtSaveRecords = dtEaglesoftPatient.Clone();

                                var itemsToBeAdded = (from OpenDentalPatient in dtEaglesoftPatient.AsEnumerable()
                                                      join LocalPatient in dtLocalPatient.AsEnumerable()
                                                      on OpenDentalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + OpenDentalPatient["Clinic_Number"].ToString().Trim()
                                                      equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                      into matchingRows
                                                      from matchingRow in matchingRows.DefaultIfEmpty()
                                                      where matchingRow == null
                                                      select OpenDentalPatient).ToList();
                                DataTable dtPatientToBeAdded = dtLocalPatient.Clone();
                                // Utility.WriteToDebugSyncLogFile_All("10 itemsToBeAdded End. Count: " + itemsToBeAdded.Count, "CallSynch_AppointmentsPatient_New");
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
                                //Utility.WriteToDebugSyncLogFile_All("11 itemsToBeUpdated Start.", "CallSynch_AppointmentsPatient_New");
                                var itemsToBeUpdated = (from OpenDentalPatient in dtEaglesoftPatient.AsEnumerable()
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
                                //Utility.WriteToDebugSyncLogFile_All("12 itemsToBeUpdated End. Count:" + itemsToBeUpdated.Count, "CallSynch_AppointmentsPatient_New");
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

                                // Utility.WriteToDebugSyncLogFile_All("13 dtSaveRecords Count : " + dtSaveRecords.Rows.Count, "CallSynch_AppointmentsPatient_New");
                                if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                                {
                                    // Utility.WriteToDebugSyncLogFile_All("14 dtSaveRecords Add: " + dtSaveRecords.Select("InsUptDlt = '1'").Length, "CallSynch_AppointmentsPatient_New");
                                    //  Utility.WriteToDebugSyncLogFile_All("15 dtSaveRecords Update: " + dtSaveRecords.Select("InsUptDlt = '2'").Length, "CallSynch_AppointmentsPatient_New");
                                    bool Patient = SynchEaglesoftBAL.Save_Patient_Eaglesoft_To_Local_New(dtEaglesoftPatient, "0", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), true);
                                    if (Patient)
                                    {
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                        ObjGoalBase.WriteToSyncLogFile("Appointment Patient New Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                        SynchDataLiveDB_Push_Patient();
                                    }
                                    else
                                    {
                                        ObjGoalBase.WriteToErrorLogFile("[Appointment Patient New Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                    }
                                }
                                else
                                {
                                    ObjGoalBase.WriteToSyncLogFile("Appointment Patient New Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                    bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                                }
                                Is_synched_AppointmentsPatient = false;
                                //SynchDataEagleSoft_PatientDisease();
                                //SynchDataEagleSoft_PatientMedication();
                                if (Is_Synched_PatientCallHit)
                                {
                                    //Is_Synched_PatientCallHit = false;
                                    //CallSynch_Patient();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Is_synched_AppointmentsPatient = false;
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Patient New Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    throw ex;
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
            bwSynchEaglesoft_OperatoryEvent = new BackgroundWorker();
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

        private void CallSynch_OperatoryEvent()
        {
            if (IsEaglesoftOperatorySync && !Is_synched_OperatoryEvent && Utility.IsApplicationIdleTimeOff)
            {


                try
                {
                    Is_synched_OperatoryEvent = true;

                    #region Operatory Event
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            DateTime dtCurrentDtTime = Utility.LastSyncDateAditServer;

                            DataTable dtEaglesoftOperatoryEvent = SynchEaglesoftBAL.GetEaglesoftOperatoryEventData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            //dtEaglesoftOperatoryEvent.Columns.Add("Operatory_EHR_ID", typeof(string));
                            //dtEaglesoftOperatoryEvent.Columns.Add("OE_EHR_ID", typeof(string));
                            dtEaglesoftOperatoryEvent.Columns.Add("OE_LocalDB_ID", typeof(int));
                            dtEaglesoftOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            foreach (DataRow dtDtxRow in dtEaglesoftOperatoryEvent.Rows)
                            {

                                //DataRow[] rowBlcOpt = dtLocalOperatory.Copy().Select("Operatory_Name = '" + dtDtxRow["Operatory_Name"].ToString().Trim() + "' ");
                                //if (rowBlcOpt.Length > 0)
                                //{
                                //    dtDtxRow["OE_EHR_ID"] = rowBlcOpt[0]["OE_EHR_ID"].ToString();
                                //}    

                                DateTime tempStartTime = Convert.ToDateTime(Convert.ToDateTime(dtDtxRow["StartTime"].ToString()).ToShortDateString() + " " + (Convert.ToDateTime(dtDtxRow["StartTime"].ToString()).ToShortTimeString() == "00:00" ? Convert.ToDateTime(dtDtxRow["StartTime"].ToString()).AddMinutes(1).ToShortTimeString() : Convert.ToDateTime(dtDtxRow["StartTime"].ToString()).ToShortTimeString()));
                                DateTime tempEndTime = Convert.ToDateTime((Convert.ToDateTime(dtDtxRow["EndTime"].ToString()).ToShortTimeString() == "00:00" ? Convert.ToDateTime(dtDtxRow["EndTime"].ToString()).AddDays(-1).ToShortDateString() : Convert.ToDateTime(dtDtxRow["EndTime"].ToString()).ToShortDateString()) + " " + (Convert.ToDateTime(dtDtxRow["EndTime"].ToString()).ToShortTimeString() == "00:00" ? Convert.ToDateTime(dtDtxRow["EndTime"].ToString()).AddMinutes(-1).ToShortTimeString() : Convert.ToDateTime(dtDtxRow["EndTime"].ToString()).ToShortTimeString()));

                                DataRow[] row = dtLocalOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtDtxRow["OE_EHR_ID"].ToString().Trim() + "' ");
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

                                    else if (dtDtxRow["comment"].ToString().ToLower().Trim().Substring(0, commentlen) != row[0]["comment"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }

                                    else if (tempStartTime != Convert.ToDateTime(row[0]["StartTime"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (tempEndTime != Convert.ToDateTime(row[0]["EndTime"].ToString().Trim()))
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

                                DataRow[] rowBlcOpt = dtEaglesoftOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtLOERow["OE_EHR_ID"].ToString().Trim() + "' ");
                                if (rowBlcOpt.Length > 0)
                                { }
                                else
                                {
                                    DataRow BlcOptDtldr = dtEaglesoftOperatoryEvent.NewRow();
                                    BlcOptDtldr["OE_EHR_ID"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                                    BlcOptDtldr["InsUptDlt"] = 3;
                                    dtEaglesoftOperatoryEvent.Rows.Add(BlcOptDtldr);
                                }
                            }

                            dtEaglesoftOperatoryEvent.AcceptChanges();

                            if (dtEaglesoftOperatoryEvent != null && dtEaglesoftOperatoryEvent.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_OperatoryEvent_Eaglesoft_To_Local(dtEaglesoftOperatoryEvent, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                if (status)
                                {
                                    ObjGoalBase.WriteToSyncLogFile("OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_OperatoryEvent();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[OperatoryEvent Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                            else
                            {
                                bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent_Push");
                            }
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryEvent");
                            #endregion


                            #region Operatory Chair
                            CallSynch_OperatoryChair();
                            #endregion

                            Is_synched_OperatoryEvent = false;
                            IsEHRAllSync = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Is_synched_OperatoryEvent = false;
                    ObjGoalBase.WriteToErrorLogFile("[OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }


            }
        }

        private void CallSynch_OperatoryChair()
        {
            try
            {
                for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                {
                    DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                    if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                    {
                        DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        DateTime dtCurrentDtTime = Utility.LastSyncDateAditServer;

                        DataTable dtEaglesoftOperatoryChair = SynchEaglesoftBAL.GetEaglesoftOperatoryChairData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        //dtEaglesoftOperatoryChair.Columns.Add("Operatory_EHR_ID", typeof(string));
                        //dtEaglesoftOperatoryChair.Columns.Add("OE_EHR_ID", typeof(string));
                        dtEaglesoftOperatoryChair.Columns.Add("OE_LocalDB_ID", typeof(int));
                        dtEaglesoftOperatoryChair.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalOperatoryChair = SynchLocalBAL.GetLocalOperatoryChairData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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

                        if (dtEaglesoftOperatoryChair != null && dtEaglesoftOperatoryChair.Rows.Count > 0)
                        {
                            bool status = SynchEaglesoftBAL.Save_OperatoryDayOff_Eaglesoft_To_Local(dtEaglesoftOperatoryChair, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (status)
                            {
                                ObjGoalBase.WriteToSyncLogFile("OperatoryDayOff Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
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
            catch (Exception)
            {
                throw;
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
            bwSynchEaglesoft_Provider = new BackgroundWorker();
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
                CommonFunction.GetMasterSync();

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

        private void CallSynch_Provider()
        {
            if (!Is_synched_Provider && Utility.IsApplicationIdleTimeOff)
            {
                Is_synched_Provider = true;

                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftProvider = SynchEaglesoftBAL.GetEaglesoftProviderData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEaglesoftProvider.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            foreach (DataRow dtDtxRow in dtEaglesoftProvider.Rows)
                            {
                                DataRow[] row = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtDtxRow["Provider_EHR_ID"].ToString().Trim() + "'");
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
                                    if (dtDtxRow["gender"].ToString().ToLower().Trim() == "M".ToString().ToLower())
                                    {
                                        strTmpGender = "male";
                                    }
                                    else
                                    {
                                        strTmpGender = "female";
                                    }

                                    if (dtDtxRow["Last_Name"].ToString().Trim() != row[0]["Last_Name"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["First_Name"].ToString().Trim() != row[0]["First_Name"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (strTmpGender != row[0]["gender"].ToString().Trim())
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

                            dtEaglesoftProvider.AcceptChanges();

                            if (dtEaglesoftProvider != null && dtEaglesoftProvider.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_Provider_Eaglesoft_To_Local(dtEaglesoftProvider, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                                    ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    IsEaglesoftProviderSync = true;
                                    SynchDataLiveDB_Push_Provider();

                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Providers Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                    IsEaglesoftProviderSync = false;
                                }
                            }
                        }
                    }
                    Is_synched_Provider = false;
                    IsProviderSyncedFirstTime = true;
                }
                catch (Exception ex)
                {
                    Is_synched_Provider = false;
                    IsProviderSyncedFirstTime = true;
                    ObjGoalBase.WriteToErrorLogFile("[Provider Sync (" + Utility.Application_Name + " to Local Database)]" + ex.Message);
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
            bwSynchEaglesoft_Speciality = new BackgroundWorker();
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

        private void CallSynch_Speciality()
        {
            if (!Is_synched_Speciality && Utility.IsApplicationIdleTimeOff)
            {
                Is_synched_Speciality = true;

                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftSpeciality = SynchEaglesoftBAL.GetEaglesoftSpecilityData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            DataTable dtSpecialitydistinctValues = new DataTable();
                            DataView view = new DataView(dtEaglesoftSpeciality);
                            dtSpecialitydistinctValues = view.ToTable(true, "speciality_Name");
                            dtSpecialitydistinctValues.Columns.Add("InsUptDlt", typeof(int));

                            DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            foreach (DataRow dtDtxRow in dtSpecialitydistinctValues.Rows)
                            {
                                DataRow[] row = dtLocalSpeciality.Copy().Select("speciality_Name = '" + dtDtxRow["speciality_Name"].ToString().Trim() + "'");
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
                                bool status = SynchEaglesoftBAL.Save_Speciality_Eaglesoft_To_Local(dtSpecialitydistinctValues, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality");
                                    ObjGoalBase.WriteToSyncLogFile("Speciality Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_Speciality();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Speciality Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
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

        #endregion

        #region FolderList

        private void Synch_FolderList()
        {
            if (Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftFolderList = SynchEaglesoftBAL.GetEaglesoftFolderListData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEaglesoftFolderList.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalFolderList = SynchLocalBAL.GetLocalFolderListData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            foreach (DataRow dtDtxRow in dtEaglesoftFolderList.Rows)
                            {
                                DataRow[] row = dtLocalFolderList.Copy().Select("FolderList_EHR_ID = '" + dtDtxRow["FolderList_EHR_ID"] + "'");
                                if (row.Length > 0)
                                {
                                    if (dtDtxRow["Folder_Name"].ToString().Trim() != row[0]["Folder_Name"].ToString().Trim())
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
                                DataRow rowBlcOpt = dtEaglesoftFolderList.Copy().Select("FolderList_EHR_ID = '" + dtRow["FolderList_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                                if (rowBlcOpt == null)
                                {
                                    if (!Convert.ToBoolean(dtRow["is_deleted"]))
                                    {
                                        DataRow ESApptDtldr = dtEaglesoftFolderList.NewRow();
                                        ESApptDtldr["FolderList_EHR_ID"] = dtRow["FolderList_EHR_ID"].ToString().Trim();
                                        ESApptDtldr["InsUptDlt"] = 3;
                                        dtEaglesoftFolderList.Rows.Add(ESApptDtldr);
                                    }
                                }
                                else
                                {
                                    if (Convert.ToBoolean(dtRow["is_deleted"]))
                                    {
                                        DataRow ESApptDtldr = dtEaglesoftFolderList.NewRow();
                                        ESApptDtldr["FolderList_EHR_ID"] = dtRow["FolderList_EHR_ID"].ToString().Trim();
                                        ESApptDtldr["InsUptDlt"] = 4;
                                        dtEaglesoftFolderList.Rows.Add(ESApptDtldr);
                                    }
                                }
                            }

                            dtEaglesoftFolderList.AcceptChanges();

                            if (dtEaglesoftFolderList != null && dtEaglesoftFolderList.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_FolderList_Eaglesoft_To_Local(dtEaglesoftFolderList, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FolderList");
                                    ObjGoalBase.WriteToSyncLogFile("FolderList Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("FolderList Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                                SynchDataLiveDB_Push_FolderList();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[FolderList Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
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
            bwSynchEaglesoft_Operatory = new BackgroundWorker();
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
                // CallSynch_OperatoryHours();
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

        private void CallSynch_Operatory()
        {
            if (!Is_synched_Operatory && Utility.IsApplicationIdleTimeOff)
            {
                Synch_FolderList();
                Is_synched_Operatory = true;

                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftOperatory = SynchEaglesoftBAL.GetEaglesoftOperatoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEaglesoftOperatory.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            foreach (DataRow dtDtxRow in dtEaglesoftOperatory.Rows)
                            {
                                DataRow[] row = dtLocalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtDtxRow["Operatory_EHR_ID"].ToString().Trim() + "'");
                                if (row.Length > 0)
                                {
                                    if (dtDtxRow["Operatory_Name"].ToString().Trim() != row[0]["Operatory_Name"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["OperatoryOrder"].ToString().Trim() != row[0]["OperatoryOrder"].ToString().Trim())
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

                            foreach (DataRow dtRow in dtLocalOperatory.Rows)
                            {
                                DataRow rowBlcOpt = dtEaglesoftOperatory.Copy().Select("Operatory_EHR_ID = '" + dtRow["Operatory_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                                if (rowBlcOpt == null)
                                {
                                    if (!Convert.ToBoolean(dtRow["is_deleted"]))
                                    {
                                        DataRow ESApptDtldr = dtEaglesoftOperatory.NewRow();
                                        ESApptDtldr["Operatory_EHR_ID"] = dtRow["Operatory_EHR_ID"].ToString().Trim();
                                        ESApptDtldr["InsUptDlt"] = 3;
                                        dtEaglesoftOperatory.Rows.Add(ESApptDtldr);
                                    }
                                }
                                else
                                {
                                    if (Convert.ToBoolean(dtRow["is_deleted"]))
                                    {
                                        DataRow ESApptDtldr = dtEaglesoftOperatory.NewRow();
                                        ESApptDtldr["Operatory_EHR_ID"] = dtRow["Operatory_EHR_ID"].ToString().Trim();
                                        ESApptDtldr["InsUptDlt"] = 4;
                                        dtEaglesoftOperatory.Rows.Add(ESApptDtldr);
                                    }
                                }
                            }

                            dtEaglesoftOperatory.AcceptChanges();

                            if (dtEaglesoftOperatory != null && dtEaglesoftOperatory.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_Operatory_Eaglesoft_To_Local(dtEaglesoftOperatory, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                                    ObjGoalBase.WriteToSyncLogFile("Chairs (Operatory) Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    IsEaglesoftOperatorySync = true;
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Chairs (Operatory) Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                    IsEaglesoftOperatorySync = false;
                                }

                                #region Deleted Operatory
                                //dtEaglesoftOperatory = dtEaglesoftOperatory.Clone();
                                //DataTable dtEaglesoftDeletedOperatory = SynchEaglesoftBAL.GetEaglesoftDeletedOperatoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                //DataTable dtLocalOperatoryAfterInsert = SynchLocalBAL.GetLocalOperatoryData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                //foreach (DataRow dtDtlRow in dtEaglesoftDeletedOperatory.Rows)
                                //{
                                //    DataRow[] row = dtLocalOperatoryAfterInsert.Copy().Select("Operatory_EHR_ID = '" + dtDtlRow["Operatory_EHR_ID"].ToString().Trim() + "'");
                                //    if (row.Length > 0)
                                //    {
                                //        if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                //        {
                                //            DataRow ApptDtldr = dtEaglesoftOperatory.NewRow();
                                //            ApptDtldr["Operatory_EHR_ID"] = dtDtlRow["Operatory_EHR_ID"].ToString().Trim();
                                //            ApptDtldr["InsUptDlt"] = 3;
                                //            dtEaglesoftOperatory.Rows.Add(ApptDtldr);
                                //        }
                                //    }
                                //}
                                //if (dtEaglesoftOperatory != null && dtEaglesoftOperatory.Rows.Count > 0)
                                //{
                                //    status = SynchEaglesoftBAL.Save_Operatory_Eaglesoft_To_Local(dtEaglesoftOperatory, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                //}
                                #endregion

                                SynchDataLiveDB_Push_Operatory();
                            }
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

        #endregion

        #region Operatory Hours

        private void fncSynchDataEaglesoft_OperatoryHours()
        {
            //SynchDataEaglesoft_OperatoryHours();
            //SynchDataEagleSoftToLocal_OperatoryOfficeHours();
            InitBgWorkerEaglesoft_OperatoryHours();
            InitBgTimerEaglesoft_OperatoryHours();
        }

        private void InitBgTimerEaglesoft_OperatoryHours()
        {
            timerSynchEaglesoft_OperatoryHours = new System.Timers.Timer();
            this.timerSynchEaglesoft_OperatoryHours.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchEaglesoft_OperatoryHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_OperatoryHours_Tick);
            timerSynchEaglesoft_OperatoryHours.Enabled = true;
            timerSynchEaglesoft_OperatoryHours.Start();
        }

        private void InitBgWorkerEaglesoft_OperatoryHours()
        {
            bwSynchEaglesoft_OperatoryHours = new BackgroundWorker();
            bwSynchEaglesoft_OperatoryHours.WorkerReportsProgress = true;
            bwSynchEaglesoft_OperatoryHours.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_OperatoryHours.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_OperatoryHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_OperatoryHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_OperatoryHours_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_OperatoryHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_OperatoryHours.Enabled = false;
                MethodForCallSynchOrderEaglesoft_OperatoryHours();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_OperatoryHours()
        {
            System.Threading.Thread procThreadmainEaglesoft_OperatoryHours = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_OperatoryHours);
            procThreadmainEaglesoft_OperatoryHours.Start();
        }

        public void CallSyncOrderTableEaglesoft_OperatoryHours()
        {
            if (bwSynchEaglesoft_OperatoryHours.IsBusy != true)
            {
                bwSynchEaglesoft_OperatoryHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_OperatoryHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_OperatoryHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SyncPullLogsAndSaveinEaglesoft();
                SynchDataEagleSoftToLocal_OperatoryOfficeHours();
                SynchDataEaglesoft_OperatoryHours();
                // CallSynch_OperatoryHoursHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }
        private void SyncPullLogsAndSaveinEaglesoft()
        {
            try
            {
                CheckCustomhoursForProviderOperatory();
                SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                SynchDataLiveDB_Pull_PatientFollowUp();
                SynchDataLocalToEagleSoft_Patient_SMSCallLog();
                fncPaymentSMSCallStatusUpdate();
                SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
            }
            catch (Exception)
            {

            }
        }
        private void bwSynchEaglesoft_OperatoryHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_OperatoryHours.Enabled = true;
        }

        public void SynchDataEaglesoft_OperatoryHours()
        {
            CallSynch_OperatoryHours();

        }

        private void CallSynch_OperatoryHours()
        {
            try
            {
                if (Utility.is_scheduledCustomhour)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEagleSoftCustomeHours = SynchEaglesoftBAL.GetEaglesoftOperatoryHours(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            DataTable dtEagleSoftLocalCustomeHours = SynchLocalBAL.GetLocalOperatoryHoursData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            dtEagleSoftCustomeHours.Columns.Add("InsUptDlt", typeof(int));
                            dtEagleSoftCustomeHours.Columns["InsUptDlt"].DefaultValue = 0;

                            if (!dtEagleSoftLocalCustomeHours.Columns.Contains("InsUptDlt"))
                            {
                                dtEagleSoftLocalCustomeHours.Columns.Add("InsUptDlt", typeof(int));
                                dtEagleSoftLocalCustomeHours.Columns["InsUptDlt"].DefaultValue = 0;
                            }

                            dtEagleSoftLocalCustomeHours = CompareDataTablewithDateOnly(ref dtEagleSoftCustomeHours, dtEagleSoftLocalCustomeHours, "OH_EHR_ID", "OH_LocalDB_ID", "OH_LocalDB_ID,OH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");

                            dtEagleSoftCustomeHours.AcceptChanges();
                            dtEagleSoftLocalCustomeHours.AcceptChanges();

                            if ((dtEagleSoftCustomeHours != null && dtEagleSoftCustomeHours.Rows.Count > 0) || (dtEagleSoftLocalCustomeHours != null && dtEagleSoftLocalCustomeHours.Rows.Count > 0))
                            {
                                bool status = false;
                                DataTable dtSaveRecords = dtEagleSoftCustomeHours.Clone();
                                if (dtEagleSoftCustomeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtEagleSoftLocalCustomeHours.Select("InsUptDlt IN (3)").Count() > 0)
                                {
                                    if (dtEagleSoftCustomeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtEagleSoftCustomeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                                    }
                                    if (dtEagleSoftLocalCustomeHours.Select("InsUptDlt IN (3)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtEagleSoftLocalCustomeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                                    }
                                    status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryHours", "OH_LocalDB_ID,OH_Web_ID", "OH_LocalDB_ID");
                                }
                                else
                                {
                                    if (dtEagleSoftCustomeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[OperatoryHours Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SynchDataEagleSoftToLocal_OperatoryOfficeHours()
        {
            CallSync_OperatoryOfficeHours();
        }

        private void CallSync_OperatoryOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && IsEaglesoftOperatorySync && IsEaglesoftOperatorySync && Utility.is_scheduledCustomhour)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEagleSoftOperatoryOfficeHours = SynchEaglesoftBAL.GetEagleSoftOperatoryOfficeHours(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            DataTable dtEagleSoftLocalOperatoryOfficeHours = SynchLocalBAL.GetLocalOperatoryOfficeHoursData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            dtEagleSoftOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                            dtEagleSoftOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                            if (!dtEagleSoftLocalOperatoryOfficeHours.Columns.Contains("InsUptDlt"))
                            {
                                dtEagleSoftLocalOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                                dtEagleSoftLocalOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                            }

                            dtEagleSoftLocalOperatoryOfficeHours = CompareDataTableRecords(ref dtEagleSoftOperatoryOfficeHours, dtEagleSoftLocalOperatoryOfficeHours, "OOH_EHR_ID", "OOH_LocalDB_ID", "OOH_LocalDB_ID,OOH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                            dtEagleSoftOperatoryOfficeHours.AcceptChanges();
                            dtEagleSoftLocalOperatoryOfficeHours.AcceptChanges();

                            if ((dtEagleSoftOperatoryOfficeHours != null && dtEagleSoftOperatoryOfficeHours.Rows.Count > 0) || (dtEagleSoftLocalOperatoryOfficeHours != null && dtEagleSoftLocalOperatoryOfficeHours.Rows.Count > 0))
                            {
                                bool status = false;
                                DataTable dtSaveRecords = dtEagleSoftOperatoryOfficeHours.Clone();
                                if (dtEagleSoftOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtEagleSoftLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                                {
                                    if (dtEagleSoftOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtEagleSoftOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());

                                    }
                                    if (dtEagleSoftLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtEagleSoftLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                                    }
                                    status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryOfficeHours", "OOH_LocalDB_ID,OOH_Web_ID", "OOH_LocalDB_ID");
                                }
                                else
                                {
                                    if (dtEagleSoftOperatoryOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[OperatoryOfficeHours Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
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
            bwSynchEaglesoft_ApptType = new BackgroundWorker();
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

        private void CallSynch_Type()
        {
            if (!Is_synched_ApptType && Utility.IsApplicationIdleTimeOff)
            {
                Is_synched_ApptType = true;

                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftApptType = SynchEaglesoftBAL.GetEaglesoftApptTypeData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEaglesoftApptType.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            foreach (DataRow dtDtxRow in dtEaglesoftApptType.Rows)
                            {
                                DataRow[] row = dtLocalApptType.Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"].ToString().Trim() + "'");
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
                                dtEaglesoftApptType.AcceptChanges();
                            }
                            foreach (DataRow dtDtxRow in dtLocalApptType.Rows)
                            {
                                DataRow[] row = dtEaglesoftApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                                if (row.Length > 0)
                                { }
                                else
                                {
                                    DataRow BlcOptDtldr = dtEaglesoftApptType.NewRow();
                                    BlcOptDtldr["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString().Trim();
                                    BlcOptDtldr["Type_Name"] = dtDtxRow["Type_Name"].ToString().Trim();
                                    BlcOptDtldr["InsUptDlt"] = 3;
                                    dtEaglesoftApptType.Rows.Add(BlcOptDtldr);
                                }
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
                                bool Type = SynchEaglesoftBAL.Save_ApptType_Eaglesoft_To_Local(dtEaglesoftApptType, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (Type)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                                    ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    IsEaglesoftApptTypeSync = true;
                                    SynchDataLiveDB_Push_ApptType();

                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Appointment Type Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                    IsEaglesoftApptTypeSync = false;
                                }
                            }
                        }
                    }
                    Is_synched_ApptType = false;
                }
                catch (Exception ex)
                {
                    Is_synched_ApptType = false;
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Type Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
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
            bwSynchEaglesoft_Patient = new BackgroundWorker();
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
            try
            {
                CallSynch_Patient_New();
            }
            catch (Exception exPat)
            {
                CallSynch_Patient();
            }
        }

        public void CallSynch_Patient()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                Is_synched_Patient = false;
                Is_synched_AppointmentsPatient = false;
            }
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient && !Is_synched_AppointmentsPatient)
            {
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            Is_Synched_PatientCallHit = false;
                            Is_synched_Patient = true;
                            SynchDataLiveDB_Pull_EHR_Patientoptout();
                            DataTable dtEaglesoftPatient = SynchEaglesoftBAL.GetEaglesoftPatientData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            //DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            //DataTable dtEaglesoftPatient = SynchEaglesoftBAL.GetEaglesoftPatientData();
                            //DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData();

                            //dtEaglesoftPatient.Columns.Add("tmpSex", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("tmpMaritalStatus", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("tmpReceiveSMS", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("tmpReceiveEmail", typeof(string));

                            //dtEaglesoftPatient.Columns.Add("tmpBirth_Date", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("tmpFirstVisit_Date", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("tmpLastVisit_Date", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("tmpnextvisit_date", typeof(string));

                            //dtEaglesoftPatient.Columns.Add("due_date", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("remaining_benefit", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("used_benefit", typeof(string));
                            //dtEaglesoftPatient.Columns.Add("InsUptDlt", typeof(int));

                            string patientTableName = "Patient";

                            DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                            {
                                patientTableName = "PatientCompare";
                            }


                            if (dtEaglesoftPatient != null && dtEaglesoftPatient.Rows.Count > 0)
                            {
                                bool Patient = SynchEaglesoftBAL.Save_Patient_Eaglesoft_To_Local(dtEaglesoftPatient, patientTableName, dtLocalPatient, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), true);

                                if (Patient)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                    ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    IsGetParientRecordDone = true;

                                    SynchDataLiveDB_Push_Patient();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
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
                    IsPatientSyncedFirstTime = true;
                    Is_synched_Patient = false;
                    SynchDataEagleSoft_PatientStatus();
                    SynchDataEagleSoft_PatientImages();
                    SynchDataEagleSoft_PatientDisease();
                    SynchDataEagleSoft_PatientMedication("");
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    Is_synched_Patient = false;
                }
            }
            else if (Is_synched_AppointmentsPatient)
            {
                Is_Synched_PatientCallHit = true;
            }
        }

        public void CallSynch_Patient_New()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                Is_synched_Patient = false;
                Is_synched_AppointmentsPatient = false;
            }
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_Patient && !Is_synched_AppointmentsPatient)
            {
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            Is_Synched_PatientCallHit = false;
                            Is_synched_Patient = true;
                            SynchDataLiveDB_Pull_EHR_Patientoptout();
                            DataTable dtEaglesoftPatient = SynchEaglesoftBAL.GetEaglesoftPatientData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                            string patientTableName = "Patient";

                            DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            DataTable dtSaveRecords = new DataTable();
                            dtSaveRecords = dtLocalPatient.Clone();
                            var itemsToBeAdded = (from EaglesoftPatient in dtEaglesoftPatient.AsEnumerable()
                                                  join LocalPatient in dtLocalPatient.AsEnumerable()
                                                  on EaglesoftPatient["Patient_EHR_ID"].ToString().Trim() + "_" + EaglesoftPatient["Clinic_Number"].ToString().Trim()
                                                  equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                  into matchingRows
                                                  from matchingRow in matchingRows.DefaultIfEmpty()
                                                  where matchingRow == null
                                                  select EaglesoftPatient).ToList();
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

                            var itemsToBeUpdated = (from LocalPatient in dtLocalPatient.AsEnumerable()
                                                    join EaglesoftPatient in dtEaglesoftPatient.AsEnumerable()
                                                    on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                    equals EaglesoftPatient["Patient_EHR_ID"].ToString().Trim() + "_" + EaglesoftPatient["Clinic_Number"].ToString().Trim()
                                                    //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                                    //equals new { PatID = EaglesoftPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = EaglesoftPatient["Clinic_Number"].ToString().Trim() }
                                                    where
                                                    (LocalPatient["First_name"] != DBNull.Value ? LocalPatient["First_name"].ToString().Trim() : "") != (EaglesoftPatient["First_name"] != DBNull.Value ? EaglesoftPatient["First_name"].ToString().Trim() : "") ||
                                                    (LocalPatient["Last_name"] != DBNull.Value ? LocalPatient["Last_name"].ToString().Trim() : "") != (EaglesoftPatient["Last_name"] != DBNull.Value ? EaglesoftPatient["Last_name"].ToString().Trim() : "") ||
                                                    (LocalPatient["Middle_Name"] != DBNull.Value ? LocalPatient["Middle_Name"].ToString().Trim() : "") != (EaglesoftPatient["Middle_Name"] != DBNull.Value ? EaglesoftPatient["Middle_Name"].ToString().Trim() : "") ||
                                                    (LocalPatient["Salutation"] != DBNull.Value ? LocalPatient["Salutation"].ToString().Trim() : "") != (EaglesoftPatient["Salutation"] != DBNull.Value ? EaglesoftPatient["Salutation"].ToString().Trim() : "") ||
                                                    (LocalPatient["Status"] != DBNull.Value ? LocalPatient["Status"].ToString().Trim() : "") != (EaglesoftPatient["Status"] != DBNull.Value ? EaglesoftPatient["Status"].ToString().Trim() : "") ||
                                                    (LocalPatient["Sex"] != DBNull.Value ? LocalPatient["Sex"].ToString().Trim() : "") != (EaglesoftPatient["Sex"] != DBNull.Value ? EaglesoftPatient["Sex"].ToString().Trim() : "") ||
                                                    (LocalPatient["MaritalStatus"] != DBNull.Value ? LocalPatient["MaritalStatus"].ToString().Trim() : "") != (EaglesoftPatient["MaritalStatus"] != DBNull.Value ? EaglesoftPatient["MaritalStatus"].ToString().Trim() : "") ||
                                                    (LocalPatient["Birth_Date"] != DBNull.Value && LocalPatient["Birth_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["Birth_Date"].ToString().Trim()) : DateTime.Now) != (EaglesoftPatient["Birth_Date"] != DBNull.Value && EaglesoftPatient["Birth_Date"].ToString().Trim() != "" ? Convert.ToDateTime(EaglesoftPatient["Birth_Date"].ToString().Trim()) : DateTime.Now) ||
                                                    (LocalPatient["Email"] != DBNull.Value ? LocalPatient["Email"].ToString().Trim() : "") != (EaglesoftPatient["Email"] != DBNull.Value ? EaglesoftPatient["Email"].ToString().Trim() : "") ||
                                                    (LocalPatient["Mobile"] != DBNull.Value ? LocalPatient["Mobile"].ToString().Trim() : "") != (EaglesoftPatient["Mobile"] != DBNull.Value ? EaglesoftPatient["Mobile"].ToString().Trim() : "") ||
                                                    (LocalPatient["Home_Phone"] != DBNull.Value ? LocalPatient["Home_Phone"].ToString().Trim() : "") != (EaglesoftPatient["Home_Phone"] != DBNull.Value ? EaglesoftPatient["Home_Phone"].ToString().Trim() : "") ||
                                                    (LocalPatient["Work_Phone"] != DBNull.Value ? LocalPatient["Work_Phone"].ToString().Trim() : "") != (EaglesoftPatient["Work_Phone"] != DBNull.Value ? EaglesoftPatient["Work_Phone"].ToString().Trim() : "") ||
                                                    (LocalPatient["Address1"] != DBNull.Value ? LocalPatient["Address1"].ToString().Trim() : "") != (EaglesoftPatient["Address1"] != DBNull.Value ? EaglesoftPatient["Address1"].ToString().Trim() : "") ||
                                                    (LocalPatient["Address2"] != DBNull.Value ? LocalPatient["Address2"].ToString().Trim() : "") != (EaglesoftPatient["Address2"] != DBNull.Value ? EaglesoftPatient["Address2"].ToString().Trim() : "") ||
                                                    (LocalPatient["City"] != DBNull.Value ? LocalPatient["City"].ToString().Trim() : "") != (EaglesoftPatient["City"] != DBNull.Value ? EaglesoftPatient["City"].ToString().Trim() : "") ||
                                                    (LocalPatient["State"] != DBNull.Value ? LocalPatient["State"].ToString().Trim() : "") != (EaglesoftPatient["State"] != DBNull.Value ? EaglesoftPatient["State"].ToString().Trim() : "") ||
                                                    (LocalPatient["Zipcode"] != DBNull.Value ? LocalPatient["Zipcode"].ToString().Trim() : "") != (EaglesoftPatient["Zipcode"] != DBNull.Value ? EaglesoftPatient["Zipcode"].ToString().Trim() : "") ||
                                                    (LocalPatient["ResponsibleParty_Status"] != DBNull.Value ? LocalPatient["ResponsibleParty_Status"].ToString().Trim() : "") != (EaglesoftPatient["ResponsibleParty_Status"] != DBNull.Value ? EaglesoftPatient["ResponsibleParty_Status"].ToString().Trim() : "") ||
                                                    (LocalPatient["CurrentBal"] != DBNull.Value ? LocalPatient["CurrentBal"].ToString().Trim() : "") != (EaglesoftPatient["CurrentBal"] != DBNull.Value ? EaglesoftPatient["CurrentBal"].ToString().Trim() : "") ||
                                                    (LocalPatient["ThirtyDay"] != DBNull.Value ? LocalPatient["ThirtyDay"].ToString().Trim() : "") != (EaglesoftPatient["ThirtyDay"] != DBNull.Value ? EaglesoftPatient["ThirtyDay"].ToString().Trim() : "") ||
                                                    (LocalPatient["SixtyDay"] != DBNull.Value ? LocalPatient["SixtyDay"].ToString().Trim() : "") != (EaglesoftPatient["SixtyDay"] != DBNull.Value ? EaglesoftPatient["SixtyDay"].ToString().Trim() : "") ||
                                                    (LocalPatient["NinetyDay"] != DBNull.Value ? LocalPatient["NinetyDay"].ToString().Trim() : "") != (EaglesoftPatient["NinetyDay"] != DBNull.Value ? EaglesoftPatient["NinetyDay"].ToString().Trim() : "") ||
                                                    (LocalPatient["Over90"] != DBNull.Value ? LocalPatient["Over90"].ToString().Trim() : "") != (EaglesoftPatient["Over90"] != DBNull.Value ? EaglesoftPatient["Over90"].ToString().Trim() : "") ||
                                                    (LocalPatient["FirstVisit_Date"] != DBNull.Value && LocalPatient["FirstVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["FirstVisit_Date"].ToString().Trim()) : DateTime.Now) != (EaglesoftPatient["FirstVisit_Date"] != DBNull.Value && EaglesoftPatient["FirstVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(EaglesoftPatient["FirstVisit_Date"].ToString().Trim()) : DateTime.Now) ||
                                                    (LocalPatient["LastVisit_Date"] != DBNull.Value && LocalPatient["LastVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["LastVisit_Date"].ToString().Trim()) : DateTime.Now) != (EaglesoftPatient["LastVisit_Date"] != DBNull.Value && EaglesoftPatient["LastVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(EaglesoftPatient["LastVisit_Date"].ToString().Trim()) : DateTime.Now) ||
                                                    (LocalPatient["Primary_Insurance"] != DBNull.Value ? LocalPatient["Primary_Insurance"].ToString().Trim() : "") != (EaglesoftPatient["Primary_Insurance"] != DBNull.Value ? EaglesoftPatient["Primary_Insurance"].ToString().Trim() : "") ||
                                                    (LocalPatient["Primary_Insurance_CompanyName"] != DBNull.Value ? LocalPatient["Primary_Insurance_CompanyName"].ToString().Trim() : "") != (EaglesoftPatient["Primary_Insurance_CompanyName"] != DBNull.Value ? EaglesoftPatient["Primary_Insurance_CompanyName"].ToString().Trim() : "") ||
                                                    (LocalPatient["Secondary_Insurance"] != DBNull.Value ? LocalPatient["Secondary_Insurance"].ToString().Trim() : "") != (EaglesoftPatient["Secondary_Insurance"] != DBNull.Value ? EaglesoftPatient["Secondary_Insurance"].ToString().Trim() : "") ||
                                                    (LocalPatient["Secondary_Insurance_CompanyName"] != DBNull.Value ? LocalPatient["Secondary_Insurance_CompanyName"].ToString().Trim() : "") != (EaglesoftPatient["Secondary_Insurance_CompanyName"] != DBNull.Value ? EaglesoftPatient["Secondary_Insurance_CompanyName"].ToString().Trim() : "") ||
                                                    (LocalPatient["Guar_ID"] != DBNull.Value ? LocalPatient["Guar_ID"].ToString().Trim() : "") != (EaglesoftPatient["Guar_ID"] != DBNull.Value ? EaglesoftPatient["Guar_ID"].ToString().Trim() : "") ||
                                                    (LocalPatient["Pri_Provider_ID"] != DBNull.Value ? LocalPatient["Pri_Provider_ID"].ToString().Trim() : "") != (EaglesoftPatient["Pri_Provider_ID"] != DBNull.Value ? EaglesoftPatient["Pri_Provider_ID"].ToString().Trim() : "") ||
                                                    (LocalPatient["Sec_Provider_ID"] != DBNull.Value ? LocalPatient["Sec_Provider_ID"].ToString().Trim() : "") != (EaglesoftPatient["Sec_Provider_ID"] != DBNull.Value ? EaglesoftPatient["Sec_Provider_ID"].ToString().Trim() : "") ||
                                                    (LocalPatient["ReceiveSms"] != DBNull.Value ? LocalPatient["ReceiveSms"].ToString().Trim() : "") != (EaglesoftPatient["ReceiveSms"] != DBNull.Value ? EaglesoftPatient["ReceiveSms"].ToString().Trim() : "") ||
                                                    (LocalPatient["ReceiveEmail"] != DBNull.Value ? LocalPatient["ReceiveEmail"].ToString().Trim() : "") != (EaglesoftPatient["ReceiveEmail"] != DBNull.Value ? EaglesoftPatient["ReceiveEmail"].ToString().Trim() : "") ||
                                                    (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["nextvisit_date"].ToString().Trim()) : DateTime.Now) != (EaglesoftPatient["nextvisit_date"] != DBNull.Value && EaglesoftPatient["nextvisit_date"].ToString().Trim() != "" ? Convert.ToDateTime(EaglesoftPatient["nextvisit_date"].ToString().Trim()) : DateTime.Now) ||
                                                    (LocalPatient["due_date"] != DBNull.Value && LocalPatient["due_date"].ToString().Trim() != "" ? LocalPatient["due_date"].ToString().Trim() : "") != (EaglesoftPatient["due_date"] != DBNull.Value && EaglesoftPatient["due_date"].ToString().Trim() != "" ? EaglesoftPatient["due_date"].ToString().Trim() : "") ||
                                                    (LocalPatient["remaining_benefit"] != DBNull.Value ? LocalPatient["remaining_benefit"].ToString().Trim() : "") != (EaglesoftPatient["remaining_benefit"] != DBNull.Value ? EaglesoftPatient["remaining_benefit"].ToString().Trim() : "") ||
                                                    (LocalPatient["collect_payment"] != DBNull.Value ? LocalPatient["collect_payment"].ToString().Trim() : "") != (EaglesoftPatient["collect_payment"] != DBNull.Value ? EaglesoftPatient["collect_payment"].ToString().Trim() : "") ||
                                                    (LocalPatient["preferred_name"] != DBNull.Value ? LocalPatient["preferred_name"].ToString().Trim() : "") != (EaglesoftPatient["preferred_name"] != DBNull.Value ? EaglesoftPatient["preferred_name"].ToString().Trim() : "") ||
                                                    (LocalPatient["used_benefit"] != DBNull.Value ? LocalPatient["used_benefit"].ToString().Trim() : "") != (EaglesoftPatient["used_benefit"] != DBNull.Value ? EaglesoftPatient["used_benefit"].ToString().Trim() : "") ||
                                                    (LocalPatient["Secondary_Ins_Subscriber_ID"] != DBNull.Value ? LocalPatient["Secondary_Ins_Subscriber_ID"].ToString().Trim() : "") != (EaglesoftPatient["Secondary_Ins_Subscriber_ID"] != DBNull.Value ? EaglesoftPatient["Secondary_Ins_Subscriber_ID"].ToString().Trim() : "") ||
                                                    (LocalPatient["Primary_Ins_Subscriber_ID"] != DBNull.Value ? LocalPatient["Primary_Ins_Subscriber_ID"].ToString().Trim() : "") != (EaglesoftPatient["Primary_Ins_Subscriber_ID"] != DBNull.Value ? EaglesoftPatient["Primary_Ins_Subscriber_ID"].ToString().Trim() : "") ||
                                                    (LocalPatient["EHR_Status"] != DBNull.Value ? LocalPatient["EHR_Status"].ToString().Trim() : "") != (EaglesoftPatient["EHR_Status"] != DBNull.Value ? EaglesoftPatient["EHR_Status"].ToString().Trim() : "") ||
                                                    (LocalPatient["ReceiveVoiceCall"] != DBNull.Value ? LocalPatient["ReceiveVoiceCall"].ToString().Trim() : "") != (EaglesoftPatient["ReceiveVoiceCall"] != DBNull.Value ? EaglesoftPatient["ReceiveVoiceCall"].ToString().Trim() : "") ||
                                                    (LocalPatient["PreferredLanguage"] != DBNull.Value ? LocalPatient["PreferredLanguage"].ToString().Trim() : "") != (EaglesoftPatient["PreferredLanguage"] != DBNull.Value ? EaglesoftPatient["PreferredLanguage"].ToString().Trim() : "") ||
                                                    (LocalPatient["Patient_Note"] != DBNull.Value ? LocalPatient["Patient_Note"].ToString().Trim() : "") != (EaglesoftPatient["Patient_Note"] != DBNull.Value ? EaglesoftPatient["Patient_Note"].ToString().Trim() : "") ||
                                                    (LocalPatient["encrypted_social_security"] != DBNull.Value ? LocalPatient["encrypted_social_security"].ToString().Trim() : "") != (EaglesoftPatient["encrypted_social_security"] != DBNull.Value ? EaglesoftPatient["encrypted_social_security"].ToString().Trim() : "") ||
                                                    (LocalPatient["driverlicense"] != DBNull.Value ? LocalPatient["driverlicense"].ToString().Trim() : "") != (EaglesoftPatient["driverlicense"] != DBNull.Value ? EaglesoftPatient["driverlicense"].ToString().Trim() : "") ||
                                                    (LocalPatient["groupid"] != DBNull.Value ? LocalPatient["groupid"].ToString().Trim() : "") != (EaglesoftPatient["groupid"] != DBNull.Value ? EaglesoftPatient["groupid"].ToString().Trim() : "") ||
                                                    (LocalPatient["emergencycontactId"] != DBNull.Value ? LocalPatient["emergencycontactId"].ToString().Trim() : "") != (EaglesoftPatient["emergencycontactId"] != DBNull.Value ? EaglesoftPatient["emergencycontactId"].ToString().Trim() : "") ||
                                                    (LocalPatient["EmergencyContact_First_Name"] != DBNull.Value ? LocalPatient["EmergencyContact_First_Name"].ToString().Trim() : "") != (EaglesoftPatient["EmergencyContact_First_Name"] != DBNull.Value ? EaglesoftPatient["EmergencyContact_First_Name"].ToString().Trim() : "") ||
                                                    (LocalPatient["EmergencyContact_Last_Name"] != DBNull.Value ? LocalPatient["EmergencyContact_Last_Name"].ToString().Trim() : "") != (EaglesoftPatient["EmergencyContact_Last_Name"] != DBNull.Value ? EaglesoftPatient["EmergencyContact_Last_Name"].ToString().Trim() : "") ||
                                                    (LocalPatient["emergencycontactnumber"] != DBNull.Value ? LocalPatient["emergencycontactnumber"].ToString().Trim() : "") != (EaglesoftPatient["emergencycontactnumber"] != DBNull.Value ? EaglesoftPatient["emergencycontactnumber"].ToString().Trim() : "") ||
                                                    (LocalPatient["school"] != DBNull.Value ? LocalPatient["school"].ToString().Trim() : "") != (EaglesoftPatient["school"] != DBNull.Value ? EaglesoftPatient["school"].ToString().Trim() : "") ||
                                                    (LocalPatient["employer"] != DBNull.Value ? LocalPatient["employer"].ToString().Trim() : "") != (EaglesoftPatient["employer"] != DBNull.Value ? EaglesoftPatient["employer"].ToString().Trim() : "") ||
                                                    (LocalPatient["spouseId"] != DBNull.Value ? LocalPatient["spouseId"].ToString().Trim() : "") != (EaglesoftPatient["spouseId"] != DBNull.Value ? EaglesoftPatient["spouseId"].ToString().Trim() : "") ||
                                                    (LocalPatient["responsiblepartyId"] != DBNull.Value ? LocalPatient["responsiblepartyId"].ToString().Trim() : "") != (EaglesoftPatient["responsiblepartyId"] != DBNull.Value ? EaglesoftPatient["responsiblepartyId"].ToString().Trim() : "") ||
                                                    (LocalPatient["RespEncrypted_social_security"] != DBNull.Value ? LocalPatient["RespEncrypted_social_security"].ToString().Trim() : "") != (EaglesoftPatient["RespEncrypted_social_security"] != DBNull.Value ? EaglesoftPatient["RespEncrypted_social_security"].ToString().Trim() : "") ||
                                                    (LocalPatient["responsiblepartybirthdate"] != DBNull.Value && LocalPatient["responsiblepartybirthdate"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["responsiblepartybirthdate"].ToString().Trim()) : DateTime.Now) != (EaglesoftPatient["responsiblepartybirthdate"] != DBNull.Value && EaglesoftPatient["responsiblepartybirthdate"].ToString().Trim() != "" ? Convert.ToDateTime(EaglesoftPatient["responsiblepartybirthdate"].ToString().Trim()) : DateTime.Now) ||
                                                    (LocalPatient["Spouse_First_Name"] != DBNull.Value ? LocalPatient["Spouse_First_Name"].ToString().Trim() : "") != (EaglesoftPatient["Spouse_First_Name"] != DBNull.Value ? EaglesoftPatient["Spouse_First_Name"].ToString().Trim() : "") ||
                                                    (LocalPatient["Spouse_Last_Name"] != DBNull.Value ? LocalPatient["Spouse_Last_Name"].ToString().Trim() : "") != (EaglesoftPatient["Spouse_Last_Name"] != DBNull.Value ? EaglesoftPatient["Spouse_Last_Name"].ToString().Trim() : "") ||
                                                    (LocalPatient["ResponsibleParty_First_Name"] != DBNull.Value ? LocalPatient["ResponsibleParty_First_Name"].ToString().Trim() : "") != (EaglesoftPatient["ResponsibleParty_First_Name"] != DBNull.Value ? EaglesoftPatient["ResponsibleParty_First_Name"].ToString().Trim() : "") ||
                                                    (LocalPatient["ResponsibleParty_Last_Name"] != DBNull.Value ? LocalPatient["ResponsibleParty_Last_Name"].ToString().Trim() : "") != (EaglesoftPatient["ResponsibleParty_Last_Name"] != DBNull.Value ? EaglesoftPatient["ResponsibleParty_Last_Name"].ToString().Trim() : "") ||
                                                    (LocalPatient["Prim_Ins_Company_Phonenumber"] != DBNull.Value ? LocalPatient["Prim_Ins_Company_Phonenumber"].ToString().Trim() : "") != (EaglesoftPatient["Prim_Ins_Company_Phonenumber"] != DBNull.Value ? EaglesoftPatient["Prim_Ins_Company_Phonenumber"].ToString().Trim() : "") ||
                                                    (LocalPatient["Sec_Ins_Company_Phonenumber"] != DBNull.Value ? LocalPatient["Sec_Ins_Company_Phonenumber"].ToString().Trim() : "") != (EaglesoftPatient["Sec_Ins_Company_Phonenumber"] != DBNull.Value ? EaglesoftPatient["Sec_Ins_Company_Phonenumber"].ToString().Trim() : "") //||
                                                    //(LocalPatient["Is_deleted"] != DBNull.Value ? Convert.ToInt32(LocalPatient["Is_deleted"]).ToString().Trim() : "0") != (EaglesoftPatient["Is_deleted"] != DBNull.Value ? EaglesoftPatient["Is_deleted"].ToString().Trim() : "0")
                                                    select EaglesoftPatient).ToList();

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

                            var itemToBeDeleted = (from LocalPatient in dtLocalPatient.AsEnumerable()
                                                   join EaglesoftPatient in dtEaglesoftPatient.AsEnumerable()
                                                   on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                   equals EaglesoftPatient["Patient_EHR_ID"].ToString().Trim() + "_" + EaglesoftPatient["Clinic_Number"].ToString().Trim()
                                                   //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                                   //equals new { PatID = EaglesoftPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = EaglesoftPatient["Clinic_Number"].ToString().Trim() }
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
                            
                            if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                            {
                                bool status = false;                                
                                if (dtSaveRecords.Rows.Count > 0)
                                {
                                    SynchEaglesoftBAL.DecryptSSN(ref dtSaveRecords);
                                    status = SynchEaglesoftBAL.Save_Patient_Eaglesoft_To_Local_New(dtSaveRecords, "0", Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), false);
                                }
                                else
                                {
                                    status = true;
                                }

                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                    ObjGoalBase.WriteToSyncLogFile("Patient New Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    IsGetParientRecordDone = true;

                                    SynchDataLiveDB_Push_Patient();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Patient New Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                            else
                            {
                                ObjGoalBase.WriteToSyncLogFile("Patient New Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                                IsGetParientRecordDone = true;
                            }
                        }
                    }
                    IsPatientSyncedFirstTime = true;
                    Is_synched_Patient = false;
                    SynchDataEagleSoft_PatientStatus();
                    SynchDataEagleSoft_PatientImages();
                    SynchDataEagleSoft_PatientDisease();
                    SynchDataEagleSoft_PatientMedication("");
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient New Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                    Is_synched_Patient = false;
                    throw ex;
                }
            }
        }

        public void SynchDataEagleSoft_PatientStatus()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftPatientStatus = SynchEaglesoftBAL.GetEaglesoftPatientStatusData("", Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            if (dtEaglesoftPatientStatus != null && dtEaglesoftPatientStatus.Rows.Count > 0)
                            {
                                SynchLocalBAL.UpdatePatient_Status(dtEaglesoftPatientStatus, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            }
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientStatus");
                            ObjGoalBase.WriteToSyncLogFile("PatientStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_PatientStatus();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);

            }
        }

        public void SynchDataEagleSoft_PatientImages()
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
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEagleSoftPatientImages = SynchEaglesoftBAL.GetEaglesoftPatientImagesData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEagleSoftPatientImages.Columns.Add("InsUptDlt", typeof(int));
                            dtEagleSoftPatientImages.Columns.Add("SourceLocation", typeof(string));
                            dtEagleSoftPatientImages.Columns["InsUptDlt"].DefaultValue = 0;
                            DataTable dtLocalPatientImages = SynchLocalBAL.GetLocalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString());
                            string DocPath = Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString();
                            foreach (DataRow dtDtxRow in dtEagleSoftPatientImages.Rows)
                            {
                                if (DocPath != string.Empty || DocPath != "")
                                {
                                    dtDtxRow["SourceLocation"] = DocPath.Replace("\\Documents\\patient", "\\Images") + "\\" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                                }
                                else if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                                {
                                    dtDtxRow["SourceLocation"] = @"C:\EagleSoft\Data\Images\" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                                }
                                else
                                {
                                    dtDtxRow["SourceLocation"] = Utility.EHRDocPath.Replace("\\Documents\\patient", "\\Images") + "\\" + dtDtxRow["Patient_EHR_ID"].ToString().Trim() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
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
                                DataRow[] row = dtEagleSoftPatientImages.Copy().Select("Patient_EHR_ID = '" + dtDtlRow["Patient_EHR_ID"].ToString().Trim() + "' ");
                                if (row.Length <= 0)
                                {
                                    if (!Convert.ToBoolean(dtDtlRow["Is_Deleted"]))
                                    {
                                        DataRow ApptDtldr = dtEagleSoftPatientImages.NewRow();
                                        ApptDtldr["Patient_EHR_ID"] = dtDtlRow["Patient_EHR_ID"].ToString().Trim();
                                        ApptDtldr["Patient_Images_EHR_ID"] = dtDtlRow["Patient_Images_EHR_ID"].ToString().Trim();
                                        ApptDtldr["Image_EHR_Name"] = dtDtlRow["Image_EHR_Name"].ToString().Trim();
                                        ApptDtldr["Clinic_Number"] = dtDtlRow["Clinic_Number"].ToString().Trim();
                                        ApptDtldr["Service_Install_Id"] = dtDtlRow["Service_Install_Id"].ToString().Trim();
                                        ApptDtldr["Is_Deleted"] = 1;
                                        ApptDtldr["InsUptDlt"] = 3;
                                        dtEagleSoftPatientImages.Rows.Add(ApptDtldr);
                                    }

                                }
                            }

                            dtEagleSoftPatientImages.AcceptChanges();
                            bool status = false;
                            DataTable dtSaveRecords = dtEagleSoftPatientImages.Clone();
                            if (dtEagleSoftPatientImages.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEagleSoftPatientImages.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                                status = SynchLocalBAL.Save_PatientProfileImage_EHR_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            }

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Images");
                                ObjGoalBase.WriteToSyncLogFile("PatientImage Sync (" + Utility.Application_Name + " to Local Database) Successfully.");


                            }
                            SynchDataLiveDB_Push_PatientImage(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }
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
            bwSynchEaglesoft_RecallType = new BackgroundWorker();
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

        private void CallSynch_RecallType()
        {
            if (!Is_synched_RecallType && Utility.IsApplicationIdleTimeOff)
            {
                Is_synched_RecallType = true;

                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftRecallType = SynchEaglesoftBAL.GetEaglesoftRecallTypeData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEaglesoftRecallType.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            foreach (DataRow dtDtxRow in dtEaglesoftRecallType.Rows)
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
                                DataRow[] row = dtEaglesoftRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                                if (row.Length > 0)
                                { }
                                else
                                {
                                    DataRow BlcOptDtldr = dtEaglesoftRecallType.NewRow();
                                    BlcOptDtldr["RecallType_EHR_ID"] = dtDtxRow["RecallType_EHR_ID"].ToString().Trim();
                                    BlcOptDtldr["InsUptDlt"] = 3;
                                    dtEaglesoftRecallType.Rows.Add(BlcOptDtldr);
                                }
                            }
                            dtEaglesoftRecallType.AcceptChanges();

                            if (dtEaglesoftRecallType != null && dtEaglesoftRecallType.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_RecallType_Eaglesoft_To_Local(dtEaglesoftRecallType, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
                                    ObjGoalBase.WriteToSyncLogFile("RecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_RecallType();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[RecallType Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
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

        #endregion

        #region Synch User
        private void fncSynchDataEagleSoft_User()
        {
            InitBgWorkerEagleSoft_User();
            InitBgTimerEagleSoft_User();
        }

        private void InitBgTimerEagleSoft_User()
        {
            timerSynchEagleSoft_User = new System.Timers.Timer();
            this.timerSynchEagleSoft_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchEagleSoft_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEagleSoft_User_Tick);
            timerSynchEagleSoft_User.Enabled = true;
            timerSynchEagleSoft_User.Start();
        }

        private void timerSynchEagleSoft_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEagleSoft_User.Enabled = false;
                MethodForCallSynchOrderEagleSoft_User();
            }
        }

        private void MethodForCallSynchOrderEagleSoft_User()
        {
            System.Threading.Thread procThreadmainEagleSoft_User = new System.Threading.Thread(this.CallSyncOrderTableEagleSoft_User);
            procThreadmainEagleSoft_User.Start();
        }

        private void CallSyncOrderTableEagleSoft_User()
        {
            if (bwSynchEagleSoft_User.IsBusy != true)
            {
                bwSynchEagleSoft_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void InitBgWorkerEagleSoft_User()
        {
            bwSynchEagleSoft_User = new BackgroundWorker();
            bwSynchEagleSoft_User.WorkerReportsProgress = true;
            bwSynchEagleSoft_User.WorkerSupportsCancellation = true;
            bwSynchEagleSoft_User.DoWork += new DoWorkEventHandler(bwSynchEagleSoft_User_DoWork);
            bwSynchEagleSoft_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEagleSoft_User_RunWorkerCompleted);
        }

        private void bwSynchEagleSoft_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEagleSoft_User.Enabled = true;
        }

        private void bwSynchEagleSoft_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEagleSoft_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEagleSoft_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SynchDataEagleSoft_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtTrackerUser = SynchEaglesoftBAL.GetEaglesoftUserData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
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

                            dtTrackerUser.AcceptChanges();

                            if (dtTrackerUser != null && dtTrackerUser.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_User_Eaglesoft_To_Local(dtTrackerUser, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
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
            bwSynchEaglesoft_ApptStatus = new BackgroundWorker();
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
            if (!Is_synched_ApptStatus && Utility.IsApplicationIdleTimeOff)
            {
                Is_synched_ApptStatus = true;
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftAppointmentStatus = new DataTable();
                            dtEaglesoftAppointmentStatus.Clear();
                            dtEaglesoftAppointmentStatus.Columns.Add("ApptStatus_EHR_ID", typeof(int));
                            dtEaglesoftAppointmentStatus.Columns.Add("ApptStatus_Name", typeof(string));
                            dtEaglesoftAppointmentStatus.Columns.Add("ApptStatus_Type", typeof(string));
                            dtEaglesoftAppointmentStatus.Columns.Add("InsUptDlt", typeof(int));
                            dtEaglesoftAppointmentStatus.Rows.Add(0, "Unconfirmed", "normal");
                            dtEaglesoftAppointmentStatus.Rows.Add(1, "Confirmed", "normal");
                            dtEaglesoftAppointmentStatus.Rows.Add(2, "Sent Email", "normal");
                            dtEaglesoftAppointmentStatus.Rows.Add(3, "Left Message", "normal");
                            dtEaglesoftAppointmentStatus.Rows.Add(4, "No Answer", "normal");
                            dtEaglesoftAppointmentStatus.Rows.Add(5, "Phone Busy", "normal");
                            dtEaglesoftAppointmentStatus.Rows.Add(6, "Waiting For Callback", "normal");
                            dtEaglesoftAppointmentStatus.Rows.Add(7, "Other", "normal");
                            dtEaglesoftAppointmentStatus.Rows.Add(8, "Mark As Walked Out", "normal");

                            dtEaglesoftAppointmentStatus.AcceptChanges();

                            DataTable dtLocalAppointmentStatus = SynchLocalBAL.GetLocalAppointmentStatusData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            foreach (DataRow dtDtxRow in dtEaglesoftAppointmentStatus.Rows)
                            {
                                DataRow[] row = dtLocalAppointmentStatus.Copy().Select("ApptStatus_EHR_ID = '" + dtDtxRow["ApptStatus_EHR_ID"] + "'");
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

                            dtEaglesoftAppointmentStatus.AcceptChanges();

                            if (dtEaglesoftAppointmentStatus != null && dtEaglesoftAppointmentStatus.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_AppointmentStatus_Eaglesoft_To_Local(dtEaglesoftAppointmentStatus, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                                    ObjGoalBase.WriteToSyncLogFile("AppointmentStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_ApptStatus();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[AppointmentStatus Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
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

        #region Synch Holidays

        private void fncSynchDataEaglesoft_Holidays()
        {
            //  SynchDataEaglesoft_Holidays();
            InitBgWorkerEaglesoft_Holidays();
            InitBgTimerEaglesoft_Holidays();
        }

        private void InitBgTimerEaglesoft_Holidays()
        {
            timerSynchEaglesoft_Holidays = new System.Timers.Timer();
            this.timerSynchEaglesoft_Holidays.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchEaglesoft_Holidays.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_Holidays_Tick);
            timerSynchEaglesoft_Holidays.Enabled = true;
            timerSynchEaglesoft_Holidays.Start();
            timerSynchEaglesoft_Holidays_Tick(null, null);
        }

        private void InitBgWorkerEaglesoft_Holidays()
        {
            bwSynchEaglesoft_Holidays = new BackgroundWorker();
            bwSynchEaglesoft_Holidays.WorkerReportsProgress = true;
            bwSynchEaglesoft_Holidays.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_Holidays.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_Holidays_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_Holidays.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_Holidays_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_Holidays_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_Holidays.Enabled = false;
                MethodForCallSynchOrderEaglesoft_Holidays();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_Holidays()
        {
            System.Threading.Thread procThreadmainEaglesoft_Holidays = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_Holidays);
            procThreadmainEaglesoft_Holidays.Start();
        }

        public void CallSyncOrderTableEaglesoft_Holidays()
        {
            if (bwSynchEaglesoft_Holidays.IsBusy != true)
            {
                bwSynchEaglesoft_Holidays.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_Holidays_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_Holidays.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_Holidays();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_Holidays_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_Holidays.Enabled = true;
        }

        public void SynchDataEaglesoft_Holidays()
        {
            CallSynch_Eaglesoft_Holidays();

        }

        private void CallSynch_Eaglesoft_Holidays()
        {
            if (!Is_synched_Holidays && Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    Is_synched_Holidays = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftHoliday = SynchEaglesoftBAL.GetEaglesoftHolidaysData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEaglesoftHoliday.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalHoliday = SynchLocalBAL.GetLocalEaglesoftHolidayData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            dtEaglesoftHoliday = CommonUtility.AddHolidays(dtEaglesoftHoliday, dtLocalHoliday, "SchedDate", "comment", "H_EHR_ID");

                            foreach (DataRow dtDtxRow in dtEaglesoftHoliday.Rows)
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
                            }

                            foreach (DataRow dtDtxRow in dtLocalHoliday.Rows)
                            {
                                DataRow[] row = dtEaglesoftHoliday.Copy().Select("SchedDate = '" + dtDtxRow["SchedDate"] + "'");
                                if (row.Length <= 0)
                                {
                                    DataRow ApptDtldr = dtEaglesoftHoliday.NewRow();
                                    ApptDtldr["SchedDate"] = dtDtxRow["SchedDate"].ToString().Trim();
                                    ApptDtldr["InsUptDlt"] = 3;
                                    dtEaglesoftHoliday.Rows.Add(ApptDtldr);
                                }
                            }

                            dtEaglesoftHoliday.AcceptChanges();

                            if (dtEaglesoftHoliday != null && dtEaglesoftHoliday.Rows.Count > 0)
                            {
                                //dtEaglesoftHoliday = CommonUtility.CreateHolidayEHRId(dtEaglesoftHoliday);
                                bool status = SynchEaglesoftBAL.Save_Holidays_Eaglesoft_To_Local(dtEaglesoftHoliday, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");
                                    ObjGoalBase.WriteToSyncLogFile("Holiday Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday_Push");
                                    SynchDataLiveDB_Push_Holiday();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[AppointmentStatus Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                        }
                    }

                    Is_synched_Holidays = false;
                }
                catch (Exception ex)
                {
                    Is_synched_Holidays = false;
                    ObjGoalBase.WriteToErrorLogFile("[Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Create Appointment

        private void fncSynchDataLocalToEagleSoft_Appointment()
        {
            InitBgWorkerLocalToEagleSoft_Appointment();
            InitBgTimerLocalToEagleSoft_Appointment();
        }

        private void InitBgTimerLocalToEagleSoft_Appointment()
        {
            timerSynchLocalToEagleSoft_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToEagleSoft_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToEagleSoft_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToEagleSoft_Appointment_Tick);
            timerSynchLocalToEagleSoft_Appointment.Enabled = true;
            timerSynchLocalToEagleSoft_Appointment.Start();
            timerSynchLocalToEagleSoft_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToEagleSoft_Appointment()
        {
            bwSynchLocalToEagleSoft_Appointment = new BackgroundWorker();
            bwSynchLocalToEagleSoft_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToEagleSoft_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToEagleSoft_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToEagleSoft_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToEagleSoft_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToEagleSoft_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToEagleSoft_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToEagleSoft_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToEagleSoft_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToEagleSoft_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToEagleSoft_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToEagleSoft_Appointment);
            procThreadmainLocalToEagleSoft_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToEagleSoft_Appointment()
        {
            if (bwSynchLocalToEagleSoft_Appointment.IsBusy != true)
            {
                bwSynchLocalToEagleSoft_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToEagleSoft_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToEagleSoft_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToEagleSoft_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToEagleSoft_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToEagleSoft_Appointment.Enabled = true;
        }

        public void SynchDataLocalToEagleSoft_Appointment()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            DataTable dtEagleSoftPatient = SynchEaglesoftBAL.GetPatientListFromEagleSoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            DataTable dtIdelProv = SynchEaglesoftBAL.GetEagleSoftIdelProvider(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                            string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                            string tmpApptProv = "";
                            string tmpPatient_id = "0";
                            string tmpPatient_Gur_id = "0";
                            Int64 tmpAppt_EHR_id = 0;
                            //int tmpNewPatient = 1;

                            string tmpLastName = "";
                            string tmpFirstName = "";

                            string TmpWebPatientName = "";
                            string TmpWebReversePatientName = "";

                            if (dtWebAppointment != null)
                            {
                                if (dtWebAppointment.Rows.Count > 0)
                                {
                                    Utility.CheckEntryUserLoginIdExist();
                                }
                            }

                            foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                            {
                                try
                                {
                                    tmpPatient_id = "0";
                                    tmpPatient_Gur_id = "0";
                                    tmpAppt_EHR_id = 0;
                                    TmpWebPatientName = "";
                                    TmpWebReversePatientName = "";

                                    Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebReversePatientName);

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
                                    DataTable dtBookOperatoryApptWiseDateTime = SynchEaglesoftBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
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
                                                    if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["start_Time"].ToString()))
                                                        && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["End_Time"].ToString())))
                                                    {
                                                        IsConflict = true;
                                                        break;
                                                    }
                                                    else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["start_Time"].ToString()))
                                                        && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["End_Time"].ToString())))
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
                                    if (tmpIdealOperatory == 0)
                                    {
                                        DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appointment_EHR_id = " + appointment_EHR_id).CopyToDataTable();

                                        bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[j]["Location_ID"].ToString());
                                    }
                                    else
                                    {

                                        #region Set Patient
                                        tmpPatient_id = dtDtxRow["Patient_EHR_Id"].ToString();

                                        if (tmpPatient_id == "0" || tmpPatient_id == "")
                                        {
                                            DateTime DOB;
                                            DOB = new DateTime();
                                            if (!string.IsNullOrEmpty(dtDtxRow["birth_date"].ToString().Trim()))
                                            {
                                                DOB = Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim());
                                            }
                                            tmpPatient_id = GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtEagleSoftPatient, tmpPatient_id, dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), dtDtxRow["Clinic_Number"].ToString(), DOB, dtDtxRow["Provider_EHR_ID"].ToString());
                                            // DataRow[] row = dtEagleSoftPatient.Copy().Select(" Mobile = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR Work_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR Home_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "'");
                                            //if (row.Length > 0)
                                            //{
                                            //    for (int i = 0; i < row.Length; i++)
                                            //    {
                                            //        if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                            //        {
                                            //            tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                                            //        }
                                            //        else if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebReversePatientName.ToString().ToLower().Trim())
                                            //        {
                                            //            tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                                            //        }
                                            //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                            //        {
                                            //            tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                                            //        }
                                            //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == dtDtxRow["First_Name"].ToString().Trim().ToLower())
                                            //        {
                                            //            tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                                            //        }
                                            //        else if (row[i]["FirstName"].ToString().ToLower().Trim() == (TmpWebPatientName.ToString().IndexOf(" ") > 0 ? TmpWebPatientName.Substring(0, TmpWebPatientName.ToString().IndexOf(" ")).ToLower() : TmpWebPatientName))
                                            //        {
                                            //            tmpPatient_id = row[i]["Patient_EHR_ID"].ToString();
                                            //        }
                                            //        if (tmpPatient_id != "0")
                                            //        {
                                            //            break;
                                            //        }
                                            //    }

                                            //    tmpPatient_Gur_id = row[0]["responsible_party"].ToString();
                                            //}
                                        }
                                        //if (tmpPatient_id == "0")
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

                                        //    tmpPatient_id = SynchEaglesoftBAL.Save_Patient_Local_To_EagleSoft(tmpLastName.Trim(),
                                        //                                                                        tmpFirstName,
                                        //                                                                        dtDtxRow["MI"].ToString().Trim(),
                                        //                                                                        dtDtxRow["Mobile_Contact"].ToString().Trim(),
                                        //                                                                        dtDtxRow["Email"].ToString().Trim(),
                                        //                                                                        tmpApptProv,
                                        //                                                                        dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                        //                                                                        tmpPatient_Gur_id.ToString(), tmpIdealOperatory,
                                        //                                                                                    dtDtxRow["Birth_Date"].ToString().Trim(),
                                        //                                                                                    Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                                        //}
                                        #endregion

                                        if (tmpPatient_id != "0")
                                        {

                                            tmpAppt_EHR_id = SynchEaglesoftBAL.Save_Appointment_Local_To_EagleSoft(TmpWebPatientName, tmpStartTime, tmpEndTime, tmpPatient_id.ToString(), tmpIdealOperatory.ToString(), "1", dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                                               Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), tmpApptProv, dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "0", dtDtxRow["comment"].ToString().Trim(),
                                                                                                                               false, false, false, false, (dtDtxRow["appt_treatmentcode"].ToString()), Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                                            if (tmpAppt_EHR_id > 0)
                                            {
                                                bool isApptId_Update = SynchEaglesoftBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                            }
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    Utility.WriteToErrorLogFromAll("Error in appt booking  " + ex.Message);
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

        #region Sync Patient Document

        public void SynchDataLocalToEagleSoft_Patient_Document()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
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

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            try
                            {
                                GetPatientDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                GetPatientDocument_New(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                SynchEaglesoftBAL.Save_Document_in_EagleSoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());


                                #region Treatment DOc
                                SynchEaglesoftBAL.Save_Treatment_Document_in_EagleSoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());
                                #region change status as treatment doc impotred Completed
                                DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                                if (statusCompleted.Rows.Count > 0)
                                {
                                    Change_Status_TreatmentDoc(statusCompleted, "Completed");
                                }
                                #endregion
                                #endregion

                                #region Insurance Carrier
                                SynchEaglesoftBAL.Save_InsuranceCarrier_Document_in_EagleSoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());
                                #region change status as Insurance Carrier doc impotred Completed
                                DataTable InsuranceCarrierstatusCompleted = SynchLocalBAL.ChangeStatusForInsuranceCarrierDoc("Completed");
                                if (InsuranceCarrierstatusCompleted.Rows.Count > 0)
                                {
                                    Change_Status_InsuranceCarrierDoc(InsuranceCarrierstatusCompleted, "Completed");
                                }
                                #endregion
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                                // throw;
                            }

                            ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
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

        private void fncSynchDataLocalToEagleSoft_Patient_Form()
        {
            InitBgWorkerLocalToEagleSoft_Patient_Form();
            InitBgTimerLocalToEagleSoft_Patient_Form();
        }

        private void InitBgTimerLocalToEagleSoft_Patient_Form()
        {
            timerSynchLocalToEagleSoft_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToEagleSoft_Patient_Form.Interval = 1000 * GoalBase.intervalEHRSynch_PatientForm;
            this.timerSynchLocalToEagleSoft_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToEagleSoft_Patient_Form_Tick);
            timerSynchLocalToEagleSoft_Patient_Form.Enabled = true;
            timerSynchLocalToEagleSoft_Patient_Form.Start();
            timerSynchLocalToEagleSoft_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToEagleSoft_Patient_Form()
        {
            bwSynchLocalToEagleSoft_Patient_Form = new BackgroundWorker();
            bwSynchLocalToEagleSoft_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToEagleSoft_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToEagleSoft_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToEagleSoft_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToEagleSoft_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToEagleSoft_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToEagleSoft_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToEagleSoft_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToEagleSoft_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToEagleSoft_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToEagleSoft_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToEagleSoft_Patient_Form);
            procThreadmainLocalToEagleSoft_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToEagleSoft_Patient_Form()
        {
            if (bwSynchLocalToEagleSoft_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToEagleSoft_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToEagleSoft_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToEagleSoft_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToEagleSoft_Patient_Form();
                //SynchDataLocalToEagleSoft_Patient_Payment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToEagleSoft_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToEagleSoft_Patient_Form.Enabled = true;
        }

        public void SynchDataLocalToEagleSoft_Patient_Form()
        {
            try
            {

                if (Utility.IsApplicationIdleTimeOff)
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

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            DataColumn dcCol = new DataColumn();
                            dcCol.ColumnName = "TableName";
                            dcCol.DefaultValue = "Patient";
                            dtWebPatient_Form.Columns.Add(dcCol);
                            // Add Columns Name As TableName
                            // default value  = 'patient'
                            
                            foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                            {

                                if (dtDtxRow["ehrfield"].ToString().Trim() == "first_name")
                                {
                                    dtDtxRow["ehrfield"] = "first_name";
                                }
                                // if else ehrfield name = 'employer'
                                //dtDtxRow["TableName"] = "employer"
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "last_name")
                                {
                                    dtDtxRow["ehrfield"] = "last_name";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "mobile")
                                {
                                    dtDtxRow["ehrfield"] = "cell_phone";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_one")
                                {
                                    dtDtxRow["ehrfield"] = "Address_1";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_two")
                                {
                                    dtDtxRow["ehrfield"] = "Address_2";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "birth_date")
                                {
                                    dtDtxRow["ehrfield"] = "Birth_Date";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "city")
                                {
                                    dtDtxRow["ehrfield"] = "City";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "email")
                                {
                                    dtDtxRow["ehrfield"] = "email_address";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "home_phone")
                                {
                                    dtDtxRow["ehrfield"] = "Home_Phone";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "marital_status")
                                {
                                    dtDtxRow["ehrfield"] = "marital_status";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "middle_name")
                                {
                                    dtDtxRow["ehrfield"] = "middle_initial";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "preferred_name")
                                {
                                    dtDtxRow["ehrfield"] = "preferred_name";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "pri_provider_id")
                                {
                                    dtDtxRow["ehrfield"] = "preferred_dentist";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance")
                                {
                                    dtDtxRow["ehrfield"] = "";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                                {
                                    dtDtxRow["ehrfield"] = "prim_member_id";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance_companyname")
                                {
                                    dtDtxRow["ehrfield"] = "";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_email")
                                {
                                    dtDtxRow["ehrfield"] = "receive_email";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_sms")
                                {
                                    dtDtxRow["ehrfield"] = "receives_sms";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "salutation")
                                {
                                    dtDtxRow["ehrfield"] = "Salutation";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "sec_provider_id")
                                {
                                    dtDtxRow["ehrfield"] = "preferred_hygienist";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance")
                                {
                                    dtDtxRow["ehrfield"] = "";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                                {
                                    dtDtxRow["ehrfield"] = "sec_member_id";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance_companyname")
                                {
                                    dtDtxRow["ehrfield"] = "";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "sex")
                                {
                                    dtDtxRow["ehrfield"] = "sex";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "work_phone")
                                {
                                    dtDtxRow["ehrfield"] = "work_phone";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "zipcode")
                                {
                                    dtDtxRow["ehrfield"] = "zipcode";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "emergencycontactname")
                                {
                                    // dtDtxRow["ehrfield"] = "answer";
                                    dtDtxRow["TableName"] = "patient_answers";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "emergencycontactnumber")
                                {
                                    //dtDtxRow["ehrfield"] = "answer";
                                    dtDtxRow["TableName"] = "patient_answers";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "school")
                                {
                                    dtDtxRow["ehrfield"] = "school";
                                    //dtDtxRow["TableName"] = "patient_answers";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "ssn")
                                {
                                    dtDtxRow["ehrfield"] = "social_security";
                                    //dtDtxRow["TableName"] = "patient_answers";
                                }
                                else if (dtDtxRow["ehrfield"].ToString().Trim() == "driverlicense")
                                {
                                    dtDtxRow["ehrfield"] = "drivers_license";
                                    //dtDtxRow["TableName"] = "patient_answers";
                                }
                                //else if (dtDtxRow["ehrfield"].ToString().Trim() == "employer")
                                //{
                                //    dtDtxRow["ehrfield"] = "name";
                                //    dtDtxRow["TableName"] = "employer";
                                //}

                                dtWebPatient_Form.AcceptChanges();

                            }
                            if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                            {
                                Utility.CheckEntryUserLoginIdExist();
                                bool Is_Record_Update = SynchEaglesoftBAL.Save_Patient_Form_Local_To_EagleSoft(dtWebPatient_Form, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            }

                            try
                            {
                                GetMedicalHistoryRecords(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                PushMedicalHistoryRecordsTOEaglesoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                ObjGoalBase.WriteToSyncLogFile("Medical_History_Save Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                            }
                            catch (Exception ex2)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Medical_History_Save Sync (Local Database To " + Utility.Application_Name + ") ]" + ex2.Message);
                            }

                            try
                            {
                                if (SynchEaglesoftBAL.SaveAllergiesToEaglesoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()))
                                {                                    
                                    ObjGoalBase.WriteToSyncLogFile("Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                                }
                            }
                            catch (Exception ex1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                            }
                            try
                            {
                                if (SynchEaglesoftBAL.DeleteAllergiesToEaglesoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString()))
                                {
                                    ObjGoalBase.WriteToSyncLogFile("Delete Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                                }
                            }
                            catch (Exception ex1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                            }

                            bool isRecordDeleted = false, isRecordSaved = false;
                            string Patient_EHR_IDS = "";
                            string Delete_Patient_EHR_ids = "";
                            string Save_Patient_EHR_ids = "";
                            try
                            {
                                if (SynchEaglesoftBAL.DeleteMedicationToEaglesoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), ref isRecordDeleted, ref Delete_Patient_EHR_ids))
                                {
                                    ObjGoalBase.WriteToSyncLogFile("Delete Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]");
                                }
                            }
                            catch (Exception ex1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                            }
                            try
                            {
                                if (SynchEaglesoftBAL.SaveMedicationToEaglesoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), ref isRecordSaved, ref Save_Patient_EHR_ids))
                                {
                                    ObjGoalBase.WriteToSyncLogFile("Save Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Save Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]");
                                }
                            }
                            catch (Exception ex1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Save Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                            }

                            if (isRecordSaved || isRecordDeleted)
                            {
                                Patient_EHR_IDS = (Delete_Patient_EHR_ids + Save_Patient_EHR_ids).TrimEnd(',');
                                if (Patient_EHR_IDS != "")
                                {
                                    SynchDataEagleSoft_PatientMedication(Patient_EHR_IDS);
                                }
                            }

                            string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (Call_Importing.ToLower() != "success")
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Importing status : " + Call_Importing);
                            }

                            string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            if (Call_Completed.ToLower() != "success")
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Completed status : " + Call_Completed);
                            }

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

                            //GetPatientDocument(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            //SynchEaglesoftBAL.Save_Document_in_EagleSoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());

                            //try
                            //{
                            //    #region Treatment DOc
                            //    SynchEaglesoftBAL.Save_Treatment_Document_in_EagleSoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtInstallServiceList.Rows[j]["Document_Path"].ToString());
                            //    #region change status as treatment doc impotred Completed
                            //    DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                            //    if (statusCompleted.Rows.Count > 0)
                            //    {
                            //        Change_Status_TreatmentDoc(statusCompleted, "Completed");
                            //    }
                            //    #endregion
                            //    #endregion
                            //}
                            //catch (Exception ex)
                            //{
                            //    ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                            //    // throw;
                            //}

                            SynchDataLocalToEagleSoft_Patient_Document();
                            ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }
        #region PatientPayment
        private void fncSynchDataEaglesoft_PatientPayment()
        {
            InitBgWorkerEaglesoft_PatientPayment();
            InitBgTimerEaglesoft_PatientPayment();
        }

        private void InitBgTimerEaglesoft_PatientPayment()
        {
            timerSynchEaglesoft_PatientPayment = new System.Timers.Timer();
            this.timerSynchEaglesoft_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
            this.timerSynchEaglesoft_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_PatientPayment_Tick);
            timerSynchEaglesoft_PatientPayment.Enabled = true;
            timerSynchEaglesoft_PatientPayment.Start();
        }

        private void InitBgWorkerEaglesoft_PatientPayment()
        {
            bwSynchEaglesoft_PatientPayment = new BackgroundWorker();
            bwSynchEaglesoft_PatientPayment.WorkerReportsProgress = true;
            bwSynchEaglesoft_PatientPayment.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_PatientPayment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEaglesoft_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_PatientPayment_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_PatientPayment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_PatientPayment.Enabled = false;
                MethodForCallSynchOrderEaglesoft_PatientPayment();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_PatientPayment()
        {
            System.Threading.Thread procThreadmainEaglesoft_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_PatientPayment);
            procThreadmainEaglesoft_PatientPayment.Start();
        }

        public void CallSyncOrderTableEaglesoft_PatientPayment()
        {
            if (bwSynchEaglesoft_PatientPayment.IsBusy != true)
            {
                bwSynchEaglesoft_PatientPayment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_PatientPayment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToEagleSoft_Patient_Payment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_PatientPayment.Enabled = true;
        }
        public void SynchDataLocalToEagleSoft_Patient_Payment()
        {
            try
            {
                if (!IsPaymentSyncing)
                {
                    IsPaymentSyncing = true;
                    if (Utility.IsApplicationIdleTimeOff)
                    {
                        Utility.WriteToSyncLogFile_All("Start Payment Ledger");
                        SynchDataLiveDB_Pull_PatientPaymentLog();
                        //CheckEntryUserLoginIdExist();
                        string noteId = "";
                        //SynchDataLiveDB_Pull_PatientPaymentLog();
                        DataTable dtPatientPayment = new DataTable();
                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                            if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                            {
                                string connctionstring = Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString();
                                //string conreg = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION= Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407;";
                                //connctionstring = conreg + connctionstring;
                                DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                //foreach (DataRow drRow in dtWebPatientPayment.Rows)
                                //{
                                noteId = "";
                                #region Insert Payment Records in EHR
                                if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                                {
                                    Utility.CheckEntryUserLoginIdExist();
                                    ObjGoalBase.WriteToSyncLogFile("Total Records to add in EHR." + dtWebPatientPayment.Rows.Count.ToString());
                                    SynchEaglesoftBAL.SavePatientPaymentTOEHR(connctionstring, dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                    ObjGoalBase.WriteToSyncLogFile("Patient Payment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                }
                                else
                                {
                                    ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");
                                }

                                #endregion

                                //#region Call API for EHR Entry Done
                                //if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                                //    {
                                //        noteId = SynchEaglesoftBAL.Save_PatientPaymentLog_LocalToEagelsoft(dtWebPatientPayment, Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                //        if (noteId != "")
                                //        {
                                //            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                //            ObjGoalBase.WriteToSyncLogFile("Patient Payment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                //        }
                                //        else
                                //        {
                                //            ObjGoalBase.WriteToErrorLogFile("[Patient Payment Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                //        }
                                //    }
                                //    else
                                //    {
                                //        ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                                //    }
                                //    #endregion

                                //    #region Sync those patient whose payment done in EHR

                                //    #endregion
                                ////}
                            }
                        }

                    }
                    IsPaymentSyncing = false;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_Paynment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
            finally { IsPaymentSyncing = false; }
        }

        #endregion
        public void SynchDataLocalToEagleSoft_Patient_SMSCallLog()
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
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            try
                            {
                                ObjGoalBase.WriteToSyncLogFile_All("Call Delete Duplicate Message");
                                SynchEaglesoftBAL.DeleteDuplicatePatientLog(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            }
                            catch (Exception ex1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[PatientSMSCallLog Sync Delete Duplicate Logs (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                            }

                            DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientSMSCallLogData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            //foreach (DataRow drRow in dtWebPatientPayment.Rows)
                            //{
                            noteId = "";
                            #region Insert Payment Records in EHR
                            //  SynchEaglesoftBAL.SavePatientPaymentTOEHR(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), drRow);
                            #endregion

                            #region Call API for EHR Entry Done
                            if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                            {
                                string connctionstring = Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString();
                                //string conreg = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION= Company=Patterson Technology Center;Application=Patterson EagleSoft;Signature=000fa55157edb8e14d818eb4fe3db41447146f1571g50efd3a3a1a842f8d14db3eccf9507c41bafc407;";
                                //connctionstring = conreg + connctionstring;

                                SynchEaglesoftBAL.Save_PatientSMSCallLog_LocalToEagelsoft(dtWebPatientPayment, connctionstring, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientSMSCallLog");
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
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[PatientSMSCallLog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Disease

        private void fncSynchDataEagleSoft_Disease()
        {
            InitBgWorkerEagleSoft_Disease();
            InitBgTimerEagleSoft_Disease();
        }

        private void InitBgTimerEagleSoft_Disease()
        {
            timerSynchEagleSoft_Disease = new System.Timers.Timer();
            this.timerSynchEagleSoft_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchEagleSoft_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEagleSoft_Disease_Tick);
            timerSynchEagleSoft_Disease.Enabled = true;
            timerSynchEagleSoft_Disease.Start();
        }

        private void InitBgWorkerEagleSoft_Disease()
        {
            bwSynchEagleSoft_Disease = new BackgroundWorker();
            bwSynchEagleSoft_Disease.WorkerReportsProgress = true;
            bwSynchEagleSoft_Disease.WorkerSupportsCancellation = true;
            bwSynchEagleSoft_Disease.DoWork += new DoWorkEventHandler(bwSynchEagleSoft_Disease_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEagleSoft_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEagleSoft_Disease_RunWorkerCompleted);
        }

        private void timerSynchEagleSoft_Disease_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEagleSoft_Disease.Enabled = false;
                MethodForCallSynchOrderEagleSoft_Disease();
            }
        }

        public void MethodForCallSynchOrderEagleSoft_Disease()
        {
            System.Threading.Thread procThreadmainEagleSoft_Disease = new System.Threading.Thread(this.CallSyncOrderTableEagleSoft_Disease);
            procThreadmainEagleSoft_Disease.Start();
        }

        public void CallSyncOrderTableEagleSoft_Disease()
        {
            if (bwSynchEagleSoft_Disease.IsBusy != true)
            {
                bwSynchEagleSoft_Disease.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEagleSoft_Disease_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEagleSoft_Disease.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEagleSoft_Disease();
                SynchDataEagleSoft_PatientDisease();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEagleSoft_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEagleSoft_Disease.Enabled = true;
        }


        public void SynchDataEagleSoft_PatientDisease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientDisease)
                {
                    Is_synched_PatientDisease = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEagleSoftpatientDisease = SynchEaglesoftBAL.GetEagleSoftPatientDiseaseData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEagleSoftpatientDisease.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalDisease = SynchLocalBAL.GetLocalPatientDiseaseData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            //foreach (DataRow dtDtxRow in dtEagleSoftpatientDisease.Rows)
                            //{
                            //    DataRow[] row = dtLocalDisease.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "' And Disease_EHR_ID='" + dtDtxRow["Disease_EHR_ID"].ToString() + "'");

                            //    if (row.Length > 0)
                            //    {
                            //        if (dtDtxRow["Patient_EHR_ID"].ToString().Trim() == row[0]["Patient_EHR_ID"].ToString().Trim() &&
                            //            dtDtxRow["Disease_EHR_ID"].ToString().Trim() != row[0]["Disease_EHR_ID"].ToString().Trim())
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
                            //foreach (DataRow dtRow in dtLocalDisease.Rows)
                            //{
                            //    DataRow rowBlcOpt = dtEagleSoftpatientDisease.Copy().Select("Disease_EHR_ID = '" + dtRow["Disease_EHR_ID"].ToString().Trim() + "' and Patient_EHR_ID = '" + dtRow["Patient_EHR_ID"].ToString() + "'").FirstOrDefault();
                            //    if (rowBlcOpt == null)
                            //    {
                            //        DataRow dtxtldr = dtEagleSoftpatientDisease.NewRow();
                            //        dtxtldr["Patient_EHR_ID"] = dtRow["Patient_EHR_ID"].ToString().Trim();
                            //        dtxtldr["Disease_EHR_ID"] = dtRow["Disease_EHR_ID"].ToString().Trim();
                            //        dtxtldr["Disease_Type"] = dtRow["Disease_Type"].ToString().Trim();
                            //        dtxtldr["Clinic_Number"] = dtRow["Clinic_Number"].ToString().Trim();
                            //        dtxtldr["is_deleted"] = 1;
                            //        dtxtldr["Is_Adit_Updated"] = 0;
                            //        dtxtldr["InsUptDlt"] = 3;
                            //        dtEagleSoftpatientDisease.Rows.Add(dtxtldr);
                            //    }
                            //}

                            DataTable dtSaveRecords = new DataTable();
                            dtSaveRecords = dtLocalDisease.Clone();
                            var itemsToBeAdded = (from Eaglesoft in dtEagleSoftpatientDisease.AsEnumerable()
                                                  join Local in dtLocalDisease.AsEnumerable()
                                                  on Eaglesoft["Disease_EHR_ID"].ToString().Trim() + "_" + Eaglesoft["Patient_EHR_ID"].ToString().Trim() + "_" + Eaglesoft["Disease_Type"].ToString().Trim() + "_" + Eaglesoft["Clinic_Number"].ToString().Trim()
                                                  equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                                                  into matchingRows
                                                  from matchingRow in matchingRows.DefaultIfEmpty()
                                                  where matchingRow == null
                                                  select Eaglesoft).ToList();
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

                            var itemsToBeUpdated = (from Eaglesoft in dtEagleSoftpatientDisease.AsEnumerable()
                                                    join Local in dtLocalDisease.AsEnumerable()
                                                    on Eaglesoft["Disease_EHR_ID"].ToString().Trim() + "_" + Eaglesoft["Patient_EHR_ID"].ToString().Trim() + "_" + Eaglesoft["Disease_Type"].ToString().Trim() + "_" + Eaglesoft["Clinic_Number"].ToString().Trim()
                                                    equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                                                    where
                                                   Eaglesoft["Patient_EHR_ID"].ToString().Trim() != Local["Patient_EHR_ID"].ToString().Trim() ||
                                                   Eaglesoft["Disease_EHR_ID"].ToString().Trim() != Local["Disease_EHR_ID"].ToString().Trim() ||
                                                   Eaglesoft["Disease_Name"].ToString().Trim() != Local["Disease_Name"].ToString().Trim() ||
                                                   Eaglesoft["Disease_Type"].ToString().Trim() != Local["Disease_Type"].ToString().Trim()
                                                    select Eaglesoft).ToList();
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
                                                   join Eaglesoft in dtEagleSoftpatientDisease.AsEnumerable()
                                                   on Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim() + "_" + Local["Disease_Type"].ToString().Trim() + "_" + Local["Clinic_Number"].ToString().Trim()
                                                   equals Eaglesoft["Disease_EHR_ID"].ToString().Trim() + "_" + Eaglesoft["Patient_EHR_ID"].ToString().Trim() + "_" + Eaglesoft["Disease_Type"].ToString().Trim() + "_" + Eaglesoft["Clinic_Number"].ToString().Trim()
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

                            dtEagleSoftpatientDisease.AcceptChanges();

                            if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                            //if (dtEagleSoftpatientDisease != null && dtEagleSoftpatientDisease.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_PatientDisease_EagleSoft_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                                    ObjGoalBase.WriteToSyncLogFile("Patient Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_PatientDisease();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Patient Disease Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
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

        public void SynchDataEagleSoft_Disease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEagleSoftDisease = SynchEaglesoftBAL.GetEagleSoftDiseaseData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                            dtEagleSoftDisease.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalDisease = SynchLocalBAL.GetLocalDiseaseData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            foreach (DataRow dtDtxRow in dtEagleSoftDisease.Rows)
                            {
                                DataRow[] row = dtLocalDisease.Copy().Select("Disease_EHR_ID = '" + dtDtxRow["Disease_EHR_ID"] + "'");
                                if (row.Length > 0)
                                {
                                    if (dtDtxRow["Disease_Name"].ToString().Trim() != row[0]["Disease_Name"].ToString().Trim())
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

                            foreach (DataRow dtRow in dtLocalDisease.Rows)
                            {
                                DataRow rowBlcOpt = dtEagleSoftDisease.Copy().Select("Disease_EHR_ID = '" + dtRow["Disease_EHR_ID"].ToString().Trim() + "'").FirstOrDefault();
                                if (rowBlcOpt == null)
                                {
                                    DataRow dtxtldr = dtEagleSoftDisease.NewRow();
                                    dtxtldr["Disease_EHR_ID"] = dtRow["Disease_EHR_ID"].ToString().Trim();
                                    dtxtldr["Disease_Name"] = dtRow["Disease_Name"].ToString().Trim();
                                    dtxtldr["Disease_Type"] = dtRow["Disease_Type"].ToString().Trim();
                                    dtxtldr["is_deleted"] = 1;
                                    dtxtldr["Is_Adit_Updated"] = 0;
                                    dtxtldr["InsUptDlt"] = 3;
                                    dtEagleSoftDisease.Rows.Add(dtxtldr);
                                }
                            }

                            dtEagleSoftDisease.AcceptChanges();

                            if (dtEagleSoftDisease != null && dtEagleSoftDisease.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_Disease_EagleSoft_To_Local(dtEagleSoftDisease, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                                    ObjGoalBase.WriteToSyncLogFile("Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_Disease();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                        }
                    }
                    SynchDataEagleSoft_Medication();
                }

            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataEagleSoft_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");

                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtMedication = SynchEaglesoftBAL.GetEagleSoftMedicationData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                            dtMedication.AcceptChanges();
                            dtMedication.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalMedication = SynchLocalBAL.GetLocalMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            //foreach (DataRow dtDtxRow in dtMedication.Rows)
                            //{
                            //    DataRow[] row = dtLocalMedication.Copy().Select("Medication_EHR_ID = '" + dtDtxRow["Medication_EHR_ID"] + "' AND Medication_Type = '" + dtDtxRow["Medication_Type"] + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"] + "' ");
                            //    if (row.Length > 0)
                            //    {
                            //        if (dtDtxRow["Medication_Name"].ToString().ToUpper().Trim() != row[0]["Medication_Name"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Drug_Quantity"].ToString().ToUpper().Trim() != row[0]["Drug_Quantity"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Description"].ToString().ToUpper().Trim() != row[0]["Medication_Description"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Notes"].ToString().ToUpper().Trim() != row[0]["Medication_Notes"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Sig"].ToString().ToUpper().Trim() != row[0]["Medication_Sig"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim() != row[0]["Medication_Parent_EHR_ID"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Type"].ToString().ToUpper().Trim() != row[0]["Medication_Type"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Allow_Generic_Sub"].ToString().ToUpper().Trim() != row[0]["Allow_Generic_Sub"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Refills"].ToString().ToUpper().Trim() != row[0]["Refills"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Is_Active"].ToString().ToUpper().Trim() != row[0]["Is_Active"].ToString().ToUpper().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Provider_ID"].ToString().ToUpper().Trim() != row[0]["Medication_Provider_ID"].ToString().ToUpper().Trim())
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

                            //foreach (DataRow dtLPHRow in dtLocalMedication.Rows)
                            //{
                            //    DataRow[] rowDis = dtMedication.Copy().Select("Medication_EHR_ID = '" + dtLPHRow["Medication_EHR_ID"] + "' AND Medication_Type = '" + dtLPHRow["Medication_Type"] + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"] + "'");
                            //    if (rowDis.Length > 0)
                            //    { }
                            //    else
                            //    {
                            //        DataRow rowDisDtldr = dtMedication.NewRow();
                            //        rowDisDtldr["Medication_EHR_ID"] = dtLPHRow["Medication_EHR_ID"].ToString().Trim();
                            //        rowDisDtldr["Medication_Type"] = dtLPHRow["Medication_Type"].ToString().Trim();
                            //        rowDisDtldr["Medication_Name"] = dtLPHRow["Medication_Name"].ToString().Trim();
                            //        rowDisDtldr["Clinic_Number"] = dtLPHRow["Clinic_Number"].ToString().Trim();
                            //        //  rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
                            //        rowDisDtldr["InsUptDlt"] = 3;
                            //        dtMedication.Rows.Add(rowDisDtldr);
                            //    }
                            //}
                            dtMedication.AcceptChanges();

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
                                                    OpenDental["Medication_Name"].ToString().Trim() != Local["Medication_Name"].ToString().Trim() ||
                                                    OpenDental["Drug_Quantity"].ToString().ToUpper().Trim() != Local["Drug_Quantity"].ToString().ToUpper().Trim() ||
                                                    OpenDental["Medication_Description"].ToString().Trim() != Local["Medication_Description"].ToString().Trim() ||
                                                    OpenDental["Medication_Notes"].ToString().Trim() != Local["Medication_Notes"].ToString().Trim() ||
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
                            //if (dtMedication != null && dtMedication.Rows.Count > 0)
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
            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Medication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataEagleSoft_PatientMedication(string Patient_EHR_IDS)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");

                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtMedication = SynchEaglesoftBAL.GetEagleSoftPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Patient_EHR_IDS);
                            dtMedication.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalMedication = SynchLocalBAL.GetLocalPatientMedicationData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Patient_EHR_IDS);

                            //foreach (DataRow dtDtxRow in dtMedication.Rows)
                            //{
                            //    DataRow[] row = dtLocalMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtDtxRow["PatientMedication_EHR_ID"].ToString().Trim() + "' And Medication_EHR_ID = '" + dtDtxRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtDtxRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
                            //    if (row.Length > 0)
                            //    {
                            //        if (dtDtxRow["Patient_EHR_ID"].ToString().Trim() != row[0]["Patient_EHR_ID"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_EHR_ID"].ToString().Trim() != row[0]["Medication_EHR_ID"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Note"].ToString().Trim() != row[0]["Medication_Note"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Name"].ToString().Trim() != row[0]["Medication_Name"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Medication_Type"].ToString().Trim() != row[0]["Medication_Type"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Provider_EHR_ID"].ToString().Trim() != row[0]["Provider_EHR_ID"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Drug_Quantity"].ToString().Trim() != row[0]["Drug_Quantity"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Start_Date"].ToString().Trim() != row[0]["Start_Date"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["Patient_Notes"].ToString().Trim() != row[0]["Patient_Notes"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["is_active"].ToString().Trim() != row[0]["is_active"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["End_Date"].ToString().Trim() != row[0]["End_Date"].ToString().Trim())
                            //        {
                            //            dtDtxRow["InsUptDlt"] = 2;
                            //        }
                            //        else if (dtDtxRow["EHR_Entry_DateTime"].ToString().Trim() != row[0]["EHR_Entry_DateTime"].ToString().Trim())
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

                            //foreach (DataRow dtLPHRow in dtLocalMedication.Rows)
                            //{
                            //    DataRow[] rowDis = dtMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtLPHRow["PatientMedication_EHR_ID"].ToString() + "' And Medication_EHR_ID = '" + dtLPHRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtLPHRow["Patient_EHR_ID"] + "'");
                            //    if (rowDis.Length > 0)
                            //    { }
                            //    else
                            //    {
                            //        DataRow rowDisDtldr = dtMedication.NewRow();
                            //        rowDisDtldr["PatientMedication_EHR_ID"] = dtLPHRow["PatientMedication_EHR_ID"].ToString().Trim();
                            //        rowDisDtldr["Patient_EHR_ID"] = dtLPHRow["Patient_EHR_ID"].ToString().Trim();
                            //        rowDisDtldr["Medication_EHR_ID"] = dtLPHRow["Medication_EHR_ID"].ToString().Trim();
                            //        rowDisDtldr["Medication_Type"] = dtLPHRow["Medication_Type"].ToString().Trim();
                            //        rowDisDtldr["Medication_Name"] = dtLPHRow["Medication_Name"].ToString().Trim();
                            //        rowDisDtldr["Clinic_Number"] = dtLPHRow["Clinic_Number"].ToString().Trim();
                            //        //rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
                            //        rowDisDtldr["InsUptDlt"] = 3;
                            //        dtMedication.Rows.Add(rowDisDtldr);
                            //    }
                            //}
                            dtMedication.AcceptChanges();

                            DataTable dtSaveRecords = new DataTable();
                            dtSaveRecords = dtLocalMedication.Clone();
                            //Add
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
                            //if (dtMedication != null && dtMedication.Rows.Count > 0)
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
            }
            catch (Exception ex)
            {
                Is_synched_PatientMedication = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientMedication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        #endregion

        #region ProviderOfficeHOurs

        private void fncSynchDataEagleSoftToLocal_ProviderOfficeHours()
        {
            InitBgWorkerEagleSoftToLocal_ProviderOfficeHours();
            InitBgTimerEagleSoftToLocal_ProviderOfficeHours();
        }

        private void InitBgTimerEagleSoftToLocal_ProviderOfficeHours()
        {
            timerSynchEagleSoftToLocal_ProviderOfficeHours = new System.Timers.Timer();
            this.timerSynchEagleSoftToLocal_ProviderOfficeHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchEagleSoftToLocal_ProviderOfficeHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEagleSoftToLocal_ProviderOfficeHours_Tick);
            timerSynchEagleSoftToLocal_ProviderOfficeHours.Enabled = true;
            timerSynchEagleSoftToLocal_ProviderOfficeHours.Start();
            timerSynchEagleSoftToLocal_ProviderOfficeHours_Tick(null, null);
        }

        private void InitBgWorkerEagleSoftToLocal_ProviderOfficeHours()
        {
            bwSynchEagleSoftToLocal_ProviderOfficeHours = new BackgroundWorker();
            bwSynchEagleSoftToLocal_ProviderOfficeHours.WorkerReportsProgress = true;
            bwSynchEagleSoftToLocal_ProviderOfficeHours.WorkerSupportsCancellation = true;
            bwSynchEagleSoftToLocal_ProviderOfficeHours.DoWork += new DoWorkEventHandler(bwSynchEagleSoftToLocal_ProviderOfficeHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEagleSoftToLocal_ProviderOfficeHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEagleSoftToLocal_ProviderOfficeHours_RunWorkerCompleted);
        }

        private void timerSynchEagleSoftToLocal_ProviderOfficeHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEagleSoftToLocal_ProviderOfficeHours.Enabled = false;
                MethodForCallSynchOrderEagleSoftToLocal_ProviderOfficeHours();
            }
        }

        public void MethodForCallSynchOrderEagleSoftToLocal_ProviderOfficeHours()
        {
            System.Threading.Thread procThreadmainEagleSoftToLocal_ProviderOfficeHours = new System.Threading.Thread(this.CallSyncOrderTableEagleSoftToLocal_ProviderOfficeHours);
            procThreadmainEagleSoftToLocal_ProviderOfficeHours.Start();
        }

        public void CallSyncOrderTableEagleSoftToLocal_ProviderOfficeHours()
        {
            if (bwSynchEagleSoftToLocal_ProviderOfficeHours.IsBusy != true)
            {
                bwSynchEagleSoftToLocal_ProviderOfficeHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEagleSoftToLocal_ProviderOfficeHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEagleSoftToLocal_ProviderOfficeHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEagleSoftToLocal_ProviderOfficeHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEagleSoftToLocal_ProviderOfficeHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEagleSoftToLocal_ProviderOfficeHours.Enabled = true;
        }

        public void SynchDataEagleSoftToLocal_ProviderOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && IsEaglesoftProviderSync && IsEaglesoftOperatorySync && Utility.is_scheduledCustomhour)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEagleSoftProviderOfficeHours = SynchEaglesoftBAL.GetEagleSoftProviderOfficeHours(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            DataTable dtEagleSoftLocalProviderOfficeHours = SynchLocalBAL.GetLocalProviderOfficeHours(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            dtEagleSoftProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                            dtEagleSoftProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                            if (!dtEagleSoftLocalProviderOfficeHours.Columns.Contains("InsUptDlt"))
                            {
                                dtEagleSoftLocalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                                dtEagleSoftLocalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                            }

                            dtEagleSoftLocalProviderOfficeHours = CompareDataTableRecords(ref dtEagleSoftProviderOfficeHours, dtEagleSoftLocalProviderOfficeHours, "POH_EHR_ID", "POH_LocalDB_ID", "POH_LocalDB_ID,POH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                            dtEagleSoftProviderOfficeHours.AcceptChanges();
                            dtEagleSoftLocalProviderOfficeHours.AcceptChanges();

                            if ((dtEagleSoftProviderOfficeHours != null && dtEagleSoftProviderOfficeHours.Rows.Count > 0) || (dtEagleSoftLocalProviderOfficeHours != null && dtEagleSoftLocalProviderOfficeHours.Rows.Count > 0))
                            {
                                bool status = false;
                                DataTable dtSaveRecords = dtEagleSoftProviderOfficeHours.Clone();
                                if (dtEagleSoftProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtEagleSoftLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                                {
                                    if (dtEagleSoftProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtEagleSoftProviderOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());

                                    }
                                    if (dtEagleSoftLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtEagleSoftLocalProviderOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                                    }
                                    status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderOfficeHours", "POH_LocalDB_ID,POH_Web_ID", "POH_LocalDB_ID");
                                }
                                else
                                {
                                    if (dtEagleSoftProviderOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[ProviderOfficeHours Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                        }
                    }
                    SynchDataEagleSoftToLocal_ProviderCustomeHours();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        public void SynchDataEagleSoftToLocal_ProviderCustomeHours()
        {

            try
            {
                if (Utility.IsApplicationIdleTimeOff && IsEaglesoftProviderSync && IsEaglesoftOperatorySync)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEagleSoftProviderHours = SynchEaglesoftBAL.GetEagleSoftProviderHours(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            DataTable dtEagleSoftLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            dtEagleSoftProviderHours.Columns.Add("InsUptDlt", typeof(int));
                            dtEagleSoftProviderHours.Columns["InsUptDlt"].DefaultValue = 0;

                            if (!dtEagleSoftLocalProviderHours.Columns.Contains("InsUptDlt"))
                            {
                                dtEagleSoftLocalProviderHours.Columns.Add("InsUptDlt", typeof(int));
                                dtEagleSoftLocalProviderHours.Columns["InsUptDlt"].DefaultValue = 0;
                            }

                            dtEagleSoftLocalProviderHours = CompareDataTablewithDateOnly(ref dtEagleSoftProviderHours, dtEagleSoftLocalProviderHours, "PH_EHR_ID", "PH_LocalDB_ID", "PH_LocalDB_ID,PH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");

                            dtEagleSoftProviderHours.AcceptChanges();
                            dtEagleSoftLocalProviderHours.AcceptChanges();

                            if ((dtEagleSoftProviderHours != null && dtEagleSoftProviderHours.Rows.Count > 0) || (dtEagleSoftLocalProviderHours != null && dtEagleSoftLocalProviderHours.Rows.Count > 0))
                            {
                                bool status = false;
                                DataTable dtSaveRecords = dtEagleSoftProviderHours.Clone();
                                if (dtEagleSoftProviderHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtEagleSoftLocalProviderHours.Select("InsUptDlt IN (3)").Count() > 0)
                                {
                                    if (dtEagleSoftProviderHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtEagleSoftProviderHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                                    }
                                    if (dtEagleSoftLocalProviderHours.Select("InsUptDlt IN (3)").Count() > 0)
                                    {
                                        dtSaveRecords.Load(dtEagleSoftLocalProviderHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                                    }
                                    status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderHours", "PH_LocalDB_ID,PH_Web_ID", "PH_LocalDB_ID");
                                }
                                else
                                {
                                    if (dtEagleSoftProviderHours.Select("InsUptDlt IN (4)").Count() > 0)
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
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[ProviderHours Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[ProviderHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        private DataTable CompareDataTablewithDateOnly(ref DataTable dtSource, DataTable dtDestination, string compareColumnName, string primarykeyColumns, string ignoreColumns)
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

        #endregion

        #region Sync Medical History

        private void fncSynchDataEaglesoft_MedicalHistory()
        {
            InitBgWorkerEaglesoft_MedicalHistory();
            InitBgTimerEaglesoft_MedicalHistory();
        }

        private void InitBgTimerEaglesoft_MedicalHistory()
        {
            timerSynchEaglesoft_MedicalHistory = new System.Timers.Timer();
            this.timerSynchEaglesoft_MedicalHistory.Interval = 1000 * GoalBase.intervalEHRSynch_MedicalHistory;
            this.timerSynchEaglesoft_MedicalHistory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_MedicalHistory_Tick);
            timerSynchEaglesoft_MedicalHistory.Enabled = true;
            timerSynchEaglesoft_MedicalHistory.Start();
            timerSynchEaglesoft_MedicalHistory_Tick(null, null);
        }

        private void InitBgWorkerEaglesoft_MedicalHistory()
        {
            bwSynchEaglesoft_MedicalHistory = new BackgroundWorker();
            bwSynchEaglesoft_MedicalHistory.WorkerReportsProgress = true;
            bwSynchEaglesoft_MedicalHistory.WorkerSupportsCancellation = true;
            bwSynchEaglesoft_MedicalHistory.DoWork += new DoWorkEventHandler(bwSynchEaglesoft_MedicalHistory_DoWork);
            bwSynchEaglesoft_MedicalHistory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoft_MedicalHistory_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_MedicalHistory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEaglesoft_MedicalHistory.Enabled = false;
                MethodForCallSynchOrderEaglesoft_MedicalHistory();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_MedicalHistory()
        {
            System.Threading.Thread procThreadmainEaglesoft_MedicalHistory = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_MedicalHistory);
            procThreadmainEaglesoft_MedicalHistory.Start();
        }

        public void CallSyncOrderTableEaglesoft_MedicalHistory()
        {
            if (bwSynchEaglesoft_MedicalHistory.IsBusy != true)
            {
                bwSynchEaglesoft_MedicalHistory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoft_MedicalHistory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEaglesoft_MedicalHistory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_MedicalHistory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoft_MedicalHistory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEaglesoft_MedicalHistory.Enabled = true;
        }

        public void SynchDataEaglesoft_MedicalHistory()
        {
            CallSynch_Eaglesoft_MedicalHistory();

        }

        private void CallSynch_Eaglesoft_MedicalHistory()
        {
            if (!Is_synched_MedicalHistory && Utility.IsApplicationIdleTimeOff)
            {
                string tablename = "";
                try
                {
                    Is_synched_MedicalHistory = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataSet dsEaglesoftMedicalHistory = SynchEaglesoftBAL.GetEaglesoftMedicalHistoryData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            DataTable dtMedicalHistory = new DataTable();
                            string ignoreColumnsList = "", primaryColumnList = "";
                            foreach (DataTable dtTable in dsEaglesoftMedicalHistory.Tables)
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

                                if (dtMedicalHistory.TableName.ToString() == "EagleSoftFormMaster")
                                {
                                    ignoreColumnsList = "FormMaster_LocalDB_ID,FormMaster_web_Id";
                                    primaryColumnList = "FormMaster_LocalDB_ID";
                                    dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "Form_EHR_Id", "FormMaster_LocalDB_ID", "FormMaster_LocalDB_ID,FormMaster_web_Id,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Clinic_Number,Service_Install_Id,EHR_Entry_DateTime,EHR_Modified_DateTime");
                                }
                                else if (dtMedicalHistory.TableName.ToString() == "EagleSoftSectionMaster")
                                {
                                    ignoreColumnsList = "SectionMaster_LocalDB_ID,SectionMaster_Web_Id";
                                    primaryColumnList = "SectionMaster_LocalDB_ID";
                                    dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "Section_EHR_Id", "SectionMaster_LocalDB_ID", "SectionMaster_LocalDB_ID,SectionMaster_Web_Id,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Clinic_Number,Service_Install_Id");
                                }
                                else if (dtMedicalHistory.TableName.ToString() == "EagleSoftAlertMaster")
                                {
                                    ignoreColumnsList = "AlertMaster_LocalDB_ID,AlertMaster_web_Id";
                                    primaryColumnList = "AlertMaster_LocalDB_ID";
                                    dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "Alert_EHR_Id", "AlertMaster_LocalDB_ID", "AlertMaster_LocalDB_ID,AlertMaster_web_Id,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Clinic_Number,Service_Install_Id");
                                }
                                else if (dtMedicalHistory.TableName.ToString() == "EagleSoftSectionItemMaster")
                                {
                                    ignoreColumnsList = "SectionItemMaster_LocalDB_ID,SectionItem_WEB_Id";
                                    primaryColumnList = "SectionItemMaster_LocalDB_ID";
                                    dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "SectionItem_EHR_Id", "SectionItemMaster_LocalDB_ID", "SectionItemMaster_LocalDB_ID,SectionItem_WEB_Id,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Clinic_Number,Service_Install_Id");
                                }
                                else if (dtMedicalHistory.TableName.ToString() == "EagleSoftSectionItemOptionMaster")
                                {
                                    ignoreColumnsList = "SectionItemOptionMaster_LocalDB_ID,SectionItemOption_WEB_Id";
                                    primaryColumnList = "SectionItemOptionMaster_LocalDB_ID";
                                    dtLocalDbRecords = CompareDataTableRecords(ref dtMedicalHistory, dtLocalDbRecords, "SectionItemOption_EHR_Id", "SectionItemOptionMaster_LocalDB_ID", "SectionItemOptionMaster_LocalDB_ID,SectionItemOption_WEB_Id,Is_Adit_Updated,Is_deleted,Last_Sync_Date,Entry_DateTime,InsUptDlt,Clinic_Number,Service_Install_Id");
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
                                        ObjGoalBase.WriteToErrorLogFile("[" + tablename + " Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                        IsTrackerOperatorySync = false;
                                    }
                                }
                                #endregion
                            }
                            SynchDataLiveDB_Push_MedicalHisotryTables("EagleSoftFormMaster");
                            SynchDataLiveDB_Push_MedicalHisotryTables("EagleSoftSectionMaster");
                            SynchDataLiveDB_Push_MedicalHisotryTables("EagleSoftAlertMaster");
                            SynchDataLiveDB_Push_MedicalHisotryTables("EagleSoftSectionItemMaster");
                            SynchDataLiveDB_Push_MedicalHisotryTables("EagleSoftSectionItemOptionMaster");
                        }
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

        private void PushMedicalHistoryRecordsTOEaglesoft(string DbString, string Service_Install_Id, string strPatientFormID = "", string strPatientID = "")
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                DataTable dtPatientWithFormLocal = SynchLocalBAL.GetMedicalHistoryPatientWithForm(Service_Install_Id, strPatientFormID);
                DataTable dtPatientWithFormEagleSoft = SynchEaglesoftBAL.GetPatientWiseMedicalForm(DbString, strPatientID);
                DataTable dtMedicalHistoryQuestion = new DataTable();
                DataTable dtMedicalHistoryAnswer = new DataTable();

                foreach (DataRow drRow in dtPatientWithFormLocal.Rows)
                {
                    var result = dtPatientWithFormEagleSoft.AsEnumerable().Where(o => o.Field<object>("Patient_EHR_id").ToString() == drRow["Patient_EHR_id"].ToString() && o.Field<object>("FormMaster_EHR_Id").ToString() == drRow["FormMaster_EHR_Id"].ToString());
                    if (result != null && result.Count() > 0)
                    {
                        drRow["FormInstanceId"] = result.First().Field<object>("FormInstanceId");
                        #region Get MedicalHistory Question and Answer From Local DB
                        SynchLocalBAL.UpdateMedicalHistorySubmitRecords(drRow["PatientForm_Web_id"].ToString(), drRow["FormMaster_EHR_Id"].ToString(), Convert.ToInt64(result.First().Field<object>("FormInstanceId")), DbString, Service_Install_Id);
                        #endregion
                    }
                    else
                    {
                        SynchLocalBAL.InsertMedicalHistorySubmitRecords(drRow["PatientForm_Web_id"].ToString(), drRow["FormMaster_EHR_Id"].ToString(), drRow["Patient_EHR_id"].ToString(), DbString, Service_Install_Id);
                    }
                    SynchLocalBAL.Update_PatientForm_MedicalHistory_Field_Pushed(drRow["PatientForm_Web_id"].ToString(), Service_Install_Id);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region PatientWiseRecall

        private void fncSynchDataEaglesoft_PatientWiseRecallType()
        {
            InitBgWorkerEaglesoft_PatientWiseRecallType();
            InitBgTimerEaglesoft_PatientWiseRecallType();
        }

        private void InitBgTimerEaglesoft_PatientWiseRecallType()
        {
            timerSynchEagleSoftToLocalPatientWiseRecallType = new System.Timers.Timer();
            this.timerSynchEagleSoftToLocalPatientWiseRecallType.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchEagleSoftToLocalPatientWiseRecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_PatientWiseRecallType_Tick);
            timerSynchEagleSoftToLocalPatientWiseRecallType.Enabled = true;
            timerSynchEagleSoftToLocalPatientWiseRecallType.Start();
        }

        private void InitBgWorkerEaglesoft_PatientWiseRecallType()
        {
            bwSynchEagleSoftToLocalPatientWiseRecallType = new BackgroundWorker();
            bwSynchEagleSoftToLocalPatientWiseRecallType.WorkerReportsProgress = true;
            bwSynchEagleSoftToLocalPatientWiseRecallType.WorkerSupportsCancellation = true;
            bwSynchEagleSoftToLocalPatientWiseRecallType.DoWork += new DoWorkEventHandler(bwSynchEaglesoftPatientWiseRecallType_DoWork);
            bwSynchEagleSoftToLocalPatientWiseRecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEaglesoftPatientWiseRecallType_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_PatientWiseRecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEagleSoftToLocalPatientWiseRecallType.Enabled = false;
                MethodForCallSynchOrderEaglesoftPatientWiseRecallType();
            }
        }

        public void MethodForCallSynchOrderEaglesoftPatientWiseRecallType()
        {
            System.Threading.Thread procThreadmainEaglesoftPatientWiseRecallType = new System.Threading.Thread(this.CallSyncOrderTableEaglesoftPatientWiseRecallType);
            procThreadmainEaglesoftPatientWiseRecallType.Start();
        }

        public void CallSyncOrderTableEaglesoftPatientWiseRecallType()
        {
            if (bwSynchEagleSoftToLocalPatientWiseRecallType.IsBusy != true)
            {
                bwSynchEagleSoftToLocalPatientWiseRecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEaglesoftPatientWiseRecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEagleSoftToLocalPatientWiseRecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoftPatientWiseRecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEaglesoftPatientWiseRecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEagleSoftToLocalPatientWiseRecallType.Enabled = true;
        }

        public void SynchDataEaglesoftPatientWiseRecallType()
        {
            CallSynch_PatientWiseRecallType();

        }

        private void CallSynch_PatientWiseRecallType()
        {
            if (Utility.IsApplicationIdleTimeOff)
            {
                // Is_synchedPatientWiseRecallType = true;

                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftRecallType = SynchEaglesoftBAL.GetPatientWiseRecall(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            // dtEaglesoftRecallType.Columns.Add("InsUptDlt", typeof(int));
                            DataTable dtLocalRecallType = SynchLocalBAL.GetLocalPatientWiseRecallTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            foreach (DataRow dtDtxRow in dtEaglesoftRecallType.Rows)
                            {
                                DataRow[] row = dtLocalRecallType.Copy().Select("Patient_Recall_Id = '" + dtDtxRow["Patient_Recall_Id"] + "'");
                                if (row.Length > 0)
                                {
                                    if (dtDtxRow["RecallType_Name"].ToString().ToLower().Trim() != row[0]["RecallType_Name"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["Default_Recall"].ToString().ToLower().Trim() != row[0]["Default_Recall"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["Recall_Date"].ToString() != "" && row[0]["Recall_Date"].ToString() != "" && Convert.ToDateTime(dtDtxRow["Recall_Date"].ToString().Trim()) != Convert.ToDateTime(row[0]["Recall_Date"].ToString().Trim()))
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
                                bool status = SynchEaglesoftBAL.SavePatientWiseRecallType_Eaglesoft_To_Local(dtEaglesoftRecallType, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
                                    ObjGoalBase.WriteToSyncLogFile("RecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    //SynchDataLiveDB_PushPatientWiseRecallType();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[RecallType Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                        }
                    }
                    //Is_synchedPatientWiseRecallType = false;
                }
                catch (Exception ex)
                {
                    //  Is_synchedPatientWiseRecallType = false;
                    ObjGoalBase.WriteToErrorLogFile("[RecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }

            }
        }

        #endregion

        //rooja -  https://app.asana.com/0/1203599217474380/1207061756651636/f
        #region Synch Insurance

        private void fncSynchDataEaglesoft_Insurance()
        {
            InitBgWorkerEaglesoft_Insurance();
            InitBgTimerEaglesoft_Insurance();
        }

        private void InitBgTimerEaglesoft_Insurance()
        {
            timerSynchEagleSoft_Insurance = new System.Timers.Timer();
            this.timerSynchEagleSoft_Insurance.Interval = 1000 * GoalBase.intervalEHRSynch_Insurance;
            this.timerSynchEagleSoft_Insurance.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEaglesoft_Insurance_Tick);
            timerSynchEagleSoft_Insurance.Enabled = true;
            timerSynchEagleSoft_Insurance.Start();
        }

        private void InitBgWorkerEaglesoft_Insurance()
        {
            bwSynchEagleSoft_Insurance = new BackgroundWorker();
            bwSynchEagleSoft_Insurance.WorkerReportsProgress = true;
            bwSynchEagleSoft_Insurance.WorkerSupportsCancellation = true;
            bwSynchEagleSoft_Insurance.DoWork += new DoWorkEventHandler(bwSynchEagleSoft_Insurance_DoWork);
            bwSynchEagleSoft_Insurance.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEagleSoft_Insurance_RunWorkerCompleted);
        }

        private void timerSynchEaglesoft_Insurance_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEagleSoft_Insurance.Enabled = false;
                MethodForCallSynchOrderEaglesoft_Insurance();
            }
        }

        public void MethodForCallSynchOrderEaglesoft_Insurance()
        {
            System.Threading.Thread procThreadmainEaglesoft_Insurance = new System.Threading.Thread(this.CallSyncOrderTableEaglesoft_Insurance);
            procThreadmainEaglesoft_Insurance.Start();
        }

        public void CallSyncOrderTableEaglesoft_Insurance()
        {
            if (bwSynchEagleSoft_Insurance.IsBusy != true)
            {
                bwSynchEagleSoft_Insurance.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEagleSoft_Insurance_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEagleSoft_Insurance.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEaglesoft_Insurance();

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEagleSoft_Insurance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEagleSoft_Insurance.Enabled = true;
        }

        public void SynchDataEaglesoft_Insurance()
        {
            if (!Is_synched_Insurance && Utility.IsApplicationIdleTimeOff)
            {
                Is_synched_Insurance = true;
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString().Trim() + "' ");
                        if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                        {
                            DataTable dtEaglesoftInsurance = SynchEaglesoftBAL.GetEaglesoftInsuranceData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());

                            dtEaglesoftInsurance.Columns.Add("InsUptDlt", typeof(int));

                            DataTable dtLocalInsurance = SynchLocalBAL.GetLocalInsuranceData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            foreach (DataRow dtDtxRow in dtEaglesoftInsurance.Rows)
                            {
                                DataRow[] row = dtLocalInsurance.Copy().Select("Insurance_EHR_ID = '" + dtDtxRow["insurance_company_id"] + "' ");
                                if (row.Length > 0)
                                {
                                    if (dtDtxRow["name"].ToString().Trim() != row[0]["Insurance_Name"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["address_1"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["address_2"].ToString().ToLower().Trim() != row[0]["Address2"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["city"].ToString().ToLower().Trim() != row[0]["City"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["state"].ToString().ToLower().Trim() != row[0]["State"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["zipcode"].ToString().ToLower().Trim() != row[0]["Zipcode"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["phone1"].ToString().ToLower().Trim() != row[0]["Phone"].ToString().ToLower().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["neic_payer_id"].ToString().ToLower().Trim() != row[0]["ElectId"].ToString().ToLower().Trim())
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
                                DataRow[] row = dtEaglesoftInsurance.Copy().Select("insurance_company_id = '" + dtDtxRow["Insurance_EHR_ID"] + "' ");
                                if (row.Length > 0)
                                { }
                                else
                                {
                                    DataRow BlcOptDtldr = dtEaglesoftInsurance.NewRow();
                                    BlcOptDtldr["insurance_company_id"] = dtDtxRow["Insurance_EHR_ID"].ToString().Trim();
                                    BlcOptDtldr["name"] = dtDtxRow["Insurance_Name"].ToString().Trim();
                                    // BlcOptDtldr["Clinic_Number"] = dtDtxRow["Clinic_Number"].ToString().Trim();
                                    BlcOptDtldr["InsUptDlt"] = 3;
                                    dtEaglesoftInsurance.Rows.Add(BlcOptDtldr);
                                }
                            }

                            dtEaglesoftInsurance.AcceptChanges();

                            if (dtEaglesoftInsurance != null && dtEaglesoftInsurance.Rows.Count > 0)
                            {
                                bool status = SynchEaglesoftBAL.Save_Insurance_Eaglesoft_To_Local(dtEaglesoftInsurance, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                if (status)
                                {
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Insurance");
                                    ObjGoalBase.WriteToSyncLogFile("Insurance Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    SynchDataLiveDB_Push_Insurance();
                                }
                                else
                                {
                                    ObjGoalBase.WriteToErrorLogFile("[Insurance Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
                                }
                            }
                        }
                    }
                    Is_synched_Insurance = false;
                }
                catch (Exception ex)
                {
                    Is_synched_Insurance = false;
                    ObjGoalBase.WriteToErrorLogFile("[AppointmentStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Event Listener
        public static bool SynchDataLocalToEagleSoft_AppointmentFromEvent(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id)
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

                DataTable dtEagleSoftPatient = SynchEaglesoftBAL.GetPatientListFromEagleSoft(strDbConnString);
                DataTable dtIdelProv = SynchEaglesoftBAL.GetEagleSoftIdelProvider(strDbConnString);

                string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                string tmpApptProv = "";
                string tmpPatient_id = "0";
                string tmpPatient_Gur_id = "0";
                Int64 tmpAppt_EHR_id = 0;
                //int tmpNewPatient = 1;

                string tmpLastName = "";
                string tmpFirstName = "";

                string TmpWebPatientName = "";
                string TmpWebReversePatientName = "";

                if(dtWebAppointment!=null)
                {
                    if(dtWebAppointment.Rows.Count>0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                    }
                }

                foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                {
                    try
                    {
                        tmpPatient_id = "0";
                        tmpPatient_Gur_id = "0";
                        tmpAppt_EHR_id = 0;
                        TmpWebPatientName = "";
                        TmpWebReversePatientName = "";

                        Utility.CreatePatientNameTOCompare(dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), ref TmpWebPatientName, ref TmpWebReversePatientName);

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
                        DataTable dtBookOperatoryApptWiseDateTime = SynchEaglesoftBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), strDbConnString);
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
                                        if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["start_Time"].ToString()))
                                            && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["End_Time"].ToString())))
                                        {
                                            IsConflict = true;
                                            break;
                                        }
                                        else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["start_Time"].ToString()))
                                            && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["End_Time"].ToString())))
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
                        if (tmpIdealOperatory == 0)
                        {
                            DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appointment_EHR_id = " + appointment_EHR_id).CopyToDataTable();

                            bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id, dtTemp, appointment_EHR_id, Location_ID);
                        }
                        else
                        {

                            #region Set Patient
                            tmpPatient_id = dtDtxRow["Patient_EHR_Id"].ToString();

                            if (tmpPatient_id == "0" || tmpPatient_id == "")
                            {
                                DateTime DOB;
                                DOB = new DateTime();
                                if (!string.IsNullOrEmpty(dtDtxRow["birth_date"].ToString().Trim()))
                                {
                                    DOB = Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim());
                                }
                                tmpPatient_id = GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtEagleSoftPatient, tmpPatient_id, dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), strDbConnString, dtDtxRow["Clinic_Number"].ToString(), DOB, dtDtxRow["Provider_EHR_ID"].ToString());
                            }

                            #endregion

                            if (tmpPatient_id != "0")
                            {

                                tmpAppt_EHR_id = SynchEaglesoftBAL.Save_Appointment_Local_To_EagleSoft(TmpWebPatientName, tmpStartTime, tmpEndTime, tmpPatient_id.ToString(), tmpIdealOperatory.ToString(), "1", dtDtxRow["ApptType_EHR_ID"].ToString().Trim(),
                                                                                                                   Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()), tmpApptProv, dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "0", dtDtxRow["comment"].ToString().Trim(),
                                                                                                                   false, false, false, false, (dtDtxRow["appt_treatmentcode"].ToString()), strDbConnString);

                                if (tmpAppt_EHR_id > 0)
                                {
                                    bool isApptId_Update = SynchEaglesoftBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim(), Service_Install_Id);
                                }
                            }

                            #region Appointment Sync
                            SyncDataEagleSoft_AppointmentFromEvent(strDbConnString, Clinic_Number, Service_Install_Id, tmpAppt_EHR_id.ToString(), tmpPatient_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                            #endregion
                        }
                    }

                    catch (Exception ex)
                    {
                        Utility.WritetoAditEventErrorLogFile_Static("Error in appt booking  " + ex.Message);
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                return false;
            }
        }

        public static void SyncDataEagleSoft_AppointmentFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        {
            try
            {
                SynchEagleSoft_AppointmentsPatientFromEvents(strDbString, Clinic_Number, Service_Install_Id, strPatID);
                SynchDataEagleSoft_PatientStatusFromEvent(strDbString, Clinic_Number, Service_Install_Id, strPatID);
                SynchDataLiveDB_Push_Patient(strPatID);

                DataTable dtEaglesoftAppointment = SynchEaglesoftBAL.GetEaglesoftAppointmentData(strDbString, strApptID);
                dtEaglesoftAppointment.Columns.Add("Provider_EHR_ID", typeof(string));
                dtEaglesoftAppointment.Columns.Add("ProviderName", typeof(string));
                dtEaglesoftAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                dtEaglesoftAppointment.Columns.Add("ProcedureDesc", typeof(string));
                dtEaglesoftAppointment.Columns.Add("ProcedureCode", typeof(string));
                dtEaglesoftAppointment.Columns.Add("InsUptDlt", typeof(int));

                string ProcedureDesc = "";
                string ProcedureCode = "";

                DataTable DtEaglesoftAppointment_Procedures_Data = SynchEaglesoftBAL.GetEaglesoftAppointment_Procedures_Data(strDbString, strApptID);

                DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData(Service_Install_Id, strApptID);

                int cntCurRecord = 0;

                foreach (DataRow dtDtxRow in dtEaglesoftAppointment.Rows)
                {
                    DataTable dtEaglesoftAppointmentProider = SynchEaglesoftBAL.GetEaglesoftAppointmentProviderData(dtDtxRow["Appt_EHR_ID"].ToString(), strDbString);
                    string ProvIDList = string.Empty;
                    string ProvNameList = string.Empty;
                    foreach (DataRow dtr in dtEaglesoftAppointmentProider.Rows)
                    {
                        ProvIDList = ProvIDList + dtr["provider_id"].ToString() + " ; ";
                        ProvNameList = ProvNameList + dtr["ProviderName"].ToString() + " ; ";
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
                    dtDtxRow["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString();

                    ////////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                    ProcedureDesc = "";
                    ProcedureCode = "";

                    DataRow[] dtCurApptProcedure = DtEaglesoftAppointment_Procedures_Data.Select("appointment_id = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "'");

                    foreach (var dtSinProc in dtCurApptProcedure.ToList())
                    {
                        ProcedureDesc = ProcedureDesc + dtSinProc["tooth"].ToString() + dtSinProc["surface"].ToString() + dtSinProc["schedabbr"].ToString();
                        ProcedureCode = ProcedureCode + dtSinProc["ADA_Code"].ToString();
                    }
                    if (ProcedureDesc.ToString().Length > 1)
                    {
                        ProcedureDesc = ProcedureDesc.Substring(0, ProcedureDesc.Length - 1);
                        dtDtxRow["ProcedureDesc"] = ProcedureDesc;
                    }
                    if (ProcedureCode.ToString().Length > 1)
                    {
                        ProcedureCode = ProcedureCode.Substring(0, ProcedureCode.Length - 1);
                        dtDtxRow["ProcedureCode"] = ProcedureCode;
                    }

                    //////////////////////////////////

                    DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                    if (row.Length > 0)
                    {
                        int commentlen = 1999;
                        if (dtDtxRow["comment"].ToString().Trim().Length < commentlen)
                        {
                            commentlen = dtDtxRow["comment"].ToString().Trim().Length;
                        }
                        if (dtDtxRow["Last_Name"].ToString().Trim() != row[0]["Last_Name"].ToString().Trim())
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
                            if (dtDtxRow["Operatory_EHR_ID"].ToString().Trim() == "-1" || dtDtxRow["Operatory_EHR_ID"].ToString().Trim() == "" || dtDtxRow["classification"].ToString().Trim() == "16" || dtDtxRow["classification"].ToString().Trim() == "32" || dtDtxRow["classification"].ToString().Trim() == "64")
                            {
                                if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                                {
                                    dtDtxRow["InsUptDlt"] = 3;
                                }
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                        }
                        else if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == true && dtDtxRow["classification"].ToString().Trim() == "1")
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
                        else if (dtDtxRow["ApptType_EHR_ID"].ToString().Trim() != row[0]["ApptType_EHR_ID"].ToString().Trim())
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Appt_DateTime"].ToString().Trim()))
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Appt_EndDateTime"].ToString().Trim()))
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (!string.IsNullOrEmpty(dtDtxRow["Birth_date"].ToString()) && string.IsNullOrEmpty(row[0]["Birth_date"].ToString()))
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (string.IsNullOrEmpty(dtDtxRow["Birth_date"].ToString()) && !string.IsNullOrEmpty(row[0]["Birth_date"].ToString()))
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (!string.IsNullOrEmpty(dtDtxRow["Birth_date"].ToString()) && !string.IsNullOrEmpty(row[0]["Birth_date"].ToString()) && (Convert.ToDateTime(dtDtxRow["Birth_date"].ToString().Trim()) != Convert.ToDateTime(row[0]["Birth_date"].ToString().Trim())))
                        {
                            dtDtxRow["InsUptDlt"] = 4;
                        }
                        else if (dtDtxRow["appointment_status_ehr_key"].ToString().Trim() != row[0]["appointment_status_ehr_key"].ToString().Trim())
                        {
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
                        else if (dtDtxRow["is_asap"].ToString().Trim() != row[0]["is_asap"].ToString().Trim())
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
                        //rooja 28-4-23- https://app.asana.com/0/0/1204426000385832/f - //condition for ' '  or null cell_phone in ehr
                        try
                        {
                            DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Utility.ConvertContactNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) + "' AND ISNULL(Appt_EHR_ID,'0') ='0'");

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
                        catch (Exception ex)
                        {

                        }
                    }
                }
                foreach (DataRow dtlApptRow in dtLocalAppointment.Rows)
                {
                    try
                    {
                        DataRow[] rowBlcOpt = dtEaglesoftAppointment.Copy().Select("Appt_EHR_ID = '" + dtlApptRow["Appt_EHR_ID"].ToString().Trim() + "' ");
                        if (rowBlcOpt.Length > 0)
                        { }
                        else
                        {
                            if (Convert.ToBoolean(dtlApptRow["is_deleted"].ToString().Trim()) == false && (dtlApptRow["Appt_EHR_ID"].ToString().Trim()) != "0")
                            {
                                DataRow ESApptDtldr = dtEaglesoftAppointment.NewRow();
                                ESApptDtldr["Appt_EHR_ID"] = dtlApptRow["Appt_EHR_ID"].ToString().Trim();
                                ESApptDtldr["InsUptDlt"] = 3;
                                dtEaglesoftAppointment.Rows.Add(ESApptDtldr);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.WritetoAditEventErrorLogFile_Static("CallSynch_Appointment - Appt_EHR_ID error : " + ex.Message.ToString());
                        Utility.WritetoAditEventErrorLogFile_Static("Appt_EHR_ID : " + dtlApptRow["Appt_EHR_ID"].ToString().Trim());
                    }
                }
                dtEaglesoftAppointment.AcceptChanges();

                if (dtEaglesoftAppointment != null && dtEaglesoftAppointment.Rows.Count > 0)
                {
                    if (!dtEaglesoftAppointment.Columns.Contains("Appt_Web_ID"))
                    {
                        dtEaglesoftAppointment.Columns.Add("Appt_Web_ID");
                    }
                    dtEaglesoftAppointment.Rows[0]["Appt_Web_ID"] = strWebID;

                    bool status = SynchEaglesoftBAL.Save_Appointment_Eaglesoft_To_Local(dtEaglesoftAppointment, Service_Install_Id);

                    if (status)
                    {
                        Utility.WritetoAditEventSyncLogFile_Static("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchDataLiveDB_Push_Appointment(strApptID);
                    }
                    else
                    {
                        Utility.WritetoAditEventErrorLogFile_Static("[Appointment Sync (" + Utility.Application_Name + " Db : " + Service_Install_Id + " to Local Database) ] Error.");
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncDataEagleSoft_AppointmentFromEvent Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        private static void SynchEagleSoft_AppointmentsPatientFromEvents(string strDbString, string Clinic_Number, string Service_Install_Id, string strPatID)
        {
            try
            {
                DataTable dtEaglesoftAppointmensPatient = SynchEaglesoftBAL.GetEaglesoftAppointmentsPatientData(strDbString, strPatID);
                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData(Service_Install_Id, strPatID);

                DataTable dtSaveRecords = new DataTable();
                dtSaveRecords = dtLocalPatient.Clone();

                var itemsToBeAdded = (from EaglesoftPatient in dtEaglesoftAppointmensPatient.AsEnumerable()
                                      join LocalPatient in dtLocalPatient.AsEnumerable()
                                      on EaglesoftPatient["Patient_EHR_ID"].ToString().Trim() + "_" + EaglesoftPatient["Clinic_Number"].ToString().Trim()
                                      equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                      //on new { PatID = OpenDentalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = OpenDentalPatient["Clinic_Number"].ToString().Trim() }
                                      //equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                      into matchingRows
                                      from matchingRow in matchingRows.DefaultIfEmpty()
                                      where matchingRow == null
                                      select EaglesoftPatient).ToList();
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

                var itemsToBeUpdated = (from EaglesoftPatient in dtEaglesoftAppointmensPatient.AsEnumerable()
                                        join LocalPatient in dtLocalPatient.AsEnumerable()
                                        on EaglesoftPatient["Patient_EHR_ID"].ToString().Trim() + "_" + EaglesoftPatient["Clinic_Number"].ToString().Trim()
                                        equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                        where
                                           (EaglesoftPatient["nextvisit_date"] != DBNull.Value && EaglesoftPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(EaglesoftPatient["nextvisit_date"]) : DateTime.Now)
                                                         !=
                                                         (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)

                                                         ||

                                                         (EaglesoftPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())

                                                         ||

                                                         (EaglesoftPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())

                                                         || (EaglesoftPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                                         || (EaglesoftPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                                         || (Utility.ConvertContactNumber(EaglesoftPatient["Home_Phone"].ToString().Trim())) != (Utility.ConvertContactNumber(LocalPatient["Home_Phone"].ToString().Trim()))
                                                         || (EaglesoftPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                                         || (EaglesoftPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                                         || (EaglesoftPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                                         || (Utility.ConvertContactNumber(EaglesoftPatient["Mobile"].ToString().Trim())) != (Utility.ConvertContactNumber(LocalPatient["Mobile"].ToString().Trim()))
                                                         || (EaglesoftPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
                                        select EaglesoftPatient).ToList();

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
                    bool status = SynchEaglesoftBAL.Save_Patient_Eaglesoft_To_Local_New(dtSaveRecords, Clinic_Number, Service_Install_Id, true);
                }

            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchEagleSoft_AppointmentsPatientFromEvents Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public static void SynchDataEagleSoft_PatientStatusFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strPatID)
        {
            try
            {
                DataTable dtEaglesoftPatientStatus = new DataTable();
                dtEaglesoftPatientStatus = SynchEaglesoftBAL.GetEaglesoftPatientStatusData(Clinic_Number, strDbString, strPatID);
                if (dtEaglesoftPatientStatus != null && dtEaglesoftPatientStatus.Rows.Count > 0)
                {
                    SynchLocalBAL.UpdatePatient_Status(dtEaglesoftPatientStatus, Service_Install_Id, Clinic_Number, strPatID);
                    //SynchDataLiveDB_Push_PatientStatus(Convert.ToInt32(Service_Install_Id), Convert.ToInt32(Clinic_Number), strPatID);
                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataEagleSoft_PatientStatus Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        public void SynchDataLocalToEagleSoft_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
        {
            string strDbConnString = "";
            string Location_ID = "";
            string strPatientID = "";
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

                DataRow[] drloc = Utility.DtLocationList.Copy().Select("Service_Install_Id = '" + Service_Install_Id + "' ");
                if (Convert.ToBoolean(drloc[0]["AditLocationSyncEnable"].ToString()))
                {
                    DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData(Service_Install_Id, strPatientFormID);

                    DataColumn dcCol = new DataColumn();
                    dcCol.ColumnName = "TableName";
                    dcCol.DefaultValue = "Patient";
                    dtWebPatient_Form.Columns.Add(dcCol);
                    // Add Columns Name As TableName
                    // default value  = 'patient'

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
                            dtDtxRow["ehrfield"] = "first_name";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "last_name")
                        {
                            dtDtxRow["ehrfield"] = "last_name";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "mobile")
                        {
                            dtDtxRow["ehrfield"] = "cell_phone";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_one")
                        {
                            dtDtxRow["ehrfield"] = "Address_1";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "address_two")
                        {
                            dtDtxRow["ehrfield"] = "Address_2";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "birth_date")
                        {
                            dtDtxRow["ehrfield"] = "Birth_Date";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "city")
                        {
                            dtDtxRow["ehrfield"] = "City";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "email")
                        {
                            dtDtxRow["ehrfield"] = "email_address";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "home_phone")
                        {
                            dtDtxRow["ehrfield"] = "Home_Phone";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "marital_status")
                        {
                            dtDtxRow["ehrfield"] = "marital_status";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "middle_name")
                        {
                            dtDtxRow["ehrfield"] = "middle_initial";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "preferred_name")
                        {
                            dtDtxRow["ehrfield"] = "preferred_name";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "pri_provider_id")
                        {
                            dtDtxRow["ehrfield"] = "preferred_dentist";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "PRIMARY_SUBSCRIBER_ID")
                        {
                            dtDtxRow["ehrfield"] = "prim_member_id";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "primary_insurance_companyname")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_email")
                        {
                            dtDtxRow["ehrfield"] = "receive_email";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "receive_sms")
                        {
                            dtDtxRow["ehrfield"] = "receives_sms";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "salutation")
                        {
                            dtDtxRow["ehrfield"] = "Salutation";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "sec_provider_id")
                        {
                            dtDtxRow["ehrfield"] = "preferred_hygienist";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToUpper() == "SECONDARY_SUBSCRIBER_ID")
                        {
                            dtDtxRow["ehrfield"] = "sec_member_id";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "secondary_insurance_companyname")
                        {
                            dtDtxRow["ehrfield"] = "";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "sex")
                        {
                            dtDtxRow["ehrfield"] = "sex";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "work_phone")
                        {
                            dtDtxRow["ehrfield"] = "work_phone";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "zipcode")
                        {
                            dtDtxRow["ehrfield"] = "zipcode";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "emergencycontactname")
                        {
                            dtDtxRow["TableName"] = "patient_answers";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "emergencycontactnumber")
                        {
                            dtDtxRow["TableName"] = "patient_answers";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "school")
                        {
                            dtDtxRow["ehrfield"] = "school";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "ssn")
                        {
                            dtDtxRow["ehrfield"] = "social_security";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim() == "driverlicense")
                        {
                            dtDtxRow["ehrfield"] = "drivers_license";
                        }
                        dtWebPatient_Form.AcceptChanges();
                    }
                    if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                    {
                        bool Is_Record_Update = SynchEaglesoftBAL.Save_Patient_Form_Local_To_EagleSoft(dtWebPatient_Form, strDbConnString, Service_Install_Id);
                    }

                    try
                    {
                        GetMedicalHistoryRecords(Service_Install_Id, strPatientFormID);

                        strPatientID = SynchLocalBAL.Get_Patient_EHR_ID_from_Patient_Form(strPatientFormID);

                        PushMedicalHistoryRecordsTOEaglesoft(strDbConnString, Service_Install_Id, strPatientFormID, strPatientID);

                        ObjGoalBase.WriteToSyncLogFile("Medical_History_Save Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    catch (Exception ex2)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Medical_History_Save Sync (Local Database To " + Utility.Application_Name + ") ]" + ex2.Message);
                    }

                    try
                    {
                        if (SynchEaglesoftBAL.SaveAllergiesToEaglesoft(strDbConnString, Service_Install_Id, strPatientFormID))
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                    }
                    try
                    {
                        if (SynchEaglesoftBAL.DeleteAllergiesToEaglesoft(strDbConnString, Service_Install_Id, strPatientFormID))
                        {
                            ObjGoalBase.WriteToSyncLogFile("Delete Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Alert Sync (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                    }

                    bool isRecordDeleted = false, isRecordSaved = false;
                    string Patient_EHR_IDS = "";
                    string Delete_Patient_EHR_ids = "";
                    string Save_Patient_EHR_ids = "";
                    try
                    {
                        if (SynchEaglesoftBAL.DeleteMedicationToEaglesoft(strDbConnString, Service_Install_Id, ref isRecordDeleted, ref Delete_Patient_EHR_ids, strPatientFormID))
                        {
                            ObjGoalBase.WriteToSyncLogFile("Delete Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                    }
                    try
                    {
                        if (SynchEaglesoftBAL.SaveMedicationToEaglesoft(strDbConnString, Service_Install_Id, ref isRecordSaved, ref Save_Patient_EHR_ids, strPatientFormID))
                        {
                            ObjGoalBase.WriteToSyncLogFile("Save Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Save Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]");
                        }
                    }
                    catch (Exception ex1)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Save Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex1.Message);
                    }

                    if (isRecordSaved || isRecordDeleted)
                    {
                        Patient_EHR_IDS = (Delete_Patient_EHR_ids + Save_Patient_EHR_ids).TrimEnd(',');
                        if (Patient_EHR_IDS != "")
                        {
                            SynchDataEagleSoft_PatientMedication(Patient_EHR_IDS);
                        }
                    }

                    #region PatientInformation Document
                    try
                    {
                        GetPatientDocument(Service_Install_Id, strPatientFormID);
                        GetPatientDocument_New(Service_Install_Id, strPatientFormID);
                        SynchEaglesoftBAL.Save_Document_in_EagleSoft(strDbConnString, Service_Install_Id, strDocumentPath, strPatientFormID);
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Form Document Sync (Service Install Id : " + Service_Install_Id + ".Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }

                    #endregion

                    string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing(Service_Install_Id, strPatientFormID);
                    if (Call_Importing.ToLower() != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Importing status : " + Call_Importing);
                    }

                    string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed(Service_Install_Id, strPatientFormID);
                    if (Call_Completed.ToLower() != "success")
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Form API error with Completed status : " + Call_Completed);
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

                    ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
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
