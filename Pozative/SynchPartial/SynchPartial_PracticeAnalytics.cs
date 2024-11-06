using Pozative.BAL;
using Pozative.UTL;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using Pozative.DAL;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Win32;
using System.Diagnostics;
using RestSharp;
using System.Net.Security;
using Pozative.BO;

namespace Pozative
{
    public partial class frmPozative
    {

        #region Variable

        private BackgroundWorker bwSynch_PracticeAnalytics = null;
        private System.Timers.Timer timerSynch_PracticeAnalytics = null;
        bool IsEHRDB_Connected = false;

        #endregion

        private void CallSynchPracticeAnalytics()
        {
            if (Utility.SyncPracticeAnalytics)
            {
                //   SynchData_PracticeAnalytics();
                fncSynchData_PracticeAnalytics();
            }

        }

        #region Synch PracticeAnalytics

        private void fncSynchData_PracticeAnalytics()
        {
            InitBgWorker_PracticeAnalytics();
            InitBgTimer_PracticeAnalytics();
        }

        private void InitBgTimer_PracticeAnalytics()
        {
            timerSynch_PracticeAnalytics = new System.Timers.Timer();
            this.timerSynch_PracticeAnalytics.Interval = 1000 * GoalBase.intervalEHRSynch_PracticeAnalytics;
            this.timerSynch_PracticeAnalytics.Elapsed += new System.Timers.ElapsedEventHandler(this.timerSynch_PracticeAnalytics_Tick);
            timerSynch_PracticeAnalytics.Enabled = true;
            timerSynch_PracticeAnalytics.Start();
            timerSynch_PracticeAnalytics_Tick(null, null);
        }

        private void InitBgWorker_PracticeAnalytics()
        {
            bwSynch_PracticeAnalytics = new BackgroundWorker();
            bwSynch_PracticeAnalytics.WorkerReportsProgress = true;
            bwSynch_PracticeAnalytics.WorkerSupportsCancellation = true;
            bwSynch_PracticeAnalytics.DoWork += new DoWorkEventHandler(bwSynch_PracticeAnalytics_DoWork);
            bwSynch_PracticeAnalytics.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSynch_PracticeAnalytics_RunWorkerCompleted);
        }

        private void timerSynch_PracticeAnalytics_Tick(object sender, EventArgs e)
        {
            if (Utility.AditSync)
            {
                timerSynch_PracticeAnalytics.Enabled = false;
                MethodForCallSynchOrder_PracticeAnalytics();
            }
        }

        public void MethodForCallSynchOrder_PracticeAnalytics()
        {
            System.Threading.Thread procThreadmain_PracticeAnalytics = new System.Threading.Thread(this.CallSyncOrderTable_PracticeAnalytics);
            procThreadmain_PracticeAnalytics.Start();
        }

