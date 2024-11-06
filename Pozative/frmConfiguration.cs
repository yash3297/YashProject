using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pozative.BAL;
using Pozative.DAL;
using System.Net;
using System.Runtime.InteropServices;
using RestSharp;
using Pozative.BO;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using Pozative.UTL;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Net.Security;
using Pozative.Properties;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Data.Sql;
using System.Data.SqlClient;
using Pozative.CommonClass;

namespace Pozative
{
    public partial class frmConfiguration : Form
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

        #region Form Load

        public frmConfiguration(string tmpIsAction)
        {
            InitializeComponent();
            IsAction = tmpIsAction;
        }

        private void frmEaglesoftConfiguration_Load(object sender, EventArgs e)
        {
            try
            {
                Utility.DBConnString = "";
                Cursor.Current = Cursors.WaitCursor;
                this.Height = 350;
                this.Width = 600;

                SetFormControlsDesign();
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

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Common Function

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
                ObjGoalBase.WriteToSyncLogFile("Error_Find_CreateSqlDatabase " + ex.Message.ToString());
                throw;
            }
        }

        private DataTable GetEHRList()
        {
            DataTable dtEHRList = new DataTable();
            dtEHRList.Clear();
            dtEHRList.Columns.Add("EHR_ID", typeof(Int32));
            dtEHRList.Columns.Add("EHR_Name", typeof(string));
            dtEHRList.Rows.Add(0, "<Select>");
            dtEHRList.Rows.Add(1, "Eaglesoft");
            dtEHRList.Rows.Add(2, "Open Dental");
            dtEHRList.Rows.Add(3, "Dentrix");
            dtEHRList.Rows.Add(4, "SoftDent");
            dtEHRList.Rows.Add(5, "ClearDent");
            dtEHRList.Rows.Add(6, "Tracker");
            dtEHRList.Rows.Add(7, "PracticeWork");
            dtEHRList.Rows.Add(8, "Easy Dental");
            dtEHRList.Rows.Add(9, "Opendental Cloud");
            dtEHRList.Rows.Add(10, "PracticeWeb");
            dtEHRList.Rows.Add(11, "AbelDent");
            dtEHRList.AcceptChanges();
            return dtEHRList;
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

        private DataTable GetEaglesoftVersionList()
        {
            DataTable dtEaglesoftVersionList = new DataTable();
            dtEaglesoftVersionList.Clear();
            dtEaglesoftVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtEaglesoftVersionList.Columns.Add("Version_Name", typeof(string));
            dtEaglesoftVersionList.Rows.Add(0, "<Select>");
            dtEaglesoftVersionList.Rows.Add(1, "17");
            dtEaglesoftVersionList.Rows.Add(2, "18.0");
            dtEaglesoftVersionList.Rows.Add(3, "19.11");
            dtEaglesoftVersionList.Rows.Add(4, "20.0");
            dtEaglesoftVersionList.Rows.Add(5, "21.20");
            dtEaglesoftVersionList.Rows.Add(6, "22.00");
            dtEaglesoftVersionList.AcceptChanges();
            return dtEaglesoftVersionList;
        }

        private DataTable GetOpenDentalVersionList()
        {
            DataTable dtOpenDentalVersionList = new DataTable();
            dtOpenDentalVersionList.Clear();
            dtOpenDentalVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtOpenDentalVersionList.Columns.Add("Version_Name", typeof(string));
            dtOpenDentalVersionList.Rows.Add(0, "<Select>");
            dtOpenDentalVersionList.Rows.Add(1, "15.4");
            dtOpenDentalVersionList.Rows.Add(2, "17.2+");
            dtOpenDentalVersionList.AcceptChanges();
            return dtOpenDentalVersionList;
        }
        private DataTable GetPracticeWebVersionList()
        {
            DataTable dtOpenDentalVersionList = new DataTable();
            dtOpenDentalVersionList.Clear();
            dtOpenDentalVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtOpenDentalVersionList.Columns.Add("Version_Name", typeof(string));
            dtOpenDentalVersionList.Rows.Add(0, "<Select>");
            dtOpenDentalVersionList.Rows.Add(1, "21.1");
            dtOpenDentalVersionList.AcceptChanges();
            return dtOpenDentalVersionList;
        }

        private DataTable GetAbelDentVersionList()
        {
            DataTable dtOpenDentalVersionList = new DataTable();
            dtOpenDentalVersionList.Clear();
            dtOpenDentalVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtOpenDentalVersionList.Columns.Add("Version_Name", typeof(string));
            dtOpenDentalVersionList.Rows.Add(0, "<Select>");
            dtOpenDentalVersionList.Rows.Add(1, "14.4.2");
            dtOpenDentalVersionList.Rows.Add(2, "14.8.2");
            dtOpenDentalVersionList.AcceptChanges();
            return dtOpenDentalVersionList;
        }

        private DataTable GetDentrixVersionList()
        {
            DataTable dtDentrixVersionList = new DataTable();
            dtDentrixVersionList.Clear();
            dtDentrixVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtDentrixVersionList.Columns.Add("Version_Name", typeof(string));
            dtDentrixVersionList.Rows.Add(0, "<Select>");
            dtDentrixVersionList.Rows.Add(1, "DTX G5");
            dtDentrixVersionList.Rows.Add(2, "DTX G6");
            dtDentrixVersionList.Rows.Add(3, "DTX G6.2+");
            dtDentrixVersionList.AcceptChanges();
            return dtDentrixVersionList;
        }

        private DataTable GetSoftDentVersionList()
        {
            DataTable dtSoftdentVersionList = new DataTable();
            dtSoftdentVersionList.Clear();
            dtSoftdentVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtSoftdentVersionList.Columns.Add("Version_Name", typeof(string));
            dtSoftdentVersionList.Rows.Add(0, "<Select>");
            dtSoftdentVersionList.Rows.Add(1, "17.0.0+");
            dtSoftdentVersionList.AcceptChanges();
            return dtSoftdentVersionList;
        }

        private DataTable GetClearDentVersionList()
        {
            DataTable dtClearDentVersionList = new DataTable();
            dtClearDentVersionList.Clear();
            dtClearDentVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtClearDentVersionList.Columns.Add("Version_Name", typeof(string));
            dtClearDentVersionList.Rows.Add(0, "<Select>");
            dtClearDentVersionList.Rows.Add(1, "9.8+");
            dtClearDentVersionList.Rows.Add(2, "9.10+");

            dtClearDentVersionList.AcceptChanges();
            return dtClearDentVersionList;
        }

        private DataTable GetTrackerVersionList()
        {
            DataTable dtTrackerVersionList = new DataTable();
            dtTrackerVersionList.Clear();
            dtTrackerVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtTrackerVersionList.Columns.Add("Version_Name", typeof(string));
            dtTrackerVersionList.Rows.Add(0, "<Select>");
            dtTrackerVersionList.Rows.Add(1, "11.29");

            dtTrackerVersionList.AcceptChanges();
            return dtTrackerVersionList;
        }

        private DataTable GetPracticeWorkVersionList()
        {
            DataTable dtPracticeWorkVersionList = new DataTable();
            dtPracticeWorkVersionList.Clear();
            dtPracticeWorkVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtPracticeWorkVersionList.Columns.Add("Version_Name", typeof(string));
            dtPracticeWorkVersionList.Rows.Add(0, "<Select>");
            dtPracticeWorkVersionList.Rows.Add(1, "7.9+");

            dtPracticeWorkVersionList.AcceptChanges();
            return dtPracticeWorkVersionList;
        }
        private DataTable GetEasyDentalVersionList()
        {
            DataTable dtEasyDentalVersionList = new DataTable();
            dtEasyDentalVersionList.Clear();
            dtEasyDentalVersionList.Columns.Add("Version_ID", typeof(Int32));
            dtEasyDentalVersionList.Columns.Add("Version_Name", typeof(string));
            dtEasyDentalVersionList.Rows.Add(0, "<Select>");
            dtEasyDentalVersionList.Rows.Add(1, "11.1");

            dtEasyDentalVersionList.AcceptChanges();
            return dtEasyDentalVersionList;
        }

        private void SetFormControlsDesign()
        {
            lblFormHead.Anchor = System.Windows.Forms.AnchorStyles.Left;

            this.BackColor = WDSColor.FormHeadBackColor;
            tblformHead.BackColor = WDSColor.FormHeadBackColor;
            lblFormHead.ForeColor = WDSColor.FormHeadForeColor;
            tblViewBody.BackColor = Color.White;
            btnClose.ForeColor = WDSColor.FormHeadForeColor;

            btnEHRSave.BackColor = WDSColor.SaveButtonBackColor;
            btnEHRCancel.BackColor = WDSColor.ButtonBackColor;

            btnEaglesoftSave.BackColor = WDSColor.SaveButtonBackColor;
            btnEaglesoftBack.BackColor = WDSColor.ButtonBackColor;

            btnOpenDentalSave.BackColor = WDSColor.SaveButtonBackColor;
            btnOpenDentalBack.BackColor = WDSColor.ButtonBackColor;

            btnClearDentSave.BackColor = WDSColor.SaveButtonBackColor;
            btnClearDentBack.BackColor = WDSColor.ButtonBackColor;

            btnDentrixSave.BackColor = WDSColor.SaveButtonBackColor;
            btnDentrixBack.BackColor = WDSColor.ButtonBackColor;

            btnTrackerSave.BackColor = WDSColor.SaveButtonBackColor;
            btnTrackerBack.BackColor = WDSColor.ButtonBackColor;

            btnAdminUserSave.BackColor = WDSColor.SaveButtonBackColor;
            btnAdminUserBack.BackColor = WDSColor.ButtonBackColor;

            btnLocationSave.BackColor = WDSColor.SaveButtonBackColor;
            btnLocationBack.BackColor = WDSColor.ButtonBackColor;

            btnEHRSave.ForeColor = WDSColor.SaveButtonForeColor;
            btnEHRCancel.ForeColor = WDSColor.ButtonForeColor;

            btnEaglesoftSave.ForeColor = WDSColor.SaveButtonForeColor;
            btnEaglesoftBack.ForeColor = WDSColor.ButtonForeColor;

            btnOpenDentalSave.ForeColor = WDSColor.SaveButtonForeColor;
            btnOpenDentalBack.ForeColor = WDSColor.ButtonForeColor;

            btnClearDentSave.ForeColor = WDSColor.SaveButtonForeColor;
            btnClearDentBack.ForeColor = WDSColor.ButtonForeColor;

            btnDentrixSave.ForeColor = WDSColor.SaveButtonForeColor;
            btnDentrixBack.ForeColor = WDSColor.ButtonForeColor;

            btnAdminUserSave.ForeColor = WDSColor.SaveButtonForeColor;
            btnAdminUserBack.ForeColor = WDSColor.ButtonForeColor;

            btnLocationSave.ForeColor = WDSColor.SaveButtonForeColor;
            btnLocationBack.ForeColor = WDSColor.ButtonForeColor;

            btnEHRSave.FlatStyle = FlatStyle.Flat;
            btnEHRCancel.FlatStyle = FlatStyle.Flat;

            btnEaglesoftSave.FlatStyle = FlatStyle.Flat;
            btnEaglesoftBack.FlatStyle = FlatStyle.Flat;

            btnOpenDentalSave.FlatStyle = FlatStyle.Flat;
            btnOpenDentalBack.FlatStyle = FlatStyle.Flat;

            btnClearDentSave.FlatStyle = FlatStyle.Flat;
            btnClearDentBack.FlatStyle = FlatStyle.Flat;

            btnDentrixSave.FlatStyle = FlatStyle.Flat;
            btnDentrixBack.FlatStyle = FlatStyle.Flat;

            btnTrackerSave.FlatStyle = FlatStyle.Flat;
            btnTrackerBack.FlatStyle = FlatStyle.Flat;

            btnAdminUserSave.FlatStyle = FlatStyle.Flat;
            btnAdminUserBack.FlatStyle = FlatStyle.Flat;

            btnLocationSave.FlatStyle = FlatStyle.Flat;
            btnLocationBack.FlatStyle = FlatStyle.Flat;

            btnEHRSave.Font = WDSColor.FormButtonFont;
            btnEHRCancel.Font = WDSColor.FormButtonFont;

            btnEaglesoftSave.Font = WDSColor.FormButtonFont;
            btnEaglesoftBack.Font = WDSColor.FormButtonFont;

            btnOpenDentalSave.Font = WDSColor.FormButtonFont;
            btnOpenDentalBack.Font = WDSColor.FormButtonFont;

            btnClearDentSave.Font = WDSColor.FormButtonFont;
            btnClearDentBack.Font = WDSColor.FormButtonFont;

            btnTrackerSave.Font = WDSColor.FormButtonFont;
            btnTrackerBack.Font = WDSColor.FormButtonFont;

            btnDentrixSave.Font = WDSColor.FormButtonFont;
            btnDentrixBack.Font = WDSColor.FormButtonFont;

            btnAdminUserSave.Font = WDSColor.FormButtonFont;
            btnAdminUserBack.Font = WDSColor.FormButtonFont;

            btnLocationSave.Font = WDSColor.FormButtonFont;
            btnLocationBack.Font = WDSColor.FormButtonFont;

            cmbAditLocation.IntegralHeight = true;
            cmbAditLocation.DropDownHeight = 200;

            cmbPozativeLocation.IntegralHeight = true;
            cmbPozativeLocation.DropDownHeight = 200;

            //lblEHRHead.Text = "Please select your Patient Management System";
            //lblLocationHead.Text = "Please Select The Location You Would Like To Sync";

        }

        private void bindFormControls()
        {
            tblEaglesoftMain.Dock = DockStyle.Fill;
            tblOpenDentalMain.Dock = DockStyle.Fill;
            tblClearDentMain.Dock = DockStyle.Fill;
            tblDentrixMain.Dock = DockStyle.Fill;
            tblAdminUserMain.Dock = DockStyle.Fill;
            tblLocationMain.Dock = DockStyle.Fill;
            tblEHRMain.Dock = DockStyle.Fill;
            tblpnTracker.Dock = DockStyle.Fill;
            tblPracticeWork.Dock = DockStyle.Fill;
            tblpnPracticeWeb.Dock = DockStyle.Fill;
            tblpnAbelDent.Dock = DockStyle.Fill;

            tblEaglesoftMain.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblClearDentMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblAdminUserMain.Visible = false;
            tblLocationMain.Visible = false;
            tblEHRMain.Visible = true;
            tblpnTracker.Visible = false;
            tblPracticeWork.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblpnAbelDent.Visible = false;

            cmbEHRName.DataSource = GetEHRList();
            cmbEHRName.ValueMember = "EHR_ID";
            cmbEHRName.DisplayMember = "EHR_Name";
            cmbEHRName.SelectedIndex = 0;
            cmbEHRName.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEHRVersion.DropDownStyle = ComboBoxStyle.DropDownList;

            cboDatabaseList.DataSource = GetDatabaseList();
            cboDatabaseList.ValueMember = "DatabaseId";
            cboDatabaseList.DisplayMember = "DatabaseName";
            cboDatabaseList.SelectedIndex = 0;
            cboDatabaseList.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDatabaseList.DropDownStyle = ComboBoxStyle.DropDownList;
            //GetDatabaseList

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

            lblFormHead.Text = "Please select your Patient Management System";
            lblFormHead.Anchor = AnchorStyles.Left;

            cmbTrackerHostName.DataSource = null;
            cmbSqlServiceName.DataSource = null;

        }

        private DataTable CreateTempOrgTableData()
        {
            DataTable dtOrg = new DataTable();
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
            return dtOrg;
        }

        private DataTable CreateTempLocationTableData()
        {
            DataTable dtLoc = new DataTable();
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
            return dtLoc;

        }

        private DataTable CreateTempAppointmentLocationTableData()
        {
            DataTable dtApptLoc = new DataTable();
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
            return dtApptLoc;
        }

        private DataTable CreateTempPozativeLocationTableData()
        {
            DataTable dtPozativeLoc = new DataTable();
            dtPozativeLoc.Clear();
            dtPozativeLoc.Columns.Add("id", typeof(string));
            dtPozativeLoc.Columns.Add("locationname", typeof(string));
            dtPozativeLoc.Columns.Add("machineid", typeof(string));
            dtPozativeLoc.AcceptChanges();
            return dtPozativeLoc;

        }

        private void ViewtblEaglesoftPanel()
        {
            tblAdminUserMain.Visible = false;
            tblEHRMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblLocationMain.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblClearDentMain.Visible = false;
            tblpnTracker.Visible = false;
            tblpnAbelDent.Visible = false;
            tblEaglesoftMain.Visible = true;
            label6.Visible = false;
            label7.Visible = false;
            txtEaglesoftUserId.Visible = false;
            txtEaglesoftPassword.Visible = false;
            txtEaglesoftHostName.Focus();
            txtEaglesoftHostName.Text = string.Empty;
            lblEaglesoftHostname.Text = "EagleSoft ServerName";
            lblFormHead.Text = "Eaglesoft Configuration";
        }

        private void ViewtblOpenDentalPanel()
        {
            tblAdminUserMain.Visible = false;
            tblEHRMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblLocationMain.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblClearDentMain.Visible = false;
            tblpnTracker.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblpnAbelDent.Visible = false;
            tblOpenDentalMain.Visible = true;
            txtOpenDentalPassword.Focus();
            txtOpenDentalPassword.Text = string.Empty;
            lblFormHead.Text = "OpenDental Configuration";
        }

        private void ViewtblPracticeWebPanel()
        {
            tblAdminUserMain.Visible = false;
            tblEHRMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblLocationMain.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblClearDentMain.Visible = false;
            tblpnTracker.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblpnPracticeWeb.Visible = true;
            tblpnAbelDent.Visible = false;
            txtPracticeWebPassword.Focus();
            txtPracticeWebPassword.Text = string.Empty;
            lblFormHead.Text = "PracticeWeb Configuration";
        }

        private void ViewtblAbelDentPanel()
        {
            tblAdminUserMain.Visible = false;
            tblEHRMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblLocationMain.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblClearDentMain.Visible = false;
            tblpnTracker.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblpnAbelDent.Visible = true;            
            cmbAbelDentHostName.DisplayMember = "SqlServerName";            
            cmbAbelDentHostName.Text = ".\\sqlexpress";
            txtAbelDentDatabase.Text = "Abel";
            txtAbelDentUserId.Text = "";
            txtAbelDentPassword.Text = "";
            btnAbeldentSave.Focus();
            lblFormHead.Text = "AbelDent Configuration";

        }

        private void ViewtblClearDentPanel()
        {
            tblAdminUserMain.Visible = false;
            tblEHRMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblLocationMain.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblpnAbelDent.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblpnTracker.Visible = false;
            tblClearDentMain.Visible = true;
            txtClearDentHostName.Text = "(local)\\VSDOTNET";
            txtClearDentUserId.Text = "sa";
            txtClearDentPassowrd.Text = "";
            btnClearDentSave.Focus();
            lblFormHead.Text = "ClearDent Configuration";
        }

        private void ViewtblTrackerPanel()
        {
            tblAdminUserMain.Visible = false;
            tblEHRMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblLocationMain.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblpnAbelDent.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblpnTracker.Visible = true;
            tblClearDentMain.Visible = false;
            GetSqlServerNameANDServiceName();

            cmbTrackerHostName.DataSource = dtSqlServerName;
            cmbTrackerHostName.DisplayMember = "SqlServerName";
            if (dtSqlServerName == null || (dtSqlServerName != null && dtSqlServerName.Rows.Count == 0))
            {
                cmbTrackerHostName.Text = ".\\sqlexpress";
            }

            txtTrackerDatabase.Text = "Tracker";
            txtTrackerCredential.Text = "Adit";
            txtTrackerUserId.Text = "TBNAdmin";
            txtTrackerPassword.Text = "{A827F13E-96B8-11DC-B1E3-A86156D89593}";
            btnTrackerSave.Focus();
            lblFormHead.Text = "Tracker Configuration";
        }

        private void ViewtblDentrixPanel()
        {
            tblAdminUserMain.Visible = false;
            tblEHRMain.Visible = false;
            tblLocationMain.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblClearDentMain.Visible = false;
            tblpnAbelDent.Visible = false;
            tblpnTracker.Visible = false;
            tblDentrixMain.Visible = true;
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

        private void ViewPracticeWorkPanel()
        {
            tblAdminUserMain.Visible = false;
            tblEHRMain.Visible = false;
            tblLocationMain.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblClearDentMain.Visible = false;
            tblpnAbelDent.Visible = false;
            tblpnTracker.Visible = false;
            tblDentrixMain.Visible = false;
            tblPracticeWork.Visible = true;
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

        private void ViewAdminConfigurationPanel()
        {

            //Dilip

            Utility.AditSync = false;
            Utility.PozativeSync = false;

            tblEHRMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblLocationMain.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblClearDentMain.Visible = false;
            tblpnTracker.Visible = false;
            tblPracticeWork.Visible = false;
            tblpnPracticeWeb.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblpnAbelDent.Visible = false;
            tblAdminUserMain.Visible = true;
            txtAdminUserName.Focus();

            SetActivePozativeSync(true, "Adit");
            SetActivePozativeSync(false, "Pozative");
            lblFormHead.Text = "User Login Credentials";

        }

        private void ViewEHRLocationPanel()
        {
            tblEHRMain.Visible = false;
            tblEaglesoftMain.Visible = false;
            tblDentrixMain.Visible = false;
            tblAdminUserMain.Visible = false;
            tblOpenDentalMain.Visible = false;
            tblClearDentMain.Visible = false;
            tblpnTracker.Visible = false;
            tblLocationMain.Visible = true;
            tblpnPracticeWeb.Visible = false;
            tblpnAbelDent.Visible = false;
            lblFormHead.Text = "Please Select The Location You Would Like To Sync";

            if (Utility.AditSync)
            {
                grpAditLoc.Visible = true;
                cmbAditLocation.Focus();
                cmbAditLocation.SelectedIndex = 0;
            }
            else
            {
                grpAditLoc.Visible = false;
            }

            if (Utility.PozativeSync)
            {
                grpPozativeLoc.Visible = true;
                cmbPozativeLocation.SelectedIndex = 0;
            }
            else
            {
                grpPozativeLoc.Visible = false;
            }
        }

        private void SetActivePozativeSync(bool isSyncActive, string ServiceName)
        {
            if (ServiceName.ToLower().ToString() == "Pozative".ToLower().ToString())
            {
                if (isSyncActive)
                {
                    picPozativeSync.Image = Resources.ON;
                    Utility.PozativeSync = true;
                    lblPozativeEmailId.Enabled = true;
                    lblPozativePassword.Enabled = true;
                    txtPozativeEmailID.Text = string.Empty;
                    txtPozativePassword.Text = string.Empty;
                    txtPozativeEmailID.Enabled = true;
                    txtPozativePassword.Enabled = true;
                    btnAdminUserSave.Enabled = true;
                    txtPozativeEmailID.Focus();
                }
                else
                {
                    picPozativeSync.Image = Resources.OFF;
                    Utility.PozativeSync = false;
                    lblPozativeEmailId.Enabled = false;
                    lblPozativePassword.Enabled = false;
                    txtPozativeEmailID.Text = string.Empty;
                    txtPozativePassword.Text = string.Empty;
                    txtPozativeEmailID.Enabled = false;
                    txtPozativePassword.Enabled = false;

                    if (Utility.AditSync == false)
                    {
                        btnAdminUserSave.Enabled = false;
                    }
                }
            }
            else
            {
                if (isSyncActive)
                {
                    picAditSync.Image = Resources.ON;
                    Utility.AditSync = true;

                    lblAditEmailId.Enabled = true;
                    lblAditPassword.Enabled = true;
                    txtAdminUserName.Text = string.Empty;
                    txtAdminPassword.Text = string.Empty;
                    txtAdminUserName.Enabled = true;
                    txtAdminPassword.Enabled = true;
                    btnAdminUserSave.Enabled = true;
                    txtAdminUserName.Focus();
                }
                else
                {
                    picAditSync.Image = Resources.OFF;
                    Utility.AditSync = false;

                    lblAditEmailId.Enabled = false;
                    lblAditPassword.Enabled = false;
                    txtAdminUserName.Text = string.Empty;
                    txtAdminPassword.Text = string.Empty;
                    txtAdminUserName.Enabled = false;
                    txtAdminPassword.Enabled = false;

                    if (Utility.PozativeSync == false)
                    {
                        btnAdminUserSave.Enabled = false;
                    }
                }
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
                                    ViewtblEaglesoftPanel();
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

        //public string FindSqlAnywhereName(string sqlName)
        //{
        //    try
        //    {
        //        string sqlAnyWhereName = "";
        //        switch (sqlName.ToUpper())
        //        {                    
        //            case "ADAPTIVE SERVER ANYWHERE 7.0":
        //                sqlAnyWhereName = "SQL Anywhere 7";
        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 8.0":
        //                sqlAnyWhereName = "SQL Anywhere 8";
        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 9.0":
        //                sqlAnyWhereName = "SQL Anywhere 9";
        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 10.0":
        //                sqlAnyWhereName = "SQL Anywhere 10";
        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 11.0":

        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 12.0":

        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 13.0":

        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 14.0":

        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 15.0":

        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 16.0":

        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 17.0":

        //                break;
        //            case "ADAPTIVE SERVER ANYWHERE 18.0":

        //                break;
        //        }
        //    }
        //    catch (Exception)
        //    {                
        //        throw;
        //    }
        //}

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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #region EHR Configuration

        private void btnEHRSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                dtTempOpenDentalClinicTable.Rows.Clear();
                RBtnSingleClinic.Checked = true;
                RBtnMultiClinic.Checked = false;

                RBtnSingleClinic.Visible = false;
                RBtnMultiClinic.Visible = false;

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

                switch (cmbEHRName.SelectedIndex)
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
                                    Utility.DBConnString = sr.ReadToEnd().ToString().Trim();
                                    EaglesoftConStr = Utility.DBConnString;
                                    if (cmbEHRVersion.Text.ToLower() != "21.20".ToLower())
                                    {
                                        EaglesoftConStr = Utility.DecryptString(Utility.DBConnString);
                                    }
                                }
                                //IsEHRConnected = SystemBAL.GetEHREagleSoftConnection();
                            }
                            catch (Exception)
                            {

                            }


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


                            Utility.APISessionToken = "";
                            Utility.EHRHostname = "";
                            Utility.EHRIntegrationKey = "";
                            Utility.EHRUserId = "";
                            Utility.EHRPassword = "";
                            Utility.EHRDatabase = "Primary Database";
                            Utility.EHRPort = string.Empty;
                            Utility.DBConnString = EaglesoftConStr;
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
                            #region Create DSN For EagleSoft
                            if (CreateDSN())
                            {
                                //  EaglesoftConStr = "SET TEMPORARY OPTION CONNECTION_AUTHENTICATION=" + EaglesoftConStr;


                                //string[] StrData = EaglesoftConStr.Split(';');

                                //if (StrData.Count() > 0)
                                //{
                                //    Utility.EHRDatabase = StrData[0].ToString().Substring(4);
                                //    Utility.EHRHostname = StrData[1].ToString().Substring(4);
                                //    Utility.EHRUserId = StrData[2].ToString().Substring(4);
                                //    Utility.EHRPassword = StrData[3].ToString().Substring(4);
                                //}

                                ViewAdminConfigurationPanel();
                                //*Commented EagleSoft NSite* (Start)
                                //ShowEagleSoftMultiSite();
                                //*Commented EagleSoft NSite* (End)
                            }
                            else
                            {
                                ViewtblEaglesoftPanel();
                            }
                            #endregion
                        }                       
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;

                    case 2:
                        if (cmbEHRVersion.Text.ToLower() == "15.4".ToLower() || cmbEHRVersion.Text.ToLower() == "17.2+".ToLower())
                        {
                            ViewtblOpenDentalPanel();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;

                    case 3:
                        //  btnDentrixSave_Click(null, null);

                        if (cmbEHRVersion.Text.ToLower() == "DTX G5".ToLower() || cmbEHRVersion.Text.ToLower() == "DTX G6".ToLower() || cmbEHRVersion.Text.ToLower() == "DTX G6.2+".ToLower())
                        {
                            ViewtblDentrixPanel();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;

                    case 4:
                        if (cmbEHRVersion.Text.ToLower() == "17.0.0+".ToLower())
                        {
                            SaveSoftDentConfigurtion();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;
                    case 5:

                        if (cmbEHRVersion.Text.ToLower() == "9.8+".ToLower() || cmbEHRVersion.Text.ToLower() == "9.10+".ToLower())
                        {
                            //ClearDentConfigurtion();
                            ViewtblClearDentPanel();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;
                    case 6:

                        if (cmbEHRVersion.Text.ToLower() == "11.29".ToLower())
                        {
                            cmbTrackerHostName.DataSource = null;
                            cmbSqlServiceName.DataSource = null;
                            ViewtblTrackerPanel();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;
                    case 7:
                        // MessageBox.Show("Click to save");
                        if (cmbEHRVersion.Text.ToLower() == "7.9+".ToLower())
                        {
                            //   MessageBox.Show("Show Practicework Panel " + cmbEHRVersion.Text.ToLower());
                            ViewPracticeWorkPanel();
                            // MessageBox.Show("Show Practicework Panel shown" + cmbEHRVersion.Text.ToLower());
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;
                    case 8:
                        if (cmbEHRVersion.Text.ToLower() == "11.1".ToLower())
                        {
                            Utility.DBConnString = EasyDentalConnection.GetEasyDentalConnection().ConnectionString;
                            ViewAdminConfigurationPanel();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;
                    case 9:
                        if (cmbEHRVersion.Text.ToLower() == "15.4".ToLower() || cmbEHRVersion.Text.ToLower() == "17.2+".ToLower())
                        {
                            ViewtblOpenDentalPanel();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;
                    case 10:
                        if (cmbEHRVersion.Text.ToLower() == "21.1".ToLower())
                        {
                            ViewtblPracticeWebPanel();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;
                    case 11:
                        if (cmbEHRVersion.Text.ToLower() == "14.4.2".ToLower() || cmbEHRVersion.Text.ToLower() == "14.8.2".ToLower())
                        {
                            cmbSqlServiceName.DataSource = null;
                            ViewtblAbelDentPanel();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " version " + cmbEHRVersion.Text.ToLower() + " is underdevelopment." + "\n" + "Please select another " + cmbEHRName.Text.ToString() + " Version.");
                            cmbEHRVersion.Focus();
                        }
                        break;
                    default:
                        ObjGoalBase.ErrorMsgBox("Pozative Service integration", "Pozative Service integration with " + cmbEHRName.Text.ToString() + " is underdevelopment." + "\n" + "Please select another EHR Name.");
                        cmbEHRName.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", ex.Message);
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
                Cursor.Current = Cursors.WaitCursor;
                ObjGoalBase.WriteToSyncLogFile("[btnEHRCancel_Click] manually Application restart");
                System.Environment.Exit(1);
            }
            catch (Exception)
            {
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

                //  Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
                //  dynamic settings = Activator.CreateInstance(esSettingsType);

                ////  bool tokenIsValid = settings.SetToken("Adit", "4a6d7243007e528f1789a2c13ffa578d936914df726801f4010d9f2a59cc0cf4");
                //  bool tokenIsValid = settings.SetToken("EagleSoft", "37444a4f485652495700b506f62098957dec9a1a8e5401f4141b8b8870552340");

                //  string EaglesoftConStr = settings.GetLegacyConnectionString(true);


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
                    Utility.EHRDocPath = SynchEaglesoftBAL.GetEagleSoftDocPath(Utility.DBConnString);
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
                        ObjGoalBase.InformationMsgBox("Auto create DSN", "DSN might not been created Automatically... Please create DSN manually after configuration");
                    }
                }
                catch (Exception)
                {

                }

                ViewAdminConfigurationPanel();
                //*Commented EagleSoft NSite* (Start)
                //ShowEagleSoftMultiSite();
                //*Commented EagleSoft NSite* (End)
                #endregion
            }
            else
            {
                string err = string.Empty;
                string result = string.Empty;
                try
                {
                    var response = await SynchEaglesoftDAL.Authenticate(txtEaglesoftHostName.Text.Trim(), strEaglesoftIntegrationKey, txtEaglesoftUserId.Text.Trim(), txtEaglesoftPassword.Text.Trim());
                    if (response.StatusCode == HttpStatusCode.OK)
                        result = response.Content.ReadAsStringAsync().Result.Replace("\"", "");
                    else
                    {
                        result = response.Content.ReadAsStringAsync().Result.Replace("\"", "");
                        err = response.Content.ReadAsStringAsync().Result.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("Message:", "");
                    }
                    if (err == string.Empty)
                    {
                        Utility.APISessionToken = result;
                        Utility.EHRHostname = txtEaglesoftHostName.Text.ToLower().Trim();
                        Utility.EHRIntegrationKey = strEaglesoftIntegrationKey;
                        Utility.EHRUserId = txtEaglesoftUserId.Text.Trim();
                        Utility.EHRPassword = txtEaglesoftPassword.Text.Trim();
                        Utility.EHRDatabase = string.Empty;
                        Utility.EHRPort = string.Empty;

                        //btnEaglesoftConfigSave_Click(null, null); 
                        ViewAdminConfigurationPanel();
                    }
                    else
                    {
                        ObjGoalBase.ErrorMsgBox("Authentication", err.ToString());
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void btnEaglesoftBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bindFormControls();
            }
            catch (Exception)
            {
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
                        //Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                        //btnOpenDentalSave_Click(null, null);
                        ViewAdminConfigurationPanel();

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
                    }
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnOpenDentalBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bindFormControls();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Dentrix Configuration

        private void btnDentrixSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Utility.Application_Version = cmbEHRVersion.Text.Trim();

                if (
                       (cmbEHRVersion.Text.ToLower() == "DTX G5".ToLower())
                    || (cmbEHRVersion.Text.ToLower() == "DTX G5.1".ToLower())
                    || (cmbEHRVersion.Text.ToLower() == "DTX G5.2".ToLower())
                    || (cmbEHRVersion.Text.ToLower() == "DTX G6".ToLower())
                    || (cmbEHRVersion.Text.ToLower() == "DTX G6.1".ToLower())
                    )
                {
                    ViewtblDentrixPanel();

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
                    bool IsEHRConnected = SystemBAL.GetEHRDentrixConnection();

                    Utility.NotAllowToChangeSystemDateFormat = picNotAllowToChangeSystemDateFormat.Tag.ToString() == "ON" ? true : false;
                    if (IsEHRConnected)
                    {
                        ViewAdminConfigurationPanel();
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
                        ObjGoalBase.WriteToErrorLogFile("EHR User ddxdesktopspc.pfx Not Intalled. Please Aplication start with run as administrator. " + ex.Message);
                    }
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
                        bool IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                        if (IsEHRConnected)
                        {
                            ViewAdminConfigurationPanel();
                        }
                        else
                        {
                            GetDentrixConnectionString(exePath);
                            IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                            if (IsEHRConnected)
                            {
                                ViewAdminConfigurationPanel();
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
                        bool IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                        if (IsEHRConnected)
                        {
                            ViewAdminConfigurationPanel();
                        }
                        else
                        {
                            GetDentrixConnectionString(exePath);
                            IsEHRConnected = SystemBAL.GetEHRDentrixConnection();
                            if (IsEHRConnected)
                            {
                                ViewAdminConfigurationPanel();
                            }
                        }

                        //  ObjGoalBase.ErrorMsgBox("EHR User", "DENTRIXAPI_RegisterUser returning code is :" + DENTRIXAPI_RegisterUser(GoalBase.DentrixG62keyFilePath).ToString() + " {0}");


                        //CommonUtility.GetDentrixDLLForDocumentUpload(exePath);
                    }
                }
                try
                {
                    //Utility.SetRegistryObject(Registry.LocalMachine, "SyncCallCount", "0", null, RegistryValueKind.String);
                    GoalBase.GetConnectionStringforDoc(true);
                    //GoalBase.fncSynchDentrixDocPwd();
                }
                catch (Exception ex1)
                {
                    ObjGoalBase.WriteToSyncLogFile("Error while Find Doc pwd for pdf Attachment " + ex1.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
                ViewAdminConfigurationPanel();
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
                    using (StreamReader sr = new StreamReader(DentrixConFileName))
                    {
                        Utility.DBConnString = sr.ReadToEnd().ToString().Trim();
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
            catch (Exception)
            {
                Utility.DBConnString = "";
                return "";
            }

        }

        private void btnDentrixBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bindFormControls();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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

                if ((Utility.AditSync) || (Utility.PozativeSync) || (Utility.AditSync && Utility.PozativeSync))
                {
                    #region Adit Sync

                    if (Utility.AditSync)
                    {
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
                            return;
                        }

                        if (responseLogin.ErrorMessage != null)
                        {
                            ObjGoalBase.ErrorMsgBox("Adit Server", "Server is down. Please try again after a few minutes.");
                            return;
                        }

                        var AdminLoginDto = JsonConvert.DeserializeObject<AdminLoginDetailBO>(responseLogin.Content);
                        if (AdminLoginDto == null)
                        {
                            ObjGoalBase.ErrorMsgBox("Adit App Admin User Authentication", responseLogin.ErrorMessage.ToString());
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
                            return;
                        }

                        UserAditLocationLinkList = response.Content;
                        var customerDto = JsonConvert.DeserializeObject<LocationDetailBO>(response.Content);

                        dtTempApptLocationTable.Clear();
                        DataRow drApptLocDef = dtTempApptLocationTable.NewRow();
                        drApptLocDef["id"] = "0";
                        drApptLocDef["name"] = "<Select>";
                        dtTempApptLocationTable.Rows.Add(drApptLocDef);
                        dtTempApptLocationTable.AcceptChanges();
                        if (customerDto != null && customerDto.data != null)
                        {
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

                            if (RBtnMultiClinic.Checked)
                            {
                                LblAditLocationSingle.Visible = false;
                                cmbAditLocation.Visible = false;
                                DGVMuliClinc.Visible = true;

                                DGVMuliClinc.DataSource = dtTempOpenDentalClinicTable;

                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DataSource = dtTempApptLocationTable.Copy();
                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).ValueMember = "id";
                                ((DataGridViewComboBoxColumn)DGVMuliClinc.Columns["Location"]).DisplayMember = "name";

                                for (int i = 0; i < DGVMuliClinc.Rows.Count; i++)
                                {
                                    DGVMuliClinc.Rows[i].Cells["Location"].Value = "0";
                                }
                            }
                            else
                            {
                                DGVMuliClinc.Visible = false;

                                LblAditLocationSingle.Visible = true;
                                cmbAditLocation.Visible = true;
                            }
                        }
                    }
                    #endregion

                    #region Pozative Sync

                    if (Utility.PozativeSync)
                    {

                        if (string.IsNullOrEmpty(txtPozativeEmailID.Text.Trim()))
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Email", "Please enter Pozative Email Id");
                            txtPozativeEmailID.Focus();
                            return;
                        }

                        if (!Utility.IsValidEmailAddress(txtPozativeEmailID.Text))
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Email", "Please enter valid Pozative Email Id");
                            txtPozativeEmailID.Focus();
                            return;
                        }

                        if (string.IsNullOrEmpty(txtPozativePassword.Text.Trim()))
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Password", "Please enter valid Pozative Eamil Id Password");
                            txtPozativePassword.Focus();
                            return;
                        }

                        //////////////////////////////
                        //DataTable dtLocation = SystemBAL.CheckLocationIsExitsInLiveDB(txtPozativeEmailID.Text.Trim(), txtPozativePassword.Text.Trim());
                        //if (dtLocation != null && dtLocation.Rows.Count > 0)
                        //{

                        //    if (Utility.AditSync)
                        //    {
                        //        ViewEHRLocationPanel();
                        //    }
                        //    else
                        //    {

                        //        string GetCurTimeZoneName = TimeZone.CurrentTimeZone.StandardName.ToString();

                        //        bool staInstallApp = SystemBAL.Save_InstallApplicationDetail(
                        //                                                        Utility.Organization_ID,
                        //                                                        Utility.Location_ID,
                        //                                                        cmbEHRName.Text.Trim(),
                        //                                                        cmbEHRVersion.Text.Trim(),
                        //                                                        System_Name,
                        //                                                        processorID,
                        //                                                        Utility.EHRHostname,
                        //                                                        Utility.EHRIntegrationKey,
                        //                                                        Utility.EHRUserId,
                        //                                                        Utility.EHRPassword,
                        //                                                        Utility.EHRDatabase,
                        //                                                        Utility.EHRPort,
                        //                                                        Utility.WebAdminUserToken,
                        //                                                        GetCurTimeZoneName,
                        //                                                        Utility.AditSync,
                        //                                                        Utility.PozativeSync,
                        //                                                        txtPozativeEmailID.Text.Trim(),
                        //                                                        txtPozativePassword.Text.Trim(),
                        //                                                        IsAction);
                        //        if (staInstallApp)
                        //        {
                        //            Utility.Application_Name = cmbEHRName.Text.Trim();
                        //            Utility.Application_Version = cmbEHRVersion.Text.Trim();
                        //            Utility.Application_ID = Convert.ToInt32(cmbEHRName.SelectedIndex.ToString());
                        //            Utility.PozativeEmail = txtPozativeEmailID.Text.Trim();
                        //            Utility.PozativeLocationID = txtPozativePassword.Text.Trim();
                        //            ObjGoalBase.WriteToSyncLogFile("Application installation has been completed successfully.");

                        //            ObjGoalBase.SuccessMsgBox("Pozative Service Configuration", "Application installation has completed successfully. ");
                        //            this.DialogResult = DialogResult.OK;
                        //            this.Close();

                        //        }

                        //    }
                        //}
                        //else
                        //{
                        //    ObjGoalBase.ErrorMsgBox("Pozative Location", "Please enter valid email address and location id");
                        //    txtAdminUserName.Focus();
                        //    return;
                        //}
                        //////////////////////


                        UserPozativeLocationLinkList = string.Empty;
                        string strApiLocOrg = SystemBAL.GetApiPozativeLocation();
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                        var client = new RestClient(strApiLocOrg);
                        var request = new RestRequest(Method.POST);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        PozatvieLoginPostBO PozatvieLoginPost = new PozatvieLoginPostBO
                        {
                            username = txtPozativeEmailID.Text.Trim(),
                            password = txtPozativePassword.Text.Trim()
                        };
                        var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string jsonString = javaScriptSerializer.Serialize(PozatvieLoginPost);
                        request.AddHeader("postman-token", "a51e59f2-8946-424d-e1e0-988e059940d2");
                        request.AddHeader("cache-control", "no-cache");
                        request.AddHeader("content-type", "application/json");
                        request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                        GoalBase.WriteToPaymentLogFromAll_Static("Request : " + strApiLocOrg.ToString());
                        IRestResponse response = client.Execute(request);

                        if (response.ErrorMessage != null)
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative App User Authentication", response.ErrorMessage);
                            return;
                        }

                        if (response.Content.ToString().Contains("User Location Not Found for Administrator Role!"))
                        {
                            MessageBox.Show("Invalid User : User Location Not Found for Administrator Role!");
                        }
                        //GoalBase.WriteToPaymentLogFromAll_Static("Response : " + response.Content.ToString());
                        UserPozativeLocationLinkList = response.Content;
                        var LocDto = JsonConvert.DeserializeObject<PozativeLocationDetailBO>(response.Content);

                        if (LocDto.success == "false")
                        {
                            ObjGoalBase.ErrorMsgBox("Pozative Email & Password", LocDto.message);
                            return;
                        }

                        dtTempPozativeLocationTable.Clear();
                        DataRow drApptLocDef = dtTempPozativeLocationTable.NewRow();
                        drApptLocDef["id"] = "0";
                        drApptLocDef["locationname"] = "<Select>";
                        dtTempPozativeLocationTable.Rows.Add(drApptLocDef);
                        dtTempPozativeLocationTable.AcceptChanges();

                        for (int i = 0; i < LocDto.data.Count; i++)
                        {
                            DataRow drApptLoc = dtTempPozativeLocationTable.NewRow();
                            drApptLoc["id"] = LocDto.data[i].id.ToString();
                            if (LocDto.data[i].locationname == null)
                            {
                                drApptLoc["locationname"] = string.Empty;
                            }
                            else
                            {
                                drApptLoc["locationname"] = LocDto.data[i].locationname.ToString();
                            }

                            if (LocDto.data[i].machineid == null)
                            {
                                drApptLoc["Machineid"] = string.Empty;
                            }
                            else
                            {
                                drApptLoc["Machineid"] = Convert.ToString(LocDto.data[i].machineid.ToString());
                            }
                            dtTempPozativeLocationTable.Rows.Add(drApptLoc);
                            dtTempPozativeLocationTable.AcceptChanges();
                        }

                        cmbPozativeLocation.DataSource = dtTempPozativeLocationTable;
                        cmbPozativeLocation.ValueMember = "id";
                        cmbPozativeLocation.DisplayMember = "locationname";
                        cmbPozativeLocation.SelectedIndex = 0;
                        cmbPozativeLocation.DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                    ViewEHRLocationPanel();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Admin User Authentication", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnAdminUserBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bindFormControls();
            }
            catch (Exception)
            {
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

                    if (Pid.ToString().Trim() != string.Empty && Pid.ToString().Trim() != "0")
                    {
                        if (processorID.ToString() != Pid.ToString().Trim())
                        {
                            ObjGoalBase.ErrorMsgBox("Location Name", cmbPozativeLocation.Text.ToString() + " Location is already configured with another system.");
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
                                    return;
                                }
                            }
                        }
                        catch (Exception)
                        {
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
                    return;
                }

                var GetEHRListWithWebId = JsonConvert.DeserializeObject<EHRListWithWebIdBO>(response.Content);
                //MessageBox.Show(response.Content.ToString());
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
                        ObjGoalBase.WriteToErrorLogFile("EHR Location " + staLocation_EHR);
                        return;
                    }
                    else
                    {
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
                }

                if (Utility.LocationTimeZone == string.Empty)
                {
                    Utility.LocationTimeZone = TimeZone.CurrentTimeZone.StandardName.ToString();
                }

                string PozatieLocationid = string.Empty;
                string PozatieLocationName = string.Empty;

                if (Utility.PozativeSync == true)
                {
                    PozatieLocationid = cmbPozativeLocation.SelectedValue.ToString().Trim();
                    PozatieLocationName = cmbPozativeLocation.Text.Trim();
                }

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
                                                                Utility.AditSync,
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
                    bool isAppVersionAutoUpdateserverData = SystemBAL.ApplicationUpdateServerDate();

                    string AppVersionAutoUpdateAditserverData = PushLiveDatabaseBAL.Push_Location_EHRUPdateForVersion();

                    ObjGoalBase.WriteToSyncLogFile("[AppVersion AutoUpdate ServerData] has completed successfully.");
                }
                catch (Exception EX)
                {
                    ObjGoalBase.WriteToErrorLogFile("AppVersion AutoUpdate ServerData : " + EX.Message);
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
                    try
                    {
                        Utility.UpdateBackupDBFromLocalDB();
                    }
                    catch { }                     
                    frmPozative.SynchDataLiveDB_Push_PozativeConfiguraion();
                }

                if (Utility.AditSync == false && Utility.PozativeSync == true)
                {
                    ObjGoalBase.SuccessMsgBox("Pozative Service Configuration", "Application installation has completed successfully with [" + PozatieLocationName + "] Location.");
                }
                else
                {
                    ObjGoalBase.SuccessMsgBox("Adit Configuration", "Application installation has completed successfully. ");
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
                ObjGoalBase.ErrorMsgBox("EHR Location", EX.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnLocationBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ViewAdminConfigurationPanel();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region SoftDent Configuration

        public bool OpenConnection()
        {
            try
            {
                string serverExeDir = SoftDentRegistryKey.ServerExeDir;
                string faircomServerName = "";//SoftDentRegistryKey.FaircomServerName;
                ErrorCode errorCode = ErrorCode.Failure;
                bool b = InteropFactory.SoftDent.Open(serverExeDir, faircomServerName, out errorCode);
                return b;
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
                        ViewAdminConfigurationPanel();
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
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion

        #region ClearDent Configuration

        private void btnClearDentBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bindFormControls();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
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


                Utility.EHRHostname = txtClearDentHostName.Text.ToLower().Trim();
                Utility.EHRIntegrationKey = string.Empty;
                Utility.EHRDatabase = "";
                Utility.EHRUserId = txtClearDentUserId.Text.Trim();
                Utility.EHRPassword = txtClearDentPassowrd.Text.Trim();
                Utility.EHRDocPath = txt_PatientDocPath.Text.Trim().TrimEnd('\\');
                Utility.EHRPort = "3306";
                Utility.DontAskPasswordOnSaveSetting = false;
                Utility.NotAllowToChangeSystemDateFormat = false;
                if (txtClearDentPassowrd.Text.Trim() == "")
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
                    //Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                    //btnOpenDentalSave_Click(null, null);
                    ViewAdminConfigurationPanel();
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
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
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
                        ViewAdminConfigurationPanel();
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
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
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
                    if (string.IsNullOrEmpty(cmbSqlServiceName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("SQL Service", "Please Enter valid Tracker SQL Service Name");
                        cmbSqlServiceName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(cmbTrackerHostName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("ServerName", "Please Enter valid Tracker ServerName");
                        cmbTrackerHostName.Focus();
                        return;
                    }
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
                    IsEHRConnected = CheckTrackerConnection();
                    if (IsEHRConnected)
                    {
                        //Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                        //btnOpenDentalSave_Click(null, null);
                        ViewAdminConfigurationPanel();
                    }
                    else
                    {
                        CreateCredentialANdUserForTracker();
                        IsEHRConnected = CheckTrackerConnection();
                        if (IsEHRConnected)
                        {
                            //Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                            //btnOpenDentalSave_Click(null, null);
                            ViewAdminConfigurationPanel();
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

                            ObjGoalBase.ErrorMsgBox("Authentication", "Tracker is not connecting. " + "\n" + " Please enter valid credentials.");
                        }
                    }
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
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
                Utility.EHRHostname = cmbTrackerHostName.Text.ToLower().Trim();
                Utility.EHRIntegrationKey = string.Empty;
                Utility.EHRDatabase = txtTrackerDatabase.Text.Trim();
                Utility.EHRUserId = txtTrackerUserId.Text.Trim();
                Utility.EHRPassword = txtTrackerPassword.Text.Trim();
                Utility.EHRPort = "";
                //Data Source=DESKTOP-LT7125H\SQLEXPRESS;Initial Catalog=Tracker;User ID=TrackerAdmin;Password=***********
                Utility.DBConnString = "Data Source=" + Utility.EHRHostname + ";Initial Catalog=" + Utility.EHRDatabase + ";User ID=" + Utility.EHRUserId + ";Password=" + Utility.EHRPassword + ";";
                bool IsEHRConnected = SystemBAL.GetEHRTrackerConnection();
                return IsEHRConnected;
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("CheckTrackerConnection", "Tracker is not connecting. " + "\n" + " Please enter valid credentials." + ex.Message.ToString());
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                throw;
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
                ObjGoalBase.ErrorMsgBox("CreateCredentialANdUserForTracker", "Tracker is not connecting. " + "\n" + " Please enter valid credentials." + ex.Message.ToString());
                FileName = Application.StartupPath + "\\MultiUser.bat";
                ExecuteBatchFile(FileName);
                throw;
            }
        }


        private void btnTrackerBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bindFormControls();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
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

                if (cmbEHRVersion.Text.ToLower() == "14.4.2" || cmbEHRVersion.Text.ToLower() == "14.8.2")
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
                        //Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                        //btnOpenDentalSave_Click(null, null);
                        ViewAdminConfigurationPanel();
                    }
                    else
                    {
                        CreateCredentialANdUserForTracker();
                        IsEHRConnected = CheckAbelDentConnection();
                        if (IsEHRConnected)
                        {
                            //Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                            //btnOpenDentalSave_Click(null, null);
                            ViewAdminConfigurationPanel();
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
                        }
                    }
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
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
                ObjGoalBase.ErrorMsgBox("CheckAbelDentConnection", "AbelDent is not connecting. " + "\n" + " Please enter valid credentials." + ex.Message.ToString());
                return false;
                throw;
            }
        }

        private void btnAbeldentBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bindFormControls();
            }
            catch (Exception)
            {
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
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void lblHead_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
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
                    //if (cmbEHRName.SelectedIndex == 1)
                    //{
                    //    cmbEHRVersion.DataSource = GetEaglesoftVersionList();
                    //}
                    //else if (cmbEHRName.SelectedIndex == 2)
                    //{
                    //    cmbEHRVersion.DataSource = GetOpenDentalVersionList();
                    //}
                    //else if (cmbEHRName.SelectedIndex == 3)
                    //{
                    //    cmbEHRVersion.DataSource = GetDentrixVersionList();
                    //}

                    switch (cmbEHRName.SelectedIndex)
                    {
                        case 1:
                            cmbEHRVersion.DataSource = GetEaglesoftVersionList();
                            break;
                        case 2:
                            cmbEHRVersion.DataSource = GetOpenDentalVersionList();
                            break;
                        case 3:
                            cmbEHRVersion.DataSource = GetDentrixVersionList();
                            break;
                        case 4:
                            cmbEHRVersion.DataSource = GetSoftDentVersionList();
                            break;
                        case 5:
                            cmbEHRVersion.DataSource = GetClearDentVersionList();
                            break;
                        case 6:
                            cmbEHRVersion.DataSource = GetTrackerVersionList();
                            break;
                        case 7:
                            cmbEHRVersion.DataSource = GetPracticeWorkVersionList();
                            break;
                        case 8:
                            cmbEHRVersion.DataSource = GetEasyDentalVersionList();
                            break;
                        case 9:
                            cmbEHRVersion.DataSource = GetOpenDentalVersionList();
                            break;
                        case 10:
                            cmbEHRVersion.DataSource = GetPracticeWebVersionList();
                            break;
                        case 11:
                            cmbEHRVersion.DataSource = GetAbelDentVersionList();
                            break;

                    }

                    cmbEHRVersion.ValueMember = "Version_ID";
                    cmbEHRVersion.DisplayMember = "Version_Name";
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("EHR Selected Index Changed", ex.Message);
            }
        }

        private void txtAdminPassword_Leave(object sender, EventArgs e)
        {
            btnAdminUserSave.Focus();
            btnAdminUserSave.Select();
        }

        private void cmbAditLocation_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Utility.PozativeSync)
                {
                    cmbPozativeLocation.Focus();
                }
                else
                {
                    btnLocationSave.Focus();
                }
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
                    ViewPracticeWorkPanel();

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
                    //MessageBox.Show(Utility.DBConnString);
                    bool IsEHRConnected = SystemBAL.GetPracticeWorkConnection();
                    //MessageBox.Show(IsEHRConnected.ToString());
                    if (IsEHRConnected)
                    {
                        ViewAdminConfigurationPanel();
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
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

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
                ObjGoalBase.ErrorMsgBox("Adit Sync", ex.Message);
            }
        }

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
                    bool IsEHRConnected = SystemBAL.GetEHRPracticeWebConnection(Utility.DBConnString);

                    if (IsEHRConnected)
                    {
                        //Utility.DBConnString = "server=" + Utility.EHRHostname + ";port=" + Utility.EHRPort + ";database=" + Utility.EHRDatabase + ";uid=" + Utility.EHRUserId + ";pwd=" + Utility.EHRPassword + ";default command timeout=120;";    
                        //btnOpenDentalSave_Click(null, null);
                        ViewAdminConfigurationPanel();

                        dtTempOpenDentalClinicTable = SynchPracticeWebBAL.GetPracticeWebClinicData(Utility.DBConnString);

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
                    }
                }
                else
                {
                    ObjGoalBase.ErrorMsgBox("EHR Version", "Wrong Version");
                }

            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Authentication", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPracticeWebBack_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bindFormControls();
            }
            catch (Exception)
            {
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ShowEagleSoftMultiSite()
        {
            try
            {
                bool IsEHRConnected = SystemBAL.GetEHREagleSoftConnection(Utility.DBConnString);
                if (!IsEHRConnected)
                {
                    this.EditDSNEagleSoft("PozativeDSN", "Pozative DSN");
                    IsEHRConnected = SystemBAL.GetEHREagleSoftConnection(Utility.DBConnString);
                }
                if (!IsEHRConnected)
                {
                    for (int index = 0; index <= 5; ++index)
                        IsEHRConnected = SystemBAL.GetEHREagleSoftConnection(Utility.DBConnString);
                }
                if (!IsEHRConnected)
                    throw new Exception("EagleSoft Database is not getting connected.");

                //this.dtTempOpenDentalClinicTable = SynchEaglesoftBAL.GetEagleSoftSiteData(Utility.DBConnString);
                if (this.dtTempOpenDentalClinicTable.Rows.Count > 1)
                {
                    this.RBtnMultiClinic.Checked = true;
                    this.RBtnSingleClinic.Visible = false;
                    this.RBtnMultiClinic.Visible = false;
                }
                else
                {
                    this.RBtnSingleClinic.Checked = true;
                    this.RBtnSingleClinic.Visible = false;
                    this.RBtnMultiClinic.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditDSNEagleSoft(string dsnName, string description)
        {
            string str1 = "";
            try
            {
                string str2 = "c:\\eaglesoft\\shared files\\dbodbc17.dll";
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\ODBC\\ODBC.INI\\" + dsnName, true);                
                if (registryKey == null)
                    return;
                try
                {
                    str1 = (string)registryKey.GetValue("Driver");
                }
                catch
                {
                }
                if (str1.Trim() != "" && str1.ToUpper().Trim() != str2.ToUpper().Trim())
                    registryKey.SetValue("Driver", (object)str2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
