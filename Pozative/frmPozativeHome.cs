
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pozative.BO;
using Pozative.BAL;
using Pozative.UTL;
using System.Net.NetworkInformation;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Pozative.DAL;
using Patterson.Eaglesoft.Library.Dtos;
using System.Diagnostics;
using Pozative.Properties;
using System.Globalization;
using System.Threading;
using Microsoft.Win32;
using Shell32;
using System.Management;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Net.Security;
using RestSharp;
using Newtonsoft.Json;
using System.ServiceProcess;
using System.Timers;
//using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Threading.Tasks;

namespace Pozative
{
    public partial class frmPozative : Form
    {



        #region Variable

        System.Timers.Timer SoftDentSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Appointment);
        System.Timers.Timer SoftDentPatientSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Patient);
        System.Timers.Timer EasyDentalSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Appointment);
        System.Timers.Timer EasyDentalPatientSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Patient);
        System.Timers.Timer CrystalPMSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Appointment);
        System.Timers.Timer CrystalPMPatientSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Patient);
        System.Timers.Timer tmrSyncPartial = new System.Timers.Timer(1000 * Utility.SyncPartialMinutes);
        System.Timers.Timer tmrEasydentalPayment = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_PatientPayment);
        System.Timers.Timer tmrCrystalPMPayment = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_PatientPayment);


        private static bool IsSoftDentSyncing = false;
        private static bool IsEasyDentalSyncing = false;
        private static bool IsEasyDentalPaymentSyncing = false;
        private static bool IsEasyDentalPatientSyncing = false;
        private static bool IsCrystalPMSyncing = false;
        private static bool IsCrystalPMPaymentSyncing = false;
        private static bool IsCrystalPMPatientSyncing = false;
        private static bool IsPracticeAnalytics = false;
        private static bool IsClearLocalRecordsSyncing = false;
        public static bool IsPaymentSyncing = false;
        public static bool IsAWSTranfer = false;

        //Update
        //string versionNo = "1.0.0.7";
        string currentPath = string.Empty;
        string backupDirName = string.Empty;
        string vFolderName = string.Empty;
        bool ApplicationManuallyUpdate = false;
        bool IsSyncFileTransfer = false;
        public static int patientPushCounter = 1000;

        bool IsAppConnection = false;

        GoalBase ObjGoalBase = new GoalBase();

        Color ConnectedColor = ColorTranslator.FromHtml("#F4891F");//Color.FromArgb(190, 255, 219);  //Color.LightGreen;
        Color DisConnectedColor = Color.LightYellow; // Color.FromArgb(255, 108, 123);  // Color.Red;
        Color IdleColor = Color.LightYellow;
        public bool EHRConn = false;
        bool IsEHRAllSync = false;
        DateTime dtAws = new DateTime();
        bool IsGetParientRecordDone = false;
        bool IsParientFirstSync = true;
        bool IsPatientSyncedFirstTime = false;
        static bool IsProviderSyncedFirstTime = false;
        bool IsMedicalHistoryRecordsPulled = false;

        static bool IsParientFirstPushSync = true;

        int TotalPatientRecord = 0;
        int GetPatientRecord = 0;
        static int TotalPushPatientRecord = 0;
        static int PushPatientRecord = 0;
        static int TotalPushOperatoryEventRecord = 0;
        static int PushOperatoryEventRecord = 0;

        string Connectedtxt = "Connected";
        string DisConnectedtxt = "Connecting";
        string Idletxt = "Connecting";

        string SyncConnectedtxt = "Sync Success";
        string SyncDisConnectedtxt = "Syncing";
        string SyncIdletxt = "Syncing";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        bool Is_synched = false;
        bool Is_synched_Provider = false;
        bool Is_synched_Speciality = false;
        bool Is_synched_Operatory = false;
        bool Is_synched_OperatoryEvent = false;
        bool Is_synched_ApptType = false;
        bool Is_synched_PatientDisease = false;
        bool Is_synched_PatientMedication = false;
        bool Is_synched_Appointment = true;
        bool Is_synched_RecallType = false;
        bool Is_synched_ApptStatus = false;
        bool Is_synched_Holidays = false;
        bool Is_synched_MedicalHistory = false;
        bool Is_synched_Patient = false;
        bool Is_synched_AppointmentsPatient = false;
        bool Is_synched_PatinetForm = false;
        bool Is_synched_PatinetPayment = false;
        bool Is_Synched_PatientCallHit = false;
        bool Is_synched_PatientImages = false;
        bool Is_synched_Insurance = false;


        #region Dll Imports

        [DllImport("Dentrix.API.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        static extern int DENTRIXAPI_Initialize(string szUserId, string szPassword);

        private const string DtxAPI = "Dentrix.API.dll";
        [DllImport(DtxAPI, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DENTRIXAPI_RegisterUser([MarshalAs(UnmanagedType.LPStr)] string szKeyFilePath); //G6.2 + available call

        [DllImport(DtxAPI, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern void DENTRIXAPI_GetConnectionString([MarshalAs(UnmanagedType.LPStr)] string szUserId, [MarshalAs(UnmanagedType.LPStr)] string szPassword, StringBuilder szConnectionsString, int ConnectionStringSize); //G5+ available

        [DllImport(DtxAPI, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern float DENTRIXAPI_GetDentrixVersion(); //G5.1+ available
        #endregion

        #region Constants
        const int SUCCESS = 0;
        const int FAIL = 1;
        #endregion




        #endregion

        #region SoftDentVariable
        private bool IsOpen { get; set; }
        private Appointment[] Appointments { get; set; }
        private Provider[] Providers { get; set; }
        private BlockedSlot[] BlockedSlots { get; set; }
        private Provider[] Specialties { get; set; }
        private Operatory[] Operatories { get; set; }
        private Patient[] Patients { get; set; }
        private PatientMin[] PatientMins { get; set; }
        private ISoftDentInterop _Interop = InteropFactory.GetSoftDentInterop();
        #endregion

        #region Form Load

        public frmPozative()
        {
            InitializeComponent();
            // this.FormBorderStyle = FormBorderStyle.None;
            // this.ShowInTaskbar = false;
            this.Size = new Size(900, 650);
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var cp = base.CreateParams;
        //        cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
        //        return cp;
        //    }
        //}

        private void Pozative_Load(object sender, EventArgs e)
        {
            try
            {
                label5.Text = "©" + DateTime.Now.Year.ToString() + " Adit";

                //try
                //{
                //    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                //    registryKey.SetValue("Pozative", Application.ExecutablePath);
                //}
                //catch (Exception)
                //{
                //}


                //For use MaxBufferSize
                try
                {
                    if (!Utility.checkbackupDB())
                    {
                        Utility.UpdateBackupDBFromLocalDB();
                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("Error from checkbackupDB" + ex.Message.ToString());
                }

                if (CommonUtility.GetValueFromAppConfig("UseMaxBufferSize").ToString() == "" || CommonUtility.GetValueFromAppConfig("UseMaxBufferSize").ToString() == "0" || CommonUtility.GetValueFromAppConfig("UseMaxBufferSize").ToString().ToLower().Trim() == "false")
                {
                    RegistryKey keyMxBufferSize = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UseMaxBufferSize");
                    if (keyMxBufferSize == null)
                    {
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseMaxBufferSize");
                        key1.SetValue("UseMaxBufferSize", false);
                    }
                    else
                    {
                        Utility.UseMaxBufferSize = Convert.ToBoolean(keyMxBufferSize.GetValue("UseMaxBufferSize").ToString());
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


                //For use UseAllProvidersForAllClinicOpendental
                if (CommonUtility.GetValueFromAppConfig("UseProvidersForAllClinicOpendental").ToString() == "" || CommonUtility.GetValueFromAppConfig("UseProvidersForAllClinicOpendental").ToString() == "0" || CommonUtility.GetValueFromAppConfig("UseProvidersForAllClinicOpendental").ToString().ToLower().Trim() == "false")
                {
                    RegistryKey keyMxBufferSize = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UseProvidersForAllClinicOpendental");
                    if (keyMxBufferSize == null)
                    {
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseProvidersForAllClinicOpendental");
                        key1.SetValue("UseProvidersForAllClinicOpendental", false);
                    }
                    else
                    {
                        Utility.UseProvidersForAllClinicOpendental = Convert.ToBoolean(keyMxBufferSize.GetValue("UseProvidersForAllClinicOpendental").ToString());
                    }
                }
                else
                {
                    Utility.UseProvidersForAllClinicOpendental = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("UseProvidersForAllClinicOpendental").ToString().Trim());
                    RegistryKey keyMxBufferSize = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UseProvidersForAllClinicOpendental");
                    if (keyMxBufferSize == null)
                    {
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseProvidersForAllClinicOpendental");
                        Utility.UseProvidersForAllClinicOpendental = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("UseProvidersForAllClinicOpendental").ToString());
                        key1.SetValue("UseProvidersForAllClinicOpendental", Utility.UseProvidersForAllClinicOpendental);
                    }
                }
                try
                {
                    #region Chiniese PDF issue Code By RoojA
                    if (CommonUtility.GetValueFromAppConfig("IsChinesePDF").ToString() == "" || CommonUtility.GetValueFromAppConfig("IsChinesePDF").ToString() == "0" || CommonUtility.GetValueFromAppConfig("IsChinesePDF").ToString().ToLower().Trim() == "false")
                    {
                        // MessageBox.Show("1.config value read :  "+CommonUtility.GetValueFromAppConfig("IsChinesePDF").ToString());
                        RegistryKey keyIsChinesePDF = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\IsChinesePDF", true);
                        if (keyIsChinesePDF == null)
                        {
                            RegistryKey key1 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\IsChinesePDF");
                            key1.SetValue("IsChinesePDF", false);
                        }
                        else
                        {
                            Utility.IsChinesePDF = Convert.ToBoolean(keyIsChinesePDF.GetValue("IsChinesePDF").ToString());
                        }
                        // MessageBox.Show("1. Utility.IsChinesePDF  :" + Utility.IsChinesePDF.ToString() + "key1 : " + keyIsChinesePDF.GetValue("IsChinesePDF").ToString());
                    }
                    else
                    {
                        //MessageBox.Show("2.config value read :  "+CommonUtility.GetValueFromAppConfig("IsChinesePDF").ToString());

                        Utility.IsChinesePDF = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("IsChinesePDF").ToString().Trim());
                        RegistryKey keyIsChinesePDF = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\IsChinesePDF", true);
                        if (keyIsChinesePDF == null)
                        {
                            RegistryKey key1 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\IsChinesePDF");
                            Utility.IsChinesePDF = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("IsChinesePDF").ToString());
                            key1.SetValue("IsChinesePDF", Utility.IsChinesePDF);
                        }
                        // MessageBox.Show("2. Utility.IsChinesePDF  :" + Utility.IsChinesePDF.ToString() + "key1 : " + keyIsChinesePDF.GetValue("IsChinesePDF").ToString());
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Utility.IsChinesePDF = false;
                }
                try
                {





                    Utility.AditDocTempPath = Application.StartupPath;
                    Utility.AditPatientProfileImagePath = Application.StartupPath;
                    Utility.LastSyncDatetimePAServer = Convert.ToDateTime(DateTime.Now);
                    Utility.LastSyncDatetimePAServerForAPICall = Convert.ToDateTime(DateTime.Now);
                    Utility.AditPatientProfileDefaultImagePath = Application.StartupPath + "\\Default.jpeg";
                    string patientcountertemp = CommonUtility.GetValueFromAppConfig("PatientPushCounter").ToString();
                    if (patientcountertemp.ToString() != string.Empty)
                    {
                        patientPushCounter = Convert.ToInt32(patientcountertemp);
                    }
                }
                catch (Exception)
                {
                    patientPushCounter = 1000;
                }
                CommonUtility.GetSystemDetails();
                picAppUpdate.Visible = false;
                grpAdit.Visible = false;
                grpPozative.Visible = false;
                btnConfig.Visible = false;
                ApplicationManuallyUpdate = false;
                IsAppConnection = false;

                lblHoliday.Visible = false;
                btnHoliday.Visible = false;
                btnSyncHoliday.Visible = false;


                SetControlsDesign();
                TablesButtonStatus();

                lblVersion.Text = "(" + Utility.Server_App_Version + ")";
                lblHead.Text = "Adit Sync Server " + lblVersion.Text;

                SetAditSyncProcessTime();

                tmrConsoleRun.Enabled = true;
                tmrConsoleRun.Interval = 1000;
                tmrConsoleRun.Start();

                //if (CommonUtility.GetValueFromAppConfig("ProviderHourPushCounter").ToString() == "")
                //{
                //    Utility.ProviderHourPushCounter = 50;
                //}
                //else
                //{
                //    Utility.ProviderHourPushCounter = Convert.ToInt16(CommonUtility.GetValueFromAppConfig("ProviderHourPushCounter").ToString());
                //}

                //if (Utility.ProviderHourPushCounter == 50)
                //{
                //    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\ProviderHourPushCounter");
                //    if (key == null)
                //    {
                //        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\ProviderHourPushCounter");
                //        key1.SetValue("ProviderHourPushCounter", 50);
                //    }

                //}
                //else
                //{
                //    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\ProviderHourPushCounter");
                //    if (key != null)
                //    {
                //        Utility.ProviderHourPushCounter = Int16.Parse(key.GetValue("ProviderHourPushCounter").ToString());
                //    }
                //    else
                //    {
                //        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\ProviderHourPushCounter");
                //        key1.SetValue("ProviderHourPushCounter", Utility.ProviderHourPushCounter);
                //    }
                //}
                Utility.ProviderHourPushCounter = 50;


                #region set AppointmentExternalsyncOn or off
                if (CommonUtility.GetValueFromAppConfig("IsExternalAppointmentSync").ToString() == "")
                {
                    Utility.IsExternalAppointmentSync = false;
                }
                else
                {
                    Utility.IsExternalAppointmentSync = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("IsExternalAppointmentSync").ToString());
                }

                if (Utility.IsExternalAppointmentSync == false)
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\IsExternalAppointmentSync");
                    if (key == null)
                    {
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\IsExternalAppointmentSync");
                        key1.SetValue("IsExternalAppointmentSync", false);
                    }
                    else
                    {
                        Utility.IsExternalAppointmentSync = Convert.ToBoolean(key.GetValue("IsExternalAppointmentSync").ToString());
                    }
                }
                else
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\IsExternalAppointmentSync");
                    if (key == null)
                    {
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\IsExternalAppointmentSync");
                        key1.SetValue("IsExternalAppointmentSync", Utility.ProviderHourPushCounter);
                    }
                }
            #endregion
            startConnection:
                if (CheckSystemAndApplicaitonConnection())
                {
                    if (Utility.IsExternalAppointmentSync)
                    {
                        foreach (Process p in Process.GetProcesses())
                        {
                            if (string.Compare("AppointmentSync", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                            {
                                p.Kill();
                                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key.SetValue("IsAppointmentSyncSyncing", false);
                                }
                                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key1.SetValue("IsPatinetSyncing", false);
                                }
                                continue;
                            }
                        }
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key.SetValue("IsAppointmentSyncSyncing", false);
                        }
                        EasyDentalSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Appointment);
                        //  SyncEasyDentalRecordsInitialy();
                        EasyDentalSync.Elapsed += new ElapsedEventHandler(AppointmentSync_Elapsed);
                        EasyDentalSync.Start();
                    }
                    if (CommonUtility.GetValueFromAppConfig("SyncPartialMinutes").ToString() == "" || CommonUtility.GetValueFromAppConfig("SyncPartialMinutes").ToString() == "0")
                    {
                        Utility.SyncPartialMinutes = 100;
                    }
                    else
                    {
                        Utility.SyncPartialMinutes = Convert.ToInt16(CommonUtility.GetValueFromAppConfig("SyncPartialMinutes").ToString());
                    }
                    Utility.SyncPartialMinutes *= 60;

                    RegistryKey ky = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
                    if (ky == null)
                    {
                        ky = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                        ky.SetValue("SyncPartialSyncing", false);
                        ky.SetValue("ClearLocalRecordSyncing", false);
                    }
                    else
                    {
                        using (RegistryKey ky1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            ky1.SetValue("SyncPartialSyncing", false);
                            ky1.SetValue("ClearLocalRecordSyncing", false);
                        }
                    }
                    tmrSyncPartial = new System.Timers.Timer(1000 * Utility.SyncPartialMinutes);
                    tmrSyncPartial.Elapsed += new ElapsedEventHandler(tmrSyncPartial_Elapsed);
                    tmrSyncPartial.Start();



                    //EagleSoft
                    if (Utility.Application_ID == 1)
                    {
                        try
                        {
                            //EagleSoftDocPath(Utility.DBConnString, "1");
                            Utility.GenerateRandomPatientId = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("GenerateRandomPatientId").ToString());

                            if (Utility.GenerateRandomPatientId == true)
                            {

                                RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\GenerateRandomPatientId");
                                key1.SetValue("IsSyncing", true);

                            }
                            else
                            {
                                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\GenerateRandomPatientId");
                                if (key != null)
                                {
                                    Utility.GenerateRandomPatientId = bool.Parse(key.GetValue("IsSyncing").ToString());
                                }
                            }
                            Utility.EHR_VersionNumber = SynchEaglesoftBAL.GetEaglesoftEHR_VersionNumber(Utility.DBConnString);

                            try
                            {
                                for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                                {
                                    EagleSoftDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                                    // SynchEaglesoftDAL.Insert_Patient_Prompt_To_EagleSoft(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString());                                   
                                }
                            }
                            catch (Exception)
                            {

                            }

                        }
                        catch (Exception)
                        {
                            Utility.GenerateRandomPatientId = false;
                        }
                    }
                    //Opendental
                    if (Utility.Application_ID == 2)
                    {
                        try
                        {
                            OpenDentalDocPath(Utility.DBConnString, "1");
                            Utility.EHR_VersionNumber = SynchOpenDentalBAL.GetOpenDentalActualVersion(Utility.DBConnString);
                        }
                        catch (Exception)
                        {
                            //Utility.ChartNumberIsNumeric = false;
                        }
                    }
                    //Dentrix
                    if (Utility.Application_ID == 3)
                    {
                        try
                        {
                            var r = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Dentrix Dental Systems, Inc.\Dentrix\General", "Installed Version", null);
                            Utility.EHR_VersionNumber = r.ToString().Split(' ')[0];
                            //Utility.GenerateRandomPatientId = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("ChartNumberIsNumeric").ToString());
                            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\ChartNumberIsNumeric");
                            if (key != null)
                            {
                                Utility.ChartNumberIsNumeric = bool.Parse(key.GetValue("IsSyncing").ToString());
                            }
                            else
                            {
                                Utility.ChartNumberIsNumeric = Convert.ToBoolean(CommonUtility.GetValueFromAppConfig("ChartNumberIsNumeric").ToString());
                                RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\ChartNumberIsNumeric");
                                key1.SetValue("IsSyncing", Utility.ChartNumberIsNumeric);
                            }
                        }
                        catch (Exception)
                        {
                            Utility.ChartNumberIsNumeric = true;
                        }
                        try
                        {
                            if (Utility.Application_Version.ToLower() == "DTX G6.2+".ToLower())
                            {
                                string exePath = string.Empty;
                                StringBuilder path = new StringBuilder(512);
                                string connectionString = string.Empty;
                                if (CommonFunction.GetDentrixG62ExePath(path, false) == SUCCESS)
                                {
                                    exePath = path.ToString();
                                    // CommonUtility.GetDentrixDLLForDocumentUpload(exePath);
                                }
                            }
                            // Utility.EHR_VersionNumber = SynchDentrixBAL.GetDenrtrixEHR_VersionNumber();
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToSyncLogFile("Error while transfer file " + ex.Message.ToString());
                        }
                        try
                        {
                            foreach (Process p in Process.GetProcesses())
                            {
                                if (string.Compare("DentrixDocument".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                                {
                                    p.Kill();
                                }
                                if (string.Compare("DTX_Helper".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                                {
                                    p.Kill();
                                }
                            }
                            if (Utility.DentrixDocPWD == null || Utility.DentrixDocPWD == "")
                            {
                                string docconnection = Utility.GetAppSettingsString("docconnection", "docconnection", "");
                                Utility.DentrixDocConnString = (docconnection != null && docconnection != "") ? Utility.DecryptString(docconnection) : "";
                                string docpassword = Utility.GetAppSettingsString("docpassword", "docpassword", "");
                                Utility.DentrixDocPWD = (docpassword != null && docpassword != "") ? Utility.DecryptString(docpassword) : "";
                                if (Utility.DentrixDocPWD == null || Utility.DentrixDocPWD == "")
                                {
                                    GoalBase.CreateShortcut();
                                }
                            }
                        }
                        catch (Exception ex1)
                        {
                            ObjGoalBase.WriteToSyncLogFile("Error while Find Doc pwd for pdf Attachment " + ex1.Message.ToString());
                        }
                    }
                    //Cleardent
                    if (Utility.Application_ID == 5)
                    {
                        try
                        {
                            ClearDentDocPath();
                            Utility.EHR_VersionNumber = SynchClearDentBAL.GetClearDentEHR_VersionNumber();
                        }
                        catch (Exception)
                        {
                            //Utility.ChartNumberIsNumeric = false;
                        }
                    }

                    // Softdent synching will start from here
                    if (Utility.Application_ID == 4)
                    {
                        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                        key.SetValue("IsSyncing", false);
                        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\SoftdentAppointmentSync");
                        key1.SetValue("IsSyncing", false);

                        // ObjGoalBase.WriteToSyncLogFile("Execute softdent interval ");

                        SoftDentSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Patient);
                        SoftDentPatientSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Appointment);
                        SyncSoftDentRecordsInitialy();
                        SoftDentSync.Elapsed += new ElapsedEventHandler(SoftDentSync_Elapsed);
                        SoftDentSync.Start();
                        SoftDentPatientSync.Elapsed += new ElapsedEventHandler(SoftDentPatientSync_Elapsed);
                        SoftDentPatientSync.Start();
                        try
                        {
                            var r = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Infosoft\SoftDent\Init", "LastVersionRan", null);
                            if (r == null)
                            {
                                r = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Infosoft\Init", "LastVersionRan", null);
                            }
                            Utility.EHR_VersionNumber = r.ToString();
                        }
                        catch (Exception e3)
                        {
                            ObjGoalBase.WriteToErrorLogFile("Softdent Sync Version error : " + e3.Message.ToString());
                        }
                    }
                    if (Utility.Application_ID == 6)
                    {
                        #region TrackerScheduleShruCode
                        //if (Utility.TrackerScheduleInterval.ToString() == "")
                        //{
                        //    Utility.TrackerScheduleInterval = 15;
                        //}
                        //else
                        //{
                        //    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TrackerScheduleIntervalTimeSlot");
                        //    if (key != null)
                        //    {
                        //        Utility.TrackerScheduleInterval = Int16.Parse(key.GetValue("TrackerScheduleInterval").ToString());
                        //    }
                        //    else
                        //    {
                        //        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerScheduleIntervalTimeSlot");
                        //        key1.SetValue("TrackerScheduleInterval", Utility.TrackerScheduleInterval);
                        //    }
                        //}
                        if (CommonUtility.GetValueFromAppConfig("TrackerScheduleInterval").ToString() == "")
                        {
                            Utility.TrackerScheduleInterval = 15;
                        }
                        else
                        {
                            Utility.TrackerScheduleInterval = Convert.ToInt16(CommonUtility.GetValueFromAppConfig("TrackerScheduleInterval").ToString());
                        }

                        try
                        {
                            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TrackerScheduleInterval");
                            if (key != null)
                            {
                                Utility.TrackerScheduleInterval = Int16.Parse(key.GetValue("TrackerScheduleInterval").ToString());
                            }
                            else
                            {
                                RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerScheduleInterval");
                                Utility.TrackerScheduleInterval = 15;
                                key1.SetValue("TrackerScheduleInterval", Utility.TrackerScheduleInterval);

                            }
                        }
                        catch (Exception)
                        {
                            try
                            {
                                RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerScheduleInterval");
                                Utility.TrackerScheduleInterval = 15;
                                key1.SetValue("TrackerScheduleInterval", Utility.TrackerScheduleInterval);
                            }
                            catch (Exception)
                            {
                                Utility.TrackerScheduleInterval = 15;
                            }
                        }
                        //Tracker [Note Move to Correspond one time] 10-08-2023 Sandeep Sharma
                        try
                        {
                            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TrackerNoteMove");
                            if (key != null)
                            {
                                Utility.TrackerNoteMove = Int16.Parse(key.GetValue("TrackerNoteMove").ToString());
                            }
                            else
                            {
                                RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerNoteMove");
                                Utility.TrackerNoteMove = 0;
                                key1.SetValue("TrackerNoteMove", Utility.TrackerNoteMove);

                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerNoteMove");
                                Utility.TrackerNoteMove = 0;
                                key1.SetValue("TrackerNoteMove", Utility.TrackerNoteMove);
                            }
                            catch (Exception)
                            {
                                Utility.TrackerNoteMove = 1;
                            }
                        }

                        //    if (Utility.TrackerScheduleInterval == 15)
                        //{
                        //    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TrackerScheduleInterval");
                        //    if (key == null)
                        //    {
                        //        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerScheduleInterval");
                        //        key1.SetValue("TrackerScheduleInterval", 15);
                        //    }

                        //}
                        //else
                        //{
                        //    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\TrackerScheduleInterval");
                        //    if (key != null)
                        //    {
                        //        Utility.TrackerScheduleInterval = Int16.Parse(key.GetValue("TrackerScheduleInterval").ToString());
                        //    }
                        //    else
                        //    {
                        //        RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TrackerScheduleInterval");
                        //        key1.SetValue("TrackerScheduleInterval", Utility.TrackerScheduleInterval);
                        //    }
                        //}
                        #endregion
                        Utility.EHR_VersionNumber = SynchTrackerBAL.GetEHR_VersionNumber();
                        #region DocumentPath
                        TrackerDocPath("1");
                        #endregion
                    }
                    if (Utility.Application_ID == 7)
                    {
                        Utility.Is_PWPatientCellPhoneAvailable = SynchPracticeWorkBAL.GetPracticeWorkPatientCellPhoneStatusData(true);
                        Utility.Is_PWPatientFillerAvailable = SynchPracticeWorkBAL.GetPracticeWorkPatientCellPhoneStatusData(false);
                        try
                        {
                            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PracticeWorkInactivePatientCodes");
                            if (key != null)
                            {
                                Utility.PW_InactivePatientCodes = key.GetValue("PracticeWorkInactivePatientCodes").ToString();
                            }
                            else
                            {
                                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PracticeWorkInactivePatientCodes"))
                                {
                                    key1.SetValue("PracticeWorkInactivePatientCodes", "65,66,70,81,84,85,86,90,92,94,116,117");
                                    Utility.PW_InactivePatientCodes = "65,66,70,81,84,85,86,90,92,94,116,117";
                                }
                            }
                        }
                        catch (Exception PW_InactivePatient)
                        {
                            Utility.WriteToErrorLogFromAll("Err in PracticeWorkInactivePatientCodes - " + PW_InactivePatient.Message);
                        }
                    }
                    //EasyDental synching will start from here
                    if (Utility.Application_ID == 8)
                    {
                        lblOperatoryEvent.Visible = false;
                        btnOperatoryEvent.Visible = false;
                        btnSyncOperatoryEvent.Visible = false;
                        lblHoliday.Visible = false;
                        btnHoliday.Visible = false;
                        btnSyncHoliday.Visible = false;
                        //fncAditLocationSyncEnable();

                        foreach (Process p in Process.GetProcesses())
                        {
                            if (string.Compare("EasyDentalSync", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
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
                                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key1.SetValue("IsEasyDentalPaymentSyncing", false);
                                }
                                continue;
                            }
                        }
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key.SetValue("IsEasyDentalSyncing", false);
                        }
                        EasyDentalSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Appointment);
                        //SyncEasyDentalRecordsInitialy();
                        EasyDentalSync.Elapsed += new ElapsedEventHandler(EasyDentalSync_Elapsed);
                        EasyDentalSync.Start();

                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsEasyDentalPatinetSyncing", false);
                        }

                        var r = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Easy Dental Systems, Inc.\Easy Dental\General", "Installed Version", null);
                        Utility.EHR_VersionNumber = r.ToString().Split(' ')[0];

                        EasyDentalPatientSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Patient);
                        //SyncEasyDentalRecordsInitialy();
                        EasyDentalPatientSync.Elapsed += new ElapsedEventHandler(EasyDentalPatientSync_Elapsed);
                        EasyDentalPatientSync.Start();

                        #region easydental payment synch code (shruti)
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key.SetValue("IsEasyDentalPaymentSyncing", false);
                        }
                        tmrEasydentalPayment = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_PatientPayment);
                        //SyncEasyDentalRecordsInitialy();
                        tmrEasydentalPayment.Elapsed += new ElapsedEventHandler(EasydentalPayment_Elapsed);
                        tmrEasydentalPayment.Start();
                        #endregion


                    }
                    if (Utility.Application_ID == 10)
                    {
                        try
                        {
                            PracticeWebDocPath(Utility.DBConnString, "1");
                            Utility.EHR_VersionNumber = SynchPracticeWebBAL.GetPracticeWebActualVersion(Utility.DBConnString);
                        }
                        catch (Exception)
                        {
                            //Utility.ChartNumberIsNumeric = false;
                        }
                    }
                    if (Utility.Application_ID == 12)
                    {
                        
                        //fncAditLocationSyncEnable();

                        foreach (Process p in Process.GetProcesses())
                        {
                            if (string.Compare("CrystalPM", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
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
                                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                {
                                    key1.SetValue("IsCrystalPMPaymentSyncing", false);
                                }
                                continue;
                            }
                        }
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key.SetValue("IsCrystalPMSyncing", false);
                        }
                        CrystalPMSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Appointment);
                        //SyncCrystalPMRecordsInitialy();
                        CrystalPMSync.Elapsed += new ElapsedEventHandler(CrystalPMSync_Elapsed);
                        CrystalPMSync.Start();

                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsCrystalPMPatinetSyncing", false);
                        }


                        CrystalPMPatientSync = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_Patient);
                        //SyncCrystalPMRecordsInitialy();
                        CrystalPMPatientSync.Elapsed += new ElapsedEventHandler(CrystalPMPatientSync_Elapsed);
                        CrystalPMPatientSync.Start();

                        #region CrystalPM payment synch code (shruti)
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key.SetValue("IsCrystalPMPaymentSyncing", false);
                        }
                        tmrCrystalPMPayment = new System.Timers.Timer(1000 * GoalBase.intervalEHRSynch_PatientPayment);
                        //SyncCrystalPMRecordsInitialy();
                        tmrCrystalPMPayment.Elapsed += new ElapsedEventHandler(CrystalPMPayment_Elapsed);
                        tmrCrystalPMPayment.Start();
                        #endregion


                    }
                    if (Utility.Application_ID == 11)
                    {
                        try
                        {
                            AbelDentDocPath(Utility.DBConnString, "1");
                        }
                        catch (Exception)
                        {
                            //Utility.ChartNumberIsNumeric = false;
                        }
                    }


                    if (Utility.Application_ID == 13)
                    {
                        try
                        {

                            //PracticeWebDocPath(Utility.DBConnString, "1");
                            Utility.EHR_VersionNumber = "12.0.2";//SynchPracticeWebBAL.GetPracticeWebActualVersion(Utility.DBConnString);
                        }
                        catch (Exception)
                        {
                            //Utility.ChartNumberIsNumeric = false;
                        }
                    }

                    try
                    {
                        // CheckApplicationUpdateVersion();
                        foreach (Process p in Process.GetProcesses())
                        {
                            if (string.Compare("PracticeAnalytics".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                            {
                                p.Kill();
                                ObjGoalBase.WriteToSyncLogFile("Practice Analytics : Start Found. Close the PA.exe");
                                break;
                            }
                        }

                        GetWebAdminUserToken();
                        if (Utility.IsApplicationIdleTimeSet)
                        {
                            tmrApplicationIdleTime.Enabled = true;
                            tmrApplicationIdleTime.Interval = 1000;
                            tmrApplicationIdleTime.Tick += tmrApplicationIdleTime_Tick;
                            tmrApplicationIdleTime.Start();
                        }

                        //tmrPaymentSMSCallLog.Enabled = true;
                        //tmrPaymentSMSCallLog.Interval = 1000 * GoalBase.intervalEHRSynch_Provider; //1000 * 60 * 30; //
                        //tmrPaymentSMSCallLog.Tick += tmrPaymentSMSCallLog_Tick;
                        //tmrPaymentSMSCallLog.Start();

                        //tmrSMSLog.Enabled = true;
                        //tmrSMSLog.Interval = 1000 * GoalBase.intervalEHRSynch_Appointment; //1000 * 60 * 30; //
                        //tmrSMSLog.Tick += tmrSMSLog_Tick;
                        //tmrSMSLog.Start();

                        tmrCheckApplicationUpdate_Tick(null, null);

                        RefreshTrayArea();

                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        grpAdit.Text = "Adit (" + Utility.AditLocationName + ")";
                        grpPozative.Text = "Pozative (" + Utility.PozativeLocationName + ")";
                        Utility.Application_StartDate = Utility.Datetimesetting();
                        bool RemoveSyncTable = SystemBAL.RemoveSyncTableLastSyncLog("All");

                    }
                    catch (Exception)
                    {
                    }
                    SetToolTipOnControls();
                    CheckVisibleControl();

                    AppVersionAutoUpdateserverData();
                    if (Utility.AditSync)
                    {
                        SyncWithAditAppServer();
                        UpdateApplicationVersionOnLiveDatabase();
                        lblHead.Text = "Adit Sync Server " + lblVersion.Text;

                    }

                    if (Utility.SyncPracticeAnalytics)
                    {
                        CallSynchPracticeAnalytics();
                        ObjGoalBase.WriteToSyncLogFile("Practice Analytics Sync : " + Utility.SyncPracticeAnalytics.ToString());
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("Practice Analytics Sync : " + Utility.SyncPracticeAnalytics.ToString());
                    }
                    if (Utility.EHR_VersionNumber.ToString() != "" || Utility.EHR_VersionNumber.ToString() != string.Empty)
                    {
                        bool EHR_VersionNumUpdate = SystemBAL.UpdateEHR_version(Utility.EHR_VersionNumber.ToString());
                        PushLiveDatabaseBAL.Push_Local_To_LiveDatabase_UpdateclientEHRVersion(Utility.Location_ID, Utility.EHR_VersionNumber.ToString());
                    }

                }
                else
                {
                    if (Utility.Application_ID == 1)
                    {
                        bool isconn = false;
                        while (!isconn)
                        {
                            // this if is for when  eaglesoft server is down then we need to check connection
                            if (SynchEaglesoftDAL.GetEaglesoftConnection(Utility.DBConnString))//SynchEaglesoftDAL.GetEaglesoftConnection(Utility.DBConnString)
                            {
                                isconn = true;
                                break;
                            }
                            else
                            {
                                EaglesoftConfiguration();
                                if(IsAppConnection)// connection string changed and server is connected
                                {
                                    isconn = true;
                                    break;
                                }
                                else if (SynchEaglesoftDAL.GetEaglesoftConnection(Utility.DBConnString))//connection string is same, so check database server is up or not
                                {
                                    isconn = true;
                                    break;
                                }
                            }
                            Thread.Sleep(60000 * 10); // 60000 = 1 minute
                        }
                        if (isconn)
                        {
                            goto startConnection;
                        }
                    }
                }
                StartAndInstallAditEventListener();
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Err in Pozative_Load() - " + ex.Message);
            }
        }
      
        private void AppVersionAutoUpdateserverData()
        {
            try
            {
                // bool isAppVersionAutoUpdateserverData = SystemBAL.ApplicationUpdateServerDate();
                string AppVersionAutoUpdateAditserverData = PushLiveDatabaseBAL.Push_Location_EHRUPdateForVersion();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("AppVersion AutoUpdate ServerData : " + ex.Message);
            }
        }

        #endregion

        #region setSystemDateTimeAndSharedFolder

        private void setSystemDateTimeAndSharedFolder()
        {
            try
            {
                RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
                rkey.SetValue("sShortDate", "MM/dd/yyyy");
                rkey.SetValue("sLongDate", "MM/dd/yyyy");
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[setSystemDateTime]" + ex.Message);
                // ObjGoalBase.ErrorMsgBox("Error", ex.Message);
            }

            //ShareFolder(Application.StartupPath.ToString(), "Pozative", "");
        }

        public string ShareFolder(string FolderPath, string ShareName, string Description)
        {
            string strSharePath = FolderPath;
            string strShareName = ShareName;
            string strShareDesc = Description;
            string msg = string.Empty;
            try
            {
                Directory.CreateDirectory(strSharePath);
                ManagementClass oManagementClass = new ManagementClass("Win32_Share");
                ManagementBaseObject inputParameters = oManagementClass.GetMethodParameters("Create");
                ManagementBaseObject outputParameters;
                inputParameters["Description"] = strShareDesc;
                inputParameters["Name"] = strShareName;
                inputParameters["Path"] = strSharePath;
                inputParameters["Type"] = 0x0;//disk drive 
                inputParameters["MaximumAllowed"] = null;
                inputParameters["Access"] = null;
                inputParameters["Password"] = null;
                outputParameters = oManagementClass.InvokeMethod("Create", inputParameters, null);

                if ((uint)(outputParameters.Properties["ReturnValue"].Value) != 0)
                {
                }

                AddPermissions(ShareName, System.Environment.MachineName, System.Environment.UserName);
            }
            catch (Exception)
            {
            }
            return msg;
        }

        public void AddPermissions(string sharedFolderName, string domain, string userName)
        {

            // Step 1 - Getting the user Account Object
            ManagementObject sharedFolder = GetSharedFolderObject(sharedFolderName);
            if (sharedFolder == null)
            {
                System.Diagnostics.Trace.WriteLine("The shared folder with given name does not exist");
                return;
            }

            ManagementBaseObject securityDescriptorObject = sharedFolder.InvokeMethod("GetSecurityDescriptor", null, null);
            if (securityDescriptorObject == null)
            {
                System.Diagnostics.Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Error extracting security descriptor of the shared path {0}.", sharedFolderName));
                return;
            }
            int returnCode = Convert.ToInt32(securityDescriptorObject.Properties["ReturnValue"].Value);
            if (returnCode != 0)
            {
                System.Diagnostics.Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Error extracting security descriptor of the shared path {0}. Error Code{1}.", sharedFolderName, returnCode.ToString()));
                return;
            }

            ManagementBaseObject securityDescriptor = securityDescriptorObject.Properties["Descriptor"].Value as ManagementBaseObject;

            // Step 2 -- Extract Access Control List from the security descriptor
            int existingAcessControlEntriesCount = 0;
            ManagementBaseObject[] accessControlList = securityDescriptor.Properties["DACL"].Value as ManagementBaseObject[];

            if (accessControlList == null)
            {
                // If there aren't any entries in access control list or the list is empty - create one
                accessControlList = new ManagementBaseObject[1];
            }
            else
            {
                // Otherwise, resize the list to allow for all new users.
                existingAcessControlEntriesCount = accessControlList.Length;
                Array.Resize(ref accessControlList, accessControlList.Length + 1);
            }

            // Step 3 - Getting the user Account Object
            ManagementObject userAccountObject = GetUserAccountObject(domain, userName);
            ManagementObject securityIdentfierObject = new ManagementObject(string.Format("Win32_SID.SID='{0}'", (string)userAccountObject.Properties["SID"].Value));
            securityIdentfierObject.Get();

            // Step 4 - Create Trustee Object
            ManagementObject trusteeObject = CreateTrustee(domain, userName, securityIdentfierObject);

            // Step 5 - Create Access Control Entry
            ManagementObject accessControlEntry = CreateAccessControlEntry(trusteeObject, false);

            // Step 6 - Add Access Control Entry to the Access Control List
            accessControlList[existingAcessControlEntriesCount] = accessControlEntry;

            // Step 7 - Assign access Control list to security desciptor 
            securityDescriptor.Properties["DACL"].Value = accessControlList;

            // Step 8 - Assign access Control list to security desciptor 
            ManagementBaseObject parameterForSetSecurityDescriptor = sharedFolder.GetMethodParameters("SetSecurityDescriptor");
            parameterForSetSecurityDescriptor["Descriptor"] = securityDescriptor;
            sharedFolder.InvokeMethod("SetSecurityDescriptor", parameterForSetSecurityDescriptor, null);
        }

        /// <summary>
        /// The method returns ManagementObject object for the shared folder with given name
        /// </summary>
        /// <param name="sharedFolderName">string containing name of shared folder</param>
        /// <returns>Object of type ManagementObject for the shared folder.</returns>

        private static ManagementObject GetSharedFolderObject(string sharedFolderName)
        {
            ManagementObject sharedFolderObject = null;

            //Creating a searcher object to search 
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * from Win32_LogicalShareSecuritySetting where Name = '" + sharedFolderName + "'");
            ManagementObjectCollection resultOfSearch = searcher.Get();
            if (resultOfSearch.Count > 0)
            {
                //The search might return a number of objects with same shared name. I assume there is just going to be one
                foreach (ManagementObject sharedFolder in resultOfSearch)
                {
                    sharedFolderObject = sharedFolder;
                    break;
                }
            }
            return sharedFolderObject;
        }

        /// <summary>
        /// The method returns ManagementObject object for the user folder with given name
        /// </summary>
        /// <param name="domain">string containing domain name of user </param>
        /// <param name="alias">string containing the user's network name </param>
        /// <returns>Object of type ManagementObject for the user folder.</returns>

        private static ManagementObject GetUserAccountObject(string domain, string alias)
        {
            ManagementObject userAccountObject = null;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("select * from Win32_Account where Name = '{0}' and Domain='{1}'", alias, domain));
            ManagementObjectCollection resultOfSearch = searcher.Get();
            if (resultOfSearch.Count > 0)
            {
                foreach (ManagementObject userAccount in resultOfSearch)
                {
                    userAccountObject = userAccount;
                    break;
                }
            }
            return userAccountObject;
        }

        /// <summary>
        /// Returns the Security Identifier Sid of the given user
        /// </summary>
        /// <param name="userAccountObject">The user object who's Sid needs to be returned</param>
        /// <returns></returns>

        private static ManagementObject GetAccountSecurityIdentifier(ManagementBaseObject userAccountObject)
        {
            ManagementObject securityIdentfierObject = new ManagementObject(string.Format("Win32_SID.SID='{0}'", (string)userAccountObject.Properties["SID"].Value));
            securityIdentfierObject.Get();
            return securityIdentfierObject;
        }

        /// <summary>
        /// Create a trustee object for the given user
        /// </summary>
        /// <param name="domain">name of domain</param>
        /// <param name="userName">the network name of the user</param>
        /// <param name="securityIdentifierOfUser">Object containing User's sid</param>
        /// <returns></returns>

        private static ManagementObject CreateTrustee(string domain, string userName, ManagementObject securityIdentifierOfUser)
        {
            ManagementObject trusteeObject = new ManagementClass("Win32_Trustee").CreateInstance();
            trusteeObject.Properties["Domain"].Value = domain;
            trusteeObject.Properties["Name"].Value = userName;
            trusteeObject.Properties["SID"].Value = securityIdentifierOfUser.Properties["BinaryRepresentation"].Value;
            trusteeObject.Properties["SidLength"].Value = securityIdentifierOfUser.Properties["SidLength"].Value;
            trusteeObject.Properties["SIDString"].Value = securityIdentifierOfUser.Properties["SID"].Value;
            return trusteeObject;
        }

        /// <summary>
        /// Create an Access Control Entry object for the given user
        /// </summary>
        /// <param name="trustee">The user's trustee object</param>
        /// <param name="deny">boolean to say if user permissions should be assigned or denied</param>
        /// <returns></returns>

        private static ManagementObject CreateAccessControlEntry(ManagementObject trustee, bool deny)
        {
            ManagementObject aceObject = new ManagementClass("Win32_ACE").CreateInstance();

            aceObject.Properties["AccessMask"].Value = 0x1U | 0x2U | 0x4U | 0x8U | 0x10U | 0x20U | 0x40U | 0x80U | 0x100U | 0x10000U | 0x20000U | 0x40000U | 0x80000U | 0x100000U; // all permissions
            aceObject.Properties["AceFlags"].Value = 0x0U; // no flags
            aceObject.Properties["AceType"].Value = deny ? 1U : 0U; // 0 = allow, 1 = deny
            aceObject.Properties["Trustee"].Value = trustee;
            return aceObject;
        }

        #endregion

        #region Common Function

        private static void CheckCustomhoursForProviderOperatory()
        {

            try
            {
                string strGetcustomhour = PullLiveDatabaseBAL.GetLiveRecord("pocustomhour", Utility.Location_ID);
                var clientCustomHour = new RestClient(strGetcustomhour);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                IRestResponse response = clientCustomHour.Execute(request);

                if (response.ErrorMessage != null)
                {
                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                    { }
                    else
                    {
                        Utility.WriteToErrorLogFromAll("[Custom Hours Sync (Adit Server)] : " + response.ErrorMessage);
                    }
                }
                // ObjGoalBase.WriteToErrorLogFile("[Custom Hours Sync Response] : " + response.Content);
                var CustomVar = JsonConvert.DeserializeObject<Pull_CustomHoursBO>(response.Content);

                if (CustomVar.data != null)
                {
                    if (CustomVar.data._id.Equals(Utility.Location_ID))
                    {
                        if (CustomVar.data.is_online_scheduled == "true")
                            Utility.is_scheduledCustomhour = true;

                        //if (CustomVar.data.operatory.Length > 0)
                        //{
                        //    try
                        //    {
                        //        Utility.is_scheduledCustomhourOperatory = true;
                        //        Utility.CustomhourOperatoryIds.Clear();
                        //        for (int i = 0; i < CustomVar.data.operatory.Length; i++)
                        //        {
                        //            Utility.CustomhourOperatoryIds.Add(CustomVar.data.operatory[i].ToString());
                        //        }
                        //    }
                        //    catch (Exception)
                        //    {
                        //        Utility.CustomhourOperatoryIds = null;
                        //    }
                        //}

                        //if (CustomVar.data.provider.Length > 0)
                        //{
                        //    try
                        //    {
                        //        Utility.is_scheduledCustomhourProvider = true;
                        //        Utility.CustomhourProviderIds.Clear();
                        //        for (int i = 0; i < CustomVar.data.provider.Length; i++)
                        //        {
                        //            Utility.CustomhourProviderIds.Add(CustomVar.data.provider[i].ToString());
                        //        }
                        //    }
                        //    catch (Exception)
                        //    {
                        //        Utility.CustomhourProviderIds = null;
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("GetCustomhoursForProviderOperatory : " + ex.Message);
            }
        }
        private bool GetLocationUpdateVersiononAditServer()
        {
            bool isLocVersionUpdate = false;
            try
            {
                string strGetLocUpdateVersion = SystemBAL.GetLocUpdateVersion();

                var clientLocUpdateVer = new RestClient(strGetLocUpdateVersion);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestRequest(Method.GET);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddHeader("apptype", "aditehr");

                IRestResponse response = clientLocUpdateVer.Execute(request);

                if (response.ErrorMessage != null)
                {
                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                    { }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("[Appointment Sync (Adit Server To Local Database)] : " + response.ErrorMessage);
                    }
                }

                var LocUpdateVer = JsonConvert.DeserializeObject<Pull_LocationEHRUPdateForVersionBO>(response.Content);

                if (LocUpdateVer.data.is_auto_update == 1)
                {
                    isLocVersionUpdate = true;
                }
            }
            catch (Exception)
            {
                isLocVersionUpdate = false;
            }
            finally
            {
            }
            return isLocVersionUpdate;
        }

        private void CheckLocationTimeZoneWithSystemTimeZone()
        {
            try
            {
                string strCheckLocationTimeZone = SystemBAL.CheckLocationTimeZoneWithSystemTimeZone();

                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var LocationTimeZone = new RestClient(strCheckLocationTimeZone);
                var strCheckLocation = new RestRequest(Method.POST);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                strCheckLocation.AddHeader("Postman-Token", "1d16df4c-48ba-4644-bc7a-9bcef2a86744");
                strCheckLocation.AddHeader("cache-control", "no-cache");

                LocationTimeZoneBO CheckLoc = new LocationTimeZoneBO
                {
                    location = Utility.Location_ID,
                    created_by = Utility.User_ID
                };
                strCheckLocation.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(CheckLoc.location).ToString());

                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(CheckLoc);
                strCheckLocation.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                IRestResponse responseLocationTimeZone = LocationTimeZone.Execute(strCheckLocation);
                if (responseLocationTimeZone.ErrorMessage != null)
                {
                    return;
                }

                var LocDto = JsonConvert.DeserializeObject<LocationTimeZoneResBO>(responseLocationTimeZone.Content);

                string webTimeZone = Utility.ConvertWebTimeZoneToSystemTimeZone(LocDto.data.name.ToString().ToLower());
                Utility.AditApp_TimeZoneWeb_ID = LocDto.data._id.ToString();

                System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();


                //  bool isDaylight = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now);
                //  if (Utility.SystemCurrentTimeZone().ToString().ToLower() != webTimeZone.ToLower()) // Dilip Timezone Issue pending

                if (TimeZone.CurrentTimeZone.StandardName.ToString().ToLower() != webTimeZone.ToLower())
                {
                    // ObjGoalBase.InformationMsgBox("Timezone Mismatch", "Timezone of Server computer and set in Adit App Location settings should be same.");

                    try
                    {
                        string CurSystemTimeZoneWebForm = Utility.ConvertSystemTimeZoneToWebTimeZone(Utility.SystemCurrentTimeZone().ToString().ToLower());

                        string strUpdateWebTimeZone = SystemBAL.UpdateWebTimeZone();
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        var clientUpdateWebTimeZone = new RestClient(strUpdateWebTimeZone);
                        var requestUpdateWebTimeZone = new RestRequest(Method.POST);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        UpdateTimeZoneServerToWebLocationBO UpdateWebTimeZoneBO = new UpdateTimeZoneServerToWebLocationBO
                        {
                            created_by = Utility.User_ID,
                            alias = CurSystemTimeZoneWebForm,
                            location = Utility.Loc_ID
                        };
                        var javaScriptSerializerUpdateWebTimeZone = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string jsonStringUpdateWebTimeZone = javaScriptSerializerUpdateWebTimeZone.Serialize(UpdateWebTimeZoneBO);
                        requestUpdateWebTimeZone.AddHeader("postman-token", "a51e59f2-8946-424d-e1e0-988e059940d2");
                        requestUpdateWebTimeZone.AddHeader("cache-control", "no-cache");
                        requestUpdateWebTimeZone.AddHeader("content-type", "application/json");
                        requestUpdateWebTimeZone.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                        requestUpdateWebTimeZone.AddParameter("application/json", jsonStringUpdateWebTimeZone, ParameterType.RequestBody);
                        IRestResponse responseUpdateWebTimeZone = clientUpdateWebTimeZone.Execute(requestUpdateWebTimeZone);

                        if (responseUpdateWebTimeZone.ErrorMessage != null)
                        {
                            return;
                        }

                        var NewLocDto = JsonConvert.DeserializeObject<LocationTimeZoneResBO>(responseUpdateWebTimeZone.Content);

                        Utility.AditApp_TimeZoneWeb_ID = NewLocDto.data._id.ToString();
                        Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();

                    }
                    catch (Exception)
                    {
                    }

                }

            }
            catch (Exception)
            {
                //    ObjGoalBase.ErrorMsgBox("Timezone", ex.Message);
            }
        }

        private void SetControlsDesign()
        {
            this.BackColor = WDSColor.FormHeadBackColor;

            using (var stream = File.OpenRead(Application.StartupPath + "\\PozativeIcon.ico"))
            {
                this.Icon = new Icon(stream);
            }
            NotifyIconPozative.Icon = new System.Drawing.Icon(Application.StartupPath + "\\PozativeIcon.ico");

            lblHead.Select();
            btnClose.ForeColor = Color.Black;

            pnlHead.BackColor = WDSColor.FormHeadBackColor;
            lblHead.ForeColor = WDSColor.FormHeadForeColor;
            lblVersion.ForeColor = WDSColor.FormHeadForeColor;
            btnClose.ForeColor = WDSColor.FormHeadForeColor;

            /*
            // lblConnectionStatus.Font = WDSColor.FormControlHeadFont;
            //lblConnectionLog.Font = WDSColor.FormControlHeadFont;
            lblSyncLog.Font = WDSColor.FormControlHeadFont;

            lblEHR.Font = WDSColor.FormControlFont;
            lblAppointment.Font = WDSColor.FormControlFont;
            lblOperatoryEvent.Font = WDSColor.FormControlFont;
            lblProviders.Font = WDSColor.FormControlFont;
            lblSpeciality.Font = WDSColor.FormControlFont;
            lblOperatories.Font = WDSColor.FormControlFont;
            lblApptType.Font = WDSColor.FormControlFont;
            lblPatient.Font = WDSColor.FormControlFont;
            lblRecallType.Font = WDSColor.FormControlFont;
            lblApptStatus.Font = WDSColor.FormControlFont;
            lblHoliday.Font = WDSColor.FormControlFont;

            btnEHR.Font = WDSColor.FormControlFont;
            btnAppt.Font = WDSColor.FormControlFont;
            btnOperatoryEvent.Font = WDSColor.FormControlFont;
            btnProviders.Font = WDSColor.FormControlFont;
            btnSpeciality.Font = WDSColor.FormControlFont;
            btnOperatories.Font = WDSColor.FormControlFont;
            btnApptType.Font = WDSColor.FormControlFont;
            btnPatient.Font = WDSColor.FormControlFont;
            btnRecallType.Font = WDSColor.FormControlFont;
            btnApptStatus.Font = WDSColor.FormControlFont;
            btnHoliday.Font = WDSColor.FormControlFont;

            btnSyncAppt.Font = WDSColor.FormControlFont;
            btnSyncOperatoryEvent.Font = WDSColor.FormControlFont;
            btnSyncProviders.Font = WDSColor.FormControlFont;
            btnSyncSpeciality.Font = WDSColor.FormControlFont;
            btnSyncOperatories.Font = WDSColor.FormControlFont;
            btnSyncApptType.Font = WDSColor.FormControlFont;
            btnSyncPatient.Font = WDSColor.FormControlFont;
            btnSyncRecallType.Font = WDSColor.FormControlFont;
            btnSyncApptStatus.Font = WDSColor.FormControlFont;
            btnSyncHoliday.Font = WDSColor.FormControlFont;

            btnPozativeAppt.Font = WDSColor.FormControlFont;
            btnPozativeSyncAppt.Font = WDSColor.FormControlFont;
            */

            //grdAppointment.DefaultCellStyle.SelectionBackColor = Color.White;
            //grdAppointment.DefaultCellStyle.SelectionForeColor = Color.Black;
            //grdAppointment.DefaultCellStyle.ForeColor = Color.Gray;

            //this.grdAppointment.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            //this.grdAppointment.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            //this.grdAppointment.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            //this.grdAppointment.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            grdPozativeAppointment.DefaultCellStyle.SelectionBackColor = Color.White;
            grdPozativeAppointment.DefaultCellStyle.SelectionForeColor = Color.Black;
            grdPozativeAppointment.DefaultCellStyle.ForeColor = Color.Gray;

            this.grdPozativeAppointment.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            this.grdPozativeAppointment.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            this.grdPozativeAppointment.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            this.grdPozativeAppointment.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            TTPView.SetToolTip(btnClose, "Click to Minimize Application in System Tray");
            TTPView.SetToolTip(btnConfig, "Click to Module Sync Time Configuration");
            TTPView.SetToolTip(btnAppRestart, "Click to Restart Application");

            TTPView.InitialDelay = 200;
        }

        private void AdjustColumnOrder()
        {
            //    grdAppointment.Columns["Appt_DateTime"].DisplayIndex = 0;
            //    grdAppointment.Columns["Patient_Name"].DisplayIndex = 1;
            //    grdAppointment.Columns["Mobile_Contact"].DisplayIndex = 2;
            //    grdAppointment.Columns["Provider_Name"].DisplayIndex = 3;
            //    grdAppointment.Columns["Operatory_Name"].DisplayIndex = 4;
            //    grdAppointment.Columns["ApptType"].DisplayIndex = 5;
            //    grdAppointment.Columns["Appointment_Status"].DisplayIndex = 6;
            //    grdAppointment.Columns["ViewHere"].DisplayIndex = 7;

            //    grdPozativeAppointment.Columns["Poz_Appt_DateTime"].DisplayIndex = 0;
            //    grdPozativeAppointment.Columns["Poz_Patient_Name"].DisplayIndex = 1;
            //    grdPozativeAppointment.Columns["Poz_Mobile_Contact"].DisplayIndex = 2;
            //    grdPozativeAppointment.Columns["Poz_Email"].DisplayIndex = 3;
            //    grdPozativeAppointment.Columns["Poz_Operatory_Name"].DisplayIndex = 4;
        }

        private void RemoveTempSyncLogFile()
        {
            string SyncLogFileName = Application.StartupPath + "\\" + "SyncLog.txt";

            if (!File.Exists(SyncLogFileName))
            {
                File.Delete(SyncLogFileName);
            }
            File.Create(SyncLogFileName).Dispose();
        }

        private bool CheckSystemAndApplicaitonConnection(bool fromTimer = false)
        {

        ApplicaitonConnection:

            DataTable dtInstallApplicationDetail = new DataTable();
            try
            {
                string processorID = ObjGoalBase.getUniqueID("C");
                Utility.System_processorID = processorID;
                // processorID = "6EBFEBF70CDAF20FBFF";
                // Utility.System_processorID = "6EBFEBF70CDAF20FBFF";
                dtInstallApplicationDetail = SystemBAL.GetInstallApplicationDetail(processorID);
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Pozative_Load]" + ex.Message);
                if (ex.Message.Contains("System.OutOfMemoryException") || ex.Message.Contains("Not enough memory resources are available to complete this operation"))
                {
                    //string[] t = Directory.GetFiles(Environment.CurrentDirectory, "Pozative.sdf");
                    //Array.ForEach(t, File.Delete);
                    //Utility.Get_LocalDbFromWeb();
                    //Utility.UpdateLocalDB_From_BackupDB();
                    Utility.ResolvedOutofMemeoryException();
                    Utility.RestartApp();
                }
                if (SystemBAL.RecoveryDatabase())
                {
                    ObjGoalBase.WriteToSyncLogFile("CheckSystemAndApplicaitonConnection_Local DB is not corrupted");
                    goto ApplicaitonConnection;
                }


            }


            if (dtInstallApplicationDetail != null && dtInstallApplicationDetail.Rows.Count > 0)
            {
                //Utility.LocationTimeZone = dtInstallApplicationDetail.Rows[0]["timezone"].ToString();
                Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();
                Utility.Adit_User_Email_Id = dtInstallApplicationDetail.Rows[0]["Adit_User_Email_ID"].ToString();
                Utility.Adit_User_Email_Password = dtInstallApplicationDetail.Rows[0]["Adit_User_Email_Password"].ToString();
                Utility.Organization_ID = dtInstallApplicationDetail.Rows[0]["Organization_ID"].ToString();
                Utility.Organization_Name = dtInstallApplicationDetail.Rows[0]["Organization_Name"].ToString();
                Utility.Location_ID = dtInstallApplicationDetail.Rows[0]["Location_ID"].ToString();
                Utility.Location_Name = dtInstallApplicationDetail.Rows[0]["Location_Name"].ToString();
                Utility.Application_Version = Convert.ToString(dtInstallApplicationDetail.Rows[0]["Application_Version"].ToString());
                Utility.System_processorID = Convert.ToString(dtInstallApplicationDetail.Rows[0]["System_processorID"].ToString());
                Utility.Application_Name = dtInstallApplicationDetail.Rows[0]["Application_Name"].ToString();
                Utility.EHRHostname = dtInstallApplicationDetail.Rows[0]["Hostname"].ToString().ToLower();
                Utility.EHRIntegrationKey = dtInstallApplicationDetail.Rows[0]["IntegrationKey"].ToString();
                Utility.EHRUserId = dtInstallApplicationDetail.Rows[0]["UserId"].ToString();
                Utility.EHRPassword = dtInstallApplicationDetail.Rows[0]["Password"].ToString();
                Utility.EHRDatabase = dtInstallApplicationDetail.Rows[0]["database"].ToString();
                Utility.EHRPort = dtInstallApplicationDetail.Rows[0]["Port"].ToString();
                Utility.DBConnString = dtInstallApplicationDetail.Rows[0]["DBConnString"].ToString();


                Utility.EHRDocPath = dtInstallApplicationDetail.Rows[0]["Document_Path"].ToString();
                Utility.DontAskPasswordOnSaveSetting = Convert.ToBoolean(dtInstallApplicationDetail.Rows[0]["DontAskPasswordOnSaveSetting"].ToString());
                Utility.NotAllowToChangeSystemDateFormat = Convert.ToBoolean(dtInstallApplicationDetail.Rows[0]["NotAllowToChangeSystemDateFormat"].ToString());
                Utility.EHR_VersionNumber = Utility.EHR_VersionNumber == "" ? dtInstallApplicationDetail.Rows[0]["EHR_VersionNumber"].ToString() : Utility.EHR_VersionNumber;

                //Change system date format
                if (!Utility.NotAllowToChangeSystemDateFormat)
                {
                    setSystemDateTimeAndSharedFolder();
                }

                Utility.DtLocationList = SystemBAL.GetLocationDetail();
                if (!Utility.DtLocationList.Columns.Contains("AditLocationSyncEnable"))
                {
                    Utility.DtLocationList.Columns.Add("AditLocationSyncEnable", typeof(bool));
                    Utility.DtLocationList.Columns["AditLocationSyncEnable"].DefaultValue = true;
                }
                Utility.DtInstallServiceList = SystemBAL.GetInstallServiceDetail();
                Utility.DtOrganizationList = SystemBAL.GetOrganizationDetail();

                if (dtInstallApplicationDetail.Columns.Contains("EHR_Sub_Version"))
                {
                    Utility.EHR_Sub_Version = dtInstallApplicationDetail.Rows[0]["EHR_Sub_Version"].ToString();
                }
                if (Utility.DBConnString == "" || Utility.DBConnString == string.Empty)
                {
                    GetConnectionString();
                }
                if (Utility.EHRDocPath == "" || Utility.EHRDocPath == string.Empty)
                {
                    GetEHRDocPath();
                }
                Utility.IsApplicationIdleTimeSet = Convert.ToBoolean(dtInstallApplicationDetail.Rows[0]["ApplicationIdleTimeOff"].ToString());
                if (Utility.IsApplicationIdleTimeSet)
                {
                    Utility.AppIdleStartTime = Convert.ToDateTime(dtInstallApplicationDetail.Rows[0]["AppIdleStartTime"].ToString());
                    Utility.AppIdleStopTime = Convert.ToDateTime(dtInstallApplicationDetail.Rows[0]["AppIdleStopTime"].ToString());

                    //tmrApplicationIdleTime.Enabled = true;
                    //tmrApplicationIdleTime.Interval = 1000;
                    //tmrApplicationIdleTime.Tick += tmrApplicationIdleTime_Tick;
                    //tmrApplicationIdleTime.Start();
                }

                Utility.ApplicationInstalledTime = Convert.ToDateTime((dtInstallApplicationDetail.Rows[0]["ApplicationInstalledTime"].ToString()));

                Utility.AditSync = Convert.ToBoolean(dtInstallApplicationDetail.Rows[0]["AditSync"].ToString());
                // Utility.ApptAutoBook = Convert.ToBoolean(dtInstallApplicationDetail.Rows[0]["ApptAutoBook"].ToString());
                Utility.PozativeSync = Convert.ToBoolean(dtInstallApplicationDetail.Rows[0]["PozativeSync"].ToString());
                Utility.PozativeEmail = dtInstallApplicationDetail.Rows[0]["PozativeEmail"].ToString();
                Utility.PozativeLocationID = dtInstallApplicationDetail.Rows[0]["PozativeLocationID"].ToString();
                Utility.PozativeLocationName = dtInstallApplicationDetail.Rows[0]["PozativeLocationName"].ToString();

                DataTable dtInstallApplicationLocationDetail = SystemBAL.GetInstallApplicationLocationDetail();
                if (dtInstallApplicationLocationDetail != null && dtInstallApplicationLocationDetail.Rows.Count > 0)
                {
                    Utility.AditLocationName = dtInstallApplicationLocationDetail.Rows[0]["name"].ToString();
                    Utility.User_ID = dtInstallApplicationLocationDetail.Rows[0]["User_ID"].ToString();
                    Utility.Loc_ID = dtInstallApplicationLocationDetail.Rows[0]["Loc_ID"].ToString();
                }


                if (Utility.Application_Name.ToLower() == "Eaglesoft".ToLower())
                {
                    #region Eaglesoft
                    if (Utility.Application_Version.ToLower() == "21.20".ToLower())
                    {
                        Utility.DBConnString = Utility.DecryptString(Utility.DBConnString);
                    }
                    else
                    {
                        Utility.DBConnString = dtInstallApplicationDetail.Rows[0]["DBConnString"].ToString();
                    }
                    lblHoliday.Visible = true;
                    btnHoliday.Visible = true;
                    btnSyncHoliday.Visible = true;
                    Utility.Application_ID = 1;
                    lblOperatories.Text = "Chairs";

                    lblOperatoryEvent.Text = "Chair Events";
                    try
                    {
                        if (SynchEaglesoftDAL.GetEaglesoftConnection(Utility.DBConnString))
                        {
                            lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";

                            #region Applicatino Reconnet
                            if (fromTimer && btnEHR.Text.ToString().ToUpper() == "CONNECTING")
                            {
                                //SetToolTipOnControls();
                                //CheckVisibleControl();

                                //AppVersionAutoUpdateserverData();
                                //if (Utility.AditSync)
                                //{
                                //    SyncWithAditAppServer();
                                //    UpdateApplicationVersionOnLiveDatabase();
                                //  //  CallSynchEaglesoftToLocal();
                                //    lblHead.Text = "Adit Sync Server";
                                //}

                                Application.Exit();
                                //Utility.RestartApp();
                            }
                            #endregion

                            SetBtnDesignForConnectionStatus(btnEHR,Connectedtxt);
                            Utility.ISEagelsoftConnected = true;
                            IsAppConnection = true;
                        }
                        else
                        {
                            Utility.DBConnString = "";
                            GetConnectionString();
                            if (IsAppConnection)
                            {
                                lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                                SetBtnDesignForConnectionStatus(btnEHR,Connectedtxt);
                                Utility.ISEagelsoftConnected = true;
                                // IsAppConnection = true;
                            }
                            else
                            {
                                SetBtnDesignForConnectionStatus(btnEHR, DisConnectedtxt, false);
                                Utility.ISEagelsoftConnected = false;
                                IsAppConnection = false;
                            }
                        }
                    }
                    catch
                    {
                        Utility.DBConnString = "";
                        SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
                        Application.Exit();
                    }
                    //    frmConfiguration fECon = new frmConfiguration("Update");
                    //    var result = fECon.ShowDialog();
                    //    if (result == DialogResult.OK)
                    //    {
                    //        goto ApplicaitonConnection;
                    //    }
                    //    else
                    //    {
                    //        ObjGoalBase.WriteToSyncLogFile("[CheckSystemAndApplicaitonConnection] Eaglesoft EHR connection is not successfully");
                    //        System.Environment.Exit(1);
                    //    }
                    //}

                    #region Code For AditPay Deposit Report #shruti
                    try
                    {

                        bool UpdateAditPayFlag = false;
                        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UpdateAditPay");
                        if (key != null)
                        {
                            //UpdateAditPayFlag = bool.Parse(key.GetValue("UpdateAditPay").ToString());
                            object updateAditPayValue = key.GetValue("UpdateAditPay");
                            if (updateAditPayValue != null)
                            {
                                UpdateAditPayFlag = bool.Parse(updateAditPayValue.ToString());
                            }
                        }
                        else
                        {
                            RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UpdateAditPay");
                            key1.SetValue("UpdateAditPay", true);
                            key1.Close();
                        }

                        RegistryKey keyy = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UpdateAditPay");
                        if (keyy != null)
                        {
                            //UpdateAditPayFlag = bool.Parse(key.GetValue("UpdateAditPay").ToString());
                            object updateAditPayValue = keyy.GetValue("UpdateAditPay");
                            if (updateAditPayValue != null)
                            {
                                UpdateAditPayFlag = bool.Parse(updateAditPayValue.ToString());
                            }
                            try
                            {
                                if (UpdateAditPayFlag == true)
                                {
                                    SynchEaglesoftDAL.UpdateIncludeAditDepositsFor_Reports();
                                }
                                keyy.Close();
                                RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\UpdateAditPay", true);
                                rkey.SetValue("UpdateAditPay", false);
                            }
                            catch (Exception ex)
                            {
                                Utility.WriteToErrorLogFromAll("Error In Deposit Report Update Query " + ex.Message.ToString());
                                // throw;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToErrorLogFromAll("Error in Payment Deposit report code " + ex.Message.ToString());
                    }
                    #endregion
                    #endregion
                }
                else if (Utility.Application_Name.ToLower() == "Open Dental".ToLower() || (Utility.Application_Name.ToLower() == "OpenDental Cloud".ToLower()))
                {
                    #region OpenDental

                    Utility.Application_ID = 2;

                    lblHoliday.Visible = true;
                    btnHoliday.Visible = true;
                    btnSyncHoliday.Visible = true;

                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";


                    if (Utility.Application_Version.ToLower() == "15.4".ToLower() || Utility.Application_Version.ToLower() == "17.2+".ToLower())
                    {
                        IsAppConnection = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("EHR Version : Pozative Service integration with this EHR version " + Utility.Application_Version.ToLower() + " is underdevelopment.");
                        IsAppConnection = false;
                    }

                    if (IsAppConnection)
                    {
                        SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);

                        //ObjGoalBase.WriteToSyncLogFile(Utility.Application_Name + " Connected");
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnEHR, DisConnectedtxt, false);
                    }
                    #endregion
                }
                else if (Utility.Application_Name.ToLower() == "PracticeWeb".ToLower())
                {
                    #region Practice Web

                    Utility.Application_ID = 10;

                    lblHoliday.Visible = true;
                    btnHoliday.Visible = true;
                    btnSyncHoliday.Visible = true;

                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";


                    if (Utility.Application_Version.ToLower() == "21.1".ToLower())
                    {
                        IsAppConnection = SystemBAL.GetEHRPracticeWebConnection(Utility.DBConnString);
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("EHR Version : Pozative Service integration with this EHR version " + Utility.Application_Version.ToLower() + " is underdevelopment.");
                        IsAppConnection = false;
                    }

                    if (IsAppConnection)
                    {
                        SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                        //ObjGoalBase.WriteToSyncLogFile(Utility.Application_Name + " Connected");
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnEHR, DisConnectedtxt, false);
                    }
                    #endregion
                }

                else if (Utility.Application_Name.ToLower() == "Dentrix".ToLower())
                {
                    #region Dentrix

                    lblHoliday.Visible = true;
                    btnHoliday.Visible = true;
                    btnSyncHoliday.Visible = true;
                    Utility.Application_ID = 3;
                    //  lblEHR.Text = Utility.Application_Name + " Status";
                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                    try
                    {
                        IsAppConnection = SystemBAL.GetEHRDentrixConnection();

                        //if ((Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                        //|| (Utility.Application_Version.ToLower() == "DTX G5.1".ToLower())
                        //|| (Utility.Application_Version.ToLower() == "DTX G5.2".ToLower())
                        //|| (Utility.Application_Version.ToLower() == "DTX G6".ToLower())
                        //|| (Utility.Application_Version.ToLower() == "DTX G6.1".ToLower()))
                        //{

                        //}
                        //else
                        //{


                        //    string exePath = string.Empty;
                        //    StringBuilder path = new StringBuilder(512);
                        //    string connectionString = string.Empty;
                        //    if (CommonFunction.GetDentrixG62ExePath(path) == SUCCESS)
                        //    {
                        //        exePath = path.ToString();
                        //        if (((int)(DENTRIXAPI_GetDentrixVersion() * 100) >= 1620) == true)
                        //        {
                        //            if (CommonFunction.GetDentrixG62ConnectionString(Utility.EHRUserId, Utility.EHRPassword))
                        //            {
                        //                IsAppConnection = true;
                        //            }
                        //            else
                        //            {
                        //                IsAppConnection = false;
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                        //    }
                        //}
                        if (IsAppConnection)
                        {
                            SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                            //ObjGoalBase.WriteToSyncLogFile(Utility.Application_Name + " Connected");
                            #region If EHR_Sub_Version is Blank then Get From Registery or Database

                            if (Utility.EHR_Sub_Version == string.Empty)
                            {
                                try
                                {
                                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Dentrix Dental Systems, Inc.\\Dentrix\\General"))
                                    {
                                        if (key != null)
                                        {
                                            Object o = key.GetValue("Installed Version");
                                            if (o != null)
                                            {
                                                Utility.EHR_Sub_Version = o.ToString();
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        DataTable dtEHRVersion = SynchDentrixBAL.GetDentrixApplicationVersion();
                                        if (dtEHRVersion != null && dtEHRVersion.Rows.Count > 0)
                                        {
                                            Utility.EHR_Sub_Version = dtEHRVersion.Rows[0][0].ToString();
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                                SystemBAL.UpdateEHRVersion();
                            }

                            #endregion
                            //lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ")[" + Utility.EHR_Sub_Version.ToString() + "]" + " Status";
                        }
                        else
                        {
                            Utility.DBConnString = "";
                            GetConnectionString();
                            if (IsAppConnection)
                            {
                                lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                                SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                                IsAppConnection = true;
                            }
                            else
                            {
                                SetBtnDesignForConnectionStatus(btnEHR, DisConnectedtxt, false);
                                IsAppConnection = false;
                            }

                        }
                        //DentrixDocPassword
                        try
                        {
                            Utility.DentrixDocConnString = dtInstallApplicationDetail.Rows[0]["DentrixPDFConstring"].ToString();
                            Utility.DentrixDocPWD = dtInstallApplicationDetail.Rows[0]["DentrixPDFPassword"].ToString();
                        }
                        catch
                        {
                            Utility.DentrixDocConnString = "";
                            Utility.DentrixDocPWD = "";
                        }
                        if (Utility.DentrixDocPWD == null || Utility.DentrixDocPWD == "")
                        {
                            GoalBase.GetConnectionStringforDoc(false);
                        }
                        else
                        {
                            if (Utility.DentrixDocConnString != null && Utility.DentrixDocConnString != "")
                            {
                                Utility.SetRegistryObject(Registry.LocalMachine, "docconnection", Utility.DentrixDocConnString, null, RegistryValueKind.String);
                            }
                            if (Utility.DentrixDocPWD != null && Utility.DentrixDocPWD != "")
                            {
                                Utility.SetRegistryObject(Registry.LocalMachine, "docpassword", Utility.DentrixDocPWD, null, RegistryValueKind.String);
                            }
                        }

                    }
                    catch
                    {
                        Utility.DBConnString = "";
                        SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
                        Application.Exit();
                    }
                    #endregion
                }
                else if (Utility.Application_Name.ToLower() == "SoftDent".ToLower())
                {
                    Utility.Application_ID = 4;

                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";

                    lblRecallType.Visible = false;
                    lblApptType.Visible = false;
                    btnRecallType.Visible = false;
                    btnSyncRecallType.Visible = false;
                    btnApptType.Visible = false;
                    btnSyncApptType.Visible = false;

                    SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                    IsAppConnection = true;

                }
                else if (Utility.Application_Name.ToLower() == "ClearDent".ToLower())
                {
                    Utility.Application_ID = 5;
                    lblOperatories.Text = "Chairs";

                    lblOperatoryEvent.Text = "Chair Events";
                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                    SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                    IsAppConnection = true;

                    // ClearDentDocPath();


                }
                else if (Utility.Application_Name.ToLower() == "Tracker".ToLower())
                {
                    lblHoliday.Visible = true;
                    btnHoliday.Visible = true;
                    btnSyncHoliday.Visible = true;
                    Utility.Application_ID = 6;
                    lblOperatories.Text = "Chairs";

                    lblOperatoryEvent.Text = "Chair Events";
                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                    SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                    IsAppConnection = true;

                }
                else if (Utility.Application_Name.ToLower() == "PracticeWork".ToLower())
                {
                    lblHoliday.Visible = true;
                    btnHoliday.Visible = true;
                    btnSyncHoliday.Visible = true;
                    Utility.Application_ID = 7;
                    lblOperatories.Text = "Chairs";

                    lblOperatoryEvent.Text = "Chair Events";
                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                    SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                    IsAppConnection = true;

                }
                else if (Utility.Application_Name.ToLower() == "Easy Dental".ToLower())
                {
                    lblHoliday.Visible = false;
                    btnHoliday.Visible = false;
                    btnSyncHoliday.Visible = false;
                    Utility.Application_ID = 8;
                    lblOperatories.Text = "Operatory";

                    lblOperatoryEvent.Text = "Operatory Events";
                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                    SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                    IsAppConnection = true;
                    //txtConnectionLog.Text = GoalBase.ConnectionLog;  
                }
                else if (Utility.Application_Name.ToLower() == "AbelDent".ToLower())
                {
                    lblHoliday.Visible = false;
                    btnHoliday.Visible = false;
                    btnSyncHoliday.Visible = false;
                    Utility.Application_ID = 11;
                    lblOperatories.Text = "Chairs";

                    lblOperatoryEvent.Text = "Chair Events";
                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                    SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                    IsAppConnection = true;

                }
                else if (Utility.Application_Name.ToLower() == "CrystalPM".ToLower())
                {
                    #region CrystalPM
                    Utility.Application_ID = 12;

                    lblHoliday.Visible = false;
                    btnHoliday.Visible = false;
                    btnSyncHoliday.Visible = false;
                    lblOperatories.Visible = false;
                    btnOperatories.Visible = false;
                    btnSyncOperatories.Visible = false;
                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";

                    if (Utility.Application_Version.ToLower() == "6.1.9".ToLower())
                    {
                        AditCrystalPM.BAL.MySqlDB.ConnString = Utility.DBConnString;
                        AditCrystalPM.BAL.MySqlDB mySql = new AditCrystalPM.BAL.MySqlDB();
                        IsAppConnection = mySql.CheckConnection(Utility.DBConnString);
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("EHR Version : Pozative Service integration with this EHR version " + Utility.Application_Version.ToLower() + " is underdevelopment.");
                        IsAppConnection = false;
                    }
                    if (IsAppConnection)
                    {
                        SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);

                        //ObjGoalBase.WriteToSyncLogFile(Utility.Application_Name + " Connected");
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnEHR, DisConnectedtxt, false);
                    }
                    #endregion
                }
                //  fncAditLocationSyncEnable();
                else if (Utility.Application_Name.ToLower() == "OfficeMate".ToLower())
                {
                    try
                    {
                        #region OfficeMate

                        Utility.Application_ID = 13;

                        lblHoliday.Visible = false;
                        btnHoliday.Visible = false;
                        btnSyncHoliday.Visible = false;

                        lblOperatories.Visible = false;
                        btnOperatories.Visible = false;
                        btnSyncOperatories.Visible = false;

                        lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";


                        if (Utility.Application_Version.ToLower() == "12.0.2".ToLower())
                        {
                            IsAppConnection = AditOfficeMate.BAL.SqlDB.Instance.CheckConnection(Utility.DBConnString);
                        }
                        else
                        {
                            ObjGoalBase.WriteToErrorLogFile("EHR Version : Pozative Service integration with this EHR version " + Utility.Application_Version.ToLower() + " is underdevelopment.");
                            IsAppConnection = false;
                        }
                        if (IsAppConnection)
                        {
                            SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                        }
                        else
                        {
                            SetBtnDesignForConnectionStatus(btnEHR, DisConnectedtxt, false);
                        }
                        #endregion
                    }
                    catch (Exception)
                    {

                    }                   
                }
                // fncAditLocationSyncEnable();
                tmrConnectionLog.Enabled = true;
                tmrConnectionLog.Interval = 1000 * GoalBase.intervalCheckConnection;
                tmrConnectionLog.Start();

                LoadAppointmentRecord();
                //              

            }

            else
            {
                frmConfiguration_Auto fECon = new frmConfiguration_Auto("Insert");
                var result = fECon.ShowDialog();
                if (result == DialogResult.OK)
                {
                    goto ApplicaitonConnection;
                }
                else
                {
                    ObjGoalBase.WriteToSyncLogFile("[CheckSystemAndApplicaitonConnection] connection is not successfully");
                    foreach (Process p in Process.GetProcesses())
                    {
                        string fileName = p.ProcessName;

                        if (string.Compare("Pozative", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                        {
                            p.Kill();
                            continue;
                        }
                        if (string.Compare("PracticeAnalytics".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                        {
                            p.Kill();
                            continue;
                        }
                        if (string.Compare("DentrixDocument".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                        {
                            p.Kill();
                            continue;
                        }
                        if (string.Compare("DTX_Helper".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                        {
                            p.Kill();
                            continue;
                        }

                    }
                    System.Environment.Exit(1);
                }
            }
            return IsAppConnection;
        }

        private void TablesButtonStatus()
        {

            #region Sync EHR Database

            SetBtnDesignForConnectionStatus(btnEHR, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnAppt, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnOperatoryEvent, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnProviders, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSpeciality, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnOperatories, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnApptType, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnPatient, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnRecallType, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnApptStatus, Idletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnHoliday, Idletxt, isIdle: true);
            btnPozativeAppt.BackColor = IdleColor;
            btnPozativeAppt.Text = Idletxt;
            #endregion

            #region Sync WebDatabase

            SetBtnDesignForConnectionStatus(btnSyncAppt, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncOperatoryEvent, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncProviders, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncSpeciality, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncOperatories, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncApptType, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncPatient, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncRecallType, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncApptStatus, SyncIdletxt, isIdle: true);
            SetBtnDesignForConnectionStatus(btnSyncHoliday, SyncIdletxt, isIdle: true);
            btnPozativeSyncAppt.BackColor = IdleColor;
            btnPozativeSyncAppt.Text = SyncIdletxt;
            #endregion

        }

        private void CheckTableConnection()
        {
            DataTable dtLastSyncTime = SystemBAL.GetLastSyncTablesDatetime();
            DateTime LastSyncDate;
            DateTime CurDatetime = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(Utility.Datetimesetting().ToString("MM/dd/yyyy hh:mm tt")) : Convert.ToDateTime(Utility.Datetimesetting());//.ToString("MM/dd/yyyy hh:mm tt"));

            if (dtLastSyncTime.Rows.Count > 0)
            {
                #region Sync EHR Database

                #region Appointment

                DataRow[] ApptRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Appointment'");
                if (ApptRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(ApptRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_Appointment))
                    {
                        SetBtnDesignForConnectionStatus(btnAppt, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnAppt, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnAppt, Idletxt, isIdle: true);
                }

                #endregion

                #region Operatory Event

                DataRow[] OERow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'OperatoryEvent'");
                if (OERow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(OERow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_OperatoryEvent))
                    {
                        SetBtnDesignForConnectionStatus(btnOperatoryEvent, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnOperatoryEvent, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnOperatoryEvent, Idletxt, isIdle: true);
                }

                #endregion


                #region Provider

                DataRow[] ProRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Provider'");
                if (ProRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(ProRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_Provider))
                    {
                        SetBtnDesignForConnectionStatus(btnProviders, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnProviders, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnProviders, Idletxt, isIdle: true);
                }

                #endregion

                #region Speciality

                DataRow[] SpeRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Speciality'");
                if (SpeRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SpeRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_Speciality))
                    {
                        SetBtnDesignForConnectionStatus(btnSpeciality, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSpeciality, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSpeciality, Idletxt, isIdle: true);
                }

                #endregion

                #region Operatory

                DataRow[] OptRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Operatory'");
                if (OptRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(OptRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_Operatory))
                    {
                        SetBtnDesignForConnectionStatus(btnOperatories, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnOperatories, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnOperatories, Idletxt, isIdle: true);
                }

                #endregion

                #region ApptType

                DataRow[] ApptTypeRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'ApptType'");
                if (ApptTypeRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(ApptTypeRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_ApptType))
                    {
                        SetBtnDesignForConnectionStatus(btnApptType, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnApptType, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnApptType, Idletxt, isIdle: true);
                }

                #endregion

                #region Patient

                DataRow[] PatientRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Patient'");
                if (PatientRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(PatientRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_Patient))
                    {
                        SetBtnDesignForConnectionStatus(btnPatient, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnPatient, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnPatient, Idletxt, isIdle: true);
                }


                #endregion

                #region Recall Type

                DataRow[] RecallTypeRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'RecallType'");
                if (RecallTypeRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(RecallTypeRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_RecallType))
                    {
                        SetBtnDesignForConnectionStatus(btnRecallType, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnRecallType, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnRecallType, Idletxt, isIdle: true);
                }


                #endregion

                #region Appt Status

                DataRow[] ApptStatusRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'ApptStatus'");
                if (ApptStatusRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(ApptStatusRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_ApptStatus))
                    {
                        SetBtnDesignForConnectionStatus(btnApptStatus, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnApptStatus, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnApptStatus, Idletxt, isIdle: true);
                }


                #endregion

                #region Holiday

                DataRow[] HolidayRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Holiday'");
                if (HolidayRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(HolidayRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_Holiday))
                    {
                        SetBtnDesignForConnectionStatus(btnHoliday, Connectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnHoliday, DisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnHoliday, Idletxt, isIdle: true);
                }


                #endregion

                #region PozativeAppointment

                DataRow[] PozativeApptRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'PozativeAppointment'");
                if (PozativeApptRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(PozativeApptRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncDatetime(LastSyncDate, GoalBase.intervalEHRSynch_PozativeAppointment))
                    {
                        btnPozativeAppt.BackColor = ConnectedColor;
                        btnPozativeAppt.Text = Connectedtxt;
                    }
                    else
                    {
                        btnPozativeAppt.BackColor = DisConnectedColor;
                        btnPozativeAppt.Text = DisConnectedtxt;
                    }
                }
                else
                {
                    btnPozativeAppt.BackColor = IdleColor;
                    btnPozativeAppt.Text = Idletxt;
                }

                #endregion

                #endregion

                #region Sync Web Database

                #region Appointment_Push

                DataRow[] SyncApptRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Appointment_Push'");
                if (SyncApptRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncApptRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_Appointment))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncAppt, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncAppt, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncAppt, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region OperatoryEvent_Push

                DataRow[] SyncOERow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'OperatoryEvent_Push'");
                if (SyncOERow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncOERow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_OperatoryEvent))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncOperatoryEvent, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncOperatoryEvent, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncOperatoryEvent, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region Provider_Push

                DataRow[] SyncProRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Provider_Push'");
                if (SyncProRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncProRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_Provider))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncProviders, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncProviders, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncProviders, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region Speciality_Push

                DataRow[] SyncSpeRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Speciality_Push'");
                if (SyncSpeRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncSpeRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_Speciality))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncSpeciality, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncSpeciality, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncSpeciality, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region Operatory_Push

                DataRow[] SyncOptRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Operatory_Push'");
                if (SyncOptRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncOptRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_Operatory))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncOperatories, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncOperatories, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncOperatories, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region ApptType_Push

                DataRow[] SyncApptTypeRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'ApptType_Push'");
                if (SyncApptTypeRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncApptTypeRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_ApptType))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncApptType, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncApptType, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncApptType, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region Patient_Push

                DataRow[] SyncPatientRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Patient_Push'");
                if (SyncPatientRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncPatientRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_Patient))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncPatient, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncPatient, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncPatient, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region RecallType_Push

                DataRow[] SyncRecallTypeRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'RecallType_Push'");
                if (SyncRecallTypeRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncRecallTypeRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_RecallType))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncRecallType, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncRecallType, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncRecallType, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region ApptStatus

                DataRow[] SyncApptStatusRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'ApptStatus_Push'");
                if (SyncApptStatusRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncApptStatusRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_ApptStatus))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncApptStatus, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncApptStatus, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncApptStatus, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region Holiday

                DataRow[] SyncHolidayRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'Holiday_Push'");
                if (SyncHolidayRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(SyncHolidayRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_Holiday))
                    {
                        SetBtnDesignForConnectionStatus(btnSyncHoliday, SyncConnectedtxt);
                    }
                    else
                    {
                        SetBtnDesignForConnectionStatus(btnSyncHoliday, SyncDisConnectedtxt, false);
                    }
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnSyncHoliday, SyncIdletxt, isIdle: true);
                }

                #endregion

                #region PozativeAppointment_Push

                DataRow[] PozativeSyncApptRow = dtLastSyncTime.Copy().Select("Sync_Table_Name = 'PozativeAppointment_Push'");
                if (PozativeSyncApptRow.Length > 0)
                {
                    LastSyncDate = Convert.ToDateTime(PozativeSyncApptRow[0]["Last_Sync_Date"].ToString());
                    if (CheckTableLastSyncPozativeWabDatabaseDatetime(LastSyncDate, GoalBase.intervalWebSynch_Push_Appointment))
                    {
                        btnPozativeSyncAppt.BackColor = ConnectedColor;
                        btnPozativeSyncAppt.Text = SyncConnectedtxt;
                    }
                    else
                    {
                        btnPozativeSyncAppt.BackColor = DisConnectedColor;
                        btnPozativeSyncAppt.Text = SyncDisConnectedtxt;
                    }
                }
                else
                {
                    btnPozativeSyncAppt.BackColor = IdleColor;
                    btnPozativeSyncAppt.Text = SyncIdletxt;
                }

                #endregion

                #endregion
            }
        }

        private void LoadAppointmentRecord()
        {

            if (Utility.AditSync)
            {
                DataTable dtGetAppointment = new DataTable();
                dtGetAppointment = SystemBAL.GetLocalAppointment();
                // grdAppointment.DataSource = dtGetAppointment;
            }
            else
            {
                if (Utility.PozativeSync)
                {
                    DataTable dtGetPozativeAppointment = new DataTable();
                    dtGetPozativeAppointment = SystemBAL.GetPozativeAppointment();
                    grdPozativeAppointment.DataSource = dtGetPozativeAppointment;
                }
            }
            AdjustColumnOrder();
        }

        private bool CheckTableLastSyncDatetime(DateTime LastSyncDate, int IntervalTime)
        {
            bool isConnected = false;
            DateTime CurDatetime = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(Utility.Datetimesetting().ToString("MM/dd/yyyy hh:mm tt")) : Convert.ToDateTime(Utility.Datetimesetting());//.ToString("MM/dd/yyyy hh:mm tt"));
            DateTime TableLastSyncDate = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(LastSyncDate.ToString("MM/dd/yyyy hh:mm tt")) : Convert.ToDateTime(LastSyncDate);//.ToString("MM/dd/yyyy hh:mm tt"));
            TimeSpan diffSpan = CurDatetime - TableLastSyncDate;
            if (diffSpan.TotalSeconds > (IntervalTime + 60))
            {
                isConnected = false;
            }
            else
            {
                isConnected = true;
            }
            return isConnected;
        }

        private bool CheckTableLastSyncPozativeWabDatabaseDatetime(DateTime LastSyncDate, int IntervalTime)
        {
            bool isConnected = false;
            DateTime CurDatetime = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(Utility.Datetimesetting().ToString("MM/dd/yyyy hh:mm tt")) : Convert.ToDateTime(Utility.Datetimesetting());//.ToString("MM/dd/yyyy hh:mm tt"));
            DateTime TableLastSyncDate = !Utility.NotAllowToChangeSystemDateFormat ? Convert.ToDateTime(LastSyncDate.ToString("MM/dd/yyyy hh:mm tt")) : Convert.ToDateTime(LastSyncDate);//.ToString("MM/dd/yyyy hh:mm tt"));
            TimeSpan diffSpan = CurDatetime - TableLastSyncDate;
            if (diffSpan.TotalSeconds > (IntervalTime + 60))
            {
                isConnected = false;
            }
            else
            {
                isConnected = true;
            }
            return isConnected;
        }

        private async void checkEaglesoftConnection()
        {
            string err = string.Empty;
            string result = string.Empty;
            try
            {
                var response = await SynchEaglesoftDAL.Authenticate(Utility.EHRHostname, Utility.EHRIntegrationKey, Utility.EHRUserId, Utility.EHRPassword);
                if (response.StatusCode == HttpStatusCode.OK)
                    result = response.Content.ReadAsStringAsync().Result.Replace("\"", "");
                else
                {
                    result = response.Content.ReadAsStringAsync().Result.Replace("\"", "");
                    err = "Error: " + response.Content.ReadAsStringAsync().Result.Replace("\"", "");
                }

                if (err == string.Empty)
                {
                    Utility.APISessionToken = result;
                    // lblEHR.Text = Utility.Application_Name + " Status";
                    lblEHR.Text = Utility.Application_Name + " (" + Utility.Application_Version + ") " + " Status";
                    //ObjGoalBase.WriteToSyncLogFile(Utility.Application_Name + " Connected");
                    SetBtnDesignForConnectionStatus(btnEHR, Connectedtxt);
                    Utility.ISEagelsoftConnected = true;
                }
                else
                {
                    SetBtnDesignForConnectionStatus(btnEHR, DisConnectedtxt, false);
                    Utility.ISEagelsoftConnected = false;
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
                SetBtnDesignForConnectionStatus(btnEHR, DisConnectedtxt, false);
                Utility.ISEagelsoftConnected = false;
            }
            finally
            {
            }
        }

        private void SyncWithAditAppServer()
        {
            try
            {
                fncAditLocationSyncEnable();
                tmrAditLocationSyncEnable.Enabled = true;
                tmrAditLocationSyncEnable.Interval = 60000 * 30; // 60000 * 2;
                tmrAditLocationSyncEnable.Start();
                tmrAditLocationSyncEnable_Tick(null, null);
            }
            catch (Exception ex)
            {
                //TO check regular disconnection if this is calling regularly and not going forword
                Utility.WriteToErrorLogFromAll("SyncWithAditAppServer : Line Number : 2771 : " + ex.Message.ToString());
                SyncWithAditAppServer();
            }

            //if (Utility.AditLocationSyncEnable)
            //{

            try
            {               

                CallSynchZohoInstall();
                switch (Utility.Application_ID)
                {
                    case 1:
                        CallSynchLiveDB_PullToLocal();
                        //CallSynchEaglesoftToLocal();
                        break;
                    case 2:
                        CallSynchLiveDB_PullToLocal();
                        CallSynchOpenDentalToLocal();
                        break;
                    case 3:
                        CallSynchLiveDB_PullToLocal();
                        CallSynchDentrixToLocal();
                        break;
                    case 5:
                        CallSynchLiveDB_PullToLocal();
                        CallSynchClearDentToLocal();
                        break;
                    case 6:
                        CallSynchLiveDB_PullToLocal();
                        CallSynchTrackerToLocal();
                        break;
                    case 7:
                        CallSynchLiveDB_PullToLocal();
                        CallSynchPracticeWorkToLocal();
                        break;
                    case 8:
                        CallSynchLiveDB_PullToLocal();
                    // CallSynchEasyDentalToLocal();
                        break;
                    case 10:
                        CallSynchLiveDB_PullToLocal();
                        CallSynchPracticeWebToLocal();
                        break;
                    case 11:
                        CallSynchLiveDB_PullToLocal();
                        CallSynchAbelDentToLocal();
                        break;
                    case 13:
                        CallSynchOfficeMateToLocal();
                        break;
                        // case 12:
                        // CallSynchCrystalPMToLocal();
                        // break;
                }

                startGetPatientRecord.Enabled = true;
                startGetPatientRecord.Interval = 1000 * 30;
                startGetPatientRecord.Start();

                EHRConn = true;
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("SyncWithAditAppServer : Line Number : 2821 : " + ex.Message.ToString());
            }
            //}
            //else
            //{
            //    grpAdit.Text = "Adit-Off (" + Utility.AditLocationName + ")";
            //}
        }

        private void CheckVisibleControl()
        {
            LoadAppointmentRecord();

            // lblConnectionLog.Visible = true;
            lblSyncLog.Visible = true;
            //txtConnectionLog.Visible = true;
            // grdAppointment.Dock = DockStyle.Fill;
            grdPozativeAppointment.Dock = DockStyle.Fill;

            if (Utility.AditSync && Utility.PozativeSync)
            {
                grpAdit.Visible = true;
                grpPozative.Visible = true;
                grdPozativeAppointment.Visible = false;
                // grdAppointment.Visible = true;
                btnConfig.Visible = true;
            }
            else if (Utility.AditSync)
            {
                grpAdit.Visible = true;
                grpPozative.Visible = false;
                grdPozativeAppointment.Visible = false;
                grdAppointment.Visible = true;
                btnConfig.Visible = true;
            }
            else if (Utility.PozativeSync)
            {
                grpAdit.Visible = false;
                grpPozative.Visible = true;
                grdAppointment.Visible = false;
                grdPozativeAppointment.Visible = true;
                btnConfig.Visible = false;
            }
            else
            {
                grpAdit.Visible = false;
                grpPozative.Visible = false;
                grdAppointment.Visible = false;
                grdPozativeAppointment.Visible = false;
                btnConfig.Visible = false;

                // lblConnectionLog.Visible = false;
                lblSyncLog.Visible = false;
                // txtConnectionLog.Visible = false;
            }
        }

        private void SetToolTipOnControls()
        {
            TTPView.SetToolTip(picAppUpdate, "Click to check for application update");

            TTPView.SetToolTip(btnEHR, "Every " + Convert.ToInt64(GoalBase.intervalCheckConnection) / 60 + " minute Connection will be check with " + Utility.Application_Name + ".");

            // TTPView.SetToolTip(lblConnectionLog, "Every " + Convert.ToInt64(GoalBase.intervalCheckConnection) / 60 + " minute Connection Log will be update.");

            if (Utility.AditSync == true)
            {
                TTPView.SetToolTip(lblSyncLog, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_Appointment) / 60 + " minute Appointment Log will be update.");
            }
            else
            {
                TTPView.SetToolTip(lblSyncLog, "Every 2 minute Appointment Log will be update.");
            }

            TTPView.SetToolTip(btnAppt, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_Appointment) / 60 + " minute " + lblAppointment.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnOperatoryEvent, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_OperatoryEvent) / 60 + " minute " + lblOperatoryEvent.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnProviders, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_Provider) / 60 + " minute " + lblProviders.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnSpeciality, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_Speciality) / 60 + " minute " + lblSpeciality.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnOperatories, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_Operatory) / 60 + " minute " + lblOperatories.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnApptType, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_ApptType) / 60 + " minute " + lblApptType.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnPatient, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_Patient) / 60 + " minute " + lblPatient.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnRecallType, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_RecallType) / 60 + " minute " + lblRecallType.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnApptStatus, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_ApptStatus) / 60 + " minute " + lblApptStatus.Text + " will be sync " + Utility.Application_Name + " to Local server.");
            TTPView.SetToolTip(btnHoliday, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_Holiday) / 60 + " minute " + lblHoliday.Text + " will be sync " + Utility.Application_Name + " to Local server.");

            TTPView.SetToolTip(btnPozativeAppt, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_PozativeAppointment) / 60 + " minute Appointment will be sync " + Utility.Application_Name + " to Local server.");

            TTPView.SetToolTip(btnSyncAppt, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_Appointment) / 60 + " minute " + lblAppointment.Text + " will be sync Local server to Adit App server");
            TTPView.SetToolTip(btnSyncOperatoryEvent, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_OperatoryEvent) / 60 + " minute " + lblOperatoryEvent.Text + " will be sync Local server to Adit App server. (" + PushOperatoryEventRecord.ToString() + "/" + TotalPushOperatoryEventRecord.ToString() + ")");
            TTPView.SetToolTip(btnSyncProviders, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_Provider) / 60 + " minute " + lblProviders.Text + " will be sync Local server to Adit App server");
            TTPView.SetToolTip(btnSyncSpeciality, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_Speciality) / 60 + " minute " + lblSpeciality.Text + " will be sync Local server to Adit App server");
            TTPView.SetToolTip(btnSyncOperatories, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_Operatory) / 60 + " minute " + lblOperatories.Text + " will be sync Local server to Adit App server");
            TTPView.SetToolTip(btnSyncApptType, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_ApptType) / 60 + " minute " + lblApptType.Text + " will be sync Local server to Adit App server");
            TTPView.SetToolTip(btnSyncPatient, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_Patient) / 60 + " minute " + lblPatient.Text + " will be sync Local server to Adit App server.");
            TTPView.SetToolTip(btnSyncRecallType, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_RecallType) / 60 + " minute " + lblRecallType.Text + " will be sync Local server to Adit App server");
            TTPView.SetToolTip(btnSyncApptStatus, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_ApptStatus) / 60 + " minute " + lblApptStatus.Text + " will be sync Local server to Adit App server");
            TTPView.SetToolTip(btnSyncHoliday, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_Holiday) / 60 + " minute " + lblHoliday.Text + " will be sync Local server to Adit App server");

            TTPView.SetToolTip(btnPozativeSyncAppt, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_PozativeAppointment) / 60 + " minute Appointment will be sync Local Server to Pozative App server.");
        }

        private void SetAditSyncProcessTime()
        {
            try
            {

                DataTable dtAditModuleSyncTime = SystemBAL.GetAditModuleSyncTime();

                if (dtAditModuleSyncTime != null && dtAditModuleSyncTime.Rows.Count > 0)
                {
                    // Appointment
                    DataRow[] AppointmentRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'appointment'");
                    if (AppointmentRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_Appointment = 60 * Convert.ToInt32(AppointmentRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_Appointment = 60 * Convert.ToInt32(AppointmentRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_Appointment = 60 * Convert.ToInt32(AppointmentRow[0]["SyncModule_Push"].ToString().Trim());
                        GoalBase.intervalEHRSynch_PracticeAnalytics = GoalBase.intervalWebSynch_Push_Appointment;
                    }

                    //Holiday
                    DataRow[] HolidayRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'holiday'");
                    if (HolidayRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_Holiday = 60 * Convert.ToInt32(HolidayRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_Holiday = 60 * Convert.ToInt32(HolidayRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_Holiday = 60 * Convert.ToInt32(HolidayRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //Patient Form
                    DataRow[] PatientFormRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'patient form'");
                    if (PatientFormRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_PatientForm = 60 * Convert.ToInt32(PatientFormRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_PatientForm = 60 * Convert.ToInt32(PatientFormRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_PatientForm = 60 * Convert.ToInt32(PatientFormRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //DataRow[] ClearLocalRecordsRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'clear local records'");
                    //if (ClearLocalRecordsRow.Length > 0)
                    //{
                    //    GoalBase.intervalEHRSynch_ClearLocalRecords = 60 * Convert.ToInt32(ClearLocalRecordsRow[0]["SyncModule_EHR"].ToString().Trim());
                    //    GoalBase.intervalWebSynch_Pull_ClearLocalRecords = 60 * Convert.ToInt32(ClearLocalRecordsRow[0]["SyncModule_Pull"].ToString().Trim());
                    //    GoalBase.intervalWebSynch_Push_ClearLocalRecords = 60 * Convert.ToInt32(ClearLocalRecordsRow[0]["SyncModule_Push"].ToString().Trim());
                    //}

                    //Operatory
                    DataRow[] OperatoryRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'operatoryevent'");
                    if (OperatoryRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_OperatoryEvent = 60 * Convert.ToInt32(OperatoryRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_OperatoryEvent = 60 * Convert.ToInt32(OperatoryRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_OperatoryEvent = 60 * Convert.ToInt32(OperatoryRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //Provider
                    DataRow[] ProviderRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'provider'");
                    if (ProviderRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_Provider = 60 * Convert.ToInt32(ProviderRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalEHRSynch_MedicalHistory = GoalBase.intervalEHRSynch_Provider;
                        GoalBase.intervalWebSynch_Pull_Provider = 60 * Convert.ToInt32(ProviderRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_Provider = 60 * Convert.ToInt32(ProviderRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //Speciality
                    DataRow[] SpecialityRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'speciality'");
                    if (SpecialityRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_Speciality = 60 * Convert.ToInt32(SpecialityRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_Speciality = 60 * Convert.ToInt32(SpecialityRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_Speciality = 60 * Convert.ToInt32(SpecialityRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //Operatory
                    DataRow[] OperatoryEventRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'operatory'");
                    if (OperatoryRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_Operatory = 60 * Convert.ToInt32(OperatoryEventRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_Operatory = 60 * Convert.ToInt32(OperatoryEventRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_Operatory = 60 * Convert.ToInt32(OperatoryEventRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //ApptType
                    DataRow[] ApptTypeRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'appointment type'");
                    if (ApptTypeRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_ApptType = 60 * Convert.ToInt32(ApptTypeRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_ApptType = 60 * Convert.ToInt32(ApptTypeRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_ApptType = 60 * Convert.ToInt32(ApptTypeRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //Patient
                    DataRow[] PatientRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'patient'");
                    if (PatientRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_Patient = 60 * Convert.ToInt32(PatientRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_Patient = 60 * Convert.ToInt32(PatientRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_Patient = 60 * Convert.ToInt32(PatientRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //RecallType
                    DataRow[] RecallTypeRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'recall type'");
                    if (RecallTypeRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_RecallType = 60 * Convert.ToInt32(RecallTypeRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_RecallType = 60 * Convert.ToInt32(RecallTypeRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_RecallType = 60 * Convert.ToInt32(RecallTypeRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //ApptStatus
                    DataRow[] ApptStatusRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'appointment status'");
                    if (ApptStatusRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_ApptStatus = 60 * Convert.ToInt32(ApptStatusRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_ApptStatus = 60 * Convert.ToInt32(ApptStatusRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_ApptStatus = 60 * Convert.ToInt32(ApptStatusRow[0]["SyncModule_Push"].ToString().Trim());
                    }

                    //Payment
                    DataRow[] paymentRow = dtAditModuleSyncTime.Copy().Select("SyncModule_Name = 'payment'");
                    if (ApptStatusRow.Length > 0)
                    {
                        GoalBase.intervalEHRSynch_PatientPayment = 60 * Convert.ToInt32(paymentRow[0]["SyncModule_EHR"].ToString().Trim());
                        GoalBase.intervalWebSynch_Pull_PatientPayment = 60 * Convert.ToInt32(paymentRow[0]["SyncModule_Pull"].ToString().Trim());
                        GoalBase.intervalWebSynch_Push_PatientPayment = 60 * Convert.ToInt32(paymentRow[0]["SyncModule_Push"].ToString().Trim());
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void UpdateApplicationVersionOnLiveDatabase()
        {
            try
            {
                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                {
                    string strApiUpdateVersionNo = SystemBAL.UpdateApplicationVersionOnLiveDatabase();

                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    var clientLogin = new RestClient(strApiUpdateVersionNo);
                    var requestVersion = new RestRequest(Method.POST);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    requestVersion.AddHeader("Postman-Token", "1d16df4c-48ba-4644-bc7a-9bcef2a86744");
                    requestVersion.AddHeader("Authorization", Utility.WebAdminUserToken);
                    requestVersion.AddHeader("cache-control", "no-cache");
                    requestVersion.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                    DesktopVersionBO DesktopVer = new DesktopVersionBO
                    {
                        location = Utility.DtLocationList.Rows[j]["Location_Id"].ToString(),
                        type = "server",
                        version = Utility.Server_App_Version,
                        created_by = Utility.User_ID
                    };
                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    ObjGoalBase.WriteToSyncLogFile("Update Version Called " + Utility.Server_App_Version.ToString());
                    string jsonString = javaScriptSerializer.Serialize(DesktopVer);
                    requestVersion.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                    IRestResponse responseLogin = clientLogin.Execute(requestVersion);
                    ObjGoalBase.WriteToSyncLogFile("Version Updated");
                }
            }
            catch (Exception)
            {
            }
        }

        private void fncAditLocationSyncEnable()
        {
            try
            {
                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                {

                    string strApiAditLocationSyncEnable = SystemBAL.AditLocationSyncEnable(Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.User_ID);
                    var client = new RestClient(strApiAditLocationSyncEnable);
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                    IRestResponse response = client.Execute(request);
                    if (response.ErrorMessage != null)
                    {
                        ObjGoalBase.WriteToErrorLogFile("[AditLocationSyncEnable] : " + response.ErrorMessage);
                        return;
                    }
                    var IsAditLocationSyncEnable = JsonConvert.DeserializeObject<AditLocationSyncBO>(response.Content);
                    try
                    {
                        bool templocEnable = Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]);
                        Utility.DtLocationList.Rows[j]["ApptAutoBook"] = Convert.ToBoolean(IsAditLocationSyncEnable.data.webauto_sync); //Utility.ApptAutoBook
                        Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"] = Convert.ToBoolean(IsAditLocationSyncEnable.data.ehr_sync_status);
                        Utility.ApptAutoBook = Convert.ToBoolean(IsAditLocationSyncEnable.data.webauto_sync);
                        Utility.imageuploadbatch = Convert.ToInt16(IsAditLocationSyncEnable.data.imageuploadbatch);
                        Utility.AditLocationSyncEnable = Convert.ToBoolean(IsAditLocationSyncEnable.data.ehr_sync_status);
                        SystemBAL.UpdateAditLocationSyncEnable(Utility.DtLocationList.Rows[j]["Clinic_Number"].ToString(), Utility.DtLocationList.Rows[j]["Service_Install_Id"].ToString(), Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Convert.ToBoolean(Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"]));
                        ObjGoalBase.WriteToSyncLogFile("[AditLocationSyncEnable] : " + DateTime.Now.ToString());
                        if (Utility.Application_ID != 2)
                        {
                            if (!Utility.AditLocationSyncEnable && Utility.AditLocationSyncEnable != templocEnable)
                            {
                                Application.Exit();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Utility.DtLocationList.Rows[j]["ApptAutoBook"] = true; //Utility.ApptAutoBook
                        Utility.DtLocationList.Rows[j]["AditLocationSyncEnable"] = true;
                        Utility.ApptAutoBook = true;
                    }
                    Utility.DtLocationList.AcceptChanges();
                    try
                    {
                        Utility.SyncPracticeAnalytics_Disabled = Convert.ToBoolean(IsAditLocationSyncEnable.data.pa_disabled_sync);

                        if (Utility.DtLocationList.Rows.Count > 1)
                        {
                            if (Utility.SyncPracticeAnalytics && !Convert.ToBoolean(IsAditLocationSyncEnable.data.sync_practice_analytics))
                            {
                                Utility.SyncPracticeAnalytics = true;
                                Utility.SyncPracticeAnalytics_Enabled = true;
                            }
                            else
                            {
                                Utility.SyncPracticeAnalytics = Convert.ToBoolean(IsAditLocationSyncEnable.data.sync_practice_analytics);
                                Utility.SyncPracticeAnalytics_Enabled = Convert.ToBoolean(IsAditLocationSyncEnable.data.sync_practice_analytics);
                            }
                        }
                        else
                        {
                            Utility.SyncPracticeAnalytics = Convert.ToBoolean(IsAditLocationSyncEnable.data.sync_practice_analytics);
                            Utility.SyncPracticeAnalytics_Enabled = Convert.ToBoolean(IsAditLocationSyncEnable.data.sync_practice_analytics);
                        }

                        // Utility.SyncPracticeAnalytics = true;
                        if (Utility.SyncPracticeAnalytics_Disabled && Utility.SyncPracticeAnalytics == false)
                        {
                            Utility.SyncPracticeAnalytics = true;
                        }
                    }
                    catch (Exception)
                    {
                        Utility.SyncPracticeAnalytics = false;
                        Utility.SyncPracticeAnalytics_Enabled = false;
                    }

                    try
                    {
                        try
                        {
                            GoalBase.intervalEHRSynch_CallSmsLog = Convert.ToInt16(IsAditLocationSyncEnable.data.smscall_synclimit);
                        }
                        catch (Exception)
                        {
                            GoalBase.intervalEHRSynch_CallSmsLog = 500;
                        }
                        try
                        {
                            GoalBase.intervalEHRSynch_PatientFollowUp = Convert.ToInt16(IsAditLocationSyncEnable.data.patient_followuplimit);
                        }
                        catch (Exception)
                        {
                            GoalBase.intervalEHRSynch_PatientFollowUp = 500;
                        }

                        Utility.LastPaymentSMSCallSyncDateAditServer = Convert.ToDateTime(IsAditLocationSyncEnable.data.log_sync_date);

                        try
                        {
                            Utility.LastSyncDateAditServer = DateTime.Parse(Utility.ConvertDatetimeToCurrentLocationFormat(IsAditLocationSyncEnable.data.lastSyncAppointment)).AddDays(-7);
                        }
                        catch (Exception)
                        {
                            DateTime dtCurrentDtTime1 = Utility.Datetimesetting();
                            Utility.LastSyncDateAditServer = dtCurrentDtTime1.AddDays(-7);
                        }

                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        if (Convert.ToDateTime(Utility.ConvertDatetimeToCurrentLocationFormat(IsAditLocationSyncEnable.data.lastSyncAppointment)) < dtCurrentDtTime.AddMonths(-6))
                        {
                            Utility.LastSyncDateAditServer = dtCurrentDtTime.AddDays(-7);
                        }
                    }
                    catch (Exception)
                    {
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        Utility.LastSyncDateAditServer = dtCurrentDtTime.AddDays(-7);
                        Utility.LastPaymentSMSCallSyncDateAditServer = dtCurrentDtTime.AddDays(7);
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[AditLocationSyncEnable] : " + ex.Message.ToString());
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                Utility.LastSyncDateAditServer = dtCurrentDtTime.AddDays(-7);
                Utility.LastPaymentSMSCallSyncDateAditServer = dtCurrentDtTime.AddDays(7);
                //GoalBase.intervalEHRSynch_CallSmsLog = 500;
                //GoalBase.intervalEHRSynch_PatientFollowUp = 500;
                Utility.AditLocationSyncEnable = true;
            }
        }

        #endregion

        #region Refresh System Tray Icon

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        public static void RefreshTrayArea()
        {
            IntPtr systemTrayContainerHandle = FindWindow("Shell_TrayWnd", null);
            IntPtr systemTrayHandle = FindWindowEx(systemTrayContainerHandle, IntPtr.Zero, "TrayNotifyWnd", null);
            IntPtr sysPagerHandle = FindWindowEx(systemTrayHandle, IntPtr.Zero, "SysPager", null);
            IntPtr notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32", "Notification Area");
            if (notificationAreaHandle == IntPtr.Zero)
            {
                notificationAreaHandle = FindWindowEx(sysPagerHandle, IntPtr.Zero, "ToolbarWindow32",
                    "User Promoted Notification Area");
                IntPtr notifyIconOverflowWindowHandle = FindWindow("NotifyIconOverflowWindow", null);
                IntPtr overflowNotificationAreaHandle = FindWindowEx(notifyIconOverflowWindowHandle, IntPtr.Zero,
                    "ToolbarWindow32", "Overflow Notification Area");
                RefreshTrayArea(overflowNotificationAreaHandle);
            }
            RefreshTrayArea(notificationAreaHandle);
        }

        private static void RefreshTrayArea(IntPtr windowHandle)
        {
            const uint wmMousemove = 0x0200;
            RECT rect;
            GetClientRect(windowHandle, out rect);
            for (var x = 0; x < rect.right; x += 5)
                for (var y = 0; y < rect.bottom; y += 5)
                    SendMessage(windowHandle, wmMousemove, 0, (y << 16) + x);
        }

        #endregion

        #region Check Application Update

        //private void tmrCheckApplicationUpdate_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        CheckApplicationUpdateVersion();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //private void CheckApplicationUpdateVersion()
        //{
        //    var version = Environment.OSVersion.Version;
        //    File.Delete(Application.StartupPath + "\\Interop.Shell32.dll");
        //    if (version < new Version(6, 2))
        //    {
        //        File.Copy(Application.StartupPath + "\\SystemOS\\Interop.Shell32_07.dll", Application.StartupPath + "\\Interop.Shell32.dll");
        //    }
        //    else
        //    {
        //        File.Copy(Application.StartupPath + "\\SystemOS\\Interop.Shell32_08.dll", Application.StartupPath + "\\Interop.Shell32.dll");
        //    }


        //    string baseURL;
        //    baseURL = "https://pozative.com/public/pozativeservice/";

        //    currentPath = Application.StartupPath;
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    System.Net.WebClient client = new System.Net.WebClient();
        //    string content = client.DownloadString(baseURL);
        //    string newVersion;
        //    newVersion = content.Substring(content.IndexOf("<TD>1."));
        //    newVersion = newVersion.Substring(newVersion.IndexOf(">") + 1);
        //    newVersion = newVersion.Substring(0, newVersion.IndexOf("</TD>"));

        //    var versionNew = new Version(newVersion);
        //    var versionCurrent = new Version(versionNo);
        //    var result = versionNew.CompareTo(versionCurrent);

        //    if (result <= 0)
        //    {
        //        newVersion = string.Empty;
        //    }

        //    if ((newVersion != string.Empty))
        //    {
        //        int returnStatus;
        //        returnStatus = UpdateEXE(baseURL + newVersion + ".zip", newVersion);

        //        if (returnStatus == 2)
        //        {
        //            // Application.Restart();
        //            // tmrConsoleRun_Tick(null, null);
        //            // System.Environment.Exit(1);
        //        }

        //    }
        //    else
        //    {
        //        if (System.IO.Directory.Exists(Application.StartupPath + "\\BK"))
        //        {
        //            DeleteDirectory(Application.StartupPath + "\\BK");
        //        }
        //        if (!string.IsNullOrEmpty(newVersion))
        //        {
        //            if (System.IO.Directory.Exists(Application.StartupPath + "\\" + newVersion))
        //            {
        //                DeleteDirectory(Application.StartupPath + "\\" + newVersion);
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(versionNo))
        //        {
        //            if (System.IO.Directory.Exists(Application.StartupPath + "\\" + versionNo))
        //            {
        //                DeleteDirectory(Application.StartupPath + "\\" + versionNo);
        //            }
        //        }
        //    }
        //}

        //public int UpdateEXE(string Location, string newVersion)
        //{
        //    var temppath = Path.GetTempPath();

        //    var dldname = System.IO.Path.GetFileName(Location);
        //    bool suc;
        //    suc = DownloadFileWithProgress(Location, temppath.ToString() + @"\" + dldname);
        //    if ((suc))
        //    {

        //        bool isUnzip = UnZip(temppath.ToString() + @"\" + dldname, currentPath, vFolderName, newVersion);

        //        backupDirName = currentPath + @"\BK\";
        //        if (!(System.IO.Directory.Exists(backupDirName)))
        //            System.IO.Directory.CreateDirectory(backupDirName);
        //        System.IO.DirectoryInfo source = new System.IO.DirectoryInfo(currentPath + @"\" + vFolderName);
        //        System.IO.DirectoryInfo target = new System.IO.DirectoryInfo(currentPath);
        //        CopyAll(source, target);
        //        return 2;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //private bool UnZip(string zipFile, string unzipFolder, string versionFolderName, string newVersion)
        //{
        //    try
        //    {
        //        Shell32.Shell shell = new Shell32.Shell();
        //        Shell32.Folder archive = shell.NameSpace(System.IO.Path.GetFullPath(zipFile));
        //        if ((archive.Items().Count == 1))
        //        {
        //            foreach (Shell32.FolderItem f in archive.Items())
        //            {
        //                if ((f.IsFolder))
        //                    vFolderName = f.Name;
        //                else
        //                {
        //                    vFolderName = newVersion;
        //                    unzipFolder = unzipFolder + @"\" + vFolderName;
        //                    System.IO.Directory.CreateDirectory(unzipFolder);
        //                }
        //            }
        //        }
        //        else if ((archive.Items().Count > 1))
        //        {
        //            vFolderName = newVersion;
        //            unzipFolder = unzipFolder + @"\" + vFolderName;
        //            System.IO.Directory.CreateDirectory(unzipFolder);
        //        }

        //        Shell32.Folder extractFolder = shell.NameSpace(System.IO.Path.GetFullPath(unzipFolder));
        //        foreach (Shell32.FolderItem f in archive.Items())
        //            extractFolder.CopyHere(f, 20);

        //        if (!WaitTillItemCountIsEqual(archive, extractFolder))
        //            return false;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //private static bool WaitTillItemCountIsEqual(Shell32.Folder folderObjSource, Shell32.Folder folderObjDestination)
        //{
        //    try
        //    {
        //        if (folderObjSource == null || folderObjDestination == null)
        //            return false;
        //        int sourceFolderItemCount = folderObjSource.Items().Count;
        //        int maxIterations = (5 * 1000) / 100;
        //        int numIterations = 0;
        //        while (folderObjDestination.Items().Count < sourceFolderItemCount)
        //        {
        //            if (maxIterations <= System.Math.Max(System.Threading.Interlocked.Increment(ref numIterations), numIterations - 1))
        //                return false;
        //            System.Threading.Thread.Sleep(100);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //private bool DownloadFileWithProgress(string URL, string Location)
        //{
        //    System.IO.FileStream FS;
        //    Stopwatch sw = new Stopwatch();
        //    FS = new System.IO.FileStream(Location, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        //    // int iTotalBytesRead = 0;
        //    try
        //    {
        //        System.Net.WebRequest wRemote;
        //        byte[] bBuffer;
        //        bBuffer = new byte[257];
        //        int iBytesRead;


        //        wRemote = System.Net.WebRequest.Create(URL);
        //        sw.Start();
        //        DateTime t1 = DateTime.Now;
        //        System.Net.WebResponse myWebResponse = wRemote.GetResponse();
        //        System.IO.Stream sChunks = myWebResponse.GetResponseStream();
        //        do
        //        {
        //            iBytesRead = sChunks.Read(bBuffer, 0, 256);
        //            FS.Write(bBuffer, 0, iBytesRead);
        //            //  iTotalBytesRead += iBytesRead;
        //        }
        //        while (iBytesRead != 0);
        //        sChunks.Close();
        //        FS.Close();
        //        sw.Stop();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        sw.Stop();
        //        if (!(FS == null))
        //        {
        //            FS.Close();
        //            FS = null;
        //        }
        //        return false;
        //    }
        //}

        //private bool CopyAll(System.IO.DirectoryInfo source, System.IO.DirectoryInfo target)
        //{
        //    try
        //    {
        //        if (source.FullName.ToLower() == target.FullName.ToLower())
        //            return false;
        //        if (System.IO.Directory.Exists(target.FullName) == false)
        //            System.IO.Directory.CreateDirectory(target.FullName);
        //        foreach (System.IO.FileInfo fi in source.GetFiles())
        //        {
        //            try
        //            {
        //                fi.CopyTo(System.IO.Path.Combine(target.ToString(), fi.Name), true);
        //            }
        //            catch (Exception ex)
        //            {
        //                if ((ex.Message.ToString().Contains("used by another process")))
        //                    fi.Replace(System.IO.Path.Combine(target.ToString(), fi.Name), System.IO.Path.Combine(backupDirName, fi.Name));
        //            }
        //        }
        //        foreach (System.IO.DirectoryInfo diSourceSubDir in source.GetDirectories())
        //            CopyAll(diSourceSubDir, target.CreateSubdirectory(diSourceSubDir.Name));
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public static void DeleteDirectory(string target_dir)
        //{
        //    string[] files = Directory.GetFiles(target_dir);
        //    string[] dirs = Directory.GetDirectories(target_dir);

        //    foreach (string file in files)
        //    {
        //        try
        //        {
        //            File.SetAttributes(file, FileAttributes.Normal);
        //            File.Delete(file);
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }

        //    foreach (string dir in dirs)
        //    {
        //        try
        //        {
        //            DeleteDirectory(dir);
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }

        //    try
        //    {
        //        Directory.Delete(target_dir, false);
        //    }
        //    catch (Exception)
        //    {
        //    }

        //}

        #endregion

        #region Check Application Update

        private void tmrCheckApplicationUpdate_Tick(object sender, EventArgs e)
        {

            try
            {
                CheckApplicationUpdateVersion();
            }
            catch (Exception)
            {

            }
        }

        private void ServiceStartStop(bool TrueIfStart = false)
        {
            try
            {
                ServiceController sc = new ServiceController();
                sc.ServiceName = "PozativeAppStatus";
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    if (TrueIfStart == true)
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running);
                    }
                }
                else
                {
                    if (TrueIfStart == false)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                }


                ServiceController sc2 = new ServiceController();
                sc2.ServiceName = Utility.sEventServiceName;
                if (sc2.Status == ServiceControllerStatus.Stopped)
                {
                    if (TrueIfStart == true)
                    {
                        sc2.Start();
                        sc2.WaitForStatus(ServiceControllerStatus.Running);
                    }
                }
                else
                {
                    if (TrueIfStart == false)
                    {
                        sc2.Stop();
                        sc2.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void CheckApplicationUpdateVersion()
        {

            var version = Environment.OSVersion.Version;
            File.Delete(Application.StartupPath + "\\Interop.Shell32.dll");
            //File.Copy(Application.StartupPath + "\\SystemOS\\Interop.Shell32_07.dll", Application.StartupPath + "\\Interop.Shell32.dll");
            if (version < new Version(6, 2))
            {
                File.Copy(Application.StartupPath + "\\SystemOS\\Interop.Shell32_07.dll", Application.StartupPath + "\\Interop.Shell32.dll");
            }
            else
            {
                File.Copy(Application.StartupPath + "\\SystemOS\\Interop.Shell32_08.dll", Application.StartupPath + "\\Interop.Shell32.dll");
            }
            string baseURL = "https://pozative.com/public/pozativeservice/";

            //try
            //{
            //    bool Is_SoftDentorEasyDental = false;
            //    bool flag2 = false;
            //    bool Is_MysqlHigherVersion = false;

            //    if (Utility.Application_ID == 4 || Utility.Application_ID == 8)
            //    {
            //        Is_SoftDentorEasyDental = true;
            //    }
            //    if (Utility.Application_ID == 2)
            //    {
            //        string str3 = Path.Combine("C:\\Program Files (x86)\\Pozative.exe", "MySql.Data.dll");
            //        if (System.IO.File.Exists(str3) && FileVersionInfo.GetVersionInfo(str3).FileMajorPart > 6)
            //            Is_MysqlHigherVersion = true;
            //    }              
            //    if (Is_SoftDentorEasyDental)
            //    {
            //        baseURL = "https://pozative.com/public/pozativeservice/PozativeEZ";
            //    }
            //    else if (Is_MysqlHigherVersion)
            //    {
            //        baseURL = "https://pozative.com/public/pozativeservice/PozativeODH";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    baseURL = "https://pozative.com/public/pozativeservice/";
            //}


            // string baseURL = "https://pozative.com/public/pozativeservicetest/";

            currentPath = Application.StartupPath;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.WebClient client = new System.Net.WebClient();
            string content = string.Empty;
            try
            {
                content = client.DownloadString(baseURL);
            }
            catch (Exception)
            {
                baseURL = "http://pozative.com/public/pozativeservice/";
                content = client.DownloadString(baseURL);
            }

            string newVersion;
            newVersion = content.Substring(content.IndexOf("<TD>1."));
            newVersion = newVersion.Substring(newVersion.IndexOf(">") + 1);
            newVersion = newVersion.Substring(0, newVersion.IndexOf("</TD>"));

            var versionNew = new Version(newVersion);
            var versionCurrent = new Version(Utility.Server_App_Version);
            var result = versionNew.CompareTo(versionCurrent);

            if (result <= 0)
            {
                newVersion = "";
                picAppUpdate.Visible = false;
            }

            if ((newVersion != ""))
            {
                // picAppUpdate.Visible = true;

                bool CurLocAllowAppUpdate = GetLocationUpdateVersiononAditServer();
                //if (CurLocAllowAppUpdate == false)
                //{
                //    CurLocAllowAppUpdate = SystemBAL.GetCurrentLocationAllowAppUpdate();
                //}

                if (CurLocAllowAppUpdate)
                {

                    int returnStatus;
                    ServiceStartStop();
                    returnStatus = UpdateEXE(baseURL + newVersion + ".zip", newVersion);
                    if (returnStatus == 2)
                    {
                        // bool updateVersionPozativeServer = SystemBAL.UpdateLocNewVersionOnPozative_Server_App(newVersion);

                        UpdateLocationEHRUPdateForVersionBO LEUFVBO = new UpdateLocationEHRUPdateForVersionBO
                        {
                            appointmentlocation_id = Utility.Location_ID.ToString(),
                            is_auto_update = 0,
                            last_updated = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-ddTHH:mm:ss"),
                            server_app_version = newVersion.ToString(),
                            system_name = CommonUtility.SystemName,
                            operating_system = CommonUtility.OperatingSystem,
                            processor_name = CommonUtility.ProcessorName,
                            service_pack = CommonUtility.ServicePack,
                            total_ram = CommonUtility.TRAM,
                            available_ram = CommonUtility.ARAM,
                            total_hdisk = CommonUtility.THardDisk,
                            available_hdisk = CommonUtility.AHardDisk,
                            dotnetframework = CommonUtility.FrameWork,
                            system_type = CommonUtility.SystemType,
                        };
                        var javaScriptSerializerLEUFVBO = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string JsonString = javaScriptSerializerLEUFVBO.Serialize(LEUFVBO);
                        JsonString = "[" + JsonString.ToString() + "]";
                        ObjGoalBase.WriteToSyncLogFile("Update Version Called " + JsonString);
                        string updateVersionAditServer = PushLiveDatabaseBAL.UpdateLocNewVersionOnAdit_Server_App(JsonString);
                        ObjGoalBase.WriteToSyncLogFile("Version Updated " + JsonString);
                        KillEXE();
                        ServiceStartStop(true);
                        try
                        {
                            Utility.UpdateBackupDBFromLocalDB();
                        }
                        catch { }
                        System.Environment.Exit(1);
                        ApplicationManuallyUpdate = false;
                    }
                    ServiceStartStop(true);
                }
            }
            else
            {
                if (System.IO.Directory.Exists(Application.StartupPath + @"\BK"))
                {
                    DeleteDirectory(Application.StartupPath + @"\BK");
                }
                if (!string.IsNullOrEmpty(newVersion))
                {
                    if (System.IO.Directory.Exists(Application.StartupPath + @"\" + newVersion))
                    {
                        DeleteDirectory(Application.StartupPath + @"\" + newVersion);
                    }
                }

                if (!string.IsNullOrEmpty(Utility.Server_App_Version))
                {
                    if (System.IO.Directory.Exists(Application.StartupPath + @"\" + Utility.Server_App_Version))
                    {
                        DeleteDirectory(Application.StartupPath + @"\" + Utility.Server_App_Version);
                    }
                }
            }
        }
        public void KillEXE()
        {
            try
            {
                foreach (Process p in Process.GetProcesses())
                {
                    string fileName = p.ProcessName;

                    if (string.Compare("Pozative", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                    {
                        p.Kill();
                        continue;
                    }
                    if (string.Compare("PracticeAnalytics".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        p.Kill();
                        continue;
                    }
                    if (string.Compare("DentrixDocument".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        p.Kill();
                        continue;
                    }
                    if (string.Compare("DTX_Helper".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        p.Kill();
                        continue;
                    }
                    if (string.Compare("EasyDentalSync".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
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
                    if (string.Compare("CrystalPM".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
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
                    if (string.Compare("SoftDentSync".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        p.Kill();
                        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                        key.SetValue("IsSyncing", false);
                        continue;
                    }
                    if (string.Compare("AditAppSyncLog".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        p.Kill();
                        continue;
                    }
                    if (string.Compare("PTIAuthenticator".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        p.Kill();
                        continue;
                    }
                    if (string.Compare("SyncServerAppCheck".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        p.Kill();
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("KillEXE" + ex.Message);
            }
        }

        public int UpdateEXE(string Location, string newVersion)
        {
            try
            {
                var temppath = Path.GetTempPath();

                var dldname = System.IO.Path.GetFileName(Location);
                bool suc;
                suc = DownloadFileWithProgress(Location, temppath.ToString() + @"\" + dldname);
                //if ((suc))
                //{

                //    bool isUnzip = UnZip(temppath.ToString() + @"\" + dldname, currentPath, vFolderName, newVersion);



                //    backupDirName = currentPath + @"\BK\";
                //    if (!(System.IO.Directory.Exists(backupDirName)))
                //        System.IO.Directory.CreateDirectory(backupDirName);
                //    System.IO.DirectoryInfo source = new System.IO.DirectoryInfo(currentPath + @"\" + vFolderName);
                //    System.IO.DirectoryInfo target = new System.IO.DirectoryInfo(currentPath);
                //    CopyAll(source, target);
                //    return 2;
                //}
                //else
                //{
                //    return 0;
                //}

                object[] args = new object[] { temppath + dldname, Application.StartupPath };
                if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
                {
                    UnZip(args);

                    System.IO.DirectoryInfo sourceT = new System.IO.DirectoryInfo(temppath.ToString() + @"\" + dldname.Replace(".zip", ""));
                    System.IO.DirectoryInfo targetT = new System.IO.DirectoryInfo(Application.StartupPath + @"\" + dldname.Replace(".zip", ""));
                    CopyAll(sourceT, targetT);

                    backupDirName = currentPath + @"\BK\";
                    if (!(System.IO.Directory.Exists(backupDirName)))
                        System.IO.Directory.CreateDirectory(backupDirName);

                    System.IO.DirectoryInfo source = new System.IO.DirectoryInfo(currentPath + @"\" + dldname.Replace(".zip", ""));
                    System.IO.DirectoryInfo target = new System.IO.DirectoryInfo(currentPath);
                    CopyAll(source, target);

                }
                else
                {
                    Thread staThread = new Thread(new ParameterizedThreadStart(UnZip));
                    staThread.SetApartmentState(ApartmentState.STA);
                    staThread.Start(args);
                    staThread.Join();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("UpdateEXE" + ex.Message);
            }
            return 2;
        }

        private static void UnZip(object param)
        {
            try
            {
                object[] args = (object[])param;
                string zipFile = (string)args[0];
                string folderPath = (string)args[1];

                var temppath = Path.GetTempPath();

                if (!File.Exists(zipFile))
                    throw new FileNotFoundException();

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                //Shell32.Shell objShell = new Shell32.Shell();
                //Shell32.Folder destinationFolder = objShell.NameSpace(folderPath);
                //Shell32.Folder sourceFile = objShell.NameSpace(zipFile);

                //foreach (var file in sourceFile.Items())
                //{
                //    destinationFolder.CopyHere(file, 4 | 16 | 1024);
                //}

                Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
                Object shell = Activator.CreateInstance(shellAppType);
                Folder zip = (Shell32.Folder)shellAppType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { zipFile });
                Folder dest = (Shell32.Folder)shellAppType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { temppath.ToString() });
                dest.CopyHere(zip.Items(), 0x14);

            }
            catch (Exception)
            {
                // ObjGoalBase.WriteToSyncLogFile("Error_UnZip " + ex.Message.ToString());
            }

        }

        public Shell32.Folder GetShell32NameSpaceFolder(Object param)
        {

            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            Object shell = Activator.CreateInstance(shellAppType);
            return (Shell32.Folder)shellAppType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { param });

        }

        //private bool UnZip(string zipFile, string unzipFolder, string versionFolderName, string newVersion)
        //{
        //    try
        //    {
        //        Shell32.Shell shell = new Shell32.Shell();
        //        Shell32.Folder archive = shell.NameSpace(System.IO.Path.GetFullPath(zipFile));
        //        if ((archive.Items().Count == 1))
        //        {
        //            foreach (Shell32.FolderItem f in archive.Items())
        //            {
        //                if ((f.IsFolder))

        //                    vFolderName = f.Name;
        //                else
        //                {
        //                    vFolderName = newVersion;
        //                    unzipFolder = unzipFolder + "\\" + vFolderName;
        //                    System.IO.Directory.CreateDirectory(unzipFolder);
        //                }
        //            }
        //        }
        //        else if ((archive.Items().Count > 1))
        //        {
        //            vFolderName = newVersion;
        //            unzipFolder = unzipFolder + "\\" + vFolderName;
        //            System.IO.Directory.CreateDirectory(unzipFolder);
        //        }

        //        Shell32.Folder extractFolder = shell.NameSpace(System.IO.Path.GetFullPath(unzipFolder));
        //        foreach (Shell32.FolderItem f in archive.Items())
        //            extractFolder.CopyHere(f, 20);

        //        if (!WaitTillItemCountIsEqual(archive, extractFolder))
        //            return false;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return false;
        //    }
        //}

        private static bool WaitTillItemCountIsEqual(Shell32.Folder folderObjSource, Shell32.Folder folderObjDestination)
        {
            try
            {
                if (folderObjSource == null || folderObjDestination == null)
                    return false;
                int sourceFolderItemCount = folderObjSource.Items().Count;
                int maxIterations = (5 * 1000) / 100;
                int numIterations = 0;
                while (folderObjDestination.Items().Count < sourceFolderItemCount)
                {
                    if (maxIterations <= System.Math.Max(System.Threading.Interlocked.Increment(ref numIterations), numIterations - 1))
                        return false;
                    System.Threading.Thread.Sleep(100);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool DownloadFileWithProgress(string URL, string Location)
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

        private bool CopyAll(System.IO.DirectoryInfo source, System.IO.DirectoryInfo target)
        {
            try
            {
                if (source.FullName.ToLower() == target.FullName.ToLower())
                    return false;
                if (System.IO.Directory.Exists(target.FullName) == false)
                    System.IO.Directory.CreateDirectory(target.FullName);
                foreach (System.IO.FileInfo fi in source.GetFiles())
                {
                    try
                    {
                        fi.CopyTo(System.IO.Path.Combine(target.ToString(), fi.Name), true);
                    }
                    catch (Exception ex)
                    {
                        if ((ex.Message.ToString().Contains("used by another process")))
                            fi.Replace(System.IO.Path.Combine(target.ToString(), fi.Name), System.IO.Path.Combine(backupDirName, fi.Name));
                    }
                }
                foreach (System.IO.DirectoryInfo diSourceSubDir in source.GetDirectories())
                    CopyAll(diSourceSubDir, target.CreateSubdirectory(diSourceSubDir.Name));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                try
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
                catch (Exception)
                {
                }
            }

            foreach (string dir in dirs)
            {
                try
                {
                    DeleteDirectory(dir);
                }
                catch (Exception)
                {
                }
            }

            try
            {
                Directory.Delete(target_dir, false);
            }
            catch (Exception)
            {
            }

        }

        #endregion

        #region Button Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
            }
            catch (Exception)
            {
            }
        }

        private void btnAppRestart_Click(object sender, EventArgs e)
        {
            try
            {
                ObjGoalBase.WriteToSyncLogFile("[btnAppRestart_Click] manually Application restart");

                //foreach (Process p in Process.GetProcesses())
                //{

                //    if (string.Compare("Pozative", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                //    {
                //        p.Kill();
                //        continue;
                //    }
                //    if (string.Compare("PracticeAnalytics".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                //    {
                //        p.Kill();
                //        continue;
                //    }
                //    // For softdent close application
                //    if (string.Compare("SoftDentSync", p.ProcessName, true) == 0)
                //    {
                //        p.Kill();
                //        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                //        key.SetValue("IsSyncing", false);
                //        continue;
                //    }
                //    if (string.Compare("EasyDentalSync", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                //    {
                //        p.Kill();
                //        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                //        {
                //            key.SetValue("IsEasyDentalSyncing", false);
                //        }
                //        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                //        {
                //            key1.SetValue("IsEasyDentalPatinetSyncing", false);
                //        }
                //        continue;
                //    }

                //}
                //System.Environment.Exit(1);
                Utility.RestartApp();
            }
            catch (Exception)
            {
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                frmAditModuleSyncConfig Obj_AditModuleSyncConfig = new frmAditModuleSyncConfig();
                Obj_AditModuleSyncConfig.ShowDialog();

                if (Obj_AditModuleSyncConfig.StatusAditSyncProcess)
                {
                    ObjGoalBase.WriteToSyncLogFile("[btnConfig_Click] manually Application restart");
                    KillEXE();
                    System.Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Adit Sync" + ex.Message);
                //  ObjGoalBase.ErrorMsgBox("Adit Sync", ex.Message);
            }
        }

        private void lblConnectionLog_Click(object sender, EventArgs e)
        {
            try
            {
                bool isfileopen = false;
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string ErrorLogFilePath = Application.StartupPath + "\\" + "ErrorLogFile" + "\\" + dtCurrentDtTime.ToString("yyyy/MM").Replace("/", "\\");
                string ErrorLogFileName = ErrorLogFilePath + "\\" + dtCurrentDtTime.ToString("yyyy_MM_dd") + ".txt";
                Process[] processes = Process.GetProcessesByName("notepad");
                for (int i = 0; i < processes.Length; i++)
                {
                    if (processes[i].MainWindowTitle.Equals(dtCurrentDtTime.ToString("yyyy_MM_dd") + " - Notepad"))
                    {
                        isfileopen = true;
                        break;
                    }
                }
                if (File.Exists(ErrorLogFileName) && !isfileopen)
                {
                    Process.Start("notepad.exe", ErrorLogFileName);
                }
                // Cursor.Current = Cursors.WaitCursor;
                //txtConnectionLog.Text = GoalBase.ConnectionLog;
                // tmrConnectionLog_Tick(null, null);
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void picAppUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ApplicationManuallyUpdate = true;
                CheckApplicationUpdateVersion();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPatient_MouseEnter(object sender, EventArgs e)
        {
            TTPView.SetToolTip(btnPatient, "Every " + Convert.ToInt64(GoalBase.intervalEHRSynch_Patient) / 60 + " minute " + lblPatient.Text + " will be sync " + Utility.Application_Name + " to Local server.");
        }

        private void btnSyncPatient_MouseEnter(object sender, EventArgs e)
        {
            TTPView.SetToolTip(btnSyncPatient, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_Patient) / 60 + " minute " + lblPatient.Text + " will be sync Local server to Adit App server.");
        }

        private void btnSyncOperatoryEvent_MouseEnter(object sender, EventArgs e)
        {
            TTPView.SetToolTip(btnSyncOperatoryEvent, "Every " + Convert.ToInt64(GoalBase.intervalWebSynch_Push_OperatoryEvent) / 60 + " minute " + lblOperatoryEvent.Text + " will be sync Local server to Adit App server. (" + PushOperatoryEventRecord.ToString() + "/" + TotalPushOperatoryEventRecord.ToString() + ")");
        }


        #endregion

        #region Common Event

        private void Pozative_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Show();
                this.WindowState = FormWindowState.Normal;
                NotifyIconPozative.Visible = false;
                this.BringToFront();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Pozative_MouseDoubleClick]" + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void tmrConnectionLog_Tick(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //if (Utility.Application_ID == 1)
                //{
                //    CheckSystemAndApplicaitonConnection();
                //}
                // CheckSystemAndApplicaitonConnection(true);
                CheckTableConnection();
                //LoadAppointmentRecord();
                if (Utility.Application_StartDate.Date != Utility.Datetimesetting().Date)
                {
                    //ObjGoalBase.WriteToSyncLogFile("[ConnectionLog] Day end application restart for clear cache");
                    //System.Environment.Exit(1);

                    try
                    {
                        // fncAditLocationSyncEnable();
                        Utility.Application_StartDate = Utility.Datetimesetting();
                    }
                    catch (Exception)
                    {

                    }
                    //txtConnectionLog.Text = GoalBase.ConnectionLog;
                }
                if (Utility.Application_ID == 4 || Utility.Application_ID == 8)
                {
                    // txtConnectionLog.Text = GoalBase.ConnectionLog;

                    string SyncLogFilePath = Application.StartupPath + "\\" + "SyncLogFile" + "\\" + Utility.Datetimesetting().ToString("yyyy/MM").Replace("/", "\\");
                    string SyncLogFileName = SyncLogFilePath + "\\" + Utility.Datetimesetting().ToString("yyyy_MM_dd") + ".txt";

                    string strConnectionLog = "";
                    try
                    {
                        using (StreamReader sr = new StreamReader(SyncLogFileName))
                        {
                            strConnectionLog = sr.ReadToEnd();
                        }
                    }
                    catch (Exception)
                    {

                    }
                    if (strConnectionLog.Contains("System.OutOfMemoryException"))
                    {
                        //string[] t = Directory.GetFiles(Environment.CurrentDirectory, "Pozative.sdf");
                        //Array.ForEach(t, File.Delete);
                        //Utility.Get_LocalDbFromWeb();
                        //Utility.UpdateLocalDB_From_BackupDB();
                        Utility.ResolvedOutofMemeoryException();
                        Utility.RestartApp();
                    }
                    //txtConnectionLog.Text = strConnectionLog;
                    // txtConnectionLog.SelectionStart = txtConnectionLog.Text.Length;
                    // txtConnectionLog.ScrollToCaret();
                }
                else
                {
                    if (GoalBase.ConnectionLog.Contains("System.OutOfMemoryException"))
                    {
                        //string[] t = Directory.GetFiles(Environment.CurrentDirectory, "Pozative.sdf");
                        //Array.ForEach(t, File.Delete);
                        //Utility.Get_LocalDbFromWeb();
                        //Utility.UpdateLocalDB_From_BackupDB();
                        Utility.ResolvedOutofMemeoryException();
                        Utility.RestartApp();
                    }
                    // txtConnectionLog.Text = GoalBase.ConnectionLog;
                }


            }
            catch (Exception ex)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void tblHead_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[tblHead_MouseDown]" + ex.Message);
            }
        }

        private void lblHeading_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[tblHead_MouseDown]" + ex.Message);
            }
        }

        private void Pozative_Resize(object sender, EventArgs e)
        {
            try
            {
                if (FormWindowState.Minimized == this.WindowState)
                {
                    NotifyIconPozative.Visible = true;
                    this.Hide();
                }
                else if (FormWindowState.Normal == this.WindowState)
                {
                    NotifyIconPozative.Visible = false;
                    lblHead.Focus();
                    lblHead.Select();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Pozative_Resize]" + ex.Message);
            }
        }

        private void frmPozative_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                e.Cancel = true;
                NotifyIconPozative.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region "EventListener"

        private void StartAndInstallAditEventListener()
        {
            bool installService = true;
            try
            {
                Utility.WriteToSyncLogFile_All("Start StartAndInstallAditEventListener.");
                //HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AditEventListener
                var regAditEventListener = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\" + Utility.sEventServiceName, "ImagePath", null);
                if (regAditEventListener != null)
                {
                    Utility.WriteToSyncLogFile_All("StartAndInstallAditEventListener Service Found.");
                    string strEventPath = System.IO.Path.GetDirectoryName(regAditEventListener.ToString()) + "\\";
                    if (strEventPath.ToUpper() != AppDomain.CurrentDomain.BaseDirectory.ToString().ToUpper())
                    {
                        Utility.WriteToSyncLogFile_All("StartAndInstallAditEventListener Service Path does not match.");
                        using (System.ServiceProcess.ServiceController AdSc = new System.ServiceProcess.ServiceController(Utility.sEventServiceName))
                        {
                            if (AdSc.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                            {
                                Utility.WriteToSyncLogFile_All("StartAndInstallAditEventListener Stopping service.");
                                AdSc.Stop();
                            }
                            AdSc.Dispose();
                        }

                        #region Call Exe to uninstall service
                        string exePath = Application.StartupPath.EndsWith("\\") ? Application.StartupPath + "InstallWinService.exe" : Application.StartupPath + "\\InstallWinService.exe";  //@"C:\\program.exe";
                        using (Process myProcess = new Process())
                        {
                            myProcess.StartInfo.UseShellExecute = false;
                            myProcess.StartInfo.FileName = exePath;
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.StartInfo.Verb = "runas";
                            myProcess.StartInfo.Arguments = "U#Adit#" + Utility.sEventServiceName;
                            Utility.WriteToErrorLogFromAll("StartAndInstallAditEventListener Uninstalling service.");
                            myProcess.Start();
                            myProcess.WaitForExit();
                        }
                        #endregion
                        Utility.WriteToErrorLogFromAll("StartAndInstallAditEventListener Service Uninstall done.");
                        //UnInstallService(svcName);
                    }
                    else
                    {
                        installService = false;
                    }
                }


                if (installService)
                {
                    #region Call Exe to install service
                    string strEventExePath = Application.StartupPath.EndsWith("\\") ? Application.StartupPath + Utility.sEventServiceName + ".exe" : Application.StartupPath + "\\" + Utility.sEventServiceName + ".exe";
                    string exePath = Application.StartupPath.EndsWith("\\") ? Application.StartupPath + "InstallWinService.exe" : Application.StartupPath + "\\InstallWinService.exe";  //@"C:\\program.exe";
                    string UserPass = "", UserName = "";
                    try
                    {
                        for (int i = 0; i <= Utility.DtInstallServiceList.Rows.Count - 1; i++)
                        {
                            if (!string.IsNullOrEmpty(Utility.DtInstallServiceList.Rows[i]["AditUserEmailID"].ToString()))
                            {
                                UserName = Utility.DtInstallServiceList.Rows[i]["AditUserEmailID"].ToString();
                                if (!string.IsNullOrEmpty(Utility.DtInstallServiceList.Rows[i]["AditUserEmailPassword"].ToString()))
                                {
                                    UserPass = Utility.DtInstallServiceList.Rows[i]["AditUserEmailPassword"].ToString();
                                    break;
                                }
                                else
                                {
                                    UserName = "";
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        UserName = ""; UserPass = "";
                        Utility.WriteToErrorLogFromAll("Error in [StartAndInstallAditEventListener] Error: Getting UserName and Password: " + ex.Message);
                    }

                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = exePath;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.StartInfo.Verb = "runas";
                        try
                        {
                            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserPass))
                            {
                                if (!string.IsNullOrEmpty(Utility.DecryptString(UserName)) && !string.IsNullOrEmpty(Utility.DecryptString(UserPass)))
                                {
                                    myProcess.StartInfo.Arguments = "I#Adit#" + Utility.sEventServiceName + "#Adit#" + @"""" + strEventExePath + @""""
                                                               + "#Adit#" + System.Environment.MachineName + "\\" + Utility.DecryptString(UserName) + "#Adit#" + Utility.DecryptString(UserPass);
                                }
                                else
                                {
                                    myProcess.StartInfo.Arguments = "I#Adit#" + Utility.sEventServiceName + "#Adit#" + @"""" + strEventExePath + @"""";
                                }
                            }
                            else
                            {
                                myProcess.StartInfo.Arguments = "I#Adit#" + Utility.sEventServiceName + "#Adit#" + @"""" + strEventExePath + @"""";
                            }
                        }
                        catch (Exception exUser)
                        {
                            Utility.WriteToErrorLogFromAll("StartAndInstallAditEventListener Error (" + exUser.Message + " " + exUser.StackTrace + ") in getting user so assigning local system to service.");
                            myProcess.StartInfo.Arguments = "I#Adit#" + Utility.sEventServiceName + "#Adit#" + @"""" + strEventExePath + @"""";
                        }


                        Utility.WriteToErrorLogFromAll("StartAndInstallAditEventListener Service Installation Start.");
                        myProcess.Start();
                        myProcess.WaitForExit();
                        Utility.WriteToErrorLogFromAll("StartAndInstallAditEventListener Service Installation End.");
                    }
                    #endregion
                }

                try
                {
                    ServiceController sc = new ServiceController();
                    sc.ServiceName = Utility.sEventServiceName;
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        Utility.WriteToSyncLogFile_All("StartAndInstallAditEventListener Service Start One.");
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running);
                    }
                }
                catch
                {
                }

                try
                {
                    using (System.ServiceProcess.ServiceController AdSc = new System.ServiceProcess.ServiceController(Utility.sEventServiceName))
                    {
                        if (AdSc.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            Utility.WriteToSyncLogFile_All("StartAndInstallAditEventListener Service Start Two.");
                            AdSc.Start();
                        }
                        AdSc.Dispose();
                    }
                }
                catch
                {
                }
                Utility.WriteToSyncLogFile_All("StartAndInstallAditEventListener End.");
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("Error in [StartAndInstallAditEventListener]. Error:" + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            //string strEventExePath = "";
            //ServiceInstaller Svc;
            //try
            //{
            //    strEventExePath = Application.StartupPath.EndsWith("\\") ? Application.StartupPath + "AditEventListener.exe" : Application.StartupPath + "\\AditEventListener.exe";  //@"C:\\program.exe";
            //    Svc = new ServiceInstaller();
            //    Svc.InstallService(strEventExePath, "AditEventListener", "AditEventListener");
            //    //Process runttk = new Process();
            //    //runttk.StartInfo.UseShellExecute = true;
            //    //runttk.StartInfo.FileName =  Application.StartupPath.EndsWith("\\") ? Application.StartupPath + "AditEventListener.exe" : Application.StartupPath + "\\AditEventListener.exe";  //@"C:\\program.exe";
            //    //runttk.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //    //runttk.Start();
            //    //this.Focus();
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private void StopAndUnInstallAditEventListener()
        {
            try
            {
                using (System.ServiceProcess.ServiceController AdSc = new System.ServiceProcess.ServiceController(Utility.sEventServiceName))
                {
                    if (AdSc.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                    {
                        AdSc.Stop();
                    }
                    AdSc.Dispose();
                }

                #region Call Exe to uninstall service
                string exePath = Application.StartupPath.EndsWith("\\") ? Application.StartupPath + "InstallWinService.exe" : Application.StartupPath + "\\InstallWinService.exe";  //@"C:\\program.exe";
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = exePath;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.Verb = "runas";
                    myProcess.StartInfo.Arguments = "U#Adit#" + Utility.sEventServiceName;
                    myProcess.Start();
                    myProcess.WaitForExit();
                }
                #endregion
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Appointment Gridview event

        private void grdAppointment_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                var dataGridView = sender as DataGridView;
                if (dataGridView.Rows[e.RowIndex].Selected)
                {
                    e.CellStyle.SelectionBackColor = Color.LightGray;
                }
            }
            catch (Exception)
            {
            }
        }

        private void grdAppointment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (e.RowIndex >= 0)
                {

                    if (grdAppointment.Columns[e.ColumnIndex].Name.ToString() == "ViewHere")
                    {
                        frmAppointmentDetail ApptDtl = new frmAppointmentDetail
                                                    (grdAppointment.CurrentRow.Cells["Patient_Name"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Home_Contact"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Mobile_Contact"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Email"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Address"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["City"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Zip"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["ST"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Appt_DateTime"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Appt_DateTime"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Operatory_Name"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Provider_Name"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["ApptType"].Value.ToString(),
                                                     grdAppointment.CurrentRow.Cells["Appointment_Status"].Value.ToString()
                                                    );
                        ApptDtl.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Appointment View" + ex.Message);
                // ObjGoalBase.ErrorMsgBox("Appointment View", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Timer Event

        private void startGetPatientRecord_Tick(object sender, EventArgs e)
        {
            try
            {
                startGetPatientRecord.Enabled = false;
                startGetPatientRecord.Stop();

                switch (Utility.Application_ID)
                {
                    case 1:
                        CallSynchEaglesoftToLocal();
                        if (!Utility.IsExternalAppointmentSync)
                        {
                            fncSynchDataEaglesoft_Patient();
                            fncSynchDataEaglesoft_Appointment();
                        }
                        else
                        {
                            SyncPatientRecordsInitialy();
                            SyncAppointmentRecordsInitialy();
                        }
                        break;
                    case 2:
                        if (!Utility.IsExternalAppointmentSync)
                        {

                            fncSynchDataOpenDental_Patient();
                            fncSynchDataOpenDental_OperatoryEvent();
                            fncSynchDataOpenDental_Appointment();
                        }
                        else
                        {
                            SyncPatientRecordsInitialy();
                            SyncAppointmentRecordsInitialy();
                        }
                        break;
                    case 3:
                        if (!Utility.IsExternalAppointmentSync)
                        {
                            fncSynchDataDentrix_Appointment();
                            fncSynchDataDentrix_Patient();
                            fncSynchDataDentrix_OperatoryEvent();
                        }
                        else
                        {
                            SyncPatientRecordsInitialy();
                            SyncAppointmentRecordsInitialy();
                        }
                        break;
                    case 5:
                        if (!Utility.IsExternalAppointmentSync)
                        {
                            fncSynchDataClearDent_Appointment();
                            fncSynchDataClearDent_Patient();
                        }
                        else
                        {
                            SyncPatientRecordsInitialy();
                            SyncAppointmentRecordsInitialy();
                        }
                        break;
                    case 6:
                        //CallSynchTrackerToLocal();
                        if (!Utility.IsExternalAppointmentSync)
                        {
                            fncSynchDataTracker_Appointment();
                            fncSynchDataTracker_Patient();
                        }
                        else
                        {
                            SyncPatientRecordsInitialy();
                            SyncAppointmentRecordsInitialy();
                        }
                        break;
                    case 7:
                        CallSynchPracticeWorkToLocal();
                        break;
                    case 8:
                        SyncEasyDentalPatientPaymentRecordsInitialy();
                        SyncEasyDentalRecordsInitialy();
                        SyncEasyDentalPatientRecordsInitialy();


                        break;
                    case 10:
                        if (!Utility.IsExternalAppointmentSync)
                        {

                            fncSynchDataPracticeWeb_Patient();
                            fncSynchDataPracticeWeb_OperatoryEvent();
                            fncSynchDataPracticeWeb_Appointment();
                        }
                        else
                        {
                            SyncPatientRecordsInitialy();
                            SyncAppointmentRecordsInitialy();
                        }
                        break;
                    case 11:
                        if (!Utility.IsExternalAppointmentSync)
                        {
                            fncSynchDataAbelDent_Appointment();
                            fncSynchDataAbelDent_Patient();
                        }
                        else
                        {
                            SyncPatientRecordsInitialy();
                            SyncAppointmentRecordsInitialy();
                        }
                        break;
                    case 12:
                        SyncCrystalPMPatientPaymentRecordsInitialy();
                        SyncCrystalPMRecordsInitialy();
                        SyncCrystalPMPatientRecordsInitialy();
                        break;
                    case 13:
                        if (!Utility.IsExternalAppointmentSync)
                        {
                            //fncSynchDataOfficeMate_Patient();
                            //fncSynchDataOfficeMate_Appointment();
                        }
                        else
                        {
                            SyncPatientRecordsInitialy();
                            SyncAppointmentRecordsInitialy();
                        }
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        int TokenTime = 0;
        private void tmrApplicationIdleTime_Tick(object sender, EventArgs e)
        {
            try
            {
                TimeSpan StartTime = Utility.AppIdleStartTime.TimeOfDay;
                TimeSpan EndTime = Utility.AppIdleStopTime.TimeOfDay;
                TimeSpan CurrentTime = Convert.ToDateTime(DateTime.Now).TimeOfDay;
                if (Utility.IsApplicationIdleTimeSet)
                {
                    if (StartTime <= CurrentTime && EndTime >= CurrentTime)
                    {
                        Utility.IsApplicationIdleTimeOff = false;
                    }
                    else
                    {
                        Utility.IsApplicationIdleTimeOff = true;
                    }
                }
                else
                {
                    Utility.IsApplicationIdleTimeOff = true;
                }



            }
            catch (Exception)
            {
            }
        }

        //private void tmrPaymentSMSCallLog_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        CheckCustomhoursForProviderOperatory();
        //        // Utility.WriteToSyncLogFile_All("Start Sync Log Last Log Synced At : " + Utility.LastPaymentSMSCallSyncDateAditServer.ToString() + " and current datetime is " + DateTime.Now.ToString());
        //        //if (Utility.LastPaymentSMSCallSyncDateAditServer <= DateTime.Now)
        //        //{
        //        Utility.WriteToSyncLogFile_All("Start Sync Log");
        //        //SynchDataLiveDB_Pull_PatientPaymentLog();
        //        SynchDataLiveDB_Pull_PatientPaymentSMSCall();
        //        SynchDataLiveDB_Pull_PatientFollowUp();

        //        switch (Utility.Application_ID)
        //        {
        //            case 1:
        //                //SynchDataLocalToEagleSoft_Patient_Payment();
        //                SynchDataLocalToEagleSoft_Patient_SMSCallLog();
        //                break;
        //            case 2:
        //                //SynchDataPatientPayment_LocalTOOpenDental();
        //                //SynchDataLocalToOpenDental_PatientPayment();
        //                SynchDataPatientSMSCall_LocalTOOpenDental();
        //                break;
        //            case 3:
        //                //SynchDataLocalToDentrix_Patient_Payment();
        //                SynchDataLocalToDentrix_Patient_SMSCallLog();
        //                break;
        //            case 5:
        //                //SynchDataLiveDB_PatientPaymentLog_LocalTOClearDent();
        //                SynchDataLiveDB_PatientSMSCallLog_LocalTOClearDent();
        //                break;
        //            case 6:
        //                //SynchDataLiveDB_PatientPayment_LocalToTracker();
        //                SynchDataLiveDB_PatientSMSCall_LocalToTracker();
        //                break;
        //            case 8:
        //                //SynchDataLocalToEasyDental_Patient_Payment();
        //                SynchDataLocalToEasyDental_Patient_SMSCallLog();
        //                break;
        //            case 10:
        //                //SynchDataPatientPayment_LocalTOOpenDental();
        //                //SynchDataLocalToOpenDental_PatientPayment();
        //                SynchDataPatientSMSCall_LocalTOPracticeWeb();
        //                break;
        //            case 11:
        //                SynchDataLiveDB_PatientSMSCallLog_LocalToAbelDent();
        //                break;
        //                //case 7:
        //                //    ///CallSynchPracticeWorkToLocal();
        //                //    break;                       
        //        }
        //        fncPaymentSMSCallStatusUpdate();
        //        SynchLocalBAL.UpdateWebPatientPaymentDataErroAPI();
        //        SynchLocalBAL.UpdateWebPatientSMSCallDataErroAPI();
        //        // }

        //        //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //        //DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        //        //int hour = indianTime.Hour;

        //        //if (hour >= 13 && hour < 14)
        //        //{
        //        //    SynchDataLiveDB_Pull_DeletedAppointmentFromWeb();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjGoalBase.WriteToErrorLogFile("Update SMS Call Log : " + ex.Message);
        //    }
        //}

        private static void fncPaymentSMSCallStatusUpdate()
        {
            try
            {
                for (int j = 0; j < Utility.DtLocationList.Rows.Count; j++)
                {

                    AditPaymentSMSCallUpdateStatus Ps = new AditPaymentSMSCallUpdateStatus();
                    Ps.location = Utility.DtLocationList.Rows[j]["Location_Id"].ToString();
                    Ps.created_by = Utility.User_ID;
                    Ps.log_sync_date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    string strApiAditLocationSyncEnable = SystemBAL.AditPaymentSMSCallStatusUpdate(Utility.DtLocationList.Rows[j]["Location_Id"].ToString(), Utility.User_ID);
                    var client = new RestClient(strApiAditLocationSyncEnable);
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[j]["Location_Id"].ToString()));
                    var JsonPatient = new System.Text.StringBuilder();
                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    JsonPatient.Append(javaScriptSerializer.Serialize(Ps));
                    request.AddParameter("application/json", JsonPatient, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.ErrorMessage != null)
                    {
                        GoalBase.WriteToErrorLogFile_Static("[PaymentSMSCallStatusUpdate] : " + response.ErrorMessage);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                GoalBase.WriteToErrorLogFile_Static("[PaymentSMSCallStatusUpdate] : " + ex.Message);
            }
        }

        private bool GetWebAdminUserToken()
        {

            Utility.WebAdminUserToken = "";
            return true;
            //string strAditLogin = SystemBAL.GetAdminUserLoginEmailIdPass();
            //AditLoginPostBO AditLoginPost = new AditLoginPostBO
            //{
            //    email = Utility.Adit_User_Email_Id,
            //    password = Utility.Adit_User_Email_Password

            //};
            //var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            //string jsonString = javaScriptSerializer.Serialize(AditLoginPost);
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            //var clientLogin = new RestClient(strAditLogin);
            //var requestLogin = new RestRequest(Method.POST);
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //requestLogin.AddHeader("Postman-Token", "1d16df4c-48ba-4644-bc7a-9bcef2a86744");
            //requestLogin.AddHeader("cache-control", "no-cache");
            //requestLogin.AddHeader("cache-control", "no-cache");
            //requestLogin.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            //IRestResponse responseLogin = clientLogin.Execute(requestLogin);
            //if (responseLogin.ErrorMessage != null)
            //{
            //    ObjGoalBase.WriteToErrorLogFile("Server is down. Please try again after a few minutes.");
            //    return false;
            //}
            //var AdminLoginDto = JsonConvert.DeserializeObject<AdminLoginDetailBO>(responseLogin.Content);
            //if (AdminLoginDto == null)
            //{
            //    ObjGoalBase.WriteToErrorLogFile(responseLogin.ErrorMessage.ToString());
            //    return false;
            //}
            //if (AdminLoginDto.status == "true")
            //{
            //    Utility.WebAdminUserToken = AdminLoginDto.Token;
            //    TokenTime = 0;
            //    return true;
            //}
            //else
            //{
            //    ObjGoalBase.WriteToErrorLogFile(AdminLoginDto.message.ToString());
            //    return false;
            //}
        }

        private void tmrConsoleRun_Tick(object sender, EventArgs e)
        {
            try
            {
                bool IsPozativeExeRunning = false;
                bool IsExeRun = false;
                bool IsSyncServerApp = false;
                string fileName = string.Empty;
                try
                {
                    IsPozativeExeRunning = false;
                    IsExeRun = false;
                    IsSyncServerApp = false;
                    fileName = string.Empty;
                    foreach (Process p in Process.GetProcesses())
                    {
                        fileName = p.ProcessName;
                        if (string.Compare("PozativeExeRun".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                        {
                            //ObjGoalBase.WriteToSyncLogFile("PozativeExeRun is already running");
                            IsPozativeExeRunning = true;
                        }
                        if (string.Compare("ExeRun".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                        {
                            IsExeRun = true;
                        }
                        if (string.Compare("SyncServerAppCheck".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                        {
                            p.Kill();// IsSyncServerApp = true;
                        }
                        if (string.Compare("AditAppSyncLog".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                        {
                            p.Kill();
                        }
                    }
                }
                catch (Exception ex2)
                {
                    Utility.WriteToErrorLogFromAll("PozativeHome_Err " + ex2.Message.ToString());
                }

                if (IsPozativeExeRunning == false)
                {
                    //ObjGoalBase.WriteToSyncLogFile("PozativeExeRun is not running");
                    string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    // var directory = System.IO.Path.GetDirectoryName(path);

                    var directory = Application.StartupPath;
                    try
                    {
                        if (File.Exists(directory + "\\PozativeExeRun.exe"))
                        {
                            Process.Start(directory + "\\PozativeExeRun.exe");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                //if (IsSyncFileTransfer == false)
                //{
                //    //string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

                //    //var directory = Application.StartupPath;
                //    //try
                //    //{
                //    //    if (File.Exists(directory + "\\AditAppSyncLog" + "\\AditAppSyncLog.exe"))
                //    //    {
                //    //        Process.Start(directory + "\\AditAppSyncLog" + "\\AditAppSyncLog.exe");
                //    //    }
                //    //}
                //    //catch (Exception)
                //    //{
                //    //}
                //    var directory = Application.StartupPath;
                //    try
                //    {
                //        //string exePath = Path.Combine(directory, "AditAppSyncLog", "AditAppSyncLog.exe");

                //        //if (File.Exists(exePath))
                //        //{
                //        //    using (Process process = new Process())
                //        //    {
                //        //        process.StartInfo.FileName = exePath;

                //        //        process.StartInfo.CreateNoWindow = true;
                //        //        process.StartInfo.UseShellExecute = false;
                //        //        process.StartInfo.RedirectStandardOutput = true;
                //        //        process.StartInfo.Arguments = "NoCall"; // File will not send on find the time

                //        //        process.Start();
                //        //        while (!process.StandardOutput.EndOfStream)
                //        //        {
                //        //            string line = process.StandardOutput.ReadLine();

                //        //            if (line.IndexOf("AWS Transfer Date : ") > -1)
                //        //            {
                //        //                dtAws = Convert.ToDateTime(line.Replace("AWS Transfer Date : ", "").Trim());
                //        //                break;
                //        //            }
                //        //        }

                //        //        IsSyncFileTransfer = true;
                //        //        process.Kill();
                //        //    }
                //        //}
                //    }
                //    catch (Exception ex)
                //    {

                //    }

                //}


                //if (DateTime.Now >= dtAws)
                //{
                //    IsSyncFileTransfer = false;
                //    var directory = Application.StartupPath;

                //    try
                //    {
                //        string exePath = Path.Combine(directory, "AditAppSyncLog", "AditAppSyncLog.exe");

                //        if (File.Exists(exePath))
                //        {
                //            using (Process process = new Process())
                //            {
                //                process.StartInfo.FileName = exePath;

                //                process.StartInfo.CreateNoWindow = true;
                //                process.StartInfo.UseShellExecute = false;
                //                process.StartInfo.RedirectStandardOutput = true;
                //                process.StartInfo.Arguments = "Call"; // Replace with your actual command line arguments

                //                process.Start();
                //                Task.Delay(10000).Wait();
                //                process.Kill();


                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                if (IsExeRun == false)
                {
                    string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    // var directory = System.IO.Path.GetDirectoryName(path);
                    var directory = Application.StartupPath;
                    try
                    {
                        if (File.Exists(directory + "\\ExeRun.exe"))
                        {
                            Process.Start(directory + "\\ExeRun.exe");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (IsSyncServerApp == false)
                {
                    //string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                    //// var directory = System.IO.Path.GetDirectoryName(path);
                    //var directory = Application.StartupPath;
                    //try
                    //{
                    //    if (File.Exists(directory + "\\SyncServerAppCheck.exe"))
                    //    {
                    //        Process.Start(directory + "\\SyncServerAppCheck.exe");
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //}
                }
                //if (Utility.DBConnString == string.Empty || Utility.DBConnString == "")
                //{
                //    GetConnectionString();
                //}
                if (TokenTime < 15)
                {
                    TokenTime += 1;
                }
                else
                {
                    if (GetWebAdminUserToken())
                    {
                        TokenTime = 0;
                    }

                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[tmrConsoleRun_Tick]" + ex.Message);
            }
        }

        private void tmrAditLocationSyncEnable_Tick(object sender, EventArgs e)
        {
            try
            {
                GoalBase.ConnectionLog = "";

                CheckLocationTimeZoneWithSystemTimeZone();
                CheckSystemAndApplicaitonConnection(true);
                bool tmpAditLocationSyncEnable = Utility.AditLocationSyncEnable;
                bool tmpApptAutoBook = Utility.ApptAutoBook;
                bool tmpSyncPracticeAnalytics = Utility.SyncPracticeAnalytics;

                fncAditLocationSyncEnable();



                //if (Utility.AditLocationSyncEnable != tmpAditLocationSyncEnable)
                //{
                //    ObjGoalBase.WriteToSyncLogFile("[AditLocationSyncEnable] AditLocationSyncEnable flag change");
                //    System.Environment.Exit(1);
                //}
                //if (Utility.ApptAutoBook != tmpApptAutoBook)
                //{
                //    ObjGoalBase.WriteToSyncLogFile("[AditLocationSyncEnable] ApptAutoBook flag change");
                //    System.Environment.Exit(1);
                //}
                if (Utility.SyncPracticeAnalytics != tmpSyncPracticeAnalytics)
                {
                    ObjGoalBase.WriteToSyncLogFile("[AditLocationSyncEnable] SyncPracticeAnalytics flag change");
                    System.Environment.Exit(1);
                }

                #region Restart Application with Single Use CPU is application consume more then 50%

                try
                {

                    PerformanceCounter total_cpu = new PerformanceCounter("Process", "% Processor Time", "_Total");
                    PerformanceCounter process_cpu = new PerformanceCounter("Process", "% Processor Time", "Pozative");

                    float t = total_cpu.NextValue();
                    float p = process_cpu.NextValue();

                    if (p > 80)
                    {
                        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\UseSingleCPU");
                        key.SetValue("Value", true);
                        System.Environment.Exit(1);
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("Find CPU Process % " + ex.Message);
                }
                #endregion


            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Private Methods

        public void GetEHRDocPath()
        {
            try
            {
                if (Utility.EHRDocPath == string.Empty || Utility.EHRDocPath == "")
                {
                    if (Utility.Application_Name.ToLower() == "Eaglesoft".ToLower())
                    {
                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            EagleSoftDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }
                    }

                    //else if (Utility.Application_Name.ToLower() == "Dentrix".ToLower())
                    //{
                    //    DentrixConfiguration();
                    //}
                    //else if (Utility.Application_Name.ToLower() == "SoftDent".ToLower())
                    //{
                    //    SoftDentConfigurtion();
                    //}
                    //else 
                    if (Utility.Application_Name.ToLower() == "ClearDent".ToLower())
                    {
                        ClearDentDocPath();
                    }
                    else if (Utility.Application_Name.ToLower() == "Open Dental".ToLower() || (Utility.Application_Name.ToLower() == "OpenDental Cloud".ToLower()))
                    {
                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            OpenDentalDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }
                    }
                    else if (Utility.Application_Name.ToLower() == "Tracker".ToLower())
                    {
                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            TrackerDocPath(Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }
                    }
                    else if (Utility.Application_Name.ToLower() == "AbelDent".ToLower())
                    {
                        for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
                        {
                            AbelDentDocPath(Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString(), Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        #region ClearDentDocPath
        private void ClearDentDocPath()
        {
            string Connectionstring = "";
            if (Utility.EHRPassword.Trim() == "")
            {
                Connectionstring = "Initial Catalog=ClearDent_Cfg;Data Source=" + Utility.EHRHostname + ";User ID=" + Utility.EHRUserId + ";";
            }
            else
            {
                Connectionstring = "Initial Catalog=ClearDent_Cfg;Data Source=" + Utility.EHRHostname + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
            }
            bool IsEHRConnected = SystemBAL.GetEHRClearDentConnection();

            if (IsEHRConnected)
            {
                Utility.EHRDocPath = SynchClearDentBAL.GetClearDentDocPath(Connectionstring);
                SystemBAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, "1");
            }
            else
            {
                ObjGoalBase.WriteToErrorLogFile("EHR Doc Path : patient Document Path Not Found.");
            }
        }
        #endregion

        #region OpenDentalDocPath
        public void OpenDentalDocPath(string DbString, string Service_Install_Id)
        {

            bool IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(DbString);

            if (IsEHRConnected)
            {
                Utility.EHRDocPath = SynchOpenDentalBAL.GetOpenDentalDocPath(DbString);
                SystemBAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, Service_Install_Id);
            }
            else
            {
                ObjGoalBase.WriteToErrorLogFile("EHR Doc Path : Patient Document Path Not Found.");
            }
        }
        #endregion

        #region PracticeWebDocPath
        public void PracticeWebDocPath(string DbString, string Service_Install_Id)
        {

            bool IsEHRConnected = SystemBAL.GetEHRPracticeWebConnection(DbString);

            if (IsEHRConnected)
            {
                Utility.EHRDocPath = SynchPracticeWebBAL.GetPracticeWebDocPath(DbString);
                SystemBAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, Service_Install_Id);
            }
            else
            {
                ObjGoalBase.WriteToErrorLogFile("EHR Doc Path : Patient Document Path Not Found.");
            }
        }
        #endregion
        #region EagleSoftDocPath
        public void EagleSoftDocPath(string DbString, string Service_Install_Id)
        {
            try
            {
                Utility.EHRDocPath = SynchEaglesoftBAL.GetEagleSoftDocPath(DbString);
                SystemBAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, Service_Install_Id);
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("EHR Doc Path : Patient Document Path Not Found." + ex.Message);
            }
        }
        #endregion
        #region AbelDentDocPath
        public void AbelDentDocPath(string DbString, string Service_Install_Id)
        {

            try
            {
                //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ABELSoft\Licensing\1.00
                string destPatientDocPath = "";
                string DocPath = "";
                if (Utility.Application_Version == "15")
                {
                    DocPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ABELSoft\ABELDent", "ProductPath", null);
                }
                else
                {
                    DocPath = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\ABELSoft\ABELDent", "ProductPath", null);
                }
                if (DocPath != null)
                {
                    if (Utility.Application_Version == "15")
                    {
                        destPatientDocPath = DocPath + @"\LocalStorage\Documents";
                    }
                    else
                    {
                        destPatientDocPath = DocPath + @"\Data\DOCUMENTS";
                    }
                }
                if (!string.IsNullOrEmpty(destPatientDocPath))
                {
                    Utility.EHRDocPath = destPatientDocPath;
                    SystemBAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, Service_Install_Id);
                }
                else
                {
                    ObjGoalBase.WriteToErrorLogFile("EHR Doc AbelDent Path : Patient Document Path Not Found. DocPath : '" + DocPath + "', destPatientDocPath : '" + destPatientDocPath + "'");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("EHR Doc Path : Patient Document Path Not Found." + ex.Message);
            }
        }
        #endregion
        #region TrackerDocPath
        private void TrackerDocPath(string Service_Install_Id)
        {
            try
            {

                bool regmismatch = false;
                string destPatientDocPath = "";
                bool EHROption_PatDoc_set = false;
                string PatientDocsLocationPath = "";
                string DocPath = "";

                DataTable dtTrackerPatientDocsLocationPath = SynchTrackerBAL.GetTrackerPatDocLocation();
                if (dtTrackerPatientDocsLocationPath != null)
                {
                    if (dtTrackerPatientDocsLocationPath.Rows.Count > 0)
                    {
                        PatientDocsLocationPath = dtTrackerPatientDocsLocationPath.Rows[0]["ApplicationOptionValue"].ToString().Trim();
                        if (PatientDocsLocationPath != "")
                        {
                            DocPath = PatientDocsLocationPath.ToString().Trim();
                            EHROption_PatDoc_set = true;
                        }
                        else
                        {
                            DocPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\ENVIRONMENT", "TrackerMedia", null);
                        }

                    }
                }
                if (EHROption_PatDoc_set)
                {
                    if (DocPath != null)
                    {
                        DataTable dtService_InstallData = SystemDAL.GetInstallServeiceDetail();
                        if (dtService_InstallData != null)
                        {
                            if (dtService_InstallData.Rows.Count > 0)
                            {
                                destPatientDocPath = DocPath;
                                //if (dtService_InstallData.Rows[0]["Document_Path"].ToString().ToLower() != destPatientDocPath.ToString().ToLower())
                                //{
                                //    regmismatch = true;
                                //}
                            }
                            if (!string.IsNullOrEmpty(destPatientDocPath))
                            {
                                Utility.EHRDocPath = destPatientDocPath;
                                SystemBAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, Service_Install_Id);
                            }
                            else
                            {
                                ObjGoalBase.WriteToErrorLogFile("EHR Doc Path : Patient Document Path Not Found. DocPath : '" + DocPath + "', destPatientDocPath : '" + destPatientDocPath + "'");
                            }
                        }
                    }
                }
                else
                {
                    // string destPatientDocPath = "";
                    DocPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\ENVIRONMENT", "TrackerMedia", null);
                    if (DocPath != null)
                    {
                        destPatientDocPath = DocPath + @"\Docs\Saved\";
                    }
                    if (!string.IsNullOrEmpty(destPatientDocPath))
                    {
                        Utility.EHRDocPath = destPatientDocPath;
                        SystemBAL.UpdateEHRDocPath_Installation(Utility.EHRDocPath, Service_Install_Id);
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("EHR Doc Path : Patient Document Path Not Found. DocPath : '" + DocPath + "', destPatientDocPath : '" + destPatientDocPath + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("EHR Doc Path : Patient Document Path Not Found." + ex.Message);
            }
        }
        #endregion
        public void GetConnectionString()
        {
            try
            {
                if (Utility.DBConnString == string.Empty || Utility.DBConnString == "")
                {
                    if (Utility.Application_Name.ToLower() == "Eaglesoft".ToLower())
                    {
                        EaglesoftConfiguration();
                    }
                    else if (Utility.Application_Name.ToLower() == "Open Dental".ToLower() || (Utility.Application_Name.ToLower() == "OpenDental Cloud".ToLower()))
                    {
                        OpenDentalConfiguration();
                    }
                    else if (Utility.Application_Name.ToLower() == "Dentrix".ToLower())
                    {
                        DentrixConfiguration();
                    }
                    else if (Utility.Application_Name.ToLower() == "SoftDent".ToLower())
                    {
                        SoftDentConfigurtion();
                    }
                    else if (Utility.Application_Name.ToLower() == "ClearDent".ToLower())
                    {
                        ClearDentConfiguration();
                    }
                    else if (Utility.Application_Name.ToLower() == "Tracker".ToLower())
                    {
                        TrackerConfiguration();
                    }
                    else if (Utility.Application_Name.ToLower() == "PracticeWeb".ToLower())
                    {
                        TrackerConfiguration();
                    }
                }

            }
            catch (Exception)
            {
            }
        }

        #region OpenDentalConfiguration
        private void OpenDentalConfiguration()
        {
            if (Utility.Application_Version.ToLower() == "15.4" || Utility.Application_Version.ToLower() == "17.2+")
            {
                Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";
                bool IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);
                if (IsEHRConnected)
                {
                    SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
                }
            }
            else
            {
                ObjGoalBase.WriteToErrorLogFile("EHR Connection : Connction string Not Found.");
            }
        }
        #endregion
        #region PracticeWebConfiguration
        private void PracticeWebConfiguration()
        {
            Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";
            bool IsEHRConnected = SystemBAL.GetEHRPracticeWebConnection(Utility.DBConnString);
            if (IsEHRConnected)
            {
                SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
            }
        }
        #endregion

        #region EaglesoftConfiguration

        private void EaglesoftConfiguration()
        {
            for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
            {
                try
                {
                    string is_primary = Utility.DtInstallServiceList.Rows[j]["Database"].ToString() == "Primary Database" ? "true" : "false";
                    frmConfiguration_Auto frmconfig = new frmConfiguration_Auto("");
                    frmconfig.selectedEHRVersion = Utility.DtInstallServiceList.Rows[j]["Application_Version"].ToString().ToLower();
                    frmconfig.IsEagleSoftConnected(is_primary: is_primary);

                    if (Utility.DBConnString != Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString())
                    {
                        if (SynchEaglesoftDAL.GetEaglesoftConnection(Utility.DBConnString))
                        {
                            IsAppConnection = true;
                            SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("EHR Connection : Connction string Not Found. " + ex.Message.ToString());
                }
            }


            #region Old Code
            //if (Utility.Application_Version.ToLower() == "20.0".ToLower() || Utility.Application_Version.ToLower() == "19.11".ToLower() || Utility.Application_Version.ToLower() == "18.0".ToLower() || Utility.Application_Version.ToLower() == "17".ToLower())
            //{

            //    #region comment by dipika

            //    //Cursor.Current = Cursors.WaitCursor;

            //    //string EaglesoftConStr = "";
            //    //if (Utility.Application_Version.ToLower() == "21.0".ToLower() || Utility.Application_Version.ToLower() == "19.11".ToLower() || Utility.Application_Version.ToLower() == "18.0".ToLower() || Utility.Application_Version.ToLower() == "17".ToLower())
            //    //{
            //    //    Process myProcess = new Process();
            //    //    myProcess.StartInfo.UseShellExecute = false;
            //    //    string DPath = Application.StartupPath;
            //    //    myProcess.StartInfo.FileName = DPath + "\\PTIAuthenticator.exe";
            //    //    myProcess.StartInfo.CreateNoWindow = true;
            //    //    myProcess.StartInfo.Verb = "runas";
            //    //    myProcess.StartInfo.Arguments = "true S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e";
            //    //    myProcess.Start();
            //    //    myProcess.WaitForExit();
            //    //    string DentrixConFileName = Application.StartupPath + "\\DentrixConnectionString.txt";
            //    //    try
            //    //    {
            //    //        using (StreamReader sr = new StreamReader(DentrixConFileName))
            //    //        {
            //    //            Utility.DBConnString = sr.ReadToEnd().ToString().Trim();
            //    //            EaglesoftConStr = Utility.DBConnString;
            //    //        }
            //    //        //IsEHRConnected = SystemBAL.GetEHREagleSoftConnection();
            //    //    }
            //    //    catch (Exception)
            //    //    {

            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    //if (Utility.Application_Version.ToLower() == "18.0".ToLower())
            //    //    //{
            //    //    //    Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
            //    //    //    dynamic settings = Activator.CreateInstance(esSettingsType);

            //    //    //    bool tokenIsValid = settings.SetToken("Adit", "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4");
            //    //    //    //  bool tokenIsValid = settings.SetToken("EagleSoft", "37444a4f485652495700b506f62098957dec9a1a8e5401f4141b8b8870552340");
            //    //    //    EaglesoftConStr = settings.GetLegacyConnectionString(true);

            //    //    //}
            //    //    //else 
            //    //    //if (Utility.Application_Version.ToLower() == "20.0".ToLower() || Utility.Application_Version.ToLower() == "19.11".ToLower() || Utility.Application_Version.ToLower() == "17".ToLower())
            //    //    //{
            //    //    Assembly.LoadFile(Application.StartupPath + "\\Patterson.PTCBaseObjects.SharedObjects.dll");
            //    //    var DLL = Assembly.LoadFile(Application.StartupPath + "\\EaglesoftSettings.dll");

            //    //    foreach (Type type in DLL.GetExportedTypes())
            //    //    {
            //    //        if (type.Name == "EaglesoftSettings")
            //    //        {
            //    //            dynamic settings = Activator.CreateInstance(type);
            //    //            EaglesoftConStr = settings.GetLegacyConnectionString(true);
            //    //            //type.InvokeMember("GetConnection", BindingFlags.InvokeMethod, null,c, new object[] { @"Hello" });
            //    //            //var cs = c.GetConnection();

            //    //        }
            //    //    }
            //    //}
            //    ////}

            //    //#region Create DSN For EagleSoft
            //    //if (CreateDSN())
            //    //{
            //    //    Utility.DBConnString = EaglesoftConStr;
            //    //    if (SynchEaglesoftDAL.GetEaglesoftConnection(Utility.DBConnString))
            //    //    {
            //    //        IsAppConnection = true;
            //    //        SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
            //    //    }
            //    //    else
            //    //    {
            //    //        IsAppConnection = false;
            //    //    }

            //    //}

            //    //#endregion
            //    #endregion
            //    string EaglesoftConStr = "";
            //    for (int j = 0; j < Utility.DtInstallServiceList.Rows.Count; j++)
            //    {
            //        try
            //        {
            //            Cursor.Current = Cursors.WaitCursor;
            //            EaglesoftConStr = "";
            //            Process myProcess = new Process();
            //            myProcess.StartInfo.UseShellExecute = false;
            //            string DPath = Application.StartupPath;
            //            myProcess.StartInfo.FileName = DPath + "\\PTIAuthenticator.exe";
            //            myProcess.StartInfo.CreateNoWindow = true;
            //            myProcess.StartInfo.Verb = "runas";
            //            string is_primary = Utility.DtInstallServiceList.Rows[j]["Database"].ToString() == "Primary Database" ? "true" : "false";
            //            if (Utility.DtInstallServiceList.Rows[j]["Application_Version"].ToString().ToLower() == "21.20".ToLower())
            //            {
            //                myProcess.StartInfo.Arguments = is_primary + "false S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e true " + Utility.DtInstallServiceList.Rows[j]["Application_Version"].ToString().ToLower();
            //            }
            //            else
            //            {
            //                myProcess.StartInfo.Arguments = is_primary + "true S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e false " + Utility.DtInstallServiceList.Rows[j]["Application_Version"].ToString().ToLower();
            //            }
            //            myProcess.Start();
            //            myProcess.WaitForExit();
            //            string DentrixConFileName = Application.StartupPath + "\\ConnectionString.txt";
            //            try
            //            {
            //                using (StreamReader sr = new StreamReader(DentrixConFileName))
            //                {
            //                    Utility.DBConnString = sr.ReadToEnd().ToString().Trim();
            //                    EaglesoftConStr = Utility.DBConnString;
            //                    if (Utility.DtInstallServiceList.Rows[j]["Application_Version"].ToString().ToLower().ToLower() != "21.20".ToLower())
            //                    {
            //                        EaglesoftConStr = Utility.DecryptString(Utility.DBConnString);
            //                    }
            //                }
            //                if (EaglesoftConStr != Utility.DtInstallServiceList.Rows[j]["DBConnString"].ToString())
            //                {
            //                    if (SynchEaglesoftDAL.GetEaglesoftConnection(EaglesoftConStr))
            //                    {
            //                        IsAppConnection = true;
            //                        SystemBAL.UpdateEHRConnectionString_Installation(EaglesoftConStr, Utility.DtInstallServiceList.Rows[j]["Installation_ID"].ToString());
            //                    }
            //                }
            //                //IsEHRConnected = SystemBAL.GetEHREagleSoftConnection();
            //            }
            //            catch (Exception)
            //            {

            //            }
            //        }
            //        catch (Exception ex)
            //        { ObjGoalBase.WriteToErrorLogFile("EHR Connection : Connction string Not Found. " + ex.Message.ToString()); }
            //    }
            //}
            //else
            //{
            //    ObjGoalBase.WriteToErrorLogFile("EHR Connection : Connction string Not Found.");

            //}
            #endregion

        }

        private bool CreateDSN(string serverNameFromUser = "")
        {
            try
            {
                bool dsnCreated = false;
                ODBCMngr.ODBCDSN[] odbcDSNs = null;
                odbcDSNs = ODBCMngr.ODBCManager.GetSystemDSNList();
                string driverName = "";
                string serverName = "";
                string driverPath = "";
                foreach (ODBCMngr.ODBCDSN item in odbcDSNs)
                {
                    if (item.ToString().ToUpper() == "DENTAL")
                    {
                        //FindSqlAnywhereName(item.GetDSNDriverName());
                        driverName = item.GetDSNDriverName().Replace("Adaptive Server", "SQL");
                        try
                        {
                            driverName = driverName.Replace(".0", "");
                        }
                        catch (Exception)
                        {
                            driverName = "SQL Anywhere 16";
                        }
                        serverName = item.GetDSNServerName();
                        driverPath = item.GetDSNDriverPath();
                    }
                }

                if (!DSNExists("PozativeDSN"))
                {
                    if (serverNameFromUser == "")
                    {
                        if (serverName == null || serverName == "")
                        {
                            try
                            {
                                using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\EagleSoft\\Server"))
                                {
                                    if (key != null)
                                    {
                                        Object o = key.GetValue("ServerName");
                                        if (o != null)
                                        {
                                            serverName = o.ToString();
                                        }
                                    }
                                }
                            }
                            catch (Exception)  //just for demonstration...it's always best to handle specific exceptions
                            {
                                //serverName = "EAGLESOFT";
                                if (serverNameFromUser != "")
                                {
                                    serverName = serverNameFromUser;
                                }
                            }
                        }
                    }
                    CreateDSNEagleSoft("PozativeDSN", "Pozative DSN", serverName, driverName, true, "", "");
                    dsnCreated = true;
                }
                else
                {
                    dsnCreated = true;
                }
                return dsnCreated;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
        }

        const string ODBCINST_INI_REG_PATH = "SOFTWARE\\WOW6432Node\\ODBC\\ODBC.INI\\";

        public static bool DSNExists(string dsnName)
        {
            try
            {
                const string ODBC_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";

                var sourcesKey = Registry.LocalMachine.OpenSubKey(ODBCINST_INI_REG_PATH + "ODBC Data Sources");

                if (sourcesKey == null)
                {
                    throw new Exception("ODBC Registry key for sources does not exist");
                }

                string[] blah = sourcesKey.GetValueNames();

                Console.WriteLine("length: " + blah.Length); //prints: 0

                if (sourcesKey.GetValue(dsnName) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //var driversKey = Registry.LocalMachine.CreateSubKey(ODBCINST_INI_REG_PATH + "ODBC Drivers");
            //if (driversKey == null)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}            
        }

        public void CreateDSNEagleSoft(string dsnName, string description, string server, string driverName, bool trustedConnection, string database, string driverPath)
        {
            try
            {
                string driverLocation = string.Empty;
                string appKey = @"SOFTWARE\ODBC\ODBC.INI\Dental\";
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                {
                    if (registryKey != null)
                    {
                        driverLocation = registryKey.GetValue("Driver").ToString();
                    }
                }

                string ODBC_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
                //driverPath = "C:\\WINDOWS\\System32\\sqlsrv32.dll";
                driverPath = !string.IsNullOrEmpty(driverLocation) ? driverLocation : "C:\\WINDOWS\\System32\\sqlsrv32.dll";

                var dsnKey = Registry.LocalMachine.CreateSubKey(ODBCINST_INI_REG_PATH + dsnName);
                if (dsnKey == null)
                {
                    ObjGoalBase.WriteToErrorLogFile("ODBC Registry key for DSN was not created");
                    throw new Exception("ODBC Registry key for DSN was not created");
                }

                dsnKey.SetValue("AutoStop", "YES");
                dsnKey.SetValue("CommLinks", "SHMEM,TCPIP{}");
                dsnKey.SetValue("DescribeCursor", "Always");
                dsnKey.SetValue("Description", description);
                dsnKey.SetValue("Driver", driverPath);
                dsnKey.SetValue("EngineName", server);
                //dsnKey.SetValue("PWD", "dpF?3nHJYejwWUmzaXCM0%m#HUjCO5EE");
                dsnKey.SetValue("ServerName", server);
                dsnKey.SetValue("Start", "");
                //dsnKey.SetValue("UID", "PDBA");
                dsnKey.SetValue("Trusted_Connection", true);

                //dsnKey.SetValue("Database", database);
                //dsnKey.SetValue("Action", "Connect to a running database on another computer");
                //dsnKey.SetValue("Host", "");
                //dsnKey.SetValue("Port", "");

                var datasourcesKey = Registry.LocalMachine.CreateSubKey(ODBCINST_INI_REG_PATH + "ODBC Data Sources");
                if (datasourcesKey == null)
                {
                    throw new Exception("ODBC Registry key does not exist");
                }
                //datasourcesKey.SetValue("Action", "Connect to a running database on another computer");
                datasourcesKey.SetValue(dsnName, driverName);
                //datasourcesKey.

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region DentrixConfiguration

        private void DentrixConfiguration()
        {
            try
            {
                if (
                       (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                    || (Utility.Application_Version.ToLower() == "DTX G5.1".ToLower())
                    || (Utility.Application_Version.ToLower() == "DTX G5.2".ToLower())
                    || (Utility.Application_Version.ToLower() == "DTX G6".ToLower())
                    || (Utility.Application_Version.ToLower() == "DTX G6.1".ToLower())
                    )
                {
                    if (Utility.Application_Version.ToLower() == "DTX G5".ToLower())
                    {
                        Utility.EHRHostname = "localhost";
                        Utility.EHRUserId = "eRxUser";
                        Utility.EHRPassword = "G87_iwx0Y";
                    }
                    else if (Utility.Application_Version.ToLower() == "DTX G6".ToLower())
                    {
                        Utility.EHRHostname = "localhost";
                        Utility.EHRUserId = "ewwb6ycp";
                        Utility.EHRPassword = "a6Vys6MRt";
                    }
                    else
                    {
                        Utility.EHRUserId = "None";
                        Utility.EHRPassword = "None";
                    }
                    Utility.EHRPort = "6604";
                    Utility.DBConnString = "host=" + Utility.EHRHostname + ";UID=" + Utility.EHRUserId + ";PWD=" + Utility.EHRPassword + ";Database=DentrixSQL;DSN=c-treeACE ODBC Driver;port=" + Utility.EHRPort + string.Empty;
                    bool IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                    if (IsEHRConnected)
                    {
                        IsAppConnection = true;
                        SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
                    }
                    else
                    {
                        IsAppConnection = false;
                        ObjGoalBase.WriteToErrorLogFile("EHR Connection : Connction string Not Found.");
                    }
                }
                else
                {
                    try
                    {
                        using (Stream cs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Pozative.Resources.Adit.pfx"))
                        {
                            Byte[] raw = new Byte[cs.Length];
                            for (Int32 i = 0; i < cs.Length; ++i)
                                raw[i] = (Byte)cs.ReadByte();
                            System.Security.Cryptography.X509Certificates.X509Store store = new System.Security.Cryptography.X509Certificates.X509Store(System.Security.Cryptography.X509Certificates.StoreName.TrustedPeople, System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine);
                            store.Open(System.Security.Cryptography.X509Certificates.OpenFlags.ReadWrite);
                            System.Security.Cryptography.X509Certificates.X509Certificate2 cert = new System.Security.Cryptography.X509Certificates.X509Certificate2();
                            cert.Import(raw);
                            store.Add(cert);
                            store.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("EHR User ddxdesktopspc.pfx Not Intalled. Please Aplication start with run as administrator. " + ex.Message);
                        //return;
                    }
                    Utility.EHRUserId = "None";
                    Utility.EHRPassword = "None";
                    Utility.EHRPort = "6604";
                    string exePath = string.Empty;
                    StringBuilder path = new StringBuilder(512);
                    string connectionString = string.Empty;
                    if (CommonFunction.GetDentrixG62ExePath(path) == SUCCESS)
                    {
                        exePath = path.ToString();
                        CommonFunction.GetDentrixG62ConnectionString();
                        bool IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                        if (IsEHRConnected)
                        {
                            SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
                        }
                        else
                        {
                            frmConfiguration.GetDentrixConnectionString(exePath);
                            IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                            if (IsEHRConnected)
                            {
                                IsAppConnection = true;
                                SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
                            }
                            else
                            {
                                IsAppConnection = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Authentication : " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region ClearDentConfiguration
        private void ClearDentConfiguration()
        {

            if (Utility.EHRPassword.Trim() == "")
            {
                Utility.DBConnString = "Initial Catalog=ClearDent;Data Source=" + Utility.EHRHostname + ";User ID=" + Utility.EHRUserId + ";";
            }
            else
            {
                Utility.DBConnString = "Initial Catalog=ClearDent;Data Source=" + Utility.EHRHostname + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
            }
            bool IsEHRConnected = SystemBAL.GetEHRClearDentConnection();

            if (IsEHRConnected)
            {
                SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
            }
            else
            {
                ObjGoalBase.WriteToErrorLogFile("EHR Connection : Connction string Not Found.");
            }
        }
        #endregion

        #region TrackerConfiguration
        private void TrackerConfiguration()
        {

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (Utility.Application_Version.ToLower() == "11.29")
                {
                    bool IsEHRConnected = false;
                    Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                    IsEHRConnected = SystemBAL.GetEHRTrackerConnection();
                    if (IsEHRConnected)
                    {
                        SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("EHR Connection : Connction string Not Found.");
                    }
                }
                else
                {
                    ObjGoalBase.WriteToErrorLogFile("EHR Version : Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Authentication" + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region SoftDentConfigurtion
        private void SoftDentConfigurtion()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (Utility.Application_Version == "17.0.0+")
                {
                    bool IsEHRConnected = OpenConnection();

                    if (IsEHRConnected)
                    {
                        SystemBAL.UpdateEHRConnectionString_Installation(Utility.DBConnString, "1");
                    }
                    else
                    {
                        ObjGoalBase.WriteToErrorLogFile("EHR Connection : Connction string Not Found.");
                    }
                }
                else
                {
                    ObjGoalBase.WriteToErrorLogFile("EHR Version : Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Authentication" + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public bool OpenConnection()
        {
            try
            {
                string serverExeDir = SoftDentRegistryKey.ServerExeDir;
                string faircomServerName = "";//SoftDentRegistryKey.FaircomServerName;
                ErrorCode errorCode = ErrorCode.Failure;
                this.IsOpen = InteropFactory.SoftDent.Open(serverExeDir, faircomServerName, out errorCode);
            }
            catch (Exception)
            {
                this.IsOpen = false;
                throw;
            }
            return this.IsOpen;
        }

        #endregion

        #endregion

        public void tmrSyncPartial_Elapsed(object sender, ElapsedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
            bool IsSyncing = false;
            if (key != null)
            {
                IsSyncing = bool.Parse(key.GetValue("SyncPartialSyncing").ToString());
            }
            else
            {
                key.SetValue("SyncPartialSyncing", false);
            }
            if (!IsSyncing)
            {
                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                {
                    key1.SetValue("SyncPartialSyncing", true);
                }

                tmrSyncPartial.Enabled = false;
                try
                {
                    DataTable dtAditModuleSyncTime = SystemBAL.GetAditModuleSyncTime();
                    foreach (DataRow dr in dtAditModuleSyncTime.Rows)
                    {
                        if (dr["SyncDateTime"] != null && dr["SyncModule_Name"].ToString() != "")
                        {
                            if (dr["SyncDateTime"].Equals(System.DBNull.Value) || DateTime.Now > Convert.ToDateTime(dr["SyncDateTime"]))
                            {
                                switch (dr["SyncModule_Name"].ToString().ToLower())
                                {
                                    case "clear local records":
                                        ClearLocalRecordsSync(dr);
                                        break;
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("SyncPartialSyncing", false);
                        ObjGoalBase.WriteToErrorLogFile("tmrSyncPartial_Elapsed_Elapsed_Err " + ex.Message);
                    }
                }
                finally
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("SyncPartialSyncing", false);
                    }
                    tmrSyncPartial.Enabled = true;
                }
            }
        }

        public bool ClearLocalRecordsSync(DataRow dr)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
                bool IsSyncing = false;
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("ClearLocalRecordSyncing").ToString());
                }

                if (!IsClearLocalRecordsSyncing && !IsSyncing)
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("SyncPartialSyncing", true);
                    }
                    IsClearLocalRecordsSyncing = true;

                    string ClearDBFilePath = Application.StartupPath + "\\ClearDB.txt";
                    if (File.Exists(ClearDBFilePath))
                    {
                        List<String> ClearDBquery = new List<String>();
                        using (StreamReader sr = new StreamReader(ClearDBFilePath))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                line = line.ToString().Replace("CurrentDateTime", DateTime.Now.AddDays(-7).ToString());
                                ClearDBquery.Add(line);
                            }
                        }
                        string str = "Update SyncModule Set SyncDateTime = '" + DateTime.Now.AddMinutes(Convert.ToInt16(dr["SyncModule_EHR"].ToString())) + "' Where SyncModule_Name = '" + dr["SyncModule_Name"].ToString() + "'";
                        ClearDBquery.Add(str);

                        bool staClearDBquery = SystemBAL.LocalDatabaseUpdateQuery(ClearDBquery);

                    }
                    ObjGoalBase.WriteToSyncLogFile("Clear Local Records Synched Successfully..");
                    IsClearLocalRecordsSyncing = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToSyncLogFile("ClearLocalRecordsSync Execution Err " + ex.Message.ToString());
                throw;
            }
            finally
            {
                using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                {
                    key1.SetValue("ClearLocalRecordSyncing", false);
                }
                IsClearLocalRecordsSyncing = false;
            }
        }


        public void AppointmentSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool IsPatientSyncing = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
            {
                if (key != null)
                {
                    IsPatientSyncing = bool.Parse(key.GetValue("IsPatinetSyncing").ToString());
                }
            }
            if (!IsPatientSyncing)
            {
                bool IsSyncing = false;
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
                {
                    if (key != null)
                    {
                        IsSyncing = bool.Parse(key.GetValue("IsAppointmentSyncSyncing").ToString());
                    }
                }
                if (!IsEasyDentalSyncing && !IsSyncing)
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsAppointmentSyncSyncing", true);
                    }

                    EasyDentalSync.Enabled = false;
                    IsEasyDentalSyncing = true;
                    try
                    {
                        using (Process myProcess = new Process())
                        {
                            try
                            {


                                IsProviderSyncedFirstTime = true;
                                IsOpenDentalProviderSync = true;
                                IsOpenDentalOperatorySync = true;
                                IsOpenDentalApptTypeSync = true;
                                Is_synched_Appointment = true;
                                //CallSynchEasyDentalToLocal();
                                myProcess.StartInfo.UseShellExecute = true;
                                // You can start any process, HelloWorld is a do-nothing example.
                                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\AppointmentSync.exe";
                                myProcess.StartInfo.Arguments = "APPOITMENT";
                                myProcess.StartInfo.CreateNoWindow = true;
                                myProcess.Start();
                                // This code assumes the process you are starting will terminate itself.
                                // Given that is is started without a window so you cannot terminate it
                                // on the desktop, it must terminate itself or you can do it programmatically
                                // from this application using the Kill method.
                                IsEasyDentalSyncing = false;
                            }
                            catch (Exception e1)
                            {
                                ObjGoalBase.WriteToErrorLogFile("Appointment_Elapsed_Err " + e1.Message);
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
                        IsEasyDentalSyncing = false;
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsAppointmentSyncSyncing", false);
                        }
                        ObjGoalBase.WriteToErrorLogFile("Appointment_Elapsed_Err " + ex.Message);
                    }
                    finally
                    {
                        EasyDentalSync.Enabled = true;
                    }
                    if (Is_PatientSyncPending)
                    {
                        SyncPatientRecordsInitialy();
                    }
                }
            }
        }

        public void PatientSync_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool IsAppointmentSyncing = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
            {
                if (key != null)
                {
                    IsAppointmentSyncing = bool.Parse(key.GetValue("IsAppointmentSyncSyncing").ToString());
                }
            }
            if (!IsAppointmentSyncing)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync");
                bool IsSyncing = false;
                if (key != null)
                {
                    IsSyncing = bool.Parse(key.GetValue("IsPatinetSyncing").ToString());
                }

                if (!IsEasyDentalPatientSyncing && !IsSyncing)
                {
                    using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                    {
                        key1.SetValue("IsPatinetSyncing", true);
                    }

                    EasyDentalPatientSync.Enabled = false;
                    IsEasyDentalPatientSyncing = true;
                    try
                    {
                        using (Process myProcess = new Process())
                        {

                            try
                            {
                                Is_synched_Patient = false;
                                Is_synched_AppointmentsPatient = false;
                                ObjGoalBase.WriteToSyncLogFile("Patient Syncing Start ");
                                // SynchDataEasyDental_Patient();
                                myProcess.StartInfo.UseShellExecute = true;
                                // You can start any process, HelloWorld is a do-nothing example.
                                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\AppointmentSync.exe";
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
                                ObjGoalBase.WriteToErrorLogFile("Patient_Elapsed_Err " + e1.Message);
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
                        Is_PatientSyncPending = false;
                        using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key1.SetValue("IsPatinetSyncing", false);
                            ObjGoalBase.WriteToErrorLogFile("Patient_Elapsed_Err " + ex.Message);
                        }
                    }
                    finally
                    {
                        EasyDentalPatientSync.Enabled = true;
                    }
                    Is_PatientSyncPending = false;
                }
            }
            else
            {
                Is_PatientSyncPending = true;
            }
        }

        bool Is_PatientSyncPending = false;

        public void SyncPatientRecordsInitialy()
        {
            bool IsAppointmentSyncing = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
            {
                if (key != null)
                {
                    IsAppointmentSyncing = bool.Parse(key.GetValue("IsAppointmentSyncSyncing").ToString());
                }
            }
            if (!IsAppointmentSyncing)
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
                                IsSyncing = bool.Parse(key.GetValue("IsPatinetSyncing").ToString());
                            }
                        }
                        if (!IsSyncing)
                        {
                            using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                            {
                                key1.SetValue("IsPatinetSyncing", true);
                            }
                            IsEasyDentalPatientSyncing = true;
                            //   SynchDataEasyDental_Patient();
                            using (Process myProcess = new Process())
                            {

                                try
                                {
                                    Is_synched_Patient = false;
                                    Is_synched_AppointmentsPatient = false;
                                    //SynchDataEasyDental_Patient();
                                    myProcess.StartInfo.UseShellExecute = true;
                                    // You can start any process, HelloWorld is a do-nothing example.
                                    myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\AppointmentSync.exe";
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
                                        key2.SetValue("IsPatinetSyncing", false);
                                    }
                                    ObjGoalBase.WriteToErrorLogFile("Patient_Elapsed_Err " + e1.Message);
                                    throw;
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Is_PatientSyncPending = false;
                        IsEasyDentalPatientSyncing = false;
                        using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key2.SetValue("IsPatinetSyncing", false);
                        }
                        ObjGoalBase.WriteToErrorLogFile("Patient_Elapsed_Err " + ex.Message);
                        throw;
                    }
                    Is_PatientSyncPending = false;
                }
            }
            else
            {
                Is_PatientSyncPending = true;
            }
        }

        public void SyncAppointmentRecordsInitialy()
        {
            bool IsPatientSyncing = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\PozativeSync"))
            {
                if (key != null)
                {
                    IsPatientSyncing = bool.Parse(key.GetValue("IsPatinetSyncing").ToString());
                }
            }
            if (!IsPatientSyncing)
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
                                IsSyncing = bool.Parse(key.GetValue("IsAppointmentSyncSyncing").ToString());
                            }
                        }
                        if (!IsSyncing)
                        {
                            using (RegistryKey key1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                            {
                                key1.SetValue("IsAppointmentSyncSyncing", true);
                            }
                            IsEasyDentalSyncing = true;
                            using (Process myProcess = new Process())
                            {

                                try
                                {
                                    //SynchDataLiveDB_Pull_PatientForm();
                                    // CallSynchEasyDentalToLocal();
                                    IsProviderSyncedFirstTime = true;
                                    IsOpenDentalProviderSync = true;
                                    IsOpenDentalOperatorySync = true;
                                    IsOpenDentalApptTypeSync = true;
                                    Is_synched_Appointment = true;
                                    Is_synched_Patient = false;
                                    Is_synched_AppointmentsPatient = false;
                                    // CallSynchEasyDentalToLocal();
                                    //SynchDataLiveDB_Push_Appointment();
                                    myProcess.StartInfo.UseShellExecute = true;
                                    // You can start any process, HelloWorld is a do-nothing example.
                                    myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\AppointmentSync.exe";
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


                                }
                                catch (Exception e1)
                                {
                                    IsEasyDentalSyncing = false;
                                    using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                                    {
                                        key2.SetValue("IsAppointmentSyncSyncing", false);
                                    }
                                    ObjGoalBase.WriteToErrorLogFile("APPOITMENT_Elapsed_Err " + e1.Message);
                                    throw;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        IsEasyDentalSyncing = false;
                        using (RegistryKey key2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync"))
                        {
                            key2.SetValue("IsAppointmentSyncSyncing", false);
                        }
                        ObjGoalBase.WriteToErrorLogFile("APPOITMENT_Elapsed_Err " + ex.Message);
                        throw;
                    }
                    if (Is_PatientSyncPending)
                    {
                        SyncPatientRecordsInitialy();
                    }
                }
            }
        }

        private void SetBtnDesignForConnectionStatus(cButton btn, string statusText, bool isConnected = true, bool isIdle = false)
        {
            btn.Text = statusText;
            if (isIdle)
            {
                btn.BackColor = IdleColor;
            }
            else
            {
                if (isConnected)
                {
                    btn.BackColor = Color.White;
                    btn.ForeColor = ColorTranslator.FromHtml("#F4891F");
                    btn.TextColor = ColorTranslator.FromHtml("#F4891F");
                    btn.BorderColor = ColorTranslator.FromHtml("#F4891F");
                }
                else
                {
                    btn.BackColor = DisConnectedColor;
                    btn.ForeColor = ColorTranslator.FromHtml("#58595B");
                    btn.TextColor = ColorTranslator.FromHtml("#58595B");
                    btn.BorderColor = ColorTranslator.FromHtml("#E5E5E5");
                }
            }
        }

        private void frmPozative_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
