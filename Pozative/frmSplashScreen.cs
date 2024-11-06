using Pozative.BAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Pozative.UTL;
using Microsoft.Win32;
using System.Configuration;
using System.Diagnostics;
using RestSharp;
using System.Net;
using System.Net.Security;
using Shell32;
using System.Data.SQLite;

namespace Pozative
{
    public partial class frmSplashScreen : Form
    {
        #region Variable

        GoalBase ObjGoalBase = new GoalBase();
        DataTable dtInstallApplicationDetail = new DataTable();
        DataTable dtInstallSqlExpress = new DataTable();
        DataTable dtInstallSqlCE = new DataTable();
        DataTable dtSqlServerName = new DataTable();

        #endregion

        #region Form Load

        public frmSplashScreen()
        {
            InitializeComponent();
        }

        private void frmSplashScreen_Load(object sender, EventArgs e)
        {

        }

        private void frmSplashScreen_Shown(object sender, EventArgs e)
        {

            try
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
                    Utility.WriteToErrorLogFromAll("PozativeSetupClose : " + ex.Message.ToString());
                }
                foreach (Process p in Process.GetProcesses())
                {
                    // For softdent close application
                    if (string.Compare("SoftDentSync", p.ProcessName, true) == 0)
                    {
                        p.Kill();
                        RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\PozativeSync");
                        key.SetValue("IsSyncing", false);
                        continue;
                    }
                }
            }
            catch (Exception)
            {
            }

