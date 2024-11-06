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
using System.Threading;
using System.Timers;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections;
using System.Globalization;

namespace Pozative
{
    public partial class frmPozative
    {
        #region Variable

        static bool IsEasyDentalProviderSync = false;
        static bool IsEasyDentalOperatorySync = false;
        static bool IsEasyDentalApptTypeSync = false;
        // bool IsDocumentUpload = false;

        static bool IsEasyDentalPatinetFormSync = false;
        //bool Is_synched = false;
        //bool Is_synched_Provider = false;
        //bool Is_synched_Speciality = false;
        //bool Is_synched_Operatory = false;
        //bool Is_synched_OperatoryEvent = false;
        //bool Is_synched_Type = false;
        //bool Is_synched_Appointment = false;
        //bool Is_synched_RecallType = false;
        //bool Is_synched_ApptStatus = false;
        private BackgroundWorker bwSynchEasyDental_Appointment = null;
        private System.Timers.Timer timerSynchEasyDental_Appointment = null;

        private BackgroundWorker bwSynchEasyDental_OperatoryEvent = null;
        private System.Timers.Timer timerSynchEasyDental_OperatoryEvent = null;

        private BackgroundWorker bwSynchEasyDental_Provider = null;
        private System.Timers.Timer timerSynchEasyDental_Provider = null;

        private BackgroundWorker bwSynchEasyDental_ProviderOfficeHours = null;
        private System.Timers.Timer timerSynchEasyDental_ProviderOfficeHours = null;

        private BackgroundWorker bwSynchEasyDental_Speciality = null;
        private System.Timers.Timer timerSynchEasyDental_Speciality = null;

        private BackgroundWorker bwSynchEasyDental_Operatory = null;
        private System.Timers.Timer timerSynchEasyDental_Operatory = null;

        private BackgroundWorker bwSynchEasyDental_OperatoryHours = null;
        private System.Timers.Timer timerSynchEasyDental_OperatoryHours = null;

        private BackgroundWorker bwSynchEasyDental_ApptType = null;
        private System.Timers.Timer timerSynchEasyDental_ApptType = null;

        private BackgroundWorker bwSynchEasyDental_Patient = null;
        private System.Timers.Timer timerSynchEasyDental_Patient = null;

        private BackgroundWorker bwSynchEasyDental_Disease = null;
        private System.Timers.Timer timerSynchEasyDental_Disease = null;

        private BackgroundWorker bwSynchEasyDental_RecallType = null;
        private System.Timers.Timer timerSynchEasyDental_RecallType = null;

        private BackgroundWorker bwSynchEasyDental_ApptStatus = null;
        private System.Timers.Timer timerSynchEasyDental_ApptStatus = null;

        private BackgroundWorker bwSynchEasyDental_Holidays = null;
        private System.Timers.Timer timerSynchEasyDental_Holidays = null;

        private BackgroundWorker bwSynchEasyDental_User = null;
        private System.Timers.Timer timerSynchEasyDental_User = null;

        private BackgroundWorker bwSynchLocalToEasyDental_Appointment = new BackgroundWorker();
        private System.Timers.Timer timerSynchLocalToEasyDental_Appointment = null;

        private BackgroundWorker bwSynchLocalToEasyDental_Patient_Form = null;
        private System.Timers.Timer timerSynchLocalToEasyDental_Patient_Form = null;


        private BackgroundWorker bwSynchEasyDental_MedicalHistory = null;
        private System.Timers.Timer timerSynchEasyDental_MedicalHistory = null;

        #endregion

        private void CallSynchEasyDentalToLocal()
        {
            if (Utility.AditSync)
            {



                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_Operatory();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_Provider();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_ApptType();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_Disease();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_RecallType();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_User();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_ApptStatus();
                SynchDataEasyDental_MedicleFormData();
                SynchDataEasyDental_MedicleQuestionData();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_Speciality();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_Appointment();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataLocalToEasyDental_Appointment();
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataLocalToEasyDental_Patient_Form();
                //rooja 15-5-23
                Thread.Sleep(1000);
                Application.DoEvents();
                SynchDataEasyDental_Holidays();
                
                // SynchDataLocalToEasyDental_Patient_Form();
                //  fncSynchDataLocalToEasyDental_Patient_Form();

                //SynchDataEasyDental_Speciality();
                // fncSynchDataEasyDental_Speciality();

                ////  SynchDataEasyDental_Operatory();
                //fncSynchDataEasyDental_Operatory();

                ////SynchDataEasyDental_Provider();
                //fncSynchDataEasyDental_Provider();



                ////SynchDataEasyDental_OperatoryEvent();
                ////fncSynchDataEasyDental_OperatoryEvent();

                ////SynchDataEasyDental_ApptType();
                //fncSynchDataEasyDental_ApptType();

                ////SynchDataEasyDental_Appointment();
                //fncSynchDataEasyDental_Appointment();

                //// SynchDataEasyDental_Disease();
                //fncSynchDataEasyDental_Disease();

                //fncSynchDataLocalToEasyDental_Appointment();

                ////SynchDataEasyDental_Patient();
                ////fncSynchDataEasyDental_Patient();

                //// SynchDataEasyDental_RecallType();
                //fncSynchDataEasyDental_RecallType();

                ////SynchDataEasyDental_ApptStatus();
                //fncSynchDataEasyDental_ApptStatus();

                ////SynchDataEasyDental_Holidays();
                ////fncSynchDataEasyDental_Holidays();

                //// SynchDataEasyDental_MedicleHistory();
                //fncSynchDataEasyDental_MedicalHistory();

            }
        }

