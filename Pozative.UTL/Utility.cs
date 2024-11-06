using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pozative.BO;
using RestSharp;

namespace Pozative.UTL
{
    public class Utility
    {
        public static string LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();
        public static string AditApp_TimeZoneWeb_ID = "";
        public static bool isSqlServer = false;
        public static string Organization_ID = string.Empty;
        public static string Organization_Name = string.Empty;
        public static string Location_ID = string.Empty;
        public static string Location_Name = string.Empty;
        public static string AppointmentEHRIds = string.Empty;
        public static string Loc_ID = string.Empty;
        public static string User_ID = string.Empty;
        public static string AditLocationName = string.Empty;
        public static int Application_ID = 0;
        public static string Application_Version = string.Empty;
        public static string Application_Name = string.Empty;
        public static string EHR_VersionNumber = string.Empty;
        public static string ConnectionLog = string.Empty;
        public static string APISessionToken = string.Empty;
        public static string ServerType = string.Empty;
        public static string EHR_UserLogin_ID = string.Empty;
        public static DataTable dtLocationWiseUser = new DataTable();
        public static string WindowsService_Version = string.Empty;
        public static string Server_App_Version = "1.0.0.36";
        public static string System_processorID = "";
        public static DateTime Application_StartDate;
        public static string EHRHostname = string.Empty;
        public static string EHRIntegrationKey = string.Empty;
        public static string EHRUserId = string.Empty;
        public static string EHRPassword = string.Empty;
        public static string EHRDatabase = string.Empty;
        public static string EHRPort = string.Empty;
        public static string DBConnString = string.Empty;
        public static string DentrixDocConnString = string.Empty;
        public static string DentrixDocPWD = string.Empty;
        public static bool ChartNumberIsNumeric = false;
        public static string ChartNumberIsNumericstr = "";
        public static bool Is_PWPatientCellPhoneAvailable = false;
        public static bool Is_PWPatientFillerAvailable = false;
        public static string Adit_User_Email_Id = string.Empty;
        public static string PW_InactivePatientCodes = string.Empty;
        public static string Adit_User_Email_Password = string.Empty;
        public static string EHR_Sub_Version = string.Empty;

        public static string PA_Server_user = string.Empty;
        public static string PA_Server_password = string.Empty;
        public static string PA_Server_Name = string.Empty;
        public static string PA_Server_database = string.Empty;

        public static bool IsApplicationIdleTimeOff = true;
        public static bool IsApplicationIdleTimeSet = false;
        public static DateTime AppIdleStartTime;
        public static DateTime AppIdleStopTime;

        public static DateTime ApplicationInstalledTime;

        public static string AditDocTempPath = "";
        public static string AditPatientProfileImagePath = "";
        public static string AditPatientProfileDefaultImagePath = "";

        public static bool GenerateRandomPatientId = false;
        public static Int16 ProviderHourPushCounter = 200;
        public static Int16 TrackerScheduleInterval = 15;
        public static Int16 TrackerNoteMove = 0;
        public static bool IsExternalAppointmentSync = false;

        //   public static bool AllowToChangeSystemDateFormat = false;
        public static Int16 SyncPartialMinutes = 100;
        public static string EHRDocPath = "";
        public static string EHRProfileImagePath = "";

        public static bool Is_InternetConnectivity = false;

        public static bool AditSync = false;
        public static bool ApptAutoBook = true;
        public static int imageuploadbatch = 10;
        public static bool SyncPracticeAnalytics = false;
        public static bool SyncPracticeAnalytics_Enabled = false;
        public static bool SyncPracticeAnalytics_Disabled = false;
        public static DateTime LastSyncDateAditServer;
        public static DateTime LastSyncDatetimePAServer;
        public static DateTime LastSyncDatetimePAServerForAPICall;
        public static DateTime LastPaymentSMSCallSyncDateAditServer;
        public static bool PozativeSync = false;
        public static bool AditLocationSyncEnable = true;
        public static string PozativeEmail = string.Empty;
        public static string PozativeLocationID = string.Empty;
        public static string PozativeLocationName = string.Empty;
        public static string ApplicationDatetimeFormat = "MM/dd/yyyy hh:mm:ss tt";
        //New
        public static string ApplicationDatetimeFormatWithoutTT = "MM/dd/yyyy hh:mm:ss";
        public static bool is_scheduledCustomhour = false;

        public static bool DontAskPasswordOnSaveSetting = false;
        public static bool NotAllowToChangeSystemDateFormat = false;

        public static bool ISEagelsoftConnected = false;
        public static string WebAdminUserToken = string.Empty;
        public static string EHR_DentrixG62_ConnString = string.Empty;
        public static string HostName_Pozative = "https://pozative.com/";                  // Pozative Live

        ///////////SingleRecordHostName///////////////////////////
        public static string HostName_Adit = "https://api.adit.com/";                   // Adit Live 
        //public static string HostName_Adit = "https://betaapi.adit.com/";                  // Adit (beta) 
        // public static string HostName_Adit = "http://192.168.1.4:8086/";                // Bharat Local
        // public static string HostName_Adit = "http://192.168.1.3:1337/";                // Dhaval Local
        // public static string HostName_Adit = "http://192.168.1.11:1337/";               // Nitin Local
        // public static string HostName_Adit = "http://192.168.1.7:1337/";                // Vishal Local
        // public static string HostName_Adit = "https://192.168.1.127:8081/";             // Chintan Local
        // public static string HostName_Adit = "http://192.168.1.11:1337/";                // Naeem Local 

        ///////////MultiRecordHostName////////////////////////////
        public static string MultiRecordHostName = "https://sync.adit.com/";          // Adit Live 
        // public static string MultiRecordHostName = "https://betasync.adit.com/";         // Adit (beta) 
        // public static string MultiRecordHostName = "http://192.168.1.4:5351/";        // Bharat Local
        // public static string MultiRecordHostName = "http://192.168.1.3:5351/";        // Dhaval Local
        // public static string MultiRecordHostName = "http://192.168.1.11:5351/";        // Naeem Local

        public static string sEventServiceName = "AditEventListener";

        public static DataTable DtLocationList = new DataTable();
        public static DataTable DtInstallServiceList = new DataTable();
        public static DataTable DtOrganizationList = new DataTable();

        public static int nEnoughStorageErrorCount = 0;
        public static DateTime DtEnoughStorageError;
        public static bool UseMaxBufferSize = false;
        public static bool UseProvidersForAllClinicOpendental = false;
        public static bool OpenDentalOldPatSync = false;
        public static bool isDebugLogRequired;
        //rooja code start--------------

        //rooja 25-8-23
        public static string Push_lastSyncAppointment = "";
        public static string Push_lastSyncAppointment_Type = "";
        public static string Push_lastSyncConfirm_Appointment = "";
        public static string Push_lastSyncOperatory = "";
        public static string Push_lastSyncOperatory_hours = "";
        public static string Push_lastSyncPatient = "";
        public static string Push_lastSyncPatient_payment_log = "";
        public static string Push_lastSyncPatient_status = "";
        public static string Push_lastSyncProvider = "";       
        public static string Pull_lastSyncSMS_call_log = "";
        public static string Push_provider_hours = "";
        public static string pull_appointment = "";
        public static string pull_patient_Form = "";
        public static string pull_patient_document = "";
        public static string pull_treatmentplan_document = "";        
        public static string currentDate = "";
        public static string appointmentlocationId = "";
        public static string user = "";

        public static bool ODClinic_Number_Flag = false;//rooja 31-10-23
      
        public static bool IsNewConnStringUpdated = false;
        public static string ODsqlConnErrorMsg = string.Empty;

        public static bool is_scheduledCustomhourOperatory = false;
        public static bool is_scheduledCustomhourProvider = false;
        public static List<string> CustomhourOperatoryIds = new List<string>();
        public static List<string> CustomhourProviderIds = new List<string>();

        //Declare  Pull Log Parameter
        public static string _filename_EHR_appointment = "EHRAppointment_Confirmation";
        public static string _EHRLogdirectory_EHR_appointment = "EHRAppointment_Confirmation";

        public static string _filename_Operatory = "Operatory";
        public static string _EHRLogdirectory_Operatory = "Operatory";

        public static string _filename_ApptType = "ApptType";
        public static string _EHRLogdirectory_ApptType = "ApptType";

        public static string _filename_appointment = "Appointment_Write";
        public static string _EHRLogdirectory_appointment = "Appointment_Write";

        public static string _filename_EHR_PatientFormt = "Patient_Form";
        public static string _EHRLogdirectory_EHR_PatientForm = "PatientForm";

        public static string _filename_Patient_Portal = "Patient_Portal";
        public static string _EHRLogdirectory_Patient_Portal = "Patient_Portal";

        public static string _filename_EHR_Patient_Document = "Patient_Document";
        public static string _EHRLogdirectory_EHR_Patient_Document = "Patient_Document Confirmation";

        public static string _filename_EHR_Payment = "Payment_Confirmation";
        public static string _EHRLogdirectory_EHR_Payment = "Payment Confirmation";

        public static string _filename_EHR_patient_sms_call = "patient_sms_call";
        public static string _EHRLogdirectory_EHR_patient_sms_call = "patient sms call";

        public static string _filename_EHR_patientoptout = "Patientoptout_Confirmation";
        public static string _EHRLogdirectory_EHR_patientoptout = "Patientoptout Confirmation";

        public static string _filename_EHR_treatmentplan_document = "treatmentplan_document";
        public static string _EHRLogdirectory_EHR_treatmentplan_document = "treatmentplan document";

        public  static string _filename_EHR_PatientFollowUp = "patientfollowup";
        public static string _EHRLogdirectory_EHR_PatientFollowUp = "patientfollowup";

        public static string _filename_StatusAppointmentlist = "Deleteappointmentfromweb_Confirmation";
        public  static string _EHRLogdirectory_StatusAppointmentlist = "Delete appointment from web";

        public static string _filename_ehr_appointment_without_patientid = "EHRAppointment_WithOut_PatientID";
        public static string _EHRLogdirectory_ehr_appointment_without_patientid = "EHRAppointment WithOut PatientID";

        public static string _filename_Provider = "Provider";
        public static string _EHRLogdirectory_Provider = "Provider";

        //rooja
        public static string _filename_EHR_InsuranceCarrier_document = "InsuranceCarrier_document";
        public static string _EHRLogdirectory_EHR_InsuranceCarrier_document = "InsuranceCarrier document";

        public static bool IsChinesePDF = false;
        public static DateTime AlterDBFileExecutionCheck = DateTime.Now;

        public static MasterSyncBatchSizeDetails mstSyncBatchSize = new MasterSyncBatchSizeDetails();

        #region ZohoDetails
        public static string EHRUserName = "";
        public static string EHRUserPassword = "";
        public static string EHRUserResult = "";
        public static string WindowsUserName = "";
        public static string WindowsUserPassword = "";
        public static bool WindowsUserResult = false;
        public static bool IsConfirmed = false;
        public static bool IsInstalled = false;
        public static string ZohoInstall = "";
        #endregion