            #region Transfer DB From 4.0 TO 3.5
            try
            {
                string DbTransferFile = Application.StartupPath + "\\Pozatie_Db_Transfer.exe";
                string DbFile = Application.StartupPath + "\\Pozative_35.sdf";
                if (File.Exists(DbTransferFile) && File.Exists(DbFile))
                {

                    #region call DbTransfer EXE
                    Process myProcess = new Process();
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName).ToString() + "\\Pozatie_Db_Transfer.exe";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    #endregion
                }
            }
            catch (Exception)
            {
                SystemBAL.RecoveryDatabase();
            }
            #endregion

            string tmpYear = DateTime.Now.Year.ToString();
            lblSplashtxt.Text = "@ " + tmpYear + " Adit. All Rights Reserved.";

            tmrSplashScreen = new System.Windows.Forms.Timer();
            tmrSplashScreen.Interval = 100;
            tmrSplashScreen.Start();
            tmrSplashScreen.Tick += tmrSplashScreen_Tick;
        }

        #endregion

        #region Private Function

        private void CheckPozativeDatabaseConnectionWithServer()
        {
            try
            {
                string message = "";
                foreach (DataRow drRow in dtSqlServerName.Rows)
                {
                    if (SystemBAL.CheckPozativeDatabaseWithSqlServerName(drRow["SqlServerName"].ToString(), ref message))
                    {
                        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
                        connectionStringsSection.ConnectionStrings["LocalDBConnectionStringSqlServer"].ConnectionString = "Data Source=" + drRow["SqlServerName"].ToString() + ";Initial Catalog=Pozative;Integrated Security=True";
                        config.Save();
                        ConfigurationManager.RefreshSection("connectionStrings");
                        //var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        //var settings = configFile.ConnectionStrings;

                        //settings["LocalDBConnectionStringSqlServer"].Value = "Data Source=" + drRow["SqlServerName"].ToString() + ";Initial Catalog=Pozative;Integrated Security=True";

                        //configFile.Save(ConfigurationSaveMode.Modified);
                        //ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                        break;

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void UnZip(object param)
        {
            try
            {
                object[] args = (object[])param;
                string zipFile = (string)args[0];
                string folderPath = (string)args[1];

                var temppath = folderPath; //Path.GetTempPath();

                if (!File.Exists(zipFile))
                    throw new FileNotFoundException();

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
                Object shell = Activator.CreateInstance(shellAppType);
                Folder zip = (Shell32.Folder)shellAppType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { zipFile });
                Folder dest = (Shell32.Folder)shellAppType.InvokeMember("NameSpace", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { temppath.ToString() });
                dest.CopyHere(zip.Items(), 0x14);

            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Timer Tick Event

        private void tmrSplashScreen_Tick(object sender, EventArgs e)
        {
            try
            {

            ApplicaitonConnection:

                this.Show();
                tmrSplashScreen.Stop();

                string AlterDBFilePath = Application.StartupPath + "\\AlterDB.txt";
                if (File.Exists(AlterDBFilePath))
                {
                    try
                    {
                        List<String> AlterDBquery = new List<String>();
                        using (StreamReader sr = new StreamReader(AlterDBFilePath))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                AlterDBquery.Add(line);
                            }
                        }
                        bool staAlterDBquery = SystemBAL.LocalDatabaseUpdateQuery(AlterDBquery);
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToSyncLogFile("Alter DB File Execution Err " + ex.Message.ToString());
                        if (SystemBAL.RecoveryDatabase())
                        {
                            goto ApplicaitonConnection;
                        }
                    }
                    finally
                    {
                        File.Delete(Application.StartupPath + "\\AlterDB.txt");
                    }
                }
                try
                {
                    string directoryPath = Application.StartupPath;

                    // Get all .txt files that start with 'alterqry'
                    string[] files = Directory.GetFiles(directoryPath, "alterqry_*.txt");

                    // Print out each file name
                    foreach (string file in files)
                    {
                        try
                        {
                            List<String> AlterDBquery = new List<String>();
                            // AlterDBFilePath = Application.StartupPath + "\\" + file;
                            using (StreamReader sr = new StreamReader(file))
                            {
                                String line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    AlterDBquery.Add(line);
                                }
                            }
                            bool staAlterDBquery = LocalDatabaseSQLLiteUpdateQuery(Path.GetFileNameWithoutExtension(file).Replace("alterqry_", ""), AlterDBquery);
                        }
                        catch (Exception ex)
                        {
                            ObjGoalBase.WriteToSyncLogFile("SqLite Alter DB File Execution Err " + ex.Message.ToString());
                            if (SystemBAL.RecoveryDatabase())
                            {
                                goto ApplicaitonConnection;
                            }
                        }
                        finally
                        {
                            File.Delete(file);
                        }
                    }
                }
                catch(Exception exsqllite)
                {
                    Utility.WriteToErrorLogFromAll("Err_SQllite db update" + exsqllite.Message.ToString());
                }

                //////////// DocumentDLL//////////////
                try
                {
                    if (File.Exists(Application.StartupPath + "\\DocumentDLL.Zip"))
                    {
                        if (Directory.Exists(Application.StartupPath + "\\DocumentDLL"))
                        {
                            Directory.Delete(Application.StartupPath + "\\DocumentDLL", true);
                        }
                        object[] args = new object[] { Application.StartupPath + "\\DocumentDLL.Zip", Application.StartupPath };
                        UnZip(args);

                        File.Delete(Application.StartupPath + "\\DocumentDLL.Zip");
                    }

                }
                catch (Exception)
                {
                }

                ///////////////DocumentDLL////////////////

                //////////////AditAppSyncLog///////////////
                try
                {
                    if (File.Exists(Application.StartupPath + "\\AditAppSyncLog.Zip"))
                    {
                        foreach (Process p in Process.GetProcesses())
                        {
                            if (string.Compare("AditAppSyncLog".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                            {
                                try
                                {
                                    p.Kill();
                                    break;
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        object[] args = new object[] { Application.StartupPath + "\\AditAppSyncLog.Zip", Application.StartupPath };
                        UnZip(args);
                        Thread.Sleep(1000);
                        File.Delete(Application.StartupPath + "\\AditAppSyncLog.Zip");
                    }

                }
                catch (Exception ex)
                {
                    Utility.WriteToErrorLogFromAll("Err_AditAppSyncLog " + ex.Message.ToString());
                }

                //////////////AditAppSyncLog///////////////
                try
                {
                    DataTable dtAditActiveServerDetail = SystemBAL.GetAditActiveServerDetail();

                    if (dtAditActiveServerDetail != null && dtAditActiveServerDetail.Rows.Count > 0)
                    {
                        Utility.HostName_Adit = dtAditActiveServerDetail.Rows[0]["HostName"].ToString();
                        Utility.MultiRecordHostName = dtAditActiveServerDetail.Rows[0]["MultiRecordHostName"].ToString();

                        #region Check Live API Host URL

                        try
                        {
                            string strAditLogin = Utility.HostName_Adit + "handshake";
                            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                            var request = new RestRequest(Method.GET);
                            var clientLocUpdateVer = new RestClient(strAditLogin);
                            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            request.AddHeader("cache-control", "no-cache");
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("apptype", "aditehr");
                            IRestResponse response = clientLocUpdateVer.Execute(request);

                            if (response.ErrorMessage != null)
                            {
                                if (response.ErrorMessage.Contains("Could not create SSL/TLS secure channel."))
                                {
                                    Utility.HostName_Adit = "https://ehrapi.adit.com/";
                                    Utility.MultiRecordHostName = "https://ehrsync.adit.com/";
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Utility.HostName_Adit = dtAditActiveServerDetail.Rows[0]["HostName"].ToString();
                            Utility.MultiRecordHostName = dtAditActiveServerDetail.Rows[0]["MultiRecordHostName"].ToString();
                        }

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    if (SystemBAL.RecoveryDatabase())
                    {
                        goto ApplicaitonConnection;
                    }
                    Utility.WriteToErrorLogFromAll(ex.Message);
                }

                string processorID = ObjGoalBase.getUniqueID("C");
                Utility.System_processorID = processorID;
                // processorID = "6EBFEBF70CDAF20FBFF";
                //Utility.System_processorID = "6EBFEBF70CDAF20FBFF";
                dtInstallApplicationDetail = SystemBAL.GetInstallApplicationDetail(processorID);
                if (dtInstallApplicationDetail != null && dtInstallApplicationDetail.Rows.Count > 0)
                {

                    frmPozative frmPoz = new frmPozative();
                    frmPoz.Show();
                }
                else
                {
                    this.Hide();
                    frmConfiguration_Auto fECon = new frmConfiguration_Auto("Insert");
                    var result = fECon.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        goto ApplicaitonConnection;
                    }
                    else
                    {
                        ObjGoalBase.WriteToSyncLogFile("[tmrSplashScreen_Tick] manually Application restart");
                        foreach (Process p in Process.GetProcesses())
                        {
                            string fileName = p.ProcessName;

                            if (string.Compare("Pozative", p.ProcessName, true) == 0 && Process.GetCurrentProcess().Id != p.Id)
                            {
                                p.Kill();
                                continue;
                            }
                        }
                        System.Environment.Exit(1);
                    }
                }
            }
            catch (System.Data.SqlServerCe.SqlCeException ex)
            {
                Utility.WriteToErrorLogFromAll(ex.Message);
                ObjGoalBase.ErrorMsgBox("Splash Screen", "We're sorry. But it seems there is some issue in SQL configuration. If you find this in error please give us a call at(832) 225 - 8865.");

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                ObjGoalBase.ErrorMsgBox("Splash Screen", ex.Message);
            }
            finally
            {
                this.Hide();
            }
        }
        public static bool LocalDatabaseSQLLiteUpdateQuery(string dbname,List<String> AlterDBquery)
        {
            bool _successfullstataus = true;
            try
            {
                foreach (var query in AlterDBquery)
                {
                    if (query.ToString().Trim() != "")
                    {
                        if (dbname.ToString().Trim() != "")
                        {

                            using (SQLiteConnection conn = new SQLiteConnection()
                            {
                                ConnectionString = @"Data Source=|DataDirectory|\" + dbname.ToString() + ".db; FailIfMissing=True;ForeignKeys=true"//dbname.ToString()+ ".db" , ForeignKeys = true }.ConnectionString
                            })
                            //using (SQLiteConnection conn = new SQLiteConnection()
                            //{
                            //    ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = dbname.ToString()+ ".db" , ForeignKeys = true }.ConnectionString
                            //})
                            {
                                try
                                {
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    string checkTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='"+ dbname.ToString() +"';";
                                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkTableQuery, conn))
                                    {
                                        var result = checkCmd.ExecuteScalar();
                                        if (result != null)
                                        {
                                            // Now run your query
                                            if (!string.IsNullOrWhiteSpace(query.ToString()))
                                            {
                                                using (SQLiteCommand SqlCeCommand = new SQLiteCommand(query.ToString().Trim(), conn))
                                                {
                                                    SqlCeCommand.CommandType = CommandType.Text;
                                                    SqlCeCommand.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                { }
                                finally
                                {
                                    if (conn.State == ConnectionState.Open) conn.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }


            return _successfullstataus;
        }
        #endregion

    }
}
