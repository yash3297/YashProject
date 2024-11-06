using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pozative.DAL;
using System.Data;
using Pozative.UTL;

namespace Pozative.BAL
{
    public class SystemBAL
    {

        #region EHR Connection

        public static bool GetEHRClearDentConnection()
        {
            return SynchClearDentDAL.GetClearDentConnection();
        }

        public static bool GetEHRDentrixConnection()
        {
            return SynchDentrixDAL.GetDentrixConnection();
        }

        public static bool GetPracticeWorkConnection()
        {
            return SynchPracticeWorkDAL.GetPracticeWorkConnection();
        }

        public static bool GetAbelDentConnection()
        {
            return SynchAbelDentDAL.GetAbelDentConnection();
        }

        public static bool GetEHROpenDentalConnection(string DbString)
        {
            return SynchOpenDentalDAL.GetOpenDentalConnection(DbString);
        }
        public static bool GetEHRPracticeWebConnection(string DbString)
        {
            return SynchPracticeWebDAL.GetPracticeWebConnection(DbString);
        }
        public static bool GetEHREagleSoftConnection(string DbString)
        {
            return SynchEaglesoftDAL.GetEaglesoftConnection(DbString);
        }
        public static bool GetEHRTrackerConnection()
        {
            return SynchTrackerDAL.GetTrackerConnection();
        }

        public static DataTable GetColumns(string tablename)
        {
            try
            {
                return SystemDAL.GetColumns(tablename);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Organization

        public static string GetAdminUserLoginEmailIdPass()
        {
            //string AdminUserId, string AdminPassword
            return SystemDAL.GetAdminUserLogin();
        }


        public static string GetAdminUserLogin(string AdminUserId, string AdminPassword)
        {
            return SystemDAL.GetAdminUserLogin();
        }


        public static DataTable GetOrganizationDetail()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetOrganizationDetail_SqlServer();
            }
            else
            {
                return SystemDAL.GetOrganizationDetail();
            }
        }