        public static DateTime Datetimesetting()
        {
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;
            //timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(GetValueFromAppConfig("GetTimeZoneID"));
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(LocationTimeZone);
            dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            return dateTime;
        }

        public static string GetCurrentDatetimestring()
        {
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;
            if (!Utility.NotAllowToChangeSystemDateFormat)
            {
                // //timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(GetValueFromAppConfig("GetTimeZoneID"));
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(LocationTimeZone);
                dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                return dateTime.ToString(ApplicationDatetimeFormat);
            }
            else
            {
                dateTime = DateTime.Now;
                return dateTime.ToString();
            }
        }

        public static DateTime DateTryParse(string inDateToParse, string inDateFormat)
        {
            DateTime dtReturn = new DateTime();
            try
            {
                DateTime.TryParseExact(inDateToParse, inDateFormat, null, DateTimeStyles.None, out dtReturn);
            }
            catch (Exception)
            {
                //dtReturn = Datetimesetting();
                dtReturn = DateTime.Now;
            }
            return dtReturn;
        }

        public static string GetValueFromAppConfig(string Name)
        {
            string value = string.Empty;
            try
            {
                value = ConfigurationManager.AppSettings[Name].ToString();
            }
            catch (Exception)
            {
            }
            return value;
        }
        public static void WriteSyncPullLog(string filename, string rootdirectory, string pullsynclogData, bool IsStringContacted = true)
        {
            WriteToPullSyncLogFile_All(filename, rootdirectory, pullsynclogData, IsStringContacted);
        }
        public static void WriteToPullSyncLogFile_All(string filename, string rootdirectory, string SyncpullLogString, bool IsStringContacted = true)
        {

            try
            {

                DateTime currentDate = DateTime.Now;
                string year = currentDate.Year.ToString();
                string month = currentDate.Month.ToString("D2");

                string fileName = filename + $"_{year}{month}{currentDate.Day:00}.txt";
                DateTime dtCurrentDtTime = Datetimesetting();
               // string ErrorLogFilePath = "SyncLogFile" + "\\" + rootdirectory + "\\" + dtCurrentDtTime.ToString("yyyy / MM").Replace(" / ", "\\");
                string SyncLogFileName = Application.StartupPath + "\\" + "SyncLogFile" + "\\" + rootdirectory + "\\" + dtCurrentDtTime.ToString("yyyy / MM").Replace(" / ", "\\") + "\\" + fileName + "";
                try
                {
                    //if (!System.IO.Directory.Exists(ErrorLogFilePath))
                    //{
                    //    System.IO.Directory.CreateDirectory(ErrorLogFilePath);
                    //}

                    if (!File.Exists(SyncLogFileName))
                    {
                        File.Create(SyncLogFileName).Dispose();
                    }

                    using (StreamWriter writer = new StreamWriter(SyncLogFileName, true))
                    {
                        writer.WriteLine(currentDate.ToString() + " -→ " + SyncpullLogString);
                        writer.Close();
                    }
                }
                catch (Exception)
                {
                }                
            }
            catch (Exception ex)
            {
                //throw;
            }
        }
        public static void WriteToErrorLogFromAll(string ErrorLogString)
        {
            try
            {
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
                     WriteToErrorLogFromAll("Error from Get db file not foud backdb failed:" + ex.Message);
                }

                try
                {
                    Match m = Regex.Match(ErrorLogString, @"System.OutOfMemoryException");  
                    if (m.Success && m3.Success)
                    {
                        //string[] t = Directory.GetFiles(Environment.CurrentDirectory, "Pozative.sdf");
                        //Array.ForEach(t, File.Delete);
                        //Utility.Get_LocalDbFromWeb();
                        //Utility.UpdateLocalDB_From_BackupDB();
                        Utility.ResolvedOutofMemeoryException();
                        Utility.RestartApp();
                    }
                }
                catch (Exception ex)
                {
                    WriteToErrorLogFromAll("Error from memory Exception  failed." + ex.Message);
                }

                try
                {
                    if (ErrorLogString.ToLower().IndexOf(" Fatal failure of the lock susbsytem for this database") >= 0)
                    {
                        Application.Restart();
                    }

                    if (ErrorLogString.ToLower().IndexOf("corrupted") >= 0)
                    {
                        try
                        {
                            RecoveryDatabase();
                            //string[] t = Directory.GetFiles(Environment.CurrentDirectory, "Pozative.sdf");
                            //Array.ForEach(t, File.Delete);
                            //Get_LocalDbFromWeb();
                            //UpdateLocalDB_From_BackupDB();
                            Utility.RestartApp();
                        }
                        catch(Exception ex)
                        {
                            WriteToErrorLogFromAll("Error from corruption db backup failed" + ex.Message);
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
                                    Application.Restart();
                                }
                            }
                            else
                            {
                                Utility.nEnoughStorageErrorCount = 0;
                            }                            
                        }
                        catch
                        {
                            //frmPozative.GetPozativeConfiguration();
                            //Utility.WriteToSyncLogFile_All("GetPozativeConfiguration from ADIT (Adit App to Backup Database ) Successfully.");
                        }
                    }
                    //else if (ErrorLogString.ToLower().IndexOf("The database file cannot be found") >= 0)
                    //{

                    //}
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

                    if (ErrorLogString.ToLower().IndexOf(" Fatal failure of the lock susbsytem for this database") >= 0)
                    {
                        Application.Restart();
                    }
                }
                catch (Exception)
                {
                }

