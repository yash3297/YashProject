using Pozative.BO;
using Pozative.QRY;
using Pozative.UTL;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Data.SqlClient;

namespace Pozative.DAL
{
    public class PushLiveDatabaseDAL
    {

        public static string GetLive_Push_Record(string TableName)
        {
            string strApiLocOrg = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
            return strApiLocOrg;
        }
        public static string Push_Local_To_LiveDatabase(string JsonString, string TableName, string EHR_ID, string Web_ID, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_Push_API(TableName);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestRequest(Method.POST);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                if (Web_ID != string.Empty)
                {
                    RestURL = RestURL + "/" + Web_ID;
                    request.Method = Method.PUT;
                }
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //request.AddHeader("Postman-Token", "11c3dfdc-236c-433c-b586-747c42983b67");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddParameter("undefined", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    if (TableName.ToLower() == "appointment".ToLower())
                    {
                        var ResMessage = JsonConvert.DeserializeObject<ResponceWithWebIDBO>(response.Content);
                        if (ResMessage.data != null)
                        {
                            _successfullstataus = "Success";
                            if (UpdateAppointmentLocalTableWebID(TableName, EHR_ID, ResMessage.data._id.ToString(), Service_Install_Id))
                            {
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessage.message.ToString();
                        }
                    }

                    else
                    {
                        var ResMessage = JsonConvert.DeserializeObject<ResponceBO>(response.Content);
                        _successfullstataus = ResMessage.message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_WithList(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;

                        switch (TableName.ToLower())
                        {
                            case ("patient"):
                                var ResMessagePatient = JsonConvert.DeserializeObject<Pull_PatientBO>(response.Content);
                                if (ResMessagePatient.data != null)
                                {
                                    SqlCeSelect = string.Empty;
                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                    {
                                        SqlCeCommand.CommandType = CommandType.Text;
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Web_Id;
                                        foreach (var item in ResMessagePatient.data)
                                        {
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.patient_ehr_id.ToString());
                                            SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                            SqlCeCommand.ExecuteNonQuery();
                                        }
                                    }
                                    _successfullstataus = "Success";
                                }
                                else
                                {
                                    _successfullstataus = ResMessagePatient.message.ToString();
                                }
                                break;

                            case ("provider"):
                                var ResMessageProviders = JsonConvert.DeserializeObject<Pull_ProvidersBO>(response.Content);
                                if (ResMessageProviders.data != null)
                                {
                                    SqlCeSelect = string.Empty;
                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                    {
                                        SqlCeCommand.CommandType = CommandType.Text;
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider_Web_Id;
                                        foreach (var item in ResMessageProviders.data)
                                        {
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.provider_ehr_id.ToString());
                                            SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                            SqlCeCommand.ExecuteNonQuery();
                                        }
                                    }
                                    _successfullstataus = "Success";
                                }
                                else
                                {
                                    _successfullstataus = ResMessageProviders.message.ToString();
                                }
                                break;

                            case ("operatory"):
                                var ResMessageOperatory = JsonConvert.DeserializeObject<Pull_OperatoryBO>(response.Content);
                                if (ResMessageOperatory.data != null)
                                {
                                    SqlCeSelect = string.Empty;
                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                    {
                                        SqlCeCommand.CommandType = CommandType.Text;
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory_Web_Id;
                                        foreach (var item in ResMessageOperatory.data)
                                        {
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.operatory_ehr_id.ToString());
                                            SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                            SqlCeCommand.ExecuteNonQuery();
                                        }
                                    }
                                    _successfullstataus = "Success";
                                }
                                else
                                {
                                    _successfullstataus = ResMessageOperatory.message.ToString();
                                }
                                break;

                            case ("appointment"):
                                _successfullstataus = "";
                                break;

                            case ("speciality"):
                                var ResMessageSpeciality = JsonConvert.DeserializeObject<Pull_SpecialityBO>(response.Content);
                                if (ResMessageSpeciality.message.ToLower() == "Success".ToLower())
                                {
                                    _successfullstataus = "Success";
                                }
                                else
                                {
                                    _successfullstataus = ResMessageSpeciality.message.ToString();
                                }
                                break;

                            case ("type"):
                                var ResMessageApptType = JsonConvert.DeserializeObject<Pull_ApptTypeBO>(response.Content);
                                if (ResMessageApptType.data != null)
                                {
                                    SqlCeSelect = string.Empty;
                                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                    {
                                        SqlCeCommand.CommandType = CommandType.Text;
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_Web_Id;
                                        foreach (var item in ResMessageApptType.data)
                                        {
                                            SqlCeCommand.Parameters.Clear();
                                            SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.Apptype_ehr_id.ToString());
                                            SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                            SqlCeCommand.ExecuteNonQuery();
                                        }
                                    }
                                    _successfullstataus = "Success";
                                }
                                else
                                {
                                    _successfullstataus = ResMessageApptType.message.ToString();
                                }
                                break;

                            default:
                                var ResMessageResponce = JsonConvert.DeserializeObject<ResponceBO>(response.Content);
                                _successfullstataus = ResMessageResponce.message.ToString();
                                break;

                        }



                        //SqlCetx.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static bool UpdateLocalTableWebID(string TableName, string EHR_ID, string Web_ID, string Clinic_Number, string Service_Install_Id)
        {
            bool _successfullstataus = true;

            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlExCommand = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);

                //    SqlTransaction SqlExtx;
                if (conn.State == ConnectionState.Closed) conn.Open();
                //    SqlExtx = conn.BeginTransaction();

                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                    //  SqlExCommand.Transaction = SqlExtx;

                    switch (TableName.ToLower())
                    {
                        case ("patient"):
                            SqlExCommand.CommandText = SynchLocalQRY.Update_Patient_Web_Id;
                            break;
                        case ("provider"):
                            SqlExCommand.CommandText = SynchLocalQRY.Update_Provider_Web_Id;
                            break;
                        case ("operatory"):
                            SqlExCommand.CommandText = SynchLocalQRY.Update_Operatory_Web_Id;
                            break;
                        case ("appointment"):
                            SqlExCommand.CommandText = SynchLocalQRY.Update_Appointment_Web_Id;
                            break;
                        case ("type"):
                            SqlExCommand.CommandText = SynchLocalQRY.Update_ApptType_Web_Id;
                            break;
                        case ("pozativeappointment"):
                            SqlExCommand.CommandText = SynchLocalQRY.Update_PozativeAppointment_Web_Id;
                            break;
                        default:
                            SqlExCommand.CommandText = string.Empty;
                            break;
                    }

