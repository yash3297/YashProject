using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Runtime.InteropServices;
using RestSharp;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using Microsoft.Win32;
using System.Net.Security;
using Pozative.Properties;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Data.Sql;
using System.Data.SqlClient;
using Pozative.UTL;
using Pozative.BAL;
using Pozative.BO;
using System.Threading;

namespace Pozative
{
    public partial class frmConfiguration_Auto_OLD : Form
    {

        #region Variable
        private const string ODBC_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
        private const string ODBCINST_INI_REG_PATH = "SOFTWARE\\WOW6432Node\\ODBC\\ODBC.INI\\";
        GoalBase ObjGoalBase = new GoalBase();
        string IsAction = "Update";
        string strEaglesoftIntegrationKey = "32313032-4553-EFFD-5083-506F7A617469";
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        string UserAditLocationLinkList = string.Empty;
        string UserPozativeLocationLinkList = string.Empty;
        string System_Name = string.Empty;
        string processorID = string.Empty;

        DataTable dtTempOgrTable = new DataTable();
        DataTable dtTempLocationTable = new DataTable();
        DataTable dtTempApptLocationTable = new DataTable();
        DataTable dtTempPozativeLocationTable = new DataTable();
        DataTable dtSqlServerName = new DataTable();
        DataTable dtTempOpenDentalClinicTable = new DataTable();

        string darkGrayColor = "#58595B";
        string lightGrayColor = "#E5E5E5";
        string orangeColor = "#F4891F";
        bool isProcessComplete = false;
        List<string> installedEHR = null;
        string selectedEHR = "";
        string configErrors = "";
        #endregion

        #region Form Load

        public frmConfiguration_Auto_OLD(string tmpIsAction)
        {
            InitializeComponent();

            IsAction = tmpIsAction;

            this.Size = new Size(900, 650);

            //cpnlMain.Size = new Size(550, 500);
            //cpnlMain.MaximumSize = new Size(550, 500);
            cpnlMain.Size = new Size(437, 422);
            cpnlMain.MaximumSize = new Size(437, 422);

            this.CenterToScreen();
        }

