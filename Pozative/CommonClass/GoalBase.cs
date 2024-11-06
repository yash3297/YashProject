using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using Pozative.UTL;
using System.Management;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Diagnostics;
using Pozative.BAL;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Pozative
{
    public class GoalBase
    {
        public bool StatusIsComfirm;
        public static string ConnectionLog = string.Empty;

        public static string DentrixG62keyFilePath = Application.StartupPath + "\\h36FadCg.dtxkey";

        public static int intervalCheckConnection = 60;     // Second * Minute

        public static int SyncTime_Appointment = 60 * 5;     // Second * Minute

        public static int SyncTime_OperatoryEvent = 60 * 30;
        public static int SyncTime_Provider = 60 * 50;
        public static int SyncTime_Speciality = 60 * 60;
        public static int SyncTime_Operatory = 60 * 70;
        public static int SyncTime_ApptType = 60 * 180;
        public static int SyncTime_Patient = 60 * 90;
        public static int SyncTime_RecallType = 60 * 240;
        public static int SyncTime_PatientPayment = 60 * 5;
        public static int SyncTime_ApptStatus = 60 * 250;
        public static int SyncTime_Holiday = 60 * 200;
        public static int SyncTime_MedicalHistory = SyncTime_Provider;
        public static int SyncTime_PatientForm = 60 * 10;
        public static int SyncTime_ClearLocalRecords = 60 * 10080;

        public static int SyncTime_Provider_OfficeHours = 60 * 50;
        public static int SyncTime_Operatory_OfficeHours = 60 * 70;

        public static int SyncTime_Patient_Document = 60 * 10;
        public static int SyncTime_Insurance = SyncTime_Provider;
        //  public static int intervalEHRSynch_PracticeAnalytics = SyncTime_Appointment;  //60 * 60;

        public static int intervalEHRSynch_Appointment = SyncTime_Appointment;    // Second * Minute
        public static int intervalEHRSynch_OperatoryEvent = SyncTime_OperatoryEvent;
        public static int intervalEHRSynch_Provider = SyncTime_Provider;
        public static int intervalEHRSynch_PatientPayment = SyncTime_PatientPayment;
        public static int intervalEHRSynch_CallSmsLog = SyncTime_Provider;
        public static int intervalEHRSynch_PatientFollowUp = SyncTime_Provider;

        public static int intervalEHRSynch_Provider_OfficeHours = SyncTime_Provider_OfficeHours;
        public static int intervalEHRSynch_Operatory_OfficeHours = SyncTime_Operatory_OfficeHours;

        public static int intervalEHRSynch_Speciality = SyncTime_Speciality;
        public static int intervalEHRSynch_Operatory = SyncTime_Operatory;
        public static int intervalEHRSynch_ApptType = SyncTime_ApptType;
        public static int intervalEHRSynch_Patient = SyncTime_Patient;
        public static int intervalEHRSynch_RecallType = SyncTime_RecallType;
        public static int intervalEHRSynch_ApptStatus = SyncTime_ApptStatus;
        public static int intervalEHRSynch_Holiday = SyncTime_Holiday;
        public static int intervalEHRSynch_MedicalHistory = SyncTime_MedicalHistory;
        public static int intervalEHRSynch_PatientForm = SyncTime_PatientForm;
        public static int intervalEHRSynch_ClearLocalRecords = SyncTime_ClearLocalRecords;

        public static int intervalEHRSynch_Insurance = SyncTime_Insurance;

        public static int intervalEHRSynch_Patient_Document = SyncTime_Patient_Document;

        public static int intervalWebSynch_Push_Appointment = SyncTime_Appointment;

        public static int intervalWebSynch_Push_OperatoryEvent = SyncTime_OperatoryEvent;
        public static int intervalWebSynch_Push_Provider = SyncTime_Provider;
        public static int intervalWebSynch_Push_Speciality = SyncTime_Speciality;
        public static int intervalWebSynch_Push_Operatory = SyncTime_Operatory;
        public static int intervalWebSynch_Push_ApptType = SyncTime_ApptType;
        public static int intervalWebSynch_Push_Patient = SyncTime_Patient;
        public static int intervalWebSynch_Push_RecallType = SyncTime_RecallType;
        public static int intervalWebSynch_Push_ApptStatus = SyncTime_ApptStatus;
        public static int intervalWebSynch_Push_Holiday = SyncTime_Holiday;
        public static int intervalWebSynch_Push_PatientForm = SyncTime_PatientForm;
        public static int intervalWebSynch_Push_ClearLocalRecords = SyncTime_ClearLocalRecords;
        public static int intervalWebSynch_Push_Patient_Document = SyncTime_Patient_Document;
        public static int intervalWebSynch_Push_PatientPayment = SyncTime_PatientPayment;

        public static int intervalEHRSynch_PracticeAnalytics = intervalWebSynch_Push_Appointment;  //60 * 60;

        public static int intervalWebSynch_Pull_Appointment = SyncTime_Appointment;
        public static int intervalWebSynch_Pull_OperatoryEvent = SyncTime_Appointment;
        public static int intervalWebSynch_Pull_Provider = SyncTime_Provider;
        public static int intervalWebSynch_Pull_Speciality = SyncTime_Speciality;
        public static int intervalWebSynch_Pull_Operatory = SyncTime_Operatory;
        public static int intervalWebSynch_Pull_ApptType = SyncTime_ApptType;
        public static int intervalWebSynch_Pull_Patient = SyncTime_Patient;
        public static int intervalWebSynch_Pull_RecallType = SyncTime_RecallType;
        public static int intervalWebSynch_Pull_ApptStatus = SyncTime_ApptStatus;
        public static int intervalWebSynch_Pull_Holiday = SyncTime_Holiday;
        public static int intervalWebSynch_Pull_PatientForm = SyncTime_PatientForm;
        public static int intervalWebSynch_Pull_ClearLocalRecords = SyncTime_ClearLocalRecords;
        public static int intervalWebSynch_Pull_PatientPayment = SyncTime_PatientPayment;
        public static int intervalWebSynch_Pull_Patient_Document = SyncTime_Patient_Document;

        public static int intervalEHRSynch_PozativeAppointment = 60 * 2;    // Second * Minute

        public void SuccessMsgBox(string MsgHead, string MsgTxt)
        {
            frmMessageBox Obj_MsgShow = new frmMessageBox(MsgHead, MsgTxt, 3);
            Obj_MsgShow.ShowDialog();
        }

        public void ErrorMsgBox(string MsgHead, string MsgTxt,bool PlayAudio = false,string AudioText = "")
        {
            //frmMessageBox Obj_MsgShow = new frmMessageBox(MsgHead, MsgTxt, 2);
            //Obj_MsgShow.ShowDialog();
            ShowMessage showMessage = new ShowMessage(MsgTxt,PlayAudio, AudioText);
            showMessage.ShowDialog();
        }

        public void ComfirmMsgBox(string MsgHead, string MsgTxt)
        {
            StatusIsComfirm = false;
            frmMessageBox Obj_MsgShow = new frmMessageBox(MsgHead, MsgTxt, 1);
            Obj_MsgShow.ShowDialog();
            if (Obj_MsgShow.Status)
            {
                StatusIsComfirm = true;
            }
            else
            {
                StatusIsComfirm = false;
            }
        }

        public void InformationMsgBox(string MsgHead, string MsgTxt)
        {
            frmMessageBox Obj_MsgShow = new frmMessageBox(MsgHead, MsgTxt, 4);
            Obj_MsgShow.ShowDialog();
        }

        public static void WriteToSyncLogFile_Static(string SyncLogString)
        {
            GoalBase objG = new GoalBase();
            objG.WriteToSyncLogFile_All(SyncLogString);

        }

        public void WriteToSyncLogFile_All(string SyncLogString, bool IsStringContacted = true)
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            string SyncLogFilePath = Application.StartupPath + "\\" + "SyncLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
            string SyncLogFileName = SyncLogFilePath + "\\" + dtCurrentDtTime.ToString("yyyy_MM_dd") + ".txt";

            try
            {
                if (!System.IO.Directory.Exists(SyncLogFilePath))
                {
                    System.IO.Directory.CreateDirectory(SyncLogFilePath);
                }

                if (!File.Exists(SyncLogFileName))
                {
                    File.Create(SyncLogFileName).Dispose();
                }

                using (StreamWriter writer = new StreamWriter(SyncLogFileName, true))
                {
                    writer.WriteLine(dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + SyncLogString);
                    writer.Close();
                }

                if (IsStringContacted)
                {
                    ConnectionLog = dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + SyncLogString + "\n" + ConnectionLog;
                }
            }
            catch (Exception)
            {
            }
        }


        public void WriteToSyncLogFile(string SyncLogString, bool IsStringContacted = true)
        {
            WriteToSyncLogFile_All(SyncLogString, IsStringContacted);

        }

        public static void WriteToErrorLogFile_Static(string ErrorLogString)
        {
            GoalBase objG = new GoalBase();
            objG.WriteToErrorLogFromAll(ErrorLogString);
            // WriteToErrorLogFromAll(ErrorLogString);
        }
        public static void WriteToPaymentLogFromAll_Static(string ErrorLogString)
        {
            GoalBase objG = new GoalBase();
            objG.WriteToPaymentLogFromAll(ErrorLogString);
            // WriteToErrorLogFromAll(ErrorLogString);
        }
        public static void WriteToFolderLogFromAll_Static(string ErrorLogString,string FolderName)
        {
            GoalBase objG = new GoalBase();
            objG.WriteToFolderLogFromAll(ErrorLogString, FolderName);
            // WriteToErrorLogFromAll(ErrorLogString);
        }

        public void WriteToFolderLogFromAll(string ErrorLogString,string FolderName)
        {
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string ErrorLogFilePath = Application.StartupPath + "\\" + FolderName + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
                string ErrorLogFileName = ErrorLogFilePath + "\\" + dtCurrentDtTime.ToString("yyyy_MM_dd") + ".txt";

                try
                {
                    if (!System.IO.Directory.Exists(ErrorLogFilePath))
                    {
                        System.IO.Directory.CreateDirectory(ErrorLogFilePath);
                    }

                    if (!File.Exists(ErrorLogFileName))
                    {
                        File.Create(ErrorLogFileName).Dispose();
                    }

                    using (StreamWriter writer = new StreamWriter(ErrorLogFileName, true))
                    {
                        writer.WriteLine(dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ErrorLogString);
                        writer.Close();
                    }
                }
                catch (Exception)
                {
                }


                //  ConnectionLog = dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ErrorLogString + "\n" + ConnectionLog;

            }
            catch (Exception)
            {
            }
        }
        public void WriteToPaymentLogFromAll(string ErrorLogString)
        {
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string ErrorLogFilePath = Application.StartupPath + "\\" + "PaymentLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
                string ErrorLogFileName = ErrorLogFilePath + "\\" + dtCurrentDtTime.ToString("yyyy_MM_dd") + ".txt";

                try
                {
                    if (!System.IO.Directory.Exists(ErrorLogFilePath))
                    {
                        System.IO.Directory.CreateDirectory(ErrorLogFilePath);
                    }

                    if (!File.Exists(ErrorLogFileName))
                    {
                        File.Create(ErrorLogFileName).Dispose();
                    }

                    using (StreamWriter writer = new StreamWriter(ErrorLogFileName, true))
                    {
                        writer.WriteLine(dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ErrorLogString);
                        writer.Close();
                    }
                }
                catch (Exception)
                {
                }


                //  ConnectionLog = dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ErrorLogString + "\n" + ConnectionLog;

            }
            catch (Exception)
            {
            }
        }
        public void WriteToErrorLogFromAll(string ErrorLogString)
        {
            try
            {
                bool blnRestartApp = false;
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string ErrorLogFilePath = Application.StartupPath + "\\" + "ErrorLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
                string ErrorLogFileName = ErrorLogFilePath + "\\" + dtCurrentDtTime.ToString("yyyy_MM_dd") + ".txt";
                Match m3 = Regex.Match(ErrorLogString, @"Pozative.sdf");
                try
                {
                    Match m = Regex.Match(ErrorLogString, @"The database file cannot be found");  //There is a file sharing violation.
                    Match m1 = Regex.Match(ErrorLogString, @"There is a file sharing violation");
                    
                    if (m3.Success)
                    {
                        if (m1.Success)
                        {
                            try
                            {
                                //Utility.Get_LocalDbFromWeb();
                                //Utility.UpdateLocalDB_From_BackupDB();
                                Utility.RestartApp();
                            }
                            catch (Exception)
                            {
                                // frmPozative.GetPozativeConfiguration();
                            }
                        }
                        else if (m.Success)
                        {
                            try
                            {
                                Utility.Get_LocalDbFromWeb();
                                Utility.UpdateLocalDB_From_BackupDB();
                                Utility.RestartApp();
                            }
                            catch (Exception)
                            {
                                // frmPozative.GetPozativeConfiguration();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("Error from Get db file not foud backdb failed:" + ex.Message);
                }

                try
                {
                    Match m = Regex.Match(ErrorLogString, @"System.OutOfMemoryException");  
                    if (m.Success && m3.Success)
                    {
                        string[] t = Directory.GetFiles(Environment.CurrentDirectory, "Pozative.sdf");
                        Array.ForEach(t, File.Delete);
                        try
                        {
                            Utility.ResolvedOutofMemeoryException();
                          
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Utility.Get_LocalDbFromWeb();
                                Utility.UpdateLocalDB_From_BackupDB();
                                Utility.RestartApp();
                            }
                            catch
                            {
                                frmPozative.GetPozativeConfiguration();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("Error from memory Exception  failed." + ex.Message);
                }


                try
                {
                    if(ErrorLogString.ToLower().IndexOf(" Fatal failure of the lock susbsytem for this database") >= 0)
                    {
                        Utility.RestartApp();
                    }

                    if(ErrorLogString.ToLower().IndexOf("corrupted") >= 0)
                    {
                        string[] t = Directory.GetFiles(Environment.CurrentDirectory, "Pozative.sdf");
                        Array.ForEach(t, File.Delete);
                        try
                        {
                            Utility.RecoveryDatabase();
                            //Utility.Get_LocalDbFromWeb();
                            //Utility.UpdateLocalDB_From_BackupDB();
                            Utility.RestartApp();
                            //UpdateLocalDBFromBackup();
                        }
                        catch
                        {
                            frmPozative.GetPozativeConfiguration();
                            WriteToSyncLogFile("GetPozativeConfiguration from ADIT (Adit App to Backup Database ) Successfully.");
                        }                                                     
                    }

                    if (ErrorLogString.ToLower().IndexOf("not enough storage is available to complete this operation") >= 0 || ErrorLogString.ToLower().IndexOf("Required Max Database Size") >= 0)
                    {
                        try
                        {
                            if (Utility.nEnoughStorageErrorCount == 0) Utility.DtEnoughStorageError = DateTime.Now;
                            if (Utility.DtEnoughStorageError.AddHours(1) >= DateTime.Now)
                            {
                                Utility.nEnoughStorageErrorCount += 1;
                                if (Utility.nEnoughStorageErrorCount > 20)
                                {
                                    if (CommonUtility.LocalConnectionString().IndexOf("Max Buffer Size=") < 0)
                                    {
                                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseMaxBufferSize"))
                                        {
                                            key1.SetValue("UseMaxBufferSize", true);
                                        }
                                    }
                                    blnRestartApp = true;
                                }
                            }
                            else
                            {
                                Utility.nEnoughStorageErrorCount = 0;
                            }
                        }
                        catch
                        { }
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    if (!System.IO.Directory.Exists(ErrorLogFilePath))
                    {
                        System.IO.Directory.CreateDirectory(ErrorLogFilePath);
                    }

                    if (!File.Exists(ErrorLogFileName))
                    {
                        File.Create(ErrorLogFileName).Dispose();
                    }

                    using (StreamWriter writer = new StreamWriter(ErrorLogFileName, true))
                    {
                        writer.WriteLine(dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ErrorLogString);
                        writer.Close();
                    }
                }
                catch (Exception)
                {
                }


                ConnectionLog = dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ErrorLogString + "\n" + ConnectionLog;

                try
                {
                    if (blnRestartApp)
                    {
                        Utility.RestartApp();
                    }
                }
                catch (Exception ex)
                { }

            }
            catch (Exception)
            {
            }
        }

        public void WriteToErrorLogFile(string ErrorLogString)
        {
            WriteToErrorLogFromAll(ErrorLogString);
        }

        public string ReadSyncLogFile()
        {
            string ReadFile = string.Empty;

            DateTime dtCurrentDtTime = Utility.Datetimesetting();

            string SyncLogFilePath = Application.StartupPath + "\\" + "SyncLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
            var lines = File.ReadAllLines(SyncLogFilePath + "\\" + dtCurrentDtTime.ToString("yyyy_MM_dd") + ".txt").Reverse();
            foreach (string line in lines)
            {
                ReadFile = ReadFile + line.ToString() + "\n";
            }
            return ReadFile;
        }

        public string ReadTempSyncLogFile()
        {
            string ReadFile = string.Empty;

            DateTime dtCurrentDtTime = Utility.Datetimesetting();

            string SyncLogFileName = Application.StartupPath + "\\" + "SyncLog.txt";
            var lines = File.ReadAllLines(SyncLogFileName).Reverse();
            foreach (string line in lines)
            {
                ReadFile = ReadFile + line.ToString() + "\n";
            }
            return ReadFile;

        }

        public string getUniqueID(string drive)
        {
            try
            {
                //return "6C1FEBF2872DB4EFBFF";
                if (drive == string.Empty)
                {
                    //Find first drive
                    foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                    {
                        if (compDrive.IsReady)
                        {
                            drive = compDrive.RootDirectory.ToString();
                            break;
                        }
                    }
                }

                if (drive.EndsWith(":\\"))
                {
                    //C:\ -> C
                    drive = drive.Substring(0, drive.Length - 2);
                }

                string volumeSerial = getVolumeSerial(drive);
                string cpuID = getCPUID();

                //Mix them up and remove some useless 0's
                return cpuID.Substring(13) + cpuID.Substring(1, 4) + volumeSerial + cpuID.Substring(4, 4);
            }
            catch (Exception)
            {
                try
                {
                    string cpuID = getCPUID();
                    string mac = "";
                    foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    {

                        if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                        {
                            if (nic.GetPhysicalAddress().ToString() != "")
                            {
                                mac = nic.GetPhysicalAddress().ToString();
                            }
                        }
                    }
                    return cpuID.Substring(13) + cpuID.Substring(1, 4) + mac + cpuID.Substring(4, 4);
                }
                catch (Exception)
                {
                    try
                    {
                        string cpuID = getCPUID();
                        String firstMacAddress = NetworkInterface
                        .GetAllNetworkInterfaces()
                        .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        .Select(nic => nic.GetPhysicalAddress().ToString())
                        .FirstOrDefault();

                        return cpuID.Substring(13) + cpuID.Substring(1, 4) + firstMacAddress + cpuID.Substring(4, 4);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            string cpuID = getCPUID();
                            const int MIN_MAC_ADDR_LENGTH = 12;
                            string macAddress = string.Empty;
                            long maxSpeed = -1;

                            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                            {
                                //log.Debug(
                                //    "Found MAC Address: " + nic.GetPhysicalAddress() +
                                //    " Type: " + nic.NetworkInterfaceType);

                                string tempMac = nic.GetPhysicalAddress().ToString();
                                if (nic.Speed > maxSpeed &&
                                    !string.IsNullOrEmpty(tempMac) &&
                                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                                {
                                    //log.Debug("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                                    maxSpeed = nic.Speed;
                                    macAddress = tempMac;
                                }
                            }
                            return cpuID.Substring(13) + cpuID.Substring(1, 4) + macAddress + cpuID.Substring(4, 4);
                        }
                        catch (Exception)
                        {
                            return "";
                        }
                    }
                }

            }

        }

        public string getVolumeSerial(string drive)
        {
            try
            {
                ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
                disk.Get();//Error blank

                string volumeSerial = disk["VolumeSerialNumber"].ToString();
                disk.Dispose();

                return volumeSerial;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string getCPUID()
        {
            try
            {
                string cpuInfo = string.Empty;
                ManagementClass managClass = new ManagementClass("win32_processor");
                ManagementObjectCollection managCollec = managClass.GetInstances();

                foreach (ManagementObject managObj in managCollec)//Error Blank
                {
                    if (cpuInfo == string.Empty)
                    {
                        //Get only the first CPU's ID
                        cpuInfo = managObj.Properties["processorID"].Value.ToString();
                        break;
                    }
                }
                return cpuInfo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void CreateShortcut()
        {

            object shDesktop = (object)"Desktop";
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            //string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Notepad.lnk";
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Adit PDF Attachment.lnk";
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Adit PDF Attachment";
            shortcut.TargetPath = Application.StartupPath + "\\AditPDF.exe";
            shortcut.Save();
        }
        public static void fncSynchDentrixDocPwd()
        {
            InitBgWorkerDentrixDocPwd();
        }
        private static BackgroundWorker bwSynchDentrixDocPwd = null;
        private static void InitBgWorkerDentrixDocPwd()
        {
            var bwSynchDentrixDocPwd = new BackgroundWorker();
            bwSynchDentrixDocPwd.WorkerReportsProgress = false;
            bwSynchDentrixDocPwd.WorkerSupportsCancellation = false;
            bwSynchDentrixDocPwd.DoWork += worker_DoWork;
            bwSynchDentrixDocPwd.RunWorkerAsync();
        }

        public void MethodForCallSynchOrderDentrixDocPwd()
        {
            System.Threading.Thread procThreadmainDentrix_Appointment = new System.Threading.Thread(this.CallSyncOrderTableDentrixDocPwd);
            procThreadmainDentrix_Appointment.Start();
        }

        public void CallSyncOrderTableDentrixDocPwd()
        {
            if (bwSynchDentrixDocPwd.IsBusy != true)
            {
                bwSynchDentrixDocPwd.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }
        static Process myProcess = null;
        private static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                GetConnectionStringforDoc(true);
            }
            catch (Exception ex)
            {

            }
        }
        public static void GetConnectionStringforDoc(bool IsConfigureExe)
        {
            try
            {
                string docconnection = Utility.GetAppSettingsString("docconnection", "docconnection", "");
                Utility.DentrixDocConnString = (docconnection != null && docconnection != "") ? Utility.DecryptString(docconnection) : "";
                string docpassword = Utility.GetAppSettingsString("docpassword", "docpassword", "");
                Utility.DentrixDocPWD = (docpassword != null && docpassword != "") ? Utility.DecryptString(docpassword) : "";
                if (Utility.DentrixDocConnString == "" || Utility.DentrixDocPWD == "")
                {
                    #region comment
                    //CreateShortcut();
                    if (IsConfigureExe)
                    {
                        //string SyncCallCount = Utility.DecryptString(Utility.GetAppSettingsString("SyncCallCount", "SyncCallCount", ""));
                        //// MessageBox.Show("Pozative SyncCallCount : " + SyncCallCount);
                        //if (SyncCallCount != null)
                        //{
                        //    if (SyncCallCount == "0" || SyncCallCount == "1" || SyncCallCount == "2" || (SyncCallCount == "3" && IsConfigureExe))
                        //    {
                        bool IsEHRConnected = false;
                        StringBuilder path = new StringBuilder(512);
                        string DentrixPath = "";
                        if (CommonFunction.GetDentrixG62ExePath(path) == 0)
                        {
                            DentrixPath = path.ToString();
                            string SPath = Application.StartupPath;
                            string DPath = DentrixPath.Substring(0, DentrixPath.LastIndexOf("\\"));
                            if (!Directory.Exists(DPath))
                            {
                                Utility.WriteToErrorLogFromAll("Dentrix EHR path not found for PDF Attachment");
                            }

                            string ExeFileName = SPath + "\\DTX_Helper.exe";

                            File.Copy(ExeFileName, DPath + "\\DTX_Helper.exe", true);
                            File.Copy(SPath + "\\DTX_Helper.exe.config", DPath + "\\DTX_Helper.exe.config", true);
                            //File.Copy(ExeFileName, DPath + "\\Adit.dtxkey", true);
                            if (File.Exists(ExeFileName))
                            {
                                #region call Dentrix6.2+
                                //DENTRIXAPI_GetConnectionString:
                                Process myProcess = new Process();
                                myProcess.StartInfo.UseShellExecute = false;
                                myProcess.StartInfo.FileName = DPath + "\\DTX_Helper.exe";
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.StartInfo.Arguments = "true";
                                myProcess.StartInfo.Verb = "runas";
                                myProcess.Start();
                                myProcess.WaitForExit();
                                //DentrixConFileName = Application.StartupPath + "\\DTX_Helper" + "\\DentrixConnectionString.txt";
                                try
                                {
                                    docconnection = Utility.GetAppSettingsString("docconnection", "docconnection", "");
                                    Utility.DentrixDocConnString = (docconnection != null && docconnection != "") ? Utility.DecryptString(docconnection) : "";
                                    docpassword = Utility.GetAppSettingsString("docpassword", "docpassword", "");
                                    Utility.DentrixDocPWD = (docpassword != null && docpassword != "") ? Utility.DecryptString(docpassword) : "";
                                    // MessageBox.Show(Utility.DentrixDocPWD);
                                    //if (Utility.DentrixDocPWD == "")
                                    //{
                                    //    SyncCallCount = Utility.DecryptString(Utility.GetAppSettingsString("SyncCallCount", "SyncCallCount", ""));
                                    //    // MessageBox.Show("Pozative SyncCallCount : " + SyncCallCount);
                                    //    if (SyncCallCount == "0" || SyncCallCount == "1" || SyncCallCount == "2" || (SyncCallCount == "3" && IsConfigureExe))
                                    //    {
                                    //        goto DENTRIXAPI_GetConnectionString;
                                    //    }
                                    //    else
                                    //    {
                                    //        Utility.WriteToErrorLogFromAll("Dentrix EHR path not found for PDF Attachment");
                                    //    }

                                    //}
                                }
                                catch (Exception ex2)
                                {
                                    Utility.WriteToErrorLogFromAll("Dentrix EHR path not found for PDF Attachment" + ex2.Message);
                                }
                                File.Delete(DPath + "\\DTX_Helper.exe");
                                File.Delete(DPath + "\\DTX_Helper.exe.config");
                                File.Delete(DPath + "\\Adit.dtxkey");
                                // File.Delete(DPath + "\\DentrixConnectionString.txt");
                                #endregion
                            }
                        }
                        if (Utility.DentrixDocConnString == "" || Utility.DentrixDocPWD == "")
                        {
                            Utility.WriteToErrorLogFromAll("Dentrix Doc Password not found for PDF Attachment");
                        }
                        else
                        {
                            try
                            {
                                if (docconnection != null && docconnection != "")
                                {
                                    SystemBAL.UpdateSingleFieldInTable("Service_Installation", "DentrixPDFConstring", Utility.EncryptString(Utility.DentrixDocConnString), " Where Installation_ID = 1");
                                }
                                if (docpassword != null && docpassword != "")
                                {
                                    SystemBAL.UpdateSingleFieldInTable("Service_Installation", "DentrixPDFPassword", Utility.EncryptString(Utility.DentrixDocPWD), " Where Installation_ID = 1");
                                }
                            }
                            catch {
                            }
                        }

                        //    }
                        //}
                    }
                    #endregion
                }
                else
                {
                    try
                    {
                        if (docconnection != null && docconnection != "")
                        {
                            SystemBAL.UpdateSingleFieldInTable("Service_Installation", "DentrixPDFConstring", Utility.EncryptString(Utility.DentrixDocConnString), " Where Installation_ID = 1");
                        }
                        if (docpassword != null && docpassword != "")
                        {
                            SystemBAL.UpdateSingleFieldInTable("Service_Installation", "DentrixPDFPassword", Utility.EncryptString(Utility.DentrixDocPWD), " Where Installation_ID = 1");
                        }
                    }
                    catch
                    { }
                }
            }
            catch (Exception ex3)
            {
                Utility.WriteToErrorLogFromAll("Error During PDF Attachment" + ex3.Message);
            }

        }         
    }
}
