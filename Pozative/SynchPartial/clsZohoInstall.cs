using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pozative.UTL;
using System.Security;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Pozative.BAL;
using Pozative.BO;
using RestSharp;
using System.Net.Security;
using Newtonsoft.Json;
using System.DirectoryServices;
using System.ComponentModel;
using System.Security.Cryptography;
using Pozative.BAL;
using System.IO;
using Microsoft.Win32.TaskScheduler;


namespace Pozative
{
    public partial class frmPozative
    {
        private BackgroundWorker bwSynchEHRAndSystemCreds = null;
        private System.Timers.Timer timerSynchEHRAndSystemCreds = null;

        private BackgroundWorker bwSynchZohoInstall = null;
        private System.Timers.Timer timerSynchZohoInstall = null;

        private BackgroundWorker bwSynchSaveServerUser = null;
        private System.Timers.Timer timerSynchSaveServerUser = null;

        private void CallSynchZohoInstall()
        {
            SynchData_SystemUsers();
            fncSynchDataSaveServerUser();

            //Sync_ZohoInstall();
            fncSynchDataZohoInstall();

            //Sync_EHRInfo();
            fncSynchDataEHRAndSystemCreds();

            CreatePozativeUATaskScheduler();
        }

        #region EHRAndSystemCreds

        #region TimerFunctions
        private void fncSynchDataEHRAndSystemCreds()
        {
            InitBgWorkerEHRAndSystemCreds();
            InitBgTimerEHRAndSystemCreds();
        }

        private void InitBgTimerEHRAndSystemCreds()
        {
            timerSynchEHRAndSystemCreds = new System.Timers.Timer();
            this.timerSynchEHRAndSystemCreds.Interval = (5 * 60000); //1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchEHRAndSystemCreds.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchEHRAndSystemCreds_Tick);
            timerSynchEHRAndSystemCreds.Enabled = true;
            timerSynchEHRAndSystemCreds.Start();
            timerSynchEHRAndSystemCreds_Tick(null, null);
        }

        private void InitBgWorkerEHRAndSystemCreds()
        {
            bwSynchEHRAndSystemCreds = new BackgroundWorker();
            bwSynchEHRAndSystemCreds.WorkerReportsProgress = true;
            bwSynchEHRAndSystemCreds.WorkerSupportsCancellation = true;
            bwSynchEHRAndSystemCreds.DoWork += new DoWorkEventHandler(bwSynchEHRAndSystemCreds_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchEHRAndSystemCreds.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchEHRAndSystemCreds_RunWorkerCompleted);
        }