        public void CallSyncOrderTable_PracticeAnalytics()
        {
            if (bwSynch_PracticeAnalytics.IsBusy != true)
            {
                bwSynch_PracticeAnalytics.RunWorkerAsync();
            }
            else
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void bwSynch_PracticeAnalytics_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if ((bwSynch_PracticeAnalytics.CancellationPending == true))
                {
                    e.Cancel = true;
                    return;
                }
                SynchData_PracticeAnalytics();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile(ex.Message);
            }
        }

        private void bwSynch_PracticeAnalytics_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerSynch_PracticeAnalytics.Enabled = true;
        }

        public void SynchData_PracticeAnalytics()
        {
            try
            {
                try
                {
                    if (Utility.PA_Server_Name.ToString().Trim() == "" || Utility.PA_Server_database.ToString().Trim() == ""
                     || Utility.PA_Server_user.ToString().Trim() == "" || Utility.PA_Server_password.ToString().Trim() == ""
                     || Utility.LastSyncDatetimePAServerForAPICall == default(DateTime))
                    {
                        Utility.LastSyncDatetimePAServerForAPICall = Convert.ToDateTime(DateTime.Now);
                        GetServerPracticeAnalytics(Utility.Location_ID);
                    }
                    else
                    {
                        TimeSpan TimeDiff = DateTime.Now - Utility.LastSyncDatetimePAServerForAPICall;
                        if (TimeDiff.TotalMinutes > 59)
                        {
                            GetServerPracticeAnalytics(Utility.Location_ID);
                            Utility.LastSyncDatetimePAServerForAPICall = Convert.ToDateTime(DateTime.Now);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    GetServerPracticeAnalytics(Utility.Location_ID);
                    Utility.LastSyncDatetimePAServerForAPICall = Convert.ToDateTime(DateTime.Now);
                    ObjGoalBase.WriteToErrorLogFile("PA Sync data :- " + Ex.ToString());
                }

                IsPracticeAnalytics = false;
                string fileName = string.Empty;

                try
                {
                    foreach (Process p in Process.GetProcesses())
                    {
                        fileName = p.ProcessName;
                        if (string.Compare("PracticeAnalytics".ToString().ToLower(), p.ProcessName.ToString().ToLower(), true) == 0)
                        {
                            IsPracticeAnalytics = true;
                            TimeSpan TimeDiff = DateTime.Now - Utility.LastSyncDatetimePAServer;
                            if (TimeDiff.TotalHours > 3)
                            {
                                p.Kill();
                                ObjGoalBase.WriteToSyncLogFile("Practice Analytics : Sync application close");
                                continue;
                            }
                            else
                            {
                                ObjGoalBase.WriteToSyncLogFile("Practice Analytics : Sync application is running");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    ObjGoalBase.WriteToErrorLogFile("PA Exe close :- " + Ex.ToString());
                    return;
                }

                EHR_Database_Connection();

                if (!IsPracticeAnalytics && Utility.IsApplicationIdleTimeOff && Utility.PA_Server_user.ToString().Trim() != "" && IsEHRDB_Connected)
                {
                    Process myProcess = new Process();

                    try
                    {
                        if (File.Exists(Application.StartupPath.ToString() + "\\PracticeAnalytics.exe"))
                        {                            
                            string C_param = "true " + Utility.PA_Server_Name.ToString().Trim() + " " + Utility.PA_Server_database.ToString().Trim() + " " + Utility.PA_Server_user.ToString().Trim() + " " + Utility.PA_Server_password.ToString().Trim() + " " + Utility.SyncPracticeAnalytics_Enabled.ToString() + " " + Utility.SyncPracticeAnalytics_Disabled.ToString();
                            myProcess.StartInfo.UseShellExecute = false;
                            myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\PracticeAnalytics.exe";
                            myProcess.StartInfo.Arguments = C_param;
                            myProcess.StartInfo.CreateNoWindow = true;                            
                            myProcess.Start();
                            Utility.LastSyncDatetimePAServer = Convert.ToDateTime(DateTime.Now);

                            ObjGoalBase.WriteToSyncLogFile("Practice Analytics : Sync application started. ");

                        }
                        else
                        {
                            ObjGoalBase.WriteToSyncLogFile("Practice Analytics : Sync application not found in Pozative folder ");
                            using (WebClient webClient = new WebClient())
                            {
                                webClient.DownloadFile("https://pozative.com/public/pozativeservice/PracticeAnalytics.exe", Application.StartupPath.ToString() + "\\PracticeAnalytics.exe");
                                ObjGoalBase.WriteToSyncLogFile("Practice Analytics : Sync application downloaded on server");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.WriteToErrorLogFile("PracticeAnalytics 1 " + ex.ToString());
                    }
                }
                else { ObjGoalBase.WriteToSyncLogFile("Practice Analytics is not called"); }

            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("PracticeAnalytics 0 " + ex.ToString());
            }
        }

        private void EHR_Database_Connection()
        {
            IsEHRDB_Connected = false;

            if (Utility.Application_Name.ToLower() == "Eaglesoft".ToLower())
            {
                IsEHRDB_Connected = SystemBAL.GetEHREagleSoftConnection(Utility.DBConnString);
            }
            else if (Utility.Application_Name.ToLower() == "Open Dental".ToLower())
            {
                IsEHRDB_Connected = SystemBAL.GetEHROpenDentalConnection(Utility.DBConnString);
            }
            else if (Utility.Application_Name.ToLower() == "Dentrix".ToLower())
            {
                IsEHRDB_Connected = SystemBAL.GetEHRDentrixConnection();
            }
            else
            {
                IsEHRDB_Connected = true;
            }
        }

        private void GetServerPracticeAnalytics(string Location_Id)
        {
            try
            {
                //  string strApiPracticeAnalytics = Utility.MultiRecordHostName + "v1/webhooks/mastersetting";
                Utility.PA_Server_Name = "";
                Utility.PA_Server_database = "";
                Utility.PA_Server_user = "";
                Utility.PA_Server_password = "";

                string strApiPracticeAnalytics = Utility.MultiRecordHostName + "v1/webhooks/mastersetting?appointmentlocation_id=" + Utility.Location_ID + "";

                var client = new RestClient(strApiPracticeAnalytics);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestRequest(Method.GET);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("Postman-Token", "1dbb96e6-2ae2-4038-a99c-05dbacee7a02");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                IRestResponse response = client.Execute(request);
                if (response.ErrorMessage != null)
                {
                    ObjGoalBase.WriteToErrorLogFile("[GetServerPracticeAnalytics] 1 : " + response.ErrorMessage);
                    return;
                }
                else
                {
                    var IsAditServerConn = JsonConvert.DeserializeObject<AditServer_ConnectioBO>(response.Content);
                    Utility.PA_Server_Name = IsAditServerConn.data.server;
                    Utility.PA_Server_database = IsAditServerConn.data.database;
                    Utility.PA_Server_user = IsAditServerConn.data.user;
                    Utility.PA_Server_password = IsAditServerConn.data.password;

                    ObjGoalBase.WriteToSyncLogFile("Practice Analytics : Server CAll");
                }
            }
            catch (Exception Ex)
            {
                ObjGoalBase.WriteToErrorLogFile("[GetServerPracticeAnalytics] 0 : " + Ex.ToString());
            }
        }


        #endregion

    }
}
