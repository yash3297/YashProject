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
using System.Globalization;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable
        DateTime lastPatientFormcalled = DateTime.Now;
        bool IsDentrixProviderSync = false;
        bool IsDentrixOperatorySync = false;
        bool IsDentrixApptTypeSync = false;
        bool IsDocumentUpload = false;
        bool IsDocumentTreatmentUpload = false;
        bool IsDentrixPatinetFormSync = false;
        bool IsDocumentInsuranceCarrierUpload = false;
        //bool Is_synched = false;
        //bool Is_synched_Provider = false;
        //bool Is_synched_Speciality = false;
        //bool Is_synched_Operatory = false;
        //bool Is_synched_OperatoryEvent = false;
        //bool Is_synched_Type = false;
        //bool Is_synched_Appointment = false;
        //bool Is_synched_RecallType = false;
        //bool Is_synched_ApptStatus = false;
        private BackgroundWorker bwSynchDentrix_Appointment = null;
        private System.Timers.Timer timerSynchDentrix_Appointment = null;

        private BackgroundWorker bwSynchDentrix_OperatoryEvent = null;
        private System.Timers.Timer timerSynchDentrix_OperatoryEvent = null;

        private BackgroundWorker bwSynchDentrix_Provider = null;
        private System.Timers.Timer timerSynchDentrix_Provider = null;

        private BackgroundWorker bwSynchDentrix_ProviderOfficeHours = null;
        private System.Timers.Timer timerSynchDentrix_ProviderOfficeHours = null;

        private BackgroundWorker bwSynchDentrix_Speciality = null;
        private System.Timers.Timer timerSynchDentrix_Speciality = null;

        private BackgroundWorker bwSynchDentrix_Operatory = null;
        private System.Timers.Timer timerSynchDentrix_Operatory = null;

        private BackgroundWorker bwSynchDentrix_OperatoryHours = null;
        private System.Timers.Timer timerSynchDentrix_OperatoryHours = null;

        private BackgroundWorker bwSynchDentrix_ApptType = null;
        private System.Timers.Timer timerSynchDentrix_ApptType = null;

        private BackgroundWorker bwSynchDentrix_Patient = null;
        private System.Timers.Timer timerSynchDentrix_Patient = null;

        private BackgroundWorker bwSynchDentrix_Disease = null;
        private System.Timers.Timer timerSynchDentrix_Disease = null;

        private BackgroundWorker bwSynchDentrix_RecallType = null;
        private System.Timers.Timer timerSynchDentrix_RecallType = null;

        private BackgroundWorker bwSynchDentrix_ApptStatus = null;
        private System.Timers.Timer timerSynchDentrix_ApptStatus = null;

        private BackgroundWorker bwSynchDentrix_User = null;
        private System.Timers.Timer timerSynchDentrix_User = null;

        private BackgroundWorker bwSynchDentrix_Holidays = null;
        private System.Timers.Timer timerSynchDentrix_Holidays = null;

        private BackgroundWorker bwSynchLocalToDentrix_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchLocalToDentrix_Appointment = null;

        private BackgroundWorker bwSynchLocalToDentrix_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToDentrix_Patient_Form = null;


        private BackgroundWorker bwSynchDentrix_MedicalHistory = null;
        private System.Timers.Timer timerSynchDentrix_MedicalHistory = null;

        private BackgroundWorker bwSynchDentrix_PatientPayment = null;
        private System.Timers.Timer timerSynchDentrix_PatientPayment = null;

        private BackgroundWorker bwSynchDentrix_Insurance = null;
        private System.Timers.Timer timerSynchDentrix_Insurance = null;

        // private BackgroundWorker bwSynchDentrix_PatientWiseRecallDate = null;
        //private System.Timers.Timer timerSynchDentrix_PatientWiseRecallDate = null;

        #endregion

        private void CallSynchDentrixToLocal()
        {

            #region New Code
            //bool IsBackUpActive = CheckBackUpActive();

            //if (IsBackUpActive == false)
            //{

            //    if (Utility.AditSync)
            //    {

            //        // SynchDataLocalToDentrix_Patient_Form();
            //        fncSynchDataLocalToDentrix_Patient_Form();

            //        SynchDataDentrix_Speciality();
            //        fncSynchDataDentrix_Speciality();

            //        SynchDataDentrix_Operatory();
            //        fncSynchDataDentrix_Operatory();

            //        SynchDataDentrix_Provider();
            //        fncSynchDataDentrix_Provider();

            //        //if (Utility.Application_Version.ToLower() != "DTX G5".ToLower())
            //        //{
            //        SynchDataDentrix_ProviderOfficeHours();
            //        fncSynchDataDentrix_ProviderOfficeHours();
            //        SynchDataDentrix_OperatoryOfficeHours();
            //        fncSynchDataDentrix_OperatoryHours();
            //        // }

            //        //SynchDataDentrix_OperatoryEvent();
            //        //fncSynchDataDentrix_OperatoryEvent();

            //        SynchDataDentrix_ApptType();
            //        fncSynchDataDentrix_ApptType();

            //        //SynchDataDentrix_Appointment();
            //        //fncSynchDataDentrix_Appointment();
            //        SynchDataDentrix_Disease();
            //        fncSynchDataDentrix_Disease();

            //        fncSynchDataLocalToDentrix_Appointment();

            //        //SynchDataDentrix_Patient();
            //        //fncSynchDataDentrix_Patient();

            //        SynchDataDentrix_RecallType();
            //        fncSynchDataDentrix_RecallType();

            //        SynchDataDentrix_ApptStatus();
            //        fncSynchDataDentrix_ApptStatus();

            //        SynchDataDentrix_Holidays();
            //        fncSynchDataDentrix_Holidays();

            //        //SynchDataDentrix_MedicleHistory();
            //        fncSynchDataDentrix_MedicalHistory();

            //    }
            //}

            //else
            //{

            //     System.Windows.Forms.MessageBox.Show("Please wait... EHR is performing a BackUp");

            //}



            #endregion

            #region Old Code

            if (Utility.AditSync)
            {

                // SynchDataLocalToDentrix_Patient_Form();
                //fncSynchDataLocalToDentrix_Patient_Form(); // Changed for testing to be reverted


                fncSynchDataDentrix_PatientPayment();

                SynchDataDentrix_Speciality();
                fncSynchDataDentrix_Speciality();

                SynchDataDentrix_Operatory();
                fncSynchDataDentrix_Operatory();

                SynchDataDentrix_Provider();
                fncSynchDataDentrix_Provider();

                //if (Utility.Application_Version.ToLower() != "DTX G5".ToLower())
                //{
                //SynchDataDentrix_ProviderOfficeHours();
                fncSynchDataDentrix_ProviderOfficeHours();
                //SynchDataDentrix_OperatoryOfficeHours();
                fncSynchDataDentrix_OperatoryHours();
                // }

                //SynchDataDentrix_OperatoryEvent();
                //fncSynchDataDentrix_OperatoryEvent();

                SynchDataDentrix_ApptType();
                fncSynchDataDentrix_ApptType();

                //SynchDataDentrix_Appointment();
                //fncSynchDataDentrix_Appointment();
                SynchDataDentrix_Disease();
                fncSynchDataDentrix_Disease();

                fncSynchDataLocalToDentrix_Appointment();

                //SynchDataDentrix_Patient();
                //fncSynchDataDentrix_Patient();

                SynchDataDentrix_RecallType();
                fncSynchDataDentrix_RecallType();

                SynchDataDentrix_User();
                fncSynchDataCleardent_User();

                SynchDataDentrix_ApptStatus();
                fncSynchDataDentrix_ApptStatus();

                SynchDataDentrix_Holidays();
                fncSynchDataDentrix_Holidays();

                //SynchDataDentrix_MedicleHistory();


                fncSynchDataDentrix_MedicalHistory();

                fncSynchDataDentrix_Insurance();
                SynchDataDentrix_Insurance();

                fncSynchDataLocalToDentrix_Patient_Form(); // Changed for testing to be reverted

            }

            #endregion

        }

        #region Synch Appointment

        private void fncSynchDataDentrix_Appointment()
        {
            InitBgWorkerDentrix_Appointment();
            InitBgTimerDentrix_Appointment();
        }

        private void InitBgTimerDentrix_Appointment()
        {
            timerSynchDentrix_Appointment = new System.Timers.Timer();
            this.timerSynchDentrix_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchDentrix_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_Appointment_Tick);
            timerSynchDentrix_Appointment.Enabled = true;
            timerSynchDentrix_Appointment.Start();
            timerSynchDentrix_Appointment_Tick(null, null);
        }

        private void InitBgWorkerDentrix_Appointment()
        {
            bwSynchDentrix_Appointment = new BackgroundWorker();
            bwSynchDentrix_Appointment.WorkerReportsProgress = true;
            bwSynchDentrix_Appointment.WorkerSupportsCancellation = true;
            bwSynchDentrix_Appointment.DoWork += new DoWorkEventHandler(bwSynchDentrix_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchDentrix_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_Appointment_RunWorkerCompleted);
        }

        private void timerSynchDentrix_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_Appointment.Enabled = false;
                MethodForCallSynchOrderDentrix_Appointment();
            }
        }

        public void MethodForCallSynchOrderDentrix_Appointment()
        {
            System.Threading.Thread procThreadmainDentrix_Appointment = new System.Threading.Thread(this.CallSyncOrderTableDentrix_Appointment);
            procThreadmainDentrix_Appointment.Start();
        }

        public void CallSyncOrderTableDentrix_Appointment()
        {
            if (bwSynchDentrix_Appointment.IsBusy != true)
            {
                bwSynchDentrix_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLocalToDentrix_Patient_Payment();
                SynchDataDentrix_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_Appointment.Enabled = true;
        }

        public void SynchDataDentrix_Appointment()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                IsDentrixProviderSync = true;
                IsDentrixOperatorySync = true;
                IsDentrixApptTypeSync = true;
                Is_synched_Appointment = true;
            }
            if (IsDentrixProviderSync && IsDentrixOperatorySync && IsDentrixApptTypeSync && Is_synched_Appointment && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    //New line by yogesh for sync appointments patients first
                    try
                    {
                        SynchDataDentrix_AppointmentsPatient_New();
                    }
                    catch (Exception ex)
                    {
                        SynchDataDentrix_AppointmentsPatient();
                    }
                    if (IsParientFirstSync)
                    {
                        SynchDataDentrix_NewPatient();
                    }
                    if (IS_Appt_Patient_Sync)
                    {
                        SynchDataDentrix_PatientStatus();
                        Is_synched_Appointment = false;
                        DataTable dtDentrixAppointment = SynchDentrixBAL.GetDentrixAppointmentData();

                        dtDentrixAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                        dtDentrixAppointment.Columns.Add("InsUptDlt", typeof(int));
                        dtDentrixAppointment.Columns.Add("ProcedureDesc", typeof(string));
                        dtDentrixAppointment.Columns.Add("ProcedureCode", typeof(string));

                        string ProcedureDesc = "";
                        string ProcedureCode = "";                        
                        DataTable DtDentrixAppointment_Procedures_Data = SynchDentrixBAL.GetDentrixAppointment_Procedures_Data();


                        DataTable dtDentrixDeletedAppointment = new DataTable();
                        try
                        {
                            dtDentrixDeletedAppointment = SynchDentrixBAL.GetDentrixDeletedAppointmentData();
                            dtDentrixDeletedAppointment.Columns.Add("InsUptDlt", typeof(int));

                        }
                        catch (Exception exc)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Appointment Sync Inner (" + Utility.Application_Name + " to Local Database) ]" + exc.Message);

                        }
                        DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData("1");

                        DataTable dtLocalCompareForDeletedAppointment = SynchLocalBAL.GetLocalCompareForDeletedAppointmentData("1");
                        //DataTable dtDentrixOperatory = SynchDentrixBAL.GetDentrixOperatoryData();

                        string MobileEmail = string.Empty;
                        string Mobile_Contact = string.Empty;
                        string Email = string.Empty;
                        string AppointmentStatus = string.Empty;
                        string Home_Contact = string.Empty;
                        string Address = string.Empty;
                        string City = string.Empty;
                        string State = string.Empty;
                        string Zipcode = string.Empty;
                        int apptFlag = 0;
                        foreach (DataRow dtDtxRow in dtDentrixAppointment.Rows)
                        {
                            MobileEmail = string.Empty;
                            Mobile_Contact = string.Empty;
                            Email = string.Empty;
                            AppointmentStatus = string.Empty;
                            Home_Contact = string.Empty;
                            Address = string.Empty;
                            City = string.Empty;
                            State = string.Empty;
                            Zipcode = string.Empty;
                            apptFlag = 0;
                            if (dtDtxRow["patId"].ToString() == "0" || dtDtxRow["patId"].ToString() == string.Empty)
                            {
                                if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                                {
                                    Mobile_Contact = Utility.ConvertContactNumber(dtDtxRow["Phone"].ToString().Trim());
                                    Email = "";
                                }
                                else
                                {
                                    MobileEmail = dtDtxRow["notetext"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace("\n", "").Trim();
                                    Mobile_Contact = string.Empty;
                                    Email = string.Empty;
                                    string Temp = "";
                                    if (MobileEmail != string.Empty && MobileEmail.Length >= 10)
                                    {
                                        Temp = MobileEmail.Substring(0, 10);
                                        if (Temp.All(char.IsDigit))
                                        {
                                            Mobile_Contact = Temp;
                                        }
                                        else
                                        {
                                            Mobile_Contact = string.Empty;
                                        }
                                        Temp = MobileEmail.Substring(10, MobileEmail.Length - 10);
                                        if (Utility.IsValidEmailAddress(Temp))
                                        {
                                            Email = Temp.ToString();
                                        }
                                        else
                                        {
                                            Email = string.Empty;
                                        }
                                        try
                                        {
                                            double isMobile = Convert.ToDouble(Mobile_Contact);
                                        }
                                        catch (FormatException)
                                        {
                                            Mobile_Contact = string.Empty;
                                            Email = MobileEmail.ToString();
                                        }
                                    }
                                }
                                dtDtxRow["patMobile"] = Mobile_Contact;
                                dtDtxRow["patEmail"] = Email;
                            }
                            else
                            {
                                Mobile_Contact = dtDtxRow["patMobile"].ToString();
                                Email = dtDtxRow["patEmail"].ToString();
                            }

                            ////////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                            ProcedureDesc = "";
                            ProcedureCode = "";

                            DataRow[] dtCurApptProcedure = DtDentrixAppointment_Procedures_Data.Select("apptid = '" + dtDtxRow["appointment_id"].ToString().Trim() + "'");

                            foreach (var dtSinProc in dtCurApptProcedure.ToList())
                            {
                                if (dtSinProc["Tooth"].ToString() != "" && dtSinProc["Tooth"].ToString() != "0")
                                {
                                    ProcedureDesc = ProcedureDesc + dtSinProc["Tooth"].ToString().Trim() + '-';
                                }
                                if (dtSinProc["Surface"].ToString() != "" && dtSinProc["Surface"].ToString() != "0")
                                {
                                    ProcedureDesc = ProcedureDesc + dtSinProc["Surface"].ToString().Trim() + '-';
                                }
                                if (dtSinProc["abbrevdescript"].ToString() != "" && dtSinProc["abbrevdescript"].ToString() != "0")
                                {
                                    ProcedureDesc = ProcedureDesc + dtSinProc["abbrevdescript"].ToString().Trim() + ',';
                                }
                                if (dtSinProc["ProcedureCode"].ToString() != "" && dtSinProc["ProcedureCode"].ToString() != "0")
                                {
                                    ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim() + ',';
                                }
                                //ProcedureDesc = ProcedureDesc + dtSinProc["abbrevdescript"].ToString().Trim() + ',';
                                //ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim() + ',';
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

                            DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appointment_id"].ToString().Trim() + "' ");
                            if (row.Length > 0)
                            {
                                // dtDtxRow["InsUptDlt"] = "UID";
                                if (row[0]["is_asap"] != null && row[0]["is_asap"].ToString() != string.Empty && Convert.ToBoolean(row[0]["is_asap"]) == true)
                                {
                                    apptFlag = 2;
                                }
                                else
                                {
                                    apptFlag = Convert.ToInt16(dtDtxRow["appt_flag"]);
                                }

                                if (row[0]["EHR_Entry_DateTime"] != DBNull.Value && Convert.ToDateTime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EHR_Entry_DateTime"]))
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else
                                {
                                    MobileEmail = "";
                                    if (dtDtxRow["patId"].ToString() == "0" || dtDtxRow["patId"].ToString() == string.Empty)
                                    {
                                        Home_Contact = dtDtxRow["patient_phone"].ToString().Trim();
                                        Address = dtDtxRow["street1"].ToString().Trim();
                                        City = dtDtxRow["city"].ToString().Trim();
                                        State = dtDtxRow["state"].ToString().Trim();
                                        Zipcode = dtDtxRow["zipcode"].ToString().Trim();
                                        dtDtxRow["patCity"] = dtDtxRow["city"].ToString().Trim();
                                        dtDtxRow["patAddress"] = dtDtxRow["street1"].ToString().Trim();
                                        dtDtxRow["patState"] = dtDtxRow["state"].ToString().Trim();
                                        dtDtxRow["patZipcode"] = dtDtxRow["zipcode"].ToString().Trim();
                                        dtDtxRow["patHomephone"] = dtDtxRow["patient_phone"].ToString().Trim();
                                    }
                                    else
                                    {
                                        Mobile_Contact = dtDtxRow["patMobile"].ToString();
                                        Email = dtDtxRow["patEmail"].ToString();
                                        Home_Contact = dtDtxRow["patHomephone"].ToString().Trim();
                                        Address = dtDtxRow["patAddress"].ToString().Trim();
                                        City = dtDtxRow["patCity"].ToString().Trim();
                                        State = dtDtxRow["patState"].ToString().Trim();
                                        Zipcode = dtDtxRow["patZipcode"].ToString().Trim();
                                    }

                                    if (dtDtxRow["appointment_status_ehr_key"].ToString().Trim() == "-106")
                                    {
                                        AppointmentStatus = "<COMPLETE>";
                                    }
                                    else if (dtDtxRow["appointment_status_ehr_key"].ToString().Trim() == "0")
                                    {
                                        AppointmentStatus = "<none>";
                                    }
                                    else
                                    {
                                        AppointmentStatus = dtDtxRow["Appointment_Status"].ToString().Trim();
                                    }

                                    if (dtDtxRow["birth_date"] != null && row[0]["birth_date"] == null)
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["birth_date"] == null && row[0]["birth_date"] != null)
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["birth_date"] != null && dtDtxRow["birth_date"].ToString().Trim() != string.Empty && row[0]["birth_date"] != null && row[0]["birth_date"].ToString().Trim() != string.Empty
                                        && Convert.ToDateTime(dtDtxRow["birth_date"]).ToShortDateString() != Convert.ToDateTime(row[0]["birth_date"]).ToShortDateString())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["op_title"].ToString().Trim() != row[0]["Operatory_Name"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["op_id"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Convert.ToInt16(dtDtxRow["appt_flag"]) != Convert.ToInt16(apptFlag))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["ApptType_EHR_ID"].ToString().Trim() != row[0]["ApptType_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["ApptType_Name"].ToString().Trim() != row[0]["ApptType"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["patId"].ToString().Trim() != row[0]["patient_ehr_id"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (AppointmentStatus.ToString().Trim() != row[0]["Appointment_Status"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Utility.ConvertContactNumber(dtDtxRow["patHomephone"].ToString().Trim()) != Utility.ConvertContactNumber(row[0]["Home_Contact"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (dtDtxRow["patEmail"].ToString().Trim() != row[0]["Email"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Address.Trim() != row[0]["Address"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (City.Trim() != row[0]["City"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (State.Trim() != row[0]["ST"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else if (Zipcode.Trim() != row[0]["Zip"].ToString().Trim())
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
                                    else if (Utility.ConvertContactNumber(Mobile_Contact.Trim()) != Utility.ConvertContactNumber(row[0]["Mobile_Contact"].ToString().Trim()))
                                    {
                                        dtDtxRow["InsUptDlt"] = 4;
                                    }
                                    else
                                    {
                                        dtDtxRow["InsUptDlt"] = 0;
                                    }
                                }
                            }
                            else
                            {
                                DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Mobile_Contact + "'  AND ISNULL(Appt_EHR_ID,'0') = '0' ");
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

                        dtDentrixAppointment.AcceptChanges();


                        ///////////////////////////
                        
                        DataView dvDTXAppt = new DataView(dtDentrixAppointment.Copy());
                        DataTable dtDTXAppt = dvDTXAppt.ToTable(true, "appointment_id");
                        DataTable dtLocal = dtLocalCompareForDeletedAppointment.Copy();
                        DataTable dtDTX = dtDTXAppt.Copy();

                        foreach (DataRow rw in dtLocal.Select())
                        {
                            string strPrnt = rw[0].ToString().Trim();
                            foreach (DataRow row in dtDTX.Select())
                            {
                                string strchild = row[0].ToString().Trim();
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
                                DataRow ApptDtldr = dtDentrixAppointment.NewRow();
                                ApptDtldr["appointment_id"] = dtDtlRow["appointment_id"].ToString().Trim();
                                ApptDtldr["InsUptDlt"] = 3;
                                dtDentrixAppointment.Rows.Add(ApptDtldr);
                            }
                        }

                        ///////////////////////////
                        if (dtDentrixAppointment != null && dtDentrixAppointment.Rows.Count > 0)
                        {
                            bool status = SynchDentrixBAL.Save_Appointment_Dentrix_To_Local(dtDentrixAppointment);

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                                ObjGoalBase.WriteToSyncLogFile("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_Appointment();
                            }
                        }
                        //////////////////////


                        DataTable dtLocalAppointmentAfterInsert = SynchLocalBAL.GetLocalAppointmentData("1");
                        bool Save_DeletedAppointment_status = false;

                        foreach (DataRow dtDtxRow in dtDentrixDeletedAppointment.Rows)
                        {
                            DataRow[] row = dtLocalAppointmentAfterInsert.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appointment_id"].ToString().Trim() + "' ");
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
                        dtDentrixDeletedAppointment.AcceptChanges();

                        if (dtDentrixDeletedAppointment != null && dtDentrixDeletedAppointment.Rows.Count > 0)
                        {
                            Save_DeletedAppointment_status = SynchDentrixBAL.Update_DeletedAppointment_Dentrix_To_Local(dtDentrixDeletedAppointment);
                        }

                        /////////////////

                        Is_synched_Appointment = true;
                        IsEHRAllSync = true;
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

        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataDentrix_OperatoryEvent()
        {
            InitBgWorkerDentrix_OperatoryEvent();
            InitBgTimerDentrix_OperatoryEvent();
        }

        private void InitBgTimerDentrix_OperatoryEvent()
        {
            timerSynchDentrix_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchDentrix_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchDentrix_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_OperatoryEvent_Tick);
            timerSynchDentrix_OperatoryEvent.Enabled = true;
            timerSynchDentrix_OperatoryEvent.Start();
            timerSynchDentrix_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerDentrix_OperatoryEvent()
        {
            bwSynchDentrix_OperatoryEvent = new BackgroundWorker();
            bwSynchDentrix_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchDentrix_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchDentrix_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchDentrix_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchDentrix_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchDentrix_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderDentrix_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderDentrix_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainDentrix_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableDentrix_OperatoryEvent);
            procThreadmainDentrix_OperatoryEvent.Start();
        }

        public void CallSyncOrderTableDentrix_OperatoryEvent()
        {
            if (bwSynchDentrix_OperatoryEvent.IsBusy != true)
            {
                bwSynchDentrix_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_OperatoryEvent();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_OperatoryEvent.Enabled = true;
        }

        public void SynchDataDentrix_OperatoryEvent()
        {
            if (IsDentrixOperatorySync && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtDentrixOperatoryEvent = SynchDentrixBAL.GetDentrixOperatoryEventData();
                    dtDentrixOperatoryEvent.Columns.Add("OE_LocalDB_ID", typeof(int));
                    dtDentrixOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData("1");

                    foreach (DataRow dtDtxRow in dtDentrixOperatoryEvent.Rows)
                    {

                        DataRow[] row = dtLocalOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtDtxRow["event_id"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            if (Convert.ToDateTime(dtDtxRow["modified_time_stamp"].ToString().Trim()) != Convert.ToDateTime(row[0]["Entry_DateTime"]))
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

                        DataRow[] rowBlcOpt = dtDentrixOperatoryEvent.Copy().Select("event_id = '" + dtLOERow["OE_EHR_ID"].ToString().Trim() + "' ");
                        if (rowBlcOpt.Length > 0)
                        {
                            DateTime OE_Date;
                            try
                            {
                                OE_Date = Convert.ToDateTime(rowBlcOpt[0]["event_date"].ToString());
                            }
                            catch (Exception)
                            {
                                DataRow BlcOptDtldr = dtDentrixOperatoryEvent.NewRow();
                                BlcOptDtldr["event_id"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtDentrixOperatoryEvent.Rows.Add(BlcOptDtldr);
                            }
                        }
                        else
                        {
                            DataRow BlcOptDtldr = dtDentrixOperatoryEvent.NewRow();
                            BlcOptDtldr["event_id"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtDentrixOperatoryEvent.Rows.Add(BlcOptDtldr);
                        }
                    }


                    dtDentrixOperatoryEvent.AcceptChanges();

                    if (dtDentrixOperatoryEvent != null && dtDentrixOperatoryEvent.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_OperatoryEvent_Dentrix_To_Local(dtDentrixOperatoryEvent);

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

        private void fncSynchDataDentrix_Provider()
        {
            InitBgWorkerDentrix_Provider();
            InitBgTimerDentrix_Provider();
        }

        private void InitBgTimerDentrix_Provider()
        {
            timerSynchDentrix_Provider = new System.Timers.Timer();
            this.timerSynchDentrix_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchDentrix_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_Provider_Tick);
            timerSynchDentrix_Provider.Enabled = true;
            timerSynchDentrix_Provider.Start();
        }

        private void InitBgWorkerDentrix_Provider()
        {
            bwSynchDentrix_Provider = new BackgroundWorker();
            bwSynchDentrix_Provider.WorkerReportsProgress = true;
            bwSynchDentrix_Provider.WorkerSupportsCancellation = true;
            bwSynchDentrix_Provider.DoWork += new DoWorkEventHandler(bwSynchDentrix_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchDentrix_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_Provider_RunWorkerCompleted);
        }

        private void timerSynchDentrix_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_Provider.Enabled = false;
                MethodForCallSynchOrderDentrix_Provider();
            }
        }

        public void MethodForCallSynchOrderDentrix_Provider()
        {
            System.Threading.Thread procThreadmainDentrix_Provider = new System.Threading.Thread(this.CallSyncOrderTableDentrix_Provider);
            procThreadmainDentrix_Provider.Start();
        }

        public void CallSyncOrderTableDentrix_Provider()
        {
            if (bwSynchDentrix_Provider.IsBusy != true)
            {
                bwSynchDentrix_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_Provider();
                CommonFunction.GetMasterSync();

                //if (Utility.Application_Version.ToLower() != "DTX G5".ToLower())
                //{
                //    SynchDataDentrix_ProviderOfficeHours();
                //    SynchDataDentrix_ProviderHours();
                //}
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_Provider.Enabled = true;
        }

        public void SynchDataDentrix_Provider()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtDentrixProvider = SynchDentrixBAL.GetDentrixProviderData();
                    dtDentrixProvider.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", "1");

                    foreach (DataRow dtDtxRow in dtDentrixProvider.Rows)
                    {
                        DataRow[] row = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtDtxRow["Provider_EHR_ID"] + "'");
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
                            else if (dtDtxRow["mi"].ToString().Trim() != row[0]["MI"].ToString().Trim())
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

                    dtDentrixProvider.AcceptChanges();

                    if (dtDentrixProvider != null && dtDentrixProvider.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_Provider_Dentrix_To_Local(dtDentrixProvider);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                            ObjGoalBase.WriteToSyncLogFile("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsDentrixProviderSync = true;
                            SynchDataLiveDB_Push_Provider();
                        }
                        else
                        {
                            IsDentrixProviderSync = false;
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
        }

        #endregion

        #region Sync Provider Hours & Office Hours

        private void fncSynchDataDentrix_ProviderOfficeHours()
        {
            InitBgWorkerDentrix_ProviderOfficeHours();
            InitBgTimerDentrix_ProviderOfficeHours();
        }

        private void InitBgTimerDentrix_ProviderOfficeHours()
        {
            timerSynchDentrix_ProviderOfficeHours = new System.Timers.Timer();
            this.timerSynchDentrix_ProviderOfficeHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchDentrix_ProviderOfficeHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_ProviderOfficeHours_Tick);
            timerSynchDentrix_ProviderOfficeHours.Enabled = true;
            timerSynchDentrix_ProviderOfficeHours.Start();
        }

        private void InitBgWorkerDentrix_ProviderOfficeHours()
        {
            bwSynchDentrix_ProviderOfficeHours = new BackgroundWorker();
            bwSynchDentrix_ProviderOfficeHours.WorkerReportsProgress = true;
            bwSynchDentrix_ProviderOfficeHours.WorkerSupportsCancellation = true;
            bwSynchDentrix_ProviderOfficeHours.DoWork += new DoWorkEventHandler(bwSynchDentrix_ProviderOfficeHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchDentrix_ProviderOfficeHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_ProviderOfficeHours_RunWorkerCompleted);
        }

        private void timerSynchDentrix_ProviderOfficeHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_ProviderOfficeHours.Enabled = false;
                MethodForCallSynchOrderDentrix_ProviderOfficeHours();
            }
        }

        public void MethodForCallSynchOrderDentrix_ProviderOfficeHours()
        {
            System.Threading.Thread procThreadmainDentrix_ProviderOfficeHours = new System.Threading.Thread(this.CallSyncOrderTableDentrix_ProviderOfficeHours);
            procThreadmainDentrix_ProviderOfficeHours.Start();
        }

        public void CallSyncOrderTableDentrix_ProviderOfficeHours()
        {
            if (bwSynchDentrix_ProviderOfficeHours.IsBusy != true)
            {
                bwSynchDentrix_ProviderOfficeHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_ProviderOfficeHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_ProviderOfficeHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                if (Utility.Application_Version.ToLower() != "DTX G5".ToLower())
                {
                    SyncPullLogsAndSaveinDentrix();
                    SynchDataDentrix_ProviderOfficeHours();
                    SynchDataDentrix_ProviderHours();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SyncPullLogsAndSaveinDentrix()
        {
            try
            {
                CheckCustomhoursForProviderOperatory();
                SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                SynchDataLiveDB_Pull_PatientFollowUp();
                SynchDataLocalToDentrix_Patient_SMSCallLog();
                fncPaymentSMSCallStatusUpdate();
                SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
            }
            catch (Exception)
            {

            }
        }

        private void bwSynchDentrix_ProviderOfficeHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_ProviderOfficeHours.Enabled = true;
        }

        public void SynchDataDentrix_ProviderOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtDentrixProviderOfficeHours = SynchDentrixBAL.GetDentrixProviderOfficeHours();
                    DataTable dtDentrixLocalProviderOfficeHours = SynchLocalBAL.GetLocalProviderOfficeHours("1");
                    DataTable dtDentrixProvider = SynchDentrixBAL.GetDentrixProviderData();

                    dtDentrixProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtDentrixProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtDentrixLocalProviderOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtDentrixLocalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtDentrixLocalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtDentrixLocalProviderOfficeHours = CompareDataTableRecords(ref dtDentrixProviderOfficeHours, dtDentrixLocalProviderOfficeHours, "POH_EHR_ID", "POH_LocalDB_ID", "POH_LocalDB_ID,POH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtDentrixProviderOfficeHours.AcceptChanges();
                    dtDentrixLocalProviderOfficeHours.AcceptChanges();

                    if ((dtDentrixProviderOfficeHours != null && dtDentrixProviderOfficeHours.Rows.Count > 0) || (dtDentrixLocalProviderOfficeHours != null && dtDentrixLocalProviderOfficeHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtDentrixProviderOfficeHours.Clone();
                        if (dtDentrixProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtDentrixLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtDentrixProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtDentrixProviderOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtDentrixLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtDentrixLocalProviderOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderOfficeHours", "POH_LocalDB_ID,POH_Web_ID", "POH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtDentrixProviderOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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

        public void SynchDataDentrix_ProviderHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtDentrixProviderHours = SynchDentrixBAL.GetDentrixProviderHoursData();
                    dtDentrixProviderHours.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData("1");

                    foreach (DataRow dtDtxRow in dtDentrixProviderHours.Rows)
                    {
                        DataRow[] row = dtLocalProviderHours.Copy().Select("PH_EHR_ID = '" + dtDtxRow["PH_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (Convert.ToDateTime(dtDtxRow["Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["Entry_DateTime"].ToString().Trim()))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                                dtDtxRow["Last_Sync_Date"] = DateTime.Now.ToString();
                            }
                        }
                        else
                        {
                            dtDtxRow["InsUptDlt"] = 1;
                            dtDtxRow["Last_Sync_Date"] = DateTime.Now.ToString();
                        }
                    }

                    dtDentrixProviderHours.AcceptChanges();


                    foreach (DataRow dtLPHRow in dtLocalProviderHours.Rows)
                    {
                        DataRow[] rowBlcOpt = dtDentrixProviderHours.Copy().Select("PH_EHR_ID = '" + dtLPHRow["PH_EHR_ID"].ToString().Trim() + "'");
                        if (rowBlcOpt.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtDentrixProviderHours.NewRow();
                            BlcOptDtldr["PH_EHR_ID"] = dtLPHRow["PH_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtDentrixProviderHours.Rows.Add(BlcOptDtldr);
                        }
                    }

                    if (dtDentrixProviderHours != null && dtDentrixProviderHours.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_ProviderHours_Dentrix_To_Local(dtDentrixProviderHours);

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
                ObjGoalBase.WriteToErrorLogFile("[ProviderHours Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Speciality

        private void fncSynchDataDentrix_Speciality()
        {
            InitBgWorkerDentrix_Speciality();
            InitBgTimerDentrix_Speciality();
        }

        private void InitBgTimerDentrix_Speciality()
        {
            timerSynchDentrix_Speciality = new System.Timers.Timer();
            this.timerSynchDentrix_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchDentrix_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_Speciality_Tick);
            timerSynchDentrix_Speciality.Enabled = true;
            timerSynchDentrix_Speciality.Start();
        }

        private void InitBgWorkerDentrix_Speciality()
        {
            bwSynchDentrix_Speciality = new BackgroundWorker();
            bwSynchDentrix_Speciality.WorkerReportsProgress = true;
            bwSynchDentrix_Speciality.WorkerSupportsCancellation = true;
            bwSynchDentrix_Speciality.DoWork += new DoWorkEventHandler(bwSynchDentrix_Speciality_DoWork);
            bwSynchDentrix_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_Speciality_RunWorkerCompleted);
        }

        private void timerSynchDentrix_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_Speciality.Enabled = false;
                MethodForCallSynchOrderDentrix_Speciality();
            }
        }

        public void MethodForCallSynchOrderDentrix_Speciality()
        {
            System.Threading.Thread procThreadmainDentrix_Speciality = new System.Threading.Thread(this.CallSyncOrderTableDentrix_Speciality);
            procThreadmainDentrix_Speciality.Start();
        }

        public void CallSyncOrderTableDentrix_Speciality()
        {
            if (bwSynchDentrix_Speciality.IsBusy != true)
            {
                bwSynchDentrix_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_Speciality();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_Speciality.Enabled = true;
        }

        public void SynchDataDentrix_Speciality()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {

                try
                {

                    DataTable dtDentrixSpeciality = SynchDentrixBAL.GetDentrixProviderData();
                    DataView view = new DataView(dtDentrixSpeciality);
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
                        bool status = SynchDentrixBAL.Save_Speciality_Dentrix_To_Local(dtSpecialitydistinctValues);
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

        #region FolderList

        public void SynchDataDentrix_FolderList()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtDentrixFolderList = SynchDentrixBAL.GetDentrixFolderListData();
                        dtDentrixFolderList.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalFolderList = SynchLocalBAL.GetLocalFolderListData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                        foreach (DataRow dtDtxRow in dtDentrixFolderList.Rows)
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

                        //foreach (DataRow dtLocalRow in dtLocalOperatory.Rows)
                        //{
                        //    DataRow[] row = dtDentrixOperatory.Copy().Select("op_id = '" + dtLocalRow["Operatory_EHR_ID"] + "'");
                        //    if (row.Length <= 0)
                        //    {
                        //        dtDentrixOperatory.Rows.Add(dtLocalRow["Operatory_EHR_ID"], dtLocalRow["Operatory_Name"], "D");
                        //    }
                        //}
                        foreach (DataRow dtRow in dtLocalFolderList.Rows)
                        {
                            DataRow rowBlcOpt = dtDentrixFolderList.Copy().Select("FolderList_EHR_ID = '" + dtRow["FolderList_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                            if (rowBlcOpt == null)
                            {
                                DataRow ESApptDtldr = dtDentrixFolderList.NewRow();
                                ESApptDtldr["FolderList_EHR_ID"] = dtRow["FolderList_EHR_ID"].ToString().Trim();
                                ESApptDtldr["InsUptDlt"] = 3;
                                dtDentrixFolderList.Rows.Add(ESApptDtldr);
                            }
                        }

                        dtDentrixFolderList.AcceptChanges();

                        if (dtDentrixFolderList != null && dtDentrixFolderList.Rows.Count > 0)
                        {
                            bool status = SynchDentrixBAL.Save_FolderList_Dentrix_To_Local(dtDentrixFolderList, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString(), Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString());
                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("FolderList");
                                ObjGoalBase.WriteToSyncLogFile("FolderList Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                IsDentrixOperatorySync = true;

                                //SynchDataLiveDB_Push_Operatory();
                            }
                            else
                            {
                                IsDentrixOperatorySync = false;
                            }
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

        #region Synch Operatory

        private void fncSynchDataDentrix_Operatory()
        {
            InitBgWorkerDentrix_Operatory();
            InitBgTimerDentrix_Operatory();
        }

        private void InitBgTimerDentrix_Operatory()
        {
            timerSynchDentrix_Operatory = new System.Timers.Timer();
            this.timerSynchDentrix_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchDentrix_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_Operatory_Tick);
            timerSynchDentrix_Operatory.Enabled = true;
            timerSynchDentrix_Operatory.Start();
        }

        private void InitBgWorkerDentrix_Operatory()
        {
            bwSynchDentrix_Operatory = new BackgroundWorker();
            bwSynchDentrix_Operatory.WorkerReportsProgress = true;
            bwSynchDentrix_Operatory.WorkerSupportsCancellation = true;
            bwSynchDentrix_Operatory.DoWork += new DoWorkEventHandler(bwSynchDentrix_Operatory_DoWork);
            bwSynchDentrix_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_Operatory_RunWorkerCompleted);
        }

        private void timerSynchDentrix_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_Operatory.Enabled = false;
                MethodForCallSynchOrderDentrix_Operatory();
            }
        }

        public void MethodForCallSynchOrderDentrix_Operatory()
        {
            System.Threading.Thread procThreadmainDentrix_Operatory = new System.Threading.Thread(this.CallSyncOrderTableDentrix_Operatory);
            procThreadmainDentrix_Operatory.Start();
        }

        public void CallSyncOrderTableDentrix_Operatory()
        {
            if (bwSynchDentrix_Operatory.IsBusy != true)
            {
                bwSynchDentrix_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_Operatory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_Operatory.Enabled = true;
        }

        public void SynchDataDentrix_Operatory()
        {

            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    SynchDataDentrix_FolderList();
                    DataTable dtDentrixOperatory = SynchDentrixBAL.GetDentrixOperatoryData();
                    dtDentrixOperatory.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData("1");
                    int i = 0;
                    foreach (DataRow dtDtxRow in dtDentrixOperatory.Rows)
                    {
                        i = i + 1;
                        dtDtxRow["OperatoryOrder"] = i;
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
                    //    DataRow[] row = dtDentrixOperatory.Copy().Select("op_id = '" + dtLocalRow["Operatory_EHR_ID"] + "'");
                    //    if (row.Length <= 0)
                    //    {
                    //        dtDentrixOperatory.Rows.Add(dtLocalRow["Operatory_EHR_ID"], dtLocalRow["Operatory_Name"], "D");
                    //    }
                    //}
                    foreach (DataRow dtRow in dtLocalOperatory.Rows)
                    {
                        DataRow rowBlcOpt = dtDentrixOperatory.Copy().Select("Operatory_EHR_ID = '" + dtRow["Operatory_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                        if (rowBlcOpt == null)
                        {
                            DataRow ESApptDtldr = dtDentrixOperatory.NewRow();
                            ESApptDtldr["Operatory_EHR_ID"] = dtRow["Operatory_EHR_ID"].ToString().Trim();
                            ESApptDtldr["InsUptDlt"] = 3;
                            dtDentrixOperatory.Rows.Add(ESApptDtldr);
                        }
                    }

                    dtDentrixOperatory.AcceptChanges();

                    if (dtDentrixOperatory != null && dtDentrixOperatory.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_Operatory_Dentrix_To_Local(dtDentrixOperatory);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                            ObjGoalBase.WriteToSyncLogFile("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsDentrixOperatorySync = true;

                            SynchDataLiveDB_Push_Operatory();
                        }
                        else
                        {
                            IsDentrixOperatorySync = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }



        #endregion

        #region Synch OperatoryHours

        private void fncSynchDataDentrix_OperatoryHours()
        {
            InitBgWorkerDentrix_OperatoryHours();
            InitBgTimerDentrix_OperatoryHours();
        }

        private void InitBgTimerDentrix_OperatoryHours()
        {
            timerSynchDentrix_OperatoryHours = new System.Timers.Timer();
            this.timerSynchDentrix_OperatoryHours.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchDentrix_OperatoryHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_OperatoryHours_Tick);
            timerSynchDentrix_OperatoryHours.Enabled = true;
            timerSynchDentrix_OperatoryHours.Start();
        }

        private void InitBgWorkerDentrix_OperatoryHours()
        {
            bwSynchDentrix_OperatoryHours = new BackgroundWorker();
            bwSynchDentrix_OperatoryHours.WorkerReportsProgress = true;
            bwSynchDentrix_OperatoryHours.WorkerSupportsCancellation = true;
            bwSynchDentrix_OperatoryHours.DoWork += new DoWorkEventHandler(bwSynchDentrix_OperatoryHours_DoWork);
            bwSynchDentrix_OperatoryHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_OperatoryHours_RunWorkerCompleted);
        }

        private void timerSynchDentrix_OperatoryHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_OperatoryHours.Enabled = false;
                MethodForCallSynchOrderDentrix_OperatoryHours();
            }
        }

        public void MethodForCallSynchOrderDentrix_OperatoryHours()
        {
            System.Threading.Thread procThreadmainDentrix_OperatoryHours = new System.Threading.Thread(this.CallSyncOrderTableDentrix_OperatoryHours);
            procThreadmainDentrix_OperatoryHours.Start();
        }

        public void CallSyncOrderTableDentrix_OperatoryHours()
        {
            if (bwSynchDentrix_OperatoryHours.IsBusy != true)
            {
                bwSynchDentrix_OperatoryHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_OperatoryHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_OperatoryHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }

                if (Utility.Application_Version.ToLower() != "DTX G5".ToLower())
                {
                    SynchDataDentrix_OperatoryOfficeHours();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_OperatoryHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_OperatoryHours.Enabled = true;
        }

        public void SynchDataDentrix_OperatoryOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtDentrixOperatoryOfficeHours = SynchDentrixBAL.GetDentrixOperatoryOfficeHours();
                    DataTable dtDentrixLocalOperatoryOfficeHours = SynchLocalBAL.GetLocalOperatoryOfficeHoursData("1");
                    //DataTable dtDentrixOperatory = SynchDentrixBAL.GetDentrixOperatoryData();

                    dtDentrixOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtDentrixOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtDentrixLocalOperatoryOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtDentrixLocalOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtDentrixLocalOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtDentrixLocalOperatoryOfficeHours = CompareDataTableRecords(ref dtDentrixOperatoryOfficeHours, dtDentrixLocalOperatoryOfficeHours, "OOH_EHR_ID", "OOH_LocalDB_ID", "OOH_LocalDB_ID,OOH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtDentrixOperatoryOfficeHours.AcceptChanges();

                    if (dtDentrixOperatoryOfficeHours != null && dtDentrixOperatoryOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtDentrixOperatoryOfficeHours.Clone();
                        if (dtDentrixOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtDentrixLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtDentrixOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtDentrixOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtDentrixLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtDentrixLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryOfficeHours", "OOH_LocalDB_ID,OOH_Web_ID", "OOH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtDentrixOperatoryOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
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

                    SynchDataDentrix_OperatoryHours();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[OperatoryOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        public void SynchDataDentrix_OperatoryHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtDentrixOperatoryHours = SynchDentrixBAL.GetDentrixOperatoryHoursData();
                    dtDentrixOperatoryHours.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalOperatoryHours = SynchLocalBAL.GetLocalOperatoryHoursData("1");

                    foreach (DataRow dtDtxRow in dtDentrixOperatoryHours.Rows)
                    {
                        DataRow[] row = dtLocalOperatoryHours.Copy().Select("OH_EHR_ID = '" + dtDtxRow["OH_EHR_ID"] + "'");
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


                    foreach (DataRow dtLPHRow in dtLocalOperatoryHours.Rows)
                    {
                        DataRow[] rowBlcOpt = dtDentrixOperatoryHours.Copy().Select("OH_EHR_ID = '" + dtLPHRow["OH_EHR_ID"].ToString().Trim() + "'");
                        if (rowBlcOpt.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtDentrixOperatoryHours.NewRow();
                            BlcOptDtldr["OH_EHR_ID"] = dtLPHRow["OH_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtDentrixOperatoryHours.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtDentrixOperatoryHours.AcceptChanges();
                    if (dtDentrixOperatoryHours != null && dtDentrixOperatoryHours.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_OperatoryHours_Dentrix_To_Local(dtDentrixOperatoryHours);

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
                Is_synched_Operatory = false;
                ObjGoalBase.WriteToErrorLogFile("[OperatoryHours Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Appointment Type

        private void fncSynchDataDentrix_ApptType()
        {
            InitBgWorkerDentrix_ApptType();
            InitBgTimerDentrix_ApptType();
        }

        private void InitBgTimerDentrix_ApptType()
        {
            timerSynchDentrix_ApptType = new System.Timers.Timer();
            this.timerSynchDentrix_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchDentrix_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_ApptType_Tick);
            timerSynchDentrix_ApptType.Enabled = true;
            timerSynchDentrix_ApptType.Start();
        }

        private void InitBgWorkerDentrix_ApptType()
        {
            bwSynchDentrix_ApptType = new BackgroundWorker();
            bwSynchDentrix_ApptType.WorkerReportsProgress = true;
            bwSynchDentrix_ApptType.WorkerSupportsCancellation = true;
            bwSynchDentrix_ApptType.DoWork += new DoWorkEventHandler(bwSynchDentrix_ApptType_DoWork);
            bwSynchDentrix_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_ApptType_RunWorkerCompleted);
        }

        private void timerSynchDentrix_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_ApptType.Enabled = false;
                MethodForCallSynchOrderDentrix_ApptType();
            }
        }

        public void MethodForCallSynchOrderDentrix_ApptType()
        {
            System.Threading.Thread procThreadmainDentrix_ApptType = new System.Threading.Thread(this.CallSyncOrderTableDentrix_ApptType);
            procThreadmainDentrix_ApptType.Start();
        }

        public void CallSyncOrderTableDentrix_ApptType()
        {
            if (bwSynchDentrix_ApptType.IsBusy != true)
            {
                bwSynchDentrix_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_ApptType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_ApptType.Enabled = true;
        }

        public void SynchDataDentrix_ApptType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtDentrixApptType = SynchDentrixBAL.GetDentrixApptTypeData();
                    dtDentrixApptType.Columns.Add("InsUptDlt", typeof(int));
                    dtDentrixApptType.Rows.Add(0, "<none>");
                    DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData("1");

                    foreach (DataRow dtDtxRow in dtDentrixApptType.Rows)
                    {
                        DataRow[] row = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
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
                        DataRow[] row = dtDentrixApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtDentrixApptType.NewRow();
                            BlcOptDtldr["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["Type_Name"] = dtDtxRow["Type_Name"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtDentrixApptType.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtDentrixApptType.AcceptChanges();

                    if (dtDentrixApptType != null && dtDentrixApptType.Rows.Count > 0)
                    {
                        bool Type = SynchDentrixBAL.Save_ApptType_Dentrix_To_Local(dtDentrixApptType);

                        if (Type)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                            ObjGoalBase.WriteToSyncLogFile("Appointment Type Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsDentrixApptTypeSync = true;

                            SynchDataLiveDB_Push_ApptType();
                        }
                        else
                        {
                            IsDentrixApptTypeSync = false;
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

        private void fncSynchDataDentrix_Patient()
        {
            InitBgWorkerDentrix_Patient();
            InitBgTimerDentrix_Patient();
        }

        private void InitBgTimerDentrix_Patient()
        {
            timerSynchDentrix_Patient = new System.Timers.Timer();
            this.timerSynchDentrix_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchDentrix_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_Patient_Tick);
            timerSynchDentrix_Patient.Enabled = true;
            timerSynchDentrix_Patient.Start();
            timerSynchDentrix_Patient_Tick(null, null);
        }

        private void InitBgWorkerDentrix_Patient()
        {
            bwSynchDentrix_Patient = new BackgroundWorker();
            bwSynchDentrix_Patient.WorkerReportsProgress = true;
            bwSynchDentrix_Patient.WorkerSupportsCancellation = true;
            bwSynchDentrix_Patient.DoWork += new DoWorkEventHandler(bwSynchDentrix_Patient_DoWork);
            bwSynchDentrix_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_Patient_RunWorkerCompleted);
        }

        private void timerSynchDentrix_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_Patient.Enabled = false;
                MethodForCallSynchOrderDentrix_Patient();
            }
        }

        public void MethodForCallSynchOrderDentrix_Patient()
        {
            System.Threading.Thread procThreadmainDentrix_Patient = new System.Threading.Thread(this.CallSyncOrderTableDentrix_Patient);
            procThreadmainDentrix_Patient.Start();
        }

        public void CallSyncOrderTableDentrix_Patient()
        {
            if (bwSynchDentrix_Patient.IsBusy != true)
            {
                bwSynchDentrix_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                try
                {
                    SynchDataDentrix_Patient_New();
                }
                catch (Exception ex)
                {
                    SynchDataDentrix_Patient();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_Patient.Enabled = true;
        }

        //public void SynchDataDentrix_Patient()
        //{
        //    if (Utility.IsApplicationIdleTimeOff && IsParientFirstSync)
        //    {
        //        try
        //        {
        //            IsParientFirstSync = false;
        //            DataTable dtDentrixPatientNextApptDate = SynchDentrixBAL.GetDentrixPatientNextApptDate();
        //            DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixPatientData();
        //            DataTable dtDentrixPatientdue_date = SynchDentrixBAL.GetDentrixPatientdue_date();
        //            DataTable dtDentrixPatientcollect_payment = SynchDentrixBAL.GetDentrixPatientcollect_payment();

        //            dtDentrixPatient.Columns.Add("InsUptDlt", typeof(int));
        //            dtDentrixPatient.Columns.Add("Patientcollect_payment", typeof(string));

        //            DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData();

        //            string MaritalStatus = string.Empty;
        //            string Status = string.Empty;
        //            string tmpBirthDate = string.Empty;
        //            string tmpFirstVisit_Date = string.Empty;
        //            string tmpLastVisit_Date = string.Empty;
        //            string tmpnextvisit_date = string.Empty;
        //            string tmpdue_date = string.Empty;
        //            string tmpReceive_Sms_Email = string.Empty;
        //            decimal curPatientcollect_payment = 0;
        //            int tmpprivacy_flags = 0;

        //            TotalPatientRecord = dtDentrixPatient.Rows.Count;
        //            GetPatientRecord = 0;

        //            foreach (DataRow dtDtxRow in dtDentrixPatient.Rows)
        //            {
        //                GetPatientRecord = GetPatientRecord + 1;

        //                tmpBirthDate = Utility.CheckValidDatetime(dtDtxRow["birth_date"].ToString().Trim());
        //                tmpFirstVisit_Date = Utility.CheckValidDatetime(dtDtxRow["FirstVisit_Date"].ToString().Trim());
        //                tmpLastVisit_Date = Utility.CheckValidDatetime(dtDtxRow["LastVisit_Date"].ToString().Trim());

        //                tmpReceive_Sms_Email = "Y";

        //                tmpprivacy_flags = Convert.ToInt32(dtDtxRow["privacy_flags"].ToString());

        //                if (tmpprivacy_flags == 2 || tmpprivacy_flags == 3 || tmpprivacy_flags == 6 || tmpprivacy_flags == 7)
        //                {
        //                    tmpReceive_Sms_Email = "N";
        //                }

        //                // https://app.asana.com/0/751059797849097/1149506260330945
        //                dtDtxRow["nextvisit_date"] = Utility.SetNextVisitDate(dtDentrixPatientNextApptDate, "patid", "Patient_EHR_ID", "nextvisit_date", dtDtxRow["Patient_EHR_ID"].ToString());
        //                tmpnextvisit_date = dtDtxRow["nextvisit_date"].ToString();

        //                try
        //                {
        //                    DataRow[] drPatientcollect_payment = dtDentrixPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");

        //                    if (drPatientcollect_payment.Length > 0)
        //                    {
        //                        curPatientcollect_payment = Convert.ToDecimal(drPatientcollect_payment[0]["collect_payment"].ToString());
        //                        curPatientcollect_payment = decimal.Round(curPatientcollect_payment, 2, MidpointRounding.AwayFromZero);
        //                        curPatientcollect_payment = System.Math.Abs(curPatientcollect_payment);
        //                        dtDtxRow["Patientcollect_payment"] = curPatientcollect_payment.ToString();
        //                    }
        //                    else
        //                    {
        //                        curPatientcollect_payment = 0;
        //                        dtDtxRow["Patientcollect_payment"] = curPatientcollect_payment.ToString();
        //                    }
        //                }
        //                catch (Exception)
        //                {
        //                    curPatientcollect_payment = 0;
        //                    dtDtxRow["Patientcollect_payment"] = curPatientcollect_payment.ToString();
        //                }
        //                try
        //                {

        //                    DataRow[] Patdue_date = dtDentrixPatientdue_date.Copy().Select("patient_id = '" + dtDtxRow["Patient_EHR_ID"] + "'");
        //                    tmpdue_date = string.Empty;

        //                    if (Patdue_date.Length > 0)
        //                    {

        //                        //foreach (DataRow due_row in Patdue_date)
        //                        //{
        //                        //    tmpdue_date = due_row["due_date"].ToString() + "@" + due_row["recall_type"].ToString() + "@" + due_row["recallid"].ToString() + "|" + tmpdue_date;
        //                        //}
        //                        //if (tmpdue_date.Length > 1000)
        //                        //{
        //                        //    tmpdue_date = tmpdue_date.Substring(0, 999);
        //                        //}
        //                        //dtDtxRow["due_date"] = tmpdue_date;                      

        //                        if (Patdue_date.Length > 5)
        //                        {
        //                            DataTable tmpDatatableduedate = new DataTable();
        //                            tmpDatatableduedate = Patdue_date.CopyToDataTable();
        //                            DataView view = tmpDatatableduedate.DefaultView;
        //                            view.Sort = "due_date desc";
        //                            DataTable SortPatdue_date = view.ToTable();
        //                            for (int i = 0; i < 5; i++)
        //                            {
        //                                tmpdue_date = SortPatdue_date.Rows[i]["due_date"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type_id"].ToString() + "|" + tmpdue_date;
        //                            }
        //                            dtDtxRow["due_date"] = tmpdue_date;
        //                        }
        //                        else
        //                        {
        //                            foreach (DataRow due_row in Patdue_date)
        //                            {
        //                                tmpdue_date = due_row["due_date"].ToString() + "@" + due_row["recall_type"].ToString() + "@" + due_row["recall_type_id"].ToString() + "|" + tmpdue_date;
        //                            }
        //                            dtDtxRow["due_date"] = tmpdue_date;
        //                        }
        //                    }
        //                }
        //                catch (Exception)
        //                {
        //                    tmpdue_date = string.Empty;
        //                }

        //                try
        //                {
        //                    Status = dtDtxRow["Status"].ToString().Trim();
        //                }
        //                catch (Exception)
        //                { Status = ""; }

        //                if (Status == "3")
        //                { Status = "I"; }
        //                else
        //                { Status = "A"; }


        //                if (Convert.ToInt32(dtDtxRow["sex"]) == 1)
        //                {
        //                    dtDtxRow["sex"] = "Male";
        //                }
        //                else if (Convert.ToInt32(dtDtxRow["sex"]) == 2)
        //                {
        //                    dtDtxRow["sex"] = "Female";
        //                }
        //                else
        //                {
        //                    dtDtxRow["sex"] = "Unknown";
        //                }

        //                if (Convert.ToInt32(dtDtxRow["MaritalStatus"]) == 1)
        //                {
        //                    dtDtxRow["MaritalStatus"] = "Married";
        //                }
        //                else if (Convert.ToInt32(dtDtxRow["MaritalStatus"]) == 2)
        //                {
        //                    dtDtxRow["MaritalStatus"] = "Single";
        //                }
        //                else if (Convert.ToInt32(dtDtxRow["MaritalStatus"]) == 3)
        //                {
        //                    dtDtxRow["MaritalStatus"] = "Child";
        //                }
        //                else if (Convert.ToInt32(dtDtxRow["MaritalStatus"]) == 4)
        //                {
        //                    dtDtxRow["MaritalStatus"] = "Other";
        //                }
        //                else
        //                {
        //                    dtDtxRow["MaritalStatus"] = "Single";
        //                }
        //                DataRow[] row = dtLocalPatient.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
        //                if (row.Length > 0)
        //                {
        //                    if (dtDtxRow["First_name"].ToString().Trim() != row[0]["First_name"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Last_name"].ToString().Trim() != row[0]["Last_name"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Middle_Name"].ToString().Trim() != row[0]["Middle_Name"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Salutation"].ToString().Trim() != row[0]["Salutation"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["preferred_name"].ToString().Trim() != row[0]["preferred_name"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (Status.Trim() != row[0]["Status"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Email"].ToString().Trim() != row[0]["Email"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["sex"].ToString().Trim() != row[0]["sex"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["MaritalStatus"].ToString().Trim() != row[0]["MaritalStatus"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Mobile"].ToString().Trim() != row[0]["Mobile"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Home_Phone"].ToString().Trim() != row[0]["Home_Phone"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Work_Phone"].ToString().Trim() != row[0]["Work_Phone"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Address1"].ToString().Trim() != row[0]["Address1"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Address2"].ToString().Trim() != row[0]["Address2"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["City"].ToString().Trim() != row[0]["City"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["State"].ToString().Trim() != row[0]["State"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Zipcode"].ToString().Trim() != row[0]["Zipcode"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["due_date"].ToString().Trim() != row[0]["due_date"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (Convert.ToDecimal(dtDtxRow["CurrentBal"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["CurrentBal"].ToString().Trim()).ToString("0.##"))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (Convert.ToDecimal(dtDtxRow["ThirtyDay"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["ThirtyDay"].ToString().Trim()).ToString("0.##"))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (Convert.ToDecimal(dtDtxRow["SixtyDay"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["SixtyDay"].ToString().Trim()).ToString("0.##"))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (Convert.ToDecimal(dtDtxRow["NinetyDay"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["NinetyDay"].ToString().Trim()).ToString("0.##"))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (Convert.ToDecimal(dtDtxRow["Over90"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["Over90"].ToString().Trim()).ToString("0.##"))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (Convert.ToDecimal(dtDtxRow["remaining_benefit"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["remaining_benefit"].ToString().Trim()).ToString("0.##"))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (Convert.ToDecimal(dtDtxRow["used_benefit"].ToString().Trim()).ToString("0.##") != Convert.ToDecimal(row[0]["used_benefit"].ToString().Trim()).ToString("0.##"))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }

        //                    else if (Convert.ToDecimal(curPatientcollect_payment).ToString("0.##") != Convert.ToDecimal(row[0]["collect_payment"].ToString().Trim()).ToString("0.##"))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Primary_Insurance"].ToString().Trim() != row[0]["Primary_Insurance"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Primary_Insurance_CompanyName"].ToString().Trim() != row[0]["Primary_Insurance_CompanyName"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Secondary_Insurance"].ToString().Trim() != row[0]["Secondary_Insurance"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Secondary_Insurance_CompanyName"].ToString().Trim() != row[0]["Secondary_Insurance_CompanyName"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Guar_ID"].ToString().Trim() != row[0]["Guar_ID"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Pri_Provider_ID"].ToString().Trim() != row[0]["Pri_Provider_ID"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (dtDtxRow["Sec_Provider_ID"].ToString().Trim() != row[0]["Sec_Provider_ID"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (row[0]["ReceiveSms"].ToString().Trim() != tmpReceive_Sms_Email)
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else if (row[0]["ReceiveEmail"].ToString().Trim() != tmpReceive_Sms_Email)
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    else
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 0;
        //                    }

        //                    if (Utility.DateDiffBetweenTwoDate(tmpBirthDate, row[0]["Birth_Date"].ToString().Trim()))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    if (Utility.DateDiffBetweenTwoDate(tmpFirstVisit_Date, row[0]["FirstVisit_Date"].ToString().Trim()))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    if (Utility.DateDiffBetweenTwoDate(tmpLastVisit_Date, row[0]["LastVisit_Date"].ToString().Trim()))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    if (Utility.DateDiffBetweenTwoDate(tmpnextvisit_date, row[0]["nextvisit_date"].ToString().Trim()))
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                    if (tmpdue_date != row[0]["due_date"].ToString().Trim())
        //                    {
        //                        dtDtxRow["InsUptDlt"] = 2;
        //                    }
        //                }
        //                else
        //                {
        //                    dtDtxRow["InsUptDlt"] = 1;
        //                }
        //            }

        //            dtDentrixPatient.AcceptChanges();

        //            if (dtDentrixPatient != null && dtDentrixPatient.Rows.Count > 0)
        //            {
        //                bool isPatientSave = SynchDentrixBAL.Save_Patient_Dentrix_To_Local(dtDentrixPatient);
        //                if (isPatientSave)
        //                {
        //                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
        //                    ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                    IsGetParientRecordDone = true;

        //                    SynchDataLiveDB_Push_Patient();
        //                }
        //            }
        //            else
        //            {
        //                ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
        //                bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
        //                IsGetParientRecordDone = true;

        //            }
        //            IsPatientSyncedFirstTime = true;
        //            IsParientFirstSync = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            IsParientFirstSync = true;
        //            ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //        }
        //    }
        //}

        public void SynchDataDentrix_Patient_New()
        {
            try
            {
                if (Utility.IsExternalAppointmentSync)
                {
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;
                    Is_synched_AppointmentsPatient = false;
                }
                if (Utility.IsApplicationIdleTimeOff && IsParientFirstSync && !Is_synched_Patient && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
                {
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;
                    DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixPatientData();
                    if (!dtDentrixPatient.Columns.Contains("ResponsibleParty_Status"))
                    {
                        dtDentrixPatient.Columns.Add("ResponsibleParty_Status", typeof(string));
                        dtDentrixPatient.Columns["ResponsibleParty_Status"].DefaultValue = "";
                    }

                    DataTable dtDentrixPatientdue_date = SynchDentrixBAL.GetDentrixPatientdue_date();

                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");

                    //var query = from dr in dtDentrixPatient.AsEnumerable()
                    //            join due_row in dtDentrixPatientdue_date.AsEnumerable()
                    //            on dr["Patient_EHR_ID"].ToString().Trim() equals due_row["patient_id"].ToString().Trim() into gj
                    //            select new
                    //            {
                    //                PatientDataRow = dr,
                    //                DueDateRows = gj.OrderByDescending(row => row.Field<DateTime>("due_date")).Take(5)
                    //            };
                    //foreach (var result in query)
                    //{
                    //    string tmpdue_date = string.Join("|", result.DueDateRows.Select(row =>
                    //        $"{row["due_date"]}@{row["recall_type"]}@{row["recall_type_id"]}"));
                    //    if (!string.IsNullOrEmpty(tmpdue_date))
                    //    {
                    //        tmpdue_date = tmpdue_date + "|";
                    //    }
                    //    result.PatientDataRow["due_date"] = tmpdue_date;
                    //}

                    foreach (DataRow dr in dtDentrixPatient.Rows)
                    {
                        string tmpdue_date = "";
                        try
                        {
                            DataRow[] Patdue_date = dtDentrixPatientdue_date.Select("patient_id = '" + dr["Patient_EHR_ID"] + "'");
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
                                        tmpdue_date = SortPatdue_date.Rows[i]["due_date"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type_id"].ToString() + "|" + tmpdue_date;
                                    }
                                    dr["due_date"] = tmpdue_date;
                                }
                                else
                                {
                                    DataTable tmpDatatableduedate = new DataTable();
                                    tmpDatatableduedate = Patdue_date.CopyToDataTable();
                                    DataView view = tmpDatatableduedate.DefaultView;
                                    view.Sort = "due_date desc";
                                    DataTable SortPatdue_date = view.ToTable();
                                    foreach (DataRow due_row in SortPatdue_date.Rows)
                                    {
                                        tmpdue_date = due_row["due_date"].ToString() + "@" + due_row["recall_type"].ToString() + "@" + due_row["recall_type_id"].ToString() + "|" + tmpdue_date;
                                    }
                                    dr["due_date"] = tmpdue_date;
                                }
                            }
                        }
                        catch (Exception x)
                        {
                            Utility.WriteToErrorLogFromAll("Error in generating due/recall date: " + x.Message);
                            tmpdue_date = string.Empty;
                        }
                    }

                    DataTable dtSaveRecords = new DataTable();
                    dtSaveRecords = dtLocalPatient.Clone();

                    #region "Add"
                    var itemsToBeAdded = (from DentrixPatient in dtDentrixPatient.AsEnumerable()
                                          join LocalPatient in dtLocalPatient.AsEnumerable()
                                          on DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                                          equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                          into matchingRows
                                          from matchingRow in matchingRows.DefaultIfEmpty()
                                          where matchingRow == null
                                          select DentrixPatient).ToList();
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
                    #endregion

                    #region TestUpdate
                    //var itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                         join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                         on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                         equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                         //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                         //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                         where
                    //                         (LocalPatient["First_name"] != DBNull.Value ? LocalPatient["First_name"].ToString().Trim() : "") != (DentrixPatient["First_name"] != DBNull.Value ? DentrixPatient["First_name"].ToString().Trim() : "")
                    //                         select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("First_name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Last_name"] != DBNull.Value ? LocalPatient["Last_name"].ToString().Trim() : "") != (DentrixPatient["Last_name"] != DBNull.Value ? DentrixPatient["Last_name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Last_name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Middle_Name"] != DBNull.Value ? LocalPatient["Middle_Name"].ToString().Trim() : "") != (DentrixPatient["Middle_Name"] != DBNull.Value ? DentrixPatient["Middle_Name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Middle_Name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Salutation"] != DBNull.Value ? LocalPatient["Salutation"].ToString().Trim() : "") != (DentrixPatient["Salutation"] != DBNull.Value ? DentrixPatient["Salutation"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Salutation:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Status"] != DBNull.Value ? LocalPatient["Status"].ToString().Trim() : "") != (DentrixPatient["Status"] != DBNull.Value ? DentrixPatient["Status"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Status:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Sex"] != DBNull.Value ? LocalPatient["Sex"].ToString().Trim() : "") != (DentrixPatient["Sex"] != DBNull.Value ? DentrixPatient["Sex"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Sex:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["MaritalStatus"] != DBNull.Value ? LocalPatient["MaritalStatus"].ToString().Trim() : "") != (DentrixPatient["MaritalStatus"] != DBNull.Value ? DentrixPatient["MaritalStatus"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("MaritalStatus:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Birth_Date"] != DBNull.Value && LocalPatient["Birth_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["Birth_Date"].ToString().Trim()).Date : DateTime.Now) != (DentrixPatient["Birth_Date"] != DBNull.Value && DentrixPatient["Birth_Date"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["Birth_Date"].ToString().Trim()).Date : DateTime.Now)
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Birth_Date:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Email"] != DBNull.Value ? LocalPatient["Email"].ToString().Trim() : "") != (DentrixPatient["Email"] != DBNull.Value ? DentrixPatient["Email"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Email:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Mobile"] != DBNull.Value ? LocalPatient["Mobile"].ToString().Trim() : "") != (DentrixPatient["Mobile"] != DBNull.Value ? DentrixPatient["Mobile"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Mobile:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Home_Phone"] != DBNull.Value ? LocalPatient["Home_Phone"].ToString().Trim() : "") != (DentrixPatient["Home_Phone"] != DBNull.Value ? DentrixPatient["Home_Phone"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Home_Phone:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Work_Phone"] != DBNull.Value ? LocalPatient["Work_Phone"].ToString().Trim() : "") != (DentrixPatient["Work_Phone"] != DBNull.Value ? DentrixPatient["Work_Phone"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Work_Phone:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Address1"] != DBNull.Value ? LocalPatient["Address1"].ToString().Trim() : "") != (DentrixPatient["Address1"] != DBNull.Value ? DentrixPatient["Address1"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Address1:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Address2"] != DBNull.Value ? LocalPatient["Address2"].ToString().Trim() : "") != (DentrixPatient["Address2"] != DBNull.Value ? DentrixPatient["Address2"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Address2:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Address1"] != DBNull.Value ? LocalPatient["Address1"].ToString().Trim() : "") != (DentrixPatient["Address1"] != DBNull.Value ? DentrixPatient["Address1"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Address1:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["City"] != DBNull.Value ? LocalPatient["City"].ToString().Trim() : "") != (DentrixPatient["City"] != DBNull.Value ? DentrixPatient["City"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("City:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["State"] != DBNull.Value ? LocalPatient["State"].ToString().Trim() : "") != (DentrixPatient["State"] != DBNull.Value ? DentrixPatient["State"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("State:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Zipcode"] != DBNull.Value ? LocalPatient["Zipcode"].ToString().Trim() : "") != (DentrixPatient["Zipcode"] != DBNull.Value ? DentrixPatient["Zipcode"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Zipcode:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["ResponsibleParty_Status"] != DBNull.Value ? LocalPatient["ResponsibleParty_Status"].ToString().Trim() : "") != (DentrixPatient["ResponsibleParty_Status"] != DBNull.Value ? DentrixPatient["ResponsibleParty_Status"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("ResponsibleParty_Status:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["CurrentBal"] != DBNull.Value ? LocalPatient["CurrentBal"].ToString().Trim() : "") != (DentrixPatient["CurrentBal"] != DBNull.Value ? DentrixPatient["CurrentBal"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("CurrentBal:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["ThirtyDay"] != DBNull.Value ? LocalPatient["ThirtyDay"].ToString().Trim() : "") != (DentrixPatient["ThirtyDay"] != DBNull.Value ? DentrixPatient["ThirtyDay"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("ThirtyDay:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["CurrentBal"] != DBNull.Value ? LocalPatient["CurrentBal"].ToString().Trim() : "") != (DentrixPatient["CurrentBal"] != DBNull.Value ? DentrixPatient["CurrentBal"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("CurrentBal:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["SixtyDay"] != DBNull.Value ? LocalPatient["SixtyDay"].ToString().Trim() : "") != (DentrixPatient["SixtyDay"] != DBNull.Value ? DentrixPatient["SixtyDay"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("SixtyDay:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["NinetyDay"] != DBNull.Value ? LocalPatient["NinetyDay"].ToString().Trim() : "") != (DentrixPatient["NinetyDay"] != DBNull.Value ? DentrixPatient["NinetyDay"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();

                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("NinetyDay:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["FirstVisit_Date"] != DBNull.Value && LocalPatient["FirstVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["FirstVisit_Date"].ToString().Trim()) : DateTime.Now) != (DentrixPatient["FirstVisit_Date"] != DBNull.Value && DentrixPatient["FirstVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["FirstVisit_Date"].ToString().Trim()) : DateTime.Now)
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("FirstVisit_Date:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Over90"] != DBNull.Value ? LocalPatient["Over90"].ToString().Trim() : "") != (DentrixPatient["Over90"] != DBNull.Value ? DentrixPatient["Over90"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();

                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Over90:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["LastVisit_Date"] != DBNull.Value && LocalPatient["LastVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["LastVisit_Date"].ToString().Trim()) : DateTime.Now) != (DentrixPatient["LastVisit_Date"] != DBNull.Value && DentrixPatient["LastVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["LastVisit_Date"].ToString().Trim()) : DateTime.Now)
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("LastVisit_Date:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Primary_Insurance"] != DBNull.Value ? LocalPatient["Primary_Insurance"].ToString().Trim() : "") != (DentrixPatient["Primary_Insurance"] != DBNull.Value ? DentrixPatient["Primary_Insurance"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Primary_Insurance:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Primary_Insurance_CompanyName"] != DBNull.Value ? LocalPatient["Primary_Insurance_CompanyName"].ToString().Trim() : "") != (DentrixPatient["Primary_Insurance_CompanyName"] != DBNull.Value ? DentrixPatient["Primary_Insurance_CompanyName"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Primary_Insurance_CompanyName:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Secondary_Insurance"] != DBNull.Value ? LocalPatient["Secondary_Insurance"].ToString().Trim() : "") != (DentrixPatient["Secondary_Insurance"] != DBNull.Value ? DentrixPatient["Secondary_Insurance"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Secondary_Insurance:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Secondary_Insurance_CompanyName"] != DBNull.Value ? LocalPatient["Secondary_Insurance_CompanyName"].ToString().Trim() : "") != (DentrixPatient["Secondary_Insurance_CompanyName"] != DBNull.Value ? DentrixPatient["Secondary_Insurance_CompanyName"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Secondary_Insurance_CompanyName:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Guar_ID"] != DBNull.Value ? LocalPatient["Guar_ID"].ToString().Trim() : "") != (DentrixPatient["Guar_ID"] != DBNull.Value ? DentrixPatient["Guar_ID"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Guar_ID:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Pri_Provider_ID"] != DBNull.Value ? LocalPatient["Pri_Provider_ID"].ToString().Trim() : "") != (DentrixPatient["Pri_Provider_ID"] != DBNull.Value ? DentrixPatient["Pri_Provider_ID"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Pri_Provider_ID:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Sec_Provider_ID"] != DBNull.Value ? LocalPatient["Sec_Provider_ID"].ToString().Trim() : "") != (DentrixPatient["Sec_Provider_ID"] != DBNull.Value ? DentrixPatient["Sec_Provider_ID"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Sec_Provider_ID:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["ReceiveSms"] != DBNull.Value ? LocalPatient["ReceiveSms"].ToString().Trim() : "") != (DentrixPatient["ReceiveSms"] != DBNull.Value ? DentrixPatient["ReceiveSms"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("ReceiveSms:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["ReceiveEmail"] != DBNull.Value ? LocalPatient["ReceiveEmail"].ToString().Trim() : "") != (DentrixPatient["ReceiveEmail"] != DBNull.Value ? DentrixPatient["ReceiveEmail"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("ReceiveEmail:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //Utility.WriteToDebugSyncLogFile_All("nextvisit_date Start", "PatientSyncNew");
                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["nextvisit_date"].ToString().Trim()) : DateTime.Now) != (DentrixPatient["nextvisit_date"] != DBNull.Value && DentrixPatient["nextvisit_date"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["nextvisit_date"].ToString().Trim()) : DateTime.Now)
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("nextvisit_date:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}
                    //Utility.WriteToDebugSyncLogFile_All("nextvisit_date End", "PatientSyncNew");
                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["due_date"] != DBNull.Value && LocalPatient["due_date"].ToString().Trim() != "" ? LocalPatient["due_date"].ToString().Trim() : "") != (DentrixPatient["due_date"] != DBNull.Value && DentrixPatient["due_date"].ToString().Trim() != "" ? DentrixPatient["due_date"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("due_date:" + itemsToBeUpdated2.Count, "PatientSyncNew");

                    //    var q = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //             join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //             on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //             equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //             where
                    //             (LocalPatient["due_date"] != DBNull.Value && LocalPatient["due_date"].ToString().Trim() != "" ? LocalPatient["due_date"].ToString().Trim() : "") != (DentrixPatient["due_date"] != DBNull.Value && DentrixPatient["due_date"].ToString().Trim() != "" ? DentrixPatient["due_date"].ToString().Trim() : "")
                    //             select new
                    //             {
                    //                 Patient_EHR_ID = DentrixPatient["Patient_EHR_ID"],
                    //                 Local_due_date = LocalPatient["due_date"],
                    //                 Dentrix_due_date = DentrixPatient["due_date"]
                    //             }).ToList();


                    //    foreach (var dr in q)
                    //    {
                    //        Utility.WriteToDebugSyncLogFile_All(dr.Patient_EHR_ID + "," + dr.Local_due_date + "," + dr.Dentrix_due_date, "PatientSyncNewDiff");
                    //    }
                    //}



                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["remaining_benefit"] != DBNull.Value ? LocalPatient["remaining_benefit"].ToString().Trim() : "") != (DentrixPatient["remaining_benefit"] != DBNull.Value ? DentrixPatient["remaining_benefit"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("remaining_benefit:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["preferred_name"] != DBNull.Value ? LocalPatient["preferred_name"].ToString().Trim() : "") != (DentrixPatient["preferred_name"] != DBNull.Value ? DentrixPatient["preferred_name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("preferred_name:" + itemsToBeUpdated2.Count, "PatientSyncNew");

                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["collect_payment"] != DBNull.Value ? LocalPatient["collect_payment"].ToString().Trim() : "0") != (DentrixPatient["collect_payment"] != DBNull.Value ? DentrixPatient["collect_payment"].ToString().Trim() : "0")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("collect_payment:" + itemsToBeUpdated2.Count, "PatientSyncNew");

                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["used_benefit"] != DBNull.Value ? LocalPatient["used_benefit"].ToString().Trim() : "") != (DentrixPatient["used_benefit"] != DBNull.Value ? DentrixPatient["used_benefit"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("used_benefit:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}


                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Secondary_Ins_Subscriber_ID"] != DBNull.Value ? LocalPatient["Secondary_Ins_Subscriber_ID"].ToString().Trim() : "") != (DentrixPatient["Secondary_Insurance"] != DBNull.Value ? DentrixPatient["Secondary_Insurance"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Secondary_Ins_Subscriber_ID:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Primary_Ins_Subscriber_ID"] != DBNull.Value ? LocalPatient["Primary_Ins_Subscriber_ID"].ToString().Trim() : "") != (DentrixPatient["Primary_Insurance"] != DBNull.Value ? DentrixPatient["Primary_Insurance"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Primary_Ins_Subscriber_ID:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}


                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["EHR_Status"] != DBNull.Value ? LocalPatient["EHR_Status"].ToString().Trim() : "") != (DentrixPatient["EHR_Status"] != DBNull.Value ? DentrixPatient["EHR_Status"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("EHR_Status:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["ReceiveVoiceCall"] != DBNull.Value ? LocalPatient["ReceiveVoiceCall"].ToString().Trim() : "") != (DentrixPatient["ReceiveVoiceCall"] != DBNull.Value ? DentrixPatient["ReceiveVoiceCall"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("ReceiveVoiceCall:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["PreferredLanguage"] != DBNull.Value ? LocalPatient["PreferredLanguage"].ToString().Trim() : "") != (DentrixPatient["PreferredLanguage"] != DBNull.Value ? DentrixPatient["PreferredLanguage"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("PreferredLanguage:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Patient_Note"] != DBNull.Value ? LocalPatient["Patient_Note"].ToString().Trim() : "") != (DentrixPatient["Patient_Note"] != DBNull.Value ? DentrixPatient["Patient_Note"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Patient_Note:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["SSN"] != DBNull.Value ? LocalPatient["SSN"].ToString().Trim() : "") != (DentrixPatient["SSN"] != DBNull.Value ? DentrixPatient["SSN"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("SSN:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["driverlicense"] != DBNull.Value ? LocalPatient["driverlicense"].ToString().Trim() : "") != (DentrixPatient["driverlicense"] != DBNull.Value ? DentrixPatient["driverlicense"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("driverlicense:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["groupid"] != DBNull.Value ? LocalPatient["groupid"].ToString().Trim() : "") != (DentrixPatient["groupid"] != DBNull.Value ? DentrixPatient["groupid"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("groupid:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["emergencycontactId"] != DBNull.Value ? LocalPatient["emergencycontactId"].ToString().Trim() : "") != (DentrixPatient["emergencycontactId"] != DBNull.Value ? DentrixPatient["emergencycontactId"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("emergencycontactId:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["EmergencyContact_First_Name"] != DBNull.Value ? LocalPatient["EmergencyContact_First_Name"].ToString().Trim() : "") != (DentrixPatient["EmergencyContact_First_Name"] != DBNull.Value ? DentrixPatient["EmergencyContact_First_Name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("EmergencyContact_First_Name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["EmergencyContact_Last_Name"] != DBNull.Value ? LocalPatient["EmergencyContact_Last_Name"].ToString().Trim() : "") != (DentrixPatient["EmergencyContact_Last_Name"] != DBNull.Value ? DentrixPatient["EmergencyContact_Last_Name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("EmergencyContact_Last_Name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["emergencycontactnumber"] != DBNull.Value ? LocalPatient["emergencycontactnumber"].ToString().Trim() : "") != (DentrixPatient["emergencycontactnumber"] != DBNull.Value ? DentrixPatient["emergencycontactnumber"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("emergencycontactnumber:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["school"] != DBNull.Value ? LocalPatient["school"].ToString().Trim() : "") != (DentrixPatient["school"] != DBNull.Value ? DentrixPatient["school"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("school:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["employer"] != DBNull.Value ? LocalPatient["employer"].ToString().Trim() : "") != (DentrixPatient["employer"] != DBNull.Value ? DentrixPatient["employer"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("employer:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["spouseId"] != DBNull.Value ? LocalPatient["spouseId"].ToString().Trim() : "") != (DentrixPatient["spouseId"] != DBNull.Value ? DentrixPatient["spouseId"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("spouseId:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["responsiblepartyId"] != DBNull.Value ? LocalPatient["responsiblepartyId"].ToString().Trim() : "") != (DentrixPatient["responsiblepartyId"] != DBNull.Value ? DentrixPatient["responsiblepartyId"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("responsiblepartyId:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["responsiblepartyssn"] != DBNull.Value ? LocalPatient["responsiblepartyssn"].ToString().Trim() : "") != (DentrixPatient["responsiblepartyssn"] != DBNull.Value ? DentrixPatient["responsiblepartyssn"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("responsiblepartyssn:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["responsiblepartybirthdate"] != DBNull.Value && LocalPatient["responsiblepartybirthdate"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["responsiblepartybirthdate"].ToString().Trim()) : DateTime.Now) != (DentrixPatient["responsiblepartybirthdate"] != DBNull.Value && DentrixPatient["responsiblepartybirthdate"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["responsiblepartybirthdate"].ToString().Trim()) : DateTime.Now)
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("responsiblepartybirthdate:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Spouse_First_Name"] != DBNull.Value ? LocalPatient["Spouse_First_Name"].ToString().Trim() : "") != (DentrixPatient["Spouse_First_Name"] != DBNull.Value ? DentrixPatient["Spouse_First_Name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Spouse_First_Name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Spouse_Last_Name"] != DBNull.Value ? LocalPatient["Spouse_Last_Name"].ToString().Trim() : "") != (DentrixPatient["Spouse_Last_Name"] != DBNull.Value ? DentrixPatient["Spouse_Last_Name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Spouse_Last_Name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["ResponsibleParty_First_Name"] != DBNull.Value ? LocalPatient["ResponsibleParty_First_Name"].ToString().Trim() : "") != (DentrixPatient["ResponsibleParty_First_Name"] != DBNull.Value ? DentrixPatient["ResponsibleParty_First_Name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("ResponsibleParty_First_Name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["ResponsibleParty_Last_Name"] != DBNull.Value ? LocalPatient["ResponsibleParty_Last_Name"].ToString().Trim() : "") != (DentrixPatient["ResponsibleParty_Last_Name"] != DBNull.Value ? DentrixPatient["ResponsibleParty_Last_Name"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("ResponsibleParty_Last_Name:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Prim_Ins_Company_Phonenumber"] != DBNull.Value ? LocalPatient["Prim_Ins_Company_Phonenumber"].ToString().Trim() : "") != (DentrixPatient["Prim_Ins_Company_Phonenumber"] != DBNull.Value ? DentrixPatient["Prim_Ins_Company_Phonenumber"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();
                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Prim_Ins_Company_Phonenumber:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}


                    //itemsToBeUpdated2 = (from LocalPatient in dtLocalPatient.AsEnumerable()
                    //                     join DentrixPatient in dtDentrixPatient.AsEnumerable()
                    //                     on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                    //                     equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                    //                     //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                    //                     //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                    //                     where
                    //                     (LocalPatient["Sec_Ins_Company_Phonenumber"] != DBNull.Value ? LocalPatient["Sec_Ins_Company_Phonenumber"].ToString().Trim() : "") != (DentrixPatient["Sec_Ins_Company_Phonenumber"] != DBNull.Value ? DentrixPatient["Sec_Ins_Company_Phonenumber"].ToString().Trim() : "")
                    //                     select DentrixPatient).ToList();

                    //if (itemsToBeUpdated2.Count > 0)
                    //{
                    //    Utility.WriteToDebugSyncLogFile_All("Sec_Ins_Company_Phonenumber:" + itemsToBeUpdated2.Count, "PatientSyncNew");
                    //}

                    #endregion

                    #region "Modify"
                    var itemsToBeUpdated = (from LocalPatient in dtLocalPatient.AsEnumerable()
                                            join DentrixPatient in dtDentrixPatient.AsEnumerable()
                                            on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                            equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                                            //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                            //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
                                            where
                                            (LocalPatient["First_name"] != DBNull.Value ? LocalPatient["First_name"].ToString().Trim() : "") != (DentrixPatient["First_name"] != DBNull.Value ? DentrixPatient["First_name"].ToString().Trim() : "") ||
                                            (LocalPatient["Last_name"] != DBNull.Value ? LocalPatient["Last_name"].ToString().Trim() : "") != (DentrixPatient["Last_name"] != DBNull.Value ? DentrixPatient["Last_name"].ToString().Trim() : "") ||
                                            (LocalPatient["Middle_Name"] != DBNull.Value ? LocalPatient["Middle_Name"].ToString().Trim() : "") != (DentrixPatient["Middle_Name"] != DBNull.Value ? DentrixPatient["Middle_Name"].ToString().Trim() : "") ||
                                            (LocalPatient["Salutation"] != DBNull.Value ? LocalPatient["Salutation"].ToString().Trim() : "") != (DentrixPatient["Salutation"] != DBNull.Value ? DentrixPatient["Salutation"].ToString().Trim() : "") ||
                                            (LocalPatient["Status"] != DBNull.Value ? LocalPatient["Status"].ToString().Trim() : "") != (DentrixPatient["Status"] != DBNull.Value ? DentrixPatient["Status"].ToString().Trim() : "") ||
                                            (LocalPatient["Sex"] != DBNull.Value ? LocalPatient["Sex"].ToString().Trim() : "") != (DentrixPatient["Sex"] != DBNull.Value ? DentrixPatient["Sex"].ToString().Trim() : "") ||
                                            (LocalPatient["MaritalStatus"] != DBNull.Value ? LocalPatient["MaritalStatus"].ToString().Trim() : "") != (DentrixPatient["MaritalStatus"] != DBNull.Value ? DentrixPatient["MaritalStatus"].ToString().Trim() : "") ||
                                            (LocalPatient["Birth_Date"] != DBNull.Value && LocalPatient["Birth_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["Birth_Date"].ToString().Trim()).Date : DateTime.Now) != (DentrixPatient["Birth_Date"] != DBNull.Value && DentrixPatient["Birth_Date"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["Birth_Date"].ToString().Trim()).Date : DateTime.Now) ||
                                            (LocalPatient["Email"] != DBNull.Value ? LocalPatient["Email"].ToString().Trim() : "") != (DentrixPatient["Email"] != DBNull.Value ? DentrixPatient["Email"].ToString().Trim() : "") ||
                                            (LocalPatient["Mobile"] != DBNull.Value ? LocalPatient["Mobile"].ToString().Trim() : "") != (DentrixPatient["Mobile"] != DBNull.Value ? DentrixPatient["Mobile"].ToString().Trim() : "") ||
                                            (LocalPatient["Home_Phone"] != DBNull.Value ? LocalPatient["Home_Phone"].ToString().Trim() : "") != (DentrixPatient["Home_Phone"] != DBNull.Value ? DentrixPatient["Home_Phone"].ToString().Trim() : "") ||
                                            (LocalPatient["Work_Phone"] != DBNull.Value ? LocalPatient["Work_Phone"].ToString().Trim() : "") != (DentrixPatient["Work_Phone"] != DBNull.Value ? DentrixPatient["Work_Phone"].ToString().Trim() : "") ||
                                            (LocalPatient["Address1"] != DBNull.Value ? LocalPatient["Address1"].ToString().Trim() : "") != (DentrixPatient["Address1"] != DBNull.Value ? DentrixPatient["Address1"].ToString().Trim() : "") ||
                                            (LocalPatient["Address2"] != DBNull.Value ? LocalPatient["Address2"].ToString().Trim() : "") != (DentrixPatient["Address2"] != DBNull.Value ? DentrixPatient["Address2"].ToString().Trim() : "") ||
                                            (LocalPatient["City"] != DBNull.Value ? LocalPatient["City"].ToString().Trim() : "") != (DentrixPatient["City"] != DBNull.Value ? DentrixPatient["City"].ToString().Trim() : "") ||
                                            (LocalPatient["State"] != DBNull.Value ? LocalPatient["State"].ToString().Trim() : "") != (DentrixPatient["State"] != DBNull.Value ? DentrixPatient["State"].ToString().Trim() : "") ||
                                            (LocalPatient["Zipcode"] != DBNull.Value ? LocalPatient["Zipcode"].ToString().Trim() : "") != (DentrixPatient["Zipcode"] != DBNull.Value ? DentrixPatient["Zipcode"].ToString().Trim() : "") ||
                                            (LocalPatient["ResponsibleParty_Status"] != DBNull.Value ? LocalPatient["ResponsibleParty_Status"].ToString().Trim() : "") != (DentrixPatient["ResponsibleParty_Status"] != DBNull.Value ? DentrixPatient["ResponsibleParty_Status"].ToString().Trim() : "") ||
                                            (LocalPatient["CurrentBal"] != DBNull.Value ? LocalPatient["CurrentBal"].ToString().Trim() : "") != (DentrixPatient["CurrentBal"] != DBNull.Value ? DentrixPatient["CurrentBal"].ToString().Trim() : "") ||
                                            (LocalPatient["ThirtyDay"] != DBNull.Value ? LocalPatient["ThirtyDay"].ToString().Trim() : "") != (DentrixPatient["ThirtyDay"] != DBNull.Value ? DentrixPatient["ThirtyDay"].ToString().Trim() : "") ||
                                            (LocalPatient["SixtyDay"] != DBNull.Value ? LocalPatient["SixtyDay"].ToString().Trim() : "") != (DentrixPatient["SixtyDay"] != DBNull.Value ? DentrixPatient["SixtyDay"].ToString().Trim() : "") ||
                                            (LocalPatient["NinetyDay"] != DBNull.Value ? LocalPatient["NinetyDay"].ToString().Trim() : "") != (DentrixPatient["NinetyDay"] != DBNull.Value ? DentrixPatient["NinetyDay"].ToString().Trim() : "") ||
                                            (LocalPatient["Over90"] != DBNull.Value ? LocalPatient["Over90"].ToString().Trim() : "") != (DentrixPatient["Over90"] != DBNull.Value ? DentrixPatient["Over90"].ToString().Trim() : "") ||
                                            (LocalPatient["FirstVisit_Date"] != DBNull.Value && LocalPatient["FirstVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["FirstVisit_Date"].ToString().Trim()) : DateTime.Now) != (DentrixPatient["FirstVisit_Date"] != DBNull.Value && DentrixPatient["FirstVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["FirstVisit_Date"].ToString().Trim()) : DateTime.Now) ||
                                            (LocalPatient["LastVisit_Date"] != DBNull.Value && LocalPatient["LastVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["LastVisit_Date"].ToString().Trim()) : DateTime.Now) != (DentrixPatient["LastVisit_Date"] != DBNull.Value && DentrixPatient["LastVisit_Date"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["LastVisit_Date"].ToString().Trim()) : DateTime.Now) ||
                                            (LocalPatient["Primary_Insurance"] != DBNull.Value ? LocalPatient["Primary_Insurance"].ToString().Trim() : "") != (DentrixPatient["Primary_Insurance"] != DBNull.Value ? DentrixPatient["Primary_Insurance"].ToString().Trim() : "") ||
                                            (LocalPatient["Primary_Insurance_CompanyName"] != DBNull.Value ? LocalPatient["Primary_Insurance_CompanyName"].ToString().Trim() : "") != (DentrixPatient["Primary_Insurance_CompanyName"] != DBNull.Value ? DentrixPatient["Primary_Insurance_CompanyName"].ToString().Trim() : "") ||
                                            (LocalPatient["Secondary_Insurance"] != DBNull.Value ? LocalPatient["Secondary_Insurance"].ToString().Trim() : "") != (DentrixPatient["Secondary_Insurance"] != DBNull.Value ? DentrixPatient["Secondary_Insurance"].ToString().Trim() : "") ||
                                            (LocalPatient["Secondary_Insurance_CompanyName"] != DBNull.Value ? LocalPatient["Secondary_Insurance_CompanyName"].ToString().Trim() : "") != (DentrixPatient["Secondary_Insurance_CompanyName"] != DBNull.Value ? DentrixPatient["Secondary_Insurance_CompanyName"].ToString().Trim() : "") ||
                                            (LocalPatient["Guar_ID"] != DBNull.Value ? LocalPatient["Guar_ID"].ToString().Trim() : "") != (DentrixPatient["Guar_ID"] != DBNull.Value ? DentrixPatient["Guar_ID"].ToString().Trim() : "") ||
                                            (LocalPatient["Pri_Provider_ID"] != DBNull.Value ? LocalPatient["Pri_Provider_ID"].ToString().Trim() : "") != (DentrixPatient["Pri_Provider_ID"] != DBNull.Value ? DentrixPatient["Pri_Provider_ID"].ToString().Trim() : "") ||
                                            (LocalPatient["Sec_Provider_ID"] != DBNull.Value ? LocalPatient["Sec_Provider_ID"].ToString().Trim() : "") != (DentrixPatient["Sec_Provider_ID"] != DBNull.Value ? DentrixPatient["Sec_Provider_ID"].ToString().Trim() : "") ||
                                            (LocalPatient["ReceiveSms"] != DBNull.Value ? LocalPatient["ReceiveSms"].ToString().Trim() : "") != (DentrixPatient["ReceiveSms"] != DBNull.Value ? DentrixPatient["ReceiveSms"].ToString().Trim() : "") ||
                                            (LocalPatient["ReceiveEmail"] != DBNull.Value ? LocalPatient["ReceiveEmail"].ToString().Trim() : "") != (DentrixPatient["ReceiveEmail"] != DBNull.Value ? DentrixPatient["ReceiveEmail"].ToString().Trim() : "") ||
                                            (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["nextvisit_date"].ToString().Trim()) : DateTime.Now) != (DentrixPatient["nextvisit_date"] != DBNull.Value && DentrixPatient["nextvisit_date"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["nextvisit_date"].ToString().Trim()) : DateTime.Now) ||
                                            (LocalPatient["due_date"] != DBNull.Value && LocalPatient["due_date"].ToString().Trim() != "" ? LocalPatient["due_date"].ToString().Trim() : "") != (DentrixPatient["due_date"] != DBNull.Value && DentrixPatient["due_date"].ToString().Trim() != "" ? DentrixPatient["due_date"].ToString().Trim() : "") ||
                                            (LocalPatient["remaining_benefit"] != DBNull.Value ? LocalPatient["remaining_benefit"].ToString().Trim() : "") != (DentrixPatient["remaining_benefit"] != DBNull.Value ? DentrixPatient["remaining_benefit"].ToString().Trim() : "") ||
                                            (LocalPatient["collect_payment"] != DBNull.Value ? LocalPatient["collect_payment"].ToString().Trim() : "0") != (DentrixPatient["collect_payment"] != DBNull.Value ? DentrixPatient["collect_payment"].ToString().Trim() : "0") ||
                                            (LocalPatient["preferred_name"] != DBNull.Value ? LocalPatient["preferred_name"].ToString().Trim() : "") != (DentrixPatient["preferred_name"] != DBNull.Value ? DentrixPatient["preferred_name"].ToString().Trim() : "") ||
                                            (LocalPatient["used_benefit"] != DBNull.Value ? LocalPatient["used_benefit"].ToString().Trim() : "") != (DentrixPatient["used_benefit"] != DBNull.Value ? DentrixPatient["used_benefit"].ToString().Trim() : "") ||
                                            (LocalPatient["Secondary_Ins_Subscriber_ID"] != DBNull.Value ? LocalPatient["Secondary_Ins_Subscriber_ID"].ToString().Trim() : "") != (DentrixPatient["Secondary_Ins_Subscriber_ID"] != DBNull.Value ? DentrixPatient["Secondary_Ins_Subscriber_ID"].ToString().Trim() : "") ||
                                            (LocalPatient["Primary_Ins_Subscriber_ID"] != DBNull.Value ? LocalPatient["Primary_Ins_Subscriber_ID"].ToString().Trim() : "") != (DentrixPatient["Primary_Ins_Subscriber_ID"] != DBNull.Value ? DentrixPatient["Primary_Ins_Subscriber_ID"].ToString().Trim() : "") ||
                                            (LocalPatient["EHR_Status"] != DBNull.Value ? LocalPatient["EHR_Status"].ToString().Trim() : "") != (DentrixPatient["EHR_Status"] != DBNull.Value ? DentrixPatient["EHR_Status"].ToString().Trim() : "") ||
                                            (LocalPatient["ReceiveVoiceCall"] != DBNull.Value ? LocalPatient["ReceiveVoiceCall"].ToString().Trim() : "") != (DentrixPatient["ReceiveVoiceCall"] != DBNull.Value ? DentrixPatient["ReceiveVoiceCall"].ToString().Trim() : "") ||
                                            (LocalPatient["PreferredLanguage"] != DBNull.Value ? LocalPatient["PreferredLanguage"].ToString().Trim() : "") != (DentrixPatient["PreferredLanguage"] != DBNull.Value ? DentrixPatient["PreferredLanguage"].ToString().Trim() : "") ||
                                            (LocalPatient["Patient_Note"] != DBNull.Value ? LocalPatient["Patient_Note"].ToString().Trim() : "") != (DentrixPatient["Patient_Note"] != DBNull.Value ? DentrixPatient["Patient_Note"].ToString().Trim() : "") ||
                                            (LocalPatient["SSN"] != DBNull.Value ? LocalPatient["SSN"].ToString().Trim() : "") != (DentrixPatient["SSN"] != DBNull.Value ? DentrixPatient["SSN"].ToString().Trim() : "") ||
                                            (LocalPatient["driverlicense"] != DBNull.Value ? LocalPatient["driverlicense"].ToString().Trim() : "") != (DentrixPatient["driverlicense"] != DBNull.Value ? DentrixPatient["driverlicense"].ToString().Trim() : "") ||
                                            (LocalPatient["groupid"] != DBNull.Value ? LocalPatient["groupid"].ToString().Trim() : "") != (DentrixPatient["groupid"] != DBNull.Value ? DentrixPatient["groupid"].ToString().Trim() : "") ||
                                            (LocalPatient["emergencycontactId"] != DBNull.Value ? LocalPatient["emergencycontactId"].ToString().Trim() : "") != (DentrixPatient["emergencycontactId"] != DBNull.Value ? DentrixPatient["emergencycontactId"].ToString().Trim() : "") ||
                                            (LocalPatient["EmergencyContact_First_Name"] != DBNull.Value ? LocalPatient["EmergencyContact_First_Name"].ToString().Trim() : "") != (DentrixPatient["EmergencyContact_First_Name"] != DBNull.Value ? DentrixPatient["EmergencyContact_First_Name"].ToString().Trim() : "") ||
                                            (LocalPatient["EmergencyContact_Last_Name"] != DBNull.Value ? LocalPatient["EmergencyContact_Last_Name"].ToString().Trim() : "") != (DentrixPatient["EmergencyContact_Last_Name"] != DBNull.Value ? DentrixPatient["EmergencyContact_Last_Name"].ToString().Trim() : "") ||
                                            (LocalPatient["emergencycontactnumber"] != DBNull.Value ? LocalPatient["emergencycontactnumber"].ToString().Trim() : "") != (DentrixPatient["emergencycontactnumber"] != DBNull.Value ? DentrixPatient["emergencycontactnumber"].ToString().Trim() : "") ||
                                            (LocalPatient["school"] != DBNull.Value ? LocalPatient["school"].ToString().Trim() : "") != (DentrixPatient["school"] != DBNull.Value ? DentrixPatient["school"].ToString().Trim() : "") ||
                                            (LocalPatient["employer"] != DBNull.Value ? LocalPatient["employer"].ToString().Trim() : "") != (DentrixPatient["employer"] != DBNull.Value ? DentrixPatient["employer"].ToString().Trim() : "") ||
                                            (LocalPatient["spouseId"] != DBNull.Value ? LocalPatient["spouseId"].ToString().Trim() : "") != (DentrixPatient["spouseId"] != DBNull.Value ? DentrixPatient["spouseId"].ToString().Trim() : "") ||
                                            (LocalPatient["responsiblepartyId"] != DBNull.Value ? LocalPatient["responsiblepartyId"].ToString().Trim() : "") != (DentrixPatient["responsiblepartyId"] != DBNull.Value ? DentrixPatient["responsiblepartyId"].ToString().Trim() : "") ||
                                            (LocalPatient["responsiblepartyssn"] != DBNull.Value ? LocalPatient["responsiblepartyssn"].ToString().Trim() : "") != (DentrixPatient["responsiblepartyssn"] != DBNull.Value ? DentrixPatient["responsiblepartyssn"].ToString().Trim() : "") ||
                                            (LocalPatient["responsiblepartybirthdate"] != DBNull.Value && LocalPatient["responsiblepartybirthdate"].ToString().Trim() != "" ? Convert.ToDateTime(LocalPatient["responsiblepartybirthdate"].ToString().Trim()) : DateTime.Now) != (DentrixPatient["responsiblepartybirthdate"] != DBNull.Value && DentrixPatient["responsiblepartybirthdate"].ToString().Trim() != "" ? Convert.ToDateTime(DentrixPatient["responsiblepartybirthdate"].ToString().Trim()) : DateTime.Now) ||
                                            (LocalPatient["Spouse_First_Name"] != DBNull.Value ? LocalPatient["Spouse_First_Name"].ToString().Trim() : "") != (DentrixPatient["Spouse_First_Name"] != DBNull.Value ? DentrixPatient["Spouse_First_Name"].ToString().Trim() : "") ||
                                            (LocalPatient["Spouse_Last_Name"] != DBNull.Value ? LocalPatient["Spouse_Last_Name"].ToString().Trim() : "") != (DentrixPatient["Spouse_Last_Name"] != DBNull.Value ? DentrixPatient["Spouse_Last_Name"].ToString().Trim() : "") ||
                                            (LocalPatient["ResponsibleParty_First_Name"] != DBNull.Value ? LocalPatient["ResponsibleParty_First_Name"].ToString().Trim() : "") != (DentrixPatient["ResponsibleParty_First_Name"] != DBNull.Value ? DentrixPatient["ResponsibleParty_First_Name"].ToString().Trim() : "") ||
                                            (LocalPatient["ResponsibleParty_Last_Name"] != DBNull.Value ? LocalPatient["ResponsibleParty_Last_Name"].ToString().Trim() : "") != (DentrixPatient["ResponsibleParty_Last_Name"] != DBNull.Value ? DentrixPatient["ResponsibleParty_Last_Name"].ToString().Trim() : "") ||
                                            (LocalPatient["Prim_Ins_Company_Phonenumber"] != DBNull.Value ? LocalPatient["Prim_Ins_Company_Phonenumber"].ToString().Trim() : "") != (DentrixPatient["Prim_Ins_Company_Phonenumber"] != DBNull.Value ? DentrixPatient["Prim_Ins_Company_Phonenumber"].ToString().Trim() : "") ||
                                            (LocalPatient["Sec_Ins_Company_Phonenumber"] != DBNull.Value ? LocalPatient["Sec_Ins_Company_Phonenumber"].ToString().Trim() : "") != (DentrixPatient["Sec_Ins_Company_Phonenumber"] != DBNull.Value ? DentrixPatient["Sec_Ins_Company_Phonenumber"].ToString().Trim() : "")
                                            select DentrixPatient).ToList();

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
                    #endregion

                    #region "Delete"
                    var itemToBeDeleted = (from LocalPatient in dtLocalPatient.AsEnumerable()
                                           join DentrixPatient in dtDentrixPatient.AsEnumerable()
                                           on LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                           equals DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                                           //on new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = LocalPatient["Clinic_Number"].ToString().Trim() }
                                           //equals new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim(), CliNum = DentrixPatient["Clinic_Number"].ToString().Trim() }
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
                    #endregion

                    if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                    {
                        bool isPatientSave = SynchDentrixBAL.Save_Patient_Dentrix_To_Local_New(dtSaveRecords, "0", "1");

                        if (isPatientSave)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            IsGetParientRecordDone = true;
                            SynchDataLiveDB_Push_Patient();
                        }
                    }
                    else
                    {
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                        bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                        IsGetParientRecordDone = true;
                    }

                    ObjGoalBase.WriteToSyncLogFile("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                    IsPatientSyncedFirstTime = true;
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;

                    SynchDataDentrix_PatientStatus();
                    SynchDataDentrix_PatientImages();
                    SynchDataDentrix_PatientDisease();
                    SynchDataDentrix_PatientMedication("");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Error in [SyncDataDentrix_Patient_New] : " + ex.Message + " " + ex.StackTrace);
                throw ex;
            }
        }      

        public void SynchDataDentrix_Patient()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                IsParientFirstSync = true;
                Is_synched_Patient = false;
                Is_synched_AppointmentsPatient = false;
            }
            if (Utility.IsApplicationIdleTimeOff && IsParientFirstSync && !Is_synched_Patient && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    Is_Synched_PatientCallHit = false;
                    Is_synched_Patient = true;
                    IsParientFirstSync = false;
                    SynchDataLiveDB_Pull_EHR_Patientoptout();
                    DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixPatientData();

                    // DataTable dtDentrixPatientNextApptDate = SynchDentrixBAL.GetDentrixPatientNextApptDate();
                    DataTable dtDentrixPatientdue_date = SynchDentrixBAL.GetDentrixPatientdue_date();

                    //  DataTable dtDentrixPatientcollect_payment = SynchDentrixBAL.GetDentrixPatientcollect_payment();

                    //dtDentrixPatient.Columns.Add("InsUptDlt", typeof(int));
                    //dtDentrixPatient.Columns.Add("collect_payment", typeof(string));

                    string patientTableName = "Patient";

                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");

                    if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                    {
                        patientTableName = "PatientCompare";
                    }



                    if (dtDentrixPatient != null && dtDentrixPatient.Rows.Count > 0)
                    {
                        bool isPatientSave = SynchDentrixBAL.Save_Patient_Dentrix_To_Local(dtDentrixPatient, patientTableName, dtDentrixPatientdue_date, dtLocalPatient, true);
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
                    IsPatientSyncedFirstTime = true;
                    IsParientFirstSync = true;
                    Is_synched_Patient = false;
                    SynchDataDentrix_PatientStatus();
                }
                catch (Exception ex)
                {
                    Is_synched_Patient = false;
                    IsParientFirstSync = true;
                    ObjGoalBase.WriteToErrorLogFile("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
                SynchDataDentrix_PatientImages();
                SynchDataDentrix_PatientDisease();
                SynchDataDentrix_PatientMedication("");
            }
            else if (Is_synched_AppointmentsPatient)
            {
                Is_Synched_PatientCallHit = true;
            }
        }

        public void SynchDataDentrix_PatientStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtDentrixPatientStatus = SynchDentrixBAL.GetDentrixPatientStatusData();
                    if (dtDentrixPatientStatus != null && dtDentrixPatientStatus.Rows.Count > 0)
                    {
                        SynchLocalBAL.UpdatePatient_Status(dtDentrixPatientStatus, "1");
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

        public void SynchDataDentrix_AppointmentsPatient()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                Is_synched_Patient = false;
                Is_synched_AppointmentsPatient = false;
            }
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    IS_Appt_Patient_Sync = false;
                    DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixAppointmentsPatientData();

                    // DataTable dtDentrixPatientNextApptDate = SynchDentrixBAL.GetDentrixPatientNextApptDate();
                    DataTable dtDentrixPatientdue_date = SynchDentrixBAL.GetDentrixPatientdue_date();

                    //  DataTable dtDentrixPatientcollect_payment = SynchDentrixBAL.GetDentrixPatientcollect_payment();

                    //dtDentrixPatient.Columns.Add("InsUptDlt", typeof(int));
                    //dtDentrixPatient.Columns.Add("collect_payment", typeof(string));

                    string patientTableName = "Patient";
                    string PatientEHRIDs = string.Join("','", dtDentrixPatient.AsEnumerable().Select(p => p.Field<object>("Patient_EHR_Id").ToString()).Distinct());

                    if (PatientEHRIDs != string.Empty)
                    {
                        Is_synched_AppointmentsPatient = true;

                        PatientEHRIDs = "'" + PatientEHRIDs + "'";
                        DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, "1");

                        if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                        {
                            patientTableName = "PatientCompare";
                        }

                        if (dtDentrixPatient != null && dtDentrixPatient.Rows.Count > 0)
                        {
                            bool isPatientSave = SynchDentrixBAL.Save_Patient_Dentrix_To_Local(dtDentrixPatient, patientTableName, dtDentrixPatientdue_date, dtLocalPatient);
                            if (isPatientSave)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                IsGetParientRecordDone = true;

                                SynchDataLiveDB_Push_Patient();
                            }
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                            IsGetParientRecordDone = true;
                        }
                        // SynchDataDentrix_PatientDisease();
                        // SynchDataDentrix_PatientMedication();
                        Is_synched_AppointmentsPatient = false;
                        if (Is_Synched_PatientCallHit)
                        {
                            //Is_Synched_PatientCallHit = false;
                            //SynchDataDentrix_Patient();
                        }

                    }
                    IS_Appt_Patient_Sync = true;
                }
                catch (Exception ex)
                {
                    IS_Appt_Patient_Sync = false;
                    Is_synched_AppointmentsPatient = false;
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }


        public void SynchDataDentrix_AppointmentsPatient_New()
        {
            if (Utility.IsExternalAppointmentSync)
            {
                Is_synched_Patient = false;
                Is_synched_AppointmentsPatient = false;
            }
            if (Utility.IsApplicationIdleTimeOff && !Is_synched_AppointmentsPatient && Utility.AditLocationSyncEnable)
            {
                try
                {
                    IS_Appt_Patient_Sync = false;
                    DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixAppointmentsPatientData();

                    // DataTable dtDentrixPatientNextApptDate = SynchDentrixBAL.GetDentrixPatientNextApptDate();
                    DataTable dtDentrixPatientdue_date = SynchDentrixBAL.GetDentrixPatientdue_date();

                    foreach (DataRow dr in dtDentrixPatient.Rows)
                    {
                        string tmpdue_date = "";
                        try
                        {
                            DataRow[] Patdue_date = dtDentrixPatientdue_date.Select("patient_id = '" + dr["Patient_EHR_ID"] + "'");
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
                                        tmpdue_date = SortPatdue_date.Rows[i]["due_date"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type_id"].ToString() + "|" + tmpdue_date;
                                    }
                                    dr["due_date"] = tmpdue_date;
                                }
                                else
                                {
                                    DataTable tmpDatatableduedate = new DataTable();
                                    tmpDatatableduedate = Patdue_date.CopyToDataTable();
                                    DataView view = tmpDatatableduedate.DefaultView;
                                    view.Sort = "due_date desc";
                                    DataTable SortPatdue_date = view.ToTable();
                                    foreach (DataRow due_row in SortPatdue_date.Rows)
                                    {
                                        tmpdue_date = due_row["due_date"].ToString() + "@" + due_row["recall_type"].ToString() + "@" + due_row["recall_type_id"].ToString() + "|" + tmpdue_date;
                                    }
                                    dr["due_date"] = tmpdue_date;
                                }
                            }
                        }
                        catch (Exception x)
                        {
                            Utility.WriteToErrorLogFromAll("Error in generating due/recall date: " + x.Message);
                            tmpdue_date = string.Empty;
                        }
                    }

                    string patientTableName = "Patient";
                    //string PatientEHRIDs = string.Join("','", dtDentrixPatient.AsEnumerable().Select(p => p["Patient_EHR_Id"].ToString()).Distinct());
                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");

                    if (dtLocalPatient.Rows.Count > 0)
                    {
                        Is_synched_AppointmentsPatient = true;

                        //PatientEHRIDs = "'" + PatientEHRIDs + "'";
                        //DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientDataByPatientEHRID(PatientEHRIDs, "1");

                        DataTable dtSaveRecords = new DataTable();
                        dtSaveRecords = dtDentrixPatient.Clone();
                        #region "Add"
                        var itemsToBeAdded = (from DentrixPatient in dtDentrixPatient.AsEnumerable()
                                              join LocalPatient in dtLocalPatient.AsEnumerable()
                                              on DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                                              equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                              into matchingRows
                                              from matchingRow in matchingRows.DefaultIfEmpty()
                                              where matchingRow == null
                                              select DentrixPatient).ToList();
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
                        #endregion

                        #region "Modify"
                        var itemsToBeUpdated = (from DentrixPatient in dtDentrixPatient.AsEnumerable()
                                                join LocalPatient in dtLocalPatient.AsEnumerable()
                                                on DentrixPatient["Patient_EHR_ID"].ToString().Trim() + "_" + DentrixPatient["Clinic_Number"].ToString().Trim()
                                                equals LocalPatient["Patient_EHR_ID"].ToString().Trim() + "_" + LocalPatient["Clinic_Number"].ToString().Trim()
                                                where
                                                 (DentrixPatient["nextvisit_date"] != DBNull.Value && DentrixPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(DentrixPatient["nextvisit_date"]) : DateTime.Now)
                                                 !=
                                                 (LocalPatient["nextvisit_date"] != DBNull.Value && LocalPatient["nextvisit_date"].ToString() != string.Empty ? Convert.ToDateTime(LocalPatient["nextvisit_date"]) : DateTime.Now)

                                                 ||

                                                 (DentrixPatient["EHR_Status"].ToString().Trim()) != (LocalPatient["EHR_Status"].ToString().Trim())

                                                 ||

                                                 (DentrixPatient["due_date"].ToString().Trim()) != (LocalPatient["due_date"].ToString().Trim())

                                                 || (DentrixPatient["First_name"].ToString().Trim()) != (LocalPatient["First_name"].ToString().Trim())
                                                 || (DentrixPatient["Last_name"].ToString().Trim()) != (LocalPatient["Last_name"].ToString().Trim())
                                                 || (Utility.ConvertContactNumber(DentrixPatient["Home_Phone"].ToString().Trim())) != (Utility.ConvertContactNumber(LocalPatient["Home_Phone"].ToString().Trim()))
                                                 || (DentrixPatient["Middle_Name"].ToString().Trim()) != (LocalPatient["Middle_Name"].ToString().Trim())
                                                 || (DentrixPatient["Status"].ToString().Trim()) != (LocalPatient["Status"].ToString().Trim())
                                                 || (DentrixPatient["Email"].ToString().Trim()) != (LocalPatient["Email"].ToString().Trim())
                                                 || (Utility.ConvertContactNumber(DentrixPatient["Mobile"].ToString().Trim())) != (Utility.ConvertContactNumber(LocalPatient["Mobile"].ToString().Trim()))
                                                //|| (DentrixPatient["PreferredLanguage"].ToString().Trim()) != (LocalPatient["PreferredLanguage"].ToString().Trim())
                                                //First_name, Last_name, Home_Phone, Middle_Name, Status, Email, Mobile, ReceiveSMS, PreferredLanguage
                                                select DentrixPatient).ToList();

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
                        #endregion

                        if (dtSaveRecords != null && dtSaveRecords.Rows.Count > 0)
                        {
                            bool isPatientSave = SynchDentrixBAL.Save_Patient_Dentrix_To_Local_New(dtSaveRecords, "0", "1");

                            if (isPatientSave)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                IsGetParientRecordDone = true;
                                SynchDataLiveDB_Push_Patient();
                            }
                        }
                        else
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                            IsGetParientRecordDone = true;
                        }
                        ObjGoalBase.WriteToSyncLogFile("Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        Is_synched_AppointmentsPatient = false;
                    }
                    IS_Appt_Patient_Sync = true;
                }
                catch (Exception ex)
                {
                    IS_Appt_Patient_Sync = false;
                    Is_synched_AppointmentsPatient = false;
                    ObjGoalBase.WriteToErrorLogFile("[Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }
        public void SynchDataDentrix_NewPatient()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixNewPatientData();
                    if (dtDentrixPatient != null && dtDentrixPatient.Rows.Count > 0)
                    {
                        DataTable dtLocalPatientResult = SynchLocalBAL.GetLocalNewPatientData("1");
                        if ((dtLocalPatientResult != null && dtLocalPatientResult.Rows.Count > 0 && dtLocalPatientResult.Select("Is_Adit_Updated = 1").Length > 0))
                        {
                            DataTable dtSaveRecords = new DataTable();
                            var itemsToBeAdded = (from DentrixPatient in dtDentrixPatient.AsEnumerable()
                                                  join LocalPatient in dtLocalPatientResult.AsEnumerable()
                                                  on new { PatID = DentrixPatient["Patient_EHR_ID"].ToString().Trim() }
                                                  equals new { PatID = LocalPatient["Patient_EHR_ID"].ToString().Trim() }
                                                  into matchingRows
                                                  from matchingRow in matchingRows.DefaultIfEmpty()
                                                  where matchingRow == null
                                                  select DentrixPatient).ToList();
                            if (itemsToBeAdded.Count > 0)
                            {
                                string strPatID = String.Join(",", itemsToBeAdded.AsEnumerable().Select(x => x["Patient_EHR_ID"].ToString()).ToArray());
                                strPatID = "'" + strPatID.Replace(",", "','") + "'";
                                if (!string.IsNullOrEmpty(strPatID))
                                {
                                    dtSaveRecords = SynchDentrixBAL.GetDentrixPatientData(strPatID);
                                }
                            }
                            if (!dtSaveRecords.Columns.Contains("InsUptDlt"))
                            {
                                dtSaveRecords.Columns.Add("InsUptDlt", typeof(int));
                                dtSaveRecords.Columns["InsUptDlt"].DefaultValue = 0;
                            }
                            if (dtSaveRecords.Rows.Count > 0)
                            {
                                bool status = SynchDentrixBAL.Save_Patient_Dentrix_To_Local_New(dtSaveRecords, "0", "1");
                                //Utility.WriteToSyncLogFile_All("Patient inserted or updated : " + System.DateTime.Now.ToString());

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
        public string GetDentrixG62ExePath()
        {
            string patientPicPath = "";
            try
            {
                RegistryKey hKey = Registry.CurrentUser.OpenSubKey(@"Software\Dentrix Dental Systems, Inc.\Dentrix\General");
                if (hKey != null)
                {
                    Object value = hKey.GetValue("Path");
                    if (value != null)
                    {
                        patientPicPath = value.ToString() + "PatPicts\\";
                    }
                }
                return patientPicPath;
            }
            catch (Exception)
            {
                return "C:\\DENTRIX\\Common\\DentrixSQL\\PatPicts\\";
            }
            // return retv;
        }


        public void SynchDataDentrix_PatientImages()
        {
            try
            {
                if (Utility.IsExternalAppointmentSync)
                {
                    Is_synched_PatientImages = false;
                }
                if (!Is_synched_PatientImages && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    // SynchDataLiveDB_Push_PatientImage();

                    Is_synched_PatientImages = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtOpenDentalPatientImages = SynchDentrixBAL.GetDentrixPatientImagesData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), GetDentrixG62ExePath());
                        dtOpenDentalPatientImages.Columns.Add("InsUptDlt", typeof(int));
                        //dtOpenDentalPatientImages.Columns.Add("SourceLocation", typeof(string));
                        dtOpenDentalPatientImages.Columns["InsUptDlt"].DefaultValue = 0;
                        DataTable dtLocalPatientImages = SynchLocalBAL.GetLocalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString());
                        Utility.EHRProfileImagePath = GetDentrixG62ExePath();//SynchOpenDentalDAL.GetOpenDentalDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        foreach (DataRow dtDtxRow in dtOpenDentalPatientImages.Rows)
                        {
                            //if (Utility.EHRProfileImagePath == string.Empty || Utility.EHRProfileImagePath == "")
                            //{
                            //    dtDtxRow["SourceLocation"] = "C:\\OpenDentImages\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                            //}
                            //else
                            //{
                            //    dtDtxRow["SourceLocation"] = Utility.EHRProfileImagePath + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString().Substring(0, 1).ToUpper() + "\\" + dtDtxRow["Patient_Images_FilePath"].ToString();
                            //}
                            DataRow[] row = dtLocalPatientImages.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");
                            if (row.Length > 0)
                            {
                                if (!Convert.ToBoolean(row[0]["Is_Deleted"]))
                                {
                                    if (dtDtxRow["Patient_Images_EHR_ID"].ToString().Trim() != row[0]["Patient_Images_EHR_ID"].ToString().Trim())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["Patient_Images_FilePath"].ToString() != row[0]["Patient_Images_FilePath"].ToString())
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (Convert.ToDateTime(dtDtxRow["Entry_DateTime"]) != Convert.ToDateTime(row[0]["Entry_DateTime"]))
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
                            DataRow[] row = dtOpenDentalPatientImages.Copy().Select("Patient_EHR_ID = '" + dtDtlRow["Patient_EHR_ID"].ToString().Trim() + "' ");
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

        public void SynchDataDentrix_PatientDisease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && !Is_synched_PatientDisease)
                {
                    Is_synched_PatientDisease = true;
                    DataTable dtDentrixDisease = SynchDentrixBAL.GetDentrixPatientDiseaseData();
                    dtDentrixDisease.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalDisease = SynchLocalBAL.GetLocalPatientDiseaseData("1");

                    DataTable dtSaveRecords = new DataTable();
                    dtSaveRecords = dtLocalDisease.Clone();

                    var itemsToBeAdded = (from OpenDental in dtDentrixDisease.AsEnumerable()
                                          join Local in dtLocalDisease.AsEnumerable()
                                          on OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Patient_EHR_ID"].ToString().Trim()
                                          equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim()
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

                    var itemsToBeUpdated = (from OpenDental in dtDentrixDisease.AsEnumerable()
                                            join Local in dtLocalDisease.AsEnumerable()
                                            on OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Patient_EHR_ID"].ToString().Trim()
                                            equals Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim()
                                            where
                                           OpenDental["EHR_Entry_DateTime"].ToString().Trim() != Local["EHR_Entry_DateTime"].ToString().Trim() ||
                                           OpenDental["Disease_Name"].ToString().Trim() != Local["Disease_Name"].ToString().Trim() ||
                                           (Local["Is_deleted"] != DBNull.Value ? Convert.ToInt32(Local["Is_deleted"]).ToString().Trim() : "0") != (OpenDental["Is_deleted"] != DBNull.Value ? OpenDental["Is_deleted"].ToString().Trim() : "0")
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

                    var itemToBeDeleted = (from Local in dtLocalDisease.AsEnumerable()
                                           join OpenDental in dtDentrixDisease.AsEnumerable()
                                           on Local["Disease_EHR_ID"].ToString().Trim() + "_" + Local["Patient_EHR_ID"].ToString().Trim()
                                           equals OpenDental["Disease_EHR_ID"].ToString().Trim() + "_" + OpenDental["Patient_EHR_ID"].ToString().Trim()
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


                    #region Old Code
                    //foreach (DataRow dtDtxRow in dtDentrixDisease.Rows)
                    //{
                    //    DataRow[] row = dtLocalDisease.Copy().Select("Disease_EHR_ID = '" + dtDtxRow["Disease_EHR_ID"].ToString() + "' and Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"].ToString() + "'");
                    //    if (row.Length > 0)
                    //    {
                    //        if (dtDtxRow["EHR_Entry_DateTime"].ToString().Trim() != row[0]["EHR_Entry_DateTime"].ToString().Trim())
                    //        {
                    //            dtDtxRow["InsUptDlt"] = 2;
                    //        }
                    //        else if (dtDtxRow["Disease_Name"].ToString().Trim() != row[0]["Disease_Name"].ToString().Trim())
                    //        {
                    //            dtDtxRow["InsUptDlt"] = 2;
                    //        }
                    //        else if (Convert.ToBoolean(dtDtxRow["is_deleted"]) != Convert.ToBoolean(row[0]["is_deleted"]))
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
                    //    DataRow rowBlcOpt = dtDentrixDisease.Copy().Select("Disease_EHR_ID = '" + dtRow["Disease_EHR_ID"].ToString().Trim() + "' AND Disease_Type = '" + dtRow["Disease_Type"].ToString().Trim() + "' and Patient_EHR_ID = '" + dtRow["Patient_EHR_ID"] + "'").FirstOrDefault();
                    //    if (rowBlcOpt == null)
                    //    {
                    //        DataRow dtxtldr = dtDentrixDisease.NewRow();
                    //        dtxtldr["Disease_EHR_ID"] = dtRow["Disease_EHR_ID"].ToString().Trim();
                    //        dtxtldr["Patient_EHR_ID"] = dtRow["Patient_EHR_ID"].ToString().Trim();
                    //        dtxtldr["Disease_Type"] = dtRow["Disease_Type"].ToString().Trim();
                    //        dtxtldr["Disease_Name"] = dtRow["Disease_Name"].ToString().Trim();
                    //        dtxtldr["Clinic_Number"] = dtRow["Clinic_Number"].ToString().Trim();
                    //        dtxtldr["InsUptDlt"] = 3;
                    //        dtDentrixDisease.Rows.Add(dtxtldr);
                    //    }
                    //}
                    //dtDentrixDisease.AcceptChanges();
                    #endregion

                    if (dtSaveRecords.Rows.Count > 0 && dtSaveRecords.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                    //if (dtDentrixDisease != null && dtDentrixDisease.Rows.Count > 0)
                    {
                        //bool status = SynchDentrixBAL.Save_PatientDisease_Dentrix_To_Local(dtDentrixDisease);
                        bool status = SynchDentrixBAL.Save_PatientDisease_Dentrix_To_Local(dtSaveRecords);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                            ObjGoalBase.WriteToSyncLogFile("PatientDisease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_PatientDisease();
                        }
                    }

                    Is_synched_PatientDisease = false;
                }


            }
            catch (Exception ex)
            {
                Is_synched_PatientDisease = false;
                ObjGoalBase.WriteToErrorLogFile("[PatientDisease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataDentrix_PatientMedication(string Patient_EHR_IDS)
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                {
                    Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchDentrixBAL.GetDentrixPatientMedicationData(Patient_EHR_IDS);
                        dtMedication.Columns.Add("is_active", typeof(string));
                        foreach (DataRow row in dtMedication.Rows)
                        {
                            if (row["is_active1"].ToString().Trim().ToUpper() == "TRUE")
                            {
                                row["is_active"] = "True";
                            }
                            else
                            {
                                row["is_active"] = "False";
                            }
                        }
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalPatientMedicationData("1", Patient_EHR_IDS);

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
                                else if (dtDtxRow["is_active1"].ToString().Trim() != row[0]["is_active"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["Patient_Notes"].ToString().Trim() != row[0]["Patient_Notes"].ToString().Trim())
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
        #endregion

        #region Synch Disease

        private void fncSynchDataDentrix_Disease()
        {
            InitBgWorkerDentrix_Disease();
            InitBgTimerDentrix_Disease();
        }

        private void InitBgTimerDentrix_Disease()
        {
            timerSynchDentrix_Disease = new System.Timers.Timer();
            this.timerSynchDentrix_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchDentrix_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_Disease_Tick);
            timerSynchDentrix_Disease.Enabled = true;
            timerSynchDentrix_Disease.Start();
        }

        private void InitBgWorkerDentrix_Disease()
        {
            bwSynchDentrix_Disease = new BackgroundWorker();
            bwSynchDentrix_Disease.WorkerReportsProgress = true;
            bwSynchDentrix_Disease.WorkerSupportsCancellation = true;
            bwSynchDentrix_Disease.DoWork += new DoWorkEventHandler(bwSynchDentrix_Disease_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchDentrix_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_Disease_RunWorkerCompleted);
        }

        private void timerSynchDentrix_Disease_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_Disease.Enabled = false;
                MethodForCallSynchOrderDentrix_Disease();
            }
        }

        public void MethodForCallSynchOrderDentrix_Disease()
        {
            System.Threading.Thread procThreadmainDentrix_Disease = new System.Threading.Thread(this.CallSyncOrderTableDentrix_Disease);
            procThreadmainDentrix_Disease.Start();
        }

        public void CallSyncOrderTableDentrix_Disease()
        {
            if (bwSynchDentrix_Disease.IsBusy != true)
            {
                bwSynchDentrix_Disease.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_Disease_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_Disease.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_Disease();
                //  SynchDataDentrix_DiseaseHours();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_Disease.Enabled = true;
        }

        public void SynchDataDentrix_Disease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {

                    DataTable dtDentrixDisease = SynchDentrixBAL.GetDentrixDiseaseData();
                    dtDentrixDisease.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalDisease = SynchLocalBAL.GetLocalDiseaseData("1");

                    foreach (DataRow dtDtxRow in dtDentrixDisease.Rows)
                    {
                        DataRow[] row = dtLocalDisease.Copy().Select("Disease_EHR_ID = '" + dtDtxRow["Disease_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["EHR_Entry_DateTime"].ToString().Trim() != row[0]["EHR_Entry_DateTime"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["Disease_Name"].ToString().Trim() != row[0]["Disease_Name"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;

                            }
                            else if (Convert.ToBoolean(dtDtxRow["is_deleted"]) != Convert.ToBoolean(row[0]["is_deleted"]))
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
                        DataRow rowBlcOpt = dtDentrixDisease.Copy().Select("Disease_EHR_ID = '" + dtRow["Disease_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                        if (rowBlcOpt == null)
                        {
                            DataRow dtxtldr = dtDentrixDisease.NewRow();
                            dtxtldr["Disease_EHR_ID"] = dtRow["Disease_EHR_ID"].ToString().Trim();
                            dtxtldr["Disease_Type"] = dtRow["Disease_Type"].ToString().Trim();
                            dtxtldr["is_deleted"] = "1";
                            dtxtldr["InsUptDlt"] = 3;
                            dtDentrixDisease.Rows.Add(dtxtldr);
                        }
                    }
                    dtDentrixDisease.AcceptChanges();

                    if (dtDentrixDisease != null && dtDentrixDisease.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_Disease_Dentrix_To_Local(dtDentrixDisease);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                            ObjGoalBase.WriteToSyncLogFile("Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Disease();
                        }
                    }
                    SynchDataDentrix_Medication();

                }

            }
            catch (Exception ex)
            {

                ObjGoalBase.WriteToErrorLogFile("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataDentrix_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchDentrixBAL.GetDentrixMedicationData();
                        dtMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalMedicationData("1");

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

        private void fncSynchDataDentrix_RecallType()
        {
            InitBgWorkerDentrix_RecallType();
            InitBgTimerDentrix_RecallType();
        }

        private void InitBgTimerDentrix_RecallType()
        {
            timerSynchDentrix_RecallType = new System.Timers.Timer();
            this.timerSynchDentrix_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchDentrix_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_RecallType_Tick);
            timerSynchDentrix_RecallType.Enabled = true;
            timerSynchDentrix_RecallType.Start();
        }

        private void InitBgWorkerDentrix_RecallType()
        {
            bwSynchDentrix_RecallType = new BackgroundWorker();
            bwSynchDentrix_RecallType.WorkerReportsProgress = true;
            bwSynchDentrix_RecallType.WorkerSupportsCancellation = true;
            bwSynchDentrix_RecallType.DoWork += new DoWorkEventHandler(bwSynchDentrix_RecallType_DoWork);
            bwSynchDentrix_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_RecallType_RunWorkerCompleted);
        }

        private void timerSynchDentrix_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_RecallType.Enabled = false;
                MethodForCallSynchOrderDentrix_RecallType();
            }
        }

        public void MethodForCallSynchOrderDentrix_RecallType()
        {
            System.Threading.Thread procThreadmainDentrix_RecallType = new System.Threading.Thread(this.CallSyncOrderTableDentrix_RecallType);
            procThreadmainDentrix_RecallType.Start();
        }

        public void CallSyncOrderTableDentrix_RecallType()
        {
            if (bwSynchDentrix_RecallType.IsBusy != true)
            {
                bwSynchDentrix_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_RecallType();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_RecallType.Enabled = true;
        }

        public void SynchDataDentrix_RecallType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtDentrixRecallType = SynchDentrixBAL.GetDentrixRecallTypeData();
                    dtDentrixRecallType.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData("1");

                    foreach (DataRow dtDtxRow in dtDentrixRecallType.Rows)
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
                        DataRow[] row = dtDentrixRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtDentrixRecallType.NewRow();
                            BlcOptDtldr["RecallType_EHR_ID"] = dtDtxRow["RecallType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtDentrixRecallType.Rows.Add(BlcOptDtldr);
                        }
                    }

                    dtDentrixRecallType.AcceptChanges();

                    if (dtDentrixRecallType != null && dtDentrixRecallType.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_RecallType_Dentrix_To_Local(dtDentrixRecallType);
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

        private void fncSynchDataDentrix_ApptStatus()
        {
            InitBgWorkerDentrix_ApptStatus();
            InitBgTimerDentrix_ApptStatus();
        }

        private void InitBgTimerDentrix_ApptStatus()
        {
            timerSynchDentrix_ApptStatus = new System.Timers.Timer();
            this.timerSynchDentrix_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchDentrix_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_ApptStatus_Tick);
            timerSynchDentrix_ApptStatus.Enabled = true;
            timerSynchDentrix_ApptStatus.Start();
        }

        private void InitBgWorkerDentrix_ApptStatus()
        {
            bwSynchDentrix_ApptStatus = new BackgroundWorker();
            bwSynchDentrix_ApptStatus.WorkerReportsProgress = true;
            bwSynchDentrix_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchDentrix_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchDentrix_ApptStatus_DoWork);
            bwSynchDentrix_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchDentrix_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_ApptStatus.Enabled = false;
                MethodForCallSynchOrderDentrix_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderDentrix_ApptStatus()
        {
            System.Threading.Thread procThreadmainDentrix_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableDentrix_ApptStatus);
            procThreadmainDentrix_ApptStatus.Start();
        }

        public void CallSyncOrderTableDentrix_ApptStatus()
        {
            if (bwSynchDentrix_ApptStatus.IsBusy != true)
            {
                bwSynchDentrix_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_ApptStatus();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_ApptStatus.Enabled = true;
        }

        public void SynchDataDentrix_ApptStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtDentrixApptStatus = SynchDentrixBAL.GetDentrixApptStatusData();
                    dtDentrixApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtDentrixApptStatus.Rows.Add(0, "<none>", "normal");
                    dtDentrixApptStatus.Rows.Add(-106, "<COMPLETE>", "normal");
                    dtDentrixApptStatus.AcceptChanges();

                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData("1");

                    foreach (DataRow dtDtxRow in dtDentrixApptStatus.Rows)
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

                    dtDentrixApptStatus.AcceptChanges();

                    if (dtDentrixApptStatus != null && dtDentrixApptStatus.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_ApptStatus_Dentrix_To_Local(dtDentrixApptStatus);
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

        #region Synch User
        private void fncSynchDataDentrix_User()
        {
            InitBgWorkerDentrix_User();
            InitBgTimerDentrix_User();
        }

        private void InitBgTimerDentrix_User()
        {
            timerSynchDentrix_User = new System.Timers.Timer();
            this.timerSynchDentrix_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchDentrix_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_User_Tick);
            timerSynchDentrix_User.Enabled = true;
            timerSynchDentrix_User.Start();
        }

        private void timerSynchDentrix_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_User.Enabled = false;
                MethodForCallSynchOrderDentrix_User();
            }
        }

        private void MethodForCallSynchOrderDentrix_User()
        {
            System.Threading.Thread procThreadmainDentrix_User = new System.Threading.Thread(this.CallSyncOrderTableDentrix_User);
            procThreadmainDentrix_User.Start();
        }

        private void CallSyncOrderTableDentrix_User()
        {
            if (bwSynchDentrix_User.IsBusy != true)
            {
                bwSynchDentrix_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void InitBgWorkerDentrix_User()
        {
            bwSynchDentrix_User = new BackgroundWorker();
            bwSynchDentrix_User.WorkerReportsProgress = true;
            bwSynchDentrix_User.WorkerSupportsCancellation = true;
            bwSynchDentrix_User.DoWork += new DoWorkEventHandler(bwSynchDentrix_User_DoWork);
            bwSynchDentrix_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_User_RunWorkerCompleted);
        }

        private void bwSynchDentrix_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_User.Enabled = true;
        }

        private void bwSynchDentrix_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void SynchDataDentrix_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtDentrixUser = SynchDentrixBAL.GetDentrixUsersData();
                    dtDentrixUser.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalUser = SynchLocalBAL.GetLocalUser("1");

                    foreach (DataRow dtDtxRow in dtDentrixUser.Rows)
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

                    dtDentrixUser.AcceptChanges();

                    if (dtDentrixUser != null && dtDentrixUser.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_Users_Dentrix_To_Local(dtDentrixUser);
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

        #region Synch Holidays

        private void fncSynchDataDentrix_Holidays()
        {
            //  SynchDataDentrix_Holidays();
            InitBgWorkerDentrix_Holidays();
            InitBgTimerDentrix_Holidays();
        }

        private void InitBgTimerDentrix_Holidays()
        {
            timerSynchDentrix_Holidays = new System.Timers.Timer();
            this.timerSynchDentrix_Holidays.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchDentrix_Holidays.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_Holidays_Tick);
            timerSynchDentrix_Holidays.Enabled = true;
            timerSynchDentrix_Holidays.Start();
            timerSynchDentrix_Holidays_Tick(null, null);
        }

        private void InitBgWorkerDentrix_Holidays()
        {
            bwSynchDentrix_Holidays = new BackgroundWorker();
            bwSynchDentrix_Holidays.WorkerReportsProgress = true;
            bwSynchDentrix_Holidays.WorkerSupportsCancellation = true;
            bwSynchDentrix_Holidays.DoWork += new DoWorkEventHandler(bwSynchDentrix_Holidays_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchDentrix_Holidays.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_Holidays_RunWorkerCompleted);
        }

        private void timerSynchDentrix_Holidays_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_Holidays.Enabled = false;
                MethodForCallSynchOrderDentrix_Holidays();
            }
        }

        public void MethodForCallSynchOrderDentrix_Holidays()
        {
            System.Threading.Thread procThreadmainDentrix_Holidays = new System.Threading.Thread(this.CallSyncOrderTableDentrix_Holidays);
            procThreadmainDentrix_Holidays.Start();
        }

        public void CallSyncOrderTableDentrix_Holidays()
        {
            if (bwSynchDentrix_Holidays.IsBusy != true)
            {
                bwSynchDentrix_Holidays.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_Holidays_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_Holidays.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_Holidays();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_Holidays_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_Holidays.Enabled = true;
        }

        private void SynchDataDentrix_Holidays()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                #region Holidays
                try
                {
                    DataTable dtDentrixHoliday = SynchDentrixBAL.GetDentrixHolidaysData();
                    dtDentrixHoliday.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalHoliday = SynchLocalBAL.GetLocalDentrixHolidayData();

                    dtDentrixHoliday = CommonUtility.AddHolidays(dtDentrixHoliday, dtLocalHoliday, "Sched_exception_date", "practice_name", "H_EHR_ID");

                    foreach (DataRow dtDtxRow in dtDentrixHoliday.Rows)
                    {
                        DataRow[] row = dtLocalHoliday.Copy().Select("SchedDate = '" + dtDtxRow["Sched_exception_date"] + "'");
                        if (row.Length > 0)
                        {
                            if (Convert.ToString(dtDtxRow["practice_name"].ToString().Trim()) != Convert.ToString(row[0]["comment"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToString(dtDtxRow["start_time1"].ToString().Trim()) != Convert.ToString(row[0]["StartTime_1"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToString(dtDtxRow["start_time2"].ToString().Trim()) != Convert.ToString(row[0]["StartTime_2"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToString(dtDtxRow["start_time3"].ToString().Trim()) != Convert.ToString(row[0]["StartTime_3"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToString(dtDtxRow["end_time1"].ToString().Trim()) != Convert.ToString(row[0]["EndTime_1"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToString(dtDtxRow["end_time2"].ToString().Trim()) != Convert.ToString(row[0]["EndTime_2"]))
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToString(dtDtxRow["end_time3"].ToString().Trim()) != Convert.ToString(row[0]["EndTime_3"]))
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
                        DataRow[] row = dtDentrixHoliday.Copy().Select("Sched_exception_date = '" + dtDtxRow["SchedDate"] + "'");
                        if (row.Length <= 0)
                        {
                            DataRow ApptDtldr = dtDentrixHoliday.NewRow();
                            ApptDtldr["Sched_exception_date"] = dtDtxRow["SchedDate"].ToString().Trim();
                            ApptDtldr["InsUptDlt"] = 3;
                            dtDentrixHoliday.Rows.Add(ApptDtldr);
                        }
                    }
                    dtDentrixHoliday.AcceptChanges();
                    if (dtDentrixHoliday != null && dtDentrixHoliday.Rows.Count > 0)
                    {
                        //dtDentrixHoliday = CommonUtility.CreateHolidayEHRId(dtDentrixHoliday);
                        bool status = SynchDentrixBAL.Save_Holidays_Dentrix_To_Local(dtDentrixHoliday);
                        if (status)
                        {
                            SynchDataLiveDB_Push_Holiday();
                        }
                    }
                    else
                    {
                        bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday_Push");
                    }
                    ObjGoalBase.WriteToSyncLogFile("Holiday Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
                #endregion

                #region OperatoryHolidays

                //if (IsDentrixOperatorySync)
                //{
                //    try
                //    {
                //        DataTable dtDentrixOperatory = SynchDentrixBAL.GetDentrixOperatoryData();
                //        DataTable dtDentrixHoliday = SynchDentrixBAL.GetDentrixOperatoryHolidaysData(dtDentrixOperatory);
                //        dtDentrixHoliday.Columns.Add("InsUptDlt", typeof(int));
                //        DataTable dtLocalHoliday = SynchLocalBAL.GetLocalDentrixOperatoryHolidayData();

                //        foreach (DataRow dtDtxRow in dtDentrixHoliday.Rows)
                //        {
                //            DataRow[] row = dtLocalHoliday.Copy().Select("SchedDate = '" + dtDtxRow["Sched_exception_date"] + "' and H_Operatory_EHR_ID = '" + dtDtxRow["op_id"].ToString() + "'");
                //            if (row.Length > 0)
                //            {
                //                if (Convert.ToString(dtDtxRow["op_title"].ToString().Trim()) != Convert.ToString(row[0]["comment"]))
                //                {
                //                    dtDtxRow["InsUptDlt"] = 2;
                //                }
                //                else if (Convert.ToString(dtDtxRow["start_time1"].ToString().Trim()) != Convert.ToString(row[0]["StartTime_1"]))
                //                {
                //                    dtDtxRow["InsUptDlt"] = 2;
                //                }
                //                else if (Convert.ToString(dtDtxRow["start_time2"].ToString().Trim()) != Convert.ToString(row[0]["StartTime_2"]))
                //                {
                //                    dtDtxRow["InsUptDlt"] = 2;
                //                }
                //                else if (Convert.ToString(dtDtxRow["start_time3"].ToString().Trim()) != Convert.ToString(row[0]["StartTime_3"]))
                //                {
                //                    dtDtxRow["InsUptDlt"] = 2;
                //                }
                //                else if (Convert.ToString(dtDtxRow["end_time1"].ToString().Trim()) != Convert.ToString(row[0]["EndTime_1"]))
                //                {
                //                    dtDtxRow["InsUptDlt"] = 2;
                //                }
                //                else if (Convert.ToString(dtDtxRow["end_time2"].ToString().Trim()) != Convert.ToString(row[0]["EndTime_2"]))
                //                {
                //                    dtDtxRow["InsUptDlt"] = 2;
                //                }
                //                else if (Convert.ToString(dtDtxRow["end_time3"].ToString().Trim()) != Convert.ToString(row[0]["EndTime_3"]))
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

                //        foreach (DataRow dtDtxRow in dtLocalHoliday.Rows)
                //        {
                //            DataRow[] row = dtDentrixHoliday.Copy().Select("Sched_exception_date = '" + dtDtxRow["SchedDate"] + "' and op_id = '" + dtDtxRow["H_Operatory_EHR_ID"].ToString() + "'");
                //            if (row.Length <= 0)
                //            {
                //                DataRow ApptDtldr = dtDentrixHoliday.NewRow();
                //                ApptDtldr["Sched_exception_date"] = dtDtxRow["SchedDate"].ToString().Trim();
                //                ApptDtldr["op_id"] = dtDtxRow["H_Operatory_EHR_ID"].ToString().Trim();
                //                ApptDtldr["InsUptDlt"] = 3;
                //                dtDentrixHoliday.Rows.Add(ApptDtldr);
                //            }
                //        }
                //        dtDentrixHoliday.AcceptChanges();
                //        if (dtDentrixHoliday != null && dtDentrixHoliday.Rows.Count > 0)
                //        {
                //            dtDentrixHoliday = CommonUtility.CreateHolidayEHRId(dtDentrixHoliday);
                //            bool status = SynchDentrixBAL.Save_Opeatory_Holidays_Dentrix_To_Local(dtDentrixHoliday);
                //            if (status)
                //            {
                //                SynchDataLiveDB_Push_Holiday();
                //            }
                //        }
                //        else
                //        {
                //            bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday_Push");
                //        }
                //        ObjGoalBase.WriteToSyncLogFile("Operatory_Holiday Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                //        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");
                //    }

                //    catch (Exception ex)
                //    {
                //        ObjGoalBase.WriteToErrorLogFile("[Operatory_Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                //    }
                //}

                #endregion
            }
        }

        #endregion

        #region Create Appointment

        private void fncSynchDataLocalToDentrix_Appointment()
        {
            InitBgWorkerLocalToDentrix_Appointment();
            InitBgTimerLocalToDentrix_Appointment();
        }

        private void InitBgTimerLocalToDentrix_Appointment()
        {
            timerSynchLocalToDentrix_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToDentrix_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToDentrix_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToDentrix_Appointment_Tick);
            timerSynchLocalToDentrix_Appointment.Enabled = true;
            timerSynchLocalToDentrix_Appointment.Start();
            timerSynchLocalToDentrix_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToDentrix_Appointment()
        {
            bwSynchLocalToDentrix_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToDentrix_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToDentrix_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToDentrix_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToDentrix_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToDentrix_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToDentrix_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToDentrix_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToDentrix_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToDentrix_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToDentrix_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToDentrix_Appointment);
            procThreadmainLocalToDentrix_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToDentrix_Appointment()
        {
            if (bwSynchLocalToDentrix_Appointment.IsBusy != true)
            {
                bwSynchLocalToDentrix_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToDentrix_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToDentrix_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToDentrix_Appointment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToDentrix_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToDentrix_Appointment.Enabled = true;
        }

        public void SynchDataLocalToDentrix_Appointment()
        {
            try
            {
                //CheckEntryUserLoginIdExist();
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData("1");
                    DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixPatientID_NameData();
                    DataTable dtIdelProv = SynchDentrixBAL.GetDentrixIdelProvider();

                    string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                    string tmpApptProv = "";
                    Int64 tmpPatient_id = 0;
                    // Int64 tmpPatient_Gur_id = 0;
                    int tmpAppt_EHR_id = 0;
                    int tmpNewPatient = 1;

                    string tmpLastName = "";
                    string tmpFirstName = "";

                    string TmpWebPatientName = "";
                    string TmpWebRevPatientName = "";

                    string PatientName = "";
                    //ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix Stage 1 Loop");
                    if (dtWebAppointment != null)
                    {
                        if (dtWebAppointment.Rows.Count > 0)
                        {
                            Utility.CheckEntryUserLoginIdExist();
                        }
                    }

                    foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                    {
                        PatientName = string.Empty;

                        string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
                        DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                        DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                        DateTime ApptDateTime = DateTime.Now;
                        DateTime ApptEndDateTime = DateTime.Now;
                        string Apptdate = "";
                        string ApptTime = "";
                        TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                        int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);

                        DataTable dtBookOperatoryApptWiseDateTime = SynchDentrixBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
                        string tmpIdealOperatory = "";
                        string appointment_EHR_id = "";
                        //ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix" + dtDtxRow["First_Name"].ToString().Trim() +' ' + dtDtxRow["Last_Name"].ToString().Trim());
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
                                        //Apptdate = Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).ToString("dd/MM/yyyy");
                                        Apptdate = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).ToString("dd/MM/yyyy") : Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).ToShortDateString().ToString();
                                        ApptTime = Convert.ToInt32(rowBookOpTime[Bop]["start_hour"].ToString()).ToString("00") + ":" + Convert.ToInt32(rowBookOpTime[Bop]["start_minute"].ToString()).ToString("00");

                                        //ApptDateTime = DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                        ApptDateTime = !Utility.NotAllowToChangeSystemDateFormat ? DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : Convert.ToDateTime(Apptdate.ToString() + " " + ApptTime);//, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                        ApptEndDateTime = ApptDateTime.AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString().Trim()));

                                        appointment_EHR_id = rowBookOpTime[Bop]["appointment_id"].ToString();

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
                            tmpIdealOperatory = Operatory_EHR_IDs[0].ToString();
                        }
                        //ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix DoubleBooking Save");
                        if (tmpIdealOperatory == "")
                        {
                            DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("appointment_id = " + appointment_EHR_id).CopyToDataTable();
                            bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());

                        }
                        else
                        {

                            tmpPatient_id = 0;
                            // tmpPatient_Gur_id = 0;
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
                            if (dtDtxRow["Patient_EHR_Id"] != null && dtDtxRow["Patient_EHR_Id"].ToString() != string.Empty)
                            {
                                tmpPatient_id = Convert.ToInt64(dtDtxRow["Patient_EHR_Id"].ToString());
                            }

                            if (tmpPatient_id == 0)
                            {
                                //ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix Check For Patient Exists");
                                tmpPatient_id = Convert.ToInt32(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDentrixPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DBConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                                #region code commented by shruti(new patient ststus logic based on(dob,mobile,firstname,lastname) )
                                //DataRow[] row = dtDentrixPatient.Copy().Select("Mobile = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR Home_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' OR Work_Phone = '" + dtDtxRow["Mobile_Contact"].ToString().Trim() + "' ");
                                //if (row.Length > 0)
                                //{
                                //    for (int i = 0; i < row.Length; i++)
                                //    {
                                //        if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebPatientName.ToString().ToLower().Trim())
                                //        {
                                //            tmpPatient_id = Convert.ToInt32(row[i]["Patient_EHR_ID"].ToString());
                                //        }
                                //        else if (row[i]["Patient_Name"].ToString().ToLower().Trim() == TmpWebRevPatientName.ToString().ToLower().Trim())
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
                                //        if (tmpPatient_id != 0)
                                //        {
                                //            break;
                                //        }
                                //    }
                                //    tmpPatient_Gur_id = Convert.ToInt32(row[0]["Guarantor"].ToString());
                                //}
                                //if (dtDtxRow["Last_Name"].ToString().Trim() == null || dtDtxRow["Last_Name"].ToString().Trim() == "")
                                //{
                                //    //string[] tmpPatientName = dtDtxRow["First_Name"].ToString().Trim().Split(' ');

                                //    string tmpPatientName = dtDtxRow["First_Name"].ToString().Trim();
                                //    var firstSpaceIndex = tmpPatientName.IndexOf(" ");
                                //    if (tmpPatientName.Contains(" "))
                                //    {
                                //        tmpFirstName = tmpPatientName.Substring(0, firstSpaceIndex).ToString().Trim();
                                //        tmpLastName = tmpPatientName.Substring(firstSpaceIndex + 1).ToString().Trim();
                                //    }
                                //    else
                                //    {
                                //        tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                                //        tmpLastName = "Na";
                                //    }
                                //}
                                //else
                                //{
                                //    tmpLastName = dtDtxRow["Last_Name"].ToString().Trim();
                                //    tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                                //}
                                //PatientName = tmpLastName + ", " + tmpFirstName;
                                //// ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix Check PatientId " + tmpPatient_id.ToString());
                                //if (tmpPatient_id == 0)
                                //{
                                //    //if (dtDtxRow["Last_Name"].ToString().Trim() == null || dtDtxRow["Last_Name"].ToString().Trim() == "")
                                //    //{
                                //    //    //string[] tmpPatientName = dtDtxRow["First_Name"].ToString().Trim().Split(' ');

                                //    //    string tmpPatientName = dtDtxRow["First_Name"].ToString().Trim();
                                //    //    var firstSpaceIndex = tmpPatientName.IndexOf(" ");
                                //    //    if (tmpPatientName.Contains(" "))
                                //    //    {
                                //    //        tmpFirstName = tmpPatientName.Substring(0, firstSpaceIndex).ToString().Trim();
                                //    //        tmpLastName = tmpPatientName.Substring(firstSpaceIndex + 1).ToString().Trim();
                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                                //    //        tmpLastName = "Na";
                                //    //    }
                                //    //}
                                //    //else
                                //    //{
                                //    //    tmpLastName = dtDtxRow["Last_Name"].ToString().Trim();
                                //    //    tmpFirstName = dtDtxRow["First_Name"].ToString().Trim();
                                //    //}
                                //    //PatientName = tmpLastName + ", " + tmpFirstName;
                                //    //ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix Save Patient " + tmpFirstName + " " + dtDtxRow["Mobile_Contact"].ToString());


                                //    tmpPatient_id = SynchDentrixBAL.Save_Patient_Local_To_Dentrix(tmpLastName.Trim(),
                                //                                                                        tmpFirstName,
                                //                                                                        dtDtxRow["MI"].ToString().Trim(),
                                //                                                                        dtDtxRow["Mobile_Contact"].ToString().Trim(),
                                //                                                                        dtDtxRow["Email"].ToString().Trim(),
                                //                                                                        tmpApptProv,
                                //                                                                        dtDtxRow["Appt_DateTime"].ToString().Trim(),
                                //                                                                        tmpPatient_Gur_id,
                                //                                                                        dtDtxRow["Birth_Date"].ToString().Trim());


                                //    // ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix Patient Successfully created " + tmpFirstName + " " + dtDtxRow["Mobile_Contact"].ToString());
                                //}

                                #endregion

                            }

                            if (tmpPatient_id != 0)
                            {
                                #region Old Code
                                //string PatientName = tmpLastName + ", " + tmpFirstName;
                                //string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
                                //DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                                //DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                                //TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                                //int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);

                                // string tmpApptDurPatern = "";
                                //for (int i = 0; i < tmpApptDurMinutes / 5; i++)
                                //{
                                //    tmpApptDurPatern = tmpApptDurPatern + "X";
                                //}

                                //DataTable dtBookOperatoryApptWiseDateTime = SynchDentrixBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
                                //string tmpIdealOperatory = "";
                                //if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
                                //{
                                //    string tmpCheckOpId = "";
                                //    bool IsConflict = false;
                                //    for (int i = 0; i < Operatory_EHR_IDs.Length; i++)
                                //    {
                                //        IsConflict = false;
                                //        tmpCheckOpId = Operatory_EHR_IDs[i].ToString();
                                //        DataRow[] rowBookOpTime = dtBookOperatoryApptWiseDateTime.Copy().Select("operatory_id = '" + tmpCheckOpId + "'");
                                //        if (rowBookOpTime.Length > 0)
                                //        {
                                //            for (int Bop = 0; Bop < rowBookOpTime.Length; Bop++)
                                //            {
                                //                if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()))
                                //                    && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                //                {
                                //                    IsConflict = true;
                                //                    break;
                                //                }
                                //                else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()))
                                //                    && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                //                {
                                //                    IsConflict = true;
                                //                    break;
                                //                }
                                //            }
                                //        }
                                //        if (IsConflict == false)
                                //        {
                                //            tmpIdealOperatory = tmpCheckOpId;
                                //            break;
                                //        }
                                //    }
                                //}

                                //if (tmpIdealOperatory == "")
                                //{
                                //    tmpIdealOperatory = Operatory_EHR_IDs[0].ToString();
                                //}
                                #endregion

                                PatientName = dtDtxRow["Last_Name"].ToString().Trim() + "," + dtDtxRow["First_Name"].ToString().Trim(); //SynchDentrixBAL.GetPatientName(tmpPatient_id);

                                tmpAppt_EHR_id = SynchDentrixBAL.Save_Appointment_Local_To_Dentrix(tmpPatient_id.ToString(), (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "0"), tmpApptDurMinutes, tmpIdealOperatory.ToString(), tmpApptProv,
                                    dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDtxRow["ApptType_EHR_ID"].ToString().Trim(), PatientName, dtDtxRow["comment"].ToString().Trim(), (dtDtxRow["appt_treatmentcode"].ToString()));

                                if (tmpAppt_EHR_id > 0)
                                {
                                    bool isApptId_Update = SynchDentrixBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                                }
                            }
                        }
                    }
                    // SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook();
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

        public void SynchDataLocalToDentrix_Patient_Document()
        {

            try
            {
                //if (!Is_synched_PatinetForm && Utility.AditLocationSyncEnable)
                //{
                if (Utility.IsApplicationIdleTimeOff)
                {
                    Is_synched_PatinetForm = true;

                    if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower() && IsDocumentUpload == false)
                    {
                        try
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

                            IsDocumentUpload = true;
                            GetPatientDocument("1");
                            GetPatientDocument_New("1");
                            GoalBase.GetConnectionStringforDoc(false);
                            if (Utility.DentrixDocPWD != null && Utility.DentrixDocPWD != "")
                            {
                                if (Utility.DentrixDocConnString != null && Utility.DentrixDocConnString != "")
                                {
                                    SynchDentrixBAL.Save_Document_in_Dentrix();
                                }
                            }
                            ObjGoalBase.WriteToSyncLogFile("Patient_Document Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                            IsDocumentUpload = false;
                        }
                        catch (Exception ex)
                        {
                            IsDocumentUpload = false;
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }
                    }
                    if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower() && IsDocumentTreatmentUpload == false)
                    {
                        //Treatment Document
                        #region Treatment Document
                        try
                        {
                            IsDocumentTreatmentUpload = true;
                            //GoalBase.GetConnectionStringforDoc(false);
                            //if (Utility.DentrixDocPWD != null && Utility.DentrixDocPWD != "")
                            //{
                            SynchDentrixBAL.Save_Treatment_Document_in_Dentrix();
                            //}
                            ObjGoalBase.WriteToSyncLogFile("Patient_Treatment_Document Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                            IsDocumentTreatmentUpload = false;
                        }
                        catch (Exception ex)
                        {
                            IsDocumentTreatmentUpload = false;
                            ObjGoalBase.WriteToErrorLogFile("[Patient_Treatment_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
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
                        #endregion

                        #region Insurance Carrier
                        try
                        {
                            IsDocumentInsuranceCarrierUpload = true;
                            
                            SynchDentrixBAL.Save_InsuranceCarrier_Document_in_Dentrix();
                          
                            ObjGoalBase.WriteToSyncLogFile("Patient_InsuranceCarrier_Document Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                            IsDocumentInsuranceCarrierUpload = false;
                        }
                        catch (Exception ex)
                        {
                            IsDocumentInsuranceCarrierUpload = false;
                            ObjGoalBase.WriteToErrorLogFile("[Patient_InsuranceCarrier_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }
                        try
                        {
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
                            ObjGoalBase.WriteToErrorLogFile("[InsuranceCarrier Document Error log] : " + ex.Message);
                            // throw;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatinetForm = false;
                if (!ex.Message.Contains("Key value already exists in index 9"))
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
            }


        }

        #endregion

        #region Patient Form

        private void fncSynchDataLocalToDentrix_Patient_Form()
        {
            InitBgWorkerLocalToDentrix_Patient_Form();
            InitBgTimerLocalToDentrix_Patient_Form();
        }

        private void InitBgTimerLocalToDentrix_Patient_Form()
        {
            timerSynchLocalToDentrix_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToDentrix_Patient_Form.Interval = 1000 * GoalBase.intervalEHRSynch_PatientForm;
            this.timerSynchLocalToDentrix_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToDentrix_Patient_Form_Tick);
            timerSynchLocalToDentrix_Patient_Form.Enabled = true;
            timerSynchLocalToDentrix_Patient_Form.Start();
            timerSynchLocalToDentrix_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToDentrix_Patient_Form()
        {
            bwSynchLocalToDentrix_Patient_Form = new BackgroundWorker();
            bwSynchLocalToDentrix_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToDentrix_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToDentrix_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToDentrix_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToDentrix_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToDentrix_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToDentrix_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToDentrix_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToDentrix_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToDentrix_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToDentrix_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToDentrix_Patient_Form);
            procThreadmainLocalToDentrix_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToDentrix_Patient_Form()
        {
            if (bwSynchLocalToDentrix_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToDentrix_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToDentrix_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToDentrix_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToDentrix_Patient_Form();
                lastPatientFormcalled = DateTime.Now;
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchLocalToDentrix_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToDentrix_Patient_Form.Enabled = true;
        }

        public void SynchDataLocalToDentrix_Patient_Form()
        {

            try
            {
                GoalBase.WriteToPaymentLogFromAll_Static("Patient Form call");
                Is_synched_PatinetForm = false;
                if (!Is_synched_PatinetForm && Utility.AditLocationSyncEnable)
                {
                    if (Utility.IsApplicationIdleTimeOff)
                    {
                        Is_synched_PatinetForm = true;
                        SynchDataLiveDB_Pull_PatientForm(); //Changed for test
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
                        DataTable dtWebPatient_Form = SynchLocalBAL.GetLocalNewWebPatient_FormData("1");


                        dtWebPatient_Form.Columns.Add("TableName", typeof(string)); // Added for Pat Form Task
                        dtWebPatient_Form.Columns["TableName"].DefaultValue = "patient"; // Added for Pat Form Task


                        if (dtWebPatient_Form != null)
                        {
                            if (dtWebPatient_Form.Rows.Count > 0)
                            {
                                Utility.CheckEntryUserLoginIdExist();
                            }
                        }

                        foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                        {

                            if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "first_name")
                            {
                                dtDtxRow["ehrfield"] = "firstname";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "last_name")
                            {
                                dtDtxRow["ehrfield"] = "lastname";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "mobile")
                            {
                                dtDtxRow["ehrfield"] = "pager";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "birth_date")
                            {
                                dtDtxRow["ehrfield"] = "birthdate";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "email")
                            {
                                dtDtxRow["ehrfield"] = "emailaddr";
                            }
                            if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "first_name")
                            {
                                dtDtxRow["ehrfield"] = "status";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "marital_status")
                            {
                                dtDtxRow["ehrfield"] = "fampos";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "SINGLE")
                                {
                                    dtDtxRow["ehrfield_value"] = 1;
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MARRIED")
                                {
                                    dtDtxRow["ehrfield_value"] = 2;
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "CHILD")
                                {
                                    dtDtxRow["ehrfield_value"] = 3;
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "OTHER")
                                {
                                    dtDtxRow["ehrfield_value"] = 4;
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = 0;
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "middle_name")
                            {
                                dtDtxRow["ehrfield"] = "mi";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "preferred_name")
                            {
                                dtDtxRow["ehrfield"] = "prefname";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sex")
                            {
                                dtDtxRow["ehrfield"] = "gender";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MALE")
                                {
                                    dtDtxRow["ehrfield_value"] = 1;
                                }
                                else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                                {
                                    dtDtxRow["ehrfield_value"] = 2;
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = 1;
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "work_phone")
                            {
                                dtDtxRow["ehrfield"] = "workphone";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "pri_provider_id")
                            {
                                dtDtxRow["ehrfield"] = "provid1";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sec_provider_id")
                            {
                                dtDtxRow["ehrfield"] = "provid2";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "salutation")
                            {
                                dtDtxRow["ehrfield"] = "Salutation";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance")
                            {
                                dtDtxRow["ehrfield"] = "";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance_companyname")
                            {
                                dtDtxRow["ehrfield"] = "primary_insurance_companyname";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "receive_email")
                            {
                                dtDtxRow["ehrfield"] = "privacyflags";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "YES" || dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "")
                                {
                                    dtDtxRow["ehrfield_value"] = 0;
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = 2;
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "receive_sms")
                            {
                                dtDtxRow["ehrfield"] = "privacyflags";
                                if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "YES" || dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "")
                                {
                                    dtDtxRow["ehrfield_value"] = 0;
                                }
                                else
                                {
                                    dtDtxRow["ehrfield_value"] = 2;
                                }
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance")
                            {
                                dtDtxRow["ehrfield"] = "";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance_companyname")
                            {
                                dtDtxRow["ehrfield"] = "secondary_insurance_companyname";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "zipcode")
                            {
                                dtDtxRow["ehrfield"] = "zipcode";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_one")
                            {
                                dtDtxRow["ehrfield"] = "street1";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_two")
                            {
                                dtDtxRow["ehrfield"] = "street2";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "city")
                            {
                                dtDtxRow["ehrfield"] = "city";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "state")
                            {
                                dtDtxRow["ehrfield"] = "state";
                            }
                            else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "home_phone")
                            {
                                dtDtxRow["ehrfield"] = "phone";
                            }

                            #region Patient Form New EHR Fields

                            else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "ssn"))
                            {
                                dtDtxRow["ehrfield"] = "ssn";
                            }
                            else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "driverlicense"))
                            {
                                dtDtxRow["ehrfield"] = "driverslicense";
                            }
                            else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "school"))
                            {
                                dtDtxRow["TableName"] = "claiminfo";
                                dtDtxRow["ehrfield"] = "school";
                            }
                            else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "employer"))
                            {
                                dtDtxRow["ehrfield"] = "name";
                                dtDtxRow["TableName"] = "employer";
                            }

                            #endregion


                            dtWebPatient_Form.AcceptChanges();
                        }
                        if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                        {
                            bool Is_Record_Update = SynchDentrixBAL.Save_Patient_Form_Local_To_Dentrix(dtWebPatient_Form);
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

                        Is_synched_PatinetForm = false;
                        try
                        {
                            GetMedicalDentrixHistoryRecords();
                            SynchDentrixBAL.SaveMedicalHistoryLocalToDentrix();
                            ObjGoalBase.WriteToSyncLogFile("Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }
                        if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower())
                        {
                            try
                            {
                                SynchDentrixBAL.SaveDiseaseLocalToDentrix();
                            }
                            catch (Exception ex)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Disease Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }
                            try
                            {
                                SynchDentrixBAL.DeleteDiseaseLocalToDentrix();
                            }
                            catch (Exception ex)
                            {
                                ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Disease Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }
                        }

                        bool isRecordSaved = false, isRecordDeleted = false;
                        string Patient_EHR_IDS = "";
                        string DeletePatientEHRID = "";
                        string SavePatientEHRID = "";
                        try
                        {
                            SynchDentrixBAL.DeleteMedicationLocalToDentrix(ref isRecordDeleted, ref DeletePatientEHRID);
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }
                        try
                        {
                            SynchDentrixBAL.SaveMedicationLocalToDentrix(ref isRecordSaved, ref SavePatientEHRID);
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToErrorLogFile("[Save_Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        }
                        if (isRecordSaved || isRecordDeleted)
                        {
                            Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                            if (Patient_EHR_IDS != "")
                            {
                                SynchDataDentrix_PatientMedication(Patient_EHR_IDS);
                            }
                        }

                        if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower() && IsDocumentUpload == false)
                        {
                            try
                            {
                                IsDocumentUpload = true;
                                GetPatientDocument("1");
                                GetPatientDocument_New("1");
                                GoalBase.GetConnectionStringforDoc(false);
                                if (Utility.DentrixDocPWD != null && Utility.DentrixDocPWD != "")
                                {
                                    if (Utility.DentrixDocConnString != null && Utility.DentrixDocConnString != "")
                                    {
                                        SynchDentrixBAL.Save_Document_in_Dentrix();
                                    }
                                }
                                ObjGoalBase.WriteToSyncLogFile("Patient_Document Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                                IsDocumentUpload = false;
                            }
                            catch (Exception ex)
                            {
                                IsDocumentUpload = false;
                                ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                            }
                        }
                        //if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower() && IsDocumentTreatmentUpload == false)
                        //{
                        //    //Treatment Document
                        //    try
                        //    {
                        //        IsDocumentTreatmentUpload = true;
                        //        //GoalBase.GetConnectionStringforDoc(false);
                        //        //if (Utility.DentrixDocPWD != null && Utility.DentrixDocPWD != "")
                        //        //{
                        //        SynchDentrixBAL.Save_Treatment_Document_in_Dentrix();
                        //        //}
                        //        ObjGoalBase.WriteToSyncLogFile("Patient_Treatment_Document Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        //        IsDocumentTreatmentUpload = false;
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        IsDocumentTreatmentUpload = false;
                        //        ObjGoalBase.WriteToErrorLogFile("[Patient_Treatment_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        //    }
                        //    try
                        //    {
                        //        #region change status as treatment doc impotred Completed
                        //        DataTable statusCompleted = SynchLocalBAL.ChangeStatusForTreatmentDoc("Completed");
                        //        if (statusCompleted.Rows.Count > 0)
                        //        {
                        //            Change_Status_TreatmentDoc(statusCompleted, "Completed");
                        //        }
                        //        #endregion
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        ObjGoalBase.WriteToErrorLogFile("[Treatment Document Error log] : " + ex.Message);
                        //        // throw;
                        //    }
                        //}
                        SynchDataLocalToDentrix_Patient_Document();
                    }
                }
            }
            catch (Exception ex)
            {
                Is_synched_PatinetForm = false;
                if (!ex.Message.Contains("Key value already exists in index 9"))
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
            }

        }
        #region PatientPayment
        private void fncSynchDataDentrix_PatientPayment()
        {
            InitBgWorkerDentrix_PatientPayment();
            InitBgTimerDentrix_PatientPayment();
        }

        private void InitBgTimerDentrix_PatientPayment()
        {
            timerSynchDentrix_PatientPayment = new System.Timers.Timer();
            this.timerSynchDentrix_PatientPayment.Interval = 1000 * GoalBase.intervalEHRSynch_PatientPayment;
            this.timerSynchDentrix_PatientPayment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_PatientPayment_Tick);
            timerSynchDentrix_PatientPayment.Enabled = true;
            timerSynchDentrix_PatientPayment.Start();
        }

        private void InitBgWorkerDentrix_PatientPayment()
        {
            bwSynchDentrix_PatientPayment = new BackgroundWorker();
            bwSynchDentrix_PatientPayment.WorkerReportsProgress = true;
            bwSynchDentrix_PatientPayment.WorkerSupportsCancellation = true;
            bwSynchDentrix_PatientPayment.DoWork += new DoWorkEventHandler(bwSynchDentrix_PatientPayment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchDentrix_PatientPayment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_PatientPayment_RunWorkerCompleted);
        }

        private void timerSynchDentrix_PatientPayment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_PatientPayment.Enabled = false;
                MethodForCallSynchOrderDentrix_PatientPayment();
            }
        }

        public void MethodForCallSynchOrderDentrix_PatientPayment()
        {
            System.Threading.Thread procThreadmainDentrix_PatientPayment = new System.Threading.Thread(this.CallSyncOrderTableDentrix_PatientPayment);
            procThreadmainDentrix_PatientPayment.Start();
        }

        public void CallSyncOrderTableDentrix_PatientPayment()
        {
            if (bwSynchDentrix_PatientPayment.IsBusy != true)
            {
                bwSynchDentrix_PatientPayment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_PatientPayment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_PatientPayment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToDentrix_Patient_Payment();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_PatientPayment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_PatientPayment.Enabled = true;
        }

        public void SynchDataLocalToDentrix_Patient_Payment()
        {
            try
            {
                if (lastPatientFormcalled.AddMinutes(10) < DateTime.Now)
                {
                    SynchDataLocalToDentrix_Patient_Form();
                }
                if (!IsPaymentSyncing)
                {
                    IsPaymentSyncing = true;
                    ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync Start");
                    if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync call pull");
                        Int64 TransactionHeaderId = 0;
                        string noteId = "";
                        SynchDataLiveDB_Pull_PatientPaymentLog();
                        // CheckEntryUserLoginIdExist();
                        DataTable dtPatientPayment = new DataTable();

                        DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData("1");

                        #region Call API for EHR Entry Done
                        //  dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData("1");
                        if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                        {
                            SynchDentrixBAL.SavePatientPaymentTOEHR(dtWebPatientPayment);
                            //SynchDentrixBAL.Save_PatientPaymentLog_LocalToDentrix(dtWebPatientPayment);
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient Payment Log");
                            ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                        }

                        #endregion

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

        #endregion

        public void SynchDataLocalToDentrix_Patient_SMSCallLog()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    Int64 TransactionHeaderId = 0;
                    string noteId = "";
                    //CheckEntryUserLoginIdExist();
                    DataTable dtPatientPayment = new DataTable();
                    SynchDentrixBAL.DeleteDuplicatePatientLog();
                    DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientSMSCallLogData("1");

                    #region Call API for EHR Entry Done
                    if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                    {
                        SynchDentrixBAL.Save_PatientSMSCallLog_LocalToDentrix(dtWebPatientPayment);


                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient SMSCall Log");
                        ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Records Not Available.");
                    }

                    #endregion


                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Patient_SMSCallLog Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }



        #region PF Medicle History

        private void fncSynchDataDentrix_MedicalHistory()
        {
            InitBgWorkerDentrix_MedicalHistory();
            InitBgTimerDentrix_MedicalHistory();
        }

        private void InitBgTimerDentrix_MedicalHistory()
        {
            timerSynchDentrix_MedicalHistory = new System.Timers.Timer();
            this.timerSynchDentrix_MedicalHistory.Interval = 1000 * GoalBase.intervalEHRSynch_MedicalHistory;
            this.timerSynchDentrix_MedicalHistory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_MedicalHistory_Tick);
            timerSynchDentrix_MedicalHistory.Enabled = true;
            timerSynchDentrix_MedicalHistory.Start();
            timerSynchDentrix_MedicalHistory_Tick(null, null);
        }

        private void InitBgWorkerDentrix_MedicalHistory()
        {
            bwSynchDentrix_MedicalHistory = new BackgroundWorker();
            bwSynchDentrix_MedicalHistory.WorkerReportsProgress = true;
            bwSynchDentrix_MedicalHistory.WorkerSupportsCancellation = true;
            bwSynchDentrix_MedicalHistory.DoWork += new DoWorkEventHandler(bwSynchDentrix_MedicalHistory_DoWork);
            bwSynchDentrix_MedicalHistory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_MedicalHistory_RunWorkerCompleted);
        }

        private void timerSynchDentrix_MedicalHistory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_MedicalHistory.Enabled = false;
                MethodForCallSynchOrderDentrix_MedicalHistory();
            }
        }

        public void MethodForCallSynchOrderDentrix_MedicalHistory()
        {
            System.Threading.Thread procThreadmainDentrix_MedicalHistory = new System.Threading.Thread(this.CallSyncOrderTableDentrix_MedicalHistory);
            procThreadmainDentrix_MedicalHistory.Start();
        }

        public void CallSyncOrderTableDentrix_MedicalHistory()
        {
            if (bwSynchDentrix_MedicalHistory.IsBusy != true)
            {
                bwSynchDentrix_MedicalHistory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_MedicalHistory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_MedicalHistory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_MedicalHistory();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_MedicalHistory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_MedicalHistory.Enabled = true;
        }

        public void SynchDataDentrix_MedicalHistory()
        {
            SynchDataDentrix_MedicleHistory();

        }

        public void SynchDataDentrix_MedicleHistory()
        {
            SynchDataDentrix_MedicleFormData();

            SynchDataDentrix_MedicleFormQuestionData();
        }

        public void SynchDataDentrix_MedicleFormData()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    DataTable dtDentrixMedicleHistory = SynchDentrixBAL.GetDentrixMedicleFormData();
                    DataTable dtDentrixLocalMedicleHistory = SynchLocalBAL.GetDentrixLocalMedicleFormData();

                    dtDentrixMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                    dtDentrixMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtDentrixLocalMedicleHistory.Columns.Contains("InsUptDlt"))
                    {
                        dtDentrixLocalMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                        dtDentrixLocalMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtDentrixLocalMedicleHistory = CompareDataTableRecords(ref dtDentrixMedicleHistory, dtDentrixLocalMedicleHistory, "Dentrix_Form_EHRUnique_ID", "Dentrix_Form_LocalDB_ID", "Dentrix_Form_LocalDB_ID,Dentrix_Form_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtDentrixMedicleHistory.AcceptChanges();

                    if (dtDentrixMedicleHistory != null && dtDentrixMedicleHistory.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtDentrixMedicleHistory.Clone();
                        if (dtDentrixMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0 || dtDentrixLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtDentrixMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtDentrixMedicleHistory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtDentrixLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtDentrixLocalMedicleHistory.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Dentrix_Form", "Dentrix_Form_LocalDB_ID,Dentrix_Form_Web_ID", "Dentrix_Form_LocalDB_ID");
                        }
                        else
                        {
                            if (dtDentrixMedicleHistory.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Dentrix_Form");
                            ObjGoalBase.WriteToSyncLogFile("Dentrix_Form Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        }
                        SynchDataLiveDB_Push_MedicalHisotryTables("Dentrix_Form");
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Dentrix_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        public void SynchDataDentrix_MedicleFormQuestionData()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {

                    DataTable dtDentrixMedicleHistory = SynchDentrixBAL.GetDentrixMedicleFormQuestionData();
                    DataTable dtDentrixLocalMedicleHistory = SynchLocalBAL.GetDentrixLocalFormQuestionData();

                    dtDentrixMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                    dtDentrixMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtDentrixLocalMedicleHistory.Columns.Contains("InsUptDlt"))
                    {
                        dtDentrixLocalMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                        dtDentrixLocalMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;
                    }
                    if (dtDentrixMedicleHistory.Columns.Contains("InsUptDlt"))
                    {
                        dtDentrixMedicleHistory.Columns.Remove("questioninfo");
                    }

                    dtDentrixLocalMedicleHistory = CompareDataTableRecords(ref dtDentrixMedicleHistory, dtDentrixLocalMedicleHistory, "Dentrix_Question_EHRUnique_ID", "Dentrix_FormQuestion_LocalDB_ID", "Dentrix_FormQuestion_LocalDB_ID,Dentrix_FormQuestion_Web_ID,Is_Adit_Updated,EHR_Entry_DateTime,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Answer_Value,Service_Install_Id,QuestionVersion_Date");

                    dtDentrixMedicleHistory.AcceptChanges();

                    if (dtDentrixMedicleHistory != null && dtDentrixMedicleHistory.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtDentrixMedicleHistory.Clone();
                        if (dtDentrixMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0 || dtDentrixLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtDentrixMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtDentrixMedicleHistory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtDentrixLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtDentrixLocalMedicleHistory.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }

                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Dentrix_FormQuestion", "Dentrix_FormQuestion_LocalDB_ID,Dentrix_FormQuestion_Web_ID", "Dentrix_FormQuestion_LocalDB_ID");
                        }
                        else
                        {
                            if (dtDentrixMedicleHistory.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Dentrix_FormQuestion");
                            ObjGoalBase.WriteToSyncLogFile("Dentrix_FormQuestion Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        }
                        SynchDataLiveDB_Push_MedicalHisotryTables("Dentrix_FormQuestion");
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Dentrix_FormQuestion Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }


        #endregion

        #region PatientWiseRecallType

        //private void fncSynchDataDentrix_PatientWiseRecallDate()
        //{
        //    InitBgWorkerDentrix_PatientWiseRecallDate();
        //    InitBgTimerDentrix_PatientWiseRecallDate();
        //}

        //private void InitBgTimerDentrix_PatientWiseRecallDate()
        //{
        //    timerSynchDentrix_PatientWiseRecallDate = new System.Timers.Timer();
        //    this.timerSynchDentrix_PatientWiseRecallDate.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
        //    this.timerSynchDentrix_PatientWiseRecallDate.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_PatientWiseRecallDate_Tick);
        //    timerSynchDentrix_PatientWiseRecallDate.Enabled = true;
        //    timerSynchDentrix_PatientWiseRecallDate.Start();
        //    timerSynchDentrix_PatientWiseRecallDate_Tick(null, null);
        //}

        //private void InitBgWorkerDentrix_PatientWiseRecallDate()
        //{
        //    bwSynchDentrix_PatientWiseRecallDate = new BackgroundWorker();
        //    bwSynchDentrix_PatientWiseRecallDate.WorkerReportsProgress = true;
        //    bwSynchDentrix_PatientWiseRecallDate.WorkerSupportsCancellation = true;
        //    bwSynchDentrix_PatientWiseRecallDate.DoWork += new DoWorkEventHandler(bwSynchDentrix_PatientWiseRecallDate_DoWork);
        //    //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
        //    bwSynchDentrix_PatientWiseRecallDate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_PatientWiseRecallDate_RunWorkerCompleted);
        //}

        //private void timerSynchDentrix_PatientWiseRecallDate_Tick(object sender, EventArgs e)
        //{
        //    if (Utility.AditSync)
        //    {
        //        timerSynchDentrix_PatientWiseRecallDate.Enabled = false;
        //        MethodForCallSynchOrderDentrix_PatientWiseRecallDate();
        //    }
        //}

        //public void MethodForCallSynchOrderDentrix_PatientWiseRecallDate()
        //{
        //    System.Threading.Thread procThreadmainDentrix_PatientWiseRecallDate = new System.Threading.Thread(this.CallSyncOrderTableDentrix_PatientWiseRecallDate);
        //    procThreadmainDentrix_PatientWiseRecallDate.Start();
        //}

        //public void CallSyncOrderTableDentrix_PatientWiseRecallDate()
        //{
        //    if (bwSynchDentrix_PatientWiseRecallDate.IsBusy != true)
        //    {
        //        bwSynchDentrix_PatientWiseRecallDate.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        System.Threading.Thread.Sleep(100);
        //    }
        //}

        //private void bwSynchDentrix_PatientWiseRecallDate_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if ((bwSynchDentrix_PatientWiseRecallDate.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            return;
        //        }

        //        SynchDataDentrix_PatientWiseRecallDate();
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile(ex.Message);
        //    }
        //}

        //private void bwSynchDentrix_PatientWiseRecallDate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    timerSynchDentrix_PatientWiseRecallDate.Enabled = true;
        //}

        //public void SynchDataDentrix_PatientWiseRecallDate()
        //{
        //    if (Utility.IsApplicationIdleTimeOff)
        //    {
        //        // Is_synchedPatientWiseRecallType = true;

        //        try
        //        {
        //            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
        //            {
        //                DataTable dtDentrixPatientdue_date = SynchDentrixBAL.GetDentrixPatientdue_date_AllData();
        //                // dtEaglesoftRecallType.Columns.Add("InsUptDlt", typeof(int));
        //                DataTable dtLocalRecallType = SynchLocalBAL.GetLocalPatientWiseRecallTypeData(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                foreach (DataRow dtDtxRow in dtDentrixPatientdue_date.Rows)
        //                {
        //                    DataRow[] row = dtLocalRecallType.Copy().Select("Patient_Recall_Id = '" + dtDtxRow["Patient_Recall_Id"] + "'");
        //                    if (row.Length > 0)
        //                    {
        //                        if (dtDtxRow["RecallType_Name"].ToString().ToLower().Trim() != row[0]["RecallType_Name"].ToString().ToLower().Trim())
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 2;
        //                        }
        //                        else if (dtDtxRow["Default_Recall"].ToString().ToLower().Trim() != row[0]["Default_Recall"].ToString().ToLower().Trim())
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 2;
        //                        }
        //                        else if (dtDtxRow["Recall_Date"].ToString() != "" && row[0]["Recall_Date"].ToString() != "" && Convert.ToDateTime(dtDtxRow["Recall_Date"].ToString().Trim()) != Convert.ToDateTime(row[0]["Recall_Date"].ToString().Trim()))
        //                        {
        //                            dtDtxRow["InsUptDlt"] = 2;
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

        //                dtDentrixPatientdue_date.AcceptChanges();

        //                if (dtDentrixPatientdue_date != null && dtDentrixPatientdue_date.Rows.Count > 0)
        //                {
        //                    bool status = SynchEaglesoftBAL.SavePatientWiseRecallType_Eaglesoft_To_Local(dtDentrixPatientdue_date, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
        //                    if (status)
        //                    {
        //                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
        //                        ObjGoalBase.WriteToSyncLogFile("RecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                        //SynchDataLiveDB_PushPatientWiseRecallType();
        //                    }
        //                    else
        //                    {
        //                        ObjGoalBase.WriteToErrorLogFile("[RecallType Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString() + " to Local Database) ] Error.");
        //                    }
        //                }
        //            }
        //            //Is_synchedPatientWiseRecallType = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            //  Is_synchedPatientWiseRecallType = false;
        //            ObjGoalBase.WriteToErrorLogFile("[RecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //        }
        //    }

        //}

        #endregion

        #region Synch Insurance

        private void fncSynchDataDentrix_Insurance()
        {
            InitBgWorkerDentrix_Insurance();
            InitBgTimerDentrix_Insurance();
        }

        private void InitBgTimerDentrix_Insurance()
        {
            timerSynchDentrix_Insurance = new System.Timers.Timer();
            this.timerSynchDentrix_Insurance.Interval = 1000 * GoalBase.intervalEHRSynch_Insurance;
            this.timerSynchDentrix_Insurance.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchDentrix_Insurance_Tick);
            timerSynchDentrix_Insurance.Enabled = true;
            timerSynchDentrix_Insurance.Start();
        }

        private void InitBgWorkerDentrix_Insurance()
        {
            bwSynchDentrix_Insurance = new BackgroundWorker();
            bwSynchDentrix_Insurance.WorkerReportsProgress = true;
            bwSynchDentrix_Insurance.WorkerSupportsCancellation = true;
            bwSynchDentrix_Insurance.DoWork += new DoWorkEventHandler(bwSynchDentrix_Insurance_DoWork);
            bwSynchDentrix_Insurance.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchDentrix_Insurance_RunWorkerCompleted);
        }

        private void timerSynchDentrix_Insurance_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchDentrix_Insurance.Enabled = false;
                MethodForCallSynchOrderDentrix_Insurance();
            }
        }

        public void MethodForCallSynchOrderDentrix_Insurance()
        {
            System.Threading.Thread procThreadmainDentrix_Insurance = new System.Threading.Thread(this.CallSyncOrderTableDentrix_Insurance);
            procThreadmainDentrix_Insurance.Start();
        }

        public void CallSyncOrderTableDentrix_Insurance()
        {
            if (bwSynchDentrix_Insurance.IsBusy != true)
            {
                bwSynchDentrix_Insurance.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchDentrix_Insurance_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchDentrix_Insurance.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataDentrix_Insurance();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchDentrix_Insurance_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchDentrix_Insurance.Enabled = true;
        }

        public void SynchDataDentrix_Insurance()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtDentrixInsurance = SynchDentrixBAL.GetDentrixInsuranceData();
                    dtDentrixInsurance.Columns.Add("InsUptDlt", typeof(int));

                    dtDentrixInsurance.AcceptChanges();

                    DataTable dtLocalInsurance = SynchLocalBAL.GetLocalInsuranceData("1");

                    foreach (DataRow dtDtxRow in dtDentrixInsurance.Rows)
                    {
                        DataRow[] row = dtLocalInsurance.Copy().Select("Insurance_EHR_ID = '" + dtDtxRow["insid"] + "' ");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["insconame"].ToString().Trim() != row[0]["Insurance_Name"].ToString().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["street1"].ToString().ToLower().Trim() != row[0]["Address"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            //else if (dtDtxRow["address_2"].ToString().ToLower().Trim() != row[0]["Address2"].ToString().ToLower().Trim())
                            //{
                            //    dtDtxRow["InsUptDlt"] = 2;
                            //}3
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
                            else if (dtDtxRow["phone"].ToString().ToLower().Trim() != row[0]["Phone"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["payerid"].ToString().ToLower().Trim() != row[0]["ElectId"].ToString().ToLower().Trim())
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
                        DataRow[] row = dtDentrixInsurance.Copy().Select("insid = '" + dtDtxRow["Insurance_EHR_ID"] + "' ");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtDentrixInsurance.NewRow();
                            BlcOptDtldr["insid"] = dtDtxRow["Insurance_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["insconame"] = dtDtxRow["Insurance_Name"].ToString().Trim();
                            // BlcOptDtldr["Clinic_Number"] = dtDtxRow["Clinic_Number"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtDentrixInsurance.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtDentrixInsurance.AcceptChanges();
                    ObjGoalBase.WriteToErrorLogFile("Insurance Sync (" + Utility.Application_Name + " to Local Database) ."+ dtDentrixInsurance.Rows.Count.ToString());

                    if (dtDentrixInsurance != null && dtDentrixInsurance.Rows.Count > 0)
                    {
                        bool status = SynchDentrixBAL.Save_Insurance_Dentrix_To_Local(dtDentrixInsurance);
                        ObjGoalBase.WriteToErrorLogFile("Insurance Sync (" + Utility.Application_Name + " to Local Database) ." + status.ToString());
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
        #region Check BackUp Active

        private bool CheckBackUpActive()
        {

            #region Test Code

            //        //Backup Time ------------------------
            int BackUpHour, BackUpMinutes;
            bool IsActive;

            BackUpHour = 18;
            BackUpMinutes = 50;
            DateTime BackupTime = DateTime.Today.AddHours(BackUpHour).AddMinutes(BackUpMinutes);

            //        //Interval ---------------------------
            int IntervalBefore, IntervalAfter;

            IntervalBefore = -2; //minutes
            IntervalAfter = 10; //minutes

            //        //Backup Period -----------------------
            DateTime SynchStopTime, SynchResumeTime;

            SynchStopTime = BackupTime.AddMinutes(IntervalBefore);
            SynchResumeTime = BackupTime.AddMinutes(IntervalAfter);

            //        //-------------------------------------
            DateTime CurTime = DateTime.Now;

            ////if (CurTime >= StartTime && CurTime <= EndTime) return true; else return false;
            if (CurTime >= SynchStopTime && CurTime <= SynchResumeTime) IsActive = true; else IsActive = false;

            return IsActive;

            //if (RVBl == false)
            //    CallSynchDentrixToLocal();
            //else
            //    System.Windows.Forms.MessageBox.Show("Please wait... EHR is performing a BackUp");

            //break;

            #endregion

        }

        #endregion

        #region Event Listener
        public static bool SynchDataDentrix_AppointmentFromEvent(DataTable dtWebAppointment, string Clinic_Number, string Service_Install_Id)
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

                DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixPatientID_NameData();
                DataTable dtIdelProv = SynchDentrixBAL.GetDentrixIdelProvider();


                string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                string tmpApptProv = "";
                Int64 tmpPatient_id = 0;
                // Int64 tmpPatient_Gur_id = 0;
                int tmpAppt_EHR_id = 0;
                int tmpNewPatient = 1;

                string tmpLastName = "";
                string tmpFirstName = "";

                string TmpWebPatientName = "";
                string TmpWebRevPatientName = "";

                string PatientName = "";

                if (dtWebAppointment != null)
                {
                    if (dtWebAppointment.Rows.Count > 0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                    }
                }
                foreach (DataRow dtDtxRow in dtWebAppointment.Rows)
                {
                    PatientName = string.Empty;

                    string[] Operatory_EHR_IDs = dtDtxRow["Operatory_EHR_ID"].ToString().Trim().Split(';');
                    DateTime tmpStartTime = Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim());
                    DateTime tmpEndTime = Convert.ToDateTime(dtDtxRow["Appt_EndDateTime"].ToString().Trim());
                    DateTime ApptDateTime = DateTime.Now;
                    DateTime ApptEndDateTime = DateTime.Now;
                    string Apptdate = "";
                    string ApptTime = "";
                    TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                    int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);

                    DataTable dtBookOperatoryApptWiseDateTime = SynchDentrixBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
                    string tmpIdealOperatory = "";
                    string appointment_EHR_id = "";
                    //ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix" + dtDtxRow["First_Name"].ToString().Trim() +' ' + dtDtxRow["Last_Name"].ToString().Trim());
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
                                    //Apptdate = Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).ToString("dd/MM/yyyy");
                                    Apptdate = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).ToString("dd/MM/yyyy") : Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).ToShortDateString().ToString();
                                    ApptTime = Convert.ToInt32(rowBookOpTime[Bop]["start_hour"].ToString()).ToString("00") + ":" + Convert.ToInt32(rowBookOpTime[Bop]["start_minute"].ToString()).ToString("00");

                                    //ApptDateTime = DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                    ApptDateTime = !Utility.NotAllowToChangeSystemDateFormat ? DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : Convert.ToDateTime(Apptdate.ToString() + " " + ApptTime);//, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                    ApptEndDateTime = ApptDateTime.AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString().Trim()));

                                    appointment_EHR_id = rowBookOpTime[Bop]["appointment_id"].ToString();

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
                        tmpIdealOperatory = Operatory_EHR_IDs[0].ToString();
                    }
                    //ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix DoubleBooking Save");
                    if (tmpIdealOperatory == "")
                    {
                        DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("appointment_id = " + appointment_EHR_id).CopyToDataTable();
                        bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());

                    }
                    else
                    {

                        tmpPatient_id = 0;
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
                        if (dtDtxRow["Patient_EHR_Id"] != null && dtDtxRow["Patient_EHR_Id"].ToString() != string.Empty)
                        {
                            tmpPatient_id = Convert.ToInt64(dtDtxRow["Patient_EHR_Id"].ToString());
                        }

                        if (tmpPatient_id == 0)
                        {
                            //ObjGoalBase.WriteToSyncLogFile("Appointment Sync Local TO Dentrix Check For Patient Exists");
                            tmpPatient_id = Convert.ToInt32(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDentrixPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DBConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                        }

                        if (tmpPatient_id != 0)
                        {
                            PatientName = dtDtxRow["Last_Name"].ToString().Trim() + "," + dtDtxRow["First_Name"].ToString().Trim(); //SynchDentrixBAL.GetPatientName(tmpPatient_id);

                            tmpAppt_EHR_id = SynchDentrixBAL.Save_Appointment_Local_To_Dentrix(tmpPatient_id.ToString(), (dtDtxRow["Is_Appt"].ToString().ToLower() == "pa" ? dtDtxRow["appointment_status_ehr_key"].ToString() : "0"), tmpApptDurMinutes, tmpIdealOperatory.ToString(), tmpApptProv,
                                dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDtxRow["ApptType_EHR_ID"].ToString().Trim(), PatientName, dtDtxRow["comment"].ToString().Trim(), (dtDtxRow["appt_treatmentcode"].ToString()));

                            if (tmpAppt_EHR_id > 0)
                            {
                                bool isApptId_Update = SynchDentrixBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                            }
                        }
                        SyncDataDentrix_AppointmentFromEvent(dbConStr.ToString().Trim(), "0", "1", tmpAppt_EHR_id.ToString().Trim(), tmpPatient_id.ToString().Trim(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                    }
                }
                Utility.WritetoAditEventSyncLogFile_Static("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");

                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataDentrix_AppointmentFromEvent Sync (Local Database To Dentrix) ]" + ex.Message);
                return false;
            }
        }

        public static void SyncDataDentrix_AppointmentFromEvent(string strDbString, string Clinic_Number, string Service_Install_Id, string strApptID, string strPatID, string strWebID)
        {
            bool IS_Appt_Patient_Sync;
            bool Is_synched_Appointment;
            try
            {
                IS_Appt_Patient_Sync = SynchDataDentrix_AppointmentsPatientFromEvent(strPatID);
                if (IS_Appt_Patient_Sync)
                {
                    Is_synched_Appointment = false;
                    DataTable dtDentrixAppointment = SynchDentrixBAL.GetDentrixAppointmentData(strApptID);

                    dtDentrixAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                    dtDentrixAppointment.Columns.Add("InsUptDlt", typeof(int));
                    dtDentrixAppointment.Columns.Add("ProcedureDesc", typeof(string));
                    dtDentrixAppointment.Columns.Add("ProcedureCode", typeof(string));

                    string ProcedureDesc = "";
                    string ProcedureCode = "";
                    DataTable DtDentrixAppointment_Procedures_Data = SynchDentrixBAL.GetDentrixAppointment_Procedures_Data(strApptID);

                    DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData("1", strApptID);

                    string MobileEmail = string.Empty;
                    string Mobile_Contact = string.Empty;
                    string Email = string.Empty;
                    string AppointmentStatus = string.Empty;
                    string Home_Contact = string.Empty;
                    string Address = string.Empty;
                    string City = string.Empty;
                    string State = string.Empty;
                    string Zipcode = string.Empty;
                    int apptFlag = 0;

                    foreach (DataRow dtDtxRow in dtDentrixAppointment.Rows)
                    {
                        MobileEmail = string.Empty;
                        Mobile_Contact = string.Empty;
                        Email = string.Empty;
                        AppointmentStatus = string.Empty;
                        Home_Contact = string.Empty;
                        Address = string.Empty;
                        City = string.Empty;
                        State = string.Empty;
                        Zipcode = string.Empty;
                        apptFlag = 0;
                        if (dtDtxRow["patId"].ToString() == "0" || dtDtxRow["patId"].ToString() == string.Empty)
                        {
                            if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                            {
                                Mobile_Contact = Utility.ConvertContactNumber(dtDtxRow["Phone"].ToString().Trim());
                                Email = "";
                            }
                            else
                            {
                                MobileEmail = dtDtxRow["notetext"].ToString().Trim().Replace("-", "").Replace("(", "").Replace(")", "").Replace("\n", "").Trim();
                                Mobile_Contact = string.Empty;
                                Email = string.Empty;
                                string Temp = "";
                                if (MobileEmail != string.Empty && MobileEmail.Length >= 10)
                                {
                                    Temp = MobileEmail.Substring(0, 10);
                                    if (Temp.All(char.IsDigit))
                                    {
                                        Mobile_Contact = Temp;
                                    }
                                    else
                                    {
                                        Mobile_Contact = string.Empty;
                                    }
                                    Temp = MobileEmail.Substring(10, MobileEmail.Length - 10);
                                    if (Utility.IsValidEmailAddress(Temp))
                                    {
                                        Email = Temp.ToString();
                                    }
                                    else
                                    {
                                        Email = string.Empty;
                                    }
                                    try
                                    {
                                        double isMobile = Convert.ToDouble(Mobile_Contact);
                                    }
                                    catch (FormatException)
                                    {
                                        Mobile_Contact = string.Empty;
                                        Email = MobileEmail.ToString();
                                    }
                                }
                            }
                            dtDtxRow["patMobile"] = Mobile_Contact;
                            dtDtxRow["patEmail"] = Email;
                        }
                        else
                        {
                            Mobile_Contact = dtDtxRow["patMobile"].ToString();
                            Email = dtDtxRow["patEmail"].ToString();
                        }

                        ////////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                        ProcedureDesc = "";
                        ProcedureCode = "";

                        DataRow[] dtCurApptProcedure = DtDentrixAppointment_Procedures_Data.Select("apptid = '" + dtDtxRow["appointment_id"].ToString().Trim() + "'");

                        foreach (var dtSinProc in dtCurApptProcedure.ToList())
                        {
                            if (dtSinProc["Tooth"].ToString() != "" && dtSinProc["Tooth"].ToString() != "0")
                            {
                                ProcedureDesc = ProcedureDesc + dtSinProc["Tooth"].ToString().Trim() + '-';
                            }
                            if (dtSinProc["Surface"].ToString() != "" && dtSinProc["Surface"].ToString() != "0")
                            {
                                ProcedureDesc = ProcedureDesc + dtSinProc["Surface"].ToString().Trim() + '-';
                            }
                            if (dtSinProc["abbrevdescript"].ToString() != "" && dtSinProc["abbrevdescript"].ToString() != "0")
                            {
                                ProcedureDesc = ProcedureDesc + dtSinProc["abbrevdescript"].ToString().Trim() + ',';
                            }
                            if (dtSinProc["ProcedureCode"].ToString() != "" && dtSinProc["ProcedureCode"].ToString() != "0")
                            {
                                ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim() + ',';
                            }
                            //ProcedureDesc = ProcedureDesc + dtSinProc["abbrevdescript"].ToString().Trim() + ',';
                            //ProcedureCode = ProcedureCode + dtSinProc["ProcedureCode"].ToString().Trim() + ',';
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

                        DataRow[] row = dtLocalAppointment.Select("Appt_EHR_ID = '" + dtDtxRow["Appointment_id"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            // dtDtxRow["InsUptDlt"] = "UID";
                            if (row[0]["is_asap"] != null && row[0]["is_asap"].ToString() != string.Empty && Convert.ToBoolean(row[0]["is_asap"]) == true)
                            {
                                apptFlag = 2;
                            }
                            else
                            {
                                apptFlag = Convert.ToInt16(dtDtxRow["appt_flag"]);
                            }

                            if (row[0]["EHR_Entry_DateTime"] != DBNull.Value && Convert.ToDateTime(dtDtxRow["EHR_Entry_DateTime"].ToString().Trim()) != Convert.ToDateTime(row[0]["EHR_Entry_DateTime"]))
                            {
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else
                            {
                                MobileEmail = "";
                                if (dtDtxRow["patId"].ToString() == "0" || dtDtxRow["patId"].ToString() == string.Empty)
                                {
                                    Home_Contact = dtDtxRow["patient_phone"].ToString().Trim();
                                    Address = dtDtxRow["street1"].ToString().Trim();
                                    City = dtDtxRow["city"].ToString().Trim();
                                    State = dtDtxRow["state"].ToString().Trim();
                                    Zipcode = dtDtxRow["zipcode"].ToString().Trim();
                                    dtDtxRow["patCity"] = dtDtxRow["city"].ToString().Trim();
                                    dtDtxRow["patAddress"] = dtDtxRow["street1"].ToString().Trim();
                                    dtDtxRow["patState"] = dtDtxRow["state"].ToString().Trim();
                                    dtDtxRow["patZipcode"] = dtDtxRow["zipcode"].ToString().Trim();
                                    dtDtxRow["patHomephone"] = dtDtxRow["patient_phone"].ToString().Trim();
                                }
                                else
                                {
                                    Mobile_Contact = dtDtxRow["patMobile"].ToString();
                                    Email = dtDtxRow["patEmail"].ToString();
                                    Home_Contact = dtDtxRow["patHomephone"].ToString().Trim();
                                    Address = dtDtxRow["patAddress"].ToString().Trim();
                                    City = dtDtxRow["patCity"].ToString().Trim();
                                    State = dtDtxRow["patState"].ToString().Trim();
                                    Zipcode = dtDtxRow["patZipcode"].ToString().Trim();
                                }

                                if (dtDtxRow["appointment_status_ehr_key"].ToString().Trim() == "-106")
                                {
                                    AppointmentStatus = "<COMPLETE>";
                                }
                                else if (dtDtxRow["appointment_status_ehr_key"].ToString().Trim() == "0")
                                {
                                    AppointmentStatus = "<none>";
                                }
                                else
                                {
                                    AppointmentStatus = dtDtxRow["Appointment_Status"].ToString().Trim();
                                }

                                if (dtDtxRow["birth_date"] != null && row[0]["birth_date"] == null)
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (dtDtxRow["birth_date"] == null && row[0]["birth_date"] != null)
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (dtDtxRow["birth_date"] != null && dtDtxRow["birth_date"].ToString().Trim() != string.Empty && row[0]["birth_date"] != null && row[0]["birth_date"].ToString().Trim() != string.Empty
                                    && Convert.ToDateTime(dtDtxRow["birth_date"]).ToShortDateString() != Convert.ToDateTime(row[0]["birth_date"]).ToShortDateString())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (dtDtxRow["op_title"].ToString().Trim() != row[0]["Operatory_Name"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (dtDtxRow["op_id"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (Convert.ToInt16(dtDtxRow["appt_flag"]) != Convert.ToInt16(apptFlag))
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (dtDtxRow["ApptType_EHR_ID"].ToString().Trim() != row[0]["ApptType_EHR_ID"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (dtDtxRow["ApptType_Name"].ToString().Trim() != row[0]["ApptType"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (dtDtxRow["patId"].ToString().Trim() != row[0]["patient_ehr_id"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (AppointmentStatus.ToString().Trim() != row[0]["Appointment_Status"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (Utility.ConvertContactNumber(dtDtxRow["patHomephone"].ToString().Trim()) != Utility.ConvertContactNumber(row[0]["Home_Contact"].ToString().Trim()))
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (dtDtxRow["patEmail"].ToString().Trim() != row[0]["Email"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (Address.Trim() != row[0]["Address"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (City.Trim() != row[0]["City"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (State.Trim() != row[0]["ST"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else if (Zipcode.Trim() != row[0]["Zip"].ToString().Trim())
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
                                else if (Utility.ConvertContactNumber(Mobile_Contact.Trim()) != Utility.ConvertContactNumber(row[0]["Mobile_Contact"].ToString().Trim()))
                                {
                                    dtDtxRow["InsUptDlt"] = 4;
                                }
                                else
                                {
                                    dtDtxRow["InsUptDlt"] = 0;
                                }
                            }
                        }
                        else
                        {
                            DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Mobile_Contact + "'  AND ISNULL(Appt_EHR_ID,'0') = '0' ");
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
                    dtDentrixAppointment.AcceptChanges();

                    if (dtDentrixAppointment != null && dtDentrixAppointment.Rows.Count > 0)
                    {
                        if (!dtDentrixAppointment.Columns.Contains("Appt_Web_ID"))
                        {
                            dtDentrixAppointment.Columns.Add("Appt_Web_ID");
                        }
                        dtDentrixAppointment.Rows[0]["Appt_Web_ID"] = strWebID;
                        bool status = SynchDentrixBAL.Save_Appointment_Dentrix_To_Local(dtDentrixAppointment);

                        if (status)
                        {
                            Utility.WritetoAditEventSyncLogFile_Static("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            //SynchDataLiveDB_Push_Appointment(Convert.ToInt32("1"), Convert.ToInt32("0"), strPatID);
                            SynchDataLiveDB_Push_Appointment(strApptID);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SyncDataDentrix_AppointmentFromEvent Appointment Sync (" + Utility.Application_Name + " To Local Database) ]" + ex.Message);
            }
        }

        public static bool SynchDataDentrix_AppointmentsPatientFromEvent(string strPatID)
        {
            try
            {
                DataTable dtDentrixPatient = SynchDentrixBAL.GetDentrixAppointmentsPatientData(strPatID);
                DataTable dtDentrixPatientdue_date = SynchDentrixBAL.GetDentrixPatientdue_date(strPatID);

                DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1", strPatID);

                foreach (DataRow dr in dtDentrixPatient.Rows)
                {
                    string tmpdue_date = string.Empty;
                    try
                    {
                        DataRow[] Patdue_date = dtDentrixPatientdue_date.Select("patient_id = '" + dr["Patient_EHR_ID"] + "'");
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
                                    tmpdue_date = SortPatdue_date.Rows[i]["due_date"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type"].ToString() + "@" + SortPatdue_date.Rows[i]["recall_type_id"].ToString() + "|" + tmpdue_date;
                                }
                                dr["due_date"] = tmpdue_date;
                            }
                            else
                            {
                                foreach (DataRow due_row in Patdue_date)
                                {
                                    tmpdue_date = due_row["due_date"].ToString() + "@" + due_row["recall_type"].ToString() + "@" + due_row["recall_type_id"].ToString() + "|" + tmpdue_date;
                                }
                                dr["due_date"] = tmpdue_date;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        tmpdue_date = string.Empty;
                    }
                }

                DataTable dtSaveRecords = new DataTable();
                dtSaveRecords = dtLocalPatient.Clone();

                var itemsToBeAdded = (from OpenDentalPatient in dtDentrixPatient.AsEnumerable()
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

                var itemsToBeUpdated = (from OpenDentalPatient in dtDentrixPatient.AsEnumerable()
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

                if (dtSaveRecords.Rows.Count > 0)
                {
                    bool isPatientSave = SynchDentrixBAL.Save_Patient_Dentrix_To_Local_New(dtSaveRecords, "0", "1");
                    if (isPatientSave)
                    {
                        Utility.WritetoAditEventSyncLogFile_Static("Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        SynchDataDentrix_PatientStatusFromEvent(strPatID);
                        SynchDataLiveDB_Push_Patient(strPatID);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataDentrix_AppointmentsPatient [Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                return false;
            }
        }

        public static void SynchDataDentrix_PatientStatusFromEvent(string strPatID)
        {
            try
            {
                DataTable dtDentrixPatientStatus = SynchDentrixBAL.GetDentrixPatientStatusData(strPatID);
                if (dtDentrixPatientStatus != null && dtDentrixPatientStatus.Rows.Count > 0)
                {
                    SynchLocalBAL.UpdatePatient_Status(dtDentrixPatientStatus, "1", "0", strPatID);
                }
                Utility.WritetoAditEventSyncLogFile_Static("PatientStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                //SynchDataLiveDB_Push_PatientStatus();
            }
            catch (Exception ex)
            {
                Utility.WritetoAditEventErrorLogFile_Static("[SynchDataDentrix_PatientStatus [Appointment Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public void SynchDataLocalToDentrix_Patient_Form_FromEvent(string strPatientFormID, string Clinic_Number, string Service_Install_Id)
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
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent WebPatientForm : " + dtWebPatient_Form.Rows.Count);
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent Location_ID : " + Location_ID);
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent strDbConnString : " + strDbConnString);

                dtWebPatient_Form.Columns.Add("TableName", typeof(string)); // Added for Pat Form Task
                dtWebPatient_Form.Columns["TableName"].DefaultValue = "patient"; // Added for Pat Form Task

                if (dtWebPatient_Form != null)
                {
                    if (dtWebPatient_Form.Rows.Count > 0)
                    {
                        Utility.CheckEntryUserLoginIdExist();
                    }
                }

                foreach (DataRow dtDtxRow in dtWebPatient_Form.Rows)
                {

                    if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "first_name")
                    {
                        dtDtxRow["ehrfield"] = "firstname";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "last_name")
                    {
                        dtDtxRow["ehrfield"] = "lastname";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "mobile")
                    {
                        dtDtxRow["ehrfield"] = "pager";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "birth_date")
                    {
                        dtDtxRow["ehrfield"] = "birthdate";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "email")
                    {
                        dtDtxRow["ehrfield"] = "emailaddr";
                    }
                    if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "first_name")
                    {
                        dtDtxRow["ehrfield"] = "status";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "marital_status")
                    {
                        dtDtxRow["ehrfield"] = "fampos";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "SINGLE")
                        {
                            dtDtxRow["ehrfield_value"] = 1;
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MARRIED")
                        {
                            dtDtxRow["ehrfield_value"] = 2;
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "CHILD")
                        {
                            dtDtxRow["ehrfield_value"] = 3;
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "OTHER")
                        {
                            dtDtxRow["ehrfield_value"] = 4;
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = 0;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "middle_name")
                    {
                        dtDtxRow["ehrfield"] = "mi";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "preferred_name")
                    {
                        dtDtxRow["ehrfield"] = "prefname";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sex")
                    {
                        dtDtxRow["ehrfield"] = "gender";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MALE")
                        {
                            dtDtxRow["ehrfield_value"] = 1;
                        }
                        else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                        {
                            dtDtxRow["ehrfield_value"] = 2;
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = 1;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "work_phone")
                    {
                        dtDtxRow["ehrfield"] = "workphone";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "pri_provider_id")
                    {
                        dtDtxRow["ehrfield"] = "provid1";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sec_provider_id")
                    {
                        dtDtxRow["ehrfield"] = "provid2";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "salutation")
                    {
                        dtDtxRow["ehrfield"] = "Salutation";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "primary_insurance_companyname")
                    {
                        dtDtxRow["ehrfield"] = "primary_insurance_companyname";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "receive_email")
                    {
                        dtDtxRow["ehrfield"] = "privacyflags";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "YES" || dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "")
                        {
                            dtDtxRow["ehrfield_value"] = 0;
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = 2;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "receive_sms")
                    {
                        dtDtxRow["ehrfield"] = "privacyflags";
                        if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "YES" || dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "")
                        {
                            dtDtxRow["ehrfield_value"] = 0;
                        }
                        else
                        {
                            dtDtxRow["ehrfield_value"] = 2;
                        }
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance")
                    {
                        dtDtxRow["ehrfield"] = "";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "secondary_insurance_companyname")
                    {
                        dtDtxRow["ehrfield"] = "secondary_insurance_companyname";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "zipcode")
                    {
                        dtDtxRow["ehrfield"] = "zipcode";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_one")
                    {
                        dtDtxRow["ehrfield"] = "street1";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_two")
                    {
                        dtDtxRow["ehrfield"] = "street2";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "city")
                    {
                        dtDtxRow["ehrfield"] = "city";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "state")
                    {
                        dtDtxRow["ehrfield"] = "state";
                    }
                    else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "home_phone")
                    {
                        dtDtxRow["ehrfield"] = "phone";
                    }

                    #region Patient Form New EHR Fields

                    else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "ssn"))
                    {
                        dtDtxRow["ehrfield"] = "ssn";
                    }
                    else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "driverlicense"))
                    {
                        dtDtxRow["ehrfield"] = "driverslicense";
                    }
                    else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "school"))
                    {
                        dtDtxRow["TableName"] = "claiminfo";
                        dtDtxRow["ehrfield"] = "school";
                    }
                    else if ((dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "employer"))
                    {
                        dtDtxRow["ehrfield"] = "name";
                        dtDtxRow["TableName"] = "employer";
                    }

                    #endregion


                    dtWebPatient_Form.AcceptChanges();
                }
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent WebPatientForm  : " + dtWebPatient_Form.Rows.Count);

                if (dtWebPatient_Form != null && dtWebPatient_Form.Rows.Count > 0)
                {
                    bool Is_Record_Update = SynchDentrixBAL.Save_Patient_Form_Local_To_Dentrix(dtWebPatient_Form);
                    Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent Is_Record_Update  : " + Is_Record_Update);
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
                ObjGoalBase.WriteToSyncLogFile("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
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

                Is_synched_PatinetForm = false;
                try
                {
                    GetMedicalDentrixHistoryRecords(strPatientFormID);
                    SynchDentrixBAL.SaveMedicalHistoryLocalToDentrix(strPatientFormID);
                    ObjGoalBase.WriteToSyncLogFile("Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : Medical History Done");
                if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower())
                {
                    try
                    {
                        SynchDentrixBAL.SaveDiseaseLocalToDentrix(strPatientFormID);
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Disease Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }
                    try
                    {
                        SynchDentrixBAL.DeleteDiseaseLocalToDentrix(strPatientFormID);
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Disease Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }
                }
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : Disease Done");
                bool isRecordSaved = false, isRecordDeleted = false;
                string Patient_EHR_IDS = "";
                string DeletePatientEHRID = "";
                string SavePatientEHRID = "";
                try
                {
                    SynchDentrixBAL.DeleteMedicationLocalToDentrix(ref isRecordDeleted, ref DeletePatientEHRID, strPatientFormID);
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Delete_Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
                try
                {
                    SynchDentrixBAL.SaveMedicationLocalToDentrix(ref isRecordSaved, ref SavePatientEHRID, strPatientFormID);
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("[Save_Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
                if (isRecordSaved || isRecordDeleted)
                {
                    Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                    if (Patient_EHR_IDS != "")
                    {
                        SynchDataDentrix_PatientMedication(Patient_EHR_IDS);
                    }
                }
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : Medication Done");
                #region PatientInformation Document
                if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower() && IsDocumentUpload == false)
                {
                    try
                    {
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : App Version: " + Utility.Application_Version.ToLower());
                        IsDocumentUpload = true;
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : GetPatientDocument Start.");
                        GetPatientDocument("1", strPatientFormID);
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : GetPatientDocument Done.");

                        GetPatientDocument_New("1", strPatientFormID);
                        GoalBase.GetConnectionStringforDoc(false);
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : GetConnectionStringforDoc Done");
                        if (Utility.DentrixDocPWD != null && Utility.DentrixDocPWD != "")
                        {
                            if (Utility.DentrixDocConnString != null && Utility.DentrixDocConnString != "")
                            {
                                SynchDentrixBAL.Save_Document_in_Dentrix(strPatientFormID);
                            }
                        }
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : Patient_Document Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        ObjGoalBase.WriteToSyncLogFile("Patient_Document Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                        IsDocumentUpload = false;
                    }
                    catch (Exception ex)
                    {
                        IsDocumentUpload = false;
                        Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent : [Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                        ObjGoalBase.WriteToErrorLogFile("[Patient_Document Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Is_synched_PatinetForm = false;
                Utility.WritetoAditEventDebugLogFile_Static("SynchDataLocalToDentrix_Patient_Form_FromEvent Error: " + ex.Message);
                if (!ex.Message.Contains("Key value already exists in index 9"))
                {
                    ObjGoalBase.WriteToErrorLogFile("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
            }
        }
        #endregion

    }
}