        public void EasyDentalSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool IsSyncing = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
            {
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("IsEasyDentalSyncing").ToString());
                }
            }
            if (!IsEasyDentalSyncing && !IsSyncing)
            {
                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                {
                    key1.SetValue("IsEasyDentalSyncing", true);
                }

                EasyDentalSync.Enabled = false;
                IsEasyDentalSyncing = true;
                try
                {
                    using (Process myProcess = new Process())
                    {
                        try
                        {
                            ObjGoalBase.WriteToSyncLogFile("Appointment Syncing Start ");
                            IsProviderSyncedFirstTime = true;
                            //CallSynchEasyDentalToLocal();
                            myProcess.StartInfo.UseShellExecute = false;
                            // You can start any process, HelloWorld is a do-nothing example.
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\EasyDentalSync.exe";
                            myProcess.StartInfo.Arguments = "APPOITMENT";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.Start();
                            // This code assumes the process you are starting will terminate itself.
                            // Given that is is started without a window so you cannot terminate it
                            // on the desktop, it must terminate itself or you can do it programmatically
                            // from this application using the Kill method.
                            IsEasyDentalSyncing = false;
                            ObjGoalBase.WriteToSyncLogFile("Appointment Syncing Successfully. ");
                        }
                        catch (Exception e1)
                        {
                            IsEasyDentalSyncing = false;
                            ObjGoalBase.WriteToErrorLogFile("EasyDentalSync_Elapsed_Err " + e1.Message);
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
                    EasyDentalSync.Enabled = true;
                    IsEasyDentalSyncing = false;
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsEasyDentalSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("EasyDentalSync_Elapsed_Err " + ex.Message);
                }
                finally
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsEasyDentalSyncing", false);
                    }
                    EasyDentalSync.Enabled = true;
                }
            }
        }

        #region easydental payment code by shrutii
        public void EasydentalPayment_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool IsSyncing = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
            {
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("IsEasyDentalPaymentSyncing").ToString());
                }
            }
            if (!IsEasyDentalPaymentSyncing && !IsSyncing)
            {
                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                {
                    key1.SetValue("IsEasyDentalPaymentSyncing", true);
                }

                tmrEasydentalPayment.Enabled = false;
                IsEasyDentalPaymentSyncing = true;
                try
                {
                    using (Process myProcess = new Process())
                    {
                        try
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient Payment Syncing Start ");
                            IsProviderSyncedFirstTime = true;
                            myProcess.StartInfo.UseShellExecute = false;
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\EasyDentalSync.exe";
                            myProcess.StartInfo.Arguments = "PAYMENT";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.Start();
                            IsEasyDentalPaymentSyncing = false;
                            ObjGoalBase.WriteToSyncLogFile("Payment Syncing Successfully. ");
                        }
                        catch (Exception e1)
                        {
                            IsEasyDentalPaymentSyncing = false;
                            ObjGoalBase.WriteToErrorLogFile("tmrEasydentalPayment_Elapsed_Err " + e1.Message);
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    tmrEasydentalPayment.Enabled = true;
                    IsEasyDentalPaymentSyncing = false;
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsEasyDentalPaymentSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("EasydentalPayment_Elapsed_Err " + ex.Message);
                }
                finally
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsEasyDentalPaymentSyncing", false);
                    }
                    tmrEasydentalPayment.Enabled = true;
                }
            }
        }
        #endregion

        public void EasyDentalPatientSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
            bool IsSyncing = false;
            if (key != null)
            {
                IsSyncing = bool.Parse(key.GetValue("IsEasyDentalPatinetSyncing").ToString());
            }

            if (!IsEasyDentalPatientSyncing && !IsSyncing)
            {
                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                {
                    key1.SetValue("IsEasyDentalPatinetSyncing", true);
                }

                EasyDentalPatientSync.Enabled = false;
                IsEasyDentalPatientSyncing = true;
                try
                {
                    using (Process myProcess = new Process())
                    {

                        try
                        {
                            ObjGoalBase.WriteToSyncLogFile("Patient Syncing Start ");
                            // SynchDataEasyDental_Patient();
                            myProcess.StartInfo.UseShellExecute = false;
                            // You can start any process, HelloWorld is a do-nothing example.
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\EasyDentalSync.exe";
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
                            ObjGoalBase.WriteToErrorLogFile("EasyDentalSync_Elapsed_Err " + e1.Message);
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

                    IsEasyDentalPatientSyncing = false;
                    //RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                    //key2.SetValue("IsEasyDentalPatinetSyncing", false);
                    ObjGoalBase.WriteToSyncLogFile("Patient Synched Successfully..");
                }
                catch (Exception ex)
                {
                    EasyDentalPatientSync.Enabled = true;
                    IsEasyDentalPatientSyncing = false;
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsEasyDentalPatinetSyncing", false);
                        ObjGoalBase.WriteToErrorLogFile("EasyDentalSync_Elapsed_Err " + ex.Message);
                    }
                }
                finally
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsEasyDentalPatinetSyncing", false);
                    }
                    EasyDentalPatientSync.Enabled = true;
                }
            }
        }

        public void SyncEasyDentalPatientRecordsInitialy()
        {

            if (!IsEasyDentalPatientSyncing && Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    bool IsSyncing = false;
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        if (key != null)
                        {
                            IsSyncing = bool.Parse(key.GetValue("IsEasyDentalPatinetSyncing").ToString());
                        }
                    }
                    if (!IsSyncing)
                    {
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsEasyDentalPatinetSyncing", true);
                        }
                        IsEasyDentalPatientSyncing = true;
                        //   SynchDataEasyDental_Patient();
                        using (Process myProcess = new Process())
                        {

                            try
                            {
                                //SynchDataEasyDental_Patient();
                                myProcess.StartInfo.UseShellExecute = false;
                                // You can start any process, HelloWorld is a do-nothing example.
                                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\EasyDentalSync.exe";
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
                                IsEasyDentalPatientSyncing = false;

                            }
                            catch (Exception e1)
                            {
                                IsEasyDentalPatientSyncing = false;
                                using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key2.SetValue("IsEasyDentalPatinetSyncing", false);
                                }
                                ObjGoalBase.WriteToErrorLogFile("EasyDentalSync_Elapsed_Err " + e1.Message);
                                throw;
                            }
                        }
                    }
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsEasyDentalPatinetSyncing", false);
                    }
                }
                catch (Exception ex)
                {
                    IsEasyDentalPatientSyncing = false;
                    using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key2.SetValue("IsEasyDentalPatinetSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("EasyDentalSync.exe_Elapsed_Err " + ex.Message);
                    throw;
                }
            }
        }

        public void SyncEasyDentalPatientPaymentRecordsInitialy()
        {

            if (!IsEasyDentalPaymentSyncing && Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    bool IsSyncing = false;
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        if (key != null)
                        {
                            IsSyncing = bool.Parse(key.GetValue("IsEasyDentalPaymentSyncing").ToString());
                        }
                    }
                    if (!IsSyncing)
                    {
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsEasyDentalPaymentSyncing", true);
                        }
                        IsEasyDentalPaymentSyncing = true;
                        using (Process myProcess = new Process())
                        {

                            try
                            {
                                myProcess.StartInfo.UseShellExecute = false;
                                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\EasyDentalSync.exe";
                                myProcess.StartInfo.Arguments = "PAYMENT";
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.Start();
                                IsEasyDentalPaymentSyncing = false;

                            }
                            catch (Exception e1)
                            {
                                IsEasyDentalPaymentSyncing = false;
                                using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key2.SetValue("IsEasyDentalPaymentSyncing", false);
                                }
                                ObjGoalBase.WriteToErrorLogFile("EasyDentalSync_Elapsed_Err " + e1.Message);
                                throw;
                            }
                        }
                    }
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsEasyDentalPaymentSyncing", false);
                    }
                }
                catch (Exception ex)
                {
                    IsEasyDentalPaymentSyncing = false;
                    using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key2.SetValue("IsEasyDentalPaymentSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("EasyDentalSync.exe_Elapsed_Err " + ex.Message);
                    throw;
                }
            }
        }
        public void SyncEasyDentalRecordsInitialy()
        {

            if (!IsEasyDentalSyncing && Utility.IsApplicationIdleTimeOff)
            {
                try
                {
                    bool IsSyncing = false;
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        if (key != null)
                        {
                            IsSyncing = bool.Parse(key.GetValue("IsEasyDentalSyncing").ToString());
                        }
                    }
                    if (!IsSyncing)
                    {
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsEasyDentalSyncing", true);
                        }
                        IsEasyDentalSyncing = true;
                        using (Process myProcess = new Process())
                        {

                            try
                            {
                                //SynchDataLiveDB_Pull_PatientForm();
                                // CallSynchEasyDentalToLocal();
                                IsProviderSyncedFirstTime = true;
                                // CallSynchEasyDentalToLocal();
                                //SynchDataLiveDB_Push_Appointment();
                                myProcess.StartInfo.UseShellExecute = false;
                                // You can start any process, HelloWorld is a do-nothing example.
                                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\EasyDentalSync.exe";
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
                                IsEasyDentalSyncing = false;
                                using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key2.SetValue("IsEasyDentalSyncing", false);
                                }


                            }
                            catch (Exception e1)
                            {
                                IsEasyDentalSyncing = false;
                                using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key2.SetValue("IsEasyDentalSyncing", false);
                                }
                                ObjGoalBase.WriteToErrorLogFile("EasyDentalSync_Elapsed_Err " + e1.Message);
                                throw;
                            }
                        }
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsEasyDentalSyncing", false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsEasyDentalSyncing = false;
                    using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key2.SetValue("IsEasyDentalSyncing", false);
                    }
                    ObjGoalBase.WriteToErrorLogFile("EasyDentalSync.exe_Elapsed_Err " + ex.Message);
                    throw;
                }
            }
        }

        #region Synch Appointment

        private void fncSynchDataEasyDental_Appointment()
        {
            InitBgWorkerEasyDental_Appointment();
            InitBgTimerEasyDental_Appointment();
        }

        private void InitBgTimerEasyDental_Appointment()
        {
            timerSynchEasyDental_Appointment = new System.Timers.Timer();
            this.timerSynchEasyDental_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchEasyDental_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_Appointment_Tick);
            timerSynchEasyDental_Appointment.Enabled = true;
            timerSynchEasyDental_Appointment.Start();
            timerSynchEasyDental_Appointment_Tick(null, null);
        }

        private void InitBgWorkerEasyDental_Appointment()
        {
            bwSynchEasyDental_Appointment = new BackgroundWorker();
            bwSynchEasyDental_Appointment.WorkerReportsProgress = true;
            bwSynchEasyDental_Appointment.WorkerSupportsCancellation = true;
            bwSynchEasyDental_Appointment.DoWork += new DoWorkEventHandler(bwSynchEasyDental_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEasyDental_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_Appointment_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_Appointment.Enabled = false;
                MethodForCallSynchOrderEasyDental_Appointment();
            }
        }

        public void MethodForCallSynchOrderEasyDental_Appointment()
        {
            System.Threading.Thread procThreadmainEasyDental_Appointment = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_Appointment);
            procThreadmainEasyDental_Appointment.Start();
        }

        public void CallSyncOrderTableEasyDental_Appointment()
        {
            if (bwSynchEasyDental_Appointment.IsBusy != true)
            {
                bwSynchEasyDental_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }

                SynchDataEasyDental_Appointment();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_Appointment.Enabled = true;
        }

        public static void SynchDataEasyDental_Appointment()
        {
            //if (IsEasyDentalProviderSync && IsEasyDentalOperatorySync && IsEasyDentalApptTypeSync && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            //{
            try
            {

                DataTable dtEasyDentalAppointment = SynchEasyDentalBAL.GetEasyDentalAppointmentData();
                SynchDataEasyDental_AppointmentPatientData(dtEasyDentalAppointment);
                if (IS_Appt_Patient_Sync)
                {

                    dtEasyDentalAppointment.Columns.Add("Appt_LocalDB_ID", typeof(int));
                    dtEasyDentalAppointment.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalAppointment.Columns.Add("ProcedureDesc", typeof(string));
                    dtEasyDentalAppointment.Columns.Add("ProcedureCode", typeof(string));

                    DataTable dtEasyDentalDeletedAppointment = SynchEasyDentalBAL.GetEasyDentalDeletedAppointmentData();
                    dtEasyDentalDeletedAppointment.Columns.Add("InsUptDlt", typeof(int));


                    DataTable dtLocalAppointment = SynchLocalBAL.GetLocalAppointmentData("1");

                    DataTable dtLocalCompareForDeletedAppointment = SynchLocalBAL.GetLocalCompareForDeletedAppointmentData("1");
                    DataTable dtEasyDentalOperatory = SynchLocalBAL.GetLocalOperatoryData("1");
                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", "1");
                    DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData("1");
                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData("1");
                    DataTable dtEasyDentalPatient = SynchEasyDentalBAL.GetEasyDentalPatientData(0);
                    DataTable dtEasyDentalApptNote = SynchEasyDentalBAL.GetEasyDentalAppointmentNoteData("");


                    string ProcedureDesc = "";
                    string ProcedureCode = "";

                    #region Getting ProcedureData for Appointments

                    //DataTable DtEasyDentalAppointmentIdsForProcDesc_Data = SynchEasyDentalBAL.GetEasyDentalAppointment_ApptId_Procedures_Data();
                    DataTable DtEasyDentalProcType0_Data = SynchEasyDentalBAL.GetEasyDentalAppointment_Procedures_Type0_Data();
                    DataTable DtEasyDentalProcType1_Data = SynchEasyDentalBAL.GetEasyDentalAppointment_Procedures_Type1_Data();
                    DataTable DtEasyDentalProcDescForCodeType0_Data = SynchEasyDentalBAL.GetEasyDentalProcDescForCodeType0_Data();
                    DataTable DtEasyDentalProcDescForCodeType1_Data = SynchEasyDentalBAL.GetEasyDentalProcDescForCodeType1_Data();
                    DataTable DtEasyDentalProcDescForCodeType1_Sub_Data = DtEasyDentalProcDescForCodeType0_Data; //SynchEasyDentalBAL.GetEasyDentalProcDescForCodeType1_Sub_Data();

                    DtEasyDentalProcType0_Data.Columns.Add("ADACode", typeof(string));
                    DtEasyDentalProcType0_Data.Columns.Add("AbbrevDesc", typeof(string));

                    DtEasyDentalProcType1_Data.Columns.Add("RangeStart", typeof(int));
                    DtEasyDentalProcType1_Data.Columns.Add("RangeEnd", typeof(int));
                    DtEasyDentalProcType1_Data.Columns.Add("Surface", typeof(string));
                    DtEasyDentalProcType1_Data.Columns.Add("AbbrevDesc", typeof(string));
                    DtEasyDentalProcType1_Data.Columns.Add("ADACode", typeof(string));

                    foreach (DataRow datRow in DtEasyDentalProcType0_Data.Rows)
                    {
                        foreach (DataRow drow in DtEasyDentalProcDescForCodeType0_Data.Select(" CodeId = '" + datRow["CodeId"].ToString().Trim() + "' "))
                        {
                            if (drow["ADACode"].ToString() != "")//&& drow["ADACode"].ToString() != "0"
                            {
                                datRow["ADACode"] = drow["ADACode"].ToString();
                            }

                            if (drow["AbbrevDesc"].ToString() != "")//&& drow["AbbrevDesc"].ToString() != "0"
                            {
                                datRow["AbbrevDesc"] = drow["AbbrevDesc"].ToString();
                            }
                        }
                    }

                    DataTable TestDt = DtEasyDentalProcType0_Data;

                    DataTable TestDt2 = new DataTable();

                    foreach (DataRow datRow in DtEasyDentalProcType1_Data.Rows)
                    {

                        int ProcId = 0;

                        foreach (DataRow drow in DtEasyDentalProcDescForCodeType1_Data.Select(" ProcedureId = '" + datRow["CodeId"].ToString().Trim() + "' "))
                        {
                            if (drow["RangeStart"].ToString() != "")// && drow["RangeStart"].ToString() != "0"
                            {
                                datRow["RangeStart"] = drow["RangeStart"].ToString();
                            }

                            if (drow["RangeEnd"].ToString() != "")// && drow["RangeEnd"].ToString() != "0"
                            {
                                datRow["RangeEnd"] = drow["RangeEnd"].ToString();
                            }

                            if (drow["Surface"].ToString() != "")// && drow["Surface"].ToString() != "0"
                            {
                                datRow["Surface"] = drow["Surface"].ToString();
                            }

                            if (Convert.ToInt16(drow["ProcCodeId"]) != 0)
                                ProcId = Convert.ToInt16(drow["ProcCodeId"]);

                        }

                        foreach (DataRow drow in DtEasyDentalProcDescForCodeType1_Sub_Data.Select(" CodeId = '" + ProcId.ToString().Trim() + "' "))//datRow["ProcCodeId"].ToString()
                        {

                            if (drow["ADACode"].ToString() != "")//&& drow["ADACode"].ToString() != "0"
                            {
                                datRow["ADACode"] = drow["ADACode"].ToString();
                            }

                            if (drow["AbbrevDesc"].ToString() != "")//&& drow["AbbrevDesc"].ToString() != "0"
                            {
                                datRow["AbbrevDesc"] = drow["AbbrevDesc"].ToString();
                            }

                        }

                    }

                    TestDt2 = DtEasyDentalProcType1_Data;

                    #endregion

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

                    int ToothStart = 0, ToothEnd = 0, Diff = 0;

                    #region
                    foreach (DataRow dtDtxRow in dtEasyDentalAppointment.Rows)
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

                        #region GetPatientDetail
                        try
                        {
                            if (dtDtxRow["patId"].ToString() != "0" || dtDtxRow["patId"].ToString() != string.Empty)
                            {
                                DataRow[] drEasyDentalPatient = dtEasyDentalPatient.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["patId"] + "'");
                                //   DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");


                                if (drEasyDentalPatient.Length > 0)
                                {
                                    dtDtxRow["patMobile"] = drEasyDentalPatient[0]["Mobile"].ToString().Trim();
                                    dtDtxRow["patient_phone"] = drEasyDentalPatient[0]["Home_Phone"].ToString().Trim();
                                    dtDtxRow["Address1"] = drEasyDentalPatient[0]["Address1"].ToString().Trim();
                                    dtDtxRow["Address2"] = drEasyDentalPatient[0]["Address2"].ToString().Trim();
                                    dtDtxRow["city"] = drEasyDentalPatient[0]["city"].ToString().Trim();
                                    dtDtxRow["state"] = drEasyDentalPatient[0]["state"].ToString().Trim();
                                    dtDtxRow["zipcode"] = drEasyDentalPatient[0]["zipcode"].ToString().Trim();
                                    dtDtxRow["birth_date"] = drEasyDentalPatient[0]["Birth_date"].ToString().Trim();
                                    dtDtxRow["patEmail"] = drEasyDentalPatient[0]["Email"].ToString().Trim();
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                        Mobile_Contact = dtDtxRow["patMobile"].ToString();
                        Email = dtDtxRow["patEmail"].ToString();
                        #endregion

                        #region GetAppt_Proc_Code_Teeth_Surface_Abbr


                        ////////////////////// For 2 Field (ProcedureDesc,ProcedureCode) in appointment table ////////////
                        ProcedureDesc = "";
                        ProcedureCode = "";

                        foreach (DataRow dtSinProc in TestDt2.Select(" AppointmentId = '" + dtDtxRow["appointment_id"].ToString().Trim() + "' "))//DtEasyDentalAppointment_Procedures_Data -- TestDt2
                        {
                            long ApptId = Convert.ToInt64(dtDtxRow["appointment_id"]);

                            #region Tooth Number Conversion


                            if (dtSinProc["RangeStart"] != DBNull.Value && dtSinProc["RangeEnd"] != DBNull.Value)
                            {
                                if ((Convert.ToInt16(dtSinProc["RangeStart"].ToString())) > 0 && (Convert.ToInt16(dtSinProc["RangeStart"].ToString())) < 33
                                    && (Convert.ToInt16(dtSinProc["RangeEnd"].ToString())) > 0 && (Convert.ToInt16(dtSinProc["RangeEnd"].ToString())) < 33)
                                {
                                    ToothStart = Convert.ToInt16(dtSinProc["RangeStart"]);
                                    ToothEnd = Convert.ToInt16(dtSinProc["RangeEnd"]);
                                    //Diff = Convert.ToInt16(dtSinProc["RangeEnd"]) - Convert.ToInt16(dtSinProc["RangeStart"]);

                                    if (ToothStart >= 9 && ToothStart <= 16)
                                    {
                                        ToothStart = ToothStart + 12;
                                    }
                                    else if (ToothStart >= 25 && ToothStart <= 32)
                                    {
                                        ToothStart = ToothStart + 16;
                                    }
                                    else
                                    {
                                        switch (ToothStart)
                                        {
                                            case 1:
                                                ToothStart = 18;
                                                break;
                                            case 2:
                                                ToothStart = 17;
                                                break;
                                            case 3:
                                                ToothStart = 16;
                                                break;
                                            case 4:
                                                ToothStart = 15;
                                                break;
                                            case 5:
                                                ToothStart = 14;
                                                break;
                                            case 6:
                                                ToothStart = 13;
                                                break;
                                            case 7:
                                                ToothStart = 12;
                                                break;
                                            case 8:
                                                ToothStart = 11;
                                                break;
                                            case 17:
                                                ToothStart = 38;
                                                break;
                                            case 18:
                                                ToothStart = 37;
                                                break;
                                            case 19:
                                                ToothStart = 36;
                                                break;
                                            case 20:
                                                ToothStart = 35;
                                                break;
                                            case 21:
                                                ToothStart = 34;
                                                break;
                                            case 22:
                                                ToothStart = 33;
                                                break;
                                            case 23:
                                                ToothStart = 32;
                                                break;
                                            case 24:
                                                ToothStart = 31;
                                                break;
                                        }
                                    }

                                    if (ToothEnd >= 9 && ToothEnd <= 16)
                                    {
                                        ToothEnd = ToothEnd + 12;
                                    }
                                    else if (ToothEnd >= 25 && ToothEnd <= 32)
                                    {
                                        ToothEnd = ToothEnd + 16;
                                    }
                                    else
                                    {
                                        switch (ToothEnd)
                                        {
                                            case 1:
                                                ToothEnd = 18;
                                                break;
                                            case 2:
                                                ToothEnd = 17;
                                                break;
                                            case 3:
                                                ToothEnd = 16;
                                                break;
                                            case 4:
                                                ToothEnd = 15;
                                                break;
                                            case 5:
                                                ToothEnd = 14;
                                                break;
                                            case 6:
                                                ToothEnd = 13;
                                                break;
                                            case 7:
                                                ToothEnd = 12;
                                                break;
                                            case 8:
                                                ToothEnd = 11;
                                                break;
                                            case 17:
                                                ToothEnd = 38;
                                                break;
                                            case 18:
                                                ToothEnd = 37;
                                                break;
                                            case 19:
                                                ToothEnd = 36;
                                                break;
                                            case 20:
                                                ToothEnd = 35;
                                                break;
                                            case 21:
                                                ToothEnd = 34;
                                                break;
                                            case 22:
                                                ToothEnd = 33;
                                                break;
                                            case 23:
                                                ToothEnd = 32;
                                                break;
                                            case 24:
                                                ToothEnd = 31;
                                                break;
                                        }
                                    }
                                }
                            }
                            #endregion


                            if (dtSinProc["RangeStart"].ToString() != "" && dtSinProc["RangeStart"].ToString() != "0")
                            {
                                //ProcedureDesc = ProcedureDesc + dtSinProc["RangeStart"].ToString().Trim() + '-';
                                ProcedureDesc = ProcedureDesc + ToothStart.ToString().Trim() + '-';
                            }
                            if (dtSinProc["RangeEnd"].ToString() != "" && dtSinProc["RangeEnd"].ToString() != "0" && dtSinProc["RangeStart"].ToString() != dtSinProc["RangeEnd"].ToString())
                            {
                                //ProcedureDesc = ProcedureDesc + dtSinProc["RangeEnd"].ToString().Trim() + '-';
                                ProcedureDesc = ProcedureDesc + ToothEnd.ToString().Trim() + '-';
                            }
                            if (dtSinProc["Surface"].ToString() != "" && dtSinProc["Surface"].ToString() != "0")
                            {
                                ProcedureDesc = ProcedureDesc + dtSinProc["Surface"].ToString().Replace(",", string.Empty).Trim() + '-';
                                //ProcedureDesc = ProcedureDesc + dtSinProc["Surface"].ToString().Trim() + '-';
                            }

                            foreach (DataRow dtSinProc_abbrevdescript in TestDt2.Select(" CodeId = '" + dtSinProc["CodeId"].ToString().Trim() + "' "))//DtEasyDentalAppointment_Procedures_Data_Sub -- ProcedureId -- CodeId -- TestDt2
                            {
                                if (dtSinProc_abbrevdescript["AppointmentId"].ToString() == ApptId.ToString())
                                {
                                    if (dtSinProc_abbrevdescript["AbbrevDesc"].ToString() != "" && dtSinProc_abbrevdescript["AbbrevDesc"].ToString() != "0")
                                    {
                                        ProcedureDesc = ProcedureDesc + dtSinProc_abbrevdescript["AbbrevDesc"].ToString().Trim() + ',';
                                    }
                                    if (dtSinProc_abbrevdescript["ADACode"].ToString() != "" && dtSinProc_abbrevdescript["ADACode"].ToString() != "0")
                                    {
                                        ProcedureCode = ProcedureCode + dtSinProc_abbrevdescript["ADACode"].ToString().Trim() + ',';
                                    }
                                }
                            }
                        }

                        foreach (DataRow dtSinProc_sec in TestDt.Select(" AppointmentId = '" + dtDtxRow["appointment_id"].ToString().Trim() + "' "))//DtEasyDentalAppointment_Procedures_SecondData -- TestDt
                        {

                            if (dtSinProc_sec["AbbrevDesc"].ToString() != "" && dtSinProc_sec["AbbrevDesc"].ToString() != "0")
                            {
                                ProcedureDesc = ProcedureDesc + dtSinProc_sec["AbbrevDesc"].ToString().Trim() + ',';
                            }
                            if (dtSinProc_sec["ADACode"].ToString() != "" && dtSinProc_sec["ADACode"].ToString() != "0")
                            {
                                ProcedureCode = ProcedureCode + dtSinProc_sec["ADACode"].ToString().Trim() + ',';
                            }

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

                        #endregion

                        #region GetProviderDetail
                        try
                        {
                            DataRow[] drEasyDentalProvider = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtDtxRow["provider_id"] + "'");
                            //   DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");

                            if (drEasyDentalProvider.Length > 0)
                            {
                                dtDtxRow["provider_first_name"] = drEasyDentalProvider[0]["First_Name"].ToString().Trim();
                                dtDtxRow["provider_last_name"] = drEasyDentalProvider[0]["Last_Name"].ToString().Trim();
                            }
                        }
                        catch (Exception)
                        {

                        }
                        #endregion

                        #region GetOperatoryDetail
                        try
                        {
                            DataRow[] drEasyDentalOperatory = dtEasyDentalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtDtxRow["op_id"] + "'");
                            //   DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");

                            if (drEasyDentalOperatory.Length > 0)
                            {
                                dtDtxRow["op_title"] = drEasyDentalOperatory[0]["Operatory_Name"].ToString().Trim();
                            }
                        }
                        catch (Exception)
                        {

                        }
                        #endregion

                        #region GetApptStatusDetail
                        try
                        {
                            DataRow[] drEasyDentalStatus = dtLocalApptStatus.Copy().Select("ApptStatus_EHR_ID = '" + dtDtxRow["appointment_status_ehr_key"] + "'");
                            //   DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");

                            if (drEasyDentalStatus.Length > 0)
                            {
                                dtDtxRow["Appointment_Status"] = drEasyDentalStatus[0]["ApptStatus_Name"].ToString().Trim();
                            }
                        }
                        catch (Exception)
                        {

                        }
                        #endregion

                        #region GetApptTypeDetail
                        try
                        {
                            DataRow[] drEasyDentalType = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                            //   DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");

                            if (drEasyDentalType.Length > 0)
                            {
                                dtDtxRow["ApptType_Name"] = drEasyDentalType[0]["Type_Name"].ToString().Trim();
                            }
                        }
                        catch (Exception)
                        {

                        }
                        #endregion

                        #region GetCommentFromEZ

                        DataRow[] drcomment = dtEasyDentalApptNote.Copy().Select("NoteID = '" + dtDtxRow["Appointment_id"] + "'");
                        if (drcomment.Length > 0)
                        {
                            dtDtxRow["comment"] = drcomment[0]["Text"].ToString().Trim();
                        }
                        else
                        {
                            dtDtxRow["comment"] = "";
                        }


                        #endregion

                        DataRow[] row = dtLocalAppointment.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appointment_id"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            // GoalBase.WriteToSyncLogFile_Static("[Appointment Sync ( .8.1) ]");
                            // dtDtxRow["InsUptDlt"] = "UID";
                            if (row[0]["is_asap"] != null && row[0]["is_asap"].ToString() != string.Empty && Convert.ToBoolean(row[0]["is_asap"]) == true)
                            {
                                apptFlag = 2;
                            }
                            else
                            {
                                apptFlag = Convert.ToInt16(dtDtxRow["appt_flag"]);
                            }
                            MobileEmail = "";
                            Home_Contact = dtDtxRow["patient_phone"].ToString().Trim();
                            Address = dtDtxRow["Address1"].ToString().Trim() + " , " + dtDtxRow["Address2"].ToString().Trim();
                            City = dtDtxRow["city"].ToString().Trim();
                            State = dtDtxRow["state"].ToString().Trim();
                            Zipcode = dtDtxRow["zipcode"].ToString().Trim();
                            string Apptdate = Convert.ToDateTime(dtDtxRow["appointment_date"].ToString()).ToString("dd/MM/yyyy");
                            string ApptTime = dtDtxRow["start_hour"].ToString().Split(':')[0].ToString() + ":" + dtDtxRow["start_hour"].ToString().Split(':')[1].ToString();
                            DateTime ApptDateTime = DateTime.ParseExact(Apptdate.ToString() + " " + ApptTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            DateTime ApptEndDateTime = ApptDateTime.AddMinutes(Convert.ToInt32(dtDtxRow["length"].ToString().Trim()));
                            if (dtDtxRow["appointment_status_ehr_key"].ToString().Trim() == "150")
                            {
                                dtDtxRow["appointment_status_ehr_key"] = "-106";
                                AppointmentStatus = "<COMPLETE>";
                                dtDtxRow["Appointment_Status"] = "<COMPLETE>";
                            }
                            else if (dtDtxRow["appointment_status_ehr_key"].ToString().Trim() == "0")
                            {
                                dtDtxRow["Appointment_Status"] = "<none>";
                                AppointmentStatus = "<none>";
                            }
                            else
                            {
                                AppointmentStatus = dtDtxRow["Appointment_Status"].ToString().Trim();
                            }
                            int commentlen = 1999;
                            if (dtDtxRow["comment"].ToString().Trim().Length < commentlen)
                            {
                                commentlen = dtDtxRow["comment"].ToString().Trim().Length;
                            }
                            if (dtDtxRow["birth_date"] != null && row[0]["birth_date"] == null)
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  birth_date]" + dtDtxRow["birth_date"].ToString() + " : " + row[0]["birth_date"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["birth_date"] == null && row[0]["birth_date"] != null)
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  birth_date _2]" + dtDtxRow["birth_date"].ToString() + " : " + row[0]["birth_date"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["birth_date"] != null && dtDtxRow["birth_date"].ToString().Trim() != string.Empty && row[0]["birth_date"] != null && row[0]["birth_date"].ToString().Trim() != string.Empty
                                && Convert.ToDateTime(dtDtxRow["birth_date"]) != Convert.ToDateTime(row[0]["birth_date"]))
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  birth_date_3]" + dtDtxRow["birth_date"].ToString() + " : " + row[0]["birth_date"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["op_title"].ToString().Trim() != row[0]["Operatory_Name"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  op_title]" + dtDtxRow["op_title"].ToString() + " : " + row[0]["Operatory_Name"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["op_id"].ToString().Trim() != row[0]["Operatory_EHR_ID"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  Operatory_EHR_ID]" + dtDtxRow["op_id"].ToString() + " : " + row[0]["Operatory_EHR_ID"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Convert.ToInt16(dtDtxRow["appt_flag"]) != Convert.ToInt16(apptFlag))
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  appt_flag]" + dtDtxRow["appt_flag"].ToString() + " : " + apptFlag.ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ApptType_EHR_ID"].ToString().Trim() != row[0]["ApptType_EHR_ID"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  ApptType_EHR_ID]" + dtDtxRow["ApptType_EHR_ID"].ToString() + " : " + row[0]["ApptType_EHR_ID"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["comment"].ToString().Trim().Substring(0, commentlen) != row[0]["comment"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  comment]" + dtDtxRow["comment"].ToString() + " : " + row[0]["comment"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["ApptType_Name"].ToString().Trim() != row[0]["ApptType"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  ApptType_Name]" + dtDtxRow["ApptType_Name"].ToString() + " : " + row[0]["ApptType"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["patId"].ToString().Trim() != row[0]["patient_ehr_id"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  patId]" + dtDtxRow["patId"].ToString() + " : " + row[0]["patient_ehr_id"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (AppointmentStatus.ToString().Trim() != row[0]["Appointment_Status"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  Appointment_Status]" + AppointmentStatus.ToString().Trim() + " : " + row[0]["Appointment_Status"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Utility.ConvertContactNumber(dtDtxRow["patient_phone"].ToString().Trim()).Trim() != row[0]["Home_Contact"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  patient_phone]" + Utility.ConvertContactNumber(dtDtxRow["patient_phone"].ToString().Trim()) + " : " + row[0]["Home_Contact"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (dtDtxRow["patEmail"].ToString().Trim() != row[0]["Email"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  patEmail]" + dtDtxRow["patEmail"].ToString() + " : " + row[0]["Email"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Address.Trim() != row[0]["Address"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  Address]" + Address.Trim() + " : " + row[0]["Address"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (City.Trim() != row[0]["City"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  City]" + City.Trim() + " : " + row[0]["City"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (State.Trim() != row[0]["ST"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  State]" + State.Trim() + " : " + row[0]["ST"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (Zipcode.Trim() != row[0]["Zip"].ToString().Trim())
                            {
                                // GoalBase.WriteToErrorLogFile_Static("[Appointment log  Zipcode]" + Zipcode.Trim() + " : " + row[0]["Zip"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (ApptDateTime.ToString().Trim() != row[0]["Appt_DateTime"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  appointment_date]" + ApptDateTime.ToString().Trim() + " : " + row[0]["Appt_DateTime"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else if (ApptEndDateTime.ToString().Trim() != row[0]["Appt_EndDateTime"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  appointment_date]" + ApptEndDateTime.ToString().Trim() + " : " + row[0]["Appt_EndDateTime"].ToString());
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

                            else if (Utility.ConvertContactNumber(Mobile_Contact).Trim() != row[0]["Mobile_Contact"].ToString().Trim())
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log  Mobile_Contact]" + Mobile_Contact.Trim() + " : " + row[0]["Mobile_Contact"].ToString());
                                dtDtxRow["InsUptDlt"] = 4;
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 0;
                            }

                        }
                        else
                        {
                            DataRow[] rowCon = dtLocalAppointment.Copy().Select("Mobile_Contact = '" + Utility.ConvertContactNumber(Mobile_Contact).Trim() + "'  AND ISNULL(Appt_EHR_ID,0) =0 ");
                            if (rowCon.Length > 0)
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Appointment log InsUptDlt 5]");
                                dtDtxRow["InsUptDlt"] = 5;
                                dtDtxRow["Appt_LocalDB_ID"] = rowCon[0]["Appt_LocalDB_ID"];
                            }
                            else
                            {
                                dtDtxRow["InsUptDlt"] = 1;
                            }
                        }
                    }

                    dtEasyDentalAppointment.AcceptChanges();


                    ///////////////////////////

                    DataView dvDTXAppt = new DataView(dtEasyDentalAppointment.Copy());
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
                            //GoalBase.WriteToErrorLogFile_Static("[Appointment log InsUptDlt 3]");
                            DataRow ApptDtldr = dtEasyDentalAppointment.NewRow();
                            ApptDtldr["appointment_id"] = dtDtlRow["appointment_id"].ToString().Trim();
                            ApptDtldr["InsUptDlt"] = 3;
                            dtEasyDentalAppointment.Rows.Add(ApptDtldr);
                        }
                    }

                    ///////////////////////////
                    if (dtEasyDentalAppointment != null && dtEasyDentalAppointment.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_Appointment_EasyDental_To_Local(dtEasyDentalAppointment);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Appointment");
                            GoalBase.WriteToSyncLogFile_Static("Appointment Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsProviderSyncPush = true;
                            IsOperatorySyncPush = true;
                            IsApptTypeSyncPush = true;
                            SynchDataLiveDB_Push_Appointment();
                        }
                    }
                    //////////////////////

                    DataTable dtLocalAppointmentAfterInsert = SynchLocalBAL.GetLocalAppointmentData("1");
                    bool Save_DeletedAppointment_status = false;

                    foreach (DataRow dtDtxRow in dtEasyDentalDeletedAppointment.Rows)
                    {
                        DataRow[] row = dtLocalAppointmentAfterInsert.Copy().Select("Appt_EHR_ID = '" + dtDtxRow["Appointment_id"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            if (Convert.ToBoolean(row[0]["is_deleted"].ToString().Trim()) == false)
                            {
                                // GoalBase.WriteToErrorLogFile_Static("[Appointment log InsUptDlt IS_Deleted]");
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                        }
                        else
                        {
                            dtDtxRow["InsUptDlt"] = 1;
                        }
                    }
                    #endregion

                    dtEasyDentalDeletedAppointment.AcceptChanges();

                    if (dtEasyDentalDeletedAppointment != null && dtEasyDentalDeletedAppointment.Rows.Count > 0)
                    {
                        Save_DeletedAppointment_status = SynchEasyDentalBAL.Update_DeletedAppointment_EasyDental_To_Local(dtEasyDentalDeletedAppointment);
                    }
                }
                else
                {
                    GoalBase.WriteToErrorLogFile_Static("Appointment Patient data not synched ");
                }

                /////////////////

            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Appointment Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
            finally
            {

            }
            // }
        }

        static bool IS_Appt_Patient_Sync = true;
        public static void SynchDataEasyDental_AppointmentPatientData(DataTable dtEasyDentalAppointment)
        {
            if (Utility.AditLocationSyncEnable)
            {
                try
                {
                    IS_Appt_Patient_Sync = false;
                    dtEasyDentalAppointment.DefaultView.RowFilter = "patId <> 0";

                    DataTable dtApptPatient = dtEasyDentalAppointment.DefaultView.ToTable(true, "patId");
                    DataTable DtLocalPatient = SynchLocalBAL.GetAllLocalPatientData();
                    var rs = dtApptPatient.AsEnumerable().Select(o => Convert.ToInt32(o.Field<object>("patId"))).Except(DtLocalPatient.AsEnumerable().Select(b => Convert.ToInt32(b.Field<object>("Patient_EHR_ID"))).Distinct()).ToList();
                    if (rs != null)
                    {
                        string NewEHRIDs = string.Join(",", rs);
                        if (NewEHRIDs != string.Empty)
                        {

                            try
                            {
                                DataTable dtEasyDentalPatient = SynchEasyDentalBAL.GetEasyDentalPatientData(NewEHRIDs);

                                DataTable dtEasyDentalPatientNextApptDate = SynchEasyDentalBAL.GetEasyDentalPatientNextApptDate(NewEHRIDs);
                                DataTable dtEasyDentalPatientdue_date = SynchEasyDentalBAL.GetEasyDentalPatientdue_date(NewEHRIDs);

                                string patientTableName = "Patient";

                                if (dtEasyDentalPatient != null && dtEasyDentalPatient.Rows.Count > 0)
                                {
                                    bool isPatientSave = SynchEasyDentalBAL.Save_ApptPatient_EasyDental_To_Local(dtEasyDentalPatient, patientTableName, dtEasyDentalPatientNextApptDate, dtEasyDentalPatientdue_date);
                                    if (isPatientSave)
                                    {
                                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                        GoalBase.WriteToSyncLogFile_Static("ApptPatient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                        SynchDataLiveDB_Push_Patient();
                                    }
                                }
                                else
                                {
                                    GoalBase.WriteToSyncLogFile_Static("ApptPatient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                                    bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                                }
                            }
                            catch (Exception ex)
                            {
                                IS_Appt_Patient_Sync = true;
                                GoalBase.WriteToErrorLogFile_Static("[ApptPatient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                            }
                            SynchDataEasyDental_PatientStatus(NewEHRIDs);

                        }
                        else
                        {
                            SynchDataEasyDental_PatientStatus(string.Empty);
                        }
                    }

                    else
                    {

                        GoalBase.WriteToSyncLogFile_Static("ApptPatient Sync (" + Utility.Application_Name + " to Local Database) Successfully(Patient Not Found).");
                    }
                    IS_Appt_Patient_Sync = true;
                }
                catch (Exception ex)
                {
                    IS_Appt_Patient_Sync = true;
                    GoalBase.WriteToErrorLogFile_Static("[ApptPatient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }



        #endregion

        #region Synch OperatoryEvent

        private void fncSynchDataEasyDental_OperatoryEvent()
        {
            InitBgWorkerEasyDental_OperatoryEvent();
            InitBgTimerEasyDental_OperatoryEvent();
        }

        private void InitBgTimerEasyDental_OperatoryEvent()
        {
            timerSynchEasyDental_OperatoryEvent = new System.Timers.Timer();
            this.timerSynchEasyDental_OperatoryEvent.Interval = 1000 * GoalBase.intervalEHRSynch_OperatoryEvent;
            this.timerSynchEasyDental_OperatoryEvent.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_OperatoryEvent_Tick);
            timerSynchEasyDental_OperatoryEvent.Enabled = true;
            timerSynchEasyDental_OperatoryEvent.Start();
            timerSynchEasyDental_OperatoryEvent_Tick(null, null);
        }

        private void InitBgWorkerEasyDental_OperatoryEvent()
        {
            bwSynchEasyDental_OperatoryEvent = new BackgroundWorker();
            bwSynchEasyDental_OperatoryEvent.WorkerReportsProgress = true;
            bwSynchEasyDental_OperatoryEvent.WorkerSupportsCancellation = true;
            bwSynchEasyDental_OperatoryEvent.DoWork += new DoWorkEventHandler(bwSynchEasyDental_OperatoryEvent_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEasyDental_OperatoryEvent.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_OperatoryEvent_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_OperatoryEvent_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_OperatoryEvent.Enabled = false;
                MethodForCallSynchOrderEasyDental_OperatoryEvent();
            }
        }

        public void MethodForCallSynchOrderEasyDental_OperatoryEvent()
        {
            System.Threading.Thread procThreadmainEasyDental_OperatoryEvent = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_OperatoryEvent);
            procThreadmainEasyDental_OperatoryEvent.Start();
        }

        public void CallSyncOrderTableEasyDental_OperatoryEvent()
        {
            if (bwSynchEasyDental_OperatoryEvent.IsBusy != true)
            {
                bwSynchEasyDental_OperatoryEvent.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_OperatoryEvent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_OperatoryEvent.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_OperatoryEvent();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_OperatoryEvent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_OperatoryEvent.Enabled = true;
        }

        public void SynchDataEasyDental_OperatoryEvent()
        {
            if (IsEasyDentalOperatorySync && Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtEasyDentalOperatoryEvent = SynchEasyDentalBAL.GetEasyDentalOperatoryEventData();
                    dtEasyDentalOperatoryEvent.Columns.Add("OE_LocalDB_ID", typeof(int));
                    dtEasyDentalOperatoryEvent.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalOperatoryEvent = SynchLocalBAL.GetLocalOperatoryEventData("1");

                    foreach (DataRow dtDtxRow in dtEasyDentalOperatoryEvent.Rows)
                    {

                        DataRow[] row = dtLocalOperatoryEvent.Copy().Select("OE_EHR_ID = '" + dtDtxRow["event_id"].ToString().Trim() + "' ");
                        if (row.Length > 0)
                        {
                            if (Convert.ToDateTime(dtDtxRow["modified_time_stamp"].ToString().Trim()) != Convert.ToDateTime(row[0]["Entry_DateTime"]))
                            {
                                GoalBase.WriteToErrorLogFile_Static("[OperatoryEvent log modified_time_stamp " + dtDtxRow["modified_time_stamp"].ToString().Trim() + " : " + row[0]["Entry_DateTime"]);
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

                        DataRow[] rowBlcOpt = dtEasyDentalOperatoryEvent.Copy().Select("event_id = '" + dtLOERow["OE_EHR_ID"].ToString().Trim() + "' ");
                        if (rowBlcOpt.Length > 0)
                        {
                            DateTime OE_Date;
                            try
                            {
                                OE_Date = Convert.ToDateTime(rowBlcOpt[0]["event_date"].ToString());
                            }
                            catch (Exception)
                            {
                                GoalBase.WriteToErrorLogFile_Static("[OperatoryEvent log Deleted InsUptDlt_3 ]");
                                DataRow BlcOptDtldr = dtEasyDentalOperatoryEvent.NewRow();
                                BlcOptDtldr["event_id"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                                BlcOptDtldr["InsUptDlt"] = 3;
                                dtEasyDentalOperatoryEvent.Rows.Add(BlcOptDtldr);
                            }
                        }
                        else
                        {
                            GoalBase.WriteToErrorLogFile_Static("[OperatoryEvent log Deleted InsUptDlt_3_else ]");
                            DataRow BlcOptDtldr = dtEasyDentalOperatoryEvent.NewRow();
                            BlcOptDtldr["event_id"] = dtLOERow["OE_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtEasyDentalOperatoryEvent.Rows.Add(BlcOptDtldr);
                        }
                    }


                    dtEasyDentalOperatoryEvent.AcceptChanges();

                    if (dtEasyDentalOperatoryEvent != null && dtEasyDentalOperatoryEvent.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_OperatoryEvent_EasyDental_To_Local(dtEasyDentalOperatoryEvent);

                        if (status)
                        {
                            GoalBase.WriteToSyncLogFile_Static("OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
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
                    GoalBase.WriteToErrorLogFile_Static("[OperatoryEvent Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Provider

        private void fncSynchDataEasyDental_Provider()
        {
            InitBgWorkerEasyDental_Provider();
            InitBgTimerEasyDental_Provider();
        }

        private void InitBgTimerEasyDental_Provider()
        {
            timerSynchEasyDental_Provider = new System.Timers.Timer();
            this.timerSynchEasyDental_Provider.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchEasyDental_Provider.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_Provider_Tick);
            timerSynchEasyDental_Provider.Enabled = true;
            timerSynchEasyDental_Provider.Start();
        }

        private void InitBgWorkerEasyDental_Provider()
        {
            bwSynchEasyDental_Provider = new BackgroundWorker();
            bwSynchEasyDental_Provider.WorkerReportsProgress = true;
            bwSynchEasyDental_Provider.WorkerSupportsCancellation = true;
            bwSynchEasyDental_Provider.DoWork += new DoWorkEventHandler(bwSynchEasyDental_Provider_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEasyDental_Provider.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_Provider_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_Provider_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_Provider.Enabled = false;
                MethodForCallSynchOrderEasyDental_Provider();
            }
        }

        public void MethodForCallSynchOrderEasyDental_Provider()
        {
            System.Threading.Thread procThreadmainEasyDental_Provider = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_Provider);
            procThreadmainEasyDental_Provider.Start();
        }

        public void CallSyncOrderTableEasyDental_Provider()
        {
            if (bwSynchEasyDental_Provider.IsBusy != true)
            {
                bwSynchEasyDental_Provider.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_Provider_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_Provider.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_Provider();
                CommonFunction.GetMasterSync();

                //if (Utility.Application_Version.ToLower() != "DTX G5".ToLower())
                //{
                //    SynchDataEasyDental_ProviderOfficeHours();
                //    SynchDataEasyDental_ProviderHours();
                //}
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_Provider_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_Provider.Enabled = true;
        }

        public static void SynchDataEasyDental_Provider()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtEasyDentalProvider = SynchEasyDentalBAL.GetEasyDentalProviderData();
                    dtEasyDentalProvider.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", "1");

                    foreach (DataRow dtDtxRow in dtEasyDentalProvider.Rows)
                    {
                        dtDtxRow["is_active"] = dtDtxRow["is_active"].ToString() == "True" ? 0 : 1;
                        DataRow[] row = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dtDtxRow["Provider_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Last_Name"].ToString().Trim() != row[0]["Last_Name"].ToString().Trim())
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Providers log Last_Name " + dtDtxRow["Last_Name"].ToString().Trim() + " : " + row[0]["Last_Name"]);
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["First_Name"].ToString().Trim() != row[0]["First_Name"].ToString().Trim())
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Providers log First_Name " + dtDtxRow["First_Name"].ToString().Trim() + " : " + row[0]["First_Name"]);
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["mi"].ToString().Trim() != row[0]["MI"].ToString().Trim())
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Providers log mi " + dtDtxRow["mi"].ToString().Trim() + " : " + row[0]["MI"]);
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToBoolean(dtDtxRow["is_active"].ToString().Trim()) != Convert.ToBoolean(row[0]["is_active"]))
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Providers log is_active " + dtDtxRow["is_active"].ToString().Trim() + " : " + row[0]["is_active"]);
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

                    dtEasyDentalProvider.AcceptChanges();

                    if (dtEasyDentalProvider != null && dtEasyDentalProvider.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_Provider_EasyDental_To_Local(dtEasyDentalProvider);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Provider");
                            GoalBase.WriteToSyncLogFile_Static("Providers Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsEasyDentalProviderSync = true;
                            SynchDataLiveDB_Push_Provider();
                        }
                        else
                        {
                            IsEasyDentalProviderSync = false;
                        }
                    }
                    IsProviderSyncedFirstTime = true;
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[Provider Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Sync Provider Hours & Office Hours

        private void fncSynchDataEasyDental_ProviderOfficeHours()
        {
            InitBgWorkerEasyDental_ProviderOfficeHours();
            InitBgTimerEasyDental_ProviderOfficeHours();
        }

        private void InitBgTimerEasyDental_ProviderOfficeHours()
        {
            timerSynchEasyDental_ProviderOfficeHours = new System.Timers.Timer();
            this.timerSynchEasyDental_ProviderOfficeHours.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchEasyDental_ProviderOfficeHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_ProviderOfficeHours_Tick);
            timerSynchEasyDental_ProviderOfficeHours.Enabled = true;
            timerSynchEasyDental_ProviderOfficeHours.Start();
        }

        private void InitBgWorkerEasyDental_ProviderOfficeHours()
        {
            bwSynchEasyDental_ProviderOfficeHours = new BackgroundWorker();
            bwSynchEasyDental_ProviderOfficeHours.WorkerReportsProgress = true;
            bwSynchEasyDental_ProviderOfficeHours.WorkerSupportsCancellation = true;
            bwSynchEasyDental_ProviderOfficeHours.DoWork += new DoWorkEventHandler(bwSynchEasyDental_ProviderOfficeHours_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEasyDental_ProviderOfficeHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_ProviderOfficeHours_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_ProviderOfficeHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_ProviderOfficeHours.Enabled = false;
                MethodForCallSynchOrderEasyDental_ProviderOfficeHours();
            }
        }

        public void MethodForCallSynchOrderEasyDental_ProviderOfficeHours()
        {
            System.Threading.Thread procThreadmainEasyDental_ProviderOfficeHours = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_ProviderOfficeHours);
            procThreadmainEasyDental_ProviderOfficeHours.Start();
        }

        public void CallSyncOrderTableEasyDental_ProviderOfficeHours()
        {
            if (bwSynchEasyDental_ProviderOfficeHours.IsBusy != true)
            {
                bwSynchEasyDental_ProviderOfficeHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_ProviderOfficeHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_ProviderOfficeHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                if (Utility.Application_Version.ToLower() != "DTX G5".ToLower())
                {
                    SynchDataEasyDental_ProviderOfficeHours();
                    SynchDataEasyDental_ProviderHours();
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_ProviderOfficeHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_ProviderOfficeHours.Enabled = true;
        }

        public void SynchDataEasyDental_ProviderOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtEasyDentalProviderOfficeHours = SynchEasyDentalBAL.GetEasyDentalProviderOfficeHours();
                    DataTable dtEasyDentalLocalProviderOfficeHours = SynchLocalBAL.GetLocalProviderOfficeHours("1");
                    DataTable dtEasyDentalProvider = SynchEasyDentalBAL.GetEasyDentalProviderData();

                    dtEasyDentalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtEasyDentalLocalProviderOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtEasyDentalLocalProviderOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtEasyDentalLocalProviderOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtEasyDentalLocalProviderOfficeHours = CompareDataTableRecords(ref dtEasyDentalProviderOfficeHours, dtEasyDentalLocalProviderOfficeHours, "POH_EHR_ID", "POH_LocalDB_ID", "POH_LocalDB_ID,POH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtEasyDentalProviderOfficeHours.AcceptChanges();
                    dtEasyDentalLocalProviderOfficeHours.AcceptChanges();

                    if ((dtEasyDentalProviderOfficeHours != null && dtEasyDentalProviderOfficeHours.Rows.Count > 0) || (dtEasyDentalLocalProviderOfficeHours != null && dtEasyDentalLocalProviderOfficeHours.Rows.Count > 0))
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtEasyDentalProviderOfficeHours.Clone();
                        if (dtEasyDentalProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtEasyDentalLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtEasyDentalProviderOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalProviderOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtEasyDentalLocalProviderOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalLocalProviderOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "ProviderOfficeHours", "POH_LocalDB_ID,POH_Web_ID", "POH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtEasyDentalProviderOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderOfficeHours");
                            GoalBase.WriteToSyncLogFile_Static("ProviderOfficeHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_ProviderOfficeHours();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[ProviderOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        public void SynchDataEasyDental_ProviderHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtEasyDentalProviderHours = SynchEasyDentalBAL.GetEasyDentalProviderHoursData();
                    dtEasyDentalProviderHours.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalProviderHours = SynchLocalBAL.GetLocalProviderHoursData("1");

                    foreach (DataRow dtDtxRow in dtEasyDentalProviderHours.Rows)
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

                    dtEasyDentalProviderHours.AcceptChanges();


                    foreach (DataRow dtLPHRow in dtLocalProviderHours.Rows)
                    {
                        DataRow[] rowBlcOpt = dtEasyDentalProviderHours.Copy().Select("PH_EHR_ID = '" + dtLPHRow["PH_EHR_ID"].ToString().Trim() + "'");
                        if (rowBlcOpt.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtEasyDentalProviderHours.NewRow();
                            BlcOptDtldr["PH_EHR_ID"] = dtLPHRow["PH_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtEasyDentalProviderHours.Rows.Add(BlcOptDtldr);
                        }
                    }

                    if (dtEasyDentalProviderHours != null && dtEasyDentalProviderHours.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_ProviderHours_EasyDental_To_Local(dtEasyDentalProviderHours);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ProviderHours");
                            GoalBase.WriteToSyncLogFile_Static("ProviderHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_ProviderHours();
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Is_synched_Provider = false;
                GoalBase.WriteToErrorLogFile_Static("[ProviderHours Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Speciality

        private void fncSynchDataEasyDental_Speciality()
        {
            InitBgWorkerEasyDental_Speciality();
            InitBgTimerEasyDental_Speciality();
        }

        private void InitBgTimerEasyDental_Speciality()
        {
            timerSynchEasyDental_Speciality = new System.Timers.Timer();
            this.timerSynchEasyDental_Speciality.Interval = 1000 * GoalBase.intervalEHRSynch_Speciality;
            this.timerSynchEasyDental_Speciality.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_Speciality_Tick);
            timerSynchEasyDental_Speciality.Enabled = true;
            timerSynchEasyDental_Speciality.Start();
        }

        private void InitBgWorkerEasyDental_Speciality()
        {
            bwSynchEasyDental_Speciality = new BackgroundWorker();
            bwSynchEasyDental_Speciality.WorkerReportsProgress = true;
            bwSynchEasyDental_Speciality.WorkerSupportsCancellation = true;
            bwSynchEasyDental_Speciality.DoWork += new DoWorkEventHandler(bwSynchEasyDental_Speciality_DoWork);
            bwSynchEasyDental_Speciality.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_Speciality_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_Speciality_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_Speciality.Enabled = false;
                MethodForCallSynchOrderEasyDental_Speciality();
            }
        }

        public void MethodForCallSynchOrderEasyDental_Speciality()
        {
            System.Threading.Thread procThreadmainEasyDental_Speciality = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_Speciality);
            procThreadmainEasyDental_Speciality.Start();
        }

        public void CallSyncOrderTableEasyDental_Speciality()
        {
            if (bwSynchEasyDental_Speciality.IsBusy != true)
            {
                bwSynchEasyDental_Speciality.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_Speciality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_Speciality.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }


            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_Speciality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_Speciality.Enabled = true;
        }

        public static void SynchDataEasyDental_Speciality()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtEasyDentalSpeciality = SynchEasyDentalBAL.GetEasyDentalSpecialityData();

                    dtEasyDentalSpeciality.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalSpeciality.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalSpeciality = SynchLocalBAL.GetLocalSpecialityData("1");
                    if (!dtLocalSpeciality.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalSpeciality.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalSpeciality.Columns["InsUptDlt"].DefaultValue = 0;
                    }


                    dtLocalSpeciality = CompareEasyDentalDataTableRecords(ref dtEasyDentalSpeciality, dtLocalSpeciality, "Speciality_Name", "Speciality_LocalDB_ID", "Speciality_LocalDB_ID,Speciality_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,EHR_Entry_DateTime,Clinic_Number,Service_Install_Id");

                    dtEasyDentalSpeciality.AcceptChanges();

                    if (dtEasyDentalSpeciality != null && dtEasyDentalSpeciality.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtEasyDentalSpeciality.Clone();
                        if (dtEasyDentalSpeciality.Select("InsUptDlt IN (1,2)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtEasyDentalSpeciality.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Speciality", "Speciality_LocalDB_ID,Speciality_Web_ID", "Speciality_LocalDB_ID");
                        }
                        else
                        {
                            if (dtEasyDentalSpeciality.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Speciality");
                            GoalBase.WriteToSyncLogFile_Static("Speciality Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Speciality();
                        }
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[Speciality Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        private static DataTable CompareEasyDentalDataTableRecords(ref DataTable dtSource, DataTable dtDestination, string compareColumnName, string primarykeyColumns, string ignoreColumns)
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
                                        if (result.First().Field<object>(dcCol.ColumnName) != null && string.IsNullOrEmpty(result.First().Field<object>(dcCol.ColumnName).ToString()) == false && o[dcCol.ColumnName].ToString().Trim().ToLower() != result.First().Field<object>(dcCol.ColumnName).ToString().Trim().ToLower())
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


        #endregion

        #region Synch Operatory

        private void fncSynchDataEasyDental_Operatory()
        {
            InitBgWorkerEasyDental_Operatory();
            InitBgTimerEasyDental_Operatory();
        }

        private void InitBgTimerEasyDental_Operatory()
        {
            timerSynchEasyDental_Operatory = new System.Timers.Timer();
            this.timerSynchEasyDental_Operatory.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchEasyDental_Operatory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_Operatory_Tick);
            timerSynchEasyDental_Operatory.Enabled = true;
            timerSynchEasyDental_Operatory.Start();
        }

        private void InitBgWorkerEasyDental_Operatory()
        {
            bwSynchEasyDental_Operatory = new BackgroundWorker();
            bwSynchEasyDental_Operatory.WorkerReportsProgress = true;
            bwSynchEasyDental_Operatory.WorkerSupportsCancellation = true;
            bwSynchEasyDental_Operatory.DoWork += new DoWorkEventHandler(bwSynchEasyDental_Operatory_DoWork);
            bwSynchEasyDental_Operatory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_Operatory_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_Operatory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_Operatory.Enabled = false;
                MethodForCallSynchOrderEasyDental_Operatory();
            }
        }

        public void MethodForCallSynchOrderEasyDental_Operatory()
        {
            System.Threading.Thread procThreadmainEasyDental_Operatory = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_Operatory);
            procThreadmainEasyDental_Operatory.Start();
        }

        public void CallSyncOrderTableEasyDental_Operatory()
        {
            if (bwSynchEasyDental_Operatory.IsBusy != true)
            {
                bwSynchEasyDental_Operatory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_Operatory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_Operatory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_Operatory();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_Operatory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_Operatory.Enabled = true;
        }

        public static void SynchDataEasyDental_Operatory()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {

                try
                {
                    DataTable dtEasyDentalOperatory = SynchEasyDentalBAL.GetEasyDentalOperatoryData();
                    dtEasyDentalOperatory.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalOperatory = SynchLocalBAL.GetLocalOperatoryData("1");
                    DataView dv = dtEasyDentalOperatory.DefaultView;
                    dv.Sort = "Operatory_EHR_ID asc";
                    DataTable sortedDT = dv.ToTable();
                    int i = 0;
                    dtEasyDentalOperatory = sortedDT;
                    foreach (DataRow dtDtxRow in dtEasyDentalOperatory.Rows)
                    {
                        i = i + 1;
                        dtDtxRow["OperatoryOrder"] = i;
                        DataRow[] row = dtLocalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtDtxRow["Operatory_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Operatory_Name"].ToString().Trim() != row[0]["Operatory_Name"].ToString().Trim())
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Operatory log Operatory_Name " + dtDtxRow["Operatory_Name"].ToString().Trim() + " : " + row[0]["Operatory_Name"]);
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (dtDtxRow["OperatoryOrder"].ToString().Trim() != row[0]["OperatoryOrder"].ToString().Trim())
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Operatory log Operatory_Name " + dtDtxRow["OperatoryOrder"].ToString().Trim() + " : " + row[0]["OperatoryOrder"]);
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
                    //    DataRow[] row = dtEasyDentalOperatory.Copy().Select("op_id = '" + dtLocalRow["Operatory_EHR_ID"] + "'");
                    //    if (row.Length <= 0)
                    //    {
                    //        dtEasyDentalOperatory.Rows.Add(dtLocalRow["Operatory_EHR_ID"], dtLocalRow["Operatory_Name"], "D");
                    //    }
                    //}
                    foreach (DataRow dtRow in dtLocalOperatory.Rows)
                    {
                        DataRow rowBlcOpt = dtEasyDentalOperatory.Copy().Select("Operatory_EHR_ID = '" + dtRow["Operatory_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                        if (rowBlcOpt == null)
                        {
                            GoalBase.WriteToErrorLogFile_Static("[Operatory log Deleted InsUptDlt_3]");
                            DataRow ESApptDtldr = dtEasyDentalOperatory.NewRow();
                            ESApptDtldr["Operatory_EHR_ID"] = dtRow["Operatory_EHR_ID"].ToString().Trim();
                            ESApptDtldr["InsUptDlt"] = 3;
                            dtEasyDentalOperatory.Rows.Add(ESApptDtldr);
                        }
                    }

                    dtEasyDentalOperatory.AcceptChanges();

                    if (dtEasyDentalOperatory != null && dtEasyDentalOperatory.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_Operatory_EasyDental_To_Local(dtEasyDentalOperatory);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Operatory");
                            GoalBase.WriteToSyncLogFile_Static("Operatory Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsEasyDentalOperatorySync = true;

                            SynchDataLiveDB_Push_Operatory();
                        }
                        else
                        {
                            IsEasyDentalOperatorySync = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[Operatory Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch OperatoryHours

        private void fncSynchDataEasyDental_OperatoryHours()
        {
            InitBgWorkerEasyDental_OperatoryHours();
            InitBgTimerEasyDental_OperatoryHours();
        }

        private void InitBgTimerEasyDental_OperatoryHours()
        {
            timerSynchEasyDental_OperatoryHours = new System.Timers.Timer();
            this.timerSynchEasyDental_OperatoryHours.Interval = 1000 * GoalBase.intervalEHRSynch_Operatory;
            this.timerSynchEasyDental_OperatoryHours.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_OperatoryHours_Tick);
            timerSynchEasyDental_OperatoryHours.Enabled = true;
            timerSynchEasyDental_OperatoryHours.Start();
        }

        private void InitBgWorkerEasyDental_OperatoryHours()
        {
            bwSynchEasyDental_OperatoryHours = new BackgroundWorker();
            bwSynchEasyDental_OperatoryHours.WorkerReportsProgress = true;
            bwSynchEasyDental_OperatoryHours.WorkerSupportsCancellation = true;
            bwSynchEasyDental_OperatoryHours.DoWork += new DoWorkEventHandler(bwSynchEasyDental_OperatoryHours_DoWork);
            bwSynchEasyDental_OperatoryHours.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_OperatoryHours_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_OperatoryHours_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_OperatoryHours.Enabled = false;
                MethodForCallSynchOrderEasyDental_OperatoryHours();
            }
        }

        public void MethodForCallSynchOrderEasyDental_OperatoryHours()
        {
            System.Threading.Thread procThreadmainEasyDental_OperatoryHours = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_OperatoryHours);
            procThreadmainEasyDental_OperatoryHours.Start();
        }

        public void CallSyncOrderTableEasyDental_OperatoryHours()
        {
            if (bwSynchEasyDental_OperatoryHours.IsBusy != true)
            {
                bwSynchEasyDental_OperatoryHours.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_OperatoryHours_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_OperatoryHours.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }

                if (Utility.Application_Version.ToLower() != "DTX G5".ToLower())
                {
                    SynchDataEasyDental_OperatoryOfficeHours();
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_OperatoryHours_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_OperatoryHours.Enabled = true;
        }

        public void SynchDataEasyDental_OperatoryOfficeHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtEasyDentalOperatoryOfficeHours = SynchEasyDentalBAL.GetEasyDentalOperatoryOfficeHours();
                    DataTable dtEasyDentalLocalOperatoryOfficeHours = SynchLocalBAL.GetLocalOperatoryOfficeHoursData("1");
                    DataTable dtEasyDentalOperatory = SynchEasyDentalBAL.GetEasyDentalOperatoryData();

                    dtEasyDentalOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtEasyDentalLocalOperatoryOfficeHours.Columns.Contains("InsUptDlt"))
                    {
                        dtEasyDentalLocalOperatoryOfficeHours.Columns.Add("InsUptDlt", typeof(int));
                        dtEasyDentalLocalOperatoryOfficeHours.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtEasyDentalLocalOperatoryOfficeHours = CompareDataTableRecords(ref dtEasyDentalOperatoryOfficeHours, dtEasyDentalLocalOperatoryOfficeHours, "OOH_EHR_ID", "OOH_LocalDB_ID", "OOH_LocalDB_ID,OOH_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,Last_Sync_Date,is_deleted,Clinic_Number,Service_Install_Id");

                    dtEasyDentalOperatoryOfficeHours.AcceptChanges();

                    if (dtEasyDentalOperatoryOfficeHours != null && dtEasyDentalOperatoryOfficeHours.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtEasyDentalOperatoryOfficeHours.Clone();
                        if (dtEasyDentalOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0 || dtEasyDentalLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtEasyDentalOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalOperatoryOfficeHours.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtEasyDentalLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalLocalOperatoryOfficeHours.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "OperatoryOfficeHours", "OOH_LocalDB_ID,OOH_Web_ID", "OOH_LocalDB_ID");
                        }
                        else
                        {
                            if (dtEasyDentalOperatoryOfficeHours.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryOfficeHours");
                            GoalBase.WriteToSyncLogFile_Static("OperatoryOfficeHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_OperatoryOfficeHours();
                        }
                    }

                    SynchDataEasyDental_OperatoryHours();
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[OperatoryOfficeHours Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        public void SynchDataEasyDental_OperatoryHours()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable && Utility.is_scheduledCustomhour)
                {
                    DataTable dtEasyDentalOperatoryHours = SynchEasyDentalBAL.GetEasyDentalOperatoryHoursData();
                    dtEasyDentalOperatoryHours.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalOperatoryHours = SynchLocalBAL.GetLocalOperatoryHoursData("1");

                    foreach (DataRow dtDtxRow in dtEasyDentalOperatoryHours.Rows)
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
                        DataRow[] rowBlcOpt = dtEasyDentalOperatoryHours.Copy().Select("OH_EHR_ID = '" + dtLPHRow["OH_EHR_ID"].ToString().Trim() + "'");
                        if (rowBlcOpt.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtEasyDentalOperatoryHours.NewRow();
                            BlcOptDtldr["OH_EHR_ID"] = dtLPHRow["OH_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtEasyDentalOperatoryHours.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtEasyDentalOperatoryHours.AcceptChanges();
                    if (dtEasyDentalOperatoryHours != null && dtEasyDentalOperatoryHours.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_OperatoryHours_EasyDental_To_Local(dtEasyDentalOperatoryHours);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("OperatoryHours");
                            GoalBase.WriteToSyncLogFile_Static("OperatoryHours Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_OperatoryHours();
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Is_synched_Operatory = false;
                GoalBase.WriteToErrorLogFile_Static("[OperatoryHours Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Appointment Type

        private void fncSynchDataEasyDental_ApptType()
        {
            InitBgWorkerEasyDental_ApptType();
            InitBgTimerEasyDental_ApptType();
        }

        private void InitBgTimerEasyDental_ApptType()
        {
            timerSynchEasyDental_ApptType = new System.Timers.Timer();
            this.timerSynchEasyDental_ApptType.Interval = 1000 * GoalBase.intervalEHRSynch_ApptType;
            this.timerSynchEasyDental_ApptType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_ApptType_Tick);
            timerSynchEasyDental_ApptType.Enabled = true;
            timerSynchEasyDental_ApptType.Start();
        }

        private void InitBgWorkerEasyDental_ApptType()
        {
            bwSynchEasyDental_ApptType = new BackgroundWorker();
            bwSynchEasyDental_ApptType.WorkerReportsProgress = true;
            bwSynchEasyDental_ApptType.WorkerSupportsCancellation = true;
            bwSynchEasyDental_ApptType.DoWork += new DoWorkEventHandler(bwSynchEasyDental_ApptType_DoWork);
            bwSynchEasyDental_ApptType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_ApptType_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_ApptType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_ApptType.Enabled = false;
                MethodForCallSynchOrderEasyDental_ApptType();
            }
        }

        public void MethodForCallSynchOrderEasyDental_ApptType()
        {
            System.Threading.Thread procThreadmainEasyDental_ApptType = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_ApptType);
            procThreadmainEasyDental_ApptType.Start();
        }

        public void CallSyncOrderTableEasyDental_ApptType()
        {
            if (bwSynchEasyDental_ApptType.IsBusy != true)
            {
                bwSynchEasyDental_ApptType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_ApptType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_ApptType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_ApptType();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_ApptType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_ApptType.Enabled = true;
        }

        public static void SynchDataEasyDental_ApptType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {

                    DataTable dtEasyDentalApptType = SynchEasyDentalBAL.GetEasyDentalApptTypeData();
                    dtEasyDentalApptType.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalApptType.Rows.Add(0, "<none>");
                    DataTable dtLocalApptType = SynchLocalBAL.GetLocalApptTypeData("1");

                    foreach (DataRow dtDtxRow in dtEasyDentalApptType.Rows)
                    {
                        DataRow[] row = dtLocalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["Type_Name"].ToString().Trim() != row[0]["Type_Name"].ToString().Trim())
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Appointment Type log Type_Name " + dtDtxRow["Type_Name"].ToString().Trim() + " : " + row[0]["Type_Name"]);
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
                        DataRow[] row = dtEasyDentalApptType.Copy().Select("ApptType_EHR_ID = '" + dtDtxRow["ApptType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtEasyDentalApptType.NewRow();
                            BlcOptDtldr["ApptType_EHR_ID"] = dtDtxRow["ApptType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["Type_Name"] = dtDtxRow["Type_Name"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtEasyDentalApptType.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtEasyDentalApptType.AcceptChanges();

                    if (dtEasyDentalApptType != null && dtEasyDentalApptType.Rows.Count > 0)
                    {
                        bool Type = SynchEasyDentalBAL.Save_ApptType_EasyDental_To_Local(dtEasyDentalApptType);

                        if (Type)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptType");
                            GoalBase.WriteToSyncLogFile_Static("Appointment Type Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            IsEasyDentalApptTypeSync = true;

                            SynchDataLiveDB_Push_ApptType();
                        }
                        else
                        {
                            IsEasyDentalApptTypeSync = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[Appointment_Type Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Patient

        private void fncSynchDataEasyDental_Patient()
        {
            InitBgWorkerEasyDental_Patient();
            InitBgTimerEasyDental_Patient();
        }

        private void InitBgTimerEasyDental_Patient()
        {
            timerSynchEasyDental_Patient = new System.Timers.Timer();
            this.timerSynchEasyDental_Patient.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchEasyDental_Patient.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_Patient_Tick);
            timerSynchEasyDental_Patient.Enabled = true;
            timerSynchEasyDental_Patient.Start();
            timerSynchEasyDental_Patient_Tick(null, null);
        }

        private void InitBgWorkerEasyDental_Patient()
        {
            bwSynchEasyDental_Patient = new BackgroundWorker();
            bwSynchEasyDental_Patient.WorkerReportsProgress = true;
            bwSynchEasyDental_Patient.WorkerSupportsCancellation = true;
            bwSynchEasyDental_Patient.DoWork += new DoWorkEventHandler(bwSynchEasyDental_Patient_DoWork);
            bwSynchEasyDental_Patient.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_Patient_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_Patient_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_Patient.Enabled = false;
                MethodForCallSynchOrderEasyDental_Patient();
            }
        }

        public void MethodForCallSynchOrderEasyDental_Patient()
        {
            System.Threading.Thread procThreadmainEasyDental_Patient = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_Patient);
            procThreadmainEasyDental_Patient.Start();
        }

        public void CallSyncOrderTableEasyDental_Patient()
        {
            if (bwSynchEasyDental_Patient.IsBusy != true)
            {
                bwSynchEasyDental_Patient.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_Patient_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_Patient.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_Patient();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_Patient_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_Patient.Enabled = true;
        }

        //public void SynchDataEasyDental_Patient()
        //{
        //    if (Utility.IsApplicationIdleTimeOff && IsParientFirstSync)
        //    {
        //        try
        //        {
        //            IsParientFirstSync = false;
        //            DataTable dtEasyDentalPatientNextApptDate = SynchEasyDentalBAL.GetEasyDentalPatientNextApptDate();
        //            DataTable dtEasyDentalPatient = SynchEasyDentalBAL.GetEasyDentalPatientData();
        //            DataTable dtEasyDentalPatientdue_date = SynchEasyDentalBAL.GetEasyDentalPatientdue_date();
        //            DataTable dtEasyDentalPatientcollect_payment = SynchEasyDentalBAL.GetEasyDentalPatientcollect_payment();

        //            dtEasyDentalPatient.Columns.Add("InsUptDlt", typeof(int));
        //            dtEasyDentalPatient.Columns.Add("Patientcollect_payment", typeof(string));

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

        //            TotalPatientRecord = dtEasyDentalPatient.Rows.Count;
        //            GetPatientRecord = 0;

        //            foreach (DataRow dtDtxRow in dtEasyDentalPatient.Rows)
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
        //                dtDtxRow["nextvisit_date"] = Utility.SetNextVisitDate(dtEasyDentalPatientNextApptDate, "patid", "Patient_EHR_ID", "nextvisit_date", dtDtxRow["Patient_EHR_ID"].ToString());
        //                tmpnextvisit_date = dtDtxRow["nextvisit_date"].ToString();

        //                try
        //                {
        //                    DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dtDtxRow["Patient_EHR_ID"] + "'");

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

        //                    DataRow[] Patdue_date = dtEasyDentalPatientdue_date.Copy().Select("patient_id = '" + dtDtxRow["Patient_EHR_ID"] + "'");
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

        //            dtEasyDentalPatient.AcceptChanges();

        //            if (dtEasyDentalPatient != null && dtEasyDentalPatient.Rows.Count > 0)
        //            {
        //                bool isPatientSave = SynchEasyDentalBAL.Save_Patient_EasyDental_To_Local(dtEasyDentalPatient);
        //                if (isPatientSave)
        //                {
        //                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
        //                     GoalBase.WriteToSyncLogFile_Static("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
        //                    IsGetParientRecordDone = true;

        //                    SynchDataLiveDB_Push_Patient();
        //                }
        //            }
        //            else
        //            {
        //                 GoalBase.WriteToSyncLogFile_Static("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
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
        //             GoalBase.WriteToErrorLogFile_Static("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
        //        }
        //    }
        //}

        public static void SynchDataEasyDental_Patient()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    SynchDataLiveDB_Pull_EHR_Patientoptout();
                    DataTable dtEasyDentalPatient = SynchEasyDentalBAL.GetEasyDentalPatientData();

                    DataTable dtEasyDentalPatientNextApptDate = SynchEasyDentalBAL.GetEasyDentalPatientNextApptDate();
                    DataTable dtEasyDentalPatientdue_date = SynchEasyDentalBAL.GetEasyDentalPatientdue_date();


                    // DataTable dtEasyDentalPatientcollect_payment = SynchEasyDentalBAL.GetEasyDentalPatientcollect_payment();

                    //dtEasyDentalPatient.Columns.Add("InsUptDlt", typeof(int));
                    //dtEasyDentalPatient.Columns.Add("collect_payment", typeof(string));

                    string patientTableName = "Patient";

                    DataTable dtLocalPatient = SynchLocalBAL.GetLocalPatientData("1");

                    if (dtLocalPatient != null && dtLocalPatient.Rows.Count > 0)
                    {
                        patientTableName = "PatientCompare";
                    }



                    if (dtEasyDentalPatient != null && dtEasyDentalPatient.Rows.Count > 0)
                    {
                        bool isPatientSave = SynchEasyDentalBAL.Save_Patient_EasyDental_To_Local(dtEasyDentalPatient, patientTableName, dtEasyDentalPatientNextApptDate, dtEasyDentalPatientdue_date, dtLocalPatient, true);
                        if (isPatientSave)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                            GoalBase.WriteToSyncLogFile_Static("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Patient();
                        }
                    }
                    else
                    {
                        GoalBase.WriteToSyncLogFile_Static("Patient Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient");
                        bool UpdateSync_TablePush_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Push");
                    }
                    SynchDataEasyDental_PatientStatus(string.Empty);
                    SynchDataEasyDental_PatientImages();
                    SynchDataEasyDental_PatientMedication("");
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[Patient Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        public static void SynchDataEasyDental_PatientStatus(string PatientEHRIDs)
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtEasyDentalPatient = SynchEasyDentalBAL.GetEasyDentalPatientStatusData(string.Empty);
                    DataTable dtEasyDentalAppointment = SynchEasyDentalBAL.GetEasyDentalPatientStatusAppointmentData(PatientEHRIDs);
                    var result = dtEasyDentalPatient.AsEnumerable().Select(o => Convert.ToInt32(o.Field<object>("Patient_EHR_ID"))).Except(dtEasyDentalAppointment.AsEnumerable().Select(b => Convert.ToInt32(b.Field<object>("Patientid"))).Distinct()).ToArray();
                    DataTable Result = CreateDataTable(result);
                    if (Result != null && Result.Rows.Count > 0)
                    {
                        SynchLocalBAL.UpdatePatient_Status(Result, "1");
                    }
                    bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("PatientStatus");
                    GoalBase.WriteToSyncLogFile_Static("PatientStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                    SynchDataLiveDB_Push_PatientStatus();
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[PatientStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
                //dtEasyDentalAppointment.AsEnumerable().Except(dtEasyDentalPatient.AsEnumerable(),                  

            }
        }

        public static DataTable CreateDataTable(IEnumerable source)
        {
            var table = new DataTable();
            int index = 0;
            var properties = new List<PropertyInfo>();
            foreach (var obj in source)
            {
                if (index == 0)
                {
                    table.Columns.Add(new DataColumn("Patient_EHR_ID", typeof(Int32)));
                }
                table.Rows.Add(obj);
                index++;
            }
            return table;
        }

        public static string GetEasyDentalDataPath()
        {
            string patientPicPath = "";
            try
            {
                RegistryKey hKey = Registry.CurrentUser.OpenSubKey(@"Software\Easy Dental Systems, Inc.\Easy Dental\General");
                if (hKey != null)
                {
                    Object value = hKey.GetValue("Path");
                    if (value != null)
                    {
                        patientPicPath = value.ToString() + "PATPICTS\\";
                    }
                }
                return patientPicPath;
            }
            catch (Exception)
            {
                return "C:\\EzDental\\DATA\\PATPICTS\\";
            }
            // return retv;
        }


        public static void SynchDataEasyDental_PatientImages()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    // SynchDataLiveDB_Push_PatientImage();

                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtEasyDentalPatientImages = SynchEasyDentalBAL.GetEasyDentalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), GetEasyDentalDataPath());
                        dtEasyDentalPatientImages.Columns.Add("InsUptDlt", typeof(int));
                        //dtEasyDentalPatientImages.Columns.Add("SourceLocation", typeof(string));
                        dtEasyDentalPatientImages.Columns["InsUptDlt"].DefaultValue = 0;
                        DataTable dtLocalPatientImages = SynchLocalBAL.GetLocalPatientImagesData(Utility.DtInstallServiceList.Rows[j]["Installation_Id"].ToString());
                        //Utility.EHRProfileImagePath = GetEasyDentalDataPath();//SynchEasyDentalDAL.GetEasyDentalDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());
                        foreach (DataRow dtDtxRow in dtEasyDentalPatientImages.Rows)
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
                                    else if (Convert.ToDateTime(dtDtxRow["Entry_DateTime"]) != Convert.ToDateTime(row[0]["Entry_DateTime"]))
                                    {
                                        dtDtxRow["InsUptDlt"] = 2;
                                    }
                                    else if (dtDtxRow["Patient_Images_FilePath"].ToString() != row[0]["Patient_Images_FilePath"].ToString())
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
                            DataRow[] row = dtEasyDentalPatientImages.Copy().Select("Patient_EHR_ID = '" + dtDtlRow["Patient_EHR_ID"].ToString().Trim() + "' ");
                            if (row.Length <= 0)
                            {
                                if (!Convert.ToBoolean(dtDtlRow["Is_Deleted"]))
                                {
                                    DataRow ApptDtldr = dtEasyDentalPatientImages.NewRow();
                                    ApptDtldr["Patient_EHR_ID"] = dtDtlRow["Patient_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Patient_Images_EHR_ID"] = dtDtlRow["Patient_Images_EHR_ID"].ToString().Trim();
                                    ApptDtldr["Image_EHR_Name"] = dtDtlRow["Image_EHR_Name"].ToString().Trim();
                                    ApptDtldr["Clinic_Number"] = dtDtlRow["Clinic_Number"].ToString().Trim();
                                    ApptDtldr["Service_Install_Id"] = dtDtlRow["Service_Install_Id"].ToString().Trim();
                                    ApptDtldr["InsUptDlt"] = 3;
                                    ApptDtldr["Is_Deleted"] = 1;
                                    dtEasyDentalPatientImages.Rows.Add(ApptDtldr);
                                }

                            }
                        }

                        dtEasyDentalPatientImages.AcceptChanges();
                        bool status = false;
                        DataTable dtSaveRecords = dtEasyDentalPatientImages.Clone();
                        if (dtEasyDentalPatientImages.Select("InsUptDlt IN (1,2,3)").Count() > 0)
                        {
                            dtSaveRecords.Load(dtEasyDentalPatientImages.Select("InsUptDlt IN (1,2,3)").CopyToDataTable().CreateDataReader());
                            status = SynchLocalBAL.Save_PatientProfileImage_EHR_To_Local(dtSaveRecords, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient_Images");
                            GoalBase.WriteToSyncLogFile_Static("PatientImage Sync (" + Utility.Application_Name + " to Local Database) Successfully.");


                        }
                        SynchDataLiveDB_Push_PatientImage(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PatientImages Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        #endregion

        #region Synch Disease

        private void fncSynchDataEasyDental_Disease()
        {
            InitBgWorkerEasyDental_Disease();
            InitBgTimerEasyDental_Disease();
        }

        private void InitBgTimerEasyDental_Disease()
        {
            timerSynchEasyDental_Disease = new System.Timers.Timer();
            this.timerSynchEasyDental_Disease.Interval = 1000 * GoalBase.intervalEHRSynch_Patient;
            this.timerSynchEasyDental_Disease.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_Disease_Tick);
            timerSynchEasyDental_Disease.Enabled = true;
            timerSynchEasyDental_Disease.Start();
        }

        private void InitBgWorkerEasyDental_Disease()
        {
            bwSynchEasyDental_Disease = new BackgroundWorker();
            bwSynchEasyDental_Disease.WorkerReportsProgress = true;
            bwSynchEasyDental_Disease.WorkerSupportsCancellation = true;
            bwSynchEasyDental_Disease.DoWork += new DoWorkEventHandler(bwSynchEasyDental_Disease_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEasyDental_Disease.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_Disease_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_Disease_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_Disease.Enabled = false;
                MethodForCallSynchOrderEasyDental_Disease();
            }
        }

        public void MethodForCallSynchOrderEasyDental_Disease()
        {
            System.Threading.Thread procThreadmainEasyDental_Disease = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_Disease);
            procThreadmainEasyDental_Disease.Start();
        }

        public void CallSyncOrderTableEasyDental_Disease()
        {
            if (bwSynchEasyDental_Disease.IsBusy != true)
            {
                bwSynchEasyDental_Disease.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_Disease_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_Disease.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_Disease();
                //  SynchDataEasyDental_DiseaseHours();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_Disease_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_Disease.Enabled = true;
        }

        public static void SynchDataEasyDental_Disease()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {

                    DataTable dtEasyDentalDisease = SynchEasyDentalBAL.GetEasyDentalDiseaseData();
                    dtEasyDentalDisease.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalDisease = SynchLocalBAL.GetLocalDiseaseData("1");

                    foreach (DataRow dtDtxRow in dtEasyDentalDisease.Rows)
                    {
                        DataRow[] row = dtLocalDisease.Copy().Select("Disease_EHR_ID = '" + dtDtxRow["Disease_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {

                            if (dtDtxRow["Disease_Name"].ToString().Trim() != row[0]["Disease_Name"].ToString().Trim())
                            {
                                GoalBase.WriteToErrorLogFile_Static("[Disease log Disease_Name " + dtDtxRow["Disease_Name"].ToString().Trim() + " : " + row[0]["Disease_Name"]);
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
                        DataRow rowBlcOpt = dtEasyDentalDisease.Copy().Select("Disease_EHR_ID = '" + dtRow["Disease_EHR_ID"].ToString().Trim() + "' ").FirstOrDefault();
                        if (rowBlcOpt == null)
                        {
                            GoalBase.WriteToErrorLogFile_Static("[Disease log Deleted  InsUptDlt _3]");
                            DataRow dtxtldr = dtEasyDentalDisease.NewRow();
                            dtxtldr["Disease_EHR_ID"] = dtRow["Disease_EHR_ID"].ToString().Trim();
                            dtxtldr["InsUptDlt"] = 3;
                            dtEasyDentalDisease.Rows.Add(dtxtldr);
                        }
                    }
                    dtEasyDentalDisease.AcceptChanges();

                    if (dtEasyDentalDisease != null && dtEasyDentalDisease.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_Disease_EasyDental_To_Local(dtEasyDentalDisease);

                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Disease");
                            GoalBase.WriteToSyncLogFile_Static("Disease Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                            SynchDataLiveDB_Push_Disease();
                        }
                    }

                }
                SynchDataEasyDental_Medication();                
            }
            catch (Exception ex)
            {

                GoalBase.WriteToErrorLogFile_Static("[Disease Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public static void SynchDataEasyDental_Medication()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtMedication = SynchEasyDentalBAL.GetEasyDentalMedicationData();
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
                                GoalBase.WriteToSyncLogFile_Static("Medication Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_Medication();
                            }
                            else
                            {
                                GoalBase.WriteToSyncLogFile_Static("[Medication Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Medication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }

        public static void SynchDataEasyDental_PatientMedication(string Patient_EHR_IDS)
        {
            try
            {
                // if (Utility.IsApplicationIdleTimeOff && !Is_synched_PatientMedication)
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    // Is_synched_PatientMedication = true;
                    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                    {
                        DataTable dtPatientMedication = SynchEasyDentalBAL.GetEasyDentalPatientMedication(Patient_EHR_IDS);
                        dtPatientMedication.Columns.Add("InsUptDlt", typeof(int));
                        DataTable dtLocalMedication = SynchLocalBAL.GetLocalPatientMedicationData("1", Patient_EHR_IDS);

                        foreach (DataRow dtDtxRow in dtPatientMedication.Rows)
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
                                else if (dtDtxRow["Patient_Notes"].ToString().Trim() != row[0]["Patient_Notes"].ToString().Trim())
                                {
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
                                else if (dtDtxRow["is_active"].ToString().Trim() != row[0]["is_active"].ToString().Trim())
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
                            DataRow[] rowDis = dtPatientMedication.Copy().Select("PatientMedication_EHR_ID = '" + dtLPHRow["PatientMedication_EHR_ID"].ToString() + "' And Medication_EHR_ID = '" + dtLPHRow["Medication_EHR_ID"].ToString() + "' And Clinic_Number = '" + dtLPHRow["Clinic_Number"].ToString() + "' and Patient_EHR_ID = '" + dtLPHRow["Patient_EHR_ID"] + "'");
                            if (rowDis.Length > 0)
                            { }
                            else
                            {
                                DataRow rowDisDtldr = dtPatientMedication.NewRow();
                                rowDisDtldr["PatientMedication_EHR_ID"] = dtLPHRow["PatientMedication_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Patient_EHR_ID"] = dtLPHRow["Patient_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Medication_EHR_ID"] = dtLPHRow["Medication_EHR_ID"].ToString().Trim();
                                rowDisDtldr["Medication_Type"] = dtLPHRow["Medication_Type"].ToString().Trim();
                                rowDisDtldr["Medication_Name"] = dtLPHRow["Medication_Name"].ToString().Trim();
                                rowDisDtldr["Clinic_Number"] = dtLPHRow["Clinic_Number"].ToString().Trim();
                                //  rowDisDtldr["is_deleted"] = Convert.ToBoolean( dtLPHRow["is_deleted"].ToString().Trim());
                                rowDisDtldr["InsUptDlt"] = 3;
                                dtPatientMedication.Rows.Add(rowDisDtldr);
                            }
                        }
                        dtPatientMedication.AcceptChanges();

                        if (dtPatientMedication != null && dtPatientMedication.Rows.Count > 0)
                        {
                            bool status = SynchLocalBAL.Save_PatientMedication_EHR_To_Local(dtPatientMedication, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (status)
                            {
                                bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Medication");
                                GoalBase.WriteToSyncLogFile_Static("PatientMedication Sync (" + Utility.Application_Name + " Db " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) Successfully.");
                                SynchDataLiveDB_Push_PatientMedication();
                            }
                            else
                            {
                                GoalBase.WriteToSyncLogFile_Static("[PatientMedication Sync (" + Utility.Application_Name + " Db : " + Utility.DtInstallServiceList.Rows[j]["Database"].ToString() + " to Local Database) ] Error...");
                            }
                        }

                    }
                    // Is_synched_PatientMedication = false;
                }
            }
            catch (Exception ex)
            {
                // Is_synched_PatientMedication = false;
                GoalBase.WriteToErrorLogFile_Static("[PatientMedication Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
            }
        }
        #endregion

        #region Synch RecallType

        private void fncSynchDataEasyDental_RecallType()
        {
            InitBgWorkerEasyDental_RecallType();
            InitBgTimerEasyDental_RecallType();
        }

        private void InitBgTimerEasyDental_RecallType()
        {
            timerSynchEasyDental_RecallType = new System.Timers.Timer();
            this.timerSynchEasyDental_RecallType.Interval = 1000 * GoalBase.intervalEHRSynch_RecallType;
            this.timerSynchEasyDental_RecallType.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_RecallType_Tick);
            timerSynchEasyDental_RecallType.Enabled = true;
            timerSynchEasyDental_RecallType.Start();
        }

        private void InitBgWorkerEasyDental_RecallType()
        {
            bwSynchEasyDental_RecallType = new BackgroundWorker();
            bwSynchEasyDental_RecallType.WorkerReportsProgress = true;
            bwSynchEasyDental_RecallType.WorkerSupportsCancellation = true;
            bwSynchEasyDental_RecallType.DoWork += new DoWorkEventHandler(bwSynchEasyDental_RecallType_DoWork);
            bwSynchEasyDental_RecallType.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_RecallType_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_RecallType_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_RecallType.Enabled = false;
                MethodForCallSynchOrderEasyDental_RecallType();
            }
        }

        public void MethodForCallSynchOrderEasyDental_RecallType()
        {
            System.Threading.Thread procThreadmainEasyDental_RecallType = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_RecallType);
            procThreadmainEasyDental_RecallType.Start();
        }

        public void CallSyncOrderTableEasyDental_RecallType()
        {
            if (bwSynchEasyDental_RecallType.IsBusy != true)
            {
                bwSynchEasyDental_RecallType.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_RecallType_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_RecallType.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_RecallType();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_RecallType_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_RecallType.Enabled = true;
        }

        public static void SynchDataEasyDental_RecallType()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtEasyDentalRecallType = SynchEasyDentalBAL.GetEasyDentalRecallTypeData();
                    dtEasyDentalRecallType.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalRecallType = SynchLocalBAL.GetLocalRecallTypeData("1");

                    foreach (DataRow dtDtxRow in dtEasyDentalRecallType.Rows)
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
                        DataRow[] row = dtEasyDentalRecallType.Copy().Select("RecallType_EHR_ID = '" + dtDtxRow["RecallType_EHR_ID"] + "'");
                        if (row.Length > 0)
                        { }
                        else
                        {
                            DataRow BlcOptDtldr = dtEasyDentalRecallType.NewRow();
                            BlcOptDtldr["RecallType_EHR_ID"] = dtDtxRow["RecallType_EHR_ID"].ToString().Trim();
                            BlcOptDtldr["InsUptDlt"] = 3;
                            dtEasyDentalRecallType.Rows.Add(BlcOptDtldr);
                        }
                    }
                    dtEasyDentalRecallType.AcceptChanges();

                    if (dtEasyDentalRecallType != null && dtEasyDentalRecallType.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_RecallType_EasyDental_To_Local(dtEasyDentalRecallType);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("RecallType");
                            GoalBase.WriteToSyncLogFile_Static("RecallType Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            SynchDataLiveDB_Push_RecallType();
                        }
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[RecallType Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch User
        public void fncSynchDataEasyDental_User()
        {
            InitBgWorkerEasyDental_User();
            InitBgTimerEasyDental_User();
        }

        public void InitBgTimerEasyDental_User()
        {
            timerSynchEasyDental_User = new System.Timers.Timer();
            this.timerSynchEasyDental_User.Interval = 1000 * GoalBase.intervalEHRSynch_Provider;
            this.timerSynchEasyDental_User.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_User_Tick);
            timerSynchEasyDental_User.Enabled = true;
            timerSynchEasyDental_User.Start();
        }

        public void timerSynchEasyDental_User_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_User.Enabled = false;
                MethodForCallSynchOrderEasyDental_User();
            }
        }

        public void MethodForCallSynchOrderEasyDental_User()
        {
            System.Threading.Thread procThreadmainEasyDental_User = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_User);
            procThreadmainEasyDental_User.Start();
        }

        public void CallSyncOrderTableEasyDental_User()
        {
            if (bwSynchEasyDental_User.IsBusy != true)
            {
                bwSynchEasyDental_User.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        public void InitBgWorkerEasyDental_User()
        {
            bwSynchEasyDental_User = new BackgroundWorker();
            bwSynchEasyDental_User.WorkerReportsProgress = true;
            bwSynchEasyDental_User.WorkerSupportsCancellation = true;
            bwSynchEasyDental_User.DoWork += new DoWorkEventHandler(bwSynchEasyDental_User_DoWork);
            bwSynchEasyDental_User.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_User_RunWorkerCompleted);
        }

        public void bwSynchEasyDental_User_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_User.Enabled = true;
        }

        public void bwSynchEasyDental_User_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_User.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_User();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        public static void SynchDataEasyDental_User()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                GoalBase.WriteToErrorLogFile_Static("[User Sync start to Local Database) ]");
                try
                {
                    DataTable dtEasyDentalUser = SynchEasyDentalBAL.GetEasyDentalUserData();
                    GoalBase.WriteToErrorLogFile_Static("1");
                    dtEasyDentalUser.Columns.Add("InsUptDlt", typeof(int));
                    DataTable dtLocalUser = SynchLocalBAL.GetLocalUser("1");
                    GoalBase.WriteToErrorLogFile_Static("2");
                    foreach (DataRow dtDtxRow in dtEasyDentalUser.Rows)
                    {
                        GoalBase.WriteToErrorLogFile_Static("[User Sync start to Local Database) ]");
                        dtDtxRow["Is_Active"] = dtDtxRow["Is_Active"].ToString() == "True" ? 0 : 1;
                        GoalBase.WriteToErrorLogFile_Static("[User Sync start to Local Database) ]");
                        DataRow[] row = dtLocalUser.Copy().Select("User_EHR_ID = '" + dtDtxRow["User_EHR_ID"] + "'");
                        GoalBase.WriteToErrorLogFile_Static("3");
                        if (row.Length > 0)
                        {
                            GoalBase.WriteToErrorLogFile_Static("4");
                            if (dtDtxRow["First_Name"].ToString().ToLower().Trim() != row[0]["First_Name"].ToString().ToLower().Trim())
                            {
                                dtDtxRow["InsUptDlt"] = 2;
                            }
                            else if (Convert.ToBoolean(dtDtxRow["Is_Active"].ToString().Trim()) != Convert.ToBoolean(row[0]["Is_Active"]))
                            {
                                //GoalBase.WriteToErrorLogFile_Static("[Providers log is_active " + dtDtxRow["is_active"].ToString().Trim() + " : " + row[0]["is_active"]);
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
                    GoalBase.WriteToErrorLogFile_Static("5");
                    dtEasyDentalUser.AcceptChanges();

                    if (dtEasyDentalUser != null && dtEasyDentalUser.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_User_EasyDental_To_Local(dtEasyDentalUser);
                        GoalBase.WriteToErrorLogFile_Static("6" + status.ToString());
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Users");
                            GoalBase.WriteToErrorLogFile_Static("User Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            SynchDataLiveDB_Push_User();
                        }
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[User Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }

        }
        #endregion

        #region Synch ApptStatus

        private void fncSynchDataEasyDental_ApptStatus()
        {
            InitBgWorkerEasyDental_ApptStatus();
            InitBgTimerEasyDental_ApptStatus();
        }

        private void InitBgTimerEasyDental_ApptStatus()
        {
            timerSynchEasyDental_ApptStatus = new System.Timers.Timer();
            this.timerSynchEasyDental_ApptStatus.Interval = 1000 * GoalBase.intervalEHRSynch_ApptStatus;
            this.timerSynchEasyDental_ApptStatus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_ApptStatus_Tick);
            timerSynchEasyDental_ApptStatus.Enabled = true;
            timerSynchEasyDental_ApptStatus.Start();
        }

        private void InitBgWorkerEasyDental_ApptStatus()
        {
            bwSynchEasyDental_ApptStatus = new BackgroundWorker();
            bwSynchEasyDental_ApptStatus.WorkerReportsProgress = true;
            bwSynchEasyDental_ApptStatus.WorkerSupportsCancellation = true;
            bwSynchEasyDental_ApptStatus.DoWork += new DoWorkEventHandler(bwSynchEasyDental_ApptStatus_DoWork);
            bwSynchEasyDental_ApptStatus.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_ApptStatus_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_ApptStatus_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_ApptStatus.Enabled = false;
                MethodForCallSynchOrderEasyDental_ApptStatus();
            }
        }

        public void MethodForCallSynchOrderEasyDental_ApptStatus()
        {
            System.Threading.Thread procThreadmainEasyDental_ApptStatus = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_ApptStatus);
            procThreadmainEasyDental_ApptStatus.Start();
        }

        public void CallSyncOrderTableEasyDental_ApptStatus()
        {
            if (bwSynchEasyDental_ApptStatus.IsBusy != true)
            {
                bwSynchEasyDental_ApptStatus.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_ApptStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_ApptStatus.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_ApptStatus();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_ApptStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_ApptStatus.Enabled = true;
        }

        public static void SynchDataEasyDental_ApptStatus()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                try
                {
                    DataTable dtEasyDentalApptStatus = SynchEasyDentalBAL.GetEasyDentalApptStatusData();
                    dtEasyDentalApptStatus.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalApptStatus.Rows.Add(0, "<none>", "normal");
                    dtEasyDentalApptStatus.Rows.Add(-106, "<COMPLETE>", "normal");
                    dtEasyDentalApptStatus.AcceptChanges();

                    DataTable dtLocalApptStatus = SynchLocalBAL.GetLocalAppointmentStatusData("1");

                    foreach (DataRow dtDtxRow in dtEasyDentalApptStatus.Rows)
                    {
                        DataRow[] row = dtLocalApptStatus.Copy().Select("ApptStatus_EHR_ID = '" + dtDtxRow["ApptStatus_EHR_ID"] + "'");
                        if (row.Length > 0)
                        {
                            if (dtDtxRow["ApptStatus_Name"].ToString() != "<none>" && dtDtxRow["ApptStatus_Name"].ToString() != "<COMPLETE>")
                            {
                                if (dtDtxRow["ApptStatus_Name"].ToString().Split(' ')[0].Substring(1, dtDtxRow["ApptStatus_Name"].ToString().Trim().Split(' ')[0].Length - 1).ToLower().Trim() != row[0]["ApptStatus_Name"].ToString().ToLower().Trim())
                                {
                                    dtDtxRow["ApptStatus_Name"] = dtDtxRow["ApptStatus_Name"].ToString().Split(' ')[0].Substring(1, dtDtxRow["ApptStatus_Name"].ToString().Trim().Split(' ')[0].Length - 1).Trim();
                                    dtDtxRow["InsUptDlt"] = 2;
                                }
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

                    dtEasyDentalApptStatus.AcceptChanges();

                    if (dtEasyDentalApptStatus != null && dtEasyDentalApptStatus.Rows.Count > 0)
                    {
                        bool status = SynchEasyDentalBAL.Save_ApptStatus_EasyDental_To_Local(dtEasyDentalApptStatus);
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("ApptStatus");
                            GoalBase.WriteToSyncLogFile_Static("ApptStatus Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            SynchDataLiveDB_Push_ApptStatus();
                        }
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToErrorLogFile_Static("[ApptStatus Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
            }
        }

        #endregion

        #region Synch Holidays

        private void fncSynchDataEasyDental_Holidays()
        {
            //  SynchDataEasyDental_Holidays();
            InitBgWorkerEasyDental_Holidays();
            InitBgTimerEasyDental_Holidays();
        }

        private void InitBgTimerEasyDental_Holidays()
        {
            timerSynchEasyDental_Holidays = new System.Timers.Timer();
            this.timerSynchEasyDental_Holidays.Interval = 1000 * GoalBase.intervalEHRSynch_Holiday;
            this.timerSynchEasyDental_Holidays.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_Holidays_Tick);
            timerSynchEasyDental_Holidays.Enabled = true;
            timerSynchEasyDental_Holidays.Start();
            timerSynchEasyDental_Holidays_Tick(null, null);
        }

        private void InitBgWorkerEasyDental_Holidays()
        {
            bwSynchEasyDental_Holidays = new BackgroundWorker();
            bwSynchEasyDental_Holidays.WorkerReportsProgress = true;
            bwSynchEasyDental_Holidays.WorkerSupportsCancellation = true;
            bwSynchEasyDental_Holidays.DoWork += new DoWorkEventHandler(bwSynchEasyDental_Holidays_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEasyDental_Holidays.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_Holidays_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_Holidays_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_Holidays.Enabled = false;
                MethodForCallSynchOrderEasyDental_Holidays();
            }
        }

        public void MethodForCallSynchOrderEasyDental_Holidays()
        {
            System.Threading.Thread procThreadmainEasyDental_Holidays = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_Holidays);
            procThreadmainEasyDental_Holidays.Start();
        }

        public void CallSyncOrderTableEasyDental_Holidays()
        {
            if (bwSynchEasyDental_Holidays.IsBusy != true)
            {
                bwSynchEasyDental_Holidays.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_Holidays_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_Holidays.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_Holidays();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_Holidays_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_Holidays.Enabled = true;
        }

        public static void SynchDataEasyDental_Holidays()
        {
            if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
            {
                #region Holidays
                try
                {
                    DataTable dtEasyDentalHoliday = SynchLocalBAL.GetLocalHolidayData("1");

                    dtEasyDentalHoliday.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalHoliday.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtLocalHoliday = SynchLocalBAL.GetLocalHolidayData("1");
                   
                    dtEasyDentalHoliday = CommonUtility.AddHolidays(dtEasyDentalHoliday, dtLocalHoliday, "SchedDate", "Comment", "H_EHR_ID");
                    //rooja 5-5-23

                    if (!dtLocalHoliday.Columns.Contains("InsUptDlt"))
                    {
                        dtLocalHoliday.Columns.Add("InsUptDlt", typeof(int));
                        dtLocalHoliday.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    foreach (DataRow dtDtxRow in dtEasyDentalHoliday.Rows)
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
                    }

                    foreach (DataRow dtDtxRow in dtLocalHoliday.Rows)
                    {
                        DataRow[] row = dtEasyDentalHoliday.Copy().Select("SchedDate = '" + dtDtxRow["SchedDate"] + "'");
                        if (row.Length <= 0)
                        {
                            dtDtxRow["InsUptDlt"] = 3;
                        }
                    }

                    dtEasyDentalHoliday.AcceptChanges();
                    dtLocalHoliday.AcceptChanges();

                    if ((dtEasyDentalHoliday != null && dtEasyDentalHoliday.Rows.Count > 0) || (dtLocalHoliday != null && dtLocalHoliday.Rows.Count > 0))
                    {
                        bool status = false;

                        //=====================existing code for save holidays======================
                        DataTable dtSaveRecords = dtEasyDentalHoliday.Clone();
                        if (dtEasyDentalHoliday.Select("InsUptDlt IN (1,2)").Count() > 0 || dtLocalHoliday.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtEasyDentalHoliday.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalHoliday.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtLocalHoliday.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtLocalHoliday.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "Holiday", "H_LocalDB_ID,H_Web_ID", "H_LocalDB_ID");
                        }
                        else
                        {
                            if (dtEasyDentalHoliday.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");

                            GoalBase.WriteToSyncLogFile_Static("Holiday Sync (" + Utility.Application_Name + " to Local Database) Successfully.");

                            SynchDataLiveDB_Push_Holiday();
                        }
                        //===================
                    }
                }
                catch (Exception ex)
                {
                    GoalBase.WriteToSyncLogFile_Static("[Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                }
                #endregion

                #region OperatoryHolidays

                //if (IsEasyDentalOperatorySync)
                //{
                //    try
                //    {
                //        DataTable dtEasyDentalOperatory = SynchEasyDentalBAL.GetEasyDentalOperatoryData();
                //        DataTable dtEasyDentalHoliday = SynchEasyDentalBAL.GetEasyDentalOperatoryHolidaysData(dtEasyDentalOperatory);
                //        dtEasyDentalHoliday.Columns.Add("InsUptDlt", typeof(int));
                //        DataTable dtLocalHoliday = SynchLocalBAL.GetLocalEasyDentalOperatoryHolidayData();

                //        foreach (DataRow dtDtxRow in dtEasyDentalHoliday.Rows)
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
                //            DataRow[] row = dtEasyDentalHoliday.Copy().Select("Sched_exception_date = '" + dtDtxRow["SchedDate"] + "' and op_id = '" + dtDtxRow["H_Operatory_EHR_ID"].ToString() + "'");
                //            if (row.Length <= 0)
                //            {
                //                DataRow ApptDtldr = dtEasyDentalHoliday.NewRow();
                //                ApptDtldr["Sched_exception_date"] = dtDtxRow["SchedDate"].ToString().Trim();
                //                ApptDtldr["op_id"] = dtDtxRow["H_Operatory_EHR_ID"].ToString().Trim();
                //                ApptDtldr["InsUptDlt"] = 3;
                //                dtEasyDentalHoliday.Rows.Add(ApptDtldr);
                //            }
                //        }
                //        dtEasyDentalHoliday.AcceptChanges();
                //        if (dtEasyDentalHoliday != null && dtEasyDentalHoliday.Rows.Count > 0)
                //        {
                //            dtEasyDentalHoliday = CommonUtility.CreateHolidayEHRId(dtEasyDentalHoliday);
                //            bool status = SynchEasyDentalBAL.Save_Opeatory_Holidays_EasyDental_To_Local(dtEasyDentalHoliday);
                //            if (status)
                //            {
                //                SynchDataLiveDB_Push_Holiday();
                //            }
                //        }
                //        else
                //        {
                //            bool UpdateSync_Table_Datetime_Push = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday_Push");
                //        }
                //         GoalBase.WriteToSyncLogFile_Static("Operatory_Holiday Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                //        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Holiday");
                //    }

                //    catch (Exception ex)
                //    {
                //         GoalBase.WriteToErrorLogFile_Static("[Operatory_Holiday Sync (" + Utility.Application_Name + " to Local Database) ]" + ex.Message);
                //    }
                //}

                #endregion
            }
        }

        #endregion

        #region Create Appointment

        private void fncSynchDataLocalToEasyDental_Appointment()
        {
            InitBgWorkerLocalToEasyDental_Appointment();
            InitBgTimerLocalToEasyDental_Appointment();
        }

        private void InitBgTimerLocalToEasyDental_Appointment()
        {
            timerSynchLocalToEasyDental_Appointment = new System.Timers.Timer();
            this.timerSynchLocalToEasyDental_Appointment.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchLocalToEasyDental_Appointment.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToEasyDental_Appointment_Tick);
            timerSynchLocalToEasyDental_Appointment.Enabled = true;
            timerSynchLocalToEasyDental_Appointment.Start();
            timerSynchLocalToEasyDental_Appointment_Tick(null, null);
        }

        private void InitBgWorkerLocalToEasyDental_Appointment()
        {
            bwSynchLocalToEasyDental_Appointment.WorkerReportsProgress = true;
            bwSynchLocalToEasyDental_Appointment.WorkerSupportsCancellation = true;
            bwSynchLocalToEasyDental_Appointment.DoWork += new DoWorkEventHandler(bwSynchLocalToEasyDental_Appointment_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToEasyDental_Appointment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToEasyDental_Appointment_RunWorkerCompleted);
        }

        private void timerSynchLocalToEasyDental_Appointment_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToEasyDental_Appointment.Enabled = false;
                MethodForCallSynchOrderLocalToEasyDental_Appointment();
            }
        }

        public void MethodForCallSynchOrderLocalToEasyDental_Appointment()
        {
            System.Threading.Thread procThreadmainLocalToEasyDental_Appointment = new System.Threading.Thread(this.CallSyncOrderTableLocalToEasyDental_Appointment);
            procThreadmainLocalToEasyDental_Appointment.Start();
        }

        public void CallSyncOrderTableLocalToEasyDental_Appointment()
        {
            if (bwSynchLocalToEasyDental_Appointment.IsBusy != true)
            {
                bwSynchLocalToEasyDental_Appointment.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToEasyDental_Appointment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToEasyDental_Appointment.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToEasyDental_Appointment();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchLocalToEasyDental_Appointment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToEasyDental_Appointment.Enabled = true;
        }

        public static void SynchDataLocalToEasyDental_Appointment()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    GoalBase.WriteToSyncLogFile_Static("User easydental 1 from appt.");
                    //CheckEntryUserLoginIdExist();
                    GoalBase.WriteToSyncLogFile_Static("User easydental 1 from appt. end.");
                    DataTable dtWebAppointment = SynchLocalBAL.GetLocalNewWebAppointmentData("1");
                    DataTable dtEasyDentalPatient = SynchEasyDentalBAL.GetEasyDentalPatientID_NameData();
                    DataTable dtIdelProv = SynchEasyDentalBAL.GetEasyDentalIdelProvider();
                    DataTable dtLocalProvider = SynchLocalBAL.GetLocalProviderData("", "1");

                    string tmpIdelProv = dtIdelProv.Rows[0][0].ToString();
                    string tmpApptProv = "";
                    int tmpPatient_id = 0;
                    int tmpPatient_Gur_id = 0;
                    int tmpAppt_EHR_id = 0;
                    int tmpNewPatient = 1;

                    string tmpLastName = "";
                    string tmpFirstName = "";

                    string TmpWebPatientName = "";
                    string TmpWebRevPatientName = "";

                    string PatientName = "";
                    // GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Stage 1 Loop");

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
                        TimeSpan tmpApptDuration = tmpEndTime - tmpStartTime;
                        int tmpApptDurMinutes = Convert.ToInt32(tmpApptDuration.TotalMinutes);

                        DataTable dtBookOperatoryApptWiseDateTime = SynchEasyDentalBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
                        string tmpIdealOperatory = "";
                        string appointment_EHR_id = "";
                        // GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental" + dtDtxRow["First_Name"].ToString().Trim() +' ' + dtDtxRow["Last_Name"].ToString().Trim());
                        if (dtBookOperatoryApptWiseDateTime != null && dtBookOperatoryApptWiseDateTime.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtBookOperatoryApptWiseDateTime.Rows)
                            {
                                #region GetPatientDetail
                                try
                                {
                                    if (dr["patId"].ToString() != "0" || dr["patId"].ToString() != string.Empty)
                                    {
                                        DataRow[] drEasyDentalPatient = dtEasyDentalPatient.Copy().Select("Patient_EHR_ID = '" + dr["patId"] + "'");

                                        if (drEasyDentalPatient.Length > 0)
                                        {
                                            dr["FirstName"] = drEasyDentalPatient[0]["FirstName"].ToString().Trim();
                                            dr["LastName"] = drEasyDentalPatient[0]["LastName"].ToString().Trim();
                                            dr["Mobile"] = drEasyDentalPatient[0]["Mobile"].ToString().Trim();
                                            dr["Email"] = drEasyDentalPatient[0]["Email"].ToString().Trim();
                                        }
                                    }
                                    else
                                    {
                                        string[] pname = dr["patient_name"].ToString().Split(',');
                                        if (pname.Length > 0)
                                        {
                                            dr["LastName"] = pname[0].ToString().Trim();
                                            dr["FirstName"] = pname.Length > 1 ? pname[1].ToString().Trim() : "";
                                        }

                                    }
                                }
                                catch (Exception)
                                {

                                }
                                #endregion

                                #region GetProviderDetail
                                try
                                {
                                    DataRow[] drEasyDentalProvider = dtLocalProvider.Copy().Select("Provider_EHR_ID = '" + dr["provider_id"] + "'");
                                    //   DataRow[] drPatientcollect_payment = dtEasyDentalPatientcollect_payment.Copy().Select("Patient_EHR_ID = '" + dr["Patient_EHR_ID"] + "'");

                                    if (drEasyDentalProvider.Length > 0)
                                    {
                                        dr["ProviderFirstName"] = drEasyDentalProvider[0]["First_Name"].ToString().Trim();
                                        dr["ProviderLastName"] = drEasyDentalProvider[0]["Last_Name"].ToString().Trim();
                                    }
                                }
                                catch (Exception)
                                {

                                }
                                #endregion
                            }
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
                                        if ((tmpStartTime >= Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()))
                                            && (tmpStartTime < Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
                                        {
                                            IsConflict = true;
                                            break;
                                        }
                                        else if ((tmpEndTime > Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()))
                                            && (tmpEndTime <= Convert.ToDateTime(rowBookOpTime[Bop]["appointment_date"].ToString()).AddMinutes(Convert.ToInt32(rowBookOpTime[Bop]["ApptMin"].ToString()))))
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
                        // GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental DoubleBooking Save");
                        if (tmpIdealOperatory == "")
                        {
                            DataTable dtTemp = dtBookOperatoryApptWiseDateTime.Select("Appointment_EHR_id = " + appointment_EHR_id).CopyToDataTable();
                            bool status = SynchLocalBAL.Save_Appointment_Is_Appt_DoubleBook_In_Local(dtDtxRow["Appt_Web_ID"].ToString().Trim(), "1", dtTemp, appointment_EHR_id, Utility.DtInstallServiceList.Rows[0]["Location_ID"].ToString());
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
                            tmpPatient_id = Convert.ToInt32(GetPatientEHRID(dtDtxRow["Appt_DateTime"].ToString().Trim(), dtEasyDentalPatient, tmpPatient_id.ToString(), dtDtxRow["Mobile_Contact"].ToString().Trim(), dtDtxRow["First_Name"].ToString().Trim(), dtDtxRow["MI"].ToString().Trim(), dtDtxRow["Last_Name"].ToString().Trim(), dtDtxRow["Email"].ToString().Trim(), Utility.DBConnString, dtDtxRow["Clinic_Number"].ToString(), Convert.ToDateTime(dtDtxRow["birth_date"].ToString().Trim()), dtDtxRow["Provider_EHR_ID"].ToString()));
                            #region code commented by shruti(new patient ststus logic based on(dob,mobile,firstname,lastname) )
                            // GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Check For Patient Exists");
                            //DataRow[] row = dtEasyDentalPatient.Copy().Select("Mobile = '" + SynchEasyDentalDAL.SetFormatePhoneNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) + "' OR Home_Phone = '" + SynchEasyDentalDAL.SetFormatePhoneNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) + "' OR Work_Phone = '" + SynchEasyDentalDAL.SetFormatePhoneNumber(dtDtxRow["Mobile_Contact"].ToString().Trim()) + "' ");
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
                            //    tmpPatient_Gur_id = Convert.ToInt32(row[0]["Guar_id"].ToString());
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
                            ////  GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Check PatientId " + tmpPatient_id.ToString());
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
                            //    // GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Save Patient " + tmpFirstName + " " + dtDtxRow["Mobile_Contact"].ToString());
                            //    tmpPatient_id = SynchEasyDentalBAL.Save_Patient_Local_To_EasyDental(tmpLastName.Trim(),
                            //                                                                        tmpFirstName,
                            //                                                                        dtDtxRow["MI"].ToString().Trim(),
                            //                                                                        dtDtxRow["Mobile_Contact"].ToString().Trim(),
                            //                                                                        dtDtxRow["Email"].ToString().Trim(),
                            //                                                                        tmpApptProv,
                            //                                                                        dtDtxRow["Appt_DateTime"].ToString().Trim(),
                            //                                                                        tmpPatient_Gur_id,
                            //                                                                        dtDtxRow["Birth_Date"].ToString().Trim());
                            //    //  GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Patient Successfully created " + tmpFirstName + " " + dtDtxRow["Mobile_Contact"].ToString());
                            //}
                            #endregion

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

                                //DataTable dtBookOperatoryApptWiseDateTime = SynchEasyDentalBAL.GetBookOperatoryAppointmenetWiseDateTime(Convert.ToDateTime(dtDtxRow["Appt_DateTime"].ToString().Trim()));
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

                                PatientName = tmpLastName + ", " + tmpFirstName;
                                PatientName = SynchEasyDentalBAL.GetPatientName(tmpPatient_id);
                                // GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Patient already exists " + PatientName );
                                tmpAppt_EHR_id = SynchEasyDentalBAL.Save_Appointment_Local_To_EasyDental(tmpPatient_id.ToString(), tmpApptDurMinutes, tmpIdealOperatory.ToString(), tmpApptProv,
                                    dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDtxRow["Appt_DateTime"].ToString().Trim(), dtDtxRow["ApptType_EHR_ID"].ToString().Trim(), PatientName, dtDtxRow["comment"].ToString().Trim());
                                // GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Appointment Created " + PatientName);
                                if (tmpAppt_EHR_id > 0)
                                {
                                    //  GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Appointment update in Local ");
                                    bool isApptId_Update = SynchEasyDentalBAL.Update_Appointment_EHR_Id_Web_Book_Appointment(tmpAppt_EHR_id.ToString(), dtDtxRow["Appt_Web_ID"].ToString().Trim());
                                    //  GoalBase.WriteToSyncLogFile_Static("Appointment Sync Local TO EasyDental Appointment updated in Local ");
                                }
                            }
                        }
                    }
                    //SynchDataLiveDB_Push_Appointment_Is_Appt_DoubleBook();
                    GoalBase.WriteToSyncLogFile_Static("Appointment Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Appointment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }

        #endregion

        #region Patient Form

        private void fncSynchDataLocalToEasyDental_Patient_Form()
        {
            InitBgWorkerLocalToEasyDental_Patient_Form();
            InitBgTimerLocalToEasyDental_Patient_Form();
        }

        private void InitBgTimerLocalToEasyDental_Patient_Form()
        {
            timerSynchLocalToEasyDental_Patient_Form = new System.Timers.Timer();
            this.timerSynchLocalToEasyDental_Patient_Form.Interval = 1000 * GoalBase.intervalEHRSynch_PatientForm;
            this.timerSynchLocalToEasyDental_Patient_Form.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchLocalToEasyDental_Patient_Form_Tick);
            timerSynchLocalToEasyDental_Patient_Form.Enabled = true;
            timerSynchLocalToEasyDental_Patient_Form.Start();
            timerSynchLocalToEasyDental_Patient_Form_Tick(null, null);
        }

        private void InitBgWorkerLocalToEasyDental_Patient_Form()
        {
            bwSynchLocalToEasyDental_Patient_Form = new BackgroundWorker();
            bwSynchLocalToEasyDental_Patient_Form.WorkerReportsProgress = true;
            bwSynchLocalToEasyDental_Patient_Form.WorkerSupportsCancellation = true;
            bwSynchLocalToEasyDental_Patient_Form.DoWork += new DoWorkEventHandler(bwSynchLocalToEasyDental_Patient_Form_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchLocalToEasyDental_Patient_Form.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchLocalToEasyDental_Patient_Form_RunWorkerCompleted);
        }

        private void timerSynchLocalToEasyDental_Patient_Form_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchLocalToEasyDental_Patient_Form.Enabled = false;
                MethodForCallSynchOrderLocalToEasyDental_Patient_Form();
            }
        }

        public void MethodForCallSynchOrderLocalToEasyDental_Patient_Form()
        {
            System.Threading.Thread procThreadmainLocalToEasyDental_Patient_Form = new System.Threading.Thread(this.CallSyncOrderTableLocalToEasyDental_Patient_Form);
            procThreadmainLocalToEasyDental_Patient_Form.Start();
        }

        public void CallSyncOrderTableLocalToEasyDental_Patient_Form()
        {
            if (bwSynchLocalToEasyDental_Patient_Form.IsBusy != true)
            {
                bwSynchLocalToEasyDental_Patient_Form.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchLocalToEasyDental_Patient_Form_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchLocalToEasyDental_Patient_Form.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataLocalToEasyDental_Patient_Form();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchLocalToEasyDental_Patient_Form_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchLocalToEasyDental_Patient_Form.Enabled = true;
        }

        public static void SynchDataLocalToEasyDental_Patient_Form()
        {

            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {                    
                    IsProviderSyncedFirstTime = true;
                    IS_EHRDocPulled = false;
                    SynchDataLiveDB_Pull_PatientForm();
                    SynchDataLiveDB_Pull_PatientPortal();
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
                            dtDtxRow["ehrfield"] = "FirstName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "last_name")
                        {
                            dtDtxRow["ehrfield"] = "LastName";
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
                            dtDtxRow["ehrfield"] = "Email";
                        }
                        if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "status")
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
                            dtDtxRow["ehrfield"] = "MI";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "preferred_name")
                        {
                            dtDtxRow["ehrfield"] = "PrefName";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sex")
                        {
                            dtDtxRow["ehrfield"] = "Gender";
                            if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "MALE")
                            {
                                dtDtxRow["ehrfield_value"] = 'M';
                            }
                            else if (dtDtxRow["ehrfield_value"].ToString().Trim().ToUpper() == "FEMALE")
                            {
                                dtDtxRow["ehrfield_value"] = 'F';
                            }
                            else
                            {
                                dtDtxRow["ehrfield_value"] = 'M';
                            }
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "work_phone")
                        {
                            dtDtxRow["ehrfield"] = "WorkPhone";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "pri_provider_id")
                        {
                            dtDtxRow["ehrfield"] = "ProvId1";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "sec_provider_id")
                        {
                            dtDtxRow["ehrfield"] = "ProvId2";
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
                            dtDtxRow["ehrfield"] = "zip";
                        }
                        else if (dtDtxRow["ehrfield"].ToString().Trim().ToLower() == "address_one")
                        {
                            dtDtxRow["ehrfield"] = "street";
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
                        bool Is_Record_Update = SynchEasyDentalBAL.Save_Patient_Form_Local_To_EasyDental(dtWebPatient_Form);
                    }

                    try
                    {
                        GetMedicalEasyDentalHistoryRecords();
                        SynchEasyDentalBAL.SaveMedicalHistoryLocalToEasyDental();
                    }
                    catch (Exception ex)
                    {
                        GoalBase.WriteToErrorLogFile_Static("[Patient_MedicleHistory Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }


                    string Call_Importing = SynchLocalDAL.Call_API_For_PatientFormDate_Importing("1");
                    if (Call_Importing.ToLower() != "success")
                    {
                        GoalBase.WriteToErrorLogFile_Static("[Patient_Form API error with Importing status : " + Call_Importing);
                    }
                    string Call_Completed = SynchLocalDAL.Call_API_For_PatientFormDate_Completed("1");
                    if (Call_Completed.ToLower() != "success")
                    {
                        GoalBase.WriteToErrorLogFile_Static("[Patient_Form API error with Completed status : " + Call_Completed);
                    }
                    GoalBase.WriteToSyncLogFile_Static("Patient_Form Sync (Local Database To " + Utility.Application_Name + ") Successfully.");

                    string Call_PatientPortalCompleted = SynchLocalDAL.Call_API_For_PatientPortalDate_Completed("1", Utility.Location_ID);
                    try
                    {
                        if (Call_PatientPortalCompleted != "success")
                        {
                            GoalBase.WriteToErrorLogFile_Static("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                        }
                        else
                        {
                            GoalBase.WriteToErrorLogFile_Static("[Patient_Portal API called with Completed status : " + Call_PatientPortalCompleted);
                        }
                    }
                    catch (Exception)
                    {
                        GoalBase.WriteToErrorLogFile_Static("[Patient_Portal API error with Completed status : " + Call_PatientPortalCompleted);
                    }
                    if (Utility.LastPaymentSMSCallSyncDateAditServer <= DateTime.Now)
                    {
                        GoalBase.WriteToSyncLogFile_Static("Test easydental 1 from pform.");
                        //CheckEntryUserLoginIdExist();
                        GoalBase.WriteToSyncLogFile_Static("Test easydental 1 from pform.");
                        SynchDataLiveDB_Pull_PatientPaymentLog();
                        SynchDataLocalToEasyDental_Patient_Payment();
                        SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                        frmPozative.CheckCustomhoursForProviderOperatory();
                        SynchDataLiveDB_Pull_PatientPaymentSMSCall();
                        SynchDataLiveDB_Pull_PatientFollowUp();
                        SynchDataLocalToEasyDental_Patient_SMSCallLog();
                        SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
                        SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
                        frmPozative.fncPaymentSMSCallStatusUpdate();
                    }

                    //try
                    //{
                    //    SynchEasyDentalBAL.SaveDiseaseLocalToEasyDental();
                    //}
                    //catch (Exception ex)
                    //{
                    //     GoalBase.WriteToErrorLogFile_Static("[Patient_Disease Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    //}

                    bool isRecordSaved = false, isRecordDeleted = false;
                    string Patient_EHR_IDS = "";
                    string DeletePatientEHRID = "";
                    string SavePatientEHRID = "";
                    try
                    {
                        SynchEasyDentalBAL.DeleteMedicationLocalToEasyDental(ref isRecordDeleted, ref DeletePatientEHRID);
                    }
                    catch (Exception ex)
                    {
                        GoalBase.WriteToErrorLogFile_Static("[Delete Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }
                    try
                    {
                        SynchEasyDentalBAL.SaveMedicationLocalToEasyDental(ref isRecordSaved, ref SavePatientEHRID);
                    }
                    catch (Exception ex)
                    {
                        GoalBase.WriteToErrorLogFile_Static("[Patient_Medication Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                    }
                    if (isRecordSaved || isRecordDeleted)
                    {
                        Patient_EHR_IDS = (DeletePatientEHRID + SavePatientEHRID).TrimEnd(',');
                        if (Patient_EHR_IDS != "")
                        {
                            SynchDataEasyDental_PatientMedication(Patient_EHR_IDS);
                        }
                    }                    
                }
            }

            catch (Exception ex)
            {

                if (!ex.Message.Contains("Key value already exists in index 9"))
                {
                    GoalBase.WriteToErrorLogFile_Static("[Patient_Form Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
                }
            }

        }

        public static void SynchDataLocalToEasyDental_Patient_Payment()
        {
            try
            {
                if (!IsPaymentSyncing)
                {
                    IsPaymentSyncing = true;
                    if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                    {
                        //CheckEntryUserLoginIdExist();
                        SynchDataLiveDB_Pull_PatientPaymentLog();
                        Int64 TransactionHeaderId = 0;
                        DataTable dtPatientPayment = new DataTable();

                        DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientPaymentData("1");
                        //  DataTable dtWebPatientPaymentSplit = SynchLocalBAL.GetLocalWebPatientPaymentSplitData("1");
                        #region Call API for EHR Entry Done
                        if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                        {
                            //noteId = SynchEasyDentalBAL.Save_PatientPaymentLog_LocalToEasyDental(dtWebPatientPayment);
                            SynchEasyDentalBAL.SavePatientPaymentTOEHR(dtWebPatientPayment);
                            // SynchEasyDentalBAL.SavePatientPaymentTOEHR(dtWebPatientPayment, dtWebPatientPaymentSplit);
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient Payment Log");
                            GoalBase.WriteToSyncLogFile_Static("Patient Payment Log Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        }
                        else
                        {
                            GoalBase.WriteToErrorLogFile_Static("Patient Payment Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                        }
                        #endregion

                        #region Sync those patient whose payment done in EHR

                        #endregion
                        //  }


                    }
                    IsPaymentSyncing = false;
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Patient_Paynment Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
            finally { IsPaymentSyncing = false; }
        }

        public static void SynchDataLocalToEasyDental_Patient_SMSCallLog()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    Int64 TransactionHeaderId = 0;
                    string noteId = "";
                    DataTable dtPatientPayment = new DataTable();
                    SynchEasyDentalBAL.DeleteDuplicatePatientLog();
                    DataTable dtWebPatientPayment = SynchLocalBAL.GetLocalWebPatientSMSCallLogData("1");

                    #region Call API for EHR Entry Done
                    if (dtWebPatientPayment != null && dtWebPatientPayment.Rows.Count > 0)
                    {
                        SynchEasyDentalBAL.Save_PatientSMSCallLog_LocalToEasyDental(dtWebPatientPayment);
                        //}
                        bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("Patient SMSCall Log");
                        GoalBase.WriteToSyncLogFile_Static("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Successfully.");
                    }
                    else
                    {
                        GoalBase.WriteToErrorLogFile_Static("Patient SMSCall Log Sync (Local Database To " + Utility.Application_Name + ") Records not available.");

                    }

                    #endregion

                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[Patient_SMSCall Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }
        }


        #region PF Medicle History

        private void fncSynchDataEasyDental_MedicalHistory()
        {
            InitBgWorkerEasyDental_MedicalHistory();
            InitBgTimerEasyDental_MedicalHistory();
        }

        private void InitBgTimerEasyDental_MedicalHistory()
        {
            timerSynchEasyDental_MedicalHistory = new System.Timers.Timer();
            this.timerSynchEasyDental_MedicalHistory.Interval = 1000 * GoalBase.intervalEHRSynch_MedicalHistory;
            this.timerSynchEasyDental_MedicalHistory.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEasyDental_MedicalHistory_Tick);
            timerSynchEasyDental_MedicalHistory.Enabled = true;
            timerSynchEasyDental_MedicalHistory.Start();
            timerSynchEasyDental_MedicalHistory_Tick(null, null);
        }

        private void InitBgWorkerEasyDental_MedicalHistory()
        {
            bwSynchEasyDental_MedicalHistory = new BackgroundWorker();
            bwSynchEasyDental_MedicalHistory.WorkerReportsProgress = true;
            bwSynchEasyDental_MedicalHistory.WorkerSupportsCancellation = true;
            bwSynchEasyDental_MedicalHistory.DoWork += new DoWorkEventHandler(bwSynchEasyDental_MedicalHistory_DoWork);
            bwSynchEasyDental_MedicalHistory.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEasyDental_MedicalHistory_RunWorkerCompleted);
        }

        private void timerSynchEasyDental_MedicalHistory_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEasyDental_MedicalHistory.Enabled = false;
                MethodForCallSynchOrderEasyDental_MedicalHistory();
            }
        }

        public void MethodForCallSynchOrderEasyDental_MedicalHistory()
        {
            System.Threading.Thread procThreadmainEasyDental_MedicalHistory = new System.Threading.Thread(this.CallSyncOrderTableEasyDental_MedicalHistory);
            procThreadmainEasyDental_MedicalHistory.Start();
        }

        public void CallSyncOrderTableEasyDental_MedicalHistory()
        {
            if (bwSynchEasyDental_MedicalHistory.IsBusy != true)
            {
                bwSynchEasyDental_MedicalHistory.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEasyDental_MedicalHistory_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEasyDental_MedicalHistory.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchDataEasyDental_MedicalHistory();
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static(ex.Message);
            }
        }

        private void bwSynchEasyDental_MedicalHistory_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchEasyDental_MedicalHistory.Enabled = true;
        }

        public void SynchDataEasyDental_MedicalHistory()
        {
            SynchDataEasyDental_MedicleHistory();

        }

        public void SynchDataEasyDental_MedicleHistory()
        {

            SynchDataEasyDental_MedicleQuestionData();
        }

        public static void SynchDataEasyDental_MedicleFormData()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    DataTable dtEasyDentalMedicleHistory = new DataTable();
                    dtEasyDentalMedicleHistory.Columns.Add("CD_FormMaster_Local_Id", typeof(Int32));
                    dtEasyDentalMedicleHistory.Columns.Add("CD_FormMaster_EHR_ID", typeof(Int32));
                    dtEasyDentalMedicleHistory.Columns.Add("CD_FormMaster_Web_ID", typeof(string));
                    dtEasyDentalMedicleHistory.Columns.Add("FormName_Name", typeof(string));
                    dtEasyDentalMedicleHistory.Columns.Add("EHR_Entry_DateTime", typeof(DateTime));
                    dtEasyDentalMedicleHistory.Columns.Add("Last_Sync_Date", typeof(DateTime));
                    dtEasyDentalMedicleHistory.Columns.Add("Is_deleted", typeof(bool));
                    dtEasyDentalMedicleHistory.Columns.Add("Is_Adit_Updated", typeof(bool));
                    dtEasyDentalMedicleHistory.Columns.Add("Clinic_Number", typeof(string));
                    dtEasyDentalMedicleHistory.Columns.Add("Service_Install_Id", typeof(string));
                    dtEasyDentalMedicleHistory.Rows.Add(0, 0, "", "Default Form", DateTime.Now, DateTime.Now, 0, 0, "0", "1");
                    dtEasyDentalMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;

                    DataTable dtEasyDentalLocalMedicleHistory = SynchLocalBAL.GetLocalMedicalHistoryRecords("CD_FormMaster", "1", true);

                    if (!dtEasyDentalLocalMedicleHistory.Columns.Contains("InsUptDlt"))
                    {
                        dtEasyDentalLocalMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                        dtEasyDentalLocalMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtEasyDentalLocalMedicleHistory = CompareEasyDentalDataTableRecords(ref dtEasyDentalMedicleHistory, dtEasyDentalLocalMedicleHistory, "CD_FormMaster_EHR_ID", "CD_FormMaster_Local_Id", "CD_FormMaster_Local_Id,CD_FormMaster_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,EHR_Entry_DateTime,is_deleted,Clinic_Number,Service_Install_Id");

                    dtEasyDentalMedicleHistory.AcceptChanges();

                    if (dtEasyDentalMedicleHistory != null && dtEasyDentalMedicleHistory.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtEasyDentalMedicleHistory.Clone();
                        if (dtEasyDentalMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0 || dtEasyDentalLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtEasyDentalMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalMedicleHistory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtEasyDentalLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalLocalMedicleHistory.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "CD_FormMaster", "CD_FormMaster_Local_Id,CD_FormMaster_Web_ID", "CD_FormMaster_Local_Id");
                        }
                        else
                        {
                            if (dtEasyDentalMedicleHistory.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("EasyDental_FormData");
                            GoalBase.WriteToSyncLogFile_Static("EasyDental_Form Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        }
                        SynchDataLiveDB_Push_MedicalHisotryTables("EasyDental_Form");
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EasyDental_Question Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }

        public static void SynchDataEasyDental_MedicleQuestionData()
        {
            try
            {
                if (Utility.IsApplicationIdleTimeOff && Utility.AditLocationSyncEnable)
                {
                    DataTable dtEasyDentalMedicleHistory = SynchEasyDentalBAL.GetEasyDentalMedicleQuestionData();
                    DataTable dtEasyDentalLocalMedicleHistory = SynchLocalBAL.GetEasyDentalLocalMedicleQuestionData();

                    dtEasyDentalMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                    dtEasyDentalMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;

                    if (!dtEasyDentalLocalMedicleHistory.Columns.Contains("InsUptDlt"))
                    {
                        dtEasyDentalLocalMedicleHistory.Columns.Add("InsUptDlt", typeof(int));
                        dtEasyDentalLocalMedicleHistory.Columns["InsUptDlt"].DefaultValue = 0;
                    }

                    dtEasyDentalLocalMedicleHistory = CompareEasyDentalDataTableRecords(ref dtEasyDentalMedicleHistory, dtEasyDentalLocalMedicleHistory, "EasyDental_QuestionId", "EasyDental_Question_LocalDB_ID", "EasyDental_Question_LocalDB_ID,EasyDental_Question_Web_ID,Is_Adit_Updated,Last_Sync_Date,InsUptDlt,Entry_DateTime,EHR_Entry_DateTime,is_deleted,EasyDental_FormMaster_Web_ID,Clinic_Number,Service_Install_Id");

                    dtEasyDentalMedicleHistory.AcceptChanges();

                    if (dtEasyDentalMedicleHistory != null && dtEasyDentalMedicleHistory.Rows.Count > 0)
                    {
                        bool status = false;
                        DataTable dtSaveRecords = dtEasyDentalMedicleHistory.Clone();
                        if (dtEasyDentalMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0 || dtEasyDentalLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                        {
                            if (dtEasyDentalMedicleHistory.Select("InsUptDlt IN (1,2)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalMedicleHistory.Select("InsUptDlt IN (1,2)").CopyToDataTable().CreateDataReader());
                            }
                            if (dtEasyDentalLocalMedicleHistory.Select("InsUptDlt IN (3)").Count() > 0)
                            {
                                dtSaveRecords.Load(dtEasyDentalLocalMedicleHistory.Select("InsUptDlt IN (3)").CopyToDataTable().CreateDataReader());
                            }
                            status = SynchTrackerBAL.Save_Tracker_To_Local(dtSaveRecords, "EasyDental_Question", "EasyDental_Question_LocalDB_ID,EasyDental_Question_Web_ID", "EasyDental_Question_LocalDB_ID");
                        }
                        else
                        {
                            if (dtEasyDentalMedicleHistory.Select("InsUptDlt IN (4)").Count() > 0)
                            {
                                status = true;
                            }
                        }
                        if (status)
                        {
                            bool UpdateSync_Table_Datetime = SynchLocalBAL.Update_Sync_Table_Datetime("EasyDental_Question");
                            GoalBase.WriteToSyncLogFile_Static("EasyDental_Question Sync (" + Utility.Application_Name + " to Local Database) Successfully.");
                        }
                        SynchDataLiveDB_Push_MedicalHisotryTables("EasyDental_Question");
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EasyDental_Question Sync (Local Database To " + Utility.Application_Name + ") ]" + ex.Message);
            }

        }




        #endregion

        #endregion

    }
}
