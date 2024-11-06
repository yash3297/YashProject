using Pozative.UTL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.Diagnostics;
using Pozative.BAL;

namespace Pozative
{
    public class CommonFunction
    {
        static GoalBase ObjGoalBase = new GoalBase();
        #region Dll Imports

        //[DllImport("Dentrix.API.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //static extern int DENTRIXAPI_Initialize(string szUserId, string szPassword);

        [DllImport("Dentrix.API.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int DENTRIXAPI_Initialize([MarshalAs(UnmanagedType.LPStr)] string szUserId, [MarshalAs(UnmanagedType.LPStr)] string szPassword);


        private const string DtxAPI = "Dentrix.API.dll";
        [DllImport(DtxAPI, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern int DENTRIXAPI_RegisterUser([MarshalAs(UnmanagedType.LPStr)] string szKeyFilePath); //G6.2 + available call

        [DllImport(DtxAPI, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern void DENTRIXAPI_GetConnectionString([MarshalAs(UnmanagedType.LPStr)] string szUserId, [MarshalAs(UnmanagedType.LPStr)] string szPassword, StringBuilder szConnectionsString, int ConnectionStringSize); //G5+ available

        [DllImport(DtxAPI, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        internal static extern float DENTRIXAPI_GetDentrixVersion(); //G5.1+ available
        #endregion

        private string DentrixG62keyFilePath = Application.StartupPath + "\\h36FadCg.dtxkey";

        #region Constants
        const int SUCCESS = 0;
        const int FAIL = 1;
        #endregion

        public static DataTable Convert_ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        //public static bool GetDentrixG62ConnectionString(string UserId, string Password)
        //{
        //    int connectionStringSize = 512;
        //    StringBuilder connectionString = new StringBuilder(connectionStringSize);
        //    bool IsConn = false;

        //    try
        //    {
        //        lock (connectionString)
        //        {
        //            DENTRIXAPI_GetConnectionString(UserId, Password, connectionString, connectionStringSize);
        //            if (string.IsNullOrWhiteSpace(connectionString.ToString()) == false)
        //            {
        //                Utility.EHR_DentrixG62_ConnString = connectionString.ToString();
        //                Utility.DBConnString = connectionString.ToString();
        //                IsConn = true;
        //            }
        //            else
        //            {
        //                Utility.EHR_DentrixG62_ConnString = string.Empty;
        //                Utility.DBConnString = string.Empty;
        //                IsConn = false;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Utility.EHR_DentrixG62_ConnString = string.Empty;
        //        IsConn = false;
        //    }
        //    return IsConn;
        //}
        private static IDtxDataProvider _dtxManager;
        private static string _password = string.Empty;
        private static string _username = "dtxuser";
        public static bool IsAudioDeviceWorking = false;
        public static string supportEmail = "";
        public static int WrongOTPTrials = 0;

        public static bool GetDentrixG62ConnectionString()
        {

            bool IsConn = false;
            string con = "";
            try
            {
                if (_dtxManager == null)
                {
                    DENTRIXAPI_Initialize("iR*9k3Hf7sd$", "kNhd4)l&64/!a");
                    _dtxManager = DataManager.Create(_username, _password);
                }
            }
            catch (Exception ex)
            {
                if (_dtxManager == null)
                {
                    DENTRIXAPI_Initialize("LNP8EGA5", "w2AdrusPa7et");
                    _dtxManager = DataManager.Create(_username, _password);
                }
                //LNP8EGA5,w2AdrusPa7et
            }
            if (_dtxManager == null)
            {
                Utility.EHR_DentrixG62_ConnString = string.Empty;
                Utility.DBConnString = string.Empty;
                IsConn = false;
            }
            else
            {
                DbConnection dentrixConnection = (DbConnection)_dtxManager.CreateCommand().Connection;
                try
                {
                    con = "host=localhost;UID=@UID;PWD=@PWD;Database=DentrixSQL;service=6597;SSL=Basic;DSN=c-treeACE ODBC Driver";
                    string ConString = dentrixConnection.ConnectionString;
                    ConString = ConString.Replace("Data Source", "host");
                    ConString = ConString.Replace("User ID", "UID");
                    ConString = ConString.Replace("Password", "PWD");
                    ConString = ConString.Replace("Initial Catalog", "Database");
                    ConString = ConString.Replace("Port Number", "service");
                    ConString = ConString + ";DSN=c-treeACE ODBC Driver";
                    //string[] str = ConString.Split(';');
                    //string _UserId = "";
                    //string _pwd = "";

                    //foreach (string s in str)
                    //{
                    //    if (s.ToUpper().Contains("USER"))
                    //    {
                    //        _UserId = s.Split('=')[1];
                    //        //txt_UserId.Text = UserId;
                    //    }
                    //    else if (s.ToUpper().Contains("PASSWORD"))
                    //    {
                    //        _pwd = s.Split('=')[1];
                    //     // txt_Passwrod.Text = pwd;
                    //    }
                    //    if (_UserId != "" && _pwd != "")
                    //    {
                    //        break;
                    //    }
                    //}
                    //con = con.Replace("@UID", _UserId);
                    //con = con.Replace("@PWD", _pwd);
                    Utility.EHR_DentrixG62_ConnString = ConString;
                    Utility.DBConnString = ConString;
                    IsConn = true;
                    // dentrixConnection.Open();
                }
                catch (Exception ex)
                {
                    con = "";
                    Utility.EHR_DentrixG62_ConnString = string.Empty;
                    Utility.DBConnString = string.Empty;
                    IsConn = false;
                    Console.WriteLine(ex.Message);
                }
            }
            return IsConn;

        }


        public static int GetDentrixG62ExePath(StringBuilder retValue, bool showMessage = true)
        {
            int retv = FAIL;
            try
            {
                RegistryKey hKey = Registry.CurrentUser.OpenSubKey(@"Software\Dentrix Dental Systems, Inc.\Dentrix\General");
                if (hKey != null)
                {
                    Object value = hKey.GetValue("ExePath");
                    if (value != null)
                    {
                        retValue.Append(value.ToString() + "Dentrix.API.dll");
                        retv = SUCCESS;
                    }
                }
            }
            catch (Exception)
            {
                if (showMessage)
                {
                    MessageBox.Show("Could not read registry value");
                }
            }
            return retv;
        }
        static System.ComponentModel.BackgroundWorker bwTextToSpeech = null;
        static string speechTextType = "";
        static string speechsuffixText = "";
        public static void TextToSpeech(string textType = "", string suffixText = "")
        {
            try
            {
                if (!IsAudioDeviceWorking)
                    return;
                #region old code
                //Application.DoEvents();
                //Process myProcess = new Process();
                //myProcess.StartInfo.UseShellExecute = false;
                //myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\System_Speech.exe";
                //Utility.WriteToErrorLogFromAll(textType + suffixText);
                //myProcess.StartInfo.Arguments = string.Format("{0} {1} {2}", "\"" + Utility.MultiRecordHostName + "\"", "\"" + textType + "\"", "\"" + suffixText + "\"");
                //myProcess.StartInfo.CreateNoWindow = true;
                //myProcess.Start();
                //myProcess.WaitForExit(); 
                #endregion                           

                speechTextType = textType;
                speechsuffixText = suffixText;

                bwTextToSpeech = new System.ComponentModel.BackgroundWorker();
                bwTextToSpeech.DoWork += BwTextToSpeech_DoWork;
                bwTextToSpeech.RunWorkerAsync();
            }
            catch (Exception ex)
            {
            }
        }
        private static void BwTextToSpeech_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                Application.DoEvents();
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = Application.StartupPath.ToString() + "\\System_Speech.exe";
                Utility.WriteToErrorLogFromAll(speechTextType + speechsuffixText);
                myProcess.StartInfo.Arguments = string.Format("{0} {1} {2}", "\"" + Utility.MultiRecordHostName + "\"", "\"" + speechTextType + "\"", "\"" + speechsuffixText + "\"");
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                ObjGoalBase.WriteToErrorLogFile("BwTextToSpeech_DoWork - " + ex.Message);
            }
        }

        public static string GetMACAddress()
        {
            try
            {

                System.Net.NetworkInformation.NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (System.Net.NetworkInformation.NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card  
                    {
                        System.Net.NetworkInformation.IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                return sMacAddress;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GetGernalInfo(string infoType)
        {
            string info = "";
            try
            {
                string strGetLocUpdateVersion = SystemBAL.GetAutoPlayAudioText();

                var clientLocUpdateVer = new RestSharp.RestClient(strGetLocUpdateVersion);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestSharp.RestRequest(RestSharp.Method.GET);
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                request.AddHeader("apptype", "aditehr");
                request.AddHeader("pagetype", "welcome");
                RestSharp.IRestResponse response = clientLocUpdateVer.Execute(request);

                BO.GeneralInfoDetails generalInfoDetails = new BO.GeneralInfoDetails();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        //ObjGoalBase.WriteToErrorLogFile("general info response - " + response.Content);
                        var infoList = Newtonsoft.Json.JsonConvert.DeserializeObject<BO.GeneralInfo>(response.Content);

                        generalInfoDetails = infoList.data;
                        switch (infoType)
                        {
                            case "multipleclinicsexists":
                                info = generalInfoDetails.multipleclinicsexists;
                                break;
                            case "multipleehrexists":
                                info = generalInfoDetails.multipleehrexists;
                                break;
                            case "success":
                                info = generalInfoDetails.success;
                                break;
                            case "welcomemessage":
                                info = generalInfoDetails.welcomemessage;
                                break;
                            case "ehrdefaultconfigurationmissing":
                                info = generalInfoDetails.ehrdefaultconfigurationmissing;
                                break;
                            case "ehrnotexists":
                                info = generalInfoDetails.ehrnotexists;
                                break;
                            case "helppage":
                                info = generalInfoDetails.helppage;
                                break;
                            case "support_email":
                                info = generalInfoDetails.support_email;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.ErrorMsgBox("Pozative Service", "GetGernalInfo_Json.Deserialize " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "GetGernalInfo " + ex.Message);
            }
            return info;
        }
        public static void GetMasterSync()
        {
            try
            {
                string strGetMasterSync = SystemBAL.GetMasterSync();

                var clientLocUpdateVer = new RestSharp.RestClient(strGetMasterSync);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestSharp.RestRequest(RestSharp.Method.GET);
                System.Net.ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                //request.AddHeader("apptype", "aditehr");
                //request.AddHeader("pagetype", "welcome");
                RestSharp.IRestResponse response = clientLocUpdateVer.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        var masterSyncBatchSize = Newtonsoft.Json.JsonConvert.DeserializeObject<BO.MasterSyncBatchSize>(response.Content);
                        Utility.mstSyncBatchSize = new BO.MasterSyncBatchSizeDetails();
                        Utility.mstSyncBatchSize = masterSyncBatchSize.data;
                    }
                    catch (Exception ex)
                    {
                        ObjGoalBase.ErrorMsgBox("Pozative Service", "GetMasterSync_Json.Deserialize " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ObjGoalBase.ErrorMsgBox("Pozative Service", "GetMasterSync " + ex.Message);
            }
            if(Utility.mstSyncBatchSize == null)
                Utility.mstSyncBatchSize = new BO.MasterSyncBatchSizeDetails();
        }
        public static void KillSpeechExe()
        {
            try
            {
                if (!IsAudioDeviceWorking)
                    return;
                Process[] pname = Process.GetProcessesByName("System_Speech.exe");
                if (pname.Length > 0)
                {
                    pname[0].Kill();
                }
                else
                {
                    pname = Process.GetProcessesByName("System_Speech");
                    if(pname.Length > 0)
                    {
                        pname[0].Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToErrorLogFromAll("KillSpeechExe - " + ex.Message);
            }
        }
        public static void CheckAudioDeviceWorking()
        {
            return;
            using (System.Speech.Synthesis.SpeechSynthesizer synth = new System.Speech.Synthesis.SpeechSynthesizer())
            {
                synth.SetOutputToDefaultAudioDevice();
                try
                {
                    synth.Speak("");
                }
                catch (Exception sph)
                {
                    IsAudioDeviceWorking = false;
                }
            }
        }

        //#region Event Listener
        //public static void CheckFeatureOnOffFlags(string Service_Install_Id, string Location_Id, 
        //    ref bool blnOS, ref bool blnPayment, ref bool blnPatientForm, ref bool blnPatient)
        //{
        //    try
        //    {
        //        string strApiActiveApps = PullLiveDatabaseBAL.GetLiveRecord("activeapps", Location_Id);

        //        var clientLocUpdateVer = new RestSharp.RestClient(strApiActiveApps);
        //        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
        //        var request = new RestSharp.RestRequest(RestSharp.Method.GET);
        //        System.Net.ServicePointManager.Expect100Continue = true;
        //        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //        request.AddHeader("apptype", "aditproduct");
        //        RestSharp.IRestResponse response = clientLocUpdateVer.Execute(request);

        //        if (response.ErrorMessage != null)
        //        {
        //            if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
        //            {
        //                GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync_ResponseError (Adit Server To Local Database)] : " + response.ErrorMessage);
        //            }
        //            else
        //            {
        //                GoalBase.WriteToErrorLogFile_Static("[PatientPortal Sync (Adit Server To Local Database)] Service Install Id  : " + Service_Install_Id + " And Clinic  :" + Service_Install_Id + "  " + response.ErrorMessage);
        //            }
        //            return;
        //        }

        //        var ResAPIActiveApps = Newtonsoft.Json.JsonConvert.DeserializeObject<Pozative.BO.ActiveApps_BO>(response.Content);

        //        foreach (var item in ResAPIActiveApps.data)
        //        {
        //            if (item.alias.ToString().ToLower() == "appointment")
        //            {
        //                blnOS = true;
        //            }
        //            if (item.alias.ToString().ToLower() == "aditpay")
        //            {
        //                blnPayment = true;
        //            }
        //            if (item.alias.ToString().ToLower() == "patient-form")
        //            {
        //                blnPatientForm  = true;
        //            }
        //            if (item.alias.ToString().ToLower() == "patients")
        //            {
        //                blnPatient = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.WritetoAditEventDebugLogFile_Static("Error in CheckFeatureOnOffFlags: " + ex.Message);
        //    }
        //}
        //#endregion
    }
}

