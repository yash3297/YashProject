using Microsoft.Win32;
using Newtonsoft.Json;
using Pozative.BAL;
using Pozative.BO;
using Pozative.DAL;
using Pozative.UTL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Pozative
{
    public partial class frmDatabaseConfiguration : Form
    {

        #region Variable

        GoalBase ObjGoalBase = new GoalBase();

        string strEaglesoftIntegrationKey = "32313032-4553-EFFD-5083-506F7A617469";
        private const string ODBC_INI_REG_PATH = "SOFTWARE\\ODBC\\ODBC.INI\\";
        private const string ODBCINST_INI_REG_PATH = "SOFTWARE\\WOW6432Node\\ODBC\\ODBC.INI\\";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        string EHRHostname = string.Empty;
        string EHRIntegrationKey = string.Empty;
        string EHRDatabase = string.Empty;
        string EHRUserId = string.Empty;
        string EHRPassword = string.Empty;
        string EHRDocPath = string.Empty;
        string EHRPort = string.Empty;

        string DBConnString = string.Empty;
        #endregion

        #region Form_Load

        public frmDatabaseConfiguration()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            this.ShowDialog();
        }

        #endregion

        #region Button Click

        private void frmDatabaseConfiguration_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                btnOpenDentalCancel.BackColor = WDSColor.ButtonBackColor;
                btnOpenDentalCancel.ForeColor = WDSColor.ButtonForeColor;

                btnOpenDentalSave.ForeColor = WDSColor.SaveButtonForeColor;
                btnOpenDentalSave.BackColor = WDSColor.SaveButtonBackColor;

                btnOpenDentalSave.Font = WDSColor.FormButtonFont;
                btnOpenDentalCancel.Font = WDSColor.FormButtonFont;

                btnEaglesoftBack.BackColor = WDSColor.ButtonBackColor;
                btnEaglesoftBack.ForeColor = WDSColor.ButtonForeColor;

                btnEaglesoftSave.ForeColor = WDSColor.SaveButtonForeColor;
                btnEaglesoftSave.BackColor = WDSColor.SaveButtonBackColor;

                btnEaglesoftSave.Font = WDSColor.FormButtonFont;
                btnEaglesoftBack.Font = WDSColor.FormButtonFont;


                if (Utility.Application_ID == 1)
                {
                    tblOpenDentalMain.Visible = false;
                    tblEaglesoftMain.Visible = true;
                }
                else if (Utility.Application_ID == 2)
                {
                    tblOpenDentalMain.Visible = true;
                    tblEaglesoftMain.Visible = false;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Database Config - Load", ex.Message);
            }
        }

        private void btnOpenDentalSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (string.IsNullOrEmpty(txtOpenDentalHostName.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Hostname", "Please Enter valid OpenDental Hostname");
                        txtOpenDentalHostName.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtOpenDentalDatabase.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("Database", "Please Enter valid OpenDental Database Name");
                        txtOpenDentalDatabase.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtOpenDentalUserId.Text.Trim()))
                    {
                        ObjGoalBase.ErrorMsgBox("IntegrationKey", "Please Enter valid OpenDental User Id");
                        txtOpenDentalUserId.Focus();
                        return;
                    }

                    EHRHostname = txtOpenDentalHostName.Text.ToLower().Trim();
                    EHRIntegrationKey = string.Empty;
                    EHRDatabase = txtOpenDentalDatabase.Text.Trim();
                    EHRUserId = txtOpenDentalUserId.Text.Trim();
                    EHRPassword = txtOpenDentalPassword.Text.Trim();
                    EHRDocPath = string.Empty;
                    EHRPort = "3306";

                    if (Utility.DtInstallServiceList.Select("Database = '" + EHRDatabase + "' ").Count() > 0)
                    {
                        ObjGoalBase.ErrorMsgBox("Database", "Please Enter Other OpenDental Database Name. This Name Is Already Use ");
                        txtOpenDentalDatabase.Focus();
                        return;
                    }

                    DBConnString = "server=" + EHRHostname + ";port=" + EHRPort + ";database=" + EHRDatabase + ";uid=" + EHRUserId + ";pwd=" + EHRPassword + ";default command timeout=120;";
                    bool IsEHRConnected = SystemBAL.GetEHROpenDentalConnection(DBConnString);

                    if (IsEHRConnected)
                    {
                        var MaxNo = Utility.DtInstallServiceList.Compute("MAX([Installation_ID])", "").ToString();
                        int Service_Install_ID = Convert.ToInt32(MaxNo) + 1;

                        EHRDocPath = SynchOpenDentalBAL.GetOpenDentalDocPath(DBConnString);

                        bool staInstallApp = SystemBAL.Save_InstallApplicationDetail(
                                                                Utility.Organization_ID,
                                                                "",
                                                                Utility.Application_Name,
                                                                Utility.Application_Version,
                                                                System.Environment.MachineName,
                                                                Utility.System_processorID,
                                                                EHRHostname,
                                                                Utility.EHRIntegrationKey,
                                                                EHRUserId,
                                                                EHRPassword,
                                                                EHRDatabase,
                                                                EHRPort,
                                                                DBConnString,
                                                                Utility.WebAdminUserToken,
                                                                Utility.LocationTimeZone,
                                                                Utility.AditSync,
                                                                Utility.PozativeSync,
                                                                "",
                                                                "",
                                                                "",
                                                                "Insert",
                                                                EHRDocPath,
                                                                Service_Install_ID.ToString(),
                                                                Utility.DontAskPasswordOnSaveSetting,
                                                                Utility.NotAllowToChangeSystemDateFormat);

                        if (staInstallApp)
                        {
                            SaveEHRErroLog(EHRUserId, EHRPassword, (Utility.Application_ID == 1 ? "Eaglesoft" :"OpenDental") + DBConnString,"", Utility.Organization_ID);
                            ObjGoalBase.InformationMsgBox("Database Add", "Database Add Successfully. Please Add Clinic for Database " + EHRDatabase + " Using Add Clinic Button.");
                            ObjGoalBase.WriteToSyncLogFile(Utility.Application_Name + "  Service Install Database " + EHRDatabase + " configuration save successfully.");
                            ObjGoalBase.WriteToSyncLogFile("EHR Database Save Successfully.");

                            Utility.DtInstallServiceList = SystemBAL.GetInstallServiceDetail();
                            this.Close();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Service Install ", "Service Install Failed..");
                        }
                    }
                    else
                    {
                        DBConnString = "";
                        EHRHostname = string.Empty;
                        EHRIntegrationKey = string.Empty;
                        EHRUserId = string.Empty;
                        EHRPassword = string.Empty;
                        EHRDatabase = string.Empty;
                        EHRPort = string.Empty;

                        DBConnString = string.Empty;
                        ObjGoalBase.ErrorMsgBox("Authentication", "OpenDental is not connecting. " + "\n" + " Please enter valid credentials.");
                    }
                }
                catch (Exception ex)
                {
                    ObjGoalBase.ErrorMsgBox("Databse Config Authentication", ex.Message);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Adit Database Config Save] : " + Ex.Message);
            }
        }
        private async void btnEaglesoftSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (string.IsNullOrEmpty(txtEaglesoftHostName.Text.Trim()))
                {
                    ObjGoalBase.ErrorMsgBox("Hostname", "Please Enter valid Eaglesoft ServerName");
                    txtEaglesoftHostName.Focus();
                    return;
                }

                //if (txtEaglesoftUserId.Visible && string.IsNullOrEmpty(txtEaglesoftUserId.Text.Trim()))
                //{
                //    ObjGoalBase.ErrorMsgBox("UserId", "Please Enter valid Eaglesoft User Id");
                //    txtEaglesoftUserId.Focus();
                //    return;
                //}
                //if (txtEaglesoftPassword.Visible && string.IsNullOrEmpty(txtEaglesoftPassword.Text.Trim()))
                //{
                //    ObjGoalBase.ErrorMsgBox("Password", "Please Enter valid Eaglesoft Password");
                //    txtEaglesoftPassword.Focus();
                //    return;
                //}


                try
                {
                    //string EaglesoftConStr = "";
                    //if (Utility.Application_Version.ToLower() == "21.20".ToLower())
                    //{
                    //    Type esSettingsType = Type.GetTypeFromProgID("EaglesoftSettings.EaglesoftSettings");
                    //    dynamic settings = Activator.CreateInstance(esSettingsType);
                    //    bool tokenIsValid = settings.SetToken("Adit", "689886565626569898989898998998989562323254543236565");                      

                    //    if (tokenIsValid)
                    //    {
                    //        //EaglesoftConStr = settings.GetLegacyConnectionStringServer(true);
                    //        //ObjGoalBase.ErrorMsgBox("Service Install ", EaglesoftConStr);
                    //        EaglesoftConStr = settings.GetLegacyConnectionStringServer(false);
                    //        ObjGoalBase.ErrorMsgBox("Service Install ", EaglesoftConStr);

                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Token InValid");
                    //    }
                    //}
                    //else
                    //{
                    //    Assembly.LoadFile(Application.StartupPath + "\\Patterson.PTCBaseObjects.SharedObjects.dll");
                    //    var DLL = Assembly.LoadFile(Application.StartupPath + "\\EaglesoftSettings.dll");

                    //    foreach (Type type in DLL.GetExportedTypes())
                    //    {
                    //        if (type.Name == "EaglesoftSettings")
                    //        {
                    //            dynamic settings = Activator.CreateInstance(type);
                    //            //if (Utility.Application_Version.ToLower() == "21.20".ToLower())
                    //            //{
                    //            //    EaglesoftConStr = settings.GetLegacyConnectionString2120(false);
                    //            //}
                    //            //else
                    //            //{
                    //                EaglesoftConStr = settings.GetLegacyConnectionString(false);
                    //            //}
                    //        }

                    //    }
                    //}
                    //ObjGoalBase.ErrorMsgBox("Service Install ", EaglesoftConStr);
                    ////EaglesoftConStr = "DBN=DENTSERV2;DSN=DENTAL;UID=VES;PWD=IAmeW4(M/$IWQlVKIg0kmkqSkjf3LQ5N";
                    ////string[] StrData = EaglesoftConStr.Split(';');

                    ////if (StrData.Count() > 0)
                    ////{
                    ////    EHRDatabase = StrData[0].ToString().Substring(4);
                    ////    EHRHostname = StrData[1].ToString().Substring(4);
                    ////    EHRUserId = StrData[2].ToString().Substring(4);
                    ////    EHRPassword = StrData[3].ToString().Substring(4);
                    ////}
                    ////}
                    //EaglesoftConStr = EaglesoftConStr.Replace("DSN=DENTAL;", "DSN=PozativeDSN;");
                    //EaglesoftConStr = EaglesoftConStr.Replace("DSN =DENTAL;", "DSN=PozativeDSN;");
                    //EaglesoftConStr = EaglesoftConStr.Replace("DSN= DENTAL;", "DSN=PozativeDSN;");
                    //EaglesoftConStr = EaglesoftConStr.Replace("DSN = DENTAL;", "DSN=PozativeDSN;");

                    //EHRIntegrationKey = "";
                    //EHRUserId = "";
                    //EHRPassword = "";
                    //EHRDatabase = "Secondary Database"; 
                    //EHRPort = string.Empty;
                    //DBConnString = EaglesoftConStr;
                    //EHRDocPath = "";

                    Cursor.Current = Cursors.WaitCursor;
                    string EaglesoftConStr = "";

                    Process myProcess = new Process();
                    myProcess.StartInfo.UseShellExecute = false;
                    string DPath = Application.StartupPath;
                    myProcess.StartInfo.FileName = DPath + "\\PTIAuthenticator.exe";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.Verb = "runas";
                    if (Utility.Application_Version.ToLower() == "21.20".ToLower())
                    {
                        myProcess.StartInfo.Arguments = "false false S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e true " + Utility.Application_Version.ToLower().ToLower();
                    }
                    else
                    {
                        myProcess.StartInfo.Arguments = "false true S37444a4f4856524m95700b506f62098957idec9a1a8e5401f4141b8bl8870552340e false " + Utility.Application_Version.ToLower().ToLower();
                    }
                    myProcess.Start();
                    myProcess.WaitForExit();
                    string DentrixConFileName = Application.StartupPath + "\\ConnectionString.txt";
                    try
                    {
                        using (StreamReader sr = new StreamReader(DentrixConFileName))
                        {
                            DBConnString = sr.ReadToEnd().ToString().Trim();
                            EaglesoftConStr = Utility.DecryptString(DBConnString);
                            if (Utility.Application_Version.ToLower() != "21.20".ToLower())
                            {
                                DBConnString = EaglesoftConStr;
                            }                           
                        }
                        //IsEHRConnected = SystemBAL.GetEHREagleSoftConnection();
                    }
                    catch (Exception)
                    {

                    }                   

                    EHRIntegrationKey = "";
                    EHRUserId = "";
                    EHRPassword = "";
                    EHRDatabase = "Secondary Database";
                    EHRPort = string.Empty;                 
                    bool IsEHRConnected = SystemBAL.GetEHREagleSoftConnection(EaglesoftConStr);

                    ObjGoalBase.ErrorMsgBox("Service Install ", EaglesoftConStr);
                   
                    if (IsEHRConnected)
                    {

                        var MaxNo = Utility.DtInstallServiceList.Compute("MAX([Installation_ID])", "").ToString();
                        int Service_Install_ID = Convert.ToInt32(MaxNo) + 1;
                        EHRDocPath = SynchEaglesoftBAL.GetEagleSoftDocPath(EaglesoftConStr);                    
                        bool staInstallApp = SystemBAL.Save_InstallApplicationDetail(
                                                                Utility.Organization_ID,
                                                                "",
                                                                Utility.Application_Name,
                                                                Utility.Application_Version,
                                                                System.Environment.MachineName,
                                                                Utility.System_processorID,
                                                                EHRHostname,
                                                                Utility.EHRIntegrationKey,
                                                                EHRUserId,
                                                                EHRPassword,
                                                                EHRDatabase,
                                                                EHRPort,
                                                                DBConnString,
                                                                Utility.WebAdminUserToken,
                                                                Utility.LocationTimeZone,
                                                                Utility.AditSync,
                                                                Utility.PozativeSync,
                                                                "",
                                                                "",
                                                                "",
                                                                "Insert",
                                                                EHRDocPath,
                                                                Service_Install_ID.ToString(),
                                                                Utility.DontAskPasswordOnSaveSetting,
                                                                Utility.NotAllowToChangeSystemDateFormat);

                        if (staInstallApp)
                        {
                            ObjGoalBase.InformationMsgBox("Database Add", "Database Add Successfully. Please Add Clinic for Database " + EHRDatabase + " Using Add Clinic Button.");
                            ObjGoalBase.WriteToSyncLogFile(Utility.Application_Name + "  Service Install Database " + EHRDatabase + " configuration save successfully.");
                            ObjGoalBase.WriteToSyncLogFile("EHR Database Save Successfully.");

                            Utility.DtInstallServiceList = SystemBAL.GetInstallServiceDetail();
                            this.Close();
                        }
                        else
                        {
                            ObjGoalBase.ErrorMsgBox("Service Install ", "Service Install Failed..");
                        }
                    }
                    else
                    {
                        DBConnString = "";
                        EHRHostname = string.Empty;
                        EHRIntegrationKey = string.Empty;
                        EHRUserId = string.Empty;
                        EHRPassword = string.Empty;
                        EHRDatabase = string.Empty;
                        EHRPort = string.Empty;

                        DBConnString = string.Empty;
                        ObjGoalBase.ErrorMsgBox("Authentication", "EagleSoft Secondory DataBase is not connecting. " + "\n" + " Please enter valid credentials.");
                    }
                }

                catch (Exception)
                {

                }

            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[Adit Database Config Save] : " + Ex.Message);
            }

        }
        private void btnOpenDentalCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.Close();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Adit Database Config Cancel", ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Database Config - Close", ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Password-Close", ex.Message);
            }
        }
        #endregion

        #region Common Event

        private void lblFormHead_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            catch (Exception)
            {

            }
        }

        private void SaveEHRErroLog(string userName,string password,string selectedEHR,string locationID = "", string organizationID = "")
        {
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
                //ObjGoalBase.ErrorMsgBox("SaveEHRErroLog -> SystemConfiguration", ex.Message);
            }
            PozativeConfigErrorLog AditLocOrgPost = new PozativeConfigErrorLog
            {
                datetime = DateTime.Now.ToString(),
                dentrix_pdf_connection_string = "",////
                ehr_connected = true,
                email = userName,
                error_log_file_generated = false,
                error_message = "",
                image_folder_path = "",///
                locationId = locationID,
                multiclinic_selected = new List<string>(),
                multidatabase_configure = true,
                organizationId = organizationID,
                password = password,
                selected_ehr = selectedEHR,
                sucessfully_configured = true,
                system_configuration = SystemConfig,
                total_installed_ehr = "",
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
        #endregion

        #region EagalSoft

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

                if (!DSNExists("PozativeDSN_1"))
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
                                }
                            }
                        }
                    }
                    CreateDSNEagleSoft("PozativeDSN_1", "Pozative DSN 1", serverName, driverName, true, "", "");
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


    }
}