        private void frmConfiguration_Load(object sender, EventArgs e)
        {
            try
            {
                Utility.DBConnString = "";
                Cursor.Current = Cursors.WaitCursor;
                GetAutoPlayAudioText();

                bindFormControls();

                DataTable dtEHRList = GetEHRList();
                if (dtEHRList.Rows.Count == 1)
                {
                    //ObjGoalBase.ErrorMsgBox("Pozative Service", "We're sorry. But it seems there isn't an EHR installed on this computer. If you find this in error please give us a call at (832) 225-8865.", true, CommonFunction.audioTextLst.ehrnotexists);
                    Environment.Exit(0);
                }
                cmbEHRName.DataSource = dtEHRList;
                cmbEHRName.ValueMember = "EHR_ID";
                cmbEHRName.DisplayMember = "EHR_Name";
                cmbEHRName.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbEHRVersion.DropDownStyle = ComboBoxStyle.DropDownList;

                dtTempOgrTable = CreateTempOrgTableData();
                dtTempLocationTable = CreateTempLocationTableData();
                dtTempApptLocationTable = CreateTempAppointmentLocationTableData();
                dtTempPozativeLocationTable = CreateTempPozativeLocationTableData();
                this.StartPosition = FormStartPosition.CenterScreen;

                System_Name = System.Environment.MachineName;
                processorID = ObjGoalBase.getUniqueID("C");
                if (dtEHRList.Rows.Count == 2)
                {
                    Utility.Application_Name = dtEHRList.Rows[1]["EHR_Name"].ToString();
                    cmbEHRName.SelectedValue = dtEHRList.Rows[1]["EHR_ID"];
                    btn_AutomainSave.PerformClick();
                }
                else
                {
                    cmbEHRName.SelectedIndex = 0;
                }
                cmbEHRName.Focus();
                cmbEHRName.Select();
                //PlayAudio("WelcomeMessage.txt");

               
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "frmConfiguration_Load " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Common Function
        private static string DENTRIX_SUBKEYPATH = "Dentrix Dental Systems, Inc.\\Dentrix\\General"; //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Dentrix Dental Systems, Inc.\Dentrix\General
        private static string OpenDental_SubKeyPath = "MakeMSI\\KeyPaths\\OpenDental\\";//Computer\HKEY_CURRENT_USER\SOFTWARE\MakeMSI\KeyPaths\OpenDental
        private static string EagleSoft_SubKeyPath = "Eaglesoft\\Server"; //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Eaglesoft\Server
        private static string ClearDent_SubKeyPath = "Prococious Technology Inc.\\ClearDent"; //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Prococious Technology Inc.\ClearDent
        private static string Tracker_SubKeyPath = ""; //HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Installer\Products\\11AB500F5D9B4934EA50E9D9B10CE878\\ProductName == Tracker Server
        private static string PracticeWork_SubKeyPath = "SOFTWARE\\PWInc\\"; //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\PWInc\PWSvr
        private static string EasyDental_SubKeyPath = "SOFTWARE\\Easy Dental Systems, Inc.\\Easy Dental\\General"; //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Easy Dental Systems, Inc.\Easy Dental\General
        private static string AbelDent_SubKeyPath = "Eaglesoft\\Server"; //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Eaglesoft\Server

        static bool IsAppInstalled(string appName)
        {
            try
            {
                string appKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                {
                    if (registryKey != null)
                    {
                        foreach (string subKeyName in registryKey.GetSubKeyNames())
                        {
                            using (RegistryKey subKey = registryKey.OpenSubKey(subKeyName))
                            {
                                string displayName = subKey.GetValue("DisplayName") as string;
                                if (displayName != null && displayName.StartsWith(appName))
                                {
                                    EHRRegInstallationPath = subKey.GetValue("InstallLocation") as string;
                                    EHRRegVersion = subKey.GetValue("DisplayVersion") as string;
                                    if (EHRRegInstallationPath != null && EHRRegInstallationPath != "")
                                    {
                                        //EHRInstallationPath = subKey.GetValue("InstallLocation") as string;
                                        //MessageBox.Show(EHRInstallationPath);
                                        return true;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        static bool IsAppTrackerInstalled(string appName)
        {
            try
            {
                string appKey = @"SOFTWARE\Classes\Installer\Products";
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                {
                    if (registryKey != null)
                    {
                        foreach (string subKeyName in registryKey.GetSubKeyNames())
                        {
                            using (RegistryKey subKey = registryKey.OpenSubKey(subKeyName))
                            {
                                string displayName = subKey.GetValue("ProductName") as string;
                                if (displayName != null && displayName.Contains(appName))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        private DataTable GetDatabaseList()
        {
            DataTable dtDatabaseList = new DataTable();
            dtDatabaseList.Clear();
            dtDatabaseList.Columns.Add("DatabaseId", typeof(Int32));
            dtDatabaseList.Columns.Add("DatabaseName", typeof(string));
            dtDatabaseList.Rows.Add(0, "Sql CE");
            dtDatabaseList.Rows.Add(1, "SqlExpress");
            dtDatabaseList.AcceptChanges();
            return dtDatabaseList;
        }

        private DataTable GetEHRVersionList(string EHRname)
        {
            DataTable dtVersionList = new DataTable();
            try
            {
                List<string> versions = new List<string>();

                switch (EHRname)
                {
                    case "OpenDental":
                        versions.AddRange(new List<string>() { "15.4", "17.2+" });
                        break;

                    case "Dentrix":
                        versions.AddRange(new List<string>() { "DTX G5", "DTX G6", "DTX G6.2+" });
                        break;

                    case "PracticeWeb":
                        versions.AddRange(new List<string>() { "21.1" });
                        break;

                    case "ClearDent":
                        versions.AddRange(new List<string>() { "9.8+", "9.10+" });
                        break;

                    case "Tracker":
                        versions.AddRange(new List<string>() { "11.29" });
                        break;

                    case "AbelDent":
                        versions.AddRange(new List<string>() { "14.8.2" });
                        break;

                    case "PracticeWork":
                        versions.AddRange(new List<string>() { "7.9+" });
                        break;

                    case "Eaglesoft":
                        versions.AddRange(new List<string>() { "17", "18.0", "19.11", "20.0", "21.20", "22.00" });
                        break;

                    case "EasyDental":
                        versions.AddRange(new List<string>() { "11.1" });
                        break;

                    case "SoftDent":
                        versions.AddRange(new List<string>() { "17.0.0+" });
                        break;
                    default:
                        break;
                }

                dtVersionList.Clear();
                dtVersionList.Columns.Add("Version_ID", typeof(Int32));
                dtVersionList.Columns.Add("Version_Name", typeof(string));
                dtVersionList.Rows.Add(0, "<Select>");
                for (int i = 0; i < versions.Count; i++)
                {
                    dtVersionList.Rows.Add(i + 1, versions[i]);
                }
                dtVersionList.AcceptChanges();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "GetEHRVersionList " + ex.Message);
            }
            return dtVersionList;
        }
        private void bindFormControls()
        {
            try
            {
                SetPanelVisiblityEHRwise("AdminUser", false);

                btn_EG.Visible = false;
                btn_OD.Visible = false;
                btn_DTX.Visible = false;
                btn_EZ.Visible = false;
                btn_Pwork.Visible = false;
                btn_PWeb.Visible = false;
                btn_CD.Visible = false;
                btn_SoftDent.Visible = false;
                btn_Tracker.Visible = false;
                btn_abeldent.Visible = false;

                DataTable dtEHRList = GetEHRList();
                cmbEHRName.DataSource = dtEHRList;
                cmbEHRName.ValueMember = "EHR_ID";
                cmbEHRName.DisplayMember = "EHR_Name";
                //cmbEHRName.SelectedIndex = 0;
                cmbEHRName.DropDownStyle = ComboBoxStyle.DropDownList;
                if (dtEHRList.Rows.Count == 2)
                {
                    //MessageBox.Show(dtEHRList.Rows[1]["EHR_Name"].ToString());
                    Utility.Application_Name = dtEHRList.Rows[1]["EHR_Name"].ToString();
                    cmbEHRName.SelectedValue = dtEHRList.Rows[1]["EHR_ID"];
                    btn_AutomainSave.PerformClick();
                }
                else
                {
                    cmbEHRName.SelectedIndex = 0;
                }
                cmbEHRVersion.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbEHRVersion.Enabled = false;

                #region commented code
                //cboDatabaseList.DataSource = GetDatabaseList();
                //cboDatabaseList.ValueMember = "DatabaseId";
                //cboDatabaseList.DisplayMember = "DatabaseName";
                //cboDatabaseList.SelectedIndex = 0;
                //cboDatabaseList.DropDownStyle = ComboBoxStyle.DropDownList;
                //cboDatabaseList.DropDownStyle = ComboBoxStyle.DropDownList;
                //GetDatabaseList 
                #endregion

                lblFormHead.Text = "Please select your Patient Management System";
                lblFormHead.Anchor = AnchorStyles.Left;

                cmbTrackerHostName.DataSource = null;
                cmbSqlServiceName.DataSource = null;

                SetEHRCredentialValues();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "BindFormControls " + ex.Message);
            }
        }
        private void SetEHRCredentialValues()
        {
            txtEaglesoftHostName.Text = "localhost";
            txtEaglesoftUserId.Text = "GGY";
            txtEaglesoftPassword.Text = string.Empty;

            txtOpenDentalHostName.Text = "localhost";
            txtOpenDentalDatabase.Text = "OpenDental";
            txtOpenDentalUserId.Text = "root";
            txtOpenDentalPassword.Text = string.Empty;

            txtDentrixHostName.Text = "localhost";
            txtDentrixUserId.Text = "h36FadCg";
            txtDentrixPassword.Text = "eSnkTrvap";
            txtDentrixPort.Text = "6604";

            txtAdminUserName.Text = string.Empty;
            txtAdminPassword.Text = string.Empty;
        }
        private DataTable CreateTempOrgTableData()
        {
            DataTable dtOrg = new DataTable();
            try
            {
                dtOrg.Clear();
                dtOrg.Columns.Add("id", typeof(string));
                dtOrg.Columns.Add("Organization_ID", typeof(string));
                dtOrg.Columns.Add("Name", typeof(string));
                dtOrg.Columns.Add("phone", typeof(string));
                dtOrg.Columns.Add("email", typeof(string));
                dtOrg.Columns.Add("address", typeof(string));
                dtOrg.Columns.Add("Adit_User_Email_ID", typeof(string));
                dtOrg.Columns.Add("Adit_User_Email_Password", typeof(string));
                dtOrg.AcceptChanges();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "CreateTempOrgTableData" + ex.Message);
            }
            return dtOrg;
        }

        private DataTable CreateTempLocationTableData()
        {
            DataTable dtLoc = new DataTable();
            try
            {
                dtLoc.Clear();
                dtLoc.Columns.Add("id", typeof(string));
                dtLoc.Columns.Add("name", typeof(string));
                dtLoc.Columns.Add("google_address", typeof(string));
                dtLoc.Columns.Add("phone", typeof(string));
                dtLoc.Columns.Add("email", typeof(string));
                dtLoc.Columns.Add("address", typeof(string));
                dtLoc.Columns.Add("timezone", typeof(string));
                dtLoc.Columns.Add("website_url", typeof(string));
                dtLoc.Columns.Add("User_ID", typeof(string));
                dtLoc.Columns.Add("Loc_ID", typeof(string));
                dtLoc.Columns.Add("Clinic_Number", typeof(string));
                dtLoc.Columns.Add("Service_Install_Id", typeof(string));
                dtLoc.AcceptChanges();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "CreateTempLocationTableData " + ex.Message);
            }
            return dtLoc;

        }

        private DataTable CreateTempAppointmentLocationTableData()
        {
            DataTable dtApptLoc = new DataTable();
            try
            {
                dtApptLoc.Clear();
                dtApptLoc.Columns.Add("id", typeof(string));
                dtApptLoc.Columns.Add("name", typeof(string));
                dtApptLoc.Columns.Add("google_address", typeof(string));
                dtApptLoc.Columns.Add("phone", typeof(string));
                dtApptLoc.Columns.Add("email", typeof(string));
                dtApptLoc.Columns.Add("address", typeof(string));
                dtApptLoc.Columns.Add("timezone", typeof(string));
                dtApptLoc.Columns.Add("website_url", typeof(string));
                dtApptLoc.Columns.Add("system_mac_address", typeof(string));
                dtApptLoc.AcceptChanges();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "CreateTempAppointmentLocationTableData " + ex.Message);
            }
            return dtApptLoc;
        }

        private DataTable CreateTempPozativeLocationTableData()
        {
            DataTable dtPozativeLoc = new DataTable();
            try
            {
                dtPozativeLoc.Clear();
                dtPozativeLoc.Columns.Add("id", typeof(string));
                dtPozativeLoc.Columns.Add("locationname", typeof(string));
                dtPozativeLoc.Columns.Add("machineid", typeof(string));
                dtPozativeLoc.AcceptChanges();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "CreateTempPozativeLocationTableData " + ex.Message);
            }
            return dtPozativeLoc;

        }

        private void SetPanelVisiblityEHRwise(string EHRname, bool isEHR = true)
        {
            try
            {
                #region Set All pnl visiblity false
                pnlDentrixMain.Visible = false;
                pnlOpenDentalMain.Visible = false;
                pnlpnPracticeWeb.Visible = false;
                pnlClearDentMain.Visible = false;
                pnlpnTracker.Visible = false;
                pnlpnAbelDent.Visible = false;
                pnlPracticeWork.Visible = false;
                pnlEaglesoftMain.Visible = false;
                pnlAdminUserMain.Visible = false;
                tblAdminUserMain.Visible = false;
                pnlLocationMain.Visible = false;
                pnlMain.Visible = false;
                tblEHRMain.Visible = false;
                #endregion

                pnlTitle.Size = new Size(pnlTitle.Width, 100);
                lblMain1.TextAlign = ContentAlignment.MiddleCenter;
                lblMain1.Padding = new Padding(40, 0, 40, 0);

                if (isEHR)
                {
                    if (cpnlMain.Controls.Find("pnlTitle", true).Count() > 0)
                        cpnlMain.Controls.Remove(pnlTitle);
                    selectedEHR = EHRname;
                }
                else
                {
                    if (cpnlMain.Controls.Find("pnlTitle", true).Count() < 1)
                        cpnlMain.Controls.Add(pnlTitle);
                }
                switch (EHRname)
                {
                    case "OpenDental":
                        {
                            pnlOpenDentalMain.Visible = true;
                            pnlOpenDentalMain.Visible = true;
                            if (cpnlMain.Controls.Find("pnlOpenDentalMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlOpenDentalMain);
                            pnlOpenDentalMain.Dock = DockStyle.Fill;
                            pnlOpenDentalMain.BringToFront();

                            txtOpenDentalPassword.Focus();
                            txtOpenDentalPassword.Text = string.Empty;
                            lblFormHead.Text = "OpenDental Configuration";
                        }
                        break;
                    case "Dentrix":
                        {
                            pnlDentrixMain.Visible = true;
                            if (cpnlMain.Controls.Find("pnlDentrixMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlDentrixMain);
                            pnlDentrixMain.Dock = DockStyle.Fill;
                            pnlDentrixMain.BringToFront();

                            txtDentrixPassword.Text = "eSnkTrvap";
                            picNotAllowToChangeSystemDateFormat.Image = Resources.OFF;
                            picNotAllowToChangeSystemDateFormat.Tag = "OFF";

                            if (cmbEHRVersion.Text.ToLower() == "DTX G5".ToLower())
                            {
                                txtDentrixHostName.Text = "localhost";
                                txtDentrixUserId.Text = "eRxUser";
                                txtDentrixPassword.Text = "G87_iwx0Y";
                            }
                            else if (cmbEHRVersion.Text.ToLower() == "DTX G6".ToLower())
                            {
                                txtDentrixHostName.Text = "localhost";
                                txtDentrixUserId.Text = "ewwb6ycp";
                                txtDentrixPassword.Text = "a6Vys6MRt";
                            }
                            else
                            {
                                txtDentrixUserId.Text = "None";
                                txtDentrixPassword.Text = "None";
                            }
                            lblFormHead.Text = "Dentrix Configuration";
                            btnDentrixSave.Focus();
                        }
                        break;
                    case "PracticeWeb":
                        {
                            pnlpnPracticeWeb.Visible = true;

                            if (cpnlMain.Controls.Find("pnlpnPracticeWeb", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlpnPracticeWeb);
                            pnlpnPracticeWeb.Dock = DockStyle.Fill;
                            pnlpnPracticeWeb.BringToFront();
                            PracticeWeb_txthostName.Text = "localhost";
                            txtPracticeWebDatabase.Text = "freedental";
                            txtPracticeWebUseID.Text = "root";
                            txtPracticeWebPassword.Focus();
                            txtPracticeWebPassword.Text = string.Empty;
                            lblFormHead.Text = "PracticeWeb Configuration";
                        }
                        break;
                    case "ClearDent":
                        {
                            pnlClearDentMain.Visible = true;

                            if (cpnlMain.Controls.Find("pnlClearDentMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlClearDentMain);
                            pnlClearDentMain.Dock = DockStyle.Fill;
                            pnlClearDentMain.BringToFront();

                            txtClearDentHostName.Text = "(local)\\VSDOTNET";
                            txtClearDentUserId.Text = "sa";
                            txtClearDentPassowrd.Text = "";
                            btnClearDentSave.Focus();
                            lblFormHead.Text = "ClearDent Configuration";
                        }
                        break;
                    case "Tracker":
                        {
                            try
                            {
                                pnlpnTracker.Visible = true;
                                if (cpnlMain.Controls.Find("pnlpnTracker", true).Count() < 1)
                                    cpnlMain.Controls.Add(pnlpnTracker);
                                pnlpnTracker.Dock = DockStyle.Fill;
                                pnlpnTracker.BringToFront();

                                cmbTrackerHostName.Visible = true;
                                cmbSqlServiceName.Visible = true;
                                label19.Visible = true;
                                label22.Visible = true;
                                //GetSqlServerNameANDServiceName();
                                if (IsAppInstalled("Tracker Server"))
                                {
                                    if (EHRRegInstallationPath != "")
                                    {
                                        string configFilePath = EHRRegInstallationPath + "\\Settings.config"; // replace with the path to your external config file                    
                                        DataSet ds = new DataSet();
                                        ds.ReadXml(configFilePath);
                                        Utility.EHRHostname = ds.Tables["Database"].Rows[0]["ServerName"].ToString();
                                    }
                                }

                                txtTrackerDatabase.Text = "Tracker";
                                txtTrackerCredential.Text = "Adit";
                                txtTrackerUserId.Text = "TBNAdmin";
                                txtTrackerPassword.Text = "{A827F13E-96B8-11DC-B1E3-A86156D89593}";
                                btnTrackerSave.Focus();
                                lblFormHead.Text = "Tracker Configuration";
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        break;
                    case "AbelDent":
                        {
                            pnlpnAbelDent.Visible = true;

                            if (cpnlMain.Controls.Find("pnlpnAbelDent", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlpnAbelDent);
                            pnlpnAbelDent.Dock = DockStyle.Fill;
                            pnlpnAbelDent.BringToFront();

                            cmbAbelDentHostName.DataSource = dtSqlServerName;
                            cmbAbelDentHostName.DisplayMember = "SqlServerName";
                            if (dtSqlServerName == null || (dtSqlServerName != null && dtSqlServerName.Rows.Count == 0))
                            {
                                //cmbAbelDentHostName.Text = ".\\sqlexpress";
                                cmbAbelDentHostName.Text = "192.168.1.197\\aditbeta";
                            }

                            txtAbelDentDatabase.Text = "Abel";
                            txtAbelDentUserId.Text = "sa";
                            txtAbelDentPassword.Text = "Adit@123";
                            btnAbeldentSave.Focus();
                            lblFormHead.Text = "AbelDent Configuration";
                        }
                        break;
                    case "PracticeWork":
                        {
                            pnlPracticeWork.Visible = true;

                            if (cpnlMain.Controls.Find("pnlPracticeWork", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlPracticeWork);
                            pnlPracticeWork.Dock = DockStyle.Fill;
                            pnlPracticeWork.BringToFront();

                            txtAdminPassword.Text = "";
                            if (cmbEHRVersion.Text.ToLower() == "7.9+".ToLower())
                            {
                                txtPracticeworkHost.Text = "localhost";
                                txtPracticeworkDbName.Text = "PW";
                                txtPracticeworkUserId.Text = "";
                                txtPracticeworkPassword.Text = "";
                            }

                            lblFormHead.Text = "PracticeWork Configuration";
                            btnPracticeWorkSave.Focus();
                        }
                        break;
                    case "Eaglesoft":
                        {
                            pnlEaglesoftMain.Visible = true;

                            if (cpnlMain.Controls.Find("pnlEaglesoftMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlEaglesoftMain);
                            pnlEaglesoftMain.Dock = DockStyle.Fill;
                            pnlEaglesoftMain.BringToFront();

                            label6.Visible = true;
                            label7.Visible = true;
                            txtEaglesoftUserId.Visible = true;
                            txtEaglesoftPassword.Visible = true;
                            txtEaglesoftHostName.Focus();
                            txtEaglesoftHostName.Text = string.Empty;
                            lblEaglesoftHostname.Text = "EagleSoft ServerName";
                            lblFormHead.Text = "Eaglesoft Configuration";
                        }
                        break;
                    case "AdminUser":
                        {
                            pnlAdminUserMain.Visible = true;
                            tblAdminUserMain.Visible = true;

                            if (cpnlMain.Controls.Find("pnlAdminUserMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlAdminUserMain);
                            pnlAdminUserMain.Dock = DockStyle.Fill;
                            pnlAdminUserMain.BringToFront();

                            RBtnMultiClinic.Visible = false;
                            RBtnSingleClinic.Visible = false;

                            txtAdminUserName.Focus();

                            SetActivePozativeSync(true, "Adit");
                            SetActivePozativeSync(false, "Pozative");
                            lblFormHead.Text = "User Login Credentials";
                            lblMain1.Text = "Adit App User Login Credentials";

                        }
                        break;
                    case "Location":
                        {
                            pnlLocationMain.Visible = true;

                            if (cpnlMain.Controls.Find("pnlLocationMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlLocationMain);
                            pnlLocationMain.Dock = DockStyle.Fill;
                            pnlLocationMain.BringToFront();

                            lblFormHead.Text = "Please select the Location you would like to Sync";
                            lblMain1.Text = "Please select the Location you would like to Sync";

                            LblAditLocationSingle.Visible = false;
                            cmbAditLocation.Visible = false;
                            DGVMuliClinc.Visible = false;

                            if (RBtnMultiClinic.Checked)
                            {
                                pnlTitle.Size = new Size(pnlTitle.Width, 125);
                                btnLocationBack.Location = new Point(68, 220);
                                btnLocationSave.Location = new Point(227, 220);
                                DGVMuliClinc.Visible = true;

                                DGVMuliClinc.DataSource = dtTempOpenDentalClinicTable;

                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DataSource = dtTempApptLocationTable.Copy();
                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).ValueMember = "id";
                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DisplayMember = "name";

                                for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                                {
                                    DGVMuliClinc.Rows[i].Cells["Location"].Value = "0";
                                }
                                DGVMuliClinc.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold, GraphicsUnit.Pixel);
                                //CommonFunction.TextToSpeech(CommonFunction.audioTextLst.multipleclinicsexists);
                            }
                            else
                            {
                                pnlTitle.Size = new Size(pnlTitle.Width, 150);
                                lblMain1.Padding = new Padding(40, 0, 40, 0);

                                btnLocationBack.Location = new Point(68, 146);
                                btnLocationSave.Location = new Point(227, 146);

                                LblAditLocationSingle.Visible = true;
                                cmbAditLocation.Visible = true;
                            }
                        }
                        break;
                    case "Main":
                        {
                            pnlMain.Visible = true;

                            if (cpnlMain.Controls.Find("pnlMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlMain);
                            pnlMain.Dock = DockStyle.Fill;
                            pnlMain.BringToFront();

                            lblMain1.Text = "Select your Patient Management System";
                            if (cmbEHRName.Items.Count > 1)
                            {
                                int cnt = cmbEHRName.Items.Count;
                                int l = flowLayoutPanel1.Padding.Left;
                                int r = flowLayoutPanel1.Padding.Right;
                                int b = flowLayoutPanel1.Padding.Bottom;
                                if (cnt == 2)
                                    flowLayoutPanel1.Padding = new Padding(l, 115,r,b);
                                else if (cnt == 3)
                                    flowLayoutPanel1.Padding = new Padding(l, 87, r, b);
                                else if (cnt == 4)
                                    flowLayoutPanel1.Padding = new Padding(l, 59, r, b);
                                else if (cnt == 5)
                                    flowLayoutPanel1.Padding = new Padding(l, 31, r, b);
                                else if (cnt == 6)
                                    flowLayoutPanel1.Padding = new Padding(l, 3, r, b);
                                else
                                    flowLayoutPanel1.Padding = new Padding(l, 0, r, b);
                            }
                            //if (cmbEHRName.Items.Count > 2)
                            //    CommonFunction.TextToSpeech(CommonFunction.audioTextLst.multipleehrexists);
                        }
                        break;
                    case "ReportGenerated":
                        {
                            if (cpnlMain.Controls.Find("pnlTitle", true).Count() > 0)
                            {
                                cpnlMain.Controls.Remove(pnlTitle);
                            }
                            pnlReportGenerated.Visible = true;
                            if (cpnlMain.Controls.Find("pnlReportGenerated", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlReportGenerated);
                            pnlReportGenerated.Dock = DockStyle.Fill;
                            pnlReportGenerated.BringToFront();

                            lblMain1.Visible = false;
                            //CommonFunction.TextToSpeech(CommonFunction.audioTextLst.success);
                            if (!string.IsNullOrEmpty(configErrors))
                            {
                                //File.WriteAllText(Application.StartupPath + "\\ErrorLog.txt", configErrors);
                                //CommonFunction.TextToSpeech(CommonFunction.audioTextLst.errormessage);
                            }
                        }
                        break;
                    case "EHRMain":
                        {
                            tblEHRMain.Visible = true;

                            if (pnlTitle.Controls.Find("tblEHRMain", true).Count() < 1)
                                pnlTitle.Controls.Add(tblEHRMain);
                            tblEHRMain.Dock = DockStyle.Fill;
                        }
                        break;
                    default:
                        break;
                }
                if (isEHR && EHRname != "ReportGenerated")
                {
                    //CommonFunction.TextToSpeech(CommonFunction.audioTextLst.ehrdefaultconfigurationmissing);
                    //CommonFunction.TextToSpeech(CommonFunction.audioTextLst.ehrdefaultconfigurationmissing, " for " + EHRname);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "SetPanelVisibilityEHRwise " + ex.Message);
            }
        }
        private void SetActivePozativeSync(bool isSyncActive, string ServiceName)
        {
            try
            {
                if (isSyncActive)
                {
                    picAditSync.Image = Resources.ON;
                    lblAditEmailId.Enabled = true;
                    lblAditPassword.Enabled = true;
                    txtAdminUserName.Text = string.Empty;
                    txtAdminPassword.Text = string.Empty;
                    txtAdminUserName.Enabled = true;
                    txtAdminPassword.Enabled = true;
                    btnAdminUserSave.Enabled = true;
                    txtAdminUserName.Focus();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "SetActivePozativeSync " + ex.Message);
            }
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
                                else
                                {
                                    //ViewtblEaglesoftPanel();
                                    SetPanelVisiblityEHRwise("Eaglesoft");
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
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "CreateDSN " + ex.Message);
                return false;
            }
        }
        public static bool DSNExists(string dsnName)
        {
            try
            {
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
                string ODBC_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";

                driverPath = "C:\\WINDOWS\\System32\\sqlsrv32.dll";

                var dsnKey = Registry.LocalMachine.CreateSubKey(ODBCINST_INI_REG_PATH + dsnName);
                if (dsnKey == null)
                {
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
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "CreateDSNEgaleSoft " + ex.Message);
            }
        }

        #endregion

        #region Button Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ObjGoalBase.WriteToSyncLogFile("[btnClose_Click] manually Application restart");
                System.Environment.Exit(1);
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "btnClose_click " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region EHR Configuration

        static string EHRRegInstallationPath = "";
        static string EHRRegVersion = "";
        private DataTable GetEHRList()
        {
            installedEHR = new List<string>();
            DataTable dtEHRList = new DataTable();
            try
            {
                dtEHRList.Clear();
                dtEHRList.Columns.Add("EHR_ID", typeof(Int32));
                dtEHRList.Columns.Add("EHR_Name", typeof(string));
                dtEHRList.Rows.Add(0, "<Select>");

                #region Temp Code for testing only
                //btn_abeldent.Visible = true;
                //btn_PWeb.Visible = true;
                //btn_EZ.Visible = true;
                //btn_Pwork.Visible = true;
                //btn_Tracker.Visible = true;
                //btn_CD.Visible = true;
                //btn_SoftDent.Visible = true;
                //btn_DTX.Visible = true;
                //btn_OD.Visible = true;
                //btn_EG.Visible = true;


                //dtEHRList.Rows.Add(1, "Eaglesoft");
                //dtEHRList.Rows.Add(2, "Open Dental");
                //dtEHRList.Rows.Add(3, "Dentrix");
                //dtEHRList.Rows.Add(4, "SoftDent");
                //dtEHRList.Rows.Add(5, "ClearDent");
                //dtEHRList.Rows.Add(6, "Tracker");
                //dtEHRList.Rows.Add(7, "PracticeWork");
                //dtEHRList.Rows.Add(8, "Easy Dental");
                //dtEHRList.Rows.Add(10, "PracticeWeb");
                //dtEHRList.Rows.Add(11, "AbelDent");
                //dtEHRList.AcceptChanges();
                //return dtEHRList;
                #endregion

                try
                {
                    if (IsAppInstalled("Patterson Eaglesoft"))
                    {
                        dtEHRList.Rows.Add(1, "Eaglesoft");
                        btn_EG.Visible = true;
                        installedEHR.Add("Eaglesoft");
                    }
                }
                catch
                {
                }
                try
                {
                    #region commented code
                    //string appKey = @"SOFTWARE\MakeMSI\KeyPaths";  //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\MakeMSI\KeyPaths
                    //using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    //{
                    //    if (registryKey != null)
                    //    {
                    //        string[] OpenDentalPath = registryKey.GetSubKeyNames();
                    //        if (OpenDentalPath.Contains("OpenDental"))
                    //        {
                    //            dtEHRList.Rows.Add(2, "Open Dental");
                    //            btn_OD.Visible = true;
                    //        }
                    //    }
                    //} 
                    #endregion
                    if (IsAppInstalled("OpenDental"))
                    {
                        dtEHRList.Rows.Add(2, "Open Dental");
                        btn_OD.Visible = true;
                        installedEHR.Add("Open Dental");
                    }
                }
                catch
                {
                }

                try
                {
                    if (IsAppInstalled("Dentrix"))
                    {
                        dtEHRList.Rows.Add(3, "Dentrix");
                        btn_DTX.Visible = true;
                        installedEHR.Add("Dentrix");
                    }
                }
                catch
                {
                }
                try
                {
                    string appKey = @"SOFTWARE\PWInc\PWSvr";  //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\PWInc\PWOffice
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            string IsPwork = registryKey.GetValue("PWSvrDir").ToString();
                            if (IsPwork.ToLower().Contains("softdent"))
                            {
                                dtEHRList.Rows.Add(4, "SoftDent");
                                btn_SoftDent.Visible = true;
                                installedEHR.Add("SoftDent");
                            }

                        }
                    }
                }
                catch
                {
                }
                try
                {
                    if (IsAppInstalled("ClearDent"))
                    {
                        dtEHRList.Rows.Add(5, "ClearDent");
                        btn_CD.Visible = true;
                        installedEHR.Add("ClearDent");
                    }
                }
                catch
                {
                }
                try
                {
                    if (IsAppInstalled("Tracker Server"))
                    {
                        dtEHRList.Rows.Add(6, "Tracker");
                        btn_Tracker.Visible = true;
                        installedEHR.Add("Tracker");
                    }
                    //if (IsAppInstalled("Tracker"))
                    //{
                    //   
                    //}
                }
                catch
                {
                }
                try
                {
                    string appKey = @"SOFTWARE\PWInc\PWSvr";  //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\PWInc\PWOffice
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            string IsPwork = registryKey.GetValue("PWSvrDir").ToString();
                            if (IsPwork.ToLower().Contains("pworks"))
                            {
                                dtEHRList.Rows.Add(7, "PracticeWork");
                                btn_Pwork.Visible = true;
                                installedEHR.Add("PracticeWork");
                            }

                        }
                    }
                }
                catch
                {
                }
                try
                {
                    if (IsAppInstalled("Easy Dental"))
                    {
                        dtEHRList.Rows.Add(8, "Easy Dental");
                        btn_EZ.Visible = true;
                        installedEHR.Add("Easy Dental");
                    }
                }
                catch
                {
                }
                try
                {
                    string appKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FeatureUsage\AppSwitched";
                    using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            foreach (string subKeyName in registryKey.GetValueNames())
                            {
                                if (subKeyName.Contains("FreeDental.exe"))
                                {
                                    dtEHRList.Rows.Add(10, "PracticeWeb");
                                    btn_PWeb.Visible = true;
                                    installedEHR.Add("PracticeWeb");
                                    break;
                                }
                            }
                        }
                    }


                }
                catch (Exception)
                {

                    throw;
                }
                try
                {
                    string appKey = @"SOFTWARE\ABELSoft\ABELDent";
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            dtEHRList.Rows.Add(11, "AbelDent");
                            btn_abeldent.Visible = true;
                            installedEHR.Add("AbelDent");
                        }
                    }


                }
                catch (Exception)
                {

                    throw;
                }
                //dtEHRList.Rows.Add(9, "Opendental Cloud");
                //dtEHRList.Rows.Add(10, "Practiceweb");
                //dtEHRList.Rows.Add(11, "AbelDent");

                dtEHRList.AcceptChanges();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "GetEHRList " + ex.Message);
            }

            return dtEHRList;
        }
        public string GetVersion(string EHRName)
        {
            try
            {
                string Version = "0.0.0.0";
                Version EHRVersion = new Version(Version);
                Version startversion = new Version();
                Version CurrentVersion = startversion;
                int EHRSelectedIndex = 0;
                DataTable dtEHRversionList = new DataTable();
                if (EHRName == "Eaglesoft")
                {
                    try
                    {
                        string EagleSoftPath = "";
                        string appKey = @"SOFTWARE\Eaglesoft\Paths";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                EagleSoftPath = registryKey.GetValue("Shared Files").ToString();
                            }

                        }
                        if (IsAppInstalled("Patterson Eaglesoft"))
                        {
                            if (EHRRegVersion == "")
                            {
                                if (EagleSoftPath != "")
                                {
                                    string configFilePath = EagleSoftPath + "\\PattersonUpgrade.exe.config"; // replace with the path to your external config file
                                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                                    fileMap.ExeConfigFilename = configFilePath;
                                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                                    ConfigurationSection cs = config.GetSection("runtime"); ///assemblyBinding/dependentAssembly/bindingRedirect"
                                    string sectionXml = cs.SectionInformation.GetRawXml();
                                    DataSet ds = new DataSet();
                                    ds.ReadXml(new System.IO.StringReader(sectionXml));
                                    EHRRegVersion = ds.Tables["bindingRedirect"].Rows[0]["newVersion"].ToString();
                                }

                                //Console.WriteLine("Value of MyKey in config file: " + configValue);

                            }
                            //dtEHRversionList = GetEaglesoftVersionList();
                            dtEHRversionList = GetEHRVersionList("Eaglesoft");
                            EHRSelectedIndex = GetVersionID(dtEHRversionList, "16.0.0.0", EHRRegVersion);
                        }

                    }
                    catch (Exception ex)
                    {


                    }
                }
                else if (EHRName == "Dentrix")
                {
                    string appKey = @"SOFTWARE\Dentrix Dental Systems, Inc.\Dentrix\General";
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            Version = registryKey.GetValue("Installed Version").ToString();
                            EHRVersion = new Version(Version);
                        }
                    }
                    //dtEHRversionList = GetDentrixVersionList();
                    dtEHRversionList = GetEHRVersionList("Dentrix");
                    if (EHRVersion.Major <= 15)
                    {
                        EHRSelectedIndex = 1;
                    }
                    else if (EHRVersion.Major <= 16 && EHRVersion.Minor < 20)
                    {
                        EHRSelectedIndex = 2;
                    }
                    else
                    {
                        EHRSelectedIndex = 3;
                    }
                }
                else if (EHRName == "ClearDent")
                {
                    string appKey = @"SOFTWARE\WOW6432Node\Prococious Technology Inc.\ClearDent\Version"; // Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Prococious Technology Inc.\ClearDent\Version
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            Version = registryKey.GetValue("Current ClearDent Version").ToString();
                            EHRVersion = new Version(Version);
                        }
                    }
                    //dtEHRversionList = GetClearDentVersionList();
                    dtEHRversionList = GetEHRVersionList("ClearDent");
                    EHRSelectedIndex = GetVersionID(dtEHRversionList, "8.0.0.0", Version);
                }
                else if (EHRName == "Easy Dental")
                {
                    //string appKey = @"SOFTWARE\Easy Dental Systems, Inc.\Easy Dental\General";
                    //using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    //{
                    //    if (registryKey != null)
                    //    {
                    //        Version = registryKey.GetValue("Installed Version").ToString().Split(' ')[0]; 
                    //        EHRVersion = new Version(Version);
                    //    }
                    //}
                    //dtEHRversionList = GetEasyDentalVersionList();
                    dtEHRversionList = GetEHRVersionList("EasyDental");
                    EHRSelectedIndex = 1;

                }
                else if (EHRName == "Open Dental")
                {
                    string appKey = @"SOFTWARE\WOW6432Node\MakeMSI\KeyPaths\OpenDental";  //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\MakeMSI\KeyPaths
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            string[] OpenDentalVersion = registryKey.GetSubKeyNames();
                            Version = OpenDentalVersion[0].ToString();
                            EHRVersion = new Version(Version);
                        }
                    }
                    //dtEHRversionList = GetOpenDentalVersionList();
                    dtEHRversionList = GetEHRVersionList("OpenDental");
                    EHRSelectedIndex = GetVersionID(dtEHRversionList, "14.0.0.0", Version);

                }
                else if (EHRName == "PracticeWork" || EHRName == "SoftDent" || EHRName == "Tracker" || EHRName == "PracticeWeb" || EHRName == "AbelDent")
                {
                    dtEHRversionList = GetEHRVersionList(EHRName);
                    EHRSelectedIndex = 1;
                }

                cmbEHRVersion.DataSource = dtEHRversionList;
                cmbEHRVersion.ValueMember = "Version_ID";
                cmbEHRVersion.DisplayMember = "Version_Name";
                cmbEHRVersion.SelectedValue = EHRSelectedIndex;
                //SetEHRConfigurations();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "GetVersion " + ex.Message);
            }
            return "";
        }
        public int GetVersionID(DataTable dtEHRVersionList, string sversion, string EHRVer)
        {
            int EHRSelectedIndex = 0;
            try
            {
                Version EHRVersion = new Version(EHRVer);
                Version startversion = new Version(sversion);
                Version CurrentVersion = startversion;
                EHRSelectedIndex = 0;
                try
                {
                    for (int i = 1; i < dtEHRVersionList.Rows.Count; i++)
                    {

                        string[] svarray = null;
                        string[] cvarray = null;
                        if (i > 1)
                        {
                            svarray = dtEHRVersionList.Rows[i - 1]["Version_name"].ToString().Replace("+", "").Split('.');
                            cvarray = dtEHRVersionList.Rows[i]["Version_name"].ToString().Replace("+", "").Split('.');
                        }
                        else
                        {
                            svarray = startversion.ToString().Split('.');
                            cvarray = dtEHRVersionList.Rows[i]["Version_name"].ToString().Replace("+", "").Split('.');
                        }
                        if (svarray.Length > 1)
                        {
                            startversion = new Version(Convert.ToInt16(svarray[0]), Convert.ToInt16(svarray[1]));
                        }
                        else
                        {
                            startversion = new Version(Convert.ToInt16(svarray[0]), 0);
                            //CurrentVersion = new Version(dt.Rows[i - (dt.Rows.Count - 1)][1].ToString()); 
                        }
                        if (cvarray.Length > 1)
                        {
                            CurrentVersion = new Version(Convert.ToInt16(cvarray[0]), Convert.ToInt16(cvarray[1]));
                            //EHRSelectedIndex = Convert.ToInt16(dt.Rows[i + 1]["Id"]);
                        }
                        else
                        {
                            CurrentVersion = new Version(Convert.ToInt16(cvarray[0]), 0);
                            //EHRSelectedIndex = Convert.ToInt16(dt.Rows[i + 1]["Id"]);
                        }
                        if (EHRVersion >= startversion && EHRVersion <= CurrentVersion)
                        {
                            EHRVersion = startversion;
                            EHRSelectedIndex = Convert.ToInt16(dtEHRVersionList.Rows[i - 1]["Version_Id"]);
                            break;
                        }
                        //i++;
                    }
                }
                catch { }
                if (EHRSelectedIndex == 0)
                {
                    EHRSelectedIndex = Convert.ToInt32(dtEHRVersionList.Rows[dtEHRVersionList.Rows.Count - 1]["Version_Id"]);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "GetVersionID " + ex.Message);
            }
            return EHRSelectedIndex;
        }
        private void SetMultiClinicForOpenDental()
        {
            try
            {
                dtTempOpenDentalClinicTable = SynchOpenDentalBAL.GetOpenDentalClinicData(Utility.DBConnString);

                if (dtTempOpenDentalClinicTable.Rows.Count > 1)
                {
                    RBtnSingleClinic.Visible = true;
                    RBtnMultiClinic.Visible = true;
                    RBtnMultiClinic.Checked = true;
                }
                else
                {
                    RBtnSingleClinic.Checked = true;
                    RBtnSingleClinic.Visible = false;
                    RBtnMultiClinic.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "SetMultiClinicForOpenDental " + ex.Message);
            }
        }
        private void btnEHRSave_Click(object sender, EventArgs e)
        {
            SetEHRConfigurations();
        }
        private void SetEHRConfigurations()
        {
            try
            {
                bool isEHRInstalled = false;
                Cursor.Current = Cursors.WaitCursor;

                dtTempOpenDentalClinicTable.Rows.Clear();
                RBtnSingleClinic.Checked = true;
                RBtnMultiClinic.Checked = false;

                RBtnSingleClinic.Visible = false;
                RBtnMultiClinic.Visible = false;
                tblpnMultiClinic.Visible = false;
                if (cmbEHRName.SelectedIndex == 0)
                {
                    ObjGoalBase.ErrorMsgBox("EHR Name", "Please select valid EHR");
                    cmbEHRName.Focus();
                    return;
                }
                if (cmbEHRVersion.SelectedIndex == 0)
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Please select valid EHR Version");
                    cmbEHRVersion.Focus();
                    return;
                }

                switch (Convert.ToInt16(cmbEHRName.SelectedValue))
                {
                    case 1:
                        if (cmbEHRVersion.Text.ToLower() == "22.00".ToLower() || cmbEHRVersion.Text.ToLower() == "21.20".ToLower() || cmbEHRVersion.Text.ToLower() == "20.0".ToLower() || cmbEHRVersion.Text.ToLower() == "19.11".ToLower() || cmbEHRVersion.Text.ToLower() == "18.0".ToLower() || cmbEHRVersion.Text.ToLower() == "17".ToLower())
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            string EaglesoftConStr = "";

                            Process myProcess = new Process();
                            myProcess.StartInfo.UseShellExecute = false;
                            string DPath = Application.StartupPath;
                            myProcess.StartInfo.FileName = DPath + "\\PTIAuthenticator.exe";
                            myProcess.StartInfo.CreateNoWindow = true;
                            myProcess.StartInfo.Verb = "runas";
                            if (cmbEHRVersion.Text.ToLower() == "21.20".ToLower() || cmbEHRVersion.Text.ToLower() == "22.00".ToLower())
                            {
                                myProcess.StartInfo.Arguments = "true false S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e true " + cmbEHRVersion.Text.ToLower();
                            }
                            else
                            {
                                myProcess.StartInfo.Arguments = "true true S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e false " + cmbEHRVersion.Text.ToLower();
                            }
                            myProcess.Start();
                            myProcess.WaitForExit();

                            string DentrixConFileName = Application.StartupPath + "\\ConnectionString.txt";
                            try
                            {
                                using (StreamReader sr = new StreamReader(DentrixConFileName))
                                {
                                    string ConnectionString = "";
                                    ConnectionString = sr.ReadToEnd().ToString().Trim();
                                    EaglesoftConStr = ConnectionString;
                                    if (cmbEHRVersion.Text.ToLower() != "21.20".ToLower())
                                    {
                                        EaglesoftConStr = Utility.DecryptString(ConnectionString);
                                    }
                                }
                                //IsEHRConnected = SystemBAL.GetEHREagleSoftConnection();
                            }
                            catch (Exception ex)
                            {
                                ObjGoalBase.ErrorMsgBox("Pozative Service", "SetEHRConfigurations_  Eaglesoft connection file decrypt" + ex.Message);
                                SaveEHRErroLog(errorMessage: "Eaglesoft connection file decrypt" + ex.Message);
                                configErrors += "\n Eaglesoft connection file decrypt issue.";
                            }


                            #region commented code
                            //Assembly.LoadFile(Application.StartupPath + "\\Patterson.PTCBaseObjects.SharedObjects.dll");
                            //var DLL = Assembly.LoadFile(Application.StartupPath + "\\EaglesoftSettings.dll");

                            //foreach (Type type in DLL.GetExportedTypes())
                            //{
                            //    if (type.Name == "EaglesoftSettings")
                            //    {
                            //        dynamic settings = Activator.CreateInstance(type);
                            //        EaglesoftConStr = settings.GetLegacyConnectionString(true);
                            //        //type.InvokeMember("GetConnection", BindingFlags.InvokeMethod, null,c, new object[] { @"Hello" });
                            //        //var cs = c.GetConnection();

                            //    }
                            //} 
                            #endregion


                            Utility.APISessionToken = "";
                            Utility.EHRHostname = "";
                            Utility.EHRIntegrationKey = "";
                            Utility.EHRUserId = "";
                            Utility.EHRPassword = "";
                            Utility.EHRDatabase = "Primary Database";
                            Utility.EHRPort = string.Empty;
                            Utility.DBConnString = EaglesoftConStr;
                            #region commented code
                            //}



                            //if (cmbEHRVersion.Text.ToLower() == "18.0".ToLower())
                            //{
                            //    Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
                            //    dynamic settings = Activator.CreateInstance(esSettingsType);

                            //    bool tokenIsValid = settings.SetToken("Adit", "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4");
                            //    // bool tokenIsValid = settings.SetToken("EagleSoft", "37444a4f485652495700b506f62098957dec9a1a8e5401f4141b8b8870552340");
                            //    EaglesoftConStr = settings.GetLegacyConnectionString(true);
                            //}
                            //else if (cmbEHRVersion.Text.ToLower() == "20.0".ToLower() || cmbEHRVersion.Text.ToLower() == "19.11".ToLower() || cmbEHRVersion.Text.ToLower() == "17".ToLower())
                            //{

                            //} 
                            #endregion
                            #region Create DSN For EagleSoft
                            if (CreateDSN())
                            {
                                #region commented code
                                //  EaglesoftConStr = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION=" + EaglesoftConStr;


                                //string[] StrData = EaglesoftConStr.Split(';');

                                //if (StrData.Count() > 0)
                                //{
                                //    Utility.EHRDatabase = StrData[0].ToString().Substring(4);
                                //    Utility.EHRHostname = StrData[1].ToString().Substring(4);
                                //    Utility.EHRUserId = StrData[2].ToString().Substring(4);
                                //    Utility.EHRPassword = StrData[3].ToString().Substring(4);
                                //} 
                                #endregion
                                PostEHRConnectionLog("Eaglesoft", true);

                                //ViewAdminConfigurationPanel();
                                SetPanelVisiblityEHRwise("Location", false);
                            }
                            else
                            {
                                PostEHRConnectionLog("Eaglesoft", false);
                                //ViewtblEaglesoftPanel();
                                SetPanelVisiblityEHRwise("Eaglesoft");
                            }
                            #endregion
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                            //ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            //cmbEHRVersion.Focus();
                        }
                        break;

                    case 2:
                        if (cmbEHRVersion.Text.ToLower() == "15.4".ToLower() || cmbEHRVersion.Text.ToLower() == "17.2+".ToLower())
                        {

                            Utility.DBConnString = "server=localhost;port=3306;database=opendental;uid=root;pwd=;default command timeout=120;";
                            bool IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);

                            if (IsEHRConnected)
                            {
                                SetMultiClinicForOpenDental();
                                //isProcessComplete = true;
                                SetPanelVisiblityEHRwise("Location", false);
                            }
                            else
                            {
                                SetPanelVisiblityEHRwise("OpenDental");
                                configErrors += "\n OpenDental not connecting automatically.";
                            }
                            PostEHRConnectionLog("OpenDental", IsEHRConnected);
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;

                    case 3:
                        //  btnDentrixSave_Click(null, null);

                        if (cmbEHRVersion.Text.ToLower() == "DTX G5".ToLower() || cmbEHRVersion.Text.ToLower() == "DTX G6".ToLower() || cmbEHRVersion.Text.ToLower() == "DTX G6.2+".ToLower())
                        {
                            //MessageBox.Show("EHRSaveClick");
                            //ViewtblDentrixPanel();
                            SetPanelVisiblityEHRwise("Dentrix");
                            btnDentrixSave.PerformClick();

                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;

                    case 4:
                        if (cmbEHRVersion.Text.ToLower() == "17.0.0+".ToLower())
                        {
                            SetPanelVisiblityEHRwise("Eaglesoft");
                            SaveSoftDentConfigurtion();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;
                    case 5:

                        if (cmbEHRVersion.Text.ToLower() == "9.8+".ToLower() || cmbEHRVersion.Text.ToLower() == "9.10+".ToLower())
                        {
                            //ClearDentConfigurtion();
                            //ViewtblClearDentPanel();
                            SetPanelVisiblityEHRwise("ClearDent");
                            btnClearDentSave.PerformClick();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;
                    case 6:

                        if (cmbEHRVersion.Text.ToLower() == "11.29".ToLower())
                        {
                            //cmbTrackerHostName.DataSource = null;
                            //cmbSqlServiceName.DataSource = null;
                            //ViewtblTrackerPanel();
                            SetPanelVisiblityEHRwise("Tracker");
                            btnTrackerSave.PerformClick();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;
                    case 7:
                        if (cmbEHRVersion.Text.ToLower() == "7.9+".ToLower())
                        {
                            //ViewPracticeWorkPanel();
                            SetPanelVisiblityEHRwise("PracticeWork");
                            btnPracticeWorkSave.PerformClick();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;
                    case 8: //Easy Dental
                        if (cmbEHRVersion.Text.ToLower() == "11.1".ToLower())
                        {
                            Utility.DBConnString = "DSN=EZD2011;DBQ=.;SERVER=NotTheServer;";
                            //ViewAdminConfigurationPanel();
                            SetPanelVisiblityEHRwise("Eaglesoft");
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;
                    case 9:
                        if (cmbEHRVersion.Text.ToLower() == "15.4".ToLower() || cmbEHRVersion.Text.ToLower() == "17.2+".ToLower())
                        {
                            //ViewtblOpenDentalPanel();
                            SetPanelVisiblityEHRwise("OpenDental");
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;
                    case 10:
                        if (cmbEHRVersion.Text.ToLower() == "21.1".ToLower())
                        {
                            Utility.DBConnString = "server=localhost;port=3306;database=freedental;uid=root;pwd=;default command timeout=120;";
                            bool IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);
                            if (IsEHRConnected)
                            {
                                //ViewtblPracticeWebPanel();
                                SetPanelVisiblityEHRwise("Location", false);
                            }
                            else
                            {
                                SetPanelVisiblityEHRwise("PracticeWeb");
                                configErrors += "\n PracticeWeb not connecting automatically.";
                            }
                            PostEHRConnectionLog("PracticeWeb", IsEHRConnected);
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;
                    case 11:
                        if (cmbEHRVersion.Text.ToLower() == "14.8.2".ToLower())
                        {
                            try
                            {
                                string appKey = @"SOFTWARE\ABELSoft\ABELDent";
                                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                                {
                                    if (registryKey != null)
                                    {
                                        Utility.EHRHostname = registryKey.GetValue("SQLServer").ToString();
                                        Utility.EHRDatabase = registryKey.GetValue("Database").ToString();
                                    }
                                }
                                Utility.EHRUserId = "";
                                Utility.EHRPassword = "";
                                Utility.EHRPort = "";
                                //Data Source=192.168.1.197\aditbeta;Initial Catalog=Abel;User id=sa;Password=Adit@123
                                if (Utility.EHRUserId == string.Empty || Utility.EHRPassword == string.Empty)
                                {
                                    Utility.DBConnString = "Server=" + Utility.EHRHostname + ";Database=" + Utility.EHRDatabase + ";Trusted_Connection=True;";
                                }
                                else
                                {
                                    Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                                }
                                bool IsEHRConnected = SystemBAL.GetAbelDentConnection();
                                if (IsEHRConnected)
                                {
                                    //ViewAdminConfigurationPanel();
                                    SetPanelVisiblityEHRwise("Location", false);
                                }
                                else
                                {
                                    //ViewtblAbelDentPanel();
                                    SetPanelVisiblityEHRwise("AbelDent");
                                    configErrors += "\n AbelDent not connecting automatically.";
                                }
                                PostEHRConnectionLog("AbelDent", IsEHRConnected);
                            }
                            catch (Exception ex)
                            {
                                ObjGoalBase.ErrorMsgBox("CheckAbelDentConnection", "AbelDent is not connecting. " + "\n" + " Please enter valid credentials." + ex.Message.ToString());
                                SaveEHRErroLog(errorMessage: "CheckAbelDentConnection" + ex.Message);
                                configErrors += "\n AbleDent connection issue.";
                                throw;
                            }
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                            cmbEHRName.Focus();
                        }
                        break;
                    default:
                        ObjGoalBase.ErrorMsgBox("Pozative Service integration", "We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.");
                        //ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " is underdevelopment." + "\n" + "Please select another EHR Name.");
                        cmbEHRName.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "SetEHRConfigurations " + ex.Message);
                SaveEHRErroLog(errorMessage: "Pozative Service" + ex.Message);
                configErrors += "\n Pozative Service issue.";
            }
            finally
            {

                Cursor.Current = Cursors.Default;
            }
        }
        private bool EaglesoftConnected()
        {
            bool connected;
            try
            {
                string EaglesoftConStr = "";

                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = false;
                string DPath = Application.StartupPath;
                myProcess.StartInfo.FileName = DPath + "\\PTIAuthenticator.exe";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.Verb = "runas";
                if (cmbEHRVersion.Text.ToLower() == "21.20".ToLower() || cmbEHRVersion.Text.ToLower() == "22.00".ToLower())
                {
                    myProcess.StartInfo.Arguments = "true false S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e true " + cmbEHRVersion.Text.ToLower();
                }
                else
                {
                    myProcess.StartInfo.Arguments = "true true S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e false " + cmbEHRVersion.Text.ToLower();
                }
                myProcess.Start();
                myProcess.WaitForExit();

                string DentrixConFileName = Application.StartupPath + "\\ConnectionString.txt";
                try
                {
                    using (StreamReader sr = new StreamReader(DentrixConFileName))
                    {
                        string ConnectionString = "";
                        ConnectionString = sr.ReadToEnd().ToString().Trim();
                        EaglesoftConStr = ConnectionString;
                        if (cmbEHRVersion.Text.ToLower() != "21.20".ToLower())
                        {
                            EaglesoftConStr = Utility.DecryptString(ConnectionString);
                        }
                    }
                    //IsEHRConnected = SystemBAL.GetEHREagleSoftConnection();
                }
                catch (Exception ex)
                {
                    ObjGoalBase.ErrorMsgBox("Pozative Service", "SetEHRConfigurations_  Eaglesoft connection file decrypt" + ex.Message);
                    SaveEHRErroLog(errorMessage: "Eaglesoft connection file decrypt" + ex.Message);
                    configErrors += "\n Eaglesoft connection file decrypt issue.";
                }


                #region commented code
                //Assembly.LoadFile(Application.StartupPath + "\\Patterson.PTCBaseObjects.SharedObjects.dll");
                //var DLL = Assembly.LoadFile(Application.StartupPath + "\\EaglesoftSettings.dll");

                //foreach (Type type in DLL.GetExportedTypes())
                //{
                //    if (type.Name == "EaglesoftSettings")
                //    {
                //        dynamic settings = Activator.CreateInstance(type);
                //        EaglesoftConStr = settings.GetLegacyConnectionString(true);
                //        //type.InvokeMember("GetConnection", BindingFlags.InvokeMethod, null,c, new object[] { @"Hello" });
                //        //var cs = c.GetConnection();

                //    }
                //} 
                #endregion


                Utility.APISessionToken = "";
                Utility.EHRHostname = "";
                Utility.EHRIntegrationKey = "";
                Utility.EHRUserId = "";
                Utility.EHRPassword = "";
                Utility.EHRDatabase = "Primary Database";
                Utility.EHRPort = string.Empty;
                Utility.DBConnString = EaglesoftConStr;
                #region commented code
                //}



                //if (cmbEHRVersion.Text.ToLower() == "18.0".ToLower())
                //{
                //    Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
                //    dynamic settings = Activator.CreateInstance(esSettingsType);

                //    bool tokenIsValid = settings.SetToken("Adit", "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4");
                //    // bool tokenIsValid = settings.SetToken("EagleSoft", "37444a4f485652495700b506f62098957dec9a1a8e5401f4141b8b8870552340");
                //    EaglesoftConStr = settings.GetLegacyConnectionString(true);
                //}
                //else if (cmbEHRVersion.Text.ToLower() == "20.0".ToLower() || cmbEHRVersion.Text.ToLower() == "19.11".ToLower() || cmbEHRVersion.Text.ToLower() == "17".ToLower())
                //{

                //} 
                #endregion
                #region Create DSN For EagleSoft
                connected = CreateDSN();
            }
            catch (Exception ex)
            {

                throw;
            }
            return connected;
        }
        private void btnEHRCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetPanelVisiblityEHRwise("Main", false);
                //Cursor.Current = Cursors.WaitCursor;
                //ObjGoalBase.WriteToSyncLogFile("[btnEHRCancel_Click] manually Application restart");
                //System.Environment.Exit(1);
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "btnEHRCancel_Click " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        #endregion

        #region Eaglesoft Configuration

        private async void btnEaglesoftSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                if (string.IsNullOrEmpty(txtEaglesoftHostName.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("Hostname", "Please Enter valid Eaglesoft ServerName");
                    txtEaglesoftHostName.Focus();
                    return;
                }

                if (txtEaglesoftUserId.Visible && string.IsNullOrEmpty(txtEaglesoftUserId.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("UserId", "Please Enter valid Eaglesoft User Id");
                    txtEaglesoftUserId.Focus();
                    return;
                }
                if (txtEaglesoftPassword.Visible && string.IsNullOrEmpty(txtEaglesoftPassword.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("Password", "Please Enter valid Eaglesoft Password");
                    txtEaglesoftPassword.Focus();
                    return;
                }

                if (!txtEaglesoftUserId.Visible && txtEaglesoftHostName.Visible)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    #region commented code

                    //  Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
                    //  dynamic settings = Activator.CreateInstance(esSettingsType);

                    ////  bool tokenIsValid = settings.SetToken("Adit", "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4");
                    //  bool tokenIsValid = settings.SetToken("EagleSoft", "37444a4f485652495700b506f62098957dec9a1a8e5401f4141b8b8870552340");

                    //  string EaglesoftConStr = settings.GetLegacyConnectionString(true);


                    #endregion
                    string EaglesoftConStr = "";
                    Assembly.LoadFile(Application.StartupPath + "\\Patterson.PTCBaseObjects.SharedObjects.dll");
                    var DLL = Assembly.LoadFile(Application.StartupPath + "\\EaglesoftSettings.dll");
                    foreach (Type type in DLL.GetExportedTypes())
                    {
                        if (type.Name == "EaglesoftSettings")
                        {
                            dynamic settings = Activator.CreateInstance(type);
                            EaglesoftConStr = settings.GetLegacyConnectionString(true);
                            //type.InvokeMember("GetConnection", BindingFlags.InvokeMethod, null,c, new object[] { @"Hello" });
                            //var cs = c.GetConnection();
                        }
                    }
                    #region Create DSN For EagleSoft
                    try
                    {
                        //EaglesoftConStr = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION=" + EaglesoftConStr;                  
                        EaglesoftConStr = EaglesoftConStr.Replace("DSN=DENTAL;", "DSN=PozativeDSN;");
                        EaglesoftConStr = EaglesoftConStr.Replace("DSN =DENTAL;", "DSN=PozativeDSN;");
                        EaglesoftConStr = EaglesoftConStr.Replace("DSN= DENTAL;", "DSN=PozativeDSN;");
                        EaglesoftConStr = EaglesoftConStr.Replace("DSN = DENTAL;", "DSN=PozativeDSN;");

                        Utility.APISessionToken = "";
                        Utility.EHRHostname = "";
                        Utility.EHRIntegrationKey = "";
                        Utility.EHRUserId = "";
                        Utility.EHRPassword = "";
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;
                        Utility.DBConnString = EaglesoftConStr;
                        // Utility.EHRDocPath = SynchEaglesoftBAL.GetEagleSoftDocPath(ConnectionString);
                        Utility.DontAskPasswordOnSaveSetting = false;
                        Utility.NotAllowToChangeSystemDateFormat = false;
                        string[] StrData = EaglesoftConStr.Split(';');

                        if (StrData.Count() > 0)
                        {
                            Utility.EHRDatabase = StrData[0].ToString().Substring(4);
                            Utility.EHRHostname = StrData[1].ToString().Substring(4);
                            Utility.EHRUserId = StrData[2].ToString().Substring(4);
                            Utility.EHRPassword = StrData[3].ToString().Substring(4);
                        }

                        if (!CreateDSN(txtEaglesoftHostName.Text.Trim()))
                        {
                            PostEHRConnectionLog("Eaglesoft", false);
                            ObjGoalBase.InformationMsgBox("Auto create DSN", "DSN might not been created Automatically... Please create DSN manually after configuration");
                            configErrors += "Eaglesoft DSN not been created Automatically.";
                        }
                        else
                            PostEHRConnectionLog("Eaglesoft", true);
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.ErrorMsgBox("Pozative Service", "btnEagleSoftSave " + ex.Message);
                        SaveEHRErroLog(errorMessage: ex.Message);
                        configErrors += "\n Eaglesoft connection issue.";
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }

                    //ViewAdminConfigurationPanel();
                    SetPanelVisiblityEHRwise("Location", false);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n Eaglesoft connection issue.";
                ObjGoalBase.ErrorMsgBox("Pozative Service", "btnEagleSoftSave " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnEaglesoftBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cmbEHRName.Items.Count == 2)
                {
                    cmbEHRName.SelectedIndex = 1;
                    SetEHRConfigurations();
                }
                else
                {
                    SetPanelVisiblityEHRwise("Main", false);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "btnEagleSoftBack_Click " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        #endregion

        #region OpenDental Configuration

        private void btnOpenDentalSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (cmbEHRVersion.Text.ToLower() == "15.4" || cmbEHRVersion.Text.ToLower() == "17.2+")
                {

                    if (string.IsNullOrEmpty(txtDentrixHostName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Hostname", "Please Enter valid OpenDental Hostname");
                        txtOpenDentalHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtOpenDentalDatabase.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Port", "Please Enter valid OpenDental Database Name");
                        txtOpenDentalDatabase.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtDentrixUserId.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("IntegrationKey", "Please Enter valid OpenDental User Id");
                        txtOpenDentalUserId.Focus();
                        return;
                    }


                    Utility.EHRHostname = txtOpenDentalHostName.Text.ToLower().Trim();
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRDatabase = txtOpenDentalDatabase.Text.Trim();
                    Utility.EHRUserId = txtOpenDentalUserId.Text.Trim();
                    Utility.EHRPassword = txtOpenDentalPassword.Text.Trim();
                    Utility.EHRDocPath = string.Empty;
                    Utility.EHRPort = "3306";
                    Utility.DontAskPasswordOnSaveSetting = false;
                    Utility.NotAllowToChangeSystemDateFormat = false;

                    Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";
                    bool IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);

                    if (IsEHRConnected)
                    {
                        SetMultiClinicForOpenDental();
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        Utility.DBConnString = "";
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;

                        ObjGoalBase.ErrorMsgBox("Authentication", "OpenDental is not connecting. " + "\n" + " Please enter valid credentials.");
                        configErrors += "\n OpenDental not connected.";
                    }
                    PostEHRConnectionLog("Open Dental-"+ Utility.DBConnString, IsEHRConnected);
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "btnOpenDentalSave_click " + ex.Message);
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n OpenDental connection issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Dentrix Configuration
        #region Constants
        const int SUCCESS = 0;
        const int FAIL = 1;
        #endregion
        private void btnDentrixSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Utility.Application_Version = cmbEHRVersion.Text.Trim();
                bool IsEHRConnected = false;
                if (
                       (cmbEHRVersion.Text.ToLower() == "DTX G5".ToLower())
                    || (cmbEHRVersion.Text.ToLower() == "DTX G5.1".ToLower())
                    || (cmbEHRVersion.Text.ToLower() == "DTX G5.2".ToLower())
                    || (cmbEHRVersion.Text.ToLower() == "DTX G6".ToLower())
                    || (cmbEHRVersion.Text.ToLower() == "DTX G6.1".ToLower())
                    )
                {
                    //ViewtblDentrixPanel();
                    SetPanelVisiblityEHRwise("Dentrix");

                    if (string.IsNullOrEmpty(txtDentrixHostName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Hostname", "Please Enter valid Dentrix Hostname");
                        txtDentrixHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtDentrixUserId.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("IntegrationKey", "Please Enter valid Dentrix User Id");
                        txtDentrixUserId.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtDentrixPassword.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("UserId", "Please Enter valid Dentrix Password");
                        txtDentrixPassword.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtDentrixPort.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Port", "Please Enter valid Dentrix Port");
                        txtDentrixPort.Focus();
                        return;
                    }
                    Utility.EHRHostname = txtDentrixHostName.Text.ToLower().Trim();
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = txtDentrixUserId.Text.Trim();
                    Utility.EHRPassword = txtDentrixPassword.Text.Trim();
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = txtDentrixPort.Text.Trim();
                    Utility.DBConnString = "host=" + Utility.EHRHostname + ";UID=" + Utility.EHRUserId + ";PWD=" + Utility.EHRPassword + ";Database=DentrixSQL;DSN=c-treeACE ODBC Driver;port=" + Utility.EHRPort + string.Empty;
                    IsEHRConnected = SystemBAL.GetEHRDentrixConnection();

                    Utility.NotAllowToChangeSystemDateFormat = picNotAllowToChangeSystemDateFormat.Tag.ToString() == "ON" ? true : false;
                    if (IsEHRConnected)
                    {
                        Cursor.Current = Cursors.Default;
                        //ViewAdminConfigurationPanel();
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;
                        ObjGoalBase.ErrorMsgBox("Authentication", "Dentrix is not connecting. " + "\n" + " Please enter valid credentials.");
                    }
                }
                else
                {

                    Utility.EHRHostname = txtDentrixHostName.Text.ToLower().Trim();
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = txtDentrixUserId.Text.Trim();
                    Utility.EHRPassword = txtDentrixPassword.Text.Trim();
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = txtDentrixPort.Text.Trim();
                    Utility.NotAllowToChangeSystemDateFormat = picNotAllowToChangeSystemDateFormat.Tag.ToString() == "ON" ? true : false;
                    string exePath = string.Empty;
                    StringBuilder path = new StringBuilder(512);
                    string connectionString = string.Empty;
                    if (CommonFunction.GetDentrixG62ExePath(path) == SUCCESS)
                    {
                        exePath = path.ToString();
                        ObjGoalBase.WriteToErrorLogFile(exePath);
                        CommonFunction.GetDentrixG62ConnectionString();
                        IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                        if (IsEHRConnected)
                        {
                            Cursor.Current = Cursors.Default;
                            //ViewAdminConfigurationPanel();
                            SetPanelVisiblityEHRwise("Location", false);
                        }
                        else
                        {
                            GetDentrixConnectionString(exePath);
                            IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                            if (IsEHRConnected)
                            {
                                Cursor.Current = Cursors.Default;
                                //ViewAdminConfigurationPanel();
                                SetPanelVisiblityEHRwise("Location", false);
                            }
                        }
                        //  ObjGoalBase.ErrorMsgBox("EHR User", "DENTRIXAPI_RegisterUser returning code is :" + DENTRIXAPI_RegisterUser(GoalBase.DentrixG62keyFilePath).ToString() + " {0}");
                        //CommonUtility.GetDentrixDLLForDocumentUpload(exePath);
                    }
                    else
                    {
                        exePath = path.ToString();
                        ObjGoalBase.WriteToErrorLogFile(exePath);
                        CommonFunction.GetDentrixG62ConnectionString();
                        IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                        if (IsEHRConnected)
                        {
                            Cursor.Current = Cursors.Default;
                            //ViewAdminConfigurationPanel();
                            SetPanelVisiblityEHRwise("Location", false);
                        }
                        else
                        {
                            GetDentrixConnectionString(exePath);
                            IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                            if (IsEHRConnected)
                            {
                                Cursor.Current = Cursors.Default;
                                //ViewAdminConfigurationPanel();
                                SetPanelVisiblityEHRwise("Location", false);
                            }
                        }

                    }
                }
                PostEHRConnectionLog("Dentrix-"+ Utility.DBConnString, IsEHRConnected);
                if(!IsEHRConnected)
                    configErrors += "\n Dentrix not connected.";
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "btnDentrixSave_click " + ex.Message);
                //ViewAdminConfigurationPanel();
                SetPanelVisiblityEHRwise("Main", false);
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n Dentrix connection issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public static string GetDentrixConnectionString(String DentrixPath)
        {
            try
            {
                bool IsEHRConnected = false;

                DENTRIXAPI_GetConnectionString:
                string SPath = Application.StartupPath;
                string DPath = Application.StartupPath + "\\DTX_Helper";
                if (!Directory.Exists(DPath))
                {
                    Directory.CreateDirectory(DPath);
                }
                string DentrixConFileName = Application.StartupPath + "\\DTX_Helper" + "\\DentrixConnectionString.txt";
                if (File.Exists(DentrixConFileName))
                {
                    string ConnectionString = "";
                    using (StreamReader sr = new StreamReader(DentrixConFileName))
                    {
                        ConnectionString = sr.ReadToEnd().ToString().Trim();
                        Utility.DBConnString = ConnectionString;
                    }
                    IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                }

                if (!IsEHRConnected)
                {
                    string ExeFileName = SPath + "\\DTX_Helper.exe";
                    if (!Directory.Exists(DPath))
                    {
                        Directory.CreateDirectory(DPath);
                    }
                    File.Copy(ExeFileName, DPath + "\\DTX_Helper.exe", true);
                    File.Copy(SPath + "\\DTX_Helper.exe.config", DPath + "\\DTX_Helper.exe.config", true);
                    string fileSourcePath = DentrixPath.Substring(0, DentrixPath.LastIndexOf("\\"));
                    string FileTargetPath = DPath;
                    string sourceFile = "", destFile = "";
                    destFile = Path.Combine(FileTargetPath, "Ctree.Data.SqlClient.dll");
                    if (!File.Exists(destFile))
                    {
                        sourceFile = Path.Combine(fileSourcePath, "Ctree.Data.SqlClient.dll");
                        File.Copy(sourceFile, destFile, true);
                    }
                    destFile = Path.Combine(FileTargetPath, "Dentrix.API.Common.dll");
                    if (!File.Exists(destFile))
                    {
                        sourceFile = Path.Combine(fileSourcePath, "Dentrix.API.Common.dll");
                        File.Copy(sourceFile, destFile, true);
                    }
                    destFile = Path.Combine(FileTargetPath, "Dtx.Common.dll");
                    if (!File.Exists(destFile))
                    {
                        sourceFile = Path.Combine(fileSourcePath, "Dtx.Common.dll");
                        File.Copy(sourceFile, destFile, true);
                    }
                    destFile = Path.Combine(FileTargetPath, "Persist.dll");
                    if (!File.Exists(destFile))
                    {
                        sourceFile = Path.Combine(fileSourcePath, "Persist.dll");
                        File.Copy(sourceFile, destFile, true);
                    }
                    sourceFile = Path.Combine(fileSourcePath, "Persist.Common.dll");
                    if (File.Exists(sourceFile))
                    {
                        destFile = Path.Combine(FileTargetPath, "Persist.Common.dll");
                        File.Copy(sourceFile, destFile, true);
                    }
                    if (File.Exists(ExeFileName))
                    {

                        #region call Dentrix6.2+
                        Process myProcess = new Process();
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = DPath + "\\DTX_Helper.exe";
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.StartInfo.Verb = "runas";
                        myProcess.Start();
                        myProcess.WaitForExit();
                        //DentrixConFileName = Application.StartupPath + "\\DTX_Helper" + "\\DentrixConnectionString.txt";
                        try
                        {
                            using (StreamReader sr = new StreamReader(DentrixConFileName))
                            {
                                Utility.DBConnString = sr.ReadToEnd().ToString().Trim();
                            }
                            IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                            if (!IsEHRConnected)
                            {
                                sourceFile = Path.Combine(fileSourcePath, "Ctree.Data.SqlClient.dll");
                                destFile = Path.Combine(FileTargetPath, "Ctree.Data.SqlClient.dll");
                                File.Copy(sourceFile, destFile, true);
                                sourceFile = Path.Combine(fileSourcePath, "Dentrix.API.Common.dll");
                                destFile = Path.Combine(FileTargetPath, "Dentrix.API.Common.dll");
                                File.Copy(sourceFile, destFile, true);
                                sourceFile = Path.Combine(fileSourcePath, "Dtx.Common.dll");
                                destFile = Path.Combine(FileTargetPath, "Dtx.Common.dll");
                                File.Copy(sourceFile, destFile, true);
                                sourceFile = Path.Combine(fileSourcePath, "Persist.dll");
                                destFile = Path.Combine(FileTargetPath, "Persist.dll");
                                File.Copy(sourceFile, destFile, true);

                                goto DENTRIXAPI_GetConnectionString;
                            }
                        }
                        catch (Exception ex2)
                        {
                            //  ObjGoalBase.WriteToErrorLogFile("Error During Generating Connection String : " + ex2.Message);
                            Utility.DBConnString = "";
                            return "";
                        }
                        #endregion
                    }
                }
                return Utility.DBConnString;
            }
            catch (Exception ex)
            {
                Utility.DBConnString = "";
                return "";
            }

        }
        #endregion

        #region Pozative Admin Configuration

        private void picAditSync_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (Utility.AditSync)
                {
                    SetActivePozativeSync(false, "Adit");
                }
                else
                {
                    SetActivePozativeSync(true, "Adit");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "picAditSync_click " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void picPozativeSync_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (Utility.PozativeSync)
                {
                    SetActivePozativeSync(false, "Pozative");
                }
                else
                {
                    SetActivePozativeSync(true, "Pozative");
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "picPozativeSync_click " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnAdminUserSave_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;

                #region Adit Sync

                if (string.IsNullOrEmpty(txtAdminUserName.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("Adit App EmailId", "Please enter valid Adit User Email Id");
                    txtAdminUserName.Focus();
                    return;
                }

                //if (!Utility.IsValidEmailAddress(txtAdminUserName.Text))
                //{
                //    ObjGoalBase.ErrorMsgBox("Adit App EmailId", "Please enter valid Adit User Email Id");
                //    txtAdminUserName.Focus();
                //    return;
                //}

                if (string.IsNullOrEmpty(txtAdminPassword.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("Adit App Password", "Please enter valid location Admin Password");
                    txtAdminPassword.Focus();
                    return;
                }

                if (RBtnSingleClinic.Visible || RBtnMultiClinic.Visible)
                {
                    if (!RBtnSingleClinic.Checked && !RBtnMultiClinic.Checked)
                    {
                        ObjGoalBase.ErrorMsgBox("Adit App Clinic", "Please Select Single Or Multi Clinic Option");
                        RBtnSingleClinic.Focus();
                        return;
                    }
                }

                string strAditLogin = SystemBAL.GetAdminUserLoginEmailIdPass();
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var clientLogin = new RestClient(strAditLogin);
                var requestLogin = new RestRequest(Method.POST);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                requestLogin.AddHeader("Postman-Token", "1d16df4c-48ba-4644-bc7a-9bcef2a86744");
                requestLogin.AddHeader("cache-control", "no-cache");
                AditLoginPostBO AditLoginPost = new AditLoginPostBO
                {
                    email = txtAdminUserName.Text.ToLower().Trim(),
                    password = txtAdminPassword.Text.Trim()
                };
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(AditLoginPost);
                requestLogin.AddHeader("cache-control", "no-cache");
                requestLogin.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                GoalBase.WriteToPaymentLogFromAll_Static("Request : " + strAditLogin.ToString());
                IRestResponse responseLogin = clientLogin.Execute(requestLogin);
                GoalBase.WriteToPaymentLogFromAll_Static("Response : " + responseLogin.Content.ToString());
                if (responseLogin.StatusCode.ToString().ToLower() == "ServiceUnavailable".ToString().ToLower())
                {
                    ObjGoalBase.ErrorMsgBox("Adit Server", "Service Unavailable.");
                    SaveEHRErroLog(errorMessage: "Adit Server" + "Service Unavailable." + responseLogin.ErrorMessage);
                    configErrors += "\n Service unavailable login failed.";
                    return;
                }

                if (responseLogin.ErrorMessage != null)
                {
                    ObjGoalBase.ErrorMsgBox("Adit Server", "Server is down. Please try again after a few minutes.");
                    SaveEHRErroLog(errorMessage: "Adit Server" + "Server is down. Please try again after a few minutes." + responseLogin.ErrorMessage);
                    configErrors += "\n Server is down login failed.";
                    return;
                }

                var AdminLoginDto = JsonConvert.DeserializeObject<AdminLoginDetailBO>(responseLogin.Content);
                if (AdminLoginDto == null)
                {
                    ObjGoalBase.ErrorMsgBox("Adit App Admin User Authentication", responseLogin.ErrorMessage.ToString());
                    SaveEHRErroLog(errorMessage: "Adit App Admin User Authentication" + responseLogin.ErrorMessage);
                    configErrors += "\n Adit App user authentication failed.";
                    return;
                }
                if (AdminLoginDto.status == "true")
                {
                    Utility.Adit_User_Email_Id = txtAdminUserName.Text.Trim();
                    Utility.Adit_User_Email_Password = txtAdminPassword.Text.Trim();
                    Utility.WebAdminUserToken = AdminLoginDto.Token;
                    // Utility.WebAdminUserToken = string.Empty;
                    Utility.User_ID = AdminLoginDto.data._id;
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("Adit App Admin User Authentication", AdminLoginDto.message.ToString());
                    SaveEHRErroLog(errorMessage: "Adit App Admin User Authentication" + AdminLoginDto.message.ToString());
                    configErrors += "\n Adit App user authentication failed.";
                    return;
                }


                UserAditLocationLinkList = string.Empty;
                string strApiLocOrg = SystemBAL.GetApiAditLocationAndOrganizationByAdminIdPassword();
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var client = new RestClient(strApiLocOrg);
                var request = new RestRequest(Method.POST);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("Postman-Token", "1d16df4c-48ba-4644-bc7a-9bcef2a86744");
                request.AddHeader("cache-control", "no-cache");
                AditLoginPostBO AditLocOrgPost = new AditLoginPostBO
                {
                    email = txtAdminUserName.Text.Trim(),
                    password = txtAdminPassword.Text.Trim(),
                    created_by = Utility.User_ID
                };
                var javaScriptSerializer1 = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString1 = javaScriptSerializer1.Serialize(AditLocOrgPost);
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("application/json", jsonString1, ParameterType.RequestBody);
                //GoalBase.WriteToPaymentLogFromAll_Static("Request : " + strApiLocOrg.ToString());
                IRestResponse response = client.Execute(request);
                //GoalBase.WriteToPaymentLogFromAll_Static("Response : " + response.Content.ToString());
                if (response.ErrorMessage != null)
                {
                    ObjGoalBase.ErrorMsgBox("Adit App Admin User Authentication", response.ErrorMessage);
                    SaveEHRErroLog(errorMessage: "Adit App Admin User Authentication" + response.ErrorMessage);
                    configErrors += "\n Adit App getting details of location & organization failed.";
                    return;
                }

                UserAditLocationLinkList = response.Content;
                var customerDto = JsonConvert.DeserializeObject<LocationDetailBO>(response.Content);

                dtTempApptLocationTable.Clear();
                DataRow drApptLocDef = dtTempApptLocationTable.NewRow();
                drApptLocDef["id"] = "0";
                drApptLocDef["name"] = " Select ";
                dtTempApptLocationTable.Rows.Add(drApptLocDef);
                dtTempApptLocationTable.AcceptChanges();

                for (int i = 0; i < customerDto.data.Count; i++)
                {
                    DataRow drApptLoc = dtTempApptLocationTable.NewRow();
                    drApptLoc["id"] = customerDto.data[i]._id.ToString();
                    drApptLoc["name"] = customerDto.data[i].name.ToString().Trim();
                    drApptLoc["system_mac_address"] = customerDto.data[i].system_mac_address.ToString();
                    dtTempApptLocationTable.Rows.Add(drApptLoc);
                    dtTempApptLocationTable.AcceptChanges();
                }

                DataView dv = dtTempApptLocationTable.DefaultView;
                dv.Sort = "name";
                dtTempApptLocationTable = dv.ToTable();

                cmbAditLocation.DataSource = dtTempApptLocationTable.Copy();
                cmbAditLocation.ValueMember = "id";
                cmbAditLocation.DisplayMember = "name";
                cmbAditLocation.SelectedValue = "0";
                cmbAditLocation.DropDownStyle = ComboBoxStyle.DropDownList;
                Cursor.Current = Cursors.Default;
                if (cmbEHRName.Items.Count == 2)
                {
                    cmbEHRName.SelectedIndex = 1;
                    SetEHRConfigurations();
                }
                else
                {
                    SetPanelVisiblityEHRwise("Main", false);
                }
                SaveEHRErroLog();
                #endregion
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Admin User Authentication", "btnAdminUserSave_click " + ex.Message);
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n Adit user authentication issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region Location

        private void btnLocationSave_Click(object sender, EventArgs e)
        {
            List<string> locationLst = new List<string>();
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                timer1.Start();
                string StrFirstLocationValue = string.Empty;
                string StrFirstLocationName = string.Empty;
                string StrFirstLocationClinicNumber = string.Empty;

                if (RBtnMultiClinic.Checked)
                {
                    bool BlnChk = false;
                    for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                    {
                        if ((DGVMuliClinc.Rows[i].Cells["Location"] as DataGridViewComboBoxCell).Value != null)
                        {
                            string SelectedValue = Convert.ToString((DGVMuliClinc.Rows[i].Cells["Location"] as DataGridViewComboBoxCell).Value.ToString());
                            if (SelectedValue != "0")
                            {
                                if (string.IsNullOrEmpty(StrFirstLocationValue))
                                {
                                    StrFirstLocationValue = SelectedValue;
                                    StrFirstLocationName = DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString();
                                    StrFirstLocationClinicNumber = DGVMuliClinc.Rows[i].Cells["Clinic_Number"].Value.ToString();
                                }
                                BlnChk = true;
                                locationLst.Add(DGVMuliClinc.Rows[i].Cells["Description"].Value.ToString() + "-" + DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString());
                                bool IsExist = DGVMuliClinc.Rows.Cast<DataGridViewRow>()
                                          .Count(c =>
                                          c.Cells["Location"]
                                          .EditedFormattedValue.ToString() == DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString()) > 1;

                                if (IsExist)
                                {
                                    ObjGoalBase.ErrorMsgBox("Adit Location Name", "Please Select One Location For One Clinic.");
                                    DGVMuliClinc.Rows[i].Cells["Location"].Selected = true;
                                    return;
                                }


                                string Pid = string.Empty;
                                DataRow[] PozLocrow = dtTempApptLocationTable.Copy().Select("id = '" + SelectedValue.Trim() + "' ");
                                if (PozLocrow.Length > 0)
                                {
                                    Pid = PozLocrow[0]["system_mac_address"].ToString().Trim();
                                }

                                if (Pid.ToString().Trim() != string.Empty && Pid.ToString().Trim() != "0")
                                {
                                    if (processorID.ToString() != Pid.ToString().Trim())
                                    {
                                        ObjGoalBase.ErrorMsgBox("Location Name", DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString() + " Location is already configured with another system.");
                                        return;
                                    }
                                }

                            }
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Adit Location Name", "Please Select Adit Location You Would Like To Sync");
                            DGVMuliClinc.Rows[i].Cells["Location"].Selected = true;
                            return;
                        }
                    }

                    if (!BlnChk)
                    {
                        ObjGoalBase.ErrorMsgBox("Adit Location Name", "Please Select Atlist One Adit Location You Would Like To Sync");
                        return;
                    }
                }
                else
                {
                    if (cmbAditLocation.SelectedIndex == 0)
                    {
                        ObjGoalBase.ErrorMsgBox("Adit Location Name", "Please Select Adit Location You Would Like To Sync");
                        cmbAditLocation.Focus();
                        return;
                    }

                    string Pid = string.Empty;

                    DataRow[] PozLocrow = dtTempApptLocationTable.Copy().Select("id = '" + cmbAditLocation.SelectedValue.ToString().Trim() + "' ");
                    if (PozLocrow.Length > 0)
                    {
                        Pid = PozLocrow[0]["system_mac_address"].ToString().Trim();
                        //processorID = Pid; changed for Urbach;
                    }
                    locationLst.Add(cmbAditLocation.Text.ToString());
                    if (Pid.ToString().Trim() != string.Empty && Pid.ToString().Trim() != "0")
                    {
                        if (processorID.ToString() != Pid.ToString().Trim())
                        {
                            ObjGoalBase.ErrorMsgBox("Location Name", cmbAditLocation.Text.ToString() + " Location is already configured with another system.");
                            return;
                        }
                    }
                }

                var customerDto = JsonConvert.DeserializeObject<LocationDetailBO>(UserAditLocationLinkList);

                string tmpTimeZone = "";
                for (int i = 0; i < customerDto.data.Count; i++)
                {
                    if (customerDto.data[i]._id.ToString() == (RBtnMultiClinic.Checked == true ? StrFirstLocationValue : cmbAditLocation.SelectedValue.ToString()))
                    {
                        try
                        {
                            TimeZoneInfo timeZoneInfo;
                            DateTime dateTime;

                            tmpTimeZone = Utility.ConvertWebTimeZoneToSystemTimeZone(customerDto.data[i].timezone.name.ToString());

                            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tmpTimeZone);
                            dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                            // Utility.LocationTimeZone = tmpTimeZone;
                            Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();

                            if (TimeZone.CurrentTimeZone.StandardName.ToString().ToLower() != tmpTimeZone.ToString().ToLower())
                            {

                                //ObjGoalBase.InformationMsgBox("Timezone Mismatch", "Timezone of Server computer and set in Adit App Location settings should be same.");
                                //System.Environment.Exit(1);
                                //return;

                                string CurSystemTimeZoneWebForm = Utility.ConvertSystemTimeZoneToWebTimeZone(TimeZone.CurrentTimeZone.StandardName.ToString().ToLower());

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
                                    location = customerDto.data[0]._id.ToString()
                                };
                                var javaScriptSerializerUpdateWebTimeZone = new System.Web.Script.Serialization.JavaScriptSerializer();
                                string jsonStringUpdateWebTimeZone = javaScriptSerializerUpdateWebTimeZone.Serialize(UpdateWebTimeZoneBO);
                                requestUpdateWebTimeZone.AddHeader("postman-token", "a51e59f2-8946-424d-e1e0-988e059940d2");
                                requestUpdateWebTimeZone.AddHeader("cache-control", "no-cache");
                                requestUpdateWebTimeZone.AddHeader("content-type", "application/json");
                                requestUpdateWebTimeZone.AddParameter("application/json", jsonStringUpdateWebTimeZone, ParameterType.RequestBody);
                                IRestResponse responseUpdateWebTimeZone = clientUpdateWebTimeZone.Execute(requestUpdateWebTimeZone);
                                Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();

                                if (responseUpdateWebTimeZone.ErrorMessage != null)
                                {
                                    ObjGoalBase.ErrorMsgBox("Update Timezone", responseUpdateWebTimeZone.ErrorMessage);
                                    SaveEHRErroLog(errorMessage: "Update Timezone" + responseUpdateWebTimeZone.ErrorMessage);
                                    configErrors += "\n Timezone issue.";
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.ErrorMsgBox("", "btnLocationSave_click " + ex.Message);
                            SaveEHRErroLog(errorMessage: ex.Message);
                            configErrors += "\n Update timezone issue.";
                            Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();
                            tmpTimeZone = Utility.LocationTimeZone;
                        }
                        dtTempOgrTable.Clear();
                        DataRow drOrg = dtTempOgrTable.NewRow();
                        drOrg["id"] = customerDto.data[i].organization._id.ToString();
                        drOrg["Organization_ID"] = customerDto.data[i].organization._id.ToString();
                        drOrg["Name"] = customerDto.data[i].organization.name.ToString();
                        drOrg["phone"] = customerDto.data[i].organization.phone.ToString();
                        drOrg["email"] = customerDto.data[i].organization.email.ToString();
                        drOrg["address"] = string.Empty;
                        drOrg["Adit_User_Email_ID"] = Utility.Adit_User_Email_Id;
                        drOrg["Adit_User_Email_Password"] = Utility.Adit_User_Email_Password;
                        dtTempOgrTable.Rows.Add(drOrg);

                        if (RBtnMultiClinic.Checked == false)
                        {
                            dtTempLocationTable.Clear();
                            DataRow drLoc = dtTempLocationTable.NewRow();
                            drLoc["id"] = customerDto.data[0]._id.ToString();
                            drLoc["name"] = customerDto.data[i].name.ToString();
                            drLoc["phone"] = customerDto.data[i].phone.ToString();
                            drLoc["email"] = customerDto.data[i].email.ToString();
                            drLoc["address"] = string.Empty;
                            drLoc["timezone"] = tmpTimeZone;
                            drLoc["website_url"] = string.Empty;
                            drLoc["User_ID"] = Utility.User_ID;
                            drLoc["Loc_ID"] = customerDto.data[i].Location._id.ToString();
                            drLoc["Clinic_Number"] = "0";
                            drLoc["Service_Install_Id"] = "1";
                            dtTempLocationTable.Rows.Add(drLoc);
                        }
                    }
                }

                if (RBtnMultiClinic.Checked == true)
                {
                    for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                    {
                        var resultProvider = customerDto.data.AsEnumerable().Where(o => DGVMuliClinc.Rows[i].Cells["Location"].Value.ToString().ToUpper() == o._id.ToString().ToUpper());

                        if (resultProvider.Count() > 0)
                        {
                            List<Pozative.BO.MainData> o = resultProvider.ToList();

                            DataRow drLoc = dtTempLocationTable.NewRow();
                            drLoc["id"] = o[0]._id.ToString();
                            drLoc["name"] = o[0].name.ToString();
                            drLoc["phone"] = o[0].phone.ToString();
                            drLoc["email"] = o[0].email.ToString();
                            drLoc["address"] = string.Empty;
                            drLoc["timezone"] = tmpTimeZone;
                            drLoc["website_url"] = string.Empty;
                            drLoc["User_ID"] = Utility.User_ID;
                            drLoc["Loc_ID"] = o[0].Location._id.ToString();
                            drLoc["Clinic_Number"] = DGVMuliClinc.Rows[i].Cells["Clinic_Number"].Value.ToString();
                            drLoc["Service_Install_Id"] = "1";
                            dtTempLocationTable.Rows.Add(drLoc);
                        }
                    }
                }

                /////////////////////////////////////////////////                
                if (dtTempLocationTable != null && dtTempLocationTable.Rows.Count > 0)
                {
                    Utility.Location_ID = dtTempLocationTable.Rows[0]["Id"].ToString();
                }
                if (dtTempOgrTable != null && dtTempOgrTable.Rows.Count > 0)
                {
                    Utility.Organization_ID = dtTempOgrTable.Rows[0]["Id"].ToString();
                }

                string strApiEHRList = SystemBAL.GetApiERHListWithWebId();
                var client = new RestClient(strApiEHRList);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestRequest(Method.GET);
                //request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                //request.AddHeader("cache-control", "no-cache");

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);

                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    ObjGoalBase.WriteToErrorLogFile("[GetApiERHListWithWebId] : " + response.ErrorMessage);
                    SaveEHRErroLog(ehrConnected: true, errorMessage: "[GetApiERHListWithWebId] : " + response.ErrorMessage, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst);
                    configErrors += "\n Get EHR list issue.";
                    return;
                }

                var GetEHRListWithWebId = JsonConvert.DeserializeObject<EHRListWithWebIdBO>(response.Content);
                string tmpEHRMaster = string.Empty;
                foreach (var item in GetEHRListWithWebId.data)
                {
                    if (item.name.ToString().ToLower() == cmbEHRName.Text.ToString().ToLower() && item.version.ToString().ToLower() == cmbEHRVersion.Text.ToString().ToLower())
                    {
                        tmpEHRMaster = item._id.ToString();
                    }
                }

                if (tmpEHRMaster == string.Empty)
                {
                    if (cmbEHRName.Text.ToUpper() == "SOFTDENT")
                    {
                        ObjGoalBase.ErrorMsgBox("EHR Configuration", cmbEHRName.Text + " With " + cmbEHRVersion.Text + " is under development");
                    }
                    else
                    {
                        ObjGoalBase.ErrorMsgBox("EHR Configuration", cmbEHRName.Text + " With " + cmbEHRVersion.Text + " cannot configure with Adit app.");
                    }
                    return;
                }
                for (int i = 0; i < dtTempLocationTable.Rows.Count; i++)
                {

                    LocationEHRBO LEHR = new LocationEHRBO
                    {
                        application_name = cmbEHRName.Text.Trim(),
                        application_version = cmbEHRVersion.Text.Trim(),
                        system_name = System_Name,
                        system_mac_address = processorID,
                        is_install_ehr = true,
                        install_date = Utility.Datetimesetting().ToString("yyyy-MM-ddT00:00:00"),
                        ehrmaster = tmpEHRMaster,
                        created_by = Utility.User_ID
                    };
                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(LEHR);

                    string staLocation_EHR = PushLiveDatabaseBAL.Push_Location_EHR(jsonString, (RBtnMultiClinic.Checked == true ? dtTempLocationTable.Rows[i]["id"].ToString() : cmbAditLocation.SelectedValue.ToString()));

                    if (staLocation_EHR == string.Empty)
                    {
                        SaveEHRErroLog(ehrConnected: true, errorMessage: "EHR Location " + staLocation_EHR, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst);
                        configErrors += "\n EHR Location issue.";
                        ObjGoalBase.WriteToErrorLogFile("EHR Location " + staLocation_EHR);
                        return;
                    }
                    else
                    {
                        SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                        ObjGoalBase.WriteToSyncLogFile(cmbEHRName.Text.Trim() + " configuration save successfully.");
                        ObjGoalBase.WriteToSyncLogFile("EHR Location save Successfully.");
                    }
                }

                bool staOrganization = SystemBAL.Save_OrganizationDetail(dtTempOgrTable.Rows[0]["id"].ToString(),
                                                                   dtTempOgrTable.Rows[0]["Name"].ToString(),
                                                                   dtTempOgrTable.Rows[0]["phone"].ToString(),
                                                                   dtTempOgrTable.Rows[0]["email"].ToString(),
                                                                   dtTempOgrTable.Rows[0]["address"].ToString(),
                                                                   "",
                                                                   dtTempOgrTable.Rows[0]["Name"].ToString(),
                                                                   "1",
                                                                   dtTempOgrTable.Rows[0]["email"].ToString(),
                                                                   dtTempOgrTable.Rows[0]["Adit_User_Email_ID"].ToString(),
                                                                   dtTempOgrTable.Rows[0]["Adit_User_Email_Password"].ToString(),
                                                                   "Insert");

                if (staOrganization)
                {
                    Utility.Organization_ID = dtTempOgrTable.Rows[0]["id"].ToString();
                    Utility.Organization_Name = dtTempOgrTable.Rows[0]["Name"].ToString();
                    ObjGoalBase.WriteToSyncLogFile("Organization configuration save successfully.");
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst, multiDatabaseConfiure: false, successfullyConfigured: true);

                }

                bool staLocation = false;

                if (RBtnMultiClinic.Checked == false)
                {
                    staLocation = SystemBAL.Save_LocationDetail((RBtnMultiClinic.Checked == true ? StrFirstLocationValue : cmbAditLocation.SelectedValue.ToString()),
                                                                (RBtnMultiClinic.Checked == true ? StrFirstLocationName : cmbAditLocation.Text.ToString()),
                                                                         dtTempLocationTable.Rows[0]["website_url"].ToString(),
                                                                         dtTempLocationTable.Rows[0]["phone"].ToString(),
                                                                         dtTempLocationTable.Rows[0]["email"].ToString(),
                                                                         dtTempLocationTable.Rows[0]["address"].ToString(),
                                                                         Utility.HostName_Adit.Replace("api", "app"),
                                                                         "EN",
                                                                         dtTempLocationTable.Rows[0]["name"].ToString(),
                                                                         "1",
                                                                         dtTempOgrTable.Rows[0]["id"].ToString(),
                                                                         Utility.User_ID,
                                                                         dtTempLocationTable.Rows[0]["Loc_ID"].ToString(),
                                                                         "Insert",
                                                                         dtTempLocationTable.Rows[0]["Clinic_Number"].ToString(),
                                                                         dtTempLocationTable.Rows[0]["Service_Install_Id"].ToString());
                }
                else if (RBtnMultiClinic.Checked)
                {
                    for (int i = 0; i < dtTempLocationTable.Rows.Count; i++)
                    {
                        staLocation = SystemBAL.Save_LocationDetail(dtTempLocationTable.Rows[i]["id"].ToString(),
                                                                 dtTempLocationTable.Rows[i]["name"].ToString(),
                                                                 dtTempLocationTable.Rows[i]["website_url"].ToString(),
                                                                 dtTempLocationTable.Rows[i]["phone"].ToString(),
                                                                 dtTempLocationTable.Rows[i]["email"].ToString(),
                                                                 dtTempLocationTable.Rows[i]["address"].ToString(),
                                                                 Utility.HostName_Adit.Replace("api", "app"),
                                                                 "EN",
                                                                 dtTempLocationTable.Rows[i]["name"].ToString(),
                                                                 "1",
                                                                 dtTempOgrTable.Rows[0]["id"].ToString(),
                                                                 Utility.User_ID,
                                                                 dtTempLocationTable.Rows[i]["Loc_ID"].ToString(),
                                                                 "Insert",
                                                                 dtTempLocationTable.Rows[i]["Clinic_Number"].ToString(),
                                                                 dtTempLocationTable.Rows[i]["Service_Install_Id"].ToString());
                    }
                }

                if (staLocation)
                {
                    //Utility.LocationTimeZone = dtTempLocationTable.Rows[0]["timezone"].ToString();
                    Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();
                    Utility.Location_ID = (RBtnMultiClinic.Checked == true ? StrFirstLocationValue : cmbAditLocation.SelectedValue.ToString());
                    Utility.Location_Name = (RBtnMultiClinic.Checked == true ? StrFirstLocationName : cmbAditLocation.Text.ToString());
                    Utility.Loc_ID = dtTempLocationTable.Rows[0]["Loc_ID"].ToString();
                    ObjGoalBase.WriteToSyncLogFile("Location configuration save successfully.");
                    SaveEHRErroLog(ehrConnected: true,locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                }

                if (Utility.LocationTimeZone == string.Empty)
                {
                    Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();
                }

                string PozatieLocationid = string.Empty;
                string PozatieLocationName = string.Empty;


                Utility.ApplicationInstalledTime = Utility.Datetimesetting();

                bool staInstallApp = SystemBAL.Save_InstallApplicationDetail(
                                                                Utility.Organization_ID,
                                                                Utility.Location_ID,
                                                                cmbEHRName.Text.Trim(),
                                                                cmbEHRVersion.Text.Trim(),
                                                                System_Name,
                                                                processorID,
                                                                Utility.EHRHostname,
                                                                Utility.EHRIntegrationKey,
                                                                Utility.EHRUserId,
                                                                Utility.EHRPassword,
                                                                Utility.EHRDatabase,
                                                                Utility.EHRPort,
                                                                Utility.DBConnString,
                                                                Utility.WebAdminUserToken,
                                                                Utility.LocationTimeZone,
                                                                true,
                                                                Utility.PozativeSync,
                                                                txtPozativeEmailID.Text.Trim(),
                                                                PozatieLocationid,
                                                                PozatieLocationName,
                                                                IsAction,
                                                                Utility.EHRDocPath,
                                                                "1",
                                                                Utility.DontAskPasswordOnSaveSetting,
                    Utility.NotAllowToChangeSystemDateFormat);


                Utility.Application_Name = cmbEHRName.Text.Trim();
                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                try
                {
                    CommonUtility.GetSystemDetails();
                    //SystemBAL.ApplicationUpdateServerDate();
                    string AppVersionAutoUpdateAditserverData = PushLiveDatabaseBAL.Push_Location_EHRUPdateForVersion();

                    ObjGoalBase.WriteToSyncLogFile("[AppVersion AutoUpdate ServerData] has completed successfully.");
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                }
                catch (Exception EX)
                {
                    ObjGoalBase.WriteToErrorLogFile("btnLocationSave_Click AppVersion AutoUpdate ServerData : " + EX.Message);
                    SaveEHRErroLog(ehrConnected: true, errorMessage: "AppVersion AutoUpdate ServerData : " + EX.Message, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                }

                if (staInstallApp)
                {
                    Utility.Application_Name = cmbEHRName.Text.Trim();
                    Utility.Application_Version = cmbEHRVersion.Text.Trim();
                    Utility.Application_ID = Convert.ToInt32(cmbEHRName.SelectedValue.ToString());
                    Utility.PozativeEmail = txtPozativeEmailID.Text.Trim();
                    Utility.PozativeLocationID = PozatieLocationid;
                    Utility.PozativeLocationName = PozatieLocationName;
                    ObjGoalBase.WriteToSyncLogFile("Application installation has been completed successfully.");
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                }

                if (Utility.AditSync == false && Utility.PozativeSync == true)
                {

                    //ObjGoalBase.SuccessMsgBox("Pozative Service Configuration", "Application installation has completed successfully with [" + PozatieLocationName + "] Location.");
                }
                else
                {
                    pnlLocationMain.Visible = false;
                    isProcessComplete = true;

                    Cursor.Current = Cursors.Default;
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                    SetPanelVisiblityEHRwise("ReportGenerated");
                    Application.DoEvents();
                    Thread.Sleep(5000);
                    Application.DoEvents();
                    // ObjGoalBase.SuccessMsgBox("Adit Configuration", "Application installation has completed successfully. ");
                    //StartExeRun();
                    //System.Environment.Exit(1);
                    //if (Utility.Application_ID == 4)
                    //{
                    //    System.Environment.Exit(1);
                    //}
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception EX)
            {
                ObjGoalBase.ErrorMsgBox("EHR Location", "btnLocationSave_Click " + EX.Message);
                SaveEHRErroLog(ehrConnected: true, errorMessage: "EHR Location" + EX.Message, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationLst, multiDatabaseConfiure: false, successfullyConfigured: false);
                configErrors += "\n EHR Location set issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                timer1.Stop();
                isProcessComplete = true;
            }
        }

        private void btnLocationBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //ViewAdminConfigurationPanel();
                if (cmbEHRName.Items.Count == 2)
                {
                    cmbEHRName.SelectedIndex = 1;
                    SetPanelVisiblityEHRwise("AdminUser", false);
                }
                else
                {
                    SetPanelVisiblityEHRwise("Main", false);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "btnLocationBack_click " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region SoftDent Configuration

        public void StartExeRun()
        {
            try
            {
                bool IsExeRun = false;
                string fileName = string.Empty;
                foreach (Process p in Process.GetProcesses())
                {
                    fileName = p.ProcessName;

                    if (string.Compare("ExeRun".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                    {
                        IsExeRun = true;
                    }
                }
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
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "StartExeRun " + ex.Message);
            }
        }
        public bool OpenConnection()
        {
            try
            {
                //string serverExeDir = SoftDentRegistryKey.ServerExeDir;
                //string faircomServerName = "";//SoftDentRegistryKey.FaircomServerName;
                //ErrorCode errorCode = ErrorCode.Failure;
                //bool b = InteropFactory.SoftDent.Open(serverExeDir, faircomServerName, out errorCode);
                return false;
            }
            catch (Exception)
            {

                throw;
            }
            //return RetrieveData();
        }

        private void SaveSoftDentConfigurtion()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Utility.Application_Version = cmbEHRVersion.Text.Trim();
                if (cmbEHRVersion.Text.ToLower() == "17.0.0+")
                {

                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRPort = string.Empty;

                    bool IsEHRConnected = OpenConnection();

                    if (IsEHRConnected)
                    {
                        //ViewAdminConfigurationPanel();
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;

                        ObjGoalBase.ErrorMsgBox("Authentication", "SoftDent is not connecting. " + "\n" + " Please try again Later or Run Practicework Server exe.");
                    }
                    PostEHRConnectionLog("SoftDent", IsEHRConnected);
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "SaveSoftDentConfigurtion " + ex.Message);
                SaveEHRErroLog(errorMessage: "SoftDent " + ex.Message);
                configErrors += "\n SoftDent connection issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region ClearDent Configuration
        public DataTable ListLocalSqlInstances(RegistryKey hive)
        {
            const string keyName = @"Software\Microsoft\Microsoft SQL Server";
            const string valueName = "InstalledInstances";
            const string defaultName = "MSSQLSERVER";
            if (!dtSqlServerName.Columns.Contains("SqlServerName"))
            {
                dtSqlServerName.Columns.Add("SqlServerName");
            }


            using (var key = hive.OpenSubKey(keyName, false))
            {
                if (key == null) //return Enumerable.Empty<string>();
                {
                    return null;
                }

                var value = key.GetValue(valueName) as string[];
                if (value == null) //return Enumerable.Empty<string>();
                {
                    return null;
                }

                for (int index = 0; index < value.Length; index++)
                {
                    if (string.Equals(value[index], defaultName, StringComparison.OrdinalIgnoreCase))
                    {
                        value[index] = ".";
                        DataRow drNew = dtSqlServerName.NewRow();
                        drNew["SqlServerName"] = ".";
                        dtSqlServerName.Rows.Add(drNew);
                    }
                    else
                    {
                        DataRow drNew = dtSqlServerName.NewRow();
                        drNew["SqlServerName"] = @".\" + value[index];
                        dtSqlServerName.Rows.Add(drNew);
                        value[index] = @".\" + value[index];
                    }
                }

                return dtSqlServerName;
            }
        }

        private void GetSqlServerNameANDServiceName()
        {
            try
            {
                dtSqlServerName.Rows.Clear();
                if (Environment.Is64BitOperatingSystem)
                {
                    using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    {
                        DataTable dtResult = ListLocalSqlInstances(hive);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            dtSqlServerName.Load(dtResult.CreateDataReader());
                        }
                    }

                    using (var hive = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                    {
                        DataTable dtResult = ListLocalSqlInstances(hive);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            dtSqlServerName.Load(dtResult.CreateDataReader());
                        }
                    }
                }
                else
                {
                    DataTable dtResult = ListLocalSqlInstances(Registry.LocalMachine);
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        dtSqlServerName.Load(dtResult.CreateDataReader());
                    }
                }

                #region Find Sql Service Name
                SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
                System.Data.DataTable table = instance.GetDataSources();
                cmbSqlServiceName.DataSource = table;
                cmbSqlServiceName.DisplayMember = "InstanceName";
                #endregion
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToSyncLogFile("Error_Find_CreateSqlDatabase " + "GetSqlServerNameANDServiceName " + ex.Message.ToString());
                //throw;
            }
        }
        private void btnClearDentSave_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;

                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (string.IsNullOrEmpty(txtClearDentHostName.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("ClearDent Hostname", "Please Enter valid ClearDent Hostname");
                    txtClearDentHostName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtClearDentUserId.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("ClearDent Userid", "Please Enter valid ClearDent User Id");
                    txtClearDentUserId.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txt_PatientDocPath.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("ClearDent DocumentPath", "Please Enter valid ClearDent Patient Document Path");
                    txt_PatientDocPath.Focus();
                    return;
                }


                #region commented code
                //Utility.EHRHostname = txtClearDentHostName.Text.ToLower().Trim();
                //Utility.EHRIntegrationKey = string.Empty;
                //Utility.EHRDatabase = "";
                //Utility.EHRUserId = txtClearDentUserId.Text.Trim();
                //Utility.EHRPassword = txtClearDentPassowrd.Text.Trim();
                //Utility.EHRDocPath = txt_PatientDocPath.Text.Trim().TrimEnd('\\');
                //Utility.EHRPort = "3306";
                //Utility.DontAskPasswordOnSaveSetting = false;
                //Utility.NotAllowToChangeSystemDateFormat = false;
                //if (txtClearDentPassowrd.Text.Trim() == "")
                //{
                //    ConnectionString = "Initial Catalog=ClearDent;Data Source=" + Utility.EHRHostname + ";User ID=" + Utility.EHRUserId + ";";
                //}
                //else
                //{
                //    ConnectionString = "Initial Catalog=ClearDent;Data Source=" + Utility.EHRHostname + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                //} 
                #endregion
                string ClearDentPath = "";
                string appKey = @"SOFTWARE\Prococious Technology Inc.\ClearDent";
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                {
                    if (registryKey != null)
                    {
                        ClearDentPath = registryKey.GetValue("ApplicationPath").ToString();
                    }

                }
                if (ClearDentPath != "")
                {
                    string configFilePath = ClearDentPath + "\\ClearDent.exe.config"; // replace with the path to your external config file
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = configFilePath;
                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    Utility.DBConnString = config.AppSettings.Settings["ClearDent.SQLConnection"].Value;

                }
                bool IsEHRConnected = SystemBAL.GetEHRClearDentConnection();

                if (IsEHRConnected)
                {
                    //ConnectionString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                    //btnOpenDentalSave_Click(null, null);
                    //ViewAdminConfigurationPanel();
                    Cursor.Current = Cursors.Default;
                    SetPanelVisiblityEHRwise("Location", false);
                }
                else
                {
                    Utility.DBConnString = "";
                    Utility.APISessionToken = string.Empty;
                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = string.Empty;

                    ObjGoalBase.ErrorMsgBox("Authentication", "ClearDent is not connecting. " + "\n" + " Please enter valid credentials.");
                    configErrors += "\n ClearDent not connected.";
                }
                PostEHRConnectionLog("ClearDent", IsEHRConnected);
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "btnClearDentSave_Click " + ex.Message);
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n ClearDent authentication issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ClearDentConfigurtion()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Utility.Application_Version = cmbEHRVersion.Text.Trim();
                if (cmbEHRVersion.Text.ToLower() == "9.8+" || cmbEHRVersion.Text.ToLower() == "9.10+")
                {

                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRPort = string.Empty;
                    Utility.DBConnString = ConfigurationManager.ConnectionStrings["CleardentConnectionString"].ConnectionString;
                    bool IsEHRConnected = SystemBAL.GetEHRClearDentConnection();
                    if (IsEHRConnected)
                    {
                        //ViewAdminConfigurationPanel();
                        SetPanelVisiblityEHRwise("AdminUser", false);
                    }
                    else
                    {
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;
                        ObjGoalBase.ErrorMsgBox("Authentication", "Cleardent is not connecting. " + "\n" + " Please enter valid credentials.");
                    }
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "ClearDentConfigurtion " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Tracker Integration

        private void btnTrackerSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (cmbEHRVersion.Text.ToLower() == "11.29")
                {
                    //if (string.IsNullOrEmpty(cmbSqlServiceName.Text.Trim()))
                    //{
                    //    ObjGoalBase.ErrorMsgBox("SQL Service", "Please Enter valid Tracker SQL Service Name");
                    //    cmbSqlServiceName.Focus();
                    //    return;
                    //}
                    //if (string.IsNullOrEmpty(cmbTrackerHostName.Text.Trim()))
                    //{
                    //    ObjGoalBase.ErrorMsgBox("ServerName", "Please Enter valid Tracker ServerName");
                    //    cmbTrackerHostName.Focus();
                    //    return;
                    //}
                    if (string.IsNullOrEmpty(txtTrackerCredential.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Credential", "Please Enter valid Tracker Database Credential");
                        txtTrackerCredential.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtTrackerDatabase.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Database", "Please Enter valid Tracker Database Name");
                        txtTrackerDatabase.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtTrackerUserId.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("UserId", "Please Enter valid Tracker User Id");
                        txtTrackerUserId.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtTrackerPassword.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Password", "Please Enter valid Tracker Password");
                        txtTrackerPassword.Focus();
                        return;
                    }

                    bool IsEHRConnected = false;

                    Utility.EHRUserId = txtTrackerUserId.Text;
                    Utility.EHRPassword = txtTrackerPassword.Text;
                    Utility.EHRDatabase = txtTrackerDatabase.Text;
                    Utility.EHRPort = string.Empty;
                    Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                    IsEHRConnected = CheckTrackerConnection();
                    if (IsEHRConnected)
                    {
                        //ConnectionString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                        //btnOpenDentalSave_Click(null, null);
                        //ViewAdminConfigurationPanel();
                        Cursor.Current = Cursors.Default;
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        Utility.DBConnString = "";
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;

                        SetPanelVisiblityEHRwise("Tracker");

                        ObjGoalBase.ErrorMsgBox("Authentication", "Tracker is not connecting. " + "\n" + " Please enter valid credentials.");
                        configErrors += "\n Tracker not connected.";
                    }
                    PostEHRConnectionLog("Tracker", IsEHRConnected);
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "btnTrackerSave_Click " + ex.Message);
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n Tracker authentication issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public bool CheckTrackerConnection()
        {
            try
            {
                //Data Source=DESKTOP-LT7125H\SQLEXPRESS;Initial Catalog=Tracker;User ID=TrackerAdmin;Password=***********
                Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                bool IsEHRConnected = SystemBAL.GetEHRTrackerConnection();
                return IsEHRConnected;
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("CheckTrackerConnection", "Tracker is not connecting. " + "\n" + " Please enter valid credentials." + "CheckTrakerConnection " + ex.Message.ToString());
                return false;
                throw;
            }
        }

        public void ExecuteBatchFile(string FileName)
        {
            var psi = new ProcessStartInfo();
            psi.CreateNoWindow = true; //This hides the dos-style black window that the command prompt usually shows
            psi.FileName = FileName;
            psi.Verb = "runas"; //This is what actually runs the command as administrator
            try
            {
                var process = new Process();
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo = psi;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "ExecuteBatchFile " + ex.Message);
                //If you are here the user clicked decline to grant admin privileges (or he's not administrator)
            }
        }

        public void CreateBatchFileForSqlService()
        {
            try
            {
                string FileName = "";
                FileName = Application.StartupPath + "\\SingleUser.bat";

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
                    SW.WriteLine(@"net stop MSSQL$" + cmbSqlServiceName.Text.ToString() + "");
                    SW.WriteLine(@"net start MSSQL$" + cmbSqlServiceName.Text.ToString() + " /m");
                    SW.Close();
                }

                // string FileName = "";
                FileName = Application.StartupPath + "\\MultiUser.bat";

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
                    SW.WriteLine(@"net stop MSSQL$" + cmbSqlServiceName.Text.ToString() + "");
                    SW.WriteLine(@"net start MSSQL$" + cmbSqlServiceName.Text.ToString() + "");
                    SW.Close();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "CreateBatchFileForSqlService " + ex.Message);
            }
        }

        public bool CreateCredentialANdUserForTracker()
        {
            string FileName = "";
            try
            {
                FileName = Application.StartupPath + "\\SingleUser.bat";

                CreateBatchFileForSqlService();

                ExecuteBatchFile(FileName);

                string strSql = "USE [master]; "
                                  //+ "   GO "

                                  + "  CREATE CREDENTIAL " + txtTrackerCredential.Text.ToString() + ""
                                  + "  WITH IDENTITY = '" + cmbTrackerHostName.Text.ToString() + "' , "
                                  + "      SECRET = '" + txtTrackerPassword.Text.ToString() + "'; "

                                  + "  CREATE LOGIN " + txtTrackerUserId.Text.ToString() + " "
                                  + "      WITH PASSWORD    = N'" + txtTrackerPassword.Text.ToString() + "', "
                                  + "      CHECK_POLICY     = OFF, "
                                  + "      CHECK_EXPIRATION = OFF; "
                                  //+ "  GO "
                                  + "  EXEC sp_addsrvrolemember  "
                                  + "      @loginame = N'" + txtTrackerUserId.Text.ToString() + "' , "
                                  + "      @rolename = N'sysadmin'; "
                                  //+ "  GO "
                                  + "  ALTER LOGIN " + txtTrackerUserId.Text.ToString() + " "
                                  + "  ADD CREDENTIAL " + txtTrackerCredential.Text.ToString() + " ";
                //+ "  Go";

                SqlConnection sqlcon = new SqlConnection("Data Source='" + cmbTrackerHostName.Text.ToString() + "';Integrated Security=True");
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(strSql, sqlcon);
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("User Created Successfully");

                FileName = Application.StartupPath + "\\MultiUser.bat";

                ExecuteBatchFile(FileName);

                return true;
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("CreateCredentialANdUserForTracker", "Tracker is not connecting. " + "\n" + " Please enter valid credentials." + "CreateCredentialANDUserForTracker " + ex.Message.ToString());
                FileName = Application.StartupPath + "\\MultiUser.bat";
                ExecuteBatchFile(FileName);
                throw;
            }
        }
        #endregion

        #region Abeldent Integration

        private void btnAbeldentSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (cmbEHRVersion.Text.ToLower() == "14.8.2")
                {
                    if (string.IsNullOrEmpty(cmbSqlServiceName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("SQL Service", "Please Enter valid AbelDent SQL Service Name");
                        cmbSqlServiceName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(cmbAbelDentHostName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("ServerName", "Please Enter valid AbelDent ServerName");
                        cmbAbelDentHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtAbelDentDatabase.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Database", "Please Enter valid AbelDent Database Name");
                        txtTrackerDatabase.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtAbelDentUserId.Text.Trim()))
                    {
                        txtAbelDentUserId.Text = string.Empty;
                    }
                    if (string.IsNullOrEmpty(txtAbelDentPassword.Text.Trim()))
                    {
                        txtAbelDentPassword.Text = string.Empty;
                    }

                    bool IsEHRConnected = false;
                    IsEHRConnected = CheckAbelDentConnection();
                    if (IsEHRConnected)
                    {
                        //ViewAdminConfigurationPanel();
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        CreateCredentialANdUserForTracker();
                        IsEHRConnected = CheckAbelDentConnection();
                        if (IsEHRConnected)
                        {
                            //ViewAdminConfigurationPanel();
                            SetPanelVisiblityEHRwise("Location", false);
                        }
                        else
                        {
                            Utility.DBConnString = "";
                            Utility.APISessionToken = string.Empty;
                            Utility.EHRHostname = string.Empty;
                            Utility.EHRIntegrationKey = string.Empty;
                            Utility.EHRUserId = string.Empty;
                            Utility.EHRPassword = string.Empty;
                            Utility.EHRDatabase = string.Empty;
                            Utility.EHRPort = string.Empty;

                            ObjGoalBase.ErrorMsgBox("Authentication", "Abeldent is not connecting. " + "\n" + " Please enter valid credentials.");
                            configErrors += "\n Abeldent not connected.";
                        }
                    }
                    PostEHRConnectionLog("AbelDent", IsEHRConnected);
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "btnAbelDentSave_Click " + ex.Message);
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n AbelDent authentication issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        public bool CheckAbelDentConnection()
        {
            try
            {
                Utility.EHRHostname = cmbAbelDentHostName.Text.ToLower().Trim();
                Utility.EHRIntegrationKey = string.Empty;
                Utility.EHRDatabase = txtAbelDentDatabase.Text.Trim();
                Utility.EHRUserId = txtAbelDentUserId.Text.Trim();
                Utility.EHRPassword = txtAbelDentPassword.Text.Trim();
                Utility.EHRPort = "";
                //Data Source=192.168.1.197\aditbeta;Initial Catalog=Abel;User id=sa;Password=Adit@123
                if (Utility.EHRUserId == string.Empty || Utility.EHRPassword == string.Empty)
                {
                    Utility.DBConnString = "Server=" + Utility.EHRHostname + ";Database=" + Utility.EHRDatabase + ";Trusted_Connection=True;";
                }
                else
                {
                    Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                }
                bool IsEHRConnected = SystemBAL.GetAbelDentConnection();
                return IsEHRConnected;
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("CheckAbelDentConnection", "AbelDent is not connecting. " + "\n" + " Please enter valid credentials." + "CheckAbelDentConnection " + ex.Message.ToString());
                return false;
                throw;
            }
        }
        #endregion

        #region PracticeWeb Configuration
        private void btnPracticeWebSave_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor.Current = Cursors.WaitCursor;

                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (cmbEHRVersion.Text.ToLower() == "21.1")
                {

                    if (string.IsNullOrEmpty(PracticeWeb_txthostName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Hostname", "Please Enter valid PracticeWeb Hostname");
                        txtOpenDentalHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtPracticeWebDatabase.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Port", "Please Enter valid PracticeWeb Database Name");
                        txtOpenDentalDatabase.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtPracticeWebUseID.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("IntegrationKey", "Please Enter valid PracticeWeb User Id");
                        txtOpenDentalUserId.Focus();
                        return;
                    }


                    Utility.EHRHostname = PracticeWeb_txthostName.Text.ToLower().Trim();
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRDatabase = txtPracticeWebDatabase.Text.Trim();
                    Utility.EHRUserId = txtPracticeWebUseID.Text.Trim();
                    Utility.EHRPassword = txtPracticeWebPassword.Text.Trim();
                    Utility.EHRDocPath = string.Empty;
                    Utility.EHRPort = "3306";
                    Utility.DontAskPasswordOnSaveSetting = false;
                    Utility.NotAllowToChangeSystemDateFormat = false;

                    Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";
                    bool IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);

                    if (IsEHRConnected)
                    {
                        //ViewAdminConfigurationPanel();
                        Cursor.Current = Cursors.Default;

                        dtTempOpenDentalClinicTable = SynchOpenDentalBAL.GetOpenDentalClinicData(Utility.DBConnString);

                        if (dtTempOpenDentalClinicTable.Rows.Count > 1)
                        {
                            RBtnSingleClinic.Visible = true;
                            RBtnMultiClinic.Visible = true;
                        }
                        else
                        {
                            RBtnSingleClinic.Checked = true;
                            RBtnSingleClinic.Visible = false;
                            RBtnMultiClinic.Visible = false;
                        }
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        Utility.DBConnString = "";
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;

                        ObjGoalBase.ErrorMsgBox("Authentication", "PracticeWeb is not connecting. " + "\n" + " Please enter valid credentials.");
                        configErrors += "\n PracticeWeb not connected.";
                    }
                    PostEHRConnectionLog("PracticeWeb", IsEHRConnected);
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "btnPracticeWebSave_Click " + ex.Message);
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n PracticeWeb authentication issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #endregion

        #region Common Event

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
                ObjGoalBase.ErrorMsgBox("Pozative Service", "tblHead_MouseDown " + ex.Message);
            }
        }

        private void lblHead_MouseDown(object sender, MouseEventArgs e)
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
                ObjGoalBase.ErrorMsgBox("Pozative Service", "lblHead_MouseDown " + ex.Message);
            }
        }

        #endregion

        #region Combobox & Textbox Event

        private void cmbEHRName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbEHRVersion.Enabled = true;
                if (cmbEHRName.SelectedIndex == 0)
                {
                    cmbEHRVersion.Enabled = false;
                    cmbEHRVersion.Text = "<Select>";
                }

                else
                {
                    switch (Convert.ToInt16(cmbEHRName.SelectedValue))
                    {
                        case 1://Eaglesoft
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 2://OpenDental
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 3://Dentrix
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 4://SoftDent
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 5://ClearDent
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 6://Tracker
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 7://PracticeWork
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 8://EasyDental
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 9://OpenDental
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 10://PracticeWeb
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 11://AbelDent
                            GetVersion(cmbEHRName.Text.ToString());
                            break;

                    }

                    //cmbEHRVersion.ValueMember = "Version_ID";
                    //cmbEHRVersion.DisplayMember = "Version_Name";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("EHR Selected Index Changed", "cmbEHRName_SelectedIndexChanged " + ex.Message);
            }
        }

        private void txtAdminPassword_Leave(object sender, EventArgs e)
        {
            try
            {
                btnAdminUserSave.Focus();
                btnAdminUserSave.Select();
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbAditLocation_Leave(object sender, EventArgs e)
        {
            try
            {
                btnLocationSave.Focus();
            }
            catch (Exception)
            {
            }
        }

        private void cmbPozativeLocation_Leave(object sender, EventArgs e)
        {
            try
            {
                btnLocationSave.Focus();
            }
            catch (Exception)
            {
            }

        }


        private void lblEaglesoftHostname_Click(object sender, EventArgs e)
        {
            Process myProcess = new Process();

            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = "C:\\EagleSoft\\Shared Files\\techaid.exe";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();

            }
            catch (Exception e1)
            {
                ObjGoalBase.WriteToErrorLogFile("Error to open Technical Reference " + e1.Message);

            }
        }

        private void btnPracticeWorkSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (cmbEHRVersion.Text.ToLower() == "7.9+".ToLower())
                {
                    if (string.IsNullOrEmpty(txtPracticeworkHost.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Hostname", "Please Enter valid PracticeWork Hostname");
                        txtPracticeworkHost.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtPracticeworkDbName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Database", "Please Enter valid PracticeWork Database");
                        txtPracticeworkDbName.Focus();
                        return;
                    }
                    Utility.EHRHostname = txtPracticeworkHost.Text.ToLower().Trim();
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = txtPracticeworkUserId.Text.Trim();
                    Utility.EHRPassword = txtPracticeworkPassword.Text.Trim();
                    Utility.EHRDatabase = txtPracticeworkDbName.Text.Trim();
                    Utility.EHRPort = string.Empty;
                    Utility.DBConnString = "Driver={Pervasive ODBC Client Interface};ServerName=" + Utility.EHRHostname + ";dbq=" + Utility.EHRDatabase + ";UID=" + Utility.EHRUserId + ";PWD=" + Utility.EHRPassword + ";";
                    bool IsEHRConnected = SystemBAL.GetPracticeWorkConnection();
                    if (IsEHRConnected)
                    {
                        //ViewAdminConfigurationPanel();
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        Utility.DBConnString = "Driver={Pervasive ODBC Engine Interface};ServerName=" + Utility.EHRHostname + ";dbq=" + Utility.EHRDatabase + ";UID=" + Utility.EHRUserId + ";PWD=" + Utility.EHRPassword + ";";
                        if (IsEHRConnected)
                        {
                            //ViewAdminConfigurationPanel();
                            SetPanelVisiblityEHRwise("Location", false);
                        }
                        else
                        {
                            Utility.APISessionToken = string.Empty;
                            Utility.EHRHostname = string.Empty;
                            Utility.EHRIntegrationKey = string.Empty;
                            Utility.EHRUserId = string.Empty;
                            Utility.EHRPassword = string.Empty;
                            Utility.EHRDatabase = string.Empty;
                            Utility.EHRPort = string.Empty;
                            ObjGoalBase.ErrorMsgBox("Authentication", "PracticeWork is not connecting. " + "\n" + " Please enter valid credentials.");
                            configErrors += "\n PracticeWork not connected.";
                        }
                    }
                    PostEHRConnectionLog("PracticeWork", IsEHRConnected);
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", "btnPracticeWorkSave_Click " + ex.Message);
                SaveEHRErroLog(errorMessage: ex.Message);
                configErrors += "\n PracticeWork authentication issue.";
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Button Events

        private void picNotAllowToChangeSystemDateFormat_Click(object sender, EventArgs e)
        {
            try
            {
                if (picNotAllowToChangeSystemDateFormat.Tag.ToString() == "OFF")
                {
                    picNotAllowToChangeSystemDateFormat.Image = Resources.ON;
                    picNotAllowToChangeSystemDateFormat.Tag = "ON";
                }

                else
                {
                    picNotAllowToChangeSystemDateFormat.Image = Resources.OFF;
                    picNotAllowToChangeSystemDateFormat.Tag = "OFF";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit Sync", "picNotAllowToChangeSystemDateFormat_Click " + ex.Message);
            }
        }


        private void btn_Tracker_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("Tracker", 6);
        }
        private void btn_CD_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("ClearDent", 5);
        }
        private void btn_DTX_Click(object sender, EventArgs e)
        {
            try
            {
                using (Stream cs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Pozative.Resources.Adit.pfx"))
                {
                    Byte[] raw = new Byte[cs.Length];
                    for (Int32 i = 0; i < cs.Length; ++i)
                        raw[i] = (Byte)cs.ReadByte();
                    X509Store store = new X509Store(StoreName.TrustedPeople, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadWrite);
                    X509Certificate2 cert = new X509Certificate2();
                    cert.Import(raw);
                    store.Add(cert);
                    store.Close();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("EHR User ddxdesktopspc.pfx Not Intalled. Please Aplication start with run as administrator. " + "btn_DTX_Click " + ex.Message);
            }

            SetValuesOnEHRSelection("Dentrix", 3);
        }
        private void btn_EG_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("Eaglesoft", 1);
        }
        private void btn_OD_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("Open Dental", 2);
        }
        private void btn_PWeb_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("PracticeWeb", 10);
        }
        private void btn_Pwork_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("PracticeWork", 7);
        }
        private void btn_EZ_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("Easy Dental", 8);
        }
        private void btn_SoftDent_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("SoftDent", 4);
        }
        private void btn_abeldent_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("AbelDent", 11);
        }
        private void SetValuesOnEHRSelection(string AppName, int EHRNo)
        {
            try
            {
                timer1.Enabled = true;
                Utility.Application_Name = AppName;
                cmbEHRName.SelectedValue = EHRNo;
                pnlMain.Visible = false;
                tblEHRMain.Visible = true;
                SetEHRConfigurations();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "SetValuesOnEHRSelection " + ex.Message);
            }
        }


        private void btn_AutomainSave_Click(object sender, EventArgs e)
        {
            tblEHRMain.Visible = true;
            SetEHRConfigurations();
        }
        private void btn_AutoMainCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ObjGoalBase.WriteToSyncLogFile("[btnEHRCancel_Click] manually Application restart");
                System.Environment.Exit(1);
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "btn_AutoMainCancel_Click " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void btn_OD_MouseHover(object sender, EventArgs e)
        {
            try
            {
                cButton btn = (cButton)sender;

                btn.ForeColor = ColorTranslator.FromHtml(orangeColor);
                btn.TextColor = ColorTranslator.FromHtml(orangeColor);
                btn.BorderColor = ColorTranslator.FromHtml(orangeColor);
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "btn_OD_MouseHover " + ex.Message);
            }
        }
        private void btn_OD_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                cButton btn = (cButton)sender;
                btn.ForeColor = ColorTranslator.FromHtml(darkGrayColor);
                btn.TextColor = ColorTranslator.FromHtml(darkGrayColor);
                btn.BorderColor = ColorTranslator.FromHtml(lightGrayColor);
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "btn_OD_MouseLeave " + ex.Message);
            }
        }

        #endregion
        #region Design changes
        private void tblOpenDentalMain_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Panel pnl = (Panel)sender;
                foreach (Control ctrlinner in pnl.Controls)
                {
                    if (ctrlinner is TextBox)
                    {
                        TextBox txt = (TextBox)ctrlinner;
                        txt.BorderStyle = BorderStyle.None;
                        SetBorderColor(txt.Location.X, txt.Location.Y, txt.ClientSize.Width, txt.ClientSize.Height, darkGrayColor, e.Graphics);
                    }
                    else if (ctrlinner is ComboBox)
                    {
                        ComboBox cmb = (ComboBox)ctrlinner;
                        SetBorderColor(cmb.Location.X, cmb.Location.Y, cmb.ClientSize.Width, cmb.ClientSize.Height, darkGrayColor, e.Graphics);
                    }
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "tblOpenDentalMain_Paint " + ex.Message);
            }
        }
        private void SetBorderColor(int x, int y, int width, int height, string color, Graphics graphics)
        {
            try
            {
                Rectangle rect = new Rectangle(x, y, width, height);

                rect.Inflate(1, 1);
                ControlPaint.DrawBorder(graphics, rect,
                   ColorTranslator.FromHtml(color), 0, ButtonBorderStyle.Solid,
                   ColorTranslator.FromHtml(color), 0, ButtonBorderStyle.Solid,
                   ColorTranslator.FromHtml(color), 0, ButtonBorderStyle.Solid,
                   ColorTranslator.FromHtml(color), 1, ButtonBorderStyle.Solid);
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "SetBorderColor " + ex.Message);
            }
        }
        #endregion

        #region Progressbar
        private void StartProgress()
        {
            pbrNewProgress.Visible = true;
            for (int j = 0; j <= 100; j++)
            {
                pbrNewProgress.Value = j;
                Application.DoEvents();
            }
        }
        #endregion
        private void SaveEHRErroLog(bool ehrConnected = false, bool errorLogFileGenerated = false, string errorMessage = "", string locationID = "", List<string> multiClinicSelected = null, bool multiDatabaseConfiure = false, string organizationID = "", bool successfullyConfigured = false)
        {
            string strApiLocOrg = SystemBAL.SaveEHRLogs();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            var client = new RestClient(strApiLocOrg);
            //client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("bharatkr", "12345678");
            var request = new RestRequest(Method.POST);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            request.AddHeader("apptype", "ehr");
            request.AddHeader("appmodule", "ehrlog");

            ulong ram = (new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory) / (1024 * 1024);
            SystemConfiguration SystemConfig = null;
            try
            {
                SystemConfig = new SystemConfiguration
                {
                    cpu = Environment.MachineName,
                    framework = Environment.Version.ToString(),
                    mac_address = CommonFunction.GetMACAddress(),
                    os = Environment.OSVersion.ToString(),
                    ram = ram.ToString() + "GB",
                };
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "SaveEHRErroLog " + ex.Message);
                //ObjGoalBase.ErrorMsgBox("SaveEHRErroLog -> SystemConfiguration", ex.Message);
            }
            PozativeConfigErrorLog AditLocOrgPost = new PozativeConfigErrorLog
            {
                datetime = DateTime.Now.ToString(),
                dentrix_pdf_connection_string = "",////
                ehr_connected = ehrConnected,
                email = txtAdminUserName.Text.Trim(),
                error_log_file_generated = errorLogFileGenerated,
                error_message = errorMessage,
                image_folder_path = "",///
                locationId = locationID,
                multiclinic_selected = multiClinicSelected,
                multidatabase_configure = multiDatabaseConfiure,
                organizationId = organizationID,
                password = txtAdminPassword.Text.Trim(),
                selected_ehr = selectedEHR + " - " + Utility.DBConnString,
                sucessfully_configured = successfullyConfigured,
                system_configuration = SystemConfig,
                total_installed_ehr = string.Join(",", installedEHR),
            };
            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(AditLocOrgPost);
            request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.ErrorMessage != null)
            {
                ObjGoalBase.ErrorMsgBox("Adit App Configuration ", response.ErrorMessage);
                return;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            return;
            try
            {
                if (pbrNewProgress.Value == 100)
                    pbrNewProgress.Value = 0;
                if (!isProcessComplete)
                {
                    pbrNewProgress.Visible = true;
                    pbrNewProgress.Value += 5;
                    Application.DoEvents();
                }
                else
                {
                    pbrNewProgress.Visible = false;
                    pbrNewProgress.Value = 0;
                    isProcessComplete = false;
                    timer1.Stop();
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "timer1_Tick " + ex.Message);
            }
        }
        
        private void PostEHRConnectionLog(string EHRName, bool isEHRConnected = true)
        {
            SaveEHRErroLog(ehrConnected: isEHRConnected, errorMessage: EHRName + " is " + (isEHRConnected ? " connected." : " not connected."));
        }
        private void frmConfiguration_Auto_Shown(object sender, EventArgs e)
        {
            try
            {
               // CommonFunction.TextToSpeech(CommonFunction.audioTextLst.welcomemessage);
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "frmConfiguration_Auto_Shown " + ex.Message);
            }
        }
        public void GetAutoPlayAudioText()
        {
            try
            {
                //string strGetLocUpdateVersion = SystemBAL.GetAutoPlayAudioText();

                //var clientLocUpdateVer = new RestClient(strGetLocUpdateVersion);
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                //var request = new RestRequest(Method.GET);
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //request.AddHeader("apptype", "aditehr");
                //request.AddHeader("pagetype", "welcome");
                //IRestResponse response = clientLocUpdateVer.Execute(request);

                //CommonFunction.audioTextLst = new AudioTextList();
                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    try
                //    {
                //        var audioTextList = JsonConvert.DeserializeObject<test>(response.Content);
                //        CommonFunction.audioTextLst = audioTextList.data;
                //    }
                //    catch (Exception ex)
                //    {
                //        ObjGoalBase.ErrorMsgBox("Pozative Service", "GetAutoPlayAudioText_Json.Deserialize " + ex.Message);
                //    }
                //}
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "GetAutoPlayAudioText " + ex.Message);
            }
        }

        private void btnFormMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnFormClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

