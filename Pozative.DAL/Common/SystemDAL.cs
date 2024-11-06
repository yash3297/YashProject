using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Pozative.BO;
using Pozative.QRY;
using System.Data.SqlServerCe;
using MySql.Data.MySqlClient;
using Pozative.UTL;
using System.Data.SqlClient;
using System.Data.Odbc;

namespace Pozative.DAL
{
    public class SystemDAL
    {

        public static DataTable GetLastSyncTablesDatetime()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetSyncTableDatetime;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static DataTable GetColumns(string tableName)
        {
            try
            {
                using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetColumnsNameFromTable;
                    SqlCeSelect = SqlCeSelect.Replace("tablename", tableName.ToString());
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetLastSyncTablesDatetime_SqlServer()
        {
            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetSyncTableDatetime;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool RemoveSyncTableLastSyncLog(string ActionTable)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        //if (ActionTable.ToLower() == "All".ToLower())
                        //{
                        //    SqlCeCommand.CommandText = SystemQRY.RemoveAllSyncTableLastSyncLog;
                        //}
                        //else if (ActionTable.ToLower() == "Adit".ToLower())
                        //{
                        //    SqlCeCommand.CommandText = SystemQRY.RemoveAditSyncTableLastSyncLog;
                        //}
                        //else if (ActionTable.ToLower() == "Pozative".ToLower())
                        //{
                        //    SqlCeCommand.CommandText = SystemQRY.RemovePozativeSyncTableLastSyncLog;
                        //}

                        switch (ActionTable.ToLower())
                        {
                            case "all":
                                SqlCeCommand.CommandText = SystemQRY.RemoveAllSyncTableLastSyncLog;
                                break;
                            case "adit":
                                SqlCeCommand.CommandText = SystemQRY.RemoveAditSyncTableLastSyncLog;
                                break;
                            case "pozative":
                                SqlCeCommand.CommandText = SystemQRY.RemovePozativeSyncTableLastSyncLog;
                                break;
                        }

                        SqlCeCommand.Parameters.AddWithValue("PozativeAppointment", "PozativeAppointment");
                        SqlCeCommand.Parameters.AddWithValue("PozativeAppointment_Push", "PozativeAppointment_Push");
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool RemoveSyncTableLastSyncLog_SqlServer(string ActionTable)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);


            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                switch (ActionTable.ToLower())
                {
                    case "all":
                        SqlCeCommand.CommandText = SystemQRY.RemoveAllSyncTableLastSyncLog;
                        break;
                    case "adit":
                        SqlCeCommand.CommandText = SystemQRY.RemoveAditSyncTableLastSyncLog;
                        break;
                    case "pozative":
                        SqlCeCommand.CommandText = SystemQRY.RemovePozativeSyncTableLastSyncLog;
                        break;
                }

                SqlCeCommand.Parameters.AddWithValue("PozativeAppointment", "PozativeAppointment");
                SqlCeCommand.Parameters.AddWithValue("PozativeAppointment_Push", "PozativeAppointment_Push");
                SqlCeCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        #region Adit

        public static string GetApiAditLocationAndOrganizationByAdminIdPassword()
        {
            string strApiLocOrg = LiveDatabaseAPI.GetAditLocationAndOrganization();
            return strApiLocOrg;
        }
        public static string GetApiERHListWithWebId()
        {
            string strApiListEHR = LiveDatabaseAPI.GetApiERHListWithWebId();
            return strApiListEHR;
        }
        public static string GetLocUpdateVersion()
        {
            string strApiListEHR = LiveDatabaseAPI.GetLocUpdateVersion();
            return strApiListEHR;
        }
        public static string GetAdminUserLogin()
        {
            string strAdminUserLogin = LiveDatabaseAPI.GetAdminUserLogin();
            return strAdminUserLogin;
        }
        public static string UpdateApplicationVersionOnLiveDatabase()
        {
            string strDesktopVersion = LiveDatabaseAPI.UpdateApplicationVersionOnLiveDatabase();
            return strDesktopVersion;
        }
        public static string CheckLocationTimeZoneWithSystemTimeZone()
        {
            string strLocationTimeZone = LiveDatabaseAPI.CheckLocationTimeZoneWithSystemTimeZone();
            return strLocationTimeZone;
        }
        public static string AditLocationSyncEnable(string Location_ID, string User_ID)
        {
            string strAditLocationSyncEnable = LiveDatabaseAPI.AditLocationSyncEnable(Location_ID, User_ID);
            return strAditLocationSyncEnable;
        }
        public static string AditPaymentSMSCallStatusUpdate(string Location_ID, string User_ID)
        {
            string strAditLocationSyncEnable = LiveDatabaseAPI.AditPaymentSMSCallStatusUpdate(Location_ID, User_ID);
            return strAditLocationSyncEnable;
        }
        #endregion

        #region Pozative
        public static string SaveEHRLogs()
        {
            string strEHRSLog = LiveDatabaseAPI.SaveEHRLogs();
            return strEHRSLog;
        }
        public static string GetAutoPlayAudioText()
        {
            string strAudioText = LiveDatabaseAPI.GetAutoPlayAudioText();
            return strAudioText;
        }
        public static string GetMasterSync()
        {
            string strGetMasterSync = LiveDatabaseAPI.GetMasterSync();
            return strGetMasterSync;
        }
        public static string SendEmailEHR()
        {
            string strEmail = LiveDatabaseAPI.SendEmailEHR();
            return strEmail;
        }
        public static string IsValidOTP()
        {
            string strOTP = LiveDatabaseAPI.IsValidOTP();
            return strOTP;
        }
        public static DataTable GetLocalAppointment()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                try
                {
                    // if (conn.State == ConnectionState.Closed) conn.Open(); 
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    DateTime ToDate = dtCurrentDtTime.AddDays(-1);
                    string SqlCeSelect = SystemQRY.GetLocalAppointment;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.Add("@ToDate", OdbcType.Date).Value = ToDate.ToString("yyyy/MM/dd");
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //   SqlCetx.Rollback();
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static string GetApiPozativeLocation()
        {
            string strApiLoc = LiveDatabaseAPI.GetPozativeLocation();
            return strApiLoc;
        }

        public static string UpdatePozativeLocationMachineId(string LocationId, string MachineId)
        {
            string strApiLoc = LiveDatabaseAPI.UpdatePozativeLocationMachineId(LocationId, MachineId);
            return strApiLoc;
        }

        public static string UpdateWebTimeZone()
        {
            string strApiLoc = LiveDatabaseAPI.UpdateWebTimeZone();
            return strApiLoc;
        }


        public static DataTable GetPozativeEmailandLocationID()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetPozativeEmailandLocationID;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static DataTable GetPozativeEmailandLocationID_SqlServer()
        {
            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetPozativeEmailandLocationID;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetPozativeAppointment()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetPozativeAppointment;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

        }

        public static DataTable GetPozativeAppointment_SqlServer()
        {
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            SqlDataAdapter SqlCeDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            SqlCeRemoteDataAccess rda = new SqlCeRemoteDataAccess();

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string SqlCeSelect = SystemQRY.GetPozativeAppointment;
                CommonDB.SqlServerCommand(SqlCeSelect, conn, ref SqlCeCommand, "txt");
                CommonDB.SqlServerDataAdapter(SqlCeCommand, ref SqlCeDa);
                DataTable SqlCeDt = new DataTable();
                SqlCeDa.Fill(SqlCeDt);
                return SqlCeDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }

        }

        public static DataTable CheckLocationIsExitsInLiveDB(string EmailId, string locationid)
        {

            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = null;
            MySqlDataAdapter MySqlDa = null;
            CommonDB.MySQLPozativeConnectionServer(ref conn);
            try
            {

                if (conn.State == ConnectionState.Closed) conn.Open();

                string MySqlSelect = SystemQRY.GetLocationIsExtis;
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                MySqlCommand.Parameters.AddWithValue("email", EmailId);
                MySqlCommand.Parameters.AddWithValue("LocId", locationid);
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
                return MySqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        #endregion

        #region Organization

        public static DataTable GetOrganizationDetail()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetOrganizationDetail;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static DataTable GetOrganizationDetail_SqlServer()
        {
            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetOrganizationDetail;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_OrganizationDetail(string Organization_ID, string Name, string phone, string email, string address,
                                                    string currency, string info, string is_active, string owner, string Adit_User_Email_ID, string Adit_User_Email_Password, string IsAction)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        if (IsAction == "Insert")
                        {
                            SqlCeCommand.CommandText = SystemQRY.DeleteOrganizationDetail;
                            SqlCeCommand.ExecuteNonQuery();
                            SqlCeCommand.CommandText = SystemQRY.InsertOrganizationDetail;
                        }
                        else
                        {
                            SqlCeCommand.CommandText = SystemQRY.UpdateOrganizationDetail;
                        }
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Organization_ID", Organization_ID);
                        SqlCeCommand.Parameters.AddWithValue("Name", Name);
                        SqlCeCommand.Parameters.AddWithValue("phone", phone);
                        SqlCeCommand.Parameters.AddWithValue("email", email);
                        SqlCeCommand.Parameters.AddWithValue("address", address);
                        SqlCeCommand.Parameters.AddWithValue("currency", currency);
                        SqlCeCommand.Parameters.AddWithValue("info", info);
                        SqlCeCommand.Parameters.AddWithValue("is_active", is_active);
                        SqlCeCommand.Parameters.AddWithValue("owner", owner);
                        SqlCeCommand.Parameters.AddWithValue("Adit_User_Email_ID", Adit_User_Email_ID);
                        SqlCeCommand.Parameters.AddWithValue("Adit_User_Email_Password", Adit_User_Email_Password);
                        SqlCeCommand.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool Save_OrganizationDetail_SqlServer(string Organization_ID, string Name, string phone, string email, string address,
                                                    string currency, string info, string is_active, string owner, string Adit_User_Email_ID, string Adit_User_Email_Password, string IsAction)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);


            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                if (IsAction == "Insert")
                {
                    SqlCeCommand.CommandText = SystemQRY.DeleteOrganizationDetail;
                    SqlCeCommand.ExecuteNonQuery();
                    SqlCeCommand.CommandText = SystemQRY.InsertOrganizationDetail;
                }
                else
                {
                    SqlCeCommand.CommandText = SystemQRY.UpdateOrganizationDetail;
                }

                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("Organization_ID", Organization_ID);
                SqlCeCommand.Parameters.AddWithValue("Name", Name);
                SqlCeCommand.Parameters.AddWithValue("phone", phone);
                SqlCeCommand.Parameters.AddWithValue("email", email);
                SqlCeCommand.Parameters.AddWithValue("address", address);
                SqlCeCommand.Parameters.AddWithValue("currency", currency);
                SqlCeCommand.Parameters.AddWithValue("info", info);
                SqlCeCommand.Parameters.AddWithValue("is_active", is_active);
                SqlCeCommand.Parameters.AddWithValue("owner", owner);
                SqlCeCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }


        public static bool LocalDatabaseUpdateQuery(List<String> AlterDBquery)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    foreach (var query in AlterDBquery)
                    {
                        if (query.ToString().Trim() != "")
                        {
                            try
                            {
                                using (SqlCeCommand SqlCeCommand = new SqlCeCommand(query.ToString().Trim(), conn))
                                {
                                    SqlCeCommand.CommandType = CommandType.Text;
                                    SqlCeCommand.ExecuteNonQuery();
                                }
                            }
                            catch (Exception)
                            { }
                        }
                    }
                }
                catch (Exception)
                { }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool LocalDatabaseUpdateQuery_SqlServer(List<String> AlterDBquery)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                foreach (var query in AlterDBquery)
                {
                    if (query.ToString().Trim() != "")
                    {
                        try
                        {
                            CommonDB.SqlServerCommand(query, conn, ref SqlCeCommand, "txt");
                            SqlCeCommand.ExecuteNonQuery();
                        }
                        catch (Exception)
                        { }
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        #endregion

        #region Location

        public static DataTable GetLocationDetail()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = "";

                    SqlCeSelect = SystemQRY.CheckColunm;
                    DataTable SqlCeDt = null;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                        }
                    }
                    try
                    {
                        if (SqlCeDt == null || (SqlCeDt != null && SqlCeDt.Rows.Count == 0))
                        {
                            SqlCeSelect = "ALTER TABLE [Location] ADD COLUMN [AditLocationSyncEnable] BIT DEFAULT 1";
                            using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                            {
                                SqlCeCommand.CommandType = CommandType.Text;
                                SqlCeCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                    SqlCeSelect = SystemQRY.GetLocationDetail;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static DataTable GetLocationDetail_SqlServer()
        {
            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetLocationDetail;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_LocationDetail(string Location_ID, string name, string google_address, string phone, string email, string address,
                                                string website_url, string language, string owner,
                                                 string location_numbers, string Organization_ID, string User_ID, string Loc_ID, string IsAction, string Clinic_Number, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        if (IsAction == "Insert")
                        {
                            SqlCeCommand.CommandText = SystemQRY.DeleteLocationDetail;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                            SqlCeCommand.ExecuteNonQuery();
                            SqlCeCommand.CommandText = SystemQRY.InsertLocationDetail;
                        }
                        else
                        {
                            SqlCeCommand.CommandText = SystemQRY.UpdateLocationDetail;
                        }
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                        SqlCeCommand.Parameters.AddWithValue("name", name);
                        SqlCeCommand.Parameters.AddWithValue("google_address", google_address);
                        SqlCeCommand.Parameters.AddWithValue("phone", phone);
                        SqlCeCommand.Parameters.AddWithValue("email", email);
                        SqlCeCommand.Parameters.AddWithValue("address", address);
                        SqlCeCommand.Parameters.AddWithValue("website_url", website_url);
                        SqlCeCommand.Parameters.AddWithValue("language", language);
                        SqlCeCommand.Parameters.AddWithValue("owner", owner);
                        SqlCeCommand.Parameters.AddWithValue("location_numbers", location_numbers);
                        SqlCeCommand.Parameters.AddWithValue("Organization_ID", Organization_ID);
                        SqlCeCommand.Parameters.AddWithValue("User_ID", User_ID);
                        SqlCeCommand.Parameters.AddWithValue("Loc_ID", Loc_ID);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool Save_LocationDetail_SqlServer(string Location_ID, string name, string google_address, string phone, string email, string address,
                                                string website_url, string language, string owner,
                                                 string location_numbers, string Organization_ID, string User_ID, string Loc_ID, string IsAction, string Clinic_Number, string Service_Install_Id)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);


            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                if (IsAction == "Insert")
                {
                    SqlCeCommand.CommandText = SystemQRY.DeleteLocationDetail;
                    SqlCeCommand.ExecuteNonQuery();
                    SqlCeCommand.CommandText = SystemQRY.InsertLocationDetail;
                }
                else
                {
                    SqlCeCommand.CommandText = SystemQRY.UpdateLocationDetail;
                }

                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                SqlCeCommand.Parameters.AddWithValue("name", name);
                SqlCeCommand.Parameters.AddWithValue("google_address", google_address);
                SqlCeCommand.Parameters.AddWithValue("phone", phone);
                SqlCeCommand.Parameters.AddWithValue("email", email);
                SqlCeCommand.Parameters.AddWithValue("address", address);
                SqlCeCommand.Parameters.AddWithValue("website_url", website_url);
                SqlCeCommand.Parameters.AddWithValue("language", language);
                SqlCeCommand.Parameters.AddWithValue("owner", owner);
                SqlCeCommand.Parameters.AddWithValue("location_numbers", location_numbers);
                SqlCeCommand.Parameters.AddWithValue("Organization_ID", Organization_ID);
                SqlCeCommand.Parameters.AddWithValue("User_ID", User_ID);
                SqlCeCommand.Parameters.AddWithValue("Loc_ID", Loc_ID);
                SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                SqlCeCommand.Parameters.AddWithValue("Service_Install_Id", Service_Install_Id);
                SqlCeCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }



        #endregion

        #region Service_Installation

        public static DataTable GetInstallServeiceDetail()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                DataTable SqlCeDt = null;
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetInstallServicesDetail;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                        }
                    }
                    foreach (DataRow dr in SqlCeDt.Rows)
                    {
                        if (Utility.Application_Version.ToLower() == "21.20".ToLower() && Utility.Application_Name == "Eaglesoft")
                        {
                            dr["DBConnString"] = Utility.DecryptString(dr["DBConnString"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
                return SqlCeDt;
            }
        }

        public static DataTable GetInstallServeiceDetail_SqlServer()
        {
            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetInstallServicesDetail;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool UpdateEHRVersion()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.UpdateEHRSubVersion;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("EHR_Sub_Version", Utility.EHR_Sub_Version);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return true;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        //hitika
        public static bool UpdateEHR_version_SqlServer( string EHR_VersionNumber)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                SqlCeCommand.CommandText = SystemQRY.UpdateEHR_VersionNumber;
                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("EHR_VersionNumber", EHR_VersionNumber);
                SqlCeCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool UpdateEHR_version(string EHR_VersionNumber)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateEHR_VersionNumber;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("EHR_VersionNumber", EHR_VersionNumber);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }


        public static DataTable GetInstallApplicationDetail(string processorID)
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetInstallApplicationDetail;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("System_processorID", processorID);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }


        public static bool RecoveryDatabase()
        {
            bool result = false;
            try
            {
                SqlCeEngine engine =
            // new SqlCeEngine("Data Source = " + txtDbPath.Text + ";Persist Security Info=False; Max Database Size=4000; Password =Smile");
            new SqlCeEngine(CommonUtility.LocalConnectionString());
                //Data Source=|DataDirectory|\Pozative.sdf;Persist Security Info=False; Max Database Size=4000; Password =Smile
                if (false == engine.Verify())
                {
                    Utility.WriteToSyncLogFile_All("Database File is Corrupted");
                    try
                    {
                        // engine.Repair(CommonUtility.LocalConnectionString(), RepairOption.RecoverCorruptedRows);
                        engine.Repair(null, RepairOption.DeleteCorruptedRows);
                        Utility.WriteToSyncLogFile_All("Database File is Recovered");
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        // Utility.WriteToSyncLogFile_All("Err_ Recover Db File " + ex.Message.ToString());
                    }
                }
                else
                {
                    // label1.Text = "Database is Not corrupted";
                }
            }
            catch (Exception ex)
            {
                Utility.WriteToSyncLogFile_All("Err_RecoveryDatabase " + ex.Message.ToString());
            }
            return result;
        }


        public static DataTable GetAditActiveServerDetail()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetAditActiveServer;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static bool CheckPozativeDatabaseWithSqlServerName(string SqlServerName, ref string message)
        {
            string ConString = "Data Source=" + SqlServerName.ToString() + ";Initial Catalog=Pozative;Integrated Security=True;";
            SqlConnection conn = new SqlConnection(ConString);
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = "Select * FROM Service_Installation";
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                message = message + "Check connection with Pozative database of table Service_Installation";
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static bool CheckSqlServerDatabaseConnection(string SqlServerName, string applicationPath, ref string message)
        {
            try
            {
                string ConString = "Data Source=" + SqlServerName.ToString() + ";Initial Catalog=master;Integrated Security=True;";
                SqlConnection conn = new SqlConnection(ConString);
                SqlCommand sqlCommand = null;
                SqlDataAdapter sqlDa = null;
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string sqlSelect = "Select * FROM sys.tables";
                    CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                    CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                    DataTable sqlDt = new DataTable();
                    sqlDa.Fill(sqlDt);
                    message = message + "Connection Done with sys.Table.. Send to create database";
                    CreatePozativeDatabase(ConString, applicationPath, ref message);
                    CreateTableSchema(SqlServerName, ref message);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public static void CreatePozativeDatabase(string connectionString, string applicationPath, ref string message)
        {
            try
            {

                String str;
                SqlConnection myConn = new SqlConnection(connectionString);

                str = "CREATE DATABASE Pozative ON PRIMARY " +
                    "(NAME = Pozative_Data, " +
                    "FILENAME = '" + applicationPath + "\\Pozative.mdf', " +
                    "SIZE = 3MB, MAXSIZE = 10MB, FILEGROWTH = 10%) " +
                    "LOG ON (NAME = Pozative_Log, " +
                    "FILENAME = '" + applicationPath + "\\Pozative.ldf', " +
                    "SIZE = 1MB, " +
                    "MAXSIZE = 5MB, " +
                    "FILEGROWTH = 10%)";

                SqlCommand myCommand = new SqlCommand(str, myConn);
                try
                {

                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    message = message + " Database Crated at Location " + str.ToString();
                }
                catch (System.Exception)
                {
                    throw;
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void CreateTableSchema(string sqlServerName, ref string message)
        {
            try
            {
                string ConString = "Data Source=" + sqlServerName.ToString() + ";Initial Catalog=Pozative;Integrated Security=True;";
                SqlConnection myConn = new SqlConnection(ConString);
                string sqlSelect = SystemQRY.databaseSchemaScript;
                SqlCommand myCommand = new SqlCommand(sqlSelect, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    message = message + " SchemaExecuted " + sqlSelect.ToString();
                }
                catch (System.Exception)
                {
                    throw;
                }
                finally
                {
                    if (myConn.State == ConnectionState.Open)
                    {
                        myConn.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetInstallApplicationDetail_SqlServer(string processorID)
        {
            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetInstallApplicationDetail;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                sqlCommand.Parameters.AddWithValue("System_processorID", processorID);
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static DataTable GetInstallApplicationLocationDetail()
        {
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetInstallApplicationLocationDetail;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static DataTable GetInstallApplicationLocationDetail_SqlServer()
        {
            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetInstallApplicationLocationDetail;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static bool Save_InstallApplicationDetail(string Organization_ID, string Location_ID, string Application_Name, string Application_Version, string System_Name, string processorID,
                                                         string EHRHostname, string EHRIntegrationKey, string EHRUserId, string EHRPassword, string EHRDatabase, string EHRPort, string DBConnString,
                                                         string WebAdminUserToken, string timezone, bool AditSync, bool PozativeSync,
                                                         string PozativeEmail, string PozativeLocationID, string PozativeLocationName, string IsAction, string Document_Path, string Installation_ID, bool DontAskPasswordOnSaveSetting, bool NotAllowToChangeSystemDateFormat, string SystemUser, string SystemPassword
                                                        , string AditPMAUserName = "", string AditPMSUserID = "", string AditPMSUserPassword = "", bool IsClientAccessAllowed = false)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        if (IsAction == "Insert")
                        {
                            SqlCeCommand.CommandText = SystemQRY.DeleteInstallApplicationDetail;
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                            SqlCeCommand.ExecuteNonQuery();
                            SqlCeCommand.CommandText = SystemQRY.InsertInstallApplicationDetail;
                        }
                        else
                        {
                            SqlCeCommand.CommandText = SystemQRY.UpdateInstallApplicationDetail;
                        }
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Organization_ID", Organization_ID);
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                        SqlCeCommand.Parameters.AddWithValue("Application_Name", Application_Name);
                        SqlCeCommand.Parameters.AddWithValue("Application_Version", Application_Version);
                        SqlCeCommand.Parameters.AddWithValue("System_Name", System_Name);
                        SqlCeCommand.Parameters.AddWithValue("System_processorID", processorID);
                        SqlCeCommand.Parameters.AddWithValue("Hostname", EHRHostname);
                        SqlCeCommand.Parameters.AddWithValue("IntegrationKey", EHRIntegrationKey);
                        SqlCeCommand.Parameters.AddWithValue("UserId", EHRUserId);
                        SqlCeCommand.Parameters.AddWithValue("Password", EHRPassword);
                        SqlCeCommand.Parameters.AddWithValue("Database", EHRDatabase);
                        SqlCeCommand.Parameters.AddWithValue("Port", EHRPort);
                        SqlCeCommand.Parameters.AddWithValue("DBConnString", DBConnString);
                        SqlCeCommand.Parameters.AddWithValue("WebAdminUserToken", WebAdminUserToken);
                        SqlCeCommand.Parameters.AddWithValue("timezone", timezone);
                        SqlCeCommand.Parameters.AddWithValue("AditSync", AditSync);
                        SqlCeCommand.Parameters.AddWithValue("PozativeSync", PozativeSync);
                        SqlCeCommand.Parameters.AddWithValue("PozativeEmail", PozativeEmail);
                        SqlCeCommand.Parameters.AddWithValue("PozativeLocationID", PozativeLocationID);
                        SqlCeCommand.Parameters.AddWithValue("PozativeLocationName", PozativeLocationName);
                        SqlCeCommand.Parameters.AddWithValue("ApplicationInstalledTime", Utility.Datetimesetting());
                        SqlCeCommand.Parameters.AddWithValue("Document_Path", Document_Path);
                        SqlCeCommand.Parameters.AddWithValue("DontAskPasswordOnSaveSetting", DontAskPasswordOnSaveSetting);
                        SqlCeCommand.Parameters.AddWithValue("NotAllowToChangeSystemDateFormat", NotAllowToChangeSystemDateFormat);
                        SqlCeCommand.Parameters.AddWithValue("AditUserEmailID", SystemUser);
                        SqlCeCommand.Parameters.AddWithValue("AditUserEmailPassword", SystemPassword);
                        SqlCeCommand.Parameters.AddWithValue("AditPMAUserName", AditPMAUserName);
                        SqlCeCommand.Parameters.AddWithValue("AditPMSUserID", AditPMSUserID);
                        SqlCeCommand.Parameters.AddWithValue("AditPMSUserPassword", AditPMSUserPassword);
                        SqlCeCommand.Parameters.AddWithValue("IsClientAccessAllowed", IsClientAccessAllowed);
                        SqlCeCommand.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool Save_InstallApplicationDetail_SqlServer(string Organization_ID, string Location_ID, string Application_Name, string Application_Version, string System_Name, string processorID,
                                                        string EHRHostname, string EHRIntegrationKey, string EHRUserId, string EHRPassword, string EHRDatabase, string EHRPort, string DBConnString,
                                                        string WebAdminUserToken, string timezone, bool AditSync, bool PozativeSync,
                                                        string PozativeEmail, string PozativeLocationID, string PozativeLocationName, string IsAction, string Installation_ID, bool DontAskPasswordOnSaveSetting, bool NotAllowToChangeSystemDateFormat, string SystemUser, string SystemPassword
                                                        , string AditPMAUserName = "", string AditPMSUserID = "", string AditPMSUserPassword = "", bool IsClientAccessAllowed = false)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);


            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                if (IsAction == "Insert")
                {
                    SqlCeCommand.CommandText = SystemQRY.DeleteInstallApplicationDetail;
                    SqlCeCommand.ExecuteNonQuery();
                    SqlCeCommand.CommandText = SystemQRY.InsertInstallApplicationDetail;
                }
                else
                {
                    SqlCeCommand.CommandText = SystemQRY.UpdateInstallApplicationDetail;
                }
                DateTime dtCurrentDtTime = Utility.Datetimesetting();

                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("Organization_ID", Organization_ID);
                SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                SqlCeCommand.Parameters.AddWithValue("Application_Name", Application_Name);
                SqlCeCommand.Parameters.AddWithValue("Application_Version", Application_Version);
                SqlCeCommand.Parameters.AddWithValue("System_Name", System_Name);
                SqlCeCommand.Parameters.AddWithValue("System_processorID", processorID);
                SqlCeCommand.Parameters.AddWithValue("Hostname", EHRHostname);
                SqlCeCommand.Parameters.AddWithValue("IntegrationKey", EHRIntegrationKey);
                SqlCeCommand.Parameters.AddWithValue("UserId", EHRUserId);
                SqlCeCommand.Parameters.AddWithValue("Password", EHRPassword);
                SqlCeCommand.Parameters.AddWithValue("Database", EHRDatabase);
                SqlCeCommand.Parameters.AddWithValue("Port", EHRPort);
                SqlCeCommand.Parameters.AddWithValue("WebAdminUserToken", WebAdminUserToken);
                SqlCeCommand.Parameters.AddWithValue("timezone", timezone);
                SqlCeCommand.Parameters.AddWithValue("AditSync", AditSync);
                SqlCeCommand.Parameters.AddWithValue("PozativeSync", PozativeSync);
                SqlCeCommand.Parameters.AddWithValue("PozativeEmail", PozativeEmail);
                SqlCeCommand.Parameters.AddWithValue("PozativeLocationID", PozativeLocationID);
                SqlCeCommand.Parameters.AddWithValue("PozativeLocationName", PozativeLocationName);
                SqlCeCommand.Parameters.AddWithValue("ApplicationInstalledTime", dtCurrentDtTime);
                SqlCeCommand.Parameters.AddWithValue("DontAskPasswordOnSaveSetting", DontAskPasswordOnSaveSetting);
                SqlCeCommand.Parameters.AddWithValue("NotAllowToChangeSystemDateFormat", NotAllowToChangeSystemDateFormat);
                SqlCeCommand.Parameters.AddWithValue("AditUserEmailID", SystemUser);
                SqlCeCommand.Parameters.AddWithValue("AditUserEmailPassword", SystemPassword);
                SqlCeCommand.Parameters.AddWithValue("AditPMAUserName", AditPMAUserName);
                SqlCeCommand.Parameters.AddWithValue("AditPMSUserID", AditPMSUserID);
                SqlCeCommand.Parameters.AddWithValue("AditPMSUserPassword", AditPMSUserPassword);
                SqlCeCommand.Parameters.AddWithValue("IsClientAccessAllowed", IsClientAccessAllowed);
                SqlCeCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }


        public static bool UpdateAditSync_InstallApplicationDetail(string Organization_ID, string Location_ID, string WebAdminUserToken, string timezone, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateAditSync_InstallApplicationDetail;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.Parameters.AddWithValue("Organization_ID", Organization_ID);
                        SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                        SqlCeCommand.Parameters.AddWithValue("WebAdminUserToken", WebAdminUserToken);
                        SqlCeCommand.Parameters.AddWithValue("timezone", timezone);
                        SqlCeCommand.Parameters.AddWithValue("AditSync", true);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdateAditSync_InstallApplicationDetail_SqlServer(string Organization_ID, string Location_ID, string WebAdminUserToken, string timezone, string Installation_ID)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                SqlCeCommand.CommandText = SystemQRY.UpdateAditSync_InstallApplicationDetail;
                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                SqlCeCommand.Parameters.AddWithValue("Organization_ID", Organization_ID);
                SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                SqlCeCommand.Parameters.AddWithValue("WebAdminUserToken", WebAdminUserToken);
                SqlCeCommand.Parameters.AddWithValue("timezone", timezone);
                SqlCeCommand.Parameters.AddWithValue("AditSync", true);
                SqlCeCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool UpdateLocationIdInstallApplicationDetail_SqlServer(string Location_ID, string Installation_ID)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                SqlCeCommand.CommandText = SystemQRY.UpdateService_Installation_LocationId;
                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                SqlCeCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool UpdateLocationIdInstallApplicationDetail(string Location_ID, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateService_Installation_LocationId;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdateConfigSettingsInstallApplicationDetail_SqlServer(bool DontAskPasswordOnSaveSetting, bool NotAllowToChangeSystemDateFormat)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                SqlCeCommand.CommandText = SystemQRY.UpdateService_Installation_ConfigSetting;
                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("DontAskPasswordOnSaveSetting", DontAskPasswordOnSaveSetting);
                SqlCeCommand.Parameters.AddWithValue("NotAllowToChangeSystemDateFormat", NotAllowToChangeSystemDateFormat);
                SqlCeCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool UpdateConfigSettingsInstallApplicationDetail(bool DontAskPasswordOnSaveSetting, bool NotAllowToChangeSystemDateFormat)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateService_Installation_ConfigSetting;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("DontAskPasswordOnSaveSetting", DontAskPasswordOnSaveSetting);
                        SqlCeCommand.Parameters.AddWithValue("NotAllowToChangeSystemDateFormat", NotAllowToChangeSystemDateFormat);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdatePozativeSyncService_Installation(bool AppSync, string AppName, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        if (AppName.ToLower() == "Adit".ToLower())
                        {
                            SqlCeCommand.CommandText = SystemQRY.UpdateAditSyncService_Installation;
                        }
                        else
                        {
                            SqlCeCommand.CommandText = SystemQRY.UpdatePozativeSyncService_Installation;
                        }

                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("AppSync", AppSync);
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdateEHRConnectionString_Installation(string EHRConnectionString, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateEHRConnectionString_Installation;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("DBConnString", EHRConnectionString);
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdateEHRDocPath_Installation(string EHRDocPath, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateEHRDocPath_Installation;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.Add("Installation_ID", Installation_ID);
                        SqlCeCommand.Parameters.AddWithValue("Document_Path", EHRDocPath);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdateAditLocationSyncEnable(string Clinic_Number, string Service_Install_Id, string Location_ID, bool AditLocationSyncEnable)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateAditLocationSyncEnable;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("AditLocationSyncEnable", AditLocationSyncEnable);
                        SqlCeCommand.Parameters.Add("Service_Install_Id", Service_Install_Id);
                        SqlCeCommand.Parameters.AddWithValue("Clinic_Number", Clinic_Number);
                        SqlCeCommand.Parameters.AddWithValue("Location_ID", Location_ID);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdatePozativeSyncService_Installation_SqlServer(bool AppSync, string AppName, string Installation_ID)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                if (AppName.ToLower() == "Adit".ToLower())
                {
                    SqlCeCommand.CommandText = SystemQRY.UpdateAditSyncService_Installation;
                }
                else
                {
                    SqlCeCommand.CommandText = SystemQRY.UpdatePozativeSyncService_Installation;
                }

                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("AppSync", AppSync);
                SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                SqlCeCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool UpdatePozativeSyncEmailLoc(string PozativeEmail, string PozativeLocationID, string PozativeLocationName, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdatePozativeSyncEmailLoc;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("PozativeSync", true);
                        SqlCeCommand.Parameters.AddWithValue("PozativeEmail", PozativeEmail);
                        SqlCeCommand.Parameters.AddWithValue("PozativeLocationID", PozativeLocationID);
                        SqlCeCommand.Parameters.AddWithValue("PozativeLocationName", PozativeLocationName);
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdatePozativeSyncEmailLoc_SqlServer(string PozativeEmail, string PozativeLocationID, string PozativeLocationName, string Installation_ID)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);


            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                SqlCeCommand.CommandText = SystemQRY.UpdatePozativeSyncEmailLoc;

                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("PozativeSync", true);
                SqlCeCommand.Parameters.AddWithValue("PozativeEmail", PozativeEmail);
                SqlCeCommand.Parameters.AddWithValue("PozativeLocationID", PozativeLocationID);
                SqlCeCommand.Parameters.AddWithValue("PozativeLocationName", PozativeLocationName);
                SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                SqlCeCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool Update_AditApptAutoBook(bool ApptAutoBook, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.Update_AditApptAutoBook;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("ApptAutoBook", ApptAutoBook);
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

        public static bool UpdateLocationTimeZoneWithSystemTimeZone(string NewWebTimeZone, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateLocationTimeZoneWithSystemTimeZone;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("timezone", NewWebTimeZone);
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;

        }


        public static bool UpdateLocationTimeZoneWithSystemTimeZone_SqlServer(string NewWebTimeZone, string Installation_ID)
        {

            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);


            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                SqlCeCommand.CommandText = SystemQRY.UpdateLocationTimeZoneWithSystemTimeZone;
                SqlCeCommand.Parameters.Clear();
                SqlCeCommand.Parameters.AddWithValue("timezone", NewWebTimeZone);
                SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                SqlCeCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;

        }

        //UpdateEHRApplicationVersion_Installation
        public static bool UpdateEHRApplicationVersion_Installation(string Application_Versoin, string Installation_ID)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateEHRApplication_Version_Installation;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("Application_Version", Application_Versoin);
                        SqlCeCommand.Parameters.AddWithValue("Installation_ID", Installation_ID);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        #endregion

        #region Adit Configuration Sync Time

        public static DataTable GetAditModuleSyncTime()
        {

            if (CheckTableExistsInDatabase("SyncModule"))
            {

            }

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetAditModuleSyncTime;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static DataTable GetService_Installation()
        {

            if (CheckTableExistsInDatabase("Service_Installation"))
            {

            }

            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.GetService_Installation;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                            return SqlCeDt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }
        public static DataTable GetAditModuleSyncTime_SqlServer()
        {

            if (CheckTableExistsInDatabase_SqlServer("SyncModule"))
            {

            }

            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetAditModuleSyncTime;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


        public static DataTable GetService_Installation_SqlServer()
        {

            if (CheckTableExistsInDatabase_SqlServer("Service_Installation"))
            {

            }

            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.GetService_Installation;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);
                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);
                return sqlDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool Save_AditModuleSyncConfigTime(DataTable TempdtAditModuleSyncTime)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;

                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;

                        SqlCeCommand.CommandText = SystemQRY.DeleteAditModuleSyncTimeDetail;
                        SqlCeCommand.ExecuteNonQuery();
                        SqlCeCommand.CommandText = SystemQRY.InsertAditModuleSyncTimeDetailDetail;
                        foreach (DataRow dr in TempdtAditModuleSyncTime.Rows)
                        {
                            SqlCeCommand.Parameters.Clear();
                            SqlCeCommand.Parameters.AddWithValue("SyncModule_Name", dr["SyncModule_Name"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("SyncModule_Pull", dr["SyncModule_Pull"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("SyncModule_Push", dr["SyncModule_Push"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("SyncModule_EHR", dr["SyncModule_EHR"].ToString().Trim());
                            SqlCeCommand.Parameters.AddWithValue("Last_Update_Date", dtCurrentDtTime);
                            SqlCeCommand.Parameters.AddWithValue("SyncDateTime", dr["SyncDateTime"].ToString().Trim());
                            SqlCeCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        public static bool Save_AditModuleSyncIdleConfigTime(bool ApplicationIdleTimeOff, DateTime AppIdleStartTime, DateTime AppIdleStopTime)
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    DateTime dtCurrentDtTime = Utility.Datetimesetting();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.UpdateAditModuleSyncIdleTimeDetail;
                        SqlCeCommand.Parameters.Clear();
                        SqlCeCommand.Parameters.AddWithValue("ApplicationIdleTimeOff", ApplicationIdleTimeOff);
                        SqlCeCommand.Parameters.AddWithValue("AppIdleStartTime", AppIdleStartTime);
                        SqlCeCommand.Parameters.AddWithValue("AppIdleStopTime", AppIdleStopTime);
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }
        public static bool Save_AditModuleSyncConfigTime_SqlServer(DataTable TempdtAditModuleSyncTime)
        {
            bool _successfullstataus = true;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);


            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DateTime dtCurrentDtTime = Utility.Datetimesetting();
                string sqlSelect = string.Empty;

                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");

                SqlCeCommand.CommandText = SystemQRY.DeleteAditModuleSyncTimeDetail;
                SqlCeCommand.ExecuteNonQuery();
                SqlCeCommand.CommandText = SystemQRY.InsertAditModuleSyncTimeDetailDetail;


                foreach (DataRow dr in TempdtAditModuleSyncTime.Rows)
                {
                    SqlCeCommand.Parameters.Clear();
                    SqlCeCommand.Parameters.AddWithValue("SyncModule_Name", dr["SyncModule_Name"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("SyncModule_Pull", dr["SyncModule_Pull"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("SyncModule_Push", dr["SyncModule_Push"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("SyncModule_EHR", dr["SyncModule_EHR"].ToString().Trim());
                    SqlCeCommand.Parameters.AddWithValue("Last_Update_Date", dtCurrentDtTime);
                    SqlCeCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _successfullstataus = false;
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return _successfullstataus;
        }

        public static bool CheckTableExistsInDatabase(string TableName)
        {
            bool isTableExists = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = SystemQRY.CheckTableExistsInDatabase;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.Parameters.AddWithValue("TABLE_NAME", TableName);
                        DataTable SqlCeDt = null;
                        using (SqlCeDataAdapter SqlCeDa = new SqlCeDataAdapter(SqlCeCommand))
                        {
                            SqlCeDt = new DataTable();
                            SqlCeDa.Fill(SqlCeDt);
                        }
                        if (SqlCeDt != null && SqlCeDt.Rows.Count > 0)
                        {
                            isTableExists = true;
                        }
                        else
                        {
                            CreateSyncModuleTableInDatabase();
                        }
                    }
                    return isTableExists;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static bool CheckTableExistsInDatabase_SqlServer(string TableName)
        {
            bool isTableExists = false;
            SqlConnection conn = null;
            SqlCommand sqlCommand = null;
            SqlDataAdapter sqlDa = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = SystemQRY.CheckTableExistsInDatabase;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref sqlCommand, "txt");
                sqlCommand.Parameters.AddWithValue("TABLE_NAME", TableName);
                CommonDB.SqlServerDataAdapter(sqlCommand, ref sqlDa);

                DataTable sqlDt = new DataTable();
                sqlDa.Fill(sqlDt);

                if (sqlDt != null && sqlDt.Rows.Count > 0)
                {
                    isTableExists = true;
                }
                else
                {
                    CreateSyncModuleTableInDatabase_SqlServer();
                }

                return isTableExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        public static bool CreateSyncModuleTableInDatabase()
        {
            bool isCreateTable = false;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = SystemQRY.CheckTableSyncModule;
                        SqlCeCommand.ExecuteNonQuery();
                        isCreateTable = true;
                        return isCreateTable;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public static bool CreateSyncModuleTableInDatabase_SqlServer()
        {
            bool isCreateTable = false;
            SqlConnection conn = null;
            SqlCommand SqlCeCommand = null;
            CommonDB.LocalConnectionServer_SqlServer(ref conn);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;
                CommonDB.SqlServerCommand(sqlSelect, conn, ref SqlCeCommand, "txt");
                SqlCeCommand.CommandText = SystemQRY.CheckTableSyncModule;
                SqlCeCommand.ExecuteNonQuery();
                isCreateTable = true;
                return isCreateTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }



        #endregion

        #region Application Auto Update

        public static bool GetCurrentLocationAllowAppUpdate()
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();
            MySqlDataAdapter MySqlDa = null;
            bool isupdateAppUpdate = false;
            CommonDB.MySQLAppUpdateConnectionServer(ref conn);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string MySqlSelect = " Select * from Client_Location_Update Where Location_Web_ID = '" + Utility.Loc_ID.ToString() + "'";
                CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                DataTable MySqlDt = new DataTable();
                MySqlDa.Fill(MySqlDt);
                if (MySqlDt.Rows.Count > 0)
                {
                    int isupdate = Convert.ToInt32(MySqlDt.Rows[0]["Is_Auto_Update"].ToString());
                    if (isupdate == 1)
                    {
                        isupdateAppUpdate = true;
                    }
                }
            }
            catch (Exception ex)
            {
                isupdateAppUpdate = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return isupdateAppUpdate;
        }

        public static bool ApplicationUpdateServerDate()
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            MySqlDataAdapter MySqlDa = null;
            bool isupdateAppUpdate = true;

            try
            {
                DataTable dtlocation = SystemDAL.GetLocationDetail();
                foreach (DataRow drRow in dtlocation.Rows)
                {
                    CommonDB.MySQLAppUpdateConnectionServer(ref conn);

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string MySqlSelect = " Select * from Client_Location_Update Where Location_Web_ID = '" + Utility.Loc_ID.ToString() + "'";
                    CommonDB.MySqlCommandServer(MySqlSelect, conn, ref MySqlCommand, "txt");
                    CommonDB.MySqlDatatAdapterServer(MySqlCommand, ref MySqlDa);
                    DataTable MySqlDt = new DataTable();
                    MySqlDa.Fill(MySqlDt);

                    string sqlSelect = string.Empty;
                    CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");
                    if (MySqlDt.Rows.Count > 0)
                    {
                        MySqlCommand.CommandText = SystemQRY.Update_ApplicationUpdateServerDate;
                    }
                    else
                    {
                        MySqlCommand.CommandText = SystemQRY.Insert_ApplicationUpdateServerDate;
                    }

                    MySqlCommand.Parameters.Clear();
                    MySqlCommand.Parameters.AddWithValue("Organization_Web_ID", Utility.Organization_ID.ToString());
                    MySqlCommand.Parameters.AddWithValue("Org_Name", Utility.Organization_Name.ToString());
                    MySqlCommand.Parameters.AddWithValue("User_Web_ID", Utility.User_ID.ToString());
                    MySqlCommand.Parameters.AddWithValue("Location_Web_Id", drRow["Loc_ID"].ToString());
                    MySqlCommand.Parameters.AddWithValue("Appointment_Location_Web_Id", drRow["Location_ID"].ToString());
                    MySqlCommand.Parameters.AddWithValue("Location_Name", drRow["name"].ToString());
                    MySqlCommand.Parameters.AddWithValue("Location_Server_App_Version", Utility.Server_App_Version.ToString());
                    MySqlCommand.Parameters.AddWithValue("Last_Update_Date", Convert.ToDateTime(DateTime.Now.ToString()));
                    MySqlCommand.Parameters.AddWithValue("EHR_Name", Utility.Application_Name.ToString());
                    MySqlCommand.Parameters.AddWithValue("EHR_Version", Utility.Application_Version.ToString());
                    MySqlCommand.Parameters.AddWithValue("System_Name", CommonUtility.SystemName);
                    MySqlCommand.Parameters.AddWithValue("Operating_System", CommonUtility.OperatingSystem);
                    MySqlCommand.Parameters.AddWithValue("Processor_Name", CommonUtility.ProcessorName);
                    MySqlCommand.Parameters.AddWithValue("Service_Pack", CommonUtility.ServicePack);
                    MySqlCommand.Parameters.AddWithValue("Total_RAM", CommonUtility.TRAM);
                    MySqlCommand.Parameters.AddWithValue("Available_RAM", CommonUtility.ARAM);
                    MySqlCommand.Parameters.AddWithValue("Total_HDisk", CommonUtility.THardDisk);
                    MySqlCommand.Parameters.AddWithValue("Available_HDisk", CommonUtility.AHardDisk);
                    MySqlCommand.Parameters.AddWithValue("DotNetFrameWork", CommonUtility.FrameWork);
                    MySqlCommand.Parameters.AddWithValue("System_Type", CommonUtility.SystemType);

                    MySqlCommand.ExecuteNonQuery();
                }

            }
            catch (Exception)
            {
                isupdateAppUpdate = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return isupdateAppUpdate;
        }

        public static bool UpdateLocNewVersionOnPozative_Server_App(string Location_Server_App_Version)
        {
            MySqlConnection conn = null;
            MySqlCommand MySqlCommand = new MySqlCommand();

            bool isupdateAppUpdate = true;
            CommonDB.MySQLAppUpdateConnectionServer(ref conn);
            try
            {
                // MySqlCommand.CommandTimeout = 120;
                if (conn.State == ConnectionState.Closed) conn.Open();
                string sqlSelect = string.Empty;
                CommonDB.MySqlCommandServer(sqlSelect, conn, ref MySqlCommand, "txt");

                MySqlCommand.CommandText = SystemQRY.Update_AppVersionOnPozative_Server_App;


                MySqlCommand.Parameters.Clear();

                MySqlCommand.Parameters.AddWithValue("Location_Web_Id", Utility.Loc_ID.ToString());
                MySqlCommand.Parameters.AddWithValue("Location_Server_App_Version", Location_Server_App_Version.ToString());
                MySqlCommand.Parameters.AddWithValue("Is_Auto_Update", false);
                MySqlCommand.Parameters.AddWithValue("Last_Update_Date", Convert.ToDateTime(DateTime.Now.ToString()));
                MySqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                isupdateAppUpdate = false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
            return isupdateAppUpdate;
        }

        #endregion

        public static bool UpdateSingleFieldInTable(string TableName, string FieldName, string UpdateValue, string WhereCondition = "")
        {
            bool _successfullstataus = true;
            using (SqlCeConnection conn = new SqlCeConnection(CommonUtility.LocalConnectionString()))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string SqlCeSelect = string.Empty;
                    string Query = SystemQRY.UpdateSingleFieldInTable;
                    Query = Query.Replace("TableName", TableName );
                    Query = Query.Replace("FieldName", FieldName );
                    Query = Query.Replace("@UpdateValue", "'" + UpdateValue + "'");
                    if (WhereCondition != "")
                    {
                        Query = Query.Replace("WhereCondition", WhereCondition);
                    }
                    else
                    {
                        Query = Query.Replace("WhereCondition", "");
                    }
                    using (SqlCeCommand SqlCeCommand = new SqlCeCommand(SqlCeSelect, conn))
                    {
                        SqlCeCommand.CommandType = CommandType.Text;
                        SqlCeCommand.CommandText = Query;
                        SqlCeCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    _successfullstataus = false;
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _successfullstataus;
        }

    }
}