                    SqlExCommand.Parameters.Clear();
                    SqlExCommand.Parameters.AddWithValue("EHR_ID", EHR_ID);
                    SqlExCommand.Parameters.AddWithValue("Web_ID", Web_ID);
                    SqlExCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                    SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    SqlExCommand.ExecuteNonQuery();
                    _successfullstataus = true;

                    // SqlExtx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    //  SqlExtx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;

                            switch (TableName.ToLower())
                            {
                                case ("patient"):
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Web_Id;
                                    break;
                                case ("provider"):
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider_Web_Id;
                                    break;
                                case ("operatory"):
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory_Web_Id;
                                    break;
                                case ("appointment"):
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Web_Id;
                                    break;
                                case ("type"):
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_Web_Id;
                                    break;
                                case ("pozativeappointment"):
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_PozativeAppointment_Web_Id;
                                    break;
                                default:
                                    SqlCeCommand.CommandText = string.Empty;
                                    break;
                            }

                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("EHR_ID", EHR_ID);
                            SqlCeCommand.Parameters.AddWithValue("Web_ID", Web_ID);
                            SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.ExecuteNonQuery();
                            _successfullstataus = true;
                        }

                        // SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
                        _successfullstataus = false;
                        //  SqlCetx.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            return _successfullstataus;
        }

        public static bool UpdateAppointmentLocalTableWebID(string TableName, string EHR_ID, string Web_ID, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            if (Utility.isSqlServer)
            {
                SqlConnection conn = null;
                SqlCommand SqlExCommand = null;
                CommonDB.LocalConnectionServer_SqlServer(ref conn);
                //   SqlTransaction SqlExtx;
                if (conn.State == ConnectionState.Closed) conn.Open();
                //   SqlExtx = conn.BeginTransaction();

                try
                {
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string sqlSelect = string.Empty;
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlExCommand, "txt");
                    //   SqlExCommand.Transaction = SqlExtx;
                    SqlExCommand.CommandText = SynchLocalQRY.Update_Appointment_Web_Id;
                    SqlExCommand.Parameters.Clear();
                    SqlExCommand.Parameters.AddWithValue("EHR_ID", EHR_ID);
                    SqlExCommand.Parameters.AddWithValue("Web_ID", Web_ID);
                    SqlExCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                    SqlExCommand.ExecuteNonQuery();
                    _successfullstataus = true;
                    // SqlExtx.Commit();
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    // SqlExtx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            else
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    try
                    {
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Web_Id;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("EHR_ID", EHR_ID);
                            SqlCeCommand.Parameters.AddWithValue("Web_ID", Web_ID);
                            SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                            SqlCeCommand.ExecuteNonQuery();
                            _successfullstataus = true;
                        }

                        // SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
                        _successfullstataus = false;
                        //SqlCetx.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) conn.Close();
                    }
                }
            }
            return _successfullstataus;
        }

        public static string Push_Location_EHR(string JsonString, string LocationID = "")
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_Push_API("EHR");

                var request = new RestRequest(Method.POST);
                if (LocationID != string.Empty)
                {
                    RestURL = RestURL + "/" + LocationID;
                    request.Method = Method.PUT;
                }
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //request.AddHeader("Postman-Token", "11c3dfdc-236c-433c-b586-747c42983b67");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(LocationID));
                request.AddParameter("undefined", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = string.Empty;
                }
                else
                {
                    var ResMessage = JsonConvert.DeserializeObject<ResponceWithWebIDBO>(response.Content);
                    _successfullstataus = ResMessage.data._id.ToString();
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = string.Empty;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_UpdateclientEHRVersion(string Location_id, string updateclientehrversion)
        {
            string _successfullstataus = string.Empty;
            string JsonString = "";
            try
            {

                //var JsonPatient_FormBO = new System.Text.StringBuilder();                    

                // var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //ClientVersion Patient_FormBO = new ClientVersion();

                //Patient_FormBO.statusupdate = new List<ClientVersion>();

                // ClientVersion updatest = new ClientVersion();
                //updatest.appointmentlocation = Location_id;
                //updatest.client_application_version = updateclientehrversion;
                // JsonPatient_FormBO.Append(javaScriptSerializer.Serialize(updatest) + ",");
                //}
                //JsonString = "" + JsonPatient_FormBO.ToString().Remove(JsonPatient_FormBO.Length - 1) + "";
                ClientVersion clientV = new ClientVersion
                {
                    appointmentlocation= Location_id,
                    client_application_version = Utility.EHR_VersionNumber

                };
                var javaScriptSerializeClientAV = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonStringClientAV = javaScriptSerializeClientAV.Serialize(clientV);

                JsonString = "" + jsonStringClientAV.ToString() +"";

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API("updateclientehrversion");
                RestURL = RestURL.Replace("Location_Id", Location_id.ToString());

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }


                else
                {
                    //var ResMessagePatient = JsonConvert.DeserializeObject<ClientVersion>(response.Content);
                    try
                    {

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;

                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Location_EHRUPdateForVersion()
        {
            string _successfullstataus = string.Empty;

            try
            {
                DataTable dtlocation = SystemDAL.GetLocationDetail();
                foreach (DataRow drRow in dtlocation.Rows)
                {
                    LocationEHRUPdateForVersionBO LEUFVBO = new LocationEHRUPdateForVersionBO
                    {
                        location_id = drRow["Loc_ID"].ToString(),
                        org_id = drRow["Organization_ID"].ToString(),
                        appointmentlocation_id = drRow["Location_ID"].ToString(),
                        location_name = drRow["name"].ToString(),
                        org_name = Utility.Organization_Name.ToString(),
                        ehr_name = Utility.Application_Name.ToString(),
                        ehr_version = Utility.Application_Version.ToString(),
                        is_auto_update = 0,
                        last_updated = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-ddTHH:mm:ss"),
                        server_app_version = Utility.Server_App_Version.ToString(),
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
                    string jsonStringLEUFVBO = javaScriptSerializerLEUFVBO.Serialize(LEUFVBO);

                    string JsonString = "[" + jsonStringLEUFVBO.ToString() + "]";


                    string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API("ehrupdateversion");
                    var request = new RestRequest(Method.POST);
                    var client = new RestClient(RestURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(drRow["Location_ID"].ToString()));
                    request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    if (response.ErrorMessage != null)
                    {
                        _successfullstataus = string.Empty;
                    }
                    _successfullstataus = "success";
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = string.Empty;
            }

            return _successfullstataus;
        }

        public static string UpdateLocNewVersionOnAdit_Server_App(string JsonString)
        {
            string _successfullstataus = string.Empty;
            try
            {

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API("updateehrupdateversion");
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = string.Empty;
                }
                _successfullstataus = "success";
            }
            catch (Exception ex)
            {
                _successfullstataus = string.Empty;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_PozativeLiveDatabase(string JsonString, string Appointment_Id, string Clinic_Number, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;
            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_Push_PozativeAPI();

                var client = new RestClient(RestURL);
                var request = new RestRequest(Method.POST);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("postman-token", "95d24702-ee27-50a4-7574-1932b0e79b69");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessage = JsonConvert.DeserializeObject<ResponceWithWebIDBO>(response.Content);
                    if (ResMessage.data != null)
                    {
                        _successfullstataus = "Success";
                        if (UpdateLocalTableWebID("PozativeAppointment", Appointment_Id, ResMessage.data._id.ToString(), Clinic_Number, Service_Install_Id))
                        {
                        }
                    }
                    else
                    {
                        _successfullstataus = ResMessage.message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
            }
            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_PatientStatus(string JsonString, string TableName, string Service_Install_Id, string Location_id, DataTable dtPatientStatus)
        {
            string _successfullstataus = string.Empty;
            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                var ResMessagePatient = JsonConvert.DeserializeObject<Push_PatientStatus_Response>(response.Content);

                if (ResMessagePatient.data != null)
                {
                    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Status_Web_Id;
                            foreach (var item in ResMessagePatient.data)
                            {
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.patient_ehr_id);
                                SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                        _successfullstataus = "Success";
                    }
                }
                else
                {
                    _successfullstataus = ResMessagePatient.message.ToString();
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }
            return _successfullstataus;
        }

        //public static string Push_Local_To_LiveDatabase_PatientBalance(string JsonString, string TableName, string Service_Install_Id, string Location_id, DataTable dtPatientBalance)
        //{
        //    string _successfullstataus = string.Empty;
        //    try
        //    {
        //        string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
        //        var request = new RestRequest(Method.POST);
        //        var client = new RestClient(RestURL);
        //        ServicePointManager.Expect100Continue = true;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("content-type", "application/json");
        //        request.Timeout = 900000;
        //        request.AddHeader("Authorization", Utility.WebAdminUserToken);
        //        request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
        //        request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
        //        IRestResponse response = client.Execute(request);

        //        if (response.ErrorMessage != null)
        //        {
        //            _successfullstataus = response.ErrorMessage;
        //        }
        //        var ResMessagePatient = JsonConvert.DeserializeObject<Push_PatientBalance_Response>(response.Content);

        //        if (ResMessagePatient.data != null)
        //        {
        //            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
        //            {
        //                if (conn.State == ConnectionState.Closed) conn.Open();
        //                DateTime dtCurrentDtTime = Utility.Datetimesetting();
        //                string SqlCeSelect = string.Empty;
        //                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
        //                {
        //                    SqlCeCommand.CommandType = CommandType.Text;
        //                    SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptPatientBalance_Web_Id;
        //                    foreach (var item in ResMessagePatient.data)
        //                    {
        //                        SqlCeCommand.Parameters.Clear();
        //                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.patient_ehr_id);
        //                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id);
        //                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
        //                        SqlCeCommand.ExecuteNonQuery();
        //                    }
        //                }
        //                _successfullstataus = "Success";
        //            }
        //        }
        //        else
        //        {
        //            _successfullstataus = ResMessagePatient.message.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _successfullstataus = ex.Message;
        //        throw ex;
        //    }
        //    return _successfullstataus;
        //}

        public static string Push_Local_To_LiveDatabase_Patient(string JsonString, string TableName, string Service_Install_Id, string Location_id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessagePatient = JsonConvert.DeserializeObject<Pull_PatientBO>(response.Content);
                    try
                    {
                        if (ResMessagePatient.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Patient_Web_Id;
                                    foreach (var item in ResMessagePatient.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.patient_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessagePatient.message.ToString();
                        }
                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        // SqlCetx.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;

                throw ex;
            }

            return _successfullstataus;
        }
        public static string Push_Local_To_LiveDatabase_PatientDisease(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                //-Autho- request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey());
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageDisease = JsonConvert.DeserializeObject<Pull_PatientDiseaseBO>(response.Content);
                    try
                    {
                        if (ResMessageDisease.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientDisease_Web_Id;
                                    foreach (var item in ResMessageDisease.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Patient_EHR_ID", item.Patient_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.disease_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Disease_Type", item.disease_type.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    _successfullstataus = "Success";
                                }
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageDisease.message.ToString();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;

                throw ex;
            }
            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_Disease(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                //-Autho- request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey());
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageDisease = JsonConvert.DeserializeObject<Pull_DiseaseBO>(response.Content);
                    try
                    {
                        if (ResMessageDisease.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Disease_Web_Id;
                                    foreach (var item in ResMessageDisease.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.disease_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Disease_Type", item.disease_type.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    _successfullstataus = "Success";
                                }
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageDisease.message.ToString();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;

                throw ex;
            }
            return _successfullstataus;
        }
        public static string Push_Local_To_LiveDatabase_Medication(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                //-Autho- request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey());
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.Location_ID));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageMedication = JsonConvert.DeserializeObject<Pull_MedicationBO>(response.Content);
                    try
                    {
                        if (ResMessageMedication.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Medication_Web_Id;
                                    foreach (var item in ResMessageMedication.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.medication_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Medication_Type", item.medication_type.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    _successfullstataus = "Success";
                                }
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageMedication.message.ToString();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;

                throw ex;
            }
            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_PatientMedication(string JsonString, string TableName, string Location_Id, string Clinic_Number, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    // _successfullstataus = "Success";
                    var ResMessagePatientMedication = JsonConvert.DeserializeObject<Pull_PatientMedicationBO>(response.Content);
                    try
                    {
                        if (ResMessagePatientMedication.message == "Success")
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                string SqlCeSelect = string.Empty;

                                using (SqlCeCommand SqlCeComBulk = new SqlCeCommand("", conn))
                                {
                                    //SqlCeComBulk.CommandText = "Select Patient_EHR_ID, Disease_EHR_ID, Disease_Type, Clinic_Number, Service_Install_Id, PatientDisease_Web_ID, Is_Adit_Updated from PatientDiseaseMaster where Clinic_Number = '" + Clinic_Number + "' And Is_Adit_Updated = 0;";
                                    SqlCeComBulk.CommandType = CommandType.TableDirect;
                                    SqlCeComBulk.CommandText = "PatientMedication"; //"Patient";
                                    SqlCeComBulk.Connection = conn;
                                    SqlCeComBulk.IndexName = "idx_PatientMedication_ID";
                                    SqlCeResultSet rs = SqlCeComBulk.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Updatable);
                                    DataTable SqlCeDt = new DataTable();
                                    using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeComBulk))
                                    {
                                        SqlCeDt = new DataTable();
                                        SqlCeDa.Fill(SqlCeDt);
                                    }
                                    foreach (var item in ResMessagePatientMedication.data)
                                    {
                                        try
                                        {
                                            if (rs.Seek(DbSeekOptions.FirstEqual, item.patientmedication_ehr_id.ToString()))
                                            {
                                                rs.Read();
                                                rs.SetValue(rs.GetOrdinal("PatientMedication_Web_ID"), item._id.ToString());
                                                rs.SetValue(rs.GetOrdinal("Is_Adit_Updated"), 1);
                                                rs.Update();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            continue;
                                        }
                                    }
                                }

                                //using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                //{
                                //    foreach (var item in ResMessagePatientMedication.data)
                                //    {
                                //        SqlCeCommand.CommandType = CommandType.Text;
                                //        SqlCeCommand.CommandText = SynchLocalQRY.Update_PatientMedications_Web_Id;
                                //        SqlCeCommand.Parameters.Clear();
                                //        SqlCeCommand.Parameters.AddWithValue("PatientMedication_Web_ID", item._id.ToString());
                                //        SqlCeCommand.Parameters.AddWithValue("PatientMedication_EHR_ID", item.patientmedication_ehr_id.ToString());
                                //        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                //        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                //        SqlCeCommand.ExecuteNonQuery();
                                //    }
                                //}
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessagePatientMedication.message.ToString();
                        }
                        //  SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
                        // SqlCetx.Rollback();
                        Utility.WriteToSyncLogFile_All("Err in Push PatMed: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }
        public static string Push_Local_To_LiveDatabase_OperatoryDayOff(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;
            try
            {
                //    JsonString = JsonString.Replace(",", ",\n");
                //    JsonString = JsonString.Replace("{\"", "{\n\"");
                //    JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    var ResMessageOperatoryEvent = JsonConvert.DeserializeObject<Pull_OperatoryEventBO>(response.Content);
                    try
                    {
                        if (ResMessageOperatoryEvent.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                //      SqlCetx = conn.BeginTransaction();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryDayoff_Web_Id;
                                    foreach (var item in ResMessageOperatoryEvent.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.appt_ehr_id);
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageOperatoryEvent.message.ToString();
                        }
                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        // SqlCetx.Rollback();
                        throw;
                    }

                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_OperatoryEvent(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //    JsonString = JsonString.Replace(",", ",\n");
                //    JsonString = JsonString.Replace("{\"", "{\n\"");
                //    JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageOperatoryEvent = JsonConvert.DeserializeObject<Pull_OperatoryEventBO>(response.Content);
                    try
                    {
                        if (ResMessageOperatoryEvent.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                //  SqlCetx = conn.BeginTransaction();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryEvent_Web_Id;
                                    foreach (var item in ResMessageOperatoryEvent.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.appt_ehr_id);
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    _successfullstataus = "Success";
                                }
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageOperatoryEvent.message.ToString();
                        }
                        //  SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //  SqlCetx.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_OperatoryHours(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    var ResMessageOperatoryhours = JsonConvert.DeserializeObject<Pull_OperatoryHoursBO>(response.Content);
                    try
                    {
                        if (ResMessageOperatoryhours.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryHours_Web_Id;
                                    foreach (var item in ResMessageOperatoryhours.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.oh_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageOperatoryhours.message.ToString();
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        // SqlCetx.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_OperatoryOfficeHours(string JsonString, string TableName, string Service_Install_Id, string Location_ID)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_ID));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageProviderOfficeHours = JsonConvert.DeserializeObject<Responce_OperatoryOfficeHours>(response.Content);
                    try
                    {
                        if (ResMessageProviderOfficeHours.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_OperatoryOfficeHours_Web_Id;
                                    foreach (var item in ResMessageProviderOfficeHours.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Operatory_EHR_ID", item.Operatory_EHR_ID);
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";

                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageProviderOfficeHours.message.ToString();
                        }
                    }
                    catch (Exception)
                    {
                        // SqlCetx.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }


        public static string Push_Local_To_LiveDatabase_Holiday(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //    JsonString = JsonString.Replace(",", ",\n");
                //    JsonString = JsonString.Replace("{\"", "{\n\"");
                //    JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {

                    var ResMessageHoliday = JsonConvert.DeserializeObject<Responce_HolidayBO>(response.Content);
                    try
                    {
                        if (ResMessageHoliday.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Holiday_Web_Id;
                                    foreach (var item in ResMessageHoliday.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.appt_ehr_id);
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageHoliday.message.ToString();
                        }
                        // SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
                        //  SqlCetx.Rollback();
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_Provider(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id, string Location_id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageProviders = JsonConvert.DeserializeObject<Pull_ProvidersBO>(response.Content);
                    try
                    {
                        if (ResMessageProviders.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Provider_Web_Id;
                                    foreach (var item in ResMessageProviders.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.provider_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageProviders.message.ToString();
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //SqlCetx.Rollback();
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }


        public static string Push_Local_To_LiveDatabase_User(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id, string Location_id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageProviders = JsonConvert.DeserializeObject<Pull_UsersLocal_To_LiveBO>(response.Content);
                    try
                    {
                        if (ResMessageProviders.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_User_Web_Id;
                                    foreach (var item in ResMessageProviders.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.user_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageProviders.message.ToString();
                        }

                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //SqlCetx.Rollback();
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_ProviderHours(string JsonString, string TableName, string Service_Install_Id, string Location_id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                RestURL = RestURL + Utility.Organization_ID.ToString();
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    var ResMessageProviderhours = JsonConvert.DeserializeObject<Pull_ProviderHoursBO>(response.Content);
                    try
                    {
                        if (ResMessageProviderhours.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_ProviderHours_Web_Id;
                                    foreach (var item in ResMessageProviderhours.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.ph_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageProviderhours.message.ToString();
                        }
                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //  SqlCetx.Rollback();
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_FolderList(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //    JsonString = JsonString.Replace(",", ",\n");
                //    JsonString = JsonString.Replace("{\"", "{\n\"");
                //    JsonString = JsonString.Replace("\"}", "\"\n}");

                //string RestURL = "http://192.168.1.170:5351/v1/webhooks/ehrfolder";


                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageOperatory = JsonConvert.DeserializeObject<Pull_FolderListBO>(response.Content);
                    try
                    {
                        if (ResMessageOperatory.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_FolderList_Web_Id;
                                    foreach (var item in ResMessageOperatory.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("FolderList_EHR_ID", item.ehrfolder_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("FolderList_Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageOperatory.message.ToString();
                        }
                        // SqlCetx.Commit();

                    }
                    catch (Exception ex)
                    {
                        //SqlCetx.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }


        public static string Push_Local_To_LiveDatabase_Operatory(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //    JsonString = JsonString.Replace(",", ",\n");
                //    JsonString = JsonString.Replace("{\"", "{\n\"");
                //    JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageOperatory = JsonConvert.DeserializeObject<Pull_OperatoryBO>(response.Content);
                    try
                    {
                        if (ResMessageOperatory.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Operatory_Web_Id;
                                    foreach (var item in ResMessageOperatory.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.operatory_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageOperatory.message.ToString();
                        }
                        // SqlCetx.Commit();

                    }
                    catch (Exception ex)
                    {
                        //SqlCetx.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_ApptType(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    var ResMessageApptType = JsonConvert.DeserializeObject<Pull_ApptTypeBO>(response.Content);
                    try
                    {
                        if (ResMessageApptType.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptType_Web_Id;
                                    foreach (var item in ResMessageApptType.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.Apptype_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageApptType.message.ToString();
                        }
                        //  SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //  SqlCetx.Rollback();
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }


        public static bool SendRecordsToAditAppForAppointmentDoubleBook(string OriginalWebId, string conflictWebId, string originalAppEHRId, string appointmentLocationId, DataTable dtApponitmentConflict)
        {
            try
            {
                // string jsonString = "[" + JsonOperatory.ToString().Remove(JsonOperatory.Length - 1) + "]";
                // string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API("AppointmentDoubleBook");

                string jsonstringn = "";
                string APIResponse = "";
                string strApiProviders = LiveDatabaseAPI.LiveRecord_WithList_Push_API("AppointmentDoubleBook");
                var client = new RestClient(strApiProviders);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                var request = new RestRequest(Method.POST);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Utility.DtLocationList.Rows[0]["Location_Id"].ToString()));
                jsonstringn = "{\"original_web_id\":\"" + OriginalWebId + "\",\"conflict_web_id\":\"" + conflictWebId + "\",\"original_appt_ehr_id\":\"" + originalAppEHRId + "\",\"appointmentlocationid\":\"" + appointmentLocationId + "\",\"first_name\":\"" + dtApponitmentConflict.Rows[0]["FirstName"].ToString() + "\",\"last_name\":\"" + dtApponitmentConflict.Rows[0]["LastName"].ToString() + "\",\"mobile\":\"" + dtApponitmentConflict.Rows[0]["Mobile"].ToString() + "\",\"email\":\"" + dtApponitmentConflict.Rows[0]["Email"].ToString() + "\",\"provider_name\":\"" + dtApponitmentConflict.Rows[0]["ProviderFirstName"].ToString() + " " + dtApponitmentConflict.Rows[0]["ProviderLastName"].ToString() + "\"}";

                request.AddParameter("application/json", jsonstringn, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.ErrorMessage != null)
                {
                    APIResponse = "Err_API Call : " + response.ErrorMessage;
                    if (response.ErrorMessage.Contains("The remote name could not be resolved:"))
                    { }
                    else
                    {
                        // Utility.WriteToSyncLogFile_All("[PatientPaymentLogStatus Sync LocalDb TO EHR_Err " + response.ErrorMessage + "  Service Install Id : " + servericeInstallId.Trim() + " And Clinic : " + clinicNumber.Trim() + "  " + response.ErrorMessage);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public static string Push_Local_To_LiveDatabase_Speciality(string JsonString, string TableName, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    var ResMessageSpeciality = JsonConvert.DeserializeObject<Pull_SpecialityBO>(response.Content);
                    try
                    {
                        if (ResMessageSpeciality.message.ToLower() == "Success".ToLower())
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Speciality_Web_Id;
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageSpeciality.message.ToString();
                        }
                        //  SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
                        // SqlCetx.Rollback();
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_RecallType(string JsonString, string TableName, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    //_successfullstataus = "Success";
                    var ResMessageRecallType = JsonConvert.DeserializeObject<Pull_RecallTypeBO>(response.Content);
                    try
                    {
                        if (ResMessageRecallType.message.ToLower() == "Success.".ToLower())
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_RecallType_Web_Id;
                                    SqlCeCommand.Parameters.Clear();
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageRecallType.message.ToString();
                        }
                        //  SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
                        // SqlCetx.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_ApptStatus(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    _successfullstataus = "Success";
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_StatusAppointmentlist(string JsonString, string TableName, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, " Call StatusAppointment API");

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Request Sent into the API " + " Authorization, TokenKey, action");


                IRestResponse response = client.Execute(request);
                if (response.Content != null)
                {
                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, "Response received from API (" + response.Content.ToString() + ")");
                }
                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                _successfullstataus = "Success";

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                Utility.WriteToErrorLogFromAll("[StatusAppointmentList Err]:" + ex.Message);
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_ApptStatus_With_Type(string JsonString, string TableName, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    _successfullstataus = "Success";
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_Appointment(string JsonString, string TableName, string Clinic_Number, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                //Utility.WriteToSyncLogFile_All("Push_Local_To_LiveDatabase_Appointment : " + JsonString);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Utility.WriteToSyncLogFile_All("Push_Local_To_LiveDatabase_Appointment Content: " + response.Content.ToString());

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                    Utility.WriteToErrorLogFromAll("Push_Local_To_LiveDatabase_Appointment ErrorMessage : " + response.ErrorMessage);
                }

                else
                {
                    try
                    {
                        var ResMessageAppointment = JsonConvert.DeserializeObject<Pull_MultiApptBO>(response.Content);
                        if (ResMessageAppointment.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Appointment_Web_Id_Where_Clinic_Number;
                                    foreach (var item in ResMessageAppointment.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.appt_ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageAppointment.message.ToString();
                        }
                        //SqlCetx.Commit();
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteToErrorLogFromAll("Err in PushLocalToLiveDatabaseAppointment: " + ex.StackTrace);
                        //SqlCetx.Rollback();
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_ProviderOfficeHours(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    var ResMessageProviderOfficeHours = JsonConvert.DeserializeObject<Responce_ProviderOfficeHours>(response.Content);
                    try
                    {
                        if (ResMessageProviderOfficeHours.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_ProviderOfficeHours_Web_Id;
                                    foreach (var item in ResMessageProviderOfficeHours.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Provider_EHR_ID", item.provider_ehr_id);
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageProviderOfficeHours.message.ToString();
                        }
                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        // SqlCetx.Rollback();
                        throw;
                    }

                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_Is_Appt_DoubleBook(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    try
                    {
                        var ResMessageAppointment = JsonConvert.DeserializeObject<Response_Is_Appt_DoubleBook>(response.Content);
                        if (ResMessageAppointment.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Is_Appt_DoubleBook;
                                    foreach (var item in ResMessageAppointment.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    _successfullstataus = "Success";
                                }
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageAppointment.message.ToString();
                        }
                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //SqlCetx.Rollback();
                        throw;
                    }


                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_MedicalHistory(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;
            string WebIdColumnsName = "";
            string EHRIdColumnsName = "";
            string EHRValue = "";

            #region set WebId ColumnsName
            switch (TableName.ToUpper())
            {
                case "EAGLESOFTFORMMASTER":
                    WebIdColumnsName = "FormMaster_web_Id";
                    EHRIdColumnsName = "Form_EHR_Id";
                    break;
                case "EAGLESOFTSECTIONMASTER":
                    WebIdColumnsName = "SectionMaster_Web_Id";
                    EHRIdColumnsName = "Section_EHR_Id";
                    break;
                case "EAGLESOFTALERTMASTER":
                    WebIdColumnsName = "AlertMaster_web_Id";
                    EHRIdColumnsName = "Alert_EHR_Id";
                    break;
                case "EAGLESOFTSECTIONITEMMASTER":
                    WebIdColumnsName = "SectionItem_WEB_Id";
                    EHRIdColumnsName = "SectionItem_EHR_Id";
                    break;
                case "EAGLESOFTSECTIONITEMOPTIONMASTER":
                    WebIdColumnsName = "SectionItemOption_WEB_Id";
                    EHRIdColumnsName = "SectionItemOption_EHR_Id";
                    break;
                case "DENTRIX_FORM":
                    WebIdColumnsName = "Dentrix_Form_Web_ID";
                    EHRIdColumnsName = "Dentrix_Form_EHRUnique_ID";
                    break;
                case "DENTRIX_FORMQUESTION":
                    WebIdColumnsName = "Dentrix_FormQuestion_Web_ID";
                    EHRIdColumnsName = "Dentrix_Question_EHRUnique_ID";
                    break;
                case "ABELDENT_FORM":
                    WebIdColumnsName = "ABELDENT_Form_Web_ID";
                    EHRIdColumnsName = "ABELDENT_Form_EHRUnique_ID";
                    break;
                case "ABELDENT_FORMQUESTION":
                    WebIdColumnsName = "ABELDENT_FormQuestion_Web_ID";
                    EHRIdColumnsName = "ABELDENT_Question_EHRUnique_ID";
                    break;
                case "OD_SHEETDEF":
                    WebIdColumnsName = "SheetDefNum_Web_ID";
                    EHRIdColumnsName = "SheetDefNum_EHR_ID";
                    break;
                case "OD_SHEETFIELDDEF":
                    WebIdColumnsName = "SheetFieldDefNum_Web_ID";
                    EHRIdColumnsName = "SheetFieldDefNum_EHR_ID";
                    break;
                case "CD_FORMMASTER":
                    WebIdColumnsName = "CD_FormMaster_Web_ID";
                    EHRIdColumnsName = "CD_FormMaster_EHR_ID";
                    break;
                case "CD_QUESTIONMASTER":
                    WebIdColumnsName = "CD_QuestionMaster_Web_ID";
                    EHRIdColumnsName = "CD_QuestionMaster_EHR_ID";
                    break;
                case "EASYDENTAL_QUESTION":
                    WebIdColumnsName = "EasyDental_Question_Web_ID";
                    EHRIdColumnsName = "EasyDental_QuestionId";
                    break;
                case "EASYDENTAL_FORM":
                    WebIdColumnsName = "CD_FormMaster_Web_ID";
                    EHRIdColumnsName = "CD_FormMaster_EHR_ID";
                    break;
                case "PRACTICEWEB_SHEETDEF":
                    WebIdColumnsName = "SheetDefNum_Web_ID";
                    EHRIdColumnsName = "SheetDefNum_EHR_ID";
                    break;
                case "PRACTICEWEB_SHEETFIELDDEF":
                    WebIdColumnsName = "SheetFieldDefNum_Web_ID";
                    EHRIdColumnsName = "SheetFieldDefNum_EHR_ID";
                    break;

            }
            #endregion

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                // Utility.WriteToSyncLogFile_All("Start Push " + TableName + " URL " + RestURL);

                if (response.ErrorMessage != null)
                {
                    //  Utility.WriteToSyncLogFile_All("Start Push " + TableName + " REsponse Error " + response.ErrorMessage);
                    _successfullstataus = response.ErrorMessage;
                }

                else
                {
                    try
                    {
                        var ResMessageAppointment = JsonConvert.DeserializeObject<Response_EaglesoftMedicalHistory>(response.Content); ;
                        if (ResMessageAppointment.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;

                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    foreach (var item in ResMessageAppointment.data)
                                    {
                                        if (TableName.ToUpper() == "EAGLESOFTFORMMASTER")
                                        {
                                            EHRValue = item.formmaster_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "EAGLESOFTSECTIONMASTER")
                                        {
                                            EHRValue = item.sectionmaster_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "EAGLESOFTALERTMASTER")
                                        {
                                            EHRValue = item.alertmaster_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "EAGLESOFTSECTIONITEMMASTER")
                                        {
                                            EHRValue = item.sectionitemmaster_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "EAGLESOFTSECTIONITEMOPTIONMASTER")
                                        {
                                            EHRValue = item.sectionitemoptionmaster_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "DENTRIX_FORM")
                                        {
                                            EHRValue = item.dentrix_form_ehrunique_id;
                                        }
                                        else if (TableName.ToUpper() == "DENTRIX_FORMQUESTION")
                                        {
                                            EHRValue = item.dentrix_formquestion_ehrunique_id;
                                        }
                                        else if (TableName.ToUpper() == "ABELDENT_FORM")
                                        {
                                            EHRValue = item.abeldent_form_ehrunique_id;
                                        }
                                        else if (TableName.ToUpper() == "ABELDENT_FORMQUESTION")
                                        {
                                            EHRValue = item.abeldent_formquestion_ehrunique_id;
                                        }
                                        else if (TableName.ToUpper() == "OD_SHEETDEF")
                                        {
                                            EHRValue = item.sheetdefnum_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "OD_SHEETFIELDDEF")
                                        {
                                            EHRValue = item.sheetfielddefnum_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "CD_FORMMASTER")
                                        {
                                            EHRValue = item.cd_formmaster_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "CD_QUESTIONMASTER")
                                        {
                                            EHRValue = item.cd_questionmaster_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "EASYDENTAL_QUESTION")
                                        {
                                            EHRValue = item.easydental_questionid;
                                        }
                                        else if (TableName.ToUpper() == "EASYDENTAL_FORM")
                                        {
                                            TableName = "CD_FORMMASTER";
                                            EHRValue = item.easydental_formmasterid;
                                        }
                                        else if (TableName.ToUpper() == "PRACTICEWEB_SHEETDEF")
                                        {
                                            TableName = "OD_SHEETDEF";
                                            EHRValue = item.sheetdefnum_ehr_id;
                                        }
                                        else if (TableName.ToUpper() == "PRACTICEWEB_SHEETFIELDDEF")
                                        {
                                            TableName = "OD_SHEETFIELDDEF";
                                            EHRValue = item.sheetfielddefnum_ehr_id;
                                        }
                                        SqlCeCommand.CommandText = SynchLocalQRY.Update_Is_Appt_MedicalHistoryTable.Replace("MedicalHistoryTableName", TableName.ToString()).Replace("WebIdColumnsName", WebIdColumnsName).Replace("EHRColumnsName", EHRIdColumnsName).Replace("EHRColumnId", "'" + EHRValue + "'");
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                    _successfullstataus = "Success";
                                }

                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageAppointment.message.ToString();
                        }
                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //SqlCetx.Rollback();
                        throw;
                    }


                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static void UpdatePatientReceive_SMStStatusToWeb(Patient_OptOutBO_StatusUpdate updatedPatientid, string LocationId,string Loc_ID)
        {
            try
            {
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(updatedPatientid);
                try
                {
                    string RestURL = LiveDatabaseAPI.LiveRecord_Pull_API("patientoptout_confirm",Loc_ID);
                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, " Call API Patient Status Ack to Web ");
                    var request = new RestRequest(Method.POST);
                    var client = new RestClient(RestURL);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("Authorization", Utility.WebAdminUserToken);
                    request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(LocationId));
                    request.Timeout = 900000;
                    request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                    Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, " Call API Patient Status Ack to Web API Request " + jsonString.ToString());
                    IRestResponse response = client.Execute(request);
                    if (response != null)
                    {
                        Utility.WriteToSyncLogFile_All("Updatepatientreceives_smsStatusToWeb " + response.ToString());
                        Utility.WriteSyncPullLog(Utility._filename_EHR_appointment, Utility._EHRLogdirectory_EHR_appointment, " Call API Patient Status Ack to Web API Response " + response.Content.ToString());
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Push_Local_To_LiveDatabase_PatientBalance(string JsonString, string TableName, string Service_Install_Id, string Location_id, DataTable dtPatientBalance)
        {
            string _successfullstataus = string.Empty;
            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                var ResMessagePatient = JsonConvert.DeserializeObject<Push_PatientBalance_Response>(response.Content);

                if (ResMessagePatient.data != null)
                {
                    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        string SqlCeSelect = string.Empty;
                        using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        {
                            SqlCeCommand.CommandType = CommandType.Text;
                            SqlCeCommand.CommandText = SynchLocalQRY.Update_ApptPatientBalance_Web_Id;
                            foreach (var item in ResMessagePatient.data)
                            {
                                SqlCeCommand.Parameters.Clear();
                                SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.patient_ehr_id);
                                SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id);
                                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                        _successfullstataus = "Success";
                    }
                }
                else
                {
                    _successfullstataus = ResMessagePatient.message.ToString();
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }
            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_PozativeConfiguration(string JsonString, string TableName, string Location_id)
        {
            string _successfullstataus = string.Empty;
            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);
                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.Timeout = 900000;
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                var ResMessageRoot = JsonConvert.DeserializeObject<Push_ConfigurationRoot_Response>(response.Content);

                if (ResMessageRoot.message.ToLower() == "Success".ToLower())
                {
                    _successfullstataus = "Success";
                }
                else
                {
                    _successfullstataus = ResMessageRoot.message.ToString();
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }
            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_EventAcknowledgement(string JsonString, string TableName, string Location_id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    _successfullstataus = "Success";
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        #region ZohoDetails
        //Push_Local_To_LiveDatabase_EHRAndSystemCreds_Results
        public static string Push_Local_To_LiveDatabase_SaveUser_Results(string JsonString, string TableName, string Location_id, string Clinic_Number, string Service_Install_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {

                    var ResMessagePatient = JsonConvert.DeserializeObject<SaveServerUsersMain>(response.Content);
                    try
                    {
                        if (ResMessagePatient != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.UpdateLocalSystemUsersDataWebId;
                                    foreach (var item in ResMessagePatient.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("Server_User_Name", item.user_name.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Server_User_Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessagePatient.message.ToString();
                        }
                        //SqlCetx.Commit();                    
                    }
                    catch (Exception Ex)
                    {
                        //SqlCetx.Rollback();
                        throw;
                    }
                }




                }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_EHRAndSystemCreds_Results(string JsonString, string TableName, string Location_id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    _successfullstataus = "Success";
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_ZohoInstall_Results(string JsonString, string TableName, string Location_id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                //JsonString = JsonString.Replace(",", ",\n");
                //JsonString = JsonString.Replace("{\"", "{\n\"");
                //JsonString = JsonString.Replace("\"}", "\"\n}");

                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    _successfullstataus = "Success";
                }

            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }
        #endregion
        //rooja 19-4-24
        public static string Push_Local_To_LiveDatabase_Insurance(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    var ResMessageInsurance = JsonConvert.DeserializeObject<Pull_InsuranceBO>(response.Content);
                    try
                    {
                        if (ResMessageInsurance.data != null)
                        {
                            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                                string SqlCeSelect = string.Empty;
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.CommandText = SynchLocalQRY.Update_Insurance_Web_Id;
                                    foreach (var item in ResMessageInsurance.data)
                                    {
                                        SqlCeCommand.Parameters.Clear();
                                        SqlCeCommand.Parameters.AddWithValue("EHR_ID", item.ehr_id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                                        SqlCeCommand.ExecuteNonQuery();
                                    }
                                }
                                _successfullstataus = "Success";
                            }
                        }
                        else
                        {
                            _successfullstataus = ResMessageInsurance.message.ToString();
                        }
                        //  SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //  SqlCetx.Rollback();
                        throw;
                    }
                    // var ResMessageHoliday = JsonConvert.DeserializeObject<Responce_HolidayBO>(response.Content);
                    _successfullstataus = "Success";
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }

            return _successfullstataus;
        }

        public static string Push_Local_To_LiveDatabase_LastSyncTime(string JsonString, string TableName, string Service_Install_Id, string Location_Id)
        {
            string _successfullstataus = string.Empty;

            try
            {
                string RestURL = LiveDatabaseAPI.LiveRecord_WithList_Push_API(TableName);

                var request = new RestRequest(Method.POST);
                var client = new RestClient(RestURL);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("Authorization", Utility.WebAdminUserToken);
                request.AddHeader("tokenkey", Utility.GenerateAuthonticationKey(Location_Id));
                request.Timeout = 900000;
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorMessage != null)
                {
                    _successfullstataus = response.ErrorMessage;
                }
                else
                {
                    try
                    {
                        var ResMessageLast_SyncTime = JsonConvert.DeserializeObject<Last_SyncTimeList>(response.Content);
                        //if (ResMessageAppointment. != null)
                        //{
                        //    using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                        //    {
                        //        if (conn.State == ConnectionState.Closed) conn.Open();
                        //        DateTime dtCurrentDtTime = Utility.Datetimesetting();
                        //        string SqlCeSelect = string.Empty;

                        if (ResMessageLast_SyncTime.message.ToLower() == "Success!".ToLower())
                        {
                            _successfullstataus = "Success";
                        }
                        else
                        {
                            _successfullstataus = ResMessageLast_SyncTime.message.ToString();
                        }
                        //        //using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                        //        //{
                        //        //    SqlCeCommand.CommandType = CommandType.Text;
                        //        //    SqlCeCommand.CommandText = SynchLocalQRY.Update_Is_Appt_DoubleBook;
                        //        //    foreach (var item in ResMessageAppointment.data)
                        //        //    {
                        //        //        SqlCeCommand.Parameters.Clear();
                        //        //        SqlCeCommand.Parameters.AddWithValue("Web_ID", item._id.ToString());
                        //        //        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        //        //        SqlCeCommand.ExecuteNonQuery();
                        //        //    }
                        //        //    _successfullstataus = "Success";
                        //        //}
                        //    }
                        //}
                        //else
                        //{
                        //    _successfullstataus = ResMessageAppointment.message.ToString();
                        //}
                        //SqlCetx.Commit();
                    }
                    catch (Exception)
                    {
                        //SqlCetx.Rollback();
                        throw;
                    }


                }
            }
            catch (Exception ex)
            {
                _successfullstataus = ex.Message;
                throw ex;
            }
            return _successfullstataus;
        }

    }
}
