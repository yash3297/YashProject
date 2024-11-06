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
using System.DirectoryServices;
using Microsoft.Win32.TaskScheduler;
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.ComponentModel;

namespace Pozative
{
    public partial class frmConfiguration_Auto : Form
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
        string transparentColor = "#00FFFFFF";
        bool isProcessComplete = false;
        string selectedEHR = "";
        string configErrors = "";
        List<string> audioPlayed = new List<string>();
        DataTable dtEHRList = new DataTable();
        List<string> locationLst = new List<string>();
        List<string> locationWithClinicLst = new List<string>();
        string selectedLocations = "";
        string StrFirstLocationValue = string.Empty;
        string StrFirstLocationName = string.Empty;
        bool SkipSystemCredential = false;
        string helpDocURL = "";
        string selectedEHRtoConfig = "";
        bool isCalledFromSave = false;
        public string selectedEHRVersion = "";

        string AbelDentHostName = "";
        string AbelDentDatabase = "";
        string AbelDentUserId = "";
        string AbelDentPassword = "";

        string loggedUserEmailID = "";
        string loggedUserFirstName = "";
        string loggedUserLastName = "";
        string loggedUserOraganization = "";
        string loggedUserOraganizationID = "";
        DateTime installationDateTime;
        MailData mailData = new MailData();
        bool setupFromOTP;
        AdminLoginDetailBO adminLoginDetailBO_OTP = null;
        bool isInstallationComplete = false;
        #endregion

        #region Form Load

        public frmConfiguration_Auto(string tmpIsAction)
        {
            InitializeComponent();
            IsAction = tmpIsAction;

            this.Size = new Size(900, 650);
            this.CenterToScreen();
            cpnlMain.Size = new Size(437, 422);
            cpnlMain.MaximumSize = new Size(437, 422);
            cpnlMain.Location = new Point(232, 75);
        }

