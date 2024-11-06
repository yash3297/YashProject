using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pozative.QRY
{
    public class SystemQRY
    {

        #region Sync Table Log

        public static string GetSyncTableDatetime = "SELECT * FROM Sync_Table WITH(NOLOCK)";

        public static string RemoveAllSyncTableLastSyncLog = "DELETE FROM Sync_Table;";

        public static string RemoveAditSyncTableLastSyncLog = "DELETE FROM Sync_Table Where [Sync_Table_Name] != @PozativeAppointment AND [Sync_Table_Name] != @PozativeAppointment_Push;";

        public static string RemovePozativeSyncTableLastSyncLog = "DELETE FROM Sync_Table Where [Sync_Table_Name] = @PozativeAppointment OR [Sync_Table_Name] = @PozativeAppointment_Push;";

        #endregion

        #region Pozative

        public static string GetPozativeEmailandLocationID = "SELECT PozativeEmail, PozativeLocationID FROM Service_Installation WITH(NOLOCK) ";

        public static string GetPozativeAppointment = " SELECT TOP 50 Appt.Appt_LocalDB_ID, Appt.appointment_id AS Appt_EHR_ID, Appt.op_id AS Operatory_Name, "
                                                    + " Appt.patient_name AS Patient_Name, Appt.patient_phone AS Mobile_Contact, Appt.email AS Email, "
                                                    + " Appt.appointment_date AS Appt_DateTime, 'Confirmed' AS Status "
                                                    + " FROM Pozative_Patient Appt WITH(NOLOCK) ORDER BY Appt.appointment_date Desc";

        public static string GetLocalAppointment = " SELECT Appt.Appt_LocalDB_ID, Appt.Appt_EHR_ID,Appt.Operatory_Name,Appt.Provider_Name, "
                                          + " RTRIM(Appt.Last_Name) + ' ' + RTRIM(Appt.First_Name) + ' ' + RTRIM(Appt.MI) AS Patient_Name, "
                                          + " Appt.Last_Name,Appt.First_Name,Appt.MI,Appt.Home_Contact,Appt.Mobile_Contact,Appt.Email,Appt.Address, "
                                          + " Appt.City,Appt.ST,Appt.Zip,Appt.ApptType,Appt.Appt_DateTime, "
                                          + " Appt.Status  AS Status, Appt.Appointment_Status  AS Appointment_Status, "
                                          + " Appt.Remind_DateTime,Appt.Entry_DateTime "
                                          + " FROM Appointment Appt WITH(NOLOCK) "
                                          + " WHERE is_deleted = 0 AND ( ( Appt.Appt_DateTime > ? AND Appt_DateTime <  dateadd(month,1,  GetDATE()) ) OR Appt.Appt_EHR_ID = '0') ORDER BY Appt.Appt_DateTime asc";

        #endregion

        #region Organization

        public static string GetOrganizationDetail = "SELECT * FROM Organization WITH(NOLOCK)";   // WHERE System_processorID=@System_processorID";

        public static string InsertOrganizationDetail = " INSERT INTO Organization (Organization_ID, Name, phone, email, address, currency, info, is_active, owner,Adit_User_Email_ID,Adit_User_Email_Password) "
                                                  + " VALUES (@Organization_ID, @Name, @phone, @email, @address, @currency, @info, @is_active, @owner,@Adit_User_Email_ID,@Adit_User_Email_Password) ";

        public static string UpdateOrganizationDetail = " UPDATE Organization SET Name =@Name, phone = @phone, email =@email, address =@address, "
                                                  + " currency =@currency, info =@info, is_active =@is_active, owner =@owner ,Adit_User_Email_ID = @Adit_User_Email_ID , Adit_User_Email_Password = @Adit_User_Email_Password Where Organization_ID = @Organization_ID ";

        public static string DeleteOrganizationDetail = " DELETE FROM Organization Where Organization_ID = Organization_ID";

        #endregion

        #region Location

        public static string CheckColunm = "select * from information_schema.columns where table_name = 'Location' and column_name = 'AditLocationSyncEnable'";

        public static string GetLocationDetail = "SELECT * FROM Location WITH(NOLOCK) where AditSync = 1";   // WHERE System_processorID=@System_processorID";

        public static string InsertLocationDetail = " INSERT INTO Location (Location_ID, name, google_address, phone, email, address,  website_url, language, "
                                                  + " owner, location_numbers, Organization_ID, User_ID, Loc_ID,Clinic_Number,Service_Install_Id) "
                                                  + " VALUES (@Location_ID, @name, @google_address, @phone, @email, @address, @website_url, @language, "
                                                  + " @owner, @location_numbers, @Organization_ID, @User_ID, @Loc_ID,@Clinic_Number,@Service_Install_Id) ";

        public static string UpdateLocationDetail = " UPDATE Location SET name = @name, google_address = @google_address, phone = @phone, email =@email, address = @address, "
                                                  + " website_url = @website_url, language = @language, owner = @owner, User_ID = @User_ID, Loc_ID = @Loc_ID, "
                                                  + " location_numbers = @location_numbers, Organization_ID = @Organization_ID ,Clinic_Number = @Clinic_Number ,Service_Install_Id = @Service_Install_Id  Where Location_ID = @Location_ID";

        public static string DeleteLocationDetail = " DELETE FROM Location Where Location_ID = @Location_ID";

        public static string GetLocationIsExtis = " SELECT loc.id,loc.loactionname FROM pozative_live.locations loc "
                                        + " JOIN pozative_live.users usr ON loc.user_id = usr.id "
                                        + " WHERE usr.email = @email and loc.id = @LocId; ";

        #endregion

        #region Service_Installation
        public static string databaseSchemaScript = "CREATE TABLE [Appointment]                                           "
                                                      + "  (                                                              "
                                                      + "     [Appt_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),           "
                                                      + "     [Appt_EHR_ID] NVARCHAR(10),                                 "
                                                      + "     [Appt_Web_ID] NVARCHAR(250),                                "
                                                      + "     [Last_Name] NVARCHAR(50),                                   "
                                                      + "     [First_Name] NVARCHAR(50),                                  "
                                                      + "     [MI] NVARCHAR(50),                                          "
                                                      + "     [Home_Contact] NVARCHAR(50),                                "
                                                      + "     [Mobile_Contact] NVARCHAR(50),                              "
                                                      + "     [Email] NVARCHAR(50),                                       "
                                                      + "     [Address] NVARCHAR(200),                                    "
                                                      + "     [City] NVARCHAR(50),                                        "
                                                      + "     [ST] NVARCHAR(50),                                          "
                                                      + "     [Zip] NVARCHAR(50),                                         "
                                                      + "     [Operatory_EHR_ID] NVARCHAR(100),                           "
                                                      + "     [Operatory_Name] NVARCHAR(100),                             "
                                                      + "     [Provider_EHR_ID] NVARCHAR(100),                            "
                                                      + "     [Provider_Name] NVARCHAR(500),                              "
                                                      + "     [comment] NVARCHAR(2000),                                   "
                                                      + "     [birth_date] NVARCHAR(100),                                 "
                                                      + "     [ApptType_EHR_ID] NVARCHAR(100),                            "
                                                      + "     [ApptType] NVARCHAR(100),                                   "
                                                      + "     [Appt_DateTime] DATETIME,                                   "
                                                      + "     [Appt_EndDateTime] DATETIME,                                "
                                                      + "     [Status] NVARCHAR(5),                                       "
                                                      + "     [Patient_Status] NVARCHAR(25),                              "
                                                      + "     [appointment_status_ehr_key] NVARCHAR(10),                  "
                                                      + "     [Appointment_Status] NVARCHAR(50),                          "
                                                      + "     [Is_Appt] NVARCHAR(10),                                     "
                                                      + "     [is_ehr_updated] BIT,                                       "
                                                      + "     [Remind_DateTime] DATETIME,                                 "
                                                      + "     [Entry_DateTime] DATETIME,                                  "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [EHR_Entry_DateTime] DATETIME,                              "
                                                      + "     [Is_Adit_Updated] BIT,                                      "
                                                      + "     [patient_ehr_id] NVARCHAR(10),                              "
                                                      + "     [unschedule_status_ehr_key] NVARCHAR(10),                   "
                                                      + "     [unschedule_status] NVARCHAR(50),                           "
                                                      + "     [confirmed_status_ehr_key] NVARCHAR(10),                    "
                                                      + "     [confirmed_status] NVARCHAR(50),                            "
                                                      + "     [is_deleted] BIT DEFAULT 0                                  "
                                                      
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Appointment_Status]                              "
                                                      + "  (                                                              "
                                                      + "     [ApptStatus_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),     "
                                                      + "     [ApptStatus_EHR_ID] NVARCHAR(10),                           "
                                                      + "     [ApptStatus_Web_ID] NVARCHAR(250),                          "
                                                      + "     [ApptStatus_Name] NVARCHAR(250),                            "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [EHR_Entry_DateTime] DATETIME,                              "
                                                      + "     [Is_Adit_Updated] BIT                                       "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Appointment_Type]                                "
                                                      + "  (                                                              "
                                                      + "     [ApptType_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),       "
                                                      + "     [ApptType_EHR_ID] NVARCHAR(10),                             "
                                                      + "     [ApptType_Web_ID] NVARCHAR(250),                            "
                                                      + "     [Type_Name] NVARCHAR(100),                                  "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [EHR_Entry_DateTime] DATETIME,                              "
                                                      + "     [Is_Adit_Updated] BIT                                       "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Location]                                        "
                                                      + "  (                                                              "
                                                      + "     [Location_ID] NVARCHAR(250),                                "
                                                      + "     [name] NVARCHAR(100),                                       "
                                                      + "     [google_address] NVARCHAR(200),                             "
                                                      + "     [phone] NVARCHAR(100),                                      "
                                                      + "     [email] NVARCHAR(100),                                      "
                                                      + "     [address] NVARCHAR(200),                                    "
                                                      + "     [website_url] NVARCHAR(1000),                               "
                                                      + "     [language] NVARCHAR(100),                                   "
                                                      + "     [owner] NVARCHAR(200),                                      "
                                                      + "     [location_numbers] NVARCHAR(100),                           "
                                                      + "     [Organization_ID] NVARCHAR(250),                            "
                                                      + "     [User_ID] NVARCHAR(250),                                    "
                                                      + "     [Loc_ID] NVARCHAR(250)                                      "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Operatory]                                       "
                                                      + "  (                                                              "
                                                      + "     [Operatory_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),      "
                                                      + "     [Operatory_EHR_ID] NVARCHAR(10),                            "
                                                      + "     [Operatory_Web_ID] NVARCHAR(250),                           "
                                                      + "     [Operatory_Name] NVARCHAR(50),                              "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [EHR_Entry_DateTime] DATETIME,                              "
                                                      + "     [Is_Adit_Updated] BIT                                       "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [OperatoryEvent]                                  "
                                                      + "  (                                                              "
                                                      + "     [OE_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),             "
                                                      + "     [OE_EHR_ID] NVARCHAR(10),                                   "
                                                      + "     [OE_Web_ID] NVARCHAR(250),                                  "
                                                      + "     [Operatory_EHR_ID] NVARCHAR(100),                           "
                                                      + "     [StartTime] DATETIME,                                       "
                                                      + "     [EndTime] DATETIME,                                         "
                                                      + "     [comment] NVARCHAR(2000),                                   "
                                                      + "     [Entry_DateTime] DATETIME,                                  "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [is_deleted] BIT DEFAULT 0,                                 "
                                                      + "     [Is_Adit_Updated] BIT DEFAULT 0                             "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Organization]                                    "
                                                      + "  (                                                              "
                                                      + "     [Organization_ID] NVARCHAR(250),                            "
                                                      + "     [Name] NVARCHAR(100),                                       "
                                                      + "     [phone] NVARCHAR(100),                                      "
                                                      + "     [email] NVARCHAR(100),                                      "
                                                      + "     [address] NVARCHAR(200),                                    "
                                                      + "     [currency] NVARCHAR(50),                                    "
                                                      + "     [info] NVARCHAR(200),                                       "
                                                      + "     [is_active] NVARCHAR(5),                                    "
                                                      + "     [owner] NVARCHAR(100)                                       "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Patient]                                         "
                                                      + "  (                                                              "
                                                      + "     [Patient_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),        "
                                                      + "     [Patient_EHR_ID] NVARCHAR(10),                              "
                                                      + "     [Patient_Web_ID] NVARCHAR(250),                             "
                                                      + "     [First_name] NVARCHAR(100),                                 "
                                                      + "     [Last_name] NVARCHAR(100),                                  "
                                                      + "     [Middle_Name] NVARCHAR(50),                                 "
                                                      + "     [Salutation] NVARCHAR(50),                                  "
                                                      + "     [Status] NVARCHAR(10),                                      "
                                                      + "     [Sex] NVARCHAR(10),                                         "
                                                      + "     [MaritalStatus] NVARCHAR(10),                               "
                                                      + "     [Birth_Date] NVARCHAR(100),                                 "
                                                      + "     [Email] NVARCHAR(100),                                      "
                                                      + "     [Mobile] NVARCHAR(100),                                     "
                                                      + "     [Home_Phone] NVARCHAR(100),                                 "
                                                      + "     [Work_Phone] NVARCHAR(100),                                 "
                                                      + "     [Address1] NVARCHAR(300),                                   "
                                                      + "     [Address2] NVARCHAR(300),                                   "
                                                      + "     [City] NVARCHAR(100),                                       "
                                                      + "     [State] NVARCHAR(50),                                       "
                                                      + "     [Zipcode] NVARCHAR(20),                                     "
                                                      + "     [ResponsibleParty_Status] NVARCHAR(10),                     "
                                                      + "     [CurrentBal] NVARCHAR(10),                                  "
                                                      + "     [ThirtyDay] NVARCHAR(10),                                   "
                                                      + "     [SixtyDay] NVARCHAR(10),                                    "
                                                      + "     [NinetyDay] NVARCHAR(10),                                   "
                                                      + "     [Over90] NVARCHAR(10),                                      "
                                                      + "     [FirstVisit_Date] NVARCHAR(100),                            "
                                                      + "     [LastVisit_Date] NVARCHAR(100),                             "
                                                      + "     [Primary_Insurance] NVARCHAR(100),                          "
                                                      + "     [Primary_Insurance_CompanyName] NVARCHAR(200),              "
                                                      + "     [Secondary_Insurance] NVARCHAR(100),                        "
                                                      + "     [Secondary_Insurance_CompanyName] NVARCHAR(200),            "
                                                      + "     [Guar_ID] NVARCHAR(20),                                     "
                                                      + "     [Pri_Provider_ID] NVARCHAR(20),                             "
                                                      + "     [Sec_Provider_ID] NVARCHAR(20),                             "
                                                      + "     [ReceiveSms] NVARCHAR(10),                                  "
                                                      + "     [ReceiveEmail] NVARCHAR(10),                                "
                                                      + "     [nextvisit_date] NVARCHAR(100),                             "
                                                      + "     [due_date] NVARCHAR(1000),                                  "
                                                      + "     [remaining_benefit] NVARCHAR(100),                          "
                                                      + "     [collect_payment] NVARCHAR(100),                            "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [EHR_Entry_DateTime] DATETIME,                              "
                                                      + "     [Is_Adit_Updated] BIT,                                      "
                                                      + "     [preferred_name] NVARCHAR(50) DEFAULT ' '                   "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Pozative_Patient]                                "
                                                      + "  (                                                              "
                                                      + "     [Appt_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),           "
                                                      + "     [appointment_id] NVARCHAR(255),                             "
                                                      + "     [Appt_Web_ID] NVARCHAR(255),                                "
                                                      + "     [patient_guid] NVARCHAR(255),                               "
                                                      + "     [patient_name] NVARCHAR(255),                               "
                                                      + "     [patient_phone] NVARCHAR(255),                              "
                                                      + "     [email_status] NVARCHAR(255),                               "
                                                      + "     [phone_status] NVARCHAR(255),                               "
                                                      + "     [appointment_date] NVARCHAR(255),                           "
                                                      + "     [modified_time_stamp] NVARCHAR(255),                        "
                                                      + "     [created_at] NVARCHAR(255),                                 "
                                                      + "     [queue_status] NVARCHAR(250),                               "
                                                      + "     [email] NVARCHAR(250),                                      "
                                                      + "     [op_id] NVARCHAR(250),                                      "
                                                      + "     [ReceivesSms] NVARCHAR(10),                                 "
                                                      + "     [Last_Sync_Date] DATETIME                                   "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Providers]                                       "
                                                      + "  (                                                              "
                                                      + "     [Provider_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),       "
                                                      + "     [Provider_EHR_ID] NVARCHAR(10),                             "
                                                      + "     [Provider_Web_ID] NVARCHAR(250),                            "
                                                      + "     [Last_Name] NVARCHAR(50),                                   "
                                                      + "     [First_Name] NVARCHAR(50),                                  "
                                                      + "     [MI] NVARCHAR(5),                                           "
                                                      + "     [gender] NVARCHAR(15),                                      "
                                                      + "     [provider_speciality] NVARCHAR(100),                        "
                                                      + "     [image] IMAGE,                                              "
                                                      + "     [bio] NVARCHAR(100),                                        "
                                                      + "     [education] NVARCHAR(200),                                  "
                                                      + "     [accreditation] NVARCHAR(200),                              "
                                                      + "     [membership] NVARCHAR(200),                                 "
                                                      + "     [language] NVARCHAR(200),                                   "
                                                      + "     [age_treated_min] NVARCHAR(100),                            "
                                                      + "     [age_treated_max] NVARCHAR(100),                            "
                                                      + "     [is_active] BIT,                                            "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [EHR_Entry_DateTime] DATETIME,                              "
                                                      + "     [Is_Adit_Updated] BIT                                       "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [RecallType]                                      "
                                                      + "  (                                                              "
                                                      + "     [RecallType_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),     "
                                                      + "     [RecallType_EHR_ID] NVARCHAR(10),                           "
                                                      + "     [RecallType_Web_ID] NVARCHAR(250),                          "
                                                      + "     [RecallType_Name] NVARCHAR(250),                            "
                                                      + "     [RecallType_Descript] NVARCHAR(500),                        "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [EHR_Entry_DateTime] DATETIME,                              "
                                                      + "     [Is_Adit_Updated] BIT                                       "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Service_Installation]                            "
                                                      + "  (                                                              "
                                                      + "     [Installation_ID] BIGINT,                                   "
                                                      + "     [Organization_ID] NVARCHAR(250),                            "
                                                      + "     [Location_ID] NVARCHAR(250),                                "
                                                      + "     [Application_Name] NVARCHAR(100),                           "
                                                      + "     [Application_Version] NVARCHAR(100),                        "
                                                      + "     [System_Name] NVARCHAR(200),                                "
                                                      + "     [System_processorID] NVARCHAR(100),                         "
                                                      + "     [Hostname] NVARCHAR(100),                                   "
                                                      + "     [Database] NVARCHAR(100),                                   "
                                                      + "     [IntegrationKey] NVARCHAR(100),                             "
                                                      + "     [UserId] NVARCHAR(100),                                     "
                                                      + "     [Password] NVARCHAR(100),                                   "
                                                      + "     [Port] NVARCHAR(10),                                        "
                                                      + "     [WebAdminUserToken] NVARCHAR(4000),                         "
                                                      + "     [timezone] NVARCHAR(100),                                   "
                                                      + "     [IS_Install] BIT,                                           "
                                                      + "     [Installation_Date] DATETIME,                               "
                                                      + "     [Installation_Modify_Date] DATETIME,                        "
                                                      + "     [AditSync] BIT,                                             "
                                                      + "     [PozativeSync] BIT,                                         "
                                                      + "     [ApptAutoBook] BIT DEFAULT 1,                               "
                                                      + "     [PozativeEmail] NVARCHAR(100),                              "
                                                      + "     [PozativeLocationID] NVARCHAR(100),                         "
                                                      + "     [PozativeLocationName] NVARCHAR(200)                        "
                                                      + "     [EHR_ActualVersion] NVARCHAR(100)                           "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Speciality]                                      "
                                                      + "  (                                                              "
                                                      + "     [Speciality_LocalDB_ID] BIGINT NOT NULL IDENTITY (1,1),     "
                                                      + "     [Speciality_EHR_ID] NVARCHAR(10),                           "
                                                      + "     [Speciality_Web_ID] NVARCHAR(250),                          "
                                                      + "     [Speciality_Name] NVARCHAR(100),                            "
                                                      + "     [Last_Sync_Date] DATETIME,                                  "
                                                      + "     [EHR_Entry_DateTime] DATETIME,                              "
                                                      + "     [Is_Adit_Updated] BIT                                       "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [Sync_Table]                                      "
                                                      + "  (                                                              "
                                                      + "     [Sync_Table_ID] BIGINT NOT NULL IDENTITY (1,1),             "
                                                      + "     [Sync_Table_Name] NVARCHAR(50),                             "
                                                      + "     [Last_Sync_Date] DATETIME                                   "
                                                      + "  );                                                             "
                                                      + "  CREATE TABLE [SyncModule]                                      "
                                                      + "  (                                                              "
                                                      + "     [SyncModule_ID] BIGINT NOT NULL IDENTITY (1,1),             "
                                                      + "     [SyncModule_Name] NVARCHAR(200),                            "
                                                      + "     [SyncModule_Pull] BIGINT,                                   "
                                                      + "     [SyncModule_Push] BIGINT,                                   "
                                                      + "     [SyncModule_EHR] BIGINT,                                    "
                                                      + "     [Last_Update_Date] DATETIME                                 "
                                                      + "  );";


        //public static string GetInstallApplicationDetail = " SELECT SI.*,o.Adit_User_Email_ID,o.Adit_User_Email_Password FROM Service_Installation SI WITH(NOLOCK) inner join Organization as o on SI.Organization_ID = o.Organization_ID" 
        //                                                 + " WHERE System_processorID=@System_processorID ";

        public static string GetInstallApplicationDetail = " SELECT SI.*,o.Name AS Organization_Name,Loc.Name AS Location_Name,o.Adit_User_Email_ID,o.Adit_User_Email_Password "
                                                     + " FROM Service_Installation SI WITH(NOLOCK) "
                                                     + " inner join Organization as o on SI.Organization_ID = o.Organization_ID "
                                                     + " inner join Location as Loc on SI.Location_ID = Loc.Location_ID "
                                                     + " WHERE System_processorID=@System_processorID Order by Installation_ID ";

        public static string UpdateEHRSubVersion = " UPDATE Service_Installation SET EHR_Sub_Version = @EHR_Sub_Version";

        public static string GetAditActiveServer = " SELECT * From Adit_HostServer Where Is_Active = 1 ";

        public static string GetInstallApplicationLocationDetail = " SELECT * FROM Location WITH(NOLOCK) ";

        public static string GetInstallServicesDetail = " SELECT * FROM Service_Installation WITH(NOLOCK) ";

        public static string InsertInstallApplicationDetail = " INSERT INTO Service_Installation "
                                      + " (Organization_ID,Installation_ID,Location_ID,Application_Name,Application_Version, System_Name, "
                                      + " System_processorID,Hostname,IntegrationKey,UserId,Password,[Database],Port, DBConnString, WebAdminUserToken, timezone, IS_Install, "
                                      + " AditSync, PozativeSync, PozativeEmail, PozativeLocationID, PozativeLocationName,ApplicationInstalledTime,Document_Path, DontAskPasswordOnSaveSetting, NotAllowToChangeSystemDateFormat,AditUserEmailID,AditUserEmailPassword," +
                                        " AditPMAUserName,AditPMSUserID,AditPMSUserPassword,IsClientAccessAllowed) "
                                      + " VALUES (@Organization_ID,@Installation_ID, @Location_ID, @Application_Name, @Application_Version, @System_Name, "
                                      + " @System_processorID, @Hostname, @IntegrationKey, @UserId, @Password, @Database, @Port, @DBConnString, @WebAdminUserToken, @timezone, 1, "
                                      + " @AditSync, @PozativeSync, @PozativeEmail, @PozativeLocationID, @PozativeLocationName, @ApplicationInstalledTime,@Document_Path, @DontAskPasswordOnSaveSetting, @NotAllowToChangeSystemDateFormat,@AditUserEmailID,@AditUserEmailPassword," +
                                        " @AditPMAUserName,@AditPMSUserID,@AditPMSUserPassword,@IsClientAccessAllowed) ";

        public static string UpdateInstallApplicationDetail = " UPDATE Service_Installation SET Hostname =@Hostname, IntegrationKey = @IntegrationKey, UserId = @UserId, "
                                                            + " Password = @Password,[Datebase] =@Database, Port = @Port, DBConnString = @DBConnString, "
                                                            + " timezone = @timezone,  IS_Install = 1, ApplicationInstalledTime =@ApplicationInstalledTime , Document_Path =@Document_Path, DontAskPasswordOnSaveSetting = @DontAskPasswordOnSaveSetting, NotAllowToChangeSystemDateFormat = @NotAllowToChangeSystemDateFormat  "
                                                            + " WHERE Installation_ID = @Installation_ID";

        public static string UpdateEHR_VersionNumber = " UPDATE Service_Installation SET EHR_VersionNumber= @EHR_VersionNumber ";

        public static string UpdateAditSync_InstallApplicationDetail = " UPDATE Service_Installation SET Organization_ID = @Organization_ID, Location_ID = @Location_ID, "
                                                                     + " WebAdminUserToken = @WebAdminUserToken, timezone = @timezone, AditSync =@AditSync where Installation_ID = @Installation_ID";

        public static string UpdateService_Installation_LocationId = " UPDATE Service_Installation SET Location_ID = @Location_ID where Installation_ID = @Installation_ID ";

        public static string UpdateService_Installation_ConfigSetting = " UPDATE Service_Installation SET DontAskPasswordOnSaveSetting = @DontAskPasswordOnSaveSetting, NotAllowToChangeSystemDateFormat = @NotAllowToChangeSystemDateFormat";

        public static string DeleteInstallApplicationDetail = " DELETE FROM Service_Installation WHERE Installation_ID = @Installation_ID";

        public static string UpdatePozativeSyncService_Installation = " UPDATE Service_Installation SET PozativeSync = @AppSync where Installation_ID = @Installation_ID ";

        public static string UpdateAditSyncService_Installation = " UPDATE Service_Installation SET AditSync = @AppSync where Installation_ID = @Installation_ID ";

        public static string UpdateEHRConnectionString_Installation = " UPDATE Service_Installation SET DBConnString = @DBConnString where Installation_ID = @Installation_ID ";
        
        public static string UpdateEHRDocPath_Installation = " UPDATE Service_Installation SET Document_Path = @Document_Path  where Installation_ID = @Installation_ID";

        public static string UpdateAditLocationSyncEnable = " UPDATE Location SET AditLocationSyncEnable = @AditLocationSyncEnable  where Service_Install_Id = @Service_Install_Id and Clinic_Number = @Clinic_Number and Location_ID = @Location_ID";

        public static string UpdatePozativeSyncEmailLoc = " UPDATE Service_Installation SET PozativeSync = @PozativeSync, PozativeEmail = @PozativeEmail, "
                                                        + " PozativeLocationID = @PozativeLocationID, PozativeLocationName = @PozativeLocationName  where Installation_ID = @Installation_Id";

        public static string Update_AditApptAutoBook = " UPDATE Service_Installation SET ApptAutoBook = @ApptAutoBook  where Installation_ID = @Installation_ID";

        public static string UpdateLocationTimeZoneWithSystemTimeZone = " UPDATE Service_Installation SET timezone = @timezone where Installation_ID = @Installation_ID";

        public static string UpdateEHRApplication_Version_Installation = " UPDATE Service_Installation SET Application_Version = @Application_Version where Installation_ID = @Installation_ID ";


        #endregion

        #region Adit Configuration Sync Time

        public static string GetAditModuleSyncTime = " SELECT SyncModule_ID, SyncModule_Name, SyncModule_Pull, SyncModule_Push, SyncModule_EHR, SyncDateTime "
                                                   + " FROM SyncModule WITH(NOLOCK) ";

        public static string GetService_Installation = "Select Installation_ID, Organization_ID, Location_ID, Application_Name, Application_Version, System_Name, System_processorID, Hostname, IntegrationKey, UserId, Password, Port, WebAdminUserToken, timezone, IS_Install, Installation_Date, Installation_Modify_Date, AditSync, PozativeSync, ApptAutoBook, PozativeEmail, PozativeLocationID, PozativeLocationName, DBConnString, Document_Path, Windows_Service_Version, ApplicationIdleTimeOff, AppIdleStartTime, AppIdleStopTime, ApplicationInstalledTime, EHR_Sub_Version, DontAskPasswordOnSaveSetting, NotAllowToChangeSystemDateFormat "
                                                   + " FROM Service_Installation WITH(NOLOCK) ";

        public static string DeleteAditModuleSyncTimeDetail = " DELETE FROM SyncModule WHERE SyncModule_ID = SyncModule_ID";

        public static string InsertAditModuleSyncTimeDetailDetail = " INSERT INTO SyncModule "
                                           + " (SyncModule_Name, SyncModule_Pull, SyncModule_Push, SyncModule_EHR, Last_Update_Date) "
                                    + " VALUES (@SyncModule_Name, @SyncModule_Pull, @SyncModule_Push, @SyncModule_EHR, @Last_Update_Date) ";

        public static string UpdateAditModuleSyncIdleTimeDetail = " Update Service_Installation set ApplicationIdleTimeOff = @ApplicationIdleTimeOff , AppIdleStartTime = @AppIdleStartTime , AppIdleStopTime = @AppIdleStopTime  ";


        public static string CheckTableExistsInDatabase = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TABLE_NAME";

        public static string CheckTableSyncModule = " CREATE TABLE [SyncModule] "
                                                  + " ([SyncModule_ID] BIGINT NOT NULL IDENTITY (1,1), "
                                                  + " [SyncModule_Name] NVARCHAR(200), "
                                                  + " [SyncModule_Pull] BIGINT, "
                                                  + " [SyncModule_Push] BIGINT, "
                                                  + " [SyncModule_EHR] BIGINT, "
                                                  + " [Last_Update_Date] DATETIME);";


        public static string GetColumnsNameFromTable = " select * from information_schema.columns where table_name = 'tablename'";

        #endregion

        #region Application Auto Update

        public static string Insert_ApplicationUpdateServerDate = "Insert Into Client_Location_Update "
                                        + " ( Organization_Web_ID, Org_Name, User_Web_ID, Location_Web_Id, Appointment_Location_Web_Id, "
                                        + " Location_Name, Location_Server_App_Version, Last_Update_Date, "
                                        + " EHR_Name, EHR_Version,"
                                        + " System_Name,Operating_System,Processor_Name,Service_Pack,Total_RAM,Available_RAM,Total_HDisk,Available_HDisk,DotNetFrameWork,System_Type ) "
                                        + " Values ( @Organization_Web_ID, @Org_Name, @User_Web_ID, @Location_Web_Id, @Appointment_Location_Web_Id, "
                                        + " @Location_Name, @Location_Server_App_Version, @Last_Update_Date,  "
                                        + " @EHR_Name, @EHR_Version, "
                                        + " @System_Name,@Operating_System,@Processor_Name,@Service_Pack,@Total_RAM,@Available_RAM,@Total_HDisk,@Available_HDisk,@DotNetFrameWork,@System_Type ) ";

        public static string Update_ApplicationUpdateServerDate = "Update Client_Location_Update SET "
                                        + " Organization_Web_ID = @Organization_Web_ID, Org_Name = @Org_Name, User_Web_ID = @User_Web_ID, "
                                        + " Appointment_Location_Web_Id =@Appointment_Location_Web_Id, Location_Name = @Location_Name, "
                                        + " Location_Server_App_Version = @Location_Server_App_Version, "
                                        + " Last_Update_Date = @Last_Update_Date, EHR_Name = @EHR_Name, EHR_Version = @EHR_Version, "
                                        + "  System_Name = @System_Name, "
                                        + " Operating_System =@Operating_System, "
                                        + " Processor_Name = @Processor_Name, "
                                        + " Service_Pack =@Service_Pack, "
                                        + " Total_RAM =@Total_RAM, "
                                        + " Available_RAM=@Available_RAM, "
                                        + " Total_HDisk =@Total_HDisk, "
                                        + " Available_HDisk =@Available_HDisk, "
                                        + " DotNetFrameWork =@DotNetFrameWork, "
                                        + " System_Type =@System_Type "
                                        + " WHERE Location_Web_Id = @Location_Web_Id ";

        public static string Update_AppVersionOnPozative_Server_App = "Update Client_Location_Update Set "
                                        + " Location_Server_App_Version = @Location_Server_App_Version,  "
                                        + " Is_Auto_Update = @Is_Auto_Update, Last_Update_Date = @Last_Update_Date  "
                                        + "  Where Location_Web_ID = @Location_Web_ID ";

        #endregion
        public static string UpdateSingleFieldInTable = " UPDATE TableName SET FieldName = @UpdateValue WhereCondition";

    }
}