                try
                {
                    try
                    {                                

                        if (AlterDBFileExecutionCheck < DateTime.Now)
                        {
                            AlterDBFileExecutionCheck = DateTime.Now.AddDays(1);
                            CheckErrorLogFile();
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteToErrorLogFromAll("CheckErrorLogFile Faild." + ex.Message);
                    }
                }
                catch (Exception)
                {

                }

                ConnectionLog = dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ErrorLogString + "\n" + ConnectionLog;

            }
            catch (Exception)
            {
            }
        }



        public static bool RecoveryDatabase()
        {
            bool result = false;
            try
            {
                SqlCeEngine engine =
            // new SqlCeEngine("Data Source = " + txtDbPath.Text + ";Persist Security Info=False; Max Database Size=4000; Password =Smile");
            new SqlCeEngine(CommonUtility.LocalConnectionString());
                //Data Source=|DataDirectory|\Pozative.sdf;Persist Security Info=False; Max Database Size=4000; Password =Smile
                if (false == engine.Verify())
                {
                    Utility.WriteToSyncLogFile_All("Database File is Corrupted");
                    try
                    {
                        // engine.Repair(CommonUtility.LocalConnectionString(), RepairOption.RecoverCorruptedRows);
                        engine.Repair(null, RepairOption.DeleteCorruptedRows);
                        Utility.WriteToSyncLogFile_All("Database File is Recovered");
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        // Utility.WriteToSyncLogFile_All("Err_ Recover Db File " + ex.Message.ToString());
                    }
                }
                else
                {
                    // label1.Text = "Database is Not corrupted";
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Err_RecoveryDatabase " + ex.Message.ToString());
            }
            return result;
        }


        public static void WriteToSyncLogFile_All(string SyncLogString)
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
            }
            catch (Exception)
            {
                //throw;
            }

            ConnectionLog = dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + SyncLogString + "\n" + ConnectionLog;

        }

        public static void WriteToConnectionLog(string ConnLogString)      
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            string ConnLogFilePath = Application.StartupPath + "\\" + "ConnectionLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
            string ConnLogFileName = ConnLogFilePath + "\\" + dtCurrentDtTime.ToString("yyyy_MM_dd") + ".txt";

            try
            {
                if (!System.IO.Directory.Exists(ConnLogFilePath))
                {
                    System.IO.Directory.CreateDirectory(ConnLogFilePath);
                }

                if (!File.Exists(ConnLogFileName))
                {
                    File.Create(ConnLogFileName).Dispose();
                    IsNewConnStringUpdated = false;
                }

                using (StreamWriter writer = new StreamWriter(ConnLogFileName, true))
                {
                    writer.WriteLine(dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ConnLogString);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLogFromAll("WriteToConnectionLog - " + ex.Message);
            }

            ConnectionLog = dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + ConnLogString + "\n" + ConnectionLog;

        }

        public static string EncryptString(string input)
        {
            string EDKey = "sblw-3hn8-sqoy19";
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(EDKey);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string DecryptString(string input)
        {
            string EDKey = "sblw-3hn8-sqoy19";
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(EDKey);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string AddSpecialCharacter(string values)
        {
            string returnvalue = "";
            try
            {
                int len = values.ToString().Trim().Length;
                string prefixstring = "";
                string suffixstring = "";
                if (len > 4)
                {
                    for (int i = 1; i <= (len-4); i++)
                    {
                        prefixstring = prefixstring + "*";
                    }

                    suffixstring = values.ToString().Trim().Substring((len - 4), (len-(len - 4)));
                    returnvalue = prefixstring + suffixstring;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
            return returnvalue;
        }

        public static string ConvertDatetimeToUTCaditFormat(string InputDatetime)
        {
            string ConvertDateTime = string.Empty;
            if (InputDatetime.ToString().Trim() != string.Empty && InputDatetime.ToString().Trim() != "0")
            {
                try
                {
                    //var tmpInputDatetime = DateTime.Parse(InputDatetime);
                    try
                    {
                        var tmpInputDatetime = DateTime.ParseExact(InputDatetime, ApplicationDatetimeFormat, null);
                        TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(LocationTimeZone);
                        ConvertDateTime = TimeZoneInfo.ConvertTimeToUtc(tmpInputDatetime, tst).ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var tmpInputDatetime = DateTime.Parse(InputDatetime);
                            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(LocationTimeZone);
                            ConvertDateTime = TimeZoneInfo.ConvertTimeToUtc(tmpInputDatetime, tst).ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        catch (Exception)
                        {
                            var tmpInputDatetime = DateTime.ParseExact(InputDatetime, Utility.ApplicationDatetimeFormatWithoutTT, null);
                            TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(LocationTimeZone);
                            ConvertDateTime = TimeZoneInfo.ConvertTimeToUtc(tmpInputDatetime, tst).ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                    }
                }
                catch (Exception)
                {
                    ConvertDateTime = "";
                }
            }
            return ConvertDateTime;
        }

        public static string ConvertRecallType_ApptStatusString(string InputString)
        {
            string ConvertString = string.Empty;
            try
            {
                ConvertString = string.Concat(InputString.Select(c => char.IsUpper(c) ? "_" + c.ToString().ToLower() : c.ToString())).TrimStart();
                ConvertString = ConvertString.Replace(" ", "_");
            }
            catch (Exception)
            {
                ConvertString = InputString;
            }

            return ConvertString;
        }

        public static string ConvertDatetimeToCurrentLocationFormat(string InputDatetime)
        {
            string strLocDatetime = string.Empty;
            if (InputDatetime.ToString().Trim() != string.Empty && InputDatetime.ToString().Trim() != "0")
            {
                try
                {
                    double timestamp = Convert.ToDouble(InputDatetime);
                    if (timestamp > 0)
                    {
                        System.DateTime ConvertDatetimeinLocatinZone = new System.DateTime(1970, 1, 1, 0, 0, 0);
                        double seconds = timestamp / 1000;
                        ConvertDatetimeinLocatinZone = ConvertDatetimeinLocatinZone.AddSeconds(seconds);
                        TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(LocationTimeZone);
                        DateTime LocDatetime = TimeZoneInfo.ConvertTimeFromUtc(ConvertDatetimeinLocatinZone, nzTimeZone);
                        //Comment and new by yogesh for not check timezone
                        if (!Utility.NotAllowToChangeSystemDateFormat)
                        {

                            strLocDatetime = LocDatetime.ToString(ApplicationDatetimeFormat);
                        }
                        else
                        {
                            strLocDatetime = LocDatetime.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    strLocDatetime = DateTime.Now.ToString();
                }
            }
            return strLocDatetime;
        }

        public static string ConvertApptPatientBirthdateToCurrentLocationFormat(string InputDatetime)
        {
            string strLocDatetime = string.Empty;
            if (InputDatetime.ToString().Trim() != string.Empty && InputDatetime.ToString().Trim() != "0")
            {
                try
                {
                    double timestamp = Convert.ToDouble(InputDatetime);

                    System.DateTime ConvertDatetimeinLocatinZone = new System.DateTime(1970, 1, 1, 0, 0, 0);
                    double seconds = timestamp / 1000;
                    ConvertDatetimeinLocatinZone = ConvertDatetimeinLocatinZone.AddSeconds(seconds);
                    TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(LocationTimeZone);
                    DateTime LocDatetime = TimeZoneInfo.ConvertTime(ConvertDatetimeinLocatinZone, nzTimeZone);
                    strLocDatetime = LocDatetime.ToString(ApplicationDatetimeFormat);

                }
                catch (Exception)
                {
                    strLocDatetime = string.Empty;
                }
            }
            return strLocDatetime;
        }


        private static DataTable CreateIgnoreColumnTable()
        {
            DataTable IgnoreColumnTable = new DataTable();
            IgnoreColumnTable.Columns.Add("ColumnName", typeof(string));
            IgnoreColumnTable.Columns.Add("DataType", typeof(string));
            return IgnoreColumnTable;
        }

        public static DataTable CreateDistinctRecords(DataTable dtList, string ignoreColumnInDistinct, string appintmentId)
        {
            string strLocDatetime = string.Empty;
            try
            {
                int colAdd = 0;
                string[] ignoreColumns = null;
                DataTable IgnoreColumnTable = CreateIgnoreColumnTable();
                if (ignoreColumnInDistinct != string.Empty)
                {
                    ignoreColumns = ignoreColumnInDistinct.Split(',');
                }
                string[] dcColumns = new string[ignoreColumnInDistinct == string.Empty ? dtList.Columns.Count : (dtList.Columns.Count - ignoreColumns.Count())];

                foreach (DataColumn dcColumn in dtList.Columns)
                {
                    if (ignoreColumns == null || (ignoreColumns != null && ignoreColumns.AsEnumerable().Where(o => o.ToString().ToUpper() == dcColumn.ColumnName.ToString().ToUpper()).Count() == 0))
                    {
                        dcColumns[colAdd] = dcColumn.ColumnName;
                        colAdd++;
                    }
                    else
                    {
                        DataRow dr = IgnoreColumnTable.Rows.Add();
                        dr["ColumnName"] = dcColumn.ColumnName;
                        dr["DataType"] = dcColumn.DataType;
                    }
                }
                // DataView dtView = new DataView(dtList);
                DataTable dtResult = dtList.DefaultView.ToTable(true, dcColumns);

                if (ignoreColumnInDistinct != string.Empty && dtResult.Rows.Count != dtList.Rows.Count)
                {
                    foreach (DataRow dr in IgnoreColumnTable.Rows)
                    {
                        dtResult.Columns.Add(dr["ColumnName"].ToString(), Type.GetType(dr["DataType"].ToString()));
                    }

                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        var result = dtList.AsEnumerable().Where(a => Convert.ToString(a.Field<object>(appintmentId)) == dtResult.Rows[i][appintmentId].ToString()).ToList();
                        foreach (string b in ignoreColumns)
                        {
                            if (result != null && result.Count() > 1)
                            {
                                var Orderbyvalue = result.AsEnumerable().OrderByDescending(c => c.Field<object>(b.ToString())).FirstOrDefault();
                                if (Orderbyvalue != null)
                                {
                                    dtResult.Rows[i][b.ToString()] = Orderbyvalue[b.ToString()].ToString();
                                }
                            }
                            else
                            {
                                if (result != null)
                                {
                                    dtResult.Rows[i][b.ToString()] = result[0][b.ToString()];
                                }
                            }
                        }
                    }
                }
                if (dtResult.Rows.Count != dtList.Rows.Count)
                {
                    return dtResult;
                }
                else
                {
                    return dtList;
                }
            }
            catch (Exception)
            {
                strLocDatetime = DateTime.Now.ToString(ApplicationDatetimeFormat);
                return dtList;
            }
        }

        /// <summary>
        /// SetNextVisitDate Used to set the NextAppointment DateTime From All EHR.
        /// </summary>
        /// <param name="dtOpenDentalPatientwiseNextAppontment">Datatable Pass Next Appointment containts records</param>
        /// <param name="EHRPatientIdColumnName">Pass stringType Columns Name Of EHR PatientId</param>
        /// <param name="LocalPatientIdColumnsName">Pass stringType Columns Name Of Local PatientId</param>
        /// <param name="NextVisitDateColumnsName">Pass stringType Columns Name Of NextVisitDateTime</param>
        /// <param name="NextVisitTempColumnsName">Pass stringType Columns Name Of Temprory columns to set NextVisitDateTime</param>
        /// <param name="drRow">Pass string type PatientEHRId to map the Appointment Records</param>
        public static string SetNextVisitDate(DataTable dtOpenDentalPatientwiseNextAppontment, string EHRPatientIdColumnName, string LocalPatientIdColumnsName, string NextVisitDateColumnsName, string PatientEHRId)
        {
            try
            {
                DataRow[] PatNextApptData = dtOpenDentalPatientwiseNextAppontment.Copy().Select(EHRPatientIdColumnName + " = '" + PatientEHRId + "'");
                if (PatNextApptData.Length > 0)
                {
                    var result = dtOpenDentalPatientwiseNextAppontment.AsEnumerable().Where(o => o.Field<object>(EHRPatientIdColumnName).ToString() == PatientEHRId);
                    if (result.Count() > 0)
                    {
                        return result.OrderBy(a => a.Field<object>(NextVisitDateColumnsName)).First().Field<object>(NextVisitDateColumnsName).ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string SetLastVisitDate(DataTable dtOpenDentalPatientwiseLastAppontment, string EHRPatientIdColumnName, string LocalPatientIdColumnsName, string LastVisitDateColumnsName, string PatientEHRId)
        {
            try
            {
                DataRow[] PatNextApptData = dtOpenDentalPatientwiseLastAppontment.Select(EHRPatientIdColumnName + " = '" + PatientEHRId + "'");
                if (PatNextApptData.Length > 0)
                {
                    var result = dtOpenDentalPatientwiseLastAppontment.AsEnumerable().Where(o => o.Field<object>(EHRPatientIdColumnName).ToString() == PatientEHRId);
                    if (result.Count() > 0)
                    {
                        return result.OrderBy(a => a.Field<object>(LastVisitDateColumnsName)).Last().Field<object>(LastVisitDateColumnsName).ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }


        public static string CheckValidDatetime(string InputDatetime)
        {
            string strLocDatetime = string.Empty;
            try
            {
                if (InputDatetime != string.Empty)
                {
                    DateTime date1 = new DateTime(1800, 1, 1, 0, 0, 0);
                    DateTime tmpBDate = Convert.ToDateTime(InputDatetime);
                    DateTime date2 = new DateTime(tmpBDate.Year, tmpBDate.Month, tmpBDate.Day, 0, 0, 0);
                    int result = DateTime.Compare(date1, date2);
                    if (result < 0)
                    {
                        strLocDatetime = !Utility.NotAllowToChangeSystemDateFormat ? tmpBDate.ToString(ApplicationDatetimeFormat) : tmpBDate.ToString();
                    }
                }
                return strLocDatetime;
            }
            catch (Exception)
            {
                strLocDatetime = DateTime.Now.ToString(ApplicationDatetimeFormat);
                return strLocDatetime;
            }
        }

        public static string ConvertContactNumber(string InputnumberString)
        {
            string OutputnumberString = InputnumberString;
            if (OutputnumberString != string.Empty)
            {
                OutputnumberString = OutputnumberString.Replace("(", "");
                OutputnumberString = OutputnumberString.Replace(")", "");
                OutputnumberString = OutputnumberString.Replace("-", "");
                OutputnumberString = OutputnumberString.Replace("'", "");
                OutputnumberString = OutputnumberString.Replace(" ", "");
            }
            return OutputnumberString;
        }

        public static string ConvertvarToString(object inputString)
        {
            string OutputStaing = string.Empty;
            try
            {
                OutputStaing = inputString.ToString();
                return OutputStaing;
            }
            catch
            {
                return OutputStaing;
            }
        }

        public static string SystemCurrentTimeZone()
        {
            string OutputStaing = TimeZone.CurrentTimeZone.StandardName.ToString().ToLower();
            try
            {
                bool isDaylight = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now);
                if (isDaylight)
                {
                    OutputStaing = TimeZone.CurrentTimeZone.StandardName.ToString().ToLower();
                    //OutputStaing = TimeZone.CurrentTimeZone.DaylightName.ToString().ToLower();
                }
                else
                {
                    OutputStaing = TimeZone.CurrentTimeZone.StandardName.ToString().ToLower();
                }

                return OutputStaing;
            }
            catch
            {
                return OutputStaing;
            }
        }


        public static string CheckEHR_ID(object inputString)
        {
            string OutputStaing = string.Empty;
            try
            {
                OutputStaing = inputString.ToString();
                if (OutputStaing == "-")
                {
                    OutputStaing = string.Empty;
                }
                return OutputStaing;
            }
            catch
            {
                return OutputStaing;
            }
        }

        public static bool IsValidEmailAddress(string email)
        {
            try
            {
                MailAddress ma = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DateDiffBetweenTwoDate(string date1, string date2)
        {
            bool tmpReturn = false;

            try
            {
                string tmpDate1 = CheckValidDatetime(date1);
                string tmpDate2 = CheckValidDatetime(date2);

                if (tmpDate1 != "")
                {
                    tmpDate1 = Convert.ToDateTime(tmpDate1).ToString(Utility.ApplicationDatetimeFormat);
                }

                if (tmpDate2 != "")
                {
                    tmpDate2 = Convert.ToDateTime(tmpDate2).ToString(Utility.ApplicationDatetimeFormat);
                }

                if (date1 != "" && date2 != "")
                {
                    if (tmpDate1 != tmpDate2)
                    {
                        tmpReturn = true;
                    }
                }
                else if (date1 != "" && date2 == "")
                {
                    tmpReturn = true;
                }
                else if (date1 == "" && date2 != "")
                {
                    tmpReturn = true;
                }
                else
                {
                    tmpReturn = false;
                }
                return tmpReturn;
            }
            catch
            {
                return tmpReturn;
            }
        }

        public static string ConvertWebTimeZoneToSystemTimeZone(string WebTimeZone)
        {
            string ConvertTimeZone = "";
            try
            {
                ConvertTimeZone = WebTimeZone.ToString().ToLower().Replace("&amp;", "&");
                if (ConvertTimeZone != string.Empty)
                {
                    if (ConvertTimeZone == "Central Time".ToLower())
                    {
                        ConvertTimeZone = "Central Standard Time";
                    }
                    else if (ConvertTimeZone == "Eastern Time".ToLower())
                    {
                        ConvertTimeZone = "Eastern Standard Time";
                    }
                    else if (ConvertTimeZone == "Mountain Time (US & Canada)".ToLower())
                    {
                        ConvertTimeZone = "Mountain Standard Time";
                    }
                    else if (ConvertTimeZone == "Pacific Time (US & Canada)".ToLower())
                    {
                        ConvertTimeZone = "Pacific Standard Time";
                    }
                    else if (ConvertTimeZone == "Tijuana".ToLower())
                    {
                        ConvertTimeZone = "Pacific Standard Time";
                    }
                    else if (ConvertTimeZone == "Kolkata".ToLower())
                    {
                        ConvertTimeZone = "India Standard Time";
                    }
                    else if (ConvertTimeZone == "Arizona".ToLower())
                    {
                        ConvertTimeZone = "US Mountain Standard Time";
                    }
                    else if (ConvertTimeZone == "Chihuahua".ToLower())
                    {
                        ConvertTimeZone = "Mexico Standard Time 2";
                    }
                    else if (ConvertTimeZone == "Mazatlan".ToLower())
                    {
                        ConvertTimeZone = "Mexico Standard Time 2";
                    }
                    else if (ConvertTimeZone == "Mexico City".ToLower())
                    {
                        ConvertTimeZone = "Mexico Standard Time";
                    }
                    else if (ConvertTimeZone == "Monterrey".ToLower())
                    {
                        ConvertTimeZone = "Mexico Standard Time";
                    }
                    else if (ConvertTimeZone == "Saskatchewan".ToLower())
                    {
                        ConvertTimeZone = "Canada Central Standard Time";
                    }
                    else if (ConvertTimeZone == "Central Time (US & Canada)".ToLower())
                    {
                        ConvertTimeZone = "Central Standard Time";
                    }
                    else if (ConvertTimeZone == "Indiana (East)".ToLower())
                    {
                        ConvertTimeZone = "US Eastern Standard Time";
                    }
                    else if (ConvertTimeZone == "Bogota".ToLower())
                    {
                        ConvertTimeZone = "SA Pacific Standard Time";
                    }
                    else if (ConvertTimeZone == "Lima".ToLower())
                    {
                        ConvertTimeZone = "SA Pacific Standard Time";
                    }
                    else if (ConvertTimeZone == "Caracas".ToLower())
                    {
                        ConvertTimeZone = "Venezuela Standard Time";
                    }
                    else if (ConvertTimeZone == "Atlantic Time (Canada)".ToLower())
                    {
                        ConvertTimeZone = "Atlantic Standard Time";
                    }
                    else if (ConvertTimeZone == "La_Paz".ToLower())
                    {
                        ConvertTimeZone = "Mexico Standard Time 2";
                    }
                    else if (ConvertTimeZone == "Santiago".ToLower())
                    {
                        ConvertTimeZone = "Pacific S.A. Standard Time";
                    }
                    else if (ConvertTimeZone == "Newfoundland".ToLower())
                    {
                        ConvertTimeZone = "Newfoundland and Labrador Standard Time";
                    }
                    else if (ConvertTimeZone == "Buenos Aires".ToLower())
                    {
                        ConvertTimeZone = "Azerbaijan Standard Time";
                    }
                    else if (ConvertTimeZone == "Samoa".ToLower())
                    {
                        ConvertTimeZone = "Samoa Standard Time";
                    }
                    else if (ConvertTimeZone == "Hawaii".ToLower())
                    {
                        ConvertTimeZone = "Hawaiian Standard Time";
                    }
                    else if (ConvertTimeZone == "Alaska".ToLower())
                    {
                        ConvertTimeZone = "Alaskan Standard Time";
                    }
                    else if (ConvertTimeZone == "Saskatchewan".ToLower())
                    {
                        ConvertTimeZone = "Canada Central Standard Time";
                    }
                }
                return ConvertTimeZone;
            }
            catch (Exception)
            {
                return ConvertTimeZone;
            }
        }



        public static string ConvertSystemTimeZoneToWebTimeZone(string CurrentSystemTimeZone)
        {
            string ConvertTimeZone = "";
            try
            {
                ConvertTimeZone = CurrentSystemTimeZone.ToString().ToLower().Replace("&amp;", "&");
                if (ConvertTimeZone != string.Empty)
                {
                    if (ConvertTimeZone == "Central Standard Time".ToLower())
                    {
                        ConvertTimeZone = "central_time_us_andamp_canada";
                    }
                    else if (ConvertTimeZone == "Eastern Standard Time".ToLower())
                    {
                        ConvertTimeZone = "eastern_standard_time";
                    }
                    else if (ConvertTimeZone == "Mountain Standard Time".ToLower())
                    {
                        ConvertTimeZone = "mountain_standard_time";
                    }
                    else if (ConvertTimeZone == "Pacific Standard Time".ToLower())
                    {
                        ConvertTimeZone = "pacific_time_us_andamp_canada";
                    }
                    else if (ConvertTimeZone == "Pacific Standard Time".ToLower())
                    {
                        ConvertTimeZone = "pacific_time_us_andamp_canada"; // tijuana
                    }
                    else if (ConvertTimeZone == "India Standard Time".ToLower())
                    {
                        ConvertTimeZone = "kolkata";
                    }
                    else if (ConvertTimeZone == "US Mountain Standard Time".ToLower())
                    {
                        ConvertTimeZone = "arizona";
                    }
                    else if (ConvertTimeZone == "Mexico Standard Time 2".ToLower())
                    {
                        ConvertTimeZone = "chihuahua";
                    }
                    else if (ConvertTimeZone == "Mexico Standard Time 2".ToLower())
                    {
                        ConvertTimeZone = "mazatlan";
                    }
                    else if (ConvertTimeZone == "Mexico Standard Time".ToLower())
                    {
                        ConvertTimeZone = "mexico_city";
                    }
                    else if (ConvertTimeZone == "Mexico Standard Time".ToLower())
                    {
                        ConvertTimeZone = "monterrey";
                    }
                    else if (ConvertTimeZone == "Canada Central Standard Time".ToLower())
                    {
                        ConvertTimeZone = "saskatchewan";
                    }
                    else if (ConvertTimeZone == "Central Standard Time".ToLower())
                    {
                        ConvertTimeZone = "central_time_us_andamp_canada";
                    }
                    else if (ConvertTimeZone == "US Eastern Standard Time".ToLower())
                    {
                        ConvertTimeZone = "indiana_east";
                    }
                    else if (ConvertTimeZone == "SA Pacific Standard Time".ToLower())
                    {
                        ConvertTimeZone = "bogota";
                    }
                    else if (ConvertTimeZone == "SA Pacific Standard Time".ToLower())
                    {
                        ConvertTimeZone = "lima";
                    }
                    else if (ConvertTimeZone == "Venezuela Standard Time".ToLower())
                    {
                        ConvertTimeZone = "caracas";
                    }
                    else if (ConvertTimeZone == "Atlantic Standard Time".ToLower())
                    {
                        ConvertTimeZone = "atlantic_time_canada";
                    }
                    else if (ConvertTimeZone == "Mexico Standard Time 2".ToLower())
                    {
                        ConvertTimeZone = "lapaz";
                    }
                    else if (ConvertTimeZone == "Pacific S.A. Standard Time".ToLower())
                    {
                        ConvertTimeZone = "santiago";
                    }
                    else if (ConvertTimeZone == "Newfoundland and Labrador Standard Time".ToLower())
                    {
                        ConvertTimeZone = "newfoundland";
                    }
                    else if (ConvertTimeZone == "Azerbaijan Standard Time".ToLower())
                    {
                        ConvertTimeZone = "buenos_aires";
                    }
                    else if (ConvertTimeZone == "Samoa Standard Time".ToLower())
                    {
                        ConvertTimeZone = "samoa";
                    }
                    else if (ConvertTimeZone == "Hawaiian Standard Time".ToLower())
                    {
                        ConvertTimeZone = "hawaii";
                    }
                    else if (ConvertTimeZone == "Alaskan Standard Time".ToLower())
                    {
                        ConvertTimeZone = "alaska";
                    }
                    else if (ConvertTimeZone == "Canada Central Standard Time".ToLower())
                    {
                        ConvertTimeZone = "saskatchewan";
                    }
                }
                return ConvertTimeZone;
            }
            catch (Exception)
            {
                return ConvertTimeZone;
            }
        }

        public static List<DataTable> SplitTable(DataTable originalTable, int batchSize)
        {
            List<DataTable> tables = new List<DataTable>();
            int i = 0;
            int j = 1;
            DataTable newDt = originalTable.Clone();
            newDt.TableName = "Table_" + j;
            newDt.Clear();

            int lastrow = originalTable.Rows.Count;
            int curRow = 1;

            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newDt.NewRow();
                newRow.ItemArray = row.ItemArray;
                newDt.Rows.Add(newRow);
                i++;
                if (i == batchSize || lastrow == curRow)
                {
                    tables.Add(newDt);
                    j++;
                    newDt = originalTable.Clone();
                    newDt.TableName = "Table_" + j;
                    newDt.Clear();
                    i = 0;
                }
                curRow++;
            }
            return tables;
        }

        public static string GenerateAuthonticationKey(string LocationID)
        {
            try
            {
                //string utcdate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm");
                const string Key = "$15#08#47#2020#@DIT!OPENDENTALEH"; // must be 32 character
                const string IV = "$#!$#!$#!##2020X"; // must be 16 character

                string message = LocationID.ToString() + "|" + Organization_ID.ToString();
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = UTF8Encoding.UTF8.GetBytes(IV);
                aes.Key = UTF8Encoding.UTF8.GetBytes(Key);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                byte[] data = Encoding.UTF8.GetBytes(message);
                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(dest);
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void CreatePatientNameTOCompare(string FirstName, string LastName, ref string PatientName, ref string RevPatientName)
        {
            try
            {
                if (FirstName != "" && LastName != "")
                {
                    PatientName = FirstName + " " + LastName;
                    RevPatientName = LastName + " " + FirstName;
                }
                if (FirstName != "" && LastName == "")
                {
                    if (FirstName.Contains(" "))
                    {
                        var firstSpaceIndex = FirstName.IndexOf(" ");
                        PatientName = FirstName.ToString();
                        RevPatientName = FirstName.Substring(firstSpaceIndex + 1, (FirstName.Length - firstSpaceIndex - 1)).Trim() + " " + FirstName.Substring(0, firstSpaceIndex).Trim();
                    }
                    else
                    {
                        PatientName = FirstName.ToString();
                        RevPatientName = FirstName.ToString();
                    }
                }
                if (LastName != "" && FirstName == "")
                {
                    if (LastName.Contains(" "))
                    {
                        var firstSpaceIndex = LastName.IndexOf(" ");
                        PatientName = LastName.ToString();
                        RevPatientName = LastName.Substring(firstSpaceIndex + 1, (LastName.Length - firstSpaceIndex - 1)).Trim() + " " + LastName.Substring(0, firstSpaceIndex).Trim();
                    }
                    else
                    {
                        PatientName = LastName.ToString();
                        RevPatientName = LastName.ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetLocationIdByClinicNumber(string ClinicNumber)
        {
            try
            {
                if (DtLocationList != null && DtLocationList.Rows.Count > 1)
                {
                    DataRow[] row = DtLocationList.Copy().Select("Clinic_Number = '" + ClinicNumber + "'");
                    if (row.Length > 0)
                        return row[0]["Location_Id"].ToString();
                    else
                        return Location_ID;
                }
                else
                    return Location_ID;
            }
            catch (Exception Ex)
            {
                return Location_ID;
            }
        }

        public static string GetLocationAppoimentIdByClinicNumber(string ClinicNumber)
        {
            try
            {
                if (DtLocationList != null && DtLocationList.Rows.Count > 1)
                {
                    DataRow[] row = DtLocationList.Copy().Select("Clinic_Number = '" + ClinicNumber + "'");
                    if (row.Length > 0)
                        return row[0]["Loc_Id"].ToString();
                    else
                        return Location_ID;
                }
                else
                    return Location_ID;
            }
            catch (Exception Ex)
            {
                return Location_ID;
            }
        }

        public static string GetDataBaseConnectionByServicesInstallId(string ServiceId)
        {
            try
            {
                if (DtInstallServiceList != null && DtInstallServiceList.Rows.Count > 1)
                {
                    DataRow[] row = DtInstallServiceList.Copy().Select("Installation_ID = '" + ServiceId + "'");
                    if (row.Length > 0)
                        return row[0]["DBConnString"].ToString();
                    else
                        return DBConnString;
                }
                else
                    return DBConnString;
            }
            catch (Exception Ex)
            {
                return DBConnString;
            }
        }


        public static void ResolvedOutofMemeoryException()
        {
            try
            {
                if (CommonUtility.GetValueFromAppConfig("UseMaxBufferSize").ToString() == "" || CommonUtility.GetValueFromAppConfig("UseMaxBufferSize").ToString() == "0" || CommonUtility.GetValueFromAppConfig("UseMaxBufferSize").ToString().ToLower().Trim() == "false")
                {
                    RegistryKey keyMxBufferSize = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UseMaxBufferSize");
                    if (keyMxBufferSize == null)
                    {
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseMaxBufferSize");
                        key1.SetValue("UseMaxBufferSize", true);
                    }
                    else
                    {
                        if (Convert.ToBoolean(keyMxBufferSize.GetValue("UseMaxBufferSize").ToString()) == false)
                        {
                            Utility.UseMaxBufferSize = true;
                        }
                        else
                        {
                            Utility.UseMaxBufferSize = Convert.ToBoolean(keyMxBufferSize.GetValue("UseMaxBufferSize").ToString());
                        }
                    }
                }
                else
                {
                    Utility.UseMaxBufferSize = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("UseMaxBufferSize").ToString().Trim());
                    RegistryKey keyMxBufferSize = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UseMaxBufferSize");
                    if (keyMxBufferSize == null)
                    {
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseMaxBufferSize");
                        Utility.UseMaxBufferSize = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("UseMaxBufferSize").ToString());
                        key1.SetValue("UseMaxBufferSize", Utility.UseMaxBufferSize);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public static void RestartApp()
        {
            try
            {
                foreach (Process p in Process.GetProcesses())
                {

                    if (string.Compare("Pozative", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                    {
                        try
                        {
                            p.Kill();
                            continue;
                        }
                        catch (Exception)
                        {
                        }

                    }
                    if (string.Compare("PracticeAnalytics".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        try
                        {
                            p.Kill();
                            continue;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (string.Compare("AditAppSyncLog".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        try
                        {
                            p.Kill();
                            continue;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (string.Compare("DentrixDocument".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch (Exception)
                        {
                        }
                        continue;
                    }
                    if (string.Compare("DTX_Helper".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch (Exception)
                        {
                        }
                        continue;
                    }
                    // For softdent close application
                    if (string.Compare("SoftDentSync", p.ProcessName, true) == 0)
                    {
                        try
                        {
                            p.Kill();
                            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                            key.SetValue("IsSyncing", false);
                            continue;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (string.Compare("EasyDentalSync", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                    {
                        try
                        {

                            p.Kill();
                            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                            {
                                key.SetValue("IsEasyDentalSyncing", false);
                            }
                            using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                            {
                                key1.SetValue("IsEasyDentalPatinetSyncing", false);
                            }
                            continue;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (string.Compare("CrystalPM", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                    {
                        try
                        {

                            p.Kill();
                            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                            {
                                key.SetValue("IsCrystalPMSyncing", false);
                            }
                            using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                            {
                                key1.SetValue("IsCrystalPMPatientSyncing", false);
                            }
                            continue;
                        }
                        catch (Exception)
                        {
                        }
                    }

                }
                System.Environment.Exit(1);
            }
            catch (Exception)
            {
            }
        }

        public static string GetAppSettingsString(string strSection,string strValue,string strDefault)
        {
            object registryObject1 = GetRegistryObject(Registry.CurrentUser, strSection, strValue);
            if (registryObject1 != null)
                return registryObject1.ToString();
            object registryObject2 = GetRegistryObject(Registry.LocalMachine, strSection, strValue);
            return registryObject2 != null ? registryObject2.ToString() : strDefault;
        }
        private static string DENTRIX_SUBKEYPATH = "Software\\Dentrix Dental Systems, Inc.\\Dentrix\\";
        private static object GetRegistryObject(RegistryKey HiveKey,string strSection, string strValue)
        {

            try
            {
                RegistryKey registryKey = HiveKey.OpenSubKey(DENTRIX_SUBKEYPATH + strSection);
                if (registryKey != null)
                {
                    object obj = registryKey.GetValue(strValue);
                    if (obj != null)
                    {
                        registryKey.Close();
                        return obj;
                    }
                    registryKey.Close();
                }
            }
            catch (Exception ex)
            {
                return (object)null;
            }
            return (object)null;
        }

        public static bool SetRegistryObject(RegistryKey hiveKey,string section,string valueKey,object valueItem,RegistryValueKind registryValueKind)
        {
            bool flag = true;

            try
            {
                RegistryKey subKey = hiveKey.CreateSubKey(DENTRIX_SUBKEYPATH + section);
                if (subKey != null)
                {
                    if (registryValueKind != RegistryValueKind.None)
                        subKey.SetValue(section, valueKey, registryValueKind);
                    else
                        subKey.SetValue(valueKey, valueItem);
                }
            }
            catch (Exception ex)
            {

                flag = false;
            }
            return flag;
        }

        //public static bool isDebugLogRequired = false;
        public static void WriteToDebugSyncLogFile_All(string SyncLogString, string strFunction)
        {
            DateTime dtCurrentDtTime = Utility.Datetimesetting();
            string SyncLogFilePath = Application.StartupPath + "\\" + "SyncLogFile\\Debug\\" + strFunction + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
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
            }
            catch (Exception)
            {
                //throw;
            }

            ConnectionLog = dtCurrentDtTime.ToString(Utility.ApplicationDatetimeFormat) + " - " + SyncLogString + "\n" + ConnectionLog;
        }


        #region Backupdata

        public static bool UpdateBackupDBFromLocalDB()
        {
            try
            {
                DataTable dtLocalAditHostServer = GetAditHostServerData(true);
                DataTable dtLocalLocation = GetLocationData(true);
                DataTable dtLocalOrganization = GetOrganizationData(true);
                DataTable dtLocalServiceInstallation = GetServiceInstallationData(true);
                DataTable dtLocalSynchModule = GetSynchModuleData(true);

                DeleteLocationTableData(false);
                DeleteOrganizationTableData(false);
                DeleteServiceInstallationTableData(false);
                DeleteSyncModuleTableData(false);
                Delete_AditHostServerTable(false);

                for (int i = 0; i < dtLocalServiceInstallation.Rows.Count; i++)
                {
                    for (int j = 0; j < dtLocalOrganization.Rows.Count; j++)
                    {
                        for (int k = 0; k < dtLocalLocation.Rows.Count; k++)
                        {
                            CreateRegistryKeyForConfiguration(dtLocalLocation.Rows[k]["Location_ID"].ToString(), dtLocalOrganization.Rows[j]["Organization_ID"].ToString(), dtLocalServiceInstallation.Rows[i]["Installation_ID"].ToString());
                        }
                    }
                }

                Save_AditHostServerData(dtLocalAditHostServer, true);
                Save_LocationData(dtLocalLocation, true);
                Save_OrganizationData(dtLocalOrganization, true);
                Save_ServiceInstallationData(dtLocalServiceInstallation, true);
                Save_SynchModuleData(dtLocalSynchModule, true);

                Utility.WriteToSyncLogFile_All("UpdateBackupDB (" + Utility.Application_Name + " to Local Database) Successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("UpdateBackupDB error : " + ex.Message.ToString());
                return false;
                //throw ex;
            }
        }

        public static bool UpdateLocalDB_From_BackupDB()
        {
            try
            {
                DataTable dtLocalAditHostServer = new DataTable();
                DataTable dtLocalLocation = new DataTable();
                DataTable dtLocalOrganization = new DataTable();
                DataTable dtLocalServiceInstallation = new DataTable();
                DataTable dtLocalSynchModule = new DataTable();

                DeleteLocationTableData(true);
                DeleteOrganizationTableData(true);
                DeleteServiceInstallationTableData(true);
                DeleteSyncModuleTableData(true);
                Delete_AditHostServerTable(true);

                try
                {
                    dtLocalAditHostServer = GetAditHostServerData(false);
                    dtLocalLocation = GetLocationData(false);
                    dtLocalOrganization = GetOrganizationData(false);
                    dtLocalServiceInstallation = GetServiceInstallationData(false);
                    dtLocalSynchModule = GetSynchModuleData(false);
                }
                catch
                {
                    // get id's from registory and get data from api 
                }


                Save_AditHostServerData(dtLocalAditHostServer, false);
                Save_LocationData(dtLocalLocation, false);
                Save_OrganizationData(dtLocalOrganization, false);
                Save_ServiceInstallationData(dtLocalServiceInstallation, false);
                Save_SynchModuleData(dtLocalSynchModule, false);

                Utility.WriteToErrorLogFromAll("UpdateLocalDB_From_BackupDB Successfully.");
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public static void DeleteLocationTableData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From Location";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From Location";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
        }

        public static void DeleteOrganizationTableData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From Organization";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From Organization";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
        }

        public static void DeleteServiceInstallationTableData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From Service_Installation";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From Service_Installation";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
        }

        public static void DeleteSyncModuleTableData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From SyncModule";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From SyncModule";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
        }

        public static void Delete_AditHostServerTable(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From Adit_HostServer";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Delete From Adit_HostServer";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
        }

        public static DataTable GetAditHostServerData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from Adit_HostServer";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                foreach (DataRow rs in SqlCeDt.Rows)
                                {
                                    if (rs["Is_Active"] == DBNull.Value)
                                    {
                                        rs["Is_Active"] = "False";
                                    }
                                }
                                SqlCeDt.AcceptChanges();
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from Adit_HostServer";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
        }

        public static DataTable GetLocationData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from Location";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from Location";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
        }

        public static DataTable GetOrganizationData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from Organization";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from Organization";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }


        }

        public static DataTable GetServiceInstallationData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from Service_Installation";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from Service_Installation";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }

        }

        public static DataTable GetSynchModuleData(bool isfromLocal)
        {
            if (isfromLocal)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from SyncModule";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        //  if (conn.State == ConnectionState.Closed) conn.Open(); 
                        string SqlCeSelect = "Select * from SyncModule";
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            DataTable SqlCeDt = null;
                            using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                            {
                                SqlCeDt = new DataTable();
                                SqlCeDa.Fill(SqlCeDt);
                                return SqlCeDt;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
        }

        public static void Save_AditHostServerData(DataTable dtAditHostServer, bool insertintoBackup)
        {
            if (insertintoBackup)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtAditHostServer.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "INSERT INTO Adit_HostServer (HostName,MultiRecordHostName,Is_Active,ServerType) VALUES (@HostName,@MultiRecordHostName,@Is_Active,@ServerType)";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                //SqlCeCommand.Parameters.AddWithValue("Host_ID", drRow["Host_ID"].ToString());                            
                                SqlCeCommand.Parameters.AddWithValue("HostName", drRow["HostName"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("MultiRecordHostName", drRow["MultiRecordHostName"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Is_Active", drRow["Is_Active"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ServerType", drRow["ServerType"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))  
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtAditHostServer.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "INSERT INTO Adit_HostServer (HostName,MultiRecordHostName,Is_Active,ServerType) VALUES (@HostName,@MultiRecordHostName,@Is_Active,@ServerType)";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                //SqlCeCommand.Parameters.AddWithValue("Host_ID", drRow["Host_ID"].ToString());                            
                                SqlCeCommand.Parameters.AddWithValue("HostName", drRow["HostName"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("MultiRecordHostName", drRow["MultiRecordHostName"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Is_Active", drRow["Is_Active"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ServerType", drRow["ServerType"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

        }
        public static void Save_LocationData(DataTable dtLocation, bool insertintoBackup)
        {
            if (insertintoBackup)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtLocation.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "INSERT INTO Location (Location_ID,name,google_address,phone,email,address,website_url,language,owner,location_numbers,Organization_ID,User_ID,Loc_ID,Clinic_Number,Service_Install_Id,AditSync,ApptAutoBook,AditLocationSyncEnable) "
                                            + " VALUES(@Location_ID, @name, @google_address, @phone, @email, @address, @website_url, @language, @owner, @location_numbers, @Organization_ID, @User_ID, @Loc_ID, @Clinic_Number, @Service_Install_Id, @AditSync, @ApptAutoBook, @AditLocationSyncEnable)";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("Location_ID", drRow["Location_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("name", drRow["name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("google_address", drRow["google_address"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("phone", drRow["phone"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("email", drRow["email"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("address", drRow["address"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("website_url", drRow["website_url"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("language", drRow["language"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("owner", drRow["owner"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("location_numbers", drRow["location_numbers"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Organization_ID", drRow["Organization_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("User_ID", drRow["User_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Loc_ID", drRow["Loc_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", drRow["Clinic_Number"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", drRow["Service_Install_Id"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("AditSync", drRow["AditSync"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ApptAutoBook", drRow["ApptAutoBook"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("AditLocationSyncEnable", drRow["AditLocationSyncEnable"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtLocation.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "INSERT INTO Location (Location_ID,name,google_address,phone,email,address,website_url,language,owner,location_numbers,Organization_ID,User_ID,Loc_ID,Clinic_Number,Service_Install_Id,AditSync,ApptAutoBook,AditLocationSyncEnable) "
                                            + " VALUES(@Location_ID, @name, @google_address, @phone, @email, @address, @website_url, @language, @owner, @location_numbers, @Organization_ID, @User_ID, @Loc_ID, @Clinic_Number, @Service_Install_Id, @AditSync, @ApptAutoBook, @AditLocationSyncEnable)";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("Location_ID", drRow["Location_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("name", drRow["name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("google_address", drRow["google_address"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("phone", drRow["phone"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("email", drRow["email"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("address", drRow["address"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("website_url", drRow["website_url"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("language", drRow["language"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("owner", drRow["owner"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("location_numbers", drRow["location_numbers"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Organization_ID", drRow["Organization_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("User_ID", drRow["User_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Loc_ID", drRow["Loc_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", drRow["Clinic_Number"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", drRow["Service_Install_Id"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("AditSync", drRow["AditSync"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ApptAutoBook", drRow["ApptAutoBook"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("AditLocationSyncEnable", drRow["AditLocationSyncEnable"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

        }
        public static void Save_OrganizationData(DataTable dtOrganization, bool insertintoBackup)
        {
            if (insertintoBackup)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtOrganization.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "INSERT INTO Organization   (Organization_ID  ,Name  ,phone  ,email  ,address  ,currency  ,info  ,is_active  ,owner  ,Adit_User_Email_ID  ,Adit_User_Email_Password)  "
                                                            + " VALUES(@Organization_ID  , @Name  , @phone  , @email  , @address  , @currency  , @info  , @is_active  , @owner  , @Adit_User_Email_ID  , @Adit_User_Email_Password  )";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("Organization_ID", drRow["Organization_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Name", drRow["Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("phone", drRow["phone"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("email", drRow["email"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("address", drRow["address"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("currency", drRow["currency"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("info", drRow["info"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("is_active", drRow["is_active"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("owner", drRow["owner"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Adit_User_Email_ID", drRow["Adit_User_Email_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Adit_User_Email_Password", drRow["Adit_User_Email_Password"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtOrganization.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "INSERT INTO Organization   (Organization_ID  ,Name  ,phone  ,email  ,address  ,currency  ,info  ,is_active  ,owner  ,Adit_User_Email_ID  ,Adit_User_Email_Password)  "
                                                            + " VALUES(@Organization_ID  , @Name  , @phone  , @email  , @address  , @currency  , @info  , @is_active  , @owner  , @Adit_User_Email_ID  , @Adit_User_Email_Password  )";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("Organization_ID", drRow["Organization_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Name", drRow["Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("phone", drRow["phone"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("email", drRow["email"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("address", drRow["address"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("currency", drRow["currency"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("info", drRow["info"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("is_active", drRow["is_active"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("owner", drRow["owner"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Adit_User_Email_ID", drRow["Adit_User_Email_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Adit_User_Email_Password", drRow["Adit_User_Email_Password"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

        }
        public static void Save_ServiceInstallationData(DataTable dtServiceInstallation, bool insertintoBackup)
        {
            if (insertintoBackup)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtServiceInstallation.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "INSERT INTO Service_Installation (Installation_ID, Organization_ID, Location_ID, Application_Name, Application_Version, System_Name, System_processorID, Hostname, [Database], IntegrationKey, UserId, Password, Port, WebAdminUserToken, timezone, IS_Install, Installation_Date, Installation_Modify_Date, AditSync, PozativeSync, ApptAutoBook, PozativeEmail, PozativeLocationID, PozativeLocationName, DBConnString, Document_Path, Windows_Service_Version, ApplicationIdleTimeOff, AppIdleStartTime, AppIdleStopTime, ApplicationInstalledTime, EHR_Sub_Version, EHR_VersionNumber, NotAllowToChangeSystemDateFormat, DontAskPasswordOnSaveSetting, DentrixPDFConstring, DentrixPDFPassword,AditUserEmailID,AditUserEmailPassword) "
                                                 + " VALUES (@Installation_ID, @Organization_ID, @Location_ID, @Application_Name, @Application_Version, @System_Name, @System_processorID, @Hostname, @Database, @IntegrationKey, @UserId, @Password, @Port, @WebAdminUserToken, @timezone, @IS_Install, @Installation_Date, @Installation_Modify_Date, @AditSync, @PozativeSync, @ApptAutoBook, @PozativeEmail, @PozativeLocationID, @PozativeLocationName, @DBConnString, @Document_Path, @Windows_Service_Version, @ApplicationIdleTimeOff, @AppIdleStartTime, @AppIdleStopTime, @ApplicationInstalledTime, @EHR_Sub_Version, @EHR_VersionNumber, @NotAllowToChangeSystemDateFormat, @DontAskPasswordOnSaveSetting, @DentrixPDFConstring, @DentrixPDFPassword,@AditUserEmailID,@AditUserEmailPassword)";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("Installation_ID", drRow["Installation_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Organization_ID", drRow["Organization_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Location_ID", drRow["Location_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Application_Name", drRow["Application_Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Application_Version", drRow["Application_Version"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("System_Name", drRow["System_Name"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("System_processorID", drRow["System_processorID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Hostname", drRow["Hostname"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Database", drRow["Database"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("IntegrationKey", drRow["IntegrationKey"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("UserId", drRow["UserId"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Password", drRow["Password"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Port", drRow["Port"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("WebAdminUserToken", drRow["WebAdminUserToken"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("timezone", drRow["timezone"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("IS_Install", Convert.ToBoolean(drRow["IS_Install"]));
                                SqlCeCommand.Parameters.AddWithValue("Installation_Date", string.IsNullOrEmpty(drRow["Installation_Date"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["Installation_Date"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("Installation_Modify_Date", string.IsNullOrEmpty(drRow["Installation_Modify_Date"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["Installation_Modify_Date"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("AditSync", Convert.ToBoolean(drRow["AditSync"]));
                                SqlCeCommand.Parameters.AddWithValue("PozativeSync", Convert.ToBoolean(drRow["PozativeSync"]));
                                SqlCeCommand.Parameters.AddWithValue("ApptAutoBook", Convert.ToBoolean(drRow["ApptAutoBook"]));
                                SqlCeCommand.Parameters.AddWithValue("PozativeEmail", drRow["PozativeEmail"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PozativeLocationID", drRow["PozativeLocationID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PozativeLocationName", drRow["PozativeLocationName"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("DBConnString", drRow["DBConnString"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Document_Path", drRow["Document_Path"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Windows_Service_Version", drRow["Windows_Service_Version"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ApplicationIdleTimeOff", drRow["ApplicationIdleTimeOff"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("AppIdleStartTime", string.IsNullOrEmpty(drRow["AppIdleStartTime"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["AppIdleStartTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("AppIdleStopTime", string.IsNullOrEmpty(drRow["AppIdleStopTime"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["AppIdleStopTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("ApplicationInstalledTime", string.IsNullOrEmpty(drRow["ApplicationInstalledTime"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["ApplicationInstalledTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("EHR_Sub_Version", drRow["EHR_Sub_Version"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("EHR_VersionNumber", drRow["EHR_VersionNumber"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("NotAllowToChangeSystemDateFormat", drRow["NotAllowToChangeSystemDateFormat"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("DontAskPasswordOnSaveSetting", drRow["DontAskPasswordOnSaveSetting"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("DentrixPDFConstring", drRow["DentrixPDFConstring"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("DentrixPDFPassword", drRow["DentrixPDFPassword"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("AditUserEmailID", drRow["AditUserEmailID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("AditUserEmailPassword", drRow["AditUserEmailPassword"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtServiceInstallation.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "INSERT INTO Service_Installation (Installation_ID, Organization_ID, Location_ID, Application_Name, Application_Version, System_Name, System_processorID, Hostname, [Database], IntegrationKey, UserId, Password, Port, WebAdminUserToken, timezone, IS_Install, Installation_Date, Installation_Modify_Date, AditSync, PozativeSync, ApptAutoBook, PozativeEmail, PozativeLocationID, PozativeLocationName, DBConnString, Document_Path, Windows_Service_Version, ApplicationIdleTimeOff, AppIdleStartTime, AppIdleStopTime, ApplicationInstalledTime, EHR_Sub_Version, EHR_VersionNumber, NotAllowToChangeSystemDateFormat, DontAskPasswordOnSaveSetting, DentrixPDFConstring, DentrixPDFPassword,AditUserEmailID,AditUserEmailPassword) "
                                                 + " VALUES (@Installation_ID, @Organization_ID, @Location_ID, @Application_Name, @Application_Version, @System_Name, @System_processorID, @Hostname, @Database, @IntegrationKey, @UserId, @Password, @Port, @WebAdminUserToken, @timezone, @IS_Install, @Installation_Date, @Installation_Modify_Date, @AditSync, @PozativeSync, @ApptAutoBook, @PozativeEmail, @PozativeLocationID, @PozativeLocationName, @DBConnString, @Document_Path, @Windows_Service_Version, @ApplicationIdleTimeOff, @AppIdleStartTime, @AppIdleStopTime, @ApplicationInstalledTime, @EHR_Sub_Version, @EHR_VersionNumber, @NotAllowToChangeSystemDateFormat, @DontAskPasswordOnSaveSetting, @DentrixPDFConstring, @DentrixPDFPassword,@AditUserEmailID,@AditUserEmailPassword)"; SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("Installation_ID", drRow["Installation_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Organization_ID", drRow["Organization_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Location_ID", drRow["Location_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Application_Name", drRow["Application_Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Application_Version", drRow["Application_Version"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("System_Name", drRow["System_Name"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("System_processorID", drRow["System_processorID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Hostname", drRow["Hostname"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Database", drRow["Database"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("IntegrationKey", drRow["IntegrationKey"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("UserId", drRow["UserId"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Password", drRow["Password"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Port", drRow["Port"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("WebAdminUserToken", drRow["WebAdminUserToken"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("timezone", drRow["timezone"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("IS_Install", Convert.ToBoolean(drRow["IS_Install"]));
                                SqlCeCommand.Parameters.AddWithValue("Installation_Date", string.IsNullOrEmpty(drRow["Installation_Date"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["Installation_Date"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("Installation_Modify_Date", string.IsNullOrEmpty(drRow["Installation_Modify_Date"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["Installation_Modify_Date"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("AditSync", Convert.ToBoolean(drRow["AditSync"]));
                                SqlCeCommand.Parameters.AddWithValue("PozativeSync", Convert.ToBoolean(drRow["PozativeSync"]));
                                SqlCeCommand.Parameters.AddWithValue("ApptAutoBook", Convert.ToBoolean(drRow["ApptAutoBook"]));
                                SqlCeCommand.Parameters.AddWithValue("PozativeEmail", drRow["PozativeEmail"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PozativeLocationID", drRow["PozativeLocationID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("PozativeLocationName", drRow["PozativeLocationName"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("DBConnString", drRow["DBConnString"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Document_Path", drRow["Document_Path"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Windows_Service_Version", drRow["Windows_Service_Version"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("ApplicationIdleTimeOff", drRow["ApplicationIdleTimeOff"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("AppIdleStartTime", string.IsNullOrEmpty(drRow["AppIdleStartTime"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["AppIdleStartTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("AppIdleStopTime", string.IsNullOrEmpty(drRow["AppIdleStopTime"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["AppIdleStopTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("ApplicationInstalledTime", string.IsNullOrEmpty(drRow["ApplicationInstalledTime"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["ApplicationInstalledTime"].ToString()));
                                SqlCeCommand.Parameters.AddWithValue("EHR_Sub_Version", drRow["EHR_Sub_Version"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("EHR_VersionNumber", drRow["EHR_VersionNumber"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("NotAllowToChangeSystemDateFormat", drRow["NotAllowToChangeSystemDateFormat"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("DontAskPasswordOnSaveSetting", drRow["DontAskPasswordOnSaveSetting"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("DentrixPDFConstring", drRow["DentrixPDFConstring"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("DentrixPDFPassword", drRow["DentrixPDFPassword"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("AditUserEmailID", drRow["AditUserEmailID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("AditUserEmailPassword", drRow["AditUserEmailPassword"].ToString());
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }
        public static void Save_SynchModuleData(DataTable dtSynchModuleData, bool insertintoBackup)
        {
            if (insertintoBackup)
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalBackupDBConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtSynchModuleData.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "Insert Into SyncModule  (SyncModule_Name,SyncModule_Pull,SyncModule_Push,SyncModule_EHR,Last_Update_Date,SyncDateTime) "
                                                           + " VALUES (@SyncModule_Name,@SyncModule_Pull,@SyncModule_Push,@SyncModule_EHR,@Last_Update_Date,@SyncDateTime)";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("SyncModule_Name", drRow["SyncModule_Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SyncModule_Pull", drRow["SyncModule_Pull"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SyncModule_Push", drRow["SyncModule_Push"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SyncModule_EHR", drRow["SyncModule_EHR"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Update_Date", drRow["Last_Update_Date"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SyncDateTime", string.IsNullOrEmpty(drRow["SyncDateTime"].ToString()) ? Convert.ToDateTime(Utility.GetCurrentDatetimestring()) : Convert.ToDateTime(drRow["SyncDateTime"].ToString()));
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;

                    try
                    {
                        foreach (DataRow drRow in dtSynchModuleData.Rows)
                        {
                            SqlCeSelect = string.Empty;
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {

                                SqlCeCommand.CommandText = "Insert Into SyncModule  (SyncModule_ID,SyncModule_Name,SyncModule_Pull,SyncModule_Push,SyncModule_EHR,Last_Update_Date,SyncDateTime) "
                                                           + " VALUES (@SyncModule_ID,@SyncModule_Name,@SyncModule_Pull,@SyncModule_Push,@SyncModule_EHR,@Last_Update_Date,@SyncDateTime)";
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.Parameters.Clear();

                                SqlCeCommand.Parameters.AddWithValue("SyncModule_ID", drRow["SyncModule_ID"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SyncModule_Name", drRow["SyncModule_Name"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SyncModule_Pull", drRow["SyncModule_Pull"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SyncModule_Push", drRow["SyncModule_Push"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("SyncModule_EHR", drRow["SyncModule_EHR"].ToString());
                                SqlCeCommand.Parameters.AddWithValue("Last_Update_Date", drRow["Last_Update_Date"].ToString());//Over90
                                SqlCeCommand.Parameters.AddWithValue("SyncDateTime", drRow["SyncDateTime"].ToString());//Over90
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

        }
        

        public static void CreateRegistryKeyForConfiguration(string locationID, string OrganizationID, string dtServiceInstallationID)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                    key.SetValue("locationID~" + dtServiceInstallationID + "~" + locationID, OrganizationID);
                    key.SetValue("ServiceInstallationID~" + dtServiceInstallationID, dtServiceInstallationID);

                }
                else
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("locationID~" + dtServiceInstallationID + "~" + locationID, OrganizationID);
                        key1.SetValue("ServiceInstallationID~" + dtServiceInstallationID, dtServiceInstallationID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
                                
        public static bool Get_LocalDbFromWeb()
        {
            try
            {
                string fileUrl = "https://pozative.com/public/pozativeservice/serverappdb/Pozative.sdf";
                string saveFilePath = Application.StartupPath + "\\Pozative.sdf";

                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    WebClient client = new WebClient();
                    client.DownloadFile(fileUrl, saveFilePath);
                    return true;
                }
                catch (HttpRequestException ex)
                {
                    Utility.WriteToErrorLogFromAll($"Get_LocalDbFromWeb : HTTP Request Error: {ex.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void CheckErrorLogFile()
        {
            try
            {
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string ErrorLogFilePath = Application.StartupPath + "\\" + "ErrorLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
                string ErrorLogFileName = ErrorLogFilePath + "\\" + dtCurrentDtTime.ToString("yyyy_MM_dd") + ".txt";
                string AlterDBFilePath = Application.StartupPath + "\\AlterDBFile.txt";
                List<string> LineList = new List<string>();

                if (File.Exists(ErrorLogFileName))
                {
                    using (StreamReader reader = new StreamReader(ErrorLogFileName))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            LineList.Add(line);
                        }
                    }

                    if (LineList.Count > 0)
                    {
                        for (int i = 0; i < LineList.Count; i++)
                        {
                            if (LineList[i].ToLower().Contains("the specified table does not exist.")
                            || LineList[i].ToLower().Contains("string truncation")
                            || LineList[i].ToLower().Contains("cannot find column")
                            || LineList[i].ToLower().Contains("missing index")
                            || LineList[i].ToLower().Contains("a column id occurred more than once in the specification.")
                            || LineList[i].ToLower().Contains("column name is not valid"))
                            {
                                if (File.Exists(AlterDBFilePath))
                                {
                                    ExecuteQueriesFromFile(CommonUtility.LocalConnectionString(), AlterDBFilePath);
                                    return;

                                }
                                else
                                {
                                    Get_AlterDBFileFromWeb();
                                    ExecuteQueriesFromFile(CommonUtility.LocalConnectionString(), AlterDBFilePath);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    WriteToErrorLogFromAll("Error file does not exist.");
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLogFromAll(ex.Message);
            }
            return;
        }

        public static void ExecuteQueriesFromFile(string connectionString, string filePath)
        {
            using (var connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                string[] queries = File.ReadAllLines(filePath);

                foreach (string query in queries)
                {
                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        using (var command = new SqlCeCommand(query, connection))
                        {
                            try
                            {
                                command.ExecuteNonQuery();
                                Utility.WriteToSyncLogFile_All("Query From AlterDBFileFromWeb executed: " + query);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }
                }
            }
        }

        public static bool Get_AlterDBFileFromWeb()
        {
            try
            {
                string fileUrl = "https://pozative.com/public/pozativeservice/AlterDB.txt";
                string saveFilePath = Utility.AditDocTempPath + "\\AlterDBFile.txt";

                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        WebClient client = new WebClient();
                        client.DownloadFile(fileUrl, saveFilePath);
                        return true;
                    }
                    catch (HttpRequestException ex)
                    {
                        Utility.WriteToErrorLogFromAll($"Get_AlterDBFileFromWeb : HTTP Request Error: {ex.Message}");
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        #region EventListener
        public static void WritetoAditEventErrorLogFile_Static(string ErrorLogString)
        {
            try
            {
                bool blnRestartApp = false;
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string ErrorLogFilePath = Application.StartupPath + "\\" + "AditEventListener\\ErrorLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
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
            }
            catch (Exception)
            {
            }
        }

        public static void WritetoAditEventDebugLogFile_Static(string ErrorLogString)
        {
            bool isTrueDebugLog = true;
            try
            {
                if (!isTrueDebugLog)
                {
                    return;
                }
                bool blnRestartApp = false;
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string ErrorLogFilePath = Application.StartupPath + "\\" + "AditEventListener\\DebugLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
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
            }
            catch (Exception)
            {
            }
        }

        public static void WritetoAditEventSyncLogFile_Static(string ErrorLogString)
        {
            try
            {
                bool blnRestartApp = false;
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string ErrorLogFilePath = Application.StartupPath + "\\" + "AditEventListener\\SyncLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
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
            }
            catch (Exception)
            {
            }
        }
        #endregion

        public static void CheckEntryUserLoginIdExist()
        {

            try
            {
                Utility.WriteToErrorLogFromAll("Get User function called.");
                Utility.dtLocationWiseUser.Rows.Clear();
                if (!Utility.dtLocationWiseUser.Columns.Contains("ClinicNumber"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("ClinicNumber");
                }
                if (!Utility.dtLocationWiseUser.Columns.Contains("LocationId"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("LocationId");
                }
                if (!Utility.dtLocationWiseUser.Columns.Contains("EHR_UserLogin_ID"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("EHR_UserLogin_ID");
                }
                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                {
                    DataRow drNew = Utility.dtLocationWiseUser.NewRow();
                    drNew["ClinicNumber"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                    drNew["LocationId"] = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString();
                    string strGetEntryUserId = Utility.MultiRecordHostName + "v1/pull/webhooks/get/ehruser?location=" + Utility.DtLocationList.Rows[j]["Loc_Id"].ToString();
                    var clientuser = new RestClient(strGetEntryUserId);
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                    IRestResponse response = clientuser.Execute(request);

                    if (response.ErrorMessage != null)
                    {
                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                        { }
                        else
                        {
                            WriteToErrorLogFromAll("[Users Sync (Adit Server)] : " + response.ErrorMessage);
                        }
                    }
                    //GoalBase.WriteToErrorLogFile_Static("[Users Sync Response] : " + response.Content);
                    var UserVar = JsonConvert.DeserializeObject<Pull_UsersBO>(response.Content);

                    if (UserVar.data != null)
                    {
                        if (UserVar.data.Count > 0)
                        {
                            for (int i = 0; i < UserVar.data.Count; i++)
                            {
                                if (UserVar.data[i]._id.Equals(Utility.DtLocationList.Rows[j]["Loc_Id"].ToString()))
                                {
                                    Utility.EHR_UserLogin_ID = UserVar.data[i].default_ehruser;
                                    drNew["EHR_UserLogin_ID"] = UserVar.data[i].default_ehruser;
                                    // Utility.DtLocationList.Rows[j]["EHR_User_Id"] = UserVar.data._id;
                                }
                            }
                        }

                    }
                    Utility.dtLocationWiseUser.Rows.Add(drNew);

                    
                }                
            }
            catch (Exception ex)
            {
                WriteToErrorLogFromAll("GetEntryUserLoginId : " + ex.Message);
            }
        }

        public static bool DownloadFileWithProgress(string URL, string Location)
        {
            System.IO.FileStream FS;
            Stopwatch sw = new Stopwatch();
            FS = new System.IO.FileStream(Location, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // int iTotalBytesRead = 0;
            try
            {
                System.Net.WebRequest wRemote;
                byte[] bBuffer;
                bBuffer = new byte[257];
                int iBytesRead;

                wRemote = System.Net.WebRequest.Create(URL);
                sw.Start();
                DateTime t1 = DateTime.Now;
                System.Net.WebResponse myWebResponse = wRemote.GetResponse();
                System.IO.Stream sChunks = myWebResponse.GetResponseStream();
                do
                {
                    iBytesRead = sChunks.Read(bBuffer, 0, 256);
                    FS.Write(bBuffer, 0, iBytesRead);
                    //  iTotalBytesRead += iBytesRead;
                }
                while (iBytesRead != 0);
                sChunks.Close();
                FS.Close();
                sw.Stop();
                return true;
            }
            catch (Exception)
            {
                sw.Stop();
                if (!(FS == null))
                {
                    FS.Close();
                    FS = null;
                }
                return false;
            }
        }

        public static void CheckEntryUserLoginIdExist_Opendental(string LocationId="")
        {

            try
            {
                Utility.dtLocationWiseUser.Rows.Clear();
                if (!Utility.dtLocationWiseUser.Columns.Contains("ClinicNumber"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("ClinicNumber");
                }
                if (!Utility.dtLocationWiseUser.Columns.Contains("LocationId"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("LocationId");
                }
                if (!Utility.dtLocationWiseUser.Columns.Contains("EHR_UserLogin_ID"))
                {
                    Utility.dtLocationWiseUser.Columns.Add("EHR_UserLogin_ID");
                }
                if (LocationId != "")
                {
                    string strGetEntryUserId = Utility.MultiRecordHostName + "v1/pull/webhooks/get/ehruser?location=" + LocationId.ToString();
                    var clientuser = new RestClient(strGetEntryUserId);
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(LocationId.ToString()));
                    IRestResponse response = clientuser.Execute(request);

                    if (response.ErrorMessage != null)
                    {
                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                        { }
                        else
                        {
                            WriteToErrorLogFromAll("[Users Sync (Adit Server)] : " + response.ErrorMessage);
                        }
                    }
                    //GoalBase.WriteToErrorLogFile_Static("[Users Sync Response] : " + response.Content);
                    var UserVar = JsonConvert.DeserializeObject<Pull_UsersBO>(response.Content);

                    if (UserVar.data != null)
                    {
                        if (UserVar.data.Count > 0)
                        {
                            for (int i = 0; i < UserVar.data.Count; i++)
                            {
                                if (UserVar.data[i]._id.Equals(LocationId.ToString()))
                                {
                                    Utility.EHR_UserLogin_ID = UserVar.data[i].default_ehruser;

                                    // Utility.DtLocationList.Rows[j]["EHR_User_Id"] = UserVar.data._id;
                                }
                            }
                        }
                    }
                    //Utility.dtLocationWiseUser.Rows.Add(drNew);


                    //}
                    //}
                    //}
                }
                //for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                //{
                //    DataRow drNew = Utility.dtLocationWiseUser.NewRow();
                //    drNew["ClinicNumber"] = Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString();
                //    drNew["LocationId"] = Utility.DtLocationList.Rows[j]["Loc_Id"].ToString();
                //    string strGetEntryUserId = Utility.MultiRecordHostName + "v1/pull/webhooks/get/ehruser?location=" + Utility.DtLocationList.Rows[j]["Loc_Id"].ToString();
                //    var clientuser = new RestClient(strGetEntryUserId);
                //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                //    ServicePointManager.Expect100Continue = true;
                //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //    var request = new RestRequest(Method.GET);
                //    request.AddHeader("cache-control", "no-cache");
                //    request.AddHeader("content-type", "application/json");
                //    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                //    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                //    IRestResponse response = clientuser.Execute(request);

                //    if (response.ErrorMessage != null)
                //    {
                //        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                //        { }
                //        else
                //        {
                //            WriteToErrorLogFromAll("[Users Sync (Adit Server)] : " + response.ErrorMessage);
                //        }
                //    }
                //    //GoalBase.WriteToErrorLogFile_Static("[Users Sync Response] : " + response.Content);
                //    var UserVar = JsonConvert.DeserializeObject<Pull_UsersBO>(response.Content);

                //    if (UserVar.data != null)
                //    {
                //        if (UserVar.data.Count > 0)
                //        {
                //            for (int i = 0; i < UserVar.data.Count; i++)
                //            {
                //                if (UserVar.data[i]._id.Equals(Utility.DtLocationList.Rows[j]["Loc_Id"].ToString()))
                //                {
                //                    Utility.EHR_UserLogin_ID = UserVar.data[i].default_ehruser;
                //                    drNew["EHR_UserLogin_ID"] = UserVar.data[i].default_ehruser;
                //                    // Utility.DtLocationList.Rows[j]["EHR_User_Id"] = UserVar.data._id;
                //                }
                //            }
                //        }

                //    }
                //    Utility.dtLocationWiseUser.Rows.Add(drNew);


                //}
            }
            catch (Exception ex)
            {
                WriteToErrorLogFromAll("GetEntryUserLoginId : " + ex.Message);
            }
        }

        public static bool checkbackupDB()
        {
            DataTable dtLocalAditHostServer = new DataTable();
            DataTable dtLocalLocation = new DataTable();
            DataTable dtLocalOrganization = new DataTable();
            DataTable dtLocalServiceInstallation = new DataTable();

            try
            {
                dtLocalAditHostServer = GetAditHostServerData(false);
                dtLocalLocation = GetLocationData(false);
                dtLocalOrganization = GetOrganizationData(false);
                dtLocalServiceInstallation = GetServiceInstallationData(false);

                if (dtLocalAditHostServer.Rows.Count <= 0 || dtLocalLocation.Rows.Count <= 0 || dtLocalOrganization.Rows.Count <= 0 || dtLocalServiceInstallation.Rows.Count <= 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                WriteToErrorLogFromAll("checkbackupDB error " + ex.Message.ToString());
                return true;
              
            }
        }
    }

}