        private void frmConfiguration_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            try
            {
                lblYearAdit.Text = "©" + DateTime.Now.Year.ToString() + " Adit";
                installationDateTime = DateTime.Now;
                CommonFunction.CheckAudioDeviceWorking();
                Utility.DBConnString = "";
                Cursor.Current = Cursors.WaitCursor;
                setupFromOTP = Convert.ToBoolean(ConfigurationManager.AppSettings["SetUpFromOTP"]);
                bindFormControls();

                dtTempOgrTable = CreateTempOrgTableData();
                dtTempLocationTable = CreateTempLocationTableData();
                dtTempApptLocationTable = CreateTempAppointmentLocationTableData();
                dtTempPozativeLocationTable = CreateTempPozativeLocationTableData();
                this.StartPosition = FormStartPosition.CenterScreen;

                cmbEHRName.Focus();
                cmbEHRName.Select();

                System_Name = System.Environment.MachineName;
                processorID = ObjGoalBase.getUniqueID("C");
                picLoader.Location = cpnlMain.Location;
                picLoader.Size = cpnlMain.Size;
                picLoader.Visible = false;
                CommonFunction.supportEmail = CommonFunction.GetGernalInfo("support_email");
                mailData.DownloadInstallStatus = true;
                btnAdminUserSave.Tag = string.Empty;
                lblOrangeTick.Image = Properties.Resources.OrangeTickVector.GetThumbnailImage(16, 16, null, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                ShowWriteError("frmConfiguration_Load " + ex.Message, "Pozative Service", show: true, write: true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            this.DialogResult = DialogResult.None;
        }

        #endregion

        #region Common Function
        private static string DENTRIX_SUBKEYPATH = "Dentrix Dental Systems, Inc.\\Dentrix\\General";
        private static string OpenDental_SubKeyPath = "MakeMSI\\KeyPaths\\OpenDental\\";
        private static string EagleSoft_SubKeyPath = "Eaglesoft\\Server";
        private static string ClearDent_SubKeyPath = "Prococious Technology Inc.\\ClearDent";
        private static string Tracker_SubKeyPath = "";
        private static string PracticeWork_SubKeyPath = "SOFTWARE\\PWInc\\";
        private static string EasyDental_SubKeyPath = "SOFTWARE\\Easy Dental Systems, Inc.\\Easy Dental\\General";
        private static string AbelDent_SubKeyPath = "Eaglesoft\\Server";

        private bool IsAppInstalled(string appName)
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
                ShowWriteError("IsAppInstalled " + appName + " " + ex.Message, write: true, SaveEHRErroLogMsg: "IsAppInstalled " + ex.Message);
            }
            return false;
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
                        versions.AddRange(new List<string>() { "14.4.2", "14.8.2" });
                        break;

                    case "PracticeWork":
                        versions.AddRange(new List<string>() { "7.9+" });
                        break;

                    case "Eaglesoft":
                        versions.AddRange(new List<string>() { "17", "18.0", "19.11", "20.0", "21.20", "22.00", "23.00" });
                        break;

                    case "EasyDental":
                        versions.AddRange(new List<string>() { "11.1" });
                        break;

                    case "SoftDent":
                        versions.AddRange(new List<string>() { "17.0.0+" });
                        break;
                    case "CrystalPM":
                        versions.AddRange(new List<string>() { "6.1.9" });
                        break;
                    case "OfficeMate":
                        versions.AddRange(new List<string>() { "12.0.2","15" });
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
                ShowWriteError("GetEHRVersionList " + ex.Message, "Pozative Service", show: true, write: true);
            }
            return dtVersionList;
        }
        private void bindFormControls()
        {
            try
            {
                if (setupFromOTP)
                    SetPanelVisiblityEHRwise("EnterOTP", false);
                else
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
                btn_CP.Visible = false;

                dtEHRList = GetEHRListByRegistry();
                cmbEHRName.DataSource = dtEHRList;
                cmbEHRName.ValueMember = "EHR_ID";
                cmbEHRName.DisplayMember = "EHR_Name";
                cmbEHRName.DropDownStyle = ComboBoxStyle.DropDownList;
                if (dtEHRList.Rows.Count == 1)
                {
                    ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer. If you find this in error please give us a call at (832) 225-8865.", "Pozative Service", show: true, write: true);
                    Environment.Exit(0);
                }
                else if (dtEHRList.Rows.Count == 2)
                {
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

                cmbTrackerHostName.DataSource = null;
                cmbSqlServiceName.DataSource = null;

                SetEHRCredentialValues();
            }
            catch (Exception ex)
            {
                ShowWriteError("BindFormControls " + ex.Message, "Pozative Service", show: true, write: true);
            }
        }
        #region SetEHRCredentialValues
        private void SetEHRCredentialValues()
        {
            SetEagleSoftCredentialControls();
            SetOpenDentalCredentialControls();
            SetDentrixCredentrialControls();
            SetPracticeWebCredentrialControls();
            SetClearDentCredentrialControls();
            SetTrackerCredentrialControls();
            SetAbelDentCredentrialControls();
            SetPracticeworCredentrialControls();
            SetSoftDentCredentialControls();
            SetCrystalPMCredentialControls();
            SetOfficeMateCredentrialControls();

            txtAdminUserName.Text = string.Empty;
            txtAdminPassword.Text = string.Empty;
        }
        private void SetSoftDentCredentialControls()
        {
            txtSoftDentServerName.Text = "localhost";
            txtSoftDentDatabase.Text = "";
            txtSoftDentUserId.Text = "sa";
            txtSoftDentPassword.Text = "Gda09nG";
        }
        private void SetEagleSoftCredentialControls()
        {
            txtEaglesoftHostName.Text = "localhost";
            txtEaglesoftUserId.Text = "GGY";
            txtEaglesoftPassword.Text = string.Empty;
        }
        private void SetOpenDentalCredentialControls()
        {
            txtOpenDentalHostName.Text = "localhost";
            txtOpenDentalDatabase.Text = "OpenDental";
            txtOpenDentalUserId.Text = "root";
            txtOpenDentalPassword.Text = string.Empty;
        }
        private void SetCrystalPMCredentialControls()
        {
            txtCrystalPMHostName.Text = "localhost";
            txtCrystalPMDatabase.Text = "easyopti";
            txtCrystalPMUserId.Text = "root";
            txtCrystalPMPassword.Text = "CNIrules";
        }
        private void SetOfficeMateCredentrialControls()
        {
            try
            {
                txtOfficeMateDatabase.Text = "OMSQLDB";
                txtOfficeMateUserId.Text = "";
                txtOfficeMatePassword.Text = "";

                AbelDentHostName = txtOfficeMateHostName.Text.ToLower().Trim();
                AbelDentDatabase = txtOfficeMateDatabase.Text.Trim();
                AbelDentUserId = txtOfficeMateUserId.Text.Trim();
                AbelDentPassword = txtOfficeMatePassword.Text.Trim();
            }
            catch (Exception)
            {

            }            
        }
        private void SetDentrixCredentrialControls()
        {
            txtDentrixHostName.Text = "localhost";
            txtDentrixUserId.Text = "h36FadCg";
            txtDentrixPassword.Text = "eSnkTrvap";
            txtDentrixPort.Text = "6604";

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
        }
        private void SetPracticeWebCredentrialControls()
        {
            PracticeWeb_txthostName.Text = "localhost";
            txtPracticeWebDatabase.Text = "freedental";
            txtPracticeWebUseID.Text = "root";
            txtPracticeWebPassword.Text = string.Empty;
        }
        private void SetClearDentCredentrialControls()
        {
            txtClearDentHostName.Text = "(local)\\VSDOTNET";
            txtClearDentUserId.Text = "sa";
            txtClearDentPassowrd.Text = "";
        }
        private void SetTrackerCredentrialControls()
        {
            try
            {
                if (IsAppInstalled("Tracker Server"))
                {
                    if (EHRRegInstallationPath != "")
                    {
                        string configFilePath = EHRRegInstallationPath + "\\Settings.config";
                        DataSet ds = new DataSet();
                        ds.ReadXml(configFilePath);
                        Utility.EHRHostname = ds.Tables["Database"].Rows[0]["ServerName"].ToString();
                    }
                }

                txtTrackerDatabase.Text = "Tracker";
                txtTrackerCredential.Text = "Adit";
                txtTrackerUserId.Text = "TBNAdmin";
                txtTrackerPassword.Text = "{A827F13E-96B8-11DC-B1E3-A86156D89593}";
            }
            catch (Exception ex)
            {
                ShowWriteError("BindFormControls - SetEHRCredentialValues - SetTrackerCredentrialControls - " + ex.Message, write: true);
            }
        }
        private void SetAbelDentCredentrialControls()
        {
            cmbAbelDentHostName.DataSource = dtSqlServerName;
            cmbAbelDentHostName.DisplayMember = "SqlServerName";
            if (dtSqlServerName == null || (dtSqlServerName != null && dtSqlServerName.Rows.Count == 0))
            {
                cmbAbelDentHostName.Text = Environment.MachineName;
            }

            txtAbelDentDatabase.Text = "Abel";
            txtAbelDentUserId.Text = "sa";
            txtAbelDentPassword.Text = "Adit@123";

            AbelDentHostName = cmbAbelDentHostName.Text.ToLower().Trim();
            AbelDentDatabase = txtAbelDentDatabase.Text.Trim();
            AbelDentUserId = txtAbelDentUserId.Text.Trim();
            AbelDentPassword = txtAbelDentPassword.Text.Trim();
        }
        private void SetPracticeworCredentrialControls()
        {
            if (cmbEHRVersion.Text.ToLower() == "7.9+".ToLower())
            {
                txtPracticeworkHost.Text = "localhost";
                txtPracticeworkDbName.Text = "PW";
                txtPracticeworkUserId.Text = "";
                txtPracticeworkPassword.Text = "";
            }
        }
        #endregion

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
                ShowWriteError("CreateTempOrgTableData" + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("CreateTempLocationTableData " + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("CreateTempAppointmentLocationTableData " + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("CreateTempPozativeLocationTableData " + ex.Message, "Pozative Service", show: true, write: true);
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
                pnlSystemCredential.Visible = false;
                lblSkip.Visible = false;
                pnlSoftDent.Visible = false;
                pnlOTPEntry.Visible = false;
                pnlOTPStatus.Visible = false;
                pnlSharingPath.Visible = false;
                pnlPMSCredential.Visible = false;
                #endregion

                pnlTitle.Size = new Size(pnlTitle.Width, 100);
                lblMain1.TextAlign = ContentAlignment.MiddleCenter;
                lblMain1.Padding = new Padding(40, 0, 40, 0);
                panel1.Visible = false;

                if (EHRname == "ReportGenerated" || EHRname == "EnterOTP" || EHRname == "OTPStatusSuccess" || EHRname == "OTPStatusFail")
                {
                    if (cpnlMain.Controls.Find("pnlTitle", true).Count() > 0)
                        cpnlMain.Controls.Remove(pnlTitle);
                    selectedEHR = EHRname;
                }
                else
                {
                    lblMainDescription.Text = "";
                    if (isEHR || EHRname == "Location" || EHRname == "PMSCredential")
                    {
                        pnlTitle.Size = new Size(pnlTitle.Width, 110);
                        selectedEHR = EHRname;
                        lblMain1.Padding = new Padding(15, 0, 20, 0);
                        panel1.Visible = true;
                        panel1.Height = 30;
                        //btnChatOurSupportTeam.Visible = true;
                        lblKBArticle.Visible = true;
                    }

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
                            lblMain1.Text = "Enter your MySQL Credentials";
                            lblShowODPassword.Location = new Point((txtOpenDentalPassword.Location.X + txtOpenDentalPassword.Width + 5) - lblShowODPassword.Width, txtOpenDentalPassword.Location.Y - 5);
                        }
                        break;
                    case "Dentrix":
                        {
                            pnlDentrixMain.Visible = true;
                            if (cpnlMain.Controls.Find("pnlDentrixMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlDentrixMain);
                            pnlDentrixMain.Dock = DockStyle.Fill;
                            pnlDentrixMain.BringToFront();

                            btnDentrixSave.Focus();
                            lblMain1.Text = "Enter your Dentrix database Credentials";
                            lblShowDTXPassword.Location = new Point((txtDentrixPassword.Location.X + txtDentrixPassword.Width + 5) - lblShowDTXPassword.Width, txtDentrixPassword.Location.Y - 5);
                        }
                        break;
                    case "PracticeWeb":
                        {
                            pnlpnPracticeWeb.Visible = true;

                            if (cpnlMain.Controls.Find("pnlpnPracticeWeb", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlpnPracticeWeb);
                            pnlpnPracticeWeb.Dock = DockStyle.Fill;
                            pnlpnPracticeWeb.BringToFront();

                            txtPracticeWebPassword.Focus();
                            txtPracticeWebPassword.Text = string.Empty;
                            lblMain1.Text = "Enter your MySQL credentials";
                            lblShowPWebPassword.Location = new Point((txtPracticeWebPassword.Location.X + txtPracticeWebPassword.Width + 5) - lblShowPWebPassword.Width, txtPracticeWebPassword.Location.Y - 5);
                        }
                        break;
                    case "ClearDent":
                        {
                            pnlClearDentMain.Visible = true;

                            if (cpnlMain.Controls.Find("pnlClearDentMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlClearDentMain);
                            pnlClearDentMain.Dock = DockStyle.Fill;
                            pnlClearDentMain.BringToFront();

                            txtClearDentPassowrd.Text = "";
                            btnClearDentSave.Focus();
                            lblMain1.Text = "Enter your MsSql Credentials";
                            lblShowCDPassword.Location = new Point((txtClearDentPassowrd.Location.X + txtClearDentPassowrd.Width + 5) - lblShowCDPassword.Width, txtClearDentPassowrd.Location.Y - 5);
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

                                cmbTrackerHostName.Visible = false;
                                cmbSqlServiceName.Visible = true;
                                label19.Visible = false;
                                label22.Visible = true;

                                btnTrackerSave.Focus();
                                lblMain1.Text = "Enter your MySQL credentials";
                                lblShowTrackerPassword.Location = new Point((txtTrackerPassword.Location.X + txtTrackerPassword.Width + 5) - lblShowTrackerPassword.Width, txtTrackerPassword.Location.Y - 5);
                            }
                            catch (Exception ex)
                            {
                                ShowWriteError("Tracker Credential visibility issue - " + ex.Message, write: true);
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

                            btnAbeldentSave.Focus();
                            lblMain1.Text = "Enter your MySQL credentials";
                            lblShowADPassword.Location = new Point((txtAbelDentPassword.Location.X + txtAbelDentPassword.Width + 5) - lblShowADPassword.Width, txtAbelDentPassword.Location.Y - 5);
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
                                txtPracticeworkUserId.Text = "";
                                txtPracticeworkPassword.Text = "";
                            }

                            btnPracticeWorkSave.Focus();
                            lblMain1.Text = "Enter your PracticeWorks database credentials";
                            lblShowPWorkPassword.Location = new Point((txtPracticeworkPassword.Location.X + txtPracticeworkPassword.Width + 5) - lblShowPWorkPassword.Width, txtPracticeworkPassword.Location.Y - 5);
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
                            lblMain1.Text = "Enter your Eaglesoft database credentials";
                            lblShowEGPassword.Location = new Point((txtEaglesoftPassword.Location.X + txtEaglesoftPassword.Width + 5) - lblShowEGPassword.Width, txtEaglesoftPassword.Location.Y - 5);
                        }
                        break;
                    case "SoftDent":
                        {
                            pnlSoftDent.Visible = true;

                            if (cpnlMain.Controls.Find("pnlpnSoftDent", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlSoftDent);
                            pnlSoftDent.Dock = DockStyle.Fill;
                            pnlSoftDent.BringToFront();

                            lblMain1.Text = "Enter your Database Credentials";
                            lblShowSDPassword.Location = new Point((txtSoftDentPassword.Location.X + txtSoftDentPassword.Width + 5) - lblShowSDPassword.Width, txtSoftDentPassword.Location.Y - 5);
                        }
                        break;
                    case "CrystalPM":
                        {
                            pnlCrystalPMMain.Visible = true;
                            if (cpnlMain.Controls.Find("pnlCrystalPMMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlCrystalPMMain);
                            pnlCrystalPMMain.Dock = DockStyle.Fill;
                            pnlCrystalPMMain.BringToFront();

                            txtCrystalPMPassword.Focus();
                            txtCrystalPMPassword.Text = string.Empty;
                            lblMain1.Text = "Enter your MySQL Credentials";
                            lblShowCPPassword.Location = new Point((txtCrystalPMPassword.Location.X + txtCrystalPMPassword.Width + 5) - lblShowCPPassword.Width, txtCrystalPMPassword.Location.Y - 5);
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

                            lblShowAdminPassword.Location = new Point((txtAdminPassword.Location.X + txtAdminPassword.Width + 5) - lblShowAdminPassword.Width, txtAdminPassword.Location.Y - 5);

                            SetActivePozativeSync(true, "Adit");
                            SetActivePozativeSync(false, "Pozative");
                            lblMain1.Text = "Please log in to your Adit account";

                        }
                        break;
                    case "Location":
                        {
                            mailData.EHRConnectionStatus = true;

                            pnlLocationMain.Visible = true;

                            if (cpnlMain.Controls.Find("pnlLocationMain", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlLocationMain);
                            pnlLocationMain.Dock = DockStyle.Fill;
                            pnlLocationMain.BringToFront();

                            lblMain1.Text = "Please select your location";
                            lblMainDescription.Text = "Select which PMS location will sync to each Adit location:";

                            LblAditLocationSingle.Visible = false;
                            cmbAditLocation.Visible = false;
                            DGVMuliClinc.Visible = false;

                            //RBtnMultiClinic.Checked = true;
                            if (RBtnMultiClinic.Checked)
                            {
                                pnlTitle.Size = new Size(pnlTitle.Width, 125);
                                pnlTitle.Padding = new Padding(pnlTitle.Padding.Left, pnlTitle.Padding.Top, pnlTitle.Padding.Right, 8);
                                btnLocationBack.Location = new Point(68, 220);
                                btnLocationSave.Location = new Point(227, 220);
                                DGVMuliClinc.Visible = true;

                                if (dtTempOpenDentalClinicTable.Rows.Count <= 0)
                                {
                                    dtTempOpenDentalClinicTable.Columns.Add("Clinic_Number", typeof(string));
                                    dtTempOpenDentalClinicTable.Columns.Add("Description", typeof(string));
                                    DataRow drApptLocDef = dtTempOpenDentalClinicTable.NewRow();
                                    drApptLocDef["Clinic_Number"] = "0";
                                    drApptLocDef["Description"] = loggedUserOraganization;
                                    dtTempOpenDentalClinicTable.Rows.Add(drApptLocDef);
                                    dtTempOpenDentalClinicTable.AcceptChanges();
                                }
                                    DGVMuliClinc.DataSource = dtTempOpenDentalClinicTable;

                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DataSource = dtTempApptLocationTable.Copy();
                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).ValueMember = "id";
                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DisplayMember = "name";
                                

                                for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                                {
                                    if (dtTempApptLocationTable != null && dtTempApptLocationTable.Rows.Count == 2)
                                        DGVMuliClinc.Rows[i].Cells["Location"].Value = dtTempApptLocationTable.Rows[1]["id"].ToString();
                                    else
                                        DGVMuliClinc.Rows[i].Cells["Location"].Value = "0";

                                }
                                DGVMuliClinc.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold, GraphicsUnit.Pixel);
                                if (!audioPlayed.Contains("multipleclinicsexists"))
                                {
                                    audioPlayed.Add("multipleclinicsexists");
                                    CommonFunction.TextToSpeech("multipleclinicsexists");
                                }
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

                            if (setupFromOTP)
                            {
                                btnLocationBack.Visible = false;
                                btnLocationSave.Location = new Point(147, btnLocationSave.Location.Y);
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

                            lblMain1.Text = "Please select your Patient Management Software (PMS)";
                            if (cmbEHRName.Items.Count > 1)
                            {
                                int cnt = cmbEHRName.Items.Count;
                                int l = flowLayoutPanel1.Padding.Left;
                                int r = flowLayoutPanel1.Padding.Right;
                                int b = flowLayoutPanel1.Padding.Bottom;
                                if (cnt == 2)
                                    flowLayoutPanel1.Padding = new Padding(l, 115, r, b);
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
                            if (cmbEHRName.Items.Count > 2)
                            {
                                if (!audioPlayed.Contains("multipleehrexists"))
                                {
                                    audioPlayed.Add("multipleehrexists");
                                    CommonFunction.TextToSpeech("multipleehrexists");
                                }
                            }
                        }
                        break;
                    case "ReportGenerated":
                        {
                            this.ShowInTaskbar = false;
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
                            if (!audioPlayed.Contains("success"))
                            {
                                audioPlayed.Add("success");
                                CommonFunction.TextToSpeech("success");
                            }
                        }
                        break;
                    case "EnterOTP":
                        {
                            ShowHideLoader(false);
                            if (cpnlMain.Controls.Find("pnlTitle", true).Count() > 0)
                            {
                                cpnlMain.Controls.Remove(pnlTitle);
                            }
                            pnlOTPEntry.Visible = true;
                            if (cpnlMain.Controls.Find("pnlOTPEntry", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlOTPEntry);
                            pnlOTPEntry.Dock = DockStyle.Fill;
                            pnlOTPEntry.BringToFront();

                            SetActivePozativeSync(true, "Adit");
                            SetActivePozativeSync(false, "Pozative");
                            txtOTPDigit1.Focus();
                        }
                        break;
                    case "OTPStatusSuccess":
                        {
                            if (cpnlMain.Controls.Find("pnlTitle", true).Count() > 0)
                            {
                                cpnlMain.Controls.Remove(pnlTitle);
                            }
                            pnlOTPStatus.Visible = true;
                            if (cpnlMain.Controls.Find("pnlOTPStatus", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlOTPStatus);
                            pnlOTPStatus.Dock = DockStyle.Fill;
                            pnlOTPStatus.BringToFront();

                            picVerificationLogo.Image = Properties.Resources.SecuritySuccess;
                            picVerificationLogo.Location = new Point(175, 53);
                            lblVerificationStatus.Text = "Verification Successful";
                            lblVerificationStatusDesc.Text = "we have successfully verified your details. You can now proceed for installation.";
                            btnOTPVerification.Text = "Start";
                            btnOTPVerification.Tag = "Start";
                            lblVerificationStatusDesc.Padding = new Padding(60, 0, 60, 0);
                        }
                        break;
                    case "OTPStatusFail":
                        {
                            if (cpnlMain.Controls.Find("pnlTitle", true).Count() > 0)
                            {
                                cpnlMain.Controls.Remove(pnlTitle);
                            }
                            pnlOTPStatus.Visible = true;
                            if (cpnlMain.Controls.Find("pnlOTPStatus", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlOTPStatus);
                            pnlOTPStatus.Dock = DockStyle.Fill;
                            pnlOTPStatus.BringToFront();

                            picVerificationLogo.Image = Properties.Resources.SecurityFail;
                            picVerificationLogo.Location = new Point(163, 53);
                            lblVerificationStatus.Text = "Verification Failed";
                            lblVerificationStatusDesc.Text = "Invalid OTP. Ensure the code is correct and try again.";
                            btnOTPVerification.Text = "Retry";
                            btnOTPVerification.Tag = "Retry";
                            lblVerificationStatusDesc.Padding = new Padding(10, 0, 10, 0);
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
                    case "SystemCredential":
                        {
                            
                            btnSystemUserNext.Enabled = false;

                            pnlTitle.Size = new Size(pnlTitle.Width, 100);
                            pnlTitle.Padding = new Padding(pnlTitle.Padding.Left, pnlTitle.Padding.Top, pnlTitle.Padding.Right, 8);
                            lblMain1.Padding = new Padding(50, 35, 50, 0);

                            lblSkip.Visible = true;
                            pnlSystemCredential.Visible = true;
                            cmbSystemUser.DataSource = GetComputerUsers();

                            if (cpnlMain.Controls.Find("pnlSystemCredential", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlSystemCredential);
                            pnlSystemCredential.Dock = DockStyle.Fill;
                            pnlSystemCredential.BringToFront();

                            lblShowSystemPassword.Location = new Point((txtSystemUserPassword.Location.X + txtSystemUserPassword.Width + 5) - lblShowSystemPassword.Width, txtSystemUserPassword.Location.Y - 5);

                            lblMain1.Text = "Please enter an admin login for this computer";

                            helpDocURL = "https://help.adit.com/portal/en/kb/articles/why-we-login-credentials-for-administrator-level-access";
                            //CommonFunction.GetGernalInfo("helppage");

                            lblOrangeTick.Image = Properties.Resources.OrangeTickVector.GetThumbnailImage(14, 14, null, IntPtr.Zero);
                            lblOrangeTick.Size = new Size(16, 16);
                            lblOrangeTick.MaximumSize = new Size(16, 16);
                            lblOrangeTick.Location = new Point(chkIsZohoAssistAllowed.Location.X, lblOrangeTick.Location.Y);
                        }
                        break;
                    case "PMSCredential":
                        {
                            pnlTitle.Size = new Size(pnlTitle.Width, 140);
                            pnlTitle.Padding = new Padding(pnlTitle.Padding.Left, pnlTitle.Padding.Top, pnlTitle.Padding.Right, 8);
                            lblMain1.Padding = new Padding(45, 0, 45, 0);


                            pnlPMSCredential.Visible = true;
                            if (cpnlMain.Controls.Find("pnlPMSCredential", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlPMSCredential);
                            pnlPMSCredential.Dock = DockStyle.Fill;
                            pnlPMSCredential.BringToFront();

                            lblShowPMSPassword.Location = new Point((txtPMSUserPassword.Location.X + txtPMSUserPassword.Width + 5) - lblShowPMSPassword.Width, txtPMSUserPassword.Location.Y - 5);

                            lblMain1.Text = "Please enter an admin login for your PMS";
                            lblMainDescription.Text = "This allows the Adit sync app to read and write data to your PMS.";

                            helpDocURL = "https://help.adit.com/portal/en/kb/articles/why-we-login-credentials-for-administrator-level-access";
                            //CommonFunction.GetGernalInfo("helppage");
                        }
                        break;
                    case "SharedPath":
                        {
                            lblsharedpath.Text = "";
                            lblMain1.Padding = new Padding(40, 50, 25, 0);
                            label46.Image = Properties.Resources.Cancel.GetThumbnailImage(14, 14, null, IntPtr.Zero);
                            pnlSharingPath.Visible = true;
                            if (cpnlMain.Controls.Find("pnlSharingPath", true).Count() < 1)
                                cpnlMain.Controls.Add(pnlSharingPath);
                            pnlSharingPath.Dock = DockStyle.Fill;
                            pnlSharingPath.BringToFront();
                            picSharedPath.Visible = true;
                            pnltextsharedpath.Visible = false;
                            lblfolderpath.Visible = false;
                            lblsharedpath.Visible = false;
                            lblMain1.Text = "Select Folder Path";
                            btnSharingPathSave.ForeColor = Color.White;
                            btnSharingPathSave.TextColor = Color.White;
                        }
                        break;
                    case "OfficeMate":
                        {
                            try
                            {
                                pnlOfficeMateMain.Visible = true;
                                pnlOfficeMateMain.Visible = true;
                                if (cpnlMain.Controls.Find("pnlOfficeMateMain", true).Count() < 1)
                                    cpnlMain.Controls.Add(pnlOfficeMateMain);
                                pnlOfficeMateMain.Dock = DockStyle.Fill;
                                pnlOfficeMateMain.BringToFront();

                                txtOfficeMatePassword.Focus();
                                txtOfficeMatePassword.Text = string.Empty;
                                lblMain1.Text = "Enter your MySQL Credentials";
                                lblShowCPPassword.Location = new Point((txtOfficeMatePassword.Location.X + txtOfficeMatePassword.Width + 5) - lblShowCPPassword.Width, txtOfficeMatePassword.Location.Y - 5);
                            }
                            catch (Exception)
                            {

                            }                            
                        }
                        break;
                    default:
                        break;
                }
                if (isEHR && EHRname != "ReportGenerated")
                {
                    //btnChatOurSupportTeam.BringToFront();
                    lblKBArticle.BringToFront();
                    if (!audioPlayed.Contains("ehrdefaultconfigurationmissing for " + EHRname))
                    {
                        audioPlayed.Add("ehrdefaultconfigurationmissing for " + EHRname);
                        CommonFunction.TextToSpeech("ehrdefaultconfigurationmissing", " for " + EHRname);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("SetPanelVisibilityEHRwise " + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("SetActivePozativeSync " + ex.Message, "Pozative Service", show: true, write: true);
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
                if (odbcDSNs != null)
                {
                    foreach (ODBCMngr.ODBCDSN item in odbcDSNs)
                    {
                        if (item.ToString().ToUpper() == "DENTAL")
                        {
                            driverName = item.GetDSNDriverName().Replace("Adaptive Server", "SQL");
                            try
                            {
                                driverName = driverName.Replace(".0", "");
                            }
                            catch (Exception ex)
                            {
                                ShowWriteError("CreateDSN - odbcDSNs - " + ex.Message, write: true);
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
                                catch (Exception ex)
                                {
                                    ShowWriteError("CreateDSN - DSN not exist - " + ex.Message, write: true);
                                    if (serverNameFromUser != "")
                                    {
                                        serverName = serverNameFromUser;
                                    }
                                    else
                                    {
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
                else
                    return false;
            }
            catch (Exception ex)
            {
                ShowWriteError("CreateDSN " + ex.Message, "Pozative Service", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                return false;
            }
        }
        public bool DSNExists(string dsnName)
        {
            try
            {
                var sourcesKey = Registry.LocalMachine.OpenSubKey(ODBCINST_INI_REG_PATH + "ODBC Data Sources");

                if (sourcesKey == null)
                {
                    ShowWriteError("DSNExists - ODBC Registry key for sources does not exist", write: true);
                    throw new Exception("ODBC Registry key for sources does not exist");
                }

                string[] blah = sourcesKey.GetValueNames();

                Console.WriteLine("length: " + blah.Length);

                if (sourcesKey.GetValue(dsnName) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("DSNExists - " + ex.Message, write: true);
                return false;
            }
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
                    ShowWriteError("ODBC Registry key for DSN was not created", write: true, forEmail: true, emailType: "EHRConnectedError");
                    throw new Exception("ODBC Registry key for DSN was not created");
                }

                dsnKey.SetValue("AutoStop", "YES");
                dsnKey.SetValue("CommLinks", "SHMEM,TCPIP{}");
                dsnKey.SetValue("DescribeCursor", "Always");
                dsnKey.SetValue("Description", description);
                dsnKey.SetValue("Driver", driverPath);
                dsnKey.SetValue("EngineName", server);
                dsnKey.SetValue("ServerName", server);
                dsnKey.SetValue("Start", "");
                dsnKey.SetValue("Trusted_Connection", true);

                var datasourcesKey = Registry.LocalMachine.CreateSubKey(ODBCINST_INI_REG_PATH + "ODBC Data Sources");
                if (datasourcesKey == null)
                {
                    ShowWriteError("ODBC Registry key does not exist", write: true, forEmail: true, emailType: "EHRConnectedError");
                    throw new Exception("ODBC Registry key does not exist");
                }
                datasourcesKey.SetValue(dsnName, driverName);
            }
            catch (Exception ex)
            {
                ShowWriteError("CreateDSNEgaleSoft " + ex.Message, "Pozative Service", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
            }
        }

        public void CheckDSNDriver()
        {
            try
            {
                if (DSNExists("PozativeDSN"))
                {
                    string DentalDriverLocation = string.Empty;
                    string appKeyDental = @"SOFTWARE\ODBC\ODBC.INI\Dental\";
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKeyDental))
                    {
                        if (registryKey != null)
                        {
                            DentalDriverLocation = registryKey.GetValue("Driver").ToString();
                        }
                    }

                    string PozativeDriverLocation = string.Empty;
                    string appKeyPozative = @"SOFTWARE\ODBC\ODBC.INI\PozativeDSN\";
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKeyPozative))
                    {
                        if (registryKey != null)
                        {
                            PozativeDriverLocation = registryKey.GetValue("Driver").ToString();
                        }
                    }

                    if (DentalDriverLocation.ToLower() != PozativeDriverLocation.ToLower())
                    {
                        appKeyPozative = @"SOFTWARE\ODBC\ODBC.INI\PozativeDSN\";
                        using (var dsnKey = Registry.LocalMachine.OpenSubKey(appKeyPozative, true))
                        {
                            dsnKey.SetValue("Driver", DentalDriverLocation);
                            dsnKey.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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
                CommonFunction.KillSpeechExe();
                System.Environment.Exit(1);
            }
            catch (Exception ex)
            {
                ShowWriteError("btnClose_click " + ex.Message, "Pozative Service", show: true, write: true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #region EHR Configuration

        static string EHRRegInstallationPath = "";
        static string EHRRegVersion = "";
        private DataTable GetEHRListByRegistry()
        {
            DataTable dtEHRList = new DataTable();
            try
            {
                bool addEHR = false;
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
                    addEHR = false;
                    if (IsAppInstalled("Patterson Eaglesoft"))
                    {
                        addEHR = true;
                    }
                    else
                    {
                        string appKey = @"SOFTWARE\Eaglesoft\Paths";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                addEHR = true;
                            }
                        }
                    }
                    if (addEHR)
                    {
                        dtEHRList.Rows.Add(1, "Eaglesoft");
                        btn_EG.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - Eaglesoft " + ex.Message, write: true);
                }
                try
                {
                    addEHR = false;
                    if (IsAppInstalled("OpenDental"))
                    {
                        addEHR = true;
                    }
                    else
                    {
                        string appKey = @"SOFTWARE\WOW6432Node\MakeMSI\KeyPaths\OpenDental";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                addEHR = true;
                            }
                        }
                    }
                    if (addEHR)
                    {
                        dtEHRList.Rows.Add(2, "Open Dental");
                        btn_OD.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - Open Dental " + ex.Message, write: true);
                }

                try
                {
                    addEHR = false;
                    if (IsAppInstalled("Dentrix"))
                    {
                        addEHR = true;
                    }
                    else
                    {
                        string appKey = @"SOFTWARE\Dentrix Dental Systems, Inc.\Dentrix\General";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                addEHR = true;
                            }
                        }
                    }
                    if (addEHR)
                    {
                        dtEHRList.Rows.Add(3, "Dentrix");
                        btn_DTX.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - Dentrix " + ex.Message, write: true);
                }
                try
                {
                    string appKey = @"SOFTWARE\PWInc\PWSvr";
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            string IsPwork = registryKey.GetValue("PWSvrDir").ToString();
                            if (IsPwork.ToLower().Contains("softdent"))
                            {
                                dtEHRList.Rows.Add(4, "SoftDent");
                                btn_SoftDent.Visible = true;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - SoftDent " + ex.Message, write: true);
                }
                try
                {
                    addEHR = false;

                    if (IsAppInstalled("ClearDent"))
                    {
                        addEHR = true;
                    }
                    else
                    {
                        string appKey = @"SOFTWARE\WOW6432Node\Prococious Technology Inc.\ClearDent\Version";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                addEHR = true;
                            }
                        }
                    }
                    if (addEHR)
                    {
                        dtEHRList.Rows.Add(5, "ClearDent");
                        btn_CD.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - ClearDent " + ex.Message, write: true);
                }
                try
                {
                    addEHR = false;

                    if (IsAppInstalled("Tracker Server"))
                    {
                        dtEHRList.Rows.Add(6, "Tracker");
                        btn_Tracker.Visible = true;
                    }
                    else
                    {
                        string appKey = @"SOFTWARE\The Bridge Network\Tracker Setup\Client";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                dtEHRList.Rows.Add(6, "Tracker");
                                btn_Tracker.Visible = true;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - Tracker " + ex.Message, write: true);
                }
                try
                {
                    string appKey = @"SOFTWARE\PWInc\PWSvr";
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            string IsPwork = registryKey.GetValue("PWSvrDir").ToString();
                            if (IsPwork.ToLower().Contains("pworks"))
                            {
                                dtEHRList.Rows.Add(7, "PracticeWork");
                                btn_Pwork.Visible = true;
                            }
                        }
                    }
                    if (!btn_Pwork.Visible)
                    {
                        appKey = @"SOFTWARE\WOW6432Node\PWInc\PWSvr";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                string IsPwork = registryKey.GetValue("PWSvrDir").ToString();
                                if (IsPwork.ToLower().Contains("pworks"))
                                {
                                    dtEHRList.Rows.Add(7, "PracticeWork");
                                    btn_Pwork.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - PracticeWork " + ex.Message, write: true);
                }
                try
                {
                    addEHR = false;

                    if (IsAppInstalled("Easy Dental"))
                    {
                        addEHR = true;
                    }
                    if (addEHR)
                    {
                        dtEHRList.Rows.Add(8, "Easy Dental");
                        btn_EZ.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - Easy Dental " + ex.Message, write: true);
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
                                    break;
                                }
                            }
                        }
                    }

                    if (!btn_PWeb.Visible)
                    {
                        string appKey1 = @"SOFTWARE\WOW6432Node\MakeMSI\KeyPaths\Free Dental";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey1))
                        {
                            if (registryKey != null)
                            {
                                dtEHRList.Rows.Add(10, "PracticeWeb");
                                btn_PWeb.Visible = true;
                            }
                        }
                    }
                    if (!btn_PWeb.Visible)
                    {
                        SetPracticeWebCredentrialControls();
                        DataTable dt = GetEHRVersionList("PracticeWeb");
                        string version = dt != null ? dt.Rows[1]["Version_Name"].ToString() : "";
                        bool isConneted = IsPracticeWebConnected(true, version);
                        if (isConneted)
                        {
                            dtEHRList.Rows.Add(10, "PracticeWeb");
                            btn_PWeb.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - PracticeWeb " + ex.Message, write: true);
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
                        }
                    }
                    if (!btn_abeldent.Visible)
                    {
                        appKey = @"SOFTWARE\WOW6432Node\ABELSoft\ABELDent";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                dtEHRList.Rows.Add(11, "AbelDent");
                                btn_abeldent.Visible = true;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - AbelDent " + ex.Message, write: true);
                }
                try
                {
                    if (IsAppInstalled("Crystal PM Server Client"))
                    {
                        addEHR = true;
                    }
                    if (addEHR)
                    {
                        dtEHRList.Rows.Add(12, "CrystalPM");
                        btn_CP.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - CrystalPM " + ex.Message, write: true);
                }                
                try
                {
                    string appKey = @"SOFTWARE\WOW6432Node\OfficeMate Software Solutions\OfficeMate";
                    using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    {
                        if (registryKey != null)
                        {
                            dtEHRList.Rows.Add(13, "OfficeMate");
                            btn_OM.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetEHRListByRegistry - OfficeMate " + ex.Message, write: true);
                }
                dtEHRList.AcceptChanges();
            }
            catch (Exception ex)
            {
                ShowWriteError("GetEHRList " + ex.Message, "Pozative Service", show: true, write: true);
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
                        if (EagleSoftPath != "")
                        {

                            try
                            {
                                string configFilePath = EagleSoftPath + "\\PattersonUpgrade.exe.config";

                                if (File.Exists(configFilePath))
                                {
                                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                                    fileMap.ExeConfigFilename = configFilePath;
                                    Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                                    ConfigurationSection cs = config != null ? config.GetSection("runtime") : null;
                                    if (cs != null)
                                    {
                                        string sectionXml = cs.SectionInformation.GetRawXml();
                                        if (!string.IsNullOrEmpty(sectionXml))
                                        {
                                            DataSet ds = new DataSet();
                                            ds.ReadXml(new System.IO.StringReader(sectionXml));
                                            if (ds.Tables["bindingRedirect"] != null && ds.Tables["bindingRedirect"].Rows.Count > 0 && !string.IsNullOrEmpty(ds.Tables["bindingRedirect"].Rows[0]["newVersion"].ToString()))
                                            {
                                                EHRRegVersion = ds.Tables["bindingRedirect"].Rows[0]["newVersion"].ToString();
                                                if (new Version(EHRRegVersion) < new Version("16.0.0.0"))
                                                {
                                                    SetEHRRegVersion();
                                                }
                                            }
                                            else
                                            {
                                                SetEHRRegVersion();
                                            }
                                        }
                                        else
                                        {
                                            SetEHRRegVersion();
                                        }
                                    }
                                    else
                                    {
                                        SetEHRRegVersion();
                                    }
                                }
                                else
                                {
                                    SetEHRRegVersion();
                                }
                            }
                            catch (Exception ex)
                            {
                                ShowWriteError("GetVersion - EagleSoft - " + ex.Message, write: true);
                                SetEHRRegVersion();
                            }

                            void SetEHRRegVersion()
                            {
                                string versionKey = @"SOFTWARE\Eaglesoft\Select";
                                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(versionKey))
                                {
                                    if (registryKey != null)
                                    {
                                        EHRRegVersion = registryKey.GetValue("Version").ToString().Replace("Version ", "");
                                    }
                                }
                            }
                        }
                        dtEHRversionList = GetEHRVersionList("Eaglesoft");
                        EHRSelectedIndex = GetVersionID(dtEHRversionList, "16.0.0.0", EHRRegVersion);

                    }
                    catch (Exception ex)
                    {
                        ShowWriteError("GetVersion - EagleSoft " + ex.Message, write: true);
                    }
                }
                else if (EHRName == "Dentrix")
                {
                    #region commented
                    //string appKey = @"SOFTWARE\Dentrix Dental Systems, Inc.\Dentrix\General";
                    //using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                    //{
                    //    if (registryKey != null)
                    //    {
                    //        Version = registryKey.GetValue("Installed Version").ToString();
                    //        EHRVersion = new Version(Version);
                    //    }
                    //} 
                    //dtEHRversionList = GetEHRVersionList("Dentrix");
                    //if (EHRVersion.Major <= 15)
                    //{
                    //}
                    //else if (EHRVersion.Major <= 16 && EHRVersion.Minor < 20)
                    //{
                    //    EHRSelectedIndex = 2;
                    //}
                    //else
                    //{
                    //    EHRSelectedIndex = 3;
                    //}
                    #endregion
                    try
                    {

                        dtEHRversionList = GetEHRVersionList("Dentrix");
                        Utility.DBConnString = "host=localhost;UID=eRxUser;PWD=G87_iwx0Y;Database=DentrixSQL;DSN=c-treeACE ODBC Driver;port=6604";
                        bool isConnected = SystemBAL.GetEHRDentrixConnection();
                        if (isConnected)
                        {
                            EHRSelectedIndex = 1;
                        }
                        else
                        {
                            Utility.DBConnString = "host=localhost;UID=ewwb6ycp;PWD=a6Vys6MRt;Database=DentrixSQL;DSN=c-treeACE ODBC Driver;port=6604";
                            isConnected = SystemBAL.GetEHRDentrixConnection();
                            if (isConnected)
                            {
                                EHRSelectedIndex = 2;
                            }
                            else
                            {
                                EHRSelectedIndex = 3;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowWriteError("GetVersion - Dentrix " + ex.Message, write: true);
                    }
                }
                else if (EHRName == "ClearDent")
                {
                    try
                    {
                        string appKey = @"SOFTWARE\WOW6432Node\Prococious Technology Inc.\ClearDent\Version";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                Version = registryKey.GetValue("Current ClearDent Version").ToString();
                                EHRVersion = new Version(Version);
                            }
                        }
                        dtEHRversionList = GetEHRVersionList("ClearDent");
                        EHRSelectedIndex = GetVersionID(dtEHRversionList, "8.0.0.0", Version);
                    }
                    catch (Exception ex)
                    {
                        ShowWriteError("GetVersion - ClearDent " + ex.Message, write: true);
                    }
                }
                else if (EHRName == "Easy Dental")
                {
                    dtEHRversionList = GetEHRVersionList("EasyDental");
                    EHRSelectedIndex = 1;

                }
                else if (EHRName == "Open Dental")
                {
                    try
                    {
                        string appKey = @"SOFTWARE\WOW6432Node\MakeMSI\KeyPaths\OpenDental";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                string[] OpenDentalVersion = registryKey.GetSubKeyNames();
                                Version = OpenDentalVersion[0].ToString();
                                EHRVersion = new Version(Version);
                            }
                        }
                        dtEHRversionList = GetEHRVersionList("OpenDental");
                        EHRSelectedIndex = GetVersionID(dtEHRversionList, "14.0.0.0", Version);
                    }
                    catch (Exception ex)
                    {
                        ShowWriteError("GetVersion - OpenDental " + ex.Message, write: true);
                    }

                }
                else if (EHRName == "PracticeWork" || EHRName == "SoftDent" || EHRName == "Tracker" || EHRName == "PracticeWeb" || EHRName == "AbelDent" || EHRName == "CrystalPM" || EHRName == "OfficeMate")
                {
                    dtEHRversionList = GetEHRVersionList(EHRName);
                    EHRSelectedIndex = 1;
                }

                if (dtEHRversionList.Rows.Count > 0)
                {
                    cmbEHRVersion.DataSource = dtEHRversionList;
                    cmbEHRVersion.ValueMember = "Version_ID";
                    cmbEHRVersion.DisplayMember = "Version_Name";
                    cmbEHRVersion.SelectedValue = EHRSelectedIndex;
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("GetVersion " + ex.Message, "Pozative Service", show: true, write: true);
            }
            return "";
        }
        public int GetVersionID(DataTable dtEHRVersionList, string sversion, string EHRVer)
        {
            int EHRSelectedIndex = 0;
            try
            {
                if (!string.IsNullOrEmpty(EHRVer))
                {
                    string[] evarray = EHRVer.Replace("+", "").Split('.');
                    string ehrver = "";
                    if (evarray.Length > 1)
                    {
                        if (evarray[1].Length == 1)
                        {
                            evarray[1] = evarray[1] + "0";
                            for (int i = 0; i < evarray.Length; i++)
                            {
                                ehrver += evarray[i] + (i == (evarray.Length - 1) ? "" : ".");
                            }
                            EHRVer = ehrver;
                        }
                    }
                }
                Version EHRVersion = new Version(EHRVer);
                if (EHRVersion >= new Version("22.00"))
                {
                    EHRVersion = new Version((EHRVersion.Major).ToString().Trim() + ".00.0.0");
                }
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
                        }
                        if (cvarray.Length > 1)
                        {
                            CurrentVersion = new Version(Convert.ToInt16(cvarray[0]), Convert.ToInt16(cvarray[1]));
                        }
                        else
                        {
                            CurrentVersion = new Version(Convert.ToInt16(cvarray[0]), 0);
                        }
                        if (EHRVersion >= startversion && EHRVersion <= CurrentVersion)
                        {
                            EHRVersion = startversion;
                            EHRSelectedIndex = Convert.ToInt16(dtEHRVersionList.Rows[i - 1]["Version_Id"]);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError("GetVersionID " + ex.Message, write: true);
                }
                if (EHRSelectedIndex == 0)
                {
                    EHRSelectedIndex = Convert.ToInt32(dtEHRVersionList.Rows[dtEHRVersionList.Rows.Count - 1]["Version_Id"]);
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("GetVersionID " + ex.Message, write: true);
            }
            return EHRSelectedIndex;
        }
        private void SetMultiClinicForEHR(string EHRName)
        {
            try
            {
                if (EHRName == "OpenDental")
                {
                    dtTempOpenDentalClinicTable = SynchOpenDentalBAL.GetOpenDentalClinicData(Utility.DBConnString);
                }
                else if (EHRName == "CrystalPM")
                {
                    dtTempOpenDentalClinicTable = AditCrystalPM.BAL.Cls_Sync_Common.GetCrystalPMClinicData(Utility.DBConnString);
                }
                else if (EHRName == "OfficeMate")
                {
                    dtTempOpenDentalClinicTable = AditOfficeMate.BAL.Cls_Synch_Patient.GetOfficeMateClinicData(Utility.DBConnString);
                }

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
                ShowWriteError("SetMultiClinicForEHR" + ex.Message, "Pozative Service", show: true, write: true);
            }
        }
        private void btnEHRSave_Click(object sender, EventArgs e)
        {
            CommonFunction.KillSpeechExe();
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
                    ShowWriteError("Please select valid EHR", "EHR Name", show: true);
                    cmbEHRName.Focus();
                    return;
                }
                if (cmbEHRVersion.SelectedIndex == 0)
                {
                    ShowWriteError("Please select valid EHR Version", "EHR Version", show: true);
                    cmbEHRVersion.Focus();
                    return;
                }

                switch (Convert.ToInt16(cmbEHRName.SelectedValue))
                {
                    case 1://Eaglesoft
                        {
                            ShowHideLoader(true);
                            btnChatOurSupportTeam.Visible = false;
                            lblKBArticle.Visible = false;
                            if (cmbEHRVersion.Text.ToLower() == "23.00".ToLower() || cmbEHRVersion.Text.ToLower() == "22.00".ToLower() || cmbEHRVersion.Text.ToLower() == "21.20".ToLower() || cmbEHRVersion.Text.ToLower() == "20.0".ToLower() || cmbEHRVersion.Text.ToLower() == "19.11".ToLower() || cmbEHRVersion.Text.ToLower() == "18.0".ToLower() || cmbEHRVersion.Text.ToLower() == "17".ToLower())
                            {
                                #region Code moved to background worker
                                //Cursor.Current = Cursors.WaitCursor;
                                //bool isEHRConnected = IsEagleSoftConnected(false);
                                //if (isEHRConnected)
                                //{
                                //    SetPanelVisiblityEHRwise("Location", false);
                                //}
                                //else
                                //{
                                //SetPanelVisiblityEHRwise("Eaglesoft");
                                //}
                                //PostEHRConnectionLog("Eaglesoft", isEHRConnected); 
                                #endregion
                                InitbwIsEagleSoftConnected();
                            }
                            else
                            {
                                this.Hide();
                                frmConfiguration fECon = new frmConfiguration("Insert");
                                var result = fECon.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    System.Environment.Exit(1);
                                }
                                ShowHideLoader(false);
                                //btnChatOurSupportTeam.Visible = true;
                                lblKBArticle.Visible = true;
                            }
                        }
                        break;

                    case 2://OpenDental
                        {
                            if (cmbEHRVersion.Text.ToLower() == "15.4".ToLower() || cmbEHRVersion.Text.ToLower() == "17.2+".ToLower())
                            {
                                isCalledFromSave = false;
                                btnOpenDentalSaveClick();
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                            ShowHideLoader(false);
                        }
                        break;

                    case 3://Dentrix
                        {
                            ShowHideLoader(true);

                            if (cmbEHRVersion.Text.ToLower() == "DTX G5".ToLower() || cmbEHRVersion.Text.ToLower() == "DTX G6".ToLower() || cmbEHRVersion.Text.ToLower() == "DTX G6.2+".ToLower())
                            {
                                //SetPanelVisiblityEHRwise("Dentrix", isConnected: false);
                                //btnDentrixSave.PerformClick();
                                isCalledFromSave = false;
                                btnDentrixSaveClick();
                            }
                            else
                            {
                                this.Hide();
                                frmConfiguration fECon = new frmConfiguration("Insert");
                                var result = fECon.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    System.Environment.Exit(1);
                                }
                            }
                        }
                        break;

                    case 4://SoftDent
                        {
                            if (cmbEHRVersion.Text.ToLower() == "17.0.0+".ToLower())
                            {
                                isCalledFromSave = false;
                                SaveSoftDentConfigurtion();
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                        }
                        break;
                    case 5://ClearDent
                        {
                            if (cmbEHRVersion.Text.ToLower() == "9.8+".ToLower() || cmbEHRVersion.Text.ToLower() == "9.10+".ToLower())
                            {
                                isCalledFromSave = false;
                                btnClearDentSaveClick();
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                        }
                        break;
                    case 6://Tracker
                        {
                            if (cmbEHRVersion.Text.ToLower() == "11.29".ToLower())
                            {
                                isCalledFromSave = false;
                                btnTrackerSaveClick();
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                        }
                        break;
                    case 7://PracticeWork
                        {
                            if (cmbEHRVersion.Text.ToLower() == "7.9+".ToLower())
                            {
                                isCalledFromSave = false;
                                btnPracticeWorkSaveClick();
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                        }
                        break;
                    case 8: //Easy Dental
                        {
                            if (cmbEHRVersion.Text.ToLower() == "11.1".ToLower())
                            {
                                Utility.DBConnString = "DSN=EZD2011;DBQ=.;SERVER=NotTheServer;";
                                SetPanelVisiblityEHRwise("Location", false);
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                            ShowHideLoader(false);
                        }
                        break;
                    case 9://OpenDental
                        {
                            if (cmbEHRVersion.Text.ToLower() == "15.4".ToLower() || cmbEHRVersion.Text.ToLower() == "17.2+".ToLower())
                            {
                                SetPanelVisiblityEHRwise("OpenDental");
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                        }
                        break;
                    case 10://PracticeWeb
                        {
                            if (cmbEHRVersion.Text.ToLower() == "21.1".ToLower())
                            {
                                isCalledFromSave = false;
                                btnPracticeWebSaveClick();
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                            ShowHideLoader(false);
                        }
                        break;
                    case 11://AbelDent
                        {
                            ShowHideLoader(true);
                            btnChatOurSupportTeam.Visible = false;
                            lblKBArticle.Visible = false;
                            if (cmbEHRVersion.Text.ToLower() == "14.4.2" || cmbEHRVersion.Text.ToLower() == "14.8.2")
                            {
                                isCalledFromSave = false;
                                btnAbeldentSaveClick();
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                        }
                        break;
                    case 12://CrystalPM
                        {
                            if (cmbEHRVersion.Text.ToLower() == "6.1.9".ToLower())
                            {
                                isCalledFromSave = false;
                                btnCrystalPMSaveClick();
                            }
                            else
                            {
                                ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                cmbEHRName.Focus();
                            }
                            ShowHideLoader(false);
                        }
                        break;
                    case 13://OfficeMate
                        {
                            try
                            {
                                if (cmbEHRVersion.Text.ToLower() == "12.0.2".ToLower() || cmbEHRVersion.Text.ToLower() == "15".ToLower())
                                {
                                    isCalledFromSave = false;
                                    btnOfficeMateSaveClick();
                                }
                                else
                                {
                                    ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                                    cmbEHRName.Focus();
                                }
                                ShowHideLoader(false);
                            }
                            catch (Exception)
                            {

                            }                            
                        }
                        break;
                    default:
                        ShowWriteError("We're sorry. But it seems there isn't an EHR installed on this computer.If you find this in error please give us a call at(832) 225 - 8865.", "Pozative Service integration", show: true);
                        cmbEHRName.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("SetEHRConfigurations " + ex.Message, "Pozative Service", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: "Pozative Service" + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void btnEHRCancel_Click(object sender, EventArgs e)
        {
            try
            {
                CommonFunction.KillSpeechExe();
                SetPanelVisiblityEHRwise("Main", false);
            }
            catch (Exception ex)
            {
                ShowWriteError("btnEHRCancel_Click " + ex.Message, "Pozative Service", show: true, write: true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }
        public bool IsEagleSoftConnected(bool isOnLoad = true, string is_primary = "")
        {
            bool isConnected = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string EaglesoftConStr = "";

                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = false;
                string DPath = selectedEHRVersion == "23.00".ToLower() ? Application.StartupPath + "\\ES_Helper" : Application.StartupPath;
                myProcess.StartInfo.FileName = DPath + "\\PTIAuthenticator.exe";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.Verb = "runas";
                is_primary = string.IsNullOrEmpty(is_primary) ? "true" : is_primary;
                if (selectedEHRVersion == "21.20".ToLower() || selectedEHRVersion == "22.00".ToLower())
                {
                    myProcess.StartInfo.Arguments = is_primary + " false S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e true " + selectedEHRVersion;
                }
                else if (selectedEHRVersion == "23.00".ToLower())
                {
                    myProcess.StartInfo.Arguments = is_primary + " false S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e false " + selectedEHRVersion + " " + @"""" + Application.StartupPath + @"""";
                    CheckDSNDriver();
                }
                else
                {
                    myProcess.StartInfo.Arguments = is_primary + " true S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e false " + selectedEHRVersion;
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
                        if (selectedEHRVersion != "21.20".ToLower())
                        {
                            EaglesoftConStr = Utility.DecryptString(ConnectionString);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!isOnLoad)
                        ShowWriteError("SetEHRConfigurations_  Eaglesoft connection file decrypt" + ex.Message, "Pozative Service", write: true, SaveEHRErroLogMsg: "Eaglesoft connection file decrypt" + ex.Message);
                    else
                        ShowWriteError("Eaglesoft connection file decrypt issue." + ex.Message, show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: "Eaglesoft connection file decrypt" + ex.Message);
                }

                if (string.IsNullOrEmpty(is_primary))
                {
                    Utility.APISessionToken = "";
                    Utility.EHRHostname = "";
                    Utility.EHRIntegrationKey = "";
                    Utility.EHRUserId = "";
                    Utility.EHRPassword = "";
                    Utility.EHRDatabase = "Primary Database";
                    Utility.EHRPort = string.Empty;
                }

                Utility.DBConnString = EaglesoftConStr;
                isConnected = CreateDSN();
                if (isConnected && !isOnLoad)
                {
                    string[] db = Utility.DBConnString.Split(';');
                    Utility.EHRHostname = db[0].Split('=')[1];
                    Utility.EHRUserId = db[2].Split('=')[1];
                    Utility.EHRPassword = db[3].Split('=')[1];
                    Utility.EHRDatabase = db[1].Split('=')[1];
                    Utility.EHRPort = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("IsEagleSoftConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", mailmsg: "EagleSoft connection issue - " + ex.Message, SaveEHRErroLogMsg: "IsEagleSoftConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return isConnected;
        }
        private bool IsOpenDentalConnected(bool isOnLoad = true)
        {
            bool IsEHRConnected = false;
            try
            {
                Utility.EHRUserId = txtOpenDentalUserId.Text;
                Utility.EHRPassword = txtOpenDentalPassword.Text;
                Utility.EHRDatabase = txtOpenDentalDatabase.Text;
                Utility.EHRHostname = txtOpenDentalHostName.Text;

                Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=3306;database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";
                IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);
            }
            catch (Exception ex)
            {
                ShowWriteError("IsOpenDentalConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", mailmsg: "OpenDental connection issue - " + ex.Message, SaveEHRErroLogMsg: "IsOpenDentalConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return IsEHRConnected;
        }
        private bool IsDentrixConnected(bool isOnLoad = true)
        {
            bool isConnected = false;
            try
            {

                if ((selectedEHRVersion == "DTX G5".ToLower()) || (selectedEHRVersion == "DTX G5.1".ToLower())
                    || (selectedEHRVersion == "DTX G5.2".ToLower()) || (selectedEHRVersion == "DTX G6".ToLower())
                    || (selectedEHRVersion == "DTX G6.1".ToLower()))
                {
                    Utility.DBConnString = "host=" + Utility.EHRHostname + ";UID=" + Utility.EHRUserId + ";PWD=" + Utility.EHRPassword + ";Database=DentrixSQL;DSN=c-treeACE ODBC Driver;port=" + Utility.EHRPort + string.Empty;
                    isConnected = SystemBAL.GetEHRDentrixConnection();
                    if (!isConnected && !isOnLoad)
                    {
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;
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
                        ShowWriteError("EHR User ddxdesktopspc.pfx Not Intalled. Please Aplication start with run as administrator. " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError");
                    }
                    string exePath = string.Empty;
                    StringBuilder path = new StringBuilder(512);
                    string connectionString = string.Empty;
                    if (CommonFunction.GetDentrixG62ExePath(path) == SUCCESS)
                    {
                        exePath = path.ToString();
                        CommonFunction.GetDentrixG62ConnectionString();
                        isConnected = SystemBAL.GetEHRDentrixConnection();
                        if (!isConnected)
                        {
                            GetDentrixConnectionString(exePath);
                            isConnected = SystemBAL.GetEHRDentrixConnection();
                        }
                    }
                    else
                    {
                        exePath = path.ToString();
                        CommonFunction.GetDentrixG62ConnectionString();
                        isConnected = SystemBAL.GetEHRDentrixConnection();
                        if (!isConnected)
                        {
                            GetDentrixConnectionString(exePath);
                            isConnected = SystemBAL.GetEHRDentrixConnection();
                        }
                    }
                    ShowWriteError(exePath, write: true);
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("IsDentrixConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "", mailmsg: "Dentrix connection issue - " + ex.Message, SaveEHRErroLogMsg: "IsDentrixConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return isConnected;
        }
        private bool IsSoftDentConnected(bool isOnLoad = true)
        {
            bool isConnected = false;
            try
            {
                Utility.Application_Version = cmbEHRVersion.Text.Trim();
                if (cmbEHRVersion.Text.ToLower() == "17.0.0+")
                {

                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRPort = string.Empty;

                    isConnected = OpenSoftDentConnection();

                    if (!isConnected && !isOnLoad)
                    {
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;

                        ShowWriteError("SoftDent is not connecting. " + "\n" + " Please try again Later or Run Practicework Server exe.", write: true, forEmail: true, emailType: "EHRConnectedError");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("IsSoftDentConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", mailmsg: "SoftDent connection issue - " + ex.Message, SaveEHRErroLogMsg: "IsSoftDentConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return isConnected;
        }
        private bool IsClearDentConnected(bool isOnLoad = true)
        {
            bool isConnected = false;
            try
            {
                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                string ClearDentPath = "";
                string appKey = @"SOFTWARE\Prococious Technology Inc.\ClearDent";
                bool isClearDentInstalled = false;
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                {
                    if (registryKey != null)
                    {
                        object obj = registryKey.GetValue("ApplicationPath");
                        ClearDentPath = obj == null ? "" : registryKey.GetValue("ApplicationPath").ToString();
                    }
                }
                if (ClearDentPath != "")
                {
                    isClearDentInstalled = File.Exists(ClearDentPath + "\\ClearDent.exe");
                    if (isClearDentInstalled)
                    {
                        string configFilePath = ClearDentPath + "\\ClearDent.exe.config";
                        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                        fileMap.ExeConfigFilename = configFilePath;
                        Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                        Utility.DBConnString = config.AppSettings.Settings["ClearDent.SQLConnection"].Value;
                    }
                }
                if (isClearDentInstalled)
                    isConnected = SystemBAL.GetEHRClearDentConnection();
                if (!isConnected && !isOnLoad)
                {
                    Utility.DBConnString = "";
                    Utility.APISessionToken = string.Empty;
                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = string.Empty;
                }
                else if (!isOnLoad)
                {
                    string[] dbDetails = Utility.DBConnString.Split(';');
                    Utility.EHRHostname = dbDetails[0].Split('=')[1];
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = dbDetails[4].Split('=')[1];
                    Utility.EHRPassword = dbDetails[5].Split('=')[1];
                    Utility.EHRDatabase = dbDetails[1].Split('=')[1];
                    Utility.EHRPort = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("IsClearDentConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", mailmsg: "ClearDent connection issue -" + ex.Message, SaveEHRErroLogMsg: "IsClearDentConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return isConnected;
        }
        private bool IsTrackerConnected(bool isOnLoad = true)
        {
            bool isConnected = false;
            try
            {
                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (cmbEHRVersion.Text.ToLower() == "11.29")
                {
                    if (IsAppInstalled("Tracker Server"))
                    {
                        if (EHRRegInstallationPath != "")
                        {
                            string configFilePath = EHRRegInstallationPath + "\\Settings.config";
                            DataSet ds = new DataSet();
                            ds.ReadXml(configFilePath);
                            Utility.EHRHostname = ds.Tables["Database"].Rows[0]["ServerName"].ToString();
                        }
                    }
                    else
                    {
                        string appKey = @"SOFTWARE\The Bridge Network\Tracker Setup\Client";
                        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                        {
                            if (registryKey != null)
                            {
                                object obj = registryKey.GetValue("Server Path");
                                EHRRegInstallationPath = obj.ToString();
                                string configFilePath = EHRRegInstallationPath + "\\Settings.config";
                                DataSet ds = new DataSet();
                                ds.ReadXml(configFilePath);
                                Utility.EHRHostname = ds.Tables["Database"].Rows[0]["ServerName"].ToString();
                            }
                        }
                    }

                    Utility.EHRUserId = txtTrackerUserId.Text;
                    Utility.EHRPassword = txtTrackerPassword.Text;
                    Utility.EHRDatabase = txtTrackerDatabase.Text;
                    Utility.EHRPort = string.Empty;
                    Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                    isConnected = CheckTrackerConnection();
                    if (!isConnected && !isOnLoad)
                    {
                        Utility.DBConnString = "";
                        Utility.APISessionToken = string.Empty;
                        Utility.EHRHostname = string.Empty;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = string.Empty;
                        Utility.EHRPassword = string.Empty;
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("IsTrackerConnected " + ex.Message, "", write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", SaveEHRErroLogMsg: "IsTrackerConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return isConnected;
        }
        private bool IsPracticeWorkConnected(bool isOnLoad = true)
        {
            bool isConnected = false;
            try
            {
                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (cmbEHRVersion.Text.ToLower() == "7.9+".ToLower())
                {
                    Utility.EHRHostname = txtPracticeworkHost.Text.ToLower().Trim();
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = txtPracticeworkUserId.Text.Trim();
                    Utility.EHRPassword = txtPracticeworkPassword.Text.Trim();
                    if (txtPracticeworkDbName.Text.Trim() != "PW")
                        txtPracticeworkDbName.Text = "PW";
                    Utility.EHRDatabase = txtPracticeworkDbName.Text.Trim();
                    Utility.EHRPort = string.Empty;
                    Utility.DBConnString = "Driver={Pervasive ODBC Client Interface};ServerName=" + Utility.EHRHostname + ";dbq=" + Utility.EHRDatabase + ";UID=" + Utility.EHRUserId + ";PWD=" + Utility.EHRPassword + ";";
                    ShowWriteInSyncLog("Practise Work 1st db connection string", write: true);
                    isConnected = SystemBAL.GetPracticeWorkConnection();
                    if (!isConnected)
                    {
                        Utility.DBConnString = "Driver={Pervasive ODBC Engine Interface};ServerName=" + Utility.EHRHostname + ";dbq=" + Utility.EHRDatabase + ";UID=" + Utility.EHRUserId + ";PWD=" + Utility.EHRPassword + ";";
                        ShowWriteInSyncLog("Practise Work 2nd db connection string", write: true);
                        isConnected = SystemBAL.GetPracticeWorkConnection();
                        if (!isConnected && !isOnLoad)
                        {
                            Utility.APISessionToken = string.Empty;
                            Utility.EHRHostname = string.Empty;
                            Utility.EHRIntegrationKey = string.Empty;
                            Utility.EHRUserId = string.Empty;
                            Utility.EHRPassword = string.Empty;
                            Utility.EHRDatabase = string.Empty;
                            Utility.EHRPort = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("IsPracticeWorkConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", mailmsg: "PracticeWork connection issue - " + ex.Message, SaveEHRErroLogMsg: "IsPracticeWorkConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return isConnected;
        }
        private bool IsPracticeWebConnected(bool isOnLoad = true, string version = "")
        {
            bool isConnected = false;
            try
            {
                Utility.Application_Version = cmbEHRVersion.Items.Count > 0 ? cmbEHRVersion.Text.Trim() : version;
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
                isConnected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);

                if (!isConnected && !isOnLoad)
                {
                    Utility.DBConnString = "";
                    Utility.APISessionToken = string.Empty;
                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = string.Empty;

                    ShowWriteError("PracticeWeb is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", write: true, forEmail: true, emailType: "EHRConnectedError");
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("IsPracticeWebConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", mailmsg: "PracitceWeb connection issue - " + ex.Message, SaveEHRErroLogMsg: "IsPracticeWebConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return isConnected;
        }
        private bool IsAbelDentConnected(bool isOnLoad = true)
        {
            bool isConnected = false;
            try
            {
                try
                {
                    if (isOnLoad)
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
                        if (!string.IsNullOrEmpty(Utility.EHRHostname) && !string.IsNullOrEmpty(Utility.EHRDatabase))
                        {
                            appKey = @"SOFTWARE\WOW6432Node\ABELSoft\ABELDent";
                            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(appKey))
                            {
                                if (registryKey != null)
                                {
                                    Utility.EHRHostname = registryKey.GetValue("SQLServer").ToString();
                                    Utility.EHRDatabase = registryKey.GetValue("Database").ToString();
                                }
                            }
                        }
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRUserId = "";
                        Utility.EHRPassword = "";
                    }
                    else
                    {
                        Utility.EHRHostname = AbelDentHostName;
                        Utility.EHRIntegrationKey = string.Empty;
                        Utility.EHRDatabase = AbelDentDatabase;
                        Utility.EHRUserId = AbelDentUserId;
                        Utility.EHRPassword = AbelDentPassword;
                    }

                    Utility.EHRPort = "";
                    int retryCnt = 0;
                    RetryAbelDentConnection:
                    if (Utility.EHRUserId == string.Empty || Utility.EHRPassword == string.Empty)
                    {
                        Utility.DBConnString = "Server=" + Utility.EHRHostname + ";Database=" + Utility.EHRDatabase + ";Trusted_Connection=True;";
                    }
                    else
                    {
                        Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                    }
                    //ShowWriteError("auto get connectionstring is :- " + Utility.DBConnString.ToString(), write: true);
                    isConnected = SystemBAL.GetAbelDentConnection();

                    if (!isConnected && Utility.EHRDatabase != AbelDentDatabase && retryCnt == 0)
                    {
                        retryCnt = 1;
                        Utility.EHRDatabase = AbelDentDatabase;
                        goto RetryAbelDentConnection;
                    }
                }
                catch (Exception ex)
                {
                    if (!isOnLoad)
                        ShowWriteError("AbelDent is not connecting. " + "\n" + " Please enter valid credentials." + ex.Message, "CheckAbelDentConnection", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: "CheckAbelDentConnection" + ex.Message);
                    else
                        ShowWriteError("IsAbelDentConnected" + ex.Message, emailType: "EHRConnectedError", forEmail: !isOnLoad, write: true, SaveEHRErroLogMsg: "CheckAbelDentConnection" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return isConnected;
        }
        private bool IsCrystalPMConnected(bool isOnLoad = true)
        {
            bool IsEHRConnected = false;
            try
            {
                Utility.EHRUserId = txtCrystalPMUserId.Text;
                Utility.EHRPassword = txtCrystalPMPassword.Text;
                Utility.EHRDatabase = txtCrystalPMDatabase.Text;
                Utility.EHRHostname = txtCrystalPMHostName.Text;
                Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=3306;database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";";//Allow Zero Datetime=true;
                AditCrystalPM.BAL.MySqlDB.ConnString = Utility.DBConnString;
                AditCrystalPM.BAL.MySqlDB mySql = new AditCrystalPM.BAL.MySqlDB();
                IsEHRConnected = mySql.CheckConnection(Utility.DBConnString);
                //mySql.Dispose();
            }
            catch (Exception ex)
            {
                ShowWriteError("IsCrystalPMConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", mailmsg: "CrystalPM connection issue - " + ex.Message, SaveEHRErroLogMsg: "IsCrystalPMConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return IsEHRConnected;
        }
        private bool IsOfficeMateConnected(bool isOnLoad = true)
        {

            bool IsEHRConnected = false;
            try
            {
                Utility.EHRHostname = txtOfficeMateHostName.Text;
                Utility.EHRIntegrationKey = string.Empty;
                Utility.EHRDatabase = txtOfficeMateDatabase.Text;
                Utility.EHRUserId = txtOfficeMateUserId.Text;
                Utility.EHRPassword = txtOfficeMatePassword.Text;

                if (Utility.EHRUserId == string.Empty || Utility.EHRPassword == string.Empty)
                {
                    Utility.DBConnString = "Server=" + Utility.EHRHostname + ";Database=" + Utility.EHRDatabase + ";Trusted_Connection=True;";
                }
                else
                {
                    Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                }
                
                IsEHRConnected = AditOfficeMate.BAL.SqlDB.Instance.CheckConnection(Utility.DBConnString);                
            }
            catch (Exception ex)
            {
                ShowWriteError("IsOfficeMateConnected " + ex.Message, write: true, forEmail: !isOnLoad, emailType: "EHRConnectedError", mailmsg: "OfficeMate connection issue - " + ex.Message, SaveEHRErroLogMsg: "IsOfficeMateConnected " + ex.Message);
                return false;
            }
            finally
            {
                CommonFunction.KillSpeechExe();
            }
            return IsEHRConnected;
        }

        #endregion

        //#region CrystalPM Configuration

        private void btnCrystalPMSave_Click(object sender, EventArgs e)
        {
            isCalledFromSave = true;
            btnCrystalPMSaveClick();
        }
        private void btnCrystalPMSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (isCalledFromSave)
                {

                    if (string.IsNullOrEmpty(txtCrystalPMHostName.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid CrystalPM Hostname", "Hostname", show: true);
                        txtCrystalPMHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtCrystalPMDatabase.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid CrystalPM Database Name", "Port", show: true);
                        txtCrystalPMDatabase.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtCrystalPMUserId.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid CrystalPM User Id", "IntegrationKey", show: true);
                        txtCrystalPMUserId.Focus();
                        return;
                    }
                }

                int cnt = 0;
                CheckODMySQLConn:

                Utility.EHRHostname = txtCrystalPMHostName.Text.ToLower().Trim();
                Utility.EHRIntegrationKey = string.Empty;
                Utility.EHRDatabase = txtCrystalPMDatabase.Text.Trim();
                Utility.EHRUserId = txtCrystalPMUserId.Text.Trim();
                Utility.EHRPassword = txtCrystalPMPassword.Text.Trim();
                Utility.EHRDocPath = string.Empty;
                Utility.EHRPort = "3306";
                Utility.DontAskPasswordOnSaveSetting = false;
                Utility.NotAllowToChangeSystemDateFormat = false;

                if (cnt == 1)
                    Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";SslMode=none;"; //;Allow Zero Datetime=true
                else
                    Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";"; //Allow Zero Datetime=true;
                AditCrystalPM.BAL.MySqlDB.ConnString = Utility.DBConnString;
                bool IsEHRConnected = AditCrystalPM.BAL.MySqlDB.Instance.CheckConnection(Utility.DBConnString);
                if (IsEHRConnected)
                {
                    SetMultiClinicForEHR("CrystalPM");
                    SetPanelVisiblityEHRwise("SharedPath", false);
                    //SetPanelVisiblityEHRwise("Location", false);
                }
                else
                {
                    if (Utility.ODsqlConnErrorMsg.ToLower().Contains("does not support ssl connections"))
                    {
                        cnt = 1;
                        Utility.ODsqlConnErrorMsg = string.Empty;
                        goto CheckODMySQLConn;
                    }

                    Utility.DBConnString = "";
                    Utility.APISessionToken = string.Empty;
                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = string.Empty;

                    SetPanelVisiblityEHRwise("CrystalPM");

                    ShowWriteError("CrystalPM is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: isCalledFromSave, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: " CrystalPM is not connecting. Please enter valid credentials.");
                }
                //PostEHRConnectionLog("Open Dental-" + Utility.DBConnString, IsEHRConnected);
                PostEHRConnectionLog("CrystalPM-", IsEHRConnected);
            }
            catch (Exception ex)
            {
                ShowWriteError("btnCrystalPMSaveclick " + ex.Message, "Authentication", show: isCalledFromSave, write: true, SaveEHRErroLogMsg: ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                CommonFunction.KillSpeechExe();
            }
        }
        //#endregion

        #region OfficeMate Integration

        private void btnOfficeMateSave_Click(object sender, EventArgs e)
        {
            isCalledFromSave = true;
            btnOfficeMateSaveClick();
        }
        private void btnOfficeMateSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (isCalledFromSave)
                {
                    if (string.IsNullOrEmpty(cmbSqlServiceName.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid OfficeMate SQL Service Name", "SQL Service", show: true);
                        cmbSqlServiceName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtOfficeMateHostName.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid OfficeMate ServerName", "ServerName", show: true);
                        txtOfficeMateHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtOfficeMateDatabase.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid OfficeMate Database Name", "Database", show: true);
                        txtTrackerDatabase.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtOfficeMateUserId.Text.Trim()))
                    {
                        txtOfficeMateUserId.Text = string.Empty;
                    }
                    if (string.IsNullOrEmpty(txtOfficeMatePassword.Text.Trim()))
                    {
                        txtOfficeMatePassword.Text = string.Empty;
                    }
                }
               
                Utility.EHRHostname = txtOfficeMateHostName.Text.ToLower().Trim();
                Utility.EHRIntegrationKey = string.Empty;
                Utility.EHRDatabase = txtOfficeMateDatabase.Text.Trim();
                Utility.EHRUserId = txtOfficeMateUserId.Text.Trim();
                Utility.EHRPassword = txtOfficeMatePassword.Text.Trim();
                Utility.EHRDocPath = string.Empty;
                Utility.DontAskPasswordOnSaveSetting = false;
                Utility.NotAllowToChangeSystemDateFormat = false;


                string serverName = "";
                string dbName = "";

                AditOfficeMate.BAL.SqlDB.Instance.GetOfficeMateConnectionString(ref serverName, ref dbName);
                Utility.DBConnString = "Server=" + serverName + ";Database=" + dbName + ";Trusted_Connection=True;Connect Timeout=300";               
                bool IsEHRConnected = AditOfficeMate.BAL.SqlDB.Instance.CheckConnection(Utility.DBConnString);

                if (IsEHRConnected)
                {
                    SetMultiClinicForEHR("OfficeMate");
                    SetPanelVisiblityEHRwise("SharedPath", false);
                }
                else
                {
                    ShowWriteError("OfficeMate not connecting automatically.", write: true, forEmail: true, emailType: "EHRConnectedError");
                    //if (!IsEHRConnected)
                    //{
                    //    cnt = 1;
                    //    goto CheckSQLConn;
                    //}

                    Utility.DBConnString = "";
                    Utility.APISessionToken = string.Empty;
                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = string.Empty;

                    SetPanelVisiblityEHRwise("OfficeMate");

                    ShowWriteError("OfficeMate is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: isCalledFromSave, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: " OfficeMate is not connecting. Please enter valid credentials.");
                }
                PostEHRConnectionLog("OfficeMate", IsEHRConnected);
            }
            catch (Exception ex)
            {
                ShowHideLoader(false);
                ShowWriteError("btnOfficeMateSave_Click " + ex.Message, "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: ex.Message);
            }
            finally
            {
                CommonFunction.KillSpeechExe();
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
                    ShowWriteError("Please Enter valid Eaglesoft ServerName", "Hostname", show: true);
                    txtEaglesoftHostName.Focus();
                    return;
                }

                if (txtEaglesoftUserId.Visible && string.IsNullOrEmpty(txtEaglesoftUserId.Text.Trim()))
                {
                    ShowWriteError("Please Enter valid Eaglesoft User Id", "UserId", show: true);
                    txtEaglesoftUserId.Focus();
                    return;
                }
                if (txtEaglesoftPassword.Visible && string.IsNullOrEmpty(txtEaglesoftPassword.Text.Trim()))
                {
                    ShowWriteError("Please Enter valid Eaglesoft Password", "Password", show: true);
                    txtEaglesoftPassword.Focus();
                    return;
                }

                if (!txtEaglesoftUserId.Visible && txtEaglesoftHostName.Visible)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string EaglesoftConStr = "";
                    Assembly.LoadFile(Application.StartupPath + "\\Patterson.PTCBaseObjects.SharedObjects.dll");
                    var DLL = Assembly.LoadFile(Application.StartupPath + "\\EaglesoftSettings.dll");

                    foreach (Type type in DLL.GetExportedTypes())
                    {
                        if (type.Name == "EaglesoftSettings")
                        {
                            dynamic settings = Activator.CreateInstance(type);
                            EaglesoftConStr = settings.GetLegacyConnectionString(true);
                        }
                    }

                    #region Create DSN For EagleSoft
                    try
                    {
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
                        {
                            PostEHRConnectionLog("Eaglesoft", true);
                            SetPanelVisiblityEHRwise("Location", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowWriteError("btnEagleSoftSave " + ex.Message, "Pozative Service", show: true, write: true, SaveEHRErroLogMsg: ex.Message);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("btnEagleSoftSave " + ex.Message, "Pozative Service", show: true, write: true, SaveEHRErroLogMsg: ex.Message);
            }
            finally
            {
                CommonFunction.KillSpeechExe();
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
                ShowWriteError("btnEagleSoftBack_Click " + ex.Message, "Pozative Service", show: true, write: true);
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
            isCalledFromSave = true;
            btnOpenDentalSaveClick();
        }
        private void btnOpenDentalSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (isCalledFromSave)
                {

                    if (string.IsNullOrEmpty(txtDentrixHostName.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid OpenDental Hostname", "Hostname", show: true);
                        txtOpenDentalHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtOpenDentalDatabase.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid OpenDental Database Name", "Port", show: true);
                        txtOpenDentalDatabase.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtDentrixUserId.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid OpenDental User Id", "IntegrationKey", show: true);
                        txtOpenDentalUserId.Focus();
                        return;
                    }
                }

                int cnt = 0;
                CheckODMySQLConn:

                Utility.EHRHostname = txtOpenDentalHostName.Text.ToLower().Trim();
                Utility.EHRIntegrationKey = string.Empty;
                Utility.EHRDatabase = txtOpenDentalDatabase.Text.Trim();
                Utility.EHRUserId = txtOpenDentalUserId.Text.Trim();
                Utility.EHRPassword = txtOpenDentalPassword.Text.Trim();
                Utility.EHRDocPath = string.Empty;
                Utility.EHRPort = "3306";
                Utility.DontAskPasswordOnSaveSetting = false;
                Utility.NotAllowToChangeSystemDateFormat = false;

                if (cnt == 1)
                    Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;SslMode=none;";
                else
                    Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";
                bool IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);

                if (IsEHRConnected)
                {
                    SetMultiClinicForEHR("OpenDental");
                    SetPanelVisiblityEHRwise("Location", false);
                }
                else
                {
                    if (Utility.ODsqlConnErrorMsg.ToLower().Contains("does not support ssl connections"))
                    {
                        cnt = 1;
                        Utility.ODsqlConnErrorMsg = string.Empty;
                        goto CheckODMySQLConn;
                    }

                    Utility.DBConnString = "";
                    Utility.APISessionToken = string.Empty;
                    Utility.EHRHostname = string.Empty;
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = string.Empty;
                    Utility.EHRPassword = string.Empty;
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = string.Empty;

                    SetPanelVisiblityEHRwise("OpenDental");

                    ShowWriteError("OpenDental is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: isCalledFromSave, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: " OpenDental is not connecting. Please enter valid credentials.");
                }
                //PostEHRConnectionLog("Open Dental-" + Utility.DBConnString, IsEHRConnected);
                PostEHRConnectionLog("Open Dental-", IsEHRConnected);
            }
            catch (Exception ex)
            {
                ShowWriteError("btnOpenDentalSaveclick " + ex.Message, "Authentication", show: isCalledFromSave, write: true, SaveEHRErroLogMsg: ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                CommonFunction.KillSpeechExe();
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
            isCalledFromSave = true;
            btnDentrixSaveClick();
        }
        private void btnDentrixSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (isCalledFromSave)
                {
                    if (string.IsNullOrEmpty(txtDentrixHostName.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid Dentrix Hostname", "Hostname", show: true);
                        txtDentrixHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtDentrixUserId.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid Dentrix User Id", "IntegrationKey", show: true);
                        txtDentrixUserId.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtDentrixPassword.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid Dentrix Password", "UserId", show: true);
                        txtDentrixPassword.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtDentrixPort.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid Dentrix Port", "Port", show: true);
                        txtDentrixPort.Focus();
                        return;
                    }
                }
                #region code divided in BackGroundWorkers
                //bool IsEHRConnected = IsDentrixConnected(false);
                //if (IsEHRConnected)
                //{
                //    MessageBox.Show("before start pdf connection finding");
                //    GoalBase.GetConnectionStringforDoc(true);
                //    MessageBox.Show("After pdf connection finding");
                //    SetPanelVisiblityEHRwise("Location", false);
                //}
                //else
                //{
                //    SetPanelVisiblityEHRwise("Dentrix");
                //    configErrors += "\n Dentrix not connected.";
                //    if (isCalledFromSave)
                //        ObjGoalBase.ErrorMsgBox("Authentication", "Dentrix is not connecting. " + "\n" + " Please enter valid credentials.");
                //}
                //PostEHRConnectionLog("Dentrix-" + Utility.DBConnString, IsEHRConnected);
                #endregion

                InitbwIsDentrixConnected();
            }
            catch (Exception ex)
            {
                ShowWriteError("btnDentrixSave_click " + ex.Message, "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: ex.Message);
                SetPanelVisiblityEHRwise("Main", false);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        BackgroundWorker bwIsDentrixConnected = null;
        private void InitbwIsDentrixConnected()
        {
            try
            {
                Utility.Application_Version = cmbEHRVersion.Text.Trim();
                Utility.EHRHostname = txtDentrixHostName.Text.ToLower().Trim();
                Utility.EHRIntegrationKey = string.Empty;
                Utility.EHRUserId = txtDentrixUserId.Text.Trim();
                Utility.EHRPassword = txtDentrixPassword.Text.Trim();
                Utility.EHRDatabase = string.Empty;
                Utility.EHRPort = txtDentrixPort.Text.Trim();
                Utility.NotAllowToChangeSystemDateFormat = picNotAllowToChangeSystemDateFormat.Tag.ToString() == "ON" ? true : false;

                bwIsDentrixConnected = new BackgroundWorker();
                bwIsDentrixConnected.DoWork += BwIsDentrixConnected_DoWork;
                bwIsDentrixConnected.RunWorkerCompleted += BwIsDentrixConnected_RunWorkerCompleted;
                bwIsDentrixConnected.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ShowWriteError("InitbwIsDentrixConnected " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError");
            }
        }

        private void BwIsDentrixConnected_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((bool)e.Result)
                {
                    InitbwDentrixDocConnection();
                }
                else
                {
                    CommonFunction.KillSpeechExe();
                    ShowHideLoader(false);
                    //btnChatOurSupportTeam.Visible = true;
                    lblKBArticle.Visible = true;
                    SetPanelVisiblityEHRwise("Dentrix");
                    if (isCalledFromSave)
                        ShowWriteError("Dentrix is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: true, write: true, emailType: "", forEmail: true);
                    else
                        ShowWriteError("Dentrix not connecting automatically.", write: true, forEmail: true, emailType: "EHRConnectedError");
                }
                //PostEHRConnectionLog("Dentrix-" + Utility.DBConnString, (bool)e.Result);
                PostEHRConnectionLog("Dentrix-", (bool)e.Result);
            }
            catch (Exception ex)
            {
                ShowWriteError("BwIsDentrixConnected_RunWorkerCompleted " + ex.Message, "Dentrix Connection", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
            }
        }

        private void BwIsDentrixConnected_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = IsDentrixConnected(false);
        }
        BackgroundWorker bwDentrixDocConnection = null;
        private void InitbwDentrixDocConnection()
        {
            try
            {
                bwDentrixDocConnection = new BackgroundWorker();
                bwDentrixDocConnection.DoWork += BwDentrixDocConnection_DoWork;
                bwDentrixDocConnection.RunWorkerCompleted += BwDentrixDocConnection_RunWorkerCompleted;
                bwDentrixDocConnection.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ShowWriteError("InitbwDentrixDocConnection " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "Dentrix Document connection issue - " + ex.Message);
            }
        }

        private void BwDentrixDocConnection_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                CommonFunction.KillSpeechExe();
                ShowHideLoader(false);
                //btnChatOurSupportTeam.Visible = true;
                lblKBArticle.Visible = true;
                SetPanelVisiblityEHRwise("Location", false);
            }
            catch (Exception ex)
            {
                ShowWriteError("BwDentrixDocConnection_RunWorkerCompleted " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "Dentrix document connection issue - " + ex.Message);
            }
        }

        private void BwDentrixDocConnection_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                GoalBase.GetConnectionStringforDoc(true);
            }
            catch (Exception ex)
            {
                ShowWriteError("BwDentrixDocConnection_DoWork " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "Dentrix document connection get issue - " + ex.Message);
            }
        }
        public string GetDentrixConnectionString(String DentrixPath)
        {
            try
            {
                int cnt = 0;
                bool IsEHRConnected = false;


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
                        DENTRIXAPI_GetConnectionString:
                        if (cnt == 1)
                        {
                            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                            doc.Load(DPath + "\\DTX_Helper.exe.config");
                            System.Xml.XmlNode node = doc.SelectSingleNode("/configuration/startup/supportedRuntime");
                            if (node != null)
                            {
                                node.Attributes["sku"].Value = ".NETFramework,Version=v4.5.1";
                            }
                            doc.Save(DPath + "\\DTX_Helper.exe.config");
                        }

                        #region call Dentrix6.2+
                        Process myProcess = new Process();
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = DPath + "\\DTX_Helper.exe";
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.StartInfo.Verb = "runas";
                        myProcess.Start();
                        myProcess.WaitForExit();
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
                                cnt = 1;
                                goto DENTRIXAPI_GetConnectionString;
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowWriteError("GetDentrixConnectionString - " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "Dentrix connection string getting issue - " + ex.Message);
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
                ShowWriteError("GetDentrixConnectionString - " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "Dentrix connection string getting issue - " + ex.Message);
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
                ShowWriteError("picAditSync_click " + ex.Message, "Pozative Service", write: true, show: true);
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
                ShowWriteError("picPozativeSync_click " + ex.Message, "Pozative Service", show: true, write: true);
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
                btnAdminUserSave.Tag = "1";
                Cursor.Current = Cursors.WaitCursor;

                //#region Adit Sync

                if (string.IsNullOrEmpty(txtAdminUserName.Text.Trim()) && string.IsNullOrEmpty(txtAdminPassword.Text.Trim()))
                {
                    ShowWriteError("Please enter Username and Password, it should not be blank.", "Adit App EmailId", show: true);
                    txtAdminUserName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtAdminUserName.Text.Trim()))
                {
                    ShowWriteError("Please enter Username, it should not be blank.", "Adit App EmailId", show: true);
                    txtAdminUserName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtAdminPassword.Text.Trim()))
                {
                    ShowWriteError("Please enter Password, it should not be blank.", "Adit App Password", show: true);
                    txtAdminPassword.Focus();
                    return;
                }
                if (RBtnSingleClinic.Visible || RBtnMultiClinic.Visible)
                {
                    if (!RBtnSingleClinic.Checked && !RBtnMultiClinic.Checked)
                    {
                        ShowWriteError("Please Select Single Or Multi Clinic Option", "Adit App Clinic", show: true);
                        RBtnSingleClinic.Focus();
                        return;
                    }
                }

                try
                {
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
                        password = txtAdminPassword.Text.Trim(),
                        self_installable = true
                    };
                    var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(AditLoginPost);
                    requestLogin.AddHeader("cache-control", "no-cache");
                    requestLogin.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                    IRestResponse responseLogin = clientLogin.Execute(requestLogin);
                    //ShowWriteError("Adit Login Response : " + responseLogin.Content.ToString(), write: true);

                    if (responseLogin.StatusCode.ToString().ToLower() == "ServiceUnavailable".ToString().ToLower())
                    {
                        ShowWriteError("Service Unavailable.", "Adit Server", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Service unavailable login failed." + responseLogin.ErrorMessage, SaveEHRErroLogMsg: "Adit Server" + "Service Unavailable." + responseLogin.ErrorMessage);
                        return;
                    }

                    if (responseLogin.ErrorMessage != null)
                    {
                        ShowWriteError("There was an issue with connecting to the Adit Server. Please wait a couple minutes and then try again.", "Adit Server", write: true, show: true, forEmail: true, emailType: "\n Server is down login failed." + responseLogin.ErrorMessage, mailmsg: "Server is down login failed." + responseLogin.ErrorMessage
                            , SaveEHRErroLogMsg: "Adit Server" + "There was an issue with connecting to the Adit Server. Please wait a couple minutes and then try again." + responseLogin.ErrorMessage);
                        return;
                    }

                    var AdminLoginDto = JsonConvert.DeserializeObject<AdminLoginDetailBO>(responseLogin.Content);
                    if (AdminLoginDto == null)
                    {
                        ShowWriteError(responseLogin.ErrorMessage.ToString(), "Adit App Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit App user authentication failed." + responseLogin.ErrorMessage, SaveEHRErroLogMsg: "Adit App Admin User Authentication" + responseLogin.ErrorMessage);
                        return;
                    }
                    if (AdminLoginDto.status == "false")
                    {
                        ShowWriteError(AdminLoginDto.message.ToString(), "Adit App Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit App user authentication failed." + AdminLoginDto.message.ToString(), SaveEHRErroLogMsg: "Adit App Admin User Authentication" + AdminLoginDto.message.ToString());
                        return;
                    }
                    else
                    {
                        LoginSuccess(AdminLoginDto);
                    }

                }
                catch (Exception ex)
                {
                    ShowWriteError("There was an issue with connecting to the Adit Server. Please wait a couple minutes and then try again.", "Adit Server", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Server is down login failed." + ex.Message, "GetAdminUserLoginEmailIdPass " + ex.Message);
                    return;
                }
                #region Code moved to LoginSuccess function

                //UserAditLocationLinkList = string.Empty;
                //LocationDetailBO customerDto = null;
                //try
                //{
                //    string strApiLocOrg = SystemBAL.GetApiAditLocationAndOrganizationByAdminIdPassword();
                //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                //    var client = new RestClient(strApiLocOrg);
                //    var request = new RestRequest(Method.POST);
                //    ServicePointManager.Expect100Continue = true;
                //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //    request.AddHeader("Postman-Token", "1d16df4c-48ba-4644-bc7a-9bcef2a86744");
                //    request.AddHeader("cache-control", "no-cache");
                //    AditLoginPostBO AditLocOrgPost = new AditLoginPostBO
                //    {
                //        email = txtAdminUserName.Text.ToLower().Trim(),
                //        password = txtAdminPassword.Text.Trim(),
                //        created_by = Utility.User_ID
                //    };
                //    var javaScriptSerializer1 = new System.Web.Script.Serialization.JavaScriptSerializer();
                //    string jsonString1 = javaScriptSerializer1.Serialize(AditLocOrgPost);
                //    request.AddHeader("cache-control", "no-cache");
                //    request.AddParameter("application/json", jsonString1, ParameterType.RequestBody);
                //    IRestResponse response = client.Execute(request);
                //    if (response.ErrorMessage != null)
                //    {
                //        ShowWriteError(response.ErrorMessage, "Adit App Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit App getting details of location & organization failed." + response.ErrorMessage, "Adit App Admin User Authentication" + response.ErrorMessage);
                //        return;
                //    }

                //    UserAditLocationLinkList = response.Content;
                //    customerDto = JsonConvert.DeserializeObject<LocationDetailBO>(response.Content);
                //    if (!string.IsNullOrEmpty(customerDto.error) && customerDto.data == null)
                //    {

                //        ShowWriteError(customerDto.message, "Adit App Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit App getting details of location & organization failed." + customerDto.message, "Adit App Admin User Authentication" + customerDto.message);
                //        return;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    ShowWriteError(ex.Message, "Adit App Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit App getting details of location & organization failed." + ex.Message, "GetApiAditLocationAndOrganizationByAdminIdPassword " + ex.Message);
                //    return;
                //}

                //dtTempApptLocationTable.Clear();
                //DataRow drApptLocDef = dtTempApptLocationTable.NewRow();
                //drApptLocDef["id"] = "0";
                //drApptLocDef["name"] = " Select ";
                //dtTempApptLocationTable.Rows.Add(drApptLocDef);
                //dtTempApptLocationTable.AcceptChanges();

                //for (int i = 0; i < customerDto.data.Count; i++)
                //{
                //    DataRow drApptLoc = dtTempApptLocationTable.NewRow();
                //    drApptLoc["id"] = customerDto.data[i]._id.ToString();
                //    drApptLoc["name"] = customerDto.data[i].name.ToString().Trim();
                //    drApptLoc["system_mac_address"] = customerDto.data[i].system_mac_address.ToString();
                //    dtTempApptLocationTable.Rows.Add(drApptLoc);
                //    dtTempApptLocationTable.AcceptChanges();
                //}

                //DataView dv = dtTempApptLocationTable.DefaultView;
                //dv.Sort = "name";
                //dtTempApptLocationTable = dv.ToTable();

                //cmbAditLocation.DataSource = dtTempApptLocationTable.Copy();
                //cmbAditLocation.ValueMember = "id";
                //cmbAditLocation.DisplayMember = "name";
                //cmbAditLocation.SelectedValue = "0";
                //cmbAditLocation.DropDownStyle = ComboBoxStyle.DropDownList;
                //Cursor.Current = Cursors.Default;
                //if (cmbEHRName.Items.Count == 2)
                //{
                //    cmbEHRName.SelectedIndex = 1;
                //    SetEHRConfigurations();
                //}
                //else
                //{
                //    CheckEHRConnectionAndVisibility();
                //    SetPanelVisiblityEHRwise("Main", false);
                //}
                //SaveEHRErroLog();
                //#endregion
                //mailData.AditAppLoginStatus = true; 
                #endregion
            }
            catch (Exception ex)
            {
                ShowWriteError("btnAdminUserSave_click " + ex.Message, "Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit user authentication issue." + ex.Message, "btnAdminUserSave_click " + ex.Message);
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

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string StrFirstLocationClinicNumber = string.Empty;
                locationWithClinicLst.Clear();
                locationLst.Clear();
                selectedLocations = string.Empty;
                if (RBtnMultiClinic.Checked)
                {
                    bool BlnChk = false;
                    for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                    {
                        locationLst.Add(null);
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
                                locationWithClinicLst.Add(DGVMuliClinc.Rows[i].Cells["Description"].Value.ToString() + "-" + DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString());
                                locationLst[i] = DGVMuliClinc.Rows[i].Cells["Location"].Value.ToString();
                                selectedLocations += string.IsNullOrEmpty(selectedLocations) ? DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString() : "," + DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString();
                                bool IsExist = DGVMuliClinc.Rows.Cast<DataGridViewRow>()
                                          .Count(c =>
                                          c.Cells["Location"]
                                          .EditedFormattedValue.ToString() == DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString()) > 1;

                                if (IsExist)
                                {
                                    ShowWriteError("Please Select One Location For One Clinic.", "Adit Location Name", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError");
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
                                        ShowWriteError(DGVMuliClinc.Rows[i].Cells["Location"].FormattedValue.ToString() + " Location is already configured with another system.", "Location Name", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError");
                                        return;
                                    }
                                }

                            }
                        }
                        else
                        {
                            ShowWriteError("Please Select Adit Location You Would Like To Sync", "Adit Location Name", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError");
                            DGVMuliClinc.Rows[i].Cells["Location"].Selected = true;
                            return;
                        }
                    }

                    if (!BlnChk)
                    {
                        ShowWriteError("Please Select Atlist One Adit Location You Would Like To Sync", "Adit Location Name", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError");
                        return;
                    }
                }
                else
                {
                    if (cmbAditLocation.SelectedIndex == 0)
                    {
                        ShowWriteError("Please Select Adit Location You Would Like To Sync", "Adit Location Name", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError");
                        cmbAditLocation.Focus();
                        return;
                    }

                    string Pid = string.Empty;

                    DataRow[] PozLocrow = dtTempApptLocationTable.Copy().Select("id = '" + cmbAditLocation.SelectedValue.ToString().Trim() + "' ");
                    if (PozLocrow.Length > 0)
                    {
                        Pid = PozLocrow[0]["system_mac_address"].ToString().Trim();
                    }
                    locationWithClinicLst.Add(cmbAditLocation.Text.ToString());
                    locationLst.Add(cmbAditLocation.Text.ToString());
                    selectedLocations = cmbAditLocation.Text.ToString();
                    if (Pid.ToString().Trim() != string.Empty && Pid.ToString().Trim() != "0")
                    {
                        if (processorID.ToString() != Pid.ToString().Trim())
                        {
                            ShowWriteError(cmbAditLocation.Text.ToString() + " Location is already configured with another system.", "Location Name", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError");
                            return;
                        }
                    }
                }
                CommonFunction.KillSpeechExe();
                SetPanelVisiblityEHRwise("PMSCredential", false);
                #region Moved to FinalSaveAfterLocationConfiguration
                /*
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
                    SaveEHRErroLog(ehrConnected: true, errorMessage: "[GetApiERHListWithWebId] : " + response.ErrorMessage, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst);
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
                        SaveEHRErroLog(ehrConnected: true, errorMessage: "EHR Location " + staLocation_EHR, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst);
                        configErrors += "\n EHR Location issue.";
                        ObjGoalBase.WriteToErrorLogFile("EHR Location " + staLocation_EHR);
                        return;
                    }
                    else
                    {
                        SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
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
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);

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
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
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
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                }
                catch (Exception EX)
                {
                    ObjGoalBase.WriteToErrorLogFile("btnLocationSave_Click AppVersion AutoUpdate ServerData : " + EX.Message);
                    SaveEHRErroLog(ehrConnected: true, errorMessage: "AppVersion AutoUpdate ServerData : " + EX.Message, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
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
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
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
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, organizationID: Utility.Organization_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
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

                //this.DialogResult = DialogResult.OK;
                //this.Close(); 
                */
                #endregion

            }
            catch (Exception EX)
            {
                ShowWriteError("btnLocationSave_Click " + EX.Message, "EHR Location", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError", mailmsg: "Location Issue in - " + EX.Message);
                SaveEHRErroLog(ehrConnected: true, errorMessage: "EHR Location" + EX.Message, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: false);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                isProcessComplete = true;
                if (string.IsNullOrEmpty(mailData.LocationConfigurationError))
                    mailData.LocationConfigurationStatus = true;
            }
        }

        private void FinalSaveAfterLocationConfiguration()
        {
            SetPanelVisiblityEHRwise("ReportGenerated");
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                #region Moved to FinalSaveAfterLocationConfiguration

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
                            Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();

                            if (TimeZone.CurrentTimeZone.StandardName.ToString().ToLower() != tmpTimeZone.ToString().ToLower())
                            {
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
                                    ShowWriteError(responseUpdateWebTimeZone.ErrorMessage, "Update Timezone", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError", mailmsg: "Update timezone issue - " + responseUpdateWebTimeZone.ErrorMessage, "Update Timezone" + responseUpdateWebTimeZone.ErrorMessage);
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowWriteError("FinalSaveAfterLocationConfiguration " + ex.Message, "", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError", mailmsg: "Update timezone issue." + ex.Message, ex.Message);
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
                            if (cmbEHRName.Text == "CrystalPM")
                            {
                                drLoc["Clinic_Number"] = "1";
                            }
                            if (cmbEHRName.Text == "OfficeMate")
                            {
                                drLoc["Clinic_Number"] = "2";
                            }
                            drLoc["Service_Install_Id"] = "1";
                            dtTempLocationTable.Rows.Add(drLoc);
                        }
                    }
                }
                if (RBtnMultiClinic.Checked == true)
                {
                    for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(locationLst[i]))
                        {
                            var resultProvider = customerDto.data.AsEnumerable().Where(o => locationLst[i].ToUpper() == o._id.ToString().ToUpper());

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
                }
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

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);

                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    ShowWriteError("[GetApiERHListWithWebId] : " + response.ErrorMessage, write: true, forEmail: true, emailType: "LocationConfigurationError", mailmsg: "Get EHR list issue. [GetApiERHListWithWebId] : " + response.ErrorMessage);
                    SaveEHRErroLog(ehrConnected: true, errorMessage: "[GetApiERHListWithWebId] : " + response.ErrorMessage, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst);
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
                        ShowWriteError("EHR Configuration " + cmbEHRName.Text + " With " + cmbEHRVersion.Text + " is under development", "EHR Configuration", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError");
                    }
                    else
                    {
                        ShowWriteError("EHR Configuration" + cmbEHRName.Text + " With " + cmbEHRVersion.Text + " cannot configure with Adit app.", "EHR Configuration", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError");
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
                        ShowWriteError("EHR Location " + staLocation_EHR, write: true, forEmail: true, emailType: "LocationConfigurationError");
                        SaveEHRErroLog(ehrConnected: true, errorMessage: "EHR Location " + staLocation_EHR, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst);
                        return;
                    }
                    else
                    {
                        SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                        ShowWriteInSyncLog(cmbEHRName.Text.Trim() + " configuration save successfully.\n EHR Location save Successfully.", write: true);
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
                    ShowWriteInSyncLog("Organization configuration save successfully.", write: true);
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);

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
                    Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();
                    Utility.Location_ID = (RBtnMultiClinic.Checked == true ? StrFirstLocationValue : cmbAditLocation.SelectedValue.ToString());
                    Utility.Location_Name = (RBtnMultiClinic.Checked == true ? StrFirstLocationName : cmbAditLocation.Text.ToString());
                    Utility.Loc_ID = dtTempLocationTable.Rows[0]["Loc_ID"].ToString();
                    ShowWriteInSyncLog("Location configuration save successfully.", write: true);
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
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
                    Utility.NotAllowToChangeSystemDateFormat, Utility.EncryptString(cmbSystemUser.Text), Utility.EncryptString(txtSystemUserPassword.Text),
                    "", txtPMSUser.Text, txtPMSUserPassword.Text, chkIsZohoAssistAllowed.Checked);


                Utility.Application_Name = cmbEHRName.Text.Trim();
                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                try
                {
                    CommonUtility.GetSystemDetails();
                    string AppVersionAutoUpdateAditserverData = PushLiveDatabaseBAL.Push_Location_EHRUPdateForVersion();

                    ShowWriteInSyncLog("[AppVersion AutoUpdate ServerData] has completed successfully.", write: true);
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                }
                catch (Exception EX)
                {
                    ShowWriteError("btnLocationSave_Click AppVersion AutoUpdate ServerData : " + EX.Message, write: true, forEmail: true, emailType: "LocationConfigurationError", mailmsg: " AppVersion AutoUpdate ServerData issue - " + EX.Message);
                    SaveEHRErroLog(ehrConnected: true, errorMessage: "AppVersion AutoUpdate ServerData : " + EX.Message, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                }

                if (staInstallApp)
                {
                    Utility.Application_Name = cmbEHRName.Text.Trim();
                    Utility.Application_Version = cmbEHRVersion.Text.Trim();
                    Utility.Application_ID = Convert.ToInt32(cmbEHRName.SelectedValue.ToString());
                    Utility.PozativeEmail = txtPozativeEmailID.Text.Trim();
                    Utility.PozativeLocationID = PozatieLocationid;
                    Utility.PozativeLocationName = PozatieLocationName;
                    ShowWriteInSyncLog("Application installation has been completed successfully.", write: true);
                    try
                    {
                        Utility.UpdateBackupDBFromLocalDB();
                    }
                    catch { }
                    frmPozative.SynchDataLiveDB_Push_PozativeConfiguraion();
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                }

                if (Utility.AditSync == false && Utility.PozativeSync == true)
                {
                }
                else
                {
                    pnlLocationMain.Visible = false;
                    isProcessComplete = true;

                    Cursor.Current = Cursors.Default;
                    SaveEHRErroLog(ehrConnected: true, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: true);
                    SetPanelVisiblityEHRwise("ReportGenerated");
                    Application.DoEvents();
                    Thread.Sleep(5000);
                    Application.DoEvents();
                }
                #endregion
            }
            catch (Exception EX)
            {
                ShowWriteError("btnLocationSave_Click " + EX.Message, "EHR Location", show: true, write: true, forEmail: true, emailType: "LocationConfigurationError", mailmsg: "EHR Location set issue." + EX.Message);
                SaveEHRErroLog(ehrConnected: true, errorMessage: "EHR Location" + EX.Message, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: false);
            }
        }
        void PozativeSetupClose()
        {
            try
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName.ToUpper().Contains("POZATIVESETUP") && Process.GetCurrentProcess().Id != p.Id)
                    {
                        p.Kill();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("PozativeSetupClose : " + ex.Message.ToString(), write: true);
            }
        }
        private void btnLocationBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
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
                ShowWriteError("btnLocationBack_click " + ex.Message, "Pozative Service", show: true, write: true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region SoftDent Configuration
        public bool OpenSoftDentConnection()
        {
            try
            {
                string serverExeDir = SoftDentRegistryKey.ServerExeDir;
                ObjGoalBase.WriteToErrorLogFile("Softdent registry path: " + serverExeDir);
                string faircomServerName = "";
                ErrorCode errorCode = ErrorCode.Failure;
                bool b = InteropFactory.SoftDent.Open(serverExeDir, faircomServerName, out errorCode);
                ObjGoalBase.WriteToErrorLogFile("Softdent auto connected : " + b.ToString());

                return b;
            }
            catch (Exception ex)
            {
                ShowWriteError("OpenSoftDentConnection - " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError");
                return false;
            }
        }

        private void SaveSoftDentConfigurtion()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cmbEHRVersion.Text.ToLower() == "17.0.0+")
                {
                    bool IsEHRConnected = IsSoftDentConnected(false);
                    if (IsEHRConnected)
                    {
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        SetPanelVisiblityEHRwise("SoftDent");
                        if (isCalledFromSave)
                            ShowWriteError("SoftDent is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                        else
                            ShowWriteError("SoftDent not connecting automatically.", write: true, forEmail: true, emailType: "EHRConnectedError");
                    }
                    PostEHRConnectionLog("SoftDent", IsEHRConnected);
                }
                else
                {
                    ShowWriteError("Wrong Version", "EHR Version", show: true, write: true);
                }

            }
            catch (Exception ex)
            {
                ShowWriteError("SaveSoftDentConfigurtion " + ex.Message, "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "SoftDent connection issue." + ex.Message, "SoftDent " + ex.Message);
            }
            finally
            {
                ShowHideLoader(false);
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region ClearDent Configuration
        public DataTable ListLocalSqlInstances(RegistryKey hive)
        {
            try
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
                    if (key == null)
                    {
                        return null;
                    }

                    var value = key.GetValue(valueName) as string[];
                    if (value == null)
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
            catch (Exception ex)
            {
                ShowWriteError("ListLocalSqlInstances : " + ex.Message.ToString(), write: true);
                return null;
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
                ShowWriteError("Error_Find_CreateSqlDatabase " + "GetSqlServerNameANDServiceName " + ex.Message.ToString(), write: true);
            }
        }
        private void btnClearDentSave_Click(object sender, EventArgs e)
        {
            isCalledFromSave = true;
            btnClearDentSaveClick();
        }
        private void btnClearDentSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (isCalledFromSave)
                {
                    if (string.IsNullOrEmpty(txtClearDentHostName.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid ClearDent Hostname", "ClearDent Hostname", show: true);
                        txtClearDentHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtClearDentUserId.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid ClearDent User Id", "ClearDent Userid", show: true);
                        txtClearDentUserId.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txt_PatientDocPath.Text.Trim()))
                    {
                        ShowWriteError("Please Enter valid ClearDent Patient Document Path", "ClearDent DocumentPath", show: true);
                        txt_PatientDocPath.Focus();
                        return;
                    }
                }
                bool IsEHRConnected = IsClearDentConnected(false);

                if (IsEHRConnected)
                {
                    Cursor.Current = Cursors.Default;
                    SetPanelVisiblityEHRwise("Location", false);
                }
                else
                {
                    ShowWriteError("ClearDent not connecting automatically.", write: true, forEmail: true, emailType: "EHRConnectedError");
                    SetPanelVisiblityEHRwise("ClearDent");
                    if (isCalledFromSave)
                        ShowWriteError("ClearDent is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: true, write: true);
                }
                PostEHRConnectionLog("ClearDent", IsEHRConnected);
            }
            catch (Exception ex)
            {
                ShowWriteError("btnClearDentSave_Click " + ex.Message, "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: ex.Message);
            }
            finally
            {
                ShowHideLoader(false);
                CommonFunction.KillSpeechExe();
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
                        ShowWriteError("Cleardent is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: true, write: true);
                    }
                }
                else
                {
                    ShowWriteError("Wrong Version", "EHR Version", show: true, write: true);
                }

            }
            catch (Exception ex)
            {
                ShowWriteError("ClearDentConfigurtion " + ex.Message, "Authentication", show: true, write: true);
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
            isCalledFromSave = true;
            btnTrackerSaveClick();
        }
        private void btnTrackerSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cmbEHRVersion.Text.ToLower() == "11.29")
                {
                    if (isCalledFromSave)
                    {
                        if (string.IsNullOrEmpty(txtTrackerCredential.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid Tracker Database Credential", "Credential", show: true);
                            txtTrackerCredential.Focus();
                            return;
                        }
                        if (string.IsNullOrEmpty(txtTrackerDatabase.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid Tracker Database Name", "Database", show: true);
                            txtTrackerDatabase.Focus();
                            return;
                        }
                        if (string.IsNullOrEmpty(txtTrackerUserId.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid Tracker User Id", "UserId", show: true);
                            txtTrackerUserId.Focus();
                            return;
                        }
                        if (string.IsNullOrEmpty(txtTrackerPassword.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid Tracker Password", "Password", show: true);
                            txtTrackerPassword.Focus();
                            return;
                        }
                    }

                    bool IsEHRConnected = IsTrackerConnected(false);
                    if (IsEHRConnected)
                    {
                        Cursor.Current = Cursors.Default;
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        ShowWriteError("Tracker not connecting automatically.", write: true, forEmail: true, emailType: "EHRConnectedError");
                        SetPanelVisiblityEHRwise("Tracker");
                        if (isCalledFromSave)
                            ShowWriteError("Tracker is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: true, write: true);
                    }
                    PostEHRConnectionLog("Tracker", IsEHRConnected);
                }
                else
                {
                    ShowWriteError("Wrong Version", "EHR Version", show: true, write: true);
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("btnTrackerSave_Click " + ex.Message, "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: ex.Message);
            }
            finally
            {
                ShowHideLoader(false);
                CommonFunction.KillSpeechExe();
                Cursor.Current = Cursors.Default;
            }
        }
        public bool CheckTrackerConnection()
        {
            try
            {
                Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                bool IsEHRConnected = SystemBAL.GetEHRTrackerConnection();
                return IsEHRConnected;
            }
            catch (Exception ex)
            {
                ShowWriteError("Tracker is not connecting. " + "\n" + " Please enter valid credentials." + "CheckTrakerConnection " + ex.Message.ToString(), "CheckTrackerConnection", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                return false;
            }
        }

        public void ExecuteBatchFile(string FileName)
        {
            var psi = new ProcessStartInfo();
            psi.CreateNoWindow = true;
            psi.FileName = FileName;
            psi.Verb = "runas";
            try
            {
                var process = new Process();
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                psi.Verb = "runas";
                process.StartInfo = psi;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                ShowWriteError("ExecuteBatchFile " + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("CreateBatchFileForSqlService " + ex.Message, "Pozative Service", show: true, write: true);
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

                FileName = Application.StartupPath + "\\MultiUser.bat";

                ExecuteBatchFile(FileName);

                return true;
            }
            catch (Exception ex)
            {
                ShowWriteError("Tracker is not connecting. " + "\n" + " Please enter valid credentials." + "CreateCredentialANDUserForTracker " + ex.Message.ToString(), "CreateCredentialANdUserForTracker", show: true, write: true);
                FileName = Application.StartupPath + "\\MultiUser.bat";
                ExecuteBatchFile(FileName);
                throw;
            }
        }
        #endregion

        #region Abeldent Integration

        private void btnAbeldentSave_Click(object sender, EventArgs e)
        {
            isCalledFromSave = true;
            btnAbeldentSaveClick();
        }
        private void btnAbeldentSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cmbEHRVersion.Text.ToLower() == "14.4.2" || cmbEHRVersion.Text.ToLower() == "14.8.2")
                {
                    if (isCalledFromSave)
                    {
                        if (string.IsNullOrEmpty(cmbSqlServiceName.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid AbelDent SQL Service Name", "SQL Service", show: true);
                            cmbSqlServiceName.Focus();
                            return;
                        }
                        if (string.IsNullOrEmpty(cmbAbelDentHostName.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid AbelDent ServerName", "ServerName", show: true);
                            cmbAbelDentHostName.Focus();
                            return;
                        }
                        if (string.IsNullOrEmpty(txtAbelDentDatabase.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid AbelDent Database Name", "Database", show: true);
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
                    }

                    AbelDentHostName = cmbAbelDentHostName.Text.ToLower().Trim();
                    AbelDentDatabase = txtAbelDentDatabase.Text.Trim();
                    AbelDentUserId = txtAbelDentUserId.Text.Trim();
                    AbelDentPassword = txtAbelDentPassword.Text.Trim();

                    InitbwIsAbelDentConnected();
                }
                else
                {
                    ShowWriteError("Wrong Version", "EHR Version", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                }

            }
            catch (Exception ex)
            {
                ShowHideLoader(false);
                ShowWriteError("btnAbelDentSave_Click " + ex.Message, "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: ex.Message);
            }
            finally
            {
                CommonFunction.KillSpeechExe();
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
                ShowWriteError("AbelDent is not connecting. " + "\n" + " Please enter valid credentials." + "CheckAbelDentConnection " + ex.Message.ToString(), "CheckAbelDentConnection", show: true, write: true);
                return false;
            }
        }
        #endregion

        #region PracticeWeb Configuration
        private void btnPracticeWebSave_Click(object sender, EventArgs e)
        {
            isCalledFromSave = true;
            btnPracticeWebSaveClick();
        }
        private void btnPracticeWebSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cmbEHRVersion.Text.ToLower() == "21.1")
                {
                    if (isCalledFromSave)
                    {
                        if (string.IsNullOrEmpty(PracticeWeb_txthostName.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid PracticeWeb Hostname", "Hostname", show: true);
                            txtOpenDentalHostName.Focus();
                            return;
                        }
                        if (string.IsNullOrEmpty(txtPracticeWebDatabase.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid PracticeWeb Database Name", "Port", show: true);
                            txtOpenDentalDatabase.Focus();
                            return;
                        }
                        if (string.IsNullOrEmpty(txtPracticeWebUseID.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid PracticeWeb User Id", "IntegrationKey", show: true);
                            txtOpenDentalUserId.Focus();
                            return;
                        }
                    }
                    bool IsEHRConnected = IsPracticeWebConnected(false);

                    if (IsEHRConnected)
                    {
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
                        SetPanelVisiblityEHRwise("PracticeWeb");
                        if (isCalledFromSave)
                            ShowWriteError("PracticeWeb is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                        else
                            ShowWriteError("PracticeWeb not connecting automatically.", "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                    }
                    PostEHRConnectionLog("PracticeWeb", IsEHRConnected);
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("btnPracticeWebSave_Click " + ex.Message, "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", SaveEHRErroLogMsg: ex.Message);
            }
            finally
            {
                ShowHideLoader(false);
                CommonFunction.KillSpeechExe();
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
                ShowWriteError("tblHead_MouseDown" + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("lblHead_MouseDown " + ex.Message, "Pozative Service", show: true, write: true);
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
                        case 12://CrystalPM
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                        case 13://OfficeMate
                            GetVersion(cmbEHRName.Text.ToString());
                            break;
                    }
                    SetEHRCredentialValues();
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("cmbEHRName_SelectedIndexChanged " + ex.Message, "EHR Selected Index Changed", show: true, write: true);
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
                ShowWriteError("txtAdminPassword_Leave : " + ex.Message, write: true);
            }
        }
        private void txtPMSUserPassword_Leave(object sender, EventArgs e)
        {
            try
            {
                btnPMSUserNext.Focus();
                btnPMSUserNext.Select();
            }
            catch (Exception ex)
            {
                ShowWriteError("txtPMSUserPassword_Leave : " + ex.Message, write: true);
            }
        }
        private void cmbAditLocation_Leave(object sender, EventArgs e)
        {
            try
            {
                btnLocationSave.Focus();
            }
            catch (Exception ex)
            {
                ShowWriteError("cmbAditLocation_Leave : " + ex.Message, write: true);
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
                ShowWriteError("Error to open Technical Reference " + e1.Message, write: true);
            }
        }

        private void btnPracticeWorkSave_Click(object sender, EventArgs e)
        {
            isCalledFromSave = true;
            btnPracticeWorkSaveClick();
        }
        private void btnPracticeWorkSaveClick()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cmbEHRVersion.Text.ToLower() == "7.9+".ToLower())
                {
                    if (isCalledFromSave)
                    {
                        if (string.IsNullOrEmpty(txtPracticeworkHost.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid PracticeWork Hostname", "Hostname", show: true);
                            txtPracticeworkHost.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(txtPracticeworkDbName.Text.Trim()))
                        {
                            ShowWriteError("Please Enter valid PracticeWork Database", "Database", show: true);
                            txtPracticeworkDbName.Focus();
                            return;
                        }
                    }
                    bool IsEHRConnected = IsPracticeWorkConnected(false);
                    if (IsEHRConnected)
                    {
                        SetPanelVisiblityEHRwise("Location", false);
                    }
                    else
                    {
                        SetPanelVisiblityEHRwise("PracticeWork");
                        if (isCalledFromSave)
                            ShowWriteError("PracticeWork is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                        else
                            ShowWriteError("PracticeWork not connecting automatically.", write: true, forEmail: true, emailType: "EHRConnectedError");

                    }
                    PostEHRConnectionLog("PracticeWork", IsEHRConnected);
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("btnPracticeWorkSave_Click " + ex.Message, "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "PracticeWork authentication issue " + ex.Message, ex.Message);
            }
            finally
            {
                ShowHideLoader(false);
                CommonFunction.KillSpeechExe();
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
                ShowWriteError("picNotAllowToChangeSystemDateFormat_Click " + ex.Message, "Adit Sync", show: true, write: true);
            }
        }
        private void btn_Tracker_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("Tracker", 6);
        }
        private void btn_CD_Click(object sender, EventArgs e)
        {
            if (Utility.EHRDatabase.ToLower() == "freedental" || Utility.EHRDatabase.ToLower() == "opendental")
            {
                Utility.EHRDatabase = string.Empty;
                Utility.EHRUserId = string.Empty;
                Utility.EHRPassword = string.Empty;
                Utility.EHRPort = string.Empty;
            }
            SetValuesOnEHRSelection("ClearDent", 5);
        }
        private void btn_DTX_Click(object sender, EventArgs e)
        {
            ShowHideLoader(true);
            btnChatOurSupportTeam.Visible = false;
            lblKBArticle.Visible = false;
            lblMain1.Text = "Connecting to Dentrix Please wait...";
            btnDTXClick();
        }
        private void btnDTXClick()
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
                ShowWriteError("EHR User ddxdesktopspc.pfx Not Intalled. Please Aplication start with run as administrator. " + "btn_DTX_Click " + ex.Message, write: true);
            }

            SetValuesOnEHRSelection("Dentrix", 3);
        }
        private void btn_EG_Click(object sender, EventArgs e)
        {
            lblMain1.Text = "Connecting to Eaglesoft Please wait...";
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
        private void btn_CP_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("CrystalPM", 12);
        }
        private void btn_OM_Click(object sender, EventArgs e)
        {
            SetValuesOnEHRSelection("OfficeMate", 13);
        }
        private void SetValuesOnEHRSelection(string AppName, int EHRNo)
        {
            try
            {
                Utility.Application_Name = AppName;
                cmbEHRName.SelectedValue = EHRNo;
                pnlMain.Visible = false;
                tblEHRMain.Visible = true;
                SetEHRConfigurations();
            }
            catch (Exception ex)
            {
                ShowWriteError("SetValuesOnEHRSelection " + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("btn_AutoMainCancel_Click " + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("btn_OD_MouseHover " + ex.Message, "Pozative Service", show: true, write: true);
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
                ShowWriteError("btn_OD_MouseLeave " + ex.Message, "Pozative Service", show: true, write: true);
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
                        if (pnl.Name == "pnlOTPDigit1" || pnl.Name == "pnlOTPDigit2" || pnl.Name == "pnlOTPDigit3" || pnl.Name == "pnlOTPDigit4")
                            SetBorderColor(txt.Location.X, txt.Location.Y, txt.ClientSize.Width, txt.ClientSize.Height, transparentColor, e.Graphics, true);
                        else
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
                ShowWriteError("tblOpenDentalMain_Paint " + ex.Message, "Pozative Service", show: true, write: true);
            }
        }
        private void SetBorderColor(int x, int y, int width, int height, string color, Graphics graphics, bool isNoneBorder = false)
        {
            try
            {
                Rectangle rect = new Rectangle(x, y, width, height);

                rect.Inflate(1, 1);
                ControlPaint.DrawBorder(graphics, rect,
                   ColorTranslator.FromHtml(color), 0, ButtonBorderStyle.Solid,
                   ColorTranslator.FromHtml(color), 0, ButtonBorderStyle.Solid,
                   ColorTranslator.FromHtml(color), 0, ButtonBorderStyle.Solid,
                   ColorTranslator.FromHtml(color), isNoneBorder ? 0 : 1, ButtonBorderStyle.Solid);
            }
            catch (Exception ex)
            {
                ShowWriteError("SetBorderColor " + ex.Message, "Pozative Service", show: true, write: true);
            }
        }
        #endregion
        private void SaveEHRErroLog(bool ehrConnected = false, bool errorLogFileGenerated = false, string errorMessage = "", string locationID = "", List<string> multiClinicSelected = null, bool multiDatabaseConfiure = false, string organizationID = "", bool successfullyConfigured = false)
        {
            try
            {
                if (errorMessage != "")
                {
                    Utility.WriteToErrorLogFromAll(errorMessage);
                }
                string installedEHR = "";
                foreach (DataRow row in dtEHRList.Rows)
                {
                    installedEHR += row["EHR_Name"] + ",";
                }
                string strApiLocOrg = SystemBAL.SaveEHRLogs();
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var client = new RestClient(strApiLocOrg);
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
                    ShowWriteError("SaveEHRErroLog " + ex.Message, "Pozative Service", show: true, write: true);
                }
                PozativeConfigErrorLog AditLocOrgPost = new PozativeConfigErrorLog
                {
                    datetime = DateTime.Now.ToString(),
                    dentrix_pdf_connection_string = "",
                    ehr_connected = ehrConnected,
                    email = txtAdminUserName.Text.Trim(),
                    error_log_file_generated = errorLogFileGenerated,
                    error_message = errorMessage,
                    image_folder_path = "",
                    locationId = locationID,
                    multiclinic_selected = multiClinicSelected,
                    multidatabase_configure = multiDatabaseConfiure,
                    organizationId = loggedUserOraganizationID,
                    password = txtAdminPassword.Text.Trim(),
                    selected_ehr = selectedEHR + " - " + Utility.DBConnString,
                    sucessfully_configured = successfullyConfigured,
                    system_configuration = SystemConfig,
                    total_installed_ehr = installedEHR,
                };
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(AditLocOrgPost);
                request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.ErrorMessage != null)
                {
                    ShowWriteError(response.ErrorMessage, "Adit App Configuration ", show: true, write: true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("SaveEHRErroLog : " + ex.Message, write: true);
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
                CommonFunction.TextToSpeech("welcomemessage");
            }
            catch (Exception ex)
            {
                ShowWriteError("frmConfiguration_Auto_Shown " + ex.Message, "Pozative Service", show: true, write: true);
            }
        }
        private void btnFormMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnFormClose_Click(object sender, EventArgs e)
        {
            PozativeSetupClose();
            Application.Exit();
        }
        private void CheckEHRConnectionAndVisibility()
        {
            try
            {
                List<string> removeEHR = new List<string>();
                bool isVisible = false;
                if (btn_EG.Visible)
                {
                    isVisible = IsEagleSoftConnected();
                    if (!isVisible)
                        removeEHR.Add("Eaglesoft");
                }
                if (btn_OD.Visible)
                {
                    //isVisible = IsOpenDentalConnected();
                    //if (!isVisible)
                    //    removeEHR.Add("Open Dental");
                }
                if (btn_abeldent.Visible)
                {
                    isVisible = IsAbelDentConnected();
                    if (!isVisible)
                        removeEHR.Add("AbelDent");
                }
                if (btn_PWeb.Visible)
                {
                    //isVisible = IsPracticeWebConnected();
                    //if (!isVisible)
                    //    removeEHR.Add("PracticeWeb");
                }
                if (btn_Pwork.Visible)
                {
                    isVisible = IsPracticeWorkConnected();
                    if (!isVisible)
                        removeEHR.Add("PracticeWork");
                }
                if (btn_Tracker.Visible)
                {
                    isVisible = IsTrackerConnected();
                    if (!isVisible)
                        removeEHR.Add("Tracker");
                }
                if (btn_CP.Visible)
                {
                    isVisible = IsCrystalPMConnected();
                    if (!isVisible)
                        removeEHR.Add("CrystalPM");
                }
                if (btn_OM.Visible)
                {
                    isVisible = IsOfficeMateConnected();
                    if (!isVisible)
                        removeEHR.Add("OfficeMate");
                }
                if (true)
                {
                    if (!IsClearDentConnected())
                        removeEHR.Add("ClearDent");
                }
                if (btn_DTX.Visible)
                {
                    Utility.Application_Version = cmbEHRVersion.Text.Trim();
                    Utility.EHRHostname = txtDentrixHostName.Text.ToLower().Trim();
                    Utility.EHRIntegrationKey = string.Empty;
                    Utility.EHRUserId = txtDentrixUserId.Text.Trim();
                    Utility.EHRPassword = txtDentrixPassword.Text.Trim();
                    Utility.EHRDatabase = string.Empty;
                    Utility.EHRPort = txtDentrixPort.Text.Trim();
                    Utility.NotAllowToChangeSystemDateFormat = picNotAllowToChangeSystemDateFormat.Tag.ToString() == "ON" ? true : false;

                    isVisible = IsDentrixConnected();
                    if (!isVisible)
                        removeEHR.Add("Dentrix");
                }
                //btn_EZ.Visible = true;
                //btn_SoftDent.Visible = true;
                //dtEHRList.Rows.Add(4, "SoftDent");
                //dtEHRList.Rows.Add(8, "Easy Dental");

                if (removeEHR.Count > 0 && removeEHR.Count != (cmbEHRName.Items.Count - 1))
                {
                    for (int i = 0; i < removeEHR.Count; i++)
                    {
                        if (removeEHR[i] == "Eaglesoft")
                            btn_EG.Visible = false;
                        //if (removeEHR[i] == "Open Dental")
                        //    btn_OD.Visible = false;
                        if (removeEHR[i] == "AbelDent")
                            btn_abeldent.Visible = false;
                        //if (removeEHR[i] == "PracticeWeb")
                        //    btn_PWeb.Visible = false;
                        if (removeEHR[i] == "PracticeWork")
                            btn_Pwork.Visible = false;
                        if (removeEHR[i] == "Tracker")
                            btn_Tracker.Visible = false;
                        if (removeEHR[i] == "ClearDent")
                            btn_CD.Visible = false;
                        if (removeEHR[i] == "Dentrix")
                            btn_DTX.Visible = false;
                        if (removeEHR[i] == "CrystalPM")
                            btn_CP.Visible = false;
                        if (removeEHR[i] == "OfficeMate")
                            btn_OM.Visible = false;
                    }

                    List<int> lstIndex = new List<int>();
                    for (int i = 0; i < cmbEHRName.Items.Count; i++)
                    {
                        if (removeEHR.Contains(cmbEHRName.GetItemText(cmbEHRName.Items[i])))
                        {
                            lstIndex.Add(i);
                        }
                    }
                    foreach (int i in lstIndex)
                    {
                        cmbEHRName.Items.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("CheckEHRConnectionAndVisibility : " + ex.Message, write: true);
            }
        }
        private void frmConfiguration_Auto_MouseDown(object sender, MouseEventArgs e)
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
                ShowWriteError("frmConfiguration_Auto_MouseDown : " + ex.Message, write: true);
            }
        }
        private void btnPMSUserBack_Click(object sender, EventArgs e)
        {
            try
            {
                SetPanelVisiblityEHRwise("Location", false);
            }
            catch (Exception ex)
            {
                ShowWriteError("btnPMSUserBack_Click " + ex.Message, "Pozative Service", show: true, write: true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void btnPMSUserNext_Click(object sender, EventArgs e)
        {
            try
            {
                SetPanelVisiblityEHRwise("SystemCredential", false);
            }
            catch (Exception ex)
            {
                ShowWriteError("btnPMSUserNext_Click - " + ex.Message.ToString(), write: true, forEmail: true, emailType: "SystemLoginError", mailmsg: "Issue in system credentials or task/service creation - " + ex.Message);
            }
        }
        #region System Credentials
        private void lblInfo_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("https://help.adit.com/portal/en/kb/articles/why-does-the-server-app-need-my-admin-credentials");
            Process.Start(helpDocURL);
        }
        private void lblSkip_Click(object sender, EventArgs e)
        {
            pnlSkipPopup.Visible = true;
            pnlSkipPopup.Location = new Point(-3, -3);
            pnlSkipPopup.BringToFront();
            this.Size = new Size(845, 580);
        }
        private void btnSystemUserBack_Click(object sender, EventArgs e)
        {
            try
            {
                SkipSystemCredential = false;

                Cursor.Current = Cursors.WaitCursor;
                SetPanelVisiblityEHRwise("PMSCredential", false);
            }
            catch (Exception ex)
            {
                ShowWriteError("btnSystemUserBack_Click " + ex.Message, "Pozative Service", show: true, write: true);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void AddFirewallPort(string RuleName, string Dir, string port)
        {
            try
            {
                RunShellCommand(
                                   "netsh.exe",
                                   String.Format("advfirewall firewall add rule name=\"{0}\" dir={1} action=allow protocol={2} localport={3} profile={4}",
                                   RuleName,
                                   Dir,// InBound Outbound
                                   "TCP", // protocol
                                   port, // Port
                                   "Any" // Can be Private, Domain, Public or Any
                                   ));
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("FireWall Error - " + Ex.Message);
            }
        }
        private void RunShellCommand(string command, string parms)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(command);
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.CreateNoWindow = true;
                psi.Verb = "runas"; //This is what actually runs the command as administrator

                Process proc = Process.Start(psi);
                System.IO.StreamWriter sw = proc.StandardInput;
                sw.WriteLine(parms);
                sw.Close();
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("Firewall Error Run Command" + Ex.Message);
            }
        }
        private void btnSystemUserNext_Click(object sender, EventArgs e)
        {
            btnSystemUserNextClick();
        }
        private void btnSystemUserNextClick()
        {
            try
            {
                Application.DoEvents();
                bool valid = SkipSystemCredential ? true : IsValidUser();

                if (valid)
                {
                    mailData.SystemLoginStatus = true;
                    FinalSaveAfterLocationConfiguration();
                    isInstallationComplete = true;
                    ConfigurationComplete();
                }
                else
                {
                    if (string.IsNullOrEmpty(mailData.SystemLoginError))
                        ShowWriteError("The credentials you have entered are incorrect. Please try again.", write: true, forEmail: true, emailType: "SystemLoginError");
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("btnSystemUserNext_Click - " + ex.Message.ToString(), write: true, forEmail: true, emailType: "SystemLoginError", mailmsg: "Issue in system credentials or task/service creation - " + ex.Message);
            }
        }
        private void ConfigurationComplete()
        {
            AddFirewallPort("Adit_Inbound_65530", "65530", "in");
            AddFirewallPort("Adit_Outbound_65530", "65530", "out");

            if (string.IsNullOrEmpty(mailData.LocationConfigurationError))
                mailData.LocationConfigurationStatus = true;
            OpenTaskScheduler_ServiceWindow();
            this.DialogResult = DialogResult.OK;
            this.Close();
            mailData.SystemAppConfigurationStatus = true;
        }
        private void OpenTaskScheduler_ServiceWindow()
        {
            try
            {
                try
                {
                    //ShowWriteInSyncLog("before CreateTaskScheduler() call in config.", write: true);
                    CreateTaskScheduler();
                }
                catch (Exception ex1)
                {
                    ShowWriteError("Task scheduler not created because of some error : " + ex1.Message, "Adit Module Sync Configuration", show: true, write: true);
                }

                try
                {
                    //ShowWriteInSyncLog("before CreateService() call in config.", write: true);
                    CreateService();
                    CreateServiceAditEventListener();
                }
                catch (Exception ex1)
                {
                    ShowWriteError("Windows Service not created because of some error : " + ex1.Message, "Adit Module Sync Configuration", show: true, write: true);
                }
            }
            catch (Exception EX)
            {
                ShowWriteError("OpenTaskScheduler_ServiceWindow " + EX.Message, "Task Scheduler/Service", show: true, write: true);
                SaveEHRErroLog(ehrConnected: true, errorMessage: "EHR Location" + EX.Message, locationID: Utility.Location_ID, multiClinicSelected: locationWithClinicLst, multiDatabaseConfiure: false, successfullyConfigured: false);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                isProcessComplete = true;
            }
        }
        private void CreateTaskScheduler()
        {
            try
            {
                //ShowWriteInSyncLog("create task scheduler after config starts ", write: true);

                if (SkipSystemCredential)
                {
                    string TaskName = "PozativeTask";
                    var ts = new TaskService();
                    var task = ts.RootFolder.Tasks.Where(a => a.Name.ToLower() == TaskName.ToLower()).FirstOrDefault();
                    if (task == null)
                    {
                        TaskDefinition td = ts.NewTask();
                        td.RegistrationInfo.Description = "Start Adit Application Daily";
                        td.Settings.Compatibility = TaskCompatibility.V2;
                        Trigger trigger = new DailyTrigger();
                        trigger.Repetition.Interval = TimeSpan.FromHours(2);
                        trigger.StartBoundary = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString() + " " + "07:00:00");
                        td.Triggers.Add(trigger);
                        td.Actions.Add(new ExecAction(Application.StartupPath.ToString() + "\\Pozative.exe", "", null));
                        td.Settings.StopIfGoingOnBatteries = false;
                        td.Settings.DisallowStartIfOnBatteries = false;
                        td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
                        td.Principal.RunLevel = TaskRunLevel.Highest;
                        ts.RootFolder.RegisterTaskDefinition(@"" + TaskName.ToString() + "", td, TaskCreation.CreateOrUpdate, "NT AUTHORITY\\System", null, TaskLogonType.ServiceAccount, null);
                    }
                }
                else
                {
                    string TaskName = "PozativeTask";
                    var ts = new TaskService();
                    //ShowWriteInSyncLog("before get task  ", write: true);
                    var task = ts.RootFolder.Tasks.Where(a => a.Name.ToLower() == TaskName.ToLower()).FirstOrDefault();
                    // ShowWriteInSyncLog("after get task  ", write: true);

                    if (task == null)
                    {
                        // ShowWriteInSyncLog("inside create new task  ", write: true);

                        TaskDefinition td = ts.NewTask();
                        td.RegistrationInfo.Description = "Start Adit Application Daily";
                        td.Settings.Compatibility = TaskCompatibility.V2;
                        Trigger trigger = new DailyTrigger();
                        trigger.Repetition.Interval = TimeSpan.FromHours(2);
                        trigger.StartBoundary = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString() + " " + "07:00:00");
                        td.Triggers.Add(trigger);
                        td.RegistrationInfo.Author = WindowsIdentity.GetCurrent().Name;
                        td.Principal.LogonType = TaskLogonType.S4U;
                        td.Actions.Add(new ExecAction(Application.StartupPath.ToString() + "\\Pozative.exe", "", null));
                        td.Settings.StopIfGoingOnBatteries = false;
                        td.Settings.DisallowStartIfOnBatteries = false;
                        td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
                        td.Principal.RunLevel = TaskRunLevel.Highest;
                        string System_AdminName = cmbSystemUser.Text;
                        string System_AdminPwd = txtSystemUserPassword.Text;
                        //ShowWriteError("before RegisterTaskDefinition  ", write: true);

                        ts.RootFolder.RegisterTaskDefinition(@"" + TaskName.ToString() + "", td, TaskCreation.CreateOrUpdate, System_AdminName, System_AdminPwd, TaskLogonType.Password, null);
                        //ShowWriteError("after RegisterTaskDefinition  ", write: true);

                        ShowWriteInSyncLog("create task scheduler after config successfully ", write: true);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("Error while create task scheduler " + ex.Message.ToString(), write: true);
            }
        }
        private void CreateService()
        {
            try
            {
                //ShowWriteError("CreateService called", write: true);
                string FileName = "";
                FileName = Application.StartupPath + "\\InstallWindowService.bat";
                if (System.IO.File.Exists(FileName))
                {
                    //ShowWriteError("file exist its deleted", write: true);
                    System.IO.File.Delete(FileName);
                }
                if (!File.Exists(FileName))
                {
                    //ShowWriteError("file not exist its created", write: true);
                    File.Create(FileName).Dispose();
                }
                #region Create service with SCManager
                if (SkipSystemCredential)
                {
                    using (StreamWriter SW = new StreamWriter(FileName))
                    {
                        string cmd = @"sc create PozativeAppStatus binpath= """ + Application.StartupPath + @"\PozativeAppStatus.exe"" start= ""auto""";
                        //ShowWriteError("SkipSystemCredential inside using - " + cmd, write: true);
                        try
                        {
                            SW.WriteLine(cmd);
                            SW.WriteLine(@"sc start PozativeAppStatus");
                            SW.WriteLine(@"exit 0");
                            SW.Close();
                        }
                        catch (Exception ex)
                        {
                            ShowWriteError("CreateService with skip - " + ex.Message.ToString(), write: true);
                        }
                    }
                }
                else
                {
                    using (StreamWriter SW = new StreamWriter(FileName))
                    {
                        //ShowWriteError("with credential inside using", write: true);

                        string System_AdminName = cmbSystemUser.Text;
                        string System_AdminPwd = txtSystemUserPassword.Text;

                        //string cmd = @"sc create PozativeAppStatus obj= """ + System.Environment.MachineName + "\\" + System_AdminName + @""" password= """ + System_AdminPwd + @""" binpath= """ + Application.StartupPath + @"\PozativeAppStatus.exe"" start= ""auto""";
                        string cmd = @"sc create PozativeAppStatus binpath= """ + Application.StartupPath + @"\PozativeAppStatus.exe"" start= ""auto""";
                        string cmdConifg = @"sc config PozativeAppStatus obj= """ + ".\\" + System_AdminName + @""" password= """ + System_AdminPwd + "\"";
                        ShowWriteInSyncLog(cmd, write: true);
                        try
                        {
                            SW.WriteLine(cmd);
                            SW.WriteLine(cmdConifg);
                            SW.WriteLine(@"sc start PozativeAppStatus");
                            SW.WriteLine(@"exit 0");
                            SW.Close();
                        }
                        catch (Exception ex)
                        {
                            ShowWriteError("CreateService with username - " + ex.Message.ToString(), write: true);
                        }
                    }
                }
                #endregion
                ExecuteBatchFile(FileName);
                ShowWriteInSyncLog("windows service created successfully.", write: true);
                UnInstallService();
            }
            catch (Exception ex)
            {
                ShowWriteError("CreateService - " + ex.Message.ToString(), write: true);
            }
        }

        private void CreateServiceAditEventListener()
        {
            try
            {
                //ShowWriteError("CreateServiceAditEventListener called", write: true);
                string FileName = "";
                FileName = Application.StartupPath + "\\InstallAditEventListener.bat";
                if (System.IO.File.Exists(FileName))
                {
                    //ShowWriteError("CreateServiceAditEventListener: file exist its deleted", write: true);
                    System.IO.File.Delete(FileName);
                }
                if (!File.Exists(FileName))
                {
                    //ShowWriteError("CreateServiceAditEventListener: file not exist its created", write: true);
                    File.Create(FileName).Dispose();
                }
                #region Create service with SCManager
                if (SkipSystemCredential)
                {
                    using (StreamWriter SW = new StreamWriter(FileName))
                    {
                        string cmd = @"sc create AditEventListener binpath= """ + Application.StartupPath + @"\AditEventListener.exe"" start= ""auto""";
                        ShowWriteInSyncLog("CreateServiceAditEventListener: SkipSystemCredential inside using - " + cmd, write: true);
                        try
                        {
                            SW.WriteLine(cmd);
                            SW.WriteLine(@"sc start AditEventListener");
                            SW.WriteLine(@"exit 0");
                            SW.Close();
                        }
                        catch (Exception ex)
                        {
                            ShowWriteError("CreateServiceAditEventListener: CreateService with skip - " + ex.Message.ToString(), write: true);
                        }
                    }
                }
                else
                {
                    using (StreamWriter SW = new StreamWriter(FileName))
                    {
                        //ShowWriteError("CreateServiceAditEventListener: with credential inside using", write: true);

                        string System_AdminName = cmbSystemUser.Text;
                        string System_AdminPwd = txtSystemUserPassword.Text;

                        //string cmd = @"sc create PozativeAppStatus obj= """ + System.Environment.MachineName + "\\" + System_AdminName + @""" password= """ + System_AdminPwd + @""" binpath= """ + Application.StartupPath + @"\PozativeAppStatus.exe"" start= ""auto""";
                        string cmd = @"sc create AditEventListener binpath= """ + Application.StartupPath + @"\AditEventListener.exe"" start= ""auto""";
                        string cmdConifg = @"sc config AditEventListener obj= """ + ".\\" + System_AdminName + @""" password= """ + System_AdminPwd + "\"";
                        ShowWriteInSyncLog(cmd, write: true);
                        try
                        {
                            SW.WriteLine(cmd);
                            SW.WriteLine(cmdConifg);
                            SW.WriteLine(@"sc start AditEventListener");
                            SW.WriteLine(@"exit 0");
                            SW.Close();
                        }
                        catch (Exception ex)
                        {
                            ShowWriteError("CreateServiceAditEventListener: with username - " + ex.Message.ToString(), write: true);
                        }
                    }
                }
                #endregion
                ExecuteBatchFile(FileName);
                ShowWriteInSyncLog("CreateServiceAditEventListener: windows service created successfully.", write: true);
                UnInstallAditEventListener();
            }
            catch (Exception ex)
            {
                ShowWriteError("CreateServiceAditEventListener - " + ex.Message.ToString(), write: true);
            }
        }

        private void CreateServiceInstallUtil()
        {
            try
            {
                #region Create service with installutil
                bool IsServiceCreated = CheckServiceCreated();
                if (!IsServiceCreated)
                {
                    string FileName = "";
                    FileName = Application.StartupPath + "\\UnInstallWindowService.bat";
                    if (System.IO.File.Exists(FileName))
                    {
                        System.IO.File.Delete(FileName);
                    }
                    if (!File.Exists(FileName))
                    {
                        File.Create(FileName).Dispose();
                    }
                    if (SkipSystemCredential)
                    {
                        using (StreamWriter SW = new StreamWriter(FileName))
                        {
                            //ShowWriteError("SkipSystemCredential inside using 2nd batch file", write: true);
                            try
                            {
                                SW.WriteLine(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe " + "\"" + Application.StartupPath + "\\PozativeAppStatus.exe\"");
                                SW.WriteLine(@"net start PozativeAppStatus");
                                SW.WriteLine(@"exit 0");
                                SW.Close();
                            }
                            catch (Exception ex)
                            {
                                ShowWriteError("CreateServiceInstallUtil with skip 2nd batch file - " + ex.Message.ToString(), write: true);
                            }
                        }
                    }
                    else
                    {
                        using (StreamWriter SW = new StreamWriter(FileName))
                        {
                            //ShowWriteError("with credential inside using  2nd batch file", write: true);

                            string System_AdminName = cmbSystemUser.Text;
                            string System_AdminPwd = txtSystemUserPassword.Text;
                            //ShowWriteError(@"sc create PozativeAppStatus binpath= " + Application.StartupPath + "\\PozativeAppStatus.exe\" start= auto obj= " + System.Environment.MachineName + "\\" + System_AdminName + " password= " + System_AdminPwd, write: true);
                            try
                            {
                                SW.WriteLine(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe " + " /username=\"" + System_AdminName + "\" /password=\"" + System_AdminPwd + "\"  \"" + Application.StartupPath + "\\PozativeAppStatus.exe\"");
                                SW.WriteLine(@"net start PozativeAppStatus");
                                SW.WriteLine(@"exit 0");
                                SW.Close();
                            }
                            catch (Exception ex)
                            {
                                ShowWriteError("CreateServiceInstallUtil with username 2nd batch file - " + ex.Message.ToString(), write: true);
                            }
                        }
                    }
                    ExecuteBatchFile(FileName);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ShowWriteError("CreateServiceInstallUtil - " + ex.Message.ToString(), write: true);
            }
        }
        private bool CheckServiceCreated()
        {
            try
            {
                System.ServiceProcess.ServiceController[] services = System.ServiceProcess.ServiceController.GetServices(Environment.MachineName);
                var service = services.FirstOrDefault(s => s.ServiceName == "PozativeAppStatus");
                if (service != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ShowWriteError("CheckServiceCreated - " + ex.Message.ToString(), write: true);
                return false;
            }
        }
        private void UnInstallService(bool onlyCreateFile = true)
        {
            try
            {
                string FileName = "";
                FileName = Application.StartupPath + "\\UnInstallWindowService.bat";
                if (System.IO.File.Exists(FileName))
                {
                    System.IO.File.Delete(FileName);
                }
                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Dispose();
                }
                if (SkipSystemCredential)
                {
                    using (StreamWriter SW = new StreamWriter(FileName))
                    {
                        SW.WriteLine(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe /u " + "\"" + Application.StartupPath + "\\PozativeAppStatus.exe\"");
                        SW.WriteLine(@"net start PozativeAppStatus");
                        SW.WriteLine(@"exit 0");
                        SW.Close();
                    }
                }
                else
                {
                    using (StreamWriter SW = new StreamWriter(FileName))
                    {
                        string System_AdminName = cmbSystemUser.Text;
                        string System_AdminPwd = txtSystemUserPassword.Text;

                        //ShowWriteError(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe /u " + "/username=\"" + System_AdminName + "\" /password=\"" + System_AdminPwd + "\"\"" + Application.StartupPath + "\\PozativeAppStatus.exe\"", write: true);
                        SW.WriteLine(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe /u " + " /username=\"" + System_AdminName + "\" /password=\"" + System_AdminPwd + "\"  \"" + Application.StartupPath + "\\PozativeAppStatus.exe\"");
                        SW.WriteLine(@"net start PozativeAppStatus");
                        SW.WriteLine(@"exit 0");
                        SW.Close();
                    }
                }
                if (!onlyCreateFile)
                    ExecuteBatchFile(FileName);
            }
            catch (Exception ex)
            {
                ShowWriteError("UnInstallService - " + ex.Message.ToString(), write: true);
            }
        }

        private void UnInstallAditEventListener(bool onlyCreateFile = true)
        {
            try
            {
                string FileName = "";
                FileName = Application.StartupPath + "\\UnInstallAditEventListener.bat";
                if (System.IO.File.Exists(FileName))
                {
                    System.IO.File.Delete(FileName);
                }
                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Dispose();
                }
                if (SkipSystemCredential)
                {
                    using (StreamWriter SW = new StreamWriter(FileName))
                    {
                        SW.WriteLine(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe /u " + "\"" + Application.StartupPath + "\\AditEventListener.exe\"");
                        SW.WriteLine(@"net start AditEventListener");
                        SW.WriteLine(@"exit 0");
                        SW.Close();
                    }
                }
                else
                {
                    using (StreamWriter SW = new StreamWriter(FileName))
                    {
                        string System_AdminName = cmbSystemUser.Text;
                        string System_AdminPwd = txtSystemUserPassword.Text;

                        //ShowWriteError(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe /u " + "/username=\"" + System_AdminName + "\" /password=\"" + System_AdminPwd + "\"\"" + Application.StartupPath + "\\AditEventListener.exe\"", write: true);
                        SW.WriteLine(@"C:\windows\microsoft.net\framework\v4.0.30319\installutil.exe /u " + " /username=\"" + System_AdminName + "\" /password=\"" + System_AdminPwd + "\"  \"" + Application.StartupPath + "\\AditEventListener.exe\"");
                        SW.WriteLine(@"net start AditEventListener");
                        SW.WriteLine(@"exit 0");
                        SW.Close();
                    }
                }
                if (!onlyCreateFile)
                    ExecuteBatchFile(FileName);
            }
            catch (Exception ex)
            {
                ShowWriteError("UnInstallAditEventListener - " + ex.Message.ToString(), write: true);
            }
        }
        //---------------------------Validate User
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        [DllImport("kernel32.dll")]
        public static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, ref IntPtr Arguments);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);
        private bool IsValidUser()
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
                UserName = cmbSystemUser.Text;
                Pwd = txtSystemUserPassword.Text;

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
                    ShowWriteError("System Credentials issue - " + errmsg, "", show: true, write: true, forEmail: true, emailType: "SystemLoginError");
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
                            ShowWriteError("The user credentials you have entered does not have the proper admin configurations in order to proceed with the set up. Please try again with a different user with admin rights.", "", show: true, write: true, forEmail: true, emailType: "SystemLoginError");
                            valid = false;
                        }
                    }
                }
                CloseHandle(tokenHandle);
            }
            catch (Exception ex)
            {
                ShowWriteError("IsValidUser - " + ex.Message.ToString(), write: true, forEmail: true, emailType: "SystemLoginError", mailmsg: "System Credential issue - " + ex.Message);
            }
            return valid;
        }
        public bool IsUserMemberOfAdministratorsGroup(string username)
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
                ShowWriteError("IsUserMemberOfAdministratorsGroup : " + ex.Message, write: true, forEmail: true, emailType: "SystemLoginError", mailmsg: "User group checking issue - " + ex.Message);
            }

            // User or group not found or error occurred
            return false;
        }
        public string GetErrorMessage(int errorCode)
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
                    ShowWriteError("Failed to format message for error code " + errorCode.ToString() + ". ", write: true);
                    throw new Exception("Failed to format message for error code " + errorCode.ToString() + ". ");
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("GetErrorMessage : " + ex.Message, write: true);
            }
            return lpMsgBuf;

        }
        private List<string> GetComputerUsers()
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
                ShowWriteError("GetComputerUsers " + ex.Message, "Pozative Service", show: true, write: true);
                return null;
            }
        }
        private void lblShowAdminPassword_Click(object sender, EventArgs e)
        {
            try
            {
                switch ((sender as Label).Name)
                {
                    case "lblShowAdminPassword":
                        txtAdminPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowSystemPassword":
                        txtSystemUserPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowTrackerPassword":
                        txtTrackerPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowODPassword":
                        txtOpenDentalPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowDTXPassword":
                        txtDentrixPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowEGPassword":
                        txtEaglesoftPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowSDPassword":
                        txtSoftDentPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowPWorkPassword":
                        txtPracticeworkPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowCDPassword":
                        txtClearDentPassowrd.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowADPassword":
                        txtAbelDentPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowPWebPassword":
                        txtPracticeWebPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowCPPassword":
                        txtCrystalPMPassword.PasswordChar = Char.MinValue;
                        break;
                    case "lblShowPMSPassword":
                        txtPMSUserPassword.PasswordChar = Char.MinValue;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("lblShowAdminPassword_Click : " + ex.Message, write: true);
            }
        }
        private void lblShowAdminPassword_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                switch ((sender as Label).Name)
                {
                    case "lblShowAdminPassword":
                        txtAdminPassword.PasswordChar = '*';
                        break;
                    case "lblShowSystemPassword":
                        txtSystemUserPassword.PasswordChar = '*';
                        break;
                    case "lblShowTrackerPassword":
                        txtTrackerPassword.PasswordChar = '*';
                        break;
                    case "lblShowODPassword":
                        txtOpenDentalPassword.PasswordChar = '*';
                        break;
                    case "lblShowDTXPassword":
                        txtDentrixPassword.PasswordChar = '*';
                        break;
                    case "lblShowEGPassword":
                        txtEaglesoftPassword.PasswordChar = '*';
                        break;
                    case "lblShowSDPassword":
                        txtSoftDentPassword.PasswordChar = '*';
                        break;
                    case "lblShowPWorkPassword":
                        txtPracticeworkPassword.PasswordChar = '*';
                        break;
                    case "lblShowCDPassword":
                        txtClearDentPassowrd.PasswordChar = '*';
                        break;
                    case "lblShowADPassword":
                        txtAbelDentPassword.PasswordChar = '*';
                        break;
                    case "lblShowPWebPassword":
                        txtPracticeWebPassword.PasswordChar = '*';
                        break;
                    case "lblShowCPPassword":
                        txtPracticeWebPassword.PasswordChar = '*';
                        break;
                    case "lblShowPMSPassword":
                        txtPMSUserPassword.PasswordChar = '*';
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("lblShowAdminPassword_MouseLeave : " + ex.Message, write: true);
            }
        }
        #endregion
        private void ShowHideLoader(bool show)
        {
            try
            {
                if (show)
                {
                    picLoader.Visible = true;
                    picLoader.BringToFront();
                }
                else
                {
                    picLoader.Visible = false;
                    picLoader.SendToBack();
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("ShowHideLoader : " + ex.Message, write: true);
            }
        }

        BackgroundWorker bwIsEagleSoftConnected = null;
        private void InitbwIsEagleSoftConnected()
        {
            try
            {
                bwIsEagleSoftConnected = new BackgroundWorker();
                bwIsEagleSoftConnected.DoWork += BwIsEagleSoftConnected_DoWork;
                bwIsEagleSoftConnected.RunWorkerCompleted += BwIsEagleSoftConnected_RunWorkerCompleted;
                bwIsEagleSoftConnected.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ShowWriteError("InitbwIsEagleSoftConnected : " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "EagleSoft connection issue - " + ex.Message);
            }
        }

        private void BwIsEagleSoftConnected_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((bool)e.Result)
                {
                    SetPanelVisiblityEHRwise("Location", false);
                }
                else
                {
                    ShowWriteError("EagleSoft not connecting automatically.", write: true, forEmail: true, emailType: "");
                    SetPanelVisiblityEHRwise("Eaglesoft");
                }
                PostEHRConnectionLog("Eaglesoft", (bool)e.Result);
                ShowHideLoader(false);
                //btnChatOurSupportTeam.Visible = true;
                lblKBArticle.Visible = true;
            }
            catch (Exception ex)
            {
                ShowWriteError("BwIsEagleSoftConnected_RunWorkerCompleted : " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "Eaglesoft auto connection issue - " + ex.Message);
            }
        }

        private void BwIsEagleSoftConnected_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                bool isConnected = IsEagleSoftConnected(false);
                e.Result = isConnected;
            }
            catch (Exception ex)
            {
                ShowWriteError("BwIsEagleSoftConnected_DoWork : " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "Eaglesoft auto connection issue - " + ex.Message);
            }
        }
        
        BackgroundWorker bwIsOfficeMateConnected = null;
        private void InitbwIsOfficeMateConnected()
        {
            try
            {
                bwIsOfficeMateConnected = new BackgroundWorker();
                bwIsOfficeMateConnected.DoWork += BwIsAbelDentConnected_DoWork;
                bwIsOfficeMateConnected.RunWorkerCompleted += BwIsAbelDentConnected_RunWorkerCompleted;
                bwIsOfficeMateConnected.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ShowWriteError("InitbwIsOfficeMateConnected : " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "AbelDent connection issue - " + ex.Message);
            }
        }
        BackgroundWorker bwIsAbelDentConnected = null;
        private void InitbwIsAbelDentConnected()
        {
            try
            {
                bwIsAbelDentConnected = new BackgroundWorker();
                bwIsAbelDentConnected.DoWork += BwIsAbelDentConnected_DoWork;
                bwIsAbelDentConnected.RunWorkerCompleted += BwIsAbelDentConnected_RunWorkerCompleted;
                bwIsAbelDentConnected.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                ShowWriteError("InitbwIsAbelDentConnected : " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "AbelDent connection issue - " + ex.Message);
            }
        }

        private void BwIsAbelDentConnected_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((bool)e.Result)
                {
                    SetPanelVisiblityEHRwise("Location", false);
                }
                else
                {
                    SetPanelVisiblityEHRwise("AbelDent");
                    if (isCalledFromSave)
                        ShowWriteError("AbelDent is not connecting. " + "\n" + " Please enter valid credentials.", "Authentication", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                    else
                        ShowWriteError("AbelDent not connecting automatically.", show: true, write: true, forEmail: true, emailType: "EHRConnectedError");
                }
                PostEHRConnectionLog("AbelDent", (bool)e.Result);
                ShowHideLoader(false);
                //btnChatOurSupportTeam.Visible = true;
                lblKBArticle.Visible = true;
            }
            catch (Exception ex)
            {
                ShowWriteError("BwIsAbelDentConnected_RunWorkerCompleted : " + ex.Message, "", write: true, forEmail: true, emailType: "EHRConnectedError");
            }
        }

        private void BwIsAbelDentConnected_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = IsAbelDentConnected(isCalledFromSave == true ? false : true);
            }
            catch (Exception ex)
            {
                ShowWriteError("BwIsAbelDentConnected_DoWork : " + ex.Message, write: true, forEmail: true, emailType: "EHRConnectedError", mailmsg: "AbelDent connection issue - " + ex.Message);
            }
        }

        private void lblKBArticle_Click(object sender, EventArgs e)
        {
            Process.Start("https://help.adit.com/portal/en/kb/articles/how-to-find-my-database-credentials-for-my-ehr");
        }

        private void btnChatOurSupportTeam_Click(object sender, EventArgs e)
        {
            string chatURL = "https://pozative.com/public/pozativeservice/ChatWidget/AditChat.html?email=" + loggedUserEmailID + "&name=" + loggedUserFirstName + " " + loggedUserLastName;
            Process.Start(chatURL);
        }

        private void cmbEHRVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedEHRVersion = cmbEHRVersion.Text.ToLower();
        }

        private void frmConfiguration_Auto_FormClosing(object sender, FormClosingEventArgs e)
        {
            CreateServiceInstallUtil();

            try
            {
                if (Convert.ToString(btnAdminUserSave.Tag) == "1" && (
                    (mailData.LocationConfigurationStatus && mailData.SystemLoginStatus) ||
                    (!mailData.EHRConnectionStatus && mailData.AditAppLoginStatus)))
                {
                    if (mailData.LocationConfigurationStatus && mailData.SystemLoginStatus)
                    {
                        mailData.SystemAppConfigurationStatus = true;
                    }
                    else
                    {
                        if (!mailData.DownloadInstallStatus)
                            mailData.SystemAppConfigurationError = "Error while download & installation.";
                        else if (!mailData.AditAppLoginStatus)
                            mailData.SystemAppConfigurationError = "Adit app credentials fail.";
                        else if (!mailData.EHRConnectionStatus)
                            mailData.SystemAppConfigurationError = "EHR connection not successful.";
                        else if (!mailData.LocationConfigurationStatus)
                            mailData.SystemAppConfigurationError = "Location configuration not successful.";
                        else if (!mailData.SystemLoginStatus)
                            mailData.SystemAppConfigurationError = "System credentials fail.";
                    }

                    if (mailData.AditAppLoginStatus) mailData.AditAppLoginStatusError = string.Empty;
                    if (mailData.EHRConnectionStatus) mailData.EHRConnectedError = string.Empty;
                    if (mailData.LocationConfigurationStatus) mailData.LocationConfigurationError = string.Empty;
                    if (mailData.SystemLoginStatus) mailData.SystemLoginError = string.Empty;

                    //SendEMail email = new SendEMail();
                    //email.SendEmail(loggedUserOraganization, Utility.Location_Name, loggedUserOraganization, installationDateTime,txtAdminUserName.Text);

                    string subject = "Server App Installation Status" + (mailData.SystemAppConfigurationStatus ? " Success" : " Failed");
                    string title2 = string.IsNullOrEmpty(loggedUserOraganization) && string.IsNullOrEmpty(selectedLocations) ? "" : loggedUserOraganization + " - " + selectedLocations; /////string.IsNullOrEmpty(loggedUserOraganization) && string.IsNullOrEmpty(Utility.Location_Name) ? "" : loggedUserOraganization + " - " + Utility.Location_Name;
                    string title1 = mailData.SystemAppConfigurationStatus ? "Successful Server App Install for " : "Failed Server App Install for ";
                    string title3 = mailData.SystemAppConfigurationStatus ? title2 + " had a successful self installation for their server app. "
                                    : (string.IsNullOrEmpty(title2) ? txtAdminUserName.Text + " user" : title2) + " recently attempted to self install their server app, however it was not successful. Please reach out to see how we can help finish their install.";


                    mailData.to = CommonFunction.supportEmail;
                    mailData.MailSubject = subject;
                    mailData.MailTitle = title1;
                    mailData.MailTitle2 = title2;
                    mailData.Description = title3;
                    mailData.heading1 = "";
                    mailData.OrganizationName = loggedUserOraganization;
                    mailData.LocationName = selectedLocations; ////Utility.Location_Name;
                    mailData.OwnerName = loggedUserFirstName + " " + loggedUserLastName;
                    mailData.DateTimeDownloaded = installationDateTime.ToString("MM/dd/yyyy  h:mm tt"); ////installationDateTime.ToString("dd/MM/yyyy");
                    SendMailForStatus();
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("Pozative configuration status mail - " + ex.Message, write: true);
            }
        }
        private void SendMailForStatus()
        {
            try
            {
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(mailData);

                //ShowWriteError("Json String - " + jsonString, write: true);

                string RestURL = SystemBAL.SendEmailEHR();
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("tokenkey", "AditEhrMail");
                request.AddParameter("application/json", jsonString, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.ErrorMessage != null)
                {
                    ShowWriteError("SendMailForStatus response.ErrorMessage - " + response.ErrorMessage, write: true);
                }
            }
            catch (Exception ex)
            {
                ShowWriteError("SendMailForStatus function - " + ex.Message, write: true);
            }
        }
        private void ShowWriteError(string message, string head = "", bool show = false, bool write = false, bool forEmail = false, string emailType = "", string mailmsg = "", string SaveEHRErroLogMsg = "")
        {
            try
            {
                if (show)
                    ObjGoalBase.ErrorMsgBox(head, message);
                if (write)
                    ObjGoalBase.WriteToErrorLogFile(message);
                if (forEmail)
                {
                    switch (emailType)
                    {
                        case "AditAppLoginStatusError":
                            mailData.AditAppLoginStatusError += " " + (string.IsNullOrEmpty(mailmsg) ? message : mailmsg);
                            break;
                        case "EHRConnectedError":
                            mailData.EHRConnectedError += " " + (string.IsNullOrEmpty(mailmsg) ? message : mailmsg);
                            break;
                        case "LocationConfigurationError":
                            mailData.LocationConfigurationError += " " + (string.IsNullOrEmpty(mailmsg) ? message : mailmsg);
                            break;
                        case "SystemLoginError":
                            mailData.SystemLoginError += " " + (string.IsNullOrEmpty(mailmsg) ? message : mailmsg);
                            break;
                        default:
                            break;
                    }
                }
                if (string.IsNullOrEmpty(SaveEHRErroLogMsg))
                    SaveEHRErroLog(errorMessage: SaveEHRErroLogMsg);
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("ShowWriteError - " + ex.Message);
            }
        }

        private void btnSoftDentSave_Click(object sender, EventArgs e)
        {
            bool isConnected = false;
            Utility.EHRHostname = txtSoftDentServerName.Text;
            Utility.EHRIntegrationKey = string.Empty;
            Utility.EHRDatabase = txtSoftDentDatabase.Text;
            Utility.EHRUserId = txtSoftDentUserId.Text;
            Utility.EHRPassword = txtSoftDentPassword.Text;
            Utility.EHRPort = string.Empty;
            string connectionstring = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";Integrated Security=false;User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";Pooling=False;";
            SqlConnection conn = new SqlConnection(connectionstring);
            try
            {
                conn.Open();
                isConnected = true;
            }
            catch (Exception ex)
            {
                isConnected = false;
                ShowWriteError("SoftDent SQL connection open error. ", write: true, forEmail: true, emailType: "EHRConnectedError");
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

            if (isConnected)
            {
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

                ShowWriteError("SoftDent is not connecting. " + "\n" + " Please try again Later or Run Practicework Server exe.", write: true, forEmail: true, emailType: "EHRConnectedError");
            }
        }
        private void ShowWriteInSyncLog(string message, string head = "", bool show = false, bool write = false, bool forEmail = false, string emailType = "", string mailmsg = "", string SaveEHRErroLogMsg = "")
        {
            try
            {
                //if (show)
                //    ObjGoalBase.ErrorMsgBox(head, message);
                if (write)
                    ObjGoalBase.WriteToSyncLogFile(message);
                if (string.IsNullOrEmpty(SaveEHRErroLogMsg))
                    SaveEHRErroLog(errorMessage: SaveEHRErroLogMsg);
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("ShowWriteError - " + ex.Message);
            }
        }
        private void txtOTPDigit4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOTPDigit4.Text))
            {
                string errorMsg = IsOTPVerificationSuccessfuly();
                if (string.IsNullOrEmpty(errorMsg))
                {
                    SetPanelVisiblityEHRwise("OTPStatusSuccess", false);
                }
                else
                {
                    CommonFunction.WrongOTPTrials += 1;
                    SetPanelVisiblityEHRwise("OTPStatusFail", false);
                    if (CommonFunction.WrongOTPTrials == 5)
                    {
                        lblVerificationStatusDesc.Text = "You have exceeded the OTP attempts. Please contact the clinic to regenerate the OTP.";
                        lblVerificationStatusDesc.ForeColor = ColorTranslator.FromHtml("#DA2128");
                        btnOTPVerification.Visible = false;
                    }
                    else
                    {
                        //ShowWriteError(errorMsg, "Adit App OTP Authentication", show: true, write: true, SaveEHRErroLogMsg: "Adit App OTP Authentication" + errorMsg);
                    }
                }
            }
        }
        private string IsOTPVerificationSuccessfuly()
        {
            try
            {
                AditLoginPostOTPBO AditLoginPost = new AditLoginPostOTPBO
                {
                    syncappcode = txtOTPDigit1.Text + txtOTPDigit2.Text + txtOTPDigit3.Text + txtOTPDigit4.Text
                };
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(AditLoginPost);

                string RestURL = SystemBAL.IsValidOTP();
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddParameter("application/json", jsonString, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                if (response.ErrorMessage != null)
                {
                    return "ErrorMessage - " + response.ErrorMessage;
                }
                var AdminLoginDto = JsonConvert.DeserializeObject<AdminLoginDetailBO>(response.Content);
                if (AdminLoginDto == null)
                {
                    return response.ErrorMessage.ToString();
                }
                else if (AdminLoginDto != null && !string.IsNullOrEmpty(AdminLoginDto.status) && !Convert.ToBoolean(AdminLoginDto.status))
                {
                    return AdminLoginDto.message.ToString();
                }
                else
                {
                    adminLoginDetailBO_OTP = AdminLoginDto;
                    return "";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("IsOTPVerificationSuccessfuly - " + ex.Message);
            }
            return "";
        }
        private void btnOTPVerification_Click(object sender, EventArgs e)
        {
            cButton btn = (cButton)sender;
            if (Convert.ToString(btn.Tag) == "Start")
            {
                LoginSuccess(adminLoginDetailBO_OTP);
                //SetPanelVisiblityEHRwise("AdminUser", false);
            }
            else
            {
                txtOTPDigit1.Text = "";
                txtOTPDigit2.Text = "";
                txtOTPDigit3.Text = "";
                txtOTPDigit4.Text = "";
                SetPanelVisiblityEHRwise("EnterOTP", false);
            }
        }
        private void txtOTPDigit1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOTPDigit1.Text))
            {
                txtOTPDigit2.Focus();
            }
        }
        private void txtOTPDigit2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOTPDigit2.Text))
            {
                txtOTPDigit3.Focus();
            }
        }
        private void txtOTPDigit3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOTPDigit3.Text))
            {
                txtOTPDigit4.Focus();
            }
        }
        private void txtOTPDigit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Back))
                e.Handled = false;
            else if (e.KeyChar == ((char)Keys.Enter))
                e.Handled = true;
            else
                e.Handled = !char.IsDigit(e.KeyChar);
        }

        private void LoginSuccess(AdminLoginDetailBO AdminLoginDto)
        {
            try
            {
                try
                {
                    txtAdminUserName.Text = string.IsNullOrEmpty(txtAdminUserName.Text.Trim()) ? AdminLoginDto.data.user_name : txtAdminUserName.Text.Trim();
                    Utility.Adit_User_Email_Id = txtAdminUserName.Text.Trim();
                    Utility.Adit_User_Email_Password = txtAdminPassword.Text.Trim();
                    Utility.WebAdminUserToken = AdminLoginDto.Token;
                    Utility.User_ID = AdminLoginDto.data._id;
                    loggedUserEmailID = AdminLoginDto.data.email;
                    loggedUserFirstName = AdminLoginDto.data.first_name;
                    loggedUserLastName = AdminLoginDto.data.last_name;
                    loggedUserOraganization = AdminLoginDto.data.organizations[0].name;
                    loggedUserOraganizationID = AdminLoginDto.data.organizations[0].id;
                }
                catch (Exception ex)
                {
                    ShowWriteError("There was an issue with connecting to the Adit Server. Please wait a couple minutes and then try again.", "Adit Server", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Server is down login failed." + ex.Message, "GetAdminUserLoginEmailIdPass " + ex.Message);
                    return;
                }

                UserAditLocationLinkList = string.Empty;
                LocationDetailBO customerDto = null;
                try
                {
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
                        email = txtAdminUserName.Text.ToLower().Trim(),
                        password = txtAdminPassword.Text.Trim(),
                        created_by = Utility.User_ID
                    };
                    var javaScriptSerializer1 = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonString1 = javaScriptSerializer1.Serialize(AditLocOrgPost);
                    request.AddHeader("cache-control", "no-cache");
                    if (string.IsNullOrEmpty(txtAdminPassword.Text.Trim()))
                        request.AddHeader("x-for-template", "aditapp-ehr-api");
                    request.AddParameter("application/json", jsonString1, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    if (response.ErrorMessage != null)
                    {
                        ShowWriteError(response.ErrorMessage, "Adit App Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit App getting details of location & organization failed." + response.ErrorMessage, "Adit App Admin User Authentication" + response.ErrorMessage);
                        return;
                    }

                    UserAditLocationLinkList = response.Content;
                    customerDto = JsonConvert.DeserializeObject<LocationDetailBO>(response.Content);
                    if (!string.IsNullOrEmpty(customerDto.error) && customerDto.data == null)
                    {

                        ShowWriteError(customerDto.message, "Adit App Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit App getting details of location & organization failed." + customerDto.message, "Adit App Admin User Authentication" + customerDto.message);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowWriteError(ex.Message, "Adit App Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit App getting details of location & organization failed." + ex.Message, "GetApiAditLocationAndOrganizationByAdminIdPassword " + ex.Message);
                    return;
                }

                dtTempApptLocationTable.Clear();
                DataRow drApptLocDef = dtTempApptLocationTable.NewRow();
                drApptLocDef["id"] = "0";
                drApptLocDef["name"] = " Select ";
                dtTempApptLocationTable.Rows.Add(drApptLocDef);
                dtTempApptLocationTable.AcceptChanges();

                for (int i = 0; i < customerDto.data.Count; i++)
                {
                    if (!setupFromOTP || (setupFromOTP && customerDto.data[i]._id.ToString() == AdminLoginDto.data.syncLocationId))
                    {
                        DataRow drApptLoc = dtTempApptLocationTable.NewRow();
                        drApptLoc["id"] = customerDto.data[i]._id.ToString();
                        drApptLoc["name"] = customerDto.data[i].name.ToString().Trim();
                        drApptLoc["system_mac_address"] = Convert.ToString(customerDto.data[i].system_mac_address);
                        dtTempApptLocationTable.Rows.Add(drApptLoc);
                        dtTempApptLocationTable.AcceptChanges();
                    }
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
                    CheckEHRConnectionAndVisibility();
                    SetPanelVisiblityEHRwise("Main", false);
                }
                SaveEHRErroLog();
                mailData.AditAppLoginStatus = true;
            }
            catch (Exception ex)
            {
                ShowWriteError("Login Success - " + ex.Message, "Admin User Authentication", show: true, write: true, forEmail: true, emailType: "AditAppLoginStatusError", mailmsg: "Adit user authentication issue." + ex.Message, "btnAdminUserSave_click " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void cButton1_Click(object sender, EventArgs e)
        {
            lblsharedpath.Text = "";
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select a folder";
                //folderDialog.RootFolder = Environment.SpecialFolder.CommonDesktopDirectory;
                folderDialog.ShowNewFolderButton = false;

                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    btnSharingPathSave.Enabled = true;
                    picSharedPath.Visible = false;
                    pnltextsharedpath.Visible = true;
                    lblfolderpath.Visible = true;
                    lblsharedpath.Visible = true;
                    Utility.EHRDocPath = folderDialog.SelectedPath;
                    lblsharedpath.Text = Utility.EHRDocPath;
                }
            }
        }

        private void btnSharingPathSave_Click(object sender, EventArgs e)
        {
            if (lblsharedpath.Text != "")
                SetPanelVisiblityEHRwise("Location", false);
        }

        private void lblSkipPopupClose_Click(object sender, EventArgs e)
        {
            HideSkipPopUp();
        }
        private void btnSkipPopupCancel_Click(object sender, EventArgs e)
        {
            HideSkipPopUp();
        }
        private void HideSkipPopUp()
        {
            pnlSkipPopup.Visible = false;
            this.Size = new Size(900, 650);
            pnlSkipPopup.Location = new Point(0, 0);
            pnlSkipPopup.SendToBack();
        }
        private void lblSystemCredentialsInfoToolTip_MouseHover(object sender, EventArgs e)
        {
            pnlToolTipSystemCredentials.Visible = true;
            pnlToolTipSystemCredentials.BringToFront();
            pnlToolTipSystemCredentials.Location = new Point(161, 373);
        }

        private void lblSystemCredentialsInfoToolTip_MouseLeave(object sender, EventArgs e)
        {
            pnlToolTipSystemCredentials.Visible = false;
        }

        private void btnSkipPopupConfirm_Click(object sender, EventArgs e)
        {
            pnlSkipPopup.Visible = false;
            this.Size = new Size(900, 650);
            pnlSkipPopup.Location = new Point(0, 0);
            pnlSkipPopup.SendToBack();
            SkipSystemCredential = true;
            btnSystemUserNextClick();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!isInstallationComplete)
            {
                ShowWriteError("Configuration is in progress please wait...",show:true);
            }
            else
            {
                ConfigurationComplete();
            }
        }

        private void chkIsZohoAssistAllowed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsZohoAssistAllowed.Checked)
            {
                lblOrangeTick.Visible = true;
            }
            else
            {
                lblOrangeTick.Visible = false;
            }
            CheckSystemCredentialsExist();
        }

        private void lblOrangeTick_Click(object sender, EventArgs e)
        {
            chkIsZohoAssistAllowed.Checked = false;
        }
        private void CheckSystemCredentialsExist()
        {
            if (!string.IsNullOrEmpty(cmbSystemUser.Text) && !string.IsNullOrEmpty(txtSystemUserPassword.Text) && chkIsZohoAssistAllowed.CheckState == CheckState.Checked)
            {
                btnSystemUserNext.Enabled = true;
            }
            else
            {
                btnSystemUserNext.Enabled = false;
            }
        }

        private void cmbSystemUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSystemCredentialsExist();
        }

        private void txtSystemUserPassword_TextChanged(object sender, EventArgs e)
        {
            CheckSystemCredentialsExist();
        }
    }
}