        public static bool Save_OrganizationDetail(string Organization_ID, string Name, string phone, string email, string address,
                                                      string currency, string info, string is_active, string owner, string Adit_User_Email_ID, string Adit_User_Email_Password, string IsAction)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.Save_OrganizationDetail_SqlServer(Organization_ID, Name, phone, email, address, currency, info, is_active, owner, Adit_User_Email_ID, Adit_User_Email_Password, IsAction);
            }
            else
            {
                return SystemDAL.Save_OrganizationDetail(Organization_ID, Name, phone, email, address, currency, info, is_active, owner, Adit_User_Email_ID, Adit_User_Email_Password, IsAction);
            }
        }

        public static bool LocalDatabaseUpdateQuery(List<String> AlterDBquery)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.LocalDatabaseUpdateQuery_SqlServer(AlterDBquery);
            }
            else
            {
                return SystemDAL.LocalDatabaseUpdateQuery(AlterDBquery);
            }
        }

        public static string GetApiAditLocationAndOrganizationByAdminIdPassword()
        {
            return SystemDAL.GetApiAditLocationAndOrganizationByAdminIdPassword();
        }


        #endregion

        #region Location

        public static DataTable GetPozativeAppointment()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetPozativeAppointment_SqlServer();
            }
            else
            {
                return SystemDAL.GetPozativeAppointment();
            }
        }

        public static DataTable GetLocalAppointment()
        {
            return SystemDAL.GetLocalAppointment();
        }


        public static string UpdateWebTimeZone()
        {
            return SystemDAL.UpdateWebTimeZone();
        }

        public static string UpdateApplicationVersionOnLiveDatabase()
        {
            return SystemDAL.UpdateApplicationVersionOnLiveDatabase();
        }


        public static DataTable GetLocationDetail()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetLocationDetail_SqlServer();
            }
            else
            {
                return SystemDAL.GetLocationDetail();
            }
        }

        public static DataTable GetInstallServiceDetail()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetInstallServeiceDetail_SqlServer();
            }
            else
            {
                return SystemDAL.GetInstallServeiceDetail();
            }
        }

        public static bool Save_LocationDetail(string Location_ID, string name, string google_address, string phone, string email, string address,
                                               string website_url, string language, string owner, string location_numbers,
                                                string Organization_ID, string User_ID, string Loc_ID, string IsAction, string Clinic_Number, string Service_Install_Id)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.Save_LocationDetail_SqlServer(Location_ID, name, google_address, phone, email, address, website_url, language, owner,
                                                      location_numbers, Organization_ID, User_ID, Loc_ID, IsAction, Clinic_Number, Service_Install_Id);
            }
            else
            {
                return SystemDAL.Save_LocationDetail(Location_ID, name, google_address, phone, email, address, website_url, language, owner,
                                                         location_numbers, Organization_ID, User_ID, Loc_ID, IsAction, Clinic_Number, Service_Install_Id);
            }
        }

        public static DataTable CheckLocationIsExitsInLiveDB(string EmailId, string locationid)
        {
            return SystemDAL.CheckLocationIsExitsInLiveDB(EmailId, locationid);
        }

        public static DataTable GetPozativeEmailandLocationID()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetPozativeEmailandLocationID_SqlServer();
            }
            else
            {
                return SystemDAL.GetPozativeEmailandLocationID();
            }
        }

        public static bool RemoveSyncTableLastSyncLog(string ActionTable)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.RemoveSyncTableLastSyncLog_SqlServer(ActionTable);
            }
            else
            {
                return SystemDAL.RemoveSyncTableLastSyncLog(ActionTable);
            }
        }

        public static DataTable GetLastSyncTablesDatetime()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetLastSyncTablesDatetime_SqlServer();
            }
            else
            {
                return SystemDAL.GetLastSyncTablesDatetime();
            }
        }

        public static string GetApiPozativeLocation()
        {
            return SystemDAL.GetApiPozativeLocation();
        }


        public static string UpdatePozativeLocationMachineId(string LocationId, string MachineId)
        {
            return SystemDAL.UpdatePozativeLocationMachineId(LocationId, MachineId);
        }



        #endregion

        #region Service_Installation

        public static string GetApiERHListWithWebId()
        {
            return SystemDAL.GetApiERHListWithWebId();
        }

        public static string GetLocUpdateVersion()
        {
            return SystemDAL.GetLocUpdateVersion();
        }


        public static DataTable GetInstallApplicationDetail_SqlServer(string processorID)
        {

            return SystemDAL.GetInstallApplicationDetail_SqlServer(processorID);

        }

        public static bool CheckPozativeDatabaseWithSqlServerName(string SqlServerName, ref string message)
        {
            return SystemDAL.CheckPozativeDatabaseWithSqlServerName(SqlServerName, ref message);
        }

        public static bool CheckSqlServerDatabaseConnection(string SqlServerName, string applicationPath, ref string message)
        {
            return SystemDAL.CheckSqlServerDatabaseConnection(SqlServerName, applicationPath, ref message);
        }

        public static DataTable GetInstallApplicationDetail(string processorID)
        {

            return SystemDAL.GetInstallApplicationDetail(processorID);

        }

        public static bool UpdateEHRVersion()
        {
            return SystemDAL.UpdateEHRVersion();
        }

        public static DataTable GetAditActiveServerDetail()
        {

            return SystemDAL.GetAditActiveServerDetail();

        }

        public static bool RecoveryDatabase()
        {
           return  SystemDAL.RecoveryDatabase();
        }

        public static DataTable GetInstallApplicationLocationDetail()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetInstallApplicationLocationDetail_SqlServer();
            }
            else
            {
                return SystemDAL.GetInstallApplicationLocationDetail();
            }
        }

        public static bool Save_InstallApplicationDetail(string Organization_ID, string Location_ID, string Application_Name, string Application_Version,
                                                        string System_Name, string processorID, string EHRHostname, string EHRIntegrationKey, string EHRUserId,
                                                        string EHRPassword, string EHRDatabase, string EHRPort, string DBConnString, string WebAdminUserToken, string timezone,
                                                        bool AditSync, bool PozativeSync, string PozativeEmail, string PozativeLocationID, string PozativeLocationName, string IsAction, string Document_Path, string Installation_ID, bool DontAskPasswordOnSaveSetting ,
                    bool NotAllowToChangeSystemDateFormat,string SystemUser = "",string SystemPassword = "",string AditPMAUserName ="",string AditPMSUserID ="",string AditPMSUserPassword = "",bool IsClientAccessAllowed = false)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.Save_InstallApplicationDetail_SqlServer(Organization_ID, Location_ID, Application_Name, Application_Version, System_Name, processorID,
                                                               EHRHostname, EHRIntegrationKey, EHRUserId, EHRPassword, EHRDatabase, EHRPort, DBConnString,
                                                               WebAdminUserToken, timezone, AditSync, PozativeSync, PozativeEmail, PozativeLocationID, PozativeLocationName, 
                                                               IsAction, Installation_ID, DontAskPasswordOnSaveSetting, NotAllowToChangeSystemDateFormat, SystemUser, SystemPassword,
                                                               AditPMAUserName, AditPMSUserID, AditPMSUserPassword, IsClientAccessAllowed);
            }
            else
            {
                return SystemDAL.Save_InstallApplicationDetail(Organization_ID, Location_ID, Application_Name, Application_Version, System_Name, processorID,
                                                               EHRHostname, EHRIntegrationKey, EHRUserId, EHRPassword, EHRDatabase, EHRPort, DBConnString,
                                                               WebAdminUserToken, timezone, AditSync, PozativeSync, PozativeEmail, PozativeLocationID, PozativeLocationName, 
                                                               IsAction, Document_Path, Installation_ID, DontAskPasswordOnSaveSetting,  NotAllowToChangeSystemDateFormat, SystemUser, SystemPassword,
                                                               AditPMAUserName, AditPMSUserID, AditPMSUserPassword, IsClientAccessAllowed);
            }
        }


        public static bool UpdateEHR_version(string EHR_VersionNumber)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.UpdateEHR_version_SqlServer(  EHR_VersionNumber);
            }
            else
            {
                return SystemDAL.UpdateEHR_version(  EHR_VersionNumber);
            }
        }

        public static bool UpdateAditSync_InstallApplicationDetail(string Organization_ID, string Location_ID, string WebAdminUserToken, string timezone, string Installation_ID)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.UpdateAditSync_InstallApplicationDetail_SqlServer(Organization_ID, Location_ID, WebAdminUserToken, timezone, Installation_ID);
            }
            else
            {
                return SystemDAL.UpdateAditSync_InstallApplicationDetail(Organization_ID, Location_ID, WebAdminUserToken, timezone, Installation_ID);
            }
        }
        
        public static bool UpdateLocationId_InstallApplicationDetail(string Location_ID, string Installation_ID)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.UpdateLocationIdInstallApplicationDetail_SqlServer(Location_ID, Installation_ID);
            }
            else
            {
                return SystemDAL.UpdateLocationIdInstallApplicationDetail(Location_ID, Installation_ID);
            }
        }

        public static bool UpdateConfigSettingsInstallApplicationDetail(bool DontAskPasswordOnSaveSetting, bool NotAllowToChangeSystemDateFormat)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.UpdateConfigSettingsInstallApplicationDetail_SqlServer(DontAskPasswordOnSaveSetting, NotAllowToChangeSystemDateFormat);
            }
            else
            {
                return SystemDAL.UpdateConfigSettingsInstallApplicationDetail(DontAskPasswordOnSaveSetting, NotAllowToChangeSystemDateFormat);
            }
        }

        public static bool UpdatePozativeSyncService_Installation(bool AppSync, string AppName, string Installation_ID)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.UpdatePozativeSyncService_Installation_SqlServer(AppSync, AppName, Installation_ID);
            }
            else
            {
                return SystemDAL.UpdatePozativeSyncService_Installation(AppSync, AppName, Installation_ID);
            }
        }

        public static bool UpdateEHRConnectionString_Installation(string EHRConnectionString, string Installation_ID)
        {

            return SystemDAL.UpdateEHRConnectionString_Installation(EHRConnectionString, Installation_ID);

        }
        public static bool UpdateEHRDocPath_Installation(string EHRDocPath, string Service_Install_Id)
        {

            return SystemDAL.UpdateEHRDocPath_Installation(EHRDocPath, Service_Install_Id);

        }
        public static bool UpdateAditLocationSyncEnable(string Clinic_Number, string Service_Install_Id, string Location_ID, bool AditLocationSyncEnable)
        {

            return SystemDAL.UpdateAditLocationSyncEnable(Clinic_Number, Service_Install_Id, Location_ID, AditLocationSyncEnable);

        }
        public static bool UpdatePozativeSyncEmailLoc(string PozativeEmail, string PozativeLocationID, string PozativeLocationName, string Installation_ID)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.UpdatePozativeSyncEmailLoc_SqlServer(PozativeEmail, PozativeLocationID, PozativeLocationName, Installation_ID);
            }
            else
            {
                return SystemDAL.UpdatePozativeSyncEmailLoc(PozativeEmail, PozativeLocationID, PozativeLocationName, Installation_ID);
            }
        }

        public static bool UpdateLocationTimeZoneWithSystemTimeZone(string NewWebTimeZone, string Installation_ID)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.UpdateLocationTimeZoneWithSystemTimeZone_SqlServer(NewWebTimeZone, Installation_ID);
            }
            else
            {
                return SystemDAL.UpdateLocationTimeZoneWithSystemTimeZone(NewWebTimeZone, Installation_ID);
            }
        }

        public static bool UpdateEHRApplicationVersion_Installation(string Application_Version, string Installation_ID)
        {
            return SystemDAL.UpdateEHRApplicationVersion_Installation(Application_Version, Installation_ID);
        }
        #endregion

        #region Adit Configuration Sync

        public static string CheckLocationTimeZoneWithSystemTimeZone()
        {
            return SystemDAL.CheckLocationTimeZoneWithSystemTimeZone();
        }

        public static bool Update_AditApptAutoBook(bool ApptAutoBook, string Installation_ID)
        {
            return SystemDAL.Update_AditApptAutoBook(ApptAutoBook, Installation_ID);
        }

        public static string AditLocationSyncEnable(string Location_ID,string User_ID)
        {
            return SystemDAL.AditLocationSyncEnable(Location_ID,User_ID);
        }

        public static string AditPaymentSMSCallStatusUpdate(string Location_ID, string User_ID)
        {
            return SystemDAL.AditPaymentSMSCallStatusUpdate(Location_ID, User_ID);
        }

        public static DataTable GetAditModuleSyncTime()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetAditModuleSyncTime_SqlServer();
            }
            else
            {
                return SystemDAL.GetAditModuleSyncTime();
            }
        }


        public static DataTable GetService_Installation()
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.GetService_Installation_SqlServer();
            }
            else
            {
                return SystemDAL.GetService_Installation();
            }
        }
 
        public static bool Save_AditModuleSyncConfigTime(DataTable TempdtAditModuleSyncTime)
        {
            if (Utility.isSqlServer)
            {
                return SystemDAL.Save_AditModuleSyncConfigTime_SqlServer(TempdtAditModuleSyncTime);
            }
            else
            {
                return SystemDAL.Save_AditModuleSyncConfigTime(TempdtAditModuleSyncTime);
            }
        }

        public static bool Save_AditModuleSyncIdleConfigTime(bool ApplicationIdleTimeOff, DateTime AppIdleStartTime, DateTime AppIdleStopTime)
        {
            //if (Utility.isSqlServer)
            //{
            //    //return SystemDAL.Save_AditModuleSyncConfigTime_SqlServer(TempdtAditModuleSyncTime);
            //}
            //else
            //{
            return SystemDAL.Save_AditModuleSyncIdleConfigTime(ApplicationIdleTimeOff, AppIdleStartTime, AppIdleStopTime);
            // }
        }
        public static string SaveEHRLogs()
        {
            return SystemDAL.SaveEHRLogs();
        }
        public static string GetAutoPlayAudioText()
        {
            return SystemDAL.GetAutoPlayAudioText();
        }
        public static string GetMasterSync()
        {
            return SystemDAL.GetMasterSync();
        }
        public static string SendEmailEHR()
        {
            return SystemDAL.SendEmailEHR();
        }
        public static string IsValidOTP()
        {
            return SystemDAL.IsValidOTP();
        }
        #endregion

        #region Application Auto Update

        public static bool GetCurrentLocationAllowAppUpdate()
        {
            return SystemDAL.GetCurrentLocationAllowAppUpdate();
        }

        public static bool ApplicationUpdateServerDate()
        {
            return SystemDAL.ApplicationUpdateServerDate();
        }

        public static bool UpdateLocNewVersionOnPozative_Server_App(string Location_Server_App_Version)
        {
            return SystemDAL.UpdateLocNewVersionOnPozative_Server_App(Location_Server_App_Version);
        }

        public static bool UpdateSingleFieldInTable(string TableName,string FieldName,string UpdateValue,string WhereCondition = "")
        {

            return SystemDAL.UpdateSingleFieldInTable(TableName, FieldName, UpdateValue, WhereCondition);

        }
        #endregion

    }

}