        private void timerSynchEHRAndSystemCreds_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchEHRAndSystemCreds.Enabled = false;
                MethodForCallSynchOrderEHRAndSystemCreds();
            }
        }

        public void MethodForCallSynchOrderEHRAndSystemCreds()
        {
            System.Threading.Thread procThreadmainEHRAndSystemCreds = new System.Threading.Thread(this.CallSyncOrderTableEHRAndSystemCreds);
            procThreadmainEHRAndSystemCreds.Start();
        }

        public void CallSyncOrderTableEHRAndSystemCreds()
        {
            if (bwSynchEHRAndSystemCreds.IsBusy != true)
            {
                bwSynchEHRAndSystemCreds.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchEHRAndSystemCreds_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchEHRAndSystemCreds.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLocalToDentrix_Patient_Payment();
                Sync_EHRInfo();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchEHRAndSystemCreds_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.timerSynchEHRAndSystemCreds.Interval = (1440 * 60000);
            timerSynchEHRAndSystemCreds.Enabled = true;
        }
        #endregion

        #region PullPush
        public static void SynchDataLiveDB_Pull_EHRAndSystemCreds()
        {
            if (Utility.IsApplicationIdleTimeOff)
            {
                string strEHRInfo = PullLiveDatabaseBAL.GetLiveRecord("ehrinfo", Utility.Loc_ID);
                var client = new RestClient(strEHRInfo);
                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Call EHRAppointment Confirm API");
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestRequest(Method.GET);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Loc_ID));
                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Request Sent into the API  " + " Authorization, TokenKey & action");
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                    {
                        Utility.WriteToErrorLogFromAll("[EHR_EHRAndSystemCreds Adit App to EHR Err  : " + response.ErrorMessage);
                    }
                    else
                    {
                        Utility.WriteToErrorLogFromAll("[EHR_EHRAndSystemCreds Adit App to EHR Err  : " + response.ErrorMessage);
                    }
                    return;
                }
                if (response.Content != null)
                {
                    Utility.WriteSyncPullLog("EHR_Info", Utility._EHRLogdirectory_EHR_appointment, "Response received from API(" + response.Content.ToString() + ")");
                }
                else
                {
                    Utility.WriteSyncPullLog("EHR_Info", Utility._EHRLogdirectory_EHR_appointment, "Response is null");
                }

                Utility.WriteSyncPullLog("EHR_Info", Utility._EHRLogdirectory_EHR_appointment, "----------------------------Deserialize repsonse--------------------------------");
                var EHR_InfoDto = JsonConvert.DeserializeObject<EHRInfoBO>(response.Content);

                if (EHR_InfoDto != null && EHR_InfoDto.data != null && EHR_InfoDto.data.Count > 0)
                {
                    Utility.EHRUserName = EHR_InfoDto.data[0].ehr_user;
                    Utility.EHRUserPassword = EHR_InfoDto.data[0].ehr_pass;
                    Utility.WindowsUserName = EHR_InfoDto.data[0].server_user;
                    Utility.WindowsUserPassword = EHR_InfoDto.data[0].server_pass;
                    Utility.WindowsUserResult = false;
                    Utility.EHRUserResult = "false";
                }
            }
        }



        public static void SynchDataLiveDB_Push_EHRAndSystemCreds_Results()
        {
            try
            {
                bool IsFlag = true;
                ehrinfoupdate ehrInfo = new ehrinfoupdate
                {
                    locationId = Utility.Loc_ID,
                    is_valid = Utility.WindowsUserResult

                };
                StringBuilder JsonEhrInfo = new StringBuilder();
                string strEhrInfo = "";
                var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonEhrInfo.Append(javaScriptApptStatusSerializer.Serialize(ehrInfo) + ",");

                if (JsonEhrInfo.Length > 0)
                {
                    string jsonString = JsonEhrInfo.ToString().Remove(JsonEhrInfo.Length - 1);
                    strEhrInfo = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_EHRAndSystemCreds_Results(jsonString, "ehrinfovalidate", Utility.Loc_ID);

                    if (strEhrInfo.ToLower() != "Success".ToLower())
                    {
                        GoalBase.WriteToErrorLogFile_Static("[EHRAndSystemCreds_Results Sync (Local Database To Adit Server) ] Location_Id: " + Utility.Loc_ID);
                    }
                }
                else
                {
                    strEhrInfo = "Success";
                }

                if (strEhrInfo.ToLower() == "Success".ToLower())
                {
                    IsFlag = true;
                }
                else
                {
                    if (strEhrInfo.Contains("The remote name could not be resolved:"))
                    {
                        IsFlag = false;
                    }
                    else
                    {
                        GoalBase.WriteToErrorLogFile_Static("[EHRAndSystemCreds_Results Sync (Local Database To Adit Server) ] : " + strEhrInfo);
                        IsFlag = false;
                    }
                }

                if (IsFlag)
                {
                    GoalBase.WriteToSyncLogFile_Static("EHRAndSystemCreds_Results Sync (Local Database To Adit Server) Successfully.");
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EHRAndSystemCreds_Results Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }
        #endregion

        #region CheckCredentials
        public static void Sync_EHRInfo()
        {
            string strErrMsg = "";
            bool WindowsUserResult = false;
            try
            {
                //SynchDataLiveDB_Pull_EHRAndSystemCreds();
                if (!string.IsNullOrEmpty(Utility.WindowsUserName) && !string.IsNullOrEmpty(Utility.WindowsUserPassword))
                {
                    WindowsUserResult = IsValidUser(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), ref strErrMsg);
                    if (Utility.WindowsUserResult == false && WindowsUserResult == true)
                    {
                        UpdateTaskScheduler(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), false);
                        UpdateWindowsService(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), false);
                    }
                    SynchDataLiveDB_Push_EHRAndSystemCreds_Results();
                }

            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("[Sync_EHRInfo: Error: " + ex.Message + "]");
            }
        }

        #region IsValidUser
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        [DllImport("kernel32.dll")]
        public static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, ref IntPtr Arguments);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);
        private static bool IsValidUser(string strSystemUser, string strSystemPassword, ref string strErrMsg)
        {
            IntPtr tokenHandle = new IntPtr(0);
            bool valid = false;
            try
            {
                string UserName = null;
                string MachineName = null;
                string Pwd = null;

                //The MachineName property gets the name of your computer.
                MachineName = System.Environment.MachineName;
                UserName = strSystemUser;
                Pwd = strSystemPassword;

                const int LOGON32_PROVIDER_DEFAULT = 0;
                const int LOGON32_LOGON_INTERACTIVE = 2;
                tokenHandle = IntPtr.Zero;

                //Call the LogonUser function to obtain a handle to an access token.
                valid = LogonUser(UserName, MachineName, Pwd, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out tokenHandle);

                if (!valid)
                {
                    valid = LogonUser(UserName, null, Pwd, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out tokenHandle);
                }

                if (valid == false)
                {
                    //This function returns the error code that the last unmanaged function returned.
                    int ret = Marshal.GetLastWin32Error();
                    string errmsg = GetErrorMessage(ret);
                    strErrMsg = errmsg;
                    Utility.WriteToErrorLogFromAll("System Credentials issue - " + errmsg);
                }
                else
                {
                    //Create the WindowsIdentity object for the Windows user account that is
                    //represented by the tokenHandle token.                    
                    WindowsIdentity newId = new WindowsIdentity(tokenHandle);
                    WindowsPrincipal userperm = new WindowsPrincipal(newId);

                    //Verify whether the Windows user has administrative credentials.
                    if (userperm.IsInRole(WindowsBuiltInRole.Administrator))
                    {
                        // Do Nothing
                    }
                    else
                    {
                        string usernameToCheck = UserName; // Replace with the username you want to check

                        bool isMemberOfAdministrators = IsUserMemberOfAdministratorsGroup(usernameToCheck);

                        if (isMemberOfAdministrators)
                        {
                            //Console.WriteLine($"{usernameToCheck} is a member of the Administrators group.");
                        }
                        else
                        {
                            //ShowWriteError("The credentials you entered does not have Admin permissions. Please try again.", "", show: true, write: true, forEmail: true, emailType: "SystemLoginError");
                            strErrMsg = "The user credentials you have entered does not have the proper admin configurations in order to proceed with the set up. Please try again with a different user with admin rights.";
                            Utility.WriteToErrorLogFromAll("The user credentials you have entered does not have the proper admin configurations in order to proceed with the set up. Please try again with a different user with admin rights.");
                            valid = false;
                        }
                    }
                }
                CloseHandle(tokenHandle);
            }
            catch (Exception ex)
            {
                strErrMsg = "IsValidUser - " + ex.Message.ToString();
                Utility.WriteToErrorLogFromAll("IsValidUser - " + ex.Message.ToString());
            }
            return valid;
        }
        public static bool IsUserMemberOfAdministratorsGroup(string username)
        {
            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username);

                    if (user != null)
                    {
                        GroupPrincipal administratorsGroup = GroupPrincipal.FindByIdentity(context, "Administrators");

                        if (administratorsGroup != null)
                        {
                            return user.IsMemberOf(administratorsGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("IsUserMemberOfAdministratorsGroup : " + ex.Message);
            }

            // User or group not found or error occurred
            return false;
        }
        public static string GetErrorMessage(int errorCode)
        {
            string lpMsgBuf = null;
            try
            {
                int FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
                int FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
                int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;

                int msgSize = 255;
                int dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

                IntPtr lpSource = IntPtr.Zero;
                IntPtr lpArguments = IntPtr.Zero;
                int returnVal = FormatMessage(dwFlags, ref lpSource, errorCode, 0, ref lpMsgBuf, msgSize, ref lpArguments);

                if (returnVal == 0)
                {
                    Utility.WriteToErrorLogFromAll("Failed to format message for error code " + errorCode.ToString() + ". ");
                    throw new Exception("Failed to format message for error code " + errorCode.ToString() + ". ");
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("GetErrorMessage : " + ex.Message);
            }
            return lpMsgBuf;

        }
        #endregion

        #region IsValidUserNew
        public static SecureString ToSecureString(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            SecureString securePassword = new SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            securePassword.MakeReadOnly();
            return securePassword;
        }

        public static bool ValidateUsernameAndPassword(string userName, SecureString securePassword)
        {
            bool result = false;
            ContextType contextType = ContextType.Machine;

            // Check if the machine is joined to a domain
            if (InDomain())
            {
                contextType = ContextType.Domain;
            }

            try
            {
                using (PrincipalContext principalContext = new PrincipalContext(contextType))
                {
                    // Use NetworkCredential with secure password
                    result = principalContext.ValidateCredentials(userName, new NetworkCredential(string.Empty, securePassword).Password);
                }
            }
            catch (PrincipalOperationException)
            {
                // Account might be disabled, consider login failed
                result = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        private static bool InDomain()
        {
            bool result = true;
            try
            {
                Domain.GetComputerDomain();
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #endregion

        #region UpdateCredsInTSandWS
        private static void UpdateTaskScheduler(string strUserName, string strPassword, bool onlyCreateFile = true)
        {
            try
            {
                //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\PozativeTask\Id
                //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tasks\Id\Author
                //var r = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tree\PozativeTask", "Id", null);
                //var Id = r.ToString();
                //var UserName = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\TaskCache\Tasks\" + Id, "Author", null);                
                //string strExistingUserName = "";
                //if (UserName.Split('\\').Length > 1)
                //{
                //    strExistingUserName = Convert.ToString(UserName.Split('\\')[1]);
                //}
                //else
                //{
                //    strExistingUserName = UserName;
                //}

                //if (strExistingUserName.ToLower().Trim() == strUserName)
                //{
                //    return;
                //}

                string FileName = "";
                FileName = Application.StartupPath + "\\UpdateTaskScheduler.bat";
                if (System.IO.File.Exists(FileName))
                {
                    System.IO.File.Delete(FileName);
                }
                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Dispose();
                }
                using (StreamWriter SW = new StreamWriter(FileName))
                {
                    SW.WriteLine(@"set taskname=PozativeTask");
                    SW.WriteLine(@"set username=" + strUserName);
                    SW.WriteLine(@"set password=" + strPassword);
                    SW.WriteLine(@"schtasks /change /tn ""%taskname%"" /ru ""%username%"" /rp ""%password%""");
                    SW.WriteLine(@"exit 0");
                    SW.Close();
                }
                if (!onlyCreateFile)
                    ExecuteBatchFile(FileName);
            }
            catch (Exception ex)
            {
                
            }
        }

        private static void UpdateWindowsService(string strUserName, string strPassword, bool onlyCreateFile = true)
        {
            try
            {

                //HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\[SERVICE_NAME]\ObjectName
                //var r = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AditEventListener", "ObjectName", null);
                //var UserName = r.ToString();
                //string strExistingUserName = "";
                //if (UserName.Split('\\').Length > 1)
                //{
                //    strExistingUserName = Convert.ToString(UserName.Split('\\')[1]);
                //}
                //else
                //{
                //    strExistingUserName = UserName;
                //}

                //if (strExistingUserName.ToLower().Trim() == strUserName)
                //{
                //    return;
                //}

                using (System.ServiceProcess.ServiceController AdSc = new System.ServiceProcess.ServiceController(Utility.sEventServiceName))
                {
                    if (AdSc.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                    {
                        AdSc.Stop();
                    }
                    AdSc.Dispose();
                }

                string FileName = "";
                FileName = Application.StartupPath + "\\UpdateWindowsService.bat";
                if (System.IO.File.Exists(FileName))
                {
                    System.IO.File.Delete(FileName);
                }
                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Dispose();
                }
                using (StreamWriter SW = new StreamWriter(FileName))
                {
                    SW.WriteLine(@"set servicename=" + Utility.sEventServiceName);
                    SW.WriteLine(@"set username=.\" + strUserName);
                    SW.WriteLine(@"set passwor=" + strPassword);
                    SW.WriteLine(@"sc config ""%servicename%"" obj= ""%username%"" password= ""%password%""");
                    SW.WriteLine(@"exit 0");
                    SW.Close();
                }
                if (!onlyCreateFile)
                    ExecuteBatchFile(FileName);

              
            }
            catch (Exception ex)
            {

            }
            finally
            {
                try
                {
                    using (System.ServiceProcess.ServiceController AdSc = new System.ServiceProcess.ServiceController(Utility.sEventServiceName))
                    {
                        if (AdSc.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            AdSc.Start();
                        }
                        AdSc.Dispose();
                    }
                }
                catch
                { 
                }
            }
        }

        public static void ExecuteBatchFile(string FileName)
        {
            //var psi = new ProcessStartInfo();
            //psi.CreateNoWindow = false;
            //psi.FileName = FileName;
            //psi.Verb = "runas";
            //try
            //{
            //    var process = new Process();
            //    //psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //    psi.Verb = "runas";
            //    process.StartInfo = psi;
            //    process.Start();
            //    process.WaitForExit();
            //}
            //catch (Exception ex)
            //{

            //}

            ProcessStartInfo processInfo = new ProcessStartInfo
            {


                FileName = FileName,   // Specify the executable (CMD in this case)
                Verb = "runas",         // Specify that the process should be run with administrative privileges
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,//, // Required to redirect standard input/output
                CreateNoWindow = true    // Do not create a window for the CMD process
            };

            try
            {
                string result = "";
                //Process.Start(processInfo);
                Process.Start(processInfo);
                //Console.WriteLine("The File Path to Run As Admin:" + filePath);
                //Console.WriteLine("result:" + result);
                //Console.ReadLine();
                //Thread.Sleep(5000);
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                //CloseDialog();
                //Console.WriteLine("Zoho Assist installed successfully");
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message.ToString());
                //Console.WriteLine("Zoho Assist installation failed");
            }
        }
        #endregion
        #endregion

        #region ZohoAccessAddition

        #region TimerFunctions
        private void fncSynchDataZohoInstall()
        {
            InitBgWorkerZohoInstall();
            InitBgTimerZohoInstall();
        }

        private void InitBgTimerZohoInstall()
        {
            timerSynchZohoInstall = new System.Timers.Timer();
            this.timerSynchZohoInstall.Interval = (5 * 60000); //1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchZohoInstall.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchZohoInstall_Tick);
            timerSynchZohoInstall.Enabled = true;
            timerSynchZohoInstall.Start();
            timerSynchZohoInstall_Tick(null, null);
        }

        private void InitBgWorkerZohoInstall()
        {
            bwSynchZohoInstall = new BackgroundWorker();
            bwSynchZohoInstall.WorkerReportsProgress = true;
            bwSynchZohoInstall.WorkerSupportsCancellation = true;
            bwSynchZohoInstall.DoWork += new DoWorkEventHandler(bwSynchZohoInstall_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchZohoInstall.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchZohoInstall_RunWorkerCompleted);
        }

        private void timerSynchZohoInstall_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchZohoInstall.Enabled = false;
                MethodForCallSynchOrderZohoInstall();
            }
        }

        public void MethodForCallSynchOrderZohoInstall()
        {
            System.Threading.Thread procThreadmainZohoInstall = new System.Threading.Thread(this.CallSyncOrderTableZohoInstall);
            procThreadmainZohoInstall.Start();
        }

        public void CallSyncOrderTableZohoInstall()
        {
            if (bwSynchZohoInstall.IsBusy != true)
            {
                bwSynchZohoInstall.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchZohoInstall_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchZohoInstall.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLocalToDentrix_Patient_Payment();
                Sync_ZohoInstall();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchZohoInstall_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchZohoInstall.Enabled = true;
        }
        #endregion

        public static void Sync_ZohoInstall()
        {
            SynchDataLiveDB_Pull_ZohoInstall();
            SynchDataLiveDB_Push_ZohoInstall_Results();
        }

        public static void SynchDataLiveDB_Pull_ZohoInstall()
        {
            bool Is_Confirmed = false;
            object Is_Valid = null;
            bool Is_Installed = false;
            string UserID = "";
            string UserName = "";
            string strOrgName = "";
            string strLocationName = "";
            string JsonString = "";
            string strErrMsg = "";
            string WindowsUser = "";
            string WindowsPass = "";
            bool WindowsUserResult = false;
            string EHRUser = "";
            string EHRPass = "";
            bool EHRUserResult = false;
            string LocationId = "";
            string LocationName = "";
            string OrganisationID = "";
            string OrganisationName = "";
            string Clinic_Number = "";
            string Service_Install_Id = "";
            int ZohoLocalDBID = 0;
            try
            {
                if (Utility.IsApplicationIdleTimeOff)
                {
                    string strEHRInfo = PullLiveDatabaseBAL.GetLiveRecord("zohoinstall", Utility.Loc_ID);
                    var client = new RestClient(strEHRInfo);
                    Utility.WriteSyncPullLog("ZohoInstall", Utility._EHRLogdirectory_EHR_appointment, "Call EHRAppointment Confirm API");
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    var request = new RestRequest(Method.GET);

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    request.AddHeader("apptype", "no-cache");
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Loc_ID));

                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Request Sent into the API  " + " Authorization, TokenKey & action");
                    IRestResponse response = client.Execute(request);

                    if (response.ErrorMessage != null)
                    {
                        if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                        {
                            Utility.WriteToErrorLogFromAll("[EHR_EHRAndSystemCreds Adit App to EHR Err  : " + response.ErrorMessage);
                        }
                        else
                        {
                            Utility.WriteToErrorLogFromAll("[EHR_EHRAndSystemCreds Adit App to EHR Err  : " + response.ErrorMessage);
                        }
                        return;
                    }
                    if (response.Content != null)
                    {
                        Utility.WriteSyncPullLog("ZohoInstall", Utility._EHRLogdirectory_EHR_appointment, "Response received from API(" + response.Content.ToString() + ")");
                    }
                    else
                    {
                        Utility.WriteSyncPullLog("ZohoInstall", Utility._EHRLogdirectory_EHR_appointment, "Response is null");
                    }

                    Utility.WriteSyncPullLog("ZohoInstall", Utility._EHRLogdirectory_EHR_appointment, "----------------------------Deserialize repsonse--------------------------------");
                    var EHR_InfoDto = JsonConvert.DeserializeObject<ZohoInstallInfoBO>(response.Content.ToString());
                    if (EHR_InfoDto != null && EHR_InfoDto.data != null && EHR_InfoDto.data.Count > 0)
                    {
                        EHRUser = EHR_InfoDto.data[0].ehr_user;
                        EHRPass = EHR_InfoDto.data[0].ehr_pass;
                        WindowsUser = EHR_InfoDto.data[0].server_user;
                        WindowsPass = EHR_InfoDto.data[0].server_pass;
                        Is_Valid = EHR_InfoDto.data[0].is_valid;
                        Is_Confirmed = EHR_InfoDto.data[0].is_confirmed;
                        Is_Installed = EHR_InfoDto.data[0].is_installed;
                        UserID = EHR_InfoDto.data[0].userId;
                        UserName = EHR_InfoDto.data[0].user_name;

                        OrganisationID = EHR_InfoDto.data[0].organizationId;
                        OrganisationName = Utility.Organization_Name.ToString();
                        LocationId = EHR_InfoDto.data[0].locationId;
                        try
                        {
                            LocationName = Convert.ToString(Utility.DtLocationList.Select("Loc_Id = '" + LocationId + "'").FirstOrDefault()["name"]);
                        }
                        catch
                        {
                        }


                        EHRUserResult = false;

                        DataTable dtLocalZohoDetails = SynchLocalBAL.GetLocalZohoDetailsData();
                        DataRow drLocalZohoDetails = dtLocalZohoDetails.Select("Location_Id = '" + LocationId + "'").FirstOrDefault();

                        #region CheckDetails
                        bool blnCheck = false;
                        try
                        {
                            if (drLocalZohoDetails != null)
                            {
                                if (drLocalZohoDetails["Zoho_LocalDB_ID"] != DBNull.Value)
                                {
                                    ZohoLocalDBID = Convert.ToInt32(drLocalZohoDetails["Zoho_LocalDB_ID"]);
                                }

                                string strTmpServerUser = "";
                                if (drLocalZohoDetails["Server_User"] != DBNull.Value)
                                {
                                    strTmpServerUser = Convert.ToString(drLocalZohoDetails["Server_User"]);
                                    Utility.WindowsUserName = strTmpServerUser;
                                }
                                if (strTmpServerUser != WindowsUser.ToString().Trim())
                                {
                                    Utility.WindowsUserName = WindowsUser;
                                    Utility.WindowsUserResult = false;
                                    blnCheck = true;
                                }

                                string strTmpServerPass = "";
                                if (drLocalZohoDetails["Server_Pass"] != DBNull.Value)
                                {
                                    strTmpServerPass = Convert.ToString(drLocalZohoDetails["Server_Pass"]);
                                    Utility.WindowsUserPassword = strTmpServerPass;
                                }
                                if (strTmpServerPass != WindowsPass.ToString().Trim())
                                {
                                    Utility.WindowsUserPassword = WindowsPass;
                                    Utility.WindowsUserResult = false;
                                    blnCheck = true;
                                }

                                bool blnTmpIsConfirmed = false;
                                if (drLocalZohoDetails["Is_Confirmed"] != DBNull.Value)
                                {
                                    blnTmpIsConfirmed = Convert.ToBoolean(drLocalZohoDetails["Is_Confirmed"]);
                                    Utility.IsConfirmed = blnTmpIsConfirmed;
                                }
                                if (blnTmpIsConfirmed != Is_Confirmed)
                                {
                                    Utility.IsConfirmed = Is_Confirmed;
                                    if (blnTmpIsConfirmed == false && Is_Confirmed == true)
                                    {
                                        Utility.WindowsUserResult = false;
                                        blnCheck = true;
                                    }
                                }


                                bool blnTmpIsValid = false;
                                if (drLocalZohoDetails["Is_Valid"] != DBNull.Value)
                                {
                                    blnTmpIsValid = Convert.ToBoolean(drLocalZohoDetails["Is_Valid"]);
                                    Utility.WindowsUserResult = blnTmpIsValid;
                                }

                                Utility.WindowsUserResult = Convert.ToBoolean(Is_Valid);
                                if (Utility.WindowsUserResult == false)
                                {
                                    blnCheck = true;
                                }
                            }
                            else
                            {
                                blnCheck = true;
                                Utility.WindowsUserName = WindowsUser;
                                Utility.WindowsUserPassword = WindowsPass;
                                Utility.WindowsUserResult = false;
                                Utility.IsConfirmed = Is_Confirmed;
                            }
                        }
                        catch
                        {
                            blnCheck = true;
                        }

                        Utility.IsInstalled = CheckAppExist();
                        if (Is_Installed == false && Is_Confirmed == true)
                        {
                            blnCheck = true;
                        }

                        if (blnCheck && Is_Confirmed)
                        {
                            if (!string.IsNullOrEmpty(Utility.WindowsUserName) && !string.IsNullOrEmpty(Utility.WindowsUserPassword))
                            {
                                WindowsUserResult = IsValidUser(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), ref strErrMsg);
                            }
                            if (Utility.WindowsUserResult == false && WindowsUserResult == true)
                            {
                                UpdateTaskScheduler(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), false);
                                UpdateWindowsService(Utility.WindowsUserName, AESHelper.Decrypt2(Utility.WindowsUserPassword), false);
                            }
                            Is_Valid = WindowsUserResult;
                        }

                        if (WindowsUserResult != Utility.WindowsUserResult)
                        {
                            Utility.WindowsUserResult = WindowsUserResult;
                            SynchDataLiveDB_Push_EHRAndSystemCreds_Results();
                        }

                        if (Utility.WindowsUserResult == true && Utility.IsConfirmed == true && Utility.IsInstalled == false)
                        {
                            Utility.ZohoInstall = "install";
                            Utility.IsInstalled = InstallZohoAccess(strLocationName, strOrgName, System.Environment.MachineName);
                            Utility.WriteToErrorLogFromAll("[InstallZohoAccess: Utility.IsInstalled: " + Utility.IsInstalled + " ]");
                            Is_Installed = Utility.IsInstalled;
                            Utility.WriteToErrorLogFromAll("[InstallZohoAccess: Is_Installed: " + Is_Installed + " ]");
                            SynchLocalBAL.Save_ZohoDetailsData(ZohoLocalDBID, OrganisationID, OrganisationName, LocationId, LocationName, EHRPass, EHRUser, WindowsUser, WindowsPass, Is_Confirmed, Is_Installed, Convert.ToBoolean(Is_Valid), UserID, UserName, Clinic_Number, Service_Install_Id);
                            SynchDataLiveDB_Push_ZohoInstall_Results();
                        }

                        #endregion



                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("[SynchDataLiveDB_Pull_ZohoInstall] : Error: " + ex.Message);
            }
        }

        public static void SynchDataLiveDB_Push_ZohoInstall_Results()
        {
            try
            {
                bool IsFlag = true;
                ZohoInstallUpdate ehrInfo = new ZohoInstallUpdate
                {
                    locationId = Utility.Loc_ID,
                    is_installed = Utility.IsInstalled,
                    message = ""
                };
                StringBuilder JsonEhrInfo = new StringBuilder();
                string strEhrInfo = "";
                var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                JsonEhrInfo.Append(javaScriptApptStatusSerializer.Serialize(ehrInfo) + ",");

                if (JsonEhrInfo.Length > 0)
                {
                    string jsonString = JsonEhrInfo.ToString().Remove(JsonEhrInfo.Length - 1);
                    strEhrInfo = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_ZohoInstall_Results(jsonString, "zohoinstallvalidate", Utility.Loc_ID);

                    if (strEhrInfo.ToLower() != "Success".ToLower())
                    {
                        GoalBase.WriteToErrorLogFile_Static("[ZohoInstall_Results Sync (Local Database To Adit Server) ] Location_Id: " + Utility.Loc_ID);
                    }
                }
                else
                {
                    strEhrInfo = "Success";
                }

                if (strEhrInfo.ToLower() == "Success".ToLower())
                {
                    IsFlag = true;
                }
                else
                {
                    if (strEhrInfo.Contains("The remote name could not be resolved:"))
                    {
                        IsFlag = false;
                    }
                    else
                    {
                        GoalBase.WriteToErrorLogFile_Static("[ZohoInstall_Results Sync (Local Database To Adit Server) ] : " + strEhrInfo);
                        IsFlag = false;
                    }
                }

                if (IsFlag)
                {
                    GoalBase.WriteToSyncLogFile_Static("ZohoInstall_Results Sync (Local Database To Adit Server) Successfully.");
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[ZohoInstall_Results Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        public static bool InstallZohoAccess(string positiveLocationName, string positiveOrganizationName, string systemName)
        {
            bool flag = false;
            try
            {
                Utility.WriteToErrorLogFromAll("[InstallZohoAccess: Start ]");
                if (Utility.ZohoInstall.ToLower() == "install")
                {
                    #region Call Exe to Install Zoho Access
                    string exePath = Application.StartupPath.EndsWith("\\") ? Application.StartupPath + "ZohoAccess\\ZohoUA.exe" : Application.StartupPath + "\\ZohoAccess\\ZohoUA.exe";  //@"C:\\program.exe";
                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = exePath;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.StartInfo.Verb = "runas";
                        myProcess.StartInfo.RedirectStandardInput = true;
                        myProcess.StartInfo.RedirectStandardOutput = true;
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.StartInfo.Arguments = System.Environment.MachineName + "#Adit#" + Utility.Location_Name + "#Adit#" + Utility.Organization_Name + "#Adit#" + Application.StartupPath + "#Adit#" + "Install";
                        myProcess.Start();
                        myProcess.WaitForExit();
                    }
                    #endregion
                }
                Utility.WriteToErrorLogFromAll("[InstallZohoAccess: flag 1 : " + flag + " ");
                flag = true;
                Utility.WriteToErrorLogFromAll("[InstallZohoAccess: flag 2 : " + flag + " ");
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("[InstallZohoAccess: Error: " + ex.Message + "]");
            }
            Utility.WriteToErrorLogFromAll("[InstallZohoAccess: End flag 3 : " + flag + " ");
            return flag;
        }


        private bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }


        #endregion

        #region SaveServerUser

        #region TimerFunctions
        private void fncSynchDataSaveServerUser()
        {
            InitBgWorkerSaveServerUser();
            InitBgTimerSaveServerUser();
        }

        private void InitBgTimerSaveServerUser()
        {
            timerSynchSaveServerUser = new System.Timers.Timer();
            this.timerSynchSaveServerUser.Interval = (5 * 60000); //1000 * GoalBase.intervalEHRSynch_Appointment;
            this.timerSynchSaveServerUser.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynchSaveServerUser_Tick);
            timerSynchSaveServerUser.Enabled = true;
            timerSynchSaveServerUser.Start();
            timerSynchSaveServerUser_Tick(null, null);
        }

        private void InitBgWorkerSaveServerUser()
        {
            bwSynchSaveServerUser = new BackgroundWorker();
            bwSynchSaveServerUser.WorkerReportsProgress = true;
            bwSynchSaveServerUser.WorkerSupportsCancellation = true;
            bwSynchSaveServerUser.DoWork += new DoWorkEventHandler(bwSynchSaveServerUser_DoWork);
            //bwSynch.ProgressChanged += new ProgressChangedEventHandler(bwSynch_ProgressChanged);
            bwSynchSaveServerUser.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynchSaveServerUser_RunWorkerCompleted);
        }

        private void timerSynchSaveServerUser_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynchSaveServerUser.Enabled = false;
                MethodForCallSynchOrderSaveServerUser();
            }
        }

        public void MethodForCallSynchOrderSaveServerUser()
        {
            System.Threading.Thread procThreadmainSaveServerUser = new System.Threading.Thread(this.CallSyncOrderTableSaveServerUser);
            procThreadmainSaveServerUser.Start();
        }

        public void CallSyncOrderTableSaveServerUser()
        {
            if (bwSynchSaveServerUser.IsBusy != true)
            {
                bwSynchSaveServerUser.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynchSaveServerUser_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynchSaveServerUser.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                //SynchDataLocalToDentrix_Patient_Payment();
                SynchData_SystemUsers();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynchSaveServerUser_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynchSaveServerUser.Enabled = true;
        }
        #endregion

        public static void SynchData_SystemUsers()
        {
            try
            {
                bool IsFlag = true;
                List<string> strUsers = GetComputerUsers();
                DataTable dtLocal = new DataTable();
                dtLocal.Columns.Add("Server_User_LocalDB_ID");
                dtLocal.Columns.Add("Server_User_Web_ID");
                dtLocal.Columns.Add("Server_User_Name");
                dtLocal.Columns.Add("Server_User_Password");
                dtLocal.Columns.Add("Is_Active");
                dtLocal.Columns.Add("is_Deleted");
                dtLocal.Columns.Add("Is_Adit_Updated");
                dtLocal.Columns.Add("Clinic_Number");
                dtLocal.Columns.Add("Service_Install_Id");

                for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        foreach (string strUser in strUsers)
                        {
                            DataRow dr = dtLocal.NewRow();
                            dr["Server_User_Name"] = strUser;
                            dr["Server_User_LocalDB_ID"] = "";
                            dr["Server_User_Web_ID"] = "";
                            dr["Server_User_Password"] = "";
                            dr["Is_Active"] = "true";
                            dr["is_Deleted"] = "false";
                            dr["Is_Adit_Updated"] = "0";
                            dr["Clinic_Number"] = Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString();
                            dr["Service_Install_Id"] = Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString();
                            dtLocal.Rows.Add(dr);
                        }
                    }
                }


                for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                {
                    for (int i = 0; i < Utility.DtLocationList.Rows.Count; i++)
                    {
                        DataTable dtUsers = SynchLocalBAL.GetLocalSystemUsersData(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        dtUsers.Columns.Add("InsUptDlt", typeof(int));

                        DataTable dtSaveRecords = new DataTable();
                        dtSaveRecords = dtLocal.Clone();

                        #region "Add"
                        var itemsToBeAdded = (from DentrixPatient in dtLocal.AsEnumerable()
                                              join LocalPatient in dtUsers.AsEnumerable()
                                              on DentrixPatient["Server_User_Name"].ToString().Trim().ToLower() + "_" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString()
                                              equals LocalPatient["Server_User_Name"].ToString().Trim().ToLower() + "_" + Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString()
                                              into matchingRows
                                              from matchingRow in matchingRows.DefaultIfEmpty()
                                              where matchingRow == null
                                              select DentrixPatient).ToList();
                        DataTable dtPatientToBeAdded = dtLocal.Clone();
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

                        #region "Update"
                        var itemToBeDeleted = (from LocalPatient in dtUsers.AsEnumerable()
                                               join DentrixPatient in dtLocal.AsEnumerable()
                                               on LocalPatient["Server_User_Name"].ToString().Trim()
                                               equals DentrixPatient["Server_User_Name"].ToString().Trim()
                                               into matchingRows
                                               from matchingRow in matchingRows.DefaultIfEmpty()
                                               where LocalPatient["is_deleted"].ToString().Trim().ToUpper() == "FALSE" && matchingRow == null
                                               select LocalPatient).ToList();

                        DataTable dtPatientToBeDelete = dtLocal.Clone();
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
                            bool isPatientSave = SynchLocalBAL.Save_SystemUsersData(dtSaveRecords, Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());

                            if (isPatientSave)
                            {
                                SynchDataLiveDB_Push_SystemUsers(Utility.DtLocationList.Rows[i]["Clinic_Number"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EHRAndSystemCreds_Results Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        public static void SynchDataLiveDB_Push_SystemUsers(string Clinic_Number, string Service_Install_Id)
        {
            bool IsFlag;
            try
            {
                DataTable dtPushUsers = SynchLocalBAL.GetPushLocalSystemusersData(Clinic_Number, Service_Install_Id);


                var JsonEhrInfo = new System.Text.StringBuilder();
                foreach (DataRow dr in dtPushUsers.Rows)
                {
                    string apptlocationId = "";
                    string locationId = "";

                    try
                    {
                        var loc = Utility.DtLocationList.Select("Clinic_Number = '" + dr["Clinic_Number"].ToString() + "'");
                        apptlocationId = Convert.ToString(loc[0]["Location_ID"]);
                        locationId = Convert.ToString(loc[0]["Loc_ID"]);
                    }
                    catch
                    {
                    }
                    SaveServerUsers Users = new SaveServerUsers
                    {
                        user_name = Convert.ToString(dr["Server_User_Name"]),
                        password = "",
                        is_active = Convert.ToString(dr["Is_Active"]).Trim().ToLower() == "true" ? true : false,
                        is_deleted = Convert.ToString(dr["Is_Deleted"]).Trim().ToLower() == "true" ? true : false,
                        appointmentlocation = apptlocationId,
                        location = locationId,
                        organization = Utility.Organization_ID
                    };
                    var javaScriptApptStatusSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonEhrInfo.Append(javaScriptApptStatusSerializer.Serialize(Users) + ",");
                }

                string strEhrInfo = "";

                if (JsonEhrInfo.Length > 0)
                {
                    string jsonString = "[" + JsonEhrInfo.ToString().Remove(JsonEhrInfo.Length - 1) + "]";
                    strEhrInfo = PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_SaveUser_Results(jsonString, "systemusers", Utility.Location_ID, Clinic_Number, Service_Install_Id);

                    if (strEhrInfo.ToLower() != "Success".ToLower())
                    {
                        GoalBase.WriteToErrorLogFile_Static("[EHRAndSystemCreds_Results Sync (Local Database To Adit Server) ] Location_Id: " + Utility.Loc_ID);
                    }
                }
                else
                {
                    strEhrInfo = "Success";
                }

                if (strEhrInfo.ToLower() == "Success".ToLower())
                {
                    IsFlag = true;
                }
                else
                {
                    if (strEhrInfo.Contains("The remote name could not be resolved:"))
                    {
                        IsFlag = false;
                    }
                    else
                    {
                        GoalBase.WriteToErrorLogFile_Static("[EHRAndSystemCreds_Results Sync (Local Database To Adit Server) ] : " + strEhrInfo);
                        IsFlag = false;
                    }
                }

                if (IsFlag)
                {
                    GoalBase.WriteToSyncLogFile_Static("EHRAndSystemCreds_Results Sync (Local Database To Adit Server) Successfully.");
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[EHRAndSystemCreds_Results Sync (Local Database To Adit Server) ] : " + ex.Message);
            }
        }

        private static List<string> GetComputerUsers()
        {
            try
            {
                List<string> users = new List<string>();
                var path = string.Format("WinNT://{0},computer", Environment.MachineName);

                using (var computerEntry = new DirectoryEntry(path))
                {
                    foreach (DirectoryEntry childEntry in computerEntry.Children)
                    {
                        if (childEntry.SchemaClassName == "User")
                        {
                            users.Add(childEntry.Name);
                        }
                    }
                }
                return users;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("GetComputerUsers " + ex.Message);
                return null;
            }
        }
        #endregion

        private void CreatePozativeUATaskScheduler()
        {
            try
            {
                string TaskName = "PozativeUATask";
                var ts = new TaskService();
                var task = ts.RootFolder.Tasks.Where(a => a.Name.ToLower() == TaskName.ToLower()).FirstOrDefault();
                if (task == null)
                {
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Check for Pozative Zoho Access Daily";
                    td.Settings.Compatibility = TaskCompatibility.V2;
                    td.Triggers.AddNew(TaskTriggerType.Daily);
                    td.Actions.Add(new ExecAction(Application.StartupPath.ToString() + "\\ZohoUAUtility.exe", "", null));
                    td.Settings.StopIfGoingOnBatteries = false;
                    td.Settings.DisallowStartIfOnBatteries = false;
                    td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
                    td.Principal.RunLevel = TaskRunLevel.Highest;
                    ts.RootFolder.RegisterTaskDefinition(@"" + TaskName.ToString() + "", td, TaskCreation.CreateOrUpdate, "NT AUTHORITY\\System", null, TaskLogonType.ServiceAccount, null);
                }
                else
                {
                   
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Error while create PozativeUATask scheduler " + ex.Message.ToString());
            }
        }

        static bool CheckAppExist()
        {
            string[] astrMatches = GetAllInstalledSoftware("Zoho Assist");

            bool isInstalled = false;
            try
            {
                foreach (string strDisplayName in astrMatches)
                {
                    //Console.WriteLine(strDisplayName);
                    if (strDisplayName == "Zoho Assist Unattended Agent")
                    {
                        isInstalled = true;
                    }
                }

                var regAditZohoService = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\", "Zoho Assist-Unattended Support", null);
                if (regAditZohoService != null)
                {
                    isInstalled = true;
                }

                try
                {
                    using (System.ServiceProcess.ServiceController AdSc = new System.ServiceProcess.ServiceController("Zoho Assist-Unattended Support"))
                    {
                        if (AdSc.Container != null)
                        {
                            isInstalled = true;
                        }
                    }
                }
                catch
                {
                }

            }
            catch (Exception ex)
            {

            }
            return isInstalled;
        }

        public static string[] GetAllInstalledSoftware(string strPrefix)
        {
            const string strUNINSTALL_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            List<string> listMatches = new List<string>();

            // Enumerate all sub keys found under the "Uninstall" key, each sub key represents a installed software
            foreach (string strSubKey in Microsoft.Win32.Registry.LocalMachine.OpenSubKey(strUNINSTALL_KEY).GetSubKeyNames())
            {
                // try to get the "DisplayName" for the installed software
                object objValue = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(strUNINSTALL_KEY + @"\" + strSubKey).GetValue("DisplayName");
                if (objValue != null)
                {
                    string strDisplayName = objValue.ToString();

                    // If display name starts with the desired prefix
                    if (strDisplayName.StartsWith(strPrefix))
                    {
                        // -> add it to the result list
                        listMatches.Add(strDisplayName);
                    }
                }
            }
            return listMatches.ToArray();
        }

    }




    public class AESHelper
    {
        private static readonly string AesKey = "$15#08#47#2020#@DIT!OPENDENTALEH";
        private static readonly string AesIV = "$#!$#!$#!##2020X";

        static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static string Decrypt2(string cipher)
        {
            return Decrypt(cipher, AesKey, AesIV);
        }

        public static string Decrypt(string ciphertextHex, string keyUtf8, string ivHex)
        {
            try
            {
                byte[] ciphertext = StringToByteArrayFastest(ciphertextHex);
                byte[] iv = Encoding.UTF8.GetBytes(ivHex);

                string plaintext = "";
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(keyUtf8);
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;          // default
                    aes.Padding = PaddingMode.PKCS7;    // default

                    ICryptoTransform decipher = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(ciphertext))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decipher, CryptoStreamMode.Read))
                        {
                            using (System.IO.StreamReader sr = new System.IO.StreamReader(cs, Encoding.UTF8)) // UTF8: default
                            {
                                plaintext = sr.ReadToEnd();
                            }
                        }
                    }
                }
                return plaintext;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string Encrypt2(string password)
        {
            return Encrypt(password, AesKey, AesIV);
        }

        public static string Encrypt(string password, string AesKey, string AesIV)
        {
            byte[] key = Encoding.UTF8.GetBytes(AesKey);
            byte[] iv = Encoding.UTF8.GetBytes(AesIV);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encryptedBytes;

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                        cs.Write(passwordBytes, 0, passwordBytes.Length);
                        cs.FlushFinalBlock();
                        encryptedBytes = ms.ToArray();
                    }
                }

                return BitConverter.ToString(encryptedBytes).Replace("-", "");
            }
        }

        public static string Encrypt(string message)
        {
            try
            {
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                byte[] aesIV = UTF8Encoding.UTF8.GetBytes(AesIV);
                byte[] aesKey = UTF8Encoding.UTF8.GetBytes(AesKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = aesKey;
                aes.IV = aesIV;
                byte[] data = Encoding.UTF8.GetBytes(message);
                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(dest);
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                string plaintext = null;
                using (AesManaged aes = new AesManaged())
                {
                    byte[] cipherText = Convert.FromBase64String(encryptedText);
                    byte[] aesIV = UTF8Encoding.UTF8.GetBytes(AesIV);
                    byte[] aesKey = UTF8Encoding.UTF8.GetBytes(AesKey);
                    ICryptoTransform decryptor = aes.CreateDecryptor(aesKey, aesIV);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(cipherText))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (System.IO.StreamReader reader = new System.IO.StreamReader(cs))
                                plaintext = reader.ReadToEnd();
                        }
                    }
                }
                return plaintext;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
